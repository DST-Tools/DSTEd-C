using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using DSTEd.Core;
using DSTEd.Core.IO;
using DSTEd.Core.Klei.Data;
using DSTEd.Core.LUA;

namespace DSTEd.UI.Components {
    class WorkshopItem : WorkspaceFolderItem {

        public WorkshopItem(FileNode file) : base(file) {
            this.FontWeight = FontWeights.Bold;

            this.LoadModInfo();
        }

        private void LoadModInfo() {
            string content = null;

            if (!File.Exists(this.file.Path + "/modinfo.lua")) {
                Logger.Error("[WorkshopItem] ModInfo not exists: " + this.file.Path);
                return;
            }

            try {
                using (StreamReader reader = new StreamReader(this.file.Path + "/modinfo.lua")) {
                    content = reader.ReadToEnd();
                }
            } catch (IOException) {
                /* Do Nothing */
            }

            if (content == null) {
                Logger.Error("[WorkshopItem] ModInfo is empty or had errors: " + this.file.Path);
                return;
            }

            Logger.Info("[WorkshopItem] OK: " + this.file.Name);
            ModInfo info = Boot.Core.LUA.GetModInfo(content, this.file.Path + "/modinfo.lua", delegate (ParserException e) {
                Logger.Error("[WorkshopItem] ModInfo is broken: " + e);
            });

            info.SetID(UInt64.Parse(this.file.Name.Replace("workshop-", "")));

            if (info.IsBroken() && !info.HasName()) {
                //this.Header = this.file.GetName(); //set at base constructs
            } else {
                this.Header = info.GetName();
            }
            // @ToDo Add ID(?)
        }
    }
}
