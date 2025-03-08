$InvList[DeathRay] = 1;
$MobileInvList[DeathRay] = 1;
$RemoteInvList[DeathRay] = 1;

$AutoUse[DeathRay] = True;
$WeaponAmmo[DeathRay] = "";

addWeapon(DeathRay);

LaserData DeathRayLaser
{	laserBitmapName = "warp.bmp";
	hitName = "paint.dts";
	damageConversion = 0.035; //BR Setting
	DamageType = $LaserDamageType;	
	beamTime = 0.5;
	lightRange = 1.0;
	lightColor = { 1.0, 0.25, 0.25 };
	detachFromShooter = false;
	hitSoundId = SoundLaserHit;
};
ItemImageData DeathRayImage
{
	shapeFile = "paint";
	mountPoint = 0;
	weaponType = 0;		//0 single, 1 rotating, 2 sustained, 3disc
	reloadTime = 0.40;
	fireTime = 0.3;
	minEnergy = 5;
//	maxEnergy = 20;
//	projectileType = DeathRayLaser;
	accuFire = true;
//	sfxFire = SoundFireLaser;
	sfxActivate = SoundPickUpWeapon;
};

ItemData DeathRay 
{
	heading = $InvHead[ihSpl];
	description = "Spell: Death Ray";
	className = "Weapon";
	shapeFile = "DSPLY_S1";
	hudIcon = "targetlaser";
	shadowDetailMask = 4;
	imageType = DeathRayImage;
	price = 250;
	showWeaponBar = true;
};


ItemImageData DeathRayBeamImage
{
	shapeFile = "breath";
	mountPoint = 0;
	weaponType = 0;		//0 single, 1 rotating, 2 sustained, 3disc
	reloadTime = 0.4;
	fireTime = 0.3;
	
	
	minEnergy = 5;
//	maxEnergy = 20;
	projectileType = DeathRayLaser;
	accuFire = true;
	sfxFire = SoundFireLaser;
//	sfxActivate = SoundPickUpWeapon;
};

ItemData DeathRayBeam 
{
	heading = $InvHead[ihSpl];
	description = "Spell: Death Ray";
	className = "Weapon";
	shapeFile = "DSPLY_S1";
//	hudIcon = "targetlaser";
	shadowDetailMask = 4;
	imageType = DeathRayBeamImage;
	price = 250;
	showWeaponBar = true;
};

function DeathRayImage::onFire(%player, %slot)
{	
	if($debug)
		Anni::Echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));		
	
	if(!%player.Reloading)
	{	
		if (Player::getMountedItem(%player,4) != DeathRayBeam)
		{
			Player::mountItem(%player,DeathRayBeam,4);	
		}			
		%player.Reloading = true;		
		schedule(%player @ ".Reloading = false;" , 0.7,%player);
		player::trigger(%player,4,true);
		schedule("Player::trigger("@%player@", 4, false);",0.1,%player);

	}
}


function DeathRay::MountExtras(%player,%weapon) 
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>This spell fires a bolt of negative energy towards your target.");
}


