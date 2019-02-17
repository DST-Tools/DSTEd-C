using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace DSTEd.Core {
    class Configuration {
        private string name = "config.json";
        private string path = null;
        private Dictionary<string, string> data;

        public Configuration() {
            this.path = AppDomain.CurrentDomain.BaseDirectory;
            this.data = new Dictionary<string, string>();
            this.Load();
        }

        public string GetFullPath() {
            return string.Format("{0}{1}{2}", this.path, Path.DirectorySeparatorChar, this.name);
        }

        public void Load() {
            try {
                using(StreamReader reader = new StreamReader(this.GetFullPath())) {
                    this.data = JsonConvert.DeserializeObject<Dictionary<string, string>>(reader.ReadToEnd());
                }
            } catch (IOException) {
                Logger.Warn("Configuration can't be found. Creating by default.", this.GetFullPath());
                this.Save();
            }
        }

        public void Save() {
            try {
                File.WriteAllText(this.GetFullPath(), JsonConvert.SerializeObject(this.data, Formatting.Indented));
            } catch (IOException) {
                Logger.Warn("Configuration can't be write!", this.GetFullPath());
            }
        }

        public void Set(string name, string value) {
            this.data.Add(name, value);
        }

        public string Get(string name, string defaults) {
            if(!this.data.ContainsKey(name)) {
                return defaults;
            }

            string value = null;

            this.data.TryGetValue(name, out value);

            if (value == null) {
                return defaults;
            }

            return value;
        }

        public string GetPath() {
            return this.path;
        }
    }
}
