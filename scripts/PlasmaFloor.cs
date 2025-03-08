$InvList[PlasmafloorPack] = 1;
$MobileInvList[PlasmafloorPack] = 1;
$RemoteInvList[PlasmafloorPack] = 1;
AddItem(PlasmafloorPack);

$CanAlwaysTeamDestroy[DeployablePlasmaFloor] = 1;

ItemImageData PlasmafloorPackImage 
{
	shapeFile = "AmmoPack";
	mountPoint = 2;
	mountOffset = { 0, -0.03, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData PlasmafloorPack 
{
	description = "Plasma Floor";
	shapeFile = "AmmoPack";
	className = "Backpack";
	heading = $InvHead[ihBar];
	imageType = PlasmafloorPackImage;
	shadowDetailMask = 4;
	mass = 1.5;
	elasticity = 0.2;
	price = 600;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function PlasmafloorPack::deployShape(%player,%item) 
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
		if(!$build)Client::sendMessage(%client,0,"Deploy position out of range.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
//		GameBase::playSound(%player, SoundPackUse, 0);
		return false;
	}

	%obj = $los::object;
	if(%obj.inmotion == true && !$Build)	 
	{ 
		Client::sendMessage(%client,0,"Deploy area crappy, cannot deploy.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
	
		
	%rot = GameBase::getRotation(%player);//floor

	if (Vector::dot($los::normal,"0 0 -1") > 0.6) 
	{
		%prot = GameBase::getRotation(%player);
		%xRot = getWord(%prot,0);
		%yRot = getWord(%prot,1);
		%zRot = getWord(%prot,2);			
		%rot =  %xRot + 3.14159@" " @%yRot@" "@%zRot;//ceiling...
		%up = true;
	}	
	
	%platform = newObject("DeployablePlasmaFloor", "StaticShape", DeployablePlasmaFloor, true);
	if(%player.repackEnergy != "")
	{
    GameBase::setDamageLevel(%platform, %player.repackDamage);
    GameBase::setEnergy(%platform, %player.repackEnergy);
    %player.repackDamage = "";
    %player.repackEnergy = "";
	}

	addToSet("MissionCleanup/deployed/Barrier", %platform);
//	%rot = GameBase::getRotation(%player);
	GameBase::setTeam(%platform, GameBase::getTeam(%player));
	GameBase::setPosition(%platform, $los::position);
	GameBase::setRotation(%platform, %rot);
	Gamebase::setMapName(%platform, "Deployable Platform "@Client::getName(%client));
	if(!$build)
		Client::sendMessage(%client,0,"Platform Deployed");
	GameBase::startFadeIn(%platform);
	playSound(SoundPickupBackpack,$los::position);
	$TeamItemCount[GameBase::getTeam(%player) @ "PlasmafloorPack"]++;
	%platform.deployer = %client; //adding this in and making plasma floors pitchforkable in pitchfork.cs -death666
	if($build && !%up)
	{		
		GameBase::setPosition(%player, vector::add(GameBase::getPosition(%player),"0 0 1"));		
	}	
	if(!$build)
		Anni::Echo("MSG: ",%client," deployed an platform");
	return true;
}

StaticShapeData DeployablePlasmaFloor 
{
	shapeFile = "PLASMAWALL";
	debrisId = defaultDebrisSmall;
	maxDamage = 10.0; 
	visibleToSensor = false;
	isTranslucent = true;
	description = "Deployable Platform";
};

function DeployablePlasmaFloor::onDestroyed(%this) 
{
	if(!$NoCalcDamage)
		calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 2, 0.40, 0.1, 250, 100);
	$TeamItemCount[GameBase::getTeam(%this) @ "PlasmafloorPack"]--;
}

function DeployablePlasmaFloor::onCollision(%this,%object)
{
	if($debug) 
		event::collision(%this,%object);
		
	if(getObjectType(%object) == "Flier") 
	{ 
		%data = GameBase::getDataName(%object);
		if($debug)
			Anni::Echo(%data@" hitting "@GameBase::getDataName(%this));
			
		%damage = GameBase::getDamageLevel(%object) + 0.01;
		GameBase::setDamageLevel(%object,%damage);
		playSound(SoundFlierCrash,GameBase::getPosition(%object));
	}
}

function PlasmafloorPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player); 
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Plasma Floor: <f2>A small plasma floor for blocking off areas or shielding against <f1>Laser Turrets<f2> above and below you.");	
}
