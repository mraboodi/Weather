using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Weather.Api.Data;
using Weather.Api.Services;
using Weather.Models.Configuration;
using Weather.Models.Entities.Favorite;
using Weather.Models.Entities.Identity;

namespace Weather.Api.Controllers
{
	/// <summary>
	/// Controller for managing user favorite cities.
	/// </summary>
	/// <remarks>
	/// Responsibilities:
	/// - Retrieve the list of favorite cities for authenticated users
	/// - Add new favorite cities, ensuring the user does not exceed their limit
	/// - Remove favorite cities
	/// 
	/// Access control:
	/// - All endpoints require authenticated users ([Authorize])
	/// - Add and Remove operations restricted to SuperUser and Administrator roles
	/// </remarks>
	/// <param name="db">Database context for accessing favorites and cities.</param>
	/// <param name="userManager">UserManager for retrieving user information.</param>
	/// <param name="options">Weather configuration options containing favorite limit.</param>
	/// <param name="logger">Logger for error and diagnostic messages.</param>
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class FavoritesController(AppDbContext db, UserManager<ApplicationUser> userManager, IOptions<WeatherOptions> options, ILogger<WeatherService> logger) : ControllerBase
	{
		private readonly int FavoriteLimit = options.Value.FavoriteMaxLimit;
		
		[HttpGet]
		public async Task<IActionResult> GetFavorites()
		{
			try
			{
				var user = await userManager.GetUserAsync(User);

				if (user == null) return Unauthorized();
				var roles = await userManager.GetRolesAsync(user);

				if (!roles.Contains(UserRoles.SuperUser) && !roles.Contains(UserRoles.Administrator))
					return Forbid("Only Super Users can delete favorites.");

				var userId = userManager.GetUserId(User);

				var favorites = await db.FavoriteCities
					.Where(f => f.UserId == userId)
					.Include(f => f.GeoCity)
					.Select(f => new
					{
						f.CityId,
						f.GeoCity.Name,
						f.GeoCity.Latitude,
						f.GeoCity.Longitude,
						f.GeoCity.Country,
						f.GeoCity.State,
					})
					.ToListAsync();

				return Ok(favorites);
			}
			catch (Exception e)
			{
				logger.LogError(e, "An error occurred while getting favorites.");
				return StatusCode(500, "An error occurred while getting favorites.");
			}
		}


		[Authorize(Roles = $"{UserRoles.SuperUser},{UserRoles.Administrator}")]
		[HttpPost]
		public async Task<IActionResult> AddFavorite([FromBody] GeoCity geoCityData)
		{
			try
			{
				// Validate user, though it is being validated thruough [Authorize]
				var user = await userManager.GetUserAsync(User);
				if (user == null) return Unauthorized();

				// Validate role
				var roles = await userManager.GetRolesAsync(user);
				if (!roles.Contains(UserRoles.SuperUser) && !roles.Contains(UserRoles.Administrator))
					return Forbid("Only Super Users can save favorites.");

				// Check user favorite limit
				var count = await db.FavoriteCities.CountAsync(f => f.UserId == user!.Id);
				if (count >= FavoriteLimit)
					return BadRequest($"You have reached the limit of {FavoriteLimit} favorite cities.");

				// Check if GeoCity exists
				var city = await db.GeoCities.FirstOrDefaultAsync(gCity => gCity.CityId == geoCityData.CityId);
				if (city == null)
				{
					db.GeoCities.Add(geoCityData);  // Add new city info (including name, id, geolocation,..)
					await db.SaveChangesAsync();
				}

				// Check if user already has this favorite
				var exists = await db.FavoriteCities.AnyAsync(f => f.UserId == user.Id && f.CityId == geoCityData!.CityId);
				if (exists)
					return BadRequest($"City \"{geoCityData.Name}\" already in favorites.");

				// Add favorite
				db.FavoriteCities.Add(new FavoriteCity { CityId = geoCityData!.CityId, UserId = user.Id });
				await db.SaveChangesAsync();
				return Ok();
			}
			catch (Exception e)
			{
				logger.LogError(e, "An error occurred while adding favorites.");
				return StatusCode(500, "An error occurred while addingfavorites.");
			}
		}

		[Authorize(Roles = $"{UserRoles.SuperUser},{UserRoles.Administrator}")]

		[HttpDelete("{cityId:int}")]
		public async Task<IActionResult> RemoveFavorite(int cityId)
		{
			try
			{
				var user = await userManager.GetUserAsync(User);
				if (user == null) return Unauthorized();

				var favorite = await db.FavoriteCities
									   .FirstOrDefaultAsync(f => f.UserId == user.Id && f.CityId == cityId);
				if (favorite == null) return NotFound("Favorite not found.");

				db.FavoriteCities.Remove(favorite);
				await db.SaveChangesAsync();

				return NoContent();
			}
			catch (Exception e)
			{
				logger.LogError(e, "An error occurred while removing favorite");
				return StatusCode(500, "An error occurred while removing favorite.");
			}
		}
	}
}
