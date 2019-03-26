using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTEd.UI.Components
{
	class CompletionWindow:ICSharpCode.AvalonEdit.CodeCompletion.CompletionWindow
	{
		CompletionWindow(TextArea ta):base(ta)
		{

		}
		protected override void OnClosing(System.ComponentModel.CancelEventArgs arg)
		{
			arg.Cancel = true;
			Hide();
		}
	}
}
