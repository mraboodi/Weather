using System.Globalization;
using System.Text.Json;
using Weather.Api.Helpers;
using Weather.Api.Interfaces;
using Weather.Models.DTOs;
using Weather.Models.DTOs.Forcast;
using Weather.Models.Entities;
using Weather.Models.Enums;

namespace Weather.Api.Services
{
	public class WeatherService(IHttpClientFactory clientFactory, IConfiguration config, ILogger<WeatherService> logger) : IWeatherService
	{
		public async Task<ServiceResult<ForecastResponseDto?>> GetWeatherByGeoLocationAsync(GeoLocationDto geoLocationDto, int forcastMaxLimit, string timeZone = "auto")
		{
			try
			{
				//var apiKey = config["ExternalWeatherApi:Key"];
				var baseForcastAddress = config["ExternalWeatherApi:BaseForcastAddress"];
				var client = clientFactory.CreateClient();
				var latitude = geoLocationDto.Latitude;
				var longitude = geoLocationDto.Longitude;

				// 1. Step Two: Get Forecast using Coordinates and timeZone (refer to external api documentation)
				var encodedTimeZone = Uri.EscapeDataString(timeZone); // "Asia/Singapore" → "Asia%2FSingapore"
				//var weatherUrl = $"{baseForcastAddress}?latitude={latitude}&longitude={longitude}" +
				//				 $"&daily=weather_code,temperature_2m_max,temperature_2m_min,rain_sum,apparent_temperature_max,apparent_temperature_min" +
				//				 $"&timezone={encodedTimeZone}&forecast_days={forcastMaxLimit}&format=json&timeformat=unixtime";

				var weatherUrl = $"{baseForcastAddress}?latitude={latitude.ToString(CultureInfo.InvariantCulture)}&longitude={longitude.ToString(CultureInfo.InvariantCulture)}" +
								 $"&daily=weather_code,temperature_2m_max,temperature_2m_min,rain_sum,apparent_temperature_max,apparent_temperature_min" +
								 $"&timezone={encodedTimeZone}&forecast_days={forcastMaxLimit}&format=json&timeformat=unixtime";

				var weatherResponse = await client.GetAsync(weatherUrl);
				if (!weatherResponse.IsSuccessStatusCode) return new(null, ServiceErrorType.TemporaryFailure);

				var weatherData = await weatherResponse.Content.ReadFromJsonAsync<JsonElement>();
				var daily = weatherData.GetProperty("daily");

				// 2. Step Three: Map parallel arrays to a List of DailyForecastDto
				var times = daily.GetProperty("time").EnumerateArray().ToList();
				if (times.Count == 0) return new(null, ServiceErrorType.NotFound); // no need to continue mapping

				var maxTemps = daily.GetProperty("temperature_2m_max").EnumerateArray().ToList();
				var minTemps = daily.GetProperty("temperature_2m_min").EnumerateArray().ToList();
				var codes = daily.GetProperty("weather_code").EnumerateArray().ToList();
				var rains = daily.GetProperty("rain_sum").EnumerateArray().ToList();


                // Map for the next {count} days (requirement: 3-5 days)
                var days = new List<ForecastDayDto>();
				
				for (int i = 0; i < times.Count; i++)
				{
					days.Add(new ForecastDayDto(
						times[i].GetInt64().ToUtcDateTime(),
						maxTemps[i].GetDouble(),
						minTemps[i].GetDouble(),
						codes[i].GetInt32(),
                        rains[i].GetDouble()
					));
				}
				return new(new ForecastResponseDto(days), ServiceErrorType.None);
			}
			catch (HttpRequestException e)
			{
				// Network issue
				logger.LogError(e, "Network error while fetching weather for {Latitude},{Longitude}", geoLocationDto.Latitude, geoLocationDto.Longitude);
				return new(null, ServiceErrorType.TemporaryFailure);
			}
			catch (TaskCanceledException e)
			{
				// Timeout
				logger.LogError(e, "Timeout while fetching weather for {Latitude},{Longitude}", geoLocationDto.Latitude, geoLocationDto.Longitude);
				return new(null, ServiceErrorType.TemporaryFailure);
			}
			catch (JsonException e)
			{
				// Invalid JSON
				logger.LogError(e, "Invalid JSON received from Weather API for {Latitude},{Longitude}", geoLocationDto.Latitude, geoLocationDto.Longitude);
				return new(null, ServiceErrorType.TemporaryFailure);
			}
			catch (Exception e)
			{
				// Unexpected error
				logger.LogError(e, "Unexpected error while fetching weather for {Latitude},{Longitude}", geoLocationDto.Latitude, geoLocationDto.Longitude);
				return new(null, ServiceErrorType.TemporaryFailure);
			}
		}
	}

}
