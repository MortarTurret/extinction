//$Arena::MapCount = 2;
$Arena::Spawn = "1200 1200 4000"; 
$Arena::Initialized = false;
$Arena::Mapchange = false;
$Arena::firstMap = "bcube";
$Arena::ActiveVote = false;
$Arena::NoForceTeam = true;
$Arena::MapRipper = false;
$Arena::Lock = false;
$Arena::Terrain = false;
$TA::ArenaReset = true;
$Pi = 3.14159;

function Arena::Init(%name)
{
	if($debug)
		echo(%name@"  <<<<<<<<<<<< Arena::Init(%opt)");
		
	if($Arena::canBuild[%name] == true && !$Arena::Initialized)
	{
		exec(%name); //load the arena map
		$Arena::Spawn = TA::pickWaypoint("arena");
		$Arena::Initialized = true; // so so true
		$Arena::curMission = %name;
		if(%name == "arenaterrain")
			$Arena::Terrain = true;
		else
			$Arena::Terrain = false;
			
		if($ArenaTD::Active && $Arena::Terrain)
			$ArenaTD::TerrainPos = TA::pickWaypoint("toob");
			


		%arenaGroup = NewObject("Arena",SimGroup);
		AddToSet("MissionCleanup",%arenaGroup);
 
		%arenaGroup = NewObject("Spawnpoints3",SimGroup);
		AddToSet("MissionCleanup\\Arena",%arenaGroup);
		
		%arenaGroup = NewObject("Spawnpoints1",SimGroup);
		AddToSet("MissionCleanup\\Arena",%arenaGroup);
		
		%arenaGroup = NewObject("Spawnpoints2",SimGroup);
		AddToSet("MissionCleanup\\Arena",%arenaGroup);

		//if($debug)
		   echo("!! Arena being created !!");
		   
		for(%i = 0; $Arena::Object[%name,%i] != ""; %i++)
		{
			%arenaObject = $Arena::Object[%name,%i];
			%shape = getWord(%arenaObject,0);
			//
			%posX = getWord(%arenaObject,1);
			%posY = getWord(%arenaObject,2);
			%posZ = getWord(%arenaObject,3);
			%pos = %posX@" "@%posY@" "@%posZ;
			//
			%rotX = getWord(%arenaObject,4);
			%rotY = getWord(%arenaObject,5);
			%rotZ = getWord(%arenaObject,6);
			%rot = %rotX@" "@%rotY@" "@%rotZ;
			if(String::findSubStr(%shape,"spawn1") != -1)
			{
				if($Arena::Terrain)
				{
					%type = "Spawn Point";
					%shape = DropPointMarker;
	
					%obj = NewObject("Spawnpoint",Marker,DropPointMarker,true);
					AddToSet("MissionCleanup\\Arena\\Spawnpoints1",%obj);
					//GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$ArenaTD::Spawn[0]));
					GameBase::setPosition(%obj,%pos);
					GameBase::setRotation(%obj,%rot);
					%obj.isArenaPart = true;
					$ArenaPart[%i] = %obj;
					%return = true;
				}
				else
				{
					%type = "Spawn Point";
					%shape = DropPointMarker;
	
					%obj = NewObject("Spawnpoint",Marker,DropPointMarker,true);
					AddToSet("MissionCleanup\\Arena\\Spawnpoints1",%obj);
					GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$Arena::Spawn));
					GameBase::setRotation(%obj,%rot);
					%obj.isArenaPart = true;
					$ArenaPart[%i] = %obj;
					%return = true;
				}

				if($debug)
				   echo("!! Piece #"@%i@" added ("@%type@" "@%shape@")");
			}
			else if(String::findSubStr(%shape,"spawn2") != -1)
			{
				if($Arena::Terrain)
				{
					%type = "Spawn Point";
					%shape = DropPointMarker;
	
					%obj = NewObject("Spawnpoint",Marker,DropPointMarker,true);
					AddToSet("MissionCleanup\\Arena\\Spawnpoints2",%obj);
					//GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$TA::Spawn[0]));
					GameBase::setPosition(%obj,%pos);
					GameBase::setRotation(%obj,%rot);
					%obj.isArenaPart = true;
					$ArenaPart[%i] = %obj;
					%return = true;
				}
				else
				{
					%type = "Spawn Point";
					%shape = DropPointMarker;
	
					%obj = NewObject("Spawnpoint",Marker,DropPointMarker,true);
					AddToSet("MissionCleanup\\Arena\\Spawnpoints2",%obj);
					GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$Arena::Spawn));
					GameBase::setRotation(%obj,%rot);
					%obj.isArenaPart = true;
					$ArenaPart[%i] = %obj;
					%return = true;
				}

				if($debug)
				   echo("!! Piece #"@%i@" added ("@%type@" "@%shape@")");
			}
			else if(String::findSubStr(%shape,"spawn3") != -1)
			{
				%type = "Spawn Point";
				%shape = DropPointMarker;

				%obj = NewObject("Spawnpoint",Marker,DropPointMarker,true);
				AddToSet("MissionCleanup\\Arena\\Spawnpoints3",%obj);
				//if($Arena::Terrain)
				//	GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$ArenaTD::Spawn[0]));
				//else
					GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$Arena::Spawn));
				GameBase::setRotation(%obj,%rot);
				%obj.isArenaPart = true;
				$ArenaPart[%i] = %obj;
				%return = true;

				if($debug)
				   echo("!! Piece #"@%i@" added ("@%type@" "@%shape@")");
			}

			if(!%return)
			{
				if(String::findSubStr(%shape,".dis") != -1)
				{
				   %type = InteriorShape;
				   //%tits = false; // like akkichu :D
				}
				else if(String::findSubStr(%shape,"PortGenerator") != -1) //added more items to make arena fun
				{
					%type = StaticShape;
				}
				else if(String::findSubStr(%shape,"Generator") != -1)
				{
					%type = StaticShape;
				}
				else if(String::findSubStr(%shape,"AmmoStation") != -1)
				{
					%type = StaticShape;
				}
				else if(String::findSubStr(%shape,"InventoryStation") != -1)
				{
					%type = StaticShape;
				}
				else if(String::findSubStr(%shape,"ForceField") != -1)
				{
					%type = StaticShape;
				}
				else if(String::findSubStr(%shape,"ForceField4") != -1)
				{
					%type = StaticShape;
				}
				else if(String::findSubStr(%shape,"TowerSwitch") != -1)
				{
					%type = StaticShape;
				}
				else if(String::findSubStr(%shape,"GrenadeAmmo") != -1) //add extra for ammo
				{
					if($TAArena::SpawnType == "AnniSpawn") //and now I added support for different spawns
						%shape = "GrenadeAmmoBase";
					else if($TAArena::SpawnType == "EliteSpawn") //but this only works at the spawn of the map. Doh!
						%shape = "GrenadeAmmoBase";
					else if($TAArena::SpawnType == "BaseSpawn")
						%shape = "GrenadeAmmoBase";
					%type = Item;
					%num = "5";
				}
				else if(String::findSubStr(%shape,"PlasmaAmmo") != -1)
				{
					if($TAArena::SpawnType == "AnniSpawn")
						%shape = "PlasmaAmmo";
					else if($TAArena::SpawnType == "EliteSpawn")
						%shape = "PlasmaAmmoBase";
					else if($TAArena::SpawnType == "BaseSpawn")
						%shape = "PlasmaAmmoBase";
					%type = Item;
					%num = "5";
				}
				else if(String::findSubStr(%shape,"DiscAmmo") != -1)
				{
					if($TAArena::SpawnType == "AnniSpawn")
						%shape = "DiscAmmo";
					else if($TAArena::SpawnType == "EliteSpawn")
						%shape = "DiscAmmoElite";
					else if($TAArena::SpawnType == "BaseSpawn")
						%shape = "DiscAmmoBase";
					%type = Item;
					%num = "5";
				}
				else if(String::findSubStr(%shape,"Grenade") != -1)
				{
					%type = Item;
					%num = "5";
				}
				else if(String::findSubStr(%shape,"SpringPad") != -1)
				{
					%type = StaticShape;
					%num = "1";
				}
				else if(String::findSubStr(%shape,"ElectricalBeamBig") != -1)
				{
					%type = StaticShape;
					%num = "1";
				}
				else if(String::findSubStr(%shape,"TreeShape") != -1)
				{
					%type = StaticShape;
					%num = "1";
				}
				else if(String::findSubStr(%shape,"TreeShapeTwo") != -1) 
				{
					%type = StaticShape;
					%num = "1";
				}
				else
				{
					%type = Item;
					%num = "1";
				}
				if(%type == InteriorShape) //%flag = NewObject("Flag","item",Flag,false); %pPos = GameBase::getPosition(2049); GameBase::setPosition(%flag,Vector::add(%pPos,"0 0 5")); AddToSet("MissionCleanup\\Arena",%flag);
				{							//GameBase::setPosition(NewObject("Flag","item",Flag,false),Vector::add(GameBase::getPosition(2049),"0 0 5")); // AddToSet("MissionCleanup\\Arena",NewObject("Flag","item",Flag,false));
					%obj = NewObject(%shape,%type,%shape,false);
					AddToSet("MissionCleanup\\Arena",%obj);
					GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$Arena::Spawn));
					GameBase::setRotation(%obj,%rot);
					%obj.isArenaPart = true;
					$ArenaPart[%i] = %obj;
				}
				else if(%type == StaticShape)
				{
					if(%shape == AmmoStation)
					{
						%obj = NewObject(%shape,%type,%shape,false);
						//%team = GameBase::setTeam(%obj,0);
						AddToSet("MissionCleanup\\Arena",%obj);
						GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$Arena::Spawn));
						GameBase::setRotation(%obj,%rot);
						GameBase::setTeam(%obj,-1);
						%obj.isArenaPart = true;
						%obj.isUnKillable = true;
						$ArenaPart[%i] = %obj;
					}
					else if(%shape == TowerSwitch)
					{
						%desc = getWord(%arenaObject, 7); 
						$descx = %desc;
						%x = 8;
						//echo("$descx == "@$descx@" <<<<<<<<<<<<<<<<<<<<< Tower Switch >>>>");
						%word = getWord(%arenaObject,%x);
						if(%word != -1 )
						{
							$descx = $descx@" "@%word;
							%desc = $descx;
						}
						//echo("%word == "@%word@" <<<<<<<<<<<<<<<<<<<<< Tower Switch >>>>");
						//echo("%x == "@%x@" <<<<<<<<<<<<<<<<<<<<< Tower Switch >>>>");
						if(%word != -1 )
						{
							%word = getWord(%arenaObject,%x++);
							//echo("%word == "@%word@" <<<<<<<<<<<<<<<<<<<<< Tower Switch >>>>");
							if(%word != -1 )
							{
								$descx = $descx@" "@%word;
								%desc = $descx;
							}
							//echo("%desc == "@$desc@" <<<<<<<<<<<<<<<<<<<<< Tower Switch >>>>");
							//echo("%x == "@%x@" <<<<<<<<<<<<<<<<<<<<< Tower Switch >>>>");
						}
						if(%word != -1 )
						{
							%word = getWord(%arenaObject,%x++);
							//echo("%word == "@%word@" <<<<<<<<<<<<<<<<<<<<< Tower Switch >>>>");
							if(%word != -1 )
							{
								$descx = $descx@" "@%word;
								%desc = $descx;
							}
							//echo("%desc == "@$desc@" <<<<<<<<<<<<<<<<<<<<< Tower Switch >>>>");
							//echo("%x == "@%x@" <<<<<<<<<<<<<<<<<<<<< Tower Switch >>>>");
						}
						if(%word != -1 )
						{
							%word = getWord(%arenaObject,%x++);
							if(%word != -1 )
							{
								$descx = $descx@" "@%word;
								%desc = $descx;
							}
						}
						if(%word != -1 )
						{
							%word = getWord(%arenaObject,%x++);
							if(%word != -1 )
							{
								$descx = $descx@" "@%word;
								%desc = $descx;
							}
						}
						if(%word != -1 )
						{
							%word = getWord(%arenaObject,%x++);
							if(%word != -1 )
							{
								$descx = $descx@" "@%word;
								%desc = $descx;
							}
						}
						if(%word != -1 )
						{
							%word = getWord(%arenaObject,%x++);
							if(%word != -1 )
							{
								$descx = $descx@" "@%word;
								%desc = $descx;
							}
						}
						if(%word != -1 )
						{
							%word = getWord(%arenaObject,%x++);
							if(%word != -1 )
							{
								$descx = $descx@" "@%word;
								%desc = $descx;
							}
							
						}
						if($debug)
							echo(%desc@" <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< TS <<");
						%obj = NewObject(%desc,%type,%shape,false);
						//%team = GameBase::setTeam(%obj,0);
						AddToSet("MissionCleanup\\Arena",%obj);
						GameBase::setMapName(%obj,%desc);
						GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$Arena::Spawn));
						GameBase::setRotation(%obj,%rot);
						GameBase::setTeam(%obj,0);
						%obj.isArenaPart = true;
						$ArenaPart[%i] = %obj;
						%mn = GameBase::getMapName(%obj);
						echo(%mn@" <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< MN <<");	
					}
					else
					{
						%obj = NewObject(%shape,%type,%shape,false);
						//%team = GameBase::setTeam(%obj,0);
						AddToSet("MissionCleanup\\Arena",%obj);
						GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$Arena::Spawn));
						GameBase::setRotation(%obj,%rot);
						GameBase::setTeam(%obj,0);
						%obj.isArenaPart = true;
						$ArenaPart[%i] = %obj;
					}
				}
				else if(%type == Item)
				{
					//%obj = newObject("RepairPack","Item","repairpack",1,true,true); //example from dropship
					%obj = NewObject(%shape,%type,%shape,%num,true,true);
					//%team = GameBase::setTeam(%obj,0);
					AddToSet("MissionCleanup\\Arena",%obj);
					GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",$Arena::Spawn));
					GameBase::setRotation(%obj,%rot);
					%obj.isArenaPart = true;
					$ArenaPart[%i] = %obj;
				}

				if($debug)
				   echo("!! Piece #"@%i@" added ("@%type@" "@%shape@")");
			}
		}
		Schedule("Arena::MatchStarted();",1+(%i/30));
	}
}

function Arena::Clear()
{ // get rid of the arena, will be used in Arena "map changes" later on

	if($Arena::Initialized)
	{
		echo("!! Clearing The Arena !!");
		
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			%player = Client::getOwnedObject(%cl);

			if(%cl.inArena && $loadingMission)
			{
				Game::assignClientTeam(%cl);
				%team = Client::getTeam(%cl);
				GameBase::setTeam(%player,%team);
				Client::setInitialTeam(%cl,%team);

				//%r = true;
			}
			else//if(!%r) 
			{
				if(%cl.inArena && !$Arena::Mapchange)
				{
					ArenaTD::Leave(%cl);
					Arena::Leave(%cl);
	
					//messageall(1, "The Arena has been disabled.");
					client::sendmessage(%cl, 1, "The Arena has been disabled."); 
				}
				else if(%cl.inArena && $Arena::Mapchange) //lol
				{
					//ArenaTD::Leave(%cl,true);
					Arena::Leave(%cl,true);

					Centerprint(%cl,"<jc><f3>The <f2>Arena<f3> is loading, please wait.",10);
				}
			}

			Game::refreshClientScore(%cl);
	
			for(%i = 0; $ArenaPart[%i] != ""; %i++)
			{
				%obj = $ArenaPart[%i];
				if(isObject(%obj) && %obj.isArenaPart)
				{
					deleteObject(%obj);
				}
			}
	
			if(isObject(NameToID("MissionCleanup/Arena/Spawnpoints")))
			   deleteObject(NameToID("MissionCleanup/Arena/Spawnpoints"));
	
			if(isObject(NameToID("MissionCleanup/Arena")))	
			   deleteObject(NameToID("MissionCleanup/Arena"));

			deleteVariables("ArenaPart[*");
			//deleteVariables("Arena::object*"); 
			$Arena::Initialized = false;
			$Arena::Terrain = false;
			$Arena::ActiveVote = false;
			
		}
	}
	else
	{
		//
	}
}

function GetOffSetRot(%offset,%rot,%pos)
{
	%x = getWord(%offset,0);
	%y = getWord(%offset,1);
	%z = getWord(%offset,2);
	%posA = Vector::add(%pos,Vector::getFromRot(Vector::Add(%rot,"0 0 -1.570796327"),%x));
	%posB = Vector::add(%posA,Vector::getFromRot(%rot,%y));
	%posC = Vector::add(%posB,Vector::getFromRot(Vector::Add(%rot,"1.570796327 0 0"),%z));
	return %posC;
}

function TA::OOB()
{

	deletevariables("$MissionInfo:*");
	deletevariables("$Duel::Spawn*");
	deletevariables("$ArenaTD::Spawn*");

	%start = waypointtoworld("0 0");
	%end = waypointtoworld("2048 2048");
	$MissionInfo:X = getword(%start,0);
	$MissionInfo:Y = getword(%start,1);
	$MissionInfo:H = getword(%end,0)-$MissionInfo:X;
	$MissionInfo:W = getword(%end,1)-$MissionInfo:Y;
	echo("X: "@$MissionInfo:X);
	echo("Y: "@$MissionInfo:Y);
	echo("H: "@$MissionInfo:H);
	echo("W: "@$MissionInfo:W);
	
	%x0 = $MissionInfo:X - 500;
	%y0 = $MissionInfo:Y - 500;
	$TA::Spawn[0] = %x0@" "@%y0@" 500";
	
	%x2 = $MissionInfo:X + $MissionInfo:W/2 + 500;
	%y2 = $MissionInfo:Y + $MissionInfo:W/2 + 500;
	$TA::Spawn[2] = %x2 @" "@ %y2 @ " 500";
	
	%dist = Vector::getDistance($TA::Spawn0,$TA::Spawn2); //echo(Vector::getDistance($watdist,"-2317 -2327 0"));
	%cx = $MissionInfo:X + %dist/3;
	%cy = $MissionInfo:Y + %dist/3;
	$MissionInfo:C = %cx @" "@ %cy @ " 0";
	
	%x1 = %cx - %dist/3 - 500;
	%y1 = %cy + %dist/3 + 500;
	%x3 = %cx + %dist/3 + 500;
	%y3 = %cy - %dist/3 - 500;
	$TA::Spawn[1] = %x1 @" "@ %y1 @ " 500"; // corners
	$TA::Spawn[3] = %x3 @" "@ %y3 @ " 500";
	
	
	%x4 = %cx + %dist/3 + 500;
	%y4 = %cy;
	%x5 = %cx;
	%y5 = %cy + %dist/3 + 500;
	%x6 = %cx - %dist/3 - 500;
	%y6 = %cy;
	%x7 = %cx;
	%y7 = %cy - %dist/3 - 500;
	$TA::Spawn[4] = %x4 @" "@ %y4 @ " 500"; // hopefully this isnt out of the maps gravity range...
	$TA::Spawn[5] = %x5 @" "@ %y5 @ " 500"; // sides
	$TA::Spawn[6] = %x6 @" "@ %y6 @ " 500";
	$TA::Spawn[7] = %x7 @" "@ %y7 @ " 500";
	
	TA::Sensor();
	
	//TA::Grid();
	
	if($botdebug)
		schedule("TA::BotDebug();", 17);
}

function Terrain::CheckPosSensor(%pos)
{
	if(GetLOSInfo(Vector::add(%pos,"0 0 10000"),Vector::sub(%pos,"0 0 10000"),1))
	   return $los::position;
	else 
	   return false;
}

function TA::Sensor()
{
	for(%i = 0; %i < 8; %i++)
	{
		%tpos = Terrain::CheckPosSensor($TA::Spawn[%i]);
		if(%tpos)
		{
			%pos = GetOffsetRot(%tpos,"0 0 0","5 0 -15");
			%rot = "0 0 0";
			%obj = NewObject("ForceBeacon","StaticShape",ForceBeacon,false);
			AddToSet("MissionCleanup",%obj);
			GameBase::setPosition(%obj,%pos);
			GameBase::setRotation(%obj,%rot);
			GameBase::setTeam(%obj,0);
			
			%pos = GetOffsetRot(%tpos,"0 0 0","0 0 200");
			%rot = $Pi@" 0 0";
			%obj = NewObject("Sensor",Sensor,TASensor,false);
			AddToSet("MissionCleanup",%obj);
			GameBase::setPosition(%obj,%pos);
			GameBase::setRotation(%obj,%rot);
			GameBase::setTeam(%obj,0);
			
			%pos = GetOffsetRot(%tpos,"0 0 0","5 5 -15");
			%rot = "0 0 0";
			%obj = NewObject("ForceBeacon","StaticShape",ForceBeacon,false);
			AddToSet("MissionCleanup",%obj);
			GameBase::setPosition(%obj,%pos);
			GameBase::setRotation(%obj,%rot);
			GameBase::setTeam(%obj,1);
			
			%pos = GetOffsetRot(%tpos,"0 0 0","0 5 200");
			%rot = $Pi@" 0 0";
			%obj = NewObject("Sensor",Sensor,TASensor,false);
			AddToSet("MissionCleanup",%obj);
			GameBase::setPosition(%obj,%pos);
			GameBase::setRotation(%obj,%rot);
			GameBase::setTeam(%obj,1);
		}
	}
}

// Checks 8 zones outside of the mission area.
function TA::pickWaypoint(%opt)
{
   for(%i = 0; %i < 25; %i++)
   {
      %pos = WayPointToWorldZ(%i);
	  //echo(%pos@" <<<<<< %pos");
      if(!%pos)
         continue;
		
		if(%opt == "toob")
		{
			$TeamDuel::Start[1] = floor(getSimTime() + 0.5);
			// $TeamDuel::Start[2] = $TeamDuel::Start[%Team1];
			$TeamDuel::Center[1] = %pos; //GetTotalLeaderCoords();
			// $TeamDuel::Center[2] = $TeamDuel::Center[%Team1];
			CreateMissionArea($TeamDuel::Start[1]);
		}
		
		if(%opt == "arena")
			%pos = GetOffsetRot(%pos,"0 0 0","0 0 4000");
      return %pos;
   }
}

function WayPointToWorldZ(%i)
{
   deleteVariables("LOS::*");
   %rnd = Floor(GetRandom()*8); 
   %pos = $TA::Spawn[%rnd]; 
   %set = newObject("Players", SimSet);
   %num = containerBoxFillSet(%set, $SimPlayerObjectType, %pos, 700, 700, 10000, 0);
   deleteObject(%set);
   echo("$TA::Spawn["@%rnd@"] >>>>>>>>>>>>>>>>>>> WayPoint2WorldZZZZZZ");
   if(getLOSInfo(Vector::add(%pos, "0 0 10000"), Vector::sub(%pos, "0 0 10000"), 1) && %pos != $ArenaTD::TerrainPos && %num < 1)
      return $LOS::Position;
   else
      return false;
}

function TA::BotDebug()
{
	if($botdebug)
	{
		%n="C0"; %p=$MissionInfo:C; %p = Terrain::CheckPos(%p); %p = Vector::add(%p,"0 0 3"); %objx = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true); AddToSet("MissionCleanup",%objx); GameBase::setPosition(%objx,%p); GameBase::setRotation(%objx,$Pi@" 0 0"); if(AI::spawn(%n,"armormWarrior",%p,"0 0 0"@getRandom()*$Pi)!="false") %a=Client::getOwnedObject(AI::getID(%n));
		$numAI = -1;		
		%n="D"@$numAI++; %p=$TA::Spawn0; %p = Terrain::CheckPos(%p); %p = Vector::add(%p,"0 0 3"); %objx = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true); AddToSet("MissionCleanup",%objx); GameBase::setPosition(%objx,%p); GameBase::setRotation(%objx,$Pi@" 0 0"); if(AI::spawn(%n,"armormWarrior",%p,"0 0 0"@getRandom()*$Pi)!="false") %a=Client::getOwnedObject(AI::getID(%n));
		%n="D"@$numAI++; %p=$TA::Spawn1; %p = Terrain::CheckPos(%p); %p = Vector::add(%p,"0 0 3"); %objx = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true); AddToSet("MissionCleanup",%objx); GameBase::setPosition(%objx,%p); GameBase::setRotation(%objx,$Pi@" 0 0"); if(AI::spawn(%n,"armormWarrior",%p,"0 0 0"@getRandom()*$Pi)!="false") %a=Client::getOwnedObject(AI::getID(%n));
		%n="D"@$numAI++; %p=$TA::Spawn2; %p = Terrain::CheckPos(%p); %p = Vector::add(%p,"0 0 3"); %objx = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true); AddToSet("MissionCleanup",%objx); GameBase::setPosition(%objx,%p); GameBase::setRotation(%objx,$Pi@" 0 0"); if(AI::spawn(%n,"armormWarrior",%p,"0 0 0"@getRandom()*$Pi)!="false") %a=Client::getOwnedObject(AI::getID(%n));
		%n="D"@$numAI++; %p=$TA::Spawn3; %p = Terrain::CheckPos(%p); %p = Vector::add(%p,"0 0 3"); %objx = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true); AddToSet("MissionCleanup",%objx); GameBase::setPosition(%objx,%p); GameBase::setRotation(%objx,$Pi@" 0 0"); if(AI::spawn(%n,"armormWarrior",%p,"0 0 0"@getRandom()*$Pi)!="false") %a=Client::getOwnedObject(AI::getID(%n));
		$anumAI = -1;
		%n="A"@$anumAI++; %p=$TA::Spawn4; %p = Terrain::CheckPos(%p); %p = Vector::add(%p,"0 0 3"); %objx = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true); AddToSet("MissionCleanup",%objx); GameBase::setPosition(%objx,%p); GameBase::setRotation(%objx,$Pi@" 0 0"); if(AI::spawn(%n,"armormWarrior",%p,"0 0 0"@getRandom()*$Pi)!="false") %a=Client::getOwnedObject(AI::getID(%n));
		%n="A"@$anumAI++; %p=$TA::Spawn5; %p = Terrain::CheckPos(%p); %p = Vector::add(%p,"0 0 3"); %objx = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true); AddToSet("MissionCleanup",%objx); GameBase::setPosition(%objx,%p); GameBase::setRotation(%objx,$Pi@" 0 0"); if(AI::spawn(%n,"armormWarrior",%p,"0 0 0"@getRandom()*$Pi)!="false") %a=Client::getOwnedObject(AI::getID(%n));
		%n="A"@$anumAI++; %p=$TA::Spawn6; %p = Terrain::CheckPos(%p); %p = Vector::add(%p,"0 0 3"); %objx = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true); AddToSet("MissionCleanup",%objx); GameBase::setPosition(%objx,%p); GameBase::setRotation(%objx,$Pi@" 0 0"); if(AI::spawn(%n,"armormWarrior",%p,"0 0 0"@getRandom()*$Pi)!="false") %a=Client::getOwnedObject(AI::getID(%n));
		%n="A"@$anumAI++; %p=$TA::Spawn7; %p = Terrain::CheckPos(%p); %p = Vector::add(%p,"0 0 3"); %objx = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true); AddToSet("MissionCleanup",%objx); GameBase::setPosition(%objx,%p); GameBase::setRotation(%objx,$Pi@" 0 0"); if(AI::spawn(%n,"armormWarrior",%p,"0 0 0"@getRandom()*$Pi)!="false") %a=Client::getOwnedObject(AI::getID(%n));
		
		echo($MissionInfo:C@" Center Spawn <<< <<< <<<");
		echo($TA::Spawn0@" Duel Spawn <<< <<< <<<");
		echo($TA::Spawn1@" Duel Spawn <<< <<< <<<");
		echo($TA::Spawn2@" Duel Spawn <<< <<< <<<");
		echo($TA::Spawn3@" Duel Spawn <<< <<< <<<");
		echo($TA::Spawn4@" ArenaTD Spawn <<< <<< <<<");
		echo($TA::Spawn5@" ArenaTD Spawn <<< <<< <<<");
		echo($TA::Spawn6@" ArenaTD Spawn <<< <<< <<<");
		echo($TA::Spawn7@" ArenaTD Spawn <<< <<< <<<");
	}
}

function Terrain::CheckPos(%pos)
{
	//echo(Vector::add(%pos,"0 0 500")@" >>> Terrain::CheckPos Vector::Add");
	//echo(Vector::sub(%pos,"0 0 1000")@" >>> Terrain::CheckPos Vector::Sub");
	if(GetLOSInfo(Vector::add(%pos,"0 0 10000"),Vector::sub(%pos,"0 0 10000"),1))//$SimTerrainObjectType|$SimInteriorObjectType|$StaticObjectType)) 
	{
		if($debug)
			echo($los::postion@" <<<<< Terrain::CheckPos <<<< PASSED!!!!!!");
	   return $los::position;
	}
	else 
	{
		//if($debug)
			echo(%pos@" << Terrain::CheckPos >> Failed");
	   return %pos;
	}
}

function Arena::pickRandomSpawn(%clientId)
{
	if($ArenaTD::Active && %clientId.inArenaTD)
	{
		if(%clientId.inArenaTDOne)
		{
			%group = nameToID("MissionCleanup/Arena/SpawnPoints1");
			%count = Group::objectCount(%group);

			if(!%count)
				return;

			for(%i = 0; %i < %count; %i++)
			{
				%spawnIdx = Group::getObject(%group, %i);

				//echo(%spawnIdx@"  ><<<<<<< Arena TD spawn0");
				%pos = GameBase::getPosition(%spawnIdx);
				if(checkArenaSpawnArea(%pos) == true)
					break;
			}

			return %spawnIdx;
		}
		else if(%clientId.inArenaTDTwo)
		{
			%group = nameToID("MissionCleanup/Arena/SpawnPoints2");
			%count = Group::objectCount(%group);
			if(!%count)
				return;

			for(%i = 0; %i < %count; %i++)
			{
				%spawnIdx = Group::getObject(%group, %i);

				//echo(%spawnIdx@"  ><<<<<<< Arena TD spawn1");
				%pos = GameBase::getPosition(%spawnIdx);
				if(checkArenaSpawnArea(%pos) == true)
					break;
			}

			return %spawnIdx;
		}
	}
	else
	{
		%group = nameToID("MissionCleanup/Arena/SpawnPoints3");
		%count = Group::objectCount(%group);

		if(!%count)
			return;

		%spawnIdx = floor(getRandom() * (%count - 0.1));
	
		%obj = Group::getObject(%group,%spawnIdx);

		return %obj;
	}
}