using AutoMapper;

using KAR.KSFC.API.ServiceFacade.Internal.Interface;
using KAR.KSFC.Components.Common.Dto.Common;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service
{
    public class CommonService : ICommonService
    {
        private readonly IEntityRepository<CnstCdtab> _constituencyRepository;
        private readonly IEntityRepository<TblPurpCdtab> _purposeLoanRepository;
        private readonly IEntityRepository<TblSizeCdtab> _sizeOfFirmRepository;
        private readonly IEntityRepository<TblProdCdtab> _productRepository;
        private readonly IEntityRepository<TblPremCdtab> _premisesRepository;
        private readonly IEntityRepository<DistCdtab> _districtRepository;
        private readonly IEntityRepository<TlqCdtab> _talukaRepository;
        private readonly IEntityRepository<HobCdtab> _hobliRepository;
        private readonly IEntityRepository<VilCdtab> _villageRepository;
        private readonly IEntityRepository<TblPincodeMaster> _pincodeRepository;
        private readonly IEntityRepository<OffcCdtab> _officeBranchRepository;
        private readonly IEntityRepository<TblRegdetailsCdtab> _accountTypeRepository;
        private readonly IEntityRepository<TblPdesigCdtab> _promoterDesignation;
        private readonly IEntityRepository<TblDomiCdtab> _domicileStatus;
        private readonly IEntityRepository<TblBuildingCdtab> _promModeofAcquire;             
        private readonly IEntityRepository<TblBankfacilityCdtab> _nameofFacility;
        private readonly IEntityRepository<TblFinyearCdtab> _financialYearAssociate;
        private readonly IEntityRepository<TblFincompCdtab> _financialComponent;
        private readonly IEntityRepository<TblPjcostCdtab> _projCostComponent;
        private readonly IEntityRepository<TblPjmfcatCdtab> _meansofFinanceCategory;
        private readonly IEntityRepository<TblPjmfCdtab> _financialType;
        private readonly IEntityRepository<TblPjsecCdtab> _typesofSecurity;
        private readonly IEntityRepository<TblSecCdtab> _securityDetails;
        private readonly IEntityRepository<TblPromrelCdtab> _relationDetails;
        private readonly IEntityRepository<TblAddressCdtab> _addressType;
        private readonly IEntityRepository<TblIndCdtab> _indtryType;
        private readonly IEntityRepository<TblSecCdtab> _securityCategoryRepository;
        private readonly IEntityRepository<TblPjsecCdtab> _securityTypeRepository;
        private readonly IEntityRepository<TblSubRegistrarMast> _subRegistrarOfficeRepository;
        private readonly IEntityRepository<TbIIfscMaster> _bankIfscCodeRepository;
        private readonly IEntityRepository<TblChargeType> _chargeTypeRepository;
        private readonly IEntityRepository<TblCondTypeCdtab> _conditionTypes;
        private readonly IEntityRepository<TblCondStageMast> _conditionStages;
        private readonly IEntityRepository<TblCondStgMast> _conditionStageMaster;        
        private readonly IEntityRepository<TblCondCdtab> _conditionDescription;
        private readonly IEntityRepository<Tblfm8fm13CDTab> _form8and13list;
        private readonly IEntityRepository<TblPromCdtab> _promoterNames;
        private readonly IEntityRepository<TblPincodeStateCdtab> _promoterState;
        private readonly IEntityRepository<TblPincodeDistrictCdtab> _promoterDistrict;
        private readonly IEntityRepository<TblPsubclasCdtab> _promSubClas;
        private readonly IEntityRepository<TblPqualCdtab> _promQual;
        private readonly IEntityRepository<TblAcTypeCdtab> _acTypeRepository;
        private readonly IEntityRepository<TblStateZone> _stateZone;
        private readonly IEntityRepository<TblAssetcatCdtab> _assetCategory;
        private readonly IEntityRepository<TblAssettypeCdtab> _assetType;
        private readonly IEntityRepository<TblAllcCdTab> _allocationCode;
        private readonly IEntityRepository<TblLandTypeMast> _landTypeRepository;
        private readonly IEntityRepository<IdmOthdebitsMast> _othdebitsRepository;
        private readonly IEntityRepository<TblUMOMaster> _umoMasterRepository;
        private readonly IEntityRepository<TblDeptMaster> _deptMasterRepository;
        
        private readonly IEntityRepository<TblDsbChargeMap> _dsbchargemapRepository;

        private readonly IMapper _mapper; 


        public CommonService(IEntityRepository<TblPurpCdtab> purposeLoanRepository, IEntityRepository<TblSizeCdtab> sizeOfFirmRepository,
                               IEntityRepository<TblProdCdtab> productRepository, IEntityRepository<TblPremCdtab> premisesRepository,
                               IEntityRepository<DistCdtab> districtRepository, IEntityRepository<TlqCdtab> talukaRepository,
                               IEntityRepository<HobCdtab> hobliRepository, IEntityRepository<VilCdtab> villageRepository,
                               IEntityRepository<OffcCdtab> officeBranchRepository,
                               IEntityRepository<TblRegdetailsCdtab> accountTypeRepository,
                               IEntityRepository<CnstCdtab> constituencyRepository,
                               IEntityRepository<TblPdesigCdtab> promoterDesignation,
                               IEntityRepository<TblDomiCdtab> domicileStatus,


                               IEntityRepository<TblBuildingCdtab> promModeofAcquire,
                               IEntityRepository<TblBankfacilityCdtab> nameofFacility,
                               IEntityRepository<TblFinyearCdtab> financialYearAssociate,
                               IEntityRepository<TblFincompCdtab> financialComponent,
                               IEntityRepository<TblPjcostCdtab> projCostComponent,
                               IEntityRepository<TblPjmfcatCdtab> meansofFinanceCategory,
                               IEntityRepository<TblPjmfCdtab> financialType,
                               IEntityRepository<TblPjsecCdtab> typesofSecurity,
                               IEntityRepository<TblSecCdtab> securityDetails,
                               IEntityRepository<TblPromrelCdtab> relationDetails,
                               IEntityRepository<TblAddressCdtab> addressType,
                               IEntityRepository<TblIndCdtab> industryType,
                               IEntityRepository<TblSecCdtab> securityCategoryRepository,
                               IEntityRepository<TblPjsecCdtab> securityTypeRepository,
                               IEntityRepository<TblSubRegistrarMast> subRegistrarOfficeRepository,
                               IEntityRepository<TbIIfscMaster> bankIfscCodeRepository,
                               IEntityRepository<TblChargeType> chargeTypeRepository,
                               IEntityRepository<TblCondTypeCdtab> conditionTypes,
                               IEntityRepository<TblCondStageMast> conditionStages,
                               IEntityRepository<TblCondCdtab> conditionDescriptions,
                                IEntityRepository<Tblfm8fm13CDTab> form8and13list,
                               IMapper mapper, IEntityRepository<TblCondStgMast> conditionStageMaster,
                               IEntityRepository<TblPromCdtab> promoterNames,
                                IEntityRepository<TblPincodeStateCdtab> promoterState,
                                IEntityRepository<TblPincodeDistrictCdtab> promoterDistrict,
                                IEntityRepository<TblPsubclasCdtab> promSubClas,
                                IEntityRepository<TblAssetcatCdtab> assetCategory,
                                IEntityRepository<TblAssettypeCdtab> assetType,
                                IEntityRepository<TblPqualCdtab> promQual
                                , IEntityRepository<TblAcTypeCdtab> acTypeRepository,
                                IEntityRepository<TblStateZone> stateZone,
                                IEntityRepository<TblUMOMaster> umoMasterRepository, IEntityRepository<TblAllcCdTab> allocationCode, IEntityRepository<TblLandTypeMast> landTypeRepository, 
                                IEntityRepository<IdmOthdebitsMast> othdebitsRepository, IEntityRepository<TblPincodeMaster> pincodeRepository,
                                IEntityRepository<TblDeptMaster> deptMasterRepository, IEntityRepository<TblDsbChargeMap> dsbchargemapRepository)
        {
            _purposeLoanRepository = purposeLoanRepository;
            _sizeOfFirmRepository = sizeOfFirmRepository;
            _productRepository = productRepository;
            _premisesRepository = premisesRepository;
            _districtRepository = districtRepository;
            _talukaRepository = talukaRepository;
            _hobliRepository = hobliRepository;
            _villageRepository = villageRepository;
            _officeBranchRepository = officeBranchRepository;
            _accountTypeRepository = accountTypeRepository;
            _constituencyRepository = constituencyRepository;
            _promoterDesignation = promoterDesignation;
            _domicileStatus = domicileStatus;
            _assetCategory = assetCategory;
            _assetType = assetType;
            _promModeofAcquire = promModeofAcquire;
            _nameofFacility = nameofFacility;
            _financialYearAssociate = financialYearAssociate;
            _financialComponent = financialComponent;
            _projCostComponent = projCostComponent;
            _meansofFinanceCategory = meansofFinanceCategory;
            _financialType = financialType;
            _typesofSecurity = typesofSecurity;
            _securityDetails = securityDetails;
            _relationDetails = relationDetails;
            _addressType = addressType;
            _indtryType = industryType;
            _securityCategoryRepository = securityCategoryRepository;
            _securityTypeRepository = securityTypeRepository;
            _subRegistrarOfficeRepository = subRegistrarOfficeRepository;
            _bankIfscCodeRepository = bankIfscCodeRepository;
            _chargeTypeRepository = chargeTypeRepository;
            _conditionTypes = conditionTypes;
            _conditionStages = conditionStages;
            _conditionDescription = conditionDescriptions;
            _mapper = mapper;
            _conditionStageMaster = conditionStageMaster;
            _form8and13list = form8and13list;
            _promoterNames = promoterNames;
            _promoterState = promoterState;
            _promoterDistrict = promoterDistrict;
            _promSubClas = promSubClas;
            _promQual = promQual;
            _acTypeRepository = acTypeRepository;
            _stateZone = stateZone;
            _allocationCode = allocationCode;
            _landTypeRepository = landTypeRepository;
            _othdebitsRepository = othdebitsRepository;
            _pincodeRepository = pincodeRepository;
            _umoMasterRepository = umoMasterRepository;
            _deptMasterRepository = deptMasterRepository;
            _dsbchargemapRepository = dsbchargemapRepository;
        }

        public async Task<IEnumerable<DropDownDTO>> GetOfficeBranchAsync(CancellationToken token)
        {
            var data = await _officeBranchRepository.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.OffcNam, Value = x.OffcCd }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetLoanPurposeAsync(CancellationToken token)
        {
            var data = await _purposeLoanRepository.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.PurpDets, Value = x.PurpCd }).ToList();
        }

       

        public async Task<IEnumerable<DropDownDTO>> GetNatureOfPremisesAsync(CancellationToken token)
        {
            var data = await _premisesRepository.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.PremDets, Value = x.PremCd }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetSizeOfFirmAsync(CancellationToken token)
        {
            var data = await _sizeOfFirmRepository.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.SizeDets, Value = x.SizeCd }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetDistrictAsync(CancellationToken token)
        {
            var data = await _districtRepository.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.DistNam, Value = x.DistCd }).ToList();

        }

        public async Task<IEnumerable<DropDownDTO>> GetTalukaAsync(int DistrictId, CancellationToken token)
        {
            var data = await _talukaRepository.FindByExpressionAsync(x => x.DistCd == DistrictId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.TlqNam, Value = x.TlqCd }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetHobliAsync(int TalukaId, CancellationToken token)
        {
            var data = await _hobliRepository.FindByExpressionAsync(x => x.TlqCd == TalukaId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.HobNam, Value = x.HobCd }).ToList();
        }
        public async Task<IEnumerable<DropDownDTO>> GetVillageAsync(int HobliId, CancellationToken token)
        {
            var data = await _villageRepository.FindByExpressionAsync(x => x.HobCd == HobliId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.VilNam, Value = x.VilCd }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetRegistrationTypeAsync(CancellationToken token)
        {
            var data = await _accountTypeRepository.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.RegrefDets, Value = x.RegrefCd }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetConstituenttypeAsync(CancellationToken token)
        {
            var data = await _constituencyRepository.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.CnstDets, Value = Convert.ToInt32(x.CnstCd) }).ToList();
        }
        public async Task<IEnumerable<ConstitutionMasterDTO>> GetAllConstitutionData(CancellationToken token)
        {
            var data = await _constituencyRepository.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return _mapper.Map<List<ConstitutionMasterDTO>>(data);

        }
        public async Task<IEnumerable<DropDownDTO>> GetPositionDesignationAsync(CancellationToken token)
        {
            var data = await _promoterDesignation.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.PdesigDets, Value = Convert.ToInt32(x.PdesigCd) }).ToList();
        }
        public async Task<IEnumerable<DropDownDTO>> GetDomicileStatusAsync(CancellationToken token)
        {
            var data = await _domicileStatus.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.DomDets, Value = Convert.ToInt32(x.DomCd) }).ToList();
        }

        


        
        public async Task<IEnumerable<DropDownDTO>> GetModeofAcquireAsync(CancellationToken token)
        {
            var data = await _promModeofAcquire.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.BldDesc, Value = Convert.ToInt32(x.BldCd) }).ToList();
        }
        public async Task<IEnumerable<DropDownDTO>> GetNameOFFacilityAsync(CancellationToken token)
        {
            var data = await _nameofFacility.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.BfacilityDesc, Value = Convert.ToInt32(x.BfacilityCode) }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetFinancialYearAsync(CancellationToken token)
        {
            var data = await _financialYearAssociate.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.FinyearDesc, Value = Convert.ToInt32(x.FinyearCode) }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetFinancialComponentAsync(CancellationToken token)
        {
            var data = await _financialComponent.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.FincompDets, Value = Convert.ToInt32(x.FincompCd) }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetPojectCostComponentAsync(CancellationToken token)
        {
            var data = await _projCostComponent.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.PjcostDets, Value = Convert.ToInt32(x.PjcostCd) }).ToList();
        }
        public async Task<IEnumerable<DropDownDTO>> GetMeansofFinanceCategoryAsync(CancellationToken token)
        {
            var data = await _meansofFinanceCategory.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.PjmfDets, Value = Convert.ToInt32(x.MfcatCd) }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetFinanceTypeAsync(int mfCategory, CancellationToken token)
        {
            var data = await _financialType.FindByExpressionAsync(x => x.MfcatCd == mfCategory && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.PjmfDets, Value = Convert.ToInt32(x.PjmfCd) }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetSecurityTypeAsync(CancellationToken token)
        {
            var data = await _typesofSecurity.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.SecDets, Value = Convert.ToInt32(x.SecCode) }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetSecurityDetailsAsync(CancellationToken token)
        {
            var data = await _securityDetails.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.SecDets, Value = Convert.ToInt32(x.SecCd) }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetRelationAsync(CancellationToken token)
        {
            var data = await _relationDetails.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.PromrelDets, Value = Convert.ToInt32(x.PromrelCd) }).ToList();
        }

        public async Task<VillageTalukaHobliDistDTO> GetVillageTalukaHobliDistAsync(int VillageCode, CancellationToken token)
        {
            var Village = await _villageRepository.FirstOrDefaultByExpressionAsync(a => a.VilCd == VillageCode && a.IsActive == true && a.IsDeleted == false, token).ConfigureAwait(false);
            var Hobli = await _hobliRepository.FirstOrDefaultByExpressionAsync(a => a.HobCd == Village.HobCd && a.IsActive == true && a.IsDeleted == false, token).ConfigureAwait(false);
            var Taluka = await _talukaRepository.FirstOrDefaultByExpressionAsync(a => a.TlqCd == Hobli.TlqCd && a.IsActive == true && a.IsDeleted == false, token).ConfigureAwait(false);
            var District = await _districtRepository.FirstOrDefaultByExpressionAsync(a => a.DistCd == Taluka.DistCd && a.IsActive == true && a.IsDeleted == false, token).ConfigureAwait(false);

            var VillageTalukaHobliDist = new VillageTalukaHobliDistDTO
            {
                VillageCode = Village.VilCd,
                VillageName = Village.VilNam,
                HobliCode = Hobli.HobCd,
                HobliName = Hobli.HobNam,
                TalukaCode = Taluka.TlqCd,
                TalukaName = Taluka.TlqNam,
                DistrictCode = District.DistCd,
                DistrictName = District.DistNam
            };
            return VillageTalukaHobliDist;
        }

        public async Task<IEnumerable<DropDownDTO>> GetAddressTypeAsync(CancellationToken token)
        {
            var data = await _addressType.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.AddtypeDets, Value = Convert.ToInt32(x.AddtypeCd) }).ToList();
        }

       

        public async Task<IEnumerable<DropDownDTO>> GetAllPromotorClass(CancellationToken token)
        {
            var list = new List<DropDownDTO>
            {
                 new DropDownDTO
                 {
                      Text="SCHEDULED CASTE",
                      Value=10
                 },
                      new DropDownDTO
                 {
                      Text="SCHEDULED TRIBE",
                      Value=20
                 },
                      new DropDownDTO
                 {
                      Text="BACKWARD COMMUNITY",
                      Value=30
                 },
                        new DropDownDTO
                 {
                      Text="GENERAL",
                      Value=50
                 }
                        ,
                        new DropDownDTO
                 {
                      Text="MINORITY COMMUNITY",
                      Value=60
                 }
                        ,
                        new DropDownDTO
                 {
                      Text="SIKHS",
                      Value=61
                        }  ,
                        new DropDownDTO
                 {
                      Text="MUSLIMS",
                      Value=62
                        }   ,
                        new DropDownDTO
                 {
                      Text="CHRISTIANS",
                      Value=63

                 },
                         new DropDownDTO
                 {
                      Text="ZORASTRIAN",
                      Value=64

                 },
                          new DropDownDTO
                 {
                      Text="NEO-BUDDHIST",
                      Value=65

                 },
                          new DropDownDTO
                 {
                      Text="JAIN",
                      Value=66

                 }
    };
            return list.OrderBy(x=>x.Text);
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllSecurityCategoryAsync(CancellationToken token)
        {
            var data = await _securityCategoryRepository.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.SecDets, Value = x.SecCd }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllSecurityTypesAsync(CancellationToken token)
        {
            var data = await _securityTypeRepository.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.SecDets, Value = x.SecCode }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllSubRegistrarOfficeAsync(CancellationToken token)
        {
            var data = await _subRegistrarOfficeRepository.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.SrOfficeName, Value = Convert.ToInt32(x.SrOfficeId) }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllBankIfscCode(CancellationToken token)
        {
            var data = await _bankIfscCodeRepository.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.IFSCCode, Value=x.IFSCRowID }).ToList();

        }

        public async Task<IEnumerable<DropDownDTO>> GetAllChargeType(CancellationToken token)
        {
            var data = await _chargeTypeRepository.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.ChargeType,Value=x.Id }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllConditionTypes(CancellationToken token)
        {
            var data = await _conditionTypes.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.CondTypeDets, Value = x.CondTypeCd }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllConditionStages(CancellationToken token)
        {
            var data = await _conditionStages.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.CondStgDets, Value = Convert.ToInt32(x.CondStageId) }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllConditionStageMaster(CancellationToken token)
        {
            var data = await _conditionStageMaster.FindByExpressionAsync(x=>x.CondStgId > 1,token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.CondStgDesc, Value = x.CondStgId }).ToList();
        }
        
        public async Task<IEnumerable<DropDownDTO>> GetAllConditionDesc(CancellationToken token)
        {
            var data = await _conditionDescription.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.CondDets, Value = Convert.ToInt32(x.CondCd) }).ToList();
        }
        public async Task<IEnumerable<DropDownDTO>> GetAllForm8and13(CancellationToken token)
        {
            var data = await _form8and13list.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.FormType, Value = x.FormTypeCD }).ToList();
        }
        #region Promter Address  
        public async Task<IEnumerable<DropDownDTO>> GetAllPromoterNames(CancellationToken token)
        {
            var data = await _promoterNames.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.PromoterName, Value = (int)x.PromoterCode}).ToList();
        }
        public async Task<IEnumerable<DropDownDTO>> GetAllPromoterPhNo(CancellationToken token)
        {
            var data = await _promoterNames.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.PromoterMobno.ToString(), Value = (int)x.PromoterCode}).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllPromoterState(CancellationToken token)
        {
            var data = await _promoterState.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.PincodeStateDesc, Value = x.PincodeStateCd }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllPromoterDistrict(CancellationToken token)
        {
            var data = await _promoterDistrict.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.PincodeDistrictDesc, Value = x.PincodeDistrictCd }).ToList();
        }
        #endregion
        #region  Unit Information Address Details 
        public async Task<IEnumerable<DropDownDTO>> GetAllDistrictDetails(CancellationToken token)
        {
            var data = await _districtRepository.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.DistNam, Value = x.DistCd }).ToList();
        }
        public async Task<IEnumerable<DropDownDTO>> GetAllTalukDetails(CancellationToken token)
        {
            var data = await _talukaRepository.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.TlqNam, Value = x.TlqCd}).ToList();
        }
        public async Task<IEnumerable<DropDownDTO>> GetAllHobliDetails(CancellationToken token)
        {
            var data = await _hobliRepository.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.HobNam, Value = x.HobCd }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllVillageDetails(CancellationToken token)
        {
            var data = await _villageRepository.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.VilNam, Value = x.VilCd }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllPincodeDetails(CancellationToken token)
        {
            var data = await _pincodeRepository.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.PincodeCd.ToString(), Value = x.PincodeRowId }).ToList();
        }
        #endregion
        //DEV
        public async Task<IEnumerable<DropDownDTO>> GetAllPromotorSubClas(CancellationToken token)
        {
            var data = await _promSubClas.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.PsubclasDesc, Value = x.PsubclasCd }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllPromotorQual(CancellationToken token)
        {
            var data = await _promQual.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.PqualDesc, Value = x.PqualCd }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllAccountType(CancellationToken token)
        {
            var data = await _acTypeRepository.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.AcTypeDets, Value = x.AcTypeCd }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetNameOfProductAsync(CancellationToken token)
         {
            var data = await _productRepository.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.ProdDets, Value = x.Id }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetIndustryType(CancellationToken token)
        {
            var data = await _indtryType.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.IndDets, Value = Convert.ToInt32(x.IndCd) }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetAssetCategoryAsync(CancellationToken token)
        {
            var data = await _assetCategory.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.AssetcatDets, Value = Convert.ToInt32(x.AssetcatCd) }).ToList();
        }


        public async Task<IEnumerable<DropDownDTO>> GetAllStateZone(CancellationToken token)
        {
            var data = await _stateZone.FindByExpressionAsync(x => x.IsActive == true, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.StateZoneDesc, Value = x.Id }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetAssetTypeAsync(CancellationToken token)
        {
            var data = await _assetType.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.AssettypeDets, Value = Convert.ToInt32(x.AssettypeCd) }).ToList();
        }
        //Added By Gagana
        public async Task<IEnumerable<DropDownDTO>> GetAllAllocationCode(CancellationToken token)
        {
            var data = await _allocationCode.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.AllcDets, Value = x.AllcId }).ToList();
        }
        
        public async Task<IEnumerable<DropDownDTO>> GetAllLandType(CancellationToken token)
        {
            var data = await _landTypeRepository.FindByExpressionAsync(x => x.IsActive == true , token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.LandTypeDesc.ToString(), Value = x.LandTypeId }).ToList();
        }

        //Added By Gagana
        public async Task<IEnumerable<DropDownDTO>> GetAllOtherDebitsDetails(CancellationToken token)
        {
            var data = await _othdebitsRepository.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.DsbOthdebitDesc.ToString(), Value = x.DsbOthdebitId }).ToList();
        }

        public async Task<IEnumerable<DropDownDTO>> GetAllUomMaster(CancellationToken token)
        {
            var data = await _umoMasterRepository.FindByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.UmoDesc, Value = (int)x.UmoId }).ToList();
        }


        public async Task<IEnumerable<DropDownDTO>> GetAllFinanceType(CancellationToken token)
        {
            var data = await _financialType.FindByExpressionAsync(x =>  x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.PjmfDets, Value = Convert.ToInt32(x.PjmfCd) }).ToList();
        }
         public async Task<IEnumerable<DropDownDTO>> GetAllDeptMaster(CancellationToken token)
        {
            var data = await _deptMasterRepository.FindByExpressionAsync(x =>  x.DeptName != null, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.DeptName, Value = Convert.ToInt32(x.DeptCode) }).ToList();
        }

           public async Task<IEnumerable<DropDownDTO>> GetAllDsbChargeMaping(CancellationToken token)
        {
            var data = await _dsbchargemapRepository.FindByExpressionAsync(x =>  x.DataFieldName != null, token).ConfigureAwait(false);
            return data.Select(x => new DropDownDTO { Text = x.DataFieldName, Value = Convert.ToInt32(x.DsbOthdebitId) }).ToList();
        }


    }   
}
