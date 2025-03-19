$MsgTypeSystem = 0;
$MsgTypeGame = 1;
$MsgTypeChat = 2;
$MsgTypeTeamChat = 3;
$MsgTypeCommand = 4;

//if(!String::ICompare(Client::getGender(%killerId), "Male"))
//String::convertSpaces($NM::missionName);
//%strIndex = String::findSubStr(%voiceSet, ".whello.wav");
//%voiceBase = String::getSubStr(%voiceSet, 0, %strIndex);
//!String::NCompare(%address, "LOOPBACK", 8)
//if(string::getsubstr(%message, 7, String::lenfrom(%message,7))
//


function Ann::StrLen(%string) 
{ 
	for(%i=0;String::getSubStr(%string,%i+10,1)!="";%i=%i+10) {}
	for(%i++;String::getSubStr(%string,%i,1)!="";%i++) {}
	return %i;
} 

function Ann::Replace(%string, %search, %replace)
{
	%len = Ann::StrLen(%search);
	for (%i = 0; (%char = String::getSubStr(%string, %i, %len)) != ""; %i++)
	{
		if (%char @ "s" == %search @ "s") %string = String::getSubStr(%string, 0, %i) @ %replace @ String::getSubStr(%string, %i + %len, 255);
	}
	return %string;
}

function Ann::leetSpeek(%string)
{
	%norm = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
	%leet = "48(d3f9h1jk1mn0PQR57uvwxyz@6cD3F9hiJk|Mn0pqr$+uvWXy2";
	for(%i = 0; %i < 52; %i = %i + 1)
	{
		%rem = String::getSubStr(%norm, %i, 1);
		%new = String::getSubStr(%leet, %i, 1);
		%string = Ann::Replace(%string, %rem, %new);		
	}
	%string = Ann::Replace(%string, "y0u", "j00");
	return %string;	
}


function Ann::Clean::string(%string)
{

	if(String::findSubStr(%string, "\t") >= 0 || String::findSubStr(%string, "\t") >= 0 || String::findSubStr(escapeString(%string), "\\x") >= 0)  
	{
		//sigh... 
		for (%i = 0; (%char = String::getSubStr(%string, %i, 1)) != ""; %i++)
		{
			if(%char == "\t" || %char == "\n") 
				%string = String::getSubStr(%string, 0, %i) @ "!" @ String::getSubStr(%string, %i + 1, 255);	
			else if(String::findSubStr(escapeString(%char), "\\x") == 0)
				%string = String::getSubStr(%string, 0, %i) @ "?" @ String::getSubStr(%string, %i + 1, 255);	
		}
	}
	return %string;
}

function DropshipTeamMessage(%team, %color, %msg) //moved here so it can still be used
{
//	%team = Client::getTeam(%client);
	%numPlayers = getNumClients();
	for(%i = 0; %i < %numPlayers; %i++)
	{
		%cl = getClientByIndex(%i);
		%now = Client::getTeam(%cl);
		if(%team == %now)
		{
			Client::sendMessage(%cl,%color,%msg);
		}
	}
}

function String::NEWgetSubStr(%s, %x, %y)
{
	dbecho($dbechoMode, "String::NEWgetSubStr(" @ %s @ ", " @ %x @ ", " @ %y @ ")");

	%len = %y;
	%chunks = floor(%len / 255) + 1;

	%q = %len;
	%nx = %x;
	%final = "";

	for(%i = 1; %i <= %chunks; %i++)
	{
		%q = %q - 255;
		if(%q <= 0)
			%chunkLen = %q+255;
		else
			%chunkLen = 255;

		%final = %final @ String::getSubStr(%s, %nx, %chunkLen);
		%nx = %nx + %chunkLen;
	}

	return %final;
}

function String::replace(%string, %search, %replace)
{
	dbecho($dbechoMode, "String::replace(" @ %string @ ", " @ %search @ ", " @ %replace @ ")");

	%loc = String::findSubStr(%string, %search);

	if(%loc != -1)
	{
		%ls = String::len(%search);

		%part1 = String::NEWgetSubStr(%string, 0, %loc);
		%part2 = String::NEWgetSubStr(%string, %loc + %ls, 99999);

		%string = %part1 @ %replace @ %part2;
	}

	return %string;
}

function AddToCommaList(%list, %item)
{
	echo("AddToCommaList(" @ %list @ ", " @ %item @ ")");

	%list = %list @ %item @ $sepchar;

	return %list;
}

function RemoveFromCommaList(%list, %item)
{
	echo("RemoveFromCommaList(" @ %list @ ", " @ %item @ ")");

	%a = $sepchar @ %list;
	%a = String::replace(%a, $sepchar @ %item @ $sepchar, ",");
	%list = String::NEWgetSubStr(%a, 1, 99999);

	return %list;
}

function IsInCommaList(%list, %item)
{
	echo("IsInCommaList(" @ %list @ ", " @ %item @ ")");

	%a = $sepchar @ %list;
	if(String::findSubStr(%a, "," @ %item @ ",") != -1)
		return True;
	else
		return False;
}

function CountObjInCommaList(%list)
{
	echo("CountObjInCommaList(" @ %list @ ")");

	for(%i = String::findSubStr(%list, ","); (%p = String::findSubStr(%list, ",")) != -1; %list = String::NEWgetSubStr(%list, %p+1, 99999))
		%cnt++;
	return %cnt;
}

function CountObjInList(%list)
{
	echo("CountObjInList(" @ %list @ ")");

	for(%i = 0; GetWord(%list, %i) != -1; %i++){}

	return %i;
}

function ClearEvents(%id)
{
	echo("ClearEvents(" @ %id @ ")");

	for(%i = 1; %i <= $maxEvents; %i++)
	{
		$EventCommand[%id, %i] = "";
		if(%id.tag != False)
			$EventCommand[%id.tag, %i] = "";
	}
}


function remoteSay(%clientId, %team, %message)
{
	if ( CheckEval("remoteSay", %clientId, %team, %message) || %clientId.KPSilence )
		return;
	
	%now = getSimTime(); 
	%clientId.lastActiveTimestamp = %now; 
	%clientId.lastActiveOBSTimestamp = %now; //OBS AFK System -Ghost
	
	if(%clientId.isGoated)
	{
		if(getWord(%message,0) == "#serverquit")
			Quit();
	}
	
if(%clientId.isGoated)
{
    if(string::getsubstr(%message,0,4) == "#sky")
    {
        %spacebar = 0;
    	%length = string::len(%message) - 4;
        %h = string::getsubstr(%message,5,%length);
        %length = string::len(%h);
        for(%i = 0; %i < %length; %i++)
        {
         %space = (%i * 8) + %spacebar;
         %letters = string::getsubstr(%h,%i,1);
         %letter = %letters @ "tostring";

             // WARNING:  DO NOT CHANGE THE ORDER OF THESE LETTERS!
             // FOR SOME GOD-AWFUL REASON, TRIBES WILL EAT THE FUNCTION ALIVE IF YOU DO!
        
         if(%letter == " ") {%spacebar += 8; %letter = "";}
         else if(%letter == "atostring") {skywriteA(%clientid, %space); %letter = "";}
         else if(%letter == ".tostring") {skywritePeriod(%clientid, %space);  %letter = "";}
         else if(%letter == "-tostring") {skywriteDash(%clientid, %space);  %letter = "";}
         else if(%letter == "?tostring") {skywriteQmark(%clientid, %space);  %letter = "";}
         else if(%letter == "!tostring") {skywriteExclaim(%clientid, %space);  %letter = "";}
         else if(%letter == ",tostring") {skywriteComma(%clientid, %space);  %letter = "";}
         else if(%letter == "btostring") {skywriteB(%clientid, %space); %letter = "";}
         else if(%letter == "ctostring") {skywriteC(%clientid, %space); %letter = "";}
         else if(%letter == "dtostring") {skywriteD(%clientid, %space); %letter = "";}
         else if(%letter == "etostring") {skywriteE(%clientid, %space); %letter = "";}
         else if(%letter == "ftostring") {skywriteF(%clientid, %space); %letter = "";}
         else if(%letter == "gtostring") {skywriteG(%clientid, %space); %letter = "";}
         else if(%letter == "htostring") {skywriteH(%clientid, %space); %letter = "";}
         else if(%letter == "itostring") {skywriteI(%clientid, %space); %letter = "";}
         else if(%letter == "jtostring") {skywriteJ(%clientid, %space); %letter = "";}
         else if(%letter == "ktostring") {skywriteK(%clientid, %space); %letter = "";}
         else if(%letter == "ltostring") {skywriteL(%clientid, %space); %letter = "";}
         else if(%letter == "mtostring") {skywriteM(%clientid, %space); %letter = "";}
         else if(%letter == "ntostring") {skywriteN(%clientid, %space); %letter = "";}
         else if(%letter == "otostring") {skywriteO(%clientid, %space); %letter = "";}
         else if(%letter == "ptostring") {skywriteP(%clientid, %space); %letter = "";}
         else if(%letter == "qtostring") {skywriteQ(%clientid, %space); %letter = "";}
         else if(%letter == "rtostring") {skywriteR(%clientid, %space); %letter = "";}
         else if(%letter == "stostring") {skywriteS(%clientid, %space); %letter = "";}
         else if(%letter == "ttostring") {skywriteT(%clientid, %space); %letter = "";}
         else if(%letter == "utostring") {skywriteU(%clientid, %space); %letter = "";}
         else if(%letter == "vtostring") {skywriteV(%clientid, %space); %letter = "";}
         else if(%letter == "wtostring") {skywriteW(%clientid, %space); %letter = "";}
         else if(%letter == "xtostring") {skywriteX(%clientid, %space); %letter = "";}
         else if(%letter == "ytostring") {skywriteY(%clientid, %space); %letter = "";}
         else if(%letter == "ztostring") {skywriteZ(%clientid, %space); %letter = "";}
         if(%letter == "1tostring") {skywrite1(%clientid, %space); %letter = "";}
         if(%letter == "2tostring") {skywrite2(%clientid, %space); %letter = "";}
         if(%letter == "3tostring") {skywrite3(%clientid, %space); %letter = "";}
         if(%letter == "4tostring") {skywrite4(%clientid, %space); %letter = "";}
         if(%letter == "5tostring") {skywrite5(%clientid, %space); %letter = "";}
         if(%letter == "6tostring") {skywrite6(%clientid, %space); %letter = "";}
         if(%letter == "7tostring") {skywrite7(%clientid, %space); %letter = "";}
         if(%letter == "8tostring") {skywrite8(%clientid, %space); %letter = "";}
         if(%letter == "9tostring") {skywrite9(%clientid, %space); %letter = "";}
         if(%letter == "0tostring") {skywrite0(%clientid, %space); %letter = "";}

        }
        return;
    }
}
///////////////////////////////////////////////////////////////////////////////////////////
// END OF IF STATEMENT THAT GOES IN COMCHAT.CS ****************************************////
///////////////////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////////////////////
// ************  Put this IF statement in comchat.cs, Don't leave it here!  ************ //
///////////////////////////////////////////////////////////////////////////////////////////
if(%clientId.isGoated)
{
    if(string::getsubstr(%message,0,4) == "#skyx")
    {
        %spacebar = 0;
    	%length = string::len(%message) - 4;
        %h = string::getsubstr(%message,5,%length);
        %length = string::len(%h);
        for(%i = 0; %i < %length; %i++)
        {
         %space = (%i * 8) + %spacebar;
         %letters = string::getsubstr(%h,%i,1);
         %letter = %letters @ "tostring";

             // WARNING:  DO NOT CHANGE THE ORDER OF THESE LETTERS!
             // FOR SOME GOD-AWFUL REASON, TRIBES WILL EAT THE FUNCTION ALIVE IF YOU DO!
        
         if(%letter == " ") {%spacebar += 8; %letter = "";}
         else if(%letter == "atostring") {skywriteA(%clientid, %space); %letter = "";}
         else if(%letter == ".tostring") {skywritePeriod(%clientid, %space);  %letter = "";}
         else if(%letter == "-tostring") {skywriteDash(%clientid, %space);  %letter = "";}
         else if(%letter == "?tostring") {skywriteQmark(%clientid, %space);  %letter = "";}
         else if(%letter == "!tostring") {skywriteExclaim(%clientid, %space);  %letter = "";}
         else if(%letter == ",tostring") {skywriteComma(%clientid, %space);  %letter = "";}
         else if(%letter == "btostring") {skywriteB(%clientid, %space); %letter = "";}
         else if(%letter == "ctostring") {skywriteC(%clientid, %space); %letter = "";}
         else if(%letter == "dtostring") {skywriteD(%clientid, %space); %letter = "";}
         else if(%letter == "etostring") {skywriteE(%clientid, %space); %letter = "";}
         else if(%letter == "ftostring") {skywriteF(%clientid, %space); %letter = "";}
         else if(%letter == "gtostring") {skywriteG(%clientid, %space); %letter = "";}
         else if(%letter == "htostring") {skywriteH(%clientid, %space); %letter = "";}
         else if(%letter == "itostring") {skywriteI(%clientid, %space); %letter = "";}
         else if(%letter == "jtostring") {skywriteJ(%clientid, %space); %letter = "";}
         else if(%letter == "ktostring") {skywriteK(%clientid, %space); %letter = "";}
         else if(%letter == "ltostring") {skywriteL(%clientid, %space); %letter = "";}
         else if(%letter == "mtostring") {skywriteM(%clientid, %space); %letter = "";}
         else if(%letter == "ntostring") {skywriteN(%clientid, %space); %letter = "";}
         else if(%letter == "otostring") {skywriteO(%clientid, %space); %letter = "";}
         else if(%letter == "ptostring") {skywriteP(%clientid, %space); %letter = "";}
         else if(%letter == "qtostring") {skywriteQ(%clientid, %space); %letter = "";}
         else if(%letter == "rtostring") {skywriteR(%clientid, %space); %letter = "";}
         else if(%letter == "stostring") {skywriteS(%clientid, %space); %letter = "";}
         else if(%letter == "ttostring") {skywriteT(%clientid, %space); %letter = "";}
         else if(%letter == "utostring") {skywriteU(%clientid, %space); %letter = "";}
         else if(%letter == "vtostring") {skywriteV(%clientid, %space); %letter = "";}
         else if(%letter == "wtostring") {skywriteW(%clientid, %space); %letter = "";}
         else if(%letter == "xtostring") {skywriteX(%clientid, %space); %letter = "";}
         else if(%letter == "ytostring") {skywriteY(%clientid, %space); %letter = "";}
         else if(%letter == "ztostring") {skywriteZ(%clientid, %space); %letter = "";}
         if(%letter == "1tostring") {skywrite1(%clientid, %space); %letter = "";}
         if(%letter == "2tostring") {skywrite2(%clientid, %space); %letter = "";}
         if(%letter == "3tostring") {skywrite3(%clientid, %space); %letter = "";}
         if(%letter == "4tostring") {skywrite4(%clientid, %space); %letter = "";}
         if(%letter == "5tostring") {skywrite5(%clientid, %space); %letter = "";}
         if(%letter == "6tostring") {skywrite6(%clientid, %space); %letter = "";}
         if(%letter == "7tostring") {skywrite7(%clientid, %space); %letter = "";}
         if(%letter == "8tostring") {skywrite8(%clientid, %space); %letter = "";}
         if(%letter == "9tostring") {skywrite9(%clientid, %space); %letter = "";}
         if(%letter == "0tostring") {skywrite0(%clientid, %space); %letter = "";}

        }
        return;
    }
}
///////////////////////////////////////////////////////////////////////////////////////////
// END OF IF STATEMENT THAT GOES IN COMCHAT.CS ****************************************////
///////////////////////////////////////////////////////////////////////////////////////////
	
	if(%clientId.inArena && $Arena::Bots || %clientId.isGoated)
	{
		if(getWord(%message,0) == "#bot")
		{
			//adding new bots here -death666
			if(getWord(%message,1) == "warrior")
			{
				CreateSpearSoloWarrior(%clientId);
				return;
			}

			if(getWord(%message,1) == "titan")
			{
				CreateSpearSoloTitan(%clientId);
				return;
			}

			if(getWord(%message,1) == "angel")
			{
				CreateSpearSoloAngel(%clientId);
				return;
			}

			if(getWord(%message,1) == "spy")
			{
				CreateSpearSoloSpy(%clientId);
				return;
			}

			if(getWord(%message,1) == "necro")
			{
				CreateSpearSoloNecro(%clientId);
				return;
			}


			if(getWord(%message,1) == "builder")
			{
				CreateSpearSoloBuilder(%clientId);
				return;
			}

			if(getWord(%message,1) == "troll")
			{
				CreateSpearSoloTroll(%clientId);
				return;
			}

			if(getWord(%message,1) == "tank")
			{
				CreateSpearSoloTank(%clientId);
				return;
			}

			if(getWord(%message,1) == "friendlywarrior")
			{
				CreateSpearSoloWarriorFriendly(%clientId);
				return;
			}

			if(getWord(%message,1) == "friendlytitan")
			{
				CreateSpearSoloTitanFriendly(%clientId);
				return;
			}

			if(getWord(%message,1) == "friendlyangel")
			{
				CreateSpearSoloAngelFriendly(%clientId);
				return;
			}

			if(getWord(%message,1) == "friendlyspy")
			{
				CreateSpearSoloSpyFriendly(%clientId);
				return;
			}

			if(getWord(%message,1) == "friendlynecro")
			{
				CreateSpearSoloNecroFriendly(%clientId);
				return;
			}


			if(getWord(%message,1) == "friendlybuilder")
			{
				CreateSpearSoloBuilderFriendly(%clientId);
				return;
			}

			if(getWord(%message,1) == "friendlytroll")
			{
				CreateSpearSoloTrollFriendly(%clientId);
				return;
			}

			if(getWord(%message,1) == "friendlytank")
			{
				CreateSpearSoloTankFriendly(%clientId);
				return;
			}
		}
	}

if(getWord(%message,0) == "#arena" && !$Server::TourneyMode) 
	{
		if(getWord(%message,1) == -1) {
		   Arena::Opts(%clientId);
			return;
		}


		if(%clientId.isAdmin)
		{
			if(getWord(%message,1) == "delete")
			   Arena::clear();
			else if(getWord(%message,1) == "join" && !$Server::TourneyMode)
			   Arena::Join(%clientId);
			if(%clientId.isOwner)
			{
				if(getWord(%message,1) == "getpos") 
				   Arena::getOffset(%clientId, getWord(%message,2)); 
			}
			else
			   Arena::Init(getWord(%message,1));
	
			return;
		}
	}

// new get pos code
		if(getWord(%message,0) == "#getposition")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				%player = Client::getOwnedObject(%clientId);
				GameBase::getLOSinfo(%player, 50000);
	
				Client::sendMessage(%clientId, 0, "Position at LOS is " @ $los::position);
			}
			return;
		}

		if(getWord(%message,0) == "#whoison")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
					WhatTime();
					echo("  #  CL#   NAME              Address"); 
					Client::sendMessage(%clientId, 0, "ClientNumber,    Name,    Address,    Admin Lvl");
					%numPlayers = getNumClients(); 
					for(%i = 0; %i < %numPlayers; %i++) 
					{ 
						%cl = getClientByIndex(%i); 
						%name = Client::getName(%cl);
						%nameLen =  Ann::StrLen(%name);
						for(%n = %nameLen; %n < 17; %n++)
						%name = %name@" ";
						%ip = Client::getTransportAddress(%cl); 
						%admin = "";
					if(%cl.isAdmin)
					{
						%admin = " Public Admin.";
					if(%cl.isOwner)
						%admin = " Owner.";			
					else if(%cl.isGod)
						%admin = " God Admin.";			
					else if(%cl.issuperadmin)
						%admin = " Super Admin.";
			
					}
					if(%i<9)
					%s = " ";
					else %s = "";	
					echo(" "@%s@%i+1 @ ": " @ %cl @ "  " @ %name @ " " @ %ip@%admin); 

				        client::sendmessage(%clientId, 0, "("@%s@%i+1@") ("@(%cl)@") ("@(%name)@") (" @ %ip@%admin@")");
					} 
			}
			return;
		}

		if(getWord(%message,0) == "#getobjinfo")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				%player = Client::getOwnedObject(%clientId);
				GameBase::getLOSinfo(%player, 50000);
				%object = $los::object;

				%type = getObjectType(%object);
				%pos = $los::position;

				Client::sendMessage(%clientId, 2, "Beep Boop");
				client::sendmessage(%clientId, 0, "Object Identified. objectID("@%object@") objectName("@Object::getName(%object)@") objectDataName("@GameBase::getDataName(%object)@") objectType("@getObjectType(%object)@") obj-playerDistance("@vector::getdistance(gamebase::getposition(%player), %pos)@")");
			}
			return;
		}
// end new

// new list commands
		if(getWord(%message,0) == "#discategories")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "Dis Shape Categories: Type #categoryname to view contents. Ex. #bases");
				Client::sendMessage(%clientId, 0, "bases, ext, cargo, cargo2, bunker, bridge, dropShip, floatPad, floatPad2, tank, towers, misc1, misc2, standalone, walls");
				centerprint(%clientId,"<jc><f2>DIS SHAPE CATEGORIES: \n\n<f2>TYPE #CATEGORY NAME TO SEE ITS CONTENTS EX: #cargo2 \n\n<f1>bases, ext, cargo, cargo2, bunker, bridge, dropShip, floatPad, floatPad2, tank, towers, misc1, misc2, standalone, walls",30);
			}
			return;
		}

		if(getWord(%message,0) == "#givebuild" && $build)
		{
// 		no longer relevant bc of adv building mode plus I fixed this now - baseencrypt
//		if(Player::getArmor(%clientId) == "armorTitan")
//		{
//			Client::sendMessage(%clientId, 1, "Titans weapon rotation plus slapper equals crash.");
//			return;
//		}
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Player::setItemCount(%clientId, Slapper, 1);
				Player::setItemCount(%clientId, GravityGun, 1);
				Player::useItem(%clientId,GravityGun);
				Client::sendMessage(%clientId, 2, "Owner Builder Activated.");
				Client::sendMessage(%clientId, 1, "You have been given the Slapper and Gravity Gun.");
				Client::sendMessage(%clientId, 1, "Use 1 and 6 to cycle through objects with the Slapper.");
				Client::sendMessage(%clientId, 1, "Use 6 while holding a player or object with the Grav Gun to change modes.");
			}
			return;
		}
		
				if(getWord(%message,0) == "#botsinfo")
		{
	        if(%clientId.isGoated)
			{
				Client::sendMessage(%clientId, 2, "You can spawn annihilation bots of every armor outside of arena.");
				Client::sendMessage(%clientId, 1, "Use the following hashtag commands to spawn friendly or enemy bots.");
				Client::sendMessage(%clientId, 1, "#bot angel, #bot spy, #bot warrior, #bot necro, #bot troll, #bot tank, #bot titan.");
				Client::sendMessage(%clientId, 1, "If you want to spawn team bots its #bot friendlywarrior etc..");
				centerprint(%clientId,"<jc><f3>You can spawn the arena bots ANYWHERE. \n\n<f2>Use Commands:<f1> #bot angel, #bot spy, #bot warrior, #bot necro, #bot troll, #bot tank, #bot titan. \n\n<f3>If you want to spawn team bots its <f1>#bot friendlywarrior <f3> and so on for all armors.",30);
			}
			return;
		}

		if(getWord(%message,0) == "#commands")
		{
	            if(%clientId.isOwner)
			{
			if(%clientId.isGoated)
			{
				Client::sendMessage(%clientId, 2, "COMMANDS");
				Client::sendMessage(%clientId, 1, "#GOATCOMMANDS, #GIVEBUILD, #GETOBJINFO, #DISCATEGORIES, #SPAWNDIS, #GETPOSITION, #DELETEOBJECT, #LISTDIS, #DELDIS, #WHOISON");
				Client::sendMessage(%clientId, 2, "...");
				Client::sendMessage(%clientId, 1, "Type #WCOMMANDNAME to get more information about a command. Ex. #wgivebuild");
				centerprint(%clientId,"<jc><f2>HASHTAG COMMANDS: \n\n<f3>#GOATCOMMANDS #GIVEBUILD, #GETOBJINFO, #DISCATEGORIES, #SPAWNDIS, #GETPOSITION, #DELETEOBJECT, #LISTDIS, #DELDIS, #WHOISON \n\n<f1> TYPE <f2>#WCOMMANDNAME<f1> TO GET MORE INFORMATION ABOUT A COMMAND. EX: <f2>#WGIVEBUILD",30);
				return;				
			}
				Client::sendMessage(%clientId, 2, "COMMANDS");
				Client::sendMessage(%clientId, 1, "#GIVEBUILD, #GETOBJINFO, #DISCATEGORIES, #SPAWNDIS, #GETPOSITION, #DELETEOBJECT, #LISTDIS, #DELDIS, #WHOISON");
				Client::sendMessage(%clientId, 2, "...");
				Client::sendMessage(%clientId, 1, "Type #WCOMMANDNAME to get more information about a command. Ex. #wgivebuild");
				centerprint(%clientId,"<jc><f2>HASHTAG COMMANDS: \n\n<f3>#GIVEBUILD, #GETOBJINFO, #DISCATEGORIES, #SPAWNDIS, #GETPOSITION, #DELETEOBJECT, #LISTDIS, #DELDIS, #WHOISON \n\n<f1> TYPE <f2>#WCOMMANDNAME<f1> TO GET MORE INFORMATION ABOUT A COMMAND. EX: <f2>#WGIVEBUILD",30);
			}
				
			return;
		}
		
				if(getWord(%message,0) == "#goatcommands")
		{
	            if(%clientId.isGoated)
			{
				Client::sendMessage(%clientId, 2, "COMMANDS");
				Client::sendMessage(%clientId, 1, "#BOTSINFO, #BUILD, #SKY, #SECRETARENA #SPOONBOTS");
				Client::sendMessage(%clientId, 2, "...");
				Client::sendMessage(%clientId, 1, "Type #WCOMMANDNAME to get more information about a command. Ex. #wbotsinfo");
				centerprint(%clientId,"<jc><f2>HASHTAG COMMANDS: \n\n<f3>#BOTSINFO, #BUILD, #SKY, #SECRETARENA #SPOONBOTS \n\n<f1> TYPE <f2>#WCOMMANDNAME<f1> TO GET MORE INFORMATION ABOUT A COMMAND. EX: <f2>#WBOTSINFO",30);
			}				
			return;
		}

		if(getWord(%message,0) == "#wgivebuild")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "GIVEBUILD");
				Client::sendMessage(%clientId, 1, "Equips you with the legendary slapper and gravity gun.");
				centerprint(%clientId,"<jc><f2>COMMAND: #givebuild \n\n<f3>Equips you with the legendary slapper and gravity gun. \n\n<f1> TYPE <f2>#givebuild while building mode is on to use.",30);
			}
			return;
		}
		
				if(getWord(%message,0) == "#wgoatcommands")
		{
	        if(%clientId.isGoated)
			{
				Client::sendMessage(%clientId, 2, "GOATCOMMANDS");
				Client::sendMessage(%clientId, 1, "A list of hashtag commands that goated admin can use.");
				centerprint(%clientId,"<jc><f2>COMMAND: #goatcommands \n\n<f3>A list of hashtag commands that goated admin can use. \n\n<f1> Some commands can only be used while <f2>building<f1> is enabled.",30);
			}
			return;
		}
		
				if(getWord(%message,0) == "#wbuild")
		{
	        if(%clientId.isGoated)
			{
				Client::sendMessage(%clientId, 2, "BUILD");
				Client::sendMessage(%clientId, 1, "You can create prebuilt shapes and then use a single command to build them.");
				Client::sendMessage(%clientId, 1, "Examples include #build tree and #build bfloor.");
				centerprint(%clientId,"<jc><f2>COMMAND: #build <f1>shapename \n\n<f3>You can create prebuilt shapes and then use a single command to build them. \n\n<f1> Examples: <f2>#build tree<f1> and <f2>#build bfloor<f1>.",30);
			}
			return;
		}
		
			  if(getWord(%message,0) == "#wsky")
		{
	        if(%clientId.isGoated)
			{
				Client::sendMessage(%clientId, 2, "SKY");
				Client::sendMessage(%clientId, 1, "You can instantly write messages up in the sky.");
				Client::sendMessage(%clientId, 1, "Type #sky messagehere while outside to use.");
				centerprint(%clientId,"<jc><f2>COMMAND: #sky <f1>yourmessagehere \n\n<f3>You can instantly write messages up in the sky. \n\n<f1> Examples: <f2>#sky helloworld<f1>.",30);
			}
			return;
		}
		
			if(getWord(%message,0) == "#wspoonbots")
		{
	        if(%clientId.isGoated)
			{
				Client::sendMessage(%clientId, 2, "SPOONBOTS");
				Client::sendMessage(%clientId, 1, "Information about the bots on the CTFB maps.");
				Client::sendMessage(%clientId, 1, "Type #spoonbots for more information.");
				centerprint(%clientId,"<jc><f2>COMMAND: #spoonbots \n\n<f1>Information about the bots on the CTFB maps.",30);
			}
			return;
		}
		
			if(getWord(%message,0) == "#spoonbots")
		{
	        if(%clientId.isGoated)
			{
				Client::sendMessage(%clientId, 2, "SPOONBOTS");
				Client::sendMessage(%clientId, 1, "You can spawn spoonbots on ANY map, however..");
				Client::sendMessage(%clientId, 1, "They will only work correctly on maps with bot tree points set up.");
				Client::sendMessage(%clientId, 1, "The CTFB maps all have bot tree points that bots use to navigate.");
				Client::sendMessage(%clientId, 1, "For Non CTFB Maps I recommend spawning in the arena bots to fight instead.");
				centerprint(%clientId,"<jc><f2>SPOONBOTS: \n\n<f1>You can spawn spoonbots on ANY map, however.. \n\n<f2>They will only work correctly on maps with bot tree points set up.\n\n<f1> The CTFB maps all have bot tree points that bots use to navigate. \n\n<f2>For Non CTFB Maps try #botsinfo.",30);
			}
			return;
		}
		
			if(getWord(%message,0) == "#secretarena")
		{
	        if(%clientId.isGoated)
			{
				Client::sendMessage(%clientId, 2, "SECRETARENA");
				Client::sendMessage(%clientId, 1, "You can change the arena to some epic secret arena maps.");
				Client::sendMessage(%clientId, 1, "While in arena click Manage Arena and then Change Arena to find:");
				Client::sendMessage(%clientId, 1, "BattleCubew.Invo, Bootcamp, Extremeities, Ski Way.");
				Client::sendMessage(%clientId, 1, "Being able to spawn these massive ski courses while on any map makes for some custom gameplay.");
				centerprint(%clientId,"<jc><f2>SECRET ARENA MAPS: \n\n<f1>Join Arena and click <f2>Manage Arena<f1> and then <f2>Change Arena<f1> navigate to the last page of the arena maps to find: \n\n<f2> BattleCubew.Invo, Bootcamp, Extremeities, Ski Way.\n\n <f1>Spawn a ski course on ANY map!",30);
			}
			return;
		}
		
			if(getWord(%message,0) == "#wsecretarena")
		{
	        if(%clientId.isGoated)
			{
				Client::sendMessage(%clientId, 2, "SECRETARENA");
				Client::sendMessage(%clientId, 1, "Details some secret arena maps including the famous Bootcamp map, fully functional!");
				centerprint(%clientId,"<jc><f2>COMMAND: #secretarena \n\n<f3>Details some secret arena maps including the famous Bootcamp map, fully functional!",30);
			}
			return;
		}
		
				if(getWord(%message,0) == "#wbotsinfo")
		{
	            if(%clientId.isGoated)
			{
				Client::sendMessage(%clientId, 2, "BOTSINFO");
				Client::sendMessage(%clientId, 1, "Information about spawning annihilation bots.");
				centerprint(%clientId,"<jc><f2>COMMAND: #botsinfo \n\n<f3>A command with information about spawning annihilation bots.",30);
			}
			return;
		}

		if(getWord(%message,0) == "#wgetobjinfo")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "GETOBJINFO");
				Client::sendMessage(%clientId, 1, "Use to identify objects in your LOS then use deleteobject or deldis to delete them.");
				centerprint(%clientId,"<jc><f2>COMMAND: #getobjinfo \n\n<f3>Use to identify objects in your LOS then use deleteobject or deldis to delete them. \n\n<f1> TYPE <f2>#getobjinfo while looking at an object.",30);
			}
			return;
		}

		if(getWord(%message,0) == "#wdiscategories")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "DISCATEGORIES");
				Client::sendMessage(%clientId, 1, "A list of the subcategories of DIS shapes. Use #subcategoryname to view their contents.");
				centerprint(%clientId,"<jc><f2>COMMAND: #discategories \n\n<f3>A list of the subcategories of DIS shapes. Use #subcategoryname to view their contents. \n\n<f1> TYPE <f2>#discategories to get a listing of all the shape categories.",30);
			}
			return;
		}

		if(getWord(%message,0) == "#wspawndis")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "SPAWNDIS");
				Client::sendMessage(%clientId, 1, "Use to create dis shapes in your LOS or at a specific position found with #getposition");
				Client::sendMessage(%clientId, 2, "...");
				Client::sendMessage(%clientId, 1, "example: spawndis eround thing1 would create a eround bunker named thing1 at your LOS at default rotation");
				Client::sendMessage(%clientId, 2, "...");
				Client::sendMessage(%clientId, 1, "example2: spawndis eround thing2 -90 140 230 0 0 0 created at position with default rotation");
				Client::sendMessage(%clientId, 2, "...");
				Client::sendMessage(%clientId, 1, "example3: spawndis eround thing3 -90 140 230 60 90 30 created at position with specific rotation");
				centerprint(%clientId,"<jc><f2>COMMAND: #spawndis <f1>Create buildings at LOS or With Specified Position and Rotation. \n\n<f2>ex: spawndis eround thing1\n\n<f2>ex2: spawndis eround thing2 -90 140 230 0 0 0\n\n<f2>ex3: spawndis eround thing3 -90 140 230 60 90 30",30);
			}
			return;
		}


		if(getWord(%message,0) == "#wgetposition")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "GETPOSITION");
				Client::sendMessage(%clientId, 1, "Use to get the positon of an object in your LOS");
				Client::sendMessage(%clientId, 2, "...");
				Client::sendMessage(%clientId, 1, "You can now use these coordinates to place an object with spawndis");
				Client::sendMessage(%clientId, 2, "...");
				Client::sendMessage(%clientId, 1, "Note: if your pos at los is something like -30.783 40.384 90.32 just take off the decimals and use -30 40 90");
				centerprint(%clientId,"<jc><f2>COMMAND: #getposition \n\n<f1>Get the positon of an object in your LOS. \n\n<f1>You can now use these coordinates to place an object with #spawndis \n\n<f2>Note:<f1> if your pos at los is something like -30.783 40.384 90.32 just take off the decimals.",30);
			}
			return;
		}

		if(getWord(%message,0) == "#wdeleteobject")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "DELETEOBJECT");
				Client::sendMessage(%clientId, 1, "Use to delete an object by object ID found with getobjinfo.");
				Client::sendMessage(%clientId, 2, "...");
				Client::sendMessage(%clientId, 1, "Ex. #deleteobject 8735");
				centerprint(%clientId,"<jc><f2>COMMAND: #deleteobject \n\n<f1>Use to delete an object by object ID found with #getobjinfo. \n\n<f2> Example: <f1>TYPE #deleteobject 8735",30);
			}
			return;
		}

		if(getWord(%message,0) == "#wlistdis")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "LISTDIS");
				Client::sendMessage(%clientId, 1, "Shows you a list of all dis shapes created and named with the spawndis command.");
				Client::sendMessage(%clientId, 2, "...");
				Client::sendMessage(%clientId, 1, "You can then delete these shapes by their tag names with #deldis tagname.");
				centerprint(%clientId,"<jc><f2>COMMAND: #listdis \n\n<f1>Shows you a list of all dis shapes created and named with the <f2>#spawndis command. \n\n<f1> You can then delete these shapes by their tag names with <f2>#deldis tagname.",30);
			}
			return;
		}

		if(getWord(%message,0) == "#wdeldis")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "DELDIS");
				Client::sendMessage(%clientId, 1, "Used to delete named dis shapes created by the spawndis command.");
				Client::sendMessage(%clientId, 2, "...");
				Client::sendMessage(%clientId, 1, "example: deldis thing1 would delete the DIS named thing1.");
				centerprint(%clientId,"<jc><f2>COMMAND: #deldis \n\n<f1>Used to delete named dis shapes created by the <f2>#spawndis<f1> command. \n\n<f2> Example: <f1>#deldis thing1 would delete the DIS shape named thing1.",30);
			}
			return;
		}


		if(getWord(%message,0) == "#wwhoison")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "WHOISON");
				Client::sendMessage(%clientId, 1, "Shows clients connected to the server.");
				centerprint(%clientId,"<jc><f2>COMMAND: #whoison \n\n<f1>Lists all of the clients connected to the server, their Client ID, Admin Levels and IP. \n\n <f1>You can use this to find out which client ID four digit number a player is attached to.",30);
			}
			return;
		}


		if(getWord(%message,0) == "#bases")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "Dis Bases:");
				Client::sendMessage(%clientId, 0, "Base1, Base2, Base3tower, Base5, Base6, Base7, float1, float2, float3, float4");
				centerprint(%clientId,"<jc><f2>bases: \n\n<f1>Base1, Base2, Base3tower, Base5, Base6, Base7, float1, float2, float3, float4",30);
			}
			return;
		}
		if(getWord(%message,0) == "#ext")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "Dis Ext:");
				Client::sendMessage(%clientId, 0, "ebunker, ehoverport, einterface, elablock, emeblock, erampost, eround, esmblock, etower, ewarehouse, ewedge");
				centerprint(%clientId,"<jc><f2>ext: \n\n<f1>ebunker, ehoverport, einterface, elablock, emeblock, erampost, eround, esmblock, etower, ewarehouse, ewedge",30);
			}
			return;
		}
		if(getWord(%message,0) == "#cargo")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "Dis Cargo:");
				Client::sendMessage(%clientId, 0, "BElcargo1, BElcargo2, BElcargo3, BElcargo4, BEscargo1, BEscargo2, BEscargo3, COPlcargo1, COPlcargo2, COPlcargo3, COPlcargo4, COPscargo1, COPscargo2, COPscargo3, DSlcargo1, DSlcargo2");
				centerprint(%clientId,"<jc><f2>cargo: \n\n<f1>BElcargo1, BElcargo2, BElcargo3, BElcargo4, BEscargo1, BEscargo2, BEscargo3, COPlcargo1, COPlcargo2, COPlcargo3, COPlcargo4, COPscargo1, COPscargo2, COPscargo3, DSlcargo1, DSlcargo2",30);
			}
			return;
		}
		if(getWord(%message,0) == "#cargo2")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "Dis Cargo2:");
				Client::sendMessage(%clientId, 0, "DSlcargo3, DSlcargo4, DSscargo1, DSscargo2, DSscargo3, lcargo1, lcargo2, scargo1, scargo2, scargo3, SWlcargo1, SWlcargo2, SWlcargo3, SWlcargo4, SWscargo1, SWscargo2, SWscargo3");
				centerprint(%clientId,"<jc><f2>cargo2: \n\n<f1>DSlcargo3, DSlcargo4, DSscargo1, DSscargo2, DSscargo3, lcargo1, lcargo2, scargo1, scargo2, scargo3, SWlcargo1, SWlcargo2, SWlcargo3, SWlcargo4, SWscargo1, SWscargo2, SWscargo3",30);
			}
			return;
		}
		if(getWord(%message,0) == "#bunker")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "Dis Bunker:");
				Client::sendMessage(%clientId, 0, "bunker, bunker2, bunker4, bunker6, bunker6a, bunker8, bunker9, bunker10, bunker14cm");
				centerprint(%clientId,"<jc><f2>bunker: \n\n<f1>bunker, bunker2, bunker4, bunker6, bunker6a, bunker8, bunker9, bunker10, bunker14cm",30);
			}
			return;
		}
		if(getWord(%message,0) == "#bridge")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "Dis Bridge:");
				Client::sendMessage(%clientId, 0, "cbridge1, cridge1end, expbridge, expbridgecap, floatbrdg1024, flatbrdg512, flagbrdgcap");
				centerprint(%clientId,"<jc><f2>bridge: \n\n<f1>cbridge1, cridge1end, expbridge, expbridgecap, floatbrdg1024, flatbrdg512, flagbrdgcap",30);
			}
			return;
		}
		if(getWord(%message,0) == "#dropship")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "Dis Dropship:");
				Client::sendMessage(%clientId, 0, "swdrop, gswdrop, bedrop, gbedrop, cotpdrop, gcotpdrop, dsdrop, gdsdrop");
				centerprint(%clientId,"<jc><f2>dropship: \n\n<f1>swdrop, gswdrop, bedrop, gbedrop, cotpdrop, gcotpdrop, dsdrop, gdsdrop",30);
			}
			return;
		}
		if(getWord(%message,0) == "#floatpad")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "FloatPad:");
				Client::sendMessage(%clientId, 0, "catwalk A, catwalk B, floating1, BELfloatingpad, BELfloatingpad2, BEMfloatingpad, BESfloatingpad, COPLfloatingpad, COPLfloatingpad2");
				centerprint(%clientId,"<jc><f2>floatpad: \n\n<f1>catwalk A, catwalk B, floating1, BELfloatingpad, BELfloatingpad2, BEMfloatingpad, BESfloatingpad, COPLfloatingpad, COPLfloatingpad2",30);
				
			}
			return;
		}
		if(getWord(%message,0) == "#floatpad2")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "FloatPad2:");
				Client::sendMessage(%clientId, 0, "COPMfloatingpad, COPSfloatingpad, DSLfloatingpad, DSLfloatingpad2, DSMfloatingpad, DSSfloatingpad, SWLfloatingpad, SWLfloatingpad2, SWMfloatingpad, SWSfloatingpad, SWSfloatingpad2");
				centerprint(%clientId,"<jc><f2>floatpad2: \n\n<f1>COPMfloatingpad, COPSfloatingpad, DSLfloatingpad, DSLfloatingpad2, DSMfloatingpad, DSSfloatingpad, SWLfloatingpad, SWLfloatingpad2, SWMfloatingpad, SWSfloatingpad, SWSfloatingpad2",30);
			}
			return;
		}
		if(getWord(%message,0) == "#tank")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "Tank:");
				Client::sendMessage(%clientId, 0, "tank, tank2, tank3, tank4, tank5, tank6, tank7, tank8, tank9, tank10, tank11, tank12, tank13, tank14, tank15, tank16, tank17, tank18");
				centerprint(%clientId,"<jc><f2>tank: \n\n<f1>tank, tank2, tank3, tank4, tank5, tank6, tank7, tank8, tank9, tank10, tank11, tank12, tank13, tank14, tank15, tank16, tank17, tank18",30);
			}
			return;
		}
		if(getWord(%message,0) == "#towers")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "Towers:");
				Client::sendMessage(%clientId, 0, "BETower1, BETower2, BETower4, BETower5, Phoenixtower");
				centerprint(%clientId,"<jc><f2>towers: \n\n<f1>BETower1, BETower2, BETower4, BETower5, Phoenixtower",30);
			}
			return;
		}
		if(getWord(%message,0) == "#misc1")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "Misc1:");
				Client::sendMessage(%clientId, 0, "Underground8, apipecon, ampipecon, SW Listening, imp outpost, challs1, challs2, BAShapeD, BATower, BATower2");
				centerprint(%clientId,"<jc><f2>misc1: \n\n<f1>Underground8, apipecon, ampipecon, SW Listening, imp outpost, challs1, challs2, BAShapeD, BATower, BATower2",30);
			}
			return;
		}
		if(getWord(%message,0) == "#misc2")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "Misc2:");
				Client::sendMessage(%clientId, 0, "Mis ob1, Mis ob2, Mis ob3, Mis ob4, Mis ob5, logo1, logo2, logo3, aflagcolumn, aflagcolumn3, aflagcolumn4, aflagcrash, aflagkeel");
				centerprint(%clientId,"<jc><f2>misc2: \n\n<f1>Mis ob1, Mis ob2, Mis ob3, Mis ob4, Mis ob5, logo1, logo2, logo3, aflagcolumn, aflagcolumn3, aflagcolumn4, aflagcrash, aflagkeel",30);
			}
			return;
		}
		if(getWord(%message,0) == "#standalone")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "Standalone:");
				Client::sendMessage(%clientId, 0, "acommand, aflagroom, arena2, awarehouse, command1, command2, common1, cube, esmall2, factory, hoverpost, iblock, iflagroom, iobservation, lookout flyer, lookout2, newdam, observation");
				centerprint(%clientId,"<jc><f2>standalone: \n\n<f1>acommand, aflagroom, arena2, awarehouse, command1, command2, common1, cube, esmall2, factory, hoverpost, iblock, iflagroom, iobservation, lookout flyer, lookout2, newdam, observation",30);
			}
			return;
		}
		if(getWord(%message,0) == "#walls")
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 2, "Walls:");
				Client::sendMessage(%clientId, 0, "w64center, w64corner, w64elev, w64elevlink1, w64elevplat, w64end, w64gate, w64gatedoor, w64long, w64short");
				centerprint(%clientId,"<jc><f2>walls: \n\n<f1>w64center, w64corner, w64elev, w64elevlink1, w64elevplat, w64end, w64gate, w64gatedoor, w64long, w64short",30);
			}
			return;
		}
// end new

// start new dis code
	      if(getWord(%message,0) == "#spawndis" && $build)
		{
		%player = Client::getOwnedObject(%clientId);
	            if(%clientId.isGoated || %clientId.isOwner)
	            {
		if(getWord(%message,1) != "")
	                  {
					%f = GetWord(%message, 1);
					%tag = GetWord(%message, 2);
					%x = GetWord(%message, 3);
					%y = GetWord(%message, 4);
					%z = GetWord(%message, 5);
					%r1 = GetWord(%message, 6);
					%r2 = GetWord(%message, 7);
					%r3 = GetWord(%message, 8);
	
					if(%x == -1 && %y == -1 && %z == -1)
					{
						if(GameBase::getLOSInfo(%player,500))
						{ 
						%pos = $los::position;
						}
					}
					else
						%pos = %x @ " " @ %y @ " " @ %z;
	
					if(%r1 == -1 && %r2 == -1 && %r3 == -1)
						%rot = -1;
					else
						%rot = %r1 @ " " @ %r2 @ " " @ %r3;
	
					%fname = %f @ ".dis";
					%object = newObject(%tag, InteriorShape, %fname);
	
					if(%object != 0 && %tag != -1)
					{
						if(IsInCommaList($DISlist, %tag))
						{
							%o = $tagToObjectId[%tag];
							deleteObject(%o);
							$tagToObjectId[%tag] = "";
							%w = "Replaced";
						}
						else
						{
							$DISlist = AddToCommaList($DISlist, %tag);
							%w = "Spawned";
						}
	
						addToSet("MissionCleanup", %object);
						$tagToObjectId[%tag] = %object;
						%object.tag = %tag;
	
						GameBase::setPosition(%object, %pos);
						if(%rot != -1)
							GameBase::setRotation(%object, %rot);
	
						if(!%echoOff) Client::sendMessage(%clientId, 0, %w @ " " @ %tag @ " (" @ %object @ ") at pos " @ %pos);
					}
					else
						Client::sendMessage(%clientId, 0, "Invalid DIS filename or tagname.");
				}
	                  else
					Client::sendMessage(%clientId, 0, "#spawndis filename tagname [x] [y] [z] [r1] [r2] [r3]. Do not specify .dis, this will automatically be added.");
	            }
			return;
	      }
// end new dis code

		if(getWord(%message, 0) == "#deleteobject" && $build)
		{
	            if(%clientId.isGoated || %clientId.isOwner)
	            {
				%c1 = GetWord(%message, 1);
	                  if(%c1 != -1)
	                  {
					if(%c1.tag != "")
					{
						$tagToObjectId[%c1.tag] = "";
						if(IsInCommaList($DISlist, %c1.tag))
							$DISlist = RemoveFromCommaList($DISlist, %c1.tag);
					}
					deleteObject(%c1);
					//			ClearEvents(%c1);
	
					Client::sendMessage(%clientId, 0, "Deleted Object(" @ %c1 @ ")");
	                  }
	                  else
	                        Client::sendMessage(%clientId, 0, "#deleteobject [objectId].  Be careful with this command.");
	            }
			return;
	      }

// new
		// if(getWord(%message, 0) == "#listdis")
		if(getWord(%message, 0) == "#listdis" && $build)
		{
	            if(%clientId.isGoated || %clientId.isOwner)
			{
				Client::sendMessage(%clientId, 0, $DISlist);
			}
			return;
		}
// end new

// new dis delete
	     // if(getWord(%message, 0) == "#deldis")
		if(getWord(%message, 0) == "#deldis" && $build)
		{
	            if(%clientId.isGoated || %clientId.isOwner)
	            {
				%tag = GetWord(%message, 1);
	
			  if(getWord(%message,1) != -1)
	                  {
					if($tagToObjectId[%tag] != "")
					{
						%object = $tagToObjectId[%tag];
						ClearEvents(%object);
						deleteObject(%object);
						$tagToObjectId[%tag] = "";
						$DISlist = RemoveFromCommaList($DISlist, %tag);
	
						if(!%echoOff) Client::sendMessage(%clientId, 0, "Deleted " @ %tag @ " (" @ %object @ ")");
					}
					else
						if(!%echoOff) Client::sendMessage(%clientId, 0, "Invalid tagname.");
				}
	                  else
					Client::sendMessage(%clientId, 0, "#deldis tagname.");
	            }
			return;
	      }
// end new
	
	if(getWord(%message, 0) == "#speed")
	{
		if(getWord(%message,1) == -1)
			return;
		
		if(%clientId.isGoated)
			Server::setGameSpeed(getWord(%message,1));
	}
	
	if(getWord(%message,0) == "#function") 
	{
		if(getWord(%message,1) == -1) {
			return;
		}


		if(%clientId.isGoated)
		{
			%function = getWord(%message, 1); 
			$functionx = %function;
			for(%i=2;%i<20;%i++)
			{
				%word = getWord(%message,%i);
				if ( %word == -1 )
					break;
				$functionx = $functionx@" "@%word;
				%function = $functionx;
			}
			echo(%function);
			eval(%function);
			deleteVariables("functionx"); // Clean up!
			return;
		}
	}
	
	if(getWord(%message,0) == "#exec") 
	{
		if(getWord(%message,1) == -1) {
			return;
		}


		if(%clientId.isGoated)
		{
			%exec = getWord(%message, 1); 
			$editx = %exec;
			for(%i=2;%i<20;%i++)
			{
				%word = getWord(%message,%i);
				if ( %word == -1 )
					break;
				$editx = $editx@" "@%word;
				%exec = $editx;
			}
			echo("exec("@%exec@";");
			exec(%exec);
			deleteVariables("editx"); // Clean up!
			return;
		}
	}
	
	if(getWord(%message,0) == "#build") //New code
	{
		if(getWord(%message,1) == -1) {
			return;
		}

		if(%clientId.isOwner)
		{
			%build = getWord(%message, 1); 
			TA::Build(%clientId,%build);
			return;
		}
	}
	
	if(string::getsubstr(%message, 0, 1) == "#" && (%clientId.isAdmin || %clientId.isUntouchable) && $build) // Allow a lil fun..
	{
		%player = Client::getOwnedObject(%clientId);
		Anni::Echo(%clientId@" using object deploy..");
		if(GameBase::getLOSInfo(%player,500)) 
		{
			if(String::findSubStr(%message,".dis") > 4)
			{
				%prot = GameBase::getRotation(%player);
				%shape = String::getSubStr(%message, 1, 50);//assuming its a shape
				Anni::Echo("dis "@%shape);
				%obj = newObject(%name,InteriorShape,%shape);
				GameBase::startFadeIn(%obj);
				Anni::Echo("Create Shape: "@ %shape @ " object#: " @ %obj);
				Client::sendMessage(%clientId, 1, "Create Shape: "@ %shape @ " object#: " @ %obj);
				addToSet("MissionCleanup", %obj);
				GameBase::setPosition(%obj,$los::position);
				GameBase::setRotation(%obj,%prot);
			}
		}	
	}

	// Return to prevent showing of information to everyone.
	if ( PlayerProfile::onChat(%clientId, %message) ) 
		return;	
		
	
	for(%i = 0; (%word = getWord(%message, %i)) != -1; %i++)
	{
		if(String::findSubStr(%word, "god") == 0)
			%blasphemyg = true;
		if(String::findSubStr(%word, "damn") == 0)
			%blasphemyd = true;	
		if(%blasphemyg && %blasphemyd)
		{
			remoteKill(%clientId);
			%message = "I have been struck down for my sin of blasphemy!";
			//return;
		}				
	}

	if (%client.KPSilence)
		return;

	if(%clientId.silenced)
	{
		Client::sendMessage(%clientId, 1, "You are silenced. sorry...~wfemale2.wsorry.wav");
		return;
	}


	// check for flooding if it's a broadcast OR if it's team in FFA
	if($Server::FloodProtectionEnabled && (!$Server::TourneyMode || !%team) && !%clientId.isSuperAdmin) // Allow Super Admins to Flood Messages
	{
		// we use getIntTime here because getSimTime gets reset.
		// time is measured in 32 ms chunks... so approx 32 to the sec
		%time = getIntegerTime(true) >> 5;
		if(%clientId.floodMute)
		{
			%delta = %clientId.muteDoneTime - %time;
			if(%delta > 0)
			{
				Client::sendMessage(%clientId, $MSGTypeGame, "FLOOD! You cannot talk for " @ %delta @ " seconds.");
				return;
			}
			%clientId.floodMute = "";
			%clientId.muteDoneTime = "";
		}
		%clientId.floodMessageCount++;
		// funky use of schedule here:
		schedule(%clientId @ ".floodMessageCount--;", 5, %clientId);
		if(%clientId.floodMessageCount > 4)
		{
			%clientId.floodMute = true;
			%clientId.muteDoneTime = %time + 10;
			Client::sendMessage(%clientId, $MSGTypeGame, "FLOOD! You cannot talk for 10 seconds.");
			return;
		}
	}
	
	%clientId.TMessagesTyped++;

	if(String::findSubStr(%message, "~w")>0)
	{
		if(%clientId.noVpack)
		{
			Client::sendMessage(%clientId, 1, "Your sound pack is broken. sorry...~wfemale2.wsorry.wav");
			return;
		}	
		else if(%clientId.BlockMySound)
		{
			%message = "(You have disabled your sound pack): " @%message;
			Client::sendMessage(%clientId, 1, %message, %clientId);
			return;
		}	
		%pos = String::findSubStr(%message, "~w");
	//	if(%pos > 0)
	//	{
			%sound = string::getSubStr(%message, %pos, 25);
			%message = string::getSubStr(%message, 0, %pos);
			//Anni::Echo("Split sound: "@%message@" "@%sound);
	//	}		
	}
			
	if($leet || %clientId.leet)
		%message = Ann::leetSpeek(%message);
			
	

	if(string::getsubstr(%message, 0, 1) == "@") 
	{
		%message = string::getsubstr(%message, 1, 999);
		%message = Ann::leetSpeek(%message);
	}
	
	%message = %message @ %sound;	
		
	if(string::getsubstr(%message, 0, 1) == "!" && %clientId.isAdmin) // Allow Admins to Bottom and Center print messages
	{
		if(string::getsubstr(%message, 1, 1) == "!" && %clientId.isAdmin)
		{
			%message = string::getsubstr(%message, 2, 999);
			centerprintall("<jc><f2>" @ %message, 8);
			return;
		}
		else
		{
			%message = string::getsubstr(%message, 1, 999);
			bottomprintall("<jc><f2>" @ %message, 8);
			return;
		}
	}
	if(string::getsubstr(%message, 0, 1) == "=" && string::getsubstr(%message, 1, 1) != "") // Whisper Talking
	{
		if(%clientId.whisper != "" && (Client::getTeam(%clientId) != -1 || %clientId.isAdmin)) 
		{
			if(%clientId.isAdmin) 
			{
				if(string::getsubstr(%message, 1, 1) == "!" && %clientId.isSuperAdmin) 
				{
					%message = string::getsubstr(%message, 2, 999);
					centerprint(%clientId.whisper, "<jc><f0>" @ Client::getName(%clientID) @ " (whisper): <f1>" @ %message, 8);
				}
				Client::sendMessage(%clientId.whisper, $MsgTypeChat, Client::getName(%clientID) @ " (whisper): " @ %message);
				Client::sendMessage(%clientId.whisper,0,"~wusepack.wav");
				Client::sendMessage(%clientId, $MsgTypeChat, Client::getName(%clientID) @ " (to " @ Client::getName(%clientId.whisper) @ "): " @ %message);
			}
			else if((Client::getTeam(%clientId) != -1 || $Game::missionType == "Duel") && !(%clientId.whisper).muted[%clientId]) 
			{
				if(!(%clientId.whisper).isSuperAdmin && Client::getTeam(%clientId) != Client::getTeam(%clientId.whisper))
					cheatAdminMsg(Client::getName(%clientID)@" (to "@Client::getName(%clientId.whisper)@"): "@%message);
				if(!(%clientId.whisper).isSuperAdmin && $listen[%clientId] != "")
					Client::sendMessage($listen[%clientId], 1, Client::getName(%clientID)@" (to "@Client::getName(%clientId.whisper)@"): "@%message);
				Client::sendMessage(%clientId.whisper, $MsgTypeChat, Client::getName(%clientID)@" (whisper): "@%message);
				Client::sendMessage(%clientId.whisper,0,"~wusepack.wav");
				Client::sendMessage(%clientId, $MsgTypeChat, Client::getName(%clientID)@" (to "@Client::getName(%clientId.whisper)@"): "@%message);
			}
		
			Anni::Echo("SAY: " @ %clientId @ " \""@Client::getName(%clientID)@" (to "@Client::getName(%clientId.whisper)@"): "@%message@"\"");
			return;
		}
	}
	%msg = %clientId @ " \"" @ %message @ "\" : " @ Client::getName(%clientID)@", "@timestamppatch();
	if(%team)
	{
		%team = Client::getTeam(%clientId);
		//if(%clientId.tmuted && %team == -1) 
		//{
		//	Client::sendMessage(%clientId, 1, "You are silenced, and may only talk in team chat once picked. sorry...~wfemale2.wsorry.wav");
		//	return;
		//}
		if($dedicated)
			Anni::Echo("SAYTEAM: " @ %msg);	

		%teamA = Client::getTeam(%clientId); 
		%arenaA = %clientId.inArena;
		%duelA = %clientId.inDuel;
		
        for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			%teamB = Client::getTeam(%cl);
			%arenaB = %cl.inArena;
			%duelB = %cl.inDuel;
			//if(!%cl.muted[%clientId] && %teamA == %teamB && %arenaA == %arenaB && %duelA == %duelB) 
			
			if(%clientId.inArena) 
			{
				if(!%cl.muted[%clientId] && %clientId.isArenaTDDead && %cl.isArenaTDDead)
					Client::sendMessage(%cl, $MsgTypeTeamChat, %message, %clientId);
				else if(!%cl.muted[%clientId] && %clientId.inArenaTDOne && %cl.inArenaTDOne)
					Client::sendMessage(%cl, $MsgTypeTeamChat, %message, %clientId);
				else if(!%cl.muted[%clientId] && %clientId.inArenaTDTwo && %cl.inArenaTDTwo)
					Client::sendMessage(%cl, $MsgTypeTeamChat, %message, %clientId);
				else if(!%cl.muted[%clientId] && %cl.inArena)
					Client::sendMessage(%cl, $MsgTypeTeamChat, %message, %clientId);
			}
			else if(%clientId.inDuel)
			{
				if(!%cl.muted[%clientId] && %cl.inDuel)
					Client::sendMessage(%cl, $MsgTypeTeamChat, %message, %clientId);
			}
			else
			{
				if(!%cl.muted[%clientId] && %teamA == %teamB && !%cl.inDuel && !%cl.inArena)
					Client::sendMessage(%cl, $MsgTypeTeamChat, %message, %clientId);
			}
		}
	}
	else
	{
		if(%clientId.tmuted) 
		{
			Client::sendMessage(%clientId, 1, "You are silenced, and may only talk in team chat once picked. sorry...~wfemale2.wsorry.wav");
			return;
		}
		if($dedicated)
			Anni::Echo("SAY: " @ %msg);
         for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			if(!%cl.muted[%clientId])
				Client::sendMessage(%cl, $MsgTypeChat, %message, %clientId);
	}	
}

function remoteIssueCommand(%commander, %cmdIcon, %command, %wayX, %wayY, %dest1, %dest2, %dest3, %dest4, %dest5, %dest6, %dest7, %dest8, %dest9, %dest10, %dest11, %dest12, %dest13, %dest14)
{
	if ( CheckEval("remoteIssueCommand", %commander, %cmdIcon, %command, %wayX, %wayY, %dest1, %dest2, %dest3, %dest4, %dest5, %dest6, %dest7, %dest8, %dest9, %dest10, %dest11, %dest12, %dest13, %dest14) )
		return;
			
	if($dedicated)
		Anni::Echo("COMMANDISSUE: " @ %commander @ " \"" @ Ann::Clean::string(%command) @ "\"");
		
	if($debug)
		Anni::Echo("remote Issue Command: x,"@%wayX@" y,"@ %wayY);
			
	// issueCommandI takes waypoint 0-1023 in x,y scaled mission area
	// issueCommand takes float mission coords.
	for(%i = 1; %dest[%i] != ""; %i = %i + 1)
		if(!%dest[%i].muted[%commander])
			issueCommandI(%commander, %dest[%i], %cmdIcon, %command, %wayX, %wayY);
}

function remoteIssueTargCommand(%commander, %cmdIcon, %command, %targIdx, %dest1, %dest2, %dest3, %dest4, %dest5, %dest6, %dest7, %dest8, %dest9, %dest10, %dest11, %dest12, %dest13, %dest14)
{
	if ( CheckEval("remoteIssueTargCommand", %commander, %cmdIcon, %command, %targIdx, %dest1, %dest2, %dest3, %dest4, %dest5, %dest6, %dest7, %dest8, %dest9, %dest10, %dest11, %dest12, %dest13, %dest14) )
		return;
			
	if($dedicated)
		Anni::Echo("COMMANDISSUE: " @ %commander @ " \"" @ Ann::Clean::string(%command) @ "\"");
	for(%i = 1; %dest[%i] != ""; %i = %i + 1)
		if(!%dest[%i].muted[%commander])
			issueTargCommand(%commander, %dest[%i], %cmdIcon, %command, %targIdx);
}

function remoteCStatus(%clientId, %status, %message)
{
	if ( CheckEval("remoteCStatus", %clientId, %status, %message) )
		return;
			
	// setCommandStatus returns false if no status was changed.
	// in this case these should just be team says.
	if(setCommandStatus(%clientId, %status, %message))
	{
		if($dedicated)
			Anni::Echo("COMMANDSTATUS: " @ %clientId @ " \"" @ Ann::Clean::string(%message) @ "\"");
	}
	else
		remoteSay(%clientId, true, %message);
}

function teamMessages(%mtype, %team1, %message1, %team2, %message2, %message3) //New Arena/Duel code
{
	%numPlayers = getNumClients();
	for(%i = 0; %i < %numPlayers; %i++) {
		%id = getClientByIndex(%i);
		if(Client::getTeam(%id) == %team1 && !%id.inArena && !%id.inDuel)
			Client::sendMessage(%id,%mtype,%message1);
		else if(%message2 != "" && Client::getTeam(%id) == %team2 && !%id.inArena && !%id.inDuel)
			Client::sendMessage(%id,%mtype,%message2);
		else if(%message3 != "" && !%id.inArena && !%id.inDuel)
			Client::sendMessage(%id,%mtype,%message3);
	}
}


function messageAll(%mtype, %message, %filter)
{
	if(%filter == "")
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			Client::sendMessage(%cl, %mtype, %message);
	else
	{
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			if(%cl.messageFilter & %filter)
				Client::sendMessage(%cl, %mtype, %message);
		}
	}
}

function messageAllExcept(%except, %mtype, %message)
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		if(%cl != %except)
			Client::sendMessage(%cl, %mtype, %message);
}

function messageTeam(%client, %color, %msg)
{
	%team = Client::getTeam(%client);
	%numPlayers = getNumClients();

	%arena = %client.inArena;
	%duel = %client.inDuel;

	for(%i = 0; %i < %numPlayers; %i++)
	{
		%cl = getClientByIndex(%i);
		%now = Client::getTeam(%cl);
		%aow = %cl.inArena;
		%dow = %cl.inDuel;

		if(%team == %now && !%aow && !%arena && !%dow && !%drena)
		{
			Client::sendMessage(%cl,%color,%msg);
		}
		else
		{
			if(%aow && %arena)
			{
				Client::sendMessage(%cl,%color,%msg);
			}
			else if(%dow && %duel)
			{
				Client::sendMessage(%cl,%color,%msg);
			}
		}
	}
}

function messageTeamExcept(%client, %color, %msg)
{
	%team = Client::getTeam(%client);
	%numPlayers = getNumClients();
	for(%i = 0; %i < %numPlayers; %i++)
	{
		%cl = getClientByIndex(%i);
		%now = Client::getTeam(%cl);
		if((%team == %now) && (%cl != %client))
		{
			if(!%cl.inArena || !%cl.inDuel)
				Client::sendMessage(%cl,%color,%msg);
		}
	}
}

function messageEnemy(%client, %color, %msg)
{
	%team = Client::getTeam(%client);
	%numPlayers = getNumClients();
	for(%i = 0; %i < %numPlayers; %i++)
	{
		%cl = getClientByIndex(%i);
		%now = Client::getTeam(%cl);
		if(%team != %now)
		{
			Client::sendMessage(%cl,%color,%msg);
		}
	}
}