using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.UI.Models;
using KAR.KSFC.UI.Services.IServices;

using Microsoft.AspNetCore.Mvc.Rendering;
//using KAR.KSFC.UI.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services
{

    public class RegisterService : IRegisterService
    {
        private readonly IHttpClientFactory _clientFactory;
        public RegisterService( IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Gets all contitution types available in DB for Customer registration page
        /// </summary>
        /// <returns></returns>
        public async Task<RegisterViewModel> GetDDLConstitutionTypes()
        {
            try
            {
                RegisterViewModel registerViewModel = new();
                var client = _clientFactory.CreateClient("ksfcApi");
                var responseHttp = await client.GetAsync("Common/GetConstitution");
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    //sessionConstitutionTypeInfo(apiResponse);
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    registerViewModel.ListConstitutionTypes = JsonConvert.DeserializeObject<List<SelectListItem>>(result.Result.ToString());
                }
                else
                    registerViewModel = null;

                return registerViewModel;
            }
            catch(Exception ex) {
                throw ex;
            }
        }

        public async Task<List<CnstCdtab>> GetAllConstitutionTypes()
        {
            List<CnstCdtab> listConstTypes = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync("Common/GetAllConstitutionData");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                //sessionConstitutionTypeInfo(apiResponse);
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                listConstTypes = JsonConvert.DeserializeObject<List<CnstCdtab>>(result.Result.ToString());
            }
            return listConstTypes;
        }

        /// <summary>
        /// Verifies customer details and registers if customer does not exist in DB.
        /// </summary>
        /// <param name="regViewModel"></param>
        /// <returns></returns>
        public async Task<bool> RegisterUser(RegisterViewModel regViewModel)
        {
            var objRegTab = new { UserPan = regViewModel.PanNumber, UserMobileno = regViewModel.MobileNumber };
            //annonymous
            var content = new StringContent(JsonConvert.SerializeObject(objRegTab), Encoding.UTF8, "application/json");
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.PostAsync("Account/SaveRegistration", content);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
                return true;
            else
                return false;
        }
    }
}
