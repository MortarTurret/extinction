dbecho(1,"Precaching Bot AI Functions");

$SPOONBOT::Version = "1.3";

// Death666 moved to nsound.cs
// SoundData SoundBotRepairItem
// {
//   wavFileName = "repair.wav";
//   profile = Profile3dNear;
// };

// AI support functions.
//

//
// This function creates an AI player using the supplied group of markers 
//    for locations.  The first marker in the group gives the starting location 
//    of the the AI, and the remaining markers specify the path to follow.  
//
// Example call:  
// 
//    createAI( guardNumberOne, "MissionGroup\\Teams\\team0\\guardPath", larmor );
//

//globals
//--------
// path type
// 0 = circular
// 1 = oneWay
// 2 = twoWay
$AI::defaultPathType = 1; //run oneWay paths. 

//armor types
//light = larmor
//medium = marmor
//heavy = harmor
// $AI::defaultArmorType = "armorfWarrior";

function pickbotarmortypeANUS()
{
	    $botarmortype = floor(getRandom() * 2);

		if($botarmortype == 0)
		{
		$botarmortype = "armorfWarrior";
		}
		else if($botarmortype == 1)
		{
		$botarmortype = "armorfWarrior"; 
		}
	
		$AI::defaultArmorType = $botarmortype;
}

function pickbotarmortype()
{
	    $botarmortype = floor(getRandom() * 7);

		if($botarmortype == 0)
		{
		$botarmortype = "armorfWarrior"; // armorfWarrior
		// messageall(1, "An Warrior was created.");
		}
		else if($botarmortype == 1)
		{
		$botarmortype = "armorTitan"; // armorTitan
			//	messageall(1, "An Titan was created.");
		}
		else if($botarmortype == 2)
		{
		$botarmortype = "armorTank"; // armorTitan
		//		messageall(1, "An Tank was created.");
		}
		else if($botarmortype == 3)
		{
		$botarmortype = "armorTroll"; // armorTank
		//		messageall(1, "An Troll was created.");
		}
		else if($botarmortype == 4)
		{
		$botarmortype = "armorfNecro"; // armorTroll
		//		messageall(1, "An Necro was created.");
		}
		else if($botarmortype == 5)
		{
		$botarmortype = "armorfSpy"; // armorTroll
		//		messageall(1, "An Spy was created.");
		}
		else if($botarmortype == 6)
		{
		$botarmortype = "armorfBuilder"; // armorTroll
		//		messageall(1, "An Builder was created.");
		}
		
		$AI::defaultArmorType = $botarmortype;
}

$AI::defaultArmorType = "armorfWarrior";

//---------------------------------
//createAI()
//---------------------------------

function createAI( %aiName, %markerGroup, %armorType, %name )
{
   %group = nameToID( %markerGroup );
   %voice = "male2";
   if( %group == -1 || Group::objectCount(%group) == 0 )
   {
      dbecho(1, %aiName @ "Couldn't create AI: " @ %markerGroup @ " empty or not found." );
      return -1;
   }
   else
   {
      %spawnMarker = Group::getObject(%group, 0);

//    %spawnMarker = AI::pickRandomSpawn(%team); //Much more convenient

      %spawnPos = GameBase::getPosition(%spawnMarker);
      %spawnRot = GameBase::getRotation(%spawnMarker);

		if((String::findSubStr(%aiName, "Optimus_Prime") >= 0) || (String::findSubStr(%aiName, "Android18") >= 0) || (String::findSubStr(%aiName, "Bumblebee") >= 0) || (String::findSubStr(%aiName, "Commander_Data") >= 0) || (String::findSubStr(%aiName, "Megatron") >= 0) || (String::findSubStr(%aiName, "T_800") >= 0) || (String::findSubStr(%aiName, "Jetfire") >= 0) || (String::findSubStr(%aiName, "R2_D2") >= 0) || (String::findSubStr(%aiName, "Starscream") >= 0) || (String::findSubStr(%aiName, "Asimo") >= 0) || (String::findSubStr(%aiName, "Dinobot") >= 0) || (String::findSubStr(%aiName, "Hal_9000") >= 0) || (String::findSubStr(%aiName, "Sentinel_Prime") >= 0) || (String::findSubStr(%aiName, "Spot") >= 0))
		{
			pickbotarmortype();
			%voice = "male2";
		}


      if( AI::spawn( %aiName, $AI::defaultArmorType, %spawnPos, %spawnRot, %aiName, %voice ) != "false" )
      {
	$Spoonbot::NumBots = $Spoonbot::NumBots + 1;
         // The order number is used for sorting waypoints, and other directives.
         // Set to two so it won't fuck up my precious chasing code :-P
  BotFuncs::InitVars( %aiId );   

  Client::setSkin(%aiId, $Server::teamSkin[Client::getTeam(%aiId)]);  
  

// if (BotTypes::IsMedic(%newName))    //As of yet only one Medic can work in the Object Repair Task Queue. (This means repairing Turrets, etc)
// {
// if (%teamnum == 0)
// $Spoonbot::Team0Medic = %aiId;
// if (%teamnum == 1)
// $Spoonbot::Team1Medic = %aiId;
// }


//  schedule("BotThink::Think(" @ %aiId @ ", True);", 3);      


         %orderNumber = 2;
         
         for(%i = 1; %i < Group::objectCount(%group); %i = %i + 1)
         {
             
            %spawnMarker = Group::getObject(%group, %i);
            %spawnPos = GameBase::getPosition(%spawnMarker);

            
//            AI::DirectiveWaypoint( %aiName, %spawnPos, %orderNumber );
            
//            %orderNumber++;
         }

      }
      else{
         dbecho( 1, "Failure spawning: " @ %aiName );
      }
   }
}

//-----------------------------------
// AI::initDrones()
//-----------------------------------
function AI::initDrones(%team, %numAi)
{
	dbecho(1, "spawning team " @ %team @ " ai...");
   for(%guard = 0; %guard < %numAi; %guard++)
   {
      //check for internal data
      %tempSet = 	nameToID("MissionGroup\\Teams\\team" @ %team @ "\\AI");
      %tempItem = Group::getObject(%tempSet, %guard);
      %aiName = Object::getName(%tempItem);
      
      %set = nameToID("MissionGroup\\Teams\\team" @ %team @ "\\AI\\" @ %aiName);
      %numPts = Group::objectCount(%set);
      
      if(%numPts > 0)
      {



         createAI(%aiName, %set, $AI::defaultArmorType, %aiName);

         %aiId = ai::GetId( %aiName );
         GameBase::setTeam(%aiId, %team);
         AI::setVar( %aiName,  iq,  60 );
         AI::setVar( %aiName,  attackMode, 1);
         AI::setVar( %aiName,  pathType, $AI::defaultPathType);
//      	 schedule("AI::setWeapons(" @ %aiName @ ");", 1);
	 %aiId = ai::GetId(%aiName);
      	 schedule("AI::setWeapons(" @ %aiId @ ");", 1);
      }
      else
         dbecho(1, "no info to spawn ai...");
   }
}


//------------------------------------------------------------------
//functions to test and move AI players.
//
//------------------------------------------------------------------

//
//This function will spawn an AI player about 5 units away from the 
//player that is passed to the function(%commandIssuer).
//
//
$numAI = 0;
function AI::helper(%aiName, %armorType, %commandIssuer)
{
   %spawnMarker = GameBase::getPosition(%commandIssuer);
   %xPos = getWord(%spawnMarker, 0) + floor(getRandom() * 15);
   %yPos = getword(%spawnMarker, 1) + floor(getRandom() * 10);
   %zPos = getWord(%spawnMarker, 2) + 5;
   %rPos = GameBase::getRotation(%commandIssuer);
   
   dbecho(2, "Spawning AI helper at position " @ %xPos @ " " @ %yPos @ " " @ %zPos);
   dbecho(2, "Current Issuer rotation: " @ %rPos);
      
   %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;
   %newName = %aiName @ $numAI;
   $numAI++;
   Ai::spawn(%newName, %armorType, %aiSpawnPos, %rPos,  %newName, %voice );
   $Spoonbot::NumBots = $Spoonbot::NumBots + 1;
   return ( %newName );
}

//
//This function will move an AI player to the position of an object
//that the players LOS is hitting(terrain included). Must be `	within 50 units.
//
//
function AI::moveToLOS(%aiName, %commandIssuer) 
{
		%aiId = BotFuncs::GetId(%aiName);
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
	
   %issuerRot = GameBase::getRotation(%commandIssuer);
   %playerObj = Client::getOwnedObject(%commandIssuer);
   %playerPos = GameBase::getPosition(%commandIssuer);
      
   //check within max dist
   if(GameBase::getLOSInfo(%playerObj, 100, %issuerRot))
   { 
      %newIssuedVec = $LOS::position;
	  %distance = Vector::getDistance(%playerPos, %newIssuedVec);
	  dbecho(2, "Command accepted, AI player(s) moving....");
	  dbecho(2, "distance to LOS: " @ %distance);
//	  AI::DirectiveWaypoint( %aiName, %newIssuedVec, 2, 2 );
   }
   else
      dbecho(2, "Distance to far.");
      
   dbecho(2, "LOS point: " @ $LOS::position);
}

//This function will move an AI player to a position directly in front of
//the player passed, at a distance that is specified.
function AI::moveAhead(%aiName, %commandIssuer, %distance) 
{
   
   %issuerRot = GameBase::getRotation(%commandIssuer);
   %commPos  = GameBase::getPosition(%commandIssuer);
//   dbecho(2, "Commanders Position: " @ %commPos);
   
   //get commanders x and y positions
   %comm_x = getWord(%commPos, 0);
   %comm_y = getWord(%commPos, 1);
   
   //get offset x and y positions
   %offSetPos = Vector::getFromRot(%issuerRot, %distance);
   %off_x = getWord(%offSetPos, 0);
   %off_y = getWord(%offSetPos, 1);
   
   //calc new position
   %new_x = %comm_x + %off_x;
   %new_y = %comm_y + %off_y;
   %newPos = %new_x  @ " " @ %new_y @ " 0";
  
   //move AI player
//   dbecho(2, "AI moving to " @ %newPos);
//   AI::DirectiveWaypoint(%aiName, %newPos, 2, 2);
}  

//
// OK, this is the complete command callback - issued for any command sent
//    to an AI. 
//
function AI::onCommand ( %name, %commander, %command, %waypoint, %targetId, %cmdText, 
         %cmdStatus, %cmdSequence )
{
   %aiId = BotFuncs::GetId( %name );
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
if (%aiId==0)
	return;
	
   %T = GameBase::getTeam( %aiId );
   %groupId = nameToID("MissionGroup\\Teams\\team" @ %T @ "\\AI\\" @ %name ); 
  	%nodeCount = Group::objectCount( %groupId );
//   dbecho(2, "checking drone information...." @ " number of nodes: " @ %nodeCount);
//   dbecho(2, "AI id: " @ %aiId @ " groupId: " @ %groupId);
   
 	   if( %command == 1 )  //Attack
	   {
	      // must convert waypoint location into world location.  waypoint location
	      //    is given in range [0-1023, 0-1023].  
	      %worldLoc = WaypointToWorld ( %waypoint );

//	      AI::DirectiveRemove( %name, 2 );   //Crude way to clear all directives. Needs to be done because else the bots won't
                                                 //respond to any new orders!

              Vehicle::passengerJump(0,%aiId,0);  //Crude way to make passengers hop off vehicles :-P
//              AI::Jump(%aiId);                    //Just jumps. Much more convenient


              %BotRot = GameBase::getRotation(%aiId);
              if(GameBase::getLOSInfo(Client::getOwnedObject(%aiId), 150, %BotRot))    //Test if AI is within firing range
                {
//	          AI::DirectiveTargetPoint( %name, %worldLoc, 2);  //Fucks up AI::DirectiveList
                }
                else
                {
//	          AI::DirectiveWaypoint( %name, %worldLoc, 2);
                }



	          AI::DirectiveWaypoint( %name, %worldLoc, 3000);
	          schedule ("AI::DirectiveRemove(" @ %name @ ", 3000);",10);

	   }





	   if( %command == 2 )  //Defend
	   {
	      // must convert waypoint location into world location.  waypoint location
	      //    is given in range [0-1023, 0-1023].  
	      %worldLoc = WaypointToWorld ( %waypoint );

//	      AI::DirectiveRemove( %name, 2 );   //Crude way to clear all directives. Needs to be done because else the bots won't
                                                 //respond to any new orders!



        if (getWord(%cmdText, 0) == "Deploy") //Deploy
         {
            if (getWord(%cmdText, 1) == "pulse") //Deploy Pulse Sensor
          {
          //    AI::DeployItem(%aiId, PulseSensorPack);
              schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
          }

            if (getWord(%cmdText, 1) == "sensor") //Deploy Sensor Jammer
          {
          //    AI::DeployItem(%aiId, DeployableSensorJammerPack);
              schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
          }

            if (getWord(%cmdText, 1) == "motion") //Deploy Motion Sensor
          {
          //    AI::DeployItem(%aiId, MotionSensorPack);
              schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
          }

            if (getWord(%cmdText, 1) == "camera") //Deploy Camera
          {
          //    AI::DeployItem(%aiId, CameraPack);
              schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
          }

            if (getWord(%cmdText, 1) == "Ammo") //Deploy Ammo Station
          {
          //    AI::DeployItem(%aiId, DeployableAmmoPack);
              schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
          }

            if (getWord(%cmdText, 1) == "Inventory") //Deploy Inventory Station
          {
          //    AI::DeployItem(%aiId, DeployableInvPack);
              schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
          }

            if (getWord(%cmdText, 1) == "Turret") //Deploy Turret
          {
          //    AI::DeployItem(%aiId, TurretPack);
              schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
          }

            if (getWord(%cmdText, 1) == "beacon") //Deploy Beacon
          {
          //    AI::DeployItem(%aiId, Beacon);
              schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
          }
         }
          else
         {


//           %xPos = getWord(%worldLoc, 0);
//           %yPos = getword(%worldLoc, 1);
//           %zPos = getWord(%worldLoc, 2);

//           newObject("AI", SimGroup);
//           newObject(%name, SimGroup);
//           newObject("Marker1", Marker, PathMarker,0,%xPos,%yPos,%zPos,0,0,0);
//           addToSet(%name, "Marker1");
//           addToSet("AI", %name);
//           addToSet("MissionGroup\\Teams\\team" @ %T, AI);

             AI::DirectiveFollow( %name, %commander, 0,3000);
             schedule ("AI::DirectiveRemove(" @ %name @ ", 3000);",10);

//	     dbecho ( 2, %name @ " IS PROCEEDING TO LOCATION " @ %worldLoc );
         }

  	 if( %command == 3 )  //Repair
	 {
	     return;
	 }


      }


//	   dbecho( 1, "AI::OnCommand() issued to  " @ %name @ "  with parameters: " );
//	   dbecho( 1, "Cmdr:        " @ %commander );
//	   dbecho( 1, "Command:     " @ %command );
//	   dbecho( 1, "Waypoint:    " @ %waypoint );
//	   dbecho( 1, "TargetId:    " @ %targetId );
//	   dbecho( 1, "cmdText:     " @ %cmdText );
//	   dbecho( 1, "cmdStatus:   " @ %cmdStatus );
//	   dbecho( 1, "cmdSequence: " @ %cmdSequence );

 
}


// Play the given wave file FROM %source to %DEST.  The wave name is JUST the basic wave
// name without voice base info (which it will grab for you from the source client Id).  
// Basically does some string fiddling for you.  
//
// Example:
//    Ai::soundHelper( 2051, cheer3 );
//
function Ai::soundHelper( %sourceId, %destId, %waveFileName )
{
   %wName = strcat( "~w", Client::getVoiceBase( %sourceId ) );
   %wName = strcat( %wName, ".w" );
   %wName = strcat( %wName, %waveFileName );
   %wName = strcat( %wName, ".wav" );
   
   dbecho( 2, "Trying to play " @ %wName );
   
   Client::sendMessage( %destId, 0, %wName );
}


function Ai::messageHelper(%targetId, %msg )
{
    Client::sendMessage( %targetId, 0, %msg );
}

//				Ai::soundHelperTeam( %aiId, %cl, $vcheerList[%msgIdx] );
//				Ai::soundHelperTeam( %aiId, %cl, waitsig );

// Default periodic callback.  [Note by default it isn't called unless a frequency 
//    is set up using AI::CallbackPeriodic().  Type in that command to see how 
//    it works].  
function AI::onPeriodic( %aiName )
{
//   dbecho(2, "onPeriodic() called with " @ %aiName );
}



//The following callbacks are responsible for the bot's chasing behaviour.

function AI::onDroneKilled(%aiName)
{

if((String::findSubStr(%aiName, "Optimus_Prime") >= 0) || (String::findSubStr(%aiName, "Android18") >= 0) || (String::findSubStr(%aiName, "Bumblebee") >= 0) || (String::findSubStr(%aiName, "Commander_Data") >= 0) || (String::findSubStr(%aiName, "Megatron") >= 0) || (String::findSubStr(%aiName, "T_800") >= 0) || (String::findSubStr(%aiName, "Jetfire") >= 0) || (String::findSubStr(%aiName, "R2_D2") >= 0) || (String::findSubStr(%aiName, "Starscream") >= 0) || (String::findSubStr(%aiName, "Asimo") >= 0) || (String::findSubStr(%aiName, "Dinobot") >= 0) || (String::findSubStr(%aiName, "Hal_9000") >= 0) || (String::findSubStr(%aiName, "Sentinel_Prime") >= 0) || (String::findSubStr(%aiName, "Spot") >= 0))
{
	%id = ai::GetId( %aiName );
	$Actions::Hunting[%id] = false;
	$Actions::Flying[%id] = false;
	$Actions::Speaking[%id] = false;
	$Actions::Grenades[%id] = false;
	$Actions::Beacons[%id] = false;
	$Actions::Weapons[%id] = false;
	$Actions::Deploys[%id] = false;
	$Actions::Passenger[%id] = false;
	
	%aiId = BotFuncs::GetId(%aiName);
    %team = GameBase::getTeam(%aiId);
	
	// $Spoonbot::IsPilot[%client] = True;
	%player = Client::getOwnedObject(%aiId);
		if (%aiId == $Spoonbot::IsPilot[%aiId])
		{
			//	messageall(1, "You Killed A Pilot.");
		}
	
	    if (%aiId == $Spoonbot::Team0Medic)
		{
			// messageall(1, "You Killed A Medic.");
			$TeamMedicCount[%team] = 0;
			// messageall(1, "Medic Count Set To Zero.");
		}
		
		if (%aiId == $Spoonbot::Team1Medic)
		{
			// messageall(1, "You Killed A Medic.");
			$TeamMedicCount[%team] = 0;
			// messageall(1, "Medic Count Set To Zero.");
		}
		
		if (%aiId == $Spoonbot::Team0Flyer)
		{
			// messageall(1, "You Killed A FLyer.");
			$TeamFlyerCount[%team] = 0;
			// messageall(1, "Flyer Count Set To Zero.");
		}
		
		if (%aiId == $Spoonbot::Team1Flyer)
		{
			// messageall(1, "You Killed A Flyer.");
			$TeamFlyerCount[%team] = 0;
			// messageall(1, "Flyer Count Set To Zero.");
		}
		
		if (%aiId == $Spoonbot::Team0Guard)
		{
			// messageall(1, "You Killed A Guard.");
			$TeamGuardCount[%team] = 0;
			// messageall(1, "Guard Count Set To Zero.");
		}
		
		if (%aiId == $Spoonbot::Team1Guard)
		{
			// messageall(1, "You Killed A Guard.");
			$TeamGuardCount[%team] = 0;
			// messageall(1, "Guard Count Set To Zero.");
		}
		
		if (%aiId == $Spoonbot::Team0Sniper)
		{
			// messageall(1, "You Killed A Sniper.");
			$TeamGuardCount[%team] = 0;
			// messageall(1, "Sniper Count Set To Zero.");
		}
		
		if (%aiId == $Spoonbot::Team1Sniper)
		{
			// messageall(1, "You Killed A Sniper.");
			$TeamGuardCount[%team] = 0;
			// messageall(1, "Sniper Count Set To Zero.");
		}
	
		//	%player = Client::getOwnedObject(%id);
		//	 	if(%player.mediccnt >= 1)
		//	{
		//		messageall(1, "You Killed A Medic.");
		//	}


   $Spoonbot::NumBots = $Spoonbot::NumBots - 1;
   if( ! $SinglePlayer )
   {	   
    $Spoonbot::BotStatus[%aiId] = "Dead";
    %curTarget = ai::getTarget( %aiName );
    %targetName = Client::getName(%curTarget);

    $BotThink::Definitive_Attackpoint[%aiId] = "";  // ERROR: I think there's no player ID for a dead bot.
    $BotThink::ForcedOfftrack[%aiId] = true;

    if ($Spoonbot::BotChat)
	{
	    %chatdelay = floor(getRandom() * (10 - 0.1));
	    schedule("AI::RandomSuckMsg(" @ %aiName @ ", " @ %team @ ");", %chatdelay );
	}


    if (%aiId != $DoNotRespawnAI)
    {
      // Delay $RespawnDelay seconds before respawning
      if ($Spoonbot::RespawnDelay == 0)
      {
          $Spoonbot::RespawnDelay = 30;    //No respawn delay set in spoonbot.cs ?? Ok, then assume 30 seconds.
      }
      schedule("AI::spawnAdditionalBot(" @ %aiName @ ", " @ %team @ ", False);", $Spoonbot::RespawnDelay );
      $DoNotRespawnAI = 0;
     }
   }
   else
   {
    // just in case:
    dbecho( 2, "Non training callback called from Training" );
   }

} // new death666

   else
   {
	   
	 //  	messageall(1, "On Drone Killed Ran.");

	$BotCooldown = true;
	schedule("$BotCooldown=false;",2.0);
    %id = ai::GetId( %aiName );
	$Actions::Hunting[%id] = false;
	$Actions::Flying[%id] = false;
	$Actions::Speaking[%id] = false;
	$Actions::Grenades[%id] = false;
	$Actions::Beacons[%id] = false;
	$Actions::Weapons[%id] = false;
		$Actions::Passenger[%id] = false;
	Player::kill(%aiName);
 	$BotsArenaCount--;
//	messageall(1, "AI On Drone Killed.");
   }

}

//these AI function callbacks can be very useful!

function AI::onTargetDied(%aiName, %idNum)
{

if((String::findSubStr(%aiName, "Optimus_Prime") >= 0) || (String::findSubStr(%aiName, "Android18") >= 0) || (String::findSubStr(%aiName, "Bumblebee") >= 0) || (String::findSubStr(%aiName, "Commander_Data") >= 0) || (String::findSubStr(%aiName, "Megatron") >= 0) || (String::findSubStr(%aiName, "T_800") >= 0) || (String::findSubStr(%aiName, "Jetfire") >= 0) || (String::findSubStr(%aiName, "R2_D2") >= 0) || (String::findSubStr(%aiName, "Starscream") >= 0) || (String::findSubStr(%aiName, "Asimo") >= 0) || (String::findSubStr(%aiName, "Dinobot") >= 0) || (String::findSubStr(%aiName, "Hal_9000") >= 0) || (String::findSubStr(%aiName, "Sentinel_Prime") >= 0) || (String::findSubStr(%aiName, "Spot") >= 0))
{ 
// new Death666

   %aiId = BotFuncs::GetId(%aiName);
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
if (%aiId==0)
	return;
   %curTarget = %idNum;

   %team=Client::getTeam(%aiId);
    if ($Spoonbot::BotChat)
	{
           %chatdelay = floor(getRandom() * (10 - 0.1));
	   schedule("AI::RandomCheerMsg(" @ %aiName@ ", " @ %team @ ");", %chatdelay );
	}

//       %chance  = floor(getRandom() * (10-0.1));
 //      if (%chance > 2)
 //      {
//			BotFuncs::Animation(%aiId, %animation);
//       }

	if(floor(getrandom() * 100) < 15 )
	{
//		messageall(1, "Target Died Animation.");
		BotFuncs::Animation(%aiId, %animation);
	}

   // BotFuncs::DelAttackerFromAll(%aiId);
   $BotThink::Definitive_Attackpoint[%aiId] = "";
   $BotThink::ForcedOfftrack[%aiId] = True;

   if(%curTarget == -1)
   {
   	return;
   }
      
   $Spoonbot::BotStatus[%aiId] = "Idle";

} 
// new Death666

   else
   {
	// messageall(1, "AI On Target Died.");
   }

}                                 

function AI::onTargetLOSAcquired(%aiName, %idNum)
{

if((String::findSubStr(%aiName, "Optimus_Prime") >= 0) || (String::findSubStr(%aiName, "Android18") >= 0) || (String::findSubStr(%aiName, "Bumblebee") >= 0) || (String::findSubStr(%aiName, "Commander_Data") >= 0) || (String::findSubStr(%aiName, "Megatron") >= 0) || (String::findSubStr(%aiName, "T_800") >= 0) || (String::findSubStr(%aiName, "Jetfire") >= 0) || (String::findSubStr(%aiName, "R2_D2") >= 0) || (String::findSubStr(%aiName, "Starscream") >= 0) || (String::findSubStr(%aiName, "Asimo") >= 0) || (String::findSubStr(%aiName, "Dinobot") >= 0) || (String::findSubStr(%aiName, "Hal_9000") >= 0) || (String::findSubStr(%aiName, "Sentinel_Prime") >= 0) || (String::findSubStr(%aiName, "Spot") >= 0))
{ 
// new Death666



//Sometimes, switching teams make your own bots chase you like an enemy. This is very
//strange, since switching to Observer mode and THEN to an other team does NOT produce this error.
//This is a quick hack so bots won't continue hunting you if you're in the same team

%aiId = BotFuncs::GetId(%aiName);  
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }            
if (%aiId==0)
	return;
%aiTeam = Client::getTeam(%aiId);       
%targetTeam = Client::getTeam(%idNum); 
if (%targetTeam != %aiTeam)
  {
//   dbecho(1, %aiName @ " just spotted an enemy");
   AI::HuntTarget(%aiName, %idNum, 1);
  }
  
  	   %aiId = BotFuncs::GetId(%aiName);  
		%player = Client::getOwnedObject(%aiId);
	   %player.cnt = 0;

} 
// new Death666

   else
   {
	   %aiId = BotFuncs::GetId(%aiName);  
		%player = Client::getOwnedObject(%aiId);
	   %player.cnt = 0;
	// messageall(1, "AI On Target Los Aquired.");
   }


}

function AI::onTargetLOSLost(%aiName, %idNum)
{

 if((String::findSubStr(%aiName, "Optimus_Prime") >= 0) || (String::findSubStr(%aiName, "Android18") >= 0) || (String::findSubStr(%aiName, "Bumblebee") >= 0) || (String::findSubStr(%aiName, "Commander_Data") >= 0) || (String::findSubStr(%aiName, "Megatron") >= 0) || (String::findSubStr(%aiName, "T_800") >= 0) || (String::findSubStr(%aiName, "Jetfire") >= 0) || (String::findSubStr(%aiName, "R2_D2") >= 0) || (String::findSubStr(%aiName, "Starscream") >= 0) || (String::findSubStr(%aiName, "Asimo") >= 0) || (String::findSubStr(%aiName, "Dinobot") >= 0) || (String::findSubStr(%aiName, "Hal_9000") >= 0) || (String::findSubStr(%aiName, "Sentinel_Prime") >= 0) || (String::findSubStr(%aiName, "Spot") >= 0))
{ 
// new Death666

%aiId = BotFuncs::GetId(%aiName);
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
if (%aiId==0)
	return;
%aiTeam = Client::getTeam(%aiId);
%targetTeam = Client::getTeam(%idNum);
if (%targetTeam != %aiTeam)
  {
//   dbecho(1, %aiName @ " lost eye contact to enemy");
   AI::HuntTarget(%aiName, %idNum, 1);

if ($Spoonbot::BotJetting[%aiId] != 1)
  AI::JetSimulation(%aiId, 0);


  }


} 
// new Death666

   else
   {
	  // 	messageall(1, "AI On Target Los Lost.");
//         	%aiId = ai::GetId( %aiName );
//              %player = Client::getOwnedObject(%aiId);
//		%loc = gamebase::getposition(%player);

//   	 if(%loc == "0 0 0")
//  	{
//		ArenaMSG(0, "An arena bot got lost at zero.");
//		return;
//	}

//	GameBase::PlaySound(%player, shockExplosion, 0);
//	Player::Kill(%player);
//	Player::blowUp(%player);
//	messageall(1, "AI On Target Los LOST.");
   }

}

function AI::onTargetLOSRegained(%aiName, %idNum)
{

if((String::findSubStr(%aiName, "Optimus_Prime") >= 0) || (String::findSubStr(%aiName, "Android18") >= 0) || (String::findSubStr(%aiName, "Bumblebee") >= 0) || (String::findSubStr(%aiName, "Commander_Data") >= 0) || (String::findSubStr(%aiName, "Megatron") >= 0) || (String::findSubStr(%aiName, "T_800") >= 0) || (String::findSubStr(%aiName, "Jetfire") >= 0) || (String::findSubStr(%aiName, "R2_D2") >= 0) || (String::findSubStr(%aiName, "Starscream") >= 0) || (String::findSubStr(%aiName, "Asimo") >= 0) || (String::findSubStr(%aiName, "Dinobot") >= 0) || (String::findSubStr(%aiName, "Hal_9000") >= 0) || (String::findSubStr(%aiName, "Sentinel_Prime") >= 0) || (String::findSubStr(%aiName, "Spot") >= 0))
{ 
// new Death666


%aiId = BotFuncs::GetId(%aiName);
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
if (%aiId==0)
	return;
%aiTeam = Client::getTeam(%aiId);
%targetTeam = Client::getTeam(%idNum);
if (%targetTeam != %aiTeam)
  {
//   dbecho(1, %aiName @ " found the enemy again");
   AI::HuntTarget(%aiName, %idNum, 1);
   $SPOONBOT::AbortAIJet = %aiId;
  }

	   %aiId = BotFuncs::GetId(%aiName);  
		%player = Client::getOwnedObject(%aiId);
	   %player.cnt = 0;

} 
// new Death666

   else
   {
	//
	%aiId = BotFuncs::GetId(%aiName);  
		%player = Client::getOwnedObject(%aiId);
	%player.cnt = 0;
		// messageall(1, "AI On Target Los Regained.");
   }

}




function AI::HuntTarget(%aiName, %idNum, %Follow) //If %Follow is 0 then waypoint will be updated. If 1 then Bot will follow regardless of LOS.
{
   %aiId = BotFuncs::GetId(%aiName);
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
if (%aiId==0)
	return;
   %curTarget = %idNum;

   if(%curTarget == -1)
   {
   	return;
   }
      
//   dbecho(1, %aiName @ " target: " @ %curTarget);	
   
   %targLoc = GameBase::getPosition(Client::getOwnedObject(%curTarget));
   %aiLoc = GameBase::getPosition(Client::getOwnedObject(%aiId));
   %targetDist = Vector::getDistance(%aiLoc, %targLoc);
//   dbecho(2, "distance to target: " @ %targetDist @ " targetPosition: " @ targLoc @ " aiLocation: " @ %aiLoc);

		if((String::findSubStr(%aiName, "Optimus_Prime") >= 0) || (String::findSubStr(%aiName, "Android18") >= 0) || (String::findSubStr(%aiName, "Bumblebee") >= 0) || (String::findSubStr(%aiName, "Commander_Data") >= 0) || (String::findSubStr(%aiName, "Megatron") >= 0) || (String::findSubStr(%aiName, "T_800") >= 0) || (String::findSubStr(%aiName, "Jetfire") >= 0) || (String::findSubStr(%aiName, "R2_D2") >= 0) || (String::findSubStr(%aiName, "Starscream") >= 0) || (String::findSubStr(%aiName, "Asimo") >= 0) || (String::findSubStr(%aiName, "Dinobot") >= 0) || (String::findSubStr(%aiName, "Hal_9000") >= 0) || (String::findSubStr(%aiName, "Sentinel_Prime") >= 0) || (String::findSubStr(%aiName, "Spot") >= 0))
		{
			   if(String::findSubStr(%aiName, "CMD") == 0)
				{
					AI::SmartFollow (%aiName, %idNum, %targLoc, 0, %targetDist, 80, 2);
				}
				else
				{
					AI::SmartStayAway (%aiName, %idNum, %targLoc, 0, %targetDist, 80, 2);
				}
		}
}

//This is for "smart" chasing/attack of enemies.
//If %Follow = 0 the AI will try to keep a minimum distance of %mintargetDist. If the distance is greater, it will come closer.
//If %Follow = 1 then the AI will follow the target %idNum to oblivion
//%order is the order number of the directive, so you can have orders OVERWRITE each other instead of stacking

function AI::SmartFollow (%aiName, %idNum, %targLoc, %Follow, %targetDist, %mintargetDist, %order)
{
			%aiId = BotFuncs::GetId(%aiName);
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
   if (%Follow == 0)
    {
      if(%targetDist > %minTargetDist)
        {
          AI::DirectiveWaypoint( %aiName, %targLoc, %order, 0 ); 

        }
        else
        {
//          AI::DirectiveRemove( %aiName, %order);
        }

    }
    else
    {
      if(%targetDist > %minTargetDist)
        {
	  AI::DirectiveFollow( %aiName, %idNum, 0, %order );

        }
        else
        {
//          AI::DirectiveRemove( %aiName, %order);
        }
    }
}

//This is for "smart" chasing/attack of enemies.
//The AI will evade slightly until distance to enemy is < %mintargetDist.
//%order is the order number of the directive, so you can have orders OVERWRITE each other instead of stacking

function AI::SmartStayAway (%aiName, %idNum, %targLoc, %Follow, %targetDist, %mintargetDist, %order)
{
			%aiId = BotFuncs::GetId(%aiName);
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
   if (%Follow == 0)
    {
      if(%targetDist < %minTargetDist)
        {
          AI::DirectiveWaypoint( %aiName, %targLoc, %order, 0 ); 
        }
        else
        {
//          AI::DirectiveRemove( %aiName, %order);
        }
    }
    else
    {
      if(%targetDist < %minTargetDist)
        {
    	  AI::DirectiveFollow( %aiName, %idNum, 0, %order );     
        }
        else
        {
//          AI::DirectiveRemove( %aiName, %order);
        }
    }

}




function AI::pickRandomSpawn(%team)
{
   %group = nameToID("MissionGroup/Teams/team" @ %team @ "/DropPoints/Random");
   %count = Group::objectCount(%group);
   if(!%count)
      return -1;

   %spawnIdx = floor(getRandom() * (%count - 0.1));
   %value = %count;
   for(%i = %spawnIdx; %i < %value; %i++) {
      %set = newObject("set",SimSet);
      %obj = Group::getObject(%group, %i);
      if(containerBoxFillSet(%set,$SimPlayerObjectType|$VehicleObjectType,GameBase::getPosition(%obj),2,2,4,0) == 0) 
         return %obj;
      if(%i == %count - 1) {
         dbecho(1, "pickRandomSpawn error: You forgot to set Random Drop points in your map!");
         %i = -1;
         %value = %spawnIdx;
      }
      deleteObject(%set);
   }
   return false;
}

//The following Callback isn't called - only God knows why..

function AI::onCollision (%aiId, %object)
{
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }

   %targLoc = GameBase::getPosition(Client::getOwnedObject(%object));
   %aiLoc = GameBase::getPosition(Client::getOwnedObject(%aiId));
   %aiRotation = GameBase::GetRotation(Client::getOwnedObject(%aiId)); 

   dbecho(1, "obstacle location" @ %targLoc);
   dbecho(1, "ai location" @ %aiLoc);
   dbecho(1, "ai rotation" @ %aiLoc);

   dbecho(1, "onCollision called in ai.cs");
   Vehicle::passengerJump(0,%aiId,0);  //Crude way to make players avoid obstacles
   Vehicle::passengerJump(0,%object,0);


}









function AI::DeployItem(%aiId,%desc,%validloc2)  
{
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
	// new jump here on any deploy 
	schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);

// List of deployable Items:
//--------------------------
//  DeployableAmmoPack
//  DeployableInvPack
//  TurretPack
//  CameraPack
//  DeployableSensorJammerPack
//  PulseSensorPack
//  MotionSensorPack
//  Beacon
//??mineammo??


        %item = %desc;
	%player = %aiId;
	%validloc = %validloc2;

//echo(1, "Item description: \"" @ %desc @ "\"");
//echo(1, "item \"" @ %item @ "\"");

	// --------------------------------------
	// This doesn't work, so we have to do it manually.
	// --------------------------------------
	// Player::setItemCount(%aiId, %item, 1);
	// Player::deployItem(%aiId,%item);
	// --------------------------------------

        if (%item == "DeployableInvPack")
            {
                %client = Player::getClient(%aiId);
                %inv = newObject("ammounit_remote","StaticShape","DeployableInvStation",true);
               // addToSet("MissionCleanup", %inv);
			    	addToSet("MissionCleanup/deployed/station", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,%name);
//				Client::sendMessage(%client,0,"Inventory Station deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "DeployableInvPack"]++;
//				echo("MSG: ",%client," deployed an Inventory Station");
            }


//        if (%item == "DeployableAmmoPack")
//            {
//                %client = Player::getClient(%aiId);
//                %inv = newObject("ammounit_remote","StaticShape","DeployableAmmoStation",true);
//                addToSet("MissionCleanup", %inv);
//				%rot = GameBase::getRotation(%aiId); 
//				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
//				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
//				GameBase::setRotation(%inv,%rot);
//				Gamebase::setMapName(%inv,%name);
//				Client::sendMessage(%client,0,"Ammo Station deployed");
//				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
//				$TeamItemCount[GameBase::getTeam(%inv) @ "DeployableAmmoPack"]++;
//				echo("MSG: ",%client," deployed an Ammo Station");
//            }

        if (%item == "MotionSensorPack")
            {
                %client = Player::getClient(%aiId);
                %inv = newObject("","Sensor","DeployableMotionSensor",true);
				addToSet("MissionCleanup/deployed/station", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,"Motion Sensor");
//				Client::sendMessage(%client,0,"Motion Sensor deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "MotionSensorPack"]++;
//				echo("MSG: ",%client," deployed a Motion Sensor");
            }
			
		if (%item == "LaserTurretPack")
            {
				%client = Player::getClient(%aiId);

				if(GameBase::getLOSInfo(%player,1.0))
				{
					%deployPosition = $los::position;
				}
				
				%deployPosition = $los::position;
				 %herplaser = "LaserTurretPack";
				 %item = %herplaser;

				if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]  && !$build) 
				{
					return false;
				}

				%obj = $los::object;			
				if(%obj.inmotion == true)	 
				{ 
					return false;
				}
				%set = newObject("set",SimSet);
				%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMaxLength,$TurretBoxMaxWidth,$TurretBoxMaxHeight,0);
				%num = CountTurrets(%set, %num);
				deleteObject(%set);
				if(%num > $MaxNumTurretsInBox) 
				{
					return;
				}

				%set = newObject("set",SimSet);
				%Mask = $StaticObjectType;
				%num = containerBoxFillSet(%set, $StaticObjectType, $los::position, $TurretBoxMinLength, $TurretBoxMinWidth, $TurretBoxMinHeight, 0);
				%num = CountObjects(%set, "DeployableLaserTurret", %num);
				deleteObject(%set);
				if(%num) 
				{	
					return;
				}

				//		if(Vector::dot($los::normal,"0 0 1") <= 0.7)
				//			{
				//				messageAll(0, "Can only deploy on flat surfaces.~wCapturedTower.wav");
				//				return false;
				//			}

				if(!checkInvDeployArea(%client,$los::position))
				{
					return false;
				}

				%obj = getObjectType($los::object);
				//		if(%obj != "SimTerrain" || %obj != "InteriorShape" || GameBase::getDataName($los::object) != "DeployablePlatform")	 
				//			{ 
				//				return false;
				//			}

				// 	%obj = getObjectType($los::object); allowing these to be set outside now -death666
				//	if(%obj != "InteriorShape")
				//	{
				//		Client::sendMessage(%client,0,"Can only deploy in buildings");
				//		return false;
				//	}

				if((GameBase::getTeam($los::object) != GameBase::getTeam(%player)) && (%obj != "SimTerrain") && (GameBase::getTeam($los::object) != -1)) 
				{
					return false;
				}

				if(!GameBase::testPosition(%player, vector::add($los::position, "0 0 1")) && %validloc == $TurretLocGroundOnly)
				{
					return false;
				}

                %inv = newObject("","Turret","DeployableLaserTurret",true);
                // addToSet("MissionCleanup", %inv);
				addToSet("MissionCleanup/deployed/turret", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,"Remote Laser Turret");
				// Client::sendMessage(%client,0,"Laser Turret deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "LaserTurretPack"]++;
//				echo("MSG: ",%client," deployed a Laser Turret");
            }
			
		if (%item == "LaserAndCrates")
            {
				%client = Player::getClient(%aiId);

				if(GameBase::getLOSInfo(%player,1.0))
				{
					%deployPosition = $los::position;
				}
				
				%deployPosition = $los::position;
				 %herplaser = "LaserTurretPack";
				 %item = %herplaser;

				if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]  && !$build) 
				{
					return false;
				}

				%obj = $los::object;			
				if(%obj.inmotion == true)	 
				{ 
					return false;
				}
				%set = newObject("set",SimSet);
				%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMaxLength,$TurretBoxMaxWidth,$TurretBoxMaxHeight,0);
				%num = CountTurrets(%set, %num);
				deleteObject(%set);
				if(%num > $MaxNumTurretsInBox) 
				{
					return;
				}

				%set = newObject("set",SimSet);
				%Mask = $StaticObjectType;
				%num = containerBoxFillSet(%set, $StaticObjectType, $los::position, $TurretBoxMinLength, $TurretBoxMinWidth, $TurretBoxMinHeight, 0);
				%num = CountObjects(%set, "DeployableLaserTurret", %num);
				deleteObject(%set);
				if(%num) 
				{	
					return;
				}

				//		if(Vector::dot($los::normal,"0 0 1") <= 0.7)
				//			{
				//				messageAll(0, "Can only deploy on flat surfaces.~wCapturedTower.wav");
				//				return false;
				//			}

				if(!checkInvDeployArea(%client,$los::position))
				{
					return false;
				}

				%obj = getObjectType($los::object);
				//		if(%obj != "SimTerrain" || %obj != "InteriorShape" || GameBase::getDataName($los::object) != "DeployablePlatform")	 
				//			{ 
				//				return false;
				//			}

				// 	%obj = getObjectType($los::object); allowing these to be set outside now -death666
				//	if(%obj != "InteriorShape")
				//	{
				//		Client::sendMessage(%client,0,"Can only deploy in buildings");
				//		return false;
				//	}

				if((GameBase::getTeam($los::object) != GameBase::getTeam(%player)) && (%obj != "SimTerrain") && (GameBase::getTeam($los::object) != -1)) 
				{
					return false;
				}

				if(!GameBase::testPosition(%player, vector::add($los::position, "0 0 1")) && %validloc == $TurretLocGroundOnly)
				{
					return false;
				}

                %inv = newObject("","Turret","DeployableLaserTurret",true);
                // addToSet("MissionCleanup", %inv);
								addToSet("MissionCleanup/deployed/turret", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,"Remote Laser Turret");
				// Client::sendMessage(%client,0,"Laser Turret deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "LaserTurretPack"]++;
//				echo("MSG: ",%client," deployed a Laser Turret");
//				schedule("BotMove::TwoBigCrates(" @ %aiId @ " );", 0.2);
				BotMove::TwoBigCrates(%aiId);
            }
			
		if (%item == "LaserAndPlatforms")
            {
				%client = Player::getClient(%aiId);

				if(GameBase::getLOSInfo(%player,1.0))
				{
					%deployPosition = $los::position;
				}
				
				%deployPosition = $los::position;
				 %herplaser = "LaserTurretPack";
				 %item = %herplaser;

				if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]  && !$build) 
				{
					return false;
				}

				%obj = $los::object;			
				if(%obj.inmotion == true)	 
				{ 
					return false;
				}
				%set = newObject("set",SimSet);
				%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMaxLength,$TurretBoxMaxWidth,$TurretBoxMaxHeight,0);
				%num = CountTurrets(%set, %num);
				deleteObject(%set);
				if(%num > $MaxNumTurretsInBox) 
				{
					return;
				}

				%set = newObject("set",SimSet);
				%Mask = $StaticObjectType;
				%num = containerBoxFillSet(%set, $StaticObjectType, $los::position, $TurretBoxMinLength, $TurretBoxMinWidth, $TurretBoxMinHeight, 0);
				%num = CountObjects(%set, "DeployableLaserTurret", %num);
				deleteObject(%set);
				if(%num) 
				{	
					return;
				}

				//		if(Vector::dot($los::normal,"0 0 1") <= 0.7)
				//			{
				//				messageAll(0, "Can only deploy on flat surfaces.~wCapturedTower.wav");
				//				return false;
				//			}

				if(!checkInvDeployArea(%client,$los::position))
				{
					return false;
				}

				%obj = getObjectType($los::object);
				//		if(%obj != "SimTerrain" || %obj != "InteriorShape" || GameBase::getDataName($los::object) != "DeployablePlatform")	 
				//			{ 
				//				return false;
				//			}

				// 	%obj = getObjectType($los::object); allowing these to be set outside now -death666
				//	if(%obj != "InteriorShape")
				//	{
				//		Client::sendMessage(%client,0,"Can only deploy in buildings");
				//		return false;
				//	}

				if((GameBase::getTeam($los::object) != GameBase::getTeam(%player)) && (%obj != "SimTerrain") && (GameBase::getTeam($los::object) != -1)) 
				{
					return false;
				}

				if(!GameBase::testPosition(%player, vector::add($los::position, "0 0 1")) && %validloc == $TurretLocGroundOnly)
				{
					return false;
				}

                %inv = newObject("","Turret","DeployableLaserTurret",true);
                // addToSet("MissionCleanup", %inv);
								addToSet("MissionCleanup/deployed/turret", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,"Remote Laser Turret");
				// Client::sendMessage(%client,0,"Laser Turret deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "LaserTurretPack"]++;
//				echo("MSG: ",%client," deployed a Laser Turret");
//				schedule("BotMove::ThreePlatforms(" @ %aiId @ " );", 0.2);
				BotMove::ThreePlatforms(%aiId);
            }
			
//        if (%item == "NeuroTurretPack")
//            {
//                %client = Player::getClient(%aiId);
//                %inv = newObject("","Turret","NeuroTurret",true);
//                addToSet("MissionCleanup", %inv);
//				%rot = GameBase::getRotation(%aiId); 
//				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
//				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
//				GameBase::setRotation(%inv,%rot);
//				Gamebase::setMapName(%inv,"Neuro Basher Turret");
//				// Client::sendMessage(%client,0,"Neuro Basher deployed");
//				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
//				$TeamItemCount[GameBase::getTeam(%inv) @ "NeuroTurretPack"]++;
//				echo("MSG: ",%client," deployed a Neuro Basher");
//          }

        if (%item == "vortexTurretPack")
            {
		%client = Player::getClient(%aiId);

	if(GameBase::getLOSInfo(%player,1.0))
	{
		%deployPosition = $los::position;
	}
	%deployPosition = $los::position;

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]  && !$build) 
	{
		return false;
	}

	%obj = $los::object;
	if(%obj.inmotion == true)	 
	{ 
		return false;
	}
		%set = newObject("set",SimSet);
		%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMaxLength,$TurretBoxMaxWidth,$TurretBoxMaxHeight,0);
		%num = CountTurrets(%set, %num);
		deleteObject(%set);
		if(%num > $MaxNumTurretsInBox) 
			{
				return;
			}

		%set = newObject("set",SimSet);
		%Mask = $StaticObjectType;
		%num = containerBoxFillSet(%set, $StaticObjectType, $los::position, $TurretBoxMinLength, $TurretBoxMinWidth, $TurretBoxMinHeight, 0);
		%num = CountObjects(%set, "DeployablevortexTurret", %num);
		deleteObject(%set);
		if(%num) 
			{	
				return;
			}

//		if(Vector::dot($los::normal,"0 0 1") <= 0.7)
//			{
//				messageAll(0, "Can only deploy on flat surfaces.~wCapturedTower.wav");
//				return false;
//			}

		if(!checkInvDeployArea(%client,$los::position))
			{
				return false;
			}

			%obj = getObjectType($los::object);
//		if(%obj != "SimTerrain" || %obj != "InteriorShape" || GameBase::getDataName($los::object) != "DeployablePlatform")	 
//			{ 
//				return false;
//			}

// 	%obj = getObjectType($los::object); allowing these to be set outside now -death666
//	if(%obj != "InteriorShape")
//	{
//		Client::sendMessage(%client,0,"Can only deploy in buildings");
//		return false;
//	}

		if((GameBase::getTeam($los::object) != GameBase::getTeam(%player)) && (%obj != "SimTerrain") && (GameBase::getTeam($los::object) != -1)) 
			{
				return false;
			}

		if(!GameBase::testPosition(%player, vector::add($los::position, "0 0 1")) && %validloc == $TurretLocGroundOnly)
			{
				return false;
			}

                %inv = newObject("","Turret","DeployablevortexTurret",true);
                // addToSet("MissionCleanup", %inv);
								addToSet("MissionCleanup/deployed/turret", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,"Vortex Turret");
				// Client::sendMessage(%client,0,"Vortex Turret deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "vortexTurretPack"]++;
//				echo("MSG: ",%client," deployed a Vortex Turret");
            }
			
		 if (%item == "vortexTurretCovered")
            {
		%client = Player::getClient(%aiId);

	if(GameBase::getLOSInfo(%player,1.0))
	{
		%deployPosition = $los::position;
	}
	%deployPosition = $los::position;
	
	%herplaser = "vortexTurretPack";
	%item = %herplaser;

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]  && !$build) 
	{
		return false;
	}

	%obj = $los::object;
	if(%obj.inmotion == true)	 
	{ 
		return false;
	}
		%set = newObject("set",SimSet);
		%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMaxLength,$TurretBoxMaxWidth,$TurretBoxMaxHeight,0);
		%num = CountTurrets(%set, %num);
		deleteObject(%set);
		if(%num > $MaxNumTurretsInBox) 
			{
				return;
			}

		%set = newObject("set",SimSet);
		%Mask = $StaticObjectType;
		%num = containerBoxFillSet(%set, $StaticObjectType, $los::position, $TurretBoxMinLength, $TurretBoxMinWidth, $TurretBoxMinHeight, 0);
		%num = CountObjects(%set, "DeployablevortexTurret", %num);
		deleteObject(%set);
		if(%num) 
			{	
				return;
			}

//		if(Vector::dot($los::normal,"0 0 1") <= 0.7)
//			{
//				messageAll(0, "Can only deploy on flat surfaces.~wCapturedTower.wav");
//				return false;
//			}

		if(!checkInvDeployArea(%client,$los::position))
			{
				return false;
			}

			%obj = getObjectType($los::object);
//		if(%obj != "SimTerrain" || %obj != "InteriorShape" || GameBase::getDataName($los::object) != "DeployablePlatform")	 
//			{ 
//				return false;
//			}

// 	%obj = getObjectType($los::object); allowing these to be set outside now -death666
//	if(%obj != "InteriorShape")
//	{
//		Client::sendMessage(%client,0,"Can only deploy in buildings");
//		return false;
//	}

		if((GameBase::getTeam($los::object) != GameBase::getTeam(%player)) && (%obj != "SimTerrain") && (GameBase::getTeam($los::object) != -1)) 
			{
				return false;
			}

		if(!GameBase::testPosition(%player, vector::add($los::position, "0 0 1")) && %validloc == $TurretLocGroundOnly)
			{
				return false;
			}

                %inv = newObject("","Turret","DeployablevortexTurret",true);
                // addToSet("MissionCleanup", %inv);
								addToSet("MissionCleanup/deployed/turret", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,"Vortex Turret");
				// Client::sendMessage(%client,0,"Vortex Turret deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "vortexTurretPack"]++;
//				schedule("BotMove::TwoBigCrates(" @ %aiId @ " );", 0.2);
				BotMove::TwoBigCrates(%aiId);
//				echo("MSG: ",%client," deployed a Vortex Turret");
            }

        if (%item == "NuclearTurretPack")
            {
		%client = Player::getClient(%aiId);

	if(GameBase::getLOSInfo(%player,1.0))
	{
		%deployPosition = $los::position;
	}
	%deployPosition = $los::position;

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]  && !$build) 
	{
		return false;
	}

	%obj = $los::object;
	if(%obj.inmotion == true)	 
	{ 
		return false;
	}
		%set = newObject("set",SimSet);
		%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMaxLength,$TurretBoxMaxWidth,$TurretBoxMaxHeight,0);
		%num = CountTurrets(%set, %num);
		deleteObject(%set);
		if(%num > $MaxNumTurretsInBox) 
			{
				return;
			}

		%set = newObject("set",SimSet);
		%Mask = $StaticObjectType;
		%num = containerBoxFillSet(%set, $StaticObjectType, $los::position, $TurretBoxMinLength, $TurretBoxMinWidth, $TurretBoxMinHeight, 0);
		%num = CountObjects(%set, "DeployableNuclearTurret", %num);
		deleteObject(%set);
		if(%num) 
			{	
				return;
			}

//		if(Vector::dot($los::normal,"0 0 1") <= 0.7)
//			{
//				messageAll(0, "Can only deploy on flat surfaces.~wCapturedTower.wav");
//				return false;
//			}

		if(!checkInvDeployArea(%client,$los::position))
			{
				return false;
			}

			%obj = getObjectType($los::object);
//		if(%obj != "SimTerrain" || %obj != "InteriorShape" || GameBase::getDataName($los::object) != "DeployablePlatform")	 
//			{ 
//				return false;
//			}

// 	%obj = getObjectType($los::object); allowing these to be set outside now -death666
//	if(%obj != "InteriorShape")
//	{
//		Client::sendMessage(%client,0,"Can only deploy in buildings");
//		return false;
//	}

		if((GameBase::getTeam($los::object) != GameBase::getTeam(%player)) && (%obj != "SimTerrain") && (GameBase::getTeam($los::object) != -1)) 
			{
				return false;
			}

		if(!GameBase::testPosition(%player, vector::add($los::position, "0 0 1")) && %validloc == $TurretLocGroundOnly)
			{
				return false;
			}

                %inv = newObject("","Turret","DeployableNuclearTurret",true);
                // addToSet("MissionCleanup", %inv);
								addToSet("MissionCleanup/deployed/turret", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,"Nuclear Turret");
				// Client::sendMessage(%client,0,"Nuclear Turret deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "NuclearTurretPack"]++;
//				echo("MSG: ",%client," deployed a Nuclear Turret");
            }

        if (%item == "FlameTurretPack")
            {
		%client = Player::getClient(%aiId);

	if(GameBase::getLOSInfo(%player,1.0))
	{
		%deployPosition = $los::position;
	}
	%deployPosition = $los::position;

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]  && !$build) 
	{
		return false;
	}

	%obj = $los::object;
	if(%obj.inmotion == true)	 
	{ 
		return false;
	}
		%set = newObject("set",SimSet);
		%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMaxLength,$TurretBoxMaxWidth,$TurretBoxMaxHeight,0);
		%num = CountTurrets(%set, %num);
		deleteObject(%set);
		if(%num > $MaxNumTurretsInBox) 
			{
				return;
			}

		%set = newObject("set",SimSet);
		%Mask = $StaticObjectType;
		%num = containerBoxFillSet(%set, $StaticObjectType, $los::position, $TurretBoxMinLength, $TurretBoxMinWidth, $TurretBoxMinHeight, 0);
		%num = CountObjects(%set, "FlameTurretPack", %num);
		deleteObject(%set);
		if(%num) 
			{	
				return;
			}

//		if(Vector::dot($los::normal,"0 0 1") <= 0.7)
//			{
//				messageAll(0, "Can only deploy on flat surfaces.~wCapturedTower.wav");
//				return false;
//			}

		if(!checkInvDeployArea(%client,$los::position))
			{
				return false;
			}

			%obj = getObjectType($los::object);
//		if(%obj != "SimTerrain" || %obj != "InteriorShape" || GameBase::getDataName($los::object) != "DeployablePlatform")	 
//			{ 
//				return false;
//			}

// 	%obj = getObjectType($los::object); allowing these to be set outside now -death666
//	if(%obj != "InteriorShape")
//	{
//		Client::sendMessage(%client,0,"Can only deploy in buildings");
//		return false;
//	}

		if((GameBase::getTeam($los::object) != GameBase::getTeam(%player)) && (%obj != "SimTerrain") && (GameBase::getTeam($los::object) != -1)) 
			{
				return false;
			}

		if(!GameBase::testPosition(%player, vector::add($los::position, "0 0 1")) && %validloc == $TurretLocGroundOnly)
			{
				return false;
			}

                %inv = newObject("","Turret","FlameTurret",true);
                // addToSet("MissionCleanup", %inv);
								addToSet("MissionCleanup/deployed/turret", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,"Flame Turret");
				// Client::sendMessage(%client,0,"Flame Turret deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "FlameTurretPack"]++;
//				echo("MSG: ",%client," deployed a Flame Turret");
            }
			
		 if (%item == "FlameTurretCovered")
            {
		%client = Player::getClient(%aiId);

	if(GameBase::getLOSInfo(%player,1.0))
	{
		%deployPosition = $los::position;
	}
	%deployPosition = $los::position;
	
	%herplaser = "FlameTurretPack";
	%item = %herplaser;

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]  && !$build) 
	{
		return false;
	}

	%obj = $los::object;
	if(%obj.inmotion == true)	 
	{ 
		return false;
	}
		%set = newObject("set",SimSet);
		%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMaxLength,$TurretBoxMaxWidth,$TurretBoxMaxHeight,0);
		%num = CountTurrets(%set, %num);
		deleteObject(%set);
		if(%num > $MaxNumTurretsInBox) 
			{
				return;
			}

		%set = newObject("set",SimSet);
		%Mask = $StaticObjectType;
		%num = containerBoxFillSet(%set, $StaticObjectType, $los::position, $TurretBoxMinLength, $TurretBoxMinWidth, $TurretBoxMinHeight, 0);
		%num = CountObjects(%set, "FlameTurretPack", %num);
		deleteObject(%set);
		if(%num) 
			{	
				return;
			}

//		if(Vector::dot($los::normal,"0 0 1") <= 0.7)
//			{
//				messageAll(0, "Can only deploy on flat surfaces.~wCapturedTower.wav");
//				return false;
//			}

		if(!checkInvDeployArea(%client,$los::position))
			{
				return false;
			}

			%obj = getObjectType($los::object);
//		if(%obj != "SimTerrain" || %obj != "InteriorShape" || GameBase::getDataName($los::object) != "DeployablePlatform")	 
//			{ 
//				return false;
//			}

// 	%obj = getObjectType($los::object); allowing these to be set outside now -death666
//	if(%obj != "InteriorShape")
//	{
//		Client::sendMessage(%client,0,"Can only deploy in buildings");
//		return false;
//	}

		if((GameBase::getTeam($los::object) != GameBase::getTeam(%player)) && (%obj != "SimTerrain") && (GameBase::getTeam($los::object) != -1)) 
			{
				return false;
			}

		if(!GameBase::testPosition(%player, vector::add($los::position, "0 0 1")) && %validloc == $TurretLocGroundOnly)
			{
				return false;
			}

                %inv = newObject("","Turret","FlameTurret",true);
                // addToSet("MissionCleanup", %inv);
								addToSet("MissionCleanup/deployed/turret", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,"Flame Turret");
				// Client::sendMessage(%client,0,"Flame Turret deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "FlameTurretPack"]++;
				// schedule("BotMove::TwoBigCrates(" @ %aiId @ " );", 0.1);
				BotMove::TwoBigCrates(%aiId);
//				echo("MSG: ",%client," deployed a Flame Turret");
            }

        if (%item == "DeployableTurret")
            {
		%client = Player::getClient(%aiId);

	if(GameBase::getLOSInfo(%player,1.0))
	{
		%deployPosition = $los::position;
	}
	%deployPosition = $los::position;
	
	%herpderpionturrets = "TurretPack";

	if($TeamItemCount[GameBase::getTeam(%player) @ %herpderpionturrets] >= $TeamItemMax[%herpderpionturrets]  && !$build) 
	{
		return false;
	}

	%obj = $los::object;
	if(%obj.inmotion == true)	 
	{ 
		return false;
	}
		%set = newObject("set",SimSet);
		%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMaxLength,$TurretBoxMaxWidth,$TurretBoxMaxHeight,0);
		%num = CountTurrets(%set, %num);
		deleteObject(%set);
		if(%num > $MaxNumTurretsInBox) 
			{
				return;
			}

		%set = newObject("set",SimSet);
		%Mask = $StaticObjectType;
		%num = containerBoxFillSet(%set, $StaticObjectType, $los::position, $TurretBoxMinLength, $TurretBoxMinWidth, $TurretBoxMinHeight, 0);
		%num = CountObjects(%set, "TurretPack", %num);
		deleteObject(%set);
		if(%num) 
			{	
				return;
			}

//		if(Vector::dot($los::normal,"0 0 1") <= 0.7)
//			{
//				messageAll(0, "Can only deploy on flat surfaces.~wCapturedTower.wav");
//				return false;
//			}

		if(!checkInvDeployArea(%client,$los::position))
			{
				return false;
			}
			
			%obj = getObjectType($los::object);
//		if(%obj != "SimTerrain" || %obj != "InteriorShape" || GameBase::getDataName($los::object) != "DeployablePlatform")	 
//			{ 
//				return false;
//			}

// 	%obj = getObjectType($los::object); allowing these to be set outside now -death666
//	if(%obj != "InteriorShape")
//	{
//		Client::sendMessage(%client,0,"Can only deploy in buildings");
//		return false;
//	}

		if((GameBase::getTeam($los::object) != GameBase::getTeam(%player)) && (%obj != "SimTerrain") && (GameBase::getTeam($los::object) != -1)) 
			{
				return false;
			}
			

		if(!GameBase::testPosition(%player, vector::add($los::position, "0 0 1")) && %validloc == $TurretLocGroundOnly)
			{
				return false;
			}

                %inv = newObject("","Turret","DeployableTurret",true);
                // addToSet("MissionCleanup", %inv);
								addToSet("MissionCleanup/deployed/turret", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,"Ion Turret");
				// Client::sendMessage(%client,0,"Ion Turret deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "TurretPack"]++;
//				echo("MSG: ",%client," deployed a Ion Turret");
            }

        if (%item == "ForceFieldDoorPack")
            {
                %client = Player::getClient(%aiId);
                %inv = newObject("","StaticShape","ForceFieldDoorShape",true);
                // addToSet("MissionCleanup", %inv);
				addToSet("MissionCleanup/deployed/Barrier", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,"Force Field Door");
				// Client::sendMessage(%client,0,"ForceFieldDoor deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "ForceFieldDoorPack"]++;
//				echo("MSG: ",%client," deployed a ForceFieldDoor");
            }

        if (%item == "JumpPadPack")
            {
                %client = Player::getClient(%aiId);
                %inv = newObject("","StaticShape","JumpPad",true);
				addToSet("MissionCleanup/deployed/Barrier", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,"JumpPad");
				// Client::sendMessage(%client,0,"JumpPad deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "JumpPadPack"]++;
//				echo("MSG: ",%client," deployed a JumpPad");
            }
			
		if (%item == "BigCratePack")
            {
                %client = Player::getClient(%aiId);
                %inv = newObject("","StaticShape","BigCrate",true);
				addToSet("MissionCleanup/deployed/Barrier", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,"BigCrate");
				// Client::sendMessage(%client,0,"JumpPad deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "BigCratePack"]++;
//				echo("MSG: ",%client," deployed a BigCrate");
            }
			
			if (%item == "PlatformPack")
            {
                %client = Player::getClient(%aiId);
                %inv = newObject("","StaticShape","DeployablePlatform",true);
				addToSet("MissionCleanup/deployed/Barrier", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,"DeployablePlatform");
				// Client::sendMessage(%client,0,"DeployablePlatform deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "PlatformPack"]++;
				
//                %inv2 = newObject("","StaticShape","DeployablePlatform",true);
//                addToSet("MissionCleanup", %inv2);
//				GameBase::setTeam(%inv2,GameBase::getTeam(%aiId));
//				GameBase::setPosition(%inv2,GameBase::getPosition(%aiId));
//				GameBase::setRotation(%inv2,%rot);
//				Gamebase::setMapName(%inv2,"DeployablePlatform");
//				$TeamItemCount[GameBase::getTeam(%inv) @ "PlatformPack"]++;
            }
			
			if (%item == "PlatformPack2")
            {
                %client = Player::getClient(%aiId);
                %inv = newObject("","StaticShape","DeployablePlatform",true);
				addToSet("MissionCleanup/deployed/Barrier", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				%playerPos2 = Vector::add(GameBase::getPosition(%aiId), "0 0 0.4");
				GameBase::setPosition(%inv,%playerpos2);
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,"DeployablePlatform");
				// Client::sendMessage(%client,0,"DeployablePlatform deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "PlatformPack"]++;
				
				%inv2 = newObject("","StaticShape","DeployablePlatform",true);
				addToSet("MissionCleanup/deployed/Barrier", %inv2);
				GameBase::setTeam(%inv2,GameBase::getTeam(%aiId));
				%playerPos2 = Vector::add(GameBase::getPosition(%aiId), "0 0 0.4");
				GameBase::setPosition(%inv2,%playerpos2);
				GameBase::setRotation(%inv2,%rot);
				Gamebase::setMapName(%inv2,"DeployablePlatform");
				$TeamItemCount[GameBase::getTeam(%inv) @ "PlatformPack"]++;
            }

//        if (%item == "DeployableCat")
//            {
//                %client = Player::getClient(%aiId);
//                %inv = newObject("","Turret","DeployableCat",true);
//                addToSet("MissionCleanup", %inv);
//				%rot = GameBase::getRotation(%aiId); 
//				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
//				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
//				GameBase::setRotation(%inv,%rot);
//				Gamebase::setMapName(%inv,"DeployableCat");
//				// Client::sendMessage(%client,0,"DeployableCat deployed");
//				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
//				$TeamItemCount[GameBase::getTeam(%inv) @ "CatPack"]++;
//				echo("MSG: ",%client," deployed a PussyCat");
//          }

//        if (%item == "MobileInventoryPack")
//            {
//                %client = Player::getClient(%aiId);
//                %inv = newObject("","StaticShape","MobileInventory",true);
//                addToSet("MissionCleanup", %inv);
//				%rot = GameBase::getRotation(%aiId); 
//				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
//				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
//				GameBase::setRotation(%inv,%rot);
//				Gamebase::setMapName(%inv,"MobileInventory");
//				// Client::sendMessage(%client,0,"MobileInventory deployed");
//				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
//				$TeamItemCount[GameBase::getTeam(%inv) @ "MobileInventoryPack"]++;
//				echo("MSG: ",%client," deployed a MobileInventory");
//          }

        if (%item == "IrradiationTurretPack")
            {
		%client = Player::getClient(%aiId);

	if(GameBase::getLOSInfo(%player,1.0))
	{
		%deployPosition = $los::position;
	}
	%deployPosition = $los::position;

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]  && !$build) 
	{
		return false;
	}

	%obj = $los::object;
	if(%obj.inmotion == true)	 
	{ 
		return false;
	}
		%set = newObject("set",SimSet);
		%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMaxLength,$TurretBoxMaxWidth,$TurretBoxMaxHeight,0);
		%num = CountTurrets(%set, %num);
		deleteObject(%set);
		if(%num > $MaxNumTurretsInBox) 
			{
				return;
			}

		%set = newObject("set",SimSet);
		%Mask = $StaticObjectType;
		%num = containerBoxFillSet(%set, $StaticObjectType, $los::position, $TurretBoxMinLength, $TurretBoxMinWidth, $TurretBoxMinHeight, 0);
		%num = CountObjects(%set, "IrradiationTurretPack", %num);
		deleteObject(%set);
		if(%num) 
			{	
				return;
			}

//		if(Vector::dot($los::normal,"0 0 1") <= 0.7)
//			{
//				messageAll(0, "Can only deploy on flat surfaces.~wCapturedTower.wav");
//				return false;
//			}

		if(!checkInvDeployArea(%client,$los::position))
			{
				return false;
			}

			%obj = getObjectType($los::object);
//		if(%obj != "SimTerrain" || %obj != "InteriorShape" || GameBase::getDataName($los::object) != "DeployablePlatform")	 
//			{ 
//				return false;
//			}

// 	%obj = getObjectType($los::object); allowing these to be set outside now -death666
//	if(%obj != "InteriorShape")
//	{
//		Client::sendMessage(%client,0,"Can only deploy in buildings");
//		return false;
//	}

		if((GameBase::getTeam($los::object) != GameBase::getTeam(%player)) && (%obj != "SimTerrain") && (GameBase::getTeam($los::object) != -1)) 
			{
				return false;
			}

		if(!GameBase::testPosition(%player, vector::add($los::position, "0 0 1")) && %validloc == $TurretLocGroundOnly)
			{
				return false;
			}

                %inv = newObject("","Turret","IrradiationTurret",true);
               // addToSet("MissionCleanup", %inv);
			   				addToSet("MissionCleanup/deployed/turret", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,"IrradiationTurret");
				// Client::sendMessage(%client,0,"IrradiationTurret deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "IrradiationTurretPack"]++;
//				echo("MSG: ",%client," deployed a IrradiationTurret");
            }

        if (%item == "FusionTurretPack")
            {
		%client = Player::getClient(%aiId);

	if(GameBase::getLOSInfo(%player,1.0))
	{
		%deployPosition = $los::position;
	}
	%deployPosition = $los::position;

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]  && !$build) 
	{
		return false;
	}

	%obj = $los::object;
	if(%obj.inmotion == true)	 
	{ 
		return false;
	}
		%set = newObject("set",SimSet);
		%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMaxLength,$TurretBoxMaxWidth,$TurretBoxMaxHeight,0);
		%num = CountTurrets(%set, %num);
		deleteObject(%set);
		if(%num > $MaxNumTurretsInBox) 
			{
				return;
			}

		%set = newObject("set",SimSet);
		%Mask = $StaticObjectType;
		%num = containerBoxFillSet(%set, $StaticObjectType, $los::position, $TurretBoxMinLength, $TurretBoxMinWidth, $TurretBoxMinHeight, 0);
		%num = CountObjects(%set, "FusionTurretPack", %num);
		deleteObject(%set);
		if(%num) 
			{	
				return;
			}

//		if(Vector::dot($los::normal,"0 0 1") <= 0.7)
//			{
//				messageAll(0, "Can only deploy on flat surfaces.~wCapturedTower.wav");
//				return false;
//			}

		if(!checkInvDeployArea(%client,$los::position))
			{
				return false;
			}

			%obj = getObjectType($los::object);
//		if(%obj != "SimTerrain" || %obj != "InteriorShape" || GameBase::getDataName($los::object) != "DeployablePlatform")	 
//			{ 
//				return false;
//			}

// 	%obj = getObjectType($los::object); allowing these to be set outside now -death666
//	if(%obj != "InteriorShape")
//	{
//		Client::sendMessage(%client,0,"Can only deploy in buildings");
//		return false;
//	}

		if((GameBase::getTeam($los::object) != GameBase::getTeam(%player)) && (%obj != "SimTerrain") && (GameBase::getTeam($los::object) != -1)) 
			{
				return false;
			}

		if(!GameBase::testPosition(%player, vector::add($los::position, "0 0 1")) && %validloc == $TurretLocGroundOnly)
			{
				return false;
			}

                %inv = newObject("","Turret","DeployableFusionTurret",true);
                // addToSet("MissionCleanup", %inv);
								addToSet("MissionCleanup/deployed/turret", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,"FusionTurret");
				// Client::sendMessage(%client,0,"FusionTurret deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "FusionTurretPack"]++;
//				echo("MSG: ",%client," deployed a FusionTurret");
            }

        if (%item == "PlasmaTurretPack")
            {
		%client = Player::getClient(%aiId);

	if(GameBase::getLOSInfo(%player,1.0))
	{
		%deployPosition = $los::position;
	}
	%deployPosition = $los::position;

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]  && !$build) 
	{
		return false;
	}

	%obj = $los::object;
	if(%obj.inmotion == true)	 
	{ 
		return false;
	}
		%set = newObject("set",SimSet);
		%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMaxLength,$TurretBoxMaxWidth,$TurretBoxMaxHeight,0);
		%num = CountTurrets(%set, %num);
		deleteObject(%set);
		if(%num > $MaxNumTurretsInBox) 
			{
				return;
			}

		%set = newObject("set",SimSet);
		%Mask = $StaticObjectType;
		%num = containerBoxFillSet(%set, $StaticObjectType, $los::position, $TurretBoxMinLength, $TurretBoxMinWidth, $TurretBoxMinHeight, 0);
		%num = CountObjects(%set, "PlasmaTurretPack", %num);
		deleteObject(%set);
		if(%num) 
			{	
				return;
			}

//		if(Vector::dot($los::normal,"0 0 1") <= 0.7)
//			{
//				messageAll(0, "Can only deploy on flat surfaces.~wCapturedTower.wav");
//				return false;
//			}

		if(!checkInvDeployArea(%client,$los::position))
			{
				return false;
			}

			%obj = getObjectType($los::object);
//		if(%obj != "SimTerrain" || %obj != "InteriorShape" || GameBase::getDataName($los::object) != "DeployablePlatform")	 
//			{ 
//				return false;
//			}

// 	%obj = getObjectType($los::object); allowing these to be set outside now -death666
//	if(%obj != "InteriorShape")
//	{
//		Client::sendMessage(%client,0,"Can only deploy in buildings");
//		return false;
//	}

		if((GameBase::getTeam($los::object) != GameBase::getTeam(%player)) && (%obj != "SimTerrain") && (GameBase::getTeam($los::object) != -1)) 
			{
				return false;
			}

		if(!GameBase::testPosition(%player, vector::add($los::position, "0 0 1")) && %validloc == $TurretLocGroundOnly)
			{
				return false;
			}

                %inv = newObject("","Turret","DeployablePlasmaTurret",true);
                // addToSet("MissionCleanup", %inv);
								addToSet("MissionCleanup/deployed/turret", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,"PlasmaTurret");
				// Client::sendMessage(%client,0,"PlasmaTurret deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "PlasmaTurretPack"]++;
//				echo("MSG: ",%client," deployed a PlasmaTurret");
            }

        if (%item == "ShockTurretPack")
            {
		%client = Player::getClient(%aiId);

	if(GameBase::getLOSInfo(%player,1.0))
	{
		%deployPosition = $los::position;
	}
	%deployPosition = $los::position;

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]  && !$build) 
	{
		return false;
	}

	%obj = $los::object;
	if(%obj.inmotion == true)	 
	{ 
		return false;
	}
		%set = newObject("set",SimSet);
		%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMaxLength,$TurretBoxMaxWidth,$TurretBoxMaxHeight,0);
		%num = CountTurrets(%set, %num);
		deleteObject(%set);
		if(%num > $MaxNumTurretsInBox) 
			{
				return;
			}

		%set = newObject("set",SimSet);
		%Mask = $StaticObjectType;
		%num = containerBoxFillSet(%set, $StaticObjectType, $los::position, $TurretBoxMinLength, $TurretBoxMinWidth, $TurretBoxMinHeight, 0);
		%num = CountObjects(%set, "DeployableShockTurret", %num);
		deleteObject(%set);
		if(%num) 
			{	
				return;
			}

//		if(Vector::dot($los::normal,"0 0 1") <= 0.7)
//			{
//				messageAll(0, "Can only deploy on flat surfaces.~wCapturedTower.wav");
//				return false;
//			}

		if(!checkInvDeployArea(%client,$los::position))
			{
				return false;
			}

			%obj = getObjectType($los::object);
//		if(%obj != "SimTerrain" || %obj != "InteriorShape" || GameBase::getDataName($los::object) != "DeployablePlatform")	 
//			{ 
//				return false;
//			}

// 	%obj = getObjectType($los::object); allowing these to be set outside now -death666
//	if(%obj != "InteriorShape")
//	{
//		Client::sendMessage(%client,0,"Can only deploy in buildings");
//		return false;
//	}

		if((GameBase::getTeam($los::object) != GameBase::getTeam(%player)) && (%obj != "SimTerrain") && (GameBase::getTeam($los::object) != -1)) 
			{
				return false;
			}

		if(!GameBase::testPosition(%player, vector::add($los::position, "0 0 1")) && %validloc == $TurretLocGroundOnly)
			{
				return false;
			}

                %inv = newObject("","Turret","DeployableShockTurret",true);
               // addToSet("MissionCleanup", %inv);
			   				addToSet("MissionCleanup/deployed/turret", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,"ShockTurret");
				// Client::sendMessage(%client,0,"ShockTurret deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "DeployableShockTurret"]++;
//				echo("MSG: ",%client," deployed a ShockTurret");
            }

        if (%item == "MortarTurretPack")
            {
		%client = Player::getClient(%aiId);

	if(GameBase::getLOSInfo(%player,1.0))
	{
		%deployPosition = $los::position;
	}
	%deployPosition = $los::position;

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]  && !$build) 
	{
		return false;
	}

	%obj = $los::object;
	if(%obj.inmotion == true)	 
	{ 
		return false;
	}
		%set = newObject("set",SimSet);
		%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMaxLength,$TurretBoxMaxWidth,$TurretBoxMaxHeight,0);
		%num = CountTurrets(%set, %num);
		deleteObject(%set);
		if(%num > $MaxNumTurretsInBox) 
			{
				return;
			}

		%set = newObject("set",SimSet);
		%Mask = $StaticObjectType;
		%num = containerBoxFillSet(%set, $StaticObjectType, $los::position, $TurretBoxMinLength, $TurretBoxMinWidth, $TurretBoxMinHeight, 0);
		%num = CountObjects(%set, "DeployableMortar", %num);
		deleteObject(%set);
		if(%num) 
			{	
				return;
			}

//		if(Vector::dot($los::normal,"0 0 1") <= 0.7)
//			{
//				messageAll(0, "Can only deploy on flat surfaces.~wCapturedTower.wav");
//				return false;
//			}

		if(!checkInvDeployArea(%client,$los::position))
			{
				return false;
			}

			%obj = getObjectType($los::object);
//		if(%obj != "SimTerrain" || %obj != "InteriorShape" || GameBase::getDataName($los::object) != "DeployablePlatform")	 
//			{ 
//				return false;
//			}

// 	%obj = getObjectType($los::object); allowing these to be set outside now -death666
//	if(%obj != "InteriorShape")
//	{
//		Client::sendMessage(%client,0,"Can only deploy in buildings");
//		return false;
//	}

		if((GameBase::getTeam($los::object) != GameBase::getTeam(%player)) && (%obj != "SimTerrain") && (GameBase::getTeam($los::object) != -1)) 
			{
				return false;
			}

		if(!GameBase::testPosition(%player, vector::add($los::position, "0 0 1")) && %validloc == $TurretLocGroundOnly)
			{
				return false;
			}

                %inv = newObject("","Turret","DeployableMortar",true);
                // addToSet("MissionCleanup", %inv);
								addToSet("MissionCleanup/deployed/turret", %inv);
				%rot = GameBase::getRotation(%aiId); 
				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
				GameBase::setRotation(%inv,%rot);
				Gamebase::setMapName(%inv,"MortarTurret");
				// Client::sendMessage(%client,0,"MortarTurret deployed");
				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
				$TeamItemCount[GameBase::getTeam(%inv) @ "DeployableMortar"]++;
//				echo("MSG: ",%client," deployed a MortarTurret");
            }

//        if (%item == "RocketPack")
//            {
//                %client = Player::getClient(%aiId);
//                %inv = newObject("","Turret","DeployableRocket",true);
//                addToSet("MissionCleanup", %inv);
//				%rot = GameBase::getRotation(%aiId); 
//				GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
//				GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
//				GameBase::setRotation(%inv,%rot);
//				Gamebase::setMapName(%inv,"RocketTurret");
//				// Client::sendMessage(%client,0,"RocketTurret deployed");
//				playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
//				$TeamItemCount[GameBase::getTeam(%inv) @ "DeployableRocket"]++;
//				echo("MSG: ",%client," deployed a RocketTurret");
//            }

	if (%item == "RocketPack")
{
		%client = Player::getClient(%aiId);

	if(GameBase::getLOSInfo(%player,1.0))
	{
		%deployPosition = $los::position;
	}
	%deployPosition = $los::position;

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]  && !$build) 
	{
		// messageAll(0, "Deployable Item limit reached for RocketTurrets.~wCapturedTower.wav");
		return false;
	}

	%obj = $los::object;
	if(%obj.inmotion == true)	 
	{ 
		// messageAll(0, "Deploy area crappy cannot deploy.~wCapturedTower.wav");
		return false;
	}
		%set = newObject("set",SimSet);
		%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMaxLength,$TurretBoxMaxWidth,$TurretBoxMaxHeight,0);
		%num = CountTurrets(%set, %num);
		deleteObject(%set);
		if(%num > $MaxNumTurretsInBox) 
			{
				// messageAll(0, "Interference from other remote turrets in the area.~wCapturedTower.wav");
				return;
			}

		%set = newObject("set",SimSet);
		%Mask = $StaticObjectType;
		%num = containerBoxFillSet(%set, $StaticObjectType, $los::position, $TurretBoxMinLength, $TurretBoxMinWidth, $TurretBoxMinHeight, 0);
		%num = CountObjects(%set, "DeployableRocket", %num);
		deleteObject(%set);
		if(%num) 
			{	
				// messageAll(0, "Too close to other remote turrets.~wCapturedTower.wav");
				return;
			}

//		if(Vector::dot($los::normal,"0 0 1") <= 0.7)
//			{
//				messageAll(0, "Can only deploy on flat surfaces.~wCapturedTower.wav");
//				return false;
//			}

		if(!checkInvDeployArea(%client,$los::position))
			{
				// messageAll(0, "Cannot deploy. Item in way.~wCapturedTower.wav");
				return false;
			}

			%obj = getObjectType($los::object);
//		if(%obj != "SimTerrain" || %obj != "InteriorShape" || GameBase::getDataName($los::object) != "DeployablePlatform")	 
//			{ 
//				return false;
//			}

// 	%obj = getObjectType($los::object); allowing these to be set outside now -death666
//	if(%obj != "InteriorShape")
//	{
//		Client::sendMessage(%client,0,"Can only deploy in buildings");
//		return false;
//	}

		if((GameBase::getTeam($los::object) != GameBase::getTeam(%player)) && (%obj != "SimTerrain") && (GameBase::getTeam($los::object) != -1)) 
			{
				// messageAll(0, "Cannot deploy on enemy base.~wCapturedTower.wav");
				return false;
			}

		if(!GameBase::testPosition(%player, vector::add($los::position, "0 0 1")) && %validloc == $TurretLocGroundOnly)
			{
				// messageAll(0, "Turret does not fit there.~wCapturedTower.wav");
				return false;
			}

		%rot = GameBase::getRotation(%aiId); 
                %inv = newObject("","Turret","DeployableRocket",true);
                // addToSet("MissionCleanup", %inv);
								addToSet("MissionCleanup/deployed/turret", %inv);
		GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
		GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
		GameBase::setRotation(%inv,%rot);
		// Gamebase::setMapName(%inv,"RocketTurret");
		Gamebase::setMapName(%inv,"RocketTurret " @ Client::getName(%client));
		// Client::sendMessage(%client,0,"RocketTurret deployed");
		playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
		$TeamItemCount[GameBase::getTeam(%inv) @ "RocketPack"]++;
//		echo("MSG: ",%client," deployed a RocketTurret");
		// messageAll(0, "RocketTurret Deployed.~wCapturedTower.wav");
}

        if (%item == "PulseSensorPack")
            {
                %client = Player::getClient(%aiId);
                %inv = newObject("","Sensor","DeployablePulseSensor",true);
				addToSet("MissionCleanup/deployed/Barrier", %inv);
		%rot = GameBase::getRotation(%aiId); 
		GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
		GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
		GameBase::setRotation(%inv,%rot);
		Gamebase::setMapName(%inv,"Pulse Sensor");
//		Client::sendMessage(%client,0,"Pulse Sensor deployed");
		playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
		$TeamItemCount[GameBase::getTeam(%inv) @ "PulseSensorPack"]++;
//		echo("MSG: ",%client," deployed a Pulse Sensor");
            }


        if (%item == "DeployableSensorJammerPack")
            {
                %client = Player::getClient(%aiId);
                %inv = newObject("","Sensor","DeployableSensorJammer",true);
				addToSet("MissionCleanup/deployed/Barrier", %inv);
		%rot = GameBase::getRotation(%aiId); 
		GameBase::setTeam(%inv,GameBase::getTeam(%aiId));
		GameBase::setPosition(%inv,GameBase::getPosition(%aiId));
		GameBase::setRotation(%inv,%rot);
		Gamebase::setMapName(%inv,"Sensor Jammer");
//		Client::sendMessage(%client,0,"Sensor Jammer deployed");
		playSound(SoundPickupBackpack, GameBase::getPosition(%aiId));
		$TeamItemCount[GameBase::getTeam(%inv) @ "DeployableSensorJammerPack"]++;
//		echo("MSG: ",%client," deployed a Sensor Jammer");
            }

        if (%item == "CameraPack")
            {
                %client = Player::getClient(%aiId);
		%camera = newObject("Camera","Turret",CameraTurret,true);
	   	// addToSet("MissionCleanup", %camera);
				addToSet("MissionCleanup/deployed/turret", %camera);
		GameBase::setTeam(%camera,GameBase::getTeam(%aiId));
		GameBase::setRotation(%camera,%rot);
		GameBase::setPosition(%camera,GameBase::getPosition(%aiId));
		Gamebase::setMapName(%camera,"Camera#"@ $totalNumCameras++ @ " " @ Client::getName(%client));
//		Client::sendMessage(%client,0,"Camera deployed");
		playSound(SoundPickupBackpack,GameBase::getPosition(%aiId));
		$TeamItemCount[GameBase::getTeam(%camera) @ "CameraPack"]++;
//		echo("MSG: ",%client," deployed a Camera");
            }


//        if (%item == "TurretPack")
//            {
//                %client = Player::getClient(%aiId);




//	if($TeamItemCount[GameBase::getTeam(%aiId) @ %item] < $TeamItemMax[%item]) {
//			%obj = getObjectType($los::object);
//	    		%set = newObject("set",SimSet);
//				%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMaxLength,$TurretBoxMaxWidth,$TurretBoxMaxHeight,0);
//				%num = CountObjects(%set,"DeployableTurret",%num);
//				deleteObject(%set);
//				if($MaxNumTurretsInBox > %num) {
//		    		%set = newObject("set",SimSet);
//					%num = containerBoxFillSet(%set,$StaticObjectType,$los::position,$TurretBoxMinLength,$TurretBoxMinWidth,$TurretBoxMinHeight,0);
//					%num = CountObjects(%set,"DeployableTurret",%num);
//					deleteObject(%set);
//					if(0 == %num) {
						







//                %rot = GameBase::getRotation(%aiId); 
//		%turret = newObject("remoteTurret","Turret",DeployableTurret,true);
//	        addToSet("MissionCleanup", %turret);
//		GameBase::setTeam(%turret,GameBase::getTeam(%aiId));
//		GameBase::setPosition(%turret,GameBase::getPosition(%aiId));
//		GameBase::setRotation(%turret,%rot);
//		Gamebase::setMapName(%turret,"RMT Turret#" @ $totalNumTurrets++ @ " " @ Client::getName(%client));
//		Client::sendMessage(%client,0,"Remote Turret deployed");
//		playSound(SoundPickupBackpack,GameBase::getPosition(%aiId));
//		$TeamItemCount[GameBase::getTeam(%aiId) @ "TurretPack"]++;
//		echo("MSG: ",%client," deployed a Remote Turret");



//					} 
//					else
//						Client::sendMessage(%client,0,"Frequency Overload - Too close to other remote turrets");
//				}
//			   else 
//					Client::sendMessage(%client,0,"Interference from other remote turrets in the area");
//	}
//	else																						  
//	 	Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
//
//	return false;








//        	}


//        if (%item == "Beacon")
//            {
//                %client = Player::getClient(%aiId);
//                %beacon = newObject("Target Beacon", "StaticShape", "DefaultBeacon", true);
//		addToSet("MissionCleanup", %beacon);
//		GameBase::setTeam(%beacon,GameBase::getTeam(%aiId));
//		GameBase::setRotation(%beacon,%rot);
//		GameBase::setPosition(%beacon,GameBase::getPosition(%aiId));
//		Gamebase::setMapName(%beacon,"Target Beacon");
//		Beacon::onEnabled(%beacon);
//		Client::sendMessage(%client,0,"Beacon deployed");
//		$TeamItemCount[GameBase::getTeam(%beacon) @ "Beacon"]++;
//            }

}

function AI::RemoveBot(%aiName, %commandissuer)
{
  %aiId = BotFuncs::GetId( %aiName );
if (%aiId==0)
	return;
  $DoNotRespawnAI = %aiId;
  Player::kill(%aiId);
}


function AI::buildGraph()
{
	echo("building AI Graph...");
   %nodeGroup = nameToID("MissionGroup\\AIGraph");
   %numNodes  = Group::ObjectCount(%nodeGroup);
   
   echo("nodeGroup: " @ %nodeGroup @ " number of Nodes: " @ %numNodes); 
   
   for(%i = 0; %i < %numNodes; %i++)
   {
   	%node    = Group::getObject(%nodeGroup, %i);
      %nodePos = GameBase::getPosition(%node);
      %name = "Node " @ %i;
      //echo("name of node:" @ %name);
      //echo("adding node: " @ %node @ " at position: " @ %nodePos);
      if(name != "")
      	Graph::AddNode(%nodePos, %name);
      else 
         Graph::AddNode(%nodePos);
   }
   if(Graph::buildGraph() == -1)
    	echo("Can't create adjacent lists for graph nodes.");
   else
   	echo("Graph build complete.");
}


//Render lines to show AI Graph
function drawGraph()
{
	newObject("graphRender", GraphPathRender);
}


function AI::RandomCheerMsg(%aiName, %team)
{
// %perChance = floor(getRandom() * (200 - 0.1));
// if (%perChance < 2) //10
 if(floor(getrandom() * 100) < 5 ) // 5
 {
	%aiId = BotFuncs::GetId( %aiName );
if (%aiId==0)
	return;
	%enemyTeam = 1;
	if (%team == 1)
	{
	  %enemyTeam = 0;
	}
	
	
//		%msgIdx = floor(getRandom() * (8 - 0.1));
		%msgIdx = floor(getRandom() * (16 - 0.1));
		%msgIdx1 = floor(getRandom() * (128 - 0.1));
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	  {
		Ai::soundHelper( %aiId, %cl, $vcheerList[%msgIdx] );
//				 Ai::soundHelper( 2051, 2049, cheer3 );
		Client::sendMessage(%cl, 2, %aiName @ ": " @ $cheerList[%msgIdx1], %aiId);
	  }
 }
}



function AI::RandomSuckMsg(%aiName, %team)
{
// %perChance = floor(getRandom() * (200 - 0.1)); // 10 - 0.1
// if (%perChance < 2 ) // => 8
 if(floor(getrandom() * 100) < 5 )
 {
	%aiId = BotFuncs::GetId( %aiName );
//if (%aiId==0)
//	return;
	%enemyTeam = 1;
	if (%team == 1)
	{
	  %enemyTeam = 0;
	}
	
		%msgIdx = floor(getRandom() * (8 - 0.1));
		%msgIdx1 = floor(getRandom() * (48 - 0.1));
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	  {
		Ai::soundHelper( %aiId, %cl, $vsuckList[%msgIdx] );
			Client::sendMessage(%cl, 2, %aiName @ ": " @ $suckList[%msgIdx1], %aiId);
	  }
	
 }
}


function AI::ProcessAutoSpawn()		// A *VERY* dirty way to process the spoonbot.cs file... I know ;-)
{
if (($Spoonbot::AutoSpawn) && (!$Spoonbot::BotTree_Design)) // Not more bots in design mode.
 {
	if ($Spoonbot::Bot1Name != "0")
		schedule("AI::spawnAdditionalBot(" @ $Spoonbot::Bot1Name @ ", " @ $Spoonbot::Bot1Team @ ", 0);",3.0); 
 	if ($Spoonbot::Bot2Name != "0")
		schedule("AI::spawnAdditionalBot(" @ $Spoonbot::Bot2Name @ ", " @ $Spoonbot::Bot2Team @ ", 0);",4.0); 
	if ($Spoonbot::Bot3Name != "0")
		schedule("AI::spawnAdditionalBot(" @ $Spoonbot::Bot3Name @ ", " @ $Spoonbot::Bot3Team @ ", 0);",5.0); 
	if ($Spoonbot::Bot4Name != "0")
		schedule("AI::spawnAdditionalBot(" @ $Spoonbot::Bot4Name @ ", " @ $Spoonbot::Bot4Team @ ", 0);",6.0); 
	if ($Spoonbot::Bot5Name != "0")
		schedule("AI::spawnAdditionalBot(" @ $Spoonbot::Bot5Name @ ", " @ $Spoonbot::Bot5Team @ ", 0);",7.0); 
	if ($Spoonbot::Bot6Name != "0")
		schedule("AI::spawnAdditionalBot(" @ $Spoonbot::Bot6Name @ ", " @ $Spoonbot::Bot6Team @ ", 0);",8.0); 
	if ($Spoonbot::Bot7Name != "0")
		schedule("AI::spawnAdditionalBot(" @ $Spoonbot::Bot7Name @ ", " @ $Spoonbot::Bot7Team @ ", 0);",9.0);
	if ($Spoonbot::Bot8Name != "0")
		schedule("AI::spawnAdditionalBot(" @ $Spoonbot::Bot8Name @ ", " @ $Spoonbot::Bot8Team @ ", 0);",10.0);
	if ($Spoonbot::Bot9Name != "0")
		schedule("AI::spawnAdditionalBot(" @ $Spoonbot::Bot9Name @ ", " @ $Spoonbot::Bot9Team @ ", 0);",11.0);
	if ($Spoonbot::Bot10Name != "0")
		schedule("AI::spawnAdditionalBot(" @ $Spoonbot::Bot10Name @ ", " @ $Spoonbot::Bot10Team @ ", 0);",12.0);
//	if ($Spoonbot::Bot11Name != "0")
//		schedule("AI::spawnAdditionalBot(" @ $Spoonbot::Bot11Name @ ", " @ $Spoonbot::Bot11Team @ ", 0);",16);
//	if ($Spoonbot::Bot12Name != "0")
//		schedule("AI::spawnAdditionalBot(" @ $Spoonbot::Bot12Name @ ", " @ $Spoonbot::Bot12Team @ ", 0);",17.5);
//	if ($Spoonbot::Bot13Name != "0")
//		schedule("AI::spawnAdditionalBot(" @ $Spoonbot::Bot13Name @ ", " @ $Spoonbot::Bot13Team @ ", 0);",19);
//	if ($Spoonbot::Bot14Name != "0")
//		schedule("AI::spawnAdditionalBot(" @ $Spoonbot::Bot14Name @ ", " @ $Spoonbot::Bot14Team @ ", 0);",20.5);
//	if ($Spoonbot::Bot15Name != "0")
//		schedule("AI::spawnAdditionalBot(" @ $Spoonbot::Bot15Name @ ", " @ $Spoonbot::Bot15Team @ ", 0);",22);
//	if ($Spoonbot::Bot16Name != "0")
//		schedule("AI::spawnAdditionalBot(" @ $Spoonbot::Bot16Name @ ", " @ $Spoonbot::Bot16Team @ ", 0);",23.5);
//	if ($Spoonbot::Bot17Name != "0")
//		schedule("AI::spawnAdditionalBot(" @ $Spoonbot::Bot17Name @ ", " @ $Spoonbot::Bot17Team @ ", 0);",25);
//	if ($Spoonbot::Bot18Name != "0")
//		schedule("AI::spawnAdditionalBot(" @ $Spoonbot::Bot18Name @ ", " @ $Spoonbot::Bot18Team @ ", 0);",26.5);
//	if ($Spoonbot::Bot19Name != "0")
//		schedule("AI::spawnAdditionalBot(" @ $Spoonbot::Bot19Name @ ", " @ $Spoonbot::Bot19Team @ ", 0);",28);
//	if ($Spoonbot::Bot20Name != "0")
//		schedule("AI::spawnAdditionalBot(" @ $Spoonbot::Bot20Name @ ", " @ $Spoonbot::Bot20Team @ ", 0);",29.5);

//	AI::spawnAdditionalBot($Spoonbot::Bot2Name, $Spoonbot::Bot2Team, 0);
 }
}

function AI::Jet(%aiId)
{
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
	%passenger = %aiId;
	%armor = Player::getArmor(%passenger);

	if(%armor == "larmor" || %armor == "lfemale") {
		%height = 15;
		%velocity = 70;
		%zVec = 40;
	}
	else if(%armor == "marmor" || %armor == "mfemale") {
		%height = 19;
		%velocity = 100;
		%zVec = 60;
	}
	else if(%armor == "harmor") {
		%height = 22;
		%velocity = 140;
		%zVec = 90;
	}

	%pos = GameBase::getPosition(%passenger);
	%posX = getWord(%pos,0);
	%posY	= getWord(%pos,1);
	%posZ	= getWord(%pos,2);

	if(GameBase::testPosition(%passenger,%posX @ " " @ %posY @ " " @ (%posZ + %height))) {	
		%rotZ = getWord(GameBase::getRotation(%passenger),2);
		GameBase::setRotation(%passenger, "0 0 " @ %rotZ);
		GameBase::setPosition(%passenger,%posX @ " " @ %posY @ " " @ (%posZ + %height));
		%jumpDir = Vector::getFromRot(GameBase::getRotation(%passenger),%velocity,%zVec);
		Player::applyImpulse(%passenger,%jumpDir);
	}

}

function AI::Jump(%aiId)      //This function makes the AI jump. If %jet=1 then it Calls the Jetpack Routine
{
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
    %passenger = %aiId;
	%armor = Player::getArmor(%passenger);
	if(%armor == "larmor" || %armor == "lfemale") {
		%height = 2;
		%velocity = 70;
		%zVec = 70;
	}
	else if(%armor == "marmor" || %armor == "mfemale") {
		%height = 2;
		%velocity = 100;
		%zVec = 100;
	}
	else if(%armor == "harmor") {
		%height = 2;
		%velocity = 140;
		%zVec = 110;
	}

	%pos = GameBase::getPosition(%passenger);
	%posX = getWord(%pos,0);
	%posY	= getWord(%pos,1);
	%posZ	= getWord(%pos,2);

	if(GameBase::testPosition(%passenger,%posX @ " " @ %posY @ " " @ (%posZ + %height))) {	
		%rotZ = getWord(GameBase::getRotation(%passenger),2);
		GameBase::setRotation(%passenger, "0 0 " @ %rotZ);
		GameBase::setPosition(%passenger,%posX @ " " @ %posY @ " " @ (%posZ + %height));
		%jumpDir = Vector::getFromRot(GameBase::getRotation(%passenger),%velocity,%zVec);
		Player::applyImpulse(%passenger,%jumpDir);
	}
}

function AI::shove(%aiId, %velocity, %zVec, %rotX, %rotY, %rotZ)
{
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
		%passenger = %aiId;
		%OldrotX = getWord(GameBase::getRotation(%passenger),0);
		%OldrotY = getWord(GameBase::getRotation(%passenger),1);
		%OldrotZ = getWord(GameBase::getRotation(%passenger),2);
		%rotation = (%OldrotX + %rotX) @ " " @ (%OldrotY + %rotY) @ " " @ (%OldrotZ + %rotZ);
		GameBase::setRotation(%passenger, %rotation);
		%jumpDir = Vector::getFromRot(%rotation, %velocity, %zVec);
		Player::applyImpulse(%passenger,%jumpDir);
}

function AI::Boost(%aiId)
{
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
	
    if(%loc == "0 0 0")
    {
	return;
    }
	
	%aiName = Client::GetName(%aiId);
	if(BotTypes::IsGuard(%aiName) == 1)
	{
		 return;			 
	}
	
	if(BotTypes::IsMedic(%aiName) == 1)
	{
		return;			 
	}
	
	%client = Player::GetClient(%aiId);	 	 	 	 
	%armor = player::getarmor(%client);
	%armorlist = $ArmorName[%armor].description;

	if(%armorlist == "Troll")
	{
			if(floor(getrandom() * 100) < 75 )
	{
		%switch = floor(getRandom() * 2);
		if(%switch == 0)
		{
			%velocity = 180;
			%zVec = 180;
			schedule("AI::shove(" @ %aiId @ ", " @ %velocity @ ", " @ %zVec @ ", 0, 0, 0);", 1);
		}
		else if(%switch == 1)
		{
			%velocity = 165;
			%zVec = 165;
			schedule("AI::shove(" @ %aiId @ ", " @ %velocity @ ", " @ %zVec @ ", 0, 0, 0);", 1);			
		}
	}
		return;
	}
	else if(%armorlist == "Titan")
	{
			if(floor(getrandom() * 100) < 75 )
	{
		%switch = floor(getRandom() * 2);
		if(%switch == 0)
		{
		%velocity = 180;
		%zVec = 180;
		schedule("AI::shove(" @ %aiId @ ", " @ %velocity @ ", " @ %zVec @ ", 0, 0, 0);", 1);
		}
		else if(%switch == 1)
		{
			%velocity = 165;
			%zVec = 165;
			schedule("AI::shove(" @ %aiId @ ", " @ %velocity @ ", " @ %zVec @ ", 0, 0, 0);", 1);			
		}
	}
		return;
	}
	else if(%armorlist == "Tank")
	{
			if(floor(getrandom() * 100) < 75 )
	{
		%switch = floor(getRandom() * 2);
		if(%switch == 0)
		{
		%velocity = 180;
		%zVec = 180;
		schedule("AI::shove(" @ %aiId @ ", " @ %velocity @ ", " @ %zVec @ ", 0, 0, 0);", 1);
		}
		else if(%switch == 1)
		{
			%velocity = 165;
			%zVec = 165;
			schedule("AI::shove(" @ %aiId @ ", " @ %velocity @ ", " @ %zVec @ ", 0, 0, 0);", 1);
		}			
		
	}
		return;
	}
	else if(%armorlist == "Warrior")
	{
			if(floor(getrandom() * 100) < 75 )
	{
		%switch = floor(getRandom() * 2);
		if(%switch == 0)
		{
		%velocity = 120;
		%zVec = 120;
		schedule("AI::shove(" @ %aiId @ ", " @ %velocity @ ", " @ %zVec @ ", 0, 0, 0);", 1);
		}
		else if(%switch == 1)
		{
			%velocity = 105;
			%zVec = 105;
			schedule("AI::shove(" @ %aiId @ ", " @ %velocity @ ", " @ %zVec @ ", 0, 0, 0);", 1);
		}
	}
		return;
	}
	else if(%armorlist == "Chameleon Assassin")
	{
			if(floor(getrandom() * 100) < 75 )
	{
		%switch = floor(getRandom() * 2);
		if(%switch == 0)
		{
		%velocity = 100;
		%zVec = 100;
		schedule("AI::shove(" @ %aiId @ ", " @ %velocity @ ", " @ %zVec @ ", 0, 0, 0);", 1);
		}
		else if(%switch == 1)
		{
			%velocity = 85;
			%zVec = 85;
			schedule("AI::shove(" @ %aiId @ ", " @ %velocity @ ", " @ %zVec @ ", 0, 0, 0);", 1);
		}
	}
		return;
	}
	else if(%armorlist == "Builder")
	{
			if(floor(getrandom() * 100) < 75 )
	{
		%switch = floor(getRandom() * 2);
		if(%switch == 0)
		{
		%velocity = 120;
		%zVec = 120;
		schedule("AI::shove(" @ %aiId @ ", " @ %velocity @ ", " @ %zVec @ ", 0, 0, 0);", 1);
		}
		else if(%switch == 1)
		{
			%velocity = 105;
			%zVec = 105;
			schedule("AI::shove(" @ %aiId @ ", " @ %velocity @ ", " @ %zVec @ ", 0, 0, 0);", 1);
		}
	}
		return;
	}
	else if(%armorlist == "Necromancer")
	{
			if(floor(getrandom() * 100) < 75 )
	{
		%switch = floor(getRandom() * 2);
		if(%switch == 0)
		{
		%velocity = 100;
		%zVec = 100;
		schedule("AI::shove(" @ %aiId @ ", " @ %velocity @ ", " @ %zVec @ ", 0, 0, 0);", 1);
		}
		else if(%switch == 1)
		{
			%velocity = 85;
			%zVec = 85;
			schedule("AI::shove(" @ %aiId @ ", " @ %velocity @ ", " @ %zVec @ ", 0, 0, 0);", 1);
		}
	}
		return;
	}
}


function AI::EvadeUp(%aiId)         //For climbing towers, one powerful jump back, and one forward after 1 second
{
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
%velocity = -100;
%zVec = 200;
AI::shove(%aiId, %velocity, %zVec, 0, 0, 0);
%velocity = 120;
%zVec = 200;
//AI::shove(%aiId, %velocity, %zVec, 0, 0, 0);
schedule("AI::shove(" @ %aiId @ ", " @ %velocity @ ", " @ %zVec @ ", 0, 0, 0);", 1);
}


function AI::EvadeBackLeft(%aiId)   //Turn back, then left and try again.
{
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
%velocity = -100;
%zVec = 100;
AI::shove(%aiId, %velocity, %zVec, 0, 0, 0);
%velocity = 150;
%zVec = 100;
schedule("AI::shove(" @ %aiId @ ", " @ %velocity @ ", " @ %zVec @ ", 0, 0, -1.6);", 1);
}

function AI::EvadeBackRight(%aiId)  //Turn back, then right and try again.
{
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
%velocity = -100;
%zVec = 100;
AI::shove(%aiId, %velocity, %zVec, 0, 0, 0);
%velocity = 150;
%zVec = 100;
schedule("AI::shove(" @ %aiId @ ", " @ %velocity @ ", " @ %zVec @ ", 0, 0, 1.6);", 1);
}

function AI::EvadeLeft(%aiId)       //Jump left
{
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
%velocity = 150;
%zVec = 100;
AI::shove(%aiId, %velocity, %zVec, 0, 0, -1.6);
}

function AI::EvadeRight(%aiId)      //Jump left
{
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
%velocity = 150;
%zVec = 100;
AI::shove(%aiId, %velocity, %zVec, 0, 0, 1.6);
}




function AI::RandomEvade(%aiId)      //Do anything of the above, randomly.
{
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
	
	SpoonBotsRoam(%aiId);
			
if ($Spoonbot::DebugMode)
 echo ("CALL AI::RandomEvade(" @ %aiId @ ");");

%evadeIdx = floor(getRandom() * (5 - 0.1));  // five possibilities

if ($Spoonbot::DebugMode)
 echo ("STATUS AI::RandomEvade = EvadeIdx=" @ %evadeIdx );


if (%evadeIdx == 0)
 {
AI::EvadeBackLeft(%aiId);
 }
else if (%evadeIdx == 1)
 {
AI::EvadeBackRight(%aiId);
 }
else if (%evadeIdx == 2)
 {
AI::EvadeUp(%aiId);
 }
else if (%evadeIdx == 3)
 {
AI::EvadeLeft(%aiId);
 }
else if (%evadeIdx == 4)
 {
AI::EvadeRight(%aiId);
 }
else if (%evadeIdx == 5)
 {
AI::EvadeBackLeft(%aiId);
 }
else if (%evadeIdx == 6)
 {
AI::EvadeBackRight(%aiId);
 }

return;
}



//The following function comes very close to human players jetting physics. This function calls itself over and over,
//each time applying a small amount of upward thrust to the AI. The variable "phase" is being increased with each call.
//The jetting time is limited, just like for human players.
//To make an AI start jetting, issue this function with "phase" set to 0. Then, this procedure will call itself every 0.5 seconds,
//each time increasing the "phase" variable.
//The AI will jet until "phase" = 6. Then, the AI will begin descending, while the descend speed is kept nominal by short thrusts 
//every 1 second. This prevents injuries.


//Note that once the jetting has started, the whole sequence is being executed. To STOP a jetting maneuver, set the 
//variable $SPOONBOT::StopAIJet to the aiId of the bot you want to stop jetting. If the jetting is being aborted, this function will
//set the "phase" variable to 6, thus starting to descend.


//What's the magic with AbortJet? Or better yet: What's the difference between STOPPING and ABORTING a jet maneuver?
//Well, let's assume you  want to issue a jet command, and don't know if the AI is already in the process of jetting. 
//You need to ABORT the first jetting, and schedule the NEW jetting command 1 or 2 seconds afterwards.
//This keeps the bots from climbing into the stratosphere.


function AI::JetSimulation(%aiId, %phase)         // Makes an AI jet like a real player.
{
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
if ($Spoonbot::DebugMode)
 echo ("STATUS AI::JetSimulation = in Phase " @ %phase @ " for bot " @ %aiId @ ". BotJetting is " @ $Spoonbot::BotJetting[%aiId]);

if ($SPOONBOT::StopAIJet == %aiId)  //To stop a jet, set $SPOONBOT::StopAIJet to the aiId you want to stop jetting
 {
 if (%phase < 6 )
   %phase = 6;			     //Just skip the climb phases, and start to descend
 $SPOONBOT::StopAIJet = 0;
 }

if ($SPOONBOT::AbortAIJet == %aiId)  //AbortJet is similar to StopJet, only there will be no controlled descend afterwards
 {
 $SPOONBOT::AbortAIJet = 0;
 return;			     //When Aborting, the function will kill itself immediately.
 }

$Spoonbot::BotJetting[%aiId] = 1;

%velocity = 20;
%zVec = 100;
AI::shove(%aiId, %velocity, %zVec, 0, 0, 0);
%phase = %phase + 1;

if (%phase < 6)
 {
 $Spoonbot::BotJettingHeat[%aiId] = 1;
 schedule("AI::JetSimulation(" @ %aiId @ ", " @ %phase @ ");", 0.5);  //First 6 phases: climb
 return;
 }

$Spoonbot::BotJettingHeat[%aiId] = 0;  //The BotJettingHeat is only 1 if we're in climbing phase. This is for rocket turrets.

if (%phase < 10)
 {
 schedule("AI::JetSimulation(" @ %aiId @ ", " @ %phase @ ");", 1);    //After climbing, do a controlled descend.
 return;
 }

$Spoonbot::BotJetting[%aiId] = -1;

// if "phase" exceeds 10, then this function will simply stop keeping itself alive.
return;
}

function AI::JetToHeight(%aiId, %height, %phase)         // Makes an AI jet to a specified height. Here the phase variable is only for INTERNAL use!
{
//			%aiId = BotFuncs::GetId(%aiName);
		%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }

	$Spoonbot::BotJetting[%aiId] = -1;
	$Spoonbot::BotJettingHeat[%aiId] = 0;

	%AiPos = GameBase::getPosition(%aiId);
	%zPos = getWord(%AiPos, 2);


					//If we reached our height, stop jetting and abort this function.
					//I tried a new trick here: I stop the jetting sooner if the bot already has a great upward speed.

	if (%phase >=8)
		{
		if ((%zPos+6) >= %height)
			{
			%phase = 0;
			%velocity = 50;
			%zVec = 50;
			AI::shove(%aiId, %velocity, %zVec, 0, 0, 0);
			return;
			}
		}

	if (%phase >=6)
		{
		if ((%zPos+5) >= %height)
			{
			%phase = 0;
			%velocity = 50;
			%zVec = 50;
			AI::shove(%aiId, %velocity, %zVec, 0, 0, 0);
			return;
			}
		}

	if (%phase >=4)
		{
		if ((%zPos+4) >= %height)
			{
			%phase = 0;
			%velocity = 50;
			%zVec = 50;
			AI::shove(%aiId, %velocity, %zVec, 0, 0, 0);
			return;
			}
		}


	$Spoonbot::BotJetting[%aiId] = 1;
	$Spoonbot::BotJettingHeat[%aiId] = 1;  //These vars make turrets track the jetting bots, and avoid "double-jetting"

	%velocity = 0;  //This is the forward velocity.

	%zVec = 150;

	AI::shove(%aiId, %velocity, %zVec, 0, 0, 0);
	%phase = %phase + 1;

	schedule("AI::JetToHeight(" @ %aiId @ ", " @ %height @ ", " @ %phase @ ");", 0.3); 
	
	//Reschedule this until bot reaches %height.

	return;
}
