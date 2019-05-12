using System;
using System.Collections.Generic;

namespace DSTEd.Core
{
	public class DebugCLICore
	{
		public delegate string CommandFunction(params string[] args);
		private Dictionary<string, CommandFunction> commands;
		private Dictionary<string, string> helps;
		public DebugCLICore()
		{
			commands = new Dictionary<string, CommandFunction>(20);
			helps = new Dictionary<string, string>(20);
			AddCommand("cvarlist", cvarlist);
			AddCommand("cmdlist", cvarlist);
			AddCommand("help", helpfn);
		}

		private string cvarlist(params string[] args)
		{
			System.Text.StringBuilder tmp = new System.Text.StringBuilder();
			uint i = 0;
			foreach (string name in commands.Keys)
				if(name.StartsWith(args != null?args[0]:string.Empty))
				{
					tmp.AppendLine(name);
					i++;
				}
			System.Text.StringBuilder output = new System.Text.StringBuilder(string.Format("{0} command(s) in total\n", i));
			output.Append(tmp);
			return output.ToString();
		}
		private string helpfn(params string[] args)
		{
			if (helps.TryGetValue(args != null?args[0]:string.Empty, out string r))
				return r;
			return string.Empty;
		}

		public void AddCommand(string name,CommandFunction fn)
		{
			commands.Add(name, fn);
		}
		public void AddCommand(string name, string help, CommandFunction fn)
		{
			commands.Add(name, fn);
			helps.Add(name, help);
		}

		public string Execute(string command)
		{
			string[] cmd = command.Split(' ');
			string[] arg = null;
			if(cmd.Length>=2)
			{
				arg = new string[cmd.Length - 1];
				for (uint i = 0; i < cmd.Length-1; i++)
				{
					arg[i] = cmd[i + 1];
				}
			}
			if (commands.TryGetValue(cmd[0], out CommandFunction fn))
				return fn(arg);
			else
				return "No such command";
		}
	}
}
