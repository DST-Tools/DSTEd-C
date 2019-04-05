using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DSTEd.Core;

namespace DSTEd.UI {
    public partial class Loading : Window {
        private Action callback_success = null;
        private List<KeyValuePair<String, Func<Boolean>>> workers = new List<KeyValuePair<String, Func<Boolean>>>();
        private Boolean running = false;

        public Loading() {
            InitializeComponent();
        }

        public void SetProgress(int value) {
            Logger.Warn("Percent: ", value);

            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(delegate () {
                progress.Value = value;
            }));
        }

        private void OnMove(object sender, MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                this.DragMove();
            }
        }

        public void Wait() {
            this.running = false;
        }

        public void Resume() {
            this.running = true;
        }

        public Boolean IsRunning() {
            return this.running;
        }

        public void Run(String name, Func<Boolean> callback) {
            this.workers.Add(new KeyValuePair<String, Func<Boolean>>(name, callback));
        }

        public void OnSuccess(Action callback) {
            this.callback_success = callback;
        }

		//private delegate void WorkerThreadFunction();

        public void Start() {
            this.Resume();
            this.Show();

            int complete = this.workers.Count;
            int position = -1;

			/*for(int i=0;i<3;i++)
			{
				
				//new Thread(workers[i].Value)
			}*/

            Task.Run(() => {
                do {
                    if (this.IsRunning()) {
                        ++position;
                        var entry = this.workers[position];
                        String name = entry.Key;
                        Func<Boolean> callback = entry.Value;

                        Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate () {
                            if (!callback()) {
                                this.Wait();
                            }
                        })).Wait();

                        this.SetProgress(position * 100 / complete);
                        Thread.Sleep(300);
                    }

                    if (position >= complete) {
                        break;
                    }
                } while (position + 1 < complete);

                this.SetProgress(100);

                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate () {
                    this.callback_success();
                }));
            });
        }
    }
}