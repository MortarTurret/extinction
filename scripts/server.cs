// putting a global variable in the argument list means:
// if an argument is passed for that parameter it gets
// assigned to the global scope, not the scope of the function

exec("AntiCrash.cs"); 
exec("PlayerProfile.cs");



function createTrainingServer()
{
	$SinglePlayer = true;
	createServer($pref::lastTrainingMission, false);
}

function event::collision(%this,%obj)
{
	%objName = GameBase::getDataName(%this);
	%moveable = GameBase::getDataName(%obj);
	Anni::Echo("?? EVENT collision "@%objName@" contacted by "@%moveable@" control cl# "@GameBase::getControlClient(%obj));	
}

function CountDeploy(%type)
{
	if(%type != -1)
	{
		%simset = nameToID("MissionCleanup/deployed/"@%type);
		for(%i = 0; (%o = Group::getObject(%simset, %i)) != -1; %i++)
		{			
		}
	}
	return %i;
}

function item::count()
{
	if($StaticShape::count < 0)
	 	$StaticShape::count = "";	
	if($Ammo::count < 0) 	
		$Ammo::count = "";
	if($item::count < 0) 
		$item::count = "";
	if($mine::count < 0) 
		$mine::count = "";
	if($turret::count < 0) 
		$turret::count = "";
	
	%count = $StaticShape::count + $Ammo::count + $item::count + $mine::count + $turret::count;
	if(%count > 300)
		Anni::Echo("!! Objs= "@%count@", Turrets= "@$turret::count @ ", Statics= " @$StaticShape::count@ ", Ammo mines= "@$Ammo::count@", Items= "@$item::count@", Mineslaunched= "@$mine::count);

	if(%count > 800) // 850
	{
		messageAll(0, "WARNING server exceeding maximum trackable objects. ~wCapturedTower.wav");
		bottomprintall("<jc><f2>!! WARNING !!<f1> server reaching critical levels <f2>"@%count@"<f1> objects in server.", 20);
	}

	if(%count > 850) // 1000
		KillDeploy();
	else if(%count > 830) // 850
	{
		%turretNUM = CountDeploy(turret);
		%objectNUM = CountDeploy(object);
		%BarrierNUM = CountDeploy(Barrier);
		%sensorNUM = CountDeploy(sensor);
		%powerNUM = CountDeploy(power);
		%stationNUM = CountDeploy(station);
		
		%total = %turretNUM + %objectNUM + %BarrierNUM + %sensorNUM + %powerNUM + %stationNUM;
		%threshold = %total/3;
		
		
		if(%BarrierNUM > %threshold)
			UnDeploy(Barrier,%BarrierNUM/10);
		else if(%turretNUM > %threshold)
			UnDeploy(turret,%turretNUM/10);			
	
	}	
	if($ghosting)
		schedule("item::count();",15);
	
}

function rotateVector(%vec,%rot){
	%pi = 3.14;
	%rot3= getWord(%rot,2);
	for(%i = 0; %rot3 >= %pi*2; %i++) %rot3 = %rot3 - %pi*2;
	if (%rot3 > %pi) %rot3 = %rot3 - %pi*2;
//	Anni::Echo(%rot3);
	%vec1= getWord(%vec,0);
	%vec2= getWord(%vec,1);
	%vc = %vec2;
	%vec3= getWord(%vec,2);
//	%ray = pow(pow(%vec1,2)+ pow(%vec2,2),0.5);  
	%ray = %vec1;
//	Anni::Echo("length ",%ray);
	%vec1 = %ray*cos(%rot3);
	%vec2 = %ray*sin(%rot3);
	%vec = %vec1 @" "@ %vec2 @" "@ %vec3;
	%vec = Vector::add(%vec,Vector::getFromRot(%rot,%vc,0));
	return %vec;
	}


function remoteSetCLInfo(%clientId, %skin, %name, %email, %tribe, %url, %info, %autowp, %enterInv, %msgMask, %pskin)
{	
	if( CheckEval("remoteSetCLInfo", %clientId, %skin, %name, %email, %tribe, %url, %info, %autowp, %enterInv, %msgMask, %pskin) )
		return;	
		
	$Client::info[%clientId, 0] = Ann::Clean::string(%skin);
	$Client::info[%clientId, 1] = Ann::Clean::string(%name);
	$Client::info[%clientId, 2] = Ann::Clean::string(%email);
	$Client::info[%clientId, 3] = Ann::Clean::string(%tribe);
	$Client::info[%clientId, 4] = Ann::Clean::string(%url);
	$Client::info[%clientId, 5] = Ann::Clean::string(%info);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        Server::validate(%clientId);	
	if(%autowp)
		%clientId.autoWaypoint = true;
	if(%enterInv)
		%clientId.noEnterInventory = true;
	if(%msgMask != "")
		%clientId.messageFilter = %msgMask;
	if(%pskin)
		%clientId.personalskin = true;
	%clientId.suicideTimer = 10;
	
}

function Server::storeData()
{
	$ServerDataFile = "serverTempData" @ $Server::Port @ ".cs";
	export("Server::*", "temp\\" @ $ServerDataFile, False);
	export("pref::lastMission", "temp\\" @ $ServerDataFile, true);
	EvalSearchPath();
	
}

function Server::refreshData()
{
	exec($ServerDataFile);  
	checkMasterTranslation();
	Server::nextMission(false,true);
	exec("TotalAnnihilation_Settings");
	
}

function Server::onClientDisconnect(%clientId)
{
	%clientId.Invalid = True;
	if ( !PlayerProfile::onClientDisconnect(%clientId) ) 
		Ann::PlayerInfo::Disconnect(%clientId);	

	if($annihilation::DisableTurretsOnTeamChange)
		Turret::DisableClients(%clientId);
	
	%clientId.score = 0; 
	%clientId.MidAirs = 0;
	%clientId.scoreKills = 0;
	%clientId.scoreDeaths = 0;
	%clientId.ratio = 0;
	%clientId.DiscDamage = 0;
	%clientId.NadeDamage = 0;
	%clientId.ChainDamage = 0;
	%clientId.BlasterDamage = 0;
	%clientId.PlasmaDamage = 0;
	%playerId.ShotgunDamage = 0;
	%clientId.CapperKills = 0;
	%clientId.FlagReturns = 0;
	%clientId.ScoreCaps = 0;
	%clientId.Killspree = 0;
	%playerId.isNotBot = false;
	%clientId.isArenaBanned = false;
	%clientId.isGoated = false;
	%clientId.isPackin = false;
	%clientId.isGayPride = false;
	%clientId.isKillPride = false;
	%clientId.hitmarker = false;
	%clientId.tmuted = "";
	%clientId.noVpack = "";
	%clientId.silenced = "";
	%clientId.BlockMySound = "";
	
	%clientId.isUntouchable = false;	
	%clientId.isAdmin = false; 
	%clientId.isSuperAdmin = false; 
	%clientId.isGod = false;
	%clientId.isOwner = false;	
	
	%clientId.fetchdata = "";
	%ClientId.FlagCurse = "";
	%clientId.droid = "";	
	%clientId.bk = "";
	%clientId.ListType = "";
	%clientId.invo = ""; 
	%clientId.ListType = "";	
	%ClientId.InvConnect = "";
	%clientId.InvTargetable = "";
	%clientId.ConnectBeam = "";	
	%clientId.isSpy = "";	
	%clientId.OrigTeam = "";	
	%clientId.locked = "";		
	%ClientId.SecretAdmin = "";
	%clientId.novote = "";
	
	if(%clientId.isTDCaptOne)
		ArenaTD::Leave(%clientId);
	
	if(%clientId.isTDCaptTwo)
		ArenaTD::Leave(%clientId);
	
	if(%clientId.inArenaWin)
			ArenaWin::Finish(%clientId,false);
	
	//Arena Stuff
		%clientId.inArena = false;
		%clientId.arenajug = false;
	
	//ArenaTD
		%clientId.inArenaTD = false;
		%clientId.isArenaTDDead = false;
	
	//Team 1
		%clientId.inArenaTDOne = false;
		%clientId.isTDCaptOne = false;
		%clientId.TDRequestOne = false;
		%clientId.TDMRequestOne = false;
		
	//Team 2
		%clientId.inArenaTDTwo = false;
		%clientId.isTDCaptTwo = false;
		%clientId.TDRequestTwo = false;
		%clientId.TDMRequestTwo = false;
	
	//Capt Prefs
		%clientId.ArenaTDMap = "None";
		%clientId.ArenaTDSpawnType = "None";
		%clientId.ArenaTDWeaponOpt = "None";
	
	if(%clientId.inDuel)
		Duel::Leave(%clientId); //get rid of duel too.
	
	%cl = %clientId.whisperFrom;	//whisper drop fix. 1/11/2006 11:43AM
	%cl.whisper = "";	
	%clientId.whisper = "";
	
	deleteVariables("$Client::info"@%clientId@"*"); 
	
	%numPlayers = getNumClients()-1;
	//$AnnihilationServer::Password = $Server::Password;
	if(%numPlayers < $Annihilation::OverflowLimit)
	{
		
		$Server::Password = $AnnihilationServer::Password;
		%lock = ". Below overflow limit, removing overflow password. New password= \""@$Server::Password@"\"";		
	}
//	Anni::Echo("Password = \""@$server::password@"\" Backup = \""@$AnnihilationServer::Password@"\" Overflow limit = \""@$Annihilation::OverflowLimit@"\" Overflow Password = \""@$Annihilation::OverflowPassword@"\"");	
//	Anni::Echo("Number of Clients: "@%numPlayers@%lock);	
	
	// Need to kill the player off here to make everything
	// is cleaned up properly.
	%player = Client::getOwnedObject(%clientId);
	if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player)) 
	{
		playNextAnim(%player);
		Player::kill(%player);
	}
	Client::setControlObject(%clientId, -1);
	if($VoteisActive==true)
	{
		if($VoteID == %clientId)
		{
			%address = Client::getTransportAddress(%clientId);
			banlist::add(%address, 900);
		}
	}
	Client::leaveGame(%clientId);

	Game::CheckTourneyMatchStart();
	if($Annihilation::FairTeams)
		schedule("FairTeamCheck();",2);	
	if($Annihilation::ResetServer != false)
		if(getNumClients() == 1) // this is the last client.
			Server::refreshData();
//			Quit();
}

function KickDaJackal(%clientId)
{
	if(%clientId.isGoated == true)
	{
		%name = Client::getName(%clientId);
		messageAll(0, %name@" cannot be kicked.");
		return;
	}
	Net::kick(%clientId, "The FBI has been notified.  You better buy a legit copy before they get to your house.");
}

function KickDaJackAss(%clientId)
{
	if(%clientId.isGoated == true)
	{
		%name = Client::getName(%clientId);
		messageAll(0, %name@" cannot be kicked.");
		return;
	}
	Net::kick(%clientId, "You are not allowed on this Server. Email " @ $Annihilation::BannedEmail @ " to have ban removed.");
}

function Server::onClientConnect(%clientId)
{
	%clientId.Invalid = True;
//	%clientId.LHStatus = "has not been tested.";
	%clientId.floodRemote = "";
	%clientId.RemoteAllowTime = "";
	%clientId.floodRemoteCount = 0;
	%clientId.KPSilence = False;
	%address = Client::getTransportAddress(%clientId);
	banlist::add(%address, 5);	
	%name = client::getName(%clientId);

	if(string::findSubStr(client::getName(%clientId), ".bmp>") != "-1" || client::getName(%clientId) == "" || string::findSubStr(client::getName(%clientId), "<R") != "-1" || string::findSubStr(client::getName(%clientId), "<L") != "-1" || string::findSubStr(client::getName(%clientId), "<S") != "-1")
	{
		
		%msg = "Try a different name...";
		schedule("net::kick("@%clientId@", \""@%msg@"\");",0.2,%clientId);
		banlist::add(client::getTransportAddress(%clientId), 60);
		return;
	}
	
	PlayerProfile::onClientConnect(%clientId); 
		Ann::PlayerInfo::Connect(%clientId);	

	Anni::Echo("CONNECT: " @ %clientId @ " \"" @ escapeString(%name) @ "\" " @ %address@", "@timestamppatch());
	
   	$AnnCLog = Client::getName(%clientId) @ " " @ %address;
	export("AnnCLog","config\\Ann_Connect.log",true);
	Godadmin::message($AnnCLog);
	
	%numPlayers = getNumClients();
	//$AnnihilationServer::Password = $Server::Password;	//back up
	if($Annihilation::OverflowLimit && $Annihilation::OverflowLimit > 0 )
	{
		if($Annihilation::OverflowPassword != "" && $Annihilation::OverflowPassword != -1)
		{
			if(%numPlayers >= $Annihilation::OverflowLimit )
			{
				
				$Server::Password = $Annihilation::OverflowPassword;
				%lock = ". Overflow limit reached, passwording server with \""@$Server::Password@"\"";
				
			}
		}
		else
			Anni::Echo("Overflow limit reached. No overflow password defined...");
	}
//	Anni::Echo("Password = \""@$server::password@"\" Backup = \""@$AnnihilationServer::Password@"\" Overflow limit = \""@$Annihilation::OverflowLimit@"\" Overflow Password = \""@$Annihilation::OverflowPassword@"\"");	
	//GeoIP::OnConnect(%clientId, Ann::ipCut(%address));
	Anni::Echo("Number of Clients: "@%numPlayers@%lock);
	
	if(!String::NCompare(%address, $messageIP, $messageIPLen) && $messageIP != "")
	{
		%clientId.SpecialMessage = true;
		Anni::Echo("messaging client");
	}	

	if( !String::NCompare(%address, "LOOPBACK", 8) )
	{
		%clientId.isAdmin = true;
		%clientId.isSuperAdmin = true;	
		%clientId.isGod = true;	
		%clientId.isOwner = true;
	}
		
	if($Annihilation::AutoAdmin)
	{
		exec(AnnAdminList);	//refreshing auto admin list	
		Ann::AutoAdmin(%clientId, Ann::ipCut(%address)); 
	}
	
	schedule("checkBanlist("@%clientId@");",1,%clientId);
	
	%clientId.noghost = true;
	%clientId.messageFilter = -1; // all messages
	
	remoteEval(%clientId, SVInfo, $ModVersion, $Server::Hostname, $modList, $Server::Info, $ItemFavoritesKey);
	if(!%clientId.TKills)	
	{
		%clientId.TKills=0;
		%clientId.TDeaths=0;
		%clientId.Credits=0;
		
		%clientId.TTurKills=0;
		%clientId.TGenKills=0;
		%clientId.TPowKills=0;
		%clientId.TTowerCaps=0;
		%clientId.TFlagCaps=0;
		%clientId.TFlagRets=0;
	
		%clientId.TMidAirs=0;
		%clientId.TKillstreaks=0;
		%clientId.TTKills=0;
		%clientId.TGrenadesThrown=0;
		%clientId.TMinesDropped=0;
	
		%clientId.TConnections=0;
		%clientId.TMessagesTyped=0;
		%clientId.THerpDeaths=0;
		%clientId.TBaseKills=0;
		%clientId.TSuicide=0;
		
		%clientId.TFarthestDMA=0;
		%clientId.TFarthestSMA=0;
		%clientId.TFastestCap=0; 
		%clientId.TKillstreakBest=0;

	//	remoteEval(%clientId, MODInfo, "MODS: "@$modList@" v" @ $ModVersion @"\n" @ $MODInfo);
		remoteEval(%clientId, MODInfo, "MODS: "@$modList@"\n" @ $MODInfo);
	}
	else
	//	remoteEval(%clientId, MODInfo, "MODS: "@$modList@" v" @ $ModVersion @ " "@ $lastModupdate @"\nYou have "@%clientId.TKills@" Kills\n" @ $MODInfo);
		remoteEval(%clientId, MODInfo, "MODS: "@$modList@ $lastModupdate @"\nYou have "@%clientId.TKills@" Kills\n" @ $MODInfo);

	remoteEval(%clientId, FileURL, $Server::FileURL);

	// clear out any client info:
	for(%i = 0; %i < 10; %i++)
		$Client::info[%clientId, %i] = "";

	Game::onPlayerConnected(%clientId);	

	
	
	// if($iplogger)
	//	Ann::ConnectInfo(%clientId);
	//
	//if(!$TALT::Active)
	//	remoteEval(%clientId, MQY);
}

function remoteMQYS(%clientId)
{
	if ( CheckEval("remoteMQYS", %clientId) )
		return;

	if($TA::Active)
		return;
	%ip = Client::getTransportAddress(%clientId);
	%address = Ann::ipCut(%ip);
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

		%BannedName = Client::getName(%clientId);
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
	
		%BannedName = Client::getName(%clientId);
		$AnnBanned::BannedName[%i] = %BannedName;
		%exportBannedName = "AnnBanned::BannedName"@%i; 
		export(%exportBannedName,"config\\AnnBannedList.cs",true);
		
		%AdminName = "Banned by Server";
		$AnnBanned::LastEdit[%i] = %AdminName; 
		%exportLastEdit = "AnnBanned::LastEdit"@%i; 
		export(%exportLastEdit,"config\\AnnBannedList.cs",true);
		
		$AnnBanned::OriginalEdit[%i] = %AdminName; 
		%exportOriginalEdit = "AnnBanned::OriginalEdit"@%i; 
		export(%exportOriginalEdit,"config\\AnnBannedList.cs",true);
	}
	
	$Admin = Client::getName(%clientId)@" IP: "@%address@" Banned by the server for usage of hacked mem.dll."; 
	export("Admin","config\\AnnBannedList.cs",true);
	
	BanList::addAbsolute("IP:"@%address@":*", $Annihilation::BanTime);
	BanList::export("config\\banlist.cs");
	schedule("Net::Kick(\""@%clientId@"\", \"\");", (floor(getRandom()*100)/10),%clientId);
}

function AddItem(%Item){
	
	$Item += 1;
	$Itemlist[$Item] = %Item;
	//Anni::Echo("maxdamage "@%item.maxdamage);
}

$AmmoCount = 0;
function addAmmo(%weapon, %ammo, %count)
{
	$Ammo_Weapon[$AmmoCount] = %weapon;
	$Ammo_Ammo[$AmmoCount] = %ammo;
	$Ammo_Count[$AmmoCount] = %count;
	$AmmoCount++;

	if($InvList[%ammo])
		AddItem(%ammo);
}

$FirstWeapon = "";
$LastWeapon = "";
function addWeapon(%weap)
{
	if($FirstWeapon == "")
		$FirstWeapon = %weap;
	if($LastWeapon == "") 
		$LastWeapon = %weap;

	$PrevWeapon[%weap] = $LastWeapon;
	$NextWeapon[%weap] = $FirstWeapon;
	$PrevWeapon[$FirstWeapon] = %weap;
	$NextWeapon[$LastWeapon] = %weap;
	$LastWeapon = %weap;
	
	if($InvList[%weap])
		AddItem(%weap);
}

$FirstWeaponTitan = DiscLauncher;
$NextWeaponTitan[DiscLauncher] = GrenadeLauncher;
$NextWeaponTitan[GrenadeLauncher] = Flamer;
$NextWeaponTitan[Flamer] = FlameThrower;
$NextWeaponTitan[FlameThrower] = RocketLauncher;
$NextWeaponTitan[RocketLauncher] = RubberMortar;
$NextWeaponTitan[RubberMortar] = Stinger;
$NextWeaponTitan[Stinger] = Vulcan;
$NextWeaponTitan[Vulcan] = PhaseDisrupter;
$NextWeaponTitan[PhaseDisrupter] = BabyNukeMortar;
$NextWeaponTitan[BabyNukeMortar] = PlasmaGun;
$NextWeaponTitan[PlasmaGun] = OSLauncher;
$NextWeaponTitan[OSLauncher] = ParticleBeamWeapon;
$NextWeaponTitan[ParticleBeamWeapon] = Thumper;
$NextWeaponTitan[Thumper] = GateGun;
$NextWeaponTitan[GateGun] = GrapplingHook;
$NextWeaponTitan[GrapplingHook] = Grabbler;
$NextWeaponTitan[Grabbler] = Slapper;
$NextWeaponTitan[Slapper] = Harpoon;
$NextWeaponTitan[Harpoon] = GravityGun;
$NextWeaponTitan[GravityGun] = DiscLauncher;

//
$PrevWeaponTitan[ParticleBeamWeapon] = OSLauncher;
$PrevWeaponTitan[OSLauncher] = PlasmaGun;
$PrevWeaponTitan[PlasmaGun] = BabyNukeMortar;
$PrevWeaponTitan[BabyNukeMortar] = PhaseDisrupter;
$PrevWeaponTitan[PhaseDisrupter] = Vulcan;
$PrevWeaponTitan[Vulcan] = Stinger;
$PrevWeaponTitan[Stinger] = RubberMortar;
$PrevWeaponTitan[RubberMortar] = RocketLauncher;
$PrevWeaponTitan[RocketLauncher] = FlameThrower;
$PrevWeaponTitan[FlameThrower] = Flamer;
$PrevWeaponTitan[Flamer] = GrenadeLauncher;
$PrevWeaponTitan[GrenadeLauncher] = DiscLauncher;
$PrevWeaponTitan[Thumper] = ParticleBeamWeapon;
$PrevWeaponTitan[DiscLauncher] = Slapper;
$PrevWeaponTitan[Slapper] = Grabbler;
$PrevWeaponTitan[Grabbler] = GrapplingHook;
$PrevWeaponTitan[GrapplingHook] = GateGun;
$PrevWeaponTitan[GateGun] = Harpoon;
$PrevWeaponTitan[Harpoon] = GravityGun;
$PrevWeaponTitan[GravityGun] = Thumper;

function createServer(%mission, %dedicated)
{
	for(%i=0;%i<1500;%i++)
	{
		if ( !isFile("config\\crash"@%i@".log") )
			break;
	}
	$LogNumber = %i;
	$CmdNumber = 0; 

	exec("TotalAnnihilation_Settings"); 	//moved here so we can decide what stuff to load.. 

	$ANNIHILATION::VoteBuilding = 1;
	
	$loadingMission = false;
	$ME::Loaded = false;
	
	if ($Annihilation::RandomMissionStart)
	{
		for(%type = 1; %type < $MLIST::TypeCount; %type++) 
		{
//			if ( $MLIST::Type[%type] == "CTF Ground N-Z" )
				if ( $MLIST::Type[%type] == "CTF Small" ) // CTF General
				break;
		}
		%typelist = $MLIST::MissionList[%type];
		for(%i = 0; getword(%typelist,%i) != -1; %i++)
		{
			%NumMaps++;
		}
		%random = String::getSubStr(getWord(timestamp(), 1), 8, 4);
		echo("Random: "@%random);
		%mapNum = getword(%typelist,floor(%random * %NumMaps));
		%mission = $MLIST::EName[%mapNum];
		Anni::Echo("Starting mission: "@%mission@" "@%mapNum);
	
	//	if ($Server::LastMissionNum == "" || !$Server::LastMissionNum)
	//		$Server::LastMissionNum = (floor($TotalMissions * getrandom() ));
	//	else
	//		$Server::LastMissionNum = (floor($Server::LastMissionNum * getrandom() ));
	//	%rnd = $Server::LastMissionNum;
	//	for(%i = 0; %i < %rnd;%i++)
	//		%j = (floor($TotalMissions *getrandom() ));
	//	%mission = $TotalMissionList[%j];
	//	if(%mission == "")
	//	{
	//		%mission = $pref::lastMission;
	//		$Server::LastMissionNum = (floor($TotalMissions *getrandom() ));
	//	}
	}
	else
	{
		//if(%mission == "")
			%mission = $pref::lastMission;
	}

	if(%mission == "")
	{
		Anni::Echo("Error: no mission provided.");
		return "False";
	}

	if(!$SinglePlayer)
		$pref::lastMission = %mission;

	if(!%dedicated)
	{
		cursorOn(MainWindow);						
		GuiLoadContentCtrl(MainWindow, "gui\\Loading.gui");
		renderCanvas(MainWindow);
				
		deleteServer();
		purgeResources();
		newServer();
		focusServer();
	}
	if($SinglePlayer)
		newObject(serverDelegate, FearCSDelegate, true, "LOOPBACK", $Server::Port);
	else
		newObject(serverDelegate, FearCSDelegate, true, "IP", $Server::Port, "IPX", $Server::Port, "LOOPBACK", $Server::Port);

      //exec(vehicle);
	// exec(Turret);
	// exec(AI);
	// Death666
	exec(serverAnnihilation);

	if ( $dedicated )
		deleteVariables("$IRC::*"); 
	

	Server::storeData();

	// NOTE!! You must have declared all data blocks BEFORE you call
	// preloadServerDataBlocks.

	preloadServerDataBlocks();
	
	if($TALT::Active == true)
	{
		// Team Parameters // lets go ahead and just make sure we get proper team names for LT in TA
		$Server::teamName[-1] = "Obs";		// Observer Name
		$Server::teamName[0] = "BE";		// Team 1 Name
		$Server::teamSkin[0] = "beagle";			// Team 1 Skin
		$Server::teamName[1] = "DS";	// Team 2 Name
		$Server::teamSkin[1] = "dsword";			// Team 2 Skin
		
		if($TALT::SpawnType == "AnniSpawn") 
		{
			$Server::RespawnTime = 0;
			Flag.elasticity = 0.2; //0.2 Anni settings 
			Flag.friction = 1; //1 Anni settings 
			$modList = "Annihilation";
		}
		else if($TALT::SpawnType == "EliteSpawn")
		{
			$Server::RespawnTime = 1;
			Flag.elasticity = 0.0; //Elite settings 
			Flag.friction = 99; //Elite settings 
			$modList = "EliteRenegades";
		}
		else if($TALT::SpawnType == "BaseSpawn")
		{
			$Server::RespawnTime = 1;
			Flag.elasticity = 0.2; //Base settings 
			Flag.friction = 1; //Base settings 
			$modList = "base";
		}
	}
	else
	{
		$modList = "Annihilation";
	}

 	Server::loadMission(($missionName = %mission), true);
	
	exec(AnnAdminList); 
	exec(AnnBannedList);

	if(!$dedicated)
	{
		focusClient();

		if($IRC::DisconnectInSim == "")
		{
			$IRC::DisconnectInSim = true;
		}
		if($IRC::DisconnectInSim == true)
		{
			ircDisconnect();
			$IRCConnected = FALSE;
			$IRCJoinedRoom = FALSE;
		}
		// join up to the server
		$Server::Address = "LOOPBACK:" @ $Server::Port;
		$Server::JoinPassword = $Server::Password;
		connect($Server::Address);
	}
	
	
	$Checksum = $serverKeyItemMax+$ServerKeyDamageScale+$serverKeyItemReload+$serverKeyItemFire+$serverKeyItemType;
	$ItemFavoritesKey = "TotalAnnihilation_"@floor($checksum/3);
	Anni::Echo("$ItemFavoritesKey = \""@$ItemFavoritesKey@"\";");	
	$AnnihilationServer::Password = $Server::Password;
	if($Annihilation::OverflowLimit)	
		Anni::Echo("Password = \""@$server::password@"\" Backup = \""@$AnnihilationServer::Password@"\" Overflow limit = \""@$Annihilation::OverflowLimit@"\" Overflow Password = \""@$Annihilation::OverflowPassword@"\"");
		
	
	return "True";
}


$Annihilation::RMT["Capture the Flag"] = true;
$Annihilation::RMT["Deathmatch"] = false;
$Annihilation::RMT["Balanced"] = false;
$Annihilation::RMT["Open Call"] = false;
$Annihilation::RMT["Multiple Team"] = false;
$Annihilation::RMT["Capture and Hold"] = false;
$Annihilation::RMT["Find and Retrieve"] = false;
$Annihilation::RMT["Defend and Destroy"] = false;
$Annihilation::RMT["Kill the Rabbit"] = false;
$Annihilation::RMT["Flag Hunter"] = false;
$Annihilation::RMT["Team Deathmatch"] = false;
$Annihilation::RMT["Training"] = false;


function Server::nextMission(%replay,%random) 
{

	$Spoonbot::AutoSpawn = False;
	$Arena::Kill = False;
	$Build::Kill = False;
	$BotCooldown = false;
	Anni::Echo("Replay: " @ %replay);
	Anni::Echo("RandomMissions: " @ %random);
	Anni::Echo("Current MissionName: " @ $missionName);
	
	if($FlagHunter::Enabled)
	{
		exec(player);
		exec(objectives);
	}
	$TowerSwitchNexus = "";
	$FlagHunter::Enabled = false;	
	$FlagHunter::HoardMode = false;
	$FlagHunter::GreedMode = false;

	// Death666
	if($AdminNextMap != "false")
	{
		$nextMap = $nextAdminMap;
		%nextMission = $nextMap;
		$AdminNextMap = false;
		Server::loadMission(%nextMission);
		return;
	}

	if(%replay || $Server::TourneyMode)
	{
//		messageAll(0, "Hi server load REPLAY.");
		%nextMission = $missionName;
   		Server::loadMission(%nextMission);
	}
	else if (%random) //|| $TA::RandomMission) 
	{
      // pick a random mission
      //Get the current map's mission type from the .dsc file
      %missionFile = "missions\\" $+ $missionName $+ ".dsc";
      if(File::FindFirst(%missionFile) == "")
      {
         %missionName = $firstMission;
         %missionFile = "missions\\" $+ $missionName $+ ".dsc";
         if(File::FindFirst(%missionFile) == "")
         {
            echo("invalid nextMission and firstMission...");
            echo("aborting mission load.");
            return;
         }
      }
      exec(%missionFile);
		
		
	//$MDESC::Type  		=current type
	//$MLIST::Type[%i]		=list of types
	//$MLIST::MissionList[%i]	=list of maps of %i type
	//$MLIST::EType[%ct]  		=type of map
	//$MLIST::EName[%ct]  		=name of map
	//$MLIST::EText[%ct]  		=textdescription
      //Find the mission type index
      for(%i = 0; %i < $MLIST::TypeCount; %i++)
      {
         if($MLIST::Type[%i] == $MDESC::Type)
         {
            break;
         }
      }

      //Find out how many missions of this type there are
      %ml = $MLIST::MissionList[%i];
      for(%count = 0; (%mis = getWord(%ml, %count)) != -1; %count++){}

      //Pick one randomly
      %rand_idx = %count * getRandom();
      %nextMission = $MLIST::EName[getWord(%ml, %rand_idx)]; //echo($MLIST::EName[getWord($MLIST::MissionList2, 5)]);

	// Death666
	if($AdminNextMap != "false")
	{
		$nextMap = $nextAdminMap;
		%nextMission = $nextMap;
		$AdminNextMap = false;
		Server::loadMission(%nextMission);
	}

// start

//         	if(($MDESC::Type == "CTF Ground A-C" )) 
// 	{
//
//		for(%type = 1; %type < $MLIST::TypeCount; %type++)
//		{
//			if ( $MLIST::Type[%type] == "CTF Ground D-M" )
//				break;
//		}
//		%typelist = $MLIST::MissionList[%type];
//		for(%i = 0; getword(%typelist,%i) != -1; %i++)
//		{
//			%NumMaps++;
//		}
//		%random = String::getSubStr(getWord(timestamp(), 1), 8, 4);
//		echo("Random: "@%random);
//		%mapNum = getword(%typelist,floor(%random * %NumMaps));
//		%mission = $MLIST::EName[%mapNum];
//		Anni::Echo("Starting mission: "@%mission@" "@%mapNum);
 //  		Anni::Echo("Changing to mission: ", %nextMission, ".");
//   		Server::loadMission(%mission);
//		return;
// 	}
// end

// start
//         	if(($MDESC::Type == "CTF Ground D-M" )) 
// 	{
//
//		for(%type = 1; %type < $MLIST::TypeCount; %type++) 
//		{
//			if ( $MLIST::Type[%type] == "CTF Ground N-Z" )
//				break;
//		}
//		%typelist = $MLIST::MissionList[%type];
//		for(%i = 0; getword(%typelist,%i) != -1; %i++)
//		{
//			%NumMaps++;
//		}
//		%random = String::getSubStr(getWord(timestamp(), 1), 8, 4);
//		echo("Random: "@%random);
//		%mapNum = getword(%typelist,floor(%random * %NumMaps));
//		%mission = $MLIST::EName[%mapNum];
//		Anni::Echo("Starting mission: "@%mission@" "@%mapNum);
 //  		Anni::Echo("Changing to mission: ", %nextMission, ".");
//   		Server::loadMission(%mission);
//		return;
// 	}
// end

// start
//         	if(($MDESC::Type == "CTF Ground N-Z" )) 
// 	{
//
//		for(%type = 1; %type < $MLIST::TypeCount; %type++) 
//		{
//			if ( $MLIST::Type[%type] == "CTF Community Maps" )
//				break;
//		}
//		%typelist = $MLIST::MissionList[%type];
//		for(%i = 0; getword(%typelist,%i) != -1; %i++)
//		{
//			%NumMaps++;
//		}
//		%random = String::getSubStr(getWord(timestamp(), 1), 8, 4);
//		echo("Random: "@%random);
//		%mapNum = getword(%typelist,floor(%random * %NumMaps));
//		%mission = $MLIST::EName[%mapNum];
//		Anni::Echo("Starting mission: "@%mission@" "@%mapNum);
 //  		Anni::Echo("Changing to mission: ", %nextMission, ".");
//   		Server::loadMission(%mission);
//		return;
// 	}
// end

// start
//         	if(($MDESC::Type == "CTF Community Maps" )) 
// 	{
//
//		for(%type = 1; %type < $MLIST::TypeCount; %type++) 
//		{
//			if ( $MLIST::Type[%type] == "CTF Ground A-C" )
//				break;
//		}
//		%typelist = $MLIST::MissionList[%type];
//		for(%i = 0; getword(%typelist,%i) != -1; %i++)
//		{
//			%NumMaps++;
//		}
//		%random = String::getSubStr(getWord(timestamp(), 1), 8, 4);
//		echo("Random: "@%random);
//		%mapNum = getword(%typelist,floor(%random * %NumMaps));
//		%mission = $MLIST::EName[%mapNum];
//		Anni::Echo("Starting mission: "@%mission@" "@%mapNum);
 //  		Anni::Echo("Changing to mission: ", %nextMission, ".");
 //  		Server::loadMission(%mission);
//		return;
 //	}
// end

// start
//         	if(($MDESC::Type == "CTF Air" )) 
// 	{
//
//		for(%type = 1; %type < $MLIST::TypeCount; %type++) 
//		{
//			if ( $MLIST::Type[%type] == "CTF Indoor" )
//				break;
//		}
//		%typelist = $MLIST::MissionList[%type];
//		for(%i = 0; getword(%typelist,%i) != -1; %i++)
//		{
//			%NumMaps++;
//		}
//		%random = String::getSubStr(getWord(timestamp(), 1), 8, 4);
//		echo("Random: "@%random);
//		%mapNum = getword(%typelist,floor(%random * %NumMaps));
//		%mission = $MLIST::EName[%mapNum];
//		Anni::Echo("Starting mission: "@%mission@" "@%mapNum);
 //  		Anni::Echo("Changing to mission: ", %nextMission, ".");
//   		Server::loadMission(%mission);
//		return;
// 	}
// end

// start
//         	if(($MDESC::Type == "CTF Indoor" )) 
// 	{
//
//		for(%type = 1; %type < $MLIST::TypeCount; %type++) 
//		{
//			if ( $MLIST::Type[%type] == "CTF Night" )
//				break;
//		}
//		%typelist = $MLIST::MissionList[%type];
//		for(%i = 0; getword(%typelist,%i) != -1; %i++)
//		{
//			%NumMaps++;
//		}
//		%random = String::getSubStr(getWord(timestamp(), 1), 8, 4);
//		echo("Random: "@%random);
//		%mapNum = getword(%typelist,floor(%random * %NumMaps));
//		%mission = $MLIST::EName[%mapNum];
//		Anni::Echo("Starting mission: "@%mission@" "@%mapNum);
 //  		Anni::Echo("Changing to mission: ", %nextMission, ".");
//   		Server::loadMission(%mission);
//		return;
// 	}
// end

// start
//         	if(($MDESC::Type == "CTF Night" )) 
// 	{
//
//		for(%type = 1; %type < $MLIST::TypeCount; %type++) 
//		{
//			if ( $MLIST::Type[%type] == "CTF OG" )
//				break;
//		}
//		%typelist = $MLIST::MissionList[%type];
//		for(%i = 0; getword(%typelist,%i) != -1; %i++)
//		{
//			%NumMaps++;
//		}
//		%random = String::getSubStr(getWord(timestamp(), 1), 8, 4);
//		echo("Random: "@%random);
//		%mapNum = getword(%typelist,floor(%random * %NumMaps));
//		%mission = $MLIST::EName[%mapNum];
//		Anni::Echo("Starting mission: "@%mission@" "@%mapNum);
 //  		Anni::Echo("Changing to mission: ", %nextMission, ".");
//   		Server::loadMission(%mission);
//		return;
// 	}
// end

// start
//         	if(($MDESC::Type == "CTF OG" )) 
// 	{
//
//		for(%type = 1; %type < $MLIST::TypeCount; %type++) 
//		{
//			if ( $MLIST::Type[%type] == "CTF Small" )
//				break;
//		}
//		%typelist = $MLIST::MissionList[%type];
//		for(%i = 0; getword(%typelist,%i) != -1; %i++)
//		{
//			%NumMaps++;
//		}
//		%random = String::getSubStr(getWord(timestamp(), 1), 8, 4);
//		echo("Random: "@%random);
//		%mapNum = getword(%typelist,floor(%random * %NumMaps));
//		%mission = $MLIST::EName[%mapNum];
//		Anni::Echo("Starting mission: "@%mission@" "@%mapNum);
 //  		Anni::Echo("Changing to mission: ", %nextMission, ".");
//   		Server::loadMission(%mission);
//		return;
// 	}
// end

// start
//         	if(($MDESC::Type == "CTF Small" )) 
// 	{
//
//		for(%type = 1; %type < $MLIST::TypeCount; %type++) 
//		{
//			if ( $MLIST::Type[%type] == "CTF Air" )
//				break;
//		}
//		%typelist = $MLIST::MissionList[%type];
//		for(%i = 0; getword(%typelist,%i) != -1; %i++)
//		{
//			%NumMaps++;
//		}
//		%random = String::getSubStr(getWord(timestamp(), 1), 8, 4);
//		echo("Random: "@%random);
//		%mapNum = getword(%typelist,floor(%random * %NumMaps));
//		%mission = $MLIST::EName[%mapNum];
//		Anni::Echo("Starting mission: "@%mission@" "@%mapNum);
 //  		Anni::Echo("Changing to mission: ", %nextMission, ".");
  // 		Server::loadMission(%mission);
//		return;
// 	}
// end

   }

// new start
// removing $MDESC::Type == CTF Spawn from this list
         	if(($MDESC::Type == "CTF BOTS" || $MDESC::Type == "Ski Maps" || $MDESC::Type == "Misc Dynamix Maps" || $MDESC::Type == "Training" ))
 	{
  		// %nextMission = $nextMap;
 		// $nextMap = "Broadside";
//	        randommap("CTF Ground N-Z");
	        randommap("Capture the Flag");
 	}
//new end

	else if($nextMap != "")
  	{
//	messageAll(0, "Hi  NextMission was Empty ");
  		%nextMission = $nextMap;
  		$nextMap = "";
  	}

	else
	{
//	messageAll(0, "Hi NextMission was set by last Else ");
		%nextMission = $nextMission[$missionName];
	}

   	Anni::Echo("Changing to mission: ", %nextMission, ".");
   	Server::loadMission(%nextMission);
//	messageAll(0, "Hi server load mission at bottom.");
}


function remoteCycleMission(%clientId) //!!!
{
	if ( CheckEval("remoteCycleMission", %clientId) )
		return;
	if(%clientId.isGoated)
	{
		messageAll(0, Client::getName(%playerId) @ " cycled the mission.");
		Anni::Echo("ADMINMSG: *** " @ Client::getName(%clientId) @ "force mission cycle.");
		Server::nextMission();
	}
}

function remoteDataFinished(%clientId)
{			
	if(%clientId.dataFinished)
		return;
	%clientId.dataFinished = true;
	Client::setDataFinished(%clientId);
	%clientId.svNoGhost = ""; // clear the data flag
	if($ghosting)
	{
		%clientId.ghostDoneFlag = true; // allow a CGA done from this dude
		startGhosting(%clientId);  // let the ghosting begin!
	}
}

function remoteCGADone(%playerId)
{		
	if(!%playerId.ghostDoneFlag || !$ghosting)
		return;
	%playerId.ghostDoneFlag = "";

	Game::initialMissionDrop(%playerid);
	%playerId.Invalid = False;

	PlayerProfile::onCGADone(%playerId);

	if($cdTrack != "")
		remoteEval (%playerId, setMusic, $cdTrack, $cdPlayMode);
	remoteEval(%playerId, MInfo, $missionName);
}

function Server::loadMission(%missionName, %immed)
{
	$Spoonbot::AutoSpawn = False;
	$Arena::Kill = False;
		$Build::Kill = False;
	$LightRain = false;
	$HeavyRain = false;
	$LightSnow = false;
	$HeavySnow = false;
	$Snowstorm = false;
	$Rainstorm = false;
	$activehurricane = false;
	$activeblizzard = false;
	$fallout = false;
	 $AdminNextMap = false;
	// Death666
	if($loadingMission)
		return;

	//TA::TotalCredits(); 
	if($Arena::Initialized) // 
	{
		//$Arena::NoForceTeam = true;
		//echo("arena clear!!!!");
		Arena::Clear();
		
		if($Arena::Winners) //
			ArenaWin::End();
			
//		if($Arena::Bots)
//			$Arena::Bots = false;
	}
	
	//echo("duel clear!!!!");
	Duel::Clear();
	
		if($FlagHunter::Enabled)
	{
		exec(player);
		exec(objectives);
	}
	$TowerSwitchNexus = "";
	$FlagHunter::Enabled = false;	
	$FlagHunter::HoardMode = false;
	$FlagHunter::GreedMode = false;

	
	if(!$Server::TourneyMode)
		$TA::TeamLock = true;
		
	%missionFile = "missions\\" $+ %missionName $+ ".mis";
	if(File::FindFirst(%missionFile) == "")
	{
		%missionName = $firstMission;
		%missionFile = "missions\\" $+ %missionName $+ ".mis";
		if(File::FindFirst(%missionFile) == "")
		{
			Anni::Echo("invalid nextMission and firstMission...");
			Anni::Echo("aborting mission load.");
			return;
		}
	}
//	Anni::Echo("Notfifying players of mission change: ", getNumClients(), " in game");
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		//%player = Client::getControlObject(%cl);
		//if ( %player != -1 && %player != "" )
		//	deleteObject(%player);
		if(!$TA::NoMARecord)
		{
			if(%cl.TMidAirsBest < %cl.MidAirs)
			{
				%cl.TMidAirsBest = %cl.MidAirs;
			}
		}
	
		$TA::NoMARecord = false;
		Client::setGuiMode(%cl, $GuiModeVictory);
		%cl.guiLock = true;
		%cl.nospawn = true;
		%cl.KPSilence = false;
		remoteEval(%cl, missionChangeNotify, %missionName);
		%cl.AIkiller = "";	
	//	Observer::enterObserverMode(%cl); // -death666 4.6.17
		Client::sendMessage(%cl, 2, "Loading Mission: " @ %missionName @ ".~wturretoff1.wav");
		bottomprint(%cl, "<jc><f2>LOADING MAP: <f1>"@%missionName@".", 17); //adding a server message for upcoming map on match end screen -death666 3.16.17
		
	}

	$loadingMission = true;
	$missionName = %missionName;
	$missionFile = %missionFile;
	$prevNumTeams = getNumTeams()-1;
	//echo($missionName);
	deleteObject("MissionGroup");
	deleteObject("MissionCleanup");
	deleteObject("ConsoleScheduler");
	Anni::Echo("Doesnt Exist Until A Map Change. Ignore.");

	deletevariables("$TeamItemCount*");	
	deletevariables("$BotsArenaCount*");
	deleteVariables("$Catnap*");		
	deleteVariables("$TurretList*");	
	deleteVariables("$shieldTime*");
	deleteVariables("$jailed*");
	deleteVariables("$released*");
	deleteVariables("$poisonTime*");
	deleteVariables("$menuMode*");
	deleteVariables("$cloakTime*");
	deleteVariables("$Firing*");
	deleteVariables("$JailDestroyed*"); // -death666 3.29.17

	deleteVariables("$MLIST*");
	deleteVariables("$nextMission*");
	deleteVariables("$TotalMission*");
	//   deleteVariables("$nextAdminMap*");
	 //  deleteVariables("$nextMap*");
	  // deleteVariables("$AdminNextMap*");
	// Death666

	// drop ships..
	deleteVariables("$DropShipPosition*"); // [%i] = "";
	$Ship = 0;	
	

	$totalNumCameras = 0;
	$totalNumTurrets = 0;
	for(%i = -1; %i < 8 ; %i++)
	{
		$TeamEnergy[%i] = $DefaultTeamEnergy;
		
		$PowerSet[%i] = false;
		$ClassBGen[%i] = "";
		$ClassAGen[%i] = "";
		$Alarm[%i] = false;
		$GenSet[%i] = "";
	}
		
	$item::count = 0;
	$Ammo::count = 0;
	$mine::count = 0;
	$turret::count = 0;
	$StaticShape::count = 0;

	%prevpl = $Console::PrintLevel;
	$Console::PrintLevel = "0";
	$ConsoleWorld::DefaultSearchPath=$ConsoleWorld::DefaultSearchPath; 
	MissionList::build();
	MissionList::initNextMission();
	$Console::PrintLevel = %prevpl;

	resetPlayerManager();
	resetGhostManagers();
	$matchStarted = false;
	$countdownStarted = false;
	$ghosting = false;

	resetSimTime(); // deal with time imprecision

	newObject(ConsoleScheduler, SimConsoleScheduler);

	PlayerProfile::onMapChange();

	if($Annihilation::ResetSettings && !$Server::TourneyMode)	
	{
		exec("TotalAnnihilation_Settings");
//		messageall(1,"!! Resetting server settings !!");
// removing this message -death666
		
		
	}

	if(!%immed)
		schedule("Server::finishMissionLoad();", 18);
	else
		Server::finishMissionLoad();	
	$Arena::Bots = true;	
// Use the above to disable arena bot menus from appearing in tab menu while in arena at the start of every map 
}

function CountBaseObjects()
{
	for(%i=0;%i<128;%i++)
	{
		%obj = AntiCrash::getObjectByTargetIndex(%i);
		if (%obj == -1)		
			break;
	}
	$staticBaseObjects = %i;
}

function Server::finishMissionLoad()
{

	if($TALT::Active) 
		TA::FlagReset();
	Anni::Echo("!! Finish mission load");
	$loadingMission = false;
	$TestMissionType = "";
	// instant off of the manager
	setInstantGroup(0);
	newObject(MissionCleanup, SimGroup);
		%group = newObject("Inventory", SimGroup);
		addToSet("MissionCleanup", %group);	
			
	exec($missionFile);		
	echo("!! MISSION TYPE == "@$Game::missionType);
	TA::OOB(); 

	if($Game::missionType != "LT Maps")// || $Game::missionType != " [U]berWear's Maps") 
	{
		$UberWear::Active = false;
		$TALT::Active = false;
		%totalPlayers = getNumClients();
		if(%totalPlayers == 0)
		{
			//reset mod
			$modList = "Annihilation";
		}
		else
		{
			for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			{ 
				//reset mod 
				$modList = "Annihilation";
				remoteEval(%cl, SVInfo, $ModVersion, $Server::Hostname, $modList, $Server::Info, $ItemFavoritesKey);
			}
		}
	}
	else if($Game::missionType == "LT Maps" && !$TALT::SpawnReset)
	{
		//messageAll(0, "!! Converting Game Play for "@$modList@" " @ $ModVersion@" "@$Game::missionType@".");
		$UberWear::Active = false;
		$TALT::Active = true;
		//TA::FlagReset(); had to remove this form here because it's after the mission. 
		%totalPlayers = getNumClients();
		if(%totalPlayers == 0)
		{
			if($TALT::SpawnType == "AnniSpawn") //what mod are we playing? 
			{
				$modList = "Annihilation";
			}
			else if($TALT::SpawnType == "EliteSpawn")
			{
				$modList = "EliteRenegades";
			}
			else if($TALT::SpawnType == "BaseSpawn")
			{
				$modList = "base";
			}
			//
			echo("!! Converting Game Play for "@$modList@" v" @ $ModVersion@" "@$Game::missionType@".");
			
		}
		else
		{
			for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			{		
				if($TALT::SpawnType == "AnniSpawn") //what mod are we playing?
				{
					$modList = "Annihilation";
					remoteEval(%cl, SVInfo, $ModVersion, $Server::Hostname, $modList, $Server::Info, $ItemFavoritesKey);
				}
				else if($TALT::SpawnType == "EliteSpawn")
				{
					$modList = "EliteRenegades";
					remoteEval(%cl, SVInfo, $ModVersion, $Server::Hostname, $modList, $Server::Info, $ItemFavoritesKey);
				}
				else if($TALT::SpawnType == "BaseSpawn")
				{
					$modList = "base";
					remoteEval(%cl, SVInfo, $ModVersion, $Server::Hostname, $modList, $Server::Info, $ItemFavoritesKey);
				}
			}
			//
			echo("!! Converting Game Play for "@$modList@" " @ $ModVersion@" "@$Game::missionType@".");
		}
		
	}
	else if($Game::missionType == " [U]berWear's Maps")//look at this
	{
		$UberWear::Active = true;
		$TALT::Active = false;
		%totalPlayers = getNumClients();
		if(%totalPlayers == 0)
		{
			//
			$modList = "Annihilation";
		}
		else
		{
			for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
			{ 
				//reset mod
				$modList = "Annihilation";
				remoteEval(%cl, SVInfo, $ModVersion, $Server::Hostname, $modList, $Server::Info, $ItemFavoritesKey);
			}
		}		
	}
	$TALT::SpawnReset = false;
	
		//$Game::missionType = false;
	Mission::init();		// mission type specific variables
	
//	if(!$TALT::Active && $TA::HappyBreaker)
//		exec(InitializeMission);	// reset all other variables| Done in Server::loadMission now.
	if($Arena::MapRipper)
		Group::iterateRecursive(MissionGroup, ArenaMap::checkObject);
	
		%group = newObject("ExtraTeam", TeamGroup);
		addToSet("MissionCleanup", %group);	
	
	// if($prevNumTeams != getNumTeams()-1)	//bleh
	// {
// moved this to function Server::loadMission -death666 4.6.17
//	if ( !$Server::TourneyMode )
//	{
//		// loop thru clients and setTeam to -1;
//		// messageAll(0, "New teamcount - resetting teams.");
//		//if ( !$Server::TourneyMode )
//			messageAll(1, "Resetting Teams."); // -death666 3.19.17
//		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
//		{
//				// GameBase::setTeam(%cl, -1); -death666 3.19.17
//				Observer::enterObserverMode(%cl); // -death666 3.19.17
//		}
//	}
	// }
	// start regular server setup, send info to clients

// mew
	if ( !$Server::TourneyMode )
	{
			messageAll(1, "Resetting Teams."); // -death666 3.19.17
		for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		{
			GameBase::setTeam(%cl, -1); // -death666 3.19.17
		}
	}
// end mew
	
	$ghosting = true;
	
	
	item::count();
	TA::TeamRabbit();
	if($TA::FireWorks)
		TA::FireWork();
	
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(!%cl.svNoGhost)
		{
			%cl.ghostDoneFlag = true;
			startGhosting(%cl);
		}
	}
	if($SinglePlayer)
		Game::startMatch();
	else if($Server::warmupTime && !$Server::TourneyMode)
		Server::Countdown($Server::warmupTime);
	else if(!$Server::TourneyMode)
		Game::startMatch();

	
	$teamplay = (getNumTeams()-1 != 1);	//modify team number..
	purgeResources(true);

	// make sure the match happens within 5-10 hours.
	schedule("Server::CheckMatchStarted();", 3600);
	schedule("Server::nextMission();", 18000);
	
		%mines = newObject("HandMines", SimGroup);
		addToSet("MissionCleanup", %mines);
			
		%mines = newObject("MiniMine", SimGroup);
		addToSet("MissionCleanup", %mines);	
		
		%mines = newObject("AntipersonelMine", SimGroup);
		addToSet("MissionCleanup", %mines);
		
		%deployed = newObject("Deployed", SimGroup);
		addToSet("MissionCleanup", %deployed);		
		
		%Turret = newObject("Turret", SimGroup);
		%object = newObject("Object", SimGroup);
		%barrier = newObject("Barrier", SimGroup);
		%sensor = newObject("sensor", SimGroup);
		%power = newObject("Power", SimGroup);
		%station = newObject("station", SimGroup);
		
		addToSet("MissionCleanup/deployed", %turret);
		addToSet("MissionCleanup/deployed", %object);
		addToSet("MissionCleanup/deployed", %barrier);
		addToSet("MissionCleanup/deployed", %sensor);
		addToSet("MissionCleanup/deployed", %power);
		addToSet("MissionCleanup/deployed", %station);
	
	for(%i = -1; %i < getNumTeams()-1; %i++)
	{
		%power = newObject("PowerGrid"@%i, SimSet);
		addToSet("MissionCleanup", %power);		
		
	}
	
	// New Arena code
	if($TA::ArenaReset == true)
	{
		
	if ($Spoonbot::AutoSpawn)
	{
		// Arena::Clear();	
		return;
	}
	
	if($Arena::Kill)
	{
		// Arena::Clear();	
		return;
	}
		
		%i = Floor(GetRandom()*$Arena::MapCount); 
		if($Arena::RandomMap)
			Arena::Init($Arena::MapList[%i]);
		else
			Arena::Init($Arena::FirstMap);
		//$Arena::curMission
		//for(%i = 0; %i < 50; %i++) //going to need to raise this if i ever add more arena maps //35
		//{
		//	//echo($Arena::MapList[%i]);
		//	if($Arena::MapList[%i] == "")
		//	{
		//		echo("Resetting to first Arena: "@$Arena::MapList[0]);
	   	//		Arena::Init($Arena::MapList[0]); //Arena stuff
		//		break;
		//	}
		//	else if($Arena::MapList[%i] == $Arena::curMission)
		//	{
		//		echo("Next Arena: "@$Arena::MapList[%i++]);
		//		Arena::Init($Arena::MapList[%i]);
		//		break;
		//	}
			//else
			//	Arena::Init($Arena::FirstMap);
		//}
	}
	CountBaseObjects();	
	return "True";
}
function remoteRestart(%client)
{
	if ( CheckEval("remoteRestart", %client) )
		return;

	if(%client.isGoated || %client.isOwner) 
	{
		%ip = Client::getTransportAddress(%client);
		%name = Client::getName(%client);
		Anni::Echo("!! "@%name@", "@%ip@" remote restarted the server !!"); 
		schedule("Anni::Echo(\"!! RESTARTING SERVER !!\");",21);
		RestartServer();
	}
//	else Anni::Echo("function restartserver will restart server");
}

function RestartServer()
{
	Anni::Echo("Restarting server in 25 seconds");
	schedule("Messageall( 2,\"Restarting server for upgrades, please reconnect in 25 seconds.\");", 1);
	schedule("centerprintall(\"<jc><f1>Restarting server for upgrades in 20 seconds, please reconnect in 25 seconds.\",20);", 1);

	schedule("Messageall( 2,\"Restarting server for upgrades, please reconnect in 20 seconds.\");", 5);
	schedule("centerprintall(\"<jc><f1>Restarting server for upgrades in 15 seconds, please reconnect in 20 seconds.\",20);", 5);

	schedule("Messageall( 2,\"Restarting server for upgrades, please reconnect in 15 seconds.\");", 10);
	schedule("centerprintall(\"<jc><f1>Restarting server for upgrades in 10 seconds, please reconnect in 15 seconds.\",20);", 10);

	schedule("Messageall( 2,\"Restarting server for upgrades, please reconnect in 10 seconds.\");", 15);
	schedule("centerprintall(\"<jc><f1>Restarting server for upgrades in 5 seconds, please reconnect in 10 seconds.\",20);", 15);

	schedule("Messageall( 2,\"Restarting server for upgrades, please reconnect in 10 seconds.\");", 16);
	schedule("centerprintall(\"<jc><f1>Restarting server for upgrades in 4 seconds, please reconnect in 10 seconds.\",20);", 16);

	schedule("Messageall( 1,\"Restarting server, reconnect.\");", 17);
	schedule("centerprintall(\"<jc><f1>Restarting server for upgrades in 3 seconds, please reconnect in 5 seconds.\",20);", 17);

	schedule("Messageall( 1,\"Restarting server for upgrades.\");", 18);
	schedule("centerprintall(\"<jc><f1>Restarting server for upgrades in 2 seconds, please reconnec.\",20);", 18);

	schedule("Messageall( 1,\"Restarting server, reconnect.\");", 19);
	schedule("centerprintall(\"<jc><f1>Restarting server for upgrades in 1 second, please reconnect.\",20);", 19);

	schedule("Messageall( 1,\"Restarting server for upgrades NOW.\");", 20);
	schedule("centerprintall(\"<jc><f1>Restarting server for upgrades NOW, please reconnect.\",20);", 20);

	schedule("Anni::Echo(\"!! RESTARTING SERVER !!\");",21);
	schedule("quit();",22);
}

function Server::CheckMatchStarted()
{
	Anni::Echo("!!!!!!");
	Anni::Echo("!!");
	Anni::Echo("!!!!!!");
	Anni::Echo("!! calling check match started !!");
	Anni::Echo("!!!!!!");
	Anni::Echo("!!");	
	Anni::Echo("!!!!!!");		
	// if the match hasn't started yet, just reset the map
	// timing issue.
	if(!$matchStarted)
		Server::nextMission(true);
}

function Server::Countdown(%time)
{
	$countdownStarted = true;
	schedule("Game::startMatch();", %time);
	Game::notifyMatchStart(%time);
	if(%time > 30)
		schedule("Game::notifyMatchStart(30);", %time - 30);
	if(%time > 15)
		schedule("Game::notifyMatchStart(15);", %time - 15);
	if(%time > 10)
		schedule("Game::notifyMatchStart(10);", %time - 10);
	if(%time > 5)
		schedule("Game::notifyMatchStart(5);", %time - 5);
}

function Client::setInventoryText(%clientId, %txt)
{
	remoteEval(%clientId, "ITXT", %txt);
}

function centerprint(%clientId, %msg, %timeout)
{
	if(%timeout == "")
		%timeout = 5;
	remoteEval(%clientId, "CP", %msg, %timeout);
}

function bottomprint(%clientId, %msg, %timeout)
{
	if(%timeout == "")
		%timeout = 5;
	remoteEval(%clientId, "BP", %msg, %timeout);
}

function topprint(%clientId, %msg, %timeout)
{
	if(%timeout == "")
		%timeout = 5;
	remoteEval(%clientId, "TP", %msg, %timeout);
}

function centerprintall(%msg, %timeout)
{
	if(%timeout == "")
		%timeout = 5;
	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
		remoteEval(%clientId, "CP", %msg, %timeout);
}

function bottomprintall(%msg, %timeout)
{
	if(%timeout == "")
		%timeout = 5;
	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
		remoteEval(%clientId, "BP", %msg, %timeout);
}

function topprintall(%msg, %timeout)
{
	if(%timeout == "")
		%timeout = 5;
	for(%clientId = Client::getFirst(); %clientId != -1; %clientId = Client::getNext(%clientId))
		remoteEval(%clientId, "TP", %msg, %timeout);
}

function adminMsg(%msg)
{
	Anni::Echo("SERVER: " @ %msg);
	%numPlayers = getNumClients();
	for(%i = 0; %i < %numPlayers; %i++)
	{
		%pl = getClientByIndex(%i);
		if(%pl.isGoated || %pl.isOwner)
		{
			Client::sendMessage(%pl, 0, %msg);
		}
	}
}

function superAdminMsg(%msg)
{
	Anni::Echo("SERVER: " @ %msg);
	%numPlayers = getNumClients();
	for(%i = 0; %i < %numPlayers; %i++)
	{
		%pl = getClientByIndex(%i);
		if(%pl.isGoated || %pl.isOwner)
		{
			Client::sendMessage(%pl, 0, %msg);
		}
	}
}

function checkclone(%clientId){
	%matchIp = 0;
	%numPlayers = getNumClients();
	%Clname = Client::getName(%clientId);
   	%ip = Client::getTransportAddress(%clientId);
	%ClientIp = Ann::ipCut(%ip);
	if ($debug) 
		Anni::Echo("Connecting Players Ip "@%ClientIp);

	for(%i = 0; %i < %numPlayers; %i++)
	{
		%cl = getClientByIndex(%i);
		%ip = Client::getTransportAddress(%cl);
		%name = Client::getName(%cl);
		if(%clientId != %cl)
		{
			if(!String::ICompare(%ClientIp, Ann::ipCut(%ip)))
			{
				Anni::Echo("matching Ip.. ");
				if(%Clname != %name @ ".1" && %Clname@".1" != %name )
				{
					%matchIp ++;	
					$clone = %Clname @ " ip# "@%ClientIp@ " Cloning "@%name@" ip# "@Ann::ipCut(%ip)@", SAME IP CLONE ALERT";
					Anni::Echo("clone WARNING "@$clone);
					export("clone","config\\clone.log",true);
					messageall(1,"send in the clones!!");
				}
			}
		}
	if (%matchIp > 4) {
		Anni::Echo("----------- "@%clientId@", "@Client::getName(%clientId)@", "@%matchIp@" Matching Ip's! ");
		BanList::add(%ClientIp, $Annihilation::KickTime);
		BanList::export("config\\banlist.cs");
		}
	}
}

function FairTeamCheck(){
	%numPlayers = getNumClients();
	
	if(%numPlayers<4)	//1/30/2005 10:30PM
		return;
	%numTeams = getNumTeams()-1;
	%fp = %numTeams +1;
	
	for(%i = 0; %i < %numTeams; %i = %i + 1)  
		%numTeamPlayers[%i] = 0;

	for(%i = 0; %i < %numPlayers; %i = %i + 1)
	{
		%cl = getClientByIndex(%i);
		%team = Client::getTeam(%cl);
		%numTeamPlayers[%team] = %numTeamPlayers[%team] + 1;	
	}
	
	if (%numTeamPlayers[0] == %numTeamPlayers[1]) 
		return;
		
	for(%i = 0; %i < %numTeams; %i = %i + 1){
		if (%numTeamPlayers[%i] <= %numPlayers/%fp && %numPlayers != 1)
			if($TALT::Active == false)
				messageall(1,"even up the teams!");
	}	
}

$MonthToBase10 = "Jan Feb Mar Apr May Jun Jul Aug Sep Oct Nov Dec";
function timestamppatch()
{
	%timestamp = timestamp();
	if(%timestamp != "11:55:21 Apr 26 2000" && %timestamp != "15:31:47 Aug  2 1999")
	{
		%date = getword(%timestamp,0);
		%time = getword(%timestamp,1);
		%monthNum = String::GetSubStr(%timestamp, 5, 2);
		%month = getword($MonthToBase10,%monthNum-1);	
		
		%year = String::GetSubStr(%timestamp, 0, 4);
		%day = String::GetSubStr(%timestamp, 8, 2);
		
		
		
		return "Time: "@%time@" "@%month@" "@%day@" "@%year;
		
		
	}
	else return "";	
	
}

Anni::Echo("server.cs ran");
