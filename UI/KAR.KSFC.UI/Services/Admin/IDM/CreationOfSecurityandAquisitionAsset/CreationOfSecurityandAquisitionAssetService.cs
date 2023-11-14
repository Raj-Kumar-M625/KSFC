using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfSecurityandAquisitionAsset;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.ICreationOfSecurityandAquisitionAssetService;

using KAR.KSFC.UI.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using KAR.KSFC.Components.Common.Logging.Client;
using System;
using System.ComponentModel;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;

namespace KAR.KSFC.UI.Services.Admin.IDM.CreationOfSecurityandAquisitionAssetService
{
    public class CreationOfSecurityandAquisitionAssetService : ICreationOfSecurityandAquisitionAssetService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _logger;
        public CreationOfSecurityandAquisitionAssetService(IHttpClientFactory clientFactory,ILogger logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        #region Land Acquisition
        //<summary>
        // Author: Dev; Module: Land Acquisition; Date:29/09/2022
        // </summary>
        public async Task<IEnumerable<TblIdmIrLandDTO>> GetAllCreationOfSecurityandAquisitionAssetList(long? accountNumber , long InspectionId)
        {
            try
            {
                _logger.Information(Constants.GetAllCreationOfSecurityandAquisitionAssetList);
                List<TblIdmIrLandDTO> landAcquisition = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(CreationofSecurityandAcquisitionAssetRoute.GetAllCreationOfSecurityandAquisitionAssetList + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    landAcquisition = JsonConvert.DeserializeObject<List<TblIdmIrLandDTO>>(result.Result.ToString());
                }
                return landAcquisition;
            }
           catch(Exception ex)
            {
                _logger.Error(Error.GetAllCreationOfSecurityandAquisitionAssetList + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
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
        #endregion
        public async Task<bool> UpdateLandAcquisitionDetails(TblIdmIrLandDTO landacq)
        {
            try
            {
                _logger.Information(Constants.UpdateLandAcquisitionDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(landacq), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(CreationofSecurityandAcquisitionAssetRoute.UpdateLandAcquisitionDetails, httpContent);
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
                _logger.Error(Error.UpdateLandAcquisitionDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        
        #endregion

        #region Machinery Acquisition 
        // <summary>
        //  Author: Dev Patel; Module: Machinery Acquisition; Date:28/09/2022
        // </summary>
        public async Task<IEnumerable<IdmIrPlmcDTO>> GetAllMachineryAcquisitionDetails(long? accountNumber,long InspectionId)
        {
            try
            {
                _logger.Information(Constants.GetAllMachineryAcquisitionDetails);
                List<IdmIrPlmcDTO> machineryAcquisition = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(CreationofSecurityandAcquisitionAssetRoute.GetAllMachineryAcquisitionDetails + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    machineryAcquisition = JsonConvert.DeserializeObject<List<IdmIrPlmcDTO>>(result.Result.ToString());
                }
                return machineryAcquisition;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllMachineryAcquisitionDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
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

        #endregion

        public async Task<bool> UpdateMachineryAcquisitionDetails(IdmIrPlmcDTO machineacq)
        {
            try
            {
                _logger.Information(Constants.UpdateMachineryAcquisitionDetails);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(machineacq), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(CreationofSecurityandAcquisitionAssetRoute.UpdateMachineryAcquisitionDetails, httpContent);
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
                _logger.Error(Error.UpdateMachineryAcquisitionDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        
        #endregion

        #region Furniture Acquisition
        // <summary>
        //  Author: Kiran V; Module: Furniture Acquisition; Date:30/09/2022
        // </summary>
        public async Task<IEnumerable<TblIdmIrFurnDTO>> GetFurnitureAcquisitionList(long? accountNumber ,long InspectionId)
        {
            try
            {
                _logger.Information(Constants.GetFurnitureAcquisitionList);
                List<TblIdmIrFurnDTO> FurnitureAcquisitionDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(CreationofSecurityandAcquisitionAssetRoute.GetAllFurnitureAcquisitionList + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var responseStrPG = await responseHttp.Content.ReadAsStringAsync();
                    var Result = JsonConvert.DeserializeObject<ApiResultResponse>(responseStrPG);
                    FurnitureAcquisitionDetails = JsonConvert.DeserializeObject<List<TblIdmIrFurnDTO>>(Result.Result.ToString());
                }
                return FurnitureAcquisitionDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetFurnitureAcquisitionList + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
           
        }

        #region FurnitureInspection

        //<summary>
        // Author: Sandeep M; Module:Inspection Detail; Date:25/08/2022
        //Modified: Gagana K Added try catch with Loggings Date:19/10/2022
        //</summary
        public async Task<IEnumerable<IdmDChgFurnDTO>> GetAllFurnitureInspectionList(long accountNumber, long InspectionId)
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
        #endregion
        public async Task<bool> UpdateFurnitureAcquisition(TblIdmIrFurnDTO addr)
        {
            try
            {
                _logger.Information(Constants.UpdateFurnitureAcquisition);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(addr), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(CreationofSecurityandAcquisitionAssetRoute.UpdateFurnitureAcquisitionList, httpContent);
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
                _logger.Error(Error.UpdateFurnitureAcquisition + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region Building Acquisition Details
        // <summary>
        //  Author: Raman A; Module: building Acquisition; Date:30/09/2022
        // </summary>
        public async Task<IEnumerable<TblIdmBuildingAcquisitionDetailsDTO>> GetAllBuildingAcquisitionDetails(long accountNumber,long InspectionId)
        {
            try
            {
                _logger.Information(Constants.GetAllBuildingAcquisitionDetails);
                List<TblIdmBuildingAcquisitionDetailsDTO> listBuildingAcquisitionDetails = new();
                var client = _clientFactory.CreateClient(Constants.client);
                var responseHttp = await client.GetAsync(CreationofSecurityandAcquisitionAssetRoute.GetAllBuildingAquisitionDetails + accountNumber);
                if (responseHttp.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await responseHttp.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResultResponse>(apiResponse);
                    listBuildingAcquisitionDetails = JsonConvert.DeserializeObject<List<TblIdmBuildingAcquisitionDetailsDTO>>(result.Result.ToString());
                }
                return listBuildingAcquisitionDetails;
            }
            catch (Exception ex)
            {
                _logger.Error(Error.GetAllBuildingAcquisitionDetails + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }
        public async Task<bool> UpdateBuildingAcquisitionDetail(TblIdmBuildingAcquisitionDetailsDTO tblidmBuildingAcquisitionDetailsDTO)
        {
            try
            {
                _logger.Information(Constants.UpdateBuildingAcquisitionDetail);
                var client = _clientFactory.CreateClient(Constants.client);
                var httpContent = new StringContent(JsonConvert.SerializeObject(tblidmBuildingAcquisitionDetailsDTO), Encoding.UTF8, Constants.ContentType);
                var responseHttp = await client.PostAsync(CreationofSecurityandAcquisitionAssetRoute.UpdateBuildingAcquisitionDetail, httpContent);
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
                _logger.Error(Error.UpdateBuildingAcquisitionDetail + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }
        }

        #endregion
    }
}