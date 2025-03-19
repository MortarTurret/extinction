$InvList[BunkerPack] = 1;
$MobileInvList[BunkerPack] = 1;
$RemoteInvList[BunkerPack] = 0;
AddItem(BunkerPack);

ItemImageData BunkerPackImage
{
	shapeFile = "ammopack";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 2.0;
	firstPerson = false;
};

ItemData BunkerPack
{
	description = "Deployable Bunker";
	shapeFile = "ammopack";
	className = "Backpack";
	heading = $InvHead[ihRem];
	imageType = BunkerPackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	price = 1000;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

MineData BombHellHalo
{
	mass = 5.0;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Halo";
	shapeFile = "force";
	shadowDetailMask = 4;
	explosionId = LargeShockwave;
	explosionRadius = 6.0;
	damageValue = 20.0;	//0.5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 0.5;
};


function BunkerPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player); 
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Bunker: <f2>deployable bunker with embrasures to fire out of.<f1> Beware: <f2>damage to outside panels may cause structural issues.");	
}


function BunkerCountObjects(%set,%name,%num) 
{
	%count = 0;
	for(%i=0;%i<%num;%i++) 
	{	%obj=Group::getObject(%set,%i);
		if(GameBase::getDataName(Group::getObject(%set,%i)) == %name) 
			%count++;
	}
	return %count;
}

function BunkerPack::deployShape(%player,%item)
{
	%client = Player::getClient(%player);
	%team = GameBase::getTeam(%player);
	%playerPos = GameBase::getPosition(%player);

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

	$BunkerDestroyed[%team] = "";

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]) 
	{
		Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
		return false;
	}
	if(!GameBase::getLOSInfo(%player,5)) 
	{
		Client::sendMessage(%client,0,"Deploy position out of range.");
		return false;
	}
	%obj = getObjectType($los::object);
	if(%obj != "SimTerrain") 
	{
		Client::sendMessage(%client,0,"Can only deploy on terrain.");
		return false;
	}
	%set = newObject("set",SimSet);
	%num = containerBoxFillSet(%set,$SimInteriorObjectType,$los::position,20,20,20,0);
	%num = BunkerCountObjects(%set,"eround1",%num);
	deleteObject(%set);
	if(20 < %num) 
	{
		Client::sendMessage(%client,0,"Too close to other objects.");
		return false;
	}
	%set = newObject("set",SimSet);
	%num = containerBoxFillSet(%set,$SimInteriorObjectType,$los::position,20,20,20,0);
	%num = BunkerCountObjects(%set,"eround1",%num);
	deleteObject(%set);
	if(0 != %num) 
	{
		Client::sendMessage(%client,0,"Too close to other objects.");
		return false;
	}

	if(%player.outArea)
	{
		Client::sendMessage(%client,0,"Can Not Deploy Bunker out of bounds");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}

// check for deployed ships.

	if(getNumTeams()-1 == 2)
	{
	%teamnumzero = 0;
	%teamnummone = 1;
	}
	
if(((($DropShipMultipass1[%teamnumzero] == true) || ($DropShipMultipass2[%teamnumzero] == true) || ($DropShipMultipass1[%teamnummone] == true) || ($DropShipMultipass2[%teamnummone] == true))))
{
	%clientId = Player::getClient(%player);
	%strekpos = $DropShipLocation[%teamnumzero @ "GunShip"];
	%strekpos2 = $DropShipLocation[%teamnummone @ "GunShip"];
	if(((Vector::getDistance(%strekpos, %playerpos)) < 50) && (%strekpos != "")) 
	{
		Client::sendMessage(%clientId,0,"You are too close to a Starwolf Gunship landing zone or crash site. ~waccess_denied.wav");
		return false;
	}
	if(((Vector::getDistance(%strekpos2, %playerpos)) < 50) && (%strekpos2 != "")) 
	{
		Client::sendMessage(%clientId,0,"You are too close to a Starwolf Gunship landing zone or crash site. ~waccess_denied.wav");
		return false;
	}
}

//	if(getNumTeams()-1 == 2)
//	{
//		if(%team == 0)
//			%enemyTeam = 1;
//		else if(%team == 1)
//			%enemyTeam = 0;
//		if(((Vector::getDistance($teamFlag[%enemyTeam].originalPosition, $los::position)) < ($FlagDistance * 0.4)) && ($FlagDistance != 0)) 
//		{
//			Client::sendMessage(%client,0,"You are too far from your flag to deploy the bunker.");
//			Client::sendMessage(%client,0,"~wC_BuySell.wav");
//			return false;
//		}
//	}

// new start
	%flagpos = $teamFlag[%team].originalPosition;	
	if(Vector::getDistance(%flagpos, %playerpos) > 450)
	{
		Client::sendMessage(%client,0,"You are too far from your flag to deploy the bunker.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
// new end
	if(Vector::dot($los::normal,"0 0 1") <= 0.7)
	{
		Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
		return false;
	}
	if(!BnkrDeployArea(%client,$los::position))
	{
		return false;
	}
	%rot = GameBase::getRotation(%player);
	%vctr = "0 0 3.3";
	%rot = Vector::add(%rot,%vctr);
	%pos = $los::position;

	%numBunkerObjects = -1;
	$BunkerDestroyed[%team] = false;
	$BunkerPos[%team] = %playerPos;

	%group = nameToID("MissionCleanup/DeployedBunker" @ %team);
	if(%group == -1)
	{
		%group = newObject("DeployedBunker" @ %team,SimGroup);
		addToSet("MissionCleanup",%group);
	}

	%object = newObject("DeployedBunker","InteriorShape", "eround.dis");
	$BunkerObject[%team,%numBunkerObjects++] = %object;
	addToSet(%group, %object);
	GameBase::setTeam(%object,%team);
	GameBase::setPosition(%object,%pos);
	GameBase::setRotation(%object,%rot);	
		
	%object = newObject("DeployedBunker","StaticShape",BunkerPanelBoom,false);
	Ann::DeployRotate(%object,%pos,"3.2688 1.86615 2",%Rot,"0 0 1.95997",%team,"Bunker");
	$BunkerObject[%team,%numBunkerObjects++] = %object;
	addToSet(%group, %object);
	GameBase::setTeam(%object,%team);
		
	%object = newObject("DeployedBunker","StaticShape",BunkerPanelBoom,false);
	Ann::DeployRotate(%object,%pos,"-3.21266 -1.96599 2",%Rot,"0 -0 1.95996",%team,"Bunker");
	$BunkerObject[%team,%numBunkerObjects++] = %object;
	addToSet(%group, %object);
	GameBase::setTeam(%object,%team);	
		
	%object = newObject("DeployedBunker","StaticShape",BunkerPanelBoom,false);
	Ann::DeployRotate(%object,%pos,"3.19522 -1.92592 2",%Rot,"0 -0 1.17996",%team,"Bunker");
	$BunkerObject[%team,%numBunkerObjects++] = %object;
	addToSet(%group, %object);
	GameBase::setTeam(%object,%team);
		
	%object = newObject("DeployedBunker","StaticShape",BunkerPanelBoom,false);
	Ann::DeployRotate(%object,%pos,"-2.16143 3.12258 2",%Rot,"0 -0 0.379997",%team,"Bunker");
	$BunkerObject[%team,%numBunkerObjects++] = %object;
	addToSet(%group, %object);
	GameBase::setTeam(%object,%team);
	Gamebase::setMapName(%object,"MrTsays");

	Client::sendMessage(%client,0,"Bunker Deployed");
	playSound(SoundPickupBackpack,$los::position);
	$TeamItemCount[GameBase::getTeam(%player) @ "BunkerPack"]++;
	Anni::Echo("MSG: ",%client," deployed a bunker.");
	return true;
}

StaticShapeData BunkerPanelBoom
{
	shapeFile = "panel_set";
	debrisId = flashDebrisSmall;
	explosionId = flashExpMedium;
	maxDamage = 70;
	damageSkinData = "objectDamageSkins";
	description = "JailTower";
};

function BunkerPanelBoom::onDestroyed(%this)
{
	DestroyBunker(%this); 
}

function DestroyBunker(%this)
{
	%team = GameBase::getTeam(%this);
	if($BunkerDestroyed[%team]) return;
	else $BunkerDestroyed[%team] = true;

	%group = nameToID("MissionCleanup/DeployedBunker" @ %team);
//		messageAll(0, "destroy bunker ran");


	for(%i = 0; $BunkerObject[%team,%i] != ""; %i++)
	{
		%obj = $BunkerObject[%team,%i];
		%dat = GameBase::getDataName(%obj);
		%name = GameBase::getMapName(%obj);
		%type = GetObjectType(%obj);

		if(%obj == %this || !isObject(%obj))
		{	//do nothing
		}

		if(Gamebase::getMapName(%obj) == "MrTsays")
		{
//		messageAll(0, "it did something cool");
		}

		if(%type == "InteriorShape")
		{
//			deleteObject(%obj);
			schedule("deleteObject("@ %obj @");",0.2,%obj);
		}
		if(%type == "StaticShape")
		{
//			messageAll(0, "static shape ran");
			GameBase::stopSequence(%this,0);
			StaticShape::objectiveDestroyed(%this);
			if(!$NoCalcDamage)
			calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 2, 0.40, 0.1, 250, 100);
			schedule("deleteObject("@ %obj @");",0.2,%obj);
		}
		else
		{
			GameBase::setDamageLevel(%obj,9999);
			schedule("deleteObject("@ %obj @");",0.2,%obj);
		}
	}
	BunkerExplosion($BunkerPos[%team]);
	$BunkerDestroyed[%team] = true; // -death666 3.29.17

	$BunkerPos[%team] = "";
	deleteVariables("$BunkerObject[%team,*");

	$TeamItemCount[GameBase::getTeam(%this) @ BunkerPack]--;
}

function BunkerExplosion(%pos)
{
//		messageAll(0, "it did the explosion");
	%SmokeAgain = Vector::add(%pos,"1 1 1");
	%SmogSmoke1 = NewObject("",Mine,"BombHellHalo");
	AddToSet("MissionCleanup",%SmogSmoke1);
	GameBase::setPosition(%SmogSmoke1,%SmokeAgain);
	GameBase::setRotation(%SmogSmoke1,"0 0 0");
	schedule("Mine::Detonate(" @ %SmogSmoke1 @ ");",0.2);
}

function BnkrDeployArea(%client,%pos)
{
	%set=newObject("set",SimSet);
	%num=containerBoxFillSet(%set,$StaticObjectType | $ItemObjectType | $SimPlayerObjectType,%pos,0.5,0.5,0.5,0.5);
	if(!%num) 
	{
		deleteObject(%set);
		return 1;
	}
	else if(%num == 1 && getObjectType(Group::getObject(%set,0)) == "Player") 
	{ 
		%obj = Group::getObject(%set,0);
		deleteObject(%set);
		if(Player::getClient(%obj) == %client)	
			Client::sendMessage(%client,0,"Unable to deploy - You're in the way");
		else
			Client::sendMessage(%client,0,"Unable to deploy - Player in the way");
	}
	else
		Client::sendMessage(%client,0,"Unable to deploy - Item in the way");
	deleteObject(%set);
	return 0;
}

function Ann::DeployRotate(%this,%deployPos,%vec,%Vrot,%rot,%team,%name)
{
	gamebase::setposition(%this,vector::add(rotateVector(%vec,%Vrot),%deployPos));
	GameBase::setRotation(%this,vector::add(%Vrot,%rot));	
}