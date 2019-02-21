using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace DSTEd.Core.Klei {
    public class KleiGame {
        protected int id = -1;
        private DSTEd core = null;
        protected string name = null;
        protected string path = null;
        protected string executable = null;
        protected List<string> files = new List<string>();

        public KleiGame(DSTEd core) {
            this.core = core;
        }

        public DSTEd GetCore() {
            return this.core;
        }

        public string GetName() {
            return this.name;
        }

        private IEnumerable<string> Load(string path) {
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(path);

            while (queue.Count > 0) {
                path = queue.Dequeue();

                try {
                    foreach (string subDir in Directory.GetDirectories(path)) {
                        queue.Enqueue(subDir);
                    }
                } catch (Exception ex) {
                    Logger.Error(ex);
                }

                string[] files = null;

                try {
                    files = Directory.GetFiles(path);
                } catch (Exception ex) {
                    Logger.Error(ex);
                }

                if (files != null) {
                    for (int i = 0; i < files.Length; i++) {
                        yield return files[i];
                    }
                }
            }
        }

        public List<string> GetFiles() {
            return this.files;
        }

        public int GetID() {
            return this.id;
        }

        public void SetPath(string path) {
            this.path = path;

            files.Clear();

            foreach (string file in this.Load(this.path)) {
                files.Add(file);
            }
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
