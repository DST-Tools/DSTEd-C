using System;
using System.Globalization;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using DSTEd.Core.IO;

namespace DSTEd.UI.Components {
    public partial class WorkspaceTree : UserControl {
        public WorkspaceTree(FileSystem files) {
            InitializeComponent();

            this.tree.Items.Clear();

            foreach (FileNode directory in files.GetDirectories()) {
                this.Render(directory, this.tree);
            }
        }

        private TreeViewItem Render(FileNode files, TreeView container) {
            TreeViewItem item = new TreeViewItem { Header = files.GetName() };

            if (files.HasSubdirectories()) {
                foreach (FileNode d in files.GetSubdirectories()) {
                    TreeViewItem root = this.Render(d, null);

                    if (container != null) {
                        container.Items.Add(root);
                    } else {
                        item.Items.Add(root);
                    }
                }
            }

            if (files.HasFiles()) {
                foreach (FileInfo d in files.GetFiles()) {
                    var tt = new TreeViewItem { Header = d.Name };

                    item.Items.Add(tt);
                }
            }

            return item;
        }
    }

    [ValueConversion(typeof(string), typeof(bool))]
    public class HeaderToImageConverter : IValueConverter {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();

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
            throw new NotSupportedException("Cannot convert back");
        }
    }
}
