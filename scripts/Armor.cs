
function Armor::add(%name, %description, %price)
{
	%aname = armorm@%name;
	%armorShape = %aname.shapeFile;
	




	if(%armorShape != false)
	{
		// defining light and med armors...
		%maxdamage = %aname.maxDamage;
		$ArmorType[Male,iarmor@%name] = armorm@%name;
		$ArmorType[Female, iarmor@%name] = armorf@%name;
		$ArmorName[armorm@%name] = iarmor@%name;
		$ArmorName[armorf@%name] = iarmor@%name;
		
		$InvList[iarmor@%name] = 1;
		$MobileInvList[iarmor@%name] = 1;
		$RemoteInvList[iarmor@%name] = 0;
	}
	else	
	{
		// defining harmor armors...
		%aname = armor@%name;
		%armorShape = %aname.shapeFile;
		if(%armorShape != false)
		{		
			%maxdamage = %aname.maxDamage;
			$ArmorType[Male,iarmor@%name] = armor@%name;
			$ArmorType[Female, iarmor@%name] = armor@%name;
			$ArmorName[armor@%name] = iarmor@%name;
			
			$InvList[iarmor@%name] = 1;
			$MobileInvList[iarmor@%name] = 1;
			$RemoteInvList[iarmor@%name] = 0;
		}
	}
	%ItemData = "ItemData iarmor"@ %name @" {    heading = \"aArmor\";    description = \""@ %description @"\";    className = Armor;   shapeFile = \""@%armorShape @"\"; price = "@ %price @"; };"; 	
	
	eval(%ItemData); 
		
	if(!%maxdamage)
		Anni::Echo("+++++ Armor::add("@%name@","@ %description@","@ %price@"); FAILED +++++ Check armor definitions.");

	AddItem("iarmor"@%name);	//add all items to a list for zappy/ quick invs.
}


function startCloak(%clientId) 
{

	
	%player = Client::getOwnedObject(%clientId);
	$PlayerCloaked[%player] = true;
	Client::sendMessage(%clientId,3,"Cloaking activated. ~wcommand_use.wav");
	GameBase::startFadeout(%player);
	%rate = Player::getSensorSupression(%player) + 3;
	Player::setSensorSupression(%player,%rate);
	if($cloakTime[%clientId] == 0) 
	{	$cloakTime[%clientId] = 15; //12
		checkPlayerCloak(%clientId);
	}
	else $cloakTime[%clientId] = 15; //12
}

function checkPlayerCloak(%clientId, %player) 
{
	%player = Client::getOwnedObject(%clientId);
	if(!Player::isDead(%player) && $cloakTime[%clientId] > 0) 
	{
		$cloakTime[%clientId] -= 1.5; 
		schedule("checkPlayerCloak(" @ %clientId @ ");",1.5,%player); 
			if(Player::getItemCount(%clientId,Flag) > 0) 
				{
				$cloakTime[%clientId] = 0;
				}
	}
	else 
	{
		$cloakTime[%clientId] = 0;
		%player.beaconcooldown = true;
		%player.usedcloakbeacon = false;
		$PlayerCloaked[%player] = false;
		Client::sendMessage(%clientId,1,"Your cloaking has ended. ~wSniper2.wav");
		schedule(%player@".beaconcooldown = false;",3.0,%player);
		GameBase::startFadein(%player);
		%rate = Player::getSensorSupression(%player) - 5;
		Player::setSensorSupression(%player,0);
	}
}

function divineintervention(%clientId, %player) 
{
	Client::sendMessage(%clientId,0,"You summon the powers of the Gods!");
	GameBase::repairDamage(%player,0.5);
	GameBase::playSound(%player,ForceFieldOpen,0);
	%player.shieldStrength = 0.200;
	%rate = Player::getSensorSupression(%player) + 5;
	Player::setSensorSupression(%player,%rate);
	if($shieldTime[%clientId] == 0) 
	{
		$shieldTime[%clientId] = 5;
		checkdivineintervention(%clientId, %player);
	}
	else
		$shieldTime[%clientId] = 5;
}

function checkdivineintervention(%clientId, %player) 
{
	if($shieldTime[%clientId] > 0 && !Player::isDead(%player)) 
	{
		$shieldTime[%clientId] -= 2;
		schedule("checkdivineintervention(" @ %clientId @ ", " @ %player @ ");",2,%player);
	}
	else 
	{
		$shieldTime[%clientId] = 0;
		Client::sendMessage(%clientId,0,"Your powers wear off.");
		%player.shieldStrength = 0;
		%rate = Player::getSensorSupression(%player) - 5;
		Player::setSensorSupression(%player,%rate);
		GameBase::playSound(%player,ForceFieldOpen,0);
	}
}

function Drain(%damagedPlayer, %damagingPlayer)
{
	if(GameBase::getTeam(%targetPlayer) == GameBase::getTeam(%sourcePlayer) || Player::isDead(%damagingPlayer))
		return;
	GameBase::applyDamage(%damagedPlayer,$EnergyDamageType,0.2,GameBase::getPosition(%damagedPlayer),"0 0 0","0 0 0",%damagingPlayer);
	GameBase::setEnergy(%damagedPlayer, GameBase::getEnergy(%damagedPlayer) - 80);
	%lev = GameBase::getDamageLevel(%damagingPlayer);
	if(%lev <0.2)
		GameBase::setDamageLevel(%damagingPlayer, 0);
	else
		GameBase::setDamageLevel(%damagingPlayer,%lev-0.2);
	GameBase::setEnergy(%damagingPlayer, GameBase::getEnergy(%damagingPlayer) + 120);
	GameBase::playSound(%damagedPlayer,ForceFieldOpen,0);
	Client::sendMessage(Player::getClient(%damagingPlayer), 1, "You suck out " @ Client::getName(Player::getClient(%damagedPlayer)) @ "'s life force");
}

function startShield(%clientId) 
{

	
	%player = Client::getOwnedObject(%clientId);
	GameBase::playSound(%player,ForceFieldOpen,0);
	%player.shieldStrength = 0.006;
	if($shieldTime[%clientId] == 0) 
	{
		$shieldTime[%clientId] = 18; //15
		checkPlayerShield(%clientId, %player);
	}
	else
		$shieldTime[%clientId] = 18; //15
}

function checkPlayerShield(%clientId) 
{

	
	%player = Client::getOwnedObject(%clientId);
	if($shieldTime[%clientId] > 0 && !Player::isDead(%player)) 
	{
		$shieldTime[%clientId] -= 1;
		schedule("checkPlayerShield(" @ %clientId @ ");",1,%player);
	}
	else 
	{
		%player.usedtshieldbeacon = false;
		Client::sendMessage(%clientId,1,"Emergency shields exausted.");
		$shieldTime[%clientId] = 0;
		%player.shieldStrength = 0;
		GameBase::playSound(%player,ForceFieldClose,0);
		%player.shieldCD = true;
		schedule(%player@".shieldCD = false;",3.0,%player);
	}
}

function Steal(%targetPlayer, %sourcePlayer)
{

	%client = Player::getClient(%targetPlayer);
	%client2 = Player::getClient(%sourcePlayer);

	if($ArmorName[Player::getArmor(%targetPlayer)] == $ArmorName[Player::getArmor(%sourcePlayer)])
	{
		
		%client = Player::getClient(%sourcePlayer);
		if(!String::ICompare(Client::getGender(%client), "Male"))
			GameBase::playSound(%client, MAaargh, 0);//client::sendmessage(%client, 0, "~wmale5.wdsgst4.wav");
		else 
			GameBase::playSound(%client, FAaargh, 0);//client::sendmessage(%client, 0, "~wfemale1.wdsgst4.wav");
		return;
	}
		
	if(GameBase::getTeam(%targetPlayer) == GameBase::getTeam(%sourcePlayer) || Player::isDead(%sourcePlayer))
		return;
		

	Player::setDamageFlash(%targetPlayer,1.95);
	Client::sendMessage(%client,1,Client::getName(%client2)@" has stolen your items! ~wError_Message.wav");
	Client::sendMessage(%client2,1,"You stole items from "@Client::getName(%client)@"");
	%sound = false;
	%max = getNumItems();
	for(%i = 0; %i < %max; %i = %i + 1)
	{
		%count = Player::getItemCount(%targetPlayer,%i);
		if(%count)
		{
			%item = getItemData(%i);//
			if(%item != flag)
			{
				%delta = Item::giveItem(%sourcePlayer,%item,%count);
				if(%delta > 0)
				{
					Annihilation::decItemCount(%targetPlayer,%i,%delta);
					%sound = true;
				}
			}
		}
	}
	if(%sound)
		playSound(SoundPickupItem,GameBase::getPosition(%sourcePlayer));
}

$AssassinateChance = 50; 

function Assassinate(%targetPlayer, %sourcePlayer)
{
	%Tteam = GameBase::getTeam(%targetPlayer);
	%Steam = GameBase::getTeam(%sourcePlayer);
	%SClient = Player::getClient(%sourcePlayer);
	if((%Tteam != %Steam && %SClient.isSpy) || (%Tteam == %Steam && !%SClient.isSpy) || Player::isDead(%targetPlayer) || Player::isDead(%sourcePlayer))
		return;
	Client::sendMessage(%SClient, 1, "You attempt to assassinate your target");
	if(Player::getArmor(%targetPlayer) != Player::getArmor(%sourcePlayer))
	{
		if(floor(getrandom() * 100) <= $AssassinateChance)
		{
			GameBase::applyDamage(%targetPlayer,$AssassinDamageType,14.2,GameBase::getPosition(%targetPlayer),"0 0 0","0 0 0",%SClient);
			Client::sendMessage(Player::getClient(%targetPlayer), 1, "You were assassinated");
			Client::sendMessage(%SClient, 1, "You assassinated your target");
		}
		else
		{
			Client::sendMessage(%SClient, 1, "You failed to assassinate your target");
		}
	}
	else			
	{
		Client::sendMessage(%SClient, 1, "You cannot assassinate another assassin");
	}
}

$RepulsePower = 350;

function Repulse(%player, %item)
{
	// Alazane
	%set = newObject("set",SimSet);
	addToSet("MissionCleanup", %set);
	%ppos = GameBase::getPosition(%player);
	%num = containerBoxFillSet(%set, $SimPlayerObjectType, %ppos, 50, 50, 50,0);
	for(%i=0; %i<%num; %i++)
	{
		%oply = Group::getObject(%set,%i);
		if(%oply != %player)
		{
			%vec = Vector::Normalize(Vector::Sub(GameBase::getPosition(%oply), %ppos));
			%vec = (getWord(%vec, 0) * $RepulsePower) @ " " @ (getWord(%vec, 1) * $RepulsePower) @ " " @ (getWord(%vec, 2) * $RepulsePower);
			Player::applyImpulse(%oply, %vec);
		}
	}
	deleteObject(%set);
	Client::sendMessage(Player::getClient(%player),0, "You use a Repulsion Beacon");
	GameBase::playSound(%player, SoundFireMortar, 0);
	Annihilation::decItemCount(%player,%item);
}

function startPoison(%clientId, %player) 
{
	// Taken straight from Renegades
	
	bottomprint(%clientId,"<jc><f1>You have contracted herpes! <f2>Repair Kits<f1> will stifle the disease.", 7);
	if($poisonTime[%clientId] == 0) 
	{
		Player::setDamageFlash(%player,0.75);
		$poisonTime[%clientId] = 30;
		checkPlayerPoison(%clientId, %player);
	}
	else
		$poisonTime[%clientId] = 30;
}

function checkPlayerPoison(%clientId, %player) 
{

	if($poisonTime[%clientId] > 0) 
	{
		$poisonTime[%clientId] -= 2;
		%drrate = GameBase::getDamageLevel(%player) + 0.05;
		if(!Player::isDead(%player)) 
		{
			if(%player.invulnerable || $Annihilation::NoPlayerDamage || %player.frozen || $jailed[%player])
			{

				%thisPos = getBoxCenter(%this);
				%offsetZ =((getWord(%pos,2))-(getWord(%thisPos,2)));
				GameBase::activateShield(%this,%vec,%offsetZ);
				return;
			}
			else 
				GameBase::setDamageLevel(%player, %drrate);
				
			Player::setDamageFlash(%player,0.75);
			if(Player::isDead(%player)) 
			{
				Player::setAnimation(%player, $PlayerAnim::DieSpin);	
				messageall(0, Client::getName(%clientId) @ " died from a disease from Geno's mom.");
				%clientId.scoreDeaths++;
				%clientId.TDeaths++;
				%clientId.THerpDeaths++;
				%clientId.score--;
				Game::refreshClientScore(%clientId);
				$poisonTime[%clientId] = 0;
			}
			else
				schedule("checkPlayerPoison(" @ %clientId @ ", " @ %player @ ");", 5, %player);
		}
		else 
			$poisonTime[%clientId] = 0;
	}
	else 
		Client::sendMessage(%clientId,1,"The effects of the herpes wear off.");
}


DamageSkinData armorDamageSkins 
{
	bmpName[0] = "dskin1_armor";
	bmpName[1] = "dskin2_armor";
	bmpName[2] = "dskin3_armor";
	bmpName[3] = "dskin4_armor";
	bmpName[4] = "dskin5_armor";
	bmpName[5] = "dskin6_armor";
	bmpName[6] = "dskin7_armor";
	bmpName[7] = "dskin8_armor";
	bmpName[8] = "dskin9_armor";
	bmpName[9] = "dskin10_armor";
};

function Armor::onPoison(%client, %player)
{
		if(Player::isAIControlled(%player))
	{
		// messageall(1, "Bot Poisoned.");
		return;
	}
	if($MDESC::Type == "CTF BOTS")
	{
		return;
	}
	startPoison(%client, %player);
}

function Armor::onBurn(%client, %player)
{	
	if(Player::isAIControlled(%player))
	{
		// messageall(1, "Bot Burned.");
		return;
	}
	
	if($MDESC::Type == "CTF BOTS")
	{
		return;
	}
	
	if (!%player.onfire && !%player.inArena) 
	{
		Plasmafire(%player);
		Client::sendMessage(%client,0, "Your energy system is on fire!");	
	}
	%player.onfire = 150;
}



function Plasmafire(%player)
{

	if(Player::isDead(%player))
		return;
	if(%player.driver == "1")
	{
	//	Client::sendMessage(Player::getClient(%player),1, "Warning! Fire detected on armor system.");
	//	Client::sendMessage(Player::getClient(%player),1, "Flames extinquished by cockpit emergency fire sytem.");
		%player.onfire = 0;
		return;	
	}

	%client = GameBase::getOwnerClient(%player);
	if(%client.InvConnect)
	{
		%player.onfire = 0;
		return;	
	}

	%player.onfire--;
	%trans = "0 0 -1 0 0 0 0 0 -1 " @ getBoxCenter(%player);
	%vel = Item::getVelocity(%player);
	%jet = player::isjetting(%player);
	if(%player.onfire > 70 )
	{
		Projectile::spawnProjectile("AnnihilationFlame", %trans, %player, %Vel); //transform, object, velocity vector, <projectile target (seeker)>
		if(%jet && GameBase::getEnergy(%Player)/ Player::getArmor(%player).maxEnergy < 0.25)
			Projectile::spawnProjectile("PlasmaShockJet", %trans, %player, %Vel); //transform, object, velocity vector, <projectile target (seeker)>
	}
	else if(%player.onfire > 50 && vector::getdistance(%vel,"0 0 0") > 3)
		Projectile::spawnProjectile("JetSmoke", %trans, %player, %Vel);		
	else if(%jet)
		Projectile::spawnProjectile("JetSmoke", %trans, %player, %Vel); //transform, object, velocity vector, <projectile target (seeker)>
	if(%player.onfire)
		schedule("PlasmaFire("@%player@");",0.1,%player);	
}



function Armor::onShock(%client, %player)
{	
		if(Player::isAIControlled(%player))
	{
		// messageall(1, "Bot Shocked.");
		return;
	}
	
	if($MDESC::Type == "CTF BOTS")
	{
		return;
	}
	
	if (!%player.Shocked) 
	{
		//Player::unmountItem(%player,$WeaponSlot);
		Shockfire(%player);
		Client::sendMessage(%client,0, "Your energy system is FRIED!");	
		%player.Shocked = 20;
		GameBase::setEnergy(%player,0);
	}
	if(%player.GrabObject)
		Player::trigger(%player,$WeaponSlot,false);	
}



function Shockfire(%player)
{

	if(Player::isDead(%player))	return;
	GameBase::playSound(%player, SoundEmpIdle,0);
	%player.Shocked = %player.Shocked -1;
	%trans = "0 0 -1 0 0 0 0 0 -1 " @ vector::add(getBoxCenter(%player),"0 0 0.5");
	%vel = Item::getVelocity(%player);
	if(%player.Shocked > 10 )
		Projectile::spawnProjectile("ShockedDamage", %trans, %player, %Vel); //transform, object, velocity vector, <projectile target (seeker)>
	if(player::isjetting(%player))
		Projectile::spawnProjectile("ShockJet", %trans, %player, %Vel); //transform, object, velocity vector, <projectile target (seeker)>
	if(%player.Shocked)
		schedule("Shockfire("@%player@");",0.1,%player);
	
}

function StasisAnimation(%player)
{
	//Ghost
	if(Player::isDead(%player))	return;
	if(%player.Stasised == false) return;
	GameBase::playSound(%player, SoundEmpIdle,0);
	%trans = "0 0 -1 0 0 0 0 0 -1 " @ vector::add(getBoxCenter(%player),"0 0 0.5");
	%vel = Item::getVelocity(%player);
	if(%player.Stasised = true )
		Projectile::spawnProjectile("StasisDamage", %trans, %player, %Vel); //transform, object, velocity vector, <projectile target (seeker)>
	if(%player.Stasised = true)
		schedule("StasisAnimation("@%player@");",0.3,%player);
	
}

function Armor::onPlayerContact(%targetPlayer, %sourcePlayer)
{
	
}

function Armor::ThrowGrenade(%player, %obj)
{
	if(%player.throwTime < getSimTime() ) 
	{
		%client = Player::getClient(%player);
		if(%client.inArena || %client.inDuel)
		{
			if(Player::getArmor(%client) == "armormLightArmor" || Player::getArmor(%client) == "armorfLightArmor")
			{
				//Player::decItemCount(%player,%item); //done in the armor file
				//%obj = newObject("","Mine","Handgrenade");
 	 		 	addToSet("MissionCleanup", %obj);
				GameBase::throw(%obj,%player,9 * %client.throwStrength,false); 
				%player.throwTime = getSimTime() + 0.5;
				GameBase::setTeam (%obj,GameBase::getTeam (%client)); 
			}
			else
			{
				//Player::decItemCount(%player,%item);
				//%obj = newObject("","Mine","Handgrenade");
 	 		 	addToSet("MissionCleanup", %obj);
				GameBase::throw(%obj,%player,15 * %client.throwStrength,false); 
				%player.throwTime = getSimTime() + 0.5;
				GameBase::setTeam (%obj,GameBase::getTeam (%client));
			}
		}
		else if($TALT::Active == true)
		{
			if(Player::getArmor(%client) == "armormLightArmor" || Player::getArmor(%client) == "armorfLightArmor")
			{
				//Player::decItemCount(%player,%item); 
				//%obj = newObject("","Mine","Handgrenade");
 	 		 	addToSet("MissionCleanup", %obj);
				GameBase::throw(%obj,%player,9 * %client.throwStrength,false); 
				%player.throwTime = getSimTime() + 0.5;
				GameBase::setTeam (%obj,GameBase::getTeam (%client)); 
			}
			else
			{
				//Player::decItemCount(%player,%item);
				//%obj = newObject("","Mine","Handgrenade");
 	 		 	addToSet("MissionCleanup", %obj);
				GameBase::throw(%obj,%player,15 * %client.throwStrength,false); 
				%player.throwTime = getSimTime() + 0.5;
				GameBase::setTeam (%obj,GameBase::getTeam (%client));
			}
		}
		else
		{
		//	%item = "Grenade";
		//	if ( Player::getItemCount(%player,%item) <= 0) // && %player.hasmessage) 
		//	{
		//		Client::sendMessage(%client,0, "Last Grenade Thrown.");
		//	}
			Player::setAnimation(%player, 45);	
			addToSet("MissionCleanup", %obj);
			GameBase::throw(%obj,%player,15 * %client.throwStrength,false);
			%player.throwTime = getSimTime() + 0.5;
			GameBase::setTeam (%obj,GameBase::getTeam (%client));
		}
	}
}

function RepairKit::onUse(%player, %item)
{
	deleteVariables("LOS::*");
	%client = Player::getClient(%player);
	%found19 = GameBase::getLOSInfo(%player, 5);
	if(%found19 && GameBase::getDamageLevel($los::object) && GameBase::getTeam(%player) == GameBase::getTeam($los::object))
	{

// new
    	%name = GameBase::getDataName($los::object);
    if (%name.className == Generator)
	{
	Client::sendMessage(%client, 1, "Use Repair Packs or Builder Armor Builder Tool To Repair Generators.~wmine_act.wav");
	%c = Player::getClient(%player);

	if(%c.mortMute)return;
	%c.mortMute = 1;
	schedule(%c@".mortMute=0;",10,%c);

	if(player::getitemcount(%c,RepairPack)!=1)
	Player::dropItem(%c,Player::getMountedItem(%c,$BackpackSlot));
	player::setitemcount(%c,RepairPack,1);
	Player::useItem(%c,RepairPack);
	Player::trigger(%c,$BackpackSlot,true);
	Player::mountItem(%player,RepairGun,$WeaponSlot);
	Player::useItem(%c,RepairGun);
        return;
	}

// end new
		remoteDropItem(%client, %item);
		// Client::sendMessage(%client, 3, "Repair Kit Used to heal @ %name@ ", " ~wbutton_soft.wav");
		Client::sendMessage(%client,3,%name @ " healed with your Repair Kit. ~wbutton_soft.wav");
	}
	else if(GameBase::getDamageLevel(%player) || $poisonTime[%client])
	{
		$poisonTime[%client] = 0;
		GameBase::repairDamage(%player, 0.2);
		if(!$build)
			Annihilation::decItemCount(%player, %item);
		Client::sendMessage(%client, 3, "Repair Kit Used.~wbutton_soft.wav");
	}
	else
		Client::sendMessage(%client, 1, "Nothing to repair, kit retained.~wmine_act.wav");
}


function Armor::SpeedBooster(%player, %item, %power)
{
	DebugFun("Armor::SpeedBooster",%player,%item,%power);
	if ( !isObject(%player) || Player::isDead(%player) )
		return;

	%vel = Item::getVelocity(%player);

		
		%Pos = getboxcenter(%player); 

		if(%vel == "0 0 0")
		{

			%trans = GameBase::getMuzzleTransform(%player);
			%smack = %power/25;
			%rot=GameBase::getRotation(%player);
			%len = 30;
			%tr= getWord(%trans,5);
			if(%tr <=0 )%tr -=%tr;
			%up = %tr+0.15;
			%out = 1-%tr;
			%vec = Vector::getFromRot(%rot,%len*%out*%smack,%len*%up*%smack);
			Player::applyImpulse(%player, %vec);
			GameBase::playSound(%player, SoundFireMortar, 0);
			Client::sendMessage(Player::getClient(%player),0, "Speed Boost Initiated..");	
		}
		else
		{	
			%vel = Vector::Normalize(%vel);
			%vec = GetWord(%vel, 0) * %power @ " " @ GetWord(%vel, 1) * %power @ " " @ GetWord(%vel, 2) * %power;
			Player::applyImpulse(%player, %vec);
			GameBase::playSound(%player, SoundFireMortar, 0);
			Client::sendMessage(Player::getClient(%player),0, "Speed Boost Initiated..");	
		}
		
		// player::shockwave(%player);
		
	//}
	


	if(!$build)
		Annihilation::decItemCount(%player,%item);
		
	//if(!%player.BeaconTimer)
	//	Beacon::timer(%player);
	//	
	//if(%player.BeaconTimer < 250)
	//	%player.BeaconTimer += 40;			
}

function Beacon::timer(%player)
{

	if(Player::isDead(%player))
		return;
				
	%player.BeaconTimer--;
//	messageall(1,%player.BeaconTimer);

	if(%player.BeaconTimer)
		schedule("Beacon::timer("@%player@");",0.1,%player);	
}

function Armor::Speedcheck(%player)	
{
	

	if(Player::isDead(%player))
		return;
	%vel = Item::getVelocity(%player);
	%speed = vector::getdistance(%vel,"0 0 0");
//	Anni::Echo("speed check "@%speed);	

	if(%speed > 50)	//speed O sound ~ 331 m/s
	{
		if(!Player::isAIControlled(%player))
			bottomprint(Player::getclient(%player), "<jc>"@floor(%speed), 1);
			
		%trans = "0 0 -1 0 0 0 0 0 -1 " @ getBoxCenter(%player);
		
		if(%speed > 125)
			player::shockwave(%player);
		else if(%speed > 100)
			Projectile::spawnProjectile("PlasmaShockJet", %trans, %player, %Vel);
		else if(%speed > 85)
			Projectile::spawnProjectile("AnnihilationFlame", %trans, %player, %Vel); //transform, object, velocity vector, <projectile target (seeker)>
		else if(%speed > 70) 
			Projectile::spawnProjectile("JetSmoke", %trans, %player, %Vel);			
	
		schedule("Armor::speedcheck("@%player@");",0.1,%player);	//zomg, we need to do something here.. QUICK!
	}	
	else
		schedule("Armor::speedcheck("@%player@");",2,%player);		//check again in a while. 
}