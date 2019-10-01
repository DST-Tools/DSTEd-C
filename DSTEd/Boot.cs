using System;
using DSTEd.Core;

namespace DSTEd {
    class Boot {
		[STAThread]
        public static void Main(String[] args) {
            Logger.Info("Booting up...");
            Core = new Core.DSTEd();
            Core.Start();
        }

		public static Core.DSTEd Core { get; private set; } = null;
	}
}
