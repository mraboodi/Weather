using Weather.Models.DTOs.Identity;

namespace Weather.Web.Interfaces.Identity
{
	/// <summary>
	/// Provides front-end services for user authentication and registration.
	/// </summary>
	/// <remarks>
	/// Responsibilities:
	/// - Call back-end authentication endpoints to login and register users.
	/// - Return structured DTOs for successful login and registration responses.
	/// - Handle asynchronous network operations and error propagation.
	/// </remarks>
	public interface IAuthService
	{
		/// <summary>
		/// Authenticates a user with the provided credentials.
		/// </summary>
		/// <remarks>
		/// - Typically calls the back-end /api/Auth/Login endpoint.
		/// - Returns a <see cref="LoginResponseDto"/> containing JWT token, expiration, role, and user info.
		/// - Returns null if login fails (invalid credentials or network error).
		/// </remarks>
		/// <param name="dto">Login credentials including email and password.</param>
		/// <returns>
		/// A <see cref="LoginResponseDto"/> on success, or null if authentication fails.
		/// </returns>
		Task<LoginResponseDto?> LoginAsync(LoginDto dto);

		/// <summary>
		/// Registers a new user account with the provided information.
		/// </summary>
		/// <remarks>
		/// - Typically calls the back-end /api/Auth/Register endpoint.
		/// - Returns an array of validation error messages if registration fails.
		/// - Returns null or empty array if registration succeeds without errors.
		/// </remarks>
		/// <param name="dto">Registration data including first name, last name, email, password, and optional role.</param>
		/// <returns>
		/// Array of error messages if registration fails, or null/empty array on success.
		/// </returns>
		Task<string[]?> RegisterAsync(RegisterDto dto);
	}
}
