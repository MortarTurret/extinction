$InvList[Stasis] = 1;
$MobileInvList[Stasis] = 1;
$RemoteInvList[Stasis] = 1;

$AutoUse[Stasis] = False;
$WeaponAmmo[Stasis] = "";

addWeapon(Stasis);

BulletData StasisBolt
{	bulletShapeName = "enbolt.dts";
	explosionTag = energyExp;
	damageClass = 1; //Radius
	damageValue = 0.01;
	damageType = $StasisDamageType;
	explosionRadius = 3;
	muzzleVelocity = 2000.0; 
	totalTime = 10; 
	liveTime =  11; 
	lightRange = 3.0;
	lightColor = { 0.25, 1, 0.25 };
	inheritedVelocityScale = 0.3;
	isVisible = True;
	soundId = SoundJetLight;
};

ItemImageData StasisImage
{
	shapefile = "enex";
	mountPoint = 0;
	weaponType = 0; // Single Shot
//	projectileType = StasisBolt;
	accuFire = true;
//	reloadTime = 1.5;
//	fireTime = 0.5;
	minEnergy = 10;
	maxEnergy = 30;
	lightType = 3;  // Weapon Fire
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 1, 1, 0.2 };
	//sfxFire = SoundFirePlasma;
	sfxActivate = SoundPickUpWeapon;
	//sfxReload = SoundDryFire;
};

ItemData Stasis
{
	description = "Spell: Stasis";
	className = "Weapon";
	shapeFile = "DSPLY_S1";
	hudIcon = "clock";
	heading = $InvHead[ihSpl];
	shadowDetailMask = 4;
	imageType = StasisImage;
	price = 155;
	showWeaponBar = true;
};

function StasisImage::onFire(%player, %slot) 
{		
	if($debug)
		Anni::Echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));	

	if(!%player.Reloading)
	{	
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);
		%player.Reloading = true;		
		schedule(%player @ ".Reloading = false;" , 5.0, %player); 
		GameBase::playSound(%player, SoundFirePlasma, 0);		
		%energy = GameBase::getEnergy(%player);
		gamebase::setenergy(%player,%energy -20);		
		Projectile::spawnProjectile("StasisBolt",%trans,%player,%vel);	
	}
}

function Stasis::MountExtras(%player,%weapon)
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>Freeze enemies in place for 10 seconds. Enemies under stasis are invulnerable and can't move, but can still attack.");
}

function Stasis::resetArmor(%client,%armor)
{
	//schedule("Player::setArmor("@ %damagedClient @", "@ %armor @");",5);
	Player::setArmor(%client,%armor);
	UnstasisMsg(%client);

	%player = Client::getOwnedObject(%client);  // -death666 3.29.17
	%player.frozen = ""; // -death666 3.29.17
	%player.Stasised = false;
}


function UnstasisMsg(%damagedClient)
{
	Client::SendMessage(%damagedClient,1,"You are free from the stasis spell and can now take damage.");
}

PlayerData harmor1
{
	className = "Armor";
	shapeFile = "larmor";
	flameShapeName = "lflame";
	shieldShapeName = "shield";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	shadowDetailMask = 1;
	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
	maxJetSideForceFactor = 0;
	maxJetForwardVelocity = 0;
	minJetEnergy = 0;
	jetForce = 0;
	jetEnergyDrain = 0;
	maxDamage = 1.0;
	maxForwardSpeed = 0.0;
	maxBackwardSpeed = 0.0;
	maxSideSpeed = 0.0;
	groundForce = 35 * 18.0;
	groundTraction = 4.5;
	mass = 18.0;
	maxEnergy = 0;
	drag = 1.0;
	density = 2.5;
	canCrouch = false;
	minDamageSpeed = 25;
	damageScale = 0.006;
	jumpImpulse = 0;
	jumpSurfaceMinDot = 0.2;
	// animation data:
	// animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread
	// movement animations:
	animData[0]  = { "root", none, 1, true, true, true, false, 0 };
	animData[1]  = { "run", none, 1, true, false, true, false, 3 };
	animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
	animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
	animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
	animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
	animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
	animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
	animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
	animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	// misc. animations:
	animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
	animData[21] = { "throw", none, 1, true, false, false, false, 3 };
	animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
	animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
	animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
	// death animations:
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
	// signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
	animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
	animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
	animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
	animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 
	// celebraton animations:
	animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
	// taunt anmations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
	// poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };
	// Bonus wave
	animData[50] = { "wave", none, 1, true, false, false, true, 1 };
	jetSound = SoundJetHeavy;
	rFootSounds = { SoundHFootRSoft, SoundHFootRHard, SoundHFootRSoft, SoundHFootRHard, SoundHFootRSoft, SoundHFootRSoft, SoundHFootRSoft, SoundHFootRHard, SoundHFootRSnow, SoundHFootRSoft, SoundHFootRSoft, SoundHFootRSoft, SoundHFootRSoft, SoundHFootRSoft, SoundHFootRSoft }; 
	lFootSounds = { SoundHFootLSoft, SoundHFootLHard, SoundHFootLSoft, SoundHFootLHard, SoundHFootLSoft, SoundHFootLSoft, SoundHFootLSoft, SoundHFootLHard, SoundHFootLSnow, SoundHFootLSoft, SoundHFootLSoft, SoundHFootLSoft, SoundHFootLSoft, SoundHFootLSoft, SoundHFootLSoft };
	footPrints = { 4, 5 };
	boxWidth = 0.5;
	boxDepth = 0.5;
	boxNormalHeight = 2.2;
	boxCrouchHeight = 1.8;
	boxNormalHeadPercentage = 0.83;
	boxNormalTorsoPercentage = 0.53;
	boxCrouchHeadPercentage = 0.6666;
	boxCrouchTorsoPercentage = 0.3333;
	boxHeadLeftPercentage = 0;
	boxHeadRightPercentage = 1;
	boxHeadBackPercentage = 0;
	boxHeadFrontPercentage = 1;
};

PlayerData harmor2
{
	className = "Armor";
	shapeFile = "marmor";
	flameShapeName = "mflame";
	shieldShapeName = "shield";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	shadowDetailMask = 1;
	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
	maxJetSideForceFactor = 0;
	maxJetForwardVelocity = 0;
	minJetEnergy = 0;
	jetForce = 0;
	jetEnergyDrain = 0;
	maxDamage = 1.0;
	maxForwardSpeed = 0.0;
	maxBackwardSpeed = 0.0;
	maxSideSpeed = 0.0;
	groundForce = 35 * 18.0;
	groundTraction = 4.5;
	mass = 18.0;
	maxEnergy = 0;
	drag = 1.0;
	density = 2.5;
	canCrouch = false;
	minDamageSpeed = 25;
	damageScale = 0.006;
	jumpImpulse = 0;
	jumpSurfaceMinDot = 0.2;
	// animation data:
	// animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread
	// movement animations:
	animData[0]  = { "root", none, 1, true, true, true, false, 0 };
	animData[1]  = { "run", none, 1, true, false, true, false, 3 };
	animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
	animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
	animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
	animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
	animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
	animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
	animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
	animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	// misc. animations:
	animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
	animData[21] = { "throw", none, 1, true, false, false, false, 3 };
	animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
	animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
	animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
	// death animations:
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
	// signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
	animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
	animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
	animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
	animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 
	// celebraton animations:
	animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
	// taunt anmations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
	// poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };
	// Bonus wave
	animData[50] = { "wave", none, 1, true, false, false, true, 1 };
	jetSound = SoundJetHeavy;
	rFootSounds = { SoundHFootRSoft, SoundHFootRHard, SoundHFootRSoft, SoundHFootRHard, SoundHFootRSoft, SoundHFootRSoft, SoundHFootRSoft, SoundHFootRHard, SoundHFootRSnow, SoundHFootRSoft, SoundHFootRSoft, SoundHFootRSoft, SoundHFootRSoft, SoundHFootRSoft, SoundHFootRSoft }; 
	lFootSounds = {	SoundHFootLSoft, SoundHFootLHard, SoundHFootLSoft, SoundHFootLHard, SoundHFootLSoft, SoundHFootLSoft, SoundHFootLSoft, SoundHFootLHard, SoundHFootLSnow, SoundHFootLSoft, SoundHFootLSoft, SoundHFootLSoft, SoundHFootLSoft, SoundHFootLSoft, SoundHFootLSoft };
	footPrints = { 4, 5 };
	boxWidth = 0.8;
	boxDepth = 0.8;
	boxNormalHeight = 2.6;
	boxCrouchHeight = 2.0;
	boxNormalHeadPercentage  = 0.70;
	boxNormalTorsoPercentage = 0.45;
	boxHeadLeftPercentage  = 0.48;
	boxHeadRightPercentage = 0.70;
	boxHeadBackPercentage  = 0.48;
	boxHeadFrontPercentage = 0.60;
};

PlayerData harmor3
{
	className = "Armor";
	shapeFile = "harmor";
	flameShapeName = "hflame";
	shieldShapeName = "shield";
	damageSkinData = "armorDamageSkins";
	debrisId = playerDebris;
	shadowDetailMask = 1;
	visibleToSensor = True;
	mapFilter = 1;
	mapIcon = "M_player";
	maxJetSideForceFactor = 0;
	maxJetForwardVelocity = 0;
	minJetEnergy = 0;
	jetForce = 0;
	jetEnergyDrain = 0;
	maxDamage = 1.0;
	maxForwardSpeed = 0.0;
	maxBackwardSpeed = 0.0;
	maxSideSpeed = 0.0;
	groundForce = 35 * 18.0;
	groundTraction = 4.5;
	mass = 18.0;
	maxEnergy = 0;
	drag = 1.0;
	density = 2.5;
	canCrouch = false;
	minDamageSpeed = 25;
	damageScale = 0.006;
	jumpImpulse = 0;
	jumpSurfaceMinDot = 0.2;
	// animation data:
	// animation name, one shot, exclude, direction,
	// firstPerson, chaseCam, thirdPerson, signalThread
	// movement animations:
	animData[0]  = { "root", none, 1, true, true, true, false, 0 };
	animData[1]  = { "run", none, 1, true, false, true, false, 3 };
	animData[2]  = { "runback", none, 1, true, false, true, false, 3 };
	animData[3]  = { "side left", none, 1, true, false, true, false, 3 };
	animData[4]  = { "side left", none, -1, true, false, true, false, 3 };
	animData[5] = { "jump stand", none, 1, true, false, true, false, 3 };
	animData[6] = { "jump run", none, 1, true, false, true, false, 3 };
	animData[7] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[8] = { "crouch root", none, 1, true, true, true, false, 3 };
	animData[9] = { "crouch root", none, -1, true, true, true, false, 3 };
	animData[10] = { "crouch forward", none, 1, true, false, true, false, 3 };
	animData[11] = { "crouch forward", none, -1, true, false, true, false, 3 };
	animData[12] = { "crouch side left", none, 1, true, false, true, false, 3 };
	animData[13] = { "crouch side left", none, -1, true, false, true, false, 3 };
	animData[14]  = { "fall", none, 1, true, true, true, false, 3 };
	animData[15]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[16]  = { "landing", SoundLandOnGround, 1, true, false, false, false, 3 };
	animData[17]  = { "tumble loop", none, 1, true, false, false, false, 3 };
	animData[18]  = { "tumble end", none, 1, true, false, false, false, 3 };
	animData[19] = { "jet", none, 1, true, true, true, false, 3 };
	// misc. animations:
	animData[20] = { "PDA access", none, 1, true, false, false, false, 3 };
	animData[21] = { "throw", none, 1, true, false, false, false, 3 };
	animData[22] = { "flyer root", none, 1, false, false, false, false, 3 };
	animData[23] = { "apc root", none, 1, true, true, true, false, 3 };
	animData[24] = { "apc pilot", none, 1, false, false, false, false, 3 };
	// death animations:
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
	// signal moves:
	animData[38] = { "sign over here",  none, 1, true, false, false, false, 2 };
	animData[39] = { "sign point", none, 1, true, false, false, true, 1 };
	animData[40] = { "sign retreat",none, 1, true, false, false, false, 2 };
	animData[41] = { "sign stop", none, 1, true, false, false, true, 1 };
	animData[42] = { "sign salut", none, 1, true, false, false, true, 1 }; 
	// celebraton animations:
	animData[43] = { "celebration 1", none, 1, true, false, false, false, 2 };
	animData[44] = { "celebration 2", none, 1, true, false, false, false, 2 };
	animData[45] = { "celebration 3", none, 1, true, false, false, false, 2 };
	// taunt anmations:
	animData[46] = { "taunt 1", none, 1, true, false, false, false, 2 };
	animData[47] = { "taunt 2", none, 1, true, false, false, false, 2 };
	// poses:
	animData[48] = { "pose kneel", none, 1, true, false, false, true, 1 };
	animData[49] = { "pose stand", none, 1, true, false, false, true, 1 };
	// Bonus wave
	animData[50] = { "wave", none, 1, true, false, false, true, 1 };
	jetSound = SoundJetHeavy;
	rFootSounds = { SoundHFootRSoft, SoundHFootRHard, SoundHFootRSoft, SoundHFootRHard, SoundHFootRSoft, SoundHFootRSoft, SoundHFootRSoft, SoundHFootRHard, SoundHFootRSnow, SoundHFootRSoft, SoundHFootRSoft, SoundHFootRSoft, SoundHFootRSoft, SoundHFootRSoft, SoundHFootRSoft }; 
	lFootSounds = { SoundHFootLSoft, SoundHFootLHard, SoundHFootLSoft, SoundHFootLHard, SoundHFootLSoft, SoundHFootLSoft, SoundHFootLSoft, SoundHFootLHard, SoundHFootLSnow, SoundHFootLSoft, SoundHFootLSoft, SoundHFootLSoft, SoundHFootLSoft, SoundHFootLSoft, SoundHFootLSoft };
	footPrints = { 4, 5 };
	boxWidth = 0.8;
	boxDepth = 0.8;
	boxNormalHeight = 2.6;
	boxCrouchHeight = 2.0;
	boxNormalHeadPercentage  = 0.70;
	boxNormalTorsoPercentage = 0.45;
	boxHeadLeftPercentage  = 0.48;
	boxHeadRightPercentage = 0.70;
	boxHeadBackPercentage  = 0.48;
	boxHeadFrontPercentage = 0.60;
};


