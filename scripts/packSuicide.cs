$InvList[SuicidePack] = 1;
$MobileInvList[SuicidePack] = 1;
$RemoteInvList[SuicidePack] = 1;
AddItem(SuicidePack);

ItemImageData SuicidePackImage 
{	
	shapeFile = "mortarpack"; 
	mountPoint = 2;
	weaponType = 2;
	minEnergy = 0;
	maxEnergy = 0;
	mountPoint = 2;
	mountOffset = { 0, -0.1, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData SuicidePack 
{	
	description = "Suicide DetPack";
	shapeFile = "mortarpack"; 
	className = "Backpack";
	heading = $InvHead[ihBac];
	imageType = SuicidePackImage;
	shadowDetailMask = 4;
	mass = 2.5;
	elasticity = 0.2;
	price = 450;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function SuicidePack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Suicide DetPack: <f2>An explosive detonated instantly or on a timer. Set the timer in the tab menu under <f1>Personal Options<f2>.");	
}

function SuicidePackImage::onActivate(%player,%imageSlot)
{	
	%clientId = Player::getClient(%player);

	if (%player.hasmessageSP)
	{
		Client::sendMessage(%clientId,0, "You must wait to use another Suicide DetPack. ~wC_BuySell.wav");
		return;
	}

	Player::trigger(%player,$BackpackSlot);
	%player.hasmessageSP = true;
	schedule(%player@".hasmessageSP = false;",8.5,%player);
	
	Player::setItemCount(%player,Player::getMountedItem(%player,$backpackslot), 0);
	//Player::unmountItem(%player,$BackpackSlot);
	%obj = newObject("","Mine","Suicidebomb2");
	%obj.timer = %clientId.suicideTimer;
	addToSet("MissionCleanup", %obj);
	GameBase::throw(%obj,%player,3 * %clientId.throwStrength,false);
	if(%obj.timer > 0)
		Client::sendMessage(%clientId,1,"Suicide DetPack will destruct in " @ %obj.timer @ " seconds");
	else
		Client::sendMessage(%clientId,1,"Your Suicide DetPack was set to INSTANT");
	schedule("Mine::Detonate(" @ %obj @ ");",%obj.timer,%obj);
}