using Guestlogix.Bll;
using Microsoft.Extensions.DependencyInjection;

namespace Guestlogix.Api.Config
{
    public static class ServicesConfig
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {

            services.AddTransient<IRouteService, RouteService>();

            return services;
        }
    }
}
