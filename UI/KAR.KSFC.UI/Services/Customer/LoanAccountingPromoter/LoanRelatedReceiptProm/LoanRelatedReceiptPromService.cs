using KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Customer.LoanAccountingPromoter.LoanRelatedReceipt;
using KAR.KSFC.UI.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.Customer.LoanAccountingPromoter.LoanRelatedReceipt
{
    public class LoanRelatedReceiptPromService : ILoanRelatedReceiptPromService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly SessionManager _sessionManager;
        private readonly ILogger _logger;

        public LoanRelatedReceiptPromService(IHttpClientFactory clientFactory, SessionManager sessionManager, ILogger logger)
        {
            _clientFactory = clientFactory;
            _sessionManager = sessionManager;
            _logger = logger;
        }

        //<summary>
        // Author: Gagana; Module:LoanRelatedReceipt; Date:15/09/2022
        //</summary
        public async Task<IEnumerable<TblLaReceiptPaymentDetDTO>> GetAllGenerateReceiptPaymentList(long accountNumber)
        {
            try
            {
                List<TblLaReceiptPaymentDetDTO> listReceiptDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(LoanRelatedReceiptPromRoute.GetAllGenerateReceiptPaymentList + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listReceiptDetails = JsonConvert.DeserializeObject<List<TblLaReceiptPaymentDetDTO>>(Result.Result.ToString());
                }
                return listReceiptDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllRecipetsForPayment + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<IEnumerable<CodeTableDTO>> GetCodeTableList(long accountNumber)
        {
            try
            {

                List<CodeTableDTO> listCodetable = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(LoanRelatedReceiptRoute.GetCodetable + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listCodetable = JsonConvert.DeserializeObject<List<CodeTableDTO>>(Result.Result.ToString());
                }
                return listCodetable;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetCodeTableList + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> UpdateCreatePromPaymentDetails(List<TblLaReceiptPaymentDetDTO> addr)
        {
            try
            {
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(LoanRelatedReceiptRoute.UpdateCreatePromPaymentDetails, httpContent);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.UpdateCreatePaymentDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

    }
}
