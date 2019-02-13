using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using DSTEd.Core;

namespace DSTEd.UI {
    public partial class Loading : Window {
        private Action callback_success = null;
        private List<KeyValuePair<String, Action>> workers = new List<KeyValuePair<String, Action>>();

        public Loading() {
            InitializeComponent();
        }

        public void SetProgress(int value) {
            Logger.Warn("Percent: ", value);
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, new Action(delegate () {
                progress.Value = value;
            }));
        }

        public void Run(String name, Action callback) {
            this.workers.Add(new KeyValuePair<String, Action>(name, callback));
        }

        public void OnSuccess(Action callback) {
            this.callback_success += callback;
        }

        public void Start() {
            this.Show();

            int complete = this.workers.Count;
            int position = 0;

            foreach (var entry in this.workers) {
                ++position;
                this.Loop(entry, position, complete);
            }

            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, new Action(delegate () {
                this.SetProgress(100);
                Thread.Sleep(500);
                this.callback_success();
            }));
        }

        private void Loop(KeyValuePair<String, Action> entry, int position, int complete) {
            String name = entry.Key;
            Action callback = entry.Value;

            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, new Action(delegate () {
                callback();
                this.SetProgress(position * 100 / complete);
                Thread.Sleep(500);
            }));
        }
    }
}