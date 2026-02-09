using Weather.Models.DTOs;
using Weather.Models.DTOs.Forcast;
using Weather.Models.Enums;


namespace Weather.Api.Interfaces
{
	/// <summary>
	/// Defines weather-related operations exposed by the API layer.
	/// </summary>
	/// <remarks>
	/// Implementations are responsible for:
	/// - Calling an external weather provider
	/// - Mapping provider responses into internal DTOs
	/// - Returning a ServiceResult to standardize success and failure handling
	/// </remarks>
	public interface IWeatherService
	{
		/// <summary>
		/// Retrieves daily weather forecast data using geographic coordinates.
		/// </summary>
		/// <param name="geoLocationDto">
		/// Geographic location containing latitude and longitude.
		/// </param>
		/// <param name="forcastMaxLimit">
		/// Maximum number of forecast days to retrieve.
		/// </param>
		/// <param name="timeZone">
		/// Time zone used by the external weather API (e.g. "Asia/Singapore", "auto").
		/// </param>
		/// <returns>
		/// A <see cref="ServiceResult{T}"/> containing:
		/// - <see cref="ForecastResponseDto"/> on success
		/// - An appropriate <see cref="ServiceErrorType"/> on failure
		/// </returns>
		Task<ServiceResult<ForecastResponseDto?>> GetWeatherByGeoLocationAsync(
			GeoLocationDto geoLocationDto,
			int forcastMaxLimit,
			string timeZone
		);
	}
}
