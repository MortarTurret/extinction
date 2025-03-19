//--- export object begin ---//
instant SimGroup "elevator6x6" {
	instant SimPath "Path1" {
		isLooping = "False";
		isCompressed = "False";
		instant Marker "Marker1" {
			dataBlock = "PathMarker";
			name = "";
			position = "0 0 -0.5";
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
	instant Moveable "elevator_6x61" {
		dataBlock = "elevator6x6";
		name = "";
		position = "0 0 -0.5";
		rotation = "0 0 0";
		destroyable = "True";
		deleteOnDestroy = "False";
		Status = "up";
		delayTime = "1750.51";
		waypoint = "top";
		stopWayDown = "1";
		stopWayUp = "1";
	};
};
//--- export object end ---//
