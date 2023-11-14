using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace EpicCrmWebApi
{
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method,
    Inherited = true, AllowMultiple = true)]
    public class RequestCheckAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        // this is global filter; 

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ControllerContext cc = filterContext.Controller.ControllerContext;
            string controller = (string)cc.RouteData.Values["Controller"];
            string action = (string)cc.RouteData.Values["Action"];

            var request = HttpContext.Current.Request;
            string message = "";
            if (request.IsAuthenticated)
            {
                //bool logPortalRequest = Utils.ParseBoolString(ConfigurationManager.AppSettings["LogIncomingPortalRequest"]);
                bool logPortalRequest = Utils.SiteConfigData.LogIncomingPortalRequest;
                string userName = HttpContext.Current.User.Identity.Name;
                // log the incoming user and url
                message = $"User {userName} has requested {request.Url.PathAndQuery}";
                if (logPortalRequest)
                {
                    Business.LogError($"Portal", message, ">");
                }

                // check that user is active
                // user has to exist in ASPNetUsers table as a non expiring user (on the fly super admin can expire)
                // User should be active in TenantEmployee and Sales Person tables
                // (since SuperAdmin, SiteAdmin, Developer users don't exist in above two tables,
                //  hence checking if it is a SetupUser)
                ICollection<DashboardUser> dashboardUsers = Business.GetAllowedWebPortalUsers();
                bool allowUser = dashboardUsers != null
                                && dashboardUsers.Any(x => x.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase))
                                && (Helper.IsUserInAdminOrDeveloperRole(userName) || Business.IsUserAllowed(userName));

                if (allowUser == false)
                {
                    // log the user out and redirect to login page
                    message = $"User {userName} has tried to get {request.Url.PathAndQuery} - not allowed.";
                    HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    Business.LogError($"Portal", message, ">");
                    filterContext.Result = new RedirectResult("~/Dashboard");
                    return;
                }
            }

          
            base.OnActionExecuting(filterContext);
        }
    }
}