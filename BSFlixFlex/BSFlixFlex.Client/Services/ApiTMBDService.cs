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
                Cinematography.Movie => API_PATH_MOVIE,
                Cinematography.Tv => API_PATH_TVSHOW,
                _ => throw new NotSupportedException()
            };

            return await httpClient.GetFromJsonAsync<ApiListResponse<T>>($"/{pathbase}?page={clientPageNumber}");
            //IEnumerable<KeyValuePair<string, StringValues>> query = [
            //       new("ReturnUrl", clientPageNumber.ToString()),
            //    new("Action", clientPageSize.ToString())];
            //var redirectUrl = UriHelper.BuildRelative(
            //        context.Request.PathBase,
            //        $"/Account/ExternalLogin",
            //        QueryString.Create(query));
        }

        public async Task<ApiItemResponse<T>> FetchItemDetailsAsync<T>(Cinematography cinematography, int id)
        {
            var pathbase = cinematography switch
            {
                Cinematography.Movie => API_PATH_MOVIE_DETAIL,
                Cinematography.Tv => API_PATH_TVSHOW_DETAIL,
                _ => throw new NotSupportedException()
            };
           
            return await httpClient.GetFromJsonAsync<ApiItemResponse<T>>($"/{pathbase}/{id}");
        }

        public async Task<VideoResponse> FetchItemVideosAsync<T>(Cinematography cinematography, int id)
        {
            var pathbase = cinematography switch
            {
                Cinematography.Movie => API_PATH_VIDEO_MOVIE,
                Cinematography.Tv => API_PATH_VIDEO_TVSHOW,
                _ => throw new NotSupportedException()
            };

            return await httpClient.GetFromJsonAsync<VideoResponse>($"/{pathbase}/{id}");
        }

        public async Task<ApiListResponse<T>> FetchTopRatedItemsAsync<T>(Cinematography cinematography, int clientPageNumber, int clientPageSize = 10) where T : class
        {
            var pathbase = cinematography switch
            {
                Cinematography.Movie => API_PATH_MOVIE_TOP_RATED,
                Cinematography.Tv => API_PATH_TVSHOW_TOP_RATED,
                _ => throw new NotSupportedException()
            };
            return await httpClient.GetFromJsonAsync<ApiListResponse<T>>($"/{pathbase}?page={clientPageNumber}");
        }

        public async Task<ApiListResponse<T>> SearchItemsAsync<T>(Cinematography cinematography, string search, int clientPageNumber, int clientPageSize = 10) where T : class
        {
            var pathbase = cinematography switch
            {
                Cinematography.Movie => API_PATH_SEARCH_MOVIE,
                Cinematography.Tv => API_PATH_SEARCH_TVSHOW,
                _ => throw new NotSupportedException()
            };
            return await httpClient.GetFromJsonAsync<ApiListResponse<T>>($"/{pathbase}?search={search}&&page={clientPageNumber}");
        }
    }
}
