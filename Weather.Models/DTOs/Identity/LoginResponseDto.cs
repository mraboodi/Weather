
namespace Weather.Models.DTOs.Identity
{
	public record LoginResponseDto(
		string Token,
		DateTime Expiration,
		string? Role,
		string FirstName,
		string LastName
	);
}
