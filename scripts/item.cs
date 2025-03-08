//----------------------------------------------------------------------------
$InvHead[ihVeh] = "bVehicle";
$InvHead[ihArm] = "aArmor";
$InvHead[ihWea] = "cWeapons";
$InvHead[ihtool] = "dTools";
$InvHead[ihBac] = "eBackpacks";
$InvHead[ihSpl] = "gSpells";
$InvHead[ihMis] = "jMiscellany";
$InvHead[ihDSe] = "lSensors";
$InvHead[ihDOb] = "nObjects";
$InvHead[ihBar] = "pBarriers";
$InvHead[ihTur] = "rTurrets";
$InvHead[ihPwr] = "tPower Systems";
$InvHead[ihRem] = "vRemote Bases";
$InvHead[ihDro] = "xDrones";
$InvHead[ihAmm] = "zAmmunition";


//--------------------------------------
$InvList[RepairKit] = 1;
$MobileInvList[RepairKit] = 1;
$RemoteInvList[RepairKit] = 1;
AddItem(RepairKit);

$AutoUse[RepairKit] = false;

ItemData RepairKit 
{
	description = "Repair Kit";
	shapeFile = "armorKit";
	heading = $InvHead[ihMis];
	shadowDetailMask = 4;
	price = 35;
};

//function RepairKit::onUse(%player,%item) 
//{
//		
//	// handling this in function Armor::onRepairKit(%player)	
//	if(!$build)
//		Player::decItemCount(%player,%item);
//	// GameBase::repairDamage(%player,0.2);
//	
//	%c = Player::getClient(%player);
//	 // Alliance armor support
//	%armor = Player::getArmor(%player);
//	eval(%armor @ "::onRepairKit(" @ %player @ ");");
//}

function RepairKit::onCollision(%this,%object) 
{
	if(getObjectType(%object) == "Player")
	{
		Item::onCollision(%this,%object);
	}
	else
	{
		if(GameBase::getDamageLevel(%object) != 0)
		{
			%name = GameBase::getDataName(%object);	
			%class = %name.className;
			%data = %name.description;		
			if(%class == Sensor || %class == Station)
			{
				// GameBase::setDamageLevel(%object, 0);
				GameBase::repairDamage(%object,1.0);
				GameBase::playSound(%object, ForceFieldOpen,0);
				deleteobject(%this);
				return;
			}
			if(%class != Sensor || %class != Station) // %name != PulseSensor
				{		
				GameBase::repairDamage(%object,0.70);
				GameBase::playSound(%object, ForceFieldOpen,0);
				deleteobject(%this);
				}
		}
	}
	
}

ItemData RepairPatch 
{
	description = "Repair Patch";
	className = "Repair";
	shapeFile = "armorPatch";
	heading = $InvHead[ihMis];
	shadowDetailMask = 4;
	price = 2;
};

function RepairPatch::onCollision(%this,%object) 
{	
	if($debug) 
		event::collision(%this,%object);

	if(getObjectType(%object) == "Player") 
	{
		if(GameBase::getDamageLevel(%object)) 
		{
			GameBase::repairDamage(%object,0.125);
			%c = Player::getClient(%object);
			$poisonTime[%c] = 0;
			%item = Item::getItemData(%this);
			Item::playPickupSound(%this);
			Item::respawn(%this);
		}
	}
}

function RepairPatch::onUse(%player,%item) 
{
	if(!$build)Player::decItemCount(%player,%item);
	GameBase::repairDamage(%player,0.1);
}


//-----------------------------------------------------------------------------
$ItemPopTime = 15; // 30

$ToolSlot=0;
$WeaponSlot=0;
$BackpackSlot=1;
$FlagSlot=2;
$DefaultSlot=3;

$AutoUse[Blaster] = True;
$AutoUse[Chaingun] = True;
$AutoUse[PlasmaGun] = True;
$AutoUse[Mortar] = True;
$AutoUse[GrenadeLauncher] = True;
$AutoUse[LaserRifle] = True;
$AutoUse[EnergyRifle] = True;
$AutoUse[TargetingLaser] = False;
$AutoUse[ChargeGun] = True;

$Use[Blaster] = True;

//$ArmorType[Male, LightArmor] = larmor;
//$ArmorType[Male, MediumArmor] = marmor;
//$ArmorType[Male, HeavyArmor] = harmor;
//$ArmorType[Female, LightArmor] = lfemale;
//$ArmorType[Female, MediumArmor] = mfemale;
//$ArmorType[Female, HeavyArmor] = harmor;

//$ArmorName[larmor] = LightArmor;
//$ArmorName[marmor] = MediumArmor;
//$ArmorName[harmor] = HeavyArmor;
//$ArmorName[lfemale] = LightArmor;
//$ArmorName[mfemale] = MediumArmor;

// Amount to remove when selling or dropping ammo
$SellAmmo[BulletAmmo] = 25;

// Global object damage skins (staticShapes Turrets Stations Sensors)
DamageSkinData objectDamageSkins
{
	bmpName[0] = "dobj1_object";
	bmpName[1] = "dobj2_object";
	bmpName[2] = "dobj3_object";
	bmpName[3] = "dobj4_object";
	bmpName[4] = "dobj5_object";
	bmpName[5] = "dobj6_object";
	bmpName[6] = "dobj7_object";
	bmpName[7] = "dobj8_object";
	bmpName[8] = "dobj9_object";
	bmpName[9] = "dobj10_object";
};

$WeaponAmmo[LaserRifle] = "";
$WeaponAmmo[EnergyRifle] = "";


//----------------------------------------------------------------------------
// Server side methods
// The client side inventory dialogs call buyItem, sellItem,
// useItem and dropItem through remoteEvals.

function teamEnergyBuySell(%player,%cost)
{
	%client = Player::getClient(%player);
	%team = Client::getTeam(%client);
	// IF - Cost positive selling	 IF - Cost Negitive buying 
	%station = %player.Station;
	%stationName = GameBase::getDataName(%station);
	if(%stationName == DeployableInvStation || %stationName == DeployableAmmoStation)
	{
		%station.Energy += %cost; //Remote StationEnergy
		if(%station.Energy < 1)
			%station.Energy = 0;
	}
	else if($TeamEnergy[%team] != "Infinite")
	{
		$TeamEnergy[%team] += %cost; //Total TeamEnergy
 		%client.teamEnergy += %cost; //Personal TeamEnergy
	}
}

function isPlayerBusy(%client)
{
	// Can't buy things if busy shooting.
	%state = Player::getItemState(%client,$WeaponSlot);
	return %state == "Fire" || %state == "Reload";
}

function remoteBuyFavorites(%client,%favItem0,%favItem1,%favItem2,%favItem3,%favItem4,%favItem5,%favItem6,%favItem7,%favItem8,%favItem9,%favItem10,%favItem11,%favItem12,%favItem13,%favItem14,%favItem15,%favItem16,%favItem17,%favItem18,%favItem19)
{
	if ( CheckEval("remoteBuyFavorites", %client,%favItem0,%favItem1,%favItem2,%favItem3,%favItem4,%favItem5,%favItem6,%favItem7,%favItem8,%favItem9,%favItem10,%favItem11,%favItem12,%favItem13,%favItem14,%favItem15,%favItem16,%favItem17,%favItem18,%favItem19) )
		return;

	if($debug)
		Anni::Echo("remote buy favorites");
	// only can buy fav every 1/2 second
	%time = getIntegerTime(true) >> 4; // int half seconds
	if(%time <= %client.lastBuyFavTime)
		return;
			
	%player = Client::getOwnedObject(%client);

	if(%player == -1 || %client.observerMode != "")
		return;
	// cant buy favs if busy shooting or in ghost armor (ghost pack on)
	if(isPlayerBusy(%client) || Player::getItemCount(%player, GhostArmor) || %player.vehicle)
		return;
		
	if((Client::getGuiMode(%client) != $GuiModeInventory && !%Client.InvConnect) && !$build)
		return;

	GameBase::setAutoRepairRate(%player, 0);

	%weapon = Player::getMountedItem(%client,$WeaponSlot);
	Player::unmountItem(%player,$WeaponSlot);
	
	%armor = $ArmorName[Player::getArmor(%client)];

	%client.lastBuyFavTime = %time;
		%player = Client::getOwnedObject(%client);
	%station = (Client::getOwnedObject(%client)).Station;
	if(%station != "" || $build || $Annihilation::QuickInv || %Client.InvConnect) 
	{
		%stationName = GameBase::getDataName(%station);
		if(%stationName == DeployableInvStation || %stationName == DeployableAmmoStation) 
			%energy = %station.Energy;
		else 
			%energy = $TeamEnergy[Client::getTeam(%client)];
		if(%energy == "Infinite" || %energy > 0 || $build || $Annihilation::QuickInv || %Client.InvConnect)
		{
			%error = 0;
			%bought = 0;
			%max = getNumItems();
			for(%i = 0; %i < %max; %i = %i + 1) 
			{ 
				%item = getItemData(%i);
				//%item = $Itemlist[%i];
				if($ServerCheats || Client::isItemShoppingOn(%client,%item)|| $TestCheats || $build || $Annihilation::QuickInv || %Client.InvConnect) 
				{
					%count = Player::getItemCount(%client,%item);
					if(%count && %item != flag) 
					{
						if(%item.className != Armor) 
							teamEnergyBuySell(Client::getOwnedObject(%client),(%item.price * %count));
						Player::setItemCount(%client, %item, 0);
					}
				}
			}
			for(%i = 0; %i < 20; %i++) 
			{ 
				if(%favItem[%i] != "") 
				{
					%item = getItemData(%favItem[%i]);
					if((Client::isItemShoppingOn(%client,%item) || $build || $Annihilation::QuickInv || %Client.InvConnect) && ($ItemMax[Player::getArmor(%client), %item] > Player::getItemCount(%client,%item) || %item.className == Armor))
					{
						if(%item.className == Armor && Player::getArmor(%client) != %item)
							%client.isBuyingFavs = true;
							
						if(!buyItem(%client,%item))
							%error = 1;
						else
							%bought++;
					}
				}
			}
			if(%bought) 
			{
				if(%error) 
					Client::sendMessage(%client,0,"~wC_BuySell.wav");
				else 
					Client::SendMessage(%client,0,"~wbuysellsound.wav");
			}
			updateBuyingList(%client);
			
		}
	}
}


function replenishTeamEnergy(%team)
{
	$TeamEnergy[%team] += $incTeamEnergy;
	schedule("replenishTeamEnergy(" @ %team @ ");", $secTeamEnergy);
}


function checkResources(%player,%item,%delta,%noMessage)
{
	if($debug)
		Anni::Echo("checkResources( "@%player@", "@%item@", "@%delta@", "@%noMessage);
		
	%client = Player::getClient(%player);
	
	if(%player == -1 || %client.observerMode != "")
		return;	
	%team = Client::getTeam(%client);
	%extraAmmo = 0;
	if(Player::getMountedItem(%client,$BackpackSlot) == ammopack && $AmmoPackMax[%item] != "") 
	{
		%extraAmmo = $AmmoPackMax[%item];
		if(%delta == $ItemMax[Player::getArmor(%client), %item]) 
			%delta = %delta + %extraAmmo;
	}

	if($TestCheats == 0) 
	{
		%energy = $TeamEnergy[%team];
		%station = %player.Station;
		%sName = GameBase::getDataName(%station);
		if(%sName == DeployableInvStation || %sName == DeployableAmmoStation)
		{
			%energy = %station.Energy;
		}
		if(%energy != "Infinite") 
		{
			if(%item.price * %delta > %energy)	
				%delta = %energy / %item.price;
			if(%delta < 1 ) 
			{
				if(%noMessage == "")
					Client::sendMessage(%client,0,"Couldn't buy " @ %item.description @ " - "@ %energy @ " Energy points left");
				return 0;
			}
		}
	}
	if(%item.className == Weapon) 
	{
		%armor = Player::getArmor(%client);
		%wcount = Player::getItemClassCount(%client,"Weapon");
		if(Player::getItemClassCount(%client,"Weapon") >= $MaxWeapons[%armor]) 
		{
			Client::sendMessage(%client,0,"To many weapons for " @ $ArmorName[%armor].description @ " to carry");
			return 0;
		}
	}
	else if(%item == RepairPatch) 
	{
		%pDamage = GameBase::getDamageLevel(%player);
		if(GameBase::getDamageLevel(%player) > 0) 
			return 1;
		return 0;
	}
	else if($TeamItemMax[%item] != "" && !$TestCheats  && !$build) 
	{
		if($TeamItemMax[%item] <= $TeamItemCount[%team, %item]) 
		{
			Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
			return 0;
		}
	}
	if(%item.className != Armor && %item.className != Vehicle) 
	{
		%count = Player::getItemCount(%client,%item);
		%max = $ItemMax[(Player::getArmor(%client)), %item] + %extraAmmo ;
		if(%delta + %count >= %max) 
			%delta = %max - %count;
	}
	return %delta;
}

function buyItem(%client,%item)
{
	if($debug)
		Anni::Echo("buyItem("@%client@","@%item@")");

	if(%item == GhostArmor)
	{	
		%name = Client::getName(%client);
		%ip = Client::getTransportAddress(%client);
		$Cheater = %name @ " : Client ID : "@%client@" IP Address : "@ %ip @" Using GhostArmor Cheat.."; 
		export("Cheater","config\\Cheater.log",true);		
		Anni::Echo($Cheater);
		return;
	}

	// fixing buying cham while already holding a flag -death666 		Redundant with new cham pack flag update -Ghost
	//if(Player::getMountedItem(%client,$BackpackSlot) == ChameleonPack && Player::getMountedItem(%client, 2) == "flag")
	//{
	//	Player::dropItem(%client,Player::getMountedItem(%client, 2));
	//	Client::sendMessage(Player::getClient(%client),1,"CAN NOT carry the flag with Chameleon Pack. ~wError_Message.wav");
	//	return;
	//}			

	%player = Client::getOwnedObject(%client);
	GameBase::setAutoRepairRate(%player, 0); 
	if(%player.repackEnergy != "")
	{
	%player.repackDamage = "";
	%player.repackEnergy = "";
	}

	if(%player == -1 || %client.observerMode != "")
		return;		
	
	%armor = Player::getArmor(%client);
	updateBuyingList(%client);

	if(%armor == "harmor1" || %armor == "harmor2" || %armor == "harmor3" || $ArmorName[%armor] == GhostArmor)
		return;

	%bypass = %client.buyBypass;

	if(($ServerCheats || Client::isItemShoppingOn(%client,%item) || $TestCheats || %Client.InvConnect || $Annihilation::QuickInv || $build) && ($ItemMax[%armor,%item] || %item.className == Armor || %item.className == Vehicle || $TestCheats) || %bypass) 
	{
		if(%item.className == Armor && !%player.vehicle) 
		{
			if(%armor != %buyarmor || Player::getItemCount(%client,%item) == 0)
			{
				if(checkResources(%player,%item,1)) 
				{

					%buyarmor = $ArmorType[Client::getGender(%client), %item];
					teamEnergyBuySell(%player,$ArmorName[%armor].price);
					teamEnergyBuySell(%player,$ArmorName[%buyarmor].price * -1);

					Player::setArmor(%client,%buyarmor);

					if(%client.isBuyingFavs)
						checkMax(%client,%buyarmor);
					else
						SellLoadout(%client);

					armorChange(%client);
					GameBase::SetDamageLevel(%player,0);
					Player::setItemCount(%client, $ArmorName[%armor], 0);
					Player::setItemCount(%client, %item, 1);

					if(%client.isBuyingFavs)
					{
						//nothing to do here...
					}					

					else if(%item == iarmorAngel)
					{
						schedule(%player@".usedbeacon = false;",0.001,%player);
						schedule(%player@".usedtshieldbeacon = false;",0.001,%player);
						schedule(%player@".shieldCD = false;",0.001,%player);
						schedule(%player@".usedcloakbeacon = false;",0.001,%player);
						schedule(%player@".beaconcooldown = false;",0.001,%player);
						//auto buy loadouts.
						%client.buyBypass = true;
						buyItem(%client,AngelRepairGun);
						buyItem(%client,HeavensFury);
						buyItem(%client,SoulSucker);
						buyItem(%client,GrapplingHook);
						buyItem(%client,TargetingLaser);
						buyItem(%client,RepairPack);
						buyItem(%client,Beacon);
						buyItem(%client,Grenade);
						buyItem(%client,MineAmmo);
						buyItem(%client,RepairKit);
						%client.buyBypass = false;
					}
					else if(%item == iarmorSpy)
					{
						schedule(%player@".usedbeacon = false;",0.001,%player);
						schedule(%player@".usedtshieldbeacon = false;",0.001,%player);
						schedule(%player@".shieldCD = false;",0.001,%player);
						schedule(%player@".usedcloakbeacon = false;",0.001,%player);
						schedule(%player@".beaconcooldown = false;",0.001,%player);
						//auto buy loadouts.
						%client.buyBypass = true;
						buyItem(%client,Shotgun);  
						buyItem(%client,PlasmaGun);
						buyItem(%client,SniperRifle);
						buyItem(%client,TargetingLaser);
						buyItem(%client,ChameleonPack);
						buyItem(%client,Beacon);
						buyItem(%client,Grenade);
						buyItem(%client,MineAmmo);
						buyItem(%client,DiscAmmo); //added
						buyItem(%client,FlamerAmmo); //added
						buyItem(%client,GrenadeAmmo); //added
						buyItem(%client,HammerAmmo); //added
						buyItem(%client,ThumperAmmo); //added
						buyItem(%client,RepairKit);
						%client.buyBypass = false;
					}
					else if(%item == iarmorNecro)
					{
						schedule(%player@".usedbeacon = false;",0.001,%player);
						schedule(%player@".usedtshieldbeacon = false;",0.001,%player);
						schedule(%player@".shieldCD = false;",0.001,%player);
						schedule(%player@".usedcloakbeacon = false;",0.001,%player);
						schedule(%player@".beaconcooldown = false;",0.001,%player);
						//auto buy loadouts. 
						%client.buyBypass = true;
						buyItem(%client,DisarmerSpell);
						buyItem(%client,ShockingGrasp);
						buyItem(%client,SpellFlameThrower);
						buyItem(%client,TargetingLaser);
						buyItem(%client,GhostPack);
						buyItem(%client,Beacon);
						buyItem(%client,Grenade);
						buyItem(%client,MineAmmo);
						buyItem(%client,HammerAmmo); //added
						buyItem(%client,RepairKit);
						%client.buyBypass = false;
					}
					else if(%item == iarmorWarrior)
					{
						schedule(%player@".usedbeacon = false;",0.001,%player);
						schedule(%player@".usedtshieldbeacon = false;",0.001,%player);
						schedule(%player@".shieldCD = false;",0.001,%player);
						schedule(%player@".usedcloakbeacon = false;",0.001,%player);
						schedule(%player@".beaconcooldown = false;",0.001,%player);
						//auto buy loadouts. 
						%client.buyBypass = true;
						buyItem(%client,Vulcan);
						buyItem(%client,Stinger);
						buyItem(%client,FlameThrower);  
						buyItem(%client,DiscLauncher);
						buyItem(%client,RocketLauncher);
						buyItem(%client,TargetingLaser);
						buyItem(%client,EnergyPack);
						buyItem(%client,Beacon);
						buyItem(%client,Grenade);
						buyItem(%client,MineAmmo);
						buyItem(%client,FlamerAmmo); //added
						buyItem(%client,GrenadeAmmo); //added
						buyItem(%client,HammerAmmo); //added
						buyItem(%client,PlasmaAmmo); //added
						buyItem(%client,RubberAmmo); //added
						buyItem(%client,ShotgunShells); //added
						buyItem(%client,ThumperAmmo); //added
						buyItem(%client,RepairKit);
						%client.buyBypass = false;
					}	
					else if(%item == iarmorBuilder)
					{
						schedule(%player@".usedbeacon = false;",0.001,%player);
						schedule(%player@".usedtshieldbeacon = false;",0.001,%player);
						schedule(%player@".shieldCD = false;",0.001,%player);
						schedule(%player@".usedcloakbeacon = false;",0.001,%player);
						schedule(%player@".beaconcooldown = false;",0.001,%player);
						//auto buy loadouts. 
						%client.buyBypass = true;
						buyItem(%client,Fixit);
						buyItem(%client,Grabbler);
						buyItem(%client,Railgun);
						buyItem(%client,RocketLauncher);
						buyItem(%client,TargetingLaser);
						buyItem(%client,EnergyPack);
						buyItem(%client,Beacon);
						buyItem(%client,Grenade);
						buyItem(%client,MineAmmo);
						buyItem(%client,discammo);  //added
						buyItem(%client,FlamerAmmo); //added
						buyItem(%client,GrenadeAmmo); //added
						buyItem(%client,PlasmaAmmo); //added
						buyItem(%client,ShotgunShells); //added
						buyItem(%client,StingerAmmo); //added
						buyItem(%client,VulcanAmmo); //added
						buyItem(%client,ThumperAmmo); //added
						buyItem(%client,RepairKit);
						%client.buyBypass = false;
					}	
					else if(%item == iarmorTroll)
					{
						schedule(%player@".usedbeacon = false;",0.001,%player);
						schedule(%player@".usedtshieldbeacon = false;",0.001,%player);
						schedule(%player@".shieldCD = false;",0.001,%player);
						schedule(%player@".usedcloakbeacon = false;",0.001,%player);
						schedule(%player@".beaconcooldown = false;",0.001,%player);
						//auto buy loadouts.
						%client.buyBypass = true;
						buyItem(%client,Mortar);  
						buyItem(%client,RocketLauncher);  
						buyItem(%client,PhaseDisrupter);
						buyItem(%client,FlameThrower); 
						buyItem(%client,Minigun);
						buyItem(%client,TargetingLaser);
						buyItem(%client,RegenerationPack);
						buyItem(%client,Beacon);
						buyItem(%client,Grenade);
						buyItem(%client,MineAmmo);
						buyItem(%client,DiscAmmo); //added
						buyItem(%client,FlamerAmmo); //added
						buyItem(%client,GrenadeAmmo); //added
						buyItem(%client,PlasmaAmmo); //added
						buyItem(%client,RubberAmmo); //added
						buyItem(%client,ShotgunShells); //added
						buyItem(%client,StingerAmmo); //added
						buyItem(%client,ThumperAmmo); //added
						buyItem(%client,RepairKit);
						%client.buyBypass = false;
					}
					else if(%item == iarmorTank)
					{
						schedule(%player@".usedbeacon = false;",0.001,%player);
						schedule(%player@".usedtshieldbeacon = false;",0.001,%player);
						schedule(%player@".shieldCD = false;",0.001,%player);
						schedule(%player@".usedcloakbeacon = false;",0.001,%player);
						schedule(%player@".beaconcooldown = false;",0.001,%player);
						//auto buy weapons. plas 3.0 
						%client.buyBypass = true;
						buyItem(%client,TBlastCannon);
						buyItem(%client,TRocketLauncher);
						buyItem(%client,TankRPGLauncher);
						buyItem(%client,TankShredder);
						buyItem(%client,TargetingLaser);
						buyItem(%client,EnergyPack);
						buyItem(%client,Beacon);
						buyItem(%client,Grenade);
						buyItem(%client,MineAmmo);
						buyItem(%client,RepairKit);
						%client.buyBypass = false;	
					}	
					else if(%item == iarmorTitan)
					{
						schedule(%player@".usedbeacon = false;",0.001,%player);
						schedule(%player@".usedtshieldbeacon = false;",0.001,%player);
						schedule(%player@".shieldCD = false;",0.001,%player);
						schedule(%player@".usedcloakbeacon = false;",0.001,%player);
						schedule(%player@".beaconcooldown = false;",0.001,%player);
						//auto buy loadouts. 
						%client.buyBypass = true;
						buyItem(%client,ParticleBeamWeapon);
						buyItem(%client,RocketLauncher);
						buyItem(%client,PhaseDisrupter);
						buyItem(%client,BabyNukeMortar);
						buyItem(%client,OSLauncher);
						buyItem(%client,TargetingLaser);
						buyItem(%client,EnergyPack);
						buyItem(%client,Beacon);
						buyItem(%client,Grenade);
						buyItem(%client,MineAmmo);
						buyItem(%client,DiscAmmo); //added
						buyItem(%client,FlamerAmmo); //added
						buyItem(%client,FlameThrowerAmmo); //added
						buyItem(%client,GrenadeAmmo); //added
						buyItem(%client,PlasmaAmmo); //added
						buyItem(%client,RubberAmmo); //added
						buyItem(%client,StingerAmmo); //added
						buyItem(%client,ThumperAmmo); //added
						buyItem(%client,RepairKit);
						%client.buyBypass = false;
					}

					%client.isBuyingFavs = false;
					
					if(Player::getMountedItem(%client,$BackpackSlot) == ammopack) 
						fillAmmoPack(%client);

					if($Annihilation::ShoppingList && %client.invo)
						setupShoppingList(%client,%client.invo,%client.ListType); // armor based list

					if($build || $Annihilation::QuickInv || %Client.InvConnect)	
						setupQuickShoppingList(%client); 
	
					return 1;
				}
				teamEnergyBuySell(%player,$ArmorName[%armor].price * -1);
			}
		}
		else if(%item.className == Backpack) 
		{
			if($TeamItemMax[%item] != "" && !$build)
			{
				if($TeamItemCount[GameBase::getTeam(%client) @ %item] >= $TeamItemMax[%item])
				return 0;
			}

			%pack = Player::getMountedItem(%client,$BackpackSlot);
			if(%pack != -1) 
			{
				if(%pack == ammopack) 
					checkMax(%client,%armor);
				teamEnergyBuySell(%player,%pack.price);
				Player::decItemCount(%client,%pack);
			}
			else
			{
				teamEnergyBuySell(%player,%pack.price * -1);
				Player::incItemCount(%client,%pack);
				Player::useItem(%client,%pack);

				if(%pack == ammopack) 
					fillAmmoPack(%client);
			}

			if(checkResources(%player,%item,1) || $testCheats) 
			{
				teamEnergyBuySell(%player,%item.price * -1);
				Player::incItemCount(%client,%item);
				Player::useItem(%client,%item);
				if(%item == ammopack) 
					fillAmmoPack(%client);
				return 1;
			}				 
		}
		else if(%item.className == Weapon) 
		{
			if(checkResources(%player,%item,1)) 
			{
				Player::incItemCount(%client,%item);
				teamEnergyBuySell(%player,(%item.price * -1));
				%ammoItem = %item.imageType.ammoType;
				if(%ammoItem != "") 
				{
					%delta = checkResources(%player,%ammoItem,$ItemMax[%armor, %ammoItem]);
					if(%delta || $testCheats) 
					{
						teamEnergyBuySell(%player,(%ammoItem.price * -1 * %delta));
						Player::incItemCount(%client,%ammoitem,%delta);
					}
				}
				return 1;
			}
		}
		else if(%item.className == Vehicle) 
		{
			if($TeamItemCount[GameBase::getTeam(%client) @ %item] < $TeamItemMax[%item]) 
			{
				%shouldBuy = VehicleStation::checkBuying(%client,%item);
				if(%shouldBuy == 1) 
				{
					teamEnergyBuySell(%player,(%item.price * -1));
					return 1;
				}
				else if(%shouldBuy == 2)
					return 1;
			}
		}
		else 
		{
			if($TeamItemMax[%item] != "" && !$build) 
			{	
				if($TeamItemCount[GameBase::getTeam(%client) @ %item] >= $TeamItemMax[%item])
				return 0;
			}
			%delta = checkResources(%player,%item,$ItemMax[%armor, %item]);
			if(%delta || $testCheats) 
			{
				teamEnergyBuySell(%player,(%item.price * -1 * %delta));
				Player::incItemCount(%client,%item,%delta);
				return 1;
			}
		}
		
	}
	return 0;
}

function armorChange(%client)
{
	$shieldTime[%client] = "";
	$cloakTime[%client]  = "";
	%player = Client::getOwnedObject(%client);
	%armor = $ArmorName[Player::getArmor(%Player)];
	if(%client.respawn == "" && %player.Station != "") 
	{
		
		%sPos = GameBase::getPosition(%player.Station);
		%pPos	= GameBase::getPosition(%client);
		%posX = getWord(%sPos,0);
		%posY = getWord(%sPos,1);
		%posZ = getWord(%pPos,2);
		%vec = Vector::getFromRot(GameBase::getRotation(%player.Station),-1);
		%newPosX = (getWord(%vec,0) * 1) + %posX;
		%newPosY = (getWord(%vec,1) * 1) + %posY;
		GameBase::setPosition(%client, %newPosX @ " " @ %newPosY @ " " @ %posZ);
	}
}

function remoteBuyItem(%client,%type)
{
	if( CheckEval("remoteBuyItem", %client, %type) )
		return;
		
	%player = client::getownedobject(%client);
	if(%player.repackEnergy != "")
	{
	%player.repackDamage = "";
	%player.repackEnergy = "";
	}

	
	if(isPlayerBusy(%client))
		return;

	%item = getItemData(%type);
	if(buyItem(%client,%item)) 
	{
		%client.lastActiveTimestamp = getSimTime(); //AFK System
 		Client::sendMessage(%client,0,"~wbuysellsound.wav");
		updateBuyingList(%client);
	}
	else 
		Client::sendMessage(%client,0,"You couldn't buy "@ %item.description @"~wC_BuySell.wav");
}

function remoteSellItem(%client,%type)
{
	if( CheckEval("remoteSellItem", %client, %type) )
		return;
		
	if(isPlayerBusy(%client))
		return;

	%item = getItemData(%type);
	%player = Client::getOwnedObject(%client);
	
	if(%player.repackEnergy != "")
	{
	%player.repackDamage = "";
	%player.repackEnergy = "";
	}
	
	if(%player == -1 || %client.observerMode != "")
		return;		
	if(Client::isItemShoppingOn(%client,%item)) 
	{
		if(Player::getItemCount(%client,%item) && %item.className != Armor) 
		{
			%numsell = 1;
			if(%item.className == Ammo || %item.className == HandAmmo) 
			{
				%count = Player::getItemCount(%client, %item);
				if(%count < $SellAmmo[%item]) 
					%numsell = %count;
				else 
					%numsell = $SellAmmo[%item];
			}
			else if(%item == ammopack) 
				checkMax(%client,Player::getArmor(%client));
			else if($TeamItemMax[%item] != "") 
			{
				if(%item.className == Vehicle) 
					$TeamItemCount[(Client::getTeam(%client)) @ %item]--;
			}
			else if(%item == EnergyPack) 
			{ 
				//if(Player::getItemCount(%client,"LaserRifle") > 0) 
				//{
				//	Client::sendMessage(%client,0,"Sold Energy Pack - Auto Selling Laser Rifle");
				//	remoteSellItem(%client,22);
				//}
			}
			teamEnergyBuySell(%player,%item.price * %numsell);
			Player::setItemCount(%player,%item,(%count-%numsell));
			updateBuyingList(%client);
			Client::SendMessage(%client,0,"~wbuysellsound.wav");
			%client.lastActiveTimestamp = getSimTime(); //AFK System
			return 1;
		}
	}
	Client::sendMessage(%client,0,"Cannot sell item ~wC_BuySell.wav");
	remoteDropItem(%client,%type); // You can't sell but you can drop
}

$WaitThrowTime = 2;
// This function can pass a player ID or a client ID through %player, but Player::getClient is used to make sure the client is passed.
function remoteUseItem(%player,%type)
{
//	Anni::Echo("UseItem: "@%player@", "@%type@".");
	%client = Player::getClient(%player);
	%player = Client::getOwnedObject(%client);
	%item = getItemData(%type);

	if( CheckEval("remoteUseItem", %client, %type) )
		return;

	if ( %player == -1 || %player == "" || Player::isDead(%player) || !$matchStarted || $loadingMission || %player != Client::getOwnedObject(%client) || %client != GameBase::getControlClient(%player) )
		return;
	
	%client.lastActiveTimestamp = getSimTime(); //AFK System
	%station = %player.Station;
	%vehicle = %player.vehicle;

	if ( %item == "" || %item == -1 )
		return;
		
	if(%item == Blaster)
	{
		Weapon::Mode(%player,1);
		return;
	}
	if(%item == LaserRifle)
	{
		Weapon::Mode(%player,-1);
		return;
	}
	if(%item == DiscLauncher)
	{
		if(Player::getArmor(%client) == "armormLightArmor" || Player::getArmor(%client) == "armorfLightArmor")
		{
			if ( Player::getItemCount(%player,DiscLauncherBase) > 0 )
				Player::useItem(%player,DiscLauncherBase);
		}
		else if(Player::getArmor(%client) == "armormMercenary" || Player::getArmor(%client) == "armorfMercenary")
		{
			if ( Player::getItemCount(%player,DiscLauncherElite) > 0 )
				Player::useItem(%player,DiscLauncherElite);
		}
		else
		{
			if ( Player::getItemCount(%player,%item) > 0 )
				Player::useItem(%player,%item);
			return;
		}
	}
	
	if(%item == PlasmaGun)
	{
		if(Player::getArmor(%client) == "armormLightArmor" || Player::getArmor(%client) == "armorfLightArmor")
		{
			if ( Player::getItemCount(%player,PlasmaGunBase) > 0 )
				Player::useItem(%player,PlasmaGunBase);
		}
		else if(Player::getArmor(%client) == "armormMercenary" || Player::getArmor(%client) == "armorfMercenary")
		{
			if ( Player::getItemCount(%player,PlasmaGunBase) > 0 )
				Player::useItem(%player,PlasmaGunBase);
		}
		else
		{
			if ( Player::getItemCount(%player,%item) > 0 )
				Player::useItem(%player,%item);
			return;
		}
	}
	// Bind 8 to Grappling Hook
	if(%item == Mortar) //for clan <!> pushbiscuit
	{
        Player::useItem(%player,GrapplingHook);
		return;
	}
	// end weap options.	
		
	if(%Station && %item != "RepairKit")
	{
		Client::sendMessage(%client,0,"Error! Close the station first. ~waccess_denied.wav");
		return;
	}

	%client.throwStrength = 1;

	if(%item == "Backpack") 
	{			
		if (Client::getGuiMode(%client) == $GuiModeInventory)
		{
			Client::sendMessage(%client,0,"Error! Close the station first. ~waccess_denied.wav");
			return;
		}
		else 
			%item = Player::getMountedItem(%player,$BackpackSlot);
		}
	else
	{
		if(%item == Weapon) 
			%item = Player::getMountedItem(%player,$WeaponSlot);
	}
	if(%item.className == Backpack)
	{
		if(%client != GameBase::getControlClient(%player))
		{		
			%object = Client::getControlObject(%client);
			if(%vehicle)
				vehicle::jump(%object,"0 0 100");
		}
	}
//		if ( %item == "RepairKit" )
//		{
//			if ( Player::getItemCount(%player,%item) <= 0 && %player.hasmessage) 
//			{
//				Client::sendMessage(%client,1, "You don't have a Repair Kit. ~wC_BuySell.wav");
//				%player.hasmessage = false;
//				schedule(%player@".hasmessage = true;",1.0,%player);	
//			}
//		if ( Player::getItemCount(%player,%item) > 0 )
//		{
//			%player.hasmessage = true;
//		}
//		}
	if ( %item == "Beacon" )
	{
		if(!%player.inArenaTD)
			%player.invulnerable = "";
		if(%player.forker && gamebase::getteam(%player.forker) == gamebase::getteam(%player))
		{
			Player::trigger(%player.forker,$WeaponSlot,false);
			%player.forker = "";
			return;
		}
	if ( Player::getItemCount(%player,%item) <= 0 && %player.hasmessage) //adding a message for no beacons here -death666
	{
		Client::sendMessage(%client,0, "You have no beacons to use. ~wC_BuySell.wav");
		%player.hasmessage = false;
		schedule(%player@".hasmessage = true;",1.0,%player);	
	}
		if ( Player::getItemCount(%player,%item) > 0 )
		{
			%player.hasmessage = true;
			%armor = Player::getArmor(%player);
			if ( %armor != "" && %armor != -1 )
			{
				//$Log="Player: "@%player@" using beacon "@%item@" with armor "@%armor@". Vel='"@Item::getVelocity(%player)@" Pos='"@GameBase::getPosition(%player)@"'"; 
				//export("Log", "config\\crash"@$LogNumber@".log");
				if ( %armor == "armormAngel" || %armor == "armorfAngel" )
					if(%player.usedbeacon)
					{
						Client::sendMessage(%client,0, "Emergency break cooling down. ~wC_BuySell.wav");
						return;
					}
					else
				{
					Client::sendMessage(%client,1, "Emergency break initiated. ~wcommand_power.wav");
					GameBase::playSound(%player, SoundCommandStationPower, 0);
					Item::setVelocity(%player, "0 0 0");

	%clientId = Player::getClient(%player);
	%wep = Player::getMountedItem(%player,$WeaponSlot);
	Player::unmountItem(%player,0);
	Schedule("Player::mountItem("@ %player @","@ %wep @", "@ $WeaponSlot @");",0.5,%player);

	%pos = getBoxCenter(%player);
	%energy = GameBase::getEnergy(%player);


	if(%energy != 40)
	{
		PlaySound("SoundFirePlasma",%pos);
		for(%i = 0; %i < floor(%energy); %i++) {
			Projectile::SpawnProjectile("NovaBoltA","0 0 0 0 0 0 0 0 0 "@%pos, %player, 50);
		}
	}
	else
	{
		PlaySound("SoundPlasmaTurretFire",%pos);
		for(%i = 0; %i < floor(%energy); %i++) {
			Projectile::SpawnProjectile("NovaBoltB","0 0 0 0 0 0 0 0 0 "@%pos, %player, 50);
		}
	}

					%player.usedbeacon = true;
					schedule(%player@".usedbeacon = false;",2.5,%player); //2.5
				}
				else if ( %armor == "armormSpy" || %armor == "armorfSpy" || %armor == "armormNecro" || %armor == "armorfNecro" || %armor == "lghost" || %armor == "fghost" )
				{
					if(%player.beaconcooldown == true)
					{
						Client::sendMessage(%client,1, "Cloak cooling down. ~wfailpack.wav");
						return;
					}
					if(%player.usedcloakbeacon)
					{
						Client::sendMessage(%client,3, "Cloaking already active. ~wC_BuySell.wav");
						return;
					}
					else
					if(Player::getItemCount(%player,Flag) > 0) //no more flag cloaking -death666
					{
						Client::sendMessage(%client,1,"Unable to cloak with the flag! ~waccess_denied.wav");
						return;
					}
					else
					startCloak(%client);
					%player.usedcloakbeacon = true;
				}
				else if ( %armor == "armormWarrior" || %armor == "armorfWarrior" || %armor == "armormDM" || %armor == "armorfDM" )
				{
					%vel = Item::getVelocity(%player);

					//if(getsimtime() - %player.LastBeacon > 4.0)
					//	%player.BeaconTimer = 0;	
					//
					//%player.LastBeacon = getSimTime();	
					//
					//if(%player.BeaconTimer > 80)
					//{			
					//	%Pos = getboxcenter(%player); 
					//	%trans =  "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%player); 
					//	Projectile::spawnProjectile("suicideShell", %trans, %player, %vel);
					//	Client::sendMessage(%client,0, "Your booster exploded!");
					//}
					//else
					//{
						if(%vel == "0 0 0")
						{
							%trans = GameBase::getMuzzleTransform(%player);
							%smack = 300/25;
							%rot=GameBase::getRotation(%player);
							%len = 30;
							%tr= getWord(%trans,5);
							if(%tr <=0 )%tr -=%tr;
							%up = %tr+0.15;
							%out = 1-%tr;
							%vec = Vector::getFromRot(%rot,%len*%out*%smack,%len*%up*%smack);
							if ( ( getWord(%vec,0) >= 0 || getWord(%vec,0) < 0 ) && ( getWord(%vec,1) >= 0 || getWord(%vec,1) < 0 ) && ( getWord(%vec,2) >= 0 || getWord(%vec,2) < 0 ) )
								Player::applyImpulse(%player, getWord(%vel,0)+getWord(%vec,0)@" "@getWord(%vel,1)+getWord(%vec,1)@" "@getWord(%vel,2)+getWord(%vec,2) );
							else
							{
								admin::message("ERROR: Booster Vectoring error!  Vec=\""@%vec@"\" from person "@GameBase::getName(%client)@" ("@%player@")");
								$Admin = "ERROR: Booster Vectoring error!  Vec=\""@%vec@"\" from person "@GameBase::getName(%client)@" ("@%player@")";
								export("Admin", "config\\Error.log", true);
								return;
							}
							GameBase::playSound(%player, SoundFireMortar, 0);
							Client::sendMessage(%client,0, "Speed Boost Initiated..");	
						}
						else
						{	
							%vel = Vector::Normalize(%vel);
							%vec = GetWord(%vel, 0) * 300 @ " " @ GetWord(%vel, 1) * 300 @ " " @ GetWord(%vel, 2) * 300;
							if ( ( getWord(%vec,0) >= 0 || getWord(%vec,0) < 0 ) && ( getWord(%vec,1) >= 0 || getWord(%vec,1) < 0 ) && ( getWord(%vec,2) >= 0 || getWord(%vec,2) < 0 ) )
								Player::applyImpulse(%player, %vec );
							else
							{
								admin::message("ERROR: Booster Vectoring error!  Vec=\""@%vec@"\" from person "@GameBase::getName(%client)@" ("@%player@")");
								$Admin = "ERROR: Booster Vectoring error!  Vec=\""@%vec@"\" from person "@GameBase::getName(%client)@" ("@%player@")";
								export("Admin", "config\\Error.log", true);
								return;
							}
							GameBase::playSound(%player, SoundFireMortar, 0);
							Client::sendMessage(%client,1, "Speed boost initiated. ~wmortar_fire.wav");	
						}
					//}
		
					//if(!%player.BeaconTimer)
					//	Beacon::timer(%player);
					//
					//if(%player.BeaconTimer < 250)
					//	%player.BeaconTimer += 40;
				}
				else if ( %armor == "armormMercenary" || %armor == "armorfMercenary" ) 
				{
					EliteBoost::onUse(%player,%item);
					return;
				}
				else if ( %armor == "armormLightArmor" || %armor == "armorfLightArmor" ) 
				{
					Beacon::deployShape(%player,%item);
					return;
				}
				else if ( %armor == "armormBuilder" || %armor == "armorfBuilder" )
				{
					%bitem = "DeployableInvPack";
					if($TeamItemCount[GameBase::getTeam(%player) @ %bitem] >= $TeamItemMax[%bitem] && !$build) 
					{
						Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %bitem.description @ "s.");
					        Client::sendMessage(%client,0,"~wC_BuySell.wav"); //first new wav all buysell wavs below this point are new -death666
						return;
					}
					if(!GameBase::getLOSInfo(%player,5)) 
					{
						Client::sendMessage(%client,0,"Deploy position out of range.");
					        Client::sendMessage(%client,0,"~wC_BuySell.wav");
						return;
					}
					%obj = getObjectType($los::object); 

					if(%obj != "SimTerrain" && %obj != "InteriorShape" && GameBase::getDataName($los::object) != "DeployablePlatform" && !$build) 
					{
						Client::sendMessage(%client,0,"Can only deploy on terrain or buildings.");
					        Client::sendMessage(%client,0,"~wC_BuySell.wav");
						return;
					}
					if(Vector::dot($los::normal,"0 0 1") <= 0.7) 
					{
						Client::sendMessage(%client,0,"Can only deploy on flat surfaces.");
					        Client::sendMessage(%client,0,"~wC_BuySell.wav");
						return;
					}
					if(!checkInvDeployArea(%client,$los::position))
						return false;
		
					%inv = newObject("ammounit_remote","StaticShape","DeployableInvStation",true); 
					%inv.deployer = %client; 

					%inv.cloakable = true;	//for base cloaker
					addToSet("MissionCleanup/deployed/station", %inv); 
					%rot = GameBase::getRotation(%player); 
					GameBase::setTeam(%inv,GameBase::getTeam(%player)); 
					GameBase::setPosition(%inv,$los::position); 
					GameBase::setRotation(%inv,%rot); 
					Gamebase::setMapName(%inv,"Portable Inventory"); //changed from Inventory Station -death666
					Client::sendMessage(%client,1,"Inventory Station deployed. ~wDryfire1.wav"); 
					playSound(SoundPickupBackpack,$los::position);
					$TeamItemCount[GameBase::getTeam(%inv) @ "DeployableInvPack"]++; 
					if(!$build)
						Anni::Echo("MSG: ",%client," deployed an Inventory Station");
				}
				else if ( %armor == "armorTroll" ) //  || %armor == "armorfTroll" )
				{
					//startShield(%client, %player); 
					if(%player.usedbeacon) //and take back just as quick, would rather rely on Troll Pack
					{
						Client::sendMessage(%client,0, "Your emergency defenses must regenerate. ~wC_BuySell.wav");
						return;
					}
					else
					{
						Client::sendMessage(%client,1, "Emergency defenses activated. ~wplasma2.wav");
						GameBase::playSound(%player, SoundFirePlasma, 0);
						for(%i=0; %i < 6.28; %i += 1.256) 
						{
							%forceVel = Vector::getFromRot("0 0 " @ %i, 10, 5);
							%trans =  "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%player);
 							%obj = Projectile::spawnProjectile("TrollBurn", %trans, %player, %forceVel);
							Projectile::spawnProjectile(%obj); // WTF?
							Item::setVelocity(%obj, %forceVel);

							%forceVel = Vector::getFromRot("0 0 " @ %i, 11, 6);
							%trans =  "0 0 2 0 0 0 0 0 2 " @ getBoxCenter(%player);
 							%obj = Projectile::spawnProjectile("TrollBurn2", %trans, %player, %forceVel);
							Projectile::spawnProjectile(%obj); // WTF?
							Item::setVelocity(%obj, %forceVel);
						}
						%player.usedbeacon = true;
						schedule(%player@".usedbeacon = false;",4.5,%player); //1.5 2.5
					}
				}
				else if ( %armor == "armorTank" )
				{
					if(%player.usedbeacon)
					{
						Client::sendMessage(%client,0, "Emergency defenses must replenish. ~wC_BuySell.wav");
						return;
					}
					else
					{
						// startShield(%client, %player);
						// Client::sendMessage(%client,0, "Armor protection shields activated.");
						Client::sendMessage(%client,0, "Emergency defenses deployed. ~wdebris_medium.wav");
						GameBase::playSound(%player, debrisMediumExplosion, 0);
						Client::sendMessage(%client,1, "Warning: Armor exhaust vented.");
						%vel = Item::getVelocity(%player);
					     // %trans =  "0 0 1 0 0 1 0 0 1 " @ getBoxCenter(%player);		//"0 0 1 0 0 0 0 0 1 "
						%trans = "0 0 1 0 0 1 0 0 1 " @ vector::add(getBoxCenter(%player),"0 0 2.0"); //4.0
						%obj = Projectile::spawnProjectile("Smokegrenade", %trans, %player, %vel); //TankShockShell
						Projectile::spawnProjectile(%obj);
						%player.usedbeacon = true;
						schedule(%player@".usedbeacon = false;",7.0,%player); //2.5
					}
				}
				else if ( %armor == "armorTitan" )
				{
					if(%player.usedtshieldbeacon == true)
					{
						Client::sendMessage(%client,3, "Shield already active. ~wC_BuySell.wav");
						return;
					}
					else
					if(%player.shieldCD == true)
					{
						Client::sendMessage(%client,1, "Shield cooling down. ~wfailpack.wav");
						return;
					}
					else
					{
					Client::sendMessage(%client,3, "Emergency Shield activated. ~wForceOpen.wav");
					startShield(%client, %player);
					%player.usedtshieldbeacon = true;
					}	
				}
				if ( !$build )
					Annihilation::decItemCount(%player,%item);
			}
		}
		return;
	}
	if ( Player::getItemCount(%player,%item) > 0 )
		Player::useItem(%player,%item);
}

function remoteThrowItem(%client,%type,%strength)
{
	%item = getItemData(%type);
	if( CheckEval("remoteThrowItem", %client, %type, %strength) )
		return;
		
	%player = Client::getOwnedObject(%client);

	if ( %player==-1 || Player::isDead(%player) )
		return;
	
	if(%player.Station == "" && %player.waitThrowTime + $WaitThrowTime <= getSimTime())
	{
		if(GameBase::getControlClient(%player) != -1 || %player.vehicle != "") 
		{
			//if(GameBase::getControlClient(%player) != -1)
			//{
			//Anni::Echo("Throw item: " @ %type @ " " @ %strength);
			if(%type == 41)
				%item = "Grenade";
			else
				%item = getItemData(%type);

			if(%item == Grenade || %item == MineAmmo) 
			{
				if(%strength < 0)
					%strength = 0;
				else
					if(%strength > 100)
						%strength = 100;
				%client.throwStrength = 0.3 + 0.7 * (%strength / 100);
				Player::useItem(%client,%item);
			}
		}
	}
}

function remoteDropItem(%client,%type)
{
	if((Client::getOwnedObject(%client)).driver != 1 && !%client.ThrowWait)
	{
		
		%client.throwStrength = 1;
		%item = getItemData(%type);
		
		if($debug)
			Anni::Echo(" remote Drop item: ",%client,%type,%item);
		
		if(%item == Backpack)
		{
			%item = Player::getMountedItem(%client,$BackpackSlot);
			Player::dropItem(%client,%item);
		}
		else if(%item == Weapon)
		{
			%item = Player::getMountedItem(%client,$WeaponSlot);
			Player::dropItem(%client,%item);
		}
		else if(%item == Ammo)
		{
			%item = Player::getMountedItem(%client,$WeaponSlot);
			if(%item.className == Weapon)
			{
				%item = %item.imageType.ammoType;
				if($debug) 
					Anni::Echo("ammo type =",%item);
				if(%item != "")
					Player::dropItem(%client,%item);
			}
		}
		else if(%item == RepairKit)
		{
			Player::dropItem(%client,%item);
		}	
		else if(%item == Flag)
		{	
			%flag = Player::getMountedItem(%client,$FlagSlot);
	
			if(%flag == ArenaFlag)
				Player::dropItem(%client,"ArenaFlag");
			else
				Player::dropItem(%client,%item);
		}	
		else
			Player::dropItem(%client,%item);
	
		if(%client.InvConnect)
		{	
			%client.ThrowWait = true;
			schedule(%client@".ThrowWait = false;",0.25,%client);		
		}
	}		
}


addAmmo("", RepairKit, 1);


function remoteDeployItem(%client,%type)
{
	if( CheckEval("remoteDeployItem", %client, %type) )
		return;
		
	 //Anni::Echo("Deploy item: ",%type);
	%item = getItemData(%type);
	Player::deployItem(%client,%item);
}



function remoteNextWeapon(%client)
{		
	if($debug)
		Anni::Echo("remoteNextWeapon"@ Client::getControlObject(%client)@ " owned "@Client::getOwnedObject(%client)@" is observer= "@Observer::isObserver(%client)@" admin obs ="@%client.AdminobserverMode);
	
	if($loadingMission)
		return;

	if(%client.InSchool)
	{
		if(%client.InSchool < 8)
		{
			%client.InSchool++;
			NewbieSchool(%client);
			return;
		}
		else 
		{
			%clientId.weaponHelp = true;
			centerprint(%client,"",0);	
			if(%client.observerMode == "justJoined")
				Observer::triggerUp(%client);
			else
				%client.InSchool = "";	
				
			return;					
		}
	}

	if(%client.observerMode == dead || %client.observerMode == justJoined || %client.observerMode == observerFly) return;

	if(%client.observerMode == "observerOrbit" || %client.observerMode == "observerAdmin")
	{
			%client.obsmode += 1;
			if(%client.obsmode >= 6)
				%client.obsmode = "";	
			if($debug)
				Anni::Echo("toggle obsmode "@%client@" to "@%client.obsmode);
				
			Observer::setAnnihilationOrbit(%client, %client.observerTarget);
			%now = getSimTime(); //OBS AFK System -Ghost
			%client.lastActiveOBSTimestamp = %now; //OBS AFK System -Ghost
			return;		
	}	
	//end toggle, start weapon change
	else
	{

		%player = Client::getOwnedObject(%client);		
		%item = Player::getMountedItem(%client,$WeaponSlot);
//		if(!%player.inArenaTD)  making this global again and removing the automatic plasma rifle equip when players spawn in game.cs function Game::playerSpawned -death666
			%player.invulnerable = "";
// remove invulnerability when player selects weapon.
		if(%item == -1 || $NextWeapon[%item] == "")
			selectValidWeapon(%client);
		else if(%client.TitanRotation != false && Player::getArmor(%client) == "armorTitan")
		{
			for(%weapon = $NextWeaponTitan[%item]; %weapon != %item; %weapon = $NextWeaponTitan[%weapon])
			{
				if(isSelectableWeapon(%client,%weapon))
				{
					Player::useItem(%client,%weapon);
					// Make sure it mounted (laser may not), or at least
					// next in line to be mounted.
					if(Player::getMountedItem(%client,$WeaponSlot) == %weapon || Player::getNextMountedItem(%client,$WeaponSlot) == %weapon)
						break;	
				}
			}			
		}
		else
		{
			for(%weapon = $NextWeapon[%item]; %weapon != %item; %weapon = $NextWeapon[%weapon])
			{
				if(isSelectableWeapon(%client,%weapon))
				{
					Player::useItem(%client,%weapon);
					// Make sure it mounted (laser may not), or at least
					// next in line to be mounted.
					if(Player::getMountedItem(%client,$WeaponSlot) == %weapon || Player::getNextMountedItem(%client,$WeaponSlot) == %weapon)
						break;	
				}
			}			
		}
	}
}
	
function remotePrevWeapon(%client)
{		
	if($loadingMission)
		return;

	if(%client.InSchool)
	{
		if(%client.InSchool < 8)
		{
			%client.InSchool--;
			NewbieSchool(%client);
			return;
		}
		else 
		{
			%clientId.weaponHelp = true;
			centerprint(%client,"",0);	
			if(%client.observerMode == "justJoined")
				Observer::triggerUp(%client);
			else
				%client.InSchool = "";	
				
			return;					
		}
	}

	if(%client.observerMode == dead || %client.observerMode == justJoined || %client.observerMode == observerFly) return;

	if(%client.observerMode == "observerOrbit" || %client.observerMode == "observerAdmin")
	{
			%client.obsmode -= 1;
			if(%client.obsmode <= 0)
				%client.obsmode = "";	
			if($debug)
				Anni::Echo("toggle obsmode "@%client@" to "@%client.obsmode);
				
			Observer::setAnnihilationOrbit(%client, %client.observerTarget);
			%now = getSimTime(); //OBS AFK System -Ghost
			%client.lastActiveOBSTimestamp = %now; //OBS AFK System -Ghost
			return;		
	}	
	//end toggle, start weapon change
	else
	{

		%player = Client::getOwnedObject(%client);		
		%item = Player::getMountedItem(%client,$WeaponSlot);
		if(!%player.inArenaTD)
			%player.invulnerable = "";
		// remove invulnerability when player selects weapon.	
		
		if(%item == -1 || $PrevWeapon[%item] == "")
			selectValidWeapon(%client);
		else if(%client.TitanRotation != false && Player::getArmor(%client) == "armorTitan")
		{
			for(%weapon = $PrevWeaponTitan[%item]; %weapon != %item; %weapon = $PrevWeaponTitan[%weapon])
			{
				if(isSelectableWeapon(%client,%weapon))
				{
					Player::useItem(%client,%weapon);
					// Make sure it mounted (laser may not), or at least
					// next in line to be mounted.
					if(Player::getMountedItem(%client,$WeaponSlot) == %weapon || Player::getNextMountedItem(%client,$WeaponSlot) == %weapon)
						break;	
				}
			}			
		}
		else
		{
			for(%weapon = $PrevWeapon[%item]; %weapon != %item; %weapon = $PrevWeapon[%weapon])
			{
				if(isSelectableWeapon(%client,%weapon))
				{
					Player::useItem(%client,%weapon);
					// Make sure it mounted (laser may not), or at least
					// next in line to be mounted.
					if(Player::getMountedItem(%client,$WeaponSlot) == %weapon || Player::getNextMountedItem(%client,$WeaponSlot) == %weapon)
						break;	
				}
			}			
		}
	}
}

function selectValidWeapon(%client)
{
	if(%client.TitanRotation && Player::getArmor(%client) == "armorTitan")
	{
		//%item = EnergyRifle;
		if(isSelectableWeapon(%client,$FirstWeaponTitan))
		{
			Player::useItem(%client,$FirstWeaponTitan);
			return;
		}
			
		%item = $FirstWeaponTitan; 
		for(%weapon = $NextWeaponTitan[%item]; %weapon != %item; %weapon = $NextWeaponTitan[%weapon])
		{
			if(isSelectableWeapon(%client,%weapon))
			{
				Player::useItem(%client,%weapon);
				break;
			}
		}
	}
	else
	{
		//%item = EnergyRifle;
		if(isSelectableWeapon(%client,$FirstWeapon))
		{
			Player::useItem(%client,$FirstWeapon);
			return;
		}
			
		%item = $FirstWeapon; 
		for(%weapon = $NextWeapon[%item]; %weapon != %item; %weapon = $NextWeapon[%weapon])
		{
			if(isSelectableWeapon(%client,%weapon))
			{
				Player::useItem(%client,%weapon);
				break;
			}
		}
	}
}

function isSelectableWeapon(%client,%weapon)
{
	if(Player::getItemCount(%client,%weapon))
	{
		%ammo = $WeaponAmmo[%weapon];
		if(%ammo == "" || Player::getItemCount(%client,%ammo) > 0)
			return true;
	}
	return false;
}


//----------------------------------------------------------------------------
// Default item scripts
//----------------------------------------------------------------------------

function Item::giveItem(%player,%item,%delta)
{
	%armor = Player::getArmor(%player);
	if(%player.repackEnergy != "")
	{
	%player.repackDamage = "";
	%player.repackEnergy = "";
	}
	if($ItemMax[%armor, %item])
	{
		%client = Player::getClient(%player);
		%description = %item.description;
		if(%item.className == Backpack)
		{
			// Only one backpack per armor, and it's always mounted
			if(Player::getMountedItem(%player,$BackpackSlot) == -1)
			{
				Player::incItemCount(%player,%item);
				Player::useItem(%player,%item);
				Client::sendMessage(%client,0,"You received a " @ %description @ " backpack");
				return 1;
			}
		}
		else
		{
			// Check num weapons carried by player can't have more then max
			if(%item.className == Weapon)
			{
				if(Player::getItemClassCount(%player,"Weapon") >= $MaxWeapons[%armor]) 
					return 0;
			}
			%extraAmmo = 0;
			if(Player::getMountedItem(%client,$BackpackSlot) == ammopack && $AmmoPackMax[%item] != "") 
				%extraAmmo = $AmmoPackMax[%item];
			// Make sure it doesn't exceed carrying capacity
			%count = Player::getItemCount(%player,%item);
			if(%count + %delta > $ItemMax[%armor, %item] + %extraAmmo) 
				%delta = ($ItemMax[%armor, %item] + %extraAmmo) - %count;
			if(%delta > 0)
			{
				Player::incItemCount(%player,%item,%delta);
				if(%count == 0 && $AutoUse[%item]) 
					Player::useItem(%player,%item);
				Client::sendMessage(%client,0,"You received " @ %delta @ " " @ %description);
				return %delta;
			}
		}
	}
	return 0;
}


//----------------------------------------------------------------------------
// Default Item object methods

$PickupSound[Ammo] = "SoundPickupAmmo";
$PickupSound[Weapon] = "SoundPickupWeapon";
$PickupSound[Backpack] = "SoundPickupBackpack";
$PickupSound[Repair] = "SoundPickupHealth";

function Item::playPickupSound(%this)
{
	%item = Item::getItemData(%this);
	%sound = $PickupSound[%item.className];
	if(%sound != "")
		playSound(%sound,GameBase::getPosition(%this));
	else 
	{
		// Generic item sound
		playSound(SoundPickupItem,GameBase::getPosition(%this));
	}
}	

function Item::respawn(%this)
{
	// If the item is rotating we respawn it,
	if(Item::isRotating(%this)) 
	{
		Item::hide(%this,True);
		schedule("Item::hide(" @ %this @ ",false); GameBase::startFadeIn(" @ %this @ ");",$ItemRespawnTime,%this);
	}
	else 
	{ 
		deleteObject(%this);
	}
}	

function Item::onAdd(%this)
{
	if($debug)Anni::Echo("Item::onAdd(%this) "@%this);
	$item::count += 1;
	if($debug)
		bottomprint(2049,"Ammo count= "@$Ammo::count@" Item count= "@$item::count@" Miner count= "@$mine::count);
}

function Item::onRemove(%this)
{
	$item::count -= 1;
	if($debug)
		bottomprint(2049,"Ammo count= "@$Ammo::count@" Item count= "@$item::count@" Miner count= "@$mine::count);
}

function Item::onCollision(%this,%object)
{	
	if($debug) 
		event::collision(%this,%object);

	if(getObjectType(%object) == "Player") 
	{
		%item = Item::getItemData(%this);
		%count = Player::getItemCount(%object,%item);
        if(Item::giveItem(%object,%item,Item::getCount(%this))) 
        {
			if(%player.repackEnergy != "")
			{
            %object.repackDamage = %this.repackDamage;
            %object.repackEnergy = %this.repackEnergy;
            %this.repackDamage = "";
            %this.repackEnergy = "";
			}
            Item::playPickupSound(%this);
            Item::respawn(%this);
        }
	}
}


//----------------------------------------------------------------------------
// Default Inventory methods

function Item::onMount(%player,%item)
{
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @", "@%item.description@", "@%item.className@", on player "@ %player @" cl# "@ Player::getclient(%player));

	%client = Player::getClient(%player);
	if(%item.className == Backpack && %client.weaponHelp) 
	{
		bottomprint(%client, "<jc>" @ %item.description , 15);	// 2.2 
	}
	if(%item.className == Weapon || %item.className == Tool) 
	{
		Weapon::CompleteMount(%player,%item); 	
	}	
}

function Item::onUnmount(%player,%item)
{
	if(%item.className == Weapon || %item.className == Tool)	
	{			
		for(%i = 4; %i<8; %i++)
		{
			%class = (Player::getMountedItem(%player,%i)).className;
			if(%class == Weapon || %class == Tool)
				Player::unmountItem(%player,%i);
		}			
	}	
}

function Item::onUse(%player,%item)
{
	if($debug)
		Anni::Echo("?? EVENT use "@ %item @", "@%item.hudIcon@" onto player "@ %player @" cl# "@ Player::getclient(%player));

	Player::mountItem(%player,%item,$DefaultSlot);	
}

function Item::pop(%item)
{
	schedule(%item@".count = false;",2.3,%item);
	schedule(%item@".type = false;",2.3,%item);
 	GameBase::startFadeOut(%item);
	schedule("deleteObject(" @ %item @ ");",2.5, %item);
}

function Item::onDrop(%player,%item)
{
	if($matchStarted) 
	{
		if(%item.className != Armor) 
		{
			if($debug)
				Anni::Echo("Item dropped: ",%player," ",%item);

			%obj = newObject("","Item",%item,1,false);	//false
		if(%player.repackEnergy != "")
		{
		%obj.repackDamage = %player.repackDamage;
		%obj.repackEnergy = %player.repackEnergy;
		%player.repackDamage = "";
		%player.repackEnergy = "";
		}
 	 	  	schedule("Item::Pop(" @ %obj @ ");", $ItemPopTime, %obj);
 	 	 	addToSet("MissionCleanup", %obj);
		
		if (Player::isDead(%player)) 
			GameBase::throw(%obj,%player,10,true);
		else 
		{
			if(GameBase::getEnergy(%player) < 4 || %Player.rThrow && !%player.rThStr)
				GameBase::throw(%obj,%player,5,true);
			else if(GameBase::getEnergy(%player) < 4 || %Player.rThrow && %player.rThStr)
				GameBase::throw(%obj,%player,%player.rThStr,true);	 			
			else GameBase::throw(%obj,%player,15,false);
			Item::playPickupSound(%obj);
		}
			
		Player::decItemCount(%player,%item,1);
		return %obj;
		}
	}
}

function Item::onDeploy(%player,%item,%pos)
{
}


//----------------------------------------------------------------------------
// Flags
//----------------------------------------------------------------------------

function Flag::onUse(%player,%item)
{
	Player::mountItem(%player,%item,$FlagSlot);
}

function ArenaFlag::onUse(%player,%item)
{
	Player::mountItem(%player,%item,$FlagSlot);
}

//----------------------------------------------------------------------------

ItemImageData FlagImage
{
	shapeFile = "flag";
	mountPoint = 2;
	mountOffset = { 0, 0, -0.35 };
	mountRotation = { 0, 0, 0 };
	lightType = 2; // Pulsing
	lightRadius = 4;
	lightTime = 1.5;
	lightColor = { 1, 1, 1};
};

ItemData Flag
{
	description = "Flag";
	shapeFile = "flag";
	//classname = "Flag";
	imageType = FlagImage;
	showInventory = false;
	shadowDetailMask = 4;
	elasticity = 0.2; 
	friction = 1; 
	lightType = 2;   // Pulsing
	lightRadius = 4;
	lightTime = 1.5;
	lightColor = { 1, 1, 1 };
};

ItemData ArenaFlag
{
	description = "Arena Flag";
	shapeFile = "flag";
	imageType = FlagImage;
	showInventory = false;
	shadowDetailMask = 4;
	lightType = 2; // Pulsing
	lightRadius = 4;
	lightTime = 1.5;
	lightColor = { 1, 1, 1 };
};

ItemData RaceFlag
{
	description = "Race Flag";
	shapeFile = "flag";
	imageType = FlagImage;
	showInventory = false;
	shadowDetailMask = 4;
	lightType = 2; // Pulsing
	lightRadius = 4;
	lightTime = 1.5;
	lightColor = { 1, 1, 1 };
};



//----------------------------------------------------------------------------

ItemData Ammo
{
	description = "Ammo";
	showInventory = false;
};


$AmmoMineStrength = 1.0;
MineData DiscAmmoMine 
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Ammo";
	shapeFile = "discammo";
	shadowDetailMask = 4;
	explosionId = grenadeExp;
	explosionRadius = 10.0;
	damageValue = $AmmoMineStrength;	//3.0;	//4,5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 10;
	triggerRadius = 0.5;
	maxDamage = 0.3;
};
function DiscammoMine::onCollision(%this,%object) 
{
	AmmoMine::onCollision(%this,%object);
}

MineData PlasAmmoMine 
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Ammo";
	shapeFile = "plasammo";
	shadowDetailMask = 4;
	explosionId = grenadeExp;
	explosionRadius = 10.0;
	damageValue = $AmmoMineStrength;	// 3.0;	//5
	damageType = $ShrapnelDamageType;
	damageClass = 1; // 0 impact, 1, radius
	kickBackStrength = 10;
	triggerRadius = 0.5;
	maxDamage = 0.3;
};
function PlasammoMine::onCollision(%this,%object) 
{
	AmmoMine::onCollision(%this,%object);
}

MineData ammo1Mine 
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Ammo";
	shapeFile = "ammo1";
	shadowDetailMask = 4;
	explosionId = grenadeExp;
	explosionRadius = 10.0;
	damageValue = $AmmoMineStrength;	// 3.0;	//5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 10;
	triggerRadius = 0.5;
	maxDamage = 0.3;
};
function ammo1Mine::onCollision(%this,%object) 
{
	AmmoMine::onCollision(%this,%object);
}

MineData grenammoMine 
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Ammo";
	shapeFile = "grenammo";
	shadowDetailMask = 4;
	explosionId = grenadeExp;
	explosionRadius = 10.0;
	damageValue = $AmmoMineStrength;	// 3.0;	//5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 10;
	triggerRadius = 0.5;
	maxDamage = 0.3;
};
function grenammoMine::onCollision(%this,%object) 
{
	AmmoMine::onCollision(%this,%object);
}

MineData mortarammoMine 
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Ammo";
	shapeFile = "mortarammo";
	shadowDetailMask = 4;
	explosionId = grenadeExp;
	explosionRadius = 10.0;
	damageValue = $AmmoMineStrength;	// 3.0;	//5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 10;
	triggerRadius = 0.5;
	maxDamage = 0.3;
};
function mortarammoMine::onCollision(%this,%object) 
{
	AmmoMine::onCollision(%this,%object);
}

MineData rocketMine 
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Ammo";
	shapeFile = "rocket";
	shadowDetailMask = 4;
	explosionId = grenadeExp;
	explosionRadius = 10.0;
	damageValue = $AmmoMineStrength;	// 3.0;	//5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 10;
	triggerRadius = 0.5;
	maxDamage = 0.3;
};
function rocketMine::onCollision(%this,%object) 
{
	AmmoMine::onCollision(%this,%object);
}

MineData ammo2Mine 
{	
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Ammo";
	shapeFile = "ammo2";
	shadowDetailMask = 4;
	explosionId = grenadeExp;
	explosionRadius = 10.0;
	damageValue = $AmmoMineStrength;	// 3.0;	//5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 10;
	triggerRadius = 0.5;
	maxDamage = 0.3;	
};
function Ammo2Mine::onCollision(%this,%object) 
{
	AmmoMine::onCollision(%this,%object);
}
MineData mortarMine 
{	
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Mine";
	description = "Ammo";
	shapeFile = "mortar";
	shadowDetailMask = 4;
	explosionId = grenadeExp;
	explosionRadius = 10.0;
	damageValue = $AmmoMineStrength;	// 3.0;	//5
	damageType = $ShrapnelDamageType;
	kickBackStrength = 10;
	triggerRadius = 0.5;
	maxDamage = 0.3;	
};
function mortarMine::onCollision(%this,%object) 
{
	AmmoMine::onCollision(%this,%object);
}

function Mine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object) 
{
	if($debug::Damage)
	{
		Anni::Echo("Mine::onDamage("@%this@", "@%type@", "@%value@", "@%pos@", "@%vec@", "@%mom@", "@%object@" )");
	}
	%damageLevel = GameBase::getDamageLevel(%this);	
	%data = GameBase::getDataName(%this);
	if(%value <= 0)return;

	if(%damageLevel + %value > %data.maxDamage && %this.count)
	{
		%type = %this.type;
		if(%type != PlasmaAmmo)	// && %type != PhaseAmmo)
		{
			%count = %this.count;
			if(%count > 3)
				%count = 3;
				
			%bomb = %type@"Bomb";
			for(%i = 0; %i < %count; %i++)
			{
				%obj = newObject("","Mine",%bomb);
				addToSet("MissionCleanup", %obj);
				GameBase::throw(%obj,%this,100 ,true);	
				schedule("GameBase::setDamageLevel("@%obj@", 20);",1,%obj);		
			}		
				
		}
	
	}			
	GameBase::setDamageLevel(%this,%damageLevel + %value);	
}

function AmmoMine::onCollision(%this,%object) 
{	
	if($debug) Anni::Echo("Ammo collision "@%this.type@", "@%this.count);
	//Anni::Echo("proj ="@%this.type.imageType);
	if(getObjectType(%object) == "Player" && !Player::isDead(%object)) 
	{	
		%item = %this.type;
		%count = %this.count;
		if(Item::giveItem(%object,%item,%count)) 
		{
			Item::playPickupSound(%this);
			deleteObject(%this);
		}
	}	
	else
	{
		//Anni::Echo("mine colllision with "@getObjectType(%object));	
	}	
}



function Mine::onAdd(%this)
{
	Mine::deployCheck(%this);
	$Ammo::count += 1;
	if(!$dedicated)
		bottomprint(2049,"Ammo count= "@$Ammo::count@" Item count= "@$item::count@" Miner count= "@$mine::count);
}

function Mine::onRemove(%this)
{
	$Ammo::count -= 1;
	if(!$dedicated)
		bottomprint(2049,"Ammo count= "@$Ammo::count@" Item count= "@$item::count@" Miner count= "@$mine::count);
}

function Mine::deployCheck(%this)
{
	if(GameBase::isAtRest(%this)) 
	{
		%set = newObject("set",SimSet);
		if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),1,1,1,0)) 
		{
			%data = GameBase::getDataName(%this);
			if(%data == GameBase::getDataName(Group::getObject(%set,0)))
				GameBase::setDamageLevel(%this, %data.maxDamage);
		}
		deleteObject(%set);
	}
	else 
		schedule("Mine::deployCheck(" @ %this @ ");", 3, %this);
}


function Ammo::onDrop(%player,%item)
{
	if($matchStarted)
	{
		%clientId = Player::getClient(%player);
		%count = Player::getItemCount(%player,%item);
		%delta = $SellAmmo[%item];
		if($debug)
			Anni::Echo("Ammo::onDrop(%player,%item,%count) ",%player,%item,%count);
		if(%count <= %delta)
		{
			if(%item == BulletAmmo || (Player::getMountedItem(%player,$WeaponSlot)).imageType.ammoType != %item)
				%delta = %count;
			else 
				%delta = %count - 1;
		}
		if(%delta > 0)
		{
			if(%clientId.inArena || $TALT::Active) // no exploading ammo for arena and lt players
            {
                %obj = newObject("","Item",%item,%delta,false);
                schedule("Item::Pop(" @ %obj @ ");", $ItemPopTime, %obj);

                addToSet("MissionCleanup", %obj);
                GameBase::throw(%obj,%player,20,false);
                Item::playPickupSound(%obj);
                Player::decItemCount(%player,%item,%delta);
				return;
            }
			else if($Annihilation::ExplodingAmmo == true) //added a toggle 
			{
				%client = Player::getClient(%player);
				%shape = %item.shapeFile;
				%obj = newObject("","Mine",%shape@"Mine");
				GameBase::setTeam (%obj,GameBase::getTeam(%client));
				GameBase::setMapName(%obj,%item);						
				schedule("Item::Pop(" @ %obj @ ");", $ItemPopTime, %obj);
				addToSet("MissionCleanup", %obj);
				GameBase::throw(%obj,%player,5,false);			

				%obj.count = %delta;
				%obj.type = %item;
			
				Item::playPickupSound(%obj);
				Player::decItemCount(%player,%item,%delta);	
			}
			else
            {
                %obj = newObject("","Item",%item,%delta,false);
                schedule("Item::Pop(" @ %obj @ ");", $ItemPopTime, %obj);

                addToSet("MissionCleanup", %obj);
                GameBase::throw(%obj,%player,20,false);
                Item::playPickupSound(%obj);
                Player::decItemCount(%player,%item,%delta);
				return;
            }
			
			if(%client.InvConnect)
			{	
				%client.ThrowWait = true;
				schedule(%client@".ThrowWait = false;",0.5,%client);		
			}
					
		}
	}
}	


function CountObjects(%set,%name,%num) 
{
	%count = 0;
	for(%i=0;%i<%num;%i++)
	{
		%obj=Group::getObject(%set,%i);
		if(GameBase::getDataName(Group::getObject(%set,%i)) == %name) 
			%count++;
	}
	return %count;
}

function checkDeployArea(%client,%pos)
{
	%set=newObject("set",SimSet);
	%num=containerBoxFillSet(%set,$StaticObjectType | $ItemObjectType | $SimPlayerObjectType,%pos,1,1,1,1);
	if(!%num) 
	{
		deleteObject(%set);
		return 1;
	}
	else if(%num == 1 && getObjectType(Group::getObject(%set,0)) == "Player") 
	{
		%obj = Group::getObject(%set,0);
		if(Player::getClient(%obj) == %client)
			Client::sendMessage(%client,0,"Unable to deploy - You're in the way ~wC_BuySell.wav");
		else
			Client::sendMessage(%client,0,"Unable to deploy - Player in the way");
		        Client::sendMessage(%client,0,"~wC_BuySell.wav");
	}
	else
		Client::sendMessage(%client,0,"Unable to deploy - Item in the way");
	        Client::sendMessage(%client,0,"~wC_BuySell.wav");

	deleteObject(%set);
	return 0;
}


//adding a new check specifically for bigcrates to make their deploy easier -death666
function checkDeployAreaCrate(%client,%pos)
{
	%set=newObject("set",SimSet);
	%num=containerBoxFillSet(%set,$SimPlayerObjectType,%pos,3.5,1.5,3.0,3.5);
	if(!%num) 
	{
		deleteObject(%set);
		return 1;
	}
	else if(%num == 1 && getObjectType(Group::getObject(%set,0)) == "Player") 
	{
		%obj = Group::getObject(%set,0);
		if(Player::getClient(%obj) == %client)
			Client::sendMessage(%client,0,"Unable to deploy - You're in the way ~wC_BuySell.wav");
		else
			Client::sendMessage(%client,0,"Unable to deploy - Player in the way");
		        Client::sendMessage(%client,0,"~wC_BuySell.wav");
	}
	else
		Client::sendMessage(%client,0,"Unable to deploy - Item in the way");
	        Client::sendMessage(%client,0,"~wC_BuySell.wav");

	deleteObject(%set);
	return 0;
}

//	to allow for deploying some items on platforms
function checkInvDeployArea(%client,%pos)
{
	if($build)return 1;
	
	%set=newObject("set",SimSet);
	%num=containerBoxFillSet(%set,$StaticObjectType |$ItemObjectType | $SimPlayerObjectType,%pos,1,1,1,1);
	if(!%num) 
	{
		deleteObject(%set);
		return 1;
	}
	else if(%num == 1) 
	{
		%obj = Group::getObject(%set,0);
		if(getObjectType(Group::getObject(%set,0)) == "Player")
		{		
			if(Player::getClient(%obj) == %client)
				Client::sendMessage(%client,0,"Unable to deploy - You're in the way ~wC_BuySell.wav");
			else
				Client::sendMessage(%client,0,"Unable to deploy - Player in the way");
			        Client::sendMessage(%client,0,"~wC_BuySell.wav");
		}
		else
		{
			if(GameBase::getDataName(%obj) == "DeployablePlatform")
			{
				deleteObject(%set);
				return 1;
			}
			else 
				Client::sendMessage(%client,0,"Unable to deploy - Item in the way");
			        Client::sendMessage(%client,0,"~wC_BuySell.wav");
		}
	}
	else
		Client::sendMessage(%client,0,"Unable to deploy - Item in the way");
	        Client::sendMessage(%client,0,"~wC_BuySell.wav");

	deleteObject(%set);
	return 0;
}

//----------------------------------------------------------------------------
function Item::deployShape(%player,%name,%shape,%item)
{
	%client = Player::getClient(%player);
	if(($TeamItemCount[GameBase::getTeam(%player) @ %item] < $TeamItemMax[%item]) || $build)
	{
		if(GameBase::getLOSInfo(%player,5))
		{
			// GetLOSInfo sets the following globals:
			// 	los::position
			// 	los::normal
			// 	los::object				
			%obj = getObjectType($los::object);
			if(%obj == "SimTerrain" || %obj == "InteriorShape")
			{
				if(Vector::dot($los::normal,"0 0 1") > 0.7)
				{
					if(checkDeployArea(%client,$los::position))
					{
						%sensor = newObject("","Sensor",%shape,true);
						%sensor.deployer = %client; 
						if(%player.repackEnergy != "")
						{
						GameBase::setDamageLevel(%sensor, %player.repackDamage);
						GameBase::setEnergy(%sensor, %player.repackEnergy);
						%player.repackDamage = "";
						%player.repackEnergy = "";
						}
						
						%sensor.cloakable = true;
						addToSet("MissionCleanup/deployed/sensor", %sensor);
						GameBase::setTeam(%sensor,GameBase::getTeam(%player));
						GameBase::setPosition(%sensor,$los::position);
						Gamebase::setMapName(%sensor,%name);
						Client::sendMessage(%client,0,%item.description @ " deployed");
						playSound(SoundPickupBackpack,$los::position);
						if(!$build)
							Anni::Echo("MSG: ",%client," deployed a ",%name);
	
						%obj = $los::object;
						if(%obj.inmotion == true)	 
						{ 
							schedule("replaceSensor("@%sensor@");",5,%sensor);
						}	
							
						return true;
					}
				}
				else 
					Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
				        Client::sendMessage(%client,0,"~wC_BuySell.wav");
			}
			else 
				Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");
			        Client::sendMessage(%client,0,"~wC_BuySell.wav");
		}
		else 
			Client::sendMessage(%client,0,"Deploy position out of range.");
		        Client::sendMessage(%client,0,"~wC_BuySell.wav");
	}
	else
		Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %name @ "s");
	        Client::sendMessage(%client,0,"~wC_BuySell.wav");
	return false;
}

function replaceSensor(%sensor)
{
	%pos = GameBase::getPosition(%sensor);
	if(%pos != "0 0 0")
	{
		%rot =GameBase::getRotation(%sensor);
		%type = getObjectType(%sensor);
		%shape = GameBase::getDataName(%sensor);
		%newsensor = newObject("",%type,%shape,true);
		
		%newsensor.cloakable = %sensor.cloakable;
		addToSet("MissionCleanup/deployed/sensor", %newsensor);
		GameBase::setTeam(%newsensor,GameBase::getTeam(%sensor));
		GameBase::setPosition(%newsensor,%pos);
		GameBase::setRotation(%newsensor,%rot);
		Gamebase::setMapName(%newsensor,gamebase::getmapname(%sensor));
		deleteObject(%sensor);
		Anni::Echo("sensor replaced");

	}
	
	
}



//----------------------------------------------------------------------------

function remoteGiveAll(%clientId)
{
	return;

	if($TestCheats)
	{
		//Player::setItemCount(%clientId,Blaster,1);
		Player::setItemCount(%clientId,Chaingun,1);
		Player::setItemCount(%clientId,PlasmaGun,1);
		Player::setItemCount(%clientId,GrenadeLauncher,1);
		Player::setItemCount(%clientId,DiscLauncher,1);
		//Player::setItemCount(%clientId,LaserRifle,1);
		Player::setItemCount(%clientId,EnergyRifle,1);
		Player::setItemCount(%clientId,TargetingLaser,1);
		Player::setItemCount(%clientId,Mortar,1);

		Player::setItemCount(%clientId,BulletAmmo,200);
		Player::setItemCount(%clientId,PlasmaAmmo,200);
		Player::setItemCount(%clientId,GrenadeAmmo,200);
		Player::setItemCount(%clientId,DiscAmmo,200);
		Player::setItemCount(%clientId,MortarAmmo,200);

		Player::setItemCount(%clientId,Grenade, 200);
		Player::setItemCount(%clientId,MineAmmo, 200);
		Player::setItemCount(%clientId,Beacon, 200);

		Player::setItemCount(%clientId,RepairKit,200);
	}
	else if($ServerCheats)
	{
		%armor = Player::getArmor(%clientId);
		Player::setItemCount(%clientId,BulletAmmo,$ItemMax[%armor, BulletAmmo]);
		Player::setItemCount(%clientId,PlasmaAmmo,$ItemMax[%armor, PlasmaAmmo]);
		Player::setItemCount(%clientId,GrenadeAmmo,$ItemMax[%armor, GrenadeAmmo]);
		Player::setItemCount(%clientId,DiscAmmo,$ItemMax[%armor, DiscAmmo]);
		Player::setItemCount(%clientId,MortarAmmo,$ItemMax[%armor, MortarAmmo]);

		Player::setItemCount(%clientId,Grenade, $ItemMax[%armor, Grenade]);
		Player::setItemCount(%clientId,MineAmmo,$ItemMax[%armor, MineAmmo]);
		Player::setItemCount(%clientId,Beacon,$ItemMax[%armor, Beacon]);

		Player::setItemCount(%clientId,RepairKit,1);
	}
}


//----------------------------------------------------------------------------


function checkMax(%client,%armor)
{
 	%weaponflag = 0;
	%numweapon = Player::getItemClassCount(%client,"Weapon");
	if(%numweapon > $MaxWeapons[%armor])
	{
		%weaponflag = %numweapon - $MaxWeapons[%armor];
	}
	%max = getNumItems();
	for(%i = 0; %i < %max; %i = %i + 1)
	{
		
		%item = getItemData(%i);
		//Anni::Echo("checkmax "@%item);
		%maxnum = $ItemMax[%armor, %item];
		if(%maxnum != "")
		{
			%numsell = 0;
			%count = Player::getItemCount(%client,%item);
			if(%count > %maxnum)
			{
				%numsell = %count - %maxnum;
			}
			if(%count > 0 && %weaponflag && %item.className == Weapon)
			{
				%numsell = 1;
				%weaponflag = %weaponflag - 1;
			}
			if(%numsell > 0)
			{
				Client::sendMessage(%client,0,"SOLD " @ %numsell @ " " @ %item);
				teamEnergyBuySell(Client::getOwnedObject(%client),(%item.price * %numsell));
				Player::setItemCount(%client, %item, %count - %numsell);
				updateBuyingList(%client);
			}
		}
	}
}

function SellLoadout(%client) //new function for armor 
{
	%max = getNumItems();
	for(%i = 0; %i < %max; %i = %i + 1)
	{
		%item = getItemData(%i);
		%count = Player::getItemCount(%client,%item);
		if(%count > 0 && %item.className != Armor && %item != flag)
		{
			if($debug)
				echo(%item@" <<<<<<< Selling this thing <<<<<<<<<< function SellLoadout()");
			teamEnergyBuySell(Client::getOwnedObject(%client),(%item.price));
			Player::setItemCount(%client, %item, 0);
			updateBuyingList(%client);
		}
	}
}

function checkPlayerCash(%client)
{
	%team = Client::getTeam(%client);
	if($TeamEnergy[%team] != "Infinite")
	{
		if(%client.teamEnergy > ($InitialPlayerEnergy * -1) )
		{
			if(%client.teamEnergy >= 0)
				%diff = $InitialPlayerEnergy;
			else 
				%diff = $InitialPlayerEnergy + %client.teamEnergy;
			$TeamEnergy[%team] -= %diff;
		}
	}
}	

function Annihilation::incItemCount(%client,%item,%count)
{
	if($debug)
		echo("incItemCount "@%client@", "@%item@", "@%count);		
	if(!%count)
		%count = 1;
	player::incItemCount(%client,%item,%count);	
}

function Annihilation::decItemCount(%client,%item,%count)
{
	if($debug)
		echo("decItemCount "@%client@", "@%item@", "@%count);			
	if(!%count)
		%count = 1;		
	player::decItemCount(%client,%item,%count);	
}

function Annihilation::setItemCount(%client,%item,%count)
{
	if($debug)
		echo("setItemCount "@%client@", "@%item@", "@%count);			
	player::setItemCount(%client,%item,%count);	
}

