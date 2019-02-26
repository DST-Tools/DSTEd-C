using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;

namespace DSTEd.Core.LUA {
    class ModInfo {
        private int id;
        private string name = null;
        private string version = null;
        private string description = null;
        private string author = null;
        private string forumthread = null;
        private string icon_atlas = null;
        private string icon = null;
        private int api_version;
        private bool dont_starve_compatible;
        private bool reign_of_giants_compatible;
        private bool all_clients_require_mod;
        private bool dst_compatible;
        private bool standalone;
        private bool restart_require;
        private List<string> server_filter_tags;
        private Boolean is_broken = false;

        /*
         * @ToDo
         * - client_only_mod (bool)
         * - configuration_options (dynamic)
         * - priority (int)
         * - dst_api_version (int)
         * - shipwrecked_compatible (bool)
         * 
         */
        public ModInfo(Table values) {
            if (values == null) {
                this.is_broken = true;
                return;
            }

            foreach(DynValue key in values.Keys) {
                switch (key.String) {
                    case "name":
                        this.name = (string) values[key.String];
                        break;
                    case "version":
                        this.version = (string) values[key.String];
                        break;
                    case "description":
                        this.description = (string) values[key.String];
                        break;
                    case "author":
                        this.author = (string) values[key.String];
                    break;
                    case "forumthread":
                        this.forumthread = (string) values[key.String];
                    break;
                    case "api_version":
                        this.api_version = Convert.ToInt32((double) values[key.String]);
                    break;
                    case "dont_starve_compatible":
                        this.dont_starve_compatible = (Boolean) values[key.String];
                    break;
                    case "all_clients_require_mod":
                        this.all_clients_require_mod = (Boolean) values[key.String];
                    break;
                    case "dst_compatible":
                        this.dst_compatible = (Boolean) values[key.String];
                    break;
                    case "reign_of_giants_compatible":
                        this.reign_of_giants_compatible = (Boolean) values[key.String];
                    break;
                    case "standalone":
                        this.standalone = (Boolean) values[key.String];
                    break;
                    case "restart_require":
                        this.restart_require = (Boolean) values[key.String];
                    break;
                    case "icon_atlas":
                        this.icon_atlas = (string) values[key.String];
                    break;
                    case "icon":
                        this.icon = (string) values[key.String];
                    break;
                    case "server_filter_tags":
                        List<string> list = new List<string>();
                        Table entries = (Table) values[key.String];

                        foreach (TablePair a in entries.Pairs) {
                            list.Add((string) a.Value.String);
                        }

                        this.server_filter_tags = list;
                    break;
                   /* default:
                        Logger.Debug("Unknown LUA-Variable: " + key.String);
                        break;*/
                }
            }
            
        }

        public Boolean IsBroken() {
            return this.is_broken;
        }

        public void SetID(int id) {
            this.id = id;
        }

        public int GetID() {
            return this.id;
        }

        public string GetName() {
            return this.name;
        }

        public string GetVersion() {
            return this.version;
        }

        public string GetDescription() {
            return this.description;
        }

        public string GetAuthor() {
            return this.author;
        }

        public string GetForumThread() {
            return this.forumthread;
        }

        public string GetIconAtlas() {
            return this.icon_atlas;
        }

        public string GetIcon() {
            return this.icon;
        }

        public int GetAPIVersion() {
            // @ToDo enum?
            return this.api_version;
        }

        public bool IsDS() {
            return this.dont_starve_compatible;
        }

        public bool IsDST() {
            return this.dst_compatible;
        }

        public bool IsROG() {
            return this.reign_of_giants_compatible;
        }

        public bool IsRequired() {
            return this.all_clients_require_mod;
        }

        public bool ModsAllowed() {
            return this.standalone;
        }

        public bool MustRestart() {
            return this.restart_require;
        }

        public List<string> GetTags() {
            return this.server_filter_tags;
        }
    }

    class LUAInterpreter {
        public static Script Run(string lua, Action<SyntaxErrorException> callback) {
            try {
                Script script = new Script();
                script.DoString(lua);
                return script;
            } catch (SyntaxErrorException e) {
                callback?.Invoke(e);
            }

            return null;
        }

        public static ModInfo GetModInfo(string lua, Action<SyntaxErrorException> callback) {
            Script result = LUAInterpreter.Run(lua, callback);

            if(result == null) {
                return new ModInfo(null);
            }

            return new ModInfo(result.Globals);
        }
    }
}
