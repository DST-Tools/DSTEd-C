using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DSTEd.Core;

namespace DSTEd.UI.Components {
    class DebugEntry : TextBlock {
        private UI.Components.ContextMenu context = new UI.Components.ContextMenu();

        public DebugEntry(string content) {
            this.Text = content;
            this.Padding = new Thickness(5, 2, 5, 2);
            this.ContextMenu = context;
            

            context.Add(I18N.__("Copy"), delegate () {
                Clipboard.SetText(this.Text);
            });

            this.IsMouseDirectlyOverChanged += new DependencyPropertyChangedEventHandler(delegate (object sender, DependencyPropertyChangedEventArgs e) {
                if (this.IsMouseOver) {
                    this.Background = new SolidColorBrush(Color.FromArgb(100, 237, 92, 45));
                } else {
                    this.Background = new SolidColorBrush(Colors.Transparent);
                }
            });
        }
    }
}
