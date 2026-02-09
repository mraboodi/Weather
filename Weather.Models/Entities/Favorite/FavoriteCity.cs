using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Weather.Models.Entities.Identity;

namespace Weather.Models.Entities.Favorite
{
	public class FavoriteCity
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int CityId{ get; set; } // In most cases (i.e., external APIs) should be integer.

		[Required]
		public required string UserId { get; set; }

		[ValidateNever]
		[ForeignKey(nameof(CityId))]
		public GeoCity GeoCity { get; set; } = null!;

		[ValidateNever]
		[ForeignKey(nameof(UserId))]
		public ApplicationUser User { get; set; } = null!;
	}
}
