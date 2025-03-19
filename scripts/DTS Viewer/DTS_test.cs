
function Define::StaticDeployable(%shape)
{

	%description = "Deployable "@%shape;
	%Data = 	"StaticShapeData Deployable"@%shape@ 
		" { "@
			"shapeFile = \""@%shape@"\";"@
			"debrisId = defaultDebrisSmall;"@
			"maxDamage = 1;"@
			"visibleToSensor = false;"@
			"isTranslucent = true;"@
			"description = \""@%description@"\";"@
		"};"; 		
		
	eval(%Data);
}

function ExportDataBlocks()
{
	%consolemode = $Console::LogMode; 
	$Console::LogMode = "2"; 
Anni::Echo("!! Starting data block export !! .............................................................");
	%file = File::FindFirst("*.dts");
	%count = 0;
	while(%file != "")
	{	
		%shape = File::getBase(%file);		
		%file = File::FindNext("*.dts");
		%nopass = false;
		%end = false;
		for(%i = 0; !%end ; %i++)
		{
			%word = getword("teleporter arrow50 arrow25 ammopad command pulse trail shield plant newdoor tree hover armor tumult steamvent plume exp ex mflame lflame laserhit hflame fusionbolt flash fiery enbolt chainspk sprk shockwave rsmoke plastrail plasmatrail plasmaex",%i);
			%find = String::findSubStr(%shape,%word);
		//	Anni::Echo(%shape@", "@%word@", "@%i@", "@%find);
			
			if(%find != -1)
				%nopass = true;
			if(%word == -1 || %word = "")
				%end = true;
		}
	
		//		Pack::Define(%name,%shape,%description,%price,%icon,%min,%max,%type,%active,%reload,%ready,%fire)
		if(!%nopass)
		{
		//	%item = %shape@"pack";
			Anni::Echo("Defining "@%shape@"  static                          ++");
			$Deployable[%count] = "Deployable"@%shape;
			Define::StaticDeployable(%shape);
			%count++;
		//	$ItemMax[armormBuilder, %item] = 1;
		//	$ItemMax[armorfBuilder, %item] = 1;
		//	$InvList[%item] = 1;
		//	$MobileInvList[%item] = 1;
		//	$RemoteInvList[%item] = 1;
		//	AddItem(%item);				
				
		}	
		else
		{
			Anni::Echo("Skipping "@%shape);
		}
			
	}
	
	$Console::LogMode = %consolemode; 
	Anni::Echo("Found " @ %count @ " .dts files loaded");
	$Slapper::DeployableMax = %count-1;
}

function BuildDTS()
{
	%type = "AMMO1 AMMO2 AMMOPACK AMMOPAD AMMOUNIT AMMOUNIT_REMOTE ANTEN_LAVA ANTEN_LRG ANTEN_MED ANTEN_ROD ANTEN_SMALL ARMORKIT ARMORPACK ARMORPATCH BIGTWIG BLUEX BREATH BRIDGE BULLET CACTUS1 CACTUS2 CACTUS3 CAMERA CHAINGUN CHAINSPK CHAINTURRET CMDPNL COMMAND DIRARROWS DISC DISCAMMO DISCB DISPLAY_ONE DISPLAY_THREE DISPLAY_TWO DOOR_4X4_DIAGONAL DOOR_8X8_L DOOR_8X8_R DOOR_BOT DOOR_TOP DSPLY_H1 DSPLY_H2 DSPLY_S1 DSPLY_S2 DSPLY_V1 DSPLY_V2 DUSTPLUME ELEVATBG ELEVATOR16X16_OCTO ELEVATOR6X4 ELEVATOR6X4THIN ELEVATOR6X6THIN ELEVATOR_4X4 ELEVATOR_4X5 ELEVATOR_5X5 ELEVATOR_6X5 ELEVATOR_6X6 ELEVATOR_6X6_OCTAGON ELEVATOR_8X4 ELEVATOR_8X6 ELEVATOR_8X8 ELEVATOR_9X9 ELEVPAD2 ELEVPAD3 ENBOLT ENDARROW ENERGYGUN ENERPAD ENEX FIERY FLAG FLAGSTAND FLASH_LARGE FLASH_MEDIUM FLASH_SMALL FLYER FORCE FORCEFIELD FORCEFIELD_3X4 FORCEFIELD_4X14 FORCEFIELD_4X17 FORCEFIELD_4X8 FORCEFIELD_5X5 FUSIONBOLT FUSIONEX GENERATOR GENERATOR_P GRENADE GRENADEL GRENADETRAIL GRENAMMO GUNTURET HARMOR HELLFIREGUN HFLAME HOVER_APC HOVER_APC_SML INDOORGUN INVENTORY_STA INVENT_REMOTE JETPACK LARMOR LASERHIT LFEMALE LFLAME LIQCYL LOGO MAGCARGO MAINPAD MARMOR MFEMALE MFLAME MICROEX MINE MINEAMMO MISSILETURRET MORTAR MORTARAMMO MORTAREX MORTARGUN MORTARPACK MORTARTRAIL MORTAR_TURRET MRTWIG NEWDOOR1_L NEWDOOR1_R NEWDOOR2_L NEWDOOR2_R NEWDOOR3_L NEWDOOR3_R NEWDOOR4_L NEWDOOR4_R NEWDOOR5 NEWDOOR6_L NEWDOOR6_R PAINT PAINTGUN PANEL_BLUE PANEL_SET PANEL_VERTICAL PANEL_YELLOW PLANT1 PLANT2 PLASAMMO PLASMA PLASMABOLT PLASMAEX PLASMATRAIL PLASMAWALL PLASTRAIL PULSE RADAR RADAR_SMALL REMOTETURRET REPAIRGUN ROCKET RSMOKE SAT_BIG SENSORJAMPACK SENSOR_JAMMER SENSOR_PULSE_MED SENSOR_SMALL SHIELD SHIELDPACK SHIELD_LARGE SHIELD_MEDIUM SHOCKWAVE SHOCKWAVE_LARGE SHOTGUN SHOTGUNBOLT SHOTGUNEX SHOTSPRK SMOKE SNIPER SNOWPLUME SOLAR SOLAR_MED STAFF STEAMVENT2_GRASS STEAMVENT2_MUD STEAMVENT_GRASS STEAMVENT_MUD TELEPORTER TELEPORT_SQUARE TELEPORT_VERTICAL TOWER TRACER TREE1 TREE2 TUMULT_LARGE TUMULT_MEDIUM TUMULT_SMALL VEHI_PUR_PNL VEHI_PUR_POLES W64ELEVPAD ZAP ZAP_5 ";
	Anni::Echo("*** starting dts export ***");
	for(%i = 0; (%word = getWord(%type, %i)) != -1; %i++)
	{
		Pack::Define(%name,%shape,%description,%price,%icon,%min,%max,%type,%active,%reload,%ready,%fire);			
	
	}
	Anni::Echo(%i+1 @ " .dts files exported");
}



