using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DSTEd.UI
{
	/// <summary>
	/// ProjectWizard.xaml 的交互逻辑
	/// </summary>
	public partial class ProjectWizard : Window
	{
		//TODO: ProjectWizard UI
		static Microsoft.VisualBasic.Devices.Computer computer = new Microsoft.VisualBasic.Devices.Computer();
		static private DirectoryInfo[] TemplateList = new DirectoryInfo(".\\Project Templates").GetDirectories();
		public ProjectWizard()
		{
			InitializeComponent();
		}
		/// <summary>
		/// Create a new Project from a specified template
		/// </summary>
		/// <param name="Template">Use ParseTemplate() in this class</param>
		/// <param name="TargetPath">Where to copy into</param>
		public void CreateNewFromTemplate(DirectoryInfo Template, string TargetPath, string ModName)
		{
			var target = Directory.CreateDirectory(TargetPath);
			int count = cp_ren(Template, target, ModName);
			Console.WriteLine("复制了{0}个文件", count);
		}
		/// <summary>
		/// Parses Template Name into DirectoryInfo
		/// </summary>
		/// <returns>Returns a DirectoryInfo, From TemplateList</returns>
		private DirectoryInfo ParseTemplate(string TemplateName)
		{
			foreach (DirectoryInfo template in TemplateList)
				if (template.Name.ToLower() == TemplateName.ToLower())
					return template;
			throw new DirectoryNotFoundException("No such template");
		}

		public void RefreshTemplateList()
		{
			TemplateList = new DirectoryInfo(".\\Project Templates").GetDirectories();
		}
		/// <summary>
		/// Copy and replace specified filename/file content
		/// </summary>
		/// <param name="Replaces">Replace "__NAME" in filename or directory name,
		/// and replace "&lt;NAME&gt;" in Lua and SCML file content</param>
		/// <returns>Returns the count of copied file</returns>
		private static int cp_ren(DirectoryInfo src, DirectoryInfo dest, string Replaces)//recursive copy!!
		{
			var srcfileref = src.GetFiles();
			var srcdirref = src.GetDirectories();
			int ret = srcfileref.Length;
			foreach (FileInfo file in srcfileref)
			{
				Directory.CreateDirectory(dest.FullName);
				string newname = file.Name;
				if (file.Name.Contains("__NAME"))
					newname = file.Name.Replace("__NAME", Replaces);
				var f2 = file.CopyTo(dest.FullName + '\\' + newname, true);
				if (f2.Extension.ToLower() == ".lua" || f2.Extension.ToLower() == ".scml")
				{
					byte[] buffier = File.ReadAllBytes(f2.FullName);
					FileStream fs = new FileStream(f2.FullName, FileMode.Truncate);
					var replaceout = Encoding.UTF8.GetString(buffier).Replace(@"<NAME>", Replaces);
					buffier = Encoding.UTF8.GetBytes(replaceout);
					fs.Write(buffier, 0, buffier.Length);
				}
			}
			foreach (DirectoryInfo directory in srcdirref)
			{
				string destname = dest.FullName + '\\' + directory.Name;
				var d2 = new DirectoryInfo(destname);
				ret += cp_ren(directory, d2, Replaces);
				if (destname.Contains("__NAME"))
					computer.FileSystem.RenameDirectory(d2.FullName, directory.Name.Replace("__NAME", Replaces));
			}
			return ret;
		}
	}
}
