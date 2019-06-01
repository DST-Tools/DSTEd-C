using DSTEd.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace DSTEd.UI.Components
{
	class WorkspaceFolderItem : TreeViewItem
	{
		public string FullPath { get; private set; }
		protected FileNode file = null;

		public WorkspaceFolderItem(FileNode file) : base()
		{
			this.file = file;
			Header = file.GetName();
		}
	}
}
