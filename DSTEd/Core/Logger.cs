using System;
using System.Linq;

namespace DSTEd.Core {
    public static class Logger {
        public static void Call(string type, object[] args) {
            string[] parameters = new string[args.Length];

            for(int index = 0; index < args.Length; index++) {
                object value = args[index];

                if (value == null) {
                    parameters[index] = "NULL";
                } else if (value.GetType() == typeof(int)) {
                    parameters[index] = value.ToString();
                } else if (value.GetType() == typeof(string)) {
                    parameters[index] = (string) value;
                } else {
                    parameters[index] = value.ToString();
                }
            }

            Console.WriteLine(string.Format("[{0}] {1:HH:mm:ss tt} - {2}", type, DateTime.Now, string.Join(" ", parameters)));
            // @ToDo write log files
        }

        public static void Info(params object[] args) {
            Call("INFO", args);
        }

        public static void Error(params object[] args) {
            Call("ERROR", args);
        }

        public static void Warn(params object[] args) {
            Call("WARN", args);
        }
    }
}
