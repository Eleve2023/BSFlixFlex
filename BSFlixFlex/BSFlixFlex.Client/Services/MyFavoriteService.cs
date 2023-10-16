using BSFlixFlex.Client.Shareds.Interfaces;
using BSFlixFlex.Client.Shareds.Models;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text;
using BSFlixFlex.Client.Shareds.Models.Cinematographies;

namespace BSFlixFlex.Client.Services
{
    public class MyFavoriteService(HttpClient httpClient) : IMyFavoriteService
    {
        public async Task AddToFavoritesAsync(int id, Cinematography cinematography, ClaimsPrincipal claimsPrincipal)
        {
            var requestMessage = GetRequestMessageOfPostOrDelete(HttpMethod.Post, id, cinematography);
            _ = await httpClient.SendAsync(requestMessage);
        }



        public async Task<ApiListResponse<IDiscovryCommonProperty>> FetchUserFavoritesAsync(ClaimsPrincipal claimsPrincipal, int clientPageNumber, int clientPageSize = 10)
        {
            return await httpClient.GetFromJsonAsync<ApiListResponse<IDiscovryCommonProperty>>($"/api/MyFavorites?page={clientPageNumber}&pageSize={clientPageSize}");
        }

        public async Task<bool> IsFavoriteAsync(int id, Cinematography cinematography, ClaimsPrincipal claimsPrincipal)
        {
            var _ = await httpClient.GetStringAsync($"/api/MyFavorites/{cinematography}/{id}");
            return bool.Parse(_);
        }

        public async Task RemoveFromFavoritesAsync(int id, Cinematography cinematography, ClaimsPrincipal claimsPrincipal)
        {
            var requestMessage = GetRequestMessageOfPostOrDelete(HttpMethod.Delete, id, cinematography);
            _ = await httpClient.SendAsync(requestMessage);
        }

        private static HttpRequestMessage GetRequestMessageOfPostOrDelete(HttpMethod post, int id, Cinematography cinematography)
        {
            Dictionary<string, string> form = new()
            {
                {"cinematography",cinematography.ToString()},
                {"id", id.ToString() }
            };
            var formContent = new FormUrlEncodedContent(form);
            var requestMessage = new HttpRequestMessage(post, $"/api/MyFavorites")
            {
                Content = formContent
            };
            return requestMessage;
        }
    }
}
