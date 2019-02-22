using System;
using System.IO;
using System.Reflection;
using System.Xml;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace DSTEd.Core.Contents.Editors {
    class Code : TextEditor {
        private Document document;

        public Code(Document document) {
            this.document = document;
            this.ShowLineNumbers = true;
            this.SyntaxHighlighting = LoadSyntax(Path.GetExtension(this.document.GetFile()));
            this.Text = document.GetFileContent();
            this.Document.UpdateFinished += new EventHandler(OnChange);
            
            new XmlFoldingStrategy().UpdateFoldings(FoldingManager.Install(this.TextArea), this.Document);
        }

        private IHighlightingDefinition LoadSyntax(string extension) {
            if (extension == ".lua") {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DSTEd.Core.Contents.Editors.Format.LUA.xshd")) {
                    using (XmlTextReader reader = new XmlTextReader(stream)) {
                        return HighlightingLoader.Load(reader, HighlightingManager.Instance);
                    }
                }
            } else {
                return HighlightingManager.Instance.GetDefinitionByExtension(extension);
            }
        }

        private void OnChange(object sender, EventArgs e) {
            this.document.UpdateChanges();
        }
    }
}
