$InvList[MineAmmo] = 1;
$MobileInvList[MineAmmo] = 1;
$RemoteInvList[MineAmmo] = 1;
AddItem(MineAmmo);

$SellAmmo[MineAmmo] = 5;
$TeamItemMax[MineAmmo] = 35;

//$InvList[ReplicatingMine] = 1;
//$RemoteInvList[ReplicatingMine] = 1;

addAmmo("", MineAmmo, 1);

function oldMine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object) 
{
	if(%type == $MineDamageType) 
		%value = %value * 0.25;
	%damageLevel = GameBase::getDamageLevel(%this);
	GameBase::setDamageLevel(%this,%damageLevel + %value);
}

function Mine::Detonate(%this) 
{
	%data = GameBase::getDataName(%this);
	GameBase::setDamageLevel(%this, %data.maxDamage);
}

//function MineAmmo::onUse(%player,%item)
//{
//	if($matchStarted)
//	{
//		if(%player.throwTime < getSimTime() )
//		{
//
//			if(Player::getItemCount(%player, OriginalMine))
//			{
//				%name = "AntipersonelMine";
//				%item = OriginalMine;
//			}
//			else if(Player::getItemCount(%player, ReplicatingMine))
//			{
//				%name = "ReplicatorMine";
//				%item = ReplicatingMine;
//			}
//			else if(Player::getItemCount(%player, FlagMine))
//			{
//				%name = "FakeFlag";
//				%item = FlagMine;
//			}
//
//			if(!$build)Player::decItemCount(%player,%item);
//			Player::decItemCount(%player, MineAmmo);
//			%obj = newObject("","Mine",%name);
//			%obj.cloakable = true;	//for base cloaker
//			addToSet("MissionCleanup", %obj);
//			%client = Player::getClient(%player);
//			GameBase::throw(%obj,%player,15 * %client.throwStrength,false);
//			%player.throwTime = getSimTime() + 0.5;
//			GameBase::setTeam(%obj,GameBase::getTeam(%client));
//		}
//	}
//}

 //-=-=-=-

MineData HandgrenadeLT 
{
   mass = 0.3;
   drag = 1.0;
   density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
   description = "Handgrenade";
   shapeFile = "grenade";
   shadowDetailMask = 4;
   explosionId = grenadeExp;
	explosionRadius = 10.0;
	damageValue = 0.5;
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 2;
};

MineData Handgrenade 
{
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "grenade";
	shadowDetailMask = 4;
	explosionId = grenadeExp;
	explosionRadius = 10.0;
	damageValue = 1.0; //BR Setting
	damageType = $ShrapnelDamageType;
	kickBackStrength = 100;
	triggerRadius = 0.5;
	maxDamage = 2.0;
	
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;	
	
};

function Handgrenade::onAdd(%this) 
{
	schedule("Mine::Detonate(" @ %this @ ");",2.0,%this);
}
 
function Handgrenade::onRemove(%this)
{
	//do nothing yet..
}


 //-=-=-=- MineAmmo

ItemData MineAmmo 
{
	description = "Mine";
	shapeFile = "mineammo";
	heading = $InvHead[ihMis];
	shadowDetailMask = 4;
	price = 10;
	className = "HandAmmo";
};

function MineAmmo::onUse(%player,%item) 
{
	if($matchStarted) 
	{
		%target = Player::getclient(%player);
		if(%target.RGrapple)
		{
			%target.AGrapple = true;
			schedule(%target @".AGrapple = false;", 15);
			return;
		}
		// adding this to prevent the mine exploit -death666
		if ($jailed[%player] == true)
		{
			Client::sendMessage(%target,0, "Unable to place mines while jailed. ~wC_BuySell.wav");
			return;
		}
		
		if ( Player::getItemCount(%player,%item) > 0 ) 
		{
			Player::getClient(%player).TMinesDropped++;
			
			GameBase::playSound(%player, SoundThrowItem,0);
			if(%player.throwTime < getSimTime() ) 
			{
				if(!$build)Player::decItemCount(%player,%item);
				%armor = Player::getArmor(%player);
				%client = Player::getClient(%player);
				if($Deathmatch)
					%obj = newObject("","Mine","DMMine");
				else 
				{
					%obj = newObject("","Mine","antipersonelMine");
					GameBase::setTeam (%obj,GameBase::getTeam (%client));
				}
				%obj.cloakable = true;	//for base cloaker
				addToSet("MissionCleanup", %obj);
				GameBase::throw(%obj,%player,15 * %client.throwStrength,false);
				%player.throwTime = getSimTime() + 0.5;
			}
		}
	}
}

 //-=-=-=- 

MineData AntipersonelMine 
{		
	className = "Mine";
	description = "Antipersonel Mine";
	shapeFile = "mine";
	shadowDetailMask = 4;
	explosionId = mineExp;
	explosionRadius = 10.0;
	damageValue = 0.65;
	damageType = $MineDamageType;
	kickBackStrength = 150;
	triggerRadius = 2.5;
	maxDamage = 0.1;
	shadowDetailMask = 0;
	destroyDamage = 1.0;
	damageLevel = {1.0, 1.0};
};

function AntipersonelMine::onAdd(%this) 
{
	%this.damage = 0;
	GameBase::setMapName(%this,"Mine");
	AntipersonelMine::deployCheck(%this);
}

function AntipersonelMine::onRemove(%this)
{
	//do nothing yet..
}


function AntipersonelMine::onCollision(%this,%object) 
{	
	%armor = Player::getArmor(%object);
	%client = Player::getClient(%object);
	if(%armor == armormAngel || %armor == armorfAngel) 
	{
		if(GameBase::getTeam(%this)!=GameBase::getTeam(%object)) 
		{
		Client::sendMessage(%client,1,"Enemy mine averted. ~wthrowitem.wav");
		return;
		}
	}
	
	if($debug) 
		event::collision(%this,%object);

	if(%this.cloaked > 0 && getObjectType(%object) == "Player"){
		GameBase::startFadein(%this);	
		%this.cloaked = "";
		}
	%type = getObjectType(%object);
	%data = GameBase::getDataName(%this);
	if( ( (%type == "Player" || %data == AntipersonelMine || %data == Vehicle) && GameBase::isActive(%this) && (GameBase::getTeam(%this)!=GameBase::getTeam(%object)) ) || %type == "Moveable") 
		GameBase::setDamageLevel(%this, 90);
}

// default annihilation mine
function AntipersonelMine::deployCheck(%this) 
{
	if ( GameBase::getDataName(%this) != AntipersonelMine )
		return;
	if(GameBase::isAtRest(%this)) 
	{
		//GameBase::playSound(%this, SoundMineActivate,0.5);
		GameBase::playSequence(%this,1,"deploy");
		GameBase::setActive(%this,true);
		%set = newObject("set",SimSet);
		if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),1,1,1,0)) 
		{
			%data = GameBase::getDataName(%this);
			GameBase::setDamageLevel(%this, 90);
		}
		deleteObject(%set);
	}
	else 
		schedule("AntipersonelMine::deployCheck(" @ %this @ ");", 3, %this);
}

function AntipersonelMine::onDestroyed(%this) 
{
	$TeamItemCount[GameBase::getTeam(%this) @ "MineAmmo"]--; 
	%this.cloakable = "";
	%this.nuetron = "";
}

function AntipersonelMine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object) 
{
	if($debug::Damage)
	{
		Anni::Echo("AntipersonelMine::onDamage("@%this@", "@%type@", "@%value@", "@%pos@", "@%vec@", "@%mom@", "@%object@" )");
	}	
	if(%value <= 0) return;	
	if(%type == $MineDamageType) 
		%value = %value * 0.25;
	%data = GameBase::getDataName(%this);
	if((%data.maxDamage/1.5) < %this.damage+%value) 
		GameBase::setDamageLevel(%this, %data.maxDamage);
	else 
		%this.damage += %value;
}

 //-=-=-=-

MineData DMMine 
{
	className = "Mine";
	description = "Antipersonel Mine";
	shapeFile = "mine";
	shadowDetailMask = 4;
	explosionId = mineExp;
	explosionRadius = 10.0;
	damageValue = 0.75;
	damageType = $ShrapnelDamageType;
	kickBackStrength = 250;
	triggerRadius = 2.5;
	maxDamage = 0.75;
	shadowDetailMask = 0;
	destroyDamage = 1.0;
	damageLevel = {1.0, 1.0};
};

function DMMine::onAdd(%this) 
{
	%this.damage = 0;
	DMMine::deployCheck(%this);
}
 
function DMMine::onRemove(%this)
{
	//do nothing yet..
}

MineData HoloMine 
{
	className = "Mine";
	description = "Hologram";
	shapeFile = "larmor";
	shadowDetailMask = 4;
	explosionId = mineExp;
	explosionRadius = 15.0;
	damageValue = 0.75;
	damageType = $MineDamageType;
	kickBackStrength = 350;
	triggerRadius = 2.5;
	maxDamage = 2.00;
	shadowDetailMask = 0;
	destroyDamage = 1.0;
	damageLevel = {1.0, 1.0};
};

function HoloMine::onAdd(%this,%clientId) 
{
	%this.damage = 0;
	HoloMine::deployCheck(%this);
	%data = GameBase::getDataName(%this);
	Client::sendMessage(%clientId,0,"You summon a mirror image of yourself");
	schedule("Mine::Detonate(" @ %this @ ");",70.0,%this); 
}

function HoloMine::onRemove(%this)
{
	//do nothing yet..
}


function HoloMine::onCollision(%this,%object) 
{	
	if($debug) 
	  Anni::Echo("?? EVENT collision "@GameBase::getDataName(%this)@" contacted by "@GameBase::getDataName(%object)@" control cl# "@GameBase::getControlClient(%object));

	if(%this.cloaked > 0 && getObjectType(%object) == "Player"){
		GameBase::startFadein(%this);
		%this.cloaked = "";
		}	
	%type = getObjectType(%object);
	%data = GameBase::getDataName(%this);
	if((%type == "Player" || %data == HoloMine|| %data == Vehicle ) && GameBase::isActive(%this) && (GameBase::getTeam(%this)!=GameBase::getTeam(%object)) || %type == "Moveable" ) GameBase::setDamageLevel(%this, %data.maxDamage);
}

function HoloMine::onUse(%clientId) 
{
Client::sendMessage(%clientId,0,"You summon a mirror image of yourself");
}

function HoloMine::deployCheck(%this) 
{
	 if(GameBase::isAtRest(%this))
	{
		GameBase::setActive(%this,true);
		%set = newObject("set",SimSet);
		if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),1,1,1,0)) 
		{
			%data = GameBase::getDataName(%this);
			GameBase::setDamageLevel(%this, %data.maxDamage);
		}
		deleteObject(%set);
	}
	else schedule("HoloMine::deployCheck(" @ %this @ ");", 3, %this);
}

function HoloMine::onDestroyed(%this) 
{
	if($debug::Damage)
	{
		Anni::Echo("HoloMine::onDamage("@%this@", "@%type@", "@%value@", "@%pos@", "@%vec@", "@%mom@", "@%object@" )");
	}	
	$TeamItemCount[GameBase::getTeam(%this) @ "hologram"]--;
	%this.cloakable = "";
	%this.nuetron = "";
}

function HoloMine::onDamage(%this,%type,%value,%pos,%vec,%mom,%object) 
{
	if(%value <= 0) return;	
	if(%type == $MineDamageType) %value = %value * 0.25;
	%data = GameBase::getDataName(%this);
	if((%data.maxDamage/1.5) < %this.damage+%value) GameBase::setDamageLevel(%this, %data.maxDamage);
	else %this.damage += %value;
}

 //-=-=-=-

MineData Hologram 
{
	className = "Mine";
	description = "Antipersonel Mine";
	shapeFile = "flag";
	shadowDetailMask = 4;
	explosionId = mineExp;
	explosionRadius = 5.0;
	damageValue = 0.75;
	damageType = $MineDamageType;
	kickBackStrength = 150;
	triggerRadius = 0.0;
	maxDamage = 3.00;
	shadowDetailMask = 0;
	destroyDamage = 2.0;
	damageLevel = {1.0, 1.0};
	lightType = 2;
	lightRadius = 4;
	lightTime = 1.5;
	lightColor = {1, 1, 1 };
};

function Hologram::onAdd(%this) 
{
	%this.damage = 0;
	Boost::deployCheck(%this); // ???
}

function Hologram::onCollision(%this,%object) 
{	
	if($debug) 
	  Anni::Echo("?? EVENT collision "@GameBase::getDataName(%this)@" contacted by "@GameBase::getDataName(%object)@" control cl# "@GameBase::getControlClient(%object));

	if(%this.cloaked > 0 && getObjectType(%object) == "Player"){
		%this.cloaked = "";
		GameBase::startFadein(%this);	}
	%type = getObjectType(%object);
	%data = GameBase::getDataName(%this);
	if(((%type == "Player" || %data == Hologram || %data == Vehicle ) && GameBase::isActive(%this) ) || %type == "Moveable") GameBase::setDamageLevel(%this, %data.maxDamage);
}

function Hologram::deployCheck(%this) 
{
	if(GameBase::isAtRest(%this)) 
	{
		GameBase::playSequence(%this,1,"deploy");
		GameBase::setActive(%this,true);
		%set = newObject("set",SimSet);
		if(1 != containerBoxFillSet(%set,$MineObjectType,GameBase::getPosition(%this),1,1,1,0)) 
		{
			%data = GameBase::getDataName(%this);
			GameBase::setDamageLevel(%this, %data.maxDamage);
		}
		deleteObject(%set);
	}
	else schedule("Hologram::deployCheck(" @ %this @ ");", 3, %this);
}

function Hologram::onDestroyed(%this)
{
	%this.cloakable = "";
	%this.nuetron = "";
}

function Hologram::onDamage(%this,%type,%value,%pos,%vec,%mom,%object) 
{
	if($debug::Damage)
	{
		Anni::Echo("Hologram::onDamage("@%this@", "@%type@", "@%value@", "@%pos@", "@%vec@", "@%mom@", "@%object@" )");
	}	
	if(%value <= 0) return;	
	if(%type == $ShrapnelDamageType) %value = %value * 0.25;
	%data = GameBase::getDataName(%this);
	if((%data.maxDamage/1.5) < %this.damage+%value) GameBase::setDamageLevel(%this, %data.maxDamage);
	else %this.damage += %value;
}

 //-=-=-=-

MineData Tranqgrenade 
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "grenade";
	shadowDetailMask = 4;
	explosionId = Shockwave;
	explosionRadius = 10.0;
	damageValue = 0.15;
	damageType = $EnergyDamageType;
	kickBackStrength = 0;
	triggerRadius = 0.5;
	maxDamage = 2.0;
};

function Tranqgrenade::onAdd(%this) 
{
	schedule("Mine::Detonate(" @ %this @ ");",2.0,%this);
}

 //-=-=-=-

MineData Shockgrenade 
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "grenade";
	shadowDetailMask = 4;
	explosionId = ShockGrenadeExp;	
	explosionRadius = 20.0;
	damageValue = 0.2;
	damageType = $ShockDamageType;
	kickBackStrength = 50;
	triggerRadius = 0.5;
	maxDamage = 2.0;
};

function Shockgrenade::onAdd(%this) 
{
	schedule("Mine::Detonate(" @ %this @ ");",2.0,%this);
}
 
function Shockgrenade::onRemove(%this)
{
	//do nothing yet..
}

MineData Nukebomb 
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.0;
	friction = 99.0;
	className = "Handgrenade";
	description = "Plastique";
	shapeFile = "sensor_small";
	shadowDetailMask = 4;
	explosionId = rocketExp;
	explosionRadius = 7.5;
	damageValue = 4.0; 
	damageType = $MortarDamageType;
	kickBackStrength = 400;
	triggerRadius = 0.5;
	maxDamage = 3.0;
	lightType = 2; 
	lightRadius = 2;
	lightTime = 0.1;
	lightColor = { 1, 0, 0};
};

function Nukebomb::onAdd(%this) 
{
	schedule("Mine::Detonate(" @ %this @ ");",15.0,%this);
}

function Nukebomb::onRemove(%this)
{
	//do nothing yet..
}


function Nukebomb::onCollision(%this,%obj) 
{	
	if($debug) 
		event::collision(%this,%obj);

	if(%this.cloaked > 0 && getObjectType(%object) == "Player"){
		GameBase::startFadein(%this);	
		%this.cloaked = "";
		}

	if ( getObjectType(%obj) == Elevator )
		Mine::Detonate(%this);
		
	if(getObjectType(%obj) != "Player" || Player::isDead(%obj)) 
		return;

	%clientId = Player::getClient(%obj);
	%playerTeam = GameBase::getTeam(%obj);
	%teleTeam = GameBase::getTeam(%this);
	%armor = Player::getArmor(%obj);
	if(%armor == "armormBuilder" || %armor == "armorfBuilder") 
	{
		%rnd = floor(getRandom() * 10);
		if(%rnd > 8.5) 
		{
			Client::sendMessage(%clientId,1,"OOPS! You cut the wrong wire...");
			Mine::Detonate(%this);
			return;
		}
		else 
		{
			deleteObject(%this);
			Client::sendMessage(%clientId,1,"You disarm the Plastique Explosive.");
		}
	}
}

// Deployed if Player wearing SuicidePack actually commits suicide
MineData Suicidebomb 
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "magcargo";
	shadowDetailMask = 4;
	explosionId = LargeShockwave;
	explosionRadius = 50.0;
	damageValue = 4.0;
	damageType = $ShrapnelDamageType;
	kickBackStrength = 300;
	triggerRadius = 0.5;
	maxDamage = 2.0;
};

function Suicidebomb::onAdd(%this) 
{
	schedule("Mine::Detonate(" @ %this @ ");",0.5,%this);
}
 
function Suicidebomb::onRemove(%this)
{
	//do nothing yet..
}


// Deployed in normal manner with pack key
MineData Suicidebomb2 
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "mortarpack"; 
	shadowDetailMask = 4;
	explosionId = LargeShockwave;
	explosionRadius = 70.0;
	damageValue = 4.0;
	damageType = $ShrapnelDamageType;
	kickBackStrength = 300;
	triggerRadius = 0.5;
	maxDamage = 2.0;
	lightType = 2;  
	lightRadius = 2;
	lightTime = 0.1;
	lightColor = { 1, 0, 0};
};

// Override base class
function Suicidebomb2::onAdd(%this) 
{
}
 
function Suicidebomb2::onRemove(%this)
{
	//do nothing yet..
}


function Suicidebomb2::onCollision(%this,%obj) 
{	
	if($debug) 
	  Anni::Echo("?? EVENT collision "@GameBase::getDataName(%this)@" contacted by "@GameBase::getDataName(%obj)@" control cl# "@GameBase::getControlClient(%obj));
	
	if(%this.cloaked > 0 && getObjectType(%object) == "Player"){
		GameBase::startFadein(%this);
		%this.cloaked = "";
	}

	if ( getObjectType(%obj) == Elevator )
		Mine::Detonate(%this);
		
	if(getObjectType(%obj) != "Player" || Player::isDead(%obj)) 
		return;

	%clientId = Player::getClient(%obj);
	%playerTeam = GameBase::getTeam(%obj);
	%teleTeam = GameBase::getTeam(%this);
	%armor = Player::getArmor(%obj);
	if(%armor == "armormBuilder" || %armor == "armorfBuilder") 
	{
		%rnd = floor(getRandom() * 10);
		if(%rnd > 8) 
		{
			Client::sendMessage(%clientId,1,"OOPS! You cut the wrong wire...");
			Mine::Detonate( %this );
			return;
		}
		else 
		{
			deleteObject(%this);
			Client::sendMessage(%clientId,1,"You disarm the DetPack.");
		}
	}
}

 //-=-=-=-

MineData Mortarbomb 
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "sensor_small"; //"grenade";
	shadowDetailMask = 4;
	explosionId = mortarExp;
	explosionRadius = 20.0;
	damageValue = 1.0;
	damageType = $MortarDamageType;
	kickBackStrength = 250;
	triggerRadius = 0.5;
	maxDamage = 2.0;
};

function Mortarbomb::onAdd(%this) 
{
	schedule("Mine::Detonate(" @ %this @ ");",2.0,%this);
}
 
function Mortarbomb::onRemove(%this)
{
	//do nothing yet..
}


MineData Clusterbombmine 
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "plasammo"; //"grenade";
	shadowDetailMask = 4;
	explosionId = mortarExp;
	explosionRadius = 25.0;
	damageValue = 0.5;
	damageType = $MortarDamageType;
	kickBackStrength = 350;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};

function Clusterbombmine::onAdd(%this) 
{
	schedule("Mine::Detonate(" @ %this @ ");",2.0,%this);
}

function Clusterbombmine::onRemove(%this)
{
	//do nothing yet..
}

MineData TankBomb
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Tank Bomb";
	shapeFile = "mortarammo";
	shadowDetailMask = 4;
	explosionId = LargeShockwave;
	explosionRadius = 30.0;
	damageValue = 0.8;
	damageType = $MortarDamageType;
	kickBackStrength = 400;
	triggerRadius = 0.5;
	maxDamage = 1.0;
};

function TankBomb::onAdd(%this) 
{
	schedule("Mine::Detonate(" @ %this @ ");",2.0,%this);
}
 
function TankBomb::onRemove(%this)
{
	//do nothing yet..
}


MineData Firebomb 
{
	mass = 0.3;
	drag = 1.0;
	density = 2.0;
	elasticity = 0.15;
	friction = 1.0;
	className = "Handgrenade";
	description = "Handgrenade";
	shapeFile = "grenade";
	shadowDetailMask = 4;
	explosionId = flashExpLarge;
	explosionRadius = 30.0;
	damageValue = 0.1;
	damageType = $ExplosionDamageType;
	kickBackStrength = 600;
	triggerRadius = 0.5;
	maxDamage = 2.0;
};

function Firebomb::onAdd(%this) 
{
	schedule("Mine::Detonate(" @ %this @ ");",2.0,%this);
}
 
function Firebomb::onRemove(%this)
{
	//do nothing yet..
}

ExplosionData EMPPulseExp 
{ 
    shapeName = "shockwave_large.dts"; 
    soundId = shockExplosion; 
    faceCamera = true; 
    randomSpin = true; 
    hasLight = true; 
    lightRange = 40.0; 
    timeScale = 0.5; 
    colors[0] = { 0.0, 0.0, 1.0 }; 
    colors[1] = { 0.0, 0.0, 1.0 }; 
    colors[2] = { 0.0, 0.0, 1.0 }; 
    radFactors = { 0.2, 0.6, 1.0 }; 
};

GrenadeData EMPGrenadeShell 
{ 
    bulletShapeName = "mortar.dts"; 
    explosionTag = EMPPulseExp; 
    collideWithOwner = True; 
    ownerGraceMS = 250; 
    collisionRadius = 0.3; 
    mass = 1.0; 
    elasticity = 0.45; 
    damageClass = 1; 
    damageValue = 0.04; 
    damageType = $ShockDamageType; 
    explosionRadius = 26; 
    kickBackStrength = -100.0; 
    maxLevelFlightDist = 275; 
    totalTime = 30.0; 
    liveTime = 0.25; 
    projSpecialTime = 0.05; 
    inheritedVelocityScale = 0.5; 
    smokeName = "fusionbolt.dts"; 
    soundId = SoundELFFire; 
};


