using System.Security.Claims;
using Weather.Web.Interfaces.Identity;

namespace Weather.Web.Helpers
{
	public static class AuthStateServiceExtensions
	{
		/// <summary>
		/// Checks if the current user is authenticated and has any of the specified roles.
		/// </summary>
		public static bool UserHasAnyRole(this IAuthStateService auth, params string[] roles)
		{
			if (!auth.IsAuthenticated || auth.User is null)
				return false;

			return roles.Any(r => auth.User.IsInRole(r));
		}

		/// <summary>
		/// Safe role check for a ClaimsPrincipal.
		/// </summary>
		public static bool IsInRole(this ClaimsPrincipal? user, string role)
			=> user?.IsInRole(role) == true;

		/// <summary>
		/// Returns the user's first name from ClaimsPrincipal.
		/// </summary>
		public static string? GetFirstName(this IAuthStateService auth)
			=> auth.User?.FindFirst(ClaimTypes.GivenName)?.Value;

		/// <summary>
		/// Returns the user's last name from ClaimsPrincipal.
		/// </summary>
		public static string? GetLastName(this IAuthStateService auth)
			=> auth.User?.FindFirst(ClaimTypes.Surname)?.Value;

		/// <summary>
		/// Returns the user's email from ClaimsPrincipal.
		/// </summary>
		public static string? GetEmail(this IAuthStateService auth)
			=> auth.User?.FindFirst(ClaimTypes.Name)?.Value;
	}
}
