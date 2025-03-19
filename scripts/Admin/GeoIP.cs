function GeoIP::OnConnect(%clientId, %ip)
{
	%mask = "192.168.*.*";
	if(TA::CompareIP(%ip, %mask))// == true)
	{
		%clientId.country = "The";
		%clientId.state = "Owner";
		%clientId.city = "Server";
		return;
	}
	%ip = Client::getTransportAddress(%clientId);
	%ip = MySql::ParseIP(%ip); //used mysql plugin dependency here
 
	GeoIp::Lookup(%ip); //magic line
	GeoIP::print(%clientId);
}

function GeoIP::print(%clientId)
{
	//echo( $GeoIP[country] );
	%clientId.country = $GeoIP[country];
	//echo( $GeoIP[region] );
	//echo( $GeoIP[regionname] );
	%clientId.state = $GeoIP[regionname];
	//echo( $GeoIP[city] );
	%clientId.city = $GeoIP[city];
	//echo( $GeoIP[latitude] );
	//echo( $GeoIP[longitude] );
	//echo( $GeoIP[metrocode] );
	//echo( $GeoIP[areacode] );
	//echo( $GeoIP[timezone] );
	//echo( $GeoIP[1] );
	//echo( $GeoIP[2] );
	//echo( $GeoIP[ASN] );
}
