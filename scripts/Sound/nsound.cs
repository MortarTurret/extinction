//----------------------------------------------------------------------------
// IMPORTANT: 3d voice profile must go first (if voices are allowed)
SoundProfileData Profile3dVoice
{
	baseVolume = 0;
	minDistance = 10.0;
	maxDistance = 70.0;
	flags = SFX_IS_HARDWARE_3D;
};
//----------------------------------------------------------------------------

SoundProfileData Profile2d
{
	baseVolume = 0.0;
};

SoundProfileData Profile2dLoop
{
	baseVolume = 0.0;
	flags = SFX_IS_LOOPING;
};

SoundProfileData Profile3dNear
{
	baseVolume = 0;
	minDistance = 5.0;
	maxDistance = 40.0;
	flags = SFX_IS_HARDWARE_3D;
};

SoundProfileData Profile3dMedium
{
	baseVolume = 0;
	minDistance = 8.0;
	maxDistance = 100.0;
	flags = SFX_IS_HARDWARE_3D;
};

SoundProfileData Profile3dFar
{
	baseVolume = 0;
	minDistance = 8.0;
	maxDistance = 500.0;
	flags = SFX_IS_HARDWARE_3D;
};

SoundProfileData Profile3dLudicrouslyFar
{
	baseVolume = 0;
	minDistance = 2.0;
	maxDistance = 700.0;
	flags = SFX_IS_HARDWARE_3D;
};

//For sounds that should be heard almost anywhere within the mission area. -Ghost
SoundProfileData Profile3dFarAsFuck 
{
	baseVolume = 0;
	minDistance = 2.0;
	maxDistance = 2000.0;
	flags = SFX_IS_HARDWARE_3D;
};

SoundProfileData Profile3dNearLoop
{
	baseVolume = 0;
	minDistance = 2.0;
	maxDistance = 40.0;
	flags = { SFX_IS_HARDWARE_3D, SFX_IS_LOOPING };
};

SoundProfileData Profile3dMediumLoop
{
	baseVolume = 0;
	minDistance = 2.0;
	maxDistance = 100.0;
	flags = { SFX_IS_HARDWARE_3D, SFX_IS_LOOPING };
};

SoundProfileData Profile3dFoot
{
	baseVolume = 0;
	minDistance = 2.0;
	maxDistance = 30.0;
	flags = SFX_IS_HARDWARE_3D;
};


//----------------------------------------------------------------------------
// sound data

// arena bot talk

SoundData AnniHelp
{
   wavFileName = "female2.whelp.wav";
   profile = Profile3dFar;
};

SoundData AnniTarget
{
   wavFileName = "female2.wtgtacq.wav";
   profile = Profile3dFar;
};

SoundData AnniAttack
{
   wavFileName = "female2.wattac2.wav";
   profile = Profile3dFar;
};

SoundData AnniOops
{
   wavFileName = "female2.woops1.wav";
   profile = Profile3dFar;
};

SoundData AnniYooHoo
{
   wavFileName = "female2.wtaunt1.wav";
   profile = Profile3dFar;
};

SoundData AnniGetSome
{
   wavFileName = "female2.wtaunt4.wav";
   profile = Profile3dFar;
};

SoundData AnniHi
{
   wavFileName = "female2.whello.wav";
   profile = Profile3dFar;
};

// end arena bot talk

SoundData SoundSniperCham
{
	wavFileName = "bxplo4.wav";
	profile = Profile3dFarAsFuck;
};

SoundData SoundPBeam
{
	wavFileName = "male1.wtgtacq.wav";
	profile = Profile3dMedium;
};

SoundData SoundLandOnGround
{
	wavFileName = "Land_On_Ground.wav";
	profile = Profile3dNear;
};

SoundData SoundPlayerDeath
{
	wavFileName = "player_death.wav";
	profile = Profile3dMedium;
};

SoundData SoundJetLight
{
	wavFileName = "thrust.wav";
	profile = Profile3dMediumLoop;
};

SoundData SoundJetHeavy
{
	wavFileName = "heavy_thrust.wav";
	profile = Profile3dMediumLoop;
};

SoundData SoundRain
{
	wavFileName = "rain.wav";
	profile = Profile2dLoop;
};

SoundData SoundSnow
{
	wavFileName = "snow.wav";
	profile = Profile2dLoop;
};

SoundData SoundWindAmbient
{
	wavFileName = "wind1.wav";
	profile = Profile2dLoop;
};

SoundData SoundWindGust
{
	wavFileName = "wind2.wav";
	profile = Profile3dNear;
};

SoundData SoundShellClick
{
	wavFileName = "shell_click.wav";
	profile = Profile2d;
};

SoundData SoundShellHilight
{
	wavFileName = "shell_hilite.wav";
	profile = Profile2d;
};

SoundData SoundDoorOpen
{
	wavFileName = "door1.wav";
	profile = Profile3dNear;
};

SoundData SoundDoorClose
{
	wavFileName = "door2.wav";
	profile = Profile3dNear;
};

SoundData ForceFieldOpen
{
	wavFileName = "ForceOpen.wav";
	profile = Profile3dNear;
};

SoundData ForceFieldClose
{
	wavFileName = "ForceClose.wav";
	profile = Profile3dNear;
};

SoundData SoundElevatorRun
{
	wavFileName = "generator.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundElevatorBlocked
{
	wavFileName = "turret_whir.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundElevatorStart
{
	wavFileName = "elevator1.wav";
	profile = Profile3dNear;
};

SoundData SoundElevatorStop
{
	wavFileName = "elevator2.wav";
	profile = Profile3dNear;
};

SoundData SoundBotRepairItem
{
   wavFileName = "repair.wav";
   profile = Profile3dNear;
};

//----------------------------------------------------------------------------
// foot sounds

SoundData SoundLFootRSoft
{
	wavFileName = "lfootrsoft.wav";
	profile = Profile3dFoot;
};

SoundData SoundLFootRHard
{
	wavFileName = "lfootrhard.wav";
	profile = Profile3dFoot;
};

SoundData SoundLFootRSnow
{
	wavFileName = "lfootrsnow.wav";
	profile = Profile3dFoot;
};

SoundData SoundLFootLSoft
{
	wavFileName = "lfootlsoft.wav";
	profile = Profile3dFoot;
};

SoundData SoundLFootLHard
{
	wavFileName = "lfootlhard.wav";
	profile = Profile3dFoot;
};

SoundData SoundLFootLSnow
{
	wavFileName = "lfootlsnow.wav";
	profile = Profile3dFoot;
};


SoundData SoundMFootRSoft
{
	wavFileName = "mfootrsoft.wav";
	profile = Profile3dFoot;
};

SoundData SoundMFootRHard
{
	wavFileName = "mfootrhard.wav";
	profile = Profile3dFoot;
};

SoundData SoundMFootRSnow
{
	wavFileName = "mfootrsnow.wav";
	profile = Profile3dFoot;
};

SoundData SoundMFootLSoft
{
	wavFileName = "mfootlsoft.wav";
	profile = Profile3dFoot;
};

SoundData SoundMFootLHard
{
	wavFileName = "mfootlhard.wav";
	profile = Profile3dFoot;
};

SoundData SoundMFootLSnow
{
	wavFileName = "mfootlsnow.wav";
	profile = Profile3dFoot;
};


SoundData SoundHFootRSoft
{
	wavFileName = "hfootrsoft.wav";
	profile = Profile3dFoot;
};

SoundData SoundHFootRHard
{
	wavFileName = "hfootrhard.wav";
	profile = Profile3dFoot;
};

SoundData SoundHFootRSnow
{
	wavFileName = "hfootrsnow.wav";
	profile = Profile3dFoot;
};

SoundData SoundHFootLSoft
{
	wavFileName = "hfootlsoft.wav";
	profile = Profile3dFoot;
};

SoundData SoundHFootLHard
{
	wavFileName = "hfootlhard.wav";
	profile = Profile3dFoot;
};

SoundData SoundHFootLSnow
{
	wavFileName = "hfootlsnow.wav";
	profile = Profile3dFoot;
};

//----------------------------------------------------------------------------

// SoundData SoundFallScream
// {
//	wavFileName = "fall_scream.wav";
//	profile = Profile3dNear;
// };

//----------------------------------------------------------------------------
// turret sound

SoundData SoundPlasmaTurretOn
{
	wavFileName = "turretOn4.wav";
	profile = Profile3dNear;
};

SoundData SoundPlasmaTurretOff
{
	wavFileName = "turretOff4.wav";
	profile = Profile3dNear;
};

SoundData SoundPlasmaTurretFire
{
	wavFileName = "turretFire4.wav";
	profile = Profile3dMedium;
};

SoundData SoundPlasmaTurretTurn
{
	wavFileName = "turretTurn4.wav";
	profile = Profile3dNear;
};


//
SoundData SoundChainTurretOn
{
	wavFileName = "turretOn1.wav";
	profile = Profile3dNear;
};

SoundData SoundChainTurretOff
{
	wavFileName = "turretOff1.wav";
	profile = Profile3dNear;
};

SoundData SoundChainTurretTurn
{
	wavFileName = "turretTurn1.wav";
	profile = Profile3dNear;
};

SoundData SoundChainTurretFire
{
	wavFileName = "machinegun.wav";
	profile = Profile3dMedium;
};

//
SoundData SoundMissileTurretOn
{
	wavFileName = "turretOn1.wav";
	profile = Profile3dNear;
};

SoundData SoundMissileTurretOff
{
	wavFileName = "turretOff1.wav";
	profile = Profile3dNear;
};

SoundData SoundMissileTurretTurn
{
	wavFileName = "turretTurn1.wav";
	profile = Profile3dNear;
};

SoundData SoundMissileTurretFire
{
	wavFileName = "turretFire1.wav";
	profile = Profile3dMedium;
};

//
SoundData SoundMortarTurretOn
{
	wavFileName = "turretOn2.wav";
	profile = Profile3dNear;
};

SoundData SoundMortarTurretOff
{
	wavFileName = "turretOff2.wav";
	profile = Profile3dNear;
};

SoundData SoundMortarTurretTurn
{
	wavFileName = "turretTurn2.wav";
	profile = Profile3dNear;
};

SoundData SoundMortarTurretFire
{
	wavFileName = "turretFire2.wav";
	profile = Profile3dMedium;
};

//
SoundData SoundEnergyTurretOn
{
	wavFileName = "turretOn4.wav";
	profile = Profile3dNear;
};

SoundData SoundEnergyTurretOff
{
	wavFileName = "turretOff4.wav";
	profile = Profile3dNear;
};

SoundData SoundEnergyTurretTurn
{
	wavFileName = "turretTurn4.wav";
	profile = Profile3dNear;
};

SoundData SoundEnergyTurretFire
{
	wavFileName = "rifle1.wav";
	profile = Profile3dMedium;
};

//
SoundData SoundRemoteTurretOn
{
	wavFileName = "turretOn2.wav";
	profile = Profile3dNear;
};

SoundData SoundRemoteTurretOff
{
	wavFileName = "turretOff2.wav";
	profile = Profile3dNear;
};

SoundData SoundRemoteTurretTurn
{
	wavFileName = "turretTurn2.wav";
	profile = Profile3dNear;
};

SoundData SoundRemoteTurretFire
{
	wavFileName = "rifle1.wav";
	profile = Profile3dMedium;
};


//----------------------------------------------------------------------------
// Item

SoundData SoundWeaponSelect
{
	wavFileName = "weapon5.wav";
	profile = Profile3dNear;
};

SoundData SoundFireBlaster
{
	wavFileName = "rifle1.wav";
	profile = Profile3dNear;
};

SoundData SoundFireChaingun
{
	wavFileName = "machinegun.wav";
	profile = Profile3dMediumLoop;
};

SoundData SoundSpinUp
{
	wavFileName = "Machgun3.wav";
	profile = Profile3dNear;
};

SoundData SoundSpinDown
{
	wavFileName = "Machgun2.wav";
	profile = Profile3dNear;
};

SoundData SoundDryFire
{
	wavFileName = "Dryfire1.wav";
	profile = Profile3dNear;
};

SoundData SoundFireGrenade
{
	wavFileName = "Grenade.wav";
	profile = Profile3dNear;
};

SoundData SoundFirePlasma
{
	wavFileName = "plasma2.wav";
	profile = Profile3dNear;
};

SoundData SoundSpinUpDisc
{
	wavFileName = "discspin.wav";
	profile = Profile3dNear;
};

SoundData SoundFireDisc
{
	wavFileName = "rocket2.wav";
	profile = Profile3dNear;
};

SoundData SoundDiscReload
{
	wavFileName = "discreload.wav";
	profile = Profile3dNear;
};

SoundData SoundDiscSpin
{
	wavFileName = "discloop.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundFireLaser
{
	wavFileName = "sniper.wav";
	profile = Profile3dNear;
};

SoundData SoundLaserHit
{
	wavFileName = "laserhit.wav";
	profile = Profile3dMedium;
};

SoundData SoundFireTargetingLaser
{
	wavFileName = "tgt_laser.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundLaserIdle
{
	wavFileName = "sniper2.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundTargetLaser
{
	wavFileName = "tgt_laser.wav";
	profile = Profile3dNear;
};

SoundData SoundFireMortar
{
	wavFileName = "mortar_fire.wav";
	profile = Profile3dNear;
};

SoundData SoundMortarIdle
{
	wavFileName = "mortar_idle.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundMortarReload
{
	wavFileName = "mortar_reload.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundFireSeeking
{
	wavFileName = "seek_fire.wav";
	profile = Profile3dNear;
};

SoundData SoundMineActivate
{
	wavFileName = "mine_act.wav";
	profile = Profile3dNear;
};

SoundData SoundFloatMineTarget
{
	wavFileName = "float_target.wav";
	profile = Profile3dNear;
};

SoundData SoundFloatMineTargetLoop
{
	wavFileName = "float_target.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundFireFlierRocket
{
	wavFileName = "flierrocket.wav";
	profile = Profile3dMedium;
};

SoundData SoundELFFire
{
	wavFileName = "elf_fire.wav";
	profile = Profile3dMediumLoop;
};
SoundData SoundFarELFFire
{
	wavFileName = "elf_fire.wav";
	profile = Profile3dMediumLoop;
};
SoundData SoundELFIdle
{
	wavFileName = "lightning_idle.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundEmpIdle
{
	wavFileName = "lightning_idle.wav";
	profile = Profile3dMedium;
};

SoundData SoundFireShotgun 
{
	wavFileName = "mine_exp.wav";
	profile = Profile3dNear;
};

SoundData SoundFireFlamer
{	wavFileName = "flyer_fly.wav";
	profile = Profile3dMedium;
};

SoundData SoundParticleBeamFire
{	wavFileName = "float_explode.wav";
	profile = Profile3dFar;
};

SoundData SoundParticleBeamRecharge
{	wavFileName = "targetlaser.wav";
	profile = Profile3dFar;
};


//----------------------------------------------------------------------------
// Inventory sounds

SoundData SoundPickupItem
{
	wavFileName = "Pku_weap.wav";
	profile = Profile3dNear;
};

SoundData SoundPickupHealth
{
	wavFileName = "Pku_hlth.wav";
	profile = Profile3dNear;
};

SoundData SoundPickupBackpack
{
	wavFileName = "Dryfire1.wav";
	profile = Profile3dNear;
};

SoundData SoundPickupWeapon
{
	wavFileName = "Pku_weap.wav";
	profile = Profile3dNear;
};

SoundData SoundPickupAmmo
{
	wavFileName = "Pku_ammo.wav";
	profile = Profile3dNear;
};

SoundData SoundActivatePDA
{
	wavFileName = "pda_on.wav";
	profile = Profile3dNear;
};

SoundData SoundPDAButtonHard
{
	wavFileName = "button_hard.wav";
	profile = Profile3dNear;
};

SoundData SoundPDAButtonSoft
{
	wavFileName = "button_soft.wav";
	profile = Profile3dNear;
};


//----------------------------------------------------------------------------
// Inventory equipment

SoundData SoundActivateAmmoStation
{
	wavFileName = "ammo_activate.wav";
	profile = Profile3dNear;
};

SoundData SoundUseAmmoStation
{
	wavFileName = "ammo_use.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundAmmoStationPower
{
	wavFileName = "ammo_power.wav";
	profile = Profile3dNear;
};

SoundData SoundAmmoStationLoopPower
{
	wavFileName = "ammo_power.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundFireAirstrike //added for airstrike fire sound -death666
{
	wavFileName = "ammo_use.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundKillstreak //added for killstreak sound -death666
{
	wavFileName = "male1.wtakcovr.wav";
	profile = Profile3dNear;
};

SoundData SoundActivateInventoryStation
{
	wavFileName = "inv_activate.wav";
	profile = Profile3dNear;
};

SoundData SoundUseInventoryStation
{
	wavFileName = "inv_use.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundInventoryStationPower
{
	wavFileName = "inv_power.wav";
	profile = Profile3dNear;
};

SoundData SoundActivateCommandStation
{
	wavFileName = "command_activate.wav";
	profile = Profile3dNear;
};

SoundData SoundUseCommandStation
{
	wavFileName = "command_use.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundCommandStationPower
{
	wavFileName = "command_power.wav";
	profile = Profile3dNear;
};

//----------------------------------------------------------------------------
// Item sounds

SoundData SoundGeneratorPower
{
	wavFileName = "generator.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundSnowLp
{
	wavFileName = "snow.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundMissileTurretTurnLp
{
	wavFileName = "turretTurn1.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundMortarTurretTurnLp
{
	wavFileName = "turretTurn2.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundActivateMotionSensor
{
	wavFileName = "motion_activate.wav";
	profile = Profile3dNear;
};

SoundData SoundSensorPower
{
	wavFileName = "pulse_power.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundTeleportPower
{
	wavFileName = "activateTele.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundTeleport
{
   wavFileName = "teleport2.wav";
   profile = Profile3dMedium;
};

SoundData SoundBeaconActive
{
	wavFileName = "activateBeacon.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundBeaconUse
{
	wavFileName = "teleport2.wav";
	profile = Profile3dNear;
};

SoundData SoundPackUse
{
	wavFileName = "usepack.wav";
	profile = Profile3dNear;
};

SoundData SoundPackFail
{
	wavFileName = "failpack.wav";
	profile = Profile3dNear;
};

SoundData SoundThrowItem
{
	wavFileName = "throwitem.wav";
	profile = Profile3dNear;
};

SoundData SoundShieldOn
{
	wavFileName = "shield_on.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundShieldHit
{
	wavFileName = "shieldhit.wav";
	profile = Profile3dNear;
};

SoundData SoundEnergyPackOn
{
	wavFileName = "energypackon.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundPBCharge
{
	wavFileName = "energypackon.wav";
	profile = Profile3dMedium;
};

SoundData SoundJammerOn
{
	wavFileName = "jammer_on.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundRepairItem
{
	wavFileName = "repair.wav";
	profile = Profile3dNearLoop;
};

SoundData SoundFlagCaptured
{
	wavFileName = "flagcapture.wav";
	profile = Profile3dMedium;
};

SoundData SoundFlagReturned
{
	wavFileName = "flagreturn.wav";
	profile = Profile3dMedium;
};

SoundData SoundFlagPickup
{
	wavFileName = "flag1.wav";
	profile = Profile3dMedium;
};

SoundData SoundFlagFlap
{
	wavFileName = "flagflap.wav";
	profile = Profile3dNear;
};

SoundData SoundDeploySensor
{
	wavFileName = "sensor_deploy.wav";
	profile = Profile3dNear;
};

SoundData SoundActiveSensor
{
	wavFileName = "sensor_active.wav";
	profile = Profile3dNear;
};

SoundData SoundTurretDeploy
{
	wavFileName = "rmt_turret.wav";
	profile = Profile3dNear;
};

SoundData SoundRadarDeploy
{
	wavFileName = "rmt_radar.wav";
	profile = Profile3dNear;
};

SoundData SoundCameraDeploy
{
	wavFileName = "rmt_camera.wav";
	profile = Profile3dNear;
};

//----------------------------------------------------------------------------
// Explosion Sounds

SoundData bigExplosion1
{
	wavFileName = "bxplo1.wav";
	profile = Profile3dFar;
};

SoundData bigExplosion2
{
	wavFileName = "bxplo2.wav";
	profile = Profile3dFar;
};

SoundData bigExplosion3
{
	wavFileName = "bxplo3.wav";
	profile = Profile3dFar;
};

SoundData bigExplosion4
{
	wavFileName = "bxplo4.wav";
	profile = Profile3dFar;
};

SoundData explosion3
{
	wavFileName = "explo3.wav";
	profile = Profile3dFar;
};

SoundData explosion4
{
	wavFileName = "explo4.wav";
	profile = Profile3dFar;
};

SoundData ricochet1
{
	wavFileName = "ricoche1.wav";
	profile = Profile3dNear;
};

SoundData ricochet2
{
	wavFileName = "ricoche2.wav";
	profile = Profile3dNear;
};

SoundData ricochet3
{
	wavFileName = "ricoche3.wav";
	profile = Profile3dNear;
};

SoundData energyExplosion
{
	wavFileName = "energyexp.wav";
	profile = Profile3dMedium;
};

SoundData rocketExplosion
{
	wavFileName = "rockexp.wav";
	profile = Profile3dLudicrouslyFar;
};

SoundData shockExplosion
{
	wavFileName = "shockexp.wav";
	profile = Profile3dLudicrouslyFar;
};


SoundData turretExplosion
{
	wavFileName = "turretexp.wav";
	profile = Profile3dMedium;
};

SoundData mineExplosion
{
	wavFileName = "mine_exp.wav";
	profile = Profile3dFar;
};

SoundData floatMineExplosion
{
	wavFileName = "float_explode.wav";
	profile = Profile3dFar;
};

SoundData debrisSmallExplosion
{
	wavFileName = "debris_small.wav";
	profile = Profile3dNear;
};

SoundData debrisMediumExplosion
{
	wavFileName = "debris_medium.wav";
	profile = Profile3dMedium;
};

SoundData debrisLargeExplosion
{
	wavFileName = "debris_large.wav";
	profile = Profile3dFar;
};

//----------------------------------------------------------------------------
// Vehicle Sounds

SoundData SoundFlyerMount
{
	wavFileName = "flyer_mount.wav";
	profile = Profile3dNear;
};

SoundData SoundFlyerDismount
{
	wavFileName = "flyer_dismount.wav";
	profile = Profile3dNear;
};

SoundData SoundFlyerActive
{
	wavFileName = "flyer_fly.wav";
	profile = Profile3dMediumLoop;
};

SoundData SoundFlyerIdle
{
	wavFileName = "flyer_idle.wav";
	profile = Profile3dMediumLoop;
};

SoundData SoundFlierCrash
{
	wavFileName = "crash.wav";
	profile = Profile3dMedium;
};

SoundData SoundTankMount
{
	wavFileName = "flyer_mount.wav";
	profile = Profile3dNear;
};

SoundData SoundTankDismount
{
	wavFileName = "flyer_dismount.wav";
	profile = Profile3dNear;
};

SoundData SoundTankActive
{
	wavFileName = "flyer_fly.wav";
	profile = Profile3dMediumLoop;
};

SoundData SoundTankIdle
{
	wavFileName = "flyer_idle.wav";
	profile = Profile3dMediumLoop;
};

SoundData SoundTankCrash
{
	wavFileName = "crash.wav";
	profile = Profile3dMedium;
};

SoundData FAaargh
{
	wavFileName = "female1.wdsgst4.wav";
	profile = Profile3dNear;
};

SoundData MAaargh
{
	wavFileName = "male4.wdsgst4.wav";
	profile = Profile3dNear;
};


SoundData FWooho
{
	wavFileName = "female2.wcheer2.wav";
	profile = Profile3dNear;
};


SoundData MWooho
{
	wavFileName = "male2.wcheer2.wav";
	profile = Profile3dNear;
};

// added for airbase deploy team message sound -death666
SoundData Fwaitpas
{
	wavFileName = "female2.wwaitpas.wav";
	profile = Profile3dNear;
};

// added for airbase deploy team message sound -death666
SoundData Mwaitpas
{
	wavFileName = "male2.wwaitpas.wav";
	profile = Profile3dNear;
};

// added for team generators destroyed sound -death666
SoundData Fneedrep
{
	wavFileName = "female2.wneedrep.wav";
	profile = Profile3dNear;
};

// added for team generators destroyed sound -death666
SoundData Mneedrep
{
	wavFileName = "male3.wneedrep.wav";
	profile = Profile3dNear;
};

// added for incoming airstrike sound -death666
SoundData Fhitdeck
{
	wavFileName = "female2.whitdeck.wav";
	profile = Profile3dNear;
};

SoundData MdepBcn
{
	wavFileName = "male2.wdepbecn.wav";
	profile = Profile3dNear;
};

SoundData F2Acknowledge
{
	wavFileName = "female2.wacknow";
	profile = Profile3dNear;
};

SoundData F3Acknowledge
{
	wavFileName = "female3.wacknow.wav";
	profile = Profile3dNear;
};

SoundData M4Acknowledge
{
	wavFileName = "male4.wacknow.wav";
	profile = Profile3dNear;
};

SoundData M1Acknowledge
{
	wavFileName = "male1.wacknow.wav";
	profile = Profile3dNear;
};

SoundData M5MoveOut
{
	wavFileName = "male5.wmoveout.wav";
	profile = Profile3dNear;
};

SoundData M4BeingAttack
{
	wavFileName = "male4.wbasatt.wav";
	profile = Profile3dNear;
};

SoundData F3HitDeck
{
	wavFileName = "female3.whitdeck.wav";
	profile = Profile3dNear;
};

SoundData M1TakeCover
{
	wavFileName = "male1.wtakcovr.wav";
	profile = Profile3dNear;
};

SoundData f5IncomingEnemies
{
	wavFileName = "female5.wincom2.wav";
	profile = Profile3dNear;
};

SoundData M4FireOnTarget
{
	wavFileName = "male4.wfiretgt.wav";
	profile = Profile3dNear;
};

SoundData F5CoverMe
{
	wavFileName = "female5.wcoverme.wav";
	profile = Profile3dNear;
};

SoundData F2CeaseFire
{
	wavFileName = "female2.wcease.wav";
	profile = Profile3dNear;
};

SoundData F5Cheer1
{
	wavFileName = "female5.wcheer1.wav";
	profile = Profile3dNear;
};

SoundData M4Cheer1
{
	wavFileName = "male4.wcheer1.wav";
	profile = Profile3dNear;
};

SoundData M4Cheer2
{
	wavFileName = "male4.wcheer2.wav";
	profile = Profile3dNear;
};

SoundData F3Cheer3
{
	wavFileName = "female3.wcheer3.wav";
	profile = Profile3dNear;
};

SoundData M4Taunt3
{
	wavFileName = "male4.wtaunt3.wav";
	profile = Profile3dNear;
};

SoundData F3Taunt1
{
	wavFileName = "female3.wtaunt1.wav";
	profile = Profile3dNear;
};

SoundData M3ArgDying
{
	wavFileName = "male3.wdsgst4.wav";
	profile = Profile3dNear;
};

SoundData M3Help
{
	wavFileName = "male3.whelp.wav";
	profile = Profile3dNear;
};

SoundData F1AhCrap
{
	wavFileName = "female1.wdoh.wav";
	profile = Profile3dNear;
};

SoundData Fgendes
{
	wavFileName = "female2.wgendes.wav";
	profile = Profile3dNear;
};

SoundData SoundShockNade
{
	wavFileName = "sniper2.wav";
	profile = Profile3dNear;
};

SoundData SoundFlyerBomb
{
	wavFileName = "flyer_idle.wav";
	profile = Profile3dMedium;
};

SoundData SoundForkRelease
{
	wavFileName = "turretTurn4.wav";
	profile = Profile3dNear;
};

