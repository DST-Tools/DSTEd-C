using System;
using MoonSharp.Interpreter;

namespace DSTEd.Core.Klei.Data {
    public class OptionsEntry {
        private string description;
        private string hover;
        private object data;

        public OptionsEntry(TablePair data) {
            //Console.WriteLine(data.ToString()); // this ToString returns only "MoonSharp.Interpreter.TablePair"
            //Console.WriteLine("k = " + data.Key.ToString() + ",v = " + data.Value.ToString());
            this.description = "";
            this.hover = "";
            this.data = (object) "";
        }

        public string GetDescription() {
            return this.description;
        }

        public string GetHover() {
            return this.hover;
        }

        public object GetData() {
            return this.data;
        }
    }
}
