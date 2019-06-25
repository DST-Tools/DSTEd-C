using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DSTEd.UI.Components
{
	public class WorkspaceFSItem : TreeViewItem
	{
		public string FullPath { get; protected set; }

		public WorkspaceFSItem(string FullPath) :base()
		{
			
			this.FullPath = FullPath;
			base.PreviewMouseRightButtonDown += trace;
		}

		private void trace(object sender,MouseButtonEventArgs args)
		{
			Console.WriteLine("context menu:" + FullPath);
		}
	}
}
