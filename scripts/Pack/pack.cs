//----------------------------------------------------------------------------
// Backpacks
//----------------------------------------------------------------------------

//----------------------------------------------------------------------------

ItemData Backpack
{
	description = "Backpack";
	showInventory = false;
};

// type = weapon type, 0 single, 1 rotating, 2 sustained, 3disc
// icon = HudIcon, deployable, ammopack, shieldpack, sensorjamerpack, energypack, repairpack, plasma, blaster
//                targetlaser, disk, weapon, sniper, grenade, energyrifle, mortar, fear, reticle, clock, chain

function Pack::Define(%name,%shape,%description,%price,%icon,%min,%max,%type,%active,%reload,%ready,%fire)
{
	%ImageData = "ItemImageData "@ %name@"Image"@	
		" { "@
			"shapeFile = \""@%shape@"\";"@
			"mountPoint = 2;"@	
			"weaponType = "@%type@";"@
			"minEnergy = "@%min@";"@
			"maxEnergy = "@%max@";"@
			"firstPerson = false;"@
		" };"; 	
	eval(%ImageData); 
	

	%Image = %name@"Image";
	if(%active)%Image.sfxActivate = %active;
	if(%reload)%Image.sfxReload = %reload;
	if(%ready)%Image.sfxReady = %ready;
	if(fire)%Image.sfxFire = %fire;

	%Data = 	"ItemData "@ %name@
		" { "@	
			"description = \""@%description@"\";"@
			"shapeFile = \""@%shape@"\";"@
			"className = \"Backpack\";"@
			"heading = "@$InvHead[ihBac]@";"@
			"shadowDetailMask = 4;"@
			"imageType = "@%name@"Image;"@
			"price = "@%price@";"@
			"showWeaponBar = true;"@
			"hudIcon = "@%icon@";"@
			"hiliteOnActive = true;"@
		"};"; 	
	eval(%Data);
}
	
	
	
	


function Backpack::onUse(%player,%item)
{
	if($debug)
		Anni::Echo("Backpack::onUse "@%item@", deployable ="@$TeamItemMax[%item]);
	%client = Player::getClient(%player);
	if(Player::getMountedItem(%player,$BackpackSlot) != %item)
	{
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else
	{

			if($TeamItemMax[%item])
			{

				if (eval(%item@"::deployShape("@%player@","@%item@");") && !$build)
					Player::decItemCount(%player,%item);	//success!!
						
			}
			else
			{
				//Anni::Echo("Trigger "@%item);
				Player::trigger(%player,$BackpackSlot);
			}
//		}
//		else Client::sendMessage(%client,0,"~waccess_denied.wav");
	}
}
//	function BackPack::onUse(%player,%item)
//	{
//		if (Player::getMountedItem(%player,$BackpackSlot) != %item)
//			Player::mountItem(%player,%item,$BackpackSlot);
//		else
//			Player::deployItem(%player,%item);
//	}