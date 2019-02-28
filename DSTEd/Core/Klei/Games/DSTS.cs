namespace DSTEd.Core.Klei.Games {
    class DSTS : KleiGame {
        public DSTS() : base() {
            this.id = 343050;
            this.name = I18N.__("Server");
            this.executable = "bin/dontstarve_dedicated_server_nullrenderer.exe";
        }
    }
}
