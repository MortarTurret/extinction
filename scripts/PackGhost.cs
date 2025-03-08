
$InvList[GhostPack] = 1;
$MobileInvList[GhostPack] = 1;
$RemoteInvList[GhostPack] = 0;
additem(GhostPack);
//-----------------------------------

$ItemMax[larmor, GhostPack] = 1;
$ItemMax[lfemale, GhostPack] = 1;
$ItemMax[Marmor, GhostPack] = 0;
$ItemMax[mfemale, GhostPack] = 0;
$ItemMax[Harmor, GhostPack] = 0;

ItemData GhostArmor
{
	heading = "aArmor";
	description = "Ghost"; // Ghosting -death666 3.16.17
	className = "Armor";
	price = 175;
	showInventory = false; //armor won't show in any inv, including when you're wearing it.
};

ItemImageData GhostPackImage
{
	shapeFile = "AmmoPack"; //grenadeL
	mountPoint = 2;
//	mountOffset = { -0.375, 0, 0 };
	weaponType = 0;		//0 single, 1 rotating, 2 sustained, 3disc
	//projectileType = PrettySplit;
	mountRotation = {1.57,0, 3.14 };
	mass = 2.5;
	firstPerson = false;
	lightType = 3; // Pulsing 2 pulsing, 3 fire, 1 continuous
	lightRadius = 5;
//	lightTime = 0.1;
	lightColor = { 0.3, 0.4, 0.5};
};

ItemData GhostPack
{
	description = "Ghost Pack";
	shapeFile = "grenadeL"; //grenadeL
	className = "Backpack";
	heading = $InvHead[ihBac];
	imageType = GhostPackImage;
	shadowDetailMask = 4;
	mass = 2.5;
	elasticity = 0.2;
	price = 500;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

ItemImageData GhostPack2Image 
{
	shapeFile = "mine"; 
	mountPoint = 2;
	mountOffset = { 0 , -0.3, 0.2 };
	mountRotation = {-1.57,0, 0 };//{1.57,0, 3.14 }
	weaponType = 0;
	lightType = 1; // Pulsing, 2 pulsing, 3 fire, 1 continuous
	lightRadius = 5;
//	lightTime = 0.1;
	lightColor = { 0.3, 0.4, 0.5};
};

ItemData GhostPack2
{
	description = "gpack";
	shapeFile = "discb"; //grenadeL
	className = "Backpack"; 
	heading = "cBackpacks";
	imageType = GhostPack2Image;
	shadowDetailMask = 4;
	mass = 2.5;
	elasticity = 0.2;
	price = 500;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

ItemImageData GhostPack3Image
{
	shapeFile = "GRENADE";	//SHOCKWAVE_LARGE";	//grenade"; //grenadeL,shield,discb
	mountPoint = 2;
	mountOffset = { 0 , 0.05, 0.15 };
	mountRotation = {1.57,0, 0 };
	weaponType = 0;
	lightType = 1; // Pulsing, 2 pulsing, 3 fire, 1 continuous
	lightRadius = 5;
//	lightTime = 0.1;
	lightColor = { 0.3, 0.4, 0.5};
};

ItemData GhostPack3
{
	description = "Ghost Pack";
	shapeFile = "grenade";
	className = "Backpack";
	heading = "cBackpacks";
	shadowDetailMask = 4;
	imageType = GhostPack3Image;
	price = 1500;
	hudIcon = "energypack";
	showWeaponBar = true;
	hiliteOnActive = true;
};


function GhostPack::onMount(%player,%item)
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));
	//	%player.shieldStrength = -0.052;

	Player::mountItem(%player, GhostPack2, 7);
	gamebase::setautorepairrate(%player, 0.001);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	bottomprint(Player::getClient(%player), "<jc>Ghost Pack: <f2>Become lighter and pass through vertical walls.\n<jc><f1>Warning:<f2> Armor deteriorates and suffers greater kickback while ghosting.", 10);
}


function GhostPack::onUnmount(%player,%item)
{	
	if (Player::getItemCount(%player, GhostArmor) == 1) noghost(%player);
		Player::unmountItem(%player, 7);
	gamebase::setautorepairrate(%player, 0.0);
}

$DropFlagChance = 15;

function GhostPack::onUse(%player,%item)
{
	//%player.GhostPackTime = getSimTime();	
	%client = GameBase::getOwnerClient(%player);
	Player::trigger(%player, 4, false);
	if(Player::getMountedItem(%player,$BackpackSlot) != %item)
	{
		Player::mountItem(%player,%item,$BackpackSlot);
		gamebase::setautorepairrate(%player, 0.001);
		bottomprint(Player::getClient(%player), "<jc>Ghost Pack: <f2>Become lighter and pass through vertical walls.\n<jc><f1>Warning: <f2>Armor deteriorates and suffers greater kickback while ghosting.", 10);
	}
	else
	{
		if (Player::getMountedItem(%player,4) == DeathRayBeam)
		{
			Player::unmountItem(%player,4);	
			
		}
		if (%player.ghoster != true)
		{

// new
//      //Get the current map's mission type from the .dsc file
//      %missionFile = "missions\\" $+ $missionName $+ ".dsc";
//      if(File::FindFirst(%missionFile) == "")
//      {
//         %missionName = $firstMission;
//         %missionFile = "missions\\" $+ $missionName $+ ".dsc";
//         if(File::FindFirst(%missionFile) == "")
//         {
//            echo("invalid nextMission and firstMission...");
//            echo("aborting mission load.");
//            return;
//         }
//      }
//      exec(%missionFile);
//		
//      for(%i = 0; %i < $MLIST::TypeCount; %i++)
//      {
//         if($MLIST::Type[%i] == $MDESC::Type)
//         {
//            break;
//         }
//    }
//
//
//         	if(($MDESC::Type == "CTF Indoor" )) 
// 	{
//		%client = Player::getClient(%player);
//		Client::sendMessage(%client,0,"Unable to Ghost on indoor maps!");
//		return false;
// 	}
// end new

			%player.ghoster = true;
			Player::mountItem(%player, GhostPack3, 6);
			Player::trigger(%player,$BackpackSlot,true);
			Casper(%player);
			Client::sendMessage(Player::getClient(%player), 1, "Ghost Pack on. Collapsing shadows.");
			%c = Player::getClient(%player);
			gamebase::setautorepairrate(%player, 0.0);
//			%player.repairRate = -0.04;
			%armor = Player::getArmor(%client);
			%buyarmor = $ArmorType[Client::getGender(%client), GhostArmor];
			%energy = GameBase::getEnergy(%player);
			Player::setArmor(%client,%buyarmor);
			GameBase::setEnergy(%player,%energy);
			Player::setItemCount(%client, $ArmorName[%armor], 0);  
			Player::setItemCount(%client, GhostArmor, 1);
		}
		else
		{
			%pos = GameBase::getPosition(%player);
			if(GameBase::testPosition(%player,%pos)) 
			{
				gamebase::setautorepairrate(%player, 0.001);
				Player::unmountItem(%player, 6);
				%client = GameBase::getOwnerClient(%player);
				%player.Ghoster = false;
				Player::trigger(%player,$BackpackSlot,false);
				%c = Player::getClient(%player);
				gamebase::setautorepairrate(%player, 0.01);
				%player.repairRate = 0.00;
				Client::sendMessage(Player::getClient(%player), 1, "Ghost Pack off. Rephasing entities.");
				%armor = Player::getArmor(%client);
				%buyarmor = $ArmorType[Client::getGender(%client), iarmorNecro];
				%energy = GameBase::getEnergy(%player);
				Player::setArmor(%client,%buyarmor);
				GameBase::setEnergy(%player,%energy);
				Player::setItemCount(%client, $ArmorName[%armor], 0);  
				Player::setItemCount(%client, iarmorNecro, 1); 	
			}
			else
				Client::sendMessage(%client,0,"Unable to disable ghost pack while in a wall."); //updated message for clarity -death666		
			
		}			
	}
}

function Casper (%player)
{

	if(Player::getMountedItem(%player,$BackpackSlot) != GhostPack)
	{
				gamebase::setautorepairrate(%player, 0.001);
				Player::unmountItem(%player, 6);
				%client = GameBase::getOwnerClient(%player);
				%player.Ghoster = false;
				Player::trigger(%player,$BackpackSlot,false);
				%c = Player::getClient(%player);
				gamebase::setautorepairrate(%player, 0.01);
				%player.repairRate = 0.00;
				Client::sendMessage(Player::getClient(%player), 1, "Ghost Pack off. Rephasing entities.");
				%armor = Player::getArmor(%client);
				%buyarmor = $ArmorType[Client::getGender(%client), iarmorNecro];
				%energy = GameBase::getEnergy(%player);
				Player::setArmor(%client,%buyarmor);
				GameBase::setEnergy(%player,%energy);
				Player::setItemCount(%client, $ArmorName[%armor], 0);  
				Player::setItemCount(%client, iarmorNecro, 1);
				%name = Client::getName(%c);
				// Messageall(0,%name@" died."); 
				Client::sendMessage(Player::getClient(%player), 1, "You became a dead shadow.");
				Admin::BlowUp(%client);
				return;
	}

	if(%player.ghoster && !Player::isDead(%player) && Player::getClient(%player) != -1)
	{
		GameBase::playSound(%player, SoundActivateMotionSensor, 0);
		%svec = Vector::getFromRot(GameBase::getRotation(Player::getClient(%player)),10,0);	//10,0
		GameBase::activateShield(%player,%svec, 0.5);
		Player::setDamageFlash(%player,0.2);
		%c = Player::getClient(%player);

		%name = Client::getName(%c);
		%dlevel = GameBase::getDamageLevel(%player) + 0.03; 
		if(!$Annihilation::NoPlayerDamage)
		{
			%data = GameBase::getDataName(%player);
			if(%dlevel > %data.maxDamage)
			{
				Admin::BlowUp(%c);
				// Messageall(0,%name@" became a dead shadow."); 
				%player.Ghoster = false;
			}
			
			else
				GameBase::setDamageLevel(%player,%dlevel);
		}
		schedule("Casper(" @ %player @  ");", 0.75,%player);
	}
}
