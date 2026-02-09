using System.Net.Http.Headers;
using Weather.Models.Entities.Favorite;
using Weather.Web.Interfaces.Identity;

namespace Weather.Web.Services.Favorite;

public class FavoriteService(HttpClient http, IAccessTokenProvider tokenProvider) : IFavoriteService
{
	private async Task AddAuthHeaderAsync()
    {
        var token = await tokenProvider.GetTokenAsync();

        if (!string.IsNullOrWhiteSpace(token))
        {
            http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<List<GeoCity>> GetAsync()
    {
        await AddAuthHeaderAsync();
        return await http.GetFromJsonAsync<List<GeoCity>>("Favorites") ?? [];
    }

    public async Task<string> GetCountryCodeAsync(string city)
    {
        var response = await http.GetAsync($"ISOCountrycode/{city}");
        return await response.Content.ReadAsStringAsync();
    }

    public async Task AddAsync(GeoCity city)
    {
        await AddAuthHeaderAsync();
        await http.PostAsJsonAsync("Favorites", city);
    }

    public async Task RemoveAsync(int cityId)
    {
        await AddAuthHeaderAsync();
        await http.DeleteAsync($"Favorites/{cityId}");
    }
}
