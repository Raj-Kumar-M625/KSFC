using KAR.KSFC.Components.Common.Dto.Common;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfDisbursmentProposal;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IInspectionOfUnitService;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.Admin.IDM.InspectionOfUnitService
{
    public class InspectionOfUnitService : IInspectionOfUnitService 
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _logger;
        public InspectionOfUnitService(IHttpClientFactory clientFactory, ILogger logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        //Author: Manoj Date:25/08/2022
        public async Task<List<LoanAccountNumberDTO>> GetAllLoanNumber()
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllLoanNumber);
                List<LoanAccountNumberDTO> listAccNo = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(InspectionOfUnitRoute.GetAccountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    listAccNo = JsonConvert.DeserializeObject<List<LoanAccountNumberDTO>>(result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllLoanNumber);
                return listAccNo;  
            }
            catch (Exception ex)
            {
                _logger.Error(Error.DeleCersai + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        #region Land Inspection
        //<summary>
        // Author: Manoj; Module:Land Inspection; Date:25/08/2022
        //Modified: Gagana K Added try catch with Loggings Date:19/10/2022
        //</summary
        public async Task<IEnumerable<IdmDchgLandDetDTO>> GetAllLandInspectionList(long accountNumber, long InspectionId)
        {
            _logger.Information(CommonLogHelpers.GetAllLandInspectionList);
            List<IdmDchgLandDetDTO> listLandInspectionDetails = new();
             var client = _clientFactory.CreateClient(Constants.client);
            var responseHttp = await client.GetAsync(InspectionOfUnitRoute.GetAllLandInspectionList + accountNumber + "&&InspectionId=" + InspectionId);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                listLandInspectionDetails = JsonConvert.DeserializeObject<List<IdmDchgLandDetDTO>>(Result.Result.ToString());
            }
            _logger.Information(CommonLogHelpers.CompletedGetAllLandInspectionList);
            return listLandInspectionDetails;
        }
        //<summary>
        // Author: Manoj; Module:Land Inspection; Date:25/08/2022
        //Modified: Gagana K Added try catch with Loggings Date:19/10/2022
        //</summary
        public async Task<bool> UpdateLandInspectionDetails(IdmDchgLandDetDTO Inspection)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedLandInspectionUpdate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(Inspection), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.UpdateLandInspectionDetails, httpContent);
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
                _logger.Error(Error.updateLandInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }


        public async Task<bool> DeleteLandInspectionDetails(IdmDchgLandDetDTO dto)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedLandInspectionDelete);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.DeleteLandInspectionDetails, httpContent);
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
                _logger.Error(Error.deleteLandInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> CreateLandInspectionDetails(IdmDchgLandDetDTO addr)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedLandInspectionCreate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.CreateLandInspectionDetails, httpContent);
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
                _logger.Error(Error.createLandInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        #endregion

        #region Building Inspection
        //<summary>
        // Author: Swetha; Module:Building Inspection; Date:25/08/2022
        //Modified: Gagana K Added try catch with Loggings Date:19/10/2022
        //</summary
        public async Task<IEnumerable<IdmDchgBuildingDetDTO>> GetAllBuildingnspectionList(long accountNumber,long InspectionId)
        {
            _logger.Information(CommonLogHelpers.GetAllBuildingnspectionList);
            List<IdmDchgBuildingDetDTO> listLandInspectionDetails = new();
             var client = _clientFactory.CreateClient(Constants.client);
            var responseHttp = await client.GetAsync(InspectionOfUnitRoute.GetAllBuildingInspectionList + accountNumber + "&&InspectionId=" + InspectionId);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                listLandInspectionDetails = JsonConvert.DeserializeObject<List<IdmDchgBuildingDetDTO>>(Result.Result.ToString());
            }
            _logger.Information(CommonLogHelpers.CompletedGetAllBuildingnspectionList);
            return listLandInspectionDetails;
        }
        public async Task<bool> CreateBuildingInspectionDetails(IdmDchgBuildingDetDTO buildingInspection)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedBuildingInspectionCreate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(buildingInspection), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.CreateBuildingInspectionDetails, httpContent);
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
                _logger.Error(Error.createBuildingInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        public async Task<bool> UpdateBuildingInspectionDetails(IdmDchgBuildingDetDTO buildingInspection)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedBuildingInspectionUpdate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(buildingInspection), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.UpdateBuildingInspectionDetails, httpContent);
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
                _logger.Error(Error.updateBuildingInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }

        public async Task<bool> DeleteBuildingInspectionDetails (IdmDchgBuildingDetDTO buildingInspection)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedBuildingInspectionDelete);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(buildingInspection), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.DeleteBuildingInspectionDetails, httpContent);
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
                _logger.Error(Error.deleteBuildingInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        #endregion

      

        #region InspectionDetials
        //<summary>
        // Author: Sandeep M; Module:Inspection Detail; Date:25/08/2022
        //Modified: Gagana K Added try catch with Loggings Date:19/10/2022
        //</summary
        public async Task<IEnumerable<IdmDspInspDTO>> GetAllInspectionDetailsList(long accountNumber)
        {
            try
            {
                _logger.Information(CommonLogHelpers.GetAllInspectionDetailsList);
                List<IdmDspInspDTO> InspectionDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(InspectionOfUnitRoute.GetAllInspectionDetailsList + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    InspectionDetails = JsonConvert.DeserializeObject<List<IdmDspInspDTO>>(Result.Result.ToString());
                }
                _logger.Information(CommonLogHelpers.CompletedGetAllInspectionDetailsList);
                return InspectionDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.getInspectionList + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }


        //<summary>
        // Author: Sandeep M; Module:Inspection Detail; Date:25/08/2022
        //</summary
        public async Task<bool> UpdateInspectionDetails(IdmDspInspDTO landInspection)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedInspectionDetailsUpdate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(landInspection), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.UpdateInspectionDetails, httpContent);
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
                _logger.Error(Error.updateInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        //<summary>
        // Author: Sandeep M; Module:Inspection Detail; Date:25/08/2022
        //</summary
        public async Task<bool> DeleteInspectionDetails(IdmDspInspDTO dto)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedInspectionDetailsDelete);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.DeleteInspectionDetails, httpContent);
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
                _logger.Error(Error.deleteInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        //<summary>
        // Author: Sandeep M; Module:Inspection Detail; Date:25/08/2022
        //</summary
        public async Task<bool> CreateInspectionDetails(IdmDspInspDTO addr)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedInspectionDetailsCreate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.CreateInspectionDetails, httpContent);
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
                _logger.Error(Error.createInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        #endregion

        #region FurnitureInspection

        //<summary>
        // Author: Sandeep M; Module:Inspection Detail; Date:25/08/2022
        //Modified: Gagana K Added try catch with Loggings Date:19/10/2022
        //</summary
        public async Task<IEnumerable<IdmDChgFurnDTO>> GetAllFurnitureInspectionList(long accountNumber,long InspectionId)
        {
            _logger.Information(CommonLogHelpers.GetAllFurnitureInspectionList);
            List<IdmDChgFurnDTO> FurnitureInspectionDetails = new();
             var client = _clientFactory.CreateClient(Constants.client);
            var responseHttp = await client.GetAsync(InspectionOfUnitRoute.GetAllFurnitureInspectionDetailsList + accountNumber + "&&InspectionId=" + InspectionId);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                FurnitureInspectionDetails = JsonConvert.DeserializeObject<List<IdmDChgFurnDTO>>(Result.Result.ToString());
            }
            _logger.Information(CommonLogHelpers.CompletedGetAllFurnitureInspectionList);
            return FurnitureInspectionDetails;
        }

        //<summary>
        // Author: Sandeep M; Module:Furniture Inspection; Date:02/09/2022
        //</summary
        public async Task<bool> UpdateFurnitureInspectionDetails(IdmDChgFurnDTO Inspection)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedFurnitureInspectionUpdate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(Inspection), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.UpdateFurnitureInspectionDetails, httpContent);
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
                _logger.Error(Error.updateFurnitureInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }


        public async Task<bool> DeleteFurnitureInspectionDetails(IdmDChgFurnDTO dto)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedFurnitureInspectionDelete);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.DeleteFurnitureInspectionDetails, httpContent);
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
                _logger.Error(Error.deleteFurnitureInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> CreateFurnitureInspectionDetails(IdmDChgFurnDTO addr)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedFurnitureInspectionCreate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.CreateFurnitureInspectionDetails, httpContent);
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
                _logger.Error(Error.createFurnitureInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        #endregion
        #region Building Material at Site Inspection
        //Author Manoj on 29/08/2022
        //Modified: Gagana K Added try catch with Loggings Date:19/10/2022
        public async Task<IEnumerable<IdmBuildingMaterialSiteInspectionDTO>> GetAllBuildingMaterialInspectionList(long accountNumber, long InspectionId)
        {
            _logger.Information(CommonLogHelpers.GetAllBuildingMaterialInspectionList);
            List<IdmBuildingMaterialSiteInspectionDTO> InspectionDetails = new();
             var client = _clientFactory.CreateClient(Constants.client);
            var responseHttp = await client.GetAsync(InspectionOfUnitRoute.GetAllBuildingMaterialInspectionList + accountNumber + "&&InspectionId=" + InspectionId);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                InspectionDetails = JsonConvert.DeserializeObject<List<IdmBuildingMaterialSiteInspectionDTO>>(Result.Result.ToString());
            }
            _logger.Information(CommonLogHelpers.CompletedGetAllBuildingMaterialInspectionList);
            return InspectionDetails;
        }

        public async Task<bool> UpdateBuildMatSiteInspectionDetails(IdmBuildingMaterialSiteInspectionDTO bildMatInspection)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedBuildMatSiteUpdate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(bildMatInspection), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.UpdateBuildMatSiteInspectionDetails, httpContent);
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
                _logger.Error(Error.updateBuildingMaterialInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }

        public async Task<bool> DeleteBuildMatSiteInspectionDetails(IdmBuildingMaterialSiteInspectionDTO dto)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedBuildMatSiteDelete);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.DeleteBuildMatSiteInspectionDetails, httpContent);
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
                _logger.Error(Error.deleteBuildingMaterialInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }

        public async Task<bool> CreateBuildMatSiteInspectionDetails(IdmBuildingMaterialSiteInspectionDTO addr)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedBuildMatSiteCreate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.CreateBuildMatSiteInspectionDetails, httpContent);
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
                _logger.Error(Error.createBuildingMaterialInspection+ ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        #endregion
        #region Indigenous Machinery Inspection
        public async Task<IEnumerable<IdmDchgIndigenousInspectionDTO>> GetAllIndigenousMachineryInspectionList(long accountNumber, long InspectionId)
        {
            _logger.Information(CommonLogHelpers.GetAllIndigenousMachineryInspectionList);
            List<IdmDchgIndigenousInspectionDTO> IndigenousMachineryInspectionDetails = new();
             var client = _clientFactory.CreateClient(Constants.client);
            var responseHttp = await client.GetAsync(InspectionOfUnitRoute.GetAllIndigenousMachineryInspectionList + accountNumber + "&&InspectionId=" + InspectionId);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                IndigenousMachineryInspectionDetails = JsonConvert.DeserializeObject<List<IdmDchgIndigenousInspectionDTO>>(Result.Result.ToString());
            }
            _logger.Information(CommonLogHelpers.CompletedGetAllIndigenousMachineryInspectionList);
            return IndigenousMachineryInspectionDetails;
        }
        public async Task<IEnumerable<TblMachineryStatusDto>> GetAllMachineryStatusList()
        {
            _logger.Information(CommonLogHelpers.GetAllMachineryStatusList);
            List<TblMachineryStatusDto> MachinaryList = new();
            var client = _clientFactory.CreateClient(Constants.client);
            var responseHttp = await client.GetAsync(InspectionOfUnitRoute.GetAllMachineryStatusList);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                MachinaryList = JsonConvert.DeserializeObject<List<TblMachineryStatusDto>>(Result.Result.ToString());
            }
            _logger.Information(CommonLogHelpers.CompletedGetAllMachineryStatusList);
            return MachinaryList;
        }
         public async Task<IEnumerable<TblProcureMastDto>> GetAllProcureList()
        {
            _logger.Information(CommonLogHelpers.GetAllProcureList);
            List<TblProcureMastDto> ProcureList = new();  
            var client = _clientFactory.CreateClient(Constants.client);
            var responseHttp = await client.GetAsync(InspectionOfUnitRoute.GetAllProcureList);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                ProcureList = JsonConvert.DeserializeObject<List<TblProcureMastDto>>(Result.Result.ToString());
            }
            _logger.Information(CommonLogHelpers.CompletedGetAllProcureList);
            return ProcureList;
         }

          public async Task<IEnumerable<TblCurrencyMastDto>> GetAllCurrencyList()
          {
            _logger.Information(CommonLogHelpers.GetAllCurrencyList);
            List<TblCurrencyMastDto> CurrencyList = new();
            var client = _clientFactory.CreateClient(Constants.client);
            var responseHttp = await client.GetAsync(InspectionOfUnitRoute.GetAllCurrencyList);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                CurrencyList = JsonConvert.DeserializeObject<List<TblCurrencyMastDto>>(Result.Result.ToString());
            }
            _logger.Information(CommonLogHelpers.CompletedGetAllCurrencyList);
            return CurrencyList;
          }

        public async Task<bool> UpdateIndigenousMachineryInspectionDetails(IdmDchgIndigenousInspectionDTO bildMatInspection)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedIndigenousMachineryInspectionUpdate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(bildMatInspection), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.UpdatIndigenousMachineryInspectionDetails, httpContent);
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
                _logger.Error(Error.updateIndigenousMachineryInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> DeleteIndigenousMachineryInspectionDetails(IdmDchgIndigenousInspectionDTO dto)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedIndigenousMachineryInspectionDelete);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.DeleteIndigenousMachineryInspectionDetails, httpContent);
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
                _logger.Error(Error.deleteIndigenousMachineryInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> CreateIndigenousMachineryInspectionDetails(IdmDchgIndigenousInspectionDTO addr)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedIndigenousMachineryInspectionCreate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.CreateIndigenousMachineryInspectionDetails, httpContent);
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
                _logger.Error(Error.createIndigenousMachineryInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        #endregion

      

        #region Import Machinery Inspection
        //<summary>
        // Author: Swetha; Module:Import Machinery Inspection; Date:25/08/2022
        //Modified: Gagana K Added try catch with Loggings Date:19/10/2022
        //</summary
        public async Task<IEnumerable<IdmDchgImportMachineryDTO>> GetAllImportMachineryList(long accountNumber, long InspectionId)
        {
            _logger.Information(CommonLogHelpers.GetAllImportMachineryList);
            List<IdmDchgImportMachineryDTO> listImportMachineryDto = new();
             var client = _clientFactory.CreateClient(Constants.client);
            var responseHttp = await client.GetAsync(InspectionOfUnitRoute.GetAllImportMachineryInspectionList + accountNumber + "&&InspectionId=" + InspectionId);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                listImportMachineryDto = JsonConvert.DeserializeObject<List<IdmDchgImportMachineryDTO>>(Result.Result.ToString());
            }
            _logger.Information(CommonLogHelpers.CompletedGetAllImportMachineryList);
            return listImportMachineryDto;
        }
        public async Task<bool> CreateImportMachineryDetails(IdmDchgImportMachineryDTO importMachinery)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedImportMachineryCreate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(importMachinery), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.CreateImportMachineryInspectionDetails, httpContent);
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
                _logger.Error(Error.createImportMachineryInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        public async Task<bool> UpdateImportMachineryDetails(IdmDchgImportMachineryDTO importMachinery)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedImportMachineryUpdate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(importMachinery), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.UpdateImportMachineryInspectionDetails, httpContent);
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
                _logger.Error(Error.updateImportMachineryInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> DeleteImportMachineryDetails(IdmDchgImportMachineryDTO importMachinery)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedImportMachineryDelete);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(importMachinery), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.DeleteImportMachineryInspectionDetails, httpContent);
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
                _logger.Error(Error.deleteImportMachineryInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
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


        #region Status of Implementaion

        public async Task<IEnumerable<IdmDsbStatImpDTO>> GetTblDsbStatImps(long accountNumber, long InspectionId)
        {
            _logger.Information(CommonLogHelpers.GetAllStatusofImplementationList);
            List<IdmDsbStatImpDTO> idmDsbStatImpDTOs = new();
            var client = _clientFactory.CreateClient(Constants.client);
            var responseHttp = await client.GetAsync(InspectionOfUnitRoute.GetAllStatusofImplementationList + accountNumber + "&&InspectionId=" + InspectionId);
            if (responseHttp.StatusCode == HttpStatusCode.OK)
            {
                var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                idmDsbStatImpDTOs = JsonConvert.DeserializeObject<List<IdmDsbStatImpDTO>>(Result.Result.ToString());
            }
            _logger.Information(CommonLogHelpers.CompletedAllStatusofImplementationList);
            return idmDsbStatImpDTOs;
        }
        public async Task<bool> CreateStatusofImplementation(IdmDsbStatImpDTO statusImp)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedStatusofImplementationCreate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(statusImp), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.CreateStatusofImplementationDetails, httpContent);
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
                _logger.Error(Error.createStatusofImplementation + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        public async Task<bool> UpdateStatusofImplementation(IdmDsbStatImpDTO implementation)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedStatusofImplementationUpdate);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(implementation), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.UpdateStatusofImplementationDetails, httpContent);
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
                _logger.Error(Error.updateStatusofImplementation + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> DeleteStatusofImplementation(IdmDsbStatImpDTO impDTO)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedStatusofImplementationDelete);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(impDTO), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(InspectionOfUnitRoute.DeleteStatusofImplementationDetails, httpContent);
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
                _logger.Error(Error.deleteStatusofImplementation + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        #endregion
        //#region Project Cost 
        //public async Task<IEnumerable<IdmDchgProjectCostDTO>> GetAllProjectCostDetailsList(long accountNumber, long ProjectCostID)
        //{
        //    try
        //    {
        //        _logger.Information(CommonLogHelpers.GetAllProjectCostDetailsList);
        //        List<IdmDchgProjectCostDTO> ProjectCostDetails = new();
        //        var client = _clientFactory.CreateClient(Constants.client);
        //        var responseHttp = await client.GetAsync(InspectionOfUnitRoute.GetAllProjectCostDetailsList + accountNumber + "&&InspectionId=" + ProjectCostID);
        //        if (responseHttp.StatusCode == HttpStatusCode.OK)
        //        {
        //            var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
        //            var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
        //            ProjectCostDetails = JsonConvert.DeserializeObject<List<IdmDchgProjectCostDTO>>(Result.Result.ToString());
        //        }
        //        _logger.Information(CommonLogHelpers.CompletedGetAllProjectCostDetailsList);
        //        return ProjectCostDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(Error.getProjectCost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        throw;
        //    }

        //}

        //public async Task<bool> UpdateProjectCostDetails(IdmDchgProjectCostDTO ProjectCost)
        //{
        //    try
        //    {
        //        _logger.Information(CommonLogHelpers.StartedProjectCostUpdate);
        //        var client = _clientFactory.CreateClient(Constants.client);
        //        var httpContent = new StringContent(JsonConvert.SerializeObject(ProjectCost), Encoding.UTF8, Constants.ContentType);
        //        var responseHttp = await client.PostAsync(InspectionOfUnitRoute.UpdateProjectCostDetails, httpContent);
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
        //        _logger.Error(Error.updateProjectCost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        throw;
        //    }

        //}

        //public async Task<bool> DeleteProjectCostDetails(IdmDchgProjectCostDTO dto)
        //{
        //    try
        //    {
        //        _logger.Information(CommonLogHelpers.StartedProjectCostDelete);
        //        var client = _clientFactory.CreateClient(Constants.client);
        //        var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, Constants.ContentType);
        //        var responseHttp = await client.PostAsync(InspectionOfUnitRoute.DeleteProjectCostDetails, httpContent);
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
        //        _logger.Error(Error.deleteProjectCost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        throw;
        //    }

        //}

        //public async Task<bool> CreateProjectCostDetails(IdmDchgProjectCostDTO addr)
        //{
        //    try
        //    {
        //        _logger.Information(CommonLogHelpers.StartedProjectCostCreate);
        //        var client = _clientFactory.CreateClient(Constants.client);
        //        var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
        //        var responseHttp = await client.PostAsync(InspectionOfUnitRoute.CreateProjectCostDetails, httpContent);
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
        //        _logger.Error(Error.createProjectCost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        throw;
        //    }

        //}
        //#endregion

        #region Working Capital
        //public async Task<bool> CreateWorkingCapitalDetails(IdmDchgWorkingCapitalDTO workingCapital)
        //{
        //    try
        //    {
        //        _logger.Information(CommonLogHelpers.StratedSaveWorkingCapitalInspectionDetails);
        //        var client = _clientFactory.CreateClient(Constants.client);
        //        var httpContent = new StringContent(JsonConvert.SerializeObject(workingCapital), Encoding.UTF8, Constants.ContentType);
        //        var responseHttp = await client.PostAsync(InspectionOfUnitRoute.CreateWorkingCapitalInspection, httpContent);
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
        //        _logger.Error(Error.createWorkingCapitalInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        throw;
        //    }

        //}

        #endregion

        //#region Means Of Finance
        ////<summary>
        //// Author: Swetha; Module:Import Machinery Inspection; Date:25/08/2022
        ////Modified: Gagana K Added try catch with Loggings Date:19/10/2022
        ////</summary
        //public async Task<IEnumerable<IdmDchgMeansOfFinanceDTO>> GetAllMeansOfFinanceList(long accountNumber)
        //{
        //    try
        //    {
        //        _logger.Information(CommonLogHelpers.GetAllMeansOfFinanceList);
        //        List<IdmDchgMeansOfFinanceDTO> meansOfFinanceDTo = new();
        //        var client = _clientFactory.CreateClient(Constants.client);
        //        var responseHttp = await client.GetAsync(InspectionOfUnitRoute.GetAllMeansOfFinanceList + accountNumber);
        //        if (responseHttp.StatusCode == HttpStatusCode.OK)
        //        {
        //            var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
        //            var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
        //            meansOfFinanceDTo = JsonConvert.DeserializeObject<List<IdmDchgMeansOfFinanceDTO>>(Result.Result.ToString());
        //        }
        //        _logger.Information(CommonLogHelpers.CompletedGetAllMeansOfFinanceList);
        //        return meansOfFinanceDTo;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(Error.getMeansOfFinance + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        throw;
        //    }
        //}

        //public async Task<IEnumerable<SelectListItem>> GetFinanceTypeAsync()
        //{
        //    try
        //    {
        //        _logger.Information(CommonLogHelpers.GetFinanceTypeAsync);
        //        List<SelectListItem> FinanceCatDTo = new();

        //        var client = _clientFactory.CreateClient(Constants.client);
        //        var responseHttp = await client.GetAsync(InspectionOfUnitRoute.GetFinanceTypeAsync);
        //        if (responseHttp.StatusCode == HttpStatusCode.OK)
        //        {
        //            var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
        //            var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
        //            if (Result.Result.ToString() != Constants.brackets)
        //                FinanceCatDTo = JsonConvert.DeserializeObject<List<SelectListItem>>(Result.Result.ToString());
        //            else
        //                FinanceCatDTo = new List<SelectListItem>();
        //        }
        //        _logger.Information(CommonLogHelpers.CompletedGetFinanceTypeAsync);
        //        return FinanceCatDTo;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(Error.getFinanceType + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        throw;
        //    }
        //}


        //public async Task<bool> CreateMeansOfFinanceDetails(IdmDchgMeansOfFinanceDTO meansOfFinance)
        //{
        //    try
        //    {
        //        _logger.Information(CommonLogHelpers.StartedMeansOfFinanceCreate);
        //        var client = _clientFactory.CreateClient(Constants.client);
        //        var httpContent = new StringContent(JsonConvert.SerializeObject(meansOfFinance), Encoding.UTF8, Constants.ContentType);
        //        var responseHttp = await client.PostAsync(InspectionOfUnitRoute.CreateMeansOfFinanceDetails, httpContent);
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
        //        _logger.Error(Error.createMeansOfFinance + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        throw;
        //    }

        //}
        //public async Task<bool> UpdateMeansOfFinanceDetails(IdmDchgMeansOfFinanceDTO meansOfFinance )
        //{
        //    try
        //    {
        //        _logger.Information(CommonLogHelpers.StartedMeansOfFinanceUpdate);
        //        var client = _clientFactory.CreateClient(Constants.client);
        //        var httpContent = new StringContent(JsonConvert.SerializeObject(meansOfFinance), Encoding.UTF8, Constants.ContentType);
        //        var responseHttp = await client.PostAsync(InspectionOfUnitRoute.UpdateMeansOfFinanceDetails, httpContent);
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
        //        _logger.Error(Error.updateMeansOfFinance + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        throw;
        //    }

        //}

        //public async Task<bool> DeleteMeansOfFinanceDetails(IdmDchgMeansOfFinanceDTO meansOfFinance)
        //{
        //    try
        //    {
        //        _logger.Information(CommonLogHelpers.StartedMeansOfFinanceDelete);
        //        var client = _clientFactory.CreateClient(Constants.client);
        //        var httpContent = new StringContent(JsonConvert.SerializeObject(meansOfFinance), Encoding.UTF8, Constants.ContentType);
        //        var responseHttp = await client.PostAsync(InspectionOfUnitRoute.DeleteMeansOfFinanceDetails, httpContent);
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
        //        _logger.Error(Error.deleteMeansOfFinance + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        throw;
        //    }

        //}

        //#endregion

        //#region Common DropDown
        //public async Task<IEnumerable<DropDownDTO>> GetAllProjectCostComponentsDetails()
        //{
        //    List<DropDownDTO> projectCostComponentDetail = new();
        //     var client = _clientFactory.CreateClient(Constants.client);
        //    var responseHttp = await client.GetAsync(RouteName.GetPojectCostComponentAsync);
        //    if (responseHttp.StatusCode == HttpStatusCode.OK)
        //    {
        //        var apiResponse = await responseHttp.Content.ReadAsStringAsync();

        //        var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
        //        projectCostComponentDetail = JsonConvert.DeserializeObject<List<DropDownDTO>>(result.Result.ToString());
        //    }
        //    return projectCostComponentDetail;
        //}
        //#endregion

        #region Recommended Disbursement Details
        public async Task<IEnumerable<IdmDsbdetsDTO>> GetAllRecomDisbursementDetails(long accountNumber)
        {
            try
            {
                _logger.Information(Constants.GetAllRecomDisbursementDetails);
                List<IdmDsbdetsDTO> listRecomDisbursementDetails = new();
                var client = _clientFactory.CreateClient("ksfcApi");
                var responseHttp = await client.GetAsync(CreationOfDisbursmentProposalAssetRoute.GetAllRecomDisburseDetails + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    listRecomDisbursementDetails = JsonConvert.DeserializeObject<List<IdmDsbdetsDTO>>(result.Result.ToString());
                }
                return listRecomDisbursementDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllRecomDisbursementDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }
        public async Task<IEnumerable<TblAllcCdTabDTO>> GetAllocationCodeDetails()
        {
            try
            {
                _logger.Information(Constants.GetAllocationCodeDetails);
                List<TblAllcCdTabDTO> listAllocationDetails = new();
                var client = _clientFactory.CreateClient("ksfcApi");
                var responseHttp = await client.GetAsync(CreationOfDisbursmentProposalAssetRoute.GetAllocationDetails);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    listAllocationDetails = JsonConvert.DeserializeObject<List<TblAllcCdTabDTO>>(result.Result.ToString());
                }
                return listAllocationDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllocationCodeDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        public async Task<IEnumerable<TblIdmReleDetlsDTO>> GetRecomDisbursementReleaseDetails()
        {
            try
            {
                _logger.Information(Constants.GetRecomDisbursementReleaseDetails);
                List<TblIdmReleDetlsDTO> listRecomDisbursementReleaseDetails = new();
                var client = _clientFactory.CreateClient("ksfcApi");
                var responseHttp = await client.GetAsync(CreationOfDisbursmentProposalAssetRoute.GetRecomDisbursementReleaseDetails);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    listRecomDisbursementReleaseDetails = JsonConvert.DeserializeObject<List<TblIdmReleDetlsDTO>>(result.Result.ToString());
                }
                return listRecomDisbursementReleaseDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetRecomDisbursementReleaseDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }

        public async Task<bool> UpdateRecomDisbursementDetail(IdmDsbdetsDTO idmDsbdetsDTO)
        {
            try
            {
                _logger.Information(Constants.UpdateRecomDisbursementDetail);
                var client = _clientFactory.CreateClient("ksfcApi");
                var httpContent = new StringContent(JsonConvert.SerializeObject(idmDsbdetsDTO), Encoding.UTF8, "application/json");
                var responseHttp = await client.PostAsync(CreationOfDisbursmentProposalAssetRoute.UpdateRecomDisburseDetail, httpContent);
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
                _logger.Error(Error.UpdateRecomDisbursementDetail + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }

       


        #endregion
    }
}
