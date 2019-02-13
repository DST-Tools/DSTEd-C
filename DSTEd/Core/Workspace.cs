using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace DSTEd.Core {
    class Workspace{
        public string BinPath;
        public string ModsPath;
        XmlDocument ConfigXML = new XmlDocument();
        public Workspace(string workspace)
        {
            BinPath = workspace + "\\bin";
            ModsPath = workspace + "\\mods";
        }
        public Workspace()
        {
            //@TODO: Load From File
            ConfigXML.Load(".\\config.xml");
            var Root = ConfigXML.DocumentElement;
            BinPath = Root.SelectSingleNode("/root/workspace/bin").Value;
            ModsPath = Root.SelectSingleNode("/root/workspace/mods").Value;
        }
        public void SaveToFile()
        {
            var Root = ConfigXML.DocumentElement;
            var workspace = Root.SelectSingleNode("/root/workspace");
            workspace.RemoveAll();
            var bin = ConfigXML.CreateElement("bin");
            var mods = ConfigXML.CreateElement("mods");
            bin.Value = BinPath;
            mods.Value = ModsPath;
            workspace.AppendChild(bin);
            workspace.AppendChild(mods);
        }
    }
}
