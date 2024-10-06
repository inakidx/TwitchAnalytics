using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TwitchAnalytics.Application.Interfaces;
using TwitchAnalytics.Authorization;

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
        if (id <= 0)
        {
            return BadRequest("Invalid ID");
        }

        string? authorizationHeader = HttpContext.Request.Headers.Authorization;

        AuthorizationManager authorizationManager = new(authorizationHeader);

        if (!authorizationManager.IsTokenValid)
        {
            authorizationManager.GenerateNewToken();
            if (!authorizationManager.IsTokenValid)
            {
                return StatusCode(500);
            }
        }

        return Ok(await _streamerService.Get(id));
    }
}
