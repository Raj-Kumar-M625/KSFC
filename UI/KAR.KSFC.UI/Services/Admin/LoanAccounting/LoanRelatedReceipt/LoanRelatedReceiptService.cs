using KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.UI.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.Admin.LoanAccounting.LoanRelatedReceipt
{
    public class LoanRelatedReceiptService : ILoanRelatedReceiptService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly SessionManager _sessionManager;
        private readonly ILogger _logger;
        public LoanRelatedReceiptService(IHttpClientFactory clientFactory, SessionManager sessionManager, ILogger logger)
        {
            _clientFactory = clientFactory;
            _sessionManager = sessionManager;
            _logger = logger;
        }

        public async Task<IEnumerable<TblLaReceiptPaymentDetDTO>> GetAllReceiptPaymentList(long? accountNumber)
        {
            try
            {
                List<TblLaReceiptPaymentDetDTO> listSavedReceiptDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(LoanRelatedReceiptRoute.GetAllReceiptPaymentList + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listSavedReceiptDetails = JsonConvert.DeserializeObject<List<TblLaReceiptPaymentDetDTO>>(Result.Result.ToString());
                }
                return listSavedReceiptDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.AllReceiptPaymentList + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
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

        public async Task<bool> UpdateReceiptPaymentDetails(TblLaReceiptPaymentDetDTO addr)
        {
            try
            {
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(LoanRelatedReceiptRoute.UpdateReceiptPaymentDetails, httpContent);
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
                _logger.Error(Error.UpdateReceiptPaymentDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> UpdateCreatePaymentDetails(List<TblLaReceiptPaymentDetDTO> addr)
        {
            try
            {
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(LoanRelatedReceiptRoute.UpdateCreatePaymentDetails, httpContent);
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
        public async Task<bool> DeleteReceiptPaymentDetails(TblLaReceiptPaymentDetDTO addr)
        {
            try
            {
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(LoanRelatedReceiptRoute.DeleteReceiptPaymentDetails, httpContent);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrCD = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrCD);

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.DeleteReceiptPaymentDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> ApproveReceiptPaymentDetails(TblLaReceiptPaymentDetDTO addr)
        {
            try
            {
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(LoanRelatedReceiptRoute.ApproveReceiptPaymentDetails, httpContent);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrCD = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrCD);

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.ApproveReceiptPaymentDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> RejectReceiptPaymentDetails(TblLaReceiptPaymentDetDTO addr)
        {
            try
            {
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(LoanRelatedReceiptRoute.RejectReceiptPaymentDetails, httpContent);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrCD = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrCD);

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.RejectReceiptPaymentDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<IEnumerable<TblLaReceiptDetDTO>> GetAllReceiptList()
        {
            try
            {
                List<TblLaReceiptDetDTO> listSavedReceiptDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(LoanRelatedReceiptRoute.GetAllReceiptList);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listSavedReceiptDetails = JsonConvert.DeserializeObject<List<TblLaReceiptDetDTO>>(Result.Result.ToString());
                }
                return listSavedReceiptDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllReceiptList + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<IEnumerable<TblLaPaymentDetDTO>> GetAllPaymentList()
        {
            try
            {

                List<TblLaPaymentDetDTO> listSavedPaymentDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(LoanRelatedReceiptRoute.GetAllPaymentList);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listSavedPaymentDetails = JsonConvert.DeserializeObject<List<TblLaPaymentDetDTO>>(Result.Result.ToString());
                }
                return listSavedPaymentDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllPaymentList + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> CreateReceiptPaymentDetails(TblLaReceiptPaymentDetDTO addr)
        {
            try
            {
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(LoanRelatedReceiptRoute.CreateReceiptPaymentDetails, httpContent);
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
                _logger.Error(Error.CreateReceiptPaymentDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> CreatePaymentDetails(TblLaPaymentDetDTO payment)
        {
            try
            {
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(LoanRelatedReceiptRoute.CreatePaymentDetails, httpContent);
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
                _logger.Error(Error.CreatePaymentDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<IEnumerable<TblLaReceiptPaymentDetDTO>> GetAllRecipetsForPayment(int PaymnetId)
        {
            try
            {
                List<TblLaReceiptPaymentDetDTO> listSavedReceiptDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(LoanRelatedReceiptRoute.GetAllReceiptPayments + PaymnetId);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listSavedReceiptDetails = JsonConvert.DeserializeObject<List<TblLaReceiptPaymentDetDTO>>(Result.Result.ToString());
                }
                return listSavedReceiptDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllRecipetsForPayment + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
    }
}
