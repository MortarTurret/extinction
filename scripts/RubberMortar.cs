$InvList[RubberMortar] = 1;
$MobileInvList[RubberMortar] = 1;
$RemoteInvList[RubberMortar] = 1;

$InvList[RubberAmmo] = 1;
$MobileInvList[RubberAmmo] = 1;
$RemoteInvList[RubberAmmo] = 1;

$AutoUse[RubberMortar]= false;
$WeaponAmmo[RubberMortar]= Rubberammo;
$SellAmmo[RubberAmmo]= 10;

addWeapon(RubberMortar);
addAmmo(RubberMortar, RubberAmmo, 2);

GrenadeData RubberMortarShell
{	bulletShapeName = "mortar.dts";
	explosionTag = mortarExp;
	collideWithOwner = True;
	ownerGraceMS = 250;
	collisionRadius = 0.3;
	mass = 5.0;
	elasticity = 0.65;
	damageClass = 1; // 0 impact, 1, radius
	damageValue = 1.0;
	damageType = $MortarDamageType;
	explosionRadius = 20.0;
	kickBackStrength = 250.0;
	maxLevelFlightDist = 275;
	totalTime = 25.0;
	liveTime = 5.0;
	projSpecialTime = 0.01;
	inheritedVelocityScale = 0.5;
	smokeName = "mortartrail.dts";
};

ItemData RubberAmmo
{
	description = "R Mortar Ammo";
	className = "Ammo";
	heading = $InvHead[ihAmm];
	shapeFile = "mortarammo";
	shadowDetailMask = 4;
	price = 5;
};
MineData RubberAmmoBomb
{
	mass = 5.0;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Halo";
	shapeFile = "mortar";
	shadowDetailMask = 4;
	explosionId = flashExpSmall;	//mineExp;
	explosionRadius = 5.0;
	damageValue = 0.0;	//0.5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 10.5;
};

ItemImageData RMortarImage
{
	shapeFile = "mortargun";
	mountPoint = 0;
	weaponType = 0; // Single Shot
	ammoType = RubberAmmo;
	projectileType = RubberMortarShell;
	accuFire = false;
	reloadTime = 0.5;
	fireTime = 2.0;
	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };
	sfxFire = SoundFireMortar;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundMortarReload;
	sfxReady = SoundMortarIdle;
};

ItemData RubberMortar
{
	description = "Rubbery Mortar";
	className = "Weapon";
	shapeFile = "mortargun";
	hudIcon = "mortar";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = RMortarImage;
	price = 375;
	showWeaponBar = true;
};

function RubberMortar::MountExtras(%player,%weapon)
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>A mortar and pack of silly putty rolled into one!.");
}