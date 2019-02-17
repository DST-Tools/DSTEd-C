using System;
using System.Collections.Generic;
using System.IO;
using DSTEd.Core.Klei;
using DSTEd.UI;
using Indieteur.SAMAPI;
using SteamKit2;

namespace DSTEd.Core.Steam {
    class Steam {
        private SteamAppsManager software = null;
        private Account account = null;
        private Workshop workshop = null;
        private string path = null;
        private Client client = null;

        public Steam() {
            this.client = new Client();
            this.software = new SteamAppsManager();
            this.account = new Account(this);
            this.workshop = new Workshop();

            /*this.account.Login("adi_does", "40dexxa35", delegate (string email, Action<string> callback) {
                if (email == null) {
                    Dialog.Open("SteamGuard App", "Steam Guard", Dialog.Buttons.OK, Dialog.Icon.Asterisk, delegate (Dialog.Result result) {
                        return true;
                    });
                } else {
                    Dialog.Open("SteamGuard E - Mail(" + email + ")", "Steam Guard", Dialog.Buttons.OK, Dialog.Icon.Asterisk, delegate (Dialog.Result result) {
                        return true;
                    });
                }

                callback("CODE");
            }, delegate (string error, Boolean logged_in) {

                Logger.Info("LOGIN", error, logged_in);
            });*/
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

            if (this.ValidatePath(this.path) || this.path == null) {
                this.path = software.InstallDir;
            }
        }

        public Client GetClient() {
            return this.client;
        }

        public String GetPath() {
            return this.path;
        }

        public Account GetAccount() {
            return this.account;
        }

        public Workshop GetWorkShop() {
            return this.workshop;
        }

        public void GetNews() {
            using (dynamic steamNews = WebAPI.GetInterface("ISteamNews")) {
                KeyValue kvNews = steamNews.GetNewsForApp(appid: 322330);

                foreach (KeyValue news in kvNews["newsitems"]["newsitem"].Children) {
                    Console.WriteLine("News: {0}", news["title"].AsString());
                }
            }
        }
    }
}
