
function MobileStation::onActivate(%this) 
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
			if(%dName == InventoryStation)
				CheckStationEject(%obj,%this,%obj.stationAccess,($Annihilation::StationTime / 5));
		}
	}
	else 
		GameBase::setActive(%this,false);
}

function MobileStation::onEndSequence(%this,%thread) 
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
		else
			Client::setInventoryText(%client, "<f1><jc>T O T A L \n<f3>A N N I H I L A T I O N"); 
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

function MobileStation::deploy(%this) 
{
	GameBase::playSequence(%this,0,"deploy");
}

function MobileStation::onDeactivate(%this) 
{
	//echo("Dectivate " @ %this);
	%obj = %this.lastPlayer;
	if(%this == %obj.inStation)
		%obj.inStation = false;
	GameBase::stopSequence(%this,2);
	GameBase::setSequenceDirection(%this,1,0);
}

function MobileStation::onEnabled(%this) 
{
	if(GameBase::isPowered(%this)) 
	{
		GameBase::playSequence(%this,0,"power");
		GameBase::playSequence(%this,1);
	}
}

function MobileStation::onDisabled(%this) 
{
	Station::weaponCheck(%this);
//	GameBase::stopSequence(%this,0);
	GameBase::setSequenceDirection(%this,1,0);
	GameBase::pauseSequence(%this,1);
	GameBase::stopSequence(%this,2);
	Station::checkTarget(%this);
}

function MobileStation::onDestroyed(%this) 
{
	MobileStation::onDisabled(%this);
	%stationName = GameBase::getDataName(%this);
	%this.cloakable = "";
	%this.nuetron = "";
	if(%stationName == MobileInventory) $TeamItemCount[GameBase::getTeam(%this) @ "MobileInventoryPack"]--;
	//else if( %stationName == DeployableAmmoStation) $TeamItemCount[GameBase::getTeam(%this) @ "DeployableAmmoPack"]--;
	if(!$NoCalcDamage)
		calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 2, 0.30, 0.1, 200, 100);
}

function MobileStation::onCollision(%this, %object) 
{	
	if($debug) 
		event::collision(%this,%object);

	if(%this.cloaked > 0 && getObjectType(%object) == "Player"){
		GameBase::startFadein(%this);	
		%this.cloaked = "";
		}
	if(%this.target == ""){
		%obj = getObjectType(%object);

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
					Client::sendMessage(%client,0,"Unit is disabled");
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