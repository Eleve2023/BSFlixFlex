using BSFlixFlex.Models;
using System.IO;

namespace BSFlixFlex.Services
{
    /// <summary>
    /// Classe de service pour interagir avec l'API TMBD.
    /// </summary>
    public class ApiTMBDService(HttpClient httpClient)
    {
        /// <summary>
        /// Récupère la liste des films ou séries les mieux notés.
        /// </summary>
        /// <param name="cinematography">Type de cinématographie (Film ou Série).</param>
        /// <param name="clientPageNumber">Numéro de page demandé par le client.</param>
        /// <param name="clientPageSize">Taille de page demandée par le client.</param>
        /// <returns>Une liste des films ou séries les mieux notés.</returns>
        public async Task<ApiListResponse<T>> FetchTopRatedItemsAsync<T>(Cinematography cinematography, int clientPageNumber, int clientPageSize = 10) where T : class
        {
            return await FetchListResponseAsync<T>("3", cinematography, clientPageNumber, clientPageSize,null,UrlType.TopRate);
        }

        /// <summary>
        /// Récupère la liste des films ou séries.
        /// </summary>
        /// <param name="cinematography">Type de cinématographie (Film ou Série).</param>
        /// <param name="clientPageNumber">Numéro de page demandé par le client.</param>
        /// <param name="clientPageSize">Taille de page demandée par le client.</param>
        /// <returns>Une liste des films ou séries les mieux notés.</returns>
        public async Task<ApiListResponse<T>> FetchDiscoveryItemsAsync<T>(Cinematography cinematography, int clientPageNumber, int clientPageSize = 10) where T : class
        {
            return await FetchListResponseAsync<T>("4/discover", cinematography, clientPageNumber, clientPageSize);
        }

        /// <summary>
        /// Récupère une liste de films ou séries correspondant à un terme de recherche.
        /// </summary>
        /// <param name="cinematography">Type de cinématographie (Film ou Série).</param>
        /// <param name="search">Terme de recherche.</param>
        /// <param name="clientPageNumber">Numéro de page demandé par le client.</param>
        /// <param name="clientPageSize">Taille de page demandée par le client.</param>
        /// <returns>Une liste de films ou séries correspondant au terme de recherche.</returns>
        public async Task<ApiListResponse<T>> SearchItemsAsync<T>(Cinematography cinematography, string search, int clientPageNumber, int clientPageSize = 10) where T : class
        {
            return await FetchListResponseAsync<T>("3/search", cinematography, clientPageNumber, clientPageSize, search);
        }

        /// <summary>
        /// Récupère les détails d'un film ou d'une série spécifique.
        /// </summary>
        /// <param name="cinematography">Type de cinématographie (Film ou Série).</param>
        /// <param name="id">Identifiant du film ou de la série.</param>
        /// <returns>Les détails du film ou de la série spécifié.</returns>
        public async Task<ApiItemResponse<T>> FetchItemDetailsAsync<T>(Cinematography cinematography, int id) 
        {
            try
            {
                CheckTypeAndCinematographyOfDetail(typeof(T), cinematography);
                var result = await httpClient.GetFromJsonAsync<T>($"3/{cinematography.ToString().ToLower()}/{id}?language=fr-Fr");
                if (result != null)
                    return new() { Item = result, IsSuccess = true };
                else
                    return new() { IsSuccess = false, Message = "Not found" };
            }            
            catch (HttpRequestException)
            {
                return new() { IsSuccess = false, Message = "erreur" };
            }
        }

        /// <summary>
        /// Récupère les vidéos associées à un film ou une série spécifique.
        /// </summary>
        /// <param name="cinematography">Type de cinématographie (Film ou Série).</param>
        /// <param name="id">Identifiant du film ou de la série.</param>
        /// <returns>Les vidéos associées au film ou à la série spécifié.</returns>
        public async Task<VideoResponse> FetchItemVideosAsync<T>(Cinematography cinematography, int id)
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

        /// <summary>
        /// Récupère une liste de films ou séries en fonction du chemin spécifié.
        /// </summary>
        /// <param name="path">Chemin pour la requête API.</param>
        /// <param name="cinematography">Type de cinématographie (Film ou Série).</param>
        /// <param name="clientPageNumber">Numéro de page demandé par le client.</param>
        /// <param name="clientPageSize">Taille de page demandée par le client.</param>
        /// <param name="search">Terme de recherche optionnel.</param>
        /// <param name="urlType">Type d'URL (TopRate ou autre).</param>
        /// <returns>Une liste de films ou séries correspondant aux critères spécifiés.</returns>
        private async Task<ApiListResponse<T>> FetchListResponseAsync<T>(string path, Cinematography cinematography, int clientPageNumber, int clientPageSize, string? search = null, UrlType urlType = UrlType.Other) where T : class
        {
            CheckTypeAndCinematographyOfDiscover(typeof(T), cinematography);
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

        /// <summary>
        /// Résout la liste des résultats de l'API en fonction de la pagination du client.
        /// </summary>
        /// <param name="clientPageNumber">Numéro de page demandé par le client.</param>
        /// <param name="clientPageSize">Taille de page demandée par le client.</param>
        /// <param name="apiResults">Résultats renvoyés par l'API.</param>
        /// <returns>Une liste de films ou séries correspondant à la pagination du client.</returns>
        private static ApiListResponse<T> ResolvedList<T>(int clientPageNumber, int clientPageSize, DiscoverResponse<T> apiResults) where T : class
        {
            int apiPageNumber = apiResults.Page;
            List<T> apiPageResults = apiResults.Results.ToList()!;
            int startIndex = (clientPageNumber - 1) * clientPageSize - (apiPageNumber - 1) * 20;
            int endIndex = startIndex + clientPageSize;
            List<T> clientPageResults = apiPageResults.GetRange(startIndex, endIndex - startIndex);
            return new ApiListResponse<T>
            {
                Items = clientPageResults,
                TotalItems = apiResults.TotalResults,
                IsSuccess = true,
            };
        }

        private void CheckTypeAndCinematographyOfDiscover(Type type, Cinematography cinematography)
        {
            var _cinematography = type switch
            {
                Type t when t == typeof(TvShow) => Cinematography.Tv,                
                Type t when t == typeof(Movie) => Cinematography.Movie,                
                _ => throw new NotSupportedException()
            };
            if (cinematography != _cinematography)
                throw new Exception();
        }
        private void CheckTypeAndCinematographyOfDetail(Type type, Cinematography cinematography)
        {
            var _cinematography = type switch
            {                
                Type t when t == typeof(TvShowDetails) => Cinematography.Tv,                
                Type t when t == typeof(MovieDetails) => Cinematography.Movie,
                _ => throw new NotSupportedException()
            };
            if (cinematography != _cinematography)
                throw new Exception();
        }
    }
}
