$InvList[Laptop] = 1;
$MobileInvList[Laptop] = 1;
$RemoteInvList[Laptop] = 1;
AddItem(Laptop);

ItemImageData LaptopImage 
{
	shapeFile = "AmmoPack";
	mountPoint = 2;
	mountOffset = { 0, -0.03, 0 };
	weaponType = 2;
	minEnergy = -1;
	maxEnergy = -3;
	mass = 0.5;
	firstPerson = false;
};

ItemData Laptop 
{
	description = "Laptop";
	shapeFile = "ammounit_remote";
	className = "Backpack";
	heading = $InvHead[ihBac];
	shadowDetailMask = 4;
	imageType = LaptopImage;
	price = 650;
	hudIcon = "energypack";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function Laptop::IsAvailable(%player)
{
	return (Player::getMountedItem(%player, $BackpackSlot) == Laptop);
}

function Laptop::Error(%client, %msg)
{
	Client::sendMessage(%client, 0, "Laptop Err: " @ %msg @ "~waccess_denied.wav");
}

function Laptop::Output(%client, %msg)
{
	Client::sendMessage(%client, 0, "Laptop: " @ %msg);
}

function Laptop::onUse(%player,%item) 
{
	if(Player::getMountedItem(%player,$BackpackSlot) != %item) 
		Player::mountItem(%player,%item,$BackpackSlot);
}

function Laptop::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));

	Player::trigger(%player,$BackpackSlot,true);
	%client = Player::getClient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Laptop: <f2>will allow you to control turrets without being at a <f1>Command Station<f2> also lets you use enemy teleporters!");
}


