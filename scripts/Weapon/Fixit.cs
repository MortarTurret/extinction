$InvList[Fixit] = 1;
$MobileInvList[Fixit] = 1;
$RemoteInvList[Fixit] = 1;

$AutoUse[Fixit] = False;
$FixitSlot = 4;

addWeapon(Fixit);
addWeapon(Fixit2);

$FixitSlotA=4;

RepairEffectData SuperRepairBolt
{
	bitmapName = "LightningNew.bmp"; 
	boltLength = 25.5; //BR Setting = 277.5
	segmentDivisions = 4;
	beamWidth = 0.125;
	updateTime = 450;
	skipPercent = 0.6;
	displaceBias = 0.15;
	lightRange = 3.0;
	lightColor = { 0.85, 0.25, 0.25 };
};

RepairEffectData RePackBolt
{
	bitmapName = "warp.bmp"; 
	boltLength = 25.5; //BR Setting = 277.5
	segmentDivisions = 8;
	beamWidth = 0.15;
	updateTime = 450;
	skipPercent = 0.6;
	displaceBias = 0.15;
	lightRange = 3.0;
	lightColor = { 0.25, 0.25, 0.85 };
};

function RePackBolt::onAcquire(%this, %player, %target)
{
	%client = Player::getClient(%player);
	%data = GameBase::getDataName(%target);
	%targetlocation = GameBase::getPosition(%target);
	
	if($ArmorName[Player::getArmor(%player)] != iarmorBuilder)
	{
		Client::sendMessage(Player::getClient(%player), 1, "Only builders can use a builder tool.");
		Admin::BlowUp(%client);
		return;	
	}
	
	//	List of all repackable shapes and their corresponding item names.
	//-----------------------Turrets-----------------------------------------
	%item[DeployableFusionTurret]		=	FusionTurretPack;
	%item[DeployableTurret]				=	TurretPack;
	%item[DeployableLaserTurret]		=	LaserTurretPack;
	%item[deployableVortexTurret]		=	vortexTurretPack;
	%item[DeployableRocket]				=	RocketPack;
	%item[IrradiationTurret]			=	IrradiationTurretPack;
	%item[DeployableMortar]				=	MortarTurretPack;
	%item[DeployableNuclearTurret]		=	NuclearTurretPack;
	%item[DeployablePlasmaTurret]		=	PlasmaTurretPack;
	%item[DeployableShockTurret]		=	ShockTurretPack;
	%item[FlameTurret]					=	FlameTurretPack;
	%item[Canister]						=	FlameTurretPack;
	%item[NeuroBase]					=	NeuroTurretPack;
	%item[NeuroCannon]					=	NeuroTurretPack;
	%item[NeuroTurret]					=	NeuroTurretPack;
	//-----------------------SENSORS-----------------------------------------
	%item[DeployableMotionSensor]		=	MotionSensorPack;
	%item[DeployablePulseSensor]		=	PulseSensorPack;
	%item[DeployableSensorJammer]		=	DeployableSensorJammerPack;
	%item[DeployableCat]				=	CatPack;
	%item[PCDsensor]					=	CatPack;
	%item[CameraTurret]					=	CameraPack;
	//-----------------------Objects-----------------------------------------
	%item[DeployableAmmoStation]		=	DeployableAmmoPack;
	%item[DeployableComStation]			=	DeployableComPack;
	%item[MobileInventory]				=	MobileInventoryPack;
	%item[InvTurret]					=	MobileInventoryPack;
	%item[DeployableInvStation]			=	DeployableInvPack;
//	%item[PortableSolar]				=	PortableSolarPack; //lots of lag and errors when trying to repack
//	%item[PortableGenerator]				=	PortableGeneratorPack; //lots of lag and errors when trying to repack
	%item[JumpPad]						=	JumpPadPack;
	%item[ControlJammer]				=	ControlJammerPack; //no errors works fine but if solar out this should be out, not much reason to move these 2 anyway
	%item[DeployableTeleport]			=	TeleportPack;
	%item[Springboard]					=	SpringPack;
	%item[AcceleratorDevice]			=	AcceleratorDevicePack;
	%item[AcceleratorDeviceFS]			=	AcceleratorDevicePack;
	%item[AcceleratorDeviceSideOne]		=	AcceleratorDevicePack;
	%item[AcceleratorDeviceSideTwo]		=	AcceleratorDevicePack;
	//-----------------------Barriers-----------------------------------------
	%item[DeployableForceField]			=	ForceFieldPack;
	%item[ForceFieldDoorShape]			=	ForceFieldDoorPack;
	%item[LargeForceField]				=	LargeForceFieldPack;
	%item[LargeForceFieldDoorShape]		=	LargeForceFieldDoorPack;
	%item[BlastWall]					=	BlastWallPack;
	%item[BigCrate]						=	BigCratePack;
	%item[DeployablePlasmaFloor]		=	PlasmafloorPack;
	%item[DeployablePlatform]			=	PlatformPack;

	if(%item[%data] == "" || %target.deployer != %client)
	{	
		if(%target == %player)
			Client::sendMessage(%client, 0, "Nothing repackable in range.");
		else if(%target.deployer != %client)
			Client::sendMessage(%client, 0, "You cannot repack what you didn't unpack.");
		else
			Client::sendMessage(%client, 0, "Nonrepackable target.");
		Player::trigger(%player, $WeaponSlot, false);
		return;
	}

	%clientId = Player::getClient(%player);

if(%clientId.hasrphallpass == "false")
{
		Client::sendMessage(%clientId,0,"Please Wait A Second To Re-Pack again. ~waccess_denied.wav");
}

if(%clientId.hasrphallpass)
{

	if(%data == AcceleratorDevice)
		%target = sprintf("%2); %1(%3); %1(%4); %1(%5", "deleteObject", %target, %target.objDeviceFS, %target.objSide1, %target.objSide2);
	else if(%data == AcceleratorDeviceFS)
		%target = sprintf("%2); %1(%3); %1(%4); %1(%5", "deleteObject",  %target, %target.objDevice, %target.objSide1, %target.objSide2);
	else if(%data == AcceleratorDeviceSideOne)
		%target = sprintf("%2); %1(%3); %1(%4); %1(%5", "deleteObject",  %target, %target.objDevice, %target.objDeviceFS, %target.objSide2);
	else if(%data == AcceleratorDeviceSideTwo)
		%target = sprintf("%2); %1(%3); %1(%4); %1(%5", "deleteObject",  %target, %target.objDevice, %target.objDeviceFS, %target.objSide1);
	else if(%data == MobileInventory)
		%target = sprintf("%2); %1(%3); %1(%4); %1(%5", "deleteObject",  %target, %target.lTurret, %target.rTurret, %target.trigger);
	else if(%data == InvTurret)
		%target = sprintf("%2); %1(%3); %1(%4); %1(%5", "deleteObject",  %target.Owner, %target.Owner.lTurret, %target.Owner.rTurret, %target.Owner.trigger);
	else if(%data == DeployableCat)
		%target = %target @"); deleteObject("@ %target.pcd;
	else if(%data == PCDsensor)
		%target = %target @"); deleteObject("@ %target.mSensor;
	else if(%data == DeployableTeleport)
		%target = %target @"); deleteObject("@ %target.beam1;
	else if(%data == FlameTurret)
		%target = %target @"); deleteObject("@ %target.cyl;
	else if(%data == Canister)
		%target = %target @"); deleteObject("@ %target.turret;
	else if(%data == NeuroBase)
		%target = sprintf("%2); %1(%3); %1(%4", "deleteObject",  %target, %target.cannon, %target.turret);
	else if(%data == NeuroCannon)
		%target = sprintf("%2); %1(%3); %1(%4", "deleteObject",  %target.base, %target, %target.turret);
	else if(%data == NeuroTurret)
		%target = sprintf("%2); %1(%3); %1(%4", "deleteObject",  %target.base, %target.cannon, %target);
	
	if(%target.OrgTeam == "")	
		%target.OrgTeam = GameBase::getTeam(%target);
		
	$TeamItemCount[%target.OrgTeam @ (%item[%data])]--;
	%target.OrgTeam = "";
	%target.repacked = true;
	
	Client::sendMessage(%client,0,"You repacked the " @ %item[%data].description @"~wflagflap.wav");
	%clientId.hasrphallpass = false;
	schedule(%clientId@".hasrphallpass= true;",1.3,%clientId);
	schedule("deleteObject("@ %target @"); Player::trigger(" @ %player @ ", $WeaponSlot, false);", 0.2); // dont want trigger to stick -Ghost
	if(Player::getMountedItem(%player, $BackpackSlot) == -1) //make sure they have backpack space -Ghost
    {
		if(%data == DeployableInvStation) 
		{
		%player.repackInvEnergy = %target.Energy;
		}
        %player.repackDamage = GameBase::getDamageLevel(%target);
        %player.repackEnergy = GameBase::getEnergy(%target);
        Player::incItemCount(%client, %item[%data]);
        Player::mountItem(%client, %item[%data], $BackpackSlot);
    }
    else
    {
        %pack = newObject("", Item, %item[%data], 1, false);
		if(%data == DeployableInvStation) 
		{
		%player.repackInvEnergy = %target.Energy;
		}
        %pack.repackDamage = GameBase::getDamageLevel(%target);
        %pack.repackEnergy = GameBase::getEnergy(%target);
        GameBase::throw(%pack, %target, 0, false); 
	addToSet("MissionCleanup", %pack);
//	schedule("deleteObject(" @ %pack @ ");",10.5, %pack);
	schedule("Item::Pop(" @ %pack @ ");", 10.0, %pack); // $ItemPopTime
    }

} // new

}

function SuperRepairBolt::onRelease(%this, %player)
{
	%object = %player.repairTarget;
	if(%object != -1) 
	{
		%client = Player::getClient(%player);
		if(%object == %player) 
		{
			Client::sendMessage(%client,0,"AutoRepair Off");
		}
		else 
		{
			if(GameBase::getDamageLevel(%object) == 0) 
			{
				Client::sendMessage(%client,0,"Repair Done");
				RepairRewards(%player);
				RepairRewardsGenerators(%player);
				%player.repairingCI = false; // -death666
			}
			else 
			{
				Client::sendMessage(%client,0,"Repair Stopped");
			}
		}
		%rate = GameBase::getAutoRepairRate(%object) - %player.repairRate;
		if(%rate < 0)
			%rate = 0;
		
		GameBase::setAutoRepairRate(%object,%rate);
		%player.repairTarget = -1;
		%player.repairRate = 0;
	}
}


function SuperRepairBolt::onAcquire(%this, %player, %target)
{
	if($debug)
		Anni::Echo("SuperRepairBolt::onAcquire, bolt="@%this@", player="@%player@", target="@ %target);	
	
		%client = Player::getClient(%player);
		
	// death666
	if($ArmorName[Player::getArmor(%player)] != iarmorBuilder)
	{
		Client::sendMessage(Player::getClient(%player), 1, "Only builders can use a builder tool.");
		Admin::BlowUp(%client);
		return;	
	}	

	checkRepairingCriticalPowers(%player, %target);	
	
	%player.fixingDisabled = false;
	if(%target == %player) 
	{
		%player.repairTarget = -1;
		if(GameBase::getDamageLevel(%player) != 0) 
		{
			%player.repairRate = 0.125;	//0.25
			%player.repairTarget = %player;
			Client::sendMessage(%client, 0, "AutoRepair On");
		}
		else 
		{
			Client::sendMessage(%client,0,"Nothing in range");
			Player::trigger(%player, $WeaponSlot, false);
			return;
		}

	}
	else 
	{
		%player.repairTarget = %target;
		%player.repairRate = 0.50;
		if(getObjectType(%player.repairTarget) == "Player") 
		{
			%rclient = Player::getClient(%player.repairTarget);
			%name = Client::getName(%rclient);
		}

		else if(fixable(%player,%target))
		{
			%target.LastRepairCl =  %client; 
			%name = GameBase::getMapName(%target);
			checkRepairingCriticalInfrastructure(%player, %target); // -death666
//			checkRepairingCriticalPowers(%player, %target);
			if(%name == "") 
			{
				%name = (GameBase::getDataName(%player.repairTarget)).description;
			}
			if(GameBase::getTeam(%player)!= GameBase::getTeam(%target))
				%name = "enemy "@%name;
		}
		else 
		{
			%player.fixingDisabled = true;
			Player::trigger(%player,$WeaponSlot,false);
			return;
		}
		if(GameBase::getDamageLevel(%player.repairTarget) == 0) 
		{			
			Client::sendMessage(%client,0,%name @ " is not damaged");
			Player::trigger(%player,$WeaponSlot,false);
			%player.repairTarget = -1;
			return;
		}
		if(getObjectType(%player.repairTarget) == "Player") 
		{
			Client::sendMessage(%rclient,0,"Being repaired by " @ Client::getName(%client));
		}
		
		//reset ownership 
		if(%target.NeedsNewOwner == true && Gamebase::getTeam(%target) == client::GetTeam(%client))
		{
			%name = GameBase::getDataName(%target).description;
			Client::sendMessage(%client,0,%name@": WOOHOO! I'm YOURS Baybeee!");	
			GameBase::playSound(%client, FWooho, 0);
			
			// Remote turrets - kill points to player that fix them
			client::setOwnedObject(%client, %target);
			%target.deployer = %client; 	
			Client::setOwnedObject(%client, %player);
			$TurretList[%target] = %client;	
			
			Gamebase::setMapName(%target, %name @"(" @ Client::getName(%client) @ ")");
			%target.NeedsNewOwner = false;
		}
		else		
			Client::sendMessage(%client,0,"Repairing " @ %name);

		%object = %player.repairTarget;	// -death666 3.30.17
		%name = GameBase::getDataName(%object);	// -death666 3.30.17
		%class = %name.className; // -death666 3.30.17

		if(%class == Generator) // -death666 3.30.17
		{
			%player.repairRate = 1.0; // was 5.50
   			 if(GameBase::getTeam(%player) != GameBase::getTeam(%target))
			 {
			 Client::sendMessage(%client,0,"Cannot repair enemy equipment");
			 Player::trigger(%player, $WeaponSlot, false);
       			 return;
			 }
		}
	}
	
	%rate = GameBase::getAutoRepairRate(%player.repairTarget) + %player.repairRate;
	GameBase::setAutoRepairRate(%player.repairTarget,%rate);
}

function SuperRepairBolt::onRelease(%this, %player)
{
	%object = %player.repairTarget;
	if(%object != -1) 
	{
		%client = Player::getClient(%player);
		if(%object == %player) 
		{
			Client::sendMessage(%client,0,"AutoRepair Off");
		}
		else 
		{
			if(GameBase::getDamageLevel(%object) == 0) 
			{
				Client::sendMessage(%client,0,"Repair Done");
				RepairRewards(%player);
				RepairRewardsGenerators(%player);
				%player.repairingCI = false; // -death666
			}
			else 
			{
				Client::sendMessage(%client,0,"Repair Stopped");
			}
		}
		%rate = GameBase::getAutoRepairRate(%object) - %player.repairRate;
		if(%rate < 0)
			%rate = 0;
		
		GameBase::setAutoRepairRate(%object,%rate);
		%player.repairTarget = -1;
		%player.repairRate = 0;
	}
}

function SuperRepairBolt::checkDone(%this, %player)
{
	if(Player::isTriggered(%player,$WeaponSlot) && Player::getMountedItem(%player,$WeaponSlot) == Fixit && %player.repairTarget != -1) 
	{
		%object = %player.repairTarget;
		if(%object == %player) 
		{
			if(GameBase::getDamageLevel(%player) == 0)
			{
				Player::trigger(%player,$WeaponSlot,false);
				return;
			}
		}
		else 
		{
			if(GameBase::getDamageLevel(%object) == 0)
			{
				Player::trigger(%player,$WeaponSlot,false);
				return;
			}
		}
	}
}

ItemImageData FixitImage 
{
	shapeFile = "repairgun";
	mountPoint = 0;
	weaponType = 2;
	mountOffset = { -0.2, 0, 0 };
	mountRotation = { 0, -1.575, 0 };
	projectileType = SuperRepairBolt;
	minEnergy = 5;
	maxEnergy = 15;
	lightType = 3;
	lightRadius = 1;
	lightTime = 1;
	lightColor = { 0.25, 1, 0.25 };
	sfxFire = SoundRepairItem;
	sfxActivate = SoundPickUpWeapon;
};

ItemData Fixit 
{
	description = "Builder's Tool";
	className = "Weapon";
	shapeFile = "repairgun";
	hudIcon = "repairpack";
	heading = $InvHead[ihtool];	//$InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = FixitImage;
	price = 50;
	showWeaponBar = true;
};

ItemImageData FixitImage2 
{
	shapeFile = "repairgun";
	mountPoint = 0;
	mountOffset = { 0, 0, 0 };
	mountRotation = { 0, 1.575, 0};
	weaponType = 2;
	projectileType = RePackBolt;
	minEnergy = 5;
	maxEnergy = 15;
	lightType = 3;
	lightRadius = 1;
	lightTime = 1;
	lightColor = { 0.25, 1, 0.25 };
	sfxFire = SoundUseCommandStation;
	sfxActivate = SoundPickUpWeapon;
};

ItemData Fixit2
{
	description = "Builder's Tool";
	className = "Weapon";
	shapeFile = "repairgun";
	hudIcon = "repairpack";
	//heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = FixitImage2;
	showWeaponBar = true;
	showInventory = false;
	Price = 0;
};


function Fixit::MountExtras(%player,%weapon)
{	
	Player::mountItem(%player,Fixit2,$FixitSlotA);
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>Builder's Tool: <f2>Repair Mode.\n<f3>Repairs players and objects at a faster rate than a Repair Pack.\n<f2>Press <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to switch modes.");	
}

function Fixit2::MountExtras(%player,%weapon)
{	
	Player::mountItem(%player,Fixit,4);
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>Builder's Tool: <f2>Repack Mode.\n<f3>Pick up your deployables.\n<f2>Press <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change modes.");
}


function RepairRewards(%player)
{
	%object = %player.repairTarget;	
	if(%object != -1) 
	{	
		if(GameBase::getTeam(%player) == GameBase::getTeam(%object)) 
		{	
			%lastdamage = %object.lastDamageObject;
//			%lastDamageType = %object.lastDamageType;
//			%lastDamageTeam = %object.lastDamageTeam;
			%client = Player::getClient(%player);
			if(%lastdamage != %client)
//			if(%lastdamage != %client && %object.lastDamageTeam != GameBase::getTeam(%object))
//			if(%lastdamage != %client && %lastDamageType != $DebrisDamageType)
			{
				%name = GameBase::getDataName(%object);	
				%class = %name.className;
				%data = %name.description;
					
			//	messageall(1,"N:"@ %name @", C:"@%class@", D:"@%data);

// new
				if(%name == PortableGenerator || %name == PortableSolar || %name == ControlJammer)
				{
					Client::sendMessage(%client,0,"~wWeapon5.wav");
					return;
				}
// end new
	
				if(%name == PulseSensor || %name == MediumPulseSensor || %class == Station || %class == Turret)
				{
					//give players a point if it's their teams, and they didnt damage it last.. 
					%client.score++;
					%client.Credits++;
					Game::refreshClientScore(%client);
					bottomprint(%client, "<jc>You received <f2>1<f0> point for repairing a <f2>"@ %data); //added this message to make it clear when you get repair points and made this specify what it is you repaired -death666
					Client::sendMessage(%client,0,"~wWeapon5.wav");
					%player.repairTarget = "";	//no more points.. 
					
					return ", 1 point";
				}	
			}
			//they caused the damage... NO POINT FOR JOO!
		}
	}
	return;
}

//hell lets just make a seperate one for generators so we can reward more points for their repairs -death666

function RepairRewardsGenerators(%player)
{
	%object = %player.repairTarget;	
	if(%object != -1) 
	{	
		if(GameBase::getTeam(%player) == GameBase::getTeam(%object)) 
		{	
			%lastdamage = %object.lastDamageObject;
			%client = Player::getClient(%player);
			if(%lastdamage != %client)
			{
				%name = GameBase::getDataName(%object);	
				%class = %name.className;
				%data = %name.description;
					
			//	messageall(1,"N:"@ %name @", C:"@%class@", D:"@%data);

// new
				if(%name == PortableGenerator || %name == PortableSolar || %name == ControlJammer)
				{
					Client::sendMessage(%client,0,"~wWeapon5.wav");
					Client::sendMessage(%client,0,"You kicked the "@%data@", and it restarted.");
					return;
				}
// end new	
				if(%class == Generator)
				{
					//give players two points if it's their teams, and they didnt damage it last.. 
					%client.score+=2;
					%client.Credits+=2;
					Game::refreshClientScore(%client);
					bottomprint(%client, "<jc>You received <f2>2<f0> points for repairing a <f2>"@ %data); //added this message to make it clear when you get repair points and made this specify what it is you repaired -death666
					Client::sendMessage(%client,0,"~wWeapon5.wav");  //shell_click
					%player.repairTarget = "";	//no more points.. 
					
					return ", 2 points";
				}	
			}
			//they caused the damage... NO POINT FOR JOO!
		}
	}
	return;
}

// neat generator stuff -death666

function checkRepairingCriticalInfrastructure(%player, %target)
{
    %name = GameBase::getDataName(%target);
    %client = Player::getClient(%player);

    // We only care about our own team's CI
    if(GameBase::getTeam(%player) != GameBase::getTeam(%target))
        return;

    // We only care about generators
    if (%name.className != Generator)
        return;

    // If the generator is receiving power, then the base already has power
    if (GameBase::isPowered(%target))
    {
        %client.repairingCI = false;
        return;
    }

    %client.repairingCI = true;

    if ($debug::repair)
        echo(%client @ " is repairing critical infrastructure");

    schedule("checkRepairingCriticalInfrastructure(" @ %player @ ", " @ %target @ ");", 0.5);
}

function checkRepairingCriticalPowers(%player, %target)
{
    %name = GameBase::getDataName(%target);
    %client = Player::getClient(%player);

    // We only care about our own team's CI
    if(GameBase::getTeam(%player) != GameBase::getTeam(%target))
        return;

    // We only care about generators
    if (%name.className != Generator)
        return;

    %client.repairingCP = true;

    schedule("checkRepairingCriticalPowers(" @ %player @ ", " @ %target @ ");", 0.5);
}

//cleaned up these team messages for power destroyed and restored -death666

function Generator::onPower(%this, %state, %generator)
{
    if ($debug::repair)
       echo("Power state for generator " @ %this @ " has changed to '" @ %state @ "', by generator " @ %generator);

    %client = Player::getClient(%this.LastRepairCl);
    %playerTeam = GameBase::getTeam(%this);

	if(%client.repairingCP && %this == %generator)
	{

 		Ann::GenPowering(%this,true);
	}

    if (%client.repairingCI && %this == %generator)
    {

        if ($debug::repair)
            echo(Client::getName(%client) @ " repaired critical infrastructure");
	    PowerTeamMessage(%playerTeam, 1,"<jl><bitem_ok.bmp><jc>Your teams power has been restored.<jr><bitem_ok.bmp>","Your teams power has been restored. ~wfemale2.wcheer2.wav");
    }
}