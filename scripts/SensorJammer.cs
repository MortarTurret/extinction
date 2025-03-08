$InvList[DeployableSensorJammerPack] = 1;
$MobileInvList[DeployableSensorJammerPack] = 1;
$RemoteInvList[DeployableSensorJammerPack] = 1;
AddItem(DeployableSensorJammerPack);

$CanAlwaysTeamDestroy[DeployableSensorJammer] = 1;

ItemImageData DeployableSensorJamPackImage 
{
	shapeFile = "sensor_jammer";
	mountPoint = 2;
	mountOffset = { 0, 0.03, 0.1 };
	mountRotation = { 1.57, 0, 0 };
	firstPerson = false;
};

ItemData DeployableSensorJammerPack 
{
	description = "Sensor Jammer";
	shapeFile = "sensor_jammer";
	className = "Backpack";
	heading = $InvHead[ihDSe];
	imageType = DeployableSensorJamPackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 225;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};


function DeployableSensorJammerPack::DeployShape(%player,%item) 
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
	if(Item::deployShape(%player,"Sensor Jammer",DeployableSensorJammer,%item)&& !$build) 
	{
		Player::decItemCount(%player,%item);
		$TeamItemCount[GameBase::getTeam(%player) @ "DeployableSensorJammerPack"]++;
	}
}

SensorData DeployableSensorJammer
{
	description = "Remote Sensor Jammer";
	className = "DeployableSensor";
	shapeFile = "sensor_jammer";
	shadowDetailMask = 4;
	visibleToSensor = true;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	sequenceSound[1] = { "power", SoundFloatMineTargetLoop };
	damageLevel = {0.8, 1.0};
	maxDamage = 3.0; 
	//explosionId = DebrisExp;
	debrisId = defaultDebrisSmall;
	range = 80;
	castLOS = true;
	supression = true;
	mapFilter = 4;
	mapIcon = "M_sensorJammer";
};

function DeployableSensorJammerPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Sensor Jammer: <f2>Renders a small area of the map undetectable to enemy <f1>Pulse Sensors.");	
}

// Override base class just in case.
function DeployableSensorJammer::onPower(%this,%power,%generator)
{
	if(%power) 
		GameBase::playSequence(%this,0,"power");
//	else 
//		GameBase::stopSequence(%this,0);
}

function DeployableSensorJammer::onEnabled(%this)
{
	if(GameBase::isPowered(%this)) 
		GameBase::playSequence(%this,0,"power");
		GameBase::playSequence(%this,1,"deploy");
}

function DeployableSensorJammer::onDisabled(%this)
{
//	GameBase::stopSequence(%this,0);
}