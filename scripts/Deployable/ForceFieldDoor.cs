$InvList[ForceFieldDoorPack] = 1;
$MobileInvList[ForceFieldDoorPack] = 1;
$RemoteInvList[ForceFieldDoorPack] = 0;
AddItem(ForceFieldDoorPack);

$CanAlwaysTeamDestroy[ForceFieldDoor] = 1;

ItemImageData ForceFieldDoorPackImage
{	
	shapeFile = "AmmoPack";
	mountPoint = 2;
	mountOffset = { 0, -0.03, 0 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData ForceFieldDoorPack
{	
	description = "Force Field Door";
	shapeFile = "AmmoPack";
	className = "Backpack";
	heading = $InvHead[ihBar];
	imageType = ForceFieldDoorPackImage;
	shadowDetailMask = 4;
	mass = 2.5;
	elasticity = 0.2;
	price = 500;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function ForceFieldDoorPack::deployShape(%player,%item)
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


	%obj = $los::object;
	if(%obj.inmotion == true)	 
	{ 
		Client::sendMessage(%client,0,"Deploy area crappy, cannot deploy.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
		
	%rot = GameBase::getRotation(%player);
	%obj = newObject("ForceFieldDoorPack","StaticShape",ForceFieldDoorShape,true);
	if(%player.repackEnergy != "")
	{
    GameBase::setDamageLevel(%obj, %player.repackDamage);
    GameBase::setEnergy(%obj, %player.repackEnergy);
    %player.repackDamage = "";
    %player.repackEnergy = "";
	}

	%obj.cloakable = true;
	addToSet("MissionCleanup/deployed/Barrier", %obj);
	GameBase::setTeam(%obj,GameBase::getTeam(%player));
	GameBase::setRotation(%obj,%rot);
	GameBase::setPosition(%obj,$los::position);
	Gamebase::setMapName(%obj,"Force Field Door "@Client::getName(%client));
	Client::sendMessage(%client,0,"Force Field Door deployed");
	playSound(SoundPickupBackpack,$los::position);
	$TeamItemCount[GameBase::getTeam(%obj) @ "ForceFieldDoorPack"]++;
	%obj.deployer = %client; 	
	if(!$build)
		Anni::Echo("MSG: ",%client," deployed a Force Field Door ");
	return true;
}

StaticShapeData ForceFieldDoorShape
{	
	className = "ForceField";
	damageSkinData = "objectDamageSkins";
	shapeFile = "forcefield_5x5";
	maxDamage = 10.0; 
	maxEnergy = 200;
	mapFilter = 2;
	visibleToSensor = true;
	explosionId = mortarExp;
	debrisId = flashDebrisLarge;
	lightRadius = 12.0;
	lightType=2;
	lightColor = {1.0,0.2,0.2};
	side = "single";
	isTranslucent = true;
	description = "Force Field Door";
};

function ForceFieldDoorShape::Destruct(%this)
{	
	ForceFieldDoorShape::doDamage(%this);
}

function ForceFieldDoorShape::doDamage(%this) 
{	
	calcRadiusDamage(%this, $DebrisDamageType, 5, 0.5, 25, 15, 4, 0.4, 0.1, 250, 100);
}

function ForceFieldDoorShape::onDestroyed(%this)
{	
	if(!$NoCalcDamage)
		ForceFieldDoorShape::doDamage(%this);
	%this.cloakable = "";
	%this.nuetron = "";
	$TeamItemCount[GameBase::getTeam(%this) @ "ForceFieldDoorPack"]--;
}

function ForceFieldDoorShape::onCollision(%this,%obj)
{	
	%data = GameBase::getDataName(%obj);
	%velocity = vector::getdistance(Item::GetVelocity(%obj),"0 0 0");
	if($debug)
		Anni::Echo("!!Collision "@%data@" hitting "@GameBase::getDataName(%this)@" Vel. "@%velocity);
						
	if(getObjectType(%obj) == "Flier") 
	{ 		
		%damage = GameBase::getDamageLevel(%obj) + %velocity/10;
		GameBase::setDamageLevel(%obj,%damage);
		playSound(SoundFlierCrash,GameBase::getPosition(%obj));
	}
	
	if(getObjectType(%obj)!="Player" || Player::isDead(%obj))
		return;
		
	%c = Player::getClient(%obj);
	%playerTeam = GameBase::getTeam(%obj);
	%fieldTeam = GameBase::getTeam(%this);
	if(%fieldTeam != %playerTeam)
		return;
		
	ForceFieldDoorShape::openDoor(%this);
	return;
}

function ForceFieldDoorShape::openDoor(%this) 
{	
	GameBase::startfadeout(%this);
	%pos=GameBase::getPosition(%this);
	%opos = %pos;
	%pos=Vector::add(%pos,"0 0 1000");
	GameBase::setPosition(%this,%pos);
	schedule("GameBase::playSound("@%this@",ForceFieldOpen,0);",0.15,%this);
	schedule("ForceFieldDoorShape::closeDoor("@%this@", \""@%opos@"\");",2,%this);
}

function ForceFieldDoorShape::closeDoor(%this, %pos) 
{	
	GameBase::setPosition(%this,%pos);
	GameBase::startfadein(%this);
	schedule("GameBase::playSound("@%this@",ForceFieldClose,0);",0.15,%this);
}

function ForceFieldDoorPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Force Field Door: <f2>A force field door that can only be used by your team.");	
}