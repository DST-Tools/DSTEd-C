using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
		/// <exception cref="ArgumentException">source is an empty string or null</exception>
		public Path(string source)
		{
			Initialize(source);
		}

		private void Initialize(string source)
		{
			if (source == null || source == string.Empty)
				throw new ArgumentException(source, nameof(source));
			original = source.Replace('/', '\\');
			string[] parts = original.Split('\\');
			FileName = parts.Last();
			//extension
			{
				int pt_index = FileName.Length - 1;
				for (; pt_index > 0; pt_index--)
				{
					if (FileName[pt_index] == '.')
						break;
				}
				Extension = FileName.Substring(pt_index);
			}
		}

		/// <summary>
		/// Get relative path
		/// </summary>
		/// <param name="L"></param>
		/// <param name="R"></param>
		/// <returns></returns>
		public static Path RelativePath(Path L, Path R)
		{
			throw new NotImplementedException();
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

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
		public void GetObjectData(SerializationInfo info, StreamingContext context)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
		{
			info.AddValue("original", original);
		}

		/// <summary>
		/// Appends Right path to Left Path
		/// </summary>
		/// <param name="l">Left</param>
		/// <param name="r">Right</param>
		/// <returns></returns>
		public static Path operator /(Path l, Path r)
		{
			string ret_value = Regex.Replace(l.original + r.original, "\\+", "\\");
			return new Path(ret_value);
		}

		//private methods

	}
}
