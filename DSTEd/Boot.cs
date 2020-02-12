using System;
using DSTEd.Core;

namespace DSTEd {
    class Boot {
		[STAThread]
        public static void Main(String[] args) {

            for (int i = 0; i < args.Length; i++)
            {
                if(args[i] == "-log")
                {
                    i++;
                    Logger.LogFile = args[i];
                }
            }

            Logger.Info("Booting up...");
            Core = new Core.DSTEd();
            Core.Start();
        }

		public static Core.DSTEd Core { get; private set; } = null;
	}
}
