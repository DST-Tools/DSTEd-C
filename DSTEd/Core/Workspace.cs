using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace DSTEd.Core {
    public class Workspace {
        private UI.Workspace window;
        private string path = "C:\\Program Files\\";
        private Dictionary<string, Document> documents = null;
        private Document welcome = null;

        public Workspace() {
            this.window = new UI.Workspace();
            this.documents = new Dictionary<string, Document>();
            this.CreateWelcome();
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
            this.window.SetPath(this.path);
        }

        public string GetPath() {
            return this.path;
        }

        public void CreateWelcome() {
            this.welcome = new Document(Document.Editor.NONE);
            this.welcome.SetTitle(I18N.__("Welcome"));
            this.welcome.SetCloseable(false);
            this.AddDocument(this.welcome);
        }

        public Boolean HasWelcome() {
            Boolean existing = false;

            foreach (KeyValuePair<string, Document> entry in this.documents) {
                if (entry.Value == this.welcome) {
                    existing = true;
                    break;
                }
            }

            return existing;
        }

        public Boolean ToggleWelcome() {
            Boolean visible = false;

            if (this.HasWelcome()) {
                visible = false;
                this.RemoveDocument(this.welcome);
            } else {
                visible = true;
                this.AddDocument(this.welcome);
                Boot.Core().GetIDE().SetActiveDocument(this.welcome);
            }

            return visible;
        }

        public Document GetDocument(string path) {
            Document existing = null;

            foreach (KeyValuePair<string, Document> entry in this.documents) {
                if (entry.Key == path || entry.Value.GetFile() == path) {
                    existing = entry.Value;
                    break;
                }
            }

            return existing;
        }

        public void OpenDocument(string file) {
            if (this.ExistingDocument(file)) {
                this.ShowDocument(file);
                return;
            }

            Document.Editor type = Document.Editor.CODE;

            switch (Path.GetExtension(file)) {
                case ".tex":
                    type = Document.Editor.TEXTURE;
                    break;
            }

            if (file.EndsWith("modinfo.lua")) {
                type = Document.Editor.MODINFO;
            }

            Document document = new Document(type);
            document.Load(file);
            // @ToDo add to RecentFiles
            Boot.Core().GetWorkspace().AddDocument(document);
        }

        internal void ShowDocument(string file) {
            foreach (KeyValuePair<string, Document> entry in this.documents) {
                if (entry.Key == file || entry.Value.GetFile() == file) {
                    // @ToDo check content if its newer and ask for reloading...
                    Boot.Core().GetIDE().SetActiveDocument(entry.Value);
                }
            }
        }

        internal bool ExistingDocument(string file) {
            Boolean existing = false;

            foreach (KeyValuePair<string, Document> entry in this.documents) {
                if (entry.Key == file || entry.Value.GetFile() == file) {
                    existing = true;
                    break;
                }
            }

            return existing;
        }

        public void RemoveDocument(Document document) {
            document.Remove();
            this.documents.Remove(document.GetHash());
        }

        public void AddDocument(Document document) {
            document.OnChange(this.OnChanged);

            if (this.documents.ContainsKey(document.GetHash())) {
                Boot.Core().GetIDE().SetActiveDocument(document);
            } else {
                this.documents.Add(document.GetHash(), document);
                document.Init();
            }
        }

        public void OnChanged(Document document, Document.State state) {
            Logger.Info("[Workspace] Changed document: " + document.GetName() + " >> " + state);

            Boot.Core().GetIDE().GetMenu().Update();
            Boot.Core().GetIDE().OnChanged(document, state);
        }
    }
}
