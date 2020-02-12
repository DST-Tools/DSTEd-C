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
#pragma warning disable CA1001
    public class KleiDebugger {
#pragma warning restore CA1001
        protected Process process = null;

        public KleiDebugger() {
            this.process = new Process();
            this.process.StartInfo.UseShellExecute = false;
            //this.process.StartInfo.CreateNoWindow = true; //for server?
            this.process.StartInfo.RedirectStandardInput = true; // Currently disabled, because the process waiting for an action  //enabled 2019/3/14,test success
            this.process.StartInfo.RedirectStandardOutput = true;
            this.process.StartInfo.RedirectStandardError = true;
			process.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
			process.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;
            this.process.EnableRaisingEvents = true;

            this.process.OutputDataReceived += new DataReceivedEventHandler(delegate (object sender, DataReceivedEventArgs e) {
                AddOutput(e.Data);
            });

            this.process.ErrorDataReceived += new DataReceivedEventHandler(delegate (object sender, DataReceivedEventArgs e) {
                AddError(e.Data);
            });
        }

		public ProcessStartInfo GetStartInfo()
		{
			return process.StartInfo;
		}

		public void SendCommand(string LuaCommand)
		{
			process.StandardInput.WriteLineAsync(LuaCommand);
		}

		public void ForceShutdown()
		{
			process.Kill();
		}

        public void AddOutput(string text) {
            Boot.Core.IDE.GetDebugPanel().AddOutput(text);
        }

        public void AddError(string text) {
            Boot.Core.IDE.GetDebugPanel().AddError(text);
        }

        public void AddDebug(string text) {
            Boot.Core.IDE.GetDebugPanel().AddDebug(text);
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

                    //AddDebug("Set encoding as UTF-8");
                    //process.

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
