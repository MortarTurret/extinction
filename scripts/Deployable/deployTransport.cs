$InvList[TransportPack] = 1;
$MobileInvList[TransportPack] = 1;
$RemoteInvList[TransportPack] = 1;
AddItem(TransportPack);

ItemImageData TransportPackImage 
{
	shapeFile = "ammounit_remote"; 
	mountPoint = 2; 
	mountOffset = { 0, -0.03, 0 }; 
	mass = 2.5; 
	firstPerson = false; 
}; 

ItemData TransportPack 
{
	description = "Transport Vehicle"; 
	shapeFile = "ammounit_remote"; 
	className = "Backpack"; 
	heading = $InvHead[ihDOb]; 
	imageType = TransportPackImage; 
	shadowDetailMask = 4; 
	mass = 1.5; 
	elasticity = 0.2; 
	price = 1600; 
	hudIcon = "deployable"; 
	showWeaponBar = true; 
	hiliteOnActive = true; 
};

function TransportPack::deployShape(%player,%item) 
{
	%client = Player::getClient(%player); 
	%clientId = Player::getClient(%player);

		if(!$build)
		{
	if(%clientId.inArena)
	{ 
		Client::sendMessage(%client,0,"Cannot deploy in arena unless building is on. ");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return;	
	}
		}
		if(!$build)
		{
	if(%player.outArea)
	{
		Client::sendMessage(%client,0,"can not deploy out of bounds unless building is on.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
		}
	if($TeamItemCount[GameBase::getTeam(%player) @ "TransportVehicle"] >= $TeamItemMax[TransportVehicle] && !$build) 
	{
		Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
	if(!GameBase::getLOSInfo(%player,5)) 
	{
		Client::sendMessage(%client,0,"Deploy position out of range.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
	%obj = getObjectType($los::object); 
	if(%obj != "SimTerrain" && %obj != "InteriorShape") 
	{
		Client::sendMessage(%client,0,"Can only deploy on terrain or buildings.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
	if(Vector::dot($los::normal,"0 0 1") <= 0.7) 
	{
		Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
	if(!Vehicle::DeployArea(%player,Interceptor,$los::position)) 
		return false;	
	

	%rot = GameBase::getRotation(%player); 
	%objVehicle = newObject("",flier,Transport,true); 

	addToSet("MissionCleanup/deployed/object", %objVehicle); 
	GameBase::setTeam(%objVehicle,GameBase::getTeam(%player)); 
	GameBase::setPosition(%objVehicle,$los::position); 
	GameBase::setRotation(%objVehicle,%rot); 
	Gamebase::setMapName(%objVehicle,"HPC Pack"); 
	Client::sendMessage(%client,0,"Deployed Transport"); 
	GameBase::startFadeIn(%objVehicle); 
	Vehicle::TerrainCheck(%objVehicle);
	playSound(SoundPickupBackpack,$los::position); 
	$TeamItemCount[GameBase::getTeam(%player) @ "TransportVehicle"]++; 

		echo("MSG: "@Client::getName(%client)@", "@%client@" deployed a Transport Vehicle Pack "@Client::getTransportAddress(%client)); 

	return true; 
}

function TransportPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Transport Vehicle: <f2>Deploys a Transport. No vehicle station required.");	
}