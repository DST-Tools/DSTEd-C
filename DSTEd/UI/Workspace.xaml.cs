using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DSTEd.UI {
    public partial class Workspace : Window {
        private Action<CancelEventArgs> callback_close = null;

        public Workspace() {
            InitializeComponent();
            Closing += OnWindowClosing;
        }

        public void OnClose(Action<CancelEventArgs> callback) {
            this.callback_close = callback;
        }

        public void OnWindowClosing(object sender, CancelEventArgs e) {
            callback_close?.Invoke(e);
        }

        private void OnBrowse(object sender, RoutedEventArgs e) {
            // @ToDo implement with own style (https://github.com/jkells/folder-browser-dialog-example/blob/master/FolderBrowserDialogEx.cs)
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.Title = "DST Location";
            dialog.EnsureReadOnly = true;
            dialog.IsFolderPicker = true;
            dialog.AllowNonFileSystemItems = false;
            dialog.Multiselect = false;
            //dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            CommonFileDialogResult result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok) {
                this.path.Text = dialog.FileName;
            }
        }

        private void OnSave(object sender, RoutedEventArgs e) {

        }
    }
}
