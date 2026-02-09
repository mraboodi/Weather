
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Weather.Api.Helpers;
using Weather.Models.Entities.Identity;

namespace Weather.Api.Data.DBInitializer
{

	public class DBInitializer(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, AppDbContext db, IConfiguration config) : IDBInitializer
	{
		private readonly RoleManager<IdentityRole> roleManager = roleManager;
		private readonly UserManager<ApplicationUser> userManager = userManager;
		private readonly AppDbContext db = db;
		private readonly IConfiguration config = config;

		/// <summary>
		/// The conditions inside the 'Initializer()' method should only executed if the application
		/// runs for the first time. Otherwise the condition should not be fullfilled.
		/// </summary>
		public void Initializer()
		{
			// Apply pending migrations.
			try
			{
				if (db.Database.GetPendingMigrations().Any())
					db.Database.Migrate();
			}
			catch (Exception e) {
				Console.WriteLine(e);
			}

			// 2. Seed User Roles
			foreach (var role in UserRoles.All)
				if (!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
					roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();

			// // 2. Seed Administrator User
			var adminEmail = config["AdminUser:Email"];
			var adminPassword = config["AdminUser:Password"];

			if (adminEmail.IsNullOrEmpty() || adminPassword.IsNullOrEmpty())
				throw new Exception("Admin email or password not configured.");

			var adminUser = userManager.FindByEmailAsync(adminEmail!).GetAwaiter().GetResult();

			if (adminUser == null)
			{
				adminUser = new ApplicationUser
				{
					UserName = adminEmail,
					Email = adminEmail,
					EmailConfirmed = true
				};

				var result = userManager.CreateAsync(adminUser, adminPassword ?? "").GetAwaiter().GetResult();
				if (!result.Succeeded)
					throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");

				// Assign Admin role
				userManager.AddToRoleAsync(adminUser, UserRoles.Administrator).GetAwaiter().GetResult();
			}
			else
			{
				// Ensure Admin role is assigned if missing.
				var roles = userManager.GetRolesAsync(adminUser).GetAwaiter().GetResult();
				if (!roles.Contains(UserRoles.Administrator))
					userManager.AddToRoleAsync(adminUser, UserRoles.Administrator).GetAwaiter().GetResult();
			}
		}
	}
}