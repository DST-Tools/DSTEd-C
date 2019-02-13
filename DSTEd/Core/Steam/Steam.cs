using System;
using Microsoft.Win32;

namespace DSTEd.Core.Steam {
    class Steam {
        public Boolean IsInstalled() {
            // @ToDo check installation path and registry
            object regvalue = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("Valve").OpenSubKey("Steam").GetValue("SteamPath");
            if (regvalue == null)
                return false;
            else
            {
                //initalize Steam Path
                SteamPath = regvalue.ToString();
                return true;
            }
        }
        
        public String GetPath() {
            // @ToDo get steam installation path/directory
            // Registry: \\Software\\Valve\\Steam
            return SteamPath;
        }
        private String SteamPath;

    }
}
