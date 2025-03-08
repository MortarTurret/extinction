$InvList[PlatformPack] = 1;
$MobileInvList[PlatformPack] = 1;
$RemoteInvList[PlatformPack] = 1;
AddItem(PlatformPack);

$CanAlwaysTeamDestroy[DeployablePlatform] = 1;

ItemImageData PlatformPackImage 
{
	shapeFile = "AmmoPack";
	mountPoint = 2;
	mountOffset = { 0, -0.03, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData PlatformPack 
{
	description = "Deployable Platform";
	shapeFile = "AmmoPack";
	className = "Backpack";
	heading = $InvHead[ihBar];
	imageType = PlatformPackImage;
	shadowDetailMask = 4;
	mass = 1.5;
	elasticity = 0.2;
	price = 600;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function PlatformPack::deployShape(%player,%item) 
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
//		GameBase::playSound(%player, SoundPackUse, 0);
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}

	%obj = $los::object;
	%type = GameBase::getDataName(%obj);
	
	%deployPos = $los::position;
	%DeployRot = GameBase::getRotation(%player);//floor
	if(%type == DeployablePlatform)
	{
		%PlatPos = GameBase::getPosition(%obj);
		%PlatRot = GameBase::getRotation(%obj);
		for(%f = 0; %f < 6; %f += 1.575)
		{
			%markerpos =  vector::add(%PlatPos,rotatevector("2.3 0 0.5",vector::add(%PlatRot,"0 0 "@%f )));
			if(Vector::getdistance(%markerpos,%deployPos) < 0.5)
			{
				%deployPos = vector::add(rotatevector("6 0 0",vector::add(%PlatRot,"0 0 "@%f )),%PlatPos);
				%DeployRot = %PlatRot;
			}
			
		}
	}
	if(%obj.inmotion == true && !$Build)	 
	{ 
		Client::sendMessage(%client,0,"Deploy area crappy, cannot deploy.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
	
		
	

	if (Vector::dot($los::normal,"0 0 -1") > 0.6) 
	{
		%prot = GameBase::getRotation(%player);
		%xRot = getWord(%prot,0);
		%yRot = getWord(%prot,1);
		%zRot = getWord(%prot,2);			
		%rot =  %xRot + 3.14159@" " @%yRot@" "@%zRot;//ceiling...
		%up = true;
	}	
	
	%platform = newObject("DeployablePlatform", "StaticShape", DeployablePlatform, true);
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
	GameBase::setPosition(%platform, %deployPos);
	GameBase::setRotation(%platform, %deployRot);
	Gamebase::setMapName(%platform, "Deployable Platform "@Client::getName(%client));
	if(!$build)
		Client::sendMessage(%client,0,"Platform Deployed");
	GameBase::startFadeIn(%platform);
	playSound(SoundPickupBackpack,%deployPos);
	$TeamItemCount[GameBase::getTeam(%player) @ "PlatformPack"]++;
	%platform.deployer = %client; 
	
	if($build && !%up)
	{		
		GameBase::setPosition(%player, vector::add(GameBase::getPosition(%player),"0 0 1"));		
	}	
	if(!$build)
		Anni::Echo("MSG: ",%client," deployed an platform");
		
		
//	for(%f = 0; %f < 6; %f += 1.575)
//	{
//		%platform = newObject("DeployableForce", "StaticShape", DeployableForce, true);
//		addToSet("MissionCleanup/deployed/Barrier", %platform);	
//		%markermove = rotatevector("2.3 0 0.5",vector::add(%deployRot,"0 0 "@%f ));
//		Anni::Echo(%f@" "@%markermove);
//		%markerpos =  vector::add(%deployPos,%markermove);
//		GameBase::setPosition(%platform, %markerPos);	
//		
//	}
		
		
	return true;
}
StaticShapeData DeployableForce 
{
	shapeFile = "force";
	debrisId = defaultDebrisSmall;
	maxDamage = 10.0; 
	visibleToSensor = false;
	isTranslucent = true;
	description = "Force Marker";
};

StaticShapeData DeployablePlatform 
{
	shapeFile = "elevator6x6thin";
	debrisId = defaultDebrisSmall;
	maxDamage = 10.0; 
	visibleToSensor = false;
	isTranslucent = true;
	description = "Deployable Platform";
};

function DeployablePlatform::onDestroyed(%this) 
{
	if(!$NoCalcDamage)
		calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 2, 0.40, 0.1, 250, 100);
	$TeamItemCount[GameBase::getTeam(%this) @ "PlatformPack"]--;
}

function PlatformPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Deployable Platform: <f2>A platform which may be used to cover turrets to further defend them.");	
}