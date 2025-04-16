namespace DataExplorerServices;
using DataExplorerModels;
using System.Text.Json;

public class GraphQLService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<List<Album>?> GetAlbumsAsync()
    {
        const string albumsQuery = @"
        query { 
            albums { 
                data { 
                    id title user { name username email phone website company { name } } 
                    photos { data { id title url } } 
                } 
            } 
        }";

        try
        {
            var query = new { query = albumsQuery };
            var response = await _httpClient.PostAsJsonAsync("https://graphqlzero.almansi.me/api", query);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return new List<Album>();
            }

            var result = await response.Content.ReadFromJsonAsync<GraphQLResponse<AlbumsResponse>>();

            if (result?.Data?.Albums?.Data == null)
            {
                Console.WriteLine("No albums found.");
                return new List<Album>();
            }

            return result.Data.Albums.Data;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request failed: {ex.Message}");
            return new List<Album>();
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error deserializing response: {ex.Message}");
            return new List<Album>();
        }
    }

    public async Task<List<Post>?> GetPostsAsync()
    {
        const string postsQuery = @"
        query { 
            posts { 
                data { 
                    id 
                    title 
                    body 
                } 
            } 
        }";

        try
        {
            var query = new { query = postsQuery };
            var response = await _httpClient.PostAsJsonAsync("https://graphqlzero.almansi.me/api", query);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return new List<Post>(); 
            }

            var result = await response.Content.ReadFromJsonAsync<GraphQLResponse<PostsResponse>>();

            if (result?.Data?.Posts?.Data == null)
            {
                Console.WriteLine("No posts found.");
                return new List<Post>(); 
            }

            return result.Data.Posts.Data;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request failed: {ex.Message}");
            return new List<Post>(); 
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error deserializing response: {ex.Message}");
            return new List<Post>();
        }
    }
}

public class AlbumsResponse
{
    public AlbumsPage Albums { get; set; } = new AlbumsPage();
}

public class AlbumsPage
{
    public List<Album> Data { get; set; } = new List<Album>();
}

public class PostsResponse
{
    public PostsPage Posts { get; set; } = new PostsPage();
}

public class PostsPage
{
    public List<Post> Data { get; set; } = new List<Post>();
}

public class GraphQLResponse<T>
{
    public T Data { get; set; }
}
