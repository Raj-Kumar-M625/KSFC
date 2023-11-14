using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IEntryOfOtherDebits;
using KAR.KSFC.UI.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.Admin.IDM.EntryOfOtherDebits
{
    public class EntryOfOtherDebitsService : IEntryOfOtherDebitsService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _logger;
        public EntryOfOtherDebitsService(IHttpClientFactory clientFactory, ILogger logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }
        #region Other Debits
        public async Task<IEnumerable<IdmOthdebitsDetailsDTO>> GetAllOtherDebitsList(long accountNumber)
        {
            try
            {
                _logger.Information(Constants.GetAllOtherDebitsList);
                List<IdmOthdebitsDetailsDTO> listOtherDebitDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(OtherDebits.GetAllOtherDebitsList + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listOtherDebitDetails = JsonConvert.DeserializeObject<List<IdmOthdebitsDetailsDTO>>(Result.Result.ToString());
                }
                return listOtherDebitDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.getAllOtherDebitsList + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        public async Task<bool> UpdateOtherDebitDetails(IdmOthdebitsDetailsDTO othdebit)
        {
            try
            {
                _logger.Information(Constants.UpdateOtherDebitDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(othdebit), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(OtherDebits.UpdateOtherDebitDetails, httpContent);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.updateOtherDebit + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        public async Task<bool> DeleteOtherDebitDetails(IdmOthdebitsDetailsDTO othdebit)
        {
            try
            {
                _logger.Information(Constants.DeleteOtherDebitDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(othdebit), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(OtherDebits.DeleteOtherDebitDetails, httpContent);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.deletedOtherDebit + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        public async Task<bool> CreateOtherDebitDetails(IdmOthdebitsDetailsDTO othdebit)
        {
            try
            {
                _logger.Information(Constants.CreateOtherDebitDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(othdebit), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(OtherDebits.CreateOtherDebitDetails, httpContent);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrCD = await responseHttp.Content.ReadAsStringAsync();
                    JsonConvert.DeserializeObject<ApiResultResponse>(responseStrCD);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.createOtherDebit + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        public async Task<bool> SubmitOtherDebitDetails(IdmOthdebitsDetailsDTO othdebit)
        {
            try
            {
                _logger.Information(Constants.SubmitOtherDebitDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(othdebit), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(OtherDebits.SubmitOtherDebitDetails, httpContent);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrCD = await responseHttp.Content.ReadAsStringAsync();
                    JsonConvert.DeserializeObject<ApiResultResponse>(responseStrCD);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.SubmitOtherDebit + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        #endregion

    }
}
