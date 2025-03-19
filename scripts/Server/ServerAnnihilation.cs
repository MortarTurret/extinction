// Anni::Echo("+++++ Starting Annihilation Loadin +++++");

//this variable will remove all the Annihilation datablocks, so only use it if you're hosting LT only server.
$TALT::Stripped = false; 

function ExecFiles(%folder)
{
	Anni::Echo(">> Loading "@%folder@" folder.....");
	
 	%file = File::findFirst(%folder@"\\*.cs");
	while(%file != "")
	{
		if(string::ICompare(file::getbase(%file), %folder) == 0)	// Using ICompare to fudge around case differences...
			exec(%file);	
		else
			%file[%c++] = %file;
		%file = File::findNext(%folder@"\\*.cs");
	}
	%file[%c++] = File::findNext(%folder@"\\*.cs");	
	while(%c-- > 0)
		exec(%file[%c]);
		
	Anni::Echo("<< "@%folder@" finished.");	
}

	exec(NSound);
	exec(BaseExpData);
	exec(BaseDebrisData);
	exec(BaseProjData);
	//exec(Flair); //getting rid of for now
	
// Generics...
	exec(Item);
	exec(Marker);
	exec(Trigger);
	exec(StaticShape);
	exec(Moveable);
	exec(Sensor);
	exec(AI);		
	exec(comchat);	
	exec(Player);
	exec(InteriorLight);
	exec(AnniBotsBeacons); // new beacon usage for total annihilation bots -death
	exec(AnniBots); //new bots -death666
	exec(skywrite);
 	exec(BotGear);
 	exec(BotThink);
 	exec(BotTypes);
 	exec(BotFuncs);
 	exec(BotSpawn);
 	exec(BotTree);
 	exec(BotHUD);
 	exec(BotMove);
 	exec(JetToPos);
				
	//ExecFiles(admin);
//	echo(">> Loading Admin folder.....");
	exec(admin);
	exec(AdminAuto);
	exec(AdminBan);
	exec(adminKill);
	exec(AdminSet);
	exec(Arena);
	exec(ArenaBBootCamp);
	exec(ArenaTD);
	exec(ArenaWin);
	exec(Duel);
	exec(IpLogger);
	//exec(GeoIP);
	exec(TimeFunctions);
	exec(AFKsystem);
	exec(TAFunctions);
//	exec(TACreditShop);
//	echo("<< Admin finished.");
	
	//ExecFiles(Station);
//	echo(">> Loading Station folder.....");
	exec(station);
	exec(AmmoStation);
if(!$TALT::Stripped)
{
	exec(CommandStation);
	exec(DeployableStation);
	exec(InventoryStation);
	exec(MobileStation);
}
	exec(Quick);
	exec(StationTrigger);
	exec(VehicleStation);
//	echo("<< Station finished.");
	
// Brand Name...	
	//ExecFiles(Armor);
//	echo(">> Loading Armor folder.....");
	exec(Armor);
if(!$TALT::Stripped)
{
	exec(Angel);
	exec(Spy);
	exec(Necro);
}
	exec(Warrior);
if(!$TALT::Stripped)
{
	exec(Builder);
	exec(Troll);
	exec(Tank);
	exec(Titan);
}
	exec(LightArmorBase);
	exec(MercenaryElite);
if(!$TALT::Stripped)
{
	exec(armorDM);
	exec(Ghost);
}
//	echo("<< Armor finished.");
	
	//ExecFiles(Weapon);
//	echo(">> Loading Weapon folder.....");
	exec(weapon);
if(!$TALT::Stripped)
{
	exec(AngelFire);
	exec(AngelRepairGun);
	exec(BabyNuke);
	exec(PortalDevice);
}
	exec(Blaster);
	//exec(BlasterBase);
	//exec(BlasterElite);
	exec(ChaingunBase);
	exec(ChaingunElite);
	//exec(ClayPigeon);
if(!$TALT::Stripped)
	exec(CuttingLaser);
	exec(DiscLauncher);
	exec(DiscLauncherBase);
	exec(DiscLauncherElite);
	// exec(Buckler);
if(!$TALT::Stripped)
{
	exec(Fixit);
	exec(Flamer);
	exec(FlameThrower);
	exec(GrenadeLauncher);
}
	exec(GrenadeLauncherBase);
	//exec(GrenadeLauncherElite);
if(!$TALT::Stripped)
{
	exec(Hammer);
	//exec(HDiscLauncher);
	exec(HeavensFury);
	exec(JailGun);
	exec(Mortar);
	exec(OSLauncher);
	exec(ParticleBeam);
	exec(PhaseDisrupter);
	exec(Pitchfork);
}
	exec(Plasmagun);
	exec(PlasmagunBase);
	//exec(PlasmagunElite);
if(!$TALT::Stripped)
{
	exec(RailGun);
	exec(HarpoonGun);
	exec(RocketLauncher);
	exec(GrapplingHook);
	// exec(RocketPodAnn);
	exec(RubberMortar);
	exec(Shockwave);
	// exec(Sweeper);
}
	exec(ShotGun);
if(!$TALT::Stripped)
{
	exec(LaserRifle);
	exec(SniperRifle);
	exec(Soul);
	exec(SpellDeathRay);
	exec(SpellDisarmer);
	exec(SpellFlameStrike);
	exec(SpellFlameThrower);
	exec(SpellShockingGrasp);
	exec(SpellStasis);
	exec(Stinger);
	exec(TankBlastCannon);
	exec(TankRocketLauncher);
	exec(TankRPG);
	exec(TankShredder);
}
	exec(TargetLaser);
if(!$TALT::Stripped)
{
	exec(Vulcan);
	exec(Minigun);
	exec(Thumper);
//	exec(ZZ_GravityGun);
	if($TA::Slap)
		exec(ZZ_Slapper);
	exec(ZZ_GravityGun);	
}
//	echo("<< Weapon finished.");
	
	//ExecFiles(Pack);
//	echo(">> Loading Pack folder.....");
	exec(pack);
if(!$TALT::Stripped)
{
	exec(packAmmo);
	exec(packChameleon);
	//exec(packCloak);
	exec(packCommand);
}
	exec(packEnergy);
if(!$TALT::Stripped)
{
	exec(packGhost);
	exec(packJammer);
	exec(packNova);
	exec(packPhaseShifter);
	exec(packRegeneration);
}
	exec(packRepair);
if(!$TALT::Stripped)
{
	exec(packAirStrike); //added airstrikepack -death666
	exec(packShield);
	exec(packStealthShield);
	exec(packSuicide);
	//exec(packTrueSight);
}
//	echo("<< Pack finished.");
	
	
	//ExecFiles(Turret);
//	echo(">> Loading Turret folder.....");
	exec(turret);
if(!$TALT::Stripped)
{
	exec(FlameTurret);
	exec(FusionTurret);
	exec(IonTurret);
	exec(IrradiationTurret);
	exec(LaserTurret);
	exec(MissileTurret);
	exec(MortarTurret);
	exec(NeuroTurret);
	exec(NuclearTurret);
	//exec(ParticleBeamTurret);
	exec(PlasmaTurret);
	exec(ShockTurret);
	exec(VortexTurret);
}
//	echo("<< Turret finished.");
	
	
//	ExecFiles(Station);
	//ExecFiles(Vehicle);	
//	echo(">> Loading Vehicle folder.....");
	exec(vehicle);
	exec(vehicleFighter);
	exec(vehicleInterceptor);
	exec(vehicleBomber);
	exec(vehicleTransport);
//	echo("<< Vehicle finished.");
	
	//ExecFiles(Deployable);
if(!$TALT::Stripped)
{
//	echo(">> Loading Deployable folder.....");
	exec(Airbase);
	//exec(AmmoStationX);
	// exec(BaseCloak); 
	exec(BigCrate);
	exec(BlastWall);
	exec(BunkerPack);
	exec(SpringPack);
//	exec(LaserWall); // lol death666
	exec(AcceleratorDevicePack);
	exec(Camera);
	//exec(CommandStationX);
	exec(ControlJammer);
	exec(deployTransport);
	exec(Interceptor); //Fighter -death666
	exec(ForceField);
	exec(ForceFieldDoor);
	exec(JailTower);
	exec(JumpPad);
	exec(LargeForceField);
	exec(LargeForceFieldDoor);
	exec(MobileInventory);
	exec(MotionSensor);
	exec(PlasmaFloor);
	exec(Platform);
	exec(PortableGenerator);
	exec(PortableInventoryStation);
	exec(PortableSolar);
	exec(powerFunctions);
	exec(PulseSensor);
	exec(SensorCat);
	exec(SensorJammer);
	exec(Teleporter);
//	echo("<< Deployable finished.");
	
	//ExecFiles(Dropship);
	//echo(">> Loading Dropship folder.....");
	//exec(CommandShip);
	exec(dropShipFunctions);
	  exec(GunShip);
	//exec(SupplyShip);
	//echo("<< Dropship finished.");
	
	
	//ExecFiles(Probe);	
	//echo(">> Loading Probe folder.....");
	exec(droidSurveyDroid);
	exec(ProbeDroid);
	exec(SuicideDroid);
	//echo("<< Probe finished.");
}
	
	//ExecFiles(Misc);
//	echo(">> Loading Misc folder.....");
	exec(Beacon);
	exec(Grenade);
	exec(miscMine);
//	echo("<< Misc finished.");

	exec(ServerItemUsageBase);
	exec(ServerItemUsageElite);
	exec(ServerItemUsage);
	exec(ServerItemUsageAnni);
	
//	ExecFiles(Buildmod);
//	exec(slapped);
	
// if(!$build)
//	exec(HappyBreaker);
//	
//	exec(HappyFlagBreaker);
	