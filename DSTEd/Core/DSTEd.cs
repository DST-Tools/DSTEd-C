using System;
using System.Windows;
using System.Windows.Media;
using DSTEd.Core.Klei.Games;
using DSTEd.UI;
using MLib.Interfaces;

namespace DSTEd.Core {
    class DSTEd : Application {
        private String version = "2.0.0";
        private String language = "en_US";
        private IDE ide = null;
        private Workspace workspace = null;
        private Loading loading = null;
        private Steam.Steam steam = null;

        public DSTEd() {
            Logger.Info("Start DSTEd v" + GetVersion());

            // Init classes
            this.workspace      = new Workspace();
            this.loading        = new Loading();
            this.ide            = new IDE();
            this.steam          = new Steam.Steam();

            this.loading.OnSuccess(delegate () {
                this.loading.Close();
                this.ide.Show();
                this.Run();
            });

            // Adding workers to the loader...
            this.loading.Run("STEAM_PATH", delegate () {
                /*if (this.steam.IsInstalled()) {
                    Logger.Info("Steam is not installed? Ask for Workspace...");
                    this.workspace.Show();
                    this.loading.Wait();
                    return true;
                }*/

                this.workspace.SetPath(this.steam.GetPath());
                Logger.Info("Steam-Path: " + this.steam.GetPath());
                Logger.Info("Installed: " + (this.steam.IsInstalled() ? "Yes" : "No"));
                return true;
            });

            this.loading.Run("KLEI_GAMES", delegate () {
                this.steam.LoadGame(new DSTC());
                this.steam.LoadGame(new DSTS());
                this.steam.LoadGame(new DSTM());
                return true;
            });

            this.loading.Run("KLEI_MODS", delegate () {
                Logger.Info("Load mods...");
                return true;
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
