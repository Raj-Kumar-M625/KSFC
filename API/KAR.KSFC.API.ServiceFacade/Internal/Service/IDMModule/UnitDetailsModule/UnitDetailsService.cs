using AutoMapper;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.UnitDetailsModule;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.Service;
using KAR.KSFC.Components.Data.Repository.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.IDMModule.UnitDetailsModule
{
    public class UnitDetailsService: IUnitDetailsService
    {
        private readonly IEntityRepository<TblIdmUnitDetails> _idmUnitDetailsRepository;
        private readonly IEntityRepository<TblUnitMast> _UnitMasterRepository;
        private readonly IEntityRepository<TblIdmUnitAddress> _idmUnitAddressRepository;
        private readonly IEntityRepository<IdmPromoter> _promoterRepository;
        private readonly IEntityRepository<TblIdmPromAddress> _promoterAddressRepository;
        private readonly IEntityRepository<IdmPromoterBankDetails> _promoterBankRepository;
        private readonly IEntityRepository<IdmUnitProducts> _appUnitDetailsRepository;
        private readonly IEntityRepository<TblIdmUnitBank> _changeBankDetailsRepository;
        private readonly IEntityRepository<TbIIfscMaster> _ifscBankDetailsRepository;
        private readonly IEntityRepository<TblPromterLiabDet> _promoterLiabilityInfoRepository;
        private readonly IEntityRepository<TblIdmPromoterNetWork> _promoterNetWorthRepository;
        private readonly IEntityRepository<TblPromCdtab> _masterPromoterDetails;
        private readonly IEntityRepository<TblPincodeMaster> _masterPincodeDetails;
        private readonly IEntityRepository<TlqCdtab> _masterTalukDetails;
        private readonly IEntityRepository<HobCdtab> _masterHobliDetails;
        private readonly IEntityRepository<VilCdtab> _masterVillageDetails;
        private readonly IEntityRepository<TblPincodeDistrictCdtab> _pincodeDistictDetails;
        private readonly IEntityRepository<TblAssettypeCdtab> _assetTypeDetails;
        private readonly IEntityRepository<TblAssetcatCdtab> _assetCategaryDetails;
        private readonly IEntityRepository<IdmPromAssetDet> _promoterAssetDetailsRepository;
        private readonly IEntityRepository<TblProdCdtab> _productList;
        private readonly IEntityRepository<TblIdmProjland> _projlandRepository;
        private readonly IEntityRepository<TblIdmProjPlmc> _projplmcRepository;
        private readonly IEntityRepository<TblIdmProjBldg> _projBldgRepository;
        private readonly IEntityRepository<TblIdmProjImpMc> _projImpMcRepository;
        private readonly IEntityRepository<TblIdmFurn> _idmFurnRepository;
        private readonly UserInfo _userInfo;
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;   


        public UnitDetailsService(IEntityRepository<TblAppLoanMast> loanRepository, IEntityRepository<TblIdmUnitDetails> idmUnitDetails,
                                   IUnitOfWork work, IMapper mapper, IEntityRepository<TblUnitMast> unitMasterRepository, IEntityRepository<TblIdmUnitAddress> idmUnitAddress,
                                   IEntityRepository<IdmPromoter> promoterRepository, IEntityRepository<TblIdmPromAddress> promoterAddressRepository,
                                   IEntityRepository<IdmPromoterBankDetails> promoterBankRepository, IEntityRepository<TblPclasCdtab> promoterClassRepository,
                                   IEntityRepository<TblPsubclasCdtab> promoterSubClassRepository, IEntityRepository<TblPqualCdtab> promoterQualificationRepository,
                                   IEntityRepository<TblDomiCdtab> domicileRepository, UserInfo userInfo, IEntityRepository<IdmUnitProducts> appUnitDetails,
                                   IEntityRepository<TblIdmUnitBank> changeBankDetailsRepository, IEntityRepository<TbIIfscMaster> ifscBankDetailsRepository, IEntityRepository<IdmPromAssetDet> promoterAssetDetailsRepository,
                                   IEntityRepository<TblPromterLiabDet> promoterLiabilityInfoRepository, IEntityRepository<TblIdmPromoterNetWork> promoterNetWorthRepository,
                                   IEntityRepository<TblPromCdtab> masterPromoterDetails, IEntityRepository<TblPincodeMaster> masterPincodeDetails,
                                   IEntityRepository<TblProdCdtab> productList, IEntityRepository<TblPincodeDistrictCdtab> pincodeDistictDetails, IEntityRepository<TlqCdtab> masterTalukDetails, IEntityRepository<HobCdtab> masterHobliDetails, IEntityRepository<VilCdtab> masterVillageDetails,
                                   IEntityRepository<TblAssettypeCdtab> assetTypeDetails,IEntityRepository<TblAssetcatCdtab> assetCategaryDetails,IEntityRepository<TblIdmFurn> furnDetails ,IEntityRepository<TblIdmProjland> landDetails ,IEntityRepository<TblIdmProjPlmc> machineDetails,
                                   IEntityRepository<TblIdmProjImpMc> imporytDetails ,IEntityRepository<TblIdmProjBldg> BuildingDetails)
                           

        {
         
            _work = work;
            _mapper = mapper;
            _idmUnitDetailsRepository = idmUnitDetails;
            _UnitMasterRepository = unitMasterRepository;
            _idmUnitAddressRepository = idmUnitAddress;
            _promoterRepository = promoterRepository;
            _promoterAddressRepository = promoterAddressRepository;
            _promoterBankRepository = promoterBankRepository;
            _appUnitDetailsRepository = appUnitDetails;
            _promoterAssetDetailsRepository = promoterAssetDetailsRepository;
            _userInfo = userInfo;
            _changeBankDetailsRepository = changeBankDetailsRepository;
            _ifscBankDetailsRepository = ifscBankDetailsRepository;
            _promoterLiabilityInfoRepository = promoterLiabilityInfoRepository;
            _promoterNetWorthRepository = promoterNetWorthRepository;
            _masterPromoterDetails = masterPromoterDetails;
            _masterPincodeDetails = masterPincodeDetails;
            _productList = productList;
            _pincodeDistictDetails = pincodeDistictDetails;
            _masterTalukDetails = masterTalukDetails;
            _masterHobliDetails = masterHobliDetails;
            _masterVillageDetails = masterVillageDetails;
            _assetTypeDetails = assetTypeDetails;
            _assetCategaryDetails = assetCategaryDetails;
            _idmFurnRepository = furnDetails;
            _projBldgRepository = BuildingDetails;
            _projImpMcRepository = imporytDetails;
            _projlandRepository = landDetails;
            _projplmcRepository = machineDetails;
        }

        #region Name Of Unit
        /// <summary>
        ///  Author: Sandeep; Module: Name Of Unit; Date:24/08/2022
        /// </summary>
        public async Task<IdmUnitDetailDTO> GetNameOfUnit(long accountNumber, CancellationToken token)
        {
            var data = await _idmUnitDetailsRepository.FindByMatchingPropertiesAsync(token,
                x => x.LoanAcc == accountNumber , unitmaster=> unitmaster.TblUnitMast).ConfigureAwait(false);
            var res = data.FirstOrDefault();
            if(res!=null)
            {
                return _mapper.Map<IdmUnitDetailDTO>(res);
            }
            else
            {
                res = new TblIdmUnitDetails();
                return _mapper.Map<IdmUnitDetailDTO>(res);
            }         
        }
        public async Task<IdmUnitDetailDTO> UpdateNameOfUnit(IdmUnitDetailDTO idmUnitDetail, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblUnitMast>(idmUnitDetail.TblUnitMast);
            await _UnitMasterRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<IdmUnitDetailDTO>(idmUnitDetail);
        }
        #endregion

        #region Change Location Address 
        /// <summary>
        ///  Author:Gagana; Module:Change Location Address ; Date:09/09/2022
        /// </summary>  
        public async Task<IEnumerable<IdmUnitAddressDTO>> GetAllAddressDetails(long accountNumber, CancellationToken token)
        {
            var data = await _idmUnitAddressRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false, address => address.TblAddressCdtab).ConfigureAwait(false);
            return _mapper.Map<List<IdmUnitAddressDTO>>(data);
        }
        public async Task<IEnumerable<TblPincodeMasterDTO>> GetAllMasterPinCodeDetails( CancellationToken token)
        {
            var data = await _masterPincodeDetails.FindByMatchingPropertiesAsync(token, x =>  x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<TblPincodeMasterDTO>>(data);
        }

        public async Task<IEnumerable<PincodeDistrictCdtabDTO>> GetAllPinCodeDistrictDetails(CancellationToken token)
        {
            var data = await _pincodeDistictDetails.FindByMatchingPropertiesAsync(token, x => x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<PincodeDistrictCdtabDTO>>(data);
        }

        public async Task<IEnumerable<TlqCdTabDTO>> GetAllTalukDetails(CancellationToken token)
        {
            var data = await _masterTalukDetails.FindByMatchingPropertiesAsync(token, x => x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<TlqCdTabDTO>>(data);
        }
        public async Task<IEnumerable<HobCdtabDTO>> GetAllHobliDetails(CancellationToken token)
        {
            var data = await _masterHobliDetails.FindByMatchingPropertiesAsync(token, x => x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<HobCdtabDTO>>(data);
        }
        public async Task<IEnumerable<VilCdTabDTO>> GetAllVillageDetails(CancellationToken token)
        {
            var data = await _masterVillageDetails.FindByMatchingPropertiesAsync(token, x => x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<VilCdTabDTO>>(data);
        }
        public async Task<bool> UpdateAddressDetails(IdmUnitAddressDTO PromAddressDto, CancellationToken token)
        {
            var currentDetails = await _idmUnitAddressRepository.FirstOrDefaultByExpressionAsync(x => x.IdmUtAddressRowid == PromAddressDto.IdmUtAddressRowid, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _idmUnitAddressRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<TblIdmUnitAddress>(PromAddressDto);
                basicDetails.IdmUtAddressRowid = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.CreatedBy = _userInfo.Name;
                basicDetails.CreatedDate = DateTime.UtcNow;

                var response = await _idmUnitAddressRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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
            return false;
        }
        #endregion

        #region Promoter Profile
        /// <summary>
        ///  Author: Dev Patel; Module: Promoter Profile; Date:26/08/2022
        ///  Author: Gagana; Module: Get Promoter Detailsfrom Master Table; Date:14/11/2022
        /// </summary>         
        /// 
        public async Task<IEnumerable<TblPromcdtabDTO>> GetAllMasterPromoterProfileDetails(CancellationToken token)
        {
            var data = await _masterPromoterDetails.FindByMatchingPropertiesAsync(token, x => x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<TblPromcdtabDTO>>(data);
        }
        public async Task<IEnumerable<IdmPromoterDTO>> GetAllPromoterProfileDetails(long? accountNumber, CancellationToken token)
        {
            var data = await _promoterRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<IdmPromoterDTO>>(data);
        }
   
        public async Task<bool> CreatePromoterProfileDetails(IdmPromoterDTO PromprofileDto, CancellationToken token)
        {
            var basicDetails = _mapper.Map<IdmPromoter>(PromprofileDto);
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreatedBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;


            var promotorAddress = new TblIdmPromAddress
            {
                LoanAcc = basicDetails.LoanAcc,
                LoanSub = basicDetails.LoanSub,
                PromoterCode = basicDetails.PromoterCode,

                CreatedDate = DateTime.UtcNow,
                CreatedBy = _userInfo.Name,
                IsActive = true,
                IsDeleted = false
            };

            var response = await _promoterRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _promoterAddressRepository.AddAsync(promotorAddress, token).ConfigureAwait(false);
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
        public async Task<bool> UpdatePromoterProfileDetails(IdmPromoterDTO PromprofileDto, CancellationToken token)
        {
            var currentDetails = await _promoterRepository.FirstOrDefaultByExpressionAsync(x => x.IdmPromId == PromprofileDto.IdmPromId, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _promoterRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<IdmPromoter>(PromprofileDto);
                basicDetails.IdmPromId = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.CreatedBy = _userInfo.Name;
                basicDetails.CreatedDate = DateTime.UtcNow;

                var response = await _promoterRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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
            return false;
        }
        public async Task<bool> DeletePromoterProfileDetails(IdmPromoterDTO PromprofileDto, CancellationToken token)
        {
            var basicDetails = _mapper.Map<IdmPromoter>(PromprofileDto);
            basicDetails.IsActive = false;
            basicDetails.IsDeleted = true;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;           
            var response = await _promoterRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
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

        #region Promoter Bank
        public async Task<IEnumerable<IdmPromoterBankDetailsDTO>> GetAllPromoterBankInfo(long? accountNumber, CancellationToken token)
        {
            var data = await _promoterBankRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false, tblpri => tblpri.TbIIfscMaster).ConfigureAwait(false);
             return _mapper.Map<List<IdmPromoterBankDetailsDTO>>(data);
        }

        public async Task<bool> CreatePromoterBankInfo(IdmPromoterBankDetailsDTO changePromoterBankDetail, CancellationToken token)
        {
            if (changePromoterBankDetail.PrmPrimaryBank)
            {
                IEnumerable<IdmPromoterBankDetails> AllPromoterBankList = await _promoterBankRepository.FindByMatchingPropertiesAsync(token, x => x.PromoterCode == changePromoterBankDetail.PromoterCode && x.IsActive == true && x.IsDeleted == false ).ConfigureAwait(false);
                if (AllPromoterBankList != null)
                {
                    foreach (IdmPromoterBankDetails promoterBankDetail in AllPromoterBankList)
                    {
                        promoterBankDetail.PrmPrimaryBank = false;
                        await _promoterBankRepository.UpdateAsync(promoterBankDetail, token).ConfigureAwait(false);
                    }
                }
            }

            var basicDetails = _mapper.Map<IdmPromoterBankDetails>(changePromoterBankDetail);
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreatedBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var response = await _promoterBankRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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

        public async Task<bool> UpdatePromoterBankInfo(IdmPromoterBankDetailsDTO changePromoterBankDetail, CancellationToken token)
        {

            if(changePromoterBankDetail.PrmPrimaryBank)
            {
                IEnumerable<IdmPromoterBankDetails> AllPromoterBankList = await _promoterBankRepository.FindByMatchingPropertiesAsync(token, x => x.PromoterCode == changePromoterBankDetail.PromoterCode && x.IsActive == true && x.IsDeleted == false && x.PrmIfscId != changePromoterBankDetail.PrmIfscId).ConfigureAwait(false);
                if (AllPromoterBankList != null)
                {
                    foreach (IdmPromoterBankDetails promoterBankDetail in AllPromoterBankList)
                    {
                        promoterBankDetail.PrmPrimaryBank = false;
                        await _promoterBankRepository.UpdateAsync(promoterBankDetail, token).ConfigureAwait(false);
                    }
                }
            }
            
               
            var currentDetails = await _promoterBankRepository.FirstOrDefaultByExpressionAsync(x => x.IdmPromBankId == changePromoterBankDetail.IdmPromBankId, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _promoterBankRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<IdmPromoterBankDetails>(changePromoterBankDetail);
                basicDetails.IdmPromBankId = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.CreatedBy = _userInfo.Name;
                basicDetails.CreatedDate = DateTime.UtcNow;

                var response = await _promoterBankRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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
            return false;
        }

        public async Task<bool> DeletePromoterBankInfo(IdmPromoterBankDetailsDTO changePromoterBankDetail, CancellationToken token)
        {
            var basicDetails = _mapper.Map<IdmPromoterBankDetails>(changePromoterBankDetail);
            basicDetails.IsActive = false;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.IsDeleted = true;
            var response = await _promoterBankRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
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

        #region Promoter Address
        /// <summary>
        ///  Author:Gagana; Module: Promoter; Date:01/09/2022
        /// </summary>   
        public async Task<IEnumerable<IdmPromAddressDTO>> GetAllPromoterAddressDetails(long? accountNumber, CancellationToken token)
        {
            var data = await _promoterAddressRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false, dis => dis.TblPincodeDistrictCdtab, stateD => stateD.TblPincodeStateCdtab).ConfigureAwait(false);
            return _mapper.Map<List<IdmPromAddressDTO>>(data);
        }
        public async Task<bool> UpdatePromAddressDetails(IdmPromAddressDTO PromAddressDto, CancellationToken token)
        {
            var currentDetails = await _promoterAddressRepository.FirstOrDefaultByExpressionAsync(x => x.IdmPromadrId == PromAddressDto.IdmPromadrId, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _promoterAddressRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<TblIdmPromAddress>(PromAddressDto);
                basicDetails.IdmPromadrId = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.CreatedBy = _userInfo.Name;
                basicDetails.CreatedDate = DateTime.UtcNow;

                var response = await _promoterAddressRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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
            return false;
        }

        public async Task<bool> CreatePromAddressDetails(IdmPromAddressDTO PromAddressDto, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmPromAddress>(PromAddressDto);
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreatedBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var response = await _promoterAddressRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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

        public async Task<bool> DeletePromAddressDetails(IdmPromAddressDTO PromAddressDto, CancellationToken token)
        {            
            var basicDetails = _mapper.Map<TblIdmPromAddress>(PromAddressDto);
            basicDetails.IsActive = false;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.IsDeleted = true;
            var response = await _promoterAddressRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
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

        #region ProductDetails
        public async Task<IEnumerable<IdmUnitProductsDTO>> GetAllProductDetails(long accountNumber, CancellationToken token)
        {
            var data = await _appUnitDetailsRepository.FindByMatchingPropertiesAsync(token,x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<IdmUnitProductsDTO>>(data);
        }

        public async Task<IEnumerable<TblProdCdtabDTO>> GetAllProductList(CancellationToken token)
        {
            var data = await _productList.FindByExpressionAsync(x=> x.IsActive == true && x.IsDeleted == false ,token).ConfigureAwait(false);
            return _mapper.Map<List<TblProdCdtabDTO>>(data);
        }
        public async Task<bool> CreateProductDetails(IdmUnitProductsDTO ProductDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<IdmUnitProducts>(ProductDTO);
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreatedBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;
            basicDetails.CreatedBy = _userInfo.Name;

            var response = await _appUnitDetailsRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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

        public async Task<bool> UpdateProductDetails(IdmUnitProductsDTO ProductDTO, CancellationToken token)
        {
            var currentDetails = await _appUnitDetailsRepository.FirstOrDefaultByExpressionAsync(x => x.IdmUtproductRowid == ProductDTO.IdmUtproductRowid, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _appUnitDetailsRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<IdmUnitProducts>(ProductDTO);
                basicDetails.IdmUtproductRowid = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.CreatedBy = _userInfo.Name;
                basicDetails.CreatedDate = DateTime.UtcNow;

                var response = await _appUnitDetailsRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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
            return false;
        }

        public async Task<bool> DeleteProductDetails(IdmUnitProductsDTO ProDetailsDto, CancellationToken token)
        {
            var basicDetails = _mapper.Map<IdmUnitProducts>(ProDetailsDto);
            basicDetails.IsActive = false;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.IsDeleted = true;
            var response = await _appUnitDetailsRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
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

        #region AssetInformation
        public async Task<IEnumerable<IdmPromAssetDetDTO>> GetAllPromoterAssetDetails(long accountNumber, CancellationToken token)
        {
            var data = await _promoterAssetDetailsRepository.FindByMatchingPropertiesAsync(token,x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<IdmPromAssetDetDTO>>(data);
        }
        public async Task<IEnumerable<AssetTypeDetailsDTO>> GetAllAssetTypeDetails(CancellationToken token)
        {
            var data = await _assetTypeDetails.FindByMatchingPropertiesAsync(token, x => x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<AssetTypeDetailsDTO>>(data);
        }
        public async Task<IEnumerable<TblAssetCatCDTabDTO>> GetAllAssetCategaryDetails(CancellationToken token)
        {
            var data = await _assetCategaryDetails.FindByMatchingPropertiesAsync(token, x => x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<TblAssetCatCDTabDTO>>(data);
        }
        public async Task<bool> CreateAssetDetails(IdmPromAssetDetDTO AssetDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<IdmPromAssetDet>(AssetDTO);
            basicDetails.IdmAssetValue = AssetDTO.IdmAssetValue/100000;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreatedDate = DateTime.UtcNow;
            basicDetails.CreatedBy = _userInfo.Name;


            
            var response = await _promoterAssetDetailsRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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

        public async Task<bool> UpdateAssetDetails(IdmPromAssetDetDTO AssetDTO, CancellationToken token)
        {
            var currentDetails = await _promoterAssetDetailsRepository.FirstOrDefaultByExpressionAsync(x => x.IdmPromassetId == AssetDTO.IdmPromassetId, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _promoterAssetDetailsRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<IdmPromAssetDet>(AssetDTO);
                basicDetails.IdmPromassetId = 0;
                basicDetails.IdmAssetValue = AssetDTO.IdmAssetValue / 100000;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.CreatedBy = _userInfo.Name;
                basicDetails.CreatedDate = DateTime.UtcNow;

                var response = await _promoterAssetDetailsRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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
            return false;
        }

        public async Task<bool> DeleteAssetDetails(IdmPromAssetDetDTO AssetdetailsDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<IdmPromAssetDet>(AssetdetailsDTO);
            basicDetails.IsActive = false;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.IsDeleted = true;
            var response = await _promoterAssetDetailsRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
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

        #region Change bank Details
        public async Task<IEnumerable<IdmChangeBankDetailsDTO>> GetAllChangeBankDetailsList(long? accountNumber, CancellationToken token)
        {
            var data = await _changeBankDetailsRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<IdmChangeBankDetailsDTO>>(data);
        }
        public async Task<IEnumerable<IfscMasterDTO>> GetAllIfscBankDetails(CancellationToken token)
        {
            var data = await _ifscBankDetailsRepository.FindByExpressionAsync(x => x.IsActive == true,token).ConfigureAwait(false);
            return _mapper.Map<List<IfscMasterDTO>>(data);
        }

        public async Task<bool> CreateChangeBankDetails(IdmChangeBankDetailsDTO changeBankDetail, CancellationToken token)
        {
            if (changeBankDetail.UtBankPrimary)
            {
                var data = await _changeBankDetailsRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == changeBankDetail.LoanAcc && x.UtBankPrimary == true && x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
                foreach(var bankdetails in data)
                {
                    bankdetails.UtBankPrimary = false;
                    await _changeBankDetailsRepository.UpdateAsync(bankdetails,token).ConfigureAwait(false);
                }
            }
            var basicDetails = _mapper.Map<TblIdmUnitBank>(changeBankDetail);
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreatedBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var response = await _changeBankDetailsRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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

        public async Task<bool> UpdateChangeBankDetails(IdmChangeBankDetailsDTO changeBankDetail, CancellationToken token)
        {
            if (changeBankDetail.UtBankPrimary)
            {
                var data = await _changeBankDetailsRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == changeBankDetail.LoanAcc && x.UtBankPrimary == true && x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
                foreach (var bankdetails in data)
                {
                    bankdetails.UtBankPrimary = false;
                    await _changeBankDetailsRepository.UpdateAsync(bankdetails, token).ConfigureAwait(false);
                }
            }

            var currentDetails = await _changeBankDetailsRepository.FirstOrDefaultByExpressionAsync(x => x.IdmUtBankRowId == changeBankDetail.IdmUtBankRowId, token);

            if(currentDetails != null)
            {

                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _changeBankDetailsRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<TblIdmUnitBank>(changeBankDetail);
                basicDetails.IdmUtBankRowId = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.CreatedBy = _userInfo.Name;
                basicDetails.CreatedDate = DateTime.UtcNow;

                var response = await _changeBankDetailsRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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
            return false;
        }

        public async Task<bool> DeleteChangeBankDetails(IdmChangeBankDetailsDTO changeBankDetail, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmUnitBank>(changeBankDetail);
            basicDetails.IsActive = false;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.IsDeleted = true;
            var response = await _changeBankDetailsRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
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

        #region Promter Liability Info
        public async Task<IEnumerable<TblPromoterLiabDetDTO>> GetAllPromoterLiabiltyInfo(long? accountNumber, CancellationToken token)
        {
            var data = await _promoterLiabilityInfoRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            
            return _mapper.Map<List<TblPromoterLiabDetDTO>>(data);
        }

        public async Task<bool> CreatePromoterLiabiltyInfo(TblPromoterLiabDetDTO PromLiabDto, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblPromterLiabDet>(PromLiabDto);
            basicDetails.LiabVal = PromLiabDto.LiabVal/100000;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreateBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var response = await _promoterLiabilityInfoRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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

        public async Task<bool> UpdatePromoterLiabiltyInfo(TblPromoterLiabDetDTO PromLiabDto, CancellationToken token)
        {
            var currentDetails = await _promoterLiabilityInfoRepository.FirstOrDefaultByExpressionAsync(x => x.PromLiabId == PromLiabDto.PromLiabId, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _promoterLiabilityInfoRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<TblPromterLiabDet>(PromLiabDto);
                basicDetails.PromLiabId = 0;
                basicDetails.LiabVal = PromLiabDto.LiabVal / 100000;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.CreateBy = _userInfo.Name;
                basicDetails.CreatedDate = DateTime.UtcNow;

                var response = await _promoterLiabilityInfoRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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
            return false;
        }

        public async Task<bool> DeletePromoterLiabiltyInfo(TblPromoterLiabDetDTO PromLiabDto, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblPromterLiabDet>(PromLiabDto);
            basicDetails.IsActive = false;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.IsDeleted = true;
            var response = await _promoterLiabilityInfoRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
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

        #region Promoter Networth
        public async Task<IEnumerable<TblPromoterNetWortDTO>> GetAllPromoterNetWorth(long? accountNumber, CancellationToken token)
        {
            var data = await _promoterNetWorthRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<TblPromoterNetWortDTO>>(data);
        }

        public async Task<bool> SaveAssetNetworthDetails(TblPromoterNetWortDTO PromoterNWDetail, CancellationToken token)
        {
            var currentDetails = await _promoterNetWorthRepository.FirstOrDefaultByExpressionAsync(x => x.PromoterCode == PromoterNWDetail.PromoterCode, token);
            if (currentDetails != null)
            {
                currentDetails.IdmMov = PromoterNWDetail.IdmMov;
                currentDetails.IdmNetworth = PromoterNWDetail.IdmMov - currentDetails.IdmLiab;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                var response = await _promoterNetWorthRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);
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
            else
            {
                var basicDetails = _mapper.Map<TblIdmPromoterNetWork>(PromoterNWDetail);
                basicDetails.IdmMov = PromoterNWDetail.IdmMov;
                basicDetails.IdmNetworth = PromoterNWDetail.IdmMov;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.CreateBy = _userInfo.Name;
                basicDetails.CreatedDate = DateTime.UtcNow;

                var response = await _promoterNetWorthRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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
        }

        public async Task<bool> SaveLiabilityNetworthDetails(TblPromoterNetWortDTO PromoterNWDetail, CancellationToken token)
        {
            var currentDetails = await _promoterNetWorthRepository.FirstOrDefaultByExpressionAsync(x => x.PromoterCode == PromoterNWDetail.PromoterCode, token);
            if (currentDetails != null)
            {
                currentDetails.IdmLiab = PromoterNWDetail.IdmLiab;
                currentDetails.IdmNetworth = currentDetails.IdmMov - PromoterNWDetail.IdmLiab;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                var response = await _promoterNetWorthRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);
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
            else
            {
                var basicDetails = _mapper.Map<TblIdmPromoterNetWork>(PromoterNWDetail);
                basicDetails.IdmLiab = PromoterNWDetail.IdmLiab;
                basicDetails.IdmNetworth = PromoterNWDetail.IdmLiab;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.CreateBy = _userInfo.Name;
                basicDetails.CreatedDate = DateTime.UtcNow;

                var response = await _promoterNetWorthRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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
        }
        #endregion

        #region Land Assets

        public async Task<IEnumerable<TblIdmProjLandDTO>> GetAllLandDetails (long accountNumber, CancellationToken token)
        {
            var data = await _projlandRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<TblIdmProjLandDTO>>(data);
        }

        public async Task<bool> CreateLandAssets(TblIdmProjLandDTO tblIdmProjLandDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmProjland>(tblIdmProjLandDTO);
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreatedBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var response = await _projlandRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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

        #region  Indegenous Asset
        public async Task<IEnumerable<TblIdmProjPlmcDTO>> GetAllIndegenousMachinaryAssets(long accountNumber, CancellationToken token)
        {
            var data = await _projplmcRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<TblIdmProjPlmcDTO>>(data);
        }


        #endregion
    }
}
