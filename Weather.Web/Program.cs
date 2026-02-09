using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Weather.Models.Configuration;
using Weather.Web.Components;
using Weather.Web.Interfaces;
using Weather.Web.Interfaces.Identity;
using Weather.Web.Services;
using Weather.Web.Services.Favorite;
using Weather.Web.Services.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<IAccessTokenProvider, AccessTokenProvider>();
builder.Services.AddScoped<AuthStateService>();
builder.Services.AddScoped<IAuthStateService, AuthStateService>(); // (sp => sp.GetRequiredService<AuthStateService>());
builder.Services.AddScoped<JwtAuthHandler>();
builder.Services.Configure<WeatherOptions>(builder.Configuration.GetSection("WeatherOptions"));

var apiBase = builder.Configuration["WeatherApi:BaseAddress"] ?? throw new NotImplementedException("base address reqired");
builder.Services.AddHttpClient<IForcastService, ForcastService>(client =>
{
	client.BaseAddress = new Uri(apiBase);
	client.Timeout = TimeSpan.FromSeconds(20);
})
.AddHttpMessageHandler<JwtAuthHandler>();

builder.Services.AddHttpClient<ISearchCityService, SearchCityService>(client =>
{
	client.BaseAddress = new Uri(apiBase);
	client.Timeout = TimeSpan.FromSeconds(20);
})
.AddHttpMessageHandler<JwtAuthHandler>();

builder.Services.AddHttpClient<IFavoriteService, FavoriteService>(client =>
{
	client.BaseAddress = new Uri(apiBase);
	client.Timeout = TimeSpan.FromSeconds(20);
})
.AddHttpMessageHandler<JwtAuthHandler>();

builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
	client.BaseAddress = new Uri(apiBase);
	client.Timeout = TimeSpan.FromSeconds(20);
})
.AddHttpMessageHandler<JwtAuthHandler>();

builder.Services.AddScoped<IFavoriteState, FavoriteState>();

// Authentication (we may remove it)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.Cookie.Name = "user_token";
		options.LoginPath = "/login";
		options.AccessDeniedPath = "/unauthorized";
		options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
	});

// Authorization
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();


var app = builder.Build();

var supportedCultures = new[] { CultureInfo.InvariantCulture };
var localizationOptions = new RequestLocalizationOptions
{
	DefaultRequestCulture = new RequestCulture(CultureInfo.InvariantCulture),
	SupportedCultures = supportedCultures,
	SupportedUICultures = supportedCultures
};
app.UseRequestLocalization(localizationOptions);


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication(); // though this is included by default if the line below was added (can be cleared, I think newer .net only)
app.UseAuthorization();


app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
