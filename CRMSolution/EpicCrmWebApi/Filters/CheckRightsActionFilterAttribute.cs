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
    [AttributeUsageAttribute(AttributeTargets.Method,
    Inherited = true, AllowMultiple = false)]
    public class CheckRightsActionFilterAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        private FeatureEnum Feature { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = HttpContext.Current.Request;
            if (request.IsAuthenticated == false)
            {
                base.OnActionExecuting(filterContext);
            }

            string reportType = "";
            SearchCriteria sc = null;
            if (filterContext.ActionParameters.ContainsKey("searchCriteria"))
            {
                sc = filterContext.ActionParameters["searchCriteria"] as SearchCriteria;
            }

            if (sc == null)
            {
                // see if there is report Type
                if (filterContext.ActionParameters.ContainsKey("reportType"))
                {
                    reportType = filterContext.ActionParameters["reportType"] as String;
                }
            }
            else
            {
                reportType = sc.ReportType;
            }

            if (String.IsNullOrEmpty(reportType))
            {
                HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                filterContext.Result = new RedirectResult("~/Dashboard");
                return;
            }

            switch (reportType)
            {
                case Constant.ActivityType:
                    Feature = FeatureEnum.ActivityFeature;
                    break;
                case Constant.OrderType:
                    Feature = FeatureEnum.OrderFeature;
                    break;
                case Constant.PaymentType:
                    Feature = FeatureEnum.PaymentFeature;
                    break;
                case Constant.ReturnType:
                    Feature = FeatureEnum.OrderReturnFeature;
                    break;
                case Constant.ExpenseType:
                    Feature = FeatureEnum.ExpenseFeature;
                    break;
                case Constant.IssueReturnType:
                    Feature = FeatureEnum.IssueReturnFeature;
                    break;

                case Constant.EntityType:
                    Feature = FeatureEnum.EntityFeature;
                    break;
                //changed by sumegha -- For giving superadmin rights to reports.
                case Constant.ExpenseReport:
                    Feature = FeatureEnum.ExpenseReportFeature;
                    break;
                case Constant.EntityWorkFlowReport: //This is the field activity report
                    Feature = FeatureEnum.WorkFlowReportFeature;
                    break;
                case Constant.EntityProgressReport:
                    Feature = FeatureEnum.EntityProgressReportFeature;
                    break;
                case Constant.AttendanceReport:
                    Feature = FeatureEnum.AttendanceReportFeature;
                    break;
                case Constant.AttendanceSummaryReport:
                    Feature = FeatureEnum.AttendanceSummaryReportFeature;
                    break;
                case Constant.AttendanceRegister:
                    Feature = FeatureEnum.AttendanceRegisterReportFeature;
                    break;
                case Constant.AbsenteeReport:
                    Feature = FeatureEnum.AbsenteeReportFeature;
                    break;
                case Constant.AppSignUpReport:
                    Feature = FeatureEnum.AppSignUpReportFeature;
                    break;
                case Constant.AppSignInReport:
                    Feature = FeatureEnum.AppSignInReportFeature;
                    break;
                case Constant.ActivityReport:
                    Feature = FeatureEnum.ActivityReportFeature;
                    break;
                case Constant.ActivityByTypeReport:
                    Feature = FeatureEnum.ActivityByTypeReportFeature;
                    break;
                case Constant.EmployeeExpenseReport:
                    Feature = FeatureEnum.EmployeeExpenseReportFeature;
                    break;
                case Constant.DistanceReport:
                    Feature = FeatureEnum.DistanceReportFeature;
                    break;
                case Constant.DistantActivityReport:
                    Feature = FeatureEnum.DistantActivityReportFeature;
                    break;
                case Constant.AdvanceRequest:
                    Feature = FeatureEnum.AdvanceRequest;
                    break;
                case Constant.RedFarmerModule:
                    Feature = FeatureEnum.RedFarmerModule;
                    break;
                case Constant.EmployeeExpenseReport2:
                    Feature = FeatureEnum.EmployeeExpenseReport2Feature;
                    break;
                case Constant.UnSownReport:
                    Feature = FeatureEnum.UnSownReportFeature;
                    break;

                case Constant.STRFeature:
                    Feature = FeatureEnum.STRFeature;
                    break;

                case Constant.STRWeighControl:
                    Feature = FeatureEnum.STRWeighControl;
                    break;

                //case Constant.STRSiloControl:
                //    Feature = FeatureEnum.STRSiloControl;
                //    break;

                case Constant.DWSPaymentReport:
                    Feature = FeatureEnum.DWSPaymentReport;
                    break;

                case Constant.TransporterPaymentReport:
                    Feature = FeatureEnum.TransporterPaymentReport;
                    break;

                // June 10 2020
                case Constant.StockReceive:
                    Feature = FeatureEnum.StockReceive;
                    break;
                case Constant.StockReceiveConfirm:
                    Feature = FeatureEnum.StockReceiveConfirm;
                    break;
                case Constant.StockRequest:
                    Feature = FeatureEnum.StockRequest;
                    break;
                case Constant.StockRequestFulfill:
                    Feature = FeatureEnum.StockRequestFulfill;
                    break;
                case Constant.ExtraAdminOption:
                    Feature = FeatureEnum.ExtraAdminOption;
                    break;
                case Constant.BonusCalculationPaymentOption:
                    Feature = FeatureEnum.BonusCalculationPaymentOption;
                    break;
                case Constant.SurveyFormReport:
                    Feature = FeatureEnum.SurveyFormReport;
                    break;
                // Added By:PankajKumar; Purpose: Added Day Planning report; Date: 30/04/2021
                case Constant.DayPlannningReport:
                    Feature = FeatureEnum.DayPlanningReport;
                    break;

                //end
                // Added By:Ajith; Purpose: Dealer Questionnaire
                case Constant.QuestionnaireType:
                    Feature = FeatureEnum.QuestionnaireFeature;
                    break;

                case Constant.FarmerSummaryReport:
                    Feature = FeatureEnum.FarmerSummaryReport;
                    break;

                case Constant.ProjectType:
                    Feature = FeatureEnum.ProjectOption;
                    break;
                case Constant.FollowUpTaskType:
                    Feature = FeatureEnum.FollowUpTaskOption;
                    break;
                case Constant.DealersNotMetReport:
                    Feature= FeatureEnum.DealerNotMetReport;
                    break;  
                
                case Constant.DealersSummaryReport:
                    Feature= FeatureEnum.DealersSummaryReport;
                    break;
                case Constant.GeoTagReport:
                    Feature = FeatureEnum.GeoTagReport;
                    break;
                case Constant.Agreements:
                    Feature = FeatureEnum.AgreementsReport;
                    break;
                case Constant.DuplicateFarmersReport:
                    Feature = FeatureEnum.DuplicateFarmersReport;
                    break;
                case Constant.FarmersBankAccountReport:
                    Feature = FeatureEnum.FarmersBankAccountReport;
                    break;

                default:
                    Feature = FeatureEnum.None;
                    break;
            }

            FeatureData availableFeatures = Helper.GetAvailableFeatures(HttpContext.Current.User.Identity.Name, Helper.IsSuperAdmin(HttpContext.Current.User));
            if (availableFeatures == null)
            {
                HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                filterContext.Result = new RedirectResult("~/Dashboard");
                return;
            }

            bool status = Helper.IsFeatureEnabled(Feature, availableFeatures);
            if (status == false)
            {
                HttpContext.Current.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                filterContext.Result = new RedirectResult("~/Dashboard");
                return;
            }
         
            base.OnActionExecuting(filterContext);
        }
    }
}