
function InvTrigger::onEnter(%this,%object)
{
	%type = getObjectType(%object);
	if(%type == "Player") 
	{
		if(Player::isAIControlled(%object))
			return;
		%group = getGroup(%this); 
		%obj = %this.Owner;
		%client = GameBase::getOwnerClient(%object);
		
		//Ok, this is a strange bug. We're only going to track last trigger we enter with this..
		%object.InvObject = %obj;	
		//Lightning projectiles are not deleted when firing object is destroyed. Yay. 
		
		%object.invtrigger++;	// Leeloo Dallas multipass fix. 8/28/2009 9:37AM
		if($debuginv)
			messageall(1,"Trigger(s) ="@%object.invtrigger);
		
		if($debug || $debuginv || $debugt)
		{
			echo("InvTrigger::onEnter "@%obj@" Damage state,"@GameBase::getDamageState(%obj)@", Powered,"@GameBase::isPowered(%obj)@" client,"@%client);
			echo("Left turret active"@%obj.lTurret@", "@GameBase::isActive(%obj.lTurret)@ " energy="@GameBase::getEnergy(%obj.lTurret)@ " right turret active"@%obj.rTurret@", "@GameBase::isActive(%obj.rTurret)@ " energy="@GameBase::getEnergy(%obj.rTurret));
		}
		if((GameBase::getTeam(%object) == GameBase::getTeam(%obj) || GameBase::getTeam(%obj) == -1) && %client.isSpy != true)
		{
			%client.InvTargetable = true;	
			if(GameBase::getDamageState(%obj) == "Enabled") 
			{
				if(GameBase::isPowered(%obj)) 
				{ 
					
					gamebase::setactive(%obj.lTurret,true);	//just to make certain.. gets changed on drop ship destroy.. for some damn reason..
					gamebase::setactive(%obj.rTurret,true);
					GameBase::setRechargeRate(%obj.lTurret,100);
					GameBase::setRechargeRate(%obj.rTurret,100);						
									

					if(!$Annihilation::Zappy)
					{
						%Client.InvConnect = true;
						%client.ListType = "InvList";
						GameBase::playSound(%object, SoundActivatePDA,0);
						Client::sendMessage(%client,3,"..Inventory Uplink established..");
						QuickInv(%client);
						schedule("ZappyResupply(" @ %client @ ");",0.25);	
					}
				}
			}			
			else
			{
				gamebase::setactive(%obj.lTurret,false);	//just to make certain.. gets changed on drop ship destroy.. for some damn reason..
				gamebase::setactive(%obj.rTurret,false);
				GameBase::setRechargeRate(%obj.lTurret,100);
				GameBase::setRechargeRate(%obj.rTurret,100);	
			}			
		}		
	}
}



function InvTrigger::onLeave(%this,%object)
{
	%type = getObjectType(%object);
	if(%type == "Player")
	{
		if(Player::isAIControlled(%object))
			return;
		
		%client = GameBase::getOwnerClient(%object);
				
		%TrigPos = Gamebase::getPosition(%this);
		%PlPos = getBoxCenter(%object);	
		%dist = vector::getdistance(%PlPos,%TrigPos);		
		if(%dist > 3)
		{
			%object.invtrigger--;	// Leeloo Dallas multipass fix. 8/28/2009 9:37AM
			if($debuginv)
				messageall(1,"Trigger(s) ="@%object.invtrigger);
			%client.ConnectBeam = "";
			
			%PlayerStation = %object.InvObject;
			%TriggerStation = %this.owner;
			if(%PlayerStation == %TriggerStation || %object.invtrigger < 1)
			{
				if($debug || $debuginv || $debugt)
				{
					echo("InvTrigger::onLeave, client= "@%client@" Dist= "@%dist);
				}
				
				
				
				%client.ConnectBeam = "";	//internal
				%client.InvTargetable = "";	//internal
				%Client.InvConnect = "";	//external	
				QuickInvOff(%client);	
				%object.ZappyResupply = "";
				%client.ListType = "";
			}
			else
			{

				// fire up the new triggers inventory station. 
		
				%client.ConnectBeam = "";	//internal
				if((GameBase::getTeam(%object) == GameBase::getTeam(%PlayerStation) || GameBase::getTeam(%PlayerStation) == -1) && %client.isSpy != true)
				{
					%client.InvTargetable = true;	
					if(GameBase::getDamageState(%PlayerStation) == "Enabled") 
					{
						if(GameBase::isPowered(%PlayerStation)) 
						{ 
							gamebase::setactive(%PlayerStation.lTurret,true);	//just to make certain.. gets changed on drop ship destroy.. for some damn reason..
							gamebase::setactive(%PlayerStation.rTurret,true);
							GameBase::setRechargeRate(%PlayerStation.lTurret,100);
							GameBase::setRechargeRate(%PlayerStation.rTurret,100);						
							Client::sendMessage(%client,3,"..Attempting Uplink transfer..");	
					

							if(!$Annihilation::Zappy)
							{
								%Client.InvConnect = true;
								%client.ListType = "InvList";
								GameBase::playSound(%object, SoundActivatePDA,0);
								Client::sendMessage(%client,3,"..Inventory Uplink established..");
								QuickInv(%client);
								schedule("ZappyResupply(" @ %client @ ");",0.25);	
							}
						}
					}			
					else
					{
						gamebase::setactive(%PlayerStation.lTurret,false);	//just to make certain.. gets changed on drop ship destroy.. for some damn reason..
						gamebase::setactive(%PlayerStation.rTurret,false);
						GameBase::setRechargeRate(%PlayerStation.lTurret,100);
						GameBase::setRechargeRate(%PlayerStation.rTurret,100);	
					}			
				}
			}	
		}
	}
}


function MobileInventory::onCollision(%this, %object) 
{	
	if($trace) 
		echo($ver,"| MobileStation::onCollision ",%this);
	if($debug) 
		event::collision(%this,%object);
	if(%object.testing)	
		return;
	if(%this.cloaked > 0 && getObjectType(%object) == "Player"){
		GameBase::startFadein(%this);	
		%this.cloaked = "";
		}
	if(%this.target == ""){
		dbecho(3, "STATION: Collision (" @ %this @ "," @ %object @ ")");
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



function MobileInventory::onDestroyed(%this)
{
	if($trace) 
		echo($ver,"| MobileInventory::onDestroyed");
	MobileInventory::onDisabled(%this);
	
	GameBase::setEnergy(%this.lTurret,0);	
	GameBase::setDamageLevel(%this.lTurret,1100);	
	GameBase::setEnergy(%this.rTurret,0);	
	GameBase::setDamageLevel(%this.rTurret,1100);	
	

	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		%player = Client::getOwnedObject(%cl);
		if(%player.InvObject == %this)
		{
			%cl.ConnectBeam = "";	//internal
			%cl.InvTargetable = "";	//internal
			%Cl.InvConnect = "";	//external	
			QuickInvOff(%cl);	
			%player.ZappyResupply = "";
			%cl.ListType = "";
		}
	}
			
	%this.cloakable = "";
	%this.nuetron = "";
	StaticShape::objectiveDestroyed(%this);
	if(!$NoCalcDamage)
		calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 3, 0.55, 0.30, 250, 170);
	$TeamItemCount[GameBase::getTeam(%this) @ "MobileInventoryPack"]--;
	
	%trigger = %this.trigger;	

	// Leeloo Dallas multipass fix. 8/28/2009 11:36AM
	%set = newObject("set",SimSet);
	%pos = GameBase::getPosition(%trigger);
	%num = containerBoxFillSet(%set,$SimPlayerObjectType,%pos,4,4,4,0);
	%totalnum = Group::objectCount(%set);
	
	for(%i = 0; %i < %totalnum; %i++)
	{
		%obj = Group::getObject(%set, %i);
		%p = GameBase::getPosition(%obj);
		%dist = Vector::getDistance(%pos, %p);
		if(%dist < 3)
			%obj.invtrigger--;

	}
	deleteObject(%set);

	Schedule("deleteobject("@%trigger@");",0.1);
}

//Updated 3/17/2007 6:56AM
function InventoryCharge::damageTarget(%target, %timeSlice, %damPerSec, %enDrainPerSec, %pos, %vec, %mom, %shooterId)
{
	// verify player is within sight of coresponding inventory -invs not covered with blast walls..	
	// could do this with a diffrent damage type, but this would have to be in player::ondamage

	%type = getObjectType(%target);
	if(%type == "Player") 
	{
		if(Player::isAIControlled(%target))
			return;
	}

	%client = GameBase::getOwnerClient(%target);
	%client.ConnectBeam ++;	// Leeloo Dallas multipass fix. 8/28/2009 11:36AM
	

	%InvObject = %target.InvObject;
	%InvPos = gamebase::getposition(%InvObject);
	//client::sendmessage(%client,0,"Inv zappy "@%client.InvTargetable@", InvStation ="@%InvPos);
	if(%client.InvTargetable && %InvPos != "0 0 0")
	{
		if(!%Client.InvConnect)
		{	
			Client::sendMessage(%client,3,"..Inventory Uplink established..");
			
			QuickInv(%client);
			schedule("ZappyResupply(" @ %client @ ");",0.25);
			GameBase::playSound(%target, SoundActivatePDA,0);
					
		}
				
		%client.InvConnect = true;
		%client.ListType = "InvList";
	
	}
	else
	{
//		%client.ConnectBeam = "";	//internal
		%client.InvTargetable = "";	//internal
		%Client.InvConnect = "";	//external	
		QuickInvOff(%client);	
		%object.ZappyResupply = "";
		%client.ListType = "";		
	}
}

function InvTurret::verifyTarget(%this,%target)
{
	if(!$Annihilation::ExtendedInvs || !$Annihilation::Zappy) 
		return false;
	
	%client = GameBase::getOwnerClient(%target);
	%control = GameBase::getControlClient(%target);
	// was a little funkyness (cheat hole) with turrets damaging vehicles and giving inv to owner.
	if(!%client.InvTargetable || %client.ConnectBeam > 60 || %control == -1)
		return "False";
		
	%owner = %this.Owner;
	if(!GameBase::isPowered(%Owner) || GameBase::getDamageState(%Owner) != "Enabled") 
		return false;
	
	else return true;
}