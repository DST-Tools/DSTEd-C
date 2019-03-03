using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using DSTEd.UI.Components;
using DSTEd.UI.Contents;
using Xceed.Wpf.AvalonDock.Layout;

namespace DSTEd.Core.Klei {
    public class KleiDebugger {
        protected Process process = null;

        public KleiDebugger() {
            this.process = new Process();
            this.process.StartInfo.UseShellExecute = false;
            //this.process.StartInfo.CreateNoWindow = true; //for server?
            this.process.StartInfo.RedirectStandardInput = false; // Currently disabled, because the process waiting for an action
            this.process.StartInfo.RedirectStandardOutput = true;
            this.process.StartInfo.RedirectStandardError = true;
            this.process.EnableRaisingEvents = true;

            this.process.OutputDataReceived += new DataReceivedEventHandler(delegate (object sender, DataReceivedEventArgs e) {
                AddOutput(e.Data);
            });

            this.process.ErrorDataReceived += new DataReceivedEventHandler(delegate (object sender, DataReceivedEventArgs e) {
                AddError(e.Data);
            });
        }

        public void AddOutput(string text) {
            Boot.Core().GetIDE().GetDebugPanel().AddOutput(text);
        }

        public void AddError(string text) {
            Boot.Core().GetIDE().GetDebugPanel().AddError(text);
        }

        public void AddDebug(string text) {
            Boot.Core().GetIDE().GetDebugPanel().AddDebug(text);
        }

        public void Attach(string executable) {
            this.process.StartInfo.WorkingDirectory = Path.GetDirectoryName(executable);
            this.process.StartInfo.FileName = Path.GetFullPath(executable);

            AddDebug("Working directory: " + this.process.StartInfo.WorkingDirectory);
            AddDebug("Attach: " + this.process.StartInfo.FileName);

            new Thread(delegate () {
                AddDebug("Attach to new Thread.");

                try {
                    AddDebug("Start Application...");
                    this.process.Start();

                    AddDebug("Invoke Output-Proxy");
                    this.process.BeginOutputReadLine();

                    AddDebug("Invoke Error-Proxy");
                    this.process.BeginErrorReadLine();

                    AddDebug("Application is now running. Waiting for exit...");
                    this.process.WaitForExit();

                    AddDebug("Detach Proxies...");
                    this.process.CancelOutputRead();
                    this.process.CancelErrorRead();

                    AddDebug("Application was exited normally");
                } catch (System.ComponentModel.Win32Exception ex) {
                    AddDebug("ERROR: " + ex.Message);
                    Logger.Warn("[KleiDebugger]", ex.Message); // mainly file notfound.
                } catch (InvalidOperationException) {
                    AddDebug("ERROR: Wrong StartInfo");
                    Logger.Warn("[KleiDebugger]", "Wrong StartInfo");
                } catch (Exception) {
                    AddDebug("ERROR: unkown error");
                    Logger.Warn("[KleiDebugger]", "unkown error");
                }
            }).Start();
        }
    }
}
