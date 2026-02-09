using Microsoft.AspNetCore.Mvc;
using Weather.Api.Interfaces;

namespace Weather.Api.Controllers
{
	/// <summary>
	/// Provides endpoints to retrieve ISO country codes for a given city.
	/// </summary>
	/// <remarks>
	/// Responsibilities:
	/// - Calls <see cref="ICountryServices.GetISOCode"/> to fetch ISO country codes.
	/// - Returns the country code as a string, or null if the city is not found.
	/// </remarks>
	[ApiController]
    [Route("[controller]")]
    public class ISOCountrycodeController(ICountryServices countryServices) : ControllerBase
    {
		[HttpGet("{city}")]
        public async Task<IActionResult> Get(string city)
        {
            var results = await countryServices.GetISOCode(city);
            return Ok(results);
        }
    }
}
