using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using KAR.KSFC.Components.Common.Dto.Enums;
using Microsoft.AspNetCore.Authorization;
using KAR.KSFC.Components.Common.Logging.Client;
using System;

namespace KAR.KSFC.UI.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = RolesEnum.Customer)]
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
                _logger.Information("Started - getAddressData method ");
                int listCount = 0;
                var res = _sessionManager.GetAddressList();
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getAddressData method");
                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "AddressdataView", res), listCount });

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading getAddressData page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getRegistrationData()
        {
            try
            {
                _logger.Information("Started - getRegistrationData method ");
                var res = _sessionManager.GetRegistrationDetList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getRegistrationData method");
                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getRegistrationData", res), listCount });

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading getRegistrationData page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getPromoterData()
        {
            try
            {
                _logger.Information("Started - getPromoterData method ");
                var res = _sessionManager.GetPromoterDetailsList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getPromoterData method ");
                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getPromoterData", res), listCount });
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading getPromoterData page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getPromoterAssetData()
        {
            try
            {
                _logger.Information("Started - getPromoterAssetData method ");
                var res = _sessionManager.GetPromoterAssetList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getPromoterAssetData method");
                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getPromoterAssetData", res), listCount });
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading getPromoterAssetData page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getPromoterLiabilityData()
        {
            try
            {
                _logger.Information("Started - getPromoterLiabilityData method ");
                var res = _sessionManager.GetPromoterLiabilityList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getPromoterLiabilityData method");
                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getPromoterLiabilityData", res), listCount });
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading getPromoterLiabilityData page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        //need to check this
        public IActionResult getPromoterNetWorthData()
        {
            try
            {
                _logger.Information("Started - getPromoterNetWorthData method ");
                var res = _sessionManager.GetPromoterNetWorthList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getPromoterNetWorthData method");
                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getPromoterNetWorthData", res), listCount });
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading getPromoterNetWorthData page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getGuarantorData()
        {
            try
            {
                _logger.Information("Started - getGuarantorData method ");
                int listCount = 0;
                var res = _sessionManager.GetGuarantorDetailsList();
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getGuarantorData method");
                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getGuarantorData", res), listCount });
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading getGuarantorData page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getGuarentorAssetData()
        {
            try
            {
                _logger.Information("Started - getGuarentorAssetData method");
                var res = _sessionManager.GetGuarantorAssetList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getGuarentorAssetData method");
                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getGuarentorAssetData", res), listCount });

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading getGuarentorAssetData page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getGuarentorLiabilityData()
        {
            try
            {
                _logger.Information("Started - getGuarentorLiabilityData method ");
                var res = _sessionManager.GetGuarantorLiabilityList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getGuarentorLiabilityData method");
                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getGuarentorLiabilityData", res), listCount });
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading getGuarentorLiabilityData page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        //need to check this...
        public IActionResult getGuarentorNetWorthData()
        {
            try
            {
                _logger.Information("Started - getGuarentorNetWorthData method ");
                var res = _sessionManager.GetGuarantorNetWorthList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getGuarentorNetWorthData method");
                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getGuarentorNetWorthData", res), listCount });

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading getGuarentorNetWorthData page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getAssociateData()
        {
            try
            {
                _logger.Information("Started - getAssociateData method ");
                var res = _sessionManager.GetAssociateDetailsDTOList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getAssociateData method");
                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getAssociateData", res), listCount });
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading getAssociateData page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getPrevFYData()
        {
            try
            {
                _logger.Information("Started - getPrevFYData method ");
                var res = _sessionManager.GetAssociatePrevFYDetailsList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getPrevFYData method");
                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getPrevFYData", res), listCount });

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading getPrevFYData page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public IActionResult getProjectCostData()
        {
            try
            {
                _logger.Information("Started - getProjectCostData method ");
                var res = _sessionManager.GetProjectCostList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getProjectCostData method");
                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getProjectCostData", res), listCount });

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading getProjectCostData page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getProjectMeansOfFinanceData()
        {
            try
            {
                _logger.Information("Started - getProjectMeansOfFinanceData method ");
                var res = _sessionManager.GetProjectMeansOfFinanceList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getProjectMeansOfFinanceData method");
                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getProjectMeansOfFinanceData", res), listCount });
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading getProjectMeansOfFinanceData page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public IActionResult getProjectPrevFYData()
        {
            try
            {
                _logger.Information("Started - getProjectPrevFYData method ");
                var res = _sessionManager.GetProjectPrevFYDetailsList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getProjectPrevFYData method");
                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getProjectPrevFYData", res), listCount });
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading getProjectPrevFYData page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public IActionResult getSecurityData()
        {
            try
            {
                _logger.Information("Started - getSecurityData method ");
                var res = _sessionManager.GetSecurityDetailsList();
                int listCount = 0;
                bool status = false;
                if (res != null)
                {
                    listCount = res.Count;
                    status = true;
                }
                _logger.Information("Completed - getSecurityData method");
                return Json(new { isValid = status, html = Helper.RenderRazorViewToString(this, viewPath + "getSecurityData", res), listCount });
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading getSecurityData page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }
}
