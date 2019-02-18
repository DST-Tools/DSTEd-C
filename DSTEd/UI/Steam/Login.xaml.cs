using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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