//--- export object begin ---//
instant SimGroup "elevator16x16" {
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
	instant Moveable "elevator_16x16_octagon1" {
		dataBlock = "elevator16x16Octa";
		name = "";
		position = "0 0 0";
		rotation = "0 0 0";
		destroyable = "True";
		deleteOnDestroy = "False";
		waypoint = "top";
		delayTime = "1989.13";
		Status = "up";
		stopWayDown = "1";
		stopWayUp = "1";
	};
};
//--- export object end ---//
