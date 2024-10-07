using TwitchAnalytics.Infrastructure.InversionOfControls.Inyectors;
using Microsoft.Extensions.DependencyInjection;

namespace TwitchAnalytics.Infrastructure.InversionOfControls;

public static class ServiceCollecionExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        DataInyector.Inyect(services);
        ServiceInyector.Inyect(services);
    }
}
