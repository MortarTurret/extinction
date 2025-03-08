$VehicleInvList[TransportVehicle] = 1;	//0;
$DataBlockName[TransportVehicle] = Transport;
$VehicleToItem[Transport] = TransportVehicle;
$VehicleSlots[Transport] = 4;

ItemData TransportVehicle 
{
	description = "Transport";
	className = "Vehicle";
	heading = $InvHead[ihVeh];
	price = 875;
};

FlierData Transport
{
	explosionId = LargeShockwave; 
	debrisId = flashDebrisLarge;
	className = "Vehicle";
	shapeFile = "hover_apc";
	shieldShapeName = "shield_large";
	mass = 18.0;
	drag = 1.0;
	density = 1.2;
	maxBank = 0.4;
	maxPitch = 0.4;
	maxSpeed = 50; 
	minSpeed = -1;
	lift = 0.75;
	maxAlt = 20000; //15
	maxVertical = 10; // 50 BR Setting
	maxDamage = 8.0; //BR Setting
	damageLevel = {1.0, 1.0};
	maxEnergy = 100;
	accel = 0.50; //BR Setting
	groundDamageScale = 0.5;	//0.125;
	repairRate = 0;
	ramDamage = 2;
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	damageSound = SoundTankCrash;
	visibleToSensor = true;
	shadowDetailMask = 2;
	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundFlyerIdle;
	moveSound = SoundFlyerActive;
	visibleDriver = true;
	driverPose = 23;
	description = "Transport";
};

function Transport::onPilot(%this, %player)
{
}

function Transport::onUnPilot(%this, %player)
{
}

function Transport::onDestroyed (%this,%mom)
{

			 %trans = GameBase::getMuzzleTransform(%this);
		 	 %vel = Item::getVelocity(%this);
		 	 Projectile::spawnProjectile("Planedead3",%trans,%this,%vel);
		 	 Projectile::spawnProjectile("suicideShell",%trans,%this,%vel);
			 playSound(SoundFlierCrash,GameBase::getPosition(%this));

	$TeamItemCount[GameBase::getTeam(%this) @ $VehicleToItem[GameBase::getDataName(%this)]]--;
	%cl = GameBase::getControlClient(%this);
	%pl = Client::getOwnedObject(%cl);
		BotsHopOff(%cl);
		PilotHopOff(%cl);
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