$InvList[SurveyDroidPack] = 1;
$MobileInvList[SurveyDroidPack] = 1;
$RemoteInvList[SurveyDroidPack] = 1;
AddItem(SurveyDroidPack);

ItemImageData SurveyDroidPackImage
{
	shapeFile = "ammopack";
	weaponType = 2;
	mountPoint = 2;
	mountOffset = { 0, -0.1, 0 };
	minEnergy = 4;
 	maxEnergy = 4.5;
	firstPerson = false;
};

ItemData SurveyDroidPack
{
	description = "Survey Droid";
	shapeFile = "ammopack";
	className = "Backpack";
	heading = $InvHead[ihDro];
	shadowDetailMask = 4;
	imageType = SurveyDroidPackImage;
	mass = 0.5;
	price = 200;
	hudIcon = "energypack";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function SurveyDroidPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Survey Droid: <f2>Press PACK key to deploy and survey enemy activity.");
}

FlierData SurveyDroid
{
	explosionId = flashExpSmall;
	debrisId = flashDebrisSmall;
	className = "Vehicle";
	shapeFile = "camera"; // camera 
	//shieldShapeName = "shield_medium";
	mass = 0.001;
	drag = 1.0;
	density = 1.2;
	maxBank = 7;
	maxPitch = 12.5;
	maxSpeed = 50; // 30
	//maxSideSpeed = 100;
	minSpeed = -15;
	lift = 1.0;
	maxAlt = 20000;
	maxVertical = 1;
	maxDamage = 0.0125;
	damageLevel = {1.0, 1.0};
	maxEnergy = 50;
	accel = 1.0;
	groundDamageScale = 1; //0.001
	weaponType = 1;
	minEnergy = 5;
	maxEnergy = 7;
	repairRate = 0;
	//fireSound = SoundFireBlaster;
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
	description = "Survey Droid";
};

function SurveyDroidPack::onUse(%player,%item)
{
	if(Player::getMountedItem(%player,$BackpackSlot) != %item) 
		Player::mountItem(%player,%item,$BackpackSlot);
	else Player::deployItem(%player,%item);
}

function SurveyDroidPack::onDeploy(%player,%item,%pos)
{
	if(DeploySurveyDroid(%player,%item,SurveyDroid,flier,SurveyDroid)) 
		if(!Player::isDead(%player)&& !$build) 
			Player::decItemCount(%player,%item);
}

function SurveyDroid::onCollision(%this,%object)
{	
	if($debug) 
		event::collision(%this,%object);

	%client = GameBase::getControlClient(%this);
	%player = Client::getOwnedObject(%client);
	if(getObjectType(%object) == "Player") 
	{
		if(GameBase::getTeam(%object)==GameBase::getTeam(%this))
		{
			playNextAnim(%client);
			Player::kill(%client);
			Client::onKilled(%client,%client);
			SuicideDroid::onDestroyed(%this);
		}
		else
		{
			SuicideDroid::onDestroyed(%this);
		}
	}
}

function SurveyDroid::jump(%this,%mom)
{
	SurveyDroid::onDestroyed(%this);
	//GameBase::applyDamage(%this,$ImpactDamageType,10,GameBase::getPosition(%this),"0 0 0",%mom,%this);
}

function DeploySurveyDroid(%player,%item,%shape,%data,%name)
{
	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ "SurveyDroidPack"] < $TeamItemMax["SurveyDroidPack"])
	{	
		%trans = GameBase::getMuzzleTransform(%client);
		%posX = getWord(%trans,9);
		%posY = getWord(%trans,10);
		%posZ = getWord(%trans,11) + 1.5; // +3.0
		%position = %posX@" "@%posY@" "@%posZ;
		%rot = GameBase::getRotation(%player);
		%obj = newObject(SurveyDroid,flier,SurveyDroid,true);

		%obj.cloakable = true;
		addToSet("MissionCleanup/deployed/object",%obj);
		GameBase::setTeam(%obj,GameBase::getTeam(%player));
		GameBase::setPosition(%obj,%position);
		Vehicle::TerrainCheck(%obj);
		GameBase::setRotation(%obj,%rot);
		Gamebase::setMapName(%obj,"SurveyDroid");
		Client::sendMessage(%client, 0, "SurveyDroid Deployed");
		GameBase::startFadeIn(%obj);
//		GameBase::startFadeOut(%obj);
		playSound(SoundPickupBackpack,$los::position);
		Client::setControlObject(%client,%obj);
		%client.droid = %obj;
		%player.vehicle = %obj;
		$TeamItemCount[GameBase::getTeam(%player) @ "SurveyDroidPack"]++;
		return true;
	}
	else
	{
		Client::sendMessage(%client,0,"Maximum number of SurveyDroids deployed");
		return false;
	}
}

function SurveyDroid::onDestroyed(%this)
{
	%client = GameBase::getControlClient(%this);
	%player = Client::getOwnedObject(%client);
	Client::setControlObject(%client,%player);
	%client.droid=false;
	%player.vehicle = "";
	%this.cloakable = "";	
	%this.nuetron = "";
	Client::sendMessage(%client,0,"Connection to SurveyDroid lost");
 	$TeamItemCount[GameBase::getTeam(%this) @ "SurveyDroidPack"]--;
	GameBase::setDamageLevel(%this,1);
}

function SurveyDroid::onFire(%this,%slot)
{		
	if($debug)
		echo("?? EVENT fire survey droid, "@ %this @" control cl# "@ gamebase::getcontrolclient(%this));
}
