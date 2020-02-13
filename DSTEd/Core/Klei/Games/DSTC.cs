namespace DSTEd.Core.Klei.Games {
    class DSTC : KleiGame {
        public DSTC() : base() {
            this.id = 322330;
            this.name = I18N.__("Client");
            this.executable = "bin/dontstarve_steam.exe";
            this.SetMainGame();

            //this.AddDebug("RUN_DST", this.executable);
            //not supported so far. [Akarinnnnn]:try using another process to run?
            /*
                comment by Bizarrus:

                The problem is in the execution.
                Here's what we want to do with the client:
                    - the client should be started by DSTEd
                    - All output and error streams should run through DSTEd
                    - The execution must not be started in a separate, encapsulated process,
                      but must be managed by the main application DSTEd. Otherwise, you cannot
                      attach yourself to the process.
             */
        }
    }
}
