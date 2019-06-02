using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DSTEd.UI.Components
{
	public class WorkspaceFileItem : TreeViewItem
	{
		public string FullPath { get; private set; }

		private void open_document(object s, MouseEventArgs arg)
		{
			Boot.Instance.GetWorkspace().OpenDocument(FullPath);
		}

		public WorkspaceFileItem(string FullPath) : base()
		{
			this.FullPath = FullPath;

			PreviewMouseDoubleClick += open_document;
		}
	}
}
