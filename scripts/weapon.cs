//----------------------------------------------------------------------------
// Tools, Weapons & ammo
//----------------------------------------------------------------------------

ItemData Weapon
{
	description = "Weapon";
	showInventory = false;
};

function Weapon::onDrop(%player,%item)
{
	%state = Player::getItemState(%player,$WeaponSlot);
	if(%state != "Fire" && %state != "Reload")
		Item::onDrop(%player,%item);
}	

function Weapon::onUse(%player,%item)
{
	
	if(%player.Station=="")
	{
		//if(%item == ParticleBeamWeapon && Player::getMountedItem(%player,$BackpackSlot) != EnergyPack)
		//{
		//	Client::sendMessage(Player::getClient(%player), 1, "You must have the Energy Pack to use the Particle Beam.");
		//	schedule("remoteNextWeapon("@%player@");", 0.3, %player);
		//	return False;
		//}
		%ammo = %item.imageType.ammoType;
		if(%ammo == "") 
		{
			// Energy weapons dont have ammo types
			Player::mountItem(%player,%item,$WeaponSlot);
		}
		else 
		{
			if(Player::getItemCount(%player,%ammo) > 0) 
				Player::mountItem(%player,%item,$WeaponSlot);
			else 
				Client::sendMessage(Player::getClient(%player),0,%item.description@" has no ammo.");
			
		}
	}
}
//----------------------------------------------------------------------------

ItemData Tool
{
	description = "Tool";
	showInventory = false;
};

function Tool::onUse(%player,%item)
{
	Player::mountItem(%player,%item,$ToolSlot);
}

function Weapon::CompleteMount(%player,%weapon)
{
	if(!Player::isDead(%player)) 
	{
		%client = Player::getclient(%player);
		%mounted = Player::getMountedItem(%client,$WeaponSlot);
		
//	Anni::Echo("Weapon::CompleteMount P"@%player@", W"@%weapon@", M"@%mounted);
		
		if(%mounted  == %weapon)	
			eval(%weapon @ "::MountExtras("@%player@","@%weapon@");");	
		else
		{
			%check = false;
			for(%i = 4; %i<8; %i++)
			{
				%w = Player::getMountedItem(%player,%i);
				if(%w == %weapon)
					%check = true;
			}			
			if(!%check)
				schedule("Weapon::CompleteMount("@%player@","@%weapon@");",0.25, %player);
		}
	}
}

function Weapon::Mode(%player,%rotate)
{
	%client = Player::getClient(%player);
	%weapon = Player::getMountedItem(%player,$WeaponSlot);
	
	if($debug)	
		Anni::Echo(%client@": change weap options "@%weapon);
		
	if(%weapon == grenadelauncher)
	{
		%mode = %client.MiniMode += %rotate; 
		
		if(%mode >2) %mode = 0;
		else if (%mode <0) %mode = 2;
		%client.MiniMode = %mode;

		Client::sendMessage(%client, 0, "~wturreton1.wav");		
		if(%client.MiniMode == 1)
			bottomprint(%client, "<jc>Mini Bomber: <f2>SINGLE <f2>Grenade Projectile.\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.", 2);
		else if(%client.MiniMode == 0)
			bottomprint(%client, "<jc>Mini Bomber: <f2>TRIPLE <f2>Grenade Projectile.\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.", 2);
		//else if(%client.MiniMode == 2) //removed mine nades 
		//	bottomprint(%client, "<jc>Mini Bomber: <f2>MINE dropper<f2> Projectile.\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.", 2);
		//else if(%client.MiniMode == 3)
		//	bottomprint(%client, "<jc>Mini Bomber: <f2>Starburst MINE <f2>Projectile.\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.", 2);
		else if(%client.MiniMode == 2)
			bottomprint(%client, "<jc>Mini Bomber: <f2>EMP <f2>Grenade Projectile.\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.", 2);
		return;
	}
	else if(%weapon == GravityGun)
	{
		if(!isObject(%player.moveObj)) return;

		if(getObjectType(%player.moveObj) != "Player")
		{
			if(!%player.gRotate)
			{
				%player.gRotate = true;
				bottomprint(%client, "<jc><f2>Object rotation module enabled.");
			}
			else
			{
				%player.gRotate = false;
				bottomprint(%client, "<jc><f2>Object rotation module disabled.");
			}
		}
		else
		{
			if(!%player.gMove)
			{
				%player.gMove = true;
				bottomprint(%client, "<jc><f2>Gravity module set to momentum tractor.");
			}
			else
			{
				%player.gMove = false;
				bottomprint(%client, "<jc><f2>Gravity module set to magnetic tractor.");
			}
		}
	}
	else if(%weapon == Vulcan)
	{
		%mode = %client.Vulcan += %rotate;

		if(%mode > 1) %mode = 0;
		else if (%mode < 0) %mode = 1;
		%client.Vulcan = %mode;	
		
		Client::sendMessage(%client, 0, "~wturreton1.wav");	
		if (!%client.Vulcan || %client.Vulcan == 0)
			bottomprint(%client, "<jc>Vulcan:<f2> Standard Chaingun Rounds.\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.", 2);
		if (%client.Vulcan == 1)
			bottomprint(%client, "<jc>Vulcan:<f2> Fiery Vulcan Rounds.\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.", 2);
		return;
	}	
	else if(%weapon == ParticleBeamWeapon && %player.pbeamcharge <= 0)
	{
		%mode = %client.pbeam += %rotate;

		if(%mode > 1) %mode = 0;
		if (%mode < 0) %mode = 1;
		%client.pbeam = %mode;
		if(%mode == 1)
		{
			Client::sendMessage(%client, 0, "~wturreton1.wav");
			bottomprint(%client,"<jc>Particle Beam Weapon: <f2>Enhanced Positron Beam. Hold fire to charge.\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.",10);
		}
		else 
		{
			Client::sendMessage(%client, 0, "~wturreton1.wav");			
			bottomprint(%client, "<jc>Particle Beam Weapon: <f2>Standard Neutron Beam.\nPress <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change.",10);
			%client.pbeam = 0;
		}

		return;
	}
	else if(%weapon == Fixit)
	{
		%mode = %client.repack += %rotate;

		if(%mode > 1) %mode = 0;
		if (%mode < 0) %mode = 1;
		%client.repack = %mode;
		if(%mode == 1)
		{
			Bottomprint(%client,"<jc>Builder's Tool: <f2>Repack Mode.\n<f3>Pick up your deployables.\n<f2>Press <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change modes.",10);
			Player::unmountItem(%client,0);
			Player::decItemCount(%client,Fixit);
			Player::incItemCount(%client,Fixit2);
			Player::mountItem(%client,Fixit2,0);
			Player::useItem(%client,Fixit2);
			Client::sendMessage(%client, 0, "~wturreton1.wav");
		}

		return;
	}
	else if(%weapon == Fixit2)
	{
		%mode = %client.repack += %rotate;

		if(%mode > 1) %mode = 0;
		if (%mode < 0) %mode = 1;
		%client.repack = %mode;
		if(%mode == 0)
		{
			Bottomprint(Player::getclient(%player), "<jc>Builder's Tool: <f2>Repair Mode.\n<f3>Repairs players and objects at a faster rate than a Repair Pack.\n<f2>Press <f1>Use Laser<f2> or <f1>Use Blaster<f2> (1 and 6 keys) to change modes.");
			Player::unmountItem(%client,0);
			Player::decItemCount(%client,Fixit2);
			Player::incItemCount(%client,Fixit);
			Player::mountItem(%client,Fixit,0);
			Player::useItem(%client,Fixit);
			Client::sendMessage(%client, 0, "~wturreton1.wav");
		}

		return;
	}
	else if(%weapon == Slapper)
	{
		Slapper::Mode(%player,%rotate);
		return;
	}	
	else if(%weapon == grabbler && Player::isTriggered(%player,$WeaponSlot))
	{
		%object = %player.GrabObject;
		if (%object)
		{
			Player::trigger(%player,$WeaponSlot,false);
		
			schedule("grabbler::fire("@%player@","@%object@");",0.1,%player);
		}
		return;
	}	
	else if($build && (%weapon == CuttingLaser || %weapon == TargetingLaser))
	{
		%mode = %client.CuttingLaser += %rotate;

		if(%mode > 1) %mode = 0;
		else if (%mode < 0) %mode = 1;
		%client.CuttingLaser = %mode;	
			
		Client::sendMessage(%client, 0, "~wturreton1.wav");
		if(%client.CuttingLaser == 0)
		{		
			Player::mountItem(%player,TargetingLaser,0);
			bottomprint(%client, "<jc>Target Laser: <f2>Standard Beam.", 7);
		}
		else if(%client.CuttingLaser == 1)
		{	
			Player::mountItem(%player,CuttingLaser,0);	
			bottomprint(%client,"<jc>Target Laser: <f2>Cutting Laser, <f2>Watch your eyes.", 7);
		}
		return;
	}					
}

