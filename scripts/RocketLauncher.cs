$InvList[RocketLauncher] = 1;
$MobileInvList[RocketLauncher] = 1;
$RemoteInvList[RocketLauncher] = 1;

$InvList[RocketAmmo] = 1;
$MobileInvList[RocketAmmo] = 1;
$RemoteInvList[RocketAmmo] = 1;

$AutoUse[RocketLauncher] = false;
$SellAmmo[RocketAmmo] = 3;
$WeaponAmmo[RocketLauncher] = RocketAmmo;

addWeapon(RocketLauncher);
addAmmo(RocketLauncher, RocketAmmo, 1);

RocketData BasicRocket
{	
	bulletShapeName = "rocket.dts";
	explosionTag = rocketExp;
	collisionRadius = 0.0;
	mass = 2.0;
	damageClass = 1;
	damageValue = 0.5;
	damageType = $RocketDamageType;		
	explosionRadius = 20.5;
	kickBackStrength = 450.0;
	muzzleVelocity = 125.0;		//75
	terminalVelocity = 2000.0;
	acceleration = 200.0;
	totalTime = 8.5;
	liveTime = 18.0;
	lightRange = 5.0;
	lightColor = { 1.0, 0.7, 0.5 };
	inheritedVelocityScale = 0.5;
	trailType = 2;
	trailString = "rsmoke.dts";
	smokeDist = 1.8;
	soundId = SoundJetHeavy;
};

ItemData RocketAmmo 
{
	description = "Rockets";
	className = "Ammo";
	heading = $InvHead[ihAmm];
	shapeFile = "rocket";
	shadowDetailMask = 4;
	price = 50;
};

MineData RocketAmmoBomb
{
	mass = 5.0;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Halo";
	shapeFile = "rocket";
	shadowDetailMask = 4;
	explosionId = flashExpSmall;	//mineExp;
	explosionRadius = 5.0;
	damageValue = 0.0;	//0.5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 10.5;
};


GrenadeData RocketAmmoStray
{
	bulletShapeName = "rocket.dts";
	explosionTag = flashExpSmall;	//mortarExp;
	collideWithOwner = True;
	ownerGraceMS = 250;
	collisionRadius = 0.3;
	mass = 5.0;
	elasticity = 0.1;
	damageClass = 1; // 0 impact, 1, radius
	damageValue = 0.0;
	damageType = $MortarDamageType;
	explosionRadius = 10.0;
	kickBackStrength = 125.0;
	maxLevelFlightDist = 10;
	totalTime = 0.5;
	liveTime = 0.5;
	projSpecialTime = 0.01;
	inheritedVelocityScale = 0.5;
	smokeName = "rsmoke.dts";
};

ItemImageData RocketLauncherImage 
{
	shapeFile = "mortargun";
	mountPoint = 0;
	mountOffset = { 0, -0.1, -0.1 };
	weaponType = 0;
	ammoType = RocketAmmo;
	projectileType = BasicRocket;
	accuFire = true;
	reloadTime = 0.8; //BR Setting
	fireTime = 0.8; //BR Setting
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };
	sfxFire = SoundMissileTurretFire;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundMortarReload;
	sfxReady = SoundMortarIdle;
};

ItemData RocketLauncher 
{
	description = "Rocket Launcher";
	className = "Weapon";
	shapeFile = "mortargun";
	hudIcon = "mortar";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = RocketLauncherImage;
	price = 375;
	showWeaponBar = true;
};
ItemImageData RocketLauncherSightimage
{
	shapeFile = "force";
	mountPoint = 0;
	mountOffset = { 0, 0.34, -0.01 };	//0, 0.34, 0.03
	//mountRotation = { -1.57 ,0 ,0 };
	weaponType = 0;
	fireTime = 0.0;
};

ItemData RocketLauncherSight 
{
	description = "Rocket Launcher";
	className = "Weapon";
	shapeFile = "mortargun";
	hudIcon = "mortar";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = RocketLauncherSightimage;
	price = 375;
	showWeaponBar = true;
};

function RocketLauncher::MountExtras(%player,%weapon)
{		
	Player::mountItem(%player,RocketLauncherSight,4);
	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>Your basic Rocket Launcher.");
}
