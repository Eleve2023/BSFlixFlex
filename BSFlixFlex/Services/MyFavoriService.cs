using BSFlixFlex.Data;
using BSFlixFlex.Models;
using BSFlixFlex.Pages;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Security.Claims;
namespace BSFlixFlex.Services;

public class MyFavoriService()
{
    readonly AppDbContext appDbContext;
    readonly HttpClient httpClient;
    private readonly AuthenticationStateProvider authenticationStateProvider;
    readonly UserManager<IdentityUser> userManager;

    public MyFavoriService(AppDbContext appDbContext, HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, UserManager<IdentityUser> userManager) : this()
    {
        this.appDbContext = appDbContext;
        this.httpClient = httpClient;
        this.authenticationStateProvider = authenticationStateProvider;
        this.userManager = userManager;
    }

    public async Task<IdentityUser?> GetIdentityUserAsync(ClaimsPrincipal? claimsPrincipal)
    {
        ClaimsPrincipal claims;

        if (claimsPrincipal is null)
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            claims = authState.User;
        }
        else
            claims = claimsPrincipal;
        if (claims != null)
        {
            var identity = claims.Identity;
            if (identity != null && identity.IsAuthenticated && identity.Name != null)
            {
                return await userManager.FindByNameAsync(identity.Name);
            }
        }
        return null;
    }

    public async Task<List<IDiscovryCommonProperty>?> GetMyListFavorisAsync(ClaimsPrincipal? claimsPrincipal = null)
    {
        List<IDiscovryCommonProperty> myFavoris = [];

        if (await GetIdentityUserAsync(claimsPrincipal) is IdentityUser user)
        {
            var _myFav = appDbContext.Set<MyFavorite>().Where(x => x.UserId == new Guid(user.Id)).ToArray();
            if (_myFav.Length > 0)
            {
                foreach (var fav in _myFav)
                {
                    IDiscovryCommonProperty? item = null;
                    try
                    {
                        if (fav.Cinematography == Cinematography.Movie)
                        {
                            item = await httpClient.GetFromJsonAsync<Movie>($"3/{fav.Cinematography.ToString().ToLower()}/{fav.IdCinematography}?language=fr-Fr");
                        }
                        if (fav.Cinematography == Cinematography.Tv)
                        {
                            item = await httpClient.GetFromJsonAsync<TvShow>($"3/{fav.Cinematography.ToString().ToLower()}/{fav.IdCinematography}?language=fr-Fr");
                        }
                    }
                    catch (Exception)
                    {
                       
                    }
                    if (item != null)
                        myFavoris.Add(item);
                }
                return myFavoris;
            }
        }
        return null;

    }

    public async Task<bool> IsFavoriAsync(int id, Cinematography cinematography, ClaimsPrincipal? claimsPrincipal = null)
    {
        if (await GetIdentityUserAsync(claimsPrincipal) is IdentityUser user)
        {
            var myFavorite = await appDbContext.Set<MyFavorite>().Where(x => x.IdCinematography == id && x.Cinematography == cinematography && x.UserId == new Guid(user.Id)).FirstOrDefaultAsync();
            if (myFavorite != null)
            {
                return true;
            }
        }
        return false;
    }
    public async void AddFavori(int id, Cinematography cinematography, ClaimsPrincipal? claimsPrincipal = null)
    {
        if (await GetIdentityUserAsync(claimsPrincipal) is IdentityUser user)
        {
            DiscovryCommonProperty? item;
            try
            {
                item = await httpClient.GetFromJsonAsync<DiscovryCommonProperty>($"3/{cinematography.ToString().ToLower()}/{id}?language=fr-Fr");
            }
            catch (Exception)
            {

                throw;
            }
            if (item != null)
            {
                await appDbContext.Set<MyFavorite>().AddAsync(new() { Cinematography = cinematography, UserId = new Guid(user.Id), IdCinematography = id });
                await appDbContext.SaveChangesAsync();
            }
        }
    }
    public async void Remove(int id, Cinematography cinematography, ClaimsPrincipal? claimsPrincipal = null)
    {

        if (await GetIdentityUserAsync(claimsPrincipal) is IdentityUser user)
        {
            var myFavorites = appDbContext.Set<MyFavorite>().Where(x => x.IdCinematography == id && x.Cinematography == cinematography && x.UserId == new Guid(user.Id));
            appDbContext.RemoveRange(myFavorites);
            await appDbContext.SaveChangesAsync();
        }
    }
}
