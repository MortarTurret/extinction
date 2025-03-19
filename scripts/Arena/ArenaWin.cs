$Arena::Winners = false;

function ArenaWin::ClearStage()
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.inArena)
		{
			if(%cl.inArenaTD)
				ArenaTD::Leave(%cl);
			
			%cl.inArenaWin = "";
			%player = Client::getOwnedObject(%cl);
			if(isObject(%player))
			{
				playNextAnim(%cl);
				Player::kill(%cl);
			}
			else
			{
				Game::playerSpawn(%cl, true);
				playNextAnim(%cl);
				Player::kill(%cl);
			}
			
			//%cl.observerMode = "observerOrbit";
		}
	}
	
	$Arena::Winners = true;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.inArena)
		{
			%clientId.isArenaWin = true;
			ArenaWin::SelectFighter();
			return;
		}
	}
}

function ArenaWin::End()
{
	$Arena::Winners = false;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.inArena)
		{
			%cl.isArenaWin = "";
			%cl.inArenaWin = "";
			%cl.clArenaWin = "";
			%cl.isArenaLoser = "";
			%player = Client::getOwnedObject(%cl);
			if(isObject(%player))
			{
				//playNextAnim(%cl);
				//Player::kill(%cl);
			}
			else
			{
				Game::playerSpawn(%cl, true);
				//playNextAnim(%cl);
				//Player::kill(%cl);
			}
		}
	}
}

function ArenaWin::ResetStage()
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.inArena)
		{
			%cl.inArenaWin = "";
			%player = Client::getOwnedObject(%cl);
			if(isObject(%player))
			{
				playNextAnim(%cl);
				Player::kill(%cl);
			}
		}
	}
	
	ArenaWin::SelectFighter();
}

$debugaw = false;

function ArenaWin::SelectFighter()
{
	%arenanum = 0;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.inArena)
		{
			%arenanum++;
		}
	}
	
	if(%arenanum == 1)
	{
		ArenaWin::End();
		ArenaMSG(1,"Not enough players to do Arena Winners.~wfemale1.wbelay.wav");
		return;
	}

	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.inArena)
		{				
			if(%cl.isArenaWin) 
			{
				%arenawin = true;
				break;
			}
			else
			{
				%arenawin = false;
			}
		}
	}
	
	%i = -1;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.inArena)
		{
			if($debugaw)
			{
				echo(%cl.isArenaWin@" << %cl.isArenaWin");
				echo(%cl.isArenaLoser@" << %cl.isArenaLoser");
				echo(%arenawin@" << %arenawin");
			}
			
			if(%cl.isArenaWin == "" && %cl.isArenaLoser == "" && %arenawin) //idkkkkkkk
			{
				if($debugaw)
					echo(%cl@" << 111");
				
				$ArenaWin::Random[%i++] = %cl;
			}
			else if(%cl.isArenaLoser == "")
			{
				if($debugaw)
					echo(%cl@" << 222");
				
				%clientId = %cl;
				%arenawin = true;
			}
			else
			{
				if(%arenanum > 2)
				{
					if($debugaw)
						echo(%cl@" << 333");
				
					%cl.isArenaLoser = "";
				}
				else
				{
					if($debugaw)
						echo(%cl@" << 444");
				
					$ArenaWin::Random[%i++] = %cl;
					%cl.isArenaLoser = "";
				}
			}
		}
	}
	
	%i++;
	%rnd = Floor(GetRandom()*%i);
	%cl = $ArenaWin::Random[%rnd];
	
	if($debugaw)
	{
		echo(%i@" << %i");
		echo(%rnd@" << %rnd");
		echo(%cl@" << %cl");
		echo(Client::getName(%cl)@" << name");
	}

	ArenaMSG(1,Client::getName(%cl)@" was selected to fight next at random.~wcapturedtower.wav");
	
	%cl.inArenaWin = true;
	%cl.clArenaWin = %clientId;
	%clientId.inArenaWin = true;
	%clientId.clArenaWin = %cl;
	ArenaWin::StartFight(%cl);
	ArenaWin::StartFight(%clientId);
	
	deleteVariables("ArenaWin::Random*");
}

function ArenaWin::StartFight(%clientId)
{
	Game::playerSpawn(%clientId, true);
	%clientId.arenajug = true;
	TA::BlackOut(%clientId,10);
	schedule("bottomprint("@%clientId@", \"<f1><jc>Match starts in 5 seconds.\", 1);", 1);
	schedule("bottomprint("@%clientId@", \"<f1><jc>Match starts in 4 seconds.\", 1);", 2);
	schedule("bottomprint("@%clientId@", \"<f1><jc>Match starts in 3 seconds.\", 1);", 3);
	schedule("bottomprint("@%clientId@", \"<f1><jc>Match starts in 2 seconds.\", 1);", 4);
	schedule("bottomprint("@%clientId@", \"<f1><jc>Match starts in 1 seconds.\", 1);", 5);
	schedule("Client::sendMessage("@%clientId@", 0, \"~wduellaugh.wav\");", 5);
	schedule("Client::sendMessage("@%clientId@", 0, \"~wduelfight.wav\");", 6);
	schedule("bottomprint("@%clientId@", \"<f1><jc>Fight!!\", 5);", 6);
	schedule(%clientId @".arenajug = false;", 7);
	Game::refreshClientScore(%clientId);
}

function ArenaWin::Finish(%clientId,%cl)
{
	if($debug)
		echo("ArenaWin::Finish("@%clientId@")");
	
	if(%cl == false)
	{
		%cl = %clientId;
		%clientId = %clientId.clArenaWin;
	}
	//%clientId.arenajug = true;
	//%cl.arenajug = true;
	%cl.isArenaLoser = true;
	%cl.isArenaWin = "";
	%clientId.isArenaWin = true;
	%cl.inArenaWin = "";
	%cl.clArenaWin = "";
	%clientId.inArenaWin = "";
	%clientId.clArenaWin = "";
	%namew = Client::getName(%clientId);
	%namel = Client::getName(%cl);
	playASound(%clientId, %cl);
	TA::BlackOut(%clientId,5);
	TA::BlackOut(%cl,5);
	schedule("centerprint("@%clientId@", \"<jc><f1>You beat "@%namel@" in a Duel !!\", 5);", 1);
	schedule("centerprint("@%cl@", \"<jc><f1>You lost to "@%namew@" in a Duel !!\", 5);", 1);
	//schedule(%clientId @".arenajug = false;", 5);
	schedule("ArenaWin::ResetStage();", 5);
}
