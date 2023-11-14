using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Models;
using KAR.KSFC.UI.Services.IServices;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services
{
    public class CustomerService : ICustomerService
    {
        
        private readonly IApiService _apiService;
        public CustomerService(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Verifies credentials and lets Customer to login.
        /// </summary>
        /// <param name="custViewModel"></param>
        /// <returns></returns>
        public async Task<CustClaimsModel> Login(CustomerViewModel custViewModel)
        {
            try
            {
                CustLoginDTO info = new()
                {
                    PanNum = custViewModel.PanNumber,
                    Otp = custViewModel.OTP,
                    MobileNum = custViewModel.MobileNumber
                };
               
                var responseHttp = await _apiService.PostAdminAsync("Account/CustomerLogin", info);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    string responseString = await responseHttp.Content.ReadAsStringAsync();
                    var responseObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                    var userClaims = JsonConvert.DeserializeObject<CustClaimsModel>(responseObj.Result.ToString());
                    return userClaims;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Verifies Customer credentials during Customer logout and updates DB
        /// </summary>
        /// <param name="panNo"></param>
        /// <param name="accToken"></param>
        /// <returns></returns>
        public async Task<bool> Logout(string panNo, string accToken)
        {
            CustLoginDTO custDTO = new() { PanNum = panNo };
            var responseHttp = await _apiService.PostAdminAsync("EnquiryHome/CustomerLogout", custDTO);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
                return true;
            return false;
        }
    }
}
