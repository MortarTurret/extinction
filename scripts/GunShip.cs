$InvList[GunShipPack] = 1;
$MobileInvList[GunShipPack] = 1;
$RemoteInvList[GunShipPack] = 1;
AddItem(GunShipPack);


ItemImageData GunShipPackImage
{
	shapeFile = "anten_small";
	mountPoint = 2;
	mountOffset = { 0, -0.5, -0.25 };
	mountRotation = { 0, 0, 0 };
	mass = 2.0; //5.0
	maxDamage = 8.0;
	firstPerson = false;
};

ItemData GunShipPack
{
	description = "Gun Ship Beacon";
	shapeFile = "ammopack";
	className = "Backpack";
	heading = $InvHead[ihRem];
	shadowDetailMask = 4;
	imageType = GunShipPackImage;
	maxDamage = 8.0;
	mass = 0.5;
	elasticity = 0.2;
	price = 500000;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function GunShipPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Gun Ship: <f2>Deploy a Landing Pad to summon a spaceship with <f1>Inventory Stations<f2>, <f1>Generator<f2> and <f1>Plasma Turrets<f2>.\n\n<f1> Warning: <f2>Protect the Landing Pad until Gun Ships arrival from deep space.");	
}

function CreateGunShipSimSet(%team)
{
	%Baseset = nameToID("MissionCleanup/GunShip" @ %team);
	if(%Baseset == -1)
	{
		%group = newObject("GunShip" @ %team,SimGroup);
		addToSet("MissionCleanup",%group);
	}	
}

 StaticShapeData GunShipGen
 {
	description = "Gunship";
	shapeFile = "generator";
	classname = "Generator";
	debrisId = flashDebrisSmall;
	sfxAmbient = SoundGeneratorPower;
	maxDamage = 8.0;
	mapIcon = "M_generator";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	explosionId = flashExpMedium;
	visibleToSensor = true;
	mapFilter = 4;
	shieldShapeName = "shield";
 };

function GunShipPack::deployShape(%player,%item)
{
	%team = GameBase::getTeam(%player);
	%client = Player::getClient(%player);
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

	if(DropShip::deployShape(%player,"GunShip",%item,65,65,65,1))
	{
		%GSLocation = Vector::add(GameBase::getPosition(%player), "0 0 15");
		%BangLocation = Vector::add(GameBase::getPosition(%player), "0 0 500"); //515
		%BeaconSound1 = Vector::add(GameBase::getPosition(%player), "0 0 3");
		playSound(SoundDoorOpen,%BeaconSound1);

		$GSpot[%team] = %GSLocation;
		$ExplosionSpot[%team] = %BangLocation;
		Player::decItemCount(%player,%item);
		%pos = $DropShip[GunShip@%team];
		%pos = Vector::add(%pos,"0 0 500"); // 300
//		schedule("GunShip::DeployShape(\"" @ %pos @ "\", " @ %team @ ");", 10); //20.0 -death666
		schedule("GunShip::DeployShape(\"" @ %pos @ "\", " @ %team @ ");", 15); //20.0 -death666

//	       $ExplosionSpot2 = $ExplosionSpot[%team];

//	       schedule("LandingExp($ExplosionSpot);",13.0); //1.0


	}
}

function GunShip::deployShape(%pos,%team,%beacon)
{	
	if(gamebase::getposition($DropShipBeacon[GunShip@%team]) != "0 0 0")
	{
		%obj = DropShip::CreateShip("GunShip","gswdrop.dis",%pos,%team);
		gamebase::setrotation(%obj,gamebase::getrotation($DropShipBeacon[GunShip@%team]));
		%obj.inmotion = true;
		DropshipTeamMessage(%team, 1, "Gunship beginning de-orbit burn.");
		DropShip::MoveShip(%obj,2900,"GunShip");
	        $ExplosionSpot2 = $ExplosionSpot[%team];
		schedule("LandingExp($ExplosionSpot);",0.1); //1.0
	}
	else {
		$TeamItemCount[%team @ GunShipPack]--;
		DropshipTeamMessage(%team, 1, "Gunship Landing Pad has been destroyed too soon, the Gunship and crew were lost in space..");
		$DropShipMultipass1[%team] = "false";
		$DropShipMultipass2[%team] = "false";
	}
}

function GunShip::DeployItems(%this)
{
	%shippos = GameBase::GetPosition(%this);
	%team = GameBase::GetTeam(%this);
	%prot = gamebase::getrotation(%this);

	%group = nameToID("MissionCleanup/GunShip" @ %team);
	if(%group == -1)
	{
		%group = newObject("GunShip" @ %team,SimGroup);
		addToSet("MissionCleanup",%group);
	}

	%obj = newObject("Generator","StaticShape",GunShipGen,false); //StaticShape
	addToSet(%group, %obj);
	GameBase::setTeam(%obj,%team);
	%rot = "0 0 1.57079";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%obj,%rot);
	Gamebase::setMapName(%obj,%name @ "Gunship Generator");
	%pos = rotateVector("0 1.0 4.538",%prot);
	%pos = Vector::add(%shippos,%pos);
	GameBase::setPosition(%obj,%pos);
	GameBase::startFadeIn(%obj);
	GameBase::setActive(%obj,False); // True

	%obj=newObject("Sensor",Sensor,PulseSensor,false);
	addToSet(%group, %obj);
	GameBase::setTeam(%obj,%team);
	GameBase::setRotation(%obj,%prot);
	Gamebase::setMapName(%obj,%name @ "Gunship Pulse Sensor");
	%pos = rotateVector("0 12 22",%prot);
	%pos = Vector::add(%shippos,%pos);
	GameBase::setPosition(%obj,%pos);
	GameBase::startFadeIn(%obj);
	
	%obj = newObject("Inventory1","StaticShape",InventoryStation,false);
	addToSet(%group, %obj);
	GameBase::setTeam(%obj,%team);
	GameBase::setRotation(%obj,%prot);
	Gamebase::setMapName(%obj,%name @ "Inventory Station");
	%pos = rotateVector("-5.6 3.4 4.5",%prot);
	%pos = Vector::add(%shippos,%pos);
	GameBase::setPosition(%obj,%pos);
	GameBase::startFadeIn(%obj);

	%obj = newObject("Inventory1","StaticShape",InventoryStation,false);
	addToSet(%group, %obj);
	GameBase::setTeam(%obj,%team);
	GameBase::setRotation(%obj,%prot);
	Gamebase::setMapName(%obj,%name @ "Inventory Station");	
	%pos = rotateVector("5.6 3.4 4.5",%prot);
	%pos = Vector::add(%shippos,%pos);
	GameBase::setPosition(%obj,%pos);
	GameBase::startFadeIn(%obj);

	%obj = newObject("remoteturret","turret",PlasmaTurret,false);
	addToSet(%group, %obj);
	GameBase::setTeam(%obj,%team);
	%rot = "0 -2.84 3.14159";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%obj,%rot);
	Gamebase::setMapName(%obj,%name @ "Plasma Turret");
	%pos = rotateVector("28.85 1.7 15.7",%prot);
	%pos = Vector::add(%shippos,%pos);
	GameBase::setPosition(%obj,%pos);
	GameBase::startFadeIn(%obj);

	%obj = newObject("remoteturret","turret",PlasmaTurret,false);
	addToSet(%group, %obj);
	GameBase::setTeam(%obj,%team);
	%rot = "0 2.84 3.14159";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%obj,%rot);
	Gamebase::setMapName(%obj,%name @ "Plasma Turret");
	%pos = rotateVector("-28.85 1.7 15.7",%prot); // -24.85 1.7 15.7
	%pos = Vector::add(%shippos,%pos);
	GameBase::setPosition(%obj,%pos);
	GameBase::startFadeIn(%obj);

	%obj = newObject("remoteturret","turret",IndoorTurret,false);
	addToSet(%group, %obj);
	GameBase::setTeam(%obj,%team);
	%rot = "0 0 3.14073";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%obj,%rot);
	Gamebase::setMapName(%obj,%name @ "Indoor Turret");
	%pos = rotateVector("0 0.2 18",%prot);
	%pos = Vector::add(%shippos,%pos);
	GameBase::setPosition(%obj,%pos);
	GameBase::startFadeIn(%obj);

	%obj = newObject("remoteturret","turret",IndoorTurret,false);
	addToSet(%group, %obj);
	GameBase::setTeam(%obj,%team);
	%rot = "0 -3.14073 3.14073";
	%rot = Vector::add(%prot,%rot);
	GameBase::setRotation(%obj,%rot);
	Gamebase::setMapName(%obj,%name @ "Indoor Turret");
	%pos = rotateVector("-0.05 17.4 11.25",%prot);
	%pos = Vector::add(%shippos,%pos);
	GameBase::setPosition(%obj,%pos);
	GameBase::startFadeIn(%obj);
		
	%obj = newObject("RepairPack","Item","repairpack",1,true,true);	//rotates	
	addToSet(%group, %obj);
	%pos = rotateVector("0 12.8 13.5",%prot); // 0 13 21
	%pos = Vector::add(%shippos,%pos);
	GameBase::setPosition(%obj,%pos);

	playSound(SoundDoorClose,%pos);
	playSound(SoundPlasmaTurretOff,%pos);
	
	return true;
}

function GunShipGen::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	if($debug::Damage)
	{
		 Anni::Echo("GunShipGen::onDamage("@%this@", "@%type@", "@%value@", "@%pos@", "@%vec@", "@%mom@", "@%object@" )");
	}
	DropShipGen::onDamage(%this,%type,%value,%pos,%vec,%mom,%object);
}

function GunShipGen::onEnabled(%this)
{
	GameBase::playSequence(%this,0,"power");
	GameBase::generatePower(%this, true);
	GameBase::isPowerGenerator(%this);
}
 
function Generator::onDisabled(%this)
{
	GameBase::stopSequence(%this,0);
 	GameBase::generatePower(%this, false);
}

function GunShipGen::onDestroyed(%this)
{
	Generator::onDisabled(%this);
	%this.nuetron = "";
	StaticShape::objectiveDestroyed(%this);
}

function GunShipGen::setActive(%this)
{
	GameBase::playSequence(%this,0,"power");
	GameBase::generatePower(%this, true);
	GameBase::isPowerGenerator(%this);
}

function GunShipGen::onActivate(%this)
{
	GameBase::playSequence(%this,0,"power");
	GameBase::generatePower(%this, true);
	GameBase::isPowerGenerator(%this);
}

function GunShipGen::onDeactivate(%this)
{
	GameBase::stopSequence(%this,0);
 	GameBase::generatePower(%this, false);
}