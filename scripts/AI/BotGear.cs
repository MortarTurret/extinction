function AI::setWeapons(%aiName)
{
//	messageall(1, "Ai Set Weapons "@ %aiName @".");
	%aiId = BotFuncs::GetId(%aiName);
	if (%aiId==0)
	return;

	%player = Client::getOwnedObject(%aiId);
	
	   		%client = Player::GetClient(%aiId);
			 	// if(%client.mortMuted)return;
				   //		%client.mortMuted = 1;
	// schedule(%client@".mortMuted=0;",9.5,%client);
	
	// if($BotWeaponCooldown == "true")
	// {
	//	return;
	// }
	
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
	
	// $BotWeaponCooldown = true;
	// schedule("$BotWeaponCooldown=false;",3.0);

	%aiName = Client::GetName(%aiId);
//	AI::callbackPeriodic(%aiId, 5, ai::BotBrains);

//	AI::setVar(%aiName, iq, 90 );
	AI::setVar(%aiName, iq, 90 );

	//== In case some moron ignorant mapmaker (just kidding ;-)) forgot to name the bots according to the readme,
	//== they'll spawn with the ugly-useless standard weapons.

	// AI::SetVar(%aiName, triggerPct, 1000 );
	AI::setVar(%aiName, attackMode, 1);
	AI::setAutomaticTargets( %aiName );
//	AI::callbackPeriodic(%aiName, 5, ai::periodicWeaponChange);
   	%player = Client::getOwnedObject(%aiId);
	 %player.cnt = 0;
	
	// new
		 %player.democnt = 0;
		 %player.mortarcnt = 0;
		 %player.guardcnt = 0;
		 %player.paintercnt = 0;
		 %player.snipercnt = 0;
		 %player.mediccnt = 0;
		 %player.minercnt = 0;
		 %player.flyercnt = 0;

//			 	%switch = 7;
	 	%switch = floor(getRandom() * 8);
			if(%switch == 0)
			{
				 // messageall(1, "Player Is a Demo Bot.");
				%player.democnt++;
			}
			else if(%switch == 1)
			{
				//  messageall(1, "Player Is a Mortar Bot.");
				%player.mortarcnt++;
			}
			else if(%switch == 2)
			{
							%team = GameBase::getTeam( %aiId );
							%teamnum = %team;
							if (%teamnum == 0)
							{
								if($TeamGuardCount[%team] <= 0) 
								{
									// messageall(1, "Player is a Guard Bot.");
									%player.guardcnt++;
									$Spoonbot::Team0Guard = %aiId;
									$TeamGuardCount[%team] = 1;
								}
								else if($TeamGuardCount[%team] >= 1)
								{
									%player.democnt++;
									// messageall(1, "An extra demo was created.");
								}
							}
							if (%teamnum == 1)
							{
								if($TeamGuardCount[%team] <= 0) 						
								{
									// messageall(1, "Player is a Guard Bot.");
									%player.guardcnt++;
									$Spoonbot::Team1Guard = %aiId;
									$TeamGuardCount[%team] = 1;
								}
								else if($TeamGuardCount[%team] >= 1)
								{
									%player.democnt++;
								//	 messageall(1, "An extra demo was created.");
								}
							}
			}
			else if(%switch == 3)
			{
				// messageall(1, "Player is a Painter Bot.");
				%player.paintercnt++;
			}
			else if(%switch == 4)
			{
							%team = GameBase::getTeam( %aiId );
							%teamnum = %team;
							if (%teamnum == 0)
							{
								if($TeamSniperCount[%team] <= 0) 
								{
								//	 messageall(1, "Player is a Sniper Bot.");
									%player.snipercnt++;
									$Spoonbot::Team0Sniper = %aiId;
									$TeamSniperCount[%team] = 1;
								}
								else if($TeamSniperCount[%team] >= 1)
								{
									%player.minercnt++;
								//	 messageall(1, "An extra miner was created.");
								}
							}
							if (%teamnum == 1)
							{
								if($TeamSniperCount[%team] <= 0) 						
								{
								//	 messageall(1, "Player is a Sniper Bot.");
									%player.snipercnt++;
									$Spoonbot::Team1Sniper = %aiId;
									$TeamSniperCount[%team] = 1;
								}
								else if($TeamSniperCount[%team] >= 1)
								{
									%player.minercnt++;
								//	 messageall(1, "An extra miner was created.");
								}
							}
			}
			else if(%switch == 5)
			{
				// messageall(1, "Medic Bot Code Ran.");
							%team = GameBase::getTeam( %aiId );
							%teamnum = %team;
							if (%teamnum == 0)
							{
								if($TeamMedicCount[%team] <= 0) 
								{
									%player.mediccnt++;
									$Spoonbot::Team0Medic = %aiId;
									$TeamMedicCount[%team] = 1;
								//	 messageall(1, "Player is a Medic Bot.");
								}
								else if($TeamMedicCount[%team] >= 1)
								{
									%player.paintercnt++;
								//	 messageall(1, "An extra painter was created.");
								}
							}
							if (%teamnum == 1)
							{
								if($TeamMedicCount[%team] <= 0) 						
								{
									%player.mediccnt++;
									$Spoonbot::Team1Medic = %aiId;
									$TeamMedicCount[%team] = 1;
								//	 messageall(1, "Player is a Medic Bot.");
								}
								else if($TeamMedicCount[%team] >= 1)
								{
									%player.paintercnt++;
								//	 messageall(1, "An extra painter was created.");
								}
							}
			}
			else if(%switch == 6)
			{
			//	 messageall(1, "Player is a Miner Bot.");
				%player.minercnt++;
			}
			else if(%switch == 7)
			{
				%client = Player::GetClient(%aiId);	 	 	 	 
				%armor = player::getarmor(%client);
				%armorlist = $ArmorName[%armor].description;
				
							%team = GameBase::getTeam( %aiId );
							%teamnum = %team;
							if (%teamnum == 0)
							{
								if($TeamFlyerCount[%team] <= 0) 
								{
									%player.flyercnt++;
									$Spoonbot::Team0Flyer = %aiId;
									$TeamFlyerCount[%team] = 1;
								//	 messageall(1, "Player is a Flyer Bot.");
								}
								else if($TeamFlyerCount[%team] >= 1)
								{
									schedule("AI::setWeapons(" @ %aiId @ ");", 1);
								//	 messageall(1, "A flyer already exists.");
									return;
								}
							}
							if (%teamnum == 1)
							{
								if($TeamFlyerCount[%team] <= 0) 						
								{
									%player.flyercnt++;
									$Spoonbot::Team1Flyer = %aiId;
									$TeamFlyerCount[%team] = 1;
								//	 messageall(1, "Player is a Flyer Bot.");
								}
								else if($TeamFlyerCount[%team] >= 1)
								{
									schedule("AI::setWeapons(" @ %aiId @ ");", 1);
								//	 messageall(1, "A flyer already exists.");
									return;
								}
							}
				
				 // messageall(1, "Player is a Flyer Bot.");
				
			// $Actions::Deploys[%aiId] = true;
			 %id = AI::getId( %AIname );
			 $Actions::Deploys[%id] = true;
				
			if(%armorlist == "Troll")
			{
				AI::setVar( %AIname,  iq,  120 );
				AI::setVar( %AIname,  attackMode, 0);
				AI::DirectiveTarget(%AIname, %Victim);
				AI::callWithId(%AIName, Player::setItemCount, EnergyPack, 1);
				AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
				AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
				AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
				AI::callWithId(%AIName, Player::setItemCount, Mortar, 1);
				AI::callWithId(%AIName, Player::setItemCount, MortarAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, PhaseDisrupter, 1);
				AI::callWithId(%AIName, Player::setItemCount, PhaseAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, FlameThrower, 1);
				AI::callWithId(%AIName, Player::setItemCount, FlameThrowerAmmo, 200);
				AI::callWithId(%AIName, Player::setItemCount, Minigun, 1);
				AI::callWithId(%AIName, Player::setItemCount, MinigunAmmo, 200);
				AI::callWithId(%AIName, Player::setItemCount, RocketLauncher, 1);
				AI::callWithId(%AIName, Player::setItemCount, RocketAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, Disclauncher, 1);
				AI::callWithId(%AIName, Player::setItemCount, DiscAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, Flamer, 1);
				AI::callWithId(%AIName, Player::setItemCount, FlamerAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, LaserRifle, 1);
				AI::callWithId(%AIName, Player::setItemCount, GrenadeLauncher, 1);
				AI::callWithId(%AIName, Player::setItemCount, GrenadeAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, PlasmaGun, 1);
				AI::callWithId(%AIName, Player::setItemCount, PlasmaAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, RubberMortar, 1);
				AI::callWithId(%AIName, Player::setItemCount, RubberAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, Shotgun, 1);
				AI::callWithId(%AIName, Player::setItemCount, ShotgunShells, 100);
				AI::callWithId(%AIName, Player::setItemCount, Stinger, 1);
				AI::callWithId(%AIName, Player::setItemCount, StingerAmmo, 100);
				AI::callWithId(%AIName, Player::mountItem, Mortar, 0);
				AI::callWithId(%AIName, Player::mountItem, EnergyPack, $BackpackSlot);
				AI::setVar( "*", SpotDist, $visDistance);
				AI::SetAutomaticTargets(%AIName);
				AnniBotsHuntTroll(%aiName);
				$DoingChore[%aiId] = false;
				$Actions::Hunting[%aiId] = true;
				$Actions::Speaking[%aiId] = true;
				$Actions::Grenades[%aiId] = true;
				$Actions::Beacons[%aiId] = true;
				$Actions::Weapons[%aiId] = true;
				return;
			}
			else if(%armorlist == "Titan")
			{
				AI::setVar( %AIname,  iq,  120 );
				AI::setVar( %AIname,  attackMode, 0);
				AI::DirectiveTarget(%AIname, %Victim);
				AI::callWithId(%AIName, Player::setItemCount, EnergyPack, 1);
				AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
				AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
				AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
				AI::callWithId(%AIName, Player::setItemCount, RocketLauncher, 1);
				AI::callWithId(%AIName, Player::setItemCount, RocketAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, PhaseDisrupter, 1);
				AI::callWithId(%AIName, Player::setItemCount, PhaseAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, BabyNukeMortar, 1);
				AI::callWithId(%AIName, Player::setItemCount, BabyNukeAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, OSLauncher, 1);
				AI::callWithId(%AIName, Player::setItemCount, OSAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, ParticleBeamWeapon, 1);
				AI::callWithId(%AIName, Player::setItemCount, Disclauncher, 1);
				AI::callWithId(%AIName, Player::setItemCount, DiscAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, Flamer, 1);
				AI::callWithId(%AIName, Player::setItemCount, FlamerAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, FlameThrower, 1);
				AI::callWithId(%AIName, Player::setItemCount, FlameThrowerAmmo, 200);
				AI::callWithId(%AIName, Player::setItemCount, GrenadeLauncher, 1);
				AI::callWithId(%AIName, Player::setItemCount, GrenadeAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, PlasmaGun, 1);
				AI::callWithId(%AIName, Player::setItemCount, PlasmaAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, RubberMortar, 1);
				AI::callWithId(%AIName, Player::setItemCount, RubberAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, Stinger, 1);
				AI::callWithId(%AIName, Player::setItemCount, StingerAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, Thumper, 1);
				AI::callWithId(%AIName, Player::setItemCount, ThumperAmmo, 100);
				AI::callWithId(%AIName, Player::mountItem, RocketLauncher, 0);
				AI::callWithId(%AIName, Player::mountItem, EnergyPack, $BackpackSlot);
				AI::setVar( "*", SpotDist, $visDistance);
				AI::SetAutomaticTargets(%AIName);
				AnniBotsHuntTitan(%aiName);
				$DoingChore[%aiId] = false;
				$Actions::Hunting[%aiId] = true;
				$Actions::Speaking[%aiId] = true;
				$Actions::Grenades[%aiId] = true;
				$Actions::Beacons[%aiId] = true;
				$Actions::Weapons[%aiId] = true;
				return;
			}
			else if(%armorlist == "Tank")
			{
				AI::setVar( %AIname,  iq,  120 );
				AI::setVar( %AIname,  attackMode, 0);
				AI::DirectiveTarget(%AIname, %Victim);
				AI::callWithId(%AIName, Player::setItemCount, EnergyPack, 1);
				AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
				AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
				AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
				AI::callWithId(%AIName, Player::setItemCount, TBlastCannon, 1);
				AI::callWithId(%AIName, Player::setItemCount, TBlastCannonAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, TRocketLauncher, 1);
				AI::callWithId(%AIName, Player::setItemCount, TRocketLauncherAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, TankRPGLauncher, 1);
				AI::callWithId(%AIName, Player::setItemCount, TankRPGAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, TankShredder, 1);
				AI::callWithId(%AIName, Player::setItemCount, TankShredderAmmo, 200);
				AI::callWithId(%AIName, Player::mountItem, TBlastCannon, 0);
				AI::callWithId(%AIName, Player::mountItem, EnergyPack, $BackpackSlot);
				AI::setVar( "*", SpotDist, $visDistance);
				AI::SetAutomaticTargets(%AIName);
				AnniBotsHuntTank(%aiName);
				$DoingChore[%aiId] = false;
				$Actions::Hunting[%aiId] = true;
				$Actions::Speaking[%aiId] = true;
				$Actions::Grenades[%aiId] = true;
				$Actions::Beacons[%aiId] = true;
				$Actions::Weapons[%aiId] = true;
				return;
			}
			else if(%armorlist == "Warrior")
			{
				AI::setVar( %AIname,  iq,  120 );
				AI::setVar( %AIname,  attackMode, 0);
				AI::DirectiveTarget(%AIname, %Victim);
				AI::callWithId(%AIName, Player::setItemCount, AmmoPack, 1);
				AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
				AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
				AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
				AI::callWithId(%AIName, Player::setItemCount, Disclauncher, 1);
				AI::callWithId(%AIName, Player::setItemCount, DiscAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, Vulcan, 1);
				AI::callWithId(%AIName, Player::setItemCount, VulcanAmmo, 200);
				AI::callWithId(%AIName, Player::setItemCount, Stinger, 1);
				AI::callWithId(%AIName, Player::setItemCount, StingerAmmo, 50);
				AI::callWithId(%AIName, Player::setItemCount, FlameThrower, 1);
				AI::callWithId(%AIName, Player::setItemCount, FlameThrowerAmmo, 200);
				AI::callWithId(%AIName, Player::setItemCount, RocketLauncher, 1);
				AI::callWithId(%AIName, Player::setItemCount, RocketAmmo, 50);
				AI::callWithId(%AIName, Player::setItemCount, Blaster, 1);
				AI::callWithId(%AIName, Player::setItemCount, Flamer, 1);
				AI::callWithId(%AIName, Player::setItemCount, FlamerAmmo, 200);
				AI::callWithId(%AIName, Player::setItemCount, GrenadeLauncher, 1);
				AI::callWithId(%AIName, Player::setItemCount, GrenadeAmmo, 50);
				AI::callWithId(%AIName, Player::setItemCount, Hammer, 1);
				AI::callWithId(%AIName, Player::setItemCount, HammerAmmo, 50);
				AI::callWithId(%AIname, Player::setItemCount, PlasmaGun, 1);
				AI::callWithId(%AIname, Player::setItemCount, PlasmaAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, RubberMortar, 1);
				AI::callWithId(%AIName, Player::setItemCount, RubberAmmo, 50);
				AI::callWithId(%AIName, Player::setItemCount, ShockwaveGun, 1);
				AI::callWithId(%AIName, Player::setItemCount, Shotgun, 1);
				AI::callWithId(%AIName, Player::setItemCount, ShotgunShells, 100);
				AI::callWithId(%AIName, Player::setItemCount, Thumper, 1);
				AI::callWithId(%AIName, Player::setItemCount, ThumperAmmo, 50);
				AI::callWithId(%AIName, Player::mountItem, Disclauncher, 0);
				AI::callWithId(%AIName, Player::mountItem, AmmoPack, $BackpackSlot);
				AI::setVar( "*", SpotDist, $visDistance);
				AI::SetAutomaticTargets(%AIName);
				AnniBotsHuntWarrior(%aiName);
				$DoingChore[%aiId] = false;
				$Actions::Hunting[%aiId] = true;
				$Actions::Speaking[%aiId] = true;
				$Actions::Grenades[%aiId] = true;
				$Actions::Beacons[%aiId] = true;
				$Actions::Weapons[%aiId] = true;
				return;
			}
			else if(%armorlist == "Chameleon Assassin")
			{
				AI::setVar( %AIname,  iq,  120 );
				AI::setVar( %AIname,  attackMode, 0);
				AI::DirectiveTarget(%AIname, %Victim);
				AI::callWithId(%AIName, Player::setItemCount, EnergyPack, 1);
				AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
				AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
				AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
				AI::callWithId(%AIName, Player::setItemCount, PlasmaGun, 1);
				AI::callWithId(%AIName, Player::setItemCount, PlasmaAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, SniperRifle, 1);
				AI::callWithId(%AIName, Player::setItemCount, SniperAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, Disclauncher, 1);
				AI::callWithId(%AIName, Player::setItemCount, DiscAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, Shotgun, 1);
				AI::callWithId(%AIName, Player::setItemCount, ShotgunShells, 100);
				AI::callWithId(%AIName, Player::setItemCount, Flamer, 1);
				AI::callWithId(%AIName, Player::setItemCount, FlamerAmmo, 200);
				AI::callWithId(%AIName, Player::setItemCount, GrenadeLauncher, 1);
				AI::callWithId(%AIName, Player::setItemCount, GrenadeAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, Hammer, 1);
				AI::callWithId(%AIName, Player::setItemCount, HammerAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, ShockwaveGun, 1);
				AI::callWithId(%AIName, Player::setItemCount, LaserRifle, 1);
				AI::callWithId(%AIName, Player::setItemCount, Thumper, 1);
				AI::callWithId(%AIName, Player::setItemCount, ThumperAmmo, 100);
				AI::callWithId(%AIName, Player::mountItem, Shotgun, 0);
				AI::callWithId(%AIName, Player::mountItem, EnergyPack, $BackpackSlot);
				AI::setVar( "*", SpotDist, $visDistance);
				AI::SetAutomaticTargets(%AIName);
				AnniBotsHuntSpy(%aiName);
				$DoingChore[%aiId] = false;
				$Actions::Hunting[%aiId] = true;
				$Actions::Speaking[%aiId] = true;
				$Actions::Grenades[%aiId] = true;
				$Actions::Beacons[%aiId] = true;
				$Actions::Weapons[%aiId] = true;
				return;
			}
			else if(%armorlist == "Builder")
			{
				AI::setVar( %AIname,  iq,  120 );
				AI::setVar( %AIname,  attackMode, 0);
				AI::DirectiveTarget(%AIname, %Victim);
				AI::callWithId(%AIName, Player::setItemCount, DeployableInvPack, 1);
				AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
				AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
				AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
				AI::callWithId(%AIName, Player::setItemCount, Railgun, 1);
				AI::callWithId(%AIName, Player::setItemCount, RailAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, Disclauncher, 1);
				AI::callWithId(%AIName, Player::setItemCount, DiscAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, RocketLauncher, 1);
				AI::callWithId(%AIName, Player::setItemCount, RocketAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, Pitchfork, 1);
				AI::callWithId(%AIName, Player::setItemCount, Flamer, 1);
				AI::callWithId(%AIName, Player::setItemCount, FlamerAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, GrenadeLauncher, 1);
				AI::callWithId(%AIName, Player::setItemCount, GrenadeAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, PlasmaGun, 1);
				AI::callWithId(%AIName, Player::setItemCount, PlasmaAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, Shotgun, 1);
				AI::callWithId(%AIName, Player::setItemCount, ShotgunShells, 100);
				AI::callWithId(%AIName, Player::setItemCount, Stinger, 1);
				AI::callWithId(%AIName, Player::setItemCount, StingerAmmo, 100);
				AI::callWithId(%AIname, Player::setItemCount, Vulcan, 1);
				AI::callWithId(%AIname, Player::setItemCount, VulcanAmmo, 100);
				AI::callWithId(%AIName, Player::setItemCount, Thumper, 1);
				AI::callWithId(%AIName, Player::setItemCount, ThumperAmmo, 100);
				AI::callWithId(%AIName, Player::mountItem, GrenadeLauncher, 0);
				AI::callWithId(%AIName, Player::mountItem, DeployableInvPack, $BackpackSlot);
				AI::setVar( "*", SpotDist, $visDistance);
				AI::SetAutomaticTargets(%AIName);
				AnniBotsHuntBuilder(%aiName);
				$DoingChore[%aiId] = false;
				$Actions::Hunting[%aiId] = true;
				$Actions::Speaking[%aiId] = true;
				$Actions::Grenades[%aiId] = true;
				$Actions::Beacons[%aiId] = true;
				$Actions::Weapons[%aiId] = true;
				return;
			}
			else if(%armorlist == "Necromancer")
			{
				AI::setVar( %AIname,  iq,  120 );
				AI::setVar( %AIname,  attackMode, 0);
				AI::DirectiveTarget(%AIname, %Victim);
				AI::callWithId(%AIName, Player::setItemCount, PhaseShifterPack, 1);
				AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
				AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
				AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
				AI::callWithId(%AIName, Player::setItemCount, DisarmerSpell, 1);
				AI::callWithId(%AIName, Player::setItemCount, ShockingGrasp, 1);
				AI::callWithId(%AIName, Player::setItemCount, SpellFlameThrower, 1);
				AI::callWithId(%AIName, Player::setItemCount, DeathRay, 1);
				AI::callWithId(%AIName, Player::setItemCount, FlameStrike, 1);
				AI::callWithId(%AIName, Player::setItemCount, Stasis, 1);
				AI::callWithId(%AIName, Player::mountItem, SpellFlameThrower, 0);
				AI::callWithId(%AIName, Player::mountItem, PhaseShifterPack, $BackpackSlot);
				AI::setVar( "*", SpotDist, $visDistance);
				AI::SetAutomaticTargets(%AIName);
				AnniBotsHuntNecro(%aiName);
				$DoingChore[%aiId] = false;
				$Actions::Hunting[%aiId] = true;
				$Actions::Speaking[%aiId] = true;
				$Actions::Grenades[%aiId] = true;
				$Actions::Beacons[%aiId] = true;
				$Actions::Weapons[%aiId] = true;
				return;
			}
			}
	// end new
		 
//		 AI::setVar( "*", SpotDist, 350);
		 AI::setVar( "*", SpotDist, $visDistance);
		 AI::SetVar( "*", triggerPct, 1000 );
					
   	%client = Player::GetClient(%aiId);
	
	 %player.cnt = 0;
	 %player.telecnt = 0;
	 	 	 	 
		 %armor = player::getarmor(%client);
		 %armorlist = $ArmorName[%armor].description;
		 
		  %id = AI::getId( %AIname );
		  $Actions::Grenades[%id] = true;
		  $Actions::Beacons[%id] = true;
		  $Actions::Weapons[%id] = true;
		  $Actions::Deploys[%id] = true;

	if(%armorlist == "Troll")
	{
//		messageall(1, "Ai Set Weapons Troll.");
	         AI::setVar(%aiName, iq, 90 );
			 // new below from oldSBWeaponChange
			     	 AI::callWithId(%aiName, Player::setItemCount, SuicidePack, 1);
 //   	 AI::callWithId(%AIName, Player::setItemCount, TargetingLaser, 1);
    	 AI::callWithId(%aiName, Player::setItemCount, RepairKit, 1);
    	 AI::callWithId(%aiName, Player::setItemCount, MineAmmo, 50);
         AI::callWithId(%aiName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%aiName, Player::setItemCount, Beacon, 100);
         AI::callWithId(%aiName, Player::setItemCount, Mortar, 1);
         AI::callWithId(%aiName, Player::setItemCount, MortarAmmo, 99);
         AI::callWithId(%aiName, Player::setItemCount, PhaseDisrupter, 1);
         AI::callWithId(%aiName, Player::setItemCount, PhaseAmmo, 99);
         AI::callWithId(%AIName, Player::setItemCount, FlameThrower, 1);
         AI::callWithId(%AIName, Player::setItemCount, FlameThrowerAmmo, 99);
         AI::callWithId(%AIName, Player::setItemCount, Minigun, 1);
         AI::callWithId(%AIName, Player::setItemCount, MinigunAmmo, 999);
         AI::callWithId(%AIName, Player::setItemCount, RocketLauncher, 1);
         AI::callWithId(%AIName, Player::setItemCount, RocketAmmo, 99);
//         AI::callWithId(%AIName, Player::setItemCount, Disclauncher, 1);
  //       AI::callWithId(%AIName, Player::setItemCount, DiscAmmo, 99);
   // 	 AI::callWithId(%AIName, Player::setItemCount, Flamer, 1);
    //	 AI::callWithId(%AIName, Player::setItemCount, FlamerAmmo, 99);
    	 AI::callWithId(%AIName, Player::setItemCount, LaserRifle, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeAmmo, 99);
    //	 AI::callWithId(%AIName, Player::setItemCount, PlasmaGun, 1);
    //	 AI::callWithId(%AIName, Player::setItemCount, PlasmaAmmo, 99);
    	 AI::callWithId(%aiName, Player::setItemCount, RubberMortar, 1);
    	 AI::callWithId(%aiName, Player::setItemCount, RubberAmmo, 99);
    //	 AI::callWithId(%AIName, Player::setItemCount, Shotgun, 1);
    //	 AI::callWithId(%AIName, Player::setItemCount, ShotgunShells, 99);
    	 AI::callWithId(%aiName, Player::setItemCount, Stinger, 1);
    	 AI::callWithId(%aiName, Player::setItemCount, StingerAmmo, 99);
		 	 AI::callWithId(%AIName, Player::mountItem, Mortar, 0);
	 AI::callWithId(%AIName, Player::mountItem, SuicidePack, $BackpackSlot);
//			 ai::AnniSwitchWeaponSBTroll(%aiName);
//			 AI::callbackPeriodic(%aiName, 5, ai::AnniSwitchWeaponSBTroll);
//			if(%player.mortarcnt >= 1)
//			{
		if(BotTypes::IsMortar(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMortar);
			 return;
		}
		if(BotTypes::IsPainter(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsPainter);
			 return;
		}
		if(BotTypes::IsMiner(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMiner);
			 return;			 
		}
		if(BotTypes::IsSniper(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsSniper);
			 return;			 
		}
		if(BotTypes::IsDemo(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsDemo);
			 return;			 
		}
		if(BotTypes::IsGuard(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsGuard);
			 return;			 
		}
		if(BotTypes::IsMedic(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMedic);
			 return;			 
		}
//			}

			 return;
	}
	else if(%armorlist == "Titan")
	{
//				messageall(1, "Ai Set Weapons Titan.");
	    	 AI::setVar(%aiName, iq, 90 );
			 // new below from oldSBWeaponChange
			     	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
//    	 AI::callWithId(%AIName, Player::setItemCount, TargetingLaser, 1);
    	 AI::callWithId(%aiName, Player::setItemCount, MineAmmo, 50);
         AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, RocketLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RocketAmmo, 5000);
    	 AI::callWithId(%AIName, Player::setItemCount, PhaseDisrupter, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, PhaseAmmo, 5000);
    //	 AI::callWithId(%AIName, Player::setItemCount, BabyNukeMortar, 1);
   // 	 AI::callWithId(%AIName, Player::setItemCount, BabyNukeAmmo, 5000);
    	// AI::callWithId(%AIName, Player::setItemCount, OSLauncher, 1);
    	// AI::callWithId(%AIName, Player::setItemCount, OSAmmo, 5000);
         AI::callWithId(%AIName, Player::setItemCount, ParticleBeamWeapon, 1);
    	// AI::callWithId(%AIName, Player::setItemCount, Disclauncher, 1);
    	// AI::callWithId(%AIName, Player::setItemCount, DiscAmmo, 5000);
    	// AI::callWithId(%AIName, Player::setItemCount, Flamer, 1);
    	// AI::callWithId(%AIName, Player::setItemCount, FlamerAmmo, 5000);
    	// AI::callWithId(%AIName, Player::setItemCount, FlameThrower, 1);
    	// AI::callWithId(%AIName, Player::setItemCount, FlameThrowerAmmo, 5000);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeAmmo, 5000);
    	// AI::callWithId(%AIName, Player::setItemCount, PlasmaGun, 1);
    	// AI::callWithId(%AIName, Player::setItemCount, PlasmaAmmo, 5000);
    	 AI::callWithId(%AIName, Player::setItemCount, RubberMortar, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RubberAmmo, 99);
    	 AI::callWithId(%AIName, Player::setItemCount, Stinger, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, StingerAmmo, 5000);
    	// AI::callWithId(%AIName, Player::setItemCount, Thumper, 1);
    	// AI::callWithId(%AIName, Player::setItemCount, ThumperAmmo, 5000);
			 AI::callWithId(%AIName, Player::mountItem, RocketLauncher, 0);
    	 AI::callWithId(%AIName, Player::setItemCount, ShieldPack, 1);
	 AI::callWithId(%AIName, Player::mountItem, ShieldPack, $BackpackSlot);
//			 ai::AnniSwitchWeaponSBTitan(%aiName);
//			 			 AI::callbackPeriodic(%aiName, 5, ai::AnniSwitchWeaponSBTitan);
//			if(%player.mortarcnt >= 1)
//			{
		if(BotTypes::IsMortar(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMortar);
			 return;
		}
		if(BotTypes::IsPainter(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsPainter);
			 return;
		}
		if(BotTypes::IsMiner(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMiner);
			 return;			 
		}
		if(BotTypes::IsSniper(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsSniper);
			 return;			 
		}
		if(BotTypes::IsDemo(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsDemo);
			 return;			 
		}
		if(BotTypes::IsGuard(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsGuard);
			 return;			 
		}
		if(BotTypes::IsMedic(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMedic);
			 return;			 
		}
//			}
			 			 return;
	}
	else if(%armorlist == "Tank")
	{
		 AI::setVar( %AIname,  iq,  90 );
	//			messageall(1, "Ai Set Weapons Tank.");
		// new below from oldSBWeaponChange
		    	 AI::callWithId(%AIName, Player::setItemCount, SensorJammerPack, 1);
//    	 AI::callWithId(%AIName, Player::setItemCount, TargetingLaser, 1);
    	 AI::callWithId(%aiName, Player::setItemCount, MineAmmo, 50);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
         AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
         AI::callWithId(%AIName, Player::setItemCount, TBlastCannon, 1);
         AI::callWithId(%AIName, Player::setItemCount, TBlastCannonAmmo, 5000);
         AI::callWithId(%AIName, Player::setItemCount, TRocketLauncher, 1);
         AI::callWithId(%AIName, Player::setItemCount, TRocketLauncherAmmo, 5000);
         AI::callWithId(%AIName, Player::setItemCount, TankRPGLauncher, 1);
         AI::callWithId(%AIName, Player::setItemCount, TankRPGAmmo, 5000);
          AI::callWithId(%AIName, Player::setItemCount, TankShredder, 1);
         AI::callWithId(%AIName, Player::setItemCount, TankShredderAmmo, 5000);
		 	 AI::callWithId(%AIName, Player::mountItem, TankRPGLauncher, 0);
	 AI::callWithId(%AIName, Player::mountItem, SensorJammerPack, $BackpackSlot);
		 // ai::AnniSwitchWeaponSBTank(%aiName);
//		 			 AI::callbackPeriodic(%aiName, 5, ai::AnniSwitchWeaponSBTank);
//			if(%player.mortarcnt >= 1)
//			{
		if(BotTypes::IsMortar(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMortar);
			 return;
		}
		if(BotTypes::IsPainter(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsPainter);
			 return;
		}
		if(BotTypes::IsMiner(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMiner);
			 return;			 
		}
		if(BotTypes::IsSniper(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsSniper);
			 return;			 
		}
		if(BotTypes::IsDemo(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsDemo);
			 return;			 
		}
		if(BotTypes::IsGuard(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsGuard);
			 return;			 
		}
		if(BotTypes::IsMedic(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMedic);
			 return;			 
		}
//			}
		 			 return;
	}
	else if(%armorlist == "Warrior")
	{
	//			messageall(1, "Ai Set Weapons Warrior.");
//	    	 AI::SetVar(%aiName, spotDist, 50);
	    	 AI::SetVar(%aiName, seekOff , 1);
	        AI::setVar(%aiName, iq, 200 );
			 // new below from oldSBWeaponChange
			     	 AI::callWithId(%AIName, Player::setItemCount, StealthShieldPack, 1);
    	 AI::callWithId(%aiName, Player::setItemCount, MineAmmo, 50);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
         AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
         AI::callWithId(%AIName, Player::setItemCount, Disclauncher, 1);
         AI::callWithId(%AIName, Player::setItemCount, DiscAmmo, 5000);
         AI::callWithId(%AIName, Player::setItemCount, Vulcan, 1);
         AI::callWithId(%AIName, Player::setItemCount, VulcanAmmo, 5000);
         AI::callWithId(%AIName, Player::setItemCount, Stinger, 1);
         AI::callWithId(%AIName, Player::setItemCount, StingerAmmo, 5000);
        // AI::callWithId(%AIName, Player::setItemCount, FlameThrower, 1);
        // AI::callWithId(%AIName, Player::setItemCount, FlameThrowerAmmo, 5000);
         AI::callWithId(%AIName, Player::setItemCount, RocketLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RocketAmmo, 99);
    	 // AI::callWithId(%AIName, Player::setItemCount, TargetingLaser, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, Blaster, 1);
    	// AI::callWithId(%AIName, Player::setItemCount, Flamer, 1);
    	// AI::callWithId(%AIName, Player::setItemCount, FlamerAmmo, 5000);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeAmmo, 5000);
    	AI::callWithId(%AIName, Player::setItemCount, Hammer, 1);
    	AI::callWithId(%AIName, Player::setItemCount, HammerAmmo, 5000);
    	 AI::callWithId(%AIname, Player::setItemCount, PlasmaGun, 1);
	 AI::callWithId(%AIname, Player::setItemCount, PlasmaAmmo, 5000);
     AI::callWithId(%AIName, Player::setItemCount, RubberMortar, 1);
    	AI::callWithId(%AIName, Player::setItemCount, RubberAmmo, 5000);
    	// AI::callWithId(%AIName, Player::setItemCount, ShockwaveGun, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, Shotgun, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, ShotgunShells, 500);
    	// AI::callWithId(%AIName, Player::setItemCount, Thumper, 1);
    	// AI::callWithId(%AIName, Player::setItemCount, ThumperAmmo, 5000);
			 AI::callWithId(%AIName, Player::mountItem, Disclauncher, 0);
	 AI::callWithId(%AIName, Player::mountItem, StealthShieldPack, $BackpackSlot);
//			 ai::AnniSwitchWeaponSBWarrior(%aiName);
//			 			 AI::callbackPeriodic(%aiName, 5, ai::AnniSwitchWeaponSBWarrior);
//			if(%player.mortarcnt >= 1)
//			{
		if(BotTypes::IsMortar(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMortar);
			 return;
		}
		if(BotTypes::IsPainter(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsPainter);
			 return;
		}
		if(BotTypes::IsMiner(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMiner);
			 return;			 
		}
		if(BotTypes::IsSniper(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsSniper);
			 return;			 
		}
		if(BotTypes::IsDemo(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsDemo);
			 return;			 
		}
		if(BotTypes::IsGuard(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsGuard);
			 return;			 
		}
		if(BotTypes::IsMedic(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMedic);
			 return;			 
		}
//			}
			 			 return;
	}
	else if(%armorlist == "Chameleon Assassin")
	{
	//			messageall(1, "Ai Set Weapons Spy.");
//	    	 AI::SetVar(%aiName, spotDist, 370);		// Make 20-20 vision for snipers 
	    	 AI::SetVar(%aiName, seekOff , 1);  
	   	 AI::setVar(%aiName, iq, 80 );
		 // new below from oldSBWeaponChange
		 	 Player::setDetectParameters(%aiId, 0.0001, 0.00001);
//    	 AI::callWithId(%AIName, Player::setItemCount, TargetingLaser, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, AirStrikePack, 1);
    	 AI::callWithId(%aiName, Player::setItemCount, MineAmmo, 50);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
         AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
         AI::callWithId(%AIName, Player::setItemCount, PlasmaGun, 1);
        AI::callWithId(%AIName, Player::setItemCount, PlasmaAmmo, 99);
         AI::callWithId(%AIName, Player::setItemCount, SniperRifle, 1);
         AI::callWithId(%AIName, Player::setItemCount, SniperAmmo, 100);
         AI::callWithId(%AIName, Player::setItemCount, Disclauncher, 1);
         AI::callWithId(%AIName, Player::setItemCount, DiscAmmo, 99);
         AI::callWithId(%AIName, Player::setItemCount, Shotgun, 1);
         AI::callWithId(%AIName, Player::setItemCount, ShotgunShells, 5000);
    	// AI::callWithId(%AIName, Player::setItemCount, Flamer, 1);
    	// AI::callWithId(%AIName, Player::setItemCount, FlamerAmmo, 5000);
    	// AI::callWithId(%AIName, Player::setItemCount, GrenadeLauncher, 1);
    	// AI::callWithId(%AIName, Player::setItemCount, GrenadeAmmo, 5000);
    	 AI::callWithId(%AIName, Player::setItemCount, Hammer, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, HammerAmmo, 5000);
    	 AI::callWithId(%AIName, Player::setItemCount, ShockwaveGun, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, LaserRifle, 1);
    	// AI::callWithId(%AIName, Player::setItemCount, Thumper, 1);
    	// AI::callWithId(%AIName, Player::setItemCount, ThumperAmmo, 5000);
		   Player::setDetectParameters(%aiId, 0.0001, 0.00001);
		   	 AI::callWithId(%AIName, Player::mountItem, Shotgun, 0);
	 AI::callWithId(%AIName, Player::mountItem, AirStrikePack, $BackpackSlot);
//		 ai::AnniSwitchWeaponSBSpy(%aiName);
//		 			 AI::callbackPeriodic(%aiName, 5, ai::AnniSwitchWeaponSBSpy);
//			if(%player.mortarcnt >= 1)
//			{
		if(BotTypes::IsMortar(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMortar);
			 return;
		}
		if(BotTypes::IsPainter(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsPainter);
			 return;
		}
		if(BotTypes::IsMiner(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMiner);
			 return;			 
		}
		if(BotTypes::IsSniper(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsSniper);
			 return;			 
		}
		if(BotTypes::IsDemo(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsDemo);
			 return;			 
		}
		if(BotTypes::IsGuard(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsGuard);
			 return;			 
		}
		if(BotTypes::IsMedic(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMedic);
			 return;			 
		}
//			}
		 			 return;
	}
	else if(%armorlist == "Builder")
	{
	//			messageall(1, "Ai Set Weapons Builder.");
	   	 AI::setVar(%aiName, iq, 70 );
		 // new below from oldSBWeaponChange
		     	 AI::callWithId(%AIName, Player::setItemCount, RepairPack, 1);
//    	 AI::callWithId(%AIName, Player::setItemCount, TargetingLaser, 1);
    	 AI::callWithId(%aiName, Player::setItemCount, MineAmmo, 50);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
         AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
         AI::callWithId(%AIName, Player::setItemCount, Railgun, 1);
         AI::callWithId(%AIName, Player::setItemCount, RailAmmo, 5000);
        AI::callWithId(%AIName, Player::setItemCount, Disclauncher, 1);
         AI::callWithId(%AIName, Player::setItemCount, DiscAmmo, 99);
         AI::callWithId(%AIName, Player::setItemCount, RocketLauncher, 1);
         AI::callWithId(%AIName, Player::setItemCount, RocketAmmo, 5000);
        // AI::callWithId(%AIName, Player::setItemCount, Pitchfork, 1);
    	AI::callWithId(%AIName, Player::setItemCount, Flamer, 1);
    	AI::callWithId(%AIName, Player::setItemCount, FlamerAmmo, 99);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeAmmo, 5000);
    	 AI::callWithId(%AIName, Player::setItemCount, PlasmaGun, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, PlasmaAmmo, 5000);
    //	 AI::callWithId(%AIName, Player::setItemCount, Shotgun, 1);
    //	 AI::callWithId(%AIName, Player::setItemCount, ShotgunShells, 5000);
    	 AI::callWithId(%AIName, Player::setItemCount, Stinger, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, StingerAmmo, 5000);
    	// AI::callWithId(%AIname, Player::setItemCount, Vulcan, 1);
	// AI::callWithId(%AIname, Player::setItemCount, VulcanAmmo, 5000);
    	 AI::callWithId(%AIName, Player::setItemCount, Thumper, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, ThumperAmmo, 500);
		 	 AI::callWithId(%AIName, Player::mountItem, GrenadeLauncher, 0);
	 AI::callWithId(%AIName, Player::mountItem, RepairPack, $BackpackSlot);
		   Player::setDetectParameters(%aiId, 0.0001, 0.00001);
//		 ai::AnniSwitchWeaponSBBuilder(%aiName);
//		 			 AI::callbackPeriodic(%aiName, 5, ai::AnniSwitchWeaponSBBuilder);
//			if(%player.mortarcnt >= 1)
//			{
		if(BotTypes::IsMortar(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMortar);
			 return;
		}
		if(BotTypes::IsPainter(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsPainter);
			 return;
		}
		if(BotTypes::IsMiner(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMiner);
			 return;			 
		}
		if(BotTypes::IsSniper(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsSniper);
			 return;			 
		}
		if(BotTypes::IsDemo(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsDemo);
			 return;			 
		}
		if(BotTypes::IsGuard(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsGuard);
			 return;			 
		}
		if(BotTypes::IsMedic(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMedic);
			 return;			 
		}
//			}
		 			 return;
	}
	else if(%armorlist == "Necromancer")
	{
	//			messageall(1, "Ai Set Weapons Necromancer.");
	   	AI::setVar(%aiName, iq, 70 );
		 // new below from oldSBWeaponChange
		     	 AI::callWithId(%AIName, Player::setItemCount, GhostPack, 1);
//    	 AI::callWithId(%AIName, Player::setItemCount, TargetingLaser, 1);
    	 AI::callWithId(%aiName, Player::setItemCount, MineAmmo, 50);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
         AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
         AI::callWithId(%AIName, Player::setItemCount, DisarmerSpell, 1);
         AI::callWithId(%AIName, Player::setItemCount, ShockingGrasp, 1);
         AI::callWithId(%AIName, Player::setItemCount, SpellFlameThrower, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, DeathRay, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, FlameStrike, 1);
    	// AI::callWithId(%AIName, Player::setItemCount, Stasis, 1);
			 AI::callWithId(%AIName, Player::mountItem, SpellFlameThrower, 0);
	 AI::callWithId(%AIName, Player::mountItem, GhostPack, $BackpackSlot);
//		 ai::AnniSwitchWeaponSBNecro(%aiName);
//		 			 AI::callbackPeriodic(%aiName, 5, ai::AnniSwitchWeaponSBNecro);
//			if(%player.mortarcnt >= 1)
//			{
		if(BotTypes::IsMortar(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMortar);
			 return;
		}
		if(BotTypes::IsPainter(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsPainter);
			 return;
		}
		if(BotTypes::IsMiner(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMiner);
			 return;			 
		}
		if(BotTypes::IsSniper(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsSniper);
			 return;			 
		}
		if(BotTypes::IsDemo(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsDemo);
			 return;			 
		}
		if(BotTypes::IsGuard(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsGuard);
			 return;			 
		}
		if(BotTypes::IsMedic(%aiName) == 1)
		{
			 AI::callbackPeriodic(%aiName, 5, ai::BotBrainsMedic);
			 return;			 
		}
//			}
		 			 return;
	}
	else
	{
			 return;
	}
}

function ai::AnniSwitchWeaponSBTroll(%aiName)
{
		AI::setVar(%aiName, iq, 90 );
//		if($BotWeaponTroll == "true")
//	{
//		return;
//	}

	%id = AI::GetID(%AIName);
	
//	AI::setVar( "*", SpotDist, 9999);	

    if(!$Actions::Weapons[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	
	%aiId = BotFuncs::GetId(%aiName);
	if (%aiId==0)
	return;


 //   %animation = radnomItems(15, 25, 22, 24, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50);
	// BotFuncs::Animation(%aiId, %animation);
	
	if (($Spoonbot::MortarBusy[%aiId] == 1 ) || ($Spoonbot::MedicBusy[%aiId] == 1))
	{
//		messageall(1, "Weapon Switch. Mortar Busy OR Medic Busy.");
		return;
	}
	
//		if(floor(getrandom() * 100) < 50 )
//	{
//		BotFuncs::checkForPaint(%aiName);
//	}
	
//	$BotWeaponTroll  = true;
//	schedule("$BotWeaponTroll =false;",3.0);
	
				%player.cnt++;
	if(%player.cnt >= 100)
	{
		%curDie = radnomItems(4, $PlayerAnim::DieHead,$PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
		Player::setAnimation(%player, %curDie);
		playNextAnim(%player);
		// AI::onDroneKilled(%aiName);
		Player::kill(%player);
//		messageall(1, "A Troll Bot Respawned.");
		return;	
	}

//	if(%player.cnt >= 50)
//	{
//		if(floor(getrandom() * 100) < 20 )
//		{		
//		AnniBotsRoam(%aiName);
//		}
//	}
	
//	schedule("AnniSwitchWeaponSBTroll(" @ %ainame @ ");",5);
	
	AnniThrowGrenade(%aiName);
	
		 %client = Player::GetClient(%aiId);
		 %armor = player::getarmor(%client);
		 %armorlist = $ArmorName[%armor].description;

		// if((%armorlist == "Troll"))
		// {
		//  if((String::findSubStr(%aiName, "Guard") >= 0) || (String::findSubStr(%aiName, "Mortar") >= 0))
		// }

	

//	if(floor(getrandom() * 100) < 60 )
//	{
	ArenaBotBeaconTroll(%AIName);
//	}

		AnniDeploysForBots(%aiName);
		
		%curTarget = ai::getTarget( %aiName );
		%targLoc = GameBase::getPosition(Client::getOwnedObject(%curTarget));
		%aiLoc = GameBase::getPosition(Client::getOwnedObject(%id));
		%targetDist = Vector::getDistance(%aiLoc, %targLoc);
// 		 AI::setVar( "*", SpotDist, $visDistance);
		%placeA = ($visDistance - 50);
		%placeB = ($visDistance - 100);
		
		if(%targetDist > %placeA)
		{
			if(floor(getrandom() * 100) < 50 )
			{
				return;
			}
//			messageall(1, "Place A Laser Rifle.");
			AI::callWithId(%AIName, Player::mountItem, LaserRifle, 0);
			// AI::SetVar(%AIname, triggerPct, 1000 );
			return;	
		}
		
		if(%targetDist > %placeB)
		{
			if(floor(getrandom() * 100) < 50 )
			{
				return;
			}
			if(Player::isJetting(%curTarget))
			{	
//				messageall(1, "Place B Jetting Stinger.");
				AI::callWithId(%AIName, Player::mountItem, Stinger, 0);
				// AI::SetVar(%AIname, triggerPct, 1000 );
				return;
			}
			else
			{
//				messageall(1, "Place B Minigun.");
				AI::callWithId(%AIName, Player::mountItem, Minigun, 0);
				
				%switch = floor(getRandom() * 7);		
				if(%switch == 0)
				{
					AI::setVar( %AIname,  iq,  50 );
				}
				else if(%switch == 1)
				{
					AI::setVar( %AIname,  iq,  60 );
				}
				else if(%switch == 2)
				{
					AI::setVar( %AIname,  iq,  70 );
				}
				else if(%switch == 3)
				{
					AI::setVar( %AIname,  iq,  80 );
				}
				else if(%switch == 4)
				{
					AI::setVar( %AIname,  iq,  90 );
				}
				else if(%switch == 5)
				{
					AI::setVar( %AIname,  iq,  100 );
				}
				else if(%switch == 6)
				{
					AI::setVar( %AIname,  iq,  120 );
				}
				
//				AI::SetVar(%AIname, triggerPct, 100 );
				return;
			}
		}

		if(%targetDist < 100)
		{
//		messageall(1, "Less Than One Hundred Meters.");
		%switch = floor(getRandom() * 8);
		
		if(%switch == 0)
		{
	           AI::callWithId(%AIName, Player::mountItem, Mortar, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.28 );
		   return;
		}
		else if(%switch == 1)
		{
		    AI::callWithId(%AIname, Player::mountItem, PhaseDisrupter, 0);
//	 	    AI::SetVar(%AIname, triggerPct, 3.0 );
		   return;
		}
		else if(%switch == 2)
		{
	           AI::callWithId(%AIName, Player::mountItem, Minigun, 0);
	 	   // AI::SetVar(%AIname, triggerPct, 1000 );
		   return;
		}
		else if(%switch == 3)
		{
	           AI::callWithId(%AIName, Player::mountItem, RocketLauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
		   return;
		}
//		else if(%switch == 4)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Disclauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.10 );
//		   return;
//		}
//		else if(%switch == 5)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Flamer, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.01 );
//		   return;
//		}
//		else if(%switch == 6)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, FlameThrower, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.1 );
//		   return;
//		}
		else if(%switch == 4)
		{
	           AI::callWithId(%AIName, Player::mountItem, LaserRifle, 0);
	 	   // AI::SetVar(%AIname, triggerPct, 1000 );
		   return;
		}
		else if(%switch == 5)
		{
	           AI::callWithId(%AIName, Player::mountItem, GrenadeLauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
		   return;
		}
//		else if(%switch == 9)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, PlasmaGun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.1 );
//		   return;
//		}
		else if(%switch == 6)
		{
	           AI::callWithId(%AIName, Player::mountItem, RubberMortar, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.5 );
		   return;
		}
//		else if(%switch == 11)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Shotgun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.38 );
//		   return;
//		}
		else if(%switch == 7)
		{
	           AI::callWithId(%AIName, Player::mountItem, Stinger, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 1.5 );
		   return;
		}
		}
		else
		{
//		messageall(1, "Only Distance Weapons.");	
		%switch = floor(getRandom() * 5);
		
		if(%switch == 0)
		{
		    AI::callWithId(%AIname, Player::mountItem, PhaseDisrupter, 0);
//	 	    AI::SetVar(%AIname, triggerPct, 3.0 );
		   return;
		}
		else if(%switch == 1)
		{
	           AI::callWithId(%AIName, Player::mountItem, Minigun, 0);
	 	   // AI::SetVar(%AIname, triggerPct, 1000 );
		   return;
		}
		else if(%switch == 2)
		{
	           AI::callWithId(%AIName, Player::mountItem, RocketLauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
		   return;
		}
		else if(%switch == 3)
		{
	           AI::callWithId(%AIName, Player::mountItem, LaserRifle, 0);
	 	   // AI::SetVar(%AIname, triggerPct, 1000 );
		   return;
		}
		else if(%switch == 4)
		{
	           AI::callWithId(%AIName, Player::mountItem, Stinger, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 1.5 );
		   return;
		}
		}
		
}

function AnniDeploysTroll(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }

		   			if(floor(getrandom() * 100) < 25 ) // 10 2 4
			{
                		AI::DeployItem(%aiId, IrradiationTurretPack);
                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
//		messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Irradiation Turret deployed.");
			}	
}

function AnniDeploysTroll2(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	
		if(floor(getrandom() * 100) < 20 ) // 10 2 4 
			{
                		AI::DeployItem(%aiId, PlasmaTurretPack);
                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
//		messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Plasma Turret deployed.");
			}
			
}

function AnniDeploysTroll3(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }			
					   		if(floor(getrandom() * 100) < 10 ) // 10 2 5 4
			{
                		AI::DeployItem(%aiId, MotionSensorPack);
                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
//		messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Motion Sensor deployed.");
			}
}

function ai::AnniSwitchWeaponSBTank(%aiName)
{
			AI::setVar(%aiName, iq, 90 );
//			if($BotWeaponTank == "true")
//	{
//		return;
//	}
	%id = AI::GetID(%AIName);
	
//			 AI::setVar( "*", SpotDist, 9999);	

    if(!$Actions::Weapons[%id]) 
    {
        return;
    }

	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	
		%aiId = BotFuncs::GetId(%aiName);
	if (%aiId==0)
	return;

 //   %animation = radnomItems(15, 25, 22, 24, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50);
	// BotFuncs::Animation(%aiId, %animation);

//	messageall(1, "Tank Switch Weapons "@ %aiName @" .");

	if (($Spoonbot::MortarBusy[%aiId] == 1 ) || ($Spoonbot::MedicBusy[%aiId] == 1))
	{
//		messageall(1, "Weapon Switch. Mortar Busy OR Medic Busy.");
		return;
	}

//		if(floor(getrandom() * 100) < 5 )
//	{
//		BotFuncs::checkForPaint(%aiName);
//	}

//	$BotWeaponTank  = true;
//	schedule("$BotWeaponTank =false;",3.0);
	
				%player.cnt++;
	if(%player.cnt >= 100)
	{
		%curDie = radnomItems(4, $PlayerAnim::DieHead,$PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
		Player::setAnimation(%player, %curDie);
		playNextAnim(%player);
		Player::kill(%player);
//		 messageall(1, "A Tank Bot Respawned.");
		return;	
	}
	
//		if(%player.cnt >= 30)
//	{
//		if(floor(getrandom() * 100) < 20 )
//		{
//			messageall(1, "ROAMING NOW.");			
//			AnniBotsRoam(%aiName);
//		}
//	}
	
//		schedule("AnniSwitchWeaponSBTank(" @ %ainame @ ");",5);
		
//			AnniThrowGrenade(%aiName);
	
//		%dlevel = GameBase::getDamageLevel(%player);

//   %rk = Player::getItemCount(%client, RepairKit);
 //  if   ((%rk > 0) && (%dlevel > 0.2))
 //  {
  //   if (GameBase::getAutoRepairRate(%player) == 0)
  //   {
  //     Player::decItemCount(%client,repairkit);
  //     GameBase::setAutoRepairRate(%player,0.15);
  //   }
  // }
  // else if (%dlevel == 0)
  // {
  //   GameBase::setAutoRepairRate(%player,0);
  // }
	
//	if(floor(getrandom() * 100) < 60 )
//	{
		ArenaBotBeaconTank(%AIName);
//	}

		AnniDeploysForBots(%aiName);
		
		%curTarget = ai::getTarget( %aiName );
		%targLoc = GameBase::getPosition(Client::getOwnedObject(%curTarget));
		%aiLoc = GameBase::getPosition(Client::getOwnedObject(%id));
		%targetDist = Vector::getDistance(%aiLoc, %targLoc);
		
		%placeA = ($visDistance - 50);
		%placeB = ($visDistance - 100);
		
		if(%targetDist > %placeA)
		{
						if(floor(getrandom() * 100) < 50 )
			{
				return;
			}
			AI::callWithId(%AIName, Player::mountItem, TankShredder, 0);
							%switch = floor(getRandom() * 7);		
				if(%switch == 0)
				{
					AI::setVar( %AIname,  iq,  50 );
				}
				else if(%switch == 1)
				{
					AI::setVar( %AIname,  iq,  60 );
				}
				else if(%switch == 2)
				{
					AI::setVar( %AIname,  iq,  70 );
				}
				else if(%switch == 3)
				{
					AI::setVar( %AIname,  iq,  80 );
				}
				else if(%switch == 4)
				{
					AI::setVar( %AIname,  iq,  90 );
				}
				else if(%switch == 5)
				{
					AI::setVar( %AIname,  iq,  100 );
				}
				else if(%switch == 6)
				{
					AI::setVar( %AIname,  iq,  120 );
				}
			// AI::SetVar(%AIname, triggerPct, 1000 );
			return;
		}
		
		if(%targetDist > %placeB)
		{
						if(floor(getrandom() * 100) < 50 )
			{
				return;
			}
			if(Player::isJetting(%curTarget))
			{	
				AI::callWithId(%AIName, Player::mountItem, TRocketLauncher, 0);
				// AI::SetVar(%AIname, triggerPct, 1000 );
				return;
			}
			else
			{
				AI::callWithId(%AIName, Player::mountItem, TankShredder, 0);
				AI::setVar( %AIname,  iq,  50 );
//				AI::SetVar(%AIname, triggerPct, 100 );
				return;
			}
		}

		if(%targetDist > 100)
		{
			    %switch = floor(getRandom() * 3);
			if(%switch == 0)
			{
				AI::callWithId(%AIname, Player::mountItem, TRocketLauncher, 0);
				// // AI::SetVar(%AIname, triggerPct, 1000 );
				AI::SetVar(%AIname, triggerPct, 1.0 );
				return;
			}
			else if(%switch == 1)
			{
	            AI::callWithId(%AIName, Player::mountItem, TankRPGLauncher, 0); 
				//  // AI::SetVar(%AIname, triggerPct, 1000 );
//				AI::SetVar(%AIname, triggerPct, 2.0 );
				return;
			}
			else if(%switch == 2)
			{
				AI::callWithId(%AIName, Player::mountItem, TankShredder, 0);
				// AI::SetVar(%AIname, triggerPct, 1000 );
				return;
			}
		}
   else
   {
	    %switch = floor(getRandom() * 4);

		if(%switch == 0)
		{
	           AI::callWithId(%AIName, Player::mountItem, TBlastCannon, 0); // TBlastCannon
			  // // AI::SetVar(%AIname, triggerPct, 1000 );
				AI::SetVar(%AIname, triggerPct, 0.5 );
		   return;
		}
		else if(%switch == 1)
		{
		    AI::callWithId(%AIname, Player::mountItem, TRocketLauncher, 0); // TRocketLauncher
			// // AI::SetVar(%AIname, triggerPct, 1000 );
	 	    AI::SetVar(%AIname, triggerPct, 1.0 );
		   return;
		}
		else if(%switch == 2)
		{
	           AI::callWithId(%AIName, Player::mountItem, TankRPGLauncher, 0); // TankRPGLauncher
			 //  // AI::SetVar(%AIname, triggerPct, 1000 );
//	 	    AI::SetVar(%AIname, triggerPct, 2.0 );
		   return;
		}
		else if(%switch == 3)
		{
	           AI::callWithId(%AIName, Player::mountItem, TankShredder, 0);
			   // AI::SetVar(%AIname, triggerPct, 1000 );
		   return;
		}
   }
}

function AnniDeploysTank(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
		   		if(floor(getrandom() * 100) < 35 ) // 15 3 7 5 
			{
                		AI::DeployItem(%aiId, DeployableTurret);
                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
//		messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Ion Turret deployed.");
			}
}

function AnniDeploysTank2(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }	
			if(floor(getrandom() * 100) < 15 ) // 10 2 5 3
			{
                		AI::DeployItem(%aiId, CameraPack);
                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
//		messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Camera deployed.");
			}
}

function ai::AnniSwitchWeaponSBTitan(%aiName)
{
			AI::setVar(%aiName, iq, 90 );
//			if($BotWeaponTitan == "true")
//	{
//		return;
//	}
	%id = AI::GetID(%AIName);

    if(!$Actions::Weapons[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	
			%aiId = BotFuncs::GetId(%aiName);
	if (%aiId==0)
	return;

//    %animation = radnomItems(15, 25, 22, 24, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50);
	// BotFuncs::Animation(%aiId, %animation);
	
	if (($Spoonbot::MortarBusy[%aiId] == 1 ) || ($Spoonbot::MedicBusy[%aiId] == 1))
	{
//		messageall(1, "Weapon Switch. Mortar Busy OR Medic Busy.");
		return;
	}
//		if(floor(getrandom() * 100) < 5 )
//	{
//		BotFuncs::checkForPaint(%aiName);
//	}
//		$BotWeaponTitan  = true;
//	schedule("$BotWeaponTitan =false;",3.0);
	
				%player.cnt++;
	if(%player.cnt >= 100)
	{
		%curDie = radnomItems(4, $PlayerAnim::DieHead,$PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
		Player::setAnimation(%player, %curDie);
		playNextAnim(%player);
		// AI::onDroneKilled(%aiName);
		Player::kill(%player);
//		messageall(1, "A Titan Bot Respawned.");
		return;	
	}
	
//		schedule("AnniSwitchWeaponSBTitan(" @ %ainame @ ");",5);
		
			AnniThrowGrenade(%aiName);
			
//	if(floor(getrandom() * 100) < 20 )
//	{
	ArenaBotBeaconTitan(%AIName);
//	}

		AnniDeploysForBots(%aiName);
		
		%curTarget = ai::getTarget( %aiName );
		%targLoc = GameBase::getPosition(Client::getOwnedObject(%curTarget));
		%aiLoc = GameBase::getPosition(Client::getOwnedObject(%id));
		%targetDist = Vector::getDistance(%aiLoc, %targLoc);
		
		%placeA = ($visDistance - 50);
		%placeB = ($visDistance - 100);
		
		if(%targetDist > %placeA)
		{
						if(floor(getrandom() * 100) < 50 )
			{
				return;
			}
			AI::callWithId(%AIName, Player::mountItem, ParticleBeamWeapon, 0);
			// AI::SetVar(%AIname, triggerPct, 1000 );
			return;
		}
		
		if(%targetDist > %placeB)
		{
						if(floor(getrandom() * 100) < 50 )
			{
				return;
			}
			if(Player::isJetting(%curTarget))
			{	
				AI::callWithId(%AIName, Player::mountItem, Stinger, 0);
				// AI::SetVar(%AIname, triggerPct, 1000 );
				return;
			}
			else
			{
				AI::callWithId(%AIName, Player::mountItem, ParticleBeamWeapon, 0);
				%switch = floor(getRandom() * 7);		
				if(%switch == 0)
				{
					AI::setVar( %AIname,  iq,  50 );
				}
				else if(%switch == 1)
				{
					AI::setVar( %AIname,  iq,  60 );
				}
				else if(%switch == 2)
				{
					AI::setVar( %AIname,  iq,  70 );
				}
				else if(%switch == 3)
				{
					AI::setVar( %AIname,  iq,  80 );
				}
				else if(%switch == 4)
				{
					AI::setVar( %AIname,  iq,  90 );
				}
				else if(%switch == 5)
				{
					AI::setVar( %AIname,  iq,  100 );
				}
				else if(%switch == 6)
				{
					AI::setVar( %AIname,  iq,  120 );
				}
//				AI::SetVar(%AIname, triggerPct, 100 );
				return;
			}
		}

		if(%targetDist > 100)
		{
			%switch = floor(getRandom() * 4);
			if(%switch == 0)
			{
				AI::callWithId(%AIName, Player::mountItem, ParticleBeamWeapon, 0);
				// AI::SetVar(%AIname, triggerPct, 0.5 );
				// AI::SetVar(%AIname, triggerPct, 1000 );
				return;
			}
			else if(%switch == 1)
			{
				AI::callWithId(%AIname, Player::mountItem, PhaseDisrupter, 0);
//				AI::SetVar(%AIname, triggerPct, 3.0 );
				return;
			}
			else if(%switch == 2)
			{
				AI::callWithId(%AIName, Player::mountItem, RocketLauncher, 0);
//				AI::SetVar(%AIname, triggerPct, 0.8 );
				return;
			}
			else if(%switch == 3)
			{
				AI::callWithId(%AIName, Player::mountItem, Stinger, 0);
//				AI::SetVar(%AIname, triggerPct, 1.5 );
				return;
			}
		}
		else
		{
	    %switch = floor(getRandom() * 5);

		if(%switch == 0)
		{
	           AI::callWithId(%AIName, Player::mountItem, ParticleBeamWeapon, 0);
//	 	    AI::SetVar(%AIname, triggerPct, 0.5 );
		   return;
		}
		else if(%switch == 1)
		{
		    AI::callWithId(%AIname, Player::mountItem, PhaseDisrupter, 0);
//	 	    AI::SetVar(%AIname, triggerPct, 3.0 );
		   return;
		}
//		else if(%switch == 2)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, BabyNukeMortar, 0);
//	 	    AI::SetVar(%AIname, triggerPct, 2.0 );
//		   return;
//		}
//		else if(%switch == 3)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, OSLauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.5 );
//		   			messageall(1, "OSLauncher.");
//		   return;
//		}
		else if(%switch == 2)
		{
	           AI::callWithId(%AIName, Player::mountItem, RocketLauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
		   return;
		}
//		else if(%switch == 3)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Disclauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.10 );
//		   return;
//		}
//		else if(%switch == 4)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Flamer, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.01 );
//		   return;
//		}
//		else if(%switch == 5)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, FlameThrower, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.1 );
//		   return;
//		}
		else if(%switch == 3)
		{
	           AI::callWithId(%AIName, Player::mountItem, GrenadeLauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
		   return;
		}
//		else if(%switch == 7)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, PlasmaGun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.1 );
//		   return;
//		}
//		else if(%switch == 8)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, RubberMortar, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.5 );
//		   return;
//		}
		else if(%switch == 4)
		{
	           AI::callWithId(%AIName, Player::mountItem, Stinger, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 1.5 );
		   return;
		}
//		else if(%switch == 10)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Thumper, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
//		   return;
//		}
		}
}

function AnniDeploysTitan(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }

		if(floor(getrandom() * 100) < 15 ) // 10 2 4 3
			{
                		AI::DeployItem(%aiId, ShockTurretPack);
                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
//		messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Shock Turret deployed.");
			}
}

function AnniDeploysTitan2(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }	
				if(floor(getrandom() * 100) < 10 ) // 10 2 4 3
			{
                		AI::DeployItem(%aiId, PulseSensorPack);
                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
//		messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Pulse Sensor deployed.");
			}

}

function AnniDeploysTitan3(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
			
					if(floor(getrandom() * 100) < 65 ) // 25 5 10 7 
			{
                		AI::DeployItem(%aiId, ForceFieldDoorPack);
                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
//		messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Force Field Door deployed.");
			}

}

function ai::AnniSwitchWeaponSBBuilder(%aiName)
{
			AI::setVar(%aiName, iq, 90 );
//			if($BotWeaponBuilder == "true")
//	{
//		return;
//	}
	%id = AI::GetID(%AIName);
	
//			 AI::setVar( "*", SpotDist, 9999);	

    if(!$Actions::Weapons[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	
			%aiId = BotFuncs::GetId(%aiName);
	if (%aiId==0)
	return;

//    %animation = radnomItems(15, 25, 22, 24, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50);
	// BotFuncs::Animation(%aiId, %animation);
	
	if (($Spoonbot::MortarBusy[%aiId] == 1 ) || ($Spoonbot::MedicBusy[%aiId] == 1))
	{
//		messageall(1, "Weapon Switch. Mortar Busy OR Medic Busy.");
		return;
	}
//		if(floor(getrandom() * 100) < 5 )
//	{
//		BotFuncs::checkForPaint(%aiName);
//	}
//		$BotWeaponBuilder  = true;
//	schedule("$BotWeaponBuilder =false;",3.0);
	
				%player.cnt++;
	if(%player.cnt >= 100)
	{
		%curDie = radnomItems(4, $PlayerAnim::DieHead,$PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
		Player::setAnimation(%player, %curDie);
		playNextAnim(%player);
		// AI::onDroneKilled(%aiName);
		Player::kill(%player);
//		 messageall(1, "A Builer Respawned.");
		return;	
	}
	
//		if(%player.cnt >= 30)
//	{
//		if(floor(getrandom() * 100) < 20 )
//		{		
//		AnniBotsRoam(%aiName);
//		}
//	}
	
//		schedule("AnniSwitchWeaponSBBuilder(" @ %ainame @ ");",5);
		
			AnniThrowGrenade(%aiName);
			
			AnniDeploysForBots(%aiName);

		%curTarget = ai::getTarget( %aiName );
		%targLoc = GameBase::getPosition(Client::getOwnedObject(%curTarget));
		%aiLoc = GameBase::getPosition(Client::getOwnedObject(%id));
		%targetDist = Vector::getDistance(%aiLoc, %targLoc);
		
		%placeA = ($visDistance - 50);
		%placeB = ($visDistance - 100);
		
		if(%targetDist > %placeA)
		{
						if(floor(getrandom() * 100) < 50 )
			{
				return;
			}
			AI::callWithId(%AIName, Player::mountItem, Railgun, 0);
			// AI::SetVar(%AIname, triggerPct, 1000 );
			return;
		}
		
		if(%targetDist > %placeB)
		{
						if(floor(getrandom() * 100) < 50 )
			{
				return;
			}
			if(Player::isJetting(%curTarget))
			{	
				AI::callWithId(%AIName, Player::mountItem, Stinger, 0);
				// AI::SetVar(%AIname, triggerPct, 1000 );
				return;
			}
			else
			{
				AI::callWithId(%AIName, Player::mountItem, Railgun, 0);
				%switch = floor(getRandom() * 7);		
				if(%switch == 0)
				{
					AI::setVar( %AIname,  iq,  50 );
				}
				else if(%switch == 1)
				{
					AI::setVar( %AIname,  iq,  60 );
				}
				else if(%switch == 2)
				{
					AI::setVar( %AIname,  iq,  70 );
				}
				else if(%switch == 3)
				{
					AI::setVar( %AIname,  iq,  80 );
				}
				else if(%switch == 4)
				{
					AI::setVar( %AIname,  iq,  90 );
				}
				else if(%switch == 5)
				{
					AI::setVar( %AIname,  iq,  100 );
				}
				else if(%switch == 6)
				{
					AI::setVar( %AIname,  iq,  120 );
				}
				// AI::SetVar(%AIname, triggerPct, 1000 );
				return;
			}
		}

		if(%targetDist > 100)
		{
			%switch = floor(getRandom() * 3);
			if(%switch == 0)
			{
				AI::callWithId(%AIName, Player::mountItem, Railgun, 0);
				// AI::SetVar(%AIname, triggerPct, 1.0 );
				// AI::SetVar(%AIname, triggerPct, 1000 );
								%switch = floor(getRandom() * 7);		
				if(%switch == 0)
				{
					AI::setVar( %AIname,  iq,  50 );
				}
				else if(%switch == 1)
				{
					AI::setVar( %AIname,  iq,  60 );
				}
				else if(%switch == 2)
				{
					AI::setVar( %AIname,  iq,  70 );
				}
				else if(%switch == 3)
				{
					AI::setVar( %AIname,  iq,  80 );
				}
				else if(%switch == 4)
				{
					AI::setVar( %AIname,  iq,  90 );
				}
				else if(%switch == 5)
				{
					AI::setVar( %AIname,  iq,  100 );
				}
				else if(%switch == 6)
				{
					AI::setVar( %AIname,  iq,  120 );
				}
				return;
			}
			else if(%switch == 1)
			{
				AI::callWithId(%AIName, Player::mountItem, RocketLauncher, 0);
				// AI::SetVar(%AIname, triggerPct, 0.8 );
				// AI::SetVar(%AIname, triggerPct, 1000 );
				return;
			}
			else if(%switch == 2)
			{
				AI::callWithId(%AIName, Player::mountItem, Stinger, 0);
				// AI::SetVar(%AIname, triggerPct, 1000 );
				return;
			}			
		}
		else
		{
	    %switch = floor(getRandom() * 5);

		if(%switch == 0)
		{
	           AI::callWithId(%AIName, Player::mountItem, Railgun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 1.0 );
		   return;
		}
		else if(%switch == 1)
		{
	           AI::callWithId(%AIName, Player::mountItem, RocketLauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
		   return;
		}
//		else if(%switch == 2)
//		{
//		    AI::callWithId(%AIname, Player::mountItem, Pitchfork, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.01 );
//		   return;
//		}
//		else if(%switch == 2)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Flamer, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.01 );
//		   return;
//		}
		else if(%switch == 2)
		{
	           AI::callWithId(%AIName, Player::mountItem, GrenadeLauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
		   return;
		}
		else if(%switch == 3)
		{
	           AI::callWithId(%AIName, Player::mountItem, PlasmaGun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.1 );
		   return;
		}
//		else if(%switch == 5)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Shotgun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.38 );
//		   return;
//		}
//		else if(%switch == 6)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Stinger, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 1.5 );
//		   return;
//		}
//		else if(%switch == 8)
//		{
//		    AI::callWithId(%AIname, Player::mountItem, Vulcan, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.0 );
//		   		   			messageall(1, "Vulcan.");
//		   return;
//		}
		else if(%switch == 4)
		{
	           AI::callWithId(%AIName, Player::mountItem, Thumper, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
		   return;
		}
//		else if(%switch == 8)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Disclauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.10 );
//		   return;
//		}

		}
}

function AnniDeploysBuilder(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }

			if(floor(getrandom() * 100) < 15 ) // 5 1 4 2
			{
                		AI::DeployItem(%aiId, DeployableInvPack);
                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
			//	messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Inventory deployed.");
			}

}

function AnniDeploysBuilder2(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
				
					if(floor(getrandom() * 100) < 75 ) 
			{
						Item::setVelocity(%aiId, "0 0 0");
                		AI::DeployItem(%aiId, LaserTurretPack);										
			}

}

function AnniDeploysBuilder3(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }

			   		if(floor(getrandom() * 100) < 50 ) // 20 4 7 6 
			{
                		AI::DeployItem(%aiId, vortexTurretPack);
                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
			//	messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Vortex Turret deployed.");
			}

}

function AnniDeploysBuilder4(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
					   		if(floor(getrandom() * 100) < 30 ) // 10 2 6 4
			{
                		AI::DeployItem(%aiId, NuclearTurretPack);
                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
			//	messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Nuclear Turret deployed.");
			}

}

function AnniDeploysBuilder5(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
					   		if(floor(getrandom() * 100) < 40 ) // 20 4 7 6
			{
                		AI::DeployItem(%aiId, FlameTurretPack);
                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
			//	messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Flame Turret deployed.");
			}

}

function AnniDeploysBuilder6(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	
	Item::setVelocity(%aiId, "0 0 0");
			
//					if(floor(getrandom() * 100) < 75 ) 
//			{
                		AI::DeployItem(%aiId, LaserAndCrates);
//                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
//			}

}

function AnniDeploysBuilder7(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	
	Item::setVelocity(%aiId, "0 0 0");
			
//					if(floor(getrandom() * 100) < 75 ) 
//			{
                		AI::DeployItem(%aiId, LaserAndPlatforms);
//                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
//			}

}

function AnniDeploysBuilder8(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }

//			   		if(floor(getrandom() * 100) < 50 ) // 20 4 7 6 
//			{
                		AI::DeployItem(%aiId, vortexTurretCovered);
                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
			//	messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Vortex Turret deployed.");
//			}

}

function AnniDeploysBuilder9(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }

//			   		if(floor(getrandom() * 100) < 50 ) // 20 4 7 6 
//			{
                		AI::DeployItem(%aiId, FlameTurretCovered);
                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
			//	messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Vortex Turret deployed.");
//			}

}

function BotMove::ThreePlatforms(%aiId)
{

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	Item::setVelocity(%aiId, "0 0 0");
	AI::DeployItem(%aiId, PlatformPack);
	AI::DeployItem(%aiId, PlatformPack2);
}

function BotMove::TwoBigCrates(%aiId)
{
    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	Item::setVelocity(%aiId, "0 0 0");
	AI::DeployItem(%aiId, BigCratePack);
	AI::DeployItem(%aiId, BigCratePack);	
}

function ai::AnniSwitchWeaponSBNecro(%aiName)
{
			AI::setVar(%aiName, iq, 90 );
//			if($BotWeaponNecro == "true")
//	{
//		return;
//	}
	%id = AI::GetID(%AIName);
	
//			 AI::setVar( "*", SpotDist, 9999);	

    if(!$Actions::Weapons[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	
			%aiId = BotFuncs::GetId(%aiName);
	if (%aiId==0)
	return;

//    %animation = radnomItems(15, 25, 22, 24, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50);
	// BotFuncs::Animation(%aiId, %animation);
	
	if (($Spoonbot::MortarBusy[%aiId] == 1 ) || ($Spoonbot::MedicBusy[%aiId] == 1))
	{
//		messageall(1, "Weapon Switch. Mortar Busy OR Medic Busy.");
		return;
	}
//		if(floor(getrandom() * 100) < 5 )
//	{
//		BotFuncs::checkForPaint(%aiName);
//	}
		
//	$BotWeaponNecro  = true;
//	schedule("$BotWeaponNecro =false;",3.0);
	
				%player.cnt++;
	if(%player.cnt >= 100)
	{
		%curDie = radnomItems(4, $PlayerAnim::DieHead,$PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
		Player::setAnimation(%player, %curDie);
		playNextAnim(%player);
		// AI::onDroneKilled(%aiName);
		Player::kill(%player);
//		 messageall(1, "A Necro Bot Respawned.");
		return;	
	}
	
//		if(%player.cnt >= 30)
//	{
//		if(floor(getrandom() * 100) < 20 )
//		{		
//		AnniBotsRoam(%aiName);
//		}
//	}
	
//		schedule("AnniSwitchWeaponSBNecro(" @ %ainame @ ");",5);
		
			AnniThrowGrenade(%aiName);
			

				AnniDeploysForBots(%aiName);
	

	
	if(floor(getrandom() * 100) < 10 )
	{
		ArenaBotBeaconNecromancer(%AIName);
	}

		%curTarget = ai::getTarget( %aiName );
		%targLoc = GameBase::getPosition(Client::getOwnedObject(%curTarget));
		%aiLoc = GameBase::getPosition(Client::getOwnedObject(%id));
		%targetDist = Vector::getDistance(%aiLoc, %targLoc);
		
		%placeA = ($visDistance - 50);
		%placeB = ($visDistance - 100);
		
		if(%targetDist > %placeA)
		{
						if(floor(getrandom() * 100) < 50 )
			{
				return;
			}
	        AI::callWithId(%AIName, Player::mountItem, DeathRay, 0);
			// AI::SetVar(%AIname, triggerPct, 1000 );
			return;
		}
		
		if(%targetDist > %placeB)
		{
						if(floor(getrandom() * 100) < 50 )
			{
				return;
			}
//			if(Player::isJetting(%curTarget))
//			{	
//				AI::callWithId(%AIName, Player::mountItem, Stinger, 0);
//				// AI::SetVar(%AIname, triggerPct, 1000 );
//				return;
//			}
//			else
//			{
				AI::callWithId(%AIName, Player::mountItem, DeathRay, 0);
				%switch = floor(getRandom() * 7);		
				if(%switch == 0)
				{
					AI::setVar( %AIname,  iq,  50 );
				}
				else if(%switch == 1)
				{
					AI::setVar( %AIname,  iq,  60 );
				}
				else if(%switch == 2)
				{
					AI::setVar( %AIname,  iq,  70 );
				}
				else if(%switch == 3)
				{
					AI::setVar( %AIname,  iq,  80 );
				}
				else if(%switch == 4)
				{
					AI::setVar( %AIname,  iq,  90 );
				}
				else if(%switch == 5)
				{
					AI::setVar( %AIname,  iq,  100 );
				}
				else if(%switch == 6)
				{
					AI::setVar( %AIname,  iq,  120 );
				}
				// AI::SetVar(%AIname, triggerPct, 1000 );
				return;
//			}
		}

		if(%targetDist > 100)
		{
			%switch = floor(getRandom() * 3);
			if(%switch == 0)
			{
				AI::callWithId(%AIName, Player::mountItem, DisarmerSpell, 0);
//				AI::SetVar(%AIname, triggerPct, 0.5 );
				return;
			}
			else if(%switch == 1)
			{
	           AI::callWithId(%AIName, Player::mountItem, DeathRay, 0);
				// AI::SetVar(%AIname, triggerPct, 1000 );
				return;
			}
			else if(%switch == 2)
			{
				AI::callWithId(%AIName, Player::mountItem, FlameStrike, 0);
//	 	        AI::SetVar(%AIname, triggerPct, 0.1 );
				// AI::SetVar(%AIname, triggerPct, 1000 );
				return;
			}
		}
		else
		{			
	    %switch = floor(getRandom() * 6);

		if(%switch == 0)
		{
	           AI::callWithId(%AIName, Player::mountItem, DisarmerSpell, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.5 );
		   return;
		}
		else if(%switch == 1)
		{
		    AI::callWithId(%AIname, Player::mountItem, ShockingGrasp, 0);
	 	   // AI::SetVar(%AIname, triggerPct, 1000 );
		   return;
		}
		else if(%switch == 2)
		{
	           AI::callWithId(%AIName, Player::mountItem, SpellFlameThrower, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.1 );
		   return;
		}
		else if(%switch == 3)
		{
	           AI::callWithId(%AIName, Player::mountItem, DeathRay, 0);
	 	   // AI::SetVar(%AIname, triggerPct, 1000 );
		   return;
		}
		else if(%switch == 4)
		{
	           AI::callWithId(%AIName, Player::mountItem, FlameStrike, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.1 );
		   return;
		}
		else if(%switch == 5)
		{
	           AI::callWithId(%AIName, Player::mountItem, Stasis, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 1.0 );
		   return;
		}
		}
}

function AnniDeploysNecro(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }

		   		if(floor(getrandom() * 100) < 10 ) // 5 1 5 3
			{
                		AI::DeployItem(%aiId, JumpPadPack);
                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
		//		messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Jump Pad deployed.");
			}

}

function ai::AnniSwitchWeaponSBSpy(%aiName)
{
			AI::setVar(%aiName, iq, 90 );
//			if($BotWeaponSpy == "true")
//	{
//		return;
//	}
	%id = AI::GetID(%aiName);
	
//			 AI::setVar( "*", SpotDist, 9999);	

    if(!$Actions::Weapons[%id]) 
    {
        return;
    }

	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	
			%aiId = BotFuncs::GetId(%aiName);
	if (%aiId==0)
	return;

//    %animation = radnomItems(15, 25, 22, 24, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50);
	// BotFuncs::Animation(%aiId, %animation);
	
	if (($Spoonbot::MortarBusy[%aiId] == 1 ) || ($Spoonbot::MedicBusy[%aiId] == 1))
	{
//		messageall(1, "Weapon Switch. Mortar Busy OR Medic Busy.");
		return;
	}
//		if(floor(getrandom() * 100) < 5 )
//	{
//		BotFuncs::checkForPaint(%aiName);
//	}
//	$BotWeaponSpy  = true;
//	schedule("$BotWeaponSpy =false;",3.0);
	
				%player.cnt++;
	if(%player.cnt >= 100)
	{
		%curDie = radnomItems(4, $PlayerAnim::DieHead,$PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
		Player::setAnimation(%player, %curDie);
		playNextAnim(%player);
		// AI::onDroneKilled(%aiName);
		Player::kill(%player);
//		messageall(1, "A Spy Bot Respawned.");
		return;	
	}
	
//		if(%player.cnt >= 30)
//	{
//		if(floor(getrandom() * 100) < 20 )
//		{		
//		AnniBotsRoam(%aiName);
//		}
//	}
	
//		schedule("AnniSwitchWeaponSBSpy(" @ %ainame @ ");",5);
		
			AnniThrowGrenade(%aiName);

	if(floor(getrandom() * 100) < 10 )
	{
		ArenaBotBeaconChameleon(%aiName);
	}
	
		AnniDeploysForBots(%aiName);
		
		%curTarget = ai::getTarget( %aiName );
		%targLoc = GameBase::getPosition(Client::getOwnedObject(%curTarget));
		%aiLoc = GameBase::getPosition(Client::getOwnedObject(%id));
		%targetDist = Vector::getDistance(%aiLoc, %targLoc);
		
		%placeA = ($visDistance - 50);
		%placeB = ($visDistance - 100);
		
		if(%targetDist > %placeA)
		{
						if(floor(getrandom() * 100) < 50 )
			{
				return;
			}
			AI::callWithId(%AIName, Player::mountItem, SniperRifle, 0);
			// AI::SetVar(%AIname, triggerPct, 1000 );
			return;
		}
		
		if(%targetDist > %placeB)
		{
						if(floor(getrandom() * 100) < 50 )
			{
				return;
			}
			if(Player::isJetting(%curTarget))
			{	
				AI::callWithId(%AIName, Player::mountItem, LaserRifle, 0);
				%switch = floor(getRandom() * 7);		
				if(%switch == 0)
				{
					AI::setVar( %AIname,  iq,  50 );
				}
				else if(%switch == 1)
				{
					AI::setVar( %AIname,  iq,  60 );
				}
				else if(%switch == 2)
				{
					AI::setVar( %AIname,  iq,  70 );
				}
				else if(%switch == 3)
				{
					AI::setVar( %AIname,  iq,  80 );
				}
				else if(%switch == 4)
				{
					AI::setVar( %AIname,  iq,  90 );
				}
				else if(%switch == 5)
				{
					AI::setVar( %AIname,  iq,  100 );
				}
				else if(%switch == 6)
				{
					AI::setVar( %AIname,  iq,  120 );
				}
				// AI::SetVar(%AIname, triggerPct, 1000 );
				return;
			}
			else
			{
				AI::callWithId(%AIName, Player::mountItem, SniperRifle, 0);
				%switch = floor(getRandom() * 7);		
				if(%switch == 0)
				{
					AI::setVar( %AIname,  iq,  50 );
				}
				else if(%switch == 1)
				{
					AI::setVar( %AIname,  iq,  60 );
				}
				else if(%switch == 2)
				{
					AI::setVar( %AIname,  iq,  70 );
				}
				else if(%switch == 3)
				{
					AI::setVar( %AIname,  iq,  80 );
				}
				else if(%switch == 4)
				{
					AI::setVar( %AIname,  iq,  90 );
				}
				else if(%switch == 5)
				{
					AI::setVar( %AIname,  iq,  100 );
				}
				else if(%switch == 6)
				{
					AI::setVar( %AIname,  iq,  120 );
				}
				// AI::SetVar(%AIname, triggerPct, 1000 );
				return;
			}
		}

		if(%targetDist > 100)
		{
			    %switch = floor(getRandom() * 2);
				if(%switch == 0)
				{
					AI::callWithId(%AIName, Player::mountItem, SniperRifle, 0);
//					AI::SetVar(%AIname, triggerPct, 1.0 );
					// AI::SetVar(%AIname, triggerPct, 1000 );
					return;
				}
				else if(%switch == 1)
				{
					AI::callWithId(%AIName, Player::mountItem, LaserRifle, 0);
					// AI::SetVar(%AIname, triggerPct, 1000 );
					return;
				}				
		}
		else
		{
	    %switch = floor(getRandom() * 5);

		if(%switch == 0)
		{
	           AI::callWithId(%AIName, Player::mountItem, SniperRifle, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 1.0 );
		   return;
		}
//		else if(%switch == 1)
//		{
//		    AI::callWithId(%AIname, Player::mountItem, PlasmaGun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.1 );
//		   return;
//		}
		else if(%switch == 1)
		{
	           AI::callWithId(%AIName, Player::mountItem, Shotgun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.38 );
		   return;
		}
//		else if(%switch == 3)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Flamer, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.01 );
//		   return;
//		}
		else if(%switch == 2)
		{
	           AI::callWithId(%AIName, Player::mountItem, GrenadeLauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
		   return;
		}
		else if(%switch == 3)
		{
	           AI::callWithId(%AIName, Player::mountItem, Hammer, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.1 );
		   return;
		}
//		else if(%switch == 6)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, ShockwaveGun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.3 );
//		   		   			messageall(1, "Shockwaveun.");
//		   return;
//		}
		else if(%switch == 4)
		{
	           AI::callWithId(%AIName, Player::mountItem, LaserRifle, 0);
	 	   // AI::SetVar(%AIname, triggerPct, 1000 );
		   return;
		}
//		else if(%switch == 7)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Thumper, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
//		   return;
//		}
//		else if(%switch == 8)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Disclauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.10 );
//		   return;
//		}
		}
}

function AnniDeploysSpy(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	
		   		if(floor(getrandom() * 100) < 40 ) // 10 2 4
			{
                		AI::DeployItem(%aiId, RocketPack); 
                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
//		messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Rocket Turret deployed.");
			}
}

function AnniDeploysSpy2(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }		
					   		if(floor(getrandom() * 100) < 10 ) // 10 2 5 3
			{
                		AI::DeployItem(%aiId, DeployableSensorJammerPack);
                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
			//	messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Sensor Jammer deployed.");
			}

}

function ai::AnniSwitchWeaponSBWarrior(%aiName)
{
			AI::setVar(%aiName, iq, 90 );
//			if($BotWeaponWarrior == "true")
//	{
//		return;
//	}
	%id = AI::GetID(%AIName);
	
//			 AI::setVar( "*", SpotDist, 9999);	

    if(!$Actions::Weapons[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

//	messageall(1, "Anni Switch Weapon Warrior.");

    if(%loc == "0 0 0")
    {
	return;
    }
	
			%aiId = BotFuncs::GetId(%aiName);
	if (%aiId==0)
	return;

//    %animation = radnomItems(15, 25, 22, 24, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50);
	// BotFuncs::Animation(%aiId, %animation);
	
	if (($Spoonbot::MortarBusy[%aiId] == 1 ) || ($Spoonbot::MedicBusy[%aiId] == 1))
	{
//		messageall(1, "Weapon Switch. Mortar Busy OR Medic Busy.");
		return;
	}
//		if(floor(getrandom() * 100) < 5 )
//	{
//		BotFuncs::checkForPaint(%aiName);
//	}
	
	 // AnniBotsRoam(%aiName);
	
//		$BotWeaponWarrior  = true;
//	schedule("$BotWeaponWarrior =false;",3.0);
	// messageall(1, "Anni Switch Weapon Warrior.");
	
				%player.cnt++;
	if(%player.cnt >= 100)
	{
		%curDie = radnomItems(4, $PlayerAnim::DieHead,$PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
		Player::setAnimation(%player, %curDie);
		playNextAnim(%player);
		// AI::onDroneKilled(%aiName);
		Player::kill(%player);
//		messageall(1, "A Warrior Bot Respawned.");
		return;	
	}
	
//		if(%player.cnt >= 30)
//	{
//		if(floor(getrandom() * 100) < 20 )
//		{		
//		AnniBotsRoam(%aiName);
//		}
//	}
	
//		schedule("AnniSwitchWeaponSBWarrior(" @ %ainame @ ");",5);
		
			AnniThrowGrenade(%aiName);
			
	Player::trigger(%aiName,$BackpackSlot);

//	if(floor(getrandom() * 100) < 20 )
//	{
	ArenaBotBeaconWarrior(%AIName);
//	}
	
			AnniDeploysForBots(%aiName);
			
		%curTarget = ai::getTarget( %aiName );
		%targLoc = GameBase::getPosition(Client::getOwnedObject(%curTarget));
		%aiLoc = GameBase::getPosition(Client::getOwnedObject(%id));
		%targetDist = Vector::getDistance(%aiLoc, %targLoc);
		
		%placeA = ($visDistance - 50);
		%placeB = ($visDistance - 100);
		
		if(%targetDist > %placeA)
		{
						if(floor(getrandom() * 100) < 50 )
			{
				return;
			}
			AI::callWithId(%AIName, Player::mountItem, Stinger, 0);
			// AI::SetVar(%AIname, triggerPct, 1000 );
			return;
		}
		
		if(%targetDist > %placeB)
		{
						if(floor(getrandom() * 100) < 50 )
			{
				return;
			}
			if(Player::isJetting(%curTarget))
			{	
				AI::callWithId(%AIName, Player::mountItem, Stinger, 0);
				// AI::SetVar(%AIname, triggerPct, 1000 );
				return;
			}
			else
			{
				AI::callWithId(%AIName, Player::mountItem, RocketLauncher, 0);
				// AI::SetVar(%AIname, triggerPct, 1000 );
				return;
			}
		}

		if(%targetDist > 100)
		{
			    	%switch = floor(getRandom() * 2);
					if(%switch == 0)
					{
//						Player::applyImpulse(%player, "0 0 600");
						AI::callWithId(%AIName, Player::mountItem, Disclauncher, 0);
//						AI::SetVar(%AIname, triggerPct, 0.10 );
					 	// AI::SetVar(%AIname, triggerPct, 1000 );
//		   				echo("Warrior Weapon Disc.");
						return;
					}
					else if(%switch == 1)
					{
						AI::callWithId(%AIName, Player::mountItem, Blaster, 0);
						// AI::SetVar(%AIname, triggerPct, 1000 );
						//	// AI::SetVar(%AIname, triggerPct, 1000 );
						return;
					}
		}
		else
		{
	    	%switch = floor(getRandom() * 7);

		if(%switch == 0)
		{
//			Player::applyImpulse(%player, "0 0 600");
	       AI::callWithId(%AIName, Player::mountItem, Disclauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.10 );
		  // 	// AI::SetVar(%AIname, triggerPct, 1000 );
		   	// echo("Warrior Weapon Disc.");
		   return;
		}
//		else if(%switch == 2)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Stinger, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 1.5 );
//		   	echo("Warrior Weapon Stinger.");
//		   return;
//		}
//		else if(%switch == 3)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, FlameThrower, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.1 );
//		   	echo("Warrior Weapon flamethrower.");
//		   return;
//		}
//		else if(%switch == 4)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, RocketLauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
//		   	echo("Warrior Weapon rocket.");
//		   return;
//		}
		else if(%switch == 1)
	{
           AI::callWithId(%AIName, Player::mountItem, Blaster, 0);
 	    // AI::SetVar(%AIname, triggerPct, 1000 );
	   //	// AI::SetVar(%AIname, triggerPct, 1000 );
		   return;
	}
//		else if(%switch == 6)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Flamer, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.01 );
//		   	echo("Warrior Weapon flamer.");
//		   return;
//		}
		else if(%switch == 2)
		{
	           AI::callWithId(%AIName, Player::mountItem, GrenadeLauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
		  // 	// AI::SetVar(%AIname, triggerPct, 1000 );
		   return;
		}
		else if(%switch == 3)
		{
	           AI::callWithId(%AIName, Player::mountItem, Hammer, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.1 );
		  // 	// AI::SetVar(%AIname, triggerPct, 1000 );
		   return;
		}
		else if(%switch == 4)
		{
		    AI::callWithId(%AIname, Player::mountItem, PlasmaGun, 0);
	 	   // AI::SetVar(%AIname, triggerPct, 1000 );
		  // 	// AI::SetVar(%AIname, triggerPct, 1000 );
		   return;
		}
		else if(%switch == 5)
		{
	           AI::callWithId(%AIName, Player::mountItem, RubberMortar, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.5 );
		  // 	// AI::SetVar(%AIname, triggerPct, 1000 );
		   return;
		}
		else if(%switch == 6)
		{
		    AI::callWithId(%AIname, Player::mountItem, Vulcan, 0);
			// AI::SetVar(%AIname, triggerPct, 1000 );
		   return;
		}
//		else if(%switch == 11)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, ShockwaveGun, 0);
//			   	echo("Warrior Weapon Shockwavegun.");
//		   return;
//		}
//		else if(%switch == 12)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Shotgun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.38 );
//		   	echo("Warrior Weapon Shotgun.");
//		   return;
//		}
//		else if(%switch == 13)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Thumper, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
//		   	echo("Warrior Weapon thumper.");
//		   return;
//		}
		}
}

function AnniDeploysWarrior(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }

			if(floor(getrandom() * 100) < 30 ) // 10 2 4 3
			{
                		AI::DeployItem(%aiId, MortarTurretPack);
                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
//		messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Mortar Turret deployed.");
			}
}

function AnniDeploysWarrior2(%aiName)
{
		%aiId = AI::GetID(%AIName);

    if(!$Actions::Deploys[%aiId]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }			
					if(floor(getrandom() * 100) < 15 ) // 10 2 4 3
			{
                		AI::DeployItem(%aiId, FusionTurretPack);
                		schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
//		 messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Fusion Turret deployed.");
			}

}

function SBThrowGrenade(%aiName)
{

	%id = AI::GetID(%aiName);
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);
	
    if(%loc == "0 0 0")
    {
	return;
    }

    if(!$Actions::Grenades[%id]) 
    {
        return;
    }
	
	%botPlayer = Client::getOwnedObject(%id);
	%armor = Player::getArmor(%botPlayer);
	
		%switch = floor(getRandom() * 3);
		if(%switch == 0)
		{
			%targetdist = 20;
		}
		else if(%switch == 1)
		{
			%targetdist = 40;
		}
		else if(%switch == 2)
		{
			%targetdist = 100;
		}
	
//	%targetdist = 20;

	$boomRockMap["armorTroll"] = "Firebomb";
	$boomRockMap["armorTank"] = "TankBomb";
	$boomRockMap["armorTitan"] = "Mortarbomb";
	$boomRockMap["armorfSpy"] = "Nukebomb";
	$boomRockMap["armormSpy"] = "Nukebomb";
	$boomRockMap["armorfNecro"] = "HoloMine";
	$boomRockMap["armormNecro"] = "HoloMine";
	$boomRockMap["armorfWarrior"] = "Handgrenade";
	$boomRockMap["armormWarrior"] = "Handgrenade";
	$boomRockMap["armorfBuilder"] = "Shockgrenade";
	$boomRockMap["armormBuilder"] = "Shockgrenade";
	$boomRockMap["armorfAngel"] = "ClusterBombmine";

    %boomrock = newObject("","Mine",$boomRockMap[%armor]);
    addToSet("MissionCleanup", %boomrock);
    GameBase::throw(%boomrock,%botPlayer,(%targetdist/75) * 15.0,false);

}
				
function AnniDeploysForBots(%aiName)
{
	
	%aiId = AI::GetID(%aiName);
	
	   %player = Client::getOwnedObject(%aiId);
   %client = Player::GetClient(%aiId);
   %dlevel = GameBase::getDamageLevel(%player);

   %rk = Player::getItemCount(%client, repairkit);
   if   ((%rk > 0) && (%dlevel > 0.2))
   {
     if (GameBase::getAutoRepairRate(%player) == 0)
     {
       Player::decItemCount(%client,repairkit);
       GameBase::setAutoRepairRate(%player,0.15);
     }
   }else if (%dlevel == 0)
   {
     GameBase::setAutoRepairRate(%player,0);
   }
	
	if(!$Actions::Deploys[%aiId]) 
    {
//		messageall(1, "Deploys Are Off.");
        return;
    }
	
	if(BotTypes::IsFlyer(%aiName) == 1)
	{
		return;
	}
	
	if (BotTypes::isCMD(%aiName) == 1)
	{
	return;	
	}
	
	
//	messageall(1, "Deploys Are On And Attempted.");
	
	Item::setVelocity(%aiId, "0 0 0");
		if(BotTypes::IsMortar(%aiName) == 1)
		{
			%switch = floor(getRandom() * 5);
			if(%switch == 0)
			{
			if(floor(getrandom() * 100) < 5 )
			{
			AnniDeploysTroll(%aiName); // Irradiation Turret
			}
			return;
			}
			else if(%switch == 1)
			{
			if(floor(getrandom() * 100) < 5 )
			{
			AnniDeploysTroll2(%aiName); // Plasma Turret
			}
			return;
			}
			else if(%switch == 2)
			{
			if(floor(getrandom() * 100) < 5 )
			{
			AnniDeploysTroll3(%aiName); // Motion Sensor
			}
			return;			
			}
			else if(%switch == 3)
			{
			if(floor(getrandom() * 100) < 5 )
			{
			AnniDeploysBuilder7(%aiName); // laser and platforms
			}
			return;			
			}
			else if(%switch == 4)
			{
			if(floor(getrandom() * 100) < 5 )
			{
			AnniDeploysBuilder8(%aiName); // vortex covered
			}
			return;			
			}
		}
		if(BotTypes::IsPainter(%aiName) == 1)
		{
			%switch = floor(getRandom() * 2);
			if(%switch == 0)
			{
			if(floor(getrandom() * 100) < 10 )
			{
			AnniDeploysWarrior(%aiName); // Mortar Turret
			}
			return;			
			}
			else if(%switch == 1)
			{
			if(floor(getrandom() * 100) < 10 )
			{
			AnniDeploysWarrior2(%aiName); // Fusion Turret
			}
			return;			
			}
		}
		if(BotTypes::IsMiner(%aiName) == 1)
		{
			if(floor(getrandom() * 100) < 2 )
			{
			AnniDeploysNecro(%aiName); // Jump Pad
			}
			return;
		}
		if(BotTypes::IsSniper(%aiName) == 1)
		{
			%switch = floor(getRandom() * 2);
			if(%switch == 0)
			{
			if(floor(getrandom() * 100) < 5 )
			{
			AnniDeploysSpy(%aiName); // Rocket Turret
			}
			return;
			}
			else if(%switch == 1)
			{
			if(floor(getrandom() * 100) < 5 )
			{
			AnniDeploysSpy2(%aiName); // Sensor Jammer
			}
			return;
			}
		}
		if(BotTypes::IsDemo(%aiName) == 1)
		{
			%switch = floor(getRandom() * 2);
			if(%switch == 0)
			{
			if(floor(getrandom() * 100) < 10 )
			{
			AnniDeploysTank(%aiName); // Ion Turret
			}
			return;			
			}
			else if(%switch == 1)
			{
			if(floor(getrandom() * 100) < 5 )
			{			
			AnniDeploysTank2(%aiName); // Camera
			}
			return;			
			}
		}
		if(BotTypes::IsGuard(%aiName) == 1)
		{
			%switch = floor(getRandom() * 6);
			if(%switch == 0)
			{
			if(floor(getrandom() * 100) < 5 )
			{
			AnniDeploysTitan(%aiName); // Shock Turret
			}
			return;			
			}
			else if(%switch == 1)
			{	
			if(floor(getrandom() * 100) < 5 )
			{		
			AnniDeploysTitan2(%aiName);	// Pulse Sensor
			}
			return;			
			}
			else if(%switch == 2)
			{
			if(floor(getrandom() * 100) < 12 )
			{
			AnniDeploysTitan3(%aiName);	 // Force Field Door
			}
			return;			
			}
			else if(%switch == 3)
			{
			if(floor(getrandom() * 100) < 5 )
			{
			AnniDeploysBuilder9(%aiName); // flame turret covered
			}
			return;			
			}
			else if(%switch == 4)
			{
			if(floor(getrandom() * 100) < 5 )
			{
			AnniDeploysBuilder6(%aiName); // laser and crates
			}
			return;			
			}
			else if(%switch == 5)
			{
			if(floor(getrandom() * 100) < 8 )
			{
			AnniDeploysBuilder4(%aiName); // nuclear turret
			}
			return;			
			}
		}
		if(BotTypes::IsMedic(%aiName) == 1)
		{
			%switch = floor(getRandom() * 9);
			if(%switch == 0)
			{
			if(floor(getrandom() * 100) < 7 )
			{
			AnniDeploysBuilder(%aiName); // inventory
			}
			return;			
			}
			else if(%switch == 1)
			{
			if(floor(getrandom() * 100) < 8 )
			{
			AnniDeploysBuilder2(%aiName); // laser turret
			}
			return;			
			}
			else if(%switch == 2)
			{
			if(floor(getrandom() * 100) < 10 )
			{
			AnniDeploysBuilder3(%aiName); // vortex turret
			}
			return;			
			}
			else if(%switch == 3)
			{
			if(floor(getrandom() * 100) < 8 )
			{
			AnniDeploysBuilder4(%aiName); // nuclear turret
			}
			return;			
			}
			else if(%switch == 4)
			{
			if(floor(getrandom() * 100) < 8 )
			{
			AnniDeploysBuilder5(%aiName); // flame turret
			}
			return;			
			}
			else if(%switch == 5)
			{
			if(floor(getrandom() * 100) < 5 )
			{
			AnniDeploysBuilder6(%aiName); // laser and crates
			}
			return;			
			}
			else if(%switch == 6)
			{
			if(floor(getrandom() * 100) < 5 )
			{
			AnniDeploysBuilder7(%aiName); // laser and platforms
			}
			return;			
			}
			else if(%switch == 7)
			{
			if(floor(getrandom() * 100) < 5 )
			{
			AnniDeploysBuilder8(%aiName); // vortex covered
			}
			return;			
			}
			else if(%switch == 8)
			{
			if(floor(getrandom() * 100) < 5 )
			{
			AnniDeploysBuilder9(%aiName); // flame turret covered
			}
			return;			
			}
		}
}

function ArenaBotBeaconAll(%aiName)
{
	%id = AI::GetID(%aiName);

    if(!$Actions::Beacons[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }								
					%vel = Item::getVelocity(%player);

						if(%vel == "0 0 0")
						{
							%trans = GameBase::getMuzzleTransform(%player);
							%smack = 300/25;
							%rot=GameBase::getRotation(%player);
							%len = 30;
							%tr= getWord(%trans,5);
							if(%tr <=0 )%tr -=%tr;
							%up = %tr+0.15;
							%out = 1-%tr;
							%vec = Vector::getFromRot(%rot,%len*%out*%smack,%len*%up*%smack);
							if ( ( getWord(%vec,0) >= 0 || getWord(%vec,0) < 0 ) && ( getWord(%vec,1) >= 0 || getWord(%vec,1) < 0 ) && ( getWord(%vec,2) >= 0 || getWord(%vec,2) < 0 ) )
								Player::applyImpulse(%player, getWord(%vel,0)+getWord(%vec,0)@" "@getWord(%vel,1)+getWord(%vec,1)@" "@getWord(%vel,2)+getWord(%vec,2) );
							else
							{
								admin::message("ERROR: Booster Vectoring error!  Vec=\""@%vec@"\" from person "@GameBase::getName(%client)@" ("@%player@")");
								$Admin = "ERROR: Booster Vectoring error!  Vec=\""@%vec@"\" from person "@GameBase::getName(%client)@" ("@%player@")";
								export("Admin", "config\\Error.log", true);
								return;
							}
							GameBase::playSound(%player, SoundFireMortar, 0);
							Client::sendMessage(%client,0, "Speed Boost Initiated..");	
						}
						else
						{	
							%vel = Vector::Normalize(%vel);
							%vec = GetWord(%vel, 0) * 300 @ " " @ GetWord(%vel, 1) * 300 @ " " @ GetWord(%vel, 2) * 300;
							if ( ( getWord(%vec,0) >= 0 || getWord(%vec,0) < 0 ) && ( getWord(%vec,1) >= 0 || getWord(%vec,1) < 0 ) && ( getWord(%vec,2) >= 0 || getWord(%vec,2) < 0 ) )
								Player::applyImpulse(%player, %vec );
							else
							{
								admin::message("ERROR: Booster Vectoring error!  Vec=\""@%vec@"\" from person "@GameBase::getName(%client)@" ("@%player@")");
								$Admin = "ERROR: Booster Vectoring error!  Vec=\""@%vec@"\" from person "@GameBase::getName(%client)@" ("@%player@")";
								export("Admin", "config\\Error.log", true);
								return;
							}
							GameBase::playSound(%player, SoundFireMortar, 0);
							Client::sendMessage(%client,1, "Speed boost initiated. ~wmortar_fire.wav");	
						}
//	messageall(1, "Beacon Beacon Beacon.");
}