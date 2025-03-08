$InvList[TurretPack] = 1;
$MobileInvList[TurretPack] = 1;
$RemoteInvList[TurretPack] = 1;
AddItem(TurretPack);


$CanAlwaysTeamDestroy[DeployableTurret] = 1;

ItemImageData TurretPackImage
{
	shapeFile = "remoteturret";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData TurretPack
{
	description = "Ion Turret";
	shapeFile = "remoteturret";
	className = "Backpack";
	heading = $InvHead[ihTur];
	imageType = TurretPackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 350;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};


function TurretPack::deployShape(%player,%item) 
{
	if(Turret::deployShape(%player, "Ion Turret (" @ Client::getName(Player::getClient(%player)) @ ")", DeployableTurret, %item, $TurretLocAnywhere)&& !$build)
		Player::decItemCount(%player,%item);
}

TurretData DeployableTurret
{
	className = "Turret";
	shapeFile = "remoteturret";
	projectileType = IonBolt;
	maxDamage = 0.65;
	maxEnergy = 60;
	minGunEnergy = 6;
	maxGunEnergy = 5;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	reloadDelay = 0.4;
	speed = 4.0;
	speedModifier = 1.5;
	range = 55;
	visibleToSensor = true;
	shadowDetailMask = 4;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = flashDebrisMedium;
	shieldShapeName = "shield";
	fireSound = SoundRemoteTurretFire;
	activationSound = SoundRemoteTurretOn;
	deactivateSound = SoundRemoteTurretOff;
	explosionId = flashExpMedium;
	description = "Remote Ion Turret";
	damageSkinData = "objectDamageSkins";
};

function DeployableTurret::onAdd(%this)
{
	schedule("DeployableTurret::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,5);
	%this.shieldStrength = 0.005;
	if(GameBase::getMapName(%this) == "") 
		GameBase::setMapName (%this, "Remote Turret");
}

function DeployableTurret::deploy(%this) 
{
	GameBase::playSequence(%this,1,"deploy");
}

function DeployableTurret::onEndSequence(%this,%thread) 
{
	GameBase::setActive(%this,true);
}

function DeployableTurret::onDestroyed(%this) 
{
	Turret::onDestroyed(%this);
	%this.OrgTeam = "";
	$TeamItemCount[GameBase::getTeam(%this) @ "TurretPack"]--;
}

function DeployableTurret::onPower(%this,%power,%generator) 
{
}

function DeployableTurret::onEnabled(%this)
{
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);
}

function TurretPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Ion Turret: <f2>Fires balls of electrical energy which render enemy <f1>shields<f2>, <f1>jetpacks<f2>, and <f1>energy weapons<f2> useless.");
}