
function AI::SpawnAdditionalBot(%aiName, %teamnum, %spawnFromAdmin)
{

	if (%aiName == "")
	{
	  return;
	}
	
	   $numAI = 0;
	
	
	   %spawnMarker = AI::pickRandomSpawn(%teamnum);
	   if(%spawnMarker == -1)
	   {
	      %spawnPos = "0 0 300";
	      %spawnRot = "0 0 0";
	   }
	   else
	   {
	      %spawnPos = GameBase::getPosition(%spawnMarker);
	      %spawnRot = GameBase::getRotation(%spawnMarker);
	   }
	
		 %rPos = %spawnRot;
	         %xPos = getWord(%spawnPos, 0);
	         %yPos = getword(%spawnPos, 1);
	         %zPos = getWord(%spawnPos, 2);
	         %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;
	
	//         dbecho(1, "Random Spawn Position is: " @ %xPos @ "  " @ %yPos @ "  " @ %zPos);


//	%Botname2 = GetBotName2(%aiName);
	
	
	//======================================================================================================================= Setup Bot Armors
	
		if((String::findSubStr(%aiName, "Optimus_Prime") >= 0) || (String::findSubStr(%aiName, "Android18") >= 0) || (String::findSubStr(%aiName, "Bumblebee") >= 0) || (String::findSubStr(%aiName, "Commander_Data") >= 0) || (String::findSubStr(%aiName, "Megatron") >= 0) || (String::findSubStr(%aiName, "T_800") >= 0) || (String::findSubStr(%aiName, "Jetfire") >= 0) || (String::findSubStr(%aiName, "R2_D2") >= 0) || (String::findSubStr(%aiName, "Starscream") >= 0) || (String::findSubStr(%aiName, "Asimo") >= 0) || (String::findSubStr(%aiName, "Dinobot") >= 0) || (String::findSubStr(%aiName, "Hal_9000") >= 0) || (String::findSubStr(%aiName, "Sentinel_Prime") >= 0) || (String::findSubStr(%aiName, "Spot") >= 0))
		{
			pickbotarmortype();
			%voice = "female1";
		}
		
//================================================================================================================== Done Setting Up Armors

	  if (%spawnFromAdmin == 1) //Are we spawning from the menu in admin.cs ?
	  {                         //If yes, we have to insert a bot count number into the name
	      %num = 1;
	      %newName = "T" @ %teamnum @ "N" @ %num @"_" @ %aiName;
	      %spawnSuccessfull = 0;
	      if( AI::spawn( %newName, $AI::defaultArmorType, %aiSpawnPos, %rPos, %newName, %voice  ) != "false" )
	      {
		$Spoonbot::NumBots = $Spoonbot::NumBots + 1;
	        %spawnSuccessfull = 1;
	      }
	      else
	      {
	        %num++;
	        %newName = "T" @ %teamnum @ "N" @ %num @"_" @ %aiName;
	        if( AI::spawn( %newName, $AI::defaultArmorType, %aiSpawnPos, %rPos, %newName, %voice  ) != "false" )
	        {
		$Spoonbot::NumBots = $Spoonbot::NumBots + 1;
	          %spawnSuccessfull = 1;
	        }
	        else
	        {
	          %num++;
	          %newName = "T" @ %teamnum @ "N" @ %num @"_" @ %aiName;
	          if( AI::spawn( %newName, $AI::defaultArmorType, %aiSpawnPos, %rPos, %newName, %voice  ) != "false" )
	          {
		$Spoonbot::NumBots = $Spoonbot::NumBots + 1;
	            %spawnSuccessfull = 1;
	          }
	          else
	          {
	            dbecho( 1, "Cannot spawn " @ %aiName @ ", only 3 bots per class allowed for each team");
	          }
	        }
	      }
	  }
	  else
	  {                     
	      %newName = %aiName;
	      if( AI::spawn( %newName, $AI::defaultArmorType, %aiSpawnPos, %rPos, %newName, %voice  ) != "false" )
	      {
		$Spoonbot::NumBots = $Spoonbot::NumBots + 1;
	         %spawnSuccessfull = 1;
	
			 %aiId = BotFuncs::GetId(%aiName);
	
			 $BotThink::BotHome[%aiId] = %spawnPos; // Set AI homepoint as spawnpoint. 
	
		     //dbecho( 1, "AutoSpawn successfull for " @ %aiName );
	      }
		  else
	      {
	         dbecho( 1, "AutoSpawn error: Cannot spawn " @ %aiName );
	      }
	    
	  }
	
	
	  newObject("AI", SimGroup);
	  newObject(%newName, SimGroup);
	  newObject("Marker1", Marker, PathMarker,0,%xPos,%yPos,%zPos,0,0,0);
	  addToSet(%newName, "Marker1");
	  addToSet("AI", %newName);
	  addToSet("MissionGroup\\Teams\\team" @ %teamnum, AI);
	  
	  %aiId = BotFuncs::GetId( %newName );
	  	  BotFuncs::InitVars( %aiId );     
	
	
	
	
	// Above method allows spawning of 3 bots per class and team. The old method below only allowed one.
	// The problem was to try all names until a free name was found. That way you can control the number of bots the client can spawn...
	// ... and we don't wanna have a player spawning huge armies of bots ;-)
	
	//   Ai::spawn(%newName, $AI::defaultArmorType, %aiSpawnPos, %rPos, %newName, %voice );
	
	
	  %aiId = BotFuncs::GetId( %newName );
	  GameBase::setTeam(%aiId, %teamnum);
	  // AI::setVar( %newName,  iq,  90 );
	  AI::setVar( %newName,  iq,  50 );
	  AI::setVar( %newName,  attackMode, 1);
	  AI::setVar( %newName,  pathType, $AI::defaultPathType);
	  

//	  schedule("AI::setWeapons(" @ %aiId @ ");", 1); 
	  schedule("AI::setWeapons(" @ %newName @ ");", 1); 
	
	// Add Bot think function to schedule 
	
//	  BotFuncs::InitVars( %aiId );     
	
	  Client::setSkin(%aiId, $Server::teamSkin[Client::getTeam(%aiId)]);  
	  
	
//		if (BotTypes::IsMedic(%newName))    //As of yet only one Medic can work in the Object Repair Task Queue. (This means repairing Turrets, etc)
//		{
//			if (%teamnum == 0)
//				 $Spoonbot::Team0Medic = %aiId;
//			if (%teamnum == 1)
//				 $Spoonbot::Team1Medic = %aiId;
//		}
	
	
    $BotThink::Definitive_Attackpoint[%aiId] = "";
    $BotThink::ForcedOfftrack[%aiId] = true;

//	  schedule("BotThink::Think(" @ %aiId @ ", True);", 3);   
	  schedule("BotMove::Move(" @ %aiId @ " );", 1.5);     
	  
	%id = ai::GetId( %aiName );
	$Actions::Hunting[%id] = true;
	$Actions::Flying[%id] = true;
	$Actions::Speaking[%id] = true;
	$Actions::Grenades[%id] = true;
	$Actions::Beacons[%id] = true;
	$Actions::Weapons[%id] = true;

	  return ( %newName );
}

//----------------------------------
// AI::setupAI()
//
// Called from Mission::init() which is defined in Objectives.cs (or Dm.cs for
//    deathmatch missions).  
//----------------------------------   

function AI::setupAI(%key, %team)
{


    %aiName = %key;

    if (%aiName == "")
     {
      return;
     }

	if((String::findSubStr(%aiName, "Optimus_Prime") >= 0) || (String::findSubStr(%aiName, "Android18") >= 0) || (String::findSubStr(%aiName, "Bumblebee") >= 0) || (String::findSubStr(%aiName, "Commander_Data") >= 0) || (String::findSubStr(%aiName, "Megatron") >= 0) || (String::findSubStr(%aiName, "T_800") >= 0) || (String::findSubStr(%aiName, "Jetfire") >= 0) || (String::findSubStr(%aiName, "R2_D2") >= 0) || (String::findSubStr(%aiName, "Starscream") >= 0) || (String::findSubStr(%aiName, "Asimo") >= 0) || (String::findSubStr(%aiName, "Dinobot") >= 0) || (String::findSubStr(%aiName, "Hal_9000") >= 0) || (String::findSubStr(%aiName, "Sentinel_Prime") >= 0) || (String::findSubStr(%aiName, "Spot") >= 0))
	{
		pickbotarmortype();
		%voice = "female1";
	}


   //if there is no key then they don't exist yet
   if(%key == "")
   {
      %aiFound = 0;
      for( %T = 0; %T < 8; %T++ )
      {
         %groupId = nameToID("MissionGroup\\Teams\\team" @ %T @ "\\AI" );
         if( %groupId != -1 )
         {
            %teamItemCount = Group::objectCount(%groupId);
            if( %teamItemCount > 0 )
            {
               AI::initDrones(%T, %teamItemCount);
               %aiFound += %teamItemCount;
            }
         }
      }
      if( %aiFound == 0 )
         dbecho(1, "No drones exist in map...");
      else
         dbecho(1, %aiFound @ " drones installed..." );
   }
   else     //respawning dead AI with original name and path
   {
      %group = nameToID("MissionGroup\\Teams\\team" @ %team @ "\\AI\\" @ %key);
      %num = Group::objectCount(%group);
      %aiId = BotFuncs::GetId(%key);

      if( %aiId <= 0) // Is it a pre-defined AI, or a dynamically spawned? If it is the latter, then...
      {
         %teamnum = %team;
         %newName = %key;



// New method for finding spawn points:
//------------------------------------
   %spawnMarker = AI::pickRandomSpawn(%teamnum);
   if(%spawnMarker == -1)
   {
      %spawnPos = "0 0 300";
      %spawnRot = "0 0 0";
   }
   else
   {
      %spawnPos = GameBase::getPosition(%spawnMarker);
      %spawnRot = GameBase::getRotation(%spawnMarker);
   }



         %xPos = getWord(%spawnPos, 0);
         %yPos = getword(%spawnPos, 1);
         %zPos = getWord(%spawnPos, 2);
//         %rPos = GameBase::getRotation(%commandIssuer);
         %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;

//         dbecho(1, "Random Spawn Position is: " @ %xPos @ "  " @ %yPos @ "  " @ %zPos);


         Ai::spawn(%newName, $AI::defaultArmorType, %aiSpawnPos, %spawnRot, %newName, %voice );
	 $Spoonbot::NumBots = $Spoonbot::NumBots + 1;
         %aiId = BotFuncs::GetId( %newName );
         GameBase::setTeam(%aiId, %teamnum);
         // AI::setVar( %newName,  iq,  90 );
		 	  AI::setVar( %newName,  iq,  50 );
         AI::setVar( %newName,  attackMode, 1);
         AI::setVar( %newName,  pathType, $AI::defaultPathType);

//  BotFuncs::InitVars( %aiId );     

  Client::setSkin(%aiId, $Server::teamSkin[Client::getTeam(%aiId)]);  

// if (BotTypes::IsMedic(%newName))    //As of yet only one Medic can work in the Object Repair Task Queue. (This means repairing Turrets, etc)
// {
// if (%teamnum == 0)
// $Spoonbot::Team0Medic = %aiId;
// if (%teamnum == 1)
// $Spoonbot::Team1Medic = %aiId;
// }

  $BotThink::Definitive_Attackpoint[%aiId] = -1;
  $BotThink::ForcedOfftrack[%aiId] = true;

//  schedule("BotThink::Think(" @ %aiId @ ", True);", 3);    

         schedule("AI::setWeapons(" @ %aiId @ ");", 1);
         
      }
      //--------------------------------------------


      else  //else respawn like regular AIs
      {


      createAI(%key, %group, $AI::defaultArmorType, %key);
      %aiId = BotFuncs::GetId(%key);


      GameBase::setTeam(%aiId, %team);
      AI::setVar(%key, pathType, $AI::defaultPathType);

      AI::setWeapons(%aiId);
//      AI::setWeapons(%key);

   }
  }	
}