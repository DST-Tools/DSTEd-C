using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DSTEd.Core;
using DSTEd.Core.LUA;

namespace DSTEd.UI
{
	public partial class ProjectWizard : Window
	{
		//TODO: ProjectWizard UI
		static Microsoft.VisualBasic.Devices.Computer computer = new Microsoft.VisualBasic.Devices.Computer();
		static private DirectoryInfo[] TemplateList = new DirectoryInfo(".\\Project Templates").GetDirectories();
		public ProjectWizard()
		{
			InitializeComponent();

			InitializeModInfo();
			InitializeModOption();
			InitializeModType();
		}

		public void InitializeModInfo()
		{
			Core.Contents.Editors.Properties properties = new Core.Contents.Editors.Properties();

			// load defaut modinfo
			string modinfo;

			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DSTEd.Project_Templates.BaseMod.modinfo.lua"))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					modinfo = reader.ReadToEnd();
				}
			}
			Core.Klei.Data.ModInfo info = Boot.Core.LUA.GetModInfo(modinfo, delegate (ParserException e) {
				Logger.Info("[ProjectWizard ModInfo] " + e);
			});

			if (info.IsBroken())
			{
				return;
			}

			// Build Properties-Editor
			properties.AddCategory(I18N.__("Informations"));
			properties.AddEntry("name", I18N.__("Name"), Core.Contents.Editors.Properties.Type.STRING, info.GetName());
			properties.AddEntry("version", I18N.__("Version"), Core.Contents.Editors.Properties.Type.STRING, info.GetVersion());
			properties.AddEntry("description", I18N.__("Description"), Core.Contents.Editors.Properties.Type.TEXT, info.GetDescription());
			properties.AddEntry("author", I18N.__("Author"), Core.Contents.Editors.Properties.Type.STRING, info.GetAuthor());
			properties.AddEntry("forumthread", I18N.__("Forum-Thread"), Core.Contents.Editors.Properties.Type.URL, info.GetForumThread());
			properties.AddEntry("server_filter_tags", I18N.__("Tags"), Core.Contents.Editors.Properties.Type.STRINGLIST, info.GetTags());

			properties.AddCategory(I18N.__("Assets"));
			properties.AddEntry("icon_atlas", I18N.__("Icon (Atlas)"), Core.Contents.Editors.Properties.Type.ATLAS, info.GetIconAtlas());
			properties.AddEntry("icon", I18N.__("Icon (Texture)"), Core.Contents.Editors.Properties.Type.KTEX, info.GetIcon());

			properties.AddCategory(I18N.__("System"));
			properties.AddEntry("priority", I18N.__("Priority"), Core.Contents.Editors.Properties.Type.NUMBER, info.GetPriority());
			properties.AddEntry("api_version", I18N.__("API Version"), Core.Contents.Editors.Properties.Type.SELECTION, new Core.Contents.Editors.Selection(new Dictionary<object, string> {
				   { 6, "Don't Starve" },
				   { 10, "Don't Starve Together" }
				}, info.GetAPIVersion()));

			// Only add outdated dst_api_version if it's exists on the modinfo.
			if (info.GetDSTAPIVersion() > 0)
			{
				properties.AddEntry("dst_api_version", I18N.__("API Version (DST)"), Core.Contents.Editors.Properties.Type.SELECTION, new Core.Contents.Editors.Selection(new Dictionary<object, string> {
					   { 6, "6" },
					   { 10, "10" }
					}, info.GetDSTAPIVersion()));
			}

			properties.AddCategory(I18N.__("Compatiblity"));
			properties.AddEntry("dont_starve_compatible", I18N.__("Don't Starve"), Core.Contents.Editors.Properties.Type.YESNO, info.IsDS());
			properties.AddEntry("dst_compatible", I18N.__("Don't Starve Together"), Core.Contents.Editors.Properties.Type.YESNO, info.IsDST());
			properties.AddEntry("reign_of_giants_compatible", I18N.__("Reign of Giants"), Core.Contents.Editors.Properties.Type.YESNO, info.IsROG());
			properties.AddEntry("shipwrecked_compatible", I18N.__("Shipwrecked"), Core.Contents.Editors.Properties.Type.YESNO, info.IsSW());

			properties.AddCategory(I18N.__("Requirements"));
			properties.AddEntry("standalone", I18N.__("Standalone"), Core.Contents.Editors.Properties.Type.YESNO, info.ModsAllowed());
			properties.AddEntry("client_only_mod", I18N.__("Only Client"), Core.Contents.Editors.Properties.Type.YESNO, info.IsOnlyClient());
			properties.AddEntry("all_clients_require_mod", I18N.__("All Clients Required"), Core.Contents.Editors.Properties.Type.YESNO, info.IsRequired());
			properties.AddEntry("restart_require", I18N.__("Restart Required"), Core.Contents.Editors.Properties.Type.YESNO, info.MustRestart());
			
			this.modinfo.Content = properties;
		}

		public void InitializeModOption()
		{
			Core.Contents.Editors.Properties properties = new Core.Contents.Editors.Properties();

			// load defaut modinfo
			string modinfo;

			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DSTEd.Project_Templates.BaseMod.modinfo.lua"))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					modinfo = reader.ReadToEnd();
				}
			}

			Core.Klei.Data.ModInfo info = Boot.Core.LUA.GetModInfo(modinfo, delegate (ParserException e) {
				Logger.Info("[ProjectWizard ModInfo] " + e);
			});

			if (info.IsBroken())
			{
				return;
			}

			if (info.GetOptions() != null)
			{
				foreach (Core.Klei.Data.Options entry in info.GetOptions())
				{
					properties.AddCategory("");
					properties.AddEntry("name", I18N.__("Name"), Core.Contents.Editors.Properties.Type.STRING, entry.GetName());
					properties.AddEntry("label", I18N.__("Label"), Core.Contents.Editors.Properties.Type.STRING, entry.GetLabel());
					if (!(entry.GetHover() == null))
					{
						properties.AddEntry("hover", I18N.__("Hover"), Core.Contents.Editors.Properties.Type.STRING, entry.GetHover());
					}
					if (!(entry.GetLongLabel() == null))
					{
						properties.AddEntry("longlabel", I18N.__("LongLabel"), Core.Contents.Editors.Properties.Type.STRING, entry.GetLongLabel());
					}

					if (entry.GetDefaults() == null)
					{
						properties.AddEntry("default", I18N.__("Default"), Core.Contents.Editors.Properties.Type.STRING, "NULL");
					}
					else if (entry.GetDefaults().GetType() == typeof(string))
					{
						properties.AddEntry("default", I18N.__("Default"), Core.Contents.Editors.Properties.Type.STRING, (string)entry.GetDefaults());
					}
					else if (entry.GetDefaults().GetType() == typeof(String))
					{
						properties.AddEntry("default", I18N.__("Default"), Core.Contents.Editors.Properties.Type.STRING, (String)entry.GetDefaults());
					}
					else if (entry.GetDefaults().GetType() == typeof(bool))
					{
						properties.AddEntry("default", I18N.__("Default"), Core.Contents.Editors.Properties.Type.YESNO, (bool)entry.GetDefaults());
					}
					else if (entry.GetDefaults().GetType() == typeof(Boolean))
					{
						properties.AddEntry("default", I18N.__("Default"), Core.Contents.Editors.Properties.Type.YESNO, (Boolean)entry.GetDefaults());
					}
					else if (entry.GetDefaults().GetType() == typeof(int))
					{
						properties.AddEntry("default", I18N.__("Default"), Core.Contents.Editors.Properties.Type.NUMBER, (int)entry.GetDefaults());
					}
					else if (entry.GetDefaults().GetType() == typeof(double))
					{
						properties.AddEntry("default", I18N.__("Default"), Core.Contents.Editors.Properties.Type.NUMBER, (double)entry.GetDefaults());
					}
					else if (entry.GetDefaults().GetType() == typeof(float))
					{
						properties.AddEntry("default", I18N.__("Default"), Core.Contents.Editors.Properties.Type.NUMBER, (float)entry.GetDefaults());
					}
					else
					{
						properties.AddEntry("default", I18N.__("Default"), Core.Contents.Editors.Properties.Type.STRING, entry.GetDefaults());
					}

					this.modoptions.Children.Add(properties);
				}
			}
		}

		public void InitializeModType()
		{
			//do something init
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
