
// base function modified to work with Annihilation, yet still compatable with base, and other mods.	
function GroupTrigger::onEnter(%this,%object)
{
//	Anni::Echo("contacting trigger...");
	%type = getObjectType(%object);
	if(%type == "Player" || %type == "Vehicle") {
		//Anni::Echo(%this.TeleTrigger);
		if(%this.TeleTrigger == true)
		{
			// Teleporters created with Annihilation mod have a TeleTrigger field
			TeleTrigger::onTrigEnter(%this, %object);
			//Anni::Echo("is teleporter");
		}
		else
		{
			%group = getGroup(%this); 
 			%count = Group::objectCount(%group);
 			for (%i = 0; %i < %count; %i++) 
 				GameBase::virtual(Group::getObject(%group,%i),"onTrigEnter",%object,%this);
 		}
	}
}


function GroupTrigger::onLeave(%this,%object)
{
	%type = getObjectType(%object);
	if(%type == "Player" || %type == "Vehicle")
	
	{
		%group = getGroup(%this);
		%count = Group::objectCount(%group);
		for(%i = 0; %i < %count; %i++) 
			GameBase::virtual(Group::getObject(%group,%i),"onTrigLeave",%object,%this);
	}
}

function GroupTrigger::onContact(%this,%object)
{
	%type = getObjectType(%object);
	if(%type == "Player" || %type == "Vehicle")
	{
		%group = getGroup(%this);
		%count = Group::objectCount(%group);
		for(%i = 0; %i < %count; %i++) 
			GameBase::virtual(Group::getObject(%group,%i),"onTrigger",%object,%this);
	}
}

function GroupTrigger::onActivate(%this)
{
}

function GroupTrigger::onDeactivate(%this)
{
}


function TeleTrigger::onEnter(%this,%object)
{	
	%type = getObjectType(%object);
	if(%type == "Player" || %type == "Vehicle" || %type == "Flier")
	{
		//Anni::Echo("searching for Teleporter drop points...");
		%client = Player::getclient(%object);
		if(GameBase::getControlClient(%object) != %client) %client = GameBase::getControlClient(%object);
		if(Client::getTeam(%client) != GameBase::getTeam(%this) && GameBase::getTeam(%this) != -1) return;
		%group = getGroup(%this);
 		%count = Group::objectCount(%group);
 		for(%i = 0; %i < %count; %i++)
 		{ 
 			//Anni::Echo("Examining drop points...");	
 			%data = GameBase::getDataName(Group::getObject(%group,%i));
 			if(%data == DropPointMarker)	//TeleDropMarker)  
 			{
 				%DropCount++;	
 				%droppos = Group::getObject(%group,%i);
 				//TeleporterSimSet(%this);
 				%teleset = nameToID("MissionCleanup/TeleportSet"@%this);
				if(%teleset == -1)
				{
					%group = newObject("TeleportSet"@%this, SimSet);
					addToSet(MissionCleanup, %group);
				}
 				addToSet("MissionCleanup/TeleportSet"@%this, %droppos);	
 			}
 		}
 			

 		%spawnIdx = floor(getRandom() * (%DropCount - 0.1));
		%group = nameToID("MissionCleanup/TeleportSet"@%this);		
 		%newpos = Group::getObject(%group,%spawnIdx);	

 		if(GameBase::GetPosition(%newpos) != "0 0 0")
 		{
 			GameBase::playSound(%object, ForceFieldOpen, 0);
			GameBase::playSound(%this, ForceFieldOpen, 0);
			Client::sendMessage(%client, 0, "You have been teleported.~wshieldhit.wav");
			GameBase::SetPosition(%object, GameBase::GetPosition(%newpos));
			%Trot = GameBase::getRotation(%newpos); 
			GameBase::setRotation(%object,%Trot);
			GameBase::startFadeIn(%object);	
 		} 
 		else Anni::Echo("!! WARNING !! No matching teleport drop point in group!!");		
	}
}




function TeleTrigger::onTrigEnter(%this,%object)
{
	%type = getObjectType(%object);
	if(%type == "Player" || %type == "Vehicle" || %type == "Flier")
	{
		%client = Player::getclient(%object);
		if(GameBase::getControlClient(%object) != %client) %client = GameBase::getControlClient(%object);
		if(Client::getTeam(%client) != GameBase::getTeam(%this) && GameBase::getTeam(%this) != -1) return;
		%group = getGroup(%this);
 		%count = Group::objectCount(%group);
 		for(%i = 0; %i < %count; %i++)
 		{ 	
 			%data = GameBase::getDataName(Group::getObject(%group,%i));
 			if(%data == DropPointMarker) 
 			{
 				%DropCount++;	
 				%droppos = Group::getObject(%group,%i);
 				%teleset = nameToID("MissionCleanup/TeleportSet"@%this);
				if(%teleset == -1)
				{
					%group = newObject("TeleportSet"@%this, SimSet);
					addToSet(MissionCleanup, %group);
				}
 				addToSet("MissionCleanup/TeleportSet"@%this, %droppos);	
 			}
 		}
 			

 		%spawnIdx = floor(getRandom() * (%DropCount - 0.1));
		%group = nameToID("MissionCleanup/TeleportSet"@%this);		
 		%newpos = Group::getObject(%group,%spawnIdx);	
 			
		%message = %newpos.message;	//new for 2.2
			
 		// Anni::Echo(%newpos@" message "@%message);
 		if(GameBase::GetPosition(%newpos) != "0 0 0")
 		{	
 			GameBase::playSound(%this, forcefieldopen, 0);
			

			if(%message != "")
				Client::sendMessage(%client, 0, %message);
			else
				Client::sendMessage(%client, 0, "You have been teleported.~wshieldhit.wav");
			GameBase::SetPosition(%object, GameBase::GetPosition(%newpos));
			%Trot = GameBase::getRotation(%newpos); 
			GameBase::setRotation(%object,%Trot);
			GameBase::startFadeIn(%object);	
			schedule("GameBase::playSound("@%object@", ForceFieldOpen, 0);",0.25,%object);
 		} 
 		else Anni::Echo("!! WARNING !! No matching teleport drop point in group!!");		
	}
}

