using KAR.KSFC.Components.Common.Dto.Common;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IUnitDetailsService;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.Admin.IDM.UnitDetailsService
{
    public class UnitDetailsService : IUnitDetailsService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _logger;

        public UnitDetailsService(IHttpClientFactory clientFactory, ILogger logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        #region Name Of Unit
        // <summary>
        //  Author: Sandeep; Module: Name Of Unit; Date:24/08/2022
        //  Modified: Dev Patel Added try catch with Loggings Date:19/10/2022
        //  Modified: Gagana - Written code for displaying Taluk, Hobli & Village details accordingly Date:06/12/2022
        // </summary>
        public async Task<IdmUnitDetailDTO> GetUnitDetails(long? accountNumber)
        {
            try
            {

                _logger.Information(CommonLogHelpers.GetAllUnitDetails);
                IdmUnitDetailDTO unitDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(UnitDetailsRoute.GetNameOfUnit + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    unitDetails = JsonConvert.DeserializeObject<IdmUnitDetailDTO>(result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllUnitDetails);
                return unitDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.getUnitName + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<IdmUnitDetailDTO> updateUnitName(IdmUnitDetailDTO idmUnitDetail)
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateSaveNameOfUnitDetails);
                IdmUnitDetailDTO unitDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(idmUnitDetail), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.UpdateNameOfUnit, httpContent);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrCD = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrCD);
                    unitDetails = JsonConvert.DeserializeObject<IdmUnitDetailDTO>(Result.Result.ToString());
                    return unitDetails;
                }
                return unitDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.updateUnitName + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        #endregion

        #region Change Location
        // <summary>
        // Author: Gagana K; Module: ChangeLocation Tab; Date:03/08/2022
        //  Modified: Dev Patel Added try catch with Loggings Date:19/10/2022
        // </summary>
        public async Task<IEnumerable<IdmUnitAddressDTO>> GetAllAddressDetails(long accountNumber)
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllAddressDetails);
                List<IdmUnitAddressDTO> listalladdress = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(UnitDetailsRoute.GetAllAddressDetails + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listalladdress = JsonConvert.DeserializeObject<List<IdmUnitAddressDTO>>(Result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllAddressDetails);
                return listalladdress;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAddressDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }           
        }

        public async Task<IEnumerable<TblPincodeMasterDTO>> GetAllMasterPinCodeDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllMasterPinCodeDetails);
                List<TblPincodeMasterDTO> listallPincode = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(UnitDetailsRoute.GetAllMasterPinCodeDetails);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listallPincode = JsonConvert.DeserializeObject<List<TblPincodeMasterDTO>>(Result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllMasterPinCodeDetails);
                return listallPincode;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetMasterPincodeDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<IEnumerable<PincodeDistrictCdtabDTO>> GetAllPinCodeDistrictDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllPinCodeDistrictDetails);
                List<PincodeDistrictCdtabDTO> listallPincodeDistrict = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(UnitDetailsRoute.GetAllPinCodeDistrictDetails);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listallPincodeDistrict = JsonConvert.DeserializeObject<List<PincodeDistrictCdtabDTO>>(Result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllPinCodeDistrictDetails);
                return listallPincodeDistrict;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetPincodeDistrictDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        public async Task<bool> UpdateAddressDetails(IdmUnitAddressDTO addr)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedUpdateAddressDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.UpdateAddressDetails, httpContent);
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
                _logger.Error(Error.UpdateAddressDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<IEnumerable<TlqCdTabDTO>> GetAllTalukDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllTalukNames);
                List<TlqCdTabDTO> listallTaluk = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(UnitDetailsRoute.GetAllTalukDetails);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listallTaluk = JsonConvert.DeserializeObject<List<TlqCdTabDTO>>(Result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedAllTalukNames);
                return listallTaluk;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllTalukDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<IEnumerable<HobCdtabDTO>> GetAllHobliDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllHobliNames);
                List<HobCdtabDTO> listallHobli = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(UnitDetailsRoute.GetAllHobliDetails);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listallHobli = JsonConvert.DeserializeObject<List<HobCdtabDTO>>(Result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedAllHobliNames);
                return listallHobli;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllHobliDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        public async Task<IEnumerable<VilCdTabDTO>> GetAllVillageDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllVillageNames);
                List<VilCdTabDTO> listallVillage = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(UnitDetailsRoute.GetAllVillageDetails);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listallVillage = JsonConvert.DeserializeObject<List<VilCdTabDTO>>(Result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedAllVillageNames);
                return listallVillage;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllVillageDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region Promoter Profile
        // <summary>
        //  Author: Dev Patel; Module: Promoter Profile; Date:26/08/2022
        //  Modified: Dev Patel Added try catch with Loggings Date:19/10/2022
        //  Modified: Gagana - Get Promoter Details from Master Table; Date:14/11/2022
        // </summary>

        public async Task<IEnumerable<TblPromcdtabDTO>> GetAllMasterPromoterProfileDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllMasterPromoterProfileDetails);
                List<TblPromcdtabDTO> MDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(UnitDetailsRoute.GetAllMasterPromoterProfileDetails);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    MDetails = JsonConvert.DeserializeObject<List<TblPromcdtabDTO>>(result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllMasterPromoterProfileDetails);
                return MDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllMasterPromoterProfileDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        public async Task<IEnumerable<IdmPromoterDTO>> GetAllPromoterProfileDetails(long? accountNumber)
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllPromoterProfileDetails);
                List<IdmPromoterDTO> PProfile = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(UnitDetailsRoute.GetAllPromoterProfileDetails + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    PProfile = JsonConvert.DeserializeObject<List<IdmPromoterDTO>>(result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllPromoterProfileDetails);
                return PProfile;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllPromoterProfileDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> CreatePromoterProfileDetails(IdmPromoterDTO pprofile)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedCreatePromoterProfile);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(pprofile), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.CreatePromoterProfileDetails, httpContent);
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
                _logger.Error(Error.CreatePromoterProfileDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> UpdatePromoterProfileDetails(IdmPromoterDTO pprofile)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedUpdatePromoterProfile);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(pprofile), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.UpdatePromoterProfileDetails, httpContent);
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
                _logger.Error(Error.UpdatePromoterProfileDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        public async Task<bool> DeletePromoterProfileDetails(IdmPromoterDTO dto)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedDeletePromoterProfile);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.DeletePromoterProfileDetails, httpContent);
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
                _logger.Error(Error.DeletePromoterProfileDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }
        #endregion 

        #region Promoter Address
        // <summary>
        //  Author: Gagana; Module: Promoter Address; Date:01/09/2022
        //  Modified: Dev Patel Added try catch with Loggings Date:19/10/2022
        // </summary>
        public async Task<IEnumerable<IdmPromAddressDTO>> GetAllPromoAddressDetails(long? accountNumber)
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllPromoAddressDetails);
                List<IdmPromAddressDTO> PAddress = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(UnitDetailsRoute.GetAllPromoAddressDetails + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    PAddress = JsonConvert.DeserializeObject<List<IdmPromAddressDTO>>(result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllPromoAddressDetails);
                return PAddress;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllPromoterAddressDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }

        public async Task<bool> CreatePromAddressDetails(IdmPromAddressDTO addr)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedCreatePromoterAddressDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.CreatePromAddressDetails, httpContent);
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
                _logger.Error(Error.CreatePromoterAddressDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }

        public async Task<bool> UpdatePromAddressDetails(IdmPromAddressDTO addr)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedUpdatePromoterAddressDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.UpdatePromAddressDetails, httpContent);
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
                _logger.Error(Error.UpdatePromoterAddressDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }
        public async Task<bool> DeletePromAddressDetails(IdmPromAddressDTO dto)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedDeletePromoterAddressDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.DeletePromAddressDetails, httpContent);
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
                _logger.Error(Error.DeletePromoterAddressDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }

        #endregion

        #region Promoter Bank
        // <summary>
        //  Author: Dev Patel; Module: Promoter Bank Info; Date:05/09/2022
        //  Modified: Dev Patel Added try catch with Loggings Date:19/10/2022
        // </summary>
        public async Task<IEnumerable<IdmPromoterBankDetailsDTO>> GetAllPromoterBankInfo(long? accountNumber)
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllPromoterBankInfo);
                List<IdmPromoterBankDetailsDTO> PBank = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(UnitDetailsRoute.GetAllPromoterBankInfo + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    PBank = JsonConvert.DeserializeObject<List<IdmPromoterBankDetailsDTO>>(result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllPromoterBankInfo);
                return PBank;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.getUnitName + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }
        public async Task<bool> CreatePromoterBankInfo(IdmPromoterBankDetailsDTO pbank)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedCreatePromoterBank);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(pbank), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.CreatePromoterBankInfo, httpContent);
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
                _logger.Error(Error.getUnitName + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }

        public async Task<bool> UpdatePromoterBankInfo(IdmPromoterBankDetailsDTO pbank)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedUpdatePromoterBank);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(pbank), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.UpdatePromoterBankInfo, httpContent);
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
                _logger.Error(Error.getUnitName + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }
        public async Task<bool> DeletePromoterBankInfo(IdmPromoterBankDetailsDTO dto)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedDeletePromoterBank);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.DeletePromoterBankInfo, httpContent);
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
                _logger.Error(Error.getUnitName + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region Product Details
        // <summary>
        //  Author: Gowtham S; Module: Product Details; Date:05/09/2022
        //  Modified: Dev Patel Added try catch with Loggings Date:19/10/2022
        // </summary>
        public async Task<IEnumerable<IdmUnitProductsDTO>> GetAllProductDetails(long accountNumber)
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllProductDetails);
                List<IdmUnitProductsDTO> listregistedaddress = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(UnitDetailsRoute.GetAllProductDetails + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listregistedaddress = JsonConvert.DeserializeObject<List<IdmUnitProductsDTO>>(Result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllProductDetails);
                return listregistedaddress;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.getUnitName + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }
         public async Task<IEnumerable<TblProdCdtabDTO>> GetAllProductList()
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllProductList);
                List<TblProdCdtabDTO> listofproduct = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(UnitDetailsRoute.GetAllProductList);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listofproduct = JsonConvert.DeserializeObject<List<TblProdCdtabDTO>>(Result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllProductList);
                return listofproduct;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.getUnitName + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }


        public async Task<IEnumerable<DropDownDTO>> GetallProducdetailsMaster()
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllProductDetails);
                List<DropDownDTO> listProduct = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(Constants.getproductName);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    listProduct = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllProductDetails);
                return listProduct;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.getUnitName + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }

        public async Task<IEnumerable<DropDownDTO>> GetallIndustrydetailsMaster()
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetallIndustrydetailsMaster);
                List<DropDownDTO> listIndustry = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(Constants.getIndustry);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    listIndustry = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetallIndustrydetailsMaster);
                return listIndustry;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.getUnitName + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }

        public async Task<bool> CreateProductDetails(IdmUnitProductsDTO pro)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedCreateProductDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(pro), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.CreateProductDetails, httpContent);
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
                _logger.Error(Error.getUnitName + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }

        public async Task<bool> UpdateProductDetails(IdmUnitProductsDTO pro)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedUpdateProductDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(pro), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.UpdateProductDetails, httpContent);
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
                _logger.Error(Error.getUnitName + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }

        public async Task<bool> DeleteProductDetails(IdmUnitProductsDTO pro)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedDeleteProductDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(pro), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.DeleteProductDetails, httpContent);
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
                _logger.Error(Error.getUnitName + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }
        #endregion

        #region Change bank Details
        // <summary>
        //  Author: Manoj ; Module: Change Bank Details ; Date:05/09/2022
        //  Modified: Dev Patel Added try catch with Loggings Date:19/10/2022
        // </summary>
        public async Task<IEnumerable<IdmChangeBankDetailsDTO>> GetAllChangebankDetails(long accountNumber)
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllChangebankDetails);
                List<IdmChangeBankDetailsDTO> listBankDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(UnitDetailsRoute.GetAllChangeBankDetails + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listBankDetails = JsonConvert.DeserializeObject<List<IdmChangeBankDetailsDTO>>(Result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllChangebankDetails);
                return listBankDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllBankDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }
        public async Task<IEnumerable<IfscMasterDTO>> GetAllIfscbankDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllIfscbankDetails);
                List<IfscMasterDTO> listIfscBankDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(UnitDetailsRoute.GetAllIfscBankDetails);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listIfscBankDetails = JsonConvert.DeserializeObject<List<IfscMasterDTO>>(Result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllIfscbankDetails);
                return listIfscBankDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.getUnitName + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }

        public async Task<bool> CreateChangeBankDetails(IdmChangeBankDetailsDTO changeBankDetail)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedCreateChangeBankDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(changeBankDetail), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.CreateChangeBankDetails, httpContent);
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
                _logger.Error(Error.CreateBankDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }

        public async Task<bool> UpdateChangeBankDetails(IdmChangeBankDetailsDTO changeBankDetail)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedUpdateChangeBankDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(changeBankDetail), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.UpdateChangeBankDetails, httpContent);
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
                _logger.Error(Error.UpdateBankDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }

        public async Task<bool> DeleteChangeBankDetails(IdmChangeBankDetailsDTO changeBankDetail)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedDeleteChangeBankDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(changeBankDetail), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.DeleteChangeBankDetails, httpContent);
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
                _logger.Error(Error.DeleteBankDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }
        #endregion

        #region Asset information
        // <summary>
        //  Author: Gowtham S; Module: Asset Information; Date:05/09/2022
        //  Modified: Dev Patel Added try catch with Loggings Date:19/10/2022
        // </summary>
        public async Task<IEnumerable<IdmPromAssetDetDTO>> GetAllPromoterAssetDetails(long accountNumber)
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllPromoterAssetDetails);
                List<IdmPromAssetDetDTO> listregistedaddress = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(UnitDetailsRoute.GetAllPromoterAssetDetails + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listregistedaddress = JsonConvert.DeserializeObject<List<IdmPromAssetDetDTO>>(Result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllPromoterAssetDetails);
                return listregistedaddress;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllPromoterAssetDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }

        public async Task<IEnumerable<DropDownDTO>> GetallAssetTypeMaster()
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetallAssetTypeMaster);
                List<DropDownDTO> listAssetType = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(Constants.getAssetType);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    listAssetType = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetallAssetTypeMaster);
                return listAssetType;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllAssetTypeDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }

       public async Task<IEnumerable<AssetTypeDetailsDTO>> GetAllAssetTypeDetails() 
      {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllAssetTypeDetails);
                List<AssetTypeDetailsDTO> listAssetType = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(UnitDetailsRoute.GetAllAssetTypeDetails);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();

                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listAssetType = JsonConvert.DeserializeObject<List<AssetTypeDetailsDTO>>(Result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllAssetTypeDetails);
                return listAssetType;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllAssetTypeDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
       }

        public async Task<IEnumerable<TblAssetCatCDTabDTO>> GetAllAssetCategaryDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllAssetCategaryDetails);
                List<TblAssetCatCDTabDTO> listAssetCategary = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(UnitDetailsRoute.GetAllAssetCategaryDetails);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();

                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listAssetCategary = JsonConvert.DeserializeObject<List<TblAssetCatCDTabDTO>>(Result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllAssetCategaryDetails);
                return listAssetCategary;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllAssetCategoryDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }


        public async Task<IEnumerable<DropDownDTO>> GetallAssetCategoryMaster()
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetallAssetCategoryMaster);
                List<DropDownDTO> listAssetCategary = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(Constants.getAssetCat);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    listAssetCategary = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetallAssetCategoryMaster);
                return listAssetCategary;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllAssetCategoryDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }


        public async Task<bool> CreateAssetDetails(IdmPromAssetDetDTO pro)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedCreateAssetDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(pro), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.CreateAssetDetails, httpContent);
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
                _logger.Error(Error.CreatePromoterAssetDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }

        public async Task<bool> UpdateAssetDetails(IdmPromAssetDetDTO pro)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedUpdateAssetDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(pro), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.UpdateAssetDetails, httpContent);
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
                _logger.Error(Error.UpdatePromoterAssetDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }
       


        public async Task<bool> DeleteAssetDetails(IdmPromAssetDetDTO pro)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedDeleteAssetDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(pro), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.DeleteAssetDetails, httpContent);
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
                _logger.Error(Error.DeletePromoterAssetDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }

        #endregion

        #region Promoter Liability Info
        // <summary>
        //  Author: Sandeep K; Module: Promoter Liability Information; Date:05/09/2022
        //  Modified: Dev Patel Added try catch with Loggings Date:19/10/2022
        // </summary>
        public async Task<IEnumerable<TblPromoterLiabDetDTO>> GetAllPromoLiabilityInfo(long? accountNumber)
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllPromoLiabilityInfo);
                List<TblPromoterLiabDetDTO> PromLiabInfo = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(UnitDetailsRoute.GetAllPromoterLiabilityInfo + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                     var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    PromLiabInfo = JsonConvert.DeserializeObject<List<TblPromoterLiabDetDTO>>(result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllPromoLiabilityInfo);
                return PromLiabInfo;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllPromoterLiabilityDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }

        public async Task<bool> UpdatePromoLiabilityInfo(TblPromoterLiabDetDTO addr)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedUpdatePromoterLiability);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.UpdatePromoterLiabilityInfo, httpContent);
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
                _logger.Error(Error.UpdatePromoterLiabilityDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }

        public async Task<bool> CreatePromoLiabilityInfo(TblPromoterLiabDetDTO addr)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedCreatePromoterLiability);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.CreatePromoterLiabilityInfo, httpContent);
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
                _logger.Error(Error.CreatePromoterLiabilityDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }

        public async Task<bool> DeletePromoLiabilityInfo(TblPromoterLiabDetDTO dto)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedDeletePromoterLiability);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.DeletePromoterLiabilityInfo, httpContent);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrCD = await responseHttp.Content.ReadAsStringAsync();
                    JsonConvert.DeserializeObject<ApiResultResponse>(responseStrCD);
                    return true;
                }
                _logger.Information(CommonLogHelpers.CompletedDeletePromoterLiability);
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.DeletePromoterLiabilityDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }


        #endregion

        #region Promoter Net Worth
        // <summary>
        //  Author: Sandeep K; Module: Promoter NetWorth Information; Date:05/09/2022
        //  Modified: Dev Patel Added try catch with Loggings Date:19/10/2022
        //  Modified: Gagana Added SaveAssetNetworthDetails/ Date:19/10/2022
        // </summary>
        public async Task<IEnumerable<TblPromoterNetWortDTO>> GetAllPromoterNetWorth(long? accountNumber)
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllPromoterNetWorth);
                List<TblPromoterNetWortDTO> PromNetWorth = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(UnitDetailsRoute.GetAllPromoterNetWorth + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    PromNetWorth = JsonConvert.DeserializeObject<List<TblPromoterNetWortDTO>>(result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllPromoterNetWorth);
                return PromNetWorth;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllPromoterNetWorthDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }            
        }

        public async Task<bool> SaveAssetNetworthDetails(TblPromoterNetWortDTO pro)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedSaveAssetNetworthDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(pro), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.SaveAssetNetworthDetails, httpContent);
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
                _logger.Error(Error.SaveAssetNetworthDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> SaveLiabilityNetworthDetails(TblPromoterNetWortDTO pro)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedSaveLiabilityNetworthDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(pro), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.SaveLiabilityNetworthDetails, httpContent);
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
                _logger.Error(Error.SaveLiabilityNetworthDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        #endregion

        //US#05 
        #region Means Of Finance
        //<summary>
        // Author: Swetha; Module:Import Machinery Inspection; Date:25/08/2022
        //Modified: Gagana K Added try catch with Loggings Date:19/10/2022
        //</summary
        public async Task<IEnumerable<IdmDchgMeansOfFinanceDTO>> GetAllMeansOfFinanceList(long accountNumber)
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllMeansOfFinanceList);
                List<IdmDchgMeansOfFinanceDTO> meansOfFinanceDTo = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(InspectionOfUnitRoute.GetAllMeansOfFinanceList + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    meansOfFinanceDTo = JsonConvert.DeserializeObject<List<IdmDchgMeansOfFinanceDTO>>(Result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllMeansOfFinanceList);
                return meansOfFinanceDTo;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.getMeansOfFinance + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetFinanceTypeAsync()
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetFinanceTypeAsync);
                List<SelectListItem> FinanceCatDTo = new();

                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(InspectionOfUnitRoute.GetFinanceTypeAsync);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    if (Result.Result.ToString() != Constants.brackets)
                        FinanceCatDTo = JsonConvert.DeserializeObject<List<SelectListItem>>(Result.Result.ToString());
                    else
                        FinanceCatDTo = new List<SelectListItem>();
                }
                _logger.Information(CommonLogHelpers.CompletedGetFinanceTypeAsync);
                return FinanceCatDTo;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.getFinanceType + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }


        public async Task<bool> CreateMeansOfFinanceDetails(IdmDchgMeansOfFinanceDTO meansOfFinance)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedMeansOfFinanceCreate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(meansOfFinance), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.CreateMeansOfFinanceDetails, httpContent);
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
                _logger.Error(Error.createMeansOfFinance + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        public async Task<bool> UpdateMeansOfFinanceDetails(IdmDchgMeansOfFinanceDTO meansOfFinance)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedMeansOfFinanceUpdate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(meansOfFinance), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.UpdateMeansOfFinanceDetails, httpContent);
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
                _logger.Error(Error.updateMeansOfFinance + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }

        public async Task<bool> DeleteMeansOfFinanceDetails(IdmDchgMeansOfFinanceDTO meansOfFinance)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedMeansOfFinanceDelete);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(meansOfFinance), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.DeleteMeansOfFinanceDetails, httpContent);
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
                _logger.Error(Error.deleteMeansOfFinance + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }

        #endregion


        #region Project Cost 
        public async Task<IEnumerable<IdmDchgProjectCostDTO>> GetAllProjectCostDetailsList(long accountNumber)
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllProjectCostDetailsList);
                List<IdmDchgProjectCostDTO> ProjectCostDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(InspectionOfUnitRoute.GetAllProjectCostDetailsList + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    ProjectCostDetails = JsonConvert.DeserializeObject<List<IdmDchgProjectCostDTO>>(Result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllProjectCostDetailsList);
                return ProjectCostDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.getProjectCost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }

        public async Task<bool> UpdateProjectCostDetails(IdmDchgProjectCostDTO ProjectCost)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedProjectCostUpdate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(ProjectCost), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.UpdateProjectCostDetails, httpContent);
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
                _logger.Error(Error.updateProjectCost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }

        public async Task<bool> DeleteProjectCostDetails(IdmDchgProjectCostDTO dto)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedProjectCostDelete);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.DeleteProjectCostDetails, httpContent);
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
                _logger.Error(Error.deleteProjectCost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }

        public async Task<bool> CreateProjectCostDetails(IdmDchgProjectCostDTO addr)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedProjectCostCreate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.CreateProjectCostDetails, httpContent);
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
                _logger.Error(Error.createProjectCost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        #endregion

        #region Letter Of Credit Details
        public async Task<IEnumerable<IdmDsbLetterOfCreditDTO>> GetAllLetterOfCreditDetailsList(long accountNumber)
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllLetterOfCreditDetailsList);
                List<IdmDsbLetterOfCreditDTO> letterOfCreditDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(InspectionOfUnitRoute.GetAllLetterOfCreditList + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    letterOfCreditDetails = JsonConvert.DeserializeObject<List<IdmDsbLetterOfCreditDTO>>(Result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllLetterOfCreditDetailsList);
                return letterOfCreditDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.getLetterOfCredit + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }

        public async Task<bool> UpdateLetterOfCreditDetails(IdmDsbLetterOfCreditDTO letterOfCreditDetails)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedLetterOfCreditUpdate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(letterOfCreditDetails), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.UpdateLetterOfCreditDetails, httpContent);
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
                _logger.Error(Error.updateLetterOfCredit + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }


        public async Task<bool> DeleteLetterOfCreditDetails(IdmDsbLetterOfCreditDTO dto)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedLetterOfCreditDelete);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.DeleteLetterOfCreditDetails, httpContent);
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
                _logger.Error(Error.deleteLetterOfCredit + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }

        public async Task<bool> CreateLetterOfCreditDetails(IdmDsbLetterOfCreditDTO addr)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedLetterOfCreditCreate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.CreateLetterOfCreditDetails, httpContent);
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
                _logger.Error(Error.createLetterOfCredit + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        #endregion

        #region Working Capital
        public async Task<bool> CreateWorkingCapitalDetails(IdmDchgWorkingCapitalDTO workingCapital)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StratedSaveWorkingCapitalInspectionDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(workingCapital), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.CreateWorkingCapitalInspection, httpContent);
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
                _logger.Error(Error.createWorkingCapitalInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }

        #endregion

        #region Land Assets


        public async Task<IEnumerable<TblIdmProjLandDTO>> GetAllProjLandDetailsList(long accountNumber)
        {
            _logger.Information(CommonLogHelpers.GetAllLandAssetDetails);
            List<TblIdmProjLandDTO> listLandassetDetails = new();
            var client = _clientFactory.CreateClient(Constants.client);
            var responseHttp = await client.GetAsync(UnitDetailsRoute.GetAllLandAssets + accountNumber);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                listLandassetDetails = JsonConvert.DeserializeObject<List<TblIdmProjLandDTO>>(Result.Result.ToString());
            }
            _logger.Information(CommonLogHelpers.CompletedGetAllLandAssetDetails);
            return listLandassetDetails;
        }

        public async Task<bool> CreateLandAssets(TblIdmProjLandDTO tblIdmProjLand)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedCreateLandAssetsDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(tblIdmProjLand), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(UnitDetailsRoute.CreateLandAssetDetails, httpContent);
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
                _logger.Error(Error.CreateLandAssets + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }


        #endregion

        #region LoanAllocation
        public async Task<IEnumerable<TblIdmDhcgAllcDTO>> GetAllLoanAllocationList(long accountNumber)
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllLoanAllocationList);
                List<TblIdmDhcgAllcDTO> listLoanAllocation = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(LoanAllocationRoute.GetAllLoanAllocationList + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    listLoanAllocation = JsonConvert.DeserializeObject<List<TblIdmDhcgAllcDTO>>(Result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllLoanAllocationList);
                return listLoanAllocation;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.getLoanAllocation + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        public async Task<bool> UpdateLoanAllocationDetails(TblIdmDhcgAllcDTO addr)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedLoanAllocationUpdate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(LoanAllocationRoute.UpdateLoanAllocationDetails, httpContent);
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
                _logger.Error(Error.updateLoanAllocation + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        public async Task<bool> CreateLoanAllocationDetails(TblIdmDhcgAllcDTO addr)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedLoanAllocationCreate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(LoanAllocationRoute.CreateLoanAllocationDetails, httpContent);
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
                _logger.Error(Error.createLoanAllocation + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        public async Task<bool> DeleteLoanAllocationDetails(TblIdmDhcgAllcDTO dto)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedLoanAllocationDelete);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(LoanAllocationRoute.DeleteLoanAllocationDetails, httpContent);
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
                _logger.Error(Error.deleteLoanAllocation + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region Common DropDown
        public async Task<IEnumerable<DropDownDTO>> GetAllProjectCostComponentsDetails()
        {
            List<DropDownDTO> projectCostComponentDetail = new();
            var client = _clientFactory.CreateClient(Constants.client);
            var responseHttp = await client.GetAsync(RouteName.GetPojectCostComponentAsync);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                projectCostComponentDetail = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return projectCostComponentDetail;
        }

       


        #endregion
    }
}
