using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weather.Models.Entities.Favorite
{
	public class GeoCity
	{
		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Range(1, int.MaxValue, ErrorMessage = "City Id must be greater than 0.")]
		public int CityId { get; set; }   // External-controlled Id

		[Required]
		public string Name { get; set; } = string.Empty;

		[Required]
		[Range(-90.0, 90.0, ErrorMessage = "Latitude must be between -90 and 90.")]
		public double Latitude { get; set; }

		[Required]
		[Range(-180.0, 180.0, ErrorMessage = "Longitude must be between -180 and 180.")]
		public double Longitude { get; set; }

		[Required]
		public string Country { get; set; } = string.Empty;

		public string? State { get; set; }

        public string? CountryCode { get; set; }
    }
}
