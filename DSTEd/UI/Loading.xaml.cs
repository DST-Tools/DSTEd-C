using System;
using System.Windows;

namespace DSTEd.UI {
    public partial class Loading : Window {
        public Loading() {
            InitializeComponent();
        }

        public void SetProgress(int value) {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate () {
                progress.Value += value;
            }));
        }
    }
}
