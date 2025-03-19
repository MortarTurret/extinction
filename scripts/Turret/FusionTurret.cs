$InvList[FusionTurretPack] = 1;
$MobileInvList[FusionTurretPack] = 1;
$RemoteInvList[FusionTurretPack] = 1;
AddItem(FusionTurretPack);

$CanControl[DeployableFusionTurret] = 1;
$EmbedController[DeployableFusionTurret] = 1;
$CanAlwaysTeamDestroy[DeployableFusionTurret] = 1;

ItemImageData FusionTurretPackImage 
{
	shapeFile = "remoteturret";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData FusionTurretPack 
{
	description = "Fusion Turret";
	shapeFile = "remoteturret";
	className = "Backpack";
	heading = $InvHead[ihTur];
	imageType = FusionTurretPackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 650;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};


function FusionTurretPack::deployShape(%player,%item) 
{
	if(Turret::deployShape(%player, "Fusion Turret (" @ Client::getName(Player::getClient(%player)) @ ")", DeployableFusionTurret, %item, $TurretLocGroundOnly)&& !$build)
		Player::decItemCount(%player,%item);
}

TurretData DeployableFusionTurret
{
	className = "Turret";
	shapeFile = "hellfiregun";
	projectileType = Fusioncharge;
	maxDamage = 1.0;
	maxEnergy = 200;
	minGunEnergy = 75;
	maxGunEnergy = 6;
	sequenceSound[0] = {"deploy", SoundActivateMotionSensor };
	reloadDelay = 0.8;
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
	fireSound = SoundPlasmaTurretFire;
	activationSound = SoundPlasmaTurretOn;
	deactivateSound = SoundPlasmaTurretOff;
	whirSound = SoundPlasmaTurretTurn;
	explosionId = flashExpMedium;
	description = "Fusion Turret";
	damageSkinData = "objectDamageSkins";
};

function DeployableFusionTurret::onAdd(%this) 
{		
	schedule("DeployableFusionTurret::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,5);
	%this.shieldStrength = 0.010;
	if(GameBase::getMapName(%this) == "") 
		GameBase::setMapName (%this, "Fusion Turret");
}

function DeployableFusionTurret::deploy(%this) 
{
	GameBase::playSequence(%this,1,"deploy");
}

function DeployableFusionTurret::onEndSequence(%this,%thread) 
{
	GameBase::setActive(%this,true);
}

function DeployableFusionTurret::onDestroyed(%this) 
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
	$TeamItemCount[GameBase::getTeam(%this) @ "FusionTurretPack"]--;
}

function DeployableFusionTurret::onPower(%this,%power,%generator) 
{
}

function DeployableFusionTurret::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);
}

function FusionTurretPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player); 
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Fusion Turret: <f2>Fires balls of electrical energy which render enemy <f1>shields<f2>, <f1>jetpacks<f2>, and <f1>energy weapons<f2> useless.");	
}