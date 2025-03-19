$InvList[AcceleratorDevicePack] = 1;
$MobileInvList[AcceleratorDevicePack] = 1;
$RemoteInvList[AcceleratorDevicePack] = 1;
AddItem(AcceleratorDevicePack);

$CanAlwaysTeamDestroy[AcceleratorDevice] = 1;
$CanAlwaysTeamDestroy[AcceleratorDeviceSideOne] = 1;
$CanAlwaysTeamDestroy[AcceleratorDeviceSideTwo] = 1;

 //-=-=-=-=-=-=-=- Pack -=-=-=-=-=-=-

ItemImageData AcceleratorDevicePackImage
{
	shapeFile = "bridge"; //ammopack
	mountPoint = 2;
	mountOffset = { 0, 0, 0.1 };
	mountRotation = { 1.57, 0, 0 };
	mass = 4.0;
	firstPerson = false;
};

ItemData AcceleratorDevicePack
{
	description = "Starwolf Accelerator"; // Accelerator Device
	shapeFile = "ammopack"; //ammopack
	className = "Backpack";
	heading = $InvHead[ihDOb];
	imageType = AcceleratorDevicePackImage;
	shadowDetailMask = 4;
	mass = 1.5;
	elasticity = 0.2;
	price = 600;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function AcceleratorDevicePack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player); 
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Starwolf Accelerator: <f2>A device to speed <f1>things<f2> up. Able to launch Players, Grenades, Mines, Ammo, Bots, Suicide Det packs.");
}

function AcceleratorDevicePack::deployShape(%player,%item) 
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

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item] ) // && !$build
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

	%obj = $los::object;
	//Anni::Echo (GameBase::getTeam(%obj));
	if((GameBase::getTeam(%obj) != GameBase::getTeam(%player)) && (getObjectType(%obj) != "SimTerrain") && (GameBase::getTeam(%obj) != -1)) 
	{
		Client::sendMessage(%client,0,"Cannot deploy on enemy base");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}

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

	if(!checkDeployArea(%client,$los::position)) 
		return false;

	%objDevice = newObject("Accelerator", "Item", AcceleratorDevice, 1, true, true);
	%objDeviceFS  = newObject("Accelerator","StaticShape",AcceleratorDeviceFS, true);
	%objDeviceFS.deployer = %client; 
	%objDevice.deployer = %client; 
	%objSide1 = newObject("AcceleratorDeviceSideOne", "StaticShape", AcceleratorDeviceSideOne, true);
	%objSide2 = newObject("AcceleratorDeviceSideTwo", "StaticShape", AcceleratorDeviceSideTwo, true);
	%objSide1.deployer = %client; 
	%objSide2.deployer = %client; 
	%objSide1.objParent = %objDevice;
	%objDeviceFS.objParent = %objDevice;
	%objSide2.objParent = %objDevice;
	%objSide2.objParent = %objDevice;
	if(%player.repackEnergy != "")
	{
    GameBase::setDamageLevel(%objDeviceFS, %player.repackDamage);
    GameBase::setEnergy(%objDeviceFS, %player.repackEnergy);
	GameBase::setDamageLevel(%objSide1, %player.repackDamage);
    GameBase::setEnergy(%objSide1, %player.repackEnergy);
	GameBase::setDamageLevel(%objSide2, %player.repackDamage);
    GameBase::setEnergy(%objSide2, %player.repackEnergy);
    %player.repackDamage = "";
    %player.repackEnergy = "";
	}

	addToSet("MissionCleanup/deployed/turret", %objDevice);
	addToSet("MissionCleanup/deployed/turret", %objDeviceFS);
	addToSet("MissionCleanup/deployed/turret", %objSide1);
	addToSet("MissionCleanup/deployed/turret", %objSide2);

	GameBase::setTeam(%objDevice, GameBase::getTeam(%player));
	GameBase::setTeam(%objDeviceFS, GameBase::getTeam(%player));
	GameBase::setTeam(%objSide1, GameBase::getTeam(%player));
	GameBase::setTeam(%objSide2, GameBase::getTeam(%player));

	 // Place the object wherever you're looking
	%pos = $los::position;

	%posBeam = Vector::add(%pos,"0 0 1");
	GameBase::setPosition(%objDevice, %posBeam);

	%posBeam = Vector::add(%pos,"0 0 1");
	%posFS = Vector::add(%pos,"0 0 0");
	GameBase::setPosition(%objDeviceFS, %posFS);
	GameBase::setRotation(%objDeviceFS,GameBase::getRotation(%player));

	 // Set values for the sides
	%rot = "1.56 0 " @ getWord(GameBase::getRotation(%player), 2) + 1.56;
	%vec = Vector::getFromRot(%rot, 500);
	%xPos = getWord(%vec, 0) + getWord(%pos, 0);
	%yPos = getWord(%vec, 1) + getWord(%pos, 1);
	%zPos = getWord(%pos, 2)+1;
	%posSide = %xPos @ " " @ %yPos @ " " @ %zPos;
	GameBase::setPosition(%objSide1, %posSide);
	GameBase::setRotation(%objSide1, %rot);

	%rot = "1.56 0 " @ getWord(GameBase::getRotation(%player), 2) - 1.56;
	%vec = Vector::getFromRot(%rot, 500);
	%xPos = getWord(%vec, 0) + getWord(%pos, 0);
	%yPos = getWord(%vec, 1) + getWord(%pos, 1);
	%zPos = getWord(%pos, 2)+1;
	%posSide = %xPos @ " " @ %yPos @ " " @ %zPos;
	GameBase::setPosition(%objSide2, %posSide);
	GameBase::setRotation(%objSide2, %rot);

	%client = Player::getClient(%player);
	Gamebase::setMapName(%objDeviceFS,"Accelerator: " @ Client::getName(%client));
	Gamebase::setMapName(%objSide1,"Accelerator: " @ Client::getName(%client));
	Gamebase::setMapName(%objSide2,"Accelerator: " @ Client::getName(%client));

	Client::sendMessage(%client,0,"Starwolf Accelerator Deployed. ~wrmt_turret.wav"); // turretOff1

	%objSide2.objSide1 = %objSide1;
	%objSide2.objDevice = %objDevice;
	%objSide2.objDeviceFS = %objDeviceFS;
	
	%objSide1.objDevice = %objDevice;
	%objSide1.objSide2 = %objSide2;
	%objSide1.objDeviceFS = %objDeviceFS;
	
	%objDevice.objSide1 = %objSide1;
	%objDevice.objSide2 = %objSide2;
	%objDevice.objDeviceFS = %objDeviceFS;

	%objDeviceFS.objSide1 = %objSide1;
	%objDeviceFS.objDevice = %objDevice;
	%objDeviceFS.objSide2 = %objSide2;

	GameBase::startFadeIn(%objAcceleratorDevice);
//	playSound(SoundPickupBackpack,$los::position);
//	playSound(ForceFieldOpen,$los::position);
	playSound(SoundTurretDeploy,$los::position);

	$TeamItemCount[GameBase::getTeam(%player) @ "AcceleratorDevicePack"]++;
	%objAcceleratorDevice.deployer = %client; 
//	if(!$build)
		Anni::Echo("MSG: ",%client," deployed a Accelerator");
	return true;

}

ItemImageData AcceleratorBeamImage
{
	shapeFile = "snowplume";
	mountPoint = 2;
	mountOffset = { 0, 0, 0 };
	mountRotation = { 0, 0, 0.1 };
	lightType = 2;   // Pulsing
	lightRadius = 4;
	lightTime = 1.5;
	lightColor = { 1, 1, 1};
	firstperson = false;
};

ItemData AcceleratorDevice
{
   	description = "Accelerator Beam";
	shapeFile = "snowplume";
	imageType = AcceleratorBeamImage;
	showInventory = false;
	shadowDetailMask = 4;
};

function AcceleratorDevice::DestroyPad(%this, %pad)
{
	deleteObject(%this);
}

function AcceleratorDevice::onDestroyed(%this) 
{
	if(!$NoCalcDamage)
		calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 2, 0.40, 0.1, 250, 100);
	$TeamItemCount[GameBase::getTeam(%this) @ "AcceleratorDevicePack"]--;
}

function AcceleratorDeviceFS::onDestroyed(%this) 
{
	StaticShape::onDestroyed(%this); 
	%this.cloakable = "";
	%this.nuetron = "";
	GameBase::setDamageLevel(%this.objSide1, 6.5);
	GameBase::setDamageLevel(%this.objSide2, 6.5);

	$TeamItemCount[GameBase::getTeam(%this) @ "AcceleratorDevicePack"]--;
	AcceleratorDevice::DestroyPad(%this.objParent, %this);
}

function AcceleratorDeviceFS::onCollision(%this,%obj)
{
	%c = Player::getClient(%obj);
	%armor = Player::getArmor(%c);
	%vecVelocity = Item::getVelocity(%obj);
//	%rnd = floor(getRandom() * 40);

	if(Player::isDead(%obj))
		return;

	if(%obj.itembounced == true)
		return;

	%type = getObjectType(%obj);
	if(getObjectType(%obj) != "Player" || (Player::isAIControlled(%obj)))
{

if(%this.faded == "") 
{
%this.faded =1;
schedule(%this@".faded = \"\";",0.5,%this);

	%name = GameBase::getDataName(%obj);	
	%class = %name.className;
	%description = %name.description; 


		if(%class ==  Mine || %class == Ammo || %class == Handgrenade || %class == HandAmmo || (Player::isAIControlled(%obj)) )
		{
		%obj.itembounced = true;
		if(%name == AntipersonelMine)
		{
			schedule("GameBase::setDamageLevel(" @ %obj@ "," @ 1 @ ");", 3.5,%obj);
			%obj.itembounced = true;
		}
		if(%description == Hologram)
		{
			schedule("GameBase::setDamageLevel(" @ %obj @ "," @ %name.maxDamage @ ");", 3.5,%obj);
			%obj.minebounced = true;
		}
	%rnd = floor(getRandom() * 40);
	if (%rnd == 1)
	{	
		GameBase::playSound(%this, debrisLargeExplosion, 0);		
		%HMult = 100;
		%ZMax = 0.1;
		%vecNewVelocity = GetWord(%vecVelocity, 0) * %HMult @ " " @ GetWord(%vecVelocity, 1) * %HMult @ " " @ %ZMax;
		Item::setVelocity(%obj, %vecNewVelocity);
		return;
	}
	else if (%rnd > 30)
	{	
		GameBase::playSound(%this, debrisLargeExplosion, 0);
		%HMult = 45.0;
		%ZMax = 0.1;
		%vecNewVelocity = GetWord(%vecVelocity, 0) * %HMult @ " " @ GetWord(%vecVelocity, 1) * %HMult @ " " @ %ZMax;
		Item::setVelocity(%obj, %vecNewVelocity);
		return;
	}
	else
	{
		GameBase::playSound(%this, SoundFireMortar, 0);
		%HMult = 20.0;
		%ZMax = 0.1; 
		%vecNewVelocity = GetWord(%vecVelocity, 0) * %HMult @ " " @ GetWord(%vecVelocity, 1) * %HMult @ " " @ %ZMax;
		Item::setVelocity(%obj, %vecNewVelocity);
		return;
	}
		return;
		}
		return;
}

}
	%type = getObjectType(%obj);
	if(getObjectType(%obj) == "Player")
{

if(%this.faded == "") 
{
%this.faded =1;
schedule(%this@".faded = \"\";",1,%this);

	if(%c.hashallpass)
{
		%c = Player::getClient(%obj);
		%c.hashallpass= false;
		schedule(%c@".hashallpass = true;",2,%c); // 5

		%c = Player::getClient(%obj);
		%armor = Player::getArmor(%c);
		%medrnd=floor(getrandom()*40);
		%armor=GameBase::getDataName(%obj);
		%mass=%armor.mass;
		%rot=GameBase::getRotation(%obj);

		if (%medrnd == 15) // one is unlucky
		{
		GameBase::playSound(%this, explosion3, 0); //debrisLargeExplosion
         		Client::SendMessage(%c, 0, "Accelerator Malfunction! ~wexplo3.wav"); //debris_large		
		%medrnd = %medrnd + 150;
		}
		else if(%medrnd > 35) 
		{
		%soundrnd = floor(getRandom() * 2); 
		if (%soundrnd == 0)
		{ 
			GameBase::playSound(%this, bigExplosion2, 0);
			Client::SendMessage(%c, 0, "~wbxplo2.wav");
		} 
		else if (%soundrnd == 1)
		{ 
			GameBase::playSound(%this, bigExplosion3, 0);
			Client::SendMessage(%c, 0, "~wbxplo3.wav");
		}
			Client::SendMessage(%c, 0, "A-C-C-E-L-E-R-A-T-E-d-d-d-!-!");
			%medrnd = %medrnd + 10;
		}
		else
		{
		%soundrnd = floor(getRandom() * 5); 
		if (%soundrnd == 0) 
		{
			GameBase::playSound(%this, turretExplosion, 0);
			Client::SendMessage(%c, 0, "~wturretexp.wav");
 		} 
		else if (%soundrnd == 1)
		{ 
			GameBase::playSound(%this, SoundBeaconUse, 0);
			Client::SendMessage(%c, 0, "~wteleport2.wav");
		} 
		else if (%soundrnd == 2)
		{ 
			GameBase::playSound(%this, SoundParticleBeamRecharge, 0);
			Client::SendMessage(%c, 0, "~wtargetlaser.wav");
		}
		else if (%soundrnd == 3) 
		{
			GameBase::playSound(%this, SoundShieldHit, 0);
			Client::SendMessage(%c, 0, "~wshieldhit.wav");
 		} 
		else if (%soundrnd == 4)
		{ 
			GameBase::playSound(%this, shockExplosion, 0);
			Client::SendMessage(%c, 0, "~wshockexp.wav");
		}
			Client::SendMessage(%c, 0, "Accelerated.");
		}	
		%len = 40 + %medrnd;
		%trans = GameBase::getMuzzleTransform(%obj);
		%tr= getWord(%trans,5);
		if(%tr < 0) %tr = -%tr;
		%tr = %tr+ 0.15;
		%up = 0.001; //%tr
		%out = 1-%tr;
		%vec = Vector::getFromRot(%rot,%len*%mass*%out,%len*%mass*%up); 
		Player::applyImpulse(%obj,%vec);
}
		return;
}
		return;
}

}

//-=-=-=-=-=-=-=- Accelerator Device Side (staticshape) =-=-=-=-=-=-=-

StaticShapeData AcceleratorDeviceSideOne
{
       	shapeFile = "bridge";
	debrisId = defaultDebrisSmall;
	maxDamage = 6.5;
	visibleToSensor = true;
	isTranslucent = false;
	className = "Decoration";
   	description = "Accelerator";
	mapFilter = 4;
	mapIcon = "M_generator";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	explosionId = flashExpMedium;
};

StaticShapeData AcceleratorDeviceSideTwo
{
       	shapeFile = "bridge";
	debrisId = defaultDebrisSmall;
	maxDamage = 6.5;
	visibleToSensor = true;
	isTranslucent = false;
	className = "Decoration";
   	description = "Accelerator";
	mapFilter = 4;
	mapIcon = "M_generator";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	explosionId = flashExpMedium;
};

StaticShapeData AcceleratorDeviceFS
{
	shapeFile = "flagstand";
	maxDamage = 6.5;
	description = "Accelerator";
	isTranslucent = false; 
	className = "Decoration";
	debrisId = flashDebrisMedium;
	visibleToSensor = true;
	mapFilter = 4;
	mapIcon = "M_generator";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	explosionId = flashExpMedium;
	sfxAmbient = SoundDiscSpin;
};

function AcceleratorDeviceSideOne::onDestroyed(%this) 
{
	StaticShape::onDestroyed(%this);
	%this.cloakable = "";	
	%this.nuetron = "";
	GameBase::setDamageLevel(%this.objSide2, 6.5);
	GameBase::setDamageLevel(%this.objDeviceFS, 6.5);
//	AcceleratorDevice::DestroyPad(%this.objParent, %this);
}

function AcceleratorDeviceSideTwo::onDestroyed(%this) 
{
	StaticShape::onDestroyed(%this);
	%this.cloakable = "";	
	%this.nuetron = "";
	GameBase::setDamageLevel(%this.objSide1, 6.5);
	GameBase::setDamageLevel(%this.objDeviceFS, 6.5);
//	AcceleratorDevice::DestroyPad(%this.objParent, %this);
}
