using KAR.KSFC.Components.Common.Dto.Common;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Customer.LoanAccountingPromoter;
using KAR.KSFC.UI.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.Customer.LoanAccounting
{
    public class LoanAccountingPromoterService : ILoanAccountingPromoterService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly SessionManager _sessionManager;
        public LoanAccountingPromoterService(IHttpClientFactory clientFactory, SessionManager sessionManager)
        {
            _clientFactory = clientFactory;
            _sessionManager = sessionManager;
        }

        public async Task<List<LoanAccountNumberDTO>> GetAllAccountingLoanNumber()
        {
            List<LoanAccountNumberDTO> listAccNo = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(LoanRelatedReceiptPromRoute.GetAllAccountingLoanNumber);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                listAccNo = JsonConvert.DeserializeObject<List<LoanAccountNumberDTO>>(result.Result.ToString());
            }
            return listAccNo;
        }

        //#region Generic Dropdowns
        //public async Task<IEnumerable<DropDownDTO>> GetAllTransactionTypes()
        //{
        //    List<DropDownDTO> listAssetType = new();
        //    var client = _clientFactory.CreateClient("ksfcApi");
        //    var responseHttp = await client.GetAsync(GenericDDRoute.GetAllTransactionType);
        //    if (responseHttp.StatusCode == HttpStatusCode.OK)
        //    {
        //        var apiResponse = await responseHttp.Content.ReadAsStringAsync();

        //        var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
        //        listAssetType = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
        //    }
        //    return listAssetType;
        //}
        //#endregion
    }
}
