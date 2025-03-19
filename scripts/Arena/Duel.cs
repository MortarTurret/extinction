// 			Duel Admin Commands:
//				#duel			-	Will bring up a menu of commands, easier than using chat
//				#duel join		-	Will teleport you into the duel
//				#duel delete		-	Will remove the duel and disable it
//				#duel <name>		-	Will load the duel you specify replacing <name>, see default below
//				#duel getpos <dsc>	-	Will export the offset of your player inside of the duel to temp\Duel_Vecs.cs
//							-	<dsc> is a one word identifier
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//$Duel::Spawn[0] = "-1000 -1000 350"; // hopefully this isnt out of the maps gravity range...
//$Duel::Spawn[1] = "-1000 1000 350";
//$Duel::Spawn[2] = "1000 -1000 350";
//$Duel::Spawn[3] = "1000 1000 350";
$Duel::Initialized = true;
$debugduel = false;

function Duel::getnum()
{
	%num = 0;
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.inDuel)
		   %num++;
	}
	echo("Duel Players: "@%num);
	return %num;
}

//

function Duel::Finish(%clientId)
{
	if($debugduel)
		echo("Duel::Finish("@%clientId@")");
	%killerId = $Dueling[%clientId];
	%killerId.arenajug = true;
	%clientId = $Dueling[%killerId];
	%namew = Client::getName(%killerId);
	%namel = Client::getName(%clientId);
	playASound(%killerId, %clientId);
	$DuelLastEnemy[%killerId] = %clientId;
	$DuelLastEnemy[%clientId] = %killerId;
	$DuelLastArmor[%killerId] = %killerId.DuelArmor;
	$DuelLastArmor[%clientId] = %clientId.DuelArmor;
	TA::BlackOut(%killerId,3);
	TA::BlackOut(%clientId,3);
	schedule("centerprint("@%killerId@", \"<jc><f1>You beat "@%namel@" in a Duel!!\", 5);", 1);
	schedule("centerprint("@%clientId@", \"<jc><f1>You lost to "@%namew@" in a Duel!!\", 5);", 1);
	schedule("Duel::Leave("@%killerId@");", 5);
	schedule("Duel::Leave("@%clientId@");", 5);
	schedule("Duel::FastReset("@%killerId@");", 14);
	schedule("Duel::FastReset("@%clientId@");", 14);
	$Dueling[%killerId] = false;
	$Dueling[%clientId] = false;
}

function Duel::FastReset(%clientId)
{
	if($debugduel)
		echo("Duel::FastReset("@%clientId@") && %clientId.inDuel == "@%clientId.inDuel);
	//if(!%clientId.inDuel)
	//{
		//%killerId = $DuelLastEnemy[%clientId];
		//$DuelLastArmor[%killerId] = "";
		$DuelLastArmor[%clientId] = "";
		//$DuelLastEnemy[%killerId] = "";
		$DuelLastEnemy[%clientId] = "";
	//}
}

function DuelMenu(%clientId, %sel) 
{
	if(!%clientId.inDuel && !%sel.inDuel)
	{
		%name = Client::getName(%sel);
		Client::buildMenu(%clientId, "Manage "@%name, "DuelMenu", true);
		if(!%sel.inDuel)
			Client::addMenuItem(%clientId, %curItem++ @ "Request Duel", "reqduel " @ %sel);
	}
}

function Duel::Opts(%clientId, %sel) 
{
	if(!%clientId.inDuel && !%sel.inDuel)
	{
		%name = Client::getName(%sel);
		Client::buildMenu(%clientId, "Duel Options "@%name, "DuelMenu", true);
		if(!$TALT::Active)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Anni Spawn", "anniduel " @ %sel);
			Client::addMenuItem(%clientId, %curItem++ @ "Anni Builder", "builderduel " @ %sel);
			Client::addMenuItem(%clientId, %curItem++ @ "Anni Titan", "titanduel " @ %sel);
			Client::addMenuItem(%clientId, %curItem++ @ "Elite Spawn", "eliteduel " @ %sel);
			Client::addMenuItem(%clientId, %curItem++ @ "Base Spawn", "baseduel " @ %sel);
		}
		else
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Anni Mod", "anniduel " @ %sel);
			Client::addMenuItem(%clientId, %curItem++ @ "Elite Mod", "eliteduel " @ %sel);
			Client::addMenuItem(%clientId, %curItem++ @ "Base Mod", "baseduel " @ %sel);
		}
	}
}

function processMenuDuelMenu(%clientId, %option)
{
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);
	
	//DuelMenu
	if(%opt == "reqduel")
	{
		Duel::Opts(%clientId, %cl);
		return;
	}
	//DuelOpts
	%name = Client::getName(%clientId);
	%namecl = Client::getName(%clientId);
	if(%opt == "anniduel")
	{
		if(%cl.DuelRequest == %clientId)
		{
			client::sendmessage(%clientId, 0, "Please wait 15 second before sending another request."); 
			return;
		}
		if(%clientId.DuelRequest == %cl)
		{
			client::sendmessage(%clientId, 0, "Please pick an option for your request first."); 
			return;
		}
		%clientId.DuelArmor = "AnniSpawn";
		%cl.DuelArmor = "AnniSpawn";
		%cl.DuelRequest = %clientId;
		client::sendmessage(%clientId, 0, "Your request has been sent."); 
		client::sendmessage(%cl, 0, %name@" has sent you a Duel request.");
		Centerprint(%cl,"<jc><f1>"@%name@" has sent you a Duel request.",5); 
		schedule("Duel::Expired("@%clientId@","@%cl@");", 15);
		//Duel::Request(%cl, %clientId);
		return;
	}
	else if(%opt == "eliteduel")
	{
		if(%cl.DuelRequest == %clientId)
		{
			client::sendmessage(%clientId, 0, "Please wait 15 second before sending another request."); 
			return;
		}
		if(%clientId.DuelRequest == %cl)
		{
			client::sendmessage(%clientId, 0, "Please pick an option for your request first."); 
			return;
		}
		%clientId.DuelArmor = "EliteSpawn";
		%cl.DuelArmor = "EliteSpawn";
		%cl.DuelRequest = %clientId;
		client::sendmessage(%clientId, 0, "Your request has been sent."); 
		client::sendmessage(%cl, 0, %name@" has sent you a Duel request.");
		Centerprint(%cl,"<jc><f1>"@%name@" has sent you a Duel request.",5);
		schedule("Duel::Expired("@%clientId@","@%cl@");", 15);
		//Duel::Request(%cl, %clientId);
		return;
	}
	else if(%opt == "baseduel")
	{
		if(%cl.DuelRequest == %clientId)
		{
			client::sendmessage(%clientId, 0, "Please wait 15 second before sending another request."); 
			return;
		}
		if(%clientId.DuelRequest == %cl)
		{
			client::sendmessage(%clientId, 0, "Please pick an option for your request first."); 
			return;
		}
		%clientId.DuelArmor = "BaseSpawn";
		%cl.DuelArmor = "BaseSpawn";
		%cl.DuelRequest = %clientId;
		client::sendmessage(%clientId, 0, "Your request has been sent."); 
		client::sendmessage(%cl, 0, %name@" has sent you a Duel request.");
		Centerprint(%cl,"<jc><f1>"@%name@" has sent you a Duel request.",5);
		schedule("Duel::Expired("@%clientId@","@%cl@");", 15);
		//Duel::Request(%cl, %clientId);
		return;
	}
	else if(%opt == "builderduel")
	{
		if(%cl.DuelRequest == %clientId)
		{
			client::sendmessage(%clientId, 0, "Please wait 20 second before sending another request."); 
			return;
		}
		if(%clientId.DuelRequest == %cl)
		{
			client::sendmessage(%clientId, 0, "Please pick an option for your request first."); 
			return;
		}
		%clientId.DuelArmor = "BuilderSpawn";
		%cl.DuelArmor = "BuilderSpawn";
		%cl.DuelRequest = %clientId;
		client::sendmessage(%clientId, 0, "Your request has been sent."); 
		client::sendmessage(%cl, 0, %name@" has sent you a Duel request.");
		Centerprint(%cl,"<jc><f1>"@%name@" has sent you a Duel request.",5);
		schedule("Duel::Expired("@%clientId@","@%cl@");", 15);
		//Duel::Request(%cl, %clientId);
		return;
	}
	else if(%opt == "titanduel")
	{
		if(%cl.DuelRequest == %clientId)
		{
			client::sendmessage(%clientId, 0, "Please wait 15 second before sending another request."); 
			return;
		}
		if(%clientId.DuelRequest == %cl)
		{
			client::sendmessage(%clientId, 0, "Please pick an option for your request first."); 
			return;
		}
		%clientId.DuelArmor = "TitanSpawn";
		%cl.DuelArmor = "TitanSpawn";
		%cl.DuelRequest = %clientId;
		client::sendmessage(%clientId, 0, "Your request has been sent."); 
		client::sendmessage(%cl, 0, %name@" has sent you a Duel request.");
		Centerprint(%cl,"<jc><f1>"@%name@" has sent you a Duel request.",5);
		schedule("Duel::Expired("@%clientId@","@%cl@");", 15);
		//Duel::Request(%cl, %clientId);
		return;
	}
	//DuelRequest
	if(%opt == "aduel")
	{
		if(!%cl.inDuel && Client::getteam(%cl) == -1 && %clientId.DuelArmor != "" && %clientId.DuelRequest == %cl)
		{
			$Duel::Spawn = TA::pickWaypoint();
			client::sendmessage(%cl, 0, %name@" has accepted your request.");
			Messageall(0,Client::getName(%cl)@" and "@%name@" are about to Duel."); 
			$Dueling[%clientId] = %cl;
			$Dueling[%cl] = %clientId;
			Duel::Join(%clientId);
			Duel::Join(%cl);
			return;
		}
		else
		{
			client::sendmessage(%clientId, 0, "This player cannot Duel right now."); 
			Duel::Expired(%cl,%clientId);
		}
	}
	else if(%opt == "dduel")
	{
		if(!%clientId.inDuel && %clientId.DuelRequest == %cl)
		{
			Duel::FastReset(%clientId);
			%clientId.DuelArmor = "";
			%clientId.DuelRequest = "";
		}
		if(!%cl.inDuel)
		{
			Duel::FastReset(%cl);
			client::sendmessage(%cl, 0, %name@" has declined your request to Duel."); 
			%cl.DuelArmor = "";
		}
		return;
	}
}

function Duel::Expired(%clientId,%cl)
{
	if(!%clientId.inDuel && %cl.DuelArmor != "")
	{
		%clientId.DuelArmor = "";
		Duel::FastReset(%clientId);
		client::sendmessage(%clientId, 0, "Request to Duel not accepted in time.");
	}
	if(!%cl.inDuel && %cl.DuelRequest == %clientId)
	{
		%cl.DuelArmor = "";
		%cl.DuelRequest = "";
		Duel::FastReset(%cl);
		client::sendmessage(%cl, 0, "Duel not accepted in time."); 
	}
}

function Duel::Request(%sel, %clientId)
{
	if(!%clientId.inDuel && !%sel.inDuel)
	{
		%name = Client::getName(%clientId);
		if(%clientId.DuelArmor == "AnniSpawn")
			Client::buildMenu(%sel, "Anni Spawn Duel "@%name, "DuelMenu", true);
		else if(%clientId.DuelArmor == "EliteSpawn")
			Client::buildMenu(%sel, "Elite Spawn Duel "@%name, "DuelMenu", true);
		else if(%clientId.DuelArmor == "BaseSpawn")
			Client::buildMenu(%sel, "Base Spawn Duel "@%name, "DuelMenu", true);
		else if(%clientId.DuelArmor == "BuilderSpawn")
			Client::buildMenu(%sel, "Builder Duel "@%name, "DuelMenu", true);
		else if(%clientId.DuelArmor == "TitanSpawn")
			Client::buildMenu(%sel, "Titan Duel "@%name, "DuelMenu", true);
		Client::addMenuItem(%sel, %curItem++ @ "Decline "@%name@"'s Duel", "dduel " @ %clientId);
		Client::addMenuItem(%sel, %curItem++ @ "Accept "@%name@"'s Duel", "aduel " @ %clientId);
	}
}

function DuelMSG(%words)
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.inDuel)
		   Client::sendMessage(%cl,0,%words);
	}
}

function Duel::Clear()
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.inDuel)
		   Duel::Leave(%cl);
	}	
}

function Duel::Leave(%clientId)//,%mapchange) // fixed to send player to observer
{
	//%player = Client::getOwnedObject(%clientId);
	//%cl = Player::getClient(%clientId);
	if($debugduel)
		echo("Duel::Leave("@%clientId@","@%mapchange@") && %clientId.inDuel == "@%clientId.inDuel);

	//if(!%mapchange)
	//{
		//Messageall(0,Client::getName(%clientId)@" has left the Duel. (#duel)~wshell_click.wav");
		%clientId.inDuel = false;
		%clientId.arenajug = false;
		%clientId.DuelArmor = "";
		%clientId.DuelArmor = "";
		%clientId.DuelRequest = "";
		//%cl.DuelFirstShot = false;
		//if(isObject(%player))
		//{
			//if($Duel::NoForceTeam)
			//{
				//$Duel::NoForceTeam = false;
			//}
			//else
			//{
				//playNextAnim(%clientId);
				//Player::kill(%clientId);
				//Game::playerSpawn(%clientId, true);
			//}
		//}
		$Dueling[%clientId] = "";
		Observer::enterObserverMode(%clientId);
		Game::refreshClientScore(%clientId);
	//}
	//Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
}

function Duel::Join(%clientId)
{
	%player = Client::getOwnedObject(%clientId);
	
	if(%clientId.isDuelBanned) return;

	if(%clientId.inDuel) return;
	
	//%duelnum = Duel::getnum();
	
	//if(%duelnum == 2)
	//{
	//	$Duel::Spawn[0]
	//}
	
	if(player::status(%clientId) == "(Observing)")
	{
		//echo("teamchange");
		Duel::JoinObs(%clientId);
	}
	else
	{
		//echo("join");
		client::sendmessage(%cl, 1, "You must be in Observer to Duel.");
	}
	
	
}

//fix that damn no join if dead and obs
function Duel::JoinObs(%clientId)
{
	schedule("processMenuPickTeam("@%clientId@", -1);", 0.1); 
	schedule("Duel::JoinTwo(" @ %clientId @ ");", 0.2);
}

function Duel::Strip(%clientId)
{
	echo("preparing for duel!!");
	//Strip them weapons 
	%player = Client::getOwnedObject(%clientId);
	%Player.rThrow = true;
	%player.rThStr = 10;
	for(%x = 0; %x < 15; %x = %x++)
	{		
		%item = Player::getMountedItem(%clientId,$WeaponSlot);
		//if(!%item) return;
		Player::trigger(%player, $WeaponSlot, false);
		Player::dropItem(%clientId,%item);
		remoteNextWeapon(%clientId);
		
		//Mines too
		%mine = "MineAmmo";
		%count = Player::getItemCount(%clientId, %mine);
		if(%count > 0)
			Player::dropItem(%clientId,%mine);
	}
	%Player.rThrow = "";
	%player.rThStr = "";
}

function Duel::JoinTwo(%clientId)
{
	//echo("join2");
	%player = Client::getOwnedObject(%clientId);
	if(isObject(%player) && getObjectType(%player) == "Player")
	{
		//echo("join3");
		%spawn = Duel::getnum();

		if(%spawn == 8)
		{
			Centerprint(%clientId,"<jc><f1>The Duel is full please wait.",6);
			return;
		}

		%clientId.inDuel = true;
		//%clientId.intTeam = GameBase::getTeam(%clientId);
		Duel::Strip(%clientId);

		//%spawnPos = GameBase::getPosition(%spawn);
		//%spawnRot = GameBase::getRotation(%spawn);

		//Item::setVelocity(%player,"0 0 0");
		//GameBase::setPosition(%player,%spawnPos);
		//GameBase::setRotation(%player,%spawnRot);

		Client::setInitialTeam(%clientId,0);
		GameBase::setTeam(%clientId,0);
		//GameBase::setTeam(%player,0);

		//Now lets respawn them
		playNextAnim(%clientId);
		player::kill(%clientId);
		Game::playerSpawn(%clientId, true);
		freeze(%clientId);
		TA::BlackOut(%clientId,12);
		schedule("centerprint("@%clientId@", \"<f1><jc>Duel starts in 10 seconds.\", 1);", 1);
		schedule("centerprint("@%clientId@", \"<f1><jc>Duel starts in 9 seconds.\", 1);", 2);
		schedule("centerprint("@%clientId@", \"<f1><jc>Duel starts in 8 seconds.\", 1);", 3);
		schedule("centerprint("@%clientId@", \"<f1><jc>Duel starts in 7 seconds.\", 1);", 4);
		schedule("centerprint("@%clientId@", \"<f1><jc>Duel starts in 6 seconds.\", 1);", 5);
		schedule("centerprint("@%clientId@", \"<f1><jc>Duel starts in 5 seconds.\", 1);", 6);
		schedule("centerprint("@%clientId@", \"<f1><jc>Duel starts in 4 seconds.\", 1);", 7);
		schedule("centerprint("@%clientId@", \"<f1><jc>Duel starts in 3 seconds.\", 1);", 8);
		schedule("centerprint("@%clientId@", \"<f1><jc>Duel starts in 2 seconds.\", 1);", 9);
		schedule("centerprint("@%clientId@", \"<f1><jc>Duel starts in 1 seconds.\", 1);", 10);
		schedule("Client::sendMessage("@%clientId@", 0, \"~wduellaugh.wav\");", 10);
		schedule("Client::sendMessage("@%clientId@", 0, \"~wduelfight.wav\");", 11);
		schedule("centerprint("@%clientId@", \"<f1><jc>Fight!!\", 1);", 11);
		schedule("freeze(" @ %clientId @",true );", 12);
		Game::refreshClientScore(%clientId);

		//Messageall(0,Client::getName(%clientId)@" has entered the Duel."); 
	}
	else
	   Centerprint(%clientId,"<jc><f1>The Duel is loading please wait.",6);
}

function playASound(%clientId, %foeId) 
{
	if (Gamebase::GetDamageLevel(Client::GetOwnedObject(%foeId)) == 0) 
	{
		%s[0, 0] = "flawless";
		%s[1, 0] = "flawless";
		%sid = 1;
	
		if(floor(getRandom() * 15) == 10) 
		{ 
			if(floor(getRandom() * 10) > 5) 
			{ 
				if(floor(getRandom() * 10) > 5) 
				{ 
					%s[0, 0] = "outstanding";
					%s[1, 0] = "outstanding";
				} 
				else 
				{
					%s[0, 0] = "superb";
					%s[1, 0] = "superb";
				}
			} 
			else 
			{							
				if(floor(getRandom() * 10) > 5) 
				{ 
					if(floor(getRandom() * 10) > 5) 
					{ 
						%s[0, 0] = "welldone";
						%s[1, 0] = "welldone";
					} 
					else 
					{
						%s[0, 0] = "excellent";
						%s[1, 0] = "excellent";
					}
				}
				else 
				{
					if(floor(getRandom() * 10) > 5) 
					{ 
						%s[0, 0] = "fatality";
						%s[1, 0] = "fatality";
					} 
					else 
					{
						%s[0, 0] = "toasty";
						%s[1, 0] = "toasty";
					}
				}
			}
		}
	}
	for(%ii = 0; %ii <= 2; %ii++)
		if(%s[0, %ii] != "")
			schedule("Client::sendMessage("@%foeId@",0,\"~wduel" @ %s[0, %ii] @ ".wav\");", ((1.4*%ii)+1));
	for(%ii = 0; %ii <= 2; %ii++)
		if(%s[1, %ii] != "")
			schedule("Client::sendMessage("@%clientId@",0,\"~wduel" @ %s[1, %ii] @ ".wav\");", ((1.4*%ii)+1));
}

function GetOffSetRot(%offset,%rot,%pos)
{
	%x = getWord(%offset,0);
	%y = getWord(%offset,1);
	%z = getWord(%offset,2);
	%posA = Vector::add(%pos,Vector::getFromRot(Vector::Add(%rot,"0 0 -1.570796327"),%x));
	%posB = Vector::add(%posA,Vector::getFromRot(%rot,%y));
	%posC = Vector::add(%posB,Vector::getFromRot(Vector::Add(%rot,"1.570796327 0 0"),%z));
	return %posC;
}

//%n = -1;
//$Duel::name["map"] = "Map";
//$Duel::canBuild["map"] = true;
//$Duel::object["map",%n++] = "hoverpost.dis 0 10 -210 0 0 0";
//$Duel::object["map",%n++] = "hoverpost.dis 0 -10 -210 0 0 0";
//$Duel::object["map",%n++] = "spawn3 0 10 0 0 -0 -3";
//$Duel::object["map",%n++] = "spawn3 0 -10 0 0 -0 -0";
//%n = -1;