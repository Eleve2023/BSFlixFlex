using BSFlixFlex.Client.Shareds.Interfaces;
using BSFlixFlex.Client.Shareds.Models.Cinematographies;
using BSFlixFlex.Client.Shareds.Models.Cinematographies.TvShows;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BSFlixFlex.MinimalApi.CinematographyEndpoints
{
    public static class CinematographySearch
    {
        public static async Task<IResult> GetSearchMovie(IApiTMBDService apiTMBDService, string search, int page = 1, int size = 10)
        {
            return TypedResults.Ok(await apiTMBDService.SearchItemsAsync<TvShow>(Cinematography.Tv, search, page, size));
        }

        public static async Task<IResult> GetSearchTvShow(IApiTMBDService apiTMBDService, string search, int page = 1, int size = 10)
        {
            return TypedResults.Ok(await apiTMBDService.SearchItemsAsync<TvShow>(Cinematography.Tv, search, page, size));
        }
    }
}