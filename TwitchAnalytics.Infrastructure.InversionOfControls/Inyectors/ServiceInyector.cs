using Microsoft.Extensions.DependencyInjection;
using TwitchAnalytics.Application.Interfaces;
using TwitchAnalytics.Application.Services;
using TwitchAnalytics.Data.Core.AccessToken;

namespace TwitchAnalytics.Infrastructure.InversionOfControls.Inyectors;

internal class ServiceInyector
{
    public static void Inyect(IServiceCollection services)
    {
        services.AddScoped<AccessTokenService>();
        services.AddScoped<IStreamerService, StreamerService>();
        services.AddScoped<ITwitchStreamService, TwitchStreamService>();
    }
}
