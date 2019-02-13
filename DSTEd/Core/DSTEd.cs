using System;
using System.Threading;
using System.Windows;
using DSTEd.UI;

namespace DSTEd.Core {
    class DSTEd : Application {
        private String version = "2.0.0";
        private String language = "en_US";
        private IDE ide = null;
        private Workspace workspace = null;
        private Loading loading = null;
        private Steam.Steam steam = null;
        private Steam.Account steam_account = null;
        private Steam.Workshop steam_workshop = null;

        public DSTEd() {
            Logger.Info("Start DSTEd v" + GetVersion());

            // Init classes
            this.workspace      = new Workspace();
            this.loading        = new Loading();
            this.ide            = new IDE();
            this.steam          = new Steam.Steam();
            this.steam_account  = new Steam.Account();
            this.steam_workshop = new Steam.Workshop();//try use ISteamRemoteStorage?

            this.loading.OnSuccess(delegate () {
                this.loading.Close();
                this.ide.Show();
                this.Run();
            });

            // Adding workers to the loader...
            this.loading.Run("STEAM_PATH", delegate () {
                Logger.Info("Get Steam path...");
                Thread.Sleep(100);
            });

            this.loading.Run("KLEI_GAME", delegate () {
                Logger.Info("Get Klei path...");
                Thread.Sleep(100);
            });

            this.loading.Run("KLEI_MODS", delegate () {
                Logger.Info("Load mods...");
                Thread.Sleep(100);
            });

            this.loading.Start();
        }

        public String GetVersion() {
            return this.version;
        }

        public String GetLanguage() {
            // @ToDo Read from settings, otherwise fallback
            return this.language;
        }
    }
}
