$InvList[BigCratePack] = 1;
$MobileInvList[BigCratePack] = 1;
$RemoteInvList[BigCratePack] = 1;
AddItem(BigCratePack);

$CanAlwaysTeamDestroy[BigCrate] = 1;

ItemImageData BigCratePackImage 
{
	shapeFile = "magcargo";
	mountPoint = 2;
	mountOffset = { 0, -0.12, -0.1 };
	mountRotation = { 0, 0, 0 };
	mass = 0.5;
	firstPerson = false;
};

ItemData BigCratePack 
{
	description = "Big Crate";
	shapeFile = "sensorjampack";
	className = "Backpack";
	heading = $InvHead[ihBar];	// $InvHead[ihBar];
	imageType = BigCratePackImage;
	shadowDetailMask = 4;
	mass = 1.5;
	elasticity = 0.2;
	price = 600;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function BigCratePack::deployShape(%player,%item) 
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
		Client::sendMessage(%client,0,"Deploy position out of range");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
	if(!checkDeployAreaCrate(%client,$los::position)) //this is a new deployarea check only for crates defined in the item cs -death666
	{
		return false;
	}

	%obj = $los::object;
	if(%obj.inmotion == true)	 
	{ 
		Client::sendMessage(%client,0,"Deploy area crappy, cannot deploy.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}

	%prot = GameBase::getRotation(%player);
	%zRot = getWord(%prot,2);
	%xpos = getWord($los::position,0);
	%ypos = getWord($los::position,1);
	%zpos = getWord($los::position,2);
	if(Vector::dot($los::normal,"0 0 1") > 0.6) 
	{
		%rot = "0 0 " @ %zRot;
		// %zpos += 1.05;//FLOOR
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

		}
	}
	%pos = %xpos@" "@%ypos@" "@%zpos;
//	%rot = vector::add(%rot,"0 0 0"); // -1.57 0 0

	%objBigCrate = newObject("","StaticShape",BigCrate,true);
	if(%player.repackEnergy != "")
	{
    GameBase::setDamageLevel(%objBigCrate, %player.repackDamage);
    GameBase::setEnergy(%objBigCrate, %player.repackEnergy);
    %player.repackDamage = "";
    %player.repackEnergy = "";
	}

	addToSet("MissionCleanup/deployed/Barrier", %objBigCrate);
	GameBase::setTeam(%objBigCrate,GameBase::getTeam(%player));
	GameBase::setRotation(%objBigCrate,%rot);
	GameBase::setPosition(%objBigCrate,%pos);
	Gamebase::setMapName(%objBigCrate,"Big Crate "@Client::getName(%client));
	Client::sendMessage(%client,0,"Big Crate Deployed");
	GameBase::startFadeIn(%objBigCrate);
	playSound(SoundPickupBackpack,$los::position);
	playSound(ForceFieldOpen,$los::position);
	$TeamItemCount[GameBase::getTeam(%player) @ "BigCratePack"]++;
	%objBigCrate.deployer = %client; 	
	if(!$build)
		Anni::Echo("MSG: ",%client," deployed a Big Crate");
	return true;
}

StaticShapeData BigCrate 
{
	shapeFile = "magcargo";
	maxDamage = 10.0; 
	debrisId = defaultDebrisLarge;
	explosionId = debrisExpLarge;
	visibleToSensor = true;
	damageSkinData = "objectDamageSkins";
	description = "Big Crate";
};

function BigCrate::onDestroyed(%this) 
{
	if(!$NoCalcDamage)
		calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 3, 0.55, 0.30, 250, 170);
	$TeamItemCount[GameBase::getTeam(%this) @ "BigCratePack"]--;
}

function BigCratePack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Big Crate: <f2>A big crate which can be used to cover turrets to further defend them.");	
}