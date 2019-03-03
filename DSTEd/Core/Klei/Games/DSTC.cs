namespace DSTEd.Core.Klei.Games {
    class DSTC : KleiGame {
        public DSTC() : base() {
            this.id = 322330;
            this.name = I18N.__("Client");
            this.executable = "bin/dontstarve_steam.exe";
            this.SetMainGame();

            this.AddDebug("DST_CLIENT", null);
            this.AddSubDebug("DST_CLIENT", "RUN_DST", this.executable);
        }
    }
}
