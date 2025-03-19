$InvList[AngelRepairGun] = 1;
$MobileInvList[AngelRepairGun] = 1;
$RemoteInvList[AngelRepairGun] = 1;

$AutoUse[AngelRepairGun] = False;
$WeaponAmmo[AngelRepairGun] = "";

addWeapon(AngelRepairGun);

// can be changed in game if needed
$AngelRepairRate = 0.01; //player heals
$AngelFeedbackRate = 0.0095; //self heals

LightningData AngelRepairBolt
{
	bitmapName = "paintpulse.bmp";
	damageType = $ElectricityDamageType;
   	boltLength       = 50.0;
   	coneAngle        = 25.0;
   	damagePerSec      = 0;
   	energyDrainPerSec = 0;
   	segmentDivisions = 2;
   	numSegments      = 5;
   	beamWidth        = 0.125;

   	updateTime   = 120;
   	skipPercent  = 0.9;
   	displaceBias = 0.15;

	lightRange = 3.0;
	lightColor = { 0.15, 0.85, 0.25 };
	soundId = SoundRepairItem;
};

function AngelRepairBolt::damageTarget(%target, %timeSlice, %damPerSec, %enDrainPerSec, %pos, %vec, %mom, %shooterId)
{
	%client = %shooterId;
	%type = GetObjectType(%target);
	%damagelvl = GameBase::getDamageLevel(%target); //echo(GameBase::getDamageLevel(2049));

	if(%type == "Player" && GameBase::getTeam(%target) == GameBase::getTeam(%shooterId) && %damagelvl != 0)
	{
		%name = Client::getName(Player::getClient(%target));
		Bottomprint(%client,"<jc><f1>Target <f2>"@ %name @" <f1>is <f2>"@floor(GameBase::getDamageLevel(%target)*100)@"<f1>% Damaged",2);
	   	GameBase::SetDamageLevel(%target, GameBase::getDamageLevel(%target)-$AngelRepairRate);
		GameBase::SetDamageLevel(%shooterId, GameBase::getDamageLevel(%target)-$AngelFeedbackRate);

	}
}
ItemImageData AngelRepairGunImage
{
	shapeFile = 	"paintgun";
	mountPoint = 0;
	mountRotation = {0, 1.57, 0};
	weaponType = 2; // Sustained

	projectileType = AngelRepairBolt;

	minEnergy = 3;
	maxEnergy = 8; // Energy used/sec for sustained weapons
	reloadTime = 0.2;

	lightType = 3; // Weapon Fire
	lightRadius = 2;
	lightTime = 1;
	lightColor = { 0, 0.95, 0.5 };

	sfxActivate = SoundPickUpWeapon;
	sfxFire = SoundBeaconActive;
};
ItemData AngelRepairGun
{
	className = "Weapon";
	description = "Bio Feedback Beam";
	shapeFile = "paintgun";
	shadowDetailMask = 4;
	heading = $InvHead[ihWea];
	hudIcon = "weapon";
	imageType = AngelRepairGunImage;
	showWeaponBar = true;
	showInventory = true;
	price = 2000;
};

function AngelRepairGun::MountExtras(%player,%weapon) 
{	
	
	if((Player::getclient(%player)).weaponHelp)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>Heal your teammates on the go.");
}
