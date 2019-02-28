using System;
using DSTEd.Core;

namespace DSTEd {
    class Boot {
        public static Core.DSTEd Instance = null;

        [STAThread]
        public static void Main(String[] args) {
            Logger.Info("Booting up...");
            Instance = new Core.DSTEd();
            Instance.Start();
        }

        public static Core.DSTEd Core() {
            return Instance;
        }
    }
}
