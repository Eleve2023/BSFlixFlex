using BSFlixFlex.Data;
using BSFlixFlexTests.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Moq;
using System.Security.Claims;
using Xunit;
using BSFlixFlex.Client.Shareds.Interfaces;
using BSFlixFlex.Client.Shareds.Models;

namespace BSFlixFlex.Services.Tests
{
    public class MyFavoriteServiceTests : IDisposable
    {
        private readonly SharedAppDataBase dataBase;
        private readonly ClaimsPrincipal claimsPrincipal;
        private readonly ClaimsPrincipal claimsPrincipal1;

        public MyFavoriteServiceTests()
        {
            dataBase = new SharedAppDataBase();

            var expectedIdentifier = Guid.Parse("4f1c7b8d-8d8c-4e8b-a9f1-45c61a76bb02").ToString();
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, expectedIdentifier) };
            var identity = new ClaimsIdentity(claims, "AuthenticationType");
            claimsPrincipal = new ClaimsPrincipal(identity);
            var expectedIdentifier1 = Guid.Parse("4f1c7b8d-8d8c-4e8b-a9f1-45c61a76bb01").ToString();
            var claims1 = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, expectedIdentifier1) };
            var identity1 = new ClaimsIdentity(claims1, "AuthenticationType");
            claimsPrincipal1 = new ClaimsPrincipal(identity1);

        }

        public void Dispose() => dataBase.Dispose();

        [Fact()]
        public async void FetchUserFavoritesAsyncTest()
        {
            // Arrange
            using var context = dataBase.CreateContext();

            var mockApiTMBDService = new Mock<IApiTMBDService>();
            mockApiTMBDService.Setup(s => s.FetchItemDetailsAsync<MovieDetails>(It.IsAny<Cinematography>(), It.IsAny<int>()))
                .ReturnsAsync((Cinematography c, int id) => new ApiItemResponse<MovieDetails> { IsSuccess = true, Item = new() { Id = id } }).Verifiable();
            mockApiTMBDService.Setup(s => s.FetchItemDetailsAsync<TvShowDetails>(It.IsAny<Cinematography>(), It.IsAny<int>()))
                .ReturnsAsync((Cinematography c, int id) => new ApiItemResponse<TvShowDetails> { IsSuccess = true, Item = new() { Id = id } }).Verifiable();

            var myService = new MyFavoriteService(context, mockApiTMBDService.Object);

            // Act
            var result = await myService.FetchUserFavoritesAsync(claimsPrincipal, 1);

            // Expected            
            var expectedfavorites = context.Set<MyFavorite>()
                .Where(f => f.UserId == Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!));

            // Assert
            Assert.Equal(expectedfavorites.Count(), result.TotalItems);
        }

        [Fact()]
        public async void IsFavoriteAsyncTest()
        {
            // Arrange
            using var context = dataBase.CreateContext();

            var mockApiTMBDService = new Mock<IApiTMBDService>();

            var myService = new MyFavoriteService(context, mockApiTMBDService.Object);

            // Act
            var result = await myService.IsFavoriteAsync(1, Cinematography.Movie, claimsPrincipal);

            // Assert
            Assert.True(result);
        }

        [Fact()]
        public async void AddToFavoritesAsyncTest()
        {
            // Arrange
            using var context = dataBase.CreateContext();
            var mockApiTMBDService = new Mock<IApiTMBDService>();
            mockApiTMBDService.Setup(s => s.FetchItemDetailsAsync<MovieDetails>(It.IsAny<Cinematography>(), It.IsAny<int>()))
                .ReturnsAsync((Cinematography c, int id) => new ApiItemResponse<MovieDetails> { IsSuccess = true, Item = new() { Id = id } }).Verifiable();
            mockApiTMBDService.Setup(s => s.FetchItemDetailsAsync<TvShowDetails>(It.IsAny<Cinematography>(), It.IsAny<int>()))
                .ReturnsAsync((Cinematography c, int id) => new ApiItemResponse<TvShowDetails> { IsSuccess = true, Item = new() { Id = id } }).Verifiable();

            var myService = new MyFavoriteService(context, mockApiTMBDService.Object);

            // Act
            var resultBehAct = await myService.IsFavoriteAsync(5, Cinematography.Movie, claimsPrincipal);
            await myService.AddToFavoritesAsync(5, Cinematography.Movie, claimsPrincipal);
            var resultAftAct = await myService.IsFavoriteAsync(5, Cinematography.Movie, claimsPrincipal);

            // Assert
            Assert.False(resultBehAct);
            Assert.True(resultAftAct);
        }

        [Fact()]
        public async void RemoveFromFavoritesAsyncTest()
        {
            // Arrange
            using var context = dataBase.CreateContext();
            var mockApiTMBDService = new Mock<IApiTMBDService>();
            mockApiTMBDService.Setup(s => s.FetchItemDetailsAsync<MovieDetails>(It.IsAny<Cinematography>(), It.IsAny<int>()))
                .ReturnsAsync((Cinematography c, int id) => new ApiItemResponse<MovieDetails> { IsSuccess = true, Item = new() { Id = id } }).Verifiable();
            mockApiTMBDService.Setup(s => s.FetchItemDetailsAsync<TvShowDetails>(It.IsAny<Cinematography>(), It.IsAny<int>()))
                .ReturnsAsync((Cinematography c, int id) => new ApiItemResponse<TvShowDetails> { IsSuccess = true, Item = new() { Id = id } }).Verifiable();

            var myService = new MyFavoriteService(context, mockApiTMBDService.Object);

            // Act
            var resultBehAct = await myService.IsFavoriteAsync(1, Cinematography.Movie, claimsPrincipal);
            await myService.RemoveFromFavoritesAsync(1, Cinematography.Movie, claimsPrincipal);
            var resultAftAct = await myService.IsFavoriteAsync(1, Cinematography.Movie, claimsPrincipal);

            // Assert
            Assert.True(resultBehAct);
            Assert.False(resultAftAct);
        }
    }
}