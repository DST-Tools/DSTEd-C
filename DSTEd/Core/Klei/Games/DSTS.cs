namespace DSTEd.Core.Klei.Games {
    class DSTS : KleiGame {
        public DSTS(DSTEd core) : base(core) {
            this.id = 343050;
            this.name = I18N.__("Server");
            this.executable = "bin/dontstarve_dedicated_server_nullrenderer.exe";
        }
    }
}
