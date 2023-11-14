using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.Disbursement;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IDisbursementService;
using KAR.KSFC.UI.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.Admin.IDM.DisbursementService
{
    public class DisbursementService : IDisbursementService
    {
        private readonly IHttpClientFactory _clientFactory;
      
        private readonly ILogger _logger;
        public DisbursementService(IHttpClientFactory clientFactory, ILogger logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<List<LoanAccountNumberDTO>> GetAllLoanNumber()
        {
            try
            {
                _logger.Information(Constants.GetAllLoanNumber);
                List<LoanAccountNumberDTO> listAccNo = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(DisbursementRoute.GetAccountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    listAccNo = JsonConvert.DeserializeObject<List<LoanAccountNumberDTO>>(result.Result.ToString());
                }
                return listAccNo;
            }
           catch (Exception ex)
           {
                _logger.Error(Error.getLoanAccount + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
           }
        }
        #region Disbursement Condition
        //<summary>
        // Author: Manoj; Module:Disbursement Condition; Date:17/08/2022
        //Modified: Gagana K Added try catch with Loggings Date:20/10/2022
        //</summary
        public async Task<IEnumerable<LDConditionDetailsDTO>> GetAllDisbursementConditionList(long accountNumber)
        {   
            try
            {
               _logger.Information(Constants.GetAllDisbursementConditionList);
                List<LDConditionDetailsDTO> listConditionDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(DisbursementRoute.GetAllConditionList + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listConditionDetails = JsonConvert.DeserializeObject<List<LDConditionDetailsDTO>>(Result.Result.ToString());
                }
                return listConditionDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllDisbursementConditionList + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> UpdateDisbursementConditionDetails(LDConditionDetailsDTO addr)
        {
            try
            {
                _logger.Information(Constants.UpdateDisbursementConditionDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(DisbursementRoute.UpdateDisbursementConditionDetails, httpContent);
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
                _logger.Error(Error.updateDisbursement + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }


        public async Task<bool> DeleteDisbursementConditionDetails(LDConditionDetailsDTO dto)
        {
            try
            {
                _logger.Information(Constants.DeleteDisbursementConditionDetails);

                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(DisbursementRoute.DeleteDisbursementConditionDetails, httpContent);
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
                _logger.Error(Error.deleteDisbursement + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> CreateDisbursementConditionDetails(LDConditionDetailsDTO addr)
        {
            try
            {
                _logger.Information(Constants.CreateDisbursementConditionDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(DisbursementRoute.CreateDisbursementConditionDetails, httpContent);
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
                _logger.Error(Error.createDisbursement + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        #endregion

        #region Sidbi Approval
        public async Task<IdmSidbiApprovalDTO> GetAllSidbiApprovalDetails(long? accountNumber)
        {
            try
            {
                _logger.Information(Constants.GetAllSidbiApprovalDetails);
                IdmSidbiApprovalDTO SidbiDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(DisbursementRoute.GetAllSidbiApprovalDetails + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    if (Result.Result != null)
                    {
                        SidbiDetails = JsonConvert.DeserializeObject<IdmSidbiApprovalDTO>(Result.Result.ToString());
                    }
                    else
                    {
                        SidbiDetails = new IdmSidbiApprovalDTO();
                    }

                }
                return SidbiDetails;
            }
           catch(Exception ex)
            {
                _logger.Error(Error.GetAllSidbiApprovalDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> UpdateSidbiApprovalDetails(IdmSidbiApprovalDTO addr)
        {
            try
            {
                _logger.Information(Constants.UpdateSidbiApprovalDetails);
            
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(DisbursementRoute.UpdateSidbiApprovalDetails, httpContent);
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
                _logger.Error(Error.updatesidbiapproval + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        #endregion

        #region Form 8 And Form 13
        public async Task<IEnumerable<Form8AndForm13DTO>> GetAllForm8AndForm13List(long accountNumber)
        {
            try
            {
                _logger.Information(Constants.GetAllForm8AndForm13List);
                List<Form8AndForm13DTO> listForm8AndForm13Details = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(DisbursementRoute.GetAllForm8AndForm13List + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listForm8AndForm13Details = JsonConvert.DeserializeObject<List<Form8AndForm13DTO>>(Result.Result.ToString());
                }
                return listForm8AndForm13Details;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllForm8AndForm13List + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> UpdateForm8AndForm13Details(Form8AndForm13DTO addr)
        {
            try
            {
                _logger.Information(Constants.UpdateForm8AndForm13Details);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(DisbursementRoute.UpdateForm8AndForm13Details, httpContent);
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
                _logger.Error(Error.updateForm8AndForm13 + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> DeleteForm8AndForm13Details(Form8AndForm13DTO dto)
        {
            try
            {
                _logger.Information(Constants.DeleteForm8AndForm13Details);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(DisbursementRoute.DeleteForm8AndForm13Details, httpContent);
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
                _logger.Error(Error.deleteForm8AndForm13 + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
            
        }

        public async Task<bool> CreateForm8AndForm13Details(Form8AndForm13DTO addr)
        {
            try
            {
                _logger.Information(Constants.CreateForm8AndForm13Details);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(DisbursementRoute.CreateForm8AndForm13Details, httpContent);
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
                _logger.Error(Error.createForm8AndForm13 + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region Additional Condition
        public async Task<IEnumerable<AdditionConditionDetailsDTO>> GetAllAdditionalConditonlist(long accountNumber)
        {
            try
            {
                _logger.Information(Constants.GetAllAdditionalConditonlist);
                List<AdditionConditionDetailsDTO> listConditionDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(DisbursementRoute.GetAllAdditionalCondition + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listConditionDetails = JsonConvert.DeserializeObject<List<AdditionConditionDetailsDTO>>(Result.Result.ToString());
                }
                return listConditionDetails;
            }
           catch (Exception ex)
            {
                _logger.Error(Error.GetAllAdditionalConditonlist + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }


        public async Task<bool> CreateAdditionalConditionDetails(AdditionConditionDetailsDTO addr)
        {
            try
            {
                _logger.Information(Constants.CreateAdditionalConditionDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(DisbursementRoute.CreateAdditionalConditionDetails, httpContent);
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
                _logger.Error(Error.createAdditionalCondtional + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
            
        }


        public async Task<bool> UpdateAdditionalConditionDetails(AdditionConditionDetailsDTO addr)
        {
            try
            {
                _logger.Information(Constants.UpdateAdditionalConditionDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(DisbursementRoute.UpdateAdditionalConditionDetails, httpContent);
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
                _logger.Error(Error.updateAdditionalCondtional + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> DeleteAdditionalConditionDetails(AdditionConditionDetailsDTO dto)
        {
            try
            {
                _logger.Information(Constants.DeleteAdditionalConditionDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(DisbursementRoute.DeleteAdditionalConditionDetails, httpContent);
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
                _logger.Error(Error.deleteAdditionalCondtional + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region FirstInvestmentClause
        public async Task<IdmFirstInvestmentClauseDTO> GetAllFirstInvestmentClauseDetails(long? accountNumber)
        {
            try
            {
                _logger.Information(Constants.GetAllFirstInvestmentClauseDetails);
                IdmFirstInvestmentClauseDTO FICDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(DisbursementRoute.GetAllFirstInvestmentClauseDetails + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    if (Result.Result != null)
                    {
                        FICDetails = JsonConvert.DeserializeObject<IdmFirstInvestmentClauseDTO>(Result.Result.ToString());
                    }
                    else
                    {
                        FICDetails = new IdmFirstInvestmentClauseDTO();
                    }

                }
                return FICDetails;
            }
           catch (Exception ex)
            {
                _logger.Error(Error.GetAllFirstInvestmentClauseDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> UpdateFirstInvestmentClauseDetails(IdmFirstInvestmentClauseDTO addr)
        {
            try
            {
                _logger.Information(Constants.UpdateFirstInvestmentClauseDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(DisbursementRoute.UpdateFirstInvestmentClauseDetails, httpContent);
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
                _logger.Error(Error.updateFirstInvestmentClause + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        #endregion

        #region Other Relaxation
        public async Task<IEnumerable<RelaxationDTO>> GetAllOtherRelaxation(long accountNumber)
        {
            try
            {
                _logger.Information(Constants.GetAllOtherRelaxation);
                List<RelaxationDTO> listRelaxationDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(DisbursementRoute.GetAllOtherRelaxation + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listRelaxationDetails = JsonConvert.DeserializeObject<List<RelaxationDTO>>(Result.Result.ToString());
                }
                return listRelaxationDetails;
            }
           catch(Exception ex)
            {
                _logger.Error(Error.GetAllOtherRelaxation + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        public async Task<List<RelaxationDTO>> UpdateOtherRelaxation(List<RelaxationDTO> addr)
        {
            try
            {
                _logger.Information(Constants.UpdateOtherRelaxation);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(DisbursementRoute.UpdateOtherRelaxation, httpContent);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrCD = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrCD);
                    List<RelaxationDTO> relax = JsonConvert.DeserializeObject<List<RelaxationDTO>>(Result.Result.ToString());
                    return relax;
                }
                return addr;
            }
            catch(Exception ex)
            {
                _logger.Error(Error.UpdateOtherRelaxation + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        #endregion
    }
}