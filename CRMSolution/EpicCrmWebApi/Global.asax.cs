using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web.Optimization;
using DomainEntities;
using System.Web.Hosting;
using Microsoft.AspNet.Identity;
using System.Configuration;
using Microsoft.AspNet.Identity.EntityFramework;
using CRMUtilities;

namespace EpicCrmWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            string url = Helper.GetCurrentSiteUrl(app.Request.Url);
            bool configRefreshed = Helper.CheckAndRefreshConfiguration(url);

            Utils.SiteConfigData = HttpContext.Current.Cache[Helper.ConfigKeyForSiteConfigurationData] as SiteConfigData;
            Utils.GlobalConfiguration = HttpContext.Current.Cache[Helper.ConfigKeyForGlobalConfigurationData] as ICollection<GlobalConfigData>;
            Utils.DatabaseServerConfiguration = HttpContext.Current.Cache[Helper.ConfigKeyForDatabaseServerData] as DBServer;

            if (configRefreshed)
            {
                Helper.CacheDbConnectionString();
            }

            Utils.DBConnectionString = HttpContext.Current.Cache[Helper.ConfigKeyForDbConnection] as String;
            Utils.EFConnectionString = HttpContext.Current.Cache[Helper.ConfigKeyForEfConnection] as String;

            if (Helper.IsValidConfiguration() == false)
            {
                HttpContext.Current.Response.Redirect("ErrorReadingConfig.html", true);
            }
            
            ApplicationDbContext context = ApplicationDbContext.Create();
            Helper.CreateRoles(context);
            Helper.CreateUsers(context);

            AddCustomViewSearchPaths();
        }

        private void AddCustomViewSearchPaths()
        {
            // https://www.ryadel.com/en/asp-net-mvc-add-custom-locations-to-the-view-engine-default-search-patterns/
            // https://archive.codeplex.com/?p=aspnetwebstack#src/System.Web.Mvc/RazorViewEngine.cs
            // https://dotnet.microsoft.com/apps/aspnet/apis
            // https://stackoverflow.com/questions/9582357/how-to-get-mvc-to-lookup-view-in-nested-folder

            List<string> searchPaths = new List<string>()
            {
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml"
            };

            string customViewFolder = Utils.SiteConfigData.CustomWebViewPath;
            if (string.IsNullOrEmpty(customViewFolder) == false)
            {
                customViewFolder = customViewFolder.Trim(new char[] { '/', ' ', '\\' });
            }
            if (string.IsNullOrEmpty(customViewFolder) == false)
            {
                string customPath = "~/Views/{1}/" + customViewFolder + "/{0}.cshtml";
                searchPaths.Insert(0, customPath);
            }

            RazorViewEngine razorEngine = ViewEngines.Engines.OfType<RazorViewEngine>().FirstOrDefault();
            razorEngine.ViewLocationFormats = searchPaths.ToArray();
            razorEngine.PartialViewLocationFormats = searchPaths.ToArray();

            // don't cache
            //razorEngine.ViewLocationCache = new DefaultViewLocationCache(TimeSpan.FromMilliseconds(0));
        }
    }
}
