$InvList[AngelFire] = 1;
$MobileInvList[AngelFire] = 1;
$RemoteInvList[AngelFire] = 1;

$AutoUse[AngelFire] = false;
$WeaponAmmo[AngelFire] = "";

addWeapon(AngelFire);

GrenadeData AngelFireGren
{	bulletShapeName = "Paint.dts";
	explosionTag = rocketExp;
	collideWithOwner = True;
	ownerGraceMS = 250;
	collisionRadius = 0.2;
	mass = 1.0;
	elasticity = 0.3;
	damageClass = 1;
	damageValue = 0.20;
	damageType = $PlasmaDamageType;
	explosionRadius = 5;
	kickBackStrength = 0;
	maxLevelFlightDist = 200;
	totalTime = 5;
	liveTime = 0.05;
	projSpecialTime = 0.05;
	inheritedVelocityScale = 0.5;
	smokeName = "enex.dts";
	smokeDist = 1.5;
};

ItemImageData AngelFireImage 
{
	shapeFile = "enbolt";
	mountPoint = 0;
	mountOffset = { 0.0, 0.0, 0.45 };
	mountRotation = { 0, 1.5, 0 };
	weaponType = 0;
	reloadTime = 0.90;
	fireTime = 0.2;
	minEnergy = 5;
	maxEnergy = 25;
	projectileType = AngelFireGren;
	accuFire = false;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1 };
	sfxFire = SoundJetHeavy;
	sfxReload = SoundDryFire;
	sfxActivate = SoundPickUpWeapon;
};

ItemData AngelFire 
{
	description = "Angel Fire";
	className = "Weapon";
	shapeFile = "enbolt";
	hudIcon = "plasma";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = AngelFireImage;
	price = 475;
	showWeaponBar = true;
};

function AngelFire::MountExtras(%player,%weapon)
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>Lobs explosive energy upon your victims which sets them on fire.");
}

