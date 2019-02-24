using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DSTEd.Core.LUA;

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
            LUA.ModInfo info = LUAInterpreter.GetModInfo(this.document.GetFileContent());

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
