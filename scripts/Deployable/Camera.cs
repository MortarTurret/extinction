$InvList[CameraPack] = 1;
$MobileInvList[CameraPack] = 1;
$RemoteInvList[CameraPack] = 1;
AddItem(CameraPack);

$CanAlwaysControl[CameraTurret] = 1;
$CanAlwaysTeamDestroy[CameraTurret] = 1;

ItemImageData CameraPackImage 
{
	shapeFile = "camera";
	mountPoint = 2;
	mountOffset = { 0, -0.1, -0.06 };
	mountRotation = { 0, 0, 0 };
	firstPerson = false;
};

ItemData CameraPack 
{
	description = "Camera";
	shapeFile = "camera";
	className = "Backpack";
	heading = $InvHead[ihDSe];
	imageType = CameraPackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 100;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function CameraPack::deployShape(%player,%item) 
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
	%obj = getObjectType($los::object);
	if(%obj != "SimTerrain" && %obj != "InteriorShape" && GameBase::getDataName($los::object) != "DeployablePlatform")
	{
		Client::sendMessage(%client,0,"Can only deploy on terrain or buildings.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
		
	%prot = GameBase::getRotation(%player);
	%zRot = getWord(%prot,2);
	if(Vector::dot($los::normal,"0 0 1") > 0.6) 
		%rot = "0 0 " @ %zRot;
	else 
	{
		if(Vector::dot($los::normal,"0 0 -1") > 0.6) 
			%rot = "3.14159 0 " @ %zRot;
		else 
			%rot = Vector::getRotation($los::normal);
	}
	if(!checkInvDeployArea(%client,$los::position)) 
	{
		return false;
	}
	%camera = newObject("Camera","Turret",CameraTurret,true);
	%camera.deployer = %client;
	if(%player.repackEnergy != "")
	{
    GameBase::setDamageLevel(%camera, %player.repackDamage);
    GameBase::setEnergy(%camera, %player.repackEnergy);
    %player.repackDamage = "";
    %player.repackEnergy = "";
	}

	%camera.cloakable = true;
	addToSet("MissionCleanup/deployed/sensor", %camera);
	GameBase::setTeam(%camera,GameBase::getTeam(%player));
	GameBase::setRotation(%camera,%rot);
	GameBase::setPosition(%camera,$los::position);
	Gamebase::setMapName(%camera,"Camera");
	Client::sendMessage(%client,0,"Camera deployed");
	playSound(SoundPickupBackpack,$los::position);
	$TeamItemCount[GameBase::getTeam(%camera) @ "CameraPack"]++;
	if(!$build)
		Anni::Echo("MSG: ",%client," deployed a Camera");
	$turret::count++;
	
	%obj = $los::object;
	if(%obj.inmotion == true)	 
	{ 
		schedule("replaceSensor("@%camera@");",5,%camera);
	}	
	
	return true;
}

TurretData CameraTurret 
{	className = "Turret";
	shapeFile = "camera";
	maxDamage = 2.5; 
	maxEnergy = 10;
	speed = 20;
	speedModifier = 1.0;
	range = 50;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	visibleToSensor = true;
	shadowDetailMask = 4;
	castLOS = true;
	supression = false;
	supressable = false;
	mapFilter = 2;
	mapIcon = "M_camera";
	debrisId = defaultDebrisSmall;
	FOV = 0.707;
	pinger = false;
	explosionId = debrisExpMedium;
	description = "Camera";
};

function CameraPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Camera: <f2>Deploy and control from a <f1>Command Station<f2> or with a <f1>Laptop Pack<f2>.\n<jc><f2>Sensor jamming enemies in the line of site of a camera are made detectable by turrets.");	
}

function CameraTurret::onAdd(%this) 
{

	schedule("CameraTurret::deploy(" @ %this @ ");",1,%this);
	if(GameBase::getMapName(%this) == "") 
		GameBase::setMapName (%this, "Camera");
}

function CameraTurret::deploy(%this) 
{
	GameBase::playSequence(%this,1,"deploy");
}

function CameraTurret::onEndSequence(%this,%thread) 
{
	GameBase::setActive(%this,true);
}

function CameraTurret::onDestroyed(%this) 
{
	Turret::onDestroyed(%this);
	%this.OrgTeam = "";
	$TeamItemCount[GameBase::getTeam(%this) @ "CameraPack"]--;
}
