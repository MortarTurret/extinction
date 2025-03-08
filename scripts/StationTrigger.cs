

function InventoryStation::onadd(%this)
{
//	echo("!! adding Inv");
	if($Annihilation::ExtendedInvs && !$ME::varsInitialized)				
		schedule("AddInventoryTrigger("@%this@");",5,%this);	
}
function testInventoryStation::onadd(%this)
{
	GameBase::setPosition(%this,vector::add(GameBase::getPosition(%this),vector::getfromrot(GameBase::getRotation(%this),-10,0)));		
	GameBase::setRotation(%this,vector::add(gamebase::getrotation(%this),"0 0 -1.57"));
	if($Annihilation::ExtendedInvs && !$ME::varsInitialized)				
		schedule("AddInventoryTrigger("@%this@");",3,%this);
	schedule("GameBase::playSequence("@%this@",0,\"busy\");",5,%this);
}



//for additional inventory check..
LightningData InventoryCharge
{
	bitmapName = "lightningNew.bmp";
	damageType = $ElectricityDamageType;
	boltLength = 10.0;
	coneAngle = 35.0;
	damagePerSec = -0.5;
	energyDrainPerSec = 0.0;
	segmentDivisions = 4;
	numSegments = 5;
	beamWidth = 0.125;//075;
	updateTime = 120;
	skipPercent = 0.5;
	displaceBias = 0.15;	// how wide..
	lightRange = 3.0;
	lightColor = { 0.25, 0.25, 0.85 };
	soundId = SoundMortarIdle;	//SoundELFFire;
};

TurretData InvTurret	   
{			 
	maxDamage = 1000.0;
	maxEnergy = 150;
	minGunEnergy = 50;
	maxGunEnergy = 5;
	range = 7.5;		

	visibleToSensor = false;
	dopplerVelocity = 0.1;
	castLOS = true;
	supression = false;
	pinger = false;
	supressable = false;
	
	debrisId = defaultDebrisMedium;
	className = "";
	shapeFile = "camera";
	projectileType = InventoryCharge;
	speed = 5.0;
	speedModifier = 1.5;
	reloadDelay = 0.0; 

	isSustained     = true;
	firingTimeMS    = 5000;	//1750
	energyRate      = 30.0;
};



TriggerData InvTrigger
{
	className = "Trigger";
	rate = 0.1;	//1.0;
};


// this is called on mission change, when inv station is added..
function AddInventoryTrigger(%this)
{
	//if(%this.isArenaPart == true) 
	//	return;
	//echo("ghost? "@$ghosting);
	if(!$ghosting)
		schedule("AddInventoryTrigger("@%this@");",3,%this);
	if(%this.isArenaPart)
	{
		%group = newObject("InvNum"@%this,Simset);
		addToSet(MissionCleanup, %group);
		addToSet("MissionCleanup/Inventory",%group);	
		addToSet(%group,%this);	
		%this.group = %group;
		
		%Invpos = GameBase::GetPosition(%this);
		%rot = gamebase::getrotation(%this);
		%team = GameBase::getTeam(%this);
		%vec = rotateVector("0 -2.25 1.5",%rot);
		%pos = vector::add(%InvPos,%vec);
		
		// now for the fun part..
		instant Trigger "InventoryTrigger" {
			dataBlock = "InvTrigger";
			name = "InvTrigger";
			Team = %team;
			position = %pos;
			rotation = %rot;
			boundingBox = "-3 -3 -2.0 3 3 2.0";
			isSphere = "true";
			Owner = %this;
		};	
		
		// add inv turrets.. bah..	
		%vec = rotateVector("2.16 -0.5 1.4",%rot);
		%pos = vector::add(%InvPos,%vec);	
		%camera = newObject("Camera","Turret",InvTurret,true); 
		addToSet("MissionCleanup\\Arena", %camera);
		addToSet("MissionCleanup\\Arena\\InvNum"@%this,%camera);
		GameBase::setRotation(%camera,vector::add(%rot,"0 0 -2.02")); 
		GameBase::setPosition(%camera,%pos); 
		%this.rTurret = %camera;
		%camera.Owner = %this;	
			GameBase::setTeam(%camera,getNumTeams()-1); 	
			GameBase::setActive(%camera,true);	
			GameBase::setRechargeRate(%camera,100); 	
					
		%vec = rotateVector("-2.2 -0.5 1.4",%rot);	
		%pos = vector::add(%InvPos,%vec);	
		%camera = newObject("Camera","Turret",InvTurret,true); 
		addToSet("MissionCleanup\\Arena", %camera);
		addToSet("MissionCleanup\\Arena\\InvNum"@%this,%camera);
		GameBase::setRotation(%camera,vector::add(%rot,"0 0 -1.14")); 
		GameBase::setPosition(%camera,%pos); 	
		%this.lTurret = %camera;
		%camera.Owner = %this;	
			GameBase::setTeam(%camera,getNumTeams()-1); 	
			GameBase::setActive(%camera,true);	
			GameBase::setRechargeRate(%camera,100); 		
	}
	else
	{				
		%group = newObject("InvNum"@%this,Simset);
		addToSet(MissionCleanup, %group);
		addToSet("MissionCleanup/Inventory",%group);	
		addToSet(%group,%this);	
		%this.group = %group;
		
		%Invpos = GameBase::GetPosition(%this);
		%rot = gamebase::getrotation(%this);
		%team = GameBase::getTeam(%this);
		%vec = rotateVector("0 -2.25 1.5",%rot);
		%pos = vector::add(%InvPos,%vec);
		
		// now for the fun part..
		instant Trigger "InventoryTrigger" {
			dataBlock = "InvTrigger";
			name = "InvTrigger";
			Team = %team;
			position = %pos;
			rotation = %rot;
			boundingBox = "-3 -3 -2.0 3 3 2.0";
			isSphere = "true";
			Owner = %this;
		};	
		
		// add inv turrets.. bah..	
		%vec = rotateVector("2.16 -0.5 1.4",%rot);
		%pos = vector::add(%InvPos,%vec);	
		%camera = newObject("Camera","Turret",InvTurret,true); 
		addToSet("MissionCleanup/Inventory", %camera);
		addToSet("MissionCleanup/Inventory/InvNum"@%this,%camera);
		GameBase::setRotation(%camera,vector::add(%rot,"0 0 -2.02")); 
		GameBase::setPosition(%camera,%pos); 
		%this.rTurret = %camera;
		%camera.Owner = %this;	
			GameBase::setTeam(%camera,getNumTeams()-1); 	
			GameBase::setActive(%camera,true);	
			GameBase::setRechargeRate(%camera,100); 	
					
		%vec = rotateVector("-2.2 -0.5 1.4",%rot);	
		%pos = vector::add(%InvPos,%vec);	
		%camera = newObject("Camera","Turret",InvTurret,true); 
		addToSet("MissionCleanup/Inventory", %camera);
		addToSet("MissionCleanup/Inventory/InvNum"@%this,%camera);
		GameBase::setRotation(%camera,vector::add(%rot,"0 0 -1.14")); 
		GameBase::setPosition(%camera,%pos); 	
		%this.lTurret = %camera;
		%camera.Owner = %this;	
			GameBase::setTeam(%camera,getNumTeams()-1); 	
			GameBase::setActive(%camera,true);	
			GameBase::setRechargeRate(%camera,100); 					
	}	
}


// gotta put this somewhere... make sure it gets added to mission cleanup, so it gets deleted on map change.
function InvTrigger::onadd(%this)
{
	%owner = %this.Owner;
	%Owner.Trigger = %this;	
	addToSet("MissionCleanup/Inventory", %this);
	addToSet("MissionCleanup/Inventory/InvNum"@%owner,%this);	
}


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
						schedule("ZappyResupply(" @ %client @ ");",0.25,%client);	
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
			
			%PlayerStation = %object.InvObject;
			%TriggerStation = %this.owner;
			if(%PlayerStation == %TriggerStation)
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
								schedule("ZappyResupply(" @ %client @ ");",0.25,%client);	
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


function InvTurret::verifyTarget(%this,%target)
{
	if(!$Annihilation::ExtendedInvs || !$Annihilation::Zappy) 
		return false;
	
	%client = GameBase::getOwnerClient(%target);
	%control = GameBase::getControlClient(%target);

	if(!%client.InvTargetable || %client.ConnectBeam || %control == -1)
		return "False";
		

	%owner = %this.Owner;
	if(!GameBase::isPowered(%Owner) || GameBase::getDamageState(%Owner) != "Enabled") 
		return false;
	
	else return true;
}

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
	if(!%Client.InvConnect)
	{	
		Client::sendMessage(%client,3,"..Inventory Uplink established..");
		
		QuickInv(%client);
		schedule("ZappyResupply(" @ %client @ ");",0.25,%client);
		GameBase::playSound(%target, SoundActivatePDA,0);		
	}	
		
	%client.InvConnect = true;
	%client.ListType = "InvList";
	schedule("QuitBeam("@%client@");",1.0,%client);
}

//Updated 3/17/2007 6:56AM
function InventoryCharge::damageTarget(%target, %timeSlice, %damPerSec, %enDrainPerSec, %pos, %vec, %mom, %shooterId)
{
	// verify player is within sight of coresponding inventory -invs not covered with blast walls..	
	// could do this with a diffrent damage type, but this would have to be in player::ondamage

	%client = GameBase::getOwnerClient(%target);

	%type = getObjectType(%target);
	if(%type == "Player") 
	{
		if(Player::isAIControlled(%target))
			return;
	}
	

	%InvObject = %target.InvObject;
	%InvPos = gamebase::getposition(%InvObject);
	//client::sendmessage(%client,0,"Inv zappy "@%client.InvTargetable@", InvStation ="@%InvPos);
	if(%client.InvTargetable && %InvPos != "0 0 0")
	{
		if(!%Client.InvConnect)
		{	
			Client::sendMessage(%client,3,"..Inventory Uplink established..");
			
			QuickInv(%client);
			schedule("ZappyResupply(" @ %client @ ");",0.25,%client);
			GameBase::playSound(%target, SoundActivatePDA,0);
					
		}
				
		%client.InvConnect = true;
		%client.ListType = "InvList";
		schedule("QuitBeam("@%client@");",1.0,%client);
	}
	else
	{
		%client.ConnectBeam = "";	//internal
		%client.InvTargetable = "";	//internal
		%Client.InvConnect = "";	//external	
		QuickInvOff(%client);	
		%object.ZappyResupply = "";
		%client.ListType = "";		
	}
}

function QuitBeam(%client)
{
	if(%client.InvTargetable)	
		%client.ConnectBeam = "true";	
	else %client.ConnectBeam = "";	
}

function InvTurret::OnDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	if($debug::Damage)
	{
		echo("InvTurret::onDamage("@%this@", "@%type@", "@%value@", "@%pos@", "@%vec@", "@%mom@", "@%object@" )");
	}	
	//do nothing, no damage..
}

function ZappyResupply(%client)
{
	if(%client.InvConnect)
	{
		if(!isPlayerBusy(%client))
		{
			%player = Client::getOwnedObject(%client);
			if (%player == -1 || Player::isDead(%player))
				return;
	//			//damnit
	//			%player = Client::getOwnedObject(%client);
	//			echo("zappy resupply dead? "@Player::isDead(%player));	
	//			//end cursing	
			%cnt = Station::itemsToResupply(%player);
			if(%cnt != 0 || gamebase::getdamagelevel(%player)) 
			{
				%player.ZappyResupply = true;
				%player.waitThrowTime = getSimTime();
				GameBase::repairDamage(%player,0.15);
				schedule("ZappyResupply(" @ %client @ ");",0.5,%client);
				//return;
				
			}
			else %player.ZappyResupply = "";	
			//Client::sendMessage(%client,3,"...Resupply Complete, do something...");
		}
		else
			schedule("ZappyResupply("@%client@");",2,%client);	
	}
	
}


function ZappyPowerSwitch(%this,%power)
{
	%type = Gamebase::getdataname(%this);
	//echo(" ZappyPowerSwitch ("@%this,%power);
	if(string::ICompare(%type, "InventoryStation") == 0 || string::ICompare(%type, "MobileInventory") == 0)
	{
		gamebase::setactive(%this.lTurret,%power);	
		gamebase::setactive(%this.rTurret,%power);
		if(%power)
		{
			GameBase::setRechargeRate(%this.lTurret,100);
			GameBase::setRechargeRate(%this.rTurret,100);
		}
		else
		{
			GameBase::setRechargeRate(%this.lTurret,0);
			GameBase::setRechargeRate(%this.rTurret,0);
		}	
	}
}