$InvList[Shotgun] = 1;
$MobileInvList[Shotgun] = 1;
$RemoteInvList[Shotgun] = 1;

$InvList[ShotgunShells] = 1;
$MobileInvList[ShotgunShells] = 1;
$RemoteInvList[ShotgunShells] = 1;

$AutoUse[Shotgun] = false;
$WeaponAmmo[Shotgun] = ShotgunShells;
$SellAmmo[ShotgunShells] = 15;

addWeapon(Shotgun);
addAmmo(Shotgun, ShotgunShells, 5);

ItemData ShotgunShells 
{
	description = "Shotgun Shells";
	className = "Ammo";
	shapeFile = "ammo2";
	heading = $InvHead[ihAmm];
	shadowDetailMask = 4;
	price = 2;
};
MineData ShotgunShellsBomb
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

ItemImageData ShotgunImage 
{
	shapeFile = "shotgun";
	mountPoint = 0;
	weaponType = 0;
	ammoType = ShotgunShells;
	reloadTime = 0.38;
	accuFire = false;
	fireTime = 0.25;
	sfxFire = SoundFireShotgun;
	sfxActivate = SoundPickUpWeapon;
};

ItemData Shotgun 
{
	description = "Shotgun";
	shapeFile = "shotgun";
	hudIcon = "ammopack";
	className = "Weapon";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = ShotgunImage;
	showWeaponBar = true;
	price = 85;
};

function Shotgun::MountExtras(%player,%weapon) 
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>Basically your standard double barreled shotgun.");
}

function ShotgunImage::onFire(%player, %slot) 
{		
	if($debug)
		Anni::Echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));	
		
	Player::decItemCount(%player,$WeaponAmmo[Shotgun],1);
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Projectile::spawnProjectile("ShotgunBullet",%trans,%player,%vel);
	Projectile::spawnProjectile("ShotgunBullet",%trans,%player,%vel);
	Projectile::spawnProjectile("ShotgunBullet",%trans,%player,%vel);
	Projectile::spawnProjectile("ShotgunBullet",%trans,%player,%vel);
	Projectile::spawnProjectile("ShotgunBullet",%trans,%player,%vel);
	Projectile::spawnProjectile("ShotgunBullet",%trans,%player,%vel);
	Projectile::spawnProjectile("ShotgunBullet",%trans,%player,%vel);
	Projectile::spawnProjectile("ShotgunBullet",%trans,%player,%vel);
	Projectile::spawnProjectile("ShotgunBullet",%trans,%player,%vel);
	Projectile::spawnProjectile("ShotgunBullet",%trans,%player,%vel);
}
