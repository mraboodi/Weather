namespace Weather.Web.Interfaces.Identity
{
	/// <summary>
	/// Provides an abstraction for managing JWT access tokens on the front-end.
	/// </summary>
	/// <remarks>
	/// Responsibilities:
	/// - Retrieve, store, and clear JWT tokens for authenticated users.
	/// - Supports asynchronous operations to allow integration with local storage, session storage, or other persistent storage mechanisms.
	/// - Used by authentication services, HTTP clients, or API calls to attach tokens for authorized requests.
	/// </remarks>
	public interface IAccessTokenProvider
	{
		/// <summary>
		/// Retrieves the currently stored access token.
		/// </summary>
		/// <remarks>
		/// - Returns null if no token is stored.
		/// - Typically called before making authorized API requests.
		/// </remarks>
		/// <returns>The JWT access token as a string, or null if unavailable.</returns>
		Task<string?> GetTokenAsync();

		/// <summary>
		/// Stores or updates the access token.
		/// </summary>
		/// <param name="token">The JWT access token to store.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task SetTokenAsync(string token);

		/// <summary>
		/// Clears the currently stored access token.
		/// </summary>
		/// <remarks>
		/// - Typically called during logout or token expiration.
		/// </remarks>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task ClearTokenAsync();
	}
}
