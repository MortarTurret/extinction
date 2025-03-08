$InvList[ParticleBeamWeapon] = 1;
$MobileInvList[ParticleBeamWeapon] = 1;
$RemoteInvList[ParticleBeamWeapon] = 1;

$AutoUse[ParticleBeamWeapon] = false;

addWeapon(ParticleBeamWeapon);

LaserData ParticleBeam
{	laserBitmapName = "warp.bmp"; 
	hitName = "shockwave_large.dts";
	damageConversion = 0.025;	//0.064;	
	//DamageType = $BulletDamageType;
	DamageType = $LaserDamageType;	
	beamTime = 1.0; 
	lightRange = 2.0;
	lightColor = { 1.0, 0.25, 0.25 };
	detachFromShooter = false;
	hitSoundId = SoundFlierCrash;
};

LaserData MarkerLaser 
{	
	laserBitmapName = "shotgunbolt.bmp"; //shotgunbolt so beam isnt same color as most skies, also looks cooler -ghost
//	hitName = "enbolt.dts";	//laserhit.dts";
	damageConversion = 0.0;
	DamageType = $LaserDamageType;	
	beamTime = 0.5; 
	lightRange = 2.0;
	lightColor = { 1.0, 1.0, 1.0 };
	detachFromShooter = true;	//false;
//	hitSoundId = SoundLaserHit;
};

LaserData PhrailterLaser
{
	laserBitmapName   = "shotgunbolt.bmp"; //shotgunbolt so beam isnt same color as most skies, also looks cooler -ghost
	hitName   	  = "shockwave_large.dts";	//"laserhit.dts";

	damageConversion  = 0.00;		//0.007
	baseDamageType    = $LaserDamageType;

	beamTime          = 2.5;	

	lightRange = 2.0;
	lightColor = { 1.0, 1.0, 1.0 };

	detachFromShooter = true;
	hitSoundId	= SoundFlierCrash;
	//   hitSoundId        = SoundParticleBeamExplosion;	//SoundLaserHit;
};
ItemImageData ParticleBeamWeaponImage 
{
	shapeFile = "paintgun";
	mountPoint = 0;
	weaponType = 1;
//	projectileType = markerLaser;
	accuFire = true;
	minEnergy = 1;
	maxEnergy = 1;
	fireTime = 0.001;
	spinUpTime = 0.001;
	spinDownTime = 0.001;
	reloadTime = 2.0;
	lightType = 3;
	lightRadius = 1;
	lightTime = 1;
	lightColor = { 0.25, 1, 0.25 };
	//sfxFire = SoundFireTargetingLaser;
	//sfxActivate = SoundPickUpWeapon;
};

ItemData ParticleBeamWeapon
{
	description = "Particle Beam";
	className = "Weapon";
	shapeFile = "paintgun";
	shadowDetailMask = 4;
	heading = $InvHead[ihWea];
	imageType = ParticleBeamWeaponImage;
	showWeaponBar = true;
	showInventory = true;
	price = 2000;
};

ItemImageData ParticleBeamWeaponImage2
{
	shapeFile = "sniper";
	mountPoint = 0;
	weaponType = 0;
	minEnergy = -60; 
	maxEnergy = -6; 
	projectileType = PhrailterLaser; 
	mountRotation = { 0 , -2.5 , 0 };
	mountOffset = { -0.1 , 0 , 0 };
	accuFire = true;
	reloadTime = 0.0;	//1.5;
	fireTime = 0.0;	//2.5;	//4/4/2007 5:35AM
};

ItemData ParticleBeamWeapon2
{
	className = "Weapon";
	shapeFile = "sniper";
	shadowDetailMask = 4;
	imageType = ParticleBeamWeaponImage2;
	showWeaponBar = true; 
	showInventory = false;
};

ItemImageData ParticleBeamWeaponImage3
{
	shapeFile = "sniper";
	mountPoint = 0;
	weaponType = 0;
	minEnergy = -60;	
	maxEnergy = -6;	
	projectileType = PhrailterLaser; 
	mountRotation = { 0 , 2.5 , 0 };
	mountOffset = { 0.1 , 0 , 0 };
	accuFire = true;
	reloadTime = 0.0;	//1.5;
	fireTime = 0.0;	//2.5;	// 4/4/2007 5:36AM
};

ItemData ParticleBeamWeapon3
{
	className = "Weapon";
	shapeFile = "sniper";
	shadowDetailMask = 4;
	imageType = ParticleBeamWeaponImage3;
	showWeaponBar = true; 
	showInventory = false;
};

ItemImageData ParticleBeamWeaponImage4
{
	shapeFile = "jetpack";
	mountPoint = 0;
	mountRotation = { -1.57, 0 , 3.14 };
	mountOffset = { 0 , 0.1 , 0 };
	weaponType = 0;
	minEnergy = 70;
	maxEnergy = 120;
	projectileType = ParticleBeam;
	accuFire = true;
	fireTime = 0.25;
	reloadTime = 0.38;
	sfxFire = SoundParticleBeamFire;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundParticleBeamRecharge;
};

ItemData ParticleBeamWeapon4
{
	shapeFile = "jetpack";
	hudIcon = "reticle";
	className = "Weapon";
	shadowDetailMask = 4;
	imageType = ParticleBeamWeaponImage4;
	showWeaponBar = false;
	showInventory = false;
};

ItemImageData ParticleBeamWeaponImage5
{
//	shapeFile = "grenammo";
//	mountPoint = 0;
	shapeFile = "jetpack";
	mountPoint = 0;
	mountRotation = { -1.57, 0 , 3.14 };
	mountOffset = { 0 , 0.1 , 0 };

	weaponType = 0; 
	minEnergy = -100;	//70
	maxEnergy = 120;
	projectileType = ParticleBeam;
	accuFire = true;
	reloadTime = 1.0;	//2.5
	fireTime = 1.0;	//2.5
	sfxFire = SoundParticleBeamFire; 
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundParticleBeamRecharge;
};

ItemData ParticleBeamWeapon5
{
	className = "Weapon";
//	shapeFile = "grenammo";
	shapeFile = "jetpack";
	hudIcon = "reticle";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = ParticleBeamWeaponImage5;
	price = 2500;
	showWeaponBar = true;
};


function ParticleBeamWeapon::MountExtras(%player,%weapon)
{			
	Player::mountItem(%player,ParticleBeamWeapon2,4);
	Player::mountItem(%player,ParticleBeamWeapon3,6);
	
	%client = Player::getclient(%player);
//	if(%client.pbeam == 1)
//	{
//		if(player::getitemcount(%client,EnergyPack) == 1)
//			Player::mountItem(%player,ParticleBeamWeapon4,7);	
//	}
//	else	
		Player::mountItem(%player,ParticleBeamWeapon5,7);	
			
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	{
		if(%client.pbeam == 1)
		{
			Bottomprint(%client, "<jc>Particle Beam Weapon: <f2>Enhanced Positron Beam. Hold fire to charge.\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.");
		}
		else
		{
			Bottomprint(%client, "<jc>Particle Beam Weapon: <f2>Standard Neutron Beam.\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.");
		}
	}	
}



function ParticleBeamWeaponImage::onFire(%player) 
{	
	// max damage new 'charge up beam' is 12.5
	// max player damage = 4;
	//
	// max damage old single shot style is 3
	// player damage = 3, over 2 times highest maxdamage for player
	
	%client = Player::getclient(%player);	
	if($debug)
		echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ %client);
		
	//if(%client.inDuel && %client.DuelFirstShot != true)
	//{
	//	Bottomprint(%client, "<jc>You must fire the Rocket once before using another weapon.");
	//	return;
	//}

	if(%client.pbeam == 1)
	{	
		if(%player.pbeamcharging)
			return;
		%player.pbeamcharge=0;
		ParticleBeamWeapon::onCharge(%player);
	}
	else if(%player.reloading != true) 
	{
		// fire standard beam. 
		if(!%client.pbeamReseting && GameBase::getEnergy(%Player) > 70)//70
		{
			%smack = GameBase::getEnergy(%Player)/20;	//8;
			Player::trigger(%player, 7, true);	
			schedule("Player::trigger(" @ %player @ ", 7, false);",0.1);	
			%client.pbeamReseting = true;
			schedule(%client@".pbeamReseting = false;",2.5);
			//schedule("GameBase::playSound("@%player@", forcefieldopen, 0);",2.5);
			
			%trans = GameBase::getMuzzleTransform(%player);
			
			%rot=GameBase::getRotation(%player);
				%len = 30;
				%tr= getWord(%trans,5);
				%tr = -%tr;
				%tr = %tr+0.15;
				%up = %tr;
				%out = %tr -1;
			%vec = Vector::getFromRot(%rot,%len*%out*%smack,%len*%up*%smack);
			Player::applyImpulse(%player,%vec);			
		}
		else 
		{
			// do nothing, recharging standard beam.
		}
		
	}
}


function reload(%player) 
{
	%player.reloading = "";
	//echo("ready");
//	playSound(SoundMortarReload, GameBase::getPosition(%player));
//	GameBase::playSound(%player, MortarReload, 0);
	GameBase::playSound(%player, forcefieldopen, 0);
}
function ResetPbeamLock(%ClientId)
{
	%clientId.locksound = "";
}

function ParticleBeamWeapon::onCharge(%player)
{
	if(%player.reloading) 
		return;
	%clientId = GameBase::getOwnerClient(%player);
	
	if(Player::getMountedItem(%player, 4) == -1 || Player::getMountedItem(%player, 6) == -1) 
	{
		GameBase::setEnergy(%player, GameBase::getEnergy(%player) + (Player::getArmor(%player).maxEnergy * (%player.pbeamcharge / 100)));
		Player::trigger(%player, $WeaponSlot, false);
		%player.pbeamcharging = false;
		%player.pbeamcharge = 0;
		return;
	}
	
	if(Player::isTriggered(%player,0))
	{
	
		%player.pbeamcharging = true;
		if (%player.pbeamcharge < 100)
			%player.pbeamcharge++;
		else
		 	%player.pbeamcharge = 100;
	 	
		schedule("ParticleBeamWeapon::onCharge(" @ %player @ ");",0.05);
		GameBase::setEnergy(%player,GameBase::getEnergy(%player)-2.5);
		
		if (%player.pbeamcharge >= 60 && GameBase::getLOSInfo(%player,3000))
		{
			if(!%clientId.locksound) 
			{
				%obj = getObjectType($los::object);
			//echo(%obj);
				if(%obj == "Player" || %obj == "Flier")
				{
					Client::sendMessage(%clientId,0,"~wmine_act.wav");
					%clientId.locksound = true;
					schedule("ResetPbeamLock("@%ClientId@");",1);
				}
				else if(%obj != "SimTerrain" && %obj != "InteriorShape")
				{
					Client::sendMessage(%clientId,0,"~wusepack.wav");
					%ClientId.locksound = true;
					schedule("ResetPbeamLock("@%ClientId@");",1);
				}	
			}
			%trans = GameBase::getMuzzleTransform(%player);	
			%d1= getWord(%trans,3);
			%d2= getWord(%trans,4);
			%d3= getWord(%trans,5);		
			
			%posX = getWord(%trans,9);		//x
			%posY = getWord(%trans,10);		//y
			%posZ = getWord(%trans,11); 		//z	
			%GunTipPos = %posX@" "@%posY@" "@%posZ;	
			
			%gunVec = vector::normalize(vector::sub($los::position,%GunTipPos));				
			
			%vec = vector::sub($los::position,%gunVec);
			%trans =  getWord(%trans,0)@" "@getWord(%trans,1)@" "@getWord(%trans,2)@" "@%gunVec@" "@getWord(%trans,6)@" "@getWord(%trans,7)@" "@getWord(%trans,8)@" "@%vec;
			%proj = Projectile::spawnProjectile("MarkerLaser", %trans, 2048, %vel);					
		}
		
		bottomprint(%clientId, "<jc><f2>ParticleBeam charged to <f1>"@%player.pbeamcharge@"<f2>%", 0.5);
	}
	
	else if(!Player::isTriggered(%player,0))	//fire this bad boy. 
	{
		if(GameBase::getLOSInfo(%player,3000) && !Player::isdead(%player)) 
		{
			// there must be something in our sight. 
			// GetLOSInfo sets the following globals:
			// 	los::position
			// 	los::normal
			// 	los::object
			%target = $los::object;	
			%obj = getObjectType(%target);
			
			if(%obj == "SimTerrain" || %obj == "InteriorShape") 
			{
				// getlosinfo doesn't pick up mine types. 
				%set = newObject("set",SimSet);
				if(containerBoxFillSet(%set,$MineObjectType,$los::position,0,0,0,0) >0 ) 
					GameBase::setDamageLevel(Group::getObject(%set, 0),(%player.pbeamcharge/8));
				
				deleteObject(%set);	
			}
			else
			{
				if (%obj=="Player" || %obj =="Flier") 
					%value = %player.pbeamcharge/25;
				if(GameBase::getMapName(%target) == Bunker)
					%value = %player.pbeamcharge/25;	
				else  
					%value = %player.pbeamcharge/8;
					
				GameBase::applyDamage(%target, $SniperDamageType, %value, $los::position, "0 0 0","0 0 0", %clientId);
				
				if($debug)
					Client::sendMessage(%clientId,0,"target "@ %target @" object type "@%obj@" Damage value "@%value);
				
			}
			
			
			
		}

		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);
		playSound(SoundFireLaser, GameBase::getPosition(%player));
		Player::trigger(%player, 4, true);
		Player::trigger(%player, 6, true);
	
		%smack = %player.pbeamcharge / 10;
		if (%smack < 4) 
			playSound(SoundMissileTurretFire, GameBase::getPosition(%player));
		if (%smack >= 4) 
			playSound(SoundPlasmaTurretFire, GameBase::getPosition(%player));
		if (%smack >= 8) 
			playSound(explo3, GameBase::getPosition(%player));
	
		if($debug)
			Client::sendMessage(%clientId,0,GameBase::getMapName(%target)@"target "@ %target @" object type "@%obj@" Damage value "@%value);

		%player.pbeamcharge = 0;
		%player.pbeamHoldcharge = 0;
		%player.pbeamcharging = false;
		Player::trigger(%player, 4, false);
		Player::trigger(%player, 6, false);

		%player.reloading = true;
		schedule("reload(" @ %player @ ");",2.0); 


		if(!Player::isDead(%player)) 
		{
			%rot=GameBase::getRotation(%player);
			%len = 30;
			%tr= getWord(%trans,5);
			%tr = -%tr;
			%tr = %tr+0.15;
			%up = %tr;
			%out = %tr -1;
			
			%vec = Vector::getFromRot(%rot,%len*%out*%smack,%len*%up*%smack);
			Player::applyImpulse(%player,%vec);
		}
	}
}