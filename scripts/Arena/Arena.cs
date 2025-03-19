// 			Arena Admin Commands:
//				#arena			-	Will bring up a menu of commands, easier than using chat
//				#arena join		-	Will teleport you into the arena
//				#arena delete		-	Will remove the arena and disable it
//				#arena <name>		-	Will load the arena you specify replacing <name>, see default below
//				#arena getpos <dsc>	-	Will export the offset of your player inside of the arena to temp\Arena_Vecs.cs
//							-	<dsc> is a one word identifier
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

$Arena::Spawn = "1200 1200 4000"; //TA::pickWaypoint();
$Arena::Initialized = false;
$Arena::Mapchange = false; //this is just something for arena code
$Arena::firstMap = "default";
$Arena::RandomMap = false;
$Arena::ActiveVote = false;
$Arena::NoForceTeam = true;
$Arena::MapRipper = false;
$Arena::Lock = false;
$Arena::Terrain = false;
$Arena::Bots = true;
$Pi = 3.14159;
//exec(ArenaFlag);

//
function Arena::Mapper(%clientId,%locA)
{
	%player = Client::getOwnedObject(%clientId);
	//$Arena::object["tdvalley",%n++] = "spawn3 -29 67 208 0 -0 -2.35613";
	if(GameBase::getLOSInfo(%player,3000) && !Player::isdead(%player))
	{
		deleteVariables("Arena::object*"); // Make sure of a clean export.
		// there must be something in our sight. 
		// GetLOSInfo sets the following globals:
		// 	los::position
		// 	los::normal
		// 	los::object	
		%object = $los::object;	
		%type = getObjectType(%object);
		%name = Object::getName(%object);
		%data = Gamebase::getDataName(%object);
		if(%locA)
		{
			%spos = " "@Vector::sub(GameBase::getPosition(%object),$Arena::Spawn);
			%srot = " "@gamebase::getrotation(%object);
		
		}
		else
		{
			%spos = " "@GameBase::GetPosition(%object);
			%srot = " "@gamebase::getrotation(%object);
		}
		%map = $Arena::curMission;
		if(%type == InteriorShape)
		{
			%name = Ann::Replace(%name, "1", ".dis");
			$Arena::object[%map] = %name @  %spos @ %srot;
			%exportArenaObject = %name @  %spos @ %srot;
			Client::sendMessage(%clientId,0,"$Arena::object["@%map@"] = "@%exportArenaObject);
			export("Arena::object*", "config\\ArenaMaps.cs", true);
			deleteVariables("Arena::object*");
			return;
		}
		
		if(%type == "StaticShape")
		{
			%name = Ann::Replace(%name, "1", "");
			echo("sDataName == "@%data);
			echo("sName == "@%name);
			echo("sDescription == "@%data.description);
			echo("sMapName == "@GameBase::getMapName(%object));
			%mname = " "@GameBase::getMapName(%object);
			$Arena::object[%map] = %data @  %spos @ %srot @ %mname;
			%exportArenaObject = %data @  %spos @ %srot @ %mname;
			Client::sendMessage(%clientId,0,"$Arena::object["@%map@"] = "@%exportArenaObject);
			export("Arena::object*", "config\\ArenaMaps.cs", true);
			deleteVariables("Arena::object*");
			return;
		}
		
		if(%type == "Item")
		{
			echo("iDataName == "@%data);
			echo("iName == "@%name);
			echo("iDescription == "@%data.description);
			%name = Ann::Replace(%name, "1", "");
			$Arena::object[%map] = %name @  %spos @ %srot;
			%exportArenaObject = %name @  %spos @ %srot;
			Client::sendMessage(%clientId,0,"$Arena::object["@%map@"] = "@%exportArenaObject);
			export("Arena::object*", "config\\ArenaMaps.cs", true);
			deleteVariables("Arena::object*");
			return;
		}
	}
}

function ArenaMap::checkObject(%object)
{
		%type = getObjectType(%object);
		%name = Object::getName(%object);
		%data = Gamebase::getDataName(%object);
		%spos = " "@GameBase::GetPosition(%object);
		%srot = " "@gamebase::getrotation(%object);
		%map = $Arena::curMission;
		if(%type == InteriorShape)
		{
			%name = Ann::Replace(%name, "1", ".dis");
			$Arena::object[%map] = %name @  %spos @ %srot;
			%exportArenaObject = %name @  %spos @ %srot;
			Client::sendMessage(%clientId,0,"$Arena::object["@%map@"] = "@%exportArenaObject);
			export("Arena::object*", "config\\ArenaMaps.cs", true);
			deleteVariables("Arena::object*");
			return;
		}
		
		if(%type == "StaticShape")
		{
			%name = Ann::Replace(%name, "1", "");
			echo("sDataName == "@%data);
			echo("sName == "@%name);
			echo("sDescription == "@%data.description);
			echo("sMapName == "@GameBase::getMapName(%object));
			%mname = " "@GameBase::getMapName(%object);
			$Arena::object[%map] = %data @  %spos @ %srot @ %mname;
			%exportArenaObject = %data @  %spos @ %srot @ %mname;
			Client::sendMessage(%clientId,0,"$Arena::object["@%map@"] = "@%exportArenaObject);
			export("Arena::object*", "config\\ArenaMaps.cs", true);
			deleteVariables("Arena::object*");
			return;
		}
		
		if(%type == "Item")
		{
			echo("iDataName == "@%data);
			echo("iName == "@%name);
			echo("iDescription == "@%data.description);
			%name = Ann::Replace(%name, "1", "");
			$Arena::object[%map] = %name @  %spos @ %srot;
			%exportArenaObject = %name @  %spos @ %srot;
			Client::sendMessage(%clientId,0,"$Arena::object["@%map@"] = "@%exportArenaObject);
			export("Arena::object*", "config\\ArenaMaps.cs", true);
			deleteVariables("Arena::object*");
			return;
		}
}

function Arena::getOffset(%clientId, %word)
{
	%player = Client::getOwnedObject(%clientId);
	%map = $Arena::curMission;
	deleteVariables("Arena::object*"); // Make sure of a clean export.
		if(%word == "spawn0")
		{
			%pos = " "@Vector::sub(GameBase::getPosition(%clientId),$Arena::Spawn);
			%rot = " "@GameBase::getRotation(%clientId);
			%object = "spawn0";
			echo("pos = "@%pos@";");
			echo("rot = "@%rot@";");

			$Arena::object[%map] = %object @ %pos @ %rot;
			%exportArenaObject = "spawn0"@ %pos @ %rot;
			Client::sendMessage(%clientId,0,"$Arena::object["@%map@"] = "@%exportArenaObject);
			export("Arena::object*", "config\\ArenaMaps.cs", true);
			deleteVariables("Arena::object*");
			return;
		}
		else if(%word == "spawn1")
		{
			%pos = " "@Vector::sub(GameBase::getPosition(%clientId),$Arena::Spawn);
			%rot = " "@GameBase::getRotation(%clientId);
			%object = "spawn1";
			echo("pos = "@%pos@";");
			echo("rot = "@%rot@";");

			$Arena::object[%map] = %object @ %pos @ %rot;
			%exportArenaObject = "spawn1"@ %pos @ %rot;
			Client::sendMessage(%clientId,0,"$Arena::object["@%map@"] = "@%exportArenaObject);
			export("Arena::object*", "config\\ArenaMaps.cs", true);
			deleteVariables("Arena::object*");
			return;
		}
}

function Arena::getOffsetxx(%clientId, %word)
{
	%player = Client::getOwnedObject(%clientId);
	//$Arena::object["tdvalley",%n++] = "spawn3 -29 67 208 0 -0 -2.35613";
	if(GameBase::getLOSInfo(%player,3000))// && !Player::isdead(%player))
	{
		deleteVariables("Arena::object*"); // Make sure of a clean export.
		// there must be something in our sight. 
		// GetLOSInfo sets the following globals:
		// 	los::position
		// 	los::normal
		// 	los::object
		%map = $Arena::curMission;
		echo("$los::object = "@$los::object);
		%target = $los::object;	
		%obj = getObjectType(%target);
		echo("getObjectType(%target) = "@%obj);
		%objname = Object::getName(%target);
		echo("Gamebase::getDataName(%target) = "@%objname);
		if(%word == "invo")
		{
			%pos = " "@Vector::sub(GameBase::getPosition(%target),$Arena::Spawn);
			%rot = " "@GameBase::getRotation(%target);
			%object = "AmmoStation";
			echo("pos = "@%pos@";");
			echo("rot = "@%rot@";");
	
			$Arena::object[%map] = %object @ %pos @ %rot;
			%exportArenaObject = "Arena::object"@%map;
			Client::sendMessage(%clientId,0,"$Arena::object["@%map@"] = "@%exportArenaObject);
			export("Arena::object*", "config\\ArenaMaps.cs", true);
			deleteVariables("Arena::object*");
			return;
		}
		else if(%word == "spawn0")
		{
			%pos = " "@Vector::sub(GameBase::getPosition(%clientId),$Arena::Spawn);
			%rot = " "@GameBase::getRotation(%clientId);
			%object = "spawn0";
			echo("pos = "@%pos@";");
			echo("rot = "@%rot@";");

			$Arena::object[%map] = %object @ %pos @ %rot;
			%exportArenaObject = "spawn0"@ %pos @ %rot;
			Client::sendMessage(%clientId,0,"$Arena::object["@%map@"] = "@%exportArenaObject);
			export("Arena::object*", "config\\ArenaMaps.cs", true);
			deleteVariables("Arena::object*");
			return;
		}
		else if(%word == "spawn1")
		{
			%pos = " "@Vector::sub(GameBase::getPosition(%clientId),$Arena::Spawn);
			%rot = " "@GameBase::getRotation(%clientId);
			%object = "spawn1";
			echo("pos = "@%pos@";");
			echo("rot = "@%rot@";");

			$Arena::object[%map] = %object @ %pos @ %rot;
			%exportArenaObject = "spawn1"@ %pos @ %rot;
			Client::sendMessage(%clientId,0,"$Arena::object["@%map@"] = "@%exportArenaObject);
			export("Arena::object*", "config\\ArenaMaps.cs", true);
			deleteVariables("Arena::object*");
			return;
		}
		else if(%word == "gen")
		{
			%pos = " "@Vector::sub(GameBase::getPosition(%target),$Arena::Spawn);
			%rot = " "@GameBase::getRotation(%target);
			%object = "PortGenerator";
			echo("pos = "@%pos@";");
			echo("rot = "@%rot@";");

			$Arena::object[%map] = %object @ %pos @ %rot;
			%exportArenaObject = "Arena::object"@%map;
			Client::sendMessage(%clientId,0,"$Arena::object["@%map@"] = "@%exportArenaObject);
			export("Arena::object*", "config\\ArenaMaps.cs", true);
			deleteVariables("Arena::object*");
			return;
		}
		else if(%word == "ff")
		{
			%pos = " "@Vector::sub(GameBase::getPosition(%target),$Arena::Spawn);
			%rot = " "@GameBase::getRotation(%target);
			%object = "ForceField";
			echo("pos = "@%pos@";");
			echo("rot = "@%rot@";");

			$Arena::object[%map] = %object @ %pos @ %rot;
			%exportArenaObject = "Arena::object"@%map;
			Client::sendMessage(%clientId,0,"$Arena::object["@%map@"] = "@%exportArenaObject);
			export("Arena::object*", "config\\ArenaMaps.cs", true);
			deleteVariables("Arena::object*");
			return;
		}
		else if(%word == "disc")
		{
			%pos = " "@Vector::sub(GameBase::getPosition(%target),$Arena::Spawn);
			%rot = " "@GameBase::getRotation(%target);
			%object = "DiscAmmo";
			echo("pos = "@%pos@";");
			echo("rot = "@%rot@";");

			$Arena::object[%map] = %object @ %pos @ %rot;
			%exportArenaObject = "Arena::object"@%map;
			Client::sendMessage(%clientId,0,"$Arena::object["@%map@"] = "@%exportArenaObject);
			export("Arena::object*", "config\\ArenaMaps.cs", true);
			deleteVariables("Arena::object*");
			return;
		}
		else if(%word == "hkit")
		{
			%pos = " "@Vector::sub(GameBase::getPosition(%target),$Arena::Spawn);
			%rot = " "@GameBase::getRotation(%target);
			%object = "RepairhKit";
			echo("pos = "@%pos@";");
			echo("rot = "@%rot@";");

			$Arena::object[%map] = %object @ %pos @ %rot;
			%exportArenaObject = "Arena::object"@%map;
			Client::sendMessage(%clientId,0,"$Arena::object["@%map@"] = "@%exportArenaObject);
			export("Arena::object*", "config\\ArenaMaps.cs", true);
			deleteVariables("Arena::object*");
			return;
		}
		else if(%word == "hpatch")
		{
			%pos = " "@Vector::sub(GameBase::getPosition(%target),$Arena::Spawn);
			%rot = " "@GameBase::getRotation(%target);
			%object = "RepairPatch";
			echo("pos = "@%pos@";");
			echo("rot = "@%rot@";");

			$Arena::object[%map] = %object @ %pos @ %rot;
			%exportArenaObject = "Arena::object"@%map;
			Client::sendMessage(%clientId,0,"$Arena::object["@%map@"] = "@%exportArenaObject);
			export("Arena::object*", "config\\ArenaMaps.cs", true);
			deleteVariables("Arena::object*");
			return;
		}
		else if(%word == "norm")
		{
			%pos = " "@GameBase::getPosition(%target);
			%rot = " "@GameBase::getRotation(%target);
			%object = %objname;
			echo("pos = "@%pos@";");
			echo("rot = "@%rot@";");

			$Arena::object[%map] = %object @ %pos @ %rot;
			%exportArenaObject = "Arena::object"@%map;
			Client::sendMessage(%clientId,0,"$Arena::object["@%map@"] = "@%exportArenaObject);
			export("Arena::object*", "config\\ArenaMaps.cs", true);
			deleteVariables("Arena::object*");
			return;
		}
		else
		{
			%pos = " "@Vector::sub(GameBase::getPosition(%target),$Arena::Spawn);
			%rot = " "@GameBase::getRotation(%target);
			%object = %target;
			echo("pos = "@%pos@";");
			echo("rot = "@%rot@";");

			$Arena::object[%map] = %objname @ %pos @ %rot;
			%exportArenaObject = %objname @ %pos @ %rot;
			Client::sendMessage(%clientId,0,"$Arena::object["@%map@"] = "@%exportArenaObject);
			export("Arena::object*", "config\\ArenaMaps.cs", true);
			deleteVariables("Arena::object*");
			return;
		}
	}
}

function ArenaAdmin(%clientId, %sel) 
{
	if(%clientId.isadmin)
	{
		%name = Client::getName(%sel);
		Client::buildMenu(%clientId, "Manage "@%name, "ArenaAdmin", true);
		if(!%sel.inArena && %clientId.isOwner)
			Client::addMenuItem(%clientId, %curItem++ @ "Force into the Arena", "adjoinarena " @ %sel);
		else
		{
		if(%sel.inArena) 
			Client::addMenuItem(%clientId, %curItem++ @ "Kick from the Arena", "adkickarena " @ %sel);
		if(%clientId.isOwner)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Ban from the Arena", "adbanarena " @ %sel);
		}			
			if(%sel.inArenaTD)
			{
				Client::addMenuItem(%clientId, %curItem++ @ "Kick from the ArenaTD", "adkickarenatd " @ %sel);
			}
			
			if($ArenaTD::One == "TD 1" && %sel.inArena || $ArenaTD::One == "TD 2" && %sel.inArena)
				Client::addMenuItem(%clientId, %curItem++ @ "Add to ArenaTD", "adaddarenatd " @ %sel);
		}
		
	}
}

function processMenuArenaAdmin(%clientId, %option)
{
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);

	%client1 = Player::getClient(%clientId);
	%client2 = Player::getClient(%cl);
	
	if(%opt == "adjoinarena" && %clientId.isadmin)
	{
	if(%client2.isGoated && %client1 != %client2)
	{
	return;
	}
		Arena::ForceJoin(%clientId, %cl);
		return;
	}
	else if(%opt == "adkickarena" && %clientId.isadmin)
	{
	if(%client2.isGoated && %client1 != %client2)
	{
	return;
	}
		Arena::Kick(%clientId, %cl);
		return;
	}
	else if(%opt == "adbanarena" && %clientId.isadmin)
	{
	if(%client2.isGoated && %client1 != %client2)
	{
	return;
	}
		Arena::Ban(%clientId, %cl);
		return;
	}
	else if(%opt == "adkickarenatd" && %clientId.isadmin)
	{
	if(%client2.isGoated && %client1 != %client2)
	{
	return;
	}
		if(%cl.inArenaTDOne)
			ArenaMSG(1,Client::getName(%cl)@" was kicked from Team TD 1 by "@Client::getName(%clientId)@".~wcapturedtower.wav");
		else if(%cl.inArenaTDTwo)
			ArenaMSG(1,Client::getName(%cl)@" was kicked from Team TD 2 by "@Client::getName(%clientId)@".~wcapturedtower.wav");
		
		%cl.isArenaTDKicked = true;
		ArenaTD::Leave(%cl);
		return;
	}
	else if(%opt == "adaddarenatd" && %clientId.isadmin)
	{
	if(%client2.isGoated && %client1 != %client2)
	{
	return;
	}
		%name = Client::getName(%cl);
		Client::buildMenu(%clientId, "ArenaTD Team Change "@%name, "ArenaAdmin", true);
		
		if(!%cl.inArenaTDOne)
			Client::addMenuItem(%clientId, %curItem++ @ "Add " @ %name @ " to TD 1", "addtdo " @ %cl);
		
		if(!%cl.inArenaTDTwo)
			Client::addMenuItem(%clientId, %curItem++ @ "Add " @ %name @ " to TD 2", "addtdt " @ %cl);
		
		return;
	}
	else if(%opt == "addtdo" && %clientId.isadmin)
	{
		if(%cl.inArenaTDtwo)
			ArenaTD::Leave(%cl);
		//client::sendmessage(%cl, 0, %name@" has accepted your request.");
		if($ArenaTD::Active)
			%cl.isArenaTDDead = true;
		else
			%cl.isArenaTDDead = false;
		
		%cl.inArenaTDOne = true;
		%cl.TDMRequestOne = false;
		GameBase::setTeam(%cl,0);
		%cl.inArenaTD = true;
		Game::refreshClientScore(%cl);
		ArenaMSG(3,Client::getName(%cl)@" has been placed on "@$ArenaTD::One@"'s team."); 
		return;
	}
	else if(%opt == "addtdt" && %clientId.isadmin)
	{
		if(%cl.inArenaTDone)
			ArenaTD::Leave(%cl);
		//client::sendmessage(%cl, 0, %name@" has accepted your request.");
		if($ArenaTD::Active)
			%cl.isArenaTDDead = true;
		else
			%cl.isArenaTDDead = false;
			
		%cl.inArenaTDTwo = true;
		%cl.TDMRequestTwo = false;
		GameBase::setTeam(%cl,1);
		%cl.inArenaTD = true;
		Game::refreshClientScore(%cl);
		ArenaMSG(3,Client::getName(%cl)@" has been placed on "@$ArenaTD::Two@"'s team."); 
		return;
	}
}

function Arena::ForceJoin(%adminClient, %clientId)
{
	if(!%adminClient.SecretAdmin)
		%adminName = Client::getName(%adminClient);
	else %adminName = SecretAdmin();
	
	if(%adminClient != %clientId && !%adminclient.SecretAdmin)
		messageAll(0, Client::getName(%clientId) @ " was teamchanged by " @ %adminName @ ".");
		
	$Admin = Client::getName(%adminClient)@ " teamchanged " @ Client::getName(%clientId);	echo($admin);
		export("Admin","config\\Admin.log",true);
		
	Arena::Join(%clientId);
}

function Arena::Kick(%adminClient, %clientId)
{
	if(!%adminClient.SecretAdmin)
		%adminName = Client::getName(%adminClient);
	else %adminName = SecretAdmin();
	
	if(%adminClient != %clientId && !%adminclient.SecretAdmin)
		messageAll(0, Client::getName(%clientId) @ " was kicked from the Arena by " @ %adminName @ ".");
		
	$Admin = Client::getName(%adminClient)@ " kicked " @ Client::getName(%clientId) @ " from the Arena";	echo($admin);
		export("Admin","config\\Admin.log",true);
	
	ArenaTD::Leave(%clientId);
	Arena::Leave(%clientId);
}

function Arena::Ban(%adminClient, %clientId)
{
	if(!%adminClient.SecretAdmin)
		%adminName = Client::getName(%adminClient);
	else %adminName = SecretAdmin();
	
	if(%adminClient != %clientId && !%adminclient.SecretAdmin)
		messageAll(0, Client::getName(%clientId) @ " was banned from the Arena by " @ %adminName @ ".");
		
	$Admin = Client::getName(%adminClient)@ " banned " @ Client::getName(%clientId) @ " from the Arena";	echo($admin);
		export("Admin","config\\Admin.log",true);
	
	ArenaTD::Leave(%clientId);
	Arena::Leave(%clientId);
	%clientId.isArenaBanned = true;
}

function Arena::Opts(%clientId)
{
	if(%clientId.isArenaBanned)
		return;
		%client = Player::getClient(%clientId);
	if($Spoonbot::AutoSpawn)
	{
		// Arena::Clear();	
				Client::sendMessage(%client, 0, "No Arena during this map type. ~wPku_ammo.wav");
		return;
	}
	
	if($Arena::Kill)
	{
				Client::sendMessage(%client, 0, "No Arena during this map type. ~wPku_ammo.wav");
		// Arena::Clear();	
		return;
	}

	if(%clientId.isAdmin && !$ArenaTD::Active)
	{
		Client::buildMenu(%clientId, "Arena Options", "ArenaOptions", true);

		if(%clientId.hasArenaVoted && $Arena::ActiveVote)
		{
		}
		else
		{
		   	Client::addMenuItem(%clientId, %curItem++ @ "Voting", "vote");	
		}

		if($Arena::Initialized)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Manage Arena", "marena");
		}
		else
		   Client::addMenuItem(%clientId, %curItem++ @ "Select Arena", "change");
	}
	else if(!$ArenaTD::Active)
	{
		Client::buildMenu(%clientId, "Arena Options", "ArenaOptions", true);

		if(%clientId.hasArenaVoted && $Arena::ActiveVote)
		{
		}
		else
		{
		   	Client::addMenuItem(%clientId, %curItem++ @ "Voting", "vote");	
		}		   
	}
	
	if($ArenaTD::Active)
		Client::buildMenu(%clientId, "Arena Options", "arenaOptions", true);

if($Arena::Initialized)
{
		if(!%clientId.inArena)
		   Client::addMenuItem(%clientId, %curItem++ @ "Join Arena", "join");
		else if(%clientId.inArena && !%clientId.inArenaTD)
		   Client::addMenuItem(%clientId, %curItem++ @ "Leave Arena", "leave");
		else if(%clientId.inArena && %clientId.inArenaTD)
		   Client::addMenuItem(%clientId, %curItem++ @ "Leave Team", "ltd");

			if($Arena::curMission == "bcube" || $Arena::curMission == "madness" || $Arena::curMission == "bfcax" || $Arena::curMission == "bfdiscord" || $Arena::curMission == "bftrappedarena" || $Arena::curMission == "bbath" || $Arena::curMission == "carlsberg" || $Arena::curMission == "damnarena" || $Arena::curMission == "ivehadworse" || $Arena::curMission == "kuth" || $Arena::curMission == "lazarena" || $Arena::curMission == "oldmilwaukee" || $Arena::curMission == "standoff" || $Arena::curMission == "thearena" || $Arena::curMission == "hive")
			{
				if(%clientId.inArena && !%clientId.inArenaTD && $Arena::Bots)
				{
					Client::addMenuItem(%clientId, %curItem++ @ "Enemy Duel Bots ", "mmbots");
					Client::addMenuItem(%clientId, %curItem++ @ "Team Duel Bots ", "mmTbots");
				}
			}
		
	if(!$Arena::Winners) 
	{

		if(%clientId.inArena && !%clientId.isTDCaptOne && !%clientId.inArenaTD || %clientId.inArena && !%clientId.isTDCaptTwo && !%clientId.inArenaTD)
			Client::addMenuItem(%clientId, %curItem++ @ "Create a Team", "cat");

		if(%clientId.inArena && %clientId.isTDCaptOne && %clientId.inArenaTD || %clientId.inArena && %clientId.isTDCaptTwo && %clientId.inArenaTD)
		{
			if(%clientId.isTDCaptOne && $ArenaTD::Two != false)
				Client::addMenuItem(%clientId, %curItem++ @ "Manage "@$ArenaTD::One@" team", "mtd");
			else if(%clientId.isTDCaptTwo && $ArenaTD::One != false)
				Client::addMenuItem(%clientId, %curItem++ @ "Manage "@$ArenaTD::Two@" team", "mtd");
		} 
	}


}		
		Client::addMenuItem(%clientId, %curItem++ @ "Main Menu", "mm");
}

function processMenuArenaOptions(%clientId,%opt)
{
	%client = Player::getClient(%clientId);
	if(%opt == "marena")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		if(!%clientId.isAdmin)
			return;
			
		Client::buildMenu(%clientId, "Manage Arena", "ArenaOptions", true);
		
		if($Arena::Initialized)
		{
		if(%clientId.isSuperAdmin || %clientId.isGoated)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Change Arena", "change");
		}
			Client::addMenuItem(%clientId, %curItem++ @ "Arena Armor/Weapon", "changespawn");
			if($Arena::Winners)
				Client::addMenuItem(%clientId, %curItem++ @ "Disable Winners Mode", "darenawin");
			else
				Client::addMenuItem(%clientId, %curItem++ @ "Enable Winners Mode", "earenawin");
		if(%clientId.isGoated || %clientId.isOwner)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Disable Arena", "disable");
			if($Arena::Lock == false)
				Client::addMenuItem(%clientId, %curItem++ @ "Lock Arena", "lockarena");
			else
				Client::addMenuItem(%clientId, %curItem++ @ "Unlock Arena", "lockarena");
		}
//			if(%clientId.isGoated && $Arena::Initialized && !$ArenaTD::Active && $Arena::curMission == "bcube")
//				Client::addMenuItem(%clientId, %curItem++ @ "Cube Bots", "cbots");
			if(%clientId.isGoated || %clientId.isOwner)
				Client::addMenuItem(%clientId, %curItem++ @ "Enable or Disable Bots", "tbots");
		}
	}
	if(%opt == "change")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Client::buildMenu(%clientId, "Choose Arena", "arenaPick", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Archaea", "archaea");
		Client::addMenuItem(%clientId, %curItem++ @ "Arena In The Sky", "arenainthesky");
		Client::addMenuItem(%clientId, %curItem++ @ "Arena Madness(bots)", "madness"); 
		Client::addMenuItem(%clientId, %curItem++ @ "Arena Terrain", "arenaterrain"); 
		Client::addMenuItem(%clientId, %curItem++ @ "Battle Cube(bots)", "bcube");
		Client::addMenuItem(%clientId, %curItem++ @ "BF-CaX(bots)", "bfcax");
		Client::addMenuItem(%clientId, %curItem++ @ "BF-Colosseum", "bfcolosseum");
		Client::addMenuItem(%clientId, %curItem++ @ "Next Page >>>", "arenamapnextone");
	} 
	else if(%opt == "earenawin")
	{
		ArenaWin::ClearStage();
	}
	else if(%opt == "darenawin")
	{
		ArenaWin::End();
	}
	else if(%opt == "changespawn")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Client::buildMenu(%clientId, "Choose Arena Armor/Weapon", "ArenaSpawnType", true); 
		if($TAArena::SpawnType != "AnniSpawn")
			Client::addMenuItem(%clientId, %curItem++ @ "Annihilation", "annispawn");
		if($TAArena::SpawnType != "EliteSpawn")
			Client::addMenuItem(%clientId, %curItem++ @ "EliteRenegades", "elitespawn");
		if($TAArena::SpawnType != "BaseSpawn")
			Client::addMenuItem(%clientId, %curItem++ @ "Base", "basespawn");
		if($TAArena::WeaponOpt != "Normal")
			Client::addMenuItem(%clientId, %curItem++ @ "Normal", "normal");
		if($TAArena::WeaponOpt != "DiscOnly")
			Client::addMenuItem(%clientId, %curItem++ @ "DiscOnly", "disconly");
				if($TAArena::WeaponOpt != "RocketOnly")
			Client::addMenuItem(%clientId, %curItem++ @ "RocketOnly", "rocketonly");
		if($TAArena::WeaponOpt != "MaxAmmo")
			Client::addMenuItem(%clientId, %curItem++ @ "MaxAmmo", "maxammo");
		if($TAArena::SpawnType != "TitanSpawn")
			Client::addMenuItem(%clientId, %curItem++ @ "Titan", "titanspawn");
		if($TAArena::SpawnType != "BuilderSpawn")
			Client::addMenuItem(%clientId, %curItem++ @ "Builder", "builderspawn");
	}
	else if(%opt == "lockarena")
	{
		if($Arena::Lock == false)
		{
			$Arena::Lock = true;
			Messageall(0,Client::getName(%clientId)@" has locked the Arena.~wcapturedtower.wav");
		}
		else if($Arena::Lock == true)
		{
			$Arena::Lock = false;
			Messageall(0,Client::getName(%clientId)@" has unlocked the Arena.~wcapturedtower.wav");
		}
	}
	else if(%opt == "join" && !%clientId.inArena)
	{
		Arena::Join(%clientId);
	}
	else if(%opt == "leave" && %clientId.inArena)
	{
		Arena::leave(%clientId);
	}
	else if(%opt == vote)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
	//if(!%clientId.inArena && $Arena::Initialized) { Centerprint(%clientId,"<jc><f2>Try joining the Arena before you vote...",2); return; }

		Client::buildMenu(%clientId, "Arena Voting", "arenaVote", true);
		//if(!%clientId.inArena && $Arena::Initialized) 
		//{ 
		//	Centerprint(%clientId,"<jc><f2>Try joining the Arena before you vote...",2); 
		//		return;
		//}
		
		if(!$Arena::ActiveVote)
		{
			if(!$Arena::Initialized)
			   Client::addMenuItem(%clientId, %curItem++ @ "Vote to Enable Arena", "enable");
			
			if(%clientId.inArena) 
			{
				Client::addMenuItem(%clientId, %curItem++ @ "Change Arena", "change");
				if($TAArena::SpawnType != "AnniSpawn")
					Client::addMenuItem(%clientId, %curItem++ @ "Annihilation Mode", "varenaannispawn");
				if($TAArena::SpawnType != "EliteSpawn")
					Client::addMenuItem(%clientId, %curItem++ @ "EliteRenegades Mode", "varenaelitespawn");
				if($TAArena::SpawnType != "BaseSpawn")
					Client::addMenuItem(%clientId, %curItem++ @ "Base Mode", "varenabasespawn");
				if($TAArena::SpawnType != "TitanSpawn")
					Client::addMenuItem(%clientId, %curItem++ @ "Titan Armor Only", "varenatitanspawn");
				if($TAArena::SpawnType != "BuilderSpawn")
				    Client::addMenuItem(%clientId, %curItem++ @ "Builder Armor Only", "varenabuilderspawn");
				if($TAArena::WeaponOpt != "DiscOnly")
				    Client::addMenuItem(%clientId, %curItem++ @ "Disc Only", "varenadiscspawn");
				if($TAArena::WeaponOpt != "Normal")
				    Client::addMenuItem(%clientId, %curItem++ @ "Default Loadouts", "varenanormal");
				if($TAArena::WeaponOpt != "MaxAmmo")
				    Client::addMenuItem(%clientId, %curItem++ @ "Max Ammo", "varenamaxammo");
				if($Arena::Winners)
					Client::addMenuItem(%clientId, %curItem++ @ "Disable Winners", "vdarenawin");
				else
					Client::addMenuItem(%clientId, %curItem++ @ "Winners Mode", "vearenawin");
			}
		}
		else
		{
			if(!%clientId.hasArenaVoted)
			{
			   Client::addMenuItem(%clientId, %curItem++ @ "Vote YES to "@$Arena::curVote, "yes");
			   Client::addMenuItem(%clientId, %curItem++ @ "Vote NO to "@$Arena::curVote, "no");
			   Client::addMenuItem(%clientId, %curItem++ @ "Abstain", "abstain");
			}
		}
//		Client::addMenuItem(%clientId, %curItem++ @ "Back", "");
	}
	else if(%opt == disable)
	{
		if($Arena::Initialized)
		{
			Messageall(0,Client::getName(%clientId)@" disabled the arena.~wcapturedtower.wav");
			//client::sendmessage(%shooterClient, 0, Client::getName(%clientId)@" disabled the arena.~wcapturedtower.wav");
			Arena::Clear();
		}
	}
	else if(%opt == cat)
	{
		if($Arena::Initialized)
		{
			//Messageall(0,Client::getName(%clientId)@" disabled the arena.~wcapturedtower.wav");
			//client::sendmessage(%shooterClient, 0, Client::getName(%clientId)@" disabled the arena.~wcapturedtower.wav");
			ArenaTD::Create(%clientId);
		}
	}
	else if(%opt == mtd)
	{
		if($Arena::Initialized)
		{
			ArenaTD::Manage(%clientId);
		}
	}
	else if(%opt == ltd)
	{
		if($Arena::Initialized)
		{
			//Messageall(0,Client::getName(%clientId)@" has left blank team.~wcapturedtower.wav");
			ArenaTD::Leave(%clientId);
		}
	}
	else if(%opt == "mm")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		%clientId.MainMenu = true;
		Game::menuRequest(%clientId);
		schedule(%clientId @ ".MainMenu = false;" , 0.2, %clientId);
	}
	else if(%opt == "mmbots")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		ArenaDBots::Manage(%clientId);
	}
	else if(%opt == "mmTbots")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		ArenaTBots::Manage(%clientId);
	}
//	else if(%opt == "cbots")
//	{
//		if($TA::CubeBot)
//		{
//			$TA::CubeBot = false;
//			schedule("TA::CubeBotA();", 1);
//			schedule("TA::CubeBotB();", 3);
//			schedule("TA::CubeBotC();", 5);
//		}
//		else
//		{
//			$TA::CubeBot = true;
//			schedule("TA::CubeBotA();", 1);
//			schedule("TA::CubeBotB();", 3);
//			schedule("TA::CubeBotC();", 5);
//		}
//	}
	else if(%opt == "tbots")
	{
		if($Arena::Bots)
		{
			$Arena::Bots = false;
			ArenaMSG(0,Client::getName(%clientId)@" disabled Arena bots.");
			Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		}
		else
		{
			$Arena::Bots = true;
			ArenaMSG(0,Client::getName(%clientId)@" enabled Arena bots.");
			Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		}
	}
	else
	{
		return;
	}
}

function processMenuArenaVote(%clientId,%opt)
{
	%client = Player::getClient(%clientId);
//	if(%opt == "") Arena::Opts(%clientId);
	if(%opt == "") 
		{
		Arena::Opts(%clientId);
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		}
	if(!$Arena::ActiveVote)
	{
		if(%opt == enable)
		{
			if(!$Arena::Initialized)
			{
				Arena::InitiateVote("enable");
				Messageall(0,Client::getName(%clientId)@" initiated a vote to enable the Arena.");
				%clientId.hasArenaVoted = true;
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
					if(!%cl.hasArenaVoted && %cl != %clientId)
					{
							TA::BlackOut(%cl,11);
							bottomprint(%cl, "<f1><jc>"@Client::getName(%clientId)@" initiated a vote to enable the Arena.", 11);
							//Game::menuRequest(%cl);
					}
			}
		}
		else if(%opt == disable)
		{
			if($Arena::Initialized)
			{
				Arena::InitiateVote("disable");
				ArenaMSG(0,Client::getName(%clientId)@" initiated a vote to disable the Arena.");
				%clientId.hasArenaVoted = true;
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
					if(%cl.inArena)
						if(!%cl.hasArenaVoted && %cl != %clientId)
							Game::menuRequest(%cl);
			}
		
		}
		else if(%opt == change)
		{
			Client::sendMessage(%client, 0, "~wPku_ammo.wav");
			Client::buildMenu(%clientId, "Choose Arena", "ArenaVoteMap", true);
			Client::addMenuItem(%clientId, %curItem++ @ "Archaea", "archaea");
			Client::addMenuItem(%clientId, %curItem++ @ "Arena In The Sky", "arenainthesky");
			Client::addMenuItem(%clientId, %curItem++ @ "Arena Madness(bots)", "madness"); 
			Client::addMenuItem(%clientId, %curItem++ @ "Arena Terrain", "arenaterrain");
			Client::addMenuItem(%clientId, %curItem++ @ "Battle Cube(bots)", "bcube");
			Client::addMenuItem(%clientId, %curItem++ @ "BF-CaX(bots)", "bfcax");
			Client::addMenuItem(%clientId, %curItem++ @ "BF-Colosseum", "bfcolosseum");
			Client::addMenuItem(%clientId, %curItem++ @ "Next Page...", "arenamapnextone");
		}
		else if(%opt == varenaannispawn)
		{
			if($Arena::Initialized)
			{
				Arena::InitiateVote("vannispawn");
				ArenaMSG(0,Client::getName(%clientId)@" initiated a vote for Arena Annihilation Mod.");
				%clientId.hasArenaVoted = true;
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
					if(%cl.inArena)
						if(!%cl.hasArenaVoted && %cl != %clientId)
						{
							TA::BlackOut(%cl,11);
							bottomprint(%cl, "<f1><jc>"@Client::getName(%clientId)@" initiated a vote for Arena Annihilation Mod.", 11);
							//Game::menuRequest(%cl);
						}
			}
		
		}
		else if(%opt == varenaelitespawn)
		{
			if($Arena::Initialized)
			{
				Arena::InitiateVote("velitespawn");
				ArenaMSG(0,Client::getName(%clientId)@" initiated a vote for Arena EliteRenegades Mod.");
				%clientId.hasArenaVoted = true;
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
					if(%cl.inArena)
						if(!%cl.hasArenaVoted && %cl != %clientId)
						{
							TA::BlackOut(%cl,11);
							bottomprint(%cl, "<f1><jc>"@Client::getName(%clientId)@" initiated a vote for Arena EliteRenegades Mod.", 11);
							//Game::menuRequest(%cl);
						}
			}
		
		}
		else if(%opt == varenabasespawn)
		{
			if($Arena::Initialized)
			{
				Arena::InitiateVote("vbasespawn");
				ArenaMSG(0,Client::getName(%clientId)@" initiated a vote for Arena Base Mod.");
				%clientId.hasArenaVoted = true;
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
					if(%cl.inArena)
						if(!%cl.hasArenaVoted && %cl != %clientId)
						{
							TA::BlackOut(%cl,11);
							bottomprint(%cl, "<f1><jc>"@Client::getName(%clientId)@" initiated a vote for Arena Base Mod.", 11);
							//Game::menuRequest(%cl);
						}
			}
		
		}
		else if(%opt == varenatitanspawn)
		{
			if($Arena::Initialized)
			{
				Arena::InitiateVote("vtitanspawn");
				ArenaMSG(0,Client::getName(%clientId)@" initiated a vote for Titan armor only in Arena.");
				%clientId.hasArenaVoted = true;
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
					if(%cl.inArena)
						if(!%cl.hasArenaVoted && %cl != %clientId)
						{
							TA::BlackOut(%cl,11);
							bottomprint(%cl, "<f1><jc>"@Client::getName(%clientId)@" initiated a vote for Titan armor only in Arena.", 11);
							//Game::menuRequest(%cl);
						}
			}
		
		}
		else if(%opt == varenabuilderspawn)
		{
			if($Arena::Initialized)
			{
				Arena::InitiateVote("vbuilderspawn");
				ArenaMSG(0,Client::getName(%clientId)@" initiated a vote for Builder armor only in Arena.");
				%clientId.hasArenaVoted = true;
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
					if(%cl.inArena)
						if(!%cl.hasArenaVoted && %cl != %clientId)
						{
							TA::BlackOut(%cl,11);
							bottomprint(%cl, "<f1><jc>"@Client::getName(%clientId)@" initiated a vote for Builder armor only in Arena.", 11);
							//Game::menuRequest(%cl);
						}
			}
		
		}
		else if(%opt == varenadiscspawn)
		{
			if($Arena::Initialized)
			{
				Arena::InitiateVote("vdiscspawn");
				ArenaMSG(0,Client::getName(%clientId)@" initiated a vote for disc launchers only in Arena.");
				%clientId.hasArenaVoted = true;
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
					if(%cl.inArena)
						if(!%cl.hasArenaVoted && %cl != %clientId)
						{
							TA::BlackOut(%cl,11);
							bottomprint(%cl, "<f1><jc>"@Client::getName(%clientId)@" initiated a vote for disc launchers only in Arena.", 11);
							//Game::menuRequest(%cl);
						}
			}
		
		}
		else if(%opt == varenanormal)
		{
			if($Arena::Initialized)
			{
				Arena::InitiateVote("vnormal");
				ArenaMSG(0,Client::getName(%clientId)@" initiated a vote for the default loadouts in Arena.");
				%clientId.hasArenaVoted = true;
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
					if(%cl.inArena)
						if(!%cl.hasArenaVoted && %cl != %clientId)
						{
							TA::BlackOut(%cl,11);
							bottomprint(%cl, "<f1><jc>"@Client::getName(%clientId)@" initiated a vote for the default loadouts in Arena.", 11);
							//Game::menuRequest(%cl);
						}
			}
		
		}
		else if(%opt == varenamaxammo)
		{
			if($Arena::Initialized)
			{
				Arena::InitiateVote("vmaxammo");
				ArenaMSG(0,Client::getName(%clientId)@" initiated a vote for max ammo in the Arena.");
				%clientId.hasArenaVoted = true;
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
					if(%cl.inArena)
						if(!%cl.hasArenaVoted && %cl != %clientId)
						{
							TA::BlackOut(%cl,11);
							bottomprint(%cl, "<f1><jc>"@Client::getName(%clientId)@" initiated a vote for max ammo in the Arena.", 11);
							//Game::menuRequest(%cl);
						}
			}
		
		}
		else if(%opt == vearenawin)
		{
			if($Arena::Initialized)
			{
				Arena::InitiateVote("vearenawin");
				ArenaMSG(0,Client::getName(%clientId)@" initiated a vote to enable TA Winners Mode.");
				%clientId.hasArenaVoted = true;
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
					if(%cl.inArena)
						if(!%cl.hasArenaVoted && %cl != %clientId)
						{
							TA::BlackOut(%cl,11);
							bottomprint(%cl, "<f1><jc>"@Client::getName(%clientId)@" initiated a vote to enable TA Winners Mode.", 11);
							//Game::menuRequest(%cl);
						}
			}
		
		}
		else if(%opt == vdarenawin)
		{
			if($Arena::Initialized)
			{
				Arena::InitiateVote("vdarenawin");
				ArenaMSG(0,Client::getName(%clientId)@" initiated a vote to disable TA Winners Mode.");
				%clientId.hasArenaVoted = true;
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
					if(%cl.inArena)
						if(!%cl.hasArenaVoted && %cl != %clientId)
						{
							TA::BlackOut(%cl,11);
							bottomprint(%cl, "<f1><jc>"@Client::getName(%clientId)@" initiated a vote to disable TA Winners Mode.", 11);
							//Game::menuRequest(%cl);
						}
			}
		
		}
	}
	else
	{
		if(%opt == yes)
		{
			$Arena::VotesFor++;
			%clientId.hasArenaVoted = true;
		}
		else if(%opt == no)
		{
			$Arena::VotesAgainst++;
			%clientId.hasArenaVoted = true;
		}
		else
		{
			$Arena::VotesAbstained++;
			%clientId.hasArenaVoted = true;
		}
	}
}

function processMenuArenaVoteMap(%clientId,%opt)
{
	%client = Player::getClient(%clientId);
	if(%opt == back)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		processMenuArenaOptions(%clientId,"vote");
	}
	else if(%opt == arenamap)
	{
		processMenuArenaMenuVote(%clientId, "change");
	}
	else if(%opt == arenamapnextone)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Arena::VoteMapNextOne(%clientId);
	}
	else if(%opt == arenamapnexttwo)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Arena::VoteMapNextTwo(%clientId);
	}
	else if(%opt == arenamapnextthree)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Arena::VoteMapNextThree(%clientId);
	}
	else if(%opt == arenamapnextfour)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Arena::VoteMapNextFour(%clientId);
	}
	else if(%opt == arenamapnextfive)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Arena::VoteMapNextFive(%clientId);
	}
	else
	{
		if($Arena::canBuild[%opt])
		{
			if( %opt == $Arena::CurMission)
			{
				Client::sendMessage(%clientId,1,"You are already playing "@$Arena::Name[$Arena::CurMission]@"!~wc_buysell.wav");
				return;
			}

			if(!$Arena::ActiveVote)
			{
				ArenaMSG(0,Client::getName(%clientId)@" initiated a vote to change the Arena to "@$Arena::Name[%opt]@".");
				Arena::InitiateVote("change "@%opt);
				%clientId.hasArenaVoted = true;
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
					if(%cl.inArena)
						if(!%cl.hasArenaVoted && %cl != %clientId)
						{
							TA::BlackOut(%cl,11);
							bottomprint(%cl, "<f1><jc>"@Client::getName(%clientId)@" initiated a vote to change the Arena to "@$Arena::Name[%opt]@".", 11);
							//Game::menuRequest(%cl);
						}
			}
		}
	}
}

function Arena::MapNextOne(%clientId)
{
	Client::buildMenu(%clientId, "Choose Arena", "arenaPick", true);
	Client::addMenuItem(%clientId, %curItem++ @ "BF-Chaos", "bfchaos");
	Client::addMenuItem(%clientId, %curItem++ @ "BF-DeadCity", "bfdeadcity");
	Client::addMenuItem(%clientId, %curItem++ @ "BF-Discord(bots)", "bfdiscord");
	Client::addMenuItem(%clientId, %curItem++ @ "BF-FriedDust", "bffrieddust");
	Client::addMenuItem(%clientId, %curItem++ @ "BF-TrappedArena(bots)", "bftrappedarena");
	Client::addMenuItem(%clientId, %curItem++ @ "Bloodbath(bots)", "bbath");
	Client::addMenuItem(%clientId, %curItem++ @ "Broken Fixed", "brokenfixed");
	Client::addMenuItem(%clientId, %curItem++ @ "Next Page >>>", "arenamapnexttwo");
	Client::addMenuItem(%clientId, %curItem++ @ "<<< Last Page", "arenamap"); //added back support to maps
		
}

function Arena::MapNextTwo(%clientId)
{
	Client::buildMenu(%clientId, "Choose Arena", "arenaPick", true);
	Client::addMenuItem(%clientId, %curItem++ @ "Carls Berg(bots)", "carlsberg");
	Client::addMenuItem(%clientId, %curItem++ @ "Default Arena", "default");
	Client::addMenuItem(%clientId, %curItem++ @ "Corona", "corona");
	Client::addMenuItem(%clientId, %curItem++ @ "Damn Arena(bots)", "damnarena");
	Client::addMenuItem(%clientId, %curItem++ @ "Ive Had Worse(bots)", "ivehadworse");
	Client::addMenuItem(%clientId, %curItem++ @ "King Under The Hill(bots)", "kuth");
	Client::addMenuItem(%clientId, %curItem++ @ "Knoll Arena", "knollarena");
	Client::addMenuItem(%clientId, %curItem++ @ "Next Page >>>", "arenamapnextthree");
	Client::addMenuItem(%clientId, %curItem++ @ "<<< Last Page", "arenamapnextone");
		
}

function Arena::MapNextThree(%clientId)
{
	Client::buildMenu(%clientId, "Choose Arena", "arenaPick", true);
	Client::addMenuItem(%clientId, %curItem++ @ "Laz Arena(bots)", "lazarena");
	Client::addMenuItem(%clientId, %curItem++ @ "Morena", "morena");
	Client::addMenuItem(%clientId, %curItem++ @ "New Yorker Arena", "newyorkerarena");
	Client::addMenuItem(%clientId, %curItem++ @ "Old Milwaukee(bots)", "oldmilwaukee");
	Client::addMenuItem(%clientId, %curItem++ @ "Standoff(bots)", "standoff");	
	Client::addMenuItem(%clientId, %curItem++ @ "The Arena(bots)", "thearena");
	Client::addMenuItem(%clientId, %curItem++ @ "The Hive(bots)", "hive");
	Client::addMenuItem(%clientId, %curItem++ @ "Next Page >>>", "arenamapnextfour");
	Client::addMenuItem(%clientId, %curItem++ @ "<<< Last Page", "arenamapnexttwo");
	
		
}

function Arena::MapNextFour(%clientId)
{
	Client::buildMenu(%clientId, "Choose Arena", "arenaPick", true);
	Client::addMenuItem(%clientId, %curItem++ @ "Trick-Or-Treat", "trickortreat");
	Client::addMenuItem(%clientId, %curItem++ @ "Walled In", "walledin");

	if(!$TALT::Stripped)
	{
	if(%clientId.isGoated)
	{
		Client::addMenuItem(%clientId, %curItem++ @ "Battle Cube Invo", "invobcube");
		Client::addMenuItem(%clientId, %curItem++ @ "B00zE BootCamp 3", "bbootcamp3");
		Client::addMenuItem(%clientId, %curItem++ @ "Extreme Elites (Ski)", "eeliteski");
		Client::addMenuItem(%clientId, %curItem++ @ "Ski Way (Ski)", "skiway");
	}
	}
	//Client::addMenuItem(%clientId, %curItem++ @ "Next Page...", "arenamapnextfive");
	Client::addMenuItem(%clientId, %curItem++ @ "<<< Last Page", "arenamapnextthree");
	Client::addMenuItem(%clientId, %curItem++ @ ">>> Arena Menu <<<", "back");
		
}

function Arena::VoteMapNextOne(%clientId)
{
	Client::buildMenu(%clientId, "Choose Arena", "ArenaVoteMap", true);
	Client::addMenuItem(%clientId, %curItem++ @ "BF-Chaos", "bfchaos");
	Client::addMenuItem(%clientId, %curItem++ @ "BF-DeadCity", "bfdeadcity");
	Client::addMenuItem(%clientId, %curItem++ @ "BF-Discord(bots)", "bfdiscord");
	Client::addMenuItem(%clientId, %curItem++ @ "BF-FriedDust", "bffrieddust");
	Client::addMenuItem(%clientId, %curItem++ @ "BF-TrappedArena(bots)", "bftrappedarena");
	Client::addMenuItem(%clientId, %curItem++ @ "Bloodbath(bots)", "bbath");
	Client::addMenuItem(%clientId, %curItem++ @ "Broken Fixed", "brokenfixed");
	Client::addMenuItem(%clientId, %curItem++ @ "Next Page >>>", "arenamapnexttwo");
	Client::addMenuItem(%clientId, %curItem++ @ "<<< Last Page", "arenamap"); //added back support to maps
		
}

function Arena::VoteMapNextTwo(%clientId)
{
	Client::buildMenu(%clientId, "Choose Arena", "ArenaVoteMap", true);
	Client::addMenuItem(%clientId, %curItem++ @ "Carls Berg(bots)", "carlsberg");
	Client::addMenuItem(%clientId, %curItem++ @ "Default Arena", "default");
	Client::addMenuItem(%clientId, %curItem++ @ "Corona", "corona");
	Client::addMenuItem(%clientId, %curItem++ @ "Damn Arena(bots)", "damnarena");
	Client::addMenuItem(%clientId, %curItem++ @ "Ive Had Worse(bots)", "ivehadworse");
	Client::addMenuItem(%clientId, %curItem++ @ "King Under The Hill(bots)", "kuth");
	Client::addMenuItem(%clientId, %curItem++ @ "Knoll Arena", "knollarena");
	Client::addMenuItem(%clientId, %curItem++ @ "Next Page >>>", "arenamapnextthree");
	Client::addMenuItem(%clientId, %curItem++ @ "<<< Last Page", "arenamapnextone");
		
}

function Arena::VoteMapNextThree(%clientId)
{
	Client::buildMenu(%clientId, "Choose Arena", "ArenaVoteMap", true);
	Client::addMenuItem(%clientId, %curItem++ @ "Laz Arena(bots)", "lazarena");
	Client::addMenuItem(%clientId, %curItem++ @ "Morena", "morena");
	Client::addMenuItem(%clientId, %curItem++ @ "New Yorker Arena", "newyorkerarena");
	Client::addMenuItem(%clientId, %curItem++ @ "Old Milwaukee(bots)", "oldmilwaukee");
	Client::addMenuItem(%clientId, %curItem++ @ "Standoff(bots)", "standoff");	
	Client::addMenuItem(%clientId, %curItem++ @ "The Arena(bots)", "thearena");
	Client::addMenuItem(%clientId, %curItem++ @ "The Hive(bots)", "hive");
	Client::addMenuItem(%clientId, %curItem++ @ "Next Page >>>", "arenamapnextfour");
	Client::addMenuItem(%clientId, %curItem++ @ "<<< Last Page", "arenamapnexttwo");
		
}

function Arena::VoteMapNextFour(%clientId)
{
	Client::buildMenu(%clientId, "Choose Arena", "ArenaVoteMap", true);
	Client::addMenuItem(%clientId, %curItem++ @ "Trick-Or-Treat", "trickortreat");
	Client::addMenuItem(%clientId, %curItem++ @ "Walled In", "walledin");

	//if(!$TALT::Stripped)
	//{
		//Client::addMenuItem(%clientId, %curItem++ @ "Battle Cube Invo", "invobcube");
		//Client::addMenuItem(%clientId, %curItem++ @ "B00zE BootCamp 3", "bbootcamp3");
		//Client::addMenuItem(%clientId, %curItem++ @ "Extreme Elites (Ski)", "eeliteski");
		//if(%clientId.isGoated)
			//Client::addMenuItem(%clientId, %curItem++ @ "Ski Way (Ski)", "skiway");
	//}
	//Client::addMenuItem(%clientId, %curItem++ @ "Next Page...", "arenamapnextfive");
	Client::addMenuItem(%clientId, %curItem++ @ "<<< Last Page", "arenamapnextthree");
	Client::addMenuItem(%clientId, %curItem++ @ ">>> Arena Menu <<<", "back");
		
}

function ArenaMSG(%opt,%words)
{
	if(%opt == "")
		%opt = 0;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.inArena)
		   Client::sendMessage(%cl,%opt,%words);
	}
}

function Arena::InitiateVote(%type)
{
	schedule("Arena::processVote(\""@ %type @"\");",10+(getNumClients()*1));

	$Arena::VotesFor = 1;
	$Arena::VotesAgainst = 0;
	$Arena::VotesAbstained = 0;
	$Arena::activeVote = true;

	%new = getWord(%type,1);
	%type = getWord(%type,0);

	if(%type == enable)
	   $Arena::curVote = "Enable the Arena";
	else if(%type == disable)
	   $Arena::curVote = "Disable the Arena";
	else if(%type == change && %new != -1)
	{
	   $Arena::curVote = "Change the Arena";
	   $Arena::VoteMission = %new;
	}
	else if(%type == vannispawn)
	   $Arena::curVote = "Enable Annihilation";
	else if(%type == velitespawn)
	   $Arena::curVote = "Enable EliteRenegades";
	else if(%type == vbasespawn)
	   $Arena::curVote = "Enable Base"; 
   	else if(%type == vtitanspawn)
	   $Arena::curVote = "Enable Titans"; 
    else if(%type == vbuilderspawn)
	   $Arena::curVote = "Enable Builders"; 
    else if(%type == vdiscspawn)
	   $Arena::curVote = "Enable Discs";
    else if(%type == vnormal)
	   $Arena::curVote = "Enable Default"; 
    else if(%type == vmaxammo)
	   $Arena::curVote = "Enable MaxAmmo"; 
	else if(%type == vearenawin)
	   $Arena::curVote = "Enable Winners Mode";
	else if(%type == vdarenawin)
	   $Arena::curVote = "Disable Winners Mode";

	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		%cl.hasArenaVoted = false;
		//if(%cl.inArena && %cl != %clientId)
		//	Game::menuRequest(%cl); 
	}
	
	
}

function Arena::processVote(%type)
{
	if(!$Arena::ActiveVote) return;

	%map = getWord(%type,1);
	%type = getWord(%type,0);

	%for = $Arena::VotesFor;
	%aga = $Arena::VotesAgainst;
	%abs = $Arena::VotesAbstained;

	echo("ARENA PROCESS VOTE "@%type@": "@%for@" to "@%aga@" with "@%abs@" abstentions");

	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.inArena)
		{
			if(!%cl.hasArenaVoted)
			{
				%abs++;
			}
		}
		else if(!$Arena::Initialized)
		{
			if(!%cl.hasArenaVoted)
			{
				%abs++;
			}
		}
	}

	if(%for > %aga)
	   %str = "PASSED";
	else if(%for == %aga)
	   %str = "TIED";
	else if(%for < %aga)
	   %str = "FAILED";

	if(%type == "enable" && !$Arena::Initialized)
	{
		%change = "enable the Arena";
		$Arena::ActiveVote = false;

		if(%str == "PASSED")
		   Arena::Init($Arena::FirstMap);

		MessageAll(0,"Vote to "@%change@" "@%str@" "@%for@" to "@%aga@" with "@%abs@" abstentions.");
		return;
	}
	else if(%type == "disable" && $Arena::Initialized)
	{
		%change = "disable the Arena";

		if(%str == "PASSED")
		   Arena::Clear();		
	}
	else if(%type == "change")
	{
		%change = "change the Arena to "@$Arena::Name[%map];

		if(%str == "PASSED" && $Arena::canBuild[$Arena::voteMission])
		{
			if($Arena::Initialized)
			{
				$Arena::Mapchange = true;
				Arena::Clear();
			}

			Arena::Init($Arena::voteMission);
		}
	}
	if(%type == "vannispawn")
	{
		%change = "enable the Annihilation Mod";
		$Arena::ActiveVote = false;

		if(%str == "PASSED")
		{
		   $TAArena::SpawnType = "AnniSpawn";
		   if($Arena::Initialized)
			{
				$Arena::Mapchange = true;
				Arena::Clear();
			}

			Arena::Init($Arena::curMission);
		   //TAArena::SpawnReset();
		}

		MessageAll(0,"Vote to "@%change@" "@%str@" "@%for@" to "@%aga@" with "@%abs@" abstentions.");
		return;
	}
	if(%type == "velitespawn")
	{
		%change = "enable the EliteRenegades Mod";
		$Arena::ActiveVote = false;

		if(%str == "PASSED")
		{
		   $TAArena::SpawnType = "EliteSpawn";
		   if($Arena::Initialized)
			{
				$Arena::Mapchange = true;
				Arena::Clear();
			}

			Arena::Init($Arena::curMission);
		}

		MessageAll(0,"Vote to "@%change@" "@%str@" "@%for@" to "@%aga@" with "@%abs@" abstentions.");
		return;
	}
	if(%type == "vbasespawn")
	{
		%change = "enable the Base Mod";
		$Arena::ActiveVote = false;

		if(%str == "PASSED")
		{
		  $TAArena::SpawnType = "BaseSpawn";
		  if($Arena::Initialized)
			{
				$Arena::Mapchange = true;
				Arena::Clear();
			}

			Arena::Init($Arena::curMission);
		}

		MessageAll(0,"Vote to "@%change@" "@%str@" "@%for@" to "@%aga@" with "@%abs@" abstentions.");
		return;
	}
	if(%type == "vtitanspawn")
	{
		%change = "enable Titans in Arena";
		$Arena::ActiveVote = false;

		if(%str == "PASSED")
		{
		  $TAArena::SpawnType = "TitanSpawn";
		  if($Arena::Initialized)
			{
				$Arena::Mapchange = true;
				Arena::Clear();
			}

			Arena::Init($Arena::curMission);
		}

		MessageAll(0,"Vote to "@%change@" "@%str@" "@%for@" to "@%aga@" with "@%abs@" abstentions.");
		return;
	}
		if(%type == "vbuilderspawn")
	{
		%change = "enable Builders in Arena";
		$Arena::ActiveVote = false;

		if(%str == "PASSED")
		{
		  $TAArena::SpawnType = "BuilderSpawn";
		  if($Arena::Initialized)
			{
				$Arena::Mapchange = true;
				Arena::Clear();
			}

			Arena::Init($Arena::curMission);
		}

		MessageAll(0,"Vote to "@%change@" "@%str@" "@%for@" to "@%aga@" with "@%abs@" abstentions.");
		return;
	}
	    if(%type == "vdiscspawn")
	{
		%change = "enable discs in Arena";
		$Arena::ActiveVote = false;

		if(%str == "PASSED")
		{
		  $TAArena::WeaponOpt = "DiscOnly";
		  if($Arena::Initialized)
			{
				$Arena::Mapchange = true;
				Arena::Clear();
			}

			Arena::Init($Arena::curMission);
		}

		MessageAll(0,"Vote to "@%change@" "@%str@" "@%for@" to "@%aga@" with "@%abs@" abstentions.");
		return;
	}
	    if(%type == "vnormal")
	{
		%change = "enable default loadouts in Arena";
		$Arena::ActiveVote = false;

		if(%str == "PASSED")
		{
		  $TAArena::WeaponOpt = "Normal";
		  if($Arena::Initialized)
			{
				$Arena::Mapchange = true;
				Arena::Clear();
			}

			Arena::Init($Arena::curMission);
		}

		MessageAll(0,"Vote to "@%change@" "@%str@" "@%for@" to "@%aga@" with "@%abs@" abstentions.");
		return;
	}
		if(%type == "vmaxammo")
	{
		%change = "enable maximum ammo in Arena";
		$Arena::ActiveVote = false;

		if(%str == "PASSED")
		{
		  $TAArena::WeaponOpt = "MaxAmmo";
		  if($Arena::Initialized)
			{
				$Arena::Mapchange = true;
				Arena::Clear();
			}

			Arena::Init($Arena::curMission);
		}

		MessageAll(0,"Vote to "@%change@" "@%str@" "@%for@" to "@%aga@" with "@%abs@" abstentions.");
		return;
	}
	if(%type == "vearenawin")
	{
		%change = "enable Winners Mode";
		$Arena::ActiveVote = false;

		if(%str == "PASSED")
		{
			ArenaWin::ClearStage();
		}

		MessageAll(0,"Vote to "@%change@" "@%str@" "@%for@" to "@%aga@" with "@%abs@" abstentions.");
		return;
	}
	if(%type == "vdarenawin")
	{
		%change = "disable Winners Mode";
		$Arena::ActiveVote = false;

		if(%str == "PASSED")
		{
			ArenaWin::End();
		}

		MessageAll(0,"Vote to "@%change@" "@%str@" "@%for@" to "@%aga@" with "@%abs@" abstentions.");
		return;
	}

	$Arena::ActiveVote = false;

	ArenaMSG(0,"Vote to "@%change@" "@%str@" "@%for@" to "@%aga@" with "@%abs@" abstentions.");
}

function processMenuArenaPick(%clientId,%opt)
{
	%client = Player::getClient(%clientId);
	if(%opt == back)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Arena::Opts(%clientId);
		return;
	}
	else if(%opt == arenamap)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		processMenuArenaMenu(%clientId, "change");
	}
	else if(%opt == arenamapnextone)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Arena::MapNextOne(%clientId);
	}
	else if(%opt == arenamapnexttwo)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Arena::MapNextTwo(%clientId);
	}
	else if(%opt == arenamapnextthree)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Arena::MapNextThree(%clientId);
	}
	else if(%opt == arenamapnextfour)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Arena::MapNextFour(%clientId);
	}
	else if(%opt == arenamapnextfive)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Arena::MapNextFive(%clientId);
	}

	if($Arena::CanBuild[%opt])
	{
		if($Arena::Initialized)
		{
		   $Arena::Mapchange = true;
		   Arena::Clear();
		}

		Arena::Init(%opt);

		Messageall(0,Client::getName(%clientId)@" changed the Arena to "@$Arena::Name[%opt]@".~wcapturedtower.wav");
	}
	else
	   return;
}

function processMenuArenaSpawnType(%clientId,%opt)
{
	%client = Player::getClient(%clientId);
	if(%opt == back)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Arena::Opts(%clientId);
		return;
	}
	else if(%opt == "annispawn")
	{
		$TAArena::SpawnType = "AnniSpawn";
		if($Arena::Initialized)
			{
				$Arena::Mapchange = true;
				Arena::Clear();
			}

			Arena::Init($Arena::curMission);
		Messageall(0,Client::getName(%clientId)@" changed the Arena Spawn Type to Annihilation.~wcapturedtower.wav");
	}
	else if(%opt == "elitespawn")
	{
		$TAArena::SpawnType = "EliteSpawn";
		if($Arena::Initialized)
			{
				$Arena::Mapchange = true;
				Arena::Clear();
			}

			Arena::Init($Arena::curMission);
		Messageall(0,Client::getName(%clientId)@" changed the Arena Spawn Type to EliteRenegades.~wcapturedtower.wav");
	}
	else if(%opt == "basespawn")
	{
		$TAArena::SpawnType = "BaseSpawn";
		if($Arena::Initialized)
			{
				$Arena::Mapchange = true;
				Arena::Clear();
			}

			Arena::Init($Arena::curMission);
		Messageall(0,Client::getName(%clientId)@" changed the Arena Spawn Type to Base.~wcapturedtower.wav");
	}
	else if(%opt == "normal")
	{
				if($Arena::Initialized)
			{
				$Arena::Mapchange = true;
				Arena::Clear();
			}

			Arena::Init($Arena::curMission);
		$TAArena::WeaponOpt = "Normal";
		Messageall(0,Client::getName(%clientId)@" changed the Arena Weapon Option to Normal.~wcapturedtower.wav");
	}
	else if(%opt == "disconly")
	{
				if($Arena::Initialized)
			{
				$Arena::Mapchange = true;
				Arena::Clear();
			}

			Arena::Init($Arena::curMission);
		$TAArena::WeaponOpt = "DiscOnly";
		Messageall(0,Client::getName(%clientId)@" changed the Arena Weapon Option to Disc Only.~wcapturedtower.wav");
	}
	else if(%opt == "rocketonly")
	{
				if($Arena::Initialized)
			{
				$Arena::Mapchange = true;
				Arena::Clear();
			}

			Arena::Init($Arena::curMission);
		$TAArena::WeaponOpt = "RocketOnly";
		Messageall(0,Client::getName(%clientId)@" changed the Arena Weapon Option to Rocket Only.~wcapturedtower.wav");
	}
	else if(%opt == "maxammo")
	{
				if($Arena::Initialized)
			{
				$Arena::Mapchange = true;
				Arena::Clear();
			}

			Arena::Init($Arena::curMission);
		$TAArena::WeaponOpt = "MaxAmmo";
		Messageall(0,Client::getName(%clientId)@" changed the Arena Weapon Option to Max Ammo.~wcapturedtower.wav");
	}
	else if(%opt == "titanspawn")
	{
		$TAArena::SpawnType = "TitanSpawn";
		if($Arena::Initialized)
		{
			$Arena::Mapchange = true;
			Arena::Clear();
		}

			Arena::Init($Arena::curMission);
		Messageall(0,Client::getName(%clientId)@" changed the Arena Spawn Type to Titan.~wcapturedtower.wav");
	}
	else if(%opt == "builderspawn")
	{
		$TAArena::SpawnType = "BuilderSpawn";
		if($Arena::Initialized)
		{
			$Arena::Mapchange = true;
			Arena::Clear();
		}

			Arena::Init($Arena::curMission);
		Messageall(0,Client::getName(%clientId)@" changed the Arena Spawn Type to Builder.~wcapturedtower.wav");
	}
	else
	   return;
}

function Arena::Leave(%clientId,%mapchange) 
{
	%player = Client::getOwnedObject(%clientId);
	Game::resetScores(%clientId);
	
	if(!%mapchange)
	{
		if(%clientId.inArenaWin)
			ArenaWin::Finish(%clientId,false);
		//Messageall(0,Client::getName(%cl)@" has left the Arena. (#arena)~wshell_click.wav");
		%clientId.inArena = false;
		%clientId.isArenaWin = "";
		%clientId.inArenaWin = "";
		%clientId.clArenaWin = "";
		%clientId.isArenaLoser = "";
		if(Observer::enterObserverMode(%clientId)) 
			Game::refreshClientScore(%clientId);
		else
			Messageall(0,Client::getName(%clientId)@" is no longer on a team.");
		
		if(!$Server::TourneyMode && !$Arena::NoForceTeam)
		{
			//if($Arena::NoForceTeam)
			//{
				//$Arena::NoForceTeam = false;
			//}
			//else
			//{
				processMenuPickTeam(%clientId, -1); 
				//playNextAnim(%clientId);
				//Player::kill(%clientId);
				//Game::playerSpawn(%clientId, true);
			//}
		}
		Game::refreshClientScore(%clientId);
		
	}
	else
	{
		if(isObject(%player))
		{
			//Let's try this
			playNextAnim(%clientId);
			Player::kill(%clientId);
		}
	}

	//Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
}

function Arena::Clear()
{ // get rid of the arena, will be used in Arena "map changes" later on

	if($Arena::Initialized)
	{
		echo("!! Clearing The Arena !!");
		
//		if($TA::CubeBot)
//			$TA::CubeBot = false;
			
		if($TA::PullMA)
			$TA::PullMA = false;
		
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			%player = Client::getOwnedObject(%cl);

			if(%cl.inArena && $loadingMission)
			{
				Game::assignClientTeam(%cl);
				%team = Client::getTeam(%cl);
				GameBase::setTeam(%player,%team);
				Client::setInitialTeam(%cl,%team);

				//%r = true;
			}
			else//if(!%r) 
			{
				if(%cl.inArena && !$Arena::Mapchange)
				{
					ArenaTD::Leave(%cl);
					Arena::Leave(%cl);
	
					//messageall(1, "The Arena has been disabled.");
					client::sendmessage(%cl, 1, "The Arena has been disabled."); 
				}
				else if(%cl.inArena && $Arena::Mapchange) //lol
				{
					//ArenaTD::Leave(%cl,true);
					Arena::Leave(%cl,true);

					Centerprint(%cl,"<jc><f3>The <f2>Arena<f3> is loading, please wait.",10);
				}
			}

			Game::refreshClientScore(%cl);
	
			for(%i = 0; $ArenaPart[%i] != ""; %i++)
			{
				%obj = $ArenaPart[%i];
				if(isObject(%obj) && %obj.isArenaPart)
				{
					deleteObject(%obj);
				}
			}
	
			if(isObject(NameToID("MissionCleanup/Arena/Spawnpoints")))
			   deleteObject(NameToID("MissionCleanup/Arena/Spawnpoints"));
	
			if(isObject(NameToID("MissionCleanup/Arena")))	
			   deleteObject(NameToID("MissionCleanup/Arena"));

			deleteVariables("ArenaPart[*");
			//deleteVariables("Arena::object*"); 
			$Arena::Initialized = false;
			$Arena::Terrain = false;
			$Arena::ActiveVote = false;
			
		}
	}
	else
	{
		//
	}
}

function Arena::Join(%clientId)
{
	%player = Client::getOwnedObject(%clientId);
	Game::resetScores(%clientId);
	
	if($Arena::Lock)
	{
		client::sendmessage(%clientId, 1, "The Arena has been locked.");
		return;
	} 
	
	if(%clientId.isArenaBanned)
	{
		client::sendmessage(%clientId, 1, "You have been banned from the the Arena");
		return;
	}

	if(%clientId.inArena)
	{
		client::sendmessage(%clientId, 1, "You're already in the Arena.");
		return;
	}

	if(Player::getItemCount(%player,Flag) > 0)
	{
		Player::dropItem(%clientId,Flag);
	}

	
	if(player::status(%clientId) == "(Observing)")
	{
		//echo("teamchange");
		if($ArenaTD::Active)
			ArenaTD::ArenaJoin(%clientId,"Obs");
		else
			Arena::JoinObs(%clientId);
	}
	else if(player::status(%clientId) == "(Dead)")
	{
		//echo("dead");
		if($ArenaTD::Active)
			ArenaTD::ArenaJoin(%clientId,"Dead");
		else
			Arena::JoinDead(%clientId);
	}
	else
	{
		//echo("join");
		if($ArenaTD::Active)
			ArenaTD::ArenaJoin(%clientId,"Live");
		else
			Arena::JoinTwo(%clientId);
	}
	
	
}

function Arena::JoinObs(%clientId)
{
	//echo("teamchange2");
	//%player = Client::getOwnedObject(%clientId);
	//schedule("Client::setInitialTeam(" @ %clientId @ ", 1);", 0.1);
	//schedule("GameBase::setTeam(" @ %clientId @ ", 1);", 0.1);
	//schedule("GameBase::setTeam(" @ %player @ ", 1);", 0.1);
	//schedule("Game::playerSpawn("@%clientId@", true);", 0.4);
	
	schedule("processMenuPickTeam("@%clientId@", -1);", 0.1); 
	schedule("Arena::JoinTwo(" @ %clientId @ ");", 0.2);
}

function Arena::JoinDead(%clientId)
{
	//echo("dead2");
	schedule("Game::playerSpawn("@%clientId@", true);", 0.1);
	schedule("Arena::JoinTwo(" @ %clientId @ ");", 0.2);
}

function Arena::Strip(%clientId)
{
	echo("preparing for arena!!");
	%player = Client::getOwnedObject(%clientId);
	%Player.rThrow = true;
	%player.rThStr = 10;
	for(%x = 0; %x < 15; %x = %x++)
	{		
		%item = Player::getMountedItem(%clientId,$WeaponSlot);
		//if(!%item) return;
		Player::trigger(%player, $WeaponSlot, false);
		Player::dropItem(%clientId,%item);
		remoteNextWeapon(%clientId);
		
		//Mines too
		%mine = "MineAmmo";
		%count = Player::getItemCount(%clientId, %mine);
		if(%count > 0)
			Player::dropItem(%clientId,%mine);
	}
	%Player.rThrow = "";
	%player.rThStr = "";
}

function Arena::JoinTwo(%clientId)
{
	//echo("join2");
	%player = Client::getOwnedObject(%clientId);

		if ($jailed[%player] == true)
		{
			Client::sendMessage(%clientId,0, "Unable to join arena while jailed. ~wC_BuySell.wav");
			return;
		}

		if (%player.frozen == true) // adding for frozen as well -death666 3.29.17
		{
			Client::sendMessage(%clientId,0, "Unable to join arena while frozen. ~wC_BuySell.wav");
			return;
		}

	if(isObject(%player) && getObjectType(%player) == "Player" && $Arena::Initialized && !$Arena::Mapchange)
	{
		//echo("join3");
		%spawn = Arena::pickRandomSpawn(%clientId);

		if(!%spawn)
		{
			Centerprint(%clientId,"<jc><f1>The Arena is full please wait.",6);
			echo("!! Arena spawnpoints not found");
			return;
		}

		%clientId.inArena = true;
		//%clientId.intTeam = GameBase::getTeam(%clientId);
		Arena::Strip(%clientId);

		//%spawnPos = GameBase::getPosition(%spawn);
		//%spawnRot = GameBase::getRotation(%spawn);

		//Item::setVelocity(%player,"0 0 0");
		//GameBase::setPosition(%player,%spawnPos);
		//GameBase::setRotation(%player,%spawnRot);

		//Client::setInitialTeam(%clientId,0);
		GameBase::setTeam(%clientId,0);
		//GameBase::setTeam(%player,0);

		//Now lets respawn them
		playNextAnim(%clientId);
		player::kill(%clientId);
		if($Arena::Winners)
		{
			//%cl.observerMode = "observerOrbit";
			Client::sendMessage(clientId, 0, "Arena Winners is on. Please wait for your turn to fight.");
		}
		else
			Game::playerSpawn(%clientId, true);
		Game::refreshClientScore(%clientId);

		Messageall(0,Client::getName(%clientId)@" has entered the Arena.~wshell_click.wav"); 
	}
	else
	   Centerprint(%clientId,"<jc><f1>The Arena is loading please wait.",6);
}

function checkArenaSpawnArea(%pos)
{
	%set=newObject("set",SimSet);
	%num=containerBoxFillSet(%set,$SimPlayerObjectType,%pos,2,2,4,0);
	if(!%num) 
	{
		deleteObject(%set);
		return true;
	}
	else
	{
		deleteObject(%set);
		return false;
	}
}

function Arena::pickRandomSpawn(%clientId)
{
	if($ArenaTD::Active && %clientId.inArenaTD)
	{
		if(%clientId.inArenaTDOne)
		{
			%group = nameToID("MissionCleanup/Arena/SpawnPoints1");
			%count = Group::objectCount(%group);

			if(!%count)
				return;

			for(%i = 0; %i < %count; %i++)
			{
				%spawnIdx = Group::getObject(%group, %i);

				//echo(%spawnIdx@"  ><<<<<<< Arena TD spawn0");
				%pos = GameBase::getPosition(%spawnIdx);
				if(checkArenaSpawnArea(%pos) == true)
					break;
			}

			return %spawnIdx;
		}
		else if(%clientId.inArenaTDTwo)
		{
			%group = nameToID("MissionCleanup/Arena/SpawnPoints2");
			%count = Group::objectCount(%group);
			if(!%count)
				return;

			for(%i = 0; %i < %count; %i++)
			{
				%spawnIdx = Group::getObject(%group, %i);

				//echo(%spawnIdx@"  ><<<<<<< Arena TD spawn1");
				%pos = GameBase::getPosition(%spawnIdx);
				if(checkArenaSpawnArea(%pos) == true)
					break;
			}

			return %spawnIdx;
		}
	}
	else
	{
		%group = nameToID("MissionCleanup/Arena/SpawnPoints3");
		%count = Group::objectCount(%group);

		if(!%count)
			return;

		%spawnIdx = floor(getRandom() * (%count - 0.1));
	
		%obj = Group::getObject(%group,%spawnIdx);

		return %obj;
	}
}

function Arena::pickRandomSpawnXXX(%clientId)
{
	if($ArenaTD::Active && %clientId.inArenaTD)
	{
		if(%clientId.inArenaTDOne)
		{
			%group = nameToID("MissionCleanup/Arena/SpawnPoints0");
			%count = Group::objectCount(%group);

			if(!%count)
				return;
			
			for(%i = 0; (%spawnIdx = Group::getObject(%group, %i)) != -1; %i++)
			{
				echo(%spawnIdx@"  ><<<<<<< Arena TD spawn0");
				if(checkArenaSpawnArea(%spawnIdx) == true)
					break;
			}
	
			%obj = Group::getObject(%group,%spawnIdx);

			%spawnPos = GameBase::getPosition(%obj);
			%spawnRot = GameBase::getRotation(%obj);
	
			return %obj;
		}
		else if(%clientId.inArenaTDTwo)
		{
			%group = nameToID("MissionCleanup/Arena/SpawnPoints1");
			%count = Group::objectCount(%group);

			if(!%count)
				return;

			for(%i = 0; (%spawnIdx = Group::getObject(%group, %i)) != -1; %i++)
			{
				echo(%spawnIdx@"  ><<<<<<< Arena TD spawn0");
				if(checkArenaSpawnArea(%spawnIdx) == true)
					break;
			}
	
			%obj = Group::getObject(%group,%spawnIdx);

			%spawnPos = GameBase::getPosition(%obj);
			%spawnRot = GameBase::getRotation(%obj);
	
			return %obj;
		}
	}
	else
	{
		%group = nameToID("MissionCleanup/Arena/SpawnPoints");
		%count = Group::objectCount(%group);

		if(!%count)
			return;

		%spawnIdx = floor(getRandom() * (%count - 0.1));
	
		%obj = Group::getObject(%group,%spawnIdx);

		%spawnPos = GameBase::getPosition(%obj);
		%spawnRot = GameBase::getRotation(%obj);

		return %obj;
	}
}

function Arena::Init(%name)
{
	
	if ($Spoonbot::AutoSpawn)
	{
		// Arena::Clear();	
		return;
	}
	
	if($debug)
		echo(%name@"  <<<<<<<<<<<< Arena::Init(%opt)");
		
	if($Arena::canBuild[%name] == true && !$Arena::Initialized)
	{
		exec(%name); //load the arena map 
		$Arena::Spawn = TA::pickWaypoint("arena");
		$Arena::Initialized = true; // so so true
		$Arena::curMission = %name;
		if(%name == "arenaterrain")
			$Arena::Terrain = true;
		else
			$Arena::Terrain = false;
			
		if($ArenaTD::Active && $Arena::Terrain)
			$ArenaTD::TerrainPos = TA::pickWaypoint("toob");
			


		%arenaGroup = NewObject("Arena",SimGroup);
		AddToSet("MissionCleanup",%arenaGroup);
 
		%arenaGroup = NewObject("Spawnpoints3",SimGroup);
		AddToSet("MissionCleanup\\Arena",%arenaGroup);
		
		%arenaGroup = NewObject("Spawnpoints1",SimGroup);
		AddToSet("MissionCleanup\\Arena",%arenaGroup);
		
		%arenaGroup = NewObject("Spawnpoints2",SimGroup);
		AddToSet("MissionCleanup\\Arena",%arenaGroup);

		//if($debug)
		   echo("!! Arena being created !!");
		

		for(%i = 0; $Arena::Object[%name,%i] != ""; %i++)
		{
			%arenaObject = $Arena::Object[%name,%i];
			%shape = getWord(%arenaObject,0);
			//
			%posX = getWord(%arenaObject,1);
			%posY = getWord(%arenaObject,2);
			%posZ = getWord(%arenaObject,3);
			%pos = %posX@" "@%posY@" "@%posZ;
			//
			%rotX = getWord(%arenaObject,4);
			%rotY = getWord(%arenaObject,5);
			%rotZ = getWord(%arenaObject,6);
			%rot = %rotX@" "@%rotY@" "@%rotZ;
			if(String::findSubStr(%shape,"spawn1") != -1)
			{
				if($Arena::Terrain)
				{
					%type = "Spawn Point";
					%shape = DropPointMarker;
	
					%obj = NewObject("Spawnpoint",Marker,DropPointMarker,true);
					AddToSet("MissionCleanup\\Arena\\Spawnpoints1",%obj);
					//GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$ArenaTD::Spawn[0]));
					GameBase::setPosition(%obj,%pos);
					GameBase::setRotation(%obj,%rot);
					%obj.isArenaPart = true;
					$ArenaPart[%i] = %obj;
					%return = true;
				}
				else
				{
					%type = "Spawn Point";
					%shape = DropPointMarker;
	
					%obj = NewObject("Spawnpoint",Marker,DropPointMarker,true);
					AddToSet("MissionCleanup\\Arena\\Spawnpoints1",%obj);
					GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$Arena::Spawn));
					GameBase::setRotation(%obj,%rot);
					%obj.isArenaPart = true;
					$ArenaPart[%i] = %obj;
					%return = true;
				}

				if($debug)
				   echo("!! Piece #"@%i@" added ("@%type@" "@%shape@")");
			}
			else if(String::findSubStr(%shape,"spawn2") != -1)
			{
				if($Arena::Terrain)
				{
					%type = "Spawn Point";
					%shape = DropPointMarker;
	
					%obj = NewObject("Spawnpoint",Marker,DropPointMarker,true);
					AddToSet("MissionCleanup\\Arena\\Spawnpoints2",%obj);
					//GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$TA::Spawn[0]));
					GameBase::setPosition(%obj,%pos);
					GameBase::setRotation(%obj,%rot);
					%obj.isArenaPart = true;
					$ArenaPart[%i] = %obj;
					%return = true;
				}
				else
				{
					%type = "Spawn Point";
					%shape = DropPointMarker;
	
					%obj = NewObject("Spawnpoint",Marker,DropPointMarker,true);
					AddToSet("MissionCleanup\\Arena\\Spawnpoints2",%obj);
					GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$Arena::Spawn));
					GameBase::setRotation(%obj,%rot);
					%obj.isArenaPart = true;
					$ArenaPart[%i] = %obj;
					%return = true;
				}

				if($debug)
				   echo("!! Piece #"@%i@" added ("@%type@" "@%shape@")");
			}
			else if(String::findSubStr(%shape,"spawn3") != -1)
			{
				%type = "Spawn Point";
				%shape = DropPointMarker;

				%obj = NewObject("Spawnpoint",Marker,DropPointMarker,true);
				AddToSet("MissionCleanup\\Arena\\Spawnpoints3",%obj);
				//if($Arena::Terrain)
				//	GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$ArenaTD::Spawn[0]));
				//else
					GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$Arena::Spawn));
				GameBase::setRotation(%obj,%rot);
				%obj.isArenaPart = true;
				$ArenaPart[%i] = %obj;
				%return = true;

				if($debug)
				   echo("!! Piece #"@%i@" added ("@%type@" "@%shape@")");
			}

			if(!%return)
			{
				if(String::findSubStr(%shape,".dis") != -1)
				{
				   %type = InteriorShape;
				   //%tits = false; // like akkichu :D
				}
				else if(String::findSubStr(%shape,"PortGenerator") != -1) 
				{
					%shape = "Generator";
					%type = StaticShape;
				}
				else if(String::findSubStr(%shape,"Generator") != -1)
				{
					%type = StaticShape;
				}
				else if(String::findSubStr(%shape,"PulseSensor") != -1)
				{
					%type = StaticShape;
				}
				else if(String::findSubStr(%shape,"AmmoStation") != -1)
				{
					%type = StaticShape;
				}
				else if(String::findSubStr(%shape,"InventoryStation") != -1)
				{
					%type = StaticShape;
				}
				else if(String::findSubStr(%shape,"ForceField") != -1)
				{
					%type = StaticShape;
				}
				else if(String::findSubStr(%shape,"ForceField4") != -1)
				{
					%type = StaticShape;
				}
				else if(String::findSubStr(%shape,"TowerSwitch") != -1)
				{
					%type = StaticShape;
				}
				else if(String::findSubStr(%shape,"GrenadeAmmo") != -1) 
				{
					if($TAArena::SpawnType == "AnniSpawn") //and now I added support for different spawns
						%shape = "GrenadeAmmoBase";
					else if($TAArena::SpawnType == "EliteSpawn") //but this only works at the spawn of the map. Doh!
						%shape = "GrenadeAmmoBase";
					else if($TAArena::SpawnType == "BaseSpawn")
						%shape = "GrenadeAmmoBase";
					%type = Item;
					%num = "5";
				}
				else if(String::findSubStr(%shape,"PlasmaAmmo") != -1)
				{
					if($TAArena::SpawnType == "AnniSpawn")
						%shape = "PlasmaAmmo";
					else if($TAArena::SpawnType == "EliteSpawn")
						%shape = "PlasmaAmmoBase";
					else if($TAArena::SpawnType == "BaseSpawn")
						%shape = "PlasmaAmmoBase";
					%type = Item;
					%num = "5";
				}
				else if(String::findSubStr(%shape,"DiscAmmo") != -1)
				{
					if($TAArena::SpawnType == "AnniSpawn")
						%shape = "DiscAmmo";
					else if($TAArena::SpawnType == "EliteSpawn")
						%shape = "DiscAmmoElite";
					else if($TAArena::SpawnType == "BaseSpawn")
						%shape = "DiscAmmoBase";
					%type = Item;
					%num = "5";
				}
				else if(String::findSubStr(%shape,"Grenade") != -1)
				{
					%type = Item;
					%num = "5";
				}
				else if(String::findSubStr(%shape,"SpringPad") != -1)
				{
					%type = StaticShape;
					%num = "1";
				}
				else if(String::findSubStr(%shape,"ElectricalBeamBig") != -1)
				{
					%type = StaticShape;
					%num = "1";
				}
				else if(String::findSubStr(%shape,"TreeShape") != -1)
				{
					%type = StaticShape;
					%num = "1";
				}
				else if(String::findSubStr(%shape,"TreeShapeTwo") != -1) 
				{
					%type = StaticShape;
					%num = "1";
				}
				else
				{
					%type = Item;
					%num = "1";
				}
				if(%type == InteriorShape) //%flag = NewObject("Flag","item",Flag,false); %pPos = GameBase::getPosition(2049); GameBase::setPosition(%flag,Vector::add(%pPos,"0 0 5")); AddToSet("MissionCleanup\\Arena",%flag);
				{							//GameBase::setPosition(NewObject("Flag","item",Flag,false),Vector::add(GameBase::getPosition(2049),"0 0 5")); // AddToSet("MissionCleanup\\Arena",NewObject("Flag","item",Flag,false));
					%obj = NewObject(%shape,%type,%shape,false);
					AddToSet("MissionCleanup\\Arena",%obj);
					GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$Arena::Spawn));
					GameBase::setRotation(%obj,%rot);
					%obj.isArenaPart = true;
					$ArenaPart[%i] = %obj;
				}
				else if(%type == StaticShape)
				{
					if(%shape == AmmoStation)
					{
						%obj = NewObject(%shape,%type,%shape,false);
						//%team = GameBase::setTeam(%obj,0);
						AddToSet("MissionCleanup\\Arena",%obj);
						GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$Arena::Spawn));
						GameBase::setRotation(%obj,%rot);
						GameBase::setTeam(%obj,-1);
						%obj.isArenaPart = true;
						%obj.isUnKillable = true;
						$ArenaPart[%i] = %obj;
					}
					else if(%shape == Generator0)
					{
						%shape = "Generator";
						%obj = NewObject(%shape,%type,%shape,false);
						//%team = GameBase::setTeam(%obj,0);
						AddToSet("MissionCleanup\\Arena",%obj);
						GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$Arena::Spawn));
						GameBase::setRotation(%obj,%rot);
						GameBase::setTeam(%obj,0);
						%obj.isArenaPart = true;
						%obj.isUnKillable = true;
						$ArenaPart[%i] = %obj;
					}
					else if(%shape == Generator1)
					{
						%shape = "Generator";
						%obj = NewObject(%shape,%type,%shape,false);
						//%team = GameBase::setTeam(%obj,0);
						AddToSet("MissionCleanup\\Arena",%obj);
						GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$Arena::Spawn));
						GameBase::setRotation(%obj,%rot);
						GameBase::setTeam(%obj,1);
						%obj.isArenaPart = true;
						%obj.isUnKillable = true;
						$ArenaPart[%i] = %obj;
					}
					else if(%shape == PulseSensor0)
					{
						%shape = "TASensor";
						%type = "Sensor";
						%obj = NewObject(%shape,%type,%shape,false);
						//%team = GameBase::setTeam(%obj,0);
						AddToSet("MissionCleanup\\Arena",%obj);
						GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$Arena::Spawn));
						GameBase::setRotation(%obj,%rot);
						GameBase::setTeam(%obj,0);
						%obj.isArenaPart = true;
						%obj.isUnKillable = true;
						$ArenaPart[%i] = %obj;
					}
					else if(%shape == PulseSensor1)
					{
						%shape = "TASensor";
						%type = "Sensor";
						%obj = NewObject(%shape,%type,%shape,false);
						//%team = GameBase::setTeam(%obj,0);
						AddToSet("MissionCleanup\\Arena",%obj);
						GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$Arena::Spawn));
						GameBase::setRotation(%obj,%rot);
						GameBase::setTeam(%obj,1);
						%obj.isArenaPart = true;
						%obj.isUnKillable = true;
						$ArenaPart[%i] = %obj;
					}
					else if(%shape == TowerSwitch)
					{
						%desc = getWord(%arenaObject, 7); 
						$descx = %desc;
						%x = 8;
						//echo("$descx == "@$descx@" <<<<<<<<<<<<<<<<<<<<< Tower Switch >>>>");
						%word = getWord(%arenaObject,%x);
						if(%word != -1 )
						{
							$descx = $descx@" "@%word;
							%desc = $descx;
						}
						//echo("%word == "@%word@" <<<<<<<<<<<<<<<<<<<<< Tower Switch >>>>");
						//echo("%x == "@%x@" <<<<<<<<<<<<<<<<<<<<< Tower Switch >>>>");
						if(%word != -1 )
						{
							%word = getWord(%arenaObject,%x++);
							//echo("%word == "@%word@" <<<<<<<<<<<<<<<<<<<<< Tower Switch >>>>");
							if(%word != -1 )
							{
								$descx = $descx@" "@%word;
								%desc = $descx;
							}
							//echo("%desc == "@$desc@" <<<<<<<<<<<<<<<<<<<<< Tower Switch >>>>");
							//echo("%x == "@%x@" <<<<<<<<<<<<<<<<<<<<< Tower Switch >>>>");
						}
						if(%word != -1 )
						{
							%word = getWord(%arenaObject,%x++);
							//echo("%word == "@%word@" <<<<<<<<<<<<<<<<<<<<< Tower Switch >>>>");
							if(%word != -1 )
							{
								$descx = $descx@" "@%word;
								%desc = $descx;
							}
							//echo("%desc == "@$desc@" <<<<<<<<<<<<<<<<<<<<< Tower Switch >>>>");
							//echo("%x == "@%x@" <<<<<<<<<<<<<<<<<<<<< Tower Switch >>>>");
						}
						if(%word != -1 )
						{
							%word = getWord(%arenaObject,%x++);
							if(%word != -1 )
							{
								$descx = $descx@" "@%word;
								%desc = $descx;
							}
						}
						if(%word != -1 )
						{
							%word = getWord(%arenaObject,%x++);
							if(%word != -1 )
							{
								$descx = $descx@" "@%word;
								%desc = $descx;
							}
						}
						if(%word != -1 )
						{
							%word = getWord(%arenaObject,%x++);
							if(%word != -1 )
							{
								$descx = $descx@" "@%word;
								%desc = $descx;
							}
						}
						if(%word != -1 )
						{
							%word = getWord(%arenaObject,%x++);
							if(%word != -1 )
							{
								$descx = $descx@" "@%word;
								%desc = $descx;
							}
						}
						if(%word != -1 )
						{
							%word = getWord(%arenaObject,%x++);
							if(%word != -1 )
							{
								$descx = $descx@" "@%word;
								%desc = $descx;
							}
							
						}
						if($debug)
							echo(%desc@" <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< TS <<");
						%obj = NewObject(%desc,%type,%shape,false);
						//%team = GameBase::setTeam(%obj,0);
						AddToSet("MissionCleanup\\Arena",%obj);
						GameBase::setMapName(%obj,%desc);
						GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$Arena::Spawn));
						GameBase::setRotation(%obj,%rot);
						GameBase::setTeam(%obj,0);
						%obj.isArenaPart = true;
						$ArenaPart[%i] = %obj;
						%mn = GameBase::getMapName(%obj);
						echo(%mn@" <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< MN <<");	
					}
					else
					{
						%obj = NewObject(%shape,%type,%shape,false);
						//%team = GameBase::setTeam(%obj,0);
						AddToSet("MissionCleanup\\Arena",%obj);
						GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$Arena::Spawn));
						GameBase::setRotation(%obj,%rot);
						GameBase::setTeam(%obj,0);
						%obj.isArenaPart = true;
						$ArenaPart[%i] = %obj;
					}
				}
				else if(%type == Item)
				{
					//%obj = newObject("RepairPack","Item","repairpack",1,true,true); 
					%obj = NewObject(%shape,%type,%shape,%num,true,true);
					//%team = GameBase::setTeam(%obj,0);
					AddToSet("MissionCleanup\\Arena",%obj);
					GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$Arena::Spawn));
					GameBase::setRotation(%obj,%rot);
					%obj.isArenaPart = true;
					$ArenaPart[%i] = %obj;
				}

				if($debug)
				   echo("!! Piece #"@%i@" added ("@%type@" "@%shape@")");
			}
		}
		Schedule("Arena::MatchStarted();",1+(%i/30));
	}
}

function Arena::MatchStarted()
{
	$Arena::Mapchange = false;
	deleteVariables("Arena::object"); //clear the arena junk
	deleteVariables("descx"); // Clean up!
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) {
		if(%cl.inArena && getObjectType(Client::getControlObject(%cl)) != "Player") {
			if(!%cl.inArenaTD)
			{
			
				Centerprint(%cl,"<jc><f2>Match started.",2);
				Game::playerSpawn(%cl,true);
			}
			else if($ArenaTD::Active)
			{
				ArenaTD::StartMatch(%cl,start); 
			}
		}
	}
}

function GetOffSetRot(%offset,%rot,%pos)
{
	%x = getWord(%offset,0);
	%y = getWord(%offset,1);
	%z = getWord(%offset,2);
	%posA = Vector::add(%pos,Vector::getFromRot(Vector::Add(%rot,"0 0 -1.570796327"),%x));
	%posB = Vector::add(%posA,Vector::getFromRot(%rot,%y));
	%posC = Vector::add(%posB,Vector::getFromRot(Vector::Add(%rot,"1.570796327 0 0"),%z));
	return %posC;
}

$Arena::name["None"] = "None"; 

$Arena::name["archaea"] = "Archaea";
$Arena::canBuild["archaea"] = true;
$Arena::MapList[0] = "archaea";
$Arena::name["arenainthesky"] = "Arena In The Sky";
$Arena::canBuild["arenainthesky"] = true;
$Arena::MapList[1] = "arenainthesky";
$Arena::name["madness"] = "Arena Madness";
$Arena::canBuild["madness"] = true;
$Arena::MapList[2] = "madness";
$Arena::name["bbath"] = "Bloodbath";
$Arena::canBuild["bbath"] = true;
$Arena::MapList[3] = "bbath";
$Arena::name["bcube"] = "Battle Cube";
$Arena::canBuild["bcube"] = true; //InventoryStation
$Arena::MapList[4] = "bcube";
$Arena::name["bfcax"] = "BF-CaX";
$Arena::canBuild["bfcax"] = true;
$Arena::MapList[5] = "bfcax";
$Arena::name["bfchaos"] = "BF-Chaos";
$Arena::canBuild["bfchaos"] = true;
$Arena::MapList[6] = "bfchaos";
$Arena::name["bfcolosseum"] = "BF-Colosseum";
$Arena::canBuild["bfcolosseum"] = true;
$Arena::MapList[7] = "bfcolosseum";
$Arena::name["bfdeadcity"] = "BF-DeadCity";
$Arena::canBuild["bfdeadcity"] = true;
$Arena::MapList[8] = "bfdeadcity";
$Arena::name["bfdiscord"] = "BF-Discord";
$Arena::canBuild["bfdiscord"] = true;
$Arena::MapList[9] = "bfdiscord";
$Arena::name["bffrieddust"] = "BF-FriedDust";
$Arena::canBuild["bffrieddust"] = true;
$Arena::MapList[10] = "bffrieddust";
$Arena::name["bftrappedarena"] = "BF-TrappedArena";
$Arena::canBuild["bftrappedarena"] = true;
$Arena::MapList[11] = "bftrappedarena";
$Arena::name["brokenfixed"] = "Broken Fixed";
$Arena::canBuild["brokenfixed"] = true;
$Arena::MapList[12] = "brokenfixed";
$Arena::name["carlsberg"] = "Carls Berg";
$Arena::canBuild["carlsberg"] = true;
$Arena::MapList[13] = "carlsberg";
$Arena::name["corona"] = "Corona";
$Arena::canBuild["corona"] = true;
$Arena::MapList[14] = "corona";
$Arena::name["damnarena"] = "Damn Arena";
$Arena::canBuild["damnarena"] = true;
$Arena::MapList[15] = "damnarena";
$Arena::name["default"] = "Default Arena";
$Arena::canBuild["default"] = true;
$Arena::MapList[16] = "default";
$Arena::name["hive"] = "The Hive";
$Arena::canBuild["hive"] = true;
$Arena::MapList[17] = "hive";
$Arena::name["ivehadworse"] = "Ive Had Worse";
$Arena::canBuild["ivehadworse"] = true;
$Arena::MapList[18] = "ivehadworse";
$Arena::name["knollarena"] = "Knoll Arena";
$Arena::canBuild["knollarena"] = true;
$Arena::MapList[19] = "knollarena";
$Arena::name["kuth"] = "King Under The Hill";
$Arena::canBuild["kuth"] = true;
$Arena::MapList[20] = "kuth";
$Arena::name["lazarena"] = "Laz Arena";
$Arena::canBuild["lazarena"] = true;
$Arena::MapList[21] = "lazarena";
$Arena::name["morena"] = "Morena";
$Arena::canBuild["morena"] = true;
$Arena::MapList[22] = "morena";
$Arena::name["newyorkerarena"] = "New Yorker Arena";
$Arena::canBuild["newyorkerarena"] = true;
$Arena::MapList[23] = "newyorkerarena";
$Arena::name["oldmilwaukee"] = "Old Milwaukee";
$Arena::canBuild["oldmilwaukee"] = true;
$Arena::MapList[24] = "oldmilwaukee";
$Arena::name["standoff"] = "Standoff";
$Arena::canBuild["standoff"] = true;
$Arena::MapList[25] = "standoff";
$Arena::name["thearena"] = "The Arena";
$Arena::canBuild["thearena"] = true;
$Arena::MapList[26] = "thearena";
$Arena::name["trickortreat"] = "Trick-Or-Treat";
$Arena::canBuild["trickortreat"] = true;
$Arena::MapList[27] = "trickortreat";
$Arena::name["walledin"] = "Walled In";
$Arena::canBuild["walledin"] = true;
$Arena::MapList[28] = "walledin";

$Arena::name["invobcube"] = "Battle Cube Invo";
$Arena::canBuild["invobcube"] = true;
//$Arena::MapList[30] = "invobcube"; 
$Arena::name["bbootcamp3"] = "B00zE BootCamp 3";
$Arena::canBuild["bbootcamp3"] = true;
//$Arena::MapList[31] = "bbootcamp3";
$Arena::name["eeliteski"] = "Extreme Elites (Ski)";
$Arena::canBuild["eeliteski"] = true;
//$Arena::MapList[32] = "eeliteski";
$Arena::name["skiway"] = "Ski Way (Ski)";
$Arena::canBuild["skiway"] = true;
//$Arena::MapList[33] = "skiway";
$Arena::name["arenaterrain"] = "Arena Terrain";
$Arena::canBuild["arenaterrain"] = true;
//$Arena::MapList[34] = "arenaterrain";

$Arena::MapCount = 30; //add one more than real map count

// default arena platform, im almost positive these can be made in editor and exported into code like this... somehow..
// Lol, and years later, I made the script.
//
//
//%n = -1;
//$Arena::name["map"] = "Map";
//$Arena::canBuild["map"] = true;
//$Arena::object["map",%n++] = "iblock.dis 0 0 -5 0 0 0";
//$Arena::object["map",%n++] = "spawn3 0 0 5 0 0 0";
//%n = -1;
//
//%n = -1;
//$Arena::name["tdvalley"] = "TD-Valley";
//$Arena::canBuild["tdvalley"] = true;
//$Arena::object["tdvalley",%n++] = "hlbase.dis 0 0 200 0 0 0";
//$Arena::object["tdvalley",%n++] = "hlterrain.dis 0 0 200 0 0 0";
//$Arena::object["tdvalley",%n++] = "hlbase.dis 0 0 200 0 0 0";
//$Arena::object["tdvalley",%n++] = "spawn3 -29 67 208 0 -0 -2.35613";
//%n = -1;