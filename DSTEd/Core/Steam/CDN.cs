using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace DSTEd.Core.Steam {

    //Singleten Pattern, Use CDN.getInstance() instead of new CDN();
    public class CDN {
        private Configuration configuration;
        private Dictionary<string, string> cdns;
        private string host;
        private readonly string media_cdn_community_url = "/steamcommunity/public";
        private static CDN cdn = new CDN();
        private CDN()
        {
            configuration = Configuration.getConfiguration();
            cdns = new Dictionary<string, string>();
            cdns.Add("akamai", "https://steamcdn-a.akamaihd.net");
            cdns.Add("highwinds", "http://cdn.highwinds.steamstatic.com");
            cdns.Add("edgecast", "http://cdn.edgecast.steamstatic.com");
            cdns.Add("steam", "https://media.st.dl.eccdnx.com");

            string s = configuration.Get("CDN", "akamai");
            cdns.TryGetValue(s, out host);
            Logger.Info("[CDN -> " + s + " ]");
        }

        public string prase(string link)
        {
            return link.Replace("{STEAM_CLAN_IMAGE}", host + media_cdn_community_url + "/images/clans");
        }

        public static CDN getInstance()
        {
            return cdn;
        }
    }
}
