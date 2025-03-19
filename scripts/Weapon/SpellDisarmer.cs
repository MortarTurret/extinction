$InvList[DisarmerSpell] = 1;
$MobileInvList[DisarmerSpell] = 1;
$RemoteInvList[DisarmerSpell] = 1;

$AutoUse[DisarmerSpell] = True;
$WeaponAmmo[DisarmerSpell] = "";

addWeapon(DisarmerSpell);

RocketData DisarmBolt
{	
	bulletShapeName = "mortartrail.dts";
	explosionTag = mortarExp;
	collisionRadius = 0.0;
	mass = 2.0;
	damageClass = 1;
	damageValue = 0.1;
	damageType = $DisarmDamageType;
	explosionRadius = 20;
	kickBackStrength = 100.0;
	muzzleVelocity = 70.0;
	terminalVelocity = 80.0;
	acceleration = 5.0;
	totalTime = 10.0;
	liveTime = 11.0;
	lightRange = 5.0;
	lightColor = { 0.0, 1.0, 0.0 };
	inheritedVelocityScale = 0.5;
	trailType = 2;
	trailString = "mortartrail.dts";
	smokeDist = 4.5;
	soundId = SoundJetHeavy;
};

ItemImageData DisarmerSpellImage
{
	shapeFile = "mortartrail";
	mountPoint = 0;
	weaponType = 0;
//	reloadTime = 2;
//	fireTime = 1.9;
	minEnergy = 10;
	maxEnergy = 20;
	//projectileType = DisarmBolt;
	accuFire = true;
	//sfxFire = SoundParticleBeamRecharge;
	sfxActivate = SoundPickUpWeapon;
};

ItemData DisarmerSpell 
{
	heading = $InvHead[ihSpl];
	description = "Spell: Disarm";
	className = "Weapon";
	shapeFile = "DSPLY_S1";
	hudIcon = "fear";
	shadowDetailMask = 4;
	imageType = DisarmerSpellImage;
	price = 250;
	showWeaponBar = true;
};

function DisarmerSpellImage::onFire(%player, %slot) 
{		
	if($debug)
		Anni::Echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));	

	if(!%player.Reloading)
	{	
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);
		%player.Reloading = true;		
		schedule(%player @ ".Reloading = false;" , 3.5, %player);
		GameBase::playSound(%player, SoundParticleBeamRecharge, 0);		
	%energy = GameBase::getEnergy(%player);
	gamebase::setenergy(%player,%energy -10);
		Projectile::spawnProjectile("DisarmBolt",%trans,%player,%vel);	
	}
}

function DisarmerSpell::MountExtras(%player,%weapon) 
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>This spell disarms all caught in its blast.");
}