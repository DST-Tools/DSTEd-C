using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DSTEd.UI.Contents;

namespace DSTEd.Core {
    public interface DocumentHandler {
        void OnInit();
    }

    public class Document {
        public enum State {
            CREATED,
            CHANGED,
            REMOVED
        };

        public enum Editor {
            NONE,
            CODE,
            TEXTURE,
            MODINFO
        }

        private string title = null;
        private string file = null;
        private Action<Document, State> callback_changed = null;
        private Editor type = Editor.NONE;
        private object content = null;
        private string file_content = null;
        private Boolean is_closeable = true;
        private Boolean is_content_created = false;
        private Boolean is_content_loaded = false;
        private Boolean is_inited = false;

        public Document(Editor type) {
            this.type = type;

            Task.Run(() => {
                do {
                    if(this.is_content_created && this.is_content_loaded && this.is_inited) {
                        if (this.content != null && typeof(DocumentHandler).IsAssignableFrom(this.content.GetType())) {
                            ((DocumentHandler) this.content).OnInit();
                        }

                        break;
                    }

                    Task.Delay(500);
                } while (true);
            });
    }

        public string GetHash() {
            return Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(string.Format("{0}-{1}", this.GetTitle(), this.GetFile()))));
        }

        public Boolean IsCloseable() {
            return this.is_closeable;
        }

        public void SetCloseable(Boolean state) {
            this.is_closeable = state;
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

            Application.Current.Dispatcher.Invoke(DispatcherPriority.ApplicationIdle, new Action(delegate () {
                try {
                    using (StreamReader reader = new StreamReader(this.GetFile(), Encoding.UTF8)) {
                        this.file_content = reader.ReadToEnd();
                        this.is_content_loaded = true;
                    }
                } catch (IOException) {
                }
            }));
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
            this.is_inited = true;
        }
        
        public void UpdateChanges() {
            this.callback_changed?.Invoke(this, State.CHANGED);
        }

		public void ChangeContent(string changed)
		{
			file_content = changed;
		}

        public void Remove() {
            this.callback_changed?.Invoke(this, State.REMOVED);
        }

        public void OnChange(Action<Document, State> callback) {
            this.callback_changed = callback;
        }

		public void SaveDocument()
		{
			Logger.Info("Saving file, path:", file);
            
            if(content is Contents.Editors.Code ce)
            {
                file_content = ce.Text;
            }

			using (var wop = new FileStream(file,FileMode.Truncate))
			{
				byte[] buff = Encoding.UTF8.GetBytes(file_content);
				wop.Write(buff, 0, buff.Length);
			}
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
                case Editor.MODINFO:
                    this.content = new Contents.Editors.ModInfo(this);
                    break;
            }

            this.is_content_created = true;

            return content;
        }
    }
}
