$InvList[PortableGeneratorPack] = 1;
$MobileInvList[PortableGeneratorPack] = 1;
$RemoteInvList[PortableGeneratorPack] = 1;

AddItem(PortableGeneratorPack);

$CanAlwaysTeamDestroy[PortableGenerator] = 1;

ItemImageData PortableGeneratorPackImage 
{
	shapeFile = "generator_p";
	mountPoint = 2;
	mountOffset = { 0, -0.65, -0.4 };
	mass = 5.0;
	firstPerson = false;
};

ItemData PortableGeneratorPack
{
	description = "Portable Generator";
//	shapeFile = "generator_p";
	shapeFile = "ammopack";
	classname = "Backpack";
	heading = $InvHead[ihPwr];
	imageType = PortableGeneratorPackImage;
	shadowDetailMask = 4;
	mass = 4.5;
	elasticity = 0.2;
	price = 2500;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function PortableGeneratorPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player); 
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Portable Generator: <f2>Powers: <f1>Base Inventories, Base Turrets and Base Sensors plus Airbases and Gunships. \n<jc><f1>Usage: <f2>Deploy at any point to backup Base Assets Power. Deploy after setting up a Air Base or Gunship to back their power up.");	
}


StaticShapeData PortableGenerator
{
	description = "Portable Generator";
	shapeFile = "generator_p";
	classname = "Generator";
	debrisId = flashDebrisSmall;
	sfxAmbient = SoundGeneratorPower;
	maxDamage = 6.0; // been 10.5
	mapIcon = "M_generator";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	explosionId = flashExpMedium;
	visibleToSensor = true;
	mapFilter = 4;
	shieldShapeName = "shield";
};

function PortableGeneratorPack::deployShape(%player,%item)
{
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
	%playerPos = GameBase::getPosition(%player);

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item])
	{
		Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
		return false;
	}
	if(!GameBase::getLOSInfo(%player,5)) 
	{
		Client::sendMessage(%client,0,"Deploy position out of range");
		return false;
	}
	%obj = getObjectType($los::object);
	if(%obj != "SimTerrain" && %obj != "InteriorShape") 
	{
		Client::sendMessage(%client,0,"Can only deploy on buildings");
		return false;
	}

	%obj = getObjectType($los::object);
	if(%obj == "SimTerrain") 
	{
		Client::sendMessage(%client,0,"Can only deploy on buildings.");
		return false;
	}

// new start
	%flagpos = $teamFlag[%team].originalPosition;	
	if(Vector::getDistance(%flagpos, %playerpos) > 450)
	{
		Client::sendMessage(%client,0,"You are too far from your flag to deploy the Portable Generator.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
// new end

	%obj = $los::object;
	if(%obj.inmotion == true)	 
	{ 
		Client::sendMessage(%client,0,"Deploy area crappy, cannot deploy.");
		return false;
	}
		
	if(Vector::dot($los::normal,"0 0 1") <= 0.7)
	{
		Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
		return false;
	}
	if(!checkDeployArea(%client,$los::position)) 
	{
		Client::sendMessage(%client,0,"Cannot deploy here");
		return false;
	}
	%obj = newObject("Portable Generator","StaticShape","PortableGenerator",true);
	%obj.deployer = %client;

	%obj.cloakable = true;

	GameBase::setTeam(%obj,%team);
	
	addToSet("MissionCleanup/deployed/power", %obj);
	
	
	//addToSet("MissionCleanup", %obj);
	//addToSet("MissionGroup/Teams/Team" @ %team, %obj);
	
	GameBase::setPosition(%obj,$los::position);
	GameBase::setRotation(%obj,GameBase::getRotation(%player));
	Gamebase::setMapName(%obj,"Portable Generator");
	Client::sendMessage(%client,0,"Portable Generator deployed");
	playSound(SoundPickupBackpack,$los::position);
	messageTeamExcept(%client, 3, "A team Portable Generator has been deployed by "@Client::getName(%client)@".");
	%team = GameBase::getTeam(%player);
	$PortableGeneratedPower[%team] = "true";
	$TeamItemCount[%team @ %item]++;
	return true;
}


function PortableGenerator::onAdd(%this)
{
	$StaticShape::count += 1;
	%team = GameBase::getTeam(%this);
	$PortableGeneratedPower[%team] = "true";
}


function PortableGenerator::onDestroyed(%this)
{
	PortableGenerator::onDisabled(%this);
//	GameBase::generatePower(%this, false);
	StaticShape::objectiveDestroyed(%this);
	%this.cloakable = false;
	%this.nuetron = "";
	calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 3, 0.55, 0.30, 250, 170);
	$TeamItemCount[GameBase::getTeam(%this) @ "PortableGeneratorPack"]--;
	%team = GameBase::getTeam(%this);
	$PortableGeneratedPower[%team] = "false";

	if($PortableSP[%team] == true)
	{
//	messageAll(0, "Portable Generator destroyed and solar panel power is true");
//	DropshipTeamMessage(%team, 1, "A Team Portable Generator has been destroyed. Solar Panel remains.");
	return;
	}
	
 	Ann::UndoPowering(%this,false);	
}

function PortableGenerator::onEnabled(%this)
{ 	
	if($trace) Anni::Echo($ver,"| PortableGenerator::onEnabled ",%this);
	GameBase::setActive(%this,true);
	GameBase::playSequence(%this,0,"power");
//	schedule("Ann::Powering("@%this@",true);",3,%this);	
	schedule("Ann::Powering("@%this@",true);",1,%this);
	GameBase::generatePower(%this, true);
	GameBase::isPowerGenerator(%this);
	%team = GameBase::getTeam(%this);
	$PortableGeneratedPower[%team] = "true";
}

function PortableGenerator::onDisabled(%this)
{
//	%team = GameBase::getTeam(%this);
	GameBase::stopSequence(%this,0);
//	Ann::UndoPowering(%this,false);			
//	GameBase::generatePower(%this, false);
//	$PortableGeneratedPower[%team] = "false";
}

function PortableGenerator::onremove(%this)
{
 	GameBase::generatePower(%this, false);
	%team = GameBase::getTeam(%this);
	$PortableGeneratedPower[%team] = "false";
// 	Ann::UndoPowering(%this,false);
}

function PortableGenerator::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	if($debug)
		 Anni::Echo("PortableGenerator::onDamage "@%this);
	
	if(GameBase::getDamageState(%this) == "Destroyed" || %value <= 0)
	 	return;

	%GunShipDropTeam = GameBase::getTeam(%this);
	%client = Player::getClient(%object);
	%AttackingGSTeam = GameBase::getTeam(%client);
	%name = GameBase::getDataName(%this);
	%player = Client::getOwnedObject(%client);
	%damageLevel = GameBase::getDamageLevel(%this);
	%pos = GameBase::getPosition(%this);

// NNNNEEWWWWW

		if(%AttackingGSTeam == %GunShipDropTeam)
		{
			%dValue = %damageLevel + %value * $Server::TeamDamageScale;
			GameBase::setDamageLevel(%this,%dValue);
		}
		if(%AttackingGSTeam != %GunShipDropTeam)
		{
			%dValue = %damageLevel + (%value/2);	
			GameBase::setDamageLevel(%this,%dValue);
		}

//NNNNEEEWWW
	
	if(GameBase::getDamageState(%this) == "Destroyed") 
	{

		PortableGenerator::onDisabled(%this);
 		GameBase::generatePower(%this, false);
		StaticShape::objectiveDestroyed(%this);
		//gen damage
		calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 3, 0.55, 0.30, 250, 170);			
		GameBase::playSound(%this, SoundrocketExplosion, 0);
		
		%DropTeam = GameBase::getTeam(%this);
		%client = Player::getClient(%object);
		%KillerTeam = GameBase::getTeam(%client);
//		 Anni::Echo("destroy portable gen");	
		if(%KillerTeam == %DropTeam)
		{
			bottomprint(%client, "<jc>You have just LOST <f2>2<f0> POINTS for destroying your teams Portable Generator."); // "@ %name.className	
	
//			 Anni::Echo("MSG: ", %client, " Destroyed a team ",  %name.description);
			DropshipTeamMessage(%KillerTeam, 3, "Team member "@Client::getName(%client)@" has destroyed your teams Portable Generator."); // "@ %name.description);
			%client.score-=2;
			Game::refreshClientScore(%client);		
		}
		else
		{
			if(%this.LastRepairCl !=  %client)
			{	
				bottomprint(%client, "<jc>You have just recieved <f2>2<f0> POINTS for destroying the enemys Portable Generator."); // "@ %name.description);
				%client.score+= 2;
				%client.Credits+= 2;
				Game::refreshClientScore(%client);			
			}	
//			 Anni::Echo("MSG: ", %client, " Destroyed enemy  ",  %name.description);
			DropshipTeamMessage(%DropTeam, 3, "WARNING "@Client::getName(%client)@" has destroyed your teams Portable Generator."); // 's "@ %name.description);
		}					
	}	
}