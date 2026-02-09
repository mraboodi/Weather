using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using Weather.Web.Interfaces.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Weather.Web.Interfaces.Identity;

namespace Weather.Web.Services.Identity
{
	//public class AuthStateService(IAccessTokenProvider tokenProvider): IAuthStateService
	//{
	//	public bool IsAuthenticated => User != null;
	//	public ClaimsPrincipal? User { get; private set; }

	//	public event Action? OnChange;

	//	public async Task SetUserAsync(string token, string role, string email)
	//	{
	//		// Store token in scoped provider
	//		await tokenProvider.SetTokenAsync(token);

	//		var claims = new[]
	//		{
	//		new Claim(ClaimTypes.Name, email),
	//		new Claim(ClaimTypes.Role, role)
	//	};

	//		User = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
	//		Notify();
	//	}

	//	public async Task LogoutAsync()
	//	{
	//		await tokenProvider.ClearTokenAsync();
	//		User = null;
	//		Notify();
	//	}

	//	private void Notify() => OnChange?.Invoke();
	//}


		public class AuthStateService : IAuthStateService
		{
			private readonly IAccessTokenProvider _tokenProvider;
			private readonly ProtectedSessionStorage _storage;

			private const string TokenKey = "authUserToken";
			private const string EmailKey = "authUserEmail";
			private const string RoleKey = "authUserRole";

			public ClaimsPrincipal? User { get; private set; }
			public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;

			public event Action? OnChange;

			private bool _isInitialized;

			public AuthStateService(IAccessTokenProvider tokenProvider, ProtectedSessionStorage storage)
			{
				_tokenProvider = tokenProvider;
				_storage = storage;
			}

			/// <summary>
			/// Initialize the service by restoring user from session storage.
			/// Must be called after JS interop is available.
			/// </summary>
			public async Task InitializeAsync()
			{
				if (_isInitialized) return;
				_isInitialized = true;

				var tokenResult = await _storage.GetAsync<string>(TokenKey);
				var emailResult = await _storage.GetAsync<string>(EmailKey);
				var roleResult = await _storage.GetAsync<string>(RoleKey);

				if (tokenResult.Success && emailResult.Success && roleResult.Success)
				{
					// Restore token for API calls
					await _tokenProvider.SetTokenAsync(tokenResult.Value);

					// Rebuild ClaimsPrincipal
					User = new ClaimsPrincipal(new ClaimsIdentity(new[]
					{
						new Claim(ClaimTypes.Name, emailResult.Value),
						new Claim(ClaimTypes.Role, roleResult.Value)
					}, "jwt"));

					OnChange?.Invoke();
				}
			}

			public async Task SetUserAsync(string token, string role, string email)
			{
				// Store token in provider for API calls
				await _tokenProvider.SetTokenAsync(token);

				// Persist in session storage
				await _storage.SetAsync(TokenKey, token);
				await _storage.SetAsync(EmailKey, email);
				await _storage.SetAsync(RoleKey, role ?? "User");

				// Update ClaimsPrincipal
				User = new ClaimsPrincipal(new ClaimsIdentity(new[]
				{
				new Claim(ClaimTypes.Name, email),
				new Claim(ClaimTypes.Role, role ?? "User")
			}, "jwt"));

				OnChange?.Invoke();
			}

			public async Task LogoutAsync()
			{
				User = null;

				await _tokenProvider.ClearTokenAsync();
				await _storage.DeleteAsync(TokenKey);
				await _storage.DeleteAsync(EmailKey);
				await _storage.DeleteAsync(RoleKey);

				OnChange?.Invoke();
			}
		}
}
