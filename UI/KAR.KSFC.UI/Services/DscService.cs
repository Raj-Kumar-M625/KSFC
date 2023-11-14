using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services
{
    public class DscService : IDscService
    {
        private static int verDSCAttemptsAdminLogin;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configure;

        public DscService(IConfiguration configure, IHttpClientFactory clientFactory)
        {
            _configure = configure;
            _clientFactory = clientFactory;
            verDSCAttemptsAdminLogin = Convert.ToInt32(_configure["SysConfig:verDSCAttemptsAdminLogin"]);
        }

        /// <summary>
        /// Resets the attempts for Admin DSC verification
        /// </summary>
        /// <returns></returns>
        public bool GetDscAttempts()
        {
            verDSCAttemptsAdminLogin = Convert.ToInt32(_configure["SysConfig:verDSCAttemptsAdminLogin"]);
            return true;
        }

        /// <summary>
        /// Verifies Admin remote IP and decides if DSC verification is required
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public async Task<bool> IsDscRequired(string ip)
        {
            bool isDSCRequiredConfig = Convert.ToBoolean(_configure["SysConfig:IsDSCRequiredAdminLogin"]);
            if (isDSCRequiredConfig)
            {
                EmployeeLoginDTO info = new() { Ip = ip };
                var content = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json");
                var client = _clientFactory.CreateClient("ksfcApi");
                var responseHttp = await client.PostAsync("Account/IsKswanRange", content);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseString = await responseHttp.Content.ReadAsStringAsync();
                    var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                    bool isIpOfKswan = Convert.ToBoolean(successObj.Result);
                    if (isIpOfKswan)
                        return false;
                    else
                        return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Verifies DSC details for Admin Login when Admin logs in with an IP outside KSWAN
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="empId"></param>
        /// <returns></returns>
        public async Task<JsonResult> VerifyDsc(string publicKey, string empId)
        {
            EmployeeLoginDTO info = new() { PublicKey = publicKey, EmpId = empId };
            var content = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json");
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.PostAsync($"Account/DSCVerification", content);

            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                return new JsonResult((id: "1", message: "DSC verification successful."));
            }
            return new JsonResult((id: "0", message: "DSC verification failed."));
        }
    }
}
