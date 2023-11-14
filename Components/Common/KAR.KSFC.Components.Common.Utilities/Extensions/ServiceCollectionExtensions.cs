using AutoMapper;
using KAR.KSFC.Components.Common.Utilities.MapperProfile;
using Microsoft.Extensions.DependencyInjection;

namespace KAR.KSFC.Components.Common.Utilities.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAutoMapper(this IServiceCollection services)
        {
            // Auto Mapper Configurations  (to bind model data to viewmodel)
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }

        //public static IServiceCollection Swagger(this IServiceCollection services)
        //{
        //    services.AddSwaggerGen(c =>
        //    {
        //        c.SwaggerDoc("v1", new OpenApiInfo { Title = "KAR.KSFC.API", Version = "v1" });
        //        c.OperationFilter<HeaderParameter>();
        //        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        //        {
        //            Name = "Authorization",
        //            Type = SecuritySchemeType.Http,
        //            Scheme = "Bearer",
        //            BearerFormat = "JWT",
        //            In = ParameterLocation.Header,
        //            Description = "JWT Authorization header using the Bearer scheme."
        //        });

        //        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        //        {
        //            {
        //                  new OpenApiSecurityScheme
        //                    {
        //                        Reference = new OpenApiReference
        //                        {
        //                            Type = ReferenceType.SecurityScheme,
        //                            Id = "Bearer"
        //                        }
        //                    },
        //                  new string[]{ } // Array.Empty<string>()
        //            }
        //        });

        //        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        //        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        //        c.IncludeXmlComments(xmlPath);

        //    });

        //    return services;
        //}
    }
}
