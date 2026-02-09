using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Weather.Models.Entities.Favorite;
using Weather.Models.Entities.Identity;

namespace Weather.Api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
		public DbSet<FavoriteCity> FavoriteCities { get; set; }
		public DbSet<GeoCity> GeoCities { get; set; }
	}
}
