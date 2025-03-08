$InvList[RepairPack] = 1;
$MobileInvList[RepairPack] = 1;
$RemoteInvList[RepairPack] = 1;
AddItem(RepairPack);

ItemImageData RepairGunImage 
{	
	shapeFile = "repairgun";
	mountPoint = 0;
	weaponType = 2;
	projectileType = RepairBolt;
	minEnergy = 3;
	maxEnergy = 10;
	lightType = 3;
	lightRadius = 4;
	lightTime = 1;
	lightColor = { 1.0, 0.1, 0.1 }; 
	sfxActivate = SoundPickUpWeapon;
	sfxFire = SoundRepairItem;
};

ItemData RepairGun 
{	
	description = "Repair Gun";
	shapeFile = "repairgun";
	className = "Weapon";
	shadowDetailMask = 4;
	imageType = RepairGunImage;
	showInventory = false;
	price = 125;
};

ItemImageData RepairPackImage 
{	
	shapeFile = "armorPack";
	mountPoint = 2;
	weaponType = 2;
	minEnergy = 0;
	maxEnergy = 0;
	mountOffset = { 0, -0.05, 0 };
	mountRotation = { 0, 0, 0 };
	firstPerson = false;
};

ItemData RepairPack 
{	
	description = "Repair Pack";
	shapeFile = "armorPack";
	className = "Backpack";
	heading = $InvHead[ihBac];
	shadowDetailMask = 4;
	imageType = RepairPackImage;
	price = 125;
	hudIcon = "repairpack";
	showWeaponBar = true;
	hiliteOnActive = true;
	lightType = 2;                
	lightRadius = 4;
 	lightTime = 1; 
	lightColor = { 1, 0.1, 0.1 };
};

function RepairPack::onCollision(%this,%player)
{
	%client = player::getclient(%player);
	%c = Player::getClient(%player);
	%pack = Player::getMountedItem(%client,$BackpackSlot);
	%armor = player::getarmor(%player);
	%armorlist = $ArmorName[%armor].description;

	// if(player::getitemcount(%c,EnergyPack)>=1)
	// if(Player::getMountedItem(%cl,$backpackSlot) == EnergyPack)
//	if(%pack == "EnergyPack" && %armorlist == "Warrior")
//	{
//	if(%c.mortMute)return;
//	Client::sendMessage(%client, 3, "Repair Pack Equipped. ~wmine_act.wav");
//	%c.mortMute = 1;
//	schedule(%c@".mortMute=0;",5,%c);
//	Player::dropItem(%c,Player::getMountedItem(%c,$BackpackSlot));
//	player::setitemcount(%c,RepairPack,1);
//	Player::useItem(%c,RepairPack);
//	Player::trigger(%c,$BackpackSlot,true);
//	Player::mountItem(%player,RepairGun,$WeaponSlot);
//	Player::useItem(%c,RepairGun);
//        return;
//	}
	// else if(Player::getMountedItem(%cl,$backpackSlot) == "")
	if(%pack == "-1")
	{
	if(%armorlist == "Ghost")
	{
		return;
	}

	if(%c.mortMute)return;
	Client::sendMessage(%client, 3, "Repair Pack Equipped. ~wDryfire1.wav");
		if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Repair Pack: <f2>Repairs damaged players and objects.");
//	Player::dropItem(%c,Player::getMountedItem(%c,$BackpackSlot));
	player::setitemcount(%c,RepairPack,1);
	// GameBase::playSound(%player, SoundFireGrenade, 0);
	Player::useItem(%c,RepairPack);
	Player::trigger(%c,$BackpackSlot,true);
//	Player::mountItem(%player,RepairGun,$WeaponSlot);
//	Player::useItem(%c,RepairGun);
	%c.mortMute = 1;
	schedule(%c@".mortMute=0;",2.5,%c);
	}
}

function RepairPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	
	
	Player::trigger(%player,$BackpackSlot,true);
	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Repair Pack: <f2>Repairs damaged players and objects.");
}

function RepairGun::onUnmount(%player,%item) 
{	
	Player::trigger(%player,$BackpackSlot,false);
}

function RepairPack::onUnmount(%player,%item) 
{	
	if(Player::getMountedItem(%player,$WeaponSlot) == RepairGun) 
	{	
		Player::unmountItem(%player,$WeaponSlot);
	}
}

function RepairPack::onUse(%player,%item) 
{	
	if(Player::getMountedItem(%player,$BackpackSlot) != %item) 
	{	
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else 
	{	
		Player::mountItem(%player,RepairGun,$WeaponSlot);
	}
}

function RepairPack::onDrop(%player,%item) 
{	
	if($matchStarted) 
	{	
		%mounted = Player::getMountedItem(%player,$WeaponSlot);
		if(%mounted == RepairGun) 
		{	
			Player::unmountItem(%player,$WeaponSlot);
		}
		else 
		{	
			Player::mountItem(%player,%mounted,$WeaponSlot);
		}
		Item::onDrop(%player,%item);
	}
}
