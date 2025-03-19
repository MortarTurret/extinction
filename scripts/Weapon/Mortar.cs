
$InvList[Mortar] = 1;
$MobileInvList[Mortar] = 1;
$RemoteInvList[Mortar] = 1;

$InvList[MortarAmmo] = 1;
$MobileInvList[MortarAmmo] = 1;
$RemoteInvList[MortarAmmo] = 1;

$AutoUse[Mortar] = false;
$SellAmmo[MortarAmmo] = 5;
$WeaponAmmo[Mortar] = MortarAmmo;

addWeapon(Mortar);
addAmmo(Mortar, MortarAmmo, 2);

// $oundReload[MortarImage] = SoundPodReload;

//======= projectile


GrenadeData MortarShell
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
   damageType         = $MortarDamageType;

   explosionRadius    = 20.0;
   kickBackStrength   = 250.0;
   maxLevelFlightDist = 275;
   totalTime          = 30.0;
   liveTime           = 2.0;
   projSpecialTime    = 0.01;

   inheritedVelocityScale = 0.5;
   smokeName              = "mortartrail.dts";
};

GrenadeData MortarTrailGren
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

// mortar payload when player is alive
MineData MortarShellMine
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
	damageValue = 0.75;	//1.0;
	damageType = $MortarDamageType;
	kickBackStrength = 250;
	triggerRadius = 2.5;	
	maxDamage = 0.005;
};

// ==== ammo
ItemData MortarAmmo 
{
	description = "Mortar Ammo";
	className = "Ammo";
	heading = $InvHead[ihAmm];
	shapeFile = "mortarammo";
	shadowDetailMask = 4;
	price = 5;
};

MineData MortarAmmoBomb
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

GrenadeData MortarAmmoStray
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

//===== weapon
ItemImageData MortarImage 
{
	shapeFile = "mortargun";
	mountPoint = 0;
	weaponType = 0;
	ammoType = MortarAmmo;
//	projectileType = MortarShell;
	accuFire = false;
	reloadTime = 0.28;
	fireTime = 1.0;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };
	sfxFire = SoundFireMortar;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundMortarReload;
	sfxReady = SoundMortarIdle;
};

ItemData Mortar 
{
	description = "Mortar";
	className = "Weapon";
	shapeFile = "mortargun";
	hudIcon = "mortar";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = MortarImage;
	price = 375;
	showWeaponBar = true;
};
//-------------------------------------------------------------------------------	
function Mortar::MountExtras(%player,%weapon)
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>Lob this explosive canister at someone you love. Shells detonate when in close proximity to enemies.");
}


// This could probably be cleaned up but should work for now -Death666
function MortarImage::onFire(%player, %slot)
{		
	if($debug)
		Anni::Echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));	
		
	if(!%player.Reloading)
	{
		%player.Reloading = true;
		schedule(%player @ ".Reloading = false;" , 1.0, %player);
		
		Player::decItemCount(%player,$WeaponAmmo[Mortar],1);
			
		%vel = Item::getVelocity(%player);

		if(%vel == 0 || vector::normalize(%vel) != "-NAN -NAN -NAN")	
		{	
			%trans = GameBase::getMuzzleTransform(%player);
			%newObj = Projectile::spawnProjectile("MortarShell",%trans,%player,%vel);
			schedule("MortarShell::Arm(" @ %NewObj @ ", " @ %player @ ");", 0.3);
		}
		else 	
			echo("!! Butterfly Error, Mortar fire. vel ="@%vel);			
	}
}


function MortarShell::Arm(%newobj,%this)
{	
	if (GameBase::getPosition(%newObj) != "0 0 0" && !player::isdead(%this))
	{
		%Pos = vector::add(GameBase::getPosition(%newobj),"0 0 0.1"); 
   		%vel = Item::getVelocity(%newobj);
   		%Mine = newObject("","Mine","MortarShellMine");
 		addToSet("MissionCleanup", %Mine);
      		GameBase::throw(%Mine,%this,1,true);	
		GameBase::setPosition(%Mine, %pos);
		Item::setVelocity(%Mine, %vel);	
		%mine.owner = %this;
			
	//	schedule("Deleteobject("@%newobj@");",0.01);	
		Deleteobject(%newobj);
	}
}


function MortarShellMine::onAdd(%this)
{
	%this.damage = 0;
	%this.spawntime = getSimTime();
	MortarShellMine::deployCheck(%this);
}

function MortarShellMine::onCollision(%this,%object)
{	
	if($debug) 
		event::collision(%this,%object);

	%type = getObjectType(%object);
	//messageall(1,%type);
	//%data = GameBase::getDataName(%this);
	if(%type == "Player" || %type == Flier) 
	{
		schedule("GameBase::setDamageLevel("@%this@",1);",0.15);
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
				echo("+ MA Mortar "@client::getname(Player::getclient(%object)));
				%time = getsimtime();
				%object.mortaredTime = %time;
				%object.morteredBy = Player::getclient(%owner);
			}
				
		}
	}
}

function MortarShellMine::deployCheck(%this)
{
	%spawntime = %this.spawntime;
	%livetime = getSimTime() - %spawntime;
//	bottomprintall(%this@" mortar time "@%livetime);
//	echo(%this@" mortar time "@%livetime);
	if(GameBase::isAtRest(%this) || %livetime >15) 
	{
		//echo("mine (mortar) Detonate -at rest");
		schedule("GameBase::setDamageLevel("@%this@", 2);",1.25);	//0.75
		$mine::count--;
	}
	else 
	{
		%vel = Item::getVelocity(%this);

		if(%vel == 0 || vector::normalize(%vel) != "-NAN -NAN -NAN")	
		{
			%Pos = GameBase::getPosition(%this); 
			%box = getBoxCenter(%this);
			%trans =  "0 0 1 0 0 0 0 0 1 " @ %box;
		//	echo("Poof! pos"@%pos@" vel "@%vel);
			%obj = Projectile::spawnProjectile("MortarTrailGren", %trans, %this, %vel);
			Projectile::spawnProjectile(%obj);
			GameBase::setPosition(%obj, %pos);
			Item::setVelocity(%obj, %vel);	
		}
		else 	
			echo("!! Butterfly Error, Mortar Trail. vel ="@%vel);	
		
		schedule("MortarShellMine::deployCheck(" @ %this @ ");", 0.25, %this);
	
	}
}


