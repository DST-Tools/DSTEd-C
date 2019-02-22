using System.IO;
using System.Windows.Controls;
using DSTEd.Core;
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
}
