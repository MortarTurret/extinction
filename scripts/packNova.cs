//$InvList[NovaPack] = 1;
//$MobileInvList[NovaPack] = 1;
//$RemoteInvList[NovaPack] = 1;
//AddItem(NovaPack);
// This has been incorporated into angels air brake beacon by Death666 so I am removing the pack. -Ghost


BulletData NovaBoltA
{
	bulletShapeName = "enbolt.dts";
	explosionTag = energyExp;
	damageClass = 0;
	damageValue = 0.1;
	damageType = $ElectricityDamageType;
	aimDeflection = 10;
	muzzleVelocity = 50.0;
	totalTime = 0.5;
	liveTime = 0.2;
	lightRange = 3.0;
	lightColor = { 1, 1, 0 };
	inheritedVelocityScale = 0.3;
	isVisible = True;
	soundId = SoundJetLight;
};

BulletData NovaBoltB
{
	bulletShapeName = "fusionbolt.dts";
	explosionTag = TurretExp;
	damageClass = 0;
	damageValue = 0.3;
	damageType = $ElectricityDamageType;
	aimDeflection = 10;
	muzzleVelocity = 50.0;
	totalTime = 0.5;
	liveTime = 0.2;
	lightRange = 3.0;
	lightColor = { 1, 1, 0 };
	inheritedVelocityScale = 0.3;
	isVisible = True;
	soundId = SoundJetLight;
};

ItemImageData NovaPackImage 
{	
	shapeFile = "ammounit_remote";
	mountPoint = 2;
	weaponType = 2;
	fireTime = 0;
	reloadTime = 1;
	minEnergy = 10;
	maxEnergy = 10;	
	firstPerson = false;
};

ItemData NovaPack
{	
	description = "Nova Pack";
	shapeFile = "ammounit_remote";
	className = "Backpack";
	heading = $InvHead[ihBac];
	imageType = NovaPackImage;
	price = 450;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function NovaPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Nova Pack: <f2>Releases a burst of electrical energy rendering enemy <f1>shields, jetpacks, and energy weapons<f2> useless.\n<jc><f2>For best results use at extremely close proximity, standing still and with full energy");	
}

function NovaPackImage::onActivate(%player,%imageSlot)
{	
	%clientId = Player::getClient(%player);
	Player::trigger(%player,$BackpackSlot,false);
	%wep = Player::getMountedItem(%player,$WeaponSlot);
	Player::unmountItem(%player,0);
	Schedule("Player::mountItem("@ %player @","@ %wep @", "@ $WeaponSlot @");",0.5,%player);

	%pos = getBoxCenter(%player);
	%energy = GameBase::getEnergy(%player);


	if(%energy != 40)
	{
		PlaySound("SoundFirePlasma",%pos);
		for(%i = 0; %i < floor(%energy); %i++) {
			Projectile::SpawnProjectile("NovaBoltA","0 0 0 0 0 0 0 0 0 "@%pos, %player, 50);
		}
	}
	else
	{
		PlaySound("SoundPlasmaTurretFire",%pos);
		for(%i = 0; %i < floor(%energy); %i++) {
			Projectile::SpawnProjectile("NovaBoltB","0 0 0 0 0 0 0 0 0 "@%pos, %player, 50);
		}
	}

	GameBase::setEnergy(%player,0);
}
