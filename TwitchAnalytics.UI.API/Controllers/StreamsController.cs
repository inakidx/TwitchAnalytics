using Microsoft.AspNetCore.Mvc;
using TwitchAnalytics.Application.Interfaces;

namespace TwitchAnalytics.UI.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StreamsController(IStreamService streamsService) : ControllerBase
{
    private readonly IStreamService _streamsService = streamsService;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _streamsService.GetStreamsAlive());
    }
}
