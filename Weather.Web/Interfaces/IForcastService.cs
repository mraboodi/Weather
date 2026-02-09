
using Weather.Models.DTOs.Forcast;

namespace Weather.Web.Interfaces
{
	/// <summary>
	/// Provides front-end services for fetching and interpreting weather forecasts.
	/// </summary>
	/// <remarks>
	/// Responsibilities:
	/// - Retrieve weather forecasts from a back-end API.
	/// - Map weather codes to human-readable icons and summary strings.
	/// </remarks>
	public interface IForcastService
	{
		/// <summary>
		/// Fetches the weather forecast for a specified latitude and longitude.
		/// </summary>
		/// <remarks>
		/// - Typically calls a back-end API to retrieve <see cref="ForecastResponseDto"/>.
		/// - Can be used to display daily forecasts on the front-end.
		/// </remarks>
		/// <param name="latitude">Latitude of the location.</param>
		/// <param name="longitude">Longitude of the location.</param>
		/// <returns>
		/// A <see cref="ForecastResponseDto"/> containing the forecast, 
		/// or null if the forecast could not be retrieved.
		/// </returns>
		Task<ForecastResponseDto?> GetWeatherAsync(double latitude, double longitude);

		/// <summary>
		/// Returns a weather icon (as string, e.g., CSS class or SVG path) for a given weather code.
		/// </summary>
		/// <remarks>
		/// - Maps numeric weather codes from the forecast API to icons.
		/// - Can be used to render weather icons in the UI.
		/// </remarks>
		/// <param name="code">Weather code from forecast API.</param>
		/// <returns>String representing the weather icon.</returns>
		string GetWeatherIcon(int code);

		/// <summary>
		/// Returns a human-readable summary for a given weather code.
		/// </summary>
		/// <remarks>
		/// - Converts numeric weather codes to descriptive text (e.g., "Sunny", "Rainy").
		/// - Useful for displaying summary text alongside icons.
		/// </remarks>
		/// <param name="code">Weather code from forecast API.</param>
		/// <returns>Human-readable weather summary.</returns>
		string GetSummery(int code);
	}
}
