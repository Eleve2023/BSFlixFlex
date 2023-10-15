using BSFlixFlex.Client.Shareds.Interfaces;
using BSFlixFlex.Client.Shareds.Models;

namespace BSFlixFlex.MiniApis
{
    public static class MapGet
    {
        public static WebApplication MiniApiApp(this WebApplication app)
        {
            app.MapGet(API_PATH_MOVIE_TOP_RATED, async (IApiTMBDService apiTMBDService, int page = 1) =>
            {
                return await apiTMBDService.FetchTopRatedItemsAsync<Movie>(Cinematography.Movie, page);
            });
            app.MapGet(API_PATH_TVSHOW_TOP_RATED, async (IApiTMBDService apiTMBDService, int page = 1) =>
            {
                return await apiTMBDService.FetchTopRatedItemsAsync<TvShow>(Cinematography.Tv, page);
            });
            
            app.MapGet(API_PATH_MOVIE, async (IApiTMBDService apiTMBDService, int page = 1) =>
            {
                return await apiTMBDService.FetchDiscoveryItemsAsync<Movie>(Cinematography.Movie, page);
            });            
            app.MapGet(API_PATH_TVSHOW, async (IApiTMBDService apiTMBDService, int page = 1) =>
            {
                return await apiTMBDService.FetchDiscoveryItemsAsync<TvShow>(Cinematography.Tv, page);
            });
            
            app.MapGet(API_PATH_SEARCH_MOVIE, async (IApiTMBDService apiTMBDService,string search ,int page = 1) =>
            {
                return await apiTMBDService.SearchItemsAsync<Movie>(Cinematography.Movie,search, page);
            });
            app.MapGet(API_PATH_SEARCH_TVSHOW, async (IApiTMBDService apiTMBDService, string search, int page = 1) =>
            {
                return await apiTMBDService.SearchItemsAsync<TvShow>(Cinematography.Tv,search ,page);
            });
            
            app.MapGet(API_PATH_MOVIE_ID, async (int id, IApiTMBDService apiTMBDService) =>
            {
                return await apiTMBDService.FetchItemDetailsAsync<MovieDetails>(Cinematography.Movie, id);
            });
            app.MapGet(API_PATH_TVSHOW_ID, async (int id, IApiTMBDService apiTMBDService) =>
            {
                return await apiTMBDService.FetchItemDetailsAsync<TvShowDetails>(Cinematography.Tv, id);
            });
            app.MapGet(API_PATH_VIDEO_MOVIE_ID, async (int id, IApiTMBDService apiTMBDService) =>
            {
                return await apiTMBDService.FetchItemVideosAsync<Movie>(Cinematography.Movie, id);
            });
            app.MapGet(API_PATH_VIDEO_TVSHOW_ID, async (int id, IApiTMBDService apiTMBDService) =>
            {
                return await apiTMBDService.FetchItemVideosAsync<TvShow>(Cinematography.Tv, id);
            });
            return app;
        }
       
    }
}
