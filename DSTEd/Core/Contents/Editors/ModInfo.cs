﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DSTEd.Core.LUA;
using DSTEd.UI;
using MoonSharp.Interpreter;

/*
 * https://forums.kleientertainment.com/forums/topic/36748-mod-api-updates-api-version-6-may-23-2014/?page=0&tab=comments#comment-486895
 */
namespace DSTEd.Core.Contents.Editors {
    class ModInfo : TabControl {
        private Document document;

        public ModInfo(Document document) {
            this.document = document;
            this.TabStripPlacement = Dock.Bottom;
            this.BorderBrush = Brushes.Transparent;
            this.BorderThickness = new Thickness(0);
            this.Background = Brushes.Transparent;

            this.CreatePropertiesEditor();
            this.CreateSourceEditor();
        }

        private void CreatePropertiesEditor() {
            Properties properties = new Properties();

            try {
                LUA.ModInfo info = LUAInterpreter.GetModInfo(this.document.GetFileContent(), delegate(SyntaxErrorException e) {
                    throw e;
                });

                if (info.IsBroken()) {
                    return;
                }

                // Build Properties-Editor
                properties.AddCategory(I18N.__("Informations"));
                properties.AddEntry("name", I18N.__("Name"), Properties.Type.STRING, info.GetName());
                properties.AddEntry("version", I18N.__("Version"), Properties.Type.STRING, info.GetVersion());
                properties.AddEntry("description", I18N.__("Description"), Properties.Type.TEXT, info.GetDescription());
                properties.AddEntry("author", I18N.__("Author"), Properties.Type.STRING, info.GetAuthor());
                properties.AddEntry("forumthread", I18N.__("Forum-Thread"), Properties.Type.URL, info.GetForumThread());
                properties.AddEntry("server_filter_tags", I18N.__("Tags"), Properties.Type.STRINGLIST, info.GetTags());

                properties.AddCategory(I18N.__("Assets"));
                properties.AddEntry("icon_atlas", I18N.__("Icon (Atlas)"), Properties.Type.ATLAS, info.GetIconAtlas());
                properties.AddEntry("icon", I18N.__("Icon (Texture)"), Properties.Type.KTEX, info.GetIcon());

                properties.AddCategory(I18N.__("Requirements"));
                properties.AddEntry("api_version", I18N.__("API Version"), Properties.Type.SELECTION, new Selection(new Dictionary<object, string> {
               { 6, "Don't Starve" },
               { 10, "Don't Starve Together" }
            }, info.GetAPIVersion()));
                properties.AddEntry("dont_starve_compatible", I18N.__("Don't Starve"), Properties.Type.YESNO, info.IsDS());
                properties.AddEntry("dst_compatible", I18N.__("Don't Starve Together"), Properties.Type.YESNO, info.IsDST());
                properties.AddEntry("reign_of_giants_compatible", I18N.__("Reign of Giants"), Properties.Type.YESNO, info.IsROG());
                properties.AddEntry("standalone", I18N.__("Standalone"), Properties.Type.YESNO, info.ModsAllowed());
                properties.AddEntry("all_clients_require_mod", I18N.__("All Clients Required"), Properties.Type.YESNO, info.IsRequired());
                properties.AddEntry("restart_require", I18N.__("Restart Required"), Properties.Type.YESNO, info.MustRestart());
            } catch(SyntaxErrorException e) {
                string message = e.DecoratedMessage;

                try {
                    Match result = new Regex("^chunk_([0-9]+):\\(([0-9]+),([0-9\\-]+)\\): (.*)$").Match(e.DecoratedMessage);

                    if(result.Success) {
                        message = string.Format("Line {0} with Position {1}:\n{2}", result.Groups[2].Value, result.Groups[3].Value, result.Groups[4].Value);
                    }
                } catch(Exception) {
                    /* Do Nothing */
                }

                Dialog.Open("The file is broken:\nmodinfo.lua on " + message + "\n\nPlease fix the problem. The ModInfo editor is deactivated and the view is changed to Code Editor.", I18N.__("Problem - Broken file"), Dialog.Buttons.OK, Dialog.Icon.Error, delegate (Dialog.Result result) {
                    return true;
                });

                properties.Disabled(string.Format(I18N.__("The ModInfo editor is deactivated due to an error.\n\n\n{0}"), message));
                this.SelectedIndex = 1;
            }

            TabItem item = new TabItem();
            item.Header = I18N.__("Editor");
            item.Content = properties;

            this.AddChild(item);
        }

        private void CreateSourceEditor() {
            TabItem item = new TabItem();
            item.Header = I18N.__("Source");
            item.Content = new Code(this.document);
            this.AddChild(item);
        }
    }
}
