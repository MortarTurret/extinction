//dropshipfunctions now below
StaticShapeData ShipAntenna
{
	shapeFile = "vehi_pur_poles"; // anten_small
	debrisId = flashDebrisLarge;
	explosionId = mortarExp;
	maxDamage = 20.0;
	shieldShapeName = "shield";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	explosionId = flashExpMedium;
	description = "Gunship Landing Pad";
	maxEnergy = 200;
	mapFilter = 2;
	visibleToSensor = true;
	lightRadius = 12.0;
	lightType=2;
	lightColor = {1.0,0.2,0.2};
};

function DropShip::deployShape(%player,%name,%item,%length,%width,%height,%offset)
{
	%clientId = Player::getClient(%player);
	%team = GameBase::getTeam(%player);
	%playerPos = GameBase::getPosition(%player);
	
	if($TeamItemCount[%team @ %item] >= $TeamItemMax[%item]) 
	{	
		Client::sendMessage(%clientId,0,"Deployable Item limit reached for " @ %item.description @ "s~waccess_denied.wav");
		return false;
	}
	if(!GameBase::getLOSInfo(%player,8)) // 5
	{	
		Client::sendMessage(%clientId,0,"Deploy position out of range ~waccess_denied.wav");
		return false;
	}

	%pos = $los::position;
//	if(!checkDeployArea(%player,%pos))
//	{
//		Client::sendMessage(%clientId,0,"You're in the way.");
//		return false;
//	}

	%obj = getObjectType($los::object);
	if(%obj != "SimTerrain") 
	{	
		Client::sendMessage(%clientId,0,"Can only deploy on terrain. ~waccess_denied.wav");
		return false;
	}

	if(getNumTeams()-1 == 2)
	{
		if(%team == 0)
			%enemyTeam = 1;
		else if(%team == 1)
			%enemyTeam = 0;

		if(Vector::getDistance($teamFlag[%enemyTeam].originalPosition, %playerpos) < 150)
		{
			Client::sendMessage(%clientId,0,"You are too close to the enemy base to deploy that.");
			Client::sendMessage(%clientId,0,"~wC_BuySell.wav");
			return false;
		}
	}

	%flagpos = $teamFlag[%team].originalPosition;	
	if(Vector::getDistance(%flagpos, %playerpos) < 150)
	{
		Client::sendMessage(%clientId,0,"You are too close to your flag, Must be further from flag to deploy.");
		Client::sendMessage(%clientId,0,"~wC_BuySell.wav");
		return false;
	}

		if(!$build)
		{
	if(%player.outArea)
	{
		Client::sendMessage(%client,0,"can not deploy out of bounds unless building is on.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
		}

// check for deployed ships.

	if(getNumTeams()-1 == 2)
	{
	%teamnumzero = 0;
	%teamnummone = 1;
	}
	
if(((($DropShipMultipass1[%teamnumzero] == true) || ($DropShipMultipass2[%teamnumzero] == true) || ($DropShipMultipass1[%teamnummone] == true) || ($DropShipMultipass2[%teamnummone] == true))))
{
	%strekpos = $DropShipLocation[%teamnumzero @ "GunShip"];
	%strekpos2 = $DropShipLocation[%teamnummone @ "GunShip"];
	if(((Vector::getDistance(%strekpos, %playerpos)) < 75) && (%strekpos != "")) 
	{
		Client::sendMessage(%clientId,0,"You are too close to a Gunship landing zone or crash site. ~waccess_denied.wav");
		return false;
	}
	if(((Vector::getDistance(%strekpos2, %playerpos)) < 75) && (%strekpos2 != "")) 
	{
		Client::sendMessage(%clientId,0,"You are too close to a Gunship landing zone or crash site. ~waccess_denied.wav");
		return false;
	}
}

	if(!DropShip::CheckDeployArea(%clientId,%pos,%length,%width,%height,%offset))
		return false;

	if(%clientId.waitmsgHP == "false")
	{
		Client::sendMessage(%clientId,0,"Please Wait A Few Seconds To Re-Deploy The Gunship or find a clear area to deploy. ~waccess_denied.wav");
		return;
	}

if(%clientId.waitmsgHP)
{
	%clientId.waitmsgHP = false;
	schedule(%clientId@".waitmsgHP= true;",1.0,%clientId);

	if(!Drophip::CheckDeployArea(%player,%pos,%name))
	{	
		Client::sendMessage(%clientId,0,"Object interference.");
		return;
	}
	
	%no = False;
	%set = newObject("set",SimSet);
	addToSet("MissionCleanup", %set);
	%Mask = $SimPlayerObjectType|$StaticObjectType|$VehicleObjectType|$MineObjectType|$SimInteriorObjectType; 
	%num = containerBoxFillSet(%set,%Mask,vector::add($los::position, "0 0 150"),80,80,80,0); //100
	%totalnum = Group::objectCount(%set);
	if ( %totalnum )
		%no = True;
	deleteObject(%set);

	if ( %no )
	{
		Client::sendMessage(%clientId, 0, "Object in the way of the landing zone.");
		Client::sendMessage(%clientId,0,"~wC_BuySell.wav");
		return False;
	}

	DropShip::CreateSimGroup(%name,%team);
	
	%obj = newObject("Sensor1",StaticShape,ShipAntenna,false);

	addToSet("MissionCleanup",%obj);
	%team = GameBase::getTeam(%player);
	GameBase::setTeam(%obj,%team);
	GameBase::setPosition(%obj,%pos);
	Gamebase::setMapName(%obj,%name @ " Landing Pad");
	$DropShipMultipass2[%team] = "true";
	%rot = "0 0 "@getWord(GameBase::getRotation(%player),2) + 3.14;
	gamebase::setRotation(%obj,%rot);
//	Gamebase::setMapName(%obj,%name@%team);
	$DropShipLocation[%team @ %name] = %pos;
	$DropShipMultipass1[%team] = "true";
	$DropShip[%name@%team] = %pos;
	$DropShipBeacon[%name@%team] = %obj;
	%data = GameBase::getDataName(%obj);	
	schedule("GameBase::setDamageLevel(" @ %obj @ "," @ %data.maxDamage @ ");", 46,%obj); //antenna beacon destroy death666
	%PlayerName = Client::getName(%clientId);
	 Client::sendMessage(%clientId,0,"~wturretOff1.wav"); // NEW
	DropshipTeamMessage(%team, 1, %PlayerName@" deployed a "@%name@" Landing Pad.");
	DropshipTeamMessage(%team, 1, "00:15 Seconds until Gunship hyper-jump arrival. Protect the Landing Pad!");
	Client::sendMessage(%clientId,0,"Gunship Landing Beacon deployed. Protect it until the ships arrival from deep space in Fifteen seconds.");
	$TeamItemCount[%team @ %item]++;
	 Anni::Echo("$TeamItemCount[%team @ %item] "@%team @ %item);
	 Anni::Echo("MSG: ",%clientId," deployed a " @ %name @ " Beacon");
	
	return %obj;

}
}

function rotateVector(%vec,%rot){
	%pi = 3.14;
	%rot3= getWord(%rot,2);
	for(%i = 0; %rot3 >= %pi*2; %i++) %rot3 = %rot3 - %pi*2;
	if (%rot3 > %pi) %rot3 = %rot3 - %pi*2;

	%vec1= getWord(%vec,0);
	%vec2= getWord(%vec,1);
	%vc = %vec2;
	%vec3= getWord(%vec,2); 
	%ray = %vec1;
	
	%vec1 = %ray*cos(%rot3);
	%vec2 = %ray*sin(%rot3);
	%vec = %vec1 @" "@ %vec2 @" "@ %vec3;
	%vec = Vector::add(%vec,Vector::getFromRot(%rot,%vc,0));
	return %vec;
	}

function ShipAntenna::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{

	%GunShipDropTeam = GameBase::getTeam(%this);
	%client = Player::getClient(%object);
	%AttackingGSTeam = GameBase::getTeam(%client);
	%name = GameBase::getDataName(%this);
	%player = Client::getOwnedObject(%client);
	%damageLevel = GameBase::getDamageLevel(%this);
	%pos = GameBase::getPosition(%this);

		if(%AttackingGSTeam == %GunShipDropTeam)
		{
			%dValue = %damageLevel + %value * $Server::TeamDamageScale;
			GameBase::setDamageLevel(%this,%dValue);
		}
		if(%AttackingGSTeam != %GunShipDropTeam)
		{
			%dValue = %damageLevel + (%value/2);	
			GameBase::setDamageLevel(%this,%dValue);
		}	
}

function ShipAntenna::onRemove(%this)
{
	%team = GameBase::getTeam(%this);
	$DropShipMultipass2[%team] = "false";
}

function ShipAntenna::onDestroyed(%this)
{		
		GameBase::playSound(%this, SoundrocketExplosion, 0);
		%this.nuetron = "";
		%team = GameBase::getTeam(%this);
		%name = Gamebase::getMapName(%this);
		$DropShipBeacon[%name] = "";
		calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 3, 0.55, 0.30, 250, 170);
		GameBase::startFadeOut(%this);
		schedule("deleteobject(" @ %this @ ");", 2, %this); // 2
		$DropShipMultipass2[%team] = "false";
		
		%DropTeam = GameBase::getTeam(%this);
		%client = Player::getClient(%object);
		%KillerTeam = GameBase::getTeam(%client);

		if(%KillerTeam == %DropTeam)
		{
			bottomprint(%client, "<jc>You have just LOST <f2>2<f0> POINTS for destroying your teams Gun Ship Landing Pad."); // "@ %name.className	
	
			 Anni::Echo("MSG: ", %client, " Destroyed a team ",  %name.description);
			DropshipTeamMessage(%KillerTeam, 3, "Team member "@Client::getName(%client)@" has destroyed your teams Gun Ship Landing Pad."); // "@ %name.description);
			%client.score-=2;
			Game::refreshClientScore(%client);		
		}
		else
		{
			if(%this.LastRepairCl !=  %client)
			{	
				bottomprint(%client, "<jc>You have just recieved <f2>2<f0> POINTS for destroying the enemys Gun Ship Landing Pad."); // "@ %name.description);
				%client.score+= 2;
				%client.Credits+= 2;
				Game::refreshClientScore(%client);			
			}	
			 Anni::Echo("MSG: ", %client, " Destroyed enemy  ",  %name.description);
//			DropshipTeamMessage(%DropTeam, 3, "WARNING "@Client::getName(%client)@" has destroyed your teams Gun Ship Landing Pad."); // 's "@ %name.description);
		}					
}

function DropshipTeamMessage(%team, %color, %msg)
{
	%numPlayers = getNumClients();
	for(%i = 0; %i < %numPlayers; %i++)
	{
		%cl = getClientByIndex(%i);
		%now = Client::getTeam(%cl);
		if(%team == %now)
		{
			Client::sendMessage(%cl,%color,%msg);
		}
	}
}

function DropShip::CheckDeployArea(%clientId,%pos,%length,%width,%height,%offset)
{
	%set=newObject("set",SimSet);
	%num=containerBoxFillSet(%set,$StaticObjectType | $SimInteriorObjectType ,%pos,%length,%width,%height,%offset);
	if(!%num) 
	{
		deleteObject(%set);
		return true;
	}
	else if(%num == 1 && getObjectType(Group::getObject(%set,0)) == "Player") 
	{
		return true;
	}
	else if(%num == 1)
	{
		%obj = Group::getObject(%set,0);
		%type = getObjectType(%obj);
		Client::sendMessage(%clientId,0,"Unable to deploy - unclear landing zone ~waccess_denied.wav");
	}
	else if(%num > 1)
		Client::sendMessage(%clientId,0,"Unable to deploy - Multiple items in the landing zone ~waccess_denied.wav");
	else
		Client::sendMessage(%clientId,0,"Unable to deploy - Item in the way ~waccess_denied.wav");
	deleteObject(%set);
	return false;
}

function DropShip::CreateSimGroup(%name,%team)
{
	%teleset = nameToID("MissionCleanup/"@%name@%team);
	if(%teleset == -1)
	{	
		%group = newObject(%name@%team,SimGroup);
		addToSet("MissionCleanup",%group);
	}
}
//$DropShipPos[%team @ %shipname @ "ShipPack"] = "";
function DropShip::CreateShip(%name,%shape,%pos,%team)
{
	$DropShipPos[%team @ %name] = %pos;	//CommandShip,GunShip,SupplyShip
	$BombPos[%team @ %name] = "";
	$BombPos[%team @ %name] = %pos;	//CommandShip,GunShip,SupplyShip //new
	$ShutdownBay = $BombPos[%team @ %name];
	$ship = $ship +1;

	%obj = newObject(%name,InteriorShape,%shape);
	GameBase::startFadeIn(%obj);
	 Anni::Echo("Created Dropship name: "@ %name @ " object#: " @ %obj);
	addToSet("MissionCleanup/"@%name@%team, %obj);
	GameBase::setPosition(%obj,%pos);
	GameBase::setTeam(%obj,%team);
	return(%obj);
}

function DropShip::MoveShip(%this,%steps,%name)
{	
	
	%movement = 0.169000; // 0.100000
	%pos = GameBase::GetPosition(%this);
	%team = GameBase::getTeam(%this);
	%VertPos = getWord(%pos,2);
	
	%pos = Vector::add(%pos,"0 0 -" @ %movement);
	GameBase::SetPosition(%this,%pos);
	%steps--;
	
	if($debug)
		 Anni::Echo("moveship "@$DropShipPos[%team @ %name]);
		
	//$DropShipPos[%team @ %name] = %pos;	//CommandShip,GunShip,SupplyShip

	if(%steps > 0)
		schedule("DropShip::MoveShip(" @ %this @ "," @ %steps @ "," @ %name @ ");", 0.01, %this);
	else if( %VertPos <= getWord($DropShipPos[%team @ %name],2))	
		{
		%this.inmotion = "";
		schedule(%name@"::DeployItems(" @ %this @ ");", 2.5, %this); // 3
		$DropShipPos[%team @ %name] = "";	// were done moving, so we can delete this..
		}
}



function DropShipGen::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	if($debug)
		 Anni::Echo("DropShipGen::onDamage "@%this);
	
	if(GameBase::getDamageState(%this) == "Destroyed" || %value <= 0)
	 	return;

	%GunShipDropTeam = GameBase::getTeam(%this);
	%client = Player::getClient(%object);
	%AttackingGSTeam = GameBase::getTeam(%client);
	%name = GameBase::getDataName(%this);
	%player = Client::getOwnedObject(%client);
	%damageLevel = GameBase::getDamageLevel(%this);
	%pos = GameBase::getPosition(%this);

		if(%AttackingGSTeam == %GunShipDropTeam)
		{
			%dValue = %damageLevel + %value * $Server::TeamDamageScale;
			GameBase::setDamageLevel(%this,%dValue);
		}
		if(%AttackingGSTeam != %GunShipDropTeam)
		{
			%dValue = %damageLevel + (%value/2);	
			GameBase::setDamageLevel(%this,%dValue);
		}

	
	if(GameBase::getDamageState(%this) == "Destroyed") 
	{
		//gen damage
		calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 3, 0.55, 0.30, 250, 170);		
		GameBase::playSound(%this, SoundrocketExplosion, 0);
		
		%DropTeam = GameBase::getTeam(%this);
		%client = Player::getClient(%object);
		%KillerTeam = GameBase::getTeam(%client);
		 Anni::Echo("destroy gunship gen");	
		if(%KillerTeam == %DropTeam)
		{
			bottomprint(%client, "<jc>You have just LOST <f2>2<f0> POINTS for destroying your teams Communications Array."); // "@ %name.className	
	
			 Anni::Echo("MSG: ", %client, " Destroyed a team ",  %name.description);
			DropshipTeamMessage(%KillerTeam, 3, "Team member "@Client::getName(%client)@" has destroyed your teams Gunship Generator."); // "@ %name.description);
			%client.score-=2;
			Game::refreshClientScore(%client);		
		}
		else
		{
			if(%this.LastRepairCl !=  %client)
			{	
				bottomprint(%client, "<jc>You have just recieved <f2>2<f0> POINTS for destroying the enemys Communication Array."); // "@ %name.description);
				%client.score+= 2;
				%client.Credits+= 2;
				Game::refreshClientScore(%client);			
			}	
			 Anni::Echo("MSG: ", %client, " Destroyed enemy  ",  %name.description);
			DropshipTeamMessage(%DropTeam, 3, "WARNING "@Client::getName(%client)@" has destroyed your teams Gunship Generator."); // 's "@ %name.description);
		}
		messageAll(0, "Leak detected in "@ %name.description@". Location Scrambling will fail in "@$DropshipExplosionTime@" seconds!! ~wCapturedTower.wav");
		WarpcoreCheck(%this,$DropshipExplosionTime);					
	}	
}


//BlowShitUp(%this,%dettime, %radius)
function WarpCoreCheck(%this,%time)
{	
	%time--;
	%name = GameBase::getDataName(%this);
	%shipname = getWord(%name.description,0);	
	if(GameBase::getDamageState(%this) == "Destroyed") 
	{

		if(%time == 15)
		{
			PreBlowShitUp(%this,15);
		}

		if(%time <= 0)
		{
			%team = GameBase::getTeam(%this);
		 	$Gspot2 = $GSpot[%team];
			StarwolfAttack($GSpot2);			
			BlowShitUp(%this,1.2, 100.0);
			$TeamItemCount[%team @ GunShipPack]--; // NEW
		}
		else
		{
			schedule("WarpCoreCheck("@%this@","@%time@");",1,%this);
		}	
	}	
	else
		messageAll(0, "Leak in "@%shipname@" Gunship Generator repaired. Location scrambling is secure.");
		


}

MineData DSSmogSmoke
{
   	mass = 0.3;
   	drag = 1.0;
   	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
   	description = "Handgrenade";
   	shapeFile = "grenade";
   	shadowDetailMask = 4;
   	explosionId = WickedBadExp;
	explosionRadius = 40.0;
	damageValue = 9999.0;
	damageType = $MortarDamageType;
	kickBackStrength = 250;
	triggerRadius = 0.5;
	maxDamage = 9999.0;
};

MineData StarwolfShell
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Tank Bomb";
	shapeFile = "mortarammo";
	shadowDetailMask = 4;
	explosionId = LargeShockwave;
	explosionRadius = 30.0;
	damageValue = 0.8;
	damageType = $MortarDamageType;
	kickBackStrength = 2000; //eggs.. death666
	triggerRadius = 0.5;
	maxDamage = 1.0;
};

MineData DropHalo
{
	mass = 5.0;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "DropHalo";
	shapeFile = "force";
	shadowDetailMask = 4;
	explosionId = LargeShockwave;
	explosionRadius = 30.0;
	damageValue = 0.8;	//0.5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 700;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};

function StarwolfAttack(%pos)
{
	%targetspot = $GSpot2;
	%DropAgain = Vector::add(%targetspot,"0 0 500");


	%obj = Projectile::SpawnProjectile("TankMissile","0 0 0 0 0 0 0 0 0 "@%DropAgain, 2048, 0); // AirstrikeShell
	Projectile::spawnProjectile(%obj);
	Item::setVelocity(%obj, "0 0 -400");

	%DropTwo = Vector::add(%targetspot,"0 10 500");

	%obj = Projectile::SpawnProjectile("TankMissile","0 0 0 0 0 0 0 0 0 "@%DropTwo, 2048, 0);
	Projectile::spawnProjectile(%obj);
	Item::setVelocity(%obj, "0 0 -400");

	%DropThree = Vector::add(%targetspot,"0 -10 500");
	%obj = Projectile::SpawnProjectile("TankMissile","0 0 0 0 0 0 0 0 0 "@%DropThree, 2048, 0);
	Projectile::spawnProjectile(%obj);
	Item::setVelocity(%obj, "0 0 -400");

	%DropAgain = Vector::add(%targetspot,"0 0 500");
	%DropSmoke1 = NewObject("",Mine,"StarwolfShell");
	AddToSet("MissionCleanup",%DropSmoke1);
	GameBase::setPosition(%DropSmoke1,%DropAgain);
	GameBase::setRotation(%DropSmoke1,"0 0 0");
	schedule("Mine::Detonate(" @ %DropSmoke1 @ ");",0.1); // 0.1

	%obj = Projectile::SpawnProjectile("TrollBurn2","0 0 0 0 0 0 0 0 0 "@%DropAgain, 2048, 0);
	Projectile::spawnProjectile(%obj);
	Item::setVelocity(%obj, "0 0 -400");

	%obj = Projectile::SpawnProjectile("TrollBurn2","0 0 0 0 0 0 0 0 0 "@%DropAgain, 2048, 0);
	Projectile::spawnProjectile(%obj);
	Item::setVelocity(%obj, "0 0 -390");

}

function DSExplosion(%pos)
{
	// messageAll(0, "Hi lets be friends ");
	%lockontarget = $GSpot2;

	%SmokeAgain = Vector::add(%lockontarget,"0 10 0");
	%SmogSmoke1 = NewObject("",Mine,"DSSmogSmoke");
	AddToSet("MissionCleanup",%SmogSmoke1);
	GameBase::setPosition(%SmogSmoke1,%SmokeAgain);
	GameBase::setRotation(%SmogSmoke1,"0 0 0");
	schedule("Mine::Detonate(" @ %SmogSmoke1 @ ");",0.1);

	%SmokeAgain = Vector::add(%lockontarget,"0 -10 0");
	%SmogSmoke1 = NewObject("",Mine,"DSSmogSmoke");
	AddToSet("MissionCleanup",%SmogSmoke1);
	GameBase::setPosition(%SmogSmoke1,%SmokeAgain);
	GameBase::setRotation(%SmogSmoke1,"0 0 0");
	schedule("Mine::Detonate(" @ %SmogSmoke1 @ ");",0.2);

	%SmokeAgain = Vector::add(%lockontarget,"10 0 0");
	%SmogSmoke1 = NewObject("",Mine,"DSSmogSmoke");
	AddToSet("MissionCleanup",%SmogSmoke1);
	GameBase::setPosition(%SmogSmoke1,%SmokeAgain);
	GameBase::setRotation(%SmogSmoke1,"0 0 0");
	schedule("Mine::Detonate(" @ %SmogSmoke1 @ ");",0.3);

	%SmokeAgain = Vector::add(%lockontarget,"-10 0 0");
	%SmogSmoke1 = NewObject("",Mine,"DSSmogSmoke");
	AddToSet("MissionCleanup",%SmogSmoke1);
	GameBase::setPosition(%SmogSmoke1,%SmokeAgain);
	GameBase::setRotation(%SmogSmoke1,"0 0 0");
	schedule("Mine::Detonate(" @ %SmogSmoke1 @ ");",0.4);

	%SmogSmoke1 = NewObject("",Mine,"DSSmogSmoke");
	AddToSet("MissionCleanup",%SmogSmoke1);
	GameBase::setPosition(%SmogSmoke1,%lockontarget);
	GameBase::setRotation(%SmogSmoke1,"0 0 0");
	schedule("Mine::Detonate(" @ %SmogSmoke1 @ ");",0.5);

	%LetsSmoke = Vector::add(%lockontarget,"0 -15 0");
	%obj = Projectile::SpawnProjectile("Smokegrenade","0 0 0 0 0 0 0 0 0 "@%LetsSmoke, 2048, 0);
	Projectile::spawnProjectile(%obj);

	%LetsSmoke2 = Vector::add(%lockontarget,"0 20 0");
	%obj = Projectile::SpawnProjectile("Smokegrenade","0 0 0 0 0 0 0 0 0 "@%LetsSmoke2, 2048, 0);
	Projectile::spawnProjectile(%obj);

	%LetsSmoke3 = Vector::add(%lockontarget,"-15 0 0");
	%obj = Projectile::SpawnProjectile("Smokegrenade","0 0 0 0 0 0 0 0 0 "@%LetsSmoke3, 2048, 0);
	Projectile::spawnProjectile(%obj);

	$Gspot2 = "";
}

function LandingExp(%pos)
{
	%BangSpot = $ExplosionSpot2;

	%SmokeAgain = Vector::add(%BangSpot,"0 0 15");
	%SmogSmoke1 = NewObject("",Mine,"DropHalo");
	AddToSet("MissionCleanup",%SmogSmoke1);
	GameBase::setPosition(%SmogSmoke1,%SmokeAgain);
	GameBase::setRotation(%SmogSmoke1,"0 0 0");
	schedule("Mine::Detonate(" @ %SmogSmoke1 @ ");",0.1);

	%SmokeD = Vector::add(%BangSpot,"0 0 -15");
	%SmogSmoke1 = NewObject("",Mine,"DropHalo");
	AddToSet("MissionCleanup",%SmogSmoke1);
	GameBase::setPosition(%SmogSmoke1,%SmokeD);
	GameBase::setRotation(%SmogSmoke1,"0 0 0");
	schedule("Mine::Detonate(" @ %SmogSmoke1 @ ");",0.1);

	%BangAgain = Vector::add(%BangSpot,"0 25 -5");
	%obj = Projectile::SpawnProjectile("TrollBurn2","0 0 0 0 0 0 0 0 0 "@%BangAgain, 2048, 0);
	Projectile::spawnProjectile(%obj);
	Item::setVelocity(%obj, "0 0 -100");

	%BangAgain2 = Vector::add(%BangSpot,"0 -25 -5");
	%obj = Projectile::SpawnProjectile("TrollBurn2","0 0 0 0 0 0 0 0 0 "@%BangAgain2, 2048, 0);
	Projectile::spawnProjectile(%obj);
	Item::setVelocity(%obj, "0 0 -100");

	%BangAgain3 = Vector::add(%BangSpot,"25 0 -5");
	%obj = Projectile::SpawnProjectile("TrollBurn2","0 0 0 0 0 0 0 0 0 "@%BangAgain3, 2048, 0);
	Projectile::spawnProjectile(%obj);
	Item::setVelocity(%obj, "0 0 -100");

	%BangAgain4 = Vector::add(%BangSpot,"-25 0 -5");
	%obj = Projectile::SpawnProjectile("TrollBurn2","0 0 0 0 0 0 0 0 0 "@%BangAgain4, 2048, 0);
	Projectile::spawnProjectile(%obj);
	Item::setVelocity(%obj, "0 0 -100");

	$ExplosionSpot2 = "";
}

$DropshipExplosionTime = 30; // 45 -death666

function PreBlowShitUp(%this,%dettime)
{
	%group = GetGroup(%this);
	%team = GameBase::getTeam(%this);
	%name = GameBase::getDataName(%this); // new
	Messageall(1,"The " @ $Server::teamName[GameBase::getTeam(%this)] @ " " @ GameBase::getDataName(%this).description @ " is going to be targeted in " @ %dettime @ " seconds!! Orbital Attack Incoming!!");
}

function BlowShitUp(%this,%dettime, %radius)
{
	%group = GetGroup(%this);
	%team = GameBase::getTeam(%this);
	%name = GameBase::getDataName(%this); // new
	$DropShipLocation[%team @ "GunShip"] = "";
	$DropShipMultipass1[%team] = "false";
	$DropShipMultipass2[%team] = "false";
	Messageall(1,"Missiles Detected in the atmosphere above " @ $Server::teamName[GameBase::getTeam(%this)] @ " " @ GameBase::getDataName(%this).description @ ". ");

		 $Gspot2 = $GSpot[%team];
	       schedule("DSExplosion($GSpot2);",1.7); //1.3

	for(%i = 0; (%obj = Group::getObject(%group, %i)) != -1; %i++)
	{
		if(%obj.group)
		{
			 Anni::Echo("found sub group for "@GameBase::getDataName(%obj));
			for(%j = 0; (%object = Group::getObject(%obj.group, %j)) != -1; %j++)
			{		
				%data = GameBase::getDataName(%object);	
				Anni::Echo("sub group object ="@%data);
				if(%data.maxDamage && %object != %obj)

				schedule("GameBase::setDamageLevel("@%object@","@%data.maxDamage@");",1.1,%object);
			else schedule("deleteObject(" @ %object @ ");", 1.7, %object); // 1.2	
			}
		}
		
		else
		{
			%data = GameBase::getDataName(%obj);
			schedule("GameBase::setDamageLevel("@%obj@","@%data.maxDamage@");",1.1,%obj);	
		}	
		
 		schedule("GameBase::startFadeOut(" @ %obj @ ");", 1.1, %obj ); //3.0
		schedule("deleteObject(" @ %obj @ ");", 1.7, %obj); // 1.2
		
	}
}

function Drophip::CheckDeployArea(%player,%pos,%name)
{

	if(%name == GunShip)
		%obj = "gswdrop.dis";
	else 
		echo("Drophip::CheckDeployArea - error");
		
	%rot = "0 0 "@getWord(GameBase::getRotation(%player),2) + 3.14;
	%shipPos = vector::add(%pos,"0 0 5");
	%object = newObject(%name,"InteriorShape", %obj);
	addToSet("MissionCleanup", %object);
	
	GameBase::setRotation(%object,%rot);
	%test = GameBase::testPosition(%object,%shipPos);
	

	schedule("deleteobject("@%object@");",1);
	

	return %test;
	
}