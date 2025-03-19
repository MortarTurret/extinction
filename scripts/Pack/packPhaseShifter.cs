$InvList[PhaseShifterPack] = 1;
$MobileInvList[PhaseShifterPack] = 1;
$RemoteInvList[PhaseShifterPack] = 1;
AddItem(PhaseShifterPack);

$WarpTime = 7.5;
$WarpRange = 550; //BR Setting
$WarpChance = 5;
$DropFlagChance = 7.5;

ItemImageData PhaseShifterPackImage
{	
	shapeFile = "discb";
	mountPoint = 2;
	weaponType = 2;
	minEnergy = 10;
	maxEnergy = 10;
	mountOffset = { 0, 0, 0.15 };
	mountRotation = { 1.57, 0, 0 };
	lightType = 3;
	lightRadius = 10;
	lightTime = 10;
	lightColor = { 0.3, 0.1, 0.6 };
	firstPerson = false;
};

ItemData PhaseShifterPack
{	
	description = "Phase Shifter";
	shapeFile = "discb";
	className = "Backpack";
	heading = $InvHead[ihBac];
	shadowDetailMask = 4;
	imageType = PhaseShifterPackImage;
	price = 15;
	hudIcon = "energypack";
	showWeaponBar = true;
	hiliteOnActive = true;
};

function PhaseShifterPackImage::onActivate(%player,%imageSlot)
{	
	%client = Player::getClient(%player);
	%player = Client::getOwnedObject(%client);

	if(%player.hasPhaseShifted)
	{	
		Bottomprint(%client, "<jc>Phase Shift pack not ready");
		Client::sendMessage(%client,0,"~wError_Message.wav");
		PhaseShifterPackImage::onDeactivate(%client,%imageSlot);
		return;
	}
	
	if(!GameBase::getLOSInfo(%player,$WarpRange))
	{
		Bottomprint(%client, "<jc>Phase Shift position out of range");
		PhaseShifterPackImage::onDeactivate(%client,%imageSlot);
		return;
	}
	
	%object = getObjectType($los::object);
	echo("jump to obj "@%object);
	if(%object == "Flier")	// || %object=="Player") 
	{
		Bottomprint(%client, "<jc>You cannot Phase Shift there.");
		Client::sendMessage(%client,0,"~wError_Message.wav");
		PhaseShifterPackImage::onDeactivate(%client,%imageSlot);
		return;
	}

	if(!Client::isItemShoppingOn(%client,PhaseShifterPack) || $build || $Annihilation::QuickInv)
	{		

		if(%object == "Player")
		{
		if(!Player::isAIControlled(%client) && ($Spoonbot::AutoSpawn))
{
		PhaseShifterPackImage::onDeactivate(%client,%imageSlot);
		return;
}
			%otherClient = GameBase::getControlClient($los::object);
			%playerPos = GameBase::getPosition(%player);
			%PlayerVel = Item::getVelocity(%player);
			%OtherPlayerPos = GameBase::getPosition($los::object);
			%otherPlayerVel = Item::getVelocity($los::object);
		
			GameBase::setPosition(%player, %OtherPlayerPos);
			Item::setVelocity(%player, %otherPlayerVel);
			Armor::onShock(%client, %player);
			Client::sendMessage(%client, 0, "You have switched places with "@Client::getName(%otherClient)@".~wshieldhit.wav");
			
			GameBase::setPosition($los::object, %playerPos);
			Item::setVelocity($los::object, %PlayerVel);
			Armor::onBurn(%OtherClient, $los::object);
			Client::sendMessage(%OtherClient, 0, "You have switched places with "@Client::getName(%Client)@".~wshieldhit.wav");
		}	
		else
		{
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
					echo(Client::getName(GameBase::getOwnerClient(%player))@" "@Player::getClient(%player)@" "@%player@" Trying to Jump near flag...");
					Client::sendMessage(%client,0,"~wError_Message.wav");
					Bottomprint(%client, "<jc>You cannot Phase Shift near enemy flags.");
					PhaseShifterPackImage::onDeactivate(%client,%imageSlot);
					deleteObject(%set);	
					return;
				}
			}
			deleteObject(%set);			
			
			//start normal reposition.
			//--------------------
			// hack so players dont 
			// end up in walls
			%obj = getObjectType($los::object);
			%prot = GameBase::getRotation(%player);
			%zRot = getWord(%prot,2);
			%xpos = getWord($los::position,0);
			%ypos = getWord($los::position,1);
			%zpos = getWord($los::position,2);
			//echo($los::normal);
			if(Vector::dot($los::normal,"0 0 1") > 0.6) 
			{
				%zpos += 0.5;//floor
			}
			else 
			{
				if(Vector::dot($los::normal,"0 0 -1") > 0.6) 
				{
					%zpos -= 2;//ceiling
				}
				else if(Vector::dot($los::normal,"0 0 -1") >= -0.1 || Vector::dot($los::normal,"0 0 -1") <= 0.1 ) 
				{
					//wall
					%xopos = getWord($los::normal,0);
					%yopos = getWord($los::normal,1);
					//%xpos = %xpos + %xopos + %xopos;
					%xpos = %xpos + %xopos;
					//%ypos = %ypos + %yopos + %yopos;
					%ypos = %ypos + %yopos;
				}
				else 
				{
					%rot = Vector::getRotation($los::normal);//other
				}
			}
		
			%dest = %xpos@" "@%ypos@" "@%zpos;
		
				schedule("phaseShiftProcess(" @ %player @ ", " @ %xpos @ ", " @ %ypos @ ", " @ %zpos @ ");", 0.25, %player);
				%player.hasPhaseShifted = true;
		
		}
		PhaseShifterPackImage::onDeactivate(%client,%imageSlot);
	}
	else
	{
		Client::sendMessage(%client,0,"~wError_Message.wav");
		Bottomprint(%client, "<jc>You cannot Phase Shift while connected to an Inventory.");	
	} 
}


function PhaseShifterPackImage::onDeactivate(%player,%imageSlot)
{	
	Player::trigger(%player,$BackpackSlot,false);
}

function PhaseShifterPack::onMount(%player,%item)
{	
	if($debug)
		echo("?? EVENT mount "@ %item @" onto player "@ %player @" cl# "@ Player::getclient(%player));	

	%client = Player::getClient(%player);
	if(%client.weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
		Bottomprint(%client, "<jc>Phase Shifter:<f2> Teleport to your crosshair.\n<jc><f2>Switch places with enemies by teleporting directly on them.");
}

function phaseShiftProcess(%player, %xpos, %ypos, %zpos)
{
	%client = Player::getClient(%player);
	%client.isshifting = true;
	GameBase::setPosition(%player, "247 247 -1564");
	schedule("phaseShiftComplete(" @ %player @ ", " @ %xpos @ ", " @ %ypos @ ", " @ %zpos @ ");", 1.50, %player);
	schedule("GameBase::playSound("@%player@",SoundTeleport, 0);",0.1);
}

function phaseShiftComplete(%player, %xpos, %ypos, %zpos)
{
	%client = Player::getClient(%player);
	schedule(%client@".isshifting = false;",0.2,%client);
	Item::setVelocity(%player, 0);
	GameBase::setPosition(%player, %xpos@" "@%ypos@" "@%zpos);
	schedule("phaseShiftRecharge(" @ %player @ ");", $WarpTime, %player);
	schedule("GameBase::playSound("@%player@",SoundTeleport, 0);",0.1);
}

function phaseShiftRecharge(%player)
{
	%player.hasPhaseShifted = false;
	%client = GameBase::getOwnerClient(%player);
	Bottomprint(%client, "<jc>Phase Shift pack ready");
}

//function checkSafeWarp(%client,%player)
//{	
//	if($WarpTime[%client] > 0)
//	{	
//		$WarpTime[%client] -= 1;
//		schedule("checkSafeWarp(" @ %client @ "," @ %player @ ");",1,%player);
//	}
//	else
//	{	
//		$WarpTime[%client] = 0;
//		Bottomprint(%client, "<jc><f1>Phase Shifter field stabilized");
//	}
//}
//
//function WarpWrap(%client,%player,%dest)
//{	
//	if($WarpWrapTime[%client] > 0)
//	{	
//		$WarpWrapTime[%client] -= 0.6;
//		schedule("WarpWrap(" @ %client @ "," @ %player @ ",\"" @ %dest @ "\");",1,%player);
//	}
//	else
//	{	
//		$WarpWrapTime[%client] = 0;
//		GameBase::setPosition(%client,%dest);
//		if($WarpStable[%client] == 2)
//		{	
//			Bottomprint(%client, "<jc><f1>Phase Shift Successful!\n<jc><f0>Phase Shift field restabilizing");
//		}
//		else
//		{	if($WarpStable[%client] == 1)
//			{	
//				Bottomprint(%client, "<jc><f1>Phase Shift Successful!\n<jc><f0>Phase Shift field still unstable");
//			}
//			else
//			{	
//				Bottomprint(%client, "<jc><f1>Phase Shift Unsuccessful!\n<jc><f0>The Phase Shift tears you to apart");
//			}
//		}
//	}
//}
