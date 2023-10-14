using BSFlixFlex.Client.Shareds.Interfaces;
using BSFlixFlex.Client.Shareds.Models;
using System.Net.Http.Json;
using System.Security.Claims;

namespace BSFlixFlex.Client.Services
{
    public class MyFavoriteService(HttpClient httpClient) : IMyFavoriteService
    {
        public Task AddToFavoritesAsync(int id, Cinematography cinematography, ClaimsPrincipal claimsPrincipal)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiListResponse<MyFavoriteItem>> FetchUserFavoritesAsync(ClaimsPrincipal claimsPrincipal, int clientPageNumber, int clientPageSize = 10)
        {
            var _= await httpClient.GetFromJsonAsync<ApiListResponse<MyFavoriteItem>>($"/api/MyFavorites?page={clientPageNumber}&pageSize={clientPageSize}");
            return _;
        }

        public async Task<bool> IsFavoriteAsync(int id, Cinematography cinematography, ClaimsPrincipal claimsPrincipal)
        {
            var _ = await httpClient.GetStringAsync($"/api/MyFavorites/{cinematography}?id={id}");
            return bool.Parse(_);
        }

        public Task RemoveFromFavoritesAsync(int id, Cinematography cinematography, ClaimsPrincipal claimsPrincipal)
        {
            throw new NotImplementedException();
        }
    }
}
