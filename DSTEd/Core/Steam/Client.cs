using System;
using System.Threading;
using System.Threading.Tasks;
using SteamKit2;
using static SteamKit2.SteamClient;

namespace DSTEd.Core.Steam {
    public class Client {
        private SteamClient client = null;
        private Boolean connected = false;
        private CallbackManager manager = null;

        internal void AddSubscription(Func<object> p) {
            throw new NotImplementedException();
        }

        public Client() {
            this.client = new SteamClient();
            this.manager = new CallbackManager(this.GetClient());

            this.manager.Subscribe<ConnectedCallback>(delegate (ConnectedCallback c) {
                Logger.Warn("ConnectedCallback");
                this.connected = true;
            });

            this.manager.Subscribe<DisconnectedCallback>(delegate (DisconnectedCallback c) {
                Logger.Warn("DisconnectedCallback");

                if (!c.UserInitiated) {
                    Logger.Warn("Account has been forcefully disconnected!");
                } else {
                    Logger.Info("Account disconnected from Steam.");
                }

                this.connected = false;
                this.Disconnect();
            });

            this.Connect();

            Task.Run(() => {
                while (true) {
                    Thread.Sleep(1000);
                    //if (this.connected) {
                    //Logger.Warn("RUN");
                    this.GetManager().RunCallbacks();
                    //}
                }
            });
        }

        public void Execute(Action callback) {
            this.Connect();

            Task.Run(() => {
                while (true) {
                    Thread.Sleep(500);

                    if (this.connected) {
                        Logger.Warn("Execute...");
                        callback();
                        break;
                    }
                }
            });
        }

        public void Connect() {
            //if (!this.connected) {
            Logger.Warn("CONNECT TRY");
            this.GetClient().Connect();
            //}
        }

        public SteamClient GetClient() {
            return this.client;
        }

        public CallbackManager GetManager() {
            return this.manager;
        }

        public SteamUser GetUserHandler() {
            return this.GetClient().GetHandler<SteamUser>();
        }

        internal void Disconnect() {
            //if (!this.connected) {
            Logger.Warn("DISCONNECT!");
            this.connected = false;
            this.GetClient().Disconnect();
            //}
        }
    }
}
