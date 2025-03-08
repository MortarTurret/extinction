$InvList[TankRPGLauncher]=1;
$MobileInvList[TankRPGLauncher]=1;
$RemoteInvList[TankRPGLauncher]=1;

$InvList[TankRPGAmmo]=1;
$MobileInvList[TankRPGAmmo]=1;
$RemoteInvList[TankRPGAmmo]=1;

$AutoUse[TankRPGLauncher] = false;
$WeaponAmmo[TankRPGLauncher] = TankRPGAmmo;

$SellAmmo[TankRPGAmmo] = 50; 

addweapon(TankRPGLauncher);
addAmmo(TankRPGLauncher, TankRPGAmmo, 5);

$TankRPGSlotA=4;
$TankRPGSlotB=7;
$TankRPGSlotC=6; 

RocketData RPG
{	
	bulletShapeName = "rocket.dts";
	explosionTag = grenadeExp;
	collisionRadius = 0.0;
	mass = 2.0;
	damageClass = 1; // 0 impact, 1, radius
	damageValue = 0.25;
	damageType = $ShrapnelDamageType;
	explosionRadius = 15;
	kickBackStrength = 150.0;
	muzzleVelocity = 100.0;
	terminalVelocity = 200.0;
	acceleration = 5.0;
	totalTime = 10.0;
	liveTime = 11.0;
	lightRange = 5.0;
	lightColor = { 1.0, 0.7, 0.5 };
	inheritedVelocityScale = 0.5;
	// rocket specific
	trailType = 2; // smoke trail
	trailString = "rsmoke.dts";
	smokeDist = 6;
	soundId = SoundJetHeavy;
};
ItemData TankRPGAmmo 
{
	description = "RPG Ammo"; 
	className = "Ammo"; 
	shapeFile = "ammo1"; 
	heading = $InvHead[ihAmm]; 
	shadowDetailMask = 4; 
	price = 1; 
}; 
MineData TankRPGAmmoBomb
{
	mass = 5.0;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Halo";
	shapeFile = "grenade";
	shadowDetailMask = 4;
	explosionId = flashExpSmall;	//mineExp;
	explosionRadius = 5.0;
	damageValue = 0.0;	//0.5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 10.5;
};
ItemImageData TankRPGLauncherImage 
{
	shapeFile = "mortargun"; 
	mountPoint = 0; 
	mountOffset = { -1.305, -0.20, 0.25 }; 
	mountRotation = { 0, 1.50, 0 }; 
	weaponType = 0; 
	reloadTime = 2.0; 
	fireTime = 0.1;
	minEnergy = 5;	
	maxEnergy = 6;
	ammoType = TankRPGAmmo; 
	accuFire = true; 
	sfxFire = SoundMissileTurretFire; 
	sfxActivate = SoundPickUpWeapon; 
	sfxReload = SoundMortarReload;
	sfxReady = SoundMortarIdle;
}; 

ItemData TankRPGLauncher 
{
	description = "Tank RPG"; 
	className = "Weapon"; 
	shapeFile = "mortargun"; 
	mountOffset = { -1.1, 0.02, 0.4 }; 
	mountRotation = { 0, -1.1, 0}; 
	hudIcon = "ammopack"; 
	heading = $InvHead[ihWea]; 
	shadowDetailMask = 4; 
	imageType = TankRPGLauncherImage; 
	price = 550; 
	showWeaponBar = true; 
};

ItemImageData TankRPGLauncher2Image 
{
	ammoType = TankRPGAmmo; 
	projectileType = RPG; 
	shapeFile = "mortargun"; 
	mountPoint = 0; 
	mountOffset = { -1.2, -0.35, 0 }; 
	mountRotation = { 0, 1.0, 0}; 
	weaponType = 0; 
	reloadTime = 2.0;
	fireTime = 0.1;
	accuFire = false; 
	sfxFire = SoundMissileTurretFire; 
};

ItemData TankRPGLauncher2 
{
	description = "Tank RPG"; 
	className = "Weapon"; 
	shapeFile = "mortargun"; 
	hudIcon = "chain"; 
	shadowDetailMask = 4; 
	imageType = TankRPGLauncher2Image; 
	price = 0; 
	showWeaponBar = true; 
	showInventory = false; 
};

ItemImageData TankRPGLauncher3Image 
{
	ammoType = TankRPGAmmo; 
	projectileType = RPG; 
	shapeFile = "mortargun"; 
	mountPoint = 0; 
	mountOffset = { 0, -0.35, 0 }; 
	mountRotation = { 0, -1.0, 0 }; 
	weaponType = 0; 
	reloadTime = 2.0;
	fireTime = 0.1;
	accuFire = false; 
	sfxFire = SoundMissileTurretFire; 
};

ItemData TankRPGLauncher3 
{
	description = "Tank RPG"; 
	className = "Weapon"; 
	shapeFile = "mortargun"; 
	hudIcon = "chain"; 
	shadowDetailMask = 4; 
	imageType = TankRPGLauncher3Image; 
	price = 0; 
	showWeaponBar = true; 
	showInventory = false; 
};
 
ItemImageData TankRPGLauncher4Image 
{
	ammoType = TankRPGAmmo; 
	projectileType = RPG; 
	shapeFile = "mortargun"; 
	mountPoint = 0; 
	mountOffset = {0.10, -0.20, 0.25 }; 
	mountRotation = { 0, -1.50, 0}; 
	weaponType = 0; 
	reloadTime = 2.0; 
	fireTime = 0.1;
	accuFire = false; 
	sfxFire = SoundMissileTurretFire; 
};

ItemData TankRPGLauncher4 
{
	description = "Tank RPG"; 
	className = "Weapon"; 
	shapeFile = "mortargun"; 
	hudIcon = "chain"; 
	shadowDetailMask = 4; 
	imageType = TankRPGLauncher4Image; 
	price = 0; 
	showWeaponBar = true; 
	showInventory = false; 
};

function TankRPGLauncherImage::onFire(%player, %slot) 
{		
	if($debug)
		Anni::Echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = GameBase::getOwnerClient(%player); 
	Player::decItemCount(%player,TankRPGammo,1); 
	%trans = GameBase::getMuzzleTransform(%player); 
	%vel = Item::getVelocity(%player); 
	Projectile::spawnProjectile("RPG",%trans,%player,%vel,%player); 
	if(!$FiringTankRPGLauncher[%client]) 
		CheckTankRPGLauncher(%client, %player); 
}

function TankRPGLauncher::MountExtras(%player,%weapon)
{		
	Player::mountItem(%player,TankRPGLauncher2,$TankRPGSlotA); 
	Player::mountItem(%player,TankRPGLauncher3,$TankRPGSlotB); 
	Player::mountItem(%player,TankRPGLauncher4,$TankRPGSlotC);
	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>Fires four highly explosive missiles to pummel your target.");
}

function CheckTankRPGLauncher(%client, %player) 
{
	if(Player::isTriggered(%player,$WeaponSlot) && (Player::getMountedItem(%player,$WeaponSlot) == "TankRPGLauncher")) 
	{	Player::trigger(%player,$TankRPGSlotA,true);
		Player::trigger(%player,$TankRPGSlotB,true);
		Player::trigger(%player,$TankRPGSlotC,true); 
		schedule("CheckTankRPGLauncher(" @ %client @ "," @ %player @ ");",0.1,%player); 
		$FiringTankRPGLauncher[%client] = true; 
	}
	else 
	{	Player::trigger(%player,$TankRPGSlotA,false); 
		Player::trigger(%player,$TankRPGSlotB,false); 
		Player::trigger(%player,$TankRPGSlotC,false); 
		$FiringTankRPGLauncher[%client] = false; 
	}
}
