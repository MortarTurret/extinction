ExplosionData rocketExp
{
	shapeName = "bluex.dts";
	soundId = rocketExplosion;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 8.0;
	timeScale = 1.5;
	timeZero = 0.250;
	timeOne = 0.850;
	colors[0] = { 0.4, 0.4, 1.0 };
	colors[1] = { 1.0, 1.0, 1.0 };
	colors[2] = { 1.0, 0.95, 1.0 };
	radFactors = { 0.5, 1.0, 1.0 };
};

ExplosionData FlareExp
{
   shapeName = "breath.dts";
   soundId = ricochet3;
   faceCamera = true;
   randomSpin = true;
   hasLight   = true;
   lightRange = 1.0;

   timeScale = 0.2;

   timeZero = 0.0;
   timeOne  = 1.0;

   colors[0]  = { 1.0, 1.0, 1.0 };
   colors[1]  = { 0.5, 0.5, 0.5 };
   colors[2]  = { 0.05, 0.05, 0.05 };
   radFactors = { 0.0, 0.0, 1.0 };
};

ExplosionData WickedBadExp
{
	shapeName = "mortarex.dts";//fiery
	soundId = rocketExplosion;
	faceCamera = true;				
	randomSpin = true;		
	hasLight = true;			
	lightRange = 8.0;			
	timeZero = 0.300;			
	timeOne = 0.900;			
	colors[0] = {0.5,0.4,0.2};
	colors[1] = {1.0,1.0,0.5};
	colors[2] = {0.0,1.0,0.0};
	radFactors = {0.5,1.0,0.0};
	shiftPosition = false;


};

//changed it up -Ghost
ExplosionData SniperChamExp
{
	shapeName = "smoke.dts";
	soundId = ricochet1;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 1.0;
	timeZero = 0.100;
	timeOne = 0.900;
	colors[0] = { 0.0, 0.0, 0.0 };
	colors[1] = { 1.0, 1.0, 1.0 };
	colors[2] = { 1.0, 1.0, 1.0 };
	radFactors = { 0.0, 1.0, 0.0 };
	shiftPosition = true;
};

ExplosionData energyExp
{
	shapeName = "enex.dts";
	soundId = energyExplosion;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 3.0;
	timeZero = 0.450;
	timeOne = 0.750;
	colors[0] = { 0.25, 0.25, 1.0 };
	colors[1] = { 0.25, 0.25, 1.0 };
	colors[2] = { 1.0, 1.0, 1.0 };
	radFactors = { 1.0, 1.0, 1.0 };
	shiftPosition = True;
};
ExplosionData VulcanExp
{
   shapeName = "smoke.dts";	//shotgunex.dts";
   soundId   = energyExplosion;	//chainspk.dts;	//energyExplosion;

   faceCamera = true;
   randomSpin = true;
   hasLight   = true;
   lightRange = 3.0;

   timeZero = 0.450;
   timeOne  = 0.750;

   colors[0]  = { 1.0, 0.25, 0.25 };
   colors[1]  = { 1.0, 0.25, 0.25 };
   colors[2]  = { 1.0, 0.25, 0.25 };
   radFactors = { 1.0, 1.0, 1.0 };

   shiftPosition = True;
};

ExplosionData blasterExp
{
   shapeName = "shotgunex.dts";
   soundId   = energyExplosion;

   faceCamera = true;
   randomSpin = true;
   hasLight   = true;
   lightRange = 3.0;

   timeZero = 0.450;
   timeOne  = 0.750;

   colors[0]  = { 1.0, 0.25, 0.25 };
   colors[1]  = { 1.0, 0.25, 0.25 };
   colors[2]  = { 1.0, 0.25, 0.25 };
   radFactors = { 1.0, 1.0, 1.0 };

   shiftPosition = True;
};
ExplosionData plasmaExp
{
	shapeName = "plasmaex.dts";
	soundId = explosion4;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 4.0;
	timeZero = 0.200;
	timeOne = 0.950;
	colors[0] = { 1.0, 1.0, 0.0 };
	colors[1] = { 1.0, 1.0, 0.75 };
	colors[2] = { 1.0, 1.0, 0.75 };
	radFactors = { 0.375, 1.0, 0.9 };
};

ExplosionData grenadeExp
{
	shapeName = "fiery.dts";
	soundId = bigExplosion3;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 10.0;
	timeScale = 1.5;
	timeZero = 0.150;
	timeOne = 0.500;
	colors[0] = { 0.0, 0.0, 0.0 };
	colors[1] = { 1.0, 0.63, 0.0 };
	colors[2] = { 1.0, 0.63, 0.0 };
	radFactors = { 0.0, 1.0, 0.9 };
};

ExplosionData mineExp
{
	shapeName = "shockwave.dts";
	soundId = shockExplosion;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 8.0;
	timeScale = 1.5;
	timeZero = 0.0;
	timeOne = 0.500;
	colors[0] = { 0.0, 0.0, 0.0 };
	colors[1] = { 1.0, 1.0, 1.0 };
	colors[2] = { 1.0, 1.0, 1.0 };
	radFactors = { 0.0, 1.0, 1.0 };
};

ExplosionData mortarExp
{
	shapeName = "mortarex.dts";
	soundId = shockExplosion;
	faceCamera = true;
	randomSpin = false;
//	faceCamera = true;
//	randomSpin = true;
	hasLight = true;
	lightRange = 8.0;
	timeScale = 1.5;
	timeZero = 0.0;
	timeOne = 0.500;
	colors[0] = { 0.0, 0.0, 0.0 };
	colors[1] = { 1.0, 1.0, 1.0 };
	colors[2] = { 1.0, 1.0, 1.0 };
	radFactors = { 0.0, 1.0, 1.0 };
};

ExplosionData AgedonSplitExp //
{
   shapeName = "plasmaex.dts";
   soundId   = shockExplosion;//explosion4;//SoundFlakDet;//explosion4;//

   faceCamera = true;
   randomSpin = true;
   hasLight   = true;
   lightRange = 4.0;

   timeZero = 0.200;
   timeOne  = 0.950;

   colors[0]  = { 1.0, 1.0,  0.0 };
   colors[1]  = { 1.0, 1.0, 0.75 };
   colors[2]  = { 1.0, 1.0, 0.75 };
   radFactors = { 0.375, 1.0, 0.9 };
};

ExplosionData turretExp
{
	shapeName = "fusionex.dts";
	soundId = turretExplosion;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 3.0;
	timeZero = 0.450;
	timeOne = 0.750;
	colors[0] = { 0.25, 0.25, 1.0 };
	colors[1] = { 0.25, 0.25, 1.0 };
	colors[2] = { 1.0, 1.0, 1.0 };
	radFactors = { 1.0, 1.0, 1.0 };
};

ExplosionData burnExp
{
	shapeName = "plasmaex.dts";
	soundId = explosion4;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 4.0;
	timeZero = 0.9;
	timeOne = 0.250;
	colors[0] = { 1.0, 1.0, 0.0 };
	colors[1] = { 1.0, 1.0, 0.75 };
	colors[2] = { 1.0, 1.0, 0.75 };
	radFactors = { 0.375, 1.0, 0.9 };
};
ExplosionData RatPoisonExp
{
	shapeName = "chainspk.dts";
	//soundId = energyExplosion;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 3.0;
	timeZero = 0.450;
	timeOne = 0.750;
	colors[0] = { 1.0, 0.25, 0.25 };
	colors[1] = { 1.0, 0.25, 0.25 };
	colors[2] = { 1.0, 0.25, 0.25 };
	radFactors = { 1.0, 1.0, 1.0 };
	shiftPosition = True;
};

ExplosionData ShockGrenadeExp
{
	shapeName = "fusionex.dts";	//chainspk.dts";
	soundId = SoundFlierCrash;	//energyExplosion;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 3.0;
	timeZero = 0.450;
	timeOne = 0.750;
	colors[0] = { 1.0, 0.25, 0.25 };
	colors[1] = { 1.0, 0.25, 0.25 };
	colors[2] = { 1.0, 0.25, 0.25 };
	radFactors = { 1.0, 1.0, 1.0 };
	shiftPosition = True;
};

ExplosionData blasterExp2
{
   shapeName = "enex.dts";
   soundId   = energyExplosion;

   faceCamera = true;
   randomSpin = true;
   hasLight   = true;
   lightRange = 3.0;

   timeZero = 0.450;
   timeOne  = 0.750;

   colors[0]  = { 1.0, 0.25, 0.25 };
   colors[1]  = { 1.0, 0.25, 0.25 };
   colors[2]  = { 1.0, 0.25, 0.25 };
   radFactors = { 1.0, 1.0, 1.0 };

   shiftPosition = True;
};

ExplosionData blasterExp3
{
   shapeName = "paint.dts";
   soundId   = energyExplosion;

   faceCamera = true;
   randomSpin = true;
   hasLight   = true;
   lightRange = 3.0;

   timeZero = 0.450;
   timeOne  = 0.750;

   colors[0]  = { 1.0, 0.25, 0.25 };
   colors[1]  = { 1.0, 0.25, 0.25 };
   colors[2]  = { 1.0, 0.25, 0.25 };
   radFactors = { 1.0, 1.0, 1.0 };

   shiftPosition = True;
};

ExplosionData blasterExp4
{
   shapeName = "shotgunex.dts";
   soundId   = energyExplosion;

   faceCamera = true;
   randomSpin = true;
   hasLight   = true;
   lightRange = 3.0;

   timeZero = 0.450;
   timeOne  = 0.750;

   colors[0]  = { 1.0, 0.25, 0.25 };
   colors[1]  = { 1.0, 0.25, 0.25 };
   colors[2]  = { 1.0, 0.25, 0.25 };
   radFactors = { 1.0, 1.0, 1.0 };

   shiftPosition = True;
};

ExplosionData MiniGunEXP
{
	shapeName = "laserhit.dts";
	soundId = ricochet1;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 1.0;
	timeZero = 0.100;
	timeOne = 0.900;
	colors[0] = { 0.0, 0.0, 0.0 };
	colors[1] = { 1.0, 1.0, 1.0 };
	colors[2] = { 1.0, 1.0, 1.0 };
	radFactors = { 0.0, 1.0, 0.0 };
	shiftPosition = True;
};

ExplosionData bulletExp0
{
	shapeName = "chainspk.dts";
	soundId = ricochet1;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 1.0;
	timeZero = 0.100;
	timeOne = 0.900;
	colors[0] = { 0.0, 0.0, 0.0 };
	colors[1] = { 1.0, 1.0, 1.0 };
	colors[2] = { 1.0, 1.0, 1.0 };
	radFactors = { 0.0, 1.0, 0.0 };
	shiftPosition = True;
};

ExplosionData bulletExp1
{
	shapeName = "chainspk.dts";
	soundId = ricochet2;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 1.0;
	timeZero = 0.100;
	timeOne = 0.900;
	colors[0] = { 0.0, 0.0, 0.0 };
	colors[1] = { 1.0, 1.0, 0.5 };
	colors[2] = { 1.0, 1.0, 0.5 };
	radFactors = { 0.0, 1.0, 0.0 };
	shiftPosition = True;
};

ExplosionData bulletExp2
{
	shapeName = "chainspk.dts";
	soundId = ricochet3;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 1.0;
	timeZero = 0.100;
	timeOne = 0.900;
	colors[0] = { 0.0, 0.0, 0.0 };
	colors[1] = { 0.75, 1.0, 1.0 };
	colors[2] = { 0.75, 1.0, 1.0 };
	radFactors = { 0.0, 1.0, 0.0 };
	shiftPosition = True;
};

ExplosionData debrisExpSmall
{
	shapeName = "tumult_small.dts";
	soundId = debrisSmallExplosion;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 2.5;
	timeZero = 0.250;
	timeOne = 0.650;
	colors[0] = { 0.0, 0.0, 0.0 };
	colors[1] = { 1.0, 0.5, 0.16 };
	colors[2] = { 1.0, 0.5, 0.16 };
	radFactors = { 0.0, 1.0, 1.0 };
};

ExplosionData debrisExpMedium
{
	shapeName = "tumult_medium.dts";
	soundId = debrisMediumExplosion;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 3.5;
	timeZero = 0.250;
	timeOne = 0.650;
	colors[0] = { 0.0, 0.0, 0.0 };
	colors[1] = { 1.0, 0.5, 0.16 };
	colors[2] = { 1.0, 0.5, 0.16 };
	radFactors = { 0.0, 1.0, 1.0 };
};

ExplosionData debrisExpLarge
{
	shapeName = "tumult_large.dts";
	soundId = debrisLargeExplosion;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 5.0;
	timeZero = 0.250;
	timeOne = 0.650;
	colors[0] = { 0.0, 0.0, 0.0 };
	colors[1] = { 1.0, 0.5, 0.16 };
	colors[2] = { 1.0, 0.5, 0.16 };
	radFactors = { 0.0, 1.0, 1.0 };
};

ExplosionData flashExpSmall
{
	shapeName = "flash_small.dts";
	soundId = debrisSmallExplosion;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 2.5;
	timeZero = 0.250;
	timeOne = 0.650;
	colors[0] = { 0.0, 0.0, 0.0 };
	colors[1] = { 1.0, 0.5, 0.16 };
	colors[2] = { 1.0, 0.5, 0.16 };
	radFactors = { 0.0, 1.0, 1.0 };
};

ExplosionData flashExpMedium
{
	shapeName = "flash_medium.dts";
	soundId = debrisMediumExplosion;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 3.75;
	timeZero = 0.250;
	timeOne = 0.650;
	colors[0] = { 0.0, 0.0, 0.0 };
	colors[1] = { 1.0, 0.5, 0.16 };
	colors[2] = { 1.0, 0.5, 0.16 };
	radFactors = { 0.0, 1.0, 1.0 };
};

ExplosionData flashExpLarge
{
	shapeName = "flash_large.dts";
	soundId = debrisLargeExplosion;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 6.0;
	timeZero = 0.250;
	timeOne = 0.650;
	colors[0] = { 0.0, 0.0, 0.0 };
	colors[1] = { 1.0, 0.5, 0.16 };
	colors[2] = { 1.0, 0.5, 0.16 };
	radFactors = { 0.0, 1.0, 1.0 };
};

ExplosionData Shockwave
{
	shapeName = "shockwave.dts";
	soundId = shockExplosion;
	faceCamera = false;
	randomSpin = false;
	hasLight = true;
	lightRange = 6.0;
	timeZero = 0.250;
	timeOne = 0.650;
	colors[0] = { 0.0, 0.0, 0.0 };
	colors[1] = { 1.0, 0.5, 0.16 };
	colors[2] = { 1.0, 0.5, 0.16 };
	radFactors = { 0.0, 1.0, 1.0 };
};

ExplosionData LargeShockwave
{
	shapeName = "shockwave_large.dts";
	soundId = shockExplosion;
	faceCamera = false;
	randomSpin = false;
	hasLight = true;
	lightRange = 10.0;
	timeZero = 0.100;
	timeOne = 0.300;
	colors[0] = { 1.0, 1.0, 1.0 };
	colors[1] = { 1.0, 1.0, 1.0 };
	colors[2] = { 0.0, 0.0, 0.0 };
	radFactors = { 1.0, 0.5, 0.0 };
};

ExplosionData ShotExp 
{
	shapeName = "smoke.dts";
	soundId = ricochet3;
	faceCamera = true;
	randomSpin = true;
	hasLight = false;
	timeScale = 0.5;
	timeZero = 0.100;
	timeOne = 0.900;
	shiftPosition = false;
};

ExplosionData PhaseDisrupterExp
{
	shapeName = "shockwave_large.dts";
	soundId = shockExplosion;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 10.0;
	timeZero = 0.100;
	timeOne = 0.900;
	colors[0] = { 5.0, 6.0, 6.0 };
	colors[1] = { 1.0, 1.0, 9.0 };
	colors[2] = { 4.0, 6.0, 3.0 };
	radFactors = { 1.0, 0.5, 0.0 };
};

ExplosionData FlameExp
{
	shapeName = "fiery.dts";
	//soundId = SoundJetHeavy;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 10.0;
	timeScale = 1.5;
	timeZero = 0.150;
	timeOne = 0.500;
	colors[0] = { 0.0, 0.0, 0.0 };
	colors[1] = { 1.0, 0.63, 0.0 };
	colors[2] = { 1.0, 0.63, 0.0 };
	radFactors = { 0.0, 1.0, 0.9 };
};

ExplosionData SmokeFade
{
	shapeName = "breath.dts";
	//soundId = vertigo;
	faceCamera = false;
	randomSpin = false;
	hasLight = true;
	lightRange = 0.0;
	timeScale = 1.5;
	timeZero = 0;
	timeOne = 1;
	colors[0] = { 0.4, 0.4, 1.0 };
	colors[1] = { 1.0, 1.0, 1.0 };
	colors[2] = { 1.0, 0.95, 1.0 };
	radFactors = { 0.5, 1.0, 1.0 };
};

ExplosionData BlastExp0
{
	shapeName = "bluex.dts";
	soundId = rocketExplosion;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 8.0;
	timeScale = 1.5;
	timeZero = 0.250;
	timeOne = 0.850;
	colors[0] = { 0.4, 0.4, 1.0 };
	colors[1] = { 1.0, 1.0, 1.0 };
	colors[2] = { 1.0, 0.95, 1.0 };
	radFactors = { 0.5, 1.0, 1.0 };
};

ExplosionData fireExp
{
	shapeName = "plasmaex.dts";
	soundId = bigExplosion2;
	faceCamera = true;
	randomSpin = true;
	hasLight = true;
	lightRange = 4.0;
	timeZero = 0.200;
	timeOne = 0.950;
	colors[0] = { 1.0, 1.0, 0.0 };
	colors[1] = { 1.0, 1.0, 0.75 };
	colors[2] = { 1.0, 1.0, 0.75 };
	radFactors = { 0.375, 1.0, 0.9 };
};