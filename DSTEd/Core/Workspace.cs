using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using DSTEd.Core;

namespace DSTEd.Core {
    class Workspace {
        private UI.Workspace window;
        private DSTEd core = null;
        private string path = "C:\\Program Files\\";
        private Dictionary<string, Document> documents = null;

        public Workspace(DSTEd core) {
            this.core = core;
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
            Document welcome = new Document();
            welcome.SetTitle("Welcome");
            this.AddDocument(welcome);
        }

        public void AddDocument(Document document) {
            document.OnChange(this.OnChanged);
            this.documents.Add(document.GetName(), document);
            document.Init();
        }

        public void OnChanged(Document document, Document.State state) {
            Logger.Info("[Workspace] Changed document: " + document.GetName() + " >> " + state);

            // Forward to IDE-UI
            this.core.GetIDE().OnChanged(document, state);
        }
    }
}
