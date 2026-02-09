using Weather.Models.Entities.Favorite;

namespace Weather.Web.Services.Favorite;

public class FavoriteState(IFavoriteService api) : IFavoriteState
{
	private readonly List<GeoCity> favorites = [];

	public IReadOnlyList<GeoCity> Favorites => favorites;
	public int Count => favorites.Count;

	public event Action? OnChange;

	public async Task LoadAsync()
	{
		favorites.Clear();
		favorites.AddRange(await api.GetAsync());
		Notify();
	}

	public bool IsFavorite(int cityId) => favorites.Any(f => f.CityId == cityId);

	public async Task AddAsync(GeoCity city)
	{
		if (IsFavorite(city.CityId)) return;

		await api.AddAsync(city);
		favorites.Add(city);
		Notify();
	}

	public async Task RemoveAsync(int cityId)
	{
		var item = favorites.FirstOrDefault(f => f.CityId == cityId);
		if (item == null) return;

		await api.RemoveAsync(cityId);
		favorites.Remove(item);
		Notify();
	}

	private void Notify() => OnChange?.Invoke();
}
