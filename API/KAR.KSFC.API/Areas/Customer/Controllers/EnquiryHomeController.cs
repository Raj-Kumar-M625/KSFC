using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.ServiceFacade.External.Interface;
using KAR.KSFC.API.ServiceFacade.Internal.Interface;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.Components.Common.Utilities.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Customer.Controllers
{
    [KSFCAuthorization]
    public class EnquiryHomeController : BaseApiController
    {
        private readonly IAccountService _accountService;
        //private readonly ILogger<EnquiryHomeController> _logger;
        private readonly UserInfo _userInfo;
        private readonly IEnquiryHomeService _enquiryHome;
        private readonly IMobileService _mobileService;
        private readonly ISmsService _smsService;
        private readonly IPanService _panService;
        private readonly ILogger _logger;
        /// <summary>
        ///  constructor Dependency injection
        /// </summary>
        /// <param name="smsService"></param>
        /// <param name="accountService"></param>
        /// <param name="logger"></param>
        /// <param name="userInfo"></param>
        /// <param name="mobileService"></param>
        /// <param name="panService"></param>
        /// <param name="enquiryHome"></param>
        public EnquiryHomeController(ISmsService smsService, IAccountService accountService, ILogger logger, UserInfo userInfo, IMobileService mobileService, IPanService panService, IEnquiryHomeService enquiryHome = null)
        {
            _smsService = smsService;
            _accountService = accountService;
            _logger = logger;
            _userInfo = userInfo;
            _enquiryHome = enquiryHome;
            _mobileService = mobileService;
            _panService = panService;
        }

        /// <summary>
        /// Customer logout.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="token"></param>
        [Authorize]
        [HttpPost, Route("CustomerLogout")]
        public async Task CustomerLogout(CustLoginDTO info, CancellationToken token)
        {
            await _accountService.CustomerLogout(info.PanNum, token).ConfigureAwait(false);
        }
        [HttpPost, Route("OtpGenaration")]
        public async Task<SmsInfoDTO> OtpGeneration(string mobile, string process, CancellationToken token)
        {
            //Start - AccountController, OtpGeneration : For mobile and process. Calling SendSMS. Header (Username), Datetime
            var response = await _smsService.SendSMS(mobile,process).ConfigureAwait(false);
            //End - AccountController, OtpGeneration : For mobile and process. SendSMS is successful
            //Start - AccountController, OtpGeneration : Save OTP for mobile and process. 
            await _mobileService.SaveOtp(response.otp, mobile, false, process, token).ConfigureAwait(false);
            //End - AccountController, OtpGeneration : Save OTP for mobile and process. 
            return response;
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="det"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("GenOtpForReview")]
        public async Task<IActionResult> GenOtpForReview(RegistrationDTO det, CancellationToken token)
        {
            try
            {
                _logger.Information(String.Format("Started - GenOtpForReview method for MobileNum:{0} Otp:{1} Process:{2} PanNum:{3}",
                    det.MobileNum, det.Otp, det.Process, det.PanNum));
                if (det.MobileNum == string.Empty || !Validation.Mobile(det.MobileNum))
                {
                    _logger.Information("Error - 400 CustomErrorMessage.E13");
                    return new BadRequestObjectResult(new ApiResponse(400, CustomErrorMessage.E13));
                }
                var response = await OtpGeneration(det.MobileNum, det.Process, token).ConfigureAwait(false);
                _logger.Information(String.Format("Completed - GenOtpForReview method for MobileNum:{0} Otp:{1} Process:{2} PanNum:{3}",
                    det.MobileNum, det.Otp, det.Process, det.PanNum));
                return Ok(new ApiResultResponse(response, "Success"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GenOtpForReview page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("SubmitEnquiry")]
        public async Task<IActionResult> SubmitEnquiry(EnquiryDetailsDto enquiryDetailsDto, CancellationToken token)
        {
            try
            {
                _logger.Information(String.Format("Started - SubmitEnquiry method for EnquiryId:{0} Note:{1}", enquiryDetailsDto.EnquiryId, enquiryDetailsDto.Note));
                var id = await _enquiryHome.SubmitEnquiry(enquiryDetailsDto.Note, (int)enquiryDetailsDto.EnquiryId, token).ConfigureAwait(false);
                if (id > 0)
                {
                    _logger.Information(String.Format("Completed - SubmitEnquiry method for EnquiryId:{0} Note:{1}", enquiryDetailsDto.EnquiryId, enquiryDetailsDto.Note));
                    return Ok(new ApiResultResponse(id, "Enquiry submitted successfully."));
                }
                _logger.Information("Error - 400 CustomErrorMessage.E06");
                return new BadRequestObjectResult(new ApiResponse(400, CustomErrorMessage.E06));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading SubmitEnquiry page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Resend otp while registration.
        /// </summary>
        /// <param name="det"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("ResOtpForReview")]
        public async Task<IActionResult> ResOtpForReview(RegistrationDTO det, CancellationToken token)
        {
            //_logger.Information("Resend otp process started.");
            return await GenOtpForReview(det, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Verify Otp
        /// </summary>
        /// <param name="det"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("VerOtpForReview")]
        public async Task<IActionResult> VerifyOtp(RegistrationDTO det, CancellationToken token)
        {
            try
            {
                _logger.Information(String.Format("Started - VerifyOtp method for MobileNum:{0} Otp:{1} Process:{2} PanNum:{3}",
                    det.MobileNum, det.Otp, det.Process, det.PanNum));
                bool isVerified = await _mobileService.IsOtpVerSuccessfull(det.Otp, det.MobileNum, det.Process, token).ConfigureAwait(false);

                if (isVerified)
                {
                    await _mobileService.PostSuccessDeleteRec(det.MobileNum, det.Process, token).ConfigureAwait(false);
                    _logger.Information(String.Format("Completed - VerifyOtp method for MobileNum:{0} Otp:{1} Process:{2} PanNum:{3}",
                    det.MobileNum, det.Otp, det.Process, det.PanNum));
                    //_logger.Information("otp verification Successful.");
                    return Ok(new ApiResultResponse("OTP verified successfully.", "Success"));
                }
                else
                {
                    await _accountService.SaveUserTryAction(Convert.ToString(det.MobileNum), "M", "failed", 0, ProcessEnum.EnquirySubmission, token).ConfigureAwait(false);

                    //_logger.Information("otp verification UnSuccessful.");
                    return new NotFoundObjectResult(new ApiValidationErrorResponse
                    {
                        Errors = new[] { CustomErrorMessage.E12 }
                    });
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading VerifyOtp page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
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
            try
            {
                _logger.Information(String.Format("Started - VerifyPan method for MobileNum:{0} Otp:{1} Process:{2} PanNum:{3}",
                    det.MobileNum, det.Otp, det.Process, det.PanNum));
                if (det.PanNum == string.Empty || !Validation.Pan(det.PanNum))//regex validation pan. return correct error
                {
                    await _accountService.SaveUserTryAction(det.PanNum, "P", "failed", 0, ProcessEnum.EnquirySubmission, token).ConfigureAwait(false);
                    _logger.Information("Error - 400, CustomErrorMessage.E06");
                    return new BadRequestObjectResult(new ApiResponse(400, CustomErrorMessage.E06));
                }

                var UserInfo = await _panService.IsPanAlreadyExist(det.PanNum, token).ConfigureAwait(false);//TO DO check with promotor and unit and NSDL portal table as well                
                await _accountService.SaveUserTryAction(det.PanNum, "P", "Success", 0, ProcessEnum.EnquirySubmission, token).ConfigureAwait(false);
                if (UserInfo != null)
                {
                    if (UserInfo.mobile != det.MobileNum)
                    {
                        _logger.Information("Error - CustomErrorMessage.E04");
                        return NotFound(String.Format(CustomErrorMessage.E04, UserInfo.mobile.Substring(0, 2) + "XXXXXX" + UserInfo.mobile.Substring(UserInfo.mobile.Length - 2, 2), UserInfo.Branch));
                    }
                }
                _logger.Information(String.Format("Completed - VerifyPan method for MobileNum:{0} Otp:{1} Process:{2} PanNum:{3}",
                    det.MobileNum, det.Otp, det.Process, det.PanNum));
                //validate pan status from NSDL portal. 
                //if status is active
                return Ok(new ApiResultResponse("PAN verification successful.", "Success"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading VerifyPan page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }/// <summary>
         /// ///.//////////////////////////////////////////////////////////////////////////////////////////
         /// </summary>
         /// <param name="token"></param>
         /// <returns></returns>

        [HttpGet, Route("GetAllEnquries")]
        public async Task<IActionResult> GetAllEnquiriesAsync(CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetAllEnquiriesAsync method ");
                var userInfo = _userInfo.Pan;
                //return EnqMinimalInfo - 5 prperties from 2 tables
                var enquiry = await _enquiryHome.GetAllEnquiryAsync(token).ConfigureAwait(false);
                if (enquiry == null)
                {
                    _logger.Information("Error - 404 Enquiry Details not exists!");
                    return new NotFoundObjectResult(new ApiException(404, "Enquiry Details not exists!"));
                }
                _logger.Information("Completed - GetAllEnquiriesAsync method ");
                return Ok(new ApiResultResponse(200, enquiry, "Success."));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetAllEnquiriesAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet, Route("DeleteEnquiry")]
        public async Task<IActionResult> DeleteEnquiryAsync(int enqId, CancellationToken token)
        {
            //return EnqMinimalInfo - 5 prperties from 2 tables
            try
            {
                _logger.Information("Started - DeleteEnquiryAsync method for enqId = " + enqId);
                bool isDeleted = await _enquiryHome.DeleteEnquiryAsync(enqId, token).ConfigureAwait(false);
                if (!isDeleted)
                {
                    _logger.Information("Error - 404 Enquiry details not exists!");
                    return new NotFoundObjectResult(new ApiException(404, "Enquiry details not exists!"));
                }
                _logger.Information("Completed - DeleteEnquiryAsync method for enqId = " + enqId);
                return Ok(new ApiResultResponse(200, true, "Enquiry Details Deleted Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteEnquiryAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet, Route("EnquirySummary")]
        public async Task<IActionResult> ViewEnquirySummaryAsync(int enqId, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - ViewEnquirySummaryAsync method for enqId = " + enqId);
                //return EnqMinimalInfo - 5 prperties from 2 tables
                var enquiry = await _enquiryHome.GetEnquiryByIdAsync(enqId, token).ConfigureAwait(false);
                if (enquiry == null)
                {
                    _logger.Information("Error - 404 Basic Details not exists!");
                    return new NotFoundObjectResult(new ApiException(404, "Basic Details not exists!"));
                }
                _logger.Information("Completed - ViewEnquirySummaryAsync method for enqId = " + enqId);
                return Ok(new ApiResultResponse(200, enquiry, "Success."));

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading ViewEnquirySummaryAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNewEnquiryAsync(string pan, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - AddNewEnquiryAsync method with pan");
                var eqnuiryId = await _enquiryHome.AddNewEnqiry(pan, token).ConfigureAwait(false);
                if (eqnuiryId > 0)
                {
                    _logger.Information("Completed - AddNewEnquiryAsync method with pan");
                    return Ok(new ApiResultResponse(200, eqnuiryId, "Success."));
                }
                _logger.Information("Error - 404 Some error occured!");
                return new NotFoundObjectResult(new ApiException(404, "Some error occured!"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading AddNewEnquiryAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
    }
}
