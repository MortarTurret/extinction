$InvList[FlameTurretPack] = 1;
$MobileInvList[FlameTurretPack] = 1;
$RemoteInvList[FlameTurretPack] = 1;
AddItem(FlameTurretPack);

$CanAlwaysTeamDestroy[FlameTurret] = 1;

ItemImageData FlameTurretPackImage
{
	shapeFile = "remoteturret";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 3.0;
	firstPerson = false;
};

ItemData FlameTurretPack
{
	description = "Flame Turret";
	shapeFile = "remoteturret";
	className = "Backpack";
	heading =  $InvHead[ihTur];
	imageType = FlameTurretPackImage;
	shadowDetailMask = 4;
	mass = 3.0;
	elasticity = 0.2;
	price = 1000;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};


function FlameTurretPack::deployShape(%player,%item)
{
	%client = Player::getClient(%player);
	%clientId = Player::getClient(%player);

		if(!$build)
		{
	if(%clientId.inArena)
	{ 
		Client::sendMessage(%client,0,"Cannot deploy in arena unless building is on. ");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return;	
	}
		}
		if(!$build)
		{
	if(%player.outArea)
	{
		Client::sendMessage(%client,0,"can not deploy out of bounds unless building is on.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
		}

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item] && !$build)
	{
		Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
	if(!GameBase::getLOSInfo(%player,5))
	{
		Client::sendMessage(%client,0,"Deploy position out of range.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
	if(GameBase::getLOSInfo(%player,1.75))
	{
		Client::sendMessage(%client,0,"Deploy position is too close.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}

	%obj = $los::object;
	if(%obj.inmotion == true)	 
	{ 
		Client::sendMessage(%client,0,"Deploy area crappy, cannot deploy.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
		
	if(getNumTeams()-1 == 2)
	{
		%playerTeam = Client::getTeam(%client);
		if(%playerTeam == 0)
			%enemyTeam = 1;
		else if(%playerTeam == 1)
			%enemyTeam = 0;
		if(((Vector::getDistance($teamFlag[%enemyTeam].originalPosition, $los::position)) < ($FlagDistance * 0.4)) && ($FlagDistance != 0)) 
		{
			Client::sendMessage(%client,0,"You are too close to the enemy flag~waccess_denied.wav");
			Client::sendMessage(%client,0,"~wC_BuySell.wav");
			return false;
		}
	}
// 	%obj = getObjectType($los::object); allowing these to be set outside now -death666
//	if(%obj != "InteriorShape")
//	{
//		Client::sendMessage(%client,0,"Can only deploy in buildings");
//		return false;
//	}
	%set = newObject("set",SimSet);
	%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMaxLength,$TurretBoxMaxWidth,$TurretBoxMaxHeight,0);
	%num = CountTurrets(%set, %num);
	deleteObject(%set);
	if(%num > $MaxNumTurretsInBox) 
	{
		Client::sendMessage(%client,0,"Interference from other remote turrets in the area");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return;
	}
	%set = newObject("set",SimSet);
	%Mask = $StaticObjectType;
	%num = containerBoxFillSet(%set, $StaticObjectType, $los::position, $TurretBoxMinLength, $TurretBoxMinWidth, $TurretBoxMinHeight, 0);
	%num = CountObjects(%set, "FlameTurret", %num);
	deleteObject(%set);
	if(%num) 
	{	
		Client::sendMessage(%client,0,"Frequency Overload - Too close to other remote turrets");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return;
	}
	if(Vector::dot($los::normal,"0 0 1") <= 0.7)
	{
		Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
	if(!checkInvDeployArea(%client,$los::position))
	{
		Client::sendMessage(%client, 0, "Cannot deploy. Item in way");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
	%rot = GameBase::getRotation(%player);
	%turret = newObject("Flame Turret","Turret",FlameTurret,true);
	%turret.cloakable = true;
	%turret.deployer = %client; 
	$TurretList[%turret] = %client;	

	addToSet("MissionCleanup/deployed/turret", %turret);
	GameBase::setTeam(%turret,GameBase::getTeam(%player));
	GameBase::setPosition(%turret,$los::position);
	GameBase::setRotation(%turret,%rot);
	Gamebase::setMapName(%turret,"Flame Turret " @ Client::getName(%client));

	%cyl = newObject("Flame Turret Fuel","StaticShape",Canister,true);
	%cyl.cloakable = true;
	%cyl.deployer = %client; 
	if(%player.repackEnergy != "")
	{
    GameBase::setDamageLevel(%turret, %player.repackDamage);
    GameBase::setEnergy(%turret, %player.repackEnergy);
	GameBase::setDamageLevel(%cyl, %player.repackDamage);
    GameBase::setEnergy(%cyl, %player.repackEnergy);
    %player.repackDamage = "";
    %player.repackEnergy = "";
	}

	addToSet("MissionCleanup/deployed/turret", %cyl);
	GameBase::setTeam(%cyl,GameBase::getTeam(%player));
	%backward = Vector::neg(Vector::getFromRot(%rot, 0.8)); //meaning backwards a little bit.

	GameBase::setPosition(%cyl,Vector::add($los::position, %backward));
	GameBase::setRotation(%cyl,%rot);
	Gamebase::setMapName(%cyl,"Flame Turret Fuel");

	%turret.cyl = %cyl;
	%cyl.turret = %turret;

	$turret::count++;
	%obj.deployer = %client;
	Client::sendMessage(%client,0,"Flame Turret deployed");
	playSound(SoundPickupBackpack,$los::position);
	$TeamItemCount[GameBase::getTeam(%player) @ "FlameTurretPack"]++;
	Anni::Echo("MSG: ",%client," deployed an Flame Turret");
	//	Remote turrets - kill points to player that deploy them
	Client::setOwnedObject(%client, %turret);
	Client::setOwnedObject(%client, %player);
	return true;
}

TurretData FlameTurret
{
	className = "Turret";
	shapeFile = "remoteturret";
	projectileType = FlameShell;
	maxDamage = 0.55;
	maxEnergy = 200; //BR Setting
	minGunEnergy = 3;
	maxGunEnergy = 3;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	reloadDelay = 0.02;
	speed = 2.0;
	speedModifier = 1.5;
	range = 30; //BR Setting
	visibleToSensor = true;
	shadowDetailMask = 4;
	supressable = false;
	pinger = true;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_Radar";
	debrisId = flashDebrisMedium;
	shieldShapeName = "shield";
	fireSound = SoundFireFlamer;
	activationSound = SoundRemoteTurretOn;
	deactivateSound = SoundRemoteTurretOff;
	explosionId = flashExpMedium;
	description = "Flame Turret";
	damageSkinData = "objectDamageSkins";
};

function FlameTurret::onAdd(%this)
{
	schedule("FlameTurret::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,5);
	if(GameBase::getMapName(%this) == "") 
		GameBase::setMapName (%this, "Flame Turret");
}

function FlameTurret::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function FlameTurret::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function FlameTurret::onDestroyed(%this)
{
	Turret::onDestroyed(%this);
	%this.OrgTeam = "";
  	$TeamItemCount[GameBase::getTeam(%this) @ "FlameTurretPack"]--;
	GameBase::setDamageLevel(%this.cyl, 0.6);
}

// Override base class just in case.
function FlameTurret::onPower(%this,%power,%generator)
{
}

function FlameTurret::onEnabled(%this) 
{
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);
}

StaticShapeData Canister
{
	description = "Flame Turret Fuel";
	shapeFile = "liqcyl";
	className = "Decoration";
	debrisId = flashDebrisMedium;
	maxDamage = 0.55;
	visibleToSensor = true;
	mapFilter = 4;
	mapIcon = "M_generator";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	explosionId = flashExpMedium;
};

function Canister::onDestroyed(%this)
{
	GameBase::stopSequence(%this,0);
	StaticShape::objectiveDestroyed(%this);
	%this.cloakable = "";
	if(!$NoCalcDamage)
		calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 2, 0.40, 0.1, 250, 100);
	GameBase::setDamageLevel(%this.turret, 0.6);
}

// Override base class just in case.
function Canister::onPower(%this,%power,%generator)
{
	if(%power) 
		GameBase::playSequence(%this,0,"power");
	else 
		GameBase::stopSequence(%this,0);
}

function Canister::onEnabled(%this)
{
	if(GameBase::isPowered(%this)) 
		GameBase::playSequence(%this,0,"power");
}

function Canister::onDisabled(%this)
{
	GameBase::stopSequence(%this,0);
}

function FlameTurretPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player); 
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Flame Turret: <f2>Shoots a stream of plasma.");	
}