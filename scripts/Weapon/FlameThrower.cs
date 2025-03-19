$InvList[FlameThrower] = 1;
$MobileInvList[FlameThrower] = 1;
$RemoteInvList[FlameThrower] = 1;

$InvList[FlameThrowerAmmo] = 1;
$MobileInvList[FlameThrowerAmmo] = 1;
$RemoteInvList[FlameThrowerAmmo] = 1;

$AutoUse[FlameThrower] = False;
$SellAmmo[FlameThrowerAmmo] = 25;	
$WeaponAmmo[FlameThrower] = FlameThrowerAmmo;

addWeapon(FlameThrower);
addAmmo(FlameThrower, FlameThrowerAmmo, 10);

GrenadeData FlameThrowerGren
{	bulletShapeName = "PlasmaBolt.dts";
	explosionTag = plasmaExp;
	collideWithOwner = True;
	ownerGraceMS = 250;
	collisionRadius = 0.2;
	mass = 1.0;
	elasticity = 0.3;
	damageClass = 1;
	damageValue = 0.5; 
	damageType = $PlasmaDamageType;
	explosionRadius = 8;
	kickBackStrength = 0;
	maxLevelFlightDist = 150;
	totalTime = 5.0;
	liveTime = 0.001;
	projSpecialTime = 0.05;
	inheritedVelocityScale = 0.5;
	smokeName = "Plasmatrail.dts";
	smokeDist = 1.5;
};


ItemData FlameThrowerAmmo 
{
	description = "Flame Cartridges";
	className = "Ammo";
	heading = $InvHead[ihAmm];
	shapeFile = "ammo1";
	shadowDetailMask = 4;
	price = 2;
};

MineData FlameThrowerAmmoBomb
{
	mass = 5.0;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Halo";
	shapeFile = "PlasmaBolt";
	shadowDetailMask = 4;
	explosionId = flashExpSmall;	//mineExp;
	explosionRadius = 5.0;
	damageValue = 0.0;	//0.5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 10.5;
};

ItemImageData FlameThrowerImage 
{
	shapeFile = "mortar";
	mountPoint = 0;
	weaponType = 0;
	ammoType = FlameThrowerAmmo;
	projectileType = FlameThrowerGren;
	accuFire = false;
	reloadTime = 0.1;
	fireTime = 0.1;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };
	sfxFire = SoundJetHeavy;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;
};

ItemData FlameThrower 
{
	description = "Flame Thrower";
	className = "Weapon";
	shapeFile = "mortar";
	hudIcon = "weapon";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = FlameThrowerImage;
	price = 175;
	showWeaponBar = true;
};

function FlameThrower::MountExtras(%player,%weapon) 
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>Hurls fiery death cartridges with a moderate range.");
}
