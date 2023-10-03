using BSFlixFlex.Data;
using BSFlixFlex.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BSFlixFlex.Services;
/// <summary>
/// Service pour gérer les favoris de l'utilisateur.
/// </summary>
public class MyFavoriteService(
    AppDbContext appDbContext,
    ApiTMBDService apiTMBDService,
    AuthenticationStateProvider authenticationStateProvider,
    UserManager<IdentityUser> userManager)
{
    /// <summary>
    /// Récupère la liste des favoris de l'utilisateur.
    /// </summary>
    /// <param name="claimsPrincipal">Le ClaimsPrincipal de l'utilisateur. Doit être fourni si appelé en dehors d'un composant Razor.</param>
    /// <returns>Une liste des favoris de l'utilisateur, ou null si l'utilisateur n'est pas authentifié.</returns>
    public async Task<List<IDiscovryCommonProperty>?> FetchUserFavoritesAsync(ClaimsPrincipal? claimsPrincipal = null)
    {
        List<IDiscovryCommonProperty> myFavoris = [];

        if (await FetchIdentityUserAsync(claimsPrincipal) is IdentityUser user)
        {
            var _myFav = appDbContext.Set<MyFavorite>().Where(x => x.UserId == new Guid(user.Id)).ToArray();
            if (_myFav.Length > 0)
            {
                foreach (var fav in _myFav)
                {
                    var itemResponse = await FetchItemResponseAsync(fav.Cinematography, fav.IdCinematography);
                    if (itemResponse is { IsSuccess: true, Item: var item } && item is not null)
                    {
                        myFavoris.Add(item);
                    }
                }
                return myFavoris;
            }
        }
        return null;
    }

    /// <summary>
    /// Vérifie si un film ou une série est dans les favoris de l'utilisateur.
    /// </summary>
    /// <param name="id">L'ID du film ou de la série.</param>
    /// <param name="cinematography">Le type de cinématographie (Film ou Série).</param>
    /// <param name="claimsPrincipal">Le ClaimsPrincipal de l'utilisateur. Doit être fourni si appelé en dehors d'un composant Razor.</param>
    /// <returns>True si le film ou la série est dans les favoris de l'utilisateur, false sinon.</returns>
    public async Task<bool> IsFavoriteAsync(int id, Cinematography cinematography, ClaimsPrincipal? claimsPrincipal = null)
    {
        if (await FetchIdentityUserAsync(claimsPrincipal) is IdentityUser user)
        {
            var myFavorite = await BuildFavoriteQuery(id,cinematography,user).FirstOrDefaultAsync();
            if (myFavorite != null)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Ajoute un film ou une série aux favoris de l'utilisateur.
    /// </summary>
    /// <param name="id">L'ID du film ou de la série.</param>
    /// <param name="cinematography">Le type de cinématographie (Film ou Série).</param>
    /// <param name="claimsPrincipal">Le ClaimsPrincipal de l'utilisateur. Doit être fourni si appelé en dehors d'un composant Razor.</param>
    public async Task AddToFavoritesAsync(int id, Cinematography cinematography, ClaimsPrincipal? claimsPrincipal = null)
    {
        if (await FetchIdentityUserAsync(claimsPrincipal) is IdentityUser user)
        {
            var itemResponse = await FetchItemResponseAsync(cinematography, id);
            if (itemResponse is { IsSuccess: true, Item: var item } && item is not null)
            {
                var myFavorite = await BuildFavoriteQuery(id, cinematography, user).FirstOrDefaultAsync();
                if (myFavorite is null)
                {
                    await appDbContext.Set<MyFavorite>()
                        .AddAsync(new() { Cinematography = cinematography, UserId = new Guid(user.Id), IdCinematography = id });
                    await appDbContext.SaveChangesAsync();
                }
            }
        }
    }

    /// <summary>
    /// Supprime un film ou une série des favoris de l'utilisateur.
    /// </summary>
    /// <param name="id">L'ID du film ou de la série.</param>
    /// <param name="cinematography">Le type de cinématographie (Film ou Série).</param>
    /// <param name="claimsPrincipal">Le ClaimsPrincipal de l'utilisateur. Doit être fourni si appelé en dehors d'un composant Razor.</param>
    public async Task RemoveFromFavoritesAsync(int id, Cinematography cinematography, ClaimsPrincipal? claimsPrincipal = null)
    {
        if (await FetchIdentityUserAsync(claimsPrincipal) is IdentityUser user)
        {
            var myFavorite = BuildFavoriteQuery(id, cinematography, user);
            appDbContext.RemoveRange(myFavorite);
            await appDbContext.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Récupère l'IdentityUser correspondant au ClaimsPrincipal fourni.
    /// </summary>
    /// <param name="claimsPrincipal">Le ClaimsPrincipal à vérifier. Si null, utilise le ClaimsPrincipal du composant Razor actuel.</param>
    /// <returns>L'IdentityUser correspondant, ou null si le ClaimsPrincipal n'est pas authentifié.</returns>
    private async Task<IdentityUser?> FetchIdentityUserAsync(ClaimsPrincipal? claimsPrincipal)
    {
        ClaimsPrincipal claims;

        if (claimsPrincipal is null)
        {
            // si la demande est fait par razor component
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            claims = authState.User;
        }
        else
            claims = claimsPrincipal;
        if (claims?.Identity is { IsAuthenticated: true, Name: var name } && name != null)
        {
            return await userManager.FindByNameAsync(name);
        }
        return null;
    }

    /// <summary>
    /// Récupère la réponse de l'élément en fonction du type de cinématographie et de l'ID.
    /// </summary>
    /// <param name="cinematography">Le type de cinématographie (Film ou Série).</param>
    /// <param name="id">L'ID du film ou de la série.</param>
    /// <returns>La réponse de l'élément, ou null si le type de cinématographie n'est pas géré.</returns>
    private async Task<ApiItemResponse<IDiscovryCommonProperty>?> FetchItemResponseAsync(Cinematography cinematography, int id)
    {
        return cinematography switch
        {
            Cinematography.Movie => await FetchDetailsAsync<MovieDetails>(cinematography, id),
            Cinematography.Tv => await FetchDetailsAsync<TvShowDetails>(cinematography, id),
            _ => null
        };
    }

    /// <summary>
    /// Récupère les détails d'un film ou d'une série spécifique.
    /// </summary>
    /// <param name="cinematography">Le type de cinématographie (Film ou Série).</param>
    /// <param name="id">L'ID du film ou de la série.</param>
    /// <returns>Les détails du film ou de la série spécifié.</returns>
    private async Task<ApiItemResponse<IDiscovryCommonProperty>> FetchDetailsAsync<T>(Cinematography cinematography, int id) where T : IDiscovryCommonProperty
    {
        var itemResponse1 = await apiTMBDService.FetchItemDetailsAsync<T>(cinematography, id);
        return new() { Item = itemResponse1.Item, IsSuccess = itemResponse1.IsSuccess, Message = itemResponse1.Message };
    }

    /// <summary>
    /// Récupère une requête pour les favoris d'un utilisateur spécifique.
    /// </summary>
    /// <param name="id">L'ID du film ou de la série.</param>
    /// <param name="cinematography">Le type de cinématographie (Film ou Série).</param>
    /// <param name="user">L'utilisateur pour lequel récupérer les favoris.</param>
    /// <returns>Une requête pour les favoris de l'utilisateur spécifié.</returns>
    private IQueryable<MyFavorite> BuildFavoriteQuery(int id, Cinematography cinematography, IdentityUser user)
    {
        return appDbContext.Set<MyFavorite>()
            .Where(x => x.IdCinematography == id && x.Cinematography == cinematography && x.UserId == new Guid(user.Id));
    }
}
