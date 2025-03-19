$InvList[PlasmaGunBase] = 1;
$MobileInvList[PlasmaGunBase] = 1;
$RemoteInvList[PlasmaGunBase] = 1;

$InvList[PlasmaAmmoBase] = 1;
$MobileInvList[PlasmaAmmoBase] = 1;
$RemoteInvList[PlasmaAmmoBase] = 1;

$AutoUse[PlasmaGunBase] = false;
$SellAmmo[PlasmaAmmoBase] = 5;
$WeaponAmmo[PlasmaGunBase] = PlasmaAmmoBase;

addWeapon(PlasmaGunBase);
addAmmo(PlasmaGunBase, PlasmaAmmoBase, 5);

BulletData PlasmaBoltBase
{
   bulletShapeName    = "plasmabolt.dts";
   explosionTag       = plasmaExp;
   damageClass        = 1;
   damageValue        = 0.45;
   damageType         = $PlasmaDamageType;
   explosionRadius    = 4.0;
   muzzleVelocity     = 55.0;
   totalTime          = 3.0;
   liveTime           = 2.0;
   lightRange         = 3.0;
   lightColor         = { 1, 1, 0 };
   inheritedVelocityScale = 0.3;
   isVisible          = True;
   soundId = SoundJetLight;
};

ItemData PlasmaAmmoBase
{
	description = "PlasmaBolt Base";
   heading = $InvHead[ihAmm];
	className = "Ammo";
	shapeFile = "plasammo";
	shadowDetailMask = 4;
	price = 2;
};

ItemImageData PlasmaGunBaseImage
{
	shapeFile = "plasma";
	mountPoint = 0;
	weaponType = 0; // Single Shot
	ammoType = PlasmaAmmoBase;
	projectileType = PlasmaBoltBase;
	accuFire = true;
	reloadTime = 0.1;
	fireTime = 0.5;
	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 1, 1, 0.2 };
	sfxFire = SoundFirePlasma;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;
};

ItemData PlasmaGunBase
{
	description = "Plasma Gun Base";
	className = "Weapon";
	shapeFile = "plasma";
	hudIcon = "plasma";
   heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = PlasmaGunBaseImage;
	price = 175;
	showWeaponBar = true;
   //validateShape = true;
};

function PlasmagunBase::MountExtras(%player,%weapon)
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>The Legendary Base Plasma Gun.");
}
