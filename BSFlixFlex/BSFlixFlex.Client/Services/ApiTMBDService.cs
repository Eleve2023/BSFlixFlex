using BSFlixFlex.Client.Shareds.Interfaces;
using BSFlixFlex.Client.Shareds.Models;
using Microsoft.Extensions.Primitives;

using System.Net.Http.Json;

namespace BSFlixFlex.Client.Services
{
    public class ApiTMBDService(HttpClient httpClient) : IApiTMBDService
    {
        public async Task<ApiListResponse<T>> FetchDiscoveryItemsAsync<T>(Cinematography cinematography, int clientPageNumber, int clientPageSize = 10) where T : class
        {
            var pathbase = cinematography switch
            {
                Cinematography.Movie => "movie",
                Cinematography.Tv => "tvshow",
                _ => throw new NotSupportedException()
            };
            IEnumerable<KeyValuePair<string, StringValues>> query = [
                   new("ReturnUrl", clientPageNumber.ToString()),
                new("Action", clientPageSize.ToString())];
            return await httpClient.GetFromJsonAsync<ApiListResponse<T>>($"/api/{pathbase}?page={clientPageNumber}");
            //var redirectUrl = UriHelper.BuildRelative(
            //        context.Request.PathBase,
            //        $"/Account/ExternalLogin",
            //        QueryString.Create(query));
        }

        public async Task<ApiItemResponse<T>> FetchItemDetailsAsync<T>(Cinematography cinematography, int id)
        {
            var pathbase = cinematography switch
            {
                Cinematography.Movie => "movie",
                Cinematography.Tv => "tv",
                _ => throw new NotSupportedException()
            };
           
            return await httpClient.GetFromJsonAsync<ApiItemResponse<T>>($"/api/{pathbase}/{id}");
        }

        public async Task<VideoResponse> FetchItemVideosAsync<T>(Cinematography cinematography, int id)
        {
            var pathbase = cinematography switch
            {
                Cinematography.Movie => "movie",
                Cinematography.Tv => "tv",
                _ => throw new NotSupportedException()
            };

            return await httpClient.GetFromJsonAsync<VideoResponse>($"/api/videos/{pathbase}/{id}");
        }

        public async Task<ApiListResponse<T>> FetchTopRatedItemsAsync<T>(Cinematography cinematography, int clientPageNumber, int clientPageSize = 10) where T : class
        {
            var pathbase = cinematography switch
            {
                Cinematography.Movie => "movie",
                Cinematography.Tv => "tvshow",
                _ => throw new NotSupportedException()
            };
            return await httpClient.GetFromJsonAsync<ApiListResponse<T>>($"/api/{pathbase}/top_rated?page={clientPageNumber}");
        }

        public async Task<ApiListResponse<T>> SearchItemsAsync<T>(Cinematography cinematography, string search, int clientPageNumber, int clientPageSize = 10) where T : class
        {
            var pathbase = cinematography switch
            {
                Cinematography.Movie => "movie",
                Cinematography.Tv => "tv",
                _ => throw new NotSupportedException()
            };
            return await httpClient.GetFromJsonAsync<ApiListResponse<T>>($"/api/search/{pathbase}?page={clientPageNumber}");
        }
    }
}
