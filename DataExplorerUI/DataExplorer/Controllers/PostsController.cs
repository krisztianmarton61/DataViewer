using DataExplorerModels;
using Microsoft.AspNetCore.Mvc;
using DataExplorerServices;
using Microsoft.Extensions.Logging;

namespace DataExplorer.Controllers;

[Route("api/posts")]
public class PostsController : ControllerBase
{
    private readonly ILogger<AlbumsController> _logger;
    private readonly GraphQLService _graphQLService;

    public PostsController(ILogger<AlbumsController> logger, GraphQLService graphQLService)
    {
        _logger = logger;
        _graphQLService = graphQLService;
    }

    [HttpGet(Name = "GetPosts")]
    public async Task<ActionResult<IEnumerable<Post>>> Get()
    {
        try
        {
            var posts = await _graphQLService.GetPostsAsync();
            return Ok(posts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching albums");
            return StatusCode(500, "Internal server error");
        }
    }
}
