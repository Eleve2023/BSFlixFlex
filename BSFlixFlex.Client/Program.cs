
using BSFlixFlex.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Web;
using BSFlixFlex.Client.Services;
using BSFlixFlex.Client.Shareds.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);


builder.Services.AddHttpClient("", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

builder.Services.AddTransient<IApiTMBDService, ApiTMBDService>();
builder.Services.AddTransient<IMyFavoriteService, MyFavoriteService>();


await builder.Build().RunAsync();