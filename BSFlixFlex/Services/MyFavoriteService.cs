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
    IApiTMBDService apiTMBDService
   ) : IMyFavoriteService
{
    /// <summary>
    /// Récupère la liste des favoris de l'utilisateur.
    /// </summary>
    /// <param name="claimsPrincipal">Le ClaimsPrincipal de l'utilisateur. Doit être fourni si appelé en dehors d'un composant Razor.</param>
    /// <returns>Une liste des favoris de l'utilisateur, ou null si l'utilisateur n'est pas authentifié.</returns>
    public async Task<ApiListResponse<IDiscovryCommonProperty>> FetchUserFavoritesAsync(ClaimsPrincipal claimsPrincipal, int clientPageNumber, int clientPageSize = 10)
    {
        List<IDiscovryCommonProperty> myFavoris = [];
        var skip = clientPageSize * (clientPageNumber - 1);
        if (claimsPrincipal.Identity is { IsAuthenticated: true })
        {
            var userIdentifier = UserIdentifier(claimsPrincipal);

            var _myFav = appDbContext.Set<MyFavorite>().Where(x => x.UserId == new Guid(userIdentifier)).Skip(skip).Take(clientPageSize).ToArray();
            var totalItems = appDbContext.Set<MyFavorite>().Where(x => x.UserId == new Guid(userIdentifier)).Count();
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
            }
            return new() { IsSuccess = true, Items = myFavoris, TotalItems = totalItems };
        }
        else
            return new() { IsSuccess = false, Message = "Not Identifier" };
    }

    /// <summary>
    /// Vérifie si un film ou une série est dans les favoris de l'utilisateur.
    /// </summary>
    /// <param name="id">L'ID du film ou de la série.</param>
    /// <param name="cinematography">Le type de cinématographie (Film ou Série).</param>
    /// <param name="claimsPrincipal">Le ClaimsPrincipal de l'utilisateur. Doit être fourni si appelé en dehors d'un composant Razor.</param>
    /// <returns>True si le film ou la série est dans les favoris de l'utilisateur, false sinon.</returns>
    public async Task<bool> IsFavoriteAsync(int id, Cinematography cinematography, ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal.Identity is { IsAuthenticated: true })
        {
            var userIdentifier = UserIdentifier(claimsPrincipal);
            var myFavorite = await BuildFavoriteQuery(id, cinematography, userIdentifier).FirstOrDefaultAsync();
            if (myFavorite != null)
            {
                return true;
            }
            else { return false; }
        }
        else
            throw new Exception();
    }

    /// <summary>
    /// Ajoute un film ou une série aux favoris de l'utilisateur.
    /// </summary>
    /// <param name="id">L'ID du film ou de la série.</param>
    /// <param name="cinematography">Le type de cinématographie (Film ou Série).</param>
    /// <param name="claimsPrincipal">Le ClaimsPrincipal de l'utilisateur. Doit être fourni si appelé en dehors d'un composant Razor.</param>
    public async Task AddToFavoritesAsync(int id, Cinematography cinematography, ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal.Identity is { IsAuthenticated: true })
        {
            var userIdentifier = UserIdentifier(claimsPrincipal);

            var itemResponse = await FetchItemResponseAsync(cinematography, id);
            if (itemResponse is { IsSuccess: true, Item: var item } && item is not null)
            {
                var myFavorite = await BuildFavoriteQuery(id, cinematography, userIdentifier).FirstOrDefaultAsync();
                if (myFavorite is null)
                {
                    await appDbContext.Set<MyFavorite>()
                        .AddAsync(new() { Cinematography = cinematography, UserId = new Guid(userIdentifier), IdCinematography = id });
                    await appDbContext.SaveChangesAsync();
                }
            }
            else throw new Exception();
        }
        else
            throw new Exception();

    }

    /// <summary>
    /// Supprime un film ou une série des favoris de l'utilisateur.
    /// </summary>
    /// <param name="id">L'ID du film ou de la série.</param>
    /// <param name="cinematography">Le type de cinématographie (Film ou Série).</param>
    /// <param name="claimsPrincipal">Le ClaimsPrincipal de l'utilisateur. Doit être fourni si appelé en dehors d'un composant Razor.</param>
    public async Task RemoveFromFavoritesAsync(int id, Cinematography cinematography, ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal.Identity is { IsAuthenticated: true })
        {
            var userIdentifier = UserIdentifier(claimsPrincipal);
            var myFavorite = BuildFavoriteQuery(id, cinematography, userIdentifier);
            appDbContext.RemoveRange(myFavorite);
            await appDbContext.SaveChangesAsync();
        }
        else
            throw new Exception();
    }


    private static string UserIdentifier(ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("claim is not present");
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
    private IQueryable<MyFavorite> BuildFavoriteQuery(int id, Cinematography cinematography, string userIdentifier)
    {
        return appDbContext.Set<MyFavorite>()
            .Where(x => x.IdCinematography == id && x.Cinematography == cinematography && x.UserId == new Guid(userIdentifier));
    }
}
