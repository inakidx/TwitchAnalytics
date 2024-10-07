using Microsoft.AspNetCore.Mvc;
using TwitchAnalytics.Application.Interfaces;

namespace TwitchAnalytics.UI.API.Controllers;

[ApiController]
[Route("analytics/[controller]")]
public class UserController(ILogger<UserController> logger, IStreamerService streamerService) : ControllerBase
{
    private readonly ILogger<UserController> _logger = logger;
    private readonly IStreamerService _streamerService = streamerService;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid ID");
        }
        var streamer = await _streamerService.Get(id);
        return Ok(streamer);
    }
}
