using Weather.Models.Entities.Favorite;

namespace Weather.Models.Configuration
{
	public class WeatherOptions
	{
		public int FavoriteMaxLimit { get; set; } = 5;

		public int ForcastMaxLimit { get; set; } = 5;
		
		public GeoCity? DefaultGeoCity { get; set; }
	}

}
 