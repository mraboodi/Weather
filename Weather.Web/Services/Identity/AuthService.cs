using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Weather.Models.DTOs.Identity;
using Weather.Web.Interfaces.Identity;

namespace Weather.Web.Services.Identity
{
	public class AuthService(HttpClient http, IAccessTokenProvider tokenProvider) : IAuthService
	{
		private async Task AddAuthHeaderAsync()
		{
			var token = await tokenProvider.GetTokenAsync();

			if (!string.IsNullOrWhiteSpace(token))
				http.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", token);
		}

		public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
		{
			var response = await http.PostAsJsonAsync("Auth/Login", dto);
			if (!response.IsSuccessStatusCode) return null;
			
			return await response.Content.ReadFromJsonAsync<LoginResponseDto>();
		}

		public async Task<string[]?> RegisterAsync(RegisterDto dto)
		{
			await AddAuthHeaderAsync();
			var response = await http.PostAsJsonAsync("Auth/Register", dto);

			if (response.IsSuccessStatusCode) return null;

			if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
			{
				var problem = await response.Content
					.ReadFromJsonAsync<ValidationProblemDetails>();

				return problem?
					.Errors
					.SelectMany(e => e.Value)
					.ToArray();
			}

			return ["An unexpected error occurred."];
		}
	}
}
