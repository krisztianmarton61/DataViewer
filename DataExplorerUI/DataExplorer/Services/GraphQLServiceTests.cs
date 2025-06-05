using DataExplorerServices;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace DataExplorer.Services
{
    public class GraphQLServiceTests
    {

        private HttpClient CreateMockHttpClient(HttpResponseMessage response)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            return new HttpClient(handlerMock.Object);
        }

        [Fact]
        public async Task GetAlbumsAsync_ReturnsAlbumList_WhenResponseIsValid()
        {
            var mockResponse = new
            {
                data = new
                {
                    albums = new
                    {
                        data = new[]
                        {
                        new
                        {
                            id = "1",
                            title = "Test Album",
                            user = new {
                                name = "User1",
                                username = "user1",
                                email = "user1@example.com",
                                phone = "123456789",
                                website = "example.com",
                                company = new { name = "Company1" }
                            },
                            photos = new {
                                data = new[] {
                                    new { id = "101", title = "Photo1", url = "http://example.com/photo1.jpg" }
                                }
                            }
                        }
                    }
                    }
                }
            };

            var httpClient = CreateMockHttpClient(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(mockResponse), Encoding.UTF8, "application/json")
            });

            var service = new GraphQLService(httpClient);

            var albums = await service.GetAlbumsAsync();

            Assert.NotNull(albums);
            Assert.Single(albums);
        }

        [Fact]
        public async Task GetAlbumsAsync_ReturnsEmptyList_WhenAlbumsDataIsNull()
        {
            var mockResponse = new
            {
                data = new
                {
                    albums = (object)null
                }
            };

            var httpClient = CreateMockHttpClient(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(mockResponse), Encoding.UTF8, "application/json")
            });

            var service = new GraphQLService(httpClient);

            var albums = await service.GetAlbumsAsync();

            Assert.NotNull(albums);
            Assert.Empty(albums);
        }

        [Fact]
        public async Task GetPostsAsync_ReturnsList_WhenResponseIsValid()
        {
            var mockResponse = new
            {
                data = new
                {
                    posts = new
                    {
                        data = new[]
                        {
                            new
                            {
                                id = "1",
                                title = "Test Post",
                                body = "This is post body."
                            }
                        }
                    }
                }
            };
            var httpClient = CreateMockHttpClient(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(mockResponse), Encoding.UTF8, "application/json")
            });

            var service = new GraphQLService(httpClient);

            var posts = await service.GetPostsAsync();

            Assert.NotNull(posts);
            Assert.Single(posts);
        }

        [Fact]
        public async Task GetPostsAsync_ReturnsEmptyList_WhenPostsDataIsNull()
        {
            var mockResponse = new
            {
                data = new
                {
                    posts = (object)null,
                }
            };
            var httpClient = CreateMockHttpClient(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(mockResponse), Encoding.UTF8, "application/json")
            });

            var service = new GraphQLService(httpClient);

            var posts = await service.GetPostsAsync();

            Assert.NotNull(posts);
            Assert.Empty(posts);
        }
    }
}
