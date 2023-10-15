using BSFlixFlex.Client.Shareds.Interfaces;
using BSFlixFlex.Client.Shareds.Models;
using BSFlixFlex.MinimalApi.CinematographyEndpoints;

namespace BSFlixFlex.MinimalApi
{
    public static class CinematographyMap
    {
        public static IEndpointRouteBuilder MapCinematographys(this IEndpointRouteBuilder builder)
        {
            var groupBuilder = builder.MapGroup(API_GROUP_CINEMATOGRAPHY );
            
            var groupDiscoveryBuilder = groupBuilder.MapGroup(API_GROUP_DISCOVERY);
            groupDiscoveryBuilder.MapGet("/"+ API_RELATIVE_PATH_MOVIE, CinematographyDiscovery.GetAllMovie);
            groupDiscoveryBuilder.MapGet("/"+ API_RELATIVE_PATH_TVSHOW, CinematographyDiscovery.GetAllTvShow);
            groupDiscoveryBuilder.MapGet("/"+ API_RELATIVE_PATH_MOVIE_TOPRATED, CinematographyDiscovery.GetTopRatedMovie);
            groupDiscoveryBuilder.MapGet("/"+ API_RELATIVE_PATH_TVSHOW_TOPRATED, CinematographyDiscovery.GetTopRatedTvShow);
            
            var groupSearchBuild = groupBuilder.MapGroup(API_GROUP_SEARCH);
            groupSearchBuild.MapGet("/" + API_RELATIVE_PATH_MOVIE, CinematographySearch.GetSearchMovie);
            groupSearchBuild.MapGet("/" + API_RELATIVE_PATH_TVSHOW, CinematographySearch.GetSearchTvShow);
            
            var groupDetailBuild = groupBuilder.MapGroup(API_GROUP_DETAIL);
            groupDetailBuild.MapGet("/" + API_RELATIVE_PATH_MOVIE + "/{id:int}", CinematographyDetail.GetMovieDetail);
            groupDetailBuild.MapGet("/" + API_RELATIVE_PATH_TVSHOW + "/{id:int}", CinematographyDetail.GetTvShowDetail);

            var groupVideoBuild = groupBuilder.MapGroup(API_GROUP_VIDEO);
            groupVideoBuild.MapGet("/" + API_RELATIVE_PATH_MOVIE + "/{id:int}", CinematographyVideo.GetMovieVideo);
            groupVideoBuild.MapGet("/" + API_RELATIVE_PATH_TVSHOW + "/{id:int}", CinematographyVideo.GetTvShowVideo);

            return builder;
        }
    }
}
