using KAR.KSFC.Components.Common.Dto.Common;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Services.IServices.Admin;
using KAR.KSFC.Components.Data.Models.DbModels;

namespace KAR.KSFC.UI.Services.Admin.IDM
{
    public class IdmService : IIdmService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly SessionManager _sessionManager;
        public IdmService(IHttpClientFactory clientFactory, SessionManager sessionManager)
        {
            _clientFactory = clientFactory;
            _sessionManager = sessionManager;
        }

        public async Task<List<LoanAccountNumberDTO>> GetAllLoanNumber(string empID)
        {
            List<LoanAccountNumberDTO> listAccNo = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(RouteName.GetAccountNumber+empID);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                listAccNo = JsonConvert.DeserializeObject<List<LoanAccountNumberDTO>>(result.Result.ToString());
            }
            return listAccNo;
        }

        public async Task<List<TblUnitDet>> GetAccountDetails()
        {
            List<TblUnitDet> Details  = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(RouteName.unitdetails);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                Details = JsonConvert.DeserializeObject<List<TblUnitDet>>(result.Result.ToString());
            }
            return Details;
        }
        #region Generic Dropdowns
        public async Task<IEnumerable<DropDownDTO>> GetAllConditionStages()
        {
            List<DropDownDTO> listConditionStages = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(GenericDDRoute.GetAllConditionStages);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                listConditionStages = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return listConditionStages;
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllConditionStageMaster()
        {
            List<DropDownDTO> listCondstage = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(GenericDDRoute.GetAllConditionStageMaster);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                listCondstage = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return listCondstage;
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllConditionTypes()
        {
            List<DropDownDTO> listConditionTypes = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(GenericDDRoute.GetAllConditionTypes);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                listConditionTypes = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return listConditionTypes;
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllForm8AndForm13Master()
        {
            List<DropDownDTO> listForm8and13 = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync("Common/GetAllForm8AndForm13Master");
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                listForm8and13 = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return listForm8and13;
        }


        public async Task<IEnumerable<DropDownDTO>> GetAllPromotorNames()
        {
            List<DropDownDTO> listPromotorNames = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(GenericDDRoute.GetAllPromoterNames);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                listPromotorNames = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return listPromotorNames;
        }
        public async Task<IEnumerable<DropDownDTO>> GetAllPromoterPhNo()
        {
            List<DropDownDTO> listPromoterPhNo = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(GenericDDRoute.GetAllPromoterPhNo);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                listPromoterPhNo = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return listPromoterPhNo;
        }

        public async Task<IEnumerable<DropDownDTO>> GetPositionDesignationAsync()
        {
            List<DropDownDTO> lstPromoterDesignation = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(GenericDDRoute.GetPositionDesignation);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                lstPromoterDesignation = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return lstPromoterDesignation;
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllPromotorClass()
        {
            List<DropDownDTO> lstPromoterClass = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(GenericDDRoute.GetAllPromotorClass);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                lstPromoterClass = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return lstPromoterClass;
        }

        public async Task<IEnumerable<DropDownDTO>> GetDomicileStatusAsync()
        {
            List<DropDownDTO> lstPromoterDomicile = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(GenericDDRoute.GetDomicileStatus);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                lstPromoterDomicile = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return lstPromoterDomicile;
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllDistrictNames()
        {
            List<DropDownDTO> listDistrictNames = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(GenericDDRoute.GetAllDistrictDetails);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                listDistrictNames = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return listDistrictNames;
        }
        public async Task<IEnumerable<DropDownDTO>> GetAllTalukNames()
        {
            List<DropDownDTO> listTalukNames = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(GenericDDRoute.GetAllTalukDetails);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                listTalukNames = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return listTalukNames;
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllHobliNames()
        {
            List<DropDownDTO> listHobliNames = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(GenericDDRoute.GetAllHobliDetails);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                listHobliNames = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return listHobliNames;
        }
        public async Task<IEnumerable<DropDownDTO>> GetAllVillageNames()
        {
            List<DropDownDTO> listVillageNames = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(GenericDDRoute.GetAllVillageDetails);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                listVillageNames = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return listVillageNames;
        }

        public async Task<IEnumerable<DropDownDTO>> GetAccountTypeAsync()
        {
            List<DropDownDTO> lstPromoterAccount = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(GenericDDRoute.GetAllAccountType);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                lstPromoterAccount = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return lstPromoterAccount;
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllocationCodes()
        {
            List<DropDownDTO> lstAllocationCodes = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(GenericDDRoute.GetAllAllocationCode);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                lstAllocationCodes = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return lstAllocationCodes;
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllLandType()
        {
            List<DropDownDTO> lstLandType = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(GenericDDRoute.GetAllLandType);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                lstLandType = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return lstLandType;
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllOtherDebitsDetails()
        {
            List<DropDownDTO> lstOtherDebits = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(GenericDDRoute.GetAllOtherDebitsDetails);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                lstOtherDebits = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return lstOtherDebits;
        }
         public async Task<IEnumerable<DropDownDTO>> GetAllUomMaster()
        {
            List<DropDownDTO> lstUomMaster = new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(GenericDDRoute.GetAllUomMasterDetails);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                lstUomMaster = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return lstUomMaster;
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllAssetRefList()
        {
            List<DropDownDTO> AssetList= new();
            var client = _clientFactory.CreateClient("ksfcApi");
            var responseHttp = await client.GetAsync(GenericDDRoute.GetAllUomMasterDetails);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var apiResponse = await responseHttp.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                AssetList = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
            }
            return AssetList;
        }

        





        #endregion

    }
}
