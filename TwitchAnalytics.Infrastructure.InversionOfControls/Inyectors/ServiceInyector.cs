using Microsoft.Extensions.DependencyInjection;
using TwitchAnalytics.Application.Interfaces;
using TwitchAnalytics.Application.Services;

namespace TwitchAnalytics.Infrastructure.InversionOfControls.Inyectors;

internal class ServiceInyector
{
    public static void Inyect(IServiceCollection services)
    {
        services.AddScoped<IStreamerService, StreamerService>();
        services.AddScoped<IStreamService, TwitchStreamService>();
    }
}
