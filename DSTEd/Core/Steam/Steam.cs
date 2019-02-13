using System;

namespace DSTEd.Core.Steam {
    class Steam {
        public Boolean IsInstalled() {
            // @ToDo check installation path and registry
            return false;
        }
        
        public String GetPath() {
            // @ToDo get steam installation path/directory
            // Registry: \\Software\\Valve\\Steam
            return "";
        }
    }
}
