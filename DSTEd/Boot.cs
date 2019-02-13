using System;
using DSTEd.Core;

namespace DSTEd {
    class Boot {
        [STAThread]
        public static void Main(String[] args) {
            Logger.Info("Booting up...");
            new Core.DSTEd();
            Console.ReadLine();
        }
    }
}
