using BSFlixFlex.Client.Shareds.Models;
using BSFlixFlex.Client.Shareds.Models.Cinematographies;
using System.Security.Claims;

namespace BSFlixFlex.Client.Shareds.Interfaces
{
    public interface IMyFavoriteService
    {
        Task AddToFavoritesAsync(int id, Cinematography cinematography, ClaimsPrincipal claimsPrincipal);
        Task<ApiListResponse<IDiscovryCommonProperty>> FetchUserFavoritesAsync(ClaimsPrincipal claimsPrincipal, int clientPageNumber, int clientPageSize = 10);
        Task<bool> IsFavoriteAsync(int id, Cinematography cinematography, ClaimsPrincipal claimsPrincipal);
        Task RemoveFromFavoritesAsync(int id, Cinematography cinematography, ClaimsPrincipal claimsPrincipal);
    }
}