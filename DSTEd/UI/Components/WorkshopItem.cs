using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using DSTEd.Core;
using DSTEd.Core.IO;
using DSTEd.Core.LUA;

namespace DSTEd.UI.Components {
    class WorkshopItem : TreeViewItem {
        private FileNode file = null;
        private Core.DSTEd core = null;

        public WorkshopItem(Core.DSTEd core, FileNode file) {
            this.core = core;
            this.file = file;
            this.FontWeight = FontWeights.Bold;

            this.LoadModInfo();
        }

        public Core.DSTEd GetCore() {
            return this.core;
        }

        private void LoadModInfo() {
            string content = null;

            if (!File.Exists(this.file.GetPath() + "/modinfo.lua")) {
                Logger.Error("[WorkshopItem] ModInfo not exists: " + this.file.GetPath());
                return;
            }

            try {
                using (StreamReader reader = new StreamReader(this.file.GetPath() + "/modinfo.lua")) {
                    content = reader.ReadToEnd();
                }
            } catch (IOException) {
                /* Do Nothing */
            }

            if (content == null) {
                Logger.Error("[WorkshopItem] ModInfo is empty or had errors: " + this.file.GetPath());
                return;
            }

            Logger.Error("[WorkshopItem] OK: " + this.file.GetName());
            ModInfo info = LUAInterpreter.GetModInfo(content);
            info.SetID(Int32.Parse(this.file.GetName().Replace("workshop-", "")));
            this.Header = info.GetName();
            // @ToDo Add ID(?)
        }
    }
}
