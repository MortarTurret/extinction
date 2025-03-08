$InvList[CatPack] = 1;
$MobileInvList[CatPack] = 1;
$RemoteInvList[CatPack] = 1;
AddItem(CatPack);

$CanAlwaysTeamDestroy[DeployableCat] = 1;
$TeamItemMax[CatPack] = 10;

ItemImageData CatPackImage 
{
	shapeFile = "shieldPack";	//sensor_small";
	mountPoint = 2;
//	mountOffset = { 0, 0, 0.1 };
//	mountRotation = { 1.57, 0, 0 };
	firstPerson = false;
};

ItemData CatPack 
{
	description = "Deployable Cat";
	shapeFile = "mineammo";	//sensor_small";
	className = "Backpack";
	heading = $InvHead[ihDSe];
	imageType = CatPackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 125;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function CatPack::deployShape(%player,%item) 
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
	if(!checkDeployArea(%client,$los::position)) 
	{
		return false;
	}		
	%obj = getObjectType($los::object);
	if(%obj != "SimTerrain" && %obj != "InteriorShape" && GameBase::getDataName($los::object) != "DeployablePlatform") 
	{
		Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");
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
	
// Check for other cats to avoid cat fights...
	%set = newObject("set",SimSet);
	addToSet("MissionCleanup", %set);
	%Mask = $StaticObjectType; 
	%num = containerBoxFillSet(%set,%Mask,$los::position,50,50,50,0);
	%totalnum = Group::objectCount(%set);
	for(%i = 0; %i < %totalnum; %i++)
	{
		%obj = Group::getObject(%set, %i);
		if ( GameBase::getDataName(%obj) != DeployableCat )
			continue;
		%dist = Vector::getDistance($los::position, GameBase::getPosition(%obj));
		if(%dist < 10)
			if(%obj.CatTeam == GameBase::getTeam(%player))
				%no = true;
	}
	deleteObject(%set);
	if(!%no!=true)	
	{
		Client::sendMessage(%client,0,"Interference from another Cat sensor.");	
		Client::sendMessage(%client,0,"~wC_BuySell.wav");	
		return false;	
	}		

	//Ok, lets set this up. 
	%prot = GameBase::getRotation(%player);
	%zRot = getWord(%prot,2);
	%xpos = getWord($los::position,0);
	%ypos = getWord($los::position,1);
	%zpos = getWord($los::position,2);
	if(Vector::dot($los::normal,"0 0 1") > 0.6) 
	{
		%rot = "0 0 " @ %zRot;
	      //%zpos -= 0.05;//FLOOR
	}
	else 
	{	if(Vector::dot($los::normal,"0 0 -1") > 0.6) 
		{
			%rot = "3.14159 0 " @ %zRot;
			// %zpos -= 0.05;//CEILING
		}
		else 	
		{
			%rot = Vector::getRotation($los::normal);

			//WALL
			%xopos = getWord($los::normal,0);
			%yopos = getWord($los::normal,1);

			%xpos = %xpos + %xopos/10;
			%ypos = %ypos + %yopos/10;
			%xpos += 0.11;

		}
	}
	%pos = %xpos@" "@%ypos@" "@%zpos;
//	%rot = vector::add(%rot,"0 0 0"); // -1.57 0 0

	%mSensor = newObject("DeployableKitty","Turret",DeployableCat,true);
	%mSensor.deployer = %client;

	addToSet("MissionCleanup/deployed/sensor", %mSensor);
	%PlayerTeam = GameBase::getTeam(%player);
	if(%PlayerTeam == 0) 	%catTeam = 1;	//changed from 0 1 0 to 0 0 1 -death666
		else %catTeam = 0;	
	%TeamNum = getNumTeams()-1;	//
	GameBase::setTeam(%mSensor,%TeamNum);		//%catTeam
	%mSensor.CatTeam = %PlayerTeam;
	GameBase::setRotation(%mSensor,%rot);
	GameBase::setPosition(%mSensor,%pos);
	Gamebase::setMapName(%mSensor,"Pussy Cat #"@$TeamItemCount[GameBase::getTeam(%player) @ %item]+1);
	Client::sendMessage(%client,0,"Pussy Cat deployed");
//	playSound(SoundPickupBackpack,$los::position);
	playSound(SoundDeploySensor,$los::position);
	if(!$build)
		Anni::Echo("MSG: ",%client," deployed a Pussy Cat");
	$TeamItemCount[GameBase::getTeam(%player) @ "CatPack"]++;

//new stuff for lolz
	%pcd = newObject("Chameleon Detector","StaticShape",PCDsensor,true);
	%pcd.deployer = %client; 
	%pcd.cloakable = true;
	addToSet("MissionCleanup/deployed/sensor", %pcd);
	GameBase::setTeam(%pcd,GameBase::getTeam(%player));	
	if(%player.repackEnergy != "")
	{
	GameBase::setDamageLevel(%mSensor, %player.repackDamage);
    GameBase::setEnergy(%mSensor, %player.repackEnergy);
    GameBase::setDamageLevel(%pcd, %player.repackDamage);
    GameBase::setEnergy(%pcd, %player.repackEnergy);
    %player.repackDamage = "";
    %player.repackEnergy = "";
	}

    //  %backward = Vector::neg(Vector::getFromRot(%rot, -2.8)); //meaning backwards a little bit.
    //	GameBase::setPosition(%pcd,Vector::add($los::position, %backward));
//	%zpos += 0.09;//FLOOR
//	%xpos += 0.09;//FLOOR
//	%ypos += 0.09;//FLOOR
//	%pos = %xpos@" "@%ypos@" "@%zpos;

	%prot = GameBase::getRotation(%player);
	%zRot = getWord(%prot,2);
	%xpos = getWord($los::position,0);
	%ypos = getWord($los::position,1);
	%zpos = getWord($los::position,2);
	if(Vector::dot($los::normal,"0 0 1") > 0.6) 
	{
		%rot = "0 0 " @ %zRot;
	        %zpos += 0.08;//FLOOR //0.05
		%xpos -= 0.08;//FLOOR //0.05
	}
	else 
	{	if(Vector::dot($los::normal,"0 0 -1") > 0.6) 
		{
			%rot = "3.14159 0 " @ %zRot;
		        %zpos -= 0.06;//CEILING  //0.05
			%ypos -= 0.08;//FLOOR //0.05
		}
		else 	
		{
			%rot = Vector::getRotation($los::normal);

			//WALL
			%xopos = getWord($los::normal,0);
			%yopos = getWord($los::normal,1);

			%xpos = %xpos + %xopos/10;
			%ypos = %ypos + %yopos/10;
			%xpos += 0.02; //0.01
			%ypos -= 0.09;//FLOOR //0.05

		}
	}
	%pos = %xpos@" "@%ypos@" "@%zpos;

        GameBase::setPosition(%pcd,%pos);
	GameBase::setRotation(%pcd,%rot);
	Gamebase::setMapName(%pcd,"Chameleon Detector #"@$TeamItemCount[GameBase::getTeam(%player) @ %item]);

	%mSensor.pcd = %pcd;
	%pcd.mSensor = %mSensor;
	
	return true;
}

TurretData DeployableCat
{
	className = "CatTurret"; //changed from Turret to CatTurret to avoid killing team cats for points -death666
	shapeFile = "mineammo";	//sensor_small"; //"remoteturret";
	projectileType = lightningCharge;
	maxDamage = 3.0;
	maxEnergy = 300;
	minGunEnergy = 20;
	maxGunEnergy = 100;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	sequenceSound[1] = { "power", SoundUseCommandStation };
	reloadDelay = 2.5;	//2.5
	speed = 0.5;		//0.5
	speedModifier = 1.0;	//1.0
	range = 50; 
	visibleToSensor = false;	//true;
//	shadowDetailMask = 4;
	dopplerVelocity = 1;	//0;
	castLOS = false;	//true;
	supression = false;
	supressable = false;
	pinger = false;		//true;
//	mapFilter = 2;
//	mapIcon = "M_turret";
	debrisId = flashDebrisMedium;
	shieldShapeName = "shield";
	fireSound = SoundGeneratorPower;
	activationSound = SoundRemoteTurretOn;
	deactivateSound = SoundRemoteTurretOff;
	explosionId = flashExpMedium;
	description = "Cat";
	damageSkinData = "objectDamageSkins";
	
};

function DeployableCat::onAdd(%this)
{
	$Catnap[%this] = "";	//resetting for simtime.. gets reset every map, could carry over...
	schedule("DeployableCat::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,5);
	%this.shieldStrength = 0.010;//0.0
	if (GameBase::getMapName(%this) == "") {
		GameBase::setMapName (%this, "Pussy cat");
	}
}

function DeployableCat::deploy(%this)
{
	//GameBase::playSequence(%this,1,"deploy");
	GameBase::setActive(%this,true);
//	GameBase::playSequence(%this,0,"power");
}

function DeployableCat::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function DeployableCat::onDestroyed(%this)
{

//new stuff for lolz
	GameBase::setDamageLevel(%this.pcd, 2.6);

	GameBase::setActive(%this,"False");
	$Catnap[%this] = "";// clean up
	Turret::onDestroyed(%this);
	%this.OrgTeam = "";
	$TeamItemCount[GameBase::getTeam(%this) @ "CatPack"]--;
}

// new stuff for lolz

StaticShapeData PCDsensor
{
	description = "Chameleon Detector";
	shapeFile = "radar_small"; //liqcyl
	className = "Decoration";
	debrisId = flashDebrisMedium;
	maxDamage = 3.0;
	visibleToSensor = true;
	mapFilter = 4;
	mapIcon = "M_generator";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	explosionId = flashExpMedium;
        maxEnergy = 300; //new
	shieldShapeName = "shield"; //new
        shieldStrength = 0.010;//0.0
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
	sequenceSound[1] = { "power", SoundUseCommandStation };
};

function PCDsensor::onDestroyed(%this)
{

	Turret::onDestroyed(%this);
	%this.OrgTeam = "";
	$TeamItemCount[GameBase::getTeam(%this) @ "CatPack"]--;

	GameBase::stopSequence(%this,0);
	StaticShape::objectiveDestroyed(%this);
	%this.cloakable = "";
	GameBase::setDamageLevel(%this.mSensor, 2.6);
	if(!$NoCalcDamage)
		calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 2, 0.40, 0.1, 250, 100);
}

// Override base class just in case.
function PCDsensor::onPower(%this,%power,%generator)
{
	if(%power) 
		GameBase::playSequence(%this,0,"power");
//	else 
//		GameBase::stopSequence(%this,0);
}

function PCDsensor::onEnabled(%this)
{
//	if(GameBase::isPowered(%this)) 
		GameBase::playSequence(%this,0,"power");
		GameBase::playSequence(%this,1,"deploy");
}

function PCDsensor::onDisabled(%this)
{
//	GameBase::stopSequence(%this,0);
}

function CatPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player); 
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Deployable Cat: <f2>Detects nearby enemy <f1>Chameleons<f2> and alerts your team.\n<jc><f2>Has an increasing chance of disabling enemy <f1>Chameleon Packs<f2> based on the number of <f1>Deployable Cats<f2> in the same area.");	
}
