using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TimeZoneConverter;
using Weather.Api.Interfaces;
using Weather.Models.Configuration;
using Weather.Models.DTOs;
using Weather.Models.Enums;

namespace Weather.Api.Controllers
{
	/// <summary>
	/// Controller exposing weather-related endpoints, including forecasts, city searches, and country code lookups.
	/// </summary>
	/// <remarks>
	/// Responsibilities:
	/// - Retrieve daily weather forecasts for a given latitude and longitude.
	/// - Search for cities by name using an external city search API.
	/// - Retrieve country codes for cities using the same city search service.
	/// 
	/// Configuration:
	/// - Forecast limit is configured via <see cref="WeatherOptions.ForcastMaxLimit"/>.
	/// </remarks>
	[ApiController]
	[Route("api/[controller]")]
	public class WeatherController(ISearchCityService searchCityService, IWeatherService weatherService, IOptions<WeatherOptions> options) : ControllerBase
	{
		private readonly int ForcastMaxLimit = options.Value.ForcastMaxLimit;

		[HttpGet("Forecast")]
		public async Task<IActionResult> GetWeatherForcast([FromQuery] double latitude, [FromQuery] double longitude)
		{
			// Get local machine time zone
			var timeZone = TZConvert.WindowsToIana(TimeZoneInfo.Local.Id);

			var result = await weatherService.GetWeatherByGeoLocationAsync(new GeoLocationDto { Latitude = latitude, Longitude = longitude }, ForcastMaxLimit, timeZone!);

			return result.Error switch
			{
				ServiceErrorType.None => Ok(result.Data), // successful response
				ServiceErrorType.NotFound => NotFound(new { message = "We could not find any weather report for the given location." }),
				ServiceErrorType.TemporaryFailure => StatusCode(503, new { message = "We could not fetch the weather report at the moment. Please try again later." }),
				_ => StatusCode(500, new { message = "An unexpected error occurred." })
			};
		}

		[HttpGet("SearchCity/{cityName}")]
		public async Task<IActionResult> GetCityGeoLocation(string cityName)
		{
			var result = await searchCityService.SearchCity(cityName);

			return result.Error switch
			{
				ServiceErrorType.None => Ok(result.Data), // successful response
				ServiceErrorType.NotFound => NotFound(new { message = $"No cities found matching '{cityName}'." }),
				ServiceErrorType.TemporaryFailure => StatusCode(503, new { message = "We could not fetch city information at the moment. Please try again later." }),
				_ => StatusCode(500, new { message = "An unexpected error occurred." })
			};
		}

        [HttpGet("SearchCountryCode/{cityName}")]
        public async Task<IActionResult> GetCountryCode(string cityName)
        {
            var result = await searchCityService.SearchCity(cityName);

            return result.Error switch
            {
                ServiceErrorType.None => Ok(result.Data), // successful response
                ServiceErrorType.NotFound => NotFound(new { message = $"No cities found matching '{cityName}'." }),
                ServiceErrorType.TemporaryFailure => StatusCode(503, new { message = "We could not fetch city information at the moment. Please try again later." }),
                _ => StatusCode(500, new { message = "An unexpected error occurred." })
            };
        }
    }
}
