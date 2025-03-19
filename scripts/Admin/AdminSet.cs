
function ServerSwitches(%clientId,%option,%action)
{
	DebugFun("ServerSwitches",%clientId,%option,%action);
	if(%clientId != -1)
	{
		if(%clientId.SecretAdmin)
			%AdminName = SecretAdmin();
		else
			%AdminName = Client::getName(%clientId);
	}	
	else %adminName = "Majority Vote";
	
	if (%action)
	{
		messageAll(0,	%AdminName @ " ENABLED " @ %option @"~wCapturedTower.wav");
		centerprintall("<jc><f1>" @ %AdminName @ " ENABLED " @ %option, 3);
		logAdminAction(%clientId, " ENABLED " @ %option);
	}	
	else
	{
		messageAll(0, %AdminName @ " DISABLED " @ %option @"~wCapturedTower.wav");	
		centerprintall("<jc><f1>" @ %AdminName @ " DISABLED " @ %option, 3);
		logAdminAction(%clientId, " DISABLED " @ %option);	
	}
	if (%option == "Fair Teams"){if(%action) $ANNIHILATION::FairTeams = 1;else $ANNIHILATION::FairTeams = 0;}
	else if(%option == "turret points"){if(%action) $TurretPoints = 1;else $TurretPoints = 0;}
	else if(%option == "deployable turrets"){if(%action) $NoDeployableTurret = 0;else $NoDeployableTurret = 1;}
	else if(%option == "map turrets"){if(%action) $NoMapTurrets = 0;else $NoMapTurrets = 1;}
// start new -death666
	else if(%option == "Deploying Turrets"){if(%action) $DisableDPT = 0;else $DisableDPT = 1;}

	else if(%option == "Pitchforks"){if(%action) $DisablePF = 0;else $DisablePF = 1;}

	else if(%option == "Ghost Packs"){if(%action) $DisableGP = 0;else $DisableGP = 1;}

	else if(%option == "ParticleBeams"){if(%action) $DisablePB = 0;else $DisablePB = 1;}

	else if(%option == "Deploying Gunships"){if(%action) $DisableGS = 0;else $DisableGS = 1;}

	else if(%option == "Deploying Jails"){if(%action) $DisableJL = 0;else $DisableJL = 1;}

	else if(%option == "Deploying Airbases"){if(%action) $DisableAB = 0;else $DisableAB = 1;}

	else if(%option == "Deployable Droids"){if(%action) $DisableDD = 0;else $DisableDD = 1;}
// end new - death666
	else if(%option == "Inventories"){if(%action) $NoInv = 0;else $NoInv = 1;}
	else if(%option == "Vehicle Stations"){if(%action) $NoVehicle = 0;else $NoVehicle = 1;}
	else if(%option == "Generators"){if(%action) $NoGenerator = 0;else $NoGenerator = 1;}
	else if(%option == "Flag Cap Limit"){if(%action) $NoFlagCaps = 0;else $NoFlagCaps = 1;}
	else if(%option == "Voting"){if(%action) $NoVote = 0;else $NoVote = 1;}
	
	else if(%option == "Voting Admin"){if(%action) $ANNIHILATION::VoteAdmin = 1;else $ANNIHILATION::VoteAdmin = 0;}

	else if(%option == "Voting Builder"){if(%action) $ANNIHILATION::VoteBuilding = 1;else $ANNIHILATION::VoteBuilding = 0;}
// start new - death666
	else if(%option == "Voting Turrets"){if(%action) $DisableVT = 1;else $DisableVT = 0;}
// end new - death666
	else if(%option == "Observer Alert"){if(%action) $ANNIHILATION::obsAlert = 1;else $ANNIHILATION::obsAlert = 0;}
	else if(%option == "personal skins"){if(%action) $ANNIHILATION::UsePersonalSkin = 1;else $ANNIHILATION::UsePersonalSkin = 0;}

	else if(%option == "Quick Inventories"){if(%action) $ANNIHILATION::QuickInv = 1;else $ANNIHILATION::QuickInv = 0;}
	else if(%option == "Zappy Inventories"){if(%action) $ANNIHILATION::Zappy = 1;else $ANNIHILATION::Zappy = 0;}
	else if(%option == "Extended Inventories"){if(%action) $ANNIHILATION::ExtendedInvs = 1;else $ANNIHILATION::ExtendedInvs = 0;}

	else if(%option == "team change turret disable"){if(%action) $annihilation::DisableTurretsOnTeamChange = 1;else $annihilation::DisableTurretsOnTeamChange = 0;}
	else if(%option == "Flag Hunter"){if(%action) $FlagHunter::Enabled = 1;else {$FlagHunter::Enabled = false;exec(player);exec(game);exec(objectives);}}


	
}



//===================================


function Admin::setTeamDamageEnable(%admin, %enabled)
{
	DebugFun("Admin::setTeamDamageEnable",%admin,%enabled);
	if(%admin.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%admin);	
	if(%admin == -1 || %admin.isAdmin)
	{
		if(%enabled)
		{
			$Server::TeamDamageScale = 1;
			if(%admin == -1)
			{
				messageAll(0, "Team damage ENABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Team damage ENABLED by consensus.", 3);
				logAdminAction(%admin, "Team damage ENABLED by consensus.");
			}
			else
			{	
				messageAll(0, %AdminName @ " ENABLED team damage.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@%AdminName @ " ENABLED team damage.", 3);
				logAdminAction(%admin, "Team damage ENABLED.");
			}
		}
		else
		{
			$Server::TeamDamageScale = 0;
			if(%admin == -1)
			{
				messageAll(0, "Team damage DISABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Team damage DISABLED by consensus.", 3);
				logAdminAction(%admin, "Team damage DISABLED by consensus.");
			}
			else
			{
				messageAll(0, %AdminName @ " DISABLED Team damage.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@%AdminName @ " DISABLED Team damage.", 3);
				logAdminAction(%admin, "Team damage DISABLED.");
			}
		}
	}
}

function Admin::setBaseDamageEnable(%admin, %enabled)
{
	DebugFun("Admin::setBaseDamageEnable",%admin,%enabled);
	if(%admin.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%admin);		
	
	if(%admin == -1 || %admin.isAdmin)
	{
		if(%enabled)
		{
			$ANNIHILATION::SafeBase = 0;
			$AutoBDMinTeam = 0; // -death666 3.24.17
			if(%admin == -1)
			{
				messageAll(0, "BASE damage ENABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>BASE damage ENABLED by consensus.", 3);
				logAdminAction(%admin, "Base damage ENABLED by consensus.");
			}
			else
			{
				messageAll(0, %AdminName @ " ENABLED BASE damage.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@%AdminName @ " ENABLED BASE damage.", 3);
				logAdminAction(%admin, "BASE damage ENABLED.");
			}
		}
		else
		{
			$ANNIHILATION::SafeBase = 1;
			$AutoBDMinTeam = 50; // -death666 3.24.17
			if(%admin == -1)
			{
				messageAll(0, "Base damage DISABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Base damage DISABLED by consensus.", 3);
				logAdminAction(%admin, "Base damage DISABLED by consensus.");
			}
			else
			{
				messageAll(0, %AdminName @ " DISABLED Base damage.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@%AdminName @ " DISABLED Base damage.", 3);
				logAdminAction(%admin, "Base damage DISABLED.");
			}
		}
	}
}

function Admin::setBaseHealingEnable(%admin, %enabled)
{
	DebugFun("Admin::setBaseHealingEnable",%admin,%enabled);
	if(%admin.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%admin);	
			
	if(%admin == -1 || %admin.isAdmin)
	{
		if(%enabled)
		{
			$ANNIHILATION::BaseHeal = 1;
			AutoRepair(0.003);
			if(%admin == -1)
			{
				messageAll(0, "Base healing ENABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Base healing ENABLED by consensus.", 3);
				logAdminAction(%admin, "Base healing ENABLED by consensus.");
			}
			else
			{
				messageAll(0, %AdminName @ " ENABLED base healing.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@%AdminName @ " ENABLED base healing.", 3);
				logAdminAction(%admin, "base healing ENABLED.");
			}
		}
		else
		{
			$ANNIHILATION::BaseHeal = 0;
			AutoRepair(-0.003);
			if(%admin == -1)
			{
				messageAll(0, "Base healing DISABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Base healing DISABLED by consensus.", 3);
				logAdminAction(%admin, "Base healing DISABLED by consensus.");
			}
			else
			{
				messageAll(0, %AdminName @ " DISABLED base healing.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@%AdminName @ " DISABLED base healing.", 3);
				logAdminAction(%admin, "Base healing DISABLED.");
			}
		}
	}
}

function Admin::setPlayerDamageEnable(%admin, %enabled)
{
	DebugFun("Admin::setPlayerDamageEnable",%admin,%enabled);
	if(%admin.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%admin);	
			
	if(%admin == -1 || %admin.isAdmin)
	{
		if(%enabled)
		{
			$Annihilation::NoPlayerDamage = 0;
			if(%admin == -1)
			{
				messageAll(0, "Player damage ENABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Player damage ENABLED by consensus.", 3);
				logAdminAction(%admin, "Player damage ENABLED by consensus.");
			}
			else
			{
				messageAll(0, %AdminName @ " ENABLED Player damage.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@%AdminName @ " ENABLED Player damage.", 3);
				logAdminAction(%admin, "Player damage ENABLED.");
			}
		}
		else
		{
			$Annihilation::NoPlayerDamage = 1;
			if(%admin == -1)
			{
				messageAll(0, "Player damage DISABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Player damage DISABLED by consensus.", 3);
				logAdminAction(%admin, "Player damage DISABLED by consensus.");
			}
			else
			{
				messageAll(0, %AdminName @ " DISABLED player damage.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@%AdminName @ " DISABLED player damage.", 3);
				logAdminAction(%admin, "Player damage DISABLED.");
			}

		}
	}
}

function Admin::setAreaDamageEnable(%admin, %enabled)
{
	DebugFun("Admin::setAreaDamageEnable",%admin,%enabled);
	if(%admin.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%admin);
			
	if(%admin == -1 || %admin.isAdmin)
	{
		if(%enabled)
		{
			$ANNIHILATION::OutOfArea = "";
			if(%admin == -1)
			{
				messageAll(0, "Map boundries ENABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Map boundries ENABLED by consensus.", 3);
				logAdminAction(%admin, "Map boundries ENABLED by consensus.");
			}
			else
			{
				messageAll(0, %AdminName @ " ENABLED map boundries.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@%AdminName @ " ENABLED map boundries.", 3);
				logAdminAction(%admin, "Map boundries ENABLED.");
			}				
		}
		else
		{
			$ANNIHILATION::OutOfArea = 1;
			if(%admin == -1)
			{
				messageAll(0, "Map boundries DISABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Map boundries DISABLED by consensus.", 3);
				logAdminAction(%admin, "Map boundries DISBLED by consensus.");
			}
			else
			{
				messageAll(0, %AdminName @ " DISABLED map boundries.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@%AdminName @ " DISABLED map boundries.", 3);
				logAdminAction(%admin, "Map boundries DISABLED.");
			}
		}
	}
}
function Admin::setBuild(%admin, %enabled)
{
	DebugFun("Admin::setBuild",%admin,%enabled);
	if(%admin.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%admin);
	
	if(%admin == -1 || %admin.isAdmin)
	{
		if(%enabled)
		{
			$Build = 1;				
			if(%admin == -1)
			{
				messageAll(0, "Builder ENABLED by consensus. Heavens Harpoons, Grappling Hooks and Pitchforks now available in inventories for all armors! ~wCapturedTower.wav");
				centerprintall("<jc><f1>Builder ENABLED by consensus. \n\n<f2>Press I to access stations from anywhere!\n\n<jc><f3>Heavens Harpoons, Grappling Hooks and Pitchforks <f1>now available in inventories for all armors!", 10);
				logAdminAction(%admin, "builder ENABLED by consensus.");
				PopulateItemMax(GrapplingHook,		1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1);
				PopulateItemMax(Grabbler,		1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1);
				PopulateItemMax(Harpoon,			1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1);
				PopulateItemMax(HarpoonAmmo,			30,   30,   30,   30,   30,   30,   30,   30,   30,   30,   30,   30,   30,   30,   30);
			}
			else
			{
				messageAll(0, %AdminName @ " ENABLED Builder. Heavens Harpoons, Grappling Hooks and Pitchforks now available in inventories for all armors! ~wCapturedTower.wav");
				centerprintall("<jc><f1>"@%AdminName @ " ENABLED Builder. \n\n<f2>Press I to access stations from anywhere!\n\n<jc><f3>Heavens Harpoons, Grappling Hooks and Pitchforks <f1>now available in inventories for all armors!", 10);
				logAdminAction(%admin, " ENABLED builder.");
				PopulateItemMax(GrapplingHook,		1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1);
				PopulateItemMax(Grabbler,		1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1);
				PopulateItemMax(Harpoon,			1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1);
				PopulateItemMax(HarpoonAmmo,			30,   30,   30,   30,   30,   30,   30,   30,   30,   30,   30,   30,   30,   30,   30);
			}
		}
		else
		{
			$Build = 0;
			$ABuild = 0;			
 			if(%admin == -1)
 			{
				messageAll(0, "Builder DISABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Builder DISABLED by consensus.", 3);
				logAdminAction(%admin, "builder DISABLED by consensus.");
				PopulateItemMax(GateGun,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
				PopulateItemMax(GrapplingHook,			0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
				PopulateItemMax(Grabbler,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
				PopulateItemMax(Slapper,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
				PopulateItemMax(Harpoon,			0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
				PopulateItemMax(HarpoonAmmo,			0,   0,   15,   15,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
				PopulateItemMax(GravityGun,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
			}
			else
			{
				messageAll(0, %AdminName @ " DISABLED Builder.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@%AdminName @ " DISABLED Builder.", 3);
				logAdminAction(%admin, " DISABLED builder.");
				PopulateItemMax(GateGun,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
				PopulateItemMax(GrapplingHook,			0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
				PopulateItemMax(Grabbler,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
				PopulateItemMax(Slapper,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
				PopulateItemMax(Harpoon,			0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
				PopulateItemMax(HarpoonAmmo,			0,   0,   15,   15,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
				PopulateItemMax(GravityGun,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
			}
		}
	}
}

function Admin::setABuild(%admin, %enabled)
{
	DebugFun("Admin::setABuild",%admin,%enabled);
	if(%admin.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%admin);
	
	if(%admin == -1 || %admin.isAdmin)
	{
		if(%enabled)
		{
			$ABuild = 1;				
			if(%admin == -1)
			{
				messageAll(0, "Advanced Building ENABLED by consensus. Slappers, Portal Guns, and Gravity Guns are now available in inventories for all armors! ~wCapturedTower.wav");
				centerprintall("<jc><f1>Advanced Building ENABLED by consensus. \n\n<f2>Press I to access stations from anywhere!\n\n<jc><f3>Slappers, Portal Guns, and Gravity Guns <f1>now available in inventories for all armors!", 10);
				logAdminAction(%admin, "advanced building ENABLED by consensus.");
				PopulateItemMax(Slapper,		1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1);
				PopulateItemMax(GravityGun,		1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1);	
				PopulateItemMax(GateGun,		1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1);				
			}
			else
			{
				messageAll(0, %AdminName @ " ENABLED Advanced Building. Slappers, Portal Guns, and Gravity Guns are available in inventories for all armors! ~wCapturedTower.wav");
				centerprintall("<jc><f1>"@%AdminName @ " ENABLED Advanced Building. \n\n<f2>Press I to access stations from anywhere!\n\n<jc><f3>Slappers, Portal Guns, and Gravity Guns <f1>now available in inventories for all armors!", 10);
				logAdminAction(%admin, " ENABLED advanced building.");
				PopulateItemMax(Slapper,		1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1);
				PopulateItemMax(GravityGun,		1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1);
				PopulateItemMax(GateGun,		1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1);
			}
		}
		else
		{
			$ABuild = 0;
			$Build = 0;				
 			if(%admin == -1)
 			{
				messageAll(0, "Builder DISABLED by consensus.~wCapturedTower.wav");
				centerprintall("<jc><f1>Builder DISABLED by consensus.", 3);
				logAdminAction(%admin, "builder DISABLED by consensus.");
				PopulateItemMax(GateGun,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
				PopulateItemMax(GrapplingHook,			0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
				PopulateItemMax(Grabbler,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
				PopulateItemMax(Slapper,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
				PopulateItemMax(Harpoon,			0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
				PopulateItemMax(HarpoonAmmo,			0,   0,   15,   15,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
				PopulateItemMax(GravityGun,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
			}
			else
			{
				messageAll(0, %AdminName @ " DISABLED Builder.~wCapturedTower.wav");
				centerprintall("<jc><f1>"@%AdminName @ " DISABLED Builder.", 3);
				logAdminAction(%admin, " DISABLED builder.");
				PopulateItemMax(GateGun,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
				PopulateItemMax(GrapplingHook,			0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
				PopulateItemMax(Grabbler,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
				PopulateItemMax(Slapper,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
				PopulateItemMax(Harpoon,			0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
				PopulateItemMax(HarpoonAmmo,			0,   0,   15,   15,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
				PopulateItemMax(GravityGun,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
			}
		}
	}
}

function Admin::setModeFFA(%clientId)
{
	DebugFun("Admin::setModeFFA",%clientId);
	if(%clientId.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%clientId);
		
	
	if($Server::TourneyMode && (%clientId == -1 || %clientId.isAdmin))
	{
		$Server::TeamDamageScale = 0;
		if(%clientId == -1)
			messageAll(0, "Server switched to Free-For-All Mode.~wCapturedTower.wav");
		else
		{	
			messageAll(0, "Server switched to Free-For-All Mode by " @ %AdminName @ ".~wCapturedTower.wav");
			centerprintall("<jc><f1>"@%AdminName @ " switched to free for all mode.", 3);
			logAdminAction(%clientId, "switched to free for all mode.");
		}
		$Server::TourneyMode = false;
		centerprintall(); // clear the messages
		if($TA::MuteAllTourney)
		{
			//echo("ffa!!");
			$TA::MuteAllTourney = false;
			TA::MuteAllTourney("unmute");
		}
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			if(%cl.isTeamCaptin)
			{
				%cl.isTeamCaptin = false;
			}
			Game::refreshClientScore(%cl); 
		}
		
		if(!$matchStarted && !$countdownStarted)
		{
			if($Server::warmupTime)
				Server::Countdown($Server::warmupTime);
			else	
				Game::startMatch();
		}
	}
}

function Admin::setModeTourney(%clientId)
{
	DebugFun("Admin::setModeTourney",%clientId);
	if(%clientId.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%clientId);
		
	if(!$Server::TourneyMode && (%clientId == -1 || %clientId.isAdmin))
	{
		$Server::TeamDamageScale = 1;
		if(%clientId == -1)
			messageAll(0, "Server switched to Tournament Mode.~wCapturedTower.wav");
		else
		{
			messageAll(0, "Server switched to Tournament Mode by " @ %AdminName @ ".~wCapturedTower.wav");
			centerprintall("<jc><f1>"@%AdminName @ " switched server to Tournament Mode.", 3);
			logAdminAction(%clientId, "Server switched to Tournament Mode.");
		}
		$Server::TourneyMode = true;
		Server::nextMission(true);
	}
}





//=======================================
// map changes


function randommap(%type,%clientId)
{
	DebugFun("randommap",%type,%clientId);
	if(%clientId && %clientId.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%clientId);
		
	if(%clientId)
		messageAll(0,%AdminName@ " picked a random mission.~wCapturedTower.wav");
	else
		// messageAll(0, "Starting a random mission");
	Anni::Echo("GAME: "@%clientId@" Starting a random mission. Type= "@%type);
	
	// new temp fix just in case -baseencrypt
			if($MLIST::Type[%type] == "Training")
		{
			messageAll(0, "big d says please dont vote a training map lol");
			randommap("Capture the Flag");
			return;
		}
	// end new
	
	//$MDESC::Type  		=current type
	//$MLIST::Type[%i]		=list of types
	//$MLIST::MissionList[%i]	=list of maps of %i type
	//$MLIST::EType[%ct]  		=type of map
	//$MLIST::EName[%ct]  		=name of map
	//$MLIST::EText[%ct]  		=textdescription	
	
	if(!%type)
	{
		%picking = 100;
		while(%picking>0)
		{
			//Anni::Echo(%picking--);
			%mapNum = floor(getrandom() * $MLIST::Count);
      			%nextMission = ($MLIST::EName[%mapNum]);
			//Anni::Echo($MLIST::EType[%mapNum]);
			// if($MLIST::EType[%mapNum] != "Training")

// new start
//         	if(($MLIST::EType[%mapNum] != "CTF BOTS" && $MLIST::EType[%mapNum] != "Ski Maps" && $MLIST::EType[%mapNum] != "Misc Dynamix Maps" && $MLIST::EType[%mapNum] != "Training" && $MLIST::EType[%mapNum] != "CTF Spawn" && $MLIST::EType[%mapNum] != "Tribes Racer" ))
	        if(($MLIST::EType[%mapNum] != "CTF BOTS" && $MLIST::EType[%mapNum] != "Ski Maps" && $MLIST::EType[%mapNum] != "Training" && $MLIST::EType[%mapNum] != "CTF Spawn" ))
 	{
	%picking = false;
 	}		
		}	
	}
// end new
	else
	{
//		%typelist = $MLIST::MissionList[1]; 91622
		%typelist = $MLIST::MissionList[%type];
		//Anni::Echo(%typelist);
		for(%i = 0; getword(%typelist,%i) != -1; %i++)
		{
			%NumMaps++;
		}
		%mapNum = getword(%typelist,floor(getrandom() * %NumMaps));	
	}
	 messageAll(0,"Mission: "@$MLIST::EName[%mapNum]@" ("@$MLIST::EType[%mapNum]@")");
	$timeLimitReached = true;
	Server::loadMission($MLIST::EName[%mapNum]);
	return $MLIST::EName[%mapNum];
}

function nextmap(%clientId)
{
	DebugFun("nextmap",%clientId);
	if(%clientId.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%clientId);
	
	%nextMission = $nextMission[$missionName];
	if(%clientId)
	{
		if($TA::RandomMission)
		{
			messageAll(0,%AdminName@ " started the next mission.~wCapturedTower.wav");// "@%nextMission@".~wCapturedTower.wav");
		}
		else
		{
			// messageAll(0,%AdminName@ " started the next mission "@%nextMission@".~wCapturedTower.wav");
			   messageAll(0,%AdminName@ " started the next mission.~wCapturedTower.wav");
		}
	}
	else
	{
		if($TA::RandomMission)
		{
			messageAll(0, "Starting next mission.");
		}
		else
		{
			// messageAll(0, "Starting next mission "@%nextMission@".");
			messageAll(0, "Starting next mission.");
		}
	}
	Anni::Echo("GAME: Starting next map.");
	$timeLimitReached = true;
	if($TA::RandomMission)
		Server::nextMission(false,true);
	else
		Server::nextMission();
}

function replaymap(%clientId)
{
	DebugFun("replaymap",%clientId);
	if(%clientId.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%clientId);
	
	if(%clientId)
		messageAll(0,%AdminName@ " restarted the mission.~wCapturedTower.wav");
	else
		messageAll(0, "Restarting Mission");
	Anni::Echo("GAME: restarting map.");
	$timeLimitReached = true;
	Server::nextMission(true);
}
//=================================================

function ReturnFlags()
{
	DebugFun("ReturnFlags");
	%group = nameToID("MissionCleanup/ObjectivesSet");
	//messageall(1,"returning flags");
	for(%i = 0; (%obj = Group::getObject(%group, %i)) != -1; %i++)
	{
		%name = Item::getItemData(%obj);
		if(%name == flag)
		{
		
			if(!%obj.atHome)
			{			
				messageall(1,"Returning "@ getTeamName(GameBase::getTeam(%obj))@"'s flag to base.");
				%player = %obj.carrier;
				Annihilation::setItemCount(%player, Flag, 0);
				GameBase::setPosition(%obj, %obj.originalPosition);
				GameBase::setIsTarget(%obj,false);	
				Item::setVelocity(%obj, "0 0 0");
				GameBase::startFadeIn(%obj);
				%obj.atHome = true;
				%obj.carrier = -1;
				%obj.pickupSequence++;
				%player.carryFlag = "";	
				
				//Rotating flag fix, completed 2/3/2007 8:45PM. Soooo Lazy.. 
				if(%obj.dummy)
					deleteobject(%obj.dummy);				
						
				Item::Hide(%obj,false);
			}
		}
	}	
}
