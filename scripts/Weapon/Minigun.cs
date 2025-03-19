$InvList[Minigun] = 1;			//1
$MobileInvList[Minigun] = 1;		//1
$RemoteInvList[Minigun] = 1;		//1

$InvList[MinigunAmmo] = 1;
$MobileInvList[MinigunAmmo] = 1;
$RemoteInvList[MinigunAmmo] = 1;

$AutoUse[Minigun] = false;
$WeaponAmmo[Minigun] = MinigunAmmo;
$SellAmmo[MinigunAmmo] = 100;

addWeapon(Minigun);
addAmmo(Minigun, MinigunAmmo, 100);


BulletData MinigunBullet
{ 
   bulletShapeName = "bullet.dts"; 
   explosionTag = MiniGunEXP; 
   expRandCycle = 1; 
   mass = 0.1; 
   bulletHoleIndex = 0; 
   damageClass = 0; 
   damageValue = 0.11; //0.1 * $Nappy::ProjectileDamage;
   damageType = $MinigunDamageType;  //going to give it it's own damage type
   aimDeflection = 0.0013; 
   muzzleVelocity = 2500.0; 
   totalTime = 10; 
   inheritedVelocityScale = 1.0; //1.0
   isVisible = true; 
   tracerPercentage = 3.0;
   lightRange = 5.0;
   lightColor = { 1.0, 0.7, 0.5 }; 
   trailType = 1;
   trailLength = 1800;
   trailWidth = 0.7;
}; 

ItemData MinigunAmmo 
{	description = "Minigun Bolts";
	className = "Ammo";
	shapeFile = "ammo1";
	heading = $InvHead[ihAmm];
	shadowDetailMask = 4;
	price = 5;
};

ItemImageData MinigunImage 
{
	shapeFile = "chaingun";
	mountPoint = 0;
	weaponType = 1;
	reloadTime = 0;
	spinUpTime = 0.1;
	spinDownTime = 3;
	fireTime = 0.045;
	ammoType = MinigunAmmo;
	projectileType = MinigunBullet;
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

ItemImageData MinigunImage2
{
	shapeFile = "chaingun";
	mountPoint = 0;
	weaponType = 1;
	reloadTime = 0;
	spinUpTime = 0.1;
	spinDownTime = 3;
	fireTime = 0.045;
	ammoType = MinigunAmmo;
	projectileType = MinigunBullet;
	accuFire = true;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1 };
	mountRotation = { 0 , -2.5 , 0 };
	mountOffset = { -0.1 , 0 , -0.1 };
	sfxFire = SoundFireChaingun;
	sfxActivate = SoundPickUpWeapon;
	sfxSpinUp = SoundSpinUp;
	sfxSpinDown = SoundSpinDown;
};

ItemImageData MinigunImage3
{
	shapeFile = "chaingun";
	mountPoint = 0;
	weaponType = 1;
	reloadTime = 0;
	spinUpTime = 0.1;
	spinDownTime = 3;
	fireTime = 0.045;
	ammoType = MinigunAmmo;
	projectileType = MinigunBullet;
	accuFire = true;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1 };
	mountRotation = { 0 , 2.5 , 0 };
	mountOffset = { 0.1 , 0 , -0.1 };
	sfxFire = SoundFireChaingun;
	sfxActivate = SoundPickUpWeapon;
	sfxSpinUp = SoundSpinUp;
	sfxSpinDown = SoundSpinDown;
};

ItemData Minigun 
{
	description = "Minigun";
	className = "Weapon";
	shapeFile = "chaingun";
	hudIcon = "chain";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = MinigunImage;
	price = 125;
	showWeaponBar = true;
};

ItemData Minigun2
{
	description = "Minigun2";
	className = "Weapon";
	shapeFile = "chaingun";
	hudIcon = "chain";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = MinigunImage2;
	price = 125;
	showWeaponBar = true;
};

ItemData Minigun3
{
	description = "Minigun3";
	className = "Weapon";
	shapeFile = "chaingun";
	hudIcon = "chain";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = MinigunImage3;
	price = 125;
	showWeaponBar = true;
};

// Weapon Options - AUTO
function MinigunImage::onFire(%player, %slot) 
{		
	if($debug)
		Anni::Echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));	
	
	%client = GameBase::getOwnerClient(%player);
	%vel = Item::getVelocity(%player);
	%trans = GameBase::getMuzzleTransform(%player);
	Projectile::spawnProjectile("MinigunBullet",%trans,%player,%vel);
	CheckMinigun(%client, %player);
}

function Minigun::Ejector(%player)
{	
	if(Player::isTriggered(%player,0))
	{
		//Player::trigger(%player,5,true);
		//Player::trigger(%player,6,true);
		schedule("Minigun::ejector("@%player@");",0.1,%player);
	}
	else
	{
		%player.Minigunfiring = false;
		//Player::trigger(%player,5,false);
		//Player::trigger(%player,6,false);
		schedule("Player::trigger("@%player@", 4, false);",0.1,%player);
		
	}
}



//ItemImageData MinigunEjectorimage
//{
//	shapeFile = "force";
//	mountPoint = 0;
//	mountOffset = { 0.09, 0.05, 0.01 };//right, forward, up	//0.1, 0.25, 0.01
//	mountRotation = {0.5,-1.57, -1.57 };	//0.5,-1.57, -1.57
//	accuFire = false;
//	projectileType = SpentShell;
//	maxEnergy = 0;	//energy/sec
//
//	weaponType = 1;
//	reloadTime = 0;
//	spinUpTime = 0.0;
//	spinDownTime = 3;
//	fireTime = 0.25;
//	reloadTime = 0.90;
//};

//ItemData MinigunEjector
//{
//	description = "Ejector";
//	className = "Weapon";
//	shapeFile = "force";
//	hudIcon = "mortar";
//	heading = $InvHead[ihWea];
//	shadowDetailMask = 4;
//	imageType = MinigunEjectorimage;
//	price = 375;
//	showWeaponBar = true;
//};



// END Weapon Options


function Minigun::MountExtras(%player,%weapon)
{		
	//Player::mountItem(%player,MinigunEjector,4);
	Player::mountItem(%player,Minigun2,6);
	Player::mountItem(%player,Minigun3,7);
	
	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	{
		bottomprint(%client, "<jc>"@%weapon.description@": <f2> Capable of firing at high velocity!", 2);
	}	
		
}

function CheckMinigun(%client, %player) 
{
	if(Player::isTriggered(%player,$WeaponSlot) && (Player::getMountedItem(%player,$WeaponSlot) == "Minigun")) 
	{
		Player::trigger(%player,6,true);
		Player::trigger(%player,7,true);
		schedule("CheckMinigun(" @ %client @ "," @ %player @ ");",0.1);
		$FiringMinigun[%client] = true;
	}
	else 
	{
		Player::trigger(%player,6,false);
		Player::trigger(%player,7,false);
		$FiringMinigun[%client] = false;
	}
}
