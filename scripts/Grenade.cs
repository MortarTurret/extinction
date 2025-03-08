$InvList[Grenade] = 1;
$MobileInvList[Grenade] = 1;
$RemoteInvList[Grenade] = 1;
AddItem(Grenade);

$SellAmmo[Grenade] = 5;

addAmmo("", Grenade, 2);

ItemData Grenade 
{
	description = "Grenade";
	shapeFile = "grenade";
	heading = $InvHead[ihMis];
	shadowDetailMask = 4;
	price = 5;
	className = "HandAmmo";
};

function Grenade::onUse(%player,%item) 
{
	if($matchStarted && %player.throwTime < getSimTime()) 
	{
		if(player::getitemcount(%player,%item) > 0)
			Player::GetClient(%player).TGrenadesThrown++;
		
		if(!%player.inArenaTD)
			%player.invulnerable = "";
		GameBase::playSound(%player, SoundThrowItem,0);
		if(!$build || !$vxmod && !Player::GetClient(%player).isGoated)Player::decItemCount(%player,%item); //idkkkkk
		%armor = Player::getArmor(%player);
		eval(%armor @ "::onGrenade(" @ %player @ ");");
	}
}

