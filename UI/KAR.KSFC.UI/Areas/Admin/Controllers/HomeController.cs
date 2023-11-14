using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Models;
using KAR.KSFC.UI.Services.IServices;
using KAR.KSFC.UI.Services.IServices.Admin;
using KAR.KSFC.UI.Services.IServices.Admin.LoanAccounting;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IInspectionOfUnitService;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using KAR.KSFC.UI.Helpers;
using System.Linq;
using Microsoft.AspNetCore.DataProtection;
using KAR.KSFC.UI.Security;

namespace KAR.KSFC.UI.Areas.Admin.Controllers
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]


    public class HomeController : Controller
    {
        const string SessionAdminUser = "AdminUsername";
        const string SessionAdminToken = "JWTToken";
        const string SessionAdminRefToken = "AdminRefToken";
        const string SessionSwitchedRole = "SwitchedRole";

        private readonly ILogger _logger;
        private readonly IAdminService _adminService;
        private readonly SessionManager _sessionManager;
        private readonly IIdmService _idmService;
        private readonly ILoanAccountingService _loanAccountingService;
        private readonly IInspectionOfUnitService _inspectionOfUnitService;
        private readonly IDataProtector protector;


        public HomeController(IAdminService adminService, SessionManager sessionManager, ILogger logger, IDataProtectionProvider dataProtectionProvider,
                                ILoanAccountingService loanAccountingService, IIdmService idmService, IInspectionOfUnitService inspectionOfUnitService,
            DataProtectionPurposeStrings dataProtectionPurposeStrings
            )
        {
            _adminService = adminService;
            _logger = logger;
            _idmService = idmService;
            _sessionManager = sessionManager;
            _loanAccountingService = loanAccountingService;
            _inspectionOfUnitService = inspectionOfUnitService;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.AdminRouteValue);
        }
        public async Task<IActionResult> Index()
        {
            _logger.Information("Home controller Index action invoked.");
            var empID = HttpContext.Session.GetString(SessionAdminUser);

            var accountDetails = await _idmService.GetAccountDetails();
            _sessionManager.SetAccountDetails(accountDetails);
            //ViewBag.details = accountDetails.Select(x => x.ut_name).ToList();

            var loans = await _idmService.GetAllLoanNumber(empID);
            _sessionManager.SetAllLoanNumber(loans);
            ViewBag.Count = loans.Count;
            return View(accountDetails);
        }
        public async Task<IActionResult> LoanAccountingIndex()
        {
            _logger.Information("Home controller Index action invoked.");
            var empID = HttpContext.Session.GetString(SessionAdminUser);
            var accountingOfficerLoans = await _loanAccountingService.GetAllAccountingLoanNumber(empID);
            _sessionManager.SetAllAccountingLoanNumber(accountingOfficerLoans);
            ViewBag.Count = accountingOfficerLoans.Count;
            return View();
        }
        public IActionResult SwitchRole()
        {
            //display role buttons based on userclaimrole session
            if (_sessionManager.GetEmployeeAccesableRoles() != null)
                ViewBag.AccessableRoles = _sessionManager.GetEmployeeAccesableRoles();

            return View();
        }

        public IActionResult SetSwitchRole(string Role)
        {
            HttpContext.Session.SetString(SessionSwitchedRole, Role);
            //Set Role in session then redirect user to Index
            // return RedirectToAction("Index", "Home", new { Area = "Admin" }); - commented by gagana
           var empID= HttpContext.Session.GetString(SessionAdminUser);


            switch (Role)
            {
                case "Accounting Officer":
                    return RedirectToAction("LoanAccountingIndex", "Home", new { Area = "Admin" });
                default:
                    return RedirectToAction("Index", "Home", new { Area = "Admin"});
            }
        }

        [HttpGet]
        public JsonResult ContinueSession()
        {
            _logger.Information("Home controller ContinueSession action invoked.");
            return Json(new { message = true });
        }

        /// <summary>
        /// Change Password Page for Admin Login
        /// </summary>
        /// <returns></returns>
        public IActionResult ChangePassword()
        {
            _logger.Information("Home controller ChangePassword action invoked.");
            return View();
        }

        /// <summary>
        /// Change Password for Admin Login Post method
        /// </summary>
        /// <param name="passViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel passViewModel)
        {
            try
            {
                _logger.Information("Started - ChangePassword with ChangePasswordViewModel ");

                if (HttpContext.Session.GetString(SessionAdminUser) != null)
                {
                    var empId = HttpContext.Session.GetString(SessionAdminUser);
                    var token = HttpContext.Session.GetString(SessionAdminToken);

                    var response = await _adminService.ChangePassword(empId, token, passViewModel.CurrentPassword, passViewModel.NewPassword);
                    if (response == string.Empty)
                    {
                        await _adminService.UserLogout(empId, token);
                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        HttpContext.Session.Clear();
                    }
                    else
                    {
                        ViewBag.errorMsg = "old password entered is wrong.";
                        return View();
                    }

                }
                _logger.Information("Completed - ChangePassword with ChangePasswordViewModel ");

                return RedirectToAction("AdminLogin", "Account", new { Area = "" });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! ChangePassword HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Admin Logout 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            try
            {
                _logger.Information("Started - Admin Logout method");

                if (HttpContext.Session.GetString(SessionAdminUser) != null)
                {
                    var token = HttpContext.Session.GetString(SessionAdminToken);
                    var responseHttp = await _adminService.UserLogout(HttpContext.Session.GetString(SessionAdminUser), token);
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.Session.Clear();
                }
                _logger.Information("Completed - Admin Logout method");

                return RedirectToAction("AdminLogin", "Account", new { Area = "" });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured while Admin Logout. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public async Task<IActionResult> Dashboard()
        {
            _logger.Information("Home controller Dashboard action invoked.");
            var empID = HttpContext.Session.GetString(SessionAdminUser);

            var allLoans = await _idmService.GetAllLoanNumber(empID);
            _sessionManager.SetAllLoanNumber(allLoans);
            ViewBag.Count = allLoans.Count;

            var loans = _sessionManager.GetAllLoanNumber().Select(e =>
                {
                    e.EncryptedLoanAcc = protector.Protect(e.LoanAcc.ToString());
                    e.EncryptedLoanUnit = protector.Protect(e.LoanUnit);
                    e.EncryptedLoanSub = protector.Protect(e.LoanSub.ToString());
                    e.EncryptedInUnit = protector.Protect(e.InUnit.ToString());
                    e.EncryptedInOffc = protector.Protect(e.InOffc.ToString());
                    return e;
                }).ToList().FirstOrDefault(e => e.LoanAcc== 98765432101);

            return View(loans);
        }


    }
}
