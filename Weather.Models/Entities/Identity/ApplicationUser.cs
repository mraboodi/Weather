using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Weather.Models.Entities.Favorite;

namespace Weather.Models.Entities.Identity
{
    // User info can be extended here 
    public class ApplicationUser : IdentityUser
    {
		[Required(AllowEmptyStrings = false, ErrorMessage = "Please provide first name")]
		public string FirstName { get; set; } = string.Empty;

		[Required(AllowEmptyStrings = false, ErrorMessage = "Please provide last name")]
		public string LastName { get; set; } = string.Empty;
		public ICollection<FavoriteCity> FavoriteCities { get; set; } = [];

	} 

}
