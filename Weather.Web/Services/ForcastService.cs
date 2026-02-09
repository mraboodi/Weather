using Weather.Models.DTOs.Forcast;
using Weather.Web.Interfaces;

namespace Weather.Web.Services
{
	public class ForcastService(HttpClient httpClient) : IForcastService
	{
		public async Task<ForecastResponseDto?> GetWeatherAsync(double latitude, double longitude)
		{
			try
			{
				var response = await httpClient.GetAsync($"Weather/Forecast?latitude={latitude}&longitude={longitude}");
				if (!response.IsSuccessStatusCode) return null;
				return await response.Content.ReadFromJsonAsync<ForecastResponseDto>();
			}
			catch (HttpRequestException)
			{
				return null;
			}
		}

		public string GetWeatherIcon(int code) => code switch
		{
			// Clear / Cloud
			0 or 1 or 2 or 3 => "☀️",

			// Fog
			45 or 48 => "🌫️",

			// Drizzle
			51 or 53 or 55 or 56 or 57 => "🌦️",

			// Rain
			61 or 63 or 65 or 66 or 67 => "🌧️",

			// Snow
			71 or 73 or 75 or 77 => "❄️",

			// Rain showers
			80 or 81 or 82 => "🌦️",

			// Snow showers
			85 or 86 => "❄️",

			// Thunderstorm
			95 or 96 or 99 => "⛈️",

			_ => "Unknown"
		};

		public string GetSummery(int code) => code switch
		{
			// Clear / Cloud
			0 => "Clear Sky",
			1 => "Mainly Clear",
			2 => "Partly Cloudy",
			3 => "Overcast",

			// Fog
			45 => "Fog",
			48 => "Depositing Rime Fog",

			// Drizzle
			51 => "Light Drizzle",
			53 => "Moderate Drizzle",
			55 => "Dense Drizzle",
			56 => "Light Freezing Drizzle",
			57 => "Dense Freezing Drizzle",

			// Rain
			61 => "Slight Rain",
			63 => "Moderate Rain",
			65 => "Heavy Rain",
			66 => "Light Freezing Rain",
			67 => "Heavy Freezing Rain",

			// Snow
			71 => "Slight Snow Fall",
			73 => "Moderate Snow Fall",
			75 => "Heavy Snow Fall",
			77 => "Snow Grains",

			// Rain showers
			80 => "Slight Rain Showers",
			81 => "Moderate Rain Showers",
			82 => "Violent Rain Showers",

			// Snow showers
			85 => "Slight Snow Showers",
			86 => "Heavy Snow Showers",

			// Thunderstorm
			95 => "Thunderstorm",
			96 => "Thunderstorm with Slight Hail",
			99 => "Thunderstorm with Heavy Hail",

			_ => "Unknown"
		};

	}
}
