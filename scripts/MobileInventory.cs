$InvList[MobileInventoryPack]	= 1;
$MobileInvList[MobileInventoryPack] = 0;
$RemoteInvList[MobileInventoryPack] = 0;
AddItem(MobileInventoryPack);

$CanAlwaysTeamDestroy[MobileInventory] = 1;

ItemImageData MobileInventoryPackImage 
{
	shapeFile = "MagCargo";
	mountPoint = 2;
	mountOffset = { 0, -0.65, -0.4 };
	mass = 5.0;
	firstPerson = false;
};

ItemData MobileInventoryPack
{
	description = "Mobile Inventory";
	shapeFile = "inventory_sta";
	classname = "Backpack";
	heading = $InvHead[ihPwr];
	imageType = MobileInventoryPackImage;
	shadowDetailMask = 4;
	elasticity = 0.2;
	price = 5000;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function MobileInventoryPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player); 
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Mobile Inventory: <f2>Deploys a large <f1>Inventory Station<f2> <f2>Independent from base power.");	
}

StaticShapeData MobileInventory //MobileInventoryPack::deployShape(2050,MobileInventoryPack);
{
	description = "Mobile Inventory";
	shapeFile = "inventory_sta";
	classname = "MobileStation";
	visibleToSensor = true;
	sequenceSound[0] = { "activate", SoundActivateInventoryStation };
	sequenceSound[1] = { "power", SoundInventoryStationPower };
	sequenceSound[2] = { "use", SoundUseInventoryStation };
	maxDamage = 1.5;
	debrisId = flashDebrisLarge;
	mapFilter = 4;
	mapIcon = "M_station";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	triggerRadius = 1.5;
	explosionId = flashExpLarge;
	shieldShapeName = "shield";
};

function MobileInventoryPack::deployShape(%player,%item)
{
	//Deploy ( Player, item, shape, placement, Max Dist, Categorys, Surfaces, Delete, PowerReq,PRange)
	//DeployStuff(%player,%item,MobileInv,1,5,0,0,true,1,250);
	
	echo(%player@" <<< DeployShape Stuff <<< --- <<< >>> "@%item);
	%team = GameBase::getTeam(%player);
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

	if($TeamItemCount[%team @ %item] >= $TeamItemMax[%item] && !$build)
	{
		Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %descr);
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
		
	if(Vector::dot($los::normal,"0 0 1") <= 0.7)
	{
		Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
	if(GameBase::getDataName($los::object) != "DeployablePlatform")
	{
		%obj = getObjectType($los::object);
		if(%obj != "SimTerrain" && %obj != "InteriorShape" && !$build) 
		{
			Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");
			Client::sendMessage(%client,0,"~wC_BuySell.wav");
			return false;
		}		
			
		if(!checkInvDeployArea(%client,$los::position)) 
		{
			Client::sendMessage(%client,0,"Cannot deploy here");
			Client::sendMessage(%client,0,"~wC_BuySell.wav");
			return false;
		}
	}
	
	%obj = newObject("Mobile Inventory","StaticShape","MobileInventory",true);
	%obj.cloakable = true;	//for base cloaker
	if(%player.repackEnergy != "")
	{
    GameBase::setDamageLevel(%obj, %player.repackDamage);
    GameBase::setEnergy(%obj, %player.repackEnergy);
    %player.repackDamage = "";
    %player.repackEnergy = "";
	}

	if($debug) Anni::Echo($Ver,"|Created New Object :",%obj," ","Mobile Inventory");
	
	//GameBase::playSequence(%obj,1,"deploy");
	//GameBase::SetActive(%obj,false);
	
	GameBase::setTeam(%obj,%team);
	addToSet("MissionCleanup/deployed/station", %obj);
	GameBase::setPosition(%obj,$los::position);
	GameBase::setRotation(%obj,GameBase::getRotation(%player));
	Gamebase::setMapName(%obj,"Mobile Inventory");
	Client::sendMessage(%client,0,"Mobile Inventory Deployed.");
	playSound(SoundCreateItem,$los::position);
	$TeamItemCount[%team @ %item]++;
//	DropshipTeamMessage(%team, 3, "Satellite Power Connection Established: "@GameBase::getDataName(%obj).description@" #"@%obj@".");
	Client::sendMessage(%client,0,"Satellite Power Connection Established: "@GameBase::getDataName(%obj).description@" #"@%obj@".");
	%obj.deployer = %client; 	
	
	AddInventoryTrigger(%obj);
//	Station::FindPower(%obj);

	schedule("MAnn::PowerTap("@%obj@");",0.1,%obj);			

	return true;	
}

function MobileInventory::onDestroyed(%this)
{
	if($trace) 
		Anni::Echo($ver,"| MobileInventoryPack::onDestroyed");
	MobileInventory::onDisabled(%this);
	
	GameBase::setDamageLevel(%this.lTurret,1100);	
	GameBase::setDamageLevel(%this.rTurret,1100);	
	deleteobject(%this.trigger);
	
	
	%this.cloakable = "";
	%this.nuetron = "";
	StaticShape::objectiveDestroyed(%this);
	if(!$NoCalcDamage)
		calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 3, 0.55, 0.30, 250, 170);
	$TeamItemCount[GameBase::getTeam(%this) @ "MobileInventoryPack"]--;
}

function MobileInventory::onEnabled(%this)
{
	if(GameBase::isPowered(%this)) 
	{
		GameBase::playSequence(%this,0,"power");
		GameBase::playSequence(%this,1);
		

		ZappyPowerSwitch(%this,true);			
	}
}

function MobileInventory::onDisabled(%this)
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		%player = Client::getOwnedObject(%cl);
		if(%player.InvObject == %this)
		{
			%cl.ConnectBeam = "";	//internal
			%cl.InvTargetable = "";	//internal
			%Cl.InvConnect = "";	//external
			if(!%cl.isBlackOut)
				bottomprint(%cl,"CLEAR!",0.01);	
			QuickInvOff(%cl);	
			%player.ZappyResupply = "";
			%cl.ListType = "";
		}
	}
	Station::weaponCheck(%this);
	GameBase::stopSequence(%this,0);
	GameBase::setSequenceDirection(%this,1,0);
	GameBase::pauseSequence(%this,1);
	GameBase::stopSequence(%this,2);
	Station::checkTarget(%this);
	if($trace) 
		Anni::Echo($ver,"| MobileInventory::onDisabled ",%this);

	ZappyPowerSwitch(%this,false);
}

function MobileInventory::onEndSequence(%this,%thread)
{
	if(Station::onEndSequence(%this,%thread)) 
		InventoryStation::onResupply(%this,"MobileInvList");
}


function MobileInventory::onPower(%this,%power,%generator)
{
	//Anni::Echo(%generator);
	if(%power) 
	{
		GameBase::playSequence(%this,0,"power");
		GameBase::playSequence(%this,1);

		ZappyPowerSwitch(%this,true);
		
	}
	else 
	{	
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			%player = Client::getOwnedObject(%cl);
			if(%player.InvObject == %this)
			{
				%cl.ConnectBeam = "";	//internal
				%cl.InvTargetable = "";	//internal
				%Cl.InvConnect = "";	//external
				if(!%cl.isBlackOut)
					bottomprint(%cl,"CLEAR!",0.01);	
				QuickInvOff(%cl);	
				%player.ZappyResupply = "";
				%cl.ListType = "";
			}
		}
		Station::weaponCheck(%this);
		GameBase::stopSequence(%this,0);	
		GameBase::setSequenceDirection(%this,1,0);
		GameBase::pauseSequence(%this,1);
		GameBase::stopSequence(%this,2);
		Station::checkTarget(%this);	
		
		%team = GameBase::getTeam(%this);
		if(!GameBase::isPowered(%this))
		{	
			if(!$build)

			schedule("MAnn::PowerTap("@%this@");",0.1,%obj);	

			ZappyPowerSwitch(%this,false);
		}
		
	}
}