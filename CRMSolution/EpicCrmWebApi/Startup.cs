using System;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;
using System.Text;
using System.Web;

[assembly: OwinStartupAttribute(typeof(EpicCrmWebApi.Startup))]
namespace EpicCrmWebApi
{
    public partial class Startup
    {
        // this is called when web server starts
        // we must get configuration here as well - as configuration contains
        // db server details and Configure Auth method calls ApplicationDbContext.Create
        // which needs connection string.

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
