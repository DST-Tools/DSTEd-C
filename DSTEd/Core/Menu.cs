using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Threading;
using DSTEd.UI;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DSTEd.Core {
    public class Menu {
        private IDE ide;
        public Menu(IDE ide) {
            this.ide = ide;
        }

        public void Init() {
            this.Update();
        }

        public void Update() {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, new Action(delegate () {
                if (Boot.Core.IsWorkspaceReady) {
                    Boot.Core.IDE.UpdateWelcome(Boot.Core.Workspace.HasWelcome());
                }
            }));
        }

        public void Handle(string name, MenuItem menu) {
            switch (name) {
                case "FILE_NEW_PROJECT":
                    new ProjectWizard().ShowDialog();
					//todo: add sample project file
					break;
                case "FILE_IMPORT_PROJECT":
                    //todo: ask mod directory and check modinfo.lua exist or not then copy them to game mod directory, open modinfo editor?
                    var dlg = new System.Windows.Forms.FolderBrowserDialog();
                    dlg.Description = I18N.__("Import Project");
                    dlg.ShowNewFolderButton = false;
                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Logger.Info(dlg.SelectedPath);
                    }
                    break;
                case "FILE_NEW_FILE":

					break;
                //case "FILE_NEW_ASSET":
                case "FILE_OPEN":
                    // @ToDo implement with own style (https://github.com/jkells/folder-browser-dialog-example/blob/master/FolderBrowserDialogEx.cs)
                    var dialog = new CommonOpenFileDialog();
                    dialog.Title = I18N.__("Open File");
                    dialog.AllowNonFileSystemItems = false;
                    dialog.Multiselect = false;
                    dialog.RestoreDirectory = false;
                    dialog.InitialDirectory = Boot.Core.Workspace.GetPath();

                    CommonFileDialogResult value = dialog.ShowDialog();

                    if (value == CommonFileDialogResult.Ok) {
                        Boot.Core.Workspace.OpenDocument(dialog.FileName);
                    }
                    break;
                //case "FILE_OPEN_RECENT":
                case "FILE_SAVE":
					Boot.Core.IDE.SaveActiveDocument();
					break;
                case "FILE_SAVE_ALL":
					Boot.Core.IDE.SaveAllDocument();
					break;
                case "FILE_CLOSE":
                    Boot.Core.IDE.CloseActiveDocument();
                    break;
                case "FILE_CLOSE_ALL":
                    Boot.Core.IDE.CloseAllDocuments();
                    break;
                case "FILE_EXIT":
                    Dialog.Open(I18N.__("Do you want to exit DSTEd?"), I18N.__("Exit DSTEd"), Dialog.Buttons.YesNo, Dialog.Icon.Warning, delegate (Dialog.Result result) {
                        if (result == Dialog.Result.Yes) {
                            Environment.Exit(0);
                        }

                        return true;
                    });
                    break;
                case "EDIT_UNDO":
                    if (ide.GetActiveDocument().GetDocument().GetEditorType() == Document.Editor.CODE)
                    {
                        ((Contents.Editors.Code)ide.GetActiveDocument().GetDocument().GetContent()).Undo();
                    }
                    break;
                case "EDIT_REDO":
                    if (ide.GetActiveDocument().GetDocument().GetEditorType() == Document.Editor.CODE)
                    {
                        ((Contents.Editors.Code)ide.GetActiveDocument().GetDocument().GetContent()).Redo();
                    }
                    break;
                case "EDIT_CUT":
                    if (ide.GetActiveDocument().GetDocument().GetEditorType() == Document.Editor.CODE)
                    {
                        ((Contents.Editors.Code)ide.GetActiveDocument().GetDocument().GetContent()).Cut();
                    }
                    break;
                case "EDIT_COPY":
                    if(ide.GetActiveDocument().GetDocument().GetEditorType() == Document.Editor.CODE)
                    {
                        ((Contents.Editors.Code)ide.GetActiveDocument().GetDocument().GetContent()).Copy();
                    }
                    break;
                case "EDIT_PASTE":
                    if (ide.GetActiveDocument().GetDocument().GetEditorType() == Document.Editor.CODE)
                    {
                        ((Contents.Editors.Code)ide.GetActiveDocument().GetDocument().GetContent()).Paste();
                    }
                    break;
                case "EDIT_SELECT_ALL":
                    if (ide.GetActiveDocument().GetDocument().GetEditorType() == Document.Editor.CODE)
                    {
                        ((Contents.Editors.Code)ide.GetActiveDocument().GetDocument().GetContent()).SelectAll();
                    }
                    break;
                //case "SEARCH_FIND":
                //case "SEARCH_FIND_NEXT":
                case "VIEW_WELCOME":
                    Boot.Core.IDE.UpdateWelcome(Boot.Core.Workspace.ToggleWelcome());
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
