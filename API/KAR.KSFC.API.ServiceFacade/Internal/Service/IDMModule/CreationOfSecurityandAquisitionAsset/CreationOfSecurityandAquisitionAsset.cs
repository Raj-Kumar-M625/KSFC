using AutoMapper;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.CreationOfSecurityandAquisitionAsset;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.UnitDetails.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfSecurityandAquisitionAsset;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.IDMModule.CreationOfSecurityandAquisitionAsset
{
    public class CreationOfSecurityandAquisitionAsset : ICreationOfSecurityandAquisitionAsset
    {
       
        private readonly IEntityRepository<TblIdmIrLand> _landacquitionRepository;
        private readonly IEntityRepository<TblIdmIrPlmc> _machineryRepository;
        private readonly IEntityRepository<TblIdmBuildingAcquisitionDetails> _buildingDetailsRepository;
        private readonly IEntityRepository<TblIdmIrFurn> _furnitureAcuisitionRepository;
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;
        private readonly UserInfo _userInfo;

        public CreationOfSecurityandAquisitionAsset(IEntityRepository<TblIdmIrLand> landacquitionRepository, IUnitOfWork work,
         IMapper mapper,  UserInfo userInfo, IEntityRepository<TblIdmIrPlmc> machineryRepository, IEntityRepository<TblIdmBuildingAcquisitionDetails> buildingDetailsRepository, IEntityRepository<TblIdmIrFurn> furnitureAcuisitionRepository)        
        {
            
            _landacquitionRepository = landacquitionRepository;
            _furnitureAcuisitionRepository = furnitureAcuisitionRepository;
            _work = work;
            _mapper = mapper;
            _userInfo = userInfo;
            _machineryRepository = machineryRepository;
            _buildingDetailsRepository = buildingDetailsRepository;
        }

        #region Land Acquisition
        /// <summary>
        ///  Author: Gowtham; Module: Land Acquisition; Date:28/09/2022
        /// </summary>
        public async Task<IEnumerable<TblIdmIrLandDTO>> GetAllCreationOfSecurityandAquisitionAssetList(long accountNumber, CancellationToken token)
        {
            var data = await _landacquitionRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber /*&& x.IsActive == true && x.IsDeleted == false*/).ConfigureAwait(false);
            return _mapper.Map<List<TblIdmIrLandDTO>>(data);
        }
        public async Task<bool> UpadteLandAcquisitionDetails(TblIdmIrLandDTO LandDTO, CancellationToken token)
        {
            var currentDetails = await _landacquitionRepository.FirstOrDefaultByExpressionAsync(x => x.IrlId == LandDTO.IrlId, token);
            if(currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _landacquitionRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);
            }
            

            var basicDetails = _mapper.Map<TblIdmIrLand>(LandDTO);
            basicDetails.IrlId = 0;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false; 
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            var response = await _landacquitionRepository.AddAsync(basicDetails, token).ConfigureAwait(false);

            await _work.CommitAsync(token).ConfigureAwait(false);
            if(response != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

       
        #endregion

        #region Furniture Acquisition
        /// <summary>
        ///  Author: Kiran; Module: Furniture Acquisition; Date:30/09/2022
        /// </summary>
        public async Task<IEnumerable<TblIdmIrFurnDTO>> GetFurnitureAcquisitionDetailsList(long accountNumber, CancellationToken token)
        {
            var data = await _furnitureAcuisitionRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber /*&& x.IsActive == true && x.IsDeleted == false*/).ConfigureAwait(false);
            return _mapper.Map<List<TblIdmIrFurnDTO>>(data);
        }

        public async Task<bool> UpdateFurnitureAcquisition(TblIdmIrFurnDTO FurnitureAcqDTO, CancellationToken token)
        {
            var currentDetails = await _furnitureAcuisitionRepository.FirstOrDefaultByExpressionAsync(x => x.IrfId == FurnitureAcqDTO.IrfId, token);
            if(currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _furnitureAcuisitionRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);
            }
            

            var FurnitureAcqDetails = _mapper.Map<TblIdmIrFurn>(FurnitureAcqDTO);
            FurnitureAcqDetails.IrfId = 0;
            FurnitureAcqDetails.IsActive = true;
            FurnitureAcqDetails.IsDeleted = false;
            FurnitureAcqDetails.ModifiedBy = _userInfo.Name;
            FurnitureAcqDetails.ModifiedDate= DateTime.UtcNow;

            var response = await _furnitureAcuisitionRepository.AddAsync(FurnitureAcqDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if (response != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Machinery Acquisition
        /// <summary>
        ///  Author: Dev Patel; Module: Machinery Acquisition; Date:28/09/2022
        /// </summary>
        public async Task<IEnumerable<IdmIrPlmcDTO>> GetAllMachineryAcquisitionDetails(long accountNumber, CancellationToken token)
        {
            var data = await _machineryRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber /*&& x.IsActive == true && x.IsDeleted == false*/).ConfigureAwait(false);
            return _mapper.Map<List<IdmIrPlmcDTO>>(data);
        }

        public async Task<bool> UpadteMachineryAcquisitionDetails(IdmIrPlmcDTO MachineDTO, CancellationToken token)
        {
            var currentDetails = await _machineryRepository.FirstOrDefaultByExpressionAsync(x => x.IrPlmcId == MachineDTO.IrPlmcId, token);
            if(currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _machineryRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);
            }
            

            var basicDetails = _mapper.Map<TblIdmIrPlmc>(MachineDTO);
            basicDetails.IrPlmcId = 0;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.ModifiedDate = DateTime.UtcNow;

            var response = await _machineryRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if (response != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

       
        #endregion

        #region BuildingAquisitionDetails

        public async Task<IEnumerable<TblIdmBuildingAcquisitionDetailsDTO>> GetAllBuildingAcquisitionDetails(long accountNumber, CancellationToken token)
        {
            var data = await _buildingDetailsRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber/* && x.IsActive == true && x.IsDeleted == false*/).ConfigureAwait(false);
            return _mapper.Map<List<TblIdmBuildingAcquisitionDetailsDTO>>(data);
        }
        public async Task<bool> UpdateBuildingAcquisitionDetail(TblIdmBuildingAcquisitionDetailsDTO tblidmBuildingAcquisitionDetailsDTO, CancellationToken token)
        {
            var currentDetails = await _buildingDetailsRepository.FirstOrDefaultByExpressionAsync(x => x.Irbid == tblidmBuildingAcquisitionDetailsDTO.Irbid, token);
            if(currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                await _buildingDetailsRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);
            }
            

            var basicDetails = _mapper.Map<TblIdmBuildingAcquisitionDetails>(tblidmBuildingAcquisitionDetailsDTO);
            basicDetails.Irbid = 0;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;

            var response = await _buildingDetailsRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            if (response != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        #endregion

    }
}