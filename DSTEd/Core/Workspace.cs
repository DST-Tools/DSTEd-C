using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using DSTEd.Core;

namespace DSTEd.Core {
    class Workspace {
        private UI.Workspace window;

        public Workspace() {
            this.window = new UI.Workspace();
        }

        public void Show() {
            this.window.Show();
        }

        public void OnClose(Action<CancelEventArgs> callback) {
            this.window.OnClose(callback);
        }

        public void OnSelect(Action<string, Boolean> callback) {
            this.window.OnSelect(callback);
        }

        public void SetPath(string path) {

        }
    }
}
