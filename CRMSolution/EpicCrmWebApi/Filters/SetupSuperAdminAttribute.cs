using BusinessLayer;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EpicCrmWebApi
{
    // Attribute created to ensure that user is a Setup Super Admin
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method,
    Inherited = true, AllowMultiple = true)]
    public class SetupSuperAdminAttribute : AuthorizeAttribute
    {
        public SetupSuperAdminAttribute()
        {
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            string userName = httpContext.User.Identity.Name;
            bool status = Helper.IsSetupSuperAdminUser(userName);

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