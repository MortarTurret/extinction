$ArenaTD::One = false;
$ArenaTD::Two = false;
//$ArenaTDT::Spawn[0] = "0 -1000 350"; // hopefully this isnt out of the maps gravity range...
//$ArenaTDT::Spawn[1] = "0 1000 350";
//$ArenaTDT::Spawn[2] = "-1000 0 350";
//$ArenaTDT::Spawn[3] = "1000 0 350";
//$ArenaTDT::curSpawn = "0 -1000 350";
$Server::ArenaTeamDamageScale = 1; //1 = true; 0 = false;
$TeamDuel::MissionArea = 850;

function ArenaTD::Create(%clientId)
{
	if($ArenaTD::One == false)
	{
		//%clientId.TDTeamOneName = true;
		//client::sendmessage(%clientId, 0, Client::getName(%clientId)@" please type in the name of your team.~wcapturedtower.wav");
		$ArenaTD::One = "TD 1";
		ArenaMSG(1, Client::GetName(%clientId)@" created "@$ArenaTD::One@".~wcapturedtower.wav");
		%clientId.inArenaTD = true;
		%clientId.isArenaTDDead = false;
		%clientId.inArenaTDOne = true;
		%clientId.isTDCaptOne = true;
		%clientId.TDMRequestOne = false;
		%clientId.ArenaTDMap = "None";
		%clientId.ArenaTDSpawnType = "None";
		%clientId.ArenaTDWeaponOpt = "None";
		$TDCaptOne = %clientId;
		GameBase::setTeam(%clientId,0);
		Game::refreshClientScore(%clientId);
		return;
	}
	else if($ArenaTD::Two == false)
	{
		//%clientId.TDTeamTwoName = true;
		//client::sendmessage(%clientId, 0, Client::getName(%clientId)@" please type in the name of your team.~wcapturedtower.wav");
		$ArenaTD::Two = "TD 2";
		ArenaMSG(1, Client::GetName(%clientId)@" created "@$ArenaTD::Two@".~wcapturedtower.wav");
		%clientId.inArenaTD = true;
		%clientId.isArenaTDDead = false;
		%clientId.inArenaTDTwo = true;
		%clientId.isTDCaptTwo = true;
		%clientId.TDMRequestTwo = false;
		%clientId.ArenaTDMap = "None";
		%clientId.ArenaTDSpawnType = "None";
		%clientId.ArenaTDWeaponOpt = "None";
		$TDCaptTwo = %clientId;
		GameBase::setTeam(%clientId,1);
		Game::refreshClientScore(%clientId);
		return;
	}
	else
	{
		echo("why am i here in ArenaTD::Create");
		return;
	}
}

// death666
function ArenaDBots::Manage(%clientId)
{
		%client = Player::getClient(%clientId);
		%name = Client::getName(%client);
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Client::buildMenu(%clientId, "Enemy Duel Bots "@%name, "ArenaTDMenu", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Spawn Enemy Angel", "AngelD");
		Client::addMenuItem(%clientId, %curItem++ @ "Spawn Enemy Chameleon", "ChameleonD");
		Client::addMenuItem(%clientId, %curItem++ @ "Spawn Enemy Necromancer", "NecromancerD");
		Client::addMenuItem(%clientId, %curItem++ @ "Spawn Enemy Warrior", "WarriorD");
		Client::addMenuItem(%clientId, %curItem++ @ "Spawn Enemy Builder", "BuilderD");
		Client::addMenuItem(%clientId, %curItem++ @ "Spawn Enemy Troll", "TrollD");
		Client::addMenuItem(%clientId, %curItem++ @ "Spawn Enemy Tank", "TankD");
		Client::addMenuItem(%clientId, %curItem++ @ "Spawn Enemy Titan", "TitanD");
		return;
}
// death666

// death666
function ArenaTBots::Manage(%clientId)
{
		%client = Player::getClient(%clientId);
		%name = Client::getName(%client);
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Client::buildMenu(%clientId, "Team Duel Bots "@%name, "ArenaTDMenu", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Spawn Team Angel", "AngelTD");
		Client::addMenuItem(%clientId, %curItem++ @ "Spawn Team Chameleon", "ChameleonTD");
		Client::addMenuItem(%clientId, %curItem++ @ "Spawn Team Necromancer", "NecromancerTD");
		Client::addMenuItem(%clientId, %curItem++ @ "Spawn Team Warrior", "WarriorTD");
		Client::addMenuItem(%clientId, %curItem++ @ "Spawn Team Builder", "BuilderTD");
		Client::addMenuItem(%clientId, %curItem++ @ "Spawn Team Troll", "TrollTD");
		Client::addMenuItem(%clientId, %curItem++ @ "Spawn Team Tank", "TankTD");
		Client::addMenuItem(%clientId, %curItem++ @ "Spawn Team Titan", "TitanTD");
		return;
}
// death666

function ArenaTD::Manage(%clientId)
{
	%client = Player::getClient(%clientId);
	if(%clientId.isTDCaptOne)
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		%map = %clientId.ArenaTDMap;
		Client::buildMenu(%clientId, "Manage "@$ArenaTD::One@" team", "ArenaTDMenu", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Send request to "@$ArenaTD::Two@"'s team", "rtd");
		Client::addMenuItem(%clientId, %curItem++ @ "Map: "@$Arena::Name[%map], "change");
		Client::addMenuItem(%clientId, %curItem++ @ "Spawn Type: "@%clientId.ArenaTDSpawnType, "changespawn");
		Client::addMenuItem(%clientId, %curItem++ @ "Weapon Options: "@%clientId.ArenaTDWeaponOpt, "weaponopt");
		Client::addMenuItem(%clientId, %curItem++ @ "Invite Player", "itd");
		Client::addMenuItem(%clientId, %curItem++ @ "Kick Player", "ktd");
		Client::addMenuItem(%clientId, %curItem++ @ "Promote Player", "ptd");
		Client::addMenuItem(%clientId, %curItem++ @ "Time Out", "totd");
		return;
	}
	else if(%clientId.isTDCaptTwo)
	{
//		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		%map = %clientId.ArenaTDMap;
		Client::buildMenu(%clientId, "Manage "@$ArenaTD::Two@" team", "ArenaTDMenu", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Send request to "@$ArenaTD::One@"'s team", "rtd");
		Client::addMenuItem(%clientId, %curItem++ @ "Map: "@$Arena::Name[%map], "change");
		Client::addMenuItem(%clientId, %curItem++ @ "Spawn Type: "@%clientId.ArenaTDSpawnType, "changespawn");
		Client::addMenuItem(%clientId, %curItem++ @ "Weapon Options: "@%clientId.ArenaTDWeaponOpt, "weaponopt");
		Client::addMenuItem(%clientId, %curItem++ @ "Invite Player", "itd");
		Client::addMenuItem(%clientId, %curItem++ @ "Kick Player", "ktd");
		Client::addMenuItem(%clientId, %curItem++ @ "Promote Player", "ptd");
		Client::addMenuItem(%clientId, %curItem++ @ "Time Out", "totd");
		return;
	}
}

function ArenaTD::MapNextOne(%clientId)
{
	Client::buildMenu(%clientId, "Choose Arena", "arenaTDPick", true);
	Client::addMenuItem(%clientId, %curItem++ @ "BF-Chaos", "bfchaos");
	Client::addMenuItem(%clientId, %curItem++ @ "BF-DeadCity", "bfdeadcity");
	Client::addMenuItem(%clientId, %curItem++ @ "BF-Discord(bots)", "bfdiscord");
	Client::addMenuItem(%clientId, %curItem++ @ "BF-FriedDust", "bffrieddust");
	Client::addMenuItem(%clientId, %curItem++ @ "BF-TrappedArena(bots)", "bftrappedarena");
	//Client::addMenuItem(%clientId, %curItem++ @ "(Broke) Bloodbath", "bbath");
	Client::addMenuItem(%clientId, %curItem++ @ "Broken Fixed", "brokenfixed");
	Client::addMenuItem(%clientId, %curItem++ @ "Next Page >>>", "arenamapnexttwo");
	Client::addMenuItem(%clientId, %curItem++ @ "<<< Last Page", "arenamap"); //added back support to maps
		
}

function ArenaTD::MapNextTwo(%clientId)
{
	Client::buildMenu(%clientId, "Choose Arena", "arenaTDPick", true);
	Client::addMenuItem(%clientId, %curItem++ @ "Carls Berg(bots)", "carlsberg");
	//Client::addMenuItem(%clientId, %curItem++ @ "(Broke) Default Arena", "default");
	Client::addMenuItem(%clientId, %curItem++ @ "Corona", "corona");
	Client::addMenuItem(%clientId, %curItem++ @ "Damn Arena(bots)", "damnarena");
	Client::addMenuItem(%clientId, %curItem++ @ "Ive Had Worse(bots)", "ivehadworse");
	Client::addMenuItem(%clientId, %curItem++ @ "(Broke) King Under The Hill", "kuth");
	//Client::addMenuItem(%clientId, %curItem++ @ "(Broke) Knoll Arena", "knollarena");
	Client::addMenuItem(%clientId, %curItem++ @ "Laz Arena(bots)", "lazarena");
	Client::addMenuItem(%clientId, %curItem++ @ "Next Page >>>", "arenamapnextthree");
	Client::addMenuItem(%clientId, %curItem++ @ "<<< Last Page", "arenamapnextone");
		
}

function ArenaTD::MapNextThree(%clientId)
{
	Client::buildMenu(%clientId, "Choose Arena", "arenaTDPick", true);
	Client::addMenuItem(%clientId, %curItem++ @ "Morena", "morena");
	Client::addMenuItem(%clientId, %curItem++ @ "New Yorker Arena", "newyorkerarena");
	Client::addMenuItem(%clientId, %curItem++ @ "Old Milwaukee(bots)", "oldmilwaukee");
	//Client::addMenuItem(%clientId, %curItem++ @ "Standoff(bots)", "standoff");	
	Client::addMenuItem(%clientId, %curItem++ @ "The Arena(bots)", "thearena");
	//Client::addMenuItem(%clientId, %curItem++ @ "The Hive(bots)", "hive");
	Client::addMenuItem(%clientId, %curItem++ @ "Trick-Or-Treat", "trickortreat");
	Client::addMenuItem(%clientId, %curItem++ @ "Walled In", "walledin");
	//Client::addMenuItem(%clientId, %curItem++ @ "Next Page >>>", "arenamapnextfour");
	Client::addMenuItem(%clientId, %curItem++ @ "<<< Last Page", "arenamapnexttwo");
	Client::addMenuItem(%clientId, %curItem++ @ ">>> Arena Menu <<<", "back");
		
}

function ArenaTD::MapNextFour(%clientId)
{
	Client::buildMenu(%clientId, "Choose Arena", "arenaTDPick", true);
	Client::addMenuItem(%clientId, %curItem++ @ "Walled In", "walledin");

	if(!$TALT::Stripped)
	{
		Client::addMenuItem(%clientId, %curItem++ @ "Battle Cube Invo", "invobcube");
		Client::addMenuItem(%clientId, %curItem++ @ "B00zE BootCamp 3", "bbootcamp3");
		Client::addMenuItem(%clientId, %curItem++ @ "Extreme Elites (Ski)", "eeliteski");
	}
	//Client::addMenuItem(%clientId, %curItem++ @ "Next Page...", "arenamapnextfive");
	Client::addMenuItem(%clientId, %curItem++ @ "Back", "back");
		
}

function processMenuArenaTDPick(%clientId,%opt)
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
		processMenuArenaTDMenu(%clientId, "change");
	}
	else if(%opt == arenamapnextone)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		ArenaTD::MapNextOne(%clientId);
	}
	else if(%opt == arenamapnexttwo)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		ArenaTD::MapNextTwo(%clientId);
	}
	else if(%opt == arenamapnextthree)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		ArenaTD::MapNextThree(%clientId);
	}
	else if(%opt == arenamapnextfour)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		ArenaTD::MapNextFour(%clientId);
	}
	else if(%opt == arenamapnextfive)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		ArenaTD::MapNextFive(%clientId);
	}

	if($Arena::CanBuild[%opt])
	{
		if($Arena::Initialized)
		{
		   %clientId.ArenaTDMap = %opt;
		}

		//Arena::Init(%opt);

		client::sendmessage(%clientId,0,"Map "@$Arena::Name[%opt]@" has been selected.~wcapturedtower.wav");
		ArenaTD::Manage(%clientId);
		return;
	}
	else
	   return;
}

function ArenaTD::Invite(%clientId, %cl)
{
	if(%clientId.isTDCaptOne)
	{
		if(%cl.TDRequestTwo)
		{
			client::sendmessage(%clientId, 0, "Please wait for his other Team Invite to expire."); 
			return;
		}
		else if(%cl.TDRequestOne)
		{
			client::sendmessage(%clientId, 0, "Please wait 15 second before sending another request."); 
			return;
		}
		%name = Client::getName(%clientId);
		%cl.TDRequestOne = true;
		client::sendmessage(%clientId, 0, "Your request has been sent."); 
		client::sendmessage(%cl, 0, %name@" has sent you a TD request.");
		Centerprint(%cl,"<jc><f1>"@%name@" has sent you a TD request.",5); 
		Game::menuRequest(%cl);
		schedule("ArenaTD::Expired("@%clientId@","@%cl@");", 15);
		return;
	}
	else if(%clientId.isTDCaptTwo)
	{
		if(%cl.TDRequestOne)
		{
			client::sendmessage(%clientId, 0, "Please wait for his other Team Invite to expire."); 
			return;
		}
		else if(%cl.TDRequestTwo)
		{
			client::sendmessage(%clientId, 0, "Please wait 15 second before sending another request."); 
			return;
		}
		%name = Client::getName(%clientId);
		%cl.TDRequestTwo = true;
		client::sendmessage(%clientId, 0, "Your request has been sent."); 
		client::sendmessage(%cl, 0, %name@" has sent you a TD request.");
		Centerprint(%cl,"<jc><f1>"@%name@" has sent you a TD request.",5); 
		Game::menuRequest(%cl);
		schedule("ArenaTD::Expired("@%clientId@","@%cl@");", 15);
		return;
	}
}

function ArenaTD::Expired(%clientId,%cl)
{
	if(%cl.TDRequestTwo)
	{
		%clientId = $TDCaptTwo;
		if(!%cl.inArenaTD)
			ArenaMSG(0,"TD request from "@Client::getName(%clientId)@" to "@Client::getName(%cl)@" not accepted in time."); 
		%cl.TDRequestTwo = false;
		return;
	}
	else if(%cl.TDRequestOne)
	{
		%clientId = $TDCaptOne;
		if(!%cl.inArenaTD)
			ArenaMSG(0,"TD request from "@Client::getName(%clientId)@" to "@Client::getName(%cl)@" not accepted in time."); 
		%cl.TDRequestOne = false;
		return;
	}
	
	if(%cl.TDMRequestTwo)
	{
		if(!%cl.inArenaTD)
			ArenaMSG(0,"Team "@$ArenaTD::One@" did not accept in time."); 
		%cl.TDMRequestTwo = false;
		return;
	}
	else if(%cl.TDMRequestOne)
	{
		if(!%cl.inArenaTD)
			ArenaMSG(0,"Team "@$ArenaTD::Two@" did not accept in time."); 
		%cl.TDMRequestOne = false;
		return;
	}
}

function ArenaTD::Count(%opt)
{
	%count = 0;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.inArenaTD)// && !Player::isDead(%cl))
		{					
			if(%cl.inArenaTDOne && %opt == "TDOnePlayers")
			{	
				%count++;
			}
			else if(%cl.inArenaTDTwo && %opt == "TDTwoPlayers")
			{
				%count++;
			}
			
			if(%cl.isArenaTDDead)
			{					
				if(%cl.inArenaTDOne && %opt == "TDOneDead")
				{	
					%count++;
				}
				else if(%cl.inArenaTDTwo && %opt == "TDTwoDead")
				{
					%count++;
				}
			}
			else
			{					
				if(%cl.inArenaTDOne && %opt == "TDOneLive")
				{	
					%count++;
				}
				else if(%cl.inArenaTDTwo && %opt == "TDTwoLive")
				{
					%count++;
				}
			}
		}
	}
	return %count;
}

function ArenaTD::Leave(%clientId,%mapchange)
{
	if(%mapchange)
	{
		%player = Client::getOwnedObject(%clientId);
		if(isObject(%player))
		{
			playNextAnim(%clientId);
			Player::kill(%clientId);
		}
		return;
	}
	//Replace the Capt!!!
	%TDOnePlayers = ArenaTD::Count("TDOnePlayers");
	%TDTwoPlayers = ArenaTD::Count("TDTwoPlayers");
	if(%clientId.isTDCaptOne && %TDOnePlayers > 1)
	{
		%onlyone = false;
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			if(%cl.inArenaTD)// && !Player::isDead(%cl))
			{
					
				if(%cl.inArenaTDOne && !%cl.isTDCaptOne && !%onlyone)
				{	
					%onlyone = true;
					$ArenaTD::One = "TD 1";
					ArenaMSG(0,Client::getName(%cl)@" is the new leader of "@$ArenaTD::One@".~wcapturedtower.wav");
					%cl.inArenaTD = true;
					%cl.inArenaTDOne = true;
					%cl.isTDCaptOne = true;
					$TDCaptOne = %cl;
					GameBase::setTeam(%cl,0);
					Game::refreshClientScore(%cl);
					//return;
				}
			}
		}
	}
	else if(%clientId.isTDCaptOne && %TDOnePlayers == 1)
	{
		if($ArenaTD::Active)
		{
			for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) 
				if(%cl.inArenaTD && %cl != %clientId) 
					ArenaTD::EndTime(%cl);
		}
		else
		{
			$ArenaTD::One = false;
			$TDCaptOne = "";
		}
	}
	else if(%clientId.isTDCaptTwo && %TDTwoPlayers > 1)
	{
		%onlyone = false;
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			if(%cl.inArenaTD)// && !Player::isDead(%cl))
			{
				if(%cl.inArenaTDTwo && !%cl.isTDCaptTwo && !%onlyone)
				{
					%onlyone = true;
					$ArenaTD::Two = "TD 2";
					ArenaMSG(0,Client::getName(%cl)@" is the new leader of "@$ArenaTD::Two@".~wcapturedtower.wav");
					%cl.inArenaTD = true;
					%cl.inArenaTDTwo = true;
					%cl.isTDCaptTwo = true;
					$TDCaptOne = %cl;
					GameBase::setTeam(%cl,1);
					Game::refreshClientScore(%cl);
					//return;
				}
			}
		}
	}
	else if(%clientId.isTDCaptTwo && %TDTwoPlayers == 1)
	{
		if($ArenaTD::Active)
		{
			for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) 
				if(%cl.inArenaTD && %cl != %clientId) 
					ArenaTD::EndTime(%cl);
		}
		else
		{
			$ArenaTD::Two = false;
			$TDCaptTwo = "";
		}
	}
	
	if(%clientId.inArenaTDOne)
	{
		if(!%clientId.isArenaTDKicked)
			ArenaMSG(0,Client::getName(%clientId)@" has left Team TD 1.~wcapturedtower.wav");
	}
	else if(%clientId.inArenaTDTwo)
	{
		if(!%clientId.isArenaTDKicked)	
			ArenaMSG(0,Client::getName(%clientId)@" has left Team TD 2.~wcapturedtower.wav");
	}
	
	//ArenaTD
		%clientId.inArenaTD = false;
		%clientId.isArenaTDDead = false;
		%clientId.isArenaTDKicked = false;
	
	//Team 1
		%clientId.inArenaTDOne = false;
		%clientId.isTDCaptOne = false;
		%clientId.TDRequestOne = false;
		%clientId.TDMRequestOne = false;
		
	//Team 2
		%clientId.inArenaTDTwo = false;
		%clientId.isTDCaptTwo = false;
		%clientId.TDRequestTwo = false;
		%clientId.TDMRequestTwo = false;
	
	//Capt Prefs
		%clientId.ArenaTDMap = "None";
		%clientId.ArenaTDSpawnType = "None";
		%clientId.ArenaTDWeaponOpt = "None";
	
	//New Team
		GameBase::setTeam(%clientId,0);

	//Now lets respawn them
	%player = Client::getOwnedObject(%clientId);
	if(isObject(%player))
	{
		playNextAnim(%clientId);
		player::kill(%clientId);
	}
		Game::refreshClientScore(%clientId);
		//Game::playerSpawn(%clientId, true);
		return;
}

function ArenaTD::CheckPlayers()
{
	if($ArenaTD::EndMatch) {echo("TD Check Players - - || - - ArenaTD::EndMatch == true"); return;}

	%ArenaTDOneLive = 0;
	%ArenaTDOneDead = 0;
	%ArenaTDTwoLive = 0;
	%ArenaTDTwoDead = 0;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.inArenaTD)// && !Player::isDead(%cl))
		{
			if(%cl.inArenaTDOne)
			{
				if(!%cl.isArenaTDDead)
					%ArenaTDOneLive++;
				else
					%ArenaTDOneDead++;
			}
			else if(%cl.inArenaTDTwo)
			{
				if(!%cl.isArenaTDDead)
					%ArenaTDTwoLive++;
				else
					%ArenaTDTwoDead++;
			}
		}
	}
	
	%testal = ArenaTD::Count("TDOneLive");
	%testad = ArenaTD::Count("TDOneDead");
	%testbl = ArenaTD::Count("TDTwoLive");
	%testbd = ArenaTD::Count("TDTwoDead");
	%testca = ArenaTD::Count("TDOnePlayers");
	%testcb = ArenaTD::Count("TDTwoPlayers");
	if($debug) {
	echo("ArenaTDOneLive = "@%ArenaTDOneLive@" ------------ New: "@%testal@" ------------- <<<");
	echo("ArenaTDTwoLive = "@%ArenaTDTwoLive@" ------------ New: "@%testbl@" ------------- <<<");
	echo("ArenaTDOneDead = "@%ArenaTDOneDead@" ------------ New: "@%testad@" ------------- <<<");
	echo("ArenaTDTwoDead = "@%ArenaTDTwoDead@" ------------ New: "@%testbd@" ------------- <<<");
	echo("New Players One: "@%testca@" ------------ New Players Two: "@%testcb@" ------------- <<<"); }
	
	if(%ArenaTDOneLive == 0 && %ArenaTDTwoLive != 0)
	{
		$ArenaTD::ScoreTwo++;
		$ArenaTD::EndMatch = true;
		$ArenaTD::EndRound = true;
		if($Arena::Terrain)
				$ArenaTD::TerrainPos = TA::pickWaypoint("toob");
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			if(%cl.inArenaTD)// && !Player::isDead(%cl))
			{
				schedule("ArenaTD::EndMatch("@%cl@", \"TDTwo\");", 5);
			}
		}	
	}
	else if(%ArenaTDOneLive != 0 && %ArenaTDTwoLive == 0)
	{
		$ArenaTD::ScoreOne++;
		$ArenaTD::EndMatch = true;
		$ArenaTD::EndRound = true;
		if($Arena::Terrain)
				$ArenaTD::TerrainPos = TA::pickWaypoint("toob");
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			if(%cl.inArenaTD)// && !Player::isDead(%cl))
			{
				schedule("ArenaTD::EndMatch("@%cl@", \"TDOne\");", 5);
			}
		}	
	}
	else if(%ArenaTDOneLive == 0 && %ArenaTDTwoLive == 0 && !$ArenaTD::EndMatch)
	{
		$ArenaTD::EndMatch = true;
		$ArenaTD::EndRound = true;
		if($Arena::Terrain)
				$ArenaTD::TerrainPos = TA::pickWaypoint("toob");
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			if(%cl.inArenaTD)// && !Player::isDead(%cl))
			{
				schedule("ArenaTD::EndMatch("@%cl@", \"Draw\");", 5);
			}
		}	
	}
	else if(%ArenaTDOneLive == 1 && !$ArenaTD::LastLifeOne && %testca > 1)
	{
		$ArenaTD::LastLifeOne = true;
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			if(%cl.inArenaTD && %cl.inArenaTDOne && !%cl.isArenaTDDead)// && !Player::isDead(%cl))
			{
				ArenaMSG(1,"("@Client::getName(%cl)@") is the last player left for "@$ArenaTD::One);
			}
		}	
	}
	else if(%ArenaTDTwoLive == 1 && !$ArenaTD::LastLifeTwo && %testcb > 1)
	{
		$ArenaTD::LastLifeTwo = true;
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			if(%cl.inArenaTD && %cl.inArenaTDTwo && !%cl.isArenaTDDead)// && !Player::isDead(%cl))
			{
				ArenaMSG(1,"("@Client::getName(%cl)@") is the last player left for "@$ArenaTD::Two);
			}
		}	
	}
}

function ArenaTD::EndMatch(%clientId, %opt)
{
	TA::BlackOut(%clientId,3);
	if($ArenaTD::ScoreOne == 10)
	{
		centerprint(%clientId, "<f1><jc>!!! --- !!! Team TD 1 has WON the Game !!! --- !!!\n<jc><f2>!!! Team TD 1 (<f3>"@$ArenaTD::ScoreOne@"<f2>) and Team TD 2 (<f3>"@$ArenaTD::ScoreTwo@"<f2>) !!!");
		schedule("ArenaTD::EndTime("@%clientId@");", 5);
		return;
	}
	else if($ArenaTD::ScoreTwo == 10)
	{
		centerprint(%clientId, "<f1><jc>!!! --- !!! Team TD 2 has WON the Game !!! --- !!!\n<jc><f2>!!! Team TD 1 ( <f3>"@$ArenaTD::ScoreOne@"<f2> ) and Team TD 2 ( <f3>"@$ArenaTD::ScoreTwo@"<f2> ) !!!");
		schedule("ArenaTD::EndTime("@%clientId@");", 5);
		return;
	}
	
	if(%opt == "Draw")
	{
		centerprint(%clientId, "<f1><jc>!!! The Match was a Draw !!!\n<jc><f2>Team TD 1 ( <f3>"@$ArenaTD::ScoreOne@"<f2> ) and Team TD 2 ( <f3>"@$ArenaTD::ScoreTwo@"<f2> )");
		if(!$ArenaTD::TimeOut)
			schedule("ArenaTD::StartMatch("@%clientId@");", 5);
		else
			schedule("ArenaTD::StartMatch("@%clientId@",timeout);", 5);
		return;
	}
	else if(%opt == "TDOne")
	{
		centerprint(%clientId, "<f1><jc>!!! Team TD 1 won vs Team TD 2 !!!\n<jc><f2>Team TD 1 ( <f3>"@$ArenaTD::ScoreOne@"<f2> ) and Team TD 2 ( <f3>"@$ArenaTD::ScoreTwo@"<f2> )");
		if(!$ArenaTD::TimeOut)
			schedule("ArenaTD::StartMatch("@%clientId@");", 5);
		else
			schedule("ArenaTD::StartMatch("@%clientId@",timeout);", 5);
		return;
	}
	else if(%opt == "TDTwo")
	{
		centerprint(%clientId, "<f1><jc>!!! Team TD 2 won vs Team TD 1 !!!\n<jc><f2>Team TD 1 ( <f3>"@$ArenaTD::ScoreOne@"<f2> ) and Team TD 2 ( <f3>"@$ArenaTD::ScoreTwo@"<f2> )");
		if(!$ArenaTD::TimeOut)
			schedule("ArenaTD::StartMatch("@%clientId@");", 5);
		else
			schedule("ArenaTD::StartMatch("@%clientId@",timeout);", 5);
		return;
	}
}

function ArenaTD::StartMatch(%clientId,%opt)
{
	if(%opt != "skip") {
	$ArenaTD::EndMatch = false;
	$ArenaTD::TO::StartRound = true;
	$ArenaTD::LastLifeOne = false;
	$ArenaTD::LastLifeTwo = false;
	//else {
	if(%clientId.isArenaTDDead)
	{
		//%player = Client::getOwnedObject(%clientId);
		//if(%clientId.inArenaTDOne)
		//{
			//Client::setInitialTeam(%clientId,0);
			//%clientId.observerMode = "";
			//GameBase::setTeam(%clientId,0);
			//Game::playerSpawn(%clientId, true);
			//processMenuPickTeam(%clientId, 0);
		//}
		//else if(%clientId.inArenaTDtwo)
		//{
			//Client::setInitialTeam(%clientId,1);
			//%clientId.observerMode = "";
			//GameBase::setTeam(%clientId,1);
			//Game::playerSpawn(%clientId, true);
	//		processMenuPickTeam(%clientId, 1);
	//	}
	//}
	//else if(Player::isDead(%clientId))
	//{
		%clientId.isArenaTDDead = false;
		Game::playerSpawn(%clientId, true);
	}
	else
	{
		%clientId.isArenaTDDead = false;
		%player = Client::getOwnedObject(%clientId);
		if(isObject(%player))
		{
			playNextAnim(%clientId);
			Player::kill(%clientId);
		}
		Game::playerSpawn(%clientId, true);
	} //} //arena terrain 
	
	if(%opt == "timeout")
	{
		Game::refreshClientScore(%clientId);
		freeze(%clientId);
		if(%clientId.isTDCaptOne)
		{
			if($ArenaTD::TimeOutTDOne)
				ArenaMSG(0,"Waiting for "@Client::getName($TDCaptOne)@" to ready up.~wmine_act.wav");
			else if($ArenaTD::TimeOutTDTwo)
				ArenaMSG(0,"Waiting for "@Client::getName($TDCaptTwo)@" to ready up.~wmine_act.wav");
		}
		return;
	}
	
	if(%opt == "start")
	{
		Game::refreshClientScore(%clientId);
		freeze(%clientId);
		if(%clientId.isTDCaptOne)
		{
			ArenaMSG(0,"Waiting for "@Client::getName($TDCaptOne)@" and "@Client::getName($TDCaptTwo)@" to ready up.~wmine_act.wav");
			$ArenaTD::StartTime = true;
		}
		return;
	} }//second one for the skip after timeout
	
	$ArenaTD::TO::StartRound = false;
	%clientId.arenajug = true;
	TA::BlackOut(%clientId,10);
	schedule("bottomprint("@%clientId@", \"<f1><jc>Match starts in 5 seconds.\", 1);", 1);
	schedule("bottomprint("@%clientId@", \"<f1><jc>Match starts in 4 seconds.\", 1);", 2);
	schedule("bottomprint("@%clientId@", \"<f1><jc>Match starts in 3 seconds.\", 1);", 3);
	schedule("bottomprint("@%clientId@", \"<f1><jc>Match starts in 2 seconds.\", 1);", 4);
	schedule("bottomprint("@%clientId@", \"<f1><jc>Match starts in 1 seconds.\", 1);", 5);
	schedule("Client::sendMessage("@%clientId@", 0, \"~wduellaugh.wav\");", 5);
	schedule("Client::sendMessage("@%clientId@", 0, \"~wduelfight.wav\");", 6);
	schedule("bottomprint("@%clientId@", \"<f1><jc>Fight!!\", 5);", 6);
	%endround = "$ArenaTD::EndRound";
	schedule(%endround @" = false;", 5);
	schedule(%clientId @".arenajug = false;", 7);
	Game::refreshClientScore(%clientId);
}

function ArenaTD::EndTime(%clientId)
{
//	$Arena::Bots = true;
//	messageAll(0, "Arena Bots Re-Enabled After Team Deathmatch ended.");
	$ArenaTD::Respawn = false;
	$ArenaTD::Active = false;
	$ArenaTD::ScoreOne = 0;
	$ArenaTD::ScoreTwo = 0;
	$ArenaTD::One = false;
	$ArenaTD::Two = false;
	$ArenaTD::StartMatch = false;
	$ArenaTD::EndMatch = false;
	$ArenaTD::EndRound = false;
	$ArenaTD::LastLifeOne = false;
	$ArenaTD::LastLifeTwo = false;
	$ArenaTD::ReadyUpOne = false;
	$ArenaTD::ReadyUpTwo = false;
	$ArenaTD::StartTime = false;
	$ArenaTD::TimeOutTDOne = false;
	$ArenaTD::TimeOutTDTwo = false;
	$ArenaTD::TimeOut = false;
	$ArenaTD::TO::StartRound = false;
	
	//Team 1
		%clientId.inArenaTD = false;
		%clientId.isArenaTDDead = false;
		%clientId.inArenaTDOne = false;
		%clientId.isTDCaptOne = false;
		%clientId.TDRequestOne = false;
		%clientId.TDMRequestOne = false;
		
	//Team 2
		%clientId.inArenaTDTwo = false;
		%clientId.isTDCaptTwo = false;
		%clientId.TDRequestTwo = false;
		%clientId.TDMRequestTwo = false;
	
	//Capt Prefs
		%clientId.ArenaTDMap = "None";
		%clientId.ArenaTDSpawnType = "None";
		%clientId.ArenaTDWeaponOpt = "None";
	
	//New Team
		GameBase::setTeam(%clientId,0);

	//Now lets respawn them
	%player = Client::getOwnedObject(%clientId);
	if(isObject(%player))
	{
		playNextAnim(%clientId);
		player::kill(%clientId);
	}
		Game::playerSpawn(%clientId, true);
		Game::refreshClientScore(%clientId);
}

function ArenaTD::MatchTime()
{
	$Arena::Bots = false;
	messageAll(0, "Arena Bots Disabled During Team Deathmatch");
	$ArenaTD::Respawn = false;
	$ArenaTD::Active = true;
	$ArenaTD::ScoreOne = 0;
	$ArenaTD::ScoreTwo = 0;
	$ArenaTD::EndMatch = false;
	$ArenaTD::EndRound = false;
	$ArenaTD::LastLifeOne = false;
	$ArenaTD::LastLifeTwo = false;
	$ArenaTD::ReadyUpOne = false;
	$ArenaTD::ReadyUpTwo = false;
	$ArenaTD::TimeOutTDOne = false;
	$ArenaTD::TimeOutTDTwo = false;
	$ArenaTD::TimeOut = false;
	$ArenaTD::TO::StartRound = false;
	
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		%player = Client::getOwnedObject(%cl);
		if(%cl.inArenaTDOne)
		{
			//Client::setInitialTeam(%cl,0);
			%cl.isArenaTDDead = false;
			GameBase::setTeam(%cl,0);
		}
		else if(%cl.inArenaTDtwo)
		{
			//Client::setInitialTeam(%cl,1);
			%cl.isArenaTDDead = false;
			GameBase::setTeam(%cl,1);
		}
		else if(%cl.inArena)
		{
			if(player::status(%cl) == "(Observing)")
			{
				ArenaTD::ArenaJoin(%cl,"Obs");
			}
			else if(player::status(%cl) == "(Dead)")
			{
				ArenaTD::ArenaJoin(%cl,"Dead");
			}
			else
			{
				ArenaTD::ArenaJoin(%cl,"Live");
			}
		}
		//if(%cl.inArenaTD)
			//schedule("ArenaTD::StartMatch("@%cl@");", 7);
			//ArenaTD::StartMatch(%cl);
	}
	//ArenaTD::StartMatch(%clientId)
}

function ArenaTD::BuildMap(%clientId)
{
	if($Arena::Initialized)
	{
		$Arena::Mapchange = true;
		Arena::Clear();
	}
	
	%map = %clientId.ArenaTDMap;
	echo(%map@"  <<<<<<<<<<<< ArenaTD Map");
	Arena::Init(%map);
}

function ArenaTD::ArenaJoin(%clientId,%opt)
{
	if(%opt == "Obs")
	{
		%clientId.inArena = true;
		Game::refreshClientScore(%clientId);
		Messageall(0,Client::getName(%clientId)@" has entered the Arena waiting area.~wshell_click.wav"); //lets get rid of #arena once and forall
		return;
	}
	else if(%opt == "Dead")
	{
		if(Observer::enterObserverMode(%clientId))
		{
			%clientId.inArena = true;
			Game::refreshClientScore(%clientId);
			Messageall(0,Client::getName(%clientId)@" has entered the Arena waiting area.~wshell_click.wav"); //lets get rid of #arena once and forall
			return;
		}
	}
	else if(%opt == "Live")
	{
		if(Observer::enterObserverMode(%clientId))
		{
			%clientId.inArena = true;
			Game::refreshClientScore(%clientId);
			Messageall(0,Client::getName(%clientId)@" has entered the Arena waiting area.~wshell_click.wav"); //lets get rid of #arena once and forall
			return;
		}
	}
}

function processMenuArenaTDMenu(%clientId, %option)
{
	%client = Player::getClient(%clientId);
//	Client::sendMessage(%client, 0, "~wPku_ammo.wav");
	%opt = getWord(%option, 0);
	//%cl = getWord(%option, 1);
	//TDRequest
	if(%opt == "atd")
	{
		if(!%clientId.inArenaTD && %clientId.TDRequestOne != false && !$Server::TourneyMode || !%clientId.inArenaTD && %clientId.TDRequestTwo != false && !$Server::TourneyMode)
		{
			if(%clientId.TDRequestOne)
			{
				//client::sendmessage(%cl, 0, %name@" has accepted your request.");
				
				%clientId.inArenaTDOne = true;
				%clientId.TDMRequestOne = false;
				GameBase::setTeam(%clientId,0);
				%clientId.inArenaTD = true;
				Game::refreshClientScore(%clientId);
				ArenaMSG(3,Client::getName(%clientId)@" has accepted "@$ArenaTD::One@"'s team invite."); //lets get rid of #duel once and forall
				if($ArenaTD::EndRound)
				{
					%clientId.isArenaTDLJ = true;
					%clientId.isArenaTDDead = false;
					Game::playerSpawn(%clientId, true);
					if($ArenaTD::TimeOut)
						freeze(%clientId);
					else
						ArenaTD::LJ(%clientId);
				}
				else
				{
					if($ArenaTD::Active)
						%clientId.isArenaTDDead = true;
					else
						%clientId.isArenaTDDead = false;
				}
				return;
			}
			else if(%clientId.TDRequestTwo)
			{
				//client::sendmessage(%cl, 0, %name@" has accepted your request.");
				if($ArenaTD::Active)
					%clientId.isArenaTDDead = true;
				else
					%clientId.isArenaTDDead = false;
				
				%clientId.inArenaTDTwo = true;
				%clientId.TDMRequestTwo = false;
				GameBase::setTeam(%clientId,1);
				%clientId.inArenaTD = true;
				Game::refreshClientScore(%clientId);
				ArenaMSG(3,Client::getName(%clientId)@" has accepted "@$ArenaTD::Two@"'s team invite."); //lets get rid of #duel once and forall 
				return;
			}
		}
		else
		{
			client::sendmessage(%clientId, 0, "This invite has expired");
			ArenaTD::Expired(%cl,%clientId);
		}
	}
	else if(%opt == "dtd")
	{
		if(!%clientId.inArenaTD)
		{
			if(%clientId.TDRequestOne)
			{
				ArenaMSG(0,Client::getName(%clientId)@" has declined "@$ArenaTD::One@"'s team invite.");
				%clientId.TDRequestOne = false;
				return;
			}
			else if(%clientId.TDRequestTwo)
			{
				ArenaMSG(0,Client::getName(%clientId)@" has declined "@$ArenaTD::Two@"'s team invite.");
				%clientId.TDRequestTwo = false;
				return;
			}
		}
	}
	else if(%opt == "change")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Client::buildMenu(%clientId, "Choose Arena", "arenaTDPick", true);
		Client::addMenuItem(%clientId, %curItem++ @ "Archaea", "archaea");
		Client::addMenuItem(%clientId, %curItem++ @ "Arena In The Sky", "arenainthesky");
		Client::addMenuItem(%clientId, %curItem++ @ "Arena Madness(bots)", "madness"); 
		Client::addMenuItem(%clientId, %curItem++ @ "Arena Terrain", "arenaterrain"); 
		Client::addMenuItem(%clientId, %curItem++ @ "Battle Cube(bots)", "bcube");
		Client::addMenuItem(%clientId, %curItem++ @ "BF-CaX(bots)", "bfcax");
		Client::addMenuItem(%clientId, %curItem++ @ "BF-Colosseum", "bfcolosseum");
		Client::addMenuItem(%clientId, %curItem++ @ "Next Page >>>", "arenamapnextone");
	}
	else if(%opt == "changespawn")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Client::buildMenu(%clientId, "Choose Arena Armor", "ArenaTDSpawnType", true); 
		//if($TAArena::SpawnType != "AnniSpawn")
			Client::addMenuItem(%clientId, %curItem++ @ "Annihilation", "annispawn");
		//if($TAArena::SpawnType != "EliteSpawn")
			Client::addMenuItem(%clientId, %curItem++ @ "EliteRenegades", "elitespawn");
		//if($TAArena::SpawnType != "BaseSpawn")
			Client::addMenuItem(%clientId, %curItem++ @ "Base", "basespawn");
	}
	else if(%opt == "weaponopt")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Client::buildMenu(%clientId, "Choose Arena Weapons", "ArenaTDWeaponOpt", true); 
			Client::addMenuItem(%clientId, %curItem++ @ "Normal", "normal");
			Client::addMenuItem(%clientId, %curItem++ @ "Disc Only", "disconly");
			Client::addMenuItem(%clientId, %curItem++ @ "Rocket Only", "rocketonly");
			Client::addMenuItem(%clientId, %curItem++ @ "Max Ammo", "maxammo");
	}
	else if(%opt == "rtd")
	{
		if(%clientId.inArenaTD)
		{
			if(%clientId.isTDCaptOne)
			{
				%cl = $TDCaptTwo;
				if(%clientId.ArenaTDMap == "None" || %clientId.ArenaTDSpawnType == "None" || %clientId.ArenaTDWeaponOpt == "None" || %clientId.ArenaTDMap == "" || %clientId.ArenaTDSpawnType == "" || %clientId.ArenaTDWeaponOpt == "" || %clientId.ArenaTDMap == false || %clientId.ArenaTDSpawnType == false || %clientId.ArenaTDWeaponOpt == false)  // ArenaTD Options
				{
					client::sendmessage(%clientId, 0, "Please select something for every option."); 
					return;
				}
				else if(%cl.TDMRequestTwo)
				{
					client::sendmessage(%clientId, 0, "Please wait for his other Team Invite to expire."); 
					return;
				}
				else if(%cl.TDMRequestOne)
				{
					client::sendmessage(%clientId, 0, "Please wait 15 second before sending another request."); 
					return;
				}
				%name = Client::getName(%clientId);
				%cl.TDMRequestTwo = true;
				client::sendmessage(%clientId, 0, "Your request has been sent."); 
				client::sendmessage(%cl, 0, %name@" has sent you a Match request.");
				Game::menuRequest(%cl);
				schedule("ArenaTD::Expired("@%clientId@","@%cl@");", 15);
				ArenaMSG(0,Client::getName(%clientId)@" has sent "@$ArenaTD::Two@"'s team a request.");
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
				{
					if(%cl.inArenaTD)
					{
						TA::BlackOut(%cl,10);
						Centerprint(%cl,"<jc><f1>TD 1's Match Details\nMap: "@$Arena::Name[%clientId.ArenaTDMap]@"\nSpawnType: "@%clientId.ArenaTDSpawnType@"\nWeaponOptions: "@%clientId.ArenaTDWeaponOpt,5); //%clientId.ArenaTDSpawnType %clientId.ArenaTDMap
					}
				}
				return;
			}
			else if(%clientId.isTDCaptTwo)
			{
				%cl = $TDCaptOne;
				if(%clientId.ArenaTDMap == "None" || %clientId.ArenaTDSpawnType == "None" || %clientId.ArenaTDWeaponOpt == "None" || %clientId.ArenaTDMap == "" || %clientId.ArenaTDSpawnType == "" || %clientId.ArenaTDWeaponOpt == "" || %clientId.ArenaTDMap == false || %clientId.ArenaTDSpawnType == false || %clientId.ArenaTDWeaponOpt == false)  // ArenaTD Options
				{
					client::sendmessage(%clientId, 0, "Please select something for every option."); 
					return;
				}
				else if(%cl.TDMRequestOne == true)
				{
					client::sendmessage(%clientId, 0, "Please wait for his other Team Invite to expire."); 
					return;
				}
				else if(%cl.TDMRequestTwo)
				{
					client::sendmessage(%clientId, 0, "Please wait 15 second before sending another request."); 
					return;
				}
				%name = Client::getName(%clientId);
				%cl.TDMRequestOne = true;
				client::sendmessage(%clientId, 0, "Your request has been sent."); 
				client::sendmessage(%cl, 0, %name@" has sent you a Match request.");
				Game::menuRequest(%cl);
				schedule("ArenaTD::Expired("@%clientId@","@%cl@");", 15);
				ArenaMSG(0,Client::getName(%clientId)@" has sent "@$ArenaTD::One@"'s team a request.");
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
				{
					if(%cl.inArenaTD)
					{
						TA::BlackOut(%cl,10);
						Centerprint(%cl,"<jc><f1>TD 2's Match Details\nMap: "@$Arena::Name[%clientId.ArenaTDMap]@"\nSpawnType: "@%clientId.ArenaTDSpawnType@"\nWeaponOptions: "@%clientId.ArenaTDWeaponOpt,5); //%clientId.ArenaTDSpawnType %clientId.ArenaTDMap
					}
				}
				return;
			}
		}
	}
	else if(%opt == "atdm")
	{
		//if(%clientId.inArenaTD && !%clientId.TDMRequestOne && !$Server::TourneyMode || %clientId.inArenaTD && %clientId.TDMRequestTwo != false && !$Server::TourneyMode)
		//{
			if(%clientId.TDMRequestOne)
			{
				//client::sendmessage(%cl, 0, %name@" has accepted your request.");
				%clientId.inArenaTDMatch = true;
				%clientId.TDMRequestOne = false;
				ArenaMSG(1,Client::getName(%clientId)@" has accepted "@$ArenaTD::Two@"'s match."); 
				ArenaTD::MatchTime(); //leaving off here
				ArenaTD::SpawnType($TDCaptTwo);
				ArenaTD::WeaponOpt($TDCaptTwo);
				ArenaTD::BuildMap($TDCaptTwo);
				return;
			}
			else if(%clientId.TDMRequestTwo)
			{
				//client::sendmessage(%cl, 0, %name@" has accepted your request.");
				%clientId.inArenaTDMatch = true;
				%clientId.TDMRequestTwo = false;
				ArenaMSG(1,Client::getName(%clientId)@" has accepted "@$ArenaTD::One@"'s match."); 
				ArenaTD::MatchTime(); //leaving off here
				ArenaTD::SpawnType($TDCaptOne); //custom spawn picks for TD
				ArenaTD::WeaponOpt($TDCaptOne);
				ArenaTD::BuildMap($TDCaptOne);
				return;
			}
		//}
		else
		{
			client::sendmessage(%clientId, 0, "This invite has expired");
			ArenaTD::Expired(%cl,%clientId);
		}
	}
	else if(%opt == "dtdm")
	{
		//if(%cl.inArenaTD)
		//{
			if(%clientId.TDMRequestOne)
			{
				//%cl = $TDCaptTwo;
				ArenaMSG(1,Client::getName(%clientId)@" has declined "@$ArenaTD::Two@"'s match.");
				%clientId.TDMRequestOne = false;
				return;
			}
			else if(%clientId.TDMRequestTwo)
			{
				//%cl = $TDCaptOne;
				ArenaMSG(1,Client::getName(%clientId)@" has declined "@$ArenaTD::One@"'s match.");
				%clientId.TDMRequestTwo = false;
				return;
			}
		//}
	}
	else if(%opt == "ktd")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Client::buildMenu(%clientId, "(Kick) Choose Player", "ArenaTDKick", true);
	
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			if(%cl.inArenaTD)// && !Player::isDead(%cl))
			{
				if(%clientId.isTDCaptOne)
				{
					if(%cl.inArenaTDOne && %cl != %clientId)
					{	
						Client::addMenuItem(%clientId, %curItem++ @ Client::GetName(%cl), %cl);
					}
				}
				else if(%clientId.isTDCaptTwo)
				{
					if(%cl.inArenaTDTwo && %cl != %clientId)
					{	
						Client::addMenuItem(%clientId, %curItem++ @ Client::GetName(%cl), %cl);
					}
				}
			}
		}
	}
	else if(%opt == "itd")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Client::buildMenu(%clientId, "(Invite) Choose Player", "ArenaTDInvite", true);
	
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			if(%cl.inArena)// && !Player::isDead(%cl))
			{
				if(!%cl.inArenaTDOne && !%cl.inArenaTDTwo)
				{	
					Client::addMenuItem(%clientId, %curItem++ @ Client::GetName(%cl), %cl);
				}
			}
		}
	}
	else if(%opt == "ptd")
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Client::buildMenu(%clientId, "(Promote) Choose Player", "ArenaTDPromote", true);
	
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			if(%cl.inArenaTD)// && !Player::isDead(%cl))
			{
				if(%clientId.isTDCaptOne)
				{
					if(%cl.inArenaTDOne && %cl != %clientId)
					{	
						Client::addMenuItem(%clientId, %curItem++ @ Client::GetName(%cl), %cl);
					}
				}
				else if(%clientId.isTDCaptTwo)
				{
					if(%cl.inArenaTDTwo && %cl != %clientId)
					{	
						Client::addMenuItem(%clientId, %curItem++ @ Client::GetName(%cl), %cl);
					}
				}
			}
		}
	}

// death666
	else if(%opt == "AngelD")
	{
		CreateSpearSoloAngel(%clientId);
	}
	else if(%opt == "ChameleonD")
	{
		CreateSpearSoloSpy(%clientId);
	}
	else if(%opt == "NecromancerD")
	{
		CreateSpearSoloNecro(%clientId);
	}
	else if(%opt == "WarriorD")
	{
		CreateSpearSoloWarrior(%clientId);
	}
	else if(%opt == "BuilderD")
	{
		CreateSpearSoloBuilder(%clientId);
	}
	else if(%opt == "TrollD")
	{
		CreateSpearSoloTroll(%clientId);
	}
	else if(%opt == "TankD")
	{
		CreateSpearSoloTank(%clientId);
	}
	else if(%opt == "TitanD")
	{
		CreateSpearSoloTitan(%clientId);
	}
// death666

// death666
	else if(%opt == "AngelTD")
	{
		CreateSpearSoloAngelFriendly(%clientId);
	}
	else if(%opt == "ChameleonTD")
	{
		CreateSpearSoloSpyFriendly(%clientId);
	}
	else if(%opt == "NecromancerTD")
	{
		CreateSpearSoloNecroFriendly(%clientId);
	}
	else if(%opt == "WarriorTD")
	{
		CreateSpearSoloWarriorFriendly(%clientId);
	}
	else if(%opt == "BuilderTD")
	{
		CreateSpearSoloBuilderFriendly(%clientId);
	}
	else if(%opt == "TrollTD")
	{
		CreateSpearSoloTrollFriendly(%clientId);
	}
	else if(%opt == "TankTD")
	{
		CreateSpearSoloTankFriendly(%clientId);
	}
	else if(%opt == "TitanTD")
	{
		CreateSpearSoloTitanFriendly(%clientId);
	}
// death666
	else if(%opt == "totd")
	{
		ArenaMSG(0,Client::getName(%clientId)@" used a time out. The match will be suspended next round.~wmine_act.wav");
		if(%clientId.inArenaTDOne)
			$ArenaTD::TimeOutTDOne = true;
		else if(%clientId.inArenaTDTwo)
			$ArenaTD::TimeOutTDTwo = true;
		$ArenaTD::TimeOut = true;
	}
}

function ArenaTD::LJ(%clientId)
{
	if($ArenaTD::EndRound)
	{
		%clientId.arenajug = true;
		schedule("ArenaTD::LJ("@%clientId@");", 0.3);
	}
	else
	{
		%clientId.arenajug = false;
		Client::sendMessage(%clientId, 0, "~wduellaugh.wav");
		Client::sendMessage(%clientId, 0, "~wduelfight.wav");
		bottomprint(%clientId, "<f1><jc>Fight!!", 3);
		return;
	}
	
}

function ArenaTD::ReadyUp(%clientId,%opt)
{
	//if(%opt == true)
	//{
	//	freeze(%clientId);
	//}
	
	if(%opt == "one" && !$ArenaTD::ReadyUpOne)
	{
		ArenaMSG(0,Client::getName(%clientId)@" is READY..~wmine_act.wav");
		$ArenaTD::ReadyUpOne = true;
	}
	else if(%opt == "two" && !$ArenaTD::ReadyUpTwo)
	{
		ArenaMSG(0,Client::getName(%clientId)@" is READY..~wmine_act.wav");
		$ArenaTD::ReadyUpTwo = true;
	}
	
	if($ArenaTD::ReadyUpOne && $ArenaTD::ReadyUpTwo)
	{
		$ArenaTD::ReadyUpOne = false;
		$ArenaTD::ReadyUpTwo = false;
		$ArenaTD::StartTime = false;
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			if(%cl.inArenaTD)// && !Player::isDead(%cl))
			{
				freeze(%cl,true);
				ArenaTD::StartMatch(%cl,skip);
			}
		}
	}
	
}

function ArenaTD::TimeOut(%clientId,%opt)
{
	if(%opt == true)
	{
		freeze(%clientId);
	}
	else
	{
		ArenaMSG(0,Client::getName(%clientId)@" is READY.~wmine_act.wav");
		$ArenaTD::TimeOutTDOne = false;
		$ArenaTD::TimeOutTDTwo = false;
		$ArenaTD::TimeOut = false;
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			if(%cl.inArenaTD)// && !Player::isDead(%cl))
			{
				freeze(%cl,true);
				ArenaTD::StartMatch(%cl,skip);
			}
		}
	}
	
}

function processMenuArenaTDPromote(%clientId,%cl)
{
	if(%cl.inArenaTD)// && !Player::isDead(%cl))
	{			
		if(%cl.inArenaTDOne && !%cl.isTDCaptOne)
		{	
			%onlyone = true;
			$ArenaTD::One = "TD 1";
			ArenaMSG(0,Client::getName(%cl)@" was promoted leader of "@$ArenaTD::One@".~wcapturedtower.wav");
			%cl.inArenaTD = true;
			%cl.inArenaTDOne = true;
			%cl.isTDCaptOne = true;
			$TDCaptOne = %cl;
			//GameBase::setTeam(%cl,0);
			Game::refreshClientScore(%cl);
			//return;
		}
		else if(%cl.inArenaTDTwo && !%cl.isTDCaptTwo)
		{
			%cl = %cl;
			$ArenaTD::Two = "TD Two";
			ArenaMSG(0,Client::getName(%cl)@" was promoted leader of "@$ArenaTD::Two@".~wcapturedtower.wav");
			%cl.inArenaTD = true;
			%cl.inArenaTDTwo = true;
			%cl.isTDCaptTwo = true;
			$TDCaptOne = %cl;
			//GameBase::setTeam(%cl,1);
			Game::refreshClientScore(%cl);
			//return;
		}
	}
	else
	{
		return;
	}
	
	if(%clientId.inArenaTDOne)
	{
		%clientId.ArenaTDMap = "None";
		%clientId.ArenaTDSpawnType = "None";
		%clientId.ArenaTDWeaponOpt = "None";
		%clientId.isTDCaptOne = false;
		Game::refreshClientScore(%clientId);
	}
	else if(%clientId.inArenaTDTwo)
	{
		%clientId.ArenaTDMap = "None";
		%clientId.ArenaTDSpawnType = "None";
		%clientId.ArenaTDWeaponOpt = "None";
		%clientId.isTDCaptTwo = false;
		Game::refreshClientScore(%clientId);
	}
}

function processMenuArenaTDInvite(%clientId,%cl)
{
	ArenaTD::Invite(%clientId, %cl);
}

function processMenuArenaTDKick(%clientId,%cl)
{
	if(%cl.inArenaTDOne)
		ArenaMSG(1,Client::getName(%cl)@" was kicked from Team TD 1 by "@Client::getName(%clientId)@".~wcapturedtower.wav");
	else if(%cl.inArenaTDTwo)
		ArenaMSG(1,Client::getName(%cl)@" was kicked from Team TD 2 by "@Client::getName(%clientId)@".~wcapturedtower.wav");
	
	%cl.isArenaTDKicked = true;
	ArenaTD::Leave(%cl);
	return;
}

function processMenuArenaTDSpawnType(%clientId,%opt)
{
	if(%opt == back)
	{
		Arena::Opts(%clientId);
		return;
	}
	else if(%opt == "annispawn")
	{
		%clientId.ArenaTDSpawnType = "Annihilation";
		client::sendmessage(%clientId,0,"You selected the Annihilation spawn type.~wcapturedtower.wav");
		ArenaTD::Manage(%clientId);
		return;
	}
	else if(%opt == "elitespawn")
	{
		%clientId.ArenaTDSpawnType = "EliteRenegades";
		client::sendmessage(%clientId,0,"You selected the EliteRenegades spawn type.~wcapturedtower.wav");
		ArenaTD::Manage(%clientId);
		return;
	}
	else if(%opt == "basespawn")
	{
		%clientId.ArenaTDSpawnType = "Base";
		client::sendmessage(%clientId,0,"You selected the Base spawn type.~wcapturedtower.wav");
		ArenaTD::Manage(%clientId);
		return;
	}
	else
	   return;
}

function processMenuArenaTDWeaponOpt(%clientId,%opt)
{
	if(%opt == back)
	{
		Arena::Opts(%clientId);
		return;
	}
	else if(%opt == "normal")
	{
		%clientId.ArenaTDWeaponOpt = "Normal";
		client::sendmessage(%clientId,0,"You selected Normal weapon option.~wcapturedtower.wav");
		ArenaTD::Manage(%clientId);
		return;
	}
	else if(%opt == "disconly")
	{
		%clientId.ArenaTDWeaponOpt = "Disc Only";
		client::sendmessage(%clientId,0,"You selected Disc Only weapon option.~wcapturedtower.wav");
		ArenaTD::Manage(%clientId);
		return;
	}
	else if(%opt == "rocketonly")
	{
		%clientId.ArenaTDWeaponOpt = "Rocket Only";
		client::sendmessage(%clientId,0,"You selected Rocket Only weapon option.~wcapturedtower.wav");
		ArenaTD::Manage(%clientId);
		return;
	}
	else if(%opt == "maxammo")
	{
		%clientId.ArenaTDWeaponOpt = "Max Ammo";
		client::sendmessage(%clientId,0,"You selected Max Ammo weapon option.~wcapturedtower.wav");
		ArenaTD::Manage(%clientId);
		return;
	}
	else
	   return;
}

function ArenaTD::SpawnType(%clientId)
{
	%opt = %clientId.ArenaTDSpawnType;
	echo(%opt@"  <<<<<<<<<<<< ArenaTD Spawn Type");
	if(%opt == "Annihilation")
	{
		$TAArena::SpawnType = "AnniSpawn";
		return;
	}
	else if(%opt == "EliteRenegades")
	{
		$TAArena::SpawnType = "EliteSpawn";
		return;
	}
	else if(%opt == "Base")
	{
		$TAArena::SpawnType = "BaseSpawn";
		return;
	}
	else
	   return;
}

function ArenaTD::WeaponOpt(%clientId)
{
	%opt = %clientId.ArenaTDWeaponOpt;
	echo(%opt@"  <<<<<<<<<<<< ArenaTD Weapon Opt");
	if(%opt == "Normal")
	{
		$TAArena::WeaponOpt = "Normal";
		return;
	}
	else if(%opt == "Disc Only")
	{
		$TAArena::WeaponOpt = "DiscOnly";
		return;
	}
	else if(%opt == "Rocket Only")
	{
		$TAArena::WeaponOpt = "RocketOnly";
		return;
	}
	else if(%opt == "Max Ammo")
	{
		$TAArena::WeaponOpt = "MaxAmmo";
		return;
	}
	else
	   return;
}


//This is how I determine the center of the mission area, could be simplified with vector::add but I don't think I knew that when I wrote this :p
function GetTotalLeaderCoords()
{
    //if(%Team1 == "" || %Team2 == "")    {        return echo("False GetTotalLeaderCoords");    }
    if($TDebug && $TeamDuel::Master)
    {
        LogFunction(GetTotalLeaderCoords);
    }

    %pos = gamebase::getposition($TDCaptOne);
    %x = getWord(%pos, 0) ;
    %y = getWord(%pos, 1) ;
    %z = getWord(%pos, 2) ;

    %pos = gamebase::getposition($TDCaptTwo);
    %x = (%x + getWord(%pos, 0)) ;
    %y = (%y + getWord(%pos, 1)) ;
    %z = (%z + getWord(%pos, 2)) ;

    return (%x/2)@" "@(%y/2)@" "@(%z/2) ;
}
//These get defined at the start of each round, keep it in line with a timestamp since it loops
   // $TeamDuel::Start[1] = floor(getSimTime() + 0.5);
   // $TeamDuel::Start[2] = $TeamDuel::Start[%Team1];
   // $TeamDuel::Center[1] = GetTotalLeaderCoords();
   // $TeamDuel::Center[2] = $TeamDuel::Center[%Team1];
   // CreateMissionArea($TeamDuel::Start[1]);
//should probably be called "checkmissionarea" but it runs every 3 seconds to make sure players don't wander too far
function CreateMissionArea(%time)
{
    if($TDebug && $TeamDuel::Master)
    {
        LogFunction(CreateMissionArea);
    }
    if($TeamDuel::Start[1] == %time && $ArenaTD::Active)
    {
        for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
        {
            if(%cl.inArenaTD && !%cl.isArenaTDDead)
            {
                %cpos = GameBase::getPosition(%cl);
                %distance = floor(Vector::getDistance(getword(%cpos, 0)@" "@getword(%cpos, 1)@" 0", getword($TeamDuel::Center[1], 0)@" "@getword($TeamDuel::Center[1], 1)@" 0") + 0.5);
                //%distance2 = floor(Vector::getDistance(GameBase::getPosition(%cl), gamebase::getposition($TeamDuel::Sensor[%cl.Team])) + 0.5);
                //messageall(1, Client::getname(%cl)@" distance from center: "@%distance@" distance from sensor: "@%distance2);
                if(%distance < $TeamDuel::MissionArea)
                {
                    if(%cl.oob)
                    {
                        Client::sendMessage(%cl,1,"You have entered the mission area.");
                    }
                    %cl.oob = false;
                }
                if(%distance > $TeamDuel::MissionArea)
                {
                    if(%cl.oob == "false")
                    {
                        %cl.oob = true;
                        Client::sendMessage(%cl,1,"You have left the mission area.");
                        alertPlayerTD(%cl, 10);
                        %posX = getWord($TeamDuel::Center[1],0);
                        %posY = getWord($TeamDuel::Center[1],1);
                        //IssueCommand(%cl, %cl, 0, "Waypoint set to mission center", %posX, %posY);
                    }
                }
            }
        }
    }
    else
    {
        //messageall(1, "Start time does not match. stopping mission area");
        return;
    }
    schedule("CreateMissionArea("@%time@");", 1); //5
}


//the other functions you probably won't need
function alertPlayerTD(%client, %count)
{
    if($TDebug && $TeamDuel::Master)
    {
        LogFunction(alertPlayer);
    }
    if(!%client.isArenaTDDead && $ArenaTD::Active) //might want to make room for non after round later.
    {
        if(%client.inArenaTD)
        {
            %distance = floor(Vector::getDistance(GameBase::getPosition(%client), $TeamDuel::Center[1]) + 0.5);
            if(%distance < $TeamDuel::MissionArea)
            {
                if(%client.oob)
                {
                    Client::sendMessage(%client,1,"You have entered the mission area.");
                }
                %client.oob = false;
                return;
            }
        }
        if(%client.oob)
        {
            Client::sendMessage(%client,1,"~wLeftMissionArea.wav");
            if(%count > 1)
            schedule("alertPlayerTD(" @ %client @ ", " @ %count - 1 @ ");",1.5,%client);
            else
            schedule("leaveMissionAreaDamageTD(" @ %client @ ");",1,%client);
        }
    }
}


function leaveMissionAreaDamageTD(%client)
{
    if($TDebug && $TeamDuel::Master)
    {
        LogFunction(leaveMissionAreaDamage);
    }
    if(!%client.isArenaTDDead && $ArenaTD::Active)
    {
        if(%client.inArenaTD)
        {
            %distance = floor(Vector::getDistance(GameBase::getPosition(%client), $TeamDuel::Center[%client.Team]) + 0.5);
            if(%distance < $TeamDuel::MissionArea)
            {
                %client.oob = false;
                Client::sendMessage(%client,1,"You have entered the mission area.");
                return;
            }
        }

        %player = Client::getOwnedObject(%client);
        if(%client.oob)
        {
            if(!Player::isDead(%player) && !%client.isArenaTDDead)
            {
                Player::setDamageFlash(%client,0.6);
                if((GameBase::getDamageLevel(%player) + 0.05) >= (Player::getArmor(%player)).maxdamage)
                {
                    Client::sendMessage(%client,1,"You have been killed for leaving the mission area.~wLeftMissionArea.wav");
                    MessageAllExcept(%client, 1, Client::getName(%client) @ " has been killed for leaving the mission area.");
                    playNextAnim(%client);
                    Player::kill(%client);
                    Client::onKilled(%client, %client, -2);
                }
                else
                {
                    GameBase::setDamageLevel(%player,GameBase::getDamageLevel(%player) + 0.05);
                    schedule("leaveMissionAreaDamageTD(" @ %client @ ");",1);
                }
            }
        }
    }
}
