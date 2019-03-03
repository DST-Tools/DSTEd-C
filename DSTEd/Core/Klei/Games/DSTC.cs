using System.Windows.Controls;
namespace DSTEd.Core.Klei.Games {
    class DSTC : KleiGame {
        public DSTC() : base() {
            this.id = 322330;
            this.name = I18N.__("Client");
            this.executable = "bin/dontstarve_steam.exe";
            this.SetMainGame();

            this.AddDebug("DST_CLIENT", null);//I18N?
            this.AddSubDebug("DST_CLIENT", "RUN_DST", this.executable);
            AddSubDebug("DST_CLIENT", "DBGCMD", null);
            AddDebugCommand("RESET", "Reset", "c_reset()");
            AddDebugCommand("SHUTDOWN", "Shutdown", "Shutdown()");
        }

        public void AddDebugCommand(string cmdname, string menutitle,string LUAcommand)
        {
            var dbgmenu = Boot.Core().GetIDE().GetDebug().Items;
            var toadd = new MenuItem
            {
                Name = cmdname,
                Header = menutitle
            };
            var clickhandler = new System.Windows.RoutedEventHandler(
            delegate (object s, System.Windows.RoutedEventArgs arg)
            {
                debugger.SendCommand(LUAcommand);
            }
            );

            toadd.Click += clickhandler;
            foreach (MenuItem sub in dbgmenu)
            {

                if (sub.Name == "DST_CLIENT")
                {
                    foreach (MenuItem sub2 in sub.Items)
                    {
                        if(sub2.Name == "DBGCMD")
                        {
                            sub2.Items.Add(toadd);
                            return;
                        }
                    }
                }
            }
        }


    }
}
