
$ArmorKickback[iarmorWarrior] = 1.00;

PlayerData armormWarrior
{
	className = "Armor";
	shapeFile = "marmor";
	flameShapeName = "PLASMATRAIL";	
	shieldShapeName = "shield";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	shadowDetailMask = 1;
	canCrouch = true;
	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
	maxJetSideForceFactor = 0.8;
	maxJetForwardVelocity = 22;
	minJetEnergy = 1;
	jetForce = 325;
	jetEnergyDrain = 1.0;
	maxDamage = 1.0;
	maxForwardSpeed = 10.0;
	maxBackwardSpeed = 8.0;
	maxSideSpeed = 8.0;
	groundForce = 35 * 13.0;
	groundTraction = 4.0;
	maxEnergy = 100;
	mass = 13.0;
	drag = 1.0;
	density = 1.5;
	minDamageSpeed = 25;
	damageScale = 0.005;
	jumpImpulse = 110;
	jumpSurfaceMinDot = 0.2;
	animData[0] = { "root", none, 1, true, true, true, false, 0 };
	animData[1] = { "run", none, 1, true, false, true, false, 3 };
	animData[2] = { "runback", none, 1, true, false, true, false, 3 };
	animData[3] = { "side left", none, 1, true, false, true, false, 3 };
	animData[4] = { "side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
	animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
	animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[14] = { "fall", none, 1, true, true, true, false, 3 };
	animData[15] = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16] = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17] = { "tumble loop", none, 1, true, false, false, false, 3 };
	animData[18] = { "tumble end", none, 1, true, false, false, false, 3 };
	animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
	animData[21] = { "throw", none, 1, true, false, false, false, 3 };
//	animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
	animData[22] = { "pose kneel", none, 1, true, false, false, true, 1 };	
	
	animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
	animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
	animData[25] = { "crouch die", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[26] = { "die chest", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[27] = { "die head", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[28] = { "die grab back", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[29] = { "die right side", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[30] = { "die left side", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[31] = { "die leg left", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[32] = { "die leg right", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[33] = { "die blown back", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[34] = { "die spin", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[35] = { "die forward", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[36] = { "die forward kneel", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[37] = { "die back", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[38] = { "sign over here", none, 1, true, false, false, false, 2 };
	animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
	animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
	animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
	animData[42] = { "sign salut", none, 1, true, false, false, true, 1 };
	animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };
	animData[50] = { "wave", none, 1, true, false, false, true, 1 };
	jetSound = SoundJetLight;
	rFootSounds = { SoundMFootRSoft, SoundMFootRHard, SoundMFootRSoft, SoundMFootRHard, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRHard, SoundMFootRSnow, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft };
	lFootSounds = {	SoundMFootLSoft, SoundMFootLHard, SoundMFootLSoft, SoundMFootLHard, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLHard, SoundMFootLSnow, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft };
	footPrints = { 2, 3 };
	boxWidth = 0.65;	//0.7
	boxDepth = 0.65;	//0.7
	boxNormalHeight = 2.4;
	boxCrouchHeight = 2.0;
	boxNormalHeadPercentage = 0.83;
	boxNormalTorsoPercentage = 0.49;
	boxHeadLeftPercentage = 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage = 0;
	boxHeadFrontPercentage = 1;
};

function armormWarrior::onPoison(%client, %player)
{
	if($TALT::Active == false) 
		Armor::onPoison(%client, %player);
}

function armormWarrior::onBurn(%client, %player)
{
	if($TALT::Active == false) 
		Armor::onBurn(%client, %player);
}

function armormWarrior::onShock(%client, %player)
{
	if($TALT::Active == false) 
		Armor::onShock(%client, %player);
}

function armormWarrior::onPlayerContact(%targetPlayer, %sourcePlayer)
{
	Armor::onPlayerContact(%targetPlayer, %sourcePlayer);
}

function armormWarrior::onGrenade(%player)
{
	if($TALT::Active == false)
		%obj = newObject("","Mine","Handgrenade");
	else
		%obj = newObject("","Mine","HandgrenadeLT");
	Armor::ThrowGrenade(%player, %obj);
}

function armormWarrior::onBeacon(%player, %item)
{
	DebugFun("armormWarrior", %player, %item);
	if ( !isObject(%player) || Player::isDead(%player) || GameBase::getPosition(%player) == "0 0 0" )
		return;

	%vel = Item::getVelocity(%player);

	//if(getsimtime() - %player.LastBeacon > 4.0)
	//	%player.BeaconTimer = 0;	
	
	//%player.LastBeacon = getSimTime();	
		
	//if(%player.BeaconTimer > 80)
	//{
	//	%Pos = getboxcenter(%player); 
	//	%trans =  "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%player); 
	//	Projectile::spawnProjectile("suicideShell", %trans, %player, %vel);
	//	Client::sendMessage(Player::getClient(%player),0, "Your booster exploded!");
	//}
	//else
	//{
		if(%vel == "0 0 0")
		{
			//fixed to send em where they look... not just forward..
			%trans = GameBase::getMuzzleTransform(%player);
			%smack = 300/25;
			%rot=GameBase::getRotation(%player);
			%len = 30;
			%tr= getWord(%trans,5);
			if(%tr <=0 )%tr -=%tr;
			%up = %tr+0.15;
			%out = 1-%tr;
			%vec = Vector::getFromRot(%rot,%len*%out*%smack,%len*%up*%smack);
			if ( ( getWord(%vec,0) > 0 || getWord(%vec,0) < 0 ) && ( getWord(%vec,1) > 0 || getWord(%vec,1) < 0 ) && ( getWord(%vec,2) > 0 || getWord(%vec,2) < 0 ) )
				Item::setVelocity(%player, getWord(%vel,0)+getWord(%vec,0)@" "@getWord(%vel,1)+getWord(%vec,1)@" "@getWord(%vel,2)+getWord(%vec,2) );
			else
			{
				admin::message("ERROR: Booster Vectoring error!  Vec=\""@%vec@"\" from person "@GameBase::getName(Player::getClient(%player))@" ("@%player@")");
				$Admin = "ERROR: Booster Vectoring error!  Vec=\""@%vec@"\" from person "@GameBase::getName(Player::getClient(%player))@" ("@%player@")";
				export("Admin", "config\\Error.log", true);
				return;
			}
			GameBase::playSound(%player, SoundFireMortar, 0);
			Client::sendMessage(Player::getClient(%player),0, "Speed Boost Initiated..");	
		}
		else
		{	
			%vel = Vector::Normalize(%vel);
			%vec = GetWord(%vel, 0) * 300 @ " " @ GetWord(%vel, 1) * 300 @ " " @ GetWord(%vel, 2) * 300;
			Player::applyImpulse(%player, %vec);
			GameBase::playSound(%player, SoundFireMortar, 0);
			Client::sendMessage(Player::getClient(%player),0, "Speed Boost Initiated..");	
		}
		
		
	//}
	


	if(!$build)
		Annihilation::decItemCount(%player,%item);
		
	//if(!%player.BeaconTimer)
	//	Beacon::timer(%player);
	//	
	//if(%player.BeaconTimer < 250)
	//	%player.BeaconTimer += 40;
}

function armormWarrior::onRepairKit(%player)
{
	Armor::onRepairKit(%player);
}

PlayerData armorfWarrior
{
	className = "Armor";
	shapeFile = "mfemale";
	flameShapeName = "PLASMATRAIL";	
	shieldShapeName = "shield";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;	
	shadowDetailMask = 1;
	canCrouch = true;
	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
	maxJetSideForceFactor = 0.8;
	maxJetForwardVelocity = 22;
	minJetEnergy = 1;
	jetForce = 325;
	jetEnergyDrain = 1.0;
	maxDamage = 1.0;
	maxForwardSpeed = 10.0;
	maxBackwardSpeed = 8.0;
	maxSideSpeed = 8.0;
	groundForce = 35 * 13.0;
	groundTraction = 4.0;
	maxEnergy = 100;
	mass = 13.0;
	drag = 1.0;
	density = 1.5;
	minDamageSpeed = 25;
	damageScale = 0.005;
	jumpImpulse = 110;
	jumpSurfaceMinDot = 0.2;
	animData[0] = { "root", none, 1, true, true, true, false, 0 };
	animData[1] = { "run", none, 1, true, false, true, false, 3 };
	animData[2] = { "runback", none, 1, true, false, true, false, 3 };
	animData[3] = { "side left", none, 1, true, false, true, false, 3 };
	animData[4] = { "side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
//	animData[7] = { "crouch root", none, 1, true, false, true, false, 3 };
//	animData[8] = { "crouch root", none, 1, true, false, true, false, 3 };
//	animData[9] = { "crouch root", none, -1, true, false, true, false, 3 };
	animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };//holds
	animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[14] = { "fall", none, 1, true, true, true, false, 3 };
	animData[15] = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16] = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17] = { "tumble loop", none, 1, true, false, false, false, 3 };
	animData[18] = { "tumble end", none, 1, true, false, false, false, 3 };
	animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
	animData[21] = { "throw", none, 1, true, false, false, false, 3 };
	
//	animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
//	animData[22] = { "crouch forward", none, 1,  false, false, false, false, 3 };
	animData[22] = { "pose kneel", none, 1, true, false, false, true, 1 };	
//	animData[22] = { "crouch die", none, 1, true, false, false, true, 1 };
//	animData[22] = { "celebration 2", none, 1, false, false, false, false, 2 };	//works
//	animData[22] = { "sign point", none, 1, false, false, false, false, 2 };
	
	animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
	animData[24] = { "apc root", none, 1, false, false, false, false, 3 };
	animData[25] = { "crouch die", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[26] = { "die chest", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[27] = { "die head", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[28] = { "die grab back", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[29] = { "die right side", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[30] = { "die left side", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[31] = { "die leg left", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[32] = { "die leg right", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[33] = { "die blown back", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[34] = { "die spin", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[35] = { "die forward", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[36] = { "die forward kneel", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[37] = { "die back", SoundPlayerDeath, 1, false, false, false, false, 4 };
	animData[38] = { "sign over here", none, 1, true, false, false, false, 2 };
	animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
	animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
	animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
	animData[42] = { "sign salut", none, 1, true, false, false, true, 1 };
	animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };
	animData[50] = { "wave", none, 1, true, false, false, true, 1 };
	jetSound = SoundJetLight;
	rFootSounds = { SoundMFootRSoft, SoundMFootRHard, SoundMFootRSoft, SoundMFootRHard, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRHard, SoundMFootRSnow, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft, SoundMFootRSoft };
	lFootSounds = { SoundMFootLSoft, SoundMFootLHard, SoundMFootLSoft, SoundMFootLHard, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLHard, SoundMFootLSnow, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft, SoundMFootLSoft };
	footPrints = { 2, 3 };
	boxWidth = 0.65;	//0.7
	boxDepth = 0.65;	//0.7
	boxNormalHeight = 2.4;
	boxCrouchHeight = 2.0;
	boxNormalHeadPercentage = 0.84;
	boxNormalTorsoPercentage = 0.55;
	boxHeadLeftPercentage = 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage = 0;
	boxHeadFrontPercentage = 1;
};

function armorfWarrior::onPoison(%client, %player)
{
	if($TALT::Active == false) 
		Armor::onPoison(%client, %player);
}

function armorfWarrior::onBurn(%client, %player)
{
	if($TALT::Active == false) 
		Armor::onBurn(%client, %player);
}

function armorfWarrior::onShock(%client, %player)
{
	if($TALT::Active == false) 
		Armor::onShock(%client, %player);
}

function armorfWarrior::onPlayerContact(%targetPlayer, %sourcePlayer)
{
	Armor::onPlayerContact(%targetPlayer, %sourcePlayer);
}

function armorfWarrior::onGrenade(%player)
{
	if($TALT::Active == false) 
		%obj = newObject("","Mine","Handgrenade");
	else
		%obj = newObject("","Mine","HandgrenadeLT");
		
//	if(%player.isGoated)
//		Item::setVelocity(%player, 0);
//	else
		Armor::ThrowGrenade(%player, %obj);
}

function armorfWarrior::onBeacon(%player, %item)
{
	DebugFun("armormWarrior", %player, %item);
	if ( !isObject(%player) || Player::isDead(%player) || GameBase::getPosition(%player) == "0 0 0" )
		return;

	%vel = Item::getVelocity(%player);

	//if(getsimtime() - %player.LastBeacon > 4.0)
	//	%player.BeaconTimer = 0;	
	
	//%player.LastBeacon = getSimTime();	
		
	//if(%player.BeaconTimer > 80)
	//{
	//	%Pos = getboxcenter(%player); 
	//	%trans =  "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%player); 
	//	Projectile::spawnProjectile("suicideShell", %trans, %player, %vel);
	//	Client::sendMessage(Player::getClient(%player),0, "Your booster exploded!");
	//}
	//else
	//{
		if(%vel == "0 0 0")
		{
			//fixed to send em where they look... not just forward..
			%trans = GameBase::getMuzzleTransform(%player);
			%smack = 300/25;
			%rot=GameBase::getRotation(%player);
			%len = 30;
			%tr= getWord(%trans,5);
			if(%tr <=0 )%tr -=%tr;
			%up = %tr+0.15;
			%out = 1-%tr;
			%vec = Vector::getFromRot(%rot,%len*%out*%smack,%len*%up*%smack);
			if ( ( getWord(%vec,0) > 0 || getWord(%vec,0) < 0 ) && ( getWord(%vec,1) > 0 || getWord(%vec,1) < 0 ) && ( getWord(%vec,2) > 0 || getWord(%vec,2) < 0 ) )
				Item::setVelocity(%player, getWord(%vel,0)+getWord(%vec,0)@" "@getWord(%vel,1)+getWord(%vec,1)@" "@getWord(%vel,2)+getWord(%vec,2) );
			else
			{
				admin::message("ERROR: Booster Vectoring error!  Vec=\""@%vec@"\" from person "@GameBase::getName(Player::getClient(%player))@" ("@%player@")");
				$Admin = "ERROR: Booster Vectoring error!  Vec=\""@%vec@"\" from person "@GameBase::getName(Player::getClient(%player))@" ("@%player@")";
				export("Admin", "config\\Error.log", true);
				return;
			}
			GameBase::playSound(%player, SoundFireMortar, 0);
			Client::sendMessage(Player::getClient(%player),0, "Speed Boost Initiated..");	
		}
		else
		{	
			%vel = Vector::Normalize(%vel);
			%vec = GetWord(%vel, 0) * 300 @ " " @ GetWord(%vel, 1) * 300 @ " " @ GetWord(%vel, 2) * 300;
			Player::applyImpulse(%player, %vec);
			GameBase::playSound(%player, SoundFireMortar, 0);
			Client::sendMessage(Player::getClient(%player),0, "Speed Boost Initiated..");	
		}
		
		
	//}
	


	if(!$build)
		Annihilation::decItemCount(%player,%item);
		
	//if(!%player.BeaconTimer)
	//	Beacon::timer(%player);
	//	
	//if(%player.BeaconTimer < 250)
	//	%player.BeaconTimer += 40;
}

function armorfWarrior::onRepairKit(%player)
{
	Armor::onRepairKit(%player);
}

Armor::add("Warrior", "Warrior", 250);	//  Name, description, price