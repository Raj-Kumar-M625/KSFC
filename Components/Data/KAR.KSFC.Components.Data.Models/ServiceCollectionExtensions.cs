using System;

using KAR.KSFC.Components.Data.Models.DbModels;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KAR.KSFC.Components.Data.Models
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDataModelServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ksfccsgContext>(options =>
            options.UseMySQL(configuration.GetConnectionString("MyConnection")));
            services.AddScoped<Func<ksfccsgContext>>((provider) => () => provider.GetService<ksfccsgContext>());
            return services;
        }
    }
}
