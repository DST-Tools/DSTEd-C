using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DSTEd.UI {
    public partial class Workspace : Window {
        private Action<CancelEventArgs> callback_close = null;
        private Action<string, Boolean> callback_select = null;
        private Boolean ignore_callback = false;

        public Workspace() {
            InitializeComponent();
            Closing += OnWindowClosing;
        }

        public void OnClose(Action<CancelEventArgs> callback) {
            this.callback_close = callback;
        }

        public void OnWindowClosing(object sender, CancelEventArgs e) {
            if (!ignore_callback) {
                callback_close?.Invoke(e);
            }
        }

        public void Close(Boolean ignore_callback) {
            this.ignore_callback = true;
            this.Close();
        }

        private void OnBrowse(object sender, RoutedEventArgs e) {
            // @ToDo implement with own style (https://github.com/jkells/folder-browser-dialog-example/blob/master/FolderBrowserDialogEx.cs)
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.Title = "DST Location";
            //dialog.EnsureReadOnly = true;
            dialog.IsFolderPicker = true;
            dialog.AllowNonFileSystemItems = false;
            dialog.Multiselect = false;
            dialog.InitialDirectory = "D:/Software/Steam/SteamApps/common/";

            CommonFileDialogResult result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok) {
                this.path.Text = dialog.FileName;
            }
        }
        public void OnSelect(Action<string, Boolean> callback) {
            this.callback_select = callback;
        }

        private void OnSave(object sender, RoutedEventArgs e) {
            this.callback_select?.Invoke(this.path.Text, (Boolean) this.save.IsChecked);
        }
    }
}
