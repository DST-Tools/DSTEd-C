namespace DSTEd.Core.Klei.Games {
    class DSTC : KleiGame {
        public DSTC(DSTEd core) : base(core) {
            this.id = 322330;
            this.name = I18N.__("Client");
            this.executable = "bin/dontstarve_steam.exe";
            this.SetMainGame();
        }
    }
}
