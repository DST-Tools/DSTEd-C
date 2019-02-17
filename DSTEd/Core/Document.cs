using System;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using DSTEd.Core.Contents;

namespace DSTEd.Core {
    public class Document {
        public enum State {
            CREATED,
            CHANGED,
            REMOVED
        };

        public enum Editor {
            NONE,
            CODE,
            TEXTURE
        }

        private string title = null;
        private string file = null;
        private Action<Document, State> callback_changed = null;
        private Editor type = Editor.NONE;
        private object content = null;
        private string file_content = null;

        public Document() {
         
        }

        public Document(Editor type) {
            this.type = type;
        }

        public void SetTitle(string title) {
            this.title = title;
        }

        public void Load(string file) {
            this.SetTitle(Path.GetFileName(file));
            this.file = file;

            try {
                using (StreamReader reader = new StreamReader(this.GetFile())) {
                    this.file_content = reader.ReadToEnd();
                }
            } catch (IOException) {
            }
        }

        public string GetName() {
            if (this.title != null) {
                return this.title;
            }

            return this.GetFile(); // Hashing(?)
        }

        public string GetFileContent() {
            return this.file_content;
        }

        public string GetFile() {
            return this.file;
        }

        public void Init() {
            this.callback_changed?.Invoke(this, State.CREATED);
        }

        public void OnChange(Action<Document, State> callback) {
            this.callback_changed = callback;
        }

        internal object GetContent() {
            switch (this.type) {
                case Editor.NONE:
                    this.content = new Welcome();
                    break;
                case Editor.CODE:
                    this.content = new Contents.Editors.Code(this);
                    break;
                case Editor.TEXTURE:
                    this.content = new Contents.Editors.TEX(this);
                    break;
            }

            return content;
        }
    }
}
