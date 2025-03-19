$InvList[MortarTurretPack] = 1;
$MobileInvList[MortarTurretPack] = 1;
$RemoteInvList[MortarTurretPack] = 1;
AddItem(MortarTurretPack);

$CanControl[DeployableMortar] = 0;
$CanAlwaysTeamDestroy[DeployableMortar] = 1;

ItemImageData MortarTurretPackImage 
{
	shapeFile = "ammounit_remote";
	mountPoint = 2;
	mountOffset = { 0, -0.1, -0.06 };
	mountRotation = { 0, 0, 0 };
	firstPerson = false;
};

ItemData MortarTurretPack 
{
	description = "Mortar Turret";
//	shapeFile = "mortar_turret";
	shapeFile = "ammopack";
	className = "Backpack";
	heading = $InvHead[ihTur];
	imageType = MortarTurretPackImage;
	shadowDetailMask = 4;
	mass = 3.0;
	elasticity = 0.2;
	price = 800;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function MortarTurretPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player); 
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Mortar Turret:<f2> Shoots mortars at nearby enemies.");	
}

function MortarTurretPack::deployShape(%player,%item) 
{
	if(Turret::deployShape(%player, "Remote Mortar Turret (" @ Client::getName(Player::getClient(%player)) @ ")", DeployableMortar, %item, $TurretLocGroundOnly)&& !$build)
		Player::decItemCount(%player,%item);
}

TurretData DeployableMortar 
{
	className = "Turret";
	shapeFile = "mortar_turret";
	projectileType = MortarTurretShell;
	maxDamage = 1.5;
	maxEnergy = 45;
	minGunEnergy = 45;
	maxGunEnergy = 15;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	reloadDelay = 2;
	speed = 2;
	speedModifier = 1.5;
	range = 200; 
	visibleToSensor = true;
	shadowDetailMask = 4;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = flashDebrisMedium;
	shieldShapeName = "shield";
	fireSound = SoundMortarTurretFire;
	activationSound = SoundMortarTurretOn;
	deactivateSound = SoundMortarTurretOff;
	whirSound = SoundMortarTurretTurn;
	explosionId = LargeShockwave;
	description = "Remote Mortar Turret";
	damageSkinData = "objectDamageSkins";
};

function DeployableMortar::onAdd(%this) 
{
	schedule("DeployableMortar::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,7);
	%this.shieldStrength = 0.005;
	if(GameBase::getMapName(%this) == "") 
		GameBase::setMapName (%this, "Mortar Turret");
}

function DeployableMortar::deploy(%this) 
{
	GameBase::playSequence(%this,1,"deploy");
}

function DeployableMortar::onEndSequence(%this,%thread) 
{
	GameBase::setActive(%this,true);
}

function DeployableMortar::onDestroyed(%this) 
{
	Turret::onDestroyed(%this);
	%this.OrgTeam = "";
	$TeamItemCount[GameBase::getTeam(%this) @ "MortarTurretPack"]--;
}

function DeployableMortar::onPower(%this,%power,%generator) 
{
}

function DeployableMortar::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,7);
	GameBase::setActive(%this,true);
}