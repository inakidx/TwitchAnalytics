using Microsoft.AspNetCore.Mvc;
using TwitchAnalytics.Application.Interfaces;

namespace TwitchAnalytics.UI.API.Controllers;

[ApiController]
[Route("analytics/[controller]")]
public class StreamsController(ITwitchStreamService streamsService) : ControllerBase
{
    private readonly ITwitchStreamService _streamsService = streamsService;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var streams = await _streamsService.GetStreamsAlive();
        return Ok(streams);
    }
}
