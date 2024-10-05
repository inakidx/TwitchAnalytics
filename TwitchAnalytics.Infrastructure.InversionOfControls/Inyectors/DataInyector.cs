using Microsoft.Extensions.DependencyInjection;
using TwitchAnalytics.Data.Repositories;
using TwitchAnalytics.Domain.Repositories;

namespace TwitchAnalytics.Infrastructure.InversionOfControls.Inyectors;

public static class DataInyector
{
    public static void Inyect(IServiceCollection services)
    {
        services.AddScoped<IStreamerRepository, StreamerRepository>();
    }
}