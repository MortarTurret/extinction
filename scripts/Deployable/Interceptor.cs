$InvList[InterceptorPack] = 1; //FighterPack -death666
$MobileInvList[InterceptorPack] = 1; //Fighterpack -death666
$RemoteInvList[InterceptorPack] = 1; //Fighterpack -death666
AddItem(InterceptorPack); //FighterPack -death666

ItemImageData InterceptorPackImage //FighterPackImage -death666
{
	shapeFile = "ammounit_remote"; 
	mountPoint = 2; 
	mountOffset = { 0, -0.03, 0 }; 
	mass = 2.5; 
	firstPerson = false; 
}; 

ItemData InterceptorPack //FighterPack -death666 
{
	description = "Interceptor Pack"; //Fighter Vehicle Pack -death666
	shapeFile = "ammounit_remote"; 
	className = "Backpack"; 
	heading = $InvHead[ihDOb]; 
	imageType = InterceptorPackImage; //FighterPackImage -death666
	shadowDetailMask = 4; 
	mass = 1.5; 
	elasticity = 0.2; 
	price = 600; 
	hudIcon = "deployable"; 
	showWeaponBar = true; 
	hiliteOnActive = true; 
};

function InterceptorPack::deployShape(%player,%item) //FighterPack -death666
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

	if($TeamItemCount[GameBase::getTeam(%player) @ "InterceptorVehicle"] >= $TeamItemMax[InterceptorVehicle] && !$build) 
	{
		Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
		return false;
	}
	if(!GameBase::getLOSInfo(%player,5)) 
	{
		Client::sendMessage(%client,0,"Deploy position out of range.");
		return false;
	}
	%obj = getObjectType($los::object); 
	if(%obj != "SimTerrain" && %obj != "InteriorShape") 
	{
		Client::sendMessage(%client,0,"Can only deploy on terrain or buildings");
		return false;
	}
	if(Vector::dot($los::normal,"0 0 1") <= 0.7) 
	{
		Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
		return false;
	}
	if(!Vehicle::DeployArea(%player,Interceptor,$los::position)) 
		return false;		
	GameBase::getLOSInfo(%player,5);
	%client = Player::getClient(%player);
		
		%rot = GameBase::getRotation(%player); 
		%objVehicle = newObject("",flier,Interceptor,true); 

		addToSet("MissionCleanup/deployed/object", %objVehicle); 
		GameBase::setTeam(%objVehicle,GameBase::getTeam(%player)); 
		GameBase::setPosition(%objVehicle,$los::position); 
		GameBase::setRotation(%objVehicle,%rot); 
		Client::sendMessage(%client,0,"Piloting Interceptor..."); 

		GameBase::startFadeIn(%objVehicle); 
		Vehicle::TerrainCheck(%objVehicle);
		playSound(SoundPickupBackpack,$los::position); 
		Anni::Echo("vehicky "@GameBase::getDataName(%objVehicle));
		%objVehicle.isInterceptorPack = true; //FighterPack -death666
		$TeamItemCount[GameBase::getTeam(%player) @ "InterceptorVehicle"]++;
	
		Anni::Echo("MSG: "@Client::getName(%client)@", "@%client@" deployed a Interceptor= "@ %objVehicle @", "@Client::getTransportAddress(%client)); 
				
		return true;	

	
}

function flierBombSound(%obj)
{
	%BombPos = vector::add(GameBase::getPosition(%obj),"0 0 1");
	playSound(SoundFlyerBomb,%BombPos);
	%obj.time--;
	%rnd = (floor(getRandom() * 60) -30)/30;
	if(%obj.time>0)
		schedule("flierBombSound("@%obj@");",0.70 + %rnd,%obj);
	
}

function InterceptorPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Interceptor Pack: <f2>Deploys an Interceptor. No vehicle pad required.");	
}