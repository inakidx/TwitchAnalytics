using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TwitchAnalytics.Application.Interfaces;

namespace TwitchAnalytics.UI.API.Controllers;

[ApiController]
[Route("analytics/[controller]")]
public class StreamsController(ILogger<StreamsController> logger, ITwitchStreamService streamsService) : ControllerBase
{
    private readonly ILogger<StreamsController> _logger = logger;
    private readonly ITwitchStreamService _streamsService = streamsService;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation($"Get");

        var streams = await _streamsService.GetStreamsAlive();
        _logger.LogInformation($"Returned: {JsonConvert.SerializeObject(streams)}");

        return Ok(streams);
    }
}
