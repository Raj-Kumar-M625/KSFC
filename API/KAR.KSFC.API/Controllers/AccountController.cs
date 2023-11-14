using AutoMapper;

using KAR.KSFC.API.ServiceFacade.External.Interface;
using KAR.KSFC.API.ServiceFacade.Internal.Interface;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.AdminModule;
using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Common.Dto.Email;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Security;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.Components.Common.Utilities.Templates.Email;
using KAR.KSFC.Components.Common.Utilities.UserIdentity;
using KAR.KSFC.Components.Common.Utilities.Validations;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Controllers
{
    /// <summary>
    /// UnAuthorized methods like registration,Customer,Employee Login process related functionalities performed here.
    /// </summary>
    public class AccountController : BaseApiController
    {
        private readonly IMobileService _mobileService;
        private readonly IPanService _panService;
        private readonly IAccountService _accountService;
        private readonly ISmsService _smsService;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly IUserLogHistory _logHistoryService;
        private readonly IDscService _dscService;
        private readonly ILogger _logger;
        private readonly IEmployeeService _empService;

        /// <summary>
        /// constructor Dependency injection
        /// </summary>
        /// <param name="mobileService"></param>
        /// <param name="accountService"></param>
        /// <param name="smsService"></param>
        /// <param name="tokenService"></param>
        /// <param name="panService"></param>
        /// <param name="logHistoryService"></param>
        /// <param name="dscService"></param>
        /// <param name="emailService"></param>
        /// <param name="logger"></param>CustomerLoginPanVerification
        public AccountController(IMobileService mobileService,
            IAccountService accountService, ISmsService smsService, ITokenService tokenService,
              IPanService panService, IUserLogHistory logHistoryService, IDscService dscService,
              IEmailService emailService, ILogger logger, IEmployeeService empService)
        {
            _mobileService = mobileService;
            _panService = panService;
            _accountService = accountService;
            _smsService = smsService;
            _tokenService = tokenService;
            _logHistoryService = logHistoryService;
            _dscService = dscService;
            _logger = logger;
            _emailService = emailService;
            _empService = empService;
        }

        #region Genaral.  
        /// <summary>
        /// Genarate Otp Generic method.
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="process"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("OtpGenaration")]
        public async Task<SmsInfoDTO> OtpGeneration(string mobile, string process, CancellationToken token)
        {
            //Start - AccountController, OtpGeneration : For mobile and process. Calling SendSMS. Header (Username), Datetime
            var response = await _smsService.SendSMS(mobile, process).ConfigureAwait(false);
            //End - AccountController, OtpGeneration : For mobile and process. SendSMS is successful
            //Start - AccountController, OtpGeneration : Save OTP for mobile and process. 
            await _mobileService.SaveOtp(response.otp, mobile, false, process, token).ConfigureAwait(false);
            //End - AccountController, OtpGeneration : Save OTP for mobile and process. 
            return response;
        }

        /// <summary>
        /// Verify Genarated Otp
        /// </summary>
        /// <param name="otp"></param>
        /// <param name="mobile"></param>
        /// <param name="process"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet, Route("OtpVerification")]
        public async Task<IActionResult> OtpVerification(string otp, string mobile, string process, CancellationToken token)
        {
            bool isVerified = await _mobileService.IsOtpVerSuccessfull(otp, mobile, process, token).ConfigureAwait(false);
            if (isVerified)
            {
                await _mobileService.PostSuccessDeleteRec(mobile, process, token).ConfigureAwait(false);
                return Ok(new ApiResultResponse(CustomErrorMessage.E11, "Success"));
            }
            return new BadRequestObjectResult(new ApiResponse(400, CustomErrorMessage.E12));
        }
        #endregion

        #region Customer Registration.

        /// <summary>
        /// Generate OTP
        /// </summary>
        /// <param name="det"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("GenerateOtp")]
        public async Task<IActionResult> GenerateOtp(RegistrationDTO det, CancellationToken token)
        {
            _logger.Warning("Tested by Serilog Warning");
            _logger.Error("Tested by Error");
            _logger.Information("Tested by Serilog Info");

            if (det.MobileNum == string.Empty || !Validation.Mobile(det.MobileNum))
            {
                return new BadRequestObjectResult(new ApiResponse(400, CustomErrorMessage.E13));
            }

            //if he try to genarate otp with same number from different browser/tab within otp expire time. 
            var isOtpAlreadyGenarated = await _mobileService.IsGeneratedOtpExpired(det.MobileNum, token).ConfigureAwait(false);

            if (isOtpAlreadyGenarated != null)
            {
                return Ok(new ApiResultResponse(201, isOtpAlreadyGenarated.Otpexpirationdatetime, CustomErrorMessage.E09));
            }

            //Validate whether this mobile number exist in promotor/regduser table.
            var userReg_Info = await _mobileService.IsMobileNumberAlreadyExist(det.MobileNum, token).ConfigureAwait(false);

            if (userReg_Info != null)
            {
                if (string.IsNullOrEmpty(userReg_Info.Pan))//phone number attached with 
                {
                    return new NotFoundObjectResult(new ApiException(404, CustomErrorMessage.E02));
                }
                else
                {
                    return new NotFoundObjectResult(new ApiException(404, CustomErrorMessage.E01));
                }

            }
            var response = await OtpGeneration(det.MobileNum, det.Process, token).ConfigureAwait(false);
            if (det.Process != "ReviewE")
                await _accountService.SaveUserTryAction(det.MobileNum, "M", "failed", 0, ProcessEnum.EnquirySubmission, token).ConfigureAwait(false);
            //save user action for only in register page for analysis.        
            return Ok(new ApiResultResponse(response, "Success"));
        }

        /// <summary>
        /// Resend otp while registration.
        /// </summary>
        /// <param name="det"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("ResendOtp")]
        public async Task<IActionResult> ResendOtp(RegistrationDTO det, CancellationToken token)
        {
            //_logger.Information("Resend otp process started.");
            return await GenerateOtp(det, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Verify Otp
        /// </summary>
        /// <param name="det"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp(RegistrationDTO det, CancellationToken token)
        {
            bool isVerified = await _mobileService.IsOtpVerSuccessfull(det.Otp, det.MobileNum, det.Process, token).ConfigureAwait(false);

            if (isVerified)
            {
                await _accountService.SaveUserTryAction(Convert.ToString(det.MobileNum), "M", "Success", 0, det.Process, token).ConfigureAwait(false);//save user action for only in register page for analysis.
                await _mobileService.PostSuccessDeleteRec(det.MobileNum, det.Process, token).ConfigureAwait(false);

                //_logger.Information("otp verification Successful.");
                return Ok(new ApiResultResponse("OTP verified successfully.", "Success"));
            }
            else
            {
                await _accountService.SaveUserTryAction(Convert.ToString(det.MobileNum), "M", "failed", 0, det.Process, token).ConfigureAwait(false);

                //_logger.Information("otp verification UnSuccessful.");
                return new NotFoundObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { CustomErrorMessage.E12 }
                });
            }
        }

        /// <summary>
        /// verify otp with NSDL portal whether its active or not.
        /// </summary>
        /// <param name="det"></param>
        /// <returns></returns>
        [HttpPost, Route("PanVerification")]
        public async Task<IActionResult> VerifyPan(RegistrationDTO det, CancellationToken token)
        {
            if (det.PanNum == string.Empty || !Validation.Pan(det.PanNum))//regex validation pan. return correct error
            {
                await _accountService.SaveUserTryAction(det.PanNum, "P", "failed", 0, det.Process, token).ConfigureAwait(false);

                return new BadRequestObjectResult(new ApiResponse(400, CustomErrorMessage.E06));
            }
            var UserInfo = await _panService.IsPanAlreadyExist(det.PanNum, token).ConfigureAwait(false);//TO DO check with promotor and unit and NSDL portal table as well                
            if (UserInfo != null)
            {
                if (UserInfo.mobile != det.MobileNum)
                {
                    await _accountService.SaveUserTryAction(det.PanNum, "P", "failed", 0, det.Process, token).ConfigureAwait(false);
                    return NotFound(String.Format(CustomErrorMessage.E04, UserInfo.mobile.Substring(0, 2) + "XXXXXX" + UserInfo.mobile.Substring(UserInfo.mobile.Length - 2, 2), UserInfo.Branch));
                }
            }

            //validate pan status from NSDL portal. 
            //if status is active
            await _accountService.SaveUserTryAction(det.PanNum, "P", "Success", 0, det.Process, token).ConfigureAwait(false);
            return Ok(new ApiResultResponse("PAN verification successful.", "Success"));

            //if status is not active 
            // return BadRequest("pan number is inactive please enter valid pan number.");

        }

        /// <summary>
        /// After successfull validation Saves the Customer data. 
        /// </summary>
        /// <param name="regDetails"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// @Devaraj
        [HttpPost, Route("SaveRegistration")]
        public async Task<IActionResult> SaveRegestrationDetails(RegduserTab regDetails, CancellationToken token) //Pass registartaion model object
        {
            if (regDetails == null)
            {
                return BadRequest(CustomErrorMessage.E06);
            }

            var responseId = await _accountService.SaveRegistration(regDetails, token).ConfigureAwait(false);
            return Ok(new ApiResultResponse(responseId, "Success"));
        }

        #endregion

        #region Customer Login.

        /// <summary>
        /// Verify login and send otp to mobile number linked with the pan number.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("CustomerLoginPanVerification")]
        public async Task<IActionResult> CustomerLoginPanVerification(CustLoginDTO info, CancellationToken token)
        {
            if (info.PanNum == string.Empty || !Validation.Pan(info.PanNum))
            {
                await _accountService.SaveUserTryAction(info.PanNum, "P", "failed", 0, info.Process, token).ConfigureAwait(false);
                return BadRequest(CustomErrorMessage.E06);
            }

            if (info.IsForceLogin == false)
            {
                var isAlreadyActiveUser = await _accountService.IsCustAlreadyActive(info.PanNum, token).ConfigureAwait(false);

                if (isAlreadyActiveUser == true)
                {
                    return Conflict("This user session is already active.");
                }
            }

            var UserInfo = await _panService.IsPanAlreadyExist(info.PanNum, token).ConfigureAwait(false);

            if (UserInfo == null)
            {
                await _accountService.SaveUserTryAction(info.PanNum, "P", "failed", 0, info.Process, token).ConfigureAwait(false);
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { CustomErrorMessage.E07 }
                });

                // return NotFound(CustomErrorMessage.E07);
            }

            //if he try to genarate otp with same number from different browser/tab within otp expire time. 
            var isOtpAlreadyGenarated = await _mobileService.IsGeneratedOtpExpired(UserInfo.mobile, token).ConfigureAwait(false);

            if (isOtpAlreadyGenarated != null)
            {
                SmsInfoDTO det = new();
                det.Mobile = isOtpAlreadyGenarated.MobileNo;
                det.OTPExpDateTime = isOtpAlreadyGenarated.Otpexpirationdatetime.Value;
                return Ok(new ApiResultResponse(200, det, CustomErrorMessage.E09));
            }

            if (UserInfo.mobile == string.Empty || UserInfo.mobile == null)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { CustomErrorMessage.E08 }
                });
            }
            await _accountService.SaveUserTryAction(info.PanNum, "P", "Success", 0, info.Process, token).ConfigureAwait(false);
            var response = await OtpGeneration(UserInfo.mobile, info.Process, token).ConfigureAwait(false);
            return Ok(new ApiResultResponse(response, "Success"));
        }

        /// <summary>
        /// Resend otp in customer login page.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("CustLogResOtp")]
        public async Task<IActionResult> CustomerLoginResendOtp(CustLoginDTO info, CancellationToken token)
        {
            return await CustomerLoginPanVerification(info, token).ConfigureAwait(false);
        }

        /// <summary>
        /// customer login after successfull verification of pan and otp. 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost, Route("CustomerLogin")]
        public async Task<IActionResult> CustomerLogin(CustLoginDTO info, CancellationToken token)
        {
            var IpAddress = Request.Headers["IpAddress"].FirstOrDefault();
            var CustClaims = new CustomerClaimsDTO
            {
                Pan = info.PanNum.ToUpper(),
                Role = RolesEnum.Customer,
                IpAddress = IpAddress,
                Email = "",
                MobileNumber = await _accountService.GetUserMobile(info.PanNum, token)
            };

            var claims = new List<Claim>
                 {
                    new Claim(IndentityClaimsConstants.pan,info.PanNum.ToUpper()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType,RolesEnum.Customer),
                    new Claim("IpAddress",IpAddress),
                };
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();
            await _logHistoryService.UpdateCustomerLoginHistory(IpAddress, info.PanNum, accessToken, refreshToken, token).ConfigureAwait(false);
            var CustomerDet = new
            {
                AccessToken = accessToken,
                Claims = CustClaims,
                RefreshToken = refreshToken,
                IpAddress = IpAddress,
                Role = RolesEnum.Customer
            };
            return Ok(new ApiResultResponse(CustomerDet, "Success"));
        }

        #endregion

        #region KSFC User forgot password.

        /// <summary>
        /// Forget password for KSFC User send otp to registered mobile number.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("ForgotPasswordGenOtp")]
        public async Task<IActionResult> KsfcUserForgotPassword(EmployeeLoginDTO info, CancellationToken token)
        {
            var user = await _accountService.KsfcUserForgotPassword(info.EmpId, token).ConfigureAwait(false);

            if (user == null)
            {
                return new NotFoundObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "EmployeeId does not exist." }
                });
            }

            var response = await OtpGeneration(user.Emp.EmpMobileNo, info.Process, token).ConfigureAwait(false);
            return Ok(new ApiResultResponse(response, "Success"));
        }

        /// <summary>
        /// Resend otp on Employee forgot password.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>       
        [HttpPost, Route("ForgotPassResOtp")]
        public async Task<IActionResult> ForgotPasswordResendotp(EmployeeLoginDTO info, CancellationToken token)
        {
            return await KsfcUserForgotPassword(info, token).ConfigureAwait(false); ;
        }

        /// <summary>
        /// Forget password for KSFC User validate and trigger email/SMS.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost, Route("ValidateAndSendPassword")]
        public async Task<IActionResult> ValidateKsfcUserSendPassword(EmployeeLoginDTO info, CancellationToken token)
        {
            var user = await _accountService.KsfcUserForgotPassword(info.EmpId, token).ConfigureAwait(false); ;
            if (user == null)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "User not found" }
                });
            }

            var newPassword = SecurityHandler.GetRandomPassword();
            var encryPassword = SecurityHandler.Base64Encode(newPassword);

            await _accountService.UpdateNewpassword(user.EmpId, encryPassword, token);

            var empdata = await _empService.GetAllEmployeesById(info.EmpId, token).ConfigureAwait(false);
            if (empdata != null)
            {
                var emailReq = new EmailServiceRequest
                {
                    ToEmail = empdata.TeyPresentEmail,
                    Body = EmailTemplate.GetNewPassword(newPassword, empdata.TeyName),
                    Subject = "KSFC New Password"
                };
                await _emailService.SendEmailAsync(emailReq);

                await _smsService.SendSms(new Components.Common.Dto.SMS.SmsDataModel
                {
                    Message = EmailTemplate.GetNewPassword(newPassword, empdata.TeyName),
                    MobileNumber = empdata.EmpMobileNo.ToString()
                });
            }

            return Ok(new ApiResultResponse("Password has been sent to registered mobile number and Email.", "Success"));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string CreateEmailBodyForOTP(string OTPMSG, string Name)
        {
            //string dirPath = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            string fileName = "MailBodyTemplats/MailTemplateForOTP.html";
            string filePath = fileName;
            using (StreamReader reader = new(filePath))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{OTPMSG}", OTPMSG);
            body = body.Replace("{Candidate}", Name);
            return body;
        }

        #endregion

        #region Admin/KSFC User Login.

        /// <summary>
        /// Verify whether Employee is within KSWAN range. 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="token"></param>
        /// <returns></returns>

        [HttpPost, Route("IsKswanRange")]
        public async Task<IActionResult> CheckIP(EmployeeLoginDTO info, CancellationToken token)
        {
            var result = await _accountService.IsIpUnderKSWAN(info.Ip, token);
            return Ok(new ApiResultResponse(result, "Success"));
        }

        /// <summary>
        /// Validate Employee using DSC key if employee try to login from outside KSWAN range.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("DSCVerification")]
        public async Task<IActionResult> VerifyDSC(EmployeeLoginDTO info, CancellationToken token)
        {
            var dscDet = await _dscService.AuthenticateDSC(info.PublicKey, info.EmpId, token).ConfigureAwait(false);
            if (dscDet == null)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "DSC not matched" }

                });
            }

            return Ok(new ApiResultResponse("DSC verified Successfully.", "Success"));
        }

        /// <summary>
        /// Verify Enmployee Logged in already if not let them login.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="token"></param>
        /// <returns></returns>

        [HttpPost, Route("AdminLogin")]
        public async Task<IActionResult> AdminLogin(EmployeeLoginDTO info, CancellationToken token)
        {
            try
            {
                _logger.Information($"Started - AdminLogin with Mobile {info.Mobile}");

                if (!info.IsForceLogin)
                {
                    var isAcitve = await _accountService.IsAdmAlreadyActive(info.EmpId, token).ConfigureAwait(false);

                    if (isAcitve)
                    {
                        return Conflict("This user session is already active.");
                    }
                }
                var encryPassword = SecurityHandler.Base64Encode(info.Password);

                //var loginInfo = await _accountService.AdminLogin(info.EmpId, encryPassword, token).ConfigureAwait(false);
                // hardcoded hashed password, needs to be removed later. 
                var loginInfo = await _accountService.AdminLogin(info.EmpId, "U3VrQDEyMzQ=", token).ConfigureAwait(false);
                var AssignedRoles = await _accountService.GetAssignedRoles(info.EmpId, token);

                if (loginInfo == null)
                {
                    return new UnauthorizedObjectResult(new ApiResponse(401));
                }
                var IpAddress = Request.Headers["IpAddress"].FirstOrDefault();
                var empClaims = new ClaimsDTO
                {
                    EmpId = loginInfo.EmpId,
                    Name = loginInfo.Emp.TeyName,
                    Email = loginInfo.Emp.TeyPermanentEmail == null ? "TEST@TEST.COM" : loginInfo.Emp.TeyPermanentEmail,
                    Mobile = loginInfo.Emp.EmpMobileNo,
                    Pan = loginInfo.Emp.TeyPanNum,
                    Role = RolesEnum.Employee,
                    IsPasswordChanged = loginInfo.IsPswdChng.Value,
                    IpAddress = IpAddress
                };

                var claims = new List<Claim>
                 {
                    new Claim(JwtRegisteredClaimNames.Email,empClaims.Email),
                    new Claim(IndentityClaimsConstants.Id,empClaims.EmpId),
                    new Claim(JwtRegisteredClaimNames.GivenName,empClaims.Name),
                    new Claim(IndentityClaimsConstants.phoneNumber,empClaims.Mobile),
                    new Claim(IndentityClaimsConstants.pan,empClaims.Pan),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType,empClaims.Role),
                    new Claim("IpAddress",IpAddress),
                };

                var accessToken = _tokenService.GenerateAccessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken();
                await _logHistoryService.UpdateAdminLoginHistory(info.EmpId, accessToken, refreshToken, IpAddress, token).ConfigureAwait(false);

                var UserDet = new
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    Claims = empClaims,
                    IpAddress = IpAddress,
                    AccessebleRoles = AssignedRoles
                };

                _logger.Information($"Completed - AdminLogin with Mobile {info.Mobile}");

                return Ok(new ApiResultResponse(UserDet, "Success"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured while AdminLogin. Error message is  {ex.Message} {Environment.NewLine} The stack trace is {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// Whenever access token about to expire call this method to get new access and refresh token without logging out the admin.
        /// </summary>
        /// <param name="tokenInfo"></param>
        /// <param name="token"></param>
        /// <returns></returns>

        [HttpPost, Route("Refresh")]
        public async Task<IActionResult> Refresh(TokenDTO tokenInfo, CancellationToken token)
        {
            var refreshToken = await _tokenService.Refresh(tokenInfo, token).ConfigureAwait(false);
            if (refreshToken == null)
            {
                return new UnauthorizedObjectResult(
                     new ApiException(401, "invalid tokens.")
                );
            }
            return Ok(new ApiResultResponse(refreshToken, "Success"));
        }

        /// <summary>
        /// Whenever access token about to expire call this method to get new access and refresh token without logging out the customer.
        /// </summary>
        /// <param name="tokenInfo"></param>
        /// <param name="token"></param>
        /// <returns></returns>

        [HttpPost, Route("PromoterRefresh")]
        public async Task<IActionResult> PromoterRefresh(TokenDTO tokenInfo, CancellationToken token)
        {
            var refreshToken = await _tokenService.PromoterRefresh(tokenInfo, token).ConfigureAwait(false);
            if (refreshToken == null)
            {
                return new UnauthorizedObjectResult(
                     new ApiException(401, "invalid tokens.")
                );
            }
            return Ok(new ApiResultResponse(refreshToken, "Success"));
        }

        [HttpGet, Route("unitdetails")]
        public async Task<IActionResult> GetAccountDetails(CancellationToken token)
        {
            var result = await _accountService.GetAccountDetails(token);
            return Ok(new ApiResultResponse(result, "Success"));
            
        }
    }
}
