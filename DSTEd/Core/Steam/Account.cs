using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using SteamKit2;

namespace DSTEd.Core.Steam {
    class Account {
        private Steam steam;
        private Boolean logged_in = false;
        private SteamUser user = null;

        public Account(Steam steam) {
            this.steam = steam;
            this.user = this.GetClient().GetUserHandler();
        }

        public Client GetClient() {
            return this.steam.GetClient();
        }

        public void Login(string username, string password, Action<string, Action<string>> guard, Action<string, Boolean> callback) {
            this.GetClient().GetManager().Subscribe<SteamUser.LoggedOnCallback>(delegate(SteamUser.LoggedOnCallback c) {
                if (c.Result == EResult.AccountLogonDenied || c.Result == EResult.AccountLoginDeniedNeedTwoFactor) {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(delegate () {
                        guard(c.Result == EResult.AccountLoginDeniedNeedTwoFactor ? null : c.EmailDomain, delegate (string code) {
                            this.GetClient().Execute(delegate () {
                                this.user.LogOn(new SteamUser.LogOnDetails {
                                    Username = username,
                                    Password = password,
                                    TwoFactorCode = code,
                                    SentryFileHash = CryptoHelper.SHAHash(Encoding.UTF8.GetBytes(Properties.Settings.Default.STEAM_HASH))
                                });
                            });
                        });
                    })).Wait();
                    //callback(null, this.logged_in);
                    return;
                }

                if (c.Result != EResult.OK) {
                    Console.WriteLine("Unable to logon to Steam: {0} / {1}", c.Result, c.ExtendedResult);
                    return;
                }

                this.logged_in = true;
                callback(null, this.logged_in);
            });

            this.GetClient().GetManager().Subscribe<SteamUser.LoggedOffCallback>(delegate(SteamUser.LoggedOffCallback c) {
                this.logged_in = false;
                callback(null, this.logged_in);
            });

            this.GetClient().GetManager().Subscribe<SteamUser.UpdateMachineAuthCallback>(delegate(SteamUser.UpdateMachineAuthCallback c) {
                using (var sha = SHA1.Create()) {
                    Properties.Settings.Default.STEAM_HASH = Encoding.UTF8.GetString(sha.ComputeHash(c.Data));
                }

                byte[] hash = CryptoHelper.SHAHash(Encoding.UTF8.GetBytes(Properties.Settings.Default.STEAM_HASH));

                this.user.SendMachineAuthResponse(new SteamUser.MachineAuthDetails {
                    JobID = c.JobID,
                    FileName = c.FileName,
                    BytesWritten = c.BytesToWrite,
                    FileSize = hash.Length,
                    Offset = c.Offset,
                    Result = EResult.OK,
                    LastError = 0,
                    OneTimePassword = c.OneTimePassword,
                    SentryFileHash = hash
                });
            });

            this.GetClient().Execute(delegate () {
                this.user.LogOn(new SteamUser.LogOnDetails {
                    Username = username,
                    Password = password,
                    SentryFileHash = Properties.Settings.Default.STEAM_HASH.Length == 0 ? null : CryptoHelper.SHAHash(Encoding.UTF8.GetBytes(Properties.Settings.Default.STEAM_HASH))
                });
            });

            this.GetClient().Connect();
        }

        public void Logout(Action callback) {
            callback();
        }
    }
}
