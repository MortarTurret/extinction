$InvList[BabyNukeMortar] = 1;
$MobileInvList[BabyNukeMortar] = 1;
$RemoteInvList[BabyNukeMortar] = 1;

$InvList[BabyNukeAmmo] = 1;
$MobileInvList[BabyNukeAmmo] = 1;
$RemoteInvList[BabyNukeAmmo] = 1;


$AutoUse[BabyNukeMortar]= True;
$WeaponAmmo[BabyNukeMortar] = BabyNukeAmmo;
$SellAmmo[BabyNukeAmmo] = 25;

addWeapon(BabyNukeMortar);
addAmmo(BabyNukeMortar, BabyNukeAmmo, 1);

$BabyNukeMortarSlotA=4;

ItemData BabyNukeAmmo 
{
	description = "Nuclear Device";
	className = "Ammo";
	shapeFile = "ammo1";
	heading = $InvHead[ihAmm];
	shadowDetailMask = 4;
	price = 1;
};

MineData BabyNukeAmmoBomb
{
	mass = 5.0;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Halo";
	shapeFile = "mortar";
	shadowDetailMask = 4;
	explosionId = flashExpSmall;	//mineExp;
	explosionRadius = 5.0;
	damageValue = 0.0;	//0.5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 10.5;
};

ItemImageData BabyNukeImage
{
	shapeFile = "mortargun";
	mountPoint = 0;
	mountOffset = { 0, -0.005, 0 };
	mountRotation = { 0, 0 , 0 };
	weaponType = 0; // Single Shot
	//projectileType = BabyNukeBomb;
	accuFire = true;
	ammoType = BabyNukeAmmo;
	reloadTime = 2;
	fireTime = 3.5;
	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 1, 1, 0.2 };
	sfxFire = SoundFireMortar;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundMortarReload;
	sfxReady = SoundMortarIdle;
};

ItemData BabyNukeMortar
{
	description = "Baby Nuke Launcher";
	className = "Weapon";
	shapeFile = "mortargun";
	hudIcon = "energypack";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = BabyNukeImage;
	price = 1500;
	showWeaponBar = true;
};

ItemImageData BabyNuke2Image
{
	shapeFile = "mortargun";
	mountPoint = 0;
	mountOffset = { -0.0365, 0, -0.035 };
	mountRotation = { 0, -1.575, 0 };
	weaponType = 0; // Single Shot
	//projectileType = BabyNukeBomb;
	accuFire = true;
	ammoType = BabyNukeAmmo;
	reloadTime = 2.0;
	fireTime = 2.0;
};

ItemData BabyNukeMortar2
{
	//description = "Baby Nuke Launcher";
	className = "Weapon";
	shapeFile = "mortargun";
	hudIcon = "plasma";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = BabyNuke2Image;
	price = 0;
	showWeaponBar = true;
	showInventory = false;
};

function BabyNukeMortar::MountExtras(%player,%weapon)
{	
	Player::mountItem(%player,BabyNukeMortar2,$BabyNukeMortarSlotA);

	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>Throw this lil baby and watch them run for the hills.");	
}

function FireBabyNukeMortar(%client, %player) 
{
	if(Player::isTriggered(%player,$WeaponSlot) && (Player::getMountedItem(%player,$WeaponSlot) == "BabyNukeMortar")) 
	{
		Player::trigger(%player,$BabyNukeMortarSlotA,true);
		schedule("FireBabyNukeMortar(" @ %client @ "," @ %player @ ");",0.1,%player); 
		%player.firingBabyNuke = true;
	}
	else 
	{
		Player::trigger(%player,$BabyNukeMortarSlotA,false);
		%player.firingBabyNuke = false;
	}
}

MineData BombHalo
{
	mass = 5.0;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Halo";
	shapeFile = "force";
	shadowDetailMask = 4;
	explosionId = LargeShockwave;
	explosionRadius = 5.0;
	damageValue = 0.0;	//0.5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 0.5;
};

function BombHalo::onAdd(%this)
{
	schedule("Mine::Detonate(" @ %this @ ");",1.5,%this);	//0.2
}

function BabyNukeBomb::onAdd(%this)
{	
	//schedule("BombHalo(" @ %this @ ");",5.25);
}
function BombHalo(%this)
{
	%obj = newObject("","Mine","BombHalo");
	addToSet("MissionCleanup", %obj);
	GameBase::throw(%obj,%this,1 ,false);
	//%pos = Vector::add(GameBase::getPosition(%this), "0 0 2");
	//GameBase::setPosition(%obj, %pos);
}

function BabyNukeImage::onFire(%player) 
{
	if ( Player::isDead(%player) )
		return;
	%client = GameBase::getOwnerClient(%player);
	if ( Client::getTransportAddress(%client) == "" || %player.launchingnuke )
		return;
//	if ( vector::getdistance(Item::getVelocity(%player), "0 0 0") > 10 )
//	{
//		centerprint(%client, "<jc><f1>Your moving too fast to launch the nuke!!!");
//		return;
//	}
	%player.launchingnuke = True;
	Player::decItemCount(%player,BabyNukeAmmo,1);
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	playSound(SoundFireMortar, GameBase::getPosition(%player));
	Projectile::spawnProjectile("BabyNukeBomb",%trans,%player,%vel,%player);
	for(%i=0; %i < 6.28; %i += 1.256)
	{
		%forceVel = Vector::getFromRot("0 0 " @ %i, 15, 40);
		%bomb = Projectile::spawnProjectile("NukeExplosion4",%trans,%player,%vel,%player);
		%bomb.forceVel = %forceVel;	
	}
	%bomb = Projectile::spawnProjectile("NukeExplosion4",%trans,%player,%vel,%player);
	%bomb.forceVel = "0 0 50";		
	%bomb = Projectile::spawnProjectile("NukeExplosion3",%trans,%player,%vel,%player);
	%bomb.forceVel = "0 0 40";		
	%bomb = Projectile::spawnProjectile("NukeExplosion2",%trans,%player,%vel,%player);
	%bomb.forceVel = "0 0 35";		
	%bomb = Projectile::spawnProjectile("NukeExplosion2",%trans,%player,%vel,%player);
	%bomb.forceVel = "0 0 30";		
	%bomb = Projectile::spawnProjectile("NukeExplosion1",%trans,%player,%vel,%player);
	%bomb.forceVel = "0 0 25";			
	%bomb = Projectile::spawnProjectile("NukeExplosion1",%trans,%player,%vel,%player);
	%bomb.forceVel = "0 0 20";			
	%bomb = Projectile::spawnProjectile("NukeExplosion1",%trans,%player,%vel,%player);
	%bomb.forceVel = "0 0 15";			
	%bomb = Projectile::spawnProjectile("NukeExplosion0",%trans,%player,%vel,%player);
	%bomb.forceVel = "0 0 10";			
	%bomb = Projectile::spawnProjectile("NukeExplosion0",%trans,%player,%vel,%player);
	%bomb.forceVel = "0 0 5";				
			
	if(!%player.firingBabyNuke)
		FireBabyNukeMortar(%client, %player);
	%player.launchingnuke = False;
}

function NukeExplosion0::onAdd(%this)
{
	schedule("NukeSpread(" @ %this @ ");",5.25,%this);
}
function NukeExplosion1::onAdd(%this)
{
	schedule("NukeSpread(" @ %this @ ");",5.25,%this);
}
function NukeExplosion2::onAdd(%this)
{
	schedule("NukeSpread(" @ %this @ ");",5.25,%this);
}
function NukeExplosion3::onAdd(%this)
{
	schedule("NukeSpread(" @ %this @ ");",5.25,%this);
	schedule("BombHalo(" @ %this @ ");",5.45,%this);
}
function NukeExplosion4::onAdd(%this)
{
	schedule("NukeSpread(" @ %this @ ");",5.25,%this);
}
function NukeSpread(%this)
{
	%forceVel = %this.forceVel;
	//%pos = Vector::add(GameBase::getPosition(%this), %padd);
	//GameBase::setPosition(%this, Vector::add(GameBase::getPosition(%this), "0 0 0.5"));	
	Item::setVelocity(%this, %forceVel);
	
}


SoundData NukeWind
{
	wavFileName = "wind1.wav";
	profile = Profile3dLudicrouslyFar;
};
ExplosionData NukeCrownExp
{
   shapeName = "mortarex.dts";//fiery
   soundId   = NukeWind;


	faceCamera=true;				
	randomSpin = true;		
	hasLight=true;			
	lightRange=9.0;			
	timeZero=0.300;			
	timeOne=0.900;			
	colors[0]={0.5,0.4,0.2};
	colors[1]={1.0,1.0,0.5};
	colors[2]={0.0,1.0,0.0};
	radFactors={0.5,1.0,0.0};
	shiftPosition=False;


};
SoundData NukeSound
{
	wavFileName = "turretexp.wav";
	profile = Profile3dLudicrouslyFar;
};
ExplosionData NukeExp
{
   shapeName = "mortarex.dts";//fiery
   soundId   = NukeSound;


	faceCamera=true;				
	randomSpin = true;		
	hasLight=true;			
	lightRange=9.0;			
	timeZero=0.300;			
	timeOne=0.900;			
	colors[0]={0.5,0.4,0.2};
	colors[1]={1.0,1.0,0.5};
	colors[2]={0.0,1.0,0.0};
	radFactors={0.5,1.0,0.0};
	shiftPosition=False;


};


GrenadeData NukeExplosion0
{	bulletShapeName = "breath.dts";
	explosionTag = NukeExp;	//WickedBadExp;	//LargeShockwave;
	collideWithOwner = false;
	ownerGraceMS = 500;
	collisionRadius = 0.3;
	mass = 5.0;
	elasticity = 0.2;	//0.4;
	damageClass = 1; // 0 impact, 1, radius
	damageValue = 0.0;	//1
	damageType = $ShrapnelDamageType;
	explosionRadius = 40.0;
	kickBackStrength = 100.0;
	maxLevelFlightDist = 350;
	totalTime = 5.85;
	liveTime = 5.85;
	projSpecialTime = 10.01;
	inheritedVelocityScale = 0.5;
	//smokeName = "paint.dts";
	   smokeName              = "breath.dts";	//plastrail.dts";//mortartrail
};
GrenadeData NukeExplosion1
{	bulletShapeName = "breath.dts";
	explosionTag = WickedBadExp;	//LargeShockwave;
	collideWithOwner = false;
	ownerGraceMS = 500;
	collisionRadius = 0.3;
	mass = 5.0;
	elasticity = 0.2;	//0.4;
	damageClass = 1; // 0 impact, 1, radius
	damageValue = 1.0;
	damageType = $ShrapnelDamageType;
	explosionRadius = 10.0; //BR Setting
	kickBackStrength = 100.0;
	maxLevelFlightDist = 350;
	totalTime = 6.0;
	liveTime = 6.0;
	projSpecialTime = 10.01;
	inheritedVelocityScale = 0.5;
	//smokeName = "paint.dts";
	   smokeName              = "breath.dts";	//plastrail.dts";//mortartrail
};
GrenadeData NukeExplosion2
{	bulletShapeName = "breath.dts";
	explosionTag = 	NukeCrownExp;	//WickedBadExp;	//LargeShockwave;
	collideWithOwner = false;
	ownerGraceMS = 500;
	collisionRadius = 0.3;
	mass = 5.0;
	elasticity = 0.2;	//0.4;
	damageClass = 1; // 0 impact, 1, radius
	damageValue = 0.45;	//1 //BR Setting
	damageType = $ShrapnelDamageType;
	explosionRadius = 30.0;	//40.0; //BR Setting
	kickBackStrength = 250;	//100.0;
	maxLevelFlightDist = 350;
	totalTime = 6.15;
	liveTime = 6.15;
	projSpecialTime = 10.01;
	inheritedVelocityScale = 0.5;
	//smokeName = "paint.dts";
	   smokeName              = "breath.dts";	//plastrail.dts";//mortartrail
};
GrenadeData NukeExplosion3
{	bulletShapeName = "breath.dts";
	explosionTag = NukeCrownExp;	//WickedBadExp;	//LargeShockwave;
	collideWithOwner = false;
	ownerGraceMS = 500;
	collisionRadius = 0.3;
	mass = 5.0;
	elasticity = 0.2;	//0.4;
	damageClass = 1; // 0 impact, 1, radius
	damageValue = 0.5;	//1
	damageType = $ShrapnelDamageType;
	explosionRadius = 60.0; //BR Setting
	kickBackStrength = 100.0;
	maxLevelFlightDist = 350;
	totalTime = 6.3;
	liveTime = 6.3;
	projSpecialTime = 10.01;
	inheritedVelocityScale = 0.5;
	//smokeName = "paint.dts";
	   smokeName              = "breath.dts";	//plastrail.dts";//mortartrail
};
GrenadeData NukeExplosion4
{	bulletShapeName = "breath.dts";
	explosionTag = NukeCrownExp;	//LargeShockwave;
	collideWithOwner = false;
	ownerGraceMS = 500;
	collisionRadius = 0.3;
	mass = 5.0;
	elasticity = 0.2;	//0.4;
	damageClass = 1; // 0 impact, 1, radius
	damageValue = 0.0;
	damageType = $ShrapnelDamageType;
	explosionRadius = 20.0; //BR Setting
	kickBackStrength = 100.0;
	maxLevelFlightDist = 350;
	totalTime = 6.45;
	liveTime = 6.45;
	projSpecialTime = 10.0;
	inheritedVelocityScale = 0.5;
	//smokeName = "paint.dts";
	   smokeName   = "breath.dts";	//plastrail.dts";//mortartrail
};
