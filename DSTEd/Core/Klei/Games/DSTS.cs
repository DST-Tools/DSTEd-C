using System.Text;
using System.Text.RegularExpressions;

namespace DSTEd.Core.Klei.Games {
    class DSTS : KleiGame {
		public ARG Argument;
        public DSTS() : base() {
            this.id = 343050;
            this.name = I18N.__("Server");
            this.executable = "bin/dontstarve_dedicated_server_nullrenderer.exe";
			AddDebug("Run debug server", executable);
			//add sub menu, with "Restart" and "Shutfown"?
        }
		
		public struct ARG//persistent_storage_root/cluster/shard
		{
			public string cluster;
			public string shard;
			public bool offline;
			public string config_dir;
			public string backup_log;
			public string persistent_storage_root;
			public string maxplayers;
			public string tickrate;

			ARG(string cluster,string shard = "Master", bool offline =true)
			{
				this.cluster = cluster;
				this.shard = shard;
				this.offline = offline;
				config_dir = null;
				backup_log = null;
				persistent_storage_root = null;
				maxplayers = null;
				tickrate = null;
			}
		}
		private string findrs()
		{
			if (Argument.persistent_storage_root != null)
				return Argument.persistent_storage_root;
			else
			{
				string MyDocuments = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
				if (MyDocuments != null)
				{
					return MyDocuments + "\\Klei";
				}
				else
					throw new System.IO.DirectoryNotFoundException("MyDocuments not found, please specify a root storage directory.");
			}
		}
		public DSTS(ARG arg) : base()
		{
			Argument = arg;
			this.id = 343050;
			this.name = I18N.__("Server");
			this.executable = "bin/dontstarve_dedicated_server_nullrenderer.exe";
			AddDebug("Run debug server", executable);
			setarg();
		}
		public void AddMod(string modfoldername)
		{
			string rs = findrs();
			byte[] buffier = { 0 };
			string modoverride = rs + '\\' + Argument.config_dir + '\\' + Argument.cluster + '\\' + Argument.shard + '\\' + "modoverrides.lua";

			using (var file = new System.IO.FileStream(modoverride, System.IO.FileMode.Open))
				file.Read(buffier, 0, (int)file.Length);
			string rawscript = Encoding.UTF8.GetString(buffier);
			string realcfg = new Regex("^return\\s.=\\s.{<ModsCFG>}").Match(rawscript).Result("${ModsCFG}");
			realcfg += string.Format("[{0}]={{}}", modfoldername);//[modname]={},let DST generate things left
			rawscript = string.Format("return\n{{{0}}}", realcfg);
			using (var w = new System.IO.FileStream(modoverride, System.IO.FileMode.Truncate))
			{
				var buff = Encoding.UTF8.GetBytes(rawscript);
				w.Write(buff, 0, buff.Length);
			}
		}
		public void ClearMod()
		{
			string rs = findrs();
			string modoverride = rs + '\\' + Argument.config_dir + '\\' + Argument.cluster + '\\' + Argument.shard + '\\' + "modoverrides.lua";
			var w = new System.IO.FileStream(modoverride, System.IO.FileMode.Truncate);
			var buff = Encoding.UTF8.GetBytes("return\n{\n}");
			w.Write(buff, 0, buff.Length);
		}
		private void setarg()
		{
			var arg = GetDebugger().GetStartInfo().Arguments;
			arg += "\"-cluster\" " + '"' + Argument.cluster + '"' + " ";
			arg += "\"-shard\" " + '"' + Argument.shard + '"' + " ";
			if (Argument.offline)
				arg += "\"-offline\" " + " ";
			if (Argument.backup_log != null)
				arg += "\"-backup_logs\" " + '"' + Argument.backup_log + '"' + " ";
			if (Argument.config_dir != null)
				arg += "\"-conf_dir\" " + '"' + Argument.config_dir + '"' + " ";
			if (Argument.persistent_storage_root != null)
				arg += "\"-persistent_storage_root \" " + '"' + Argument.persistent_storage_root + '"' + " ";
			if (Argument.maxplayers != null)
				arg += "\"-maxplayers \" " + '"' + Argument.maxplayers + '"' + " ";
			if (Argument.tickrate != null)
				arg += "\"-tickrate \" " + '"' + Argument.tickrate + '"' + " ";
		}

		#region Commands
		public void Shutdown()
		{
			GetDebugger().SendCommand("ShutDown()");
		}
		public void Spawn(string ItemName)
		{
			GetDebugger().SendCommand(string.Format("c_spawn({0})",ItemName));
		}
		#endregion
	}
}
