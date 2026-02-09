using Weather.Web.Interfaces.Identity;

namespace Weather.Web.Services.Identity
{
	//public class AccessTokenProvider() : IAccessTokenProvider
	//{
	//	private string? token;

	//	public Task<string?> GetTokenAsync() => Task.FromResult(token);

	//	public Task SetTokenAsync(string token)
	//	{
	//		this.token = token;
	//		return Task.CompletedTask;
	//	}

	//	public Task ClearTokenAsync()
	//	{
	//		token = null;
	//		return Task.CompletedTask;
	//	}
	//}
	public class AccessTokenProvider : IAccessTokenProvider
	{
		private string? token;

		public Task<string?> GetTokenAsync() => Task.FromResult(token);

		public Task SetTokenAsync(string token)
		{
			this.token = token;
			return Task.CompletedTask;
		}

		public Task ClearTokenAsync()
		{
			token = null;
			return Task.CompletedTask;
		}
	}

}
