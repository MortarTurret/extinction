$InvList[AirBasePack] = 1;
$MobileInvList[AirBasePack] = 1;
$RemoteInvList[AirBasePack] = 0;
AddItem(AirBasePack);

ItemImageData AirBasePackImage
{	
	shapeFile = "vehi_pur_pnl";
	mountPoint = 2;
	mountOffset = { 0, -0.65, -0.4 };
	mountRotation = { 0, 0, 0 };
	firstPerson = false;
};

ItemData AirBasePack
{	
	description = "Air Base";
	shapeFile = "vehi_pur_pnl";
	className = "Backpack";
	heading = $InvHead[ihRem];
	imageType = AirBasePackImage;
	shadowDetailMask = 4;
	mass = 5.0;
	elasticity = 0.2;
	price = 9000;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function AirBasePack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player); 
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Air Base: <f2>Deploys an air base complete with an <f1>Inventory Station<f2>, <f1>Vehicle Pad<f2> and <f1>Solar Panels<f2>.");	
}

StaticShapeData CommandStation2
{
	description = "Air Base Health";
	shapeFile = "cmdpnl";
	className = "Station";
	visibleToSensor = true;
	shieldShapeName = "shield";
	sequenceSound[0] = { "activate", SoundActivateCommandStation };
	sequenceSound[1] = { "power", SoundCommandStationPower };
	sequenceSound[2] = { "use", SoundUseCommandStation };
	maxDamage = 10.0;
	debrisId = flashDebrisMedium;
	mapFilter = 4;
	mapIcon = "M_station";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	triggerRadius = 1.5;
	explosionId = flashExpLarge;
};

StaticShapeData CommandStation1
{
	description = "Air Base Health"; 
	shapeFile = "mainpad"; 
	maxDamage = 5000.0;
	debrisId = flashDebrisMedium;
	isTranslucent = true; 
	description = "MainPad";
	damageSkinData = "objectDamageSkins";
	triggerRadius = 1.5;
	explosionId = flashExpLarge;
};

MineData SmogSmoke
{
   	mass = 0.3;
   	drag = 1.0;
   	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
   	description = "Handgrenade";
   	shapeFile = "grenade";
   	shadowDetailMask = 4;
   	explosionId = WickedBadExp;
	explosionRadius = 40.0;
	damageValue = 9999.0;
	damageType = $MortarDamageType;
	kickBackStrength = 250;
	triggerRadius = 0.5;
	maxDamage = 9999.0;
};

function ABDropshipTeamMessage(%team, %color, %msg)
{
	%numPlayers = getNumClients();
	for(%i = 0; %i < %numPlayers; %i++)
	{
		%cl = getClientByIndex(%i);
		%now = Client::getTeam(%cl);
		if(%team == %now)
		{
			Client::sendMessage(%cl,%color,%msg);
		}
	}
}

function CommandStation2::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
		%ABDropTeam = GameBase::getTeam(%this);
		%client = Player::getClient(%object);
		%KillerTeam = GameBase::getTeam(%client);
		%name = GameBase::getDataName(%this);
		%player = Client::getOwnedObject(%client);
		%damageLevel = GameBase::getDamageLevel(%this);
		%pos = GameBase::getPosition(%this);

	if(GameBase::getDamageState(%this) == "Destroyed" || %value <= 0)
	 	return;
		if(%KillerTeam == %ABDropTeam)
		{
			%dValue = %damageLevel + %value * $Server::TeamDamageScale;
			GameBase::setDamageLevel(%this,%dValue);
		}
		if(%KillerTeam != %ABDropTeam)
		{
			%dValue = %damageLevel + (%value/2);	
			GameBase::setDamageLevel(%this,%dValue);
		}
	if(GameBase::getDamageState(%this) == "Destroyed") 
	{
		calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 3, 0.55, 0.30, 250, 170);		
		GameBase::playSound(%this, SoundFirePlasma, 0);

		Anni::Echo("destroy airbase started");
		if(%KillerTeam == %ABDropTeam)
		{
			bottomprint(%client, "<jc>You have just LOST <f2>5<f0> points for damaging your teams Air Base");	
	
			Anni::Echo("MSG: ", %client, " Destroyed a team ",  %name.description);
			ABDropshipTeamMessage(%KillerTeam, 3, "Team member "@Client::getName(%client)@" has damaged your teams Air Base");
			%client.score-=5;
			Game::refreshClientScore(%client);		
		}
		else
		{
			if(%this.LastRepairCl !=  %client)
			{	
				bottomprint(%client, "<jc>You have just recieved <f2>2<f0> points for damaging the enemy Air Base");
				%client.score+= 2;
				%client.Credits+= 2;
				Game::refreshClientScore(%client);			
			}	
			Anni::Echo("MSG: ", %client, " Destroyed enemy  ",  %name.description);
			ABDropshipTeamMessage(%ABDropTeam, 3, "WARNING "@Client::getName(%client)@" has damaged your teams Air Base");
		}

	%EnemyTeam = GameBase::getTeam(%this);
	if(%EnemyTeam == 0)
	{
	TeamMessages(1, 0, "Your teams Air Base will explode in "@$AirbaseExplosionTime@" seconds without repairs! ~wCapturedTower.wav");
	TeamMessages(3, 1, "The " @ getTeamName(%EnemyTeam) @ " Air Base will explode in "@$AirbaseExplosionTime@" seconds! ~wCapturedTower.wav");
	}
	if(%EnemyTeam == 1)
	{
	TeamMessages(1, 1, "Your teams Air Base will explode in "@$AirbaseExplosionTime@" seconds without repairs! ~wCapturedTower.wav");
	TeamMessages(3, 0, "The " @ getTeamName(%EnemyTeam) @ " Air Base will explode in "@$AirbaseExplosionTime@" seconds! ~wCapturedTower.wav");
	}
		ABWarpcoreCheck(%this,$AirbaseExplosionTime);			
	}	
}

function ABWarpCoreCheck(%this,%time)
{	
	%time--;
	%name = GameBase::getDataName(%this);
	// %shipname = getWord(%name.description,0);
	if(GameBase::getDamageState(%this) == "Destroyed") 
	{
		if(%time <= 0)
		{
			%team = GameBase::getTeam(%this);			
			schedule("ABBlowShitUp("@%this@","@5@", "@100.0@");",5,%this);

			%EnemyTeam3 = GameBase::getTeam(%this);
			if(%EnemyTeam3 == 0)
				{
					TeamMessages(1, 0, "Your teams Air Base is going to blow in 5 seconds!! Run!! ~wshell_click.wav");
					TeamMessages(3, 1, "The " @ getTeamName(%EnemyTeam3) @ " Air Base is going to blow in 5 seconds!! Run!! ~wshell_click.wav");
				}
			if(%EnemyTeam3 == 1)
				{
					TeamMessages(1, 1, "Your teams Air Base is going to blow in 5 seconds!! Run!! ~wshell_click.wav");
					TeamMessages(3, 0, "The " @ getTeamName(%EnemyTeam3) @ " Air Base is going to blow in 5 seconds!! Run!! ~wshell_click.wav");
				}	
			$TeamItemCount[%team @ AirBasePack] -= 1;
		}
		else
		{
			schedule("ABWarpCoreCheck("@%this@","@%time@");",1,%this);
		}	
	}	
	else
	{
		//messageAll(0, "Air Base "@%shipname@" destruction averted!");
	%EnemyTeam2 = GameBase::getTeam(%this);
	if(%EnemyTeam2 == 0)
	{
	TeamMessages(1, 0, "Your teams Air Base has been repaired, destruction averted.");
	TeamMessages(3, 1, "The " @ getTeamName(%EnemyTeam2) @ " Air Base was repaired.");
	}
	if(%EnemyTeam2 == 1)
	{
	TeamMessages(1, 1, "Your teams Air Base has been repaired, destruction averted.");
	TeamMessages(3, 0, "The " @ getTeamName(%EnemyTeam2) @ " Air Base was repaired.");
	}
	}
}

$AirbaseExplosionTime = 60; // 30 Death666
$dettime = 5;
function ABBlowShitUp(%this, %dettime, %radius)
{
	//%group = GetGroup(%this);
	%team = GameBase::getTeam(%this);
	%group = nameToID("MissionCleanup/Airbase" @ %team);
	%dettime = 5.0;

	for(%i = 0; $ABObject[%team,%i] != ""; %i++)
	{
		%obj = $ABObject[%team,%i];
		%dat = GameBase::getDataName(%obj);
		%name = GameBase::getMapName(%obj);
		%type = GetObjectType(%obj);

		if(%obj == %this || !isObject(%obj))
		{	//do nothing
		}

		if(Gamebase::getMapName(%obj) == "Airbase Inventory Station")
		{
			GameBase::setDamageLevel(%obj.lTurret,1100);
			GameBase::setDamageLevel(%obj.rTurret,1100);	
			deleteobject(%obj.trigger);
		}

		if(%type == "InteriorShape")
		{
			deleteObject(%obj);
		}
		else
		{
			GameBase::setDamageLevel(%obj,9999);
			schedule("deleteObject("@ %obj @");",0.2,%obj);
		}
	}
	ABExplosion($SmokePosi[%team]);
	$SmokePosi[%team] = "";	
	deleteVariables("$ABObject[%team,*");
}

function ABExplosion(%pos)
{
	messageall(1,"~wEXPLO3.wav");
	%SmokeAgain = Vector::add(%pos,"0 10 0");
	%SmogSmoke1 = NewObject("",Mine,"SmogSmoke");
	AddToSet("MissionCleanup",%SmogSmoke1);
	GameBase::setPosition(%SmogSmoke1,%SmokeAgain);
	GameBase::setRotation(%SmogSmoke1,"0 0 0");
	schedule("Mine::Detonate(" @ %SmogSmoke1 @ ");",0.1);

	%SmokeAgain = Vector::add(%pos,"0 -10 0");
	%SmogSmoke1 = NewObject("",Mine,"SmogSmoke");
	AddToSet("MissionCleanup",%SmogSmoke1);
	GameBase::setPosition(%SmogSmoke1,%SmokeAgain);
	GameBase::setRotation(%SmogSmoke1,"0 0 0");
	schedule("Mine::Detonate(" @ %SmogSmoke1 @ ");",0.2);

	%SmokeAgain = Vector::add(%pos,"10 0 0");
	%SmogSmoke1 = NewObject("",Mine,"SmogSmoke");
	AddToSet("MissionCleanup",%SmogSmoke1);
	GameBase::setPosition(%SmogSmoke1,%SmokeAgain);
	GameBase::setRotation(%SmogSmoke1,"0 0 0");
	schedule("Mine::Detonate(" @ %SmogSmoke1 @ ");",0.4);

	%SmokeAgain = Vector::add(%pos,"-10 0 0");
	%SmogSmoke1 = NewObject("",Mine,"SmogSmoke");
	AddToSet("MissionCleanup",%SmogSmoke1);
	GameBase::setPosition(%SmogSmoke1,%SmokeAgain);
	GameBase::setRotation(%SmogSmoke1,"0 0 0");
	schedule("Mine::Detonate(" @ %SmogSmoke1 @ ");",0.5);

	%SmogSmoke1 = NewObject("",Mine,"SmogSmoke");
	AddToSet("MissionCleanup",%SmogSmoke1);
	GameBase::setPosition(%SmogSmoke1,%pos);
	GameBase::setRotation(%SmogSmoke1,"0 0 0");
	schedule("Mine::Detonate(" @ %SmogSmoke1 @ ");",0.6);
}

StaticShapeData AirBaseForceFieldLarge
{
	shapeFile = "forcefield";
	debrisId = defaultDebrisLarge;
	maxDamage = 50000;
	visibleToSensor = true;
	isTranslucent = true;
	description = "Force Field";
};

StaticShapeData AirBaseForceFieldSmall
{
	shapeFile = "ForceField_4x8";
	debrisId = defaultDebrisSmall;
	maxDamage = 50000;
	visibleToSensor = true;
	isTranslucent = true;
	description = "Force Field";
};

function CreateAirBaseSimSet(%team)
{
	%Baseset = nameToID("MissionCleanup/AirBase" @ %team);
	if(%Baseset == -1)
	{
		%group = newObject("AirBase" @ %team,SimGroup);
		addToSet("MissionCleanup",%group);
	}	
}

function AirBasePack::deployshape(%player,%item)
{

	%client = Player::getClient(%player);
	%team = GameBase::getTeam(%player);
	%playerPos = Vector::add(GameBase::getPosition(%player), "0 0 117");
	%SmokeLoc = Vector::add(GameBase::getPosition(%player), "0 0 135");
	%prot = GameBase::getRotation(%player);

	%clientId = Player::getClient(%player);

		if(!$build)
		{
	if(%clientId.inArena)
	{ 
		Client::sendMessage(%client,0,"Cannot deploy in arena unless building is on. ");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return;	
	}
		}
		if(!$build)
		{
	if(%player.outArea)
	{
		Client::sendMessage(%client,0,"can not deploy out of bounds unless building is on.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
		}

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]) 
	{
		Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
		return false;
	}

	if(!GameBase::getLOSInfo(%player,5))
	{
		Client::sendMessage(%client,0,"Deploy position out of range.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item])
	{
		Client::sendMessage(%client,0,"Can not deploy, Air Base already in place.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}

	if(getNumTeams()-1 == 2)
	{
		if(%team == 0)
			%enemyTeam = 1;
		else if(%team == 1)
			%enemyTeam = 0;

		if(Vector::getDistance($teamFlag[%enemyTeam].originalPosition, %playerpos) < 150)
		{
			Client::sendMessage(%client,0,"You are too close to the enemy base to deploy that.");
			Client::sendMessage(%client,0,"~wC_BuySell.wav");
			return false;
		}
	}
		
	%obj = getObjectType($los::object);
	if(%obj != "SimTerrain") 
	{
		Client::sendMessage(%client,0,"Can only deploy on terrain");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}

//	if(%player.outArea)
//	{
//		Client::sendMessage(%client,0,"Can Not Deploy Airbase out of bounds");
//		Client::sendMessage(%client,0,"~wC_BuySell.wav");
//		return false;
//	}

	%flagpos = $teamFlag[%team].originalPosition;	
	if(Vector::getDistance(%flagpos, %playerpos) < 200)
	{
		Client::sendMessage(%client,0,"You are too close to your flag to deploy the Airbase.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}

// new
      //Get the current map's mission type from the .dsc file
      %missionFile = "missions\\" $+ $missionName $+ ".dsc";
      if(File::FindFirst(%missionFile) == "")
      {
         %missionName = $firstMission;
         %missionFile = "missions\\" $+ $missionName $+ ".dsc";
         if(File::FindFirst(%missionFile) == "")
         {
            echo("invalid nextMission and firstMission...");
            echo("aborting mission load.");
            return;
         }
      }
      exec(%missionFile);
		
      for(%i = 0; %i < $MLIST::TypeCount; %i++)
      {
         if($MLIST::Type[%i] == $MDESC::Type)
         {
            break;
         }
      }


         	if(($MDESC::Type == "CTF Indoor" )) 
 	{
		%client = Player::getClient(%player);
		Client::sendMessage(%client,0,"Unable to deploy on indoor maps!");
		return false;
 	}
// end new

// check for deployed ships.

	if(getNumTeams()-1 == 2)
	{
	%teamnumzero = 0;
	%teamnummone = 1;
	}
	
if(((($DropShipMultipass1[%teamnumzero] == true) || ($DropShipMultipass2[%teamnumzero] == true) || ($DropShipMultipass1[%teamnummone] == true) || ($DropShipMultipass2[%teamnummone] == true))))
{
	%clientId = Player::getClient(%player);
	%playerPosA = GameBase::getPosition(%player);
	%strekpos = $DropShipLocation[%teamnumzero @ "GunShip"];
	%strekpos2 = $DropShipLocation[%teamnummone @ "GunShip"];
	if(((Vector::getDistance(%strekpos, %playerposA)) < 70) && (%strekpos != "")) 
	{
		Client::sendMessage(%clientId,0,"You are too close to a Starwolf Gunship landing zone or crash site. ~waccess_denied.wav");
		return false;
	}
	if(((Vector::getDistance(%strekpos2, %playerposA)) < 70) && (%strekpos2 != "")) 
	{
		Client::sendMessage(%clientId,0,"You are too close to a Starwolf Gunship landing zone or crash site. ~waccess_denied.wav");
		return false;
	}
}

	%clientId = Player::getClient(%player);

	if(%clientId.waitmsgHP == "false")
	{
		Client::sendMessage(%clientId,0,"Please Wait A Few Seconds To Re-Deploy The Airbase or find a clear area to deploy. ~waccess_denied.wav");
		return;
	}

if(%clientId.waitmsgHP)
{

	%clientId.waitmsgHP = false;
	schedule(%clientId@".waitmsgHP= true;",1.0,%clientId);

	%no = False;
	%set = newObject("set",SimSet);
	addToSet("MissionCleanup", %set);
	%Mask = $SimPlayerObjectType|$StaticObjectType|$VehicleObjectType|$MineObjectType|$SimInteriorObjectType; 
	%num = containerBoxFillSet(%set,%Mask,vector::add($los::position, "0 0 150"),40,40,40,0); //100
	%totalnum = Group::objectCount(%set);
	if ( %totalnum )
		%no = True;
	deleteObject(%set);

	if ( %no )
	{
		Client::sendMessage(%client, 0, "Object in the way of the airbase.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return False;
	}

//	CreateAirbaseSimSet(%team);
	%group = nameToID("MissionCleanup/AirBase" @ %team);
	if(%group == -1)
	{
		%group = newObject("AirBase" @ %team,SimGroup);
		addToSet("MissionCleanup",%group);
	}


	%numABObjects = -1;
	$DestroyAirbase[%team] = false;
	$SmokePosi[%team] = %SmokeLoc;
	
	%object = newObject("Air Base2","InteriorShape", "BEMfloatingPad.dis");
	$ABObject[%team,%numABObjects++] = %object;
	addToSet(%group, %object);
//	addToSet(MissionCleanup, %object);
//	addToSet("MissionCleanup/Airbase" @ %team, %object);

	%pos = rotateVector("-5 -5 0",%prot);
	%pos = Vector::add(%playerPos,%pos);	
	%rot = "0 0 0";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);

	%object = newObject("Air Base2","InteriorShape", "BESfloatingPad.dis");
	$ABObject[%team,%numABObjects++] = %object;
	addToSet(%group, %object);
//	addToSet(MissionCleanup, %object);
//	addToSet("MissionCleanup/Airbase" @ %team, %object);

	%pos = rotateVector("0 0 38",%prot);
	%pos = Vector::add(%playerPos,%pos);	
	%rot = "0 3.14159 0";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);	

	%object = newObject("Air Base2","StaticShape",AirBaseForceFieldLarge,false);
	$ABObject[%team,%numABObjects++] = %object;
	addToSet(%group, %object);
//	addToSet(MissionCleanup, %object);
//	addToSet("MissionCleanup/Airbase" @ %team, %object);

	%pos = rotateVector("5.8746 0 14.5",%prot);
	%pos = Vector::add(%playerPos,%pos);	
	%rot = "0 0 1.570796";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);
	GameBase::startFadeIn(%object);	
	
	%object = newObject("Air Base2","StaticShape",AirBaseForceFieldSmall,false);
	$ABObject[%team,%numABObjects++] = %object;
	addToSet(%group, %object);
//	addToSet(MissionCleanup, %object);
//	addToSet("MissionCleanup/Airbase" @ %team, %object);

	%pos = rotateVector("-5.75 2.05 16",%prot);
	%pos = Vector::add(%playerPos,%pos);	
	%rot = "0 0 1.570796";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);
	GameBase::startFadeIn(%object);
	
	%object = newObject("Air Base2","StaticShape",AirBaseForceFieldLarge,false);
	$ABObject[%team,%numABObjects++] = %object;
	addToSet(%group, %object);
//	addToSet(MissionCleanup, %object);
//	addToSet("MissionCleanup/Airbase" @ %team, %object);

	%pos = rotateVector("0 5.8746 14.5",%prot);
	%pos = Vector::add(%playerPos,%pos);	
	%rot = "0 0 0";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);
	GameBase::startFadeIn(%object);
	
	%object = newObject("Air Base2","StaticShape",AirBaseForceFieldSmall,false);
	$ABObject[%team,%numABObjects++] = %object;
	addToSet(%group, %object);
//	addToSet(MissionCleanup, %object);
//	addToSet("MissionCleanup/Airbase" @ %team, %object);

	%pos = rotateVector("2.03 -5.75 16",%prot);
	%pos = Vector::add(%playerPos,%pos);	
	%rot = "0 0 0";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);
	GameBase::startFadeIn(%object);
	
//	%object = newObject("Vehicle Station","StaticShape",VehicleStation,false);
	%object = newObject("Vehicle Station","StaticShape",VehicleStation,false);
	$ABObject[%team,%numABObjects++] = %object;
	addToSet(%group, %object);
//	addToSet(MissionCleanup, %object);
//	addToSet("MissionCleanup/Airbase" @ %team, %object);

	%pos = rotateVector("-3.60177 3.95522 16",%prot);
	%pos = Vector::add(%playerPos,%pos);	
	%rot = "0 0 0.7853981";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);
	Gamebase::setMapName(%object,"Airbase Vehicle Station");
		
//	%object = newObject("Air Base2","StaticShape",VehiclePad,false);
	%object = newObject("Vehicle Pad","StaticShape",VehiclePad,false);
	$ABObject[%team,%numABObjects++] = %object;
	addToSet(%group, %object);
//	addToSet(MissionCleanup, %object);
//	addToSet("MissionCleanup/Airbase" @ %team, %object);

	%pos = rotateVector("-10.6862 -11.1697 16",%prot);
	%pos = Vector::add(%playerPos,%pos);	
	%rot = "0 -0 2.3561944";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);
	Gamebase::setMapName(%object,"Airbase Vehicle Pad");
	
	%object = newObject("Inventory Station","StaticShape",InventoryStation,false);
	$ABObject[%team,%numABObjects++] = %object;
	addToSet(%group, %object);
//	addToSet(MissionCleanup, %object);
//	addToSet("MissionCleanup/Airbase" @ %team, %object);

	%pos = rotateVector("3.69689 -3.64708 16",%prot);
	%pos = Vector::add(%playerPos,%pos);	
	%rot = "0 0 -2.3561944";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);
	Gamebase::setMapName(%object,"Airbase Inventory Station");

	%object = newObject("RepairPack","Item","repairpack",1,true,true);
	$ABObject[%team,%numABObjects++] = %object;
	addToSet(%group, %object);
//	addToSet(MissionCleanup, %object);
//	addToSet("MissionCleanup/Airbase" @ %team, %object);

	%pos = rotateVector("3.69689 -3.64708 16",%prot);
	%pos = Vector::add(%playerPos,%pos);	
	%rot = "0 0 -2.3561944";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	
	%object = newObject("Command Station","StaticShape",CommandStation2,false);
	$ABObject[%team,%numABObjects++] = %object;
	addToSet(%group, %object);
//	addToSet(MissionCleanup, %object);
//	addToSet("MissionCleanup/Airbase" @ %team, %object);

	%pos = rotateVector("4.25588 4.13451 16",%prot);
	%pos = Vector::add(%playerPos,%pos);	
	%rot = "0 0 -0.7853981";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);
//	Gamebase::setMapName(%object,"Airbase Command Station");

	%object = newObject("Command Station","StaticShape",CommandStation1,false);
	$ABObject[%team,%numABObjects++] = %object;
	addToSet(%group, %object);
//	addToSet(MissionCleanup, %object);
//	addToSet("MissionCleanup/Airbase" @ %team, %object);

	%pos = rotateVector("4.25588 4.13451 16",%prot);
	%pos = Vector::add(%playerPos,%pos);	
	%rot = "0 0 -0.7853981";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);
	Gamebase::setMapName(%object,"Airbase Command Station");
	
	%object = newObject("MedPulse", "Sensor", MediumPulseSensor,false);
	$ABObject[%team,%numABObjects++] = %object;
	addToSet(%group, %object);
//	addToSet(MissionCleanup, %object);
//	addToSet("MissionCleanup/Airbase" @ %team, %object);

	%pos = rotateVector("0 0 20.55",%prot);
	%pos = Vector::add(%playerPos,%pos);	
	%rot = "0 0 0";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);
	Gamebase::setMapName(%object,"Airbase Pulse Sensor");
	
	%object = newObject("Solar Panel","StaticShape",SolarPanel,false);
	$ABObject[%team,%numABObjects++] = %object;
	addToSet(%group, %object);
//	addToSet(MissionCleanup, %object);
//	addToSet("MissionCleanup/Airbase" @ %team, %object);

	%pos = rotateVector("-7.49755 1.43987 16",%prot);
	%pos = Vector::add(%playerPos,%pos);	
	%rot = "0 0 -1.5707963";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);
	Gamebase::setMapName(%object,"Airbase Solar Panel");
	
	%object = newObject("Solar Panel","StaticShape",SolarPanel,false);
	$ABObject[%team,%numABObjects++] = %object;
	addToSet(%group, %object);
//	addToSet(MissionCleanup, %object);
//	addToSet("MissionCleanup/Airbase" @ %team, %object);

	%pos = rotateVector("1.58014 -7.45512 16",%prot);
	%pos = Vector::add(%playerPos,%pos);	
	%rot = "0 0 0";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);
	Gamebase::setMapName(%object,"Airbase Solar Panel");

	%posX = getWord(%playerpos, 0);
	%posY = getWord(%playerpos, 1);
	%posZ = getWord(%playerpos, 2) + 16;
	%playerPos2 = Vector::add(GameBase::getPosition(%player), "0 0 133");

	Client::sendMessage(%client,0,%item.description @ " deployed and teleported to!~wteleport2.wav");	//150
	messageTeamExcept(%client, 3, "A team Air Base has been deployed by "@Client::getName(%client)@". ~wfemale2.wwaitpas.wav");
  	GameBase::setPosition(%client,%playerpos2);
   	Item::setVelocity(%client,"0 0 0");
	// playSound(SoundPickupBackpack,$los::position);

	$TeamItemCount[%team @ %item]++;
	Anni::Echo("MSG: ",%client," deployed an Air Base");
	return true;
}
}