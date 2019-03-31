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
		public ProjectWizard()
		{
			InitializeComponent();
		}
		/// <summary>
		/// Overwrite existed files
		/// </summary>
		/// <param name="TemplateName">Which template to use</param>
		/// <param name="TargetPath">Where to copy into</param>
		/// <param name="ModName">Replace "__NAME" in filename\n"&lt;NAME%gt;"in Lua and SCML file</param>
		public void CreateNewFromTemplate(string TemplateName, string TargetPath, string ModName)
		{
			var template = new DirectoryInfo("\\Project Templates\\" + TemplateName);
			var target = Directory.CreateDirectory(Boot.Core().GetWorkspace().GetPath() + '\\' + TargetPath);
			cp_x(template, target,ModName);
		}

		private int cp_x(DirectoryInfo src,DirectoryInfo dest, string Replaces)//recursive copy!!
		{
			var srcfileref = src.GetFiles();
			var srcdirref = src.GetDirectories();
			int ret = srcfileref.Length;
			foreach (FileInfo file in srcfileref)
			{
				var f2 = file.CopyTo(dest.FullName, true);
				computer.FileSystem.RenameFile(f2.FullName, f2.FullName.Replace("__NAME", Replaces));
				if(f2.Extension.ToLower() == ".lua" || f2.Extension.ToLower() == ".scml")
				{
					var fs = new FileStream(f2.FullName, FileMode.Open);
					byte[] buffier = { 0 };
					fs.Read(buffier, 0, (int)fs.Length);
					fs = new FileStream(f2.FullName, FileMode.Truncate);
					buffier = Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(buffier).Replace("<NAME>", Replaces));
					fs.Write(buffier, 0, buffier.Length);
				}
			}
			foreach (DirectoryInfo directory in srcdirref)
			{
				ret += cp_x(directory, dest,Replaces);
				string destname = dest.FullName + directory.Name;
				computer.FileSystem.RenameDirectory(destname, directory.Name.Replace("__NAME", Replaces));
			}
			return ret;
		}
	}
}
