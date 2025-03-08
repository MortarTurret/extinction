// GRAVITY GUN
$InvList[GravityGun] = 1;
$MobileInvList[GravityGun] = 1;
$RemoteInvList[GravityGun] = 1;

$AutoUse[GravityGun] = false;

addWeapon(GravityGun);

ItemImageData GravityGunImage
{
	shapeFile = "paintgun"; // grenade
	mountPoint = 0;

	weaponType = 0;
	accuFire = true;
	reloadTime = 0.2;
	fireTime = 0;
	minEnergy = 0;
	maxEnergy = 0;

	sfxActivate = SoundPickUpWeapon;
};

ItemData GravityGun
{
	description = "Gravity Gun";
	className = "Tool";
	shapeFile = "sniper";
	hudIcon = "weapon";
   	heading = $InvHead[ihtool];
	shadowDetailMask = 4;
	imageType = GravityGunImage;
	price = 375;
	showWeaponBar = true;
};

function GravityGunImage::onFire(%Player, %Item)
{
	%clientId = Player::getClient(%player);
	
			%clientId = Player::getClient(%player);	

		if(!$build)
		{
		Client::sendMessage(%clientId,0,"The Gravity Gun will not function unless Building is enabled. ");
		Client::sendMessage(%clientId,0,"~wC_BuySell.wav");
		return;
		}

	if(%player.moving)
	{
		%player.moveObj.beingMoved = "";

		%player.moving = false;
		%player.moveObj = "";
		%player.moveOff = "";
		%player.moveDst = "";
		%player.moveOriRot = "";
		%player.moveOriPos = "";

		Bottomprint(%clientId,"<jc><f2>Released object.",2);
	}
	else
	{
		if(GameBase::getLOSInfo(%Player,9000))
		{
			%nrm = $LOS::Normal;
			%pos = $LOS::Position;
			%obj = $LOS::Object;

			if(%obj.beingMoved) return;

			%objTyp = getObjectType($LOS::Object);
			%objPos = GameBase::getPosition(%obj);
			%objRot = GameBase::getRotation(%obj);

			%muzzPos = GetMuzzlePos(%player);
			%muzzRot = GetMuzzleRot(%player);

			if(%obj.isDoor)
			{
				%player.moving = true;
				%player.moveObj = %obj;
				%player.moveDst = Vector::getDistance(%muzzPos,%pos);
				%player.moveOriPos = GameBase::getPosition(%obj);
				%obj.beingMoved = true;

				GravityGun::doorLoop(%Player);

				Bottomprint(%clientId,"<jc><f2>Grabbing a door.",2);
				return;
			}

			if(%objTyp == "InteriorShape" || %objTyp == "Turret" || %objTyp == "StaticShape")
			{
				%player.moving = true;
				%player.moveObj = %obj;
				%player.moveOff = Vector::sub(%objPos,%pos);
				%player.moveDst = Vector::getDistance(%muzzPos,%pos);
				%player.moveOriRot = Vector::add(%muzzRot,%objRot);
				%obj.beingMoved = true;

				GravityGun::moveLoop(%Player);

				Bottomprint(%clientId,"<jc><f2>Grabbing a weightless object.",2);
			}
			else if(%objTyp == "Player")
			{
				%player.moving = true;
				%player.moveObj = %obj;
				%player.moveOff = Vector::sub(%objPos,%pos);
				%player.moveDst = Vector::getDistance(%muzzPos,%pos);
				%obj.beingMoved = true;

				GravityGun::gravityLoop(%Player);

				Bottomprint(%clientId,"<jc><f2>Gravitational pull established.",2);
			}
		}
	}
//	if($build == 0)
//    	{
//        bottomprint(%clientid,"You can't use that when builder isn't on!");
//	}
}


function GravityGun::doorLoop(%player)
{
	if(%player.moving)
	{
		%obj = %player.moveObj;
		%dst = %player.moveDst;
		%ori = %player.moveOriPos;

		%muzzPos = GetMuzzlePos(%player);
		%muzzRot = GetMuzzleRot(%player);

		%distVec = Vector::getFromRot(%muzzRot,%dst);
		%z = getWord(%distVec,2);

		GameBase::setPosition(%obj,getWord(%ori,0)@" "@getWord(%ori,1)@" "@getWord(%ori,2)+%z);

		Schedule("GravityGun::doorLoop("@ %Player @");", 0.01, %Player);
	}
}

function GravityGun::moveLoop(%player)
{
	if(%player.moving)
	{
		%obj = %player.moveObj;
		%off = %player.moveOff;
		%dst = %player.moveDst;
		%rot = %player.moveOriRot;

		%muzzPos = GetMuzzlePos(%player);
		%muzzRot = GetMuzzleRot(%player);

		%distVec = Vector::getFromRot(%muzzRot,%dst);
		%offVec = Vector::add(%distVec,%off);

		if(%player.gRotate)
		{
			%mRotX = getWord(%muzzRot,0)*2;
			%mRotY = getWord(%muzzRot,1)*2;
			%mRotZ = getWord(%muzzRot,2);
			%newRot = %mRotX@" "@%mRotY@" "@%mRotZ;
			GameBase::setRotation(%obj,%newRot);
		}

		GameBase::setPosition(%obj,Vector::add(%muzzPos,%offVec));

		Schedule("GravityGun::moveLoop("@ %Player @");", 0.01, %Player);
	}
}

function GravityGun::gravityLoop(%player)
{
	if(%player.moving)
	{
		%obj = %player.moveObj;
		%off = %player.moveOff;
		%dst = %player.moveDst;

		%muzzPos = GetMuzzlePos(%player);
		%muzzRot = GetMuzzleRot(%player);

		%distVec = Vector::getFromRot(%muzzRot,%dst);
		%distVec = Vector::add(%muzzPos,%distVec);
		%offVec = Vector::add(%distVec,%off);

		%vel = Item::getVelocity(%obj);
		%pos = GameBase::getPosition(%obj);

		%dist = Vector::getDistance(%pos,%distVec);
		%rot = Vector::getRotAim(%pos,%distVec);
		%vec = Vector::getFromRot(%rot,%dist);
		%mass = 10;
		%mul = Vector::mult(%vec,%mass@" "@%mass@" "@%mass);

		if(%player.gMove)
		{
			Player::applyImpulse(%obj,%mul);
		}
		else
		{
			Item::setVelocity(%obj,%mul);
		}

		Schedule("GravityGun::gravityLoop("@ %player @");", 0.1, %player);
	}
}

function GravityGun::boost(%player,%obj,%str)
{
	%muzzPos = GetMuzzlePos(%player);
	%muzzRot = GetMuzzleRot(%player);

	if(!isObject(%player.moveObj)) return;

	if(getObjectType(%player.moveObj) == "Player")
	{
		%obj = %player.moveObj;
		Player::applyImpulse(%obj,Vector::mult(Vector::getFromRot(%muzzRot),%str@" "@%str@" "@%str));
		PlaySound("SoundActivateMine",%muzzPos);

		Projectile::spawnProjectile("BlasterBolt",GameBase::getMuzzleTransform(%player),%player,Item::getVelocity(%Player),%obj); // GravityBoost
	}

	%player.moveObj.beingMoved = "";

	%player.moving = false;
	%player.moveObj = "";
	%player.moveOff = "";
	%player.moveDst = "";
	%player.moveOriRot = "";
	%player.moveOriPos = "";
}

// StaticShapeData TrackerTracer
// {
//	shapeFile = "tracer"; // tracer
//	maxDamage = 10.0;
//   	description = "Tracker Tracer";
//	disableCollision = true;
// };

function Vector::getVelRotation(%vel)
{
	%rotA = Vector::getRotation(%vel);
	%rot = Vector::add(%rotA,$Pi/-2@" 0 0");
	return %rot;
}

// function TrackPath(%player)
// {
//	%box = GetBoxCenter(%player);
//	%vel = Item::getVelocity(%player);
//
//	%obj = NewObject("Tracker",StaticShape,TrackerTracer,true);
//	AddToSet("MissionCleanup\\Extras",%obj);
//	GameBase::setPosition(%obj,Vector::add(%box,"0 0 6"));
//
//	if(%vel != "0 0 0")
//	{
//		GameBase::setRotation(%obj,Vector::getVelRotation(%vel));
//	}
//	else
//	{
//		GameBase::setRotation(%obj,GameBase::getRotation(%Player));
//	}
//
//	Schedule("GameBase::startFadeOut("@ %Obj @");", 18, %obj);
//	Schedule("deleteObject("@ %Obj @");", 20.5, %obj);
//
//	Schedule("TrackPath("@ %Player @");", 0.2, %player);
// }

//Stuff added from FileLoad.cs

function Vector::Mult(%Vec,%Mul)
{
	return GetWord(%Vec,0)*GetWord(%Mul,0)@" "@GetWord(%Vec,1)*GetWord(%Mul,1)@" "@GetWord(%Vec,2)*GetWord(%Mul,2);
}

function Vector::Div(%Vec,%Div)
{
	return GetWord(%Vec,0)/GetWord(%Div,0)@" "@GetWord(%Vec,1)/GetWord(%Div,1)@" "@GetWord(%Vec,2)/GetWord(%Div,2);
}

function Vector::getRotAim(%pos1,%pos2,%neg)
{
	%vec = Vector::normalize(Vector::neg(Vector::sub(%pos1,%pos2)));
	if(%neg)
		%vec = Vector::normalize(Vector::sub(%pos1,%pos2));
	%rot = Vector::add(Vector::getRotation(%vec),"1.570796327 0 0");
	return %rot;
}

function GetMuzzleRot(%Player)
{
	%Proj = Projectile::spawnProjectile("PlasmaBolt",GameBase::getMuzzleTransform(%Player),%Player,"0 0 0");
	%Rotation = GameBase::getRotation(%Proj);
	DeleteObject(%Proj);
	return %Rotation;
}

function GetMuzzlePos(%Player)
{
	%Proj = Projectile::spawnProjectile("PlasmaBolt",GameBase::getMuzzleTransform(%Player),%Player,"0 0 0");
	%Position = GameBase::getPosition(%Proj);
	DeleteObject(%Proj);
	return %Position;
}

function GetOffSetRot(%offset,%rot,%pos)
{
	%x = getWord(%offset,0);
	%y = getWord(%offset,1);
	%z = getWord(%offset,2);
	%posA = Vector::add(%pos,Vector::getFromRot(Vector::Add(%rot,"0 0 -1.570796327"),%x));
	%posB = Vector::add(%posA,Vector::getFromRot(%rot,%y));
	%posC = Vector::add(%posB,Vector::getFromRot(Vector::Add(%rot,"1.570796327 0 0"),%z));
	return %posC;
}

function GravityGun::MountExtras(%player,%weapon)
{	
	if((Player::getclient(%player)).weaponHelp && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>Grab and Move ANY Object or Players from ANY Distance.\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) WHILE holding something to change Rotation and Player Gravity Modes.");
}