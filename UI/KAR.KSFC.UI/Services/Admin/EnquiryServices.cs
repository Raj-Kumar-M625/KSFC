using KAR.KSFC.Components.Common.Dto.EnquirySubmission;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Services.IServices;
using KAR.KSFC.UI.Services.IServices.Admin;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.Admin
{
    public class EnquiryServices : IEnquiryService
    {
        private readonly IApiService _apiService;
        

        public EnquiryServices(IApiService apiService)
        {
            _apiService = apiService;
            
        }
        public async Task<List<EnquirySummary>> GetAllEnquiriesForAdmin()
        {
            try
            {
                List<EnquirySummary> enquiriesList = new List<EnquirySummary>();
                var successObj = new ApiResultResponse();
                var responseHttp = await _apiService.GetAdminAsync("EnquiryHome/GetAllEnquriesForAdmin");
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    enquiriesList = JsonConvert.DeserializeObject<List<EnquirySummary>>(successObj.Result.ToString());

                }
                return enquiriesList;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
