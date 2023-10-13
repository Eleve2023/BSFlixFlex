using BSFlixFlex.Client;
using BSFlixFlex.Client.Services;
using BSFlixFlex.Client.Shareds.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

builder.Services.AddHttpClient("", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services.AddTransient<IApiTMBDService, ApiTMBDService>();
builder.Services.AddTransient<IMyFavoriteService, MyFavoriteService>();


await builder.Build().RunAsync();
