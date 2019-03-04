using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Xceed.Wpf.AvalonDock.Layout;

namespace DSTEd.UI.Components {
    public partial class Debugger : LayoutAnchorablePane {
        public Debugger() {
            InitializeComponent();

            this.DockMinHeight = 100;
            this.DockHeight = new GridLength(200);

            this.input.KeyDown += new KeyEventHandler(delegate (object sender, KeyEventArgs e) {
                if (Keyboard.IsKeyDown(Key.Enter)) {
                    AddDebug(this.input.Text);
                    this.input.Text = "";
                    this.input.Focus();
                }
            });
        }

        public StackPanel GetDebugger() {
            return this.debugger_debug;
        }

        public StackPanel GetOutput() {
            return this.debugger_output;
        }

        public StackPanel GetErrors() {
            return this.debugger_errors;
        }

        public void AddOutput(string text) {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action) (() => {
                this.debugger_output.Children.Add(new DebugEntry(text));
            }));

            Scroll(this.debugger_output);
        }

        public void AddError(string text) {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action) (() => {
                this.debugger_errors.Children.Add(new DebugEntry(text));
            }));

            Scroll(this.debugger_errors);
        }

        public void AddDebug(string text) {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action) (() => {
                this.debugger_debug.Children.Add(new DebugEntry(text));
            }));

            Scroll(this.debugger_debug);
        }

        public void Scroll(StackPanel element) {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action) (() => {
                if (this.autoscroll.IsChecking()) {
                    ((ScrollViewer) element.Parent).ScrollToVerticalOffset(((ScrollViewer) element.Parent).ScrollableHeight - 0.01);
                }
            }));
        }

        public void Clear(StackPanel element) {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action) (() => {
                element.Children.Clear();

                for (int index = 0; index < element.Children.Count; index++) {
                    element.Children.RemoveAt(index);
                }
            }));
        }

        public void OnClear(object sender, RoutedEventArgs e) {
            Clear(this.debugger_output);
        }
    }
}
