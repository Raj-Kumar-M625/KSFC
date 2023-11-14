using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfSecurityandAquisitionAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.CreationOfSecurityandAquisitionAsset
{
    public interface ICreationOfSecurityandAquisitionAsset
    {
        #region Land Acquition
        Task<IEnumerable<TblIdmIrLandDTO>> GetAllCreationOfSecurityandAquisitionAssetList(long accountNumber, CancellationToken token);
        Task<bool> UpadteLandAcquisitionDetails(TblIdmIrLandDTO LandDTO, CancellationToken token);
        #endregion

        #region Machinery Acquisition
        Task<IEnumerable<IdmIrPlmcDTO>> GetAllMachineryAcquisitionDetails(long accountNumber, CancellationToken token);
        Task<bool> UpadteMachineryAcquisitionDetails(IdmIrPlmcDTO MachineDTO, CancellationToken token);
        #endregion

        #region Building Acquisition Details
        Task<IEnumerable<TblIdmBuildingAcquisitionDetailsDTO>> GetAllBuildingAcquisitionDetails(long accountNumber, CancellationToken token);
        Task<bool> UpdateBuildingAcquisitionDetail(TblIdmBuildingAcquisitionDetailsDTO tblidmBuildingAcquisitionDetailsDTO, CancellationToken token);
        #endregion

        #region Furniture Acquisition Details        
        Task<IEnumerable<TblIdmIrFurnDTO>> GetFurnitureAcquisitionDetailsList(long accountNumber, CancellationToken token);        
        Task<bool> UpdateFurnitureAcquisition(TblIdmIrFurnDTO FurnitureAcqDTO, CancellationToken token);      
        #endregion
    }
}