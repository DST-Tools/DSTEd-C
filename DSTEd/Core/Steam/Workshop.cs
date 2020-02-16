using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Steamworks;

/*
 * https://partner.steamgames.com/doc/features/workshop/implementation
 */
namespace DSTEd.Core.Steam {
    public class WorkshopItem {
        private string title = null;
        private string description = null;
        private string url = null;

        public WorkshopItem(SteamUGCDetails_t details) {
            //details.m_bAcceptedForUse;
            //details.m_bBanned;
            //details.m_bTagsTruncated;
            //details.m_eFileType;
            //details.m_eResult;
            //details.m_eVisibility;
            //details.m_flScore;
            //details.m_hFile;
            //details.m_hPreviewFile;
            //details.m_nConsumerAppID;
            //details.m_nCreatorAppID;
            //details.m_nFileSize;
            //details.m_nPreviewFileSize;
            //details.m_nPublishedFileId;
            //details.m_pchFileName;
            this.description = details.m_rgchDescription;
            //details.m_rgchTags;
            this.title = details.m_rgchTitle;
            this.url = details.m_rgchURL;
            //details.m_rtimeAddedToUserList;
            //details.m_rtimeCreated;
            //details.m_rtimeUpdated;
            //details.m_ulSteamIDOwner;
            //details.m_unNumChildren;
            //details.m_unVotesDown;
            //details.m_unVotesUp;
        }

        public new string ToString() {
            return string.Format("[WorkshopItem Title=\"{0}\" URL=\"{1}\" Description=\"{2}\"]", this.title, this.url, this.description);
        }
    }

    public class Workshop {
        private List<Action> queue;
        private Boolean initalized = false;
        private Boolean running = false;

        public Workshop() {
            this.queue = new List<Action>();

            Task.Delay(2500);

            Task.Run(() => {
                while (true) {
                    this.Init();
                    Thread.Sleep(5000);
                }
            });
        }

        public AccountID_t GetAccountID() {
            return SteamUser.GetSteamID().GetAccountID();
        }

        public CSteamID GetSteamID() {
            return SteamUser.GetSteamID();
        }

        string GetSteamImageAsTexture2D(int iImage)
        {
            uint ImageWidth;
            uint ImageHeight;
            bool bIsValid = SteamUtils.GetImageSize(iImage, out ImageWidth, out ImageHeight);

            if (bIsValid)
            {
                byte[] Image = new byte[ImageWidth * ImageHeight * 4];

                bIsValid = SteamUtils.GetImageRGBA(iImage, Image, (int)(ImageWidth * ImageHeight * 4));
                if (bIsValid)
                {
                    string result = Convert.ToBase64String(Image);
                    return "data:image/jpeg;base64," + result;
                }
            }

            return null;
        }

        public string GetPicture()
        {
            // @ToDo wont work, get the avatar of profile
            int test = SteamFriends.GetSmallFriendAvatar(this.GetSteamID());
           return this.GetSteamImageAsTexture2D(test);
        }

        public string GetUsername() {
            return SteamFriends.GetPersonaName();
        }

        public AppId_t GetCreatorApp() {
            return new AppId_t((uint) (245850 & 0xFFFFFFul));
        }

        public void Init() {
            if (!SteamAPI.Init() && !this.initalized)
            {
                Logger.Info("Steam-INIT FAILED! Retry in 5 Seconds...");
                if (Boot.Core.IDE != null)
                {
                    Boot.Core.IDE.SetSteamProfile(null);
                }
                return;
            }

            this.initalized = true;

            if (SteamAPI.IsSteamRunning() && !this.running) {
                this.running = true;

                Task.Run(() => {
                    while (true) {
                        if (this.queue.Count >= 1) {
                            Logger.Info("Enqueue Call!");
                            Action callback = this.queue[0];
                            callback();
                            this.queue.RemoveAt(0);
                            Thread.Sleep(100);
                        }

                        SteamAPI.RunCallbacks();
                        Thread.Sleep(100);
                    }
                });

                Logger.Info("ID: " + this.GetSteamID() + ", Name: " + this.GetUsername() + ", Account: " + this.GetAccountID());
                if (Boot.Core.IDE != null)
                {
                    Boot.Core.IDE.SetSteamProfile(this);
                }
            }
        }

        public void Call(Action callback) {
            this.queue.Add(callback);
        }

        public void GetPublishedMods(int app_id, Action<WorkshopItem[]> callback) {
            this.Call(delegate () {
                EUserUGCList list = EUserUGCList.k_EUserUGCList_Published;
                EUGCMatchingUGCType type = EUGCMatchingUGCType.k_EUGCMatchingUGCType_All;
                EUserUGCListSortOrder order = EUserUGCListSortOrder.k_EUserUGCListSortOrder_CreationOrderAsc;
                uint page = 1; //@ToDo iterate/count up! A single page has a maximum of 50 items. is the result-count under 50, it's the last page!
                UGCQueryHandle_t handle = SteamUGC.CreateQueryUserUGCRequest(this.GetAccountID(), list, type, order, this.GetCreatorApp(), new AppId_t((uint) ((uint) app_id & 0xFFFFFFul)), page);

                CallResult<SteamUGCQueryCompleted_t>.Create(delegate (SteamUGCQueryCompleted_t pCallback, bool bIOFailure) {
                    WorkshopItem[] result = new WorkshopItem[pCallback.m_unTotalMatchingResults];

                    for(uint index = 0; index < pCallback.m_unTotalMatchingResults; index++) {
                        SteamUGCDetails_t details;

                        if (SteamUGC.GetQueryUGCResult(handle, index, out details)) {
                            result[index] = new WorkshopItem(details);
                        }
                    }

                    callback(result);
                    SteamUGC.ReleaseQueryUGCRequest(handle);
                }).Set(SteamUGC.SendQueryUGCRequest(handle));
            });
        }
    }
}
