using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Models;
using KAR.KSFC.UI.Services.IServices;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace KAR.KSFC.UI.Services
{
    public class AdminService : IAdminService
    {
        private readonly IApiService _apiService;
        public AdminService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// New password is saved to DB after verifying existing credentials for Admin. 
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="token"></param>
        /// <param name="currPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<string> ChangePassword(string empId, string token, string currPassword, string newPassword)
        {
            PasswordChangeDTO info = new() { EmpId = empId, OldPassword = currPassword, NewPassword = newPassword };
            var responseHttp = await _apiService.PostAdminAsync("Home/ChangePassword", info);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                return string.Empty;
            }
            else
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                return responseStr;
            }
        }   

        /// <summary>
        /// Verifies Admin credentials for Login.
        /// </summary>
        /// <param name="empLoginDTO"></param>
        /// <returns></returns>
        public async Task<EmployeeClaimsModel> Login(EmployeeLoginDTO empLoginDTO)
        {

            var responseHttp = await _apiService.PostAdminAsync("Account/AdminLogin", empLoginDTO);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                //Gets user claims from API
                string responseString = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);

                var userClaims = JsonConvert.DeserializeObject<EmployeeClaimsModel>(successObj.Result.ToString());
                return userClaims;
            }
            else if (responseHttp.StatusCode == HttpStatusCode.NotFound || responseHttp.StatusCode == HttpStatusCode.Conflict)
            {
                // Redirecting Admin user to Login page with error message from API
                string errorMessage = await responseHttp.Content.ReadAsStringAsync();
                return new EmployeeClaimsModel() { ErrorMessage = errorMessage };
            }
            return null;
        }

        /// <summary>
        /// Updates DB when Admin logs out
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="adminAccToken"></param>
        /// <returns></returns>
        public async Task<bool> UserLogout(string empId, string adminAccToken)
        {
            EmployeeLoginDTO info = new() { EmpId = empId };
            var response = await _apiService.PostAdminAsync("Home/AdminLogout", info);
            if (response.StatusCode == HttpStatusCode.OK)
                return true;
            return false;
        }

        /// <summary>
        /// Verifies Admin and Sends new password to the registered mobile number for Forgot Password page
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public async Task<JsonResult> ValidateUserSendPassword(string empId)
        {
            EmployeeLoginDTO info = new() { EmpId = empId, Process = "KUPassGen" };

            var responseHttp = await _apiService.PostAdminAsync("Account/ValidateAndSendPassword", info);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseString = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                //string successMsg = JsonConvert.DeserializeObject<string>(successObj.Result.ToString());
                return new JsonResult(new { id = "1", message = successObj.Result.ToString() });
            }
            else if (responseHttp.StatusCode == HttpStatusCode.BadRequest)
            {
                var responseString = await responseHttp.Content.ReadAsStringAsync();
                var failureObj = JsonConvert.DeserializeObject<ApiValidationErrorResponse>(responseString);
                //var failureMsg = JsonConvert.DeserializeObject<string>(failureObj.Errors.ToString());
                return new JsonResult(new { id = "0", message = failureObj.Errors.ToString() });
            }
            return new JsonResult(new { id = "0", message = "User not found" });
        }

        /// <summary>
        /// Calls API to to get new access and refresh tokens when access token gets expired.
        /// </summary>
        /// <param name="accToken"></param>
        /// <param name="refToken"></param>
        /// <returns></returns>
        public async Task<string> Refresh(string accToken, string refToken)
        {
            TokenDTO info = new() { Access_Token = accToken, Refresh_Token = refToken };
            var responseHttp = await _apiService.PostAdminAsync("Account/Refresh", info);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseString = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                return successObj.Result.ToString();
            }
            else if (responseHttp.StatusCode == HttpStatusCode.Unauthorized)
            {
                var responseString = await responseHttp.Content.ReadAsStringAsync();
                var failureObj = JsonConvert.DeserializeObject<ApiException>(responseString);
                return failureObj.Details.ToString();
            }
            return "Invalid Tokens";
        }

    }
}
