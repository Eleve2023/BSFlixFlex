﻿using BSFlixFlex.Client.Shareds.Interfaces;
using BSFlixFlex.Client.Shareds.Models;

namespace BSFlixFlex.MinimalApi.CinematographyEndpoints
{
    internal class CinematographyDetail
    {
        internal static async Task<IResult> GetMovieDetail(int id, IApiTMBDService apiTMBDService)
        {
            return TypedResults.Ok(await apiTMBDService.FetchItemDetailsAsync<MovieDetails>(Cinematography.Movie, id));
        }

        internal static async Task<IResult> GetTvShowDetail(int id, IApiTMBDService apiTMBDService)
        {
            return TypedResults.Ok(await apiTMBDService.FetchItemDetailsAsync<TvShowDetails>(Cinematography.Tv, id));
        }
    }
}