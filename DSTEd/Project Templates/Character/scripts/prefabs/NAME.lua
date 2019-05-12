
local MakePlayerCharacter = require "prefabs/player_common"
--local easing = require("easing")
local assets = {

        Asset( "ANIM", "anim/player_basic.zip" ),
        Asset( "ANIM", "anim/player_idles_shiver.zip" ),
        Asset( "ANIM", "anim/player_actions.zip" ),
        Asset( "ANIM", "anim/player_actions_axe.zip" ),
        Asset( "ANIM", "anim/player_actions_pickaxe.zip" ),
        Asset( "ANIM", "anim/player_actions_shovel.zip" ),
        Asset( "ANIM", "anim/player_actions_blowdart.zip" ),
        Asset( "ANIM", "anim/player_actions_eat.zip" ),
        Asset( "ANIM", "anim/player_actions_item.zip" ),
        Asset( "ANIM", "anim/player_actions_uniqueitem.zip" ),
        Asset( "ANIM", "anim/player_actions_bugnet.zip" ),
        Asset( "ANIM", "anim/player_actions_fishing.zip" ),
        Asset( "ANIM", "anim/player_actions_boomerang.zip" ),
        Asset( "ANIM", "anim/player_bush_hat.zip" ),
        Asset( "ANIM", "anim/player_attacks.zip" ),
        Asset( "ANIM", "anim/player_idles.zip" ),
        Asset( "ANIM", "anim/player_rebirth.zip" ),
        Asset( "ANIM", "anim/player_jump.zip" ),
        Asset( "ANIM", "anim/player_amulet_resurrect.zip" ),
        Asset( "ANIM", "anim/player_teleport.zip" ),
        Asset( "ANIM", "anim/wilson_fx.zip" ),
        Asset( "ANIM", "anim/player_one_man_band.zip" ),
        Asset( "ANIM", "anim/shadow_hands.zip" ),
        Asset( "SOUND", "sound/sfx.fsb" ),
        Asset( "SOUND", "sound/wilson.fsb" ),
        --Asset( "ANIM", "anim/beard.zip" ),

        Asset( "ANIM", "anim/<NAME>.zip" ),
}
local prefabs = {

}

local start_inv=
{
	
}
local start_inv_biantaiban=
{
	
}

local function onload(inst)
end



local common_postinit = function(inst) 
--Executes both Main server and clients
--服务器、客户端（包括洞穴）均执行
	inst.MiniMapEntity:SetIcon("map_<NAME>.tex" )
	inst:AddTag("<NAME>")
	--inst:AddTag("reader")
end

local master_postinit = function(inst)
--EN: Executes only on Main server
--仅主机执行(设置三维等需要在服务端执行)
		inst.soundsname = "wilson"
		inst.starting_inventory = start_inv
        --inst:ListenForEvent("onattackother", dagui)
        --inst:AddComponent("reader")
end

return MakePlayerCharacter("<NAME>", prefabs, assets, common_postinit, master_postinit, start_inv)

 

 
 