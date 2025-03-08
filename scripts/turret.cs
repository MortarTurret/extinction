


function Turret::verifyTarget(%this,%target)
{
	DebugFun("Turret::verifyTarget", %this, %target);
//	if ( GameBase::getLOSInfo(%this,3000) )
//	{
//		if ( $los::object != %target )
//		{
//			Anni::Echo("Turret "@%this@" trying to shoot "@%target@" through "@$los::object@".");
//			return "False";
//		}
//	}
//	else
//		return "False";

	%client = Player::getClient(%target);
	%type = gamebase::getdataname(%this);

	if(%client.isSpy == "True")
	{
		if ( %type == DeployableCat )
		{
			if (getSimTime() - $Catnap[%this] > 5)
			{
				%thispos = GameBase::getPosition(%this);
				%targetpos = GameBase::getPosition(%target);
				%dist = Vector::getDistance(%thispos, %targetpos);
				if(%dist < 50 && %client.isSpy && %this.CatTeam == gamebase::getteam(%target)) 
				{		
					//Anni::Echo("Distance to target is " @ %dist);
					$Catnap[%this] = getSimTime();
					%team =gamebase::getteam(%this);
					%client = Player::getClient(%target);
					%playerId = Player::getClient(%target);
				if ( %playerId.haschammessage) //adding a delay to kill the chat spam by pussycats -death666
				{
					TeamMessages(3, %this.CatTeam, Gamebase::getMapName(%this)@" has detected chameleon " @ Client::getName(%client)@"~wC_BuySell.wav");
					%playerId.haschammessage = false;
					schedule(%playerId@".haschammessage = true;",5.0,%playerId);
				}	

					%chance = floor(150*(1/%dist));
					if(%chance> 100)
					%chance = 100;
				
					if(floor(getrandom() * 100) < %chance)
					{
						%target.ChamCollapse = true; 
					//	Client::sendMessage(%client,0,"WARNING! Imminent Chameleon field collapse.~waccess_denied.wav"); // removed this line and added the next -death666
						Client::sendMessage(%client,1,"WARNING! Chameleon failure due to pussycat interference!~waccess_denied.wav");
						schedule("Player::trigger("@%target@", $BackpackSlot, false);",1.0,%target);
					}	
					else
						ChameleonPack::Buffer(%target);
				}
			}
		}
		return "False";
	}
	else 
	{
		if ( %type == DeployableCat )
			return "False";

		if(%type == RocketTurret)
		{
			if(GameBase::virtual(%target, "getHeatFactor") >= 0.5)
				return "True";
			else
				return "False";
		}


			return "True";	
		
	}
}


//----------------------------------------------------------------------------
// TURRET DYNAMIC DATA

TurretData PlasmaTurret
{
	maxDamage = 1.75;
	maxEnergy = 300;
	minGunEnergy = 50;
	maxGunEnergy = 6;
	reloadDelay = 0.75;
	fireSound = SoundPlasmaTurretFire;
	activationSound = SoundPlasmaTurretOn;
	deactivateSound = SoundPlasmaTurretOff;
	whirSound = SoundPlasmaTurretTurn;
	range = 125;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	visibleToSensor = true;
	debrisId = defaultDebrisMedium;
	className = "Turret";
	shapeFile = "hellfiregun";
	shieldShapeName = "shield_medium";
	speed = 2.5;
	speedModifier = 2.0;
	projectileType = FusionBolt;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 8;
	explosionId = LargeShockwave;
	description = "Plasma Turret";
	//isSustained = true;
	sfxAmbient = SoundMortarTurretTurnLp;
};
																						 
TurretData ELFTurret		
{			 
	maxDamage = 1.25;
	maxEnergy = 175;
	minGunEnergy = 35;
	maxGunEnergy = 5;
	range = 50;
	visibleToSensor = true;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = defaultDebrisMedium;
	className = "Turret"; // ELF Turret
	shapeFile = "chainturret";
	shieldShapeName = "shield";
	speed = 5.0;
	speedModifier = 1.5;
	projectileType = turretCharge;
	reloadDelay = 0.3;
	explosionId = LargeShockwave;
	description = "ELF Turret";
	fireSound = SoundGeneratorPower;
	activationSound = SoundChainTurretOn;
	deactivateSound = SoundChainTurretOff;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 8;
	isSustained = true;
	firingTimeMS = 750;
	energyRate = 30.0;
	sfxAmbient = SoundMissileTurretTurnLp;
};

TurretData RocketTurret
{
	maxDamage = 1.5;
	maxEnergy = 175;
	minGunEnergy = 50;
	maxGunEnergy = 50;
	range = 175;
	gunRange = 350;
	visibleToSensor = true;
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = defaultDebrisLarge;
	className = "Turret";
	shapeFile = "missileturret";
	shieldShapeName = "shield_medium";
	speed = 4.0;
	speedModifier = 1.5;
	projectileType = TurretMissile;
	reloadDelay = 1.5;
	fireSound = SoundMissileTurretFire;
	activationSound = SoundMissileTurretOn;
	deactivateSound = SoundMissileTurretOff;
	//whirSound = SoundMissileTurretTurn;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 8;
	targetableFovRatio = 0.5;
	explosionId = LargeShockwave;
	description = "Rocket Turret";
	sfxAmbient = SoundElevatorBlocked;
};

function RocketTurret::onPower(%this,%power,%generator)
{
	DebugFun("RocketTurret::onPower", %this, %power, %generator);
	if(%power) 
	{
		%this.shieldStrength = 0.03;
		GameBase::setRechargeRate(%this,14);
	}
	else 
	{
		%this.shieldStrength = 0;
		GameBase::setRechargeRate(%this,0);
		Turret::checkOperator(%this);
	}
	GameBase::setActive(%this,%power);
}


TurretData MortarTurret
{
	maxDamage = 1.5;
	maxEnergy = 45;
	minGunEnergy = 45;
	maxGunEnergy = 15;
	reloadDelay = 2;
	fireSound = SoundMortarTurretFire;
	activationSound = SoundMortarTurretOn;
	deactivateSound = SoundMortarTurretOff;
	whirSound = SoundMortarTurretTurn;
	range = 150; // 0
	dopplerVelocity = 0;
	castLOS = true;
	supression = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	visibleToSensor = true;
	debrisId = defaultDebrisMedium;
	className = "Turret";
	shapeFile = "mortar_turret";
	shieldShapeName = "shield_medium";
	speed = 2.0;
	speedModifier = 2.0;
	projectileType = MortarTurretShell;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 8;
	explosionId = LargeShockwave;
	description = "Mortar Turret";
	sfxAmbient = SoundMortarIdle;
};
																						 
//--------------------------------------------

TurretData IndoorTurret
{
	className = "Turret";
	shapeFile = "indoorgun";
	projectileType = MiniFusionBolt;
	maxDamage = 2.5;
	maxEnergy = 60;
	minGunEnergy = 20;
	maxGunEnergy = 6;
	reloadDelay = 0.4;
	speed = 5.0;
	speedModifier = 1.0;
	range = 25;
	visibleToSensor = true;
	dopplerVelocity = 2;
	castLOS = true;
	supression = false;
	supressable = false;
	pinger = false;
	mapFilter = 2;
	mapIcon = "M_turret";
	debrisId = defaultDebrisMedium;
	shieldShapeName = "shield";
	fireSound = SoundEnergyTurretFire;
	activationSound = SoundEnergyTurretOn;
	deactivateSound = SoundEnergyTurretOff;
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 8;
	explosionId = debrisExpMedium;
	description = "Indoor Turret";
	sfxAmbient = SoundSnowLp;

};

//---------------------------------------------------

$TurretStatusIdle = 0;
$TurretStatusRecalibrating = 1;
$TurretStatusAlert = 2;

function Turret::onAdd(%this)
{
	DebugFun("Turret::onAdd", %this);
	if(GameBase::getMapName(%this) == "") 
		GameBase::setMapName (%this, "Turret");
}
 
function Turret::onRemove(%this)
{
	DebugFun("Turret::onRemove", %this);
}


function Turret::onActivate(%this)
{
	DebugFun("Turret::onActivate", %this);
	GameBase::playSequence(%this,0,power);
	%this.Status = $TurretStatusIdle;
}

function Turret::onDeactivate(%this)
{
	DebugFun("Turret::onDeactivate", %this);
	GameBase::stopSequence(%this,0);
	Turret::checkOperator(%this);
}

function Turret::onSetTeam(%this,%oldTeam)
{
	if(!%this.ControlledTeamChange)
		%this.OrgTeam = GameBase::getTeam(%this);
	else
		%this.ControlledTeamChange = "";
	if(GameBase::getTeam(%this) != Client::getTeam(GameBase::getControlClient(%this))) 
		Turret::checkOperator(%this);
}

function Turret::checkOperator(%this)
{
	%cl = GameBase::getControlClient(%this);
	if(%cl != -1) 
	{
		%pl = Client::getOwnedObject(%cl);
		if(%player.ManualCommandTag)
		{	%player.ManualCommandTag = False;
			%player.CommandTag = "";
		}
		Player::setMountObject(%pl, -1,0);
		Client::setControlObject(%cl, %pl);
	}
	//Client::setGuiMode(%cl,2);
}

function Turret::onPower(%this,%power,%generator)
{
	if(%power) 
	{
		%this.shieldStrength = 0.03;
		GameBase::setRechargeRate(%this,10);
	}
	else 
	{
		%this.shieldStrength = 0;
		GameBase::setRechargeRate(%this,0);
		Turret::checkOperator(%this);
	}
	GameBase::setActive(%this,%power);
}

function Turret::onEnabled(%this)
{
	if(GameBase::isPowered(%this)) 
	{
		%this.shieldStrength = 0.03;
		GameBase::setRechargeRate(%this,10);
		GameBase::setActive(%this,true);
	}
}

function Turret::onDisabled(%this)
{
	%this.shieldStrength = 0;
	GameBase::setRechargeRate(%this,0);
	Turret::onDeactivate(%this);
}

function Turret::onDestroyed(%this)
{
	%this.NeedsNewOwner = "";	
	StaticShape::objectiveDestroyed(%this);
	%this.shieldStrength = 0;
	%this.cloakable = "";	
	GameBase::setRechargeRate(%this,0);
	Turret::onDeactivate(%this);
	Turret::objectiveDestroyed(%this);
	if(!$NoCalcDamage)
		calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 9, 3, 0.40, 0.1, 200, 100);
}

function Turret::objectiveDestroyed(%this) // ???
{
}

function Turret::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	if($debug::Damage)
	{
		Anni::Echo("Turret::onDamage("@%this@", "@%type@", "@%value@", "@%pos@", "@%vec@", "@%mom@", "@%object@" )");
	}	
	if(%value <= 0) return;	
	if(%this.objectiveLine)
		%this.lastDamageTeam = GameBase::getTeam(%object);
//		%this.lastDamageType = %type; //new -death666
	%TDS= 1;
	if(GameBase::getTeam(%this) == GameBase::getTeam(%object)) 
	{
		%name = GameBase::getDataName(%this);
		if(!$CanAlwaysTeamDestroy[%name])
			%TDS = $Server::TeamDamageScale;
	}
	else if(%type == $ShockDamageType)	
		GameBase::setEnergy(%this,0);
		
	StaticShape::shieldDamage(%this,%type,%value * %TDS,%pos,%vec,%mom,%object);
}

function Turret::onManualControl(%this, %player) 
{	 
	// When a player jumps in
	%client = Player::getClient(%player);

	%player.ManualCommandTag = True;
	%player.CommandTag = 1;
	Client::takeControl(%client, %this);

	 // Move the player onto the turret if appropriate
	%name = GameBase::getDataName(%this);
	if($EmbedController[%name])
		GameBase::SetPosition(%player, GameBase::GetPosition(%this));
}

function Turret::onControl (%this, %object)
{
	%client = Player::getClient(%object);
	Client::sendMessage(%client,0,"Controlling turret " @ %this);
}

function Turret::onDismount (%this, %object) 
{	 
	// When a player ceases to control the turret (either jump or stop control)
	%client = %object;
	Turret::checkOperator(%this);
	%this.Status = $TurretStatusRecalibrating;
	schedule("Turret::onInternalDiagnostics(" @ %this @ ", " @ %object @ ");", 10, %this);
}

function Turret::Jump(%this, %mom) 
{	
	%cl = GameBase::getControlClient(%this);
	Turret::onDismount(%this, %cl);
}

function Turret::onInternalDiagnostics(%this, %object)
{	
	 // Verify that we're on our original team
	if(GameBase::getTeam(%this) != %this.OrgTeam)
	{	%this.Status = $TurretStatusAlert;
		GameBase::setTeam(%this, %this.OrgTeam);
		//  Spend additional time in alert if we had to correct our team
		schedule("Turret::onReturnToIdle(" @ %this @ ");", 10, %this);
	}
	else 
		Turret::onReturnToIdle(%this);
}

function Turret::onReturnToIdle(%this)
{	
	%this.Status = $TurretStatusIdle;
}

function Turret::onCollision (%this, %object)
{	
	if($debug) 
		event::collision(%this,%object);

	if(%this.cloaked > 0 && getObjectType(%object) == "Player"){
		GameBase::startFadein(%this);
		%this.cloaked = "";
		}	
	if(%this.CommandTag) return;

	%name = GameBase::getDataName(%this);
	if(getObjectType(%object) != "Player" || !$CanControl[%name]) 
		return;

	if (Player::isAIControlled(%object)) //Is this a bot?
		return;

	 // Still standing in the turret?
	if(GameBase::GetPosition(%this) == GameBase::GetPosition(%object)) 
		return;

	 // Verify the status of the turret
	if(Player::getMountedItem(%object,$BackpackSlot) == Laptop)
	{	if(%this.Status == $TurretStatusRecalibrating)
		{	 
			Laptop::Error(%client, "Turret is recalibrating.");
			return;
		}
		else if(%this.Status == $TurretStatusAlert)
		{	
			Laptop::Error(%client, "Turret is on alert.");
			return;
		}
	}
	else
	{	if(%this.Status != $TurretStatusIdle)
		{	 
			Client::sendMessage(%client,0,"Turret is recharging.");
			return;
		}
	}
	if(GameBase::getTeam(%object) != GameBase::getTeam(%this))
	{	//  If the player has a laptop, then they can control anything
		if(Player::getMountedItem(%object,$BackpackSlot) != Laptop)
		{	
			Client::sendMessage(%client,0,"--ACCESS DENIED-- Wrong Team.~waccess_denied.wav");
			return;
		}

		//  Team change the turret (internal diagnostics will change it back once dismounted)
		%this.OrgTeam = GameBase::getTeam(%this);
		%this.ControlledTeamchange = True;
		GameBase::setTeam(%this, GameBase::getTeam(%client));

		Turret::onManualControl(%this, %object);
		Laptop::Output(%client, "Successful patch into enemy turret.");
	}
	else // Normal non-laptop connection to team turret
	{	
		Turret::onManualControl(%this, %object);
		Client::sendMessage(%client, 0, "Manually controlling turret");
	}
}

function Turret::onAttemptControl(%this, %client)
{	
	//  If you're controlling via a Command station, this is the first entry into
	//  Turret.  If you're running through collision, this is called above.
	%player = Client::getOwnedObject(%client);
	%name = GameBase::getDataName(%this);

	//  Run some final checks
	if(!%player.CommandTag && !$CanAlwaysControl[%name])
	{	if(Player::getMountedItem(%player, $BackpackSlot) != Laptop) 
		{	
			Client::SendMessage(%client, 0, "Need laptop or Command Station.");
			return;
		}
	}

	//  Actually control the turret
	Client::setControlObject(%client, %this);
	Client::setGuiMode(%client, $GuiModePlay);
}

$TurretLocGroundOnly = 0;
$TurretLocAnywhere = 1;

function CountTurrets(%set, %num, %team) 
{	
	%count = 0;
	for(%i=0; %i < %num; %i++)
	{	
		%obj = GameBase::getDataName(Group::getObject(%set,%i));
		if(%obj.ClassName == Turret && %obj.Range > 0 && GameBase::getTeam(%obj) == %team) 
			%count++;
	}
	return %count;
}

function Turret::deployShape(%player, %name, %shape, %item, %validloc)
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

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]  && !$build) 
	{ 
		Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return;
	}

	if(!GameBase::getLOSInfo(%player, 5) && (Player::getItemCount(%player, iarmorBuilder) != 1 || !GameBase::getLOSInfo(%player, 4.5)))
	{ 
		if(!$build)
			Client::sendMessage(%client,0,"Deploy position out of range.");
//		GameBase::playSound(%player, SoundPackUse, 0);
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return;
	}

	if(getNumTeams()-1 == 2)
	{
		%playerTeam = Client::getTeam(%client);
		if(%playerTeam == 0)
			%enemyTeam = 1;
		else if(%playerTeam == 1)
			%enemyTeam = 0;
		if(((Vector::getDistance($teamFlag[%enemyTeam].originalPosition, $los::position)) < ($FlagDistance * 0.4)) && ($FlagDistance != 0)) 
		{
			Client::sendMessage(%client,0,"You are too close to the enemy flag~wC_BuySell.wav");
			return false;
		}
	}
	
	%obj = getObjectType($los::object);
	if(%obj != "SimTerrain" && %obj != "InteriorShape" && GameBase::getDataName($los::object) != "DeployablePlatform")	 
	{ 
		Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return;
	}

	if((GameBase::getTeam($los::object) != GameBase::getTeam(%player)) && (%obj != "SimTerrain") && (GameBase::getTeam($los::object) != -1)) 
	{
		Client::sendMessage(%client,0,"Cannot deploy on enemy base");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}

	if(!GameBase::testPosition(%player, vector::add($los::position, "0 0 1")) && %validloc == $TurretLocGroundOnly)
	{ 
		Client::sendMessage(%client,0,"Turret does not fit there");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return;
	}


	%obj = $los::object;
	//Anni::Echo("$los::object ",%obj);

	if(%obj.inmotion == true && gamebase::getteam($los::object) == Client::getTeam(%client))	 
	{ 
		Client::sendMessage(%client,0,"Deploy area crappy, cannot deploy.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return;
	}
	
	
	//  Check long range count (not applicable to manual turrets)
	if(%shape.Range > 0)
	{	
		%set = newObject("set",SimSet);
		%box = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMaxLength,$TurretBoxMaxWidth,$TurretBoxMaxHeight,0);
		%num = CountTurrets(%set, %box ,%playerTeam);
		deleteObject(%set);
		if(%num > $MaxNumTurretsInBox) 
		{	
			if(!$build)
				Client::sendMessage(%client,0,"Interference from other remote turrets in the area");
//			GameBase::playSound(%player, mine_act, 0);
		        Client::sendMessage(%client,0,"~wC_BuySell.wav");
			return;
		}
	}

	// Check short range count
	%set = newObject("set",SimSet);
	%num = containerBoxFillSet(%set, $StaticObjectType, $los::position, $TurretBoxMinLength, $TurretBoxMinWidth, $TurretBoxMinHeight, 0);
	%num = CountObjects(%set, %shape, %num);
	deleteObject(%set);
	if(%num) 
	{	Client::sendMessage(%client,0,"Frequency Overload - Too close to other remote turrets");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return;
	}

	// Check slope of the floor
	if(%validloc == $TurretLocGroundOnly)
	{	
		if(Vector::dot($los::normal,"0 0 1") <= 0.7) 
		{	
			Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
			Client::sendMessage(%client,0,"~wC_BuySell.wav");
			return;
		}
		%rot = GameBase::getRotation(%player);
	}
	else if(%validloc == $TurretLocAnywhere)
	{	
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
	}
	else
		return;

	// Make sure this isn't colliding with other objects
	if(!checkInvDeployArea(%client,$los::position)) 
	{	
		return;
	}

	// Passed validations, deploy
	%obj = newObject("hellfiregun", "Turret", %shape, true);
	if(%player.repackEnergy != "")
	{
    GameBase::setDamageLevel(%obj, %player.repackDamage);
    GameBase::setEnergy(%obj, %player.repackEnergy);
    %player.repackDamage = "";
    %player.repackEnergy = "";
	}
	addToSet("MissionCleanup/deployed/turret", %obj);
	%obj.cloakable = true;	//for base cloaker
	$turret::count++;
	
	GameBase::setTeam(%obj, GameBase::getTeam(%player));
	GameBase::setPosition(%obj, $los::position);
	GameBase::setRotation(%obj, %rot);
	Gamebase::setMapName(%obj, %name);
	Client::sendMessage(%client, 0, %item.description @ " deployed.");
	if(!$build)
		Anni::Echo("MSG: "@Client::getName(%client)@", "@%client@" deployed a " @ %item.description@ " ob# "@%obj);
	playSound(SoundPickupBackpack,$los::position);
	$TeamItemCount[GameBase::getTeam(%player) @ %item]++;
	//reportDeploy(%obj, %client);
	
	// Remote turrets - kill points to player that deploy them
	client::setOwnedObject(%client, %obj);
	%obj.deployer = %client; 	
	Client::setOwnedObject(%client, %player);
	$TurretList[%obj] = %client;	
	
	if($debug)
		Anni::Echo(%client @ " deployed a " @ %item.description @" object # "@%obj);
	
	return true;
}

function Turret::DisableClients(%client)
{	
	%simset = nameToID("MissionCleanup/deployed/turret");
	for(%i = 0; (%o = Group::getObject(%simset, %i)) != -1 && %i < 2000; %i++)
	{
		if($TurretList[%o] == %client)
		{
			if(Client::getTeam(%client) != Gamebase::getTeam(%o))
			{
				if(!%o.NeedsNewOwner)
				{
					%count++;
					%data = GameBase::getDataName(%o);
					Anni::Echo("!! Disabling "@%o@" "@%data);
					%destroy = %data.maxDamage;
					%disable = (%destroy/3)*2;
					GameBase::setDamageLevel(%o, %disable);
					%o.NeedsNewOwner = true;
					Gamebase::setMapName(%o,"Repair me please.");
					$TurretList[%o] = "";
				}
			}
		}		
	}	
	if(%count > 1)
		messageall(1,"Disabled "@ %count @" of "@ Client::getName(%client) @"'s turrets.");
	else if(%count > 0)
		messageall(1,"Disabled "@ Client::getName(%client) @"'s turret.");
}
