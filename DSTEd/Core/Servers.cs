using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using DSTEd.Core;

namespace DSTEd.Core.Servers
{
    //i will create a default arg at Server construct.
    struct SV_args
    {
        public string cluster;
        public string shard;
        public string logbackup;
        public string storage_root;
        public string configdir;
        public string tickrate;
        public string maxplayers;
        public bool offline;
    }

    class Servers
    {
        public Servers()//default construct
        {
            sv_arg.cluster = "Cluster_1";
            sv_arg.shard = "Master";
            sv_arg.offline = true;
            sv_dir = ".\\";
        }
        public Servers(SV_args arg)
        {
            sv_arg = arg;
            sv_dir = ".\\";
        }
        public Servers(SV_args arg,string dir)
        {
            sv_arg = arg;
            sv_dir = dir;
        }
        public void terminte_server()
        {
            Server_Master.Kill();
        }

        public bool call_server()
        {
            //parse offline
            string offline;
            if(sv_arg.offline)
            {
                offline = "-offline";
            }
            else
            {
                offline = "false";
            }
            Server_Master.StartInfo.FileName = sv_dir + "\\dontstarve_dedicated_server_nullrenderer.exe";
            Server_Master.StartInfo.WorkingDirectory = sv_dir;
            //Arguments
            Server_Master.StartInfo.Arguments =
                offline + " " +
                "-cluster " + sv_arg.cluster + " " +
                "-shard " + sv_arg.shard + " " +
                "-conf_dir " + sv_arg.configdir + " " +
                "-persistent_storage_root " + sv_arg.storage_root + " " +
                "-tickrate " + sv_arg.tickrate + " " +
                "-maxplayers " + sv_arg.maxplayers + " " +
                "-backup_logs " + sv_arg.logbackup + " ";

            Server_Master.Start();
            return true;
        }
        
        public void set_arg(SV_args arg)
        {
            sv_arg = arg;
        }
        private string sv_dir;
        private SV_args sv_arg;
        private Process Server_Master;

        //Process Server_Cave;
    }
}
