using System.Collections.Generic;
using System.Threading.Tasks;
using SteamKit2;
namespace DSTEd.Core.Steam
{
    class Workshop {
        private SteamWorkshop WorkShopHandler;
        private SteamWorkshop.PublishedFilesCallback filesCallback;
        public async Task<SteamWorkshop.PublishedFilesCallback> Quary(uint startindex = 0,uint count = 9,SteamKit2.EWorkshopEnumerationType how = EWorkshopEnumerationType.Recent)
        {
            throw new System.NotImplementedException("");

            var details = new SteamWorkshop.EnumerationDetails();
            details.AppID = 332330;
            details.AppID = count;
            details.StartIndex = startindex;
            details.Type = how;
            SteamWorkshop.PublishedFilesCallback publishedFiles = await WorkShopHandler.EnumeratePublishedFiles(details);
            filesCallback = publishedFiles;
            return publishedFiles;
        }
    }
}
