$Server::MasterAddressN0 = "t1m1.masters.dynamix.com:28000 t1m2.masters.dynamix.com:28000 t1m3.masters.dynamix.com:28000";
$Server::MasterAddressN1 = "t1ukm1.masters.dynamix.com:28000 t1ukm2.masters.dynamix.com:28000 t1ukm3.masters.dynamix.com:28000";
$Server::MasterAddressN2 = "t1aum1.masters.dynamix.com:28000 t1aum2.masters.dynamix.com:28000 t1aum3.masters.dynamix.com:28000";
$Server::MasterName0 = "US Tribes Master";
$Server::MasterName1 = "UK Tribes Master";
$Server::MasterName2 = "Australian Tribes Master";
$Server::CurrentMaster = 0;

$Server::respawnTime = 2; // number of seconds before a respawn is allowed

// default translated masters:
$Server::XLMasterN0 = "IP:209.185.222.237:28000";
$Server::XLMasterN1 = "IP:209.67.28.148:28000";
$Server::XLMasterN2 = "IP:198.74.40.67:28000";
$Server::FloodProtectionEnabled = true;



// Server Parameters
$Server::HostName = "TA Server";	// Server Name
$Server::Port = "28001";					// Port used for client connections to server (usually 28001)
$Server::HostPublicGame = true;			// Server is shown in MasterServer List
$Server::Password = "";						// Password needed to join server
$AdminPassword = "";							// Local SuperAdmin password - CHANGE THIS
$pref::LastMission = "Stonehenge";		// This sets the first map in rotate when server launches (make sure it is spelled correctly)

//===========================================================================================
// Server Info Parameters (<jc> = center justified, <f1> = tan font, <f2> = white font, <f3> = orange font, \n = new line)
$Server::Info = "TA\nServer Owner\nEmail: ";	// Server information listed on MasterServer List
//$Server::MODInfo = "<jc><f1>www.annihilation.info";	// Information listed on server join screen
$Server::MODInfo = "<jc><f1>Server Discord:\n<f2>www.annihilation.info";
$Server::JoinMOTD = "<jc><f1>Welcome to <f2>"@$modList@" "@$ModVersion@"\n<f2>Have fun and don't be a jerk.";	// Message of the day listed once connected to the server

//===========================================================================================
// Telnet (Console) Parameters
// If you want telnet access, set the port number to the desired port (23 is normal)
// BE SURE TO SET A PASSWORD THAT IS HARD TO GUESS
$TelnetPort="";								// Port for telnet connections
$TelnetPassword="";							// Password for telnet connections

//===========================================================================================
// Server Connection Parameters
$pref::PacketRate = 12;						// Packet rate for client connections
$pref::PacketSize = 200;					// Packet size for client connections

//===========================================================================================
// Annihilation Parameters
//$Annihilation::NetMask = "IP:192.168";	// This is used to increase server player limit when local LAN players connect.
$Annihilation::IncreaseMax = false;		// If true, will increase player limit on server if IP of client connect matches NetMask.
$Annihilation::GiveLocalAdmin = true;	// If true, will give SuperAdmin status to players who are on the same machine as the server.
$Annihilation::ShoppingList = true;		// Set to true to limit item shopping list to display only items available for current armor.
$Annihilation::ResetServer = false;		// Set to true to rotate server to next map in list when last player leaves.
$Annihilation::KickTime = 180;			// Time (in seconds) for kicks.
$Annihilation::BanTime = 1800;			// Time (in seconds) for bans.
$Annihilation::StationTime = 200;		// Time allowed for Station Access.

//===========================================================================================
// Public Voting Parameters
$Annihilation::VoteAdmin = false;		// Allow Voting a Public Admin in.
$Annihilation::PVKick = true;				// Allow Public Kick Voting.
$Annihilation::PVChangeMission = true;	// Allow Public Mission Voting.
$Annihilation::PVTeamDamage = true;		// Allow Public Team Damage Voting.
$Annihilation::PVTourneyMode = true;	// Allow Public Tournament Mode Voting.
$Server::AdminMinVotes = 4;				// Minimum number of votes needed to vote admin
$Server::MinVotes = 1;						// Minimum number of votes needed to pass
$Server::MinVotesPct = 0.5;				// Percentage of available votes needed to pass a vote
$Server::MinVoteTime = 25;					// Time allotted for voting
$Server::VoteAdminWinMargin = 0.8;		// Ratio of Yes to No votes needed to pass
$Server::VoteFailTime = 30; 				// 30 seconds if your vote fails + $Server::MinVoteTime
$Server::VoteWinMargin = 0.6;				// Ratio of Yes to No votes needed to pass
$Server::VotingTime = 20;					// Length of votes if people are voting.

//===========================================================================================
// SuperAdmin Passwords, Up to 100 are available
$Annihilation::SADPassword[1] = "";
$Annihilation::SADPassword[2] = "";
$Annihilation::SADPassword[3] = "";
$Annihilation::SADPassword[4] = "";
$Annihilation::SADPassword[5] = "";

//===========================================================================================
// Public Admin Passwords, Up to 100 are available
$Annihilation::PAPassword[1] = "";
$Annihilation::PAPassword[2] = "";
$Annihilation::PAPassword[3] = "";
$Annihilation::PAPassword[4] = "";
$Annihilation::PAPassword[5] = "";
// Public Admin Parameters
$Annihilation::PAKick = true;				// Allow Public Admins to Kick.
$Annihilation::PATeamChange = true;		// Allow Public Admins to Change other Players Teams.
$Annihilation::PAChangeMission = true;	// Allow Public Admins to Change the Mission.
$Annihilation::PATeamDamage = true;		// Allow Public Admins to Enable/Disable Team Damage.
$Annihilation::PATourneyMode = true;	// Allow Public Admins to Enable/Disable Tournament Mode.

//===========================================================================================
// Other Parameters
$Annihilation::AutoAdmin = true; 	
	// Uses the AnnAdminList.cs in config, edit to your liking.

$Annihilation::ResetSettings = false;	// Resets server settings from this file on map change.

$Annihilation::FairTeams = true;			// Prevent team changing to the larger team

$Annihilation::UsePersonalSkin = true;	// Allows use of Personal Skins

$Annihilation::SafeBase = false;		
	// Base damage. True for safe (undestroyable) station and generators.

$Annihilation::BaseHeal = false;	
	// Base healing. True for regenerating (self healing) station and generators.

$Annihilation::VoteBuild = true;		// Allow voting on builder mode.

$Annihilation::obsAlert = true;		
	// Observer alert, notifies player who is watching them in observer.

$Annihilation::OutOfArea = false;		// Allow players out of bounds.

$Annihilation::QuickInv = false;		// inventory without stations
$Annihilation::ExtendedInvs = true;		// Extended Inventories, multiple use inventory stations.
$Annihilation::Zappy = true;		// uses electro beams to verify Extended Inventories aren't covered with blastwalls, force fields etc..
$Annihilation::HappyBreaker = false;	// creates dummy player models and flage to fool Happy Mod2
$debuginv = false;
$IpLogger = false;
//===========================================================================================
// Player Parameters
$Server::MaxPlayers = "15";				// Maximum number of client connections allowed
$Server::AutoAssignTeams = true;			// Server assigned teams
$Server::RespawnTime = 2; 					// Number of seconds before a respawn is allowed
$Server::TimeLimit = 30;					// Mission time limit in minutes
$Server::WarmupTime = 0;					// Time (in seconds) players are left standing before movement is allowed
$Server::TeamDamageScale = 0;				// Team damage, 0 = Off, 1 = On
$Server::TourneyMode = false;				// Tournament mode

//===========================================================================================
// Team Parameters
$Server::teamName[-1] = "";		// Obs Name
$Server::teamSkin[-1] = "base";			// Obs Skin
$Server::teamName[0] = "BE";		// Team 1 Name
$Server::teamSkin[0] = "beagle";			// Team 1 Skin
$Server::teamName[1] = "DS";	// Team 2 Name
$Server::teamSkin[1] = "dsword";			// Team 2 Skin
$Server::teamName[2] = "Subversion";	// Team 3 Name
$Server::teamSkin[2] = "cphoenix";		// Team 3 Skin
$Server::teamName[3] = "Downfall";		// Team 4 Name
$Server::teamSkin[3] = "swolf";			// Team 4 Skin
$Server::teamName[4] = "Generic 1";		// Team 5 Name
$Server::teamSkin[4] = "blue";			// Team 5 Skin
$Server::teamName[5] = "Generic 2";		// Team 6 Name
$Server::teamSkin[5] = "green";			// Team 6 Skin
$Server::teamName[6] = "Generic 3";		// Team 7 Name
$Server::teamSkin[6] = "orange";			// Team 7 Skin
$Server::teamName[7] = "Generic 4";		// Team 8 Name
$Server::teamSkin[7] = "purple";			// Team 8 Skin

//===========================================================================================
// Console Parameters
$Console::LogMode = "1"; 			// Log the console to console.log file.
$debug = false;					// Used for debugging... will spam the server window when on.
$adminpassword = "";
$build = false;

$annihilation::VehicleImpactor = true;	// adds a little cpu strain to safeguard against vehicle instability
$annihilation::DisableTurretsOnTeamChange = true;	//disables a clients turrets when they switch teams or disconnect
$annihilation::voteFlagCaps = true;		// allow public voting on flag caps