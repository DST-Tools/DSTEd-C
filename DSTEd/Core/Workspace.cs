using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using DSTEd.Core;

namespace DSTEd.Core {
    class Workspace {
        private UI.Workspace window;
        private string path = "C:\\Program Files\\";

        public Workspace() {
            this.window = new UI.Workspace();
        }

        public void Show() {
            this.window.Show();
        }

        public void Close() {
            this.window.Close(false);
        }

        public void Close(Boolean ignore_callback) {
            this.window.Close(ignore_callback);
        }

        public void OnClose(Action<CancelEventArgs> callback) {
            this.window.OnClose(callback);
        }

        public void OnSelect(Action<string, Boolean> callback) {
            this.window.OnSelect(callback);
        }

        public void SetPath(string path) {
            this.path = path;
        }

        public string GetPath() {
            return this.path;
        }
    }
}
