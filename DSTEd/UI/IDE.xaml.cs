using System;
using System.Windows;
using DSTEd.Core;
using DSTEd.Core.Contents;
using DSTEd.UI.Theme;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace DSTEd.UI {
    public partial class IDE : Window {
        private Menu menu = null;
        private Core.DSTEd core = null;

        public IDE(Core.DSTEd core) {
            InitializeComponent();

            this.core = core;
            this.menu = new Menu(this);
            this.dockManager.Theme = new Dark();
            this.Closing += this.IDE_Closing;
        }

        public System.Windows.Controls.MenuItem GetTools() {
            return this.tools;
        }

        public Menu GetMenu() {
            return this.menu;
        }

        public Core.DSTEd GetCore() {
            return this.core;
        }

        private void OnLayoutRootPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            var activeContent = ((LayoutRoot) sender).ActiveContent;

            if (e.PropertyName == "ActiveContent") {
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
            Dialog.Open("Are you sure you want to close the document?", "DSTEd", Dialog.Buttons.YesNo, Dialog.Icon.Warning, delegate (Dialog.Result result) {
                if (result == Dialog.Result.Yes) {
                    return true;
                }
                // @ToDo check closing document to fire REMOVED event
                e.Cancel = true;
                return true;
            });
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
            Dialog.Open("Are you sure you want to close this tool?", "DSTEd", Dialog.Buttons.YesNo, Dialog.Icon.Warning, delegate (Dialog.Result result) {
                if (result == Dialog.Result.Yes) {
                    return true;
                }

                e.Cancel = true;
                return true;
            });
        }

        private void OnShowHeader(object sender, RoutedEventArgs e) {

        }

        private void OnMenu(object sender, RoutedEventArgs e) {
            var item = sender as System.Windows.Controls.MenuItem;

            if (item != null) {
                this.menu.Handle(item.Name, item);
            }
        }

        public LayoutDocumentPane GetEditors() {
            return this.editors;
        }

        public void SetActiveDocument(Document document) {
            foreach (LayoutDocument entry in this.editors.Children) {
                if (entry.GetType() == typeof(AvalonDocument)) {
                    AvalonDocument doc = (AvalonDocument) entry;
                    doc.IsActive = (doc.GetDocument() == document);
                }
            }
        }

        internal void OnChanged(Document document, Document.State state) {
            Logger.Info("[IDE] Changed document: " + document.GetName() + " >> " + state);

            switch (state) {
                case Document.State.CREATED:
                    AvalonDocument d = new AvalonDocument(document);
                    this.editors.Children.Add(d);
                    this.SetActiveDocument(document);
                    break;
                case Document.State.CHANGED:
                    foreach (LayoutDocument entry in this.editors.Children) {
                        if (entry.GetType() == typeof(AvalonDocument)) {
                            AvalonDocument doc = (AvalonDocument) entry;

                            if (doc.GetDocument() == document) {
                                doc.Title = document.GetName() + "*";
                            }
                        }
                    }
                    break;
                case Document.State.REMOVED:
                    // @ToDo
                    break;
            }
        }

        private void IDE_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Environment.Exit(0);
        }
    }
}
