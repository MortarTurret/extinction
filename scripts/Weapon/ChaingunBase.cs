//$InvList[Chaingun] = 1;			//1
//$MobileInvList[Chaingun] = 1;		//1
//$RemoteInvList[Chaingun] = 1;		//1

//$InvList[BulletAmmo] = 1;
//$MobileInvList[BulletAmmo] = 1;
//$RemoteInvList[BulletAmmo] = 1;

$AutoUse[ChaingunBase] = false;
$WeaponAmmo[ChaingunBase] = BulletAmmoBase;
$SellAmmo[BulletAmmoBase] = 50;

 addWeapon(ChaingunBase);
addAmmo(ChaingunBase, BulletAmmoBase, 50);

BulletData ChaingunBulletBase
{
   bulletShapeName    = "bullet.dts";
   //validateShape      = true;
   explosionTag       = bulletExp0;
   expRandCycle       = 3;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.11;
   damageType         = $BulletDamageType;

   aimDeflection      = 0.005;
   muzzleVelocity     = 425.0;
   totalTime          = 1.5;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 1.0;
   tracerLength       = 30;
};

ItemData BulletAmmoBase
{
	description = "Bullet";
	className = "Ammo";
	shapeFile = "ammo1";
   heading = $InvHead[ihAmm];
	shadowDetailMask = 4;
	price = 1;
};

ItemImageData ChaingunImageBase
{
	shapeFile = "chaingun";
	mountPoint = 0;

	weaponType = 1; // Spinning
	reloadTime = 0;
	spinUpTime = 0.5;
	spinDownTime = 3;
	fireTime = 0.2;

	ammoType = BulletAmmoBase;
	projectileType = ChaingunBulletBase;
	accuFire = false;

	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1 };

	sfxFire = SoundFireChaingun;
	sfxActivate = SoundPickUpWeapon;
	sfxSpinUp = SoundSpinUp;
	sfxSpinDown = SoundSpinDown;
};

ItemData ChaingunBase
{
	description = "Chaingun";
	className = "Weapon";
	shapeFile = "chaingun";
   //validateShape = true;
	hudIcon = "chain";
   heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = ChaingunImageBase;
	price = 125;
	showWeaponBar = true;
};

function ChaingunBase::MountExtras(%player,%weapon)
{	
	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	{
		bottomprint(%client, "<jc>Chaingun:<f2> The Legendary Base Chaingun", 10);
	}
		
}
