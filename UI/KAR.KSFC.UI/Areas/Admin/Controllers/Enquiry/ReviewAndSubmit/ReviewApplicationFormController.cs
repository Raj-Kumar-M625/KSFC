using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using KAR.KSFC.Components.Common.Dto.Enums;
using Microsoft.AspNetCore.Authorization;
using KAR.KSFC.Components.Common.Logging.Client;
using System;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.Enquiry.ReviewAndSubmit
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    [SwitchModuleFilter(SwitchedModule = SwitchedModuleEnum.Admin)]

    public class ReviewApplicationFormController : Controller
    {
        private const string resultViewPath = "~/Areas/Customer/Views/Enquiry/ReviewAndSubmit/ReviewApplicationForm/";
        private const string viewPath = "../../Areas/Customer/Views/Enquiry/ReviewAndSubmit/ReviewApplicationForm/";
        private readonly SessionManager _sessionManager;
        private readonly ILogger _logger;

        public ReviewApplicationFormController(SessionManager sessionManager, ILogger logger)
        {
            _sessionManager = sessionManager;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult getAddressData()
        {
            try
            {
                _logger.Information("Started - getAddressData ");

                int listCount = 0;
                var res = _sessionManager.GetAddressList();
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getAddressData ");

                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "AddressdataView", res), listCount });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! getAddressData page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getRegistrationData()
        {
            try
            {
                _logger.Information("Started - getRegistrationData ");

                var res = _sessionManager.GetRegistrationDetList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getRegistrationData ");

                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getRegistrationData", res), listCount });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getPromoterData()
        {           

            try
            {
                _logger.Information("Started - getPromoterData ");

                var res = _sessionManager.GetPromoterDetailsList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getPromoterData ");

                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getPromoterData", res), listCount });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getPromoterAssetData()
        {
            try
            {
                _logger.Information("Started - getPromoterAssetData ");

                var res = _sessionManager.GetPromoterAssetList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getPromoterAssetData ");

                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getPromoterAssetData", res), listCount });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getPromoterLiabilityData()
        {
            try
            {
                _logger.Information("Started - getPromoterLiabilityData ");

                var res = _sessionManager.GetPromoterLiabilityList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getPromoterLiabilityData ");

                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getPromoterLiabilityData", res), listCount });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        //need to check this
        public IActionResult getPromoterNetWorthData()
        {
            try
            {
                _logger.Information("Started - getPromoterNetWorthData ");

                var res = _sessionManager.GetPromoterNetWorthList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }

                _logger.Information("Completed - getPromoterNetWorthData ");

                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getPromoterNetWorthData", res), listCount });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getGuarantorData()
        {
            try
            {
                _logger.Information("Started - getGuarantorData ");

                int listCount = 0;
                var res = _sessionManager.GetGuarantorDetailsList();
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getGuarantorData ");

                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getGuarantorData", res), listCount });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getGuarentorAssetData()
        {
            try
            {
                _logger.Information("Started - getGuarentorAssetData ");

                var res = _sessionManager.GetGuarantorAssetList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getGuarentorAssetData ");

                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getGuarentorAssetData", res), listCount });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getGuarentorLiabilityData()
        {
            try
            {
                _logger.Information("Started - getGuarentorLiabilityData ");

                var res = _sessionManager.GetGuarantorLiabilityList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getGuarentorLiabilityData ");

                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getGuarentorLiabilityData", res), listCount });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        //need to check this...
        public IActionResult getGuarentorNetWorthData()
        {
            try
            {
                _logger.Information("Started - getGuarentorNetWorthData ");

                var res = _sessionManager.GetGuarantorNetWorthList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getGuarentorNetWorthData ");

                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getGuarentorNetWorthData", res), listCount });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getAssociateData()
        {
            try
            {
                _logger.Information("Started - getAssociateData ");

                var res = _sessionManager.GetAssociateDetailsDTOList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getAssociateData ");

                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getAssociateData", res), listCount });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getPrevFYData()
        {
            try
            {
                _logger.Information("Started - getPrevFYData ");

                var res = _sessionManager.GetAssociatePrevFYDetailsList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getPrevFYData ");

                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getPrevFYData", res), listCount });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public IActionResult getProjectCostData()
        {
            try
            {
                _logger.Information("Started - getProjectCostData ");

                var res = _sessionManager.GetProjectCostList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getProjectCostData ");

                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getProjectCostData", res), listCount });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        public IActionResult getProjectMeansOfFinanceData()
        {           

            try
            {
                _logger.Information("Started - getProjectMeansOfFinanceData ");

                var res = _sessionManager.GetProjectMeansOfFinanceList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getProjectMeansOfFinanceData ");

                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getProjectMeansOfFinanceData", res), listCount });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public IActionResult getProjectPrevFYData()
        {
            
            try
            {
                _logger.Information("Started - getProjectPrevFYData ");

                var res = _sessionManager.GetProjectPrevFYDetailsList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getProjectPrevFYData ");

                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getProjectPrevFYData", res), listCount });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getSecurityData()
        {
            try
            {
                _logger.Information("Started - getSecurityData ");

                var res = _sessionManager.GetSecurityDetailsList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getSecurityData ");

                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getSecurityData", res), listCount });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

    }
}
