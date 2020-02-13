PrefabFiles = {
	"<NAME>",
}
Assets = {
    Asset( "IMAGE", "images/saveslot_portraits/<NAME>.tex" ),
    Asset( "ATLAS", "images/saveslot_portraits/<NAME>.xml" ),

    Asset( "IMAGE", "images/selectscreen_portraits/<NAME>.tex" ),
    Asset( "ATLAS", "images/selectscreen_portraits/<NAME>.xml" ),
	
    Asset( "IMAGE", "images/selectscreen_portraits/<NAME>_none.tex" ),
    Asset( "ATLAS", "images/selectscreen_portraits/<NAME>_none.xml" ),
    
    Asset( "IMAGE", "bigportraits/<NAME>.tex" ),
    Asset( "ATLAS", "bigportraits/<NAME>.xml" ),
    
 	Asset( "IMAGE", "bigportraits/<NAME>_none.tex" ),
    Asset( "ATLAS", "bigportraits/<NAME>_none.xml" ),
    
	Asset( "IMAGE", "images/map_icons/<NAME>.tex" ),
	Asset( "ATLAS", "images/map_icons/<NAME>.xml" ),
	
	Asset( "IMAGE", "images/avatars/avatar_<NAME>.tex" ),
    Asset( "ATLAS", "images/avatars/avatar_<NAME>.xml" ),
    ---------------------SND PART----------------
    --Asset("SOUNDPACKAGE", "sound/<NAME>.fev"),--
    --Sound Bank
	--------------------------------------------------    
    Asset("ANIM","anim/<NAME>.zip"),
}

local require = GLOBAL.require
local STRINGS = GLOBAL.STRINGS
local Ingredient = GLOBAL.Ingredient
local RECIPETABS = GLOBAL.RECIPETABS
local Recipe = GLOBAL.Recipe
local TECH = GLOBAL.TECH

local TUNING = GLOBAL.TUNING
local Player = GLOBAL.ThePlayer
local TheNet = GLOBAL.TheNet
local IsServer = GLOBAL.TheNet:GetIsServer()
local TheInput = GLOBAL.TheInput
local TimeEvent = GLOBAL.TimeEvent
local FRAMES = GLOBAL.FRAMES
local EQUIPSLOTS = GLOBAL.EQUIPSLOTS
local EventHandler = GLOBAL.EventHandler
local SpawnPrefab = GLOBAL.SpawnPrefab
local State = GLOBAL.State
local DEGREES = GLOBAL.DEGREES
local Vector3 = GLOBAL.Vector3
local ACTIONS = GLOBAL.ACTIONS
local FOODTYPE = GLOBAL.FOODTYPE
local PLAYERSTUNLOCK = GLOBAL.PLAYERSTUNLOCK
local GetTime = GLOBAL.GetTime
local HUMAN_MEAT_ENABLED = GLOBAL.HUMAN_MEAT_ENABLED
local TheSim = GLOBAL.TheSim
local ActionHandler = GLOBAL.ActionHandler
GLOBAL.setmetatable(env,{__index=function(t,k) return GLOBAL.rawget(GLOBAL,k) end})

----------------------导入自定义StateGraph---------------------------
------------------------CUSTOM Stategraph----------------------------
--modimport("scripts/stategraphs/modstategraphs.lua") 
------------------------------END------------------------------------

STRINGS.CHARACTER_TITLES.<NAME> = ""
STRINGS.CHARACTER_NAMES.<NAME> = ""
STRINGS.CHARACTER_DESCRIPTIONS.<NAME> = ""
STRINGS.CHARACTER_QUOTES.<NAME> = ""

--STRINGS.CHARACTERS.FA = require "speech_fa"

STRINGS.NAMES.<NAME> = ""
--TUNING.biantaiban=GetModConfigData("biantaiban")
----------------------------------------------------------------------------------
AddMinimapAtlas("images/map_icons/<NAME>.xml")

AddModCharacter("<NAME>", "")

