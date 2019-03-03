using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using DSTEd.Core.IO;

namespace DSTEd.Core.Klei {
    public class KleiGame {
        protected int id = -1;
        protected string name = null;
        protected string path = null;
        protected string executable = null;
        protected Process AppProcess = null;
        public class ConsoleIO
        {
            public System.IO.StreamReader ConOut;
            public System.IO.StreamWriter ConIn;
            public System.IO.StreamReader ConErr;
        }
        protected ConsoleIO AppConsole;

        private Boolean is_main = false;
        private FileSystem files = null;



        public KleiGame()
        {
            AppConsole = new ConsoleIO();

            AppProcess = new Process();
            AppProcess.StartInfo.UseShellExecute = false;
            AppProcess.StartInfo.RedirectStandardInput = true;
            AppProcess.StartInfo.RedirectStandardOutput = true;
            AppProcess.StartInfo.RedirectStandardError = true;
            //AppProcess.StartInfo.CreateNoWindow = true; //for server?
        }

        public string GetName() {
            return this.name;
        }

        public int GetID() {
            return this.id;
        }

        public Boolean IsMainGame() {
            return this.is_main;
        }

        public void SetMainGame() {
            this.is_main = true;
        }

        public void SetPath(string path) {
            this.path = path;
            this.Update();
        }

        private void Update() {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.ApplicationIdle, new Action(delegate () {
                this.files = new FileSystem(this.GetPath());
            }));
        }

        public FileSystem GetFiles() {
            return this.files;
        }

        public string GetPath() {
            return this.path;
        }

        public string GetExecutable() {
            return this.executable;
        }

        private MenuItem AddToolMenu(string name, string executable) {
            MenuItem item = new MenuItem();
            item.Name = name;
            item.Header = I18N.__(name);

            if (executable != null) {
                item.Click += new RoutedEventHandler(delegate (object sender, RoutedEventArgs e) 
                {
                    AppProcess.StartInfo.FileName = System.IO.Path.GetFullPath(this.path + "\\" + this.executable);
                    try
                    {
                        AppProcess.Start();
                        AppConsole.ConIn = AppProcess.StandardInput;
                        AppConsole.ConOut = AppProcess.StandardOutput;
                        AppConsole.ConErr = AppProcess.StandardError;
                    }
                    catch(System.ComponentModel.Win32Exception ex)
                    {
                        Logger.Warn("KleiGame.cs, RunGameLambda\n", ex.Message);//mainly file notfound.
                    }
                    catch (InvalidOperationException)
                    {
                        Logger.Warn("KleiGame.cs, RunGameLambda\n", "Wrong StartInfo");
                    }
                    catch (Exception)
                    {
                        Logger.Warn("KleiGame.cs, RunGameLambda\n", "unkown error");
                    }
                });
            }

            return item;
        }

        public void AddTool(string name, string executable) {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Render, new Action(delegate () {
                MenuItem item = AddToolMenu(name, executable);
                MenuItem tools = Boot.Core().GetIDE().GetTools();
                tools.Items.Add(item);
            }));
        }

        public void AddSubTool(string node, string name, string executable) {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Render, new Action(delegate () {
                MenuItem item = AddToolMenu(name, executable);
                MenuItem tools = Boot.Core().GetIDE().GetTools();
                MenuItem found = null;

                foreach (MenuItem entry in tools.Items) {
                    if (entry.Name == node) {
                        found = entry;
                        break;
                    }
                }

                if (found != null) {
                    found.Items.Add(item);
                }
            }));
        }
    }
}
