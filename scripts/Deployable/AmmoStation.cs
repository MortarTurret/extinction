
StaticShapeData AmmoStation
{
	description = "Ammo Supply Unit";
	shapeFile = "ammounit";
	className = "Station";
	visibleToSensor = true;
	shieldShapeName = "shield";
	sequenceSound[0] = { "activate", SoundActivateAmmoStation };
	sequenceSound[1] = { "power", SoundAmmoStationPower };
	sequenceSound[2] = { "use", SoundUseAmmoStation };
	maxDamage = 1.0;
	debrisId = flashDebrisLarge;
	mapFilter = 4;
	mapIcon = "M_station";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	explosionId = flashExpLarge;
};

function AmmoStation::onEndSequence(%this,%thread)
{
	//echo("End Seq ",%thread);
	%player = Station::getTarget(%this);
	if(%this.clTeamEnergy == "")
		%this.clTeamEnergy = (Player::getClient(%player)).TeamEnergy;
	if(Station::onEndSequence(%this,%thread)) 
	{	 
		%weapon = Player::getMountedItem(%player,$WeaponSlot);
		%client = Player::getClient(%player);
		%client.ListType = "";
		AmmoStation::onResupply(%this);
	}
}

function AmmoStation::onResupply(%this)
{
	if(GameBase::isActive(%this)) 
	{
		%player = Station::getTarget(%this);
		if(%player != -1 && %this.lastPlayer == %player) 
		{
			%player.Station = %this;
			GameBase::SetDamageLevel(%player,0); //ammostation repair
			// Hardcoded here for the ammo types
			%cnt = Station::itemsToResupply(%player);
			if(getSimTime() - %this.enterTime > 11)
				%cnt = 0;
			if(%cnt != 0) 
			{
				%player.waitThrowTime = getSimTime();
				schedule("AmmoStation::onResupply(" @ %this @ ");",0.5,%this);
				return;
			}
			%player.Station = "";
			%client = Player::getClient(%player);
			%this.target = "";
			Client::sendMessage(%client,0,"Resupply Complete");
			if($TeamEnergy[Client::getTeam(%client)] == "Infinite" && $TALT::Active == false) //
				Client::setInventoryText(%client, "<f1><jc>T O T A L \n<f3>A N N I H I L A T I O N"); //updated -death666
			else	
				Client::setInventoryText(%client, "<f1><jc>TEAM ENERGY: " @ $TeamEnergy[Client::getTeam(%client)]);

			if(Player::getMountedItem(%player,$WeaponSlot) == -1)
			{
				if(%player.lastWeapon != "")
				{
					Player::useItem(%player,%player.lastWeapon);
					%player.lastWeapon = "";
				}
			}
		}
		else if(%this.target != "") 
		{
			%player = Client::getOwnedObject(%this.target);
			%player.Station = "";
			if(Player::getMountedItem(%player,$WeaponSlot) == -1)
			{
				if(%player.lastWeapon != "") 
				{
					Player::useItem(%player,%player.lastWeapon);
					%player.lastWeapon = "";
				}
			}		
			%this.target = "";
		}
		else 
		{
			%this.lastPlayer.Station = "";
			if(Player::getMountedItem(%this.lastPlayer,$WeaponSlot) == -1)
			{
				if(%this.lastPlayer.lastWeapon != "") 
				{
					Player::useItem(%this.lastPlayer,%this.lastPlayer.lastWeapon); 
					%this.lastPlayer.lastWeapon = "";
				}
			}
			%this.target = "";
		}
		GameBase::setActive(%this,false);
		%this.enterTime="";
	}
}

function AmmoStation::resupply(%player,%weapon,%item,%delta)
{
	%client = Player::getClient(%player);
	%delta = checkResources(%player,%item,%delta,1);
	if(%delta > 0) 
	{
		if(%item == RepairPatch) 
		{
			teamEnergyBuySell(%player,%item.price * %delta * -1);
			GameBase::repairDamage(%player,0.25);
			return %delta;
		}
		else if(%item == MineAmmo || %item == Grenade || %item == RepairKit) 
		{
			teamEnergyBuySell(%player,%item.price * %delta * -1);
			Annihilation::incItemCount(%player,%item,%delta);
			return %delta;
		}
		else if(%item == Beacon) 
		{
			
			if(%client.ListType == RemoteInvList && $ArmorName[Player::getArmor(%player)] == iarmorBuilder)
				return 0;
			teamEnergyBuySell(%player,%item.price * %delta * -1);
			Annihilation::incItemCount(%player,%item,%delta);
			return %delta;
		}			
		else if(Player::getItemCount(%player,%weapon)) 
		{
			teamEnergyBuySell(%player,%item.price * %delta * -1);
			Annihilation::incItemCount(%player,%item,%delta);
		
			return %delta;
		}
	}
	return 0;
}
