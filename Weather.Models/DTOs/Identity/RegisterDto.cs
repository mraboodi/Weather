using System.ComponentModel.DataAnnotations;
using Weather.Models.Entities.Identity;

namespace Weather.Models.DTOs.Identity
{
	public class RegisterDto
	{
		[Required(AllowEmptyStrings = false, ErrorMessage = "Please provide first name")]
		public string FirstName { get; set; } = string.Empty;

		[Required(AllowEmptyStrings = false, ErrorMessage = "Please provide last name")]
		public string LastName { get; set; } = string.Empty;

		[Required(AllowEmptyStrings = false, ErrorMessage = "Please provide user email")]
		public string Email { get; set; } = string.Empty;

		[Required(AllowEmptyStrings = false, ErrorMessage = "Please provide user password")]
		public string Password { get; set; } = string.Empty;

		public string Role { get; set; } = UserRoles.SimpleUser;
	}
}
