// registervol volumename
function registervolume( %VolName )
{
	if( %VolName == "")
	{
		Anni::Echo("registervolume volumename");
	}
	else
	{
		if( focusServer )
		{
			%Name = strcat("MissionGroup\\Volumes\\", %VolName);
			if( isObject( %Name) )
			{
			//		
			}
			else
			{
				%Volume = strcat(%VolName, ".vol");
				newObject( %VolName, SimVolume, %Volume );
				addToSet( "MissionGroup\\Volumes", %VolName);
			}
			focusClient();
		}
	}
}