$InvList[PhaseDisrupter] = 1;
$MobileInvList[PhaseDisrupter] = 1;
$RemoteInvList[PhaseDisrupter] = 1;

$InvList[PhaseAmmo] = 1;
$MobileInvList[PhaseAmmo] = 1;
$RemoteInvList[PhaseAmmo] = 1;

$AutoUse[PhaseDisrupter] = False;
$SellAmmo[PhaseAmmo] = 2;
$WeaponAmmo[PhaseDisrupter] = PhaseAmmo;

addWeapon(PhaseDisrupter);
addAmmo(PhaseDisrupter, PhaseAmmo, 2);
RocketData PhaseDisrupterBolt
{	
	bulletShapeName = "fusionbolt.dts";
	explosionTag = PhaseDisrupterExp;
	collisionRadius = 0.0;
	mass = 2.0;
	damageClass = 1;
	damageValue = 1.25;
	DamageType = $RocketDamageType;
	explosionRadius = 20.0; 
	kickBackStrength = 450.0;
	muzzleVelocity = 80.0;
	terminalVelocity = 80.0;
	acceleration = 5.0;
	totalTime = 15.0;
	liveTime = 15.0;
	lightRange = 10.0;
	lightColor = { 1.0, 6.7, 9.5 };
	inheritedVelocityScale = 0.5;
	trailType = 2;
	trailString = "fusionex.dts";
	smokeDist = 1.8;
	soundId = SoundJetHeavy;
};

ItemData PhaseAmmo
{
	description = "Disrupter Ammo";
	className = "Ammo";
	heading = $InvHead[ihAmm];
	shapeFile = "mortar";	//shapeFile = "rocket";
	shadowDetailMask = 4;
	price = 200;
};

MineData PhaseAmmoBomb
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

ItemImageData PhaseDisrupterImage
{
	shapeFile = "energygun";
	mountPoint = 0;
	weaponType = 0; // Single Shot
	ammoType = PhaseAmmo;
	projectileType = PhaseDisrupterBolt;
	accuFire = true;
	reloadTime = 3.0;	//3.0
	fireTime = 2.0;	//2.0
	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };
	sfxFire = SoundFireMortar;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundJetLight;
	sfxReady = SoundJammerOn;
};

ItemData PhaseDisrupter
{
	description = "Phase Disrupter";
	className = "Weapon";
	shapeFile = "plasma";
	hudIcon = "plasma";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = PhaseDisrupterImage;
	price = 1000;
	showWeaponBar = true;
};

function PhaseDisrupter::MountExtras(%player,%weapon) 
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>Disrupts the time-space continuum to send all in its path to oblivion at the cost of a long reload time. Time your shots!");
}
