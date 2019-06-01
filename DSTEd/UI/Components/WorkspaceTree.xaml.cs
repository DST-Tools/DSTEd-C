using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DSTEd.Core;
using DSTEd.Core.IO;

namespace DSTEd.UI.Components {
	public partial class WorkspaceTree : UserControl
	{
		private Func<FileNode, TreeViewItem> callback = null;//maybe someday will use, keep

		public WorkspaceTree(FileSystem files, Func<FileNode, TreeViewItem> callback)
		{
			InitializeComponent();

			this.callback = callback;
			this.tree.Items.Clear();

			files.GetDirectories(delegate (List<FileNode> f)
			{
				foreach (FileNode directory in f)
				{
					Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(delegate ()
					{
						this.RenderV2(directory, this.tree);
					}));
				}
			});
		}

		private TreeViewItem RenderV2(FileNode files, TreeView container)
		{
			TreeViewItem item;

			//workshop mods
			if(files.GetName().StartsWith("workshop-"))
			{
				item = new WorkshopItem(files);
			}
			else
			{
				//initalize as a normal mod.
				item = new TreeViewItem()
				{
					Header = files.GetName(),
					FontWeight = FontWeights.Normal
				};
			}

			if(files.HasSubdirectories())
			{
				foreach (FileNode dir in files.GetSubdirectories())
				{
					TreeViewItem root = this.RenderV2(dir, null);
					root.FontWeight = FontWeights.Normal;

					if (container != null)
					{
						container.Items.Add(root);
					}
					else
					{
						item.Items.Add(root);
					}
				}
			}

			if(files.HasFiles())
			{
				List<string> skiplist = new List<string>(20);
				foreach (FileInfo file in files.GetFiles())
				{
					/* skip these files which added into skiplist, include bundled ktex(2019/5/30)
					 * put this first to avoid object construction
					 * this is the first check.
					 */
					if (skiplist.Contains(file.FullName)) continue;

					WorkspaceFileItem entry;

					//modinfo
					if(file.Name == "modinfo.lua")
					{
						//build
						entry = new ModInfoItem(file.FullName) { Header = "ModInfo" };
						item.Items.Add(entry);
						//skip other steps
						continue;
					}

					//texture bundle, read XML atlas first
					if (string.Compare(Path.GetExtension(file.Name), ".xml", true) == 0)
					{
						XmlDocument ktex_atlas = new XmlDocument();

						try
						{
							//create a bundle item
							entry = new TextureItem(file.FullName) { Header = Path.GetFileNameWithoutExtension(file.FullName) };
							ktex_atlas.Load(file.FullName);

							//read XML atlas to find out all the textures
							foreach (XmlNode texture in ktex_atlas.SelectSingleNode("Atlas").SelectNodes("Texture"))
							{
								//iterate attributes to find an attribute named "filename"
								foreach (XmlAttribute att in texture.Attributes)
								{
									if (att.LocalName != "filename")
										continue;
									string ktex_path = Path.Combine(file.DirectoryName, att.Value);
									//add it's path into skip list
									skiplist.Add(ktex_path);
									//build bundle, this is the texture step.
									WorkspaceFileItem ktex = new WorkspaceFileItem(ktex_path) { Header = att.Value };
									entry.Items.Add(ktex);
								}
							}

							//add XML itself into the bundle
							{
								TreeViewItem xml_atlas = new TreeViewItem { Header = file.Name };
								xml_atlas.PreviewMouseDown += new MouseButtonEventHandler(delegate (object s, MouseButtonEventArgs arg)
								  {
									  Boot.Instance.GetWorkspace().OpenDocument(file.FullName);
								  });
								entry.Items.Add(xml_atlas);
							}

							//set bundle mouse event handling
							entry.MouseRightButtonDown += new MouseButtonEventHandler(delegate (object sender, MouseButtonEventArgs e)
							{
								Logger.Info("ContextMenu: " + file.FullName);
							});
							entry.PreviewMouseDown += new MouseButtonEventHandler(delegate (object sender, MouseButtonEventArgs e)
							{
								// @ToDo open Texture-Editor
								Logger.Info("Texture-Editor: " + file.FullName);
								//this.GetCore().GetWorkspace().OpenDocument(file.FullName);
							});
						}
						catch (System.Xml.XPath.XPathException)
						{
							//use continue to avoid a wrong bundle being added into the tree by skipping the code above
							continue;
						}
						catch(XmlException)
						{
							//skip if the xml file does not contain any texture info
							continue;
						}
						catch(Exception e)
						{
							Console.WriteLine(e.Message);
							Console.WriteLine(e.StackTrace);
							//same as before
							continue;
						}


						item.Items.Add(entry);
						//skip other steps to make it faster
						continue;
					}

					//other files
					{
						entry = new WorkspaceFileItem(file.FullName)
						{
							Header = file.Name
						};
						item.Items.Add(entry);
					}
				}

				/* remove those item which shuold have skipped. 
				 * for example, there is 2 files named "a.tex" "a.xml"
				 * because "a.tex" being iterated earlier than "a.xml"
				 * "a.tex" will be added into Items before it had beed added into skiplist.
				 */
				foreach (WorkspaceFileItem item_to_check in item.Items)
				{
					if (skiplist.Contains(item_to_check.FullPath))
					{
						item.Items.Remove(item_to_check);
					}
				}
			}

			return item;
		}


	}

    [ValueConversion(typeof(string), typeof(bool))]
    public class Iconizer : IValueConverter {
        public static Iconizer Instance = new Iconizer();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string name = "";

            if ((value as string).EndsWith(".lua")) {
                name = "LUA";
            } else if ((value as string).EndsWith(".tex")) {
                name = "KTEX";
            } else if ((value as string).EndsWith(".xml")) {
                name = "XML";
            } else if ((value as string).EndsWith(".zip")) {
                name = "Archive";
            } else if ((value as string).EndsWith(".png") || (value as string).EndsWith(".bmp") || (value as string).EndsWith(".gif") || (value as string).EndsWith(".jpg") || (value as string).EndsWith(".jpeg") || (value as string).EndsWith(".tif")) {
                name = "Image";
            } else if ((value as string).EndsWith(".txt")) {
                name = "Text";
            } else if ((value as string).Contains(".")) {//this may cause some strange bug, if a folder named like ".gnupg" the folder will display as a unkown file
                name = "Unknown";
            } else {//if a file named like "hosts", it will sorts wrong, too.
                name = "Folder_Closed";
            }

            return new BitmapImage(new Uri("pack://application:,,,/DSTEd;component/Assets/FileSystem/" + name + ".png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            /* Do Nothing */

            return null;
        }
    }
}
