using System;
using System.IO;
using ICSharpCode.AvalonEdit;

namespace DSTEd.Core.Contents.Editors {
    class Code : TextEditor {
        private Document document;

        public Code(Document document) {
            this.document = document;
            this.ShowLineNumbers = true;
            this.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(this.document.GetFile()));
            this.Text = document.GetFileContent();
            this.Document.UpdateFinished += new EventHandler(OnChange);
        }

        private void OnChange(object sender, EventArgs e) {
            this.document.UpdateChanges();
        }
    }
}
