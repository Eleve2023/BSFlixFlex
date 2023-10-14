using BSFlixFlex.Client.Shareds.Interfaces;
using BSFlixFlex.Client.Shareds.Models;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text;

namespace BSFlixFlex.Client.Services
{
    public class MyFavoriteService(HttpClient httpClient) : IMyFavoriteService
    {
        public async Task AddToFavoritesAsync(int id, Cinematography cinematography, ClaimsPrincipal claimsPrincipal)
        {
            var re = new HttpRequestMessage(HttpMethod.Post, $"/api/MyFavorites?id={id}&cinematography={cinematography}");
            _ = await httpClient.SendAsync(re);
        }

        public async Task<ApiListResponse<IDiscovryCommonProperty>> FetchUserFavoritesAsync(ClaimsPrincipal claimsPrincipal, int clientPageNumber, int clientPageSize = 10)
        {
            return await httpClient.GetFromJsonAsync<ApiListResponse<IDiscovryCommonProperty>>($"/api/MyFavorites?page={clientPageNumber}&pageSize={clientPageSize}");
        }

        public async Task<bool> IsFavoriteAsync(int id, Cinematography cinematography, ClaimsPrincipal claimsPrincipal)
        {
            var _ = await httpClient.GetStringAsync($"/api/MyFavorites/{cinematography}?id={id}");
            return bool.Parse(_);
        }

        public async Task RemoveFromFavoritesAsync(int id, Cinematography cinematography, ClaimsPrincipal claimsPrincipal)
        {
            var re = new HttpRequestMessage(HttpMethod.Delete, $"/api/MyFavorites/{cinematography}/{id}");
            _ = await httpClient.SendAsync(re);
        }
    }
}
