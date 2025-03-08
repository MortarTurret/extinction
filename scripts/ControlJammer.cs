$InvList[ControlJammerPack] = 1;
$MobileInvList[ControlJammerPack] = 0;
$RemoteInvList[ControlJammerPack] = 0;
AddItem(ControlJammerPack);

$CanAlwaysTeamDestroy[SatBig] = 1;
// ControlJammerPack

ItemImageData ControlJammerPackImage
{
	shapeFile = "sensorjampack"; // magcargo
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData ControlJammerPack
{
	description = "Control Jammer";	
	shapeFile = "sensorjampack";
	className = "Backpack";
  	heading = $InvHead[ihDOb];
	imageType = ControlJammerPackImage;
	shadowDetailMask = 4;
	mass = 2.5;
	elasticity = 0.2;
	price = 1350;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function ControlJammerPack::deployShape(%player,%item)
{
	%client = Player::getClient(%player);
	%team = GameBase::getTeam(%player);

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

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item])
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
	if(GameBase::getLOSInfo(%player,1))
	{
		Client::sendMessage(%client,0,"Deploy position is too close.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
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
	if(!GameBase::testPosition(%player,vector::add($los::position,"10.25 10.25 10.25")))  // 0 0 0.25
	{
		Client::sendMessage(%client,0,"The Control Jammer will not function there.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}

// check for deployed ships.

	if(getNumTeams()-1 == 2)
	{
	%teamnumzero = 0;
	%teamnummone = 1;
	}
	
if(((($DropShipMultipass1[%teamnumzero] == true) || ($DropShipMultipass2[%teamnumzero] == true) || ($DropShipMultipass1[%teamnummone] == true) || ($DropShipMultipass2[%teamnummone] == true))))
{
	%clientId = Player::getClient(%player);
	%playerPos = GameBase::getPosition(%player);
	%strekpos = $DropShipLocation[%teamnumzero @ "GunShip"];
	%strekpos2 = $DropShipLocation[%teamnummone @ "GunShip"];
	if(((Vector::getDistance(%strekpos, %playerpos)) < 50) && (%strekpos != "")) 
	{
		Client::sendMessage(%clientId,0,"You are too close to a Starwolf Gunship landing zone or crash site. ~waccess_denied.wav");
		return false;
	}
	if(((Vector::getDistance(%strekpos2, %playerpos)) < 50) && (%strekpos2 != "")) 
	{
		Client::sendMessage(%clientId,0,"You are too close to a Starwolf Gunship landing zone or crash site. ~waccess_denied.wav");
		return false;
	}
}

	if(!checkInvDeployArea(%client,$los::position)) 
	{
		return false;
	}
	%obj = getObjectType($los::object);
	if(%obj != "SimTerrain") 
	{
		Client::sendMessage(%client,0,"Can only deploy on terrain.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
	%rot = GameBase::getRotation(%player); 
	%turret = newObject("ControlJammer","Turret",ControlJammer,true);

	%turret.cloakable = true;
	addToSet("MissionCleanup/deployed/object", %turret);
	GameBase::setTeam(%turret,GameBase::getTeam(%player));
	GameBase::setPosition(%turret,$los::position);
	GameBase::setRotation(%turret,%rot);
	Gamebase::setMapName(%turret,"Control Jammer Device");
	Client::sendMessage(%client,0,"Control Jammer Device deployed");
//	playSound(SoundPickupBackpack,$los::position);
	playSound(SoundDeploySensor,$los::position);
	$TeamItemCount[GameBase::getTeam(%player) @ "ControlJammerPack"]++;
	%turret.deployer = %client; 	
	Anni::Echo("MSG: ",%client," deployed an Control Jammer Device");
//	messageTeamExcept(%client, 3, "A team Control Jammer has been deployed by "@Client::getName(%client)@".");
	return true;
}


TurretData ControlJammer
{
	className = "Turret";
	shapeFile = "sat_big"; // magcargo
	maxDamage = 10.5;
	maxEnergy = 100;
	sequenceSound[0] = { "deploy", SoundActivateMotionSensor };
   	// range = 100;
   	dopplerVelocity = 0;
	visibleToSensor = true;
	shadowDetailMask = 4;
	supressable = true;
	pinger = false;
	castLOS = true;
	supression = true;
	mapFilter = 2; // 4
	mapIcon = "M_Radar";
	debrisId = flashDebrisMedium;
	shieldShapeName = "shield";
	activationSound = SoundRemoteTurretOn;
	deactivateSound = SoundRemoteTurretOff;
	explosionId = flashExpMedium;
	description = "Command Jammer Device";
	damageSkinData = "objectDamageSkins";
	sfxAmbient = SoundDiscSpin;
};

function ControlJammer::onAdd(%this)
{
	schedule("ControlJammer::deploy(" @ %this @ ");",1,%this);
	GameBase::setRechargeRate(%this,5);
	if(GameBase::getMapName(%this) == "") 
		GameBase::setMapName (%this, "Command Jammer Device");
}

function ControlJammer::deploy(%this)
{
	GameBase::playSequence(%this,1,"deploy");
}

function ControlJammer::onEndSequence(%this,%thread)
{
	GameBase::setActive(%this,true);
}

function ControlJammer::onDisabled(%this)
{
	%this.shieldStrength = 0;
	GameBase::setRechargeRate(%this,0);
	Turret::onDisabled(%this);
}

function ControlJammer::onDestroyed(%this)
{
	%this.shieldStrength = 0;
	GameBase::setRechargeRate(%this,0);
	Turret::onDestroyed(%this);
	%this.OrgTeam = "";
  	$TeamItemCount[GameBase::getTeam(%this) @ "ControlJammerPack"]--;
}

// Override base class just in case.

function ControlJammer::onPower(%this,%power,%generator) 
{
	if (%power) {
		%this.shieldStrength = 0.05;
		GameBase::setRechargeRate(%this,20);
	} else {
		%this.shieldStrength = 0;
		GameBase::setRechargeRate(%this,0);
	}
	GameBase::setActive(%this,%power);
}

function ControlJammer::onEnabled(%this) 
{
	%this.shieldStrength = 0.05;	
	GameBase::setRechargeRate(%this,5);
	GameBase::setActive(%this,true);
	schedule("ControlJammer::checkControlJammer(" @ %this @ ");", 0.1, %this);
}	

function ControlJammer::checkControlJammer(%this)
{	
	if(GameBase::getDamageState(%this) != "Enabled")
		return;
	%Set = newObject("set",SimSet);
	addToSet("MissionCleanup", %Set);
	%Pos = GameBase::getPosition(%this); 
	%Mask = $VehicleObjectType;
	containerBoxFillSet(%Set, %Mask, %Pos, 400, 400, 250,0); // 250 250 150 0
	%num = Group::objectCount(%Set);
	for(%i = 0; %i < %num; %i++)
	{
		%obj = Group::getObject(%Set, %i);
		%type = getObjectType(%obj);
		// Anni::Echo(%type);
		if(%type == Flier)
		{	

			if(GameBase::getTeam(%obj) != GameBase::getTeam(%this))
			{
				
				%name = GameBase::getDataName(%obj);
				if(%name == "ProbeDroid" || %name == "SuicideDroid" || %name == "SurveyDroid" || %name == "OSMissile")
				{
					
					%client = GameBase::getControlClient(%obj);
					%player = Client::getOwnedObject(%client);
					%dam = GameBase::getDamageLevel(%player) + 0.2;
					
					if(%player.invulnerable || $Annihilation::NoPlayerDamage || %player.frozen || $jailed[%player])
					{
						%thisPos = getBoxCenter(%this);
						%offsetZ =((getWord(%pos,2))-(getWord(%thisPos,2)));
						GameBase::activateShield(%this,%vec,%offsetZ);
						return;
					}
					else 
						GameBase::setDamageLevel(%player, %dam);
						
					Player::setDamageFlash(%player,0.75);
					if(Player::isDead(%player)) 
					{
						Player::setAnimation(%player, $PlayerAnim::DieSpin);	
						messageall(0, Client::getName(%client) @ " died from a Control Jammers feedback!"); // -death666 3.18.17
						%client.scoreDeaths++;
						%client.TDeaths++;
						Game::refreshClientScore(%client);
					}					
					else
						client::sendmessage(%client, 1,"You have been damaged by an enemy Control Jammer!~waccess_denied.wav");	// -death666 3.18.17			
					if(%name == "OSMissile")
					{
						%client = GameBase::getControlClient(%obj);				
						if(!Player::isDead(%player))
						{
							GameBase::setDamageLevel(%obj,2); 
						}
					}
					else
						GameBase::setDamageLevel(%obj, 1000);						
				}
			}
		}
	}
	deleteObject(%Set);
//	%dlev = GameBase::getDamageLevel(%this);
//	GameBase::setDamageLevel(%this, %dlev+0.0010);	//0.0015 -Removing control jammer damaging itself to prevent point farming -Death666
	schedule("ControlJammer::checkControlJammer(" @ %this @ ");", 1, %this);
}

function ControlJammerPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player); 
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Control Jammmer: <f2>Detonates enemy <f1>Titan OS Missiles and Builder Droids<f2> that come within range.");	
}

function ControlJammer::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	if($debug)
		 Anni::Echo("ControlJammer::onDamage "@%this);
	
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
		//gen damage
		calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 3, 0.55, 0.30, 250, 170);			
		GameBase::playSound(%this, SoundrocketExplosion, 0);
		
		%DropTeam = GameBase::getTeam(%this);
		%client = Player::getClient(%object);
		%KillerTeam = GameBase::getTeam(%client);
		 Anni::Echo("destroy portable gen");	
		if(%KillerTeam == %DropTeam)
		{
			bottomprint(%client, "<jc>You have just LOST <f2>2<f0> POINTS for destroying your teams Control Jammer."); // "@ %name.className	
	
			 Anni::Echo("MSG: ", %client, " Destroyed a team ",  %name.description);
			DropshipTeamMessage(%KillerTeam, 3, "Team member "@Client::getName(%client)@" has destroyed your teams Control Jammer."); // "@ %name.description);
			%client.score-=2;
			Game::refreshClientScore(%client);		
		}
		else
		{
			if(%this.LastRepairCl !=  %client)
			{	
				bottomprint(%client, "<jc>You have just recieved <f2>2<f0> POINTS for destroying the enemys Control Jammer."); // "@ %name.description);
				%client.score+= 2;
				%client.Credits+= 2;
				Game::refreshClientScore(%client);			
			}	
			 Anni::Echo("MSG: ", %client, " Destroyed enemy  ",  %name.description);
			DropshipTeamMessage(%DropTeam, 3, "WARNING "@Client::getName(%client)@" has destroyed your teams Control Jammer."); // 's "@ %name.description);
		}					
	}	
}