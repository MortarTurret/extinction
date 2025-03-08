

$InvList[Grabbler] = 1;
$MobileInvList[Grabbler] = 1;
$RemoteInvList[Grabbler] = 1;

$AutoUse[Grabbler] = False;
$WeaponAmmo[Grabbler] = "";

addWeapon(Grabbler);

SoundData GrabblerFire
{
	wavFileName = "turretfire2.wav";//door2.wav";//BXplo4.wav//Grenade.wav//rifle1.wav//turretfire2.wav
	profile = Profile3dFar;
};

SeekingMissileData GrabblerShock
{
	bulletShapeName = "fusionbolt.dts";
	explosionTag = LargeShockwave;
	collisionRadius = 0.0;
	mass = 2.0;
	damageClass = 1; // 0 impact, 1, radius
	damageValue = 0.0;	//0.5
	damageType = $MissileDamageType;
	explosionRadius = 9.5;
	kickBackStrength = 350.0;
	muzzleVelocity = 120.0;
	totalTime = 1;
	liveTime = 1;
	seekingTurningRadius = 2.5;
	nonSeekingTurningRadius = 3.0;
	proximityDist = 0.25;
	smokeDist = 1.75;
	lightRange = 5.0;
	lightColor = { 0.4, 0.4, 1.0 };
	inheritedVelocityScale = 0.5;
	soundId = SoundJetHeavy;
};

LightningData GrabLightning
{
	bitmapName = "grn_blink4.bmp";	//lightningNew.bmp";
	damageType = $ElectricityDamageType;
	boltLength = 65.0;
	coneAngle = 35.0;
	damagePerSec = 0.0;
	energyDrainPerSec = 60.0;
	segmentDivisions = 4;
	numSegments = 5;
	beamWidth = 0.075;//075;
	updateTime = 120;
	skipPercent = 0.5;
	displaceBias = 0.15;
	lightRange = 3.0;
	lightColor = { 0.25, 0.25, 0.85 };
	soundId = SoundLaserIdle;
};


RepairEffectData GrabBolt
{
	bitmapName = "grn_blink4.bmp";//lightningNewSub.bmp";//lightningTemp.bmp";//fx_lensflare_5.bmp";//cphoenix.flag.bmp";	//grn_blink4.bmp";	//mort0000.bmp";	//repairadd.bmp";
	boltLength = 15.5;
	
	segmentDivisions = 4;
	beamWidth = 0.25;
	updateTime = 450;	//how fast beam oscilates
	skipPercent = 0.0001;	//0.6
	displaceBias = 0.15;	//0.15 //displaced side to side
	lightRange = 3.0;
	lightColor = { 0.85, 0.25, 0.25 };
};



ItemImageData GrabblerImage 
{
	shapeFile = "mortargun";	//"tracer";
	mountPoint = 0;
	mountOffset = { -0.1, -0.3, -0.1 };
	mountRotation = {0, 1.57, 0};
	weaponType = 2; // 2Sustained
	projectileType = GrabBolt;	//GrabLightning;	//GrabBolt;
	minEnergy = 3;
	maxEnergy = 0; //8 Energy used/sec for sustained weapons
	reloadTime = 0.0;
	lightType = 3; // Weapon Fire
	lightRadius = 2;
	lightTime = 1;
	lightColor = { 0.25, 0.25, 0.85 };
	sfxReady = SoundJammerOn;
	sfxActivate = SoundActivateAmmoStation;
	sfxFire = SoundLaserIdle;//SoundSpinUpDisc;	//SoundELFIdle;
};

ItemData Grabbler 
{
	description = "Pitchfork";	//Energy Glove";	//"Spider Beam";
	className = "Weapon";
	shapeFile = "mortargun";
	hudIcon = "sniper";
	heading = $InvHead[ihtool];	//$InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = GrabblerImage;
	price = 375;
	showWeaponBar = true;
};
ItemImageData PitchforkImage 
{
	shapeFile = "mine";	//"tracer";
	mountPoint = 0;
	mountOffset = { 0.05, 0.5, -0.1 };	// 0.1, 0.5, -0.2	right,forward,up
	mountRotation = {0, -1.57, 1.57};
	weaponType = 2; // 2Sustained
};

ItemData Pitchfork 
{
	description = "Pitchfork";	//Energy Glove";	//"Spider Beam";
	className = "Weapon";
	shapeFile = "mine";
	//hudIcon = "sniper";
	heading = $InvHead[ihWea];
	shadowDetailMask = 4;
	imageType = PitchforkImage;
	price = 375;
	showWeaponBar = true;
};

function GrabblerImage::onFire(%player)
{		
	if($debug)
		echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));		

	%clientId = Player::getClient(%player);
	%trans = GameBase::getMuzzleTransform(%player);
	%vel = Item::getVelocity(%player);
	Projectile::spawnProjectile("GrabBolt",%trans,%player,%vel);
}

function Grabbler::MountExtras(%player,%weapon)
{	
	Player::mountItem(%player,Pitchfork,7);
	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>Move deployed objects around.\n<jc><f2>Press <f1>Use Laser<f2> or <f1>Use Blaster (1 and 6 keys) <f2>to weld your deployables in place or to boost players.");
}

function GrabBolt::onAcquire(%this, %player, %target)
{
	%clientId = Player::getClient(%player);
	if(%clientId.noPfork)
	{
		Client::sendMessage(%clientId, 0, "Your pitchfork has been removed by an Admin.");
		return;
	}
	%player.GrabObject = "";
	%player.Grabdist = "";
	%player.GrabRoty = "";
	%player.GrabOffsetVec = "";
		
	if($build)
		%player.GrabObject = %target;
		
	if(%target == %player) 
	{
		return;
	}
	else
	{
		
		%client = Player::getClient(%player);
		%player.GrabObject = %target;		
		%type = getObjectType(%target);
		%dataName = GameBase::getDataName(%target);
		%shape = %dataName.shapeFile;
		%description = %dataName.description; // muhaha death666
		
		if($debug)
			bottomprint(%client,"<jc>"@%type@" Data Name= "@%dataName@" shape= "@%shape);
				

		if(!$build)
		{

			if(%dataName == AntipersonelMine)
			{
	if(GameBase::getTeam(%target) == Client::getTeam(%client))
				{
				return;
				}
	if(GameBase::getTeam(%target) != Client::getTeam(%client))
				{
				schedule("GameBase::setDamageLevel(" @ %target@ "," @ 1 @ ");", 0.01,%target);
				Client::sendMessage(%clientId, 0, "Enemy ordinance detonated.");
				return;
				}
			}
			if(%description == Hologram)
			{
	if(GameBase::getTeam(%target) == Client::getTeam(%client))
				{
				return;
				}
	if(GameBase::getTeam(%target) != Client::getTeam(%client))
				{
				schedule("GameBase::setDamageLevel(" @ %target@ "," @ %dataName.maxDamage @ ");", 0.01,%target); //et tu lol
				Client::sendMessage(%clientId, 0, "Enemy ordinance detonated.");
				return;
				}
			}
			if(%type != "Player" && %dataName != BlastWall && %dataName != LargeForcefield && %dataName != ForceFieldDoorShape && %dataName != DeployableForcefield && %dataName != LargeForceFieldDoorShape && %dataName != DeployablePlatform && %dataName != BigCrate && %dataName != DeployablePlasmaFloor) //added DeployablePlasmaFloor -death666
				return;

		}
		if(%dataname == JailWall)	// not even with $build
			return;
			
		%trans = GameBase::getMuzzleTransform(%player);	//position of tip
			%posX = getWord(%trans,9);		//x
			%posY = getWord(%trans,10);		//y
			%posZ = getWord(%trans,11); 		//z	
		%GunTipPos = %posX@" "@%posY@" "@%posZ;
			
		//figure out general relativity, Einstein..
			%d1= getWord(%trans,3);
			%d2= getWord(%trans,4);
			%d3= getWord(%trans,5);				
			
			%TargetPos = GameBase::getPosition(%target);
			

		if(%type == "Player")// || %type == "Flier")// || %type == "Mine")
		{
			%player.Grabdist = vector::getdistance(%TargetPos,%GunTipPos);
			%player.GrabOffsetVec = "";
			%player.GrabRoty = "";
			%target.forker = %player;	
		}

		else		
		{	
		// This works ok for now. When beam attach point changes
		// while rotating, movement is still funky...
			GameBase::getLOSInfo(%player,1000);
				// GetLOSInfo sets the following globals:
				// 	los::position
				// 	los::normal
				// 	los::object	
						
			%RealDist = vector::getdistance($los::position,%GunTipPos);				
			%player.Grabdist = %RealDist;	
			
			%TargetRot = GameBase::getRotation(%target);
			%playerRot = GameBase::getRotation(%player);
			%targetRelative = vector::add(%TargetRot ,vector::multiply("0 0 -1",%playerRot));
			
			%player.GrabRoty = %targetRelative;
				
		//%GunExtPos is like the end of a broom handle shoved down gun barrel.
			%GunExtPos = %posX + %d1 * %RealDist@" "@%posY + %d2 * %RealDist@" "@%posZ + %d3 * %RealDist;
			
			%OffsetVec = vector::add(vector::multiply("-1 -1 -1",%GunExtPos),%TargetPos);	//dist from gun tip to where beam is hitting, more or less...
	//		%Nrot = vector::add(vector::multiply("-1 -1 -1",%targetRot),%playerRot);
	//		%OffsetVecNew = RotateVector(%OffsetVec,%Nrot);	//normalizing vector relative to player
			%player.GrabOffsetVec = %OffsetVec;			
			
				
		}			
	Grabler::move(%player,%target);			
	}
}

function GrabBolt::checkDone(%this, %player)
{
}



function Grabler::move(%player,%target)
{
	%client = Player::getClient(%player);
	// Server will crash without this check! Repacking a forked object will fuck shit up! -Ghost
	if(%target.repacked == true)
	{
		Player::trigger(%player, $WeaponSlot, false);
		%target.repacked = false;
		%player.GrabOffsetVec = "";
		%player.GrabObject = "";
		%player.Grabdist = "";
		%player.GrabRoty = "";
		%target.forker = "";
		return;
	}
	if(%target.welded && GameBase::getTeam(%target) == Client::getTeam(%client))
	{
		if(%target.deployer == %client || %client.isGoated)
		{	
			%target.welded = "";
			//%target.ewelded = "";
			Client::sendMessage(%client, 0, Client::getName(%target.deployer) @"'s "@ GameBase::getDataName(%target) @" is dislodged!~wflagflap.wav");
		}
		else
		{	
			Client::sendMessage(%client, 0, Client::getName(%target.deployer) @"'s "@ GameBase::getDataName(%target) @" is welded in place!~wflagflap.wav");
			Projectile::spawnProjectile("ForkReleaser", "0 0 -1 0 0 0 0 0 -1 "@ getBoxCenter(%target), %player, Item::getVelocity(%target));
			Player::trigger(%player, $WeaponSlot, false);
			%player.GrabOffsetVec = "";
			%player.GrabObject = "";
			%player.Grabdist = "";
			%player.GrabRoty = "";
			%target.forker = "";
			return;
		}
	}
	else if(getObjectType(%target) == "Player" && Client::getTeam(Player::getClient(%target)) == Client::getTeam(%client) && Player::isJetting(%target))
	{	
		Projectile::spawnProjectile("ForkReleaser", "0 0 -1 0 0 0 0 0 -1 "@ getBoxCenter(%target), %player, Item::getVelocity(%target));
		Player::trigger(%player, $WeaponSlot, false);
		%player.GrabOffsetVec = "";
		%player.GrabObject = "";
		%player.Grabdist = "";
		%player.GrabRoty = "";
		%target.forker = "";
		return;
	}
	%weapon = Player::getMountedItem(%player,$WeaponSlot);
	if(Player::isTriggered(%player,$WeaponSlot) && %weapon == Grabbler)
	{
		if(%player.GrabObject != %target || %player.GrabObject == "")
		{
			%player.GrabObject = "";
			%player.Grabdist = "";
			%player.GrabRoty = "";
			%player.GrabOffsetVec = "";
			return;
		}
		%type = getObjectType(%target);
		%trans = GameBase::getMuzzleTransform(%player);	
		%vel = Item::getVelocity(%player);
		
		//position of tip
			%posX = getWord(%trans,9);		//x
			%posY = getWord(%trans,10);		//y
			%posZ = getWord(%trans,11); 		//z	
			%GunTipPos = %posX@" "@%posY@" "@%posZ;
		//direction gun is pointed foo.
		%d1= getWord(%trans,3);
			%d2= getWord(%trans,4);
			%d3= getWord(%trans,5);		
			%GunRotVec = %d1 @" " @ %d2 @" " @ %d3;	//%d3 is up/ down
	
		%dist = %player.Grabdist;
		
		if(%type == "Player")	// || %type == "Flier")
		{
			Item::setVelocity(%target, 0);
			%position = %posX + %d1 * %dist@" "@%posY + %d2 * %dist@" "@%posZ + %d3 * %dist;
			if(GameBase::testPosition(%target, %position))
			{
				GameBase::setPosition(%target,%position);
			}	
			else 
			{
				%TargetPos = GameBase::getPosition(%target);
				%oldheight = getword(%TargetPos,2);
				%newHeight = getword(%position,2);
				
				//player aiming down, pushing player through ground?
				if(%newHeight < %oldheight)		//%d3 < -0.5)
				{
					//Player::trigger(%player,$WeaponSlot,false);
					GameBase::setPosition(%target,vector::add(%TargetPos,"0 0 0.2"));
					return;
				}
				else
				{
					
					%player.Grabdist = vector::getdistance(%TargetPos,%GunTipPos);			
				}
				
			}
			schedule("Grabler::move("@%player@","@%target@");",0.037);	
		}		
		else
		{					
			%targetRelative = %player.GrabRoty;
			%playerRot = GameBase::getRotation(%player);
			%newrot = vector::add(%playerRot,%targetRelative);
			%GunExtPos = vector::add(vector::multiply(%GunRotVec,%dist@" "@%dist@" "@%dist),%GunTipPos);		
			
			%OffsetVec = %player.GrabOffsetVec;
	//	bottomprint(Player::getClient(%player),%OffsetVec@" "@%playerRot);
	//		%OffsetVecNew = RotateVector(%OffsetVec,%targetRot);	//%newrot);	//ah the joys of rotation... 
			%position = vector::add(%GunExtPos,%OffsetVec);	//%OffsetVecNew
			GameBase::setPosition(%target,%position);
			gamebase::setrotation(%target,%newrot);	
			schedule("Grabler::move("@%player@","@%target@");",0.025);			
		}
		
		
	}

}



function GrabBolt::onRelease(%this, %player)
{
	(%player.GrabObject).forker = "";	
	%player.GrabOffsetVec = "";	
	%player.GrabObject = "";
	%player.Grabdist = "";
	%player.GrabRoty = "";
	%player.GrabTime = "";
	%player.Mine = "";
}


// called when player shoots object
function grabbler::fire(%player,%object)
{	
	//echo("firegrab");
	%type = getObjectType(%object);
	%client = Player::getClient(%player);
	if(%type == "Player")
	{		
		//%object = %player.GrabObject;
	
		%trans = GameBase::getMuzzleTransform(%player);	
		%vel = Item::getVelocity(%player);
								//position of tip
		%posX = getWord(%trans,9);		//x
		%posY = getWord(%trans,10);		//y
		%posZ = getWord(%trans,11); 		//z	
		%GunTipPos = %posX@" "@%posY@" "@%posZ;
	
		%d1= getWord(%trans,3);
		%d2= getWord(%trans,4);
		%d3= getWord(%trans,5);		
		if(%player != %object)
		{		
			Projectile::spawnProjectile("GrabblerShock",%trans,%player,%vel,%object);
			playSound(GrabblerFire, GameBase::getPosition(%client));
			%target = Player::getClient(%object);
			%targetName = Client::getName(%target);
			%clientName = Client::getName(%client);

			Client::sendMessage(%client, 0, "You gave "@ %targetName @" a boost.");
			Client::sendMessage(%target, 0, %clientName @" gave you a boost.");
			
			 if(Client::getTeam(%client) == GameBase::getTeam(%object))
			//	Admin::Message(%clientName @" gave "@ getWord("his her", Client::getGender(%client) == "Female") @" ally "@ %targetName @" a boost.");
			{
			}
			
			else if(Client::getTeam(%client) != GameBase::getTeam(%object))
				{
				if(!Player::isAIControlled(%player))
				{
				if($MDESC::Type != "CTF BOTS")
				{				
				Armor::onShock(%target, %object);
				Armor::onBurn(%target, %object);
				startPoison(%target, %object);
				bottomprint(%target, "<jc><f0>You were poisoned by <f2>"@ %clientName @"<f0>'s dirty pitchfork!");
				Client::sendMessage(%target, 1, "You were poisoned by "@ %clientName @"'s dirty pitchfork!");
				}
				}
				}
			
			//anti tk code, and death message tracker.
			%object.LastBoost = %client;
			%object.BoostTime = getSimTime();	
				
		}	
		
		%GunRotVec = %d1 @" " @ %d2 @" " @ %d3;
		%fireImpulse = vector::add(vector::multiply("75 75 75",%GunRotVec),%vel);	//75
	//	Player::trigger(%player,$WeaponSlot,false);
		//Player::applyImpulse(%obj,%vec);
		//bottomprint(%client,"fire grabbler "@%object@" impulse "@%fireImpulse);
		//echo(%client,"fire grabbler "@%object@" impulse "@%fireImpulse);
	
		GameBase::playSound(%player, GrabblerFire, 0);	//SoundDoorClose, 0);
	//	Player::applyImpulse(%object,%fireImpulse); //gives a screwy trajectory
		
		%object.Grabfire = 150;
		Grabbler::smoke(%object);
		Item::setVelocity(%object,%fireImpulse);
		

		%target = %player.GrabObject;	
		%target.forker = "";			
		%player.GrabObject = "";
		%player.Grabdist = "";
		%player.GrabRoty = "";
		%player.GrabOffsetVec = "";		
		
	}
	else if(Object::getName(getGroup(%object)) == "Barrier" && ((%object.deployer == %client || %client.isAdmin) && Client::getTeam(%client) == GameBase::getTeam(%object)))
	{
		%object.welded = true;
		//%object.ewelded = true; 
		Client::sendMessage(%client, 0, Client::getName(%object.deployer) @"'s "@ GameBase::getDataName(%object) @" is welded in place!~wflagflap.wav");
	}
	
}


// player smoke trail
function Grabbler::smoke(%player)
{
	
	if(Player::isAIControlled(%player))
	{
		// messageall(1, "Bot Grabbler Smoke.");
		return;
	}
	
	if($MDESC::Type != "CTF BOTS")
	{
		return;
	}
	
	if(Player::isDead(%player))	return;
	%player.Grabfire = %player.Grabfire -1;
	%trans = "0 0 -1 0 0 0 0 0 -1 " @ getBoxCenter(%player);
	%vel = Item::getVelocity(%player);
	if(vector::getdistance(%vel,"0 0 0") > 10)
		Projectile::spawnProjectile("JetSmoke", %trans, %player, %Vel); //transform, object, velocity vector, <projectile target (seeker)>
	if(%player.Grabfire > 1)
		schedule("Grabbler::smoke("@%player@");",0.05);	
	
}

//}
