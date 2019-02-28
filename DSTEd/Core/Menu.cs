using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using DSTEd.UI;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DSTEd.Core {
    public class Menu {
        public Menu() {}

        public void Init() {
            this.Update();
        }

        public void Update() {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.DataBind, new Action(delegate () {
                if (Boot.Core().IsWorkspaceReady()) {
                    Boot.Core().GetIDE().UpdateWelcome(Boot.Core().GetWorkspace().HasWelcome());
                }
            }));
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
                    dialog.InitialDirectory = Boot.Core().GetWorkspace().GetPath();

                    CommonFileDialogResult value = dialog.ShowDialog();

                    if (value == CommonFileDialogResult.Ok) {
                        Boot.Core().GetWorkspace().OpenDocument(dialog.FileName);
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
                case "VIEW_WELCOME":
                    Boot.Core().GetIDE().UpdateWelcome(Boot.Core().GetWorkspace().ToggleWelcome());
                    break;
                //case "DEBUG_RUN_DST":
                //case "TOOLS_STEAM":
                //case "TOOLS_SERVER":
                //case "SETTINGS":
                case "HELP_FORUM":
                    Process.Start("https://forums.kleientertainment.com/forums/topic/78739-dsted-the-ide-for-dont-starve-together/");
                    break;
                case "HELP_ABOUT":
                    new About().ShowDialog();
                break;
                case "HELP_FEEDBACK":
                    Process.Start("https://github.com/DST-Tools/DSTEd-C/issues");
                    break;
                //case "STEAM":
                //case "STEAM_SETTINGS":
                default:
                    Logger.Warn("[Menu] Entry is not implemented: " + name);
                    break;
            }
        }
    }
}
