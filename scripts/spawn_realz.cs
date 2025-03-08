function Game::playerSpawn(%clientId, %respawn)
{
	if(!$ghosting)
		return false;
	%clientId.AdminobserverMode = "";
	Client::clearItemShopping(%clientId);
	%spawnMarker = Game::pickPlayerSpawn(%clientId, %respawn);
	
	if(%clientId.inArena) 
	{
		if($ArenaTD::Active)
			if(%clientId.isArenaTDDead) return;
		if($ArenaTD::Active && !%clientId.inArenaTD) return;
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
		else if(%clientId.inDuel) 
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

		if($build)
		{
			$DefaultArmor[Male] = armormBuilder;
			$DefaultArmor[Female] = armorfBuilder;	
		}
		else if(%clientId.inArena) 
		{
			if($TAArena::SpawnType == "AnniSpawn") 
			{
				$DefaultArmor[Male] = armormWarrior;
				$DefaultArmor[Female] = armorfWarrior;	
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
			if(%clientId.DuelArmor == "AnniSpawn") 
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
			if($TALT::SpawnType == "AnniSpawn") 
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
			//echo("start"); 
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

			if($TALT::Active == false) 
				$respawnInvulnerableTime = 10;
			if(%respawn)	
			{
				if($TALT::Active == false) 
					$respawnInvulnerableTime = 5;
				if($siegeFlag)	
					Siege::waypointClient(%clientId);
			}
			if($TALT::Active == false) 
           		%damage = %armor.maxDamage - 0.001;
            GameBase::setEnergy(%pl, %armor.maxEnergy);	///2);
			if($TALT::Active == false) 
			{
            	GameBase::setAutoRepairRate(%pl, %damage / $respawnInvulnerableTime);
            	%pl.invulnerable = true;
			}
            %pl.cloakable = true;	
            $jailed[%pl] = "";	
            $released[%pl] = "";
      		if($TALT::Active == false) 
            	incInvulnerable(%pl, 0);
		    %weapon = Player::getMountedItem(%pl,$WeaponSlot);
		    if(%weapon != -1) 
			{
			      %pl.lastWeapon = %weapon;
			      Player::unMountItem(%pl,$WeaponSlot); 
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