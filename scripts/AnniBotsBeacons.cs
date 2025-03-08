function BotBeaconUse(%aiName)
{

		 %aiId = BotFuncs::GetId(%aiName);
   		 %client = Player::GetClient(%aiId);
		 %armor = player::getarmor(%client);
		 %armorlist = $ArmorName[%armor].description;
		 %player = Client::getOwnedObject(%aiId);
		 
	%loc = gamebase::getposition(%player);
    if(%loc == "0 0 0")
    {
	return;
    }
		 
		if((%armorlist == "Troll"))
			{
if(floor(getrandom() * 100) < 50 )
{
	   %id = AI::GetID(%AIName);
	   %curTarget = ai::getTarget( %aiName );

	   if(%curTarget == -1)
	   {
  	 	return;
	   }

	   dbecho(1, %aiName @ " target: " @ %curTarget);

	   %targLoc = GameBase::getPosition(Client::getOwnedObject(%curTarget));
	   %aiLoc = GameBase::getPosition(Client::getOwnedObject(%id));
	   %targetDist = Vector::getDistance(%aiLoc, %targLoc);

 	//  if(%targetDist < 150 && %targetDist > 25)
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
}
			}
	else if(%armorlist == "Tank")
	{
if(floor(getrandom() * 100) < 50 )
{
	   %id = AI::GetID(%AIName);
	   %curTarget = ai::getTarget( %aiName );

	   if(%curTarget == -1)
	   {
  	 	return;
	   }

	   dbecho(1, %aiName @ " target: " @ %curTarget);

	   %targLoc = GameBase::getPosition(Client::getOwnedObject(%curTarget));
	   %aiLoc = GameBase::getPosition(Client::getOwnedObject(%id));
	   %targetDist = Vector::getDistance(%aiLoc, %targLoc);

 	//  if(%targetDist < 150 && %targetDist > 25)
 	  if(%targetDist < 10)
 	  {
						GameBase::playSound(%player, debrisMediumExplosion, 0);
						%vel = Item::getVelocity(%player);
						%trans = "0 0 1 0 0 1 0 0 1 " @ vector::add(getBoxCenter(%player),"0 0 2.0"); //4.0
						%obj = Projectile::spawnProjectile("Smokegrenade", %trans, %player, %vel); //TankShockShell
						Projectile::spawnProjectile(%obj);
	  }
}
	}
	else if(%armorlist == "Titan")
	{
if(floor(getrandom() * 100) < 25 )
{
	   %id = AI::GetID(%AIName);
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
				%player = Client::getOwnedObject(%aiId);
				GameBase::playSound(%player,ForceFieldOpen,0);
				%player.shieldStrength = 0.006;
	  }
}
	}
	else if(%armorlist == "Warrior")
	{
		if(floor(getrandom() * 100) < 25 )
			{

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

			}
	}
	else if(%armorlist == "Chameleon Assassin")
	{
		if(floor(getrandom() * 100) < 5 )
			{
	%player = Client::getOwnedObject(%aiId);
	GameBase::playSound(%player,ForceFieldOpen,0);
	GameBase::startFadeout(%player);
	%rate = Player::getSensorSupression(%player) + 3;
	Player::setSensorSupression(%player,%rate);
			}
	}
	else if(%armorlist == "Necromancer")
	{
	// START
		if(floor(getrandom() * 100) < 5 )
			{
	%player = Client::getOwnedObject(%aiId);
	GameBase::playSound(%player,ForceFieldOpen,0);
	GameBase::startFadeout(%player);
	%rate = Player::getSensorSupression(%player) + 3;
	Player::setSensorSupression(%player,%rate);
			}
	}

}