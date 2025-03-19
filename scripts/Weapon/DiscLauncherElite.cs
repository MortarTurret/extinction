$InvList[DisclauncherElite] = 1;
$MobileInvList[DisclauncherElite] = 1;
$RemoteInvList[DisclauncherElite] = 1;

$InvList[DiscAmmoElite] = 1;
$MobileInvList[DiscAmmoElite] = 1;
$RemoteInvList[DiscAmmoElite] = 1;

$SellAmmo[DiscAmmoElite] = 5;
$WeaponAmmo[DiscLauncherElite] = DiscAmmoElite;

addWeapon(DiscLauncherElite);
addAmmo(DiscLauncherElite, DiscAmmoElite, 5);

RocketData DiscShellElite
{
   bulletShapeName = "discb.dts";
   explosionTag    = rocketExp;

   collisionRadius = 0.0;
   mass            = 2.0;

   damageClass      = 1;       // 0 impact, 1, radius
   damageValue      = 0.5;
   damageType       = $ExplosionDamageType;

   explosionRadius  = 7.5;
   kickBackStrength = 150.0;

   muzzleVelocity   = 65.0;
   terminalVelocity = 80.0;
   acceleration     = 5.0;

   totalTime        = 6.5;
   liveTime         = 8.0;

   lightRange       = 5.0;
   lightColor       = { 0.4, 0.4, 1.0 };

   inheritedVelocityScale = 0.5;

   // rocket specific
   trailType   = 1;
   trailLength = 15;
   trailWidth  = 0.3;

   soundId = SoundDiscSpin;
};

ItemData DiscAmmoElite
{
	description = "Disc Elite";
	className = "Ammo";
	shapeFile = "discammo";
    heading = $InvHead[ihAmm];
	shadowDetailMask = 4;
	price = 2;
};

ItemImageData DiscLauncherEliteImage
{
	shapeFile = "disc";
	mountPoint = 0;

	weaponType = 3; // DiscLauncher
	ammoType = DiscAmmoElite;
	projectileType = DiscShellElite;
	accuFire = true;
	reloadTime = 0.25;
	fireTime = 1.25;
	spinUpTime = 0.25;

	sfxFire = SoundFireDisc;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDiscReload;
	sfxReady = SoundDiscSpin;
};

ItemData DiscLauncherElite
{
	description = "Disc Launcher Elite";
	className = "Weapon";
	shapeFile = "disc";
	hudIcon = "disk";
    heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = DiscLauncherEliteImage;
	price = 150;
	showWeaponBar = true;
};

function DisclauncherElite::MountExtras(%player,%weapon)
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>The Legendary EliteRenegades Disc Launcher.");
}


