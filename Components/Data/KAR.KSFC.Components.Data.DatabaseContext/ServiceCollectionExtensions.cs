using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KAR.KSFC.Components.Data.DatabaseContext
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDataContextServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<DbFactory>();
            return services;
        }

    }
}
