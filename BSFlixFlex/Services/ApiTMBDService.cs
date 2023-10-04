﻿using BSFlixFlex.Exceptions;
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
            return await FetchListResponseAsync<T>("3", cinematography, clientPageNumber, clientPageSize, null, UrlType.TopRate);
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
                VerifyDetailItemTypeAndCinematography(typeof(T), cinematography);
                var result = await httpClient.GetFromJsonAsync<T>($"3/{cinematography.ToString().ToLower()}/{id}?language=fr-Fr");
                if (result != null)
                    return new() { Item = result, IsSuccess = true };
                else
                    return new() { IsSuccess = false, Message = "Not found" };
            }
            catch (HttpRequestException)
            {
                throw;
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
                    return new() { IsSuccess = false, Message = "Null" };
            }
            catch (Exception)
            {
                throw;
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
            VerifyDiscoverItemTypeAndCinematography(typeof(T), cinematography);
            if (20 < clientPageSize)
                throw new PageSizeMismatchException();
            int apiPageNumber = (int)Math.Ceiling((double)(clientPageNumber * clientPageSize) / 20);

            Func<int, string> getRequestUri = (int apiPageNumber) =>
            {
                var uriRelatif = urlType switch
                {
                    UrlType.TopRate => $"{path}/{cinematography.ToString().ToLower()}/top_rated?page={apiPageNumber}&language=fr-Fr",
                    _ => $"{path}/{cinematography.ToString().ToLower()}?page={apiPageNumber}&language=fr-Fr"
                };

                if (!string.IsNullOrEmpty(search))
                    uriRelatif += $"&query={search}";

                return uriRelatif;
            };
            return await ResolvedList<T>(clientPageNumber, clientPageSize, getRequestUri);
        }

        private async Task<DiscoverResponse<T>?> GetDiscoverResponseAsync<T>(int apiPageNumber, Func<int, string> getRequestUri) where T : class
        {
            DiscoverResponse<T>? result = null;
            try
            {
                result = await httpClient.GetFromJsonAsync<DiscoverResponse<T>>(getRequestUri(apiPageNumber));
            }
            catch (HttpRequestException m)
            {
                if (m.Message.Contains("422"))
                {
                    result = await HandleApiPaginationErrors<T>(apiPageNumber, getRequestUri);
                }
                else
                    throw;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                // Gérer les limitations de l'API TMBD
                if (result?.TotalPages > 500)
                {
                    result.TotalPages = 500;
                    result.TotalResults = 10000;
                }
            }
            return result;
        }

        /// <summary>
        /// Correction du numéro de page de l'API pour gérer les limites de pagination de l'API.
        /// Par exemple, si l'API TMDB a un total de 500 pages et que la taille de page du client est 3,
        /// alors une demande pour la page 3334 entraînerait un numéro de page API de 501.
        /// Cependant, la page 501 n'existe pas car l'API a seulement 500 pages.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiPageNumber"></param>
        /// <param name="getRequestUri">Fonction pour obtenir l'URI de requête.</param>
        /// <returns></returns>
        private async Task<DiscoverResponse<T>?> HandleApiPaginationErrors<T>(int apiPageNumber, Func<int, string> getRequestUri) where T : class
        {
            var apiResultsTest = await GetDiscoverResponseAsync<T>(1, getRequestUri);
            if (apiResultsTest != null)
            {
                if (apiPageNumber == apiResultsTest.TotalPages + 1)
                {
                    apiPageNumber--;
                    return await GetDiscoverResponseAsync<T>(apiPageNumber, getRequestUri);
                }
                else
                {

                }
            }
            return null;
        }

        /// <summary>
        /// Résout la liste des résultats de l'API en fonction de la pagination du client.
        /// </summary>
        /// <param name="clientPageNumber">Numéro de page demandé par le client.</param>
        /// <param name="clientPageSize">Taille de page demandée par le client.</param>
        /// <param name="getRequestUri">Fonction pour obtenir l'URI de requête.</param>
        /// <returns>Une liste de films ou séries correspondant à la pagination du client.</returns>
        private async Task<ApiListResponse<T>> ResolvedList<T>(int clientPageNumber, int clientPageSize, Func<int, string> getRequestUri) where T : class
        {
            // Calculer le numéro de page API
            int apiPageNumber = (int)Math.Ceiling((double)(clientPageNumber * clientPageSize) / 20);

            //partie récupération des donnes dans Api TMBD 
             var apiResults = await GetDiscoverResponseAsync<T>(apiPageNumber, getRequestUri);
            
            //partie création de la liste clientPageResult
            if (apiResults == null)
                return new() { IsSuccess = false, Message = "null" };
            apiPageNumber = apiResults.Page;
            List<T> apiPageResults = apiResults.Results.ToList()!;
            int startIndex = (clientPageNumber - 1) * clientPageSize - (apiPageNumber - 1) * 20;
            int endIndex = startIndex + clientPageSize;
            List<T> clientPageResults = [];
            if (startIndex < 0)
            {
                clientPageResults = apiPageResults.GetRange(0,clientPageSize - Math.Abs( startIndex));
                var apiPageResults2 = (await GetDiscoverResponseAsync<T>(apiPageNumber - 1, getRequestUri))!.Results.ToList();
                apiPageResults2 = apiPageResults2.GetRange(apiPageResults2.Count - Math.Abs(startIndex), Math.Abs(startIndex));
                apiPageResults2.AddRange(clientPageResults);
                clientPageResults = apiPageResults2;
            }
            else if (apiPageNumber + 1 > apiResults.TotalPages)
            {
                clientPageResults = apiPageResults.GetRange(startIndex, apiPageResults.Count - startIndex);
            }
            else
            {
                clientPageResults = apiPageResults.GetRange(startIndex, apiPageResults.Count - startIndex);
            }
            return new ApiListResponse<T>
            {
                Items = clientPageResults,
                TotalItems = apiResults.TotalPages,
                IsSuccess = true,
            };
        }

        /// <summary>
        /// Vérifie si le type et la cinématographie de l'objet correspondent pour la découverte.
        /// </summary>
        /// <param name="type">Le type de l'objet à découvrir.</param>
        /// <param name="cinematography">La cinématographie de l'objet à découvrir.</param>
        /// <exception cref="NotSupportedTypeException">Lancée lorsque le type n'est pas supporté.</exception>
        /// <exception cref="CinematographyMismatchException">Lancée lorsque la cinématographie ne correspond pas au type.</exception>
        private static void VerifyDiscoverItemTypeAndCinematography(Type type, Cinematography cinematography)
        {
            var _cinematography = type switch
            {
                Type t when t == typeof(TvShow) => Cinematography.Tv,
                Type t when t == typeof(Movie) => Cinematography.Movie,
                _ => throw new NotSupportedTypeException()
            };
            if (cinematography != _cinematography)
                throw new CinematographyMismatchException();
        }

        /// <summary>
        /// Vérifie si le type et la cinématographie de l'objet correspondent pour le détail.
        /// </summary>
        /// <param name="type">Le type de l'objet dont on veut obtenir les détails.</param>
        /// <param name="cinematography">La cinématographie de l'objet dont on veut obtenir les détails.</param>
        /// <exception cref="NotSupportedTypeException">Lancée lorsque le type n'est pas supporté.</exception>
        /// <exception cref="CinematographyMismatchException">Lancée lorsque la cinématographie ne correspond pas au type.</exception>
        private static void VerifyDetailItemTypeAndCinematography(Type type, Cinematography cinematography)
        {
            var _cinematography = type switch
            {
                Type t when t == typeof(TvShowDetails) => Cinematography.Tv,
                Type t when t == typeof(MovieDetails) => Cinematography.Movie,
                _ => throw new NotSupportedTypeException()
            };
            if (cinematography != _cinematography)
                throw new CinematographyMismatchException();
        }
    }
}
