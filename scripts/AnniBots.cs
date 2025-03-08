function AnniFlyBuilder(%aiName, %flytime)
{

    %id = AI::GetID(%aiName);

    if(!$Actions::Flying[%id]) 
    {
        return;
    }

	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);
	
	if(%loc == "0 0 0")
    {
	return;
    }

	if(%flytime > 0)
	{
		%vel = Item::getVelocity(%id);
		%velZ = getWord(%vel,2);
		%velx = getWord(%vel,0);
		%vely = getWord(%vel,1);

		if(%velz > 35)
		{
			%flytime = 1;
		}
		%flytime--;
		$DoingChore[%Id] = true;
		schedule("AnniFlyBuilder(" @ %ainame @ ", " @ %flytime @ ");",0.1);
		Player::applyImpulse(%id, "0 0 38");
		Player::setAnimation(%id, 19);
		ixApplyKickback(%id, -6, 15);
	}
	else if (%flytime < 2)
	{
	 $DoingChore[%id] = false;
	}

}

function AnniFlyNecromancer(%aiName, %flytime)
{
	%id = AI::GetID(%aiName);

    if(!$Actions::Flying[%id]) 
    {
        return;
    }

	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);
	
	if(%loc == "0 0 0")
    {
	return;
    }
	
	if(%flytime > 0)
	{

		%vel = Item::getVelocity(%id);
		%velZ = getWord(%vel,2);
		%velx = getWord(%vel,0);
		%vely = getWord(%vel,1);

		if(%velz > 35)
		{
			%flytime = 1;
		}
		%flytime--;
		$DoingChore[%Id] = true;
		schedule("AnniFlyNecromancer(" @ %ainame @ ", " @ %flytime @ ");",0.1);
		Player::applyImpulse(%id, "0 0 23");
		Player::setAnimation(%id, 19);
		ixApplyKickback(%id, -6, 15);
	}
	else if (%flytime < 2)
	{
	 $DoingChore[%id] = false;
	}

}

function AnniFlyWarrior(%aiName, %flytime)
{

    %id = AI::GetID(%aiName);

    if(!$Actions::Flying[%id]) 
    {
        return;
    }

	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);
	
	if(%loc == "0 0 0")
    {
	return;
    }
	
	if(%flytime > 0)
	{
		%vel = Item::getVelocity(%id);
		%velZ = getWord(%vel,2);
		%velx = getWord(%vel,0);
		%vely = getWord(%vel,1);

		if(%velz > 35)
		{
			%flytime = 1;
		}
		%flytime--;
		$DoingChore[%Id] = true;
		schedule("AnniFlyWarrior(" @ %ainame @ ", " @ %flytime @ ");",0.1);
		Player::applyImpulse(%id, "0 0 34");
		Player::setAnimation(%id, 19);
		ixApplyKickback(%id, -6, 15);
	}
	else if (%flytime < 2)
	{
	 $DoingChore[%id] = false;
	}

}

function AnniFlyAssassin(%aiName, %flytime)
{
    %id = AI::GetID(%aiName);

    if(!$Actions::Flying[%id]) 
    {
        return;
    }

	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);
	
	if(%loc == "0 0 0")
    {
	return;
    }
	
	if(%flytime > 0)
	{
		%vel = Item::getVelocity(%id);
		%velZ = getWord(%vel,2);
		%velx = getWord(%vel,0);
		%vely = getWord(%vel,1);
		
		if(%velz > 35)
		{
			%flytime = 1;
		}
		%flytime--;
		$DoingChore[%Id] = true;
		schedule("AnniFlyAssassin(" @ %ainame @ ", " @ %flytime @ ");",0.1);
		Player::applyImpulse(%id, "0 0 24");
		Player::setAnimation(%id, 19);
		ixApplyKickback(%id, -6, 15);
	}
	else if (%flytime < 2)
	{
	 $DoingChore[%id] = false;
	}

}

function AnniFlyAngel(%aiName, %flytime)
{

    %id = AI::GetID(%aiName);

    if(!$Actions::Flying[%id]) 
    {
        return;
    }

	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);
	
	if(%loc == "0 0 0")
    {
	return;
    }
	
	if(%flytime > 0)
	{

		%vel = Item::getVelocity(%id);
		%velZ = getWord(%vel,2);
		%velx = getWord(%vel,0);
		%vely = getWord(%vel,1);

		if(%velz > 35)
		{
			%flytime = 1;
		}
		%flytime--;
		$DoingChore[%Id] = true;
		schedule("AnniFlyAngel(" @ %ainame @ ", " @ %flytime @ ");",0.1);
		Player::applyImpulse(%id, "0 0 25");
		Player::setAnimation(%id, 19);
		ixApplyKickback(%id, -6, 15);
	}
	else if (%flytime < 2)
	{
	 $DoingChore[%id] = false;
	}
}

function AnniFlyTitan(%aiName, %flytime)
{

    %id = AI::GetID(%aiName);

    if(!$Actions::Flying[%id]) 
    {
        return;
    }

	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);
	
	if(%loc == "0 0 0")
    {
	return;
    }

	if(%flytime > 0)
	{
		%vel = Item::getVelocity(%id);
		%velZ = getWord(%vel,2);
		%velx = getWord(%vel,0);
		%vely = getWord(%vel,1);

		if(%velz > 35)
		{
			%flytime = 1;
		}
		%flytime--;
		$DoingChore[%Id] = true;
		schedule("AnniFlyTitan(" @ %ainame @ ", " @ %flytime @ ");",0.1);
		Player::applyImpulse(%id, "0 0 47");
		Player::setAnimation(%id, 19);
		ixApplyKickback(%id, -6, 15);
	}
	else if (%flytime < 2)
	{
	 $DoingChore[%id] = false;
	}

}

function AnniFlyTank(%aiName, %flytime)
{

    %id = AI::GetID(%aiName);

    if(!$Actions::Flying[%id]) 
    {
        return;
    }

	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);
	
	if(%loc == "0 0 0")
    {
	return;
    }

	if(%flytime > 0)
	{
		%vel = Item::getVelocity(%id);
		%velZ = getWord(%vel,2);
		%velx = getWord(%vel,0);
		%vely = getWord(%vel,1);
		if(%velz > 35)
		{
			%flytime = 1;
		}
		%flytime--;
		$DoingChore[%Id] = true;
		schedule("AnniFlyTank(" @ %ainame @ ", " @ %flytime @ ");",0.1);
		Player::applyImpulse(%id, "0 0 47");
		Player::setAnimation(%id, 19);
		ixApplyKickback(%id, -6, 15);
	}
	else if (%flytime < 2)
	{
	 $DoingChore[%id] = false;
	}
}

function AnniFlyTroll(%aiName, %flytime)
{

    %id = AI::GetID(%aiName);

    if(!$Actions::Flying[%id]) 
    {
        return;
    }

	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);
	
	if(%loc == "0 0 0")
    {
	return;
    }
	
	if(%flytime > 0)
	{

		%vel = Item::getVelocity(%id);
		%velZ = getWord(%vel,2);
		%velx = getWord(%vel,0);
		%vely = getWord(%vel,1);

		if(%velz > 35)
		{
			%flytime = 1;
		}

		%flytime--;
		$DoingChore[%Id] = true;
		schedule("AnniFlyTroll(" @ %ainame @ ", " @ %flytime @ ");",0.1);
		Player::applyImpulse(%id, "0 0 47");
		Player::setAnimation(%id, 19);
		ixApplyKickback(%id, -6, 15);
	}
	else if (%flytime < 2)
	{
	 $DoingChore[%id] = false;
	}
}

function AnniBotsRoam(%aiName)
{
//          if($Guarding[%aiName] == true)
//          return false;

  
	%id = AI::GetID(%aiName);
	
		if($Actions::Guarding[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);
	
	if(%loc == "0 0 0")
    {
	return;
    }
	
		// messageall(1, "Anni Bots Roam.");

 	  %gotoMarker = GameBase::getPosition(%id);
 	  %xPos = getWord(%gotoMarker, 0) + (floor(getRandom() * 50)-25);
 	  %yPos = getword(%gotoMarker, 1) + (floor(getRandom() * 50)-25);
 	  %zPos = 0;

	  %aiGotoPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;

	AI::DirectiveWaypoint(%aiName, %aiGotoPos, 1);
}

function AnniBotsHuntTank(%aiName)
{
//	messageall(1, "Anni Bots Hunt.");
	
	%id = AI::GetID(%AIName);
	
    if(!$Actions::Hunting[%id]) 
    {
        return;
    }

	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }	
		
			%curTarget = ai::getTarget( %aiName );

	   if((%curTarget != -1) && ($Actions::Deploys[%id] != "false"))
	   {
		schedule("AnniBotsHuntTank(" @ %ainame @ ");",1);

	    if(!$DoingChore[%id])
	    {
			%vel = Item::getVelocity(%id);
			%velZ = getWord(%vel,2);
			%velx = getWord(%vel,0);
			%vely = getWord(%vel,1);

			if(%velz < -18)
			{
				$Actions::Flying[%id] = true;
				AnniFlyTank(%aiName, 25);
				return;
			}
			%chore = floor(getRandom() * 6);
			if(%chore == 1)
			{
				$Actions::Flying[%id] = true;
				AnniFlyTank(%aiName, 25);
				AnniBotsRoam(%aiName);
				return;
			}
			if(%chore == 3)
			{
				$Actions::Flying[%id] = true;
        		AnniFlyTank(%aiName, 25);
				AnniBotsRoam(%aiName);
				return;
			}
	    }
	   }
	   if((%curTarget == -1) || ($Actions::Deploys[%id] == "false"))
	   {
//			%CurrentSpread = GameBase::getDamageLevel(%player);
//			%armor = Player::getArmor(%player);
//			%Toast = %armor.maxDamage;
//			%Butter = "0.0003";

//	if(%CurrentSpread + %Butter > %Toast)
//	{
//		%curDie = radnomItems(4, $PlayerAnim::DieHead,$PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
//		Player::setAnimation(%player, %curDie);
//		playNextAnim(%player);
//		Player::kill(%player);
//		ArenaMSG(0,"TankBot died.");
//		return;
//	}
		
//	GameBase::setDamageLevel(%player,GameBase::getDamageLevel(%player) + 0.0015); // 0.0003
	%player.cnt++;
	if(%player.cnt >= 25) // 120
	{
		%curDie = radnomItems(4, $PlayerAnim::DieHead,$PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
		Player::setAnimation(%player, %curDie);
		playNextAnim(%player);
		Player::kill(%player);
		ArenaMSG(0,"TankBot died.");
		return;	
	}
			%roamchore = floor(getRandom() * 4);
			%aiId = AI::GetID(%AIName);
			if(%roamchore == 0)
			{
				AnniBotsRoam(%aiName);
			}
			if(%roamchore == 1)
			{
				AnniBotsRoam(%aiName);
			}
			if(%roamchore == 2)
			{
				BotFuncs::Animation(%aiId, %animation);
			}
			if(%roamchore == 3)
			{
				BotFuncs::Animation(%aiId, %animation);
			}
			
		schedule("AnniBotsHuntTank(" @ %ainame @ ");",5);
		return;
	   }
}

function AnniBotsHuntTroll(%aiName)
{
	%id = AI::GetID(%AIName);
	
    if(!$Actions::Hunting[%id]) 
    {
        return;
    }

	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }	
		
			%curTarget = ai::getTarget( %aiName );
	   if((%curTarget != -1) && ($Actions::Deploys[%id] != "false"))
	   {
		schedule("AnniBotsHuntTroll(" @ %ainame @ ");",1);
	    if(!$DoingChore[%id])
	    {
			%vel = Item::getVelocity(%id);
			%velZ = getWord(%vel,2);
			%velx = getWord(%vel,0);
			%vely = getWord(%vel,1);

			if(%velz < -18)
			{
				$Actions::Flying[%id] = true;
				AnniFlyTroll(%aiName, 25);
				return;
			}
			%chore = floor(getRandom() * 6);
			if(%chore == 1)
			{
				$Actions::Flying[%id] = true;
				AnniFlyTroll(%aiName, 25);
				AnniBotsRoam(%aiName);
				return;
			}
			if(%chore == 3)
			{
				$Actions::Flying[%id] = true;
        		AnniFlyTroll(%aiName, 25);
				AnniBotsRoam(%aiName);
				return;
			}
	    }
	   }
	   if((%curTarget == -1) || ($Actions::Deploys[%id] == "false"))
	   {
		%player.cnt++;
	if(%player.cnt >= 25)
	{
		%curDie = radnomItems(4, $PlayerAnim::DieHead,$PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
		Player::setAnimation(%player, %curDie);
		playNextAnim(%player);
		Player::kill(%player);
		ArenaMSG(0,"TrollBot died.");
		return;	
	}
			%roamchore = floor(getRandom() * 4);
			%aiId = AI::GetID(%AIName);
			if(%roamchore == 0)
			{
				AnniBotsRoam(%aiName);
			}
			if(%roamchore == 1)
			{
				AnniBotsRoam(%aiName);
			}
			if(%roamchore == 2)
			{
				BotFuncs::Animation(%aiId, %animation);
			}
			if(%roamchore == 3)
			{
				BotFuncs::Animation(%aiId, %animation);
			}
		schedule("AnniBotsHuntTroll(" @ %ainame @ ");",5);
		return;
	   }
}

function AnniBotsHuntTitan(%aiName)
{
	%id = AI::GetID(%AIName);
	
    if(!$Actions::Hunting[%id]) 
    {
        return;
    }

	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }	
		
			%curTarget = ai::getTarget( %aiName );
	   if((%curTarget != -1) && ($Actions::Deploys[%id] != "false"))
	   {
		schedule("AnniBotsHuntTitan(" @ %ainame @ ");",1);

	    if(!$DoingChore[%id])
	    {
			%vel = Item::getVelocity(%id);
			%velZ = getWord(%vel,2);
			%velx = getWord(%vel,0);
			%vely = getWord(%vel,1);

			if(%velz < -18)
			{
				$Actions::Flying[%id] = true;
				AnniFlyTitan(%aiName, 25);
				return;
			}
			%chore = floor(getRandom() * 6);
			if(%chore == 1)
			{
				$Actions::Flying[%id] = true;
				AnniFlyTitan(%aiName, 25);
				AnniBotsRoam(%aiName);
				return;
			}
			if(%chore == 3)
			{
				$Actions::Flying[%id] = true;
        		AnniFlyTitan(%aiName, 25);
				AnniBotsRoam(%aiName);
				return;
			}

	    }
	   }
	   if((%curTarget == -1) || ($Actions::Deploys[%id] == "false"))
	   {
		%player.cnt++;
	if(%player.cnt >= 25)
	{
		%curDie = radnomItems(4, $PlayerAnim::DieHead,$PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
		Player::setAnimation(%player, %curDie);
		playNextAnim(%player);
		Player::kill(%player);
		ArenaMSG(0,"TitanBot died.");
		return;	
	}
			%roamchore = floor(getRandom() * 4);
			%aiId = AI::GetID(%AIName);
			if(%roamchore == 0)
			{
				AnniBotsRoam(%aiName);
			}
			if(%roamchore == 1)
			{
				AnniBotsRoam(%aiName);
			}
			if(%roamchore == 2)
			{
				BotFuncs::Animation(%aiId, %animation);
			}
			if(%roamchore == 3)
			{
				BotFuncs::Animation(%aiId, %animation);
			}
				
		schedule("AnniBotsHuntTitan(" @ %ainame @ ");",5);
		return;
	   }
}

function AnniBotsHuntBuilder(%aiName)
{
	%id = AI::GetID(%AIName);
	
    if(!$Actions::Hunting[%id]) 
    {
        return;
    }

	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }	
		
			%curTarget = ai::getTarget( %aiName );
	   if((%curTarget != -1) && ($Actions::Deploys[%id] != "false"))
	   {			
		schedule("AnniBotsHuntBuilder(" @ %ainame @ ");",1);

	    if(!$DoingChore[%id])
	    {
			%vel = Item::getVelocity(%id);
			%velZ = getWord(%vel,2);
			%velx = getWord(%vel,0);
			%vely = getWord(%vel,1);

			if(%velz < -18)
			{
				$Actions::Flying[%id] = true;
				AnniFlyBuilder(%aiName, 25);
				return;
			}
			%chore = floor(getRandom() * 6);
			if(%chore == 1)
			{
				$Actions::Flying[%id] = true;
				AnniFlyBuilder(%aiName, 25);
				AnniBotsRoam(%aiName);
				return;
			}
			if(%chore == 3)
			{
				$Actions::Flying[%id] = true;
        		AnniFlyBuilder(%aiName, 25);
				AnniBotsRoam(%aiName);
				return;
			}
	    }
	   }
	   if((%curTarget == -1) || ($Actions::Deploys[%id] == "false"))
	   {
		%player.cnt++;
	if(%player.cnt >= 25)
	{
		%curDie = radnomItems(4, $PlayerAnim::DieHead,$PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
		Player::setAnimation(%player, %curDie);
		playNextAnim(%player);
		Player::kill(%player);
		ArenaMSG(0,"BuilderBot died.");
		return;	
	}
			%roamchore = floor(getRandom() * 4);
			%aiId = AI::GetID(%AIName);
			if(%roamchore == 0)
			{
				AnniBotsRoam(%aiName);
			}
			if(%roamchore == 1)
			{
				AnniBotsRoam(%aiName);
			}
			if(%roamchore == 2)
			{
				BotFuncs::Animation(%aiId, %animation);
			}
			if(%roamchore == 3)
			{
				BotFuncs::Animation(%aiId, %animation);
			}
				
		schedule("AnniBotsHuntBuilder(" @ %ainame @ ");",5);
		return;
	   }
}

function AnniBotsHuntNecro(%aiName)
{
	%id = AI::GetID(%AIName);
	
    if(!$Actions::Hunting[%id]) 
    {
        return;
    }

	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }	
	
			%curTarget = ai::getTarget( %aiName );
	   if((%curTarget != -1) && ($Actions::Deploys[%id] != "false"))
	   {
		schedule("AnniBotsHuntNecro(" @ %ainame @ ");",1);

	    if(!$DoingChore[%id])
	    {
			%vel = Item::getVelocity(%id);
			%velZ = getWord(%vel,2);
			%velx = getWord(%vel,0);
			%vely = getWord(%vel,1);

			if(%velz < -18)
			{
				$Actions::Flying[%id] = true;
				AnniFlyNecromancer(%aiName, 25);
				return;
			}
			%chore = floor(getRandom() * 6);
			if(%chore == 1)
			{
				$Actions::Flying[%id] = true;
				AnniFlyNecromancer(%aiName, 25);
				AnniBotsRoam(%aiName);
				return;
			}
			if(%chore == 3)
			{
				$Actions::Flying[%id] = true;
        		AnniFlyNecromancer(%aiName, 25);
				AnniBotsRoam(%aiName);
				return;
			}
	    }
	   }
	   if((%curTarget == -1) || ($Actions::Deploys[%id] == "false"))
	   {
		%player.cnt++;
	if(%player.cnt >= 25)
	{
		%curDie = radnomItems(4, $PlayerAnim::DieHead,$PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
		Player::setAnimation(%player, %curDie);
		playNextAnim(%player);
		Player::kill(%player);
		ArenaMSG(0,"NecromancerBot died.");
		return;	
	}
			%roamchore = floor(getRandom() * 4);
			%aiId = AI::GetID(%AIName);
			if(%roamchore == 0)
			{
				AnniBotsRoam(%aiName);
			}
			if(%roamchore == 1)
			{
				AnniBotsRoam(%aiName);
			}
			if(%roamchore == 2)
			{
				BotFuncs::Animation(%aiId, %animation);
			}
			if(%roamchore == 3)
			{
				BotFuncs::Animation(%aiId, %animation);
			}
				
		schedule("AnniBotsHuntNecro(" @ %ainame @ ");",5);
		return;
	   }
}

function AnniBotsHuntWarrior(%aiName)
{
	%id = AI::GetID(%AIName);
	
    if(!$Actions::Hunting[%id]) 
    {
        return;
    }

	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	
		%curTarget = ai::getTarget( %aiName );
	   if((%curTarget != -1) && ($Actions::Deploys[%id] != "false"))
	   {		
		schedule("AnniBotsHuntWarrior(" @ %ainame @ ");",1);
		
	    if(!$DoingChore[%id])
	    {
			%vel = Item::getVelocity(%id);
			%velZ = getWord(%vel,2);
			%velx = getWord(%vel,0);
			%vely = getWord(%vel,1);

			if(%velz < -18)
			{
				$Actions::Flying[%id] = true;
				AnniFlyWarrior(%aiName, 25);
				return;
			}
			%chore = floor(getRandom() * 6);
			if(%chore == 1)
			{
				$Actions::Flying[%id] = true;
				AnniFlyWarrior(%aiName, 25);
				AnniBotsRoam(%aiName);
				return;
			}
			if(%chore == 3)
			{
				$Actions::Flying[%id] = true;
        		AnniFlyWarrior(%aiName, 25);
				AnniBotsRoam(%aiName);
				return;
			}
	    }
	   }
	   if((%curTarget == -1) || ($Actions::Deploys[%id] == "false"))
	   {
		%player.cnt++;
	if(%player.cnt >= 25)
	{
		%curDie = radnomItems(4, $PlayerAnim::DieHead,$PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
		Player::setAnimation(%player, %curDie);
		playNextAnim(%player);
		Player::kill(%player);
		ArenaMSG(0,"WarriorBot died.");
		return;	
	}
			%roamchore = floor(getRandom() * 4);
			%aiId = AI::GetID(%AIName);
			if(%roamchore == 0)
			{
				AnniBotsRoam(%aiName);
			}
			if(%roamchore == 1)
			{
				AnniBotsRoam(%aiName);
			}
			if(%roamchore == 2)
			{
				BotFuncs::Animation(%aiId, %animation);
			}
			if(%roamchore == 3)
			{
				BotFuncs::Animation(%aiId, %animation);
			}
				
		schedule("AnniBotsHuntWarrior(" @ %ainame @ ");",5);
		return;
	   }
}

function AnniBotsHuntAngel(%aiName)
{
	%id = AI::GetID(%AIName);
	
    if(!$Actions::Hunting[%id]) 
    {
        return;
    }

	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }	
	
			%curTarget = ai::getTarget( %aiName );
	   if((%curTarget != -1) && ($Actions::Deploys[%id] != "false"))
	   {
		schedule("AnniBotsHuntAngel(" @ %ainame @ ");",1);

	    if(!$DoingChore[%id])
	    {
			%vel = Item::getVelocity(%id);
			%velZ = getWord(%vel,2);
			%velx = getWord(%vel,0);
			%vely = getWord(%vel,1);

			if(%velz < -18)
			{
				$Actions::Flying[%id] = true;
				AnniFlyAngel(%aiName, 25);
				return;
			}
			%chore = floor(getRandom() * 6);
			if(%chore == 1)
			{
				$Actions::Flying[%id] = true;
				AnniFlyAngel(%aiName, 25);
				AnniBotsRoam(%aiName);
				return;
			}
			if(%chore == 3)
			{
				$Actions::Flying[%id] = true;
        		AnniFlyAngel(%aiName, 25);
				AnniBotsRoam(%aiName);
				return;
			}
	    }
	   }
	   if((%curTarget == -1) || ($Actions::Deploys[%id] == "false"))
	   {
		%player.cnt++;
	if(%player.cnt >= 25)
	{
		%curDie = radnomItems(4, $PlayerAnim::DieHead,$PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
		Player::setAnimation(%player, %curDie);
		playNextAnim(%player);
		Player::kill(%player);
		ArenaMSG(0,"AngelBot died.");
		return;	
	}
			%roamchore = floor(getRandom() * 4);
			%aiId = AI::GetID(%AIName);
			if(%roamchore == 0)
			{
				AnniBotsRoam(%aiName);
			}
			if(%roamchore == 1)
			{
				AnniBotsRoam(%aiName);
			}
			if(%roamchore == 2)
			{
				BotFuncs::Animation(%aiId, %animation);
			}
			if(%roamchore == 3)
			{
				BotFuncs::Animation(%aiId, %animation);
			}
				
		schedule("AnniBotsHuntAngel(" @ %ainame @ ");",5);
		return;
	   }
}

function AnniBotsHuntSpy(%aiName)
{
	%id = AI::GetID(%AIName);
	
    if(!$Actions::Hunting[%id]) 
    {
        return;
    }

	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }	
	
			%curTarget = ai::getTarget( %aiName );
	   if((%curTarget != -1) && ($Actions::Deploys[%id] != "false"))
	   {
		schedule("AnniBotsHuntSpy(" @ %ainame @ ");",1);

	    if(!$DoingChore[%id])
	    {
			%vel = Item::getVelocity(%id);
			%velZ = getWord(%vel,2);
			%velx = getWord(%vel,0);
			%vely = getWord(%vel,1);

			if(%velz < -18)
			{
				$Actions::Flying[%id] = true;
				AnniFlyAssassin(%aiName, 25);
				return;
			}
			%chore = floor(getRandom() * 6);
			if(%chore == 1)
			{
				$Actions::Flying[%id] = true;
				AnniFlyAssassin(%aiName, 25);
				AnniBotsRoam(%aiName);
				return;
			}
			if(%chore == 3)
			{
				$Actions::Flying[%id] = true;
        		AnniFlyAssassin(%aiName, 25);
				AnniBotsRoam(%aiName);
				return;
			}
	    }
	   }
	   if((%curTarget == -1) || ($Actions::Deploys[%id] == "false"))
	   {
		%player.cnt++;
	if(%player.cnt >= 25)
	{
		%curDie = radnomItems(4, $PlayerAnim::DieHead,$PlayerAnim::DieBack,$PlayerAnim::DieForward,$PlayerAnim::DieSpin);
		Player::setAnimation(%player, %curDie);
		playNextAnim(%player);
		Player::kill(%player);
		ArenaMSG(0,"ChameleonBot died.");
		return;	
	}
			%roamchore = floor(getRandom() * 4);
			%aiId = AI::GetID(%AIName);
			if(%roamchore == 0)
			{
				AnniBotsRoam(%aiName);
			}
			if(%roamchore == 1)
			{
				AnniBotsRoam(%aiName);
			}
			if(%roamchore == 2)
			{
				BotFuncs::Animation(%aiId, %animation);
			}
			if(%roamchore == 3)
			{
				BotFuncs::Animation(%aiId, %animation);
			}
				
		schedule("AnniBotsHuntSpy(" @ %ainame @ ");",5);
		return;
	   }
}

function AnniSwitchWeaponTroll(%aiName)
{

	%id = AI::GetID(%AIName);

    if(!$Actions::Weapons[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	
		if(floor(getrandom() * 100) < 20 )
	{
	AnniBotSpeak(%aiName);
	}
	
	AnniThrowGrenade(%aiName);

	if(floor(getrandom() * 100) < 60 )
	{
	ArenaBotBeaconTroll(%AIName);
	}
	
			 AI::SetVar( "*", triggerPct, 1000 );

	    %switch = floor(getRandom() * 8);

		if(%switch == 0)
		{
	           AI::callWithId(%AIName, Player::mountItem, Mortar, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.28 );
		   return;
		}
		else if(%switch == 1)
		{
		    AI::callWithId(%AIname, Player::mountItem, PhaseDisrupter, 0);
//	 	    AI::SetVar(%AIname, triggerPct, 3.0 );
		   return;
		}
		else if(%switch == 2)
		{
	           AI::callWithId(%AIName, Player::mountItem, Minigun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.01 );
		   return;
		}
		else if(%switch == 3)
		{
	           AI::callWithId(%AIName, Player::mountItem, RocketLauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
		   return;
		}
//		else if(%switch == 4)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Disclauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.10 );
//		   return;
//		}
//		else if(%switch == 5)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Flamer, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.01 );
//		   return;
//		}
//		else if(%switch == 6)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, FlameThrower, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.1 );
//		   return;
//		}
		else if(%switch == 4)
		{
	           AI::callWithId(%AIName, Player::mountItem, LaserRifle, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 1.5 );
		   return;
		}
		else if(%switch == 5)
		{
	           AI::callWithId(%AIName, Player::mountItem, GrenadeLauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
		   return;
		}
//		else if(%switch == 9)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, PlasmaGun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.1 );
//		   return;
//		}
		else if(%switch == 6)
		{
	           AI::callWithId(%AIName, Player::mountItem, RubberMortar, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.5 );
		   return;
		}
//		else if(%switch == 11)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Shotgun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.38 );
//		   return;
//		}
		else if(%switch == 7)
		{
	           AI::callWithId(%AIName, Player::mountItem, Stinger, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 1.5 );
		   return;
		}
	else
	{
	return;

	}
}

function AnniSwitchWeaponTank(%aiName)
{
	%id = AI::GetID(%AIName);

    if(!$Actions::Weapons[%id]) 
    {
        return;
    }

	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	
		if(floor(getrandom() * 100) < 20 )
	{
	AnniBotSpeak(%aiName);
	}
	
	AnniThrowGrenade(%aiName);

	if(floor(getrandom() * 100) < 60 )
	{
	ArenaBotBeaconTank(%AIName);
	}
	
		 AI::SetVar( "*", triggerPct, 1000 );
	    %switch = floor(getRandom() * 3);

		if(%switch == 0)
		{
	           AI::callWithId(%AIName, Player::mountItem, TBlastCannon, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.5 );
		   return;
		}
		else if(%switch == 1)
		{
		    AI::callWithId(%AIname, Player::mountItem, TRocketLauncher, 0);
//	 	    AI::SetVar(%AIname, triggerPct, 1.0 );
		   return;
		}
		else if(%switch == 2)
		{
	           AI::callWithId(%AIName, Player::mountItem, TankRPGLauncher, 0);
//	 	    AI::SetVar(%AIname, triggerPct, 2.0 );
		   return;
		}
//		else if(%switch == 3)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, TankShredder, 0);
//	 	    AI::SetVar(%AIname, triggerPct, 0.01 );
//		   return;
//		}
	else
	{
	return;

	}
}

function AnniSwitchWeaponTitan(%aiName)
{
	%id = AI::GetID(%AIName);

    if(!$Actions::Weapons[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	
		if(floor(getrandom() * 100) < 20 )
	{
	AnniBotSpeak(%aiName);
	}
	
	AnniThrowGrenade(%aiName);

	if(floor(getrandom() * 100) < 20 )
	{
	ArenaBotBeaconTitan(%AIName);
	}
			 AI::SetVar( "*", triggerPct, 1000 );
	    %switch = floor(getRandom() * 5);

		if(%switch == 0)
		{
	           AI::callWithId(%AIName, Player::mountItem, ParticleBeamWeapon, 0);
//	 	    AI::SetVar(%AIname, triggerPct, 0.5 );
		   return;
		}
		else if(%switch == 1)
		{
		    AI::callWithId(%AIname, Player::mountItem, PhaseDisrupter, 0);
//	 	    AI::SetVar(%AIname, triggerPct, 3.0 );
		   return;
		}
//		else if(%switch == 2)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, BabyNukeMortar, 0);
//	 	    AI::SetVar(%AIname, triggerPct, 2.0 );
//		   return;
//		}
//		else if(%switch == 3)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, OSLauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.5 );
//		   			messageall(1, "OSLauncher.");
//		   return;
//		}
		else if(%switch == 2)
		{
	           AI::callWithId(%AIName, Player::mountItem, RocketLauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
		   return;
		}
//		else if(%switch == 3)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Disclauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.10 );
//		   return;
//		}
//		else if(%switch == 4)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Flamer, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.01 );
//		   return;
//		}
//		else if(%switch == 5)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, FlameThrower, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.1 );
//		   return;
//		}
		else if(%switch == 3)
		{
	           AI::callWithId(%AIName, Player::mountItem, GrenadeLauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
		   return;
		}
//		else if(%switch == 7)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, PlasmaGun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.1 );
//		   return;
//		}
//		else if(%switch == 8)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, RubberMortar, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.5 );
//		   return;
//		}
		else if(%switch == 4)
		{
	           AI::callWithId(%AIName, Player::mountItem, Stinger, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 1.5 );
		   return;
		}
//		else if(%switch == 10)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Thumper, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
//		   return;
//		}

	else
	{
	return;

	}
}

function AnniSwitchWeaponBuilder(%aiName)
{
	%id = AI::GetID(%AIName);

    if(!$Actions::Weapons[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	
		if(floor(getrandom() * 100) < 20 )
	{
	AnniBotSpeak(%aiName);
	}
	
	AnniThrowGrenade(%aiName);
	
	    %switch = floor(getRandom() * 3);

		if(%switch == 0)
		{
	           AI::callWithId(%AIName, Player::mountItem, Railgun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 1.0 );
		   return;
		}
		else if(%switch == 1)
		{
	           AI::callWithId(%AIName, Player::mountItem, RocketLauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
		   return;
		}
//		else if(%switch == 2)
//		{
//		    AI::callWithId(%AIname, Player::mountItem, Pitchfork, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.01 );
//		   return;
//		}
//		else if(%switch == 2)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Flamer, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.01 );
//		   return;
//		}
		else if(%switch == 2)
		{
	           AI::callWithId(%AIName, Player::mountItem, GrenadeLauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
		   return;
		}
//		else if(%switch == 4)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, PlasmaGun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.1 );
//		   return;
//		}
//		else if(%switch == 5)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Shotgun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.38 );
//		   return;
//		}
//		else if(%switch == 6)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Stinger, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 1.5 );
//		   return;
//		}
//		else if(%switch == 8)
//		{
//		    AI::callWithId(%AIname, Player::mountItem, Vulcan, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.0 );
//		   		   			messageall(1, "Vulcan.");
//		   return;
//		}
//		else if(%switch == 7)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Thumper, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
//		   return;
//		}
//		else if(%switch == 8)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Disclauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.10 );
//		   return;
//		}

	else
	{
	return;

	}
}

function AnniSwitchWeaponNecro(%aiName)
{
	%id = AI::GetID(%AIName);

    if(!$Actions::Weapons[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	
		if(floor(getrandom() * 100) < 20 )
	{
	AnniBotSpeak(%aiName);
	}
	
	AnniThrowGrenade(%aiName);

	if(floor(getrandom() * 100) < 10 )
	{
	ArenaBotBeaconNecromancer(%AIName);
	}
	
			 AI::SetVar( "*", triggerPct, 1000 );
	    %switch = floor(getRandom() * 6);

		if(%switch == 0)
		{
	           AI::callWithId(%AIName, Player::mountItem, DisarmerSpell, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.5 );
		   return;
		}
		else if(%switch == 1)
		{
		    AI::callWithId(%AIname, Player::mountItem, ShockingGrasp, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.0 );
		   return;
		}
		else if(%switch == 2)
		{
	           AI::callWithId(%AIName, Player::mountItem, SpellFlameThrower, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.1 );
		   return;
		}
		else if(%switch == 3)
		{
	           AI::callWithId(%AIName, Player::mountItem, DeathRay, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 1.0 );
		   return;
		}
		else if(%switch == 4)
		{
	           AI::callWithId(%AIName, Player::mountItem, FlameStrike, 0);
	 	  //  AI::SetVar(%AIname, triggerPct, 0.1 );
		   return;
		}
		else if(%switch == 5)
		{
	           AI::callWithId(%AIName, Player::mountItem, Stasis, 0);
	 	  //  AI::SetVar(%AIname, triggerPct, 1.0 );
		   return;
		}
	else
	{
	return;

	}
}

function AnniSwitchWeaponSpy(%aiName)
{
	%id = AI::GetID(%aiName);

    if(!$Actions::Weapons[%id]) 
    {
        return;
    }

	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	
		if(floor(getrandom() * 100) < 20 )
	{
	AnniBotSpeak(%aiName);
	}
	
	AnniThrowGrenade(%aiName);

	if(floor(getrandom() * 100) < 10 )
	{
	ArenaBotBeaconChameleon(%aiName);
	}
			 AI::SetVar( "*", triggerPct, 1000 );
	    %switch = floor(getRandom() * 5);

		if(%switch == 0)
		{
	           AI::callWithId(%AIName, Player::mountItem, SniperRifle, 0);
	 	  //  AI::SetVar(%AIname, triggerPct, 1.0 );
		   return;
		}
//		else if(%switch == 1)
//		{
//		    AI::callWithId(%AIname, Player::mountItem, PlasmaGun, 0);
//	 	  //  AI::SetVar(%AIname, triggerPct, 0.1 );
//		   return;
//		}
		else if(%switch == 1)
		{
	           AI::callWithId(%AIName, Player::mountItem, Shotgun, 0);
	 	  //  AI::SetVar(%AIname, triggerPct, 0.38 );
		   return;
		}
//		else if(%switch == 3)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Flamer, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.01 );
//		   return;
//		}
		else if(%switch == 2)
		{
	           AI::callWithId(%AIName, Player::mountItem, GrenadeLauncher, 0);
	 	  //  AI::SetVar(%AIname, triggerPct, 0.8 );
		   return;
		}
		else if(%switch == 3)
		{
	           AI::callWithId(%AIName, Player::mountItem, Hammer, 0);
	 	  //  AI::SetVar(%AIname, triggerPct, 0.1 );
		   return;
		}
//		else if(%switch == 6)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, ShockwaveGun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.3 );
//		   		   			messageall(1, "Shockwaveun.");
//		   return;
//		}
		else if(%switch == 4)
		{
	           AI::callWithId(%AIName, Player::mountItem, LaserRifle, 0);
	 	  //  AI::SetVar(%AIname, triggerPct, 1.0 );
		   return;
		}
//		else if(%switch == 7)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Thumper, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
//		   return;
//		}
//		else if(%switch == 8)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Disclauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.10 );
//		   return;
//		}
	else
	{
	return;
	}
}

function AnniSwitchWeaponAngel(%aiName)
{
	%id = AI::GetID(%AIName);

    if(!$Actions::Weapons[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	
		if(floor(getrandom() * 100) < 20 )
	{
	AnniBotSpeak(%aiName);
	}
	
	AnniThrowGrenade(%aiName);

	if(floor(getrandom() * 100) < 99 )
	{
	ArenaBotBeaconAngel(%AIName);
	}

		 AI::SetVar( "*", triggerPct, 1000 );
	    %switch = floor(getRandom() * 2); // 5

		if(%switch == 0)
		{
	           AI::callWithId(%AIName, Player::mountItem, HeavensFury, 0);
	 	   AI::SetVar(%AIname, triggerPct, 0.02 );
		   return;
		}
//		else if(%switch == 1)
//		{
//		    AI::callWithId(%AIname, Player::mountItem, SoulSucker, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.3 );
//		   return;
//		}
//		else if(%switch == 2)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, GrapplingHook, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 3.0 );
//		   return;
//		}
		else if(%switch == 1)
		{
		    AI::callWithId(%AIname, Player::mountItem, AngelFire, 0);
	 	  //  AI::SetVar(%AIname, triggerPct, 0.3 );
		   return;
		}
//		else if(%switch == 4)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, AngelRepairGun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.01 );
//		   return;
//		}

	else
	{
	return;

	}
}

function AnniSwitchWeaponWarrior(%aiName)
{
	%id = AI::GetID(%AIName);

    if(!$Actions::Weapons[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

//	messageall(1, "Anni Switch Weapon Warrior.");

    if(%loc == "0 0 0")
    {
	return;
    }
	
	if(floor(getrandom() * 100) < 20 )
	{
	AnniBotSpeak(%aiName);
	}
	
	AnniThrowGrenade(%aiName);
	
	// messageall(1, "Anni Switch Weapon Warrior.");
	
	Player::trigger(%aiName,$BackpackSlot);

	if(floor(getrandom() * 100) < 20 )
	{
	ArenaBotBeaconWarrior(%AIName);
	}
	
			 AI::SetVar( "*", triggerPct, 1000 );

//	    	%switch = floor(getRandom() * 14);

//		if(%switch == 0)
//		{
	       //    AI::callWithId(%AIName, Player::mountItem, Disclauncher, 0);
	 	  //  AI::SetVar(%AIname, triggerPct, 0.10 );
		   	// echo("Warrior Weapon Disc.");
		   return;
//		}
//		else if(%switch == 1)
//		{
//		    AI::callWithId(%AIname, Player::mountItem, Vulcan, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.01 );
//		   	echo("Warrior Weapon Vulcan.");
//		   return;
//		}
//		else if(%switch == 2)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Stinger, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 1.5 );
//		   	echo("Warrior Weapon Stinger.");
//		   return;
//		}
//		else if(%switch == 3)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, FlameThrower, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.1 );
//		   	echo("Warrior Weapon flamethrower.");
//		   return;
//		}
//		else if(%switch == 4)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, RocketLauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
//		   	echo("Warrior Weapon rocket.");
//		   return;
//		}
//		else if(%switch == 5)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Blaster, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.10 );
//		   	echo("Warrior Weapon blaster.");
//		   return;
//		}
//		else if(%switch == 6)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Flamer, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.01 );
//		   	echo("Warrior Weapon flamer.");
//		   return;
//		}
//		else if(%switch == 7)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, GrenadeLauncher, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
//		   	echo("Warrior Weapon grenadelauncher.");
//		   return;
//		}
//		else if(%switch == 8)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, PlasmaGun, 0); // Hammer
//	 	   AI::SetVar(%AIname, triggerPct, 0.1 );
//		   	echo("Warrior Weapon hammer.");
//		   return;
//		}
//		else if(%switch == 9)
//		{
//		    AI::callWithId(%AIname, Player::mountItem, PlasmaGun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.1 );
//		   	echo("Warrior Weapon plasma.");
//		   return;
//		}
//		else if(%switch == 10)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, RubberMortar, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.5 );
//		   	echo("Warrior Weapon rubber mortar.");
//		   return;
//		}
//		else if(%switch == 11)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, ShockwaveGun, 0);
//			   	echo("Warrior Weapon Shockwavegun.");
//		   return;
//		}
//		else if(%switch == 12)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Shotgun, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.38 );
//		   	echo("Warrior Weapon Shotgun.");
//		   return;
//		}
//		else if(%switch == 13)
//		{
//	           AI::callWithId(%AIName, Player::mountItem, Thumper, 0);
//	 	   AI::SetVar(%AIname, triggerPct, 0.8 );
//		   	echo("Warrior Weapon thumper.");
//		   return;
//		}
}

// our team bots
function CreateSpearSoloWarriorFriendly(%clientID)
{	
    if(GameBase::getPosition(%clientID) == "0 0 0")
    {
       client::sendmessage(%clientid, 1, "You must be spawned in to do that!");
       bottomprint(%clientID, "<f1><jc>You must be spawned in to do that!");
       Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       return;
    }
	if(!%clientID.isGoated)
	{
			if($BotCooldown == "true")
	{
		Client::sendMessage(%clientID,0,"Please wait a few seconds to spawn another bot.");
		return;
	}
    if(!%clientId.inArena)
	{
      		client::sendmessage(%clientid, 1, "You must be in Arena to do that!");
      	 	bottomprint(%clientID, "<f1><jc>You must be in Arena to do that!");
      		Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       		return;
	}

	if($BotsArenaCount >= $BotsArenaMax)
   { 
     Client::sendMessage(%clientID,0,"Arena bots at maximum.");
    Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
   return;
    }
	}
    
	$BotCooldown = true;
	schedule("$BotCooldown=false;",2.0);

	 %Botname = "WarriorBot" @ $NumAI;
//	 %Botname = AnniBotName(%ClientID);
//   	 %spawnMarker = GameBase::getPosition(%clientID);
		if(!%clientID.isGoated)
	{
	%spawnMarker = Arena::pickRandomSpawn(%clientId);
	}
	if(%clientID.isGoated)
	{
     %spawnMarker = GameBase::getPosition(%clientID);
	}
	
  	 %xPos = getWord(%spawnMarker, 0) + (floor(getRandom() * 10)-5);
  	 %yPos = getword(%spawnMarker, 1) + (floor(getRandom() * 10)-5);
  	 %zPos = getWord(%spawnMarker, 2) + 5;
  	 %rPos = GameBase::getRotation(%clientID);

	 echo("ADMINMSG: The bot " @ %botname @ " : " @ %aiName @ " Joined the game.");
	 %spawnPos = GameBase::getPosition(%spawnMarker);
	 %spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);

  	 %team = GameBase::getTeam(%clientID);
	 $NumAI++;
//	 %AIName = "WarriorBot";
	 %AIName = "WarriorBot" @ $NumAI;
	 %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;	 
	 	 
	if(%clientID.isGoated)
	{
		AI::spawn(%AIName,"armorfWarrior",%aiSpawnPos, "0 0 0", %AIName, "female2");
	}
	
	if(!%clientID.isGoated)
	{
		AI::spawn(%AIName,"armorfWarrior",%spawnPos, "0 0 0", %AIName, "female2");
	}
	
	
	 %id = AI::getId( %AIname );
	// AddToSet("MissionCleanup",%id);
	 	 %player = Client::getOwnedObject(%id);
	 %player.cnt = 0;
 	 GameBase::setTeam(%id, %team);

	 Client::setSkin(%id, $Server::teamSkin[Client::getTeam(%id)]);


//	schedule("AnniBotsDie(" @ %AIname @ ");", 90);

	%aiId = Client::getOwnedObject(AI::getID(%aiName));
	 %aiId.isDuelDuck = 1;

     	Client::sendMessage(%clientID,0,"You spawned a Team Warrior bot.");
	Client::sendMessage(%clientID,0,"~wturretOff4.wav");
	bottomprint(%clientID, "<f1><jc>You spawned a Team Warrior bot.");
	$BotsArenaCount++;

	 AI::setVar( %AIname,  iq,  120 );
 	 AI::setVar( %AIname,  attackMode, 0);
 	 AI::DirectiveTarget(%AIname, %Victim);
    	 AI::callWithId(%AIName, Player::setItemCount, AmmoPack, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
       	 AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
       	 AI::callWithId(%AIName, Player::setItemCount, Disclauncher, 1);
      	 AI::callWithId(%AIName, Player::setItemCount, DiscAmmo, 100);
      	 AI::callWithId(%AIName, Player::setItemCount, Vulcan, 1);
      	 AI::callWithId(%AIName, Player::setItemCount, VulcanAmmo, 200);
      	 AI::callWithId(%AIName, Player::setItemCount, Stinger, 1);
       	 AI::callWithId(%AIName, Player::setItemCount, StingerAmmo, 50);
       	 AI::callWithId(%AIName, Player::setItemCount, FlameThrower, 1);
        	 AI::callWithId(%AIName, Player::setItemCount, FlameThrowerAmmo, 200);
        	 AI::callWithId(%AIName, Player::setItemCount, RocketLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RocketAmmo, 50);
    	 AI::callWithId(%AIName, Player::setItemCount, Blaster, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, Flamer, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, FlamerAmmo, 200);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeAmmo, 50);
    	 AI::callWithId(%AIName, Player::setItemCount, Hammer, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, HammerAmmo, 50);
    	 AI::callWithId(%AIname, Player::setItemCount, PlasmaGun, 1);
	 AI::callWithId(%AIname, Player::setItemCount, PlasmaAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, RubberMortar, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RubberAmmo, 50);
    	 AI::callWithId(%AIName, Player::setItemCount, ShockwaveGun, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, Shotgun, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, ShotgunShells, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Thumper, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, ThumperAmmo, 50);
	 AI::callWithId(%AIName, Player::mountItem, Disclauncher, 0);
	 AI::callWithId(%AIName, Player::mountItem, AmmoPack, $BackpackSlot);
	 AI::SetVar( "*", triggerPct, 1000 );
	 AI::setVar( "*", SpotDist, $visDistance);
	 AI::SetAutomaticTargets(%AIName);
	 $DoingChore[%id] = false;
	 $Actions::Hunting[%id] = true;
	 $Actions::Speaking[%id] = true;
	 $Actions::Grenades[%id] = true;
	 $Actions::Beacons[%id] = true;
	 $Actions::Weapons[%id] = true;
	 $Actions::Deploys[%id] = true;
	 AnniBotsHuntWarrior(%aiName);
	 AI::callbackPeriodic(%aiName, 5, AnniSwitchWeaponWarrior);
}

function CreateSpearSoloAngelFriendly(%clientID)
{
    if(GameBase::getPosition(%clientID) == "0 0 0")
    {
       client::sendmessage(%clientid, 1, "You must be spawned in to do that!");
       bottomprint(%clientID, "<f1><jc>You must be spawned in to do that!");
       Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       return;
    }

	if(!%clientID.isGoated)
	{
			if($BotCooldown == "true")
	{
		Client::sendMessage(%clientID,0,"Please wait a few seconds to spawn another bot.");
		return;
	}
    if(!%clientId.inArena)
	{
      		client::sendmessage(%clientid, 1, "You must be in Arena to do that!");
      	 	bottomprint(%clientID, "<f1><jc>You must be in Arena to do that!");
      		Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       		return;
	}

if($BotsArenaCount >= $BotsArenaMax)
   { 
     Client::sendMessage(%clientID,0,"Arena bots at maximum.");
    Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
   return;
    }
	}
	
	$BotCooldown = true;
	schedule("$BotCooldown=false;",2.0);
    
	 %Botname = "AngelBot" @ $NumAI;
//	 %Botname = AnniBotName(%ClientID);
//   	 %spawnMarker = GameBase::getPosition(%clientID);
		if(!%clientID.isGoated)
	{
	%spawnMarker = Arena::pickRandomSpawn(%clientId);
	}
			if(%clientID.isGoated)
	{
     %spawnMarker = GameBase::getPosition(%clientID);
	}
  	 %xPos = getWord(%spawnMarker, 0) + (floor(getRandom() * 10)-5);
  	 %yPos = getword(%spawnMarker, 1) + (floor(getRandom() * 10)-5);
  	 %zPos = getWord(%spawnMarker, 2) + 5;
  	 %rPos = GameBase::getRotation(%clientID);

	 echo("ADMINMSG: The bot " @ %botname @ " : " @ %aiName @ " Joined the game.");
	 
	%spawnPos = GameBase::getPosition(%spawnMarker);
	%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);

  	 %team = GameBase::getTeam(%clientID);
	 $NumAI++;
	 %AIName = "AngelBot" @ $NumAI;
  	 %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;	 
	 	if(%clientID.isGoated)
	{
	 		AI::spawn(%AIName,"armorfAngel",%aiSpawnPos, "0 0 0", %AIName, "female2"); 
	}
	
	if(!%clientID.isGoated)
	{
			AI::spawn(%AIName,"armorfAngel",%spawnPos, "0 0 0", %AIName, "female2"); 
	}
	

	 %id = AI::getId( %AIname );
	 	 %player = Client::getOwnedObject(%id);
	 %player.cnt = 0;
 	 GameBase::setTeam(%id, %team);
	 Client::setSkin(%id, $Server::teamSkin[Client::getTeam(%id)]);
//	schedule("AnniBotsDie(" @ %AIname @ ");", 90);
	%aiId = Client::getOwnedObject(AI::getID(%aiName));
	 %aiId.isDuelDuck = 1;

     	Client::sendMessage(%clientID,0,"You spawned a Team Angel bot.");
	Client::sendMessage(%clientID,0,"~wturretOff4.wav");
	bottomprint(%clientID, "<f1><jc>You spawned a Team Angel bot.");
	$BotsArenaCount++;

	 AI::setVar( %AIname,  iq,  120 );
 	 AI::setVar( %AIname,  attackMode, 0);
 	 AI::DirectiveTarget(%AIname, %Victim);
    	 AI::callWithId(%AIName, Player::setItemCount, EnergyPack, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
         	 AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
         	 AI::callWithId(%AIName, Player::setItemCount, HeavensFury, 1);
         	 AI::callWithId(%AIName, Player::setItemCount, SoulSucker, 1);
        	 AI::callWithId(%AIName, Player::setItemCount, GrapplingHook, 1);
       	 AI::callWithId(%AIname, Player::setItemCount, AngelFire, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, AngelRepairGun, 1);
	 AI::callWithId(%AIName, Player::mountItem, AngelFire, 0);
	 AI::callWithId(%AIName, Player::mountItem, EnergyPack, $BackpackSlot);
	 AI::SetVar( "*", triggerPct, 1000 );
	 AI::setVar( "*", SpotDist, $visDistance);
	 AI::SetAutomaticTargets(%AIName);
	 $DoingChore[%id] = false;
	 $Actions::Hunting[%id] = true;
	 $Actions::Speaking[%id] = true;
	 $Actions::Grenades[%id] = true;
	 $Actions::Beacons[%id] = true;
	 $Actions::Weapons[%id] = true;
	   $Actions::Deploys[%id] = true;
	 AI::callbackPeriodic(%aiName, 5, AnniSwitchWeaponAngel);
	 AnniBotsHuntAngel(%aiName);
}

function CreateSpearSoloSpyFriendly(%clientID)
{
    if(GameBase::getPosition(%clientID) == "0 0 0")
    {
       client::sendmessage(%clientid, 1, "You must be spawned in to do that!");
       bottomprint(%clientID, "<f1><jc>You must be spawned in to do that!");
       Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       return;
    }

	if(!%clientID.isGoated)
	{
			if($BotCooldown == "true")
	{
		Client::sendMessage(%clientID,0,"Please wait a few seconds to spawn another bot.");
		return;
	}
    if(!%clientId.inArena)
	{
      		client::sendmessage(%clientid, 1, "You must be in Arena to do that!");
      	 	bottomprint(%clientID, "<f1><jc>You must be in Arena to do that!");
      		Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       		return;
	}
if($BotsArenaCount >= $BotsArenaMax)
   { 
     Client::sendMessage(%clientID,0,"Arena bots at maximum.");
    Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
   return;
    }
	}
	
	$BotCooldown = true;
	schedule("$BotCooldown=false;",2.0);
    
	 %Botname = "ChameleonBot" @ $NumAI;
//	 %Botname = AnniBotName(%ClientID);
//   	 %spawnMarker = GameBase::getPosition(%clientID);
		if(!%clientID.isGoated)
	{
	%spawnMarker = Arena::pickRandomSpawn(%clientId);
	}
			if(%clientID.isGoated)
	{
     %spawnMarker = GameBase::getPosition(%clientID);
	}
  	 %xPos = getWord(%spawnMarker, 0) + (floor(getRandom() * 10)-5);
  	 %yPos = getword(%spawnMarker, 1) + (floor(getRandom() * 10)-5);
  	 %zPos = getWord(%spawnMarker, 2) + 5;
  	 %rPos = GameBase::getRotation(%clientID);

	 echo("ADMINMSG: The bot " @ %botname @ " : " @ %aiName @ " Joined the game.");
	 
	 	%spawnPos = GameBase::getPosition(%spawnMarker);
	%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);

  	 %team = GameBase::getTeam(%clientID);
	 $NumAI++;
	 %AIName = "ChameleonBot" @ $NumAI;
  	 %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;	 
		 	if(%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorfSpy",%aiSpawnPos, "0 0 0", %AIName, "female2"); 
	}
	
	if(!%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorfSpy",%spawnPos, "0 0 0", %AIName, "female2"); 
	} 
	

	 
	 %id = AI::getId( %AIname );
	 	 %player = Client::getOwnedObject(%id);
	 %player.cnt = 0;
 	 GameBase::setTeam(%id, %team);
	 Client::setSkin(%id, $Server::teamSkin[Client::getTeam(%id)]);
//	schedule("AnniBotsDie(" @ %AIname @ ");", 90);
	%aiId = Client::getOwnedObject(AI::getID(%aiName));
	 %aiId.isDuelDuck = 1;

     	Client::sendMessage(%clientID,0,"You spawned a Team Chameleon bot.");
	Client::sendMessage(%clientID,0,"~wturretOff4.wav");
	bottomprint(%clientID, "<f1><jc>You spawned a Team Chameleon bot.");
	$BotsArenaCount++;

	 AI::setVar( %AIname,  iq,  120 );
 	 AI::setVar( %AIname,  attackMode, 0);
 	 AI::DirectiveTarget(%AIname, %Victim);
    	 AI::callWithId(%AIName, Player::setItemCount, EnergyPack, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
         	 AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
        	 AI::callWithId(%AIName, Player::setItemCount, PlasmaGun, 1);
       	 AI::callWithId(%AIName, Player::setItemCount, PlasmaAmmo, 100);
         	 AI::callWithId(%AIName, Player::setItemCount, SniperRifle, 1);
       	 AI::callWithId(%AIName, Player::setItemCount, SniperAmmo, 100);
       	 AI::callWithId(%AIName, Player::setItemCount, Disclauncher, 1);
       	 AI::callWithId(%AIName, Player::setItemCount, DiscAmmo, 100);
        	 AI::callWithId(%AIName, Player::setItemCount, Shotgun, 1);
       	 AI::callWithId(%AIName, Player::setItemCount, ShotgunShells, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Flamer, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, FlamerAmmo, 200);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Hammer, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, HammerAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, ShockwaveGun, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, LaserRifle, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, Thumper, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, ThumperAmmo, 100);
	 AI::callWithId(%AIName, Player::mountItem, Shotgun, 0);
	 AI::callWithId(%AIName, Player::mountItem, EnergyPack, $BackpackSlot);
	 AI::SetVar( "*", triggerPct, 1000 );
	 AI::setVar( "*", SpotDist, $visDistance);
	 AI::SetAutomaticTargets(%AIName);
	 $DoingChore[%id] = false;
	 $Actions::Hunting[%id] = true;
	 $Actions::Speaking[%id] = true;
	 $Actions::Grenades[%id] = true;
	 $Actions::Beacons[%id] = true;
	 $Actions::Weapons[%id] = true;
	   $Actions::Deploys[%id] = true;
	 	 AI::callbackPeriodic(%aiName, 5, AnniSwitchWeaponSpy);
	 AnniBotsHuntSpy(%aiName);
}

function CreateSpearSoloNecroFriendly(%clientID)
{
    if(GameBase::getPosition(%clientID) == "0 0 0")
    {
       client::sendmessage(%clientid, 1, "You must be spawned in to do that!");
       bottomprint(%clientID, "<f1><jc>You must be spawned in to do that!");
       Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       return;
    }

	if(!%clientID.isGoated)
	{
			if($BotCooldown == "true")
	{
		Client::sendMessage(%clientID,0,"Please wait a few seconds to spawn another bot.");
		return;
	}
    if(!%clientId.inArena)
	{
      		client::sendmessage(%clientid, 1, "You must be in Arena to do that!");
      	 	bottomprint(%clientID, "<f1><jc>You must be in Arena to do that!");
      		Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       		return;
	}

if($BotsArenaCount >= $BotsArenaMax)
   { 
     Client::sendMessage(%clientID,0,"Arena bots at maximum.");
    Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
   return;
    }
	}
	
		$BotCooldown = true;
	schedule("$BotCooldown=false;",2.0);
    
	 %Botname = "NecromancerBot" @ $NumAI;
//	 %Botname = AnniBotName(%ClientID);
//   	 %spawnMarker = GameBase::getPosition(%clientID);
		if(!%clientID.isGoated)
	{
	%spawnMarker = Arena::pickRandomSpawn(%clientId);
	}
			if(%clientID.isGoated)
	{
     %spawnMarker = GameBase::getPosition(%clientID);
	}
  	 %xPos = getWord(%spawnMarker, 0) + (floor(getRandom() * 10)-5);
  	 %yPos = getword(%spawnMarker, 1) + (floor(getRandom() * 10)-5);
  	 %zPos = getWord(%spawnMarker, 2) + 5;
  	 %rPos = GameBase::getRotation(%clientID);

	 echo("ADMINMSG: The bot " @ %botname @ " : " @ %aiName @ " Joined the game.");
	 
	 	%spawnPos = GameBase::getPosition(%spawnMarker);
	%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);

  	 %team = GameBase::getTeam(%clientID);
	 $NumAI++;
	 %AIName = "NecromancerBot" @ $NumAI;
  	 %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;	 
	 		 	if(%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorfNecro",%aiSpawnPos, "0 0 0", %AIName, "female2");
	}
	
	if(!%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorfNecro",%spawnPos, "0 0 0", %AIName, "female2");
	} 
	

	 
	 %id = AI::getId( %AIname );
	 	 %player = Client::getOwnedObject(%id);
	 %player.cnt = 0;
 	 GameBase::setTeam(%id, %team);
	 Client::setSkin(%id, $Server::teamSkin[Client::getTeam(%id)]);
//	schedule("AnniBotsDie(" @ %AIname @ ");", 90);
	%aiId = Client::getOwnedObject(AI::getID(%aiName));
	 %aiId.isDuelDuck = 1;

     	Client::sendMessage(%clientID,0,"You spawned a Team Necromancer bot.");
	Client::sendMessage(%clientID,0,"~wturretOff4.wav");
	bottomprint(%clientID, "<f1><jc>You spawned a Team Necromancer bot.");
	$BotsArenaCount++;

	 AI::setVar( %AIname,  iq,  120 );
 	 AI::setVar( %AIname,  attackMode, 0);
 	 AI::DirectiveTarget(%AIname, %Victim);
    	 AI::callWithId(%AIName, Player::setItemCount, PhaseShifterPack, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
        	 AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
        	 AI::callWithId(%AIName, Player::setItemCount, DisarmerSpell, 1);
        	 AI::callWithId(%AIName, Player::setItemCount, ShockingGrasp, 1);
         	AI::callWithId(%AIName, Player::setItemCount, SpellFlameThrower, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, DeathRay, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, FlameStrike, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, Stasis, 1);
	 AI::callWithId(%AIName, Player::mountItem, SpellFlameThrower, 0);
	 AI::callWithId(%AIName, Player::mountItem, PhaseShifterPack, $BackpackSlot);
	 AI::SetVar( "*", triggerPct, 1000 );
	 AI::setVar( "*", SpotDist, $visDistance);
	 AI::SetAutomaticTargets(%AIName);
	 $DoingChore[%id] = false;
	 $Actions::Hunting[%id] = true;
	 $Actions::Speaking[%id] = true;
	 $Actions::Grenades[%id] = true;
	 $Actions::Beacons[%id] = true;
	 $Actions::Weapons[%id] = true;
	   $Actions::Deploys[%id] = true;
	 AnniBotsHuntNecro(%aiName);
	 	 AI::callbackPeriodic(%aiName, 5, AnniSwitchWeaponNecro);
}

function CreateSpearSoloBuilderFriendly(%clientID)
{
    if(GameBase::getPosition(%clientID) == "0 0 0")
    {
       client::sendmessage(%clientid, 1, "You must be spawned in to do that!");
       bottomprint(%clientID, "<f1><jc>You must be spawned in to do that!");
       Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       return;
    }

	if(!%clientID.isGoated)
	{
			if($BotCooldown == "true")
	{
		Client::sendMessage(%clientID,0,"Please wait a few seconds to spawn another bot.");
		return;
	}
    if(!%clientId.inArena)
	{
      		client::sendmessage(%clientid, 1, "You must be in Arena to do that!");
      	 	bottomprint(%clientID, "<f1><jc>You must be in Arena to do that!");
      		Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       		return;
	}

if($BotsArenaCount >= $BotsArenaMax)
   { 
     Client::sendMessage(%clientID,0,"Arena bots at maximum.");
    Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
   return;
    }
	}
	
		$BotCooldown = true;
	schedule("$BotCooldown=false;",2.0);
    
	 %Botname = "BuilderBot" @ $NumAI;
//	 %Botname = AnniBotName(%ClientID);
//   	 %spawnMarker = GameBase::getPosition(%clientID);
		if(!%clientID.isGoated)
	{
	%spawnMarker = Arena::pickRandomSpawn(%clientId);
	}
			if(%clientID.isGoated)
	{
     %spawnMarker = GameBase::getPosition(%clientID);
	}
  	 %xPos = getWord(%spawnMarker, 0) + (floor(getRandom() * 10)-5);
  	 %yPos = getword(%spawnMarker, 1) + (floor(getRandom() * 10)-5);
  	 %zPos = getWord(%spawnMarker, 2) + 5;
  	 %rPos = GameBase::getRotation(%clientID);

	 echo("ADMINMSG: The bot " @ %botname @ " : " @ %aiName @ " Joined the game.");
	 
	 	%spawnPos = GameBase::getPosition(%spawnMarker);
	%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);

  	 %team = GameBase::getTeam(%clientID);
	 $NumAI++;
	 %AIName = "BuilderBot" @ $NumAI;
  	 %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;	 
	 	 		 	if(%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorfBuilder",%aiSpawnPos, "0 0 0", %AIName, "female2");
	}
	
	if(!%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorfBuilder",%spawnPos, "0 0 0", %AIName, "female2");
	} 
	

	 
	 %id = AI::getId( %AIname );
	 	 %player = Client::getOwnedObject(%id);
	 %player.cnt = 0;
 	 GameBase::setTeam(%id, %team);
	 Client::setSkin(%id, $Server::teamSkin[Client::getTeam(%id)]);
//	schedule("AnniBotsDie(" @ %AIname @ ");", 90);
	%aiId = Client::getOwnedObject(AI::getID(%aiName));
	 %aiId.isDuelDuck = 1;

     	Client::sendMessage(%clientID,0,"You spawned a Team Builder bot.");
	Client::sendMessage(%clientID,0,"~wturretOff4.wav");
	bottomprint(%clientID, "<f1><jc>You spawned a Team Builder bot.");
	$BotsArenaCount++;

	 AI::setVar( %AIname,  iq,  120 );
 	 AI::setVar( %AIname,  attackMode, 0);
 	 AI::DirectiveTarget(%AIname, %Victim);
    	 AI::callWithId(%AIName, Player::setItemCount, DeployableInvPack, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
         	 AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
        	 AI::callWithId(%AIName, Player::setItemCount, Railgun, 1);
       	 AI::callWithId(%AIName, Player::setItemCount, RailAmmo, 100);
       	 AI::callWithId(%AIName, Player::setItemCount, Disclauncher, 1);
        	 AI::callWithId(%AIName, Player::setItemCount, DiscAmmo, 100);
       	 AI::callWithId(%AIName, Player::setItemCount, RocketLauncher, 1);
       	 AI::callWithId(%AIName, Player::setItemCount, RocketAmmo, 100);
         	 AI::callWithId(%AIName, Player::setItemCount, Pitchfork, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, Flamer, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, FlamerAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, PlasmaGun, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, PlasmaAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Shotgun, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, ShotgunShells, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Stinger, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, StingerAmmo, 100);
    	 AI::callWithId(%AIname, Player::setItemCount, Vulcan, 1);
	 AI::callWithId(%AIname, Player::setItemCount, VulcanAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Thumper, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, ThumperAmmo, 100);
	 AI::callWithId(%AIName, Player::mountItem, GrenadeLauncher, 0);
	 AI::callWithId(%AIName, Player::mountItem, DeployableInvPack, $BackpackSlot);
	 AI::SetVar( "*", triggerPct, 1000 );
	 AI::setVar( "*", SpotDist, $visDistance);
	 AI::SetAutomaticTargets(%AIName);
	 $DoingChore[%id] = false;
	$Actions::Hunting[%id] = true;
	 $Actions::Speaking[%id] = true;
	 $Actions::Grenades[%id] = true;
	 $Actions::Beacons[%id] = true;
	 $Actions::Weapons[%id] = true;
	   $Actions::Deploys[%id] = true;
	 AnniBotsHuntBuilder(%aiName);
	 	 AI::callbackPeriodic(%aiName, 5, AnniSwitchWeaponBuilder);
}

function CreateSpearSoloTrollFriendly(%clientID)
{
    if(GameBase::getPosition(%clientID) == "0 0 0")
    {
       client::sendmessage(%clientid, 1, "You must be spawned in to do that!");
       bottomprint(%clientID, "<f1><jc>You must be spawned in to do that!");
       Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       return;
    }

	if(!%clientID.isGoated)
	{
			if($BotCooldown == "true")
	{
		Client::sendMessage(%clientID,0,"Please wait a few seconds to spawn another bot.");
		return;
	}
    if(!%clientId.inArena)
	{
      		client::sendmessage(%clientid, 1, "You must be in Arena to do that!");
      	 	bottomprint(%clientID, "<f1><jc>You must be in Arena to do that!");
      		Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       		return;
	}

if($BotsArenaCount >= $BotsArenaMax)
   { 
     Client::sendMessage(%clientID,0,"Arena bots at maximum.");
    Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
   return;
    }
	}
	
		$BotCooldown = true;
	schedule("$BotCooldown=false;",2.0);
    
	 %Botname = "TrollBot" @ $NumAI;
//	 %Botname = AnniBotName(%ClientID);
//   	 %spawnMarker = GameBase::getPosition(%clientID);
		if(!%clientID.isGoated)
	{
	%spawnMarker = Arena::pickRandomSpawn(%clientId);
	}
			if(%clientID.isGoated)
	{
     %spawnMarker = GameBase::getPosition(%clientID);
	}
  	 %xPos = getWord(%spawnMarker, 0) + (floor(getRandom() * 10)-5);
  	 %yPos = getword(%spawnMarker, 1) + (floor(getRandom() * 10)-5);
  	 %zPos = getWord(%spawnMarker, 2) + 5;
  	 %rPos = GameBase::getRotation(%clientID);

	 echo("ADMINMSG: The bot " @ %botname @ " : " @ %aiName @ " Joined the game.");
	 
	 	%spawnPos = GameBase::getPosition(%spawnMarker);
	%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);

  	 %team = GameBase::getTeam(%clientID);
	 $NumAI++;
	 %AIName = "TrollBot" @ $NumAI;
  	 %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;	 
	 	 	 		 	if(%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorTroll",%aiSpawnPos, "0 0 0", %AIName, "female2");
	}
	
	if(!%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorTroll",%spawnPos, "0 0 0", %AIName, "female2");
	} 
	

	 
	 %id = AI::getId( %AIname );
	 	 %player = Client::getOwnedObject(%id);
	 %player.cnt = 0;
 	 GameBase::setTeam(%id, %team);
	 Client::setSkin(%id, $Server::teamSkin[Client::getTeam(%id)]);
//	schedule("AnniBotsDie(" @ %AIname @ ");", 90);
	%aiId = Client::getOwnedObject(AI::getID(%aiName));
	 %aiId.isDuelDuck = 1;

     	Client::sendMessage(%clientID,0,"You spawned a Team Troll bot.");
	Client::sendMessage(%clientID,0,"~wturretOff4.wav");
	bottomprint(%clientID, "<f1><jc>You spawned a Team Troll bot.");
	$BotsArenaCount++;

	 AI::setVar( %AIname,  iq,  120 );
 	 AI::setVar( %AIname,  attackMode, 0);
 	 AI::DirectiveTarget(%AIname, %Victim);
    	 AI::callWithId(%AIName, Player::setItemCount, EnergyPack, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
       	 AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
       	 AI::callWithId(%AIName, Player::setItemCount, Mortar, 1);
      	 AI::callWithId(%AIName, Player::setItemCount, MortarAmmo, 100);
      	 AI::callWithId(%AIName, Player::setItemCount, PhaseDisrupter, 1);
      	 AI::callWithId(%AIName, Player::setItemCount, PhaseAmmo, 100);
       	 AI::callWithId(%AIName, Player::setItemCount, FlameThrower, 1);
      	 AI::callWithId(%AIName, Player::setItemCount, FlameThrowerAmmo, 200);
     	 AI::callWithId(%AIName, Player::setItemCount, Minigun, 1);
     	 AI::callWithId(%AIName, Player::setItemCount, MinigunAmmo, 200);
      	 AI::callWithId(%AIName, Player::setItemCount, RocketLauncher, 1);
      	 AI::callWithId(%AIName, Player::setItemCount, RocketAmmo, 100);
       	 AI::callWithId(%AIName, Player::setItemCount, Disclauncher, 1);
         	 AI::callWithId(%AIName, Player::setItemCount, DiscAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Flamer, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, FlamerAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, LaserRifle, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, PlasmaGun, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, PlasmaAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, RubberMortar, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RubberAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Shotgun, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, ShotgunShells, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Stinger, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, StingerAmmo, 100);
	 AI::callWithId(%AIName, Player::mountItem, Mortar, 0);
	 AI::callWithId(%AIName, Player::mountItem, EnergyPack, $BackpackSlot);
	 AI::SetVar( "*", triggerPct, 1000 );
	 AI::setVar( "*", SpotDist, $visDistance);
	 AI::SetAutomaticTargets(%AIName);
	 $DoingChore[%id] = false;
	 $Actions::Hunting[%id] = true;
	 $Actions::Speaking[%id] = true;
	 $Actions::Grenades[%id] = true;
	 $Actions::Beacons[%id] = true;
	 $Actions::Weapons[%id] = true;
	   $Actions::Deploys[%id] = true;
	 AnniBotsHuntTroll(%aiName);
	 	 AI::callbackPeriodic(%aiName, 5, AnniSwitchWeaponTroll);

}

function CreateSpearSoloTankFriendly(%clientID)
{
    if(GameBase::getPosition(%clientID) == "0 0 0")
    {
       client::sendmessage(%clientid, 1, "You must be spawned in to do that!");
       bottomprint(%clientID, "<f1><jc>You must be spawned in to do that!");
       Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       return;
    }

	if(!%clientID.isGoated)
	{
			if($BotCooldown == "true")
	{
		Client::sendMessage(%clientID,0,"Please wait a few seconds to spawn another bot.");
		return;
	}
    if(!%clientId.inArena)
	{
      		client::sendmessage(%clientid, 1, "You must be in Arena to do that!");
      	 	bottomprint(%clientID, "<f1><jc>You must be in Arena to do that!");
      		Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       		return;
	}

if($BotsArenaCount >= $BotsArenaMax)
   { 
     Client::sendMessage(%clientID,0,"Arena bots at maximum.");
    Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
   return;
    }
	}
	
		$BotCooldown = true;
	schedule("$BotCooldown=false;",2.0);
    
	 %Botname = "TankBot" @ $NumAI;
//	 %Botname = AnniBotName(%ClientID);
//   	 %spawnMarker = GameBase::getPosition(%clientID);
		if(!%clientID.isGoated)
	{
	%spawnMarker = Arena::pickRandomSpawn(%clientId);
	}
			if(%clientID.isGoated)
	{
     %spawnMarker = GameBase::getPosition(%clientID);
	}
  	 %xPos = getWord(%spawnMarker, 0) + (floor(getRandom() * 10)-5);
  	 %yPos = getword(%spawnMarker, 1) + (floor(getRandom() * 10)-5);
  	 %zPos = getWord(%spawnMarker, 2) + 5;
  	 %rPos = GameBase::getRotation(%clientID);

	 echo("ADMINMSG: The bot " @ %botname @ " : " @ %aiName @ " Joined the game.");
	 
	 	%spawnPos = GameBase::getPosition(%spawnMarker);
	%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);

  	 %team = GameBase::getTeam(%clientID);
	 $NumAI++;
	 %AIName = "TankBot" @ $NumAI;
  	 %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;	 
	 	 	 	 		 	if(%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorTank",%aiSpawnPos, "0 0 0", %AIName, "female2");
	}
	
	if(!%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorTank",%spawnPos, "0 0 0", %AIName, "female2");
	} 
	

	 
	 %id = AI::getId( %AIname );
	 	 %player = Client::getOwnedObject(%id);
	 %player.cnt = 0;
 	 GameBase::setTeam(%id, %team);
	 Client::setSkin(%id, $Server::teamSkin[Client::getTeam(%id)]);
//	schedule("AnniBotsDie(" @ %AIname @ ");", 90);
	%aiId = Client::getOwnedObject(AI::getID(%aiName));
	 %aiId.isDuelDuck = 1;

     	Client::sendMessage(%clientID,0,"You spawned a Team Tank bot.");
	Client::sendMessage(%clientID,0,"~wturretOff4.wav");
	bottomprint(%clientID, "<f1><jc>You spawned a Team Tank bot.");
	$BotsArenaCount++;

	 AI::setVar( %AIname,  iq,  120 );
 	 AI::setVar( %AIname,  attackMode, 0);
 	 AI::DirectiveTarget(%AIname, %Victim);
    	 AI::callWithId(%AIName, Player::setItemCount, EnergyPack, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
        	 AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
       	 AI::callWithId(%AIName, Player::setItemCount, TBlastCannon, 1);
        	 AI::callWithId(%AIName, Player::setItemCount, TBlastCannonAmmo, 100);
        	 AI::callWithId(%AIName, Player::setItemCount, TRocketLauncher, 1);
      	 AI::callWithId(%AIName, Player::setItemCount, TRocketLauncherAmmo, 100);
        	 AI::callWithId(%AIName, Player::setItemCount, TankRPGLauncher, 1);
       	 AI::callWithId(%AIName, Player::setItemCount, TankRPGAmmo, 100);
        	 AI::callWithId(%AIName, Player::setItemCount, TankShredder, 1);
        	 AI::callWithId(%AIName, Player::setItemCount, TankShredderAmmo, 200);
	 AI::callWithId(%AIName, Player::mountItem, TBlastCannon, 0);
	 AI::callWithId(%AIName, Player::mountItem, EnergyPack, $BackpackSlot);
	 AI::SetVar( "*", triggerPct, 1000 );
	 AI::setVar( "*", SpotDist, $visDistance);
	 AI::SetAutomaticTargets(%AIName);
	 $DoingChore[%id] = false;
	$Actions::Hunting[%id] = true;
	 $Actions::Speaking[%id] = true;
	 $Actions::Grenades[%id] = true;
	 $Actions::Beacons[%id] = true;
	 $Actions::Weapons[%id] = true;
	   $Actions::Deploys[%id] = true;
	 AnniBotsHuntTank(%aiName);
	 	 AI::callbackPeriodic(%aiName, 5, AnniSwitchWeaponTank);

}

function CreateSpearSoloTitanFriendly(%clientID)
{
    if(GameBase::getPosition(%clientID) == "0 0 0")
    {
       client::sendmessage(%clientid, 1, "You must be spawned in to do that!");
       bottomprint(%clientID, "<f1><jc>You must be spawned in to do that!");
       Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       return;
    }

	if(!%clientID.isGoated)
	{
			if($BotCooldown == "true")
	{
		Client::sendMessage(%clientID,0,"Please wait a few seconds to spawn another bot.");
		return;
	}
    if(!%clientId.inArena)
	{
      		client::sendmessage(%clientid, 1, "You must be in Arena to do that!");
      	 	bottomprint(%clientID, "<f1><jc>You must be in Arena to do that!");
      		Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       		return;
	}
if($BotsArenaCount >= $BotsArenaMax)
   { 
     Client::sendMessage(%clientID,0,"Arena bots at maximum.");
    Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
   return;
    }
	}
	
		$BotCooldown = true;
	schedule("$BotCooldown=false;",2.0);
    
	 %Botname = "TitanBot" @ $NumAI;
//	 %Botname = AnniBotName(%ClientID);
//   	 %spawnMarker = GameBase::getPosition(%clientID);
		if(!%clientID.isGoated)
	{
	%spawnMarker = Arena::pickRandomSpawn(%clientId);
	}
			if(%clientID.isGoated)
	{
     %spawnMarker = GameBase::getPosition(%clientID);
	}
  	 %xPos = getWord(%spawnMarker, 0) + (floor(getRandom() * 10)-5);
  	 %yPos = getword(%spawnMarker, 1) + (floor(getRandom() * 10)-5);
  	 %zPos = getWord(%spawnMarker, 2) + 5;
  	 %rPos = GameBase::getRotation(%clientID);

	 echo("ADMINMSG: The bot " @ %botname @ " : " @ %aiName @ " Joined the game.");
	 
	 	%spawnPos = GameBase::getPosition(%spawnMarker);
	%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);

  	 %team = GameBase::getTeam(%clientID);
	 $NumAI++;
	 %AIName = "TitanBot" @ $NumAI;
  	 %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;	 
	 	 	 	 	 		 	if(%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorTitan",%aiSpawnPos, "0 0 0", %AIName, "female2");
	}
	
	if(!%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorTitan",%spawnPos, "0 0 0", %AIName, "female2");
	} 
	

	 
	 %id = AI::getId( %AIname );
	 	 %player = Client::getOwnedObject(%id);
	 %player.cnt = 0;
 	 GameBase::setTeam(%id, %team);
	 Client::setSkin(%id, $Server::teamSkin[Client::getTeam(%id)]);
//	schedule("AnniBotsDie(" @ %AIname @ ");", 90);
	%aiId = Client::getOwnedObject(AI::getID(%aiName));
	 %aiId.isDuelDuck = 1;

     	Client::sendMessage(%clientID,0,"You spawned a Team Titan bot.");
	Client::sendMessage(%clientID,0,"~wturretOff4.wav");
	bottomprint(%clientID, "<f1><jc>You spawned a Team Titan bot.");
	$BotsArenaCount++;

	 AI::setVar( %AIname,  iq,  120 );
 	 AI::setVar( %AIname,  attackMode, 0);
 	 AI::DirectiveTarget(%AIname, %Victim);
    	 AI::callWithId(%AIName, Player::setItemCount, EnergyPack, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
         	AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, RocketLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RocketAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, PhaseDisrupter, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, PhaseAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, BabyNukeMortar, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, BabyNukeAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, OSLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, OSAmmo, 100);
         	AI::callWithId(%AIName, Player::setItemCount, ParticleBeamWeapon, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, Disclauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, DiscAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Flamer, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, FlamerAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, FlameThrower, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, FlameThrowerAmmo, 200);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, PlasmaGun, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, PlasmaAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, RubberMortar, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RubberAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Stinger, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, StingerAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Thumper, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, ThumperAmmo, 100);
	 AI::callWithId(%AIName, Player::mountItem, RocketLauncher, 0);
	 AI::callWithId(%AIName, Player::mountItem, EnergyPack, $BackpackSlot);
	 AI::SetVar( "*", triggerPct, 1000 );
	 AI::setVar( "*", SpotDist, $visDistance);
	 AI::SetAutomaticTargets(%AIName);
	 $DoingChore[%id] = false;
	$Actions::Hunting[%id] = true;
	 $Actions::Speaking[%id] = true;
	 $Actions::Grenades[%id] = true;
	 $Actions::Beacons[%id] = true;
	 $Actions::Weapons[%id] = true;
	   $Actions::Deploys[%id] = true;
	 AnniBotsHuntTitan(%aiName);
	 	 AI::callbackPeriodic(%aiName, 5, AnniSwitchWeaponTitan);
}
// end team bots

function CreateSpearSoloWarrior(%clientID)
{
    if(GameBase::getPosition(%clientID) == "0 0 0")
    {
       client::sendmessage(%clientid, 1, "You must be spawned in to do that!");
       bottomprint(%clientID, "<f1><jc>You must be spawned in to do that!");
       Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       return;
    }

	if(!%clientID.isGoated)
	{
			if($BotCooldown == "true")
	{
		Client::sendMessage(%clientID,0,"Please wait a few seconds to spawn another bot.");
		return;
	}
    if(!%clientId.inArena)
	{
      		client::sendmessage(%clientid, 1, "You must be in Arena to do that!");
      	 	bottomprint(%clientID, "<f1><jc>You must be in Arena to do that!");
      		Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       		return;
	}

if($BotsArenaCount >= $BotsArenaMax)
   { 
     Client::sendMessage(%clientID,0,"Arena bots at maximum.");
    Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
   return;
    }
	}
	
		$BotCooldown = true;
	schedule("$BotCooldown=false;",2.0);
    

	 %Botname = "WarriorBot" @ $NumAI;
//	 %Botname = AnniBotName(%ClientID);
//   	 %spawnMarker = GameBase::getPosition(%clientID);
		if(!%clientID.isGoated)
	{
	%spawnMarker = Arena::pickRandomSpawn(%clientId);
	}
			if(%clientID.isGoated)
	{
     %spawnMarker = GameBase::getPosition(%clientID);
	}
  	 %xPos = getWord(%spawnMarker, 0) + (floor(getRandom() * 10)-5);
  	 %yPos = getword(%spawnMarker, 1) + (floor(getRandom() * 10)-5);
  	 %zPos = getWord(%spawnMarker, 2) + 5;
  	 %rPos = GameBase::getRotation(%clientID);

	 echo("ADMINMSG: The bot " @ %botname @ " : " @ %aiName @ " Joined the game.");
	 
	 	%spawnPos = GameBase::getPosition(%spawnMarker);
	%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);

  	 %team = GameBase::getTeam(%clientID);
	 $NumAI++;
//	 %AIName = "WarriorDan";
	 %AIName = "WarriorBot" @ $NumAI;
	 
  	 %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;
	 
	if(%clientID.isGoated)
	{
	 // if(AI::spawn(%AIName,"armorfWarrior",%aiSpawnPos, "0 0 0", %AIName, "female2")) {}
	 AI::spawn(%AIName,"armorfWarrior",%aiSpawnPos, "0 0 0", %AIName, "female2");
	}
	
	if(!%clientID.isGoated)
	{
	// if(AI::spawn(%AIName,"armorfWarrior",%spawnPos, "0 0 0", %AIName, "female2")) {}
	AI::spawn(%AIName,"armorfWarrior",%spawnPos, "0 0 0", %AIName, "female2");
	}
	

	 %id = AI::getId( %AIname );
	 	 %player = Client::getOwnedObject(%id);
	 %player.cnt = 0;
 	 GameBase::setTeam(%id, 1);
	 client::setOwnedObject( %id ,  %id);
	 Client::setSkin(%id, $Server::teamSkin[Client::getTeam(%id)]);

//	schedule("AnniBotsDie(" @ %AIname @ ");", 90);

	%aiId = Client::getOwnedObject(AI::getID(%aiName));
	 %aiId.isDuelDuck = 1;

     	Client::sendMessage(%clientID,0,"You spawned a Warrior bot.");
	Client::sendMessage(%clientID,0,"~wturretOff4.wav");
	bottomprint(%clientID, "<f1><jc>You spawned a Warrior bot.");
	$BotsArenaCount++;

	 %id = AI::getId( %AIname );
	 AI::setVar( %AIname,  iq,  120 );
 	 AI::setVar( %AIname,  attackMode, 0);
 	 AI::DirectiveTarget(%AIname, %Victim);
    	 AI::callWithId(%AIName, Player::setItemCount, EnergyPack, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
       	 AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
        	 AI::callWithId(%AIName, Player::setItemCount, Disclauncher, 1);
      	 AI::callWithId(%AIName, Player::setItemCount, DiscAmmo, 100);
       	 AI::callWithId(%AIName, Player::setItemCount, Vulcan, 1);
       	 AI::callWithId(%AIName, Player::setItemCount, VulcanAmmo, 200);
       	 AI::callWithId(%AIName, Player::setItemCount, Stinger, 1);
       	 AI::callWithId(%AIName, Player::setItemCount, StingerAmmo, 50);
        	 AI::callWithId(%AIName, Player::setItemCount, FlameThrower, 1);
      	 AI::callWithId(%AIName, Player::setItemCount, FlameThrowerAmmo, 200);
        	 AI::callWithId(%AIName, Player::setItemCount, RocketLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RocketAmmo, 50);
    	 AI::callWithId(%AIName, Player::setItemCount, Blaster, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, Flamer, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, FlamerAmmo, 200);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeAmmo, 50);
    	 AI::callWithId(%AIName, Player::setItemCount, Hammer, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, HammerAmmo, 50);
    	 AI::callWithId(%AIname, Player::setItemCount, PlasmaGun, 1);
	 AI::callWithId(%AIname, Player::setItemCount, PlasmaAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, RubberMortar, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RubberAmmo, 50);
    	 AI::callWithId(%AIName, Player::setItemCount, ShockwaveGun, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, Shotgun, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, ShotgunShells, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Thumper, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, ThumperAmmo, 50);
	 AI::callWithId(%AIName, Player::mountItem, Disclauncher, 0);
	 AI::callWithId(%AIName, Player::mountItem, EnergyPack, $BackpackSlot);
	 AI::SetVar( "*", triggerPct, 1000 );
	 AI::setVar( "*", SpotDist, $visDistance);
	 AI::SetAutomaticTargets(%AIName);
	 $DoingChore[%id] = false;
	$Actions::Hunting[%id] = true;
	 $Actions::Speaking[%id] = true;
	 $Actions::Grenades[%id] = true;
	 $Actions::Beacons[%id] = true;
	 $Actions::Weapons[%id] = true;
	   $Actions::Deploys[%id] = true;
	 AnniBotsHuntWarrior(%aiName);
	 	 AI::callbackPeriodic(%aiName, 5, AnniSwitchWeaponWarrior);
}

function CreateSpearSoloAngel(%clientID)
{
    if(GameBase::getPosition(%clientID) == "0 0 0")
    {
       client::sendmessage(%clientid, 1, "You must be spawned in to do that!");
       bottomprint(%clientID, "<f1><jc>You must be spawned in to do that!");
       Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       return;
    }

	if(!%clientID.isGoated)
	{
			if($BotCooldown == "true")
	{
		Client::sendMessage(%clientID,0,"Please wait a few seconds to spawn another bot.");
		return;
	}
    if(!%clientId.inArena)
	{
      		client::sendmessage(%clientid, 1, "You must be in Arena to do that!");
      	 	bottomprint(%clientID, "<f1><jc>You must be in Arena to do that!");
      		Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       		return;
	}

if($BotsArenaCount >= $BotsArenaMax)
   { 
     Client::sendMessage(%clientID,0,"Arena bots at maximum.");
    Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
   return;
    }
	}
	
		$BotCooldown = true;
	schedule("$BotCooldown=false;",2.0);
    
	 %Botname = "AngelBot" @ $NumAI;
//	 %Botname = AnniBotName(%ClientID);
//   	 %spawnMarker = GameBase::getPosition(%clientID);
		if(!%clientID.isGoated)
	{
	%spawnMarker = Arena::pickRandomSpawn(%clientId);
	}
			if(%clientID.isGoated)
	{
     %spawnMarker = GameBase::getPosition(%clientID);
	}
  	 %xPos = getWord(%spawnMarker, 0) + (floor(getRandom() * 10)-5);
  	 %yPos = getword(%spawnMarker, 1) + (floor(getRandom() * 10)-5);
  	 %zPos = getWord(%spawnMarker, 2) + 5;
  	 %rPos = GameBase::getRotation(%clientID);

	 echo("ADMINMSG: The bot " @ %botname @ " : " @ %aiName @ " Joined the game.");
	 
	 	%spawnPos = GameBase::getPosition(%spawnMarker);
	%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);

  	 %team = GameBase::getTeam(%clientID);
	 $NumAI++;
	 %AIName = "AngelBot" @ $NumAI;
  	 %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;	 
	 	 	 	 	 	 		 	if(%clientID.isGoated)
	{
	  AI::spawn(%AIName,"armorfAngel",%aiSpawnPos, "0 0 0", %AIName, "female2");
	}
	
	if(!%clientID.isGoated)
	{
	  AI::spawn(%AIName,"armorfAngel",%spawnPos, "0 0 0", %AIName, "female2");
	} 
	

	 
	 %id = AI::getId( %AIname );
	 // AddToSet("MissionCleanup",%id);
	 	 %player = Client::getOwnedObject(%id);
	 %player.cnt = 0;
 	 GameBase::setTeam(%id, 1);
	 Client::setSkin(%id, $Server::teamSkin[Client::getTeam(%id)]);

//	schedule("AnniBotsDie(" @ %AIname @ ");", 90);

	%aiId = Client::getOwnedObject(AI::getID(%aiName));
	 %aiId.isDuelDuck = 1;

     	Client::sendMessage(%clientID,0,"You spawned a Angel bot.");
	Client::sendMessage(%clientID,0,"~wturretOff4.wav");
	bottomprint(%clientID, "<f1><jc>You spawned a Angel bot.");
	$BotsArenaCount++;

	 %id = AI::getId( %AIname );
	 AI::setVar( %AIname,  iq,  120 );
 	 AI::setVar( %AIname,  attackMode, 0);
 	 AI::DirectiveTarget(%AIname, %Victim);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairPack, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
         	AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
        	 AI::callWithId(%AIName, Player::setItemCount, HeavensFury, 1);
       	  AI::callWithId(%AIName, Player::setItemCount, SoulSucker, 1);
       	  AI::callWithId(%AIName, Player::setItemCount, GrapplingHook, 1);
        	AI::callWithId(%AIname, Player::setItemCount, AngelFire, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, AngelRepairGun, 1);
	 AI::callWithId(%AIName, Player::mountItem, AngelFire, 0);
	 AI::callWithId(%AIName, Player::mountItem, RepairPack, $BackpackSlot);
	 AI::SetVar( "*", triggerPct, 1000 );
	 AI::setVar( "*", SpotDist, $visDistance);
	 AI::SetAutomaticTargets(%AIName);
	 $DoingChore[%id] = false;
	$Actions::Hunting[%id] = true;
	 $Actions::Speaking[%id] = true;
	 $Actions::Grenades[%id] = true;
	 $Actions::Beacons[%id] = true;
	 $Actions::Weapons[%id] = true;
	   $Actions::Deploys[%id] = true;
	 AnniBotsHuntAngel(%aiName);
	 	 AI::callbackPeriodic(%aiName, 5, AnniSwitchWeaponAngel);
}

function CreateSpearSoloSpy(%clientID)
{
    if(GameBase::getPosition(%clientID) == "0 0 0")
    {
       client::sendmessage(%clientid, 1, "You must be spawned in to do that!");
       bottomprint(%clientID, "<f1><jc>You must be spawned in to do that!");
       Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       return;
    }

	if(!%clientID.isGoated)
	{
			if($BotCooldown == "true")
	{
		Client::sendMessage(%clientID,0,"Please wait a few seconds to spawn another bot.");
		return;
	}
    if(!%clientId.inArena)
	{
      		client::sendmessage(%clientid, 1, "You must be in Arena to do that!");
      	 	bottomprint(%clientID, "<f1><jc>You must be in Arena to do that!");
      		Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       		return;
	}

if($BotsArenaCount >= $BotsArenaMax)
   { 
     Client::sendMessage(%clientID,0,"Arena bots at maximum.");
    Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
   return;
    }
	}
	
		$BotCooldown = true;
	schedule("$BotCooldown=false;",2.0);
    
	 %Botname = "ChameleonBot" @ $NumAI;
//	 %Botname = AnniBotName(%ClientID);
//   	 %spawnMarker = GameBase::getPosition(%clientID);
		if(!%clientID.isGoated)
	{
	%spawnMarker = Arena::pickRandomSpawn(%clientId);
	}
			if(%clientID.isGoated)
	{
     %spawnMarker = GameBase::getPosition(%clientID);
	}
  	 %xPos = getWord(%spawnMarker, 0) + (floor(getRandom() * 10)-5);
  	 %yPos = getword(%spawnMarker, 1) + (floor(getRandom() * 10)-5);
  	 %zPos = getWord(%spawnMarker, 2) + 5;
  	 %rPos = GameBase::getRotation(%clientID);

	 echo("ADMINMSG: The bot " @ %botname @ " : " @ %aiName @ " Joined the game.");
	 
	 	%spawnPos = GameBase::getPosition(%spawnMarker);
	%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);

  	 %team = GameBase::getTeam(%clientID);
	 $NumAI++;
	 %AIName = "ChameleonBot" @ $NumAI;
  	 %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;	 
	 	 	 	 	 	 	 		 	if(%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorfSpy",%aiSpawnPos, "0 0 0", %AIName, "female2");
	}
	
	if(!%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorfSpy",%spawnPos, "0 0 0", %AIName, "female2");
	} 
	
	 
	 %id = AI::getId( %AIname );
	 	 %player = Client::getOwnedObject(%id);
	 %player.cnt = 0;
 	 GameBase::setTeam(%id, 1);
	 Client::setSkin(%id, $Server::teamSkin[Client::getTeam(%id)]);
//	schedule("AnniBotsDie(" @ %AIname @ ");", 90);
	%aiId = Client::getOwnedObject(AI::getID(%aiName));
	 %aiId.isDuelDuck = 1;

     	Client::sendMessage(%clientID,0,"You spawned a Chameleon bot.");
	Client::sendMessage(%clientID,0,"~wturretOff4.wav");
	bottomprint(%clientID, "<f1><jc>You spawned a Chameleon bot.");
	$BotsArenaCount++;

	 %id = AI::getId( %AIname );
	 AI::setVar( %AIname,  iq,  120 );
 	 AI::setVar( %AIname,  attackMode, 0);
 	 AI::DirectiveTarget(%AIname, %Victim);
    	 AI::callWithId(%AIName, Player::setItemCount, EnergyPack, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
         	 AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
       	 AI::callWithId(%AIName, Player::setItemCount, PlasmaGun, 1);
       	 AI::callWithId(%AIName, Player::setItemCount, PlasmaAmmo, 100);
        	 AI::callWithId(%AIName, Player::setItemCount, SniperRifle, 1);
     	 AI::callWithId(%AIName, Player::setItemCount, SniperAmmo, 100);
       	 AI::callWithId(%AIName, Player::setItemCount, Disclauncher, 1);
      	 AI::callWithId(%AIName, Player::setItemCount, DiscAmmo, 100);
       	 AI::callWithId(%AIName, Player::setItemCount, Shotgun, 1);
         	 AI::callWithId(%AIName, Player::setItemCount, ShotgunShells, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Flamer, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, FlamerAmmo, 200);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Hammer, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, HammerAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, ShockwaveGun, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, LaserRifle, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, Thumper, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, ThumperAmmo, 100);
	 AI::callWithId(%AIName, Player::mountItem, Shotgun, 0);
	 AI::callWithId(%AIName, Player::mountItem, EnergyPack, $BackpackSlot);
	 AI::SetVar( "*", triggerPct, 1000 );
	 AI::setVar( "*", SpotDist, $visDistance);
	 AI::SetAutomaticTargets(%AIName);
	 $DoingChore[%id] = false;
	$Actions::Hunting[%id] = true;
	 $Actions::Speaking[%id] = true;
	 $Actions::Grenades[%id] = true;
	 $Actions::Beacons[%id] = true;
	 $Actions::Weapons[%id] = true;
	   $Actions::Deploys[%id] = true;
	 AnniBotsHuntSpy(%aiName);
	 	 AI::callbackPeriodic(%aiName, 5, AnniSwitchWeaponSpy);
}

function CreateSpearSoloNecro(%clientID)
{
    if(GameBase::getPosition(%clientID) == "0 0 0")
    {
       client::sendmessage(%clientid, 1, "You must be spawned in to do that!");
       bottomprint(%clientID, "<f1><jc>You must be spawned in to do that!");
       Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       return;
    }

	if(!%clientID.isGoated)
	{
			if($BotCooldown == "true")
	{
		Client::sendMessage(%clientID,0,"Please wait a few seconds to spawn another bot.");
		return;
	}
    if(!%clientId.inArena)
	{
      		client::sendmessage(%clientid, 1, "You must be in Arena to do that!");
      	 	bottomprint(%clientID, "<f1><jc>You must be in Arena to do that!");
      		Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       		return;
	}

if($BotsArenaCount >= $BotsArenaMax)
   { 
     Client::sendMessage(%clientID,0,"Arena bots at maximum.");
    Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
   return;
    }
	}
	
		$BotCooldown = true;
	schedule("$BotCooldown=false;",2.0);
    
	 %Botname = "NecromancerBot" @ $NumAI;
//	 %Botname = AnniBotName(%ClientID);
//   	 %spawnMarker = GameBase::getPosition(%clientID);
		if(!%clientID.isGoated)
	{
	%spawnMarker = Arena::pickRandomSpawn(%clientId);
	}
			if(%clientID.isGoated)
	{
     %spawnMarker = GameBase::getPosition(%clientID);
	}
  	 %xPos = getWord(%spawnMarker, 0) + (floor(getRandom() * 10)-5);
  	 %yPos = getword(%spawnMarker, 1) + (floor(getRandom() * 10)-5);
  	 %zPos = getWord(%spawnMarker, 2) + 5;
  	 %rPos = GameBase::getRotation(%clientID);

	 echo("ADMINMSG: The bot " @ %botname @ " : " @ %aiName @ " Joined the game.");
	 
	 	%spawnPos = GameBase::getPosition(%spawnMarker);
	%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);

  	 %team = GameBase::getTeam(%clientID);
	 $NumAI++;
	 %AIName = "NecromancerBot" @ $NumAI;
  	 %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;	 
	 	 	 	 	 	 	 	 		 	if(%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorfNecro",%aiSpawnPos, "0 0 0", %AIName, "female2");
	}
	
	if(!%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorfNecro",%spawnPos, "0 0 0", %AIName, "female2");
	} 
	
	 
	 %id = AI::getId( %AIname );
	 	 %player = Client::getOwnedObject(%id);
	 %player.cnt = 0;
 	 GameBase::setTeam(%id, 1);
	 Client::setSkin(%id, $Server::teamSkin[Client::getTeam(%id)]);
//	schedule("AnniBotsDie(" @ %AIname @ ");", 90);
	%aiId = Client::getOwnedObject(AI::getID(%aiName));
	 %aiId.isDuelDuck = 1;

     	Client::sendMessage(%clientID,0,"You spawned a Necromancer bot.");
	Client::sendMessage(%clientID,0,"~wturretOff4.wav");
	bottomprint(%clientID, "<f1><jc>You spawned a Necromancer bot.");
	$BotsArenaCount++;

	 %id = AI::getId( %AIname );
	 AI::setVar( %AIname,  iq,  120 );
 	 AI::setVar( %AIname,  attackMode, 0);
 	 AI::DirectiveTarget(%AIname, %Victim);
    	 AI::callWithId(%AIName, Player::setItemCount, EnergyPack, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
        	 AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
         	AI::callWithId(%AIName, Player::setItemCount, DisarmerSpell, 1);
       	  AI::callWithId(%AIName, Player::setItemCount, ShockingGrasp, 1);
         	AI::callWithId(%AIName, Player::setItemCount, SpellFlameThrower, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, DeathRay, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, FlameStrike, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, Stasis, 1);
	 AI::callWithId(%AIName, Player::mountItem, SpellFlameThrower, 0);
	 AI::callWithId(%AIName, Player::mountItem, EnergyPack, $BackpackSlot);
	 AI::SetVar( "*", triggerPct, 1000 );
	 AI::setVar( "*", SpotDist, $visDistance);
	 AI::SetAutomaticTargets(%AIName);
	 $DoingChore[%id] = false;
	$Actions::Hunting[%id] = true;
	 $Actions::Speaking[%id] = true;
	 $Actions::Grenades[%id] = true;
	 $Actions::Beacons[%id] = true;
	 $Actions::Weapons[%id] = true;
	   $Actions::Deploys[%id] = true;
	 AnniBotsHuntNecro(%aiName);
	 	 AI::callbackPeriodic(%aiName, 5, AnniSwitchWeaponNecro);
}

function CreateSpearSoloBuilder(%clientID)
{
    if(GameBase::getPosition(%clientID) == "0 0 0")
    {
       client::sendmessage(%clientid, 1, "You must be spawned in to do that!");
       bottomprint(%clientID, "<f1><jc>You must be spawned in to do that!");
       Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       return;
    }

	if(!%clientID.isGoated)
	{
			if($BotCooldown == "true")
	{
		Client::sendMessage(%clientID,0,"Please wait a few seconds to spawn another bot.");
		return;
	}
    if(!%clientId.inArena)
	{
      		client::sendmessage(%clientid, 1, "You must be in Arena to do that!");
      	 	bottomprint(%clientID, "<f1><jc>You must be in Arena to do that!");
      		Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       		return;
	}

if($BotsArenaCount >= $BotsArenaMax)
   { 
     Client::sendMessage(%clientID,0,"Arena bots at maximum.");
    Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
   return;
    }
	}
	
		$BotCooldown = true;
	schedule("$BotCooldown=false;",2.0);
    
	 %Botname = "BuilderBot" @ $NumAI;
//	 %Botname = AnniBotName(%ClientID);
//   	 %spawnMarker = GameBase::getPosition(%clientID);
		if(!%clientID.isGoated)
	{
	%spawnMarker = Arena::pickRandomSpawn(%clientId);
	}
			if(%clientID.isGoated)
	{
     %spawnMarker = GameBase::getPosition(%clientID);
	}
  	 %xPos = getWord(%spawnMarker, 0) + (floor(getRandom() * 10)-5);
  	 %yPos = getword(%spawnMarker, 1) + (floor(getRandom() * 10)-5);
  	 %zPos = getWord(%spawnMarker, 2) + 5;
  	 %rPos = GameBase::getRotation(%clientID);

	 echo("ADMINMSG: The bot " @ %botname @ " : " @ %aiName @ " Joined the game.");
	 
	 	%spawnPos = GameBase::getPosition(%spawnMarker);
	%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);

  	 %team = GameBase::getTeam(%clientID);
	 $NumAI++;
	 %AIName = "BuilderBot" @ $NumAI;
  	 %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;	 
	 	 	 	 	 	 	 	 	 		 	if(%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorfBuilder",%aiSpawnPos, "0 0 0", %AIName, "female2");
	}
	
	if(!%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorfBuilder",%spawnPos, "0 0 0", %AIName, "female2");
	} 
	
	 
	 %id = AI::getId( %AIname );
	 	 %player = Client::getOwnedObject(%id);
	 %player.cnt = 0;
 	 GameBase::setTeam(%id, 1);
	 Client::setSkin(%id, $Server::teamSkin[Client::getTeam(%id)]);
//	schedule("AnniBotsDie(" @ %AIname @ ");", 90);
	%aiId = Client::getOwnedObject(AI::getID(%aiName));
	 %aiId.isDuelDuck = 1;

     	Client::sendMessage(%clientID,0,"You spawned a Builder bot.");
	Client::sendMessage(%clientID,0,"~wturretOff4.wav");
	bottomprint(%clientID, "<f1><jc>You spawned a Builder bot.");
	$BotsArenaCount++;

	 %id = AI::getId( %AIname );
	 AI::setVar( %AIname,  iq,  120 );
 	 AI::setVar( %AIname,  attackMode, 0);
 	 AI::DirectiveTarget(%AIname, %Victim);
    	 AI::callWithId(%AIName, Player::setItemCount, DeployableInvPack, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
        	 AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
      	 AI::callWithId(%AIName, Player::setItemCount, Railgun, 1);
      	 AI::callWithId(%AIName, Player::setItemCount, RailAmmo, 100);
      	 AI::callWithId(%AIName, Player::setItemCount, Disclauncher, 1);
       	 AI::callWithId(%AIName, Player::setItemCount, DiscAmmo, 100);
      	 AI::callWithId(%AIName, Player::setItemCount, RocketLauncher, 1);
        	 AI::callWithId(%AIName, Player::setItemCount, RocketAmmo, 100);
         	 AI::callWithId(%AIName, Player::setItemCount, Pitchfork, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, Flamer, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, FlamerAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, PlasmaGun, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, PlasmaAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Shotgun, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, ShotgunShells, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Stinger, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, StingerAmmo, 100);
    	 AI::callWithId(%AIname, Player::setItemCount, Vulcan, 1);
	 AI::callWithId(%AIname, Player::setItemCount, VulcanAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Thumper, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, ThumperAmmo, 100);
	 AI::callWithId(%AIName, Player::mountItem, GrenadeLauncher, 0);
	 AI::callWithId(%AIName, Player::mountItem, DeployableInvPack, $BackpackSlot);
	 AI::SetVar( "*", triggerPct, 1000 );
	 AI::setVar( "*", SpotDist, $visDistance);
	 AI::SetAutomaticTargets(%AIName);
	 $DoingChore[%id] = false;
	$Actions::Hunting[%id] = true;
	 $Actions::Speaking[%id] = true;
	 $Actions::Grenades[%id] = true;
	 $Actions::Beacons[%id] = true;
	 $Actions::Weapons[%id] = true;
	   $Actions::Deploys[%id] = true;
	 AnniBotsHuntBuilder(%aiName);
	 	 AI::callbackPeriodic(%aiName, 5, AnniSwitchWeaponBuilder);
}

function CreateSpearSoloTroll(%clientID)
{
    if(GameBase::getPosition(%clientID) == "0 0 0")
    {
       client::sendmessage(%clientid, 1, "You must be spawned in to do that!");
       bottomprint(%clientID, "<f1><jc>You must be spawned in to do that!");
       Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       return;
    }

	if(!%clientID.isGoated)
	{
			if($BotCooldown == "true")
	{
		Client::sendMessage(%clientID,0,"Please wait a few seconds to spawn another bot.");
		return;
	}
    if(!%clientId.inArena)
	{
      		client::sendmessage(%clientid, 1, "You must be in Arena to do that!");
      	 	bottomprint(%clientID, "<f1><jc>You must be in Arena to do that!");
      		Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       		return;
	}

if($BotsArenaCount >= $BotsArenaMax)
   { 
     Client::sendMessage(%clientID,0,"Arena bots at maximum.");
    Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
   return;
    }
	}
	
		$BotCooldown = true;
	schedule("$BotCooldown=false;",2.0);
    
	 %Botname = "TrollBot" @ $NumAI;
//	 %Botname = AnniBotName(%ClientID);
//   	 %spawnMarker = GameBase::getPosition(%clientID);
		if(!%clientID.isGoated)
	{
	%spawnMarker = Arena::pickRandomSpawn(%clientId);
	}
			if(%clientID.isGoated)
	{
     %spawnMarker = GameBase::getPosition(%clientID);
	}
  	 %xPos = getWord(%spawnMarker, 0) + (floor(getRandom() * 10)-5);
  	 %yPos = getword(%spawnMarker, 1) + (floor(getRandom() * 10)-5);
  	 %zPos = getWord(%spawnMarker, 2) + 5;
  	 %rPos = GameBase::getRotation(%clientID);

	 echo("ADMINMSG: The bot " @ %botname @ " : " @ %aiName @ " Joined the game.");
	 
	 	%spawnPos = GameBase::getPosition(%spawnMarker);
	%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);

  	 %team = GameBase::getTeam(%clientID);
	 $NumAI++;
	 %AIName = "TrollBot" @ $NumAI;
  	 %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;	 
	 	 	 	 	 	 	 	 	 	 		 	if(%clientID.isGoated)
	{
	AI::spawn(%AIName,"armorTroll",%aiSpawnPos, "0 0 0", %AIName, "female2");
	}
	
	if(!%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorTroll",%spawnPos, "0 0 0", %AIName, "female2");
	} 
	
	 
	 %id = AI::getId( %AIname );
	 	 %player = Client::getOwnedObject(%id);
	 %player.cnt = 0;
 	 GameBase::setTeam(%id, 1);
	 Client::setSkin(%id, $Server::teamSkin[Client::getTeam(%id)]);
//	schedule("AnniBotsDie(" @ %AIname @ ");", 90);
	%aiId = Client::getOwnedObject(AI::getID(%aiName));
	 %aiId.isDuelDuck = 1;

     	Client::sendMessage(%clientID,0,"You spawned a Troll bot.");
	Client::sendMessage(%clientID,0,"~wturretOff4.wav");
	bottomprint(%clientID, "<f1><jc>You spawned a Troll bot.");
	$BotsArenaCount++;

	 %id = AI::getId( %AIname );
	 AI::setVar( %AIname,  iq,  120 );
 	 AI::setVar( %AIname,  attackMode, 0);
 	 AI::DirectiveTarget(%AIname, %Victim);
    	 AI::callWithId(%AIName, Player::setItemCount, EnergyPack, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
        	 AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
         	 AI::callWithId(%AIName, Player::setItemCount, Mortar, 1);
       	 AI::callWithId(%AIName, Player::setItemCount, MortarAmmo, 100);
      	 AI::callWithId(%AIName, Player::setItemCount, PhaseDisrupter, 1);
       	 AI::callWithId(%AIName, Player::setItemCount, PhaseAmmo, 100);
     	 AI::callWithId(%AIName, Player::setItemCount, FlameThrower, 1);
      	 AI::callWithId(%AIName, Player::setItemCount, FlameThrowerAmmo, 200);
       	 AI::callWithId(%AIName, Player::setItemCount, Minigun, 1);
       	 AI::callWithId(%AIName, Player::setItemCount, MinigunAmmo, 200);
      	 AI::callWithId(%AIName, Player::setItemCount, RocketLauncher, 1);
       	 AI::callWithId(%AIName, Player::setItemCount, RocketAmmo, 100);
      	 AI::callWithId(%AIName, Player::setItemCount, Disclauncher, 1);
        	 AI::callWithId(%AIName, Player::setItemCount, DiscAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Flamer, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, FlamerAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, LaserRifle, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, PlasmaGun, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, PlasmaAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, RubberMortar, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RubberAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Shotgun, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, ShotgunShells, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Stinger, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, StingerAmmo, 100);
	 AI::callWithId(%AIName, Player::mountItem, Mortar, 0);
	 AI::callWithId(%AIName, Player::mountItem, EnergyPack, $BackpackSlot);
	 AI::SetVar( "*", triggerPct, 1000 );
	 AI::setVar( "*", SpotDist, $visDistance);
	 AI::SetAutomaticTargets(%AIName);
	 $DoingChore[%id] = false;
	$Actions::Hunting[%id] = true;
	 $Actions::Speaking[%id] = true;
	 $Actions::Grenades[%id] = true;
	 $Actions::Beacons[%id] = true;
	 $Actions::Weapons[%id] = true;
	   $Actions::Deploys[%id] = true;
	 AnniBotsHuntTroll(%aiName);
	 	 AI::callbackPeriodic(%aiName, 5, AnniSwitchWeaponTroll);

}

function CreateSpearSoloTank(%clientID)
{
    if(GameBase::getPosition(%clientID) == "0 0 0")
    {
       client::sendmessage(%clientid, 1, "You must be spawned in to do that!");
       bottomprint(%clientID, "<f1><jc>You must be spawned in to do that!");
       Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       return;
    }

	if(!%clientID.isGoated)
	{
			if($BotCooldown == "true")
	{
		Client::sendMessage(%clientID,0,"Please wait a few seconds to spawn another bot.");
		return;
	}
    if(!%clientId.inArena)
	{
      		client::sendmessage(%clientid, 1, "You must be in Arena to do that!");
      	 	bottomprint(%clientID, "<f1><jc>You must be in Arena to do that!");
      		Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       		return;
	}

if($BotsArenaCount >= $BotsArenaMax)
   { 
     Client::sendMessage(%clientID,0,"Arena bots at maximum.");
    Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
   return;
    }
	}
	
		$BotCooldown = true;
	schedule("$BotCooldown=false;",2.0);
    
	 %Botname = "TrollBot" @ $NumAI;
//	 %Botname = AnniBotName(%ClientID);
//   	 %spawnMarker = GameBase::getPosition(%clientID);
		if(!%clientID.isGoated)
	{
	%spawnMarker = Arena::pickRandomSpawn(%clientId);
	}
			if(%clientID.isGoated)
	{
     %spawnMarker = GameBase::getPosition(%clientID);
	}
  	 %xPos = getWord(%spawnMarker, 0) + (floor(getRandom() * 10)-5);
  	 %yPos = getword(%spawnMarker, 1) + (floor(getRandom() * 10)-5);
  	 %zPos = getWord(%spawnMarker, 2) + 5;
  	 %rPos = GameBase::getRotation(%clientID);

	 echo("ADMINMSG: The bot " @ %botname @ " : " @ %aiName @ " Joined the game.");
	 
	 	%spawnPos = GameBase::getPosition(%spawnMarker);
	%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);

  	 %team = GameBase::getTeam(%clientID);
	 $NumAI++;
	 %AIName = "TankBot" @ $NumAI;
  	 %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;	 
	 	 	 	 	 	 	 	 	 	 	 		 	if(%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorTank",%aiSpawnPos, "0 0 0", %AIName, "female2");
	}
	
	if(!%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorTank",%spawnPos, "0 0 0", %AIName, "female2");
	} 
	
	 
	 %id = AI::getId( %AIname );
	 %player = Client::getOwnedObject(%id);
	 %player.cnt = 0;
 	 GameBase::setTeam(%id, 1);
	 Client::setSkin(%id, $Server::teamSkin[Client::getTeam(%id)]);
//	schedule("AnniBotsDie(" @ %AIname @ ");", 90);
	%aiId = Client::getOwnedObject(AI::getID(%aiName));
	 %aiId.isDuelDuck = 1;

     	Client::sendMessage(%clientID,0,"You spawned a Tank bot.");
	Client::sendMessage(%clientID,0,"~wturretOff4.wav");
	bottomprint(%clientID, "<f1><jc>You spawned a Tank bot.");
	$BotsArenaCount++;

	 %id = AI::getId( %AIname );
	 AI::setVar( %AIname,  iq,  120 );
 	 AI::setVar( %AIname,  attackMode, 0);
 	 AI::DirectiveTarget(%AIname, %Victim);
    	 AI::callWithId(%AIName, Player::setItemCount, EnergyPack, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
        	 AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
        	 AI::callWithId(%AIName, Player::setItemCount, TBlastCannon, 1);
        	 AI::callWithId(%AIName, Player::setItemCount, TBlastCannonAmmo, 100);
       	  AI::callWithId(%AIName, Player::setItemCount, TRocketLauncher, 1);
        	 AI::callWithId(%AIName, Player::setItemCount, TRocketLauncherAmmo, 100);
        	 AI::callWithId(%AIName, Player::setItemCount, TankRPGLauncher, 1);
        	 AI::callWithId(%AIName, Player::setItemCount, TankRPGAmmo, 100);
        	 AI::callWithId(%AIName, Player::setItemCount, TankShredder, 1);
        	 AI::callWithId(%AIName, Player::setItemCount, TankShredderAmmo, 200);
	 AI::callWithId(%AIName, Player::mountItem, TBlastCannon, 0);
	 AI::callWithId(%AIName, Player::mountItem, EnergyPack, $BackpackSlot);
	 AI::SetVar( "*", triggerPct, 1000 );
	 AI::setVar( "*", SpotDist, $visDistance);
	 AI::SetAutomaticTargets(%AIName);
	 $DoingChore[%id] = false;
	$Actions::Hunting[%id] = true;
	 $Actions::Speaking[%id] = true;
	 $Actions::Grenades[%id] = true;
	 $Actions::Beacons[%id] = true;
	 $Actions::Weapons[%id] = true;
	   $Actions::Deploys[%id] = true;
	 AnniBotsHuntTank(%aiName);
	 	 AI::callbackPeriodic(%aiName, 5, AnniSwitchWeaponTank);

}

function CreateSpearSoloTitan(%clientID)
{
    if(GameBase::getPosition(%clientID) == "0 0 0")
    {
       client::sendmessage(%clientid, 1, "You must be spawned in to do that!");
       bottomprint(%clientID, "<f1><jc>You must be spawned in to do that!");
       Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       return;
    }

	if(!%clientID.isGoated)
	{
			if($BotCooldown == "true")
	{
		Client::sendMessage(%clientID,0,"Please wait a few seconds to spawn another bot.");
		return;
	}
    if(!%clientId.inArena)
	{
      		client::sendmessage(%clientid, 1, "You must be in Arena to do that!");
      	 	bottomprint(%clientID, "<f1><jc>You must be in Arena to do that!");
      		Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
       		return;
	}

if($BotsArenaCount >= $BotsArenaMax)
   { 
     Client::sendMessage(%clientID,0,"Arena bots at maximum.");
    Client::sendMessage(%clientID,0,"~wC_BuySell.wav");
   return;
    }
	}
	
		$BotCooldown = true;
	schedule("$BotCooldown=false;",2.0);
    
	 %Botname = "TitanBot" @ $NumAI;
//	 %Botname = AnniBotName(%ClientID);
//   	 %spawnMarker = GameBase::getPosition(%clientID);
		if(!%clientID.isGoated)
	{
	%spawnMarker = Arena::pickRandomSpawn(%clientId);
	}
			if(%clientID.isGoated)
	{
     %spawnMarker = GameBase::getPosition(%clientID);
	}
  	 %xPos = getWord(%spawnMarker, 0) + (floor(getRandom() * 10)-5);
  	 %yPos = getword(%spawnMarker, 1) + (floor(getRandom() * 10)-5);
  	 %zPos = getWord(%spawnMarker, 2) + 5;
  	 %rPos = GameBase::getRotation(%clientID);

	 echo("ADMINMSG: The bot " @ %botname @ " : " @ %aiName @ " Joined the game.");
	 
	 	%spawnPos = GameBase::getPosition(%spawnMarker);
	%spawnRot = "0 0 "@ getword(GameBase::getRotation(%spawnMarker),2);

  	 %team = GameBase::getTeam(%clientID);
	 $NumAI++;
	 %AIName = "TitanBot" @ $NumAI;
  	 %aiSpawnPos = %xPos @ "  " @ %yPos @ "  " @ %zPos;
	 
	if(%clientID.isGoated)
	{
	  AI::spawn(%AIName,"armorTitan",%aiSpawnPos, "0 0 0", %AIName, "female2");
	}
	
	if(!%clientID.isGoated)
	{
	 AI::spawn(%AIName,"armorTitan",%spawnPos, "0 0 0", %AIName, "female2");
	} 
	
	 
	 %id = AI::getId( %AIname );
	 	 %player = Client::getOwnedObject(%id);
	 %player.cnt = 0;
 	 GameBase::setTeam(%id, 1);
	 Client::setSkin(%id, $Server::teamSkin[Client::getTeam(%id)]);
//	schedule("AnniBotsDie(" @ %AIname @ ");", 90);
	%aiId = Client::getOwnedObject(AI::getID(%aiName));
	 %aiId.isDuelDuck = 1;

     	Client::sendMessage(%clientID,0,"You spawned a Titan bot.");
	Client::sendMessage(%clientID,0,"~wturretOff4.wav");
	bottomprint(%clientID, "<f1><jc>You spawned a Titan bot.");
	$BotsArenaCount++;

	 %id = AI::getId( %AIname );
	 AI::setVar( %AIname,  iq,  120 );
 	 AI::setVar( %AIname,  attackMode, 0);
 	 AI::DirectiveTarget(%AIname, %Victim);
    	 AI::callWithId(%AIName, Player::setItemCount, EnergyPack, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RepairKit, 1);
         	AI::callWithId(%AIName, Player::setItemCount, Grenade, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Beacon, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, RocketLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RocketAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, PhaseDisrupter, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, PhaseAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, BabyNukeMortar, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, BabyNukeAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, OSLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, OSAmmo, 100);
         	AI::callWithId(%AIName, Player::setItemCount, ParticleBeamWeapon, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, Disclauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, DiscAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Flamer, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, FlamerAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, FlameThrower, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, FlameThrowerAmmo, 200);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeLauncher, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, GrenadeAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, PlasmaGun, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, PlasmaAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, RubberMortar, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, RubberAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Stinger, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, StingerAmmo, 100);
    	 AI::callWithId(%AIName, Player::setItemCount, Thumper, 1);
    	 AI::callWithId(%AIName, Player::setItemCount, ThumperAmmo, 100);
	 AI::callWithId(%AIName, Player::mountItem, RocketLauncher, 0);
	 AI::callWithId(%AIName, Player::mountItem, EnergyPack, $BackpackSlot);
	 AI::SetVar( "*", triggerPct, 1000 );
	 AI::setVar( "*", SpotDist, $visDistance);
	 AI::SetAutomaticTargets(%AIName);
	 $DoingChore[%id] = false;
	$Actions::Hunting[%id] = true;
	 $Actions::Speaking[%id] = true;
	 $Actions::Grenades[%id] = true;
	 $Actions::Beacons[%id] = true;
	 $Actions::Weapons[%id] = true;
	   $Actions::Deploys[%id] = true;
	 AnniBotsHuntTitan(%aiName);
	 	 AI::callbackPeriodic(%aiName, 5, AnniSwitchWeaponTitan);
}

function AnniThrowGrenade(%aiName)
{

	%id = AI::GetID(%aiName);
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }

    if(!$Actions::Grenades[%id]) 
    {
        return;
    }
	
		// echo("Anni Throw Grenade Ran.");

	   %curTarget = ai::getTarget( %aiName );

	   if(%curTarget == -1)
	   {
  	 	return;
	   }
	  // 	messageall(1, "Anni Throw Grenade.");

	   dbecho(1, %aiName @ " target: " @ %curTarget);

	   %targLoc = GameBase::getPosition(Client::getOwnedObject(%curTarget));
	   %aiLoc = GameBase::getPosition(Client::getOwnedObject(%id));
	   %targetDist = Vector::getDistance(%aiLoc, %targLoc);

	   %botPlayer = Client::getOwnedObject(%id);
	   %armor = Player::getArmor(%botPlayer);

	$boomRockMap["armorTroll"] = "Firebomb";
	$boomRockMap["armorTank"] = "TankBomb";
	$boomRockMap["armorTitan"] = "Mortarbomb";
	$boomRockMap["armorfSpy"] = "Nukebomb";
	$boomRockMap["armormSpy"] = "Nukebomb";
	$boomRockMap["armorfNecro"] = "HoloMine";
	$boomRockMap["armormNecro"] = "HoloMine";
	$boomRockMap["armorfWarrior"] = "Handgrenade";
	$boomRockMap["armormWarrior"] = "Handgrenade";
	$boomRockMap["armorfBuilder"] = "Shockgrenade";
	$boomRockMap["armormBuilder"] = "Shockgrenade";
	$boomRockMap["armorfAngel"] = "ClusterBombmine";

	if(%targetDist < 150 && %targetDist > 25)
		{
    			%boomrock = newObject("","Mine",$boomRockMap[%armor]);
    			addToSet("MissionCleanup", %boomrock);
    			GameBase::throw(%boomrock,%botPlayer,(%targetdist/75) * 15.0,false);
		}

}

function ArenaBotBeaconTroll(%aiName)
{
	%id = AI::GetID(%aiName);

    if(!$Actions::Beacons[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
   		%client = Player::GetClient(%id);			
		%curTarget = ai::getTarget( %aiName );

	   if(%curTarget == -1)
	   {
  	 	return;
	   }

	   dbecho(1, %aiName @ " target: " @ %curTarget);
	   %targLoc = GameBase::getPosition(Client::getOwnedObject(%curTarget));
	   %aiLoc = GameBase::getPosition(Client::getOwnedObject(%id));
	   %targetDist = Vector::getDistance(%aiLoc, %targLoc);

 	  if(%targetDist < 15)
 	  {
						GameBase::playSound(%player, SoundFirePlasma, 0);
						for(%i=0; %i < 6.28; %i += 1.256) 
						{
							%forceVel = Vector::getFromRot("0 0 " @ %i, 10, 5);
							%trans =  "0 0 1 0 0 0 0 0 1 " @ getBoxCenter(%player);
 							%obj = Projectile::spawnProjectile("TrollBurn", %trans, %player, %forceVel);
							Projectile::spawnProjectile(%obj);
							Item::setVelocity(%obj, %forceVel);

							%forceVel = Vector::getFromRot("0 0 " @ %i, 11, 6);
							%trans =  "0 0 2 0 0 0 0 0 2 " @ getBoxCenter(%player);
 							%obj = Projectile::spawnProjectile("TrollBurn2", %trans, %player, %forceVel);
							Projectile::spawnProjectile(%obj);
							Item::setVelocity(%obj, %forceVel);
						}
	  }
//	messageall(1, "Beacon Beacon Beacon.");
}

function ArenaBotBeaconTank(%aiName)
{
	%id = AI::GetID(%aiName);

    if(!$Actions::Beacons[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
   		%client = Player::GetClient(%id);			
		%curTarget = ai::getTarget( %aiName );

	   if(%curTarget == -1)
	   {
  	 	return;
	   }

	   dbecho(1, %aiName @ " target: " @ %curTarget);
	   %targLoc = GameBase::getPosition(Client::getOwnedObject(%curTarget));
	   %aiLoc = GameBase::getPosition(Client::getOwnedObject(%id));
	   %targetDist = Vector::getDistance(%aiLoc, %targLoc);

 	  if(%targetDist < 10)
 	  {
						GameBase::playSound(%player, debrisMediumExplosion, 0);
						%vel = Item::getVelocity(%player);
						%trans = "0 0 1 0 0 1 0 0 1 " @ vector::add(getBoxCenter(%player),"0 0 2.0"); //4.0
						%obj = Projectile::spawnProjectile("Smokegrenade", %trans, %player, %vel); //TankShockShell
						Projectile::spawnProjectile(%obj);
	  }
//	messageall(1, "Beacon Beacon Beacon.");
}

function ArenaBotBeaconTitan(%aiName)
{
	%id = AI::GetID(%aiName);

    if(!$Actions::Beacons[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
   		%client = Player::GetClient(%id);			
		%curTarget = ai::getTarget( %aiName );

	   if(%curTarget == -1)
	   {
  	 	return;
	   }

	   dbecho(1, %aiName @ " target: " @ %curTarget);
	   %targLoc = GameBase::getPosition(Client::getOwnedObject(%curTarget));
	   %aiLoc = GameBase::getPosition(Client::getOwnedObject(%id));
	   %targetDist = Vector::getDistance(%aiLoc, %targLoc);

 	  if(%targetDist < 150 && %targetDist > 25)
 	  {
				%player = Client::getOwnedObject(%id);
				GameBase::playSound(%player,ForceFieldOpen,0);
				%player.shieldStrength = 0.006;
	  }
//	messageall(1, "Beacon Beacon Beacon.");
}

function ArenaBotBeaconWarrior(%aiName)
{
	%id = AI::GetID(%aiName);

    if(!$Actions::Beacons[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
	
			
   		%client = Player::GetClient(%id);			
		%curTarget = ai::getTarget( %aiName );

	   if(%curTarget == -1)
	   {
  	 	return;
	   }
	   
				
					%vel = Item::getVelocity(%player);

						if(%vel == "0 0 0")
						{
							%trans = GameBase::getMuzzleTransform(%player);
							%smack = 300/25;
							%rot=GameBase::getRotation(%player);
							%len = 30;
							%tr= getWord(%trans,5);
							if(%tr <=0 )%tr -=%tr;
							%up = %tr+0.15;
							%out = 1-%tr;
							%vec = Vector::getFromRot(%rot,%len*%out*%smack,%len*%up*%smack);
							if ( ( getWord(%vec,0) >= 0 || getWord(%vec,0) < 0 ) && ( getWord(%vec,1) >= 0 || getWord(%vec,1) < 0 ) && ( getWord(%vec,2) >= 0 || getWord(%vec,2) < 0 ) )
								Player::applyImpulse(%player, getWord(%vel,0)+getWord(%vec,0)@" "@getWord(%vel,1)+getWord(%vec,1)@" "@getWord(%vel,2)+getWord(%vec,2) );
							else
							{
								admin::message("ERROR: Booster Vectoring error!  Vec=\""@%vec@"\" from person "@GameBase::getName(%client)@" ("@%player@")");
								$Admin = "ERROR: Booster Vectoring error!  Vec=\""@%vec@"\" from person "@GameBase::getName(%client)@" ("@%player@")";
								export("Admin", "config\\Error.log", true);
								return;
							}
							GameBase::playSound(%player, SoundFireMortar, 0);
							Client::sendMessage(%client,0, "Speed Boost Initiated..");	
						}
						else
						{	
							%vel = Vector::Normalize(%vel);
							%vec = GetWord(%vel, 0) * 300 @ " " @ GetWord(%vel, 1) * 300 @ " " @ GetWord(%vel, 2) * 300;
							if ( ( getWord(%vec,0) >= 0 || getWord(%vec,0) < 0 ) && ( getWord(%vec,1) >= 0 || getWord(%vec,1) < 0 ) && ( getWord(%vec,2) >= 0 || getWord(%vec,2) < 0 ) )
								Player::applyImpulse(%player, %vec );
							else
							{
								admin::message("ERROR: Booster Vectoring error!  Vec=\""@%vec@"\" from person "@GameBase::getName(%client)@" ("@%player@")");
								$Admin = "ERROR: Booster Vectoring error!  Vec=\""@%vec@"\" from person "@GameBase::getName(%client)@" ("@%player@")";
								export("Admin", "config\\Error.log", true);
								return;
							}
							GameBase::playSound(%player, SoundFireMortar, 0);
							Client::sendMessage(%client,1, "Speed boost initiated. ~wmortar_fire.wav");	
						}
//	messageall(1, "Beacon Beacon Beacon.");
}

function ArenaBotBeaconChameleon(%aiName)
{
	%id = AI::GetID(%aiName);

    if(!$Actions::Beacons[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
   		%client = Player::GetClient(%id);			
		%curTarget = ai::getTarget( %aiName );

	   if(%curTarget == -1)
	   {
  	 	return;
	   }
	   
	GameBase::playSound(%player,ForceFieldOpen,0);
	GameBase::startFadeout(%player);
	%rate = Player::getSensorSupression(%player) + 3;
	Player::setSensorSupression(%player,%rate);
	if($cloakTime[%id] == 0) 
	{	
		$cloakTime[%id] = 15; //12
		checkBotCloak(%id);
		return;
	}
	else 
		$cloakTime[%id] = 15; //12
		checkBotCloak(%id);
//	messageall(1, "Beacon Beacon Beacon.");
}

function ArenaBotBeaconNecromancer(%aiName)
{
	%id = AI::GetID(%aiName);

    if(!$Actions::Beacons[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
   		%client = Player::GetClient(%id);			
		%curTarget = ai::getTarget( %aiName );

	   if(%curTarget == -1)
	   {
  	 	return;
	   }
	   
	GameBase::playSound(%player,ForceFieldOpen,0);
	GameBase::startFadeout(%player);
	%rate = Player::getSensorSupression(%player) + 3;
	Player::setSensorSupression(%player,%rate);
	if($cloakTime[%id] == 0) 
	{	
		$cloakTime[%id] = 15; //12
		checkBotCloak(%id);
		return;
	}
	else 
		$cloakTime[%id] = 15; //12
		checkBotCloak(%id);
//	messageall(1, "Beacon Beacon Beacon.");
}

function ArenaBotBeaconAngel(%aiName)
{
	%id = AI::GetID(%aiName);

    if(!$Actions::Beacons[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

    if(%loc == "0 0 0")
    {
	return;
    }
   		%client = Player::GetClient(%id);			
		%curTarget = ai::getTarget( %aiName );

	   if(%curTarget == -1)
	   {
  	 	return;
	   }

	   dbecho(1, %aiName @ " target: " @ %curTarget);
	   %targLoc = GameBase::getPosition(Client::getOwnedObject(%curTarget));
	   %aiLoc = GameBase::getPosition(Client::getOwnedObject(%id));
	   %targetDist = Vector::getDistance(%aiLoc, %targLoc);

	  	if(%targetDist < 10)
		  {
			GameBase::playSound(%player, debrisMediumExplosion, 0);
			%vel = Item::getVelocity(%player);
			%trans = "0 0 1 0 0 1 0 0 1 " @ vector::add(getBoxCenter(%player),"0 0 2.0"); //4.0
			%obj = Projectile::spawnProjectile("Smokegrenade", %trans, %player, %vel); //TankShockShell
			Projectile::spawnProjectile(%obj);
		  }
//	messageall(1, "Beacon Beacon Beacon.");
}

function checkBotCloak(%aiId, %player) 
{
	%player = Client::getOwnedObject(%aiId);
	if(!Player::isDead(%player) && $cloakTime[%aiId] > 0) 
	{
		$cloakTime[%aiId] -= 1.5; //changed from 2 to 1.5 -death666
		schedule("checkPlayerCloak(" @ %aiId @ ");",1.5,%player); //changed from 2 to 1.5 -death666
			if(Player::getItemCount(%aiId,Flag) > 0) //no cloaking with the flag.. -death666
				{
				$cloakTime[%aiId] = 0;
				}
	}
	else 
	{
		$cloakTime[%aiId] = 0;
		%player.beaconcooldown = true;
		%player.usedcloakbeacon = false;
		$PlayerCloaked[%aiId] = false;
		schedule(%player@".beaconcooldown = false;",3.0,%player);
		GameBase::startFadein(%player);
		%rate = Player::getSensorSupression(%player) - 5;
		Player::setSensorSupression(%player,0);
	}
}

function AnniBotSpeak(%aiName)
{

	%id = AI::GetID(%aiName);

    if(!$Actions::Speaking[%id]) 
    {
        return;
    }
	
	%player = Client::getOwnedObject(%id);
	%loc = gamebase::getposition(%player);

//	messageall(1, "Anni Bot Speak.");

    if(%loc == "0 0 0")
    {
	return;
    }
		%aiId = AI::GetID(%aiName);
		%switch = floor(getRandom() * 35);
		if(%switch == 0)
		{
			playSound(AnniHelp, GameBase::getPosition(%aiId));	
		}
		else if(%switch == 1)
		{
			playSound(AnniTarget, GameBase::getPosition(%aiId));
		}
		else if(%switch == 2)
		{
			playSound(AnniOops, GameBase::getPosition(%aiId));
		}
		else if(%switch == 3)
		{
			playSound(AnniYoohoo, GameBase::getPosition(%aiId));
		}
		else if(%switch == 4)
		{
			playSound(AnniGetsome, GameBase::getPosition(%aiId));
		}
		else if(%switch == 5)
		{
			playSound(AnniHi, GameBase::getPosition(%aiId));
		}
				else if(%switch == 6)
		{
			playSound(SoundKillstreak, GameBase::getPosition(%aiId));
		}
				else if(%switch == 7)
		{
			playSound(FAaargh, GameBase::getPosition(%aiId));
		}
				else if(%switch == 8)
		{
			playSound(MAaargh, GameBase::getPosition(%aiId));
		}
				else if(%switch == 9)
		{
			playSound(FWooho, GameBase::getPosition(%aiId));
		}
				else if(%switch == 10)
		{
			playSound(MWooho, GameBase::getPosition(%aiId));
		}
				else if(%switch == 11)
		{
			playSound(Fhitdeck, GameBase::getPosition(%aiId));
		}
				else if(%switch == 12)
		{
			playSound(MdepBcn, GameBase::getPosition(%aiId));
		}
				else if(%switch == 13)
		{
			playSound(F2Acknowledge, GameBase::getPosition(%aiId));
		}
				else if(%switch == 14)
		{
			playSound(F3Acknowledge, GameBase::getPosition(%aiId));
		}
				else if(%switch == 15)
		{
			playSound(M4Acknowledge, GameBase::getPosition(%aiId));
		}
				else if(%switch == 16)
		{
			playSound(M1Acknowledge, GameBase::getPosition(%aiId));
		}
				else if(%switch == 17)
		{
			playSound(M5MoveOut, GameBase::getPosition(%aiId));
		}
				else if(%switch == 18)
		{
			playSound(M4BeingAttack, GameBase::getPosition(%aiId));
		}
				else if(%switch == 19)
		{
			playSound(F3HitDeck, GameBase::getPosition(%aiId));
		}
				else if(%switch == 21)
		{
			playSound(M1TakeCover, GameBase::getPosition(%aiId));
		}
				else if(%switch == 22)
		{
			playSound(f5IncomingEnemies, GameBase::getPosition(%aiId));
		}
				else if(%switch == 23)
		{
			playSound(M4FireOnTarget, GameBase::getPosition(%aiId));
		}
				else if(%switch == 24)
		{
			playSound(F5CoverMe, GameBase::getPosition(%aiId));
		}
				else if(%switch == 25)
		{
			playSound(F2CeaseFire, GameBase::getPosition(%aiId));
		}
				else if(%switch == 26)
		{
			playSound(F5Cheer1, GameBase::getPosition(%aiId));
		}
				else if(%switch == 27)
		{
			playSound(M4Cheer1, GameBase::getPosition(%aiId));
		}
				else if(%switch == 28)
		{
			playSound(M4Cheer2, GameBase::getPosition(%aiId));
		}
				else if(%switch == 29)
		{
			playSound(F3Cheer3, GameBase::getPosition(%aiId));
		}
				else if(%switch == 30)
		{
			playSound(M4Taunt3, GameBase::getPosition(%aiId));
		}
				else if(%switch == 31)
		{
			playSound(F3Taunt1, GameBase::getPosition(%aiId));
		}
				else if(%switch == 32)
		{
			playSound(M3ArgDying, GameBase::getPosition(%aiId));
		}
				else if(%switch == 33)
		{
			playSound(M3Help, GameBase::getPosition(%aiId));
		}
				else if(%switch == 34)
		{
			playSound(F1AhCrap, GameBase::getPosition(%aiId));
		}
}

function ixApplyKickback(%player, %strength, %lift) 
{
	%loc = gamebase::getposition(%player);
	
	if(%loc == "0 0 0")
    {
	return;
    }
	
	if((!%lift) && (%lift != 0))
		%lift = 0;

	%rot = GameBase::getRotation(%player);
	%rad = getWord(%rot, 2);
	%x = (-1) * (ixSin(%rad));
	%y = ixCos(%rad);
	%dir = %x @ " " @ %y @ " 0";
	%force = ixDotProd(Vector::neg(%dir),%strength);
	%x = getWord(%force, 0);
	%y = getWord(%force, 1);
	%dir = %x @ " " @ %y @ " " @ %lift;
	Player::applyImpulse(%player,%force);
}

function ixDotProd(%vec, %scalar)
{
	%return = Vector::dot(%vec,%scalar @ " 0 0") @ " " @ Vector::dot(%vec,"0 " @ %scalar @ " 0") @ " " @ Vector::dot(%vec,"0 0 " @ %scalar);
	return %return;
}

function ixSin(%theta) 
{
	return (%theta - (pow(%theta,3)/6) + (pow(%theta,5)/120) - (pow(%theta,7)/5040) + (pow(%theta,9)/362880) - (pow(%theta,11)/39916800));
}

function ixCos(%theta) 
{
	return (1 - (pow(%theta,2)/2) + (pow(%theta,4)/24) - (pow(%theta,6)/720) + (pow(%theta,8)/40320) - (pow(%theta,10)/3628800));
}

