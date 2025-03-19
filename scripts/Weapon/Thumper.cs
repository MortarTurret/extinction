$InvList[Thumper] = 1;
$MobileInvList[Thumper] = 1;
$RemoteInvList[Thumper] = 1;

$InvList[ThumperAmmo] = 1;
$MobileInvList[ThumperAmmo] = 1;
$RemoteInvList[ThumperAmmo] = 1;

$AutoUse[Thumper] = false;
$SellAmmo[ThumperAmmo] = 5;
$WeaponAmmo[Thumper] = ThumperAmmo;

addWeapon(Thumper);
addAmmo(Thumper, ThumperAmmo, 2);

GrenadeData ThumperGren
{	
	bulletShapeName = "mortar.dts";
	explosionTag = grenadeExp;
	collideWithOwner = True;
	ownerGraceMS = 350;
	collisionRadius = 0.0;
	mass = 0.1;
	elasticity = 0.01;
	damageClass = 1;
	damageValue = 0.45;	//0.25
	damageType = $ShrapnelDamageType;
	explosionRadius = 07;	//15
	kickBackStrength = 315.0; //lolz
	muzzleVelocity     = 250.0; //250 goooooood
	//acceleration = 2.0;
	maxLevelFlightDist = 130;
	totalTime = 0.9; //Time before Exp
	liveTime = 0.01; //Time after collision
	projSpecialTime = 0.01; //idk
	inheritedVelocityScale = 0.75; //25
	smokeName = "plasmatrail.dts";
};

MineData ThumperGrenMine
{
	mass = 0.1;		//5.0;
	drag = 2.0;	//1,0
	density = 1.0;	//5.0;	//2.0;
		elasticity = 0.1;	//0.1;	//0.15;
		friction = 2.0;	//1.0;
	className = "Handgrenade";	//className = "Mine";
	description = "Mini Mine";
	shapeFile = "mortar";
	shadowDetailMask = 4;
	explosionId = grenadeExp;
	explosionRadius = 20.0;
	damageValue = 0.25;	//0.75;	//1.0;
	damageType = $ShrapnelDamageType;
	kickBackStrength = 250;
	triggerRadius = 2.5;	
	maxDamage = 0.005;
};

//GrenadeData ThumperTrailGren
//{
//	bulletShapeName    = "breath.dts";		//mortartrail.dts";	//mortar.dts";
//	explosionTag       = SmokeFade;	//mortarExp;
//	collideWithOwner   = True;
//	ownerGraceMS       = 350;
//	collisionRadius    = 0.3;
//	mass               = 5.0;
//	elasticity         = 0.1;
//
//	damageClass        = 1;       // 0 impact, 1, radius
//	damageValue        = 0.0;	//1.0
//	damageType         = $ShrapnelDamageType;
//
//	explosionRadius    = 1.0;
//	kickBackStrength   = 0.0;
//	maxLevelFlightDist = 275;
//	totalTime          = 0.25;
//	liveTime           = 0.25;
//	projSpecialTime    = 0.01;
//
//	inheritedVelocityScale = 0.75;
//	smokeName              = "plasmatrail.dts";
//};

ItemData ThumperAmmo 
{	
	description = "Thumper ammo"; 
	className = "Ammo"; 
	shapeFile = "grenammo"; 
	heading = $InvHead[ihAmm]; 
	shadowDetailMask = 4; 
	price = 2;
}; 

MineData ThumperAmmoBomb
{
	mass = 5.0;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Halo";
	shapeFile = "grenade";
	shadowDetailMask = 4;
	explosionId = flashExpSmall;	//mineExp;
	explosionRadius = 5.0;
	damageValue = 0.0;	//0.5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 10.5;
};
ItemImageData ThumperImage 
{	
	shapeFile = "energygun";
	mountPoint = 0;
	weaponType = 0;
	ammoType = ThumperAmmo;
	//projectileType = BomberWarhead;
	accuFire = false;
	reloadTime = 0.8; //0.8
	fireTime = 0.3; //0.5
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };
	mountOffset = { 0, -0.2, -0.1 }; //back
	mountRotation = { 0, 3.1416, 0 }; //upsidedown
	//mountRotation = { 0, 3.1416, 0 }; //upsidedown
	sfxFire = SoundPickUpWeapon;	//SoundTurretDeploy;	//SoundFireGrenade;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;
};

ItemData Thumper 
{	
	description = "Thumper";
	className = "Weapon";
	shapeFile = "energygun";
	hudIcon = "grenade";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = ThumperImage;
	price = 150;
	showWeaponBar = true;
};

function Thumper::MountExtras(%player,%weapon)
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));

	%clientId = Player::getclient(%player);
	if(%clientId.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
			Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>A close range explosive weapon which can also be used to boost oneself.");
}

function ThumperImage::onFire(%player, %slot)
{		
	if($debug)
		Anni::Echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));		

	%AmmoCount = Player::getItemCount(%player, $WeaponAmmo[Thumper]);
	
	%clientId = Player::getClient(%player);
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	
		if(%AmmoCount > 0)
		{
			Projectile::spawnProjectile("ThumperGren",%trans,%player,%vel,%player);
			Player::decItemCount(%player,$WeaponAmmo[Thumper],1);
			playSound(SoundFireGrenade, GameBase::getPosition(%clientId));
		}
		else
		{
			Client::sendMessage(Player::getClient(%player),1,"Thumper out of ammo.");
			return false;
		}
}


function ThumperImage::onFireXX(%player, %slot) 
{		
	if($debug)
		Anni::Echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));	
		
	Player::decItemCount(%player,$WeaponAmmo[Player::getMountedItem(%player,$WeaponSlot)],1);
		
	%vel = Item::getVelocity(%player);

	if(%vel == 0 || vector::normalize(%vel) != "-NAN -NAN -NAN")	
	{	
		%trans = GameBase::getMuzzleTransform(%player);
		%newObj = Projectile::spawnProjectile("ThumperGren",%trans,%player,%vel);
		schedule("ThumperGren::Arm(" @ %NewObj @ ", " @ %player @ ");", 0.1, %NewObj);	//0.3
	}
	else 	
		Anni::Echo("!! Butterfly Error, Thumper fire. vel ="@%vel);			
}


function ThumperGren::Arm(%newobj,%this)
{	
	if (GameBase::getPosition(%newObj) != "0 0 0" && !player::isdead(%this))
	{
		%Pos = vector::add(GameBase::getPosition(%newobj),"0 0 0.1"); 
   		%vel = Item::getVelocity(%newobj);
   		%Mine = newObject("","Mine","ThumperGrenMine");
 		addToSet("MissionCleanup", %Mine);
      		GameBase::throw(%Mine,%this,1,true);	
		GameBase::setPosition(%Mine, %pos);
		Item::setVelocity(%Mine, %vel);	
		%mine.owner = %this;
			
	//	schedule("Deleteobject("@%newobj@");",0.01);	
		Deleteobject(%newobj);
	}
}


function ThumperGrenMine::onAdd(%this)
{
	%this.damage = 0;
	%this.spawntime = getSimTime();
	ThumperGrenMine::deployCheck(%this);
	$mine::count++;
	if(!$dedicated)
		bottomprint(2049,"Ammo count= "@$Ammo::count@" Item count= "@$item::count@" Miner count= "@$mine::count);
}




function ThumperGrenMine::onCollision(%this,%object)
{	
	if($debug) 
		event::collision(%this,%object);

	%type = getObjectType(%object);

		schedule("GameBase::setDamageLevel("@%this@",1);",0.15, %this);
		//GameBase::setDamageLevel(%this, %data.maxDamage);
		$mine::count--;
		if(%type == "Player")
		{
			%owner = %this.owner;
			if(%owner != %object)
			{
			//		messageall(0,client::getname(Player::getclient(%object))@" set off "@client::getname(Player::getclient(%owner))@"'s mortar.");
			//	else
			//		messageall(0,client::getname(Player::getclient(%object))@" heroically swallowed a mortar.");
				Anni::Echo("+ MA Thumper "@client::getname(Player::getclient(%object)));
				%time = getsimtime();
				%object.ThumperedTime = %time;
				%object.morteredBy = Player::getclient(%owner);
			}
				
		}
	//}
}

function ThumperGrenMine::deployCheck(%this)
{
	%spawntime = %this.spawntime;
	%livetime = getSimTime() - %spawntime;
	if($debug)
		bottomprintall(%this@" Hammer time "@%livetime);
		
	if(GameBase::isAtRest(%this) || %livetime > 0.9) 
	{
		//Anni::Echo("mine (Hammer) Detonate -at rest");
		schedule("GameBase::setDamageLevel("@%this@", 2);",1.25,%this);	//0.75
		$mine::count--;
	}
	%vel = Item::getVelocity(%this);

	if(%vel == 0 || vector::normalize(%vel) != "-NAN -NAN -NAN")	
	{
		%Pos = GameBase::getPosition(%this); 
		%box = getBoxCenter(%this);
		%trans =  "0 0 1 0 0 0 0 0 1 " @ %box;
	//	Anni::Echo("Poof! pos"@%pos@" vel "@%vel);
		%obj = Projectile::spawnProjectile("ThumperTrailGren", %trans, %this, %vel);
		Projectile::spawnProjectile(%obj);
		GameBase::setPosition(%obj, %pos);
		Item::setVelocity(%obj, %vel);
	}
	else 	
		Anni::Echo("!! Butterfly Error, Hammer Trail. vel ="@%vel);	
		
	if(%this)	
		schedule("ThumperGrenMine::deployCheck(" @ %this @ ");", 0.25, %this);
}