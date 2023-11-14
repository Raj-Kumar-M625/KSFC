using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EpicCrmWebApi
{
    // Attribute created to ensure that user is in the listed roles
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method,
    Inherited = true, AllowMultiple = true)]
    public class AuthorizeAndAttribute : AuthorizeAttribute
    {
        private List<string> inclusionList = null;
        public AuthorizeAndAttribute()
        {
            inclusionList = new List<string>();
        }

        public string AndRole
        {
            get { return ""; }
            set
            {
                inclusionList.Add(value);
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // custom auth for exclusion list
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            foreach (string roleName in inclusionList)
            {
                if (!httpContext.User.IsInRole(roleName))
                {
                    HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    return false;
                }
            }

            return base.AuthorizeCore(httpContext);
        }
    }
}