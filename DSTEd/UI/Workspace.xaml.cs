using System;
using System.ComponentModel;
using System.Windows;

namespace DSTEd.UI {
    public partial class Workspace : Window {
        private Action<CancelEventArgs> callback_close = null;

        public Workspace() {
            InitializeComponent();
            Closing += OnWindowClosing;
        }

        public void OnClose(Action<CancelEventArgs> callback) {
            this.callback_close = callback;
        }

        public void OnWindowClosing(object sender, CancelEventArgs e) {
            callback_close?.Invoke(e);
        }
    }
}
