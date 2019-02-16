using System;
using SteamKit2;
using Indieteur.SAMAPI;
using DSTEd.Core.Klei;
using System.Collections.Generic;
using System.IO;

namespace DSTEd.Core.Steam {
    class Steam {
        private SteamAppsManager software = null;
        private Account account = null;
        private Workshop workshop = null;
        private string path = null;

        public Steam() {
            this.software = new SteamAppsManager();
            this.account = new Account();
            this.workshop = new Workshop();
        }

        public void LoadGame(KleiGame game) {
            Console.WriteLine(string.Format("LoadGame: [#{0}] {1}", game.GetID(), game.GetName()));

            IReadOnlyList<SteamApp> apps = software.SteamApps;
            foreach (SteamApp app in apps) {
                if (game.GetID() == app.AppID) {
                    game.SetPath(app.InstallDir);
                }
            }
        }

        public Boolean ValidatePath(string path) {
            if (path == null) {
                return false;
            }

            return File.Exists(string.Format("{0}{1}Steam.exe", path, Path.DirectorySeparatorChar));
        }

        public Boolean IsInstalled() {
            return this.ValidatePath(this.path);
        }

        public void SetPath(string path) {
            this.path = path;
        }

        public String GetPath() {
            return this.path; //software.InstallDir;
        }

        public Account GetAccount() {
            return this.account;
        }

        public Workshop GetWorkShop() {
            return this.workshop;
        }

        public void GetNews() {
            using(dynamic steamNews = WebAPI.GetInterface("ISteamNews")) {
                KeyValue kvNews = steamNews.GetNewsForApp(appid: 322330);

                foreach(KeyValue news in kvNews["newsitems"]["newsitem"].Children) {
                    Console.WriteLine("News: {0}", news["title"].AsString());
                }
            }
        }
    }
}
