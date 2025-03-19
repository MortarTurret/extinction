$InvList[RocketPack] = 1;
$MobileInvList[RocketPack] = 1;
$RemoteInvList[RocketPack] = 1;
AddItem(RocketPack);

$CanControl[DeployableRocket] = 1; //changed from 0 to 1 -death666
$EmbedController[DeployableRocket] = 0; //changed from 0 to 1 -death666
$CanAlwaysTeamDestroy[DeployableRocket] = 1;

ItemImageData RocketPackImage 
{
	shapeFile = "remoteturret";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 3.0;
	firstPerson = false;
};

ItemData RocketPack 
{
	description = "Missile Turret";
//	shapeFile = "missileturret";
	shapeFile = "sensorjampack";
	className = "Backpack";
	heading = $InvHead[ihTur];
	imageType = RocketPackImage;
	shadowDetailMask = 4;
	mass = 3.0;
	elasticity = 0.2;
	price = 950;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function RocketPack::deployShape(%player,%item) 
{
	if(Turret::deployShape(%player, "Missile Turret (" @ Client::getName(Player::getClient(%player)) @ ")", DeployableRocket, %item, $TurretLocGroundOnly)&& !$build)
		Player::decItemCount(%player,%item);
}

TurretData DeployableRocket 
{
	className = "Turret";
	// shapeFile = "missileturret";
	shapeFile = "remoteturret";
	projectileType = TurretMissileDeployed; 
	maxDamage = 1.25; //BR Setting
	maxEnergy = 200; 
	minGunEnergy = 50; 
	maxGunEnergy = 100; 
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	reloadDelay = 3.5; 
	speed = 3.0; //BR Setting
	speedModifier = 2.5; //BR Setting
	range = 250; 
	visibleToSensor = true;
	shadowDetailMask = 4;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = flashDebrisMedium;
	shieldShapeName = "shield";
	fireSound = turretExplosion; 
	activationSound = SoundRemoteTurretOn;
	deactivateSound = SoundRemoteTurretOff;
	explosionId = flashExpMedium;
	description = "Remote Missile Turret";
	damageSkinData = "objectDamageSkins";
};


function DeployableRocket::verifyTarget(%this, %target) 
{
	if(GameBase::virtual(%target, "getHeatFactor") >= 0.5)
		return "True";
	else
		return "False";
}

function DeployableRocket::onAdd(%this) 
{
	schedule("DeployableRocket::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,2); // 5
	%this.shieldStrength = 0.015;
	if(GameBase::getMapName(%this) == "") 
		GameBase::setMapName (%this, "Remote Rocket");
}

function DeployableRocket::deploy(%this) 
{
	GameBase::playSequence(%this,1,"deploy");
}

function DeployableRocket::onEndSequence(%this,%thread) 
{
	GameBase::setActive(%this,true);
}

function DeployableRocket::onDestroyed(%this) 
{
	StaticShape::objectiveDestroyed(%this);
	%this.shieldStrength = 0;
	GameBase::setRechargeRate(%this,0);
	Turret::onDeactivate(%this);
	%this.cloakable = "";
	%this.nuetron = "";
	%this.OrgTeam = "";
	Turret::objectiveDestroyed(%this);
	if(!$NoCalcDamage)
		// CalcRadiusDamage(%this,$DebrisDamageType,20,0.2,25,20,20,2.5,1.1,200,100);
		calcRadiusDamage(%this, $DebrisDamageType, 7.5, 0.05, 25, 9, 3, 0.40, 0.1, 200, 100);
	$TeamItemCount[GameBase::getTeam(%this) @ "RocketPack"]--;
}

function DeployableRocket::onPower(%this,%power,%generator) 
{
}

function DeployableRocket::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,7);
	GameBase::setActive(%this,true);
}

function RocketPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Missile Turret:<f2> Fires homing missiles at jetting enemies and vehicles.\n<jc><f1>Warning: <f2>Only able to detect enemies within your teams sensor network.");	
}