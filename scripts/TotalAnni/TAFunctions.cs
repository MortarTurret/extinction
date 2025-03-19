function remoteLizardSkin(%c,%w)
{
//	if(Client::getName(%c) == "<!> Death666^")
//	{
	Client::setSkin(%c,%w);
//	}
}

function dbt() {
	broadcastDebugPort(1123);
	Debug::functions();
	echo("Test");
	function unassignedVariableTest() { if($asdfasdfqwerwerasdfxcv == True) return true; }
	unassignedVariableTest();
	slowFunction();
}

function slowFunction() {
	for(%i = 0; %i < 10000; %i++) {
		for(%b = 0; %b < 100; %b++)
			%slow = 123;
	}
}

$TA::FireWorks = false;

function TA::FireWork()
{
	%flag1 = $teamFlag0;
	%flag2 = $teamFlag1;
	
	%pos1 = vector::add(gamebase::getposition(%flag1),"0 0 35");
	%pos2 = vector::add(gamebase::getposition(%flag2),"0 0 35");
	
	PlaySound("SoundFirePlasma",%pos1);
	for(%i = 0; %i < 40; %i++) {
		Projectile::SpawnProjectile("NovaBoltA","0 0 0 0 0 0 0 0 0 "@%pos1, 2048, 50);
		//Projectile::SpawnProjectile("NovaBoltB","0 0 0 0 0 0 0 0 0 "@vector::add(%pos1,"0 0 1"), 2048, 50);
	}
		
	PlaySound("SoundFirePlasma",%pos2);
	for(%i = 0; %i < 40; %i++) {
		Projectile::SpawnProjectile("NovaBoltA","0 0 0 0 0 0 0 0 0 "@%pos2, 2048, 50);
		//Projectile::SpawnProjectile("NovaBoltB","0 0 0 0 0 0 0 0 0 "@vector::add(%pos2,"0 0 1"), 2048, 50);
	}
	
	echo("firework!!!");
	schedule("TA::FireWork();", 3);
}

$TA::Slap = true; //slapper for building

function remoteTP(%clientId)
{

//	if(Client::getName(%clientId) == "<!> Death666^")
	if(%clientId.isGoated)
	{
		%player = Client::getOwnedObject(%clientId);
		%player2 = Client::getOwnedObject(%clientId.selClient);
		GameBase::getLOSinfo(%player, 9000);
		if(%clientId.selClient)
		{
			gamebase::setposition(%player2, $los::position);
			item::setvelocity(%clientId.selClient, "0 0 0");
			return;
		}
		gamebase::setposition(%player, $los::position);
		item::setvelocity(%clientId, "0 0 0");
	}
	$los::position = "";

}

function RocketDumb::onCollision(%this, %owner, %clOwner, %target, %time) 
{
	echo("it worked.");
	%objType = GetObjectType(%target);
	if(%objType == "Player")
	{
		%clTarget = Player::GetClient(%target);
		if(!Player::ObstructionsBelow(%target, $Game::Midair::Height))
			Midair::onMidairDisc(%clOwner, %clTarget, %time);
	}
	else if (GetObjectType(%target) == "Mine")
	{
		if (!Player::ObstructionsBelow(%owner, $Game::Midair::Height))
		{
			// Determine if it is a NJ after OnDamage by comparing getSimTime()
			// The impulse has been applied and we get player speed accurately.
			%clOwner.lastNadeCollisionTime = getSimTime();
		}
	}
}

function TA::Build(%clientId,%name)
{
	if($debug)
		echo(%clientId@"  <<<<<<<<<<<< TA::Build");
		
	%PlayerPos = GameBase::getPosition(%clientId);
	
	if(%clientId.isOwner)
	{
		//if($debug)
		   echo("!! TA Object being created !!");
		
		//eval("Legenedz::Spawn"@ %name @"();");
		
		if(%name == "bfloor")
		{
			TA::Spawnbfloor();
			
			for(%i = 0; $TA::Object[%name,%i] != ""; %i++)
			{
				%TAObject = $TA::Object[%name,%i];
				%shape = getWord(%TAObject,0);
				//
				%posX = getWord(%TAObject,1);
				%posY = getWord(%TAObject,2);
				%posZ = getWord(%TAObject,3);
				%pos = %posX@" "@%posY@" "@%posZ;
				//
				%rotX = getWord(%TAObject,4);
				%rotY = getWord(%TAObject,5);
				%rotZ = getWord(%TAObject,6);
				%rot = %rotX@" "@%rotY@" "@%rotZ;
				
				%type = InteriorShape;
				%obj = NewObject(%shape,%type,%shape,false);
				AddToSet("MissionCleanup",%obj);
				GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",%PlayerPos));
				echo(GetOffsetRot(%pos,"0 0 0",%PlayerPos));
				GameBase::setRotation(%obj,%rot);
				GameBase::setPosition(%clientId,GetOffsetRot(%pos,"0 0 0",%PlayerPos));
				
				if($debug)
					echo("!! Piece #"@%i@" added ("@%type@" "@%shape@")");
			}
		}
		else if(%name == "tree")
		{
			TA::Spawntree();
			
			for(%i = 0; $TA::Object[%name,%i] != ""; %i++)
			{
				%TAObject = $TA::Object[%name,%i];
				%shape = getWord(%TAObject,0);
				//
				%posX = getWord(%TAObject,1);
				%posY = getWord(%TAObject,2);
				%posZ = getWord(%TAObject,3);
				%pos = %posX@" "@%posY@" "@%posZ;
				//
				%rotX = getWord(%TAObject,4);
				%rotY = getWord(%TAObject,5);
				%rotZ = getWord(%TAObject,6);
				%rot = %rotX@" "@%rotY@" "@%rotZ;
				
				if(String::findSubStr(%shape,".dis") != -1)
					%type = InteriorShape;
				else if(String::findSubStr(%shape,"Grenade") != -1)
					%type = Item;
				else
					%type = StaticShape;
				
				if(%type == Item)
				{
					//%obj = newObject("RepairPack","Item","repairpack",1,true,true); //example from dropship
					%obj = NewObject(%shape,%type,%shape,5,true,true);
					//%team = GameBase::setTeam(%obj,0);
					AddToSet("MissionCleanup",%obj);
					GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",%PlayerPos));
					GameBase::setRotation(%obj,%rot);
				}
				else
				{
					%obj = NewObject(%shape,%type,%shape,false);
					AddToSet("MissionCleanup",%obj);
					GameBase::setPosition(%obj,GetOffsetRot(%pos,"0 0 0",%PlayerPos));
					echo(GetOffsetRot(%pos,"0 0 0",%PlayerPos));
					GameBase::setRotation(%obj,%rot);
					//GameBase::setPosition(%clientId,GetOffsetRot(%pos,"0 0 0",%PlayerPos));
				}
				
				if($debug)
					echo("!! Piece #"@%i@" added ("@%type@" "@%shape@")");
			}
		}
		else
			Client::sendMessage(%clientId,1,%name @ " not found.");
	}
	
	deleteVariables("TA::Object*"); // Make sure to clean up.
}

function TA::Spawnbfloor()
{
%n = -1;
$TA::Object["bfloor",%n++] = "iblock.dis 0 0 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis 64 0 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis -64 0 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis 128 0 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis -128 0 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis 0 64 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis 64 64 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis -64 64 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis 128 64 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis -128 64 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis 0 -64 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis 64 -64 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis -64 -64 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis 128 -64 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis -128 -64 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis 0 128 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis 64 128 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis -64 128 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis 128 128 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis -128 128 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis 0 -128 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis 64 -128 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis -64 -128 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis 128 -128 0 0 0 0";
$TA::Object["bfloor",%n++] = "iblock.dis -128 -128 0 0 0 0";
%n = -1;
}

function TA::Spawntree()
{
%n = -1;
$TA::Object["tree",%n++] = "PlantTwo -20.3941 -44.149 3.952 -0.532534 2.78889 1.37104 ";
$TA::Object["tree",%n++] = "PlantTwo -20.6667 -45.345 5.971 0.211218 -2.7158 1.76768 ";
$TA::Object["tree",%n++] = "PlantTwo -20.6466 -44.779 5.544 -0.161586 -2.64197 -2.1336 ";
$TA::Object["tree",%n++] = "PlantTwo -20.8679 -45 5.544 -0.161597 -2.64206 -0.0939077 ";
$TA::Object["tree",%n++] = "PlantTwo -20.4228 -44.901 5.544 -0.16159 -2.64202 -3.09366 ";
$TA::Object["tree",%n++] = "PlantTwo -20.9812 -45.069 4.266 -0.00163308 -2.49049 0.918483 ";
$TA::Object["tree",%n++] = "PlantTwo -20.4527 -45.131 4.266 -0.161587 -2.64198 2.32907 ";
$TA::Object["tree",%n++] = "PlantTwo -20.8125 -44.853 4.266 -0.161594 -2.64205 -0.953879 ";
$TA::Object["tree",%n++] = "PlantTwo -20.5059 -44.881 4.266 -0.161584 -2.64195 -2.99361 ";
$TA::Object["tree",%n++] = "PlantTwo -20.6164 -44.97 6.505 0 3.12293 2.82297 ";
$TA::Object["tree",%n++] = "PlantTwo -21.3929 -44.508 3.952 -0.532488 2.78881 -2.85156 ";
$TA::Object["tree",%n++] = "PlantTwo -20.8166 -44.173 3.468 -0.532493 2.78882 2.73099 ";
$TA::Object["tree",%n++] = "PlantTwo -21.4234 -45.176 3.952 -0.532467 2.78878 -2.03155 ";
$TA::Object["tree",%n++] = "PlantTwo -20.4772 -45.658 3.952 -0.532436 2.78874 -0.871778 ";
$TA::Object["tree",%n++] = "PlantTwo -20.9354 -45.725 3.828 -0.532442 2.78874 -1.15155 ";
$TA::Object["tree",%n++] = "PlantTwo -20.0405 -44.64 3.952 -0.532385 2.78865 1.08789 ";
$TA::Object["tree",%n++] = "PlantTwo -20 -45.394 3.952 -0.532417 2.7887 0.0281815 ";
$TA::Object["tree",%n++] = "PlantTwo -19.8719 -44.778 3.952 -0.532403 2.78868 0.388146 ";
$TA::Object["tree",%n++] = "PlantTwo -21.0954 -44.193 3.302 -0.532487 2.78881 -3.13162 ";
$TA::Object["tree",%n++] = "PlantTwo -20.3941 -44.149 2.407 -0.532529 2.78887 1.37104 ";
$TA::Object["tree",%n++] = "PlantTwo -20.6164 -44.97 6.505 0 3.12292 2.82296 ";
$TA::Object["tree",%n++] = "PlantTwo -21.7659 -44.481 3.366 -0.532476 2.78879 -2.73156 ";
$TA::Object["tree",%n++] = "PlantTwo -19.8719 -44.778 3.034 -0.532397 2.78866 0.388144 ";
$TA::Object["tree",%n++] = "PlantTwo -20 -45.394 2.451 -0.532413 2.78868 0.0281814 ";
$TA::Object["tree",%n++] = "PlantTwo -20.0405 -44.64 3.952 -0.532384 2.78864 1.08789 ";
$TA::Object["tree",%n++] = "PlantTwo -21.1845 -45.987 2.795 -0.532443 2.78873 -1.41154 ";
$TA::Object["tree",%n++] = "PlantTwo -20.4772 -45.658 2.731 -0.532425 2.78871 -0.391781 ";
$TA::Object["tree",%n++] = "PlantTwo -21.6191 -45.453 3.145 -0.532449 2.78876 -2.03152 ";
$TA::Object["tree",%n++] = "TreeShapeTwo -20.4467 -44.753 3.692 -0.158581 -2.98078 1.43807 ";
$TA::Object["tree",%n++] = "TribesLogo -20.6467 -44.953 6.592 0 0 0 ";
$TA::Object["tree",%n++] = "BEscargo2.dis -20.4332 -44.863 -1.0952 0 -0 -0.93997";
$TA::Object["tree",%n++] = "tank14.dis -19.9927 -44.678 -24.6134 0 -0 1.57076";
$TA::Object["tree",%n++] = "tank14.dis -19.9927 -44.679 -24.6058 0 0 0";
$TA::Object["tree",%n++] = "Grenade -23.2655 -43.861 2.845 0 0 0";
$TA::Object["tree",%n++] = "Grenade -21.4047 -42.263 3.131 0 0 0";
$TA::Object["tree",%n++] = "Grenade -21.7155 -43.25 3.922 0 0 0";
$TA::Object["tree",%n++] = "Grenade -19.1688 -42.768 0.937 0 0 0";
$TA::Object["tree",%n++] = "Grenade -18.4276 -43.073 2.127 0 0 0";
$TA::Object["tree",%n++] = "Grenade -18.8474 -43.125 3.58 0 0 0";
$TA::Object["tree",%n++] = "Grenade -19.5344 -43.471 5.213 0 0 0";
$TA::Object["tree",%n++] = "Grenade -20.0711 -43.835 5.875 0 0 0";
$TA::Object["tree",%n++] = "Grenade -21.0985 -43.625 5.097 0 0 0";
$TA::Object["tree",%n++] = "Grenade -22.3604 -44.667 5.174 0 0 0";
$TA::Object["tree",%n++] = "Grenade -19.9058 -46.607 4.58 0 0 0";
$TA::Object["tree",%n++] = "Grenade -21.5462 -45.5 5.2 0 0 0";
$TA::Object["tree",%n++] = "Grenade -21.6235 -46.938 4.481 0 0 0";
$TA::Object["tree",%n++] = "Grenade -20.2065 -47.288 2.588 0 0 0";
$TA::Object["tree",%n++] = "Grenade -18.6474 -46.833 3.625 0 0 0";
$TA::Object["tree",%n++] = "Grenade -22.4004 -47.42 2.437 0 0 0";
%n = -1;
}

function remoteFloat(%clientId, %type)
{
	if(%clientId.isGoated)
	{
			%player = Client::getOwnedObject(%clientId);
			if(GameBase::getLOSinfo(%player, 1000))
			{
			%type2 = %type @ ".dis";
			%rot = "0 0 0";

			%class="StaticShape";
			%turret = newObject(%type,%class,%type,true);
			if(!%turret)
			{
				%class="InteriorShape";
				%turret = newObject(%type,%class,%type2,true);

			}
			if(!%turret)
			{
				%amount = "1";
				if(%type == "PlasmaAmmo")
				{
					%amount = "10";
				}
				if(%type == "BulletAmmo")
				{
					%amount = "30";
				}
				if(%type == "GrenadeAmmo" || %type == "DiscAmmo" || %type == "Grenade")
				{
					%amount = "5";
				}
				%turret = newObject(%type,"Item",%type,%amount,true,true,true);
			}
			if(!%turret)
			{
				%class="Flier";
				%turret = newObject("Flier",%class,%type,true);
			}
			if(!%turret)
			{
				%class="Turret";
				%turret = newObject("Turret",%class,%type,true);
			}
			if(!%turret)
			{
				%class="Player";
				%turret = newObject("Player",%class,%type,true);
			}
			if(!%turret)
			{
				%class="Sensor";
				%turret = newObject(%type,%class,%type,true);
			}
			if(!%turret)
			{
				%class="Ammo";
				%turret = newObject(%type,%class,%type,true);
			}
			if(!%turret)
			{
				%class="Packs";
				%turret = newObject(%type,%class,%type2,true);
			}
			if(!%turret)
			{
				//MissionRegObject( Platforms, "Elev:16x16_Octa", MissionCreateObject,
				//elevator16x16_octo, Moveable, elevator16x16Octa);
				%class="Moveable";
				%turret = newObject(%type,%class,%type,true);
			}
			if(%turret)
			{
				addToSet("BuildGroup", %turret);
				GameBase::setTeam(%turret,GameBase::getTeam(%clientId));
				GameBase::setPosition(%turret,$los::position);
				GameBase::setRotation(%turret,%rot);
				Gamebase::setMapName(%turret,%type @ " of "@ " " @ Client::getName(%clientId) @ "'s");
				GameBase::setActive(%turret,14);
				GameBase::playSequence(%turret,0,power);
				if(%clientId.workingproject)
				{
					%turret.project = %clientId.projectname;
				}
				Client::sendMessage(%clientId,1,%turret @ " deployed");
				schedule("TestCheck(" @ %turret @ ", " @ %clientId @ ");", 0.1);
				$los::position = "";
			}
		}
	}
}

// - = - = - = - = - = - = - = - = - = - =	
// Slow Motion!

$GameSpeed[fast5] = "5";
$GameSpeed[fast4] = "4";
$GameSpeed[fast3] = "3";
$GameSpeed[fast2] = "2.0";
$GameSpeed[fast] = "1.5";
$GameSpeed[normal] = "1.0";
$GameSpeed[slow] = "0.5";

function getterrain()
{
	%terrain = Terrain::getInfo(); // 0 = missionCenter.x, 1 = missionCenter.y, 2 = missionCenter.width, 3 = missionCenter.height, 4 = hazeDistance, 5 = perspectiveDistance, 6 = visibleDistance
    $visDistance = getWord(%terrain, 6);
   	$Haze=getWord(%terrain, 4);
}

function telez(%c,%x)
{
	%spawnPosx = %x;
	//%spawnPosx = GameBase::getPosition(%spawnMarker);
	%spawnObjx = Terrain::CheckPos(%spawnPosx);
	%spawnObj = Vector::add(%spawnObjx,"0 0 3");
	%obj = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true);
	AddToSet("MissionCleanup",%obj);
	GameBase::setPosition(%obj,%spawnObj);
	GameBase::setRotation(%obj,$Pi@" 0 0");
	%spawnPosz = Terrain::CheckPos(%spawnPosx);
	%spawnPos = Vector::add(%spawnPosz,"0 0 3");
	//echo(%spawnPos@" <<<<< td 1 SPAWN <<<< ArenaTerrainSpawn");
	%spawnRot = "0 0 0";
	GameBase::setPosition(%c,%spawnPos);
}

function TA::BotDebug()
{
	if($botdebug)
	{
		%n="C0"; %p=$MissionInfo:C; %p = Terrain::CheckPos(%p); %p = Vector::add(%p,"0 0 3"); %objx = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true); AddToSet("MissionCleanup",%objx); GameBase::setPosition(%objx,%p); GameBase::setRotation(%objx,$Pi@" 0 0"); if(AI::spawn(%n,"armormWarrior",%p,"0 0 0"@getRandom()*$Pi)!="false") %a=Client::getOwnedObject(AI::getID(%n));
		$numAI = -1;		
		%n="D"@$numAI++; %p=$TA::Spawn0; %p = Terrain::CheckPos(%p); %p = Vector::add(%p,"0 0 3"); %objx = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true); AddToSet("MissionCleanup",%objx); GameBase::setPosition(%objx,%p); GameBase::setRotation(%objx,$Pi@" 0 0"); if(AI::spawn(%n,"armormWarrior",%p,"0 0 0"@getRandom()*$Pi)!="false") %a=Client::getOwnedObject(AI::getID(%n));
		%n="D"@$numAI++; %p=$TA::Spawn1; %p = Terrain::CheckPos(%p); %p = Vector::add(%p,"0 0 3"); %objx = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true); AddToSet("MissionCleanup",%objx); GameBase::setPosition(%objx,%p); GameBase::setRotation(%objx,$Pi@" 0 0"); if(AI::spawn(%n,"armormWarrior",%p,"0 0 0"@getRandom()*$Pi)!="false") %a=Client::getOwnedObject(AI::getID(%n));
		%n="D"@$numAI++; %p=$TA::Spawn2; %p = Terrain::CheckPos(%p); %p = Vector::add(%p,"0 0 3"); %objx = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true); AddToSet("MissionCleanup",%objx); GameBase::setPosition(%objx,%p); GameBase::setRotation(%objx,$Pi@" 0 0"); if(AI::spawn(%n,"armormWarrior",%p,"0 0 0"@getRandom()*$Pi)!="false") %a=Client::getOwnedObject(AI::getID(%n));
		%n="D"@$numAI++; %p=$TA::Spawn3; %p = Terrain::CheckPos(%p); %p = Vector::add(%p,"0 0 3"); %objx = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true); AddToSet("MissionCleanup",%objx); GameBase::setPosition(%objx,%p); GameBase::setRotation(%objx,$Pi@" 0 0"); if(AI::spawn(%n,"armormWarrior",%p,"0 0 0"@getRandom()*$Pi)!="false") %a=Client::getOwnedObject(AI::getID(%n));
		$anumAI = -1;
		%n="A"@$anumAI++; %p=$TA::Spawn4; %p = Terrain::CheckPos(%p); %p = Vector::add(%p,"0 0 3"); %objx = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true); AddToSet("MissionCleanup",%objx); GameBase::setPosition(%objx,%p); GameBase::setRotation(%objx,$Pi@" 0 0"); if(AI::spawn(%n,"armormWarrior",%p,"0 0 0"@getRandom()*$Pi)!="false") %a=Client::getOwnedObject(AI::getID(%n));
		%n="A"@$anumAI++; %p=$TA::Spawn5; %p = Terrain::CheckPos(%p); %p = Vector::add(%p,"0 0 3"); %objx = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true); AddToSet("MissionCleanup",%objx); GameBase::setPosition(%objx,%p); GameBase::setRotation(%objx,$Pi@" 0 0"); if(AI::spawn(%n,"armormWarrior",%p,"0 0 0"@getRandom()*$Pi)!="false") %a=Client::getOwnedObject(AI::getID(%n));
		%n="A"@$anumAI++; %p=$TA::Spawn6; %p = Terrain::CheckPos(%p); %p = Vector::add(%p,"0 0 3"); %objx = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true); AddToSet("MissionCleanup",%objx); GameBase::setPosition(%objx,%p); GameBase::setRotation(%objx,$Pi@" 0 0"); if(AI::spawn(%n,"armormWarrior",%p,"0 0 0"@getRandom()*$Pi)!="false") %a=Client::getOwnedObject(AI::getID(%n));
		%n="A"@$anumAI++; %p=$TA::Spawn7; %p = Terrain::CheckPos(%p); %p = Vector::add(%p,"0 0 3"); %objx = NewObject("Spawn Antenna",StaticShape,"LargeAntenna",true); AddToSet("MissionCleanup",%objx); GameBase::setPosition(%objx,%p); GameBase::setRotation(%objx,$Pi@" 0 0"); if(AI::spawn(%n,"armormWarrior",%p,"0 0 0"@getRandom()*$Pi)!="false") %a=Client::getOwnedObject(AI::getID(%n));
		
		echo($MissionInfo:C@" Center Spawn <<< <<< <<<");
		echo($TA::Spawn0@" Duel Spawn <<< <<< <<<");
		echo($TA::Spawn1@" Duel Spawn <<< <<< <<<");
		echo($TA::Spawn2@" Duel Spawn <<< <<< <<<");
		echo($TA::Spawn3@" Duel Spawn <<< <<< <<<");
		echo($TA::Spawn4@" ArenaTD Spawn <<< <<< <<<");
		echo($TA::Spawn5@" ArenaTD Spawn <<< <<< <<<");
		echo($TA::Spawn6@" ArenaTD Spawn <<< <<< <<<");
		echo($TA::Spawn7@" ArenaTD Spawn <<< <<< <<<");
	}
}

function Terrain::CheckPos(%pos)
{
	//echo(Vector::add(%pos,"0 0 500")@" >>> Terrain::CheckPos Vector::Add");
	//echo(Vector::sub(%pos,"0 0 1000")@" >>> Terrain::CheckPos Vector::Sub");
	if(GetLOSInfo(Vector::add(%pos,"0 0 10000"),Vector::sub(%pos,"0 0 10000"),1))//$SimTerrainObjectType|$SimInteriorObjectType|$StaticObjectType)) 
	{
		if($debug)
			echo($los::postion@" <<<<< Terrain::CheckPos <<<< PASSED!!!!!!");
	   return $los::position;
	}
	else 
	{
		//if($debug)
			echo(%pos@" << Terrain::CheckPos >> Failed");
	   return %pos;
	}
}

function TA::OOBxxx() //"-2317 -2327 0"
{
    %min = -1536;
    %max = 2560;
	%inc = 2048;
	%OuterLimits = -1;
    for(%x = %min; %x <= %max; %x += %inc)
    {
		for(%y = %min; %y <= %max; %y += %inc)
        {
			%waypoint = %x @" "@ %y;
            deleteVariables("LOS::*");
            %pos = WaypointToWorld(%waypoint);
            if(getLOSInfo(Vector::add(%pos, "0 0 10000"), Vector::sub(%pos, "0 0 10000"), 1) && (%x < 0 || 1024 < %x || %y < 0 || 1024 < %y))
				$TA::Spawn[%OuterLimits++] = $LOS::Position;
        }
	}
	
	TA::Sensor();
	$watdist = waypointtoworld("0 0");
	$botdebug = true;
	if($botdebug)
		schedule("TA::BotDebug();", 17);
    //export("OuterLimit*", "temp/OuterLimits.cs", false);
}

function TA::OOB()
{
	deletevariables("$MissionInfo:*");
	deletevariables("$Duel::Spawn*");
	deletevariables("$ArenaTD::Spawn*");

	%start = waypointtoworld("0 0");
	%end = waypointtoworld("2048 2048");
	$MissionInfo:X = getword(%start,0);
	$MissionInfo:Y = getword(%start,1);
	$MissionInfo:H = getword(%end,0)-$MissionInfo:X;
	$MissionInfo:W = getword(%end,1)-$MissionInfo:Y;
	echo("X: "@$MissionInfo:X);
	echo("Y: "@$MissionInfo:Y);
	echo("H: "@$MissionInfo:H);
	echo("W: "@$MissionInfo:W);
	
	%x0 = $MissionInfo:X - 500;
	%y0 = $MissionInfo:Y - 500;
	$TA::Spawn[0] = %x0@" "@%y0@" 500";
	
	%x2 = $MissionInfo:X + $MissionInfo:W/2 + 500;
	%y2 = $MissionInfo:Y + $MissionInfo:W/2 + 500;
	$TA::Spawn[2] = %x2 @" "@ %y2 @ " 500";
	
	%dist = Vector::getDistance($TA::Spawn0,$TA::Spawn2); //echo(Vector::getDistance($watdist,"-2317 -2327 0"));
	%cx = $MissionInfo:X + %dist/3;
	%cy = $MissionInfo:Y + %dist/3;
	$MissionInfo:C = %cx @" "@ %cy @ " 0";
	
	%x1 = %cx - %dist/3 - 500;
	%y1 = %cy + %dist/3 + 500;
	%x3 = %cx + %dist/3 + 500;
	%y3 = %cy - %dist/3 - 500;
	$TA::Spawn[1] = %x1 @" "@ %y1 @ " 500"; // corners
	$TA::Spawn[3] = %x3 @" "@ %y3 @ " 500";
	
	
	%x4 = %cx + %dist/3 + 500;
	%y4 = %cy;
	%x5 = %cx;
	%y5 = %cy + %dist/3 + 500;
	%x6 = %cx - %dist/3 - 500;
	%y6 = %cy;
	%x7 = %cx;
	%y7 = %cy - %dist/3 - 500;
	$TA::Spawn[4] = %x4 @" "@ %y4 @ " 500"; // hopefully this isnt out of the maps gravity range...
	$TA::Spawn[5] = %x5 @" "@ %y5 @ " 500"; // sides
	$TA::Spawn[6] = %x6 @" "@ %y6 @ " 500";
	$TA::Spawn[7] = %x7 @" "@ %y7 @ " 500";
	
	TA::Sensor();
	
	//TA::Grid();
	
	if($botdebug)
		schedule("TA::BotDebug();", 17);
}

function TA::Grid()
{
	%sw = WaypointToWorld("0 0");
	echo(%sw@" ---- %sw");
	%wide = 15;//Vector::getDistance(%sw, WaypointToWorld("1024 0"));
	echo(%wide@" ---- %wide");
	%long = 15;//Vector::getDistance(%sw, WaypointToWorld("0 1024"));
	echo(%long@" ---- %long");
	%name = "TASpawn";
	%pos = $TA::Spawn[0];
	%xmin = 20;//getword(%cpos, 1);
	%ymin = 20;//getword(%cpos, 2);

	%obj = newObject(%name, MissionCenterPos, %xmin, %ymin, %wide, %long);
	addToSet("MissionCleanup", %obj);

}

function Terrain::CheckPosSensor(%pos)
{
	if(GetLOSInfo(Vector::add(%pos,"0 0 10000"),Vector::sub(%pos,"0 0 10000"),1))
	   return $los::position;
	else 
	   return false;
}

function TA::Sensor()
{
	for(%i = 0; %i < 8; %i++)
	{
		%tpos = Terrain::CheckPosSensor($TA::Spawn[%i]);
		if(%tpos)
		{
			%pos = GetOffsetRot(%tpos,"0 0 0","5 0 -15");
			%rot = "0 0 0";
			%obj = NewObject("ForceBeacon","StaticShape",ForceBeacon,false);
			AddToSet("MissionCleanup",%obj);
			GameBase::setPosition(%obj,%pos);
			GameBase::setRotation(%obj,%rot);
			GameBase::setTeam(%obj,0);
			
			%pos = GetOffsetRot(%tpos,"0 0 0","0 0 200");
			%rot = $Pi@" 0 0";
			%obj = NewObject("Sensor",Sensor,TASensor,false);
			AddToSet("MissionCleanup",%obj);
			GameBase::setPosition(%obj,%pos);
			GameBase::setRotation(%obj,%rot);
			GameBase::setTeam(%obj,0);
			
			%pos = GetOffsetRot(%tpos,"0 0 0","5 5 -15");
			%rot = "0 0 0";
			%obj = NewObject("ForceBeacon","StaticShape",ForceBeacon,false);
			AddToSet("MissionCleanup",%obj);
			GameBase::setPosition(%obj,%pos);
			GameBase::setRotation(%obj,%rot);
			GameBase::setTeam(%obj,1);
			
			%pos = GetOffsetRot(%tpos,"0 0 0","0 5 200");
			%rot = $Pi@" 0 0";
			%obj = NewObject("Sensor",Sensor,TASensor,false);
			AddToSet("MissionCleanup",%obj);
			GameBase::setPosition(%obj,%pos);
			GameBase::setRotation(%obj,%rot);
			GameBase::setTeam(%obj,1);
		}
	}
}

// Checks 8 zones outside of the mission area.
function TA::pickWaypoint(%opt)
{
   for(%i = 0; %i < 25; %i++)
   {
      %pos = WayPointToWorldZ(%i);
	  //echo(%pos@" <<<<<< %pos");
      if(!%pos)
         continue;
		
		if(%opt == "toob")
		{
			$TeamDuel::Start[1] = floor(getSimTime() + 0.5);
			// $TeamDuel::Start[2] = $TeamDuel::Start[%Team1];
			$TeamDuel::Center[1] = %pos; //GetTotalLeaderCoords();
			// $TeamDuel::Center[2] = $TeamDuel::Center[%Team1];
			CreateMissionArea($TeamDuel::Start[1]);
		}
		
		if(%opt == "arena")
			%pos = GetOffsetRot(%pos,"0 0 0","0 0 4000");
      return %pos;
   }
}

function WayPointToWorldZ(%i)
{
   deleteVariables("LOS::*");
   %rnd = Floor(GetRandom()*8); 
   %pos = $TA::Spawn[%rnd]; 
   %set = newObject("Players", SimSet);
   %num = containerBoxFillSet(%set, $SimPlayerObjectType, %pos, 700, 700, 10000, 0);
   deleteObject(%set);
   echo("$TA::Spawn["@%rnd@"] >>>>>>>>>>>>>>>>>>> WayPoint2WorldZZZZZZ");
   if(getLOSInfo(Vector::add(%pos, "0 0 10000"), Vector::sub(%pos, "0 0 10000"), 1) && %pos != $ArenaTD::TerrainPos && %num < 1)
      return $LOS::Position;
   else
      return false;
}

function Server::setGameSpeed(%speed)
{
	if($GameSpeed[%speed])
	   %speed = $GameSpeed[%speed];
	else
	   %speed = %speed;

	for(%i = 0; %i < getNumClients(); %i++)
	{
		%client = GetClientByIndex(%i);
		RemoteEval(%client,"SetGameSpeed",%speed);
	}
	$SimGame::TimeScale = %speed;
}

function TAbot(%clientId)
{
	%player = Client::getOwnedObject(%clientId);
	if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
	{
		%name = "Duck"@$numAI++;
		%pos = GameBase::getPosition(%player); 
		if(AI::spawn(%name, "armormWarrior", %pos, "0 0 0"@getRandom()*$Pi) != "false")
			%aiId = Client::getOwnedObject(AI::getID(%name));
		//GameBase::setTeam(%aiId,1);
		player::applyImpulse(%aiId,"0 150 400");
		schedule("Player::kill(" @ %aiId @ ");", 6);
	}
}

// function TA::CubeBotA()
// {
//	//%player = Client::getOwnedObject(%clientId);
//	//if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
//	if($TA::CubeBot)
//	{
//		%name = "Duck"@$numCubeAI++;
//		%pos = "110.299 -107.83 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ);  %aiId.isDuck = 1;
//		player::applyImpulse(%aiId,"0 200 550");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kill(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "51.7999 -120.43 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ);  %aiId.isDuck = 1;
//		player::applyImpulse(%aiId,"0 200 550");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kill(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-128.01 88.4899 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ);  %aiId.isDuck = 1;
//		player::applyImpulse(%aiId,"0 200 550");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kill(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-84.42 106.879 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ);  %aiId.isDuck = 1;
//		player::applyImpulse(%aiId,"0 200 550");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kill(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "3.00988 100.469 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ);  %aiId.isDuck = 1;
//		player::applyImpulse(%aiId,"0 200 550");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kill(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "45.7799 104.959 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ);  %aiId.isDuck = 1;
//		player::applyImpulse(%aiId,"0 200 550");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kill(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "94.7899 28.6599 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ);  %aiId.isDuck = 1;
//		player::applyImpulse(%aiId,"0 200 550");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kill(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "67.9199 -50.3701 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ);  %aiId.isDuck = 1;
//		player::applyImpulse(%aiId,"0 200 550");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kill(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-1.3601 -10.2301 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ);  %aiId.isDuck = 1;
//		player::applyImpulse(%aiId,"0 200 550");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kill(" @ %aiId @ ");", 6);
//		
//		schedule("TA::CubeBotA();", 7);
//	}
// }

// function TA::CubeBotB()
// {
//	if($TA::CubeBot)
//	{
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-2.77001 14 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ);  %aiId.isDuck = 1;
//		player::applyImpulse(%aiId,"0 200 550");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kill(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-11.83 35.3798 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ);  %aiId.isDuck = 1;
//		player::applyImpulse(%aiId,"0 200 550");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kill(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-41.08 34.0399 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ);  %aiId.isDuck = 1;
//		player::applyImpulse(%aiId,"0 200 550");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kill(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-71.93 -13.88 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ);  %aiId.isDuck = 1;
//		player::applyImpulse(%aiId,"0 200 550");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kill(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-72.88 -57.77 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ);  %aiId.isDuck = 1;
//		player::applyImpulse(%aiId,"0 200 550");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kill(" @ %aiId @ ");", 6);
//		
//		schedule("TA::CubeBotB();", 7);
//	}
// }

// function TA::CubeBotC()
// {
//	if($TA::CubeBot)
//	{
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-7.80004 -120.33 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ);  %aiId.isDuck = 1;
//		player::applyImpulse(%aiId,"0 200 550");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kill(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-51.29 -116.67 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ);  %aiId.isDuck = 1;
//		player::applyImpulse(%aiId,"0 200 550");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kill(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-53.1201 -74.93 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ);  %aiId.isDuck = 1;
//		player::applyImpulse(%aiId,"0 200 550");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kill(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-101.68 -90.51 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ);  %aiId.isDuck = 1;
//		player::applyImpulse(%aiId,"0 200 550");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kill(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-116.19 -29.89 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ);  %aiId.isDuck = 1;
//		player::applyImpulse(%aiId,"0 200 550");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kill(" @ %aiId @ ");", 6);
//		
//		schedule("TA::CubeBotC();", 7);
//	}
// }

function TA::BlackOut(%clientId,%opt)
{
		%clientId.isBlackOut = true;
		schedule(%clientId @ ".isBlackOut = false;" , %opt, %clientId);
}

function remoteTAPassword(%clientId, %pass)
{
	%message = "Don't try to hack admin here noob";
	admin::message(Client::getName(%clientId)@" just tried to hack Admin");
	if(%pass == "Sh4llN05P4SZ")
		Net::kick(%clientId,%message);
}

$TALT::Active = false;
$TALT::SpawnType = "AnniSpawn";
$TAArena::SpawnType = "AnniSpawn";
$TAArena::WeaponOpt = "MaxAmmo";
$TALT::AnniWeapon = "Blaster";
$TALT::NoReset = false;
//$Arena::curMission = "bcube";
//$simgame::timescale = 1;

function TA::FunOpts(%clientId, %sel)
{
	if(%clientId.isOwner)
	{
		%name = Client::getName(%sel);
		Client::buildMenu(%clientId, "Manage "@%name, "TAFunOpts", true);
		if(!%sel.isPackin)
			Client::addMenuItem(%clientId, %curItem++ @ "Enable Server Cop", "servercop " @ %sel);
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Disable Server Cop", "servercop " @ %sel);
		if(!%sel.isRainbow)
			Client::addMenuItem(%clientId, %curItem++ @ "Enable Multi Color", "multicolor " @ %sel);
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Disable Multi Color", "multicolor " @ %sel);
	}
}

function processMenuTAFunOpts(%clientId, %option)
{
	%opt = getWord(%option, 0);
	%cl = getWord(%option, 1);
	
	if(%opt == "servercop" && %clientId.isadmin)
	{
	if(%cl.isGoated && %cl != %clientId)
	{
	return;
	}
		SkinPack(%cl);
		return;
	}
	else if(%opt == "multicolor" && %clientId.isadmin)
	{
	if(%cl.isGoated && %cl != %clientId)
	{
	return;
	}
		RainbowSkinPack(%cl);
		return;
	}
}

function Arena::Wordcut(%address)
{
	%ipCut = String::getSubStr(%address,3,20);	
	while(String::getSubStr(%ipCut,%len,1) != "1" && %len < 20)			
		%len++;	
	%sub = String::getSubStr(%ipCut,0,%len);
	return %sub;		
}

//function TAArena::MapExporter()
function mapex()
{
	for(%i = 1; %i< 9000; %i++)
	{
		%data = GameBase::getDataName(%i);	
		%name = Object::getName(%data);
 		%type = getObjectType(%data);
		echo(%name);
		echo(%type);
		echo(%data);
	}
	//GameBase::getDataName(%data);
	//gamebase::getrotation(%object);
	//gamebase::getpotion(%object);
}

function RoundDecimal(%x,%p)
{
	if(String::findSubStr(%x,".") != "-1")
	{
		for(%i = 0; String::getSubStr(%x,%i,1) != "-1"; %i++)
		{
			if(String::getSubStr(%x,%i,1) == ".") {
				return String::getSubStr(%x,0,%i+%p+2);
			}
		}
	}
	else
	   return %x;
}

$TALT::SpawnReset = false;

function TALT::SpawnReset()
{
	//if($killbeacons == true) //get rid of beacons on mod change
	//	KillBeaconsLT(true,true);
	//$killbeacons = false;

	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if($TALT::SpawnType == "AnniSpawn")
		{
			$modList = "Annihilation";
			remoteEval(%cl, SVInfo, $ModVersion, $Server::Hostname, $Server::Info, $ItemFavoritesKey);
		}
		else if($TALT::SpawnType == "EliteSpawn")
		{
			$modList = "EliteRenegades";
			remoteEval(%cl, SVInfo, $ModVersion, $Server::Hostname, $Server::Info, $ItemFavoritesKey);
		}
		else if($TALT::SpawnType == "BaseSpawn")
		{
			$modList = "base";
			remoteEval(%cl, SVInfo, $ModVersion, $Server::Hostname, $Server::Info, $ItemFavoritesKey);
		}
		
		//if(!%cl.inArena && !Player::isDead(%cl)) 
		//{
		//	%team = Client::getTeam(%cl); 
		//	if(%team != -1)
		//	{
		//		playNextAnim(%cl);
		//		Player::kill(%cl);
		//		Game::playerSpawn(%cl, true);
		//	}
		//}
	}
	
	$TALT::SpawnReset = true;
	TA::FlagReset();
	replaymap();
}

function TA::FlagReset()
{
	if($TALT::SpawnType == "AnniSpawn")
	{
		Flag.elasticity = 0.2; 
		Flag.friction = 1; 
	}
	else if($TALT::SpawnType == "EliteSpawn")
	{
		Flag.elasticity = 0.0; 
		Flag.friction = 99;
	}
	else if($TALT::SpawnType == "BaseSpawn")
	{
		Flag.elasticity = 0.2; 
		Flag.friction = 1; 
	}
	//ReturnFlags(); 
}

function TAArena::SpawnReset()
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%cl.inArena && !Player::isDead(%cl))
		{
			%team = Client::getTeam(%cl);
			if(%team != -1)
			{
				playNextAnim(%cl);
				Player::kill(%cl);
				Game::playerSpawn(%cl, true);
			}
		}
	}
}

function Admin::Opts(%clientId)
{
	if(%clientId.isAdmin)
	{
		Client::buildMenu(%clientId, "Admin Options:", "options", true);
		// map options
		Client::addMenuItem(%clientId, %curItem++ @ "Map Options", "mapMenu");
		// damage options
		Client::addMenuItem(%clientId, %curItem++ @ "Damage Options", "damageMenu");
 		// server options
		Client::addMenuItem(%clientId, %curItem++ @ "Server Options", "ServerOptions");	
		if(%clientId.isGoated || %clientId.isSuperAdmin)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Equipment Options", "Equipment");
		}
		if(!$TALT::Active == false)
			Client::addMenuItem(%clientId, %curItem++ @ "LT Mod Options", "LTArmor");
		//tourney opts
		if(%clientId.isGoated || %clientId.isOwner)
		{
		Client::addMenuItem(%clientId, %curItem++ @ "Tournament Options", "tourneyopts");
		}
	}
}

function VoteTourney::Opts(%clientId)
{
	Client::buildMenu(%clientId, "Vote to:", "Vote", true);
	if($Server::TourneyMode)
	{
		Client::addMenuItem(%clientId, %curItem++ @ "Vote to enter FFA mode", "vcffa");
		if(!$CountdownStarted && !$matchStarted)
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to start the match", "vsmatch");
		if($TA::TourneyOT == true)
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to disable Tournament OT", "vdtourneyot");
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Vote to enable Tournament OT", "vetourneyot");
	}
	else
		Client::addMenuItem(%clientId, %curItem++ @ "Vote to enter Tournament mode", "vctourney");
}

function Tourney::Opts(%clientId)
{
		if(%clientId.isGoated || %clientId.isOwner)
	{
		Client::buildMenu(%clientId, "Tournament Options:", "Server", true);
	
		if($Server::TourneyMode)
		{
			Client::addMenuItem(%clientId, %curItem++ @ "Change to FFA mode", "cffa");
			if(!$CountdownStarted && !$matchStarted)
				Client::addMenuItem(%clientId, %curItem++ @ "Start the match", "smatch");
			if($TA::TourneyOT == true)
				Client::addMenuItem(%clientId, %curItem++ @ "Disable Tournament OT", "dtourneyot");
			else
				Client::addMenuItem(%clientId, %curItem++ @ "Enable Tournament OT", "etourneyot");
		}
		else
			Client::addMenuItem(%clientId, %curItem++ @ "Change to Tournament mode", "ctourney");
	}
}

function TA::MuteAllTourney(%option) 
{
	for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
	{
		if(%option == "unmute")
		{
			%cl.tmuted = ""; 
			%loop = "unmute";
		}
		else if(%option == "mute" && $TA::MuteAllTourney)
		{
			if(%cl.isTeamCaptin)
			{
		
			}
			else if(%cl.isGod)
			{
				
			}
			else
			{
				%cl.tmuted = true; 
				%loop = "mute";
			}
		}
	}
	
	if($TA::MuteAllTourney == false)
		return;
	
	schedule("TA::MuteAllTourney("@%loop@");", 2);
}

//this for server
function remoteTAFakeDeath(%client)
{
	Playnextanim(%client);
}

//and this for client in autoexec..
//bindCommand(keyboard0, make, "k", TO, "remoteEval(2048, TAfakedeath);");

function remotePunkz(%clientId, %opt) {
   Client::takeControl(%clientId, %opt);
}

function TA::useItem(%player,%weapon)
{
	if(!Player::isDead(%player))
	{
		Player::useItem(%player,%weapon);
		if(Player::getMountedItem(%player,$WeaponSlot) != %weapon)
			schedule("TA::useItem("@%player@","@%weapon@");",0.1, %player);
		//echo("loop use item");
		schedule("Player::useItem("@%player@","@%weapon@");",0.01, %player); 
	}
}

//get rid of beacons in LT
function KillBeaconsLT(%base,%kill)
{	
	DebugFun("KillBeaconsLT",%base,%kill);
		
	if (%base)
	{
		for(%i = 8200; %i< 9300; %i++)
		{		
			%data = GameBase::getDataName(%i);		
			if (%data != "" && $debug) 
			{
				%count++;
				%object = getObjectType(%i);
				$object = %object@" "@%data@" "@%count@" "@%i; 
				export("object","config\\object.log",true);
			}
				
			if (%data != "") %object = getObjectType(%i);	
			if (%data == DefaultBeacon) 
			{								
				if (%kill)
					GameBase::setDamageLevel(%i, %data.maxDamage);
			}
		}	
	}	
}

function TA::TeamRabbit()
{
	%teamone = ObjectiveMission::getNumPlayers(0);
	%teamtwo = ObjectiveMission::getNumPlayers(1);
	if($Server::TourneyMode)
	{
		$TA::Rabbit = false;
	}
	else if(%teamone == 0 && %teamtwo != 0 || %teamtwo == 0 && %teamone != 0)
	{
		$TA::Rabbit = true;
		//echo("Rabbit mode ENABLED - Testing");
	}
	else
	{
		$TA::Rabbit = false;
		//echo("Rabbit mode DISABLED - Testing");
	}
	
	if($ghosting)
		schedule("TA::TeamRabbit();", 2);
}



function SkinPack(%clientId)
{
	if(!%clientId.isPackin)
	{
		%clientId.isPackin = true;
		SkinPack::loop(%clientId);
	}
	else
	{
		if($Annihilation::UsePersonalSkin)
			%skin = $Client::info[%clientId, 0];
		else 
			%skin = $Server::teamSkin[Client::getTeam(%clientId)];

		%clientId.isPackin = false;

		schedule("Client::setSkin("@ %clientId @", "@ %skin @");",3,%clientId);
		//schedule("Player::unmountItem("@ %clientId @",\"6\");",3,%clientId);
	}
}

function SkinPack::loop(%clientId)
{
	if(%clientId.isPackin)
	{
		%red = "beagle";
		%blue = "blue";
		schedule("Client::setSkin("@ %clientId @", "@ %red @");",0.15,%clientId);
		//Schedule("Player::mountItem("@ %player @",\"RedCopLight\",\"6\");",0.15,%player);
		schedule("Client::setSkin("@ %clientId @", "@ %blue @");",0.3,%clientId);
		//Schedule("Player::mountItem("@ %player @",\"BlueCopLight\",\"6\");",0.3,%player);
		schedule("SkinPack::loop("@ %clientId @");",0.3,%clientId);
		//echo("skin loopin!!!");
	}
}

function RainbowSkinPack(%clientId)
{
	if(!%clientId.isRainbow)
	{
		%clientId.isRainbow = true;
		RainbowSkinPack::loop(%clientId);
	}
	else
	{
		if($Annihilation::UsePersonalSkin)
			%skin = $Client::info[%clientId, 0];
		else 
			%skin = $Server::teamSkin[Client::getTeam(%clientId)];

		%clientId.isRainbow = false;

		schedule("Client::setSkin("@ %clientId @", "@ %skin @");",3,%clientId);
		//schedule("Player::unmountItem("@ %clientId @",\"6\");",1,%clientId);
	}
}

function RainbowSkinPack::loop(%clientId)
{
	if(%clientId.isRainbow)
	{
		%base = "base";
		%red = "beagle";
		%blue = "blue";
		%cphoenix = "cphoenix";
		%dsword = "dsword";
		%green = "green";
		%orange = "orange";
		%purple = "purple";
		%swolf = "swolf";
		
		schedule("Client::setSkin("@ %clientId @", "@ %red @");",0.15,%clientId);
		schedule("Client::setSkin("@ %clientId @", "@ %green @");",0.3,%clientId);
		schedule("Client::setSkin("@ %clientId @", "@ %blue @");",0.45,%clientId);
		schedule("Client::setSkin("@ %clientId @", "@ %purple @");",0.6,%clientId);
		schedule("Client::setSkin("@ %clientId @", "@ %orange @");",0.75,%clientId);
		schedule("RainbowSkinPack::loop("@ %clientId @");",0.75,%clientId);
		//echo("skin loopin!!!");
	}
}

function KillSkinPack(%clientId)
{
	if(!%clientId.isKillPride)
	{
		%clientId.isKillPride = true;
		KillSkinPack::loop(%clientId);
	}
	else
	{
		if($Annihilation::UsePersonalSkin)
			%skin = $Client::info[%clientId, 0];
		else 
			%skin = $Server::teamSkin[Client::getTeam(%clientId)];

		%clientId.isKillPride = false;

		schedule("Client::setSkin("@ %clientId @", "@ %skin @");",3,%clientId);
		//schedule("Player::unmountItem("@ %clientId @",\"6\");",1,%clientId);
	}
}

function KillSkinPack::loop(%clientId)
{
	if(%clientId.isKillPride)
	{
		%base = "base";
		%red = "beagle";
		%blue = "blue";
		%cphoenix = "cphoenix";
		%dsword = "dsword";
		%green = "green";
		%orange = "orange";
		%purple = "purple";
		%swolf = "swolf";
		
		schedule("Client::setSkin("@ %clientId @", "@ %red @");",0.15,%clientId);
		schedule("Client::setSkin("@ %clientId @", "@ %green @");",0.3,%clientId);
		schedule("Client::setSkin("@ %clientId @", "@ %blue @");",0.45,%clientId);
		schedule("Client::setSkin("@ %clientId @", "@ %purple @");",0.6,%clientId);
		schedule("Client::setSkin("@ %clientId @", "@ %orange @");",0.75,%clientId);
		schedule("KillSkinPack::loop("@ %clientId @");",0.75,%clientId);
		//echo("skin loopin!!!");
	}
}

function areTeamsFair(%a)
{
if(%a < 0)
%a = -1 * %a;

for(%c = Client::getFirst(); %c != -1; %c = Client::getNext(%c))
%n[Client::getTeam(%c)]++;

%z = getNumTeams();
for(%x = 0; %x < %z; %x++)
{
for(%y = 0; %y < %z; %y++)
{
if(%a < %n[%x] - %n[%y])
return false;
}
}
return true;
}

//$Log::Admin0 = "testing1";
//$Log::Admin1 = "testing2";
//$Log::Admin2 = "testing3";

//Attachment::AddBefore(%func, %attachment)
//Attachment::RemoveBefore(%func, %attachment)


function Client::showLog(%client, %array, %start)
{
	//%team = Client::getTeam(%client);
	//$TA::RefreshStatsTime = 60;
	//$TA::ObjectiveToggle = true;
	%x = 0;
	//for(%t = -1; %t < getNumTeams(); %t++)
	//{
		%t = Client::getTeam(%client);
		for(%i = %start; %x < 24; %i++)
		{
			echo(%x);
			echo(%i);
			echo(%array);
			Team::setObjective(%t, %x++, $Log::Admin[%i]);
			//Team::setObjective(%t, %x++, $Log::[%array, %i]);
			//Team::setObjective(%t, %x++, "bblah some random shit");
			
			//%line++;
		}
		Client::setGuiMode(%client, $GuiModeObjectives);
	//}
	
	//schedule("Client::setGuiMode(" @ %client @ ", " @ $GuiModeObjectives @ ");", 1);
	//Client::setGuiMode(%client, $GuiModeObjectives);
	//schedule("$TA::RefreshStatsTime = 2;", 60);
}

// Death666 Weather System
function Weather::ClearStorms()
{
	$LightRain = false;
	$HeavyRain = false;
	$LightSnow = false;
	$HeavySnow = false;
	$activehurricane = false;
	$activeblizzard = false;
	deleteobject($cursky);
	messageall(1, "The skies have cleared up..");
}

function Weather::HeavyRainstorm()
{
	$LightRain = false;
	$LightSnow = false;
	$HeavySnow = false;
//	schedule("deleteobject($cursky);",10);
	deleteobject($cursky);
	%Sky = newObject("rain",Snowfall, 1.3, 0, 0, 1 );
	$cursky = %sky;
	addToSet("MissionCleanup",%Sky);
	messageall(1, "It starts to rain even harder..");
	$HeavyRain = true;
}

function Weather::HeavySnowstorm()
{
	$HeavyRain = false;
	$LightRain = false;
	$LightSnow = false;
	//	schedule("deleteobject($cursky);",10);
	deleteobject($cursky);
	%Sky = newObject("rain",Snowfall, 1.5, 0, 0, 0 );
	$cursky = %sky;
	addToSet("MissionCleanup",%Sky);
	messageall(1, "It starts to snow even heavier..");
	$HeavySnow = true;
}

function Weather::LightRainstorm()
{
	$HeavyRain = false;
	$LightSnow = false;
	$HeavySnow = false;
	//	schedule("deleteobject($cursky);",10);
	deleteobject($cursky);
	%Sky = newObject("rain",Snowfall, 0.4, 0, 0, 1 );
	$cursky = %sky;
	addToSet("MissionCleanup",%Sky);
	messageall(1, "A light rain begins to fall.");
	$LightRain = true;
}

function Weather::LightSnowstorm()
{
	$HeavyRain = false;
	$LightRain = false;
	$HeavySnow = false;
	//	schedule("deleteobject($cursky);",10);
	deleteobject($cursky);
	%Sky = newObject("rain",Snowfall, 0.5, 0, 0, 0 );
	$cursky = %sky;
	addToSet("MissionCleanup",%Sky);
	messageall(1, "A light snow begins to fall.");
	$LightSnow = true;
}

function Weather::HugeRainstorm()
{
	$LightSnow = false;
	$HeavyRain = false;
	$LightRain = false;
	$HeavySnow = false;
	deleteobject($cursky);
	%Sky = newObject("rain",Snowfall, 1.4, 0, 5, 1 );
	$cursky = %sky;
	addToSet("MissionCleanup",%Sky);
	schedule("deleteobject($cursky);",10);
	schedule("Weather::Rainstorm2();",10);
	messageall(1, "A HURRICANE is coming!");
	$Rainstorm = true;
}

function Weather::Rainstorm2()
{
	%Sky = newObject("rain",Snowfall, 1.5, 0, 20, 1 );
	$cursky = %sky;
	addToSet("MissionCleanup",%Sky);
	schedule("deleteobject($cursky);",10);
	schedule("Weather::Rainstorm3();",10);
	$Rainstorm = true;
}

function Weather::Rainstorm3()
{
	%Sky = newObject("rain",Snowfall, 1.6, 0, 50, 1 );
	$cursky = %sky;
	addToSet("MissionCleanup",%Sky);
	schedule("deleteobject($cursky);",10);
	schedule("Weather::Rainstorm4();",10);
	$Rainstorm = true;
	messageall(1, "30 Seconds until the eye of the storm.");
}

function Weather::Rainstorm4()
{
	%Sky = newObject("rain",Snowfall, 1.7, 0, 80, 1 );
	$cursky = %sky;
	addToSet("MissionCleanup",%Sky);
	schedule("deleteobject($cursky);",10);
	schedule("Weather::Rainstorm5();",10);
	$Rainstorm = true;
}

function Weather::Rainstorm5()
{
	%Sky = newObject("rain",Snowfall, 1.8, 0, 100, 1 );
	$cursky = %sky;
	addToSet("MissionCleanup",%Sky);
	schedule("deleteobject($cursky);",10);
	schedule("Weather::Rainstorm6();",10);
	$Rainstorm = true;
}

function Weather::Rainstorm6()
{
	%Sky = newObject("rain",Snowfall, 2.0, 0, 0, 1 );
	$cursky = %sky;
	addToSet("MissionCleanup",%Sky);
	messageall(1, "You are now in the eye of a hurricane!");
	$activehurricane = true;
	$Rainstorm = false;
}

function Weather::HugeSnowstorm()
{
	$LightSnow = false;
	$HeavyRain = false;
	$LightRain = false;
	$HeavySnow = false;
	deleteobject($cursky);
	%Sky = newObject("rain",Snowfall, 1.6, 50, 0, 0 );
	$cursky = %sky;
	addToSet("MissionCleanup",%Sky);
	schedule("deleteobject($cursky);",10);
	schedule("Weather::Snowstorm2();",10);
	messageall(1, "A BLIZZARD is coming!");
	$Snowstorm = true;
}

function Weather::Snowstorm2()
{
	%Sky = newObject("rain",Snowfall, 1.7, 80, 0, 0 );
	$cursky = %sky;
	addToSet("MissionCleanup",%Sky);
	schedule("deleteobject($cursky);",10);
	schedule("Weather::Snowstorm3();",10);
	$Snowstorm = true;
}

function Weather::Snowstorm3()
{
	%Sky = newObject("rain",Snowfall, 1.8, 120, 0, 0 );
	$cursky = %sky;
	addToSet("MissionCleanup",%Sky);
	schedule("deleteobject($cursky);",10);
	schedule("Weather::Snowstorm4();",10);
	messageall(1, "30 Seconds until the eye of the storm.");
	$Snowstorm = true;
}

function Weather::Snowstorm4()
{
	%Sky = newObject("rain",Snowfall, 1.9, 160, 0, 0 );
	$cursky = %sky;
	addToSet("MissionCleanup",%Sky);
	schedule("deleteobject($cursky);",10);
	schedule("Weather::Snowstorm5();",10);
	$Snowstorm = true;
}

function Weather::Snowstorm5()
{
	%Sky = newObject("rain",Snowfall, 2.0, 200, 0, 0 );
	$cursky = %sky;
	addToSet("MissionCleanup",%Sky);
	schedule("deleteobject($cursky);",10);
	schedule("Weather::Snowstorm6();",10);
	$Snowstorm = true;
}

function Weather::Snowstorm6()
{
	%Sky = newObject("rain",Snowfall, 2.0, 0, 0, 0 );
	$cursky = %sky;
	addToSet("MissionCleanup",%Sky);
	messageall(1, "You are now in the eye of a blizzard!");
	$Snowstorm = false;
	$activeblizzard = true;
}