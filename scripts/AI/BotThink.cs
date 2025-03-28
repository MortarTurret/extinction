
function BotThink::GetFlag(%aiId)
{
	//skip if we already have the flag
	if (Player::getMountedItem(%aiId, $FlagSlot) != -1)
	return;

		$Spoonbot::BotStatus[%aiId] = "Going after Flag";

		%imtheone = 0;
		%aiName = Client::getName(%aiId);
		%team = GameBase::getTeam(%aiId);
		if (%team == 1)
		  %enemyTeam = 0;
		else
		  %enemyTeam = 1;
		%flaghere = BotFuncs::GetFlagId(%enemyTeam);

		if (GameBase::getTeam(%flaghere) != -1)
		{
		   %newbugs = GameBase::GetPosition(%flaghere);
		   for(%cl = Client::getFirst(); %cl != -1; %cl = Client::getNext(%cl))
		   {
			%clteam = Client::getTeam(%cl);
			if(%clteam == %team)
			{
			   %player = Client::getOwnedObject(%cl);
			   if (!Player::isDead(%player))
			   {
				if (Player::getMountedItem(%cl, $FlagSlot) != -1)
				{
					%imtheone = %cl;
				}
			   }
			}
		   }
		   if (%imtheone != 0)
		   {
			%newguy = GameBase::GetPosition(%imtheone);
			BotTree::Getmetopos(%aiId,%newguy, false);
			$Spoonbot::BotStatus[%aiId] = "Chasing Flagcarrier";
		   }
		   else
		   {
			BotTree::Getmetopos(%aiId,%newbugs, false);
			$BotThink::Definitive_Attackpoint[%aiId] = %flaghere;
			$Spoonbot::BotStatus[%aiId] = "Getting enemy flag";
		   }
		}

		// Else play C&H
		else if ($BotFuncs::switchcount>0)
		{
//			messageall(1, "Switch Coint Was Great.");
			%switch = BotFuncs::GetSwitchId(%enemyTeam, %BotPosition);
			if (%switch == -1)
				%switch = BotFuncs::GetSwitchId(%team, %BotPosition);
			if (%switch != -1)
			{
				%newguy = GameBase::GetPosition(%switch);
				BotTree::Getmetopos(%aiId,%newguy, True);
				$Spoonbot::BotStatus[%aiId] = "Capturing Switch";
				%pos1 = GameBase::getPosition(%aiId);
				%pos2 = GameBase::getPosition(%switch);
				%distance = Vector::getDistance(%pos1, %pos2);
				if (%distance<8)
					TowerSwitch::onCollision(%switch, Client::getControlObject(%aiId));
			}
		}


}

function ai::BotBrainsMortar(%aiName)
{
	// messageall(2, "Bot Brains Mortar TOP.");
	
	%aiId = BotFuncs::GetId(%aiName);
	%client = Client::getOwnedObject(%aiId);
	
	if(%client.mortMuted)
	{
//		messageall(1, "A Bot Think Was Stopped.");			
		return;
	}
	
	if ($Spoonbot::MortarBusy[%aiId] == 1 )
	{
		return;
	}
	
	if ($Spoonbot::MedicBusy[%aiId] == 1 )
	{
		return;
	}
	
	if ($Spoonbot::BotStatus[%aiId] == "Dead")
	return;

	%client.mortMuted = 1;
	schedule(%client@".mortMuted=0;",5.0,%client);

//	if ($Spoonbot::BotStatus[%aiId] == "FollowPainter")
//	{
//		messageall(1, "Bot Brain Returned Because Status is Following Painter.");	
//		return;
//	}

    %aiName = Client::getName(%aiId);
	
	// postAction(2050, IDACTION_MOVEUP, 1);
//	AI::Boost(%aiId);
//	ArenaBotBeaconAll(%aiName);
//	schedule("Vehicle::passengerJump(0," @ %aiId @ ",0);", 1);
	
	%armor = player::getarmor(%client);
	%armorlist = $ArmorName[%armor].description;
	
		if(%armorlist == "Troll")
	{
		ai::AnniSwitchWeaponSBTroll(%aiName);
	}
		else if(%armorlist == "Titan")
	{
		ai::AnniSwitchWeaponSBTitan(%aiName);
	}
		else if(%armorlist == "Tank")
	{
		ai::AnniSwitchWeaponSBTank(%aiName);
	}
		else if(%armorlist == "Warrior")
	{
		ai::AnniSwitchWeaponSBWarrior(%aiName);
	}
		else if(%armorlist == "Chameleon Assassin")
	{
		ai::AnniSwitchWeaponSBSpy(%aiName);
	}
		else if(%armorlist == "Builder")
	{
		ai::AnniSwitchWeaponSBBuilder(%aiName);
	}
		else if(%armorlist == "Necromancer")
	{
		ai::AnniSwitchWeaponSBNecro(%aiName);
	}
	
	if (BotTypes::isCMD(%aiName) == 1)
	return;
	
	%player = Client::getOwnedObject(%aiId);

	// Tidy Attacker List
	BotFuncs::TidyAttackerList(%aiId);

	// Generic variables
	%aiTeam = Client::getTeam(%aiId);
	%AiPos = GameBase::getPosition(%aiId);

	if(%aiTeam == 0)
		%EnemyTeam = 1;
	else
		%EnemyTeam = 0;
	
//		%Team = GameBase::getTeam(%player);
//	BotFuncs::TeamMessage(%Team, "Happy Day ~wind2");
	
//	BotPilot::Check(%aiId);

	//if we have a flag do something with it
	//if dont cap within 60 seconds, die to let humans have it
	if ((Player::getMountedItem(%player, $FlagSlot) != -1) || (Player::getMountedItem(%aiId, $FlagSlot) != -1))
	{
	    if ($hasFlag[%aiId]!=True)
	    {
		$hasFlag[%aiId]=True;
		$increaseflag[%aiId] = $increaseflag[%aiId] + 1;
		if ($increaseflag[%aiId] == 100)
		{
			$increaseflag[%aiId] = 0;
			Player::kill(%player);
		}

		%flaghere = BotFuncs::GetFlagId(%aiTeam);
		%newbugs = GameBase::GetPosition(%flaghere);

		$BotThink::ForcedOfftrack[%aiId] = True;
		BotTree::Getmetopos(%aiId,%newbugs, false);
		$BotThink::Definitive_Attackpoint[%aiId] = %flaghere;

		$Spoonbot::BotStatus[%aiId] = "Capping flag";
//		messageall(0, "Bot Status Capping Flag.");

		$ThinkingNow[%aiId] = 0;
		return;
	    }
	return;
	}
	else
	  $hasFlag[%aiId]=False;

	//Follow Painter bots and players with paint-guns enabled
	if(BotTypes::IsMortar(%aiName) == 1)
	{
		$NearestPainter[%aiId]=-1;
		if ($Spoonbot::BotStatus[%aiId] != "FollowPainter")
		{
			BotFuncs::GetNearestPainter(%aiId);
		}
		if ($NearestPainter[%aiId] != -1)
		{
//			messageall(1, "ISMO Nearest painter Not Equal To Neg One.");
			if ($Spoonbot::PainterTarget[$NearestPainter[%aiId]] != -1)
			{
				%objectpos = GameBase::getPosition($Spoonbot::PainterTarget[$NearestPainter[%aiId]]);
//			messageall(1, "ISMO PainterTarget NearestPainter Not Neg One.");
			}
			else
				%objectpos = GameBase::getPosition($NearestPainter[%aiId]);
			if ($Spoonbot::MortarTarget != %objectpos)
				BotTree::Getmetopos(%aiId,%objectpos, false);
				// messageall(1, "ISMO Get Me To Position RAN.");

			$Spoonbot::MortarTarget = %objectpos;

			$LastPainter[%aiId] = $NearestPainter[%aiId];
//			$Spoonbot::BotStatus[%aiId] = "Following Painter " @ $NearestPainter[%aiId];
			$Spoonbot::BotStatus[%aiId] = "FollowPainter";
			// messageall(1, "Following Painter.");
			BotFuncs::TidyAttackerList(%aiId);
			BotThink::NormalBotBehaviour(%aiName, %aiId);
			return;
		}
		 else
			BotThink::NormalBotBehaviour(%aiName, %aiId);
			// messageall(1, "NBB Inside IS Mortar Ran.");
	}
	
//	else
//	{
//		BotThink::NormalBotBehaviour(%aiName, %aiId);
//		messageall(1, "Very Bottom NBB RAN.");
//	}
	
// Tidy Attacker List Again !
	BotFuncs::TidyAttackerList(%aiId);
}

function ai::BotBrainsPainter(%aiName)
{
//	messageall(0, "Bot Brains Painter TOP.");
	
	%aiId = BotFuncs::GetId(%aiName);
	%client = Client::getOwnedObject(%aiId);
	
	if(%client.mortMuted)
	{
//		messageall(1, "A Bot Think Was Stopped.");			
		return;
	}
	
	if ($Spoonbot::MortarBusy[%aiId] == 1 )
	{
		return;
	}
	
	if ($Spoonbot::MedicBusy[%aiId] == 1 )
	{
		return;
	}
	
	if ($Spoonbot::BotStatus[%aiId] == "Dead")
	return;

	%client.mortMuted = 1;
	schedule(%client@".mortMuted=0;",5.0,%client);

//	if ($Spoonbot::BotStatus[%aiId] == "FollowPainter")
//	{
//		messageall(1, "Bot Brain Returned Because Status is Following Painter.");	
//		return;
//	}

    %aiName = Client::getName(%aiId);

	// postAction(2050, IDACTION_MOVEUP, 1);
//	AI::Boost(%aiId);
//	ArenaBotBeaconAll(%aiName);
	%armor = player::getarmor(%client);
	%armorlist = $ArmorName[%armor].description;
	
		if(%armorlist == "Troll")
	{
		ai::AnniSwitchWeaponSBTroll(%aiName);
	}
		else if(%armorlist == "Titan")
	{
		ai::AnniSwitchWeaponSBTitan(%aiName);
	}
		else if(%armorlist == "Tank")
	{
		ai::AnniSwitchWeaponSBTank(%aiName);
	}
		else if(%armorlist == "Warrior")
	{
		ai::AnniSwitchWeaponSBWarrior(%aiName);
	}
		else if(%armorlist == "Chameleon Assassin")
	{
		ai::AnniSwitchWeaponSBSpy(%aiName);
	}
		else if(%armorlist == "Builder")
	{
		ai::AnniSwitchWeaponSBBuilder(%aiName);
	}
		else if(%armorlist == "Necromancer")
	{
		ai::AnniSwitchWeaponSBNecro(%aiName);
	}
	
	if (BotTypes::isCMD(%aiName) == 1)
	return;
	
	%player = Client::getOwnedObject(%aiId);

	// Tidy Attacker List
	BotFuncs::TidyAttackerList(%aiId);

	// Generic variables
	%aiTeam = Client::getTeam(%aiId);
	%AiPos = GameBase::getPosition(%aiId);

	if(%aiTeam == 0)
		%EnemyTeam = 1;
	else
		%EnemyTeam = 0;

	//if we have a flag do something with it
	//if dont cap within 60 seconds, die to let humans have it
	if ((Player::getMountedItem(%player, $FlagSlot) != -1) || (Player::getMountedItem(%aiId, $FlagSlot) != -1))
	{
	    if ($hasFlag[%aiId]!=True)
	    {
		$hasFlag[%aiId]=True;
		$increaseflag[%aiId] = $increaseflag[%aiId] + 1;
		if ($increaseflag[%aiId] == 100)
		{
			$increaseflag[%aiId] = 0;
			Player::kill(%player);
		}

		%flaghere = BotFuncs::GetFlagId(%aiTeam);
		%newbugs = GameBase::GetPosition(%flaghere);

		$BotThink::ForcedOfftrack[%aiId] = True;
		BotTree::Getmetopos(%aiId,%newbugs, false);
		$BotThink::Definitive_Attackpoint[%aiId] = %flaghere;

		$Spoonbot::BotStatus[%aiId] = "Capping flag";
//		messageall(0, "Bot Status Capping Flag.");

		$ThinkingNow[%aiId] = 0;
		return;
	    }
	return;
	}
	else
	  $hasFlag[%aiId]=False;

	if(BotTypes::IsPainter(%aiName) == 1)
	{
		%Target = -1;
		%AttackDefend=%EnemyTeam;
		%BotPosition = GameBase::getPosition(%aiId);

		if (($Spoonbot::PainterTarget[%aiId]==-1) || (GameBase::getDamageLevel($Spoonbot::PainterTarget[%aiId]) >= 0.7) || (GameBase::getTeam($Spoonbot::PainterTarget[%aiId]) == -1))
		{
				%Target = BotFuncs::GetGenId(%AttackDefend, %BotPosition);
				if ((GameBase::getDamageLevel(%Target) >= 0.7) || (GameBase::getTeam(%Target) == -1))
				%Target = -1;
				%TargetType = "Generators";

			if (%Target==-1)
			{
					%Target = BotFuncs::GetGunId(%AttackDefend, %BotPosition);
					if ((GameBase::getDamageLevel(%Target) >= 0.7) || (GameBase::getTeam(%Target) == -1))
					%Target = -1;
					%TargetType = "Guns";
			}
			if (%Target==-1)
			{
				%Target = BotFuncs::GetSensorId(%AttackDefend, %BotPosition);
				if ((GameBase::getDamageLevel(%Target) >= 0.7) || (GameBase::getTeam(%Target) == -1))
					%Target = -1;
				%TargetType = "Sensors";
			}
//			if (%Target==-1)
//			{
//				%Target = BotFuncs::GetVehId(%AttackDefend, %BotPosition);
//				if ((GameBase::getDamageLevel(%Target) >= 0.7) || (GameBase::getTeam(%Target) == -1))
//					%Target = -1;
//				%TargetType = "Sensors";
//			}
	
			$Spoonbot::PainterTarget[%aiId]=%Target;
			if (%Target!=-1)
				BotFuncs::PaintTarget(%aiName, %Target);
			else
				BotThink::GetFlag(%aiId);

		}
// Death666
//		if ($Spoonbot::PainterTarget[%aiId]!=-1)
//		{
//			AI::SetVar(%aiName, triggerPct, 1000 );
//			BotFuncs::PaintTarget(%aiName, $Spoonbot::PainterTarget[%aiId]);
//			%objectpos = GameBase::getPosition(%Target);
//			$BotThink::Definitive_Attackpoint[%aiId] = %Target;
//		}
	}
	
//	else
//	{
//		BotThink::NormalBotBehaviour(%aiName, %aiId);
//		messageall(1, "Very Bottom NBB RAN.");
//	}
	
// Tidy Attacker List Again !
	BotFuncs::TidyAttackerList(%aiId);
}

function ai::BotBrainsMiner(%aiName)
{
	// messageall(2, "Bot Brains Miner TOP.");
	
	%aiId = BotFuncs::GetId(%aiName);
	%client = Client::getOwnedObject(%aiId);
	
	if(%client.mortMuted)
	{
//		messageall(1, "A Bot Think Was Stopped.");			
		return;
	}
	
	if ($Spoonbot::MortarBusy[%aiId] == 1 )
	{
		return;
	}
	
	if ($Spoonbot::MedicBusy[%aiId] == 1 )
	{
		return;
	}
	
	if ($Spoonbot::BotStatus[%aiId] == "Dead")
	return;

	%client.mortMuted = 1;
	schedule(%client@".mortMuted=0;",5.0,%client);
	
    %aiName = Client::getName(%aiId);
	
	// postAction(2050, IDACTION_MOVEUP, 1);
//	AI::Boost(%aiId);
//	ArenaBotBeaconAll(%aiName);
	%armor = player::getarmor(%client);
	%armorlist = $ArmorName[%armor].description;
	
		if(%armorlist == "Troll")
	{
		ai::AnniSwitchWeaponSBTroll(%aiName);
	}
		else if(%armorlist == "Titan")
	{
		ai::AnniSwitchWeaponSBTitan(%aiName);
	}
		else if(%armorlist == "Tank")
	{
		ai::AnniSwitchWeaponSBTank(%aiName);
	}
		else if(%armorlist == "Warrior")
	{
		ai::AnniSwitchWeaponSBWarrior(%aiName);
	}
		else if(%armorlist == "Chameleon Assassin")
	{
		ai::AnniSwitchWeaponSBSpy(%aiName);
	}
		else if(%armorlist == "Builder")
	{
		ai::AnniSwitchWeaponSBBuilder(%aiName);
	}
		else if(%armorlist == "Necromancer")
	{
		ai::AnniSwitchWeaponSBNecro(%aiName);
	}
	
	if (BotTypes::isCMD(%aiName) == 1)
	return;
	
	%player = Client::getOwnedObject(%aiId);

	// Tidy Attacker List
	BotFuncs::TidyAttackerList(%aiId);

	// Generic variables
	%aiTeam = Client::getTeam(%aiId);
	%AiPos = GameBase::getPosition(%aiId);

	if(%aiTeam == 0)
		%EnemyTeam = 1;
	else
		%EnemyTeam = 0;

	//if we have a flag do something with it
	//if dont cap within 60 seconds, die to let humans have it
	if ((Player::getMountedItem(%player, $FlagSlot) != -1) || (Player::getMountedItem(%aiId, $FlagSlot) != -1))
	{
	    if ($hasFlag[%aiId]!=True)
	    {
		$hasFlag[%aiId]=True;
		$increaseflag[%aiId] = $increaseflag[%aiId] + 1;
		if ($increaseflag[%aiId] == 100)
		{
			$increaseflag[%aiId] = 0;
			Player::kill(%player);
		}

		%flaghere = BotFuncs::GetFlagId(%aiTeam);
		%newbugs = GameBase::GetPosition(%flaghere);

		$BotThink::ForcedOfftrack[%aiId] = True;
		BotTree::Getmetopos(%aiId,%newbugs, false);
		$BotThink::Definitive_Attackpoint[%aiId] = %flaghere;

		$Spoonbot::BotStatus[%aiId] = "Capping flag";
//		messageall(0, "Bot Status Capping Flag.");

		$ThinkingNow[%aiId] = 0;
		return;
	    }
	return;
	}
	else
	  $hasFlag[%aiId]=False;
  
	if(BotTypes::IsMiner(%aiName) == 1)
	{
		BotThink::GetFlag(%aiId);
	}
	
//	else
//	{
//		BotThink::NormalBotBehaviour(%aiName, %aiId);
//		messageall(1, "Very Bottom NBB RAN.");
//	}
	
// Tidy Attacker List Again !
	BotFuncs::TidyAttackerList(%aiId);
}

function ai::BotBrainsSniper(%aiName)
{
	// messageall(2, "Bot Brains Sniper TOP.");
	
	%aiId = BotFuncs::GetId(%aiName);
	%client = Client::getOwnedObject(%aiId);
	
	if(%client.mortMuted)
	{
//		messageall(1, "A Bot Think Was Stopped.");			
		return;
	}
	
	if ($Spoonbot::MortarBusy[%aiId] == 1 )
	{
		return;
	}
	
	if ($Spoonbot::MedicBusy[%aiId] == 1 )
	{
		return;
	}
	
	if ($Spoonbot::BotStatus[%aiId] == "Dead")
	return;

	%client.mortMuted = 1;
	schedule(%client@".mortMuted=0;",5.0,%client);
	
    %aiName = Client::getName(%aiId);
	
	// postAction(2050, IDACTION_MOVEUP, 1);
//	AI::Boost(%aiId);
//	ArenaBotBeaconAll(%aiName);
	%armor = player::getarmor(%client);
	%armorlist = $ArmorName[%armor].description;
	
		if(%armorlist == "Troll")
	{
		ai::AnniSwitchWeaponSBTroll(%aiName);
	}
		else if(%armorlist == "Titan")
	{
		ai::AnniSwitchWeaponSBTitan(%aiName);
	}
		else if(%armorlist == "Tank")
	{
		ai::AnniSwitchWeaponSBTank(%aiName);
	}
		else if(%armorlist == "Warrior")
	{
		ai::AnniSwitchWeaponSBWarrior(%aiName);
	}
		else if(%armorlist == "Chameleon Assassin")
	{
		ai::AnniSwitchWeaponSBSpy(%aiName);
	}
		else if(%armorlist == "Builder")
	{
		ai::AnniSwitchWeaponSBBuilder(%aiName);
	}
		else if(%armorlist == "Necromancer")
	{
		ai::AnniSwitchWeaponSBNecro(%aiName);
	}
	
	if (BotTypes::isCMD(%aiName) == 1)
	return;
	
	%player = Client::getOwnedObject(%aiId);

	// Tidy Attacker List
	BotFuncs::TidyAttackerList(%aiId);

	// Generic variables
	%aiTeam = Client::getTeam(%aiId);
	%AiPos = GameBase::getPosition(%aiId);

	if(%aiTeam == 0)
		%EnemyTeam = 1;
	else
		%EnemyTeam = 0;

	//if we have a flag do something with it
	//if dont cap within 60 seconds, die to let humans have it
	if ((Player::getMountedItem(%player, $FlagSlot) != -1) || (Player::getMountedItem(%aiId, $FlagSlot) != -1))
	{
	    if ($hasFlag[%aiId]!=True)
	    {
		$hasFlag[%aiId]=True;
		$increaseflag[%aiId] = $increaseflag[%aiId] + 1;
		if ($increaseflag[%aiId] == 100)
		{
			$increaseflag[%aiId] = 0;
			Player::kill(%player);
		}

		%flaghere = BotFuncs::GetFlagId(%aiTeam);
		%newbugs = GameBase::GetPosition(%flaghere);

		$BotThink::ForcedOfftrack[%aiId] = True;
		BotTree::Getmetopos(%aiId,%newbugs, false);
		$BotThink::Definitive_Attackpoint[%aiId] = %flaghere;

		$Spoonbot::BotStatus[%aiId] = "Capping flag";
//		messageall(0, "Bot Status Capping Flag.");

		$ThinkingNow[%aiId] = 0;
		return;
	    }
	return;
	}
	else
	  $hasFlag[%aiId]=False;
  
	if(BotTypes::IsSniper(%aiName) == 1) // If sniper then try to find a sniper point.
	{

		// AI::SetVar(%aiName, triggerPct, 0.005 );
	 	if ($Spoonbot::DebugMode)
			dbecho(1,%ainame @ " attack due to sniper");
//		BotThink::NormalAttackDefend(%aiId, %aiName, %aiTeam);

		%aiTeam = Client::getTeam(%aiId);
		if(%aiTeam == 0)
		  %EnemyTeam = 1;
		else
		  %EnemyTeam = 0;

		// play C&H
		%switch=-1;
                if ($BotFuncs::switchcount>0)
		{
			%switch = BotFuncs::GetSwitchId(%enemyTeam, %BotPosition);
			if (%switch != -1)
			{
				%newguy = GameBase::GetPosition(%switch);
				BotTree::Getmetopos(%aiId,%newguy, True);
				$Spoonbot::BotStatus[%aiId] = "Capturing Switch";
				%pos1 = GameBase::getPosition(%aiId);
				%pos2 = GameBase::getPosition(%switch);
				%distance = Vector::getDistance(%pos1, %pos2);
				if (%distance<8)
					TowerSwitch::onCollision(%switch, Client::getControlObject(%aiId));
			}
		}
		// No switch around? Ok then search for a sniper spot
		if (%switch==-1)
		{
			//This function is worthless crap!!!
			//BotThink::NormalBotBehaviour(%aiName, %aiId);

			if ($Spoonbot::Target<=0)
			{
				//if no target then:
				//  go through Treepoints
				//  if treepoint has LOS to at least 2 enemy objects
				//  if treepoint is more than 150 units away from any enemy object
				//  if treepoints is outside (no roof above)
				$Spoonbot::Target = BotTree::FindSniperPos(%aiId);
				$Spoonbot::SniperTime[%aiId]=0;
			}

			if ($Spoonbot::Target != -1)
			{
				//  walk towards target
				BotTree::Getmetopos(%aiId,$Spoonbot::Target, False);
				$Spoonbot::BotStatus[%aiId] = "En route";

				if (Vector::getDistance(GameBase::getPosition(%aiId), $Spoonbot::Target[%aiId]) < 30)
					$Spoonbot::SniperTime[%aiId]++;

				if ($Spoonbot::SniperTime[%aiId] >= 40)
					$Spoonbot::Target = -1;
					//  stay put for 40 think cycles, then set target=-1
			}
		}

	}
	
//	else
//	{
//		BotThink::NormalBotBehaviour(%aiName, %aiId);
//		messageall(1, "Very Bottom NBB RAN.");
//	}
	
// Tidy Attacker List Again !
	BotFuncs::TidyAttackerList(%aiId);
}

function ai::BotBrainsDemo(%aiName)
{
	// messageall(2, "Bot Brains Demo TOP.");
	
	%aiId = BotFuncs::GetId(%aiName);
	%client = Client::getOwnedObject(%aiId);
	
	if(%client.mortMuted)
	{
//		messageall(1, "A Bot Think Was Stopped.");			
		return;
	}
	
	if ($Spoonbot::MortarBusy[%aiId] == 1 )
	{
		return;
	}
	
	if ($Spoonbot::MedicBusy[%aiId] == 1 )
	{
		return;
	}
	
	if ($Spoonbot::BotStatus[%aiId] == "Dead")
	return;

	%client.mortMuted = 1;
	schedule(%client@".mortMuted=0;",5.0,%client);
	
    %aiName = Client::getName(%aiId);
	
	// postAction(2050, IDACTION_MOVEUP, 1);
//	AI::Boost(%aiId);
//	ArenaBotBeaconAll(%aiName);
	%armor = player::getarmor(%client);
	%armorlist = $ArmorName[%armor].description;
	
		if(%armorlist == "Troll")
	{
		ai::AnniSwitchWeaponSBTroll(%aiName);
	}
		else if(%armorlist == "Titan")
	{
		ai::AnniSwitchWeaponSBTitan(%aiName);
	}
		else if(%armorlist == "Tank")
	{
		ai::AnniSwitchWeaponSBTank(%aiName);
	}
		else if(%armorlist == "Warrior")
	{
		ai::AnniSwitchWeaponSBWarrior(%aiName);
	}
		else if(%armorlist == "Chameleon Assassin")
	{
		ai::AnniSwitchWeaponSBSpy(%aiName);
	}
		else if(%armorlist == "Builder")
	{
		ai::AnniSwitchWeaponSBBuilder(%aiName);
	}
		else if(%armorlist == "Necromancer")
	{
		ai::AnniSwitchWeaponSBNecro(%aiName);
	}
	
	if (BotTypes::isCMD(%aiName) == 1)
	return;
	
	%player = Client::getOwnedObject(%aiId);

	// Tidy Attacker List
	BotFuncs::TidyAttackerList(%aiId);

	// Generic variables
	%aiTeam = Client::getTeam(%aiId);
	%AiPos = GameBase::getPosition(%aiId);

	if(%aiTeam == 0)
		%EnemyTeam = 1;
	else
		%EnemyTeam = 0;

	//if we have a flag do something with it
	//if dont cap within 60 seconds, die to let humans have it
	if ((Player::getMountedItem(%player, $FlagSlot) != -1) || (Player::getMountedItem(%aiId, $FlagSlot) != -1))
	{
	    if ($hasFlag[%aiId]!=True)
	    {
		$hasFlag[%aiId]=True;
		$increaseflag[%aiId] = $increaseflag[%aiId] + 1;
		if ($increaseflag[%aiId] == 100)
		{
			$increaseflag[%aiId] = 0;
			Player::kill(%player);
		}

		%flaghere = BotFuncs::GetFlagId(%aiTeam);
		%newbugs = GameBase::GetPosition(%flaghere);

		$BotThink::ForcedOfftrack[%aiId] = True;
		BotTree::Getmetopos(%aiId,%newbugs, false);
		$BotThink::Definitive_Attackpoint[%aiId] = %flaghere;

		$Spoonbot::BotStatus[%aiId] = "Capping flag";
//		messageall(0, "Bot Status Capping Flag.");

		$ThinkingNow[%aiId] = 0;
		return;
	    }
	return;
	}
	else
	  $hasFlag[%aiId]=False;
  
	if(BotTypes::IsDemo(%aiName) == 1) // DemoBot uses same stuff as Painter bot
	{
 	if ($Spoonbot::DebugMode)
		dbecho(1, "Demo code running");
 	if ($Spoonbot::DebugMode)
		dbecho(1, "Demo ForcedOffTrack is " @ $BotThink::ForcedOfftrack[%aiId]);


		%aiTeam = Client::getTeam(%aiId);
		if(%aiTeam == 0)
		  %E = 1;
		else
		  %E = 0;
		%enemyteam=%E;
		%Target = -1;
		%AttackDefend=%EnemyTeam;
		%BotPosition = GameBase::getPosition(%aiId);

		// BotFuncs::ScanObjectTree();
		
//	   if (($Spoonbot::Target[%aiId]==-1) || (GameBase::getTeam($BotThink::Definitive_Attackpoint[%aiId] == -1)) || (GameBase::getDamageLevel($Spoonbot::Target[%aiId]) >= 0.7) || (GameBase::getTeam($Spoonbot::Target[%aiId]) == -1))
	   if (($Spoonbot::Target[%aiId]==-1) || (GameBase::getTeam($BotThink::Definitive_Attackpoint[%aiId] == -1)) || (GameBase::getTeam($Spoonbot::Target[%aiId]) == -1))
	   {

			%Target = BotFuncs::GetGenId(%AttackDefend, %BotPosition);
		 	if ($Spoonbot::DebugMode)
			dbecho(1,%Target @ " Powered = " @ GameBase::isPowered(%Target));
//			dbecho(1,%Target @ " Damage = " @ GameBase::getDamageLevel(%Target));
			if ((GameBase::getDamageLevel(%Target) >= 0.7) //Is it already destroyed?
				|| (GameBase::getTeam(%Target) == -1))
				%Target = -1;
			%TargetType = "Generators";

		if (%Target==-1)
		{
			%Target = BotFuncs::GetGunId(%AttackDefend, %BotPosition);
		 	if ($Spoonbot::DebugMode)
			dbecho(1,%Target @ " Powered = " @ GameBase::isPowered(%Target));
			if ( (GameBase::getDamageLevel(%Target) >= 0.7) //Is it already destroyed?
				|| (GameBase::getTeam(%Target) == -1))  // || (GameBase::isPowered(%Target)) )
				%Target = -1;
			%TargetType = "Guns";
		}
		if (%Target==-1)
		{
			%Target = BotFuncs::GetInvId(%AttackDefend, %BotPosition);
		 	if ($Spoonbot::DebugMode)
			dbecho(1,%Target @ " Powered = " @ GameBase::isPowered(%Target));
			if ((GameBase::getDamageLevel(%Target) >= 0.7) //Is it already destroyed?
				|| (GameBase::getTeam(%Target) == -1))
				%Target = -1;
			%TargetType = "Inventory Points";
		}
		if (%Target==-1)
		{
			%Target = BotFuncs::GetVehId(%AttackDefend, %BotPosition);
 	if ($Spoonbot::DebugMode)
			dbecho(1,%Target @ " Powered = " @ GameBase::isPowered(%Target));
			if ((GameBase::getDamageLevel(%Target) == 1.0) //Is it already destroyed?
				|| (GameBase::getTeam(%Target) == -1))
				%Target = -1;
			%TargetType = "Sensors";
		}
		if (%Target==-1)
		{
			%Target = BotFuncs::GetSensorId(%AttackDefend, %BotPosition);
 	if ($Spoonbot::DebugMode)
			dbecho(1,%Target @ " Powered = " @ GameBase::isPowered(%Target));
			if ((GameBase::getDamageLevel(%Target) >= 0.7) //Is it already destroyed?
				|| (GameBase::getTeam(%Target) == -1))
				%Target = -1;
			%TargetType = "Sensors";
		}
		if (%Target==-1)
		{
			%Target = BotFuncs::GetComId(%AttackDefend, %BotPosition);
		 	if ($Spoonbot::DebugMode)
			dbecho(1,%Target @ " Powered = " @ GameBase::isPowered(%Target));
			if ((GameBase::getDamageLevel(%Target) >= 0.7) //Is it already destroyed?
				|| (GameBase::getTeam(%Target) == -1))
				%Target = -1;
			%TargetType = "Command Stations";
		}

		$Spoonbot::Target[%aiId]=%Target;
		if (%Target == -1)
			BotThink::GetFlag(%aiId);

	   }
	   if ($Spoonbot::Target[%aiId]!=-1)
	   {
		BotFuncs::AttackObject(%aiName, %Target);
		$BotThink::Definitive_Attackpoint[%aiId] = %Target;
	   }


	}
	
//	else
//	{
//		BotThink::NormalBotBehaviour(%aiName, %aiId);
//		messageall(1, "Very Bottom NBB RAN.");
//	}
	
// Tidy Attacker List Again !
	BotFuncs::TidyAttackerList(%aiId);
}

function ai::BotBrainsGuard(%aiName)
{
	// messageall(2, "Bot Brains Guard TOP.");
	
	%aiId = BotFuncs::GetId(%aiName);
	%client = Client::getOwnedObject(%aiId);
	
	if(%client.mortMuted)
	{
//		messageall(1, "A Bot Think Was Stopped.");			
		return;
	}
	
	if ($Spoonbot::MortarBusy[%aiId] == 1 )
	{
		return;
	}
	
	if ($Spoonbot::MedicBusy[%aiId] == 1 )
	{
		return;
	}
	
		if(floor(getrandom() * 100) < 25 )
	{
		BotFuncs::Animation(%aiId, %animation);
	}
	
	if ($Spoonbot::BotStatus[%aiId] == "Dead")
	return;

	%client.mortMuted = 1;
	schedule(%client@".mortMuted=0;",5.0,%client);
	
    %aiName = Client::getName(%aiId);
	
	// postAction(2050, IDACTION_MOVEUP, 1);
//	AI::Boost(%aiId);
//	ArenaBotBeaconAll(%aiName);
	%armor = player::getarmor(%client);
	%armorlist = $ArmorName[%armor].description;
	
		if(%armorlist == "Troll")
	{
		ai::AnniSwitchWeaponSBTroll(%aiName);
	}
		else if(%armorlist == "Titan")
	{
		ai::AnniSwitchWeaponSBTitan(%aiName);
	}
		else if(%armorlist == "Tank")
	{
		ai::AnniSwitchWeaponSBTank(%aiName);
	}
		else if(%armorlist == "Warrior")
	{
		ai::AnniSwitchWeaponSBWarrior(%aiName);
	}
		else if(%armorlist == "Chameleon Assassin")
	{
		ai::AnniSwitchWeaponSBSpy(%aiName);
	}
		else if(%armorlist == "Builder")
	{
		ai::AnniSwitchWeaponSBBuilder(%aiName);
	}
		else if(%armorlist == "Necromancer")
	{
		ai::AnniSwitchWeaponSBNecro(%aiName);
	}
	
	if (BotTypes::isCMD(%aiName) == 1)
	return;
	
	%player = Client::getOwnedObject(%aiId);

	// Tidy Attacker List
	BotFuncs::TidyAttackerList(%aiId);

	// Generic variables
	%aiTeam = Client::getTeam(%aiId);
	%AiPos = GameBase::getPosition(%aiId);

	if(%aiTeam == 0)
		%EnemyTeam = 1;
	else
		%EnemyTeam = 0;

	//if we have a flag do something with it
	//if dont cap within 60 seconds, die to let humans have it
	if ((Player::getMountedItem(%player, $FlagSlot) != -1) || (Player::getMountedItem(%aiId, $FlagSlot) != -1))
	{
	    if ($hasFlag[%aiId]!=True)
	    {
		$hasFlag[%aiId]=True;
		$increaseflag[%aiId] = $increaseflag[%aiId] + 1;
		if ($increaseflag[%aiId] == 100)
		{
			$increaseflag[%aiId] = 0;
			Player::kill(%player);
		}

		%flaghere = BotFuncs::GetFlagId(%aiTeam);
		%newbugs = GameBase::GetPosition(%flaghere);

		$BotThink::ForcedOfftrack[%aiId] = True;
		BotTree::Getmetopos(%aiId,%newbugs, false);
		$BotThink::Definitive_Attackpoint[%aiId] = %flaghere;

		$Spoonbot::BotStatus[%aiId] = "Capping flag";
//		messageall(0, "Bot Status Capping Flag.");

		$ThinkingNow[%aiId] = 0;
		return;
	    }
	return;
	}
	else
	  $hasFlag[%aiId]=False;

	if(BotTypes::IsGuard(%aiName) == 1)
	{
		//now sticks to flag and hunts it down if flag is gone
		%imtheone = 0;
		%aiName = Client::getName(%aiId);
		%team = GameBase::getTeam(%aiId);
		%flaghere = BotFuncs::GetFlagId(%team);
		%newbugs = GameBase::GetPosition(%flaghere);
		//Jump up and forward every now and then just in case
		%flyaway = floor(getRandom() * 10); 
		if (%flyaway == 0)
		{
			if ($Spoonbot::BotEvadeUp[%aiId] != 1)
		 	AI::EvadeUp(%aiId);
		}

                if (GameBase::getTeam(%flaghere) != -1)
		{
		   //Enumerate all clients and see if an enemy has flag
		   for(%cl = 2049; %cl < 2120; %cl++)
		   {
			%clteam = Client::getTeam(%cl);
			//Client is not same team as bot
			if(%clteam != %team)
			{
			   %player = Client::getOwnedObject(%cl);
			   if (!Player::isDead(%player))
			   {
				//And enemy has a flag
				if (Player::getMountedItem(%cl, $FlagSlot) != -1)
				%imtheone = %cl;
			   }
			}
		   }
		   //If so then follow that enemy
		   if (%imtheone != 0)
		   {
			%newguy = GameBase::GetPosition(%imtheone);
			BotTree::Getmetopos(%aiId,%newguy, false);

		   }
		   //If not then go right to the flag
		   else
		   {
			BotTree::Getmetopos(%aiId,%newbugs, false);
			$BotThink::Definitive_Attackpoint[%aiId] = %flaghere;
		   }
		}
		// Else defend a C&H-Switch
		else if ($BotFuncs::switchcount>0)
		{
			%switch = BotFuncs::GetSwitchId(%team, %BotPosition);
			if (%switch != -1)
			{
				%newguy = GameBase::GetPosition(%switch);
				BotTree::Getmetopos(%aiId,%newguy, True);
				$Spoonbot::BotStatus[%aiId] = "Capturing Switch";
			}
		}


	}
	
//	else
//	{
//		BotThink::NormalBotBehaviour(%aiName, %aiId);
//		messageall(1, "Very Bottom NBB RAN.");
//	}
	
// Tidy Attacker List Again !
	BotFuncs::TidyAttackerList(%aiId);
}

function ai::BotBrainsMedic(%aiName)
{
	// messageall(2, "Bot Brains Medic TOP.");
	
	%aiId = BotFuncs::GetId(%aiName);
	%client = Client::getOwnedObject(%aiId);
	
	if(%client.mortMuted)
	{
//		messageall(1, "A Bot Think Was Stopped.");			
		return;
	}
	
	if ($Spoonbot::MortarBusy[%aiId] == 1 )
	{
		return;
	}
	
//	if ($Spoonbot::MedicBusy[%aiId] == 1 )
//	{
//		return;
//	}
	
	if ($Spoonbot::BotStatus[%aiId] == "Dead")
	return;

	%client.mortMuted = 1;
	schedule(%client@".mortMuted=0;",5.0,%client);
	
    %aiName = Client::getName(%aiId);
	
	// postAction(2050, IDACTION_MOVEUP, 1);
//	AI::Boost(%aiId);
//	ArenaBotBeaconAll(%aiName);
//		Vehicle::passengerJump(0,(%aiId),0);
	
	%armor = player::getarmor(%client);
	%armorlist = $ArmorName[%armor].description;
	
		if(%armorlist == "Troll")
	{
		ai::AnniSwitchWeaponSBTroll(%aiName);
	}
		else if(%armorlist == "Titan")
	{
		ai::AnniSwitchWeaponSBTitan(%aiName);
	}
		else if(%armorlist == "Tank")
	{
		ai::AnniSwitchWeaponSBTank(%aiName);
	}
		else if(%armorlist == "Warrior")
	{
		ai::AnniSwitchWeaponSBWarrior(%aiName);
	}
		else if(%armorlist == "Chameleon Assassin")
	{
		ai::AnniSwitchWeaponSBSpy(%aiName);
	}
		else if(%armorlist == "Builder")
	{
		ai::AnniSwitchWeaponSBBuilder(%aiName);
	}
		else if(%armorlist == "Necromancer")
	{
		ai::AnniSwitchWeaponSBNecro(%aiName);
	}
	
	if (BotTypes::isCMD(%aiName) == 1)
	return;
	
	%player = Client::getOwnedObject(%aiId);

	// Tidy Attacker List
	BotFuncs::TidyAttackerList(%aiId);

	// Generic variables
	%aiTeam = Client::getTeam(%aiId);
	%AiPos = GameBase::getPosition(%aiId);

	if(%aiTeam == 0)
		%EnemyTeam = 1;
	else
		%EnemyTeam = 0;

	//if we have a flag do something with it
	//if dont cap within 60 seconds, die to let humans have it
	if ((Player::getMountedItem(%player, $FlagSlot) != -1) || (Player::getMountedItem(%aiId, $FlagSlot) != -1))
	{
	    if ($hasFlag[%aiId]!=True)
	    {
		$hasFlag[%aiId]=True;
		$increaseflag[%aiId] = $increaseflag[%aiId] + 1;
		if ($increaseflag[%aiId] == 100)
		{
			$increaseflag[%aiId] = 0;
			Player::kill(%player);
		}

		%flaghere = BotFuncs::GetFlagId(%aiTeam);
		%newbugs = GameBase::GetPosition(%flaghere);

		$BotThink::ForcedOfftrack[%aiId] = True;
		BotTree::Getmetopos(%aiId,%newbugs, false);
		$BotThink::Definitive_Attackpoint[%aiId] = %flaghere;

		$Spoonbot::BotStatus[%aiId] = "Capping flag";
//		messageall(0, "Bot Status Capping Flag.");

		$ThinkingNow[%aiId] = 0;
		return;
	    }
	return;
	}
	else
	  $hasFlag[%aiId]=False;

	if(BotTypes::IsMedic(%aiName) == 1) // If medic then try to find item to repair.
	{

	if ($Spoonbot::DebugMode)
			dbecho(1,%ainame @ "," @ %aiId @ " is medic");
	if ($Spoonbot::DebugMode)
			dbecho(1,%ainame @ " task = " @ $Spoonbot::MedicTask[%aiId]);

			if (($Spoonbot::MedicTask[%aiId] == -1))  
		// || (GameBase::getTeam($BotThink::Definitive_Attackpoint[%aiId] == -1))) 
								// no target to repair, get one!
			{

			 	if ($Spoonbot::DebugMode)
				dbecho(1,%ainame @ " finding task " @ %Task);

				%Task = BotFuncs::GetRepairTask(%aiName);

				if(%Task != -1)
				{
					%object = %Task;

					// %object = $Spoonbot::RepairTaskObject[%task];
					%objectpos = GameBase::getPosition(%object);

				 	if ($Spoonbot::DebugMode)
					dbecho(1,"Target pos = " @ %objectpos);


					$BotThink::ForcedOfftrack[%aiId] = True;

					BotTree::Getmetopos(%aiId,%objectpos, false);

					$Spoonbot::MedicTask[%aiId] = %task;
					$Spoonbot::Target[%aiId] = %object;

					$BotThink::Definitive_Attackpoint[%aiId] = %object;
				}
			}
			
			
			if($Spoonbot::MedicTask[%aiId] != -1) // Have a target ? Close enough to repair ? repaired ?
			{

			 	if ($Spoonbot::DebugMode)
				dbecho(1,%ainame @ " found target");

				%object = $Spoonbot::MedicTask[%aiId];
				%objectpos = GameBase::getPosition(%object);

				%aiPos = GameBase::getPosition(%aiId);

				$Spoonbot::BotStatus[%aiId] = "Approaching damaged " @ GameBase::GetDataName(%object);

				%curDistance = Vector::getDistance(%objectpos,%aipos);  //Check if AI is in range for repairs
		
			 	if ($Spoonbot::DebugMode)
				dbecho(1,"distance = " @ %curdistance);
			
//				if ( (%curDistance < 40) && ((BotFuncs::CheckForLOS(%aiId, %object))||(BotFuncs::CheckForItemLOS(%aiId, %object))) )

				if ( (%curDistance < 40) && (BotFuncs::NumAttackers(%aiId, 4) == 0) )
				{
					if (GameBase::getDamageLevel(%object) == 0.0)
					{
						%rate = 0;
						$Spoonbot::BotStatus[%aiId] = GameBase::GetDataName(%object) @ " repaired!";

						%id = AI::GetID(%aiName); //AIName
						messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Finished repairing " @GameBase::GetDataName(%object)  );

						GameBase::setAutoRepairRate(%object,%rate);
						$Spoonbot::MedicTask[%aiId] = -1;
						%Task=-1;
						$Spoonbot::MedicBusy[%aiId] = 0;
						$Spoonbot::Target[%aiId] = -1;

//						BotFuncs::DeleteAllAttackPointsByPrio(%aiId, 0);
						$BotThink::ForcedOfftrack[%aiId] = True;

						if ($Spoonbot::DebugMode)
							echo ("STATUS BotFuncs::RepairTask = Repair Done");
						%Task = BotFuncs::GetRepairTask(%aiName);
					 	if ($Spoonbot::DebugMode)
							dbecho(1,%ainame @ " finding another task " @ %Task);



						if(%Task != -1)
						{
							%object = %Task;

							%objectpos = GameBase::getPosition(%object);

						 	if ($Spoonbot::DebugMode)
								dbecho(1,"Target pos = " @ %objectpos);

							$BotThink::ForcedOfftrack[%aiId] = True;
							BotTree::Getmetopos(%aiId,%objectpos, false);

							$Spoonbot::MedicTask[%aiId] = %Task;
		
							$BotThink::Definitive_Attackpoint[%aiId] = %object;
						}

						
					}
					else
					{
						if ($Spoonbot::DebugMode)
							dbecho (1, "STATUS BotFuncs::RepairTask = Repairing...");

						playSound(SoundBotRepairItem ,GameBase::getPosition(%aiId));

						%repairRate = 2.5; // 0.3
						%damage = GameBase::getDamageLevel(%object);
						$Spoonbot::MedicBusy[%aiId] = 1;

						$Spoonbot::BotStatus[%aiId] = "Repairing " @ GameBase::GetDataName(%object);
					//	%id = AI::GetID(%aiName); //AIName
					//	messageTeamExcept(%id, 3, ""@Client::getName(%id)@": Reparing " @GameBase::GetDataName(%object)  );
						
						if ($Spoonbot::DebugMode)
							dbecho(1, "STATUS BotFuncs::RepairTask = Object Damage = " @ %damage);

						%difference = %damage - %repairRate;

						if (%difference < 0.0)	//If repair by 10 percent would result in exceeding 100 percent, just repair the missing few percent
						{
							%repairRate = %damage;
						}

						%newDamage = %damage - %repairRate;

						GameBase::setDamageLevel(%object, %newDamage);
					}
				}
			}

			if(($Spoonbot::MedicTask[%aiId] == -1)) 
			{					// no repair task, do normal attack/defend - on your bike for now!
			 	if ($Spoonbot::DebugMode)
					dbecho(1,%ainame @ " no repair target");
			 	if ($Spoonbot::DebugMode)
					dbecho(1,%ainame @ " get flag due to no repair target");

		$Spoonbot::MedicBusy[%aiId] = 0;
		BotThink::GetFlag(%aiId);
			}

	}
	
//	else
//	{
//		BotThink::NormalBotBehaviour(%aiName, %aiId);
//		messageall(1, "Very Bottom NBB RAN.");
//	}
	
// Tidy Attacker List Again !
	BotFuncs::TidyAttackerList(%aiId);
}

function BotThink::getWordCount(%string)
{
    for(%num = 0; getWord(%string, %num) != -1; %num++)
    {} // it's all done above!
    return %num;
}

function BotThink::NormalAttackDefend(%aiId, %aiName, %aiTeam)
{


	if(%aiTeam == 0)
		%EnemyTeam = 1;
	else
		%EnemyTeam = 0;


// First decide if attacking enemy or defending or if we've got an existing destination

		if ((1 == 1) ||		// Fix to always attack a present!
			($BotThink::Definitive_Attackpoint[%aiId] != ""))
		{

			if($BotThink::Definitive_Attackpoint[%aiId] == "")
			{
				//dbecho(1,"no definitive");

				%BotPosition=GameBase::GetPosition(%aiId);

				%Target = -1;
				%LastPoint = False;

				%loopcount = 0;

				while((%Target == -1) && (%loopcount < 10))
				{
					%loopcount = %loopcount + 1;

					//dbecho(1,"Target Loop");

					%AttackDefend = floor(getRandom() * 10); // heavily weighted towards attacking.

					if (%AttackDefend > 1) // Selects team eg if enemy team attack else defend.
						%AttackDefend = %enemyTeam;

					%mod = floor(getRandom() * 8); // 7 Groups + 1 for Extra Flag Selection

					%flagDist = BotFuncs::GetDistanceToFlag(%aiId, BotFuncs::GetFlagId(%AttackDefend));  // makes the bot go for the flag if it's near it.

					if (%flagDist < 5)
						%mod = 7;

					//dbecho(1,"%mod = " @ %mod);

					if (%mod == 1) // Vehicle type Targets
					{
						%Target = BotFuncs::GetVehId(%AttackDefend, %BotPosition);
						//dbecho(1,%Target @ " Damage level = " @ GameBase::getDamageLevel(%Target));
						if (GameBase::getDamageLevel(%Target) == 1.0) //Is it already destroyed?
							{
							%Target = BotFuncs::GetFlagId(%AttackDefend); //If so, go for the flag.
							%LastPoint = False;
							%TargetType = "Flags";
							}

						%TargetType = "Vehicle Pads";
					}
					else if (%mod == 2) // Sensor Type Targets
					{
						%Target = BotFuncs::GetSensorId(%AttackDefend, %BotPosition);

						//dbecho(1,%Target @ " Powered = " @ GameBase::isPowered(%Target));
						
						if(GameBase::isPowered(%Target))			  // Dont attack if it has shields!
							%Target = -1;

						//dbecho(1,%Target @ " Damage level = " @ GameBase::getDamageLevel(%Target));
						if (GameBase::getDamageLevel(%Target) == 1.0) //Is it already destroyed?
							%Target = -1;

						%TargetType = "Sensors";
					}
					else if (%mod == 3) // Gun Type Targets
					{
						%Target = BotFuncs::GetGunId(%AttackDefend, %BotPosition);

						//dbecho(1,%Target @ " Powered = " @ GameBase::isPowered(%Target));
						
						if(GameBase::isPowered(%Target))			  // Dont attack if it has shields!
							%Target = -1;

						//dbecho(1,%Target @ " Damage level = " @ GameBase::getDamageLevel(%Target));

						if (GameBase::getDamageLevel(%Target) == 1.0) //Is it already destroyed?
							%Target = -1;

						%TargetType = "Guns";
					}
					else if (%mod == 4) // Gen Type Targets
					{
						%Target = BotFuncs::GetGenId(%AttackDefend, %BotPosition);

						//dbecho(1,%Target @ " Damage level = " @ GameBase::getDamageLevel(%Target));

						if (GameBase::getDamageLevel(%Target) == 1.0) //Is it already destroyed?
							%Target = -1;
					
						%TargetType = "Generators";
					}
					else if (%mod == 5) // Com Type Targets
					{
						%Target = BotFuncs::GetComId(%AttackDefend, %BotPosition);

						//dbecho(1,%Target @ " Damage level = " @ GameBase::getDamageLevel(%Target));

						if (GameBase::getDamageLevel(%Target) == 1.0) //Is it already destroyed?
							%Target = -1;
						
						%TargetType = "Command Stations";
					}
					else if (%mod == 6) // Inv Type Targets
					{
						%Target = BotFuncs::GetInvId(%AttackDefend, %BotPosition);

						//dbecho(1,%Target @ " Damage level = " @ GameBase::getDamageLevel(%Target));

						if (GameBase::getDamageLevel(%Target) == 1.0) //Is it already destroyed?
							%Target = -1;
						
						%TargetType = "Inventory Points";
					}
					else // Flags etc.
					{
						%Target = BotFuncs::GetFlagId(%AttackDefend, %BotPosition);
						%LastPoint = False;
						
						%TargetType = "Flags";
					}
		
					%Distance = Vector::getDistance(
								$BotTree::Treepoint_Location[BotTree::FindNearestTreebyId(%Target)],
								GameBase::getPosition(%Target));

				//	if ((%Distance > 10) || (%Distance == 0)) // Too far away, needs tuning.
				//	{
				//		%Target = -1;
				//		%TargetType = "Error, Object Deselected";
				//	}

					if(%Target == "")
						%Target = -1;
				}

				//dbecho(1,%aiid @ " Target = " @ %Target );				

				if(%Target != -1)
				{
					%AttackPos = GameBase::getPosition(%Target);
					%AttackPoint = %Target;
					$BotThink::ForcedOfftrack[%aiId] = True;
					$BotThink::LastPoint[%aiId] = False;

					if(%AttackDefend == %enemyteam)
					{
						%Style = "Attacking Enemy";
						$Spoonbot::BotStatus[%aiId] = "Attacking enemy " @ %TargetType; // This is for the ButHud
					}
					else
					{
						%Style = "Defending team";
						$Spoonbot::BotStatus[%aiId] = "Defending team " @ %TargetType; // This is for the ButHud
					}
				
					//BotFuncs::TeamMessage(%aiteam,%AiName @ ": " @ %Style @ " " @ %TargetType );
					//This annoyed me.

					//dbecho(1,%AiName @ ": " @ %Style @ " " @ %TargetType @ ", id = " @ %Target );
				}
			}
			else
			{
				%AttackPoint = $BotThink::Definitive_Attackpoint[%aiId];
				%AttackPos = $BotThink::Definitive_Attackpos[%aiId];
				%LastPoint = $BotThink::LastPoint[%aiId];
			}

// Determine Nearest TreePoint to attack Point.

			if(%Attackpoint != -1)
			{
				BotTree::GetMeToPos(%aiId, %AttackPos, %LastPoint);
				$BotThink::Definitive_Attackpoint[%aiId] = %AttackPoint;
				$BotThink::Definitive_Attackpos[%aiId] = %AttackPos;
				$BotThink::ForcedOfftrack[%aiId] = false;
				$BotThink::LastPoint[%aiId] = %LastPoint;
			}
		}

}




function BotThink::NormalBotBehaviour(%aiName, %aiId)
{
	// messageall(1, "Normal Bot Behavior RAN.");
	
	if ($Spoonbot::DebugMode)
 echo ("STATUS BotThink::Think = Roaming code running...");
	
	%player = Client::getOwnedObject(%aiId);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
	
if ($Spoonbot::Target[%aiId]!=-1)
{
	// messageall(1, "NBB Target isnt negative one.");
	if (!BotFuncs::CheckForLOS(%aiId, $Spoonbot::Target)) 
	{
			// messageall(1, "NBB Target Not in Line Of Sight.");
		AI::DirectiveFollow(%aiId, $Spoonbot::Target, 255);
		schedule("AI::DirectiveRemove(" @ %aiId @ ", 255);", 3);

	}
}

// if ($Spoonbot::Target[%aiId]!=-1)
//	if (!BotFuncs::CheckAlive($Spoonbot::Target))
//		return;

		%attacking = False;
		if ($Spoonbot::HuntFlagrunner == 0)      //Check if in Normal mode or if bots are chasing a flagrunner
		{
		// messageall(1, "NBB Not Chasing A Flag Runner.");
		%mode = floor(getRandom() * 3);      //There are two modes of operation: 1) Attack random enemy and 2) attack/defend objects
if ($Spoonbot::DebugMode)
 echo ("STATUS BotThink::Think MODE = " @ %mode);

		  if (%mode == 0)
		  {
			// messageall(1, "NBB Mode Zero Rolled.");

			%spawnIdx = floor(getRandom() * ($Spoonbot::NumBots - 0.1));  //Add a random variation to the target selection

			//   %startCl = Client::getFirst() + %spawnIdx;  //This has caused the bots to not attack unless a player is present.

			%startCl = 2048 + %spawnIdx;                  

			%endCl = %startCl + 90;
   
			for(%cl = %startCl; ((%cl < %endCl) && (%attacking == False)); %cl++)
			{
		        %targetTeam = Client::getTeam(%cl);

				if (%targetTeam != %aiTeam)           //Is this an enemy?
				{


					%player = Client::getOwnedObject(%cl);
					if (!Player::isDead(%player))
				    {
						%enemyName = Client::getName(%cl);
						if (%enemyName != "")
						{

							%objectpos = GameBase::getPosition(%cl);
							BotTree::Getmetopos(%aiId,%objectpos, false);

							%objectpos = GameBase::getPosition(%aiId);
							BotTree::Getmetopos(%cl,%objectpos, false);

							$Spoonbot::Target[%aiId]=%cl;
							$Spoonbot::BotStatus[%aiId] = "Attacking";
							if ($Spoonbot::DebugMode)
							  echo ("STATUS BotThink::Think = " @ %aiName @ " has decided to attack " @ %enemyName);
						    	%attacking = True;
						}
					}
				}
			}
		  }
		  else if (%mode >= 1) //mode 1 and 2 mean attack/defend base equipment
		  {
			// messageall(1, "NBB Mode One or Two Rolled.");
	if ($Spoonbot::DebugMode)
			dbecho(1,%ainame @ " attack due to roaming atackdefend");

			BotThink::NormalAttackDefend(%aiId, %aiName, %aiTeam);

		  }  //End of mode 1 and 2



		}
			//end of roaming part, now comes the flagrunner-chasing part.
			//----------All this stuff is now inside BotFuncs.cs------------
		else
		{
			// messageall(1, "NBB Chase Mode Activated.");
			%cl = $Spoonbot::HuntFlagrunner;
			%aiTeam = Client::getTeam(%aiId);
			%targetTeam = Client::getTeam(%cl);
			if (%targetTeam != %aiTeam)           //Is this an enemy?
			{
				%player = Client::getOwnedObject(%cl);
				if (!Player::isDead(%player))
				{
					%enemyName = Client::getName(%cl);
//					BotFuncs::AddAttacker(%cl,%aiName,10,4);
//					BotFuncs::AddAttacker(%aiName,%cl,10,4);

					%objectpos = GameBase::getPosition(%cl);
					BotTree::Getmetopos(%aiid,%objectpos, false);
					%objectpos = GameBase::getPosition(%aiId);
					BotTree::Getmetopos(%cl,%objectpos, false);


					$Spoonbot::Target[%aiId]=%cl;
					if ($Spoonbot::DebugMode)
					  echo (%aiName @ " has decided to attack enemy flagrunner" @ %enemyName);
					$Spoonbot::BotStatus[%aiId] = "Attacking Flagrunner";

				}
			}
		        else				    //else this is a friendly, so let's escort him to safety.
		        {
					// messageall(1, "NBB Escort A Friendly Capper Activated.");
		          %player = Client::getOwnedObject(%cl);
		 	  if (!Player::isDead(%player))
			  {
			        %enemyName = Client::getName(%cl);
			        if (%enemyName != "")
				{

					%objectpos = GameBase::getPosition(%cl);
					BotTree::Getmetopos(%aiid,%objectpos, false);
					%objectpos = GameBase::getPosition(%aiId);
					BotTree::Getmetopos(%cl,%objectpos, false);

					$Spoonbot::Target[%aiId]=%cl;
					if ($Spoonbot::DebugMode)
				   	  echo(%aiName @ " is escorting the friendly flagrunner " @ %enemyName);
					$Spoonbot::BotStatus[%aiId] = "Escorting Flagrunner";

				}
		 	   }
		        }
//--------------------------------------------------------------
		}

// End of Original Roam Code

}
