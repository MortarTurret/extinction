$InvList[OSLauncher] = 1;
$MobileInvList[OSLauncher] = 1;
$RemoteInvList[OSLauncher] = 1;

$InvList[OSAmmo] = 1;
$MobileInvList[OSAmmo] = 1;
$RemoteInvList[OSAmmo] = 1;

$AutoUse[OSLauncher] = false;
$SellAmmo[OSAmmo] = 3;
$WeaponAmmo[OSLauncher] = OSAmmo;

addWeapon(OSLauncher);
addAmmo(OSLauncher, OSAmmo, 1);

ItemData OSAmmo 
{
	description = "OS Rockets";
	className = "Ammo";
	heading = $InvHead[ihAmm];
	shapeFile = "rocket";
	shadowDetailMask = 4;
	price = 50;
};

MineData OSAmmoBomb
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

ItemImageData OSLauncherImage
{
	shapeFile = "grenadel";
	ammoType = OSAmmo;
	mountPoint = 0;
	mountRotation = { 0, 3.1416, 0 };
	weaponType = 0; // Single Shot
	minEnergy = 32;
	maxEnergy = 32;
	accuFire = true;
	reloadTime = 0.5;
	fireTime = 2.5;
	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };
	sfxFire = SoundFireFlierRocket;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;
};

ItemData OSLauncher 
{
	description = "OS Laucher";
	className = "Weapon";
	shapeFile = "grenadel";
	hudIcon = "fear";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = OSLauncherImage;
	price = 5000;
	showWeaponBar = true;
};

function OSLauncher::MountExtras(%player,%weapon)
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>Fires a remotely controlled missile capable of destroying small planes.\n<jc><f2>Press <f1>SPACE<f2> or make contact to detonate.");
}





GrenadeData OSDummy 
{	bulletShapeName = "rocket.dts";
	explosionTag = debrisExpSmall;
	collideWithOwner = True;
	ownerGraceMS = 250;
	collisionRadius = 0.2;
	mass = 0.5;
	elasticity = 0.45;
	damageClass = 1; // 0 impact, 1, radius
	damageValue = 0.2;
	damageType = $MineDamageType;
	explosionRadius = 10;
	kickBackStrength = 0.0;
	maxLevelFlightDist = 150;	//150
	totalTime = 0.75; // 40.0
	liveTime = 0.15;	//1.0// grenade time live after contact
	projSpecialTime = 0.05;
	inheritedVelocityScale = 0.5;
	smokeName = "smoke.dts";
};
RocketData OSDummy2
{	
	bulletShapeName = "rocket.dts";
	explosionTag = rocketExp;
	collisionRadius = 0.0;
	mass = 2.0;
	damageClass = 1;
	damageValue = 0.5;
	damageType = $ExplosionDamageType;
	explosionRadius = 20.5;
	kickBackStrength = 135.0;	
	muzzleVelocity = 40.0;		//75
	terminalVelocity = 160.0;
	acceleration = 20.0;
	totalTime = 0.5;
	liveTime = 0.0;
	lightRange = 5.0;
	lightColor = { 1.0, 0.7, 0.5 };
	inheritedVelocityScale = 0.5;
	trailType = 2;
	trailString = "rsmoke.dts";
	smokeDist = 1.8;
	soundId = SoundJetHeavy;
};
function OSLauncherImage::onFire(%this)
{
	if($debug)
		echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %this @" cl# "@ Player::getclient(%this));	

	%client = Player::getClient(%this);
	%trans = GameBase::getMuzzleTransform(%this);
	%vel = Item::getVelocity(%player);
	if(!%this.driver)
	{
		if(!$debug)
			Annihilation::decItemCount(%client,$WeaponAmmo[OSLauncher],1);
		%proj = Projectile::spawnProjectile("OSDummy2",%trans,%this,%vel);
		schedule("OSLauncher::takecontrol("@%proj@","@%this@");",0.25);
		
	}	
}

function OSLauncher::takecontrol(%this,%owner)
{
//	Player::trigger(%owner,$WeaponSlot,false);
//	echo("OSLauncher::takecontrol "@%this@", "@%owner);
	%Pos = GameBase::getPosition(%this);
	%PlPos = GameBase::getPosition(%owner);
	
	//simplified this 12/28/2005 8:32AM
	%vel = Item::getVelocity(%this);
	%rot = vector::add("1.57 0 0",vector::getrotation(%vel));
	
	
	//echo("dist "@%dist@" Position player "@GameBase::getPosition(%owner));
	
	if(%pos && %pos != "0 0 0" && !Player::isDead(%owner))
	{
		%dist = vector::getdistance(%pos,%PlPos);
		if(%dist > 0)	//7.5)	//we want to make sure the rocket is far enough away. -not behind us. 
		{
			deleteobject(%this);
			Player::trigger(%owner,$WeaponSlot,false);
			
			%obj = newObject("Optical Seeking Missile",flier,OSMissile,true);	//flier
			GameBase::setPosition(%obj,%pos);
			GameBase::setRotation(%obj,%rot);
					
			%obj.cloakable = true;
			addToSet("MissionCleanup/deployed/object",%obj);
			Gamebase::setMapName(%obj,"Optical Seeking Missile");
			GameBase::setTeam(%obj,GameBase::getTeam(%owner));	//this
			
			GameBase::startFadeIn(%obj);
			OSMissile::TerrainCheck(%obj);
			%obj.OpOwner = %owner; 	
			
			%client = Player::getClient(%owner);			//new 2.2
			%player = Client::getOwnedObject(%client);
			
			Client::setOwnedObject(%client, %obj);
			Client::setOwnedObject(%client, %player);
					
			Client::setControlObject(%client,%obj);
			
			%owner.vehicle = %obj;
			%owner.driver = 1;
			%owner.lastWeapon = %weapon;
			//Player::unMountItem(%owner,$WeaponSlot);			
		}	
	}
}


function testguntip(%this)
{
	%client = Player::getClient(%this);
//	%weapon = Player::getMountedItem(%client,$WeaponSlot);
	%trans = GameBase::getMuzzleTransform(%client);
//	%vel = Item::getVelocity(%client);		
		
		%rot = GameBase::getRotation(%client);
							//position of tip
		%posX = getWord(%trans,9);		//x
		%posY = getWord(%trans,10);		//y
		%posZ = getWord(%trans,11); 		//z	

		%dist = 5;
		%rot=GameBase::getRotation(%player);
	//	%len = 30;
		%d1= getWord(%trans,3);
		%d2= getWord(%trans,4);
		%d3= getWord(%trans,5);		//3,4,5 are dir vec?
		
		%position = %posX + %d1 * %dist@" "@%posY + %d2 * %dist@" "@%posZ + %d3 * %dist;
	//	if(%d3 <=0 )%d3 -=%d3;
	//	%up = %d3+0.15;
	//	%out = 1-%d3;
	//	%vec = Vector::getFromRot(%rot,%len*%out*%smack,%len*%up*%smack);		
		
	%obj = newObject("","Mine","Handgrenade");
	addToSet("MissionCleanup", %obj);
//	%client = Player::getClient(%this);
	GameBase::throw(%obj,%this,0,false);
	%player.throwTime = getSimTime() + 0.5;
	GameBase::setTeam (%obj,GameBase::getTeam (%client));
	GameBase::setPosition(%obj,%position);
}


//VehicleData	//CarData	//TankData
FlierData OSMissile
{
	explosionId = WickedBadExp;	
	className = "Vehicle";
	shapeFile = "rocket";
	shieldShapeName = "shield_medium";
	mass = 7.5; //0.1
	drag = 1.0;
	density = 1.2;
	maxBank = 8;
	maxPitch = 10;
	maxSpeed = 60; //80
	minSpeed = 40;
	lift = 0; //.85
	maxAlt = 20000;
	maxVertical = 10; //200
	maxDamage = 0.001;
	destroyDamage = 0.001;
	damageLevel = {0.001, 0.001};
	maxEnergy = 15; // time in seconds until explodes
	accel = 60; //1.0
	groundDamageScale = 20.0;
	repairRate = 0;
	damageSound = shockExplosion;
	ramDamage = 0.5;
	ramDamageType = $ImpactDamageType;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	visibleToSensor = true;
	shadowDetailMask = 2;
	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundJetHeavy;
	moveSound = SoundJetHeavy;
	visibleDriver = false;
	driverPose = 22;
	description = "Optical Seeking Missile";

};

function OSMissile::onAdd(%this)
{	
	GameBase::setRechargeRate (%this,0);
	schedule("OSMissile::exhaustFuel("@%this@");",0.1,%this);
}


function KillRocket(%this)
{
	%energy = GameBase::getEnergy(%this);
	%Pos = GameBase::getPosition(%this); 
   	%vel = Item::getVelocity(%this);
	if(vector::normalize(%vel) != "-NAN -NAN -NAN")	
	{		
		%trans =  "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%this);
	 	if(!player::isdead(%this.OpOwner))
	 	{

	 		%player = %this.opowner;
	 		%client = Player::getclient(%player);
	 		%radius = 25;
	 		%fuel = %energy / 15;	// percent left. 
	 		%damageValue = 0.45;	 //changed to weaken
	 		%force = %fuel * 250;
	 		GameBase::applyRadiusDamage($OSMissileDamageType,%pos, %radius,	%damageValue,%force,%client);
	 	}
		else 	
			return;
	}
	else
		echo("!! Butterfly Error, Killrocket. vel ="@%vel);				
}

function OSMissile::onCollision(%this,%object)
{	
//	if($debug) 
//		event::collision(%this,%object);

	GameBase::setDamageLevel(%this,2); 
	
	//GameBase::applyDamage(%this,$MissileDamageType,10,GameBase::getPosition(%this),"0 0 0","0 0 0",%this);
}


function OSMissile::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	if($debug::Damage)
	{
		echo("OSMissile::onDamage("@%this@", "@%type@", "@%value@", "@%pos@", "@%vec@", "@%mom@", "@%object@" )");
	}	
	if(GameBase::getDamageState(%this) == Destroyed) 
		return;

	if(%Value == 0)
	{
		GameBase::setDamageLevel(%this,2);
		echo("!! No Damage OS!!ZOMG!!~!@~!@, ERROR! cl# "@%this.clLastMount);
		return;	
	}
		
	%damageLevel = GameBase::getDamageLevel(%this);	
	%dValue = %damageLevel + %Value;
	GameBase::setDamageLevel(%this,%dValue);
}


function OSMissile::jump(%this,%mom)
{	
	GameBase::setDamageLevel(%this,2);	
	//GameBase::applyDamage(%this,$ImpactDamageType,10,GameBase::getPosition(%this),"0 0 0",%mom,%this);
}

function OSMissile::onDestroyed (%this,%mom)
{
	%client = GameBase::getControlClient(%this);
	%player = %this.OpOwner;	

		
	%this.cloakable = "";
	%this.nuetron = "";
	
	KillRocket(%this);
		
	%this.oprocketowner = %client; 
	if(%player != -1) 
	{
		if(!player::isdead(%player))
	 	{
			Client::setControlObject(%client, %player);
			if(%player.lastWeapon != "") 
			{
				Player::useItem(%player,%player.lastWeapon);		 	
				%player.lastWeapon = "";
			}
		}
		else
		{
			//He's dead Jim
			
			
		}
		%player.driver = "";
		%player.vehicle= "";
	}
	//else messageall(1,"oopsie");
	
}

function OSMissile::exhaustFuel(%this)
{
	//centerprintall("<jc>"@GameBase::getRotation(%this),200);
	if(GameBase::getDamageState(%this) == Destroyed) 
		return;
		
	%energy = GameBase::getEnergy(%this);
	if(%energy < 1) 
		GameBase::setDamageLevel(%this,2);	
	else if(GameBase::getLOSInfo(%this,5))
	{ 
		//echo("LOS Kill Rocket");
		GameBase::setDamageLevel(%this,2);	
	}		
	else 
	{
		
		%fuel = (%energy/15)*100;
				
		if(%fuel/5 -floor(%fuel/5) <0.1 || %fuel/10 -floor(%fuel/10) <0.1)
		{
			
		//	messageall(1,%fuel);
		 	%player = %this.opowner;
		 	%client = Player::getclient(%player);
			%warhead = floor(%fuel);
			bottomprint(%client, "<jc>OS Warhead <f2>"@ 100 - %warhead @"% depleted.\n<jc><f2>Press <f1>SPACE<f2> or make contact to detonate!",5);
		}
		
		GameBase::setEnergy(%this,%energy - 0.1);
		schedule("OSMissile::exhaustFuel("@%this@");",0.1,%this);
	}
}

function OSMissile::getHeatFactor(%this) 
{
	return 1.0;
}



function OSMissile::TerrainCheck(%object)
{
	%pos = getBoxCenter(%object);
	%object.Lpos = %pos;
	schedule("OSMissile::Checkpos(" @ %object @ ");",0.1);				
}

function OSMissile::Checkpos(%object)
{	
	%pos = getBoxCenter(%object);
	%pos3 = getWord(%pos,2);
	%Lpos = %object.Lpos;
	%Lpos3 = getWord(%Lpos,2);
	%object.Lpos = "";
	%up = getword(GameBase::getRotation(%object),0)*3;
	if(%up<0)
		%up = 0;	
	if(%pos3 -3 -%up > %Lpos3)
	{
		GameBase::setPosition(%object,%Lpos);
		Item::setVelocity(%object, 0);
		GameBase::setDamageLevel(%object,2);		
	}		
}