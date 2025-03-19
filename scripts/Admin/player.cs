$PlayerAnim::Crouching = 25;
$PlayerAnim::DieChest = 26;
$PlayerAnim::DieHead = 27;
$PlayerAnim::DieGrabBack = 28;
$PlayerAnim::DieRightSide = 29;
$PlayerAnim::DieLeftSide = 30;
$PlayerAnim::DieLegLeft = 31;
$PlayerAnim::DieLegRight = 32;
$PlayerAnim::DieBlownBack = 33;
$PlayerAnim::DieSpin = 34;
$PlayerAnim::DieForward = 35;
$PlayerAnim::DieForwardKneel = 36;
$PlayerAnim::DieBack = 37;

//----------------------------------------------------------------------------
$CorpseTimeoutValue = 22;
//----------------------------------------------------------------------------

// Player & Armor data block callbacks

function Player::onAdd(%this)
{
	DebugFun("Player::onAdd",%this);
	GameBase::setRechargeRate(%this,8);
	GameBase::setAutoRepairRate(%this,0);
}

function Player::onRemove(%this)
{
	DebugFun("Player::onRemove",%this);
	// Player::trigger(%this,$WeaponSlot,false);
	// Drop anything left at the players pos
	if(%this.repairTarget && %this.repairTarget != -1) 
	{
		%rate = GameBase::getAutoRepairRate(%object) - %this.repairRate;
		if(%rate < 0)
			%rate = 0;
		GameBase::setAutoRepairRate(%object,%rate);
	}
	for(%i = 0; %i < 8; %i = %i + 1)
	{
		%type = Player::getMountedItem(%this,%i);
		if(%type != -1)
		{
			// Note: Player::dropItem ius not called here.
			%item = newObject("","Item",%type,1,false);
			schedule("Item::Pop(" @ %item @ ");", $ItemPopTime, %item);
			addToSet("MissionCleanup", %item);
			GameBase::setPosition(%item,GameBase::getPosition(%this));
		}
	}
}

function Player::onNoAmmo(%player,%imageSlot,%itemType)
{
	//Anni::Echo("No ammo for weapon ",%itemType.description," slot(",%imageSlot,")");
}

function Player::onKilled(%this)
{
	if($debug)
		echo("Player::onKilled("@%this@")");
	DebugFun("Player::onKilled",%this);
	%this.Station = "";
	%cl = GameBase::getOwnerClient(%this);
	%player = Client::getOwnedObject(%this);
	%cl.dead = 1;

	if(%player.repackEnergy != "")
	{
	%player.repackDamage = "";
	%player.repackEnergy = "";
	}
	
	if($AutoRespawn > 0)
		schedule("Game::autoRespawn(" @ %cl @ ");",$AutoRespawn,%cl);
	if(%this.outArea==1)	
		leaveMissionAreaDamage(%cl);
//	Player::setDamageFlash(%this,0.75);
	Player::trigger(%this,$WeaponSlot,false);
	for(%i = 0; %i < 8; %i = %i + 1)
	{
		%type = Player::getMountedItem(%this,%i);
		if(%type != -1)
		{
			if(%i != $WeaponSlot || !Player::isTriggered(%this,%i) || getRandom() > "0.2") 
				Player::dropItem(%this,%type);
			if(%i == $WeaponSlot && %type == BabyNukeMortar)
				Player::trigger(%this,$WeaponSlot,false);
		}
	}

	if(%cl != -1)
	{
		if(%this.vehicle != "")	
		{
			if(%this.driver != "") 
			{
				%this.driver = "";
				%this.vehicle.Pilot = "";
				Client::setControlObject(Player::getClient(%this), %this);
				Player::setMountObject(%this, -1, 0);
			}
			else 
			{
				%this.vehicle.Seat[%this.vehicleSlot-2] = "";
				%this.vehicleSlot = "";
			}
			%this.vehicle = "";
		}
		schedule("GameBase::startFadeOut(" @ %this @ ");", $CorpseTimeoutValue, %this);
		Client::setOwnedObject(%cl, -1);
		Client::setControlObject(%cl, Client::getObserverCamera(%cl));
		Observer::setOrbitObject(%cl, %this, 5, 5, 5);
		schedule("deleteObject(" @ %this @ ");", $CorpseTimeoutValue + 2.5, %this);
		//echo("whooooo comes first???????? Player");
		if(%cl.inArenaTD && $ArenaTD::Active || %cl.inArena && $Arena::Winners)
			%cl.observerMode = "observerOrbit"; //observerFly //Observer::enterObserverMode(%clientId);
		else
			%cl.observerMode = "dead"; //observerOrbit
		%cl.AdminobserverMode = "";
		%cl.dieTime = getSimTime();
	}
}

// front_left
// front_right
// back_left
// back_right
// middle_middle
// middle_front
// middle_back
// left_middle
// right_middle


function Player::onDamage(%this,%type,%value,%pos,%vec,%mom,%vertPos,%quadrant,%object)
{
	DebugFun("Player::onDamage",%this,%type,%value,%pos,%vec,%mom,%vertPos,%quadrant,%object);
	if ( $debug::Damage )
		Anni::Echo("Player::onDamage("@%this@", "@%type@", "@%value@", "@%pos@", "@%vec@", "@%mom@", "@%vertPos@", "@%quadrant@", "@%object@")");

	if(%type == $NullDamageType || !Player::isExposed(%this) || Player::isDead(%this) || %value <= 0)
		return;

	%damagedClient = Player::getClient(%this);

	//if ( %damagedClient == "" || %damagedClient == "-1" || getObjectType(%damagedClient) != "Net::PacketStream" ) //this seems to crash the server everytime an AI was spawned
	//{
	//	Anni::Echo("What the hell call! "@%damagedClient@" "@%this@"....");
	//	deleteObject(%this);
	//	return;
	//}
		
	if ( !%object && %damagedClient.AIkiller )
		%object = %damagedClient.AIkiller;

	%targetTeam = GameBase::getTeam(Player::getClient(%this));

	if ( !%object )
	{
		if ( %targetTeam == 1 )
			%shooterTeam = 0;
		else
			%shooterTeam = 1;
	}
	else
		%shooterTeam = GameBase::getTeam(%object);

	%shooterClient = %object;

	%armor = Player::getArmor(%this);
	
	// new death666
//	%damagedClient = Player::getClient(%this);
//	%targetTeam = GameBase::getTeam(Player::getClient(%this));
//	%shooterTeam = GameBase::getTeam(%shooterClient);
//	%shooterObject = Client::getOwnedObject(%shooterClient);  
	// Check for wankers, ignore laser damage as it won't damage past visible anyhow.
//	if(%type != 6 && %shooterObject != -1)
//	{
//		%shooterPos = gamebase::getposition(%shooterObject);
//		%targetPos = gamebase::getposition(%this);
//		%hitDist = vector::getdistance(%shooterPos,%targetPos);
//		if(%hitDist > $visDistance)
//		messageall(0,Client::getName(%shooterClient)@" hit "@Client::getName(%damagedClient)@" "@floor(%hitDist - $visDistance) @"m past visible distance. Type = "@%type);		
//	}	
	// end new death666
	
	%now = getSimTime(); //AFK System 
	%damagedClient.lastActiveTimestamp = %now; //AFK System
	if (%shooterClient != -1) //AFK System 
		%shooterClient.lastActiveTimestamp = %now; //AFK System
	
	if(%shooterClient.inArena || %damagedClient.inArena) //just get rid of this and use arena jug
		%this.invulnerable = false;
	
	if(%this.invulnerable || %damagedClient.arenajug || $Annihilation::NoPlayerDamage || %this.frozen || $jailed[%this])
	{	
		// no damage, just play a shield.
		 %thisPos = getBoxCenter(%this);
		 %offsetZ = ((getWord(%pos,2))-(getWord(%thisPos,2)));
		 GameBase::activateShield(%this,%vec,%offsetZ);
		return;
	}

	if(%type == $LandingDamageType && $ArmorName[%armor] != iarmorLightArmor && $ArmorName[%armor] != iarmorMercenary) //lol forgot about this for base / elite 
	{
		if( ( getSimTime() - %this.BoostTime ) < 2 )
		{
			%object = %this.LastBoost;
			%type = $ForkImpact;
		}	
		%vel = Item::getVelocity(%this);	
		%speed = vector::getdistance(%vel,"0 0 0");
		%speedChange = vector::getdistance(%speed,%this.lastSpeed);
		%this.lastSpeed = %speed;
		%this.lastVel = %vel;
		%this.lastImpact  = getSimTime();
			
		if(%speedchange > 20)
		{	
			Player::setAnimation(Client::getOwnedObject(%this),28);		
			GameBase::playSound(%this, SoundLandOnGround, 0);	
		}
		%value = %value - 0.12;
		if( %value <= 0 )
			return;
	}
	
	if(%type == $LandingDamageType && $ArmorName[%armor] == iarmorLightArmor && %damagedClient.inArena)
	{
		%dmg = floor(100 - (gamebase::getdamagelevel(%this) / 0.66) * 100) ;
		%dmg2 = 100 - floor(100 - (%value / 0.66) * 100) ;

		if(%type == $LandingDamageType && %dmg2 < 17 && %dmg < 17 && %dmg2*1.5 > %dmg)
		{
			return;
		}
	}
	
	%station = %this.Station;
	if(%targetTeam == %shooterTeam && %station && %type == $ExplosionDamageType)
	{
		%shooterObject = Client::getOwnedObject(%object);		
		Player::trigger(%shooterObject, $WeaponSlot, false);
		%weaponType = Player::getMountedItem(%shooterObject,$WeaponSlot);
		if(%weaponType == DiscLauncher)
		{
			Player::unmountItem(%shooterObject,$WeaponSlot);
			//Player::dropItem(%shooterObject,%weaponType);		
			Client::sendMessage(%shooterClient,0,"~wfemale2.wdsgst2.wav");
			schedule("client::sendmessage("@%shooterClient@",0,\"~wfemale1.wwshoot.wav\");",0.8, %shooterClient);
			return;
		}
		
	}
	else if ( %type != $LandingDamageType )
	{
		%mult = $ArmorKickback[$ArmorName[%armor]];
		if(%mult != 0)
		{	
			%mom = (getWord(%mom, 0) * %mult) @ " " @ (getWord(%mom, 1) * %mult) @ " " @ (getWord(%mom, 2) * %mult);
		}
		Player::applyImpulse(%this,%mom);
	}

	if(%type == $MineDamageType && ($ArmorName[%armor] == iarmorSpy || $ArmorName[%armor] == iarmorNecro))	
	{
		%energy = GameBase::getEnergy(%this);	
		GameBase::setEnergy(%this,%energy - 40);
	}

	if(%type == 6)
	{
		%shooterObject = Client::getOwnedObject(%object);
		%weaponType = Player::getMountedItem(%shooterObject,$WeaponSlot);
		if(%weaponType == CuttingLaser)
		{	
			return;	
		}		
	}	

	%InAir = Player::getLastContactCount(%damagedClient);	//time in air

// Start new big ass function
	        if(Player::isAIControlled(%this))
{

	if(%type == $StasisDamageType)
	{
		// Do Nothing
	}
	
//	if(%type == $ImpactDamageType) 
//	{
//		return;
//	}

	%friendFire = $Server::TeamDamageScale;

	if(%type == $DisarmDamageType) 
	{
	Player::trigger(%this, $WeaponSlot, false);
	%weaponType = Player::getMountedItem(%this,$WeaponSlot);
	if(%weaponType != -1)
	Player::dropItem(%this,%weaponType);
	Client::sendMessage(Player::getClient(%damagedClient),0,"Your have been hit by a disarm spell!");
	}

	if(Player::getClient(%this) == %object)
	{	
		if(%type == $PlasmaDamageType)
			schedule(%armor @ "::onBurn(" @ %damagedClient @ ", " @ %this @ ");",0.01,%damagedClient);							
		else if(%type == $ShockDamageType)
			schedule(%armor @ "::onShock(" @ %damagedClient @ ", " @ %this @ ");",0.01,%damagedClient);
		else if(%type == $PoisonDamageType && !$Annihilation::NoPlayerDamage)
			schedule(%armor @ "::onPoison(" @ %damagedClient @ ", " @ %this @ ");",0.01,%damagedClient);
	}


	if(%type == $ImpactDamageType && Client::getTeam(%object.clLastMount) == Client::getTeam(%damagedClient)) 
	%friendFire = $Server::TeamDamageScale;
	else
	%friendFire = 1.0;

	if(%this.isDuck == 1 && %shooterClient != %damagedClient && Client::getName(%shooterClient) != "")
	{
			Player::blowUp(%this);
			Player::Kill(%this);
			GameBase::PlaySound(%this, shockExplosion, 0);
			ArenaMSG(0, Client::getName(%shooterClient) @ " nailed a duck, shot position: " @ %vertPos @ " " @ %quadrant @ ".");
			%this.deadDuck = 1;			
			%shooterClient.score++; //no credits or kills for ducks
			Game::refreshClientScore(%shooterClient);
			return;
	}

	if(%this.isDuelDuck == 1 && %shooterClient != %damagedClient && Client::getName(%shooterClient) != "")
	{
			GameBase::PlaySound(%this, SoundPDAButtonSoft, 0); // shockExplosion
			// ArenaMSG(0, Client::getName(%shooterClient) @ " hit a bot, shot position: " @ %vertPos @ " " @ %quadrant @ ".");
			// Client::sendMessage(%shooterClient,0,"Successful hit on bot: " @ %vertPos @ " " @ %quadrant @ ".");
			bottomprint(%shooterClient, "<f1><jc>Successful hit on bot: <f2>" @ %vertPos @ " " @ %quadrant @ ".");
	}

	if(%vertPos == "head" && %type == $LaserDamageType) 
	{
		if(%armor == "armorTroll" || %armor == "armorTank" ||  %armor == "armorTitan") 	
		{
			if(%quadrant == "middle_back" || %quadrant == "middle_front" || %quadrant == "middle_middle") 
			{
				%value += (%value * 0.45);	//0.3
			}
		}
		else 
		{
			%value += (%value * 0.55);	//0.3
		}
	}

	if(%type == $SniperDamageType) 
	{
		if(%vertPos == "head")
		{
			if(%armor == "armorTroll" || %armor == "armorTank" ||  %armor == "armorTitan") 
			{
				if(%quadrant == "middle_back" || %quadrant == "middle_front" || %quadrant == "middle_middle") 
				{
					%value += (%value * 0.3);	//head shot to heavy
				}
			}
			else 
			{
				%value += (%value * 0.35);	//med/ light head shots
			}
		}
		else 
			%value += (%value * 0.27);// adding 27% to any other shot
	}			
		
		
	//If Shield Pack is on
	if(%type != -1 && %this.shieldStrength ) 
	{
		%energy = GameBase::getEnergy(%this);
		%strength = %this.shieldStrength;
		if(%type == $ShrapnelDamageType || %type == $MortarDamageType)
			%strength *= 0.75;
		if(%type == $ElectricityDamageType)
			%strength *= 0.0;
		%absorb = %energy * %strength;
		if(%value < %absorb) 
		{
			if ( %type != $LandingDamageType )
			{
				GameBase::setEnergy(%this,%energy - ((%value / %strength)*%friendFire));
				%thisPos = getBoxCenter(%this);
				%offsetZ =((getWord(%pos,2))-(getWord(%thisPos,2)));
				GameBase::activateShield(%this,%vec,%offsetZ);
			}
			%value = 0;
		}
		else 
		{
			GameBase::setEnergy(%this,0);
			%value = %value - %absorb;
		}
	}
	%value = $DamageScale[%armor, %type] * %value * %friendFire;

	%sameTeam = (%damagedteam == %shooterteam);
	//update player score here.  no points for TKs
	// elite score stuff 
	if($TALT::Active == true || %shooterClient.inArena && %damagedClient.inArena) //New LT code
	{
		if(%type == $ExplosionDamageType && %damagedClient != %shooterClient)
		{
			if(%shooterClient.inArena && %damagedClient.inArena && !$ArenaTD::Active) //reworked for arena 
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.DiscDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			else if($TA::Rabbit && !%shooterClient.inArena)
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.DiscDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			else if(Client::getTeam(%damagedClient) != Client::getTeam(%shooterClient))
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.DiscDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			
		}
		if(%type == $ShrapnelDamageType && %damagedClient != %shooterClient)
		{
			if(%shooterClient.inArena && %damagedClient.inArena && !$ArenaTD::Active)
			{
							
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.NadeDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			else if($TA::Rabbit && !%shooterClient.inArena)
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.NadeDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			else if(Client::getTeam(%damagedClient) != Client::getTeam(%shooterClient))
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.NadeDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
		}
		if(%type == $BulletDamageType && %damagedClient != %shooterClient)
		{
			if(%shooterClient.inArena && %damagedClient.inArena && !$ArenaTD::Active)
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.ChainDamage += %value;
				Game::refreshClientScore(%shooterClient);	
			}
			else if($TA::Rabbit && !%shooterClient.inArena)
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.ChainDamage += %value;
				Game::refreshClientScore(%shooterClient);	
			}
			else if(Client::getTeam(%damagedClient) != Client::getTeam(%shooterClient))
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.ChainDamage += %value;
				Game::refreshClientScore(%shooterClient);	
			}
		}
		if(%type == $BlasterDamageType && %damagedClient != %shooterClient)
		{
			if(%shooterClient.inArena && %damagedClient.inArena && !$ArenaTD::Active)
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.BlasterDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			else if($TA::Rabbit && !%shooterClient.inArena)
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.BlasterDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			else if(Client::getTeam(%damagedClient) != Client::getTeam(%shooterClient))
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.BlasterDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
		}
		if(%type == $PlasmaDamageType && %damagedClient != %shooterClient)
		{
			if(%shooterClient.inArena && %damagedClient.inArena && !$ArenaTD::Active)
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.PlasmaDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			else if($TA::Rabbit && !%shooterClient.inArena)
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.PlasmaDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			else if(Client::getTeam(%damagedClient) != Client::getTeam(%shooterClient))
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.PlasmaDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
		}
		if(%type == $ShotgunDamageType && %damagedClient != %shooterClient)
		{
			if(%shooterClient.inArena && %damagedClient.inArena && !$ArenaTD::Active)
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.ShotgunDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			else if($TA::Rabbit && !%shooterClient.inArena)
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.ShotgunDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			else if(Client::getTeam(%damagedClient) != Client::getTeam(%shooterClient))
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.ShotgunDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
		}
	}
	//elite score stuff end
	if ( %value <= 0 )
		return;
	
	if ( %shooterClient != "" && %shooterClient != "-1" && getObjectType(%shooterClient) == "Net::PacketStream" )
	{
		// MA Code
		%damagedPlayer = Player::getClient(%damagedClient);
		%shooterPlayer = Player::getClient(%shooterClient);
		if(%InAir > 10 && %damagedClient != %shooterClient)
			MidAirCheck(%damagedClient, %shooterClient, %type, %value, %damagedPlayer, %shooterPlayer);
	}
	
	if(%object != 0 && %object != "" && %object != -1 && getObjectType(%shooterClient) == "Net::PacketStream" && %damagedClient != %object && !%object.lastDamageSound)
	{
		//hit sound optional
		Client::sendMessage(%object, 0, "~wButton3.wav");
		if(%shooterClient.hitmarker)
		{
			%damagedPlayer = Client::getName(%damagedClient);
			%hitdist = floor(Vector::getDistance(GameBase::getPosition(%damagedClient), GameBase::getPosition(%shooterClient)) + 0.5);
			remoteeval(%shooterClient, TA::Hit, %damagedPlayer, %hitdist);
		}
		%object.lastDamageSound = true;
		schedule(%object@".lastDamageSound = false;",0.0125,%object);	
	}	

	%dlevel = GameBase::getDamageLevel(%this) + %value;
//	%spillOver = %dlevel - %armor.maxDamage;
	GameBase::setDamageLevel(%this,%dlevel);
	%flash = Player::getDamageFlash(%this) + %value * 2;
	if(%flash > 0.75) 
		%flash = 0.75;
	if ( %flash > 0 )
		Player::setDamageFlash(%this,%flash);

	if ( Player::isDead(%this) )
	{
		if((%value > 0.40 && (%type== $ExplosionDamageType || %type == $ShrapnelDamageType || %type== $MortarDamageType || %type == $MissileDamageType )) || (Player::getLastContactCount(%this) > 6) )
		{
			if(%quadrant == "front_left" || %quadrant == "front_right") 
				%curDie = $PlayerAnim::DieBlownBack;
			else
				%curDie = $PlayerAnim::DieForward;
		}
		else if( Player::isCrouching(%this) )
		{
			%curDie = $PlayerAnim::Crouching;
		}
		else if( %vertPos=="head" )
		{
			if( %quadrant == "front_left" ||	%quadrant == "front_right" ) 
				%curDie = radnomItems(2, $PlayerAnim::DieHead, $PlayerAnim::DieBack);
			else
				%curDie = radnomItems(2, $PlayerAnim::DieHead, $PlayerAnim::DieForward);
		}
		else if(%vertPos == "torso")
		{
			if(%quadrant == "front_left" ) 
				%curDie = radnomItems(3, $PlayerAnim::DieLeftSide, $PlayerAnim::DieChest, $PlayerAnim::DieForwardKneel);
			else if(%quadrant == "front_right") 
				%curDie = radnomItems(3, $PlayerAnim::DieChest, $PlayerAnim::DieRightSide, $PlayerAnim::DieSpin);
			else if(%quadrant == "back_left" ) 
				%curDie = radnomItems(4, $PlayerAnim::DieLeftSide, $PlayerAnim::DieGrabBack, $PlayerAnim::DieForward, $PlayerAnim::DieForwardKneel);
			else if(%quadrant == "back_right") 
				%curDie = radnomItems(4, $PlayerAnim::DieGrabBack, $PlayerAnim::DieRightSide, $PlayerAnim::DieForward, $PlayerAnim::DieForwardKneel);
		}
		else if(%vertPos == "legs")
		{
			if(%quadrant == "front_left" ||	%quadrant == "back_left") 
				%curDie = $PlayerAnim::DieLegLeft;
			if(%quadrant == "front_right" || %quadrant == "back_right") 
				%curDie = $PlayerAnim::DieLegRight;
		}
		Player::setAnimation(%this, %curDie);

		if(%type == $ImpactDamageType && %object.clLastMount != "")
			%shooterClient = %object.clLastMount;
				
		if(%damagedClient != -1)
			Client::onKilled(%damagedClient, %shooterClient, %type);
		else 
			schedule("deleteObject(" @ %this @ ");", $CorpseTimeoutValue + 2.5, %this);	//necro bot..

	}

		return;
}

// End New big ass function	

	if(%type == $StasisDamageType)
	{
	        if(Player::isAIControlled(%this))
		return;
		if (Player::getClient(%this) == %object || (%shooterTeam == %targetTeam && !$Server::TeamDamageScale))
			return;
		Client::SendMessage(%damagedClient,1,"You've been hit with a stasis spell! Unable to move or take damage for 10 seconds.");
		if(%armor == harmor1 || %armor == harmor2 || %armor == harmor3)
			return;
		if(%armor == armormAngel || %armor == armorfAngel || %armor == armormSpy || %armor == armorfSpy || %armor == armormNecro || %armor == armorfNecro)
			Player::setArmor(%damagedClient, "harmor1");
		else if(%armor == armormWarrior || %armor == armorfWarrior || %armor == armormBuilder || %armor == armorfBuilder || %armor == armormDM || %armor == armorfDM)
			Player::setArmor(%damagedClient, "harmor2");
		else if(%armor == armorTroll || %armor == armorTank || %armor == armorTitan)
			Player::setArmor(%damagedClient, "harmor3");
		%player = Client::getOwnedObject(%damagedClient); // -death666 3.29.17
		%player.frozen = true; // -death666 3.29.17
		%player.Stasised = 20;
		schedule("StasisAnimation("@%player@");",0.1,%player);
		schedule("Stasis::resetArmor("@ %damagedClient @", "@ %armor @");",10,%damagedClient);
	}
	//end stasis
	if(Client::getTeam(%damagedClient) != Client::getTeam(%shooterClient) || $Deathmatch)
	{
		if(%type == $DisarmDamageType) 
		{
			Player::trigger(%this, $WeaponSlot, false);
			%weaponType = Player::getMountedItem(%this,$WeaponSlot);
			if(%weaponType != -1)
				Player::dropItem(%this,%weaponType);
			Client::sendMessage(Player::getClient(%damagedClient),0,"Your have been hit by a disarm spell!");
		}
	}
		
	if(%shooterTeam != %targetTeam || $Server::TeamDamageScale || Player::getClient(%this) == %object)
	{	
		if(%type == $PlasmaDamageType)
			schedule(%armor @ "::onBurn(" @ %damagedClient @ ", " @ %this @ ");",0.01,%damagedClient);							
		else if(%type == $ShockDamageType)
			schedule(%armor @ "::onShock(" @ %damagedClient @ ", " @ %this @ ");",0.01,%damagedClient);
		else if(%type == $PoisonDamageType && !$Annihilation::NoPlayerDamage)
			schedule(%armor @ "::onPoison(" @ %damagedClient @ ", " @ %this @ ");",0.01,%damagedClient);
	}
// Old TeamPlay code	
//	//================
//
//	if($teamplay && %damagedClient != %shooterClient && Client::getTeam(%damagedClient) == Client::getTeam(%shooterClient) ) 
//	{
//		if(%shooterClient != -1) 
//		{
//			%curTime = getSimTime();
//			if((%curTime - %this.DamageTime > 3.5 || %this.LastHarm != %shooterClient) && %damagedClient != %shooterClient && $Server::TeamDamageScale > 0) 
//			{
//				if(%type != $MineDamageType) 
//				{
//					Client::sendMessage(%shooterClient,0,"You just harmed Teammate " @ Client::getName(%damagedClient) @ "!");
//					Client::sendMessage(%damagedClient,0,"You took Friendly Fire from " @ Client::getName(%shooterClient) @ "!");
//				}
//				else
//				{
//					Client::sendMessage(%shooterClient,0,"You just harmed Teammate " @ Client::getName(%damagedClient) @ " with your mine!");
//					Client::sendMessage(%damagedClient,0,"You just stepped on Teamate " @ Client::getName(%shooterClient) @ "'s mine!");
//				}
//				%this.LastHarm = %shooterClient;
//				%this.DamageStamp = %curTime;
//			}
//		}
//		%friendFire = $Server::TeamDamageScale;
//	}
//	else if(%type == $ImpactDamageType && Client::getTeam(%object.clLastMount) == Client::getTeam(%damagedClient)) 
//		%friendFire = $Server::TeamDamageScale;
//	else
//		%friendFire = 1.0;
//	//================
	// New Arena code 
	if($TA::Rabbit == true) //new rabbit code 
	{
		if(%type == $ImpactDamageType && Client::getTeam(%object.clLastMount) == Client::getTeam(%damagedClient)) 
			%friendFire = $Server::TeamDamageScale;
		else
			%friendFire = 1.0;
	}
	else if(%damagedClient.inArenaTD && %shooterClient.inArenaTD)
	{
		if(%damagedClient != %shooterClient && Client::getTeam(%damagedClient) == Client::getTeam(%shooterClient) )
		{
			//if(%shooterClient != -1 && !%damagedClient.inArena && !%shooterClient.inArena && !%damagedClient.inDuel && !%shooterClient.inDuel)
			if(%shooterClient != -1) 
			{
				%curTime = getSimTime();
				if((%curTime - %this.DamageTime > 3.5 || %this.LastHarm != %shooterClient) && %damagedClient != %shooterClient && $Server::ArenaTeamDamageScale > 0) 
				{
					if(%type != $MineDamageType) 
					{
						Client::sendMessage(%shooterClient,0,"You just harmed Teammate " @ Client::getName(%damagedClient) @ "!");
						Client::sendMessage(%damagedClient,0,"You took Friendly Fire from " @ Client::getName(%shooterClient) @ "!");
					}
					else
					{
						Client::sendMessage(%shooterClient,0,"You just harmed Teammate " @ Client::getName(%damagedClient) @ " with your mine!");
						Client::sendMessage(%damagedClient,0,"You just stepped on Teammate " @ Client::getName(%shooterClient) @ "'s mine!");
					}
					%this.LastHarm = %shooterClient;
					%this.DamageStamp = %curTime;
				}
			}
			%friendFire = $Server::ArenaTeamDamageScale;
		}
		else if(%type == $ImpactDamageType && Client::getTeam(%object.clLastMount) == Client::getTeam(%damagedClient)) 
			%friendFire = $Server::ArenaTeamDamageScale;
		else
			%friendFire = 1.0;
	}
	else if(!%damagedClient.inArena && !%shooterClient.inArena && !%damagedClient.inDuel && !%shooterClient.inDuel) 
	{
		if($teamplay && %damagedClient != %shooterClient && Client::getTeam(%damagedClient) == Client::getTeam(%shooterClient) )
		{
			//if(%shooterClient != -1 && !%damagedClient.inArena && !%shooterClient.inArena && !%damagedClient.inDuel && !%shooterClient.inDuel)
			if(%shooterClient != -1) 
			{
				%curTime = getSimTime();
				if((%curTime - %this.DamageTime > 3.5 || %this.LastHarm != %shooterClient) && %damagedClient != %shooterClient && $Server::TeamDamageScale > 0) 
				{
					if(%type != $MineDamageType) 
					{
						Client::sendMessage(%shooterClient,0,"You just harmed Teammate " @ Client::getName(%damagedClient) @ "!");
						Client::sendMessage(%damagedClient,0,"You took Friendly Fire from " @ Client::getName(%shooterClient) @ "!");
					}
					else
					{
						Client::sendMessage(%shooterClient,0,"You just harmed Teammate " @ Client::getName(%damagedClient) @ " with your mine!");
						Client::sendMessage(%damagedClient,0,"You just stepped on Teammate " @ Client::getName(%shooterClient) @ "'s mine!");
					}
					%this.LastHarm = %shooterClient;
					%this.DamageStamp = %curTime;
				}
			}
			%friendFire = $Server::TeamDamageScale;
		}
		else if(%type == $ImpactDamageType && Client::getTeam(%object.clLastMount) == Client::getTeam(%damagedClient)) 
			%friendFire = $Server::TeamDamageScale;
		else
			%friendFire = 1.0;
	}
	else
	{
		if(%type == $ImpactDamageType && Client::getTeam(%object.clLastMount) == Client::getTeam(%damagedClient)) 
			%friendFire = $Server::TeamDamageScale;
		else
			%friendFire = 1.0;
	}
	
	if(%vertPos == "head" && %type == $LaserDamageType) 
	{
		if(%armor == "armorTroll" || %armor == "armorTank" ||  %armor == "armorTitan") 	
		{
			if(%quadrant == "middle_back" || %quadrant == "middle_front" || %quadrant == "middle_middle") 
			{
				%value += (%value * 0.45);	//0.3
			}
		}
		else 
		{
			%value += (%value * 0.55);	//0.3
		}
	}

	if(%type == $SniperDamageType) 
	{
		if(%vertPos == "head")
		{
			if(%armor == "armorTroll" || %armor == "armorTank" ||  %armor == "armorTitan") 	
			{
				if(%quadrant == "middle_back" || %quadrant == "middle_front" || %quadrant == "middle_middle") 
				{
					%value += (%value * 0.3);	//head shot to heavy
				}
			}
			else 
			{
				%value += (%value * 0.35);	//med/ light head shots
			}
		}
		else 
			%value += (%value * 0.27);// adding 27% to any other shot
	}			
		
		
	//If Shield Pack is on
	if(%type != -1 && %this.shieldStrength ) 
	{
		%energy = GameBase::getEnergy(%this);
		%strength = %this.shieldStrength;
		if(%type == $ShrapnelDamageType || %type == $MortarDamageType)
			%strength *= 0.75;
		if(%type == $ElectricityDamageType)
			%strength *= 0.0;
		%absorb = %energy * %strength;
		if(%value < %absorb) 
		{
			if ( %type != $LandingDamageType )
			{
				GameBase::setEnergy(%this,%energy - ((%value / %strength)*%friendFire));
				%thisPos = getBoxCenter(%this);
				%offsetZ =((getWord(%pos,2))-(getWord(%thisPos,2)));
				GameBase::activateShield(%this,%vec,%offsetZ);
			}
			%value = 0;
		}
		else 
		{
			GameBase::setEnergy(%this,0);
			%value = %value - %absorb;
		}
	}
	%value = $DamageScale[%armor, %type] * %value * %friendFire;
	//End of damage stuff
	%sameTeam = (%damagedteam == %shooterteam);
	//update player score here.  no points for TKs
	// elite score stuff
	if($TALT::Active == true || %shooterClient.inArena && %damagedClient.inArena) //New LT code
	{
		if(%type == $ExplosionDamageType && %damagedClient != %shooterClient)
		{
			if(%shooterClient.inArena && %damagedClient.inArena && !$ArenaTD::Active) //reworked for arena
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.DiscDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			else if($TA::Rabbit && !%shooterClient.inArena)
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.DiscDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			else if(Client::getTeam(%damagedClient) != Client::getTeam(%shooterClient))
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.DiscDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			
		}
		if(%type == $ShrapnelDamageType && %damagedClient != %shooterClient)
		{
			if(%shooterClient.inArena && %damagedClient.inArena && !$ArenaTD::Active)
			{
							
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.NadeDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			else if($TA::Rabbit && !%shooterClient.inArena)
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.NadeDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			else if(Client::getTeam(%damagedClient) != Client::getTeam(%shooterClient))
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.NadeDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
		}
		if(%type == $BulletDamageType && %damagedClient != %shooterClient)
		{
			if(%shooterClient.inArena && %damagedClient.inArena && !$ArenaTD::Active)
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.ChainDamage += %value;
				Game::refreshClientScore(%shooterClient);	
			}
			else if($TA::Rabbit && !%shooterClient.inArena)
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.ChainDamage += %value;
				Game::refreshClientScore(%shooterClient);	
			}
			else if(Client::getTeam(%damagedClient) != Client::getTeam(%shooterClient))
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.ChainDamage += %value;
				Game::refreshClientScore(%shooterClient);	
			}
		}
		if(%type == $BlasterDamageType && %damagedClient != %shooterClient)
		{
			if(%shooterClient.inArena && %damagedClient.inArena && !$ArenaTD::Active)
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.BlasterDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			else if($TA::Rabbit && !%shooterClient.inArena)
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.BlasterDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			else if(Client::getTeam(%damagedClient) != Client::getTeam(%shooterClient))
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.BlasterDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
		}
		if(%type == $PlasmaDamageType && %damagedClient != %shooterClient)
		{
			if(%shooterClient.inArena && %damagedClient.inArena && !$ArenaTD::Active)
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.PlasmaDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			else if($TA::Rabbit && !%shooterClient.inArena)
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.PlasmaDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			else if(Client::getTeam(%damagedClient) != Client::getTeam(%shooterClient))
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.PlasmaDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
		}
		if(%type == $ShotgunDamageType && %damagedClient != %shooterClient)
		{
			if(%shooterClient.inArena && %damagedClient.inArena && !$ArenaTD::Active)
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.ShotgunDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			else if($TA::Rabbit && !%shooterClient.inArena)
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.ShotgunDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
			else if(Client::getTeam(%damagedClient) != Client::getTeam(%shooterClient))
			{
				%shooterClient.score += %value;
				%shooterClient.Credits += %value;
				%shooterClient.ShotgunDamage += %value;
				Game::refreshClientScore(%shooterClient);
			}
		}
	}
	//elite score stuff end	
	if ( %value <= 0 )
		return;
	
	if ( %shooterClient != "" && %shooterClient != "-1" && getObjectType(%shooterClient) == "Net::PacketStream" )
	{
		// MA Code hax
		%damagedPlayer = Player::getClient(%damagedClient);
		%shooterPlayer = Player::getClient(%shooterClient);
		if(%InAir > 10 && %damagedClient != %shooterClient)
			MidAirCheck(%damagedClient, %shooterClient, %type, %value, %damagedPlayer, %shooterPlayer);
	}
	
	if(%object != 0 && %object != "" && %object != -1 && getObjectType(%shooterClient) == "Net::PacketStream" && %damagedClient != %object && !%object.lastDamageSound)
	{
		//hit sound optional
		Client::sendMessage(%object, 0, "~wButton3.wav");
		if(%shooterClient.hitmarker)
		{
			%damagedPlayer = Client::getName(%damagedClient);
			%hitdist = floor(Vector::getDistance(GameBase::getPosition(%damagedClient), GameBase::getPosition(%shooterClient)) + 0.5);
			remoteeval(%shooterClient, TA::Hit, %damagedPlayer, %hitdist);
		}
		%object.lastDamageSound = true;
		schedule(%object@".lastDamageSound = false;",0.0125,%object);	
	}	

	%dlevel = GameBase::getDamageLevel(%this) + %value;
//	%spillOver = %dlevel - %armor.maxDamage;
	GameBase::setDamageLevel(%this,%dlevel);
	%flash = Player::getDamageFlash(%this) + %value * 2;
	if(%flash > 0.75) 
		%flash = 0.75;
	if ( %flash > 0 )
		Player::setDamageFlash(%this,%flash);

	if ( Player::isDead(%this) )
	{
		if((%value > 0.40 && (%type== $ExplosionDamageType || %type == $ShrapnelDamageType || %type== $MortarDamageType || %type == $MissileDamageType )) || (Player::getLastContactCount(%this) > 6) )
		{
			if(%quadrant == "front_left" || %quadrant == "front_right") 
				%curDie = $PlayerAnim::DieBlownBack;
			else
				%curDie = $PlayerAnim::DieForward;
		}
		else if( Player::isCrouching(%this) )
		{
			%curDie = $PlayerAnim::Crouching;
		}
		else if( %vertPos=="head" )
		{
			if( %quadrant == "front_left" ||	%quadrant == "front_right" ) 
				%curDie = radnomItems(2, $PlayerAnim::DieHead, $PlayerAnim::DieBack);
			else
				%curDie = radnomItems(2, $PlayerAnim::DieHead, $PlayerAnim::DieForward);
		}
		else if(%vertPos == "torso")
		{
			if(%quadrant == "front_left" ) 
				%curDie = radnomItems(3, $PlayerAnim::DieLeftSide, $PlayerAnim::DieChest, $PlayerAnim::DieForwardKneel);
			else if(%quadrant == "front_right") 
				%curDie = radnomItems(3, $PlayerAnim::DieChest, $PlayerAnim::DieRightSide, $PlayerAnim::DieSpin);
			else if(%quadrant == "back_left" ) 
				%curDie = radnomItems(4, $PlayerAnim::DieLeftSide, $PlayerAnim::DieGrabBack, $PlayerAnim::DieForward, $PlayerAnim::DieForwardKneel);
			else if(%quadrant == "back_right") 
				%curDie = radnomItems(4, $PlayerAnim::DieGrabBack, $PlayerAnim::DieRightSide, $PlayerAnim::DieForward, $PlayerAnim::DieForwardKneel);
		}
		else if(%vertPos == "legs")
		{
			if(%quadrant == "front_left" ||	%quadrant == "back_left") 
				%curDie = $PlayerAnim::DieLegLeft;
			if(%quadrant == "front_right" || %quadrant == "back_right") 
				%curDie = $PlayerAnim::DieLegRight;
		}
		Player::setAnimation(%this, %curDie);

		if(%type == $ImpactDamageType && %object.clLastMount != "")
			%shooterClient = %object.clLastMount;
				
		if(%damagedClient != -1)
			Client::onKilled(%damagedClient, %shooterClient, %type);
		else 
			schedule("deleteObject(" @ %this @ ");", $CorpseTimeoutValue + 2.5, %this);	//necro bot..
//duel code 
//		if(%shooterClient.inDuel && %damagedClient.inDuel) //going to load this up in game.cs instead 
//		{
//			Duel::Finish(%shooterClient, %damagedClient);
//		}
//end duel code 
//killing spree 
//added duel and arena stuff plus some extra color for v5.0 
//added sounds and btmprint and armor healing -death666 4.7.17
				if(!$Server::TourneyMode && %damagedClient.isNotBot)
				{
				    %client = client::getname(%shooterClient);
				    %player = Client::getOwnedObject(%object);
				  //%shooterclient.scoreKillspree++;
				    %kill = %shooterClient.Killspree;
					if(Player::getArmor(%shooterClient) == "armormLightArmor" ||Player::getArmor(%shooterClient) == "armorfLightArmor" ||Player::getArmor(%shooterClient) == "armormMercenary" ||Player::getArmor(%shooterClient) == "armorfMercenary" ||Player::getArmor(%shooterClient) == "armormAngel" ||Player::getArmor(%shooterClient) == "armorfAngel" ||Player::getArmor(%shooterClient) == "armormSpy" ||Player::getArmor(%shooterClient) == "armorfSpy" ||Player::getArmor(%shooterClient) == "armormNecro" ||Player::getArmor(%shooterClient) == "armorfNecro" ||Player::getArmor(%shooterClient) == "armormWarrior" ||Player::getArmor(%shooterClient) == "armorfWarrior" ||Player::getArmor(%shooterClient) == "armormBuilder" ||Player::getArmor(%shooterClient) == "armorfBuilder" ||Player::getArmor(%shooterClient) == "armorTroll" ||Player::getArmor(%shooterClient) == "armorTank" ||Player::getArmor(%shooterClient) == "armorTitan")
					{
                                    //  echo(""@ %client @" is on a KILLING SPREE he has "@ %kill @" in a row! ~wcapturedtower.wav");
                 					if(%kill > 30)
                                         {
									if(%shooterClient.inArena)
										ArenaMSG(0,"CAN NO ONE STOP "@ %client @"? HE NOW HAS "@ %kill @" KILL'S IN A ROW!");
									else
				   messageall(3,""@ %client @" is on a KILLING SPREE with "@ %kill @" kill's in a row!"); 
				   bottomprintall("<jc><f1> "@ %client @" is on a <f2>KILLING SPREE<f1> with <f2>"@ %kill @"<f1> kill's in a row!", 10);
				   Client::sendMessage(%shooterclient,1, "Killing spree maintained. Armor repairing to full health. ~wBXplo1.wav");
				   GameBase::repairDamage(%player, 1.5);
				       }
                                    else if(%kill == 20)
                                         {
									if(%shooterClient.inArena)
										ArenaMSG(0,""@ %client @" is on an EXTREME KILLING SPREE he has "@ %kill @" kill's in a row!");
									else
				   messageall(3,""@ %client @" is on a KILLING SPREE with "@ %kill @" kill's in a row! ~wmale1.wtakcovr.wav");
				   messageall(3,""@ %client @" is on a KILLING SPREE with "@ %kill @" kill's in a row! ~wBXplo1.wav");
				   bottomprintall("<jc><f1> "@ %client @" is on a <f2>KILLING SPREE<f1> with <f2>"@ %kill @"<f1> kill's in a row!", 10);
				   Client::sendMessage(%shooterclient,1, "Killing spree initiated. Armor repairing to full health. ~wBXplo1.wav");
				   GameBase::repairDamage(%player, 1.5);
				       }
                                     else if(%kill == 10)
                                        {
										%shooterClient.TKillstreaks++;
									KillSkinPack(%shooterClient);
									if(%shooterClient.inArena)
										ArenaMSG(3,""@ %client @" is on a KILLING SPREE he has "@ %kill @" kill's in a row!");
									else
				   messageall(3,""@ %client @" is on a KILLING SPREE with "@ %kill @" kill's in a row! ~wmale1.wtakcovr.wav");
				   messageall(3,""@ %client @" is on a KILLING SPREE with "@ %kill @" kill's in a row! ~wBXplo1.wav");
				   bottomprintall("<jc><f1> "@ %client @" is on a <f2>KILLING SPREE<f1> with <f2>"@ %kill @"<f1> kill's in a row!", 10);
				   Client::sendMessage(%shooterclient,1, "Killing spree initiated. Armor repairing to full health. ~wBXplo1.wav");
				   GameBase::repairDamage(%player, 1.5);
						}
					}
				}
//end killing spree
	}
}

function remoteTA::Hitmarker(%clientId) //client side script 
{
	%clientId.hitmarker = true;
}

function MidAirCheck(%damagedClient, %shooterClient, %type, %value, %damagedPlayer, %shooterPlayer)
{
	%set = newObject("set",SimSet);
	%objectMA = Group::getObject(%set, 0);
	%sName = GameBase::getDataName(%objectMA);
	if(%sName.className == SelfPoweredTurret) //get rid of an MA bug
	{
		deleteObject(%set);
		return;
	}
		
	%wav = "~wbutton3.wav";
	%extramsg = "";
	if(Player::getLastContactCount(%damagedPlayer) > 14)
	{
		//if(%type == "3" && %value >= 0.3 || %type == "4" && %value >= 0.34 || %type == "5" && %value >= 0.35)
		//{
			if(isObject(%damagedPlayer) && isObject(%shooterPlayer))
			{
				if(%type == 5) { //fuck keep getting false ma so i need to boost this numbers up
					%distance = "9.0"; } //8.0
				else {
					%distance = "9.0"; } //7.5

				if(NotTouching(%damagedPlayer, %distance, %shooterClient))
				{
					%madistance = floor(Vector::getDistance(GameBase::getPosition(%damagedPlayer), GameBase::getPosition(%shooterPlayer)) + 0.5);
					if(%madistance > -77)
					{
						//eval( "%distance = %shooterClient.");
						%weapon = Player::getMountedItem(%shooterClient,$WeaponSlot);
						if((%damagedClient != %shooterClient) && %weapon == DiscLauncher)
   						{
							if(%type == $ExplosionDamageType)
							{
								if(%value >= 0.35)
								{
							if(%damagedClient.inArena && %shooterClient.inArena)
							{
								%shooterClient.score += 1;
								%shooterClient.Credits += 1;
								%shooterClient.MidAirs++;
								%shooterClient.TMidAirs++;
								if(%madistance > %shooterClient.TFarthestDMA)
									%shooterClient.TFarthestDMA = %madistance;
								Game::refreshClientScore(%shooterClient);
								client::sendmessage(%shooterClient, 0, %wav);
								ArenaMSG(0,Client::getName(%shooterClient) @ " landed a Mid Air Disc hit on "@Client::getName(%damagedClient)@" from "@%madistance@" meters away!");
								bottomPrint(%shooterclient, "<jc><f1>You landed a Mid Air Disc hit on <f3>"@Client::getName(%damagedClient)@"<f1> from <f3>"@%madistance@"<f1> meters away!\n<jc><f2> One point bonus MID AIR DISC!",8);
								bottomPrint(%damagedclient, "<jc><f3>"@Client::getName(%shooterClient)@"<f1> landed a Mid Air Disc hit on you from <f3>"@%madistance@"<f1> meters away!",8);
							
							}
							else
							{
								client::sendmessage(%shooterClient, 0, %wav);
								if(%damagedClient.inDuel && %shooterClient.inDuel)
									DuelMSG(Client::getName(%shooterClient) @ " landed a Mid Air Disc hit on "@Client::getName(%damagedClient)@" from "@%madistance@" meters away!");
								bottomPrint(%shooterclient, "<jc><f1>You landed a Mid Air Disc hit on <f3>"@Client::getName(%damagedClient)@"<f1> from <f3>"@%madistance@"<f1> meters away!",8);
								bottomPrint(%damagedclient, "<jc><f3>"@Client::getName(%shooterClient)@"<f1> landed a Mid Air Disc hit on you from <f3>"@%madistance@"<f1> meters away!",8);
								if($TALT::Active)
									%shooterClient.score += 0.5;
								%shooterClient.Credits += 0.5;
								%shooterClient.MidAirs++;
								%shooterClient.TMidAirs++;
								if(%madistance > %shooterClient.TFarthestDMA)
									%shooterClient.TFarthestDMA = %madistance;
								Game::refreshClientScore(%shooterClient);
							}
								}
							}
						}
						if((%damagedClient != %shooterClient) && %weapon == DiscLauncherElite)
   						{
							if(%type == $ExplosionDamageType)
							{
								if(%value >= 0.35)
								{
							if(%damagedClient.inArena && %shooterClient.inArena)
							{
								%shooterClient.score += 1;
								%shooterClient.Credits += 1;
								%shooterClient.MidAirs++;
								%shooterClient.TMidAirs++;
								if(%madistance > %shooterClient.TFarthestDMA)
									%shooterClient.TFarthestDMA = %madistance;
								Game::refreshClientScore(%shooterClient);
								client::sendmessage(%shooterClient, 0, %wav);
								ArenaMSG(0,Client::getName(%shooterClient) @ " landed a Mid Air Disc hit on "@Client::getName(%damagedClient)@" from "@%madistance@" meters away!");
								bottomPrint(%shooterclient, "<jc><f1>You landed a Mid Air Disc hit on <f3>"@Client::getName(%damagedClient)@"<f1> from <f3>"@%madistance@"<f1> meters away!\n<jc><f2> One point bonus MID AIR DISC!",8);
								bottomPrint(%damagedclient, "<jc><f3>"@Client::getName(%shooterClient)@"<f1> landed a Mid Air Disc hit on you from <f3>"@%madistance@"<f1> meters away!",8);
							
							}
							else
							{
								client::sendmessage(%shooterClient, 0, %wav);
								if(%damagedClient.inDuel && %shooterClient.inDuel)
									DuelMSG(Client::getName(%shooterClient) @ " landed a Mid Air Disc hit on "@Client::getName(%damagedClient)@" from "@%madistance@" meters away!");
								bottomPrint(%shooterclient, "<jc><f1>You landed a Mid Air Disc hit on <f3>"@Client::getName(%damagedClient)@"<f1> from <f3>"@%madistance@"<f1> meters away!",8);
								bottomPrint(%damagedclient, "<jc><f3>"@Client::getName(%shooterClient)@"<f1> landed a Mid Air Disc hit on you from <f3>"@%madistance@"<f1> meters away!",8);
								%shooterClient.MidAirs++;
								%shooterClient.TMidAirs++;
								if(%madistance > %shooterClient.TFarthestDMA)
									%shooterClient.TFarthestDMA = %madistance;
								if($TALT::Active)
									%shooterClient.score += 0.5;
								%shooterClient.Credits += 0.5;
								Game::refreshClientScore(%shooterClient);
							}
								}
							}
						}
						if((%damagedClient != %shooterClient) && %weapon == DiscLauncherBase)
   						{
							if(%type == $ExplosionDamageType)
							{
								if(%value >= 0.35)
								{
							if(%damagedClient.inArena && %shooterClient.inArena)
							{
								%shooterClient.score += 1;
								%shooterClient.Credits += 1;
								%shooterClient.MidAirs++;
								%shooterClient.TMidAirs++;
								if(%madistance > %shooterClient.TFarthestDMA)
									%shooterClient.TFarthestDMA = %madistance;
								Game::refreshClientScore(%shooterClient);
								client::sendmessage(%shooterClient, 0, %wav);
								ArenaMSG(0,Client::getName(%shooterClient) @ " landed a Mid Air Disc hit on "@Client::getName(%damagedClient)@" from "@%madistance@" meters away!");
								bottomPrint(%shooterclient, "<jc><f1>You landed a Mid Air Disc hit on <f3>"@Client::getName(%damagedClient)@"<f1> from <f3>"@%madistance@"<f1> meters away!\n<jc><f2> One point bonus MID AIR DISC!",8);
								bottomPrint(%damagedclient, "<jc><f3>"@Client::getName(%shooterClient)@"<f1> landed a Mid Air Disc hit on you from <f3>"@%madistance@"<f1> meters away!",8);
							
							}
							else
							{
								client::sendmessage(%shooterClient, 0, %wav);
								if(%damagedClient.inDuel && %shooterClient.inDuel)
									DuelMSG(Client::getName(%shooterClient) @ " landed a Mid Air Disc hit on "@Client::getName(%damagedClient)@" from "@%madistance@" meters away!");
								bottomPrint(%shooterclient, "<jc><f1>You landed a Mid Air Disc hit on <f3>"@Client::getName(%damagedClient)@"<f1> from <f3>"@%madistance@"<f1> meters away!",8);
								bottomPrint(%damagedclient, "<jc><f3>"@Client::getName(%shooterClient)@"<f1> landed a Mid Air Disc hit on you from <f3>"@%madistance@"<f1> meters away!",8);
								%shooterClient.MidAirs++;
								%shooterClient.TMidAirs++;
								if(%madistance > %shooterClient.TFarthestDMA)
									%shooterClient.TFarthestDMA = %madistance;
								if($TALT::Active)
									%shooterClient.score += 0.5;
								%shooterClient.Credits += 0.5;
								Game::refreshClientScore(%shooterClient);
							}
								}
							}
						}
						else if((%damagedClient != %shooterClient) && %weapon == PlasmaGunBase)
   						{
							
							if(%type == $PlasmaDamageType)
							{
								if(%value >= 0.3)
								{
									if(%damagedClient.inArena && %shooterClient.inArena)
									{
							%shooterClient.MidAirs++;
							%shooterClient.TMidAirs++;
							%shooterClient.score += 1;
							%shooterClient.Credits += 1;
							Game::refreshClientScore(%shooterClient);
							client::sendmessage(%shooterClient, 0, %wav);
							ArenaMSG(0,Client::getName(%shooterClient) @ " landed a Mid Air Plasma hit on "@Client::getName(%damagedClient)@" from "@%madistance@" meters away!");
							bottomPrint(%shooterclient, "<jc><f1>You landed a Mid Air Plasma hit on <f3>"@Client::getName(%damagedClient)@"<f1> from <f3>"@%madistance@"<f1> meters away!\n<jc><f2> One point bonus MID AIR PLASMA!",8);
							bottomPrint(%damagedclient, "<jc><f3>"@Client::getName(%shooterClient)@"<f1> landed a Mid Air Plasma hit on you from <f3>"@%madistance@"<f1> meters away!",8);
									}
									else
									{
							%shooterClient.MidAirs++;
							%shooterClient.TMidAirs++;
							if(%madistance > %shooterClient.TFarthestDMA)
								%shooterClient.TFarthestDMA = %madistance;
							if($TALT::Active)
								%shooterClient.score += 0.5;
							%shooterClient.Credits += 0.5;
							Game::refreshClientScore(%shooterClient);
							client::sendmessage(%shooterClient, 0, %wav);
							if(%damagedClient.inDuel && %shooterClient.inDuel)
								DuelMSG(Client::getName(%shooterClient) @ " landed a Mid Air Plasma hit on "@Client::getName(%damagedClient)@" from "@%madistance@" meters away!");
							bottomPrint(%shooterclient, "<jc><f1>You landed a Mid Air Plasma hit on <f3>"@Client::getName(%damagedClient)@"<f1> from <f3>"@%madistance@"<f1> meters away!",8);
							bottomPrint(%damagedclient, "<jc><f3>"@Client::getName(%shooterClient)@"<f1> landed a Mid Air Plasma hit on you from <f3>"@%madistance@"<f1> meters away!",8);
									}
										
								}
							}
						}
   			   			else if((%damagedClient != %shooterClient) && %weapon == LaserRifle)
    					{
							
							if(%type == $LaserDamageType)
							{
								if(%value >= 0.37)
								{
							client::sendmessage(%shooterClient, 0, %wav);
							bottomPrint(%shooterclient, "<jc><f1>You landed a Mid Air Laser hit on <f3>"@Client::getName(%damagedClient)@"<f1> from <f3>"@%madistance@"<f1> meters away!",8);
							bottomPrint(%damagedclient, "<jc><f3>"@Client::getName(%shooterClient)@"<f1> landed a Mid Air Laser hit on you from <f3>"@%madistance@"<f1> meters away!",8);
							%shooterClient.MidAirs++;
							%shooterClient.TMidAirs++;
							if(%madistance > %shooterClient.TFarthestSMA)
								%shooterClient.TFarthestSMA = %madistance;
							Game::refreshClientScore(%shooterClient);
								}
							}
						}
						else if((%damagedClient != %shooterClient) && %weapon == ParticleBeamWeapon)
						{
							if(%type == $LaserDamageType || %type == $SniperDamageType)
							{
								if(%value >= 0.37)
								{
							client::sendmessage(%shooterClient, 0, %wav);
							bottomPrint(%shooterclient, "<jc><f1>You landed a Mid Air Laser hit on <f3>"@Client::getName(%damagedClient)@"<f1> from <f3>"@%madistance@"<f1> meters away!",8);
							bottomPrint(%damagedclient, "<jc><f3>"@Client::getName(%shooterClient)@"<f1> landed a Mid Air Laser hit on you from <f3>"@%madistance@"<f1> meters away!",8);
							%shooterClient.MidAirs++;
							%shooterClient.TMidAirs++;
							if(%madistance > %shooterClient.TFarthestSMA)
								%shooterClient.TFarthestSMA = %madistance;
							Game::refreshClientScore(%shooterClient);
								}
							}
						}  
   					   	else if((%damagedClient != %shooterClient) && %weapon == Railgun)
    					{
							if(%type == $SniperDamageType)
							{
								if(%value >= 0.37)
								{
							client::sendmessage(%shooterClient, 0, %wav);
							bottomPrint(%shooterclient, "<jc><f1>You landed a Mid Air Sniper hit on <f3>"@Client::getName(%damagedClient)@"<f1> from <f3>"@%madistance@"<f1> meters away!",8);
							bottomPrint(%damagedclient, "<jc><f3>"@Client::getName(%shooterClient)@"<f1> landed a Mid Air Sniper hit on you from <f3>"@%madistance@"<f1> meters away!",8);
							%shooterClient.MidAirs++;
							%shooterClient.TMidAirs++;
							if(%madistance > %shooterClient.TFarthestSMA)
								%shooterClient.TFarthestSMA = %madistance;
							Game::refreshClientScore(%shooterClient);
								}
							}
						}
   					   	else if((%damagedClient != %shooterClient) && %weapon == SniperRifle)
    					{
							if(%type == $SniperDamageType)
							{
								if(%value >= 0.37)
								{
							client::sendmessage(%shooterClient, 0, %wav);
							bottomPrint(%shooterclient, "<jc><f1>You landed a Mid Air Sniper hit on <f3>"@Client::getName(%damagedClient)@"<f1> from <f3>"@%madistance@"<f1> meters away!",8);
							bottomPrint(%damagedclient, "<jc><f3>"@Client::getName(%shooterClient)@"<f1> landed a Mid Air Sniper hit on you from <f3>"@%madistance@"<f1> meters away!",8);
							%shooterClient.MidAirs++;
							%shooterClient.TMidAirs++;
							if(%madistance > %shooterClient.TFarthestSMA)
								%shooterClient.TFarthestSMA = %madistance;
							Game::refreshClientScore(%shooterClient);
								}
							}
						}
					}
				}
			}
		//}
		else {
			if($debug)
			{
				client::sendmessage(%shooterClient, 2, "MA Failed type-value ("@%type@")-("@%value@")");
			}
		}
	}
	else {
		if($debug)
		{
			client::sendmessage(%shooterClient, 2, "MA Last Contact failed ("@Player::getLastContactCount(%damagedPlayer)@")");
		}
	}
}

function NotTouching(%player, %distance, %client)
{
	%angle[0] = "1.57 0 0";//above
	%angle[1] = "-1.57 0 0";//below
	%angle[2] = "0 0 1.57";//right
	%angle[3] = "0 0 -1.57";//left
	%angle[4] = "0 0 0";//front
	%angle[5] = "0 0 3.14";//behind

	for(%x = 0; %x < 7; %x++)
	{
		resetlos();
		gamebase::getlosinfo(%player, %distance, %angle[%x]);
		%myobject = $los::object;

		if(%myobject != "")
		{
			%type = getObjectType(%myobject);
			%pos = $los::position;
			if(%type == "SimTerrain" || %type == "InteriorShape" || %type == "StaticShape")
			{
				if($debug)
				{
					client::sendmessage(%client, $White, "MA object check failed["@%x@"] distance("@%distance@") objectID("@%myobject@") objectName("@Object::getName(%myobject)@") objectDataName("@GameBase::getDataName(%myobject)@") objectType("@getObjectType(%myobject)@") obj-playerDistance("@vector::getdistance(gamebase::getposition(%player), %pos)@")");
				}
					return false;
			}
		}

	}
	return true;
}

function resetlos()
{
	$los::postion = "";
	$los::normal = "";
	$los::object = "";
}
//MA Functions END

function radnomItems(%num, %an0, %an1, %an2, %an3, %an4, %an5, %an6)
{
	return %an[floor(getRandom() * (%num - 0.01))];
}

function Player::onCollision(%this,%obj)
{	
	DebugFun("Player::onCollision",%this,%obj);
	if($debug) 
		event::collision(%this,%obj);

	if(getObjectType(%obj) == "Player")
	{
		//player colliding with another player
		if(Player::isDead(%obj) && $TALT::Active == false) 
		{
			%thisVel = Item::getVelocity(%this);	
			%objVel = Item::getVelocity(%obj);
			Item::setVelocity(%obj, %thisVel);
		}	
		else if(Player::isDead(%this)) 
		{
			// Dead players transfer all items to the live player
			%sound = false;
			%max = getNumItems();
			for(%i = 0; %i < %max; %i = %i + 1) 
			{
				%count = Player::getItemCount(%this,%i);
				if(%count) 
				{
					%delta = Item::giveItem(%obj,getItemData(%i),%count);
					if(%delta > 0) 
					{
						Player::decItemCount(%this,%i,%delta);
						%sound = true;
					}
				}
			}
			if(%sound) 
			{
				// Play pickup if we gave him anything
				playSound(SoundPickupItem,GameBase::getPosition(%this));
			}
		}			
		else if($TALT::Active == false) //New LT code 
		{
			// 2 live players colliding. Annihilation stuff. 
			%cliendId = Player::getClient(%obj);
			%thisId = Player::getClient(%this);
			%armor = Player::getArmor(%obj);
			eval(%armor @ "::onPlayerContact(" @ %this @ ", " @ %obj @ ");");	
			
			if(GameBase::getTeam(%obj) == GameBase::getTeam(%this))
			{
				if(%this.cloaked > 0)
				{
					GameBase::startFadein(%this);	
					%this.cloaked = "";
				}
			}			
			if(getSimTime() - %this.lastImpact < 1 && getObjectType(%obj) == "Player")
			{
				%Tm = player::getarmor(%this).mass;
				%Om = player::getarmor(%obj).mass;
				%m = %tm/%om;	//weight ratio between armors
				
				%vel = vector::multiply(%this.lastVel,%m@" "@%m@" "@%m);
				%speed = vector::getdistance("0 0 0",%vel);
						
				if(%speed > 100 && GameBase::getTeam(%this) != GameBase::getTeam(%obj) )
				{
					// Tackle deaths.
					playSound(shockExplosion,GameBase::getPosition(%this));
				//	GameBase::playSound(%player, shockExplosion, 0);
				//	Item::setVelocity(%obj,0);				
					%dead = Client::getName(GameBase::getControlClient(%obj));
					%killer = Client::getName(GameBase::getControlClient(%this));
					Item::setVelocity(%obj,"0 0 1");
						Player::blowUp(%obj);		
					Player::kill(%obj);
					%message = %killer @ " tackled " @ %dead @" to death at "@%speed @"mph";
					Anni::Echo(%message);
					messageall(0,%message);
					Player::setAnimation(%obj,$animNumber++);
										
					return;
				}
				else if(%speed > 65)
					playSound(soundArmorCrunch,GameBase::getPosition(%this));		
				else if(%speed > 45)
					playSound(soundArmorCrash,GameBase::getPosition(%this));
				else if(%speed > 25)	{}
				else if(%speed > 10)
					playSound(soundArmorSmack,GameBase::getPosition(%this));
				else 	
					playSound(soundArmorSlap,GameBase::getPosition(%this));
			
				Item::setVelocity(%obj,%vel);
			//	messageall(1,"bam? "@%m@", "@%speed@", "@%impactsound);				
			}		
		}
	}
	else
	{
		//player collision with something besides a player
		if(!Player::isDead(%this))
		{
			if(GameBase::getTeam(%obj) == GameBase::getTeam(%this))
			{
				if($ArmorName[Player::getArmor(%this)] == iarmorBuilder)		
				{
					//Get rid of builder repair for now				
					//if(GameBase::getDamageLevel(%obj))
					//{
					//	//%this.repairTarget = %obj;	
					//	GameBase::repairDamage(%obj, 0.07);
					//	GameBase::playSound(%this, ForceFieldOpen,0);
					//}
					//else if(%this.repairTarget == %obj)
					//{
					//	//we repaired this last, eh?
					//	RepairRewards(%this);	
					//}
				}
			}	
		}
		else
		{
			//dead player health kit?
		}			
	}
}


function Player::getHeatFactor(%this)
{
	DebugFun("Player::getHeatFactor",%this);

	%client = Player::getClient(%this);
	// new spoonbot below death666
  if (Player::isAIControlled(%this))		//If it's a bot and it's jetting, then by all means DESTROY HIM!
  {
        if ($Spoonbot::BotJettingHeat[%client] == 1)
	{
		return 1.0;
	}
  }	
	// end new spoonbot stuff
	
	
	%control = Client::getControlObject(%client);
	if(%control != %this)
	{
		%data = gamebase::getdataname(%control);
		if(%client.AdminobserverMode == "AdminObserve")		
			return 0.0;
		
		if(%data == OSMissile || %data == ProbeDroid || %data == SuicideDroid || %data == SurveyDroid)
			return 0.0;
				
		else	return 1.0;
	}
	
	// more spoonbot
	  if ((Client::getControlObject(%client) != %this) && (!Player::isAIControlled(%this))) 
	{
	 return 1.0;
	}
	// end more spoonbot
	
	%time = getIntegerTime(true) >> 5;
	%lastTime = Player::lastJetTime(%this) >> 10;

	if((%lastTime + 1.5) < %time) 
	{
		return 0.0;
	}
	else 
	{
		%diff = %time - %lastTime;
		%heat = 1.0 - (%diff / 1.5);
		return %heat;
	}
}

function Player::jump(%this,%mom)
{
	%cl = GameBase::getControlClient(%this);
	if(%cl != -1)
	{
		%vehicle = Player::getMountObject (%this);
		%this.lastMount = %vehicle;
		%this.newMountTime = getSimTime() + 3.0;
		Player::setMountObject(%this, %vehicle, 0);
		Player::setMountObject(%this, -1, 0);
		Player::applyImpulse(%pl,%mom);
		playSound (GameBase::getDataName(%this).dismountSound, GameBase::getPosition(%this));
	}
}


//----------------------------------------------------------------------------

function remoteKill(%client)
{
	if( CheckEval("remoteKill", %client) )
		return;
		
	if(!$matchStarted || %client.observerMode != "")
		return;

	%player = Client::getOwnedObject(%client);
	if (%player == -1 || %player == "")
		return;
	if(%player.frozen == true || $jailed[%player] == true || %player.arenajug == true)
	{
		client::sendmessage(%client,2,"WARDEN: Not on my watch son...");
		return;
	}
	if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
	{
		if(Player::getMountedItem(%player,$BackpackSlot) == SuicidePack) 
		{
			Player::unmountItem(%player,$BackpackSlot);
			%obj = newObject("","Mine","Suicidebomb");
			addToSet("MissionCleanup", %obj);
			%client = Player::getClient(%player);
			GameBase::throw(%obj,%player,9 * %client.throwStrength,false);
		}
		else 
		{
			playNextAnim(%client);
				
			Player::kill(%client);
			Client::onKilled(%client,%client);
		}

		if(%client.droid)
		{
			%obj = %client.droid;
			GameBase::setDamageLevel(%obj,1);	
		}
	}
	schedule("Client::clearItemShopping("@%client@");",0.5,%client);	

}

$animNumber = 25;
function playNextAnim(%client)
{
	if($animNumber > 36) 
		$animNumber = 25;
	Player::setAnimation(%client,$animNumber++);
}
function Client::takeControl(%clientId, %objectId)
{
	DebugFun("Client::takeControl", %clientId, %objectId);
	// remote control
	if(%objectId == -1)
	{
		//Anni::Echo("objectId = " @ %objectId);
		return;
	}

	%pl = Client::getOwnedObject(%clientId);
	// If mounted to a vehicle then can't mount any other objects
	if(%pl.driver != "" || %pl.vehicleSlot != "")
		return;


	if(GameBase::getTeam(%objectId) != Client::getTeam(%clientId))
	{
		//Anni::Echo(GameBase::getTeam(%objectId) @ " " @ Client::getTeam(%clientId));
		return;
	}
	if(GameBase::getControlClient(%objectId) != -1)
	{
		//Anni::Echo("Ctrl Client = " @ GameBase::getControlClient(%objectId));
		return;
	}
	if(GameBase::getDamageState(%objectId) != "Enabled")
		return;

	Turret::onAttemptControl(%objectId, %clientId);
}

function remoteCmdrMountObject(%clientId, %objectIdx)
{
	if( CheckEval("remoteCmdrMountObject", %clientId, %objectIdx) )
		return;

	%objectIdx = floor(%objectIdx);

	if ( %objectIdx > 200 || %objectIdx < 0 )
	{
		CrashAttemptLog(%clientId, "Incorrect Object ID", "remoteCmdrMountObject", %objectIdx);
		return;
	}

	%objectIdx = AntiCrash::getObjectByTargetIndex(%objectIdx);
	
	if ( isObject(%objectIdx) )
		Client::takeControl(%clientId, %objectIdx);
		
}

function checkControlUnmount(%clientId)
{
	%ownedObject = Client::getOwnedObject(%clientId);
	%ctrlObject = Client::getControlObject(%clientId);
	if(%ownedObject != %ctrlObject)
	{
		if(%ownedObject == -1 || %ctrlObject == -1)
			return;
		if(getObjectType(%ownedObject) == "Player" && Player::getMountObject(%ownedObject) == %ctrlObject)
			return;
		Client::setControlObject(%clientId, %ownedObject);
	}
	GameBase::virtual(%ctrlObject, onDismount, %ctrlObject, %clientID);
}

function getPerfectTrans(%player)
{
        $los::position = "";
        GameBase::getLOSInfo(%player,3000);
        %rot = GameBase::getRotation(%player);
        %targetPos = $los::position;
        $los::position = "";
        %trans = GameBase::getMuzzleTransform(%player);
        %vec0 = getWord(%trans,0);
        %vec1 = getWord(%trans,1);
        %vec2 = getWord(%trans,2);
        %vec3 = getWord(%trans,6);
        %vec4 = getWord(%trans,7);
        %vec5 = getWord(%trans,8);
        %vec6 = getWord(%trans,9);
        %vec7 = getWord(%trans,10);
        %vec8 = getWord(%trans,11);
        %rot1 = %vec3@" "@%vec4@" "@%vec5;
        %pos1 = %vec6@" "@%vec7@" "@%vec8;
        %rot0 = Vector::getRotAim(%Pos1,%targetPos);
        %vec = Vector::getFromRot(%rot0,1);
        if(%targetPos)
                %trans = %vec0@" "@%vec1@" "@%vec2@" "@%vec@" "@%rot1@" "@%pos1;
        return %trans;
}