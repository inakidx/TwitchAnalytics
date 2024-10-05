using Microsoft.AspNetCore.Mvc;
using TwitchAnalytics.Application.Interfaces;

namespace TwitchAnalytics.UI.API.Controllers;

[ApiController]
[Route("analytics/[controller]")]
public class StreamerController(ILogger<StreamerController> logger, IStreamerService streamerService) : ControllerBase
{
    private readonly ILogger<StreamerController> _logger = logger;
    private readonly IStreamerService _streamerService = streamerService;

    [HttpGet("id")]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await _streamerService.Get(id));
    }
}
