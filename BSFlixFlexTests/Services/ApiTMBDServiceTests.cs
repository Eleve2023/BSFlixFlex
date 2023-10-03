using Xunit;
using BSFlixFlex.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFlixFlex.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using BSFlixFlex.Exceptions;

namespace BSFlixFlex.Services.Tests
{
    public class ApiTMBDServiceTests
    {
        private readonly HttpClient _httpClient;

        public ApiTMBDServiceTests()
        {
            var client = new HttpClient() { BaseAddress = new Uri("https://api.themoviedb.org/") };
            var token = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI2NzUxMTU1Y2QxZDQ1NjczMGJlOTg1OTViY2RlZTQ4NSIsInN1YiI6IjY1MTJkMDY0ZTFmYWVkMDEzYTBjOGYxYyIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.eWjXyaDpeLGJPrWFfB_ZnAwjz2NldXIsPxKk4D-6tVM";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            _httpClient = client;
        }

        [Fact()]
        public async void FetchTopRatedItemsAsyncTest_AvecMovie()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClient);
            var cinematography = Cinematography.Movie;
            var clientPageNumber = 1;
            var clientPageSize = 5;

            var path = "3/movie/top_rated?page=1&language=fr-Fr";
            var apiResults = await _httpClient.GetFromJsonAsync<DiscoverResponse<Movie>>(path);

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

        private static ApiListResponse<T> ParcialArrangeOfDisciverResponse<T>(DiscoverResponse<T>? apiResults) where T : class
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
            var mockService = new ApiTMBDService(_httpClient);
            var cinematography = Cinematography.Tv;
            var clientPageNumber = 1;
            var clientPageSize = 5;

            var path = "3/tv/top_rated?page=1&language=fr-Fr";
            var apiResults = await _httpClient.GetFromJsonAsync<DiscoverResponse<TvShow>>(path);
            var expected = ParcialArrangeOfDisciverResponse(apiResults);

            // Act
            var result = await mockService.FetchTopRatedItemsAsync<TvShow>(cinematography, clientPageNumber, clientPageSize);

            // Assert
            ParcialAssertOfDiscoverResponse(clientPageSize, expected, result);
        }

        [Fact()]
        public async void FetchTopRatedItemsAsyncTest_AvecMovieEtCinematographyTv_Exption()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClient);
            var cinematography = Cinematography.Tv;
            var clientPageNumber = 1;
            var clientPageSize = 5;

            // Act & Assert
            await Assert.ThrowsAsync<CinematographyMismatchException>(async () =>
            await mockService.FetchTopRatedItemsAsync<Movie>(cinematography, clientPageNumber, clientPageSize));
        }

        [Fact()]
        public async void FetchTopRatedItemsAsyncTest_AvecMovieDetail_Exption()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClient);
            var cinematography = Cinematography.Movie;
            var clientPageNumber = 1;
            var clientPageSize = 5;

            // Act & Assert
            await Assert.ThrowsAsync<NotSupportedTypeException>(async () =>
            await mockService.FetchTopRatedItemsAsync<MovieDetails>(cinematography, clientPageNumber, clientPageSize));
        }

        [Fact()]
        public async void FetchDiscoveryItemsAsyncTest_AvecMovie()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClient);
            var cinematography = Cinematography.Movie;
            var clientPageNumber = 1;
            var clientPageSize = 5;

            var path = "4/discover/movie?page=1&language=fr-Fr";
            var apiResults = await _httpClient.GetFromJsonAsync<DiscoverResponse<Movie>>(path);
            var expected = ParcialArrangeOfDisciverResponse(apiResults);

            // Act
            var result = await mockService.FetchDiscoveryItemsAsync<Movie>(cinematography, clientPageNumber, clientPageSize);

            // Assert
            ParcialAssertOfDiscoverResponse(clientPageSize, expected, result);
        }

        [Fact()]
        public async void FetchDiscoveryItemsAsyncTest_AvecTv()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClient);
            var cinematography = Cinematography.Tv;
            var clientPageNumber = 1;
            var clientPageSize = 5;

            var path = "4/discover/tv?page=1&language=fr-Fr";
            var apiResults = await _httpClient.GetFromJsonAsync<DiscoverResponse<TvShow>>(path);
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
            var mockService = new ApiTMBDService(_httpClient);
            var cinematography = Cinematography.Movie;
            var search = "a";
            var clientPageNumber = 1;
            var clientPageSize = 5;


            var path = $"3/search/movie?page=1&language=fr-Fr&query={search}";
            var apiResults = await _httpClient.GetFromJsonAsync<DiscoverResponse<Movie>>(path);
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
            var mockService = new ApiTMBDService(_httpClient);
            var cinematography = Cinematography.Tv;
            var search = "a";
            var clientPageNumber = 1;
            var clientPageSize = 5;


            var path = $"3/search/tv?page=1&language=fr-Fr&query={search}";
            var apiResults = await _httpClient.GetFromJsonAsync<DiscoverResponse<TvShow>>(path);
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
            var mockService = new ApiTMBDService(_httpClient);
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
            var mockService = new ApiTMBDService(_httpClient);
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
            var mockService = new ApiTMBDService(_httpClient);
            var cinematography = Cinematography.Tv;
            var movieId = 114;

            // Act & Assert
            await Assert.ThrowsAsync<CinematographyMismatchException>(async () => await mockService.FetchItemDetailsAsync<MovieDetails>(cinematography, movieId));
        }
        [Fact()]
        public async void FetchItemDetailsAsyncTest_AvecMovie_Exption()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClient);
            var cinematography = Cinematography.Movie;
            var movieId = 114;

            // Act & Assert
            await Assert.ThrowsAsync<NotSupportedTypeException>(async () => await mockService.FetchItemDetailsAsync<Movie>(cinematography, movieId));
        }

        [Fact()]
        public async void FetchItemVideosAsyncTest_AvecMovie()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClient);
            var cinematography = Cinematography.Movie;
            var movieId = 114;
            var path = $"3/movie/{movieId}/videos?language=fr-Fr";

            var apiResults = await _httpClient.GetFromJsonAsync<VideoResponse>(path);

            // Act
            var result = await mockService.FetchItemVideosAsync<VideoResponse>(cinematography, movieId);

            // Assert
            Assert.True(result!.IsSuccess);
            Assert.Equal(apiResults!.Results.Count, result.Results.Count);
        }
        [Fact()]
        public async void FetchItemVideosAsyncTest_AvecTv()
        {
            // Arrange
            var mockService = new ApiTMBDService(_httpClient);
            var cinematography = Cinematography.Tv;
            var movieId = 114;
            var path = $"3/tv/{movieId}/videos?language=fr-Fr";

            var apiResults = await _httpClient.GetFromJsonAsync<VideoResponse>(path);

            // Act
            var result = await mockService.FetchItemVideosAsync<VideoResponse>(cinematography, movieId);

            // Assert
            Assert.True(result!.IsSuccess);
            Assert.Equal(apiResults!.Results.Count, result.Results.Count);
        }
    }
}