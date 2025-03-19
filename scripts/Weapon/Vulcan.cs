$InvList[Vulcan] = 1;			//1
$MobileInvList[Vulcan] = 1;		//1
$RemoteInvList[Vulcan] = 1;		//1

$InvList[VulcanAmmo] = 1;
$MobileInvList[VulcanAmmo] = 1;
$RemoteInvList[VulcanAmmo] = 1;

$AutoUse[Vulcan] = false;
$WeaponAmmo[Vulcan] = VulcanAmmo;
$SellAmmo[VulcanAmmo] = 100;

addWeapon(Vulcan);
addAmmo(Vulcan, VulcanAmmo, 100);

ExplosionData SpentShellExp
{
	shapeName = "tumult_small.dts";
	//soundId = debrisSmallExplosion;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 2.5;
	timeZero = 0.250;
	timeOne = 0.650;
	colors[0] = { 0.0, 0.0, 0.0 };
	colors[1] = { 1.0, 0.5, 0.16 };
	colors[2] = { 1.0, 0.5, 0.16 };
	radFactors = { 0.0, 1.0, 1.0 };
};
GrenadeData SpentShell
{	bulletShapeName = "force.dts";	//bullet
	explosionTag = SpentShellExp;
	collideWithOwner = True;
	ownerGraceMS = 250;
	collisionRadius = 0.2;
	mass = 0.25;
	elasticity = 0.5;
	damageClass = 1;
	damageValue = 0.0;
	damageType = $ShrapnelDamageType;
	explosionRadius = 8;
	kickBackStrength = 0;
	maxLevelFlightDist = 2;
	totalTime = 3.0;
	liveTime = 1.0;	//after collision
	projSpecialTime = 0.005;	//smoke time
	inheritedVelocityScale = 0.5;
	smokeName = "breath.dts";	//rsmoke
	smokeDist = 0.01;	//1.5;
};
ItemData VulcanAmmo 
{
	description = "Vulcan Bullet";
	className = "Ammo";
	shapeFile = "ammo1";
	heading = $InvHead[ihAmm];
	shadowDetailMask = 4;
	price = 1;
};
MineData VulcanAmmoBomb
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

ItemImageData VulcanImage 
{
	shapeFile = "chaingun";
	mountPoint = 0;
	weaponType = 1;
	reloadTime = 0;
	spinUpTime = 0.75;
	spinDownTime = 3;
	fireTime = 0.045;
	ammoType = VulcanAmmo;
	//projectileType = VulcanBullet;
	accuFire = true;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1 };
	sfxFire = SoundFireChaingun;
	sfxActivate = SoundPickUpWeapon;
	sfxSpinUp = SoundSpinUp;
	sfxSpinDown = SoundSpinDown;
};

ItemData Vulcan 
{
	description = "Vulcan";
	className = "Weapon";
	shapeFile = "chaingun";
	hudIcon = "chain";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = VulcanImage;
	price = 125;
	showWeaponBar = true;
};

// Weapon Options - AUTO
function VulcanImage::onFire(%player, %slot) 
{		
	if($debug)
		Anni::Echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));	
	
	%AmmoCount = Player::getItemCount(%player, $WeaponAmmo[Vulcan]);
	%clientId = Player::getClient(%player);
	if(%AmmoCount > 0)
	{
		%vel = Item::getVelocity(%player);
		%trans = GameBase::getMuzzleTransform(%player);

		if (%clientId.Vulcan == 1)
		{
			Projectile::spawnProjectile("VulcanBullet",%trans,%player,%vel);
			Player::decItemCount(%player,VulcanAmmo);
			Player::decItemCount(%player,VulcanAmmo);
		}
		else
		{
			Projectile::spawnProjectile("ChaingunBullet",%trans,%player,%vel);
			Player::decItemCount(%player,VulcanAmmo);
		}
		if(!%player.vulcanfiring)
		{
			%player.vulcanfiring = true;
			Player::trigger(%player, 4, true);
			vulcan::ejector(%player);	
		}
	}
}

function vulcan::Ejector(%player)
{	
	%AmmoCount = Player::getItemCount(%player, $WeaponAmmo[Vulcan]);	
	if(Player::isTriggered(%player,0) && %AmmoCount)
		schedule("vulcan::ejector("@%player@");",0.1,%player);
	else
	{
		schedule("Player::trigger("@%player@", 4, false);",0.1,%player);
		%player.vulcanfiring = false;
	}
}



ItemImageData VulcanEjectorimage
{
	shapeFile = "force";
	mountPoint = 0;
	mountOffset = { 0.09, 0.05, 0.01 };//right, forward, up	//0.1, 0.25, 0.01
	mountRotation = {0.5,-1.57, -1.57 };	//0.5,-1.57, -1.57
	accuFire = false;
	projectileType = SpentShell;
	maxEnergy = 0;	//energy/sec

	weaponType = 1;
	reloadTime = 0;
	spinUpTime = 0.0;
	spinDownTime = 3;
	fireTime = 0.25;
	reloadTime = 0.90;
};

ItemData VulcanEjector
{
	description = "Ejector";
	className = "Weapon";
	shapeFile = "force";
	hudIcon = "mortar";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = VulcanEjectorimage;
	price = 375;
	showWeaponBar = true;
};

function Vulcan::MountExtras(%player,%weapon)
{		
	Player::mountItem(%player,VulcanEjector,4);
	
	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	{
		if (!%client.Vulcan || %client.Vulcan == 0)
			bottomprint(%client, "<jc>"@%weapon.description@": <f2> Standard Chaingun Rounds.\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.", 2);
		if (%client.Vulcan == 1)
			bottomprint(%client, "<jc>"@%weapon.description@": <f2> Fiery Vulcan Rounds.\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.", 2);
	}	
		
}
