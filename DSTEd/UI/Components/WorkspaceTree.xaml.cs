using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DSTEd.Core;
using DSTEd.Core.IO;

namespace DSTEd.UI.Components {
    public partial class WorkspaceTree : UserControl {
        private Core.DSTEd core = null;
        private Func<FileNode, TreeViewItem> callback = null;

        public WorkspaceTree(FileSystem files, Core.DSTEd core, Func<FileNode, TreeViewItem> callback) {
            InitializeComponent();

            this.callback = callback;
            this.core = core;
            this.tree.Items.Clear();

            foreach (FileNode directory in files.GetDirectories()) {
                this.Render(directory, this.tree);
            }
        }

        private TreeViewItem Render(FileNode files, TreeView container) {
            TreeViewItem item = new TreeViewItem { Header = files.GetName() };
            item.FontWeight = FontWeights.Normal;

            if (files.GetName().StartsWith("workshop-")) {
                item = callback?.Invoke(files);
            }

            if (files.HasSubdirectories()) {
                foreach (FileNode directory in files.GetSubdirectories()) {
                    TreeViewItem root = this.Render(directory, null);
                    root.FontWeight = FontWeights.Normal;

                    if (container != null) {
                        container.Items.Add(root);
                    } else {
                        item.Items.Add(root);
                    }
                }
            }

            if (files.HasFiles()) {
                foreach (FileInfo file in files.GetFiles()) {
                    TreeViewItem entry = new TreeViewItem { Header = file.Name };

                    if (file.Name == "modinfo.lua") {
                        entry = new ModInfoItem { Header = "ModInfo" }; // @ToDo need Translation(?)
                    }

                    entry.MouseRightButtonDown += new MouseButtonEventHandler(delegate (object sender, MouseButtonEventArgs e) {
                        Logger.Info("ContextMenu: " + file.FullName);
                    });

                    entry.PreviewMouseDown += new MouseButtonEventHandler(delegate (object sender, MouseButtonEventArgs e) {
                        this.GetCore().GetWorkspace().OpenDocument(file.FullName);
                    });

                    item.Items.Add(entry);
                }
            }

            return item;
        }

        public Core.DSTEd GetCore() {
            return this.core;
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
                name = "Bundle";
            } else if ((value as string).EndsWith(".xml")) {
                name = "XML";
            } else if ((value as string).EndsWith(".zip")) {
                name = "Archive";
            } else if ((value as string).EndsWith(".png")) {
                name = "Image";
            } else if ((value as string).EndsWith(".txt")) {
                name = "Text";
            } else if ((value as string).Contains(".")) {
                name = "Unknown";
            } else {
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
