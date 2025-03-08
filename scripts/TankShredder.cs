$InvList[TankShredder]=1;
$MobileInvList[TankShredder]=1;
$RemoteInvList[TankShredder]=1;

$InvList[TankShredderAmmo]=1;
$MobileInvList[TankShredderAmmo]=1;
$RemoteInvList[TankShredderAmmo]=1;

$AutoUse[TankShredder] = false;
$WeaponAmmo[TankShredder] = TankShredderAmmo;
$SellAmmo[TankShredderAmmo] = 150;

addweapon(TankShredder);
addAmmo(TankShredder, TankShredderAmmo, 15);

$TankShredderSlotA=4;
$TankShredderSlotB=7;
$TankShredderSlotC=6;

BulletData ShredderBullet
{	bulletShapeName = "bullet.dts";
	explosionTag = bulletExp0;
	expRandCycle = 3;
	mass = 0.05;
	bulletHoleIndex = 0;
	damageClass = 0; // 0 impact, 1, radius
	damageValue = 0.055;
	damageType = $BulletDamageType;
	muzzleVelocity = 768.0;
	totalTime = 1.5;
	inheritedVelocityScale = 1.0;
	isVisible = False;
	tracerPercentage = 1.0;
	tracerLength = 30;
};

ItemData TankShredderAmmo 
{
	description = "Tank Shredder Ammo";
	className = "Ammo";
	shapeFile = "ammo1";
	heading = $InvHead[ihAmm];
	shadowDetailMask = 4;
	price = 1;
};
MineData TankShredderAmmoBomb
{
	mass = 5.0;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Halo";
	shapeFile = "bullet";
	shadowDetailMask = 4;
	explosionId = flashExpSmall;	//mineExp;
	explosionRadius = 5.0;
	damageValue = 0.0;	//0.5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 10.5;
};

ItemImageData TankShredderImage 
{
	shapeFile = "chaingun";
	mountPoint = 0;
	mountOffset = { 0, -0.351, 0 };
	mountRotation = { 0, -1.01, 0 };
	weaponType = 1;
	reloadTime = 0;
	spinUpTime = 0.1;
	spinDownTime = 3;
	fireTime = 0.1;
	ammoType = TankShredderAmmo;
	accuFire = true;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1 };
	sfxFire = SoundFireChaingun;
	sfxActivate = SoundPickupWeapon;
	sfxSpinUp = SoundSpinUp;
	sfxSpinDown = SoundSpinDown;
};

ItemData TankShredder 
{
	description = "Tank Shredder";
	className = "Weapon";
	shapeFile = "chaingun";
	hudIcon = "chain";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = TankShredderImage;
	price = 7500;
	showWeaponBar = true;
};


ItemImageData TankShredder2Image 
{
	shapeFile = "chaingun";
	mountPoint = 0;
	mountOffset = { -1.21, -0.351, 0 };
	mountRotation = { 0, 1.01, 0};
	weaponType = 1;
	reloadTime = 0;
	spinUpTime = 0.1;
	spinDownTime = 3;
	fireTime = 0.1;
	ammoType = TankShredderAmmo;
	projectileType = ShredderBullet;
	accuFire = true;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1 };
	sfxFire = SoundFireChaingun;
	sfxSpinUp = SoundSpinUp;
	sfxSpinDown = SoundSpinDown;
};

ItemData TankShredder2 
{
	description = "Tank Shredder";
	className = "Weapon";
	shapeFile = "chaingun";
	hudIcon = "chain";
	shadowDetailMask = 4;
	imageType = TankShredder2Image;
	price = 0;
	showWeaponBar = false;
	showInventory = false;
};

ItemImageData TankShredder3Image 
{
	shapeFile = "chaingun";
	mountPoint = 0;
	mountOffset = { -1.3051, -0.201, 0.251 };
	mountRotation = { 0, 1.501, 0 };
	weaponType = 1;
	reloadTime = 0;
	spinUpTime = 0.1;
	spinDownTime = 3;
	fireTime = 0.1;
	ammoType = TankShredderAmmo;
	projectileType = ShredderBullet;
	accuFire = true;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1 };
	sfxFire = SoundFireChaingun;
};

ItemData TankShredder3 
{
	description = "Tank Shredder";
	className = "Weapon";
	shapeFile = "chaingun";
	hudIcon = "chain";
	shadowDetailMask = 4;
	imageType = TankShredder3Image;
	price = 0;
	showWeaponBar = false;
	showInventory = false;
};
ItemImageData TankShredder4Image 
{
	shapeFile = "chaingun";
	mountPoint = 0;
	mountOffset = {0.101, -0.201, 0.251 };
	mountRotation = { 0, -1.501, 0}; 
	weaponType = 1;
	reloadTime = 0;
	spinUpTime = 0.1;
	spinDownTime = 3;
	fireTime = 0.1;
	ammoType = TankShredderAmmo;
	projectileType = ShredderBullet;
	accuFire = true;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1 };
	sfxFire = SoundFireChaingun;
};

ItemData TankShredder4 
{
	description = "Tank Shredder";
	className = "Weapon";
	shapeFile = "chaingun";
	hudIcon = "chain";
	shadowDetailMask = 4;
	imageType = TankShredder4Image;
	price = 0;
	showWeaponBar = false;
	showInventory = false;
};

function TankShredderImage::onFire(%player, %slot) 
{		
	if($debug)
		Anni::Echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));	

	//TankShredderImage::spawnProjectile(%player);
	%client = GameBase::getOwnerClient(%player);
	//%player.firingShredder = true;
	//schedule("TankShredderImage::spawnProjectile(" @ %player @ ");",1);
	Player::decItemCount(%player,$WeaponAmmo[TankShredder],1);
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Projectile::spawnProjectile("ShredderBullet",%trans,%player,%vel,%player);
	if(!$FiringTankShredder[%client]) 
		CheckTankShredder(%client, %player);
}

function TankShredderImage::spawnProjectile(%player)
{
	%client = GameBase::getOwnerClient(%player);
	if(Player::isTriggered(%player,$WeaponSlot) && (Player::getMountedItem(%player,$WeaponSlot) == "TankShredder"))
	{
		Player::decItemCount(%player,$WeaponAmmo[TankShredder],1);
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);
		Projectile::spawnProjectile("ShredderBullet",%trans,%player,%vel,%player);
	}
}


function TankShredder::MountExtras(%player,%weapon)
{		
	Player::mountItem(%player,TankShredder2,$TankShredderSlotA);
	Player::mountItem(%player,TankShredder3,$TankShredderSlotB);
	Player::mountItem(%player,TankShredder4,$TankShredderSlotC);
	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>You don't want to get in the way of this quad chaingun!");
}

function CheckTankShredder(%client, %player) 
{
	if(Player::isTriggered(%player,$WeaponSlot) && (Player::getMountedItem(%player,$WeaponSlot) == "TankShredder")) 
	{
		Player::trigger(%player,$TankShredderSlotA,true);
		Player::trigger(%player,$TankShredderSlotB,true);
		Player::trigger(%player,$TankShredderSlotC,true);
		schedule("CheckTankShredder(" @ %client @ "," @ %player @ ");",0.1,%player);
		$FiringTankShredder[%client] = true;
	}
	else 
	{
		Player::trigger(%player,$TankShredderSlotA,false);
		Player::trigger(%player,$TankShredderSlotB,false);
		Player::trigger(%player,$TankShredderSlotC,false);
		$FiringTankShredder[%client] = false;
	}
}
