
$InvList[LaserRifle] = 1;
$MobileInvList[LaserRifle] = 1;
$RemoteInvList[LaserRifle] = 1;

$AutoUse[LaserRifle] = false;

addWeapon(LaserRifle);


//----------------------------------------------------------------------------
LaserData SniperLaser
{
	laserBitmapName = "discglow2.bmp";
	hitName = "laserhit.dts";
	damageConversion = 0.010; //0.020
	DamageType = $LaserDamageType;	
	beamTime = 1.5;
	lightRange = 2.0;
	lightColor = { 1.0, 0.25, 0.25 };
	detachFromShooter = true;
	hitSoundId = SoundLaserHit;
};


//--------------------------------------
ItemImageData LaserRifleImage
{
	shapeFile = "sniper";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	projectileType = SniperLaser;
	accuFire = true;
	reloadTime = 0.1;
	fireTime = 0.5;
	minEnergy = 10;
	maxEnergy = 30;

	lightType = 3;  // Weapon Fire
	lightRadius = 2;
	lightTime = 1;
	lightColor = { 1, 0, 0 };
	mountRotation = { 0, 3.1416, 0 };
	sfxFire = SoundFireLaser;
	sfxActivate = SoundPickUpWeapon;
};

ItemData LaserRifle
{
	description = "Laser Rifle";
	className = "Weapon";
	shapeFile = "sniper";
	hudIcon = "sniper";
   heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = LaserRifleImage;
	price = 200;
	showWeaponBar = true;
   //validateShape = true;
//   validateMaterials = true;
};

function LaserRifle::MountExtras(%player,%weapon)
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>The Legendary Laser Rifle");
}