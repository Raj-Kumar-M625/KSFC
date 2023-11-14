using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IAuditService;
using KAR.KSFC.UI.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.Admin.IDM.AuditService
{
    // <summary>
    //  Author: Gagana K; Module:Audit Clearance ; Date:18/08/2022
    // </summary>
    public class AuditService : IAuditService
    {
        private readonly IHttpClientFactory _clientFactory;
       
        private readonly ILogger _logger;
        public AuditService(IHttpClientFactory clientFactory, ILogger logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        #region AuditClearance

        public async Task<IEnumerable<IdmAuditDetailsDTO>> GetAllAuditClearanceList(long accountNumber)
        {
           try
            {
                List<IdmAuditDetailsDTO> listAuditClearanceDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(AuditRoute.GetAllAuditClearnceList + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listAuditClearanceDetails = JsonConvert.DeserializeObject<List<IdmAuditDetailsDTO>>(Result.Result.ToString());
                }
                return listAuditClearanceDetails;   
            }
            catch (Exception ex)
            {
                _logger.Error(Error.getAuditClearenceList + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        public async Task<bool> UpdateAuditClearanceDetails(IdmAuditDetailsDTO addr)
        {
            try
            {
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(AuditRoute.UpdateAuditClearanceDetails, httpContent);
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
                _logger.Error(Error.updateAuditClearenceList + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        public async Task<bool> CreateAuditClearanceDetails(IdmAuditDetailsDTO addr)
        {
            
            try
            {
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(AuditRoute.CreateAuditClearanceDetails, httpContent);
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
                _logger.Error(Error.createAuditClearenceList + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
            
        }

        public async Task<bool> DeleteAuditClearanceDetails(IdmAuditDetailsDTO dto)
        {
            try
            {
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(AuditRoute.DeleteAuditClearanceDetails, httpContent);
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
                _logger.Error(Error.deleteAuditClearenceList + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        #endregion
    }
}
