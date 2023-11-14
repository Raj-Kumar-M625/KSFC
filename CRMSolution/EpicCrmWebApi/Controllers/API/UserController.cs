using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using EpicCrmWebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace EpicCrmWebApi
{
    public class UserController : CrmBaseController
    {
        [HttpGet]
        public JsonResult<IEnumerable<long>> GetActiveEmployeesOnIMEI(string IMEI = "")
        {
            return Json(Business.GetActiveEmployees(IMEI));
        }

        [HttpPost]
        public JsonResult<UserDataResponse> RegisterUser(RegisterUserRequest registerRequest)
        {
            UserDataResponse rur = new UserDataResponse()
            {
                DateTimeUtc = DateTime.UtcNow,
                StatusCode = HttpStatusCode.BadRequest,
                Content = "",
                EmployeeId = -1,
                EmployeeName = "",
                TimeIntervalInMillisecondsForTracking = 0
            };

            if (!ModelState.IsValid)
            {
                LogModelErrors(nameof(UserController));
                rur.Content = "Server: ModelState is invalid";
                return Json(rur);
            }

            if (registerRequest == null)
            {
                Business.LogError(nameof(UserController), $"{nameof(registerRequest)} is null", " ");
                rur.Content = "Server: request parameter is null";
                return Json(rur);
            }

            // validate tenant Id;
            IEnumerable<Tenant> tenants = Business.GetTenants();
            if (tenants.Any(t => t.Id == Utils.SiteConfigData.TenantId) == false)
            {
                Business.LogError(nameof(UserController), $"Config Tenant Id {Utils.SiteConfigData.TenantId} is Invalid.", " ");
                rur.Content = "Invalid Tenant specified in configuration.";
                return Json(rur);
            }
            
            // is phone number associated to employee code 
            if (!Business.IsPhoneValidForEmployeeCode(registerRequest.EmployeeCode, registerRequest.PhoneNumber))
            {
                rur.Content = "Invalid Code/Phone number";
                rur.StatusCode = HttpStatusCode.BadRequest;
                return Json(rur);
            }

            if (!String.IsNullOrEmpty(registerRequest.AppVersion))
            {
                if (!Business.IsAppVersionSupported(registerRequest.AppVersion, Helper.GetCurrentIstDateTime()))
                {
                    rur.StatusCode = HttpStatusCode.BadRequest;
                    rur.Content = $"App version {registerRequest.AppVersion} is not supported.";
                    return Json(rur);
                }
            }

            try
            {
                RegisterUserData userData = new RegisterUserData()
                {
                    IMEI = Utils.TruncateString(registerRequest.IMEI, 50),
                    EmployeeCode = registerRequest.EmployeeCode,
                    At = registerRequest.At,
                    TenantId = Utils.SiteConfigData.TenantId,
                    TimeIntervalInMillisecondsForTracking = Utils.SiteConfigData.TimeIntervalInMillisecondsForTracking
                };

                UserRecord registerResponse = Business.RegisterUser(userData);
                rur.EmployeeId = registerResponse.EmployeeId;
                rur.EmployeeName = registerResponse.EmployeeName;
                rur.StatusCode = HttpStatusCode.OK;
                rur.IsActive = registerResponse.IsActive;
                rur.TimeIntervalInMillisecondsForTracking = registerResponse.TimeIntervalInMillisecondsForTracking;
                rur.Content = registerResponse.Message;

                // send supported locale list in response
                rur.Locales = Business.GetCodeTable("Locale");
            }
            catch(Exception ex)
            {
                Business.LogError(nameof(UserController), ex.ToString(), " ");
                rur.Content = ex.Message;
            }

            return Json(rur);
        }

        [HttpGet]
        public SupportedStatus IsSupported(string employeeCode, string phoneNumber)
        {
            SupportedStatus response = new SupportedStatus()
            {
                IsSupported = false
            };

            try
            {
                response.IsSupported = Business.IsValidUser(employeeCode, phoneNumber);
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(UserController), ex.ToString(), " ");
                response.Content = ex.ToString();
            }

            return response;
        }
    }
}