// if($build)
// {
	$InvList[Slapper] = 1;
	$MobileInvList[Slapper] = 1;
	$RemoteInvList[Slapper] = 1;
	
	$AutoUse[Slapper] = false;
	
	
	addWeapon(Slapper);

function TABuilder(%clientId)
{
	//if(!%clientId.isGoated) return;
	Player::setItemCount(%clientId, Slapper, 1);
	Player::setItemCount(%clientId, GravityGun, 1);
	Player::useItem(%clientId,GravityGun);
}
	
	ItemImageData SlapperImage 
	{
		shapeFile = "sniper";
		mountPoint = 0;
		weaponType = 0;	
		reloadTime = 3.01; // 2.0
		accuFire = false;
		fireTime = 0.05;
		sfxActivate = SoundPickUpWeapon;
	};
	
	ItemData Slapper 
	{
		description = "Slapper";
		shapeFile = "sniper";
//		hudIcon = "ammopack";
		hudIcon = "weapon";
		className = "Tool";	//className = "Weapon"; 	//tools dont take weapon slots
		shadowDetailMask = 4;
		imageType = SlapperImage;
		showWeaponBar = true;
		price = 85;
	};
	
	function Slapper::MountExtras(%player,%weapon) 
	{	
		%client = Player::getclient(%player);
		%setting = %client.slapperSetting;
		if(!%setting)
			%client.slapperSetting = 0;
		%setting = %client.slapperSetting;
			Bottomprint(%client, "<jc>"@%weapon.description@": <f2>Set to deploy# "@%Setting@": <f1>"@String::getSubStr($Deployable[$Slapper::DeployableMax-%Setting], 7, 40)@".<f2> Press the One or Six keys to cycle. You can move Slapper deploys with your Pitchfork!");
		%clientId = Player::getClient(%player);
	if(!%clientId.isGoated)
	{		
		if(%clientId.hassphallpass == "false")
		{
			// Client::sendMessage(%clientId,0,"Please Wait Ten Seconds Between Slapper Deploys. ~waccess_denied.wav");
			//	%clientId.hasrphallpass = false;
			schedule(%clientId@".hassphallpass= true;",3.0,%clientId);
		}
	}
	}
	
	function SlapperImage::onFire(%player, %slot) 
	{	
		%client = Player::getclient(%player);
		%clientId = Player::getClient(%player);	

		if(!$build)
		{
		Client::sendMessage(%client,0,"The Slapper will not function unless Building is enabled. ");
		Client::sendMessage(%client,0,"~wC_BuySell.wav");
		return;
		}		

	if(!%clientId.isGoated)
	{		
		if(%clientId.hassphallpass == "false")
		{
			// Client::sendMessage(%clientId,0,"Wait A Moment Between Slapper Deploys.");
			Bottomprint(%client, "<jc><f1>"@%name@" cannot deploy. <f2>Slapper firing chamber is cooling down..",3);
			schedule(%clientId@".hassphallpass= true;",3.0,%clientId);
			return;
		}

		%clientId.hassphallpass = false;
	}
	//	if($debug)
		//	Anni::Echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));	
			
		if(!GameBase::getLOSInfo(%player,7)) 
		{
		//	playSound(Soundpackuse,getboxcenter(%player));
			return false;
		}
	
	
		Player::trigger(%player, $WeaponSlot, false);
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);	
		%newobj = Projectile::spawnProjectile(BlasterBolt,%trans,%player,%vel);	//LaserTrail
			
		%setting = %client.slapperSetting;	
		%shape = $Deployable[$Slapper::DeployableMax-%Setting];
		%name = String::getSubStr(%shape, 7, 40);
		%rot = GameBase::getRotation(%player);
		%obj = newObject("","StaticShape",%shape,true);
	
		addToSet("MissionCleanup/deployed/Barrier", %obj);
		GameBase::setTeam(%obj,GameBase::getTeam(%player));
		GameBase::setPosition(%obj,$los::position);
		GameBase::setRotation(%obj,%rot);
		Gamebase::setMapName(%obj,%name@" "@Client::getName(%client));
		Bottomprint(%client, "<jc><f1>"@%name@" deployed. <f2>You Can Now Move This Object With A Pitchfork or Gravity Gun!",3);
	
		GameBase::startFadeIn(%obj);
		playSound(SoundPickupBackpack,$los::position);
		playSound(ForceFieldOpen,$los::position);		
	}
	
	function Slapper::Mode(%player,%rotate)
	{
		%rotate = -%rotate;
		%client = Player::getclient(%player);
		%setting = %client.slapperSetting;
		%setting += %rotate;
		if(%setting < 0)
			%setting = $Slapper::DeployableMax;
		if(%setting > $Slapper::DeployableMax)
			%setting = 0;
		%client.slapperSetting = %setting;
	
		%weapon = Slapper;
		Bottomprint(%client, "<jc>"@%weapon.description@": <f2>Set to deploy# "@%setting@": <f1>"@String::getSubStr($Deployable[$Slapper::DeployableMax-%Setting], 7, 40)@".<f2> Press the One or Six keys to cycle.");
		
	}
	
//		%shape = File::getBase(%file);
		%consolemode = $Console::LogMode; 
		$Console::LogMode = "2"; 
		// Anni::Echo("!! Starting Slapper data block export !!");
		%file = File::FindFirst("*.dts");
		%count = 0;
		while(%file != "")
		{	
			%shape = File::getBase(%file);		
			%file = File::FindNext("*.dts");
			%nopass = false;
			%end = false;
			for(%i = 0; !%end ; %i++)
			{
				%word = getword("teleporter ammopad command pulse trail shield plant newdoor tree2 hover armor tumult steamvent plume exp ex mflame lflame laserhit hflame fusionbolt flash fiery enbolt chainspk sprk shockwave rsmoke plastrail plasmatrail plasmaex AMMOPACK AMMOUNIT ANTEN_LAVA ANTEN_LRG ANTEN_MED ANTEN_ROD",%i);
				%find = String::findSubStr(%shape,%word);
				
				if(%find != -1)
					%nopass = true;
				if(%nopass || %word == -1 || %word = "")
					%end = true;
			}
			if(!%nopass)
			{
				// Anni::Echo("+ #"@%count@" Defining static: "@%shape);
				$Deployable[%count] = "Slapper"@%shape;
	
				%description = "Slapper "@%shape;
				%Data = 	"StaticShapeData Slapper"@%shape@ 
					" { "@
						"shapeFile = \""@%shape@"\";"@
						"debrisId = defaultDebrisSmall;"@
						"maxDamage = 1;"@
						"visibleToSensor = false;"@
						"isTranslucent = true;"@
						"description = \""@%description@"\";"@
					"};"; 		
					
				eval(%Data);
				
				%function = "function Slapper"@%shape@"::onDestroyed(%this) { }";
				eval(%function);
				
				%function = "function Slapper"@%shape@"::onDamage(%this,%type,%value)"@
					" { "@
						"%damageLevel = GameBase::getDamageLevel(%this);"@
						"%dValue = %damageLevel + %value;"@
						"GameBase::setDamageLevel(%this,%dValue);"@							
					"}";
				eval(%function);			
	
				%count++;			
			}	
			else
			{
				// Anni::Echo("- Skipping "@%shape);
			}
				
		}
		
		$Console::LogMode = %consolemode; 
		// Anni::Echo("Finishing Slapper datablock export. " @ %count @ " .dts files loaded");
		$Slapper::DeployableMax = %count-1;
// }
