
$AutoUse[SoulSucker] = false;
$WeaponAmmo[SoulSucker] = "";

//--------------------------------------

$InvList[SoulSucker] = 1;
$MobileInvList[SoulSucker] = 1;
$RemoteInvList[SoulSucker] = 1;


addWeapon(SoulSucker);

//--------------------------------------


SoundData SoundCrash
{
   wavFileName = "crash.wav";
   profile = Profile3dFar;
};

SoundData SoundNope
{
   wavFileName = "failpack.wav";
   profile = Profile3dMedium;
};



function SoulBolt::damageTarget(%target, %timeSlice, %damPerSec, %enDrainPerSec, %pos, %vec, %mom, %shooterId)
{
	%player = Client::getOwnedObject(%shooterId);
	%energy = GameBase::getEnergy(%player);
	%object = getObjectType(%target); 

	if (%energy < 10) 
		{
		Player::trigger(%shooterId, $WeaponSlot, false);
		if (!%shooterId.SoulMute) 
			{
			%shooterId.SoulMute = 1;
			schedule(%shooterId @ ".SoulMute--;",0.1,%shooterId);
			//GameBase::playSound( %shooterId,SoundNope,0);
			}
		Player::trigger(%shooterId, 0, false);
		return;
		}

	GameBase::playSound(%shooterId,SoundCrash,0);
	%shooterId.SoulMute = 1;
	schedule(%shooterId @ ".SoulMute--;",0.1,%shooterId);
	schedule("Player::trigger("@ %shooterId @", 0, false);", 1, %shooterId);

	if(%object == "Player" || %object == "Flier") 
	{ 
		SoulWarning(%shooterId, %target, %object);
		GameBase::applyDamage(%target, $SoulDamageType, 0.50, %pos, %vec, %mom, %shooterId);	// damages players more. //BR Setting
		if(GameBase::getDamageLevel(%player))
		{
			GameBase::repairDamage(%player, 0.05);	//added bonus for damaging players.
		}
	}
	GameBase::setEnergy(%player,0);
	GameBase::applyDamage(%target, $SoulDamageType, 0.1, %pos, %vec, %mom, %shooterId);	//turrets dont have souls..

}


function SoulSuckerImage::onFire(%player, %slot)
{		
	if($debug)
		Anni::Echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));		
	
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	%obj = Projectile::spawnProjectile("SoulBolt",%trans,%player,%vel);
	schedule("deleteobject("@%obj@");",0.5, %obj);
	schedule("Player::trigger("@%player@", 0, false);",1,%player);
}



ItemImageData SoulSuckerImage
{
   shapeFile = "mortar";	//paintgun";//shotgun
   mountPoint = 0;

   weaponType = 0;  // 2continuous 0singleshot 1spinning
   //projectileType = SoulBolt;//lightningcharge
   minEnergy = 30; 	//50
   maxEnergy = 150;		//150;  // Energy used/sec for sustained weapons 11
   reloadTime = 2;	//1;
	mountOffset = { 0, -0.22, -0.085};
   lightType = 3;  // Weapon Fire
   lightRadius = 2;
   lightTime = 1;
   lightColor = { 0.25, 0.25, 0.85 };
	//	sfxFire = SoundPickUpWeapon;
   sfxActivate = SoundPickUpWeapon;
   sfxFire     = SoundNope;
};

ItemImageData SoulSuckerRImage
{
	shapeFile = "paintgun";
	mountPoint = 0;
	mountRotation = { 0, 1.57, 3.1416 };
	mountOffset = { 0.08, 0.0, -0.085};
	weaponType = 0;
	reloadTime = 0.05;
	fireTime = 0;
	minEnergy = 5;
	maxEnergy = 6;
//	projectileType = OmegaBolt;
	accuFire = false;
	sfxFire = SoundJetHeavy;
	sfxActivate = SoundPickUpWeapon;
}; 

ItemImageData SoulSuckerLImage 
{
	shapeFile = "paintgun"; mountPoint = 0;
	mountRotation = { 0, -1.57, 3.1416 };
	mountOffset = { -0.08, 0.0, -0.085};
	weaponType = 0;
	reloadTime = 0.05;
	fireTime = 0;
	minEnergy = 5;
	maxEnergy = 6;
//	projectileType = OmegaBolt;
	accuFire = false;
	sfxFire = SoundJetHeavy;
	sfxActivate = SoundPickUpWeapon;
}; 

ItemImageData SoulSuckerBImage 
{
	shapeFile = "paintgun"; mountPoint = 0;
	mountRotation = { 0, 3.1416, 3.1416 };
	mountOffset = { 0, 0.0, 0.045};
	weaponType = 0;
	reloadTime = 0.05;
	fireTime = 0;
	minEnergy = 5;
	maxEnergy = 6;
//	projectileType = OmegaBolt;
	accuFire = false;
	sfxFire = SoundJetHeavy;
	sfxActivate = SoundPickUpWeapon;
}; 
ItemData SoulSucker
{
	description = "Soul Grabber";
	className = "Weapon";
	shapeFile = "mortar";	//;paintgun";//sniper
	hudIcon = "sniper";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = SoulSuckerImage;
	price = 200;
	showWeaponBar = true;
};
ItemData SoulSuckerR
{
	heading = "bWeapons";
	
	className = "Weapon";
	shapeFile = "shotgun";
	hudIcon = "plasma";
	shadowDetailMask = 4;
	imageType = SoulSuckerRImage;
	price = 1;
	showWeaponBar = false;
	showInventory = false;
}; 

ItemData SoulSuckerL
{
	heading = "bWeapons";
	
	className = "Weapon";
	shapeFile = "shotgun";
	hudIcon = "plasma";
	shadowDetailMask = 4;
	imageType = SoulSuckerLImage;
	price = 1;
	showWeaponBar = false;
	showInventory = false;
}; 


ItemData SoulSuckerB
{
	heading = "bWeapons";
	
	className = "Weapon";
	shapeFile = "shotgun";
	hudIcon = "plasma";
	shadowDetailMask = 4;
	imageType = SoulSuckerBImage;
	price = 1;
	showWeaponBar = false;
	showInventory = false;
}; 

function SoulSucker::MountExtras(%player,%weapon)
{		
	Player::mountItem(%player,SoulSuckerR,6); 
	Player::mountItem(%player,SoulSuckerL,7);
	Player::mountItem(%player,SoulSuckerB,5); 
	GameBase::playSound( %player,ForceFieldOpen,0);
	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>A short range weapon which steals the enemies life force.", 10);
}

function SoulWarning(%clientId, %target, %targetType) 
{ 
	%targetId = GameBase::getControlClient(%target);
	%targetName = Client::getName(%targetId); 
	%name = Client::getName(%clientId); 
	if(%targetType == Flier)
		{
		if(%targetName != "") 
			%msg = "Vehicle Piloted by " @ %targetName; 
		else  
			%msg = "Vehicle"; 
		} 
		else	%msg = %targetName;

	Client::sendMessage(%clientId,0,"Soul Lock Acquired: " @ %msg @ "~wmine_act.wav");
	if(%targetType == Flier || (Player::getArmor(%target)) != "") {
		Client::sendMessage(%targetId,0, %name @ " grabbed part of your soul!~waccess_denied.wav");
		schedule("Client::sendMessage(" @ %targetId @ ",0,\"~waccess_denied.wav\");", 0.5,%targetId);
		schedule("Client::sendMessage(" @ %targetId @ ",0,\"~waccess_denied.wav\");", 1.0,%targetId); 
	}
} 



