using System;
using System.Collections.Generic;
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

        public void SetPath(string path) {

        }
    }
}
