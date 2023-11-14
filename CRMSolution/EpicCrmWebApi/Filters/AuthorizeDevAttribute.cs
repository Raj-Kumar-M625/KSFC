using BusinessLayer;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EpicCrmWebApi
{
    // Attribute created to ensure that user is either developer or super admin
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method,
    Inherited = true, AllowMultiple = true)]
    public class AuthorizeDevAttribute : AuthorizeAttribute
    {
        public AuthorizeDevAttribute()
        {
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            //bool status = httpContext.User.IsInRole("Developer") ||
            //    (httpContext.User.IsInRole("Admin") && httpContext.User.IsInRole("Manager"));

            string userName = httpContext.User.Identity.Name;
            bool status = (Helper.IsSetupSuperAdminUser(userName) || Helper.GetAvailableFeatures(userName).SuperAdminPage);

            if (status == false)
            {
                string message = $"User {HttpContext.Current.User.Identity.Name} has tried to get {HttpContext.Current.Request.Url.PathAndQuery} - not allowed.";
                HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                Business.LogError($"Portal", message, ">");
            }

            return status;

            //return base.AuthorizeCore(httpContext);
        }
    }
}