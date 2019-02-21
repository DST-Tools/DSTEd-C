using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
