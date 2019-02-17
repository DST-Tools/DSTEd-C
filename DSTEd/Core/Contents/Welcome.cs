using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Resources;

namespace DSTEd.Core.Contents {
    public class Welcome : WrapPanel {
        private WebBrowser browser = null;

        public Welcome() {
            this.Width = Double.NaN;
            this.Height = Double.NaN;
            this.browser = new WebBrowser();
            this.browser.Width = Double.NaN;
            this.browser.Height = Double.NaN;

            try {
                this.browser.NavigateToStream(Assembly.Load("DSTEd").GetManifestResourceStream("DSTEd.Assets.Documents.Welcome.html"));
            } catch(Exception) {

            }
                
            this.Children.Add(this.browser);
        }
    }
}
