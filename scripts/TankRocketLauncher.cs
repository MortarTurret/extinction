$InvList[TRocketLauncher] = 1;
$MobileInvList[TRocketLauncher] = 1;
$RemoteInvList[TRocketLauncher] = 1;

$InvList[TRocketLauncherAmmo] = 1;
$MobileInvList[TRocketLauncherAmmo] = 1;
$RemoteInvList[TRocketLauncherAmmo] = 1;
$SellAmmo[TRocketLauncherAmmo] = 10;

$AutoUse[TRocketLauncher] = false;
$WeaponAmmo[TRocketLauncher] = TRocketLauncherAmmo;

addweapon(TRocketLauncher);
addAmmo(TRocketLauncher, TRocketLauncherAmmo, 8);

$TRocketLauncherSlotA=4;
$TRocketLauncherSlotB=7;
$TRocketLauncherSlotC=6; 

ItemData TRocketLauncherAmmo
{
	description = "TRocketLauncher Ammo";
	classname = "Ammo";
	shapeFile = "mortarammo";		
	heading = $InvHead[ihAmm];
	shadowDetailMask = 4;
	price = 50;
};
MineData TRocketLauncherAmmoBomb
{
	mass = 5.0;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Halo";
	shapeFile = "rocket";
	shadowDetailMask = 4;
	explosionId = flashExpSmall;	//mineExp;
	explosionRadius = 5.0;
	damageValue = 0.0;	//0.5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 10.5;
};

ItemImageData TRocketLauncherImage 
{
	shapeFile = "mortargun"; 
	mountPoint = 0; 
	mountOffset = { -1.346, 0.08, 0.01 }; 
	mountRotation = { 0, 1.575, 0 }; 
	weaponType = 0; 
	reloadTime = 1.0; 
	fireTime = 0.1;
	minEnergy = 5;	
	maxEnergy = 6;
	ammoType = TRocketLauncherAmmo; 
	accuFire = true; 
	sfxFire = SoundMissileTurretFire; 
	sfxActivate = SoundPickUpWeapon; 
	sfxReload = SoundMortarReload;
	sfxReady = SoundMortarIdle;
}; 

ItemData TRocketLauncher 
{
	description = "Tank Rocketgun"; 
	className = "Weapon"; 
	shapeFile = "mortargun"; 
	mountOffset = { -1.346, 0.08, 0.01 }; 
	mountRotation = { 0, 1.575, 0 }; 
	hudIcon	= "sensorjamerpack";
	heading = $InvHead[ihWea]; 
	shadowDetailMask = 4; 
	imageType = TRocketLauncherImage; 
	price = 2500; 
	showWeaponBar = true; 
}; 

ItemImageData TRocketLauncher2Image 
{
	shapeFile = "mortargun"; 
	mountPoint = 0; 
	mountOffset = { -1.21, -0.45, 0 }; 
	mountRotation = { 0, 0, 0 }; 
	weaponType = 0; 
	reloadTime = 1.0; 
	fireTime = 0.1;
	ammoType = TRocketLauncherAmmo; 
	accuFire = true; 
	sfxFire = SoundMissileTurretFire; 
	minEnergy = 5;	
	maxEnergy = 6;
};	

ItemData TRocketLauncher2 
{
	description = "Tank Rocketgun"; 
	className = "Weapon"; 
	shapeFile = "mortargun"; 
	shadowDetailMask = 4; 
	imageType = TRocketLauncher2Image; 
	price = 0; 
	showWeaponBar = false; 
	showInventory = false; 
}; 

ItemImageData TRocketLauncher3Image 
{
	shapeFile = "mortargun"; 
	mountPoint = 0; 
	mountOffset = { 0, -0.45, 0 }; 
	mountRotation = { 0, 0, 0 }; 
	weaponType = 0; 
	reloadTime = 1.0; 
	fireTime = 0.1;
	ammoType = TRocketLauncherAmmo; 
	accuFire = true; 
	sfxFire = SoundMissileTurretFire; 
	minEnergy = 5;	
	maxEnergy = 6;
}; 

ItemData TRocketLauncher3 
{
	description = "Tank Rocketgun"; 
	className = "Weapon"; 
	shapeFile = "mortargun";
	shadowDetailMask = 4;
	imageType = TRocketLauncher3Image;
	price = 0;
	showWeaponBar = false;
	showInventory = false;
}; 

ItemImageData TRocketLauncher4Image 
{
	shapeFile = "mortargun";
	mountPoint = 0; 
	mountOffset = { 0.15, 0.08, 0.01 };
	mountRotation = { 0, -1.575, 0}; 
	weaponType = 0; 
	reloadTime = 1.0; 
	fireTime = 0.1;
	ammoType = TRocketLauncherAmmo; 
	accuFire = true; 
	sfxFire = SoundMissileTurretFire; 
	minEnergy = 5;	
	maxEnergy = 6;
}; 

ItemData TRocketLauncher4 
{
	description = "Tank Rocketgun"; 
	className = "Weapon"; 
	shapeFile = "mortargun"; 
	shadowDetailMask = 4; 
	imageType = TRocketLauncher4Image; 
	price = 0; 
	showWeaponBar = false;
	showInventory = false; 
}; 

//  Begin TRocketLauncher Fire Function

function TRocketLauncherImage::onFire(%player, %slot) 
{		
	if($debug)
		Anni::Echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));		
	
	%AmmoCount = Player::getItemCount(%player, $WeaponAmmo[TRocketLauncher]);
	if(%AmmoCount)
	{
		%client = GameBase::getOwnerClient(%player);
		%clientName = Player::getClient(%player);
		%clientId = Client::getName(%client);
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);
		
		// ------- Second Projectile Placement --------
		
		%pos=gamebase::getposition(%player);
		%rot=gamebase::getrotation(%player);
		%vec=Vector::getFromRot(%rot);
		
		%vec1=getWord(%vec,0);
		%vec2=getWord(%vec,1);
		
		%pos1=getWord(%trans,0);
		%pos2=getWord(%trans,1);
		%pos3=getWord(%trans,2);
		%pos4=getWord(%trans,3);
		%pos5=getWord(%trans,4);
		%pos6=getWord(%trans,5);
		%pos7=getWord(%trans,6);
		%pos8=getWord(%trans,7);
		%pos9=getWord(%trans,8);
		%pos10=getWord(%trans,9) + %vec2;
		%pos11=getWord(%trans,10) - %vec1;
		%pos12=getWord(%trans,11);
		
		%trans2=%pos1@" "@%pos2@" "@%pos3@" "@%pos4@" "@%pos5@" "@%pos6@" "@%pos7@" "@%pos8@" "@%pos9@" "@%pos10@" "@%pos11@" "@%pos12;
		
		// ----- End of Second Projectile Placement -----
		
		if(GameBase::getLOSInfo(%player,1500))
		{	
			%object = getObjectType($los::object);
			%targeted = GameBase::getOwnerClient($los::object);
			if(%object == "Player" || %object == "Flier")
			{
				%targetP = Client::getName(%targeted);
				Projectile::spawnProjectile("TankMissile",%trans,%player,%vel,$los::object);
				Projectile::spawnProjectile("TankMissile",%trans2,%player,%vel,$los::object);
				Client::sendMessage(%client,0,"Tank Rocket Launcher lock acquired "@ %targetP @ "~wmine_act.wav");
				Client::sendMessage(%targeted,0,"Tank Rocket Launcher lock detected - " @ %clientId @ "~wono.wav");
				if(!$FiringTRocketLauncher[%client]) 
					CheckTRocketLauncher(%client, %player);
				//GiveKickBack(%player, 125, 1);
				Player::decItemCount(%player,$WeaponAmmo[TRocketLauncher],2);
			}
			else
			{
				Projectile::spawnProjectile("TankRocket",%trans,%player,%vel,%target);
				Projectile::spawnProjectile("TankRocket",%trans2,%player,%vel,%target);
				if(!$FiringTRocketLauncher[%client]) 
					CheckTRocketLauncher(%client, %player); 
				//GiveKickBack(%player, 125, 1);
				Player::decItemCount(%player,$WeaponAmmo[TRocketLauncher],2);
			}
		}
		else
		{
			Projectile::spawnProjectile("TankRocket",%trans,%player,%vel,%target);
			Projectile::spawnProjectile("TankRocket",%trans2,%player,%vel,%target);
			if(!$FiringTRocketLauncher[%client]) 
				CheckTRocketLauncher(%client, %player); 
			//GiveKickBack(%player, 125, 1);
			Player::decItemCount(%player,$WeaponAmmo[TRocketLauncher],2);
		}
	}
	else
		Client::sendMessage(%client,0,"Tank Rocket Launcher out of ammo.~waccess_denied.wav");
}

//function TRocketLauncher::onUse()
//{
//}

function TRocketLauncher::MountExtras(%player,%weapon) 
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>This duel heat seeking launcher will lock onto your enemies."); 
}

function TRocketLauncher::MountExtras(%player,%weapon)
{		
	Player::mountItem(%player,TRocketLauncher2,$TRocketLauncherSlotA); 
	Player::mountItem(%player,TRocketLauncher3,$TRocketLauncherSlotB); 
	Player::mountItem(%player,TRocketLauncher4,$TRocketLauncherSlotC);
	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>This duel heat seeking missile launcher will lock onto <f1>Enemies <f2>and<f1> Vehicles<f2> within your crosshairs."); 
}

function CheckTRocketLauncher(%client, %player) 
{
	if(Player::isTriggered(%player,$WeaponSlot) && (Player::getMountedItem(%player,$WeaponSlot) == "TRocketLauncher")) 
	{
		Player::trigger(%player,$TRocketLauncherSlotA,true);
		Player::trigger(%player,$TRocketLauncherSlotB,true);
		Player::trigger(%player,$TRocketLauncherSlotC,true);
		schedule("CheckTRocketLauncher(" @ %client @ "," @ %player @ ");",0.1,%player); 
		$FiringTRocketLauncher[%client] = true;
	}
	else 
	{
		Player::trigger(%player,$TRocketLauncherSlotA,false); 
		Player::trigger(%player,$TRocketLauncherSlotB,false); 
		Player::trigger(%player,$TRocketLauncherSlotC,false); 
		$FiringTRocketLauncher[%client] = false; 
	}
}
