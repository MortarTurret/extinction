
$InvList[AmmoPack] = 1;
$MobileInvList[AmmoPack] = 1;
$RemoteInvList[AmmoPack] = 1;
AddItem(AmmoPack);


// Max Amount of ammo the Ammo Pack can carry
$AmmoPackMax[VulcanAmmo]= 100;
$AmmoPackMax[PlasmaAmmo] = 30;
$AmmoPackMax[DiscAmmo] = 15;
$AmmoPackMax[GrenadeAmmo] = 15;
$AmmoPackMax[MortarAmmo] = 10;
$AmmoPackMax[RubberAmmo]= 10;
$AmmoPackMax[TankRPGAmmo] = 300; 
$AmmoPackMax[RocketAmmo] = 10;
$AmmoPackMax[RailAmmo] = 10;
$AmmoPackMax[Grenade]= 5;
$AmmoPackMax[MineAmmo] = 5; 
$AmmoPackMax[FlamerAmmo] = 25;
$AmmoPackMax[FlameThrowerAmmo] = 25;
// $AmmoPackMax[HDiscLauncherAmmo] = 15;
$AmmoPackMax[PhaseAmmo] = 15;
$AmmoPackMax[ShotgunShells] = 10;
$AmmoPackMax[SniperAmmo]= 10;
$AmmoPackMax[StingerAmmo] = 10; 
$AmmoPackMax[TankShredderAmmo] = 100;
$AmmoPackMax[TBlastCannonAmmo] = 10;



// Items in the AmmoPack
$AmmoPackItems[0] = VulcanAmmo;
$AmmoPackItems[1] = PlasmaAmmo;
$AmmoPackItems[2] = DiscAmmo;
$AmmoPackItems[3] = GrenadeAmmo;
$AmmoPackItems[4] = MortarAmmo;
$AmmoPackItems[5] = RubberAmmo;
$AmmoPackItems[6] = TankRPGAmmo; 
$AmmoPackItems[7] = RocketAmmo;
$AmmoPackItems[8] = RailAmmo;
$AmmoPackItems[9] = Grenade;
$AmmoPackItems[10] = MineAmmo;
$AmmoPackItems[11] = FlamerAmmo;
$AmmoPackItems[12] = FlameThrowerAmmo;
// $AmmoPackItems[13] = HDiscLauncherAmmo;
$AmmoPackItems[13] = PhaseAmmo;
$AmmoPackItems[14] = ShotgunShells;
$AmmoPackItems[15] = SniperAmmo;
$AmmoPackItems[16] = StingerAmmo; 
$AmmoPackItems[17] = TankShredderAmmo;
$AmmoPackItems[18] = TBlastCannonAmmo;


ItemImageData AmmoPackImage 
{
	shapeFile = "AmmoPack";
	mountPoint = 2;
	mountOffset = { 0, -0.03, 0 };
	firstPerson = false;
  //energy pack like..	
	minEnergy = -2;
	maxEnergy = -3;
	weaponType = 2;	
};

ItemData AmmoPack 
{
	description = "Ammo Pack";
	shapeFile = "AmmoPack";
	className = "Backpack";
	heading = $InvHead[ihBac];
	imageType = AmmoPackImage;
	shadowDetailMask = 4;
	mass = 2.0;
	elasticity = 0.2;
	price = 325;
	hudIcon = "ammopack";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function AmmoPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Ammo Pack: <f2>Allows more ammo to be held.");	
}

function AmmoPack::onDrop(%player, %item)
{
	if($matchStarted) {
		%item = Item::onDrop(%player,%item);
		for(%i = 0; %i < 20 ; %i = %i +1) {
			%numPack = 0;
			%ammoItem = $AmmoPackItems[%i];
			%maxnum = $ItemMax[Player::getArmor(%player), %ammoItem];
			%pCount = Player::getItemCount(%player, %ammoItem);
			if(%pCount > %maxnum) 
			{
				%numPack = %pCount - %maxnum;
				Player::decItemCount(%player,%ammoItem,%numPack);
			}	
			$AmmoPackCount[%this,%i] = %numPack;
		}
	}
}

function AmmoPack::onCollision(%this,%object)
{
	if (getObjectType(%object) == "Player") {
		%item = Item::getItemData(%this);
		%count = Player::getItemCount(%object,%item);
		if (Item::giveItem(%object,%item,Item::getCount(%this))) {
			Item::playPickupSound(%this);
			checkPacksAmmo(%object, %this);
			Item::respawn(%this);
		}
	}
}

function checkPacksAmmo(%player, %item)
{
	for(%i = 0; %i < 20 ; %i = %i +1) {
		%ammoItem = $AmmoPackItems[%i];		
		%numAdd = $AmmoPackCount[%this,%i];	
		Player::incItemCount(%player,%ammoItem,%numAdd);
	}						 
}

function fillAmmoPack(%client)
{
	%player = Client::getOwnedObject(%client);
	for(%i = 0; %i < 20 ; %i = %i +1) {
		%item = $AmmoPackItems[%i];
		%maxnum = $AmmoPackMax[%item];
		%maxnum = checkResources(%player,%item,%maxnum); 
		if(%maxnum) {
			Player::incItemCount(%client,%item,%maxnum);
			teamEnergyBuySell(%player,%item.price * %maxnum * -1);
		}	
	}
}
