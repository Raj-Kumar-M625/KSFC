using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Models;
using KAR.KSFC.UI.Services.IServices;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace KAR.KSFC.UI.Controllers
{
    public class AccountController : Controller
    {
        const string SessionCustUser = "Username";
        const string SessionCustToken = "JWTToken";
        const string SessionAdminUser = "AdminUsername";
        const string SessionAdminToken = "JWTToken";
        const string SessionAdminRefToken = "AdminRefToken";
        //private readonly ILoggerService _logger;
        private readonly ILogger _logger;
        private readonly IConfiguration _configure;
        private readonly IOtpService _otpService;
        private readonly IPanService _panService;
        private readonly IDscService _dscService;
        private readonly IRegisterService _registerService;
        private readonly ICustomerService _promoterService;
        private readonly IAdminService _adminService;
        private readonly SessionManager _sessionManager;
        private IWebHostEnvironment _hostEnvironment;
        public AccountController(IConfiguration configure, //ILoggerService logger,
            IOtpService otpService,
            IPanService panService, IDscService dscService, IRegisterService registerService, ICustomerService promoterService,
            IAdminService adminService, SessionManager sessionManager, IWebHostEnvironment environment, ILogger logger)
        {
            _configure = configure;
             _logger = logger;
            _otpService = otpService;
            _panService = panService;
            _dscService = dscService;
            _registerService = registerService;
            _promoterService = promoterService;
            _adminService = adminService;
            _sessionManager = sessionManager;
            _hostEnvironment = environment;
            _logger = logger;
        }
        #region Register


        /// <summary>
        /// Loads Registration page for Customer
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Register()
        {
            try
            {
                _logger.Information("Started loading registration page.");
                // var registerViewModel = await _registerService.GetDDLConstitutionTypes();//Get constitution types to bind the dropdown list
                var allConstitutionTypes = await _registerService.GetAllConstitutionTypes();
                _sessionManager.SetAllConstitutionTypes(allConstitutionTypes); //Get Constitution types in session to validate pan 4th char with constitution types
                var registerViewModel = new RegisterViewModel();
                registerViewModel.ListConstitutionTypes = allConstitutionTypes.Select(x => new SelectListItem
                {
                    Text = x.CnstDets,
                    Value = x.CnstCd.ToString()
                }).ToList();

                if (_otpService.GetOtpAttempts() && _panService.GetPanAttempts() && registerViewModel != null)
                {
                    return View(registerViewModel);
                }

                _logger.Error("Error occured while loading registration page.");
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";

                return View("~/Views/Shared/Error.cshtml");
            }
            catch(Exception ex)
            {
                _logger.Error("Error occured while loading registration page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Verifies details and Creates new Customer
        /// </summary>
        /// <param name="regViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel regViewModel)
        {
            try
            {
                _logger.Information("Entered into HttpPost Customer Register method with Mobile No-" + regViewModel.MobileNumber + " and PAN number- " + regViewModel.PanNumber);
               
                _logger.Information("Invoking register service with regViewModel data");
                var response = await _registerService.RegisterUser(regViewModel);
                if (response)
                {
                    TempData["RegistrationResult"] = "<div class='alert alert-success'  style='text-align:center' role='alert'>Registration is successful! Please login using PAN. </ div > ";
                    return View("Result");
                }
                _logger.Error("Error occured while saving registration details with Mobile No-" + regViewModel.MobileNumber + " and PAN number- " + regViewModel.PanNumber);
                
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
            catch(Exception ex)
            {
                _logger.Error("Error occured while saving registration details with Mobile No-" + regViewModel.MobileNumber + " and PAN number- " + regViewModel.PanNumber + ". Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        /// <summary>
        /// Generates OTP for Mobile in Registration page
        /// </summary>
        /// <param name="mobileNo"></param>
        /// <returns>OTP or Message</returns>
        [HttpPost]
        public async Task<IActionResult> GenerateOtp(string mobileNo)
        {
            //return OtpService.Generate();
            try
            {
                _logger.Information("Started - GenerateOtp method for "+ ProcessEnum.PReg + " with Mobile No" + mobileNo);

                var response = await _otpService.Generate(ProcessEnum.PReg, mobileNo, null, null);

                _logger.Information("Completed - GenerateOtp method for " + ProcessEnum.PReg + " with Mobile No" + mobileNo +"Response - "+ response.Value);               
                return response;
            }
            catch(Exception ex)
            {

                _logger.Error("Error occured while Generating OTP for Mobile no." + mobileNo + " in registration page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Resend OTP for Mobile No while registration. 
        /// </summary>
        /// <param name="mobileNo"></param>
        /// <returns></returns>
        public async Task<IActionResult> ResendOtp(string mobileNo)
        {
            try
            {
                _logger.Information("Started - Registration page ResendOtp method for" + ProcessEnum.PReg + "  with Mobile No" + mobileNo);

                var response = await _otpService.Resend(ProcessEnum.PReg, mobileNo, "", "");

                _logger.Information("Completed - Registration page ResendOtp method with Mobile No" + mobileNo+"Response - "+ response.Value);
                return response;
            }
            catch(Exception ex)
            {
                _logger.Error("Error occured while resend otp with Mobile No.-" + mobileNo + ". Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Validates OTP for Registration page
        /// </summary>
        /// <param name="entOtp"></param>
        /// <param name="mobileNum"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ValidateOtp(string entOtp, string mobileNum)
        {
            try
            {
                _logger.Information("Entered into Registration page ValidateOtp method with entOtp: " + entOtp + " and mobile no: " + mobileNum);
                
                var response = await _otpService.Validate(ProcessEnum.PReg, mobileNum, entOtp);

                _logger.Information("Completed Registration page ValidateOtp method with entOtp: " + entOtp + " and mobile no: " + mobileNum + "response" + response.Value);
                return response;
            }
            catch(Exception ex)
            {
                _logger.Error("Error occured while Validate OTP for Mobile number - " + mobileNum + " and OTP -" + entOtp + ". Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        /// <summary>
        /// Verifies PAN with Constitution type and and Database
        /// </summary>
        /// <param name="panNo"></param>
        /// <param name="constitutionName"></param>
        /// <param name="mobileNum"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> VerifyPanWithConstTypeAndDb(string panNo, string constitutionName, string mobileNum)
        {
            try
            {
                _logger.Information("Entered into Registration page VerifyPanWithConstTypeAndDb method with panNo: " + panNo + " , constitutionName = " + constitutionName + " and mobile no: " + mobileNum);
                
                var response = await _panService.VerifyPanWithConstTypeAndDb(panNo, constitutionName, mobileNum);

                _logger.Information("Completed Registration page VerifyPanWithConstTypeAndDb method with panNo: " + panNo + " , constitutionName = " + constitutionName + " and mobile no: " + mobileNum);
                return response;
            }
            catch(Exception ex)
            {
                _logger.Error("Error occured while VerifyPanWithConstTypeAndDb for Mobile number - " + mobileNum + " and PAN number -" + panNo + ". Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        #endregion

        #region Customer Login


        /// <summary>
        /// Customer Login Get Method
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Login()
        {
            try
            {
                _logger.Information("Entered into HttpGet Customer Login method");
                if (HttpContext.Session.GetString(SessionCustUser) == null)
                {
                    if (_otpService.GetOtpAttemptsCustomer() && _panService.GetPanAttempts())
                    {
                        return View();
                    }
                    _logger.Error("Error occurred at HttpGet Customer Login method");
                    ViewBag.error = "Unknown error occurred! Please try again after sometime.";

                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { Area = "Customer" });
                }
            }
            catch(Exception ex)
            {
                _logger.Error("Error occured while loading Customer Login page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Generate Otp for Pan Number in Customer Login
        /// </summary>
        /// <param name="panNo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GenerateOtpForCustLogin(string panNo)
        {
            try
            {
                _logger.Information("Entered into GenerateOtpForCustLogin method with panNo - " + panNo);

                var response = await _otpService.Generate(ProcessEnum.PLogin, null, panNo, null);

                _logger.Information("Completed GenerateOtpForCustLogin method with panNo - " + panNo);
                return response;
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured in GenerateOtpForCustLogin for PAN number-" + panNo + ". Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";

                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Resend OTP for Pan No in Customer Login
        /// </summary>
        /// <param name="panNo"></param>
        /// <returns></returns>
        public async Task<IActionResult> ResendOtpForCustLogin(string panNo)
        {
            try
            {
                _logger.Information("Entered into ResendOtpForCustLogin method with panNo - " + panNo);

                var response = await _otpService.Resend(ProcessEnum.PLogin, null, panNo, null);

                _logger.Information("Entered into ResendOtpForCustLogin method with panNo - " + panNo);
                return response;
            }
            catch(Exception ex)
            {
                _logger.Error("Error occured in ResendOtpForCustLogin with Pan number-" + panNo + ". Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Verify OTP for Customer
        /// </summary>
        /// <param name="entOtp"></param>
        /// <param name="mobileNum"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> VerifyOtp(string entOtp)
        {
            try
            {
                _logger.Information("Entered into VerifyOtp method with entOtp - " + entOtp);

                var response = await _otpService.Validate(ProcessEnum.PLogin, "", entOtp);

                _logger.Information("Completed VerifyOtp method with entOtp - " + entOtp);
                return response;
            }
            catch(Exception ex)
            {
                _logger.Error("Error occured while VerifyOtp for customer login page. The OTP is " + entOtp + " and  Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        /// <summary>
        /// Verifies Customer Login credentials, Gets tokens and create Authorization claims
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(CustomerViewModel custViewModel)
        {
            try
            {
                _logger.Information("Entered into HttpPost Customer Login method with pan number as " + custViewModel.PanNumber + " mobile number is " + custViewModel.MobileNumber);
                if (custViewModel.PanNumber != null && custViewModel.OTP != null)
                {
                    _logger.Information("Invoking promoter service with custViewModel data");
                    var userClaims = await _promoterService.Login(custViewModel);
                    if (userClaims != null)
                    {
                        // Role authorization code start
                        ClaimsIdentity iden = new(new[] {
                    new Claim(ClaimTypes.Role, userClaims.Claims.Role)
                    }, CookieAuthenticationDefaults.AuthenticationScheme);

                        var principal = new ClaimsPrincipal(iden);
                        var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        // Role authorization code end
                        _sessionManager.SetCustLoginTime(DateTime.Now.ToString("HH:mm"));//To be added from Claims From API
                        _sessionManager.SetCustLoginDateTime(DateTime.Now.ToString());
                        _sessionManager.SetLoginCustUserName(custViewModel.PanNumber.ToUpper());
                        _sessionManager.SetLoginCustToken(userClaims.AccessToken);
                        _sessionManager.SetLoginCustRefToken(userClaims.RefreshToken);
                        _sessionManager.SetLoginCustMobile(userClaims.Claims.MobileNumber);
                        return RedirectToAction("Index", "Home", new { Area = "Customer" });
                    }
                }
                _logger.Error("Error occured while Customer Login. For PAN number: " + custViewModel.PanNumber + "  and mobile number : ");
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";

                return View("~/Views/Shared/Error.cshtml");
            }
            catch(Exception ex)
            {
                _logger.Error("Error occured while Customer Login. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";

                return View("~/Views/Shared/Error.cshtml");
            }
        }

        #endregion

        #region KSFC Login

        /// <summary>
        /// Admin Login Get method
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> AdminLogin()
        {
            try
            {
                _logger.Information("Entered into  AdminLogin method");
                bool isDSCRequired = false;
                if (HttpContext.Session.GetString(SessionAdminUser) == null)
                {
                    _logger.Information("Invoking dscService with GetLocalIpAddress()");
                    if (await _dscService.IsDscRequired(GetLocalIpAddress()))
                        isDSCRequired = true;
                }
                else
                {
                     _logger.Information("Redirecting to HttpPost  AdminLogin method for preexisting SessionAdminUser " + HttpContext.Session.GetString(SessionAdminUser));
                    return RedirectToAction("Index", "Home", new { Area = "Admin" });
                }
                if (TempData["Info"] != null)
                    ViewBag.Info = TempData["Info"];

                return View(new EmployeeLoginDTO { IsDSC = isDSCRequired });
            }
            catch(Exception ex)
            {
                _logger.Error("Error occured while loading AdminLogin page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Gets the IP address of the Client system
        /// </summary>
        /// <returns></returns>
        public string GetLocalIpAddress()
        {
            string ip = HttpContext.Connection.RemoteIpAddress.ToString();
            _logger.Information("Entered into  GetLocalIpAddress method for IP-" + ip);
            return ip;//DSC Not required
                      // return "::2";//DSC required
        }
        /// <summary>
        /// DSC credentials verification for Employee login
        /// </summary>
        /// <param name="empID"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DSCVerify(string empId, string publicKey)
        {
            try
            {
                _logger.Information("Entered into DSCVerify method for employee id: " + empId + " and DSC Public key: " + publicKey);

                var response = await _dscService.VerifyDsc(empId, publicKey);

                _logger.Information("Completed DSCVerify method for employee id: " + empId + " and DSC Public key: " + publicKey);
                return response;
            }
            catch(Exception ex)
            {
                _logger.Error("Error occured in DSCVerify method for employee id: " + empId + ", and Public key:" + publicKey + ". Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Admin Login Post method to check employee credentials and login
        /// </summary>
        /// <param name="empViewModel"></param>
        /// <returns></returns>
        [HttpPost]
            public async Task<IActionResult> AdminLogin(EmployeeLoginDTO empLoginDTO)
            {
            try
            {
                empLoginDTO.Password = "DummyPass"; // TO be removed later


                _logger.Information("Entered into HttpPost AdminLogin method for employee id: " + empLoginDTO.EmpId + " and Password : " + empLoginDTO.Password);
                if (empLoginDTO.EmpId.Length != 0 && empLoginDTO.Password != null)
                {

                    _logger.Information("Invoking admin service with empLoginDTO data");
                    var userClaims = await _adminService.Login(empLoginDTO);
                    //if (userClaims.ErrorMessage != null)
                    //{
                    //    ViewBag.error = userClaims.ErrorMessage.Replace('"', ' ');
                    //    return View(empLoginDTO);
                    //}
                    if (userClaims != null)
                    {
                        // Role authorization code start
                        ClaimsIdentity iden = new(new[] {
                    new Claim(ClaimTypes.Role, userClaims.Claims.Role)
                    }, CookieAuthenticationDefaults.AuthenticationScheme);

                        var principal = new ClaimsPrincipal(iden);
                        var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        // Role authorization code end
                        _sessionManager.SetCustLoginTime(DateTime.Now.ToString("HH:mm"));//To be added from Claims From API
                        _sessionManager.SetCustLoginDateTime(DateTime.Now.ToString());
                        HttpContext.Session.SetString(SessionAdminUser, empLoginDTO.EmpId);
                        HttpContext.Session.SetString(SessionAdminToken, userClaims.AccessToken);
                        HttpContext.Session.SetString(SessionAdminRefToken, userClaims.RefreshToken);
                        _sessionManager.SetEmployeeAccesableRoles(userClaims.AccessebleRoles);
                        var Roles = _sessionManager.GetEmployeeAccesableRoles();
                        //if user has more than one role, redirect him to switch role page. Else redirect user to Admin/Home/Index
                        if (userClaims.Claims.IsPasswordChanged)
                            return RedirectToAction("ChangePassword", "Home", new { Area = "Admin" });
                        else if (Roles.Count > 1)
                            return RedirectToAction("SwitchRole", "Home", new { Area = "Admin" });
                        else
                            return RedirectToAction("Index", "Home", new { Area = "Admin" });
                    }

                }
                _logger.Information("User entered invalid employee id and password in HttpPost AdminLogin method, employee id entered: " + empLoginDTO.EmpId);
                ViewBag.error = "Please enter valid employee id and password.";
                bool isDSCRequired = false;
                if (await _dscService.IsDscRequired(GetLocalIpAddress()))
                    isDSCRequired = true;
                return View(new EmployeeLoginDTO { IsDSC = isDSCRequired });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured on AdminLogin page for employee id: " + empLoginDTO.EmpId + ". Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";

                return View("~/Views/Shared/Error.cshtml");
            }
        }

        #endregion

        #region Admin Forgot Password
        /// <summary>
        /// Admin Forgot Password page 
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public IActionResult ForgotPassword()
        {
            try
            {
                _logger.Information("Entered into  admin ForgotPassword method");

                var res = _otpService.GetOtpAttemptsFP();

                _logger.Information("Completed invoking otp service");
                return View(new EmployeeViewModel() { });
            }
            catch(Exception ex)
            {
                _logger.Error("Error occured while loading ForgotPassword page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }


        /// <summary>
        /// Generate OTP method for Admin  Forgot Password
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public async Task<IActionResult> GenerateOtpForAdminForgotPass(string empId)
        {
            try
            {
                _logger.Information("Entered into GenerateOtpForAdminForgotPass method for employee id: " + empId);

                var response = await _otpService.Generate(ProcessEnum.AdminFP, null, null, empId);

                _logger.Information("Completed GenerateOtpForAdminForgotPass method for employee id: " + empId);
                return response;
            }
            catch(Exception ex)
            {
                _logger.Error("Error occured in GenerateOtpForAdminForgotPass for empId -" + empId + ". Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Resend OTP for Employee Id of Admin Forgot password page
        /// </summary>
        /// <param name="panNo"></param>
        /// <returns></returns>
        public async Task<IActionResult> ResendOtpForAdminForgotPass(string empId)
        {
            try
            {
                _logger.Information("Entered into ResendOtpForAdminForgotPass method for employee id: " + empId);

                var response = await _otpService.Resend(ProcessEnum.AdminFP, null, null, empId);

                _logger.Information("Completed ResendOtpForAdminForgotPass method for employee id: " + empId);
                return response;
            }
            catch(Exception ex)
            {
                _logger.Error("Error occured in ResendOtpForAdminForgotPass for empId -" + empId + ". Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Verify OTP for Admin Forgot password page
        /// </summary>
        /// <param name="entOtp"></param>
        /// <param name="mobileNum"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> VerifyOtpForgotPass(string entOtp, string empId, string mobileNo)//OtpVerification
        {
            try
            {
                _logger.Information("Entered into VerifyOtpForgotPass method for employee id: " + empId + " and OTP entered: " + entOtp);

                var response = await _otpService.Validate(ProcessEnum.AdminFP, mobileNo, entOtp);

                _logger.Information("Completed VerifyOtpForgotPass method for employee id: " + empId + " and OTP entered: " + entOtp);
                return response;
            }
            catch(Exception ex)
            {
                _logger.Error("Error occured in VerifyOtpForgotPass method. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        /// <summary>
        /// Verifies KSFC USER id and send password to registered mobile
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public async Task<IActionResult> ValidateUserSendPassword(string empId)
        {
            try
            {
                _logger.Information("Entered into ValidateUserSendPassword method for employee id: " + empId);
                var response = await _adminService.ValidateUserSendPassword(empId);
                _logger.Information("Completed ValidateUserSendPassword method for employee id: " + empId);
                return response;
            }
            catch(Exception ex)
            {
                _logger.Error("Error occured in VerifyOtpForgotPass method. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public FileResult ShowPDF()
        {
            string fileName = "TC.pdf";
            string filePath = Path.Combine(_hostEnvironment.WebRootPath, @"documents\" + fileName);
            var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
            return File(fileStream, "application/pdf");
        }

        #endregion

    }
}
