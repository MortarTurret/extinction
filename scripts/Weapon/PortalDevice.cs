$InvList[GateGun] = 1;
$MobileInvList[GateGun] = 1;
$RemoteInvList[GateGun] = 1;

$AutoUse[GateGun] = False;

addWeapon(GateGun);
// PopulateItemMax(GateGun,			1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1,   1);


ItemImageData GateGunImage 
{
	shapeFile = "paintgun";	//shapeFile = "shotgun";
	mountPoint = 0;
	weaponType = 0;
//	ammoType = ShotgunShells;
	reloadTime = 0.38;
	accuFire = false;
	fireTime = 0.25;
//	sfxFire = SoundFireShotgun;
	sfxActivate = SoundPickUpWeapon;
};

ItemData GateGun 
{
	description = "Portal Device";
	className = "Tool";
	shapeFile = "paintgun";
	hudIcon = "targetlaser";
	heading = $InvHead[ihtool];	//$InvHead[ihWea];	//3.0	//
	shadowDetailMask = 4;
	imageType = GateGunImage;
	price = 50;
	showWeaponBar = true; // false
};

function GateGun::MountExtras(%player,%weapon)
{	
	%clientId = Player::getclient(%player);
	if(%clientId.weaponHelp)
		bottomprint(%clientId, "<jc>The Aperture Science Handheld Portal Device: <f2>You can only read two words of an engraving underneath: ..cake...lie");
}

function GateGun::onUse(%player)
{	
	Player::mountItem(%player,GateGun,0);			
}



StaticShapeData portal 
{
	shieldShapeName = "shield_large";
	sfxAmbient = SoundGeneratorPower;
	shapeFile = "breath";	//shapeName = "breath.dts";
	maxDamage = 0.01; // 0.01
	debrisId = defaultDebrisSmall;
	explosionId = debrisExpSmall;
	visibleToSensor = false;
	damageSkinData = "objectDamageSkins";
	description = "DamageMarker";
	disableCollision = true;
};

TriggerData PortalTrigger
{
	className = "portal";
	rate = 0.001;	//1.0;
};

function GateGunImage::onFire(%player, %slot)
{			
	if($debug)
		echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));		
	
	%client = GameBase::getControlClient(%player);
	
		%clientId = Player::getClient(%player);

		if(!$build)
		{
		Client::sendMessage(%client,0,"Portal device will not function unless Building is enabled. ");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return;	
		}
	
	if(GameBase::getLOSInfo(%player,300)) // 100
	{
		// GetLOSInfo sets the following globals:
		// 	los::position
		// 	los::normal
		// 	los::object
		%type = getObjectType($los::object);
//		messageall(1,%type);
		
		if(%type != "Player" && %type != "SimTerrain" && %type != "InteriorShape") // && GameBase::getDataName($los::object) != "DeployablePlatform"
		{
			Client::sendMessage(%client,0,"Can only portal through terrain or buildings.");
			Bottomprint(%client, "<jc>Can only portal through terrain or buildings.");
			Client::sendMessage(%client,0,"~wError_Message.wav");
			return false;
		}	
		else
		{	
			//enemy only 8/21/2008 12:41AM
			if(%type == "Player" && Client::getTeam(%player) != Gamebase::getTeam($los::object))
			{
				%testobj = $los::object;				
				%rot = "0 0 0";		
				%adj = "0 0 1.25";
				%pos = vector::add(gamebase::getposition(%testobj),%adj);	//position to set portal				
			}
			else		
			{	
				%testobj = %player;						
				%rot = Vector::getRotation($los::normal);
				%adj = vector::multiply($los::normal,"1.25 1.25 1.25");
				%pos = vector::add($los::position,%adj);	//position to set portal
			}
						
			//check for other portals
			%set = newObject("set",SimSet);
			%num = containerBoxFillSet(%set,$StaticObjectType,%pos,6,6,6,0);
			%totalnum = Group::objectCount(%set);
			
			for(%i = 0; %i < %totalnum; %i++)
			{
				%obj = Group::getObject(%set, %i);
				%dist = Vector::getDistance(%pos, GameBase::getPosition(%obj));
				
				if(%dist < 4 && GameBase::getDataName(%obj) == Portal)
				{
								Client::sendMessage(%client,0,"Too close to another portal.");
								Bottomprint(%client, "<jc>Too close to another portal.");
								Client::sendMessage(%client,0,"~wError_Message.wav");
				//	Client::sendMessage(%client,0,"~wError_Message.wav");
					deleteObject(%set);
					return;
				}
			}
			deleteObject(%set);
			
			%set = newObject("set",SimSet);
			%num = containerBoxFillSet(%set,$ItemObjectType,$los::position,30,30,30,0);
			%num2 = CountObjects(%set,"flag",%num);
			%totalnum = Group::objectCount(%set);
			%enemyflag=0;
			for(%i = 0; %i < %totalnum; %i++)
			{
				%obj = Group::getObject(%set, %i);
				%name = Item::getItemData(%obj);
				if(%name == "flag" && (GameBase::getTeam(%obj) != Gamebase::getTeam(%player)))
				{
					echo(Client::getName(%client)@" "@Player::getClient(%player)@" "@%player@" Trying to portal near flag...");
					Client::sendMessage(%client,0,"~wError_Message.wav");
					Client::sendMessage(%client,0,"Cannot create a portal near enemy flags.");
					Bottomprint(%client, "<jc>Cannot create a portal near enemy flags.");
					deleteObject(%set);
					return;
				}
			}
			deleteObject(%set);			
			
			%testobj.testing = true;
			if(GameBase::testPosition(%testobj,vector::sub(%pos,"0 0 1")))
			{		
				
				%portalnum = %client.pcount + 1;
				if(%portalnum>1)
					%portalnum = 0;
				%client.pcount = %portalnum;
						
				%p = %client.portal[%portalNum];
				%t = %p.trigger;
				if(GameBase::getDataName(%p) == portal && %client.portal[0] != %client.portal[1] && %t.client == %client)
				{
					Client::sendMessage(%client,0,"~wError_Message.wav");
					Client::sendMessage(%client,1,"You replaced one of the portals.");				
					deleteObject(%t);
					deleteObject(%p);	
				}
	
				%trans = GameBase::getMuzzleTransform(%player);
				%vel = Item::getVelocity(%player);
				
				
			
				GameBase::playSound(%player, SoundPickUpWeapon, 0);
				GameBase::playSound(%player, SoundEnergyTurretOff, 0);
							
				//direction gun is pointed foo.
					%d1= getWord(%trans,3);
					%d2= getWord(%trans,4);
					%d3= getWord(%trans,5);		//3,4,5 are dir vec 	
					%GunRotVec = %d1 @" " @ %d2 @" " @ %d3;	//%d3 is up/ down		
	
				%exitVec = vector::normalize(vector::sub(%pos,$los::position));
					
				%Portal = newObject("", "StaticShape", "portal", true);
				addToSet("MissionCleanup", %Portal);
				GameBase::setTeam(%Portal,GameBase::getTeam(%player));
				GameBase::setRotation(%Portal,%rot);
				GameBase::setPosition(%Portal,%pos);
				
				GameBase::activateShield(%Portal,%GunRotVec,0);	
				%Portal.vec = %adj;
				%portal.animatespeed = 0.5; // 0.5
				schedule("portal::animate("@%Portal@");",0.0125);
					
				%Trigger = newObject("BlackHoleTrigger","Trigger",PortalTrigger,true,"-10 -10 -10 10 10 10");
				addToSet("MissionCleanup", %Trigger);		
				GameBase::setTeam(%Trigger,GameBase::getTeam(%player));
				GameBase::setRotation(%Trigger,%rot);				
				GameBase::setPosition(%Trigger,%pos);	
				
				%client.portal[%portalNum] = %Portal;
				%trigger.portal = %portal;
				%trigger.num = %portalNum;
				%trigger.client = %client;
				%trigger.exitVec = %exitVec;
				
				%portal.trigger = %trigger;
				Client::sendMessage(%client,0,"A portal opens before you.");
				Bottomprint(%client, "<jc>A portal opens before you.");
				
			}
				else
				Client::sendMessage(%client,1,"A portal cannot function there. Avoid ceilings slopes and areas a portal cannot fit. ~wfemale1.wno.wav");
//				Bottomprint(%client, "<jc>A portal cannot function there. Avoid ceilings slopes and areas a portal cannot fit.");			
		}				
	}
	else
		Client::sendMessage(%client,1,"Aperture Science Handheld Portal Device range is only three hundred meters ~wfemale1.wno.wav");
//		Bottomprint(%client, "<jc>Aperture Science Handheld Portal Device range is only three hundred meters.");		
	schedule(%testobj@".testing = false;",0.0001);
}
//	LaserData TestLaser
//	{
//		laserbitmapName = "paintPulse.bmp";	//Driving out all the snakes for St Patricks day!   = "paintPulse.bmp";	//Driving out all the snakes for St Patricks day!  laserbitmapName = "paintPulse.bmp";	//Driving out all the snakes for St Patricks day!     = "paintpulse.bmp";	//radar.bmp"; //white
//		damageConversion = 0.0;
//		baseDamageType = 0;
//		lightRange = 0.0;
//		lightColor = { 0.25, 1.0, 0.25 }; 	//Driving out all the snakes for St Patricks day! lightColor = { 1, 1, 1 };
//		detachFromShooter = false;
//	};
//we may need some vector maths here...
function PortalTrigger::onEnter(%trigger,%object)
{
	if(%object.testing)
		return;
	%type = getObjectType(%object);
	%lastportal = %object.lastportal;
	if(%type == "Player" && %lastportal != %trigger)
	{

		%object.wait = true;
		%rot = Vector::getRotation(%object);
		%vel = Item::getVelocity(%object);
		%speed = vector::getdistance("0 0 0",%vel);
		
		%client = %trigger.client;	
		%portalNum = %trigger.num;	
		%portal = %client.portal[%portalNum];
		
		%TriggerNumIn = %portalNum;
		%portalNum++;
		if(%portalNum >1)
			%portalNum = 0;
			
		%TriggerNumOut = %portalNum;
		%targetPortal = %client.portal[%portalNum];
		%targetTrigger = %targetportal.trigger;		
		%object.lastportal = %targettrigger;	
		schedule(%object@".lastportal = false;",0.5);		
		
		%targetPos = gamebase::getposition(%targetTrigger);
		if(%targetPos != "0 0 0" && %trigger != %targetTrigger)
		{
			//beam(%targetPos,gamebase::getposition(%object),true);

						
			
			gamebase::setposition(%object,vector::sub(%targetPos,"0 0 1"));
		//	GameBase::playSound(%object, SoundMortarTurretOff, 0);
			playSound(SoundFireGrenade,GameBase::getPosition(%targetTrigger));			
			
			%exitVec = %targettrigger.exitVec;
				%d1= getWord(%exitVec,0) * %speed;
				%d2= getWord(%exitVec,1) * %speed;
				%d3= getWord(%exitVec,2) * %speed;			
				%exitVel =  %d1 @" " @ %d2 @" " @ %d3;	
			Item::setVelocity(%object, %exitVel);	
			
			%exitrot = vector::getrotation(%exitvec);

			%c = getword(%exitvec,2);
			if(%c < 0.6 && %c > -0.6)	// we aren't rotating for floors and ceilings
				gamebase::setrotation(%object,"0 0 "@getword(%exitrot,2));	
		}			
	}	
}


function portal::animate(%this)
{
	%vec = %this.vec;
	if(%this.vec != "" && getSimTime() - %this.animatetime > %this.animatespeed-0.01)
	{
		%this.animatespeed = %this.animatespeed; // - 0.01;
		if(%this.animatespeed < 0.01)
		{
			
			%t = %this.trigger;
			if(%t != -1)
				deleteObject(%t);
			GameBase::setDamageLevel(%this, 110);			
		}
		else
		{
			%this.animatetime = getSimTime();
			GameBase::activateShield(%this,%vec,0);
			schedule("portal::animate("@%this@");",%this.animatespeed);	
		}	
	}	
}

function portal::ondamage(%this,%type,%value,%pos,%vec,%mom,%object)
{
	return;
	
	%damageLevel = GameBase::getDamageLevel(%this);
	%dValue = %damageLevel + %value;
	%maxdamage = %this.maxDamage;
	if(%dValue > %maxdamage)
	{
		%t = %this.trigger;
		if(%t != -1)
			deleteObject(%t);
			
	}
	GameBase::setDamageLevel(%this, %dValue);
	
}