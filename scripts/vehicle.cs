


function Vehicle::onAdd(%this)
{
	// %this.hasexpmessage = true;
	%this.shieldStrength = 0.0;
	GameBase::setRechargeRate (%this, 10);
	GameBase::setMapName (%this, "Vehicle");
	// if($annihilation::VehicleImpactor)
		vehicle::ImpactCheck(%this);

//	%name = Vehicle::getDataName(%this); not sure why this was vehicle and not gamebase
	%name = GameBase::getDataName(%this);
	$IsBotPiloted[%this] = false;
	echo("ADD: "@%this@" "@%name);
	%this.coming = 1;
	schedule(%this @ ".coming = \"\";",5.0,%this);
	
}
function checklos(%this)
{
	if(GameBase::getLOSInfo(%this,100,"0.15 0 0"))	//,"0 0 3.14" ,"0 0 1.57" left, "0 1.57 0" forward
	{
		%object = getObjectType($Los::Object);
		return(%object);
		bottomprint(2049,"<jc>"@%object);
	}
}

function vehicle::ImpactCheck(%this)
{
	if(GameBase::getDamageState(%this) == Destroyed) 
		return;	
//	%name = GameBase::getDataName(%this);	
	%type = getObjectType(%this);
	%control = GameBase::getControlClient(%this);

	if(%control != -1 && %type == Flier)
	{	
		%vel = Item::getVelocity(%this);
		%velocity = vector::getdistance(%vel,"0 0 0");
		%data = GameBase::getDataName(%this);
		%shape = %data.shapeFile;
		if(%shape != camera && %shape != rocket)
		{
			if(%velocity > 0.75)
			{		
				
				%check = 5 + %velocity/5;	
				if(GameBase::getLOSInfo(%this,%check,"0.15 0 0"))	//look up a lil...
				{
					%object = getObjectType($Los::Object);
						// GetLOSInfo sets the following globals:
						// 	los::position
						// 	los::normal
						// 	los::object		
					if(%object == StaticShape)
					{
						//	if(%control != -1)
						//		bottomprint(%control,"<jc>IMPACT?! "@%object@" Velocity = "@%velocity,5);		
						echo(%this@" "@%shape@" "@%control@" IMPACT?! "@%object@" killing vehicle -error");
						GameBase::setDamageLevel(%this,2);
						return;
					}
				}
				//echo("check vehicle impact. vel ="@%velocity@" check= "@%check);
			}
			
		}
		if ( %data == OSMissile )
		{
			%check = 5 + %velocity/5;	
			if(GameBase::getLOSInfo(%this,%check))	//look up a lil...
			{
				if ( getObjectType($Los::Object) == Player )
				{
					GameBase::setDamageLevel(%this,2);
					GameBase::applyDamage($Los::Object,$RocketDamageType,3,GameBase::getPosition(%this),"0 0 0","0 0 0",%this);
				}
			}
		}
	}
	schedule("vehicle::ImpactCheck("@%this@");",0.03,%this);	
}

function Vehicle::onCollision(%this, %object)
{	
	%client = Player::getclient(%object);
	
	
	if(Player::isAiControlled(%object))
	{
		
				// without this your tribes will lose its will to exist
				if(%this.coming == "1") 
				{
				//	messageall(1, "That is coming dawg.");	
				//	echo("That is coming dawg.");
					
					%curDie = radnomItems(4, $PlayerAnim::DieHead,$PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
					Player::setAnimation(%object, %curDie);
					playNextAnim(%object);
					// AI::onDroneKilled(%aiName);
					Player::kill(%object);
					return;
				}
				
				
		
//		if(GameBase::getDataName(%this) == Interceptor)
//		{
//			return;
//		}
//		if(GameBase::getDataName(%this) == Scout)
//		{
//			return;
//		}

	if($Actions::Pilot[%client]) 
    {
	//	messageall(1, "Actions Pilot Return.");
        return;
    }
	
	if($Actions::Passenger[%client]) 
    {
	//	messageall(1, "Actions Passenger Return.");
		%curDie = radnomItems(4, $PlayerAnim::DieHead,$PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
		Player::setAnimation(%object, %curDie);
		playNextAnim(%object);
		Player::kill(%object);
        return;
    }
		
		if(GameBase::getDamageLevel(%this) < (GameBase::getDataName(%this)).maxDamage) 
		{
			// if(getObjectType(%object) == "Player" && %object.vehicle == "" && %this.fading == "" || %object.lastMount != %this )
			if(getObjectType(%object) == "Player" && %object.vehicle == "" && (getSimTime() > %object.newMountTime || %object.lastMount != %this) && %this.fading == "")
			// if (getObjectType (%object) == "Player" && (getSimTime() > %object.newMountTime || %object.lastMount != %this) && %this.fading == "")
			{
						
			%armor = Player::getArmor(%object);
			%client = Player::getClient(%object);
			
				%mountSlot = Vehicle::findEmptySeat(%this,%client);
				if(%mountSlot) 
				{
						%object.vehicleSlot = %mountSlot;
						%object.vehicle = %this;
						Player::setMountObject(%object, %this, %mountSlot);
						
								
					// Client::setControlObject(%client, %this);
					playSound (GameBase::getDataName(%this).mountSound, GameBase::getPosition(%this));
					BotFuncs::BotsHopOn(%client);
					$Actions::Passenger[%client] = true;
				//	messageall(1, "Vehicle Boarded. You Found a place to sit after all.");
					return;
				}
				else
					Client::sendMessage(Player::getClient(%object),0,"No slot.  Dismount the vehicle to free it for takeoff.~wError_Message.wav");
					//				messageall(1, "Hmm No there really is nowhere to sit for you.");
		// %player = Client::getOwnedObject(%object);
		%curDie = radnomItems(4, $PlayerAnim::DieHead,$PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
		Player::setAnimation(%object, %curDie);
		playNextAnim(%object);
		// AI::onDroneKilled(%aiName);
		Player::kill(%object);
//			}
//			return;
			}
		}
	}
	
	if(!Player::isAiControlled(%object))
	{
	
	%Client.InvConnect = "";	//external	
	QuickInvOff(%client);	
	%object.ZappyResupply = "";
	%client.ListType = "";
		
	if(%client != GameBase::getControlClient(%object))return;
	
	%vehname = GameBase::getDataName(%this);	
	if(GameBase::getDamageLevel(%this) < (GameBase::getDataName(%this)).maxDamage) 
	{
		if(getObjectType(%object) == "Player" && %object.vehicle == "" && (getSimTime() > %object.newMountTime || %object.lastMount != %this) && %this.fading == "")
		{
			
		%station = %object.Station;
		if(%Station)
		{
			Client::sendMessage(%client,0,"Error! Close the station first. ~waccess_denied.wav");
			return;
		}				
			%armor = Player::getArmor(%object);
			%client = Player::getClient(%object);
			
	if($IsBotPiloted[%this] != 1) 
	{
		// messageall(1, "That vehicle already has a pilot.");
	
			
			if(($VehicleUse[%armor, %vehname] & $CP) && Vehicle::canMount(%this, %object))
			{
				%weapon = Player::getMountedItem(%object,$WeaponSlot);
				if(%weapon != -1) 
				{
					%object.lastWeapon = %weapon;
					Player::unMountItem(%object,$WeaponSlot);
				}				
				
				Player::setMountObject(%object, %this, 1);	//player, vehicle, seat
				Client::setControlObject(%client, %this);
				// %player = Player::getClient(%object);
				// %player = Client::getOwnedObject(%object);
				// %clientId = Player::getClient(%player);

				%client = Player::getclient(%object);
				// %cl = GameBase::getControlClient(%this);
				if(GameBase::getTeam(%this) != (GameBase::getTeam(%object)))
				{
					$TeamItemCount[GameBase::getTeam(%this) @ $VehicleToItem[GameBase::getDataName(%this)]]--;
					Client::sendMessage(Player::getClient(%object),0,"Enemy vehicle hijacked. It's now your teams. ~wError_Message.wav");
					GameBase::setTeam(%this,GameBase::getTeam(%object));
				}				

				// GameBase::setTeam(%this,GameBase::getTeam(%object));
				%this.smoking = true;
				%this.selfdestructing = false;
				%cl = Player::getclient(%object);
				%cl.hasmessage = true;
				%cl.hasjumpermessage = true;
				Interceptor2::smoke(%this);
				playSound (GameBase::getDataName(%this).mountSound, GameBase::getPosition(%this));
				%object.driver= 1;
				%object.vehicle = %this;
				%this.clLastMount = %client;
			}
			else if($VehicleSlots[%vehname] == 0)
				Client::sendMessage(Player::getClient(%object),0,"Cannot pilot this vehicle in your current armor class.~wError_Message.wav");
			else if($VehicleUse[%armor, %vehname] & $CR)
			{
				%mountSlot = Vehicle::findEmptySeat(%this,%client);
				if(%mountSlot) 
				{
					%object.vehicleSlot = %mountSlot;
					%object.vehicle = %this;
					Player::setMountObject(%object, %this, %mountSlot);
					playSound (GameBase::getDataName(%this).mountSound, GameBase::getPosition(%this));
					%cl = Player::getclient(%object);
					%cl.hasmessage = true;
					%cl.hasjumpermessage = true;
					%this.selfdestructing = false;
				}
				else
					Client::sendMessage(Player::getClient(%object),0,"No slot.  Dismount the vehicle to free it for takeoff.~wError_Message.wav");
					if ($Spoonbot::AutoSpawn)
					{
						Client::sendMessage(Player::getClient(%object),0,"An AI vomited in this seat and it is unsanitary.~wError_Message.wav");						
					}
			}
			else if(GameBase::getControlClient(%this) == -1)
				Client::sendMessage(Player::getClient(%object),0,"You must be in Light Armor to pilot the vehicles.~wError_Message.wav");
	}
	if($IsBotPiloted[%this]) 
	{
				if($VehicleSlots[%vehname] == 0)
				Client::sendMessage(Player::getClient(%object),0,"Cannot pilot this vehicle in your current armor class.~wError_Message.wav");
			else if($VehicleUse[%armor, %vehname] & $CR)
			{
				%mountSlot = Vehicle::findEmptySeat(%this,%client);
				if(%mountSlot) 
				{
					Client::sendMessage(Player::getClient(%object),1,"AI ride detected. Riding shotgun. ~wError_Message.wav");
					Client::sendMessage(Player::getClient(%object),1,"If AI ride is in the way of the vehicle pad destroy it!");
					%object.vehicleSlot = %mountSlot;
					%object.vehicle = %this;
					Player::setMountObject(%object, %this, %mountSlot);
					playSound (GameBase::getDataName(%this).mountSound, GameBase::getPosition(%this));
					%cl = Player::getclient(%object);
					%cl.hasmessage = true;
					%cl.hasjumpermessage = true;
					%this.selfdestructing = false;
				}
				else
					Client::sendMessage(Player::getClient(%object),0,"No slot.  Dismount the vehicle to free it for takeoff.~wError_Message.wav");
			}
			else if(GameBase::getControlClient(%this) == -1)
				Client::sendMessage(Player::getClient(%object),0,"You must be in Light Armor to pilot the vehicles.~wError_Message.wav");		
	}
		}
	}
	}
}

function startDestruction(%this) 
{
	$GettingDestroyed[%this] = true;
	if($DestructionTime[%this] == 0) 
	{
		$DestructionTime[%this] = 240;
		checkDestructionTime(%this);
	}
	else
		$DestructionTime[%this] = 240;
		checkDestructionTime(%this);
}

function checkDestructionTime(%this) 
{
	if($DestructionTime[%this] > 0) 
	{
				if(%this.selfdestructing == "false")
		{
			return;	
		}
		if(GameBase::getDamageState(%this) == Destroyed) 
		{
			return;
		}
		$DestructionTime[%this] -= 60;
		schedule("checkDestructionTime(" @ %this @ ");", 60, %this);
		return;
	}
	if($DestructionTime[%this] <= 0)
	{
		if(%this.selfdestructing == "false")
		{
			return;	
		}
		if(GameBase::getDamageState(%this) == Destroyed) 
		{
			return;
		}
		GameBase::setDamageLevel(%this,GameBase::getDataName(%this).maxDamage);
		return;
	}
	else
				if(%this.selfdestructing == "false")
		{
			return;	
		}
		if(GameBase::getDamageState(%this) == Destroyed) 
		{
			return;
		}
	return;
}

function Interceptor2::smoke(%this)
{
	
	if($MDESC::Type == "CTF BOTS")
	{
		return;
	}
	
	if(GameBase::getDataName(%this) == "Interceptor")
{
 	%control = GameBase::getControlClient(%this);
	if(%control == -1)	
	{
		return;		
	}

	if(%this.smoking == "false")
	{
		return;	
	}

	if(GameBase::getDamageState(%this) == Destroyed)
	{
		return;	
	}


	%pos1 = vector::add(gamebase::getposition(%this),"0 0 0");
	%vel = Item::getVelocity(%this);
	if(vector::getdistance(%vel,"0 0 0") > 20.5) // 10
	Projectile::SpawnProjectile("JetSmoke","0 0 0 0 0 0 0 0 0 "@%pos1, %this, %vel);

//	if(vector::getdistance(%vel,"0 0 0") > 80.0) // 10
//	{
//	Projectile::spawnProjectile("AnnihilationFlame","0 0 0 0 0 0 0 0 0 "@%pos1, %this, %vel);
//	}
	schedule("Interceptor2::smoke("@%this@");",0.05); // 0.05
	return;
}

	if(GameBase::getDataName(%this) == "LAPC")
{
 	%control = GameBase::getControlClient(%this);
	if(%control == -1)	
	{
		return;		
	}

	if(%this.smoking == "false")
	{
		return;	
	}

	if(GameBase::getDamageState(%this) == Destroyed)
	{
		return;	
	}

	// %pos1 = vector::add(gamebase::getposition(%this),"-3.5 0 -10.5"); // -10.5
	// %pos2 = vector::add(gamebase::getposition(%this),"3.5 0 10.5"); // 10.5
	%pos1 = vector::add(gamebase::getposition(%this),"-2.0 0 0");
	%pos2 = vector::add(gamebase::getposition(%this),"2.0 0 0");
	%vel = Item::getVelocity(%this);
	if(vector::getdistance(%vel,"0 0 0") > 20.5) // 10
	{
	Projectile::SpawnProjectile("JetSmoke","0 0 0 0 0 0 0 0 0 "@%pos1, %this, %vel);
	Projectile::SpawnProjectile("JetSmoke","0 0 0 0 0 0 0 0 0 "@%pos2, %this, %vel);
	}
	schedule("Interceptor2::smoke("@%this@");",0.05); // 0.05
	return;
}

	if(GameBase::getDataName(%this) == "Transport")
{
 	%control = GameBase::getControlClient(%this);
	if(%control == -1)	
	{
		return;		
	}

	if(%this.smoking == "false")
	{
		return;	
	}

	if(GameBase::getDamageState(%this) == Destroyed)
	{
		return;	
	}

	%pos1 = vector::add(gamebase::getposition(%this),"-4.3 0 0");
	%pos2 = vector::add(gamebase::getposition(%this),"4.3 0 0");
	%vel = Item::getVelocity(%this);
	if(vector::getdistance(%vel,"0 0 0") > 20.5) // 10
	{
	Projectile::SpawnProjectile("JetSmoke","0 0 0 0 0 0 0 0 0 "@%pos1, %this, %vel);
	Projectile::SpawnProjectile("JetSmoke","0 0 0 0 0 0 0 0 0 "@%pos2, %this, %vel);
	}
	schedule("Interceptor2::smoke("@%this@");",0.05); // 0.05
	return;
}

	if(GameBase::getDataName(%this) == "Scout")
{
 	%control = GameBase::getControlClient(%this);
	if(%control == -1)	
	{
		return;		
	}

	if(%this.smoking == "false")
	{
		return;	
	}

	if(GameBase::getDamageState(%this) == Destroyed)
	{
		return;	
	}

	%pos1 = vector::add(gamebase::getposition(%this),"0 0 0");
	%vel = Item::getVelocity(%this);
	if(vector::getdistance(%vel,"0 0 0") > 20.5) // 10
	Projectile::SpawnProjectile("JetSmoke","0 0 0 0 0 0 0 0 0 "@%pos1, %this, %vel);

//	if(vector::getdistance(%vel,"0 0 0") > 75.0) // 10
//	{
//	Projectile::spawnProjectile("AnnihilationFlame","0 0 0 0 0 0 0 0 0 "@%pos1, %this, %vel);
//	}
	schedule("Interceptor2::smoke("@%this@");",0.05); // 0.05
	return;
}
	return;
}


function Vehicle::findEmptySeat(%this,%client)
{
	%numSlots = $VehicleSlots[GameBase::getDataName(%this)];
	%count=0;
	for(%i=0;%i<%numSlots;%i++)  
		if(%this.Seat[%i] == "")
		{
			%slotPos[%count] = Vehicle::getMountPoint(%this,%i+2);
			%slotVal[%count] = %i+2;
			%lastEmpty = %i+2;
			%count++;
		}
	if(%count == 1)
	{
		%this.Seat[%lastEmpty-2] = %client;
		return %lastEmpty;
	}
	else if(%count > 1)
	{
		%freeSlot = %slotVal[getClosestPosition(%count,GameBase::getPosition(%client),%slotPos[0],%slotPos[1],%slotPos[2],%slotPos[3])];
		%this.Seat[%freeSlot-2] = %client;
		return %freeSlot;
	}
	else
		return "False";
}

function getClosestPosition(%num,%playerPos,%slotPos0,%slotPos1,%slotPos2,%slotPos3)
{
	%playerX = getWord(%playerPos,0);
	%playerY = getWord(%playerPos,1);
	for(%i = 0 ;%i<%num;%i++)
	{
		%x = (getWord(%slotPos[%i],0)) - %playerX;
		%y = (getWord(%slotPos[%i],1)) - %playerY;
		if(%x < 0)
			%x *= -1;
		if(%y < 0)
			%y *= -1;
		%newDistance = sqrt((%x*%x)+(%y*%y));
		if(%newDistance < %distance || %distance == "") 
		{
			%distance = %newDistance;
			%closePos = %i;
		}
	}
	return %closePos;
}

function Vehicle::passengerJump(%this,%passenger,%mom)
{
//	messageall(1, "Passenger Jump Detected.");
	%armor = Player::getArmor(%passenger);
	%cl = GameBase::getControlClient(%passenger);
	
	%height = 2;
	%velocity = 140;
	%zVec = 110;

	%pos = GameBase::getPosition(%passenger);
	%posX = getWord(%pos,0);
	%posY	= getWord(%pos,1);
	%posZ	= getWord(%pos,2);

	if(GameBase::testPosition(%passenger,%posX @ " " @ %posY @ " " @ (%posZ + %height))) 
	{		
		%client = Player::getClient(%passenger);
		%this.Seat[%passenger.vehicleSlot-2] = "";
		%passenger.vehicleSlot = "";
		%passenger.vehicle= "";
		Player::setMountObject(%passenger, -1, 0);
		%rotZ = getWord(GameBase::getRotation(%passenger),2);
		GameBase::setRotation(%passenger, "0 0 " @ %rotZ);
		GameBase::setPosition(%passenger,%posX @ " " @ %posY @ " " @ (%posZ + %height));
		%jumpDir = Vector::getFromRot(GameBase::getRotation(%passenger),%velocity,%zVec);
		Player::applyImpulse(%passenger,%jumpDir);
		//	messageall(1, "Passenger Jump Impulse Applied.");
	   if (Player::isAIControlled(%cl)) //Is this a bot?
       {
		$IsBotPiloted[%this] = false;
		//	messageall(1, "The Passenger is an AI.");
	   }
	
	}
	else
			if(%cl.hasmessage)
			{
				%cl.hasmessage = false;
				Client::sendMessage(%cl,0,"Can not dismount - Obstacle in the way.~wError_Message.wav");
				schedule(%cl@".hasmessage = true;",1.0,%cl);
			}
		// Client::sendMessage(Player::getClient(%passenger),0,"Can not dismount - Obstacle in the way.~wError_Message.wav");
}

function Vehicle::pilotJump(%this,%passenger,%mom)
{
	%armor = Player::getArmor(%passenger);
	%cl = GameBase::getControlClient(%passenger);
	
	%height = 2;
	%velocity = 140;
	%zVec = 110;

	%pos = GameBase::getPosition(%passenger);
	%posX = getWord(%pos,0);
	%posY	= getWord(%pos,1);
	%posZ	= getWord(%pos,2);

	if(GameBase::testPosition(%passenger,%posX @ " " @ %posY @ " " @ (%posZ + %height))) 
	{
		%client = Player::getClient(%passenger);
		%this.Seat[%passenger.vehicleSlot-2] = "";
		%passenger.vehicleSlot = "";
		%passenger.vehicle= "";
		Player::setMountObject(%passenger, -1, 0);
		%rotZ = getWord(GameBase::getRotation(%passenger),2);
		GameBase::setRotation(%passenger, "0 0 " @ %rotZ);
		GameBase::setPosition(%passenger,%posX @ " " @ %posY @ " " @ (%posZ + %height));
		%jumpDir = Vector::getFromRot(GameBase::getRotation(%passenger),%velocity,%zVec);
		Player::applyImpulse(%passenger,%jumpDir);
	   if (Player::isAIControlled(%cl)) //Is this a bot?
       {
		$IsBotPiloted[%this] = false;
	   }
	
	}
	else
			if(%cl.hasmessage)
			{
				%cl.hasmessage = false;
				Client::sendMessage(%cl,0,"Can not dismount - Obstacle in the way.~wError_Message.wav");
				schedule(%cl@".hasmessage = true;",1.0,%cl);
			}
		// Client::sendMessage(Player::getClient(%passenger),0,"Can not dismount - Obstacle in the way.~wError_Message.wav");
}

function Vehicle::jump(%this,%mom)
{
	%cl = GameBase::getControlClient(%this);
				if(%cl.hasjumpermessage)
			{
				%cl.hasjumpermessage = false;
				schedule(%cl@".hasjumpermessage = true;",0.5,%cl);
				Vehicle::dismount(%this,%mom);
			}
}

function Vehicle::dismount(%this,%mom)
{
	
//	messageall(1, "Vehicle Dismount Main Function Ran.");
	
	%vel = Item::getVelocity(%this);
	%cl = GameBase::getControlClient(%this);
	
				// BotsHopOff(%cl);
				// BotsHopOff(%pl);				
				// schedule("BotsHopOff("@%cl@");", 1);
		
	if(%cl != -1)
	{
		%pl = Client::getOwnedObject(%cl);
		%height = 10;
		%height2 = 7;
		%height3 = 4;
		%pos = GameBase::getPosition(%pl);
		%posX = getWord(%pos,0);
		%posY	= getWord(%pos,1);
		%posZ	= getWord(%pos,2);
		%velX = getWord(%vel,0);
		%velY = getWord(%vel,1);
		%velZ = getWord(%vel,2);
		if(getObjectType(%pl) == "Player")
		{	

//					if(%velX >= 10 || %velY >= 10 || %velZ >= 10)
						if((%velX >= 10) || (%velY >= 10) || (%velZ >= 10))
		{
							if(!GameBase::testPosition(%pl,%posX @ " " @ %posY @ " " @ (%posZ + %height))) 
						{
							Client::sendMessage(%cl,0,"Can not dismount - Obstacle in the way.~wError_Message.wav");
							return;
						}
							if(!GameBase::testPosition(%pl,%posX @ " " @ %posY @ " " @ (%posZ + %height2))) 
						{
							Client::sendMessage(%cl,0,"Can not dismount - Obstacle in the way.~wError_Message.wav");
							return;
						}
							if(!GameBase::testPosition(%pl,%posX @ " " @ %posY @ " " @ (%posZ + %height3))) 
						{
							Client::sendMessage(%cl,0,"Can not dismount - Obstacle in the way.~wError_Message.wav");
							return;
						}
		}	
			// dismount the player	  
			if(GameBase::testPosition(%pl, Vehicle::getMountPoint(%this,0)))
			{
				if ($Spoonbot::AutoSpawn)
				{
				BotsHopOff(%cl);
				}
				%pl.lastMount = %this;
				%pl.newMountTime = getSimTime() + 3.0;
				Player::setMountObject(%pl, %this, 0);
				Player::setMountObject(%pl, -1, 0);
				%rot = GameBase::getRotation(%this);
				%rotZ = getWord(%rot,2);
				%velX = getWord(%vel,0);
				%velY = getWord(%vel,1);
				%velZ = getWord(%vel,2); // 2

//		if(%velX >= 10 || %velY >= 10 || %velZ >= 10) 
		if((%velX >= 10) || (%velY >= 10) || (%velZ >= 10)) 
		{
				// BotsHopOff(%cl);
				// BotsHopOff(%pl);
				
			%velocity = 8.5 * pow((%velX*%velX + %velY*%velY), 0.5); // 6 7.5
			%height = 8; // 15 10
			%zVec = %velZ + 2; //170;

			%jumpDir = Vector::getFromRot(GameBase::getRotation(%this),%velocity,%zVec);
			%jumpDir = Vector::add(%jumpDir, %vel);

			%pos = GameBase::getPosition(%this);

			%posX = getWord(%pos,0);
			%posY	= getWord(%pos,1);
			%posZ	= getWord(%pos,2);
			GameBase::setPosition(%pl,%posX @ " " @ %posY @ " " @ (%posZ + %height));
			playSound (GameBase::getDataName(%this).dismountSound, GameBase::getPosition(%this));
			GameBase::setRotation(%pl, "0 0 " @ %rotZ);
			%this.smoking = false;
			%this.selfdestructing = true;
			$DestructionTime[%this] = 240;
			
				if(%pl.lastWeapon != "")
				{
					Player::useItem(%pl,%pl.lastWeapon);
					%pl.lastWeapon = "";
				}
				//
				if((%pl.outArea == 1) && (!$build) && (!$ANNIHILATION::OutOfArea)) 
				{
						// %vehicle = Player::getMountObject(%pl);
						GameBase::setDamageLevel(%this,GameBase::getDataName(%this).maxDamage);	
						%this.smoking = false;
						%this.selfdestructing = false;
						%pl.driver = "";
						%pl.vehicle = "";
						return;
				}
				//
				%pl.driver = "";
				%pl.vehicle = "";
				if($GettingDestroyed[%this])
				 {
					 $DestructionTime[%this] = 240;
				 }
				else if(!$GettingDestroyed[%this])
				 {
					startDestruction(%this);
				 }
				Player::applyImpulse(%pl,%jumpDir);
				Client::setControlObject(%cl, %pl);
		}
//		if(%velX < 10 && %velY < 10 && %velZ < 10) 
			if((%velX < 10) && (%velY < 10) && (%velZ < 10)) 
		{
				// BotsHopOff(%cl);
				// BotsHopOff(%pl);
				
			%jumpDir = %mom;

				Player::applyImpulse(%pl,%jumpDir);
				Client::setControlObject(%cl, %pl);
				playSound (GameBase::getDataName(%this).dismountSound, GameBase::getPosition(%this));
				GameBase::setRotation(%pl, "0 0 " @ %rotZ);
				%this.smoking = false;
				%this.selfdestructing = true;
				$DestructionTime[%this] = 240;
				
				if(%pl.lastWeapon != "")
				{
					Player::useItem(%pl,%pl.lastWeapon);
					%pl.lastWeapon = "";
				}
				//
				//	if(%pl.outArea == 1) 
				if((%pl.outArea == 1) && (!$build) && (!$ANNIHILATION::OutOfArea)) 
				{
						// %vehicle = Player::getMountObject(%pl);
						GameBase::setDamageLevel(%this,GameBase::getDataName(%this).maxDamage);	
						%this.smoking = false;
						%this.selfdestructing = false;
						%pl.driver = "";
						%pl.vehicle = "";
						return;
				}
				//
				%pl.driver = "";
				%pl.vehicle = "";
				// HopOut(%pl);
				
		 // if(%this.hasexpmessage)
		 // {
				 if($GettingDestroyed[%this])
				 {
					 $DestructionTime[%this] = 240;
					 // return;
				 }
				else if(!$GettingDestroyed[%this])
				 {
				// %this.hasexpmessage = false;
				// schedule(%this@".hasexpmessage = true;",30.0,%this);	
				// schedule("Interceptor2::destruct("@%this@");",20); // 0.05 240
				startDestruction(%this);
				 }
//				else
		// }
		}
			}
			else
			if(%cl.hasmessage)
			{
				%cl.hasmessage = false;
				Client::sendMessage(%cl,0,"Can not dismount - Obstacle in the way.~wError_Message.wav");
				schedule(%cl@".hasmessage = true;",1.0,%cl);
			}	
		}
	}
}

function Vehicle::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	if($debug::Damage)
	{
		echo("Vehicle::onDamage("@%this@", "@%type@", "@%value@", "@%pos@", "@%vec@", "@%mom@", "@%object@" )");
	}	
	
	if(GameBase::getDamageState(%this) == Destroyed) 
		return;

	if(%Value == 0)
		%value = 0.25;
//	{
//		//GameBase::setDamageLevel(%this,2);
//		echo("!! No "@GameBase::getDataName(%this)@" Damage, ERROR! cl# "@%this.clLastMount);
//		return;	
//	}
		
	%damageLevel = GameBase::getDamageLevel(%this);	
	%dValue = %damageLevel + %Value;
	GameBase::setDamageLevel(%this,%dValue);
}

function Vehicleold::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	if(GameBase::getDamageState(%this) == Destroyed) 
		return;

	if($debug)
	{
		%data = GameBase::getDataName(%this);
		%velocity = vector::getdistance(%vec,"0 0 0");		
		echo("!!Vehicle::onDamage "@%this@" damaged by "@GameBase::getDataName(%object)@" object ="@%object@" Vel. "@%velocity@" type ="@%type@" pos="@%pos@" vec="@%vec@" mom="@%mom);
	}
	if(%type == -1)
	{
		%control = GameBase::getControlClient(%this);
		echo(%this@" "@GameBase::getDataName(%this)@" cl# "@%control@" IMPACT!!  %value ="@%value);
		if(%Value == 0)
			%Value = 0.05;
	}	
	%damageLevel = GameBase::getDamageLevel(%this);	
	//%Value *= $damageScale[GameBase::getDataName(%this), %type];
	%dValue = %damageLevel + %Value;
	GameBase::setDamageLevel(%this,%dValue);

}

function Vehicle::getHeatFactor(%this)
{
	// Not getting called right now because turrets don't track
	// vehicles.  A hack has been placed in Player::getHeatFactor.
	return 1.0;
}

function Vehicle::DeployArea(%player,%shape,%pos)
{
	echo("!! Check vehicle deploy area");
	%client = Player::getClient(%player);
	%Ppos = GameBase::getPosition(%player);
	%rot = gamebase::getrotation(%player);
	gamebase::setposition(%player,vector::add(%Ppos, "0 0 100"));	//moving player temporarily out of way
	%objVehicle = newObject("",flier,%shape,true);
	GameBase::setRotation(%objVehicle, %rot);
	addToSet("MissionCleanup", %objVehicle); 

	if(!GameBase::testPosition(%objVehicle, vector::add(%pos,"0 0 5.75"))) // 0 0 1.75
	{ 
		Client::sendMessage(%client,0,"Vehicle will not fit there.");
		deleteObject(%objVehicle);
		gamebase::setposition(%player,%Ppos);
		return false;
	}
	else
	{
		deleteObject(%objVehicle);
		gamebase::setposition(%player,%Ppos);
		return true;	
	}
}

function Vehicle::TerrainCheck(%object)
{
	%pos = getBoxCenter(%object);
	%object.Lpos = %pos;
	schedule("Vehicle::Checkpos(" @ %object @ ");",0.1,%object); // Fix?				
}

function Vehicle::Checkpos(%object)
{	
	%pos = getBoxCenter(%object);
	%pos3 = getWord(%pos,2);
	%Lpos = %object.Lpos;
	%Lpos3 = getWord(%Lpos,2);
	%object.Lpos = "";
	//echo("vehiclepos/ lastpos ",%pos," ",%Lpos);
	if(%pos3 -2 > %Lpos3)
	{
		GameBase::setPosition(%object,%Lpos);
		Item::setVelocity(%object, 0);
		GameBase::setDamageLevel(%object,2);		
	}		
}

function GrabAss(%client,%passenger)
{
	%player = Client::getOwnedObject(%passenger);
	%vehicle = Client::getOwnedObject(%client);
	Player::setMountObject(%player, %vehicle, 1);	//player, vehicle, seat
}
