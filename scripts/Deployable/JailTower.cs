$InvList[JailTower] = 1;
$MobileInvList[JailTower] = 1;
$RemoteInvList[JailTower] = 0;
AddItem(JailTower);

ItemImageData JailTowerImage
{	
	shapeFile = "ammopack";
	mountPoint = 2;
	mountOffset = { 0, 0, 0 };
	mountRotation = { 0, 0, 0 };
	firstPerson = false;
};

ItemData JailTower
{	
	description = "Jail Tower";
	shapeFile = "shieldpack";
	className = "Backpack";
	heading = $InvHead[ihDob];
	imageType = JailTowerImage;
	shadowDetailMask = 4;
	mass = 5.0;
	elasticity = 0.2;
	price = 9000;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

StaticShapeData JailWall
{
	shapeFile = "forcefield";
	debrisId = defaultDebrisLarge;
	maxDamage = 75;
	visibleToSensor = true;
	isTranslucent = true;
	description = "Jail Tower";
};

StaticShapeData JailWallMessage
{
	shapeFile = "forcefield";
	debrisId = defaultDebrisLarge;
	maxDamage = 75;
	visibleToSensor = true;
	isTranslucent = true;
	description = "Jail Tower";
};

function JailWall::onDestroyed(%this)
{
	DestroyJail(%this); 
}

function JailWallMessage::onDestroyed(%this, %object)
{
	%EnemyTeam = GameBase::getTeam(%this);
	if(%EnemyTeam == 0)
	{
	TeamMessages(1, 0, "Your teams Jail Tower has been destroyed! ~wCapturedTower.wav");
	TeamMessages(3, 1, "The " @ getTeamName(%EnemyTeam) @ " Jail Tower has been destroyed! ~wEXPLO3.wav");
	}
	if(%EnemyTeam == 1)
	{
	TeamMessages(1, 1, "Your teams Jail Tower has been destroyed! ~wCapturedTower.wav");
	TeamMessages(3, 0, "The " @ getTeamName(%EnemyTeam) @ " Jail Tower has been destroyed! ~wEXPLO3.wav");
	}
	DestroyJail(%this); 
}

function DestroyJail(%this)
{
	%team = GameBase::getTeam(%this);

	if($JailDestroyed[%team]) return;
	else $JailDestroyed[%team] = true;

//	messageAll(0, "destroy jail ran");

	for(%i = 0; $JailObject[%team,%i] != ""; %i++)
	{
		%obj = $JailObject[%team,%i];
		%dat = GameBase::getDataName(%obj);
		%name = GameBase::getMapName(%obj);
		%type = GetObjectType(%obj);

		if(%obj == %this || !isObject(%obj))
		{	//do nothing
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
	JailExplosion($JailPos[%team]);
	$JailDestroyed[%team] = true; // -death666 3.29.17

//	%Mask = $StaticObjectType|$MineObjectType|$VehicleObjectType;
//	BoxDestroy($JailPos[%team],50,50,50,%mask);

	$JailPos[%team] = "";
	deleteVariables("$JailObject[%team,*");

	$TeamItemCount[%team @ JailTower] -= 1;
}

function JailExplosion(%pos)
{
//	messageAll(0, "jail explosion ran");
	%SmokeAgain = Vector::add(%pos,"0 10 0");
	%SmogSmoke1 = NewObject("",Mine,"BombHellHalo");
	AddToSet("MissionCleanup",%SmogSmoke1);
	GameBase::setPosition(%SmogSmoke1,%SmokeAgain);
	GameBase::setRotation(%SmogSmoke1,"0 0 0");
	schedule("Mine::Detonate(" @ %SmogSmoke1 @ ");",0.2);
}


StaticShapeData JailStand
{
	shapeFile = "flagstand";
	debrisId = defaultDebrisSmall;
	maxDamage = 50000;
	description = "Jail Stand";
};

StaticShapeData JailReleasePad
{
	shapeFile = "flagstand";
	debrisId = defaultDebrisSmall;
	maxDamage = 50000;
	description = "Jail Release Pad";
};

StaticShapeData JailReleaseSwitch
{
	className = "towerSwitch";
	shapeFile = "tower";
	showInventory = "false";
	visibleToSensor = true;
	mapFilter = 4;
	mapIcon = "M_generator";
	maxDamage = 50000;
	description = "Jail Release Switch";
};

// function CreateJailSimSet(%team)
// {
//	%teleset = nameToID("MissionCleanup/JailTower" @ %team);
//	if(%teleset == -1)
//	{	newObject("JailTower" @ %team,SimSet);
//		addToSet("MissionCleanup","JailTower" @ %team);
//	}
//	%teleset = nameToID("MissionCleanup/JailRelease" @ %team);
//	if(%teleset == -1)
//	{
//		newObject("JailRelease" @ %team,SimSet);
//		addToSet("MissionCleanup","JailRelease" @ %team);
//	}
//	%teleset = nameToID("MissionCleanup/JailWalls" @ %team);
//	if(%teleset == -1)
//	{
//		newObject("JailWalls" @ %team,SimSet);
//		addToSet("MissionCleanup","JailWalls" @ %team);
//	}
// }

function CreateJailSimSet(%team)
{
//	%Baseset = nameToID("MissionCleanup/JailTower" @ %team);
//	if(%Baseset == -1)
//	{
//		%group = newObject("JailTower" @ %team,SimGroup);
//		addToSet("MissionCleanup",%group);
//	}
//	%Baseset = nameToID("MissionCleanup/JailRelease" @ %team);
//	if(%Baseset == -1)
//	{
//		%group = newObject("JailRelease" @ %team,SimGroup);
//		addToSet("MissionCleanup",%group);
//	}
	%Baseset = nameToID("MissionCleanup/JailWalls" @ %team);
	if(%Baseset == -1)
	{
		%group = newObject("JailWalls" @ %team,SimGroup);
		addToSet("MissionCleanup",%group);
	}	
}




function JailTower::deployshape(%player,%item)
{
	%client = Player::getClient(%player);
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
	%team = GameBase::getTeam(%player);
	%playerPos = GameBase::getPosition(%player);
	%prot =GameBase::getRotation(%player);
	$JailDestroyed[%team] = ""; // -death666 3.29.17

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]) 
	{
		Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
		return false;
	}
	
	if(!GameBase::getLOSInfo(%player,2))
	{
		Client::sendMessage(%client,0,"Deploy position too far away");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}	
	
	%obj = getObjectType($los::object);
	if(%obj != "SimTerrain") 
	{
		Client::sendMessage(%client,0,"Can only deploy on terrain");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
		

	if($TeamItemCount[%team @ %item] >= $TeamItemMax[%item])
	{
		Client::sendMessage(%client,0,"Can Not Deploy, Jail Tower Already In Place");
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


	%flagpos = $teamFlag[%team].originalPosition;	
	if(Vector::getDistance(%flagpos, %playerpos) < 150)
	{
		Client::sendMessage(%client,0,"You are too close to your flag, Must be further from flag to deploy.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}

	%clientId = Player::getClient(%player);

// check for deployed ships.

	if(getNumTeams()-1 == 2)
	{
	%teamnumzero = 0;
	%teamnummone = 1;
	}
	
if(((($DropShipMultipass1[%teamnumzero] == true) || ($DropShipMultipass2[%teamnumzero] == true) || ($DropShipMultipass1[%teamnummone] == true) || ($DropShipMultipass2[%teamnummone] == true))))
{
	%strekpos = $DropShipLocation[%teamnumzero @ "GunShip"];
	%strekpos2 = $DropShipLocation[%teamnummone @ "GunShip"];
	if(((Vector::getDistance(%strekpos, %playerpos)) < 70) && (%strekpos != "")) 
	{
		Client::sendMessage(%clientId,0,"You are too close to a Starwolf Gunship landing zone or crash site. ~waccess_denied.wav");
		return false;
	}
	if(((Vector::getDistance(%strekpos2, %playerpos)) < 70) && (%strekpos2 != "")) 
	{
		Client::sendMessage(%clientId,0,"You are too close to a Starwolf Gunship landing zone or crash site. ~waccess_denied.wav");
		return false;
	}
}

	if(%clientId.waitmsgHP == "false")
	{
		Client::sendMessage(%clientId,0,"Please Wait A Few Seconds To Re-Deploy The Jail or find a clear area to deploy. ~waccess_denied.wav");
		return;
	}

if(%clientId.waitmsgHP)
{
	%clientId.waitmsgHP = false;
	schedule(%clientId@".waitmsgHP= true;",1.0,%clientId);


	%set = newObject("JailCheck",SimSet);
	addToSet("MissionCleanup", %set);
	containerBoxFillSet(%set, $StaticObjectType | $ItemObjectType, %playerPos, 27, 27, 27,0);
	%num = Group::objectCount(%set);
	//Anni::Echo("checking area for other objects "@%num@" set # "@%set);
	if(%num){
		Client::sendMessage(%client,0,"Other objects in the way~waccess_denied.wav");
		deleteObject(%set);
		return false;
		}
	else deleteObject(%set);


	%camera = newObject("Camera","Turret",cameraturret,true);
	GameBase::setPosition(%camera,Vector::add(%playerPos,"0 0 2"));
	if(GameBase::getLOSInfo(%camera,35,"1.5708 0 0"))
		{
		%name = GameBase::getDataName($los::object);
		if(!%name) %name = getObjectType($los::object);
		Client::sendMessage(%client,0,%name@" in way, "@Vector::getDistance($los::position, GameBase::getPosition(%camera))@" meters up.");
		DeleteObject(%camera);
		return false;			
		}
	else DeleteObject(%camera);	
	
// looks ok to set this up.
	CreateJailSimSet(%team);

	%numJailObjects = -1;
	$jailDestroyed[%team] = false;
	$jailPos[%team] = %playerPos;

	%group = nameToID("MissionCleanup/JailTower" @ %team);
	if(%group == -1)
	{
		%group = newObject("JailTower" @ %team,SimGroup);
		addToSet("MissionCleanup",%group);
	}

	// Tower Floor
	%object = newObject("Jail Tower","InteriorShape", "BESfloatingPad.dis");
	$JailObject[%team,%numJailobjects++] = %object;
	addToSet(%group, %object);
//	addToSet("MissionCleanup/JailTower" @ %team, %object);
	%pos = rotateVector("0 0 -1.5",%prot);
	%pos = Vector::add(%playerPos,%pos);	
	%rot = "0 0 0";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);
	%object.inmotion = true;
	GameBase::startFadeIn(%object);

	// Tower Roof
	%object = newObject("Jail Tower","InteriorShape", "BESfloatingPad.dis");
	$JailObject[%team,%numJailobjects++] = %object;
	addToSet(%group, %object);
//	addToSet("MissionCleanup/JailTower" @ %team, %object);
	%pos = rotateVector("0 0 39.5219",%prot);
	%pos = Vector::add(%playerPos,%pos);	
	%rot = "0 3.14159 0";	
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);
	%object.inmotion = true;
	GameBase::startFadeIn(%object);
	
	// Tower Base
	%object = newObject("Jail Tower","InteriorShape","mis_ob1.0.dis");
	$JailObject[%team,%numJailobjects++] = %object;
	addToSet(%group, %object);
//	addToSet("MissionCleanup/JailTower" @ %team, %object);
	%pos = rotateVector("0 0 15",%prot);
	%pos = Vector::add(%playerPos,%pos);	
	%rot = "0 3.14159 0";
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);
	%object.inmotion = true;
	GameBase::startFadeIn(%object);

	// Force Field Wall
	%object = newObject("Jail Tower","StaticShape",JailWall,false);
	$JailObject[%team,%numJailobjects++] = %object;
//	addToSet(%group2, %object);
	addToSet("MissionCleanup/JailWalls" @ %team, %object);
	%pos = rotateVector("5.8746 0 16.0111",%prot);
	%pos = Vector::add(%playerPos,%pos);	
	%rot = "0 0 1.570796";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);
	GameBase::startFadeIn(%object);
	
	// Force Field Wall
	%object = newObject("Jail Tower","StaticShape",JailWall,false);
	$JailObject[%team,%numJailobjects++] = %object;
//	addToSet(%group2, %object);
	addToSet("MissionCleanup/JailWalls" @ %team, %object);
	%pos = rotateVector("-5.8746 0 16.0111",%prot);
	%pos = Vector::add(%playerPos,%pos);
	%rot = "0 0 1.570796";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);
	GameBase::startFadeIn(%object);
	
	// Force Field Wall
	%object = newObject("Jail Tower","StaticShape",JailWall,false);
	$JailObject[%team,%numJailobjects++] = %object;
//	addToSet(%group2, %object);
	addToSet("MissionCleanup/JailWalls" @ %team, %object);
	%pos = rotateVector("0 5.8746 16.0111",%prot);
	%pos = Vector::add(%playerPos,%pos);
	%rot = "0 0 0";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);
	GameBase::startFadeIn(%object);
	
	// Force Field Wall
	%object = newObject("Jail Tower","StaticShape",JailWallMessage,false);
	$JailObject[%team,%numJailobjects++] = %object;
//	addToSet(%group2, %object);
	addToSet("MissionCleanup/JailWalls" @ %team, %object);
	%pos = rotateVector("0 -5.8746 16.0111",%prot);
	%pos = Vector::add(%playerPos,%pos);
	%rot = "0 0 0";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);
	GameBase::startFadeIn(%object);

	// Release Switch
	%name = "Jail Tower";
	%object = newObject("Release Switch","StaticShape",JailReleaseSwitch,false);
	$JailObject[%team,%numJailobjects++] = %object;
	addToSet(%group, %object);
//	addToSet("MissionCleanup/JailTower" @ %team, %object);
	%pos = rotateVector("0 0 22.25",%prot); 
	%pos = Vector::add(%playerPos,%pos);
	%rot = "0 0 0"; 
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%object,%rot);
	GameBase::setPosition(%object,%pos);
	GameBase::setTeam(%object,%team);
	Gamebase::setMapName(%object,%name);
	GameBase::startFadeIn(%object);

	%posX = getWord(%playerpos, 0);
	%posY = getWord(%playerpos, 1);
	%posZ = getWord(%playerpos, 2) + 16;
	$JailPosition[%team] = %posX@" "@%posY@" "@%posZ;

	$TeamItemCount[%team @ %item]++;
	Anni::Echo("MSG: ",%client," deployed a Jail Tower");
	Client::sendMessage(%client,0,%item.description @ " deployed.");

	messageTeamExcept(%client, 3, "A team Jail Tower has been deployed by "@Client::getName(%client)@".");	
	playSound(SoundPickupBackpack,%playerPos);
	return true;
}
}


function JailReleaseSwitch::onCollision(%this,%obj)
{	
	if($debug) 
		event::collision(%this,%obj);

	if(!%this.wait)
	{
		schedule("GameBase::playSound("@%this@",ForceFieldOpen,0);",0.01,%this);
		%this.wait = true;
		%this.Jailup = 60;
		%this.Jaildown = 60;
		schedule("JailWall::move(" @ %this @ ",true);",0.1,%this);
		schedule("JailWall::move(" @ %this @ ");",10,%this);	
	}
}


function JailReleaseSwitch::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{	
	if($debug::Damage)
	{
		Anni::Echo("JailReleaseSwitch::onDamage("@%this@", "@%type@", "@%value@", "@%pos@", "@%vec@", "@%mom@", "@%object@" )");
	}	
	if($debug) 
		bottomprintall("<jc>damage jail switch "@%this@" object# "@%object@" type "@%type@" value "@%value);

	if(!%this.wait && %value > 0.3)
	{
		schedule("GameBase::playSound("@%this@",ForceFieldOpen,0);",0.01,%this);
		%this.wait = true;
		%this.Jailup = 60;
		%this.Jaildown = 60;
		schedule("JailWall::move(" @ %this @ ",true);",0.1,%this);
		schedule("JailWall::move(" @ %this @ ");",10,%this);	
	}
}

function ToJail(%client,%team)
{
	%player = Client::getOwnedObject(%client);
	if (%player == -1 || Player::isDead(%player))
		return;
//	Anni::Echo("jailed pl# "@%player);
	//%player.invulnerable = true;
 	//%player.jailed = true;	

	//	schedule("prisonguard("@%client@");",1);
		prisonguard(%client);
		%name = Client::getName(%Client);
		GameBase::SetPosition(%client, $JailPosition[%Team]);		
		Client::SendMessage(%client,1,"Your jail sentence will last 20 seconds.");
		schedule("JailBreak(" @ %client @ ", " @ %team @ ");",20,%client);
		schedule("JailBird(" @ %client @ ", " @ %team @ ");",1,%client);		
		if(GameBase::getControlClient(%player) != %client)
		{
		//yank em outa that turret or vehicle NYPD style!
			if(%player.ManualCommandTag)
			{	
				%player.ManualCommandTag = False;
				%player.CommandTag = "";
			}
			Player::setMountObject(%player, -1,0);
			Client::setControlObject(%client, %player);	
		}

}

function prisonguard(%client)
{
	%player = Client::getOwnedObject(%client);
	$jailed[%player] = "True";
	Anni::Echo(%client,%player," jailed ",$jailed[%player]);
}

//check to see if they're still in jail, or if someone hit switch and released them..
function JailBird(%client, %team)
{	
	%player = Client::getOwnedObject(%client);
	if(%player == -1 || Player::isDead(%player)) return;
	%playerPos = GameBase::getPosition(%player);
	%playerdist = Vector::getDistance($JailPosition[%Team], %playerpos);
	if(%playerdist > 10 && !$released[%player])
	{
		Client::SendMessage(%client, 0, "You've escaped! Run Forrest Run!!!!");	
		Messageallexcept(%client,0,Client::getName(%client)@" escaped from jail.");				
		$jailed[%player] = "";
		%player = Client::getOwnedObject(%client);	
	}
	if($jailed[%player] && !$released[%player])  schedule("JailBird(" @ %client @ ", " @ %team @ ");",0.25, %client);
}

function JailBreak(%client, %team)
{
	%player = Client::getOwnedObject(%client);
	if(%player == -1 || Player::isDead(%player)) return;
//	Anni::Echo("jail client "@%client@" player# "@%player@" in team's "@ %team@" jail");
	%playerPos = GameBase::getPosition(%player);
	%playerdist = Vector::getDistance($JailPosition[%Team], %playerpos);
	$jailed[%player] = ""; 
//	Anni::Echo("%playerdist "@%playerdist);
	if(%playerdist < 10){	
		%posX = getWord($JailPosition[%Team], 0);
		%posY = getWord($JailPosition[%Team], 1);
		%posZ = getWord($JailPosition[%Team], 2) + 10;
		%freeplayerpos = %posX@" "@%posY@" "@%posZ;
		GameBase::SetPosition(%client, %freeplayerpos);
		$released[%player] = true;
		schedule("$released[" @ %player @ "] = false;",5,%player);
		Client::SendMessage(%client, 0, "You Have Been Released, now get outa here!!!!~wshieldhit.wav");
		Messageallexcept(%client,0,Client::getName(%client)@" was paroled early from jail.");
		
	}
}

function JailWall::move(%this, %up)
{
	//Anni::Echo("up "@%this.Jailup@" down "@%this.Jaildown);
	if(%up)
	{
		%team = GameBase::getTeam(%this);
		%teleset = nameToID("MissionCleanup/JailWalls" @ %team);
		for(%i = 0; (%object = Group::getObject(%teleset, %i)) != -1; %i++)
		{	
			%pos = GameBase::getPosition(%object);
			%pos = Vector::add(%pos,"0 0 0.1");
			GameBase::setPosition(%object,%pos);
		}
		%this.Jailup = %this.Jailup -1;
		if(%this.Jailup)
			schedule("JailWall::move(" @ %this @ ",true);",0.05,%this);
		
	}
	else
	{
		%team = GameBase::getTeam(%this);
		%teleset = nameToID("MissionCleanup/JailWalls" @ %team);		
		for(%i = 0; (%object = Group::getObject(%teleset, %i)) != -1; %i++)
		{	
			%pos = GameBase::getPosition(%object);
			%pos = Vector::add(%pos,"0 0 -0.1");
			GameBase::setPosition(%object,%pos);
		}
		%this.Jaildown = %this.Jaildown -1;

		%Limit = getWord($JailPosition[%team],2) + 0.1;
		if(%this.Jaildown && getWord(%pos,2) > %Limit)schedule("JailWall::move(" @ %this @ ",false);",0.05,%this);	
		else %this.wait = false;	
	}
	
}

function JailTower::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Jail Tower: <f2>Deploys a team jail. Use the <f1>Jailers Gun<f2> to jail your enemies.");	
}

//whew..