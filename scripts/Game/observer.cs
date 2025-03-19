$GuiModeCommand	= 2;
$LastControlObject = 0;

function Observer::triggerDown(%client)
{
}

function Observer::orbitObjectDeleted(%cl)
{
}

function Observer::leaveMissionArea(%cl)
{
	centerprint(%cl,"<jc>You are out of bounds...");
}

function Observer::enterMissionArea(%cl)
{
}


function Observer::onBeacon(%player, %item)
{
	Anni::Echo("observer beacon");
}


function Observer::setAnnihilationOrbit(%client, %target)
{	
	Client::setControlObject(%client, Client::getObserverCamera(%client));
	if(!%client.isBlackOut)
		bottomprint(%client, "<jc>Observing " @ Client::getName(%target)@".", 50);

	if(!%client.obsmode || %client.obsmode == "") Observer::setOrbitObject(%client, %target, 5, 5, 5);
	else if(%client.obsmode == 1) Observer::setOrbitObject(%client, %target, 15, 15, 15);
	else if(%client.obsmode == 2) Observer::setOrbitObject(%client, %target, 30, 30, 30);
	else if(%client.obsmode == 3) Observer::setOrbitObject(%client, %target, -3, -1, -0);
	else if(%client.obsmode == 4) Observer::setOrbitObject(%client, %target, -7, -7, -0);
	else if(%client.obsmode == 5) Observer::setFlyMode(%client, vector::add("0 0 1",GameBase::getPosition(Client::getControlObject(%target))), GameBase::getRotation(Client::getControlObject(%target)), true, true);	

}



function Observer::triggerUp(%client)
{
	%now = getSimTime(); //OBS AFK System -Ghost
	%client.lastActiveOBSTimestamp = %now; //OBS AFK System -Ghost
	
	if($debug)
		Anni::Echo("Trigger up Obs Mode ="@%client.observerMode@" Admin Obs Mode ="@%client.AdminobserverMode);
	
	if($ArenaTD::TimeOut && $ArenaTD::TO::StartRound)
	{
		if($ArenaTD::TimeOutTDOne && %client.isTDCaptOne)
		{
			ArenaTD::TimeOut(%client,false);
		}
		else if($ArenaTD::TimeOutTDTwo && %client.isTDCaptTwo)
		{
			ArenaTD::TimeOut(%client,false);
		}
	
	}
	
	if($ArenaTD::StartTime)
	{
		if(%client.isTDCaptOne)
		{
			ArenaTD::ReadyUp(%client,"one");
		}
		else if(%client.isTDCaptTwo)
		{
			ArenaTD::ReadyUp(%client,"two");
		}
	
	}
	
	if(%client.observerMode == "observerAdmin")
		Admin::observe(%client,%client.observerTarget);		
	else if(%client.observerMode == "dead")
	{
		if(%client.dieTime + $Server::RespawnTime < getSimTime())
		{
			if(Game::playerSpawn(%client, true))
			{
				%client.observerMode = "";
				Observer::checkObserved(%client);
			}
		}
	}

	else if(%client.observerMode == "observerOrbit")
		Observer::nextObservable(%client);
	else if(%client.observerMode == "observerFly")
	{
		%camSpawn = Game::pickObserverSpawn(%client);
		Observer::setFlyMode(%client, GameBase::getPosition(%camSpawn), 
			GameBase::getRotation(%camSpawn), true, true);
	}
	else if(%client.observerMode == "justJoined")
	{

		if(%client.firstConnect == true && $TALT::Active == false) //get rid of this
		{
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
					%client.firstConnect = false;
					%client.InSchool = "";
					%client.observerMode = "";
					Game::playerSpawn(%client, false);						
				}
			}
			else
			{				
				%client.InSchool = 1;
				NewbieSchool(%client);
				return;
			}
			
		}
		else
		{	
			%client.observerMode = "";
			Game::playerSpawn(%client, false);
		}
	}
	else if(%client.observerMode == "pregame" && $Server::TourneyMode)
	{
		if($CountdownStarted)
			return;

		if(%client.notready)
		{
			%client.notready = "";
			MessageAll(0, Client::getName(%client) @ " is READY.");
			if(%client.notreadyCount < 3)
				bottomprint(%client, "<f1><jc>Waiting for match start (FIRE if not ready).", 0);
			else 
				bottomprint(%client, "<f1><jc>Waiting for match start.", 0);
		}
		else
		{
			%client.notreadyCount++;
			if(%client.notreadyCount < 4)
			{
				%client.notready = true;
				MessageAll(0, Client::getName(%client) @ " is NOT READY.");
				bottomprint(%client, "<f1><jc>Press FIRE when ready.", 0);
			}
			return;
		}
		Game::CheckTourneyMatchStart();
	}
}


function Observer::jump(%client)
{	
	%now = getSimTime(); //OBS AFK System -Ghost
	%client.lastActiveOBSTimestamp = %now; //OBS AFK System -Ghost
	
	if($debug)
		Anni::Echo("Jump Obs Mode ="@%client.observerMode@" Admin Obs Mode ="@%client.AdminobserverMode);
		
	//if(%client.skiFix) 
	//	return;
	//%client.skiFix = true;
	//schedule(%client @".skiFix = false;", 0.3);
	
	//%mode = %client.ObserverMode;
	//%target = %client.ObserverTarget;
	//if(%target)
	//	%type = getObjectType(%target);

	//if(%mode != "")
	//	%action = %mode;
	//else if(%type != Net::PacketStream)
	//	%action = %type;

	//if(%action)
	//eval(%action@"::jump("@%client@");");

	//if($debug)
	//	echo(%action@"::jump "@%this);

	
	if(%client.observerMode == "justJoined")
	{
		%client.firstConnect = false;
		%client.InSchool = "";
		Observer::triggerUp(%client);
	}

	
	if(%client.observerMode == "observerAdmin")
	{
		%pl = Client::getOwnedObject(%client);
		if ( %client.lastControlObject )
			Client::setControlObject(%client, %client.lastControlObject);
		else
			Client::setControlObject(%client, %pl);
		%client.lastControlObject = "";
		%pl.invulnerable = false;
		%client.observerTarget = "";
		%client.observerMode = "";
		if(!%client.isBlackOut)
			bottomprint(%client, "", 0);	
	}
	else if(%client.observerMode == "observerFly")
	{
		%client.observerMode = "observerOrbit";
		if(!%client.isBlackOut)
			bottomprint(%client, "", 0);
		schedule("observer::alert("@%client@");",20);

		if(%client.AdminobserverMode == "AdminObserve") %client.AdminobserverMode = "";
		%client.observerTarget = %client;
		Observer::nextObservable(%client);
	}
	
	else if(%client.observerMode == "observerOrbit")
	{
		%lastObserved = %client.observerTarget;
		if($Annihilation::obsAlert && %lastObserved != %client && !%client.isAdmin && !%client.isBlackOut)
		{
			bottomprint(%lastObserved, "<jc> No longer Being Observed by " @ Client::getName(%client), 15);
			client::sendmessage(%lastObserved,0,"No longer Being Observed by " @ Client::getName(%client));
		}
			if(%lastObserved != %client)
		{
			if(%lastObserved.isGoated)
		{
if(%client.ObsCD == "false")
{
	return;
}

if(%client.ObsCD)
{
	%client.ObsCD = false;
	schedule(%client@".ObsCD= true;",15.0,%client);
}
			bottomprint(%lastObserved, "<jc> No longer Being Observed by " @ Client::getName(%client), 15);
			client::sendmessage(%lastObserved,0,"No longer Being Observed by " @ Client::getName(%client));
		}
		}
		%client.observerTarget = "";
		
		if(%client.AdminobserverMode == "AdminObserve") %client.AdminobserverMode = "";
		
		%client.observerMode = "observerFly";
		%camSpawn = Game::pickObserverSpawn(%client);
		Observer::setFlyMode(%client, GameBase::getPosition(%camSpawn), GameBase::getRotation(%camSpawn), true, true);
	}

}

function Observer::isObserver(%clientId)
{
	return %clientId.observerMode == "observerOrbit" || %clientId.observerMode == "observerFly";
}

function Observer::enterObserverMode(%clientId)
{
	if(%clientId.observerMode == "observerOrbit" || %clientId.observerMode == "observerFly")
		return false;
	Client::clearItemShopping(%clientId);
	%player = Client::getOwnedObject(%clientId);
	if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
	{
		playNextAnim(%clientId);
		Player::kill(%clientId);
	}
	Client::setOwnedObject(%clientId, -1);
	Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
	%clientId.observerMode = "observerOrbit";
	//if(!%clientId.inArenaTD) //Dont change their team
	//{
		if(Client::getTeam(%clientId) != -1)
			%clientId.LastTeam = Client::getTeam(%clientId);
		GameBase::setTeam(%clientId, -1);
	//}
	Observer::jump(%clientId);
	remotePlayMode(%clientId);
	return true;
}

function Observer::checkObserved(%client)
{
	// this function loops through all the clients and checks
	// if anyone was observing %client... if so, it updates that
	// observation to reflect the new %client owned object.

	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.observerTarget == %client)
		{
			if(%cl.observerMode == "observerOrbit" || %clientId.observerMode == "observerAdmin")
				Observer::setAnnihilationOrbit(%cl, %client);//Observer::setOrbitObject(%cl, %client, 5, 5, 5);
			else if(%cl.observerMode == "commander")
				Observer::setOrbitObject(%cl, %client, -3, -3, -3);
		}
	}
}

function Observer::setTargetClient(%client, %target)
{
	if(%client.observerMode != "observerOrbit")
		return false;
	%owned = Client::getOwnedObject(%target);
	if(%owned == -1)
		return false;
	Observer::setAnnihilationOrbit(%client, %target);	
	if(!%client.isBlackOut)
		bottomprint(%client, "<jc>Observing " @ Client::getName(%target), 5);
		if(%target.isGoated)
		{
if(%client.ObsCD == "false")
{
	return;
}

if(%client.ObsCD)
{
	%client.ObsCD = false;
	schedule(%client@".ObsCD= true;",15.0,%client);
}
			bottomprint(%target, "<jc>Being Observed by " @ Client::getName(%client), 15);
			client::sendmessage(%target,0,"Being Observed by " @ Client::getName(%client));
		}
	%client.observerTarget = %target;
	schedule("observer::alert("@%client@","@%target@");",50);
	return true;
}


function observer::alert(%client,%target)
{


if(%client.ObsCD == "false")
{
	return;
}

if(%client.ObsCD)
{
	%client.ObsCD = false;
	schedule(%client@".ObsCD= true;",15.0,%client);
}
	if($debug)
		Anni::Echo("observer::alert "@%client@", "@%target);
	if(%client.observerMode == "observerOrbit" || %client.observerMode == "observerAdmin")
	{	
		%CurrentTarget = %client.observerTarget;
		if(%target == %CurrentTarget)
			{
			bottomprint(%client, "<jc>Observing " @ Client::getName(%target)@".", 50);
		if(%target.isGoated)
		{
				bottomprint(%target, "<jc>Being Observed by " @ Client::getName(%client), 15);
				client::sendmessage(%target,0,"Being Observed by " @ Client::getName(%client));
		}		 
			// schedule("observer::alert("@%client@");",60);
			schedule("observer::alert("@%client@","@%target@");",30);	
		}	 	
	}	
}

function Observer::nextObservable(%client)
{
	%lastObserved = %client.observerTarget;
	%nextObserved = Client::getNext(%lastObserved);


		if(%lastObserved != %client)
		{
		if(%target.isGoated)
		{
if(%client.ObsCD == "false")
{
	return;
}

if(%client.ObsCD)
{
	%client.ObsCD = false;
	schedule(%client@".ObsCD= true;",15.0,%client);
}
		bottomprint(%lastObserved, "<jc> No longer Being Observed by " @ Client::getName(%client), 15);
		client::sendmessage(%lastObserved,0,"No longer Being Observed by " @ Client::getName(%client));
		}
		}
	%ct = 128; // just in case
	if(%client.inArena)
	{
		while(%ct--)
		{
			if(%nextObserved == -1)
			{
				%nextObserved = Client::getFirst();
				continue;
			}
			%owned = Client::getOwnedObject(%nextObserved);
			if(%nextObserved == %lastObserved && %owned == -1)
			{
				Observer::jump(%client);
				return;
			}
			if(%owned == -1 || !%nextObserved.inArena)
			{
				%nextObserved = Client::getNext(%nextObserved);
				continue;
			}
			Observer::setTargetClient(%client, %nextObserved);
			return;
		}
	}
	else
	{
		while(%ct--)
		{
			if(%nextObserved == -1)
			{
				%nextObserved = Client::getFirst();
				continue;
			}
			%owned = Client::getOwnedObject(%nextObserved);
			if(%nextObserved == %lastObserved && %owned == -1)
			{
				Observer::jump(%client);
				return;
			}
			if(%owned == -1)
			{
				%nextObserved = Client::getNext(%nextObserved);
				continue;
			}
			Observer::setTargetClient(%client, %nextObserved);
			return;
		}
	}
	Observer::jump(%client);

}

function Observer::prevObservable(%client)
{
}






function remoteSCOM(%clientId, %observeId)
{
	%observeId = floor(%observeId);
	if( CheckEval("remoteSCOM", %clientId, %observeId) || ( Client::getTransportAddress(%observeId) == "" && %observeId != -1 ) )
		return;
		
	if(%observeId != -1)
	{
		if(Client::getTeam(%clientId) == Client::getTeam(%observeId) &&
			(%clientId.observerMode == "" || %clientId.observerMode == "commander") && Client::getGuiMode(%clientId) == $GuiModeCommand)
		{
			Client::limitCommandBandwidth(%clientId, true);
			if(%clientId.observerMode != "commander")
			{
				%clientId.observerMode = "commander";
				%clientId.lastControlObject = Client::getControlObject(%clientId);
			}
			Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
			Observer::setOrbitObject(%clientId, %observeId, -3, -3, -3);
			%clientId.observerTarget = %observeId;
			Observer::setDamageObject(%clientId, %clientId);
		}
	}
	else
	{
		Client::limitCommandBandwidth(%clientId, false);
		if(%clientId.observerMode == "commander")
		{
			Client::setControlObject(%clientId, %clientId.lastControlObject);
			%clientId.lastControlObject = "";
			%clientId.observerMode = "";
			%clientId.observerTarget = "";
		}
	}
}

// EoF
