// mini bomber
// grenade, tripple grenade, mine dropper, starburst mine

$InvList[GrenadeLauncher] = 1;
$MobileInvList[GrenadeLauncher] = 1;
$RemoteInvList[GrenadeLauncher] = 1;

$InvList[GrenadeAmmo] = 1;
$MobileInvList[GrenadeAmmo] = 1;
$RemoteInvList[GrenadeAmmo] = 1;

$AutoUse[GrenadeLauncher] = false;
$SellAmmo[GrenadeAmmo] = 5;
$WeaponAmmo[GrenadeLauncher] = GrenadeAmmo;

addWeapon(GrenadeLauncher);
addAmmo(GrenadeLauncher, GrenadeAmmo, 2);

ItemData GrenadeAmmo 
{	
	description = "Mini ammo"; 
	className = "Ammo"; 
	shapeFile = "grenammo"; 
	heading = $InvHead[ihAmm]; 
	shadowDetailMask = 4; 
	price = 2;
}; 

MineData GrenadeAmmoBomb
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
ItemImageData GrenadeLauncherImage 
{	
	shapeFile = "grenadeL";
	mountPoint = 0;
	weaponType = 0;
	ammoType = GrenadeAmmo;
	//projectileType = BomberWarhead;
	accuFire = false;
	reloadTime = 0.8;
	fireTime = 0.5;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };
	sfxFire = SoundPickUpWeapon;	//SoundTurretDeploy;	//SoundFireGrenade;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;
};

ItemData GrenadeLauncher 
{	
	description = "Mini Bomber";
	className = "Weapon";
	shapeFile = "grenadeL";
	hudIcon = "grenade";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = GrenadeLauncherImage;
	price = 150;
	showWeaponBar = true;
};

function GrenadeLauncher::MountExtras(%player,%weapon)
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));

	%clientId = Player::getclient(%player);
	if(%clientId.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	{
		if(%clientId.MiniMode == 1)
			bottomprint(%clientId, "<jc>Mini Bomber: <f2>SINGLE <f2>Grenade Projectile.\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.", 2);
		//else if(%clientId.MiniMode == 2)
		//	bottomprint(%clientId, "<jc>Mini Bomber: <f2>MINE dropper<f2> Projectile.\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.", 2);
		//else if(%clientId.MiniMode == 3)
		//	bottomprint(%clientId, "<jc>Mini Bomber: <f2>Starburst MINE <f2>Projectile.\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.", 2);
		else if(%clientId.MiniMode == 2)
			bottomprint(%clientId, "<jc>Mini Bomber: <f2>EMP <f2>Grenade Projectile.\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.", 2);
		else
			bottomprint(%clientId, "<jc>Mini Bomber: <f2>TRIPLE <f2>Grenade Projectile.\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.", 2);
	}	
}

function GrenadelauncherImage::onFire(%player, %slot)
{		
	if($debug)
		Anni::Echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));		

	%AmmoCount = Player::getItemCount(%player, $WeaponAmmo[GrenadeLauncher]);
	
	%clientId = Player::getClient(%player);
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);

	if(%clientId.MiniMode == 1)
	{
		if(%AmmoCount > 0)
		{
			Projectile::spawnProjectile("GrenadeLauncherGren",%trans,%player,%vel,%player);
			Player::decItemCount(%player,$WeaponAmmo[GrenadeLauncher],1);
			playSound(SoundFireGrenade, GameBase::getPosition(%clientId));
		}
		else
		{
			Client::sendMessage(Player::getClient(%player),1,"Grenade Launcher out of ammo.");
			return false;
		}
	}

	else if(%clientId.MiniMode == 2)
	{
		if(%AmmoCount > 0)
		{
			Projectile::spawnProjectile("EMPGrenadeShell",%trans,%player,%vel,%player);
			Player::decItemCount(%player,$WeaponAmmo[GrenadeLauncher],1);
			playSound(SoundFireGrenade, GameBase::getPosition(%clientId));
		}
		else
		{
			Client::sendMessage(Player::getClient(%player),1,"Grenade Launcher out of ammo.");
			return false;
		}
	}
	else
	{
		if(%AmmoCount > 1)
		{
			Projectile::spawnProjectile("GrenadeLauncherGren1",%trans,%player,%vel,%player);
			Projectile::spawnProjectile("GrenadeLauncherGren2",%trans,%player,%vel,%player);
			Projectile::spawnProjectile("GrenadeLauncherGren3",%trans,%player,%vel,%player);
			Player::decItemCount(%player,$WeaponAmmo[GrenadeLauncher],2);
			playSound(SoundFireGrenade, GameBase::getPosition(%clientId));
		}
		else
		{
			Client::sendMessage(Player::getClient(%player),1,"Not enough ammo to fire Triple Projectiles.");//~wdryfire1.wav");
			return false;
		}
	}
}

function SplitMines(%newobj,%this)
{
	%Pos = GameBase::getPosition(%newobj); 
	%vel = Item::getVelocity(%newobj);
	if(vector::normalize(%vel) != "-NAN -NAN -NAN")	
	{	
	// Pretty shell split
		%trans =  "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%newObj);
	 	%obj = Projectile::spawnProjectile("PrettySplit", %trans, %this, %vel);
		Projectile::spawnProjectile(%obj);
		GameBase::setPosition(%obj, %pos);
		Item::setVelocity(%obj, %vel);
	}
	else
		Anni::Echo("!! Butterfly Error, Splitmines. vel ="@%vel);		
}