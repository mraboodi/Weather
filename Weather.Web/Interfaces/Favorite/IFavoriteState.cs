using Weather.Models.Entities.Favorite;

/// <summary>
/// Provides a front-end abstraction for managing the in-memory state of a user's favorite cities.
/// </summary>
/// <remarks>
/// Responsibilities:
/// - Maintain a reactive in-memory list of favorite cities.
/// - Expose read-only access to favorites and their count.
/// - Allow checking if a specific city is in the favorites.
/// - Trigger UI updates when favorites change.
/// - Integrates with back-end services to load, add, or remove favorites.
/// </remarks>
public interface IFavoriteState
{
	/// <summary>
	/// Read-only list of the user's favorite cities.
	/// </summary>
	IReadOnlyList<GeoCity> Favorites { get; }

	/// <summary>
	/// Total count of favorite cities.
	/// </summary>
	int Count { get; }

	/// <summary>
	/// Checks whether a given city is already in the user's favorites.
	/// </summary>
	/// <param name="cityId">The unique identifier of the city.</param>
	/// <returns>True if the city is in the favorites; otherwise, false.</returns>
	bool IsFavorite(int cityId);

	/// <summary>
	/// Loads the user's favorite cities from the back-end service into the in-memory state.
	/// </summary>
	/// <returns>A task representing the asynchronous load operation.</returns>
	Task LoadAsync();

	/// <summary>
	/// Adds a city to the in-memory favorites state and optionally persists it to the back-end.
	/// </summary>
	/// <param name="city">The <see cref="GeoCity"/> object to add.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	Task AddAsync(GeoCity city);

	/// <summary>
	/// Removes a city from the in-memory favorites state and optionally removes it from the back-end.
	/// </summary>
	/// <param name="cityId">The unique identifier of the city to remove.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	Task RemoveAsync(int cityId);

	/// <summary>
	/// Event triggered whenever the favorites state changes.
	/// </summary>
	/// <remarks>
	/// - Can be used by UI components to update dynamically when a favorite is added or removed.
	/// </remarks>
	event Action? OnChange;
}