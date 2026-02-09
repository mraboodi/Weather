using Weather.Models.Entities.Favorite;
using Weather.Models.Enums;

namespace Weather.Api.Interfaces
{
	/// <summary>
	/// Provides operations for searching cities via an external weather/geolocation API.
	/// </summary>
	/// <remarks>
	/// Implementations are responsible for:
	/// - Querying the external (or any other mean) API with a city name
	/// - Mapping API results to internal <see cref="GeoCity"/> DTOs
	/// - Returning a standardized <see cref="ServiceResult{T}"/> indicating success or error type
	/// </remarks>
	public interface ISearchCityService
	{
		/// <summary>
		/// Searches for cities matching the specified city name.
		/// </summary>
		/// <param name="cityName">The name of the city to search for.</param>
		/// <returns>
		/// A <see cref="ServiceResult{T}"/> containing:
		/// - A list of <see cref="GeoCity"/> if found
		/// - <see cref="ServiceErrorType.NotFound"/> if no results are returned
		/// - <see cref="ServiceErrorType.TemporaryFailure"/> if a network or API issue occurs
		/// </returns>
		Task<ServiceResult<List<GeoCity>?>> SearchCity(string cityName);
	}
}
