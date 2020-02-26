using Xceed.Wpf.AvalonDock.Layout;

namespace DSTEd.Core.Contents {
    public class AvalonDocument : LayoutDocument {
        private Document document = null;

        public AvalonDocument(Document document) {
            this.document = document;
            this.Title = document.GetName();
            this.ContentId = document.GetName();
            this.CanClose = document.IsCloseable();
            // this.IconSource = document.GetIcon();
            this.Content = document.GetContent();
            this.CanFloat = true;
        }

        public Document GetDocument() {
            return this.document;
        }
    }
}
