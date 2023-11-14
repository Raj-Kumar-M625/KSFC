using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using KAR.KSFC.Components.Common.Dto.Enums;

namespace KAR.KSFC.UI.Services
{
    public class OtpService : IOtpService
    {

        //For Registration Page
        private static int resOtpAttemptsLeft;
        private static int valOtpAttempsLeft;


        //For Customer Login
        private static int genOtpAttemptsCustLogin;
        private static int verOtpAtmLeftCustLogin;
        private static string custMobNum;

        //For Forgot Password
        private static int verOtpAtmLeftsAdmForgotPass;
        private static int resOtpAttemptsAdmForgotPass;
        private static string adminFPMobNum;

        //For All three
        private static int otpExpTimeConfig;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configure;

        public OtpService(IConfiguration configure, IHttpClientFactory clientFactory)
        {
            _configure = configure;
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Resets the OTP attempts when page loads
        /// </summary>
        /// <returns></returns>
        public bool GetOtpAttempts()
        {
            resOtpAttemptsLeft = Convert.ToInt32(_configure["SysConfig:ResOtpAttemptsLeft"]);
            valOtpAttempsLeft = Convert.ToInt32(_configure["SysConfig:ValOtpAttempsLeft"]);
            otpExpTimeConfig = Convert.ToInt32(_configure["SysConfig:OtpExpiryTimeConfig"]);
            return true;
        }

        /// <summary>
        /// Generic method for generate OTP for Registration, Customer Login and Admin Forgot Password page
        /// </summary>
        /// <param name="process"></param>
        /// <param name="mobileNo"></param>
        /// <param name="panNo"></param>
        /// <param name="empId"></param>
        /// <returns></returns>
        public async Task<JsonResult> Generate(string process, string mobileNo = null, string panNo = null, string empId = null)
        {
            if (process ==ProcessEnum.PReg && mobileNo != null)
            {
                if (resOtpAttemptsLeft >= 1)
                {
                    resOtpAttemptsLeft--;
                    RegistrationDTO det = new()
                    {
                        Process = process,
                        MobileNum = mobileNo
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(det), Encoding.UTF8, "application/json");
                    var client = _clientFactory.CreateClient("ksfcApi");
                    var responseHttp = await client.PostAsync($"Account/GenerateOtp", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        if (successObj.StatusCode == 201)
                        {
                            //var responseString = await responseHttp.Content.ReadAsStringAsync();

                            //DateTime otpExpTim1 = JsonConvert.DeserializeObject<DateTime>(successObj.Result.ToString());
                            DateTime otpExpTime = Convert.ToDateTime(successObj.Result.ToString());
                            int tspanSec = Convert.ToInt32((otpExpTime - DateTime.Now).TotalSeconds);
                            return new JsonResult(new { Id = "1", Mobile = mobileNo.Substring(0, 2) + "XXXXXX" + mobileNo.Substring(mobileNo.Length - 2, 2), OtpExpTimeLeftInSec = tspanSec, Message = successObj.Message });
                        }
                        else if (successObj.StatusCode == 200)
                        {
                            SmsInfoDTO smsInfoObj = JsonConvert.DeserializeObject<SmsInfoDTO>(successObj.Result.ToString());
                            return new JsonResult(new { Id = "1", Mobile = mobileNo.Substring(0, 2) + "XXXXXX" + mobileNo.Substring(mobileNo.Length - 2, 2), OtpExpTimeLeftInSec = otpExpTimeConfig, Message = "OTP successfully generated." });
                        }
                    }
                    else if (responseHttp.StatusCode == HttpStatusCode.NotFound)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var failureObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        //var failureExcp = JsonConvert.DeserializeObject<ApiException>(failureObj.Value.ToString());
                        return new JsonResult(new { Id = "0", Mobile = mobileNo.Substring(0, 2) + "XXXXXX" + mobileNo.Substring(mobileNo.Length - 2, 2), OtpExpTimeLeftInSec = 0, Message = failureObj.Message });
                    }
                    else if (responseHttp.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var failureObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        //var failureRes = JsonConvert.DeserializeObject<ApiResponse>(failureObj.Value.ToString());
                        return new JsonResult(new { Id = "0", Mobile = mobileNo.Substring(0, 2) + "XXXXXX" + mobileNo.Substring(mobileNo.Length - 2, 2), OtpExpTimeLeftInSec = 0, Message = failureObj.Message });
                    }
                }
                else
                {
                    return new JsonResult(new { Id = "2", Mobile = mobileNo.Substring(0, 2) + "XXXXXX" + mobileNo.Substring(mobileNo.Length - 2, 2), OtpExpTimeLeftInSec = 0, Message = "You have exceeded maximum number of attempts for OTP generation." });
                }
            }

            if (process == ProcessEnum.PLogin && panNo != null)
            {
                if (genOtpAttemptsCustLogin >= 1)
                {
                    genOtpAttemptsCustLogin--;

                    CustLoginDTO info = new()
                    {
                        PanNum = panNo,
                        Process = process,
                        IsForceLogin = false
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json");
                    var client = _clientFactory.CreateClient("ksfcApi");
                    var responseHttp = await client.PostAsync($"Account/CustomerLoginPanVerification", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)//ABle to communicate with API
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        //Handle if conditon appropriately
                        if (successObj.StatusCode == 201)//OTP already created. Handling scenarios based on specific code
                        {
                            SmsInfoDTO objInfo = JsonConvert.DeserializeObject<SmsInfoDTO>(successObj.Result.ToString());
                            int tspanSec = Convert.ToInt32((objInfo.OTPExpDateTime - DateTime.Now).TotalSeconds);

                            custMobNum = objInfo.Mobile;
                            return new JsonResult(new { id = "1", otpExpTimeLeftInSec = tspanSec, message = "OTP already generated." });
                        }
                        else if (successObj.StatusCode == 200)//OTP newly created. Handling scenarios based on specific code
                        {
                            var resInfo = JsonConvert.DeserializeObject<SmsInfoDTO>(successObj.Result.ToString());

                            custMobNum = resInfo.Mobile;
                            return new JsonResult(new { id = "1", otpExpTimeLeftInSec = otpExpTimeConfig, mobNoEnd = custMobNum.Substring(0, 2) + "XXXXXX" + custMobNum.Substring(custMobNum.Length - 2, 2), message = "OTP successfully generated." });
                        }
                        else if (successObj.StatusCode == 202)//User session is already active. Wait for 10 minutes. Handling scenarios based on specific code
                        {
                            //var resMessage = JsonConvert.DeserializeObject<string>(successObj.Message.ToString());


                            return new JsonResult(new { id = "1", otpExpTimeLeftInSec = otpExpTimeConfig, message = successObj.Message.ToString() });
                        }

                    }
                    else if (responseHttp.StatusCode == HttpStatusCode.NotFound)//No API or No Database connectivity
                    {
                        var failureString = await responseHttp.Content.ReadAsStringAsync();
                        var failObj = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(failureString);

                        return new JsonResult(new { id = "0", otpExpTimeLeftInSec = 0, message = failObj.Errors.FirstOrDefault().ToString() });
                    }
                    else if (responseHttp.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var failureString = await responseHttp.Content.ReadAsStringAsync();
                        var failureObj = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(failureString);
                        //var failureMsgArray = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(failureObj.ToString());
                        return new JsonResult(new { id = "0", otpExpTimeLeftInSec = 0, message = failureObj.Errors.FirstOrDefault().ToString() });
                    }
                }
                else
                {
                    return new JsonResult(new { id = "2", mobileNo = "", message = "You have exceeded maximum number of attempts for OTP generation." });
                }
                //
                //If control reached here then some unknown issue
                //return new JsonResult(new { id = "2", mobileNo = "", message = "An unknown error has occurred. Contact support." });
            }

            if (process == ProcessEnum.AdminFP && empId != null)
            {
                if (resOtpAttemptsAdmForgotPass >= 1)
                {
                    resOtpAttemptsAdmForgotPass--;
                    EmployeeLoginDTO info = new() { EmpId = empId, Process = process };
                    var content = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json");
                    var client = _clientFactory.CreateClient("ksfcApi");
                    var responseHttp = await client.PostAsync($"Account/ForgotPasswordGenOtp", content);

                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        SmsInfoDTO resMobObj = JsonConvert.DeserializeObject<SmsInfoDTO>(successObj.Result.ToString());
                        adminFPMobNum = resMobObj.Mobile;
                        return new JsonResult(new { id = "1", otpExpTimeLeftInSec = otpExpTimeConfig, mobNum = adminFPMobNum.Substring(0, 2) + "XXXXXX" + adminFPMobNum.Substring(adminFPMobNum.Length - 2, 2), message = "Received OTP is valid for next 10 minutes." });
                    }
                    else if (responseHttp.StatusCode == HttpStatusCode.NotFound)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var failObj = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(responseString);
                        //var failObj = JsonConvert.DeserializeObject<NotFoundObjectResult>(responseString);
                        //var errorMsg = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(failObj.Value.ToString());
                        return new JsonResult(new { id = "0", otpExpTimeLeftInSec = 0, message = failObj.Errors.FirstOrDefault().ToString() });
                    }
                }
                return new JsonResult(new { id = "2", message = "You have exceeded maximum number of attempts for OTP generation." });
            }

            if (process == "ReviewE" && mobileNo != null)
            {
                if (resOtpAttemptsLeft >= 1)
                {
                    resOtpAttemptsLeft--;
                    RegistrationDTO det = new()
                    {
                        Process = process,
                        MobileNum = mobileNo
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(det), Encoding.UTF8, "application/json");
                    var client = _clientFactory.CreateClient("ksfcApi");
                    var responseHttp = await client.PostAsync($"EnquiryHome/GenOtpForReview", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        if (successObj.StatusCode == 200)
                        {
                            SmsInfoDTO smsInfoObj = JsonConvert.DeserializeObject<SmsInfoDTO>(successObj.Result.ToString());
                            return new JsonResult(new { Id = "1", Mobile = mobileNo.Substring(0, 2) + "XXXXXX" + mobileNo.Substring(mobileNo.Length - 2, 2), OtpExpTimeLeftInSec = otpExpTimeConfig, Message = "OTP successfully generated." });
                        }
                    }
                    else if (responseHttp.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var failureObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        return new JsonResult(new { Id = "0", Mobile = mobileNo.Substring(0, 2) + "XXXXXX" + mobileNo.Substring(mobileNo.Length - 2, 2), OtpExpTimeLeftInSec = 0, Message = failureObj.Message });
                    }
                }
                else
                {
                    return new JsonResult(new { Id = "2", Mobile = mobileNo.Substring(0, 2) + "XXXXXX" + mobileNo.Substring(mobileNo.Length - 2, 2), OtpExpTimeLeftInSec = 0, Message = "You have exceeded maximum number of attempts for OTP generation." });
                }
            }
            return null;

        }

        /// <summary>
        /// Generic method for resend OTP for Registration, Customer Login and Admin Forgot Password page
        /// </summary>
        /// <param name="process"></param>
        /// <param name="mobileNo"></param>
        /// <param name="panNo"></param>
        /// <param name="empId"></param>
        /// <returns></returns>
        public async Task<JsonResult> Resend(string process, string mobileNo = "", string panNo = "", string empId = "")
        {
            var client = _clientFactory.CreateClient("ksfcApi");
            if (process == ProcessEnum.PReg && mobileNo != null)
            {
                if (resOtpAttemptsLeft >= 1)
                {
                    resOtpAttemptsLeft--;

                    RegistrationDTO det = new()
                    {
                        Process = process,
                        MobileNum = mobileNo
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(det), Encoding.UTF8, "application/json");

                    var responseHttp = await client.PostAsync($"Account/ResendOtp", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        if (successObj.StatusCode == 201)
                        {
                            //var responseString = await responseHttp.Content.ReadAsStringAsync();
                            // DateTime otpExpTime = JsonConvert.DeserializeObject<DateTime>(successObj.Result.ToString());
                            DateTime otpExpTime = Convert.ToDateTime(successObj.Result.ToString());
                            int tspanSec = Convert.ToInt32((otpExpTime - DateTime.Now).TotalSeconds);
                            return new JsonResult(new { Id = "1", Mobile = mobileNo.Substring(0, 2) + "XXXXXX" + mobileNo.Substring(mobileNo.Length - 2, 2), OtpExpTimeLeftInSec = tspanSec, Message = "You have " + resOtpAttemptsLeft.ToString() + " more attempt(s) left to resend OTP." });
                        }
                        else if (successObj.StatusCode == 200)
                        {
                            SmsInfoDTO smsInfoObj = JsonConvert.DeserializeObject<SmsInfoDTO>(successObj.Result.ToString());
                            return new JsonResult(new { Id = "1", Mobile = mobileNo.Substring(0, 2) + "XXXXXX" + mobileNo.Substring(mobileNo.Length - 2, 2), OtpExpTimeLeftInSec = otpExpTimeConfig, Message = "You have " + resOtpAttemptsLeft.ToString() + " more attempt(s) left to resend OTP." });
                        }
                    }
                    else if (responseHttp.StatusCode == HttpStatusCode.NotFound)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var failObj = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(responseString);
                        //var failureObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        //var failureExcp = JsonConvert.DeserializeObject<ApiException>(failureObj.Value.ToString());
                        return new JsonResult(new { Id = "0", Mobile = mobileNo.Substring(0, 2) + "XXXXXX" + mobileNo.Substring(mobileNo.Length - 2, 2), OtpExpTimeLeftInSec = 0, Message = failObj.Errors.FirstOrDefault().ToString() });
                    }
                    else if (responseHttp.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var failureObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        //var failureRes = JsonConvert.DeserializeObject<ApiResponse>(failureObj.Value.ToString());
                        return new JsonResult(new { Id = "0", Mobile = mobileNo.Substring(0, 2) + "XXXXXX" + mobileNo.Substring(mobileNo.Length - 2, 2), OtpExpTimeLeftInSec = 0, Message = failureObj.Message });
                    }

                }
                else
                {
                    return new JsonResult(new { Id = "2", Mobile = mobileNo.Substring(0, 2) + "XXXXXX" + mobileNo.Substring(mobileNo.Length - 2, 2), OtpExpTimeLeftInSec = 0, Message = "You have exceeded maximum number of attempts for OTP generation." });
                }
            }

            if (process == ProcessEnum.PLogin && panNo != null)//check if PanNo. is not null
            {
                if (genOtpAttemptsCustLogin >= 1)
                {
                    genOtpAttemptsCustLogin--;

                    CustLoginDTO info = new()
                    {
                        PanNum = panNo,
                        Process = process,
                        IsForceLogin = false
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json");
                    //var client = _clientFactory.CreateClient("ksfcApi");
                    var responseHttp = await client.PostAsync($"Account/CustomerLoginPanVerification", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        if (successObj.StatusCode == 201)
                        {
                            SmsInfoDTO objInfo = JsonConvert.DeserializeObject<SmsInfoDTO>(successObj.Result.ToString());
                            int tspanSec = Convert.ToInt32((objInfo.OTPExpDateTime - DateTime.Now).TotalSeconds);

                            custMobNum = objInfo.Mobile;
                            return new JsonResult(new { id = "1", otpExpTimeLeftInSec = tspanSec, message = "You have " + genOtpAttemptsCustLogin.ToString() + " more attempt(s) left to resend OTP." });
                        }
                        else if (successObj.StatusCode == 200)
                        {
                            SmsInfoDTO resInfo = JsonConvert.DeserializeObject<SmsInfoDTO>(successObj.Result.ToString());
                            custMobNum = resInfo.Mobile;
                            return new JsonResult(new { id = "1", otpExpTimeLeftInSec = otpExpTimeConfig, mobNoEnd = resInfo.Mobile.Substring(0, 2) + "XXXXXX" + resInfo.Mobile.Substring(resInfo.Mobile.Length - 2, 2), message = "You have " + genOtpAttemptsCustLogin.ToString() + " more attempt(s) left to resend OTP." });
                        }

                    }
                    else if (responseHttp.StatusCode == HttpStatusCode.NotFound)
                    {
                        var failureString = await responseHttp.Content.ReadAsStringAsync();
                        var failObj = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(failureString);
                        //var failureObj = JsonConvert.DeserializeObject<BadRequestObjectResult>(failureString);
                        //var failureMsgArray = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(failureObj.ToString());
                        //string failureMsg = failureMsgArray.Errors.ToString();

                        return new JsonResult(new { id = "0", otpExpTimeLeftInSec = 0, message = failObj.Errors.FirstOrDefault().ToString() });
                    }

                }

                return new JsonResult(new { id = "2", message = "You have exceeded maximum number of attempts for OTP generation." });

            }

            if (process == ProcessEnum.AdminFP && empId != null)//Check if EmpId is not null
            {
                if (resOtpAttemptsAdmForgotPass >= 1)
                {
                    resOtpAttemptsAdmForgotPass--;
                    EmployeeLoginDTO info = new() { EmpId = empId, Process = process };
                    var content = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json");
                    var responseHttp = await client.PostAsync($"Account/ForgotPassResOtp", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        SmsInfoDTO resMobObj = JsonConvert.DeserializeObject<SmsInfoDTO>(successObj.Result.ToString());
                        adminFPMobNum = resMobObj.Mobile;
                        return new JsonResult(new { id = "1", otpExpTimeLeftInSec = otpExpTimeConfig, mobNum = resMobObj.Mobile.Substring(0, 2) + "XXXXXX" + resMobObj.Mobile.Substring(resMobObj.Mobile.Length - 2, 2), message = "You have " + resOtpAttemptsAdmForgotPass.ToString() + " more attempt(s) left to resend OTP." });
                    }
                    else if (responseHttp.StatusCode == HttpStatusCode.NotFound)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var failObj = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(responseString);
                        // var failObj = JsonConvert.DeserializeObject<NotFoundObjectResult>(responseString);
                        //var errorMsg = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(failObj.Value.ToString());
                        return new JsonResult(new { id = "0", otpExpTimeLeftInSec = 0, message = failObj.Errors.FirstOrDefault().ToString() });
                    }
                }
                return new JsonResult(new { id = "2", message = "You have exceeded maximum number of attempts for OTP generation." });
            }
            if (process == "ReviewE" && mobileNo != null)
            {
                if (resOtpAttemptsLeft >= 1)
                {
                    resOtpAttemptsLeft--;

                    RegistrationDTO det = new()
                    {
                        Process = process,
                        MobileNum = mobileNo
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(det), Encoding.UTF8, "application/json");

                    var responseHttp = await client.PostAsync($"EnquiryHome/ResOtpForReview", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        if (successObj.StatusCode == 200)
                        {
                            SmsInfoDTO smsInfoObj = JsonConvert.DeserializeObject<SmsInfoDTO>(successObj.Result.ToString());
                            return new JsonResult(new { Id = "1", Mobile = mobileNo.Substring(0, 2) + "XXXXXX" + mobileNo.Substring(mobileNo.Length - 2, 2), OtpExpTimeLeftInSec = otpExpTimeConfig, Message = "You have " + (resOtpAttemptsLeft + 1).ToString() + " more attempt(s) left to resend OTP." });
                        }
                    }
                    else if (responseHttp.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var failureObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        return new JsonResult(new { Id = "0", Mobile = mobileNo.Substring(0, 2) + "XXXXXX" + mobileNo.Substring(mobileNo.Length - 2, 2), OtpExpTimeLeftInSec = 0, Message = failureObj.Message });
                    }

                }
                else
                {
                    return new JsonResult(new { Id = "2", Mobile = mobileNo.Substring(0, 2) + "XXXXXX" + mobileNo.Substring(mobileNo.Length - 2, 2), OtpExpTimeLeftInSec = 0, Message = "You have exceeded maximum number of attempts for OTP generation." });
                }
            }
            return null;
        }

        /// <summary>
        /// Generic method for verify OTP for Registration, Customer Login and Admin Forgot Password page
        /// </summary>
        /// <param name="process"></param>
        /// <param name="mobileNo"></param>
        /// <param name="otp"></param>
        /// <returns></returns>
        public async Task<JsonResult> Validate(string process, string mobileNo, string otp)
        {
            var client = _clientFactory.CreateClient("ksfcApi");
            if (process == ProcessEnum.PReg && otp != null)
            {
                RegistrationDTO det = new()
                {
                    Otp = otp,
                    MobileNum = mobileNo,
                    Process = process
                };
                var content = new StringContent(JsonConvert.SerializeObject(det), Encoding.UTF8, "application/json");
                var responseHttp = await client.PostAsync($"Account/VerifyOtp", content);

                if (valOtpAttempsLeft >= 1)
                {
                    valOtpAttempsLeft--;
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);

                        return new JsonResult(new { Id = "1", Message = successObj.Result.ToString() });
                    }
                    else if (responseHttp.StatusCode == HttpStatusCode.NotFound)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var failObj = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(responseString);
                        // var failObj = JsonConvert.DeserializeObject<NotFoundObjectResult>(responseString);
                        // var errorMsg = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(failObj.ToString());
                        return new JsonResult(new { Id = "0", Message = failObj.Errors.FirstOrDefault().ToString() });
                    }
                }
                else
                    return new JsonResult(new { Id = "2", Message = "Your session has been terminated." });
            }


            if (process == ProcessEnum.PLogin && otp != null)
            {
                RegistrationDTO det = new()
                {
                    Otp = otp,
                    MobileNum = custMobNum,
                    Process = process
                };
                var content = new StringContent(JsonConvert.SerializeObject(det), Encoding.UTF8, "application/json");
                var responseHttp = await client.PostAsync($"Account/VerifyOtp", content);
                if (verOtpAtmLeftCustLogin >= 1)
                {
                    verOtpAtmLeftCustLogin--;
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        //var successMsg = JsonConvert.DeserializeObject<string>(successObj.Result.ToString());
                        return new JsonResult(new { id = "1", message = successObj.Result.ToString() });
                    }
                    else if (responseHttp.StatusCode == HttpStatusCode.NotFound)
                    {
                        var resString = await responseHttp.Content.ReadAsStringAsync();
                        var failObj = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(resString);

                        return new JsonResult(new { id = "0", message = failObj.Errors.FirstOrDefault().ToString() });
                    }
                }
                return new JsonResult(new { id = "2", message = "Your session has been terminated." });
            }

            if (process == ProcessEnum.AdminFP && otp != null)
            {
                var responseHttp = await client.GetAsync($"Account/OtpVerification?otp={otp}&mobile={adminFPMobNum}&process={process}");//OtpVerification
                if (verOtpAtmLeftsAdmForgotPass >= 1)
                {
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        verOtpAtmLeftsAdmForgotPass--;

                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        //string successMsg = JsonConvert.DeserializeObject<string>(successObj.Result.ToString());
                        return new JsonResult(new { id = "1", message = successObj.Result.ToString() });
                    }
                    else if (responseHttp.StatusCode == HttpStatusCode.NotFound)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var failObj = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(responseString);

                        return new JsonResult(new { id = "0", message = failObj.Errors.FirstOrDefault().ToString() });
                        // return new JsonResult(new { id = "0", message = failObj.Message });
                    }
                    else if (responseHttp.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var failObj = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(responseString);

                        return new JsonResult(new { id = "0", message = failObj.Message });
                    }
                }
                return new JsonResult(new { id = "2", message = "You have exceeded maximum number of attempts for OTP verification." });
            }

            if (process == "ReviewE" && otp != null)
            {
                RegistrationDTO det = new()
                {
                    Otp = otp,
                    MobileNum = mobileNo,
                    Process = process
                };
                var content = new StringContent(JsonConvert.SerializeObject(det), Encoding.UTF8, "application/json");
                var responseHttp = await client.PostAsync($"EnquiryHome/VerOtpForReview", content);

                if (valOtpAttempsLeft >= 1)
                {
                    valOtpAttempsLeft--;
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);

                        return new JsonResult(new { Id = "1", Message = successObj.Result.ToString() });
                    }
                    else if (responseHttp.StatusCode == HttpStatusCode.NotFound)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var failObj = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(responseString);
                        // var failObj = JsonConvert.DeserializeObject<NotFoundObjectResult>(responseString);
                        // var errorMsg = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(failObj.ToString());
                        return new JsonResult(new { Id = "0", Message = failObj.Errors.FirstOrDefault().ToString() });
                    }
                }
                else
                    return new JsonResult(new { Id = "2", Message = "You have exceeded maximum number of attempts for OTP verification." });
            }
            return null;
        }

        /// <summary>
        /// Sets the number of attempts from environment configuration file for Customer Login.
        /// </summary>
        /// <returns></returns>
        public bool GetOtpAttemptsCustomer()
        {
            //Sets the values to default when the page loads.
            genOtpAttemptsCustLogin = Convert.ToInt32(_configure["SysConfig:GenOtpAttemptsCustLogin"]);
            verOtpAtmLeftCustLogin = Convert.ToInt32(_configure["SysConfig:VerOtpAtmLeftCustLogin"]);
            otpExpTimeConfig = Convert.ToInt32(_configure["SysConfig:OtpExpiryTimeConfig"]);
            custMobNum = string.Empty;
            return true;
        }


        /// <summary>
        /// Sets the number of attempts from environment configuration file for Admin Forgot password page.
        /// </summary>
        /// <returns></returns>
        public bool GetOtpAttemptsFP()
        {
            //Sets the values to default when the page loads.
            resOtpAttemptsAdmForgotPass = Convert.ToInt32(_configure["SysConfig:resOtpAttemptsAdmForgotPass"]);
            verOtpAtmLeftsAdmForgotPass = Convert.ToInt32(_configure["SysConfig:verOtpAtmLeftsAdmForgotPass"]);
            otpExpTimeConfig = Convert.ToInt32(_configure["SysConfig:OtpExpiryTimeConfig"]);
            return true;
        }
    }
}
