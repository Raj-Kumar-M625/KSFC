using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfSecurityandAquisitionAsset;
using KAR.KSFC.Components.Common.Dto.IDM.Disbursement;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Data.Models.DbModels;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices.Admin.IDM.ICreationOfSecurityandAquisitionAssetService
{
    public interface ICreationOfSecurityandAquisitionAssetService
    {
        #region Land Acquisition
        Task<IEnumerable<TblIdmIrLandDTO>> GetAllCreationOfSecurityandAquisitionAssetList(long? accountNumber, long InspectionId);
        Task<IEnumerable<IdmDchgLandDetDTO>> GetAllLandInspectionList(long accountNumber, long InspectionId);
        Task<bool> UpdateLandAcquisitionDetails(TblIdmIrLandDTO landacq);
        #endregion

        #region Machinery Acquisition
        Task<IEnumerable<IdmIrPlmcDTO>> GetAllMachineryAcquisitionDetails(long? accountNumber, long InspectionId);
        Task<IEnumerable<IdmDchgIndigenousInspectionDTO>> GetAllIndigenousMachineryInspectionList(long accountNumber, long InspectionId);
        Task<bool> UpdateMachineryAcquisitionDetails(IdmIrPlmcDTO machineacq);
        #endregion
        //Author Kiran: 29-Sep-2022
        #region FurnitureAcquisition
        Task<IEnumerable<IdmDChgFurnDTO>> GetAllFurnitureInspectionList(long accountNumber, long InspectionId);
        Task<IEnumerable<TblIdmIrFurnDTO>> GetFurnitureAcquisitionList(long? accountNumber, long InspectionId);
            Task<bool> UpdateFurnitureAcquisition(TblIdmIrFurnDTO addr);
           
        #endregion

        #region Building Acquisition Details

        Task<IEnumerable<TblIdmBuildingAcquisitionDetailsDTO>> GetAllBuildingAcquisitionDetails(long accountNumber , long InspectionId);
        Task<bool> UpdateBuildingAcquisitionDetail(TblIdmBuildingAcquisitionDetailsDTO tblidmBuildingAcquisitionDetailsDTO);
       #endregion
    }
}
