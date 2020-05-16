using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DSTEd.Core.IO.EnumerableFileSystem
{
	/// <summary>
	/// Repersents a Windows path
	/// </summary>
	[Serializable]
	public class Path :ISerializable
	{
		//private members
		private string original;

		//props

		/// <summary>
		/// Gets the extension.
		/// </summary>
		/// <remarks>a file named like ".hidden" will treated as no extension exists</remarks>
		public string Extension { get; private set; }

		/// <summary>Get Filename, this property will be string.Empty in case of a path like "/p/"</summary>
		/// <remarks></remarks>
		public string FileName { get; private set; }

		/// <summary>
		/// Get parent path
		/// </summary>
		public string Parent { get; private set; }

		//ctor
		/// <summary>
		/// Initalize an instance of path class by a source path string
		/// </summary>
		/// <param name="source">Path string</param>
		/// <exception cref="ArgumentNullException">source is null</exception>
		public Path(string source)
		{
			Initialize(source);
		}


		private void Initialize(string source)
		{
			if (source == null)
				throw new ArgumentNullException(source, nameof(source));
			original = source.Replace('/', '\\');
			//extension
			if (original != string.Empty) 
			{
				string[] parts = original.Split('\\');
				FileName = parts.Last();

				//pt is point.
				int pt_index = FileName.Length == 0 ? 0 : FileName.Length - 1;
				for (; pt_index > 0; pt_index--)
				{
					if (FileName[pt_index] == '.')
						break;
				}
				Extension = FileName.Substring(pt_index);

				{
					int parent_len = 0;
					for (int i = 0; i < original.Length; i++)
						if (original[i] == '\\')
							parent_len = i;
					Parent = original.Substring(0, parent_len);
				}
			}
			else
			{
				Extension = string.Empty;
				FileName = string.Empty;
				Parent = string.Empty;
			}
		}

		/// <summary>
		/// Get relative path
		/// </summary>
		/// <param name="path1"></param>
		/// <param name="path2"></param>
		/// <returns></returns>
		public static Path RelativePath(Path path1, Path path2)
		{
			string[] b = path1.original.Replace('/', '\\').Split('\\');
			string[] p = path2.original.Replace('/', '\\').Split('\\');
			string relative_path;

			//check if driver are the same. e.g. "C:" and "D:" does not same, in this case they can't be relative
			if ((b[0][1] == ':'  )&& (b[0] != p[0]))
				throw new ArgumentException(string.Format("base path's driver \"{0}\" doesn't same as path's\"{1}\"", b[0], p[0]));

			IEnumerator<string>
			it_base = ((IEnumerable<string>)b).GetEnumerator(),
			it_path = ((IEnumerable<string>)p).GetEnumerator();
			{
				bool b_end = true, p_end = true;

				//find same part by "foreach"
				do
				{
					b_end = it_base.MoveNext();
					p_end = it_path.MoveNext();

					//no difference until one collection end
					if (!b_end)
					{
						relative_path = "\\" + it_path.Current;
						while (it_path.MoveNext())
						{
							relative_path += '\\' + it_path.Current;
						}
						return new Path(relative_path);
					}
					if (!p_end)
					{
						relative_path = "\\..";
						while (it_base.MoveNext())
						{
							relative_path += "\\..";
						}
						return new Path(relative_path);
					}

					//find difference before ends
					if (it_base.Current != it_path.Current)
					{
						StringBuilder rel_base = new StringBuilder("..\\");
						StringBuilder rel_path = new StringBuilder(it_path.Current);

						Action[] build_relative =
						{
							()=>
							{
								while (it_base.MoveNext())
								{
									rel_base.Append("..\\");
								}
							},//base
							()=>
							{
								while (it_path.MoveNext())
								{
									rel_path.Append('\\').Append(it_path.Current);
								}
							}//path
						};
						System.Threading.Tasks.Parallel.Invoke(build_relative);

						return new Path(rel_base.Append(rel_path).ToString());
					}
				} while (b_end && p_end);
			}

			throw new Exception(string.Format("Critical error happened, base:{0}," +
				"path:{1}", path1.original, path2.original));
		}

		/// <summary>
		/// Get absulote
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="child"></param>
		/// <returns></returns>
		public static Path Canonical(Path parent, Path child)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Append another path to this path. 
		/// </summary>
		/// <param name="r">Another path</param>
		/// <returns></returns>
		public Path Append(Path r)
		{
			original = Regex.Replace(original + r.original, "\\+", "\\");
			return this;
		}

		#region Serialization

		#pragma warning disable CS1591
		public void GetObjectData(SerializationInfo info, StreamingContext context)
#pragma warning restore CS1591
		{
			info.AddValue("original", original);
		}

		#pragma warning disable CS1591
		public Path(SerializationInfo info, StreamingContext context)
#pragma warning restore CS1591
		{
			string original = info.GetString("original");
			Initialize(original);
		}

		#endregion

		/// <summary>
		/// Append Right path to Left Path then return a new path
		/// </summary>
		/// <param name="l">Left</param>
		/// <param name="r">Right</param>
		/// <returns>A new path</returns>
		public static Path operator /(Path l, Path r)
		{
			string ret_value = Regex.Replace(l.original + r.original, "\\+", "\\");
			return new Path(ret_value);
		}

		//private methods

	}
}
