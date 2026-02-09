using Weather.Models.Entities.Favorite;

namespace Weather.Web.Interfaces
{
	/// <summary>
	/// Provides front-end services for searching cities by name.
	/// </summary>
	/// <remarks>
	/// Responsibilities:
	/// - Call back-end APIs to search for cities matching a given name.
	/// - Return a list of <see cref="GeoCity"/> objects containing city details.
	/// - Used for autocomplete, dropdowns, or location selection in the UI.
	/// </remarks>
	public interface ISearchCityService
	{
		/// <summary>
		/// Searches for cities matching the specified name.
		/// </summary>
		/// <remarks>
		/// - Typically calls a back-end API endpoint like /api/Weather/SearchCity/{cityName}.
		/// - Returns an empty list if no cities are found.
		/// </remarks>
		/// <param name="cityName">The name or partial name of the city to search.</param>
		/// <returns>
		/// A <see cref="List{GeoCity}"/> of matching cities.  
		/// Each <see cref="GeoCity"/> contains details like name, latitude, longitude, country, and state.
		/// </returns>
		Task<List<GeoCity>> SearchAsync(string cityName);
	}

}
