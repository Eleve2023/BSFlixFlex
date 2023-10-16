using BSFlixFlex.Client.Shareds.Interfaces;
using BSFlixFlex.Client.Shareds.Models.Cinematographies;
using BSFlixFlex.Client.Shareds.Models.Cinematographies.Movies;
using BSFlixFlex.Client.Shareds.Models.Cinematographies.TvShows;

namespace BSFlixFlex.MinimalApi.CinematographyEndpoints
{
    public static class CinematographyDiscovery
    {
        public static async Task<IResult> GetAllMovie(IApiTMBDService apiTMBDService, int page = 1, int pageSize = 10)
        {
            return TypedResults.Ok(await apiTMBDService.FetchDiscoveryItemsAsync<Movie>(Cinematography.Movie, page, pageSize));
        }
        public static async Task<IResult> GetTopRatedMovie(IApiTMBDService apiTMBDService, int page = 1, int pageSize = 10)
        {
            return TypedResults.Ok(await apiTMBDService.FetchTopRatedItemsAsync<Movie>(Cinematography.Movie, page, pageSize));
        }
        public static async Task<IResult> GetAllTvShow(IApiTMBDService apiTMBDService, int page = 1, int pageSize = 10)
        {
            return TypedResults.Ok(await apiTMBDService.FetchDiscoveryItemsAsync<TvShow>(Cinematography.Tv, page, pageSize));
        }
        public static async Task<IResult> GetTopRatedTvShow(IApiTMBDService apiTMBDService, int page = 1, int pageSize = 10)
        {
            return TypedResults.Ok(await apiTMBDService.FetchTopRatedItemsAsync<TvShow>(Cinematography.Tv, page, pageSize));
        }
    }
}
