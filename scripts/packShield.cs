$InvList[ShieldPack] = 1;
$MobileInvList[ShieldPack] = 1;
$RemoteInvList[ShieldPack] = 1;
AddItem(ShieldPack);

	ItemImageData ShieldPackImage 
	{	
		shapeFile = "shieldPack";
		mountPoint = 2;
		weaponType = 2;
		minEnergy = 4;
		maxEnergy = 9;
		sfxFire = SoundShieldOn;
		firstPerson = false;
	};
	
//Pack::Define(ShieldPack,ShieldPack,"Shield Pack",175,ShieldPack,4,9,%active,%reload,%ready,SoundShieldOn);
	ItemData ShieldPack 
	{	
		description = "Shield Pack";
		shapeFile = "shieldPack";
		className = "Backpack";
		heading = $InvHead[ihBac];
		shadowDetailMask = 4;
		imageType = ShieldPackImage;
		price = 175;
		hudIcon = "shieldpack";
		showWeaponBar = true;
		hiliteOnActive = true;
	};

function ShieldPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Shield Pack: <f2>Protects you from damage at the cost of energy.");	
}

function ShieldPackImage::onActivate(%player,%imageSlot) 
{	
	Client::sendMessage(Player::getClient(%player),0,"Shield On");
	%player.shieldStrength = 0.012;
}

function ShieldPackImage::onDeactivate(%player,%imageSlot) 
{	
	Client::sendMessage(Player::getClient(%player),0,"Shield Off");
	Player::trigger(%player,$BackpackSlot,false);
	%player.shieldStrength = 0;
}
