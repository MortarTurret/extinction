//--------------------------------------------------------------------------

//--------------------------------------------------------------------------


//--------------------------------------------------------------------------

StaticShapeData Example
{
	shapeFile = "radar";
	shadowDetailLevel = 0;
//	explosionId = 0;
	ambientSoundId = IDSFX_GENERATOR;
	maxDamage = 2.0;
};

function Example::onAdd(%this)
{
	//Anni::Echo("Example added: ", %this);
}

function Example::onRemove(%this)
{
	//Anni::Echo("Example removed: ", %this);
}

function Example::onEnabled(%this)
{
	//Anni::Echo("Example enabled");
}

function Example::onDisabled(%this)
{
	//Anni::Echo("Example disabled");
}

function Example::onDestroyed(%this)
{
	//Anni::Echo("Example destroyed");
}

function Example::onPower(%this, %newState, %generator)
{
	//Anni::Echo("Example power state: ", %newState);
}

function Example::onCollision(%this, %object)
{
	if($debug) 
		event::collision(%this,%object);


}

function Example::onAttack(%this, %object)
{
	//Anni::Echo("Example attacked ", %object);
}

