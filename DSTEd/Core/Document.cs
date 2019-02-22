using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using DSTEd.UI.Contents;

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
        private DSTEd core;

        public Document(DSTEd core, Editor type) {
            this.core = core;
            this.type = type;
        }

        public string GetHash() {
            return Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(string.Format("{0}-{1}", this.GetTitle(), this.GetFile()))));
        }

        public DSTEd GetCore() {
            return this.core;
        }

        public void SetTitle(string title) {
            this.title = title;
        }

        public string GetTitle() {
            return this.title;
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

        public void UpdateChanges() {
            this.callback_changed?.Invoke(this, State.CHANGED);
        }

        public void OnChange(Action<Document, State> callback) {
            this.callback_changed = callback;
        }

        internal object GetContent() {
            switch (this.type) {
                case Editor.NONE:
                    this.content = new Welcome(this);
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
