using System;
using System.Windows;
using DSTEd.UI;

namespace DSTEd.Core {
    class DSTEd : Application {
        private String version = "2.0.0";
        private String language = "en_US";
        private IDE ide = null;
        private Workspace workspace = null;
        private Loading loading = null;

        public DSTEd() {
            Logger.Info("Start DSTEd v" + GetVersion());

            Logger.Info("Init Core...");
            this.workspace  = new Workspace();
            this.loading    = new Loading();
            this.ide        = new IDE();

            Logger.Info("Start IDE...");
            this.loading.Show();
            //this.loading.SetProgress(10);
            this.loading.Close();
            this.ide.Show();
            this.Run();
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
