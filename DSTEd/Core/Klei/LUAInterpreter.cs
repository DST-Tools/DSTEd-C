using System;
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
        private string[] server_filter_tags;
        private Boolean is_broken = false;

        public ModInfo(Table values) {
            if (values == null) {
                this.is_broken = true;
                return;
            }

            try {
                this.name = (string) values["name"];
            } catch (Exception) {
                Logger.Warn("Can't find \"name\" on ModInfo.");
            }

            try {
                this.version = (string) values["version"];
            } catch (Exception) {
                Logger.Warn("Can't find \"version\" on ModInfo.");
            }

            try {
                this.description = (string) values["description"];
            } catch (Exception) {
                Logger.Warn("Can't find \"description\" on ModInfo.");
            }

            try {
                this.author = (string) values["author"];
            } catch (Exception) {
                Logger.Warn("Can't find \"author\" on ModInfo.");
            }

            try {
                this.forumthread = (string) values["forumthread"];
            } catch (Exception) {
                Logger.Warn("Can't find \"forumthread\" on ModInfo.");
            }

            try {
                this.api_version = (int) values["api_version"];
            } catch (Exception) {
                Logger.Warn("Can't find \"api_version\" on ModInfo.");
            }

            try {
                this.dont_starve_compatible = (bool) values["dont_starve_compatible"];
            } catch (Exception) {
                Logger.Warn("Can't find \"dont_starve_compatible\" on ModInfo.");
            }

            try {
                this.all_clients_require_mod = (bool) values["all_clients_require_mod"];
            } catch (Exception) {
                Logger.Warn("Can't find \"all_clients_require_mod\" on ModInfo.");
            }

            try {
                this.dst_compatible = (bool) values["dst_compatible"];
            } catch (Exception) {
                Logger.Warn("Can't find \"dst_compatible\" on ModInfo.");
            }

            try {
                this.reign_of_giants_compatible = (bool) values["reign_of_giants_compatible"];
            } catch (Exception) {
                Logger.Warn("Can't find \"reign_of_giants_compatible\" on ModInfo.");
            }

            try {
                this.standalone = (bool) values["standalone"];
            } catch (Exception) {
                Logger.Warn("Can't find \"standalone\" on ModInfo.");
            }

            try {
                this.restart_require = (bool) values["restart_require"];
            } catch (Exception) {
                Logger.Warn("Can't find \"restart_require\" on ModInfo.");
            }

            try {
                this.icon_atlas = (string) values["icon_atlas"];
            } catch (Exception) {
                Logger.Warn("Can't find \"icon_atlas\" on ModInfo.");
            }

            try {
                this.icon = (string) values["icon"];
            } catch (Exception) {
                Logger.Warn("Can't find \"icon\" on ModInfo.");
            }

            try {
                this.server_filter_tags = (string[]) values["server_filter_tags"];
            } catch (Exception) {
                Logger.Warn("Can't find \"server_filter_tags\" on ModInfo.");
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

        public string[] GetTags() {
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
