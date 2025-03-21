function Client::cancelMenu(%clientId)
{
	if(!%clientId.menuLock)
	{
		%clientId.selClient = "";
		%clientId.menuMode = "";
		%clientId.menuLock = "";
		remoteEval(%clientId, "CancelMenu");
		Client::setMenuScoreVis(%clientId, false);
	}
}

function Client::buildMenu(%clientId, %menuTitle, %menuCode, %cancellable)
{
	Client::setMenuScoreVis(%clientId, true);
	%clientId.menuLock = !%cancellable;
	%clientId.menuMode = %menuCode;
	remoteEval(%clientId, "NewMenu", %menuTitle);
}

function Client::addMenuItem(%clientId, %option, %code)
{
	remoteEval(%clientId, "AddMenuItem", %option, %code);
}

function remoteCancelMenu(%server)
{
	if(%server != 2048)
		return;
	if(isObject(CurServerMenu))
		deleteObject(CurServerMenu);
}

function remoteNewMenu(%server, %title)
{
	if(%server != 2048)
		return;

	if(isObject(CurServerMenu))
		deleteObject(CurServerMenu);

	newObject(CurServerMenu, ChatMenu, %title);
	setCMMode(PlayChatMenu, 0);
	setCMMode(CurServerMenu, 1);
}

function remoteAddMenuItem(%server, %title, %code)
{
	if(%server != 2048)
		return;
	addCMCommand(CurServerMenu, %title, clientMenuSelect, %code);
}

function clientMenuSelect(%code)
{
	deleteObject(CurServerMenu);
	remoteEval(2048, menuSelect, %code);
}

function remoteMenuSelect(%clientId, %code)
{
	if( CheckEval("remoteMenuSelect", %clientId, %code) )
		return;
		
	if ( $debug )
		Anni::Echo("remoteMenuSelect("@%clientId@", "@Ann::Clean::string(%code));	

	%mm = %clientId.menuMode;
	if(%mm == "")
		return;
	if( String::findSubStr(%code, "\"") != -1 
         || String::findSubStr(%code, "\\") != -1 
         || String::findSubStr(%code, ";") != -1 
         || String::findSubStr(%code, "$") != -1 
         || String::findSubStr(%code, "(") != -1 
         || String::findSubStr(%code, ")") != -1 ) 
		return;

	%evalString = "processMenu" @ %mm @ "(" @ %clientId @ ", \"" @ %code @ "\");";
	%clientId.menuMode = "";
	%clientId.menuLock = "";
	eval(%evalString);
	if(%clientId.menuMode == "")
	{
		Client::setMenuScoreVis(%clientId, false);
		%clientId.selClient = "";
	}
}
