$InvList[Blaster] = 1;
$MobileInvList[Blaster] = 1;
$RemoteInvList[Blaster] = 1;

$WeaponAmmo[Blaster] = "";

$AutoUse[Blaster] = false;
addWeapon(Blaster);

//--------------------------------------

//	ExplosionData blasterExp	defined in baseexpdata, used elsewhere..


//	BulletData BlasterBolt		defined in baseprojdata, used elsewhere..
BulletData BlasterBolt
{
   bulletShapeName    = "enex.dts";
   explosionTag       = blasterExp2;

   damageClass        = 0;
   damageValue        = 0.09;
   damageType         = $BlasterDamageType;

   muzzleVelocity     = 270.0;
   totalTime          = 3.0;
   liveTime           = 2.125;

   lightRange         = 3.0;
   lightColor         = { 1.0, 0.25, 0.25 };
   inheritedVelocityScale = 0.5;
   isVisible          = True;

   rotationPeriod = 1;
};

BulletData BlasterBolt2
{
   bulletShapeName    = "paint.dts";
   explosionTag       = blasterExp3;

   damageClass        = 0;
   damageValue        = 0.09;
   damageType         = $BlasterDamageType;

   muzzleVelocity     = 270.0;
   totalTime          = 3.0;
   liveTime           = 2.125;

   lightRange         = 3.0;
   lightColor         = { 1.0, 0.25, 0.25 };
   inheritedVelocityScale = 0.5;
   isVisible          = True;

   rotationPeriod = 1;
};

BulletData BlasterBolt3
{
   bulletShapeName    = "shotgunbolt.dts";
   explosionTag       = blasterExp4;

   damageClass        = 0;
   damageValue        = 0.09;
   damageType         = $BlasterDamageType;

   muzzleVelocity     = 270.0;
   totalTime          = 3.0;
   liveTime           = 2.125;

   lightRange         = 3.0;
   lightColor         = { 1.0, 0.25, 0.25 };
   inheritedVelocityScale = 0.5;
   isVisible          = True;

   rotationPeriod = 1;
};

//----------------------------------------------------------------------------

ItemImageData BlasterImage
{
   shapeFile  = "energygun";
	mountPoint = 0;

	weaponType = 0; // Single Shot
	reloadTime = 0;
	fireTime = 0.4;
	minEnergy = 5;
	maxEnergy = 6;

	accuFire = true;

	sfxFire = SoundFireBlaster;
	sfxActivate = SoundPickUpWeapon;
};

ItemData Blaster
{
   heading = $InvHead[ihWea];
	description = "Blaster";
	className = "Weapon";
   shapeFile  = "energygun";
	hudIcon = "blaster";
	shadowDetailMask = 4;
	imageType = BlasterImage;
	price = 85;
	showWeaponBar = true;
};

function BlasterImage::onFire(%player, %slot) 
{ 

%trans = getPerfectTrans(%player); 
%vel = Item::getVelocity(%player); 
Projectile::spawnProjectile("BlasterBolt",%trans,%player,%vel); 
Projectile::spawnProjectile("BlasterBolt2",%trans,%player,%vel); 
Projectile::spawnProjectile("BlasterBolt3",%trans,%player,%vel); 

} 

function Blaster::MountExtras(%player,%weapon)
{	
	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	{
		bottomprint(%client, "<jc>Blaster v2:<f2> An supercharged upgrade of the classic Blaster.", 10);
	}
		
}