
StaticShapeData InventoryStation
{
	description = "Station Supply Unit";
	shapeFile = "inventory_sta"; 
	className = "Station";
	visibleToSensor = true;
	shieldShapeName = "shield_medium";
	sequenceSound[0] = { "activate", SoundActivateInventoryStation };
	sequenceSound[1] = { "power", SoundInventoryStationPower };
	sequenceSound[2] = { "use", SoundUseInventoryStation };
	maxDamage = 1.0;
	debrisId = flashDebrisLarge;
	mapFilter = 4;
	mapIcon = "M_station";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	triggerRadius = 1.5;
	explosionId = flashExpLarge;
};

function InventoryStation::onEndSequence(%this,%thread)
{
	//echo("End Seq ",%thread);
	if(Station::onEndSequence(%this,%thread)) 
		InventoryStation::onResupply(%this,"InvList");
}

function InventoryStation::onResupply(%this,%InvShopList)
{
	if(GameBase::isActive(%this)) 
	{
		%player = Station::getTarget(%this);
		if(%player != -1 && %this.lastPlayer == %player) 
		{
			GameBase::SetDamageLevel(%player,0); // Fix all damage
			%client = Player::getClient(%player);
			if(%this.target != %client && !%player.Station) 
			{
				%player.Station = %this;
				%client.ListType = %InvShopList; //armor based list
				setupShoppingList(%client,%this,%InvShopList);
				updateBuyingList(%client);
				%this.target = %client;
				%this.clTeamEnergy = %client.TeamEnergy;
				if($Annihilation::ShoppingList)
				{
					%client.invo = %this; //armor based list
					%client.ListType = %InvShopList;
				}
				if(!%client.noEnterInventory)
					Client::setGuiMode(%client,$GuiModeInventory);
				Client::sendMessage(%client,0,"Station Access On");
				//Client::sendMessage(%client,1, "We are looking for programming help, email TATribes@Yahoo.com");
				%player.ResupplyFlag = 1;
				%weapon = Player::getMountedItem(%player,$WeaponSlot);
				//if(%weapon != -1) 
				//{
				//	%player.lastWeapon = %weapon;
				//	Player::unMountItem(%player,$WeaponSlot);
				//}
			}
			%player.waitThrowTime = getSimTime();
			schedule("InventoryStation::onResupply(" @ %this @ ");",0.5,%this);
			if(%player.ResupplyFlag) 
				%player.ResupplyFlag = resupply(%this);
			return;
		}
		GameBase::setActive(%this,false);
	}
	//if(%this.target != "") 
	if(%this.target) // 7
	{
		%player = Client::getOwnedObject(%this.target);
		Client::clearItemShopping(%this.target);
		Client::sendMessage(%this.target,0,"Station Access Off");
		Station::onEndSequence(%this);
		if(GameBase::getDataName(%player.Station) == DeployableInvStation) 
		{
			Client::setInventoryText(%this.target, "<f1><jc>TEAM ENERGY: " @ $TeamEnergy[Client::getTeam(%this.target)]);
			if(Client::getGuiMode(%this.target) != 1)
				Client::setGuiMode(%this.target,1);
			%player.Station = "";
			%this.target = "";
		}
		if(Player::getMountedItem(%player,$WeaponSlot) == -1)
		{
			if(%player.lastWeapon != "") 
			{
				Player::useItem(%player,%player.lastWeapon);
				%player.lastWeapon = "";
			}
		}
	}
	%this.enterTime="";
}

