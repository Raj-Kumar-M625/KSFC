using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.Service;
using KAR.KSFC.Components.Data.Repository.UoW;

using Microsoft.Extensions.DependencyInjection;

namespace KAR.KSFC.Components.Data.Repository
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IEntityRepository<>), typeof(EntityRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
