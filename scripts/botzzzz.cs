// function TA::CubeBotA()
// {
//	//%player = Client::getOwnedObject(%clientId);
//	//if(%player != -1 && getObjectType(%player) == "Player" && !Player::isDead(%player))
//	if($TA::CubeBot)
//	{
//		%name = "Duck"@$numCubeAI++;
//		%pos = "110.299 -107.83 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ); 
//		player::applyImpulse(%aiId,"0 1000 500");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kil(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "51.7999 -120.43 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ); 
//		player::applyImpulse(%aiId,"0 1000 500");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kil(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-128.01 88.4899 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ); 
//		player::applyImpulse(%aiId,"0 1000 500");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kil(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-84.42 106.879 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ); 
//		player::applyImpulse(%aiId,"0 1000 500");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kil(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "3.00988 100.469 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ); 
//		player::applyImpulse(%aiId,"0 1000 500");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kil(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "45.7799 104.959 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ); 
//		player::applyImpulse(%aiId,"0 1000 500");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kil(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "94.7899 28.6599 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ); 
//		player::applyImpulse(%aiId,"0 1000 500");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kil(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "67.9199 -50.3701 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ); 
//		player::applyImpulse(%aiId,"0 1000 500");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kil(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-1.3601 -10.2301 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ); 
//		player::applyImpulse(%aiId,"0 1000 500");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kil(" @ %aiId @ ");", 6);
//		
//		schedule("TA::CubeBotA();", 7);
//	}
// }
//
// function TA::CubeBotB()
// {
//	if($TA::CubeBot)
//	{
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-2.77001 14 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ); 
//		player::applyImpulse(%aiId,"0 1000 500");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kil(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-11.83 35.3798 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ); 
//		player::applyImpulse(%aiId,"0 1000 500");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kil(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-41.08 34.0399 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ); 
//		player::applyImpulse(%aiId,"0 1000 500");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kil(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-71.93 -13.88 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ); 
//		player::applyImpulse(%aiId,"0 1000 500");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kil(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-72.88 -57.77 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ); 
//		player::applyImpulse(%aiId,"0 1000 500");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kil(" @ %aiId @ ");", 6);
//		
//		schedule("TA::CubeBotB();", 7);
//	}
// }
//
// function TA::CubeBotC()
// {
//	if($TA::CubeBot)
//	{
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-7.80004 -120.33 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ); 
//		player::applyImpulse(%aiId,"0 1000 500");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kil(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-51.29 -116.67 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ); 
//		player::applyImpulse(%aiId,"0 1000 500");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kil(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-53.1201 -74.93 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ); 
//		player::applyImpulse(%aiId,"0 1000 500");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kil(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-101.68 -90.51 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ); 
//		player::applyImpulse(%aiId,"0 1000 500");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kil(" @ %aiId @ ");", 6);
//		
//		%name = "Duck"@$numCubeAI++;
//		%pos = "-116.19 -29.89 6"; 
//		if(AI::spawn(%name, "armormWarrior", GetOffsetRot(%pos,"0 0 0",$Arena::Spawn), "0 0 0"@getRandom()*$Pi) != "false")
//			%aiId = Client::getOwnedObject(AI::getID(%name));
//		%rotZ = getWord(GameBase::getRotation(%aiId),2); GameBase::setRotation(%aiId, "0 0 " @ %rotZ); 
//		player::applyImpulse(%aiId,"0 1000 500");
//		schedule("Player::blowUp(" @ %aiId @ ");", 6); schedule("Player::Kil(" @ %aiId @ ");", 6);
//		
//		schedule("TA::CubeBotC();", 7);
//	}
// }