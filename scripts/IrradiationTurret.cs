$InvList[IrradiationTurretPack] = 1;
$MobileInvList[IrradiationTurretPack] = 1;
$RemoteInvList[IrradiationTurretPack] = 1; // changed from 0 to 1 -death666
AddItem(IrradiationTurretPack);

$CanControl[IrradiationTurret] = 1;
$EmbedController[IrradiationTurret] = 1;
$CanAlwaysTeamDestroy[IrradiationTurret] = 1;

ItemImageData IrradiationTurretPackImage 
{
	shapeFile = "remoteturret";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData IrradiationTurretPack
{
	description = "Irradiation Cannon";
	shapeFile = "remoteturret";
	className = "Backpack";
	heading = $InvHead[ihTur];
	imageType = IrradiationTurretPackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 2250; //changed from 3050 -death666
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function IrradiationTurretPack::deployShape(%player,%item) 
{
	if(Turret::deployShape(%player, "Irradiation Cannon (" @ Client::getName(Player::getClient(%player)) @ ")", IrradiationTurret, %item, $TurretLocGroundOnly)&& !$build)
		Player::decItemCount(%player,%item);
}

TurretData IrradiationTurret 
{
	className = "Turret";
	shapeFile = "hellfiregun";
	projectileType = IrradiationBlast;
	maxDamage = 1.0;
	maxEnergy = 200;
	minGunEnergy = 75;
	maxGunEnergy = 6;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	reloadDelay = 0.5;  // 0.05
	aimDeflection = 0.0004;
	speed = 4.0;
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
	fireSound = SoundFireMortar;
	activationSound = SoundPlasmaTurretOn;
	deactivateSound = SoundPlasmaTurretOff;
	whirSound = SoundPlasmaTurretTurn;
	explosionId = flashExpMedium;
	description = "Irradiation Cannon";
	damageSkinData = "objectDamageSkins";
};

function IrradiationTurret::onAdd(%this) 
{
	schedule("IrradiationTurret::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,10);
	%this.shieldStrength = 0.005;
	if(GameBase::getMapName(%this) == "") 
		GameBase::setMapName (%this, "Irradiation Cannon");
}

function IrradiationTurret::deploy(%this) 
{
	GameBase::playSequence(%this,1,"deploy");
}

function IrradiationTurret::onEndSequence(%this,%thread) 
{
	GameBase::setActive(%this,true);
}

function IrradiationTurret::onDestroyed(%this) 
{
	StaticShape::objectiveDestroyed(%this);
	%this.shieldStrength = 0;
	%this.cloakable = "";
	%this.nuetron = "";
	%this.OrgTeam = "";
	GameBase::setRechargeRate(%this,0);
	Turret::onDeactivate(%this);
	Turret::objectiveDestroyed(%this);
	if(!$NoCalcDamage)
		CalcRadiusDamage(%this,$DebrisDamageType,20,0.2,25,20,20,2.5,1.1,200,100);
	$TeamItemCount[GameBase::getTeam(%this) @ "IrradiationTurretPack"]--;
}

function IrradiationTurret::onPower(%this,%power,%generator) 
{
}

function IrradiationTurret::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,10);
	GameBase::setActive(%this,true);
}

function IrradiationTurretPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Irradiation Cannon: <f2>Rapidly fires missiles.");	
}