//--- export object begin ---//
instant SimGroup "elevator6x6Octa" {
	instant SimPath "Path1" {
		isLooping = "False";
		isCompressed = "False";
		instant Marker "Marker1" {
			dataBlock = "PathMarker";
			name = "";
			position = "0 0 -0.75";
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
	instant Moveable "elevator_6x6_octagon1" {
		dataBlock = "elevator6x6Octa";
		name = "";
		position = "0 0 -0.75";
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
