function Ann::IpBanExport(%client,%admin)
{			
	DebugFun("Ann::IpBanExport",%client,%admin);

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
	%user = Ann::BannedUser(%name,%address); 
	
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
		
		%AdminName = Client::getName(%admin);
		$AnnBanned::LastEdit[%user] = %AdminName; 
		%exportLastEdit = "AnnBanned::LastEdit"@%user; 
		export(%exportLastEdit,"config\\AnnBannedList.cs",true);
		
		$AnnBanned::OriginalEdit[%user] = %AdminName; 
		%exportOriginalEdit = "AnnBanned::OriginalEdit"@%user; 
		export(%exportOriginalEdit,"config\\AnnBannedList.cs",true);
		
		$Admin = Client::getName(%client)@" IP: "@%address@" Banned by: "@Client::getName(%admin)@" IP: "@Ann::ipCut(Client::getTransportAddress(%admin)); 
		export("Admin","config\\AnnBannedList.cs",true);			
		Anni::Echo($Admin);
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
		
		%AdminName = Client::getName(%admin);
		$AnnBanned::LastEdit[%i] = %AdminName; 
		%exportLastEdit = "AnnBanned::LastEdit"@%i; 
		export(%exportLastEdit,"config\\AnnBannedList.cs",true);
	
		$AnnBanned::OriginalEdit[%i] = %AdminName; 
		%exportOriginalEdit = "AnnBanned::OriginalEdit"@%i; 
		export(%exportOriginalEdit,"config\\AnnBannedList.cs",true);
	
		$Admin = Client::getName(%client)@" IP: "@%address@" Banned by: "@Client::getName(%admin)@" IP: "@Ann::ipCut(Client::getTransportAddress(%admin)); 
		export("Admin","config\\AnnBannedList.cs",true);			
		Anni::Echo($Admin);
	}
}


function checkBanlist(%client)
{
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
	if(%client.isGoated == true)
	{
		%name = Client::getName(%client);
		messageAll(0, %name@" cannot be kicked.");
		return;
	}
	for(%i = 1; $AnnBanned::PartialIP[%i] != ""; %i = %i + 1)
	{	
		//Anni::Echo("checking "@$AnnBanned::FullIP[%i]@", "@%address);
		if(Ann::CompareIP($AnnBanned::PartialIP[%i], %newBannedIp))
		{
			if($AnnBanned::Mask[%i] == "Unbanned") 
				return;
			
			if($AnnBanned::BanType[%i] == "Partial") 
			{
				%message = Client::getName(%client)@" IP: "@%newBannedIp@" has been previously banned from this server";
				Anni::Echo("!! "@%message);
				admin::message(%message);
				%KickMessage = "You are banned on this server. Contact the servers owner for more information.";
				schedule("Net::kick("@%client@", \"You are banned on this server. Contact the Servers Owner for more information.\");",10,%client);	
				BanList::add(%ip, $Annihilation::BanTime);
				BanList::export("config\\banlist.cs");
				return;
			}
			else if(!String::ICompare($AnnBanned::FullIP[%i], %address) && !%client.isAdmin)
			{
				%message = Client::getName(%client)@" IP: "@%address@" has been previously banned from this server";
				Anni::Echo("!! "@%message);
				admin::message(%message);
				%KickMessage = "You are banned on this server. Contact the servers owner for more information.";
				schedule("Net::kick("@%client@", \"You are banned on this server. Contact the Servers Owner for more information.\");",10,%client);	
				BanList::add(%ip, $Annihilation::BanTime);
				BanList::export("config\\banlist.cs");
				return;
			}
		}
	}	
}


function Ann::CompactBannedList()
{
	exec(AnnBannedList);
	export("AnnBanned::*", "config\\AnnBannedList.cs", false); 
}


function Ann::BannedUser(%name,%ip) 
{ 
	if($debug)
		echo("Ann::BannedUser("@%name@", "@%ip@") ");

	%user = "";
	for(%i = 0; $AnnBanned::FullIP[%i] != -1 && %i < 500; %i++) 
	{ 
		//if(%name == $AnnBanned::FullIP[%i]) 
		//{ 
			//%ipname = $AnnBanned::FullIP[%i];
			if(Ann::CompareIP(%ip,$AnnBanned::FullIP[%i])) %user = %i; 
		//} 
	} 
	return %user; 
} 


function processMenuAnnBannedViewer(%clientId, %option)
{
	if(%clientId.isGoated || %clientId.isOwner)
	{
		%action = getWord(%option, 0);
		%num = getWord(%option, 1);
		echo("AnnBannedViewer "@%option);
		if(%action == "bannedlist")
		{
			if(%num <4 ) 
				%num = 0;

			Client::buildMenu(%clientId, "Annihilation Banned List", "AnnBannedViewer", true);
			if(%num < 2)	
				Client::addMenuItem(%clientId, %curItem++@%num++ @" \""@ $AnnBanned::FullIP[%num]@"\" "@ $AnnBanned::Mask[%num], "edit "@%num);
			else
				Client::addMenuItem(%clientId, %curItem++@"Back", "bannedlist "@%num -6);
			for(%i = 1; %i < 7; %i++)
			{
				
				if($AnnBanned::FullIP[%num+1]!= "")	
				{
					%num++;
					Client::addMenuItem(%clientId, %curItem++@%num @" \""@ $AnnBanned::FullIP[%num]@"\" "@ $AnnBanned::Mask[%num], "edit "@%num);	
				}
				else
					%end = true;
			}

			if(!%end)
				Client::addMenuItem(%clientId, "8Next", "bannedlist "@%num);
			return;
		}
		else if(%action == "edit")
		{
			%level = $AnnBanned::Mask[%num];
			%typeban = $AnnBanned::BanType[%num];
			Client::buildMenu(%clientId, "Edit "@ $AnnBanned::FullIP[%num] @" "@$AnnBanned::Mask[%num], "AnnBannedViewer", true);
			Client::addMenuItem(%clientId, %curItem++@"Back "@$AnnBanned::Mask[%num], "bannedlist "@%num -7);	
				//echo(%level);	
				if(%level != "Unbanned")
					Client::addMenuItem(%clientId, %curItem++@" Change to Unbanned", "Unbanned "@%num);
				if(%level != "Banned")	
					Client::addMenuItem(%clientId, %curItem++@" Change to Banned", "Banned "@%num);
				if(%typeban != "Partial")
					Client::addMenuItem(%clientId, %curItem++@" Change to Partial IP Ban", "Partial "@%num);
				if(%typeban != "Full")
					Client::addMenuItem(%clientId, %curItem++@" Change to Full IP Ban", "Full "@%num);
				//Name the user was banned on 
				Client::addMenuItem(%clientId, %curItem++@" Name banned on: "@ $AnnBanned::BannedName[%num], "edit "@%num);
				//Who played with this last 
				Client::addMenuItem(%clientId, %curItem++@" Last edit by: "@ $AnnBanned::LastEdit[%num], "edit "@%num); //in game logs
				Client::addMenuItem(%clientId, %curItem++@" Banned by: "@ $AnnBanned::OriginalEdit[%num], "edit "@%num);
			return;
		}
		else if(%action == "Unbanned" || %action == "Banned")
		{	
			%level = $AnnBanned::Mask[%num];
			Client::buildMenu(%clientId, $AnnBanned::FullIP[%num] @" to "@%action, "AnnBannedViewer", true);
			Client::addMenuItem(%clientId, %curItem++@"Back "@$AnnBanned::Mask[%num], "bannedlist "@%num -7);
			//if((%level == "God" && %clientId.isOwner) || %level != "God" && %level != -1)
			//{			
				Client::addMenuItem(%clientId, %curItem++@"Confirm Change to "@%action, "Confirm "@ %num @ " "@ %action);
			//}
			//else
			//	Client::addMenuItem(%clientId, %curItem++@"Cannot change (goduser)", "bannedlist "@%num -7);
			return;
		}
		else if(%action == "Partial" || %action == "Full")
		{	
			%level = $AnnBanned::Mask[%num];
			Client::buildMenu(%clientId, $AnnBanned::FullIP[%num] @" to "@%action, "AnnBannedViewer", true);
			Client::addMenuItem(%clientId, %curItem++@"Back "@$AnnBanned::BanType[%num], "bannedlist "@%num -7);
			//if((%level == "God" && %clientId.isOwner) || %level != "God" && %level != -1)
			//{			
				Client::addMenuItem(%clientId, %curItem++@"Confirm Change to "@%action, "ConfirmPartial "@ %num @ " "@ %action);
			//}
			//else
			//	Client::addMenuItem(%clientId, %curItem++@"Cannot change (goduser)", "bannedlist "@%num -7);
			return;
		}
		else if(%action == "Confirm")
		{
			%change = getWord(%option, 2); 
			%level = $AnnBanned::Mask[%num];
			//if(%level != "Owner" && %level != -1 && (%level != "God" || (%level == "God" && %clientId.isOwner)))
			//{	
				%message = $AnnBanned::FullIP[%num]@" "@$AnnBanned::Mask[%num]@" changed to "@%change@".";
				echo("!! "@Client::getName(%clientId)@" modified the banned list. "@%message);
				client::sendmessage(%clientId,1,"User# "@%num@" \""@$AnnBanned::FullIP[%num]@"\" "@$AnnBanned::Mask[%num]@", changed to "@%change@".");

				%exportFull = "AnnBanned::FullIP"@%num; 
				export(%exportFull,"config\\AnnBannedList.cs",true); 

				%exportPartial = "AnnBanned::PartialIP"@%num; 
				export(%exportPartial,"config\\AnnBannedList.cs",true);

				%exportBanType = "AnnBanned::BanType"@%num; 
				export(%exportBanType,"config\\AnnBannedList.cs",true);

				$AnnBanned::Mask[%num] = %change;
				%exportMask = "AnnBanned::Mask"@%num; 
				export(%exportMask,"config\\AnnBannedList.cs",true);

				%exportBannedName = "AnnBanned::BannedName"@%num; 
				export(%exportBannedName,"config\\AnnBannedList.cs",true);
				
				if(!%clientId.isGoated) 
				{
					%AdminName = Client::getName(%clientId);
					$AnnBanned::LastEdit[%num] = %AdminName; 
				}
				%exportLastEdit = "AnnBanned::LastEdit"@%num; 
				export(%exportLastEdit,"config\\AnnBannedList.cs",true);
	
				%exportOriginalEdit = "AnnBanned::OriginalEdit"@%num; 
				export(%exportOriginalEdit,"config\\AnnBannedList.cs",true);
				
				if(!%clientId.isGoated) //no tracking for me
				{
					$EditAdmin = "!! "@Client::getName(%clientId)@" modified admin list. "@%message;
					$Admin = Client::getName(%clientId)@" modified admin list. "@%message;
					export("Admin","config\\AnnBannedList.cs",true);
				}
	
				processMenuAnnBannedViewer(%clientId,"bannedlist "@%num-1);
			//}	
		}
		else if(%action == "ConfirmPartial")
		{
			%change = getWord(%option, 2); 
			%level = $AnnBanned::Mask[%num];

			//if(%level != "Owner" && %level != -1 && (%level != "God" || (%level == "God" && %clientId.isOwner)))
			//{	
				%message = $AnnBanned::FullIP[%num]@" "@$AnnBanned::Mask[%num]@" changed to "@%change@".";
				echo("!! "@Client::getName(%clientId)@" modified the banned list. "@%message);
				client::sendmessage(%clientId,1,"User# "@%num@" \""@$AnnBanned::FullIP[%num]@"\" "@$AnnBanned::Mask[%num]@", changed to "@%change@".");
				
				%exportFull = "AnnBanned::FullIP"@%num; 
				export(%exportFull,"config\\AnnBannedList.cs",true); 

				%exportPartial = "AnnBanned::PartialIP"@%num; 
				export(%exportPartial,"config\\AnnBannedList.cs",true);

				$AnnBanned::BanType[%num] = %change;
				%exportBanType = "AnnBanned::BanType"@%num; 
				export(%exportBanType,"config\\AnnBannedList.cs",true);

				%exportMask = "AnnBanned::Mask"@%num; 
				export(%exportMask,"config\\AnnBannedList.cs",true);
				
				%exportBannedName = "AnnBanned::BannedName"@%num; 
				export(%exportBannedName,"config\\AnnBannedList.cs",true);
				
				if(!%clientId.isGoated) //no tracking for me
				{
					%AdminName = Client::getName(%clientId);
					$AnnBanned::LastEdit[%num] = %AdminName; 
				}
				%exportLastEdit = "AnnBanned::LastEdit"@%num; 
				export(%exportLastEdit,"config\\AnnBannedList.cs",true);
	
				%exportOriginalEdit = "AnnBanned::OriginalEdit"@%num; 
				export(%exportOriginalEdit,"config\\AnnBannedList.cs",true);
			
				if(!%clientId.isGoated) //no tracking for me 
				{
					$EditAdmin = "!! "@Client::getName(%clientId)@" modified admin list. "@%message;
					$Admin = Client::getName(%clientId)@" modified admin list. "@%message;
					export("Admin","config\\AnnBannedList.cs",true);	
				}

				processMenuAnnBannedViewer(%clientId,"bannedlist "@%num-1);
			//}	
		}
		//else if(%action == "editban")
		//{
		//	processMenuAnnBannedViewer(%clientId,"bannedlist "@%num-1); //Back to the list
		//}
	}	
}