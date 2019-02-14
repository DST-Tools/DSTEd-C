using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using DSTEd.Core;
using DSTEd.UI.Theme;
using MLib.Interfaces;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace DSTEd.UI {
    public partial class IDE : Window {

        public IDE() {
            InitializeComponent();

            this.dockManager.Theme = new Dark();
        }

        private void OnLayoutRootPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            var activeContent = ((LayoutRoot) sender).ActiveContent;

            if(e.PropertyName == "ActiveContent") {
                Logger.Info(string.Format("ActiveContent-> {0}", activeContent));
            }
        }

        private void OnLoadLayout(object sender, RoutedEventArgs e) {
            
        }

        private void OnSaveLayout(object sender, RoutedEventArgs e) {

        }

        private void OnShowWinformsWindow(object sender, RoutedEventArgs e) {
            
        }

        private void AddTwoDocuments_click(object sender, RoutedEventArgs e) {
           
        }

        private void OnShowToolWindow1(object sender, RoutedEventArgs e) {
           
        }

        private void dockManager_DocumentClosing(object sender, DocumentClosingEventArgs e) {
            if (MessageBox.Show("Are you sure you want to close the document?", "DSTEd", MessageBoxButton.YesNo) == MessageBoxResult.No) {
                e.Cancel = true;
            }
        }

        private void OnDumpToConsole(object sender, RoutedEventArgs e) {
           
        }

        private void OnReloadManager(object sender, RoutedEventArgs e) {
        }

        private void OnUnloadManager(object sender, RoutedEventArgs e) {
            if (layoutRoot.Children.Contains(dockManager)) {
                layoutRoot.Children.Remove(dockManager);
            }
        }

        private void OnLoadManager(object sender, RoutedEventArgs e) {
            if (!layoutRoot.Children.Contains(dockManager)) {
                layoutRoot.Children.Add(dockManager);
            }
        }

        private void OnToolWindow1Hiding(object sender, System.ComponentModel.CancelEventArgs e) {
            if (MessageBox.Show("Are you sure you want to hide this tool?", "DSTEd", MessageBoxButton.YesNo) == MessageBoxResult.No) {
                e.Cancel = true;
            }
        }

        private void OnShowHeader(object sender, RoutedEventArgs e) {
            
        }
    }
}
