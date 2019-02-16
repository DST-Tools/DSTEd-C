using System;
using System.Net.Http;
using SteamKit2;

namespace DSTEd.Core.Steam {
    class Account {
        public async System.Threading.Tasks.Task LoginAsync(Action callback) {
           
            using (dynamic steamUserAuth = WebAPI.GetInterface("ISteamUserAuth", "6AE9A518112DC2C2D67EDF9BCCE6FC88")) {
                // as the interface functions are synchronous, it may be beneficial to specify a timeout for calls
                steamUserAuth.Timeout = TimeSpan.FromSeconds(5);

                // additionally, if the API you are using requires you to POST,
                // you may specify with the "method" reserved parameter
                try {
                    steamUserAuth.AuthenticateUser(someParam: "someValue", method: HttpMethod.Post);
                } catch (Exception ex) {
                    Console.WriteLine("Unable to make AuthenticateUser API Request: {0}", ex.Message);
                }
            }

        }

        public void Logout(Action callback) {
            callback();
        }
    }
}
