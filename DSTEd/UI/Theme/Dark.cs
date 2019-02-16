using System;

namespace DSTEd.UI.Theme {
    public class Dark : Xceed.Wpf.AvalonDock.Themes.Theme {
        public override Uri GetResourceUri() {
            return new Uri("/DSTEd;component/UI/Theme/Dark.xaml", UriKind.Relative);
        }
    }
}
