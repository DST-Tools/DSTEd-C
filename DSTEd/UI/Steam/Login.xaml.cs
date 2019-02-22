using System;
using System.Windows;
using DSTEd.Core;

namespace DSTEd.UI.Steam {
    /// <summary>
    /// Interaktionslogik für Login.xaml
    /// </summary>
    public partial class Login : Window {
        private Core.DSTEd core = null;

        public Login(Core.DSTEd core) {
            InitializeComponent();
            this.core = core;
        }

        private void OnCancel(object sender, RoutedEventArgs e) {
            this.Hide();
        }

        private void OnLogin(object sender, RoutedEventArgs e) {
            this.core.GetSteam().GetAccount().Login(this.username.Text, this.password.Text, delegate (string email, Action<string> callback) {

                new Guard(email, delegate (string code) {
                    callback(code);
                }).ShowDialog();

            }, delegate (string error, Boolean logged_in) {
                Logger.Info("LOGIN", error, logged_in);
            });
        }
    }
}