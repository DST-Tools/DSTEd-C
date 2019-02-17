using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSTEd.Core {
    class Document {
        public enum State {
            CREATED,
            CHANGED,
            REMOVED
        };

        private string title = null;
        private string file = null;
        private Action<Document, State> callback_changed = null;

        public Document() {
            
        }

        public void SetTitle(string title) {
            this.title = title;
        }

        public string GetName() {
            if (this.title != null) {
                return this.title;
            }

            return this.GetFile(); // Hashing(?)
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
    }
}
