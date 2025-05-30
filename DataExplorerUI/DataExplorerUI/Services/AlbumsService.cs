using DataExplorerModels;
using DataExplorerUI.Configuration;
using Microsoft.Extensions.Options;

namespace DataExplorerUI.Services
{
    public class AlbumsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public AlbumsService(HttpClient httpClient, IOptions<ApiOptions> options)
        {
            _httpClient = httpClient;
            _baseUrl = options.Value.BaseUrl;
        }
        public async Task<List<Album>> GetAlbumsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/albums");

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Server returned {response.StatusCode}");

            var albums = await response.Content.ReadFromJsonAsync<List<Album>>();
            return albums ?? [];
        }
    }
}
