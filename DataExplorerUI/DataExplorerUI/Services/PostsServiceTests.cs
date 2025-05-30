using DataExplorerModels;
using DataExplorerUI.Configuration;
using DataExplorerUI.Services;
using Moq.Protected;
using Moq;
using System.Net;
using Microsoft.Extensions.Options;
using Xunit;

namespace DataExplorerUI.Tests;

public class PostsServiceTests
{
    [Fact]
    public async Task GetPostsAsync_ReturnsPosts_WhenResponseIsSuccessful()
    {
        var expectedPosts = new List<Post>
        {
            new() { Id = "001", Title = "First" },
            new() { Id = "002", Title = "Second" }
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expectedPosts),
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

        var service = new PostsService(httpClient, optionsMock.Object);

        var actualPosts = await service.GetPostsAsync();

        Assert.Equal(expectedPosts.Count, actualPosts.Count);
        Assert.Equal(expectedPosts[0].Id, actualPosts[0].Id);
        Assert.Equal(expectedPosts[1].Title, actualPosts[1].Title);
    }

    [Fact]
    public async Task GetPostsAsync_ThrowsException_WhenResponseIsUnsuccessful()
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

        var service = new PostsService(httpClient, optionsMock.Object);

        await Assert.ThrowsAsync<HttpRequestException>(() => service.GetPostsAsync());
    }

    [Fact]
    public async Task GetPostsAsync_ReturnsEmptyList_WhenContentIsNull()
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

        var service = new PostsService(httpClient, optionsMock.Object);

        var result = await service.GetPostsAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
