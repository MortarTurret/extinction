// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// ARMOR WEAPON MAXs
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$MaxWeapons[armormDM] = 4;
$MaxWeapons[armorfDM] = 4;
$MaxWeapons[armormAngel] = 3;
$MaxWeapons[armorfAngel] = 3;
$MaxWeapons[armormSpy] = 3;
$MaxWeapons[armorfSpy] = 3;
$MaxWeapons[armormNecro] = 3;
$MaxWeapons[armorfNecro] = 3;
$MaxWeapons[armormWarrior] = 5;
$MaxWeapons[armorfWarrior] = 5;
$MaxWeapons[armormBuilder] = 4;
$MaxWeapons[armorfBuilder] = 4;
$MaxWeapons[armorTroll] = 5;
$MaxWeapons[armorTank] = 4;
$MaxWeapons[armorTitan] = 5;

function PopulateCanPilot(%APC, %mDM, %fDM, %mAN, %fAN, %mSP, %fSP, %mNE, %fNE, %mME, %fME, %mBU, %fBU, %TR, %TA, %TI)
{
	$VehicleUse[armormDM, %APC] = %mDM;
	$VehicleUse[armorfDM, %APC] = %fDM;
	$VehicleUse[armormAngel, %APC] = %mAN;
	$VehicleUse[armorfAngel, %APC] = %fAN;
	$VehicleUse[armormSpy, %APC] = %mSP;
	$VehicleUse[armorfSpy, %APC] = %fSP;
	$VehicleUse[armormNecro, %APC] = %mNE;
	$VehicleUse[armorfNecro, %APC] = %fNE;
	$VehicleUse[armormWarrior, %APC] = %mME;
	$VehicleUse[armorfWarrior, %APC] = %fME;
	$VehicleUse[armormBuilder, %APC] = %mBU;
	$VehicleUse[armorfBuilder, %APC] = %fBU;
	$VehicleUse[armorTroll, %APC] = %TR;
	$VehicleUse[armorTank, %APC] = %TA;
	$VehicleUse[armorTitan, %APC] = %TI;  
}

$CP = 1;
$CR = 2;

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// CAN PILOT                  	mDM,       fDM,       mAN,       fAN,       mSP,       fSP,       mNE,       fNE,       mME,       fME,       mBU,       fBU,        TR,        TA,  TI
// -=-=-=-=-=-=-=-=-=-=-=-=-=-	---        ---        ---        ---        ---        ---        ---        ---        ---        ---        ---        ---        ---        ---  ---
//PopulateCanPilot(Wraith,	$CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CR,       $CR,       $CR,       $CR, $CR);
PopulateCanPilot(Interceptor,	$CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CR,       $CR,       $CR,       $CR, $CR);
PopulateCanPilot(Scout,		$CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CR,       $CR,       $CR,       $CR, $CR);
PopulateCanPilot(LAPC,		$CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CR, $CR);
PopulateCanPilot(HAPC,		$CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CR, $CR);

PopulateCanPilot(Transport,	$CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CP | $CR, $CR, $CR);


function PopulateDamageScale(%damagetype, %mDM, %fDM, %mAN, %fAN, %mSP, %fSP, %mNE, %fNE, %mME, %fME, %mBU, %fBU, %TR, %TA, %TI, %h1, %vSc, %vIn, %vWr, %vLa, %vHa, %pSv, %pPr, %pSu, %wOs)
{
	$ServerKeyDamageScale = $ServerKeyDamageScale+%mDM+%fDM+%mAN+%fAN+%mSP+%fSP+%mNE+%fNE+%mME+%fME+%mBU+%fBU+%TR+%TA+%TI+%h1+%vSc+%vIn+%vWr+%vLa+%vHa+%pSv+%pPr+%pSu+%wOs;
	$DamageScale[armormDM, %damagetype] = %mDM;
	$DamageScale[armorfDM, %damagetype] = %fDM;
	$DamageScale[armormAngel, %damagetype] = %mAN;
	$DamageScale[armorfAngel, %damagetype] = %fAN;
	$DamageScale[armormSpy, %damagetype] = %mSP;
	$DamageScale[armorfSpy, %damagetype] = %fSP;
	$DamageScale[armormNecro, %damagetype] = %mNE;
	$DamageScale[armorfNecro, %damagetype] = %fNE;

	$DamageScale[lghost, %damagetype] = %mNE;
	$DamageScale[fghost, %damagetype] = %fNE;
	$DamageScale[armormWarrior, %damagetype] = %mME;
	$DamageScale[armorfWarrior, %damagetype] = %fME;
	$DamageScale[armormBuilder, %damagetype] = %mBU;
	$DamageScale[armorfBuilder, %damagetype] = %fBU;
	$DamageScale[armorTroll, %damagetype] = %TR;
	$DamageScale[armorTank, %damagetype] = %TA;
	$DamageScale[armorTitan, %damagetype] = %TI;  
	$DamageScale[harmor, %damagetype] = %h1;
	$DamageScale[Scout, %damagetype] = %vSc;
	$DamageScale[interceptor, %damagetype] = %vIn;
	$DamageScale[wraith, %damagetype] = %vWr;
	$DamageScale[lapc, %damagetype] = %vLa;
	$DamageScale[hapc, %damagetype] = %vHa;
	$DamageScale[SurveyDroid, %damagetype] = %pSv;
	$DamageScale[ProbeDroid, %damagetype] = %pPr;
	$DamageScale[SuicideDroid, %damagetype] = %pSu;
	$DamageScale[OSMissile, %damagetype] = %wOs;
}
//					     armormDM  ,  Angel  ,   Spy   ,  Necro  ,  Warrior, Builder, Troll,Tank,Titn,harm,Figh,intr,Wrai,lapc,hapc,Surv,probe,sui,OSMis
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// DAMAGE SCALE                                mDM, fDM, mAN, fAN, mSP, fSP, mNE, fNE, mME, fME, mBU, fBU,  TR,  TA,  TI,  h1,  vSc, vIn, vWr, vLa, vHa, pSv, pPr, pSu, wOS
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---
PopulateDamageScale($ImpactDamageType,         1.0, 1.0, 1.2, 1.2, 0.6, 0.6, 0.8, 0.8, 1.0, 1.0, 1.0, 1.0, 0.7, 0.5, 0.7, 1.0, 2.0, 2.0, 2.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($LandingDamageType,        1.0, 1.0, 0.0, 0.0, 0.8, 0.8, 0.8, 0.8, 1.0, 1.0, 0.9, 0.9, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($BulletDamageType,         1.0, 1.0, 0.1, 0.1, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 0.8, 0.8, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($MinigunDamageType,         1.0, 1.0, 0.1, 0.1, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 0.8, 0.8, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($EnergyDamageType,         1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 0.5, 0.5, 1.0, 1.0, 0.7, 0.7, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($PlasmaDamageType,         1.0, 1.0, 1.2, 1.2, 1.0, 1.0, 0.8, 0.8, 1.0, 1.0, 1.0, 1.0, 1.0, 0.9, 0.8, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($ExplosionDamageType,      1.0, 1.0, 0.7, 0.7, 0.9, 0.9, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 0.8, 0.7, 0.8, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($ShrapnelDamageType,       1.0, 1.0, 1.5, 1.5, 0.8, 0.8, 0.9, 0.9, 1.0, 1.0, 1.0, 1.0, 0.6, 0.8, 0.7, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($LaserDamageType,          1.0, 1.0, 1.0, 1.0, 0.8, 0.8, 0.9, 0.9, 1.1, 1.1, 1.0, 1.0, 1.1, 1.0, 1.2, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($MortarDamageType,         1.0, 1.0, 0.5, 0.5, 0.8, 0.8, 1.0, 1.0, 1.0, 1.0, 0.9, 0.9, 0.8, 0.7, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($BlasterDamageType,        1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($ElectricityDamageType,    1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 0.8, 0.8, 1.0, 1.0, 1.2, 1.2, 1.2, 1.4, 1.2, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($CrushDamageType,          1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($DebrisDamageType,         1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 0.8, 0.8, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($MissileDamageType,        1.0, 1.0, 1.2, 1.2, 1.0, 1.0, 0.8, 0.8, 1.0, 1.0, 1.0, 1.0, 1.0, 0.8, 0.8, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($OSMissileDamageType,      1.0, 1.0, 1.5, 1.5, 1.0, 1.0, 0.7, 0.7, 1.0, 1.0, 1.0, 1.0, 0.8, 0.5, 0.8, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($MineDamageType,           1.0, 1.0, 0.7, 0.7, 0.4, 0.4, 0.4, 0.4, 1.0, 1.0, 1.0, 1.0, 0.7, 0.6, 0.5, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($SniperDamageType,         1.0, 1.0, 1.0, 1.0, 0.8, 0.8, 1.0, 1.0, 1.0, 1.0, 0.9, 0.9, 0.8, 0.9, 0.8, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($ShockDamageType,          1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 0.7, 0.7, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($ShotgunDamageType,        1.0, 1.0, 1.0, 1.0, 0.7, 0.7, 0.5, 0.5, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($SoulDamageType,           1.0, 1.0, 0.8, 0.8, 1.0, 1.0, 0.7, 0.7, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($AssassinDamageType,       1.0, 1.0, 1.0, 1.0, 0.2, 0.2, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($DisarmDamageType,         1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($VortexTurretDamageType,   1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);

// changed TurretVortexDamageType to VortexTurretDamageType -death666

PopulateDamageScale($PoisonDamageType,         1.0, 1.0, 0.8, 0.8, 1.0, 1.0, 0.9, 0.9, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($TrollPlasmaDamageType,    1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.5, 1.5, 0.2, 1.5, 1.5, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
//new for Pitchfork
PopulateDamageScale($ForkImpact,	       1.0, 1.0, 1.2, 1.2, 0.6, 0.6, 0.8, 0.8, 1.0, 1.0, 1.0, 1.0, 1.0, 0.5, 0.7, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);

PopulateDamageScale($RocketDamageType,         1.0, 1.0, 1.2, 1.2, 1.0, 1.0, 0.8, 0.8, 1.0, 1.0, 1.0, 1.0, 0.8, 0.8, 0.8, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
PopulateDamageScale($JettingDamage,            1.0, 1.0, 0.5, 0.5, 1.0, 1.0, 0.5, 0.5, 1.0, 1.0, 0.7, 0.7, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);

//					     armormDM  ,  Angel  ,   Spy   ,  Necro  ,  Warrior, Builder ,Trol,Tank,Titn,harm,Figh,intr,Wrai,lapc,hapc,Surv,probe,sui,OSMis
$Console::Prompt = ">>";	

function PopulateItemMax(%item, %mDM, %fDM, %mAN, %fAN, %mSP, %fSP, %mNE, %fNE, %mME, %fME, %mBU, %fBU, %TR, %TA, %TI, %h1, %h2, %h3)
{
	$serverKeyItemMax = $serverKeyItemMax+%mDM+%fDM+%mAN+%fAN+%mSP+%fSP+%mNE+%fNE+%mME+%fME+%mBU+%fBU+%TR+%TA+%TI+%h1+%h2+%h3;
	$serverKeyItemReload = $serverKeyItemReload+ %item.imageType.reloadTime;
	$serverKeyItemFire = $serverKeyItemFire + %item.imageType.fireTime;
	$serverKeyItemType = $serverKeyItemType + %item.imageType.weaponType;

	$ItemMax[armormDM, %item] = %mDM;
	$ItemMax[armorfDM, %item] = %fDM;
	$ItemMax[armormAngel, %item] = %mAN;
	$ItemMax[armorfAngel, %item] = %fAN;
	$ItemMax[armormSpy, %item] = %mSP;
	$ItemMax[armorfSpy, %item] = %fSP;
	$ItemMax[armormNecro, %item] = %mNE;
	$ItemMax[armorfNecro, %item] = %fNE;

	$ItemMax[lghost, %item] = %mNE;
	$ItemMax[fghost, %item] = %fNE;		
	$ItemMax[armormWarrior, %item] = %mME;
	$ItemMax[armorfWarrior, %item] = %fME;
	$ItemMax[armormBuilder, %item] = %mBU;
	$ItemMax[armorfBuilder, %item] = %fBU;
	$ItemMax[armorTroll, %item] = %TR;
	$ItemMax[armorTank, %item] = %TA;
	$ItemMax[armorTitan, %item] = %TI;  
}

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// WEAPONS                                     mDM, fDM, mAN, fAN, mSP, fSP, mNE, fNE, mME, fME, mBU, fBU,  TR,  TA,  TI
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---
PopulateItemMax(AngelFire,			0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0); //AngelRepairGun
PopulateItemMax(AngelRepairGun,			0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
PopulateItemMax(BabyNukeMortar,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1);
PopulateItemMax(BabyNukeAmmo,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   4);
PopulateItemMax(GateGun,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
//3.0
//PopulateItemMax(ClayPigeon,			1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   1,   0,   1);
//PopulateItemMax(ClayPigeonShells,		100,  100,  0,   0,   0,   0,   0,   0,  100,  100,  100,  100,  100,   0,  200);
PopulateItemMax(DiscLauncher,			1,   1,   0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   1,   0,   1);
PopulateItemMax(DiscAmmo,			30,  30,  0,   0,  30,  30,   0,   0,  30,  30,  30,  30,  30,   0,  15);
PopulateItemMax(Fixit,				0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
PopulateItemMax(Fixit2,				0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
PopulateItemMax(Flamer,			1,   1,   0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   1,   0,   1);
PopulateItemMax(FlamerAmmo,			50,  50,  0,   0,  40,  40,   0,   0,  50,  50,  50,  50,  75,   0, 100);
PopulateItemMax(FlameThrower,			1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   1,   0,   1);
PopulateItemMax(FlameThrowerAmmo,		50,  50,  0,   0,   0,   0,   0,   0,  50,  50,   0,   0,  100,   0, 100);

//PopulateItemMax(GravityGun,			1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1); // now doing this in adminset
PopulateItemMax(GrapplingHook,			0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
PopulateItemMax(LaserRifle,			0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   1,   0,   0);
// PopulateItemMax(Buckler,			0,   0,   1,   1,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0);

PopulateItemMax(Grabbler,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
PopulateItemMax(GrenadeLauncher,		1,   1,   0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   1,   0,   1);
PopulateItemMax(GrenadeAmmo,			20,  20,  0,   0,  15,  15,   0,   0,  20,  20,  15,  15,  30,   0,  30);
//New TA Gun!!!!!!
PopulateItemMax(Thumper,		1,   1,   0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   1,   0,   1);
PopulateItemMax(ThumperAmmo,			20,  20,  0,   0,  15,  15,   0,   0,  20,  20,  15,  15,  30,   0,  20);
//New base weapons for arena/lt
//PopulateItemMax(GrenadeLauncherBase,		1,   1,   0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   1,   0,   1); 
//PopulateItemMax(GrenadeAmmoBase,			20,  20,  0,   0,  15,  15,   0,   0,  20,  20,  15,  15,  30,   0,  30);
//PopulateItemMax(PlasmaGunElite,			1,   1,   0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   1,   0,   1);
//PopulateItemMax(PlasmaAmmoElite,			30,  30,  0,   0,  25,  25,   0,   0,  30,  30,  30,  30,  40,   0,  40);
//PopulateItemMax(BlasterBase,		1,   1,   0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   1,   0,   1);

PopulateItemMax(Hammer,				1,   1,   0,   0,   1,   1,   1,   1,   1,   1,   0,   0,   0,   0,   0);
PopulateItemMax(HammerAmmo,			30,  30,  0,   0,  10,  10,  10,  10,  15,  15,   0,   0,   0,   0,   0);
//PopulateItemMax(HDiscLauncher,			1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   1,   0,   1);
//PopulateItemMax(HDiscLauncherAmmo,		30,  30,  0,   0,   0,   0,   0,   0,  30,  30,  30,  30,  30,   0,  40);
PopulateItemMax(HeavensFury,			0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
PopulateItemMax(Jailgun,			1,   1,   0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   0,   0,   0);

PopulateItemMax(Lamer,			1,   1,   0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   1,   0,   1);
PopulateItemMax(LamerAmmo,			30,  30,  0,   0,  25,  25,   0,   0,  30,  30,  30,  30,  40,   0,  40);
PopulateItemMax(MineLauncher,			1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   0,   0,   1);
PopulateItemMax(MineLauncherAmmo,		5,   5,   0,   0,   0,   0,   0,   0,   5,   5,   5,   5,   0,   0,  10);
PopulateItemMax(Mortar,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0);
PopulateItemMax(MortarAmmo,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  20,   0,   0); 
PopulateItemMax(OSLauncher,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1);
PopulateItemMax(OSAmmo,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   8);
PopulateItemMax(ParticleBeamWeapon,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1);
PopulateItemMax(ParticleBeamShells,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1);
PopulateItemMax(PhaseDisrupter,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0,   1);
PopulateItemMax(PhaseAmmo,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  20,   0,  20);
PopulateItemMax(PlasmaGun,			1,   1,   0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   1,   0,   1);
PopulateItemMax(PlasmaAmmo,			30,  30,  0,   0,  25,  25,   0,   0,  30,  30,  30,  30,  40,   0,  40);
PopulateItemMax(Railgun,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
PopulateItemMax(RailAmmo,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  18,  18,   0,   0,   0); //15
PopulateItemMax(RocketLauncher,		1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   1,   0,   1);
PopulateItemMax(RocketAmmo,			10,  10,  0,   0,   0,   0,   0,   0,  10,  10,  10,  10,  20,   0,  20);


// PopulateItemMax(RocketPod,			1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   1,   0,   1);
// PopulateItemMax(RocketPodShells,		6,  6,  0,   0,   0,   0,   0,   0,  6,  6,  6,  6,  6,   0,  6);

PopulateItemMax(RubberMortar,			1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   1,   0,   1);
PopulateItemMax(RubberAmmo,			10,  10,  0,   0,   0,   0,   0,   0,  10,  10,   0,   0,  15,   0,  20);
PopulateItemMax(ShockwaveGun,			1,   1,   0,   0,   1,   1,   0,   0,   1,   1,   0,   0,   0,   0,   0);
PopulateItemMax(Shotgun,			1,   1,   0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   1,   0,   0);
PopulateItemMax(ShotGunShells,			30,  30,  0,   0,  30,  30,   0,   0,  30,  30,  30,  30,  30,   0,   0);
PopulateItemMax(SniperRifle,			0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0);
PopulateItemMax(SniperAmmo,			0,   0,   0,   0,  15,  15,   0,   0,   0,   0,   0,   0,   0,   0,   0);
PopulateItemMax(SoulSucker,			0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
PopulateItemMax(Harpoon,			0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
PopulateItemMax(HarpoonAmmo,			0,   0,   15,   15,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
PopulateItemMax(Stinger,			1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   1,   0,   1);
PopulateItemMax(StingerAmmo,			10,  10,  0,   0,   0,   0,   0,   0,   8,   8,   8,   8,  10,   0,   10);
PopulateItemMax(TargetingLaser,			1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1);
PopulateItemMax(TankShredder,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0);
PopulateItemMax(TankShredderAmmo,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0, 500,   0);
PopulateItemMax(TRocketLauncher,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0);
PopulateItemMax(TRocketLauncherAmmo,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  40,   0);
PopulateItemMax(TankRPGLauncher,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0);
PopulateItemMax(TankRPGAmmo,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  60,   0);
PopulateItemMax(TBlastCannon,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0);
PopulateItemMax(TBlastCannonAmmo,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  30,   0);
PopulateItemMax(Vulcan,					1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   0,   0,   0);
PopulateItemMax(VulcanAmmo,				400, 400, 0,   0,   0,   0,   0,   0, 400, 400, 400, 400, 0,   0,   0);
PopulateItemMax(Minigun,					0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0);
PopulateItemMax(MinigunAmmo,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  500,   0,  0);

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// SPELLS                                      mDM, fDM, mAN, fAN, mSP, fSP, mNE, fNE, mME, fME, mBU, fBU,  TR,  TA,  TI
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---
PopulateItemMax(DeathRay,			0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0);
PopulateItemMax(DisarmerSpell,			0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0);
PopulateItemMax(ShockingGrasp,			0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0);
PopulateItemMax(Stasis,			0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0);
PopulateItemMax(Elf,				0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0);
PopulateItemMax(FlameStrike,			0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0);  
PopulateItemMax(SpellFlameThrower,		0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0);

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// MISC                                        mDM, fDM, mAN, fAN, mSP, fSP, mNE, fNE, mME, fME, mBU, fBU,  TR,  TA,  TI
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---
PopulateItemMax(flag,				1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1);  
PopulateItemMax(ArenaFlag,			1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1);  
PopulateItemMax(Beacon,				1,   1,   5,   5,   3,   3,   3,   3,   5,   5,   1,   1,   3,   3,   3);  
PopulateItemMax(Grenade,			8,   8,   5,   5,   5,   5,   5,   5,   8,   8,   8,   8,   8,   8,   8);
PopulateItemMax(MineAmmo,			3,   3,   3,   3,   3,   3,   3,   3,   3,   3,   3,   3,   5,   3,   5);
PopulateItemMax(RepairKit,			1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1); //added repair kits to angel -death666

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// PACKS                                       mDM, fDM, mAN, fAN, mSP, fSP, mNE, fNE, mME, fME, mBU, fBU,  TR,  TA,  TI
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---
PopulateItemMax(AirStrikePack,			0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0); //adding the airstrikepack -death666
PopulateItemMax(AmmoPack,			1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   0,   0,   0);
PopulateItemMax(EnergyPack,			1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1);
PopulateItemMax(RepairPack,			1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1);
PopulateItemMax(ShieldPack,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1); //removed from necromancer. removed from warrior. -death666 3.18.17 //- Removed from Troll (Mortar Turret, 2024/03/21)
//PopulateItemMax(BuilderPack,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
PopulateItemMax(SensorJammerPack,		1,   1,   1,   1,   0,   0,   0,   0,   1,   1,   1,   1,   0,   1,   1); //removed from necromancer -death666. added fBU 4.6.17 -death666
PopulateItemMax(PhaseShifterPack,		0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0);
//PopulateItemMax(CloakingDevice,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
PopulateItemMax(StealthShieldPack,		1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0);
PopulateItemMax(Laptop,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);  
PopulateItemMax(SightPack,			0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
PopulateItemMax(NovaPack,			0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
PopulateItemMax(SuicidePack,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0);
PopulateItemMax(RegenerationPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   0,   0);
PopulateItemMax(ChameleonPack,		0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0);
PopulateItemMax(SurveyDroidPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
PopulateItemMax(ProbeDroidPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
PopulateItemMax(SuicideDroidPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);

PopulateItemMax(ghostpack,			0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0);

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// SENSORS                                     mDM, fDM, mAN, fAN, mSP, fSP, mNE, fNE, mME, fME, mBU, fBU,  TR,  TA,  TI
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---
PopulateItemMax(CameraPack,			0,   0,   1,   1,   0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   1);
PopulateItemMax(DeployableSensorJammerPack,	0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   1,   1,   1,   1,   1);
PopulateItemMax(MotionSensorPack,		0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   1,   1,   1,   1,   1);
PopulateItemMax(CatPack,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
PopulateItemMax(PulseSensorPack,		0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   1,   1,   1,   1,   1);

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// TURRETS                                     mDM, fDM, mAN, fAN, mSP, fSP, mNE, fNE, mME, fME, mBU, fBU,  TR,  TA,  TI
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---
PopulateItemMax(FusionTurretPack,		1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   1,   0,   0);
PopulateItemMax(LaserTurretPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
PopulateItemMax(RocketPack,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   1,   0,   0);
PopulateItemMax(PlasmaTurretPack,		0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   1,   0,   0);
PopulateItemMax(MortarTurretPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);  

PopulateItemMax(NeuroTurretPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);  
PopulateItemMax(ShockTurretPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
//PopulateItemMax(MMTurretPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
PopulateItemMax(NuclearTurretPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
PopulateItemMax(FlameTurretPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
//PopulateItemMax(RailTurretPack,		1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
PopulateItemMax(IrradiationTurretPack,		1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   0,   0,   1);
PopulateItemMax(TurretPack,			0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   0,   0,   1);
PopulateItemMax(VortexTurretPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
//PopulateItemMax(ParticleBeamTurretPack,		1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   0,   0,   1);

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// BARRIERS                                    mDM, fDM, mAN, fAN, mSP, fSP, mNE, fNE, mME, fME, mBU, fBU,  TR,  TA,  TI
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---
PopulateItemMax(BigCratePack,			0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   0,   0,   1);
PopulateItemMax(BlastWallPack,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
PopulateItemMax(ForceFieldPack,			1,   1,   1,   1,   0,   0,   1,   1,   1,   1,   1,   1,   1,   1,   1);
PopulateItemMax(ForceFieldDoorPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
PopulateItemMax(LargeForceFieldPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
PopulateItemMax(LargeForceFieldDoorPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
PopulateItemMax(PlatformPack,			0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   0,   0,   1);
PopulateItemMax(PlasmafloorPack,		0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   0,   0,   1);

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// DEPLOYABLES                          mDM, fDM, mAN, fAN, mSP, fSP, mNE, fNE, mME, fME, mBU, fBU,  TR,  TA,  TI
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---
PopulateItemMax(AcceleratorDevicePack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
// PopulateItemMax(BaseCloakPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
PopulateItemMax(ControlJammerPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
//PopulateItemMax(DeployableAmmoPack,		1,   1,   0,   0,   1,   1,   0,   0,   1,   1,   1,   1,   1,   0,   1);
//PopulateItemMax(DeployableComPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   1);
PopulateItemMax(DeployableInvPack,		1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   0,   0,   0);
PopulateItemMax(InterceptorPack,		0,   0,   1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0); //FighterPack -death666
PopulateItemMax(JailTower,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
PopulateItemMax(JumpPadPack,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
//PopulateItemMax(MannequinPack,		1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1);
//PopulateItemMax(Plastique,			1,   1,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
//PopulateItemMax(ProbePack,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
PopulateItemMax(SpringPack,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
// PopulateItemMax(LaserWallPack,		1,   1,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0,   0);
PopulateItemMax(TeleportPack,			1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   0,   0,   0);
PopulateItemMax(TransportPack,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// POWER SYSTEMS                               mDM, fDM, mAN, fAN, mSP, fSP, mNE, fNE, mME, fME, mBU, fBU,  TR,  TA,  TI
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---
PopulateItemMax(MobileInventoryPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0); 
PopulateItemMax(PortableGeneratorPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0); //removed from mBU and fBU -death666
PopulateItemMax(PortableSolarPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
// PopulateItemMax(PowerNodePack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
// PopulateItemMax(SatSystemPack,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
// PopulateItemMax(ShieldGenPack,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
// PopulateItemMax(ShieldNodePack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// REMOTE BASES                                mDM, fDM, mAN, fAN, mSP, fSP, mNE, fNE, mME, fME, mBU, fBU,  TR,  TA,  TI
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---  ---
PopulateItemMax(AirBasePack,			1,   1,   0,   0,   0,   0,   0,   0,   1,   1,   1,   1,   0,   0,   0);
//PopulateItemMax(BaseOpsPack,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   0);
PopulateItemMax(BunkerPack,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   0,   0,   1);
//PopulateItemMax(CommandShipPack,		0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);
PopulateItemMax(GunShipPack,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   1,   1,   1);
//PopulateItemMax(SupplyShipPack,			0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0);


// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// TEAM BARRIERS MAXs
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$TeamItemMax[BigCratePack] = 20;
$TeamItemMax[BlastWallPack] = 20;
$TeamItemMax[ForceFieldPack] = 16;
$TeamItemMax[ForceFieldDoorPack] = 8;
$TeamItemMax[LargeForceFieldPack] = 8;
$TeamItemMax[LargeForceFieldDoorPack] = 4;
$TeamItemMax[PlatformPack] = 20;
$TeamItemMax[PlasmafloorPack] = 20;

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// TEAM DEPLOYABLES MAXs
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$TeamItemMax[AcceleratorDevicePack] = 2;
// $TeamItemMax[BaseCloakPack] = 1;
$TeamItemMax[ControlJammerPack] = 1;
//$TeamItemMax[DeployableAmmoPack] = 10;
//$TeamItemMax[DeployableComPack] = 5;
$TeamItemMax[DeployableInvPack] = 10;
$TeamItemMax[TeleportPack] = 3; // 2
$TeamItemMax[JailTower] = 1;
$TeamItemMax[SpringPack] = 2;
// $TeamItemMax[LaserWallPack] = 2;
$TeamItemMax[JumpPadPack] = 5; 
$TeamItemMax[InterceptorPack] = 5;	
$TeamItemMax[TransportPack] = 5;	

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// TEAM DRONES MAXs
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$TeamItemMax[SurveyDroidPack] = 3;
$TeamItemMax[ProbeDroidPack] = 3;
$TeamItemMax[SuicideDroidPack] = 3;

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// TEAM POWER SYSTEMS MAXs
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$TeamItemMax[MobileInventoryPack] = 5;
$TeamItemMax[PortableGeneratorPack] = 1;
$TeamItemMax[PortableSolarPack] = 1;
// $TeamItemMax[PowerNodePack] = 20;
// $TeamItemMax[SatSystemPack] = 1;
// $TeamItemMax[ShieldGenPack] = 1;
// $TeamItemMax[ShieldNodePack] = 20;

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// TEAM REMOTE BASES MAXs
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//$TeamItemMax[BaseOpsPack] = 1; //Borg Cube
//$TeamItemMax[DeployableBaseOps] = 1; //Borg Cube
$TeamItemMax[AirBasePack] = 1;
$TeamItemMax[BunkerPack] = 1;
//	$TeamItemMax[CommandShipPack] = 1;
	$TeamItemMax[GunShipPack] = 1;
//	$TeamItemMax[SupplyShipPack] = 1;

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// TEAM SENSORS MAXs
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
//$TeamItemMax[BeaconAmmo] = 20;
$TeamItemMax[CameraPack] = 10;
$TeamItemMax[DeployableSensorJammerPack] = 10;
$TeamItemMax[MotionSensorPack] = 10;
$TeamItemMax[PulseSensorPack] = 10;

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// TEAM TURRETS MAXs
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$TeamItemMax[AirStrikePack] = 10;
$TeamItemMax[FlameTurretPack] = 6;
$TeamItemMax[FusionTurretPack] = 4;
$TeamItemMax[TurretPack] = 10;
$TeamItemMax[IrradiationTurretPack] = 2; 
$TeamItemMax[LaserTurretPack] = 16;
//$TeamItemMax[MMTurretPack] = 2;
$TeamItemMax[MortarTurretPack] = 2;
$TeamItemMax[NuclearTurretPack] = 2;
//$TeamItemMax[ParticleBeamTurretPack] = 2;
$TeamItemMax[PlasmaTurretPack] = 8;
//$TeamItemMax[RailTurretPack] = 4;
$TeamItemMax[RocketPack] = 3; 
$TeamItemMax[ShockTurretPack] = 8;
$TeamItemMax[VortexTurretPack] = 6;

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// TEAM VEHICLES MAXs
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
$TeamItemMax[ScoutVehicle] = 5;
$TeamItemMax[InterceptorVehicle] = 4;
$TeamItemMax[TransportVehicle] = 3;
$TeamItemMax[LAPCVehicle] = 3;
$TeamItemMax[HAPCVehicle] = 3;

// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// TEAM BOTS MAX
// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// $BotsArenaMax = 100;