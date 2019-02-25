using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace DSTEd.Core.Steam {
    public class Account {
        private Steam steam;
        private Boolean logged_in = false;

        public Account(Steam steam) {
            this.steam = steam;
        }
    }
}
