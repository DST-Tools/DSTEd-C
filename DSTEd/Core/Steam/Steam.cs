using System;
using System.Collections.Generic;
using System.IO;
using DSTEd.Core.Klei;
using Indieteur.SAMAPI;
using SteamKit2;

namespace DSTEd.Core.Steam {
    public class Steam {
        private SteamAppsManager software = null;
        private Account account = null;
        private Workshop workshop = null;
        private string path = null;
        private List<KleiGame> games = new List<KleiGame>();

        public Steam() {
            this.software = new SteamAppsManager();
            this.account = new Account(this);
            this.workshop = new Workshop();
			path = software.InstallDir;
        }

        public void LoadGame(KleiGame game) {
            Logger.Info(string.Format("LoadGame: [#{0}] {1}", game.GetID(), game.GetName()));

            IReadOnlyList<SteamApp> apps = software.SteamApps;
            foreach (SteamApp app in apps) {
                if (game.GetID() == app.AppID) {
                    game.SetPath(app.InstallDir);
                }
            }

            games.Add(game);
        }

        public List<KleiGame> GetGames() {
            return this.games;
        }

		public KleiGame GetGame() {
			KleiGame game = null;

			foreach (KleiGame instance in this.games) {
				if (instance.IsMainGame()) {
					game = instance;
					break;
				}
			}

			return game;
		}
		[Obsolete("Always true.")]
        public Boolean ValidatePath(string path) {
			return true;
        }
		[Obsolete("Always true.")]
		public Boolean IsInstalled() {
			return true;
		}
		[Obsolete("Path was set in the contructor.Just not call this.",true)]
        public void SetPath(string path) {
			
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

        public void GetNews(Action<List<KeyValue>> callback) {
            using (dynamic steamNews = WebAPI.GetInterface("ISteamNews")) {
                try
                {
                    KeyValue kvNews = steamNews.GetNewsForApp(appid: 322330);
                    callback(kvNews["newsitems"]["newsitem"].Children);
                }
                catch (System.Net.Http.HttpRequestException e)
                {
                    if(e.HResult == -2146233079)//hex will cause some problems........
                    {
                        //this HResult means invalid SSL cert
                        var ProcessList = System.Diagnostics.Process.GetProcesses();
                        foreach (System.Diagnostics.Process P in ProcessList)
                        {
                            if(P.ProcessName == "steamcommunity_302.caddy")
                            {
                                //中国特色
                                //no necessary of I18N, only steam users in china use this
                                Logger.Warn("Steamcommunity302证书出错");
                                UI.Dialog.Open("获取新闻失败，请到steamcommunity302重新生成代理证书！"
                                    , "警告", UI.Dialog.Buttons.OK);
                                return;
                            }
                        }
                        Logger.Warn("Invalid steam SSL Cert.");
                    }
                    else
                    {
                        //do nothing.
                    }
                }
            }
        }
    }
}
