using Weather.Models.Entities.Favorite;



/// <summary>
/// Provides front-end services to manage the user's favorite cities.
/// </summary>
/// <remarks>
/// Responsibilities:
/// - Retrieve the user's favorite cities.
/// - Add or remove cities from the user's favorites.
/// - Fetch country codes for cities.
/// - Interfaces with back-end services or APIs to persist favorites.
/// </remarks>
public interface IFavoriteService
{
	/// <summary>
	/// Retrieves the list of favorite cities for the current user.
	/// </summary>
	/// <returns>A list of <see cref="GeoCity"/> representing the user's favorite cities.</returns>
	Task<List<GeoCity>> GetAsync();

	/// <summary>
	/// Adds a city to the user's list of favorites.
	/// </summary>
	/// <param name="city">The <see cref="GeoCity"/> object representing the city to add.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	Task AddAsync(GeoCity city);

	/// <summary>
	/// Removes a city from the user's list of favorites by city ID.
	/// </summary>
	/// <param name="cityId">The unique identifier of the city to remove.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	Task RemoveAsync(int cityId);

	/// <summary>
	/// Retrieves the ISO country code for a specified city.
	/// </summary>
	/// <param name="city">The name of the city.</param>
	/// <returns>A string representing the ISO country code of the city.</returns>
	Task<string> GetCountryCodeAsync(string city);
}