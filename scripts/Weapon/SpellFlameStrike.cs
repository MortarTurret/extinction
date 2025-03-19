$InvList[FlameStrike] = 1;
$MobileInvList[FlameStrike] = 1;
$RemoteInvList[FlameStrike] = 1;

$AutoUse[FlameStrike] = True;
$WeaponAmmo[FlameStrike] = "";

addWeapon(FlameStrike);

ItemImageData FlameStrikeImage
{
	shapeFile = "plasmabolt";
	mountPoint = 0;
	weaponType = 0; // Single Shot
	//projectileType = PlasIC;
	accuFire = true;
	reloadTime = 0.0;	
	fireTime = 0.0;		//1.0;
	minEnergy = 50;
	maxEnergy = 60;
	lightType = 3;  // Weapon Fire
	lightRadius = 2;
	lightTime = 1;
	lightColor = { 1, 0, 0 };
	//sfxFire = bigExplosion2;
	sfxActivate = SoundPickUpWeapon;
};

ItemData FlameStrike
{
	description = "Spell: Flame Strike";
	className = "Weapon";
	shapeFile = "DSPLY_S1";
	hudIcon = "plasma";
	heading = $InvHead[ihSpl];
	shadowDetailMask = 4;
	imageType = FlameStrikeImage;
	price = 1000;
	showWeaponBar = true;
};

function FlameStrike::MountExtras(%player,%weapon)
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>Sends a racing bolt of flame to incinerate your target.");
}

function FlameStrikeImage::onFire(%player, %slot) 
{		
	if($debug)
		Anni::Echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));	

	if(!%player.Reloading)
	{	
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);
		%player.Reloading = true;		
		schedule(%player @ ".Reloading = false;" , 1.7, %player);
		GameBase::playSound(%player, bigExplosion2, 0);		
		if(floor(getRandom() * 10) == 0) // 30
		{
			%client = Player::getClient(%player);
			Bottomprint(%client, "<jc><f1>WARNING:<f2> FLAME STRIKE OUT OF CONTROL- CLEAR THE EXPLOSION AREA!!!");
			Client::sendMessage(%client, 1, "FLAME STRIKE OUT OF CONTROL- CLEAR THE EXPLOSION AREA!!! ~wfemale2.whitdeck.wav");
			Projectile::spawnProjectile("NukeShell",%trans,%player,%vel);
		}
		else
		{
			Projectile::spawnProjectile("FlameStrikeSmall",%trans,%player,%vel);
		}
	}
}
