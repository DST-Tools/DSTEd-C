using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DSTEd.UI.Components
{
	public class WorkspaceFSItem : TreeViewItem
	{
		public string FullPath { get; protected set; }

		public WorkspaceFSItem(string FullPath) :base()
		{
			this.FullPath = FullPath;
		}
	}
}
