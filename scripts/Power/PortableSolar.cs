$InvList[PortableSolarPack] = 1;
$MobileInvList[PortableSolarPack] = 1;
$RemoteInvList[PortableSolarPack] = 1;

AddItem(PortableSolarPack);

$CanAlwaysTeamDestroy[PortableSolar] = 1;

ItemImageData PortableSolarPackImage 
{
	shapeFile = "solar_med"; //MagCargo
	mountPoint = 2;
	mountOffset = { 0, -0.65, -0.4 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData PortableSolarPack
{
	description = "Portable Solar Panel";
	shapeFile = "ammopack"; // solar_med
	classname = "Backpack";
	heading = $InvHead[ihPwr];
	imageType = PortableSolarPackImage;
	shadowDetailMask = 4;
	mass = 1;
	elasticity = 0.2;
	price = 500;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function PortableSolarPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Portable Solar Panel: <f2>Powers: <f1>Base Inventories, Base Turrets and Base Sensors plus Airbases and Gunships. \n<jc><f1>Usage: <f2>Deploy at any point to backup Base Assets Power. Deploy after setting up a Air Base or Gunship to back their power up.");
}

StaticShapeData PortableSolar
{
	description = "Portable Solar Panel";
	shapeFile = "solar_med"; // solar_med solar
	classname = "Generator";
	debrisId = flashDebrisSmall;
	maxDamage = 8.5; // been 10.5
	mapIcon = "M_station";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	explosionId = flashExpMedium;
	visibleToSensor = true;
	mapFilter = 4;
	sfxAmbient = SoundGeneratorPower;
};

function PortableSolarPack::deployShape(%player,%item)
{
	%team = GameBase::getTeam(%player);
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
	%playerPos = GameBase::getPosition(%player);
	
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item])
	{
		Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
		return false;
	}
	if(!GameBase::getLOSInfo(%player,5)) 
	{
		Client::sendMessage(%client,0,"Deploy position out of range");
		return false;
	}
	%obj = getObjectType($los::object);
	if(%obj != "SimTerrain") 
	{
		Client::sendMessage(%client,0,"Can only deploy on terrain");
		return false;
	}

	%obj = $los::object;
	if(%obj.inmotion == true)	 
	{ 
		Client::sendMessage(%client,0,"Deploy area crappy, cannot deploy.");
		return false;
	}

// new start
	%flagpos = $teamFlag[%team].originalPosition;	
	if(Vector::getDistance(%flagpos, %playerpos) > 450)
	{
		Client::sendMessage(%client,0,"You are too far from your flag to deploy the Solar Panel.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
// new end

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

	if(Vector::dot($los::normal,"0 0 1") <= 0.7)
	{
		Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
		return false;
	}
	if(!checkDeployArea(%client,$los::position)) 
	{
		Client::sendMessage(%client,0,"Cannot deploy here");
		return false;
	}
	%obj = newObject("Solar Panel","StaticShape","PortableSolar",true);
	%obj.deployer = %client;

	%obj.cloakable = true;
	if($debug) Anni::Echo($Ver,"|Created New Object :",%obj," ","Solar Panel");

	
	GameBase::setTeam(%obj,%team);

	addToSet("MissionCleanup/deployed/power", %obj);


	GameBase::setPosition(%obj,$los::position);
	GameBase::setRotation(%obj,GameBase::getRotation(%player));
	Gamebase::setMapName(%obj,"Solar Panel");
	Client::sendMessage(%client,0,"Solar Panel deployed");
	playSound(SoundPickupBackpack,$los::position);
	messageTeamExcept(%client, 3, "A team Solar Panel generator has been deployed by "@Client::getName(%client)@".");
	%team = GameBase::getTeam(%player);
	$PortableSP[%team] = "true";
	playSound(SoundCreateItem,$los::position);
	$TeamItemCount[%team @ %item]++;
	return true;
}

function PortableSolar::onAdd(%this)
{
	$StaticShape::count += 1;
	%team = GameBase::getTeam(%this);
	$PortableSP[%team] = "true";
}

function PortableSolar::onDestroyed(%this)
{
	PortableSolar::onDisabled(%this);
	$PortableSP[%team] = "false";
	StaticShape::objectiveDestroyed(%this);
	%this.cloakable = false;
	%this.nuetron = "";
	calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 3, 0.55, 0.30, 250, 170);
	$TeamItemCount[GameBase::getTeam(%this) @ "PortableSolarPack"]--;

	%team = GameBase::getTeam(%this);

	if($PortableGeneratedPower[%team] == true)
	{
//	messageAll(0, "Portable Solar Panel destroyed and port gen power is true");
//	DropshipTeamMessage(%team, 1, "A Team Portable Solar Panel has been destroyed. Portable Generator remains.");
	return;	
	}

 	Ann::UndoPowering(%this,false);	
}

function PortableSolar::onEnabled(%this)
{
	if($trace) Anni::Echo($ver,"| PortableSolor::onEnabled ",%this);
	GameBase::setActive(%this,true);
	GameBase::playSequence(%this,0,"power");
	GameBase::generatePower(%this, true);

	schedule("Ann::Powering("@%this@",true);",1,%this);
	GameBase::isPowerGenerator(%this);
	%team = GameBase::getTeam(%this);
	$PortableSP[%team] = "true";
}

function PortableSolar::onDisabled(%this)
{
	if($debug) Anni::Echo($ver,"|PortableSolar::onDisabled");
	GameBase::stopSequence(%this,0);
//	Ann::UndoPowering(%this,false);	
	GameBase::generatePower(%this, false);
	%team = GameBase::getTeam(%this);
	$PortableSP[%team] = "false";
}

function PortableSolar::onRemove(%this)
{
	GameBase::generatePower(%this, false);
	%team = GameBase::getTeam(%this);
	$PortableSP[%team] = "false";
}

function PortableSolar::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	if($debug)
		 Anni::Echo("PortableSolar::onDamage "@%this);
	
	if(GameBase::getDamageState(%this) == "Destroyed" || %value <= 0)
	 	return;

	%GunShipDropTeam = GameBase::getTeam(%this);
	%client = Player::getClient(%object);
	%AttackingGSTeam = GameBase::getTeam(%client);
	%name = GameBase::getDataName(%this);
	%player = Client::getOwnedObject(%client);
	%damageLevel = GameBase::getDamageLevel(%this);
	%pos = GameBase::getPosition(%this);

// NNNNEEWWWWW

		if(%AttackingGSTeam == %GunShipDropTeam)
		{
			%dValue = %damageLevel + %value * $Server::TeamDamageScale;
			GameBase::setDamageLevel(%this,%dValue);
		}
		if(%AttackingGSTeam != %GunShipDropTeam)
		{
			%dValue = %damageLevel + (%value/2);	
			GameBase::setDamageLevel(%this,%dValue);
		}

//NNNNEEEWWW
	
	if(GameBase::getDamageState(%this) == "Destroyed") 
	{
		StaticShape::objectiveDestroyed(%this);
		PortableSolar::onDisabled(%this);
		//gen damage
		calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 3, 0.55, 0.30, 250, 170);			
		GameBase::playSound(%this, SoundrocketExplosion, 0);
		
		%DropTeam = GameBase::getTeam(%this);
		%client = Player::getClient(%object);
		%KillerTeam = GameBase::getTeam(%client);
//		 Anni::Echo("destroy portable gen");	
		if(%KillerTeam == %DropTeam)
		{
			bottomprint(%client, "<jc>You have just LOST <f2>2<f0> POINTS for destroying your teams Solar Panel."); // "@ %name.className	
	
//			 Anni::Echo("MSG: ", %client, " Destroyed a team ",  %name.description);
			DropshipTeamMessage(%KillerTeam, 3, "Team member "@Client::getName(%client)@" has destroyed your teams Solar Panel."); // "@ %name.description);
			%client.score-=2;
			Game::refreshClientScore(%client);		
		}
		else
		{
			if(%this.LastRepairCl !=  %client)
			{	
				bottomprint(%client, "<jc>You have just recieved <f2>2<f0> POINTS for destroying the enemys Solar Panel."); // "@ %name.description);
				%client.score+= 2;
				%client.Credits+= 2;
				Game::refreshClientScore(%client);			
			}	
//			 Anni::Echo("MSG: ", %client, " Destroyed enemy  ",  %name.description);
			DropshipTeamMessage(%DropTeam, 3, "WARNING "@Client::getName(%client)@" has destroyed your teams Solar Panel."); // 's "@ %name.description);
		}					
	}	
}