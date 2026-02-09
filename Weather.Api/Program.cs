using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Weather.Api.Data;
using Weather.Api.Data.DBInitializer;
using Weather.Api.Interfaces;
using Weather.Api.Services;
using Weather.Models.Configuration;
using Weather.Models.Entities.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 1. Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");				// for MySql
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))); // for MySql
//var connectionString = builder.Configuration.GetConnectionString("SQLServerConnection");			// for SQL Server database
//builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));	// for SQL Server database

// 2. Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders();

// 3. Authentication (JWT)
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option => {
		option.RequireHttpsMetadata = true;
		option.TokenValidationParameters = new TokenValidationParameters
		{
			// defines the rules for verification
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"] ?? "low-security")),			
			ValidIssuer = builder.Configuration["JWT:Issuer"],
			ValidAudience = builder.Configuration["JWT:Audience"],
			ClockSkew = TimeSpan.Zero, 
		};
		
		option.RequireHttpsMetadata = true;
		option.RequireHttpsMetadata = true;
	});

builder.Services.AddHttpClient();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<ISearchCityService, SearchCityService>();
builder.Services.Configure<WeatherOptions>(builder.Configuration.GetSection("WeatherOptions"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();								// To Enable RESTFUL
builder.Services.AddHttpClient();								// For calling external APIs. i.e., Weather API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();								// For API Documentation purpose
builder.Services.AddScoped<IDBInitializer, DBInitializer>();	// For Db initilization and seeding

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); // we can do  .WithOrigins("http://localhost:3000", "https://example.com")
} else
{
	var clientAcceptedOrigin = builder.Configuration.GetConnectionString("clientAcceptedOrigin");
	app.UseHttpsRedirection();
	app.UseCors(options => (clientAcceptedOrigin.IsNullOrEmpty()? 
								options.AllowAnyOrigin():
								options.WithOrigins(clientAcceptedOrigin!)
						   ).AllowAnyMethod().AllowAnyHeader());
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
SeedDatabase();

app.Run();


void SeedDatabase()
{
	// Ensures that a disposable instance (var scope) is disposed even if an
	// exception occurs within the block of the using statement.
	using var scope = app.Services.CreateScope();
	var dbInitializer = scope.ServiceProvider.GetRequiredService<IDBInitializer>(); // 'IDBInitializer' Service need to be registered above using
																					// 'builder'. We cann't get the service if it is not registered.
	dbInitializer.Initializer();
}

