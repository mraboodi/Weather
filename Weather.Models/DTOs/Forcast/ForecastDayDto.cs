using Weather.Models.Entities;

namespace Weather.Models.DTOs.Forcast
{
	public record ForecastDayDto(
		DateTime DateTime,
		double MaxTemp,
		double MinTemp,
		int WeatherCode,
        double RainSum
	);
}
