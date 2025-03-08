

LaserData CuttingLaserBeam
{
	laserBitmapName = "laserPulse.bmp";
	hitName = "laserhit.dts";
	damageConversion = 15.0;
	DamageType = $LaserDamageType;	
	beamTime = 0.5;
	lightRange = 2.0;
	lightColor = { 1.0, 0.25, 0.25 };
	detachFromShooter = false;
	hitSoundId = SoundLaserHit;
};

ItemImageData CuttingLaserImage 
{
	shapeFile = "paintgun";
	mountPoint = 0;
	weaponType = 1;		//0 single, 1 rotating, 2 sustained, 3disc
	spinUpTime = 0.1;
	spinDownTime = 0.1;
	fireTime = 0.1;	
	projectileType = CuttingLaserBeam;
	accuFire = true;
	minEnergy = 5;
	maxEnergy = 0.1;	//15
	reloadTime = 1.0;
	lightType = 3;
	lightRadius = 1;
	lightTime = 1;
	lightColor = { 0.25, 1, 0.25 };
	sfxFire = SoundFireTargetingLaser;
	sfxActivate = SoundPickUpWeapon;
};

ItemData CuttingLaser 
{
	description = "Cutting Laser";	
	className = "Tool";
	shapeFile = "paintgun";
	hudIcon = "targetlaser";
	shadowDetailMask = 4;
	imageType = CuttingLaserImage;
};
