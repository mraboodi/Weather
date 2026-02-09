using System.ComponentModel.DataAnnotations;

namespace Weather.Models.DTOs.Identity
{
	public class LoginDto
	{
		[Required(AllowEmptyStrings = false, ErrorMessage = "Please provide user email")]
		public string Email { get; set; } = string.Empty;

		[Required(AllowEmptyStrings = false, ErrorMessage = "Please provide user password")]
		public string Password { get; set; } = string.Empty;
	}
}
