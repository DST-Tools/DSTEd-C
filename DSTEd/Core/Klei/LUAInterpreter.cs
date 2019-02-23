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

        public ModInfo(Table values) {
            try {
                this.name = (string) values["name"];
            } catch (Exception) { }

            try {
                this.version = (string) values["version"];
            } catch (Exception) { }

            try {
                this.description = (string) values["description"];
            } catch (Exception) { }

            try {
                this.author = (string) values["author"];
            } catch (Exception) { }

            try {
                this.forumthread = (string) values["forumthread"];
            } catch (Exception) { }

            try {
                this.api_version = (int) values["api_version"];
            } catch (Exception) { }

            try {
                this.dont_starve_compatible = (bool) values["dont_starve_compatible"];
            } catch (Exception) { }

            try {
                this.all_clients_require_mod = (bool) values["all_clients_require_mod"];
            } catch (Exception) { }

            try {
                this.dst_compatible = (bool) values["dst_compatible"];
            } catch (Exception) { }

            try {
                this.standalone = (bool) values["standalone"];
            } catch (Exception) { }

            try {
                this.restart_require = (bool) values["restart_require "];
            } catch (Exception) { }

            try {
                this.icon_atlas = (string) values["icon_atlas"];
            } catch (Exception) { }

            try {
                this.icon = (string) values["icon"];
            } catch (Exception) { }

            try {
                this.server_filter_tags = (string[]) values["server_filter_tags"];
            } catch (Exception) { }
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
        public static Script Run(string lua) {
            Script script = new Script();
            script.DoString(lua);
            return script;
        }

        public static ModInfo GetModInfo(string lua) {
            return new ModInfo(LUAInterpreter.Run(lua).Globals);
        }
    }
}
