using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.LoanAccounting;
using KAR.KSFC.Components.Common.Security;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace KAR.KSFC.UI.Services.Admin.IDM
{
    public class CommonService : ICommonService
    {
        private readonly IHttpClientFactory _clientFactory;

        public CommonService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<bool> DeleteFile(string fileId, string mainModule)
        {
            var client = _clientFactory.CreateClient(Constants.client);
            var responseHttp = await client.DeleteAsync(Constants.idmdocFile + fileId+ Constants.mainModule + mainModule);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var enquiryOBJ = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                return (bool)enquiryOBJ.Result;
            }
            return false;
        }

        public async Task<List<ldDocumentDto>> FileList(string MainModule)
        {
            List<ldDocumentDto> fileList = new();
            var client = _clientFactory.CreateClient(Constants.client);
            var responseHttp = await client.GetAsync(Constants.getFileList + MainModule);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                fileList = JsonConvert.DeserializeObject<List<ldDocumentDto>>(result.Result.ToString());
            }
            return fileList;

        }

        public async Task<(bool, string, int, string)> UploadDocument(IdmFileUploadDto item)
        {
            item.Bytes.EnctypedByteArray(Constants.Password);
            var client = _clientFactory.CreateClient(Constants.client);
            var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8,  Constants.ContentType);
            var responseHttp = await client.PostAsync(Constants.idmDocUpload, content);
           
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var enquiryOBJ = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                var data = JsonConvert.DeserializeObject<ldDocumentDto>(enquiryOBJ.Result.ToString());
              
                return (true, Constants.fileuploaded, (int)data.Id, (string)data.UniqueId);

            }
            else if (responseHttp.StatusCode == HttpStatusCode.BadRequest)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var retrunData = JsonConvert.DeserializeObject<ApiResponse>(responseStrPG);
                return (false, retrunData.Message, 0, string.Empty);

            }
            return (false, Constants.errorOccuredfileUpload, 0, string.Empty);
        }

        public async Task<byte[]> ViewFile(string fileId, string mainModule)
        {
            var client = _clientFactory.CreateClient(Constants.client);
            var responseHttp = await client.GetAsync(Constants.idmGetfile+ fileId + Constants.mainModule + mainModule);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var enquiryOBJ = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                IdmFileUploadDto result = JsonConvert.DeserializeObject<IdmFileUploadDto>(enquiryOBJ.Result.ToString());
                return result.Bytes.DecryptByteArray(Constants.Password);
            }
            return null;
        }
        public async Task<IdmDDLListDTO> GetAllIdmDropDownList()
        {
            IdmDDLListDTO idmDTO = new IdmDDLListDTO();
            idmDTO = new IdmDDLListDTO();

            //Get SecurityList DD
            var client = _clientFactory.CreateClient(Constants.client);
            var responseHttp = await client.GetAsync(RouteName.GetAllSecurityCategory);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllSecurityCategory = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllSecurityCategory = new List<SelectListItem>();
            }
            //Get All Bank IFSC CODE DD
            responseHttp = await client.GetAsync(RouteName.GetAllBankIfscCode);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllIfscCode = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllIfscCode = new List<SelectListItem>();
            }
            //Get All Charge Type 
            responseHttp = await client.GetAsync(RouteName.GetAllChargeType);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllChargeType = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllChargeType = new List<SelectListItem>();
            }
            //Get all Security Types
            responseHttp = await client.GetAsync(RouteName.GetAllSecurityTypes);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllSecurityTypes = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllSecurityTypes = new List<SelectListItem>();
            }
            //Get all SubRegister Office
            responseHttp = await client.GetAsync(RouteName.GetAllSubRegistrarOffice);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllSubRegistrarOffice = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllSubRegistrarOffice = new List<SelectListItem>();
            }
            //Get All Asset Type
            responseHttp = await client.GetAsync(RouteName.GetAllAssetTypes);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllAssetTypes = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllAssetTypes = new List<SelectListItem>();
            }
            //Get All Condition Type
            responseHttp = await client.GetAsync(RouteName.GetAllConditionTypes);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllConditionTypes = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllConditionTypes = new List<SelectListItem>();
            }
            //Get All Condition Stage
            responseHttp = await client.GetAsync(RouteName.GetAllConditionStages);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllConditionStages = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllConditionStages = new List<SelectListItem>();
            }


            responseHttp = await client.GetAsync(RouteName.GetAllConditionStageMaster);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllConditionStagesMaster = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllConditionStagesMaster = new List<SelectListItem>();
            }

            //Get All Condition Description
            responseHttp = await client.GetAsync(RouteName.GetAllConditionDescriptions);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllConditionDescriptions = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllConditionDescriptions = new List<SelectListItem>();
            }
            // Get All type Of Form
            responseHttp = await client.GetAsync(RouteName.GetAllForm8AndForm13Master);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllForm8andForm13Master = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllForm8andForm13Master = new List<SelectListItem>();
            }

            responseHttp = await client.GetAsync(RouteName.GetAllStateZone);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllStateZone = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllStateZone = new List<SelectListItem>();
            }

            // Get All Project Cost Components Details
            responseHttp = await client.GetAsync(RouteName.GetPojectCostComponentAsync);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllProjectCostComponents = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllProjectCostComponents = new List<SelectListItem>();
            }


            #region Promoter Address
            responseHttp = await client.GetAsync(GenericDDRoute.GetAllPromoterNames);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllPromoterNames = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllPromoterNames = new List<SelectListItem>();

            }

            responseHttp = await client.GetAsync(GenericDDRoute.GetAllPromoterDistrict);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllPromoterDistricts = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllPromoterDistricts = new List<SelectListItem>();
            }

            responseHttp = await client.GetAsync(GenericDDRoute.GetAllPromoterState);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllPromoterState = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllPromoterState = new List<SelectListItem>();
            }
            #endregion

            #region  Unit Information Address Details 
            responseHttp = await client.GetAsync(GenericDDRoute.GetAllDistrictDetails);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllDistrictDetails = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllDistrictDetails = new List<SelectListItem>();
            }

            responseHttp = await client.GetAsync(GenericDDRoute.GetAllTalukDetails);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllTalukDetails = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllTalukDetails = new List<SelectListItem>();
            }

            responseHttp = await client.GetAsync(GenericDDRoute.GetAllHobliDetails);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllHobliDetails = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllHobliDetails = new List<SelectListItem>();
            }

            responseHttp = await client.GetAsync(GenericDDRoute.GetAllVillageDetails);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllVillageDetails = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllVillageDetails = new List<SelectListItem>();
            }

            responseHttp = await client.GetAsync(GenericDDRoute.GetAllPincodeDetails);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != Constants.brackets)
                    idmDTO.AllPincodeDetails = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllPincodeDetails = new List<SelectListItem>();
            }
            #endregion

            // BY Dev on 01/09/2022
            //Get all Promoter Designation
            responseHttp = await client.GetAsync(RouteName.GetPositionDesignation);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != Constants.brackets)
                    idmDTO.AllPositionDesignation = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllPositionDesignation = new List<SelectListItem>();
            }

            // BY Dev on 01/09/2022
            //Get all Promoter Domicile
            responseHttp = await client.GetAsync(RouteName.GetDomicileStatus);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllDomicileStatus = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllDomicileStatus = new List<SelectListItem>();
            }

            // BY Dev on 01/09/2022
            //Get all Promoter Class
            responseHttp = await client.GetAsync(RouteName.GetAllPromoterClass);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllPromotorClass = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllPromotorClass = new List<SelectListItem>();
            }

            // BY Dev on 03/09/2022
            //Get all Promoter Sub Class
            responseHttp = await client.GetAsync(RouteName.GetAllPromoterSubClass);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllPromotorSubClass = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllPromotorSubClass = new List<SelectListItem>();
            }

            // BY Dev on 03/09/2022
            //Get all Promoter Qualification
            responseHttp = await client.GetAsync(RouteName.GetAllPromoterQualification);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllPromotorQual = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllPromotorQual = new List<SelectListItem>();
            }

            responseHttp = await client.GetAsync(Constants.getproductName);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.allProducdetailsMaster = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.allProducdetailsMaster = new List<SelectListItem>();
            }

            responseHttp = await client.GetAsync(Constants.getIndustry);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.allindustrydetailsMaster = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.allindustrydetailsMaster = new List<SelectListItem>();
            }

            responseHttp = await client.GetAsync(Constants.getAssetType);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllAssetTypeMaster = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllAssetTypeMaster = new List<SelectListItem>();
            }

            responseHttp = await client.GetAsync(Constants.getAssetCat);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllAssetCategoryMaster = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllAssetCategoryMaster = new List<SelectListItem>();
            }

            // BY Dev on 06/09/2022
            //Get all Promoter Account
            responseHttp = await client.GetAsync(RouteName.GetAllAccountType);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllAccountType = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllAccountType = new List<SelectListItem>();
            }

            // Get all Allocation Codes
            responseHttp = await client.GetAsync(RouteName.GetAllAllocationCode);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.GetAllAllocationCode = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.GetAllAllocationCode = new List<SelectListItem>();
            }

            // Get all Land Types  AllOtherDebitsDetails
            responseHttp = await client.GetAsync(RouteName.GetAllLandType);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    idmDTO.AllLandType = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllLandType = new List<SelectListItem>();
            }

            // Get All Other Debits Details  
            responseHttp = await client.GetAsync(RouteName.GetAllOtherDebitsDetails);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != Constants.brackets)
                    idmDTO.AllOtherDebitsDetails = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllOtherDebitsDetails = new List<SelectListItem>();
            }

            responseHttp = await client.GetAsync(RouteName.GetAllUomMaster);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != Constants.brackets)
                    idmDTO.AllUmoMasterDetails = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllUmoMasterDetails = new List<SelectListItem>();
            }

            responseHttp = await client.GetAsync(RouteName.GetMeansofFinanceCategoryAsync);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != Constants.brackets)
                    idmDTO.AllFinanceCategory = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllFinanceCategory = new List<SelectListItem>();
            }

            responseHttp = await client.GetAsync(RouteName.GetAllConstitution);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != Constants.brackets)
                    idmDTO.AllConstutionDetails = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllConstutionDetails = new List<SelectListItem>();
            }

            responseHttp = await client.GetAsync(RouteName.GetAllDeptMaster);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != Constants.brackets)
                    idmDTO.AllDeptMaster = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllDeptMaster = new List<SelectListItem>();
            }

             responseHttp = await client.GetAsync(RouteName.GetAllDsbChargeMaping);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() != Constants.brackets)
                    idmDTO.AllDsbChargeMap = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    idmDTO.AllDsbChargeMap = new List<SelectListItem>();
            }


            return idmDTO;
        }

        public async Task<LA_DDLListDTO> GetAllLADropDownList()
        {
            LA_DDLListDTO la_DDLListDTO = new();
            la_DDLListDTO = new LA_DDLListDTO();

            var client = _clientFactory.CreateClient(Constants.client);

            var responseHttp = await client.GetAsync(LoanAccountingGenericDDRoute.GetAllTransactionTypes);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                string responseStr = await responseHttp.Content.ReadAsStringAsync();
                var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseStr);
                if (successObj.Result.ToString() !=Constants.brackets)
                    la_DDLListDTO.AllTransactionTypes = JsonConvert.DeserializeObject<List<SelectListItem>>(successObj.Result.ToString());
                else
                    la_DDLListDTO.AllTransactionTypes = new List<SelectListItem>();
            }

            return la_DDLListDTO;
        }
    }
}
