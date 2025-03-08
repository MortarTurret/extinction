$InvList[AirStrikePack] = 1;
$MobileInvList[AirStrikePack] = 1;
$RemoteInvList[AirStrikePack] = 1;

AddItem(AirStrikePack);

ItemImageData AirstrikeImage
{
	shapeFile = "paintgun";
	mountPoint = 0;

	weaponType = 1; // Single (Sustained)
	// projectileType = FakeStuff;
	accuFire = true;
	minEnergy = 0;
	maxEnergy = 0;
	spinUpTime = 0.0;
	spinDownTime = 0;
	fireTime = 0.1;
	reloadTime = 0.0;

	lightType   = 3;  // Weapon Fire
	lightRadius = 1;
	lightTime   = 1;
	lightColor  = { 0.25, 1, 0.25 };

	sfxFire     = SoundFireAirstrike; //SoundFireTargetingLaser
	sfxActivate = SoundPickUpWeapon;
};

ItemData Airstrike
{
	description   = "Airstrike";
	className     = "Tool";
	shapeFile     = "paintgun";
	hudIcon       = "targetlaser";
   	//heading = "bWeapons";
	heading = $InvHead[ihBac];
	shadowDetailMask = 4;
	imageType     = AirstrikeImage;
	price         = 999;
	showWeaponBar = false;
};

ItemImageData AirstrikePackImage
{
	shapeFile = "radar_small"; //armorPack sensor_jammer
	mountPoint = 2;
	weaponType = 2;  // Sustained
   	minEnergy = 0;
	maxEnergy = 0;   // Energy used/sec for sustained weapons
	mountOffset		= { 0, -0.1, 0 };
  	mountRotation = { 0, 0, 0 };
	firstPerson = false;
};

ItemData AirstrikePack
{
	description = "Airstrike Pack";
	shapeFile = "radar_small"; //armorPack
	className = "Backpack";
   	heading = "eBackpacks";
	shadowDetailMask = 4;
	imageType = AirstrikePackImage;
	price = 1000;
	hudIcon = "compass"; //repairpack
	showWeaponBar = true;
	hiliteOnActive = true;
};

function AirstrikePack::onMount(%player,%item) 
{	
	if($debug)
		Anni::Echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getclient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
	Bottomprint(%client, "<jc>Airstrike Pack: <f2>Call in air support.");	
}

function AirstrikePack::onUnmount(%player,%item)
{
	if (Player::getMountedItem(%player,$WeaponSlot) == Airstrike) {
		Player::unmountItem(%player,$WeaponSlot);
	}

	$airstrikeIn[%player] = 0;
}

function AirstrikePack::onUse(%player,%item)
{
	if (Player::getMountedItem(%player,$BackpackSlot) != %item) {
		Player::mountItem(%player,%item,$BackpackSlot);
	}
	else {
		Player::mountItem(%player,Airstrike,$WeaponSlot);
	}
}

function AirstrikePack::onDrop(%player,%item)
{
	if($matchStarted) {
		%mounted = Player::getMountedItem(%player,$WeaponSlot);
		if (%mounted == Airstrike) {
			Player::unmountItem(%player,$WeaponSlot);
		}
		else {
			// Remount the existing weapon to make sure the Airstrike
			// is not on the delayed mount "stack".
			Player::mountItem(%player,%mounted,$WeaponSlot);
		}
		Item::onDrop(%player,%item);
	}
}

// HACK!
$AirTime[0] = 0.1;
$AirTime[1] = 0.2;
$AirTime[2] = 0.3;
$AirTime[3] = 0.4;
$AirTime[4] = 0.5;

function AirstrikeImage::onFire(%player, %slot) {
	//%tform = Gamebase::getMuzzleTransform(%player);
	%client = Player::getClient(%player);
	%clientId = Gamebase::getOwnerClient(%player);

	%team = GameBase::getTeam(%player);
	%playerPos = GameBase::getPosition(%player);
	%prot =GameBase::getRotation(%player);

	%flagpos = $teamFlag[%team].originalPosition;	
// disabling this and leaving a block around using airstrike on your own teams flag instead -death666
//	if(Vector::getDistance(%flagpos, %playerPos) < 255) //playerpos
//	{
//		//Client::sendMessage(%clientId, 1, "You are too close to your base to call in an airstrike.");
//		Bottomprint(%clientId, "<jc>You are too close to your base to call in an airstrike.", 2);
//		return false;
//	}

	if(!GameBase::getLOSInfo(%player,280)) 
	{
		Bottomprint(%clientId, "<jc>Target out of range.", 2);
		return false;
	}

		if(!GameBase::getLOSInfo(%player,1)) 
		{
			%set = newObject("set",SimSet);
			%num = containerBoxFillSet(%set,$ItemObjectType,$los::position,40,40,40,0); // 30 30 30
			%num2 = CountObjects(%set,"flag",%num);
			%totalnum = Group::objectCount(%set);
			%enemyflag=0;
			for(%i = 0; %i < %totalnum; %i++)
			{
				%obj = Group::getObject(%set, %i);
				%name = Item::getItemData(%obj);
				if(%name == "flag" && (GameBase::getTeam(%obj) == Gamebase::getTeam(%player)))
				{
					Bottomprint(%clientId, "<jc>Unable to designate target.");
					deleteObject(%set);
					return;
				}
			}
			deleteObject(%set);
		}
	
	$tempCount[%player]++;

	// If the player is activating the airstrike for the first time, start the count
	if($airstrikeIn[%player] == 0) {
		$airstrikeIn[%player] = 11;
		//Client::sendMessage(%clientId, 1, "Airstrike called in - designate target for 10 seconds");
		Bottomprint(%clientId, "<jc>Airstrike called in - designate target for 10 seconds.", 10);
		$AirstrikePos[%player] = GameBase::getPosition(%player);
		if(Client::getTeam(%clientId) == 0) {
			%numOtherTeam = 1;
		} else %numOtherTeam = 0;
		//sendGameTeamMessage(%numOtherTeam, "Beware - the enemy is calling in an airstrike.");
		schedule("decAirstrike(" @ %player @ "," @ %clientId @ ");", 1);
	}

	// If the count has run out, "call in" the airstrike and remove the pack 
	if($airstrikeIn[%player] == 1) {
		GameBase::getLOSInfo(%player, 5000);
		%tx = getWord($los::position, 0);
		%ty = getWord($los::position, 1);
		%tz = getWord($los::position, 2);
		%tz += 200;
		//echo("Transform: " @ %transform);
		if($ASMode[%clientId] == 0) {
			%transform = "0 0 0 0 0 0 0 0 0 "@%tx@" "@%ty@" "@%tz@"";
			Projectile::spawnProjectile("AirstrikeShell", %transform, %player, "5 0 0");
			Projectile::spawnProjectile("AirstrikeShell", %transform, %player, "0 5 0");
			Projectile::spawnProjectile("AirstrikeShell", %transform, %player, "0 0 5");
		} else {
			%vec = Vector::normalize(Vector::sub($los::position, GameBase::getPosition(%player)));
			%nx = getWord(%vec, 0) * 10;
			%ny = getWord(%vec, 1) * 10;
			for(%i = -2.0; %i < 3.0; %i++) {
				%transform = "0 0 0 0 0 0 0 0 0 "@%tx+(%nx*%i)@" "@%ty+(%ny*%i)@" "@%tz@"";
				Projectile::spawnProjectile("AirFlame", %transform, %player, "5 0 0");
				Projectile::spawnProjectile("AirFlame", %transform, %player, "0 5 0");
				Projectile::spawnProjectile("AirFlame", %transform, %player, "0 0 5");
			}
		}
		$airstrikeIn[%player]--;
		Player::setItemCount(%clientId,Airstrike,0);
		Player::setItemCount(%clientId,AirstrikePack,0);
		Player::setItemCount(%clientId,ChameleonPack,1);
	        Player::mountItem(%clientId,ChameleonPack,1);
		Client::sendMessage(%clientId, 1, "Airstrike successful, Chameleon Pack equipped. ~wfemale2.whitdeck.wav");
		Bottomprint(%clientId, "<jc>Airstrike successful, Chameleon Pack equipped.", 3);
		$successAS[%player] = 1;
		$tempCount[%player] = 0;
		%quitFlag[%player] = 1;
		//PowerTeamMessage(%team, 1,"","Enemy airstrike incoming. ~wfemale2.whitdeck.wav");
		return;
	}
}

function decAirstrike(%player, %clientId) {
	if ($quitFlag[%player] == 1) {
		$quitFlag[%player] = 0;
		$countRef[%player] = 0;
		$tempCount[$player] = 0;
		$airstrikeIn[%player] = 0;
		return;
	}
	%diff = $tempCount[%player] - $countRef[%player];
	if (%diff < 3 || GameBase::getPosition(%player) != $AirstrikePos[%player]) {
		if ($successAS[%player] == 1) {
			$successAS[%player] = 0;
			return;
		}
		//client::sendMessage(%clientId, 1, "Airstrike cancelled.");
		Bottomprint(%clientId, "<jc>Airstrike cancelled. Must designate target for 10 seconds.", 3);
		if(Client::getTeam(%clientId) == 0) {
			%numOtherTeam = 1;
		} else %numOtherTeam = 0;
		//sendGameTeamMessage(%numOtherTeam, "The enemy has cancelled their airstrike.");
		$airstrikeIn[%player] = 0;
		$tempCount[%player] = 0;
		$countRef[%player] = 0;
		return;
	}	
	$countRef[%player] = $tempCount[%player];	
	$airstrikeIn[%player]--;
	schedule("decAirstrike(" @ %player @ ", " @ %clientId @ ");", 1);
}