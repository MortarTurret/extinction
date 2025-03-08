exec("game.cs");
exec(TriggerFunctions);	

$flagReturnTime = 45;
$siegeFlag = "";
function ObjectiveMission::missionComplete()
{
	$missionComplete = true;
	$OT = false;
	%group = nameToID("MissionCleanup/ObjectivesSet");
	for(%i = 0; (%obj = Group::getObject(%group, %i)) != -1; %i++)
	{
		ObjectiveMission::objectiveChanged(%obj);
	}
	if($TA::Stats == false) //New Toggle Stats code
	{
		for(%i = 0; %i < getNumTeams()-1; %i++)
		{
			Team::setObjective(%i, $firstObjectiveLine-4, " ");
			Team::setObjective(%i, $firstObjectiveLine-3, "<f5>Mission Summary:");
			Team::setObjective(%i, $firstObjectiveLine-2, " ");
		}
	}
	ObjectiveMission::setObjectiveHeading();
	ObjectiveMission::refreshTeamScores();
	%lineNum = "";
	$missionComplete = false;

	// back out of all the functions...
	if($TA::RandomMission)
		schedule("Server::nextMission(false,true);", 0);
	else
		schedule("Server::nextMission();", 0);
}

function objective::displayBitmap(%team, %line)
{
	if($TestMissionType == "CTF")
	{
		%bitmap1 = "capturetheflag1.bmp";
		%bitmap2 = "capturetheflag2.bmp";
	}
	else if($TestMissionType == "C&H")
	{
		%bitmap1 = "captureandhold1.bmp";
		%bitmap2 = "captureandhold2.bmp";
	}
	else if($TestMissionType == "D&D")
	{
		%bitmap1 = "defendanddest1.bmp";
		%bitmap2 = "defendanddest2.bmp";
	}		
	else if($TestMissionType == "F&R")
	{
		%bitmap1 = "findandret1.bmp";
		%bitmap2 = "findandret2.bmp";
	}
	if($TA::Stats == false) //New Toggle Stats code
	{
		if(%bitmap1 == "" || %bitmap2 == "")
	 		Team::setObjective(%team, %line, " ");
		else
	 		Team::setObjective(%team, %line, "<jc><B0,0:" @ %bitmap1 @ "><B0,0:" @ %bitmap2 @ ">");
	}
}

function Game::checkTimeLimit()
{
	// if no timeLimit set or timeLimit set to 0,
	// just reschedule the check for a minute hence
	$timeLimitReached = false;
	//ObjectiveMission::setObjectiveHeading(); //disabling this for now
	if ($Spoonbot::AutoSpawn)
	{
		BotFuncs::ScanObjectTree();
	}
	if(!$Server::timeLimit)
	{
		schedule("Game::checkTimeLimit();", 60);
		return;
	}
	%curTimeLeft = ($Server::timeLimit * 60) + $missionStartTime - getSimTime();
		if ( floor(%curTimeLeft) == 600 )
		{
			messageAll(2, "Match Time: 10 minutes remaining.");
		}
		if ( floor(%curTimeLeft) == 1200 )
		{
			messageAll(2, "Match Time: 20 minutes remaining.");
		}
		if ( floor(%curTimeLeft) == 300 )
		{
			messageAll(2, "Match Time: 5 minutes remaining.");			
		}

	if(%curTimeLeft <= 0 && $matchStarted && $Server::TourneyMode && $TA::TourneyOT && $teamScore[0] == $teamScore[1]) //fix maybe
	{
		//$Server::timeLimit = 0;
		if(!$OT)
			messageAll(1, "The match has gone into overtime mode.~wCapturedTower.wav");
		$OT = true;
		schedule("Game::checkTimeLimit();", 3);
		return;
	}
	else if(%curTimeLeft <= 0 && $matchStarted && $ArenaTD::Active) //support for arena td
	{
		if(!$OT)
			messageAll(1, "TD in progress. The match has gone into overtime mode.~wCapturedTower.wav");
		$OT = true;
		schedule("Game::checkTimeLimit();", 3);
		return;
	}
	else if(%curTimeLeft <= 0 && $matchStarted)
	{
		Anni::Echo("GAME: timelimit");
		$timeLimitReached = true;
		//Anni::Echo("checking for objective time limit status...");
		%set = nameToID("MissionCleanup/ObjectiveSet");
		for(%i = 0; (%obj = Group::getObject(%set, %i)) != -1; %i++)
			GameBase::virtual(%obj, "timeLimitReached", %clientId);
		ObjectiveMission::missionComplete();
	}
	else
	{

		if(%curTimeLeft >= 20)
			schedule("Game::checkTimeLimit();", 20);
		else
			schedule("Game::checkTimeLimit();", %curTimeLeft + 1);
		UpdateClientTimes(%curTimeLeft);
	}
}

function Vote::changeMission()
{
	$missionComplete = true;
	ObjectiveMission::refreshTeamScores();
	%group = nameToID("MissionCleanup/ObjectivesSet");
	%lineNum = "";
	for(%i = 0; (%obj = Group::getObject(%group, %i)) != -1; %i++)
	{
		ObjectiveMission::objectiveChanged(%obj);
	}
	if($TA::Stats == false) //New Toggle Stats code
	{
		for(%i = 0; %i < getNumTeams()-1; %i++)
		{ 
			Team::setObjective(%i, $firstObjectiveLine-2, " ");
			Team::setObjective(%i, $firstObjectiveLine-1, "<f5>Mission Summary:");
		}
	}
	ObjectiveMission::setObjectiveHeading();
	$missionComplete = false;
}

function ObjectiveMission::checkScoreLimit()
{
	%done = false;
	ObjectiveMission::refreshTeamScores();
	for(%i = 0; %i < getNumTeams()-1; %i++)
		if($teamScore[%i] >= $teamScoreLimit)
			%done = true;
	if(%done && !$NoFlagCaps && !$ArenaTD::Active) //no capping out if arena td active
		ObjectiveMission::missionComplete();
}

function ObjectiveMission::checkPoints()
{
	for(%i = 0; %i < getNumTeams()-1; %i++)
		$teamScore[%i] += $deltaTeamScore[%i] / 12;
	schedule("ObjectiveMission::checkPoints();", 5);
	ObjectiveMission::checkScoreLimit();
}

function ObjectiveMission::initCheck(%object)
{
	if($TestMissionType == "")
	{
		%name = gamebase::getdataname(%object);
		if(%name == Flag)
		{
			if(gamebase::getteam(%object) != -1)
				$TestMissionType = "CTF";
			else
				$TestMissionType = "F&R";
		}
		else if(%object.objectiveName != "" && %object.scoreValue)
			$TestMissionType = "D&D";
		else if(%name == TowerSwitch)
			$NumTowerSwitchs++;
	}
	%object.trainingObjectiveComplete = "";
	%object.objectiveLine = "";
	if(GameBase::virtual(%object, objectiveInit))
		addToSet("MissionCleanup/ObjectivesSet", %object);
}

function Game::refreshClientScore(%clientId) //reworked to sort right
{
	%team = Client::getTeam(%clientId);
	if(%clientId.isTeamCaptin)
		%teamcaptin = "* ";
	else
		%teamcaptin = "  ";
	
	if(%clientId.isTDCaptOne)
		%tdcapt = "+";
	else if(%clientId.isTDCaptTwo)
		%tdcapt = "+";
	else
		%tdcapt = "";
	
	if(!$ArenaTD::Active)
		%tddead = "  ";
	else if(!%clientId.isArenaTDDead && %clientId.inArenaTD)
		%tddead = "* ";
	else
		%tddead = "  ";
	
	if(%team == -1) // observers go last.
		%team = 9;
		
	if(%clientId.inDuel)
	{
		%arz = 500;
	   %tName = "Dueling";
	}
	else if(%clientId.inArena)
	{
		if(%clientId.inArenaTDOne)
		{
			%arz = 10000;
			%tName = "TD 1"@%tdcapt;
		}
		else if(%clientId.inArenaTDTwo)
		{
			%arz = 5000;
			%tName = "TD 2"@%tdcapt;
		}
		else
		{
			%arz = 1000;
			%tName = "Arena";
		}
	}
	else
	{
		%arz = 20000;
	   %tName = $Server::TeamName[GameBase::getTeam(%clientId)];
	}


	%tmp = %clientId.score;
   	%inte = floor(%clientId.score);
   	%deci = floor(10*(%tmp - %inte));
	%points = %inte @ "." @ %deci;
	%score = floor(%clientId.score);
	
	if($Server::TourneyMode)
	{
		if($TALT::Active == true)
			Client::setScore(%clientId, %teamcaptin @ "%n\t"@%tName@"\t " @ %points  @ "\t"@%clientId.MidAirs@"\t%p\t%l", %clientId.score + (9 - %team) * 10000 * %arz);
		else
			Client::setScore(%clientId, %teamcaptin @ "%n\t"@%tName@"\t  " @ %score  @ "\t%p\t%l\t" @%clientId.Tkills @"\t"@%clientId.Tdeaths, %clientId.score + (9 - %team) * 10000 * %arz);
	}
	else
	{
		if(!%clientId.inArena)
		{
			if($TALT::Active == true)
				Client::setScore(%clientId, "%n\t"@%tName@"\t " @ %points  @ "\t"@%clientId.MidAirs@"\t%p\t%l", %clientId.score + (9 - %team) * 10000 * %arz);
			else
				Client::setScore(%clientId, "%n\t"@%tName@"\t  " @ %score  @ "\t%p\t%l\t" @%clientId.Tkills @"\t"@%clientId.Tdeaths, %clientId.score + (9 - %team) * 10000 * %arz);
		}
		else
		{
			if($TALT::Active)
				Client::setScore(%clientId, %tddead@"%n\t"@%tName@"\t " @ %points  @ "\t"@%clientId.MidAirs@"\t%p\t%l", %clientId.score + (9 - %team) * 10000 * %arz);
			else
				Client::setScore(%clientId, %tddead@"%n\t"@%tName@"\t " @ %points  @ "\t%p\t%l\t" @%clientId.Tkills @"\t"@%clientId.Tdeaths, %clientId.score + (9 - %team) * 10000 * %arz);
		}
	}
	
//	Client::setScore(%clientId, "%n\t%t\t " @ %clientId.score, %clientId.score);
}

function BackupGame::refreshClientScore(%clientId)
{
	%team = Client::getTeam(%clientId);
	if(%clientId.isTeamCaptin)
		%teamcaptin = "* ";
	else
		%teamcaptin = "  ";
	//if(%team == -1) // observers go last.
	//	%team = 9;
		
	if(%clientId.inArena)
	{
		%arz = 20000;
	   %tName = "Arena";
	}
	else if(%clientId.inDuel)
	{
		%arz = 10000;
	   %tName = "Dueling";
	}
	else
	   %tName = $Server::TeamName[GameBase::getTeam(%clientId)];


	%tmp = %clientId.score;
   	%inte = floor(%clientId.score);
   	%deci = floor(10*(%tmp - %inte));
	%points = %inte @ "." @ %deci;
	
	if($Server::TourneyMode)
	{
		if($TALT::Active == true)
			Client::setScore(%clientId, %teamcaptin @ "%n\t"@%tName@"\t " @ %points  @ "\t"@%clientId.MidAirs@"\t%p\t%l", %clientId.score + (9 - %team) * 10000 + (%arz));
		else
			Client::setScore(%clientId, %teamcaptin @ "%n\t"@%tName@"\t  " @ %clientId.score  @ "\t%p\t%l\t" @%clientId.Tkills @"\t"@%clientId.Tdeaths, %clientId.score + (9 - %team) * 10000 + (%arz));
	}
	else
	{
		if(%clientId.inArena)
		{
			if($TALT::Active)
				Client::setScore(%clientId, "%n\t"@%tName@"\t " @ %points  @ "\t"@%clientId.MidAirs@"\t%p\t%l", %clientId.score + (9 - %team) * 10000 + (%arz));
			else
				Client::setScore(%clientId, "%n\t"@%tName@"\t " @ %points  @ "\t%p\t%l\t" @%clientId.Tkills @"\t"@%clientId.Tdeaths, %clientId.score + (9 - %team) * 10000 + (%arz));
			//	Client::setScore(%clientId, "%n\t"@%tName@"\t" @ %clientId.score  @ "\t%p\t%l\t" @%clientId.Tkills @"\t"@%clientId.Tdeaths, %clientId.score + (9 - %team) * 10000 + (%arz));
			//Client::setScore(%clientId, %admin @ " %n\t"@%tName@"\t  " @ %clientId.scoreArena  @ "\t%p\t%l\t" @ %clientId.TKills @ "/" @ %clientId.TDeaths @"\t"@%admin, %clientId.scoreArena + (9 - %team) * 10000 + (%arz));
			//return;
		}
		else if(%team == 9)
		{
			if($TALT::Active == true)
				Client::setScore(%clientId, "%n\t"@%tName@"\t " @ %points  @ "\t"@%clientId.MidAirs@"\t%p\t%l", %clientId.score + (9 - %team) * 10000 + (%arz));
			else
				Client::setScore(%clientId, "%n\t"@%tName@"\t  " @ %clientId.score  @ "\t%p\t%l\t" @%clientId.Tkills @"\t"@%clientId.Tdeaths, %clientId.score + (9 - %team) * 10000 + (%arz));
			//return;
		}
		else
		{
			if($TALT::Active == true)
				Client::setScore(%clientId, "%n\t"@%tName@"\t " @ %points  @ "\t"@%clientId.MidAirs@"\t%p\t%l", %clientId.score + (9 - %team) * 10000 + (%arz));
			else
				Client::setScore(%clientId, "%n\t"@%tName@"\t  " @ %clientId.score  @ "\t%p\t%l\t" @%clientId.Tkills @"\t"@%clientId.Tdeaths, %clientId.score + (9 - %team) * 10000 + (%arz));
		}
		//Client::setScore(%clientId, "%n\t"@%tName@"\t  " @ %clientId.score  @ "\t%p\t%l\t" @%clientId.Tkills @"\t"@%clientId.Tdeaths, %clientId.score + (9 - %team) * 10000 + (%arz)); //original 
	}
	
//	Client::setScore(%clientId, "%n\t%t\t " @ %clientId.score, %clientId.score);
}

function ObjectiveMission::refreshTeamScores()
{
	%nt = getNumTeams()-1;
	Team::setScore(-1, "%t\t  0", 0);
	for(%i = -1; %i < %nt; %i++)
	{
		%numPlayers = ObjectiveMission::getNumPlayers(%i);
		if(%i == -1)
			Team::setScore(%i, "%t\t  " @ $teamScore[%i], $teamScore[%i]);
		else
			Team::setScore(%i, "%t\t  " @ $teamScore[%i] @ "\t   " @ %numPlayers, $teamScore[%i]);
		if($TA::Stats == false) //New Toggle Stats code 
		{
			for(%j = 0; %j < %nt; %j++) 
				Team::setObjective(%i,%j+$firstTeamLine, "<f1>	- Team " @ getTeamName(%j) @ " score = " @ $teamScore[%j]);
		}
	}
}

function ObjectiveMission::getNumPlayers(%team)
{
	%numPlayers = 0;
	%totalPlayers = getNumClients();
	for(%i = 0; %i < %totalPlayers; %i++)
	{
		%client = getClientByIndex(%i);
		if(!%client.inArena && !%client.inDuel && %team == Client::getTeam(%client))
			%numPlayers++;
	}
	return %numPlayers;
}

function ObjectiveMission::objectiveChanged(%this)
{
	if($TA::Stats == false) //New Toggle Stats code
	{
		if(%this.objectiveLine)
			for(%i = -1; %i < getNumTeams()-1; %i++)
				Team::setObjective(%i,%this.objectiveLine, "<f1> " @ GameBase::virtual(%this, getObjectiveString, %i));
	}
}



function Mission::init()
{

	if($TALT::Active == true)
		setClientScoreHeading("Player Name\t\x6FTeam\t\x9AScore\t\xC3MA\t\xDFPing\t\xFFPL");
	else
		setClientScoreHeading("Player Name\t\x6ATeam\t\xA6Score\t\xCFPing\t\xEFPL\t\xFFKills");
//	setClientScoreHeading("Player Name\t\x55Kills\t\x75Deaths\t\xA5Efficiency\t\xE3Ping\t\xFFPL");	// ann dm

	//setClientScoreHeading("A Player Name\t\x75Team\t\xA0Score\t\xC8Ping\t\xE8PL\t\xFFPit / Team");//ren
//	setClientScoreHeading("Player Name\t\x6FTeam\t\xD6Score");//\t\xFFPing\t\xFFPL");
//	setTeamScoreHeading("Team Name\t\xD6Score");
	setTeamScoreHeading("Team Name\t\xA6Score\t\xD0Players");

	$firstTeamLine = 7;
	$firstObjectiveLine = $firstTeamLine + getNumTeams()-1 + 1;
	
	//initialize jail positions
	$JailPosition[0] = "0 0 0";
	$JailPosition[1] = "0 0 0";
	$JailPosition[2] = "0 0 0";
	$JailPosition[3] = "0 0 0";
	$JailPosition[4] = "0 0 0";
	$JailPosition[5] = "0 0 0";
	$JailPosition[6] = "0 0 0";
	$JailPosition[7] = "0 0 0";
	
	$FlagDistance = 0;
	for(%i = -1; %i < getNumTeams(); %i++)	
	{
		$teamFlagStand[%i] = "";
		$teamFlag[%i] = "";
		if($TA::Stats == false) //New Toggle Stats code
		{
			Team::setObjective(%i, $firstTeamLine - 1, " ");
			Team::setObjective(%i, $firstObjectiveLine - 1, " ");
			Team::setObjective(%i, $firstObjectiveLine, "<f5>Mission Objectives: ");
		}
		$firstObjectiveLine++;
		$deltaTeamScore[%i] = 0;
		$teamScore[%i] = 0;
		newObject("TeamDrops" @ %i, SimSet);
		addToSet(MissionCleanup, "TeamDrops" @ %i);
		%dropSet = nameToID("MissionGroup/Teams/Team" @ %i @ "/DropPoints/Random");
		for(%j = 0; (%dropPoint = Group::getObject(%dropSet, %j)) != -1; %j++)
			addToSet("MissionCleanup/TeamDrops" @ %i, %dropPoint);
	}
	$numObjectives = 0;
	newObject(ObjectivesSet, SimSet);
	addToSet(MissionCleanup, ObjectivesSet);
	
	Group::iterateRecursive(MissionGroup, ObjectiveMission::initCheck);
	%group = nameToID("MissionCleanup/ObjectivesSet");

	ObjectiveMission::setObjectiveHeading();
	for(%i = 0; (%obj = Group::getObject(%group, %i)) != -1; %i++)
	{
		%obj.objectiveLine = %i + $firstObjectiveLine;
		ObjectiveMission::objectiveChanged(%obj);
	}
	ObjectiveMission::refreshTeamScores();
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		%cl.score = 0;
		Game::refreshClientScore(%cl);
	}
	schedule("ObjectiveMission::checkPoints();", 5);
	if($TestMissionType == "") 
	{
		if($NumTowerSwitchs) 
			$TestMissionType = "C&H";
		else 
			$TestMissionType = "NONE";
		$NumTowerSwitchs = "";
	}
	Flag::findDistance();

   $Spoonbot::HuntFlagrunner = 0;
   $Spoonbot::NumBots = 0;
							//In case some idiot forgot some variables in SPOONBOT.CS, set them here.
   if ($Spoonbot::RespawnDelay == 0)
	{
	$Spoonbot::RespawnDelay = 30;
	}

   if ($Spoonbot::IQ == 0)
	{
	$Spoonbot::IQ = 90;
	}

   if ($Spoonbot::ThinkingInterval == 0)
	{
	$Spoonbot::ThinkingInterval = 3;
	}

	AI::setupAI();
}

function Game::pickRandomSpawn(%team)
{
	%spawnSet = nameToID("MissionCleanup/TeamDrops" @ %team);
	%spawnCount = Group::objectCount(%spawnSet);
	if(!%spawnCount)
		return -1;
	%spawnIdx = floor(getRandom() * (%spawnCount - 0.1));
	%value = %spawnCount;
	for(%i = %spawnIdx; %i < %value; %i++)
	{
		%set = newObject("set",SimSet);
		%obj = Group::getObject(%spawnSet, %i);
		if(containerBoxFillSet(%set,$SimPlayerObjectType|$VehicleObjectType,GameBase::getPosition(%obj),2,2,4,0) == 0)
		{
			deleteObject(%set);
			return %obj;
		}
		if(%i == %spawnCount - 1)
		{
			%i = -1;
			%value = %spawnIdx;
		}
		deleteObject(%set);
	}
	return false;
}

//handles all scoring based on distance to Towers
function Client::leaveGame(%clientId)
{
	Anni::Echo("GAME: clientdrop " @ %clientId@", "@timestamppatch());
	%set = nameToID("MissionCleanup/ObjectivesSet");
	for(%i = 0; (%obj = Group::getObject(%set, %i)) != -1; %i++)
		GameBase::virtual(%obj, "clientDropped", %clientId);
}

function Game::clientKilled(%playerId, %killerId)
{
	%set = nameToID("MissionCleanup/ObjectivesSet");
	for(%i = 0; (%obj = Group::getObject(%set, %i)) != -1; %i++)
		GameBase::virtual(%obj, "clientKilled", %playerId, %killerId);
}

function Player::enterMissionArea(%this)
{
	%client = Player::getClient(%this);
	if(%client.inDuel)
	{
		Client::sendMessage(Player::getClient(%this),1,"You may not enter the mission area.");
		%this.outArea=1;
		alertPlayer(%this, 4);
		if($TALT::Active)
			bouncePlayerBack(%client);
		return;
	}
	if(%client.inArena) //fix for 5.0 arena map terrain
	{
		Client::sendMessage(Player::getClient(%this),1,"You may not enter the mission area.");
		%this.outArea=1;
		alertPlayer(%this, 4);
		if($TALT::Active)
			bouncePlayerBack(%client);
		return;
	}
	//if(!Player::getClient(%this).inArena) // New Arena/Duel code
	//{
		%set = nameToID("MissionCleanup/ObjectivesSet");
		%this.outArea = "";
		if(%client.isshifting != true)
		{
		Client::sendMessage(Player::getClient(%this),1,"You have entered the mission area.");
		}
		for(%i = 0; (%obj = Group::getObject(%set, %i)) != -1; %i++)
			GameBase::virtual(%obj, "playerEnterMissionArea", %this);
	//}
}

function Player::leaveMissionArea(%this)
{
	%client = Player::getClient(%this);
	if(%client.inDuel || %client.inArena)
	{
		%this.outArea = "";
		return;
	}

		if(%client.isshifting != true)
		{
		Client::sendMessage(Player::getClient(%this),1,"You have left the mission area.");
		}

		%this.outArea=1;
		alertPlayer(%this, 3); // 4
		
}

function rocket::leaveMissionArea(%this)
{
	Anni::Echo("proj out of area");
	messageall(1,"projectile out of area, foo!");
}
	
function Vector::Multiply(%vecA, %vecB)
{
	return getWord(%vecA, 0)*getWord(%vecB, 0)@" "@getWord(%vecA, 1)*getWord(%vecB, 1)@" "@getWord(%vecA, 2)*getWord(%vecB, 2);
} 

// -Death666
function bouncePlayerBack(%player)
{
	%client = Player::getClient(%player);
	%playerguy = Client::getOwnedObject(%player);
	if(Player::isDead(%player))
	{
		if(%player.hasmessage)
		{
	%vel = Item::getVelocity(%player);
	%newVel = Vector::neg(%vel);
	Item::setVelocity(%player, %newVel);
	// schedule("Item::setVelocity(" @ %player @ ", 10.0);", 0.3);
 	// schedule("Item::Pop(" @ %player @ ");", 10.5, %player);
				%player.hasmessage = false;
				schedule(%player@".hasmessage = true;",10.0,%player);	
	return;
		}
		else
		return;
	}

	if(%player.driver == "1")
	{
		Client::sendMessage(%client,1,"Warning your vehicle will self destruct out of bounds.");
		return;	
	}

	%CurrentSpread = GameBase::getDamageLevel(%player);
	%armor = Player::getArmor(%player);
	%Toast = %armor.maxDamage;
	%Butter = "0.05";
           	%Jelly = %CurrentSpread + %Butter;
	%c = Player::getClient(%player);


		Player::setDamageFlash(%player, 0.1);
		GameBase::setDamageLevel(%player,%Jelly);
		%vel = Item::getVelocity(%player);
		%newVel = Vector::neg(%vel);
		Item::setVelocity(%player, %newVel);

	if(%CurrentSpread + %Butter > %Toast)
	{
		Player::setDamageFlash(%player,0.75);
//		Player::setAnimation(%player, $PlayerAnim::DieSpin);
		%curDie = radnomItems(4, $PlayerAnim::DieHead, $PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
		Player::setAnimation(%player, %curDie);
		playNextAnim(%player);

	}

}


// end new
   

function alertPlayer(%player, %count)
{
	%client = Player::getClient(%player);
	if(%player.isDuck) //new bootcamp code
		return;

	if(%player.isDuelDuck)
		return;

	if(%player.outArea == 1 && $Annihilation::OutOfArea)
	{
		if(%count > 0)
		{
			Client::sendMessage(Player::getClient(%player),0,"~wLeftMissionArea.wav");
			schedule("alertPlayer(" @ %player @ ", " @ %count - 1 @ ");",1.5,%player);
		}
		else
		{
			%set = nameToID("MissionCleanup/ObjectivesSet");
			for(%i = 0; (%obj = Group::getObject(%set, %i)) != -1; %i++)
				GameBase::virtual(%obj, "playerLeaveMissionArea", %player);
		}
		
	}
	if(%player.outArea == 1 && !$Annihilation::OutOfArea) 
	{
	%type = getObjectType(%player);
	if(%type == "Player") 
	{
		if(Player::isAIControlled(%player))
		{
				GameBase::setDamageLevel(%player,GameBase::getDamageLevel(%player) + 0.6);
		}
	}
		%clientId = Player::getClient(%player);
		Client::sendMessage(%clientId,1,"~wLeftMissionArea.wav");
		if(%count > 1)
			schedule("alertPlayer(" @ %player @ ", " @ %count - 1 @ ");",1.5,%player);
		else if($TALT::Active == false) //New LT code
		{
			schedule("leaveMissionAreaDamage(" @ %clientId @ ");",1,%clientId);
			
		}
		else
		{
			schedule("leaveMissionAreaDamage(" @ %clientId @ ");",1,%clientId);
			%set = nameToID("MissionCleanup/ObjectivesSet");
			for(%i = 0; (%obj = Group::getObject(%set, %i)) != -1; %i++)
				GameBase::virtual(%obj, "playerLeaveMissionArea", %player);
		}
		
		if($TALT::Active == false) //moved here
			bouncePlayerBack(%player);
	}	
}


function leaveMissionAreaDamage(%client)
{
	%player = Client::getOwnedObject(%client);

//

	%CurrentSpread = GameBase::getDamageLevel(%player);
	%armor = Player::getArmor(%player);
	%Toast = %armor.maxDamage;
	%Butter = "0.5";
//
	if(%player.outArea == 1) 
	{
		if(!Player::isDead(%player)) 
		{
			if(%player.driver == "1")
			{
				Client::sendMessage(%client,1,"~wLeftMissionArea.wav");			
				%vehicle = Player::getMountObject(%player);
				GameBase::setDamageLevel(%vehicle,GameBase::getDataName(%vehicle).maxDamage);
			}	
			Player::setDamageFlash(%client,0.1);
			GameBase::setDamageLevel(%player,GameBase::getDamageLevel(%player) + 0.5);
			schedule("leaveMissionAreaDamage(" @ %client @ ");",1, %client);
//
			if(%CurrentSpread + %Butter > %Toast)
			{
					Player::setAnimation(%player, $PlayerAnim::DieSpin);
			}
//

		}
	}
}



function checkObjectives(%this)
{
	//Anni::Echo("checking for objective player leave mission area...");
}

// objective init must return true
function TowerSwitch::objectiveInit(%this)
{
	return %this.scoreValue || %this.deltaTeamScore;
}

function TowerSwitch::onAdd(%this)
{
	%this.numSwitchTeams = 0;
}

function TowerSwitch::onDamage()
{
	if($debug::Damage)
	{
		Anni::Echo("TowerSwitch::onDamage("@%this@", "@%type@", "@%value@", "@%pos@", "@%vec@", "@%mom@", "@%object@" )");
	}	
	// tower switches can't take damage
}

function TowerSwitch::getObjectiveString(%this, %forTeam)
{
	%thisTeam = GameBase::getTeam(%this);
	
	if($missionComplete)
	{
		if(%thisTeam == -1)
			return "<Btowers_neutral.bmp>\nNo team claimed " @ %this.objectiveName @ ".";
		else if(%thisTeam == %forTeam)
			return "<Btower_teamcontrol.bmp>\nYour team finished the mission in control of " @ %this.objectiveName @ ".";
		else 
		{
			if(%forTeam != -1)
				return "<Btower_enemycontrol.bmp>\nThe " @ getTeamName(%thisTeam) @ " team finished the mission in control of " @ %this.objectiveName @ ".";
			else
				return "<Btower_teamcontrol.bmp>\nThe " @ getTeamName(%thisTeam) @ " team finished the mission in control of " @ %this.objectiveName @ ".";
		}
	}
	else
	{
		if(%forTeam != -1)
		{
			if(%this.deltaTeamScore)
			{
				if(%thisTeam == -1)
 					return "<Btowers_neutral.bmp>\nClaim " @ %this.objectiveName @ " to gain " @ %this.deltaTeamScore @ " points per minute.";
 				else if(%thisTeam == %forTeam)
 					return "<Btower_teamcontrol.bmp>\nDefend " @ %this.objectiveName @ " to retain " @ %this.deltaTeamScore @ " points per minute.";
 				else
 					return "<Btower_enemycontrol.bmp>\nCapture " @ %this.objectiveName @ " from the " @ getTeamName(%thisTeam) @ " team to gain " @ %this.deltaTeamScore @ " points per minute.";
			}
			else if(%this.scoreValue)
			{
				if(%thisTeam == -1)
					return "<Btowers_neutral.bmp>\nClaim and defend " @ %this.objectiveName @ " to gain " @ %this.scoreValue @ " points.";
				else if(%thisTeam == %forTeam)
					return "<Btower_teamcontrol.bmp>\nDefend " @ %this.objectiveName @ " to retain " @ %this.scoreValue @ " points.";
				else
					return "<Btower_enemycontrol.bmp>\nCapture " @ %this.objectiveName @ " from the " @ getTeamName(%thisTeam) @ " team to gain " @ %this.deltaTeamScore @ " points.";
			}
		}
		else 
		{
 			if(%thisTeam == -1)
				return "<Btowers_neutral.bmp>\n" @ %this.objectiveName @ " has not been claimed.";
 			else
 				return "<Btower_teamcontrol.bmp>\nThe " @ getTeamName(%thisTeam) @ " team is in control of the " @ %this.objectiveName @ ".";
		}
	}
}

function TowerSwitch::onCollision(%this, %object)
{	
	if($debug) 
		event::collision(%this,%object);
	
	//Messy fucking bootcamp code
	if(GameBase::getMapName(%this) == "Chain!!")
	{
		spawnbot11(%object);
		schedule("spawnbot11(" @ %object @ ");", 11);
		client::sendmessage(Player::getClient(%object), 0, "~wMachgun3.wav");
	}
      if(GameBase::getMapName(%this) == "PlasmaTwo!!")
	{
		spawnbot6(%object);
		schedule("spawnbot6(" @ %object @ ");", 6);
		client::sendmessage(Player::getClient(%object), 0, "~wrifle1.wav");
	}
	if(GameBase::getMapName(%this) == "Mortar Bot Switch")	
	{
		spawnbot3(%object);
		spawnbot4(%object);
		spawnbot5(%object);
		client::sendmessage(Player::getClient(%object), 0, "~wmale1.wretreat.wav");
	}
      if(GameBase::getMapName(%this) == "Plasma!!")
	{
		spawnbot7(%object);
		schedule("spawnbot7(" @ %object @ ");", 7);
		client::sendmessage(Player::getClient(%object), 0, "~wplasma2.wav");
	}
	if(GameBase::getMapName(%this) == "Disc!!")
	{
		spawnbot10(%object);
		schedule("spawnbot10(" @ %object @ ");", 10);
		client::sendmessage(Player::getClient(%object), 0, "~wdiscreload.wav");
	}
	if(GameBase::getMapName(%this) == "ALL!!")	
	{
		spawnbot6(%object);
                spawnbot7(%object);
		spawnbot10(%object);
		spawnbot11(%object);
	}

	if(GameBase::getMapName(%this) == "MA Switch ALL")
	{
		Duck::PullMA::Start(%this, %object);
	}
	if(GameBase::getMapName(%this) == "MA Switch 1")
	{
		%ducksite = 0;
		duck::Pull(%ducksite, %object);
	}
	if(GameBase::getMapName(%this) == "MA Switch 2")
	{
		%ducksite = 1;
		duck::Pull(%ducksite, %object);
	}
	if(GameBase::getMapName(%this) == "MA Switch 3")
	{
		%ducksite = 2;
		duck::Pull(%ducksite, %object);
	}
	if(GameBase::getMapName(%this) == "MA Switch 4")
	{
		%ducksite = 3;
		duck::Pull(%ducksite, %object);
	}
	if(GameBase::getMapName(%this) == "Sniper Switch 1")
	{
		%ducksite = 4;
		duck::Pull(%ducksite, %object);
	}
	if(GameBase::getMapName(%this) == "Sniper Switch 2")
	{
		%ducksite = 5;
		duck::Pull(%ducksite, %object);
        	if(GameBase::getMapName(%this) == "Sniper Switch 5")
	{
		%ducksite = 5;
		duck::Pull(%ducksite, %object);
	}
	}
	if(GameBase::getMapName(%this) == "Sniper Switch 3")
	{
		%ducksite = 6;
		duck::Pull(%ducksite, %object);
	}
	if(GameBase::getMapName(%this) == "Sniper Switch 4")
	{
		%ducksite = 7;
		duck::Pull(%ducksite, %object);
	}
        if(GameBase::getMapName(%this) == "p1")
	{
		%ducksite = 11;
		duck::Pull(%ducksite, %object);
	}

         if(GameBase::getMapName(%this) == "p2")
	{
		%ducksite = 12;
		duck::Pull(%ducksite, %object);
	}
           if(GameBase::getMapName(%this) == "p3")
	{
		%ducksite = 13;
		duck::Pull(%ducksite, %object);
	}
            if(GameBase::getMapName(%this) == "p4")	
	{
		%ducksite = 14;
		duck::Pull(%ducksite, %object);
	}
    if(GameBase::getMapName(%this) == "p5")	
	{
		%ducksite = 30;
		duck::Pull(%ducksite, %object);
	}
    if(GameBase::getMapName(%this) == "p6")	
	{
		%ducksite = 31;
		duck::Pull(%ducksite, %object);
	}
                    if(GameBase::getMapName(%this) == "Sniper duck 7")
	{
		%ducksite = 15;
		duck::Pull(%ducksite, %object);
        }
    	if(GameBase::getMapName(%this) == "Sniper duck 8")
	{
		%ducksite = 28;
		duck::Pull(%ducksite, %object);
        }
	 if(GameBase::getMapName(%this) == "Sniper duck 9")
	{
		%ducksite = 29;
		duck::Pull(%ducksite, %object);
        }
        if(GameBase::getMapName(%this) == "Sniper Switch 6")
	{
		%ducksite = 16;
		duck::Pull(%ducksite, %object);
	}
           if(GameBase::getMapName(%this) == "Sniper Switch 5")	
	{
		%ducksite = 17;
		duck::Pull(%ducksite, %object);
	}
             if(GameBase::getMapName(%this) == "PlasmaTwo 1")
	{
		%ducksite = 18;
		duck::Pull(%ducksite, %object);
	}
                 if(GameBase::getMapName(%this) == "PlasmaTwo 2")
	{
		%ducksite = 19;
		duck::Pull(%ducksite, %object);
	}
     if(GameBase::getMapName(%this) == "PlasmaTwo 3")
	{
		%ducksite = 20;
		duck::Pull(%ducksite, %object);
	}
        if(GameBase::getMapName(%this) == "MA Switch 5")
	{
		%ducksite = 24;
		duck::Pull(%ducksite, %object);
	}
	if(GameBase::getMapName(%this) == "MA Switch 6")
	{
		%ducksite = 25;
		duck::Pull(%ducksite, %object);
	}
	if(GameBase::getMapName(%this) == "MA Switch 7")
	{
		%ducksite = 26;
		duck::Pull(%ducksite, %object);
	}
	if(GameBase::getMapName(%this) == "MA Switch 8")
	{
		%ducksite = 27;
		duck::Pull(%ducksite, %object);
	}



	if(GameBase::getMapName(%this) == "Info!!!")
	{
		%clientId = Player::getClient(%object);
		TA::BlackOut(%clientId,12);
		schedule("Client::sendMessage(" @ %clientId @ ", 1,\"~wteleport2.wav\");", 0);
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1> ]-[eY, " @ Client::getName(%clientId) @ "!\", 4);", 0);
		schedule("Client::sendMessage(" @ %clientId @ ", 1,\"~wusepack.wav\");", 3);
		schedule("bottomprint(" @ %clientId @ ", \"<jc><f1>Welcome to the <f3>B00zE <F0>B00T CAMP! (Version 3)\", 4);", 3);
		schedule("Client::sendMessage(" @ %clientId @ ", 1,\"~wshell_click.wav\");", 6);
		schedule("centerprint(" @ %clientId @ ", \"<jc><f1>Map created by <F0>B00zE\", 5);", 6);
	}
   	if(GameBase::getMapName(%this) == "F4A!")
	{
	 //GameBase::setPosition(%object,"-752.519 107.002 125.592"); 
		GameBase::setPosition(%object,GetOffsetRot("-752.519 107.002 125.592","0 0 0",$Arena::Spawn));
	}
 	if(GameBase::getMapName(%this) == "Climbing Arena!")
	{
	GameBase::setPosition(%object,"-1078.81 191.206 573.663");       
	}
 	if(GameBase::getMapName(%this) == "Top")
	{
	GameBase::setPosition(%object,"-848.679 234.076 2183.38");       
	}
	if(GameBase::getMapName(%this) == "Part 2")
	{
	GameBase::setPosition(%object,"1257.03 528.936 1248.76");       
	}
	 if(GameBase::getMapName(%this) == "Woooo-hoooo")
	{
	GameBase::setPosition(%object,"-1078.81 191.206 573.663");     
	messageall(1, Client::getName(%playerClient) @ " **** Woooooo-hoooooooo ****"); 
	schedule("bottomprint(" @ %playerClient @ ", \"<jc><f1>**** Good Job !! ****\", 1);", 0);
	}
        if(GameBase::getMapName(%this) == "Plasma Range!")
	{
	 //GameBase::setPosition(%object,"-390.959 -1048.62 171.786");
		GameBase::setPosition(%object,GetOffsetRot("-390.959 -1048.62 171.786","0 0 0",$Arena::Spawn));	 
	 }
          if(GameBase::getMapName(%this) == "Back 2 the base!")
	{
	 //GameBase::setPosition(%object,"-384.875 91.5404 147.5");   
		GameBase::setPosition(%object,GetOffsetRot("-384.875 91.5404 147.5","0 0 0",$Arena::Spawn));	 
	}
         if(GameBase::getMapName(%this) == "MA Range!")
	{
	 //GameBase::setPosition(%object,"251.025 358.568 182.652");
		GameBase::setPosition(%object,GetOffsetRot("251.025 358.568 182.652","0 0 0",$Arena::Spawn));
	}
         if(GameBase::getMapName(%this) == "Mortar Range!")
	{
	 GameBase::setPosition(%object,"498.895 -216.583 146.949");
	}
         if(GameBase::getMapName(%this) == "Sniper Range!")
	{
	// GameBase::setPosition(%object,"70.298 -371.778 197.386");
		GameBase::setPosition(%object,GetOffsetRot("70.298 -371.778 197.386","0 0 0",$Arena::Spawn));
	}
       if(GameBase::getMapName(%this) == "PlasmaTwo Range!")
	{
	 GameBase::setPosition(%object,"-881.963 -441.796 111.078");
	}
        if(GameBase::getMapName(%this) == "B00zE Bot Range!")
	{
	 GameBase::setPosition(%object,"33.7819 -6.85908 202.304");          
	}
        if(GameBase::getMapName(%this) == "Outside!")
	{
	 GameBase::setPosition(%object,"139.237 22.7593 162.525");          
	}
if(GameBase::getMapName(%this) == "Duel Base!")
	{
	 GameBase::setPosition(%object,"-170 -1884 154");          
	}
	if(GameBase::getMapName(%this) == "Dueler 2")
	{
	 GameBase::setPosition(%object,"-200.766 -1814.85 169.503");          
	}
	if(GameBase::getMapName(%this) == "Dueler 1")
	{
	 GameBase::setPosition(%object,"-140.767 -1814.86 169.504");          
	}
	if(GameBase::getMapName(%this) == "Start!")
	{
	 messageall(1, Client::getName(%playerClient) @ " Start the Fight!!");
	}
if(GameBase::getMapName(%this) == "Vertical climbing!")
	{
	GameBase::setPosition(%object,"-3137.1 -2682.86 -720.795");       
	}
	if(GameBase::getMapName(%this) == "Part 2!")
	{
		GameBase::setPosition(%object,"-3196.87 -1436.85 -959.511");       
	}
	if(GameBase::getMapName(%this) == "Energetic Switch")
	{
		client::sendmessage(Player::getClient(%object), 0, "~wshell_click.wav");
		Player::setItemCount(%playerClient,RepairKit,1);
			%playerClient = Player::getClient(%object);
		if (GameBase::getDamageLevel(Client::GetOwnedObject(%playerclient)) > 0)
		{
		GameBase::SetDamageLevel(Client::GetOwnedObject(%playerclient),0);
		}
	}
	
	if(Player::getClient(%object).inArena) return;
	
	%playerClient = Player::getClient(%object);
	if(getObjectType(%object) != "Player" || %playerClient.isSpy || Player::isDead(%object))
		return;

	%playerTeam = GameBase::getTeam(%object);
	%oldTeam = GameBase::getTeam(%this);
	if(%oldTeam == %playerTeam)
		return;

	if ( %this.NoCapture )
	{
		client::sendMessage(%playerClient, 1, "Wait a bit foo.~waccess_denied.wav");
		return;
	}

	%this.trainingObjectiveComplete = true;
	
	%touchClientName = Client::getName(%playerClient);
	%group = GetGroup(%this);
	Group::iterateRecursive(%group, GameBase::setTeam, %playerTeam);

	%dropPoints = nameToID(%group @ "/DropPoints");
	%oldDropSet = nameToID("MissionCleanup/TeamDrops" @ %oldTeam);
	%newDropSet = nameToID("MissionCleanup/TeamDrops" @ %playerTeam);

	$deltaTeamScore[%oldTeam] -= %this.deltaTeamScore;
	$deltaTeamScore[%playerTeam] += %this.deltaTeamScore;
	$teamScore[%oldTeam] -= %this.scoreValue;
	$teamScore[%playerTeam] += %this.scoreValue;

	if(%dropPoints != -1)
	{
		for(%i = 0; (%dropPoint = Group::getObject(%dropPoints, %i)) != -1; %i++)
		{
			if(%oldDropSet != -1)
				removeFromSet(%oldDropSet, %dropPoint);
			addToSet(%newDropSet, %dropPoint);
		}
	}

	if(%oldTeam == -1)
	{
		MessageAllExcept(%playerClient, 0, %touchClientName @ " claimed " @ %this.objectiveName @ " for the " @ getTeamName(%playerTeam) @ " team!");
		Client::sendMessage(%playerClient, 0, "You claimed " @ %this.objectiveName @ " for the " @ getTeamName(%playerTeam) @ " team!");
 	}
	else
	{
		if(%this.objectiveLine)
		{
			MessageAllExcept(%playerClient, 0, %touchClientName @ " captured " @ %this.objectiveName @ " from the " @ getTeamName(%oldTeam) @ " team!");
			Client::sendMessage(%playerClient, 0, "You captured " @ %this.objectiveName @ " from the " @ getTeamName(%oldTeam) @ " team!");
			%this.numSwitchTeams++;
			%playerClient.TTowerCaps++;
			schedule("TowerSwitch::timeLimitCheckPoints(" @ %this @ "," @ %playerClient @ "," @ %this.numSwitchTeams @ ");",60,%playerClient);
		}
	}
	if(%this.objectiveLine)
	{
		TeamMessages(1, %playerTeam, "Your team has taken an objective.~wCapturedTower.wav");
		TeamMessages(0, %playerTeam, "The " @ getTeamName(%playerTeam) @ " has taken an objective.");
		if(%oldTeam != -1)
			TeamMessages(1, %oldTeam, "The " @ getTeamName(%playerTeam) @ " team has taken your objective.~wLostTower.wav");
		ObjectiveMission::ObjectiveChanged(%this);
	}
	%this.NoCapture = True;
	schedule(%this@".NoCapture = False;", 5, %this);
	ObjectiveMission::checkScoreLimit();
}

function TowerSwitch::timeLimitCheckPoints(%this,%client,%numChange)
{
	//give player 5 points for capturing tower!
	if(%this.numSwitchTeams == %numChange)
	{
		%client.score+=5;
		%client.Credits+= 5;
		Game::refreshClientScore(%client);
		Client::sendMessage(%client, 0, "You receive 5 points for holding your captured tower!");
	}
}

function TowerSwitch::clientKilled(%this, %playerId, %killerId)
{		
	if(!%this.objectiveLine)
		return;

	%killerTeam = Client::getTeam(%killerId);
	%playerTeam = Client::getTeam(%playerId);
	%killerPos = GameBase::getPosition(%killerId);
		
	if(%killerId && (%playerTeam != %killerTeam))
	{	
		%dist = Vector::getDistance(%killerPos, GameBase::getPosition(%this));
		//Anni::Echo(%dist);
		if(%dist <= 80)
		{
			//Anni::Echo("distance to objective" @ %this @ " : " @ %dist);
			if(GameBase::getTeam(%this) == Client::getTeam(%killerId) && getObjectType(%killerId) == "Player")
			{
				%killerId.score++;
				%killerId.Credits++;
				Game::refreshClientScore(%killerId);
				messageAll(0, strcat(Client::getName(%killerId), " receives a bonus for defending " @ %this.objectiveName @ "."));
			}
		}
	}
}

function Flag::onDamage(%this,%type,%value,%pos,%vec,%mom,%vertPos,%quadrant,%object)
{
//	messageall(1,"flag dam");
	Item::setVelocity(%this, %newVel);
	Player::applyImpulse(%this,%mom);
}
	
// objective init must return true
function Flag::objectiveInit(%this)
{
	%this.originalPosition = GameBase::getPosition(%this);
	%this.atHome = true;
	%this.pickupSequence = 0;
	%this.carrier = -1;
	%this.holdingTeam = -1;
	%this.holder = "";
	%this.changeTeamCount = 0;

	%this.enemyCaps = 0;
	%this.caps[0] = 0;
	%this.caps[1] = 0;
	%this.caps[2] = 0;
	%this.caps[3] = 0;
	%this.caps[4] = 0;
	%this.caps[5] = 0;
	%this.caps[6] = 0;
	%this.caps[7] = 0;

	$teamFlag[GameBase::getTeam(%this)] = %this;
	return true;
}

function Flag::getObjectiveString(%this, %forTeam)
{
	%thisTeam = GameBase::getTeam(%this);
	//Anni::Echo("Flag objectiveString");
	
	if($missionComplete)
	{
		if(%thisTeam == -1)
		{
			if(%this.holdingTeam == %forTeam && %forTeam != -1)
				return "<Bflag_atbase.bmp>\nYour team finished the mission in control of " @ %this.objectiveName @ ".";
			else if(%this.holdingTeam == -1)
				return "<Bflag_neutral.bmp>\nNo team finished the mission in control of " @ %this.objectiveName @ ".";
			else
			{
				if(%forTeam != -1)
					return "<Bflag_enemycaptured.bmp>\nThe " @ getTeamName(%this.holdingTeam) @ " team finished the mission in control of " @ %this.objectiveName @ ".";
				else
					return "<Bflag_atbase.bmp>\nThe " @ getTeamName(%this.holdingTeam) @ " team finished the mission in control of " @ %this.objectiveName @ ".";
			}
		}
		else if(%forTeam != -1)
		{
			if(%thisTeam == %forTeam)
				return "<Bflag_atbase.bmp>\nYour flag was captured " @ %this.enemyCaps @ " times.";
			else
				return "<Bflag_enemycaptured.bmp>\nYour team captured the " @ getTeamName(%thisTeam) @ " flag " @ %this.caps[%forTeam] @ " times.";
		}
		else 
			return "<Bflag_atbase.bmp>\nThe " @ getTeamName(%thisTeam) @ "'s flag was captured " @ %this.enemyCaps @ " times.";
	}
	else
	{
		if(%thisTeam == -1)
		{
			if(%forTeam != -1)
			{
				if(%this.holdingTeam == %forTeam)
					return "<Bflag_atbase.bmp>\nDefend " @ %this.objectiveName @ ".";
				else if(%this.holdingTeam != -1)
					return "<Bflag_enemycaptured.bmp>\nGrab " @ %this.objectiveName @ " from the " @ getTeamName(%this.holdingTeam) @ " team.";
				else if(%this.carrier != -1)
				{
					if(GameBase::getTeam(%this.carrier) == %forTeam)
						return "<Bflag_atbase.bmp>\nConvey " @ %this.objectiveName @ " to an empty flag stand. (carried by " @ Client::getName(Player::getClient(%this.carrier)) @ ")";
					else
						return "<Bflag_enemycaptured.bmp>\nWaylay " @ Client::getName(Player::getClient(%this.carrier)) @ " and convey " @ %this.objectiveName @ " to your base.";
				}
				else if(%this.atHome)
					return "<Bflag_neutral.bmp>\nGrab " @ %this.objectiveName @ " and convey it to an empty flag stand.";
				else
					return "<Bflag_notatbase.bmp>\nFind " @ %this.objectiveName @ " and convey it to an empty flag stand.";
			}
			else
			{
				if(%this.holdingTeam != -1)
					return "<Bflag_atbase.bmp>\nThe " @ getTeamName(%this.holdingTeam) @ " team has " @ %this.objectiveName @ ".";
				else if(%this.carrier != -1)
					return "<Bflag_atbase.bmp>\n" @ Client::getName(Player::getClient(%this.carrier)) @ " has " @ %this.objectiveName @ ".";
				else if(%this.atHome)
					return "<Bflag_neutral.bmp>\n" @ %this.objectiveName @ " has not been found.";
				else
					return "<Bflag_notatbase.bmp>\n" @ %this.objectiveName @ " has been dropped in the field.";
			}
		}
		else
		{
			if(%thisTeam == %forTeam)
			{
				if(%this.atHome)
					return "<Bflag_atbase.bmp>\nDefend your flag to prevent enemy captures.";
				else if(%this.carrier != -1)
					return "<Bflag_enemycaptured.bmp>\nReturn your flag to base. (carried by " @ Client::getName(Player::getClient(%this.carrier)) @ ")";
				else
					return "<Bflag_notatbase.bmp>\nReturn your flag to base. (dropped in the field)";
			}
			else
			{
				if(%forTeam != -1)
				{
					if(%this.atHome)
						return "<Bflag_enemycaptured.bmp>\nGrab the " @ getTeamName(%thisTeam) @ " flag and touch it to your's to score " @ %this.scoreValue @ " points.";
					else if(%this.carrier == -1)
						return "<Bflag_notatbase.bmp>\nFind the " @ getTeamName(%thisTeam) @ " flag and touch it to your's to score " @ %this.scoreValue @ " points.";
					else if(GameBase::getTeam(%this.carrier) == %forTeam)
						return "<Bflag_atbase.bmp>\nEscort friendly carrier " @ Client::getName(Player::getClient(%this.carrier)) @ " to base.";
					else
						return "<Bflag_enemycaptured.bmp>\nWaylay enemy carrier " @ Client::getName(Player::getClient(%this.carrier)) @ " and steal his flag.";
				}
				else
				{
					if(%this.atHome)
						return "<Bflag_atbase.bmp>\nThe " @ getTeamName(%thisTeam) @ " flag is at their base.";
					else if(%this.carrier == -1)
						return "<Bflag_notatbase.bmp>\nThe " @ getTeamName(%thisTeam) @ " flag has been dropped in the field.";
					else 
						return "<Bflag_atbase.bmp>\n" @ Client::getName(Player::getClient(%this.carrier)) @ " has the " @ getTeamName(%thisTeam) @ " flag.";
				}
			}			
		}
	}
}

function Flag::onDrop(%player, %type)
{
	%playerTeam = GameBase::getTeam(%player);
	%flag = %player.carryFlag;
	
	%flag.dropped = true;
	%flag.bounced = "";
	%flagTeam = GameBase::getTeam(%flag);
	%playerClient = Player::getClient(%player);
	%dropClientName = Client::getName(%playerClient);
	$Stats::FlagDropped[%flagTeam] = getIntegerTime(true) >> 5; //New Stats code 
	$Stats::PlayerDropped[%flagTeam] = %playerClient;
	Anni::Echo("!! flag::ondrop, player="@%player@" flag="@%flag@" team="@getTeamName(%flagTeam)@" seq="@%flag.pickupSequence@" rotates? "@Item::isRotating(%flag));	
	if(%flagTeam == -1)
	{
		MessageAllExcept(%playerClient, 1, %dropClientName @ " dropped " @ %flag.objectiveName @ "!");
		Client::sendMessage(%playerClient, 1, "You dropped " @ %flag.objectiveName @ "!");
   $Spoonbot::HuntFlagrunner = 0;
	}
	else
	{
		MessageAllExcept(%playerClient, 0, %dropClientName @ " dropped the " @ getTeamName(%flagTeam) @ " flag!");
		Client::sendMessage(%playerClient, 0, "You dropped the " @ getTeamName(%flagTeam) @ " flag!");
		TeamMessages(1, %flagTeam, "Your flag was dropped in the field.", -2, "", "The " @ getTeamName(%flagTeam) @ " flag was dropped in the field.");
		%playerClient.lastActiveTimestamp = getSimTime(); //AFK System
   $Spoonbot::HuntFlagrunner = 0;
	}
	
	GameBase::throw(%flag, %player, 10, false);
	Item::hide(%flag, false);
	if($TALT::Active == false) //get rid of flag beacon 
		GameBase::setIsTarget(%flag,true);	
	Player::setItemCount(%player, "Flag", 0);
	%flag.carrier = -1;
	%player.carryFlag = "";
	Flag::clearWaypoint(%playerClient, false);
	schedule("Flag::checkReturn(" @ %flag @ ", " @ %flag.pickupSequence @ ");", $flagReturnTime, %flag); // 45 seconds after drop

	schedule("Flag::checkBoundary(" @ %flag @ ", " @ %flag.pickupSequence @ ");", 0.25, %flag); // 0.5
	schedule("Flag::checkBoundary(" @ %flag @ ", " @ %flag.pickupSequence @ ");", 3.0, %flag); // 0.5
	schedule("Flag::checkBoundary(" @ %flag @ ", " @ %flag.pickupSequence @ ");", 6.5, %flag); // 0.5

	schedule("Flag::checkBoundaryReturn(" @ %flag @ ", " @ %flag.pickupSequence @ ");", 15.5, %flag);

	%flag.dropFade = 1;
	ObjectiveMission::ObjectiveChanged(%flag);

   $Spoonbot::HuntFlagrunner = 0;

   schedule("Flag::AIReturnFlag(" @ %flag @ ");", 4);  // Makes the nearest AI return the flag.
}

function Flag::checkBoundary(%flag, %sequenceNum)
{

if(%flag.dropped == "true")
{ 
				%pos = gamebase::getposition(%flag);
				%x = getword(%pos,0);
				%y = getword(%pos,1);
if(%flag.bounced == "") 
{
				if(%x < $MissionInfo:X)
				{
					%curVelocity = Item::getVelocity(%flag);
					%newVel = Vector::neg(%curVelocity);
					Item::setVelocity(%flag, %newVel);
					%flag.bounced =1;
					schedule(%flag@".bounced  = \"\";",15.0,%flag);
					return;
				}
				if(%x > $MissionInfo:X + ($MissionInfo:H / 2) ) // yep need to cut this in half..
				{
					%curVelocity = Item::getVelocity(%flag);
					%newVel = Vector::neg(%curVelocity);
					Item::setVelocity(%flag, %newVel);
					%flag.bounced =1;
					schedule(%flag@".bounced  = \"\";",15.0,%flag);
					return;	
				}
				if(%y < $MissionInfo:Y)
				{
					%curVelocity = Item::getVelocity(%flag);
					%newVel = Vector::neg(%curVelocity);
					Item::setVelocity(%flag, %newVel);
					%flag.bounced =1;
					schedule(%flag@".bounced  = \"\";",15.0,%flag);
					return;
				}
				if(%y > $MissionInfo:Y + ($MissionInfo:W / 2) )
				{
					%curVelocity = Item::getVelocity(%flag);
					%newVel = Vector::neg(%curVelocity);
					Item::setVelocity(%flag, %newVel);
					%flag.bounced =1;
					schedule(%flag@".bounced  = \"\";",15.0,%flag);
					return;
				}
}
}
}

function Flag::checkBoundaryReturn(%flag, %sequenceNum)
{
				%pos = gamebase::getposition(%flag);
				%x = getword(%pos,0);
				%y = getword(%pos,1);

		if(%flag.dropped == "true")
		{ 

				if(%x < $MissionInfo:X)
				{
					schedule("Flag::checkReturn(" @ %flag @ ", " @ %flag.pickupSequence @ ");", 0.1, %flag);
					messageAll(0, "Flag returning from out of bounds..");
					return;
				}
				if(%x > $MissionInfo:X + ($MissionInfo:H / 2) )
				{
					schedule("Flag::checkReturn(" @ %flag @ ", " @ %flag.pickupSequence @ ");", 0.1, %flag);
					messageAll(0, "Flag returning from out of bounds..");
					return;	
				}
				if(%y < $MissionInfo:Y)
				{
					schedule("Flag::checkReturn(" @ %flag @ ", " @ %flag.pickupSequence @ ");", 0.1, %flag);
					messageAll(0, "Flag returning from out of bounds..");
					return;
				}
				if(%y > $MissionInfo:Y + ($MissionInfo:W / 2) )
				{
					schedule("Flag::checkReturn(" @ %flag @ ", " @ %flag.pickupSequence @ ");", 0.1, %flag);
					messageAll(0, "Flag returning from out of bounds..");
					return;
				}
		}
}

function Flag::checkReturn(%flag, %sequenceNum)
{
if(%flag.dropped == "true") //first off .. -Death
{ 
	Anni::Echo("checking for flag return: ", %flag, ", ", %sequenceNum);
	if(%flag.pickupSequence == %sequenceNum && %flag.timerOn == "")
	{
		if(%flag.dropFade)
		{ 
			GameBase::startFadeOut(%flag);
			%flag.dropFade= "";
			%flag.fadeOut= 1;
			schedule("Flag::checkReturn(" @ %flag @ ", " @ %sequenceNum @ ");", 2.5, %flag);
		}
		else
		{
			%flagTeam = GameBase::getTeam(%flag);
			if(%flagTeam == -1)
			{
				if(%flag.flagStand == "" || %flag.flagStand.flag != "")
				{
					MessageAll(0, %flag.objectiveName @ " was returned to its initial position.");
					%flag.dropped = false;
					GameBase::setPosition(%flag, %flag.originalPosition);
					if($TALT::Active == false) //get rid of flag beacon
						GameBase::setIsTarget(%flag,false);	
					Item::setVelocity(%flag, "0 0 0");
					%flag.flagStand = "";
   					$Spoonbot::HuntFlagrunner = 0;
					%flag.dropped = false;
					%flag.bounced= "";
				}
				else
				{
					%holdTeam = GameBase::getTeam(%flag.flagStand);
					TeamMessages(0, %holdTeam, "Your flag was returned to base.~wflagreturn.wav", -2, "", "The " @ getTeamName(GameBase::getTeam(%flag.flagStand)) @ " flag was returned to base.~wflagreturn.wav");
				   	$Spoonbot::HuntFlagrunner = 0;
					GameBase::setPosition(%flag, GameBase::getPosition(%flag.flagStand));
					if($TALT::Active == false) //get rid of flag beacon
						GameBase::setIsTarget(%flag,false);	
					%flag.dropped = false;
					%flag.bounced= "";
					%flag.flagStand.flag = %flag;
					%flag.holdingTeam = %holdTeam;
					%flag.carrier = -1;
					$teamScore[%holdTeam] += %flag.scoreValue;
					$deltaTeamScore[%holdTeam] += %flag.deltaTeamScore;
					%flag.holder = %flag.flagStand;
					TeamMessages(0,%holdTeam, "Your team holds " @ %flag.objectiveName @ ".~wflagcapture.wav", -2, "", "The " @ getTeamName(%playerTeam) @ " team holds " @ %flag.objectiveName @ ".");
					ObjectiveMission::checkScoreLimit();
				}
			}
			else
			{
				TeamMessages(0, %flagTeam, "Your flag was returned to base.~wflagreturn.wav", -2, "", "The " @ getTeamName(%flagTeam) @ " flag was returned to base.~wflagreturn.wav");
				GameBase::setPosition(%flag, %flag.originalPosition);
	      			$Spoonbot::HuntFlagrunner = 0;
				if($TALT::Active == false) //get rid of flag beacon
					GameBase::setIsTarget(%flag,false);	
				Item::setVelocity(%flag, "0 0 0");
				%flag.dropped = false;
				%flag.bounced= "";
			}
			%flag.atHome = true;
			GameBase::startFadeIn(%flag);
			%flag.fadeOut= "";
			%flag.bounced= "";
			%flag.dropped = false; // no longer dropped if returned ..
			ObjectiveMission::ObjectiveChanged(%flag);
   $Spoonbot::HuntFlagrunner = 0;
		}
	}
}
}

function HoldTheFlag::checkPoints(%clientId) //New LT code
{
	if(!Player::isDead(%clientId)) //little bug fix, nothing big
	{
		if(Player::getItemCount(%clientId,Flag) > 0)
		{
			%clientId.score += 0.1;
			%clientId.Credits += 0.1;
			Game::refreshClientScore(%clientId);
			schedule("HoldTheFlag::checkPoints(" @ %clientId @ ");", 5);
		}
	}
}

function Flag::onCollision(%this, %object)
{	
	if($debug) 
		event::collision(%this,%object);

	//	Anni::Echo("Flag collision ", %object," this ",%this);
	if(getObjectType(%object) != "Player")
		return;
	if(%this.carrier != -1)
		return; // spurious collision
//	if(Player::isAIControlled(%object))
//		return;
	%name = Item::getItemData(%this);

	%playerTeam = GameBase::getTeam(%object);
	%flagTeam = GameBase::getTeam(%this);
	%playerClient = Player::getClient(%object);
	if(%playerClient.inArena && %name == "Arena Flag") //new very early alpha Flag stuff
	{
		ArenaMSG(1,Client::getName(%playerClient)@" has grabbed the flag.");
		Player::setItemCount(%object, Flag, 1);
	}

	%touchClientName = Client::getName(%playerClient);
	if(%flagTeam == %playerTeam)
	{
		%armor = Player::getArmor(%object);
		if(Player::getMountedItem(%playerclient,$BackpackSlot) == ChameleonPack && %playerClient.isSpy) 
		{
			%player = Client::getOwnedObject(%playerclient);
			Player::setDamageFlash(%player,0.8);
		//	GameBase::setDamageLevel(%player,GameBase::getDamageLevel(%player)+0.1);
			Client::sendMessage(%playerclient ,1, "CAN NOT carry the flag with Chameleon Pack. ~wError_message.wav");
			return;
		}		
		
		// player is touching his own flag...
		if(!%this.atHome)	
		{
			if(%playerClient.isSpy)
				return;	
				
			// the flag isn't home! so return it.
			%flag.dropped = false;
			GameBase::startFadeOut(%this);
			GameBase::setPosition(%this, %this.originalPosition);
			if($TALT::Active == false) //get rid of flag beacon
				GameBase::setIsTarget(%this,false);	
			Item::setVelocity(%this, "0 0 0");
			GameBase::startFadeIn(%this);
			%this.atHome = true;
			%armor = player::getarmor(%object);
			%armorlist = $ArmorName[%armor].description;
			MessageAllExcept(%playerClient, 0, %touchClientName @ " returned the " @ getTeamName(%playerTeam) @ " flag in " @ (%armorlist) @ " armor! ~wflagreturn.wav"); //adding the armor type -death666
	if(%flagTeam == %playerTeam)
	{
			Client::sendMessage(%playerClient, 0, "You returned your teams flag! ~wflagreturn.wav");
	}
	if(%flagTeam != %playerTeam)
	{
			Client::sendMessage(%playerClient, 0, "You returned the " @ getTeamName(%playerTeam) @ " flag! ~wflagreturn.wav");
	}
			teamMessages(1, %playerTeam, "Your flag was returned to base.", -2, "", "The " @ getTeamName(%playerTeam) @ " flag was returned to base.");
        		$Spoonbot::HuntFlagrunner = 0;
			%this.pickupSequence++;
			ObjectiveMission::ObjectiveChanged(%this);
			%playerClient.FlagReturns++; //New Stats code
			%playerClient.TFlagRets++;
			%playerClient.score++;
			%playerClient.Credits++;
			%playerClient.lastActiveTimestamp = getSimTime(); //AFK System
			Game::refreshClientScore(%playerClient);
		}
      		else 
		{
			// it's at home - see if we have an enemy flag!
			if(%object.carryFlag != "")
			{
				// can't cap the neutral flags, duh
				%enemyTeam = GameBase::getTeam(%object.carryFlag);
				if(%enemyTeam != -1)
				{
					%armor = player::getarmor(%object);
					%armorlist = $ArmorName[%armor].description;
					MessageAllExcept(%playerClient, 0, %touchClientName @ " captured the " @ getTeamName(%enemyTeam) @ " flag in " @ (%armorlist) @ " armor! ~wflagcapture.wav"); //adding armor type -death666
					Client::sendMessage(%playerClient, 0, "You captured the " @ getTeamName(%enemyTeam) @ " flag! ~wflagcapture.wav");
					TeamMessages(1, %playerTeam, "Your team captured the flag.", %enemyTeam, "Your team's flag was captured.");

					%flag = %object.carryFlag;
					%flag.atHome = true;
					%flag.carrier = -1;
					%flag.caps[%playerTeam]++;
					%flag.enemyCaps++;

    						BotTree::StopTeamBots(%Playerclient);

					Item::hide(%flag, false);
					if($TALT::Active == false) //get rid of flag beacon
						GameBase::setIsTarget(%flag,false);
					$flagAtHome[1] = true;
					GameBase::setPosition(%flag, %flag.originalPosition);
					Item::setVelocity(%flag, "0 0 0");
		       			$Spoonbot::HuntFlagrunner = 0;

					%flag.trainingObjectiveComplete = true;
					ObjectiveMission::ObjectiveChanged(%flag);
					
					Player::setItemCount(%object, Flag, 0);
					%object.carryFlag = "";
					Flag::clearWaypoint(%playerClient, true);

					$teamScore[%playerTeam] += %flag.scoreValue;
					ObjectiveMission::checkScoreLimit();
					
					%capTime = getSimTime();
					%grabTime = %object.grabFlagTime;
					%totalTime = %capTime-%grabTime;
					%tmp = %totalTime;
					%inte = floor(%totalTime);
					%deci = floor(10*(%tmp - %inte));
					%totalTime = %inte @ "." @ %deci;
					%recTime = %playerClient.TFastestCap;

					if(%flag.dropped != "true")
					{	
						if(%totalTime < %recTime || %recTime == 0 || %recTime == "-1" || %recTime == "")
						{
							%playerClient.TFastestCap = %totalTime;
	
							if(%recTime == 0 || %recTime == "-1" || %recTime == "")
							   Bottomprint(%playerClient,"<jc><f2>You just set your Fastest Flag Capture record for this profile!\n<f1>You captured the flag in <f2>"@%totalTime@" <f1>seconds. This has been saved to your Player Profile, to beat your record try for a better flag cap time!",10);
							else
							   Bottomprint(%playerClient,"<jc><f2>You just beat your Fastest Flag Capture record!\n<f1>You captured the flag in <f2>"@%totalTime@"<f1> seconds!",10);
						}
					}
					%flag.dropped = false;
					
					//flag carrier gets 5 points for caputure
					%playerClient.score += 5;
					%playerClient.Credits += 5;
					%playerClient.ScoreCaps++; //New Stats code
					%playerClient.TFlagCaps++;
					%playerClient.lastActiveTimestamp = getSimTime(); //AFK System
					Game::refreshClientScore(%playerClient);
					messageAll(0, Client::getName(%playerClient) @ " receives 5 point capture bonus.");

				%AiId = Player::getClient(%Object);

				$BotThink::Definitive_Attackpoint[%aiId] = "";
				$BotThink::ForcedOfftrack[%aiId] = True;
				}
			}
		}
	}
// start enemy flag contact..
	else
	{
		%armor = Player::getArmor(%object);
		if(Player::getMountedItem(%playerclient,$BackpackSlot) == ChameleonPack)
		{
			if(!%playerClient.isSpy)	// 1/18/2005 5:39AM
			{
				%player = Client::getOwnedObject(%playerclient);
				Player::setDamageFlash(%player,0.8);
			//	GameBase::setDamageLevel(%player,GameBase::getDamageLevel(%player)+0.1);
				Client::sendMessage(%playerclient ,1, "CAN NOT carry the flag with Chameleon Pack. ~wError_message.wav");
			}
			return;	
		}
// it's an enemy's flag! woohoo!

		if(%object.carryFlag == "")
		{
			if(%object.outArea == "") 
			{
				// don't pick up our flags
				if(%this.holdingTeam == %playerTeam)
					return;
					
				Player::setItemCount(%object, Flag, 1);
				%flag.dropped = false;
				Player::mountItem(%object, Flag, $FlagSlot, %flagTeam);
				Item::hide(%this, true);
				if($TALT::Active == false) //get rid of flag beacon
					GameBase::setIsTarget(%this,false);	


				%object.GrabFlagTime = getSimTime();
			
			if(%playerClient.FlagCurse)
			 schedule("PlayerFlagCurse("@%playerClient@");",2,%playerClient);

				$flagAtHome[1] = false;
				%this.atHome = false;
				%this.carrier = %object;
				%this.pickupSequence++;
				%object.carryFlag = %this;
				Flag::setWaypoint(%playerClient, %this);


				%AiId = Player::getClient(%Object);
				%aiTeam = GameBase::getTeam( %aiId );

				%Target = BotFuncs::GetFlagId(%aiTeam);

				%LastPoint = True;
				%AttackPos = GameBase::getPosition(%Target);
				%AttackPoint = %Target;

				BotTree::GetMeToPos(%aiId, %AttackPos, True);

				$BotThink::Definitive_Attackpoint[%aiId] = %AttackPoint;
				$BotThink::Definitive_Attackpos[%aiId] = %AttackPos;
				$BotThink::ForcedOfftrack[%aiId] = true;
				$BotThink::LastPoint[%aiId] = %LastPoint;

				if(%this.fadeOut) 
				{
					GameBase::startFadeIn(%this);
					%this.fadeOut= "";
				}
				if((%this.lastTeam == "" || %this.lastTeam != %playerTeam) && %flagTeam == -1)
				{
					%this.currentFlagStand="";
					%this.changeTeamCount++;
					%this.lastTeam = %playerTeam;
					%this.timerOn = 1;
					if($flagToStandTime >= 30)
					{
						%timeToStand = $flagToStandTime - 30;
						%timeLeft = 30;
						if($flagToStandTime > 30)
							Client::sendMessage(%playerClient, 0, "You have " @ $flagToStandTime @ " sec to put the flag in a stand.");
					}
					else
					{
						if($flagToStandTime >= 10)
							%remain = $flagToStandTime % 10;
						else 
							%remain = $flagToStandTime % 5;
						if(%remain > 0 && %remain != $flagToStandTime)
						{
							%timeToStand = %remain;
							%timeLeft = $flagToStandTime - %remain;
							Client::sendMessage(%playerClient, 0, "You have " @ $flagToStandTime @ " sec to put the flag in a stand.");
						}
						else
						{
							%timeToStand = 0;
							%timeLeft = $flagToStandTime;
						}
					}
					schedule("Flag::checkFlagsTime(" @ %this @"," @ %timeLeft @ "," @ %this.changeTeamCount @ ");",%timeToStand, %this);
				}
				if(%flagTeam != -1)
				{
					%armor = player::getarmor(%object);
					%armorlist = $ArmorName[%armor].description;
					MessageAllExcept(%playerClient, 0, %touchClientName @ " took the " @ getTeamName(%flagTeam) @ " flag in " @ (%armorlist) @ " armor! ~wflag1.wav"); //adding the armor type -death666
					Client::sendMessage(%playerClient, 0, "You took the " @ getTeamName(%flagTeam) @ " flag! ~wflag1.wav");
					TeamMessages(1, %playerTeam, "Your team has the " @ getTeamName(%flagTeam) @ " flag.", %flagTeam, "Your team's flag has been taken.");
					if($TALT::Active == true)
						HoldTheFlag::checkPoints(%playerClient); //New LT code
				}
				else
				{
					%hteam = %this.holdingTeam;
					if(%hteam != -1)
					{
						$teamScore[%hteam] -= %this.scoreValue;
						$deltaTeamScore[%hteam] -= %this.deltaTeamScore;
						MessageAllExcept(%playerClient, 0, %touchClientName @ " took " @ %this.objectiveName @ " from the " @ getTeamName(%hteam) @ " team.~wflag1.wav");
						Client::sendMessage(%playerClient, 0, "You took " @ %this.objectiveName @ " from the " @ getTeamName(%hteam) @ " team.~wflag1.wav");
						TeamMessages(1, %playerTeam, "Your team has " @ %this.objectiveName @ ".", %hteam, "Your team lost " @ %this.objectiveName @ ".", "The " @ getTeamName(%playerTeam) @ " team has taken " @ %this.objectiveName @ " from the " @ getTeamName(%hteam) @ " team.");
						%this.holdingTeam = -1;
						%this.holder.flag = "";
					}
					else
					{
						MessageAllExcept(%playerClient, 0, %touchClientName @ " took " @ %this.objectiveName @ ".~wflag1.wav");
						Client::sendMessage(%playerClient, 0, "You took " @ %this.objectiveName @ ".~wflag1.wav");
						TeamMessages(1, %playerTeam, "Your team has " @ %this.objectiveName @ ".", -2, "", "The " @ getTeamName(%playerTeam) @ " team has taken " @ %this.objectiveName @ ".");
					}
				}
				%this.trainingObjectiveComplete = true;
				ObjectiveMission::ObjectiveChanged(%this);

			//Now we'll set up all enemy AI players to chase the flag carrier. 
			
			   %startCl = Client::getFirst();	//Browse through all clients and check for bots.
			   %endCl = %startCl + 90;
			   for(%cl = %startCl; %cl < %endCl; %cl = %cl + 1)
			       if (Player::isAIControlled(%cl)) //Is this a bot?
			       {
			        %aiName = Client::getName(%cl);
				%aiTeam = Client::getTeam(%cl);
				if (%aiTeam==%flagTeam)
				 {
				  AI::HuntTarget(%aiName, %playerClient, 1);
				  if ($Spoonbot::DebugMode)
				    echo (%aiName @ " now hunts the flagrunner " @ %playerClient);
				  $Spoonbot::HuntFlagrunner = %playerClient;
				 }
			       }

			//End of AI chasing enemy flag runner.

			}
			else if($TA::Rabbit)
			{
				// the flag isn't home! so return it.
				GameBase::startFadeOut(%this);
				GameBase::setPosition(%this, %this.originalPosition);
				if($TALT::Active == false) //get rid of flag beacon
					GameBase::setIsTarget(%this,false);	
				Item::setVelocity(%this, "0 0 0");
				GameBase::startFadeIn(%this);
				%this.atHome = true;
				MessageAllExcept(%playerClient, 0, %touchClientName @ " returned the " @ getTeamName(%playerTeam) @ " flag!~wflagreturn.wav");
	if(%flagTeam == %playerTeam)
	{
			Client::sendMessage(%playerClient, 0, "You returned your teams flag! ~wflagreturn.wav");
	}
	if(%flagTeam != %playerTeam)
	{
			Client::sendMessage(%playerClient, 0, "You returned the " @ getTeamName(%playerTeam) @ " flag! ~wflagreturn.wav");
	}
				teamMessages(1, %playerTeam, "The flag was returned to base.", -2, "", "The " @ getTeamName(%playerTeam) @ " flag was returned to base.");
				%this.pickupSequence++;
				ObjectiveMission::ObjectiveChanged(%this);
				%playerClient.FlagReturns++; //New Stats code
				%playerClient.TFlagRets++;
				%playerClient.score++;
				%playerClient.Credits++;
				%playerClient.lastActiveTimestamp = getSimTime(); //AFK System
				Game::refreshClientScore(%playerClient);
			}
			else
				Client::sendMessage(%playerClient, 1, "Flag not in mission area.");
		}
	}
}

function Flag::checkFlagsTime(%flag,%timeleft,%changeCount)
{
	if(%flag.changeTeamCount == %changeCount)
	{
		%client = Player::getClient(%flag.carrier);
		if(%timeleft <= 0 && %flag.currentFlagStand == "")
		{ 
			GameBase::startFadeOut(%flag);
			Player::setItemCount(%flag.carrier, "Flag", 0);
			%clientName = Client::getName(%client);
			%flagTeam = GameBase::getTeam(%flag);
			if(%flagTeam == -1 && (%flag.flagStand == "" || (%flag.flagStand).flag != "")) 
			{
				if(%client != -1)
				{
					MessageAllExcept(%client, 0, %clientName @ " didn't put " @ %flag.objectiveName @ " in a flag stand in time!  It was returned to its initial position.");
					Client::sendMessage(%client, 0, "You didn't get " @ %flag.objectiveName @ " to a flag stand in time!  It was returned to its initial position.");
				}
				else
					MessageAll(0, %flag.objectiveName @ " was not put in a flag stand in time!  It was returned to its initial position.");
				GameBase::setPosition(%flag, %flag.originalPosition);
				Item::setVelocity(%flag, "0 0 0");
				%flag.flagStand = "";
			}
			else
			{
				if(%flagTeam != -1)
				{
					%team = %flagTeam;
					GameBase::setPosition(%flag, %flag.originalPosition);
					Item::setVelocity(%flag, "0 0 0");
				}
				else
				{
					%team = GameBase::getTeam(%flag.flagStand);
					GameBase::setPosition(%flag, GameBase::getPosition(%flag.flagStand));
					Item::setVelocity(%flag, "0 0 0");
				}
				if(%client != -1)
				{
					MessageAllExcept(%client, 0, %clientName @ " didn't put " @ %flag.objectiveName @ " in a flag stand in time!");
					Client::sendMessage(%client, 0, "You didn't get " @ %flag.objectiveName @ " to a flag stand in time!");
				}
				else
					MessageAll(0, %flag.objectiveName @ " was not put in a flag stand in time!");
				TeamMessages(1, %team, %flag.objectiveName @ " was returned to your base.~wflagreturn.wav", -2, "", %flag.objectiveName @ " was returned to the " @ getTeamName(%team) @ " base.");
				%holdTeam = GameBase::getTeam(%flag.flagStand);
				$teamScore[%holdTeam] += %flag.scoreValue;
				$deltaTeamScore[%holdTeam] += %flag.deltaTeamScore;
				%flag.holder = %flag.flagStand;
				%flag.flagStand.flag = %flag;
				%flag.holdingTeam = %holdTeam;
			}
			GameBase::startFadeIn(%flag);
			Item::hide(%flag, false);
			if($TALT::Active == false) //get rid of flag beacon
				GameBase::setIsTarget(%flag,false);	
			(%flag.carrier).carryFlag = "";
			%flag.carrier = -1;
			Flag::clearWaypoint(%client, false);
			ObjectiveMission::ObjectiveChanged(%flag);
			ObjectiveMission::checkScoreLimit();
			%flag.lastTeam = "";
		}
		else if(%flag.currentFlagStand == "")
		{
			Client::sendMessage(%client, 0, "You have " @ %timeleft @ " sec to put the flag in a stand.");
			if(%timeleft <= 5)
			{
				%timeleft--;
				%nextTime = 1;
			}
			else if(%timeleft == 10)
			{
				%timeleft -= 5;
				%nextTime = 5;
			}
			else
			{
				%timeleft -= 10;
				%nextTime = 10;
			}
			schedule("Flag::checkFlagsTime(" @ %flag @","@ %timeleft @"," @ %changeCount @ ");",%nextTime, %flag);
		}
	}
}

function Flag::clearWaypoint(%client, %success)
{
	if(%success)
		setCommandStatus(%client, 0, "Objective completed.~wobjcomp");
	else
		setCommandStatus(%client, 0, "Objective failed.");
}

function Flag::setWaypoint(%client, %flag)
{
	if(!%client.autoWaypoint)
		return;
	%flagTeam = GameBase::getTeam(%flag);
	%team = Client::getTeam(%client);

	if(%flagTeam == -1)
	{ 
		for(%s = $teamFlagStand[%team]; %s != ""; %s = %s.nextFlagStand) 
		{
			if(%s.flag == "")
			{
				%pos = GameBase::getPosition(%s);
				%posX = getWord(%pos,0);
				%posY = getWord(%pos,1);
				issueCommand(%client, %client, 0,"Take " @ %flag.objectiveName @ " to empty flag stand.~wcapobj", %posX, %posY);
				return;
			}
		}
	}
	else
	{
		%pos = ($teamFlag[%team]).originalPosition;
		%posX = getWord(%pos,0);
		%posY = getWord(%pos,1);
  		issueCommand(%client, %client, 0,"Take the " @ getTeamName(%flagTeam) @ " flag to our flag.~wcapobj", %posX, %posY);

		BotTree::StopTeamBots(%client);	

		return;
	}
}

function FlagStand::objectiveInit(%this)
{
	%this.flag = "";
	%team = GameBase::getTeam(%this);

	%this.nextFlagStand = $teamFlagStand[%team];
	$teamFlagStand[%team] = %this;
	
	return false;
}

function FlagStand::onCollision(%this, %object)
{	
	if($debug) 
		event::collision(%this,%object);

	//Anni::Echo("FlagStand collision ", %object);
	%standTeam = GameBase::getTeam(%this);
	%playerTeam = GameBase::getTeam(%object);

	if(%standTeam == -1 || getObjectType(%object) != "Player" || %object.carryFlag == ""
			|| %playerTeam != %standTeam || %this.flag != "" || GameBase::getTeam(%object.carryFlag) != -1)
		return;

	// if we're here, we're carrying a flag, we've hit 
	// our flag stand, it doesn't have a flag, and we're not carrying
	// a team coded flag.

	%flag = %object.carryFlag;
	%flag.carrier = -1;
	Item::hide(%flag, false);
	if($TALT::Active == false) //get rid of flag beacon
		GameBase::setIsTarget(%flag,false);	
	GameBase::setPosition(%flag, GameBase::getPosition(%this));
	%flag.flagStand = %this;
	Player::setItemCount(%object, Flag, 0);
	%object.carryFlag = "";
	%playerClient = Player::getClient(%object);
	Flag::clearWaypoint(%playerClient, true);

	$teamScore[%playerTeam] += %flag.scoreValue;
	$deltaTeamScore[%playerTeam] += %flag.deltaTeamScore;
	%flag.holder = %this;
	%flag.holdingTeam = %playerTeam;
	%this.flag = %flag;
	%flag.currentFlagStand = %this;

	MessageAllExcept(%playerClient, 0, Client::getName(%playerClient) @ " conveyed " @ %flag.objectiveName @ " to base.");
	Client::sendMessage(%playerClient, 0, "You conveyed " @ %flag.objectiveName @ " to base.");
	TeamMessages(1, %playerTeam, "Your team holds " @ %flag.objectiveName @ ".~wflagcapture.wav", -2, "", "The " @ getTeamName(%playerTeam) @ " team holds " @ %flag.objectiveName @ ".");

	%flag.trainingObjectiveComplete = true;
	ObjectiveMission::ObjectiveChanged(%flag);
	ObjectiveMission::checkScoreLimit();
}

function Flag::clientKilled(%this, %playerId, %killerId)
{
	%player = Client::getOwnedObject(%playerId);
	%killer = Client::getOwnedObject(%killerId);
	if(%player == -1 || %killer == -1)
		return;

	%flagTeam = GameBase::getTeam(%this);
	if(%flagTeam == -1)
		return;

	%playerTeam = GameBase::getTeam(%player);
	%killerTeam = GameBase::getTeam(%killer);

	if(%playerTeam == %killerTeam)
		return;

	// killer's the only guy who gets a bonus.
	if(%killerTeam == %flagTeam)
	{
		// check for defending the flag
		// only if the flag is not being carried
		if(%this.carrier == -1)
		{
			%flagPos = GameBase::getPosition(%this);
			%playerPos = GameBase::getPosition(%player);

			if(Vector::getDistance(%flagPos, %playerPos) < 80)
			{
				%killerId.score++;
				%killerId.Credits++;
				Game::refreshClientScore(%killerId);
				messageAll(0, Client::getName(%killerId) @ " gets a bonus for defending the flag!");
			}
		}
	}
	else
	{
		if(%this.carrier != -1)
		{
			%carrierTeam = GameBase::getTeam(%this.carrier);
			// check for defending the carrier bonus
			if(%carrierTeam == %killerTeam)
			{
				if(Vector::getDistance(GameBase::getPosition(%this.carrier), GameBase::getPosition(%killer)) < 80)
				{
					%killerId.score++;
					%killerId.Credits++;
					Game::refreshClientScore(%killerId);
					messageAll(0, Client::getName(%killerId) @ " gets a bonus for defending the flag carrier!");
				}
			}
		}
	}
}

function Flag::clientDropped(%this, %clientId)
{
	//Anni::Echo(%this @ " " @ %clientId);
	%type = Player::getMountedItem(%clientId, $FlagSlot);
	if(%type != -1)
		Player::dropItem(%clientId, %type);
}

function Flag::playerLeaveMissionArea(%this, %playerId)
{
	//Anni::Echo("%this "@%this @" %playerId "@%playerId);
	// if a guy leaves the area, warp the flag back to its base
	if(%this.carrier == %playerId)
	{
		GameBase::startFadeOut(%this);
		Player::setItemCount(%playerId, "Flag", 0);
		%playerClient = Player::getClient(%playerId);
		%clientName = Client::getName(%playerClient);
		%flagTeam = GameBase::getTeam(%this);
		if(%flagTeam == -1 && (%this.flagStand == "" || (%this.flagStand).flag != "") ) 
		{
			MessageAllExcept(%playerClient, 0, %clientName @ " left the mission area while carrying " @ %this.objectiveName @ "!  It was returned to its initial position.");
			Client::sendMessage(%playerClient, 0, "You left the mission area while carrying " @ %this.objectiveName @ "!  It was returned to its initial position.");
			GameBase::setPosition(%this, %this.originalPosition);
	      		$Spoonbot::HuntFlagrunner = 0;
			Item::setVelocity(%this, "0 0 0");
			%this.flagStand = "";
		}
		else
		{
			if(%flagTeam != -1)
			{
				%team = %flagTeam;
				GameBase::setPosition(%this, %this.originalPosition);
				Item::setVelocity(%this, "0 0 0");
			}
			else
			{
				%team = GameBase::getTeam(%this.flagStand);
				GameBase::setPosition(%this, GameBase::getPosition(%this.flagStand));
				Item::setVelocity(%this, "0 0 0");
			}
			MessageAllExcept(%playerClient, 0, %clientName @ " left the mission area while carrying the " @ getTeamName(%team) @ " flag!");
			Client::sendMessage(%playerClient, 0, "You left the mission area while carrying the " @ getTeamName(%team) @ " flag!");
			TeamMessages(1, %team, "Your flag was returned to base.~wflagreturn.wav", -2, "", "The " @ getTeamName(%team) @ " flag was returned to base.");
	      		$Spoonbot::HuntFlagrunner = 0;
			%holdTeam = GameBase::getTeam(%this.flagStand);
			$teamScore[%holdTeam] += %this.scoreValue;
			$deltaTeamScore[%holdTeam] += %this.deltaTeamScore;
			%this.holder = %this.flagStand;
			%this.flagStand.flag = %this;
			%this.holdingTeam = %holdTeam;
			%this.atHome = true; //no idea how this ended up missing
		}
		GameBase::startFadeIn(%this);
		%this.carrier = -1;
		Item::hide(%this, false);
		if($TALT::Active == false) //get rid of flag beacon
			GameBase::setIsTarget(%this,false);	

		%playerId.carryFlag = "";
		Flag::clearWaypoint(%playerClient, false);
		ObjectiveMission::ObjectiveChanged(%this);
		ObjectiveMission::checkScoreLimit();
	}
}

function Sensor::objectiveInit(%this)
{
	return StaticShape::objectiveInit(%this);
}

function Turret::objectiveInit(%this)
{
	return StaticShape::objectiveInit(%this);
}

function StaticShape::objectiveInit(%this)
{
	%this.destroyerTeam = "";
	return %this.scoreValue != "";
}

function Sensor::getObjectiveString(%this, %forTeam)
{
	return StaticShape::getObjectiveString(%this, %forTeam);
}

function Turret::getObjectiveString(%this, %forTeam)
{
	return StaticShape::getObjectiveString(%this, %forTeam);
}

function StaticShape::getObjectiveString(%this, %forTeam)
{
	%thisTeam = GameBase::getTeam(%this);
	if(%this.destroyerTeam != "")
	{
		if(%forTeam == %this.destroyerTeam && %thisTeam != %forTeam)
			return "<Bitem_ok.bmp>\nYour team successfully destroyed the " @ getTeamName(%thisTeam) @ " " @ %this.objectiveName @ " objective.";
		else if(%forTeam == %thisTeam)
			return "<Bitem_damaged.bmp>\nYour team failed to defend " @ %this.objectiveName;
		else
			return "<Bitem_ok.bmp>\n" @ getTeamName(%this.destroyerTeam) @ " team destroyed the " @ getTeamName(%thisTeam) @ " " @ %this.objectiveName @ " objective.";
	}
	else
	{
		if($missionComplete)
		{
			if(%forTeam != -1) 
			{
				if(%forTeam == %thisTeam)
					return "<Bitem_ok.bmp>\nYour team successfully defended " @ %this.objectiveName @ ".";
				else
					return "<Bitem_damaged.bmp>\nYour team failed to destroy " @ getTeamName(%thisTeam) @ " objective, " @ %this.objectiveName @ ".";
			}
			else 
				return "<Bitem_ok.bmp>\n" @ getTeamName(%thisTeam) @ " failed to destroy the " @ %this.objectiveName @ " objective.";
		}
		else
		{
			if(%forTeam != -1) 
			{
				if(%forTeam == %thisTeam)
					return "<Bitem_ok.bmp>\nDefend " @ %this.objectiveName @ ".";
				else
					return "<Bitem_damaged.bmp>\nDestroy " @ getTeamName(%thisTeam) @ " objective, " @ %this.objectiveName @ "(" @ %this.scoreValue @ " points).";
			}
			else 
				return "<Bitem_ok.bmp>\n" @ getTeamName(%thisTeam) @ " must defend the " @ %this.objectiveName @ " objective.";
		}
	}
}

function StaticShape::timeLimitReached(%this)
{
	if(%this.scoreValue && !%this.destroyerTeam)
	{
		// give the defense some props!
		$teamScore[GameBase::getTeam(%this)] += %this.scoreValue;
	}
}

function StaticShape::objectiveDestroyed(%this)
{
	if(%this.destroyed == "")
	{
		// test if it's really an objective
		if(!%this.objectiveLine)
			return;
		%destroyerTeam = %this.lastDamageTeam;
		%thisTeam = GameBase::getTeam(%this);
		%playerClient = GameBase::getControlClient(%this.lastDamageObject);
		if(%playerClient != -1)
			%clientName = Client::getName(%playerClient);

		if(%thisTeam == %destroyerTeam)
		{
			// uh-oh... we killed our own stuff.
			// award the points to everyone else
			for(%i = 0; %i < getNumTeams()-1; %i++)
			{
				if(%i == %thisTeam)
					continue;
				$teamScore[%i] += %this.scoreValue;
			}
			if(%playerClient != -1)
			{
				MessageAllExcept(%playerClient, 0, %clientName @ " destroyed a friendly objective.");
				Client::sendMessage(%playerClient, 0, "You destroyed a friendly objective!");
			}
			MessageAll(1, getTeamName(%destroyerTeam) @ " objective " @ %this.objectiveName @ " destroyed.");
		}
		else
		{
			$teamScore[%destroyerTeam] += %this.scoreValue;
			if(%playerClient != -1)
			{
				%playerClient.score+=5;
				%playerClient.Credits+= 5;
				Game::refreshClientScore(%playerClient);
				MessageAllExcept(%playerClient, 0, %clientName @ " destroyed an objective!");
				Client::sendMessage(%playerClient, 0, "You destroyed an objective!");
			}
			MessageAll(1, getTeamName(%thisTeam) @ " objective " @ %this.objectiveName @ " destroyed.");
		}
		%this.destroyerTeam = %destroyerTeam;
		ObjectiveMission::ObjectiveChanged(%this);
		ObjectiveMission::checkScoreLimit();
		%this.destroyed = 1;
	}
}

function StaticShape::objectiveDisabled(%this)
{
}

function Flag::GetFlagPosition(%flag)
{

   %flagTeam = GameBase::getTeam(%flag);
	if(%flagTeam == -1)
	{ 
		for(%s = $teamFlagStand[%team]; %s != ""; %s = %s.nextFlagStand) 
		{
			if(%s.flag == "") {
				%pos = GameBase::getPosition(%s);
				%posX = getWord(%pos,0);
				%posY = getWord(%pos,1);
				%waypoint = %posX @ " " @ %posY;
				%worldLoc = WaypointToWorld (%waypoint);
				return %worldLoc;
			}
	        }
	}
}



function Flag::AIReturnFlag(%flag)  //The nearest AI player should now come and return the flag. 
{
//	messageall(1, "AI Return Flag Started.");
			   %flagTeam = GameBase::getTeam(%flag);

			%pos = ($teamFlag[%flagTeam]).originalPosition;
			%xPos = getWord(%pos, 0);
			%yPos = getword(%pos, 1);
			%zPos = getWord(%pos, 2);
			%flagPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;


			   %minDistance = 999999;
			   %startCl = Client::getFirst();	//Browse through all clients and check for bots.
			   %endCl = %startCl + 90;
			   for(%cl = %startCl; %cl < %endCl; %cl = %cl + 1)
			       if (Player::isAIControlled(%cl)) //Is this a bot?
			       {
			        %aiName = Client::getName(%cl);
				%aiTeam = Client::getTeam(%cl);
				if (%aiTeam==%flagTeam)
				 {

					AI::DirectiveWaypoint( %aiName, %flagPos, 125);
//					AI::SetVar(%aiName, spotDist, 0);
//					AI::SetVar(%aiName, seekOff , 1);
//					schedule("AI::SetVar(" @ %aiName @ "," @ spotDist @ ", 350);",60); // 150
//					schedule("AI::SetVar(" @ %aiName @ "," @ seekOff @ " , 0);",60);
//					schedule("AI::DirectiveRemove(" @ %aiName @ ", 125);",60);
				 }
			       }


			 //  TeamMessages(1, %flagTeam, "Our bots try to return our flag", -2, "", "The " @ getTeamName(%playerTeam) @ " bots try to return their flag to their base!!");

			//End of AI returning the flag.

}

function getMissionNumber(%mission)
{
	for(%i = 0;%i < $TotalMissions; %i++)
	{
		if (%mission == $TotalMissionList[%i])
		{
			%MissionNumber = %i;
			return %MissionNumber;
		}
	}
}
function Flag::findDistance()
{
	getterrain();
	if(getNumTeams() == 2)
	{
		%t1Flagpos = ($teamFlag[0]).originalPosition;
		%t2Flagpos = ($teamFlag[1]).originalPosition;
		$FlagDistance = Vector::getDistance(%t1FlagPos,%t2FlagPos);
		Anni::Echo("Flag To Flag Distance " @ $FlagDistance);
		Anni::Echo("Haze Distance " @ $Haze);
		Anni::Echo("Visible Distance " @ $visDistance);
	}
	else
	{
		$FlagDistance = 0;
		Anni::Echo("Too many teams for flag checking.");
	}
}

//$blahdeda = true;

function ObjectiveMission::setObjectiveHeading()
{
	//if($blahdeda)
	//{
	//	
	//}
	if($TA::Stats)
	{
		if(!$Server::timeLimit)
		%tstr = "No time limit on the game.";
	else if($timeLimitReached)
	{
		%time = getSimTime() - $missionStartTime;
		%minutes = Time::getMinutes(%time);
		%seconds = Time::getSeconds(%time);
		if(%minutes < 10)
			%minutes = "0" @ %minutes;
		if(%seconds < 10)
			%seconds = "0" @ %seconds;
			%tstr = %minutes @ ":" @ %seconds;
	}
	else if($missionComplete) 
	{
		%time = getSimTime() - $missionStartTime;
		%minutes = Time::getMinutes(%time);
		%seconds = Time::getSeconds(%time);
		if(%minutes < 10)
			%minutes = "0" @ %minutes;
		if(%seconds < 10)
			%seconds = "0" @ %seconds;
			%tstr = %minutes @ ":" @ %seconds;
	}
	else
		%tstr = floor($Server::timeLimit - (getSimTime() - $missionStartTime) / 60);
		

	//if($teamScore[0] > $teamScore[1])
	//	%fteamscore = "<f1>";
//	else if($teamScore[0] == $teamScore[1])
		//%fteamscore = "<f0>";
	//else
		//%fteamscore = "<f4>";
		

	//if($teamScore[1] > $teamScore[0])
		//%steamscore = "<f1>";
	//else if($teamScore[1] == $teamScore[0])
		//%steamscore = "<f0>";
	//else
		//%steamscore = "<f4>";
		
		
		%fteamscore = "<f0>";
		%steamscore = "<f0>";
		
		//%bitmap1 = "capturetheflag1.bmp";
		//%bitmap2 = "capturetheflag2.bmp";
		
		//Team::setObjective(%i,20, "<f4><L60><B0,0:capturetheflag1.bmp><B0,0:capturetheflag2.bmp>");
	
	for(%i = -1; %i < getNumTeams(); %i++) {
		Team::setObjective(%i,0,"<L24><f2>Cap Limit:<f0> " @ $teamScoreLimit @ "<L57><f2>Mission Name:<f0> " @ $missionName @"<L110><f2>Mission Time:<f0> "@ %tstr);
		Team::setObjective(%i,1, "<f2><L35>Team " @ getteamname(0) @ ": " @ %fteamscore @ $teamScore[0] @ "<L64><f4>CTF Statistics <L98><f2>Team " @ getteamname(1) @ ": " @ %steamscore @ $teamScore[1]);
		Team::setObjective(%i,2, "<f3><L12>Player <L36>Score <L46>MA <L52>Caps <L58>C.Kill <L64>Ret <L79>Player <L103>Score <L113>MA <L119>Caps <L121>C.Kill <L131>Ret");
	}

	
	sortscore::sort();

	%lastln = 3;
	
	for(%i = -1; %i < getNumTeams(); %i++){
		for(%j = 0; %j != 10; %j++){

			%cl = $firstteam[%j];
			
			%clname = "-";
			%clscore = "-";
			%clma = "-";
			%clcap = "-";
			%clckill = "-";
			%clret = "-";
			
			if(%cl != "" && %cl > 1 && %j<$fteam){
				%tmp = %cl.score;
   				%inte = floor(%cl.score);
   				%deci = floor(10*(%tmp-%inte));
				%points = %inte @ "." @ %deci;

				%clname = Client::getname(%cl);
				if($TALT::Active == true) //New LT code
					%clscore = %points;
				else
					%clscore = floor(%cl.score);
				%clma = %cl.MidAirs;
				%clcap = %cl.ScoreCaps;
				%clckill = %cl.CapperKills;
				%clret = %cl.FlagReturns;
			}

			%cl2 = $secondteam[%j];
			
			%cl2name = "-";
			%cl2score = "-";
			%cl2ma = "-";
			%cl2cap = "-";
			%cl2ckill = "-";
			%cl2ret = "-";
			
			if(%cl2 != "" && %cl > 1 && %j<$steam){
				%tmp = %cl2.score;
   				%inte = floor(%cl2.score);
   				%deci = floor(10*(%tmp-%inte));
				%points = %inte @ "." @ %deci;
				
				%cl2name = Client::getname(%cl2);
				if($TALT::Active == true) //New LT code
					%cl2score = %points;
				else
					%cl2score = floor(%cl2.score); //%inte = floor(%nadepl.NadeDamage);
				%cl2ma = %cl2.MidAirs;
				%cl2cap = %cl2.ScoreCaps;
				%cl2ckill = %cl2.CapperKills;
				%cl2ret = %cl2.FlagReturns;
			}

			Team::setObjective(%i,%j+6, "<f2><L12> " @ %clname @ "<L36> " @ %clscore @ "<L46> " @ %clma @ "<L54> " @ %clcap @ "<L60> " @ %clckill @ "<L66> " @ %clret @ "<L79> " @ %cl2name @ "<L103> " @ %cl2score @ "<L113> " @ %cl2ma @ "<L121> " @ %cl2cap @ "<L127> " @ %cl2ckill@ "<L133> " @ %cl2ret);
		%lastln++;
		}
	}
	
	//for(%i = -1; %i < getNumTeams(); %i++) {
//
		//for(%j = 0; %j != 1; %j++){
			//%scorepl = $scorelist[%j];
//			
//			%scorestr = "- (-) ";//
//
//			if(%scorepl != "" && %scorepl > 1){
//				%tmp = %scorepl.score;
 //  				%inte = floor(%scorepl.score);
//   				%deci = floor(10*(%tmp-%inte));
//				%points = %inte @ "." @ %deci;
//
//				%scorestr = Client::getname(%scorepl) @ " " @ "(" @ %points @ ")";
//			}
//
//			//Team::setObjective(%i, 19 + %j, "<L24><f2>MVP:<f0> " @ %scorestr @ "<L60><f5>Arena Statistics");
//			
//		}
//	}
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.inArena) //get rid of arena when not in use
		{
			%clarena = %cl;
		}
	}
	
	if(%clarena != 0 || !$TALT::Active)
	{
		for(%i = -1; %i < getNumTeams(); %i++) 
		{
			//Team::setObjective(%i,20, "<f4><L56><B0,0:deathmatch1.bmp><B0,0:deathmatch2.bmp>");
			Team::setObjective(%i,20, "<L64><f4>Arena Statistics");
			Team::setObjective(%i,21, "<f3><L31>Player <L55>Score <L65>Kills <L73>MA <L83>Disc <L91>Plasma <L105>Nade");
		}
	
		%lastlna = 22;
	
		for(%i = -1; %i < getNumTeams(); %i++)
		{
			for(%a = 0; %a != 10; %a++)
			{

				%cla = $arenateam[%a];
			
				%claname = "-";
				%clascore = "-";
				%clama = "-";
				%clakill = "-";
				%DiscDamagepoints = "-";
				%PlasmaDamagepoints = "-";
				%ChainDamagepoints = "-";
				%BlasterDamagepoints = "-";
				%NadeDamagepoints = "-";
			
				if(%cla != "" && %cla > 1 && %a<$ateam)
				{
					%tmp = %cla.score;
   					%inte = floor(%cla.score);
   					%deci = floor(10*(%tmp-%inte));
					%points = %inte @ "." @ %deci;
				
					%tmp = %cla.DiscDamage;
   					%inte = floor(%cla.DiscDamage);
   					%deci = floor(10*(%tmp-%inte));
					%DiscDamagepoints = %inte @ "." @ %deci;
					//chain
					%tmp = %cla.ChainDamage;
   					%inte = floor(%cla.ChainDamage);
   					%deci = floor(10*(%tmp-%inte));
					%ChainDamagepoints = %inte @ "." @ %deci;
					//blaster
					%tmp = %cla.BlasterDamage;
   					%inte = floor(%cla.BlasterDamage);
   					%deci = floor(10*(%tmp-%inte));
					%BlasterDamagepoints = %inte @ "." @ %deci;
					//plasma
					%tmp = %cla.PlasmaDamage;
   					%inte = floor(%cla.PlasmaDamage);
   					%deci = floor(10*(%tmp-%inte));
					%PlasmaDamagepoints = %inte @ "." @ %deci;
					//nade
					%tmp = %cla.NadeDamage;
   					%inte = floor(%cla.NadeDamage);
   					%deci = floor(10*(%tmp-%inte));
					%NadeDamagepoints = %inte @ "." @ %deci;

					%claname = Client::getname(%cla);
					%clascore = %points;
					%clama = %cla.MidAirs;
					%clakill = %cla.scoreKills;
				}

				Team::setObjective(%i,%a+22, "<f2><L31> " @ %claname @ "<L55> " @ %clascore @ "<L65> " @ %clakill @ "<L73> " @ %clama @ "<L83> " @ %DiscDamagepoints @ "<L94> " @ %PlasmaDamagepoints @ "<L107> " @ %NadeDamagepoints);
				%lastlna++;
			}
		}
	}
	else if(%clarena == 0)
	{
	
	for(%i = -1; %i < getNumTeams(); %i++) {
		if($TALT::SpawnType == "AnniSpawn")
		{
			if($TALT::AnniWeapon == "Shotgun")
			{
				Team::setObjective(%i, 20, "<f4><L60>Damage By Weapon");
				Team::setObjective(%i, 21, "<f3>" @ "<L22>Disc" @ "<L52>Plasma" @ "<L82>Shotgun" @ "<L112>Nade");
			}
			else if($TALT::AnniWeapon == "GrenadeLauncher")
			{
				Team::setObjective(%i, 20, "<f4><L60>Damage By Weapon");
				Team::setObjective(%i, 21, "<f3>" @ "<L38>Disc" @ "<L68>Plasma" @ "<L98>Nade");
			}
			else if($TALT::AnniWeapon == "Blaster")
			{
				Team::setObjective(%i, 20, "<f4><L60>Damage By Weapon");
				Team::setObjective(%i, 21, "<f3>" @ "<L22>Disc" @ "<L52>Plasma" @ "<L82>Blaster" @ "<L112>Nade");
			}
				
		}
		else if($TALT::SpawnType == "EliteSpawn")
		{
			Team::setObjective(%i, 20, "<f4><L60>Damage By Weapon");
			Team::setObjective(%i, 21, "<f3>" @ "<L22>Disc" @ "<L52>Chain" @ "<L82>Blaster" @ "<L112>Nade");
		}
		else if($TALT::SpawnType == "BaseSpawn")
		{
			Team::setObjective(%i, 20, "<f4><L60>Damage By Weapon");
			Team::setObjective(%i, 21, "<f3>" @ "<L38Disc" @ "<L68>Chain" @ "<L98>Nade");
		}
		

		for(%j = 0; %j != 10; %j++){
			%clt = $totteamx[%j];
			
			%discpl = $disclist[%j];
			%cgpl = $cglist[%j];
			%plasmapl = $plasmalist[%j];
			%shotgunpl = $shotgunlist[%j];
			%blastpl = $blastlist[%j];
			%nadepl = $nadelist[%j];
			
			%discstr = "- (-) ";
			%cgstr = "- (-) ";
			%plasmastr = "- (-) ";
			%shotgunstr = "- (-) ";
			%blaststr = "- (-) ";
			%nadestr = "- (-) ";
			
			if(%clt != "" && %clt > 1 && %j<$totteam)
			{
			if(%discpl != "" && %discpl > 1){
				%tmp = %discpl.DiscDamage;
   				%inte = floor(%discpl.DiscDamage);
   				%deci = floor(10*(%tmp-%inte));
				%DiscDamagepoints = %inte @ "." @ %deci;
				%discstr = Client::getname(%discpl) @ " " @ "(" @ %DiscDamagepoints @ ")";
			}
			
			if(%cgpl != "" && %cgpl > 1){
				//chain
				%tmp = %cgpl.ChainDamage;
   				%inte = floor(%cgpl.ChainDamage);
   				%deci = floor(10*(%tmp-%inte));
				%ChainDamagepoints = %inte @ "." @ %deci;
				%cgstr = Client::getname(%cgpl) @ " " @ "(" @ %ChainDamagepoints @ ")";
			}
			
			if(%plasmapl != "" && %plasmapl > 1){
				//plasma
				%tmp = %plasmapl.PlasmaDamage;
   				%inte = floor(%plasmapl.PlasmaDamage);
   				%deci = floor(10*(%tmp-%inte));
				%PlasmaDamagepoints = %inte @ "." @ %deci;
				%plasmastr = Client::getname(%plasmapl) @ " " @ "(" @ %PlasmaDamagepoints @ ")";
			}
			if(%shotgunpl != "" && %shotgunpl > 1){
				//shotgun
				%tmp = %shotgunpl.ShotgunDamage;
   				%inte = floor(%shotgunpl.ShotgunDamage);
   				%deci = floor(10*(%tmp-%inte));
				%ShotgunDamagepoints = %inte @ "." @ %deci;
				%shotgunstr = Client::getname(%shotgunpl) @ " " @ "(" @ %ShotgunDamagepoints @ ")";
			}
			
			if(%blastpl != "" && %blastpl > 1){
				//blaster
				%tmp = %blastpl.BlasterDamage;
   				%inte = floor(%blastpl.BlasterDamage);
   				%deci = floor(10*(%tmp-%inte));
				%BlasterDamagepoints = %inte @ "." @ %deci;
				%blaststr = Client::getname(%blastpl) @ " " @ "(" @ %BlasterDamagepoints @ ")";
			}

			if(%nadepl != "" && %nadepl > 1){
				//nade
				%tmp = %nadepl.NadeDamage;
   				%inte = floor(%nadepl.NadeDamage);
   				%deci = floor(10*(%tmp-%inte));
				%NadeDamagepoints = %inte @ "." @ %deci;
				%nadestr = Client::getname(%nadepl) @ " " @ "(" @ %NadeDamagepoints @ ")";
			}
			}
			
			if($TALT::SpawnType == "AnniSpawn")
			{
				if($TALT::AnniWeapon == "Shotgun")
				{
					Team::setObjective(%i, 22 + %j, "<f2>" @ "<L22> " @ %discstr @ "<L52> " @ %plasmastr @ "<L82> " @ %shotgunstr @ "<L112> " @ %nadestr);
				}
				else if($TALT::AnniWeapon == "GrenadeLauncher")
				{
					Team::setObjective(%i, 22 + %j, "<f2>" @ "<L37> " @ %discstr @ "<L67> " @ %plasmastr @ "<L97> " @ %nadestr);
				}
				else if($TALT::AnniWeapon == "Blaster")
				{
					Team::setObjective(%i, 22 + %j, "<f2>" @ "<L22> " @ %discstr @ "<L52> " @ %plasmastr @ "<L82> " @ %blaststr @ "<L112> " @ %nadestr);
				}
			}
			else if($TALT::SpawnType == "EliteSpawn")
				Team::setObjective(%i, 22 + %j, "<f2>" @ "<L22> " @ %discstr @ "<L52> " @ %cgstr @ "<L82> " @ %blaststr @ "<L112> " @ %nadestr);
			else if($TALT::SpawnType == "BaseSpawn")
				Team::setObjective(%i, 22 + %j, "<f2>" @ "<L37> " @ %discstr @ "<L67> " @ %cgstr @ "<L97> " @ %nadestr);
			
		}
	}
	}
		
	schedule("ObjectiveMission::setObjectiveHeading();", $TA::RefreshStatsTime);
	} //end stats
	else if($TA::Stats == false)
	{
	if($missionComplete)
	{
		%curLeader = 0;
		%tieGame = false;
		%tie = 0;
		%tieTeams[%tie] = %curLeader;
		for(%i = 0; %i < getNumTeams()-1; %i++) 
			Anni::Echo("GAME: teamfinalscore " @ %i @ " " @ $teamScore[%i]);
		for(%i = 1; %i < getNumTeams()-1; %i++) 
		{
			if($teamScore[%i] == $teamScore[%curLeader]) 
			{
				%tieGame = true;
				%tieTeams[%tie++] = %i;
			}
			else if($teamScore[%i] > $teamScore[%curLeader])
			{
				%curLeader = %i;
				%tieGame = false;
				%tie = 0;
				%tieTeams[%tie] = %curLeader;
			}
		}
		if(%tieGame) 
		{
			for(%g = 0; %g <= %tie; %g++) 
			{
				%names = %names @ getTeamName(%tieTeams[%g]);
				if(%g == %tie-1)
					%names = %names @ " and ";
				else if(%g != %tie)
					%names = %names @ ", ";
			}
			if(%tie > 1) 
				%names = %names @ " all";
		}
		for(%i = -1; %i < getNumTeams()-1; %i++)
		{
			objective::displayBitmap(%i,0);
			if(!%tieGame) 
			{
				if(%i == %curLeader) 
				{
					if($teamScore[%curLeader] == 1)
						Team::setObjective(%i, 1, "<F5>			  Your team won the mission with " @ $teamScore[%curLeader] @ " point!");
					else
						Team::setObjective(%i, 1, "<F5>			  Your team won the mission with " @ $teamScore[%curLeader] @ " points!");
				}
				else 
				{
					if($teamScore[%curLeader] == 1)
						Team::setObjective(%i, 1, "<F5>	  The " @ getTeamName(%curLeader) @ " team won the mission with " @ $teamScore[%curLeader] @ " point!");
					else
						Team::setObjective(%i, 1, "<F5>	  The " @ getTeamName(%curLeader) @ " team won the mission with " @ $teamScore[%curLeader] @ " points!");
				}
			}	
			else 
			{
				if(getNumTeams()-1 > 2) 
				{
					Team::setObjective(%i, 1, "<F5>	  The " @ %names @ " tied with a score of " @ $teamScore[%curLeader]);
				}
				else
					Team::setObjective(%i, 1, "<F5>	  The mission ended in a tie where each team had a score of " @ $teamScore[%curLeader]);
			}
			Team::setObjective(%i, 2, " ");
		}
	}
	else 
	{
		for(%i = -1; %i < getNumTeams()-1; %i++)
		{
			objective::displayBitmap(%i,0);
			Team::setObjective(%i,1, "<f5>Mission Completion:");
			Team::setObjective(%i, 2,"<f1>	- " @ $teamScoreLimit @ " points needed to win the mission.");
		}
	}
	if(!$Server::timeLimit)
		%str = "<f1>	- No time limit on the game.";
	else if($timeLimitReached)
		%str = "<f1>	- Time limit reached.";
	else if($missionComplete)
	{
		%time = getSimTime() - $missionStartTime;
		%minutes = Time::getMinutes(%time);
		%seconds = Time::getSeconds(%time);
		if(%minutes < 10)
			%minutes = "0" @ %minutes;
		if(%seconds < 10)
			%seconds = "0" @ %seconds;
		%str = "<f1>	- Total match time: " @ %minutes @ ":" @ %seconds;
	}
	else
		%str = "<f1>	- Time remaining: " @ floor($Server::timeLimit - (getSimTime() - $missionStartTime) / 60) @ " minutes.";
	for(%i = -1; %i < getNumTeams()-1; %i++) 
	{
		Team::setObjective(%i, 3, " ");
		Team::setObjective(%i, 4, "<f5>Mission Information:");
		Team::setObjective(%i, 5, "<f1>	- Mission Name: " @ $missionName);
		Team::setObjective(%i, 6, %str);
	}
	} //end normal stats 
}

function sortScore::init(%list, %type){

}


function sortScore::sort()
{

	$ateam = 0;
	$fteam = 0;
	$steam = 0;
	$totteam = 0;

	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(!%cl.inArena && !%cl.inDuel) //New duel code
		{
			%team = Client::getTeam(%cl);
			if(%team != -1)
			{
				if(%team == 0)
				{
					$firstteam[$fteam] = %cl;
					$fteam++;
				}		
				if(%team == 1)
				{
					$secondteam[$steam] = %cl;
					$steam++;
				}
				
				//if(%team != -1)
				//{
					//$totteam = $fteam + $steam;
					$totteamx[$totteam] = %cl;
  					$disclist[$totteam] = %cl;
					$cglist[$totteam] = %cl;
					$blastlist[$totteam] = %cl;
					$shotgunlist[$totteam] = %cl;
					$plasmalist[$totteam] = %cl;
					$nadelist[$totteam] = %cl;
					$totteam++;
				//}
			}
		}
		else if(%cl.inArena)
		{
			$arenateam[$ateam] = %cl;
			$ateam++;
		}
		else if(%cl.inDuel)
		{
			$duelteam[$dteam] = %cl;
			$dteam++;
		}
	}


	for (%i=1; %i < $ateam; %i++)
	{
    		%index = $arenateam[%i];
		%j = %i;
    		while ((%j > 0) && ($arenateam[%j-1].score < %index.score))
    		{
      			$arenateam[%j] = $arenateam[%j-1];
      			%j = %j - 1;
    		}
    		$arenateam[%j] = %index;
  	}
	
	
	for (%i=1; %i < $dteam; %i++)
	{
    		%index = $duelteam[%i];
		%j = %i;
    		while ((%j > 0) && ($duelteam[%j-1].score < %index.score))
    		{
      			$duelteam[%j] = $duelteam[%j-1];
      			%j = %j - 1;
    		}
    		$duelteam[%j] = %index;
  	}

	for (%i=1; %i < $fteam; %i++)
	{
    		%index = $firstteam[%i];
		%j = %i;
    		while ((%j > 0) && ($firstteam[%j-1].score < %index.score))
    		{
      			$firstteam[%j] = $firstteam[%j-1];
      			%j = %j - 1;
    		}
    		$firstteam[%j] = %index;
  	}

	for (%i=1; %i < $steam; %i++)
	{
    		%index = $secondteam[%i];
		%j = %i;
    		while ((%j > 0) && ($secondteam[%j-1].score < %index.score))
    		{
      			$secondteam[%j] = $secondteam[%j-1];
      			%j = %j - 1;
    		}
    		$secondteam[%j] = %index;
  	}
	
	//weapon damage
	for (%i=1; %i < $totteam; %i++)
	{
    		%index = $disclist[%i];
    		%j = %i;
    		while ((%j > 0) && ($disclist[%j-1].DiscDamage < %index.DiscDamage))
    		{
      			$disclist[%j] = $disclist[%j-1];
      			%j = %j - 1;
    		}
    		$disclist[%j] = %index;
  	}

	for (%i=1; %i < $totteam; %i++)
	{
    		%index = $cglist[%i];
    		%j = %i;
    		while ((%j > 0) && ($cglist[%j-1].ChainDamage < %index.ChainDamage))
    		{
      			$cglist[%j] = $cglist[%j-1];
      			%j = %j - 1;
    		}
    		$cglist[%j] = %index;
  	}

	for (%i=1; %i < $totteam; %i++)
	{
    		%index = $plasmalist[%i];
    		%j = %i;
    		while ((%j > 0) && ($plasmalist[%j-1].PlasmaDamage < %index.PlasmaDamage))
    		{
      			$plasmalist[%j] = $plasmalist[%j-1];
      			%j = %j - 1;
    		}
    		$plasmalist[%j] = %index;
  	}

	for (%i=1; %i < $totteam; %i++)
	{
    		%index = $shotgunlist[%i];
    		%j = %i;
    		while ((%j > 0) && ($shotgunlist[%j-1].ShotgunDamage < %index.ShotgunDamage))
    		{
      			$shotgunlist[%j] = $shotgunlist[%j-1];
      			%j = %j - 1;
    		}
    		$shotgunlist[%j] = %index;
  	}

	for (%i=1; %i < $totteam; %i++)
	{
    		%index = $blastlist[%i];
    		%j = %i;
    		while ((%j > 0) && ($blastlist[%j-1].BlasterDamage < %index.BlasterDamage))
    		{
      			$blastlist[%j] = $blastlist[%j-1];
      			%j = %j - 1;
    		}
    		$blastlist[%j] = %index;
  	}

	for (%i=1; %i < $totteam; %i++)
	{
    		%index = $nadelist[%i];
    		%j = %i;
    		while ((%j > 0) && ($nadelist[%j-1].NadeDamage < %index.NadeDamage))
    		{
      			$nadelist[%j] = $nadelist[%j-1];
      			%j = %j - 1;
    		}
    		$nadelist[%j] = %index;
  	}


}

if($FlagHunter::Enabled)
	schedule("exec(flaghunter);",2);
