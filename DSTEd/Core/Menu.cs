using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using DSTEd.UI;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DSTEd.Core {
    public class Menu {
        private IDE ide = null;

        public Menu(IDE ide) {
            this.ide = ide;
        }

        public IDE GetIDE() {
            return this.ide;
        }

        public void Handle(string name, MenuItem menu) {
            switch (name) {
                //case "FILE_NEW_PROJECT":
                //case "FILE_NEW_FILE":
                //case "FILE_NEW_ASSET":
                case "FILE_OPEN":
                    // @ToDo implement with own style (https://github.com/jkells/folder-browser-dialog-example/blob/master/FolderBrowserDialogEx.cs)
                    var dialog = new CommonOpenFileDialog();
                    dialog.Title = I18N.__("Open File");
                    dialog.AllowNonFileSystemItems = false;
                    dialog.Multiselect = false;
                    dialog.RestoreDirectory = false;
                    dialog.InitialDirectory = this.GetIDE().GetCore().GetWorkspace().GetPath();

                    CommonFileDialogResult value = dialog.ShowDialog();

                    if (value == CommonFileDialogResult.Ok) {
                        this.GetIDE().GetCore().GetWorkspace().OpenDocument(dialog.FileName);
                    }
                    break;
                //case "FILE_OPEN_RECENT":
                //case "FILE_SAVE":
                //case "FILE_SAVE_ALL":
                //case "FILE_CLOSE":
                //case "FILE_CLOSE_ALL":
                case "FILE_EXIT":
                    Dialog.Open(I18N.__("Do you want to exit DSTEd?"), I18N.__("Exit DSTEd"), Dialog.Buttons.YesNo, Dialog.Icon.Warning, delegate (Dialog.Result result) {
                        if (result == Dialog.Result.Yes) {
                            Environment.Exit(0);
                        }

                        return true;
                    });
                    break;
                //case "EDIT_UNDO":
                //case "EDIT_REDO":
                //case "EDIT_CUT":
                //case "EDIT_COPY":
                //case "EDIT_PASTE":
                //case "EDIT_SELECT_ALL":
                //case "SEARCH_FIND":
                //case "SEARCH_FIND_NEXT":
                //case "VIEW":
                //case "DEBUG_RUN_DST":
                //case "TOOLS_STEAM":
                //case "TOOLS_SERVER":
                //case "SETTINGS":
                case "HELP_FORUM":
                    Process.Start("https://forums.kleientertainment.com/forums/topic/78739-dsted-the-ide-for-dont-starve-together/");
                    break;
                //case "HELP_ABOUT":
                case "HELP_FEEDBACK":
                    Process.Start("https://github.com/DST-Tools/DSTEd-C/issues");
                    break;
                //case "STEAM":
                case "STEAM_LOGIN":
                    this.GetIDE().GetCore().GetLogin().ShowDialog();
                    break;
                //case "STEAM_SETTINGS":
                default:
                    Logger.Warn("[Menu] Entry is not implemented: " + name);
                    break;
            }
        }
    }
}
