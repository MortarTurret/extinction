$InvList[LaserTurretPack] = 1;
$MobileInvList[LaserTurretPack] = 1;
$RemoteInvList[LaserTurretPack] = 1;
AddItem(LaserTurretPack);

$CanAlwaysTeamDestroy[DeployableLaserTurret] = 1;

ItemImageData LaserTurretPackImage 
{
	shapeFile = "camera";
	mountPoint = 2;
	mountOffset = { 0, -0.1, -0.06 };
	mountRotation = { 0, 0, 0 };
	firstPerson = false;
};

ItemData LaserTurretPack 
{
	description = "Laser Turret";
	shapeFile = "camera";
	className = "Backpack";
	heading = $InvHead[ihTur];
	imageType = LaserTurretPackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 300;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function LaserTurretPack::deployShape(%player,%item,%pos) 
{
	if(Turret::deployShape(%player, "Laser Turret (" @ Client::getName(Player::getClient(%player)) @ ")", DeployableLaserTurret, %item, $TurretLocAnywhere)&& !$build)
		Player::decItemCount(%player,%item);
}

TurretData DeployableLaserTurret 
{
	className = "Turret";
	shapeFile = "camera";
	projectileType = TurretLaser;
	maxDamage = 0.6; //BR Setting
	maxEnergy = 200;
	minGunEnergy = 10;
	maxGunEnergy = 100;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	reloadDelay = 6.0;	//6.0
	speed = 2.0;		//100.0;// because the projectiles hit instantly,
	speedModifier = 2.15;	
	range = 70; //BR Setting
	visibleToSensor = true;
	shadowDetailMask = 4;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = flashDebrisMedium;
	shieldShapeName = "shield";
	fireSound = SoundFireLaser;
	activationSound = SoundRemoteTurretOn;
	deactivateSound = SoundRemoteTurretOff;
	explosionId = flashExpMedium;
	description = "Remote Laser Turret";
	damageSkinData = "objectDamageSkins";
};

function DeployableLaserTurret::onAdd(%this) 
{	
	schedule("DeployableLaserTurret::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,5);
	%this.shieldStrength = 0;
	if(GameBase::getMapName(%this) == "") 
		GameBase::setMapName (%this, "Remote Laser Turret");
}

function DeployableLaserTurret::deploy(%this) 
{	
		GameBase::playSequence(%this,1,"deploy");
}

function DeployableLaserTurret::onEndSequence(%this,%thread) 
{	
	GameBase::setActive(%this,true);
}

function DeployableLaserTurret::onDestroyed(%this) 
{	
	Turret::onDestroyed(%this);
	%this.OrgTeam = "";
	$TeamItemCount[GameBase::getTeam(%this) @ "LaserTurretPack"]--;
}

function DeployableLaserTurret::onPower(%this,%power,%generator) 
{
}

function DeployableLaserTurret::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);
}

function LaserTurretPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player); 
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Laser Turret:<f2> Shoots a laser that hits enemies instantly.");	
}