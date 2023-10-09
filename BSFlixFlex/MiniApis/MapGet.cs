using BSFlixFlex.Models;
using BSFlixFlex.Services;

namespace BSFlixFlex.MiniApis
{
    public static class MapGet
    {
        public static WebApplication MiniApiApp(this WebApplication app)
        {
            app.MapGet("api/movie/top_rated", async (IApiTMBDService apiTMBDService, int page = 1) =>
            {
                return await apiTMBDService.FetchTopRatedItemsAsync<Movie>(Cinematography.Movie, page);
            });
            app.MapGet("api/tvshow/top_rated", async (IApiTMBDService apiTMBDService, int page = 1) =>
            {
                return await apiTMBDService.FetchTopRatedItemsAsync<TvShow>(Cinematography.Tv, page);
            });
            
            app.MapGet("api/movie", async (IApiTMBDService apiTMBDService, int page = 1) =>
            {
                return await apiTMBDService.FetchDiscoveryItemsAsync<Movie>(Cinematography.Movie, page);
            });            
            app.MapGet("api/tvshow", async (IApiTMBDService apiTMBDService, int page = 1) =>
            {
                return await apiTMBDService.FetchDiscoveryItemsAsync<TvShow>(Cinematography.Tv, page);
            });
            
            app.MapGet("api/search/movie", async (IApiTMBDService apiTMBDService,string search ,int page = 1) =>
            {
                return await apiTMBDService.SearchItemsAsync<Movie>(Cinematography.Movie,search, page);
            });
            app.MapGet("api/search/tvshow", async (IApiTMBDService apiTMBDService, string search, int page = 1) =>
            {
                return await apiTMBDService.SearchItemsAsync<TvShow>(Cinematography.Tv,search ,page);
            });
            
            app.MapGet("api/movie/{id:int}", async (int id, IApiTMBDService apiTMBDService) =>
            {
                return await apiTMBDService.FetchItemDetailsAsync<Movie>(Cinematography.Movie, id);
            });
            app.MapGet("api/tvshow/{id:int}", async (int id, IApiTMBDService apiTMBDService) =>
            {
                return await apiTMBDService.FetchItemDetailsAsync<TvShow>(Cinematography.Tv, id);
            });

            return app;
        }
       
    }
}
