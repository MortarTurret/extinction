
StaticShapeData SkyWriteShape
{
        shapeFile = "flagstand";
        debrisId = defaultDebrisLarge;
        maxDamage = 50.0;
        visibleToSensor = true;
        isTranslucent = true;
        description = "Skywriter!";
};

function SkyWriteShape::onDamage(%this,%type,%value,%pos,%vec,%mom,%object)
{

}

function SkyWriteShape::onAdd (%this)
{
        if($rbCurObstacle == "")
            $rbcurobstacle = 0;
        $rbObstacle[$rbCurObstacle] = %this;
        $rbCurObstacle++;
}

function SkyWriteShape::onDestroyed(%this)
{
	StaticShape::onDestroyed(%this);
}

function String::len(%string)
{
	for(%i=0; String::getSubStr(%string, %i, 1) != ""; %i++)
		%result++;
	return %result;
}

function skywriteA(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
        %offset[0] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
        %offset[1] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
        %offset[2] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
        %offset[3] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
        %offset[4] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
        %offset[5] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
        %offset[6] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
        %offset[7] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
        %offset[8] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
        %offset[9] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
        %offset[10] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
        %offset[11] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 12; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }

}

function skywriteB(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[3] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[5] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[7] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[8] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[9] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[10] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[11] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 12; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }

}

function skywriteC(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[3] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[6] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[7] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[8] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 9; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }

}

function skywriteD(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[3] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[5] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[7] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[8] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[9] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[10] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[11] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 12; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }

}

function skywriteE(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[3] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[7] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[8] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[9] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[10] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 11; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }

}

function skywriteF(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[3] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[7] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[8] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 9; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }

}

function skywriteG(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[3] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[7] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[8] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[9] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[10] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[11] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 12; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }

}

function skywriteH(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[3] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[7] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[8] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[9] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[10] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[11] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 12; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }

}

function skywriteI(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[3] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[6] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[7] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[8] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 9; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }

}

function skywriteJ(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[3] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[6] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[7] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 8; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }

}

function skywriteK(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[3] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[7] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[8] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[9] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 10; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }

}

function skywriteL(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[2] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[3] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[4] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[5] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[6] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[7] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 8; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }

}

function skywriteM(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[3] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[5] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[6] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[7] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[8] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[9] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[10] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[11] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[12] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[13] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 14; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywriteN(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[3] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[5] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[7] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[8] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[9] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[10] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[11] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 12; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywriteO(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[3] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[5] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[7] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[8] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[9] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[10] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[11] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 12; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywriteP(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[3] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[5] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[7] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[8] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[9] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 10; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywriteQ(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[3] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[5] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[7] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[8] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[9] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[10] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[11] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[12] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
%offset[13] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 14; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywriteR(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[3] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[7] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[8] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[9] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 10; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywriteS(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[3] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[4] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[5] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[6] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 7; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywriteT(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[3] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[4] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[5] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[6] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[7] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[8] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 9; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywriteU(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[3] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[7] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[8] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[9] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 10; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywriteV(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[3] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[7] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[8] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[9] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 10; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywriteW(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
 %offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[3] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[5] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[7] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[8] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[9] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[10] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[11] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 12; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywriteX(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[3] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[6] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[7] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[8] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 9; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywriteY(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[3] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[6] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 7; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywriteZ(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[3] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[4] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[5] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[7] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[8] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[9] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[10] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[11] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 12; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywrite1(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[2] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[3] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[4] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[5] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[6] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
%offset[7] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
%offset[8] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 9; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywrite2(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[3] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[4] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[5] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[6] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
%offset[7] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
%offset[8] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 9; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywrite3(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[3] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[4] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[6] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[7] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
%offset[8] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 9; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywrite4(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[2] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[3] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[4] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[6] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[7] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[8] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[9] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[10] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 11; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}
function skywrite5(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[3] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[7] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[8] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
%offset[9] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 10; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}
function skywrite6(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[3] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[4] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[7] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[8] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[9] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[10] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
%offset[11] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 12; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}
function skywrite7(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[3] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[4] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[5] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[7] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[8] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 9; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}
function skywrite8(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[3] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[7] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[8] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[9] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[10] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
%offset[11] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 12; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}
function skywrite9(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[3] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[7] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[8] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[9] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[10] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
%offset[11] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 12; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}
function skywrite0(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[3] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[6] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[7] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[8] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[9] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[10] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
%offset[11] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 12; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywriteDash(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 0*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[1] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[2] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 3; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywriteQmark(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[2] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[3] = " " @ 4*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[4] = " " @ 3*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[5] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[6] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 7; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywriteExclaim(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 64*%spacing @ " ";
%offset[1] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 63*%spacing @ " ";
%offset[2] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 62*%spacing @ " ";
%offset[3] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[4] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 5; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywriteComma(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
%offset[0] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 61*%spacing @ " ";
%offset[1] = " " @ 2*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
%offset[2] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 59*%spacing @ " ";
    //End Letter Code

	 for(%i = 0; %i < 3; %i++)
	 {
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
	 }
}

function skywritePeriod(%this, %space)
{
 %position = Vector::add(gamebase::getposition(%this), %space @ " 0 20");
	%spacing = 1.7;  //Space between tiles, not space between letters
    //Begin Letter Code
       %offset[0] = " " @ 1*%spacing @ " " @ 0.0 @ " " @ 60*%spacing @ " ";
    //End Letter Code

        %i = 0;
    	%mgpole = newObject("Skywriter","StaticShape",SkywriteShape,true);
    	addToSet("MissionCleanup", %mgpole);
    	GameBase::setTeam(%mgpole,GameBase::getTeam(%player));
    	GameBase::setPosition(%mgpole,Vector::add(%position, %offset[%i]));
    	GameBase::setRotation(%mgpole,"-1.57 3.14 -1.57");
    	Gamebase::setMapName(%mgpole,"Skywriter!");
}