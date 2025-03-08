$InvList[PulseSensorPack] = 1;
$MobileInvList[PulseSensorPack] = 1;
$RemoteInvList[PulseSensorPack] = 1;
AddItem(PulseSensorPack);

$CanAlwaysTeamDestroy[DeployablePulseSensor] = 1;

ItemImageData PulseSensorPackImage 
{
	shapeFile = "radar_small";
	mountPoint = 2;
	mountOffset = { 0, 0, 0.1 };
	mountRotation = { 1.57, 0, 0 };
	firstPerson = false;
};

ItemData PulseSensorPack 
{
	description = "Pulse Sensor";
	shapeFile = "radar_small";
	className = "Backpack";
	heading = $InvHead[ihDSe];
	imageType = PulseSensorPackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 125;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};


function PulseSensorPack::DeployShape(%player,%item) 
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
	if(Item::deployShape(%player,"Pulse Sensor",DeployablePulseSensor,%item) && !$build) 
	{
		Player::decItemCount(%player,%item);
		$TeamItemCount[GameBase::getTeam(%player) @ "PulseSensorPack"]++;
	}
}

SensorData DeployablePulseSensor
{
	description = "Remote Pulse Sensor";
	className = "DeployableSensor";
	shapeFile = "radar_small";
	shadowDetailMask = 4;
	visibleToSensor = true;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	sequenceSound[1] = { "power", SoundAmmoStationLoopPower };
	damageLevel = {0.8, 1.0};
	maxDamage = 3.0; 
	debrisId = defaultDebrisSmall;
	//explosionId = flashDebrisSmall;
	range = 200; 
	castLOS = true;
	supression = false;
	mapFilter = 4;
	mapIcon = "M_Radar";
	damageSkinData = "objectDamageSkins";
};

function PulseSensorPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player); 
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Pulse Sensor: <f2>Increase the range of your teams sensor network.\n<jc><f1>Warning:<f2> Unlike <f1>Motion Sensors<f2>, pulse sensors are able to be jammed by enemy sensor jammers.");	
}

// Override base class just in case.
function DeployablePulseSensor::onPower(%this,%power,%generator)
{
	if(%power) 
		GameBase::playSequence(%this,0,"power");
//	else 
//		GameBase::stopSequence(%this,0);
}

function DeployablePulseSensor::onEnabled(%this)
{
	if(GameBase::isPowered(%this)) 
		GameBase::playSequence(%this,0,"power");
		GameBase::playSequence(%this,1,"deploy");
}

function DeployablePulseSensor::onDisabled(%this)
{
//	GameBase::stopSequence(%this,0);
}