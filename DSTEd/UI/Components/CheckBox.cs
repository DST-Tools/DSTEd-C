using System.Windows;

namespace DSTEd.UI.Components {
    public class CheckBox : System.Windows.Controls.CheckBox {
        private bool is_checked = false;

        public CheckBox() : base() {
            this.is_checked = (bool) this.IsChecked;

            this.Checked += new RoutedEventHandler(delegate (object sender, RoutedEventArgs e) {
                this.is_checked = true;
            });
            
            this.Unchecked += new RoutedEventHandler(delegate (object sender, RoutedEventArgs e) {
                this.is_checked = false;
            });
        }

        public bool IsChecking() {
            return this.is_checked;
        }
    }
}
