using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.UI.Services.IServices;
using KAR.KSFC.UI.Utility;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services
{
    public class PanService : IPanService
    {

        private static int valPanAttempsLeft;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configure;
        private readonly SessionManager _sessionManager;

        public PanService(IConfiguration configure, IHttpClientFactory clientFactory, SessionManager sessionManager)
        {
            _configure = configure;
            _clientFactory = clientFactory;
            _sessionManager = sessionManager;
        }

        /// <summary>
        /// Resets attemps for PAN verification for customer
        /// </summary>
        /// <returns></returns>
        public bool GetPanAttempts()
        {
            valPanAttempsLeft = Convert.ToInt32(_configure["SysConfig:ValPanAttempsLeft"]);
            return true;
        }

        /// <summary>
        /// Gets the fourth char of PAN as per the selected Constitution type
        /// </summary>
        /// <param name="constitutionName"></param>
        /// <returns></returns>
        public string GetPanFourthCharByConstitution(string constitutionName)
        {
            //sessionConstitutionTypeInfo(apiResponse);
            var allConstitutions = _sessionManager.GetAllConstitutionTypes();
            string fourthChar = allConstitutions.Find(c => c.CnstDets == constitutionName).CnstPanchar;


            //string fourthChar = constitutionName.Trim() switch
            //{
            //    "Sole Proprietor" => "P",
            //    "Private Limited Company" => "C",
            //    "Public Limited Company" => "C",
            //    "Partnership" => "F",
            //    "Society" => "J",
            //    "Individual" => "P",
            //    "Hindu Undivided Family" => "H",
            //    "Trust" => "T",
            //    _ => "",
            //};
            return fourthChar;
        }

        /// <summary>
        /// Verifies PAN with contitution type and checks if PAN is already present in DB
        /// </summary>
        /// <param name="panNo"></param>
        /// <param name="constitutionName"></param>
        /// <param name="mobileNum"></param>
        /// <returns></returns>
        public async Task<JsonResult> VerifyPanWithConstTypeAndDb(string panNo, string constitutionName, string mobileNum)
        {
            if (valPanAttempsLeft >= 1)
            {
                valPanAttempsLeft--;
                if (panNo.ToUpper().Substring(3, 1) == GetPanFourthCharByConstitution(constitutionName))
                {
                    RegistrationDTO det = new()
                    {
                        PanNum = panNo,
                        MobileNum = mobileNum,
                        Process = ProcessEnum.PReg
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(det), Encoding.UTF8, "application/json");
                    var client = _clientFactory.CreateClient("ksfcApi");
                    var responseHttp = await client.PostAsync($"Account/PanVerification", content);


                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        return new JsonResult(new { Id = "1", Message = successObj.Result.ToString() });
                    }
                    else if (responseHttp.StatusCode == HttpStatusCode.NotFound)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var failObj = JsonConvert.DeserializeObject<string>(responseString);
                        return new JsonResult(new { Id = "0", Message = responseString });
                    }
                    else// if (responseHttp.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var responseString = await responseHttp.Content.ReadAsStringAsync();
                        var failObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        // var failResponse = JsonConvert.DeserializeObject<ApiResponse>(failObj.Value.ToString());
                        return new JsonResult(new { Id = "0", Message = failObj.Message });
                    }
                }
                else
                {
                    return new JsonResult(new { id = "0", message = "Please enter valid PAN as per selected Constitution Type" });
                }
            }
            else
            {
                return new JsonResult(new { id = "2", message = "You have exceeded maximum number of attempts for PAN validation." });
            }
        }

        public CnstCdtab GetConstitutionByPanNumber()
        {
            var panNo = _sessionManager.GetLoginCustUserName();
            var AllConsitutionTypes = _sessionManager.GetAllConstitutionTypes();
            var fourthChar = panNo.ToUpper().Substring(3, 1);
            return AllConsitutionTypes.FirstOrDefault(c => c.CnstPanchar == fourthChar);
             
        }
        public string GetPanFourthCharByUserName()
        {
            var panNo = _sessionManager.GetLoginCustUserName();
           return panNo.ToUpper().Substring(3, 1);
             

        }
    }
}
