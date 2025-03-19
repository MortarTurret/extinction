$InvList[Disclauncher] = 1;
$MobileInvList[Disclauncher] = 1;
$RemoteInvList[Disclauncher] = 1;

$InvList[DiscAmmo] = 1;
$MobileInvList[DiscAmmo] = 1;
$RemoteInvList[DiscAmmo] = 1;

$SellAmmo[DiscAmmo] = 5;
$WeaponAmmo[DiscLauncher] = DiscAmmo;

addWeapon(DiscLauncher);
addAmmo(DiscLauncher, DiscAmmo, 2);

RocketData DiscShell
{
	bulletShapeName = "discb.dts";
	explosionTag = rocketExp;
	collisionRadius = 0.0;
	mass = 2.0;
	damageClass = 1; // 0 impact, 1, radius
	damageValue = 0.5;
	damageType = $ExplosionDamageType;
	explosionRadius = 7.5;
	kickBackStrength = 150.0;
	muzzleVelocity = 65.0; //65.0 //BR Setting = 75.0;
	terminalVelocity = 80.0;
	acceleration = 5.0;
	totalTime = 6.5;
	liveTime = 8.0;
	lightRange = 5.0;
	lightColor = { 0.4, 0.4, 1.0 };
	inheritedVelocityScale = 0.5;
	// rocket specific
	trailType = 1;
	trailLength = 15;
	trailWidth = 0.3;
	soundId = SoundDiscSpin;
};

ItemData DiscAmmo 
{
	description = "Disc";
	className = "Ammo";
	shapeFile = "discammo";
	heading = $InvHead[ihAmm];
	shadowDetailMask = 4;
	price = 2;
};

MineData DiscAmmoBomb
{
	mass = 5.0;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Halo";
	shapeFile = "discb";
	shadowDetailMask = 4;
	explosionId = flashExpSmall;	//mineExp;
	explosionRadius = 5.0;
	damageValue = 0.0;	//0.5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 10.5;
};

ItemImageData DiscLauncherImage 
{
	shapeFile = "disc";
	mountPoint = 0;
	weaponType = 3;
	ammoType = DiscAmmo;
	projectileType = DiscShell;
	accuFire = true;
	reloadTime = 0.10;
	fireTime = 0.75;
	spinUpTime = 0.25;
	sfxFire = SoundFireDisc;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDiscReload;
	sfxReady = SoundDiscSpin;
};

ItemData DiscLauncher 
{
	description = "Disc Launcher";
	className = "Weapon";
	shapeFile = "disc";
	hudIcon = "disk";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = DiscLauncherImage;
	price = 150;
	showWeaponBar = true;
};

function DiscLauncher::MountExtras(%player,%weapon)
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>Fires spinning blades that explode upon impact.");
}
