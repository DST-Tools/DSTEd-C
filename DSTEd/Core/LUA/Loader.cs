using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

namespace DSTEd.Core.LUA {
    class Loader : ScriptLoaderBase {
        public Loader() {
            this.ModulePaths = new string[] {
                this.GetPath() + "?",
                this.GetPath() + "?.lua",
                this.GetPath() + "?/?.lua"
            };

            Logger.Info("LUA PATHS: " + this.GetPath(), this.GetPaths());
        }

        public string GetPath() {
            return string.Format("{0}/data/scripts/", Boot.Core().GetSteam().GetGame().GetPath());
        }

        public string GetPaths() {
            return string.Join(";", this.ModulePaths);
        }

        public override object LoadFile(string file, Table globalContext) {
            return string.Format("print ([[A request to load '{0}' has been made]])", file);
			/* Lua regex string "%?" main will cause some problems
			 * but not happened in DST's Lua interpreter????
			 * sepical version???
			 */
			
        }

        public override bool ScriptFileExists(string name) {
            return true;
        }
    }
}
