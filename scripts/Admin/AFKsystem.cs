function TA::AFK()
{
	if (!$TA::AFKsystem)
		return;
		
	//echo("AFK check");
	$TA::AFKTimestamp = getSimTime();
	if (!$Server::TourneyMode)
	{
		%now = getSimTime();
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			//echo(Client::GetName(%cl) @ ", " @ %now - %cl.lastActiveTimestamp);
			if ((%now - %cl.lastActiveTimestamp) >= $TA::AFKtimelimit)
			{
					if(%cl.isGoated)
					{
						return;			
					}
				%team = Client::getTeam(%cl);
				if(%team != -1)
				{
					if(%cl.inArenaTD)
					{
						if ((%now - %cl.lastActiveTimestamp) >= 500) //a lil higher for td players.
						{
							ArenaTD::Leave(%cl);
						}
					}
							
					//echo("!! clearing AFK player to observer");
					%cl.afk = true;
					%cl.inArena = false; //
					%cl.inDuel = false; //
					playNextAnim(%cl);
					player::kill(%cl);
					processMenuInitialPickTeam(%cl, -2);
					Game::refreshClientScore(%cl);
				}
			}
			else
			{
				%cl.afk = false;
			}
		}
	}
	schedule("TA::AFK();", $TA::AFKmonitorInterval);
}

function TA::AFKStatus()
{
	echo("Next AFK Check in: " @ $TA::AFKmonitorInterval - (getSimTime() - $TA::AFKTimestamp) @ " seconds ");
	if (!$Server::TourneyMode)
	{
		%now = getSimTime();
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			%time = (%now - %cl.lastActiveTimestamp);
			echo(client::getname(%cl) @ " idle time: " @ %time);
		}
	}
}

function TA::OBSAFK()
{
	
	$TA::OBSAFKTimestamp = getSimTime();
	if (!$Server::TourneyMode)
	{
		%now = getSimTime();
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			if ((%now - %cl.lastActiveOBSTimestamp) >= 3600) // 1200
			{
				if(%cl.observerMode == "observerFly" || %cl.observerMode == "observerOrbit")
				{
					if(%cl.isGoated)
					{
						return;			
					}	
					messageAll(0, Client::getName(%cl) @ " was automatically removed by a bot for being AFK. ~wshell_click.wav");
					%message = "You were automatically removed by a bot for being AFK. Please rejoin.";
					Net::kick(%cl,%message);
				}
			}
		
		}
	}
	schedule("TA::OBSAFK();", 60);
}