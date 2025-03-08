$InvList[ProbeDroidPack] = 1;
$MobileInvList[ProbeDroidPack] = 1;
$RemoteInvList[ProbeDroidPack] = 1;
AddItem(ProbeDroidPack);

ItemImageData ProbeDroidPackImage
{
	shapeFile = "ammopack";
	weaponType = 2;
	mountPoint = 2;
	mountOffset = { 0, -0.1, 0 };
	minEnergy = 4;
 	maxEnergy = 4.5;
	firstPerson = false;
};

ItemData ProbeDroidPack
{
	description = "Probe Droid";
	shapeFile = "ammopack";
	className = "Backpack";
	heading = $InvHead[ihDro];
	shadowDetailMask = 4;
	imageType = ProbeDroidPackImage;
	mass = 0.5;
	price = 400;
	hudIcon = "energypack";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function ProbeDroidPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Probe Droid: <f2>Press PACK key to deploy and FIRE to shoot rockets.");
}

FlierData ProbeDroid
{	
	explosionId = flashExpSmall;
	debrisId = flashDebrisSmall;
	className = "Vehicle";
	shapeFile = "camera";
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
	maxAlt = 20000;
	maxVertical = 1;
	maxDamage = 0.0125;
	damageLevel = {1.0, 1.0};
	maxEnergy = 50;
	accel = 1.0;
	groundDamageScale = 1; //0.001
	projectileType = IrradiationBlast; //DisruptorBolt
	reloadDelay = 2;
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
	description = "Probe Droid";
};

function ProbeDroidPack::onUse(%player,%item)
{
	if(Player::getMountedItem(%player,$BackpackSlot) != %item) 
		Player::mountItem(%player,%item,$BackpackSlot);
	else Player::deployItem(%player,%item);
}

function ProbeDroidPack::onDeploy(%player,%item,%pos)
{
	if(DeployProbeDroid(%player,%item,ProbeDroid,flier,ProbeDroid)) 
		if(!Player::isDead(%player)&& !$build) 
			Player::decItemCount(%player,%item);
}

function ProbeDroid::onCollision(%this,%object)
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
			ProbeDroid::onDestroyed(%this);
		}
		else
		{
			ProbeDroid::onDestroyed(%this);
		}
	}
}

function ProbeDroid::jump(%this,%mom)
{	
	ProbeDroid::onDestroyed(%this);
	//GameBase::applyDamage(%this,$ImpactDamageType,10,GameBase::getPosition(%this),"0 0 0",%mom,%this);
}




function DeployProbeDroid(%player,%item,%shape,%data,%name)
{	
	%client = Player::getClient(%player);
	if($TeamItemCount[GameBase::getTeam(%player) @ "ProbeDroidPack"] < $TeamItemMax["ProbeDroidPack"])
	{
		%trans = GameBase::getMuzzleTransform(%client);
		%posX = getWord(%trans,9);
		%posY = getWord(%trans,10);
		%posZ = getWord(%trans,11); 
		%pos = %posX@" "@%posY@" "@%posZ;
		
		%rot = GameBase::getRotation(%player);
		%position = vector::add(Vector::getFromRot(%rot,1.5,0),%pos);
		if(GameBase::getLOSInfo(%player,2))
			%wall = true;

		%obj = newObject(ProbeDroid,flier,ProbeDroid,true);

		%obj.cloakable = true;
		addToSet("MissionCleanup/deployed/object",%obj);
		GameBase::setTeam(%obj,GameBase::getTeam(%player));
		GameBase::setPosition(%obj,%position);
		Vehicle::TerrainCheck(%obj);
		GameBase::setRotation(%obj,%rot);
		Gamebase::setMapName(%obj,"ProbeDroid");
		Client::sendMessage(%client, 0, "ProbeDroid Deployed");
		GameBase::startFadeIn(%obj);
		
		
		//GameBase::startFadeOut(%obj);
		playSound(SoundPickupBackpack,$los::position);
		Client::setControlObject(%client,%obj);
		%client.droid = %obj;
		%player.vehicle = %obj;
		$TeamItemCount[GameBase::getTeam(%player) @ "ProbeDroidPack"]++;
		
		if(%wall)
			schedule("GameBase::setDamageLevel("@%obj@",1);",2);
		
		return true;
	}
	else
	{
		Client::sendMessage(%client,0,"Maximum number of ProbeDroids deployed");
		return false;
	}
}

function ProbeDroid::onDestroyed(%this)
{
	%client = GameBase::getControlClient(%this);
	%player = Client::getOwnedObject(%client);
	Client::setControlObject(%client,%player);
	%client.droid=false;
	%this.cloakable = "";
	%this.nuetron = "";
	%player.vehicle = "";
	Client::sendMessage(%client,0,"Connection to ProbeDroid lost");
	$TeamItemCount[GameBase::getTeam(%this) @ "ProbeDroidPack"]--;
	GameBase::setDamageLevel(%this,1);
}
