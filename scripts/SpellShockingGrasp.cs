$InvList[ShockingGrasp] = 1;
$MobileInvList[ShockingGrasp] = 1;
$RemoteInvList[ShockingGrasp] = 1;

$AutoUse[ShockingGrasp] = False;
$WeaponAmmo[ShockingGrasp] = "";

addWeapon(ShockingGrasp);

LightningData ShockingGraspBolt
{	bitmapName = "rtrail3.bmp";
	damageType = $ElectricityDamageType;
	boltLength = 40.0;
	coneAngle = 45.0;
	damagePerSec = 0.40; //BR Setting
	energyDrainPerSec = 100.0;
	segmentDivisions = 2;
	numSegments = 5;
	beamWidth = 0.6;  //10
	updateTime = 120;
	skipPercent = 0.5;
	displaceBias = 0.15;
	lightRange = 3.0;
	lightColor = { 0.25, 0.25, 0.85 };
	soundId = SoundElfFire;
};
ItemImageData ShockingGraspImage 
{
	shapeFile = "breath";
	mountPoint = 0;
    mountOffset = { 0, -0.4, 0.1}; //-  left-right, back-front, up-down
    mountRotation = { 0, 0 , 0 };
	weaponType = 2; // Sustained
	projectileType = ShockingGraspBolt;
	minEnergy = 3;
	maxEnergy = 8; // Energy used/sec for sustained weapons
	reloadTime = 0.2;
	lightType = 3; // Weapon Fire
	lightRadius = 2;
	lightTime = 1;
	lightColor = { 0.25, 0.25, 0.85 };
	sfxReady = SoundJammerOn;
	sfxActivate = SoundPickUpWeapon;
	sfxFire = SoundEmpIdle; // SoundELFIdle is a loop!! -death666
};
ItemData ShockingGrasp
{
	className = "Weapon";
	description = "Spell: Shocking Grasp";
	shapeFile = "DSPLY_S1"; //mortartrail
	shadowDetailMask = 4;
	heading = $InvHead[ihSpl];
	hudIcon = "targetlaser";
	imageType = ShockingGraspImage;
	showWeaponBar = true;
	showInventory = true;
	price = 2000;
};

ItemImageData ShockingGrasp2Image
{
//	shapeFile = "mortartrail";
	shapeFile = "rocket"; //mortartrail
//	mountPoint = 2;
//	mountOffset = { 0.2, 0.4, 0.0 };
//	mountRotation = { -1.57, 0, 0 };

	mountPoint = 0;
    mountOffset = { 0, -0.7, 0.1}; //-  left-right, back-front, up-down
    mountRotation = { 3.14, 0 , 0 };

	weaponType = 2; // Sustained
	projectileType = ShockingGraspBolt;
	minEnergy = 3;
	maxEnergy = 8; // Energy used/sec for sustained weapons
	reloadTime = 0.2;
	lightType = 3; // Weapon Fire
	lightRadius = 2;
	lightTime = 1;
	lightColor = { 0.25, 0.25, 0.85 };
//	sfxReady = SoundJammerOn;
//	sfxActivate = SoundPickUpWeapon;
//	sfxFire = SoundELFIdle;
	showInventory = false;
};

ItemData ShockingGrasp2
{
	className = "Weapon";
	description = "Spell: Shocking Grasp";
	heading = $InvHead[ihSpl];
	hudIcon = "targetlaser";
	imageType = ShockingGrasp2Image;
	price = 350;
	shadowDetailMask = 4;
	shapeFile = "DSPLY_S1"; 
	showWeaponBar = false;
	showInventory = false;
};

function ShockingGraspBolt::damageTarget(%target, %timeSlice, %damPerSec, %enDrainPerSec, %pos, %vec, %mom, %shooterId)
{
	%client = %shooterId;
	%type = GetObjectType(%target);
	%damagelvl = GameBase::getDamageLevel(%target);

	if(%type == "Player" && GameBase::getTeam(%target) == GameBase::getTeam(%shooterId))
	{
		// %energy = GameBase::getEnergy(%target);
		// GameBase::setEnergy(%target,%energy + 0.2);
		return;
	}
	else
		%damVal = %timeSlice * %damPerSec;
	%enVal = %timeSlice * %enDrainPerSec;

	GameBase::applyDamage(%target, $ElectricityDamageType, %damVal, %pos, %vec, %mom, %shooterId);

	%energy = GameBase::getEnergy(%target);
	%energy = %energy - %enVal;
	if(%energy < 0) 
		%energy = 0;
	GameBase::setEnergy(%target, %energy);
		// %energy = GameBase::getEnergy(%target);
		// GameBase::setEnergy(%this,%energy - 40);
}

function ShockingGrasp::MountExtras(%player,%weapon) 
{	
	Player::mountItem(%player,ShockingGrasp2,4);
	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>This spell drains a target's energy while inflicting electrical damage rendering shields, jetpacks, and energy weapons useless.");
}
