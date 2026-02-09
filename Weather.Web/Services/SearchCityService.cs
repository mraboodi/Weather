using Weather.Models.Entities.Favorite;
using Weather.Web.Interfaces;

namespace Weather.Web.Services
{
	public class SearchCityService(HttpClient httpClient) : ISearchCityService
	{
		public async Task<List<GeoCity>> SearchAsync(string cityName)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(cityName) || cityName.Length < 2) return [];

				var response = await httpClient.GetAsync($"Weather/SearchCity/{cityName}");
				if (!response.IsSuccessStatusCode) return [];

				return await response.Content.ReadFromJsonAsync<List<GeoCity>>() ?? [];
			}
			catch
			{
				return [];
			}
		}
	}
}
