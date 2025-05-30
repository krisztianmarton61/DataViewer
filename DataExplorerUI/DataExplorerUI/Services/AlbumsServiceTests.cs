using DataExplorerModels;
using DataExplorerUI.Configuration;
using DataExplorerUI.Services;
using Moq.Protected;
using Moq;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Xunit;

namespace DataExplorerUI.Tests;

public class AlbumsServiceTests
{
    [Fact]
    public async Task GetAlbumsAsync_ReturnsAlbums_WhenResponseIsSuccessful()
    {
        var expectedAlbums = new List<Album>
        {
            new() { Id = "101", Title = "Rock" },
            new() { Id = "102", Title = "Jazz" }
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expectedAlbums),
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://fake-api.com")
        };

        var optionsMock = new Mock<IOptions<ApiOptions>>();
        optionsMock.Setup(o => o.Value).Returns(new ApiOptions
        {
            BaseUrl = "https://fake-api.com"
        });

        var service = new AlbumsService(httpClient, optionsMock.Object);

        var actualAlbums = await service.GetAlbumsAsync();

        Assert.Equal(expectedAlbums.Count, actualAlbums.Count);
        Assert.Equal(expectedAlbums[0].Id, actualAlbums[0].Id);
        Assert.Equal(expectedAlbums[1].Title, actualAlbums[1].Title);
    }

    [Fact]
    public async Task GetAlbumsAsync_ThrowsException_WhenResponseIsUnsuccessful()
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://fake-api.com")
        };

        var optionsMock = new Mock<IOptions<ApiOptions>>();
        optionsMock.Setup(o => o.Value).Returns(new ApiOptions
        {
            BaseUrl = "https://fake-api.com"
        });

        var service = new AlbumsService(httpClient, optionsMock.Object);

        await Assert.ThrowsAsync<HttpRequestException>(() => service.GetAlbumsAsync());
    }

    [Fact]
    public async Task GetAlbumsAsync_ReturnsEmptyList_WhenContentIsNull()
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("null", System.Text.Encoding.UTF8, "application/json")
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://fake-api.com")
        };

        var optionsMock = new Mock<IOptions<ApiOptions>>();
        optionsMock.Setup(o => o.Value).Returns(new ApiOptions
        {
            BaseUrl = "https://fake-api.com"
        });

        var service = new AlbumsService(httpClient, optionsMock.Object);

        var result = await service.GetAlbumsAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
