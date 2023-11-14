using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.ILegalDocumentationService;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.Admin.IDM.LegalDocumentationService
{
    public class LegalDocumentationService : ILegalDocumentationService
    {
        private readonly IHttpClientFactory _clientFactory;
       
        private readonly ILogger _logger;

        public LegalDocumentationService(IHttpClientFactory clientFactory,ILogger logger)
        {
            _clientFactory = clientFactory;
        
            _logger = logger;   
        }

        #region Primary Security
        // <summary>
        //  Author: Rajesh; Module: Primary/CollateralSecurity; Date:15/07/2022
        //Modified: Swetha M Added try catch with Loggings Date:17/10/2022
        // </summary>
        public async Task<IEnumerable<IdmSecurityDetailsDTO>> GetAllPrimaryCollateralList(long accountNumber)
        {
            try
            {
                _logger.Information(Constants.GetAllPrimaryCollateralList);
                List<IdmSecurityDetailsDTO> listSecurityHolders = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(RouteName.GetAllSecurityHolders + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listSecurityHolders = JsonConvert.DeserializeObject<List<IdmSecurityDetailsDTO>>(Result.Result.ToString());
                }
                return listSecurityHolders;
            }
            catch(Exception ex)
            {
                _logger.Error(Error.getallPrimary + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
          
        }
        public async Task<bool> UpdatePrimaryCollateralDetails(IdmSecurityDetailsDTO addr)
        {
            try
            {
                _logger.Information(Constants.UpdatePrimaryCollateralDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(RouteName.UpdateLDSecurityDetails, httpContent);
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
                _logger.Error(Error.updatePrimary + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }

        #endregion

        #region Colletral Security

        public async Task<IEnumerable<IdmSecurityDetailsDTO>> GetAllCollateralList(long accountNumber)
        {
            try
            {
                _logger.Information(Constants.GetAllCollateralList);
                List<IdmSecurityDetailsDTO> listSecurityHolders = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(RouteName.GetAllSecurityHolders + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listSecurityHolders = JsonConvert.DeserializeObject<List<IdmSecurityDetailsDTO>>(Result.Result.ToString());
                }
                return listSecurityHolders;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.getallColletral + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        public async Task<bool> UpdateCollateralDetails(IdmSecurityDetailsDTO addr)
        {
            try
            {
                _logger.Information(Constants.UpdateCollateralDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(RouteName.UpdateLDSecurityDetails, httpContent);
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
                _logger.Error(Error.updateColletral + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }

        public async Task<bool> CreateCollateralDetails(IdmSecurityDetailsDTO addr)
        {
            try
            {
                _logger.Information(Constants.CreateCollateralDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(RouteName.CreateLDSecurityDetails, httpContent);
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
                _logger.Error(Error.CreateColletral + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }




        #endregion


        #region Hypothecation
        // <summary>
        //  Author:Manoj; Module: Hypothecation; Date:21/07/2022
        //Modified: Swetha M Added try catch with Loggings Date:17/10/2022

        // </summary>    
        public async Task<IEnumerable<HypoAssetDetailDTO>> GetAllHypothecationList(long accountNumber, string paramater)
        {
            try
            {
                _logger.Information(Constants.GetAllHypothecationList);
                List<HypoAssetDetailDTO> listHypotheDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(String.Format(RouteName.GetAllHypothecationList, accountNumber, paramater));

                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listHypotheDetails = JsonConvert.DeserializeObject<List<HypoAssetDetailDTO>>(Result.Result.ToString());
                }
                return listHypotheDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.gethypo + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        public async Task<IEnumerable<AssetRefnoDetailsDTO>> GetAllAssetRefList(long accountNumber)
        {
            try
            {
                _logger.Information(Constants.GetAllAssetRefList);
                List<AssetRefnoDetailsDTO> listAssetDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(RouteName.GetAllAssetRefList + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listAssetDetails = JsonConvert.DeserializeObject<List<AssetRefnoDetailsDTO>>(Result.Result.ToString());
                }
                return listAssetDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.getAsset + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
           
        }
        public async Task<bool> UpdateHypothecationDetails(IdmHypotheDetailsDTO addr)
        {
            try
            {
                _logger.Information(Constants.UpdateHypothecationDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(RouteName.UpdateLDHypothecationDetails, httpContent);
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
                _logger.Error(Error.UpdateHypo + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

           

        }
        public async Task<bool> DeleteHypothecationDetail(IdmHypotheDetailsDTO dto)
        {
            try
            {
                _logger.Information(Constants.DeleteHypothecationDetail);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(RouteName.DeleteLDHypothecationDetails, httpContent);
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
                _logger.Error(Error.getallPrimary + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
           
        }
        public async Task<bool> CreateHypothecationDetails(List<IdmHypotheDetailsDTO> addr)
        {
            try
            {
                _logger.Information(Constants.CreateHypothecationDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(RouteName.CreateLDHypothecationDetails, httpContent);
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
                _logger.Error(Error.Createhypo + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
      
        }
        #endregion

        #region SecurityCharge
        // <summary>
        //  Author: Sandeep; Module: SecurityCharge; Date:21/07/2022
        //Modified: Swetha M Added try catch with Loggings Date:17/10/2022
        // </summary>
        public async Task<IEnumerable<IdmSecurityChargeDTO>> GetAllSecurityChargeList(long accountNumber)
        {
            try
            {
                _logger.Information(Constants.GetAllSecurityChargeList);
                List<IdmSecurityChargeDTO> listSecurityChargeDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(RouteName.GetAllSecurityChargeList + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listSecurityChargeDetails = JsonConvert.DeserializeObject<List<IdmSecurityChargeDTO>>(Result.Result.ToString());
                }
                return listSecurityChargeDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.getSec + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
           
        }
        public async Task<bool> UpdateSecurityCharge(IdmSecurityChargeDTO addr)
        {
            try
            {
                _logger.Information(Constants.UpdateSecurityCharge);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(RouteName.UpdateSecurityChargeDetails, httpContent);
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
                _logger.Error(Error.updateSec + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
           
        }
        
        #endregion

        #region CERSAI
        // <summary>
        // Author: Gagana K; Module: CERSAIRegistration; Date:03/08/2022
        //Modified: Swetha M Added try catch with Loggings Date:17/10/2022

        // </summary>
        public async Task<IEnumerable<IdmCersaiRegDetailsDTO>> GetAllCERSAIList(long accountNumber, string parameter)
        {
            try
            {
                List<IdmCersaiRegDetailsDTO> listCERSAIDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(String.Format(RouteName.GetAllCERSAIRegistrationList,accountNumber, parameter));
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listCERSAIDetails = JsonConvert.DeserializeObject<List<IdmCersaiRegDetailsDTO>>(Result.Result.ToString());
                }
                return listCERSAIDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.getCeresai + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

         
        }
        public async Task<bool> CreateLDCersaiRegDetails(IdmCersaiRegDetailsDTO addr)
        {
            try
            {
                _logger.Information(Constants.CreateLDCersaiDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(RouteName.CreateLDCersaiDetails, httpContent);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrCR = await responseHttp.Content.ReadAsStringAsync();
                    JsonConvert.DeserializeObject<ApiResultResponse>(responseStrCR);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.updateCersai + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> DeleteLDCersaiRegDetails(IdmCersaiRegDetailsDTO dto)
        {
            try
            {
                _logger.Information(Constants.DeleteLDConditionDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(RouteName.DeleteLDCersaiDetails, httpContent);
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
                _logger.Error(Error.deleteCondition + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }

        #endregion

        #region GuarantorDeed
        // <Summary>
        // Author: Akhiladevi D M; Module: GuarantorDeed; Date: 10/08/2022
        //Modified: Swetha M Added try catch with Loggings Date:17/10/2022

        // <summary>
        public async Task<IEnumerable<IdmGuarantorDeedDetailsDTO>> GetAllGuarantorDeedList(long accountNumber)
        {
            try
            {
                _logger.Information(Constants.GetAllGuarantorDeedList);
                List<IdmGuarantorDeedDetailsDTO> listGuarantorDeedDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(RouteName.GetAllGuarantorList + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listGuarantorDeedDetails = JsonConvert.DeserializeObject<List<IdmGuarantorDeedDetailsDTO>>(Result.Result.ToString());
                }
                return listGuarantorDeedDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.getGauarntor + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
          
        }
        public async Task<bool> UpdateLDGuarantorDeedDetails(IdmGuarantorDeedDetailsDTO addr)

        {
            try
            {
                _logger.Information(Constants.UpdateLDGuarantorDeedDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(RouteName.UpdateLDGuarantorDeedDetails, httpContent);
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
                _logger.Error(Error.updateGuarantor + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
          
        }
        
        #endregion

        #region Condition
        // <summary>
        //  Author: Gagana K; Module:Conditions ; Date:11/08/2022
        //Modified: Swetha M Added try catch with Loggings Date:17/10/2022

        // </summary>
        public async Task<IEnumerable<LDConditionDetailsDTO>> GetAllConditionList(long accountNumber)
        {
            try
            {
                _logger.Information(Constants.GetAllConditionList);
                List<LDConditionDetailsDTO> listConditionDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(RouteName.GetAllConditionList + accountNumber);
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
                _logger.Error(Error.getCondition + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
          
        }
        public async Task<bool> DeleteLDConditionDetails(LDConditionDetailsDTO dto)
        {
            try
            {
                _logger.Information(Constants.DeleteLDConditionDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(RouteName.DeleteLDConditionDetails, httpContent);
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
                _logger.Error(Error.deleteCondition + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
           
        }
        public async Task<bool> CreateLDConditionDetails(LDConditionDetailsDTO addr)
        {
            try
            {
                _logger.Information(Constants.CreateLDConditionDetails);

                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(RouteName.CreateLDConditionDetails, httpContent);
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
                _logger.Error(Error.CreateCondition + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
          
        }
        public async Task<bool> UpdateLDConditionDetails(LDConditionDetailsDTO addr)
        {
            try
            {
                _logger.Information(Constants.UpdateLDConditionDetails);

                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(RouteName.UpdateLDConditionDetails, httpContent);
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
                _logger.Error(Error.updateCondition + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
          
        }
        #endregion
    }
}
