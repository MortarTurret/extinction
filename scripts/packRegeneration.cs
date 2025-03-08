$InvList[RegenerationPack] = 1;
$MobileInvList[RegenerationPack] = 1;
$RemoteInvList[RegenerationPack] = 1;
AddItem(RegenerationPack);

ItemImageData RegenerationPackImage 
{	shapeFile = "armorkit"; 
	mountPoint = 2;
	weaponType = 2;
	minEnergy = 0;
	maxEnergy = 0;
	mountOffset = { 0, 0.05, -0.2 }; 
	mountRotation = { 0, 0, 0 };
	firstPerson = false;
};

ItemData RegenerationPack 
{	description = "Troll Pack";
	shapeFile = "AmmoPack"; 
	className = "Backpack";
	heading = $InvHead[ihBac];
	shadowDetailMask = 4;
	imageType = RegenerationPackImage;
	price = 275;
	hudIcon = "repairpack";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function RegenerationPack::OnMount(%player,%item)
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	
	
	//Player::mountItem(%player, RegenerationPack2, 7); -Removed because Troll Minigun uses slot 7 also. -Ghost
	Player::trigger(%player,$BackpackSlot,true);
	%client = Player::getclient(%player); 
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Troll Pack: <f2>Passively repairs your armor while worn.");
}

function RegenerationPack::onUnmount(%player,%item)
{	
	//Player::unmountItem(%player, 7); // -death666 3.18.17
}

function RegenerationPackImage::onActivate(%player,%imageSlot) 
{
	%player.regen = true;
	schedule("checkRegeneration(" @ %player @ ");",0.1,%player);
}

function RegenerationPackImage::onDeactivate(%player,%item)
{	
	%player.regen = false;
}

function checkRegeneration(%player, %switch)
{	
	if(%player.frozen == true)
		return;
	if(%player.regen == false)
		return;
	if(Player::isDead(%player))
		return;
	if(Player::getMountedItem(%player,$BackpackSlot) != "RegenerationPack")
		return;
	
	%dlev = GameBase::getDamageLevel(%player);
	%armor = Player::getArmor(%player);
	if(%armor == armorTroll)
	{
		GameBase::setDamageLevel(%player, %dlev-0.0275);
	}
	else
	{
		GameBase::setDamageLevel(%player, %dlev+0.275);
	}
	schedule("checkRegeneration(" @ %player @ ");",1,%player);
}

function RegenerationPack::onUse(%player,%item) 
{	
	if(Player::getMountedItem(%player,$BackpackSlot) != %item) 
	{	
		Player::mountItem(%player,%item,$BackpackSlot);
	}
}
