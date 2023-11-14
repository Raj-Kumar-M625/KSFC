using KAR.KSFC.Components.Common.Dto.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.AssociateSisterConcernDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;
using KAR.KSFC.Components.Common.Security;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Services.IServices;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services
{
    public class EnquirySubmissionService : IEnquirySubmissionService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly SessionManager _sessionManager;
        private readonly IApiService _apiService;

        public EnquirySubmissionService(IHttpClientFactory clientFactory, SessionManager sessionManager,IApiService apiService)
        {
            _clientFactory = clientFactory;
            _sessionManager = sessionManager;
            _apiService = apiService;
        }

        public async Task<AllDDLListDTO> getCascadeDDLForEditPrefill(int? distId, int? talukaId, int? hobliId)
        {
            AllDDLListDTO cascadeDDLList = new AllDDLListDTO();
           

            var responseHttp = await _apiService.GetAsync($"Common/GetTaluka?DistrictId={distId}");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    cascadeDDLList.ListTaluka = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    cascadeDDLList.ListTaluka = new List<SelectListItem>();
            }
            responseHttp = await _apiService.GetAsync($"Common/GetHobli?TalukaId={talukaId}");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    cascadeDDLList.ListHobli = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    cascadeDDLList.ListHobli = new List<SelectListItem>();
            }
            responseHttp = await _apiService.GetAsync($"Common/GetVillage?HobliId={hobliId}");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    cascadeDDLList.ListVillage = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    cascadeDDLList.ListVillage = new List<SelectListItem>();
            }
            return cascadeDDLList;
        }

        public async Task<List<SelectListItem>> getAllAddressTypesFromDB()
        {
           
            List<SelectListItem> listAddressTypes = new List<SelectListItem>();

            var responseHttp = await _apiService.GetAsync($"Common/GetAllAddressType");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                listAddressTypes = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
            }
            return listAddressTypes;
        }
        public async Task<AllDDLListDTO> GetAllEnquiryDropDownList()
        {
            EnquiryDTO enquiryDTO = new EnquiryDTO();
            enquiryDTO.DDLDTO = new AllDDLListDTO();

            //GetOfficeBranch
           
            var responseHttp = await _apiService.GetAsync($"Common/GetOfficeBranch");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListBranch = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListBranch = new List<SelectListItem>();
            }

            responseHttp = await _apiService.GetAsync($"Common/GetLoanPurpose");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListLoanPurpose = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListLoanPurpose = new List<SelectListItem>();
            }

            responseHttp = await _apiService.GetAsync($"Common/GetNatureOfPremises");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListPremises = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListPremises = new List<SelectListItem>();
            }

            responseHttp = await _apiService.GetAsync($"Common/GetSizeOfFirm");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListFirmSize = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListFirmSize = new List<SelectListItem>();
            }

            responseHttp = await _apiService.GetAsync($"Common/GetProductName");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListProduct = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListProduct = new List<SelectListItem>();
            }
            //District, Taluka, Hobli, Village


            responseHttp = await _apiService.GetAsync($"Common/GetRegistrationType");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListRegnType = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListRegnType = new List<SelectListItem>();
            }

            //Position / Designation
            responseHttp = await _apiService.GetAsync($"Common/GetPositionDesignation");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListPromDesgnType = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListPromDesgnType = new List<SelectListItem>();
            }
            //Domicile Status
            responseHttp = await _apiService.GetAsync($"Common/GetDomicileStatus");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListDomicileStatus = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListDomicileStatus = new List<SelectListItem>();
            }
            //Asset Category
            responseHttp = await _apiService.GetAsync($"Common/GetAssetCategoryAsync");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListAssetCategory = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListAssetCategory = new List<SelectListItem>();
            }
            //Type of Asset
            responseHttp = await _apiService.GetAsync($"Common/GetAssetTypeAsync");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListAssetType = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListAssetType = new List<SelectListItem>();
            }
            //Mode of Acquire
            responseHttp = await _apiService.GetAsync($"Common/GetModeofAcquireAsync");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                enquiryDTO.DDLDTO.ListAcquireMode = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
            }
            

            //Name of Facility
            responseHttp = await _apiService.GetAsync($"Common/GetNameOFFacilityAsync");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListFacility = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListFacility = new List<SelectListItem>();
            }
            //Financial Year
            responseHttp = await _apiService.GetAsync($"Common/GetFinancialYearAsync");
            if(responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListFY = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListFY = new List<SelectListItem>();
            }
            //Financial Component
            responseHttp = await _apiService.GetAsync($"Common/GetFinancialComponentAsync");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListFinancialComponent = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListFinancialComponent = new List<SelectListItem>();
            }

            //Project Cost Component
            responseHttp = await _apiService.GetAsync($"Common/GetPojectCostComponentAsync");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListProjectCost = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListProjectCost = new List<SelectListItem>();
            }
            //Category of Means of Finance
            responseHttp = await _apiService.GetAsync($"Common/GetMeansofFinanceCategoryAsync");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListMeansOfFinanceCategory = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListMeansOfFinanceCategory = new List<SelectListItem>();
            }


            //Type of Security
            responseHttp = await _apiService.GetAsync($"Common/GetSecurityTypeAsync");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListSecurityType = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListSecurityType = new List<SelectListItem>();
            }
            //Security Details
            responseHttp = await _apiService.GetAsync($"Common/GetSecurityDetailsAsync");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListSecurityDet = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListSecurityDet = new List<SelectListItem>();
            }
            //Relation
            responseHttp = await _apiService.GetAsync($"Common/GetRelationAsync");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListSecurityRelation = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListSecurityRelation = new List<SelectListItem>();
            }
            //Adding null values to Taluka, Hobli and Villlage drop down list

            responseHttp = await _apiService.GetAsync($"Common/GetDistrict");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListDistrict = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListDistrict = new List<SelectListItem>();
            }
            
            //get industry type

            responseHttp = await _apiService.GetAsync($"Common/GetAllIndustrytype");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListIndustryType = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListIndustryType = new List<SelectListItem>();
            }

            //get promotor class

            responseHttp = await _apiService.GetAsync($"Common/GetAllPromotorClass");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != "[]")
                    enquiryDTO.DDLDTO.ListPromotorClass = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    enquiryDTO.DDLDTO.ListPromotorClass = new List<SelectListItem>();
            }

            return enquiryDTO.DDLDTO;
        }

        public async Task<List<SelectListItem>> PopulateSubLocationList(string locationType, int id)
        {
           
            if (locationType == "District")
            {
                var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
                var responseHttp = await _apiService.GetAsync($"Common/GetTaluka?DistrictId={id}");
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    string responseStr = await responseHttp.Content.ReadAsStringAsync();
                    var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                    var responseTaluk = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                    return responseTaluk;
                }
            }
            if (locationType == "Taluka")
            {
                var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
                var responseHttp = await _apiService.GetAsync($"Common/GetHobli?TalukaId={id}");
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    string responseStr = await responseHttp.Content.ReadAsStringAsync();
                    var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                    var responseHobli = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                    return responseHobli;
                }
            }
            if (locationType == "Hobli")
            {
                var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
                var responseHttp = await _apiService.GetAsync($"Common/GetVillage?HobliId={id}");
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    string responseStr = await responseHttp.Content.ReadAsStringAsync();
                    var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                    var responseTaluk = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                    return responseTaluk;
                }
            }
            return null;
        }

        public async Task<List<SelectListItem>> PopulateFinanceTypeList(int id)
        {
           
            var responseHttp = await _apiService.GetAsync($"Common/GetFinanceTypeAsync?mfCategory={id}");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                var response = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                return response;
            }
            return null;
        }

        /// <summary>
        /// Get all Enquiries..
        /// </summary>
        /// <param name="regDetailsList"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public async Task<List<EnquirySummary>> GetAllEnquiries()
        {
            try
            {
                //int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                List<EnquirySummary> enquiriesList = new List<EnquirySummary>();
                var successObj = new ApiResultResponse();
                var responseHttp = await _apiService.GetAsync($"EnquiryHome/GetAllEnquries");
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    enquiriesList = JsonConvert.DeserializeObject<List<EnquirySummary>>(successObj.Result.ToString());

                }
                return enquiriesList;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> SaveUnitDetailsBasicDetails(BasicDetailsDto basicDetailsDTO, Boolean isNew)
        {
            try
            {
               
                var Result = new ApiResultResponse();
                if (basicDetailsDTO.EnqtempId == null || basicDetailsDTO.EnqtempId == 0)
                {
                    int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                    basicDetailsDTO.PromoterPan = basicDetailsDTO.PromoterPan;

                    basicDetailsDTO.EnqtempId = newTempEnqId;
                    var contentBasicDetails = new StringContent(JsonConvert.SerializeObject(basicDetailsDTO), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"BasicDetails/AddBasicDetails", contentBasicDetails);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        BasicDetailsDto basicDetDTO = JsonConvert.DeserializeObject<BasicDetailsDto>(Result.Result.ToString());
                        _sessionManager.SetNewEnqTempId(basicDetDTO.EnqtempId.ToString());
                        _sessionManager.SetUDPersonalDetails(basicDetDTO);
                        return true;
                    }
                }
                else
                {
                    var contentBasicDetails = new StringContent(JsonConvert.SerializeObject(basicDetailsDTO), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"BasicDetails/UpdateBasicDetails", contentBasicDetails);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        BasicDetailsDto basicDetDTO = JsonConvert.DeserializeObject<BasicDetailsDto>(Result.Result.ToString());
                        _sessionManager.SetNewEnqTempId(basicDetDTO.EnqtempId.ToString());
                        _sessionManager.SetUDPersonalDetails(basicDetDTO);
                        return true;
                    }
                }

                return false;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<bool> SaveUnitDetailsAddressDetails(List<AddressDetailsDTO> addressDetailsList, Boolean isNew)
        {
            try
            {
                int? EnquiryId = 0;
               
                var response = new ApiResultResponse();
                EnquiryId = addressDetailsList.FirstOrDefault().EnqtempId;
                if (EnquiryId == null || EnquiryId == 0)
                {
                    int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                    addressDetailsList.ForEach(x => x.EnqtempId = newTempEnqId);
                    var content = new StringContent(JsonConvert.SerializeObject(addressDetailsList), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"AddressDetails/AddAddressDetails", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        List<AddressDetailsDTO> dto = JsonConvert.DeserializeObject<List<AddressDetailsDTO>>(response.Result.ToString());
                        return true;
                    }
                }
                else
                {
                    addressDetailsList.ForEach(x => x.EnqtempId = EnquiryId.Value);
                    var content = new StringContent(JsonConvert.SerializeObject(addressDetailsList), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"AddressDetails/UpdateAddressDetails", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        return true;
                    }
                }
                return false;
            }
            catch (System.Exception)
            {

                throw;
            }

        }
        public async Task<bool> SaveUnitDetailsBankDetails(BankDetailsDTO bankDetailsDTO, Boolean isNew)
        {
            try
            {
               
                var respopnse = new ApiResultResponse();
                if (bankDetailsDTO.EnqtempId == null || bankDetailsDTO.EnqtempId == 0)
                {
                    bankDetailsDTO.EnqtempId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                    var content = new StringContent(JsonConvert.SerializeObject(bankDetailsDTO), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"BankDetails/AddBankDetails", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        respopnse = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        BankDetailsDTO BankDetailsDto = JsonConvert.DeserializeObject<BankDetailsDTO>(respopnse.Result.ToString());
                        _sessionManager.SetUDBankDetails(BankDetailsDto);
                        return true;
                    }
                }
                else
                {
                    var content = new StringContent(JsonConvert.SerializeObject(bankDetailsDTO), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"BankDetails/UpdateBankDetails", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        respopnse = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        BankDetailsDTO BankDetailsDto = JsonConvert.DeserializeObject<BankDetailsDTO>(respopnse.Result.ToString());
                        _sessionManager.SetUDBankDetails(BankDetailsDto);
                        return true;
                    }
                }
                return false;
            }
            catch (System.Exception)
            {

                throw;
            }

        }
        public async Task<bool> SaveUnitDetailsRegistrationDetails(List<RegistrationNoDetailsDTO> regDetailsList, Boolean isNew)
        {
            try
            {
                int? EnquiryId = 0;
               
                var response = new ApiResultResponse();
                EnquiryId = regDetailsList.FirstOrDefault().EnqtempId;
                if (EnquiryId == null || EnquiryId == 0)
                {
                    int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                    regDetailsList.ForEach(x => x.EnqtempId = newTempEnqId);
                    var content = new StringContent(JsonConvert.SerializeObject(regDetailsList), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"RegistrationDetails/AddRegistrationDetails", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        List<RegistrationNoDetailsDTO> dto = JsonConvert.DeserializeObject<List<RegistrationNoDetailsDTO>>(response.Result.ToString());
                        return true;
                    }
                }
                else
                {
                    regDetailsList.ForEach(x => x.EnqtempId = EnquiryId.Value);
                    var content = new StringContent(JsonConvert.SerializeObject(regDetailsList), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"RegistrationDetails/UpdateRegistrationDetails", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        return true;
                    }
                }
                return false;
            }
            catch (System.Exception)
            {



                throw;
            }
        }

        public async Task<bool> SavePromoterDetails(List<PromoterDetailsDTO> promoterDetailsDTOsList)
        {
            try
            {

               
                var response = new ApiResultResponse();
                int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                promoterDetailsDTOsList.ForEach(x => x.EnqtempId = newTempEnqId);
                var content = new StringContent(JsonConvert.SerializeObject(promoterDetailsDTOsList), Encoding.UTF8, "application/json");
                var responseHttp = await _apiService.PostAsync($"PromoterDetails/AddPromoterDetails", content);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    List<PromoterDetailsDTO> dto = JsonConvert.DeserializeObject<List<PromoterDetailsDTO>>(response.Result.ToString());
                    var dtoItem = dto.FirstOrDefault();
                    _sessionManager.SetNewEnqTempId(dtoItem.EnqtempId.ToString());
                    promoterDetailsDTOsList.ForEach(x =>
                    {
                        x.EnqPromId = dtoItem.EnqPromId;
                        x.PromoterCode = dtoItem.PromoterCode;
                        x.PromoterMaster = dtoItem.PromoterMaster;
                    });
                    if (_sessionManager.GetPromoterDetailsList() != null)
                    {
                        promoterDetailsDTOsList.AddRange(_sessionManager.GetPromoterDetailsList());
                    }
                    _sessionManager.SetPromoterDetailsList(promoterDetailsDTOsList);
                    return true;
                }
                return false;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdatePromoterDetails(List<PromoterDetailsDTO> promoterDetailsDTOsList)
        {
            try
            {
                int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                promoterDetailsDTOsList.ForEach(x => x.EnqtempId = newTempEnqId);
               
                var response = new ApiResultResponse();
                var content = new StringContent(JsonConvert.SerializeObject(promoterDetailsDTOsList), Encoding.UTF8, "application/json");
                var responseHttp = await _apiService.PostAsync($"PromoterDetails/UpdatePromoterDetails", content);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    List<PromoterDetailsDTO> dto = JsonConvert.DeserializeObject<List<PromoterDetailsDTO>>(response.Result.ToString());
                    var dtoItem = dto.FirstOrDefault();
                    _sessionManager.SetNewEnqTempId(dtoItem.EnqtempId.ToString());

                    if (_sessionManager.GetPromoterDetailsList() != null)
                    {
                        var proDetailList = _sessionManager.GetPromoterDetailsList();
                        var itemToRemove = proDetailList.FirstOrDefault(m => m.EnqPromId == promoterDetailsDTOsList.FirstOrDefault().EnqPromId);
                        proDetailList.Remove(itemToRemove);
                        _sessionManager.SetPromoterDetailsList(proDetailList);
                    }
                    promoterDetailsDTOsList.ForEach(x =>
                    {
                        x.EnqPromId = dtoItem.EnqPromId;
                        x.PromoterCode = dtoItem.PromoterCode;
                        x.PromoterMaster = dtoItem.PromoterMaster;
                    });
                    promoterDetailsDTOsList.AddRange(_sessionManager.GetPromoterDetailsList());
                    _sessionManager.SetPromoterDetailsList(promoterDetailsDTOsList);

                    return true;
                }
                return false;
            }
            catch (System.Exception)
            {

                throw;
            }
        }


        public async Task<bool> SaveorEditPromoterLiabilityDetails(List<PromoterLiabilityDetailsDTO> promoterliabilityDetailsDTOList)
        {
            try
            {


                promoterliabilityDetailsDTOList.ForEach(x => x.EnqPromliabId = null);
                int? EnquiryId = 0;
               
                var response = new ApiResultResponse();
                EnquiryId = promoterliabilityDetailsDTOList.FirstOrDefault().EnqtempId;
                if (EnquiryId == null || EnquiryId == 0)
                {
                    int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                    promoterliabilityDetailsDTOList.ForEach(x => x.EnqtempId = newTempEnqId);
                    var content = new StringContent(JsonConvert.SerializeObject(promoterliabilityDetailsDTOList), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"PromoterLiabilityDetails/AddPromoterLiabilityDetails", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        List<PromoterLiabilityDetailsDTO> dto = JsonConvert.DeserializeObject<List<PromoterLiabilityDetailsDTO>>(response.Result.ToString());
                        _sessionManager.SetPromoterLiabilityList(dto);
                        return true;
                    }
                }
                else
                {
                    int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                    promoterliabilityDetailsDTOList.ForEach(x => x.EnqtempId = newTempEnqId);
                    var content = new StringContent(JsonConvert.SerializeObject(promoterliabilityDetailsDTOList), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"PromoterLiabilityDetails/UpdatePromoterLiabilityDetails", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        return true;
                    }
                }
                return false;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<bool> SaveorEditPromoterLiabilityNetWorth(List<PromoterNetWorthDetailsDTO> promoterNetWorthDetailsList)
        {
            try
            {
                int? EnquiryId = 0;
               
                var response = new ApiResultResponse();
                if (promoterNetWorthDetailsList.Count > 0)
                    EnquiryId = promoterNetWorthDetailsList.FirstOrDefault().EnqtempId;
                if (EnquiryId == null || EnquiryId == 0)
                {
                    int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                    promoterNetWorthDetailsList.ForEach(x => x.EnqtempId = newTempEnqId);
                    var content = new StringContent(JsonConvert.SerializeObject(promoterNetWorthDetailsList), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"PromoterLiabilityNetWorth/AddPromoterLiabilityNetWorth", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        List<PromoterNetWorthDetailsDTO> dto = JsonConvert.DeserializeObject<List<PromoterNetWorthDetailsDTO>>(response.Result.ToString());
                        _sessionManager.SetPromoterNetWorthList(dto);
                        return true;
                    }
                }
                else
                {
                    int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                    promoterNetWorthDetailsList.ForEach(x => x.EnqtempId = newTempEnqId);
                    var content = new StringContent(JsonConvert.SerializeObject(promoterNetWorthDetailsList), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"PromoterLiabilityNetWorth/UpdatePromoterLiabilityNetWorth", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        return true;
                    }
                }
                return false;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<bool> SaveorEditPromoterAssetsNetWorthDetails(List<PromoterAssetsNetWorthDTO> promoterassetsNetWorthDTOsList)
        {
            try
            {
                int? EnquiryId = 0;
                promoterassetsNetWorthDTOsList.ForEach(x => x.EnqPromassetId = null);
               
                var response = new ApiResultResponse();
                EnquiryId = promoterassetsNetWorthDTOsList.FirstOrDefault().EnqtempId;
                if (EnquiryId == null || EnquiryId == 0)
                {
                    int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                    promoterassetsNetWorthDTOsList.ForEach(x => x.EnqtempId = newTempEnqId);
                    var content = new StringContent(JsonConvert.SerializeObject(promoterassetsNetWorthDTOsList), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"PromoterAssetsNetWorthDetails/AddPromoterAssetsNetWorthDetails", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        List<PromoterAssetsNetWorthDTO> dto = JsonConvert.DeserializeObject<List<PromoterAssetsNetWorthDTO>>(response.Result.ToString());
                        _sessionManager.SetPromoterAssetList(dto);
                        return true;
                    }
                }
                else
                {
                    int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                    promoterassetsNetWorthDTOsList.ForEach(x => x.EnqtempId = newTempEnqId);
                    var content = new StringContent(JsonConvert.SerializeObject(promoterassetsNetWorthDTOsList), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"PromoterAssetsNetWorthDetails/UpdatePromoterAssetsNetWorthDetails", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        return true;
                    }
                }
                return false;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Save or edit for guarentor servvice
        /// </summary>
        /// <param name="guarantorAllDetailsDTO"></param>
        /// <returns></returns>


        public async Task<bool> SaveGuarantorDetails(List<GuarantorDetailsDTO> guarantorDetailsDTOsList)
        {
            try
            {
               
                var response = new ApiResultResponse();
                int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                guarantorDetailsDTOsList.ForEach(x => x.EnqtempId = newTempEnqId);
                var content = new StringContent(JsonConvert.SerializeObject(guarantorDetailsDTOsList), Encoding.UTF8, "application/json");
                var responseHttp = await _apiService.PostAsync($"GuarantorDetails/AddGuarantorDetails", content);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    List<GuarantorDetailsDTO> dto = JsonConvert.DeserializeObject<List<GuarantorDetailsDTO>>(response.Result.ToString());
                    var dtoItem = dto.FirstOrDefault();
                    _sessionManager.SetNewEnqTempId(dtoItem.EnqtempId.ToString());
                    guarantorDetailsDTOsList.ForEach(x =>
                    {
                        x.EnqGuarId = dtoItem.EnqGuarId;
                        x.PromoterCode = dtoItem.PromoterCode;
                        x.PromoterMaster = dtoItem.PromoterMaster;
                    });
                    if (_sessionManager.GetGuarantorDetailsList() != null)
                    {
                        guarantorDetailsDTOsList.AddRange(_sessionManager.GetGuarantorDetailsList());
                    }
                    _sessionManager.SetGuarantorDetailsList(guarantorDetailsDTOsList);
                    return true;
                }
                return false;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public async Task<bool> UpdateGuarantorDetails(List<GuarantorDetailsDTO> guarantorDetailsDTOsList)
        {
            try
            {
               
                int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                guarantorDetailsDTOsList.ForEach(x => x.EnqtempId = newTempEnqId);
                var response = new ApiResultResponse();
                var content = new StringContent(JsonConvert.SerializeObject(guarantorDetailsDTOsList), Encoding.UTF8, "application/json");
                var responseHttp = await _apiService.PostAsync($"GuarantorDetails/UpdateGuarantorDetails", content);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    List<GuarantorDetailsDTO> dto = JsonConvert.DeserializeObject<List<GuarantorDetailsDTO>>(response.Result.ToString());
                    var dtoItem = dto.FirstOrDefault();
                    _sessionManager.SetNewEnqTempId(dtoItem.EnqtempId.ToString());

                    if (_sessionManager.GetGuarantorDetailsList() != null)
                    {
                        var GuarantDetailList = _sessionManager.GetGuarantorDetailsList();
                        var itemToRemove = GuarantDetailList.FirstOrDefault(m => m.EnqGuarId == guarantorDetailsDTOsList.FirstOrDefault().EnqGuarId);
                        GuarantDetailList.Remove(itemToRemove);
                        _sessionManager.SetGuarantorDetailsList(GuarantDetailList);
                    }
                    guarantorDetailsDTOsList.ForEach(x =>
                    {
                        x.EnqGuarId = dtoItem.EnqGuarId;
                        x.PromoterCode = dtoItem.PromoterCode;
                        x.PromoterMaster = dtoItem.PromoterMaster;
                    });
                    guarantorDetailsDTOsList.AddRange(_sessionManager.GetGuarantorDetailsList());
                    _sessionManager.SetGuarantorDetailsList(guarantorDetailsDTOsList);
                    return true;
                }
                return false;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<bool> SaveorEditGuarantorLiabilityDetails(List<GuarantorLiabilityDetailsDTO> guarantorliabilityDetailsDTOList)
        {
            try
            {

                guarantorliabilityDetailsDTOList.ForEach(x => x.EnqGuarliabId = null);
                int? EnquiryId = 0;
               
                var response = new ApiResultResponse();
                EnquiryId = guarantorliabilityDetailsDTOList.FirstOrDefault().EnqtempId;
                if (EnquiryId == null || EnquiryId == 0)
                {
                    int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                    guarantorliabilityDetailsDTOList.ForEach(x => x.EnqtempId = newTempEnqId);
                    var content = new StringContent(JsonConvert.SerializeObject(guarantorliabilityDetailsDTOList), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"GuarantorLiabilityDetails/AddGuarantorLiabilityDetails", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        List<GuarantorLiabilityDetailsDTO> dto = JsonConvert.DeserializeObject<List<GuarantorLiabilityDetailsDTO>>(response.Result.ToString());
                        _sessionManager.SetGuarantorLiabilityList(dto);
                        return true;
                    }
                }
                else
                {
                    int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                    guarantorliabilityDetailsDTOList.ForEach(x => x.EnqtempId = newTempEnqId);
                    var content = new StringContent(JsonConvert.SerializeObject(guarantorliabilityDetailsDTOList), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"GuarantorLiabilityDetails/UpdateGuarantorLiabilityDetails", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        return true;
                    }
                }
                return false;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<bool> SaveorEditGuarantorLiabilityNetWorth(List<GuarantorNetWorthDetailsDTO> guarantorNetWorthDetailsList)
        {
            try
            {
                int? EnquiryId = 0;
               
                var response = new ApiResultResponse();
                EnquiryId = guarantorNetWorthDetailsList.FirstOrDefault().EnqtempId;
                if (EnquiryId == null || EnquiryId == 0)
                {
                    int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                    guarantorNetWorthDetailsList.ForEach(x => x.EnqtempId = newTempEnqId);
                    var content = new StringContent(JsonConvert.SerializeObject(guarantorNetWorthDetailsList), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"GuarantorLiabilityNetWorth/AddGuarantorLiabilityNetWorth", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        List<GuarantorNetWorthDetailsDTO> dto = JsonConvert.DeserializeObject<List<GuarantorNetWorthDetailsDTO>>(response.Result.ToString());
                        _sessionManager.SetGuarantorNetWorthList(dto);
                        return true;
                    }
                }
                else
                {
                    int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                    guarantorNetWorthDetailsList.ForEach(x => x.EnqtempId = newTempEnqId);
                    var content = new StringContent(JsonConvert.SerializeObject(guarantorNetWorthDetailsList), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"GuarantorLiabilityNetWorth/UpdateGuarantorLiabilityNetWorth", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        return true;
                    }
                }
                return false;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<bool> SaveorEditGuarantorAssetsNetWorthDetails(List<GuarantorAssetsNetWorthDTO> guarantorassetsNetWorthDTOsList)
        {
            try
            {

                guarantorassetsNetWorthDTOsList.ForEach(x => x.EnqGuarassetId = null);
                int? EnquiryId = 0;
               
                var response = new ApiResultResponse();
                EnquiryId = guarantorassetsNetWorthDTOsList.FirstOrDefault().EnqtempId;
                if (EnquiryId == null || EnquiryId == 0)
                {
                    int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                    guarantorassetsNetWorthDTOsList.ForEach(x => x.EnqtempId = newTempEnqId);
                    var content = new StringContent(JsonConvert.SerializeObject(guarantorassetsNetWorthDTOsList), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"GuarantorAssetsNetWorthDetails/AddGuarantorAssetsNetWorthDetails", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        List<GuarantorAssetsNetWorthDTO> dto = JsonConvert.DeserializeObject<List<GuarantorAssetsNetWorthDTO>>(response.Result.ToString());
                        _sessionManager.SetGuarantorAssetList(dto);
                        return true;
                    }
                }
                else
                {
                    int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                    guarantorassetsNetWorthDTOsList.ForEach(x => x.EnqtempId = newTempEnqId);
                    var content = new StringContent(JsonConvert.SerializeObject(guarantorassetsNetWorthDTOsList), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"GuarantorAssetsNetWorthDetails/UpdateGuarantorAssetsNetWorthDetails", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        return true;
                    }
                }
                return false;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Save or edit for Associate and sister concern
        /// </summary>
        /// <param name="listAssociateFYDetailsDTO"></param>
        /// <returns></returns>

        public async Task<bool> SaveAssociateSisterDetails(List<SisterConcernDetailsDTO> listAssociateDetailsDTO)
        {
            try
            {
               
                var response = new ApiResultResponse();
                int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                listAssociateDetailsDTO.ForEach(x => x.EnqtempId = newTempEnqId);
                var content = new StringContent(JsonConvert.SerializeObject(listAssociateDetailsDTO), Encoding.UTF8, "application/json");
                var responseHttp = await _apiService.PostAsync($"SisterConcernDetails/AddSisterConcernDetails", content);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    List<SisterConcernDetailsDTO> dto = JsonConvert.DeserializeObject<List<SisterConcernDetailsDTO>>(response.Result.ToString());
                    var dtoItem = dto.FirstOrDefault();
                    _sessionManager.SetNewEnqTempId(dtoItem.EnqtempId.ToString());
                    listAssociateDetailsDTO.ForEach(x =>
                    {
                        x.EnqSisId = dtoItem.EnqSisId;
                    });
                    if (_sessionManager.GetAssociateDetailsDTOList() != null)
                    {
                        listAssociateDetailsDTO.AddRange(_sessionManager.GetAssociateDetailsDTOList());
                    }
                    _sessionManager.SetAssociateDetailsDTOList(listAssociateDetailsDTO);

                    return true;
                }

                return false;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public async Task<bool> UpdateAssociateSisterDetails(List<SisterConcernDetailsDTO> listAssociateDetailsDTO)
        {

            try
            {
                int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                listAssociateDetailsDTO.ForEach(x => x.EnqtempId = newTempEnqId);
                var oldEnqSisId = listAssociateDetailsDTO.FirstOrDefault().EnqSisId;
               
                var response = new ApiResultResponse();
                var content = new StringContent(JsonConvert.SerializeObject(listAssociateDetailsDTO), Encoding.UTF8, "application/json");
                var responseHttp = await _apiService.PostAsync($"SisterConcernDetails/UpdateSisterConcernDetails", content);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    List<SisterConcernDetailsDTO> dto = JsonConvert.DeserializeObject<List<SisterConcernDetailsDTO>>(response.Result.ToString());
                    var dtoItem = dto.FirstOrDefault();
                    _sessionManager.SetNewEnqTempId(dtoItem.EnqtempId.ToString());
                    if (_sessionManager.GetAssociateDetailsDTOList() != null)
                    {
                        var AssociateDetails = _sessionManager.GetAssociateDetailsDTOList();
                        var itemToRemove = AssociateDetails.FirstOrDefault(m => m.EnqSisId == oldEnqSisId);
                        AssociateDetails.Remove(itemToRemove);
                        _sessionManager.SetAssociateDetailsDTOList(AssociateDetails);
                    }
                    listAssociateDetailsDTO.ForEach(x =>
                    {
                        x.EnqSisId = dtoItem.EnqSisId;
                    });

                    listAssociateDetailsDTO.AddRange(_sessionManager.GetAssociateDetailsDTOList());
                    _sessionManager.SetAssociateDetailsDTOList(listAssociateDetailsDTO);

                    if (_sessionManager.GetAssociatePrevFYDetailsList() != null)
                    {
                        var FyDetail = _sessionManager.GetAssociatePrevFYDetailsList();
                        var itemToRemove = FyDetail.Where(m => m.EnqSisId == oldEnqSisId).Select(x => x).ToList();
                        var list = FyDetail.Where(x => x.EnqSisId == oldEnqSisId).Select(x => x).ToList();
                        list.ForEach(x =>
                        {
                            x.EnqSisId = dtoItem.EnqSisId;
                            x.EnqSis = dtoItem;
                        });
                        foreach (var item in itemToRemove)
                        {
                            FyDetail.Remove(item);
                        }
                        FyDetail.AddRange(list);
                        _sessionManager.SetAssociatePrevFYDetailsList(FyDetail);
                    }
                    return true;
                }

                return false;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateAssociateSisterConcernDetail()
        {
            var enqId = _sessionManager.GetNewEnqTempId();
            int newTempEnqId = 0;
            if (enqId != null)
            {
                newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
            }
           
            var response = new ApiResultResponse();
            var responseHttp = await _apiService.GetAsync($"SisterConcernDetails/UpdateAssociateSisterConcernStatus?enqId={newTempEnqId}");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                _sessionManager.SetNewEnqTempId(response.Result.ToString());
                return true;
            }
            return false;
        }
        public async Task<bool> SaveAssociateSisterFYDetails(List<SisterConcernFinancialDetailsDTO> listAssociateFYDetailsDTO)
        {
            try
            {

                int newTempEnqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                listAssociateFYDetailsDTO.ForEach(x => x.EnqtempId = newTempEnqId);
               
                var response = new ApiResultResponse();
                string UniqueId = listAssociateFYDetailsDTO.FirstOrDefault().UniqueId;
                if (string.IsNullOrEmpty(UniqueId))
                {
                    var content = new StringContent(JsonConvert.SerializeObject(listAssociateFYDetailsDTO), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"SisterConcernFinancialDetails/AddSisterConcernFinancialDetails", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        List<SisterConcernFinancialDetailsDTO> dto = JsonConvert.DeserializeObject<List<SisterConcernFinancialDetailsDTO>>(response.Result.ToString());
                        listAssociateFYDetailsDTO.ForEach(x => x.UniqueId = dto.FirstOrDefault().UniqueId);
                        _sessionManager.SetAssociatePrevFYDetailsList(listAssociateFYDetailsDTO);
                        return true;
                    }
                }
                else
                {
                    var content = new StringContent(JsonConvert.SerializeObject(listAssociateFYDetailsDTO), Encoding.UTF8, "application/json");
                    var responseHttp = await _apiService.PostAsync($"SisterConcernFinancialDetails/UpdateSisterConcernFinancialDetails", content);
                    if (responseHttp.StatusCode == HttpStatusCode.OK)
                    {
                        var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                        return true;
                    }
                }
                return false;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Save Project Details Service 
        /// </summary>
        /// <param name="enquiryDTO"></param>
        /// <returns></returns>

        public async Task<bool> saveCapitalDetails(ProjectWorkingCapitalDeatailsDTO capitalArrgmentDtls)
        {
            if (Convert.ToInt32(_sessionManager.GetNewEnqTempId()) != 0)
            {
                capitalArrgmentDtls.EnqtempId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
            }
           
            var content = new StringContent(JsonConvert.SerializeObject(capitalArrgmentDtls), Encoding.UTF8, "application/json");
            var responseHttp = await _apiService.PostAsync($"WorkingCapitalDetails/AddWorkingCapitalDetails", content);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var enquiryOBJ = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                ProjectWorkingCapitalDeatailsDTO data = JsonConvert.DeserializeObject<ProjectWorkingCapitalDeatailsDTO>(enquiryOBJ.Result.ToString());
                if (Convert.ToInt32(_sessionManager.GetNewEnqTempId()) == 0)
                {
                    _sessionManager.SetNewEnqTempId(data.EnqtempId.ToString());
                }
                return true;
            }
            return false;
        }
        public async Task<bool> saveProjectCostDetails(List<ProjectCostDetailsDTO> listprojectCosts)
        {
            var tempId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
            listprojectCosts.ForEach(x => x.EnqtempId = tempId);
           
            var content = new StringContent(JsonConvert.SerializeObject(listprojectCosts), Encoding.UTF8, "application/json");
            var responseHttp = await _apiService.PostAsync($"ProjectCost/AddProjectCostDetails", content);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> saveProjectMeansOfFinanceDetails(List<ProjectMeansOfFinanceDTO> listmeansOfFinance)
        {
            var tempId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
            listmeansOfFinance.ForEach(x => x.EnqtempId = tempId);
           
            var content = new StringContent(JsonConvert.SerializeObject(listmeansOfFinance), Encoding.UTF8, "application/json");
            var responseHttp = await _apiService.PostAsync($"MeansOfFinance/AddMeansOfFinance", content);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return true;
        }
        public async Task<bool> saveProjectPrevYearFinDetailsDetails(List<ProjectFinancialYearDetailsDTO> listprevFYDetails)
        {
            var tempId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
            listprevFYDetails.ForEach(x => x.EnqtempId = tempId);
           
            var content = new StringContent(JsonConvert.SerializeObject(listprevFYDetails), Encoding.UTF8, "application/json");
            var responseHttp = await _apiService.PostAsync($"FinancialPreYear/AddFinancePreYearDetails", content);

            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Save Security and Documents Service
        /// </summary>
        /// <param name="enquiryDTO"></param>
        /// <returns></returns>
        public async Task<JsonResult> SaveDocumentDetails(EnquiryDTO enquiryDTO)
        {
           
            var content = new StringContent(JsonConvert.SerializeObject(enquiryDTO), Encoding.UTF8, "application/json");
            var responseHttp = await _apiService.PostAsync($"EnquirySubmission/SaveDocumentDetails", content);

            return new JsonResult(enquiryDTO);
        }
        public async Task<List<SecurityDetailsDTO>> SaveSecurityDetails(List<SecurityDetailsDTO> listSecurityDetails)
        {
           
            var content = new StringContent(JsonConvert.SerializeObject(listSecurityDetails), Encoding.UTF8, "application/json");
            var responseHttp = await _apiService.PostAsync($"SecurityDetails/AddSecurityDetails", content);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var enquiryOBJ = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                List<SecurityDetailsDTO> data = JsonConvert.DeserializeObject<List<SecurityDetailsDTO>>(enquiryOBJ.Result.ToString());
                if (Convert.ToInt32(_sessionManager.GetNewEnqTempId()) == 0)
                {
                    _sessionManager.SetNewEnqTempId(data[0].EnqtempId.ToString());
                }
                return data;
            }
            return null;
        }
        public async Task<JsonResult> saveDeclarationDetails(Declaration declaration)
        {
           
            var content = new StringContent(JsonConvert.SerializeObject(declaration), Encoding.UTF8, "application/json");
            var responseHttp = await _apiService.PostAsync($"EnquirySubmission/saveDeclarationDetails", content);

            return new JsonResult(responseHttp);
        }
        public async Task<byte[]> ViewFile(string fileId)
        {
           
            var responseHttp = await _apiService.GetAsync($"Document/GetEncryptedFileById?documentId={fileId}");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var enquiryOBJ = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                FileDetailsDTO result = JsonConvert.DeserializeObject<FileDetailsDTO>(enquiryOBJ.Result.ToString());
                return result.Bytes.DecryptByteArray("password");
            }
            return null;
        }


        public async Task<bool> DeleteFile(int fileId)
        {
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.DeleteAsync($"Document/DeleteDocument?documentId={fileId}");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var enquiryOBJ = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                return (bool)enquiryOBJ.Result;
            }
            return false;
        }

        public async Task<(bool, string, int, string)> FileUpload(FileUploadDTO fileUpload)
        {
            fileUpload.Bytes.EnctypedByteArray("password");
           
            var content = new StringContent(JsonConvert.SerializeObject(fileUpload), Encoding.UTF8, "application/json");
            var responseHttp = await _apiService.PostAsync($"Document/FileUpload", content);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var enquiryOBJ = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                var data = JsonConvert.DeserializeObject<EnqDocumentDTO>(enquiryOBJ.Result.ToString());
                if (fileUpload.EnquiryId == 0)
                {
                    _sessionManager.SetNewEnqTempId(data.EnquiryId.ToString());
                }
                if (data != null)
                {
                    List<EnqDocumentDTO> documentlist = new();
                    if (_sessionManager.GetDocuments() != null)
                        documentlist = _sessionManager.GetDocuments();
                    documentlist.Add(data);
                    _sessionManager.SetDocuments(documentlist);
                }
                return (true, "File uploaded successfully", (int)data.Id, (string)data.UniqueId);

            }
            else if (responseHttp.StatusCode == HttpStatusCode.BadRequest)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var retrunData = JsonConvert.DeserializeObject<ApiResponse>(responseStrPG);
                return (false, retrunData.Message, 0, string.Empty);

            }
            return (false, "Some Unknown Error occured. This will resolved soon.", 0, string.Empty);
        }

        public async Task<JsonResult> saveDocuments(List<byte[]> EncryptedFilesList)
        {
            var client = _clientFactory.CreateClient("ksfcApi");
            foreach (var file in EncryptedFilesList)
            {
                var content = new ByteArrayContent(file);
                var responseHttp = await client.PostAsync($"EnquirySubmission/FileUpload", content);
                return new JsonResult(responseHttp);
            }
            return new JsonResult(null);
        }

        public async Task<int> SubmitEnquiry(string sNote)
        {
            EnquiryDetailsDto enquiryDetailsDto = new();
            enquiryDetailsDto.EnquiryId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
            enquiryDetailsDto.Note = sNote;
           
            var content = new StringContent(JsonConvert.SerializeObject(enquiryDetailsDto), Encoding.UTF8, "application/json");
            var responseHttp = await _apiService.PostAsync($"EnquiryHome/SubmitEnquiry", content);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseString = await responseHttp.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);

                var refNo = JsonConvert.DeserializeObject<int>(result.Result.ToString());
                if (refNo > 0)
                {
                    return refNo;
                }
            }
            return 0;
        }
        public async Task<EnquiryDTO> getEnquiryDetails(int enquiryId)
        {
            EnquiryDTO enquiryDetails = new EnquiryDTO();
           
            var responseHttp = await _apiService.GetAsync($"EnquiryHome/EnquirySummary?enqId={enquiryId}");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var enquiryOBJ = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                enquiryDetails = JsonConvert.DeserializeObject<EnquiryDTO>(enquiryOBJ.Result.ToString());
            }
            return enquiryDetails;
        }

        public async Task<bool> DeleteEnquiry(int id)
        {
            var Result = new ApiResultResponse();
           
            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var responseHttp = await _apiService.GetAsync($"EnquiryHome/DeleteEnquiry?enqId={id}");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                return true;
            }
            return false;
        }
        public async Task<bool> UpdateEnquiryStatus(int id, int EnqStatus)
        {
            var Result = new ApiResultResponse();
           
            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var responseHttp = await _apiService.GetAdminAsync($"EnquiryHome/UpdateEnquiryStatus?enqId={id}&EnqStatus={EnqStatus}");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                return true;
            }
            return false;
        }

        public async Task<JsonResult> DeleteEnquiryRegistrationDetails(int id)
        {
            var Result = new ApiResultResponse();
           
            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var responseHttp = await _apiService.PostAsync($"RegistrationDetails/DeleteRegistrationDetails?Id={id}", content);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                return new JsonResult(Result);
            }
            return new JsonResult(Result);
        }

        public async Task<JsonResult> DeleteEnquiryAddressDetails(int id)
        {
            var Result = new ApiResultResponse();
           
            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var responseHttp = await _apiService.PostAsync($"AddressDetails/DeleteAddressDetails?Id={id}", content);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                return new JsonResult(Result);
            }
            return new JsonResult(Result);
        }
        public async Task<bool> DeletePromotorDetails(int id)
        {
           
            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var responseHttp = await _apiService.PostAsync($"PromoterDetails/DeletePromoterDetails?Id={id}", content);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteGuarantor(int id)
        {
           
            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var responseHttp = await _apiService.PostAsync($"GuarantorDetails/DeleteGuarantorDetails?Id={id}", content);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteSisterConcernDetails(int id)
        {
           
            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            var responseHttp = await _apiService.PostAsync($"SisterConcernDetails/DeleteSisterConcernDetails?Id={id}", content);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public async Task<List<SelectListItem>> GetGenderTypes()
        {
            var GenderTypes = new List<SelectListItem>();

            await Task.Run(() =>
             {
                 GenderTypes = new List<SelectListItem>()
             {

                 new SelectListItem()
                 {
                     Text="Male",
                     Value="Male"
                 },
                  new SelectListItem()
                 {
                     Text="Female",
                     Value="Female"
                 },
                     new SelectListItem()
                 {
                     Text="Other",
                     Value="Other"
                 },
             };
             });

            return GenderTypes;

        }
    }
}
