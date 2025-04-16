using DataExplorerModels;
using Microsoft.AspNetCore.Mvc;
using DataExplorerServices;
using Microsoft.Extensions.Logging;

namespace DataExplorer.Controllers;

[ApiController]
[Route("api/albums")]
public class AlbumsController : ControllerBase
{
    private readonly ILogger<AlbumsController> _logger;
    private readonly GraphQLService _graphQLService;

    public AlbumsController(ILogger<AlbumsController> logger, GraphQLService graphQLService)
    {
        _logger = logger;
        _graphQLService = graphQLService;
    }

    [HttpGet(Name = "GetAlbums")]
    public async Task<ActionResult<IEnumerable<Album>>> Get()
    {
        try
        {
            var albums = await _graphQLService.GetAlbumsAsync();
            return Ok(albums);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching albums");
            return StatusCode(500, "Internal server error");
        }
    }
}