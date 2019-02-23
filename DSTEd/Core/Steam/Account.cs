using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using SteamKit2;

namespace DSTEd.Core.Steam {
    public class Account {
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

        public byte[] GetHash() {
            if (Properties.Settings.Default.STEAM_HASH.Length == 0) {
                return null;
            }

            return CryptoHelper.SHAHash(Encoding.UTF8.GetBytes(Properties.Settings.Default.STEAM_HASH));
        }

        public string GetKey() {
            if (Properties.Settings.Default.STEAM_KEY.Length == 0) {
                return null;
            }

            return Properties.Settings.Default.STEAM_KEY;
        }

        public void Login(string username, string password, Action<string, Action<string>> guard, Action<string, Boolean> callback) {
            Logger.Info("STEAM_KEY", GetKey());
            Logger.Info("STEAM_HASH", GetHash());

            this.GetClient().GetManager().Subscribe<SteamUser.LoggedOnCallback>(delegate (SteamUser.LoggedOnCallback c) {
                Logger.Info("LoggedOnCallback");
                Console.WriteLine("{0} / {1}", c.Result, c.ExtendedResult);

                if (c.Result == EResult.AccountLogonDenied || c.Result == EResult.AccountLoginDeniedNeedTwoFactor) {
                    Logger.Info("SteamGuard");

                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(delegate () {
                        guard(c.Result == EResult.AccountLoginDeniedNeedTwoFactor ? null : c.EmailDomain, delegate (string code) {
                            this.GetClient().Execute(delegate () {
                                Logger.Info("SteamGuard - Submit");

                                this.user.LogOn(new SteamUser.LogOnDetails {
                                    ShouldRememberPassword = true,
                                    Username = username,
                                    Password = password,
                                    TwoFactorCode = c.Result == EResult.AccountLoginDeniedNeedTwoFactor ? code : null,
                                    AuthCode = c.Result == EResult.AccountLoginDeniedNeedTwoFactor ? null : code,
                                    SentryFileHash = GetHash(),
                                    //LoginKey = GetKey()
                                });
                            });
                        });
                    })).Wait();

                    Logger.Info("LoggedOnCallback - Commit 1");
                    callback(null, this.logged_in);
                    return;
                }

                if (c.Result != EResult.OK) {
                    Console.WriteLine("Unable to logon to Steam: {0} / {1}", c.Result, c.ExtendedResult);
                    return;
                }

                Logger.Info("LoggedOnCallback - Commit 2");
                this.logged_in = true;
                callback(null, this.logged_in);
            });

            this.GetClient().GetManager().Subscribe<SteamUser.LoginKeyCallback>(delegate (SteamUser.LoginKeyCallback c) {
                Logger.Info("LoginKeyCallback");
                Properties.Settings.Default.STEAM_KEY = c.LoginKey;
                this.GetClient().GetUserHandler().AcceptNewLoginKey(c);
                this.GetClient().GetUserHandler().LogOff();
            });

            this.GetClient().GetManager().Subscribe<SteamUser.LoggedOffCallback>(delegate (SteamUser.LoggedOffCallback c) {
                Console.WriteLine($"LoggedOffCallback - {c.Result}");
                this.GetClient().Disconnect();
                this.logged_in = false;
                callback(null, this.logged_in);
            });

            this.GetClient().GetManager().Subscribe<SteamUser.AccountInfoCallback>(delegate (SteamUser.AccountInfoCallback c) {
                Console.WriteLine($"AccountInfoCallback - {c.AccountFlags} {c.PersonaName} {c.CountAuthedComputers}");
            });

            this.GetClient().GetManager().Subscribe<SteamUser.UpdateMachineAuthCallback>(delegate (SteamUser.UpdateMachineAuthCallback c) {
                Console.WriteLine("UpdateMachineAuthCallback");

                using (var sha = SHA1.Create()) {
                    Properties.Settings.Default.STEAM_HASH = Encoding.UTF8.GetString(sha.ComputeHash(c.Data));
                    Console.WriteLine("Update Hash " + Properties.Settings.Default.STEAM_HASH);
                }

                byte[] hash = GetHash();

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
                Console.WriteLine("Auth start");
                this.user.LogOn(new SteamUser.LogOnDetails {
                    //ShouldRememberPassword = true,
                    Username = username,
                    Password = password,
                    SentryFileHash = GetHash(),
                    //LoginKey = GetKey()
                });
            });
        }

        public void Logout(Action callback) {
            callback();
        }
    }
}
