$InvList[ChameleonPack] = 1;
$MobileInvList[ChameleonPack] = 1;
$RemoteInvList[ChameleonPack] = 1;
AddItem(ChameleonPack);

ItemImageData ChameleonPackImage 
{
	shapeFile = "ammoPack";
	mountPoint = 2;
	weaponType = 2;
	minEnergy = 5;
	maxEnergy = 12;	//Energy used/sec
	sfxFire = SoundShieldOn;
	firstPerson = false;
};

ItemData ChameleonPack 
{
	description = "Chameleon Pack";
	shapeFile = "ammoPack";
	className = "Backpack";
	heading = $InvHead[ihBac];
	shadowDetailMask = 4;
	imageType = ChameleonPackImage;
	price = 175;
	hudIcon = "shieldpack";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function ChameleonPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Chameleon Pack: <f2>Mimic enemy armor to appear as a friendly. You can use enemy teleporters!\n<jc><f1>Warning:<f2> Enemy deployed cat sensors may detect and reveal you.");	
}

function ChameleonPackImage::onActivate(%player) 
{	
	%clientId = Player::getClient(%player);
	if(getNumTeams()-1 > 2 || $UnCvrA || $UnCvrB) 
	{
		if(getNumTeams()-1 > 2) 
		{
			Client::sendMessage(%clientId,0,"Can't go use your Chameleon powers with more then 2 teams.~waccess_denied.wav");
			Player::decItemCount(%clientId, Player::getMountedItem(%clientId,$BackpackSlot));
			return;
		}
		else 
			if(GameBase::getTeam($UnCvrA) == GameBase::getTeam(%clientId)) 
			{
				Client::sendMessage(%clientId,0,Client::getName($UnCvrA) @ " already summoned the Chameleon power, one at a time only.~waccess_denied.wav");
				Player::trigger(%player, $BackpackSlot, false);
				return;
			}
		else 
			if(GameBase::getTeam($UnCvrB) == GameBase::getTeam(%clientId)) 
			{
				Client::sendMessage(%clientId,0,Client::getName($UnCvrB) @ " already summoned the Chameleon power, one at a time only.~waccess_denied.wav");
				Player::trigger(%player, $BackpackSlot, false);
				return;
			}
	}
	Player::dropItem(%clientId, Flag);
	%clientId.isSpy = true;
	%player.cloakable = "";
	if(Client::getTeam(%clientId) == 0) 
	{
		$UnCvrA = %clientId;
		%clientId.OrigTeam = 5;
		teamMessages(1, 0, Client::getName(%clientId) @ " summons Chameleon powers.", -2, "", "");
		Client::setinitialTeam(%clientId, 1);
		GameBase::setTeam(%clientId, 1);
		Client::setinitialTeam(%clientId, 0);
		//Client::setSkin(%clientId, $Server::teamSkin[1]);
		
	}
	else
	{
		$UnCvrB = %clientId;
		%clientId.OrigTeam = 6;
		teamMessages(1, 1, Client::getName(%clientId) @ " summons Chameleon powers.", -2, "", "");
		Client::setinitialTeam(%clientId, 0);
		GameBase::setTeam(%clientId, 0);
		Client::setinitialTeam(%clientId, 1);
		//Client::setSkin(%clientId, $Server::teamSkin[0]);
	}
	%player.buffer = 0;
	%player.ChamCollapse = false;
	//schedule("ChameleonPack::Buffer("@%player@");",5);
}

function ChameleonPackImage::onDeactivate(%player) 
{	
	// Anni::Echo("spy ",%player.spy);
	%cl = Player::getClient(%player);
	if(%cl.OrigTeam != 5 && %cl.OrigTeam != 6) 
		return;
	if(%cl.OrigTeam == 5) 
	{
		$UnCvrA = "";
		GameBase::setTeam(%cl,0);
	}
	else 
		if(%cl.OrigTeam == 6) 
		{
			$UnCvrB = "";
			GameBase::setTeam(%cl,1);
		}
//	if(%cl.custom)
//		Client::setSkin(%cl, $Client::info[%cl, 0]);
//	else 
//		Client::setSkin(%cl, $Server::teamSkin[Client::getTeam(%cl)]);
	%pack = Player::getMountedItem(%cl,$BackpackSlot);
	//Player::decItemCount(%player, %pack);
	%cl.isSpy = false;
	%player.cloakable = true;
	%cl.OrigTeam = "";
	Client::sendMessage(%cl,0,"Your Chameleon powers wear off.~waccess_denied.wav");
	Player::trigger(%player, $BackpackSlot, false);
}

function ChameleonPack::Buffer(%player)
{
	if(%player.ChamCollapse || %player.Catscratch)
		return;
	%cl = Player::getClient(%player);
	%pack = Player::getMountedItem(%cl,$BackpackSlot);
	%player.buffer = 0;
	if(%pack == ChameleonPack && Player::isTriggered(%player,$BackpackSlot))
	{
		%ppos = GameBase::getPosition(%player);
		%set = newObject("set",SimSet);
		%Mask = $StaticObjectType; 
		%num = containerBoxFillSet(%set,%Mask,%ppos,50,50,50,0);
		%totalnum = Group::objectCount(%set);
		for(%i = 0; %i < %totalnum; %i++)
		{
			%obj = Group::getObject(%set, %i);
			%name = GameBase::getDataName(%obj);
			%dist = Vector::getDistance(%ppos, GameBase::getPosition(%obj));
			if(%dist < 50 && %name == DeployableCat && %obj.CatTeam == GameBase::getTeam(%player)) //changed from 25 to 50 -death666
			{
				%int = floor(150*(1/%dist));
				if(%int> 100)
					%int = 100;	
				%player.buffer += %int;						
			}
		}
		deleteObject(%set);		

	//	if(%player.buffer > 0)
		%player.Catscratch = true;
		schedule(%player@".Catscratch = false;",5,%player);
	//	Client::sendMessage(%cl,0,"Buffering against "@%player.buffer@"mW interference."); //removed this line and added the next -death666
		Bottomprint(%cl, "<jc><f2>Warning! <f1>Buffering against pussycat interference levels of:<f0> "@%player.buffer@" ");

	//	schedule("ChameleonPack::Buffer("@%player@");",1);
	}
	
	
	
}