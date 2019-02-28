
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace DSTEd.UI {
    public partial class About : Window {
        public About() {
            InitializeComponent();

            // @ToDo insert system informations
            Label version = new Label();
            version.Content = "Version: " + Assembly.GetExecutingAssembly().GetName().Version;
            this.system.Children.Add(version);
        }

        private void OnOK(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
