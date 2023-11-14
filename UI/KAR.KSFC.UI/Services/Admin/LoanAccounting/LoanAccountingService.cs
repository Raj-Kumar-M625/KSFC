using KAR.KSFC.Components.Common.Dto.Common;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.LoanAccounting;
using KAR.KSFC.UI.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.Admin.LoanAccounting
{
    public class LoanAccountingService : ILoanAccountingService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly SessionManager _sessionManager;
        public LoanAccountingService(IHttpClientFactory clientFactory, SessionManager sessionManager)
        {
            _clientFactory = clientFactory;
            _sessionManager = sessionManager;
        }

        public async Task<List<LoanAccountNumberDTO>> GetAllAccountingLoanNumber(string empID)
        {
            List<LoanAccountNumberDTO> listAccNo = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(LoanRelatedReceiptRoute.GetAllAccountingLoanNumber+empID);
          
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                listAccNo = JsonConvert.DeserializeObject<List<LoanAccountNumberDTO>>(result.Result.ToString());
            }
            return listAccNo;
        }

        #region Generic Dropdowns
        public async Task<IEnumerable<DropDownDTO>> GetAllTransactionTypes()
        {
            List<DropDownDTO> listTransactionTypes = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(LoanAccountingGenericDDRoute.GetAllTransactionTypes);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                listTransactionTypes = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return listTransactionTypes;
        }
        #endregion
    }
}
