namespace Weather.Api.Interfaces
{
	/// <summary>
	/// Provides operations related to country information based on city names.
	/// </summary>
	/// <remarks>
	/// Implementations are responsible for:
	/// - Querying an external geocoding API
	/// - Mapping API responses to return the ISO country code
	/// - Returning null or error indicators if data is unavailable
	/// </remarks>
	public interface ICountryServices
	{
		/// <summary>
		/// Retrieves the ISO country code for a given city.
		/// </summary>
		/// <param name="city">The name of the city to look up.</param>
		/// <returns>
		/// A string representing the ISO country code (e.g., "SG", "US"), or null if not found.
		/// </returns>
		Task<string> GetISOCode(string city);
	}
}
