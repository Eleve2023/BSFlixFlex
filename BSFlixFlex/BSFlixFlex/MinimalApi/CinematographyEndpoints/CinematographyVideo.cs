using BSFlixFlex.Client.Shareds.Interfaces;
using BSFlixFlex.Client.Shareds.Models;

namespace BSFlixFlex.MinimalApi.CinematographyEndpoints
{
    public static class CinematographyVideo
    {
        public static async Task<IResult> GetMovieVideo(int id, IApiTMBDService apiTMBDService)
        {
            return TypedResults.Ok(await apiTMBDService.FetchItemVideosAsync<Movie>(Cinematography.Movie, id));
        }

        public static async Task<IResult> GetTvShowVideo(int id, IApiTMBDService apiTMBDService)
        {
            return TypedResults.Ok(await apiTMBDService.FetchItemVideosAsync<TvShow>(Cinematography.Tv, id));
        }
    }
}