$InvList[MotionSensorPack] = 1;
$MobileInvList[MotionSensorPack] = 1;
$RemoteInvList[MotionSensorPack] = 1;
AddItem(MotionSensorPack);

$CanAlwaysTeamDestroy[DeployableMotionSensor] = 1;

ItemImageData MotionSensorPackImage 
{
	shapeFile = "sensor_small";
	mountPoint = 2;
	mountOffset = { 0, 0, 0.1 };
	mountRotation = { 1.57, 0, 0 };
	firstPerson = false;
};

ItemData MotionSensorPack 
{
	description = "Motion Sensor";
	shapeFile = "sensor_small";
	className = "Backpack";
	heading = $InvHead[ihDSe];
	imageType = MotionSensorPackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 125;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function MotionSensorPack::deployShape(%player,%item) 
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
		Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
			
	%prot = GameBase::getRotation(%player);
	%zRot = getWord(%prot,2);
	if(Vector::dot($los::normal,"0 0 1") > 0.6) 
		%rot = "0 0 " @ %zRot;
	else 
	{	if(Vector::dot($los::normal,"0 0 -1") > 0.6) 
			%rot = "3.14159 0 " @ %zRot;
		else 
			%rot = Vector::getRotation($los::normal);
	}
	if(!checkInvDeployArea(%client,$los::position)) 
	{
		return false;
	}
	%mSensor = newObject("","Sensor",DeployableMotionSensor,true);
	%mSensor.deployer = %client;
	if(%player.repackEnergy != "")
	{
    GameBase::setDamageLevel(%mSensor, %player.repackDamage);
    GameBase::setEnergy(%mSensor, %player.repackEnergy);
    %player.repackDamage = "";
    %player.repackEnergy = "";
	}

	%msensor.cloakable = true;
	addToSet("MissionCleanup/deployed/sensor", %mSensor);
	GameBase::setTeam(%mSensor,GameBase::getTeam(%player));
	GameBase::setRotation(%mSensor,%rot);
	GameBase::setPosition(%mSensor,$los::position);
	Gamebase::setMapName(%mSensor,"Motion Sensor");
	Client::sendMessage(%client,0,"Motion Sensor deployed");
	playSound(SoundPickupBackpack,$los::position);
	if(!$build)
		Anni::Echo("MSG: ",%client," deployed a Motion Sensor");

	%obj = $los::object;
	if(%obj.inmotion == true)	 
	{ 
		schedule("replaceSensor("@%mSensor@");",5,%mSensor);
	}	
	$TeamItemCount[GameBase::getTeam(%player) @ "MotionSensorPack"]++;
	return true;
}

SensorData DeployableMotionSensor
{
	description = "Motion Sensor";
	className = "DeployableSensor";
	shapeFile = "sensor_small";
	shadowDetailMask = 16;
	visibleToSensor = true;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	sequenceSound[1] = { "power", SoundDiscSpin };
	//explosionId = DebrisExp;
	damageLevel = {0.8, 1.0};
	maxDamage = 3.0; 
	debrisId = defaultDebrisSmall;
	range = 50;
	dopplerVelocity = 1;
	castLOS = false;
	supression = false;
	supressable = false;
	pinger = false;
	mapFilter = 4;
	mapIcon = "M_motionSensor";
	damageSkinData = "objectDamageSkins";
};

function MotionSensorPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Motion Sensor: <f2>Detects nearby moving enemies, disables <f1>Sensor Jamming<f2> and makes them detectable by your turrets.");	
}

// Override base class just in case.
function DeployableMotionSensor::onPower(%this,%power,%generator)
{
	if(%power) 
		GameBase::playSequence(%this,0,"power");
//	else 
//		GameBase::stopSequence(%this,0);
}

function DeployableMotionSensor::onEnabled(%this)
{
	if(GameBase::isPowered(%this)) 
		GameBase::playSequence(%this,0,"power");
		GameBase::playSequence(%this,1,"deploy");
}

function DeployableMotionSensor::onDisabled(%this)
{
//	GameBase::stopSequence(%this,0);
}
