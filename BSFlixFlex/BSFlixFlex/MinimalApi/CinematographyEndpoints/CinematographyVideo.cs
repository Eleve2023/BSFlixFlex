using BSFlixFlex.Client.Shareds.Interfaces;
using BSFlixFlex.Client.Shareds.Models.Cinematographies;
using BSFlixFlex.Client.Shareds.Models.Cinematographies.Movies;
using BSFlixFlex.Client.Shareds.Models.Cinematographies.TvShows;

namespace BSFlixFlex.MinimalApi.CinematographyEndpoints
{
    public static class CinematographyVideo
    {
        public static async Task<IResult> GetMovieVideo(int id, IApiTMBDService apiTMBDService)
        {
            var apiListResponse = await apiTMBDService.FetchItemVideosAsync<Movie>(Cinematography.Movie, id);
            return TypedResults.Ok(apiListResponse);
        }

        public static async Task<IResult> GetTvShowVideo(int id, IApiTMBDService apiTMBDService)
        {
            var apiListResponse = await apiTMBDService.FetchItemVideosAsync<TvShow>(Cinematography.Tv, id);
            return TypedResults.Ok(apiListResponse);
        }
    }
}