using System.Security.Claims;

namespace Weather.Web.Interfaces.Identity
{
	/// <summary>
	/// Provides a front-end abstraction for managing the authentication state of the current user.
	/// </summary>
	/// <remarks>
	/// Responsibilities:
	/// - Maintain the current authenticated user's identity and claims.
	/// - Track whether a user is currently authenticated.
	/// - Trigger events when authentication state changes.
	/// - Persist or clear JWT tokens, roles, and email for session management.
	/// - Initialize authentication state on application start or page reload.
	/// </remarks>
	public interface IAuthStateService
	{
		/// <summary>
		/// Gets the current authenticated user as a <see cref="ClaimsPrincipal"/>.
		/// </summary>
		/// <remarks>
		/// - Returns null if no user is authenticated.
		/// - Used for authorization checks or retrieving claims such as email or role.
		/// </remarks>
		ClaimsPrincipal? User { get; }

		/// <summary>
		/// Indicates whether a user is currently authenticated.
		/// </summary>
		bool IsAuthenticated { get; }

		/// <summary>
		/// Event triggered whenever the authentication state changes.
		/// </summary>
		/// <remarks>
		/// - Can be used by UI components to reactively update when a user logs in or out.
		/// </remarks>
		event Action? OnChange;

		/// <summary>
		/// Sets the current user, storing the JWT token, role, and email.
		/// </summary>
		/// <param name="token">JWT access token for the user.</param>
		/// <param name="role">Assigned role of the user.</param>
		/// <param name="email">Email of the user.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task SetUserAsync(string token, string role, string email);

		/// <summary>
		/// Logs out the current user, clearing stored token and session information.
		/// </summary>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task LogoutAsync();

		/// <summary>
		/// Initializes the authentication state on app startup or page reload.
		/// </summary>
		/// <remarks>
		/// - Typically retrieves a persisted token from storage and sets the current user if valid.
		/// </remarks>
		Task InitializeAsync();
	}
}
