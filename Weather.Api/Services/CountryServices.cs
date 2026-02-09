using System.Text.Json;
using Weather.Api.Interfaces;
using Weather.Models.Entities;

namespace Weather.Api.Services
{
    public class CountryServices(IHttpClientFactory factory) : ICountryServices
    {
        private readonly HttpClient _httpClient = factory.CreateClient();

		public async Task<string> GetISOCode(string city)
        {
            var geoUrl = $"https://geocoding-api.open-meteo.com/v1/search?name={city}&count=1";
            var geoResponse = await _httpClient.GetAsync(geoUrl);
            if (!geoResponse.IsSuccessStatusCode)
                return null;

            var geoJson = await geoResponse.Content.ReadAsStringAsync();
            var geoData = JsonSerializer.Deserialize<GeoResponse>(geoJson);

            if (geoData == null)
                return null;

            return geoData.country_code;
        }
    }
}
