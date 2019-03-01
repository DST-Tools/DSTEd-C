using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.CoreLib;
using MoonSharp.Interpreter.Interop;

/*
 * http://www.moonsharp.org/MoonSharpStdLib.pdf
 */
namespace DSTEd.Core.LUA {
    public class Parser {
        private string lua = null;
        private Loader loader;

        public Parser() {
            this.loader = new Loader();

            try {
                using (StreamReader reader = new StreamReader(this.loader.GetPath() + "main.lua", true)) {
                    this.lua = string.Format("package.path = \"{0}\"", this.loader.GetPaths());
                    this.lua = reader.ReadToEnd().Replace("package.path", "-- package.path");
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

        private Script Inject(Script script, Action<ParserException> callback) {
            try {
                script.Options.ScriptLoader = this.loader;
                script.DebuggerEnabled = true;
                script.Options.DebugPrint = s => { Console.WriteLine(s); };
                UserData.RegistrationPolicy = InteropRegistrationPolicy.Automatic;
                UserData.RegisterType<TheSim>(InteropAccessMode.Reflection, "TheSim");
                TheSim sim = new TheSim();
                UserData.RegisterAssembly();

                script.Globals["TheSim"] = UserData.Create(sim);
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
            } catch (InternalErrorException e) {
                if (this.lua == null) {
                    callback?.Invoke(new ParserException(e, lua));
                } else {
                    callback?.Invoke(new ParserException(e, this.lua + "\n\n" + lua));
                }
            } catch (InterpreterException e) {
                if (this.lua == null) {
                    callback?.Invoke(new ParserException(e, lua));
                } else {
                    callback?.Invoke(new ParserException(e, this.lua + "\n\n" + lua));
                }
            }
            return script;
        }

        public Script Run(string lua, Boolean injector, Action<ParserException> callback) {
            try {
                Script script = new Script();

                if (injector) {
                    script = this.Inject(script, callback);
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
            } catch (InternalErrorException e) {
                if (this.lua == null) {
                    callback?.Invoke(new ParserException(e, lua));
                } else {
                    callback?.Invoke(new ParserException(e, this.lua + "\n\n" + lua));
                }
            } catch (InterpreterException e) {
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
