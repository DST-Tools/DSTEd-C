using System;
using System.Windows;

namespace DSTEd.UI.Steam {
    public partial class Guard : Window {
        private Action<string> callback = null;

        public Guard(string email, Action<string> callback) {
            InitializeComponent();

            if (email == null) {
                this.Title = "SteamGuard App";
            } else {
                this.Title = "SteamGuard E-Mail (" + email + ")";
            }

            this.callback = callback;
        }

        private void OnCancel(object sender, RoutedEventArgs e) {
            this.Hide();
        }

        private void OnLogin(object sender, RoutedEventArgs e) {
            this.callback?.Invoke(this.code.Text);
            this.Hide();
        }
    }
}
