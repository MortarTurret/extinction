$VehicleInvList[ScoutVehicle] = 1;
$DataBlockName[ScoutVehicle] = Scout;
$VehicleToItem[Scout] = ScoutVehicle;
$VehicleSlots[Scout] = 0;

ItemData ScoutVehicle 
{
	description = "Fighter";
	className = "Vehicle";
	heading = $InvHead[ihVeh];
	price = 600;
};

FlierData Scout 
{
	explosionId = LargeShockwave; 
	debrisId = flashDebrisLarge;
	className = "Vehicle";
	shapeFile = "flyer";
	shieldShapeName = "shield_medium";
	mass = 9.0;
	drag = 1.0;
	density = 1.2;
	maxBank = 1.3; 
	maxPitch = 1.3; 
	maxSpeed = 75; 
	minSpeed = -10;
	lift = 0.95;
	maxAlt = 20000; 
	maxVertical = 10; 
	maxDamage = 1.5; 
	damageLevel = {1.0, 1.0};
	maxEnergy = 250;
	accel = 1.5;
	groundDamageScale = 0.5;
	projectileType = ScoutMissile;
	reloadDelay = 2.5;
	repairRate = 0;
	fireSound = SoundFireFlierRocket;
	damageSound = SoundFlierCrash;
	ramDamage = 1.5;
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	visibleToSensor = true;
	shadowDetailMask = 2;
	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundFlyerIdle;
	moveSound = SoundFlyerActive;
	visibleDriver = true;
	driverPose = 22;
	description = "Fighter";
};

function Scout::onFire(%this)
{
	%client = gamebase::getcontrolclient(%this);
	%player = Client::getOwnedObject(%client);
	playSound(SoundFireFlierRocket,GameBase::getPosition(%player)); // -death666
}

function Scout::onPilot(%this, %player)
{
}

function Scout::onUnPilot(%this, %player)
{
}

// $VehicleWeapon[Scout, 0] = ScoutMissile;
// $VehicleWeapon[Scout, 1] = IonBolt;

function Scout::onDestroyed (%this,%mom)
{

			 %trans = GameBase::getMuzzleTransform(%this);
		 	 %vel = Item::getVelocity(%this);
			 Projectile::spawnProjectile("Smokegrenade",%trans,%this,%vel);
		 	 Projectile::spawnProjectile("Planedead1",%trans,%this,%vel);
			 playSound(SoundFlierCrash,GameBase::getPosition(%this));

	$TeamItemCount[GameBase::getTeam(%this) @ $VehicleToItem[GameBase::getDataName(%this)]]--;
	%cl = GameBase::getControlClient(%this);
	%pl = Client::getOwnedObject(%cl);
	%this.cloakable = "";
	%this.nuetron = "";
	if(%pl != -1)
	{
		Player::setMountObject(%pl, -1, 0);
		Client::setControlObject(%cl, %pl);
		if(%pl.lastWeapon != "")
		{
			Player::useItem(%pl,%pl.lastWeapon);
			%pl.lastWeapon = "";
		}
		%pl.driver = "";
		%pl.vehicle= "";
	}
	for(%i = 0 ; %i < 4 ; %i++)
		if(%this.Seat[%i] != "")
		{
			%pl = Client::getOwnedObject(%this.Seat[%i]);
			Player::setMountObject(%pl, -1, 0);
			Client::setControlObject(%this.Seat[%i], %pl);
			%pl.vehicleSlot = "";
			%pl.vehicle= "";
		}
	calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 2, 0.55, 0.1, 225, 100);
}