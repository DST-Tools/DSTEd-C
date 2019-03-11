namespace DSTEd.Core.Klei.Games {
    class DSTC : KleiGame {
        public DSTC() : base() {
            this.id = 322330;
            this.name = I18N.__("Client");
            this.executable = "bin/dontstarve_steam.exe";
            this.SetMainGame();

            //this.AddDebug("RUN_DST", this.executable);
			//not supported so far. [Akarinnnnn]:try using another process to run?
        }
    }
}
