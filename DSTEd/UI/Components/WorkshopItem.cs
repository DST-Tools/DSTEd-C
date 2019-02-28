using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using DSTEd.Core;
using DSTEd.Core.IO;
using DSTEd.Core.LUA;
using MoonSharp.Interpreter;

namespace DSTEd.UI.Components {
    class WorkshopItem : TreeViewItem {
        private FileNode file = null;

        public WorkshopItem(FileNode file) {
            this.file = file;
            this.FontWeight = FontWeights.Bold;

            this.LoadModInfo();
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

            ModInfo info = LUAInterpreter.GetModInfo(content, delegate(SyntaxErrorException e) {
                Logger.Error("[WorkshopItem] ModInfo is broken: " + e.Message + "\n" + e.StackTrace);
            });

            info.SetID(Int32.Parse(this.file.GetName().Replace("workshop-", "")));

            if (info.IsBroken() && !info.HasName()) {
                this.Header = this.file.GetName();
            } else {
                this.Header = info.GetName();
            }
            // @ToDo Add ID(?)
        }
    }
}
