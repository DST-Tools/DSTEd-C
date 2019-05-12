using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using DSTEd.Core.LUA;
using Xceed.Wpf.AvalonDock.Layout;

namespace DSTEd.UI.Components {
    public partial class Debugger : LayoutAnchorablePane {
        public Debugger() {
            InitializeComponent();

            this.DockMinHeight = 100;
            this.DockHeight = new GridLength(200);
			//debugger_debug.Children.
			Boot.Instance.DBGCLI.AddCommand("clear",clearcon);

			this.input.KeyDown += new KeyEventHandler(delegate (object sender, KeyEventArgs e) {
                if (Keyboard.IsKeyDown(Key.Enter)) {
                    AddDebug(this.input.Text);
					switch (((ComboBoxItem)target.SelectedItem).Content)
					{
						case "Server":
							foreach(var g in Boot.Core().GetSteam().GetGames())
							{
								if(g.GetID() == 343050)
								{
									((Core.Klei.Games.DSTS)g).SendCommand(input.Text);
								}
							}
							break;
						case "Console":
							AddDebug(Boot.Instance.DBGCLI.Execute(input.Text));
							//command example: 
							//cl_interp 0.0031
							break;
					}

                    this.input.Text = "";
                    this.input.Focus();
                }
            });
        }

		private string clearcon(params string[] args)
		{
			debugger_debug.Children.Clear();
			return string.Empty;
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
            Dispatcher.Invoke(DispatcherPriority.Normal, (Action) (() => {
                this.debugger_output.Children.Add(new DebugEntry(text));
            }));

            Scroll(this.debugger_output, this.autoscroll_debugger.IsChecking());
        }

        public void AddError(string text) {
            Dispatcher.Invoke(DispatcherPriority.Normal, (Action) (() => {
                this.debugger_errors.Children.Add(new DebugEntry(text));
            }));

            Scroll(this.debugger_errors, this.autoscroll_errors.IsChecking());
        }

        public void AddDebug(string text) {
            Dispatcher.Invoke(DispatcherPriority.Normal, (Action) (() => {
                this.debugger_debug.Children.Add(new DebugEntry(text));
            }));

            Scroll(this.debugger_debug, this.autoscroll_output.IsChecking());
        }

        public void Scroll(StackPanel element, Boolean autoscroll) {
            Dispatcher.Invoke(DispatcherPriority.Normal, (Action) (() => {
                if (autoscroll) {
                    ((ScrollViewer) element.Parent).ScrollToVerticalOffset(((ScrollViewer) element.Parent).ScrollableHeight - 0.01);
                }
            }));
        }

        public void Clear(StackPanel element) {
            element.Dispatcher.Invoke(DispatcherPriority.Normal, (Action) (() => {
                element.Children.Clear();

                for (int index = 0; index < element.Children.Count; index++) {
                    element.Children.RemoveAt(index);
                }
            }));
        }

        public void OnClearDebugger(object sender, RoutedEventArgs e) {
            Clear(this.debugger_debug);
        }

        public void OnClearOutput(object sender, RoutedEventArgs e) {
            Clear(this.debugger_output);
        }

        public void OnClearErrors(object sender, RoutedEventArgs e) {
            Clear(this.debugger_errors);
        }

        internal void AddError(ParserException exception) {
            AddError(exception.ToString());
        }
    }
}
