using KAR.KSFC.API.Filters;
using KAR.KSFC.API.ServiceFacade;
using KAR.KSFC.API.ServiceFacade.External.Interface;
using KAR.KSFC.API.ServiceFacade.External.Service;
using KAR.KSFC.Components.Common.Dto.Email;
using KAR.KSFC.Components.Common.Logging;
using KAR.KSFC.Components.Common.Logging.Enrichers;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Extensions;
using KAR.KSFC.Components.Common.Utilities.Middlewares;
using KAR.KSFC.Components.Common.Utilities.UserIdentity;
using KAR.KSFC.Components.Data.DatabaseContext;
using KAR.KSFC.Components.Data.Models;
using KAR.KSFC.Components.Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace KAR.KSFC.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment Env { get; set; }


        //// This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //sets the environment specific appsettings.json file
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.{Env.EnvironmentName ?? ""}.json", optional: true, reloadOnChange: true)
            .Build();
            services.AddControllers(options=>options.Filters.Add(typeof(SetLogContext)))
                .AddNewtonsoftJson(options =>
                   options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);//microsoft has removed text.json instead need to use newtonsoft.json for .net Core 3.1 or later
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);//We set Time here 
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.RegisterDataModelServices(Configuration);
            services.RegisterDataContextServices(Configuration);
            services.RegisterRepositoryServices();
            services.RegisterAppServices();
            services.AddAuthentication();
            services.AddAuthorization();
            services.RegisterAutoMapper();
            //services.RegisterSeriLogger();
            services.RegisterLoggingService(Env.EnvironmentName,new List<EnricherTypeEnum>() {EnricherTypeEnum.RequestHeaders,EnricherTypeEnum.UserClaims });
           

            //services.AddSingleton(Log.Logger);
            services.AddUserInformation();
            services.JwtService(Configuration);
            services.AddHttpClient("SMSApi", c =>
            {
                c.BaseAddress = new Uri(Configuration["SMS:Url"]);
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage);

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "KAR.KSFC.API", Version = "v1" });
                c.OperationFilter<HeaderParameter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                          new string[]{ } // Array.Empty<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailService, EmailService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "KAR.KSFC.API v1");
            });

            app.UseHttpsRedirection();
            app.UseSession();
            app.UseRouting();
            app.CustomJwtMiddleware();
            app.UseAuthentication();
            app.UseAuthorization();
            // app.UseBasicAuthMiddleware();//To get to know who is calling our api.

            app.CustomExceptionMiddleware();
            app.CustomUserInfoMiddleware();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
