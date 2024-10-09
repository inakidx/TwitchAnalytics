using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Numerics;
using TwitchAnalytics.Application.Interfaces;

namespace TwitchAnalytics.UI.API.Controllers;

[ApiController]
[Route("analytics/[controller]")]
public class UserController(ILogger<UserController> logger, IStreamerService streamerService) : ControllerBase
{
    private readonly ILogger<UserController> _logger = logger;
    private readonly IStreamerService _streamerService = streamerService;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string id)
    {
        _logger.LogInformation($"Request with Id:{id} paramether");
        if (BigInteger.Parse(id) <= 0)
        {
            _logger.LogError($"Bad request: {id}");
            return BadRequest("Invalid ID");
        }
        var streamer = await _streamerService.Get(id);
        _logger.LogInformation($"Ok, returned streamer: {JsonConvert.SerializeObject(streamer)}");
        return Ok(streamer);
    }
}
