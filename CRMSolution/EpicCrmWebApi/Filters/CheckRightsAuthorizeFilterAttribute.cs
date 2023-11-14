using BusinessLayer;
using DomainEntities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EpicCrmWebApi
{
    // Attribute created to ensure that virtual admin user has the rights;
    [AttributeUsageAttribute(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CheckRightsAuthorizeAttribute : AuthorizeAttribute
    {
        private List<FeatureEnum> OrFeatureList = null;
        public CheckRightsAuthorizeAttribute()
        {
            //OrFeatureList = new List<FeatureEnum>();
        }

        public FeatureEnum Feature
        {
            get
            {
                return FeatureEnum.None;
            }
            set
            {
                OrFeatureList = new List<FeatureEnum>();
                OrFeatureList.Add(value);
            }
        }

        public FeatureEnum[] FeatureList
        {
            get
            {
                return null;
            }
            set
            {
                OrFeatureList = new List<FeatureEnum>(value);
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // custom auth for exclusion list
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            if (OrFeatureList == null)
            {
                return true;
            }

            FeatureData availableFeatures = Helper.GetAvailableFeatures(httpContext.User.Identity.Name, Helper.IsSuperAdmin(httpContext.User));
            if (availableFeatures == null)
            {
                return false;
            }

            // if any feature is enabled - good to go;
            bool status = false;
            foreach (FeatureEnum fe in OrFeatureList)
            {
                status = Helper.IsFeatureEnabled(fe, availableFeatures);
                if (status)
                {
                    break;
                }
            }

            if (status == false)
            {
                HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            }

            return status;
        }
    }
}