using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EpicCrmWebApi
{
    // Attribute created to ensure that user is not in the listed roles
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method,
    Inherited = true, AllowMultiple = true)]
    public class ExcludeRoleAttribute : AuthorizeAttribute
    {
        private List<string> exclusionList = null;
        public ExcludeRoleAttribute()
        {
            exclusionList = new List<string>();
        }

        public string XRole
        {
            get { return ""; }
            set
            {
                exclusionList.Add(value);
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // custom auth for exclusion list
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            foreach (string roleName in exclusionList)
            {
                if (httpContext.User.IsInRole(roleName))
                {
                    HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    return false;
                }
            }

            return base.AuthorizeCore(httpContext);
        }
    }
}