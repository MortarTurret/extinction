// Made for easy addition.
function PlayerProfile::onChat(%clientId, %message)
{
	if ( String::getSubStr(%message, 0, 1) == "#")
	{
		%cmd = getWord(%message, 0);
		if ( %cmd == "#help" ) 
		{
//			Centerprint(%clientId,"<jc><f3>Profiles: \n <f1>Type: #register E-Mail Password \n <f2>This will register your playername into the profile system.\n <f1> Type: #login Password\n<f2> Use this command to login to your profile when joining the server. \n<f2> Type: #setpassword Password Username. \n<f2> This will change the Password for your account. \n<f2> Type: #CombatStats #ObjectiveStats #MiscStats #RecordsStats playername \n<f2> These four commands show you a registered players statistics.",8);
			Centerprint(%clientId,"<jc><f0>PROFILE SYSTEM COMMANDS:\n<f1>Note: New Profiles Work Only After A Server Reset\n\n<f1>Register:\n <f2>Type: #register E-Mail Password \n\n <f1>Login On Join:\n <f2> Type: #login Password \n\n <f1>Update Account Password:\n <f2> Type: #setpassword Password Username. ",20);

			Client::sendMessage(%clientId, 1, "Commands for the profile system below:");
			Client::sendMessage(%clientId, 2, "Type: #register E-Mail Password");
			Client::sendMessage(%clientId, 0, " - This will register your playername into the profile system.");
			Client::sendMessage(%clientId, 0, " NOTE: Profile Will Only Work After The Server Resets.");
			Client::sendMessage(%clientId, 1, ".");
			Client::sendMessage(%clientId, 2, "Type: #login Password");
			Client::sendMessage(%clientId, 0, " - Use this command to login to your profile when joining the server.");
			Client::sendMessage(%clientId, 1, ".");
			Client::sendMessage(%clientId, 2, "Type: #setpassword Password Username");
			Client::sendMessage(%clientId, 0, " - This will change the Password for your account.");
			Client::sendMessage(%clientId, 1, ".");
			Client::sendMessage(%clientId, 1, "Commands for the profile system above.");
//			Client::sendMessage(%clientId, 2, "Type: #CombatStats #ObjectiveStats #MiscStats #RecordsStats playername"); // 
//			Client::sendMessage(%clientId, 0, " - These four commands show you a registered players statistics.");
			if(%clientId.isOwner)
			{
				Client::sendMessage(%clientId, 0, " - This will set an AutoAdmin in the profile system for the User.");
				Client::sendMessage(%clientId, 0, "Usage: #setadmin <Admin level> <Username>");
			}
			return True;	
		}
		if ( %cmd == "#combatstats" )
		{
			%name = String::getSubStr(%message,(String::findSubStr(%message," ")+1),20);
			if ( %name == -1 )
			{
				Client::sendMessage(%clientId, 0, "Usage: #CombatStats Playername");
				return True;
			}
			for(%cl=Client::getFirst();%cl!=-1;%cl=Client::getNext(%cl))
			{
				if ( Client::getName(%cl) == %name )
				{
		if(%cl.TKills=="") %cl.TKills=0; else %cl.TKills=%cl.TKills;
		if(%cl.TDeaths=="") %cl.TDeaths=0; else %cl.TDeaths=%cl.TDeaths;
		if(%cl.TMidAirs=="") %cl.TMidAirs=0; else %cl.TMidAirs=%cl.TMidAirs;
		if(%cl.TKillstreaks=="") %cl.TKillstreaks=0; else %cl.TKillstreaks=%cl.TKillstreaks;
		if(%cl.TTKills=="") %cl.TTKills=0; else %cl.TTKills=%cl.TTKills;
		if(%cl.TGrenadesThrown=="") %cl.TGrenadesThrown=0; else %cl.TGrenadesThrown=%cl.TGrenadesThrown;
		if(%cl.TMinesDropped=="") %cl.TMinesDropped=0; else %cl.TMinesDropped=%cl.TMinesDropped;
					bottomprint(%clientId, "<jc><f0>" @ %name @ " CombatStats:<f1> Kills<f2> " @ %cl.TKills @ " <f1>Deaths<f2> " @ %cl.TDeaths @ " <f1>MidAirs<f2> " @ %cl.TMidAirs @ " <f1>Killstreaks<f2> " @ %cl.TKillstreaks @ " <f1>Teammates Killed<f2> " @ %cl.TTKills @ " <f1>Grenades Thrown<f2> " @ %cl.TGrenadesThrown @ " <f1>Mines Dropped<f2> " @ %cl.TMinesDropped, 10);
					Client::sendMessage(%clientId, 1, Client::getName(%cl)@" Combat Stats: ");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Kills "@%cl.TKills@".");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Deaths "@%cl.TDeaths@".");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Mid Airs "@%cl.TMidAirs@".");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Kill Streaks "@%cl.TKillstreaks@".");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Teammates Killed "@%cl.TTKills@".");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Grenades Thrown "@%cl.TGrenadesThrown@".");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Mines Dropped "@%cl.TMinesDropped@".");
					return True;
				}
			}
			Client::sendMessage(%clientId, 0, "Unable to find that person currently playing.");
			return True;
		}
		if ( %cmd == "#objectivestats" )
		{
			%name = String::getSubStr(%message,(String::findSubStr(%message," ")+1),20);
			if ( %name == -1 )
			{
				Client::sendMessage(%clientId, 0, "Usage: #ObjectiveStats Playername");
				return True;
			}
			for(%cl=Client::getFirst();%cl!=-1;%cl=Client::getNext(%cl))
			{
				if ( Client::getName(%cl) == %name )
				{
		if(%cl.TTurKills=="") %cl.TTurKills=0; else %cl.TTurKills=%cl.TTurKills;
		if(%cl.TGenKills=="") %cl.TGenKills=0; else %cl.TGenKills=%cl.TGenKills;
		if(%cl.TPowKills=="") %cl.TPowKills=0; else %cl.TPowKills=%cl.TPowKills;
		if(%cl.TTowerCaps=="") %cl.TTowerCaps=0; else %cl.TTowerCaps=%cl.TTowerCaps;
		if(%cl.TFlagCaps=="") %cl.TFlagCaps=0; else %cl.TFlagCaps=%cl.TFlagCaps;
		if(%cl.TFlagRets=="") %cl.TFlagRets=0; else %cl.TFlagRets=%cl.TFlagRets;
					bottomprint(%clientId, "<jc><f0>" @ %name @ " ObjectiveStats:<f1> Turrets Destroyed<f2> " @ %cl.TTurKills @ " <f1>Gens Destroyed<f2> " @ %cl.TGenKills @ " <f1>Power Outages Caused<f2> " @ %cl.TPowKills @ " <f1>Tower Captures<f2> " @ %cl.TTowerCaps @ " <f1>Flag Captures<f2> " @ %cl.TFlagCaps @ " <f1>Flag Returns<f2> " @ %cl.TFlagRets, 10);
					Client::sendMessage(%clientId, 1, Client::getName(%cl)@" Objective Stats: ");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Turrets Killed "@%cl.TTurKills@".");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Gens Destroyed "@%cl.TGenKills@".");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Power Outages Made "@%cl.TPowKills@".");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Tower Caps "@%cl.TTowerCaps@".");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Flag Caps "@%cl.TFlagCaps@".");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Flag Returns "@%cl.TFlagRets@".");
					return True;
				}
			}
			Client::sendMessage(%clientId, 0, "Unable to find that person currently playing.");
			return True;
		}
		if ( %cmd == "#miscstats" )
		{
			%name = String::getSubStr(%message,(String::findSubStr(%message," ")+1),20);
			if ( %name == -1 )
			{
				Client::sendMessage(%clientId, 0, "Usage: #MiscStats Playername");
				return True;
			}
			for(%cl=Client::getFirst();%cl!=-1;%cl=Client::getNext(%cl))
			{
				if ( Client::getName(%cl) == %name )
				{
		if(%cl.TConnections=="") %cl.TConnections=0; else %cl.TConnections=%cl.TConnections;
		if(%cl.TMessagesTyped=="") %cl.TMessagesTyped=0; else %cl.TMessagesTyped=%cl.TMessagesTyped;
		if(%cl.THerpDeaths=="") %cl.THerpDeaths=0; else %cl.THerpDeaths=%cl.THerpDeaths;
		if(%cl.TBaseKills=="") %cl.TBaseKills=0; else %cl.TBaseKills=%cl.TBaseKills;
		if(%cl.TSuicide=="") %cl.TSuicide=0; else %cl.TSuicide=%cl.TSuicide;
					bottomprint(%clientId, "<jc><f0>" @ %name @ " MiscStats:<f1> Times Connected<f2> " @ %cl.TConnections @ " <f1>Messages Entered<f2> " @ %cl.TMessagesTyped @ " <f1>Times Died By Herpes<f2> " @ %cl.THerpDeaths @ " <f1>Base Kills<f2> " @ %cl.TBaseKills @ " <f1>Suicides<f2> " @ %cl.TSuicide, 10);
					Client::sendMessage(%clientId, 1, Client::getName(%cl)@" Misc Stats: ");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Times Connected "@%cl.TConnections@".");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Messages Entered "@%cl.TMessagesTyped@".");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Times Died By Herpes "@%cl.THerpDeaths@".");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Base Kills "@%cl.TBaseKills@".");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Suicides "@%cl.TSuicide@".");
					return True;
				}
			}
			Client::sendMessage(%clientId, 0, "Unable to find that person currently playing.");
			return True;
		}
		if ( %cmd == "#recordsstats" )
		{
			%name = String::getSubStr(%message,(String::findSubStr(%message," ")+1),20);
			if ( %name == -1 )
			{
				Client::sendMessage(%clientId, 0, "Usage: #RecordsStats Playername");
				return True;
			}
			for(%cl=Client::getFirst();%cl!=-1;%cl=Client::getNext(%cl))
			{
				if ( Client::getName(%cl) == %name )
				{
		if(%cl.TFarthestDMA=="") %cl.TFarthestDMA=0; else %cl.TFarthestDMA=%cl.TFarthestDMA;
		if(%cl.TFarthestSMA=="") %cl.TFarthestSMA=0; else %cl.TFarthestSMA=%cl.TFarthestSMA;
		if(%cl.TFastestCap=="") %cl.TFastestCap=0; else %cl.TFastestCap=%cl.TFastestCap;
		if(%cl.TKillstreakBest=="") %cl.TKillstreakBest=0; else %cl.TKillstreakBest=%cl.TKillstreakBest;
		if(%cl.TMidAirsBest=="") %cl.TMidAirsBest=0; else %cl.TMidAirsBest=%cl.TMidAirsBest;
					bottomprint(%clientId, "<jc><f0>" @ %name @ " RecordsStats:<f1> Farthest Disc Mid Air<f2> " @ %cl.TFarthestDMA @ "Meters. <f1>Farthest Sniper Mid Air<f2> " @ %cl.TFarthestSMA @ "Meters. <f1>Fastest Flag Capture<f2> " @ %cl.TFastestCap @ "Seconds. <f1>Longest Killstreak<f2> " @ %cl.TKillstreakBest @ "Kills. <f1>Most Mid Airs in 30 min<f2> " @ %cl.TMidAirsBest, 10);
					Client::sendMessage(%clientId, 1, Client::getName(%cl)@" Records Stats: ");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Farthest Disc Mid Air "@%cl.TFarthestDMA@" Meters.");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Farthest Sniper Mid Air "@%cl.TFarthestSMA@" Meters.");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Fastest Flag Capture "@%cl.TFastestCap@" Seconds.");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Longest Killstreak "@%cl.TKillstreakBest@" Kills.");
					Client::sendMessage(%clientId, 0, Client::getName(%cl)@" Most Mid Airs in 30 min "@%cl.TMidAirsBest@".");
					return True;
				}
			}
			Client::sendMessage(%clientId, 0, "Unable to find that person currently playing.");
			return True;
		}
		if ( %cmd == "#register" )
		{
      %name = Client::getName(%clientId);

      //- EXT Patch: Prevent namesquatting by simply locking registrations for 
      //- certain names. This is why we can't have nice things. - Mortar Turret
      if(String::findSubStr(%name, "Turret") != -1) {
				Client::sendMessage(%clientId, 0, "This name cannot be registered on this server.");
      }

			if ( %clientId.PPIsRegistered )
			{
				Client::sendMessage(%clientId, 0, "You are already registered with the server.");
			}
			else
			{
				%email = getWord(%message, 1);
				%pass = getWord(%message, 2);
				if ( %email == -1 || %email == "" || %pass == -1 || %pass == "" )
				{
					Client::sendMessage(%clientId, 0, "Usage: #register <E-Mail> <Password>");
					return True;
				}
				if ( String::findSubStr(%email, "@") == -1 || String::findSubStr(%email, ".") == -1 )
				{
					Client::sendMessage(%clientId, 0, "Please give a valid email with no spaces.  It will only be seen by the admins so that you can recover your account.");
					return True;
				}
				if ( strlen(%pass) < 5 || strlen(%pass) > 20 )
				{
					Client::sendMessage(%clientId, 0, "Your password must be 5 characters long and no more than 20.");
					return True;
				}
				%first = String::getFirstAlphaNum(%name);
				if ( %first == -1 )
				{
					Client::sendMessage(%clientId, 0, "The system cannot save your profile due to the fact your username is missing any alphanumerical characters.");
					return True;
				}
				if ( !$PlayerProfileCount )
					exec(PPStuff);
				if ( !$PlayerProfileCount )
					$PlayerProfileCount = 0;
				deleteVariables("PlayerProfileTemp*"); // Make sure of a clean export.
				exec("PP"@%first@".cs");
				for(%i=0;%i<1000;%i++)
				{
					if ( $PlayerProfileTempName[%i] == %name && floor($PlayerProfileTempID[%i]) != 0 )
					{
						deleteVariables("PlayerProfileTemp*");
						Client::sendMessage(%clientId, 0, "That username is already registered in the system.");
						return True;
					}
					if ( !$PlayerProfileTempName[%i] && !$PlayerProfileTempID[%i] )
						break;
				}
				$PlayerProfileCount++;
				%clientId.PPID = $PlayerProfileCount;
				$PlayerProfileTempID[%i] = %clientId.PPID;
				$PlayerProfileTempName[%i] = %name;
				export("PlayerProfileTemp*", "temp\\PP"@%first@".cs");
				deleteVariables("PlayerProfileTemp*"); // Clean up.
				export("PlayerProfile*", "temp\\PPStuff.cs");
				%clientId.PPPassword = %pass;
				%clientId.PPEmail = %email;
				%clientId.PPAdminLevel = "None";
				%clientId.PPRequiresLogin = True;
				%clientId.PPValidateByInfo = False;
				%clientId.PPInfoName = $Client::info[%clientId, 1];
				%clientId.PPInfoEmail = $Client::info[%clientId, 2];
				%clientId.PPInfoTribe = $Client::info[%clientId, 3];
	 			%clientId.PPInfoURL = $Client::info[%clientId, 4];
	 			%clientId.PPInfoOther = $Client::info[%clientId, 5];
				%clientId.PPIsRegistered = True;
				%clientId.PPValidated = True;
				%clientId.PPAuthenticated = True;
				if ( PlayerProfile::Save(%clientId) )
				{
					Client::sendMessage(%clientId, 1, "You are now successfully registred with the system.");
					Client::sendMessage(%clientId, 1, "To login the next time you join simply type the following after connecting.");
					Client::sendMessage(%clientId, 1, "#login password");
					PlayerProfile::Log(Client::getName(%clientId)@" "@Ann::ipCut(Client::getTransportAddress(%clientId))@" is now registered with the Player Profile system.");
				}
				else
				{
					%clientId.PPIsRegistered = False;
					%clientId.PPValidated = False;
					%clientId.PPAuthenticated = False;
					Client::sendMessage(%clientId, 0, "There was an error, please contact no one.");
					deleteVariables("PlayerProfileTemp*");
					exec("PP"@%first@".cs");
					$PlayerProfileTempID[%i] = "";
					$PlayerProfileTempName[%i] = "";
					export("PlayerProfileTemp*", "temp\\PP"@%first@".cs");
					deleteVariables("PlayerProfileTemp*");
					$PlayerProfileCount--;
					export("PlayerProfile*", "temp\\PPStuff.cs");
					%clientId.PPIsRegistered = False;
					%clientId.PPPassword = "";
					PlayerProfile::Log(Client::getName(%clientId)@" "@Ann::ipCut(Client::getTransportAddress(%clientId))@" profile failed to save on registering.");
				}
			}
			return True;
		}
		if ( %cmd == "#requirelogin" )
		{
			if ( !%clientId.PPIsRegistered )
			{
				Client::sendMessage(%clientId, 0, "This account is not registred.");
				return True;
			}
			if ( !%clientId.PPAuthenticated )
			{
				Client::sendMessage(%clientId, 1, "You must login to alter this account's settings.");
				return True;
			}
			if ( %clientId.PPRequiresLogin )
			{
				if ( !%clientId.PPValidateByInfo )
				{
					Client::sendMessage(%clientId, 0, "You must turn on Validate By Info in order to turn this off.");
					return True;
				}
				%clientId.PPRequiresLogin = False;
				if ( PlayerProfile::Save(%clientId) )
				{
					Client::sendMessage(%clientId, 0, "You are no longer required to login.");
					return True;
				}
				Client::sendMessage(%clientId, 1, "There was an error, please contact no one.");
				%clientId.PPRequiresLogin = True;
				return True;
			}
			%clientId.PPRequiresLogin = True;
			if ( PlayerProfile::Save(%clientId) )
			{
				Client::sendMessage(%clientId, 0, "You are now required to login.");
				return True;
			}
			Client::sendMessage(%clientId, 0, "There was an error, please contact no one.");
			%clientId.PPRequiresLogin = False;
			PlayerProfile::Log(Client::getName(%clientId)@" "@Ann::ipCut(Client::getTransportAddress(%clientId))@" profile failed to save. Requirelogin.");
			return True;
		}
		if ( %cmd == "#validatebyinfo" )
		{
			if ( !%clientId.PPIsRegistered )
			{
				Client::sendMessage(%clientId, 0, "This account is not registred.");
				return True;
			}
			if ( !%clientId.PPAuthenticated )
			{
				Client::sendMessage(%clientId, 1, "You must login to alter this account's settings.");
				return True;
			}
			if ( %clientId.PPValidateByInfo )
			{
				// if ( !%clientId.RequiresLogin )
				// {
				// 	Client::sendMessage(%clientId, 0, "You must either require a login or validate by info.");
				// 	return True;
				// }
				%clientId.PPValidateByInfo = False;
				%clientId.PPRequiresLogin = True;
				if ( PlayerProfile::Save(%clientId) )
				{
					Client::sendMessage(%clientId, 0, "Your account will no longer validate by info.");
					return True;
				}
				Client::sendMessage(%clientId, 1, "There was an error, please contact no one.");
				%clientId.PPValidateByInfo = True;
			}
			%clientId.PPInfoName = $Client::info[%clientId, 1];
			%clientId.PPInfoEmail = $Client::info[%clientId, 2];
			%clientId.PPInfoTribe = $Client::info[%clientId, 3];
	 		%clientId.PPInfoURL = $Client::info[%clientId, 4];
	 		%clientId.PPInfoOther = $Client::info[%clientId, 5];
			%clientId.PPValidateByInfo = True;
			%clientId.PPRequiresLogin = False;
			if ( PlayerProfile::Save(%clientId) )
			{
				Client::sendMessage(%clientId, 0, "Your account will now validate by info.");
				return True;
			}
			Client::sendMessage(%clientId, 1, "There was an error, please contact no one.");
			%clientId.PPValidateByInfo = False;
			PlayerProfile::Log(Client::getName(%clientId)@" "@Ann::ipCut(Client::getTransportAddress(%clientId))@" profile failed to save. ValidateByInfo.");
			return True;
		}
		if ( %cmd == "#setadmin" )
		{
			if ( !%clientId.PPIsRegistered || !%clientId.PPAuthenticated )
			{
				Client::sendMessage(%clientId, 0, "You must login to use administrative commands.");
				return True;
			}
			if ( !%clientId.isOwner )
			{
				Client::sendMessage(%clientId, 1, "Only the server owner may use this command.~wAccess_Denied.wav");
				return True;
			}
			%adminlvl = getWord(%message, 1);
			%name = getWord(%message, 2);
			$profilenamex = %name;
			for(%i=3;%i<20;%i++)
			{
				%word = getWord(%message,%i);
				echo("word: "@%word);
				if ( %word == -1 )
					break;
				$profilenamex = $profilenamex @" "@ %word;
				%name = $profilenamex;
				echo("name: "@%name);
			}
			deleteVariables("profilenamex"); // Clean up!
			if ( %name == -1 || %adminlvl == -1 )
			{
				Client::sendMessage(%clientId, 0, "Usage: #setadmin <Admin level> <Username>");
				return True;
			}
			if ( %adminlvl != "Owner" && %adminlvl != "God" && %adminlvl != "Super" && %adminlvl != "Public" && %adminlvl != "Safe" )
			{
				Client::sendMessage(%clientId, 1, "The admin level may only be: Owner, God, Super, Public, Safe, or None.");
				return True;
			}
			for(%cl=Client::getFirst();%cl!=-1;%cl=Client::getNext(%cl))
			{
				if ( Client::getName(%cl) == %name )
				{
					if ( !%cl.PPIsRegistered )
					{
						Client::sendMessage(%clientId, 1, "That user is not registered with the system.");
						return True;
					}
					if ( !%cl.PPAuthenticated )
					{
						Client::sendMessage(%clientId, 1, "That user hasn't logged in.");
						return True;
					}
					if(%adminlvl == "Owner") 
					{ 
						%cl.isUntouchable = true;
						%cl.isAdmin = true; 
						%cl.isSuperAdmin = true; 
						%cl.isGod = true;
						%cl.isOwner = true;
						%cl.PPAdminLevel = "Owner";				
					}
					else if(%adminlvl == "God") 
					{ 
						// %cl.isUntouchable = true;
						%cl.isAdmin = true; 
						%cl.isSuperAdmin = true; 
						%cl.isGod = true;
						%cl.isOwner = false;
						%cl.PPAdminLevel = "God";  
					} 						
					else if(%adminlvl == "Super") 
					{ 
						// %cl.isUntouchable = true;
						%cl.isAdmin = true; 
						%cl.isSuperAdmin = true; 
						%cl.isGod = false;
						%cl.isOwner = false;
						%cl.PPAdminLevel = "Super";		
					}
					else if(%adminlvl == "Public") 
					{ 
						// %cl.isUntouchable = true;
						%cl.isAdmin = true; 
						%cl.isSuperAdmin = false; 
						%cl.isGod = false;
						%cl.isOwner = false;
						%cl.PPAdminLevel = "Public";  
					} 
					else if(%adminlvl == "Safe") 
					{ 
						%cl.isUntouchable = true;
						%cl.isAdmin = false; 
						%cl.isSuperAdmin = false; 
						%cl.isGod = false;
						%cl.isOwner = false;
						%cl.PPAdminLevel = "Safe";
					}
					else
					{
						%cl.isUntouchable = false;
						%cl.isAdmin = false; 
						%cl.isSuperAdmin = false; 
						%cl.isGod = false;
						%cl.isOwner = false;
						%cl.PPAdminLevel = "None";
					}
					Client::sendMessage(%cl, 1, "You have been given "@%cl.PPAdminLevel@" admin status.~wCaptured_Tower.wav");
					if ( PlayerProfile::Save(%cl) )
						Client::sendMessage(%clientId, 0, Client::getName(%cl)@" has successfully been given "@%cl.PPAdminLevel@" admin.");
					else
					{
						Client::sendMessage(%clientId, 0, Client::getName(%cl)@"'s profile failed to save.");
						PlayerProfile::Log(Client::getName(%clientId)@" "@Ann::ipCut(Client::getTransportAddress(%clientId))@" profile failed to save. SetAdmin.");
					}
					return True;
				}
			}
			Client::sendMessage(%clientId, 0, "Failed to find that user in the active clients list.");
			return True;
		}
		if ( %cmd == "#setpassword" )
		{
			%username = Client::getName(%clientId);
			%password = getWord(%message, 1);
			%name = getWord(%message, 2); 
			$profilenamex = %name;
			for(%i=3;%i<20;%i++)
			{
				%word = getWord(%message,%i);
				echo("word: "@%word);
				if ( %word == -1 )
					break;
				$profilenamex = $profilenamex @" "@ %word;
				%name = $profilenamex;
				echo("name: "@%name);
			}
			deleteVariables("profilenamex"); // Clean up!
			if ( %username == %name && %clientId.PPAuthenticated ) 
			{
				PlayerProfileEdit::Load(%clientId, %password, %name);
				return True;
			}
			else if ( %clientId.isOwner )
			{
				echo("name edit: "@%name);
				PlayerProfileEdit::Load(%clientId, %password, %name);
				return True;
			}
			else
			{
				Client::sendMessage(%clientId, 1, "Only the server owner may use this command.~wAccess_Denied.wav");
				return True;
			}
		}
		if ( %cmd == "#login" )
		{
			if ( !%clientId.PPIsRegistered )
			{
				Client::sendMessage(%clientId, 0, "This account is not registered.");
				return True;
			}
			if ( %clientId.PPAuthenticated )
			{
				Client::sendMessage(%clientId, 0, "You are already logged in.");
				return True;
			}
			if ( getWord(%message, 1) == %clientId.PPPassword )
			{
				Client::sendMessage(%clientId, 0, "You have logged in.~wfemale1.whello.wav");
				// Centerprint(%clientId,"<jc><f3>Welcome to <f3>"@$ModVersion@"\n\n<f1>Update: Duel Bots have been added to indoor Arenas!",8);
				Centerprint(%clientId,"<jc><f3>Welcome to <f2>"@$ModVersion@"\n\n<f1>Press left click to spawn. Press tab for options. \n\n<f1>Tribes Discord:\n<f2>www.playt1.com\n\n<f1>Server Discord:\n<f2>www.annihilation.info",8);
				%clientId.PPAuthenticated = True;
				%clientId.PPValidated = True; // Password overrides info validation.
				%clientId.PPIsFake = False;
				if ( !%clientId.isUntouchable ) 
				{
					if(%clientId.PPAdminLevel == "Owner") 
					{ 
						%clientId.isUntouchable = true;
						%clientId.isAdmin = true; 
						%clientId.isSuperAdmin = true; 
						%clientId.isGod = true;
						%clientId.isOwner = true;				
					}
					else if(%clientId.PPAdminLevel == "God") 
					{ 
						%clientId.isUntouchable = true;
						%clientId.isAdmin = true; 
						%clientId.isSuperAdmin = true; 
						%clientId.isGod = true;  
					} 						
					else if(%clientId.PPAdminLevel == "Super") 
					{ 
						%clientId.isUntouchable = true;
						%clientId.isAdmin = true; 
						%clientId.isSuperAdmin = true;
					}
					else if(%clientId.PPAdminLevel == "Public") 
					{ 
						%clientId.isUntouchable = true;
						%clientId.isAdmin = true;  
					} 
					else if(%clientId.PPAdminLevel == "Safe") 
					{ 
						%clientId.isUntouchable = true;  
					}
				} 
			}
			else
			{
				if ( %clientId.PPLoginAttempts >= 3 )
				{
					Client::sendMessage(%clientId, 1, "You have exceeded the maximum number of login attempts.  Bye.");
					schedule("Net::Kick(\""@%clientId@"\", \"You have exceeded the maximum number of login attempts.  Come back in a few minutes.\");", 0.1, %clientId);
					BanList::add(Client::getTransportAddress(%clientId), 120); // 2 minutes.
					BanList::export("config\\banlist.cs");
					PlayerProfile::Log(Client::getName(%clientId)@" "@Ann::ipCut(Client::getTransportAddress(%clientId))@" exceeded the maximum number of login attempts.");
					return True;
				}
				%clientId.PPLoginAttempts++;
				Client::sendMessage(%clientId, 1, "Password is incorrect. You have "@(4-%clientId.PPLoginAttempts)@" login attempts left.~wAccess_Denied.wav");
			}
			return True;
		}
	}
	if ( ( %clientId.PPIsRegistered && %clientId.PPRequiresLogin && !%clientId.PPAuthenticated ) || %clientId.PPIsFake )
	{
		Client::sendMessage(%clientId, 1, "You must login before you are able to talk.~wAccess_Denied.wav");
		return True;
	}
	return False;
}

function DisplayStats(%clientId,%type,%look)
{
	echo("DisplayStats("@%clientId@","@%type@","@%look@");");

		if(%clientId.TKills=="") %TKills=0; else %TKills=%clientId.TKills;
		if(%clientId.TDeaths=="") %TDeaths=0; else %TDeaths=%clientId.TDeaths;
		
		if(%clientId.TTurKills=="") %TTurKills=0; else %TTurKills=%clientId.TTurKills;
		if(%clientId.TGenKills=="") %TGenKills=0; else %TGenKills=%clientId.TGenKills;
		if(%clientId.TPowKills=="") %TPowKills=0; else %TPowKills=%clientId.TPowKills;
		if(%clientId.TTowerCaps=="") %TTowerCaps=0; else %TTowerCaps=%clientId.TTowerCaps;
		if(%clientId.TFlagCaps=="") %TFlagCaps=0; else %TFlagCaps=%clientId.TFlagCaps;
		if(%clientId.TFlagRets=="") %TFlagRets=0; else %TFlagRets=%clientId.TFlagRets;
	
		if(%clientId.TMidAirs=="") %TMidAirs=0; else %TMidAirs=%clientId.TMidAirs;
		if(%clientId.TKillstreaks=="") %TKillstreaks=0; else %TKillstreaks=%clientId.TKillstreaks;
		if(%clientId.TTKills=="") %TTKills=0; else %TTKills=%clientId.TTKills;
		if(%clientId.TGrenadesThrown=="") %TGrenadesThrown=0; else %TGrenadesThrown=%clientId.TGrenadesThrown;
		if(%clientId.TMinesDropped=="") %TMinesDropped=0; else %TMinesDropped=%clientId.TMinesDropped;
	
		if(%clientId.TConnections=="") %TConnections=0; else %TConnections=%clientId.TConnections;
		if(%clientId.TMessagesTyped=="") %TMessagesTyped=0; else %TMessagesTyped=%clientId.TMessagesTyped;
		if(%clientId.THerpDeaths=="") %THerpDeaths=0; else %THerpDeaths=%clientId.THerpDeaths;
		if(%clientId.TBaseKills=="") %TBaseKills=0; else %TBaseKills=%clientId.TBaseKills;
		if(%clientId.TSuicide=="") %TSuicide=0; else %TSuicide=%clientId.TSuicide;
		
		if(%clientId.TFarthestDMA=="") %TFarthestDMA=0; else %TFarthestDMA=%clientId.TFarthestDMA;
		if(%clientId.TFarthestSMA=="") %TFarthestSMA=0; else %TFarthestSMA=%clientId.TFarthestSMA;
		if(%clientId.TFastestCap=="") %TFastestCap=0; else %TFastestCap=%clientId.TFastestCap; 
		if(%clientId.TKillstreakBest=="") %TKillstreakBest=0; else %TKillstreakBest=%clientId.TKillstreakBest;
		if(%clientId.TMidAirsBest=="") %TMidAirsBest=0; else %TMidAirsBest=%clientId.TMidAirsBest;
	
	%x = "";
	if(%type == "objective")
	{
		%x = "<jc><f1>Objective Statistics:\n";
		%x = %x@"<jc><f2>Turrets Destroyed - <f3>"@%TTurKills@"\n";
		%x = %x@"<jc><f2>Gens Destroyed - <f3>"@%TGenKills@"\n";
		%x = %x@"<jc><f2>Power Outages Caused - <f3>"@%TPowKills@"\n";
		%x = %x@"<jc><f2>Tower Captures - <f3>"@%TTowerCaps@"\n"; //oooooops
		%x = %x@"<jc><f2>Flag Captures - <f3>"@%TFlagCaps@"\n";
		%x = %x@"<jc><f2>Flag Returns - <f3>"@%TFlagRets@"\n";
	}
	else if(%type == "combat")
	{
		%x = "<jc><f1>Combat Statistics:\n";
		%x = %x@"<jc><f2>Kills/Deaths - <f3>"@%TKills@"<f1>/<f3>"@%TDeaths@"\n";
		%x = %x@"<jc><f2>Mid Airs - <f3>"@%TMidAirs@"\n";
		%x = %x@"<jc><f2>Kill Streaks - <f3>"@%TKillstreaks@"\n";
		%x = %x@"<jc><f2>Teammates Killed - <f3>"@%TTKills@"\n";
		%x = %x@"<jc><f2>Grenades Thrown - <f3>"@%TGrenadesThrown@"\n";
		%x = %x@"<jc><f2>Mines Dropped - <f3>"@%TMinesDropped@"\n";
	}
	else if(%type == "misc")
	{
		%x = "<jc><f1>Miscellaneous Statistics:\n";
		%x = %x@"<jc><f2>Times Connected - <f3>"@%TConnections@"\n";
		%x = %x@"<jc><f2>Messages Entered - <f3>"@%TMessagesTyped@"\n";
		%x = %x@"<jc><f2>Times Died By Herpes - <f3>"@%THerpDeaths@"\n";
		%x = %x@"<jc><f2>Base Kills - <f3>"@%TBaseKills@"\n";
		%x = %x@"<jc><f2>Suicides - <f3>"@%TSuicide@"\n";
	}
	else if(%type == "records")
	{
		%x = "<jc><f1>Records Statistics :\n";
		%x = %x@"<jc><f2>Farthest Disc Mid Air - <f3>"@%TFarthestDMA@" meters\n";
		%x = %x@"<jc><f2>Farthest Sniper Mid Air - <f3>"@%TFarthestSMA@" meters\n";
		%x = %x@"<jc><f2>Fastest Flag Capture - <f3>"@%TFastestCap@" seconds\n";
		%x = %x@"<jc><f2>Longest Killstreak - <f3>"@%TKillstreakBest@" kills\n";
		%x = %x@"<jc><f2>Most Mid Airs in 30 min - <f3>"@%TMidAirsBest@" ma\n";
	}

	if(%look != "" && %look != "-1")
	   Centerprint(%look,%x,10);
	else
	   Centerprint(%clientId,%x,10);
}

function PlayerProfile::Log(%string)
{
	%string = String::getSubStr(%string,0,250); 
	echo(%string);
	admin::message(%string);
	$Log = %string;
	export("Log", "config\\PlayerProfile.log", true);
	return;
}

function PlayerProfileEdit::Load(%clientId, %password, %name)
{
	// Load Player Profile for %clientId.
	%username = Client::getName(%clientId);
	//$profilenamex = "";
		
	echo("Admin: "@%username@", Password: "@%password@", Name: "@%name);
	%first = String::getFirstAlphaNum(%name);
	if ( %first == -1 )
		return False; 
	exec("PP"@%first@".cs");
	for(%i=0;%i<1000;%i++)
	{
		if ( $PlayerProfileTempName[%i] == %name && floor($PlayerProfileTempID[%i]) != 0 )
		{
			%id = $PlayerProfileTempID[%i];
			break;
		}
		if ( !$PlayerProfileTempName[%i] && !$PlayerProfileTempID[%i] )
			break;
	}
	deleteVariables("PlayerProfileTemp*"); // Clean up!
	if ( floor(%id) == 0 )
		return; // No profile.
	// Profile ID found, exec file.
	if(%username != %name)
	{
		exec("Player"@%id@".cs");
		$ProfileEdit::PPID = %id;
		$ProfileEdit::PPPassword = %password; // Password, obviously.
		$ProfileEdit::PPEmail = $PlayerProfileTempEmail; // Email used at registration.
		$ProfileEdit::PPInfoName = $PlayerProfileTempIName; // Info Name
		$ProfileEdit::PPInfoEmail = $PlayerProfileTempIEmail; // Info Email
		$ProfileEdit::PPInfoTribe = $PlayerProfileTempITribe; // Info Tribe
		$ProfileEdit::PPInfoURL = $PlayerProfileTempIURL; // Info URL
		$ProfileEdit::PPInfoOther = $PlayerProfileTempIOther; // Info Other
		
		$ProfileEdit::TKills = $PlayerProfileTempKills; // Annihilation Kills stat.
		$ProfileEdit::TDeaths = $PlayerProfileTempDeaths; // Annihilation Deaths stat.
		
		$ProfileEdit::TTurKills = $PlayerProfileTempTTurKills;
		$ProfileEdit::TGenKills = $PlayerProfileTempTGenKills;
		$ProfileEdit::TPowKills = $PlayerProfileTempTPowKills;
		$ProfileEdit::TTowerCaps = $PlayerProfileTempTTowerCaps;
		$ProfileEdit::TFlagCaps = $PlayerProfileTempTFlagCaps;
		$ProfileEdit::TflagRets = $PlayerProfileTempTFlagRets;
		
		$ProfileEdit::TMidAirs = $PlayerProfileTempTMidAirs;
		$ProfileEdit::TKillstreaks = $PlayerProfileTempTKillstreaks;
		$ProfileEdit::TTKills = $PlayerProfileTempTTKills;
		$ProfileEdit::TGrenadesThrown = $PlayerProfileTempTGrenadesThrown;
		$ProfileEdit::TMinesDropped = $PlayerProfileTempTMinesDropped;
		
		$ProfileEdit::TConnections = $PlayerProfileTempTConnections;
		$ProfileEdit::TMessagesTyped = $PlayerProfileTempTMessagesTyped;
		$ProfileEdit::THerpDeaths = $PlayerProfileTempTHerpDeaths;
		$ProfileEdit::TBaseKills = $PlayerProfileTempTBaseKills;
		$ProfileEdit::TSuicide = $PlayerProfileTempTSuicide;
		
		$ProfileEdit::TFarthestDMA = $PlayerProfileTempTFarthestDMA;
		$ProfileEdit::TFarthestSMA = $PlayerProfileTempTFarthestSMA;
		$ProfileEdit::TFastestCap = $PlayerProfileTempTFastestCap;
		$ProfileEdit::TKillstreakBest = $PlayerProfileTempTKillstreakBest;
		$ProfileEdit::TMidAirsBest = $PlayerProfileTempTMidAirsBest;
		//End
		
		//End
		$ProfileEdit::Credits = $PlayerProfileTempCredits; // Annihilation Credits stat.
		$ProfileEdit::MiniMode = $PlayerProfileTempMiniMode; // Mini-Bomber weapon mode.	
		$ProfileEdit::Vulcan = $PlayerProfileTempVulcan; // Vulcan weapon mode.
		$ProfileEdit::pbeam = $PlayerProfileTempPbeam; // Particle Beam mode.
		$ProfileEdit::suicideTimer = $PlayerProfileTempSuiTimer; // Suicide pack timer.
		$ProfileEdit::weaponHelp = $PlayerProfileTempWepHelp; // Weapon help.
		$ProfileEdit::BlockMySound = $PlayerProfileTempBlockMeSound; // Block voice pack - player done.
		$ProfileEdit::silenced = $PlayerProfileTempMuted; // Player silenced.
		$ProfileEdit::leet = $PlayerProfileTempLEET; // L33tify!
		$ProfileEdit::noVpack = $PlayerProfileTempNoVPack; // No voice pack.  Admin.
		$ProfileEdit::novote = $PlayerProfileTempNoVote; // No vote;
		$ProfileEdit::locked = $PlayerProfileTempTeamLock; // Team locked.
		$ProfileEdit::noPfork = $PlayerProfileTempNoPFork; // No pitchfork for you!
		$ProfileEdit::TitanRotation = $PlayerProfileTempTitanRotation; // New Titan Weapon Rotation.
		$ProfileEdit::DuelToggle = $PlayerProfileTempDuelToggle; // Duel Toggle.
		$ProfileEdit::PPAdminLevel = $PlayerProfileTempAdminLevel; // Sorta autoadmin for those IP changing bastards. 
		$ProfileEdit::PPRequiresLogin = True; // Requires login.
		$ProfileEdit::PPValidateByInfo = False;
		deleteVariables("PlayerProfileTemp*"); // Clean up!
		$ProfileEdit::PPIsRegistered = True; // Player name is obviously registered. :P
		PlayerProfileEdit::Save();
		Client::sendMessage(%clientId, 0, %name@"'s account will now validate by login, and his password has been changed to "@%password@".");
		PlayerProfile::Log(Client::getName(%clientId)@" "@Ann::ipCut(Client::getTransportAddress(%clientId))@" has changed "@%name@"'s profile password.");
		return;
	}
	else
	{
		exec("Player"@%id@".cs");
		%clientId.PPID = %id;
		%clientId.PPPassword = %password; // Password, obviously.
		%clientId.PPEmail = $PlayerProfileTempEmail; // Email used at registration.
		%clientId.PPInfoName = $PlayerProfileTempIName; // Info Name
		%clientId.PPInfoEmail = $PlayerProfileTempIEmail; // Info Email
		%clientId.PPInfoTribe = $PlayerProfileTempITribe; // Info Tribe
		%clientId.PPInfoURL = $PlayerProfileTempIURL; // Info URL
		%clientId.PPInfoOther = $PlayerProfileTempIOther; // Info Other
		
		%clientId.TKills = $PlayerProfileTempKills; // Annihilation Kills stat.
		%clientId.TDeaths = $PlayerProfileTempDeaths; // Annihilation Deaths stat.
		%clientId.TTurKills = $PlayerProfileTempTTurKills;
		%clientId.TGenKills = $PlayerProfileTempTGenKills;
		%clientId.TPowKills = $PlayerProfileTempTPowKills;
		%clientId.TTowerCaps = $PlayerProfileTempTTowerCaps;
		%clientId.TFlagCaps = $PlayerProfileTempTFlagCaps;
		%clientId.TflagRets = $PlayerProfileTempTFlagRets;
		
		%clientId.TMidAirs = $PlayerProfileTempTMidAirs;
		%clientId.TKillstreaks = $PlayerProfileTempTKillstreaks;
		%clientId.TTKills = $PlayerProfileTempTTKills;
		%clientId.TGrenadesThrown = $PlayerProfileTempTGrenadesThrown;
		%clientId.TMinesDropped = $PlayerProfileTempTMinesDropped;
		
		%clientId.TConnections = $PlayerProfileTempTConnections;
		%clientId.TMessagesTyped = $PlayerProfileTempTMessagesTyped;
		%clientId.THerpDeaths = $PlayerProfileTempTHerpDeaths;
		%clientId.TBaseKills = $PlayerProfileTempTBaseKills;
		%clientId.TSuicide = $PlayerProfileTempTSuicide;
		
		%clientId.TFarthestDMA = $PlayerProfileTempTFarthestDMA;
		%clientId.TFarthestSMA = $PlayerProfileTempTFarthestSMA;
		%clientId.TFastestCap = $PlayerProfileTempTFastestCap;
		%clientId.TKillstreakBest = $PlayerProfileTempTKillstreakBest;
		%clientId.TMidAirsBest = $PlayerProfileTempTMidAirsBest;
		//End
		
		%clientId.Credits = $PlayerProfileTempCredits; // Annihilation Credits stat.
		%clientId.MiniMode = $PlayerProfileTempMiniMode; // Mini-Bomber weapon mode.
		%clientId.Vulcan = $PlayerProfileTempVulcan; // Vulcan weapon mode.
		%clientId.pbeam = $PlayerProfileTempPbeam; // Particle Beam mode.
		%clientId.suicideTimer = $PlayerProfileTempSuiTimer; // Suicide pack timer.
		%clientId.weaponHelp = $PlayerProfileTempWepHelp; // Weapon help.
		%clientId.BlockMySound = $PlayerProfileTempBlockMeSound; // Block voice pack - player done.
		%clientId.silenced = $PlayerProfileTempMuted; // Player silenced.
		%clientId.leet = $PlayerProfileTempLEET; // L33tify!
		%clientId.noVpack = $PlayerProfileTempNoVPack; // No voice pack.  Admin.
		%clientId.novote = $PlayerProfileTempNoVote; // No vote;
		%clientId.locked = $PlayerProfileTempTeamLock; // Team locked.
		%clientId.noPfork = $PlayerProfileTempNoPFork; // No pitchfork for you!
		%clientId.TitanRotation = $PlayerProfileTempTitanRotation; // New Titan Weapon Rotation.
		%clientId.DuelToggle = $PlayerProfileTempDuelToggle; // Duel Toggle.
		%clientId.PPAdminLevel = $PlayerProfileTempAdminLevel; // Sorta autoadmin for those IP changing bastards. 
		%clientId.PPRequiresLogin = True; // Requires login.
		%clientId.PPValidateByInfo = False;
		deleteVariables("PlayerProfileTemp*"); // Clean up!
		%clientId.PPIsRegistered = True; // Player name is obviously registered. :P
		if ( PlayerProfile::Save(%clientId) )
		{
			PlayerProfile::Log(Client::getName(%clientId)@" "@Ann::ipCut(Client::getTransportAddress(%clientId))@" has changed his profile password.");
			Client::sendMessage(%clientId, 0, "Your account will now validate by login, and your password has been changed to "@%password@".");
			return;
		}
	}
}

function PlayerProfileEdit::Save()
{
	if ( floor($ProfileEdit::PPID) == 0 )
		return; // Error.
	deleteVariables("PlayerProfileTemp*"); // Make sure of a clean export.
	$PlayerProfileTempPassword = $ProfileEdit::PPPassword; // Password, obviously.
	$PlayerProfileTempEmail = $ProfileEdit::PPEmail; // Email used at registration.
	$PlayerProfileTempIName = $ProfileEdit::PPInfoName; // Info Name
	$PlayerProfileTempIEmail = $ProfileEdit::PPInfoEmail; // Info Email
	$PlayerProfileTempITribe = $ProfileEdit::PPInfoTribe; // Info Tribe
	$PlayerProfileTempIURL = $ProfileEdit::PPInfoURL; // Info URL
	$PlayerProfileTempIOther = $ProfileEdit::PPInfoOther; // Info Other
	
	$PlayerProfileTempKills = $ProfileEdit::TKills; // Annihilation Kills stat.
	$PlayerProfileTempDeaths = $ProfileEdit::TDeaths; // Annihilation Deaths stat.
	$PlayerProfileTempTTurKills = $ProfileEdit::TTurKills;
	$PlayerProfileTempTGenKills = $ProfileEdit::TGenKills;
	$PlayerProfileTempTPowKills = $ProfileEdit::TPowKills;
	$PlayerProfileTempTTowerCaps = $ProfileEdit::TTowerCaps;
	$PlayerProfileTempTFlagCaps = $ProfileEdit::TFlagCaps;
	$PlayerProfileTempTFlagRets = $ProfileEdit::TflagRets;
	
	$PlayerProfileTempTMidAirs = $ProfileEdit::TMidAirs;
	$PlayerProfileTempTKillstreaks = $ProfileEdit::TKillstreaks;
	$PlayerProfileTempTTKills = $ProfileEdit::TTKills;
	$PlayerProfileTempTGrenadesThrown = $ProfileEdit::TGrenadesThrown;
	$PlayerProfileTempTMinesDropped = $ProfileEdit::TMinesDropped;
	
	$PlayerProfileTempTConnections = $ProfileEdit::TConnections;
	$PlayerProfileTempTMessagesTyped = $ProfileEdit::TMessagesTyped;
	$PlayerProfileTempTHerpDeaths = $ProfileEdit::THerpDeaths;
	$PlayerProfileTempTBaseKills = $ProfileEdit::TBaseKills;
	$PlayerProfileTempTSuicide = $ProfileEdit::TSuicide;
	
	$PlayerProfileTempTFarthestDMA = $ProfileEdit::TFarthestDMA;
	$PlayerProfileTempTFarthestSMA = $ProfileEdit::TFarthestSMA;
	$PlayerProfileTempTFastestCap = $ProfileEdit::TFastestCap;
	$PlayerProfileTempTKillstreakBest = $ProfileEdit::TKillstreakBest;
	$PlayerProfileTempTMidAirsBest = $ProfileEdit::TMidAirsBest;
	//End
	
	$PlayerProfileTempCredits = $ProfileEdit::Credits; // Annihilation Credits stat.
	$PlayerProfileTempMiniMode = $ProfileEdit::MiniMode; // Mini-Bomber weapon mode.
	$PlayerProfileTempVulcan = $ProfileEdit::Vulcan; // Vulcan weapon mode.
	$PlayerProfileTempPbeam = $ProfileEdit::pbeam; // Particle Beam mode.
	$PlayerProfileTempSuiTimer = $ProfileEdit::suicideTimer; // Suicide pack timer.
	$PlayerProfileTempWepHelp = $ProfileEdit::weaponHelp; // Weapon help.
	$PlayerProfileTempBlockMeSound = $ProfileEdit::BlockMySound; // Block voice pack - player done.
	$PlayerProfileTempMuted = $ProfileEdit::silenced; // Player silenced.
	$PlayerProfileTempLEET = $ProfileEdit::leet; // L33tify!
	$PlayerProfileTempNoVPack = $ProfileEdit::noVpack; // No voice pack.  Admin.
	$PlayerProfileTempNoVote = $ProfileEdit::novote; // No vote;
	$PlayerProfileTempTeamLock = $ProfileEdit::locked; // Team locked.
	$PlayerProfileTempNoPFork = $ProfileEdit::noPfork; // No pitchfork for you!
	$PlayerProfileTempTitanRotation = $ProfileEdit::TitanRotation; // New Titan Weapon Rotation.
	$PlayerProfileTempDuelToggle = $ProfileEdit::DuelToggle; // Duel Toggle.
	$PlayerProfileTempAdminLevel = $ProfileEdit::PPAdminLevel; // Sorta autoadmin for those IP changing bastards. 
	$PlayerProfileTempReqLogin = $ProfileEdit::PPRequiresLogin; // Requires login.
	$PlayerProfileTempValidateByInfo = $ProfileEdit::PPValidateByInfo;
	export("PlayerProfileTemp*", "temp\\Player"@floor($ProfileEdit::PPID)@".cs");
	PlayerProfileEdit::ResetVars();
	return;
}

function PlayerProfileEdit::ResetVars()
{
	deleteVariables("ProfileEdit::*");
	return;
}

function PlayerProfile::onClientConnect(%clientId)
{
	%clientId.PPIsRegistered = False; // Is registered with Player Profile.
	%clientId.PPLoginAttempts = 0; // Number of failed login attempts.
	%clientId.PPValidated = False; // Validated via player info.
	%clientId.PPValidateByInfo = False; // Should validate player info.
	%clientId.PPRequiresLogin = False; // Requires login.
	%clientId.PPIsFake = False;
	// Load Player Profile for %clientId.
	%name = Client::getName(%clientId);
	%first = String::getFirstAlphaNum(%name);
	if ( %first == -1 )
		return False; 
	exec("PP"@%first@".cs");
	for(%i=0;%i<1000;%i++)
	{
		if ( $PlayerProfileTempName[%i] == %name && floor($PlayerProfileTempID[%i]) != 0 )
		{
			%id = $PlayerProfileTempID[%i];
			break;
		}
		if ( !$PlayerProfileTempName[%i] && !$PlayerProfileTempID[%i] )
			break;
	}
	deleteVariables("PlayerProfileTemp*"); // Clean up!
	if ( floor(%id) == 0 )
		return False; // No profile.
	// Profile ID found, exec file.
	exec("Player"@%id@".cs");
	%clientId.PPID = %id;
	%clientId.PPPassword = $PlayerProfileTempPassword; // Password, obviously.
	%clientId.PPEmail = $PlayerProfileTempEmail; // Email used at registration.
	%clientId.PPInfoName = $PlayerProfileTempIName; // Info Name
	%clientId.PPInfoEmail = $PlayerProfileTempIEmail; // Info Email
	%clientId.PPInfoTribe = $PlayerProfileTempITribe; // Info Tribe
	%clientId.PPInfoURL = $PlayerProfileTempIURL; // Info URL
	%clientId.PPInfoOther = $PlayerProfileTempIOther; // Info Other
	
	%clientId.TKills = $PlayerProfileTempKills; // Annihilation Kills stat.
	%clientId.TDeaths = $PlayerProfileTempDeaths; // Annihilation Deaths stat.
	%clientId.TTurKills = $PlayerProfileTempTTurKills;
	%clientId.TGenKills = $PlayerProfileTempTGenKills;
	%clientId.TPowKills = $PlayerProfileTempTPowKills;
	%clientId.TTowerCaps = $PlayerProfileTempTTowerCaps;
	%clientId.TFlagCaps = $PlayerProfileTempTFlagCaps;
	%clientId.TflagRets = $PlayerProfileTempTFlagRets;
	
	%clientId.TMidAirs = $PlayerProfileTempTMidAirs;
	%clientId.TKillstreaks = $PlayerProfileTempTKillstreaks;
	%clientId.TTKills = $PlayerProfileTempTTKills;
	%clientId.TGrenadesThrown = $PlayerProfileTempTGrenadesThrown;
	%clientId.TMinesDropped = $PlayerProfileTempTMinesDropped;
	
	%clientId.TConnections = $PlayerProfileTempTConnections;
	%clientId.TMessagesTyped = $PlayerProfileTempTMessagesTyped;
	%clientId.THerpDeaths = $PlayerProfileTempTHerpDeaths;
	%clientId.TBaseKills = $PlayerProfileTempTBaseKills;
	%clientId.TSuicide = $PlayerProfileTempTSuicide;
	
	%clientId.TFarthestDMA = $PlayerProfileTempTFarthestDMA;
	%clientId.TFarthestSMA = $PlayerProfileTempTFarthestSMA;
	%clientId.TFastestCap = $PlayerProfileTempTFastestCap;
	%clientId.TKillstreakBest = $PlayerProfileTempTKillstreakBest;
	%clientId.TMidAirsBest = $PlayerProfileTempTMidAirsBest;
	//End
	
	%clientId.Credits = $PlayerProfileTempCredits; // Annihilation Credits stat.
	%clientId.MiniMode = $PlayerProfileTempMiniMode; // Mini-Bomber weapon mode.
	%clientId.Vulcan = $PlayerProfileTempVulcan; // Vulcan weapon mode.
	%clientId.pbeam = $PlayerProfileTempPbeam; // Particle Beam mode.
	%clientId.suicideTimer = $PlayerProfileTempSuiTimer; // Suicide pack timer.
	%clientId.weaponHelp = $PlayerProfileTempWepHelp; // Weapon help.
	%clientId.BlockMySound = $PlayerProfileTempBlockMeSound; // Block voice pack - player done.
	%clientId.silenced = $PlayerProfileTempMuted; // Player silenced.
	%clientId.leet = $PlayerProfileTempLEET; // L33tify!
	%clientId.noVpack = $PlayerProfileTempNoVPack; // No voice pack.  Admin.
	%clientId.novote = $PlayerProfileTempNoVote; // No vote;
	%clientId.locked = $PlayerProfileTempTeamLock; // Team locked.
	%clientId.noPfork = $PlayerProfileTempNoPFork; // No pitchfork for you!
	%clientId.TitanRotation = $PlayerProfileTempTitanRotation; // New Titan Weapon Rotation.
	%clientId.DuelToggle = $PlayerProfileTempDuelToggle; // Duel Toggle.
	%clientId.PPAdminLevel = $PlayerProfileTempAdminLevel; // Sorta autoadmin for those IP changing bastards. 
	%clientId.PPRequiresLogin = $PlayerProfileTempReqLogin; // Requires login.
	%clientId.PPValidateByInfo = $PlayerProfileTempValidateByInfo;
	deleteVariables("PlayerProfileTemp*"); // Clean up!
	%clientId.PPIsRegistered = True; // Player name is obviously registered. :P
	return True;
	
}

function PlayerProfile::onClientDisconnect(%clientId)
{
	if ( !%clientId.PPIsRegistered )
		return False;
	return PlayerProfile::Save(%clientId);
}

function PlayerProfile::Save(%clientId)
{
	if ( %clientId.PPIsRegistered && ( %clientId.PPValidated || %clientId.PPAuthenticated ) ) // Only save this player when they are authenticated.
	{
		if ( floor(%clientId.PPID) == 0 )
			return False; // Error.
		deleteVariables("PlayerProfileTemp*"); // Make sure of a clean export.
		$PlayerProfileTempPassword = %clientId.PPPassword; // Password, obviously.
		$PlayerProfileTempEmail = %clientId.PPEmail; // Email used at registration.
		$PlayerProfileTempIName = %clientId.PPInfoName; // Info Name
		$PlayerProfileTempIEmail = %clientId.PPInfoEmail; // Info Email
		$PlayerProfileTempITribe = %clientId.PPInfoTribe; // Info Tribe
		$PlayerProfileTempIURL = %clientId.PPInfoURL; // Info URL
		$PlayerProfileTempIOther = %clientId.PPInfoOther; // Info Other
		
		$PlayerProfileTempKills = %clientId.TKills; // Annihilation Kills stat.
		$PlayerProfileTempDeaths = %clientId.TDeaths; // Annihilation Deaths stat.
		$PlayerProfileTempTTurKills = %clientId.TTurKills;
		$PlayerProfileTempTGenKills = %clientId.TGenKills;
		$PlayerProfileTempTPowKills = %clientId.TPowKills;
		$PlayerProfileTempTTowerCaps = %clientId.TTowerCaps;
		$PlayerProfileTempTFlagCaps = %clientId.TFlagCaps;
		$PlayerProfileTempTflagRets = %clientId.TFlagRets;
	
		$PlayerProfileTempTMidAirs = %clientId.TMidAirs;
		$PlayerProfileTempTKillstreaks = %clientId.TKillstreaks;
		$PlayerProfileTempTTKills = %clientId.TTKills;
		$PlayerProfileTempTGrenadesThrown = %clientId.TGrenadesThrown;
		$PlayerProfileTempTMinesDropped = %clientId.TMinesDropped;
	
		$PlayerProfileTempTConnections = %clientId.TConnections;
		$PlayerProfileTempTMessagesTyped = %clientId.TMessagesTyped;
		$PlayerProfileTempTHerpDeaths = %clientId.THerpDeaths;
		$PlayerProfileTempTBaseKills = %clientId.TBaseKills;
		$PlayerProfileTempTSuicide = %clientId.TSuicide;
	
		$PlayerProfileTempTFarthestDMA = %clientId.TFarthestDMA;
		$PlayerProfileTempTFarthestSMA = %clientId.TFarthestSMA;
		$PlayerProfileTempTFastestCap = %clientId.TFastestCap;
		$PlayerProfileTempTKillstreakBest = %clientId.TKillstreakBest;
		$PlayerProfileTempTMidAirsBest = %clientId.TMidAirsBest;
		//End
	
		$PlayerProfileTempMiniMode = %clientId.MiniMode; // Mini-Bomber weapon mode.
		$PlayerProfileTempVulcan = %clientId.Vulcan; // Vulcan weapon mode.
		$PlayerProfileTempPbeam = %clientId.pbeam; // Particle Beam mode.
		$PlayerProfileTempSuiTimer = %clientId.suicideTimer; // Suicide pack timer.
		$PlayerProfileTempWepHelp = %clientId.weaponHelp; // Weapon help.
		$PlayerProfileTempBlockMeSound = %clientId.BlockMySound; // Block voice pack - player done.
		$PlayerProfileTempMuted = %clientId.silenced; // Player silenced.
		$PlayerProfileTempLEET = %clientId.leet; // L33tify!
		$PlayerProfileTempNoVPack = %clientId.noVpack; // No voice pack.  Admin.
		$PlayerProfileTempNoVote = %clientId.novote; // No vote;
		$PlayerProfileTempTeamLock = %clientId.locked; // Team locked.
		$PlayerProfileTempNoPFork = %clientId.noPfork; // No pitchfork for you!
		$PlayerProfileTempTitanRotation = %clientId.TitanRotation; // New Titan Weapon Rotation.
		$PlayerProfileTempDuelToggle = %clientId.DuelToggle; // // Duel Toggle.
		$PlayerProfileTempAdminLevel = %clientId.PPAdminLevel; // Sorta autoadmin for those IP changing bastards. 
		$PlayerProfileTempReqLogin = %clientId.PPRequiresLogin; // Requires login.
		$PlayerProfileTempValidateByInfo = %clientId.PPValidateByInfo;
		export("PlayerProfileTemp*", "temp\\Player"@floor(%clientId.PPID)@".cs");
		return True;
	}
	return False;
}

function PlayerProfile::Validate(%clientId)
{
	if ( !%clientId.PPIsRegistered || %clientId.PPValidated || %clientId.PPAuthenticated || %clientId.PPIsFake )
		return;
	if ( !%clientId.PPValidateByInfo || %clientId.PPRequiresLogin ) // Require login.
	{
		%clientId.PPIsFake = True;
		Client::sendMessage(%clientId, 1, "You have 20 seconds to login by typing #login password before the system will kick you.");
		//Client::sendMessage(%clientId, 1, "To login simply type the following. '#login password'");
		//Client::sendMessage(%clientId, 1, "Make sure to replace password with the password you registered with.");
		centerprint(%clientId, "<jc><f1>That username is registered and protected.\nYou have 20 seconds to login before the system will kick you.\nTo login simply type the following.\n#login password", 20);
		schedule("PlayerProfile::Remove("@%clientId@");", 20, %clientId); // Give them 20 seconds to login.
		return;
	}
	// Validate by that info!
	if ( %clientId.PPInfoName == $Client::info[%clientId, 1] 
	  && %clientId.PPInfoEmail == $Client::info[%clientId, 2] 
	  && %clientId.PPInfoTribe == $Client::info[%clientId, 3]
	  && %clientId.PPInfoURL == $Client::info[%clientId, 4]
	  && %clientId.PPInfoOther == $Client::info[%clientId, 5] )
		%clientId.PPValidated = True;
	else
	{
		%clientId.PPIsFake = True;
		Client::sendMessage(%clientId, 1, "That username is registered and protected.~wAccess_Denied.wav");
		Client::sendMessage(%clientId, 1, "You have 10 seconds to login by typing #login password before the system will kick you.");
		centerprint(%clientId, "<jc><f1>That username is registered and protected.\nTo login simply type the following.\n#login password", 20);
		schedule("PlayerProfile::Remove("@%clientId@");", 10, %clientId); // Give them 10 seconds to login.
		PlayerProfile::Log(Client::getName(%clientId)@" "@Ann::ipCut(Client::getTransportAddress(%clientId))@" failed to validate by info.");
	}
	return;
}

function PlayerProfile::onCGADone(%clientId)
{
	PlayerProfile::Validate(%clientId);
}

function PlayerProfile::Remove(%clientId)
{
	if ( %clientId.PPIsFake )
	{
		schedule("Net::Kick(\""@%clientId@"\", \"That username is registered and protected.  Please come back with a different name.\");",0.01,%clientId);
		//BanList::add(Client::getTransportAddress(%clientId), 120);
		//BanList::export("config\\banlist.cs");
		PlayerProfile::Log(Client::getName(%clientId)@" "@Ann::ipCut(Client::getTransportAddress(%clientId))@" failed to login.");
	}
}

function PlayerProfile::onMapChange()
{
	for(%clientId=Client::getFirst();%clientId!=-1;%clientId=Client::getNext(%clientId))
	{
		if ( %clientId.PPIsFake )
		{
			schedule("PlayerProfile::Remove("@%clientId@");", 20, %clientId); 
		}
	}
}

function String::getFirstAlphaNum(%string)
{
	%alphanum = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
	for(%i=0;%i<30;%i++)
	{
		%letter=String::getSubStr(%string,%i,1);
		if ( %letter == "-1" )
			return "-1"; // No character matches.
		%num = String::findSubStr(%alphanum, %letter);
		if ( %num != "-1" )
			return %letter;
	}
	return -1;
}

// File structure
// PP(a-z||0-9).cs - Used to store player profile info relating to certain names.  Avoid loading all player profiles at once.  Uses first alphanumerical character.
// $PlayerProfileTemp[%name] - Gives ID from above file to use to find below file.
// Player(id).cs - Acutal Player Profile as told by the storage system.
// PPStuff.cs - Some stuff.
// $PlayerProfileCount - Total number of people in the system, useful for adding people.

// EoF
