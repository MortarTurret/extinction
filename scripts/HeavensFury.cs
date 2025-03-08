$InvList[Heavensfury] = 1;
$MobileInvList[Heavensfury] = 1;
$RemoteInvList[Heavensfury] = 1;

$AutoUse[Heavensfury] = false;
$WeaponAmmo[Heavensfury] = "";

addWeapon(Heavensfury);

BulletData HeavensfuryBolt 
{	bulletShapeName = "fusionbolt.dts";
	explosionTag = energyExp;
	damageClass = 0;
	damageValue = 0.07;
	damageType = $ElectricityDamageType;
	aimDeflection = 0.015;
	muzzleVelocity = 100.0;
	totalTime = 1.3;
	liveTime = 1.3;
	lightRange = 3.0;
	lightColor = { 1, 1, 0 };
	inheritedVelocityScale = 0.3;
	isVisible = True;
	soundId = SoundJetLight;
};
ItemImageData HeavensfuryImage 
{
	shapeFile = "shotgun";
	mountPoint = 0;
	weaponType = 0;
	reloadTime = 0.02;
	fireTime = 0.02;
	minEnergy = 2; 
	maxEnergy = 3; 
	projectileType = HeavensfuryBolt;
	accuFire = false;
	sfxFire = SoundJetHeavy;
	sfxActivate = SoundPickUpWeapon;
};

ItemData Heavensfury 
{
	heading = $InvHead[ihWea];
	description = "Heaven's Fury";
	className = "Weapon";
	shapeFile = "shotgun";
	hudIcon = "weapon";
	shadowDetailMask = 4;
	imageType = HeavensfuryImage;
	price = 385;
	showWeaponBar = true;
};

function Heavensfury::MountExtras(%player,%weapon) 
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>The angel's version of a shotgun.\n<jc><f2>Inflicts electrical damage rendering enemy <f1>shields, jetpacks, and energy weapons<f2> useless.");
}