using System;
using System.IO;
using System.Text.RegularExpressions;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using MoonSharp.Interpreter.Loaders;

namespace DSTEd.Core.LUA {
    public class Parser {
        private string lua = null;

        public Parser() {
            try {
                using (StreamReader reader = new StreamReader("D:/Software/Steam/SteamApps/common/Don't Starve Together/data/scripts/main.lua", true)) {
                    this.lua = "package.loaders = {}\n\n" + reader.ReadToEnd();
                }
            } catch (IOException) {
                /* Do Nothing */
            }
        }

        public string GetVariable(string name, string lua) {
            try {
                Match match = new Regex(name + "(:?[\\s]+)?=(:?[\\s]+)?\"(.*)\"").Match(lua);

                if (match.Success) {
                    return match.Groups[match.Groups.Count - 1].Value;
                }
            } catch (Exception) {
                /* Do Nothing */
            }

            return null;
        }

        private Script Inject(Script script) {
            EmbeddedResourcesScriptLoader a = new EmbeddedResourcesScriptLoader();
            a.ModulePaths = new string[] {
                "D:\\Software\\Steam\\SteamApps\\common\\Don't Starve Together\\data\\scripts\\?.lua"
            };
            Script.DefaultOptions.ScriptLoader = a;
            Script.DefaultOptions.DebugPrint = Console.WriteLine;
            UserData.RegistrationPolicy = InteropRegistrationPolicy.Automatic;
            UserData.RegisterType<TheSim>(InteropAccessMode.Reflection, "TheSim");
            TheSim sim = new TheSim();
            UserData.RegisterAssembly();
            
            script.Globals["TheSim"] = UserData.Create(sim);

            return script;
        }

        public Script Run(string lua, Boolean injector, Action<ParserException> callback) {
            try {
                Script script = new Script();

                if (injector) {
                    script = this.Inject(script);
                }

                if (this.lua == null || !injector) {
                    script.DoString(lua);
                } else {
                    script.DoString(this.lua + "\n\n" + lua);
                }

                return script;
            } catch (ScriptRuntimeException e) {
                if (this.lua == null) {
                    callback?.Invoke(new ParserException(e, lua));
                } else {
                    callback?.Invoke(new ParserException(e, this.lua + "\n\n" + lua));
                }
            } catch (SyntaxErrorException e) {
                if (this.lua == null) {
                    callback?.Invoke(new ParserException(e, lua));
                } else {
                    callback?.Invoke(new ParserException(e, this.lua + "\n\n" + lua));
                }
            }

            return null;
        }
    }
}
