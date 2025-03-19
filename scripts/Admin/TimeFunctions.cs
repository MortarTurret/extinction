

function uptime()
{
	DebugFun("uptime");
	%secondsLive = getintegertime(true)>>5;
	%minutesLive = %secondsLive/60;
	%hoursLive = %secondslive/3600;
}	
function WhatTime()
{
	DebugFun("WhatTime");
	%days = 0;
	%hours = 0;
	%minutes = 0;
	%seconds = 0;
	%time = getintegertime(true)>>5;
	%minutes = floor(%time / 60);
	%seconds = %time % 60;

	while(%minutes > 60)
	{			
		%minutes = %minutes - 60;
		%hours++;
		if(%hours > 2400)
			break;
	}
	while(%hours > 24)
	{			
		%hours = %hours - 24;
		%days++;
		if(%days > 100)
			break;
	}
	%string = "";
	if ( %days > 0 )
		%string = ""@%days@" days, ";
	return("Server up time: "@%string@%hours@":"@%minutes@":"@%seconds);
}	

function Time::getMinutes(%simTime)
{
	DebugFun("Time::getMinutes",%simTime);
	return floor(%simTime / 60);
}

function Time::getSeconds(%simTime)
{
	DebugFun("Time::getSeconds",%simTime);
	return %simTime % 60;
}
	