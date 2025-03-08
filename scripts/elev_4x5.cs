//--- export object begin ---//
instant SimGroup "elevator4x5" {
	instant SimPath "Path1" {
		isLooping = "False";
		isCompressed = "False";
		instant Marker "Marker1" {
			dataBlock = "PathMarker";
			name = "";
			position = "0 0 0";
			rotation = "0 0 0";
		};
		instant Marker "Marker1" {
			dataBlock = "PathMarker";
			name = "";
			position = "0 0 6";
			rotation = "0 0 0";
		};
		instant Marker "Marker1" {
			dataBlock = "PathMarker";
			name = "";
			position = "0 0 15";
			rotation = "0 0 0";
		};
	};
	instant Moveable "elevator_4x51" {
		dataBlock = "elevator4x5";
		name = "";
		position = "0 0 0";
		rotation = "0 0 0";
		destroyable = "True";
		deleteOnDestroy = "False";
		waypoint = "top";
		delayTime = "446.586";
		Status = "up";
		stopWayDown = "1";
		stopWayUp = "1";
	};
};
//--- export object end ---//
