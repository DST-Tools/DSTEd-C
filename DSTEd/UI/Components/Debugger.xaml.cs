using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace DSTEd.UI.Components {
    public partial class Debugger : LayoutAnchorablePane {
        
        public Debugger() {
            InitializeComponent();
            this.DockMinHeight = 200;
        }

        public StackPanel GetDebugger() {
            return this.debugger_debug;
        }

        public StackPanel GetOutput() {
            return this.debugger_output;
        }

        public StackPanel GetErrors() {
            return this.debugger_errors;
        }
    }
}
