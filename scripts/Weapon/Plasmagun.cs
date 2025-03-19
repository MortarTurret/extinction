$InvList[PlasmaGun] = 1;
$MobileInvList[PlasmaGun] = 1;
$RemoteInvList[PlasmaGun] = 1;

$InvList[PlasmaAmmo] = 1;
$MobileInvList[PlasmaAmmo] = 1;
$RemoteInvList[PlasmaAmmo] = 1;

$AutoUse[PlasmaGun] = false;
$SellAmmo[PlasmaAmmo] = 5;
$WeaponAmmo[PlasmaGun] = PlasmaAmmo;

addWeapon(PlasmaGun);
addAmmo(PlasmaGun, PlasmaAmmo, 5);

BulletData PlasmaBolt
{
	bulletShapeName = "plasmabolt.dts";
	explosionTag = plasmaExp;
	damageClass = 1;
	damageValue = 0.45;
	damageType = $PlasmaDamageType;
	explosionRadius = 4.0;
	muzzleVelocity = 100.0; //BR Setting
	totalTime = 3.0;
	liveTime = 2.0;
	lightRange = 3.0;
	lightColor = { 1, 1, 0 };
	inheritedVelocityScale = 0.3;
	isVisible = True;
	soundId = SoundJetLight;
};

ItemData PlasmaAmmo
{
	description = "PlasmaBolt";
	heading = $InvHead[ihAmm];
	className = "Ammo";
	shapeFile = "plasammo";
	shadowDetailMask = 4;
	price = 2;	
};

MineData PlasmaAmmoBomb
{
	mass = 5.0;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Halo";
	shapeFile = "plasmabolt";
	shadowDetailMask = 4;
	explosionId = flashExpSmall;	//mineExp;
	explosionRadius = 5.0;
	damageValue = 0.0;	//0.5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 10.5;
};

ItemImageData PlasmaGunImage
{
	shapeFile = "plasma";
	mountPoint = 0;
	weaponType = 0;
	ammoType = PlasmaAmmo;
	projectileType = PlasmaBolt;
	accuFire = true;
	reloadTime = 0.1;
	fireTime = 0.25;
	lightType = 3;
	lightRadius = 4;
	lightTime = 1;
	lightColor = { 1, 1, 0.2 };
	sfxFire = SoundFirePlasma;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;
};

ItemData PlasmaGun
{
	description = "Plasma Gun";
	className = "Weapon";
	shapeFile = "plasma";
	hudIcon = "plasma";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = PlasmaGunImage;
	price = 175;
	showWeaponBar = true;
};

function Plasmagun::MountExtras(%player,%weapon)
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>Fires flaming plasma to toast those marshmallows. Excels against covered enemy defenses.");
}

function PlasmaGunImageXX::onFire(%player)
{
	if($debug)
		Anni::Echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));
		
	Player::decItemCount(%player,$WeaponAmmo[PlasmaGun],1);
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Projectile::spawnProjectile("PlasmaBolt",%trans,%player,%vel);
	%player.invulnerable = false;
}
