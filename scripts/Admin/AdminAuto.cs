
function Ann::StrLen(%string)
{
	while(String::getSubStr(%string,%len,1) != "" && %len < 255)			
		%len++;	
	return %len;	
}



function Ann::Replace(%string, %search, %replace)
{
	%len = Ann::StrLen(%search);
	for (%i = 0; (%char = String::getSubStr(%string, %i, %len)) != "" && %i < 300; %i++)
	{
		if (%char @ "s" == %search @ "s") 
			%string = String::getSubStr(%string, 0, %i) @ %replace @ String::getSubStr(%string, %i + %len, 255);
	}
	return %string;
}

// compares ip mask to clients ip
function Ann::CompareIP(%ip, %mask) 
{ 
	%ipClnt = Ann::Replace(%ip, ".", " ");		
	%ipMask = Ann::Replace(%mask, ".", " ");	
	for(%x=0;%x<4;%x++) 
	{ 
		%ipC = getWord(%ipClnt, %x); 
		%ipM = getWord(%ipMask, %x); 
		if((%ipC != %ipM) && (%ipM != "*")) return "false"; 
	} 
	return "true"; 
} 

// compares TA Developer ip
function TA::CompareIP(%ip, %mask) 
{ 
	%ipClnt = Ann::Replace(%ip, ".", " ");		
	%ipMask = Ann::Replace(%mask, ".", " ");	
	for(%x=0;%x<4;%x++) 
	{ 
		%ipC = getWord(%ipClnt, %x); 
		%ipM = getWord(%ipMask, %x); 
		if((%ipC != %ipM) && (%ipM != "*")) return "false"; 
	} 
	return "true"; 
} 


function Ann::IPCut(%address)
{

	if(String::getSubStr(%address,0,8) == "LOOPBACK")
		return "LOCAL";
		
	%ipCut = String::getSubStr(%address,3,20);	
	while(String::getSubStr(%ipCut,%len,1) != ":" && %len < 20)			
		%len++;	
	%sub = String::getSubStr(%ipCut,0,%len);
	return %sub;		
}



function Ann::NumUsers() 
{ 
	exec(AnnAdminList);
	for(%i = 1; $AnnAdmin::Name[%i] != "" && %i < 500; %i++) 
	{ 
	} 
	return %i - 1; 
} 


function Ann::CompactAdminList()
{
	exec(AnnAdminList);
	export("AnnAdmin::*", "config\\AnnAdminList.cs", false);
}



function Ann::AdminUser(%name,%ip) 
{ 
	if($debug)
		echo("Ann::AdminUser("@%name@", "@%ip@") ");

	%user = "";
	for(%i = 0; $AnnAdmin::Name[%i] != -1 && %i < 500; %i++) 
	{ 
		if(%name == $AnnAdmin::Name[%i]) 
		{ 
			%mask = getWord($AnnAdmin::Mask[%i], 1);	
			if(Ann::CompareIP(%ip,%mask)) %user = %i;	 
		} 
	} 
	return %user; 
} 

function whoison() 
{ 
	WhatTime();
	echo("  #  CL#   NAME              Address"); 
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
	} 
}
	
//------------------------------------ Admin list Checker, determines level if any on connect -----------------

function Ann::AutoAdmin(%clientId,%ip) 
{ 
	if($Annihilation::AutoAdmin) 
	{ 
		%name = Client::getName(%clientId); 
		%user = Ann::AdminUser(%name,%ip); 

		if(%user) 
		{ 
			%username = $AnnAdmin::Name[%user]; 
			%flags = getWord($AnnAdmin::Mask[%user], 0); 
			echo("Admin: User: ",%username," identified. Access level: ",%flags); 
			if(%flags == "Owner") 
			{ 
				%clientId.isUntouchable = true;
				%clientId.isAdmin = true; 
				%clientId.isSuperAdmin = true; 
				%clientId.isGod = true;
				%clientId.isOwner = true;
				Client::sendMessage(%clientId, 1,"Owner: " @ %username @ " automatically assigned."); 
				Game::refreshClientScore(%clientId);
			}
			else if(%flags == "God") 
			{ 
				%clientId.isUntouchable = true;
				%clientId.isAdmin = true; 
				%clientId.isSuperAdmin = true; 
				%clientId.isGod = true; 
				Client::sendMessage(%clientId, 1,"God Admin: " @ %username @ " automatically assigned."); 
				Game::refreshClientScore(%clientId);
			} 						
			if(%flags == "Super") 
			{ 
				%clientId.isUntouchable = true;
				%clientId.isAdmin = true; 
				%clientId.isSuperAdmin = true;
				Client::sendMessage(%clientId, 1,"Super Admin: " @ %username @ " automatically assigned."); 
				Game::refreshClientScore(%clientId);
			}
			else if(%flags == "Public") 
			{ 
				%clientId.isUntouchable = true;
				%clientId.isAdmin = true; 
				Client::sendMessage(%clientId, 1,"Public Admin: " @ %username @ " automatically assigned.");
				Game::refreshClientScore(%clientId);	
			} 
			else if(%flags == "Safe") 
			{ 
				%clientId.isUntouchable = true; 
				Client::sendMessage(%clientId, 1,"Untouchable: " @ %username @ " automatically assigned."); 
				Game::refreshClientScore(%clientId);
			} 			
		} 
		else 
		{ 
			echo("Admin: User: ",%name," Unknown."); 
		}
	}
} 



//------------------------------------ Admin List manipulation functions --------------------------------------


function Admin::Owner(%client)
{
	%client.isAdmin = "true";
	%client.isSuperAdmin = "true";
	%client.isGod = "true";
	%client.isOwner = "true";
	Game::refreshClientScore(%client);
	if(Ann::AddAdmin(2048,%client,"Owner"))
	{
		Client::sendMessage(%client, 0, "You have been given Owner status, and have been added to the auto admin list.~wCapturedTower.wav");
		bottomprint(%client, "<f1><jc>You have been given Owner status, and have been added to the auto admin list.", 20);
	}
	else
	{
		Client::sendMessage(%client, 0, "You have been given Owner status.~wCapturedTower.wav");
		bottomprint(%client, "<f1><jc>You have been given Owner status.", 20);		
	}
	echo("isAdmin: ",%client.isAdmin,". isSuperAdmin: ",%client.isSuperAdmin,". isGod: ",%client.isGod,". isOwner: ",%client.isOwner);
}


function Ann::AddAdmin(%admin,%client,%level)
{
	echo("!! Ann::AddAdmin "@%admin@", "@%client@", "@%level);
	if(%admin.isGoated || %admin.isOwner || %admin == 2048)
	{	
		%ip = Client::getTransportAddress(%client);
		%name = Client::getName(%client);
		if(!String::NCompare(%ip, "LOOPBACK", 8))
		{

			echo("!! admin user = LOOPBACK, aborting operation.");
			return;
		}	
   		%user = Ann::AdminUser(%name,Ann::ipCut(%ip));
   		if(%user)
   		{
   			%userNumber = %user;
   			%change = true;
   			echo("!! Existing user: "@$AnnAdmin::Name[%user]@" "@getword($AnnAdmin::Mask[%user],0)@" changed to "@$AnnAdmin::Name[%user]@" "@%level);
   		}
   		else
			%userNumber = Ann::NumUsers() +1;
			

		%ipCut = String::getSubStr(%ip,3,10);	
		while(%dot < 2)
		{			
			%char =  String::getSubStr(%ipCut,%i,1);
			if(!String::ICompare(%char, "."))
				%dot++;
			%i++;	
			%sub = %sub @ %char;
		}
		%newAdminIp = %sub@"*.*";
		
		if(%change)
			client::sendmessage(%admin,1,"User# "@%userNumber@" \""@$AnnAdmin::Name[%userNumber]@"\" "@getword($AnnAdmin::Mask[%userNumber],0)@", changed to "@%level@".");
		else
			client::sendmessage(%admin,1,"User# "@%userNumber@" \""@%name@"\" Level: "@%level@" Added.");
		
		$AnnAdmin::Name[%userNumber] = %name;
		%exportName = "AnnAdmin::Name"@%userNumber; 
		export(%exportName, "config\\AnnAdminList.cs", true); 
		
		$AnnAdmin::Mask[%userNumber] = %level@" "@%newAdminIp;
		%exportMask = "AnnAdmin::Mask"@%userNumber;
		export(%exportMask, "config\\AnnAdminList.cs", true);
		
		%AdminName = Client::getName(%admin); 
		$AnnAdmin::LastEdit[%userNumber] = %AdminName;
		%exportLastEdit = "AnnAdmin::LastEdit"@%userNumber;
		export(%exportLastEdit, "config\\AnnAdminList.cs", true);
		
		$AnnAdmin::OriginalEdit[%userNumber] = %AdminName;
		%exportOriginalEdit = "AnnAdmin::OriginalEdit"@%userNumber;
		export(%exportOriginalEdit, "config\\AnnAdminList.cs", true);
			
		if(%admin == 2048)
			%god = "Server Operator added ";
		else 
			%god = Client::getName(%admin)@" ip: "@Ann::IPCut(Client::getTransportAddress(%admin))@" added ";
		
		$Admin = %god@$AnnAdmin::Name[%userNumber]@" IP: "@$AnnAdmin::Mask[%userNumber]@" to admin list";
		export("Admin","config\\AnnAdminList.cs",true);
		export("Admin","config\\admin.log",true);
		echo($admin);
		return true;					
	}
	else 
	{
		$admin = Client::getName(%admin)@" ip: "@Client::getTransportAddress(%client)@" trying to exploit remote add admin. ";		
		export("Admin","config\\Admin.log",true);
		echo($admin);
	}
}


function Ann::levels(%clientId, %cl)
{	
	%name = Client::getName(%cl);	
	%address = Client::getTransportAddress(%cl);
   	%user = Ann::AdminUser(%name,Ann::ipCut(%address));

   	if(%clientId.isOwner)
   	{
   		%level = "None";
   		if(%cl.isUntouchable)%level = "Safe";
   		if(%cl.isAdmin)%level = "Public";
   		if(%cl.isSuperAdmin)%level = "Super";
   		if(%cl.isGod)%level = "God";
   		if(%cl.isOwner)%level = "Owner";
   		
   			
   		Client::buildMenu(%clientId, "Admin Level "@%level, "atype", true);
    		Client::addMenuItem(%clientId, %curItem++ @ "Temporary Levels", "temp " @ %cl);
    		Client::addMenuItem(%clientId, %curItem++ @ "Automatic Levels", "perm " @ %cl);
	}
	else if(%clientId.isGoated)
	{
   		%level = "None";
   		if(%cl.isUntouchable)%level = "Safe";
   		if(%cl.isAdmin)%level = "Public";
   		if(%cl.isSuperAdmin)%level = "Super";
   		if(%cl.isGod)%level = "God";
   		if(%cl.isOwner)%level = "Owner";
   		
   			
   		Client::buildMenu(%clientId, "Admin Level "@%level, "atype", true);
    		Client::addMenuItem(%clientId, %curItem++ @ "Temporary Levels", "temp " @ %cl);
    		Client::addMenuItem(%clientId, %curItem++ @ "Automatic Levels", "perm " @ %cl);
	}
	return;
	
}

function processMenuAtype(%clientId,%opt)
{
	%type = getWord(%opt,0);
	%cl = getWord(%opt,1);

	%name = Client::getName(%cl);	
	%address = Client::getTransportAddress(%cl);
   	%user = Ann::AdminUser(%name,Ann::ipCut(%address));
	if(%clientId.isGoated || %clientId.isOwner)
	{
   		%level = "None";
   		if(%cl.isUntouchable)%level = "Safe";
   		if(%cl.isAdmin)%level = "Public";
   		if(%cl.isSuperAdmin)%level = "Super";
   		if(%cl.isGod)%level = "God";
   		if(%cl.isOwner)%level = "Owner";

		if(%type == "temp")
		{
			Client::buildMenu(%clientId, "Temp-Admin Levels", "Levels", true);

	   		if(%level != "None")
    			Client::addMenuItem(%clientId, %curItem++ @ "Strip all Levels.", "Strip " @ %cl);
    		if(%level != "Safe")
   				Client::addMenuItem(%clientId, %curItem++ @ "Change to Untouchable.", "Safe " @ %cl);
   			if(%level != "Public")
   				Client::addMenuItem(%clientId, %curItem++ @ "Change to Public.", "Public " @ %cl);
   			if(%level != "Super")	
   				Client::addMenuItem(%clientId, %curItem++ @ "Change to Super.", "Super " @ %cl);
			if(%level != "God")
				Client::addMenuItem(%clientId, %curItem++ @ "Change to God.", "God " @ %cl);
			if(%clientId.isGoated)
			{
			if(%level != "Owner")
				Client::addMenuItem(%clientId, %curItem++ @ "Change to Owner.", "Owner " @ %cl);
			}
		}
		else if(%type == "perm")
		{
			Client::buildMenu(%clientId, "Auto-Admin Levels", "Levels", true);

   			if(%user)
   			   Client::addMenuItem(%clientId, %curItem++ @ "Edit Auto admin.", "EditAutoAdminLevel " @ %cl);
			else
			{
				
				Client::addMenuItem(%clientId, %curItem++ @ "Add to Untouchable list ", "AutoSafe " @ %cl);
				Client::addMenuItem(%clientId, %curItem++ @ "Add to Admin list ", "AutoPublic " @ %cl);
				Client::addMenuItem(%clientId, %curItem++ @ "Add to Super Admin List ", "AutoSuper " @ %cl);
				Client::addMenuItem(%clientId, %curItem++ @ "Add to God Admin List ", "AutoGod " @ %cl);
			if(%clientId.isGoated)
			{
				Client::addMenuItem(%clientId, %curItem++ @ "Add to Owner Admin List ", "AutoOwner " @ %cl);
			}
			}
		}
	}
//   	else if((%clientId.isOwner && !%cl.isowner) || (%clientId.isGod && !%cl.isGod))
//   	{
//   		%level = "None";
//   		if(%cl.isUntouchable)%level = "Safe";
//   		if(%cl.isAdmin)%level = "Public";
//   		if(%cl.isSuperAdmin)%level = "Super";
//   		if(%cl.isGod)%level = "God";
//   		if(%cl.isOwner)%level = "Owner";
//
//		if(%type == "temp")
//		{
//			Client::buildMenu(%clientId, "Temp-Admin Levels", "Levels", true);
//
//	   		if(%level != "None")
  //  			Client::addMenuItem(%clientId, %curItem++ @ "Strip all Levels.", "Strip " @ %cl);
//    		if(%level != "Safe")
//   				Client::addMenuItem(%clientId, %curItem++ @ "Change to Untouchable.", "Safe " @ %cl);
//   			if(%level != "Public")
//   				Client::addMenuItem(%clientId, %curItem++ @ "Change to Public.", "Public " @ %cl);
//   			if(%level != "Super")	
//   				Client::addMenuItem(%clientId, %curItem++ @ "Change to Super.", "Super " @ %cl);
//			if(%clientId.isOwner && %level != "God")
//				Client::addMenuItem(%clientId, %curItem++ @ "Change to God.", "God " @ %cl);
//			if(%clientId.isOwner && %level != "Owner")
//				Client::addMenuItem(%clientId, %curItem++ @ "Change to Owner.", "Owner " @ %cl);
//		}
//		else if(%type == "perm")
//		{
//			Client::buildMenu(%clientId, "Auto-Admin Levels", "Levels", true);
//
  // 			if(%user)
   //			   Client::addMenuItem(%clientId, %curItem++ @ "Edit Auto admin.", "EditAutoAdminLevel " @ %cl);
	//		else
//			{
//				
//				Client::addMenuItem(%clientId, %curItem++ @ "Add to Untouchable list ", "AutoSafe " @ %cl);
//				Client::addMenuItem(%clientId, %curItem++ @ "Add to Admin list ", "AutoPublic " @ %cl);
//				Client::addMenuItem(%clientId, %curItem++ @ "Add to Super Admin List ", "AutoSuper " @ %cl);
//				if(%clientId.isOwner)
//					Client::addMenuItem(%clientId, %curItem++ @ "Add to God Admin List ", "AutoGod " @ %cl);
//				if(%clientId.isOwner)
//					Client::addMenuItem(%clientId, %curItem++ @ "Add to Owner Admin List ", "AutoOwner " @ %cl);
//			}
//		}
//	}
	return;
	
}


function Ann::GodAdminMenu(%client)
{
	//echo(GodAdminMenu);
	if(%client.isGoated || %client.isOwner)
	{
		Client::buildMenu(%client, "Server Options:", "AnnGodOptions", true);			
		Client::addMenuItem(%client, %curItem++ @ "Reset Server Defaults", "reset");
			Client::addMenuItem(%client, %curItem++ @ "Restart Server", "restart");
		Client::addMenuItem(%client, %curItem++ @ "View Admin List", "list");
		Client::addMenuItem(%client, %curItem++ @ "View Banned List", "bannedlist");
		return;
	}	
}


function processMenuAnnGodOptions(%clientId, %option)
{
	//echo("process god ",%clientId, %option);
	if(%clientId.isGoated || %clientId.isOwner)
	{   
	// reset server
	  	if(%option == "reset") 
	  	{
	  		Client::buildMenu(%clientId, "Reset Server?", "Server", true);	
	  		Client::addMenuItem(%clientId, %curItem++ @ "yes, Reset server", "resetYes");
	  		Client::addMenuItem(%clientId, %curItem++ @ "no, Don't reset", "resetNo");
	  		return;
	  	}
	  	else if(%option == "resetYes") 
	  	{
			if(!%clientId.SecretAdmin)
				messageAll(0, Client::getName(%clientId) @ " reset the server to default settings.~wCapturedTower.wav");
			else
				messageAll(0, "Reseting the server to default settings.~wCapturedTower.wav");
			Server::refreshData();
	  	}		
        else if(%option == "resetNo")
            return;
            
	// Just for you Redneck, restart server
	  	else if(%option == "restart")
	  	{
	  		Client::buildMenu(%clientId, "Restart Server?", "Restart", true);
	  		Client::addMenuItem(%clientId, %curItem++ @ "yes, Restart server", "restartYes");
	  		Client::addMenuItem(%clientId, %curItem++ @ "no, Don't restart", "restartNo");
	  		return;
	  	}

	// auto admin list viewer
	  	else if(%option == "list") 
	  	{   	
			processMenuAnnAdminViewer(%clientId,list);
			return;	
		}
	// banned list viewer
	  	else if(%option == "bannedlist") 
	  	{   	
			processMenuAnnBannedViewer(%clientId,bannedlist);
			return;	
		}
	}
}

function processMenuRestart(%clientId, %option)
{
  	if(%option == "restartYes")
  	{
		messageAll(0, Client::getName(%clientId) @ " is restarting the server!~wCapturedTower.wav");
		RestartServer();
  	}
    else if(%option == "restartNo")
        return;
}


function processMenuAnnAdminViewer(%clientId, %option)
{
	if(%clientId.isGoated || %clientId.isOwner)
	{	
		%action = getWord(%option, 0);
		%num = getWord(%option, 1);
		echo("AnnAdminViewer "@%option);
		if(%action == "list")
		{
			if(%num <4 ) 
				%num = 0;
				
			Client::buildMenu(%clientId, "Annihilation Auto Admin List", "AnnAdminViewer", true);
			if(%num < 2)	
				Client::addMenuItem(%clientId, %curItem++@%num++ @" \""@ $AnnAdmin::Name[%num]@"\" "@ getword($AnnAdmin::Mask[%num],0), "edit "@%num);
			else
				Client::addMenuItem(%clientId, %curItem++@"Back", "list "@%num -6);
			for(%i = 1; %i < 7; %i++)
			{
				
				if($AnnAdmin::Name[%num+1]!= "")	
				{
					%num++;
					Client::addMenuItem(%clientId, %curItem++@%num @" \""@ $AnnAdmin::Name[%num]@"\" "@ getword($AnnAdmin::Mask[%num],0), "edit "@%num);	
				}
				else
					%end = true;
			}
					
				
			if(!%end)
				Client::addMenuItem(%clientId, "8Next", "list "@%num);
				
			return;
		}
		else if(%action == "edit")
		{
			%level = getword($AnnAdmin::Mask[%num],0);
			Client::buildMenu(%clientId, "Edit "@ $AnnAdmin::Name[%num] @" "@getword($AnnAdmin::Mask[%num],0), "AnnAdminViewer", true);
			Client::addMenuItem(%clientId, %curItem++@"Back "@getword($AnnAdmin::Mask[%num],1), "list "@%num -7);
			//echo(%level);	
			if(%level != "None")
				Client::addMenuItem(%clientId, %curItem++@" Change to None", "None "@%num);
			if(%level != "Safe")	
				Client::addMenuItem(%clientId, %curItem++@" Change to Safe", "Safe "@%num);
			if(%level != "Public")	
				Client::addMenuItem(%clientId, %curItem++@" Change to Public", "Public "@%num);	
			if(%level != "Super")	
				Client::addMenuItem(%clientId, %curItem++@" Change to Super", "Super "@%num);	
			if(%level != "God")
				Client::addMenuItem(%clientId, %curItem++@" Change to God", "God "@%num);
			if(%clientId.isGoated)
			{
			if(%level != "Owner")
				Client::addMenuItem(%clientId, %curItem++@" Change to Owner", "Owner "@%num);
			}
			//Who played with this last
			Client::addMenuItem(%clientId, %curItem++@" Last edit by: "@ $AnnAdmin::LastEdit[%num], "edit "@%num); //put client back here
			//Who added this user 
			Client::addMenuItem(%clientId, %curItem++@" Added by: "@ $AnnAdmin::OriginalEdit[%num], "edit "@%num);
					
			return;
		}
		else if(%action == "None" || %action == "Safe" || %action == "Public" || %action == "Super" || %action == "God" ||  %action == "Owner")
		{	
			%level = getword($AnnAdmin::Mask[%num],0);
			Client::buildMenu(%clientId, $AnnAdmin::Name[%num] @" to "@%action, "AnnAdminViewer", true);
			Client::addMenuItem(%clientId, %curItem++@"Back "@getword($AnnAdmin::Mask[%num],1), "list "@%num -7);		
			Client::addMenuItem(%clientId, %curItem++@"Confirm Change to "@%action, "Confirm "@ %num @ " "@ %action);
			return;
		}
		else if(%action == "Confirm")
		{
			%change = getWord(%option, 2);
			%level = getword($AnnAdmin::Mask[%num],0);
			%message = $AnnAdmin::Name[%num]@" "@getword($AnnAdmin::Mask[%num],0)@" changed to "@%change@".";
			echo("!! "@Client::getName(%clientId)@" modified admin list. "@%message);
			client::sendmessage(%clientId,1,"User# "@%num@" \""@$AnnAdmin::Name[%num]@"\" "@getword($AnnAdmin::Mask[%num],0)@", changed to "@%change@".");
									
			%exportName = "AnnAdmin::Name"@%num; 
			export(%exportName,"config\\AnnAdminList.cs",true); 
			
			$AnnAdmin::Mask[%num] = %change@" "@getword($AnnAdmin::Mask[%num],1);
			%exportMask = "AnnAdmin::Mask"@%num; 
			export(%exportMask,"config\\AnnAdminList.cs",true);
			
			//%AdminName = Client::getName(%clientId); 
			//$AnnAdmin::LastEdit[%userNumber] = %AdminName;
			%exportLastEdit = "AnnAdmin::LastEdit"@%num; 
			export(%exportLastEdit, "config\\AnnAdminList.cs", true);
				
			%exportOriginalEdit = "AnnAdmin::OriginalEdit"@%num;
			export(%exportOriginalEdit, "config\\AnnAdminList.cs", true);
			
			//$EditAdmin = "!! "@Client::getName(%clientId)@" modified admin list. "@%message;
			//$Admin = Client::getName(%clientId)@" modified admin list. "@%message;
			//export("Admin","config\\AnnAdminList.cs",true);	
			
			processMenuAnnAdminViewer(%clientId,"list "@%num-1);
		}
	}
	else if(%clientId.isGoated || %clientId.isOwner) //switch it over to other admins here
	{	
		%action = getWord(%option, 0);
		%num = getWord(%option, 1);
		echo("AnnAdminViewer "@%option);
		if(%action == "list")
		{
			if(%num <4 ) 
				%num = 0;
				
			Client::buildMenu(%clientId, "Annihilation Auto Admin List", "AnnAdminViewer", true);
			if(%num < 2)	
				Client::addMenuItem(%clientId, %curItem++@%num++ @" \""@ $AnnAdmin::Name[%num]@"\" "@ getword($AnnAdmin::Mask[%num],0), "edit "@%num);
			else
				Client::addMenuItem(%clientId, %curItem++@"Back", "list "@%num -6);
			for(%i = 1; %i < 7; %i++)
			{
				
				if($AnnAdmin::Name[%num+1]!= "")	
				{
					%num++;
					Client::addMenuItem(%clientId, %curItem++@%num @" \""@ $AnnAdmin::Name[%num]@"\" "@ getword($AnnAdmin::Mask[%num],0), "edit "@%num);	
				}
				else
					%end = true;
			}
					
				
			if(!%end)
				Client::addMenuItem(%clientId, "8Next", "list "@%num);
				
			return;
		}
		else if(%action == "edit")
		{
			%level = getword($AnnAdmin::Mask[%num],0);
			Client::buildMenu(%clientId, "Edit "@ $AnnAdmin::Name[%num] @" "@getword($AnnAdmin::Mask[%num],0), "AnnAdminViewer", true);
			Client::addMenuItem(%clientId, %curItem++@"Back "@getword($AnnAdmin::Mask[%num],1), "list "@%num -7);
			if(%level != "Owner" && %level != -1 && (%level != "God" || (%level == "God" && %clientId.isOwner)))
			{	
				//echo(%level);	
				if(%level != "None")
					Client::addMenuItem(%clientId, %curItem++@" Change to None", "None "@%num);
				if(%level != "Safe")	
					Client::addMenuItem(%clientId, %curItem++@" Change to Safe", "Safe "@%num);
				if(%level != "Public")	
					Client::addMenuItem(%clientId, %curItem++@" Change to Public", "Public "@%num);	
				if(%level != "Super")	
					Client::addMenuItem(%clientId, %curItem++@" Change to Super", "Super "@%num);	
				if(%clientId.isOwner && %level != "God")
					Client::addMenuItem(%clientId, %curItem++@" Change to God", "God "@%num);
			if(%clientId.isGoated)
			{
				 if(%clientId.isOwner && %level != "Owner")
					Client::addMenuItem(%clientId, %curItem++@" Change to Owner", "Owner "@%num);
			}
				//Who played with this last
				Client::addMenuItem(%clientId, %curItem++@" Last edit by: "@ $AnnAdmin::LastEdit[%num], "edit "@%num); //put client back here
				//Who added this user 
				Client::addMenuItem(%clientId, %curItem++@" Added by: "@ $AnnAdmin::OriginalEdit[%num], "edit "@%num);
			}	
			return;
		}
		else if(%action == "None" || %action == "Safe" || %action == "Public" || %action == "Super" || %action == "God" ||  %action == "Owner")
		{	
			%level = getword($AnnAdmin::Mask[%num],0);
			Client::buildMenu(%clientId, $AnnAdmin::Name[%num] @" to "@%action, "AnnAdminViewer", true);
			Client::addMenuItem(%clientId, %curItem++@"Back "@getword($AnnAdmin::Mask[%num],1), "list "@%num -7);
			if((%level == "God" && %clientId.isOwner) || %level != "God" && %level != -1)
			{			
				Client::addMenuItem(%clientId, %curItem++@"Confirm Change to "@%action, "Confirm "@ %num @ " "@ %action);
			}
			else
				Client::addMenuItem(%clientId, %curItem++@"Cannot change (goduser)", "list "@%num -7);
			return;
		}
		else if(%action == "Confirm")
		{
			%change = getWord(%option, 2);
			%level = getword($AnnAdmin::Mask[%num],0);
			
			if(%level != "Owner" && %level != -1 && (%level != "God" || (%level == "God" && %clientId.isOwner)))
			{	
				%message = $AnnAdmin::Name[%num]@" "@getword($AnnAdmin::Mask[%num],0)@" changed to "@%change@".";
				echo("!! "@Client::getName(%clientId)@" modified admin list. "@%message);
				client::sendmessage(%clientId,1,"User# "@%num@" \""@$AnnAdmin::Name[%num]@"\" "@getword($AnnAdmin::Mask[%num],0)@", changed to "@%change@".");
										
				%exportName = "AnnAdmin::Name"@%num; 
				export(%exportName,"config\\AnnAdminList.cs",true); 
				
				$AnnAdmin::Mask[%num] = %change@" "@getword($AnnAdmin::Mask[%num],1);
				%exportMask = "AnnAdmin::Mask"@%num; 
				export(%exportMask,"config\\AnnAdminList.cs",true);	
				
				%AdminName = Client::getName(%clientId); 
				$AnnAdmin::LastEdit[%userNumber] = %AdminName;
				%exportLastEdit = "AnnAdmin::LastEdit"@%userNumber;
				export(%exportLastEdit, "config\\AnnAdminList.cs", true);
				
				%exportOriginalEdit = "AnnAdmin::OriginalEdit"@%userNumber;
				export(%exportOriginalEdit, "config\\AnnAdminList.cs", true);
						
				$EditAdmin = "!! "@Client::getName(%clientId)@" modified admin list. "@%message;
				$Admin = Client::getName(%clientId)@" modified admin list. "@%message;
				export("Admin","config\\AnnAdminList.cs",true);	
				
				processMenuAnnAdminViewer(%clientId,"list "@%num-1);
			}
		}
	}
}


function processMenuLevels(%clientId, %option)
{
	if(%clientId.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%clientId);
		
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);	
	%name = Client::getName(%cl);
	%AdminName = Client::getName(%clientId);	
	%ip = Client::getTransportAddress(%clientId);
	
	if(%clientId.isGoated || %clientId.isOwner)
	{		
		if(%opt == "EditAutoAdminLevel")
		{
	   		%address = Client::getTransportAddress(%cl);
	   		%user = Ann::AdminUser(%name,Ann::ipCut(%address));		
			processMenuAnnAdminViewer(%clientId, "edit "@%user);
			return;
		}				
		else if(%opt == "Strip" || %opt == "Safe" || %opt == "Public" || %opt == "Super" || %opt == "God" || %opt == "Owner")
		{
			Client::buildMenu(%clientId, %opt@" Admim "@%name, "Levels", true);
			Client::addMenuItem(%clientId, "1"@%opt@" Admin." , "Confirm"@%opt@ " " @ %cl);
			Client::addMenuItem(%clientId, "2Naye.", "no " @ %cl);
			return;
		}
		else if(%opt == "ConfirmStrip")
		{
			%cl.isUntouchable = false;
			%cl.isAdmin = false;
			%cl.isSuperAdmin = false;
			%cl.isGod = false;
			%cl.isOwner = false;
			Game::refreshClientScore(%cl);
			messageAll(0, %AdminName @ " stripped " @ %name @ "'s Admin.~wCapturedTower.wav");
			$Admin = %AdminName @ " "@%ip@", stripped " @ %name @ " "@Client::getTransportAddress(%cl)@" admin."; 
			export("Admin","config\\Admin.log",true);
			return;		
		}		
		else if(%opt == "ConfirmSafe")
		{
			%cl.isUntouchable = true;
			%cl.isAdmin = false;
			%cl.isSuperAdmin = false;
			%cl.isGod = false;
			Game::refreshClientScore(%cl);
			messageAll(0, %AdminName @ " made " @ %name @ " Untouchable.~wCapturedTower.wav");
			$Admin = %AdminName @ " "@%ip@", made " @ %name @ " "@Client::getTransportAddress(%cl)@" Untouchable."; 
			export("Admin","config\\Admin.log",true);
			return;		
		}		
		else if(%opt == "ConfirmPublic")
		{
			%cl.isUntouchable = true;
			%cl.isAdmin = true;
			%cl.isSuperAdmin = false;
			%cl.isGod = false;
			Game::refreshClientScore(%cl);
			messageAll(0, %AdminName @ " made " @ %name @ " into an Admin.~wCapturedTower.wav");
			$Admin = %AdminName @ " "@%ip@", made " @ %name @ " "@Client::getTransportAddress(%cl)@" into an admin."; 
			export("Admin","config\\Admin.log",true);
			return;		
		}
		else if(%opt == "ConfirmSuper")
		{
			%cl.isUntouchable = true;
			%cl.isAdmin = true;
			%cl.isSuperAdmin = true;
			%cl.isGod = false;
			Game::refreshClientScore(%cl);
			messageAll(0, %AdminName @ " made " @ %name @ " into a Super Admin.~wCapturedTower.wav");
			$Admin = %AdminName @ " "@%ip@", made " @ %name @ " "@Client::getTransportAddress(%cl)@" into an admin."; 
			export("Admin","config\\Admin.log",true);
			return;		
		}				
		else if(%opt == "ConfirmGod")
		{
			%cl.isUntouchable = true;
			%cl.isAdmin = true;
			%cl.isSuperAdmin = true;
			%cl.isGod = true;
			Game::refreshClientScore(%cl);
			messageAll(0, %AdminName @ " made " @ %name @ " into a God Admin.~wCapturedTower.wav");
			$Admin = %AdminName @ " "@%ip@", made " @ %name @ " "@Client::getTransportAddress(%cl)@" into an admin."; 
			export("Admin","config\\Admin.log",true);
			return;		
		}				
		else if(%opt == "ConfirmOwner")
		{
			%cl.isUntouchable = true;
			%cl.isAdmin = true;
			%cl.isSuperAdmin = true;
			%cl.isGod = true;
			%cl.isOwner = true;
			Game::refreshClientScore(%cl);
			messageAll(0, %AdminName @ " made " @ %name @ " into an Owner Admin.~wCapturedTower.wav");
			$Admin = %AdminName @ " "@%ip@", made " @ %name @ " "@Client::getTransportAddress(%cl)@" into an admin."; 
			export("Admin","config\\Admin.log",true);
			return;		
		}	
		
	//end regular admin parser, start parsing auto admin	
		else if(%opt == "AutoSafe" || %opt == "AutoPublic" || %opt == "AutoSuper" || %opt == "AutoGod" || %opt == "AutoOwner")
		{
			Client::buildMenu(%clientId, "Confirm "@%opt, "Levels", true);
			Client::addMenuItem(%clientId, "1Add To "@%opt@" List ", "Confirm"@%opt@" " @ %cl);
			Client::addMenuItem(%clientId, "2Noper. ", "no " @ %cl);		
			return;
		}
		else if(%opt == "ConfirmAutoSafe")
		{
			%cl.isUntouchable = true;
			%cl.isAdmin = false;
			%cl.isSuperAdmin = false;
			%cl.isGod = false;
			Game::refreshClientScore(%cl);
			Ann::AddAdmin(%clientId,%cl,"Safe");	
			%message = Client::getName(%clientId)@ " added you to the Untouchable list.";
			Client::sendMessage(%cl, 0, %message@"~wCapturedTower.wav");
			Client::sendMessage(%clientId, 0, %name@ " added to the Untouchable list.~wCapturedTower.wav");
			bottomprint(%cl, "<f1><jc>"@%message, 20);
			return;		
		}		
		else if(%opt == "ConfirmAutoPublic")
		{
			%cl.isUntouchable = true;
			%cl.isAdmin = true;
			%cl.isSuperAdmin = false;
			%cl.isGod = false;
			Game::refreshClientScore(%cl);
			Ann::AddAdmin(%clientId,%cl,"Public");	
			%message = Client::getName(%clientId)@ " added you to the AutoPublic Admin list.";
			Client::sendMessage(%cl, 0, %message@"~wCapturedTower.wav");
			Client::sendMessage(%clientId, 0, %name@ " added to the AutoPublic Admin list.~wCapturedTower.wav");
			bottomprint(%cl, "<f1><jc>"@%message, 20);
			return;		
		}
		else if(%opt == "ConfirmAutoSuper")
		{
			%cl.isUntouchable = true;
			%cl.isAdmin = true;
			%cl.isSuperAdmin = true;
			%cl.isGod = false;
			Game::refreshClientScore(%cl);
			Ann::AddAdmin(%clientId,%cl,"Super");	
			%message = Client::getName(%clientId)@ " added you to the AutoSuper Admin list.";
			Client::sendMessage(%cl, 0, %message@"~wCapturedTower.wav");
			Client::sendMessage(%clientId, 0, %name@ " added to the AutoSuper Admin list.~wCapturedTower.wav");
			bottomprint(%cl, "<f1><jc>"@%message, 20);
			return;		
		}
		else if(%opt == "ConfirmAutoGod")
		{
			%cl.isUntouchable = true;
			%cl.isAdmin = true;
			%cl.isSuperAdmin = true;
			%cl.isGod = true;
			Game::refreshClientScore(%cl);
			Ann::AddAdmin(%clientId,%cl,"God");	
			%message = "You have been added to the Auto God Admin list.";
			Client::sendMessage(%cl, 0, %message@"~wCapturedTower.wav");
			bottomprint(%cl, "<f1><jc>"@%message, 20);
			return;		
		}
		else if(%opt == "ConfirmAutoOwner") //owner 
		{
			%cl.isUntouchable = true;
			%cl.isAdmin = true;
			%cl.isSuperAdmin = true;
			%cl.isGod = true;
			%cl.isOwner = true;
			Game::refreshClientScore(%cl);
			Ann::AddAdmin(%clientId,%cl,"Owner");	
			%message = "You have been added to the Auto Owner Admin list.";
			Client::sendMessage(%cl, 0, %message@"~wCapturedTower.wav");
			bottomprint(%cl, "<f1><jc>"@%message, 20);
			return;		
		}		
	}		
}

