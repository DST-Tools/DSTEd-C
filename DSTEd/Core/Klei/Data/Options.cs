using MoonSharp.Interpreter;
using DSTEd.Core.Klei.Data;
using System.Collections.Generic;

namespace DSTEd.Core.Klei.Data {
    public class Options {
        private string name;
        private string label;
        private string longlabel;
        private string hover;
        private object defaults;
        private List<OptionsEntry> options;

        public Options(IEnumerable<TablePair> pairs) {
            foreach (TablePair entry in pairs) {
                switch (entry.Key.String) {
                    case "name":
                        this.name = entry.Value.String;
                        break;
                    case "label":
                        this.label = entry.Value.String;
                        break;
                    case "longlabel":
                        this.longlabel = entry.Value.String;
                        break;
                    case "hover":
                        this.hover = entry.Value.String;
                        break;
                    case "default":
                        this.defaults = entry.Value.ToObject();
                        break;
                    case "options":
                        this.options = new List<OptionsEntry>();

                        foreach (TablePair sub in entry.Value.Table.Pairs) {
                            this.options.Add(new OptionsEntry(sub));
                        }
                        break;
                }
            }
        }

        public string GetName() {
            return this.name;
        }

        public string GetLabel() {
            return this.label;
        }

        public string GetLongLabel() {
            return this.longlabel;
        }

        public string GetHover() {
            return this.hover;
        }

        public object GetDefaults() {
            return this.defaults;
        }

        public List<OptionsEntry> GetOptions() {
            return this.options;
        }
    }
}
