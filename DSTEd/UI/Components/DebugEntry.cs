using System.Windows.Controls;

namespace DSTEd.UI.Components {
    class DebugEntry : TextBlock {
        public DebugEntry(string content) {
            this.Text = content;
            this.Padding = new System.Windows.Thickness(5, 2, 5, 2);
        }
    }
}
