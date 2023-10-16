using Xunit;
using BSFlixFlex.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using BSFlixFlex.Exceptions;
using BSFlixFlex.Client.Shareds.Models;
using BSFlixFlex.Client.Shareds.Exceptions;
using Microsoft.Extensions.Http;
using Moq;
using BSFlixFlex.Client.Shareds.Models.Cinematographies.Movies;
using BSFlixFlex.Client.Shareds.Models.Cinematographies.TvShows;
using BSFlixFlex.Client.Shareds.Models.Cinematographies;
using BSFlixFlex.Models;

namespace BSFlixFlex.Services.Tests
{
    public class ApiTMBDServiceTests
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiTMBDServiceTests()
        {
            var client = new HttpClient() { BaseAddress = new Uri("https://api.themoviedb.org/") };
            var token = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI2NzUxMTU1Y2QxZDQ1NjczMGJlOTg1OTViY2RlZTQ4NSIsInN1YiI6IjY1MTJkMDY0ZTFmYWVkMDEzYTBjOGYxYyIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.eWjXyaDpeLGJPrWFfB_ZnAwjz2NldXIsPxKk4D-6tVM";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory.Setup(h => h.CreateClient(It.IsAny<string>())).Returns(client);
            _httpClientFactory = httpClientFactory.Object;
            _httpClient = client;
            
            
        }

        [Fact()]
        public async void FetchTopRatedItemsAsyncTest_AvecMovie()
        {
            // Arrange
            
            var mockService = new ApiTMBDService(_httpClientFactory);
            var cinematography = Cinematography.Movie;
            var clientPageNumber = 1;
            var clientPageSize = 5;

            var path = "3/movie/top_rated?page=1&language=fr-Fr";
            var apiResults = await _httpClient.GetFromJsonAsync<ApiTmBdDiscoverResponse<Movie>>(path);

            var expected = ParcialArrangeOfDisciverResponse(apiResults);

            // Act
            var result = await mockService.FetchTopRatedItemsAsync<Movie>(cinematography, clientPageNumber, clientPageSize);

            // Assert
            ParcialAssertOfDiscoverResponse(clientPageSize, expected, result);
        }

        private static void ParcialAssertOfDiscoverResponse<T>(int clientPageSize, ApiListResponse<T> expected, ApiListResponse<T> result) where T : IDiscovryCommonProperty
        {
            Assert.True(result.IsSuccess);
            Assert.Equal(clientPageSize, result.Items.Count);
            Assert.Equal(expected.Items[0].Id, result.Items[0].Id);
            Assert.Equal(expected.Items[0].Title, result.Items[0].Title);
            Assert.Equal(expected.Items[4].Id, result.Items[^1].Id);
        }

        private static ApiListResponse<T> ParcialArrangeOfDisciverResponse<T>(ApiTmBdDiscoverResponse<T>? apiResults) where T : class
        {
            ApiListResponse<T> expected = new();
            if (apiResults != null)
                expected = new() { IsSuccess = true, Items = [.. apiResults.Results], TotalItems = apiResults.TotalResults, Message = "" };
            return expected;
        }

        [Fact()]
        public async void FetchTopRatedItemsAsyncTest_AvecTv()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClientFactory);
            var cinematography = Cinematography.Tv;
            var clientPageNumber = 1;
            var clientPageSize = 5;

            var path = "3/tv/top_rated?page=1&language=fr-Fr";
            var apiResults = await _httpClient.GetFromJsonAsync<ApiTmBdDiscoverResponse<TvShow>>(path);
            var expected = ParcialArrangeOfDisciverResponse(apiResults);

            // Act
            var result = await mockService.FetchTopRatedItemsAsync<TvShow>(cinematography, clientPageNumber, clientPageSize);

            // Assert
            ParcialAssertOfDiscoverResponse(clientPageSize, expected, result);
        }

        [Fact()]
        public async void FetchTopRatedItemsAsyncTest_Exception_AvecMovieEtCinematographyTv()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClientFactory);
            var cinematography = Cinematography.Tv;
            var clientPageNumber = 1;
            var clientPageSize = 5;

            // Act & Assert
            await Assert.ThrowsAsync<CinematographyMismatchException>(async () =>
            await mockService.FetchTopRatedItemsAsync<Movie>(cinematography, clientPageNumber, clientPageSize));
        }

        [Fact()]
        public async void FetchTopRatedItemsAsyncTest_Exception_AvecMovieDetail()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClientFactory);
            var cinematography = Cinematography.Movie;
            var clientPageNumber = 1;
            var clientPageSize = 5;

            // Act & Assert
            await Assert.ThrowsAsync<NotSupportedTypeException>(async () =>
            await mockService.FetchTopRatedItemsAsync<MovieDetails>(cinematography, clientPageNumber, clientPageSize));
        }

        [Fact()]
        public async void FetchTopRatedItemsAsyncTest_Exception_SiPageSeizeSuperieurApi()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClientFactory);
            var cinematography = Cinematography.Movie;
            var clientPageNumber = 1;
            var clientPageSize = 21;

            // Act & Assert
            await Assert.ThrowsAsync<PageSizeMismatchException>(async () =>
            await mockService.FetchTopRatedItemsAsync<Movie>(cinematography, clientPageNumber, clientPageSize));
        }

        [Fact()]
        public async void FetchTopRatedItemsAsyncTest_Page_Seiz3()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClientFactory);
            var cinematography = Cinematography.Movie;
            //var clientPageNumber = 1;
            var clientPageSize = 3;

            var path1 = "3/movie/top_rated?page=1&language=fr-Fr";
            var path2 = "3/movie/top_rated?page=2&language=fr-Fr";
            var path3 = "3/movie/top_rated?page=500&language=fr-Fr";
            var apiResults1 = await _httpClient.GetFromJsonAsync<ApiTmBdDiscoverResponse<Movie>>(path1);
            var apiResults2 = await _httpClient.GetFromJsonAsync<ApiTmBdDiscoverResponse<Movie>>(path2);
            var apiResults3 = await _httpClient.GetFromJsonAsync<ApiTmBdDiscoverResponse<Movie>>(path3);

            var expected1 = ParcialArrangeOfDisciverResponse(apiResults1);
            var expected2 = ParcialArrangeOfDisciverResponse(apiResults2);
            var expected3 = ParcialArrangeOfDisciverResponse(apiResults3);

            // Act
            var result1 = await mockService.FetchTopRatedItemsAsync<Movie>(cinematography, 1, clientPageSize);
            //var result2 = await mockService.FetchTopRatedItemsAsync<Movie>(cinematography, 2, clientPageSize);
            //var result3 = await mockService.FetchTopRatedItemsAsync<Movie>(cinematography, 3, clientPageSize);
            //var result4 = await mockService.FetchTopRatedItemsAsync<Movie>(cinematography, 4, clientPageSize);
            //var result5 = await mockService.FetchTopRatedItemsAsync<Movie>(cinematography, 5, clientPageSize);
            var result6 = await mockService.FetchTopRatedItemsAsync<Movie>(cinematography, 6, clientPageSize);
            var result7 = await mockService.FetchTopRatedItemsAsync<Movie>(cinematography, 7, clientPageSize);
            var result8 = await mockService.FetchTopRatedItemsAsync<Movie>(cinematography, 3334, clientPageSize);

            // Assert

            Assert.Equal(expected1.Items[0].Id, result1.Items[0].Id);
            Assert.Equal(expected1.Items[15].Id, result6.Items[0].Id);
            Assert.Equal(expected1.Items[17].Id, result6.Items[^1].Id);
            Assert.Equal(expected1.Items[18].Id, result7.Items[0].Id);
            Assert.Equal(expected2.Items[0].Id, result7.Items[^1].Id);

            Assert.Equal(expected3.Items[^1].Id, result8.Items[^1].Id);
            Assert.Single(result8.Items);
        }

        [Fact()]
        public async void FetchDiscoveryItemsAsyncTest_AvecMovie()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClientFactory);
            var cinematography = Cinematography.Movie;
            var clientPageNumber = 1;
            var clientPageSize = 5;

            var path = "4/discover/movie?page=1&language=fr-Fr";
            var apiResults = await _httpClient.GetFromJsonAsync<ApiTmBdDiscoverResponse<Movie>>(path);
            var expected = ParcialArrangeOfDisciverResponse(apiResults);

            // Act
            var result = await mockService.FetchDiscoveryItemsAsync<Movie>(cinematography, clientPageNumber, clientPageSize);

            // Assert
            ParcialAssertOfDiscoverResponse(clientPageSize, expected, result);
        }

        [Fact()]
        public async void FetchDiscoveryItemsAsyncTest_TotalItemsLimiteApiTMBD()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClientFactory);
            var cinematography = Cinematography.Movie;
            var clientPageNumber = 1;
            var clientPageSize = 10;

            var path = "4/discover/movie?page=1&language=fr-Fr";
            var apiResults = await _httpClient.GetFromJsonAsync<ApiTmBdDiscoverResponse<Movie>>(path);
            var expected = ParcialArrangeOfDisciverResponse(apiResults);

            // Act
            var result = await mockService.FetchDiscoveryItemsAsync<Movie>(cinematography, clientPageNumber, clientPageSize);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(10000 == result.TotalItems);
            Assert.True(result.Items.Count == 10);
        }
        [Fact()]
        public async void FetchDiscoveryItemsAsyncTest_AvecTv()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClientFactory);
            var cinematography = Cinematography.Tv;
            var clientPageNumber = 1;
            var clientPageSize = 5;

            var path = "4/discover/tv?page=1&language=fr-Fr";
            var apiResults = await _httpClient.GetFromJsonAsync<ApiTmBdDiscoverResponse<TvShow>>(path);
            var expected = ParcialArrangeOfDisciverResponse(apiResults);

            // Act
            var result = await mockService.FetchDiscoveryItemsAsync<TvShow>(cinematography, clientPageNumber, clientPageSize);

            // Assert
            ParcialAssertOfDiscoverResponse(clientPageSize, expected, result);
        }

        [Fact()]
        public async void SearchItemsAsyncTest_AvecMovie()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClientFactory);
            var cinematography = Cinematography.Movie;
            var search = "a";
            var clientPageNumber = 1;
            var clientPageSize = 5;


            var path = $"3/search/movie?page=1&language=fr-Fr&query={search}";
            var apiResults = await _httpClient.GetFromJsonAsync<ApiTmBdDiscoverResponse<Movie>>(path);
            var expected = ParcialArrangeOfDisciverResponse(apiResults);

            // Act
            var result = await mockService.SearchItemsAsync<Movie>(cinematography, search, clientPageNumber, clientPageSize);

            // Assert
            ParcialAssertOfDiscoverResponse(clientPageSize, expected, result);
        }

        [Fact()]
        public async void SearchItemsAsyncTest_AvecTv()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClientFactory);
            var cinematography = Cinematography.Tv;
            var search = "a";
            var clientPageNumber = 1;
            var clientPageSize = 5;


            var path = $"3/search/tv?page=1&language=fr-Fr&query={search}";
            var apiResults = await _httpClient.GetFromJsonAsync<ApiTmBdDiscoverResponse<TvShow>>(path);
            var expected = ParcialArrangeOfDisciverResponse(apiResults);

            // Act
            var result = await mockService.SearchItemsAsync<TvShow>(cinematography, search, clientPageNumber, clientPageSize);

            // Assert
            ParcialAssertOfDiscoverResponse(clientPageSize, expected, result);
        }

        [Fact()]
        public async void FetchItemDetailsAsyncTest_AvecMovieDetails()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClientFactory);
            var cinematography = Cinematography.Movie;
            var movieId = 114;
            var path = $"3/movie/{movieId}?language=fr-Fr";

            var apiResults = await _httpClient.GetFromJsonAsync<MovieDetails>(path);

            // Act
            var result = await mockService.FetchItemDetailsAsync<MovieDetails>(cinematography, movieId);

            // Assert
            ParcialAssertOfDetail(apiResults, result);
        }

        [Fact()]
        public async void FetchItemDetailsAsyncTest_AvecTvShowDetails()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClientFactory);
            var cinematography = Cinematography.Tv;
            var id = 114;
            var path = $"3/tv/{id}?language=fr-Fr";

            var apiResults = await _httpClient.GetFromJsonAsync<TvShowDetails>(path);

            // Act
            var result = await mockService.FetchItemDetailsAsync<TvShowDetails>(cinematography, id);

            // Assert
            ParcialAssertOfDetail(apiResults, result);
        }

        private static void ParcialAssertOfDetail<T>(T? apiResults, ApiItemResponse<T> result) where T : IDetailsCommonProperty
        {
            Assert.True(result.IsSuccess);
            Assert.Equal(apiResults!.Id, result.Item!.Id);
            Assert.Equal(apiResults!.Overview, result.Item!.Overview);
        }

        [Fact()]
        public async void FetchItemDetailsAsyncTest_AvecMovieDetailsEtCinematographyTv_Exption()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClientFactory);
            var cinematography = Cinematography.Tv;
            var movieId = 114;

            // Act & Assert
            await Assert.ThrowsAsync<CinematographyMismatchException>(async () => await mockService.FetchItemDetailsAsync<MovieDetails>(cinematography, movieId));
        }
        [Fact()]
        public async void FetchItemDetailsAsyncTest_Exception_AvecMovie()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClientFactory);
            var cinematography = Cinematography.Movie;
            var movieId = 114;

            // Act & Assert
            await Assert.ThrowsAsync<NotSupportedTypeException>(async () => await mockService.FetchItemDetailsAsync<Movie>(cinematography, movieId));
        }

        [Fact()]
        public async void FetchItemVideosAsyncTest_AvecMovie()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClientFactory);
            var cinematography = Cinematography.Movie;
            var movieId = 114;
            var path = $"3/movie/{movieId}/videos?language=fr-Fr";

            var apiResults = await _httpClient.GetFromJsonAsync<ApiTmBdVideoResponse>(path);

            // Act
            var result = await mockService.FetchItemVideosAsync<Movie>(cinematography, movieId);

            // Assert
            Assert.True(result!.IsSuccess);
            Assert.Equal(apiResults!.Results.Count, result.Items.Count);
        }
        [Fact()]
        public async void FetchItemVideosAsyncTest_AvecTv()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClientFactory);
            var cinematography = Cinematography.Tv;
            var movieId = 114;
            var path = $"3/tv/{movieId}/videos?language=fr-Fr";

            var apiResults = await _httpClient.GetFromJsonAsync<ApiTmBdVideoResponse>(path);

            // Act
            var result = await mockService.FetchItemVideosAsync<TvShow>(cinematography, movieId);

            // Assert
            Assert.True(result!.IsSuccess);
            Assert.Equal(apiResults!.Results.Count, result.Items.Count);
        }
    }
}