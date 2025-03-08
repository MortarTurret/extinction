//--- export object begin ---//
instant SimGroup "elevator6x5" {
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
	instant Moveable "elevator_6x51" {
		dataBlock = "elevator6x5";
		name = "";
		position = "0 0 0";
		rotation = "0 0 0";
		destroyable = "True";
		deleteOnDestroy = "False";
		Status = "up";
		stopWayDown = "1";
		stopWayUp = "1";
	};
};
//--- export object end ---//
