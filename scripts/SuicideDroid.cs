$InvList[SuicideDroidPack] = 1;
$MobileInvList[SuicideDroidPack] = 1;
$RemoteInvList[SuicideDroidPack] = 1;
AddItem(SuicideDroidPack);

ItemImageData SuicideDroidPackImage
{
	shapeFile = "ammopack";
	weaponType = 2;
	mountPoint = 2;
	mountOffset = { 0, -0.1, 0 };
	minEnergy = 4;
 	maxEnergy = 4.5;
	firstPerson = false;
};

ItemData SuicideDroidPack
{
	description = "Suicide Droid";
	shapeFile = "ammopack";
	className = "Backpack";
	heading = $InvHead[ihDro];
	shadowDetailMask = 4;
	imageType = SuicideDroidPackImage;
	mass = 0.5;
	price = 400;
	hudIcon = "energypack";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function SuicideDroidPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Suicide Droid: <f2>Press PACK key to deploy and FIRE to Detonate near unsespecting players.");
}

FlierData SuicideDroid
{
	explosionId = flashExpSmall;
	debrisId = flashDebrisSmall;
	className = "Vehicle";
	shapeFile = "camera"; // camera
	//shieldShapeName = "shield_medium";
	mass = 0.001;
	drag = 1.0;
	density = 1.2;
	maxBank = 5;
	maxPitch = 12.5;
	maxSpeed = 30; // 10
	//maxSideSpeed = 10;
	minSpeed = -10;
	lift = 1.0;
	maxAlt = 20000;	//40
	maxVertical = 1;
	maxDamage = 0.0125;
	damageLevel = {1.0, 1.0};
	maxEnergy = 50;
	accel = 1.0;
	groundDamageScale = 1; //0.001
	projectileType = suicideShell; // SuicideGren
	reloadDelay = 0.5;
	repairRate = 0;
	fireSound = SoundLaserHit;
	damageSound = SoundFlierCrash;
	ramDamage = 0.0001; //0.0001
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	visibleToSensor = true;
	shadowDetailMask = 2;
	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundDiscSpin;
	moveSound = SoundDiscSpin;
	visibleDriver = false;
	driverPose = 22;
	description = "Suicide Droid";
};

function SuicideDroidPack::onUse(%player,%item)
{
	if(Player::getMountedItem(%player,$BackpackSlot) != %item) 
		Player::mountItem(%player,%item,$BackpackSlot);
	else Player::deployItem(%player,%item);
}

function SuicideDroidPack::onDeploy(%player,%item,%pos)
{
	if(DeploySuicideDroid(%player,%item,SuicideDroid,flier,SuicideDroid)) 
		if(!Player::isDead(%player)&& !$build) 
			Player::decItemCount(%player,%item);
}

function SuicideDroid::onCollision(%this,%object)
{
	if($debug) 
		event::collision(%this,%object);

	%client = GameBase::getControlClient(%this);
	%player = Client::getOwnedObject(%client);

	if(getObjectType(%object) == "Player") 
	{
			SuicideDroid::onDestroyed(%this);
	}
}

function SuicideDroid::jump(%this,%mom)
{	
	SuicideDroid::onDestroyed(%this);
}

function DeploySuicideDroid(%player,%item,%shape,%data,%name)
{	
	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ "SuicideDroidPack"] < $TeamItemMax["SuicideDroidPack"])
	{
		%trans = GameBase::getMuzzleTransform(%client);
		%posX = getWord(%trans,9);
		%posY = getWord(%trans,10);
		%posZ = getWord(%trans,11) + 4.5; // +3.0 was 1.5
		%position = %posX@" "@%posY@" "@%posZ;
		%rot = GameBase::getRotation(%player);
		%obj = newObject(SuicideDroid,flier,SuicideDroid,true);

		%obj.cloakable = true;
		addToSet("MissionCleanup/deployed/object",%obj);
		GameBase::setTeam(%obj,GameBase::getTeam(%player));
		GameBase::setPosition(%obj,%position);
		Vehicle::TerrainCheck(%obj);
		GameBase::setRotation(%obj,%rot);
		Gamebase::setMapName(%obj,"SuicideDroid");
		Client::sendMessage(%client,0,"SuicideDroid Deployed");
		GameBase::startFadeIn(%obj);
		//GameBase::startFadeOut(%obj);
		playSound(SoundPickupBackpack,$los::position);
		Client::setControlObject(%client,%obj);
		%client.droid = %obj;
		%player.vehicle = %obj;
		$TeamItemCount[GameBase::getTeam(%player) @ "SuicideDroidPack"]++;
		return true;
	}
	else
	{
		Client::sendMessage(%client,0,"Maximum number of SuicideDroids deployed");
		return false;
	}
}

function SuicideDroid::onDestroyed(%this)
{	
	%client = GameBase::getControlClient(%this);
	%player = Client::getOwnedObject(%client);
	Client::setControlObject(%client,%player);
	%client.droid=false;
	%this.cloakable = "";
	%this.nuetron = "";
	%player.vehicle = "";
	Client::sendMessage(%client,0,"Connection to SuicideDroid lost");
	$TeamItemCount[GameBase::getTeam(%this) @ "SuicideDroidPack"]--;
	GameBase::setDamageLevel(%this,1);
}

