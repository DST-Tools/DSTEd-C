using System;
using System.ComponentModel;
using System.Threading;
using DSTEd.Core.Klei.Games;
using DSTEd.Core.Steam;
using DSTEd.UI;

namespace DSTEd.Core {
    public class DSTEd : System.Windows.Application {
		private Loadingv2 loaderv2 = new Loadingv2();//UI
		private Configuration configuration = null;
		public DebugCLICore DBGCLI;
		public IDE IDE { get; private set; } = null;
		public LUA.LUA LUA { get; private set; }
		public Steam.Steam Steam { get; private set; } = null;
		public Workspace Workspace { get; private set; } = null;
		public bool IsWorkspaceReady => this.Workspace != null;
		public string Version { get; } = "2.0.0";
		public string Language
		{
			get
			{
				return this.configuration.Get("LANGUAGE", "en_US");
			}
		}

		public DSTEd()
		{
			DBGCLI = new DebugCLICore();
		}

		public void Start()
		{
			Logger.Info("Start DSTEd v" + Version);

			this.configuration = Configuration.getConfiguration();

			// Init Language
			I18N.SetLanguage(this.Language);

			// Init classes
			this.Steam = new Steam.Steam();
			this.IDE = new IDE();
			this.Workspace = new Workspace();
			//this.loading = new Loading();

			//Set the steam path by configuration
			this.Workspace.SetPath(this.Steam.GetPath());

			this.Workspace.OnSelect(delegate (string path, Boolean save)
			{
				//if (!this.steam.ValidatePath(path))
				//{
				//	Dialog.Open(I18N.__("Bad steam path! Please select the directory of Steam."), I18N.__("Problem"), Dialog.Buttons.OK, Dialog.Icon.Warning, delegate (Dialog.Result result)
				//	{
				//		return true;
				//	});
				//	return;
				//}

				this.Workspace.SetPath(Steam.GetPath());

				if (save)
				{
					this.configuration.Set("STEAM_PATH", path);
					this.configuration.Save();
				}

				this.Workspace.Close(true);
			});

			this.Workspace.OnClose(delegate (CancelEventArgs e) {
				Dialog.Open(I18N.__("You must set the workspace path! If you cancel these, DSTEd will be closed."), I18N.__("Problem"), Dialog.Buttons.RetryCancel, Dialog.Icon.Warning, delegate (Dialog.Result result) {
					if (result == Dialog.Result.Cancel)
					{
						Environment.Exit(0);
						return true;
					}

					e.Cancel = true;
					return true;
				});
			});

			#region Define workers
			void gameloading()
			{
				Steam.LoadGame(new DSTC());//CL
				Steam.LoadGame(new DSTM());//MT
				Steam.LoadGame(new DSTS());//SV
				LUA = new LUA.LUA();
				IDE.Init();
			}
			void modsloading()
			{
				//do nothing now
			}
			void workshoploading()
			{
				Steam.GetWorkShop().GetPublishedMods(322330, delegate (WorkshopItem[] items) {
					Logger.Info("You have " + items.Length + " published Mods on the Steam-Workshop!");

					for (int index = 0; index < items.Length; index++)
					{
						Logger.Info(items[index].ToString());
					}
				});
			}
			#endregion
			Action[] q2 = { gameloading, modsloading, workshoploading };
			loaderv2.Start(q2);
			IDE.Show();
			this.Run();
		}
	}
}
