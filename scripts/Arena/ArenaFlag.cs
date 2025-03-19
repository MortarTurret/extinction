function ArenaFlag::onDrop(%player, %type)
{
	%playerTeam = GameBase::getTeam(%player);
	%playerClient = Player::getClient(%player);
	%dropClientName = Client::getName(%playerClient);
	Anni::Echo("!! arenaflag::ondrop, player="@%player@" flag="@%flag@" team="@getTeamName(%flagTeam)@" seq="@%flag.pickupSequence@" rotates? "@Item::isRotating(%flag));	
		
	ArenaMSG(0,%dropClientName @ " dropped the Arena Flag!");
	
	GameBase::throw(ArenaFlag, %player, 10, false);
	Player::setItemCount(%player, ArenaFlag, 0);
}

function HoldTheArenaFlag::checkPoints(%clientId) 
{
	if(!Player::isDead(%clientId)) 
	{
		if(Player::getItemCount(%clientId,ArenaFlag) > 0)
		{
			%clientId.score += 0.1;
			%clientId.Credits += 0.1;
			Game::refreshClientScore(%clientId);
			schedule("HoldTheArenaFlag::checkPoints(" @ %clientId @ ");", 5);
		}
	}
}

function ArenaFlag::onCollision(%this, %object)
{	
	if($debug) 
		event::collision(%this,%object);

	Anni::Echo("Flag collision ", %object," this ",%this);
	if(getObjectType(%object) != "Player")
		return;
	//if(Player::isAIControlled(%object))
	//	return;
	%name = Item::getItemData(%this);

	%playerTeam = GameBase::getTeam(%object);
	%flagTeam = GameBase::getTeam(%this);
	%playerClient = Player::getClient(%object);
	%playerClient.carryArenaFlag = true;
	//if(%playerClient.inArena && %name == "Arena Flag")
	//{
		ArenaMSG(0,Client::getName(%playerClient)@" has grabbed the Arena Flag.");
		Player::setItemCount(%object, ArenaFlag, 1);
		HoldTheArenaFlag::checkPoints(%playerClient);
		ArenaFlag::onUse(%playerClient,%name);
		deleteObject(%this);
	//}
}
