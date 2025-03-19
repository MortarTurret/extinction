$InvList[SpellFlameThrower] = 1; 
$MobileInvList[SpellFlameThrower] = 1;
$RemoteInvList[SpellFlameThrower] = 1;

$AutoUse[SpellFlameThrower] = False;
$WeaponAmmo[SpellFlameThrower] = "";

addWeapon(SpellFlameThrower);
GrenadeData SpellFlameThrowerGren
{	
	bulletShapeName = "PlasmaBolt.dts";
	explosionTag = plasmaExp;
	collideWithOwner = True;
	ownerGraceMS = 250;
	collisionRadius = 0.2;
	mass = 1.0;
	elasticity = 0.3;
	damageClass = 1;
	damageValue = 0.1;
	damageType = $PlasmaDamageType;
	explosionRadius = 8;
	kickBackStrength = 0;
	maxLevelFlightDist = 150;
	totalTime = 5.0;
	liveTime = 0.001;
	projSpecialTime = 0.05;
	inheritedVelocityScale = 0.5;
	smokeName = "Plasmatrail.dts";
	smokeDist = 1.5;
};

ItemImageData SpellFlameThrowerImage 
{
	shapeFile = "plasmabolt";
	mountPoint = 0;
	weaponType = 0;
	minEnergy = 5;
	//projectileType = SpellFlameThrowerGren;
	accuFire = false;
	reloadTime = 0.1;
	fireTime = 0.1;
	lightType = 3;
	lightRadius = 3;
	lightTime = 1;
	lightColor = { 0.6, 1, 1.0 };
	sfxFire = SoundJetHeavy;
	sfxActivate = SoundPickUpWeapon;
	sfxReload = SoundDryFire;
};

ItemData SpellFlameThrower 
{
	description = "Spell: Flame Thrower";
	className = "Weapon";
	shapeFile = "DSPLY_S1";
	hudIcon = "weapon";
	heading = $InvHead[ihSpl];
	shadowDetailMask = 4;
	imageType = SpellFlameThrowerImage;
	price = 175;
	showWeaponBar = true;
};

function SpellFlameThrowerImage::onFire(%player, %slot) 
{		
	if($debug)
		echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));	

		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);		
		%energy = GameBase::getEnergy(%player);
		gamebase::setenergy(%player,%energy -10);
		
		%pos = GameBase::getPosition(%player);
		if(!GameBase::getLOSInfo(%player,0.2))	//if(GameBase::testPosition(%player,%pos)) 
		{
			
			
			Projectile::spawnProjectile("SpellFlameThrowerGren",%trans,%player,%vel);	
		}	
		else
		{
			GameBase::applyDamage(%player,$PlasmaDamageType,0.1,GameBase::getPosition(%player),"0 0 0","0 0 0",%player);
			//Projectile::spawnProjectile("SpellFlameThrowerGren",%trans,%player,%vel);
			GameBase::playSound(%player, explosion4, 0);
		}	
				
	
}

function SpellFlameThrower::MountExtras(%player,%weapon) 
{	
	if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(Player::getclient(%player), "<jc>"@%weapon.description@": <f2>Hurls fiery death.");
}
