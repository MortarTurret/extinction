$VehicleInvList[LAPCVehicle] = 1;
$DataBlockName[LAPCVehicle] = LAPC;
$VehicleToItem[LAPC] = LAPCVehicle;
$VehicleSlots[LAPC] = 2;

ItemData LAPCVehicle 
{
	description = "Bomber";
	className = "Vehicle";
	heading = $InvHead[ihVeh];
	price = 675;
};

FlierData LAPC 
{
	explosionId = LargeShockwave; //flashExpLarge -death666
	debrisId = flashDebrisLarge;
	className = "Vehicle";
	shapeFile = "hover_apc_sml";
	shieldShapeName = "shield_large";
	mass = 18.0;
	drag = 1.0;
	density = 1.2;
	maxBank = 0.5;
	maxPitch = 0.5;
	maxSpeed = 60; 
	minSpeed = -3;
	lift = 0.5;
	maxAlt = 20000; //25
	maxVertical = 10; // 50 BR Setting
	maxDamage = 8.5; //BR Setting
	damageLevel = {1.0, 1.0};
	destroyDamage = 1.0;
	maxEnergy = 400; //BR Setting
	accel = 0.65; //BR Setting
	groundDamageScale = 0.50;
	repairRate = 0;
	ramDamage = 12; //BR Setting
	ramDamageType = -1;
	mapFilter = 2;
	mapIcon = "M_vehicle";
	//projectileType = MortarShell;
	fireSound = SoundMortarTurretFire;
	reloadDelay = 0.009; //BR Setting
	//minGunEnergy = 15;
	//maxGunEnergy = 200;
	damageSound = SoundTankCrash;
	visibleToSensor = true;
	shadowDetailMask = 2;
	mountSound = SoundFlyerMount;
	dismountSound = SoundFlyerDismount;
	idleSound = SoundFlyerIdle;
	moveSound = SoundFlyerActive;
	visibleDriver = true;
	driverPose = 23;
	description = "Bomber";
};

function LAPC::onPilot(%this, %player)
{
}

function LAPC::onUnPilot(%this, %player)
{
}

function LAPC::onFire(%this)
{		
	if($debug)
		echo("?? EVENT fire LAPC, "@ %this @" control cl# "@ gamebase::getcontrolclient(%this));

	if (%this.refire)
		return;
	
	%energy = GameBase::getEnergy(%this);
	if(%energy < 30)
		return;
	
	GameBase::setEnergy(%this,%energy - 30);
		
	%client = gamebase::getcontrolclient(%this);
	%player = Client::getOwnedObject(%client);
	%trans = GameBase::getMuzzleTransform(%this);
	%vel = Item::getVelocity(%this);
	
	%pos1=getWord(%trans,0);
	%pos2=getWord(%trans,1);
	%pos3=getWord(%trans,2);
	%pos4=getWord(%trans,3);
	%pos5=getWord(%trans,4);
	//%pos6=getWord(%trans,5);
	%pos6 = -0.5;
	%pos7=getWord(%trans,6);
	%pos8=getWord(%trans,7);
	%pos9=getWord(%trans,8);
	%pos10=getWord(%trans,9);
	%pos11=getWord(%trans,10);
	%pos12=getWord(%trans,11);
	
	%trans=%pos1@" "@%pos2@" "@%pos3@" "@%pos4@" "@%pos5@" "@%pos6@" "@%pos7@" "@%pos8@" "@%pos9@" "@%pos10@" "@%pos11@" "@%pos12;
	
	Projectile::spawnProjectile("BombShell",%trans,%this,%vel);

	playSound(SoundMortarTurretFire,GameBase::getPosition(%player)); // -death666

	%this.refire = true;
	schedule(%this @ ".refire = false;", 0.25,%this);
}

function LAPC::onDestroyed (%this,%mom)
{

			 %trans = GameBase::getMuzzleTransform(%this);
		 	 %vel = Item::getVelocity(%this);
			 Projectile::spawnProjectile("Smokegrenade",%trans,%this,%vel);
		 	 Projectile::spawnProjectile("Planedead2",%trans,%this,%vel);
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