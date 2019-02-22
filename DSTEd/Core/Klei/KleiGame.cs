using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using DSTEd.Core.IO;

namespace DSTEd.Core.Klei {
    public class KleiGame {
        protected int id = -1;
        private DSTEd core = null;
        protected string name = null;
        protected string path = null;
        protected string executable = null;
        private Boolean is_main = false;
        private FileSystem files = null;

        public KleiGame(DSTEd core) {
            this.core = core;
        }

        public DSTEd GetCore() {
            return this.core;
        }

        public string GetName() {
            return this.name;
        }

        public int GetID() {
            return this.id;
        }

        public Boolean IsMainGame() {
            return this.is_main;
        }

        public void SetMainGame() {
            this.is_main = true;
        }

        public void SetPath(string path) {
            this.path = path;
            this.Update();
        }

        private void Update() {
            this.files = new FileSystem(this.GetPath());
        }

        public FileSystem GetFiles() {
            return this.files;
        }

        public string GetPath() {
            return this.path;
        }

        public string GetExecutable() {
            return this.executable;
        }

        public void AddTool(string name, string executable) {
            MenuItem item = new MenuItem();
            item.Header = I18N.__(name);
            item.Click += new RoutedEventHandler(delegate (object sender, RoutedEventArgs e) {
                try {
                    Process.Start(this.GetPath() + "/" + executable);
                } catch {
                    Logger.Error("Can't open executable: " + this.GetPath() + "/" + executable);
                }
            });

            MenuItem tools = this.GetCore().GetIDE().GetTools();
            tools.Items.Add(item);
        }
    }
}
