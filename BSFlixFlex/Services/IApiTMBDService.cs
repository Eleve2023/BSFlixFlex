using BSFlixFlex.Models;

namespace BSFlixFlex.Services
{
    public interface IApiTMBDService
    {
        Task<ApiListResponse<T>> FetchDiscoveryItemsAsync<T>(Cinematography cinematography, int clientPageNumber, int clientPageSize = 10) where T : class;
        Task<ApiItemResponse<T>> FetchItemDetailsAsync<T>(Cinematography cinematography, int id);
        Task<VideoResponse> FetchItemVideosAsync<T>(Cinematography cinematography, int id);
        Task<ApiListResponse<T>> FetchTopRatedItemsAsync<T>(Cinematography cinematography, int clientPageNumber, int clientPageSize = 10) where T : class;
        Task<ApiListResponse<T>> SearchItemsAsync<T>(Cinematography cinematography, string search, int clientPageNumber, int clientPageSize = 10) where T : class;
    }
}