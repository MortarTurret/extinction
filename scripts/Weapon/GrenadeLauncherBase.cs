//$InvList[GrenadeLauncherBase] = 1;
//$MobileInvList[GrenadeLauncherBase] = 1;
//$RemoteInvList[GrenadeLauncherBase] = 1;

//$InvList[GrenadeAmmoBase] = 1;
//$MobileInvList[GrenadeAmmoBase] = 1;
//$RemoteInvList[GrenadeAmmoBase] = 1;

$AutoUse[GrenadeLauncherBase] = false;
$SellAmmo[GrenadeAmmoBase] = 5;
$WeaponAmmo[GrenadeLauncherBase] = GrenadeAmmoBase;

addWeapon(GrenadeLauncherBase);
addAmmo(GrenadeLauncherBase, GrenadeAmmoBase, 2);

GrenadeData GrenadeShellBase
{
   bulletShapeName    = "grenade.dts";
   explosionTag       = grenadeExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.2;
   mass               = 1.0;
   elasticity         = 0.45;
   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.4;
   damageType         = $ShrapnelDamageType;
   explosionRadius    = 15;
   kickBackStrength   = 150.0;
   maxLevelFlightDist = 150;
   totalTime          = 30.0;    // special meaning for grenades...
   liveTime           = 1.0;
   projSpecialTime    = 0.05;
   inheritedVelocityScale = 0.5;
   smokeName              = "smoke.dts";
};

ItemData GrenadeAmmoBase
{
	description = "Grenade Ammo";
	className = "Ammo";
	shapeFile = "grenammo";
    heading = $InvHead[ihAmm];
	shadowDetailMask = 4;
	price = 2;
};

ItemImageData GrenadeLauncherBaseImage
{
	shapeFile = "grenadeL";
	mountPoint = 0;
	weaponType = 0; // Single Shot
	ammoType = GrenadeAmmoBase;
	projectileType = GrenadeShellBase;
	accuFire = false;
	reloadTime = 0.5;
	fireTime = 0.5;
	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };
	sfxFire = SoundFireGrenade;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;
};

ItemData GrenadeLauncherBase
{
	description = "Grenade Launcher";
	className = "Weapon";
	shapeFile = "grenadeL";
	hudIcon = "grenade";
    heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = GrenadeLauncherBaseImage;
	price = 150;
	showWeaponBar = true;
   //validateShape = true;
};

function GrenadeLauncherBase::MountExtras(%player,%weapon)
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>The Legendary Base Grenade Launcher.");
}
