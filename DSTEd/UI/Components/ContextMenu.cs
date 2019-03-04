using System;
using System.Windows;
using System.Windows.Controls;

namespace DSTEd.UI.Components {
    public class ContextMenu : System.Windows.Controls.ContextMenu {
        public void Add(string title, Action callback) {
            MenuItem item = new MenuItem();
            item.Header = title;
            item.Click += new RoutedEventHandler(delegate (object sender, RoutedEventArgs e) {
                callback?.Invoke();
            });

            this.AddChild(item);
        }
    }
}
