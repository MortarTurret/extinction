


$InvList[Hammer] = 1;
$MobileInvList[Hammer] = 1;
$RemoteInvList[Hammer] = 1;

$InvList[HammerAmmo] = 1;
$MobileInvList[HammerAmmo] = 1;
$RemoteInvList[HammerAmmo] = 1;

$AutoUse[Hammer] = false;
$SellAmmo[HammerAmmo] = 5;
$WeaponAmmo[Hammer] = HammerAmmo;

addWeapon(Hammer);
addAmmo(Hammer, HammerAmmo, 2);

$oundReload[HammerImage] = SoundPodReload;

//======= projectile


GrenadeData HammerShell
{
   bulletShapeName    = "mortar.dts";
   explosionTag       = mortarExp;
   collideWithOwner   = True;
   ownerGraceMS       = 250;
   collisionRadius    = 0.3;
   mass               = 5.0;
   elasticity         = 0.1;

   damageClass        = 1;       // 0 impact, 1, radius
   damageValue        = 0.75;	//1.0;
   damageType         = $ShockDamageType;

   explosionRadius    = 20.0;
   kickBackStrength   = 250.0;
   maxLevelFlightDist = 275;
   totalTime          = 30.0;
   liveTime           = 2.0;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 0.5;
   smokeName              = "mortartrail.dts";
};

GrenadeData HammerTrailGren
{
	bulletShapeName    = "breath.dts";		//mortartrail.dts";	//mortar.dts";
	explosionTag       = SmokeFade;	//mortarExp;
	collideWithOwner   = True;
	ownerGraceMS       = 250;
	collisionRadius    = 0.3;
	mass               = 5.0;
	elasticity         = 0.1;

	damageClass        = 1;       // 0 impact, 1, radius
	damageValue        = 0.0;	//1.0
	damageType         = $ShrapnelDamageType;

	explosionRadius    = 1.0;
	kickBackStrength   = 0.0;
	maxLevelFlightDist = 275;
	totalTime          = 0.25;
	liveTime           = 0.25;
	projSpecialTime    = 0.01;

	inheritedVelocityScale = 0.5;
	smokeName              = "mortartrail.dts";
};

// Hammer payload when player is alive
MineData HammerShellMine
{
	mass = 5.0;		//5.0;
	drag = 2.0;	//1,0
	density = 1.0;	//5.0;	//2.0;
		elasticity = 0.1;	//0.1;	//0.15;
		friction = 2.0;	//1.0;
	className = "Handgrenade";	//className = "Mine";
	description = "Mini Mine";
	shapeFile = "mortar";
	shadowDetailMask = 4;
	explosionId = mortarExp;
	explosionRadius = 20.0;
	damageValue = 0.20;	//0.75;	//1.0;
	damageType = $ShockDamageType;
	kickBackStrength = 250;
	triggerRadius = 2.5;	
	maxDamage = 0.005;
};

// ==== ammo
ItemData HammerAmmo 
{
	description = "Hammer Ammo";
	className = "Ammo";
	heading = $InvHead[ihAmm];
	shapeFile = "mortarammo";
	shadowDetailMask = 4;
	price = 5;
};
MineData HammerAmmoBomb
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

GrenadeData HammerAmmoStray
{
	bulletShapeName = "mortar.dts";
	explosionTag = flashExpSmall;	//mortarExp;
	collideWithOwner = True;
	ownerGraceMS = 250;
	collisionRadius = 0.3;
	mass = 5.0;
	elasticity = 0.1;
	damageClass = 1; // 0 impact, 1, radius
	damageValue = 0.0;
	damageType = $MortarDamageType;
	explosionRadius = 10.0;
	kickBackStrength = 125.0;
	maxLevelFlightDist = 10;
	totalTime = 0.5;
	liveTime = 0.5;
	projSpecialTime = 0.01;
	inheritedVelocityScale = 0.5;
	smokeName = "mortartrail.dts";
};

//-------------------------------------------------------------------------------	

ItemImageData HammerImage
{
	shapeFile = "mortar";
	mountPoint = 0;
	weaponType = 0;
	ammoType = HammerAmmo;

//	mountOffset = { 0.0, -0.095, 0.05};		// 0.07, -0.05, 0.01 };//right, forward, up	//0.1, 0.25, 0.01
//	mountRotation = {0.0, 3.14, 0.0};		// ?, around gun barrel, ?
	accuFire = true;
	
	reloadTime = 0.1;	//0.1
	fireTime = 1.0;
	
	lightType = 3;
	lightRadius = 4;
	lightTime = 1;
	lightColor = { 1, 1, 0.2 };
	
	sfxFire = SoundFireMortar;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundMortarReload;
	sfxReady = SoundMortarIdle;	
};

ItemData Hammer
{
	description = "Lucifers Hammer";
	className = "Weapon";
	shapeFile = "mortargun";
	hudIcon = "mortar";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = HammerImage;
	price = 175;
	showWeaponBar = true;
};

$SpareSlot1 = 7;
//-------------------------------------------------------------------------------	
function Hammer::MountExtras(%player,%weapon)
{	
//	Player::mountItem(%player,HammerA,$SpareSlot1);
	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>Lobs explosive canisters which detonate on close proximity to enemies.\n<jc><f2>Inflicts electrical damage rendering enemy <f1>shields, jetpacks, and energy weapons<f2> useless.");
}

function HammerImage::onFire(%player, %slot) 
{		
	if($debug)
		Anni::Echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));	
		
	Player::decItemCount(%player,$WeaponAmmo[Player::getMountedItem(%player,$WeaponSlot)],1);
		
	%vel = Item::getVelocity(%player);

	if(%vel == 0 || vector::normalize(%vel) != "-NAN -NAN -NAN")	
	{	
		%trans = GameBase::getMuzzleTransform(%player);
		%newObj = Projectile::spawnProjectile("HammerShell",%trans,%player,%vel);
		schedule("HammerShell::Arm(" @ %NewObj @ ", " @ %player @ ");", 0.1, %NewObj);	//0.3
	}
	else 	
		Anni::Echo("!! Butterfly Error, Hammer fire. vel ="@%vel);			
}


 
function HammerShell::Arm(%newobj,%this)
{	
	if (GameBase::getPosition(%newObj) != "0 0 0" && !player::isdead(%this))
	{
		%Pos = vector::add(GameBase::getPosition(%newobj),"0 0 0.1"); 
   		%vel = Item::getVelocity(%newobj);
   		%Mine = newObject("","Mine","HammerShellMine");
 		addToSet("MissionCleanup", %Mine);
      		GameBase::throw(%Mine,%this,1,true);	
		GameBase::setPosition(%Mine, %pos);
		Item::setVelocity(%Mine, %vel);	
		%mine.owner = %this;
			
	//	schedule("Deleteobject("@%newobj@");",0.01);	
		Deleteobject(%newobj);
	}
}


function HammerShellMine::onAdd(%this)
{
	%this.damage = 0;
	%this.spawntime = getSimTime();
	HammerShellMine::deployCheck(%this);
	$mine::count++;
	if(!$dedicated)
		bottomprint(2049,"Ammo count= "@$Ammo::count@" Item count= "@$item::count@" Miner count= "@$mine::count);
}




function HammerShellMine::onCollision(%this,%object)
{	
	if($debug) 
		event::collision(%this,%object);

	%type = getObjectType(%object);
	//messageall(1,%type);
	//%data = GameBase::getDataName(%this);
	if(%type == "Player" || %type == Flier) 
	{
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
				Anni::Echo("+ MA Hammer "@client::getname(Player::getclient(%object)));
				%time = getsimtime();
				%object.HammeredTime = %time;
				%object.morteredBy = Player::getclient(%owner);
			}
				
		}
	}
}

function HammerShellMine::deployCheck(%this)
{
	%spawntime = %this.spawntime;
	%livetime = getSimTime() - %spawntime;
	if($debug)
		bottomprintall(%this@" Hammer time "@%livetime);
		
	if(GameBase::isAtRest(%this) || %livetime > 10) 
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
		%obj = Projectile::spawnProjectile("HammerTrailGren", %trans, %this, %vel);
		Projectile::spawnProjectile(%obj);
		GameBase::setPosition(%obj, %pos);
		Item::setVelocity(%obj, %vel);
			
		%obj2 = Projectile::spawnProjectile("ShockedDamage", %trans, %this, %Vel);
		Projectile::spawnProjectile(%obj2);
		GameBase::setPosition(%obj2, %pos);
		Item::setVelocity(%obj2, %vel);	
	}
	else 	
		Anni::Echo("!! Butterfly Error, Hammer Trail. vel ="@%vel);	
		
	if(%this)	
		schedule("HammerShellMine::deployCheck(" @ %this @ ");", 0.25, %this);
}


