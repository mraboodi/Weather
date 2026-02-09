using Microsoft.Extensions.Logging;
using System.Text.Json;
using Weather.Api.Interfaces;
using Weather.Models.Entities.Favorite;
using Weather.Models.Enums;

namespace Weather.Api.Services
{
	/// <summary>
	/// Implementation of <see cref="ISearchCityService"/> that queries an external API
	/// to retrieve city information based on a city name.
	/// </summary>
	/// <remarks>
	/// Responsibilities:
	/// - Build and execute HTTP requests to the external city search API
	/// - Parse and validate JSON responses
	/// - Map results into <see cref="GeoCity"/> DTOs
	/// - Handle errors consistently using <see cref="ServiceResult{T}"/>
	/// </remarks>
	public class SearchCityService(IHttpClientFactory clientFactory, IConfiguration config, ILogger<WeatherService> logger) : ISearchCityService
	{
		public async Task<ServiceResult<List<GeoCity>?>> SearchCity(string cityName)
		{
			// Create HttpClient via factory (handles DNS refresh, pooling, etc.)
			//var apiKey = config["ExternalWeatherApi:Key"];
			var baseSearchCityAddress = config["ExternalWeatherApi:BaseSearchCityAddress"];
			var geoCitiesExternalApiUrl = $"{baseSearchCityAddress}?name={cityName}&language=en&format=json";
				var client = clientFactory.CreateClient();
				var response = await client.GetAsync(geoCitiesExternalApiUrl);

				if (!response.IsSuccessStatusCode) return new(null, ServiceErrorType.TemporaryFailure); // Rfere to externak api documentation

				// JSON parsing can fail if API returns invalid or unexpected JSON
				var geoData = await response.Content.ReadFromJsonAsync<JsonElement>();

				// Ensure "results" exists and is an array
				if (!geoData.TryGetProperty("results", out var results) || results.ValueKind != JsonValueKind.Array)
					return new(null, ServiceErrorType.NotFound);

				// Map found cities
				var cities = new List<GeoCity>();
				foreach (var result in results.EnumerateArray())
				{
					// Validate before acceptance
					if (!result.TryGetProperty("id", out var id) ||
						!result.TryGetProperty("latitude", out var latitude) ||
						!result.TryGetProperty("longitude", out var longitude)) continue;
					
					cities.Add(new GeoCity
					{
						CityId = id.GetInt32(),
						Name = result.GetProperty("name").GetString() ?? "",
						Latitude = latitude.GetDouble(),
						Longitude = longitude.GetDouble(),
						Country = result.GetProperty("country").GetString() ?? "",
						State = result.GetProperty("admin1").GetString() ?? "",                        
                        CountryCode = result.GetProperty("country_code").GetString() ?? ""
                    });
				}

			try
			{
				return cities.Count == 0
						? new(null, ServiceErrorType.NotFound)
						: new(cities, ServiceErrorType.None); ;
			}
			catch (HttpRequestException e)
			{
				// Network-level failure (API down, no internet, DNS issue)
				logger.LogError(e, "Network error while fetching weather for {cityName}}", cityName);
				return new(null, ServiceErrorType.TemporaryFailure);
			}
			catch (TaskCanceledException e)
			{
				// Timeout
				logger.LogError(e, "Timeout while fetching weather for {cityName}", cityName);
				return new(null, ServiceErrorType.TemporaryFailure);
			}
			catch (JsonException e)
			{
				// Invalid JSON
				logger.LogError(e, "Invalid JSON received from Weather API for {cityName}", cityName);
				return new(null, ServiceErrorType.TemporaryFailure);
			}
			catch (Exception e)
			{
				// Unexpected error
				logger.LogError(e, "Unexpected error while fetching weather for {cityName}", cityName);
				return new(null, ServiceErrorType.TemporaryFailure);
			}
		}
	}
}
