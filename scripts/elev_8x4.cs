//--- export object begin ---//
instant SimGroup "elevator8x4" {
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
	instant Moveable "elevator_8x41" {
		dataBlock = "elevator8x4";
		name = "";
		position = "0 0 -0.5";
		rotation = "0 0 0";
		destroyable = "True";
		deleteOnDestroy = "False";
		waypoint = "top";
		delayTime = "2046.8";
		Status = "up";
		stopWayDown = "1";
		stopWayUp = "1";
	};
};
//--- export object end ---//
