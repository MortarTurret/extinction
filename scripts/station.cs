




function Station::onActivate(%this)
{
	//echo("Activate " @ %this);
	%obj = Station::getTarget(%this);
	if(%obj != -1) 
	{
		GameBase::playSequence(%this,1,"activate");
		GameBase::setSequenceDirection(%this,1,1);
		%this.lastPlayer = %obj;
		%obj.inStation = %this;
		if($Annihilation::StationTime) 
		{
			if($Annihilation::StationTime < 10) 
				$Annihilation::StationTime = 10;
			else if($Annihilation::StationTime > 60)
				$Annihilation::StationTime = 60;
			%obj.stationAccess++;
			%dName = GameBase::getdataName(%this);
			if(%dName == InventoryStation && !$Annihilation::ExtendedInvs)
				CheckStationEject(%obj,%this,%obj.stationAccess,($Annihilation::StationTime / 5));
		}
	}
	else 
		GameBase::setActive(%this,false);
}

function Station::onDeactivate(%this)
{
	//echo("Dectivate " @ %this);
	%obj = %this.lastPlayer;
	if(%this == %obj.inStation)
		%obj.inStation = false;
	GameBase::stopSequence(%this,2);
	GameBase::setSequenceDirection(%this,1,0);
}

function Station::onEndSequence(%this,%thread)
{
	//echo("End sequence " @ %this);
 	if(%thread == 1 && GameBase::isActive(%this)) 
 	{
		GameBase::playSequence(%this,2,"use");
		return true;
	}
	%client = %this.target;
	if(%client == "") 
	{
		%player = Station::getTarget(%this);
		%client = Player::getClient(%player);
	}
	if(%client != "") 
	{
		if(Client::getGuiMode(%client) != 1)
			Client::setGuiMode(%client,1);
		
		%team = Client::getTeam(%client);
		if($TeamEnergy[%team] != "Infinite") 
		{
			if(%this.clTeamEnergy != %client.TeamEnergy) 
			{
				if(%client.teamEnergy < 0)
					Client::sendMessage(%client,0,"Your total mission purchases have come to " @ (%client.teamEnergy * -1) @ ".");
				else
					Client::sendMessage(%client,0,"You have increased the Team Energy by " @ %client.teamEnergy @ ".");
			}
			if((%client.teamEnergy -%client.EnergyWarning < $TeammateSpending) && ($TeammateSpending != 0) && !$TeamEnergyCheat) 
			{
				TeamMessages(0, %team, "Teammate " @ Client::getName(%client) @ " has spent " @ (%client.teamEnergy *-1) @ " of the TeamEnergy");
				%client.EnergyWarning = %client.teamEnergy;
			}
			if($TeamEnergy[%team] < $WarnEnergyLow)
				TeamMessages(0, %team, "TeamEnergy Low: " @ $TeamEnergy[%team]);
		}
	}
	if(%this.target != "") 
	{
		(Client::getOwnedObject(%this.target)).Station = "";
		%this.target = "";
	}
	if(GameBase::getDataName(%this) == VehicleStation && %this.vehiclePad.busy < getSimTime())
		VehiclePad::checkSeq(%this.vehiclePad, %this);
	%this.clTeamEnergy = "";
	return false;
}

function Station::onPower(%this,%power,%generator)
{
	if(%power) 
	{
		GameBase::playSequence(%this,0,"power");
		GameBase::playSequence(%this,1);
		
		ZappyPowerSwitch(%this,true);					
	}
	else // 7
	{
//		Station::weaponCheck(%this);
		GameBase::stopSequence(%this,0);
//		GameBase::setSequenceDirection(%this,1,0);		
		GameBase::pauseSequence(%this,1);
		GameBase::pauseSequence(%this,2);
		Station::checkTarget(%this);
		
		ZappyPowerSwitch(%this,false);			
	}
}

function Station::onEnabled(%this)
{
	if(GameBase::isPowered(%this)) 
	{
		GameBase::playSequence(%this,0,"power");
		GameBase::playSequence(%this,1);
		
		ZappyPowerSwitch(%this,true);			
	}
}

function Station::checkTarget(%this)
{
	if(%this.target) 
	{
		Client::setGuiMode(%this.target,1);
		GameBase::setActive(%this,false);
	}
}

function Station::onDisabled(%this)
{
	Station::weaponCheck(%this);
	GameBase::stopSequence(%this,0);
	GameBase::setSequenceDirection(%this,1,0);
	GameBase::pauseSequence(%this,1);
	GameBase::stopSequence(%this,2);
	Station::checkTarget(%this);
	
	ZappyPowerSwitch(%this,false);	
}

function Station::onDestroyed(%this)
{
	Station::weaponCheck(%this);
	StaticShape::objectiveDestroyed(%this);
	GameBase::stopSequence(%this,0);
	GameBase::stopSequence(%this,1);
	GameBase::stopSequence(%this,2);
	Station::checkTarget(%this);
	if(!$NoCalcDamage)
		calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 2, 0.40, 0.1, 250, 100);
		
	ZappyPowerSwitch(%this,false);				
}

function Station::weaponCheck(%this)
{
	if(%this.lastPlayer != "") 
	{
		%player = %this.lastPlayer;
		%player.Station = "";
		if(Player::getMountedItem(%player,$WeaponSlot) == -1)
		{
			if(%player.lastWeapon != "") 
			{
				Player::useItem(%player,%player.lastWeapon);
				%player.lastWeapon = "";
			}
		}
		%this.lastPlayer = "";
	}
}

function Station::getTarget(%this)
{
	if(GameBase::getLOSInfo(%this,1.5,"0 0 3.14")) 
	{
		// GetLOSInfo sets the following globals:
		// 	los::position
		// 	los::normal
		// 	los::object
		%obj = getObjectType($los::object);
		if(%obj == "Player") 
		{
			%player = $los::object;
			%client = Player::getClient(%player);
			if( Player::isAiControlled( %player ) != "True" ) 
			{
				if ( (GameBase::getTeam(%player) == GameBase::getTeam(%this) || GameBase::getTeam(%this) == -1) && %client.isSpy != true )
				{
					return %player;
				}
				else
				{
					%curTime = getSimTime();
					if(!Player::isDead(%player) && %curTime - %player.stationDeniedStamp > 3.5 && GameBase::getDamageState(%this) == "Enabled") 
					{
						%player.stationDeniedStamp = %curTime;
						Client::sendMessage(%client,0,"--ACCESS DENIED-- Wrong Team ~waccess_denied.wav");
					}
				}
			}
		}
	}
	return -1;
}	

function Station::onCollision(%this, %object)
{	
	if($debug) 
		event::collision(%this,%object);
	
		        if(Player::isAIControlled(%object))
				{
					if(GameBase::getDataName(%this) == VehicleStation)
					{					
					%player = Client::getControlObject(%object);
					// %player = Client::getOwnedObject(%id);
					
					// messageall(1, "Bot Touched A Vehicle Station.");
					// %player = Client::getOwnedObject(%object);
					%curDie = radnomItems(4, $PlayerAnim::DieHead,$PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
					Player::setAnimation(%object, %curDie);
					playNextAnim(%object);
					Player::kill(%object);
					}
				}

	if(%this.cloaked > 0 && getObjectType(%object) == "Player")
	{
		GameBase::startFadein(%this);
		%this.cloaked = "";
	}
	if(%this.target == "")
	{
		%obj = getObjectType(%object);
		
		if(Player::isDead(%obj))
			return;
		if(%obj == "Player" && isPlayerBusy(%object) == 0) 
		{
			%client = Player::getClient(%object);
			if((GameBase::getTeam(%object) == GameBase::getTeam(%this) || GameBase::getTeam(%this) == -1) && %client.isSpy != true) 
			{
				if(GameBase::getDamageState(%this) == "Enabled") 
				{
					if(GameBase::isPowered(%this)) 
					{ 
						if(%this.enterTime == "")
							%this.enterTime = getSimTime();
						GameBase::setActive(%this,true);
					}
					else 
						Client::sendMessage(%client,0,"Unit is not powered");
				}
				else 
				{
					Client::sendMessage(%client,0,"Unit is disabled");
					GameBase::playSound(%client, Fneedrep, 0);
				}
			}
			else if(Station::getTarget(%this) == %object)
			{
				%curTime = getSimTime();
				if(%curTime - %object.stationDeniedStamp > 3.5 && GameBase::getDamageState(%this) == "Enabled") 
				{
					Client::clearItemShopping(%client);
					Station::onDeactivate(%this);
					Station::onEndSequence(%this,1);
					if(Client::getGuiMode(%client) != 1)
						Client::setGuiMode(%client,1);
					%object.stationDeniedStamp = %curTime;
					Client::sendMessage(%client,0,"--ACCESS DENIED-- Wrong Team ~waccess_denied.wav");
				}
			}
		}
	}
}

function Station::itemsToResupply(%player) 
{
	%cnt = 0;

	for(%i = 0; %i < $AmmoCount; %i++)
		%cnt = %cnt + AmmoStation::resupply(%player, $Ammo_Weapon[%i], $Ammo_Ammo[%i], $Ammo_Count[%i]);

	return %cnt;
}

function CheckStationEject(%player, %station, %stat, %count)
{
	if($Annihilation::StationTime)
	{
		if((%player.inStation) && (%player.inStation == %station) && (%stat == %player.stationAccess))
		{
			if(%count)
			{
				if(%count == 1)
					Client::sendMessage(GameBase::getOwnerClient(%player),1,"You will be Ejected from the station in 5 Seconds if you don't hurry your ass up!~waccess_denied.wav");
				schedule("CheckStationEject(" @ %player @ "," @ %station @ "," @ %stat @ "," @ %count - 1 @ ");",5,%player);
			}
			else
			{
				Client::sendMessage(GameBase::getOwnerClient(%player),1,"You have been Ejected from the Station, use favorites!~waccess_denied.wav");
				StationEjectPlayer(%station, %player);
			}
		}
	}
}

function StationEjectPlayer(%station, %player)
{
	%rot = GameBase::getRotation(%station);
	%rad = getWord(%rot, 2);
	%x = (-1) * (Sin(%rad));
	%y = Cos(%rad);
	%dir = %x @ " " @ %y @ " 0";
	%force = DotProd(Vector::neg(%dir),10);
	%x = getWord(%force, 0);
	%y = getWord(%force, 1);
	%vel = %x @ " " @ %y @ " " @ 15;
	Item::setVelocity(%player,%vel);
}

function DotProd(%vec, %scalar)
{
	%return = Vector::dot(%vec,%scalar @ " 0 0") @ " " @ Vector::dot(%vec,"0 " @ %scalar @ " 0") @ " " @ Vector::dot(%vec,"0 0 " @ %scalar);
	return %return;
}	

function Sin(%theta)
{
	return (%theta - (pow(%theta,3)/6) + (pow(%theta,5)/120) - (pow(%theta,7)/5040) + (pow(%theta,9)/362880) - (pow(%theta,11)/39916800));
}

function Cos(%theta)
{
	return (1 - (pow(%theta,2)/2) + (pow(%theta,4)/24) - (pow(%theta,6)/720) + (pow(%theta,8)/40320) - (pow(%theta,10)/3628800));
}

function resupply(%this)
{
	if(GameBase::isActive(%this)) 
	{
		%player = Station::getTarget(%this);
		if(%player != -1) 
		{
			// Hardcoded here for the ammo types
			%cnt = Station::itemsToResupply(%player);
			if(getSimTime() - %this.enterTime > 11)
				%cnt = 0;
			%client = Player::getClient(%player);
			if(%cnt != 0) 
			{
				updateBuyingList(%client);
				return 1;
			}
			Client::sendMessage(%client,0,"Resupply Complete");
			return 0;
		}
	}
	return 0;
}

function setupShoppingList(%client,%station,%ListType)
{
	Client::clearItemShopping(%client);
	%armor = Player::getarmor(%client);
	%max = getNumItems();
	if(%ListType == "InvList") 
	{
		for(%i = 0; %i < %max; %i = %i + 1) 
		{
			%item = getItemData(%i);
			if($Annihilation::ShoppingList)
			{
				if($InvList[%item] != "" && $InvList[%item] && !%station.dontSell[%item] && $ItemMax[%armor,%item] > 0) 
					Client::setItemShopping(%client, %item);
				else if(%item.className == Armor && !%station.dontSell[%item])
					Client::setItemShopping(%client, %item);
			}
			else
			{
				if($InvList[%item] != "" && $InvList[%item] && !%station.dontSell[%item]) 
					Client::setItemShopping(%client, %item);
				else if(%item.className == Armor && !%station.dontSell[%item])
					Client::setItemShopping(%client, %item);
			}
		}
	}
	else if(%ListType == "MobileInvList") 
	{
		for(%i = 0; %i < %max; %i = %i + 1) 
		{
			%item = getItemData(%i);
			if($Annihilation::ShoppingList)
			{
				if($InvList[%item] != "" && $InvList[%item] && !%station.dontSell[%item] && $ItemMax[%armor,%item] > 0) 
					Client::setItemShopping(%client, %item);
				else if(%item.className == Armor && !%station.dontSell[%item])
					Client::setItemShopping(%client, %item);
			}
			else
			{
				if($InvList[%item] != "" && $InvList[%item] && !%station.dontSell[%item]) 
					Client::setItemShopping(%client, %item);
				else if(%item.className == Armor && !%station.dontSell[%item])
					Client::setItemShopping(%client, %item);
			}
		}
	}
	else if(%ListType == "RemoteInvList") 
	{
		for(%i = 0; %i < %max; %i = %i + 1) 
		{
			%item = getItemData(%i);
			if($Annihilation::ShoppingList)
			{
				if($RemoteInvList[%item] != "" && $RemoteInvList[%item] && !%station.dontSell[%item] && $ItemMax[%armor,%item] > 0) 
					Client::setItemShopping(%client, %item);
			}
			else
			{
				if($RemoteInvList[%item] != "" && $RemoteInvList[%item] && !%station.dontSell[%item]) 
					Client::setItemShopping(%client, %item);
			}
		}
	}
	else if(%ListType == "VehicleInvList")
	{
		for(%i = 0; %i < %max; %i = %i + 1) 
		{
			%item = getItemData(%i);
			if($VehicleInvList[%item] != "" && $VehicleInvList[%item] && !%station.dontSell[%item]) 
				Client::setItemShopping(%client, %item);
		}
	}
	else
		return;
}

function updateBuyingList(%client)
{
	Client::clearItemBuying(%client);
	%station = (Client::getOwnedObject(%client)).Station;
	%stationName = GameBase::getDataName(%station);
	if(%stationName == DeployableInvStation || %stationName == DeployableAmmoStation) 
	{
		%energy = %station.Energy;
		Client::setInventoryText(%client, "<f1><jc>STATION ENERGY: " @ %energy );
	}
	else 
	{
		%energy = $TeamEnergy[Client::getTeam(%client)];
		if(%energy == "Infinite" && $TALT::Active == false) 
		Client::setInventoryText(%client, "<f1><jc>T O T A L \n<f3>A N N I H I L A T I O N"); 

		else			
			Client::setInventoryText(%client, "<f1><jc>TEAM ENERGY: " @ %energy);
	}
	%armor = Player::getArmor(%client);
	%max = getNumItems();
	for(%i = 0; %i < %max; %i++) 
	{
		%item = getItemData(%i);
		if(!%item.showInventory)
			continue;
		if($ItemMax[%armor, %item] != "" && Client::isItemShoppingOn(%client,%i)) 
		{
			%extraAmmo = 0;
			if(Player::getMountedItem(%client,$BackpackSlot) == ammopack)
				%extraAmmo = $AmmoPackMax[%item];
			if($ItemMax[%armor, %item] != "" && $ItemMax[%armor, %item] + %extraAmmo > Player::getItemCount(%client,%item)) 
			{
				if(%energy >= %item.price ) 
				{
					if($ItemMax[%armor,%item] > 0)
					{
						if(%item.className == Weapon) 
						{
							if(Player::getItemClassCount(%client,"Weapon") < $MaxWeapons[%armor]) 
								Client::setItemBuying(%client, %item);
						}
						else 
						{ 
							if($TeamItemMax[%item] != "") 
							{
								if($TeamItemCount[GameBase::getTeam(%client) @ %item] < $TeamItemMax[%item] || $build)
									Client::setItemBuying(%client, %item);
							}
							else
								Client::setItemBuying(%client, %item);
						}
					}
				}
			}
		}
		else if(%item.className == Armor && %item != $ArmorName[%armor] && Client::isItemShoppingOn(%client,%i)) 
			Client::setItemBuying(%client, %item);
		else if(%item.className == Vehicle && ($TeamItemCount[client::getTeam(%client) @ %item] < $TeamItemMax[%item] || $build) && Client::isItemShoppingOn(%client,%i))
			Client::setItemBuying(%client, %item);
	}
}
