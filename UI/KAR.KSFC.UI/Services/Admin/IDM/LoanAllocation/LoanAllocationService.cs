using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.ILoanAllocationService;
using KAR.KSFC.UI.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.Admin.IDM.LoanAllocation
{
    // <summary>
    // Author: Gagana K; Module: LoanAllocation; Date:26/09/2022

    // </summary>
    public class LoanAllocationService : ILoanAllocationService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _logger;
        public LoanAllocationService(IHttpClientFactory clientFactory, ILogger logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        //#region LoanAllocation
        //public async Task<IEnumerable<TblIdmDhcgAllcDTO>> GetAllLoanAllocationList(long accountNumber)
        //{
        //    try
        //    {
        //        _logger.Information(CommonLogHelpers.GetAllLoanAllocationList);
        //        List<TblIdmDhcgAllcDTO> listLoanAllocation = new();
        //        var client = _clientFactory.CreateClient(Constants.client);
        //        var responseHttp = await client.GetAsync(LoanAllocationRoute.GetAllLoanAllocationList + accountNumber);
        //        if (responseHttp.StatusCode == HttpStatusCode.OK)
        //        {
        //            var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
        //            var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
        //            listLoanAllocation = JsonConvert.DeserializeObject<List<TblIdmDhcgAllcDTO>>(Result.Result.ToString());
        //        }
        //        _logger.Information(CommonLogHelpers.CompletedGetAllLoanAllocationList);
        //        return listLoanAllocation;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(Error.getLoanAllocation + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        throw;
        //    }

        //}
        //public async Task<bool> UpdateLoanAllocationDetails(TblIdmDhcgAllcDTO addr)
        //{
        //    try
        //    {
        //        _logger.Information(CommonLogHelpers.StartedLoanAllocationUpdate);
        //        var client = _clientFactory.CreateClient(Constants.client);
        //        var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
        //        var responseHttp = await client.PostAsync(LoanAllocationRoute.UpdateLoanAllocationDetails, httpContent);
        //        if (responseHttp.StatusCode == HttpStatusCode.OK)
        //        {
        //            var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
        //            JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(Error.updateLoanAllocation + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        throw;
        //    }

        //}
        //public async Task<bool> CreateLoanAllocationDetails(TblIdmDhcgAllcDTO addr)
        //{
        //    try
        //    {
        //        _logger.Information(CommonLogHelpers.StartedLoanAllocationCreate);
        //        var client = _clientFactory.CreateClient(Constants.client);
        //        var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
        //        var responseHttp = await client.PostAsync(LoanAllocationRoute.CreateLoanAllocationDetails, httpContent);
        //        if (responseHttp.StatusCode == HttpStatusCode.OK)
        //        {
        //            var responseStrCD = await responseHttp.Content.ReadAsStringAsync();
        //            JsonConvert.DeserializeObject<ApiResultResponse>(responseStrCD);
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(Error.createLoanAllocation + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        throw;
        //    }

        //}
        //public async Task<bool> DeleteLoanAllocationDetails(TblIdmDhcgAllcDTO dto)
        //{
        //    try
        //    {
        //        _logger.Information(CommonLogHelpers.StartedLoanAllocationDelete);
        //        var client = _clientFactory.CreateClient(Constants.client);
        //        var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
        //        var responseHttp = await client.PostAsync(LoanAllocationRoute.DeleteLoanAllocationDetails, httpContent);
        //        if (responseHttp.StatusCode == HttpStatusCode.OK)
        //        {
        //            var responseStrCD = await responseHttp.Content.ReadAsStringAsync();
        //            JsonConvert.DeserializeObject<ApiResultResponse>(responseStrCD);
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(Error.deleteLoanAllocation + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        throw;
        //    }
        //}
        //#endregion
    }
}
