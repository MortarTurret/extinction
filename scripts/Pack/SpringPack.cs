$InvList[SpringPack] = 1;
$MobileInvList[SpringPack] = 1;
$RemoteInvList[SpringPack] = 1;
AddItem(SpringPack);

$CanAlwaysTeamDestroy[Springboard] = 1;

 //-=-=-=-=-=-=-=- Pack =-=-=-=-=-=-=-

ItemImageData SpringPackImage
{	shapeFile = "flagstand";
	mountPoint = 2;
	mountOffset = { 0, 0, 0.1 };
	mountRotation = { 1.57, 0, 0 };
	firstPerson = false;
};

ItemData SpringPack
{	description = "Starwolf Springboard";
	shapeFile = "ammopack"; // ammopack
	className = "Backpack";
	heading = $InvHead[ihDOb];
	imageType = SpringPackImage;
	shadowDetailMask = 4;
	mass = 1.5;
	elasticity = 0.2;
	price = 600;
	hudIcon = "deployable";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function SpringPack::onAdd(%this)
{
 	%this.faded = "";
}

function SpringPack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player); 
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Starwolf Springboard: <f2>A springboard to launch <f1>things<f2> from. Able to launch Players, Grenades, Mines, Ammo, Bots, Suicide Det packs.");
}

function SpringPack::deployShape(%player,%item) 
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

	if($TeamItemCount[GameBase::getTeam(%player) @ %item] >= $TeamItemMax[%item]) // && !$build
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
	%objSpringboard = newObject("","StaticShape",Springboard,true);
	if(%player.repackEnergy != "")
	{
    GameBase::setDamageLevel(%objSpringboard, %player.repackDamage);
    GameBase::setEnergy(%objSpringboard, %player.repackEnergy);
    %player.repackDamage = "";
    %player.repackEnergy = "";
	}


	addToSet("MissionCleanup/deployed/Barrier", %objSpringboard);
	GameBase::setTeam(%objSpringboard,GameBase::getTeam(%player));
	GameBase::setPosition(%objSpringboard,$los::position);
	GameBase::setRotation(%objSpringboard,%rot);
	Gamebase::setMapName(%objSpringboard,"Springboard "@Client::getName(%client));
	Client::sendMessage(%client,0,"Springboard Deployed ~wturretOff1.wav");
	GameBase::startFadeIn(%objSpringboard);
	playSound(SoundChainTurretOff,$los::position);
	$TeamItemCount[GameBase::getTeam(%player) @ "SpringPack"]++;
	%objSpringboard.deployer = %client; 
 	%objSpringboard.faded = ""; // Death666 NEW
//	if(!$build)
		Anni::Echo("MSG: ",%client," deployed a Springboard");
	return true;
}

function Springboard::onAdd(%this)
{
 	%this.faded = "";
}

 //-=-=-=-=-=-=-=- Object =-=-=-=-=-=-=-

StaticShapeData Springboard
{	shapeFile = "flagstand";
	className = "Decoration";
	debrisId = flashDebrisMedium;
	explosionId = flashExpMedium;
	maxDamage = 6.5;
	isTranslucent = false;
   	description = "Deployable Spring";
	visibleToSensor = true;
	mapFilter = 4;
	mapIcon = "M_generator";
	damageSkinData = "objectDamageSkins";
	shadowDetailMask = 16;
	sfxAmbient = SoundAmmoStationLoopPower;
};

function Springboard::onDestroyed(%this) 
{
	StaticShape::onDestroyed(%this); 
	%this.cloakable = "";
	%this.nuetron = "";
	$TeamItemCount[GameBase::getTeam(%this) @ "SpringPack"]--;
}

function Springboard::onCollision(%this,%obj)
{
	%c = Player::getClient(%obj);
	%armor = Player::getArmor(%c);
	%vecVelocity = Item::getVelocity(%obj);
	%rnd = floor(getRandom() * 40);

	if(Player::isDead(%obj))
		return;

	if(%obj.minebounced == true)
		return;

	%type = getObjectType(%obj);
	if(getObjectType(%obj) != "Player" || (Player::isAIControlled(%obj)))
{

if(%this.faded == "") 
{
%this.faded =1;
schedule(%this@".faded = \"\";",0.5,%this);

	%name = GameBase::getDataName(%obj);	
	%class = %name.className;	
	%description = %name.description; 

		if(%class ==  Mine || %class == Ammo || %class == Handgrenade || %class == HandAmmo || (Player::isAIControlled(%obj)) )
		{
		%obj.minebounced = true;
		if(%name == AntipersonelMine)
		{
			schedule("GameBase::setDamageLevel(" @ %obj@ "," @ 1 @ ");", 3.5,%obj);
			%obj.minebounced = true;
		}
		if(%description == Hologram)
		{
			schedule("GameBase::setDamageLevel(" @ %obj @ "," @ %name.maxDamage @ ");", 3.5,%obj);
			%obj.minebounced = true;
		}
	%rnd = floor(getRandom() * 40);
	if (%rnd == 1)
	{	
		 GameBase::playSound(%this, explosion3, 0); //debrisLargeExplosion	
		%HMult = 2;
		%ZMax = 150;
		%vecNewVelocity = GetWord(%vecVelocity, 0) * %HMult @ " " @ GetWord(%vecVelocity, 1) * %HMult @ " " @ %ZMax;
		Item::setVelocity(%obj, %vecNewVelocity);
		return;
	}
	else if (%rnd > 30)
	{	
		GameBase::playSound(%this, debrisLargeExplosion, 0);
		%HMult = 2;
		%ZMax = 120;
		%vecNewVelocity = GetWord(%vecVelocity, 0) * %HMult @ " " @ GetWord(%vecVelocity, 1) * %HMult @ " " @ %ZMax;
		Item::setVelocity(%obj, %vecNewVelocity);
		return;
	}
	else
	{
		GameBase::playSound(%this, SoundFireMortar, 0);
		%HMult = 2;
		%ZMax = 90; // 50 came to about 63 height
		%vecNewVelocity = GetWord(%vecVelocity, 0) * %HMult @ " " @ GetWord(%vecVelocity, 1) * %HMult @ " " @ %ZMax;
		Item::setVelocity(%obj, %vecNewVelocity);
		return;
	}
		return;
		}
		return;
}

}
	%type = getObjectType(%obj);
	if(getObjectType(%obj) == "Player")
{

if(%this.faded == "") 
{
%this.faded =1;
schedule(%this@".faded = \"\";",1,%this);

	if(%c.hashallpass2)
{
		%c = Player::getClient(%obj);
		%c.hashallpass2 = false;
		schedule(%c@".hashallpass2 = true;",2,%c);

		%c = Player::getClient(%obj);
		%armor = Player::getArmor(%c);
		%medrnd=floor(getrandom()*40);
		%armor=GameBase::getDataName(%obj);
		%mass=%armor.mass;
		%rot=GameBase::getRotation(%obj);

		if (%medrnd == 19) // one is unlucky
		{	
		 GameBase::playSound(%this, explosion3, 0); //debrisLargeExplosion
         		Client::SendMessage(%c, 0, "Springboard Malfunction! ~wexplo3.wav");	
		%medrnd = %medrnd + 100;
		}
		else if(%medrnd > 35) 
		{
		%soundrnd = floor(getRandom() * 2); 
		if (%soundrnd == 0)
		{ 
			GameBase::playSound(%this, debrisLargeExplosion, 0);
			Client::SendMessage(%c, 0, "~wdebris_large.wav");
		}
		else if (%soundrnd == 1)
		{
			GameBase::playSound(%this, bigExplosion1, 0);
			Client::SendMessage(%c, 0, "~wbxplo1.wav");
 		}
			Client::SendMessage(%c, 0, "K-E-R-S-P-R-O-I-N-G-g-g-g-!-!");
			%medrnd = %medrnd + 10;
		}
		else
		{
		%soundrnd = floor(getRandom() * 5); 
		if (%soundrnd == 0)
		{ 
			GameBase::playSound(%this, SoundFireSeeking, 0);
			Client::SendMessage(%c, 0, "~wseek_fire.wav");
		}
		else if (%soundrnd == 1) 
		{
			GameBase::playSound(%this, rocketExplosion, 0);
			Client::SendMessage(%c, 0, "~wrockexp.wav");
 		} 
		else if (%soundrnd == 2)
		{ 
			GameBase::playSound(%this, SoundLandOnGround, 0);
			Client::SendMessage(%c, 0, "~wLand_On_Ground.wav");
		} 
		else if (%soundrnd == 3)
		{ 
			GameBase::playSound(%this, SoundFireGrenade, 0);
			Client::SendMessage(%c, 0, "~wGrenade.wav");
		}
		else if (%soundrnd == 4)
		{ 
			GameBase::playSound(%this, SoundFireMortar, 0);
			Client::SendMessage(%c, 0, "~wmortar_fire.wav");
		}
			Client::SendMessage(%c, 0, "SPROING!");
		}
		%len = 40 + %medrnd;
		%trans = GameBase::getMuzzleTransform(%obj);
		%tr= getWord(%trans,5);
		if(%tr < 0) %tr = -%tr;
		%tr = %tr+ 0.15;
		%up = 1.45-%tr;
		%out = 0.01;
		%vec = Vector::getFromRot(%rot,%len*%mass*%out,%len*%mass*%up); 
		Player::applyImpulse(%obj,%vec);
}
		return;
}
		return;
}

}