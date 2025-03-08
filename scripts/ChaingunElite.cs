//$InvList[Chaingun] = 1;			//1
//$MobileInvList[Chaingun] = 1;		//1
//$RemoteInvList[Chaingun] = 1;		//1

//$InvList[BulletAmmo] = 1;
//$MobileInvList[BulletAmmo] = 1;
//$RemoteInvList[BulletAmmo] = 1;

$AutoUse[ChaingunElite] = false;
$WeaponAmmo[ChaingunElite] = BulletAmmoElite;
$SellAmmo[BulletAmmoElite] = 50;

 addWeapon(ChaingunElite);
addAmmo(ChaingunElite, BulletAmmoElite, 50);

BulletData ChaingunBulletElite
{
   bulletShapeName    = "bullet.dts";
   explosionTag       = bulletExp0;
   expRandCycle       = 3;
   mass               = 0.05;
   bulletHoleIndex    = 0;

   damageClass        = 0;       // 0 impact, 1, radius
   damageValue        = 0.0825;
   damageType         = $BulletDamageType;

   aimDeflection      = 0.008;
   muzzleVelocity     = 425.0;
   totalTime          = 1.5;
   inheritedVelocityScale = 1.0;
   isVisible          = False;

   tracerPercentage   = 1.0;
   tracerLength       = 30;
};

ItemData BulletAmmoElite
{
	description = "Bullet";
	className = "Ammo";
	shapeFile = "ammo1";
   heading = $InvHead[ihAmm];
	shadowDetailMask = 4;
	price = 1;
};

ItemImageData ChaingunImageElite
{
	shapeFile = "chaingun";
	mountPoint = 0;

	weaponType = 1; // Spinning
	reloadTime = 0;
	spinUpTime = 0.5;
	spinDownTime = 3;
	fireTime = 0.2;

	ammoType = BulletAmmoElite;
	projectileType = ChaingunBulletElite;
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

ItemData ChaingunElite
{
	description = "Chaingun";
	className = "Weapon";
	shapeFile = "chaingun";
   //validateShape = true;
	hudIcon = "chain";
   heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = ChaingunImageElite;
	price = 125;
	showWeaponBar = true;
};

function ChaingunElite::MountExtras(%player,%weapon)
{	
	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	{
		bottomprint(%client, "<jc>Chaingun:<f2> The Legendary EliteRenegades Chaingun", 10);
	}
		
}
