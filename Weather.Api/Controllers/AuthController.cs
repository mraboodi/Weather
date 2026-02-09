using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Weather.Models.DTOs.Identity;
using Weather.Models.Entities.Identity;

namespace Weather.Api.Controllers
{
	/// <summary>
	/// Handles user authentication and registration operations.
	/// </summary>
	/// <remarks>
	/// Responsibilities:
	/// - User registration with optional role assignment
	/// - User login and JWT token generation
	/// 
	/// JWT token includes:
	/// - Name, NameIdentifier, FirstName, LastName, Jti
	/// - Assigned user roles
	/// </remarks>
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController(UserManager<ApplicationUser> userManager, IConfiguration config) : ControllerBase
	{
		[HttpPost("Register")]
		public async Task<IActionResult> Register(RegisterDto model)
		{
			var user = new ApplicationUser
			{
				FirstName = model.FirstName,
				LastName = model.LastName,
				UserName = model.Email,
				Email = model.Email,
			};

			var result = await userManager.CreateAsync(user, model.Password);

			if (!result.Succeeded)
			{
				var errors = result.Errors
					.GroupBy(e => e.Code)
					.ToDictionary(
						g => g.Key,
						g => g.Select(e => e.Description).ToArray()
					);

				return ValidationProblem(new ValidationProblemDetails(errors)
				{
					Title = "Registration failed",
					Status = StatusCodes.Status400BadRequest
				});
			}

			var assignedRole = UserRoles.SimpleUser;

			if (User.Identity?.IsAuthenticated == true)
			{
				var roles = User.Claims
					.Where(c => c.Type == ClaimTypes.Role)
					.Select(c => c.Value);

				if (model.Role == UserRoles.SuperUser &&
					roles.Any(r => r is UserRoles.Administrator or UserRoles.SuperUser))
					assignedRole = UserRoles.SuperUser;
			}

			await userManager.AddToRoleAsync(user, assignedRole);

			return Ok(new { RoleAssigned = assignedRole, Message = "User registered successfully" });
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login(LoginDto model)
		{
			var user = await userManager.FindByEmailAsync(model.Email);
			if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
			{
				var userRoles = await userManager.GetRolesAsync(user);

				var authClaims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.UserName!),
					new Claim(ClaimTypes.NameIdentifier, user.Id),
					new Claim("FirstName", user.FirstName ?? ""),
					new Claim("LastName", user.LastName ?? ""),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
				};

				foreach (var role in userRoles)
				{
					authClaims.Add(new Claim(ClaimTypes.Role, role));
				}

				var token = GenerateToken(authClaims);

				return Ok(new LoginResponseDto(
							new JwtSecurityTokenHandler().WriteToken(token),
							token.ValidTo,
							userRoles.FirstOrDefault(),
							user.FirstName ?? "",
							user.LastName ?? ""
						));
			}
			return Unauthorized();
		}


		private JwtSecurityToken GenerateToken(List<Claim> authClaims)
		{
			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecretKey"]!));

			return new JwtSecurityToken(
				issuer: config["JWT:Issuer"],
				audience: config["JWT:Audience"],
				expires: DateTime.Now.AddHours(3),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
			);
		}
	}
}
