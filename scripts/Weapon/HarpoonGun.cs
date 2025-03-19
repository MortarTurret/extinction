$InvList[Harpoon] = 1;
$MobileInvList[Harpoon] = 1;
$RemoteInvList[Harpoon] = 1;

$InvList[HarpoonAmmo] = 1;
$MobileInvList[HarpoonAmmo] = 1;
$RemoteInvList[HarpoonAmmo] = 1;

$AutoUse[Harpoon] = false;
$SellAmmo[HarpoonAmmo] = 10;
$WeaponAmmo[Harpoon] = HarpoonAmmo;

addWeapon(Harpoon);
addAmmo(Harpoon, HarpoonAmmo, 2);


ItemData HarpoonAmmo
{
   description = "Harpoons";
   className = "Ammo";
   shapeFile = "mortarammo";
   heading = $InvHead[ihAmm];
   shadowDetailMask = 4;
   price = 1;
};

ItemImageData HarpoonImage
{
   shapeFile = "grenadeL";
   mountPoint = 0;

   weaponType = 0; // Single Shot
   projectileType = ShotHarpoon;
   ammoType = HarpoonAmmo;
   accuFire = True;
   reloadTime = 0.25;
   fireTime = 1.25;
   
   lightType = 3; // Weapon Fire
   lightRadius = 5;
   lightTime = 2;
   lightColor = { 0, 0, 1 };

   sfxFire = debrisMediumExplosion;
   sfxActivate = SoundPickUpWeapon;
   sfxReload = SoundMortarReload;
};

ItemData Harpoon
{
   description = "Heaven's Harpoon";
   className = "Weapon";
   shapeFile = "grenadeL";
   hudIcon = "plasma";
   heading = $InvHead[ihWea];
   shadowDetailMask = 4;
   imageType = HarpoonImage;
   price = 250;
   showWeaponBar = true;
};

ItemImageData HarpoonScopeImage
{
   shapeFile  = "discammo";
   mountPoint = 0;
   mountRotation = { 1.5, 0, 0 };
   weaponType = 0; // Single Shot
   mountOffset = { -0.0, 0.0, -0.1 };

   ammoType = HarpoonAmmo;
   reloadTime = 0.1;
   fireTime = 0.1;

   projectileType = ShotHarpoon; //RailRound
   accuFire = false;

   sfxFire = turretexplosion;
   sfxActivate = SoundPickUpWeapon;
};

ItemData HarpoonScope
{
   heading = $InvHead[ihWea];
   description = "HarpoonScope";
   className = "Weapon";
   shapeFile  = "force";
   hudIcon = "blaster";
   shadowDetailMask = 4;
   imageType = HarpoonScopeImage;
   price = 50;
   showWeaponBar = true;
};

ItemImageData HarpoonClipImage
{
   shapeFile  = "grenadel";
   mountPoint = 0;
   mountRotation = {3.1, 0, 0 };
   weaponType = 0; // Single Shot
   mountOffset = { -0.0, -0.0, 0.0 };

   ammoType = HarpoonAmmo;
   reloadTime = 0.1;
   fireTime = 0.1;

   projectileType = ShotHarpoon; // RailRound
   accuFire = false;

   sfxFire = turretexplosion;
   sfxActivate = SoundPickUpWeapon;
};

ItemData HarpoonClip
{
   heading = $InvHead[ihWea];
   description = "HarpoonClip";
   className = "Weapon";
   shapeFile  = "grenadel";
   hudIcon = "blaster";
   shadowDetailMask = 4;
   imageType = HarpoonClipImage;
   price = 50;
   showWeaponBar = true;
};

function Harpoon::onMount(%player,%item)
{
   Player::MountItem(%player,HarpoonScope,7);
   Player::MountItem(%player,HarpoonClip,6);
   if((Player::getclient(%player)).weaponHelp && $TALT::Active == false && !(Player::getclient(%player)).isBlackOut)
   Bottomprint(Player::getclient(%player), "<jc><f2>Heavens Harpoon: Take a bite out of reality.");
}
function Harpoon::onUnMount(%player,%item)
{
   Player::UnMountItem(%player,7);
   Player::UnMountItem(%player,6);
}

function HarpoonImage::onFire(%player,%slot)
{		
	if($debug)
		Anni::Echo("?? EVENT fire "@Player::getMountedItem(%player,0)@ " player "@ %player @" cl# "@ Player::getclient(%player));		

	%AmmoCount = Player::getItemCount(%player, $WeaponAmmo[Harpoon]);
	if(%AmmoCount)
	{	
		%client = GameBase::getOwnerClient(%player);
		%clientName = Player::getClient(%player);
		%clientId = Client::getName(%client);
		%trans = GameBase::getMuzzleTransform(%player);
		%vel = Item::getVelocity(%player);
		Projectile::spawnProjectile("ShotHarpoon",%trans,%player,%vel,$los::object);		
		Player::decItemCount(%player,$WeaponAmmo[Harpoon],1);
	}
	else 
		Client::sendMessage(Player::getClient(%player),0,"Harpoon out of ammo.~waccess_denied.wav");
}

function ShotHarpoon::onAdd(%this)
{
    %this.isAlive = true;
    schedule("trackHarpoon(" @ %this @ ");", 0.1);
}

function ShotHarpoon::onremove(%this)
{
    %this.isAlive = false;
}

function trackHarpoon(%this)
{
    if(%this.isAlive == true)
    {
       if($harpoonPos[%this] == gamebase::getposition(%this))
       {
        %this.isAlive = false;

               %camera = newObject("harpoon","StaticShape",HarpoonStill,true);
               addToSet("MissionCleanup", %camera);
               %myrot = $harpoonRot[%this];
                    %mypos = $harpoonPos[%this];

               GameBase::setRotation(%camera, %myrot);
               GameBase::setPosition(%camera,%mypos);

                 schedule("GameBase::setposition(" @ %camera @ ",'0 0 -1000');", 20);
                    schedule("removeharpoon(" @ %camera @ ");",25);
        return;
       }
       $harpoonPos[%this] = gamebase::getposition(%this);
       $harpoonRot[%this] = gamebase::getrotation(%this);
       schedule("trackHarpoon(" @ %this @ ");", 0.1);
    }
}

function removeharpoon(%this)
{
if(%this.isharpoon == true)
     deleteObject(%this);
}