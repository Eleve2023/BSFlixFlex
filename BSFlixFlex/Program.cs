using BSFlixFlex.Areas.Identity;
using BSFlixFlex.Data;
using BSFlixFlex.MiniApis;
using BSFlixFlex.Models;
using BSFlixFlex.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("DataSource=users.db"));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("DataSource=app.db"));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = IdentityConstants.ApplicationScheme;
    o.DefaultSignInScheme = IdentityConstants.ExternalScheme;

})
    .AddBearerToken(IdentityConstants.BearerScheme)
    .AddIdentityCookies(o => { });
builder.Services.AddAuthorizationBuilder().AddPolicy("CookiesOrBearer", policy =>
{
    policy.AddAuthenticationSchemes(IdentityConstants.ApplicationScheme, IdentityConstants.BearerScheme);
    policy.RequireAuthenticatedUser();
});

builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders()
    .AddApiEndpoints();


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
var token = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI2NzUxMTU1Y2QxZDQ1NjczMGJlOTg1OTViY2RlZTQ4NSIsInN1YiI6IjY1MTJkMDY0ZTFmYWVkMDEzYTBjOGYxYyIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.eWjXyaDpeLGJPrWFfB_ZnAwjz2NldXIsPxKk4D-6tVM";
builder.Services.AddHttpClient("", client =>
{
    client.BaseAddress = new Uri("https://api.themoviedb.org/");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    client.Timeout = TimeSpan.FromSeconds(130);
});
builder.Services.AddTransient<MyFavoriService>();
builder.Services.AddTransient<ApiTMBDService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<IdentityUser>();
app.MapGet("/api/test", (ClaimsPrincipal user) => user.Identity.Name)
    .RequireAuthorization("CookiesOrBearer");
app.MiniApiApp();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
