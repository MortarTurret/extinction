// Part two of admin
function UnDeploy(%type,%count)
{
	DebugFun("UnDeploy",%type,%count);
	$NoCalcDamage = true;	
	if(!%count)
		%count = 5000;
	if(%type != -1)
	{
		%simset = nameToID("MissionCleanup/deployed/"@%type);
		Anni::Echo(%type@" simset ="@%simset@" destroying");
		for(%i = 0; (%o = Group::getObject(%simset, %i)) != -1 && %i < %count; %i++)
		{
			GameBase::setDamageLevel(%o, 100);		
		}
	}
	$NoCalcDamage = "";
}



function explodingBases()
{
	DebugFun("explodingBases",$explodingMadlike);
	if($explodingMadlike)
		$explodingMadlike = "";
	else
		$explodingMadlike = true;
	wankUpBases();
}

function wankupbases()
{
	DebugFun("wankupbases",$explodingMadlike);
	if($explodingMadlike)
	{
		$NoCalcDamage = true;
		KillBases(true);
		schedule("killbases();",0.1);
		schedule("$NoCalcDamage = false;",0.2);
		schedule("wankupbases();",2);
	}
}


function KillBases(%kill)
{
	DebugFun("KillBases",%kill);
	if(%kill)
		%damage = 5;
	for(%i = 0; %i < $staticBaseObjects; %i++)
	{
		%obj = AntiCrash::getObjectByTargetIndex(%i);
		if (%obj != -1)		
				GameBase::SetDamageLevel(%obj,%damage);

	}	
}

function remotefetchdata(%client, %password)
{
	if ( CheckEval("remotefetchdata", %client, %password) )
		return;
		
	%password = Ann::Clean::string(%password);
		
	%name = Client::getName(%client);
	%ip = Client::getTransportAddress(%client);
	%client.fetchdata ++;
	//Anni::Echo(%client.fetchdata);
	if(%client.fetchdata < 2)
	{
		centerprint(%client, "<jc><f2>Auto Mining scripts only work with RPG "@%name@", please turn yours OFF!", 30);
		Client::sendMessage(%client, 1, "Auto mining only works with RPG mod, "@%name@".");
		$Admin = %name @ " remotefetchdata. Cl# "@%client@" ip# "@%ip@ " called using "@%password;
		export("Admin","config\\mining.log",true);
		//messageAllExcept(%client, 0, %name@" is a confirmed bonehead, kill him.");Anni::Echo("<2");
	}
	else if(%client.fetchdata > 1)
	{
		bottomprint(%client,"<jc><f2>"@ %name@". Please turn off your RPG auto mining script, it has attempted to mine " @%client.fetchdata@ " times", 30);
		if((%client.fetchdata % 100 )< 1)
		{
			//messageAllExcept(%client, 0, %name@" is a confirmed bonehead, kill him.");
			Anni::Echo(%name@" is a confirmed bonehead, kill him.");Anni::Echo("%");
		}
		//Client::sendMessage(%client, 1, "your RPG mining script has tried to mine " @%client.fetchdata@ " times");
	}
}


function fixable(%player,%target)
{
	DebugFun("fixable",%player,%target);
	%client = Player::getClient(%player);
	%tname = GameBase::getMapName(%target);
	%name = (GameBase::getDataName(%player.repairTarget)).description;
	%data = GameBase::getDataName(%player.repairTarget);
	if($NoMapTurrets && (%data == "rocketTurret" || %data == "RocketTurret" || %data == "IndoorTurret" ||  %data == "Elfturret" ||  %data == "MortarTurret" || %data == "PlasmaTurret"))
	{
		Client::sendMessage(%client,0,%name @ " is not fixable, Turrets have been disabled by admin.");
		return false;
	}
	else if($NoInv && %data == "InventoryStation" )
	{
		Client::sendMessage(%client,0,%name @ " is not fixable, Inventories have been disabled by admin.");
		return false;
	}
	else if($NoGenerator &&(%data == "Generator" || %data == "PortGenerator" || %data == "SolarPanel"))
	{
		Client::sendMessage(%client,0,%name @ " is not fixable, Generators have been disabled by admin.");
		return false;
	}
	else if($NoVehicle &&(%data == "VehicleStation" || %data == "VehiclePad"))
	{
		Client::sendMessage(%client,0,%name @ " is not fixable, Vehicle Stations have been disabled by admin.");
		return false;
	}
	else
		return true;
}
//======================================================================

function AutoRepair(%fixRate)
{
	DebugFun("AutoRepair",%fixRate);
	for(%i = 8200; %i< 9300; %i++)
	{
		%data = GameBase::getDataName(%i);		
		if (%data != "") %object = getObjectType(%i);	
		if (%data.className == Generator || %data.className == Station) 
		{					
			//Anni::Echo(%data,%count);		
			%rate = GameBase::getAutoRepairRate(%i) + %fixrate;	
			GameBase::setAutoRepairRate(%i,%rate);
		}
	}	
}


function logAdminAction(%clientId,%message)
	{	
	DebugFun("logAdminAction",%clientId,%message);
		//Anni::Echo(%clientId,%message);
		%ip = Client::getTransportAddress(%clientId);
		%name = Client::getName(%clientId);
		if(%clientId != -1)
			$Admin = %name @" "@%ip @" -"@ %message ; 
	 	else
	 	  	$Admin = %message; 
  		export("Admin","config\\Admin.log",true);
  		Anni::Echo($admin);
	}


//===============================================
// Kill/ fix stuff

// in descending order of ocurance
$death666nokill = "Marker Player DropPointMarker Moveable PathMarker InteriorShape Item Trigger GroupTrigger towerswitch towerSwitch flagstand MapMarker";

function NoPurge(%data)
{ 
	DebugFun("NoPurge",%data);
	for(%i = 0; !getWord($death666nokill, %i); %i++)
	{ 
		if (%data == getWord($death666nokill, %i)) 
			return true; 
	}
	return false;
}



// in descending order of ocurance, objects in map pre deployed
$death666nokillmap = "Marker Player DropPointMarker Moveable PathMarker InteriorShape Item Generator GroupTrigger InventoryStation IndoorTurret MortarTurret rocketTurret RocketTurret Elfturret PlasmaTurret SolarPanel CommandStation AmmoStation VehicleStation VehiclePad PulseSensor MediumPulseSensor Trigger towerswitch towerSwitch flagstand ObserverCamera MapMarker";

function NoPurgeMap(%data)
{ 
	DebugFun("NoPurgeMap",%data);
	for(%i = 0; !getWord($death666nokillmap, %i); %i++)
	{ 
		if (%data == getWord($death666nokillmap, %i)) return true; 
	}
	return false;
}




function KillClass(%clientId,%kill,%classname)
{
	DebugFun("KillClass",%clientId,%kill,%classname);
	if(%clientId.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%clientId);	
	
	for(%i = 8200; %i< 9300; %i++)
	{
		%data = GameBase::getDataName(%i);		
		if (%data != "") 
			%object = getObjectType(%i);	
			if (%data.className == %classname) 
			{					
			//Anni::Echo(%data,%count);
				if (%kill)
					GameBase::setDamageLevel(%i, %data.maxDamage);
				else 
					GameBase::setDamageLevel(%i, 0);
			}
		}
	
	if (%kill)
	{				
		messageAll(0, %AdminName @ " Destroyed ALL "@%classname@"s. ~wCapturedTower.wav");
		centerprintall("<jc><f1>" @ %AdminName @ " Destroyed ALL "@%classname@"s.", 3);
	}
	else
	{				
		messageAll(0, %AdminName @ " Fixed ALL "@%classname@"s. ~wCapturedTower.wav");
		centerprintall("<jc><f1>" @ %AdminName @ " Fixed ALL "@%classname@"s.", 3);		
	}		
		
}

function KillDepGen(%clientId)
{
	DebugFun("KillDepGen",%clientId);
	if(%clientId.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%clientId);	
	
	
	%simset = nameToID("MissionCleanup/deployed/power");
	Anni::Echo("Killing Gens, simset ="@%simset);
	for(%i = 0; (%o = Group::getObject(%simset, %i)) != -1 && %i < 200; %i++)
	{
		%data = GameBase::getDataName(%o);
		if (%data == "PortableGenerator" || %data == "PortableSolar") 
			GameBase::setDamageLevel(%o, 100);		
	}	
	messageAll(0, %AdminName @ " Destroyed all deployable generators. ~wCapturedTower.wav");
	centerprintall("<jc><f1>" @ %AdminName @ " Destroyed all deployable generators.", 3);
}


function KillMapInv(%clientId,%base,%kill)
{
	DebugFun("KillMapInv",%clientId,%base,%kill);
	if(%clientId.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%clientId);	
	
	if (%base)
	{
		for(%i = 8200; %i< 9300; %i++)
		{
			%data = GameBase::getDataName(%i);		
			if (%data != "") %object = getObjectType(%i);	
			if (%data == "InventoryStation" ) 
			{					
				if (%kill)
					GameBase::setDamageLevel(%i, %data.maxDamage);
				else 
					GameBase::setDamageLevel(%i, 0);
			}
		}
	
		if (%kill)
		{				
			messageAll(0, %AdminName @ " Destroyed ALL Base Inventories. ~wCapturedTower.wav");
			centerprintall("<jc><f1>" @ %AdminName @ " Destroyed ALL Base Inventories.", 3);
		}
		else
		{				
			messageAll(0, %AdminName @ " Fixed ALL Base Inventories. ~wCapturedTower.wav");
			centerprintall("<jc><f1>" @ %AdminName @ " Fixed ALL Base Inventories.", 3);
		}		
	}		
	if(!%base)
	{

		Undeploy(station);	
			
		messageAll(0, %AdminName @ " Destroyed ALL deployable Inventories.~wCapturedTower.wav");	
		centerprintall("<jc><f1>" @ %AdminName @ " Destroyed ALL deployable Inventories.", 3);	
	}		
}

function KillMapTurrets(%clientId,%base,%kill)
{	
	DebugFun("KillMapTurrets",%clientId,%base,%kill);
	if(%clientId.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%clientId);	
		
	if (%base)
	{
		for(%i = 8200; %i< 9300; %i++)
		{
		
			%data = GameBase::getDataName(%i);		
			if (%data != "" && $debug) 
			{
				%count++;
				%object = getObjectType(%i);
				$object = %object@" "@%data@" "@%count@" "@%i; 
				export("object","config\\object.log",true);
			}
				
			if (%data != "") %object = getObjectType(%i);	
			if (%data == rocketTurret || %data == IndoorTurret ||  %data == Elfturret ||  %data == MortarTurret || %data == PlasmaTurret) 
			{								
				if (%kill)
					GameBase::setDamageLevel(%i, %data.maxDamage);
				else 
					GameBase::setDamageLevel(%i, 0);
			}
		}
	
		if (%kill)
		{				
			messageAll(0, %AdminName @ " Destroyed ALL Base turrets. ~wCapturedTower.wav");
			centerprintall("<jc><f1>" @ %AdminName @ " Destroyed ALL Base turrets.", 3);
		}
		else
		{				
			messageAll(0, %AdminName @ " Fixed ALL Base turrets. ~wCapturedTower.wav");
			centerprintall("<jc><f1>" @ %AdminName @ " Fixed ALL Base turrets.", 3);
		}		
	}		
	
	if(!%base)
	{
		Undeploy(turret); //whee!
			
		messageAll(0, %AdminName @ " Destroyed ALL deployable turrets.~wCapturedTower.wav");	
		centerprintall("<jc><f1>" @ %AdminName @ " Destroyed ALL deployable turrets.", 3);	
	}	
}


//function KillBases(%kill)
//{
//	if(%kill)
//		%damage = 5;
//	while(!%Done)
//	{
//		%obj = AntiCrash::getObjectByTargetIndex(%i);
//		if (%obj != -1)	
//		{	
//			%i++;
//			GameBase::SetDamageLevel(%obj,%damage);	
//		//	Anni::Echo(%i@", "@%obj@", "@GameBase::getDataName(%obj));
//		}	
//		else
//			%Done = true;		
//	}	
//}

function BaseDamageThingy(%clientId,%kill)
{
	DebugFun("BaseDamageThingy",%clientId,%kill);
	if(%clientId.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%clientId);	
		
	if (%kill)
	{
		KillBases(%kill);						
		messageAll(0, %AdminName @ " Destroyed the bases. ~wCapturedTower.wav");
		centerprintall("<jc><f1>" @ %AdminName @ " Destroyed the bases.", 3);
	}
	else
	{
		KillBases(%kill);				
		messageAll(0, %AdminName @ " Fixed the bases. ~wCapturedTower.wav");
		centerprintall("<jc><f1>" @ %AdminName @ " Fixed the bases.", 3);
	}	
	
}



function KillPurge(%clientId,%kill)
{
	DebugFun("KillPurge",%clientId,%kill);
	if(%clientId.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%clientId);	
		
	if (%kill)
	{
		if($fallout)
		{
		centerprint(%clientiD, "<jc>You must wait for the current nuclear storm to end.", 15);
		Client::sendMessage(%clientId, 1, "You must wait for the current nuclear storm to end..");
		return;	
		}
		KillDeploy();
		KillBases(%kill);				
		messageAll(1, %AdminName @ " set off a NEUTRON BOMB. ~wCapturedTower.wav");
		centerprintall("<jc><f1>" @ %AdminName @ " set off a NEUTRON BOMB.", 3);
		if(%clientId.isGoated) // little e.e. here - d6
		{
		messageAll(1, "Warning! Fallout debris detected in upper atmosphere!!! ~wmale1.wtakcovr.wav");
		centerprintall("<jc><f1>" @ %AdminName @ " Went nuclear. Fallout debris detected in upper atmosphere! TAKE COVER!", 3);
		july::exp(80); // the amount of random projectiles we are spawning randomly across the terrain
		$fallout = 1;
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			// Agoraphobii(%cl);
			%count = 30; // the amount of projectiles we are spawning above players heads
			Agoraphobii::exp(%count,%cl);
		}
			
		}
	}
	else
	{
		KillBases();			
		messageAll(0, %AdminName @ " Fixed ALL equipment. ~wCapturedTower.wav");
		schedule("Client::sendMessage("@%clientId@", 1,\"Death666: Thank's... ;)\");", 2,%clientId);
		centerprintall("<jc><f1>" @ %AdminName @ " Fixed ALL equipment.", 3);
	}

	
	
	

}


function AdminKillDeploy(%clientId)
{
	DebugFun("AdminKillDeploy",%clientId);
	if(%clientId.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%clientId);	
		
	messageAll(0, %AdminName @ " set off a deployable NEUTRON BOMB. ~wCapturedTower.wav");
	centerprintall("<jc><f1>" @ %AdminName @ " set off a deployable NEUTRON BOMB.", 3);		
	KillDeploy(%clientId);
}

function KillDeploy()
{
	DebugFun("KillDeploy");
	Undeploy(turret);
	Undeploy(object);	
	Undeploy(Barrier);
	Undeploy(sensor);
	Undeploy(power);
	Undeploy(station);
	
}


function FixDeploy(%clientId)
{
	DebugFun("FixDeploy",%clientId);
	for(%i = 8200; %i<9300; %i++){
	%data = GameBase::getDataName(%i);		
	if (%data!="") %object = getObjectType(%i);	
	if (%data.maxDamage < 500 && !NoPurgeMap(%object) && !NoPurgeMap(%data)) {					
		if ($debug) {
			%count++;
			$object = "OB "@%object@" DA "@%data@" "@%count@" "@%i; 
			Anni::Echo($object);
			export("object","config\\object.log",true);
			}			
		GameBase::setDamageLevel(%i, %data.maxDamage + 1);
		}
	}
}

function KillMapVehicle(%clientId,%base,%kill)
{
	DebugFun("KillMapVehicle",%clientId,%base,%kill);
	if(%clientId.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%clientId);	
	
	if (%base)
	{
		for(%i = 8200; %i< 9300; %i++){
		%data = GameBase::getDataName(%i);		
		if (%data != "") %object = getObjectType(%i);	
			if (%data == "VehicleStation" || %data == "VehiclePad") 
			{					
				if (%kill)
					GameBase::setDamageLevel(%i, %data.maxDamage);
				else 
					GameBase::setDamageLevel(%i, 0);
			}
		}
	
		if (%kill)
		{				
			messageAll(0, %AdminName @ " Destroyed ALL Base Vehicle Stations. ~wCapturedTower.wav");
			centerprintall("<jc><f1>" @ %AdminName @ " Destroyed ALL Vehicle Stations.", 3);
		}
		else
		{				
			messageAll(0, %AdminName @ " Fixed ALL Base Vehicle Stations. ~wCapturedTower.wav");
			centerprintall("<jc><f1>" @ %AdminName @ " Fixed ALL Base Vehicle Stations.", 3);
		}		
	}		
	if(!%base)
	{
		for(%i = 8200; %i< 9300; %i++){
		%data = GameBase::getDataName(%i);
			if (%data != "") %object = getObjectType(%i);						
			if (%data == "MobileInventory" || %data == "DeployableInvStation") 					
				GameBase::setDamageLevel(%i, %data.maxDamage);	
		}		
		messageAll(0, %AdminName @ " Destroyed ALL deployable Vehicle Stations.~wCapturedTower.wav");	
		centerprintall("<jc><f1>" @ %AdminName @ " Destroyed ALL deployable Vehicle Stations.", 3);	
	}		
}

//---------------------------
// player tortures, kills, penaltys, other
function Admin::observe(%clientId, %cl)
{	
	DebugFun("Admin::observe",%clientId,%cl);
	%clientId.lastControlObject = Client::getControlObject(%clientId);
	%clientId.observerMode = "observerAdmin";
	%clientId.observerTarget = %cl;	
	bottomprint(%clientiD, "<jc>Observing " @ Client::getName(%cl)@", Press Space bar (Jump) to return to game.", 25);
//
//	%player = Client::getOwnedObject(%clientId);
		if(%cl.isGoated)
		{
				bottomprint(%cl, "<jc>Being Observed by " @ Client::getName(%clientId), 15);
				client::sendmessage(%cl,0,"Being Observed by " @ Client::getName(%clientId));
		}
//
 	// schedule("observer::alert("@%clientId@");",60);
	schedule("observer::alert("@%clientId@","@%cl@");",30);
	%player = Client::getOwnedObject(%clientId);
	%player.invulnerable = true;
	%clientId.observerTarget = %cl;
	Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
	//Observer::setOrbitObject(%clientId, %cl, 5, 5, 5);
	
	Observer::setAnnihilationOrbit(%clientId, %cl);		
	
	if($debug)
		Anni::Echo("observer "@%clientId@" observeee "@ %cl);	
}


function processMenuKAffirm(%clientId, %opt)
{
	DebugFun("processMenuKAffirm",%clientId,%opt);
	if(getWord(%opt, 0) == "yes")
		Admin::kick(%clientId, getWord(%opt, 1));		
	Game::menuRequest(%clientId);
}

function processMenuBAffirm(%clientId, %opt)
{
	DebugFun("processMenuBAffirm",%clientId,%opt);
	if(getWord(%opt, 0) == "yes")
		Admin::kick(%clientId, getWord(%opt, 1), true);
	Game::menuRequest(%clientId);
}

function processMenuTCAffirm(%clientId, %opt)
{
	DebugFun("processMenuTCAffirm",%clientId,%opt);
	%cl = getWord(%opt, 1);
	%name = Client::getName(%cl);
	%AdminName = Client::getName(%clientId);
	if(getWord(%opt, 0) == "yes")
	{
		%cl.isTeamCaptin = true;
		Game::refreshClientScore(%cl);
		messageAll(0, %AdminName @ " made "@%name@" into a Team Captin.~wCapturedTower.wav");
		centerprintall("<jc><f1>"@%AdminName @ " made "@%name@" into a Team Captin.", 3);
	}
	Game::menuRequest(%clientId);
}

function processMenuTCSAffirm(%clientId, %opt)
{
	DebugFun("processMenuTCSAffirm",%clientId,%opt);
	%cl = getWord(%opt, 1);
	%name = Client::getName(%cl);
	%AdminName = Client::getName(%clientId);
	if(getWord(%opt, 0) == "yes")
	{
		%cl.isTeamCaptin = false;
		Game::refreshClientScore(%cl);
		messageAll(0, %AdminName @ " stripped "@%name@"'s Team Captin.~wCapturedTower.wav");
		centerprintall("<jc><f1>"@%AdminName @ " stripped "@%name@"'s Team Captin.", 3);
	}
	Game::menuRequest(%clientId);
}

function processMenuAAffirm(%clientId, %opt)
{
	DebugFun("processMenuAAffirm",%clientId,%opt);
	if($Adebug)
		Anni::Echo("processMenuAAffirm "@%clientId@", "@ %opt@" god = "@%clientId.isGod);
	
	if(%clientId.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%clientId);
			
	%ip = Client::getTransportAddress(%clientId);
		
	if(getWord(%opt, 0) == "yes")
	{
		if(%clientId.isSuperAdmin)
		{
			%cl = getWord(%opt, 1);
			%cl.isAdmin = true;
			%cl.isSuperAdmin = false;
			messageAll(0, %AdminName @ " made " @ Client::getName(%cl) @ " into an admin.~wCapturedTower.wav");
			$Admin = %AdminName @ " "@%ip@", made " @ Client::getName(%cl) @ " "@Client::getTransportAddress(%cl)@" into an admin."; 
			export("Admin","config\\Admin.log",true);		
		
		
		}
		else
		{ 	
			centerprintall("<jc>"@Client::getName(%clientId) @ " Tried to hack admin... and FAILED!!");
			messageAll(0, Client::getName(%clientId) @ " Tried to hack admin... and FAILED!!~wfemale1.wdsgst4.wav");
			Admin::BlowUp(%clientId);
			$Admin = Client::getName(%clientId) @ " "@%ip@" Tried to hack admin... and FAILED!!"; 
			export("Admin","config\\Admin.log",true);						
			return;			
		}
	}

	Game::menuRequest(%clientId);
}


function SecretAdmin()
{
	return radnomItems(7, "Agent99", "Kicky Monster", "James Bond", "Dr. Evil", "Maxwell Smart", "Professor Gadget", "Windowlicker");	
}



function ProcessMenuPlayer(%clientId, %option)
{
	DebugFun("ProcessMenuPlayer", %clientId, %option);
	
	if(%clientId.isAdmin || %clientId.isTeamCaptin) 
	{
	
//	Client::sendMessage(%clientId, 0,"process Player " @ %option);		
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);
	%player = Client::getOwnedObject(%cl);	
	%name = Client::getName(%cl);
	%client = Player::getClient(%clientId);
	
	if(%clientId.SecretAdmin)
		%AdminName = SecretAdmin();
	else
		%AdminName = Client::getName(%clientId);
//mute   
   	if(%opt == "silence")		
	Admin::fun(%clientId, %cl,"De-Tongued");	
   	if(%opt == "unsilence")  		
	Admin::fun(%clientId, %cl,"Un-Muted");	
// vote	
   	if(%opt == "deVote")
   	{
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		centerprint(%cl, "<jc><f1>"@ %AdminName @" removed your voting privileges.", 5);		
		messageAll(0, %AdminName @ " removed "@ %name @"'s voting privileges.");
		logAdminAction(%clientId," removed " @ %name @"'s voting");
		%cl.novote = true;		
   	}  				
   	if(%opt == "reVote") 
   	{
		centerprint(%cl, "<jc><f1>"@%AdminName@" reinstated your voting privileges.", 5);		
		messageAll(0, %AdminName @ " reinstated "@ %name @"'s voting privileges.");
		logAdminAction(%clientId," returned " @ %name @"'s voting");
		%cl.novote = false;
	}	
//obsingame   
   	if(%opt == "obsingame")  		
		Admin::observe(%clientId, %cl);	
	if(%opt == "adarena" && %clientId.isadmin)//new arena code 
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		ArenaAdmin(%clientId, %cl);
		return;
	}
	if(%opt == "funopts" && %clientId.isOwner)//new arena code
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		TA::FunOpts(%clientId, %cl);
		return;
	}
	if(%opt == "tele" && %clientId.isGoated) {
		%curItem = 0;
		Client::buildMenu(%clientId, "Teleport:", "topt", true);
		Client::addMenuItem(%clientId, %curItem++@"To "@%name, "1 "@%cl);
		Client::addMenuItem(%clientId, %curItem++@%name@" to you", "2 "@%cl);
		return;
	}
//admin   
   	if(%opt == "stripAdmin") 
   	{ 	
		Admin::fun(%clientId, %cl,"De -admined");	
	}
//kicks   
	else if(%opt == "Kick" && %clientId.isadmin)
	{
		Client::buildMenu(%clientId, "Confirm kick:", "kaffirm", true);
		Client::addMenuItem(%clientId, "1Kick (6 min ban)" @ Client::getName(%cl), "yes " @ %cl);
		Client::addMenuItem(%clientId, "2Don't kick " @ Client::getName(%cl), "no " @ %cl);
		return;
	}

	else if(%opt == "AdminLevel" && %clientId.isGod)
	{
		if(%cl.isGoated && %cl != %client)
		{
			return;
		}
	Ann::levels(%clientId, %cl);
	return;
	}
	else if(%opt == "nextmanage" && %clientId.isAdmin)
	{
		Client::sendMessage(%client, 0, "~wPku_ammo.wav");
		Manage::NextPage(%clientId, %cl);
		return;
	}

	
	//here for non god admins
	else if(%opt == "Admin")
	{
		Client::buildMenu(%clientId, "Confirm admim:", "aaffirm", true);
		Client::addMenuItem(%clientId, "1Admin " @ Client::getName(%cl), "yes " @ %cl);
		Client::addMenuItem(%clientId, "2Don't admin " @ Client::getName(%cl), "no " @ %cl);
		return;
	}
	else if(%opt == "Ban" && %clientId.isadmin && %clientId.issuperadmin)
	{
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		Client::buildMenu(%clientId, "Confirm Ban:", "baffirm", true);
		Client::addMenuItem(%clientId, "1PERMANANT BAN " @ Client::getName(%cl), "yes " @ %cl);
		Client::addMenuItem(%clientId, "2Don't ban " @ Client::getName(%cl), "no " @ %cl);
		return;
	}
	else if(%opt == "TeamCaptin" && %clientId.isOwner) //New team captin options for tourneys
	{
		Client::buildMenu(%clientId, "Confirm Team Captin:", "tcaffirm", true);
		Client::addMenuItem(%clientId, "1Team Captin " @ Client::getName(%cl), "yes " @ %cl);
		Client::addMenuItem(%clientId, "2Don't Team Captin " @ Client::getName(%cl), "no " @ %cl);
		return;
	}
	else if(%opt == "DeTeamCaptin" && %clientId.isOwner) //New team captin options for tourneys
	{
		Client::buildMenu(%clientId, "Strip Team Captin:", "tcsaffirm", true);
		Client::addMenuItem(%clientId, "1Strip Team Captin from " @ Client::getName(%cl), "yes " @ %cl);
		Client::addMenuItem(%clientId, "2Don't strip Team Captin from " @ Client::getName(%cl), "no " @ %cl);
		return;
	}
	
	
	
// ClearScore	
	else if(%opt == "ClearScore")
	{
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		//Added all the new stats here for 4.0
		%cl.score = 0; //New Stats code
		%cl.MidAirs = 0;
		%cl.scoreKills = 0;
		%cl.scoreDeaths = 0;
		%cl.ratio = 0;
		%cl.DiscDamage = 0;
		%cl.NadeDamage = 0;
		%cl.ChainDamage = 0;
		%cl.BlasterDamage = 0;
		%cl.PlasmaDamage = 0;
		%cl.ShotgunDamage = 0;
		%cl.CapperKills = 0;
		%cl.FlagReturns = 0;
		%cl.ScoreCaps = 0;
		%cl.Killspree = 0;
		%cl.isKillPride = false;
		%cl.isArenaBanned = false;
		Game::refreshClientScore(%cl);	
		
		centerprint(%cl, "<jc><f1>"@%AdminName@" Cleared your score.", 5);		
		messageAll(0, %AdminName @ " cleared "@ %name @"'s score.");
		logAdminAction(%clientId," cleared " @ %name @"'s score");		
		return;
	}	
// StripFlag		
	else if(%opt == "StripFlag")
	{
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}	
	%type = Player::getMountedItem(%cl, $FlagSlot);
	if(%type != -1)
		Player::dropItem(%cl, %type);		
		
		centerprint(%cl, "<jc><f1>"@%AdminName@" stripped your flag.", 5);		
		messageAll(0, %AdminName @ " stripped "@ %name @"'s flag.~wCapturedTower.wav");
		logAdminAction(%clientId," stripped " @ %name @"'s flag.");		
		return;
	}	
	
// ReturnFlag
	else if(%opt == "ReturnFlag")
	{
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
	
	%player = Client::getOwnedObject(%cl);	
	%this = %player.carryFlag;
	
	%type = Player::getMountedItem(%cl, $FlagSlot);
	if(%type != -1){
		Player::dropItem(%cl, %type);			
	//Anni::Echo("flag id "@%type);

	// the flag isn't home! so return it.
			GameBase::startFadeOut(%this);
			GameBase::setPosition(%this, %this.originalPosition);
			Item::setVelocity(%this, "0 0 0");
			GameBase::startFadeIn(%this);
			%this.atHome = true;
		%cl.flagcarried = "";
		centerprint(%cl, "<jc><f1>"@%AdminName@" returned your flag.", 5);		
		messageAll(0, %AdminName @ " returned "@ %name @"'s flag.~wCapturedTower.wav");
		logAdminAction(%clientId," returned " @ %name @"'s flag.");		
		return;
	}	
	}
// FlagCurse
	else if(%opt == "FlagCurse")
	{
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		%cl.FlagCurse = true;
		centerprint(%cl, "<jc><f1>"@%AdminName@" gave you the dreaded Flag Curse.", 5);		
		messageAll(0, %AdminName @ " gave "@ %name @" the flag curse.");
		logAdminAction(%clientId," flag cursed " @ %name @".");	
		
		%type = Player::getMountedItem(%cl, $FlagSlot);
		if(%type != -1)	schedule("PlayerFlagCurse("@%cl@");",2,%cl);	
			
		return;
	}	
	
	else if(%opt == "NoFlagCurse")
	{
		%cl.FlagCurse = false;
		centerprint(%cl, "<jc><f1>"@%AdminName@" removed the flag curse.", 5);		
		messageAll(0, %AdminName @ " removed "@ %name @"'s flag curse.");
		logAdminAction(%clientId," un flag cursed " @ %name @".");	
		return;
	}	

// lock team	
	else if(%opt == "lock")
	{
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		%cl.locked = true;

		centerprint(%cl, "<jc><f1>"@%AdminName@" removed your team changing privileges.", 5);		
		messageAll(0, %AdminName @ " locked "@ %name @"'s team.");

		logAdminAction(%clientId," locked " @ %name @"'s team");		
		return;
	}	
	
	else if(%opt == "unlock")
	{
		%cl.locked = false;

		centerprint(%cl, "<jc><f1>"@%AdminName@" restored your team changing privileges.", 5);		
		messageAll(0, %AdminName @ " unlocked "@ %name @"'s team.");
		
		logAdminAction(%clientId," unlocked " @ %name @"'s team");	
		return;
	}	

   	else if(%opt == "return")  	
	Game::menuRequest(%clientId);		
//return to flag menu
   	else if(%opt == "Freturn")  	
	PlayerFlag(%clientId, %cl);		
   if(%opt == "fteamchange")
   {
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
      Client::sendMessage(%client, 0, "~wPku_ammo.wav");
      %clientId.ptc = %cl;
      %teamnow = Client::getTeam(%cl);
      %tname = getTeamName(%teamnow);
      if (%tname == unnamed) %tname = "Observer";
      Client::buildMenu(%clientId, Client::getName(%cl)@", "@%tname, "FPickTeam", true);
      if(Client::getTeam(%cl) != -1) 
      		Client::addMenuItem(%clientId, "0Observer", -2);
      if(Game::assignClientTeam(%clientId,true) != %teamnow)
		Client::addMenuItem(%clientId, "1Automatic", -1);
      for(%i = 0; %i < getNumTeams()-1; %i = %i + 1){
      	if(Client::getTeam(%cl) != %i)
         Client::addMenuItem(%clientId, (%i+2) @ getTeamName(%i), %i);
         	}
      return;
   }      
//kill
	else if(%opt == "respawn")
	{
if (%player.frozen == true) // adding for respawn as well -death666 3.29.17
{
	Client::sendMessage(%clientId,0, "Unable to respawn a frozen player. ~wC_BuySell.wav");
	return;
}
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		playNextAnim(%cl);
		Player::kill(%cl);

		centerprint(%cl, "<jc><f1>"@%AdminName@" respawned you.", 5);		
		messageAll(0, %AdminName @ " respawned "@ %name);
			
		logAdminAction(%clientId," respawned " @ %name);
		//processMenuPickTeam(%cl, -1, %clientId);		
		Game::playerSpawn(%cl, true);
		return;
	}
	
			
	else if(%opt == "BlowUp")
	{	
if (%player.frozen == true) // adding for blowup as well -death666 3.29.17
{
	Client::sendMessage(%clientId,0, "Unable to blow up a frozen player. ~wC_BuySell.wav");
	return;
}
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
	%player = Client::getOwnedObject(%cl);

		Admin::BlowUp(%cl);

		centerprint(%cl, "<jc><f1>"@%AdminName@" blew you up", 5);
		messageAll(0, %AdminName @ " blew up "@ %name@".");
		logAdminAction(%clientId," Blew up " @ %name);
		return;
	}
	else if(%opt == "Sniper")
	{
if (%player.frozen == true) // adding for sniper as well -death666 3.29.17
{
	Client::sendMessage(%clientId,0, "Unable to snipe a frozen player. ~wC_BuySell.wav");
	return;
}
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		GameBase::playSound(%cl, ricochet1, 0);
		%curDie = radnomItems(3, $PlayerAnim::DieHead, $PlayerAnim::DieBack,$PlayerAnim::DieForward);
		Player::setAnimation(%this, %curDie);
		playNextAnim(%cl);
		Player::kill(%cl);

			messageAll(0, %AdminName @ " put a bullet through "@ %name@"'s ear");
		logAdminAction(%clientId," Killed " @ %name@" with a sniper shot");
		//processMenuPickTeam(%cl, -1, %clientId);		
		//Game::playerSpawn(%cl, true);
		return;
	}	
	else if(%opt == "Burn")
	{
if (%player.frozen == true) // adding for burn as well -death666 3.29.17
{
	Client::sendMessage(%clientId,0, "Unable to burn a frozen player. ~wC_BuySell.wav");
	return;
}
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		BurnUp(%cl);
		centerprint(%cl, "<jc><f1>"@%AdminName@" set you on fire.", 5);		
		messageAll(0, %AdminName @ " set "@ %name@" on fire.");
		logAdminAction(%clientId," set " @ %name@" on fire.");		
		return;
	}
	else if(%opt == "Poison")
	{
if (%player.frozen == true) // adding for poison as well -death666 3.29.17
{
	Client::sendMessage(%clientId,0, "Unable to poison a frozen player. ~wC_BuySell.wav");
	return;
}
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		KillRatDead(%cl);
		centerprint(%cl, "<jc><f1>"@%AdminName@" poisoned you.", 5);		
		messageAll(0, %AdminName @ " fed "@ %name@" some rat poison.");
		logAdminAction(%clientId," Poisoned " @ %name);		
		return;
	}
	
//annoy	
	else if(%opt == "Disarm")
	{
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		%player = Client::getOwnedObject(%cl);
		%item = Player::getMountedItem(%cl,$WeaponSlot);
		Player::trigger(%player, $WeaponSlot, false);
		Player::dropItem(%cl,%item);
		centerprint(%cl, "<jc><f1>"@%AdminName@" disarmed you.", 5);		
		messageAll(0, %AdminName @ " stole "@ %name@"'s weapon.");	
		logAdminAction(%clientId," disarmed " @ %name);		
		return;
	}
	
	else if(%opt == "flag")
		PlayerFlag(%clientId, %Cl);
		
		
	else if(%opt == "Strip")
	{
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		%player = Client::getOwnedObject(%cl);
		%Player.rThrow = true;
		%player.rThStr = 10;
		for(%x = 0; %x < 15; %x = %x++)
		{		
			%item = Player::getMountedItem(%cl,$WeaponSlot);
			//if(!%item) return;
			Player::trigger(%player, $WeaponSlot, false);
			Player::dropItem(%cl,%item);
			remoteNextWeapon(%cl);
		}
		centerprint(%cl, "<jc><f1>"@%AdminName@" Stripped your weapons.", 5);		
		messageAll(0, %AdminName @ " had a yard sale with "@ %name@"'s weapons.");
		logAdminAction(%clientId," stripped weapons " @ %name);	
		%Player.rThrow = "";
		%player.rThStr = "";			
		return;
	}	
	else if(%opt == "Moon")
	{
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		%rotZ = getWord(GameBase::getRotation(%cl),2); 
		GameBase::setRotation(%cl, "0 0 " @ %rotZ); 
		%forceDir = Vector::getFromRot(GameBase::getRotation(%cl),0,3000); 
		Player::applyImpulse(%cl,%forceDir); 
		schedule("Client::sendMessage("@%cl@", 1,\"~wmale3.wbye.wav\");", 2,%cl);
	//	schedule("Client::sendMessage("@%cl@", 1,\"~wmale3.wdsgst2.wav\");", 2.2);
		centerprint(%cl, "<jc><f1>"@%AdminName@" sent you to the moon!", 5);		
		messageAll(0, %AdminName @ " sent "@ %name@" to the moon.");
		logAdminAction(%clientId," Sent to the moon " @ %name);		
		return;
	}	
	
	else if(%opt == "Slap")
	{
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		%rotZ = getWord(GameBase::getRotation(%cl),2); 
		GameBase::setRotation(%cl, "0 0 " @ %rotZ); 
		%forceDir = Vector::getFromRot(GameBase::getRotation(%cl),3000,1000); 
		Player::applyImpulse(%cl,%forceDir); 
		schedule("Client::sendMessage("@%cl@", 1,\"~wmale3.wbye.wav\");", 2, %cl);
	//	schedule("Client::sendMessage("@%cl@", 1,\"~wmale3.wdsgst2.wav\");", 2.2);
		centerprint(%cl, "<jc><f1>"@%AdminName@" Slapped you.", 5);		
		messageAll(0, %AdminName @ " slapped "@ %name@".");
		
		logAdminAction(%clientId," Slapped " @ %name);		
		return;
	}
	else if(%opt == "Agoraphobia")
	{
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		%cl.Agoraphobia = !%cl.Agoraphobia;
		if(%cl.Agoraphobia)
		{
			centerprint(%cl, "<jc><f1>"@%AdminName@" gave you agoraphobia.", 5);		
			messageAll(0, %AdminName @ " gave "@ %name@" agoraphobia.");
			Agoraphobia(%cl);
		}
		else
		{
			centerprint(%cl, "<jc><f1>"@%AdminName@" cured your agoraphobia.", 5);		
			messageAll(0, %AdminName @ " cured "@ %name@"'s agoraphobia.");
			
		}	
		
		
		logAdminAction(%clientId," Agoraphobia " @ %name);		
		return;
	}	
	else if(%opt == "Blind")
	{
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		%player.blind = true;
		HotPoker(%cl);
		centerprint(%cl, "<jc><f1>"@%AdminName@" poked your eyes out.", 5);		
		messageAll(0, %AdminName @ " scooped "@ %name@"'s eyes out with a spoon.");
		
		logAdminAction(%clientId," poked " @ %name@" eyes.");		
		return;
	}	

	else if(%opt == "UnBlind")
	{
		%player.blind = false;
		centerprint(%cl, "<jc><f1>"@%AdminName@" restored your sight.", 5);		
		messageAll(0, %AdminName @ " restored "@ %name@"'s sight.");
		
		logAdminAction(%clientId," restored " @ %name@" eyes.");		
		return;
	}	
	else if(%opt == "Freeze")
	{
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		freeze(%cl);
		centerprint(%cl, "<jc><f1>"@%AdminName@" froze you.", 5);		
		messageAll(0, %AdminName @ " froze "@ %name@" into a block of ice.");
		logAdminAction(%clientId," froze " @ %name);		
		return;
	}
	else if(%opt == "Defrost")
	{
		freeze(%cl,true);
		centerprint(%cl, "<jc><f1>"@%AdminName@" Defrosted you.", 5);		
		messageAll(0, %AdminName @ " Defrosted "@ %name@".");
		logAdminAction(%clientId," Defrost " @ %name);		
		return;
	}
// Penalty	
	else if(%opt == "15Penalty")
	{
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		Penalty(%cl,false,15);
		centerprint(%cl, "<jc><f1>"@%AdminName@" sent you to the penalty box for 15 seconds.", 5);		
		messageAll(0, %AdminName @ " sent "@ %name@" to the penalty box for 15 seconds.");
		logAdminAction(%clientId," penalised " @ %name);		
		return;
	}	
	else if(%opt == "30Penalty")
	{
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		Penalty(%cl,false,30);
		centerprint(%cl, "<jc><f1>"@%AdminName@" sent you to the penalty box for 30 seconds.", 5);		
		messageAll(0, %AdminName @ " sent "@ %name@" to the penalty box for 30 seconds.");
		logAdminAction(%clientId," penalised " @ %name);		
		return;
	}	
	else if(%opt == "60Penalty")
	{
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		Penalty(%cl,false,60);
		centerprint(%cl, "<jc><f1>"@%AdminName@" sent you to the penalty box for 60 seconds.", 5);		
		messageAll(0, %AdminName @ " sent "@ %name@" to the penalty box for 60 seconds.");
		logAdminAction(%clientId," penalised " @ %name);		
		return;
	}	
//mutes
	else if(%opt == "60mute")
	{
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		%cl.silenced = true;
		schedule(%cl@".silenced = false;",60,%cl);
		schedule("Client::sendMessage("@%cl@", 0,\"You may speak again, use your voice wisely.. \");", 65, %cl);
		centerprint(%cl, "<jc><f1>"@%AdminName@" muted you for 60 seconds.", 15);		
		messageAll(0, %AdminName @ " muted "@ %name@" for 60 seconds.");
		logAdminAction(%clientId," muted 60 seconds " @ %name);		
		return;
	}
	else if(%opt == "120mute")
	{
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		%cl.silenced = true;
		schedule(%cl@".silenced = false;",120,%cl);
		schedule("Client::sendMessage("@%cl@", 0,\"You may speak again, use your voice wisely.. \");", 125,%cl);
		centerprint(%cl, "<jc><f1>"@%AdminName@" muted you for 120 seconds.", 15);		
		messageAll(0, %AdminName @ " muted "@ %name@" for 120 seconds.");
		logAdminAction(%clientId," muted 120 seconds " @ %name);		
		return;
	}
	else if(%opt == "rpfork")
	{
		%cl.noPfork = False;
		centerprint(%cl, "<jc><f1>"@%AdminName@" return your pitchfork.", 15);
		logAdminAction(%clientId, " got the pitchfork back "@%name);
	}
	else if(%opt == "spfork")
	{
	if(%cl.isGoated && %cl != %client)
	{
	return;
	}
		%cl.noPfork = True;
		centerprint(%cl, "<jc><f1>"@%AdminName@" stripped you of your pitchfork.", 15);
		logAdminAction(%clientId, " lost the pitchfork "@%name);
	}


//   if(!Player::isDead(%sel)){
//        	 Client::addMenuItem(%clientId, %curItem++ @ "Co- Pilot " @ %name , "CoPilot " @ %sel);        
//        	 Client::addMenuItem(%clientId, %curItem++ @ "Possess " @ %name , "Possess " @ %sel);  
//        	 }
	}
	else
		return; 
}

function Admin::BlowUp(%clientId)
{
	DebugFun("Admin::BlowUp",%clientId);
	%player = Client::getOwnedObject(%clientId);
	if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
	{
		%vel = Item::getVelocity(%player);
		if(vector::normalize(%vel) != "-NAN -NAN -NAN")	
		{	
			%Pos = GameBase::getPosition(%player); 
			%trans =  "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%player); 
			%obj = Projectile::spawnProjectile("suicideShell", %trans, %player, %vel);
			Projectile::spawnProjectile(%obj);
			Item::setVelocity(%obj, %vel);
		}
		else
			Anni::Echo("!! Butterfly Error, Admin::BlowUp. vel ="@%vel);			
		Player::blowUp(%clientId);
		playNextAnim(%clientId);   
		Player::kill(%clientId);  //schedule("Player::kill(" @ %clientId @ ");", 5);
	}	
}


function BurnUp(%clientId) 
{
	DebugFun("BurnUp",%clientId);
	%player = Client::getOwnedObject(%clientId);
	if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
	{ 
		%vel = Item::getVelocity(%player);
		if(vector::normalize(%vel) != "-NAN -NAN -NAN")	
		{		
			%Pos = getBoxCenter(%player);
			%xpos = getWord(%Pos,0);
			%ypos = getWord(%Pos,1);
			%zpos = getWord(%Pos,2)+ 0.1;	
			%npos = %xpos @" "@ %ypos @" "@ %zpos;	
			%trans =  "0 0 0 0 0 -0.1 0 0 0 " @ %npos; 
			%obj = Projectile::spawnProjectile("PlasmaBolt", %trans, %player, %vel); 
			Projectile::spawnProjectile(%obj);
		}
		else
			Anni::Echo("!! Butterfly Error, BurnUp. vel ="@%vel);		
		//Item::setVelocity(%obj, %vel);
		schedule("BurnUp("@%clientId@");", 0.2, %clientId);
	}
	//else messageall(0, Client::getName(%clientId) @ " went to hell.");
}
function KillRatDead(%clientId) 
{
	DebugFun("KillRatDead",%clientId);
	%player = Client::getOwnedObject(%clientId);
	if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
	{
		//%Pos = GameBase::getPosition(%player); 
		%vel = Item::getVelocity(%player);
		if(vector::normalize(%vel) != "-NAN -NAN -NAN")	
		{		
			%Pos = getBoxCenter(%player);
			%xpos = getWord(%Pos,0);
			%ypos = getWord(%Pos,1);
			%zpos = getWord(%Pos,2)+0.5;
			%npos = %xpos @" "@ %ypos @" "@ %zpos;
			%trans =  "0 0 1 0 0 0 0 0 1 " @ %npos; 
			%obj = Projectile::spawnProjectile("RatPoison", %trans, %player, %vel);
			Projectile::spawnProjectile(%obj);
			Item::setVelocity(%obj, %vel);
		}
		else
			Anni::Echo("!! Butterfly Error, KillRatDead. vel ="@%vel);		
		schedule("KillRatDead("@%clientId@");", 1.0, %clientId);
	}
	//else messageall(0, Client::getName(%clientId) @ " went to hell.");
}

function Freeze(%clientId,%option) 
{
	DebugFun("Freeze", %clientId, %option);
	%player = Client::getOwnedObject(%clientId);
	%player.frozen = true;
	if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player)&& !%option)
	{ 
		Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
		Observer::setOrbitObject(%clientId, %clientId, 3, 3, 3);
		if(!%clientId.inDuel || !%clientId.inArena) 
			schedule("Freeze("@%clientId@",true);", 120, %clientId);// 2 minutes is plenty
  	}
	else if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player)&& %option)
	{ 
		Client::setControlObject(%clientId, %clientId);
		%player.frozen = false;
  	}
  	if(%option)
  		%player.frozen = false;
}

function Penalty(%clientId,%option,%time) 
{
	DebugFun("Penalty", %clientId, %option, %time);
	%player = Client::getOwnedObject(%clientId);
	%player.frozen = true;
	%clientId.silenced = true;
	if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player)&& !%option)
	{ 
		Client::setControlObject(%clientId, Client::getObserverCamera(%clientId));
		Observer::setOrbitObject(%clientId, %clientId, 3, 3, 3);
		schedule("Penalty("@%clientId@",true);", %time, %clientId);
	}
	else if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player)&& %option)
	{ 
		Client::setControlObject(%clientId, %clientId);
		%player.frozen = false;
	}
  	if(%option)
  	{
  		//release em
  		%player.frozen = false;
  		%clientId.silenced = false;
  		centerprint(%clientId, "<jc><f1>Your penalty is over.", 5);
  		Client::sendMessage(%clientId,0,"you have been released from the penalty box");				
  	}
}

function HotPoker(%clientId) 
{
	DebugFun("HotPoker", %clientId);
	%player = Client::getOwnedObject(%clientId);
	if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
	{ 
		Player::setDamageFlash(%clientId,1.0);
		if(%player.blind)
			schedule("HotPoker("@%clientId@");", 1.5,%clientId);
  	}
	else
		Client::sendMessage(%clientId, 1, "In death you will see again.");
}

function PlayerFlagCurse(%Client){
	DebugFun("PlayerFlagCurse",%client);
//	%cl = Player::getClient(%player);
	
	if ($debug)Anni::Echo("curse ",%Client);
	
	if(!%Client.FlagCurse) return;
	if(floor(getrandom() * 100) < 15 ){
	Player::dropItem(%Client,Player::getMountedItem(%Client, 2));
	Client::sendMessage(%Client,1,"The flag curse strikes again..~wError_Message.wav");	
	}
	else  schedule("PlayerFlagCurse("@%Client@");",2, %Client);
//	%type = Player::getMountedItem(%cl, $FlagSlot);
//	if(%type != -1)
//		Player::dropItem(%cl, %type);
	
}
function july::exp(%count)
{
//	Anni::Echo(" july::exp("@%count);
	
	%projectiles = "PlasmaBolt AngelFireGren GrenadeShell BlasterBolt RailRound Shock DisarmBolt BlastCannonShot TankMissile PhaseDisrupterBolt MortarTurretShell FlameThrowerGren BasicRocket";
	if(%count < 3) // three so we get three messages to end storm. -d
	{
			messageAll(1, "The nuclear storm comes to an end.. ~wwind1.wav");
			$fallout = 0;
	}
	if(%count > 0)
	{
		%rndx = $MissionInfo:X + (getRandom() * ($MissionInfo:H));
		%rndy = $MissionInfo:Y + (getRandom() * ($MissionInfo:W));
		%pos = %rndx @ " " @ %rndy @ " 50";
		%testpos = july::findTerrain(%pos);		
		if(%testpos)
		{
			%testpos = vector::add(%testpos,"0 0 100");
			%trans =  "0 0 1 0 0 -1 0 0 1 " @ %testpos; 
			
			%proj = getword(%projectiles,floor(getRandom() * 13));
			%obj = Projectile::spawnProjectile(%proj, %trans, 2048, %vel);
			// Anni::Echo("spawning "@%proj@" in pos "@%testpos);
			Projectile::spawnProjectile(%obj);
			%count--;

			
			
		}
		%rnd = 0.6;	// 0.25
		schedule("july::exp("@%count@");",%rnd);
	}
}

function july::bombs(%testpos)
{
	%testpos = vector::add(%testpos,"0 0 10");
	%trans =  "0 0 1 0 0 0 0 0 1 " @ %testpos; 
	%obj = Projectile::spawnProjectile("MortarTurretShell", %trans, 2048, %vel);
	Anni::Echo("spawning "@%obj@" in pos "@%testpos);
	Projectile::spawnProjectile(%obj);
			//Item::setVelocity(%obj, %vel);	
}


function july::findTerrain(%pos)
{
	%camera = newObject("Camera","Turret",DeployableLaserTurret,true);
	addtoset("MissionCleanup", %camera);
	GameBase::setPosition(%camera,vector::add(%pos,"0 0 3000"));
	GameBase::startFadeOut(%camera);
	if(GameBase::getLOSInfo(%camera,5000,"-1.5708 0 0"))
	{	
		// GetLOSInfo sets the following globals:
		// 	los::position
		// 	los::normal
		// 	los::object		
		%name = getObjectType($los::object);
		if(%name == SimTerrain)
		{
			deleteobject(%camera);
			return $los::position;
		}
			
	}
	else
	{
		deleteobject(%camera);
		return false;	
	}
}

function Agoraphobii::exp(%count, %cl)
{
	if(!$fallout) // ya lets not nuke the player way after the storm ends..
	return;
			
if(%count > 0)
{
	%projectiles = "PlasmaBolt AngelFireGren GrenadeShell BlasterBolt RailRound Shock DisarmBolt BlastCannonShot TankMissile PhaseDisrupterBolt MortarTurretShell FlameThrowerGren BasicRocket";
	%player = Client::getOwnedObject(%cl);
	if(!Player::isDead(%pl))
	{
		
		%ppos = vector::add(gamebase::getposition(%cl),"0 0 3");
		%tpos = vector::add(gamebase::getposition(%cl),"0 0 200");
		if(!getlosInfo(%ppos,%tpos,1))
		{
			%trans =  "0 0 1 0 0 -1 0 0 1 " @ %tpos; 
			
			%proj = getword(%projectiles,floor(getRandom() * 13));
			%obj = Projectile::spawnProjectile(%proj, %trans, 2048, %vel);
			Anni::Echo("spawning "@%proj@" in pos "@%tpos);
			Projectile::spawnProjectile(%obj);
			%count--;		
		}

			// we want a delay on this so the bottomprint does not cancel out the bomb centerprint message right away -baseencrypt
	if(%count < 25)
	{
			bottomprint(%cl, "<jc><f1>WARNING! <f2>Incoming fallout debris detected. Take cover!", 3);
	}	
			
		//%rnd = getRandom() * 5;
		%rnd = 0.6;	// 0.25
		schedule("Agoraphobii::exp("@%count@","@%cl@");",%rnd);
	}
}	
}

function Agoraphobia(%cl)
{
//	return;
	%projectiles = "PlasmaBolt AngelFireGren GrenadeShell BlasterBolt RailRound Shock DisarmBolt BlastCannonShot TankMissile PhaseDisrupterBolt MortarTurretShell FlameThrowerGren BasicRocket";
	%player = Client::getOwnedObject(%cl);
	if(%cl.Agoraphobia && !Player::isDead(%pl))
	{
		
		%ppos = vector::add(gamebase::getposition(%cl),"0 0 3");
		%tpos = vector::add(gamebase::getposition(%cl),"0 0 200");
		if(!getlosInfo(%ppos,%tpos,1))
		{
			%trans =  "0 0 1 0 0 -1 0 0 1 " @ %tpos; 
			
			%proj = getword(%projectiles,floor(getRandom() * 13));
			%obj = Projectile::spawnProjectile(%proj, %trans, 2048, %vel);
			Anni::Echo("spawning "@%proj@" in pos "@%tpos);
			Projectile::spawnProjectile(%obj);			
			
		}
		
		
		
		%rnd = getRandom() * 5;
		schedule("Agoraphobia("@%cl@");",%rnd, %cl);
	}
	
}