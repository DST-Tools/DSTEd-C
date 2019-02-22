using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DSTEd.UI.Components;

namespace DSTEd.Core.Contents.Editors {
    class LUA : TabControl {
        private Document document;

        public LUA(Document document) {
            this.document = document;
            this.TabStripPlacement = Dock.Bottom;
            this.BorderBrush = Brushes.Transparent;
            this.BorderThickness = new Thickness(0);
            this.Background = Brushes.Transparent;

            this.CreatePropertiesEditor();
            this.CreateSourceEditor();
        }

        private void CreatePropertiesEditor() {
            TabItem item = new TabItem();
            item.Header = I18N.__("Editor");
            this.AddChild(item);
        }

        private void CreateSourceEditor() {
            TabItem item = new TabItem();
            item.Header = I18N.__("Source");
            item.Content = new Code(this.document);
            this.AddChild(item);
        }
    }
}
