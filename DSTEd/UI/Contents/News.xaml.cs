using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace DSTEd.UI.Contents {
    public partial class News : UserControl {
        private string url = null;

        public News() {
            InitializeComponent();

            this.title.MouseLeftButtonDown += new MouseButtonEventHandler(this.OnClick);
        }

        internal void AddLink(string url) {
            this.url = url;
        }

        protected void OnClick(object sender, MouseButtonEventArgs e) {
            if (this.url != null) {
                Process.Start(this.url);
            }
        }
    }
}
