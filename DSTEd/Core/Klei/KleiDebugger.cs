using System;
using System.Diagnostics;
using System.IO;

namespace DSTEd.Core.Klei {
    public class KleiDebugger {
        protected Process process = null;
        private StreamReader output;
        private StreamWriter input;
        private StreamReader error;
        public bool readytouse = false;
        public KleiDebugger() {
            this.process = new Process();
            this.process.StartInfo.UseShellExecute = false;
            this.process.StartInfo.RedirectStandardInput = true;
            this.process.StartInfo.RedirectStandardOutput = true;
            this.process.StartInfo.RedirectStandardError = true;
            //this.process.StartInfo.CreateNoWindow = true; //for server?
        }

        public void Attach(string executable) {
            this.process.StartInfo.FileName = System.IO.Path.GetFullPath(executable);

            try {
                this.process.Start();
                this.input = this.process.StandardInput;
                this.output = this.process.StandardOutput;
                this.error = this.process.StandardError;
                readytouse = true;
            } catch (System.ComponentModel.Win32Exception ex) {
                Logger.Warn("KleiGame.cs, RunGameLambda\n", ex.Message); //mainly file notfound.
            } catch (InvalidOperationException) {
                Logger.Warn("KleiGame.cs, RunGameLambda\n", "Wrong StartInfo");
            } catch (Exception) {
                Logger.Warn("KleiGame.cs, RunGameLambda\n", "unkown error");
            }
        }

        public void SendCommand(string LUAcommand)
        {
            input.WriteLine(LUAcommand);
        }

        public async System.Threading.Tasks.Task<string> Read()
        {
            return await output.ReadToEndAsync();
        }

        public string ReadError()
        {
            return error.ReadToEnd();
        }
    }
}
