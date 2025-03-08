$InvList[TargetingLaser] = 1;
$MobileInvList[TargetingLaser] = 1;
$RemoteInvList[TargetingLaser] = 1;

AddItem(TargetingLaser);	
$AutoUse[TargetingLaser] = False;


TargetLaserData targetLaser
{
	laserBitmapName = "paintPulse.bmp";
	damageConversion = 0.0;
	baseDamageType = 0;
	lightRange = 20.0;
	lightColor = { 0.25, 1.0, 0.25 };
	detachFromShooter = false;
};

ItemImageData TargetingLaserImage 
{
	shapeFile = "paintgun";
	mountPoint = 0;
	weaponType = 2;
	projectileType = targetLaser;
	accuFire = true;
	minEnergy = 5;
	maxEnergy = 2;	//15
	reloadTime = 1.0;
	lightType = 3;
	lightRadius = 1;
	lightTime = 1;
	lightColor = { 0.25, 1, 0.25 };
	sfxFire = SoundFireTargetingLaser;
	sfxActivate = SoundPickUpWeapon;
};

ItemData TargetingLaser 
{
	description = "Targeting Laser";
	className = "Tool";
	shapeFile = "paintgun";
	hudIcon = "targetlaser";
	heading = $InvHead[ihtool];	//$InvHead[ihWea];	3.0
	shadowDetailMask = 4;
	imageType = TargetingLaserImage;
	price = 50;
	showWeaponBar = false;
};

function TargetingLaser::MountExtras(%player,%weapon)
{	
	%clientId = Player::getclient(%player);
	if(%clientId.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		bottomprint(%clientId, "<jc>Target Laser: <f2>The best rangefinding laser pointer around. Deals no damage.");
}

function TargetingLaser::onUse(%player)
{	
	%clientId = Player::getclient(%player);
	if($build && %clientId.CuttingLaser == 1)
	{
		
		Player::mountItem(%player,CuttingLaser,0);
		bottomprint(%clientId, "<jc>Cutting Laser: <f2>Watch your eyes.");
	}
	else
		Player::mountItem(%player,TargetingLaser,0);			
}
