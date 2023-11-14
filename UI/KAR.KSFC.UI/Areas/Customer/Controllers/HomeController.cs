using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Services.IServices;
using KAR.KSFC.UI.Services.IServices.Customer.LoanAccountingPromoter;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Customer.Controllers.Enquiry
{
    [Area("Customer")]
    [Authorize(Roles = RolesEnum.CE)]
    public class HomeController : Controller
    {
        private readonly SessionManager _sessionManager;
        private readonly ICustomerService _promoterService;
        private readonly ILoanAccountingPromoterService _loanAccountingService;
        private readonly IEnquirySubmissionService _enquirySubmissionService;
        private readonly ILogger _logger;
        public HomeController(ICustomerService promoterService, IEnquirySubmissionService enquirySubmissionService, SessionManager sessionManager, ILogger logger, ILoanAccountingPromoterService loanAccountingService)
        {
            _promoterService = promoterService;
            _enquirySubmissionService = enquirySubmissionService;
            _sessionManager = sessionManager;
            _loanAccountingService = loanAccountingService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<JsonResult> PopulateSubLocationList(string locationType, int locationId)
        {
            try
            {
                _logger.Information("Started - PopulateSubLocationList method for locationType = " + locationType + "locationId" + locationId);
                var response = await _enquirySubmissionService.PopulateSubLocationList(locationType, locationId);
                _logger.Information("Completed - PopulateSubLocationList method for locationType = " + locationType + "locationId" + locationId);
                return new JsonResult(response);

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading PopulateSubLocationList  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> PopulateFinanceTypeList(int categoryId)
        {
            try
            {
                _logger.Information("Started - PopulateFinanceTypeList method for categoryId = " + categoryId);
                var response = await _enquirySubmissionService.PopulateFinanceTypeList(categoryId);
                _sessionManager.SetDDListProjectFinanceType(response);
                _logger.Information("Completed - PopulateFinanceTypeList method for categoryId = " + categoryId);
                return new JsonResult(response);

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading PopulateFinanceTypeList  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
        /// <summary>
        /// Customer Logout
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                _logger.Information("Started -  Logout HttpGet Method");
                //_logger.Information("Entered into Customer Logout method");
                if (HttpContext.Session.IsAvailable)
                {
                    var accToken = _sessionManager.GetLoginCustToken();// HttpContext.Session.GetString(SessionCustToken);
                    await _promoterService.Logout(_sessionManager.GetLoginCustUserName(), accToken);// HttpContext.Session.GetString(SessionCustUser), accToken);
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.Session.Clear();
                }
                _logger.Information("Completed -  Logout HttpGet Method");
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Logout page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                // _logger.Error("Error occured while Customer Logout. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        //Home
        public async Task<IActionResult> Index()
        {

            var accountingOfficerLoans = await _loanAccountingService.GetAllAccountingLoanNumber();
            _sessionManager.SetAllAccountingLoanNumber(accountingOfficerLoans);
            ViewBag.Count = accountingOfficerLoans.Count;
            return View();
        }
        //Dashboard or Notifications


    }
}
