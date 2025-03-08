$InvList[StealthShieldPack] = 1;
$MobileInvList[StealthShieldPack] = 1;
$RemoteInvList[StealthShieldPack] = 1;
AddItem(StealthShieldPack);

ItemImageData StealthShieldPackImage 
{	
	shapeFile = "shieldPack";
	mountPoint = 2;
	weaponType = 2;
	minEnergy = 6;
	maxEnergy = 9;
	sfxFire = SoundShieldOn;
	firstPerson = false;
};

ItemData StealthShieldPack 
{	
	description = "StealthShield Pack";
	shapeFile = "shieldPack";
	className = "Backpack";
	heading = $InvHead[ihBac];
	shadowDetailMask = 4;
	imageType = StealthShieldPackImage;
	price = 275;
	hudIcon = "shieldpack";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function StealthShieldPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>StealthShield Pack: <f2>A <f1>Sensor Jammer Pack<f2> and <f1>Shield Pack<f2> rolled into one!");	
}

function StealthShieldPackImage::onActivate(%player,%imageSlot) 
{	
	Client::sendMessage(Player::getClient(%player),0,"StealthShield On");
	%player.shieldStrength = 0.012;
	%rate = Player::getSensorSupression(%player) + 20;
	Player::setSensorSupression(%player,%rate);
}

function StealthShieldPackImage::onDeactivate(%player,%imageSlot) 
{	
	Client::sendMessage(Player::getClient(%player),0,"StealthShield Off");
	Player::trigger(%player,$BackpackSlot,false);
	%player.shieldStrength = 0;
	%rate = Player::getSensorSupression(%player) - 20;
	Player::setSensorSupression(%player,%rate);
}
