//------------------------------------//
// FlagHunter.cs							 //
// created by Tinman (Kidney Thief)	//
// -----------------------------------//

exec("game.cs");
exec("HunterRecords.cs");

$TA::RefreshStatsTime = 10;

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //

function FlagHunter::setGlobalDefaults()
{
	$FlagHunter::Enabled = true; 	
	if($ItemFavoritesKey >= "Annihilation_2100")
	{
		%numTeams = getNumTeams()-1;
		echo("found Annihilation 2.1 or newer!");
	}
	else 
		%numTeams = getNumTeams();
		
	if(%numteams <= 1)
		$Server::TeamDamageScale = 1; //team damage must stay on if one team.

	//default ban list
//		$FlagHunter::banList[0, type] = "Mortar";
//		$FlagHunter::banList[0, name] = "Mortar";
//		$FlagHunter::banList[1, type] = "MineAmmo";
//		$FlagHunter::banList[1, name] = "Mines";
//	
//		//default spawn list
//		$FlagHunter::spawnWeapon = "Disclauncher";
//		$FlagHunter::spawnList[0] = "LightArmor";
//		$FlagHunter::spawnList[1] = "Disclauncher";
//		$FlagHunter::spawnList[2] = "RepairKit";

	//Game globals
	$FlagHunter::MostFlagsReturned = "";
	$FlagHunter::MostFlagsReturnCount = 0;
	$FlagHunter::MostFlagsDropped = 0;
	$FlagHunter::MostFlagsDroppedName = "";
	$FlagHunter::AltarCampingTimer = 10;
	$FlagHunter::FlagFadeTime = 120;
	$FlagHunter::CarryingNumber = 1;
	$FlagHunter::YardSaleNumber = 10;
	$FlagHunter::YardSaleTime = 30;
	$FlagHunter::GreedAmount = 5;
	$FlagHunter::HoardStartTime = 5;
	$FlagHunter::HoardEndTime = 2;
	
	if(!CountFlags())
		FlagHunter::FlagHunterMap();
	else
		echo("Flags found in map, loading CTF hunter defaults.");
	
}

function CountFlags()
{
	%group = nameToID("MissionCleanup/ObjectivesSet");
	//messageall(1,"returning flags");
	for(%i = 0; (%obj = Group::getObject(%group, %i)) != -1; %i++)
	{
		%name = Item::getItemData(%obj);
		if(%name == flag)
		{
			%count++;
		}
	}
	echo(%count@" Flags in map");
	return %count;	
}


//call it initially
schedule("FlagHunter::setGlobalDefaults();",4);	

function Player::onKilled(%this)
{

	Flag::onDrop(%this, unused);
	
	%this.Station = "";
	%cl = GameBase::getOwnerClient(%this);
	%cl.dead = 1;
	if($AutoRespawn > 0)
		schedule("Game::autoRespawn(" @ %cl @ ");",$AutoRespawn,%cl);
	if(%this.outArea==1)	
		leaveMissionAreaDamage(%cl);

	%type = Player::getMountedItem(%this,$FlagSlot);
	
	if(%type == flag)
	{
		
		Player::dropItem(%cl,"Flag");
	}




	for(%i = 0; %i < 8; %i = %i + 1)
	{
		%type = Player::getMountedItem(%this,%i);
		if(%type != -1)
		{
			if(%i != $WeaponSlot || !Player::isTriggered(%this,%i) || getRandom() > "0.2") 
				Player::dropItem(%this,%type);
		}
	}

	if(%cl != -1)
	{
		if(%this.vehicle != "")	
		{
			if(%this.driver != "") 
			{
				%this.driver = "";
				%this.vehicle.Pilot = "";
				Client::setControlObject(Player::getClient(%this), %this);
				Player::setMountObject(%this, -1, 0);
			}
			else 
			{
				%this.vehicle.Seat[%this.vehicleSlot-2] = "";
				%this.vehicleSlot = "";
			}
			%this.vehicle = "";
		}
		schedule("GameBase::startFadeOut(" @ %this @ ");", $CorpseTimeoutValue, %this);
		Client::setOwnedObject(%cl, -1);
		Client::setControlObject(%cl, Client::getObserverCamera(%cl));
		Observer::setOrbitObject(%cl, %this, 5, 5, 5);
		schedule("deleteObject(" @ %this @ ");", $CorpseTimeoutValue + 2.5, %this);
		%cl.observerMode = "dead";
		%cl.dieTime = getSimTime();
	}
}


function Game::clientKilled(%playerId, %killerId)
{
	Game::refreshClientScore(%playerId);
	DM::checkMissionObjectives(%killerId);	
}


function Game::playerSpawned(%pl, %clientId, %armor)
{

	schedule("Client::clearItemShopping("@%clientId@");",1);
	
	if($Annihilation::UsePersonalSkin)
		Client::setSkin(%clientId, $Client::info[%clientId, 0]);
	else Client::setSkin(%clientId, $Server::teamSkin[Client::getTeam(%clientId)]);
	
	client::setSkin(%clientId, "green");	
	
	Annihilation::setItemCount(%clientId, iarmorWarrior, 1);	
	Annihilation::setItemCount(%clientId, DiscLauncher, 1);
	Annihilation::setItemCount(%clientId, PlasmaGun, 1);
	Annihilation::setItemCount(%clientId, Shotgun, 1);
	Annihilation::setItemCount(%clientId, TargetingLaser, 1);
		
	Annihilation::setItemCount(%clientId, discammo, 30);			
	Annihilation::setItemCount(%clientId, PlasmaAmmo, 30);	  
	Annihilation::setItemCount(%clientId, ShotgunShells, 30);
		
	Annihilation::setItemCount(%clientId, RepairKit, 1);
	Annihilation::setItemCount(%clientId, Beacon, 5);
	Annihilation::setItemCount(%clientId, Grenade, 8);		
	Annihilation::setItemCount(%clientId, MineAmmo, 3);
				
	Annihilation::setItemCount(%clientId, EnergyPack, 1);
	Player::useItem(%pl,EnergyPack);		
	Player::useItem(%pl,PlasmaGun);
	

		Annihilation::setItemCount(%clientId, Flag, 1);
		Player::mountItem(%clientId, Flag, $FlagSlot, 1);
		%clientId.flagCount = 1;
		%clientId.inAltarArea = false;	
}


function Game::checkTimeLimit()
{
	// if no timeLimit set or timeLimit set to 0,
	// just reschedule the check for a minute hence
	$timeLimitReached = false;
	if(!$Server::timeLimit)
	{
		schedule("Game::checkTimeLimit();", 60);
		return;
	}
	%curTimeLeft = ($Server::timeLimit * 60) + $missionStartTime - getSimTime();
	if (%curTimeLeft <= 0)
	{
		$timeLimitReached = true;
		$timeReached = true;
		DM::missionObjectives();
		FlagHunter::restoreServerDefaults();
		Server::nextMission();
	}
	else
	{
		schedule("Game::checkTimeLimit();", 1);
		UpdateClientTimes(%curTimeLeft);
		DM::missionObjectives();
		
		
		if ($FlagHunter::HoardMode)
		{
			if ((%curTimeLeft < ($FlagHunter::HoardStartTime * 60)) && (%curTimeLeft >= ($FlagHunter::HoardEndTime * 60)))
			{
				%hoardSecondsLeft = floor(%curTimeLeft - ($FlagHunter::HoardEndTime * 60));
				%hoardMinutesLeft = floor(%hoardSecondsLeft / 60);
				if (((%hoardSecondsLeft / 60) == %hoardMinutesLeft) && (%hoardMinutesLeft > 1))
					MessageAll(1, %hoardMinutesLeft @ " minutes left until the HOARD period is over.~wmine_act.wav");
				else if (%hoardSecondsLeft == 60)
					MessageAll(1, "1 minute left until the HOARD period is over.~wmine_act.wav");
				else if (%hoardSecondsLeft == 30)
					MessageAll(1, "30 seconds left until the HOARD period is over.~wmine_act.wav");
				else if (%hoardSecondsLeft == 10)
					MessageAll(1, "10 seconds left until the HOARD period is over.~wmine_act.wav");
				else if (%hoardSecondsLeft == 5)
					MessageAll(1, "5 seconds left until the HOARD period is over.~wmine_act.wav");
				else if (%hoardSecondsLeft == 0)
					MessageAll(1, "The HOARD period is over - return your flags now!~wmine_act.wav");
			}
			else
			{
				%timeUntilHoard = %curTimeLeft - ($FlagHunter::HoardStartTime * 60);
				if (%timeUntilHoard >= 0)
				{
					if (%timeUntilHoard == 60)
						MessageAll(1, "The HOARD period begins in 1 minute.~wmine_act.wav");
					else if (%timeUntilHoard == 30)
						MessageAll(1, "The HOARD period begins in 30 seconds.~wmine_act.wav");
					else if (%timeUntilHoard == 10)
						MessageAll(1, "The HOARD period begins in 10 seconds.~wmine_act.wav");
					else if (%timeUntilHoard == 5)
						MessageAll(1, "The HOARD period begins in 5 seconds.~wmine_act.wav");
					else if (%timeUntilHoard == 0)
						MessageAll(1, "Let the HOARDING begin!~wmine_act.wav");
				}
			}
		}
	}
}

function DM::checkMissionObjectives()
{
	if(DM::missionObjectives()) 
		schedule("nextMission();", 0);
}

function DM::missionObjectives()
{
	%numClients = getNumClients();
	for(%i = 0 ; %i < %numClients ; %i++) 
		%clientList[%i] = getClientByIndex(%i);
	%doIt = 1;
	while(%doIt == 1)
	{
		%doIt = "";
		for(%i= 0 ; %i < %numClients; %i++)
		{
			if((%clientList[%i]).score < (%clientList[%i+1]).score)
			{
				%hold = %clientList[%i];
				%clientList[%i] = %clientList[%i+1];
				%clientList[%i+1]= %hold;
				%doIt=1;
			}
		}
	}
	if(!$Server::timeLimit)
		%str = "<f1>	- No time limit on the game.";
	else if($timeLimitReached)
		%str = "<f1>	- Time limit reached.";
	else
		%str = "<f1>	- Time remaining: " @ floor($Server::timeLimit - (getSimTime() - $missionStartTime) / 60) @ " minutes.";
	for(%l = -1; %l < 2 ; %l++)
	{		
		%lineNum = 0;
		if(! $timeReached)
		{
			Team::setObjective(%l, %lineNum, "<jc><B0,0:deathmatch1.bmp><B0,0:deathmatch2.bmp>");
			Team::setObjective(%l, %lineNum++, "<f5>Mission Information:");
			Team::setObjective(%l, %lineNum++, "<f1>	- Mission Name: " @ $missionName);
			Team::setObjective(%l, %lineNum++, %str);
			
			//greed mode
			if ($FlagHunter::GreedMode && ($FlagHunter::GreedAmount >= 2))
			{
				Team::setObjective(%l, %lineNum++, "<f1>	- GREED mode is ON!  You must have at least " @ $FlagHunter::GreedAmount @ " flags");
				Team::setObjective(%l, %lineNum++, "<f1>	  before you can return them to the Nexus.");
			}
			else
			{
				Team::setObjective(%l, %lineNum++, "<f1>	- GREED mode is Off.");
			}
			
			//hoard mode
			if ($FlagHunter::HoardMode)
			{
				%curTimeLeft = ($Server::timeLimit * 60) + $missionStartTime - getSimTime();
				if ((%curTimeLeft <= ($FlagHunter::HoardStartTime * 60)) && (%curTimeLeft > ($FlagHunter::HoardEndTime * 60)))
				{
					%hoardTimeLeft = %curTimeLeft - ($FlagHunter::HoardEndTime * 60);
					%hoardMinutesLeft = floor(%hoardTimeLeft / 60);
					%hoardSecondsLeft = floor(%hoardTimeLeft - (%hoardMinutesLeft * 60));
					if (%hoardMinutesLeft == 0)
					{
						if (%hoardSecondsLeft == 1)
							%timeString = "1 second";
						else
							%timeString = %hoardSecondsLeft @ " seconds";
					}
					else
					{
						if (%hoardMinutesLeft == 1)
							%timeString = "1 minute";
						else
							%timeString = %hoardMinutesLeft @ " minutes";
						if (%hoardSecondsLeft > 0)
						{
							if (%hoardSecondsLeft == 1)
								%timeString = %timeString @ " and 1 second";
							else
								%timeString = %timeString @ " and " @ %hoardSecondsLeft @ " seconds";
						}
					}
					Team::setObjective(%l, %lineNum++, "<f1>	- HOARD mode is ON!  You will not be able to return");
					Team::setObjective(%l, %lineNum++, "<f1>	  any flags to the nexus for " @ %timeString @ ".");
				}
				else
				{
					%timeUntilHoard = %curTimeLeft - ($FlagHunter::HoardStartTime * 60);
					if (%timeUntilHoard > 0)
					{
						%hoardMinutesLeft = floor(%timeUntilHoard / 60);
						if (%hoardMinutesLeft == 0)
						{
							%hoardSecondsLeft = floor(%timeUntilHoard);
							if (%hoardSecondsLeft == 1)
								%timeString = "1 second";
							else
								%timeString = %hoardSecondsLeft @ " seconds";
						}
						else
						{
							if (%hoardMinutesLeft == 1)
								%timeString = "approximately 1 minute";
							else
								%timeString = "approximately " @ %hoardMinutesLeft @ " minutes";
						}
						Team::setObjective(%l, %lineNum++, "<f1>	- HOARD mode is ON!  In " @ %timeString @ " you will");
						Team::setObjective(%l, %lineNum++, "<f1>	  not be able to return any flags to the nexus.");
					}
				}
			}
			else
			{
				Team::setObjective(%l, %lineNum++, "<f1>	- HOARD mode is Off.");
			}
			
			Team::setObjective(%l, %lineNum++, " ");
			Team::setObjective(%l, %lineNum++, "<f5>Mission Objectives:");
			Team::setObjective(%l, %lineNum++, "<f1>  Kill everyone!  Each person you kill, grab the flag(s) they drop.");
			Team::setObjective(%l, %lineNum++, "<f1>	- Return the flags you've captured to the Nexus.");
			Team::setObjective(%l, %lineNum++, "<f1>	- Scoring is progressive:  1 flag  = 1 point,");
			Team::setObjective(%l, %lineNum++, "<f1>										2 flags = 1 + 2 = 3 points,");
			Team::setObjective(%l, %lineNum++, "<f1>										3 flags = 1 + 2 + 3 = 6 points, etc...");
			Team::setObjective(%l, %lineNum++, "<f1>	- If you camp near the Nexus, you will take damage!");
			Team::setObjective(%l, %lineNum++, "<f1>	- If GREED mode is on, you will need " @ $FlagHunter::GreedAmount @ " flags before you can");
			Team::setObjective(%l, %lineNum++, "<f1>	  return them to the Nexus.");
			if ($FlagHunter::HoardEndTime == 1)
			{
				Team::setObjective(%l, %lineNum++, "<f1>	- If HOARD mode is on, you will not be able to return any flags");
				Team::setObjective(%l, %lineNum++, "<f1>	  from " @ $FlagHunter::HoardStartTime @ " minutes until 1 minute left in the game.");
			}
			else
			{
				Team::setObjective(%l, %lineNum++, "<f1>	- If HOARD mode is on, you will not be able to return any flags");
				Team::setObjective(%l, %lineNum++, "<f1>	  from " @ $FlagHunter::HoardStartTime @ " minutes until " @ $FlagHunter::HoardEndTime @ " minutes left in the game.");
			}

			
			Team::setObjective(%l, %lineNum++, " ");
			Team::setObjective(%l, %lineNum++, "<f5>Additional info:" );
			Team::setObjective(%l, %lineNum++, "<f1>	- The Nexus is marked on your commander screen.");
			Team::setObjective(%l, %lineNum++, "<f1>	- Flags fade out after " @ $FlagHunter::FlagFadeTime @ " seconds.");
			Team::setObjective(%l, %lineNum++, "<f1>	- Only players carrying 5 or more flags have a flag on their back.");
			Team::setObjective(%l, %lineNum++, "<f1>	- The TAB menu lists how many flags each player is carrying.");
			Team::setObjective(%l, %lineNum++, "<f1>	- You can find and track the person currently carrying the most flags:");
			Team::setObjective(%l, %lineNum++, "<f1>	  bind a key in the ActionMap to: \"remoteEval(2048, HuntersFindTarget);\"");
		}
		else
		{
			Team::setObjective(%l, %lineNum++, "<f1>	  Highest Flag return count held by: " );
			if ($FlagHunter::MostFlagsReturnCount == 0)
				Team::setObjective(%l, %lineNum++, "<L14><f5><Bskull_big.bmp>\n" @ Client::getName(%clientList[0]) @ "<f5> with a return count of " @ $FlagHunter::MostFlagsReturnCount @ "!");
			else
				Team::setObjective(%l, %lineNum++, "<L14><f5><Bskull_big.bmp>\n" @ $FlagHunter::MostFlagsReturned @ "<f5> with a return count of " @ $FlagHunter::MostFlagsReturnCount @ "!");
			Team::setObjective(%l, %lineNum++, "<f1>	  Highest Score: " );
			Team::setObjective(%l, %lineNum++, "<L14><f5><Bskull_big.bmp>\n" @ Client::getName(%clientList[0]) @ " with a score of " @ (%clientList[0].score) @ "!");
			if (($FlagHunter::MostFlagsEverCount[$missionName] > 0) && ($FlagHunter::MostFlagsEver[$missionName] != ""))
			{
				Team::setObjective(%l, %lineNum++, "<f4>	  Record Holder for this mission: " );
				Team::setObjective(%l, %lineNum++, "<L14><f5><Bflag_enemycaptured.bmp>\n" @ $FlagHunter::MostFlagsEver[$missionName] @ "<f5> with a return count of " @ $FlagHunter::MostFlagsEverCount[$missionName] @ "!");
			}
			if (($FlagHunter::MostFlagsDropped > 0) && ($FlagHunter::MostFlagsDroppedName != ""))
			{
				Team::setObjective(%l, %lineNum++, "<f1>	  Honorary Greed award for this mission: " );
				Team::setObjective(%l, %lineNum++, "<f1>							" @ $FlagHunter::MostFlagsDroppedName @ " who dropped " @ $FlagHunter::MostFlagsDropped @ "!");
			}
			Team::setObjective(%l, %lineNum++, " " );
			Team::setObjective(%l, %lineNum++, "<f1>Player Name<L30>Score");
			%i=0;
			while(%i < %numClients){
				%plyr = %clientList[%i];
				Team::setObjective(%l, %lineNum++, "<f1> - " @ Client::getName(%plyr) @ "<L32>" @ %plyr.score);
				echo(Client::getName(%plyr) @ "  " @ %plyr.score);
				%i++;
			}
			
			//this is really hacky, but it's seems to be the only guaranteed place to restore the server defaults
			FlagHunter::restoreServerDefaults();
		}
	
	  
		for(%s = %lineNum+1; %s < 30 ;%s++)
			Team::setObjective(%l, %s, " ");
	}
	$timeReached = false;
}

//	function Game::refreshClientScore(%clientId)
//	{	
//		DM::missionObjectives();
//	}

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //


function Game::refreshClientScore(%clientId)
{
	%team = Client::getTeam(%clientId);

	if(%team == -1) // observers go last.
		%team = 9;

	Client::setScore(%clientId, "%n\t%t\t  " @ %clientId.score  @ "\t%p\t%l", %clientId.score + (9 - %team) * 10000);
	
//	Client::setScore(%clientId, "%n\t%t\t " @ %clientId.score, %clientId.score);
}

function Mission::init()
{
	//set the TAB menu format
//	setClientScoreHeading("Player Name\t\x65Score\t\x90Carrying\t\xCFPing\t\xEFPL");

	$firstTeamLine = 7;
	$firstObjectiveLine = $firstTeamLine + getNumTeams() + 1;
	for(%i = -1; %i < getNumTeams(); %i++)
	{
		$teamFlagStand[%i] = "";
		$teamFlag[%i] = "";
		Team::setObjective(%i, $firstTeamLine - 1, " ");
		Team::setObjective(%i, $firstObjectiveLine - 1, " ");
		Team::setObjective(%i, $firstObjectiveLine, "<f5>Mission Objectives: ");
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
	
	// Group::iterateRecursive(MissionGroup, ObjectiveMission::initCheck);
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

	if($TestMissionType == "") {
		if($NumTowerSwitchs) 
			$TestMissionType = "C&H";
		else 
			$TestMissionType = "NONE";		
		$NumTowerSwitchs = "";
	}
	
	DM::missionObjectives();
	

	if ($FlagHunter::HoardEndTime < 1)
		$FlagHunter::HoardEndTime = 1;
	if ($FlagHunter::HoardStartTime - $FlagHunter::HoardEndTime < 1)
	{
		//reset the start and end times
		$FlagHunter::HoardStartTime = 5;
		$FlagHunter::HoardEndTime = 2;
		$FlagHunter::HoardMode = false;
	}
	
	%numClients = getNumClients();
	for (%i = 0; %i < %numclients; %i++)
	{ 
		%client = getClientByIndex(%i);
		setCommandStatus(%client, 0, "");
		%client.target = -1;
	}
}

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
//FLAG functions

function fadeOutObject(%object)
{
	GameBase::startFadeOut(%object);
	schedule("deleteObject(" @ %object @ ");", 2.5, %object);
}

function AFlag::leaveMissionArea(%this)
{
	%vel = Item::getVelocity(%this);
	%newVel = Vector::neg(%vel);
	Item::setVelocity(%this, %newVel);
}

function Flag::onDrop(%player, %type)
{
	%client = Player::getClient(%player);
	%numFlagsDropped = %client.flagCount;
	
	if (%numFlagsDropped > $FlagHunter::YardSaleNumber)
	{
		%numberSinglePointFlags = $FlagHunter::YardSaleNumber;
		%excess = %numFlagsDropped - $FlagHunter::YardSaleNumber;
		if (%excess % 2 == 1)
		{
			%numberSinglePointFlags++;
			%excess--;
		}
		%numberToSpawn = %numberSinglePointFlags + floor(%excess / 2);
	}
	else
	{
		%numberToSpawn = %numFlagsDropped;
		%numberSinglePointFlags = %numFlagsDropped;
	}
	//messageall(1,"drop "@%type@", "@%numberToSpawn);
	for (%i = 0; %i < %numberToSpawn; %i++)
	{
		// create a flag
		%flag = newObject("", Item, Flag, 1, false, false, true);
 	 	addToSet("MissionCleanup", %flag);
		
		if (%i < %numberSinglePointFlags)
			%flag.value = 1;
		else
			%flag.value = 2;
			 
		%flag.carrier = -1;
		
		GameBase::setTeam(%flag, -1);	
		GameBase::throw(%flag, %player, 10, false);
		
		//if the flag hasn't been picked up in 2 minutes or so, fade it out
		schedule("fadeOutObject(" @ %flag @ ");", $FlagHunter::FlagFadeTime, %flag);
		
		//randomize the direction a bit so the flags don't all bunch up
		%curVelocity = Item::getVelocity(%flag);
		%velX = getWord(%curVelocity, 0) + floor(getRandom() * 20) - 10;
		%velY = getWord(%curVelocity, 1) + floor(getRandom() * 20) - 10;
		%velZ = getWord(%curVelocity, 2) + floor(getRandom() * 20) - 10;
		Item::setVelocity(%flag, %velX @ " " @ %velY @ " " @ %velZ);

		if(%i == 0)
		{
			%flag.LastClient = %client;
			%flag.LastPlayer = %player;
			
		}		
		
	}
		
	//remove the flag from the player	
	Annihilation::setItemCount(%client, "Flag", 0);
	%client.carryFlag = "";
	%client.flagCount = 0;
	Game::refreshClientScore(%client);
	
	//update anyone who is tracking this player
	%name = Client::getName(%client);
	%numClients = getNumClients();
	for (%i = 0; %i < %numclients; %i++)
	{ 
		%tracker = getClientByIndex(%i);  
		if (%tracker != %client && %tracker.target == %client)
		{
			IssueTargCommand(%tracker, %tracker, 0, %name @ " dropped " @ %numFlagsDropped @ " flags!", %client - 2048);
		}
	}
	
	//find the location and advertise a "yard sale" if enough flags were dropped
	if (%numFlagsDropped >= $FlagHunter::YardSaleNumber)
	{
		MessageAll(1, "YARD SALE!!!~wfemale5.wtaunt4.wav");
		%beacon = newObject("", Item, Beacon, 1, false, false, true);
 	 	addToSet("MissionCleanup", %beacon);
		GameBase::setTeam(%beacon, 0);
		GameBase::throw(%beacon, %player, 10, false);
		schedule("StartYardSaleBeacon(" @ %beacon @ ");", 0.5, %beacon);
	}
	
	if (%numFlagsDropped - 1 > $FlagHunter::MostFlagsDropped)
	{
		$FlagHunter::MostFlagsDropped = %numFlagsDropped - 1;
		$FlagHunter::MostFlagsDroppedName = Client::getName(%client);
	}
}

function StartYardSaleBeacon(%beacon)
{
	if (! GameBase::isAtRest(%beacon))
		schedule("StartYardSaleBeacon(" @ %beacon @ ");", 0.5, %beacon);
	else
	{
		//sink the beacon 1 meter below the surface...
		%pos = GameBase::getPosition(%beacon); 
		%posX = getWord(%pos, 0);
		%posY = getWord(%pos, 1);
		%posZ = getWord(%pos, 2) - 1.0;
		%newPos = %posX @ " " @ %posY @ " " @ %posZ;
			
		//delete the thrown beacon object
		deleteObject(%beacon);
		
		//create a deployed targetting one
		%targBeacon = newObject("Target Beacon", "StaticShape", "DefaultBeacon", true);
		addToSet("MissionCleanup", %targBeacon);
		GameBase::setTeam(%targBeacon, 0);
		GameBase::setPosition(%targBeacon, %newPos);
		Gamebase::setMapName(%targBeacon,"Yard Sale!");
		Beacon::onEnabled(%targBeacon);
		
		//schedule the beacon to fade in 30 seconds
		schedule("fadeOutObject(" @ %targBeacon @ ");", $FlagHunter::YardSaleTime, %targBeacon);
	}
}


function Flag::onCollision(%this, %object)
{
	if (getObjectType(%object) != "Player")
		return;
	if(%object.testing)	
		return;
	if (%this.carrier != -1)
		return; // spurious collision
		
	%name = Item::getItemData(%this);
	//messageall(1,"collision "@%name);	
	%playerTeam = GameBase::getTeam(%object);
	%flagTeam = GameBase::getTeam(%this);
	%playerClient = Player::getClient(%object);
	%touchClientName = Client::getName(%playerClient);

	// return wayward flags, and let everyone know whats going on..	
	if(%flagTeam != -1)
	{
		//let players know whats going on here.
		if(%playerClient.flagCount < 2 || %flagTeam == %playerTeam)
		{	
			if((getSimTime() - %this.LastMessage > 15.0 && %this.LastTouch != %object) || !%this.LastTouch)
			{
				%this.LastTouch = %playerClient;
				%this.LastMessage = getSimTime();
				messageall(1,"Welcome to Flaghunter Mode! Collect dead players flags, and take them to the enemy flag for points.");
			}
			else
			{
				Client::sendMessage(%playerClient,1,"Welcome to Flaghunter Mode! Collect dead players flags, and take them to the enemy flag for points.");	
			}
		}
				
		if(%flagTeam == %playerTeam)
		{
			// player is touching his own flag...
			if(!%this.atHome)
			{
				// the flag isn't home! so return it.
				GameBase::startFadeOut(%this);
				GameBase::setPosition(%this, %this.originalPosition);
				GameBase::setIsTarget(%this,false);	
				Item::setVelocity(%this, "0 0 0");
				GameBase::startFadeIn(%this);
				%this.atHome = true;
				MessageAllExcept(%playerClient, 0, %touchClientName @ " returned the " @ getTeamName(%playerTeam) @ " flag!~wflagreturn.wav");
				Client::sendMessage(%playerClient, 0, "You returned the " @ getTeamName(%playerTeam) @ " flag!~wflagreturn.wav");
				teamMessages(1, %playerTeam, "Your flag was returned to base.", -2, "", "The " @ getTeamName(%playerTeam) @ " flag was returned to base.");
				%this.pickupSequence++;
				ObjectiveMission::ObjectiveChanged(%this);
			}
		}
		else if(%flagTeam != %playerTeam)
		{
			%this.nexus = true;
			FlagHunter::EnterNexus(%this, %object);	
			schedule(%playerClient@".inAltarArea = false;",4);
			%messaged = true;
		}
		
		
		
		return;		
	}
	else
	{
		if(%this.LastPlayer != -1 && %this.LastPlayer != %object)
		{
			if(%this.LastClient == %PlayerClient)
			{	
				GameBase::playSound(%playerClient, SoundShockNade, 0);	
				deleteObject(%this);
				return;
			}
		
		}
			
		

		%playerClient.flagCount += %this.value;
		
		//set the state, and turn it invisble - the scheduled callback for this flag will delete it
		deleteObject(%this);
		
		%numFlags = %playerClient.flagCount - 1;
		
		//only send messages to everyone if the number is odd - to cut down on spam...
		%currentTime = getSimTime();
		if ((%currentTime - %playerClient.lastMessageTime < 1.0) && ( floor(%numFlags / 2.0) == (%numFlags / 2.0)))
		{
			%sendMsg = false;
		}
		else
		{
			%sendMsg = true;
			%playerClient.lastMessageTime = %currentTime;
		}
		
		if (%numFlags == 1)
		{
			Client::sendMessage(%playerClient, 0, "You now have " @ %numFlags @ " flag.~wflag1.wav");
			if (%sendMsg)
				MessageAllExcept(%playerClient, 0, %touchClientName @ " now has " @ %numFlags @ " flag.~wflag1.wav");
		}
		else if (%numFlags >= 2)
		{
			Client::sendMessage(%playerClient, 0, "You now have " @ %numFlags @ " flags!~wflag1.wav");
			if (%sendMsg)
				MessageAllExcept(%playerClient, 0, %touchClientName @ " now has " @ %numFlags @ " flags!  Kill him!~wflag1.wav");
		}
	
		//make sure he's still carrying a flag
		if (%playerClient.flagCount >= $FlagHunter::CarryingNumber && Player::getItemCount(%playerClient, Flag) < 1)
		{
			Annihilation::setItemCount(%playerClient, Flag, 1);
			Player::mountItem(%playerClient, Flag, $FlagSlot, 1);
		}
		Game::refreshClientScore(%playerClient);
		
		//update anyone who is tracking this player
		%name = Client::getName(%playerClient);
		%numClients = getNumClients();
		for (%i = 0; %i < %numclients; %i++)
		{ 
			%client = getClientByIndex(%i);  
			if (%client != %playerClient && %client.target == %playerClient)
			{
				IssueTargCommand(%client, %client, 0, "Kill " @ %name @ " and grab " @ %playerClient.flagCount @ " flags!", %playerClient - 2048);
			}
		}
	}
}

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
//brought the flags to the Nexus

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
//	//Player has a total of 10 seconds per life allowed outside designated mission area.
//	//After a player expends this 10 sec, the player is remotely killed.
//	function Player::leaveMissionArea(%player){
//		%cl = Player::getClient(%player);
//		Client::sendMessage(%cl,1,"You have left the mission area.");
//		%player.outArea=1;
//		alertPlayer(%player, 3);
//	}
//	
//	//checking for timeout of dieSeqCount
//	function Player::checkLMATimeout(%player, %seqCount)
//	{
//		echo("checking player timeout " @ %player @ " " @ %seqCount);
//		if(%player.dieSeqCount == %seqCount)
//			remoteKill(Player::getClient(%player));
//	}
//	
//	
//	function Player::enterMissionArea(%player)
//	{
//		%player.outArea="";
//		%player.dieSeqCount = 0;
//		%player.timeLeft = %player.timeLeft - (getSimTime() - %player.leaveTime);
//	}
//	
//	  
//	function alertPlayer(%player, %count){
//		if(%player.outArea == 1){
//			%clientId = Player::getClient(%player);
//			Client::sendMessage(%clientId,1,"~wLeftMissionArea.wav");
//	
//			if(%count > 1)
//				schedule("alertPlayer(" @ %player @ ", " @ %count - 1 @ ");",1.5,%clientId);
//			else 
//				schedule("leaveMissionAreaDamage(" @ %clientId @ ");",1,%clientId);
//		}
//	}
//	
//	function leaveMissionAreaDamage(%client){
//		%player = Client::getOwnedObject(%client);
//	
//		if(%player.outArea == 1){
//			if(!Player::isDead(%player)){
//				Player::setDamageFlash(%client,0.1);
//				GameBase::setDamageLevel(%player,GameBase::getDamageLevel(%player) + 0.05);
//		  schedule("leaveMissionAreaDamage(" @ %client @ ");",1);
//			}
//			else { 
//				playNextAnim(%client);	
//				Client::onKilled(%client, %client);
//			}
//		}
//	}
//

function Player::leaveMissionArea(%this)
{
	%this.outArea=1;
	Client::sendMessage(Player::getClient(%this),1,"You have left the mission area.");
	alertPlayer(%this, 3);
}
function Player::enterMissionArea(%this)
{
	%set = nameToID("MissionCleanup/ObjectivesSet");
	%this.outArea = "";
	Client::sendMessage(Player::getClient(%this),1,"You have entered the mission area.");
	for(%i = 0; (%obj = Group::getObject(%set, %i)) != -1; %i++)
		GameBase::virtual(%obj, "playerEnterMissionArea", %this);
}
	
function Vector::Multiply(%vecA, %vecB)
{
	return getWord(%vecA, 0)*getWord(%vecB, 0)@" "@getWord(%vecA, 1)*getWord(%vecB, 1)@" "@getWord(%vecA, 2)*getWord(%vecB, 2);
} 
 
function bouncePlayerBack(%player) //= LOL!
{
	//%rand = getRandom() * 50;

	if(%player.driver == "1") //== LOL
	{
	//	%vehicle = Player::getMountObject(%player);
	//	%rot = GameBase::getRotation(%vehicle);
	//	%newRot = Vector::Multiply(%rot, "-0.5 0.5 -0.5");
	//	GameBase::setRotation(%vehicle, %newRot);
		Player::setDamageFlash(%player, 0.5);
	}
	else
	{
		%vel = Item::getVelocity(%player);
		%newVel = Vector::neg(%vel);
		Item::setVelocity(%player, %newVel);
	//	%center = waypointtoworld(200,200) @ " 0";
	//	%velocity = vector::getdistance("0 0 0",%vel);
	//	%vec = Vector::getFromRot(%rot,%len*%mass*%out,%len*%mass*%up);
	//	Player::applyImpulse(%obj,%vec);	
	
		//Player::setDamageFlash(%player, Vector::AvgXYZ(%vel)/50);
	}
}
   
function alertPlayer(%player, %count)
{
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
		%clientId = Player::getClient(%player);
		Client::sendMessage(%clientId,1,"~wLeftMissionArea.wav");
		if(%count > 1)
			schedule("alertPlayer(" @ %player @ ", " @ %count - 1 @ ");",1.5,%clientId);
		else 
			schedule("leaveMissionAreaDamage(" @ %clientId @ ");",1,%clientId);
			bouncePlayerBack(%player);
	}	
}

function leaveMissionAreaDamage(%client)
{
	%player = Client::getOwnedObject(%client);
	if(%player.outArea == 1) 
	{
		if(!Player::isDead(%player)) 
		{
			if(%player.driver == "1")
			{
				Client::sendMessage(%client,1,"~wLeftMissionArea.wav");			
				%vehicle = Player::getMountObject(%player);
				GameBase::setDamageLevel(%vehicle,GameBase::getDamageLevel(%vehicle) + 0.15);
			}	
			Player::setDamageFlash(%client,0.1);
			GameBase::setDamageLevel(%player,GameBase::getDamageLevel(%player) + 0.05);
			schedule("leaveMissionAreaDamage(" @ %client @ ");",1);
		}
		else 
		{ 
			playNextAnim(%client);
			Client::onKilled(%client, %client);
		}
	}
}



function FlagHunter::FlagHunterMap()
{
	function GroupTrigger::onEnter(%this, %object)
	{
		FlagHunter::EnterNexus(%this, %object);
	}
	echo("No flags in mission, loading Hunter defaults.");
}


// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
// client cannot camp near the flag
function FlagHunter::EnterNexus(%this, %object)
{
	if (getObjectType(%object) != "Player")
		return;
		
	%client = Player::getClient(%object);
	
	//compatable with annihilation teleporters
	if(%this.TeleTrigger == true)
	{
		TeleTrigger::onTrigEnter(%this, %object);
		//echo("is teleporter");
	}	
	else if (%this.nexus)
	{
		%totalFlags = %client.flagCount - 1;
		if (%totalFlags <= 0)
			return;
		
		//if "greed mode" is on, can't cap less than greed amount...
		if ($FlagHunter::GreedMode && (%totalFlags < $FlagHunter::GreedAmount))
		{
			Client::sendMessage(%client, 1, "Greed mode is ON!  You must have " @ $FlagHunter::GreedAmount @ " flags before you can return them.~wmine_act.wav");
			return;
		}
			
		//if "greed mode" is on, can't cap less than greed amount...
		%curTimeLeft = ($Server::timeLimit * 60) + $missionStartTime - getSimTime();
		if ($FlagHunter::HoardMode && (%curTimeLeft <= ($FlagHunter::HoardStartTime * 60)) && (%curTimeLeft > ($FlagHunter::HoardEndTime * 60)))
		{
			%hoardTimeLeft = %curTimeLeft - ($FlagHunter::HoardEndTime * 60);
			%hoardMinutesLeft = floor(%hoardTimeLeft / 60);
			%hoardSecondsLeft = floor(%hoardTimeLeft - (%hoardMinutesLeft * 60));
			if (%hoardMinutesLeft == 0)
			{
				if (%hoardSecondsLeft == 1)
					%timeString = "1 second";
				else
					%timeString = %hoardSecondsLeft @ " seconds";
			}
			else
			{
				if (%hoardMinutesLeft == 1)
					%timeString = "1 minute";
				else
					%timeString = %hoardMinutesLeft @ " minutes";
				if (%hoardSecondsLeft > 0)
				{
					if (%hoardSecondsLeft == 1)
						%timeString = %timeString @ " and 1 second";
					else
						%timeString = %timeString @ " and " @ %hoardSecondsLeft @ " seconds";
				}
			}
			
			Client::sendMessage(%client, 1, "Hoard mode is in effect!  You must wait " @ %timeString @ " before you can return your flags.~wmine_act.wav");
			return;
		}
		
		//return the flags he's captured, and score them (note, he's always carrying his own - don't score it)
		%touchClientName = Client::getName(%client);
		%totalScore = 0;
		for (%i = 1; %i < %client.flagCount; %i++)
		{
			%totalScore += %i;
		}
		%client.score += %totalScore;
		%client.Credits += %totalScore;
		%client.flagCount = 1;
		//take the flag off the players back
	//	Annihilation::setItemCount(%client, Flag, 0);
		
		if (%totalFlags > $FlagHunter::MostFlagsReturnCount)
		{
			$FlagHunter::MostFlagsReturnCount = %totalFlags;
			$FlagHunter::MostFlagsReturned = %touchClientName;
		}
		
		%newRecord = false;
		if ((%totalFlags > $FlagHunter::MostFlagsEverCount[$missionName]) && (getNumClients() >= 4))
		{
			$FlagHunter::MostFlagsEverCount[$missionName] = %totalFlags;
			$FlagHunter::MostFlagsEver[$missionName] = %touchClientName;
			%newRecord = true;
			
			//save it to a file
			export("$FlagHunter::MostFlagsEver*", "config\\HunterRecords.cs", False);
		}
		
		Game::refreshClientScore(%client);
			
		//send the message
		if (%totalFlags >= 5 && (! %newRecord))
		{
			%color = 1;
			%sound = "!~wflagreturn.wav";
		}
		else
		{
			%color = 0;
			%sound = "!";
		}
		MessageAllExcept(%client, %color, %touchClientName @ " has returned " @ %totalFlags @ " flags for a score of " @ %totalScore @ %sound);
		Client::sendMessage(%client, %color, "You returned " @ %totalFlags @ " flags for a score of " @ %totalScore @ %sound);
		
		%playerTeam = GameBase::getTeam(%object);
		if(%totalFlags >= 5)
		{
			%teamScore++;
			if(%totalFlags >= 10)
				%teamScore++;	
			$teamScore[%playerTeam] += floor(%teamScore);
		
		}
		if (%newRecord)
		{
			MessageAll(1, "New record of " @ $FlagHunter::MostFlagsEverCount[$missionName] @ " set by " @ $FlagHunter::MostFlagsEver[$missionName] @ "!~wflagcapture.wav");
		}
		
		//update anyone who is tracking this player
		%name = Client::getName(%client);
		%numClients = getNumClients();
		for (%i = 0; %i < %numclients; %i++)
		{ 
			%tracker = getClientByIndex(%i);  
			if (%tracker != %client && %tracker.target == %client)
			{
				IssueTargCommand(%tracker, %tracker, 0, "Too late! " @ %name @ " capped!", %client - 2048);
			}
		}
	}
	else
	{	
		//set the flag
		%client.inAltarArea = true;
		%client.inAltarAreaTime = getSimTime();
		
		//schedule the call to see if he's still there
		schedule("altarCampingDamage(" @ %object @ ", true);", $FlagHunter::AltarCampingTimer);
	}
}	

// client cannot camp near the flag
function GroupTrigger::onLeave(%this, %object)
{
	if (getObjectType(%object) != "Player")
		return;
		
	if (%this.nexus)
		return;
		
	%client = Player::getClient(%object);
	
	//reset
	%client.inAltarArea = false;
}

function altarCampingDamage(%player, %giveWarning)
{
	if (getObjectType(%player) != "Player")
		return;
		
	%client = Player::getClient(%player);
	if (%client <= 0)
		return;
	
	//give damage if the person is still camping, and has been there for 8 seconds or more
	if ((! Player::isDead(%client)) && %client.inAltarArea && ((getSimTime() - %client.inAltarTime) >= $FlagHunter::AltarCampingTimer))
	{
		if (%giveWarning)
		{
			Client::sendMessage(%client, 1, "No camping near the Nexus! ~wLeftMissionArea.wav");
			schedule("altarCampingDamage(" @ %player @ ", false);", $FlagHunter::AltarCampingTimer / 2);
		}
		else
		{	
			Player::setDamageFlash(%client,0.1);
			GameBase::setDamageLevel(%player, GameBase::getDamageLevel(%player) + 0.05);
			schedule("altarCampingDamage(" @ %player @ ", false);", 1);
		}
	}
}

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
function remoteHuntersFindTarget(%sender)
{
	if(!evalspam(%sender))
		return;	
			
	%numClients = getNumClients();
	%maxFlags = 0;
	%maxFlagCarrier = -1;
	for (%i = 0; %i < %numclients; %i++)
	{ 
		%client = getClientByIndex(%i);  
		if (%client != %sender && %client.flagCount > %maxFlags)
		{
			%maxFlags = %client.flagCount;
			%maxFlagCarrier = %client;
		}
	}
	
	if (%maxFlags > 0)
	{
		%name = Client::getName(%maxFlagCarrier);
		%sender.target = %maxFlagCarrier;
		if (%maxFlags > 1)
			%temp = " flags!";
		else
			%temp = " flag";
		IssueTargCommand(%sender, %sender, 0, "Kill " @ %name @ " and grab " @ %maxFlags @ %temp, %maxFlagCarrier - 2048);
	}
	else
	{
		Client::sendMessage(%sender, 0, "No one has any flags.");
	}
}

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
function FlagHunter::restoreServerDefaults()
{
	exec("player.cs");	
	exec("objectives.cs");
}

function ReturnObjectives()
{
	%group = nameToID("MissionCleanup/ObjectivesSet");
	//messageall(1,"returning flags");
	for(%i = 0; (%obj = Group::getObject(%group, %i)) != -1; %i++)
	{
		%name = Item::getItemData(%obj);
		echo(%name);
		if(%name == flag)
		{
			%flagcount++;
			if(!%obj.atHome)
			{
				messageall(1,"returning "@ getTeamName(GameBase::getTeam(%obj))@"'s "@%name);	
				%player = %obj.carrier;
				Annihilation::setItemCount(%player, Flag, 0);
				GameBase::setPosition(%obj, %obj.originalPosition);
				GameBase::setIsTarget(%obj,false);	
				Item::setVelocity(%obj, "0 0 0");
				GameBase::startFadeIn(%obj);
			
				%obj.atHome = true;
				%obj.carrier = -1;
				%obj.pickupSequence++;
				%player.carryFlag = "";		
					
				Item::Hide(%obj,false);
			}
		}
	}
	if(!%flagcount)
	{
		$TowerSwitchNexus = true;	
	}
	else
		$TowerSwitchNexus = false;
}


function TowerSwitch::onCollision(%this, %object)
{	
	if($debug) 
		event::collision(%this,%object);
	if(%object.testing)	
		return;
	%playerClient = Player::getClient(%object);
	if(getObjectType(%object) != "Player" || %playerClient.isSpy || Player::isDead(%object))
		return;

	%playerTeam = GameBase::getTeam(%object);
	%oldTeam = GameBase::getTeam(%this);
	

	if($TowerSwitchNexus)
	{
		%this.nexus = true;
		FlagHunter::EnterNexus(%this, %object);
		schedule(%playerClient@".inAltarArea = false;",4);
		
		//let players know whats going on.. 
		if((getSimTime() - %this.LastMessage > 15.0 && %this.LastTouch != %object) || !%this.LastTouch)
		{
			%this.LastTouch = %playerClient;
			%this.LastMessage = getSimTime();
			messageall(1,"Welcome to Flaghunter Mode! Collect dead players flags, and exchange them for points at tower switches.");
		}			
	}
	
	if(%oldTeam == %playerTeam)
		return;

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
			schedule("TowerSwitch::timeLimitCheckPoints(" @ %this @ "," @ %playerClient @ "," @ %this.numSwitchTeams @ ");",60);
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
	ObjectiveMission::checkScoreLimit();
}



echo("******* loaded successfully ********");
