using DataExplorerModels;
using DataExplorerUI.Configuration;
using Microsoft.Extensions.Options;

namespace DataExplorerUI.Services
{
    public class PostsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public PostsService(HttpClient httpClient, IOptions<ApiOptions> options)
        {
            _httpClient = httpClient;
            _baseUrl = options.Value.BaseUrl;
        }
        public async Task<List<Post>> GetPostsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/posts");

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Server returned {response.StatusCode}");

            var data = await response.Content.ReadFromJsonAsync<List<Post>>();
            return data ?? [];
        }
    }
}
