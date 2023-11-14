using Application;
using Identity;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Omu.AwesomeMvc;
using Persistence;
using Presentation.Helpers;
using SmartBreadcrumbs.Extensions;
using System;
using System.Reflection;

namespace MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddBreadcrumbs(typeof(Program).Assembly);
            services.ConfigureApplicationServices();
            services.ConfigureInfrastructureServices(Configuration);
            services.ConfigurePersistenceServices(Configuration);
            services.ConfigureIdentityServices(Configuration);
            services.AddHostedService<AutoTaskScheduler>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Index";
            });
            //services.AddHttpClient<IClient, Client>(cl => cl.BaseAddress = new Uri("https://localhost:5003"));
            //services.AddHttpClient<IClient, Client>(cl => cl.BaseAddress = new Uri(Configuration.GetSection("BackendHttps").GetSection("APIURL").Value));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(120);
                options.IOTimeout = TimeSpan.FromMinutes(120);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            var provider = new AweMetaProvider();
            services.AddMvc(o =>
            {
                o.ModelMetadataDetailsProviders.Add(provider);
            });
            services.AddMvc(o =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            services.AddCors(o =>
            {
                o.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsProduction())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Home/Error");
            }
            else
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //  app.UseHsts();
            }
            app.UseStatusCodePagesWithRedirects("/Error/HttpStatusCode?code={0}");
            //loggerFactory.AddFile($"D:\\Logging\\MVS_Logs\\Logs\\Log-1.txt");

            //app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("CorsPolicy");
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Identity}/{action=Index}/{id?}");
            });
        }
    }
}
