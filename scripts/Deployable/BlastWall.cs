$InvList[BlastWallPack] = 1;
$MobileInvList[BlastWallPack] = 1;
$RemoteInvList[BlastWallPack] = 1;
AddItem(BlastWallPack);

$CanAlwaysTeamDestroy[BlastWall] = 1;

ItemImageData BlastWallPackImage 
{
	shapeFile = "sensorjampack";
	mountPoint = 2;
	mountOffset = { 0, -0.1, 0 };
	mass = 2.5;
	firstPerson = false;
};

ItemData BlastWallPack 
{
	description = "Blast Wall";
	shapeFile = "sensorjampack"; 
	className = "Backpack";
	heading = $InvHead[ihBar];
	imageType = BlastWallPackImage;
	shadowDetailMask = 4;
	mass = 1.5;
	elasticity = 0.2;
	price = 600;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};


function BlastWallPack::deployShape(%player,%item) 
{	
	%client = Player::getClient(%player);
	%clientId = Player::getClient(%player);

		if(!$build)
		{
	if(%clientId.inArena)
	{ 
		Client::sendMessage(%client,0,"Cannot deploy in arena unless building is on. ");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return;	
	}
		}
		if(!$build)
		{
	if(%player.outArea)
	{
		Client::sendMessage(%client,0,"can not deploy out of bounds unless building is on.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
		}
	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]  && !$build) 
	{
		Client::sendMessage(%client,0,"Deployable Item limit reached for " @ %item.description @ "s");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}

	if(!GameBase::getLOSInfo(%player,5)) 
	{
		Client::sendMessage(%client,0,"Deploy position out of range.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}

	%obj = $los::object;
	//Anni::Echo (GameBase::getTeam(%obj));
	if((GameBase::getTeam(%obj) != GameBase::getTeam(%player)) && (getObjectType(%obj) != "SimTerrain") && (GameBase::getTeam(%obj) != -1)) 
	{
		Client::sendMessage(%client,0,"Cannot deploy on enemy base");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}

	if(%obj.inmotion == true)	 
	{ 
		Client::sendMessage(%client,0,"Deploy area crappy, cannot deploy.");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}
	
	if(Vector::dot($los::normal,"0 0 1") <= 0.7) 
	{
		Client::sendMessage(%client,0,"Can only deploy on flat surfaces");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return false;
	}

	if(!checkDeployArea(%client,$los::position)) 
		return false;

	%rot = GameBase::getRotation(%player);
	%objBlastWall = newObject("","StaticShape",BlastWall,true);
	if(%player.repackEnergy != "")
	{
    GameBase::setDamageLevel(%objBlastWall, %player.repackDamage);
    GameBase::setEnergy(%objBlastWall, %player.repackEnergy);
    %player.repackDamage = "";
    %player.repackEnergy = "";
	}

	addToSet("MissionCleanup/deployed/Barrier", %objBlastWall);
	GameBase::setTeam(%objBlastWall,GameBase::getTeam(%player));
	GameBase::setPosition(%objBlastWall,$los::position);
	GameBase::setRotation(%objBlastWall,%rot);
	Gamebase::setMapName(%objBlastWall,"Blast Wall "@Client::getName(%client));
	Client::sendMessage(%client,0,"Blast Wall Deployed");
	GameBase::startFadeIn(%objBlastWall);
	playSound(SoundPickupBackpack,$los::position);
	playSound(ForceFieldOpen,$los::position);
	$TeamItemCount[GameBase::getTeam(%player) @ "BlastWallPack"]++;
	%objBlastWall.deployer = %client; 	
	if(!$build)
		Anni::Echo("MSG: ",%client," deployed a Blast Wall");
	return true;
}

StaticShapeData BlastWall 
{
	shapeFile = "newdoor5";
	maxDamage = 10.0; 
	debrisId = defaultDebrisLarge;
	explosionId = debrisExpLarge;
	visibleToSensor = true;
	damageSkinData = "objectDamageSkins";
	description = "Blast Wall";
};

function BlastWall::onDestroyed(%this) 
{
	if(!$NoCalcDamage)
		calcRadiusDamage(%this, $DebrisDamageType, 2.5, 0.05, 25, 13, 2, 0.40, 0.1, 250, 100);
	$TeamItemCount[GameBase::getTeam(%this) @ "BlastWallPack"]--;
}
StaticShapeData DamageMarker 
{
	shapeFile = "bullet";
	maxDamage = 0.01;
	debrisId = defaultDebrisSmall;
	explosionId = debrisExpSmall;
	visibleToSensor = true;
	damageSkinData = "objectDamageSkins";
	description = "DamageMarker";
	disableCollision = true;
};
function DamageMarker::ondamage(%this)
{
	deleteobject(%this);
}


function DamageMarker::onadd(%this)
{
	//schedule("deleteobject("@%this@");",50);
}

function stickyBlastWall::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	%damageLevel = GameBase::getDamageLevel(%this);
	%dValue = %damageLevel + %value;
	GameBase::setDamageLevel(%this,%dValue);
			
	if(%value > 0 && %pos != "0 0 0" )
	{
		if(%type == 1 || %type == 6 || %type == 14 || %type == 16)
		{
			%obj = newObject("","StaticShape",DamageMarker,true);
			addToSet("MissionCleanup/deployed/Barrier", %obj);
			
			GameBase::setPosition(%obj,%pos);
			%player = Client::getOwnedObject(%object);
			%trans = GameBase::getMuzzleTransform(%player);

				
			%posX = getWord(%trans,9);		//x
			%posY = getWord(%trans,10);		//y
			%posZ = getWord(%trans,11); 		//z	
			%GunTipPos = %posX@" "@%posY@" "@%posZ;	
					
			%vektar = vector::normalize(vector::sub(%pos,%GunTippos));		
			%xrot = getword(%vektar,2);
			%rot = gamebase::getrotation(%player);
			GameBase::setRotation(%obj,%xrot@" 0 "@getword(%rot,2));	
			
			%vector = vector::normalize(%vec);
		//	messageall(1,"pos "@%pos@", vec "@%vector@", mom "@%mom@", rot "@%rot);
		}

	}
	if(%value >0 && (%type == 12 || %type == 28))
	{
		%smack = 0.15;
		%player = Client::getOwnedObject(%object);
		%ppos = gamebase::getposition(%player);
		%bpos = gamebase::getposition(%this);
		%dist = vector::getdistance(%ppos,%bpos);
		%rot = gamebase::getrotation(%this);
		%bvec = vector::getfromrot(%rot,%dist);
		
	//	%vec = vector::getfromrot(
		%lean = vector::add(%smack@" 0 0",%rot);
		GameBase::setRotation(%this,%lean);
	}	
}

function Rotation::getFromVec(%vector)
{
	// Input vector should be normalized
	%vec = vector::normalize(%vector);
	%pi = 3.14159265;
	%v1 = getword(%vec,0);
	%v2 = getword(%vec,1);
	%v3 = getword(%vec,2);
	
	// vectors are in the form x,y,z
	// X and Y are north/ east
	// Z is up
	
	// object rotation is in the form x,y,z 
	// Players only use Z
	// X value is up/ down with 0 being 'flat'
	// Y is rotation around the axis of direction. 
	
	//need a!@#$%!ing atan function here... 
	
	return %rot;
}

function arcTan(%x,%y)
{
	//ArcTan[x, y] == (-I) Log[(x + I y)/Sqrt[x^2 + y^2]]
	// yeah right... 
}


function rotateVector(%vec,%rot)
{
	%pi = 3.14;
	%rot3= getWord(%rot,2);
	for(%i = 0; %rot3 >= %pi*2; %i++) %rot3 = %rot3 - %pi*2;
	if (%rot3 > %pi) %rot3 = %rot3 - %pi*2;

	%vec1= getWord(%vec,0);
	%vec2= getWord(%vec,1);
	%vc = %vec2;
	%vec3= getWord(%vec,2); 
	%ray = %vec1;
	
	%vec1 = %ray*cos(%rot3);
	%vec2 = %ray*sin(%rot3);
	%vec = %vec1 @" "@ %vec2 @" "@ %vec3;
	%vec = Vector::add(%vec,Vector::getFromRot(%rot,%vc,0));
	return %vec;
}

function BlastWallPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player); 
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Blast Wall: <f2>A large blast wall which may be used to cover turrets to further defend them.");	
}