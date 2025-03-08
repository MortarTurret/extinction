
function Ann::PlayerInfo::Connect(%client)
{
	DebugFun("Ann::PlayerInfo::Connect",%client);
//	Anni::Echo("player Info?!! ZOMG!@@!##1`q!");

	%transport = Client::getTransportAddress(%client);
	%ip = Ann::ipCut(%transport);

	if(%ip!="")
	{
		if(!%client.PPIsRegistered) //fix for registered users
			schedule("Ann::PlayerInfo::Saves("@%client@", "@%transport@");",18000,%client);	//5 mins between saves.
		
		%address = Ann::Replace(%ip, ".", ":");	//"IP"@Ann::Replace(%ip, ".", ":");
		%clname = client::getName(%client);
		
		%file = "Z" @ %ip @ ".cs";	// no ':'s in windows file names. 
	

		
		if(isFile("Temp\\"@%file)) 
		{			
		//	Anni::Echo(%file@" found.");
			// push into sys memory
			exec(%file);	
			
			//now we parse this crap up
			%more = %address@":name";
			%nameString = $annInfo::[%more];	//$annInfo::[%address];	
			
			if(Ann::StrLen(%nameString)>950)
				%namestring = string::getsubstr(%namestring,0,900);
				
				
		//	Anni::Echo("found an existing name");		
			if(%nameString == %clname)	//!string::ICompare(%nameString,%clname) == 0)
			{
				%adminmessage = %clname@" Has connected before, only one name saved for this IP.";
				%names = %namestring;	
			}
			else
			{
				if(string::findSubStr(%nameString, "~") == 0)
				{
					// corrupt file... Remove ~ at beginning of string.
					// This happens when a file is cleared accidently. 
					// Shouldn't happen anymore.
					%namestring = string::getsubstr(%namestring,1,900);
					$annInfo::[%more] = %nameString;
					Ann::PlayerInfo::VarSet(%file, %address, "name", %nameString);
				}
				else if(String::findSubStr(%nameString, "~") > 0)
				{		
					//check other names here.. code gnomes, do your work..
					%names = Ann::Replace(%namestring, "~", ", ");
					if(string::findSubStr(%nameString, %clname) != -1)
					{
				//		Anni::Echo("found clients name, not changing");
						%client.names = %names;
						%adminmessage = %clname@"'s IP Has connected before as: "@%names;	
					}
					else
					{
				//		Anni::Echo("adding new name to list");
						%nameString = %nameString@"~"@%clname;
						%client.names = %names;
						%adminmessage = %clname@"'s IP Has connected before as: "@%names;
						Ann::PlayerInfo::VarSet(%file, %address, "name", %nameString);
					}				
				}
				else
				{
			//		Anni::Echo("adding new name to list");
					if(%nameString != "")
						%nameString = %nameString@"~"@%clname;
					else
						%nameString = %clname;
					%client.names = Ann::Replace(%nameString, "~", ", ");
					%adminmessage = %clname@"'s IP Has connected before as: "@Ann::Replace(%namestring, "~", ", ");						
					Ann::PlayerInfo::VarSet(%file, %address, "name", %nameString);
				}			
			}
			
			if(!%client.PPIsRegistered) //fix for registered users
			{
				%more = %address@":Tkills";
				%client.TKills = $annInfo::[%more];
			
				%more = %address@":TDeaths";
				%client.TDeaths = $annInfo::[%more];

				%more = %address@":Prefs";
				%temp = $annInfo::[%more];
				%client.MiniMode = NumCheck(getWord(%temp, 0));
				%client.Vulcan = NumCheck(getWord(%temp, 1));
				%client.pbeam = NumCheck(getWord(%temp, 2));
				%client.suicideTimer = NumCheck(getWord(%temp, 3));
				%client.weaponHelp = NumToBool(getWord(%temp, 4));
				%client.BlockMySound = NumToBool(getWord(%temp, 5));
				if ( %client.isSuperAdmin )
					%client.SecretAdmin = NumToBool(getWord(%temp, 6));
				%client.silenced = NumToBool(getWord(%temp, 7));
				%client.leet = NumToBool(getWord(%temp, 8));
				%client.noVpack = NumToBool(getWord(%temp, 9));
				%client.novote = NumToBool(getWord(%temp, 10));
				%client.locked = NumToBool(getWord(%temp, 11));
				%client.noPfork = NumToBool(getWord(%temp, 12));
				%client.TitanRotation = NumToBool(getWord(%temp, 13)); //added new prefs
				%client.DuelToggle = NumToBool(getWord(%temp, 14));
			}
			else if(%client.PPIsRegistered) //fix for registered users
			{
				//haha do nothing
			}
		}
		else
		{
			// first connect. 
		//	Anni::Echo("Creating "@%file@", dumping to Temp\\");
			Ann::PlayerInfo::VarSet(%file, %address, "name", %clname);
			%adminmessage = %clname@"'s IP hasn't connected before.";	
			
			//Export name
			%export = "annInfo::"@%address@":*";			
			export(%export, "Temp\\"@ %file, false);	//true = append			
				
					
		}
	}
	%adminmessage = %ip@" "@%adminmessage;
	Anni::Echo(%adminmessage);
	admin::BPmessage(%adminmessage);
	//%client.names = %names; //cant use this 
	$Client::names[%client] = %names;	//we can't use %client. because the memory isn't allocated yet.. 
	//if(!%client.PPIsRegistered) 
		Ann::PlayerInfo::Export(%client);	
}

function NumCheck(%var)
{
	if ( !%var || %var == "" || %var < 0 )
		return 0;
	return %var;
	
}

function BoolToNum(%var)
{
	if ( %var )
		return 1;
	return 0;
}

function NumToBool(%var)
{
	if ( %var == 1 )
		return "True";
	return "False";
}

function Ann::PlayerInfo::Export(%client)
{
	DebugFun("Ann::PlayerInfo::Export",%client);
	%transport = Client::getTransportAddress(%client);
	%ip = Ann::ipCut(%transport);
	
	if(%client.PPIsRegistered)
		return;

	if(%ip!="")
	{
		%address = Ann::Replace(%ip, ".", ":");	
		
	//	Anni::Echo("Store player Info?!! ZOMG!@@!##1`q!");
		%ip = Ann::ipCut(%transport);

		%address = Ann::Replace(%ip, ".", ":");	//"IP"@Ann::Replace(%ip, ".", ":");
		%clname = client::getName(%client);
			
		%file = "Z" @ %ip @ ".cs";
				
		if(isFile("Temp\\"@%file)) 
		{
			//set our saved info vars.
			Ann::PlayerInfo::VarSet(%file, %address, "TKills", %client.TKills);
			Ann::PlayerInfo::VarSet(%file, %address, "TDeaths", %client.TDeaths);
			Ann::PlayerInfo::VarSet(%file, %address, "Prefs", %client.MiniMode@" "@%client.Vulcan@" "@%client.pbeam@" "@%client.suicideTimer@" "@BoolToNum(%client.weaponHelp)@" "@BoolToNum(%client.BlockMySound)@" "@BoolToNum(%client.SecretAdmin)@" "@BoolToNum(%client.silenced)@" "@BoolToNum(%client.leet)@" "@BoolToNum(%client.noVpack)@" "@BoolToNum(%client.novote)@" "@BoolToNum(%client.locked)@" "@BoolToNum(%client.noPfork)@" "@BoolToNum(%client.TitanRotation)@" "@BoolToNum(%client.DuelToggle));
			
			//Export them all.
			%export = "annInfo::"@%address@":*";			
			export(%export, "Temp\\"@ %file, false);	//true = append
											
		}
	}
}

function Ann::PlayerInfo::VarSet(%file, %address, %variableName, %value)
{
	DebugFun("Ann::PlayerInfo::VarSet",%flie,%address,%variableName,%value);
	%more = %address@":"@%variableName;
	$annInfo::[%more] = %value;
	%export = "$annInfo::"@%more;
//	export(%export,"Temp\\"@ %file, true);	
//	Anni::Echo("Setting " @ %export @ " = " @ %value ); 	
}

function Ann::PlayerInfo::Disconnect(%client)
{
	DebugFun("Ann::PlayerInfo::Disconnect",%client);
	Ann::PlayerInfo::Export(%client);
	
	%transport = Client::getTransportAddress(%client);
	%ip = Ann::ipCut(%transport);

	if(%ip!="")
	{
		%address = Ann::Replace(%ip, ".", ":");	
		// They left, delete crap. 
		%DUMP = "annInfo::" @ %address @ "*";
		deleteVariables(%dump);			
		
	}	
}

function Ann::PlayerInfo::Saves(%client,%trans)
{
	DebugFun("Ann::PlayerInfo::Saves",%client,%trans);
	// Anni::Echo("Save player info");
	%transport = Client::getTransportAddress(%client);
	if(%trans == %transport)
	{
		Ann::PlayerInfo::Export(%client);
		schedule("Ann::PlayerInfo::Saves("@%client@", "@%transport@");",18000,%client);	//5 mins between saves.
	}	
	
}
