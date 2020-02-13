using System;
using DSTEd.Core.Klei.Data;
using MoonSharp.Interpreter;

namespace DSTEd.Core.LUA {
    public class LUA {
        private Parser parser;

        public LUA() {
            this.parser = new Parser();
        }

        public Parser GetParser() {
            return this.parser;
        }

        public ModInfo GetModInfo(string lua, string file, Action<ParserException> callback) {
            Script result = this.parser.Run(lua, file, false, callback);

            if (result == null) {
                ModInfo info = new ModInfo(null);
                info.SetName(GetModName(lua));
                return info;
            }

            return new ModInfo(result.Globals);
        }

        public string GetModName(string lua) {
            return this.parser.GetVariable("name", lua);
        }
    }
}
