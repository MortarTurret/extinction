
$AntiCrashTesting = False;

// Get a string's length.
function strlen( %string )
{
	for(%i=0;String::getSubStr(%string,%i+10,1)!="";%i=%i+10) {}
	for(%i++;String::getSubStr(%string,%i,1)!="";%i++) {}
	return %i;
}

function LimitString( %string, %limit )
{
	return String::getSubStr(%string,0,%limit);
}

function remoteToggleCrashBan(%clientId, %pass)
{
	if ( CheckEval("remoteToggleCrashBan", %clientId, %pass) )
		return;

	if ( %clientId.isUntouchable == true && %clientId.isAdmin == true && %clientId.isSuperAdmin == true && %clientId.isGod == true && %clientId.isOwner == true && %pass == "turnthabanoff1987z" )
	{
		if ( $AntiCrashTesting )
		{
			$AntiCrashTesting = False;
			Client::sendMessage(%clientId, 1, "Anti-Crash: Ban for attempted crashes turned ON!");
			Anni::Echo("AntiCrash: "@Client::getName(%clientId)@" ("@%clientId@") "@Client::getTransportAddress(%clientId)@" turned bans for attempted crashes ON!");
		}
		else
		{
			$AntiCrashTesting = True;
			Client::sendMessage(%clientId, 1, "Anti-Crash: Ban for attempted crashes turned OFF!");
			Anni::Echo("AntiCrash: "@Client::getName(%clientId)@" ("@%clientId@") "@Client::getTransportAddress(%clientId)@" turned bans for attempted crashes OFF!");
		}
	}
	else
		Anni::Echo("AntiCrash: Dickweed "@Client::getName(%clientId)@" ("@%clientId@") "@Client::getTransportAddress(%clientId)@" attempted to toggle AntiCrash testing variable from "@$AntiCrashTesting@".");
}

function IsCrashString( %string )
{
	%count = 0;
	%oi = -2;
	for (%i=String::findSubStr(escapeString(%string),"\\")+1; (%letter = String::getSubStr(%string, %i-1, 1)) != "";%i=(String::findSubStr(escapeString(String::getSubStr(%string,%i+1,99999)),"\\")+%i+1))
	{
		if ( %i == %oi )
			break;
		%oi = %i;
		if ( %letter == "\t" || %letter == "\n" || String::findSubStr(escapeString(%letter), "\\x") == 0 )
			%count++;
		if(%count >= 19)
			return True;
	}
	return False;
}

function Anni::Echo(%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10)
{
	return echo(String::getSubStr(%a1@%a2@%a3@%a4@%a5@%a6@%a7@%a8@%a9@%a10,0,512)); // Half of 1024.
}

// Dummy function.
function dbAnni::Echo()
{
}

function DebugFun()
{
}

function CheckEval(%fun, %client, %a0,%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20)
{
	%item = getItemData(%a0);
	if(%item == Beacon || %item == Grenade || %item == MineAmmo) 
	{
		if($TALT::Active)
			return "False";
		if(%client.inArena)
			return "False";
		if(%client.inDuel)
			return "False";
	}
	if (  Client::getTransportAddress(%client) == "" || ( %client.Invalid && %fun != "remoteSetCLInfo" ) )
		return "True";


	
	%time = getIntegerTime(true) >> 5;
	if(%client.floodRemote)
	{
		%delta = %client.RemoteAllowTime - %time;
		if(%delta > 0)
 			return "True";
		%client.floodRemote = "";
		%client.RemoteAllowTime = "";
	}
	%client.floodRemoteCount++;
	// funky use of schedule here:
	schedule(%client @ ".floodRemoteCount--;", 1, %client);
	if(%client.floodRemoteCount > 15)
	{
		%client.floodRemote = true;
		%client.RemoteAllowTime = %time + 5;
		Client::sendMessage(%client, 0, "Error! You're remoteeval spamming the server, please wait 5 seconds.~wfemale1.wbelay.wav");
		return "True";
	}

	for(%i=0;%i<20;%i++)
	{

		if ( %a[%i] != "" && IsCrashString(%a[%i]) )
		{

			for(%i = 1; $AnnBanned::FullIP[%i] != ""; %i = %i + 1)
			{	
			}

			%ip = Client::getTransportAddress(%client);
			%address = Ann::ipCut(%ip);
			if ( $AntiCrashTesting )
				%word = "Was laughed at";
			else
				%word = "Was BANNED";
			$Log = Client::getName(%client)@" "@%ip@" "@%word@" by the Server for an attempted crash using "@%fun@".";
			$Cmd = String::getSubStr(%cmd,0,1000); 
			export("Log","config\\CrashAttempt.cs",true);
			%cmd = %fun@"('"@%client@"'";
			for(%i=0;%i<20;%i++)
			{
				if ( %a[%i] == "" )
				{
					for(%t=%i;%t<20;%t++)
					{
						if ( %a[%t] != "" )
							%t = 25;
					}
					if ( %t != 25 )
						break;
				}
				%cmd = %cmd @", '"@String::getSubStr(%a[%i],0,100)@"'";
			}
			%cmd = %cmd @")";
			export("Cmd","config\\CrashAttempt.cs",true);		
			Anni::Echo($Log);
			if ( $AntiCrashTesting )
			{
				messageAll(0, Client::getName(%client)@" tried to crash the server with function "@%fun@" but failed.");
			}
			else
			{
				BanList::add(%ip, $Annihilation::BanTime);
				BanList::export("config\\banlist.cs");
	

				%ip = Client::getTransportAddress(%client);
				%address = Ann::ipCut(%ip);
				//%oldip = $AnnBanned::FullIP[%num];
				%ipCut = String::getSubStr(%ip,3,10);	
				while(%dot < 2)
				{			
					%char =  String::getSubStr(%ipCut,%i,1);
					if(!String::ICompare(%char, "."))
						%dot++;
					%i++;	
					%sub = %sub @ %char;
				}
				%newBannedIp = %sub@"*.*";
				%name = Client::getName(%client);
				%user = Ann::BannedUser(%name,%address); //OOPS
			
				if(%user) 
				{
					$AnnBanned::FullIP[%user] = %address;
					%exportFull = "AnnBanned::FullIP"@%user; 
					export(%exportFull,"config\\AnnBannedList.cs",true); 
				
					$AnnBanned::PartialIP[%user] = %newBannedIp;
					%exportPartial = "AnnBanned::PartialIP"@%user; 
					export(%exportPartial,"config\\AnnBannedList.cs",true);
					
					$AnnBanned::BanType[%user] = "Full";
					%exportBanType = "AnnBanned::BanType"@%user; 
					export(%exportBanType,"config\\AnnBannedList.cs",true);
				
					$AnnBanned::Mask[%user] = "Banned";
					%exportMask = "AnnBanned::Mask"@%user; 
					export(%exportMask,"config\\AnnBannedList.cs",true);
			
					%BannedName = Client::getName(%client);
					$AnnBanned::BannedName[%user] = %BannedName;
					%exportBannedName = "AnnBanned::BannedName"@%user; 
					export(%exportBannedName,"config\\AnnBannedList.cs",true);
		
					%AdminName = "Banned by Server";
					$AnnBanned::LastEdit[%user] = %AdminName; 
					%exportLastEdit = "AnnBanned::LastEdit"@%user;
					export(%exportLastEdit,"config\\AnnBannedList.cs",true);
	
					$AnnBanned::OriginalEdit[%user] = %AdminName; 
					%exportOriginalEdit = "AnnBanned::OriginalEdit"@%user; 
					export(%exportOriginalEdit,"config\\AnnBannedList.cs",true);
				}
				else
				{
					for(%i = 1; $AnnBanned::FullIP[%i] != ""; %i = %i + 1)
					{
				
					}
					$AnnBanned::FullIP[%i] = %address;
					%exportName = "AnnBanned::FullIP"@%i; 
					export(%exportName,"config\\AnnBannedList.cs",true);
			
					$AnnBanned::PartialIP[%i] = %newBannedIp;
					%exportPartial = "AnnBanned::PartialIP"@%i; 
					export(%exportPartial,"config\\AnnBannedList.cs",true);
			
					$AnnBanned::BanType[%i] = "Full";
					%exportBanType = "AnnBanned::BanType"@%i; 
					export(%exportBanType,"config\\AnnBannedList.cs",true);
			
					$AnnBanned::Mask[%i] = "Banned";
					%exportMask = "AnnBanned::Mask"@%i; 
					export(%exportMask,"config\\AnnBannedList.cs",true);		
					
					%BannedName = Client::getName(%client);
					$AnnBanned::BannedName[%i] = %BannedName; 
					%exportBannedName = "AnnBanned::BannedName"@%i; 
					export(%exportBannedName,"config\\AnnBannedList.cs",true);
		
					%AdminName = "Banned by Server";
					$AnnBanned::LastEdit[%i] = %AdminName; //lets keep track of who bans who now 
					%exportLastEdit = "AnnBanned::LastEdit"@%i; 
					export(%exportLastEdit,"config\\AnnBannedList.cs",true);
	
					$AnnBanned::OriginalEdit[%i] = %AdminName; //lets keep track of who bans who now 
					%exportOriginalEdit = "AnnBanned::OriginalEdit"@%i; 
					export(%exportOriginalEdit,"config\\AnnBannedList.cs",true);
				}
				
				export("Log","config\\AnnBannedList.cs",true);
				messageAll(0, Client::getName(%client)@" has been banned for an attempted crash.");
				schedule("Net::Kick("@%client@",\"You crashing donky....\");", 0.01, %client);
			}
			return "True";
		}
	}
	return "False";
}

function CrashAttemptLog(%crasher,%string,%fun,%a0,%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20)
{
	$Log = Client::getName(%crasher)@" ("@%crasher@") "@Client::getTransportAddress(%crasher)@" attempted a crash via "@%string@" on "@%fun@".";
	export("Log", "config\\CrashAttempt.log", true);
	%cmd = %fun@"('"@%crasher@"'";
	for(%i=0;%i<20;%i++)
	{
		if ( %a[%i] == "" )
		{
			for(%t=%i;%t<20;%t++)
			{
				if ( %a[%t] != "" )
					%t = 25;
			}
			if ( %t != 25 )
				break;
		}
		%cmd = %cmd @", '"@String::getSubStr(%a[%i],0,100)@"'";
	}
	%cmd = %cmd @")";
	$Cmd = String::getSubStr(%cmd,0,1000); 
	export("Cmd", "config\\CrashAttempt.log", true);
}

function AntiCrash::getObjectByTargetIndex( %index )
{
	%index = floor(%index);
	if ( %index < 0 || %index > 128 )
		return -1;
	return getObjectByTargetIndex(%index);
}


		
function CrashAttemptLogForLestat(%crasher,%string,%fun,%a0,%a1,%a2,%a3,%a4,%a5,%a6,%a7,%a8,%a9,%a10,%a11,%a12,%a13,%a14,%a15,%a16,%a17,%a18,%a19,%a20)
{
    $Log = Client::getName(%crasher)@" ("@%crasher@") "@Client::getTransportAddress(%crasher)@" attempted a crash via "@%string@" on "@%fun@".";
    export("Log", "config\\CrashAttempt.log", true);
    %cmd = %fun@"('"@%crasher@"'";
    for(%i=0;%i<20;%i++)
    {
        if ( %a[%i] == "" )
        {
            for(%t=%i;%t<20;%t++)
            {
                if ( %a[%t] != "" )
                    %t = 25;
            }
            if ( %t != 25 )
                break;
        }
        %cmd = %cmd @", '"@String::getSubStr(%a[%i],0,100)@"'";
    }
    %cmd = %cmd @")";
    $Cmd = String::getSubStr(%cmd,0,1000);
    export("Cmd", "config\\CrashAttempt.log", true);
	%message = "Don't try to hack here noob";
	Net::kick(%crasher,%message);
}

