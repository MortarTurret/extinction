//------------------------------------------------------------------------

$Power::Range[Generator] = 400;
$Power::Range[PortGenerator] = 400;
$Power::Range[SolarPanel] = 400;

$Power::Range[PortableGenerator] = 300;
$Power::Range[PortableSolar] = 200;


$Power::Range[InventoryStation] = 175;
$Power::Range[CommandStation] = 150;


function Ann::PowerTap(%this)
{
	//Anni::Echo("Ann::PowerTap"@GameBase::getDamageState(%this));
	if(GameBase::getDamageState(%this) == "0" || GameBase::isPowered(%this))
		return;	
	Anni::Echo("Cycling power source for "@GameBase::getDataName(%this).description@" #"@%this);
	%team = GameBase::getteam(%this);
	%Pos = GameBase::getPosition(%this);
	for(%i = 0; %i < 128; %i++)
	{
		%obj = AntiCrash::getObjectByTargetIndex(%i);
		if ( %obj == -1 )
			break;
		%data = GameBase::getDataName(%obj);	
		//Anni::Echo(%data.className);
		%objteam = GameBase::getteam(%obj);	
		if((%objteam == %team || %objteam == -1) && GameBase::isPowered(%obj))
		{
			if((%data.className == Generator || %data == InventoryStation || %data == CommandStation))	//1/27/2005 4:50AM
			{
				%GenPos = GameBase::getPosition(%obj);
				//Anni::Echo("Found Gen "@%data);
				if(Vector::getDistance(%Pos,%GenPos) <= $Power::Range[%data])
				{
					%group = getGroup(%obj);
					addToSet(%group, %this);
					Anni::Echo("Found power, adding "@%this@" to group "@%group);
					%team = GameBase::getTeam(%this);
					DropshipTeamMessage(%team, 3, "Power found for "@GameBase::getDataName(%this).description@" #"@%this@".");	
					return;				
				}
			}
		}		
	}
	schedule("Ann::PowerTap("@%this@");",5,%this);				
}

function MAnn::PowerTap(%this)
{
	GameBase::generatePower(%this, true);
	GameBase::playSequence(%this,0,"power");
	GameBase::setActive(%this,true);
}

function Ann::Powering(%this,%power)
{
	if (!isObject(%this) || GameBase::getDamageState(%this) == "0")
		return;
		%team = GameBase::getTeam(%this);
		%data = GameBase::getDataName(%this);
		%Pos = GameBase::getPosition(%this);
			for(%i = 0; %i < 128; %i++)
			{
				%obj = AntiCrash::getObjectByTargetIndex(%i);
					if ( %obj == -1 )
					return;
				// %desc = GameBase::getDataName(%this).description;
				%objData = GameBase::getDataName(%obj);	
	%data = GameBase::getDataName(%obj);
	%desc = GameBase::getDataName(%obj).description;
	
//		if(%objdata.description != "TA Pulse Sensor")
//		{
//		if(%desc != "TA Pulse Sensor" && %desc != "Remote Laser Turret" && %desc != "Vortex Turret" && %desc != "Shock Turret" && %desc != "Nuclear Turret" && %desc != "Neuro Turret" && %desc != "Irradiation Cannon" && %desc != "Remote Ion Turret" && %desc != "Fusion Turret" && %desc != "Flame Turret" && %desc != "Flame Turret Fuel" && %desc != "Remote Mortar Turret" && %desc != "Neuro Basher" && %desc != "Remote Plasma Turret" && %desc != "Remote Missile Turret")
		if(%desc == "Station Supply Unit" || %desc == "Ammo Supply Unit" || %desc == "Vehicle Pad" || %desc == "Station Vehicle Unit" || %desc == "Command Station" || %desc == "Plasma Turret" || %desc == "Mortar Turret" || %desc == "Indoor Turret" || %desc == "Rocket Turret" || %desc == "ELF Turret" || %desc == "Large Pulse Sensor" || %desc == "Medium Pulse Sensor")
		{

				%class = %objData.className;
				%state = GameBase::getDamageState(%obj);	
				%Objteam = GameBase::getTeam(%obj);
				if(%class != "Generator")
				{ 
					if(%Objteam == %team)
						{ 
							GameBase::generatePower(%obj, true);
						//	Anni::Echo("Powering "@%objdata.description@" #"@%obj@" team "@%objTeam@" with "@%data.description);
						//	messageAll(0, "Powering "@%objdata.description@" #"@%obj@" team "@%objTeam@" with "@%data.description);
						}
				}	
			}
		}
}

function Ann::UndoPowering(%this,%power)
{
	if (!isObject(%this) || GameBase::getDamageState(%this) == "0")
		return;
		%team = GameBase::getTeam(%this);
		%data = GameBase::getDataName(%this);
		%Pos = GameBase::getPosition(%this);
			for(%i = 0; %i < 128; %i++)
			{
				%obj = AntiCrash::getObjectByTargetIndex(%i);
					if ( %obj == -1 )
					return;
				%objData = GameBase::getDataName(%obj);		
	%data = GameBase::getDataName(%obj);
	%desc = GameBase::getDataName(%obj).description;
	
//		if(%objdata.description != "TA Pulse Sensor")
//		{
//		if(%desc != "TA Pulse Sensor" && %desc != "Remote Laser Turret" && %desc != "Vortex Turret" && %desc != "Shock Turret" && %desc != "Nuclear Turret" && %desc != "Neuro Turret" && %desc != "Irradiation Cannon" && %desc != "Remote Ion Turret" && %desc != "Fusion Turret" && %desc != "Flame Turret" && %desc != "Flame Turret Fuel" && %desc != "Remote Mortar Turret" && %desc != "Neuro Basher" && %desc != "Remote Plasma Turret" && %desc != "Remote Missile Turret")
		if(%desc == "Station Supply Unit" || %desc == "Ammo Supply Unit" || %desc == "Vehicle Pad" || %desc == "Station Vehicle Unit" || %desc == "Command Station" || %desc == "Plasma Turret" || %desc == "Mortar Turret" || %desc == "Indoor Turret" || %desc == "Rocket Turret" || %desc == "ELF Turret" || %desc == "Large Pulse Sensor" || %desc == "Medium Pulse Sensor")
		{		
				%class = %objData.className;
				%state = GameBase::getDamageState(%obj);	
				%Objteam = GameBase::getTeam(%obj);
				if(%class != "Generator")
				{ 
					if(%Objteam == %team)
						{ 
							GameBase::generatePower(%obj, false);
							// Anni::Echo("Undoing Powering "@%objdata.description@" #"@%obj@" team "@%objTeam@" with "@%data.description);
							// messageAll(0, "Undoing Powering "@%objdata.description@" #"@%obj@" team "@%objTeam@" with "@%data.description);
						}
				}
			}
		}
}

function Ann::GenPowering(%this,%power)
{
	if (!isObject(%this) || GameBase::getDamageState(%this) == "0")
		return;
		%team = GameBase::getTeam(%this);
		%data = GameBase::getDataName(%this);
		%Pos = GameBase::getPosition(%this);
			for(%i = 0; %i < 128; %i++)
			{
				%obj = AntiCrash::getObjectByTargetIndex(%i);
					if ( %obj == -1 )
					return;

				%objData = GameBase::getDataName(%obj);		
	%data = GameBase::getDataName(%obj);
	%desc = GameBase::getDataName(%obj).description;
	
//		if(%objdata.description != "TA Pulse Sensor")
//		{
//		if(%desc != "TA Pulse Sensor" && %desc != "Remote Laser Turret" && %desc != "Vortex Turret" && %desc != "Shock Turret" && %desc != "Nuclear Turret" && %desc != "Neuro Turret" && %desc != "Irradiation Cannon" && %desc != "Remote Ion Turret" && %desc != "Fusion Turret" && %desc != "Flame Turret" && %desc != "Flame Turret Fuel" && %desc != "Remote Mortar Turret" && %desc != "Neuro Basher" && %desc != "Remote Plasma Turret" && %desc != "Remote Missile Turret")
		if(%desc == "Station Supply Unit" || %desc == "Ammo Supply Unit" || %desc == "Vehicle Pad" || %desc == "Station Vehicle Unit" || %desc == "Command Station" || %desc == "Plasma Turret" || %desc == "Mortar Turret" || %desc == "Indoor Turret" || %desc == "Rocket Turret" || %desc == "ELF Turret" || %desc == "Large Pulse Sensor" || %desc == "Medium Pulse Sensor")
		{		
				%class = %objData.className;
				%state = GameBase::getDamageState(%obj);	
				%Objteam = GameBase::getTeam(%obj);
				if(%class != "Generator")
				{ 
					if(%Objteam == %team)
						{ 
							GameBase::generatePower(%obj, true);
							// Anni::Echo("Powering "@%objdata.description@" #"@%obj@" team "@%objTeam@" with "@%data.description);
							// messageAll(0, "Powering "@%objdata.description@" #"@%obj@" team "@%objTeam@" with "@%data.description);
						}
				}
			}
		}
}

function Ann::GenUndoPowering(%this,%power)
{
	if (!isObject(%this) || GameBase::getDamageState(%this) == "0")
		return;
		%team = GameBase::getTeam(%this);
		%data = GameBase::getDataName(%this);
		%Pos = GameBase::getPosition(%this);
			for(%i = 0; %i < 128; %i++)
			{
				%obj = AntiCrash::getObjectByTargetIndex(%i);
					if ( %obj == -1 )
					return;
				%objData = GameBase::getDataName(%obj);		
	%data = GameBase::getDataName(%obj);
	%desc = GameBase::getDataName(%obj).description;
	
//		if(%objdata.description != "TA Pulse Sensor")
//		{
//		if(%desc != "TA Pulse Sensor" && %desc != "Remote Laser Turret" && %desc != "Vortex Turret" && %desc != "Shock Turret" && %desc != "Nuclear Turret" && %desc != "Neuro Turret" && %desc != "Irradiation Cannon" && %desc != "Remote Ion Turret" && %desc != "Fusion Turret" && %desc != "Flame Turret" && %desc != "Flame Turret Fuel" && %desc != "Remote Mortar Turret" && %desc != "Neuro Basher" && %desc != "Remote Plasma Turret" && %desc != "Remote Missile Turret")
		if(%desc == "Station Supply Unit" || %desc == "Ammo Supply Unit" || %desc == "Vehicle Pad" || %desc == "Station Vehicle Unit" || %desc == "Command Station" || %desc == "Plasma Turret" || %desc == "Mortar Turret" || %desc == "Indoor Turret" || %desc == "Rocket Turret" || %desc == "ELF Turret" || %desc == "Large Pulse Sensor" || %desc == "Medium Pulse Sensor")
		{	
				%class = %objData.className;
				%state = GameBase::getDamageState(%obj);	
				%Objteam = GameBase::getTeam(%obj);
				if(%class != "Generator")
				{ 
					if(%Objteam == %team)
						{ 
							GameBase::generatePower(%obj, false);
							// Anni::Echo("Undoing Powering "@%objdata.description@" #"@%obj@" team "@%objTeam@" with "@%data.description);
							// messageAll(0, "Undoing Powering "@%objdata.description@" #"@%obj@" team "@%objTeam@" with "@%data.description);
						}
				}
			}
		}
}