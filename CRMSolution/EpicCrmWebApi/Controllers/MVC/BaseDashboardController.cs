using BusinessLayer;
using DomainEntities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EpicCrmWebApi
{
    public class BaseDashboardController : Controller
    {
        public BaseDashboardController()
        {
        }

        protected void LogModelErrors(string errorSource)
        {
            string errorText = GetModelErrors();
            if (String.IsNullOrEmpty(errorText) == false)
            {
                Business.LogError(errorSource, errorText, " ");
            }
        }

        protected string GetModelErrors()
        {
            if (!ModelState.IsValid)
            {
                // https://www.exceptionnotfound.net/asp-net-mvc-demystified-modelstate/
                string allModelErrors = String.Join("; ",
                            ModelState.Values
                            .SelectMany(ms => ms.Errors)
                            .Select(x =>
                            {
                                return String.IsNullOrEmpty(x.ErrorMessage) ? x.Exception.Message : x.ErrorMessage;
                            }));
                return allModelErrors;
            }
            return String.Empty;
        }

        protected IEnumerable<OfficeHierarchy> GetOfficeHierarchy() => Helper.GetOfficeHierarchy();

        protected string CurrentUserStaffCode => this.HttpContext.User.Identity.Name;
        protected bool IsSuperAdmin => Helper.IsSuperAdmin(this.HttpContext.User);
        protected bool IsSetupSuperAdmin => Helper.IsSetupSuperAdminUser(CurrentUserStaffCode);

        //private Lazy<IEnumerable<OfficeHierarchy>> _OfficeHierarchy = new Lazy<IEnumerable<OfficeHierarchy>>(() =>
        //{
        //    return Business.GetAssociations(System.Web.HttpContext.Current.User.Identity.Name);
        //});

        protected ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            set
            {
                _signInManager = value;
            }
        }

        protected ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        protected void SignoutCurrentUser()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        protected bool RemovePortalAccess(long empId)
        {
            bool returnStatus = false;
            string staffCode = Business.GetStaffCode(empId);
            // user sign up with their staff code only
            var userRecord = UserManager.FindByName(staffCode);
            if (userRecord != null)
            {
                RemoveUserFromRole(userRecord.Id, "Manager");
                RemoveUserFromRole(userRecord.Id, "Admin");
                IdentityResult result = UserManager.Delete(userRecord);
                returnStatus = (result.Succeeded);
            }

            return returnStatus;
        }

        //protected string NewXlsFileName =>
        //     HttpContext.Server.MapPath("~/App_Data/" + Guid.NewGuid().ToString() + ".xls");

        protected string NewXlsxFileName(string siteName, string tableName)
        {
            string fileName = $"{siteName.Replace(' ', '_')}_{tableName}_{Guid.NewGuid().ToString()}.xlsx";
            return Helper.GetStorageFileNameWithPath(fileName);

            //return HttpContext.Server.MapPath($"~/App_Data/{tenantName.Replace(' ', '_')}_{tableName}_{Guid.NewGuid().ToString()}.xlsx");
        }

        protected void PutFeatureSetInViewBag()
        {
            FeatureData featureControlModel = Helper.GetAvailableFeatures(CurrentUserStaffCode, IsSuperAdmin);
            if (IsSuperAdmin)
            {
                // don't show customer button on dashboard page to super admin
                // super admin can go to Admin page to see customers
                featureControlModel.CustomerFeature = false;
                featureControlModel.ProductFeature = false;
                featureControlModel.SalesPersonFeature = false;
            }
            ViewBag.FeatureSet = featureControlModel;
        }

        private void RemoveUserFromRole(string userId, string roleName)
        {
            if (UserManager.IsInRole(userId, roleName))
            {
                UserManager.RemoveFromRole(userId, roleName);
            }
        }

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
    }
}