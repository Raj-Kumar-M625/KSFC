using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Web;
using EpicCrmWebApi.Models;

namespace EpicCrmWebApi
{
    public static class Helper
    {
        public static string GetCrmUserName(this IIdentity identity)
        {
            if (identity.IsAuthenticated == false)
            {
                return "";
            }

            string aspnetUserName = identity.Name;
            if (String.IsNullOrEmpty(aspnetUserName))
            {
                return "";
            }

            if (IsUserInAdminOrDeveloperRole(aspnetUserName))
            {
                return aspnetUserName;
            }

            // get user name from CRM tables;
            // aspnetUserName - is basically staffcode/employeeCode
            return Business.GetTenantEmployeeName(aspnetUserName);
        }

        //internal static bool IsSetupUser(string userName)
        //{
        //    return Helper.IsUserInAdminOrDeveloperRole(userName);
        //    //return userName.Equals(ConfigurationManager.AppSettings["AdminUserName"]) ||
        //    //    userName.Equals(ConfigurationManager.AppSettings["SuperAdminUserName"]) ||
        //    //    userName.Equals(ConfigurationManager.AppSettings["DeveloperUserName"]);
        //}

        /// <summary>
        /// This method returns true, if user is Super Admin as defined in web.config
        /// and thereby has been set up at the time of application first run.
        /// (Other Super admin user can be created by Setup Super Admin User and
        ///  this method is to distinguish between the two)
        /// </summary>
        /// <returns></returns>
        public static bool IsSetupSuperAdminUser(string userName)
        {
            return userName.Equals(Utils.SiteConfigData.SuperAdminUserName, StringComparison.OrdinalIgnoreCase);
        }

        internal static bool CreateUser(UserManager<ApplicationUser> userManager,
                            string userName,
                            string userEmail,
                            string userPassword,
                            bool enableLockout,
                            string[] rolesToAssign)
        {
            bool status = false;
            if (userManager.FindByName(userName) == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = userName,
                    Email = userEmail,
                    LockoutEnabled = enableLockout
                };

                var chkUser = userManager.Create(user, userPassword);

                //Add default User to Role Admin
                if (chkUser.Succeeded)
                {
                    foreach (string r in rolesToAssign)
                    {
                        userManager.AddToRole(user.Id, r);
                    }
                    status = true;
                }
            }
            return status;
        }

        internal static bool CreateUser(string userName, string userPassword, bool enableLockout, string[] rolesToAssign)
        {
            ApplicationDbContext context = ApplicationDbContext.Create();
            using (var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context)))
            {
                string userEmail = $"{Guid.NewGuid().ToString()}@b.com";
                return CreateUser(UserManager, userName, userEmail, userPassword, enableLockout, rolesToAssign);
            }
        }

        internal static ICollection<string> GetUserRoles(string userName)
        {
            ApplicationDbContext context = ApplicationDbContext.Create();
            using (var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context)))
            {
                ApplicationUser appUser = UserManager.FindByName(userName);
                if (appUser == null)
                {
                    return new List<string>();
                }

                return UserManager.GetRoles(appUser.Id);
            }
        }

        internal static bool IsUserInAdminOrDeveloperRole(string userName)
        {
            return GetUserRoles(userName).Any(x => x.Equals("Admin", StringComparison.OrdinalIgnoreCase)
                                            || x.Equals("Developer", StringComparison.OrdinalIgnoreCase));
        }

        internal static void EnsureRoleExists(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!roleManager.RoleExists(roleName))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = roleName;
                roleManager.Create(role);
            }
        }

        internal static void SetAutoDeletionTimeForUser(string userName, DateTime autoDisableUtcTimeForUser)
        {
            ApplicationDbContext context = ApplicationDbContext.Create();
            using (var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context)))
            {
                ApplicationUser appUser = UserManager.FindByName(userName);
                if (appUser != null)
                {
                    appUser.DisableUserAfterUtc = autoDisableUtcTimeForUser;
                    context.SaveChanges();
                }
            }
        }

        internal static void DeletePortalLogin(string userName)
        {
            ApplicationDbContext context = ApplicationDbContext.Create();
            using (var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context)))
            {
                ApplicationUser appUser = UserManager.FindByName(userName);
                if (appUser != null)
                {
                    UserManager.Delete(appUser);
                }
            }
        }

        internal static bool IsGradeFeatureEnabled() => Utils.SiteConfigData.SalesPersonGradeEnabled;

        // Server is running in different time zone, so get utc time and convert to IST
        public static DateTime GetCurrentIstDateTime()
        {
            return ConvertUtcTimeToIst(DateTime.UtcNow);
        }

        public static DateTime ConvertUtcTimeToIst(DateTime utcDateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        }

        public static IEnumerable<SmsSummaryDataModel> GetModelForPeopleInField(long tenantId, DateTime processingDate,
                                ExecAppRollupEnum rollUp, IEnumerable<string> managingCodes = null)
        {

            Func<Geography, string> selector = null;
            string managerType = "";
            switch (rollUp)
            {
                case ExecAppRollupEnum.Zone:
                    selector = (x) => x.ZoneCode;
                    managerType = "Zone";
                    break;
                case ExecAppRollupEnum.Area:
                    selector = (x) => x.AreaCode;
                    managerType = "AreaOffice";
                    break;
                case ExecAppRollupEnum.Territory:
                    selector = (x) => x.TerritoryCode;
                    managerType = "Territory";
                    break;
                case ExecAppRollupEnum.HQ:
                    selector = (x) => x.HQCode;
                    managerType = "HeadQuarter";
                    break;
            }

            ICollection<SmsDataEx> smsData = Business.GetSmsDetailData(tenantId, processingDate);
            // filter the data for only managing codes
            if (managingCodes != null)
            {
                smsData = smsData.Where(x => managingCodes.Any(y => y.Equals(selector(x), StringComparison.OrdinalIgnoreCase))).ToList();
            }

            IEnumerable<CodeTableEx> offices = Business.GetCodeTable(managerType);
            ICollection<SmsSummary> smsSummary = Business.GetSmsSummaryData(smsData, offices, selector);

            // Get Customer Totals as well
            IEnumerable<CustomerData> customerTotals = Business.GetCustomerTotal(tenantId, selector);

            // Get all Managers
            IEnumerable<SalesPersonEx> managers = Business.GetAssignedManagers(managerType);

            IEnumerable<SmsSummaryDataModel> smsSummaryDataModel =
                (from x in smsSummary
                 join t in customerTotals on x.Code equals t.Code into custOuter
                 from co in custOuter.DefaultIfEmpty()
                 select new SmsSummaryDataModel()
                 {
                     Name = x.Name,
                     Code = x.Code,
                     CurrentlyRoamingCount = smsData.Where(y => selector(y) == x.Code).Count(b => b.IsInFieldToday && b.EndTime.HasValue == false),
                     InFieldCount = x.InFieldCount,
                     RegisteredCount = x.RegisteredCount,
                     HeadCount = x.HeadCount,
                     Orders = x.TotalOrderAmount,
                     Returns = x.TotalReturnAmount,
                     Payments = x.TotalPaymentAmount,
                     Expenses = x.TotalExpenseAmount,
                     Activities = x.TotalActivityCount,

                     CustomerCount = co?.CustomerCount ?? 0,
                     Outstanding = co?.TotalOutstanding ?? 0M,
                     LongOutstanding = co?.TotalLongOutstanding ?? 0M,
                     Target = co?.TotalTarget ?? 0M,

                     Details = smsData.Where(y => selector(y) == x.Code).Select(y => new SmsDetailDataModel()
                     {
                         StaffCode = y.StaffCode,
                         IsInFieldToday = y.IsInFieldToday,
                         IsRegisteredOnPhone = y.IsRegisteredOnPhone,
                         Name = y.Name,
                         Phone = y.Phone,
                         Orders = y.TotalOrderAmount,
                         Payments = y.TotalPaymentAmount,
                         Expenses = y.TotalExpenseAmount,
                         Returns = y.TotalReturnAmount,
                         Activities = y.TotalActivityCount,
                         StartTime = y.StartTime,
                         EndTime = y.EndTime,
                         Latitude = y.Latitude,
                         Longitude = y.Longitude,
                         CurrentLocTime = y.CurrentLocTime,
                         PhoneModel = y.PhoneModel,
                         PhoneOS = y.PhoneOS,
                         AppVersion = y.AppVersion
                     }).ToList(),
                     Managers = managers.Where(m => m.AssociationCode == x.Code).ToList()
                 }).ToList();

            return smsSummaryDataModel;
        }

        /// <summary>
        /// We are defining rights for virtual super admins - so this
        /// method is used to get rights for any given user;
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static FeatureData GetAvailableFeatures(string userName, bool isSuperAdmin)
        {
            FeatureData fc = new FeatureData()
            {
                UserName = userName
            };

            // Restriction applies only to virtual super admin accounts
            if (Helper.IsSetupSuperAdminUser(userName) || !isSuperAdmin)
            {
                fc.ActivityFeature = true;
                fc.OrderFeature = true;
                fc.PaymentFeature = true;
                fc.OrderReturnFeature = true;
                fc.ExpenseFeature = true;
                fc.IssueReturnFeature = true;
                fc.EntityFeature = true;
                fc.ExpenseReportFeature = true;
                fc.FieldActivityReportFeature = true;
                fc.EntityProgressReportFeature = true;
                fc.AttendanceReportFeature = true;
                fc.AttendanceSummaryReportFeature = true;
                fc.AttendanceRegisterReportFeature = true;
                fc.AbsenteeReportFeature = true;
                fc.AppSignUpReportFeature = true;
                fc.AppSignInReportFeature = true;
                fc.ActivityReportFeature = true;
                fc.ActivityByTypeReportFeature = true;
                fc.KPIFeature = true;
                fc.MAPFeature = true;
                fc.EmployeeExpenseReport = true;
                fc.DistanceReport = true;
                fc.SalesPersonFeature = true;
                fc.CustomerFeature = true;
                fc.ProductFeature = true;
                fc.CrmUserFeature = true;
                fc.AssignmentFeature = true;
                fc.UploadDataFeature = true;
                fc.OfficeHierarchyFeature = true;
                fc.BankAccountFeature = true;
                fc.GstRateFeature = true;
                fc.WorkFlowFeature = true;
                fc.RedFarmerModule = true;
                fc.DistantActivityReport = true;
                fc.AdvanceRequestModule = true;
                fc.EmployeeExpenseReport = true;
                fc.EmployeeExpenseReport2 = true;
                fc.UnSownReport = true;
                fc.IsReadOnlyUser = false;
                fc.IssueReturnFeature = true;
                fc.STRFeature = true;
                fc.STRWeighFeature = true;
                //fc.STRSiloFeature = true;
                fc.DWSApproveWeightOption = true;
                fc.DWSApproveAmountOption = true;
                fc.DWSPaymentOption = true;
                fc.DWSPaymentReportFeature = true;

                // Author - SA, kartik; Date:20/05/2021; Purpose: VendorPayment feature

                fc.TransporterPaymentReportFeature = true;
                //fc.AddOrApproveTransporterPayment = fc.TransporterPaymentOption

                // June 10 2020
                fc.StockReceiveOption = true;
                fc.StockReceiveConfirmOption = true;
                fc.StockRequestOption = true;
                fc.StockRequestFulfillOption = true;
                fc.ExtraAdminOption = true;

                fc.StockLedgerOption = fc.StockBalanceOption = fc.StockRemoveOption = true;
                fc.StockRemoveConfirmOption = true;

                fc.StockAddOption = fc.StockAddConfirmOption = true;

                // April 20 2021
                //fc.BonusCalculationOption = fc.BonusPaymentReport = true;
                fc.BonusCalculationPaymentOption = true;
                fc.SurveyFormReport = true;

                // Author:PankajKumar; Purpose: Day Planning; Date: 26/04/2021
                fc.DayPlanReport = true;
                fc.QuestionnaireFeature = true;

                // Author:Rajesh V; Purpose: Farmer Summary Report; Date: 08/10/2021
                fc.FarmerSummaryReport = true;

                // Author:Kartik; Purpose: Followup Task; Date: 14/09/2021
                fc.ProjectOption = fc.FollowUpTaskOption = true;

                fc.LeaveFeature = true;
                fc.DealersNotMetReport = true;
                fc.DealersSummaryReport = true;
                fc.GeoTagReport = true;
                fc.AgreementsReport = true;
                fc.DuplicateFarmersReport= true;
                fc.FarmersBankAccountReport = true;

                return fc;
            }

            // for virtual super admins - retrieve rights;
            FeatureControl adminRights = Business.GetVirtualSuperAdminWithRights(userName);
            if (adminRights != null)
            {
                fc.ActivityFeature = adminRights.ActivityFeature;
                fc.OrderFeature = adminRights.OrderFeature;
                fc.PaymentFeature = adminRights.PaymentFeature;
                fc.OrderReturnFeature = adminRights.OrderReturnFeature;
                fc.ExpenseFeature = adminRights.ExpenseFeature;
                fc.IssueReturnFeature = adminRights.IssueReturnFeature;

                fc.ExpenseReportFeature = adminRights.ExpenseReportFeature;
                fc.FieldActivityReportFeature = adminRights.FieldActivityReportFeature;
                fc.EntityProgressReportFeature = adminRights.EntityProgressReportFeature;
                fc.AttendanceReportFeature = adminRights.AttendanceReportFeature;
                fc.AttendanceSummaryReportFeature = adminRights.AttendanceSummaryReportFeature;
                fc.AttendanceRegisterReportFeature = adminRights.AttendanceRegister;

                fc.AbsenteeReportFeature = adminRights.AbsenteeReportFeature;
                fc.AppSignUpReportFeature = adminRights.AppSignUpReportFeature;
                fc.AppSignInReportFeature = adminRights.AppSignInReportFeature;
                fc.ActivityReportFeature = adminRights.ActivityReportFeature;
                fc.ActivityByTypeReportFeature = adminRights.ActivityByTypeReportFeature;
                fc.KPIFeature = adminRights.KPIFeature;
                fc.MAPFeature = adminRights.MAPFeature;
                fc.EmployeeExpenseReport = adminRights.EmployeeExpenseReport;
                fc.DistanceReport = adminRights.DistanceReport;

                fc.SalesPersonFeature = adminRights.SalesPersonFeature;
                fc.CustomerFeature = adminRights.CustomerFeature;
                fc.ProductFeature = adminRights.ProductFeature;
                fc.CrmUserFeature = adminRights.CrmUserFeature;
                fc.AssignmentFeature = adminRights.AssignmentFeature;
                fc.UploadDataFeature = adminRights.UploadDataFeature;
                fc.OfficeHierarchyFeature = adminRights.OfficeHierarchyFeature;
                fc.SuperAdminPage = adminRights.SuperAdminPage;
                fc.BankAccountFeature = adminRights.BankAccountFeature;
                fc.EntityFeature = adminRights.EntityFeature;
                fc.GstRateFeature = adminRights.GstRateFeature;

                fc.WorkFlowFeature = true;  // this option is not set on virtual super admin rights page

                fc.DistantActivityReport = adminRights.DistantActivityReport;
                fc.RedFarmerModule = adminRights.RedFarmerModule;
                fc.AdvanceRequestModule = adminRights.AdvanceRequestModule;

                fc.EmployeeExpenseReport = adminRights.EmployeeExpenseReport;
                fc.EmployeeExpenseReport2 = adminRights.EmployeeExpenseReport2;
                fc.UnSownReport = adminRights.UnSownReport;
                fc.IsReadOnlyUser = adminRights.IsReadOnlyUser;

                fc.STRFeature = adminRights.STRFeature;
                fc.STRWeighFeature = adminRights.STRWeighControl;
                //fc.STRSiloFeature = adminRights.STRSiloControl;

                fc.DWSApproveWeightOption = adminRights.DWSApproveWeightOption;
                fc.DWSApproveAmountOption = adminRights.DWSApproveAmountOption;
                fc.DWSPaymentOption = adminRights.DWSPaymentOption;

                fc.DWSPaymentReportFeature = adminRights.DWSPaymentReport;
                fc.TransporterPaymentReportFeature = adminRights.TransporterPaymentReport;

                // June 10 2020
                fc.StockReceiveOption = adminRights.StockReceiveOption;
                fc.StockReceiveConfirmOption = adminRights.StockReceiveConfirmOption;
                fc.StockRequestOption = adminRights.StockRequestOption;
                fc.StockRequestFulfillOption = adminRights.StockRequestFulfillOption;
                fc.ExtraAdminOption = adminRights.ExtraAdminOption;

                fc.StockLedgerOption = adminRights.StockLedgerOption;
                fc.StockBalanceOption = adminRights.StockBalanceOption;
                fc.StockRemoveOption = adminRights.StockRemoveOption;
                fc.StockRemoveConfirmOption = adminRights.StockRemoveConfirmOption;

                fc.StockAddOption = adminRights.StockAddOption;
                fc.StockAddConfirmOption = adminRights.StockAddConfirmOption;

                // April 20 2021
                //fc.BonusCalculationOption = adminRights.BonusCalculationOption;
                fc.BonusCalculationPaymentOption = adminRights.BonusCalculationPaymentOption;
                //fc.BonusPaymentReport = adminRights.BonusPaymentReport;
                fc.SurveyFormReport = adminRights.SurveyFormReport;
                fc.DayPlanReport = adminRights.DayPlanReport;
                fc.QuestionnaireFeature = adminRights.QuestionnaireFeature;
                fc.FarmerSummaryReport = adminRights.FarmerSummaryReport;
                fc.ProjectOption = adminRights.ProjectOption;
                fc.FollowUpTaskOption = adminRights.FollowUpTaskOption;
                fc.LeaveFeature = true;
                fc.DealersNotMetReport = adminRights.DealersNotMetReport;
                fc.DealersSummaryReport = adminRights.DealersSummaryReport;
                fc.GeoTagReport = adminRights.GeoTaggingReport;
                fc.AgreementsReport = adminRights.AgreementsReport;
                fc.DuplicateFarmersReport = adminRights.DuplicateFarmersReport;
                fc.FarmersBankAccountReport = adminRights.FarmersBankAccountReport;
                // Author - SA; Date:20/05/2021; Purpose: VendorPayment feature
                //fc.AddOrApproveTransporterPayment = adminRights.AddOrApproveTransporterPayment;
                //fc.TransporterPaymentOption = adminRights.TransporterPaymentOption;
            }

            return fc;
        }

        /// <summary>
        /// We are defining rights for virtual super admins - so this
        /// method is used to get rights for any given user;
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static FeatureData GetAvailableFeatures(string userName)
        {
            FeatureData fc = new FeatureData()
            {
                UserName = userName
            };

            // retrieve rights for the user - if user is in FeatureControl table
            FeatureControl availableFeatures = Business.GetVirtualSuperAdminWithRights(userName);
            if (availableFeatures != null)
            {
                Utils.CopyProperties(availableFeatures, fc);
            }

            return fc;
        }

        // returns a tuple to indicate if user is to be treated as super admin
        // for the purpose of applying security;  If not to be treated as super admin
        // then what is the user to be used for security context.  Remember, that
        // for virtual super admin, security can be applied for a different user;
        // August 13 2019;
        //
        public static Tuple<bool, string> GetSecurityContextUser(IPrincipal user)
        {
            string userNameInPrincipal = user.Identity.Name;
            bool isSetupSuperAdmin = Helper.IsSetupSuperAdminUser(userNameInPrincipal);
            if (isSetupSuperAdmin)
            {
                return new Tuple<bool, string>(true, userNameInPrincipal);
            }

            bool isSuperAdmin = IsSuperAdmin(user);
            if (isSuperAdmin == false)
            {
                return new Tuple<bool, string>(false, userNameInPrincipal);
            }

            // now the user is a virtual super admin - get its data and check context user set for it.
            FeatureControl fc = Business.GetVirtualSuperAdminWithRights(userNameInPrincipal);
            if (fc?.IsDefaultSecurityContext ?? true)
            {
                // return associations as that for Super Admin
                return new Tuple<bool, string>(true, userNameInPrincipal);
            }
            else
            {
                return new Tuple<bool, string>(false, fc.SecurityContextUser);
            }
        }

        public static bool IsSuperAdmin(IPrincipal user) => user.IsInRole("Admin") && user.IsInRole("Manager");

        public static bool IsFeatureEnabled(FeatureEnum feature, FeatureData availableFeatures)
        {
            bool status = false;
            switch (feature)
            {
                case FeatureEnum.ActivityFeature:
                    status = availableFeatures.ActivityFeature;
                    break;
                case FeatureEnum.OrderFeature:
                    status = availableFeatures.OrderFeature;
                    break;
                case FeatureEnum.PaymentFeature:
                    status = availableFeatures.PaymentFeature;
                    break;
                case FeatureEnum.OrderReturnFeature:
                    status = availableFeatures.OrderReturnFeature;
                    break;
                case FeatureEnum.ExpenseFeature:
                    status = availableFeatures.ExpenseFeature;
                    break;
                case FeatureEnum.IssueReturnFeature:
                    status = availableFeatures.IssueReturnFeature;
                    break;
                case FeatureEnum.EntityFeature:
                    status = availableFeatures.EntityFeature;
                    break;
                case FeatureEnum.ExpenseReportFeature:
                    status = availableFeatures.ExpenseReportFeature;
                    break;
                case FeatureEnum.WorkFlowReportFeature://This is for a FieldActivityReport
                    status = availableFeatures.FieldActivityReportFeature;
                    break;
                case FeatureEnum.EntityProgressReportFeature:
                    status = availableFeatures.EntityProgressReportFeature;
                    break;
                case FeatureEnum.AttendanceReportFeature:
                    status = availableFeatures.AttendanceReportFeature;
                    break;
                case FeatureEnum.AttendanceSummaryReportFeature:
                    status = availableFeatures.AttendanceSummaryReportFeature;
                    break;
                case FeatureEnum.AttendanceRegisterReportFeature:
                    status = availableFeatures.AttendanceRegisterReportFeature;
                    break;

                case FeatureEnum.AbsenteeReportFeature:
                    status = availableFeatures.AbsenteeReportFeature;
                    break;
                case FeatureEnum.AppSignUpReportFeature:
                    status = availableFeatures.AppSignUpReportFeature;
                    break;
                case FeatureEnum.AppSignInReportFeature:
                    status = availableFeatures.AppSignInReportFeature;
                    break;
                case FeatureEnum.ActivityReportFeature:
                    status = availableFeatures.ActivityReportFeature;
                    break;
                case FeatureEnum.ActivityByTypeReportFeature:
                    status = availableFeatures.ActivityByTypeReportFeature;
                    break;
                case FeatureEnum.KPIFeature:
                    status = availableFeatures.KPIFeature;
                    break;
                case FeatureEnum.MAPFeature:
                    status = availableFeatures.MAPFeature;
                    break;
                case FeatureEnum.EmployeeExpenseReportFeature: // PJMargo
                    status = availableFeatures.EmployeeExpenseReport;
                    break;
                case FeatureEnum.EmployeeExpenseReport2Feature:  // heranba
                    status = availableFeatures.EmployeeExpenseReport2;
                    break;
                case FeatureEnum.UnSownReportFeature:  // Reitzel
                    status = availableFeatures.UnSownReport;
                    break;
                case FeatureEnum.DistanceReportFeature:
                    status = availableFeatures.DistanceReport;
                    break;
                case FeatureEnum.SurveyFormReport:
                    status = availableFeatures.SurveyFormReport;
                    break;
                case FeatureEnum.SalesPersonFeature:
                    status = availableFeatures.SalesPersonFeature;
                    break;
                case FeatureEnum.CustomerFeature:
                    status = availableFeatures.CustomerFeature;
                    break;
                case FeatureEnum.ProductFeature:
                    status = availableFeatures.ProductFeature;
                    break;
                case FeatureEnum.CrmUserFeature:
                    status = availableFeatures.CrmUserFeature;
                    break;
                case FeatureEnum.AssignmentFeature:
                    status = availableFeatures.AssignmentFeature;
                    break;
                case FeatureEnum.UploadDataFeature:
                    status = availableFeatures.UploadDataFeature;
                    break;
                case FeatureEnum.OfficeHierarchyFeature:
                    status = availableFeatures.OfficeHierarchyFeature;
                    break;
                case FeatureEnum.BankAccountFeature:
                    status = availableFeatures.BankAccountFeature;
                    break;
                case FeatureEnum.GstRateFeature:
                    status = availableFeatures.GstRateFeature;
                    break;

                case FeatureEnum.WorkflowFeature:
                    status = availableFeatures.WorkFlowFeature;
                    break;
                case FeatureEnum.RedFarmerModule:
                    status = availableFeatures.RedFarmerModule;
                    break;
                case FeatureEnum.DistantActivityReportFeature:
                    status = availableFeatures.DistantActivityReport;
                    break;
                case FeatureEnum.AdvanceRequest:
                    status = availableFeatures.AdvanceRequestModule;
                    break;
                case FeatureEnum.STRFeature:
                    status = availableFeatures.STRFeature;
                    break;
                case FeatureEnum.STRWeighControl:
                    status = availableFeatures.STRWeighFeature;
                    break;
                //case FeatureEnum.STRSiloControl:
                //    status = availableFeatures.STRSiloFeature;
                //    break;

                case FeatureEnum.DWSApproveWeightOption:
                    status = availableFeatures.DWSApproveWeightOption;
                    break;
                case FeatureEnum.DWSApproveAmountOption:
                    status = availableFeatures.DWSApproveAmountOption;
                    break;
                case FeatureEnum.DWSPaymentOption:
                    status = availableFeatures.DWSPaymentOption;
                    break;
                //case FeatureEnum.BonusCalculationOption:
                //    status = availableFeatures.BonusCalculationOption;
                //    break;
                case FeatureEnum.BonusCalculationPaymentOption:
                    status = availableFeatures.BonusCalculationPaymentOption;
                    break;

                case FeatureEnum.DWSPaymentReport:
                    status = availableFeatures.DWSPaymentReportFeature;
                    break;
                //case FeatureEnum.BonusPaymentReport:
                //    status = availableFeatures.BonusPaymentReport;
                //    break;
                case FeatureEnum.TransporterPaymentReport:
                    status = availableFeatures.TransporterPaymentReportFeature;
                    break;

                // June 10 2020
                case FeatureEnum.StockReceive:
                    status = availableFeatures.StockReceiveOption;
                    break;
                case FeatureEnum.StockReceiveConfirm:
                    status = availableFeatures.StockReceiveConfirmOption;
                    break;
                case FeatureEnum.StockRequest:
                    status = availableFeatures.StockRequestOption;
                    break;
                case FeatureEnum.StockRequestFulfill:
                    status = availableFeatures.StockRequestFulfillOption;
                    break;
                case FeatureEnum.ExtraAdminOption:
                    status = availableFeatures.ExtraAdminOption;
                    break;

                case FeatureEnum.StockLedger:
                    status = availableFeatures.StockLedgerOption;
                    break;
                case FeatureEnum.StockBalance:
                    status = availableFeatures.StockBalanceOption;
                    break;
                case FeatureEnum.StockRemove:
                    status = availableFeatures.StockRemoveOption;
                    break;
                case FeatureEnum.StockRemoveConfirm:
                    status = availableFeatures.StockRemoveConfirmOption;
                    break;

                case FeatureEnum.StockAdd:
                    status = availableFeatures.StockAddOption;
                    break;
                case FeatureEnum.StockAddConfirm:
                    status = availableFeatures.StockAddConfirmOption;
                    break;
                //Author:Pankaj Kumar; Purpose: Day Planning; Date: 30/04/2021
                case FeatureEnum.DayPlanningReport:
                    status = availableFeatures.DayPlanReport;
                    break;
                //Author:SA; Purpose : TransporterPayment
                //case FeatureEnum.AddOrApproveVendorPayment:
                //    status = availableFeatures.AddOrApproveTransporterPayment;
                //    break;
                //case FeatureEnum.VendorPaymentOption:
                //    status = availableFeatures.TransporterPaymentOption;
                //    break;
                case FeatureEnum.QuestionnaireFeature:
                    status = availableFeatures.QuestionnaireFeature;
                    break;

                case FeatureEnum.FarmerSummaryReport:
                    status = availableFeatures.FarmerSummaryReport;
                    break;

                case FeatureEnum.ProjectOption:
                    status = availableFeatures.ProjectOption;
                    break;
                case FeatureEnum.FollowUpTaskOption:
                    status = availableFeatures.FollowUpTaskOption;
                    break;

                case FeatureEnum.LeaveFeature:
                    status = availableFeatures.LeaveFeature;
                    break;

                case FeatureEnum.DealerNotMetReport:
                    status = availableFeatures.DealersNotMetReport;
                    break;

                case FeatureEnum.DealersSummaryReport:
                    status = availableFeatures.DealersSummaryReport;
                    break;

                case FeatureEnum.GeoTagReport:
                    status = availableFeatures.GeoTagReport;
                    break;
                case FeatureEnum.AgreementsReport:
                    status = availableFeatures.AgreementsReport;
                    break;
                case FeatureEnum.DuplicateFarmersReport:
                    status = availableFeatures.DuplicateFarmersReport;
                    break;
                case FeatureEnum.FarmersBankAccountReport:
                    status = availableFeatures.FarmersBankAccountReport;
                    break;
                default:
                    status = false;
                    break;
            }

            return status;
        }

        public static DomainEntities.CustomersFilter ParseCustomersSearchCriteria(CustomersFilter searchCriteria)
        {
            DomainEntities.CustomersFilter s = new DomainEntities.CustomersFilter()
            {
                ApplyCodeFilter = false,
                ApplyNameFilter = false,
                ApplyTypeFilter = false,
                ApplyHQCodeFilter = false,
                ApplyStaffCodeFilter = false
            };
            if (searchCriteria == null)
            {
                return s;
            }

            s.ApplyNameFilter = IsValidSearchCriteria(searchCriteria.Name);
            s.Name = s.ApplyNameFilter ? searchCriteria.Name.Trim() : searchCriteria.Name;

            s.ApplyCodeFilter = IsValidSearchCriteria(searchCriteria.Code);
            s.Code = s.ApplyCodeFilter ? searchCriteria.Code.Trim() : searchCriteria.Code;

            if (searchCriteria.Type != null && !searchCriteria.Type.Equals("Select Type"))
            {
                s.ApplyTypeFilter = true;
                s.Type = searchCriteria.Type;
            }

            s.ApplyHQCodeFilter = IsValidSearchCriteria(searchCriteria.HQCode);
            s.HQCode = s.ApplyHQCodeFilter ? searchCriteria.HQCode.Trim() : searchCriteria.HQCode;

            return s;
        }

        public static DomainEntities.TerminateAgreementRequestFilter ParseRedFarmerSearchCriteria(TerminateAgreementRequestFilter searchCriteria)
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.Current.User);
            DomainEntities.TerminateAgreementRequestFilter s = new DomainEntities.TerminateAgreementRequestFilter()
            {
                ApplyZoneFilter = false,
                ApplyAreaFilter = false,
                ApplyTerritoryFilter = false,
                ApplyHQFilter = false,
                ApplyCropFilter = false,

                ApplyClientNameFilter = false,
                ApplyEmployeeNameFilter = false,
                ApplyEmployeeCodeFilter = false,
                ApplyDateFilter = false,

                ApplyAgreementNumberFilter = false,
                ApplyAgreementStatusFilter = false,
                ApplyRedFarmerReqReasonFilter = false,
                ApplyRedFarmerReqStatusFilter = false,

                IsSuperAdmin = securityContext.Item1,
                CurrentUserStaffCode = securityContext.Item2,
                TenantId = Utils.SiteConfigData.TenantId
            };

            if (searchCriteria == null)
            {
                return s;
            }

            s.ApplyZoneFilter = IsValidSearchCriteria(searchCriteria.Zone);
            s.Zone = searchCriteria.Zone;

            s.ApplyAreaFilter = IsValidSearchCriteria(searchCriteria.Area);
            s.Area = searchCriteria.Area;

            s.ApplyTerritoryFilter = IsValidSearchCriteria(searchCriteria.Territory);
            s.Territory = searchCriteria.Territory;

            s.ApplyHQFilter = IsValidSearchCriteria(searchCriteria.HQ);
            s.HQ = searchCriteria.HQ;

            s.ApplyCropFilter = IsValidSearchCriteria(searchCriteria.Crop);
            s.Crop = searchCriteria.Crop;

            s.ApplyClientNameFilter = IsValidSearchCriteria(searchCriteria.ClientName);
            s.ClientName = s.ApplyClientNameFilter ? searchCriteria.ClientName.Trim() : searchCriteria.ClientName;

            s.ApplyEmployeeNameFilter = IsValidSearchCriteria(searchCriteria.EmployeeName);
            s.EmployeeName = s.ApplyEmployeeNameFilter ? searchCriteria.EmployeeName.Trim() : searchCriteria.EmployeeName;

            s.ApplyEmployeeCodeFilter = IsValidSearchCriteria(searchCriteria.EmployeeCode);
            s.EmployeeCode = s.ApplyEmployeeCodeFilter ? searchCriteria.EmployeeCode.Trim() : searchCriteria.EmployeeCode;

            s.ApplyAgreementStatusFilter = IsValidSearchCriteria(searchCriteria.AgreementStatus);
            s.AgreementStatus = searchCriteria.AgreementStatus;

            s.ApplyAgreementNumberFilter = IsValidSearchCriteria(searchCriteria.AgreementNumber);
            s.AgreementNumber = s.ApplyAgreementNumberFilter ? searchCriteria.AgreementNumber.Trim() : searchCriteria.AgreementNumber;

            // parse dates
            var r = ParseSearchCriteriaDates(searchCriteria.DateFrom, searchCriteria.DateTo);
            s.ApplyDateFilter = r.Item1;
            s.DateFrom = r.Item2;
            s.DateTo = r.Item3;

            //if (String.IsNullOrEmpty(searchCriteria.DateFrom))
            //{
            //    searchCriteria.DateFrom = "01-01-0001";
            //}
            //if (String.IsNullOrEmpty(searchCriteria.DateTo))
            //{
            //    searchCriteria.DateTo = "01-01-0001";
            //}

            //var culture = CultureInfo.CreateSpecificCulture("en-GB");

            //DateTime fromDate;
            //DateTime toDate;
            //bool isValidFromDate = DateTime.TryParse(searchCriteria.DateFrom, culture, DateTimeStyles.None, out fromDate);
            //bool isValidToDate = DateTime.TryParse(searchCriteria.DateTo, culture, DateTimeStyles.None, out toDate);

            //s.DateFrom = fromDate;
            //s.DateTo = toDate;

            //if (isValidFromDate && isValidToDate)
            //{
            //    if (s.DateFrom > DateTime.MinValue && s.DateTo == DateTime.MinValue)
            //    {
            //        //s.DateTo = DateTime.MaxValue;
            //        s.DateTo = DateTime.UtcNow.AddDays(1);
            //    }

            //    if ((s.DateFrom > DateTime.MinValue && s.DateTo > DateTime.MinValue && s.DateTo >= s.DateFrom) ||
            //        (s.DateFrom == DateTime.MinValue && s.DateTo > DateTime.MinValue))
            //    {
            //        s.ApplyDateFilter = true;
            //    }
            //}

            //if (s.ApplyDateFilter)
            //{
            //    if (s.DateFrom == DateTime.MinValue)
            //    {
            //        s.DateFrom = new DateTime(2017, 09, 01);
            //    }

            //    if (s.DateFrom == DateTime.MaxValue)
            //    {
            //        s.DateFrom = DateTime.UtcNow.AddDays(1);
            //    }
            //}

            s.ApplyRedFarmerReqStatusFilter = IsValidSearchCriteria(searchCriteria.RedFarmerReqStatus);
            s.RedFarmerReqStatus = searchCriteria.RedFarmerReqStatus;

            s.ApplyRedFarmerReqReasonFilter = IsValidSearchCriteria(searchCriteria.RedFarmerReqReason);
            s.RedFarmerReqReason = searchCriteria.RedFarmerReqReason;

            return s;
        }

        public static DomainEntities.AdvanceRequestFilter GetDefaultAdvanceRequestFilter()
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.Current.User);
            return new DomainEntities.AdvanceRequestFilter()
            {
                ApplyZoneFilter = false,
                ApplyAreaFilter = false,
                ApplyTerritoryFilter = false,
                ApplyHQFilter = false,
                ApplyCropFilter = false,

                ApplyClientNameFilter = false,
                ApplyEmployeeNameFilter = false,
                ApplyEmployeeCodeFilter = false,
                ApplyDateFilter = false,

                ApplyAgreementNumberFilter = false,
                ApplyAgreementStatusFilter = false,
                ApplyAdvanceRequestStatusFilter = false,

                IsSuperAdmin = securityContext.Item1,
                CurrentUserStaffCode = securityContext.Item2,
                TenantId = Utils.SiteConfigData.TenantId
            };
        }

        public static DomainEntities.AdvanceRequestFilter ParseAdvanceSearchCriteria(AdvanceRequestFilter searchCriteria)
        {
            DomainEntities.AdvanceRequestFilter s = GetDefaultAdvanceRequestFilter();

            if (searchCriteria == null)
            {
                return s;
            }

            s.ApplyZoneFilter = IsValidSearchCriteria(searchCriteria.Zone);
            s.Zone = searchCriteria.Zone;

            s.ApplyAreaFilter = IsValidSearchCriteria(searchCriteria.Area);
            s.Area = searchCriteria.Area;

            s.ApplyTerritoryFilter = IsValidSearchCriteria(searchCriteria.Territory);
            s.Territory = searchCriteria.Territory;

            s.ApplyHQFilter = IsValidSearchCriteria(searchCriteria.HQ);
            s.HQ = searchCriteria.HQ;

            s.ApplyCropFilter = IsValidSearchCriteria(searchCriteria.Crop);
            s.Crop = searchCriteria.Crop;

            s.ApplyClientNameFilter = IsValidSearchCriteria(searchCriteria.ClientName);
            s.ClientName = s.ApplyClientNameFilter ? searchCriteria.ClientName.Trim() : searchCriteria.ClientName;

            s.ApplyEmployeeNameFilter = IsValidSearchCriteria(searchCriteria.EmployeeName);
            s.EmployeeName = s.ApplyEmployeeNameFilter ? searchCriteria.EmployeeName.Trim() : searchCriteria.EmployeeName;

            s.ApplyEmployeeCodeFilter = IsValidSearchCriteria(searchCriteria.EmployeeCode);
            s.EmployeeCode = s.ApplyEmployeeCodeFilter ? searchCriteria.EmployeeCode.Trim() : searchCriteria.EmployeeCode;

            s.ApplyAgreementStatusFilter = IsValidSearchCriteria(searchCriteria.AgreementStatus);
            s.AgreementStatus = searchCriteria.AgreementStatus;

            s.ApplyAgreementNumberFilter = IsValidSearchCriteria(searchCriteria.AgreementNumber);
            s.AgreementNumber = s.ApplyAgreementNumberFilter ? searchCriteria.AgreementNumber.Trim() : searchCriteria.AgreementNumber;

            s.ApplyAdvanceRequestStatusFilter = IsValidSearchCriteria(searchCriteria.AdvanceRequestStatus);
            s.AdvanceRequestStatus = searchCriteria.AdvanceRequestStatus;

            // parse dates
            var r = ParseSearchCriteriaDates(searchCriteria.DateFrom, searchCriteria.DateTo);
            s.ApplyDateFilter = r.Item1;
            s.DateFrom = r.Item2;
            s.DateTo = r.Item3;

            //if (String.IsNullOrEmpty(searchCriteria.DateFrom))
            //{
            //    searchCriteria.DateFrom = "01-01-0001";
            //}
            //if (String.IsNullOrEmpty(searchCriteria.DateTo))
            //{
            //    searchCriteria.DateTo = "01-01-0001";
            //}

            //var culture = CultureInfo.CreateSpecificCulture("en-GB");

            //DateTime fromDate;
            //DateTime toDate;
            //bool isValidFromDate = DateTime.TryParse(searchCriteria.DateFrom, culture, DateTimeStyles.None, out fromDate);
            //bool isValidToDate = DateTime.TryParse(searchCriteria.DateTo, culture, DateTimeStyles.None, out toDate);

            //s.DateFrom = fromDate;
            //s.DateTo = toDate;

            //if (isValidFromDate && isValidToDate)
            //{
            //    if (s.DateFrom > DateTime.MinValue && s.DateTo == DateTime.MinValue)
            //    {
            //        //s.DateTo = DateTime.MaxValue;
            //        s.DateTo = DateTime.UtcNow.AddDays(1);
            //    }

            //    if ((s.DateFrom > DateTime.MinValue && s.DateTo > DateTime.MinValue && s.DateTo >= s.DateFrom) ||
            //        (s.DateFrom == DateTime.MinValue && s.DateTo > DateTime.MinValue))
            //    {
            //        s.ApplyDateFilter = true;
            //    }
            //}

            //if (s.ApplyDateFilter)
            //{
            //    if (s.DateFrom == DateTime.MinValue)
            //    {
            //        s.DateFrom = new DateTime(2017, 09, 01);
            //    }

            //    if (s.DateFrom == DateTime.MaxValue)
            //    {
            //        s.DateFrom = DateTime.UtcNow.AddDays(1);
            //        // s.DateTo = DateTime.UtcNow.AddDays(1);
            //    }
            //}

            return s;
        }

        public static DomainEntities.VendorSTRFilter ParseSTRSearchCriteriaApproval(VendorSTRFilter searchCriteria)
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.Current.User);

            DomainEntities.VendorSTRFilter vs = new DomainEntities.VendorSTRFilter()
            {
                ApplySTRNumberFilter = false,
                ApplyDateFilter = false,
                ApplyVendorNameFilter = false,
                ApplyVehicleNumberFilter = false,
                ApplySeasonNameFilter = false,
                ApplySTRPaymentStatusFilter = false
            };

            if (searchCriteria == null)
            {
                return vs;
            }

            vs.ApplySTRNumberFilter = IsValidSearchCriteria(searchCriteria.STRNumber);
            vs.STRNumber = vs.ApplySTRNumberFilter ? searchCriteria.STRNumber.Trim() : searchCriteria.STRNumber;

            vs.ApplyVendorNameFilter = IsValidSearchCriteria(searchCriteria.VendorName);
            vs.VendorName = vs.ApplyVendorNameFilter ? searchCriteria.VendorName.Trim() : searchCriteria.VendorName;

            vs.ApplyVehicleNumberFilter = IsValidSearchCriteria(searchCriteria.VehicleNumber);
            vs.VehicleNumber = vs.ApplyVehicleNumberFilter ? searchCriteria.VehicleNumber.Trim() : searchCriteria.VehicleNumber;

            vs.ApplySeasonNameFilter = IsValidSearchCriteria(searchCriteria.SeasonName);
            vs.SeasonName = vs.ApplySeasonNameFilter ? searchCriteria.SeasonName.Trim() : searchCriteria.SeasonName;

            vs.ApplySTRPaymentStatusFilter = IsValidSearchCriteria(searchCriteria.STRPaymentStatus);
            vs.STRPaymentStatus = vs.ApplySTRPaymentStatusFilter ? searchCriteria.STRPaymentStatus.Trim() : searchCriteria.STRPaymentStatus;

            var r = ParseSearchCriteriaDates(searchCriteria.DateFrom, searchCriteria.DateTo);
            vs.ApplyDateFilter = r.Item1;
            vs.DateFrom = r.Item2;
            vs.DateTo = r.Item3;

            return vs;

        }

        public static DomainEntities.STRFilter ParseSTRSearchCriteria(STRFilter searchCriteria)
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.Current.User);
            DomainEntities.STRFilter s = new DomainEntities.STRFilter()
            {
                ApplyZoneFilter = false,
                ApplyAreaFilter = false,
                ApplyTerritoryFilter = false,
                ApplyHQFilter = false,

                ApplySTRNumberFilter = false,
                ApplyDateFilter = false,
                ApplyDWSNumberFilter = false,
                ApplyAgreementNumberFilter = false,
                ApplyClientNameFilter = false,
                ApplyTypeNameFilter = false,

                IsSuperAdmin = securityContext.Item1,
                CurrentUserStaffCode = securityContext.Item2,
                TenantId = Utils.SiteConfigData.TenantId
            };

            if (searchCriteria == null)
            {
                return s;
            }

            s.ApplySTRNumberFilter = IsValidSearchCriteria(searchCriteria.STRNumber);
            s.STRNumber = s.ApplySTRNumberFilter ? searchCriteria.STRNumber.Trim() : searchCriteria.STRNumber;

            s.ApplyDWSNumberFilter = IsValidSearchCriteria(searchCriteria.DWSNumber);
            s.DWSNumber = searchCriteria.DWSNumber;

            s.ApplyAgreementNumberFilter = IsValidSearchCriteria(searchCriteria.AgreementNumber);
            s.AgreementNumber = s.ApplyAgreementNumberFilter ? searchCriteria.AgreementNumber.Trim() : "";

            s.ApplyTypeNameFilter = IsValidSearchCriteria(searchCriteria.TypeName);
            s.TypeName = s.ApplyTypeNameFilter ? searchCriteria.TypeName.Trim() : "";

            s.ApplyClientNameFilter = IsValidSearchCriteria(searchCriteria.ClientName);
            s.ClientName = s.ApplyClientNameFilter ? searchCriteria.ClientName.Trim() : "";

            var r = ParseSearchCriteriaDates(searchCriteria.DateFrom, searchCriteria.DateTo);
            s.ApplyDateFilter = r.Item1;
            s.DateFrom = r.Item2;
            s.DateTo = r.Item3;

            //// parse dates
            //if (String.IsNullOrEmpty(searchCriteria.DateFrom))
            //{
            //    searchCriteria.DateFrom = "01-01-0001";
            //}
            //if (String.IsNullOrEmpty(searchCriteria.DateTo))
            //{
            //    searchCriteria.DateTo = "01-01-0001";
            //}

            //var culture = CultureInfo.CreateSpecificCulture("en-GB");

            //DateTime fromDate;
            //DateTime toDate;
            //bool isValidFromDate = DateTime.TryParse(searchCriteria.DateFrom, culture, DateTimeStyles.None, out fromDate);
            //bool isValidToDate = DateTime.TryParse(searchCriteria.DateTo, culture, DateTimeStyles.None, out toDate);

            //s.DateFrom = fromDate;
            //s.DateTo = toDate;

            //if (isValidFromDate && isValidToDate)
            //{
            //    if (s.DateFrom > DateTime.MinValue && s.DateTo == DateTime.MinValue)
            //    {
            //        //s.DateTo = DateTime.MaxValue;
            //        s.DateTo = DateTime.UtcNow.AddDays(1);
            //    }

            //    if ((s.DateFrom > DateTime.MinValue && s.DateTo > DateTime.MinValue && s.DateTo >= s.DateFrom) ||
            //        (s.DateFrom == DateTime.MinValue && s.DateTo > DateTime.MinValue))
            //    {
            //        s.ApplyDateFilter = true;
            //    }
            //}

            //if (s.ApplyDateFilter)
            //{
            //    if (s.DateFrom == DateTime.MinValue)
            //    {
            //        s.DateFrom = new DateTime(2020, 05, 01);
            //    }

            //    if (s.DateFrom == DateTime.MaxValue)
            //    {
            //        s.DateFrom = DateTime.UtcNow.AddDays(1);
            //        // s.DateTo = DateTime.UtcNow.AddDays(1);
            //    }
            //}

            return s;
        }

        public static DomainEntities.DWSFilter GetDefaultDWSFilter()
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.Current.User);
            return new DomainEntities.DWSFilter()
            {
                ApplyZoneFilter = false,
                ApplyAreaFilter = false,
                ApplyTerritoryFilter = false,
                ApplyHQFilter = false,

                ApplyClientNameFilter = false,
                ApplyAgreementNumberFilter = false,

                ApplySTRNumberFilter = false,
                ApplyDWSNumberFilter = false,
                ApplyDateFilter = false,
                ApplyDWSStatusFilter = false,

                IsSuperAdmin = securityContext.Item1,
                CurrentUserStaffCode = securityContext.Item2,
                TenantId = Utils.SiteConfigData.TenantId
            };
        }

        public static DomainEntities.StockFilter GetDefaultStockFilter()
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.Current.User);
            return new DomainEntities.StockFilter()
            {
                ApplyZoneFilter = false,
                ApplyAreaFilter = false,
                ApplyTerritoryFilter = false,
                ApplyHQFilter = false,

                ApplyVendorNameFilter = false,
                ApplyGRNNumberFilter = false,
                ApplyInvoiceNumberFilter = false,

                ApplyDateFilter = false,
                ApplyStatusFilter = false,

                IsSuperAdmin = securityContext.Item1,
                CurrentUserStaffCode = securityContext.Item2,
                TenantId = Utils.SiteConfigData.TenantId
            };
        }

        public static DomainEntities.StockRequestFilter GetDefaultStockRequestFilter()
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.Current.User);
            return new DomainEntities.StockRequestFilter()
            {
                ApplyZoneFilter = false,
                ApplyAreaFilter = false,
                ApplyTerritoryFilter = false,
                ApplyHQFilter = false,

                ApplyRequestNumberFilter = false,

                ApplyDateFilter = false,
                ApplyStatusFilter = false,

                IsSuperAdmin = securityContext.Item1,
                CurrentUserStaffCode = securityContext.Item2,
                TenantId = Utils.SiteConfigData.TenantId,
            };
        }

        public static DomainEntities.StockLedgerFilter GetDefaultStockLedgerFilter()
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.Current.User);
            return new DomainEntities.StockLedgerFilter()
            {
                ApplyZoneFilter = false,
                ApplyAreaFilter = false,
                ApplyTerritoryFilter = false,
                ApplyHQFilter = false,

                ApplyReferenceNumberFilter = false,

                ApplyDateFilter = false,
                ApplyItemIdFilter = false,

                IsSuperAdmin = securityContext.Item1,
                CurrentUserStaffCode = securityContext.Item2,
                TenantId = Utils.SiteConfigData.TenantId
            };
        }

        public static DomainEntities.DWSFilter ParseDWSSearchCriteria(DWSFilter searchCriteria)
        {
            DomainEntities.DWSFilter s = GetDefaultDWSFilter();

            if (searchCriteria == null)
            {
                return s;
            }

            s.ApplyZoneFilter = IsValidSearchCriteria(searchCriteria.Zone);
            s.Zone = searchCriteria.Zone;

            s.ApplyAreaFilter = IsValidSearchCriteria(searchCriteria.Area);
            s.Area = searchCriteria.Area;

            s.ApplyTerritoryFilter = IsValidSearchCriteria(searchCriteria.Territory);
            s.Territory = searchCriteria.Territory;

            s.ApplyHQFilter = IsValidSearchCriteria(searchCriteria.HQ);
            s.HQ = searchCriteria.HQ;

            s.ApplyClientNameFilter = IsValidSearchCriteria(searchCriteria.ClientName);
            s.ClientName = s.ApplyClientNameFilter ? searchCriteria.ClientName.Trim() : "";

            s.ApplyAgreementNumberFilter = IsValidSearchCriteria(searchCriteria.AgreementNumber);
            s.AgreementNumber = s.ApplyAgreementNumberFilter ? searchCriteria.AgreementNumber.Trim() : "";

            s.ApplySTRNumberFilter = IsValidSearchCriteria(searchCriteria.STRNumber);
            s.STRNumber = s.ApplySTRNumberFilter ? searchCriteria.STRNumber.Trim() : "";

            s.ApplyDWSNumberFilter = IsValidSearchCriteria(searchCriteria.DWSNumber);
            s.DWSNumber = s.ApplyDWSNumberFilter ? searchCriteria.DWSNumber.Trim() : "";

            var r = ParseSearchCriteriaDates(searchCriteria.DateFrom, searchCriteria.DateTo);
            s.ApplyDateFilter = r.Item1;
            s.DateFrom = r.Item2;
            s.DateTo = r.Item3;

            s.ApplyDWSStatusFilter = true;
            s.DWSStatus = Utils.TruncateString(searchCriteria.DWSStatus, 50);

            return s;
        }

        public static DomainEntities.StockFilter ParseStockSearchCriteria(StockFilter searchCriteria)
        {
            DomainEntities.StockFilter s = GetDefaultStockFilter();

            if (searchCriteria == null)
            {
                return s;
            }

            s.ApplyZoneFilter = IsValidSearchCriteria(searchCriteria.Zone);
            s.Zone = searchCriteria.Zone;

            s.ApplyAreaFilter = IsValidSearchCriteria(searchCriteria.Area);
            s.Area = searchCriteria.Area;

            s.ApplyTerritoryFilter = IsValidSearchCriteria(searchCriteria.Territory);
            s.Territory = searchCriteria.Territory;

            s.ApplyHQFilter = IsValidSearchCriteria(searchCriteria.HQ);
            s.HQ = searchCriteria.HQ;

            s.ApplyVendorNameFilter = IsValidSearchCriteria(searchCriteria.VendorName);
            s.VendorName = s.ApplyVendorNameFilter ? searchCriteria.VendorName.Trim() : "";

            s.ApplyGRNNumberFilter = IsValidSearchCriteria(searchCriteria.GRNNumber);
            s.GRNNumber = s.ApplyGRNNumberFilter ? searchCriteria.GRNNumber.Trim() : "";

            s.ApplyInvoiceNumberFilter = IsValidSearchCriteria(searchCriteria.InvoiceNumber);
            s.InvoiceNumber = s.ApplyInvoiceNumberFilter ? searchCriteria.InvoiceNumber.Trim() : "";

            var r = ParseSearchCriteriaDates(searchCriteria.DateFrom, searchCriteria.DateTo);
            s.ApplyDateFilter = r.Item1;
            s.DateFrom = r.Item2;
            s.DateTo = r.Item3;

            s.ApplyStatusFilter = IsValidSearchCriteria(searchCriteria.Status);
            s.Status = s.ApplyStatusFilter ? Utils.TruncateString(searchCriteria.Status, 50) : "";

            return s;
        }

        public static DomainEntities.StockRequestFilter ParseStockRequestSearchCriteria(StockRequestFilter searchCriteria)
        {
            DomainEntities.StockRequestFilter s = GetDefaultStockRequestFilter();

            if (searchCriteria == null)
            {
                return s;
            }

            s.StockRequestType = Utils.TruncateString(searchCriteria.StockRequestType, 50);
            s.TagRecordStatus = Utils.TruncateString(searchCriteria.TagRecordStatus, 50);

            s.ApplyZoneFilter = IsValidSearchCriteria(searchCriteria.Zone);
            s.Zone = searchCriteria.Zone;

            s.ApplyAreaFilter = IsValidSearchCriteria(searchCriteria.Area);
            s.Area = searchCriteria.Area;

            s.ApplyTerritoryFilter = IsValidSearchCriteria(searchCriteria.Territory);
            s.Territory = searchCriteria.Territory;

            s.ApplyHQFilter = IsValidSearchCriteria(searchCriteria.HQ);
            s.HQ = searchCriteria.HQ;

            s.ApplyRequestNumberFilter = IsValidSearchCriteria(searchCriteria.RequestNumber);
            s.RequestNumber = s.ApplyRequestNumberFilter ? searchCriteria.RequestNumber.Trim() : "";

            var r = ParseSearchCriteriaDates(searchCriteria.DateFrom, searchCriteria.DateTo);
            s.ApplyDateFilter = r.Item1;
            s.DateFrom = r.Item2;
            s.DateTo = r.Item3;

            s.ApplyStatusFilter = IsValidSearchCriteria(searchCriteria.Status);
            s.Status = s.ApplyStatusFilter ? Utils.TruncateString(searchCriteria.Status, 50) : "";

            if (!string.IsNullOrEmpty(searchCriteria.EmployeeCode))
            {
                s.ApplyEmployeeCodeFilter = true;
                s.EmployeeCode = searchCriteria.EmployeeCode.Trim();
            }

            if (!string.IsNullOrEmpty(searchCriteria.EmployeeName))
            {
                s.ApplyEmployeeNameFilter = true;
                s.EmployeeName = searchCriteria.EmployeeName.Trim();
            }

            s.ApplyItemTypeFilter = IsValidSearchCriteria(searchCriteria.ItemType);
            s.ItemType = s.ApplyItemTypeFilter ? searchCriteria.ItemType.Trim() : "";

            s.ApplyItemIdFilter = (searchCriteria.ItemId > 0);
            s.ItemId = searchCriteria.ItemId;

            return s;
        }

        public static DomainEntities.StockLedgerFilter ParseStockLedgerSearchCriteria(StockLedgerFilter searchCriteria)
        {
            DomainEntities.StockLedgerFilter s = GetDefaultStockLedgerFilter();

            if (searchCriteria == null)
            {
                return s;
            }

            s.ApplyZoneFilter = IsValidSearchCriteria(searchCriteria.Zone);
            s.Zone = searchCriteria.Zone;

            s.ApplyAreaFilter = IsValidSearchCriteria(searchCriteria.Area);
            s.Area = searchCriteria.Area;

            s.ApplyTerritoryFilter = IsValidSearchCriteria(searchCriteria.Territory);
            s.Territory = searchCriteria.Territory;

            s.ApplyHQFilter = IsValidSearchCriteria(searchCriteria.HQ);
            s.HQ = searchCriteria.HQ;

            s.ApplyReferenceNumberFilter = IsValidSearchCriteria(searchCriteria.ReferenceNumber);
            s.ReferenceNumber = s.ApplyReferenceNumberFilter ? searchCriteria.ReferenceNumber.Trim() : "";

            var r = ParseSearchCriteriaDates(searchCriteria.DateFrom, searchCriteria.DateTo);
            s.ApplyDateFilter = r.Item1;
            s.DateFrom = r.Item2;
            s.DateTo = r.Item3;

            s.ApplyItemTypeFilter = IsValidSearchCriteria(searchCriteria.ItemType);
            s.ItemType = s.ApplyItemTypeFilter ? searchCriteria.ItemType.Trim() : "";

            s.ApplyItemIdFilter = (searchCriteria.ItemId > 0);
            s.ItemId = searchCriteria.ItemId;

            if (!string.IsNullOrEmpty(searchCriteria.EmployeeCode))
            {
                s.ApplyEmployeeCodeFilter = true;
                s.EmployeeCode = searchCriteria.EmployeeCode.Trim();
            }

            if (!string.IsNullOrEmpty(searchCriteria.EmployeeName))
            {
                s.ApplyEmployeeNameFilter = true;
                s.EmployeeName = searchCriteria.EmployeeName.Trim();
            }

            return s;
        }

        public static DomainEntities.PaymentReferenceFilter ParsePaymentRefSearchCriteria(PaymentReferenceFilter searchCriteria)
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.Current.User);
            DomainEntities.PaymentReferenceFilter s = new DomainEntities.PaymentReferenceFilter()
            {
                ApplyZoneFilter = false,
                ApplyAreaFilter = false,
                ApplyTerritoryFilter = false,
                ApplyHQFilter = false,

                ApplyPaymentReferenceFilter = false,
                ApplyDateFilter = false,

                IsSuperAdmin = securityContext.Item1,
                CurrentUserStaffCode = securityContext.Item2,
                TenantId = Utils.SiteConfigData.TenantId
            };

            if (searchCriteria == null)
            {
                return s;
            }

            s.ApplyPaymentReferenceFilter = IsValidSearchCriteria(searchCriteria.PaymentReference);
            s.PaymentReference = s.ApplyPaymentReferenceFilter ? searchCriteria.PaymentReference.Trim() : "";

            // parse dates
            var r = ParseSearchCriteriaDates(searchCriteria.DateFrom, searchCriteria.DateTo);
            s.ApplyDateFilter = r.Item1;
            s.DateFrom = r.Item2;
            s.DateTo = r.Item3;

            //if (String.IsNullOrEmpty(searchCriteria.DateFrom))
            //{
            //    searchCriteria.DateFrom = "01-01-0001";
            //}
            //if (String.IsNullOrEmpty(searchCriteria.DateTo))
            //{
            //    searchCriteria.DateTo = "01-01-0001";
            //}

            //var culture = CultureInfo.CreateSpecificCulture("en-GB");

            //DateTime fromDate;
            //DateTime toDate;
            //bool isValidFromDate = DateTime.TryParse(searchCriteria.DateFrom, culture, DateTimeStyles.None, out fromDate);
            //bool isValidToDate = DateTime.TryParse(searchCriteria.DateTo, culture, DateTimeStyles.None, out toDate);

            //s.DateFrom = fromDate;
            //s.DateTo = toDate;

            //if (isValidFromDate && isValidToDate)
            //{
            //    if (s.DateFrom > DateTime.MinValue && s.DateTo == DateTime.MinValue)
            //    {
            //        //s.DateTo = DateTime.MaxValue;
            //        s.DateTo = DateTime.UtcNow.AddDays(1);
            //    }

            //    if ((s.DateFrom > DateTime.MinValue && s.DateTo > DateTime.MinValue && s.DateTo >= s.DateFrom) ||
            //        (s.DateFrom == DateTime.MinValue && s.DateTo > DateTime.MinValue))
            //    {
            //        s.ApplyDateFilter = true;
            //    }
            //}

            //if (s.ApplyDateFilter)
            //{
            //    if (s.DateFrom == DateTime.MinValue)
            //    {
            //        s.DateFrom = new DateTime(2020, 05, 01);
            //    }

            //    if (s.DateFrom == DateTime.MaxValue)
            //    {
            //        s.DateFrom = DateTime.UtcNow.AddDays(1);
            //        // s.DateTo = DateTime.UtcNow.AddDays(1);
            //    }
            //}

            return s;
        }

        public static DomainEntities.EntitiesFilter ParseEntitiesSearchCriteria(EntitiesFilter searchCriteria)
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.Current.User);
            DomainEntities.EntitiesFilter s = new DomainEntities.EntitiesFilter()
            {
                ApplyZoneFilter = false,
                ApplyAreaFilter = false,
                ApplyTerritoryFilter = false,
                ApplyHQFilter = false,
                ApplyEntityNumberFilter = false,
                ApplyClientNameFilter = false,
                ApplyClientTypeFilter = false,
                ApplyEmployeeNameFilter = false,
                ApplyEmployeeCodeFilter = false,
                ApplyAgreementNumberFilter = false,
                ApplyAgreementStatusFilter = false,
                ApplyBankDetailStatusFilter = false,
                ApplyUniqueIdFilter = false,
                ApplyCropFilter = false,
                ApplyActiveFilter = false,
                IsSuperAdmin = securityContext.Item1,
                CurrentUserStaffCode = securityContext.Item2,
                TenantId = Utils.SiteConfigData.TenantId
            };

            if (searchCriteria == null)
            {
                return s;
            }

            s.ApplyClientNameFilter = IsValidSearchCriteria(searchCriteria.ClientName);
            s.ClientName = s.ApplyClientNameFilter ? searchCriteria.ClientName.Trim() : searchCriteria.ClientName;

            if (searchCriteria.ClientType != null && !searchCriteria.ClientType.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                s.ApplyClientTypeFilter = true;
                s.ClientType = searchCriteria.ClientType;
            }

            s.ApplyEmployeeNameFilter = IsValidSearchCriteria(searchCriteria.EmployeeName);
            s.EmployeeName = s.ApplyEmployeeNameFilter ? searchCriteria.EmployeeName.Trim() : searchCriteria.EmployeeName;

            s.ApplyEmployeeCodeFilter = IsValidSearchCriteria(searchCriteria.EmployeeCode);
            s.EmployeeCode = s.ApplyEmployeeCodeFilter ? searchCriteria.EmployeeCode.Trim() : searchCriteria.EmployeeCode;

            if (searchCriteria.AgreementStatus != null && !searchCriteria.AgreementStatus.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                s.ApplyAgreementStatusFilter = true;
                s.AgreementStatus = searchCriteria.AgreementStatus;
            }

            if (searchCriteria.BankDetailStatus != null && !searchCriteria.BankDetailStatus.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                s.ApplyBankDetailStatusFilter = true;
                s.BankDetailStatus = searchCriteria.BankDetailStatus;
            }

            s.ApplyAgreementNumberFilter = IsValidSearchCriteria(searchCriteria.AgreementNumber);
            s.AgreementNumber = s.ApplyAgreementNumberFilter ? searchCriteria.AgreementNumber.Trim() : searchCriteria.AgreementNumber;

            s.ApplyUniqueIdFilter = IsValidSearchCriteria(searchCriteria.UniqueId);
            s.UniqueId = s.ApplyUniqueIdFilter ? searchCriteria.UniqueId.Trim() : searchCriteria.UniqueId;

            s.ApplyCropFilter = IsValidSearchCriteria(searchCriteria.Crop);
            s.Crop = searchCriteria.Crop;

            s.ApplyZoneFilter = IsValidSearchCriteria(searchCriteria.Zone);
            s.Zone = searchCriteria.Zone;

            s.ApplyAreaFilter = IsValidSearchCriteria(searchCriteria.Area);
            s.Area = searchCriteria.Area;

            s.ApplyTerritoryFilter = IsValidSearchCriteria(searchCriteria.Territory);
            s.Territory = searchCriteria.Territory;

            s.ApplyHQFilter = IsValidSearchCriteria(searchCriteria.HQ);
            s.HQ = searchCriteria.HQ;

            if (searchCriteria.ProfileStatus == 1) // active
            {
                s.ApplyActiveFilter = true;
                s.IsActive = true;
            }
            else if (searchCriteria.ProfileStatus == 2) // inactive
            {
                s.ApplyActiveFilter = true;
                s.IsActive = false;
            }

            if (searchCriteria.EntityNumber != null) 
            {
                s.ApplyEntityNumberFilter = true;
                s.EntityNumber = searchCriteria.EntityNumber;
            }

            return s;
        }

        public static bool IsValidSearchCriteria(string criteria)
        {
            if (String.IsNullOrEmpty(criteria) || criteria.Length == 0 || criteria.Equals("0") || criteria.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }

        public static DomainEntities.ProductsFilter ParseProductsSearchCriteria(ProductsFilter searchCriteria)
        {
            DomainEntities.ProductsFilter s = new DomainEntities.ProductsFilter()
            {
                ApplyProductCodeFilter = false,
                ApplyNameFilter = false,
                ApplyStatusFilter = false,
                ApplyAreaFilter = false,
                ApplyZoneFilter = false,
                MaxResultCount = 1
            };
            if (searchCriteria == null)
            {
                return s;
            }

            s.ApplyNameFilter = Helper.IsValidSearchCriteria(searchCriteria.Name);
            s.Name = s.ApplyNameFilter ? searchCriteria.Name.Trim() : searchCriteria.Name;

            s.ApplyProductCodeFilter = Helper.IsValidSearchCriteria(searchCriteria.ProductCode);
            s.ProductCode = s.ApplyProductCodeFilter ? searchCriteria.ProductCode.Trim() : searchCriteria.ProductCode;

            if (searchCriteria.Status < 2)
            {
                s.ApplyStatusFilter = true;
                s.Status = searchCriteria.Status == 1;
            }

            s.ApplyAreaFilter = Helper.IsValidSearchCriteria(searchCriteria.Area);
            s.Area = searchCriteria.Area;

            // Sweta
            s.ApplyZoneFilter = Helper.IsValidSearchCriteria(searchCriteria.Zone);
            s.Zone = searchCriteria.Zone;

            if (s.ApplyAreaFilter)
            {
                s.ApplyZoneFilter = false;
            }

            s.MaxResultCount = searchCriteria.MaxItems;
            return s;
        }

        public static DomainEntities.StaffFilter ParseStaffSearchCriteria(StaffFilter searchCriteria)
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.Current.User);
            DomainEntities.StaffFilter s = new DomainEntities.StaffFilter()
            {
                ApplyEmployeeCodeFilter = false,
                ApplyNameFilter = false,
                ApplyPhoneFilter = false,
                ApplyStatusFilter = false,
                ApplyGradeFilter = false,
                ApplyAssociationFilter = false,
                ApplyDepartmentFilter = false,
                ApplyDesignationFilter = false,
                ApplyZoneFilter = false,
                ApplyAreaFilter = false,
                ApplyTerritoryFilter = false,
                IsSuperAdmin = securityContext.Item1,
                CurrentUserStaffCode = securityContext.Item2,
                ApplyHQFilter = false,
                TenantId = Utils.SiteConfigData.TenantId
            };
            if (searchCriteria == null)
            {
                return s;
            }

            s.ApplyEmployeeCodeFilter = IsValidSearchCriteria(searchCriteria.EmployeeCode);
            s.EmployeeCode = s.ApplyEmployeeCodeFilter ? searchCriteria.EmployeeCode.Trim() : searchCriteria.EmployeeCode;

            s.ApplyNameFilter = IsValidSearchCriteria(searchCriteria.Name);
            s.Name = s.ApplyNameFilter ? searchCriteria.Name.ToLower().Trim() : searchCriteria.Name;

            s.ApplyPhoneFilter = IsValidSearchCriteria(searchCriteria.Phone);
            s.Phone = s.ApplyPhoneFilter ? searchCriteria.Phone.Trim() : searchCriteria.Phone;

            s.ApplyGradeFilter = IsValidSearchCriteria(searchCriteria.Grade);
            s.Grade = searchCriteria.Grade;

            s.ApplyDepartmentFilter = IsValidSearchCriteria(searchCriteria.Department);
            s.Department = searchCriteria.Department;

            s.ApplyDesignationFilter = IsValidSearchCriteria(searchCriteria.Designation);
            s.Designation = searchCriteria.Designation;

            if (searchCriteria.Association < 2)
            {
                s.ApplyAssociationFilter = true;
                s.Association = searchCriteria.Association == 1;
            }

            if (searchCriteria.Status < 2)
            {
                s.ApplyStatusFilter = true;
                s.Status = searchCriteria.Status == 1;
            }

            s.ApplyZoneFilter = IsValidSearchCriteria(searchCriteria.Zone);
            s.Zone = searchCriteria.Zone;

            s.ApplyAreaFilter = IsValidSearchCriteria(searchCriteria.Area);
            s.Area = searchCriteria.Area;

            s.ApplyTerritoryFilter = IsValidSearchCriteria(searchCriteria.Territory);
            s.Territory = searchCriteria.Territory;

            s.ApplyHQFilter = IsValidSearchCriteria(searchCriteria.HQ);
            s.HQ = searchCriteria.HQ;

            return s;
        }

        public static DomainEntities.SearchCriteria ParseSearchCriteria(SearchCriteria searchCriteria)
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.Current.User);
            DomainEntities.SearchCriteria s = new DomainEntities.SearchCriteria()
            {
                ApplyZoneFilter = false,
                ApplyAreaFilter = false,
                ApplyTerritoryFilter = false,
                ApplyHQFilter = false,
                ApplyDateFilter = false,
                IsSuperAdmin = securityContext.Item1,
                CurrentUserStaffCode = securityContext.Item2,
                TenantId = Utils.SiteConfigData.TenantId
            };

            if (searchCriteria == null)
            {
                return s;
            }

            s.ApplyZoneFilter = IsValidSearchCriteria(searchCriteria.Zone);
            s.Zone = searchCriteria.Zone;

            s.ApplyAreaFilter = IsValidSearchCriteria(searchCriteria.Area);
            s.Area = searchCriteria.Area;

            s.ApplyTerritoryFilter = IsValidSearchCriteria(searchCriteria.Territory);
            s.Territory = searchCriteria.Territory;

            s.ApplyHQFilter = IsValidSearchCriteria(searchCriteria.HQ);
            s.HQ = searchCriteria.HQ;

            // parse dates
            var r = ParseSearchCriteriaDates(searchCriteria.DateFrom, searchCriteria.DateTo);
            s.ApplyDateFilter = r.Item1;
            s.DateFrom = r.Item2;
            s.DateTo = r.Item3;
            s.CustomerName = searchCriteria.CustomerName;
            s.CustomerCode = searchCriteria.CustomerCode;
            s.EmployeeCode = searchCriteria.EmployeeCode;
            s.EmployeeName = searchCriteria.EmployeeName;




            //if (String.IsNullOrEmpty(searchCriteria.DateFrom))
            //{
            //    searchCriteria.DateFrom = "01-01-0001";
            //}
            //if (String.IsNullOrEmpty(searchCriteria.DateTo))
            //{
            //    searchCriteria.DateTo = "01-01-0001";
            //}

            //var culture = CultureInfo.CreateSpecificCulture("en-GB");
            //DateTime fromDate;
            //DateTime toDate;
            //bool isValidFromDate = DateTime.TryParse(searchCriteria.DateFrom, culture, DateTimeStyles.None, out fromDate);
            //bool isValidToDate = DateTime.TryParse(searchCriteria.DateTo, culture, DateTimeStyles.None, out toDate);

            //s.DateFrom = fromDate;
            //s.DateTo = toDate;

            //if (isValidFromDate && isValidToDate)
            //{
            //    if (s.DateFrom > DateTime.MinValue && s.DateTo == DateTime.MinValue)
            //    {
            //        //s.DateTo = DateTime.MaxValue;
            //        s.DateTo = DateTime.UtcNow.AddDays(1);
            //    }

            //    if ((s.DateFrom > DateTime.MinValue && s.DateTo > DateTime.MinValue && s.DateTo >= s.DateFrom) ||
            //        (s.DateFrom == DateTime.MinValue && s.DateTo > DateTime.MinValue))
            //    {
            //        s.ApplyDateFilter = true;
            //    }
            //}

            //if (s.ApplyDateFilter)
            //{
            //    if (s.DateFrom == DateTime.MinValue)
            //    {
            //        s.DateFrom = new DateTime(2017, 09, 01);
            //    }

            //    if (s.DateFrom == DateTime.MaxValue)
            //    {
            //        s.DateTo = DateTime.UtcNow.AddDays(1);
            //    }
            //}

            return s;
        }

        public static DomainEntities.ConfigCodeTable ParseCodeTableSearchCriteria(ConfigCodeTableModel searchCriteria)
        {
            DomainEntities.ConfigCodeTable s = new DomainEntities.ConfigCodeTable()
            {
                ApplyCodeTypeFilter = false,
                ApplyCodeStatusFilter = false,
            };

            s.ApplyCodeTypeFilter = IsValidSearchCriteria(searchCriteria.CodeType);
            s.CodeType = searchCriteria.CodeType;

            if (searchCriteria.CodeStatus < 2)
            {
                s.ApplyCodeStatusFilter = true;
                s.CodeStatus = searchCriteria.CodeStatus == 1;
            }

            s.ApplyCodeNameFilter = IsValidSearchCriteria(searchCriteria.CodeName);
            s.CodeName = s.ApplyCodeNameFilter ? searchCriteria.CodeName.Trim() : searchCriteria.CodeName;

            s.TenantId = searchCriteria.TenantId;

            return s;
        }

        public static DomainEntities.UnifiedLogFilter GetDefaultUnifiedLogFilter()
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.Current.User);
            return new DomainEntities.UnifiedLogFilter()
            {
                ApplyZoneFilter = false,
                ApplyAreaFilter = false,
                ApplyTerritoryFilter = false,
                ApplyHQFilter = false,
                ApplyDateFilter = false,

                IsSuperAdmin = securityContext.Item1,
                CurrentUserStaffCode = securityContext.Item2,
                TenantId = Utils.SiteConfigData.TenantId
            };
        }

        public static DomainEntities.UnifiedLogFilter ParseUnifiedLogSearchCriteria(UnifiedLogFilter searchCriteria)
        {
            DomainEntities.UnifiedLogFilter s = GetDefaultUnifiedLogFilter();

            if (searchCriteria == null)
            {
                return s;
            }

            s.LogType = searchCriteria.LogType.Trim();
            s.StartItem = searchCriteria.StartItem < 1 ? 1 : searchCriteria.StartItem;

            s.ItemCount = searchCriteria.ItemCount < 1 ? 10 : searchCriteria.ItemCount;

            s.ApplyProcessFilter = IsValidSearchCriteria(searchCriteria.ProcessFilter);
            s.ProcessFilter = s.ApplyProcessFilter ? searchCriteria.ProcessFilter.Trim() : "";

            var r = ParseSearchCriteriaDates(searchCriteria.DateFrom, searchCriteria.DateTo);
            s.ApplyDateFilter = r.Item1;
            s.DateFrom = r.Item2;
            s.DateTo = r.Item3;

            return s;
        }

        private static Tuple<bool, DateTime, DateTime> ParseSearchCriteriaDates(string DateFrom, string DateTo)
        {
            if (String.IsNullOrEmpty(DateFrom))
            {
                DateFrom = "01-01-0001";
            }
            if (String.IsNullOrEmpty(DateTo))
            {
                DateTo = "01-01-0001";
            }

            var culture = CultureInfo.CreateSpecificCulture("en-GB");

            bool applyDateFilter = false;
            DateTime fromDate;
            DateTime toDate;
            bool isValidFromDate = DateTime.TryParse(DateFrom, culture, DateTimeStyles.None, out fromDate);
            bool isValidToDate = DateTime.TryParse(DateTo, culture, DateTimeStyles.None, out toDate);

            if (isValidFromDate && isValidToDate)
            {
                if (fromDate > DateTime.MinValue && toDate == DateTime.MinValue)
                {
                    //s.DateTo = DateTime.MaxValue;
                    toDate = DateTime.UtcNow.AddDays(1);
                }

                if ((fromDate > DateTime.MinValue && toDate > DateTime.MinValue && toDate >= fromDate) ||
                    (fromDate == DateTime.MinValue && toDate > DateTime.MinValue))
                {
                    applyDateFilter = true;
                }
            }

            if (applyDateFilter)
            {
                if (fromDate == DateTime.MinValue)
                {
                    fromDate = new DateTime(2020, 05, 01);
                }

                if (fromDate == DateTime.MaxValue)
                {
                    fromDate = DateTime.UtcNow.AddDays(1);
                }
            }

            return new Tuple<bool, DateTime, DateTime>(applyDateFilter, fromDate, toDate);
        }

        internal static string ActivityImageFilePrefix => Utils.SiteConfigData.ActivityImageFilePrefix;

        internal static string ExpenseImageFilePrefix => Utils.SiteConfigData.ExpenseImageFilePrefix;

        internal static string PaymentImageFilePrefix => Utils.SiteConfigData.PaymentImageFilePrefix;

        internal static string OrderImageFilePrefix => Utils.SiteConfigData.OrderImageFilePrefix;

        internal static string EntityImageFilePrefix => Utils.SiteConfigData.EntityImageFilePrefix;

        internal static bool ShowTerminateAndDeleteLinksOnCrmUsersPage => Utils.SiteConfigData.ShowTerminateAndDeleteLinksOnCrmUsersPage;

        internal static string ConfigKeyForRefreshTime => "ConfigRefreshTime";
        internal static string ConfigKeyForSiteConfigurationData => "SiteConfigurationData";
        internal static string ConfigKeyForGlobalConfigurationData => "GlobalConfigurationData";
        internal static string ConfigKeyForDatabaseServerData => "DatabaseServerData";
        public static string ConfigKeyForDbConnection => "DbConnectionString";
        public static string ConfigKeyForEfConnection => "EfConnectionString";

        internal static string GetCurrentSiteUrl(Uri uri)
        {
            return $"{uri.Scheme}://{uri.Authority}";
        }

        internal static Lock lockObject = new Lock();

        /// <summary>
        /// Check if configuration is more than 15 minutes old, get it from
        /// UrlResolve and refresh it.
        /// </summary>
        public static bool CheckAndRefreshConfiguration(string siteUrl)
        {
            bool refreshConfigNow = false;

            lock (lockObject)
            {
                if (HttpContext.Current.Cache[Helper.ConfigKeyForSiteConfigurationData] == null)
                {
                    refreshConfigNow = true;
                }
                else
                {
                    var configData = HttpContext.Current.Cache[Helper.ConfigKeyForSiteConfigurationData] as SiteConfigData;
                    if (configData == null)
                    {
                        refreshConfigNow = true;
                    }
                    else
                    {
                        DateTime lastRefreshTime = configData.UtcRefreshTime;
                        DateTime currentUtcTime = DateTime.UtcNow;
                        double totalMinutesSinceLastRefresh = currentUtcTime.Subtract(lastRefreshTime).TotalMinutes;
                        refreshConfigNow = (totalMinutesSinceLastRefresh >= 15);
                    }
                }

                if (refreshConfigNow)
                {
                    RefreshConfiguration(siteUrl);
                }
            }

            return refreshConfigNow;
        }

        public static void RefreshConfiguration(string siteUrl)
        {
            // for development scenario - turn the value of siteurl
            // http://localhost:55696 to http://localhost

            //if (string.IsNullOrEmpty(siteUrl) == false && siteUrl.ToLower().Contains("localhost"))
            //{
            //    siteUrl = "http://localhost";
            //}

            // Inovke API to get config
            try
            {
                string configServerUrl = Utils.GetConfigValue("UrlResolveSite", "https://urlresolve.gotomyday.com");
                HttpResponseMessage response = null;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.BaseAddress = new Uri(configServerUrl);
                    response = client.GetAsync($"api/Config/GetUrlConfigAll?siteUrl={siteUrl}").Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    var allConfigData = JsonConvert.DeserializeObject<AllConfigData>(response.Content.ReadAsStringAsync().Result);

                    if (allConfigData != null)
                    {
                        allConfigData.SiteInfo.UtcRefreshTime = DateTime.UtcNow;

                        // Author: Ajith N; Purpose:[Enable Questionnaire button] development only; Date: 11-06-2021
                        //allConfigData.SiteInfo.QuestionnaireOption = true;

                        HttpContext.Current.Cache[ConfigKeyForSiteConfigurationData] = allConfigData.SiteInfo;
                        HttpContext.Current.Cache[ConfigKeyForGlobalConfigurationData] = allConfigData.GlobalConfig;
                        HttpContext.Current.Cache[ConfigKeyForDatabaseServerData] = allConfigData.DbServer;
                    }
                    else
                    {
                        RemoveConfigDataFromCache();
                    }
                }
                else
                {
                    RemoveConfigDataFromCache();
                    //Business.LogError(nameof(CheckAndRefreshConfiguration),
                    //        $"Not able to get Site configuration from {configServerUrl} for {siteUrl}");
                }
            }
            catch (Exception ex)
            {
                //Business.LogError(nameof(RefreshConfiguration), "Error occured while getting Site Configuration");
                Business.LogError(nameof(RefreshConfiguration), ex);

                RemoveConfigDataFromCache();
            }
        }

        public static bool IsValidConfiguration() =>
            Utils.SiteConfigData != null && Utils.GlobalConfiguration != null && Utils.DatabaseServerConfiguration != null;

        private static void RemoveConfigDataFromCache()
        {
            RemoveKeyFromCache(ConfigKeyForSiteConfigurationData);
            RemoveKeyFromCache(ConfigKeyForGlobalConfigurationData);
            RemoveKeyFromCache(ConfigKeyForDatabaseServerData);
            RemoveKeyFromCache(ConfigKeyForDbConnection);
            RemoveKeyFromCache(ConfigKeyForEfConnection);
        }

        private static void RemoveKeyFromCache(string keyName)
        {
            if (string.IsNullOrEmpty(keyName))
            {
                return;
            }

            if (HttpContext.Current.Cache[keyName] != null)
            {
                HttpContext.Current.Cache.Remove(keyName);
            }
        }

        public static void CacheDbConnectionString()
        {
            if (IsValidConfiguration() == false)
            {
                return;
            }

            //Build an SQL connection string
            SqlConnectionStringBuilder sqlString = new SqlConnectionStringBuilder()
            {
                DataSource = Utils.DatabaseServerConfiguration.IP,
                InitialCatalog = Utils.SiteConfigData.DBName,
                MultipleActiveResultSets = true,
                IntegratedSecurity = Utils.DatabaseServerConfiguration.IntegratedAuth,
            };

            if (Utils.DatabaseServerConfiguration.IntegratedAuth == false)
            {
                sqlString.UserID = Utils.DatabaseServerConfiguration.UserName;
                sqlString.Password = Utils.DatabaseServerConfiguration.Password;
            }

            string dbConnectionString = sqlString.ToString();

            //Build an Entity Framework connection string
            EntityConnectionStringBuilder entityString = new EntityConnectionStringBuilder()
            {
                Provider = "System.Data.SqlClient",
                Metadata = "res://*/EpicCrmModel.csdl|res://*/EpicCrmModel.ssdl|res://*/EpicCrmModel.msl",
                ProviderConnectionString = dbConnectionString
            };

            string efConnectionString = entityString.ConnectionString;

            lock (lockObject)
            {
                HttpContext.Current.Cache[ConfigKeyForDbConnection] = dbConnectionString;
                HttpContext.Current.Cache[ConfigKeyForEfConnection] = efConnectionString;
            }
        }

        public static DomainEntities.SearchCriteria GetDefaultSearchCriteria()
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.Current.User);

            return new DomainEntities.SearchCriteria()
            {
                ApplyZoneFilter = false,
                ApplyAreaFilter = false,
                ApplyTerritoryFilter = false,
                ApplyHQFilter = false,
                ApplyDataStatusFilter = false,
                ApplyAmountFilter = false,
                ApplyDateFilter = false,
                ReportType = DomainEntities.Constant.OrderType,
                ApplyActivityTypeFilter = false,
                ApplyClientTypeFilter = false,
                ApplyEmployeeCodeFilter = false,
                ApplyEmployeeNameFilter = false,
                ApplyEmployeeStatusFilter = false,
                ApplyDepartmentFilter = false,
                ApplyDesignationFilter = false,
                ApplyClientNameFilter = false,
                ApplyOrderIdFilter = false,
                ApplyWorkFlowFilter = false,
                ApplyWorkFlowStatusFilter = false,
                ApplyDistanceFilter = false,
                ApplyAgreementNumberFilter = false,
                ApplyAgreementStatusFilter = false,
                ApplyCropFilter = false,
                // Author: Pankaj Kumar; Purpose: Search criteria for Day Plan Type; Date: 30-04-2021
                ApplyTargetStatusFilter = false,
                ApplyDayPlanTypeFilter = false,
                CurrentISTTime = Helper.GetCurrentIstDateTime(),
                IsSuperAdmin = securityContext.Item1,
                CurrentUserStaffCode = securityContext.Item2,
                TenantId = Utils.SiteConfigData.TenantId
            };
        }

        public static DomainEntities.SearchCriteria DashboardParseSearchCriteria(SearchCriteria searchCriteria)
        {
            DomainEntities.SearchCriteria s = GetDefaultSearchCriteria();

            if (searchCriteria == null)
            {
                s.ReportType = "Order";
                return s;
            }

            s.ApplyZoneFilter = IsValidSearchCriteria(searchCriteria.Zone);
            s.Zone = searchCriteria.Zone;

            s.ApplyAreaFilter = IsValidSearchCriteria(searchCriteria.Area);
            s.Area = searchCriteria.Area;

            s.ApplyTerritoryFilter = IsValidSearchCriteria(searchCriteria.Territory);
            s.Territory = searchCriteria.Territory;

            s.ApplyHQFilter = IsValidSearchCriteria(searchCriteria.HQ);
            s.HQ = searchCriteria.HQ;

            s.ApplyDataStatusFilter = IsValidSearchCriteria(searchCriteria.DataStatus);

            if (s.ApplyDataStatusFilter)
            {
                // different rules for Expense and other types
                if (searchCriteria.ReportType.Equals(DomainEntities.Constant.ExpenseType, StringComparison.OrdinalIgnoreCase))
                {
                    s.TMApprovedExpense = s.AMApprovedExpense = s.ZMApprovedExpense = false;
                    switch (searchCriteria.DataStatus)
                    {
                        case Constant.TerritoryManagerApproved:
                            s.TMApprovedExpense = true;
                            break;
                        case Constant.AreaManagerApproved:
                            s.TMApprovedExpense = s.AMApprovedExpense = true;
                            break;
                        case Constant.ZoneManagerApproved:
                            s.TMApprovedExpense = s.AMApprovedExpense = s.ZMApprovedExpense = true;
                            break;
                        case Constant.ExpenseNotApproved:
                            s.TMApprovedExpense = s.AMApprovedExpense = s.ZMApprovedExpense = false;
                            break;
                        default:
                            s.ApplyDataStatusFilter = false;
                            break;
                    }
                }
                else // Order/Return/Payment
                {
                    switch (searchCriteria.DataStatus)
                    {
                        case Constant.Approved:
                            s.DataStatus = true;
                            break;
                        case Constant.NotApproved:
                            s.DataStatus = false;
                            break;
                        default:
                            s.ApplyDataStatusFilter = false;
                            break;
                    }
                }
                //bool status = false;
                //if (bool.TryParse(searchCriteria.DataStatus, out status) == false)
                //{
                //    s.ApplyDataStatusFilter = false;
                //}
                //else
                //{
                //    s.DataStatus = status;
                //}
            }

            s.ReportType = searchCriteria.ReportType;

            s.ApplyWorkFlowFilter = IsValidSearchCriteria(searchCriteria.WorkFlow);
            s.WorkFlow = searchCriteria.WorkFlow;

            s.ApplyWorkFlowStatusFilter = (Enum.IsDefined(typeof(DomainEntities.WorkFlowStatus), searchCriteria.WorkFlowStatus) &&
                                                                        DomainEntities.WorkFlowStatus.All != (DomainEntities.WorkFlowStatus)searchCriteria.WorkFlowStatus);
            s.WorkFlowStatus = (s.ApplyWorkFlowStatusFilter) ? (DomainEntities.WorkFlowStatus)searchCriteria.WorkFlowStatus : DomainEntities.WorkFlowStatus.All;

            if (!string.IsNullOrEmpty(searchCriteria.EntityName))
            {
                s.ApplyEntityNameFilter = true;
                s.EntityName = searchCriteria.EntityName.Trim();
            }

            //ActivityType, ClientType
            s.ApplyActivityTypeFilter = IsValidSearchCriteria(searchCriteria.ActivityType);
            s.ActivityType = searchCriteria.ActivityType;

            s.ApplyClientTypeFilter = IsValidSearchCriteria(searchCriteria.ClientType);
            s.ClientType = searchCriteria.ClientType;

            //Department
            s.ApplyDepartmentFilter = IsValidSearchCriteria(searchCriteria.Department);
            s.Department = searchCriteria.Department;

            //Designation
            s.ApplyDesignationFilter = IsValidSearchCriteria(searchCriteria.Designation);
            s.Designation = searchCriteria.Designation;

            s.ApplyAgreementNumberFilter = !String.IsNullOrWhiteSpace(searchCriteria.AgreementNumber);
            s.AgreementNumber = (s.ApplyAgreementNumberFilter) ? searchCriteria.AgreementNumber.Trim() : "";

            s.ApplySlipNumberFilter = !String.IsNullOrEmpty(searchCriteria.SlipNumber);
            s.SlipNumber = (s.ApplySlipNumberFilter) ? searchCriteria.SlipNumber.Trim() : "";

            s.ApplyRowStatusFilter = IsValidSearchCriteria(searchCriteria.RowStatus);
            s.RowStatus = searchCriteria.RowStatus;

            if (!string.IsNullOrEmpty(searchCriteria.EmployeeCode))
            {
                s.ApplyEmployeeCodeFilter = true;
                s.EmployeeCode = searchCriteria.EmployeeCode.Trim();
            }

            if (!string.IsNullOrEmpty(searchCriteria.EmployeeName))
            {
                s.ApplyEmployeeNameFilter = true;
                s.EmployeeName = searchCriteria.EmployeeName.Trim();
            }

            // parse amounts
            if (String.IsNullOrEmpty(searchCriteria.AmountFrom))
            {
                searchCriteria.AmountFrom = "0";
            }
            if (String.IsNullOrEmpty(searchCriteria.AmountTo))
            {
                searchCriteria.AmountTo = "0";
            }

            if (String.IsNullOrEmpty(searchCriteria.Distance))
            {
                searchCriteria.Distance = "0";
            }

            decimal amountFrom = 0;
            decimal amountTo = 0;
            decimal distanceTo = 0;

            bool isValidFromAmount = decimal.TryParse(searchCriteria.AmountFrom, out amountFrom);
            bool isValidToAmount = decimal.TryParse(searchCriteria.AmountTo, out amountTo);
            bool isValidDistance = decimal.TryParse(searchCriteria.Distance, out distanceTo);

            if (distanceTo < 0)
            {
                distanceTo = 0;
            }

            s.ApplyDistanceFilter = true;
            s.Distance = distanceTo;

            s.AmountFrom = amountFrom;
            s.AmountTo = amountTo;

            if (isValidFromAmount && isValidToAmount)
            {
                // user just wants to search for data > from amount
                // and hence has not entered to amount;
                if (s.AmountFrom > 0 && s.AmountTo == 0)
                {
                    s.AmountTo = amountTo = Decimal.MaxValue;
                }

                if ((s.AmountFrom > 0 && s.AmountTo > 0 && s.AmountTo >= s.AmountFrom) ||
                    (s.AmountFrom == 0 && s.AmountTo > 0))
                {
                    s.ApplyAmountFilter = true;
                }
            }

            // parse dates
            if (searchCriteria.ReportType.Equals("Employee Expense Report"))
            {
                if (String.IsNullOrEmpty(searchCriteria.DateFrom))
                {
                    searchCriteria.DateFrom = "01-01-0001";
                }

                if (String.IsNullOrEmpty(searchCriteria.DateTo))
                {
                    searchCriteria.DateTo = "01-01-0001";
                }

                var culture = CultureInfo.CreateSpecificCulture("en-GB");
                DateTime fromDate;
                DateTime toDate;

                bool isValidFromDate = DateTime.TryParse(searchCriteria.DateFrom, culture, DateTimeStyles.None, out fromDate);
                bool isValidToDate = DateTime.TryParse(searchCriteria.DateTo, culture, DateTimeStyles.None, out toDate);

                s.DateFrom = fromDate;
                s.DateTo = toDate;

                if (isValidFromDate && isValidToDate)
                {
                    if (s.DateFrom > DateTime.MinValue && s.DateTo == DateTime.MinValue)
                    {
                        //s.DateTo = DateTime.MaxValue;
                        s.DateTo = DateTime.UtcNow.AddDays(1);
                    }

                    if ((s.DateFrom > DateTime.MinValue && s.DateTo > DateTime.MinValue && s.DateTo >= s.DateFrom) ||
                        (s.DateFrom == DateTime.MinValue && s.DateTo > DateTime.MinValue))
                    {
                        s.ApplyDateFilter = true;
                    }
                }
            }
            else
            {
                s.DateFrom = searchCriteria.searchDates[0];
                s.DateTo = searchCriteria.searchDates[1];
                if (searchCriteria.dateParseStatus[0] && searchCriteria.dateParseStatus[1])
                {
                    if (s.DateFrom > DateTime.MinValue && s.DateTo == DateTime.MinValue)
                    {
                        //s.DateTo = DateTime.MaxValue;
                        s.DateTo = DateTime.UtcNow.AddDays(1);
                    }

                    if ((s.DateFrom > DateTime.MinValue && s.DateTo > DateTime.MinValue && s.DateTo >= s.DateFrom) ||
                        (s.DateFrom == DateTime.MinValue && s.DateTo > DateTime.MinValue))
                    {
                        s.ApplyDateFilter = true;
                    }
                }
            }

            if (s.ApplyDateFilter)
            {
                if (s.DateFrom == DateTime.MinValue)
                {
                    s.DateFrom = new DateTime(2017, 09, 01);
                }

                if (s.DateFrom == DateTime.MaxValue)
                {
                    s.DateTo = DateTime.UtcNow.AddDays(1);
                }
            }

            // Parse Planned Start / End Dates - Oct 03 2020
            s.PlannedDateFrom = searchCriteria.searchDates[2];
            s.PlannedDateTo = searchCriteria.searchDates[3];
            if (searchCriteria.dateParseStatus[2] && searchCriteria.dateParseStatus[3])
            {
                if (s.PlannedDateFrom > DateTime.MinValue && s.PlannedDateTo == DateTime.MinValue)
                {
                    s.PlannedDateTo = DateTime.UtcNow.AddDays(1);
                }

                if ((s.PlannedDateFrom > DateTime.MinValue && s.PlannedDateTo > DateTime.MinValue && s.PlannedDateTo >= s.PlannedDateFrom) ||
                    (s.PlannedDateFrom == DateTime.MinValue && s.PlannedDateTo > DateTime.MinValue))
                {
                    s.ApplyPlannedDateFilter = true;
                }
            }

            if (s.ApplyPlannedDateFilter)
            {
                if (s.PlannedDateFrom == DateTime.MinValue)
                {
                    s.PlannedDateFrom = new DateTime(2017, 09, 01);
                }

                if (s.PlannedDateFrom == DateTime.MaxValue)
                {
                    s.PlannedDateTo = DateTime.UtcNow.AddDays(1);
                }
            }

            // Harvest Date
            s.HarvestDate = searchCriteria.searchDates[4];
            if (searchCriteria.dateParseStatus[4] && s.HarvestDate > DateTime.MinValue)
            {
                s.ApplyHarvestDateFilter = true;
            }

            if (searchCriteria.EmployeeStatus < 2)
            {
                s.ApplyEmployeeStatusFilter = true;
                s.EmployeeStatus = (searchCriteria.EmployeeStatus == 1);
            }

            if (!string.IsNullOrEmpty(searchCriteria.ClientName))
            {
                s.ApplyClientNameFilter = true;
                s.ClientName = s.ApplyClientNameFilter ? searchCriteria.ClientName.ToLower().Trim() : searchCriteria.ClientName;
            }

            if (searchCriteria.Id > 0)
            {
                s.ApplyOrderIdFilter = true;
                s.Id = searchCriteria.Id;
            }

            s.ApplyAgreementStatusFilter = IsValidSearchCriteria(searchCriteria.AgreementStatus);
            s.AgreementStatus = s.ApplyAgreementStatusFilter ? searchCriteria.AgreementStatus : "";

            s.ApplyCropFilter = IsValidSearchCriteria(searchCriteria.Crop);
            s.Crop = searchCriteria.Crop;

            //PK
            s.ApplyTargetStatusFilter = IsValidSearchCriteria(searchCriteria.TargetStatus);
            s.TargetStatus = s.ApplyTargetStatusFilter ? searchCriteria.TargetStatus : "";

            //PK
            s.ApplyDayPlanTypeFilter = IsValidSearchCriteria(searchCriteria.DayPlanType);
            s.DayPlanType = s.ApplyDayPlanTypeFilter ? searchCriteria.DayPlanType : "";

            if (searchCriteria.QuestionPaperId > 0)
            {
                s.ApplyQuestionnaireFilter = true;
                s.QuestionPaperId = searchCriteria.QuestionPaperId;
            }

            //Rajesh V
            s.ApplyUniqueIdFilter = IsValidSearchCriteria(searchCriteria.UniqueId);
            s.UniqueId = searchCriteria.UniqueId;

            s.ApplySeasonNameFilter = IsValidSearchCriteria(searchCriteria.SeasonName);

            s.SeasonName = searchCriteria.SeasonName;

            if (!string.IsNullOrEmpty(searchCriteria.CustomerName))
            {
                s.ApplyCustomerNameFilter = true;
                s.CustomerName = searchCriteria.CustomerName.Trim();
            }

            if (!string.IsNullOrEmpty(searchCriteria.CustomerCode))
            {
                s.ApplyCustomerCodeFilter = true;
                s.CustomerCode = searchCriteria.CustomerCode.Trim();
            }

            if (searchCriteria.GeoTagStatus < 2)
            {
                s.ApplyGeoTagStatusFilter = true;
                s.GeoTagStatus = (searchCriteria.GeoTagStatus == 1);
            }

            if (searchCriteria.BankDetailStatus != null && !searchCriteria.BankDetailStatus.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                s.ApplyBankDetailStatusFilter = true;
                s.BankDetailStatus = searchCriteria.BankDetailStatus;
            }
            if (searchCriteria.BusinessRole != null && !searchCriteria.BusinessRole.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                s.ApplyBusinessRoleFilter = true;
                s.BusinessRole = searchCriteria.BusinessRole;
            }

            if (searchCriteria.ProfileStatus < 2)
            {
                s.ApplyProfileStatusFilter = true;
                s.ProfileStatus = (searchCriteria.ProfileStatus == 1);
            }
            return s;
        }

        public static void FixDatesInSearchCriteriaForAnyDateRange(DomainEntities.SearchCriteria searchCriteria)
        {
            if (searchCriteria.ApplyDateFilter == false)
            {
                searchCriteria.ApplyDateFilter = true;
                searchCriteria.DateTo = DateTime.UtcNow;
                searchCriteria.DateFrom = DateTime.UtcNow.AddDays(-7);
            }
        }

        public static IEnumerable<OfficeHierarchy> GetOfficeHierarchy()
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.Current.User);
            if (securityContext.Item1 == true)
            {
                // super admin
                return Business.GetAssociations();
            }
            else
            {
                return Business.GetSelectableAssociations(securityContext.Item2);
            }
        }

        // PJMargo version of Employee Expense Report
        //public static IEnumerable<EmployeeExpenseReportDataModel> GetEmployeeExpenseReportModel(DomainEntities.SearchCriteria searchCriteria)
        //{
        //    Helper.FixDatesInSearchCriteriaForAnyDateRange(searchCriteria);

        //    var completeOfficeHierarchy = Helper.GetOfficeHierarchy();

        //    var expenseReportData = Business.GetExpenseReportData(searchCriteria);

        //    var staffCodeList = expenseReportData.Select(x => x.DailyConsolidation.StaffCode).ToList();
        //    IEnumerable<SalesPersonModel> salesPersons = Business.GetSelectedSalesPersonData(staffCodeList);

        //    List<EmployeeExpenseReportDataModel> reportModel =
        //                        (from x in expenseReportData
        //                         let oh = completeOfficeHierarchy.Where(a => a.HQCode == x.ExpenseHQCode).FirstOrDefault()
        //                         select new EmployeeExpenseReportDataModel()
        //                         {
        //                             Name = x.Name,
        //                             ExpenseDate = x.ExpenseDate,
        //                             ModeAndClassOfFare = x.Fuel.Sum(a => a.Amount) + x.ParkingAmount + x.TollTaxAmount
        //                                                                + x.TravelPublic.Sum(a => a.Amount)
        //                                                                + x.VehicleRepairAmount,

        //                             LodgeRent = x.LodgeRent,
        //                             LocalConveyance = x.DALocalAmount + x.DailyAllowanceAmount + x.DAOutstationAmount,

        //                             IncdlCharges = x.InternetAmount + x.MiscellaneousAmount,
        //                             CommunicationExpenses = x.TelephoneAmount,

        //                             StartPosition = x.DailyConsolidation.StartPosition,
        //                             EndPosition = x.DailyConsolidation.EndPosition,
        //                             StaffCode = x.DailyConsolidation.StaffCode,
        //                             StartTime = x.DailyConsolidation.StartTime,
        //                             EndTime = x.DailyConsolidation.EndTime,

        //                             Department = salesPersons.Where(a => a.StaffCode == x.DailyConsolidation.StaffCode)
        //                                                                    .FirstOrDefault()?.Department ?? "",

        //                             Designation = salesPersons.Where(a => a.StaffCode == x.DailyConsolidation.StaffCode)
        //                                                                    .FirstOrDefault()?.Designation ?? "",

        //                             ZoneCode = oh?.ZoneCode ?? "",
        //                             ZoneName = oh?.ZoneName ?? "",
        //                             AreaCode = oh?.AreaCode ?? "",
        //                             AreaName = oh?.AreaName ?? "",
        //                             TerritoryCode = oh?.TerritoryCode ?? "",
        //                             TerritoryName = oh?.TerritoryName ?? "",
        //                             HQCode = oh?.HQCode ?? "",
        //                             HQName = oh?.HQName ?? ""
        //                         }).ToList();

        //    return reportModel;
        //}

        public static IEnumerable<EmployeeExpenseReportDataModel> GetEmployeeExpenseReportModel(DomainEntities.SearchCriteria searchCriteria)
        {
            Helper.FixDatesInSearchCriteriaForAnyDateRange(searchCriteria);

            var completeOfficeHierarchy = Helper.GetOfficeHierarchy();

            ICollection<ExpenseReportData> expenseReportData = Business.GetExpenseReportData(searchCriteria);

            var staffCodeList = expenseReportData.Select(x => x.DailyConsolidation.StaffCode).ToList();
            IEnumerable<SalesPersonModel> salesPersons = Business.GetSelectedSalesPersonData(staffCodeList);

            IExpenseCalc expenseCalc = null;
            string expenseSumupClassName = Utils.SiteConfigData.EmployeeExpenseSumupClassName;
            if ("HeranbaExpense".Equals(expenseSumupClassName, StringComparison.OrdinalIgnoreCase))
            {
                expenseCalc = new HeranbaExpense();
            }
            else if ("PJMargoExpense".Equals(expenseSumupClassName, StringComparison.OrdinalIgnoreCase))
            {
                expenseCalc = new PJMargoExpense();
            }
            else
            {
                expenseCalc = null;
            }

            List<EmployeeExpenseReportDataModel> reportModel =
                                 expenseReportData.Select(x =>
                                 {
                                     var oh = completeOfficeHierarchy.Where(a => a.HQCode == x.ExpenseHQCode).FirstOrDefault();
                                     EmployeeExpenseReportDataModel eerdm = new EmployeeExpenseReportDataModel()
                                     {
                                         Name = x.Name,
                                         ExpenseDate = x.ExpenseDate,

                                         StartPosition = x.DailyConsolidation.StartPosition,
                                         EndPosition = x.DailyConsolidation.EndPosition,
                                         StaffCode = x.DailyConsolidation.StaffCode,
                                         StartTime = x.DailyConsolidation.StartTime,
                                         EndTime = x.DailyConsolidation.EndTime,
                                         TrackingDistanceInMeters = x.DailyConsolidation.TrackingDistanceInMeters,

                                         Department = salesPersons.Where(a => a.StaffCode == x.DailyConsolidation.StaffCode)
                                                                            .FirstOrDefault()?.Department ?? "",

                                         Designation = salesPersons.Where(a => a.StaffCode == x.DailyConsolidation.StaffCode)
                                                                            .FirstOrDefault()?.Designation ?? "",

                                         ZoneCode = oh?.ZoneCode ?? "",
                                         ZoneName = oh?.ZoneName ?? "",
                                         AreaCode = oh?.AreaCode ?? "",
                                         AreaName = oh?.AreaName ?? "",
                                         TerritoryCode = oh?.TerritoryCode ?? "",
                                         TerritoryName = oh?.TerritoryName ?? "",
                                         HQCode = oh?.HQCode ?? "",
                                         HQName = oh?.HQName ?? ""
                                     };

                                     // now fill calculated data
                                     expenseCalc?.FillCalculatedData(x, eerdm);
                                     return eerdm;

                                 }).ToList();

            return reportModel;
        }

        public static void CreateRoles(ApplicationDbContext context = null)
        {
            if (context == null)
            {
                context = ApplicationDbContext.Create();
            }

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            Helper.EnsureRoleExists(roleManager, "Manager");
            Helper.EnsureRoleExists(roleManager, "Admin");
            Helper.EnsureRoleExists(roleManager, "Developer");
        }

        public static void CreateUsers(ApplicationDbContext context = null)
        {
            if (context == null)
            {
                context = ApplicationDbContext.Create();
            }

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            Helper.CreateUser(UserManager,
                CRMUtilities.Utils.SiteConfigData.SuperAdminUserName,
                CRMUtilities.Utils.SiteConfigData.SuperAdminUserEmail,
                CRMUtilities.Utils.SiteConfigData.SuperAdminUserPassword,
                       false,
                       new string[] { "Admin", "Manager" }
                       );
        }

        // it has to return A for 1, AA for 27 etc.
        public static string GetExcelColumnLabel(int cellPosition)
        {
            if (cellPosition < 1 || cellPosition > 702)
            {
                return "";
            }

            cellPosition -= 1;

            int p2 = cellPosition % 26;
            int p1 = (cellPosition - p2) / 26;
            p2++;

            if (p1 < 0 || p1 > 26 || p2 < 1 || p2 > 26)
            {
                return "";
            }

            return $"{CellMnemonics[p1]}{CellMnemonics[p2]}".Trim();
        }

        public static void DeleteFile(string fileNameWithPath)
        {
            if (String.IsNullOrEmpty(fileNameWithPath))
            {
                return;
            }

            bool existStatus = System.IO.File.Exists(fileNameWithPath);
            if (existStatus == false)
            {
                return;
            }

            try
            {
                System.IO.File.Delete(fileNameWithPath);
            }
            catch (Exception ex)
            {
                Business.LogError($"{nameof(DeleteFile)}", ex.ToString(), fileNameWithPath);
            }
        }

        // create file name with path, where data file can be created;
        public static string GetStorageFileNameWithPath(string fileName)
        {
            string folderPath = GetServerPathForFileStorage();
            return Path.Combine(folderPath, fileName);
        }

        public static string GetServerPathForFileStorage()
        {
            if (String.IsNullOrEmpty(Utils.SiteConfigData.PostDataFileLocation) == false &&
                Directory.Exists(Utils.SiteConfigData.PostDataFileLocation))
            {
                return Utils.SiteConfigData.PostDataFileLocation;
            }
            else
            {
                return HttpContext.Current.Server.MapPath($"~/App_Data/");
            }
        }

        internal static void FillResponseCodeTableData(DownloadMiniDataResponse response, string staffCode)
        {
            response.ExpenseTypes = Business.GetCodeTable("ExpenseType");
            response.TransportTypes = Business.GetTransportTypes(staffCode);
            response.CustomerTypes = Business.GetCodeTable("CustomerType");
            response.ActivityTypes = Business.GetCodeTable("ActivityType");
            response.PaymentTypes = Business.GetCodeTable("PaymentType");
            response.LeaveTypes = Business.GetCodeTable("LeaveType");
            response.LeaveDurations = Business.GetCodeTable("LeaveDuration"); //Added by Swetha -Mar 16
            //response.LeaveReasons = Business.GetCodeTable("LeaveReason");
            response.CropTypes = Business.GetCodeTable("CropType");
            response.StateCodes = Business.GetCodeTable("StateCode");
            response.TransactionTypes = Business.GetCodeTable("TransactionType");
            response.ItemTypes = Business.GetCodeTable("ItemType");
            response.FuelTypes = Business.GetCodeTable("FuelType");
            response.Divisions = Business.GetCodeTable("Division");
            response.Segments = Business.GetCodeTable("Segment");

            response.WaterSources = Business.GetCodeTable("WaterSource");
            response.SoilTypes = Business.GetCodeTable("SoilType");
            response.MajorCrops = Business.GetCodeTable("MajorCrop");
            response.LastCrops = Business.GetCodeTable("LastCrop");

            response.SowingTypes = Business.GetCodeTable("SowingType");

            // apk 2.1.0296 onwards this is not used
            response.ActiveCrops = Business.GetActiveCrops(); //Business.GetCodeTable("ActiveCrop");

            response.TerminateAgreementReasons = Business.GetCodeTable("TerminateAgreementReason");
            response.Locales = Business.GetCodeTable("Locale");

            response.XamlEntityAddPages = Business.GetCodeTable("XamlEntityAddPage");
            response.IssueReturnActivityTypes = Business.GetCodeTable("IssueReturnActivityType");

            response.WorkFlowFollowUpPhases = Business.GetCodeTable("WorkFlowFollowUp");

            response.Fertilizers = Business.GetCodeTable("Fertilizers");
            response.Sprays = Business.GetCodeTable("Sprays");
            response.Acres = Business.GetCodeTable("Acres");
            response.CustomerBanks = Business.GetCodeTable("CustomerBank");
            response.NumberPrefixes = Business.GetCodeTable("NumberPrefix");
            response.QuestionnaireType = Business.GetCodeTable("QuestionnaireType");
            response.ProjectCategory = Business.GetCodeTable("ProjectCategory");
            response.ProjectStatus = Business.GetCodeTable("ProjectStatus");
            response.TaskStatus = Business.GetCodeTable("TaskStatus");
            response.Notification = Business.GetCodeTable("Notification");
        }

        internal static string NewSaveFileName(string siteName, string tableName, string fileType)
        {
            string fileName = $"{siteName.Replace(' ', '_')}_{tableName}_{Guid.NewGuid().ToString()}.{fileType}";
            return Helper.GetStorageFileNameWithPath(fileName);
        }

        internal static IssueReturnModel CreateNewIssueReturnModel(IssueReturn x)
        {
            return new IssueReturnModel()
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                EmployeeCode = x.EmployeeCode,
                EmployeeName = x.EmployeeName,
                DayId = x.DayId,
                TransactionDate = x.TransactionDate,
                StaffCode = x.StaffCode,

                EntityAgreementId = x.EntityAgreementId,
                AgreementNumber = x.AgreementNumber,
                WorkflowSeasonId = x.WorkflowSeasonId,
                WorkflowSeasonName = x.WorkflowSeasonName,
                TypeName = x.TypeName,
                EntityId = x.EntityId,
                EntityType = x.EntityType,
                EntityName = x.EntityName,

                ItemMasterId = x.ItemMasterId,
                ItemType = x.ItemType,
                ItemCode = x.ItemCode,
                ItemUnit = x.ItemUnit,

                TransactionType = x.TransactionType,
                Quantity = x.Quantity,

                Phone = x.Phone,
                IsActive = x.IsActive,
                IsActiveInSap = x.IsActiveInSap,
                ReportType = x.ReportType,

                ActivityId = x.ActivityId,
                SlipNumber = x.SlipNumber,
                LandSizeInAcres = x.LandSizeInAcres,
                ItemRate = x.ItemRate,

                AppliedTransactionType = x.AppliedTransactionType,
                AppliedItemMasterId = x.AppliedItemMasterId,
                AppliedItemType = x.AppliedItemType,
                AppliedItemCode = x.AppliedItemCode,
                AppliedItemUnit = x.AppliedItemUnit,
                AppliedQuantity = x.AppliedQuantity,
                AppliedItemRate = x.AppliedItemRate,
                Status = x.Status,
                DateUpdated = x.DateUpdated,
                Comments = x.Comments,
                CyclicCount = x.CyclicCount,
                IsIssueItem = x.IsIssueItem
            };
        }

        internal static AdvanceRequestModel CreateNewAdvanceRequestModel(AdvanceRequest x)
        {
            return new AdvanceRequestModel()
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                HQCode = x.HQCode,
                EmployeeCode = x.EmployeeCode,
                EmployeeName = x.EmployeeName,
                EntityName = x.EntityName,
                AgreementNumber = x.AgreementNumber,
                AgreementStatus = x.AgreementStatus,
                Crop = x.Crop,
                SeasonName = x.WorkFlowSeasonName,
                AmountRequested = x.Amount,
                RequestDate = x.AdvanceRequestDate,
                AmountApproved = x.AmountApproved,
                ApprovalDate = Helper.ConvertUtcTimeToIst(x.ReviewDate),
                AdvanceReqStatus = x.AdvanceRequestStatus,
                ApproveNote = x.ApproveNotes,
                ReviewedBy = x.ReviewedBy,
                ActivityId = x.ActivityId
            };
        }

        internal static EntityWorkFlowDetailModel CreateNewWorkFlowDetailModel(EntityWorkFlowDetail ewfd)
        {
            return new EntityWorkFlowDetailModel()
            {
                Id = ewfd.Id,
                EntityId = ewfd.EntityId,
                EntityName = ewfd.EntityName,
                EntityNumber = ewfd.EntityNumber,
                EmployeeCode = ewfd.EmployeeCode,
                EmployeeName = ewfd.EmployeeName,
                TypeName = ewfd.TypeName,
                SeasonName = ewfd.SeasonName,
                Phase = ewfd.Phase,
                PlannedFromDate = ewfd.PlannedFromDate,
                PlannedEndDate = ewfd.PlannedEndDate,
                CompletedOn = ewfd.CompletedOn,
                Sequence = ewfd.Sequence,
                HQCode = ewfd.HQCode,
                Status = ewfd.Status,
                IsStarted = ewfd.IsStarted,
                WorkFlowDate = ewfd.WorkFlowDate,
                MaterialType = ewfd.MaterialType,
                MaterialQuantity = ewfd.MaterialQuantity,
                GapFillingRequired = ewfd.GapFillingRequired,
                GapFillingSeedQuantity = ewfd.GapFillingSeedQuantity,
                LaborCount = ewfd.LaborCount,
                PercentCompleted = ewfd.PercentCompleted,
                Agreement = ewfd.Agreement,
                UniqueId = ewfd.UniqueId,
                ActivityId = ewfd.ActivityId,
                // April 11 2020
                BatchNumber = ewfd.BatchNumber,
                LandSize = ewfd.LandSize,
                DWSEntry = ewfd.DWSEntry,
                ItemCount = ewfd.ItemCount, // Plant Count or Nipping Count
                ItemsUsedCount = ewfd.ItemsUsedCount,
                YieldExpected = ewfd.YieldExpected,
                BagsIssued = ewfd.BagsIssued,
                HarvestDate = ewfd.HarvestDate,
                IsFollowUpRow = ewfd.IsFollowUpRow,
                IsActiveAsNumber = (ewfd.IsActive == true ? 1 : 0),

                PlannedFromDateAsText = ewfd.PlannedFromDate.ToString("dd-MM-yyyy"),
                PlannedEndDateAsText = ewfd.PlannedEndDate.ToString("dd-MM-yyyy"),
                Notes = ewfd.Notes,
                AgreementStatus = ewfd.AgreementStatus,
            };
        }

        public static DomainEntities.BankAccountFilter GetDefaultBankAccountFilter()
        {
            return new DomainEntities.BankAccountFilter()
            {
                ApplyBankNameFilter = false,
                ApplyAreaCodeFilter = false
            };
        }

        public static DateTime ConvertStringToDateTime(string datetimeAsText)
        {
            var culture = CultureInfo.CreateSpecificCulture("en-GB");
            DateTime fDate = DateTime.MinValue;
            bool isValidDate = DateTime.TryParse(datetimeAsText, culture, DateTimeStyles.None, out fDate);
            return fDate;
        }

        public static StockInputTag GetSingleStockInputTag(long stockInputTagId)
        {
            DomainEntities.StockFilter s = Helper.GetDefaultStockFilter();
            s.ApplyStockInputTagIdFilter = true;
            s.StockInputTagId = stockInputTagId;

            IEnumerable<StockInputTag> items = Business.GetStockInputTags(s);
            return items?.FirstOrDefault();
        }

        public static StockRequestTag GetSingleStockRequestTag(long stockRequestTagId)
        {
            DomainEntities.StockRequestFilter s = Helper.GetDefaultStockRequestFilter();
            s.ApplyStockRequestTagIdFilter = true;
            s.StockRequestTagId = stockRequestTagId;

            IEnumerable<StockRequestTag> items = Business.GetStockRequestTags(s);
            return items?.FirstOrDefault();
        }

        public static StockInput GetSingleStockInputItem(long stockInputTagId, long stockInputId)
        {
            ICollection<StockInput> items = Business.GetStockInput(stockInputTagId);
            return items.FirstOrDefault(x => x.Id == stockInputId);
        }

        public static StockRequest GetSingleStockRequestItem(long stockRequestTagId, long stockRequestId)
        {
            ICollection<StockRequest> items = Business.GetStockRequest(stockRequestTagId);
            return items.FirstOrDefault(x => x.Id == stockRequestId);
        }

        private static char[] CellMnemonics = new char[] { ' ', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        /// <summary>
        /// Author:Ajith, BonuscalculationsearchFilter ,23/07/2021
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public static DomainEntities.BonusCalculationFilter BonusCalculationSearchCriteriaApproval(BonusCalculationFilter searchCriteria)
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.Current.User);

            DomainEntities.BonusCalculationFilter vs = new DomainEntities.BonusCalculationFilter()
            {
                ApplyZoneFilter = false,
                ApplyAreaFilter = false,
                ApplyTerritoryFilter = false,
                ApplyHQFilter = false,
                ApplyClientNameFilter = false,
                ApplyDateFilter = false,
                ApplyAgreementNumberFilter = false,
                ApplyBonusStatusFilter = false,
                ApplySeasonNameFilter = false,

            };

            if (searchCriteria == null)
            {
                return vs;
            }

            vs.ApplyZoneFilter = IsValidSearchCriteria(searchCriteria.Zone);
            vs.Zone = searchCriteria.Zone;

            vs.ApplyAreaFilter = IsValidSearchCriteria(searchCriteria.Area);
            vs.Area = searchCriteria.Area;

            vs.ApplyTerritoryFilter = IsValidSearchCriteria(searchCriteria.Territory);
            vs.Territory = searchCriteria.Territory;

            vs.ApplyHQFilter = IsValidSearchCriteria(searchCriteria.HQ);
            vs.HQ = searchCriteria.HQ;

            vs.ApplyClientNameFilter = IsValidSearchCriteria(searchCriteria.ClientName);
            vs.ClientName = vs.ApplyClientNameFilter ? searchCriteria.ClientName.Trim() : searchCriteria.ClientName;

            vs.ApplyAgreementNumberFilter = IsValidSearchCriteria(searchCriteria.AgreementNumber);
            vs.AgreementNumber = vs.ApplyAgreementNumberFilter ? searchCriteria.AgreementNumber.Trim() : searchCriteria.AgreementNumber;

            vs.ApplyBonusStatusFilter = IsValidSearchCriteria(searchCriteria.BonusStatus);
            vs.BonusStatus = vs.ApplyBonusStatusFilter ? searchCriteria.BonusStatus.Trim() : searchCriteria.BonusStatus;

            vs.ApplySeasonNameFilter = IsValidSearchCriteria(searchCriteria.SeasonName);
            vs.SeasonName = vs.ApplySeasonNameFilter ? searchCriteria.SeasonName.Trim() : searchCriteria.SeasonName;

            vs.ApplySeasonNameFilter = IsValidSearchCriteria(searchCriteria.SeasonName);
            vs.SeasonName = vs.ApplySeasonNameFilter ? searchCriteria.SeasonName.Trim() : searchCriteria.SeasonName;

            var r = ParseSearchCriteriaDates(searchCriteria.DateFrom, searchCriteria.DateTo);
            vs.ApplyDateFilter = r.Item1;
            vs.DateFrom = r.Item2;
            vs.DateTo = r.Item3;

            vs.IsSuperAdmin = securityContext.Item1;
            vs.CurrentUserStaffCode = securityContext.Item2;
            vs.TenantId = Utils.SiteConfigData.TenantId;

            return vs;

        }

        public static DomainEntities.ProjectsFilter ParseProjectSearchCriteria(ProjectsFilter searchCriteria)
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.Current.User);

            DomainEntities.ProjectsFilter s = new DomainEntities.ProjectsFilter()
            {
                ApplyNameFilter = false,
                ApplyCategoryFilter = false,
                ApplyStatusFilter = false,
                ApplyDateFilter = false,
                ApplyActiveFilter = false
            };

            if (searchCriteria == null)
            {
                return s;
            }

            s.ApplyNameFilter = IsValidSearchCriteria(searchCriteria.Name);
            s.Name = s.ApplyNameFilter ? searchCriteria.Name.Trim() : searchCriteria.Name;

            s.ApplyCategoryFilter = IsValidSearchCriteria(searchCriteria.Category);
            s.Category = s.ApplyCategoryFilter ? searchCriteria.Category.Trim() : searchCriteria.Category;

            s.ApplyStatusFilter = IsValidSearchCriteria(searchCriteria.Status);
            s.Status = s.ApplyStatusFilter ? searchCriteria.Status.Trim() : searchCriteria.Status;

            var r = ParseSearchCriteriaDates(searchCriteria.DateFrom, searchCriteria.DateTo);
            s.ApplyDateFilter = r.Item1;
            s.DateFrom = r.Item2;
            s.DateTo = r.Item3;

            if (searchCriteria.IsActive == 1) // active
            {
                s.ApplyActiveFilter = true;
                s.IsActive = true;
            }
            else if (searchCriteria.IsActive == 2) // inactive
            {
                s.ApplyActiveFilter = true;
                s.IsActive = false;
            }

            return s;
        }

        public static DomainEntities.FollowUpTaskFilter GetDefaultFollowUpTaskFilter()
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.Current.User);
            return new DomainEntities.FollowUpTaskFilter()
            {
                ApplyZoneFilter = false,
                ApplyAreaFilter = false,
                ApplyTerritoryFilter = false,
                ApplyHQFilter = false,
                ApplyProjectFilter = false,
                ApplyTaskFilter = false,

                ApplyClientTypeFilter = false,
                ApplyClientNameFilter = false,
                ApplyActivityTypeFilter = false,

                ApplyDateFilter = false,

                ApplyCreatedByFilter = false,
                ApplyUpdatedByFilter = false,
                ApplyTaskStatusFilter = false,
                ApplyActiveFilter = false,

                IsSuperAdmin = securityContext.Item1,
                CurrentUserStaffCode = securityContext.Item2,
                TenantId = Utils.SiteConfigData.TenantId
            };
        }

        public static DomainEntities.FollowUpTaskFilter ParseFollowUpTaskSearchCriteria(FollowUpTaskFilter searchCriteria)
        {
            DomainEntities.FollowUpTaskFilter s = GetDefaultFollowUpTaskFilter();

            if (searchCriteria == null)
            {
                return s;
            }

            s.ApplyZoneFilter = IsValidSearchCriteria(searchCriteria.Zone);
            s.Zone = searchCriteria.Zone;

            s.ApplyAreaFilter = IsValidSearchCriteria(searchCriteria.Area);
            s.Area = searchCriteria.Area;

            s.ApplyTerritoryFilter = IsValidSearchCriteria(searchCriteria.Territory);
            s.Territory = searchCriteria.Territory;

            s.ApplyHQFilter = IsValidSearchCriteria(searchCriteria.HQ);
            s.HQ = searchCriteria.HQ;

            s.ApplyProjectFilter = IsValidSearchCriteria(searchCriteria.ProjectName);
            s.ProjectName = searchCriteria.ProjectName;

            s.ApplyTaskFilter = IsValidSearchCriteria(searchCriteria.TaskDescription);
            s.TaskDescription = searchCriteria.TaskDescription;

            s.ApplyClientTypeFilter = IsValidSearchCriteria(searchCriteria.ClientType);
            s.ClientType = searchCriteria.ClientType;

            s.ApplyClientNameFilter = IsValidSearchCriteria(searchCriteria.ClientName);
            s.ClientName = s.ApplyClientNameFilter ? searchCriteria.ClientName.Trim() : searchCriteria.ClientName;

            s.ApplyActivityTypeFilter = IsValidSearchCriteria(searchCriteria.ActivityType);
            s.ActivityType = searchCriteria.ActivityType;

            s.ApplyCreatedByFilter = IsValidSearchCriteria(searchCriteria.CreatedBy);
            s.CreatedBy = s.ApplyCreatedByFilter ? searchCriteria.CreatedBy.Trim() : searchCriteria.CreatedBy;

            s.ApplyUpdatedByFilter = IsValidSearchCriteria(searchCriteria.UpdatedBy);
            s.UpdatedBy = s.ApplyUpdatedByFilter ? searchCriteria.UpdatedBy.Trim() : searchCriteria.UpdatedBy;

            s.ApplyTaskStatusFilter = IsValidSearchCriteria(searchCriteria.TaskStatus);
            s.TaskStatus = searchCriteria.TaskStatus;

            // parse dates
            var r = ParseSearchCriteriaDates(searchCriteria.DateFrom, searchCriteria.DateTo);
            s.ApplyDateFilter = r.Item1;
            s.DateFrom = r.Item2;
            s.DateTo = r.Item3.AddDays(1);

            if (searchCriteria.IsActive == 1) // active
            {
                s.ApplyActiveFilter = true;
                s.IsActive = true;
            }
            else if (searchCriteria.IsActive == 2) // inactive
            {
                s.ApplyActiveFilter = true;
                s.IsActive = false;
            }

            return s;
        }
        public static DomainEntities.LeaveFilter ParseLeaveSearchCriteria(EpicCrmWebApi.LeaveFilter searchCriteria)
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.Current.User);
            DomainEntities.LeaveFilter s = new DomainEntities.LeaveFilter()
            {
                ApplyZoneFilter = false,
                ApplyAreaFilter = false,
                ApplyTerritoryFilter = false,
                ApplyHQFilter = false,

                ApplyEmployeeNameFilter = false,
                ApplyEmployeeCodeFilter = false,
                ApplyDateFilter = false,

                ApplyLeaveTypeFilter = false,
                ApplyLeaveDurationFilter = false,
                ApplyLeaveStatusFilter = false,

                IsSuperAdmin = securityContext.Item1,
                CurrentUserStaffCode = securityContext.Item2,
                TenantId = Utils.SiteConfigData.TenantId
            };

            if (searchCriteria == null)
            {
                return s;
            }

            s.ApplyZoneFilter = IsValidSearchCriteria(searchCriteria.Zone);
            s.Zone = searchCriteria.Zone;

            s.ApplyAreaFilter = IsValidSearchCriteria(searchCriteria.Area);
            s.Area = searchCriteria.Area;

            s.ApplyTerritoryFilter = IsValidSearchCriteria(searchCriteria.Territory);
            s.Territory = searchCriteria.Territory;

            s.ApplyHQFilter = IsValidSearchCriteria(searchCriteria.HQ);
            s.HQ = searchCriteria.HQ;

            s.ApplyEmployeeNameFilter = IsValidSearchCriteria(searchCriteria.EmployeeName);
            s.EmployeeName = s.ApplyEmployeeNameFilter ? searchCriteria.EmployeeName.Trim() : searchCriteria.EmployeeName;

            s.ApplyEmployeeCodeFilter = IsValidSearchCriteria(searchCriteria.EmployeeCode);
            s.EmployeeCode = s.ApplyEmployeeCodeFilter ? searchCriteria.EmployeeCode.Trim() : searchCriteria.EmployeeCode;

            s.ApplyLeaveTypeFilter = IsValidSearchCriteria(searchCriteria.LeaveType);
            s.LeaveType = searchCriteria.LeaveType;

            if (searchCriteria.LeaveDuration > s.LeaveDuration)
            {
                s.ApplyLeaveDurationFilter = true;
                s.LeaveDuration = searchCriteria.LeaveDuration;

            }
            s.ApplyLeaveStatusFilter = IsValidSearchCriteria(searchCriteria.LeaveStatus);
            s.LeaveStatus = searchCriteria.LeaveStatus;

            // parse dates
            var r = ParseSearchCriteriaDates(searchCriteria.DateFrom, searchCriteria.DateTo);
            s.ApplyDateFilter = r.Item1;
            s.DateFrom = r.Item2;
            s.DateTo = r.Item3.AddDays(1);
            return s;
        }

    }
}