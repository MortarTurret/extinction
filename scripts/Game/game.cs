if($TALT::Stripped)
	$ModVersion = "TA -Stripped";	//9/6/2003 7:51AM		// BETA"; //TA Setting
else
	$ModVersion = "Total Annihilation"; 
$betaBuildNumber = "";			//"11.02.02";

$SensorNetworkEnabled = true;

$GuiModePlay = 1;
$GuiModeCommand = 2;
$GuiModeVictory = 3;
$GuiModeInventory = 4;
$GuiModeObjectives = 5;
$GuiModeLobby = 6;
$timeLimitWarned = false;


// Global Variables

$MODInfo = $Server::MODInfo;
//$boostStr = 0.03;

$ClassAGen[0] = "";
$ClassAGen[1] = "";

$ClassBGen[0] = "";
$ClassBGen[1] = "";

$GenSet[0] = "";
$GenSet[1] = "";

//$Alarm[0] = false;
//$Alarm[1] = false;

//---------------------------------------------------------------------------------
// Energy each team is given at beginning of game
//---------------------------------------------------------------------------------
$DefaultTeamEnergy = "Infinite";

//---------------------------------------------------------------------------------
// Energy each team is given at beginning of game
//---------------------------------------------------------------------------------
$DefaultTeamEnergy = $Spoonbot::DefaultTeamEnergy;

//---------------------------------------------------------------------------------
// Team Energy variables
//---------------------------------------------------------------------------------
$TeamEnergy[-1] = $DefaultTeamEnergy;
$TeamEnergy[0] = $DefaultTeamEnergy;
$TeamEnergy[1] = $DefaultTeamEnergy;
$TeamEnergy[2] = $DefaultTeamEnergy;
$TeamEnergy[3] = $DefaultTeamEnergy;
$TeamEnergy[4] = $DefaultTeamEnergy;
$TeamEnergy[5] = $DefaultTeamEnergy;
$TeamEnergy[6] = $DefaultTeamEnergy;
$TeamEnergy[7] = $DefaultTeamEnergy;

//---------------------------------------------------------------------------------
// Time in sec player must wait before he can throw a Grenade or Mine after leaving
//	a station.
//---------------------------------------------------------------------------------
$WaitThrowTime = 2;

//---------------------------------------------------------------------------------
// If 1 then Team Spending Ignored -- Team Energy is set to $MaxTeamEnergy every
// 	$secTeamEnergy.
//---------------------------------------------------------------------------------
$TeamEnergyCheat = 0;

//---------------------------------------------------------------------------------
// MAX amount team energy can reach
//---------------------------------------------------------------------------------
$MaxTeamEnergy = 700000;

//---------------------------------------------------------------------------------
// Time player has to put flag in flagstand before it gets returned to its last
// location. 
//---------------------------------------------------------------------------------
$flagToStandTime = 180;

//---------------------------------------------------------------------------------
// Amount to inc team energy every ($secTeamEnergy) seconds
//---------------------------------------------------------------------------------
$incTeamEnergy = 700;

//---------------------------------------------------------------------------------
// (Rate is sec's) Set how often TeamEnergy is incremented
//---------------------------------------------------------------------------------
$secTeamEnergy = 30;

//---------------------------------------------------------------------------------
// (Rate is sec's) Items respwan
//---------------------------------------------------------------------------------
$ItemRespawnTime = 30;

//---------------------------------------------------------------------------------
//Amount of Energy remote stations start out with
//---------------------------------------------------------------------------------
$RemoteAmmoEnergy = 2500;
$RemoteInvEnergy = 3000;

//---------------------------------------------------------------------------------
// TEAM ENERGY - Warn team when teammate has spent x amount - Warn team that
//	energy level is low when it reaches x amount 
//---------------------------------------------------------------------------------
$TeammateSpending = -4000; //Set = to 0 if don't want the warning message
$WarnEnergyLow = 4000; //Set = to 0 if don't want the warning message

//---------------------------------------------------------------------------------
// Amount added to TeamEnergy when a player joins a team
//---------------------------------------------------------------------------------
$InitialPlayerEnergy = 5000;

//---------------------------------------------------------------------------------
// REMOTE TURRET
//---------------------------------------------------------------------------------
$MaxNumTurretsInBox = 6; //Number of remote turrets allowed in the area
$TurretBoxMaxLength = 35; //Define Max Length of the area
$TurretBoxMaxWidth = 35; //Define Max Width of the area
$TurretBoxMaxHeight = 25; //Define Max Height of the area

$TurretBoxMinLength = 5; //Define Min Length from another turret
$TurretBoxMinWidth = 5; //Define Min Width from another turret
$TurretBoxMinHeight = 5; //Define Min Height from another turret

//---------------------------------------------------------------------------------
//	Object Types	
//---------------------------------------------------------------------------------
$SimTerrainObjectType = 1 << 1;
$SimInteriorObjectType = 1 << 2;
$SimPlayerObjectType = 1 << 7;

$MineObjectType = 1 << 26;
$MoveableObjectType = 1 << 22;
$VehicleObjectType = 1 << 29;
$StaticObjectType = 1 << 23;
$ItemObjectType = 1 << 21;

//---------------------------------------------------------------------------------
// CHEATS
//---------------------------------------------------------------------------------
$ServerCheats = 0;
$TestCheats = 0;

//---------------------------------------------------------------------------------
//Respawn automatically after X sec's - If 0..no respawn
//---------------------------------------------------------------------------------
$AutoRespawn = 0;

//---------------------------------------------------------------------------------
// Player death messages - %1 = killer's name, %2 = victim's name
//		 %3 = killer's gender pronoun (his/her), %4 = victim's gender pronoun
//---------------------------------------------------------------------------------
//Fixed for 1.40 built in stats
$deathMsg[$LandingDamageType, 0] = "%1 falls to %2 death.";
$deathMsg[$LandingDamageType, 1] = "%1 forgot to tie %2 bungie cord.";
$deathMsg[$LandingDamageType, 2] = "%1 bites the dust in a forceful manner.";
$deathMsg[$LandingDamageType, 3] = "%1 thought the ground looked softer.";
$deathMsg[$LandingDamageType, 4] = "%1 leaves a big ugly crater.";
$deathMsg[$ImpactDamageType, 0] = "%1 makes quite an impact on %2.";
$deathMsg[$ImpactDamageType, 1] = "%2 becomes the victim of a fly-by from %1.";
$deathMsg[$ImpactDamageType, 2] = "%2 leaves a nasty dent in %1's fender.";
$deathMsg[$ImpactDamageType, 3] = "%1 says, 'Hey %2, you scratched my paint job!'";
$deathMsg[$ImpactDamageType, 4] = "%2 didn't get out of %1's way.";
$deathMsg[$BulletDamageType, 0] = "%2 got regulated by %1.";
$deathMsg[$BulletDamageType, 1] = "%1 gives %2 an overdose of lead.";
$deathMsg[$BulletDamageType, 2] = "%1 fills %2 full of holes.";
$deathMsg[$BulletDamageType, 3] = "%1 guns down %2.";
$deathMsg[$BulletDamageType, 4] = "%1 gave %2 a hole in %3 head that wasn't there before.";
$deathMsg[$MinigunDamageType, 0] = "%2 got regulated by %1.";
$deathMsg[$MinigunDamageType, 1] = "%1 gives %2 an overdose of lead.";
$deathMsg[$MinigunDamageType, 2] = "%1 fills %2 full of holes.";
$deathMsg[$MinigunDamageType, 3] = "%1 guns down %2.";
$deathMsg[$MinigunDamageType, 4] = "%1 gave %2 a hole in %3 head that wasn't there before.";
$deathMsg[$EnergyDamageType, 0] = "%2 dies of turret trauma.";
$deathMsg[$EnergyDamageType, 1] = "%2 is chewed to pieces by a turret.";
$deathMsg[$EnergyDamageType, 2] = "%2 walks into a stream of turret fire.";
$deathMsg[$EnergyDamageType, 3] = "%2 ends up on the wrong side of a turret.";
$deathMsg[$EnergyDamageType, 4] = "%2 finds the business end of a turret.";

$deathMsg[$PlasmaDamageType, 0] = "%2 feels the warm glow of %1's plasma.";
$deathMsg[$PlasmaDamageType, 1] = "%1 gives %2 a white-hot plasma injection.";
$deathMsg[$PlasmaDamageType, 2] = "%1 asks %2, 'Got plasma?'";
$deathMsg[$PlasmaDamageType, 3] = "%1 gives %2 a burning itch.";
$deathMsg[$PlasmaDamageType, 4] = "%2 feels the warm glow of %1's plasma."; 

$deathMsg[$TrollPlasmaDamageType, 0] = "%2 smelled the warm stench of %1.";
$deathMsg[$TrollPlasmaDamageType, 1] = "%1 gives %2 a white-hot plasma injection.";
$deathMsg[$TrollPlasmaDamageType, 2] = "%1 asks %2, 'Got Troll?'";
$deathMsg[$TrollPlasmaDamageType, 3] = "%1 gives %2 the troll flu.";
$deathMsg[$TrollPlasmaDamageType, 4] = "%1 gives %2 a white-hot plasma injection."; 

$deathMsg[$ExplosionDamageType, 0] = "%2 catches a Frisbee of Death thrown by %1.";
$deathMsg[$ExplosionDamageType, 1] = "%1 blasts %2 with a well-placed disc.";
$deathMsg[$ExplosionDamageType, 2] = "%1's spinfusor caught %2 by surprise.";
$deathMsg[$ExplosionDamageType, 3] = "%2 falls victim to %1's Stormhammer.";
$deathMsg[$ExplosionDamageType, 4] = "%1 shows off %3 mad skills of body dismemberment on %2.";
$deathMsg[$ShrapnelDamageType, 0] = "%1 blows %2 up real good.";
$deathMsg[$ShrapnelDamageType, 1] = "%2 gets a taste of %1's explosive temper.";
$deathMsg[$ShrapnelDamageType, 2] = "%1 gives %2 a fatal concussion.";
$deathMsg[$ShrapnelDamageType, 3] = "%2 never saw it coming from %1.";
$deathMsg[$ShrapnelDamageType, 4] = "%2 saw the bad side of %1.";


$deathMsg[$LaserDamageType, 0] = "%1 adds %2 to %3 list of sniper victims.";
$deathMsg[$LaserDamageType, 1] = "%1 fells %2 with a sniper shot.";
$deathMsg[$LaserDamageType, 2] = "%2 was assassinated by %1.";
$deathMsg[$LaserDamageType, 3] = "%2 stayed in %1's crosshairs for too long.";
$deathMsg[$LaserDamageType, 4] = "%2 was assassinated by %1."; //%2 gets another hole in %3 head. 
$deathMsg[$MortarDamageType, 0] = "%1 explodes %2 into oblivion.";
$deathMsg[$MortarDamageType, 1] = "%2 found %1's silly putty.";
$deathMsg[$MortarDamageType, 2] = "%1 placed the explosives where %2 could find them.";
$deathMsg[$MortarDamageType, 3] = "%1's bomb takes out %2.";
$deathMsg[$MortarDamageType, 4] = "%1's bomb takes out %2"; //%2 falls all to pieces. 

$deathMsg[$BlasterDamageType, 0] = "%2 gets a blast out of %1.";
$deathMsg[$BlasterDamageType, 1] = "%2 succumbs to %1's rain of blaster fire.";
$deathMsg[$BlasterDamageType, 2] = "%1's puny blaster shows %2 a new world of pain.";
$deathMsg[$BlasterDamageType, 3] = "%2 meets %1's master blaster.";
$deathMsg[$BlasterDamageType, 4] = "%1's punks %2 ghetto style.";
$deathMsg[$ElectricityDamageType, 0] = "%2 gets zapped with %1's ELF gun.";
$deathMsg[$ElectricityDamageType, 1] = "%1 gives %2 a nasty jolt.";
$deathMsg[$ElectricityDamageType, 2] = "%2 gets a real shock out of meeting %1.";
$deathMsg[$ElectricityDamageType, 3] = "%1 short-circuits %2's systems.";
$deathMsg[$ElectricityDamageType, 4] = "%2 gets a real shock out of meeting %1."; 
//$SoulDamageType
$deathMsg[$SoulDamageType, 0] = "%2 Gets %4 soul pulled out by %1.";
$deathMsg[$SoulDamageType, 1] = "%1 pulled a rabbit out of %2's head.";
$deathMsg[$SoulDamageType, 2] = "%1 sent %2's soul to Odin.";
$deathMsg[$SoulDamageType, 3] = "%2 dies for %1's sins.";
$deathMsg[$SoulDamageType, 4] = "%1 fried up %2's soul cajun style.";
$deathMsg[$CrushDamageType, 0] = "%2 didn't stay away from the moving parts.";
$deathMsg[$CrushDamageType, 1] = "%2 is crushed.";
$deathMsg[$CrushDamageType, 2] = "%2 gets smushed flat.";
$deathMsg[$CrushDamageType, 3] = "%2 gets caught in the machinery.";
$deathMsg[$CrushDamageType, 4] = "%2 is sat on by %1's mom.";
$deathMsg[$DebrisDamageType, 0] = "%2 is a victim among the wreckage.";
$deathMsg[$DebrisDamageType, 1] = "%2 is killed by debris.";
$deathMsg[$DebrisDamageType, 2] = "%2 becomes a victim of collateral damage.";
$deathMsg[$DebrisDamageType, 3] = "%2 got too close to the exploding stuff.";
$deathMsg[$DebrisDamageType, 4] = "%2 feels the rain of debris.";
$deathMsg[$MissileDamageType, 0] = "%2 takes a missile up the keister.";
$deathMsg[$MissileDamageType, 1] = "%2 gets shot down.";
$deathMsg[$MissileDamageType, 2] = "%2 gets real friendly with a rocket.";
$deathMsg[$MissileDamageType, 3] = "%2 feels the burn from a warhead.";
$deathMsg[$MissileDamageType, 4] = "%2 rides the rocket.";

$deathMsg[$RocketDamageType, 0]	    = "%2 gets a hot rocket injection from %1.";
$deathMsg[$RocketDamageType, 1]	    = "%1 gives %2 a lesson in rocketry.";
$deathMsg[$RocketDamageType, 2]	    = "%2 polishes %1's rocket.";
$deathMsg[$RocketDamageType, 3]	    = "%2 rides %1's big one.";
$deathMsg[$RocketDamageType, 4]	    = "%2 failed to outrun %1's rocket.";

$deathMsg[$OSMissileDamageType, 0]	    = "%2 gets a hot rocket injection from %1.";
$deathMsg[$OSMissileDamageType, 1]	    = "%1 gives %2 a lesson in rocketry.";
$deathMsg[$OSMissileDamageType, 2]	    = "%2 polishes %1's rocket.";
$deathMsg[$OSMissileDamageType, 3]	    = "%2 rides %1's big one.";
$deathMsg[$OSMissileDamageType, 4]	    = "%2 failed to outrun %1's rocket.";
$deathMsg[$MineDamageType, 0] = "%2 gets a taste of %1's explosive temper."; 
$deathMsg[$MineDamageType, 1] = "%2 gets a taste of %1's explosive temper.";
$deathMsg[$MineDamageType, 2] = "%1 gives %2 a fatal concussion.";
$deathMsg[$MineDamageType, 3] = "%2 never saw it coming from %1.";
$deathMsg[$MineDamageType, 4] = "%2 stepped in %1's cow pie. Mooooo!";
$deathMsg[$SniperDamageType, 0] = "%1 adds %2 to %3 list of sniper victims.";
$deathMsg[$SniperDamageType, 1] = "%1 fells %2 with a shot between the eyes.";
$deathMsg[$SniperDamageType, 2] = "%2 was assassinated by %1.";
$deathMsg[$SniperDamageType, 3] = "%2 stayed in %1's crosshairs for too long.";
$deathMsg[$SniperDamageType, 4] = "%2 gets picked off from afar by %1.";
$deathMsg[$ShockDamageType, 0] = "%1 blows %2 up real good.";
$deathMsg[$ShockDamageType, 1] = "%2 gets a taste of %1's explosive temper.";
$deathMsg[$ShockDamageType, 2] = "%1 gives %2 a fatal concussion.";
$deathMsg[$ShockDamageType, 3] = "%2 never saw it coming from %1.";
$deathMsg[$ShockDamageType, 4] = "%2 gets flashed by %1.";
$deathMsg[$ShotgunDamageType, 0] = "%2 caught a chest full of %1's shotgun blast.";
$deathMsg[$ShotgunDamageType, 1] = "%1 filled %2 full of 00-Buckshot.";
$deathMsg[$ShotgunDamageType, 2] = "%1 relieved %2's constipation with a shotgun blast.";
$deathMsg[$ShotgunDamageType, 3] = "%2 gets pelted with %1's rock salt.";
$deathMsg[$ShotgunDamageType, 4] = "%2 gets %3 guts blown out by %1's shotgun.";
$deathMsg[$AssassinDamageType, 0] = "%2 is assassinated by %1.";
$deathMsg[$AssassinDamageType, 1] = "%2 gets %3 throat cut by %1.";
$deathMsg[$AssassinDamageType, 2] = "%2 gets stabbed in the back by %1.";
$deathMsg[$AssassinDamageType, 3] = "%2 didn't see it coming from %1.";
$deathMsg[$AssassinDamageType, 4] = "%1 slices %2's throat.";
$deathMsg[$DisarmDamageType, 0] = "%1 removes %2's weapon for the last time.";
$deathMsg[$DisarmDamageType, 1] = "%2 is embarrased by %1.";
$deathMsg[$DisarmDamageType, 2] = "%1 removes %2's weapon for the last time."; 
$deathMsg[$DisarmDamageType, 3] = "%1 removes %2's weapon for the last time."; 
$deathMsg[$DisarmDamageType, 4] = "%2 is embarrased by %1.";
$deathMsg[$StasisDamageType, 0] = "%1 told %2 to stand still.";
$deathMsg[$StasisDamageType, 1] = "%1 told %2 not to move.";
$deathMsg[$StasisDamageType, 2] = "%1 told %2 to stand still."; //%2 is frozen in %4 tracks. 
$deathMsg[$StasisDamageType, 3] = "%1 told %2 not to move."; //%2 died where %4 stood. 
$deathMsg[$StasisDamageType, 4] = "%1 plays Mr.Freeze on %2.";
$deathMsg[$VortexTurretDamageType, 0] = "%2 gets sucked in by %1's turret.";
$deathMsg[$VortexTurretDamageType, 1] = "%2 is vortexed to another dimension by %1.";
$deathMsg[$VortexTurretDamageType, 2] = "%2 is vortexed to another dimension by %1."; 
$deathMsg[$VortexTurretDamageType, 3] = "%1 sends %2 to a better place.";
$deathMsg[$VortexTurretDamageType, 4] = "%1 shows %2 a wormhole.";
$deathMsg[$PoisonDamageType, 0] = "%2 dies of turret trauma.";
$deathMsg[$PoisonDamageType, 1] = "%2 is chewed to pieces by a turret.";
$deathMsg[$PoisonDamageType, 2] = "%2 walks into a stream of turret fire.";
$deathMsg[$PoisonDamageType, 3] = "%2 ends up on the wrong side of a turret.";
$deathMsg[$PoisonDamageType, 4] = "%2 finds the business end of a turret.";
$deathMsg[$JailDamageType, 0] = "%2 is incarcerated by %1.";
$deathMsg[$JailDamageType, 1] = "%2 is incarcerated by %1.";
$deathMsg[$JailDamageType, 2] = "%2 is incarcerated by %1.";
$deathMsg[$JailDamageType, 3] = "%2 is incarcerated by %1.";
$deathMsg[$JailDamageType, 4] = "%2 is incarcerated by %1.";

// changed TurretVortexDamageType messages to VortexTurretDamageType -death666

// new for 2.2 -PitchFork damage type
$deathMsg[$ForkImpact, 0] = "%1 forked %2 off a cliff.";
$deathMsg[$ForkImpact, 1] = "%1 threw %2 against a wall.";
$deathMsg[$ForkImpact, 2] = "%1 won the midget toss with %2.";
$deathMsg[$ForkImpact, 3] = "%1 put %2's head through a wall.";
$deathMsg[$ForkImpact, 4] = "%2 visited a hard object, courtesy of %1.";

// new for 2.2 -jetting damage type
// Player death messages - %1 = killer's name, %2 = victim's name
//		 %3 = killer's gender pronoun (his/her), %4 = victim's gender pronoun
$deathMsg[$JettingDamage, 0] = "%1 got burned up by %2 jets.";
$deathMsg[$JettingDamage, 1] = "%1's energy system exploded.";
$deathMsg[$JettingDamage, 2] = "%1's jets got turned inside out.";
$deathMsg[$JettingDamage, 3] = "%1 forgot %2 energy system was broken.";
$deathMsg[$JettingDamage, 4] = "%1 was killed by %2 own jump jets.";

// "you just killed yourself" messages
// %1 = player name, %2 = player gender pronoun

$deathMsg[-2,0] = "%1 checked into the flatline hotel.";
$deathMsg[-2,1] = "%1 takes a dirt nap.";
$deathMsg[-2,2] = "%1 kills %2 own dumb self.";
$deathMsg[-2,3] = "%1 takes a 6 foot holiday.";	
$deathMsg[-2,4] = "%1 goes for the quick and dirty respawn.";

$numDeathMsgs = 5;
//---------------------------------------------------------------------------------

function remotePlayMode(%clientId)
{
	if ( CheckEval("remotePlayMode", %clientId) )
		return;
		
	if(!%clientId.guiLock)
	{
		remoteSCOM(%clientId, -1);
		Client::setGuiMode(%clientId, $GuiModePlay);		
		
		%owned = Client::getOwnedObject(%clientId);
		%vehicle = %owned.vehicle;
		if(%vehicle && GameBase::getDataName(%vehicle)==OSMissile)	
			Client::setControlObject(%clientId,%vehicle);
	}
}

function remoteCommandMode(%clientId)
{
	if ( CheckEval("remoteCommandMode", %clientId) )
		return;
		
	%player = Client::getOwnedObject(%clientId);
	if(%player.frozen) return;
	
	// can't switch to command mode while a server menu is up
	if(%clientId.droid)
		return;
	if(!%clientId.guiLock)
	{
		remoteSCOM(%clientId, -1); // force the bandwidth to be full command
		if(%clientId.observerMode != "pregame")
			checkControlUnmount(%clientId);
		Client::setGuiMode(%clientId, $GuiModeCommand);
		if(!%clientId.isBlackOut)
		bottomprint(%clientId, "<jc><f1>CLEAR!", 0.01);
	}
}

function remoteInventoryMode(%clientId)
{
	if ( CheckEval("remoteInventoryMode", %clientId) || %clientId.observerMode != "" )
		return;
		
	%player = Client::getOwnedObject(%clientId);
	if(%player == -1 || %player == "" || %player.frozen) return;
	
		
	if((!%clientId.guiLock && !Observer::isObserver(%clientId)) && %clientId.AdminobserverMode != "AdminObserve")
	{
		if(!GameBase::isPowered(%player.InvObject) || GameBase::getDamageState(%player.InvObject) != "Enabled")
		{
			%clientId.ConnectBeam = "";	//internal
			%clientId.InvTargetable = "";	//internal
			%ClientId.InvConnect = "";	//external	
			QuickInvOff(%clientId);	
			%objectId.ZappyResupply = "";
			%clientId.ListType = "";
		}
		centerprint(%clientId, "", 0.01);
		remoteSCOM(%clientId, -1);
		Client::clearItemShopping(%clientId);
		Client::setGuiMode(%clientId, $GuiModeInventory);
		if(($build || $Annihilation::QuickInv || %ClientId.InvConnect) && !Player::getItemCount(%player, GhostArmor))
		{
			QuickInv(%clientId);
			if($build || $Annihilation::QuickInv)	
				bottomprintall("<jc><f2>Global Inventories enabled. Toggle Inventory screen to access!!");
		}
		if(!%clientId.isBlackOut)
		bottomprint(%clientId, "<jc><f1>CLEAR!", 0.01);
	}

}

function remoteObjectivesMode(%clientId)
{
	if ( CheckEval("remoteObjectivesMode", %clientId) )
		return;
		
	%player = Client::getOwnedObject(%clientId);
	if(%player.frozen) return;
		
	if(!%clientId.guiLock)
	{
		remoteSCOM(%clientId, -1);
		Client::setGuiMode(%clientId, $GuiModeObjectives);
	}
}

function remoteScoresOn(%clientId)
{
	if ( CheckEval("remoteScoresOn", %clientId) )
		return;
		
	if(!%clientId.menuMode)
	{
		Game::menuRequest(%clientId);
		if(!%clientId.isBlackOut)
			bottomprint(%clientId, "<jc><f1>CLEAR!", 0.01);
	}
}

function remoteScoresOff(%clientId)
{
	if ( CheckEval("remoteScoresOff", %clientId) )
		return;
	Client::cancelMenu(%clientId);
	if(!%clientId.isBlackOut)
		bottomprint(%clientId, "<jc><f1>CLEAR!", 0.01);
}

function remoteToggleCommandMode(%clientId)
{
	if ( CheckEval("remoteToggleCommandMode", %clientId) )
		return;
		
	if(%clientId.AdminobserverMode == "AdminObserve")  Observer::jump(%clientId);

	%player = Client::getOwnedObject(%clientId);
	if(%player.frozen) return;
		
	if(Client::getGuiMode(%clientId) != $GuiModeCommand)
		remoteCommandMode(%clientId);
	else
		remotePlayMode(%clientId);
}

function remoteToggleInventoryMode(%clientId)
{
	if ( CheckEval("remoteToggleInventoryMode", %clientId) )
		return;
		
	%player = Client::getOwnedObject(%clientId);
	if(%player.frozen) return;
		
	if(Client::getGuiMode(%clientId) != $GuiModeInventory)
		remoteInventoryMode(%clientId);
	else	
		remotePlayMode(%clientId);
}

function remoteToggleObjectivesMode(%clientId)
{
	if ( CheckEval("remoteToggleObjectivesMode", %clientId) )
		return;
		

	if(%clientId.AdminobserverMode == "AdminObserve")  Observer::jump(%clientId);

	%player = Client::getOwnedObject(%clientId);
	if(%player.frozen) return;
		
	if(Client::getGuiMode(%clientId) != $GuiModeObjectives)
		remoteObjectivesMode(%clientId);
	else
		remotePlayMode(%clientId);
}

function Time::getMinutes(%simTime)
{
	return floor(%simTime / 60);
}

function Time::getSeconds(%simTime)
{
	return %simTime % 60;
}

function Game::pickRandomSpawn(%team)
{
	%group = nameToID("MissionGroup/Teams/team" @ %team @ "/DropPoints/Random");
	%count = Group::objectCount(%group);
	if(!%count)
		return -1;
	%spawnIdx = floor(getRandom() * (%count - 0.1));
	%value = %count;
	for(%i = %spawnIdx; %i < %value; %i++)
	{
		%set = newObject("set",SimSet);
		%obj = Group::getObject(%group, %i);
		if(containerBoxFillSet(%set,$SimPlayerObjectType|$VehicleObjectType,GameBase::getPosition(%obj),2,2,4,0) == 0)
		{
			deleteObject(%set);
			return %obj;
		}
		if(%i == %count - 1)
		{
			%i = -1;
			%value = %spawnIdx;
		}
		deleteObject(%set);
	}
	return false;
}

function Game::pickRandomTeleporter(%team)
{
	%group = nameToID("MissionCleanup/Teleports");
	%count = Group::objectCount(%group);
	if(!%count)
		return -1;
	%spawnIdx = floor(getRandom() * (%count - 0.1));
	%value = %count;
	for(%i = %spawnIdx; %i < %value; %i++)
	{
		%set = newObject("set",SimSet);
		%obj = Group::getObject(%group, %i);
		if(containerBoxFillSet(%set,$SimPlayerObjectType|$VehicleObjectType,GameBase::getPosition(%obj),2,2,4,0) == 0)
		{
			deleteObject(%set);
			return %obj;
		}
		if(%i == %count - 1)
		{
			%i = -1;
			%value = %spawnIdx;
		}
		deleteObject(%set);
	}
	return false;
}

function Game::pickStartSpawn(%team)
{
	%group = nameToID("MissionGroup\\Teams\\team" @ %team @ "\\DropPoints\\Start");
	%count = Group::objectCount(%group);
	if(!%count)
		return -1;
	%spawnIdx = $lastTeamSpawn[%team] + 1;
	if(%spawnIdx >= %count)
		%spawnIdx = 0;
	$lastTeamSpawn[%team] = %spawnIdx;
	return Group::getObject(%group, %spawnIdx);
}

function Game::pickTeamSpawn(%team, %respawn)
{
	if(%respawn)
		return Game::pickRandomSpawn(%team);
	else
	{
		%spawn = Game::pickStartSpawn(%team);
		if(%spawn == -1)
			return Game::pickRandomSpawn(%team);
		return %spawn;
	}
}

function Game::pickObserverSpawn(%client)
{
	%group = nameToID("MissionGroup\\ObserverDropPoints");
	%count = Group::objectCount(%group);
	if(%group == -1 || !%count)
		%group = nameToID("MissionGroup\\Teams\\team" @ Client::getTeam(%client) @ "\\DropPoints\\Random");
	%count = Group::objectCount(%group);
	if(%group == -1 || !%count)
		%group = nameToID("MissionGroup\\Teams\\team0\\DropPoints\\Random");
	%count = Group::objectCount(%group);
	if(%group == -1 || !%count)
		return -1;
	%spawnIdx = %client.lastObserverSpawn + 1;
	if(%spawnIdx >= %count)
		%spawnIdx = 0;
	%client.lastObserverSpawn = %spawnIdx;
	return Group::getObject(%group, %spawnIdx);
}

function UpdateClientTimes(%time)
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		remoteEval(%cl, "setTime", -%time);
}

function Game::notifyMatchStart(%time)
{
	messageAll(0, "Match starts in " @ %time @ " seconds. ~wshell_click.wav"); // adding this -death666 3.16.17
	messageAll(1, "Press Tab to join teams."); // adding this -death666 3.16.17
//	bottomprintall("<jc><f2>Press Tab to join teams.", 10); // adding this -death666 3.16.17
	$TA::TeamLock = false; // -death666 4.3.17 moved here
	UpdateClientTimes(%time);
}

function Game::PopupSpoonbotMessage(%text)
{
	if (!isObject(Notice))
		newObject(Notice, FearGui::FearGuiBox, %width - 250,%height - 157, 200,112);
	if (!isObject(NoticeText))
		newObject(NoticeText, FearGuiFormattedText, 1,0,190,400);
	AddToSet(Notice, NoticeText);
	AddToSet(MainMenuGui, Notice);
	Control::SetValue(NoticeText, %text);
}

function Game::RemoveSpoonbotMessage()
{
	%text = " ";
	Control::SetValue(NoticeText, %text);

	deleteObject(NoticeText);
	deleteObject(Notice);

}

function Game::startMatch()
{
	$TowerSwitchNexus = "";
	$FlagHunter::Enabled = false;	
	$FlagHunter::HoardMode = false;
	$FlagHunter::GreedMode = false;
	
	$matchStarted = true;
	$missionStartTime = getSimTime();
	messageAll(1, "Match started! ~wDoor2.wav"); // adding this -death666 3.16.17
	$BotsArenaCount = "0";
	bottomprintall("<jc><f2>Match started!", 10); // adding this -death666 3.16.17
	Game::resetScores();

//	exec("ai");

    BotTree::Init_Tree();		//Initialise Tree Matrix
	BotPilot::Init_Waypoints();
	Anni::Echo("Ignore unknown command this is needed for botpilots.");
	Anni::Echo("Total Annihilation Server Initiated.");
	
	if ($Spoonbot::AutoSpawn)
	{
			// BotFuncs::ScanObjectTree(); //Initialise Bot Targets
			$BotFuncs::flagcount[0] = 0;
			$BotFuncs::flagcount[1] = 0;
			$BotFuncs::sensorcount[0] = 0;
			$BotFuncs::sensorcount[1] = 0;
			$BotFuncs::guncount[0] = 0;
			$BotFuncs::guncount[1] = 0;
			$BotFuncs::gencount[0] = 0;
			$BotFuncs::gencount[1] = 0;
			$BotFuncs::comcount[0] = 0;
			$BotFuncs::comcount[1] = 0;
			$BotFuncs::vehcount[0] = 0;
			$BotFuncs::vehcount[1] = 0;
			$BotFuncs::invcount[0] = 0;
			$BotFuncs::invcount[1] = 0;
			$BotFuncs::switchcount = 0;
			$BotFuncs::AllCount = 0;

//			Arena::Clear();			
	}

   AI::ProcessAutoSpawn();                               //Spawn all the bots defined in SPOONBOT.CS
   	$ScanjObjTree = true;

	if($TA::MuteAllTourney) //added to get rid of mute at the start of the map 
	{
		echo("startmatch!!");
		$TA::MuteAllTourney = false;
		TA::MuteAllTourney("unmute");
	}

	if($TALT::Active)
	{ //moved this here so we get the love on early
		//TA::FlagReset();
		// Team Parameters // lets go ahead and just make sure we get proper team names for LT in TA
		$Server::teamName[-1] = "Obs";		// Observer Name
		$Server::teamName[0] = "BE";		// Team 1 Name
		$Server::teamSkin[0] = "beagle";			// Team 1 Skin
		$Server::teamName[1] = "DS";	// Team 2 Name
		$Server::teamSkin[1] = "dsword";			// Team 2 Skin
		
		if($TALT::SpawnType == "AnniSpawn") //what mod are we playing?
			$Server::RespawnTime = 0;
		else if($TALT::SpawnType == "EliteSpawn")
			$Server::RespawnTime = 1;
		else if($TALT::SpawnType == "BaseSpawn")
			$Server::RespawnTime = 1;
	}
	else
		$Server::RespawnTime = 1;

	if($Annihilation::BaseHeal) AutoRepair(0.003);
	
	%numTeams = getNumTeams()-1;
	for(%i = 0; %i < %numTeams; %i = %i + 1)
	{
		if($TeamEnergy[%i] != "Infinite")
			schedule("replenishTeamEnergy(" @ %i @ ");", $secTeamEnergy);
	}
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.observerMode == "pregame")
		{
			%cl.observerMode = "";
			Client::setControlObject(%cl, Client::getOwnedObject(%cl));
		}
		%cl.lastActiveTimestamp = getSimTime(); 
		Game::refreshClientScore(%cl);
	}
	Game::checkTimeLimit();

	if(getNumTeams()-1 == 2)
	{
	%teamnumzero = 0;
	%teamnummone = 1;

	$DropShipMultipass1[%teamnumzero] = "false";
	$DropShipMultipass2[%teamnumzero] = "false";

	$DropShipMultipass1[%teamnummone] = "false";
	$DropShipMultipass2[%teamnummone] = "false";

	$PortableGeneratedPower[%teamnumzero] = "false";
	$PortableGeneratedPower[%teamnummone] = "false";

	$PortableSP[%teamnummone] = "false";
	$PortableSP[%teamnumzero] = "false";
	
	$TeamMedicCount[%teamnummone] = 0;
	$TeamMedicCount[%teamnumzero] = 0;
	$TeamFlyerCount[%teamnummone] = 0;
	$TeamFlyerCount[%teamnumzero] = 0;
	$TeamGuardCount[%teamnummone] = 0;
	$TeamGuardCount[%teamnumzero] = 0;
	$TeamSniperCount[%teamnummone] = 0;
	$TeamSniperCount[%teamnumzero] = 0;
	}
	
//	$Spoonbot::Team0Medic = 69;
//	$Spoonbot::Team1Medic = 69;
	
	$DISlist = "";
	$sepchar = ",";
	$maxEvents = 8;
	
//	if($Annihilation::HappyBreaker)
//		HappyBreaker::CreateStuff();

	
//	$TA::TeamLock = false; // -death666 4.3.17 moved this
	TA::AFK(); //AFK System 
	TA::OBSAFK(); //OBS AFK System -Ghost
	
	PopulateItemMax(GateGun,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
	PopulateItemMax(GrapplingHook,			0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
	PopulateItemMax(Grabbler,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
	PopulateItemMax(Slapper,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
	PopulateItemMax(Harpoon,			0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
	PopulateItemMax(HarpoonAmmo,			0,   0,   15,   15,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
	PopulateItemMax(GravityGun,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
}

function Game::pickPlayerSpawn(%clientId, %respawn)
{
	return Game::pickTeamSpawn(Client::getTeam(%clientId), %respawn);

}

function NewbieSchool(%client)
{
// <jc> = center justified, 
// <f1> = tan font, 
// <f2> = white font, 
// <f3> = orange font, \n = new line)
// new school this took way too long dont mess with it lol -death
	if(%client.observerMode == "justJoined" && %client.InSchool == 1)
	{		
		centerprint(%client,"<jc><f1>Welcome to <f2>" @ $ModVersion@"<f1> by BaseEncrypt.\n\n<f1>Hit <f3>Next Weapon<f1> for the next message, or <f3>Spacebar<f1> to spawn.",30);
		Client::sendMessage(%client, 1, "Welcome to Total Annihilation by BaseEncrypt. ~wfemale2.whitdeck.wav");
	}
	else if(%client.InSchool == 1)
	{		
		centerprint(%client,"<jc><f1>WELCOME TO <f2>" @ $ModVersion@"<f1> BY BASEENCRYPT.\n\n<f1>HIT <f3>NEXT WEAPON KEY<f1> TO CONTINUE, OR <f3>PREVIOUS WEAPON KEY<f1> TO GO BACK A TIP.",30);
		Client::sendMessage(%client, 1, "Welcome to Total Annihilation by BaseEncrypt. ~wfemale2.whitdeck.wav");
	}		
	else if(%client.InSchool == 2)
	{
		centerprint(%client,"<jc><f2>NEWCOMER TIPS:\n\n<f1> PRESS <f3>TAB KEY <f1>AND GO TO YOUR <f3>'PERSONAL OPTIONS'<f1> AND <f3>'ENABLE WEAPON HELP'.\n\n<f1>THIS WILL TURN ON THE DESCRIPTION OF WEAPONS AND PACKS WHEN YOU EQUIP THEM!",30);
		Client::sendMessage(%client, 1, "Press Tab key. Open Personal Options. Enable Weapon Help to turn on item descriptions. ~wDoor2.wav");
	}
	else if(%client.InSchool == 3)
	{
		centerprint(%client,"<jc><f2>ACCOUNT TIPS:\n\n<f1>REGISTER YOUR PLAYER NAME TO START KEEPING A RECORD OF YOUR IN GAME STATS LIKE KILLS AND DEATHS ETC.\n\n<f1>TYPE <f2>#HELP <f1>FOR REGISTRATION INFO!\n\n <f1>ONCE REGISTERED TYPE <f2>#LOGIN YOURPASSWORD<f1> WHEN YOU JOIN TO LOGIN.",30);
		Client::sendMessage(%client, 1, "Register your playername for stats. Type #help for the registration commands. ~wDoor2.wav");
	}	
	else if(%client.InSchool == 4)
	{
		centerprint(%client,"<jc><f2>USE COMMANDS:\n\n<f1>FOR EXAMPLE TYPE<f2> #combatstats PLAYERNAME \n\n<f1> COMMANDS ARE LOWERCASE UNLESS THE PLAYERNAME HAS CAPITOL LETTERS.\n\n <f1>Other Commands: <f2>#recordsstats #objectivestats #miscstats",30);
		Client::sendMessage(%client, 1, "Type #combatstats playername to check others stats. Also try #recordsstats #miscstats and #objectivestats. ~wDoor2.wav");
	}
	else if(%client.InSchool == 5)
	{
		centerprint(%client,"<jc><f2>OTHER GAME TYPES:\n\n <f1>TOTAL ANNIHILATION HAS SKI MAPS, BOT MAPS, CTF, ARENA AND MORE.\n\nBE SURE TO TRY OUT ALL OF THE DIFFERENT GAME MODES.",30);
		Client::sendMessage(%client, 1, "Total Annihilation has ski maps, bot maps, ctf, arena and more. Vote different map types to try them all. ~wDoor2.wav");
	}
	else if(%client.InSchool == 6)
	{
		centerprint(%client,"<jc><f2>BUILDING:<f1> VOTE FOR BUILDING FOR INVENTORY ACCESS ANYWHERE AND UNLIMITED DEPLOYABLES!\n\n<f2>BUILDING:<f1> GRAPPLING HOOKS, HEAVENS HARPOONS AND PITCHFORKS!\n\n <F2>ADVANCED BUILDING: <F1>SLAPPERS, GRAVITY GUNS AND PORTAL GUNS!",30);
		Client::sendMessage(%client, 1, "Vote to enable Building mode to give inventory access at all times and unlock new weapons. ~wDoor2.wav");
	}
	else if(%client.InSchool == 7)
	{
		centerprint(%client,"<jc><f2>TOTAL ANNIHILATION HAS SEVERAL DIFFERENT TYPES OF BOTS: \n\n<f1> YOU CAN VOTE TO A 'CTF BOTS' MAP AKA 'CTFB' WHICH HAS BOTS ON EACH TEAM THAT WILL PLAY CTF.\n\n THE #ARENA ALSO HAS FLYING BOTS THAT EXCEL AT DUELS AND USE JETPACKS!",30);
		Client::sendMessage(%client, 1, "There are different bot types including ctf bots, arena bots that fly, and target practice bots. ~wDoor2.wav");
	}
	else if(%client.observerMode == "justJoined" && %client.InSchool == 8)
	{
		centerprint(%client,"<jc><f1>THANKS FOR PLAYING!\n\n<f3>HIT NEXT WEAPON<f1> TO JOIN TOTAL ANNIHILATION.",30);
		Client::sendMessage(%client, 1, "Thanks for playing. ~wwind1.wav");
	}
	else if(%client.InSchool == 8)
	{
		centerprint(%client,"<jc><f1>THANKS FOR PLAYING!\n\n<f3>HIT NEXT WEAPON<f1> TO RETURN TO TOTAL ANNIHILATION.",30);
		Client::sendMessage(%client, 1, "Thanks for playing. ~wwind1.wav");
	}

	else 	centerprint(%client,"",0);
}

function ModSettingsInfo(%clientId, %weaponHelp)
{
	if($betaBuildNumber)
		%modvers = "<f1> build "@ $betaBuildNumber;
	if($Annihilation::SafeBase)
		%modinfostring = " Base supply is undestroyable";
	else %modinfostring = " Base supply is destroyable";
	if($Annihilation::BaseHeal && !$Annihilation::SafeBase)
		%modinfostring = %modinfostring @ ", self regenerating.";
	else %modinfostring = %modinfostring @ ". ";
	if($Annihilation::NoPlayerDamage)
		%modinfostring = %modinfostring @ " Player damage: OFF.";
	if($Build)
		%modinfostring = %modinfostring @ " Builder mode: ON.";

//      //Get the current map's mission type from the .dsc file
      %missionFile = "missions\\" $+ $missionName $+ ".dsc";
      if(File::FindFirst(%missionFile) == "")
      {
         %missionName = $firstMission;
         %missionFile = "missions\\" $+ $missionName $+ ".dsc";
         if(File::FindFirst(%missionFile) == "")
         {
            return;
         }
      }
      exec(%missionFile);
		
      for(%i = 0; %i < $MLIST::TypeCount; %i++)
      {
         if($MLIST::Type[%i] == $MDESC::Type)
         {
            break;
         }
      }		
	
	
//	%clientId.weaponHelp = false;
	if($TALT::Active == false) //Switch this up a bit 
	//	bottomprint(%clientId, "<jc><f2> " @ $ModVersion@ %modvers @"\n<f2>"@%modinfostring@"\n<f0>Mission: <f1>" @ $missionName @ "	<f0> Mission Type: <f1>" @ $Game::missionType @ "\n<f3>To mute players: <f1>Open the menu and click <f2>'Ignore' <f1>after clicking on the selected player.",5);
		bottomprint(%clientId, "<jc><f0> Mission: <f1>" @ $missionName @ "	<f0> Mission Type: <f1>" @ $MDESC::Type @ ".",5);	// $Game::missionType																																								
	else
	//	bottomprint(%clientId, "<jc><f2> " @ $ModVersion@ %modvers @"\n<f2>"@%modinfostring@"\n<f0>Mission: <f1>" @ $missionName @ "	<f0> Mission Type: <f1>" @ $Game::missionType @ "\n<f3>To mute players: <f1>Open the menu and click <f2>'Ignore' <f1>after clicking on the selected player.",5);
		bottomprint(%clientId, "<jc><f0> Mission: <f1>" @ $missionName @ "	<f0> Mission Type: <f1>" @ $MDESC::Type @ ".",5);	
//	if(%weaponHelp)schedule(%clientId @ ".weaponHelp = true;" , 10, %clientId);
}

//	$DefaultArmor[Male] = armormWarrior;
//	$DefaultArmor[Female] = armorfWarrior;

function Game::playerSpawn(%clientId, %respawn)
{
	if(!$ghosting)
		return false;
	%clientId.AdminobserverMode = "";
	Client::clearItemShopping(%clientId);
	%spawnMarker = Game::pickPlayerSpawn(%clientId, %respawn);
	
	if(%clientId.inArena) // New Arena code
	{
		if($ArenaTD::Active)
			if(%clientId.isArenaTDDead) return;
		if($ArenaTD::Active && !%clientId.inArenaTD) return;
		if($Arena::Winners && %clientId.inArenaWin == "") return;
		if($Arena::Mapchange) return; // dont want anyone spawning before arena loads O.O
		
		//if(!$Arena::Terrain)
			%spawnMarker = Arena::pickRandomSpawn(%clientId);
			
		//echo(%spawnMarker@" <<<<< Game::playerSpawn(%clientId, %respawn); <<<< ArenaTerrainSpawn <<< GAME");
	}
	
	if(%spawnMarker)
	{
		%clientId.guiLock = "";
		%clientId.dead = "";
		//%adterrain = TA::pickWaypoint();
		if($ArenaTD::Active && %clientId.inArenaTD)
		{
			if(%clientId.inArenaTDOne)
			{
				if($Arena::Terrain)
				{
					%spawnPosx = GetOffsetRot(GameBase::getPosition(%spawnMarker),"0 0 0",$ArenaTD::TerrainPos);
					//%spawnPosx = GameBase::getPosition(%spawnMarker);
					%spawnObjx = Terrain::CheckPos(%spawnPosx);
					%spawnObj = Vector::add(%spawnObjx,"0 0 3");
					%obj = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true);
					AddToSet("MissionCleanup",%obj);
					GameBase::setPosition(%obj,%spawnObj);
					GameBase::setRotation(%obj,$Pi@" 0 0");
					%spawnPosz = Terrain::CheckPos(%spawnPosx);
					%spawnPos = Vector::add(%spawnPosz,"0 0 3");
					//echo(%spawnPos@" <<<<< td 1 SPAWN <<<< ArenaTerrainSpawn");
					%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);
					Schedule("deleteObject("@ %obj @");",5,%obj);
				}
				else
				{
					%spawnPos = GameBase::getPosition(%spawnMarker);
					%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);
				}
				//echo(">>> "@%spawnPos@" <<< Arena TD 1 spawnRot >>> "@%spawnRot@" <<< Arena TD 1 spawnRot >>> Team One <<< GAME");
			}
			else if(%clientId.inArenaTDTwo)
			{
	
				if($Arena::Terrain)
				{
					%spawnPosx = GetOffsetRot(GameBase::getPosition(%spawnMarker),"0 0 0",$ArenaTD::TerrainPos);
					//%spawnPosx = GameBase::getPosition(%spawnMarker);
					%spawnObjx = Terrain::CheckPos(%spawnPosx);
					%spawnObj = Vector::add(%spawnObjx,"0 0 3");
					%obj = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true);
					AddToSet("MissionCleanup",%obj);
					GameBase::setPosition(%obj,%spawnObj);
					GameBase::setRotation(%obj,$Pi@" 0 0");
					%spawnPosz = Terrain::CheckPos(%spawnPosx);
					%spawnPos = Vector::add(%spawnPosz,"0 0 3");
					//echo(%spawnPos@" <<<<< td 2 SPAWN <<<< ArenaTerrainSpawn");
					%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);
					Schedule("deleteObject("@ %obj @");",5,%obj);
				}
				else
				{
					%spawnPos = GameBase::getPosition(%spawnMarker);
					%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);
				}
				//echo(">>> "@%spawnPos@" <<< Arena TD 2 spawnRot >>> "@%spawnRot@" <<< Arena TD 2 spawnRot >>> Team Two <<< GAME");
			}
		}
		else if(%clientId.inArena)
		{
			if($Arena::Terrain)
			{
				%spawnPosx = GameBase::getPosition(%spawnMarker);
				%spawnObjx = Terrain::CheckPos(%spawnPosx);
				%spawnObj = Vector::add(%spawnObjx,"0 0 3");
				%objx = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true);
				AddToSet("MissionCleanup",%objx);
				GameBase::setPosition(%objx,%spawnObj);
				GameBase::setRotation(%objx,$Pi@" 0 0");
				%spawnPosz = Terrain::CheckPos(%spawnPosx);
				%spawnPos = Vector::add(%spawnPosz,"0 0 3");
				//echo(%spawnPos@" <<<<< arena SPAWN <<<< ArenaTerrainSpawn");
				%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);
				Schedule("deleteObject("@ %objx @");",5,%objx);
			}
			else
			{
				%spawnPos = GameBase::getPosition(%spawnMarker);
				%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);
			}
			//echo(">>> "@%spawnPos@" <<< Arena spawnRot >>> "@%spawnRot@" <<< Arena spawnRot >>> Team Two <<< GAME");
		}
		else if(%clientId.inDuel) // New Arena code
		{
			if($Dueling[%clientId] == false) return;
			if($Duel::Mapchange) return; // dont want anyone spawning before arena loads O.O
		
			//%duelnum = Duel::getnum();
			//if(%duelnum <= 2)
			//{
				if(!$Duela)
				{					
					%pos = "0 10 0";
					%spawnPosx = GetOffsetRot(%pos,"0 0 0",$Duel::Spawn);
					%spawnObj = Vector::add(%spawnPosx,"0 0 3");
					%obj = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true);
					AddToSet("MissionCleanup",%obj);
					GameBase::setPosition(%obj,%spawnObj);
					GameBase::setRotation(%obj,$Pi@" 0 0");
					%spawnPosz = Terrain::CheckPos(%spawnPosx);
					%spawnPos = Vector::add(%spawnPosz,"0 0 3");
					Schedule("deleteObject("@ %objx @");",11,%objx);
					%spawnRot = "0 -0 -3";
					$Duela = true;
				}
				else
				{
					%pos = "0 -10 0";
					%spawnPosx = GetOffsetRot(%pos,"0 0 0",$Duel::Spawn);
					%spawnObj = Vector::add(%spawnPosx,"0 0 3");
					%obj = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true);
					AddToSet("MissionCleanup",%obj);
					GameBase::setPosition(%obj,%spawnObj);
					GameBase::setRotation(%obj,$Pi@" 0 0");
					%spawnPosz = Terrain::CheckPos(%spawnPosx);
					%spawnPos = Vector::add(%spawnPosz,"0 0 3");
					Schedule("deleteObject("@ %objx @");",11,%objx);
					%spawnRot = "0 -0 -0";
					$Duela = false;
				}
			//}
			
			//%spawnMarker = Duel::pickRandomSpawn();
		}
		else if(%spawnMarker == -1)
		{
			%spawnPos = "0 0 300";
			%spawnRot = "0 0 0";
		}
		else
		{
			%spawnPos = GameBase::getPosition(%spawnMarker);

			%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);
		}
		if(%clientId.inArena) // New Arena code 
		{
			if($TAArena::SpawnType == "AnniSpawn") //New Arena toggle 
			{
				$DefaultArmor[Male] = armormWarrior;
				$DefaultArmor[Female] = armorfWarrior;	
			}
			if($TAArena::SpawnType == "TitanSpawn") //new shittttt!!!!!
			{
				$DefaultArmor[Male] = armorTitan;
				$DefaultArmor[Female] = armorTitan;
			}
			if($TAArena::SpawnType == "BuilderSpawn")
			{
				$DefaultArmor[Male] = armormBuilder;
				$DefaultArmor[Female] = armorfBuilder;
			}
			if($TAArena::SpawnType == "EliteSpawn")
			{
				$DefaultArmor[Male] = armormMercenary;
				$DefaultArmor[Female] = armorfMercenary;
			}
			if($TAArena::SpawnType == "BaseSpawn")
			{
				$DefaultArmor[Male] = armormLightArmor;
				$DefaultArmor[Female] = armorfLightArmor;
			}
		}
		else if(%clientId.inDuel) 
		{
			if(%clientId.DuelArmor == "AnniSpawn") //New Duel toggle, stores by clientId 
			{
				$DefaultArmor[Male] = armormWarrior;
				$DefaultArmor[Female] = armorfWarrior;	
			}
			if(%clientId.DuelArmor == "EliteSpawn")
			{
				$DefaultArmor[Male] = armormMercenary;
				$DefaultArmor[Female] = armorfMercenary;
			}
			if(%clientId.DuelArmor == "BaseSpawn")
			{
				$DefaultArmor[Male] = armormLightArmor;
				$DefaultArmor[Female] = armorfLightArmor;
			}
			if(%clientId.DuelArmor == "BuilderSpawn")
			{
				$DefaultArmor[Male] = armormBuilder;
				$DefaultArmor[Female] = armorfBuilder;
			}
			if(%clientId.DuelArmor == "TitanSpawn")
			{
				$DefaultArmor[Male] = armorTitan;
				$DefaultArmor[Female] = armorTitan;
			}
		}
		else if($TALT::Active == true) 
		{
			if($TALT::SpawnType == "AnniSpawn") //New Arena toggle 
			{
				$DefaultArmor[Male] = armormWarrior;
				$DefaultArmor[Female] = armorfWarrior;	
			}
			if($TALT::SpawnType == "EliteSpawn")
			{
				$DefaultArmor[Male] = armormMercenary;
				$DefaultArmor[Female] = armorfMercenary;
			}
			if($TALT::SpawnType == "BaseSpawn")
			{
				$DefaultArmor[Male] = armormLightArmor;
				$DefaultArmor[Female] = armorfLightArmor;
			}
		}
		else if($UberWear::Active == true)
		{
			$DefaultArmor[Male] = armormLightArmor;
			$DefaultArmor[Female] = armorfLightArmor;
		}
		else
		{
			$DefaultArmor[Male] = armormWarrior;
			$DefaultArmor[Female] = armorfWarrior;			
		}
		%armor = $DefaultArmor[Client::getGender(%clientId)];

		%pl = spawnPlayer(%armor, %spawnPos, %spawnRot);
		Anni::Echo("SPAWN: "@ Client::getName(%clientID)@ " cl:" @ %clientId @ " pl:" @ %pl @ " marker:" @ %spawnMarker @ " armor:" @ %armor);
		%pl.cloakable = true;	
		%clientId.AdminobserverMode = "";
		%clientId.observerMode = "";
		%pl.teled = "";	
		%clientId.ConnectBeam = "";	
		%clientId.InvTargetable = "";	
		%ClientId.InvConnect = "";	
		
		if(%pl != -1)
		{
			//Client::clearItemShopping(%clientId);
			GameBase::setTeam(%pl, Client::getTeam(%clientId));
			Client::setOwnedObject(%clientId, %pl);
			//echo("start"); //debug for the damn player::useitem crap 
			Game::playerSpawned(%pl, %clientId, %armor, %respawn);
			%pl.cloakable = true;
			$jailed[%pl] = "";
			$released[%pl] = "";
			if($matchStarted)
			{
				Client::setControlObject(%clientId, %pl);
				%clientId.droid=false;
			}
			else
			{
				%clientId.observerMode = "pregame";
				Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
				Observer::setOrbitObject(%clientId, %pl, 3, 3, 3);
			}
			if($TALT::Active == false) //get rid of safe gaurds on start //New LT code
				$respawnInvulnerableTime = 10;
			if(%respawn)	
			{
				if($TALT::Active == false) //get rid of safe gaurds on start //New LT code
					$respawnInvulnerableTime = 5;
				if($siegeFlag)	
					Siege::waypointClient(%clientId);
			}
			if($TALT::Active == false) //get rid of safe gaurds on start //New LT code 
           		%damage = %armor.maxDamage - 0.001;
            GameBase::setEnergy(%pl, %armor.maxEnergy);	///2);
			if($TALT::Active == false) //get rid of safe gaurds on start //New LT code 
			{
//            	GameBase::setAutoRepairRate(%pl, %damage / $respawnInvulnerableTime); -removing auto repair here -death666
            	%pl.invulnerable = true;
			}
            %pl.cloakable = true;	
            $jailed[%pl] = "";	//just to be certain for jail..
            $released[%pl] = "";
      		if($TALT::Active == false) //get rid of safe gaurds on start //New LT code 
            	incInvulnerable(%pl, 0);
		    %weapon = Player::getMountedItem(%pl,$WeaponSlot);
		    if(%weapon != -1) //lol, I would put money on it this is what kept unmounting the item on spawn
			{
			      %pl.lastWeapon = %weapon;
			      Player::unMountItem(%pl,$WeaponSlot); //w.e might be here for a reason, my quick fix wont fuck with this
			}
       		if(!%respawn)
			{
			// initial drop
		if($siegeFlag)
		{
			schedule("Client::sendMessage("@%clientId@", 0, \"Welcome to Siege! Capture or hold the switch to complete the mission.\");",5, %clientId);
			schedule("Siege::InitialwaypointClient("@%clientId@");",5,%clientId);
		}
		if(%clientId.SpecialMessage == true)
			CustomMessage(%clientId);
		ModSettingsInfo(%clientId, true);
		//Anni::Echo("join team "@Client::getTeam(%clientId)@" old team = "@%clientId.lastteam);
		if(%clientId.LastTeam != -1 && %clientId.LastTeam != Client::getTeam(%clientId))
		{
			if($annihilation::DisableTurretsOnTeamChange)
				Turret::DisableClients(%clientId);	
		}
		
		%clientId.LastTeam = Client::getTeam(%clientId);
	}  
			
		}
		return true;
	}
	else 
	{
		Client::sendMessage(%clientId,0,"Sorry No Respawn Positions Are Empty - Try again later ");
		return false;
	}
}
function incInvulnerable(%player, %time)
{
   if(%time > $respawnInvulnerableTime)
   {
	  if(!%player.inArenaTD)
		%player.invulnerable = "";
      GameBase::setAutoRepairRate(%player, 0);
	%weapon = Player::getMountedItem(%player,$WeaponSlot);
	   if(%player.lastWeapon != "" && %weapon == -1) {
		   Player::useItem(%player,%player.lastWeapon);
		   %player.lastWeapon = "";
	    }
   }
   else
   { 
      schedule("incInvulnerable(" @ %player @ ", "@ %time + 0.5 @ ");", 0.5, %player);
	   if(%player.invulnerable != "") //adding a shield animation for spawn shield -death666
	   {
	   %svec = Vector::getFromRot(GameBase::getRotation(Player::getClient(%player)),10,0);	//10,0
	   GameBase::activateShield(%player,%svec, 0.01); //0.5
	   }

   }
}


function Game::playerSpawned(%pl, %clientId, %armor)
{
	
	schedule("Client::clearItemShopping("@%clientId@");",1, %clientId);
	
	if($Annihilation::UsePersonalSkin)
		Client::setSkin(%clientId, $Client::info[%clientId, 0]);
	else 
		Client::setSkin(%clientId, $Server::teamSkin[Client::getTeam(%clientId)]);
	
	//%pl = Client::getOwnedObject(%clientId);
		%clientId.hasmessageend = true;
		%clientId.waitmsgHP = true;
		%clientId.ObsCD = true;
		%clientId.hasrphallpass = true;
		%clientId.hashallpass = true;
		%clientId.hashallpass2 = true;
		%clientId.hasfirstcount = true;
		%clientId.hassecondcount = true;
		%clientId.hassphallpass = false;
		

//		Client::setControlObject(%clientId, %pl);
		%clientId.droid=false;
// new death666


	
	if($build)
	{
		Player::setItemCount(%clientId, iarmorWarrior, 1);	
		Player::setItemCount(%clientId, DiscLauncher, 1);
		Player::setItemCount(%clientId, PlasmaGun, 1);
		Player::setItemCount(%clientId, RocketLauncher, 1);
		Player::setItemCount(%clientId, Shotgun, 1);
		Player::setItemCount(%clientId, ShockwaveGun, 1);
		Player::setItemCount(%clientId, Stinger, 1);
		Player::setItemCount(%clientId, Blaster, 1);
		Player::setItemCount(%clientId, Vulcan, 1);
			
		Player::setItemCount(%clientId, TargetingLaser, 1);
			   
		Player::setItemCount(%clientId, DiscAmmo, 30);		   
		Player::setItemCount(%clientId, PlasmaAmmo, 30);
		Player::setItemCount(%clientId, RocketAmmo, 30);
    	 	Player::setItemCount(%clientId, ShotgunShells, 30);	
    	 	Player::setItemCount(%clientId, StingerAmmo, 30);
    	 	Player::setItemCount(%clientId, VulcanAmmo, 200);	
		   
		Player::setItemCount(%clientId, RepairKit, 1);
		Player::setItemCount(%clientId, Beacon, 5);
		Player::setItemCount(%clientId, Grenade, 8);	   
		//Player::setItemCount(%clientId, MineAmmo, 0);
		   	   
		Player::setItemCount(%clientId, EnergyPack, 1);
		if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
		Player::useItem(%pl,EnergyPack);
		TA::useItem(%pl,DiscLauncher);
	}
	else if(%clientId.inArena) // New Arena code
	{
		if($TAArena::SpawnType == "AnniSpawn") 
		{
			if($TAArena::WeaponOpt == "Normal")
			{
				Player::setItemCount(%clientId, iarmorWarrior, 1);	
				Player::setItemCount(%clientId, DiscLauncher, 1);
				Player::setItemCount(%clientId, PlasmaGun, 1);
				Player::setItemCount(%clientId, GrenadeLauncherBase, 1);
			
				Player::setItemCount(%clientId, TargetingLaser, 1);
			   
				Player::setItemCount(%clientId, DiscAmmo, 30);		   
				Player::setItemCount(%clientId, PlasmaAmmo, 30);
				Player::setItemCount(%clientId, GrenadeAmmoBase, 20);
		   
				Player::setItemCount(%clientId, RepairKit, 1);
				Player::setItemCount(%clientId, Beacon, 5);
				Player::setItemCount(%clientId, Grenade, 8);	   
				//Player::setItemCount(%clientId, MineAmmo, 0);
		   	   
				Player::setItemCount(%clientId, EnergyPack, 1);
				if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
					Player::useItem(%pl,EnergyPack);
				TA::useItem(%pl,DiscLauncher);
			}
			else if($TAArena::WeaponOpt == "DiscOnly")
			{
				Player::setItemCount(%clientId, iarmorWarrior, 1);	
				Player::setItemCount(%clientId, DiscLauncher, 1);
			
				Player::setItemCount(%clientId, TargetingLaser, 1);
			   
				Player::setItemCount(%clientId, DiscAmmo, 100);
		   
				Player::setItemCount(%clientId, RepairKit, 1);
				Player::setItemCount(%clientId, Beacon, 5);
				Player::setItemCount(%clientId, Grenade, 8);	   
				//Player::setItemCount(%clientId, MineAmmo, 0);
		   	   
				Player::setItemCount(%clientId, EnergyPack, 1);
				if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
					Player::useItem(%pl,EnergyPack);
				TA::useItem(%pl,DiscLauncher);
			}
			else if($TAArena::WeaponOpt == "RocketOnly")
			{
				Player::setItemCount(%clientId, iarmorWarrior, 1);	
				Player::setItemCount(%clientId, RocketLauncher, 1);
			
				Player::setItemCount(%clientId, TargetingLaser, 1);
			   
				Player::setItemCount(%clientId, RocketAmmo, 100);
		   
				Player::setItemCount(%clientId, RepairKit, 1);
				Player::setItemCount(%clientId, Beacon, 5);
				Player::setItemCount(%clientId, Grenade, 8);	   
				//Player::setItemCount(%clientId, MineAmmo, 0);
		   	   
				Player::setItemCount(%clientId, EnergyPack, 1);
				if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
					Player::useItem(%pl,EnergyPack);
				TA::useItem(%pl,RocketLauncher);
			}
			else if($TAArena::WeaponOpt == "MaxAmmo")
			{
				Player::setItemCount(%clientId, iarmorWarrior, 1);	
				Player::setItemCount(%clientId, DiscLauncher, 1);
				Player::setItemCount(%clientId, PlasmaGun, 1);
				Player::setItemCount(%clientId, GrenadeLauncherBase, 1);
			
				Player::setItemCount(%clientId, TargetingLaser, 1);
			   
				Player::setItemCount(%clientId, DiscAmmo, 100);		   
				Player::setItemCount(%clientId, PlasmaAmmo, 100);
				Player::setItemCount(%clientId, GrenadeAmmoBase, 100);
		   
				Player::setItemCount(%clientId, RepairKit, 1);
				Player::setItemCount(%clientId, Beacon, 5);
				Player::setItemCount(%clientId, Grenade, 8);	   
				//Player::setItemCount(%clientId, MineAmmo, 0);
		   	   
				Player::setItemCount(%clientId, EnergyPack, 1);
				if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
					Player::useItem(%pl,EnergyPack);
				TA::useItem(%pl,DiscLauncher);
			}
			
			//Player::mountItem(%clientId,BlowUpDoll,7);
		}
		if($TAArena::SpawnType == "TitanSpawn") 
		{
			Player::setItemCount(%clientId, iarmorTitan, 1);	
			Player::setItemCount(%clientId, RocketLauncher, 1);
			Player::setItemCount(%clientId, ParticleBeamWeapon, 1);
			
			Player::setItemCount(%clientId, TargetingLaser, 1);
			   
			Player::setItemCount(%clientId, RocketAmmo, 100);
	   
			Player::setItemCount(%clientId, RepairKit, 1);
			Player::setItemCount(%clientId, Beacon, 5);
			Player::setItemCount(%clientId, Grenade, 8);	   
			Player::setItemCount(%clientId, MineAmmo, 5);
		   	   
			Player::setItemCount(%clientId, EnergyPack, 1);
			if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
				Player::useItem(%pl,EnergyPack);
			TA::useItem(%pl,RocketLauncher);
		}
		if($TAArena::SpawnType == "BuilderSpawn") 
		{
			Player::setItemCount(%clientId, Railgun, 1);
			Player::setItemCount(%clientId, RocketLauncher, 1);
    	    Player::setItemCount(%clientId, Fixit, 1);
			
			Player::setItemCount(%clientId, TargetingLaser, 1);
			   
			Player::setItemCount(%clientId, RailAmmo, 100);		   
			Player::setItemCount(%clientId, RocketAmmo, 100);
		   
			Player::setItemCount(%clientId, RepairKit, 1);
			//Player::setItemCount(%clientId, Beacon, 1);
			Player::setItemCount(%clientId, Grenade, 8);   
			//Player::setItemCount(%clientId, MineAmmo, 3);
		   	   
			Player::setItemCount(%clientId, EnergyPack, 1);
			if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
				Player::useItem(%pl,EnergyPack);
			TA::useItem(%pl,RocketLauncher);
		}
		if($TAArena::SpawnType == "EliteSpawn")
		{
			if($TAArena::WeaponOpt == "Normal")
			{
				//Player::setItemCount(%clientId, iarmorMercenary, 1);	
				Player::setItemCount(%clientId, DiscLauncherElite, 1);
				Player::setItemCount(%clientId, PlasmaGunBase, 1);
				Player::setItemCount(%clientId, GrenadeLauncherBase, 1);
			
				Player::setItemCount(%clientId, TargetingLaser, 1);
			   
				Player::setItemCount(%clientId, DiscAmmoElite, 15);		   
				Player::setItemCount(%clientId, PlasmaAmmoBase, 40);
				Player::setItemCount(%clientId, GrenadeAmmoBase, 10);
		   
				Player::setItemCount(%clientId, RepairKit, 1);
				Player::setItemCount(%clientId, Beacon, 3);
				Player::setItemCount(%clientId, Grenade, 6);	   
				//Player::setItemCount(%clientId, MineAmmo, 0);
		   	   
				Player::setItemCount(%clientId, EnergyPack, 1);
				if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
					Player::useItem(%pl,EnergyPack);
				TA::useItem(%pl,DiscLauncherElite);
			
			}
			else if($TAArena::WeaponOpt == "DiscOnly")
			{
				//Player::setItemCount(%clientId, iarmorMercenary, 1);	//Removed since this isn't an actual item 
				Player::setItemCount(%clientId, DiscLauncherElite, 1);
			
				Player::setItemCount(%clientId, TargetingLaser, 1);
			   
				Player::setItemCount(%clientId, DiscAmmoElite, 100);
		   
				Player::setItemCount(%clientId, RepairKit, 1);
				Player::setItemCount(%clientId, Beacon, 3);
				Player::setItemCount(%clientId, Grenade, 6);	   
				//Player::setItemCount(%clientId, MineAmmo, 0);
		   	   
				Player::setItemCount(%clientId, EnergyPack, 1);
				if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
					Player::useItem(%pl,EnergyPack);
				TA::useItem(%pl,DiscLauncherElite);
			
			}
			else if($TAArena::WeaponOpt == "RocketOnly")
			{
				//Player::setItemCount(%clientId, iarmorMercenary, 1);	//Removed since this isn't an actual item 
				Player::setItemCount(%clientId, RocketLauncher, 1);
			
				Player::setItemCount(%clientId, TargetingLaser, 1);
			   
				Player::setItemCount(%clientId, RocketAmmo, 100);
		   
				Player::setItemCount(%clientId, RepairKit, 1);
				Player::setItemCount(%clientId, Beacon, 3);
				Player::setItemCount(%clientId, Grenade, 6);	   
				//Player::setItemCount(%clientId, MineAmmo, 0);
		   	   
				Player::setItemCount(%clientId, EnergyPack, 1);
				if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
					Player::useItem(%pl,EnergyPack);
				TA::useItem(%pl,RocketLauncher);
			
			}
			else if($TAArena::WeaponOpt == "MaxAmmo")
			{
				//Player::setItemCount(%clientId, iarmorMercenary, 1);	//Removed since this isn't an actual item
				Player::setItemCount(%clientId, DiscLauncherElite, 1);
				Player::setItemCount(%clientId, PlasmaGunBase, 1);
				Player::setItemCount(%clientId, GrenadeLauncherBase, 1);
			
				Player::setItemCount(%clientId, TargetingLaser, 1);
			   
				Player::setItemCount(%clientId, DiscAmmoElite, 100);		   
				Player::setItemCount(%clientId, PlasmaAmmoBase, 100);
				Player::setItemCount(%clientId, GrenadeAmmoBase, 100);
		   
				Player::setItemCount(%clientId, RepairKit, 1);
				Player::setItemCount(%clientId, Beacon, 3);
				Player::setItemCount(%clientId, Grenade, 6);	   
				//Player::setItemCount(%clientId, MineAmmo, 0);
		   	   
				Player::setItemCount(%clientId, EnergyPack, 1);
				if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
					Player::useItem(%pl,EnergyPack);
				TA::useItem(%pl,DiscLauncherElite);
			
			}
		}
		if($TAArena::SpawnType == "BaseSpawn")
		{
			if($TAArena::WeaponOpt == "Normal")
			{
				//Player::setItemCount(%clientId, iarmorLightArmor, 1);	//Removed since this isn't an actual item 
				Player::setItemCount(%clientId, DiscLauncherBase, 1);
				Player::setItemCount(%clientId, PlasmaGunBase, 1);
				Player::setItemCount(%clientId, GrenadeLauncherBase, 1);
			
				Player::setItemCount(%clientId, TargetingLaser, 1);
			   
				Player::setItemCount(%clientId, DiscAmmoBase, 15);		   
				Player::setItemCount(%clientId, PlasmaAmmoBase, 30);
				Player::setItemCount(%clientId, GrenadeAmmoBase, 10);
		   
				Player::setItemCount(%clientId, RepairKit, 1);
				Player::setItemCount(%clientId, Beacon, 3);
				Player::setItemCount(%clientId, Grenade, 5);	   
				//Player::setItemCount(%clientId, MineAmmo, 0);
		   	   
				Player::setItemCount(%clientId, EnergyPack, 1);
				if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
					Player::useItem(%pl,EnergyPack);
				TA::useItem(%pl,DiscLauncherBase);
			
			}
			else if($TAArena::WeaponOpt == "DiscOnly")
			{
				//Player::setItemCount(%clientId, iarmorLightArmor, 1);	//Removed since this isn't an actual item 
				Player::setItemCount(%clientId, DiscLauncherBase, 1);
			
				Player::setItemCount(%clientId, TargetingLaser, 1);
			   
				Player::setItemCount(%clientId, DiscAmmoBase, 100);
		   
				Player::setItemCount(%clientId, RepairKit, 1);
				Player::setItemCount(%clientId, Beacon, 3);
				Player::setItemCount(%clientId, Grenade, 5);	   
				//Player::setItemCount(%clientId, MineAmmo, 0);
		   	   
				Player::setItemCount(%clientId, EnergyPack, 1);
				if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
					Player::useItem(%pl,EnergyPack);
				TA::useItem(%pl,DiscLauncherBase);
			
			}
			else if($TAArena::WeaponOpt == "RocketOnly")
			{
				//Player::setItemCount(%clientId, iarmorLightArmor, 1);	//Removed since this isn't an actual item
				Player::setItemCount(%clientId, RocketLauncher, 1);
			
				Player::setItemCount(%clientId, TargetingLaser, 1);
			   
				Player::setItemCount(%clientId, RocketAmmo, 100);
		   
				Player::setItemCount(%clientId, RepairKit, 1);
				Player::setItemCount(%clientId, Beacon, 3);
				Player::setItemCount(%clientId, Grenade, 5);	   
				//Player::setItemCount(%clientId, MineAmmo, 0);
		   	   
				Player::setItemCount(%clientId, EnergyPack, 1);
				if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
					Player::useItem(%pl,EnergyPack);
				TA::useItem(%pl,RocketLauncher);
			
			}
			else if($TAArena::WeaponOpt == "MaxAmmo")
			{
				//Player::setItemCount(%clientId, iarmorLightArmor, 1);	//Removed since this isn't an actual item 
				Player::setItemCount(%clientId, DiscLauncherBase, 1);
				Player::setItemCount(%clientId, PlasmaGunBase, 1);
				Player::setItemCount(%clientId, GrenadeLauncherBase, 1);
			
				Player::setItemCount(%clientId, TargetingLaser, 1);
			   
				Player::setItemCount(%clientId, DiscAmmoBase, 100);		   
				Player::setItemCount(%clientId, PlasmaAmmoBase, 100);
				Player::setItemCount(%clientId, GrenadeAmmoBase, 100);
		   
				Player::setItemCount(%clientId, RepairKit, 1);
				Player::setItemCount(%clientId, Beacon, 3);
				Player::setItemCount(%clientId, Grenade, 5);	   
				//Player::setItemCount(%clientId, MineAmmo, 0);
		   	   
				Player::setItemCount(%clientId, EnergyPack, 1);
				if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
					Player::useItem(%pl,EnergyPack);
				TA::useItem(%pl,DiscLauncherBase);
			
			}
		}
		
	}
	else if(%clientId.inDuel) // New Duel code
	{
		if(%clientId.DuelArmor == "AnniSpawn") //New Arena toggle 
		{
			Player::setItemCount(%clientId, iarmorWarrior, 1);	
			Player::setItemCount(%clientId, DiscLauncher, 1);
			Player::setItemCount(%clientId, PlasmaGun, 1);
    	    Player::setItemCount(%clientId, GrenadeLauncherBase, 1);
			
			Player::setItemCount(%clientId, TargetingLaser, 1);
			   
			Player::setItemCount(%clientId, DiscAmmo, 30);		   
			Player::setItemCount(%clientId, PlasmaAmmo, 30);
			Player::setItemCount(%clientId, GrenadeAmmoBase, 20);
		   
			Player::setItemCount(%clientId, RepairKit, 1);
			Player::setItemCount(%clientId, Beacon, 5);
			Player::setItemCount(%clientId, Grenade, 8);	   
			//Player::setItemCount(%clientId, MineAmmo, 0);
		   	   
			Player::setItemCount(%clientId, EnergyPack, 1);
			if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
				Player::useItem(%pl,EnergyPack);
			TA::useItem(%pl,DiscLauncher);
		}
		if(%clientId.DuelArmor == "EliteSpawn")
		{
			//Player::setItemCount(%clientId, iarmorMercenary, 1);	//Removed since this isn't an actual item 
			Player::setItemCount(%clientId, DiscLauncherElite, 1);
			Player::setItemCount(%clientId, PlasmaGunBase, 1);
    	    Player::setItemCount(%clientId, GrenadeLauncherBase, 1);
			
			Player::setItemCount(%clientId, TargetingLaser, 1);
			   
			Player::setItemCount(%clientId, DiscAmmoElite, 15);		   
			Player::setItemCount(%clientId, PlasmaAmmoBase, 40);
			Player::setItemCount(%clientId, GrenadeAmmoBase, 10);
		   
			Player::setItemCount(%clientId, RepairKit, 1);
			Player::setItemCount(%clientId, Beacon, 3);
			Player::setItemCount(%clientId, Grenade, 6);	   
			//Player::setItemCount(%clientId, MineAmmo, 0);
		   	   
			Player::setItemCount(%clientId, EnergyPack, 1);
			if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
				Player::useItem(%pl,EnergyPack);
			TA::useItem(%pl,DiscLauncherElite);
		}
		if(%clientId.DuelArmor == "BaseSpawn")
		{
			//Player::setItemCount(%clientId, iarmorLightArmor, 1);	//Removed since this isn't an actual item 
			Player::setItemCount(%clientId, DiscLauncherBase, 1);
			Player::setItemCount(%clientId, PlasmaGunBase, 1);
    	    Player::setItemCount(%clientId, GrenadeLauncherBase, 1);
			
			Player::setItemCount(%clientId, TargetingLaser, 1);
			   
			Player::setItemCount(%clientId, DiscAmmoBase, 15);		   
			Player::setItemCount(%clientId, PlasmaAmmoBase, 30);
			Player::setItemCount(%clientId, GrenadeAmmoBase, 10);
		   
			Player::setItemCount(%clientId, RepairKit, 1);
			Player::setItemCount(%clientId, Beacon, 3);
			Player::setItemCount(%clientId, Grenade, 5);	   
			//Player::setItemCount(%clientId, MineAmmo, 0);
		   	   
			Player::setItemCount(%clientId, EnergyPack, 1);
			if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
				Player::useItem(%pl,EnergyPack);
			TA::useItem(%pl,DiscLauncherBase);
		}
		if(%clientId.DuelArmor == "BuilderSpawn")
		{
			//Player::setItemCount(%clientId, iarmorLightArmor, 1);	//Removed since this isn't an actual item 
			Player::setItemCount(%clientId, Railgun, 1);
			Player::setItemCount(%clientId, RocketLauncher, 1);
    	    Player::setItemCount(%clientId, Fixit, 1);
			
			Player::setItemCount(%clientId, TargetingLaser, 1);
			   
			Player::setItemCount(%clientId, RailAmmo, 15);		   
			Player::setItemCount(%clientId, RocketAmmo, 10);
		   
			Player::setItemCount(%clientId, RepairKit, 1);
			//Player::setItemCount(%clientId, Beacon, 1);
			Player::setItemCount(%clientId, Grenade, 8);	   
			//Player::setItemCount(%clientId, MineAmmo, 3);
		   	   
			Player::setItemCount(%clientId, EnergyPack, 1);
			if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
				Player::useItem(%pl,EnergyPack);
			TA::useItem(%pl,RocketLauncher);
		}
		if(%clientId.DuelArmor == "TitanSpawn")
		{
			//Player::setItemCount(%clientId, iarmorLightArmor, 1);	//Removed since this isn't an actual item 
			Player::setItemCount(%clientId, ParticleBeamWeapon, 1);
			Player::setItemCount(%clientId, RocketLauncher, 1);
    	    Player::setItemCount(%clientId, PhaseDisrupter, 1);
			
			Player::setItemCount(%clientId, TargetingLaser, 1);
			   
			Player::setItemCount(%clientId, RocketAmmo, 20);		   
			Player::setItemCount(%clientId, PhaseAmmo, 30);
		   
			Player::setItemCount(%clientId, RepairKit, 1);
			Player::setItemCount(%clientId, Beacon, 5);
			Player::setItemCount(%clientId, Grenade, 8);	   
			//Player::setItemCount(%clientId, MineAmmo, 5);
		   	   
			Player::setItemCount(%clientId, EnergyPack, 1);
			if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
				Player::useItem(%pl,EnergyPack);
			TA::useItem(%pl,RocketLauncher);
		}
		
	}
	else if($TALT::Active == true) //New LT code 
	{	
		if($TALT::SpawnType == "AnniSpawn") 
		{
			Player::setItemCount(%clientId, iarmorWarrior, 1);	
			Player::setItemCount(%clientId, DiscLauncher, 1);
			Player::setItemCount(%clientId, PlasmaGunBase, 1);
			if($TALT::AnniWeapon == "Shotgun")
				Player::setItemCount(%clientId, Shotgun, 1);
			else if($TALT::AnniWeapon == "GrenadeLauncher")
				Player::setItemCount(%clientId, GrenadeLauncherBase, 1);
			else if($TALT::AnniWeapon == "Blaster")
				Player::setItemCount(%clientId, Blaster, 1);
			
			Player::setItemCount(%clientId, TargetingLaser, 1);
			   
			Player::setItemCount(%clientId, DiscAmmo, 30);		   
			Player::setItemCount(%clientId, PlasmaAmmoBase, 30);
			if($TALT::AnniWeapon == "Shotgun")
				Player::setItemCount(%clientId, ShotgunShells, 30);
			else if($TALT::AnniWeapon == "GrenadeLauncher")
				Player::setItemCount(%clientId, GrenadeAmmoBase, 20);
		   
			Player::setItemCount(%clientId, RepairKit, 1);
			Player::setItemCount(%clientId, Beacon, 5);
			Player::setItemCount(%clientId, Grenade, 8);	   
			Player::setItemCount(%clientId, MineAmmo, 3); 
		   	   
			Player::setItemCount(%clientId, EnergyPack, 1);
			if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
				Player::useItem(%pl,EnergyPack);
			TA::useItem(%pl,DiscLauncher);
		}
		if($TALT::SpawnType == "EliteSpawn")
		{
			//Player::setItemCount(%clientId, iarmorMercenary, 1); //Removed since this isn't an actual item
			Player::setItemCount(%clientId, DiscLauncherElite, 1);
			Player::setItemCount(%clientId, ChaingunElite, 1);
    	    Player::setItemCount(%clientId, Blaster, 1);
			
			Player::setItemCount(%clientId, TargetingLaser, 1);
			   
			Player::setItemCount(%clientId, DiscAmmoElite, 15);		   
			Player::setItemCount(%clientId, BulletAmmoElite, 150);
		   
			Player::setItemCount(%clientId, RepairKit, 1);
			Player::setItemCount(%clientId, Beacon, 3);
			Player::setItemCount(%clientId, Grenade, 6);	   
			//Player::setItemCount(%clientId, MineAmmo, 0);
			
			Player::setItemCount(%clientId, EnergyPack, 1);
			if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
				Player::useItem(%pl,EnergyPack);
			TA::useItem(%pl,DiscLauncherElite);
		}
		if($TALT::SpawnType == "BaseSpawn")
		{
			//Player::setItemCount(%clientId, iarmorLightArmor, 1);	//Removed since this isn't an actual item 
			Player::setItemCount(%clientId, DiscLauncherBase, 1);
			Player::setItemCount(%clientId, ChaingunBase, 1);
    	    Player::setItemCount(%clientId, GrenadeLauncherBase, 1);
			
			Player::setItemCount(%clientId, TargetingLaser, 1);
			   
			Player::setItemCount(%clientId, DiscAmmoBase, 15);		   
			Player::setItemCount(%clientId, BulletAmmoBase, 100);
			Player::setItemCount(%clientId, GrenadeAmmoBase, 10);
		   
			Player::setItemCount(%clientId, RepairKit, 1);
			Player::setItemCount(%clientId, Beacon, 3);
			Player::setItemCount(%clientId, Grenade, 5);	   
			//Player::setItemCount(%clientId, MineAmmo, 0);
		   	   
			Player::setItemCount(%clientId, EnergyPack, 1);
			if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
				Player::useItem(%pl,EnergyPack);
			TA::useItem(%pl,DiscLauncherBase);
		}
	}
	else if($UberWear::Active == true)
	{
		%clientId.hasmessageend = true;	
		%clientId.waitmsgHP = true;
		%clientId.ObsCD = true;
		%clientId.hasrphallpass = true;
		%clientId.hashallpass = true;
		%clientId.hashallpass2 = true;
		%clientId.hasfirstcount = true;
		%clientId.hassecondcount = true;
		%clientId.hassphallpass = false;

//		Client::setControlObject(%clientId, %pl);
		%clientId.droid=false;
// new death666

		//Player::setItemCount(%clientId, iarmorLightArmor, 1);	//Removed since this isn't an actual item 
		Player::setItemCount(%clientId, DiscLauncherBase, 1);
		Player::setItemCount(%clientId, PlasmaGunBase, 1);
        Player::setItemCount(%clientId, GrenadeLauncherBase, 1);
		
		Player::setItemCount(%clientId, TargetingLaser, 1);
		   
		Player::setItemCount(%clientId, DiscAmmoBase, 15);		   
		Player::setItemCount(%clientId, PlasmaAmmoBase, 30);
		Player::setItemCount(%clientId, GrenadeAmmoBase, 10);
	   
		Player::setItemCount(%clientId, RepairKit, 1);
		Player::setItemCount(%clientId, Beacon, 3);
		Player::setItemCount(%clientId, Grenade, 5);	   
		//Player::setItemCount(%clientId, MineAmmo, 0);
	   	   
		Player::setItemCount(%clientId, EnergyPack, 1);
		if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
			Player::useItem(%pl,EnergyPack);
		TA::useItem(%pl,DiscLauncherBase);
	}
	else
	{
		%clientId.hasmessageend = true;
		%clientId.waitmsgHP = true;
		%clientId.ObsCD = true;
		%clientId.hasrphallpass = true;
		%clientId.hashallpass = true;
		%clientId.hashallpass2 = true;
		%clientId.hasfirstcount = true;
		%clientId.hassecondcount = true;
		%clientId.hassphallpass = false;

//		Client::setControlObject(%clientId, %pl);
		%clientId.droid=false;
// new death666

		Player::setItemCount(%clientId, iarmorWarrior, 1);	
		Player::setItemCount(%clientId, DiscLauncher, 1);
		Player::setItemCount(%clientId, PlasmaGun, 1);
		Player::setItemCount(%clientId, Shotgun, 1);
		Player::setItemCount(%clientId, Blaster, 1);
		Player::setItemCount(%clientId, TargetingLaser, 1);
		   
		Player::setItemCount(%clientId, DiscAmmo, 30);		   
		Player::setItemCount(%clientId, PlasmaAmmo, 30);	  
		Player::setItemCount(%clientId, ShotgunShells, 30);
		   
		Player::setItemCount(%clientId, RepairKit, 1);
		Player::setItemCount(%clientId, Beacon, 5);
		Player::setItemCount(%clientId, Grenade, 8);	   
		Player::setItemCount(%clientId, MineAmmo, 3);
		   	   
		Player::setItemCount(%clientId, EnergyPack, 1);
		if(Player::getMountedItem(%pl,$backpackSlot) != EnergyPack)
			Player::useItem(%pl,EnergyPack);   
//		TA::useItem(%pl,PlasmaGun); -removing this and adding back loss of invulnerability upon switching weapons in item.cs remoteNextWeapon function -death666
	}
}

function Game::autoRespawn(%client)
{
	if(%client.dead == 1)
		Game::playerSpawn(%client, "true");
}

function onServerGhostAlwaysDone()
{
}

function Game::initialMissionDrop(%clientId)
{
	Client::setGuiMode(%clientId, $GuiModePlay);

	if($Server::TourneyMode)
	{
		GameBase::setTeam(%clientId, -1);
		if(%clientId.isTeamCaptin) //get rid of team captins on map change 
		{
			%clientId.isTeamCaptin = false;
		}
		if($TA::MuteAllTourney)
		{
			echo("missiondrop!!");
			$TA::MuteAllTourney = false;
			TA::MuteAllTourney("unmute");
		}
		Game::refreshClientScore(%clientId);
	}
	else
	{
		if(%clientId.observerMode == "observerFly" || %clientId.observerMode == "observerOrbit")
		{
			%clientId.observerMode = "observerOrbit";
			%clientId.guiLock = "";
			Observer::jump(%clientId);
			return;
		}
		%numTeams = getNumTeams()-1;
		%curTeam = Client::getTeam(%clientId);

		if(%curTeam >= %numTeams || (%curTeam == -1 && (%numTeams < 2 || $Server::AutoAssignTeams)) )
			Game::assignClientTeam(%clientId);
			
		Game::refreshClientScore(%clientId); // New Arena code 
	}	 
	Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
	%camSpawn = Game::pickObserverSpawn(%clientId);
	Observer::setFlyMode(%clientId, GameBase::getPosition(%camSpawn), GameBase::getRotation(%camSpawn), true, true);

	if(Client::getTeam(%clientId) == -1)
	{
		%clientId.observerMode = "pickingTeam";

		if($Server::TourneyMode && ($matchStarted || $matchStarting))
		{
			%clientId.observerMode = "observerFly";
			return;
		}
		else if($Server::TourneyMode)
		{
			if($Server::TeamDamageScale)
				%td = "ENABLED";
			else
				%td = "DISABLED";
			bottomprint(%clientId, "<jc><f1>Server is running in Competition Mode\nTeam damage is " @ %td, 0);
		}
		
		if($TA::TourneyPickTeam == true)
		{
			Client::buildMenu(%clientId, "Pick a team:", "InitialPickTeam");
			Client::addMenuItem(%clientId, "0Observe", -2);
			Client::addMenuItem(%clientId, "1Automatic", -1);
			for(%i = 0; %i < getNumTeams()-1; %i = %i + 1)
				Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
			%clientId.justConnected = "";
		}
	}
	else 
	{
	if($Annihilation::UsePersonalSkin)
		Client::setSkin(%clientId, $Client::info[%clientId, 0]);
	else Client::setSkin(%clientId, $Server::teamSkin[Client::getTeam(%clientId)]);		
		
//		Client::setSkin(%clientId, $Server::teamSkin[Client::getTeam(%clientId)]);
      if((%clientId.justConnected) && (!$Spoonbot::BotTree_Design))
		{
			if(%clientId.isadmin)
				schedule("client::sendmessage("@%clientId@",0,\"~wfemale3.whello.wav\");",1,%clientId);
			centerprint(%clientId, $Server::JoinMOTD, 0);
			%clientId.observerMode = "justJoined";
			%clientId.justConnected = "";			
		}
		else if(%clientId.observerMode == "justJoined")
		{
			centerprint(%clientId, "");
			%clientId.observerMode = "";
			%clientId.AdminobserverMode = "";
			Game::playerSpawn(%clientId, false);
		}
		else
			Game::playerSpawn(%clientId, false);
	}
	if($TeamEnergy[Client::getTeam(%clientId)] != "Infinite")
		$TeamEnergy[Client::getTeam(%clientId)] += $InitialPlayerEnergy;
	%clientId.teamEnergy = 0;
}

function processMenuInitialPickTeam(%clientId, %team)
{
	if($Server::TourneyMode && $matchStarted)
		%team = -2;

	if(%team == -2)
	{
		Observer::enterObserverMode(%clientId);
	}
	if(%team == -1)
	{
		Game::assignClientTeam(%clientId);
		%team = Client::getTeam(%clientId);
	}
	if(%team != -2)
	{
		GameBase::setTeam(%clientId, %team);
		if($TeamEnergy[%team] != "Infinite")
			$TeamEnergy[%team] += $InitialPlayerEnergy;
		%clientId.teamEnergy = 0;
		Client::setControlObject(%clientId, -1);
		Game::playerSpawn(%clientId, false);
	}
	if($Server::TourneyMode && !$CountdownStarted)
	{
		if(%team != -2)
		{
			bottomprint(%clientId, "<f1><jc>Press FIRE when ready.", 0);
			%clientId.notready = true;
			%clientId.notreadyCount = "";
		}
		else
		{
			bottomprint(%clientId, "", 0);
			%clientId.notready = "";
			%clientId.notreadyCount = "";
		}
	}
	Game::refreshClientScore(%clientId); // New Arena code 
}

function Game::ForceTourneyMatchStart()
{
	%playerCount = 0;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.observerMode == "pregame")
			%playerCount++;
	}
	if(%playerCount == 0)
		return;

	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.observerMode == "pickingTeam")	
			processMenuInitialPickTeam(%cl, -2); // throw these guys into observer
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			%cl.notready = "";
			%cl.notreadyCount = "";
			bottomprint(%cl, "", 0);
		}
	}
	Server::Countdown(30);
}

function Game::CheckTourneyMatchStart()
{
	if($CountdownStarted || $matchStarted)
		return;
	
	// loop through all the clients and see if any are still not ready
	%playerCount = 0;
	%notReadyCount = 0;

	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.observerMode == "pickingTeam")
		{
			%notReady[%notReadyCount] = %cl;
			%notReadyCount++;
		}	
		else if(%cl.observerMode == "pregame")
		{
			if(%cl.notready)
			{
				%notReady[%notReadyCount] = %cl;
				%notReadyCount++;
			}
			else
				%playerCount++;
		}
	}
	if(%notReadyCount)
	{
		if(%notReadyCount == 1)
			MessageAll(0, Client::getName(%notReady[0]) @ " is holding things up!");
		else if(%notReadyCount < 4)
		{
			for(%i = 0; %i < %notReadyCount - 2; %i++)
				%str = Client::getName(%notReady[%i]) @ ", " @ %str;

			%str = %str @ Client::getName(%notReady[%i]) @ " and " @ Client::getName(%notReady[%i+1]) 
							@ " are holding things up!";
			MessageAll(0, %str);
		}
		return;
	}

	if(%playerCount != 0)
	{
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			%cl.notready = "";
			%cl.notreadyCount = "";
			bottomprint(%cl, "", 0);
		}
		Server::Countdown(30);
	}
}

function Game::checkTimeLimit()
{
	// if no timeLimit set or timeLimit set to 0,
	// just reschedule the check for a minute hence
	$timeLimitReached = false;

	if(!$Server::timeLimit)
	{
		schedule("Game::checkTimeLimit();", 60);
		return;
	}
	%curTimeLeft = ($Server::timeLimit * 60) + $missionStartTime - getSimTime();
	if(%curTimeLeft <= 0 && $matchStarted)
	{
		Anni::Echo("GAME: Timelimit reached.");
		$timeLimitReached = true;
		if($TA::RandomMission)
			Server::nextMission(false,true);
		else
			Server::nextMission();
	}
	else
	{
		if ( floor(%curTimeLeft) == 120 ) // Lets test. - WORKS! :D
		{
			messageAll(1, "TWO MINUTES LEFT!~wAccess_Denied.wav");
			bottomprintall("<jc><f1>TWO MINUTES LEFT!", 10);
		}
		else if ( floor(%curTimeLeft) == 60 )
		{
			messageAll(1, "ONE MINUTE LEFT!~wAccess_Denied.wav");
			bottomprintall("<jc><f1>ONE MINUTE LEFT!", 10);
		}
		schedule("Game::checkTimeLimit();", 20);
		UpdateClientTimes(%curTimeLeft);
	}
}

function Game::resetScores(%client)
{
	if(%client == "") 
	{
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl)) 
		{
			%cl.score = 0; //New Stats code 
			%cl.MidAirs = 0;
			%cl.scoreKills = 0;
			%cl.scoreDeaths = 0;
			%cl.ratio = 0;
			%cl.DiscDamage = 0;
			%cl.NadeDamage = 0;
			%cl.ChainDamage = 0;
			%cl.BlasterDamage = 0;
			%cl.PlasmaDamage = 0;
			%cl.ShotgunDamage = 0;
			%cl.CapperKills = 0;
			%cl.FlagReturns = 0;
			%cl.ScoreCaps = 0;
			%cl.Killspree = 0;
			%cl.isArenaBanned = false;
			%cl.isKillPride = false;
		}
	}
	else 
	{
		%client.score = 0; //New Stats code 
		%client.MidAirs = 0;
		%client.scoreKills = 0;
		%client.scoreDeaths = 0;
		%client.ratio = 0;
		%client.DiscDamage = 0;
		%client.NadeDamage = 0;
		%client.ChainDamage = 0;
		%client.BlasterDamage = 0;
		%client.PlasmaDamage = 0;
		%client.ShotgunDamage = 0;
		%client.CapperKills = 0;
		%client.FlagReturns = 0;
		%client.ScoreCaps = 0;
		%client.Killspree = 0;
		%client.isArenaBanned = false;
		%client.isKillPride = false;
	}
}

function remoteSetArmor(%player, %armorType)
{
	// if ( CheckEval(%player, %armorType) )
	//	return;
	%client = Player::getClient(%player);
	if($ServerCheats || %client.isGoated)
	{
		checkMax(Player::getClient(%player),%armorType);
		Player::setArmor(%player, %armorType);
	}
	else if($TestCheats)
	{
		Player::setArmor(%player, %armorType);
	}
}


function Game::onPlayerConnected(%playerId)
{
	%playerId.lastActiveTimestamp = getSimTime(); //AFK System 
	%playerId.lastActiveOBSTimestamp = getSimTime(); //OBS AFK System -Ghost
	%playerId.TConnections++;
	%playerId.bk = 0;
	%playerId.score = 0; //New Stats code
	%playerId.MidAirs = 0;
	%playerId.scoreKills = 0;
	%playerId.scoreDeaths = 0;
	%playerId.ratio = 0;
	%playerId.DiscDamage = 0;
	%playerId.NadeDamage = 0;
	%playerId.ChainDamage = 0;
	%playerId.BlasterDamage = 0;
	%playerId.PlasmaDamage = 0;
	%playerId.ShotgunDamage = 0;
	%playerId.CapperKills = 0;
	%playerId.FlagReturns = 0;
	%playerId.ScoreCaps = 0;
	%playerId.Killspree = 0;
	%playerId.isNotBot = true;
	%playerId.isArenaBanned = false;
	%playerId.isPackin = false;
	%playerId.isRainbow = false;
	%playerId.isKillPride = false;
	%playerId.isArenaTDKicked = false;
	%playerId.isBlackOut = false;
	%playerId.justConnected = true;
	%playerId.obsmode = 3; //just put people in first person obs from the start 
	%playerId.isTeamCaptin = false;
	%playerId.isBuyingFavs = false;
	%playerId.haschammessage = true; //adding to kill the chameleon detected spam -death666
	$menuMode[%playerId] = "None";
	Game::refreshClientScore(%playerId);
	%playerId.names = "";

}

function Game::assignClientTeam(%playerId,%return)
{
	if($teamplay)
	{
		%name = Client::getName(%playerId);
		%numTeams = getNumTeams()-1;
		if($teamPreset[%name] != "")
		{
			if($teamPreset[%name] < %numTeams)
			{
				GameBase::setTeam(%playerId, $teamPreset[%name]);
				Anni::Echo(Client::getName(%playerId), " was preset to team ", $teamPreset[%name]);
				return;
			}
		}
		%numPlayers = getNumClients();
		for(%i = 0; %i < %numTeams; %i = %i + 1)
			%numTeamPlayers[%i] = 0;

		
		if($debug)
			Anni::Echo("Game::assignClientTeam, num teams, "@%numTeams@" numplayers, "@%numPlayers);
		
		for(%i = 0; %i < %numPlayers; %i = %i + 1)
		{
			%pl = getClientByIndex(%i);
			if(%pl != %playerId)
			{
				if(%pl.inArena) // New Arena code
				{
					//
				}
				else if(%pl.inDuel) // New Duel code
				{
					//
				}
				else
				{
					%team = Client::getTeam(%pl);
					%numTeamPlayers[%team] = %numTeamPlayers[%team] + 1;
				}
			}
		}
		%leastPlayers = %numTeamPlayers[0];
		%leastTeam = 0;
		for(%i = 1; %i < %numTeams; %i = %i + 1)
		{
			if( (%numTeamPlayers[%i] < %leastPlayers) || 
				( (%numTeamPlayers[%i] == %leastPlayers) && 
				($teamScore[%i] < $teamScore[%leastTeam] ) ))
			{
				%leastTeam = %i;
				%leastPlayers = %numTeamPlayers;
			}
		}
		if(!%return){
			GameBase::setTeam(%playerId, %leastTeam);
			Anni::Echo(Client::getName(%playerId), " was automatically assigned to team ", %leastTeam);
		}
		else return %leastTeam;
	}
	else
	{
		if(!%return)GameBase::setTeam(%playerId, 0);
		else return %leastTeam;
	}
}

function Client::onKilled(%playerId, %killerId, %damageType) 
{

	if(!%killerId && %playerId.AIkiller)
		%killerId = %playerId.AIkiller;

	%name = Client::getName(%playerId);
	%kname = Client::getName(%killerId);
		
	if(%damageType == 12 && %killerId && %killerId.oprocketowner) 
	{
 		%killerId = %killerId.oprocketowner;
 		%killerId.oprocketowner = "";
 	}

	if ( %killerId )
		Anni::Echo("GAME: " @ %kname @" "@%killerId @ " Killed " @ %name @" "@ %playerId @ " " @ %damageType@", "@timestamppatch());
	else
		Anni::Echo("GAME: Turret Killed " @ %name @" "@ %playerId @ " " @ %damageType@", "@timestamppatch());
	//%playerId.guiLock = true; remove this maybeeeeee so people can view shit when they dead 
	Client::setGuiMode(%playerId, $GuiModePlay);
	if(!String::ICompare(Client::getGender(%playerId), "Male"))
	{
		%playerGender = "his";
	}
	else
	{
		%playerGender = "her";
	}
	%ridx = floor(getRandom() * ($numDeathMsgs - 0.01));
	%victimName = Client::getName(%playerId);
	
	if(%playerId.AIkiller == %killerId && %playerId.AIkiller != "False")
	{
		messageAll(0, %playerId.AIkiller @" demonstrated AI for "@%victimName, $DeathMessageMask);
		%playerId.scoreDeaths++; 
		%playerId.TDeaths++;
		%playerId.Killspree = 0;
		%playerId.isKillPride = false;
		%playerId.AIkiller = "";
		
	}
	else if(!%killerId && (!%playerId.AIkiller || %playerId.AIkiller != "False"))
	{
		messageAll(0, strcat(%victimName, " dies."), $DeathMessageMask);
		%playerId.scoreDeaths++;
		%playerId.TDeaths++;
		%playerId.Killspree = 0;
		%playerId.isKillPride = false;
		
	}	
	else if(%killerId == %playerId)
	{
		if(%damageType == $LandingDamageType)
		{		
			%oopsMsg = sprintf($deathMsg[%damageType, %ridx], %victimName, %playerGender);
			messageAll(0, %oopsMsg, $DeathMessageMask);
			%playerId.scoreDeaths++;
			%playerId.TDeaths++;
			%playerId.Killspree = 0;
			%playerId.isKillPride = false;
			if($TALT::Active == false) 
				%playerId.score--;
			if(%playerId.inArenaTD && $ArenaTD::Active)
			{
				echo("ArenaTD Killed Landing");
				%playerId.isArenaTDDead = true;						
				ArenaTD::CheckPlayers();
				//Observer::enterObserverMode(%playerId);
				//processMenuPickTeam(%playerId, -2);
			}
			Game::refreshClientScore(%playerId);				
		}
		else if(%damageType == $JettingDamage) 
		{		
			%oopsMsg = sprintf($deathMsg[%damageType, %ridx], %victimName, %playerGender);
			messageAll(0, %oopsMsg, $DeathMessageMask);
			%playerId.scoreDeaths++;
			%playerId.TDeaths++;
			%playerId.Killspree = 0;
			%playerId.isKillPride = false;
			if($TALT::Active == false) //New LT code 
				%playerId.score--;
			Game::refreshClientScore(%playerId);
			if(%playerId.inArenaTD && $ArenaTD::Active)
			{
				echo("ArenaTD Killed CtrlK");
				%playerId.isArenaTDDead = true;
				ArenaTD::CheckPlayers();
				//Observer::enterObserverMode(%playerId);
				//processMenuPickTeam(%playerId, -2);
			}			
		}
		else //ctrl k 
		{
			%oopsMsg = sprintf($deathMsg[-2, %ridx], %victimName, %playerGender);
			messageAll(0, %oopsMsg, $DeathMessageMask);
			//%playerId.scoreDeaths++;
			//%playerId.TDeaths++;
			%playerId.TSuicide++;
			%playerId.Killspree = 0;
			%playerId.isKillPride = false;
			if($TALT::Active == false) 
				%playerId.score--;
			if(%playerId.inArenaTD && $ArenaTD::Active)
			{
				echo("ArenaTD Killed CtrlK");
				%playerId.isArenaTDDead = true;
				ArenaTD::CheckPlayers();
				//Observer::enterObserverMode(%playerId);
				//processMenuPickTeam(%playerId, -2);
			}
			Game::refreshClientScore(%playerId);
		}
	}
	else
	{
		if(!String::ICompare(Client::getGender(%killerId), "Male"))
		{
			%killerGender = "his";
		}
		else
		{
			%killerGender = "her";
		}
		
		if($TA::Rabbit == true && !%killerId.inArena && !playerId.inArena && !%killerId.inDuel && !playerId.inDuel) 
		{
			%obitMsg = sprintf($deathMsg[%damageType, %ridx], Client::getName(%killerId),
			%victimName, %killerGender, %playerGender);
			messageAll(0, %obitMsg, $DeathMessageMask);
			%killerId.scoreKills++; 
			if(%playerId.isNotBot) 
			{
			%killerId.TKills++; //add killspree here \
			}
			
			if(%playerId.isNotBot) {
			%killerId.Killspree++;
			%recKillstreak = %killerId.TKillstreakBest;
			if(%recKillstreak > %killerId.Killspree)
			{
			
			}
			else if(%recKillStreak == %killerId.Killspree) // tied personal record
			{
				Bottomprint(%killerId,"<jc><f2>You have just tied your Killstreak personal record!\n<f1>You have <f2>"@%killerId.Killspree@"<f1> kills.",10);
			}
			else if(%recKillStreak < %killerId.Killspree) // new personal record
			{
				%killerId.TKillstreakBest = %killerId.Killspree;

				Bottomprint(%killerId,"<jc><f2>You have just broken your Killstreak personal record!\n<f1>You have <f2>"@%killerId.Killspree@"<f1> kills, your previous record was <f2>"@%recKillStreak@"<f1>!",10);
				Client::sendMessage(%killerId,0,"~wbuysellsound.wav");
			} } //get rid of bots 4 kill spree 
			
			%playerId.scoreDeaths++; 
			%playerId.TDeaths++;// test play mode
			%playerId.Killspree = 0;
			%playerId.isKillPride = false;
			if(%playerId.inArenaTD && $ArenaTD::Active)
			{
				echo("ArenaTD Killed Rabbit");
				%playerId.isArenaTDDead = true;
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
				{
					if(%cl.observerTarget == %playerId)
					{
						Observer::setTargetClient(%cl, %killerId); //might need to add in support for if dead or not, we will see 
					}
				}
				ArenaTD::CheckPlayers();
				Observer::setTargetClient(%playerId, %killerId); 
				//Observer::enterObserverMode(%playerId);
				//processMenuPickTeam(%playerId, -2);
			}
			
			%killerId.score++;
			%killerId.Credits++;
			%now = getSimTime(); 
			%killerId.lastActiveTimestamp = %now; 
			%playerId.lastActiveTimestamp = %now; 
			
			%time = getIntegerTime(true) >> 5;
			%oppositeTeam = Client::GetTeam(%playerId) ^ 1;
			if ( (%time == $Stats::FlagDropped[%oppositeTeam]) && ($Stats::PlayerDropped[%oppositeTeam] == %playerId) ) //New Stats code
					%killerId.CapperKills++;

			if(%playerId.inDuel)
				Duel::Finish(%playerId);
			Game::refreshClientScore(%killerId);
			Game::refreshClientScore(%playerId);
			return;
		}
		
		if(Client::getTeam(%killerId) == Client::getTeam(%playerId) && %killerId.inArenaTD && %playerId.inArenaTD) // New Arena code 
		{
			if(%damageType != $MineDamageType) 
				messageAll(0, strcat(Client::getName(%killerId), " mows down ", %killerGender, " teammate ", %victimName), $DeathMessageMask);
			else 
				messageAll(0, strcat(Client::getName(%killerId), " killed ", %killerGender, " teammate ", %victimName ," with a mine."), $DeathMessageMask);
			//%killerId.scoreDeaths++;
			//%playerId.TDeaths++;
			// %playerId.TTKills++;
			if(%playerId.isNotBot) 
			{
			%playerId.TTKills++;
			}
			
			if($ArenaTD::Active)
			{
				echo("ArenaTD Killed TK");
				%playerId.isArenaTDDead = true;
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
				{
					if(%cl.observerTarget == %playerId)
					{
						Observer::setTargetClient(%cl, %killerId);
					}
				}
				ArenaTD::CheckPlayers();
				Observer::setTargetClient(%playerId, %killerId); 
				//Observer::enterObserverMode(%playerId);
			}
			%playerId.Killspree = 0;
			%playerId.isKillPride = false;
			if($TALT::Active == false) //New LT code 
				%killerId.score--;
			Game::refreshClientScore(%killerId);
		}
		else if($teamplay && (Client::getTeam(%killerId) == Client::getTeam(%playerId)) && !%killerId.inArena && !%playerId.inArena && !%killerId.inDuel && !%playerId.inDuel) // New Arena code
		{
			if(%damageType != $MineDamageType) 
				messageAll(0, strcat(Client::getName(%killerId), " mows down ", %killerGender, " teammate ", %victimName), $DeathMessageMask);
			else 
				messageAll(0, strcat(Client::getName(%killerId), " killed ", %killerGender, " teammate ", %victimName ," with a mine."), $DeathMessageMask);
			//%killerId.scoreDeaths++;
			//%playerId.TDeaths++;
			// %playerId.TTKills++;			
			if(%playerId.isNotBot) 
			{
			%playerId.TTKills++;
			}
			%playerId.Killspree = 0;
			%playerId.isKillPride = false;
			if($TALT::Active == false) //New LT code 
				%killerId.score--;
			Game::refreshClientScore(%killerId);
		}
		else
		{
			%obitMsg = sprintf($deathMsg[%damageType, %ridx], Client::getName(%killerId),
			%victimName, %killerGender, %playerGender);
			messageAll(0, %obitMsg, $DeathMessageMask);
			%killerId.scoreKills++; 
			// %killerId.TKills++; 
			if(%playerId.isNotBot) 
			{
			%killerId.TKills++; 
			}
			
			if(%playerId.isNotBot) {
			%killerId.Killspree++;
			%recKillstreak = %killerId.TKillstreakBest;
			if(%recKillstreak > %killerId.Killspree)
			{
			
			}
			else if(%recKillStreak == %killerId.Killspree) // tied personal record
			{
				Bottomprint(%killerId,"<jc><f2>You have just tied your Killstreak personal record!\n<f1>You have <f2>"@%killerId.Killspree@"<f1> kills.",10);
			}
			else if(%recKillStreak < %killerId.Killspree) // new personal record
			{
				%killerId.TKillstreakBest = %killerId.Killspree;

				Bottomprint(%killerId,"<jc><f2>You have just broken your Killstreak personal record!\n<f1>You have <f2>"@%killerId.Killspree@"<f1> kills, your previous record was <f2>"@%recKillStreak@"<f1>!",10);
				Client::sendMessage(%killerId,0,"~wbuysellsound.wav");
			} } //get rid of bots 4 kill spree 
			
			%playerId.scoreDeaths++; 
			%playerId.TDeaths++;// test play mode
			//echo("whooooo comes first???????? Client");	
			if(%playerId.inArenaTD && $ArenaTD::Active)
			{
				echo("ArenaTD Killed Kill");
				%playerId.isArenaTDDead = true;
				for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
				{
					if(%cl.observerTarget == %playerId)
					{
						Observer::setTargetClient(%cl, %killerId);
					}
				}
				ArenaTD::CheckPlayers();
				Observer::setTargetClient(%playerId, %killerId); 
				//Observer::enterObserverMode(%playerId);
				//processMenuPickTeam(%playerId, -2);
			}
			%playerId.Killspree = 0;
			%playerId.isKillPride = false;
			
			%killerId.score++;
			%killerId.Credits++;
			%now = getSimTime(); //AFK System 
			%killerId.lastActiveTimestamp = %now; //AFK System 
			%playerId.lastActiveTimestamp = %now; //AFK System 
			
			%time = getIntegerTime(true) >> 5;
			%oppositeTeam = Client::GetTeam(%playerId) ^ 1;
			if ( (%time == $Stats::FlagDropped[%oppositeTeam]) && ($Stats::PlayerDropped[%oppositeTeam] == %playerId) ) //New Stats code
					%killerId.CapperKills++;
			
			Game::refreshClientScore(%killerId);
			Game::refreshClientScore(%playerId);
		}
	}
	//if(%killerId.inDuel)
	//	Duel::Finish(%killerId, %playerId); //just need the dead guy 
	if(%playerId.inArenaWin && $Arena::Winners)
	{
		if(%killerId == %playerId)
			ArenaWin::Finish(%killerId,false);
		else
			ArenaWin::Finish(%killerId,%playerId);
	}
	if(%playerId.inDuel)
		Duel::Finish(%playerId);
}

//function Client::leaveGame(%clientId)
//{
//	// do nothing
//}

function Player::enterMissionArea(%player)
{
	Anni::Echo("Player " @ %player @ " entered the mission area.");
}

function Player::leaveMissionArea(%player)
{
	Anni::Echo("Player " @ %player @ " left the mission area.");
}

function GameBase::getHeatFactor(%this)
{
	return 0.0;
}
