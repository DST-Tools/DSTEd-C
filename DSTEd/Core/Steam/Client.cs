using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SteamKit2;
using static SteamKit2.SteamClient;
using static SteamKit2.SteamUser;
using SteamKit2.Internal;

namespace DSTEd.Core.Steam {
    class Client {
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
                this.connected = true;
            });

            this.manager.Subscribe<DisconnectedCallback>(delegate (DisconnectedCallback c) {
                if (!c.UserInitiated) {
                    Logger.Warn("Account has been forcefully disconnected!");
                } else {
                    Logger.Info("Account disconnected from Steam.");
                }

                this.connected = false;
            });
            
            //this.GetClient().Connect();
        }

        public void Execute(Action callback) {
            Task.Run(() => {
                while (true) {
                    Thread.Sleep(500);

                    if (this.connected) {
                        callback();
                        break;
                    }
                }
            });
        }

        public void Connect() {
            this.GetClient().Connect();

            Task.Run(() => {
                while (true) {
                    this.manager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
                }
            });
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
    }
}
