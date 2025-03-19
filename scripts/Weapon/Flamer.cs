$InvList[Flamer] = 1;
$MobileInvList[Flamer] = 1;
$RemoteInvList[Flamer] = 1;

$InvList[FlamerAmmo] = 1;
$MobileInvList[FlamerAmmo] = 1;
$RemoteInvList[FlamerAmmo] = 1;

$WeaponAmmo[Flamer] = FlamerAmmo;
$AutoUse[Flamer] = false;
$SellAmmo[FlamerAmmo] = 10;

addWeapon(Flamer);
addAmmo(Flamer, FlamerAmmo, 5);

ItemData FlamerAmmo
{
	description = "FlamerAmmo";
	heading = $InvHead[ihAmm];
	className = "Ammo";
	shapeFile = "plasammo";
	shadowDetailMask = 4;
	price = 2;
};

MineData FlamerAmmoBomb
{
	mass = 5.0;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Halo";
	shapeFile = "tumult_large";
	shadowDetailMask = 4;
	explosionId = flashExpSmall;	//mineExp;
	explosionRadius = 5.0;
	damageValue = 0.0;	//0.5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 10.5;
};


ItemImageData FlamerImage
{
	shapeFile = "shotgun";
	mountPoint = 0;
	ammoType = FlamerAmmo;
	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = 0.15;
	accuFire = false;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 1.0, 0.7, 0.5 };
	sfxActivate = SoundPickUpWeapon;
	sfxFire = SoundFireFlamer;
};

ItemData Flamer
{
	description = "Flamer";
	shapeFile = "shotgun";
	hudIcon = "sensorjamerpack";
	className = "Weapon";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = FlamerImage;
	showWeaponBar = true;
	price = 2500;
};

function FlamerImage::onFire(%player, %slot) 
{		
	if($debug)
		Anni::Echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));	

	%AmmoCount = Player::getItemCount(%player, $WeaponAmmo[Flamer]);
	if(%AmmoCount) 
	{
		%client = GameBase::getOwnerClient(%player);
		Player::decItemCount(%player,$WeaponAmmo[Flamer],1);
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);
		Projectile::spawnProjectile("Flames",%trans,%player,%vel);
		Projectile::spawnProjectile("FlameSmoke",%trans,%player); 
	}
	else
		Client::sendMessage(Player::getClient(%player), 0,"Out Of FlamerAmmo");
}

function Flamer::MountExtras(%player,%weapon)
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>Fires flaming gasoline to engulf your targets.");
}
