using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using Newtonsoft.Json;

namespace DSTEd.Core {
    //Singleten Pattern, Use Configuration.getConfiguration() instead of new Configuration();
    public class Configuration {
        private string name = "config.json";
        private string path = null;
        private Dictionary<string, string> data;
        private static Configuration config = new Configuration();

        private Configuration() {
            this.path = AppDomain.CurrentDomain.BaseDirectory;
            this.data = new Dictionary<string, string>();
            this.Load();
        }

        public static Configuration getConfiguration()
        {
            return config;
        }

        public string GetFullPath() {
            return string.Format("{0}{1}{2}", this.path, Path.DirectorySeparatorChar, this.name);
        }

        public void Load() {
            try {
                using (StreamReader reader = new StreamReader(this.GetFullPath())) {
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

        // GetString
        public string Get(string name, string defaults) {
            if (!this.data.ContainsKey(name)) {
                return defaults;
            }
            string value = null;
            this.data.TryGetValue(name, out value);
            if (value == null) {
                return defaults;
            }
            return value;
        }

        public double GetDouble(string name, double defaults)
        {
            string s = Get(name, defaults.ToString());
            return Double.Parse(s);
        }

        public int GetInt(string name, int defaults) {
            string s = Get(name, defaults.ToString());
            return Int32.Parse(s);
        }

        public bool GetBool(string name, bool defaults) {
            string s = Get(name, defaults.ToString());
            return Boolean.Parse(s);
        }

        public Color GetColor(string name, Color defaults) {
            string s = Get(name, "");
            if (!s.Equals(""))
            {
                string[] c = s.Split(',');
                return Color.FromArgb(byte.Parse(c[0]), byte.Parse(c[1]), byte.Parse(c[2]), byte.Parse(c[3]));
            } else
            {
                return defaults;
            }
        }

        public string GetPath() {
            return this.path;
        }
    }
}
