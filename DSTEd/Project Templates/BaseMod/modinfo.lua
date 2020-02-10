--The name of the mod displayed in the 'mods' screen.
name = "New Mod"

--A description of the mod.
description = "This is a New Mod description."

--Who wrote this awesome mod?
author = "yourname"

--A version number so you can ask people if they are running an old version of your mod.
version = "1.0.0"

--This lets other players know if your mod is out of date. This typically needs to be updated every time there's a new game update.
api_version = 10

dst_compatible = true

--This lets clients know if they need to get the mod from the Steam Workshop to join the game
all_clients_require_mod = false

--This determines whether it causes a server to be marked as modded (and shows in the mod list)
client_only_mod = true

--This lets people search for servers with this mod by these tags
server_filter_tags = {}

icon_atlas = "icon_atlas.xml"
icon = "icon.tex"

-- This is the URL name of the mod's thread on the forum; the part after the ? and before the first & in the url
-- example forumthread = "/files/file/923-dst-status-announcements"
forumthread = ""

configuration_options = 
{
	{
		name = "options_name",
		label = "options_label",
		hover = "options_hover.",
		options =
		{
			{ description = "On", data = true },
			{ description = "Off", data = false },
		},
		default = true,
	}
}