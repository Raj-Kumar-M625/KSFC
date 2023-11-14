using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.ICreationOfDisbursmentProposalService;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfDisbursmentProposal;
using KAR.KSFC.UI.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Threading;
using KAR.KSFC.Components.Data.Models.DbModels;
using System;
using KAR.KSFC.Components.Common.Logging.Client;

namespace KAR.KSFC.UI.Services.Admin.IDM.CreationOfDisbursmentProposal
{
    public class CreationOfDisbursmentPropsalService :ICreationOfDisbursmentProposalService
    {
        private readonly IHttpClientFactory _clientFactory;
      
        private readonly ILogger _logger;

        public CreationOfDisbursmentPropsalService(IHttpClientFactory clientFactory,ILogger logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        //#region Recommended Disbursement Details
        //public async Task<IEnumerable<IdmDsbdetsDTO>> GetAllRecomDisbursementDetails(long accountNumber)
        //{
        //    try
        //    {
        //        _logger.Information(Constants.GetAllRecomDisbursementDetails);
        //        List<IdmDsbdetsDTO> listRecomDisbursementDetails = new();
        //        var client = _clientFactory.CreateClient("ksfcApi");
        //        var responseHttp = await client.GetAsync(CreationOfDisbursmentProposalAssetRoute.GetAllRecomDisburseDetails + accountNumber);
        //        if (responseHttp.StatusCode == HttpStatusCode.OK)
        //        {
        //            var apiResponse = await responseHttp.Content.ReadAsStringAsync();
        //            var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
        //            listRecomDisbursementDetails = JsonConvert.DeserializeObject<List<IdmDsbdetsDTO>>(result.Result.ToString());
        //        }
        //        return listRecomDisbursementDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(Error.GetAllRecomDisbursementDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        throw;
        //    }

        //}
        //public async Task<IEnumerable<TblAllcCdTabDTO>> GetAllocationCodeDetails()
        //{
        //    try
        //    {
        //        _logger.Information(Constants.GetAllocationCodeDetails);
        //        List<TblAllcCdTabDTO> listAllocationDetails = new();
        //        var client = _clientFactory.CreateClient("ksfcApi");
        //        var responseHttp = await client.GetAsync(CreationOfDisbursmentProposalAssetRoute.GetAllocationDetails);
        //        if (responseHttp.StatusCode == HttpStatusCode.OK)
        //        {
        //            var apiResponse = await responseHttp.Content.ReadAsStringAsync();
        //            var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
        //            listAllocationDetails = JsonConvert.DeserializeObject<List<TblAllcCdTabDTO>>(result.Result.ToString());
        //        }
        //        return listAllocationDetails;
        //    }
        //  catch (Exception ex)
        //    {
        //        _logger.Error(Error.GetAllocationCodeDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        throw;
        //    }
        //}

        //public async Task<IEnumerable<TblIdmReleDetlsDTO>> GetRecomDisbursementReleaseDetails()
        //{
        //    try
        //    {
        //        _logger.Information(Constants.GetRecomDisbursementReleaseDetails);
        //        List<TblIdmReleDetlsDTO> listRecomDisbursementReleaseDetails = new();
        //        var client = _clientFactory.CreateClient("ksfcApi");
        //        var responseHttp = await client.GetAsync(CreationOfDisbursmentProposalAssetRoute.GetRecomDisbursementReleaseDetails);
        //        if (responseHttp.StatusCode == HttpStatusCode.OK)
        //        {
        //            var apiResponse = await responseHttp.Content.ReadAsStringAsync();
        //            var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
        //            listRecomDisbursementReleaseDetails = JsonConvert.DeserializeObject<List<TblIdmReleDetlsDTO>>(result.Result.ToString());
        //        }
        //        return listRecomDisbursementReleaseDetails;
        //    }
        //    catch(Exception ex)
        //    {
        //        _logger.Error(Error.GetRecomDisbursementReleaseDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        throw;
        //    }
           
        //}
        
        //public async Task<bool> UpdateRecomDisbursementDetail(IdmDsbdetsDTO idmDsbdetsDTO)
        //{
        //    try
        //    {
        //        _logger.Information(Constants.UpdateRecomDisbursementDetail);
        //        var client = _clientFactory.CreateClient("ksfcApi");
        //        var httpContent = new StringContent(JsonConvert.SerializeObject(idmDsbdetsDTO), Encoding.UTF8, "application/json");
        //        var responseHttp = await client.PostAsync(CreationOfDisbursmentProposalAssetRoute.UpdateRecomDisburseDetail, httpContent);
        //        if (responseHttp.StatusCode == HttpStatusCode.OK)
        //        {
        //            var responseStrCD = await responseHttp.Content.ReadAsStringAsync();
        //           JsonConvert.DeserializeObject<ApiResultResponse>(responseStrCD);
                   
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch(Exception ex)
        //    {
        //        _logger.Error(Error.UpdateRecomDisbursementDetail + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        throw;
        //    }
           
        //}

        //#endregion

        #region disbursement proposal details
        public async Task<IEnumerable<TblIdmReleDetlsDTO>> GetAllProposalDetails(long? accountNumber)
        {
            try
            {
                _logger.Information(Constants.GetAllProposalDetails);
                List<TblIdmReleDetlsDTO> proposalDetails = new();
                var client = _clientFactory.CreateClient("ksfcApi");
                var responseHttp = await client.GetAsync(CreationOfDisbursmentProposalAssetRoute.GetAllProposalDetails + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    proposalDetails = JsonConvert.DeserializeObject<List<TblIdmReleDetlsDTO>>(result.Result.ToString());
                }
                return proposalDetails;
            }
           catch (Exception ex)
            {
                _logger.Error(Error.GetAllProposalDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        public async Task<bool> CreateProposalDetail(TblIdmReleDetlsDTO tblIdmReleDetlsdto)
        {
            try
            {
                _logger.Information(Constants.CreateProposalDetail);
                var client = _clientFactory.CreateClient("ksfcApi");
                var httpContent = new StringContent(JsonConvert.SerializeObject(tblIdmReleDetlsdto), Encoding.UTF8, "application/json");
                var responseHttp = await client.PostAsync(CreationOfDisbursmentProposalAssetRoute.CreateProposalDetail, httpContent);
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
                _logger.Error(Error.CreateProposalDetail + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> UpdateProposalDetail(TblIdmReleDetlsDTO tblIdmReleDetlsdto)
        {
            try
            {
                _logger.Information(Constants.UpdateProposalDetail);
                var client = _clientFactory.CreateClient("ksfcApi");
                var httpContent = new StringContent(JsonConvert.SerializeObject(tblIdmReleDetlsdto), Encoding.UTF8, "application/json");
                var responseHttp = await client.PostAsync(CreationOfDisbursmentProposalAssetRoute.UpdateProposalDetail, httpContent);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrCD = await responseHttp.Content.ReadAsStringAsync();
                    JsonConvert.DeserializeObject<ApiResultResponse>(responseStrCD);
                    
                    return true;
                }
                return false;
            }
           catch(Exception ex)
            {
                _logger.Error(Error.UpdateProposalDetail + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> DeleteProposalDetail(TblIdmReleDetlsDTO tblIdmReleDetlsdto)
        {
            try
            {
                _logger.Information(Constants.DeleteProposalDetail);
                var client = _clientFactory.CreateClient("ksfcApi");
                var httpContent = new StringContent(JsonConvert.SerializeObject(tblIdmReleDetlsdto), Encoding.UTF8, "application/json");
                var responseHttp = await client.PostAsync(CreationOfDisbursmentProposalAssetRoute.DeleteProposalDetail, httpContent);
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
                _logger.Error(Error.DeleteProposalDetail + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
            
        }

        #endregion

        #region Beneficiary Details
        public async Task<IEnumerable<TblIdmBenfDetDTO>> GetAllBeneficiaryDetails(long accountNumber)
        {
            try
            {
                _logger.Information(Constants.GetAllBeneficiaryDetails);
                List<TblIdmBenfDetDTO> beneficiaryDetails = new();
                var client = _clientFactory.CreateClient("ksfcApi");
                var responseHttp = await client.GetAsync(CreationOfDisbursmentProposalAssetRoute.GetAllBeneficiaryDetails + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);                   
                    beneficiaryDetails = JsonConvert.DeserializeObject<List<TblIdmBenfDetDTO>>(Result.Result.ToString());                   
                }
                return beneficiaryDetails;
            }
           catch (Exception ex)
            {
                _logger.Error(Error.GetAllBeneficiaryDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        public async Task<TblIdmBenfDetDTO> UpdateBeneficiaryDetails(TblIdmBenfDetDTO addr)
        {
            try
            {
                _logger.Information(Constants.UpdateBeneficiaryDetails);
                TblIdmBenfDetDTO beneficiaryDetails = new();
                var client = _clientFactory.CreateClient("ksfcApi");
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, "application/json");
                var responseHttp = await client.PostAsync(CreationOfDisbursmentProposalAssetRoute.UpdateBeneficiaryDetails, httpContent);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrCD = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrCD);
                    beneficiaryDetails = JsonConvert.DeserializeObject<TblIdmBenfDetDTO>(Result.Result.ToString());
                    return beneficiaryDetails;
                }
                return beneficiaryDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.UpdateBeneficiaryDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        #endregion


    }
}
