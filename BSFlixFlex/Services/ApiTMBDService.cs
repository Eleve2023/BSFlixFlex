using BSFlixFlex.Models;
using System.IO;

namespace BSFlixFlex.Services
{
    public class ApiTMBDService(HttpClient httpClient)
    {
        public async Task<ListResponse<T>> GetTopRateAsync<T>(Cinematography cinematography, int clientPageNumber, int clientPageSize = 10) where T : class
        {
            return await Get<T>("3", cinematography, clientPageNumber, clientPageSize,null,UrlType.TopRate);
        }

        public async Task<ListResponse<T>> GetDiscoverAsync<T>(Cinematography cinematography, int clientPageNumber, int clientPageSize = 10) where T : class
        {
            return await Get<T>("4/discover", cinematography, clientPageNumber, clientPageSize);
        }
        public async Task<ListResponse<T>> GetSearchAsync<T>(Cinematography cinematography, string search, int clientPageNumber, int clientPageSize = 10) where T : class
        {
            return await Get<T>("3/search", cinematography, clientPageNumber, clientPageSize, search);
        }
        public async Task<ItemResponse<T>> GetDetail<T>(Cinematography cinematography, int id) 
        {
            try
            {
                var result = await httpClient.GetFromJsonAsync<T>($"3/{cinematography.ToString().ToLower()}/{id}?language=fr-Fr");
                if (result != null)
                    return new() { Item = result, IsSuccess = true };
                else
                    return new() { IsSuccess = false, Message = "Not found" };
            }
            catch (Exception)
            {
                return new() { IsSuccess = false, Message = "erreur" };
            }

        }
        public async Task<VideoResponse> GetVideos<T>(Cinematography cinematography, int id)
        {
            try
            {
                var result = await httpClient.GetFromJsonAsync<VideoResponse>($"3/{cinematography.ToString().ToLower()}/{id}/videos?language=fr-Fr");
                if (result != null)
                {
                    result.IsSuccess = true;
                    return result;
                }
                else
                    return new() { IsSuccess = false, Message = "Not found" };
            }
            catch (Exception)
            {
                return new() { IsSuccess = false, Message = "erreur" };
            }

        }

        private async Task<ListResponse<T>> Get<T>(string path, Cinematography cinematography, int clientPageNumber, int clientPageSize, string? search = null, UrlType urlType = UrlType.Other) where T : class
        {
            int apiPageNumber = (int)Math.Ceiling((double)(clientPageNumber * clientPageSize) / 20);
            var uriRelatif = urlType switch
            {
                UrlType.TopRate => $"{path}/{cinematography.ToString().ToLower()}/top_rated?page={apiPageNumber}&language=fr-Fr",
                _ => $"{path}/{cinematography.ToString().ToLower()}?page={apiPageNumber}&language=fr-Fr"
            };
            
            if (!string.IsNullOrEmpty(search))
                uriRelatif += $"&query={search}";

            try
            {
                var apiResults = await httpClient.GetFromJsonAsync<DiscoverResponse<T>>(uriRelatif);
                if (apiResults != null)
                {
                    var listResponse = ResolvedList(clientPageNumber, clientPageSize, apiResults);
                    listResponse.IsSuccess = true;
                    return listResponse;
                }
                return new() { IsSuccess = false, Message = "null" };
            }
            catch (Exception)
            {
                return new() { IsSuccess = false, Message = "erreur" };
            }
        }
        private static ListResponse<T> ResolvedList<T>(int clientPageNumber, int clientPageSize, DiscoverResponse<T> apiResults) where T : class
        {
            int apiPageNumber = apiResults.Page;
            List<T> apiPageResults = apiResults.Results.ToList()!;
            int startIndex = (clientPageNumber - 1) * clientPageSize - (apiPageNumber - 1) * 20;
            int endIndex = startIndex + clientPageSize;
            List<T> clientPageResults = apiPageResults.GetRange(startIndex, endIndex - startIndex);
            //var apiTotalPages = apiResults.TotalPages;
            //int clientTotalPages = (int)Math.Ceiling((double)(apiTotalPages * 20) / clientPageSize);
            return new ListResponse<T>
            {
                Items = clientPageResults,
                TotalItems = apiResults.TotalResults,
                IsSuccess = true,
            };
        }

        //public async Task<ListResponse<T>> GetPaginatedResult(int clientPageSize, int clientPageNumber, List<T> apiResults, int apiTotalPages)
        //{
        //    // Calculer le nombre total de pages pour le client
        //    int clientTotalPages = (int)Math.Ceiling((double)(apiTotalPages * 20) / clientPageSize);

        //    // Calculer la page de l'API à partir de laquelle obtenir les résultats
        //    int apiPageNumber = (int)Math.Ceiling((double)(clientPageNumber * clientPageSize) / 20);

        //    // Obtenir les résultats de l'API
        //    // Note : Vous devrez remplacer cette partie par votre propre logique pour obtenir les résultats de l'API
        //    List<T> apiPageResults = apiResults;

        //    // Calculer l'index de début et de fin pour la sous-liste des résultats de l'API
        //    int startIndex = (clientPageNumber - 1) * clientPageSize - (apiPageNumber - 1) * 20;
        //    int endIndex = startIndex + clientPageSize;

        //    // Obtenir la sous-liste des résultats de l'API
        //    List<T> clientPageResults = apiPageResults.GetRange(startIndex, endIndex - startIndex);

        //    // Créer le résultat paginé pour le client
        //    var result = new ListResponse<T>
        //    {
        //        Items = clientPageResults,
        //        TotalItems = clientTotalPages
        //    };

        //    return result;
        //}

    }
}
