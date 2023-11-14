using AutoMapper;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.InspectionOfUnitModule;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using KAR.KSFC.Components.Data.Models;
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
using KAR.KSFC.API.ServiceFacade.Internal.Interface.UnitDetails.EnquirySubmission;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.IDMModule.InspectionOfUnitModule
{
    public class InspectionOfUnitService : IInspectionOfUnitService
    {
        private readonly IEntityRepository<TblAppLoanMast> _loanRepository;

        private readonly IEntityRepository<TblIdmDchgBuildingDet> _buildingInspectionRepository;
        private readonly IEntityRepository<TblIdmDchgLand> _landInspectionRepository;
        private readonly IEntityRepository<TblIdmDspInsp> _InspectionDetailRepository;
        private readonly IEntityRepository<TblIdmDChgFurn> _furnitureInspectionDetailRepository;
        private readonly IEntityRepository<TblIdmIrBldgMat> _buildingMaterialRepository;
        private readonly IEntityRepository<TblIdmDchgWorkingCapital> _WokingCapitalRepository;
        private readonly IEntityRepository<TblIdmDchgImportMachinery> _importMachineryRepository;
        private readonly IEntityRepository<TblIdmDchgMeansOfFinance> _meansOfFinanceRepository;
        private readonly IEntityRepository<TblIdmBuildingAcquisitionDetails> _buildingAcquisitionDetailsRepository;
        private readonly IEntityRepository<TblDsbStatImp> _dsbStatImp;
        private readonly IEntityRepository<TblIdmIrFurn> _idmIrFurn;
        private readonly IEntityRepository<TblIdmIrLand> _idmIrLand;
        private readonly IEntityRepository<TblIdmIrPlmc> _idmIrPlmc;
        private readonly IEntityRepository<TblIdmDeedDet> _primaryColleteralRepository;
        private readonly IEntityRepository<TblIdmBuildingAcquisitionDetails> _buildingDetailsRepository;
        private readonly IEntityRepository<TblIdmIrFurn> _furnitureAcuisitionRepository;

        private readonly UserInfo _userInfo;

        private readonly IEntityRepository<TblIdmDchgIndigenousMachineryDet> _indigenousMachineryRepository;
        private readonly IEntityRepository<TblIdmDchgProjectCost> _projectCostRepository;
        private readonly IEntityRepository<TblIdmDsbLetterOfCredit> _letterOfCreditRepository;
        private readonly IEntityRepository<TblMachineryStatus> _machinaryStatus;
        private readonly IEntityRepository<TblProcureMast> _procureStatus;
        private readonly IEntityRepository<TblCurrencyMast> _currencyStatus;


        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;


        public InspectionOfUnitService(IEntityRepository<TblAppLoanMast> loanRepository, IUnitOfWork work, IMapper mapper,
                                        IEntityRepository<TblIdmDchgLand> landInspectionRepository,
                                        IEntityRepository<TblIdmDspInsp> inspectionDetailRepository,
                                        IEntityRepository<TblIdmDchgBuildingDet> buildingInspectionRepository,
                                        IEntityRepository<TblIdmDchgWorkingCapital> WokingCapitalRepository,
                                         IEntityRepository<TblIdmIrBldgMat> buildingMaterialRepository,
                                         IEntityRepository<TblIdmDchgImportMachinery> importMachineryRepository,
                                        IEntityRepository<TblIdmDChgFurn> furnitureInspectionDetailRepository,
                                        IEntityRepository<TblIdmDchgIndigenousMachineryDet> indigenousMachineryRepository,
                                        UserInfo userInfo, IEntityRepository<TblIdmBuildingAcquisitionDetails> buildingDetailsRepository,
                                        IEntityRepository<TblIdmIrFurn> furnitureAcuisitionRepository, IEntityRepository<TblIdmIrLand> landacquitionRepository,
                                        IEntityRepository<TblIdmDchgMeansOfFinance> meansOfFinanceRepository, IEntityRepository<TblIdmDsbLetterOfCredit> letterOfCreditRepository,
                                         IEntityRepository<TblIdmDchgProjectCost> projectCostRepository, IEntityRepository<TblIdmIrLand> idmIrLand, IEntityRepository<TblMachineryStatus> machinaryStatus, IEntityRepository<TblProcureMast> procureStatus, IEntityRepository<TblCurrencyMast> currencyStatus, IEntityRepository<TblIdmIrPlmc> idmIrPlmc
                                       , IEntityRepository<TblIdmBuildingAcquisitionDetails> buildingAcquisitionDetailsRepository
                                        , IEntityRepository<TblIdmDeedDet> primaryColleteralRepository, IEntityRepository<TblIdmIrFurn> idmIrFurn, IEntityRepository<TblDsbStatImp> dsbStatImp)
        {
            _loanRepository = loanRepository;
            _work = work;
            _mapper = mapper;
            _dsbStatImp = dsbStatImp;
            _landInspectionRepository = landInspectionRepository;
            _InspectionDetailRepository = inspectionDetailRepository;
            _buildingInspectionRepository = buildingInspectionRepository;
            _WokingCapitalRepository = WokingCapitalRepository;
            _buildingMaterialRepository = buildingMaterialRepository;
            _furnitureInspectionDetailRepository = furnitureInspectionDetailRepository;
            _indigenousMachineryRepository = indigenousMachineryRepository;
            _importMachineryRepository = importMachineryRepository;
            _buildingAcquisitionDetailsRepository = buildingAcquisitionDetailsRepository;
            _meansOfFinanceRepository = meansOfFinanceRepository;
            _letterOfCreditRepository = letterOfCreditRepository;
            _primaryColleteralRepository = primaryColleteralRepository;
            _userInfo = userInfo;
            _projectCostRepository = projectCostRepository;
            _idmIrLand = idmIrLand;
            _idmIrFurn = idmIrFurn;
            _idmIrPlmc = idmIrPlmc;
            _buildingDetailsRepository = buildingDetailsRepository;
            _furnitureAcuisitionRepository = furnitureAcuisitionRepository;
            _machinaryStatus = machinaryStatus;
            _procureStatus = procureStatus;
            _currencyStatus = currencyStatus;
            _dsbStatImp = dsbStatImp;
        }

        //Author: Manoj Date:25/08/2022
        public async Task<IEnumerable<LoanAccountNumberDTO>> GetAccountNumber(CancellationToken token)
        {
            var data = await _loanRepository.FindByMatchingPropertiesAsync(token, x => x.IsActive == true,
                incOff => incOff.OffcCdtab,
                incUnit => incUnit.TblUnitMast,
                 incLoanAcc => incLoanAcc.TblIdmLegalWorkflow
                ).ConfigureAwait(false);

            return _mapper.Map<List<LoanAccountNumberDTO>>(data);
        }

        #region LandInspection 
        public async Task<IEnumerable<IdmDchgLandDetDTO>> GetAllLandInspectionListAsync(long accountNumber, long InspectionId, CancellationToken token)
        {
            List<TblIdmDchgLand> landInspectionList;
            if (InspectionId != 0)
            {
                landInspectionList = await _landInspectionRepository.FindByExpressionAsync(x => x.LoanAcc == accountNumber && x.DcLndIno == InspectionId && x.IsActive == true && x.IsDeleted == false, token);

            }
            else
            {
                landInspectionList = await _landInspectionRepository.FindByExpressionAsync(x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false, token);

            }
            return _mapper.Map<List<IdmDchgLandDetDTO>>(landInspectionList);
        }
        //Author Manoj on 26/08/2022

        public async Task<bool> UpdateLandInspectionDetails(IdmDchgLandDetDTO landInspectionDTO, CancellationToken token)
        {
            var currentDetails = await _landInspectionRepository.FirstOrDefaultByExpressionAsync(x => x.DcLndRowId == landInspectionDTO.DcLndRowId, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = false;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _landInspectionRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<TblIdmDchgLand>(landInspectionDTO);
                basicDetails.DcLndRowId = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.ModifiedBy = _userInfo.Name;
                basicDetails.ModifiedDate = DateTime.UtcNow;

                await _landInspectionRepository.AddAsync(basicDetails, token).ConfigureAwait(false);

                var landAcquisitiondetails = await _idmIrLand.FirstOrDefaultByExpressionAsync(x => x.IrlId == landInspectionDTO.IrlId, token);
                if (landAcquisitiondetails != null)
                {
                    landAcquisitiondetails.IsDeleted = false;
                    landAcquisitiondetails.IsActive = false;
                    landAcquisitiondetails.ModifiedBy = _userInfo.Name;
                    landAcquisitiondetails.ModifiedDate = DateTime.UtcNow;
                    await _idmIrLand.UpdateAsync(landAcquisitiondetails, token).ConfigureAwait(false);

                    var landacquisitionDetails = new TblIdmIrLand();
                    landacquisitionDetails.LoanAcc = basicDetails.LoanAcc;
                    landacquisitionDetails.LoanSub = basicDetails.LoanSub;
                    landacquisitionDetails.OffcCd = basicDetails.OffcCd;
                    landacquisitionDetails.IrlLandCost = basicDetails.DcLndAmt;

                    landacquisitionDetails.IrlIno = basicDetails.DcLndIno;
                    landacquisitionDetails.IrlDevCost = basicDetails.DcLndDevCst;
                    landacquisitionDetails.IrlLandFinance = basicDetails.DcLndLandFinance;
                    landacquisitionDetails.IrlArea = basicDetails.DcLndArea;
                    landacquisitionDetails.IrlLandTy = basicDetails.DcLndType;
                    landacquisitionDetails.IrlSecValue = basicDetails.DcLndSecCreated;
                    //newly Added 
                    landacquisitionDetails.IrlAreaIn = landInspectionDTO.IrlAreaIn;
                    landacquisitionDetails.IrlSecValue = landInspectionDTO.IrlSecValue;
                    landacquisitionDetails.IrlRelStat = Convert.ToInt32(landInspectionDTO.IrlRelStat);
                    landacquisitionDetails.UniqueId = basicDetails.UniqueId;
                    landacquisitionDetails.IsActive = true;
                    landacquisitionDetails.IsDeleted = false;
                    landacquisitionDetails.CreatedBy = _userInfo.Name;
                    landacquisitionDetails.CreatedDate = DateTime.UtcNow;

                    await _idmIrLand.AddAsync(landacquisitionDetails, token).ConfigureAwait(false);
                }
                await _work.CommitAsync(token).ConfigureAwait(false);

            }


            return false;
        }
        //Author Manoj on 26/08/2022
        public async Task<bool> CreateLandInspectionDetails(IdmDchgLandDetDTO landInspectionDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmDchgLand>(landInspectionDTO);
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreatedBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var landacquisitionDetails = new TblIdmIrLand();
            landacquisitionDetails.LoanAcc = basicDetails.LoanAcc;
            landacquisitionDetails.LoanSub = basicDetails.LoanSub;
            landacquisitionDetails.OffcCd = basicDetails.OffcCd;
            landacquisitionDetails.IrlLandCost = basicDetails.DcLndAmt;

            landacquisitionDetails.IrlIno = basicDetails.DcLndIno;
            landacquisitionDetails.IrlDevCost = basicDetails.DcLndDevCst;
            landacquisitionDetails.IrlLandFinance = basicDetails.DcLndLandFinance;
            landacquisitionDetails.IrlArea = basicDetails.DcLndArea;
            landacquisitionDetails.IrlLandTy = basicDetails.DcLndType;
            landacquisitionDetails.IrlSecValue = basicDetails.DcLndSecCreated;
            //newly Added 
            landacquisitionDetails.IrlAreaIn = landInspectionDTO.IrlAreaIn;
            landacquisitionDetails.IrlSecValue = landInspectionDTO.IrlSecValue;
            landacquisitionDetails.IrlRelStat = Convert.ToInt32(landInspectionDTO.IrlRelStat);
            landacquisitionDetails.UniqueId = basicDetails.UniqueId;
            landacquisitionDetails.IsActive = true;
            landacquisitionDetails.IsDeleted = false;
            landacquisitionDetails.CreatedBy = _userInfo.Name;
            landacquisitionDetails.CreatedDate = DateTime.UtcNow;

            var primaryColleteral = new TblIdmDeedDet();
            primaryColleteral.LoanAcc = basicDetails.LoanAcc;
            primaryColleteral.LoanSub = basicDetails.LoanSub;
            primaryColleteral.OffcCd = basicDetails.OffcCd;
            primaryColleteral.IsActive = true;
            primaryColleteral.IsDeleted = false;
            primaryColleteral.CreatedBy = _userInfo.Name;
            primaryColleteral.CreatedDate = DateTime.UtcNow;

            var response = await _landInspectionRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _primaryColleteralRepository.AddAsync(primaryColleteral, token).ConfigureAwait(false);
            await _idmIrLand.AddAsync(landacquisitionDetails, token).ConfigureAwait(false);
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
        //Author Manoj on 26/08/2022
        public async Task<bool> DeleteLandInspectionDetails(IdmDchgLandDetDTO landInspectionDTO, CancellationToken token)
        {
            //var basicDetails = _mapper.Map<TblIdmDchgLand>(landInspectionDTO);
            var basicDetails = await _landInspectionRepository.FirstOrDefaultByExpressionAsync(x => x.UniqueId == landInspectionDTO.UniqueId, token);
            if (basicDetails != null)
            {
                basicDetails.IsActive = false;
                basicDetails.IsDeleted = true;
                basicDetails.ModifiedDate = DateTime.UtcNow;
                basicDetails.ModifiedBy = _userInfo.Name;
                await _landInspectionRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
            }

            var landAcquisitiondetails = await _idmIrLand.FirstOrDefaultByExpressionAsync(x => x.UniqueId == landInspectionDTO.UniqueId, token);
            if (landAcquisitiondetails != null)
            {
                landAcquisitiondetails.IsActive = false;
                landAcquisitiondetails.IsDeleted = true;
                landAcquisitiondetails.ModifiedDate = DateTime.UtcNow;
                landAcquisitiondetails.ModifiedBy = _userInfo.Name;
                await _idmIrLand.UpdateAsync(landAcquisitiondetails, token).ConfigureAwait(false);
            }

            var response = await _work.CommitAsync(token).ConfigureAwait(false);
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


        #region InpsectionDetail
        public async Task<IEnumerable<IdmDspInspDTO>> GetAllInspectionDetailsList(long accountNumber, CancellationToken token)
        {
            var InspectionList = await _InspectionDetailRepository.FindByExpressionAsync(x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false, token);
            return _mapper.Map<List<IdmDspInspDTO>>(InspectionList);
        }
        public async Task<bool> UpdateInspectionDetails(IdmDspInspDTO InspectionDTO, CancellationToken token)
        {
            var currentDetails = await _InspectionDetailRepository.FirstOrDefaultByExpressionAsync(x => x.DinRowID == InspectionDTO.DinRowID, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _InspectionDetailRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);


                var basicDetails = _mapper.Map<TblIdmDspInsp>(InspectionDTO);
                basicDetails.DinRowID = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.ModifiedBy = _userInfo.Name;
                basicDetails.ModifiedDate = DateTime.UtcNow;

                var response = await _InspectionDetailRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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

        public async Task<bool> CreateInspectionDetails(IdmDspInspDTO inspectionDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmDspInsp>(inspectionDTO);
            basicDetails.IsActive = true;
            basicDetails.CreateBy = _userInfo.Name;
            basicDetails.IsDeleted = false;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var response = await _InspectionDetailRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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

        public async Task<bool> DeleteInspectionDetails(IdmDspInspDTO InspectionDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmDspInsp>(InspectionDTO);
            basicDetails.IsActive = false;
            basicDetails.IsDeleted = true;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            var response = await _InspectionDetailRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
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


        #region Building Inspection
        //Author: Swetha  Date: 25/08/2022
        public async Task<IEnumerable<IdmDchgBuildingDetDTO>> GetAllBuildingInspectionList(long accountNumber, long InspectionId, CancellationToken token)
        {
            List<TblIdmDchgBuildingDet> landInspectionList;
            if (InspectionId != 0)
            {
                landInspectionList = await _buildingInspectionRepository.FindByExpressionAsync(x => x.LoanAcc == accountNumber /*&& x.DcBdgIno == InspectionId && x.IsActive == true && x.IsDeleted == false*/, token);

            }
            else
            {
                landInspectionList = await _buildingInspectionRepository.FindByExpressionAsync(x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false, token);

            }
            return _mapper.Map<List<IdmDchgBuildingDetDTO>>(landInspectionList);
        }


        public async Task<bool> CreateBuilidngInspectionDetails(IdmDchgBuildingDetDTO BuildingDto, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmDchgBuildingDet>(BuildingDto);
            basicDetails.CreatedDate = DateTime.UtcNow;
            basicDetails.CreatedBy = _userInfo.Name;

            var buildingAcquisitionDetails = new TblIdmBuildingAcquisitionDetails();

            buildingAcquisitionDetails.LoanAcc = basicDetails.LoanAcc;
            buildingAcquisitionDetails.LoanSub = basicDetails.LoanSub;
            buildingAcquisitionDetails.OffcCd = basicDetails.OffcCd;

            buildingAcquisitionDetails.IrbItem = basicDetails.DcBdgItmNo;
            buildingAcquisitionDetails.IrbIno = basicDetails.DcBdgIno;
            buildingAcquisitionDetails.IrbBldgDetails = basicDetails.DcBdgDets;
            buildingAcquisitionDetails.IrbAPArea = basicDetails.DcBdgAplnth;
            buildingAcquisitionDetails.IrbATCost = basicDetails.DcBdgAtCost;
            buildingAcquisitionDetails.IrbArea = basicDetails.DcBdgPlnth;
            buildingAcquisitionDetails.IrbValue = basicDetails.DcBdgUcost;
            buildingAcquisitionDetails.IrbSecValue = BuildingDto.IrbSecValue;
            //buildingAcquisitionDetails.IrbNo = basicDetails.DcBdgIno;
            buildingAcquisitionDetails.IrbStatus = basicDetails.DcBdgStat;
            buildingAcquisitionDetails.IrbSecValue = basicDetails.DcBdgSecCreatd;
            buildingAcquisitionDetails.IrbBldgDetails = basicDetails.DcBdgDets;

            //newly added
            buildingAcquisitionDetails.IrbBldgConstStatus = BuildingDto.IrbBldgConstStatus;
            buildingAcquisitionDetails.IrbUnitCost = BuildingDto.IrbUnitCost;
            buildingAcquisitionDetails.IrbPercentage = BuildingDto.IrbPercentage;
            buildingAcquisitionDetails.RoofType = BuildingDto.RoofType;
            buildingAcquisitionDetails.UniqueId = basicDetails.UniqueId;
            //buildingAcquisitionDetails.Irbid = BuildingDto.Irbid;
            buildingAcquisitionDetails.CreatedDate = DateTime.UtcNow;
            buildingAcquisitionDetails.CreatedBy = _userInfo.Name;
            buildingAcquisitionDetails.IsActive = true;
            buildingAcquisitionDetails.IsDeleted = false;

            var primaryColleteral = new TblIdmDeedDet();
            primaryColleteral.LoanAcc = basicDetails.LoanAcc;
            primaryColleteral.LoanSub = basicDetails.LoanSub;
            primaryColleteral.OffcCd = basicDetails.OffcCd;
            primaryColleteral.IsActive = true;
            primaryColleteral.IsDeleted = false;
            primaryColleteral.CreatedBy = _userInfo.Name;
            primaryColleteral.CreatedDate = DateTime.UtcNow;
            var response = await _buildingInspectionRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _primaryColleteralRepository.AddAsync(primaryColleteral, token).ConfigureAwait(false);
            await _buildingAcquisitionDetailsRepository.AddAsync(buildingAcquisitionDetails, token).ConfigureAwait(false);
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
        public async Task<bool> UpdateBuilidngInspectionDetails(IdmDchgBuildingDetDTO BuildingDto, CancellationToken token)
        {
            var currentDetails = await _buildingInspectionRepository.FirstOrDefaultByExpressionAsync(x => x.DcBdgRowId == BuildingDto.DcBdgRowId, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = false;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _buildingInspectionRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<TblIdmDchgBuildingDet>(BuildingDto);
                basicDetails.DcBdgRowId = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.ModifiedBy = _userInfo.Name;
                basicDetails.ModifiedDate = DateTime.UtcNow;
                await _buildingInspectionRepository.AddAsync(basicDetails, token).ConfigureAwait(false);

                var currentDetails1 = await _buildingAcquisitionDetailsRepository.FirstOrDefaultByExpressionAsync(x => x.Irbid == BuildingDto.Irbid, token);
                if (currentDetails1 != null)
                {
                    currentDetails1.IsDeleted = false;
                    currentDetails1.IsActive = false;
                    currentDetails1.ModifiedBy = _userInfo.Name;
                    currentDetails1.ModifiedDate = DateTime.UtcNow;
                    await _buildingAcquisitionDetailsRepository.UpdateAsync(currentDetails1, token).ConfigureAwait(false);


                    var buildingAquisition = new TblIdmBuildingAcquisitionDetails();
                    buildingAquisition.LoanAcc = basicDetails.LoanAcc;
                    buildingAquisition.LoanSub = basicDetails.LoanSub;
                    buildingAquisition.OffcCd = basicDetails.OffcCd;

                    buildingAquisition.IrbItem = basicDetails.DcBdgItmNo;
                    buildingAquisition.IrbIno = basicDetails.DcBdgIno;
                    buildingAquisition.IrbBldgDetails = basicDetails.DcBdgDets;
                    buildingAquisition.IrbAPArea = basicDetails.DcBdgAplnth;
                    buildingAquisition.IrbATCost = basicDetails.DcBdgAtCost;
                    buildingAquisition.IrbArea = basicDetails.DcBdgPlnth;
                    buildingAquisition.IrbValue = basicDetails.DcBdgUcost;
                    buildingAquisition.IrbStatus = basicDetails.DcBdgStat;
                    buildingAquisition.IrbSecValue = basicDetails.DcBdgSecCreatd;
                    buildingAquisition.IrbBldgDetails = basicDetails.DcBdgDets;
                    buildingAquisition.IrbSecValue = BuildingDto.IrbSecValue;

                    buildingAquisition.IrbBldgConstStatus = BuildingDto.IrbBldgConstStatus;
                    buildingAquisition.IrbUnitCost = BuildingDto.IrbUnitCost;
                    buildingAquisition.IrbPercentage = BuildingDto.IrbPercentage;
                    buildingAquisition.RoofType = BuildingDto.RoofType;
                    buildingAquisition.UniqueId = basicDetails.UniqueId;
                    buildingAquisition.IsActive = true;
                    buildingAquisition.IsDeleted = false;
                    buildingAquisition.ModifiedBy = _userInfo.Name;
                    buildingAquisition.ModifiedDate = DateTime.UtcNow;

                    await _buildingAcquisitionDetailsRepository.AddAsync(buildingAquisition, token).ConfigureAwait(false);
                }

                await _work.CommitAsync(token).ConfigureAwait(false);
            }
            return false;
        }

        public async Task<bool> DeleteBuilidngInspectionDetails(IdmDchgBuildingDetDTO BuildingDto, CancellationToken token)
        {
            //var basicDetails = _mapper.Map<TblIdmDchgBuildingDet>(BuildingDto);
            var basicDetails = await _buildingInspectionRepository.FirstOrDefaultByExpressionAsync(x => x.UniqueId == BuildingDto.UniqueId, token);
            basicDetails.IsActive = false;
            basicDetails.IsDeleted = true;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            await _buildingInspectionRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
            //await _work.CommitAsync(token).ConfigureAwait(false);
            var buildingAcquisition = await _buildingAcquisitionDetailsRepository.FirstOrDefaultByExpressionAsync(x => x.UniqueId == BuildingDto.UniqueId, token);
            buildingAcquisition.IsActive = false;
            buildingAcquisition.IsDeleted = true;
            buildingAcquisition.ModifiedDate = DateTime.UtcNow;
            buildingAcquisition.ModifiedBy = _userInfo.Name;
            var response = await _buildingAcquisitionDetailsRepository.UpdateAsync(buildingAcquisition, token).ConfigureAwait(false);

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


        #region Working Capital
        public async Task<bool> CreateWorkingCapitalInspectionDetails(IdmDchgWorkingCapitalDTO workingCapital, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmDchgWorkingCapital>(workingCapital);
            basicDetails.CreatedBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;


            var response = await _WokingCapitalRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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


        #region Building Material at Site Inspection
        //Author Manoj on 29/08/2022
        public async Task<IEnumerable<IdmBuildingMaterialSiteInspectionDTO>> GetAllBuildingMaterialInspectionListAsync(long accountNumber, long InspectionId, CancellationToken token)
        {
            var buildingMaterialInspectionList = await _buildingMaterialRepository.FindByExpressionAsync(x => x.LoanAcc == accountNumber /*&& x.IrbmIno == InspectionId && x.IsActive == true && x.IsDeleted == false*/, token);
            return _mapper.Map<List<IdmBuildingMaterialSiteInspectionDTO>>(buildingMaterialInspectionList);
        }
        //Author Manoj on 29/08/2022
        public async Task<bool> UpdateBuildMatSiteInspectionDetails(IdmBuildingMaterialSiteInspectionDTO bildMatInspectionDTO, CancellationToken token)
        {
            var currentDetails = await _buildingMaterialRepository.FirstOrDefaultByExpressionAsync(x => x.IrbmRowId == bildMatInspectionDTO.IrbmRowId, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = false;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _buildingMaterialRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<TblIdmIrBldgMat>(bildMatInspectionDTO);
                basicDetails.IrbmRowId = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.ModifiedBy = _userInfo.Name;
                basicDetails.ModifiedDate = DateTime.UtcNow;

                var result = await _buildingInspectionRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == bildMatInspectionDTO.LoanAcc);
                var building = result.LastOrDefault();
                building.DcBdgSecCreatd = (int)basicDetails.IrbmAmt;

                var response = await _buildingMaterialRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
                await _buildingInspectionRepository.UpdateAsync(building, token).ConfigureAwait(false);
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
        //Author Manoj on 29/08/2022
        public async Task<bool> CreateBuildMatSiteInspectionDetails(IdmBuildingMaterialSiteInspectionDTO bildMatInspectionDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmIrBldgMat>(bildMatInspectionDTO);
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreatedBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var result = await _buildingInspectionRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == bildMatInspectionDTO.LoanAcc);
            var building = result.LastOrDefault();
            building.DcBdgSecCreatd = (int)basicDetails.IrbmAmt;

            var response = await _buildingMaterialRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _buildingInspectionRepository.UpdateAsync(building, token).ConfigureAwait(false);
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

        //Author Manoj on 29/08/2022
        public async Task<bool> DeleteBuildMatSiteInspectionDetails(IdmBuildingMaterialSiteInspectionDTO bildMatInspectionDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmIrBldgMat>(bildMatInspectionDTO);
            basicDetails.IsActive = false;
            basicDetails.IsDeleted = true;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            var response = await _buildingMaterialRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
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



        #region Import Machinery Inspection
        //Author: Swetha  Date: 25/08/2022
        public async Task<IEnumerable<IdmDchgImportMachineryDTO>> GetAllImportMachineryList(long accountNumber, long InspectionId, CancellationToken token)
        {
            List<TblIdmDchgImportMachinery> importMachineryList;
            if (InspectionId != 0)
            {
                importMachineryList = await _importMachineryRepository.FindByExpressionAsync(x => x.LoanAcc == accountNumber /*&& x.DimcIno == InspectionId && x.IsActive == true && x.IsDeleted == false*/, token);

            }
            else
            {
                importMachineryList = await _importMachineryRepository.FindByExpressionAsync(x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false, token);

            }
            return _mapper.Map<List<IdmDchgImportMachineryDTO>>(importMachineryList);
        }
        public async Task<IEnumerable<TblCurrencyMastDto>> GetAllCurrencyList(CancellationToken token)
        {
            var currencyStatus = await _currencyStatus.FindByExpressionAsync(x => x.CurrencyId != null, token);
            return _mapper.Map<List<TblCurrencyMastDto>>(currencyStatus);
        }
        public async Task<bool> CreateImportMachineryDetails(IdmDchgImportMachineryDTO importMachinery, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmDchgImportMachinery>(importMachinery);
            basicDetails.CreatedBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            var importmachineryAcquisition = new TblIdmIrPlmc();

            importmachineryAcquisition.LoanAcc = basicDetails.LoanAcc;
            importmachineryAcquisition.LoanSub = basicDetails.LoanSub;
            importmachineryAcquisition.OffcCd = basicDetails.OffcCd;
            importmachineryAcquisition.IrPlmcItem = basicDetails.DimcItmNo;
            importmachineryAcquisition.IrPlmcIno = basicDetails.DimcIno;
            importmachineryAcquisition.IrPlmcItemDets = basicDetails.DimcDets;
            importmachineryAcquisition.IrPlmcSupplier = basicDetails.DimcSup;
            importmachineryAcquisition.IrPlmcAcqrdStatus = basicDetails.DimcAqrdStat;
            importmachineryAcquisition.IrPlmcFlg = 2;
            importmachineryAcquisition.IrPlmcAamt = importMachinery.IrPlmcAamt;
            importmachineryAcquisition.IrPlmcRelseStat = Convert.ToInt32(importMachinery.IrPlmcRelseStat);
            importmachineryAcquisition.IrPlmcTotalRelease = importMachinery.IrPlmcTotalRelease;
            importmachineryAcquisition.IrPlmcSecAmt = importMachinery.IrPlmcSecAmt;
            importmachineryAcquisition.UniqueId = basicDetails.UniqueId;
            importmachineryAcquisition.CreatedDate = DateTime.UtcNow;
            importmachineryAcquisition.CreatedBy = _userInfo.Name;
            importmachineryAcquisition.IsActive = true;
            importmachineryAcquisition.IsDeleted = false;
            var primaryColleteral = new TblIdmDeedDet();
            primaryColleteral.LoanAcc = basicDetails.LoanAcc;
            primaryColleteral.LoanSub = basicDetails.LoanSub;
            primaryColleteral.OffcCd = basicDetails.OffcCd;
            primaryColleteral.IsActive = true;
            primaryColleteral.IsDeleted = false;
            primaryColleteral.CreatedBy = _userInfo.Name;
            primaryColleteral.CreatedDate = DateTime.UtcNow;

            var response = await _importMachineryRepository.AddAsync(basicDetails, token).ConfigureAwait(false); //uc5 
            await _idmIrPlmc.AddAsync(importmachineryAcquisition, token).ConfigureAwait(false);
            await _primaryColleteralRepository.AddAsync(primaryColleteral, token).ConfigureAwait(false);
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
        public async Task<bool> UpdateImportMachineryDetails(IdmDchgImportMachineryDTO importMachinery, CancellationToken token)
        {
            var currentDetails = await _importMachineryRepository.FirstOrDefaultByExpressionAsync(x => x.DimcRowId == importMachinery.DimcRowId, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = false;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _importMachineryRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<TblIdmDchgImportMachinery>(importMachinery);
                basicDetails.DimcRowId = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.ModifiedBy = _userInfo.Name;
                basicDetails.ModifiedDate = DateTime.UtcNow;

                var response = await _importMachineryRepository.AddAsync(basicDetails, token).ConfigureAwait(false);

                var importMachinarys = await _idmIrPlmc.FirstOrDefaultByExpressionAsync(x => x.IrPlmcId == importMachinery.IrPlmcId, token);

                if (importMachinarys != null)
                {
                    importMachinarys.IsDeleted = false;
                    importMachinarys.IsActive = false;
                    importMachinarys.ModifiedBy = _userInfo.Name;
                    importMachinarys.ModifiedDate = DateTime.UtcNow;
                    await _idmIrPlmc.UpdateAsync(importMachinarys, token).ConfigureAwait(false);

                    var importMachinary = new TblIdmIrPlmc();
                    importMachinary.LoanAcc = basicDetails.LoanAcc;
                    importMachinary.LoanSub = basicDetails.LoanSub;
                    importMachinary.OffcCd = basicDetails.OffcCd;
                    importMachinary.IrPlmcIno = basicDetails.DimcIno;
                    importMachinary.IrPlmcItem = basicDetails.DimcItmNo;
                    importMachinary.IrPlmcItemDets = basicDetails.DimcDets;
                    importMachinary.IrPlmcAcqrdStatus = basicDetails.DimcAqrdStat;
                    importMachinary.IrPlmcSupplier = basicDetails.DimcSup;
                    importMachinary.IrPlmcFlg = 2;
                    //newly added
                    importMachinary.IrPlmcAamt = importMachinery.IrPlmcAamt;
                    importMachinary.IrPlmcRelseStat = Convert.ToInt32(importMachinery.IrPlmcRelseStat);
                    importMachinary.IrPlmcTotalRelease = importMachinery.IrPlmcTotalRelease;
                    importMachinary.IrPlmcSecAmt = importMachinery.IrPlmcSecAmt;
                    importMachinary.UniqueId = basicDetails.UniqueId;
                    importMachinary.CreatedDate = DateTime.UtcNow;
                    importMachinary.CreatedBy = _userInfo.Name;
                    importMachinary.IsActive = true;
                    importMachinary.IsDeleted = false;

                    await _idmIrPlmc.AddAsync(importMachinary, token).ConfigureAwait(false);



                }
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

        public async Task<bool> DeleteImportMachineryDetails(IdmDchgImportMachineryDTO importMachinery, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmDchgImportMachinery>(importMachinery);
            basicDetails.IsActive = false;
            basicDetails.IsDeleted = true;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            var response = await _importMachineryRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
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

        #region Furniture Inspection 

        public async Task<IEnumerable<IdmDChgFurnDTO>> GetAllFurnitureInspectionList(long accountNumber, long InspectionId, CancellationToken token)
        {
            List<TblIdmDChgFurn> furnList;
            if (InspectionId != 0)
            {
                furnList = await _furnitureInspectionDetailRepository.FindByExpressionAsync(x => x.LoanAcc == accountNumber/* && x.FurnIno == InspectionId && x.IsActive == true && x.IsDeleted == false*/, token);

            }
            else
            {
                furnList = await _furnitureInspectionDetailRepository.FindByExpressionAsync(x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false, token);

            }
            return _mapper.Map<List<IdmDChgFurnDTO>>(furnList);
        }

        public async Task<bool> CreateFurnitureInspectionDetails(IdmDChgFurnDTO furnitureInspectionDTO, CancellationToken token)
        {

            var basicDetails = _mapper.Map<TblIdmDChgFurn>(furnitureInspectionDTO);
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreateBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var furnitureAcquisition = new TblIdmIrFurn();
            furnitureAcquisition.LoanAcc = basicDetails.LoanAcc;
            furnitureAcquisition.LoanSub = basicDetails.LoanSub;
            furnitureAcquisition.OffcCd = basicDetails.OffcCd;
            furnitureAcquisition.IrfItem = basicDetails.FurnItemNo;
            furnitureAcquisition.IrfIno = basicDetails.FurnIno;
            furnitureAcquisition.IrfSupplier = basicDetails.FurnSupp;
            furnitureAcquisition.IrfItemDets = basicDetails.FurnDetails;
            furnitureAcquisition.IrfAamt = basicDetails.FurnActualCost;
            furnitureAcquisition.IrfAmt = basicDetails.FurnTotalCost;
            furnitureAcquisition.IrfAqrdStat = basicDetails.FurnAqrdStat;

            //newly added
            furnitureAcquisition.IrfSecAmt = furnitureInspectionDTO.IrfSecAmt;
            furnitureAcquisition.IrfTotalRelease = furnitureInspectionDTO.IrfTotalRelease;
            furnitureAcquisition.IrfRelStat = Convert.ToInt32(furnitureInspectionDTO.IrfRelStat);
            furnitureAcquisition.UniqueId = basicDetails.UniqueID;
            furnitureAcquisition.IsDeleted = false;
            furnitureAcquisition.IsActive = true;
            furnitureAcquisition.CreateBy = _userInfo.Name;
            furnitureAcquisition.CreatedDate = DateTime.UtcNow;

            var primaryColleteral = new TblIdmDeedDet();
            primaryColleteral.LoanAcc = basicDetails.LoanAcc;
            primaryColleteral.LoanSub = basicDetails.LoanSub;
            primaryColleteral.OffcCd = basicDetails.OffcCd;
            primaryColleteral.IsActive = true;
            primaryColleteral.IsDeleted = false;
            primaryColleteral.CreatedBy = _userInfo.Name;
            primaryColleteral.CreatedDate = DateTime.UtcNow;


            var response = await _furnitureInspectionDetailRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _primaryColleteralRepository.AddAsync(primaryColleteral, token).ConfigureAwait(false);
            await _furnitureAcuisitionRepository.AddAsync(furnitureAcquisition, token).ConfigureAwait(false);

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

        public async Task<bool> UpdateFurnitureInspectionDetails(IdmDChgFurnDTO furnitureInspectionDTO, CancellationToken token)
        {
            var currentDetails = await _furnitureInspectionDetailRepository.FirstOrDefaultByExpressionAsync(x => x.Id == furnitureInspectionDTO.Id, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = false;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _furnitureInspectionDetailRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<TblIdmDChgFurn>(furnitureInspectionDTO);
                basicDetails.Id = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.ModifiedBy = _userInfo.Name;
                basicDetails.ModifiedDate = DateTime.UtcNow;

                await _furnitureInspectionDetailRepository.AddAsync(basicDetails, token).ConfigureAwait(false);

                var currentDetails1 = await _idmIrFurn.FirstOrDefaultByExpressionAsync(x => x.IrfId == furnitureInspectionDTO.IrfId, token);

                if (currentDetails1 != null)
                {
                    currentDetails1.IsDeleted = false;
                    currentDetails1.IsActive = false;
                    currentDetails1.ModifiedBy = _userInfo.Name;
                    currentDetails1.ModifiedDate = DateTime.UtcNow;
                    await _idmIrFurn.UpdateAsync(currentDetails1, token).ConfigureAwait(false);


                    var furnitureAcquisition = new TblIdmIrFurn();
                    furnitureAcquisition.LoanAcc = basicDetails.LoanAcc;
                    furnitureAcquisition.LoanSub = basicDetails.LoanSub;
                    furnitureAcquisition.OffcCd = basicDetails.OffcCd;
                    furnitureAcquisition.IrfItem = basicDetails.FurnItemNo;
                    furnitureAcquisition.IrfIno = basicDetails.FurnIno;
                    furnitureAcquisition.IrfSupplier = basicDetails.FurnSupp;
                    furnitureAcquisition.IrfItemDets = basicDetails.FurnDetails;
                    furnitureAcquisition.IrfAamt = basicDetails.FurnActualCost;
                    furnitureAcquisition.IrfAmt = basicDetails.FurnTotalCost;
                    furnitureAcquisition.IrfAqrdStat = basicDetails.FurnAqrdStat;
                    furnitureAcquisition.IrfSecAmt = furnitureInspectionDTO.IrfSecAmt;
                    furnitureAcquisition.IrfTotalRelease = furnitureInspectionDTO.IrfTotalRelease;
                    furnitureAcquisition.IrfRelStat = Convert.ToInt32(furnitureInspectionDTO.IrfRelStat);
                    furnitureAcquisition.UniqueId = basicDetails.UniqueID;
                    furnitureAcquisition.IsActive = true;
                    furnitureAcquisition.IsDeleted = false;
                    furnitureAcquisition.ModifiedBy = _userInfo.Name;
                    furnitureAcquisition.ModifiedDate = DateTime.UtcNow;

                    await _idmIrFurn.AddAsync(furnitureAcquisition, token).ConfigureAwait(false);
                }

                await _work.CommitAsync(token).ConfigureAwait(false);


            }
            return false;
        }

        public async Task<bool> DeleteFurnitureInspectionDetails(IdmDChgFurnDTO furnitureInspectionDTO, CancellationToken token)
        {

            var basicDetails = _mapper.Map<TblIdmDChgFurn>(furnitureInspectionDTO);
            basicDetails.IsActive = false;
            basicDetails.IsDeleted = true;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            await _furnitureInspectionDetailRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);

            var furnitureAcquisition = await _idmIrFurn.FirstOrDefaultByExpressionAsync(x => x.UniqueId == furnitureInspectionDTO.UniqueID, token);
            furnitureAcquisition.IsActive = false;
            furnitureAcquisition.IsDeleted = true;
            furnitureAcquisition.ModifiedDate = DateTime.UtcNow;
            furnitureAcquisition.ModifiedBy = _userInfo.Name;
            var response = await _idmIrFurn.UpdateAsync(furnitureAcquisition, token).ConfigureAwait(false);
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

        #region Indigenous Machinary Inspection
        //Author Manoj on 26/08/2022
        public async Task<IEnumerable<IdmDchgIndigenousInspectionDTO>> GetAllIndigenousMachineryInspectionListAsync(long accountNumber, int InspectionId, CancellationToken token)
        {
            List<TblIdmDchgIndigenousMachineryDet> indigenousMachineryInspectionList;
            if (InspectionId != 0)
            {
                indigenousMachineryInspectionList = await _indigenousMachineryRepository.FindByExpressionAsync(x => x.LoanAcc == accountNumber /*&& x.Ino == InspectionId && x.IsActive == true && x.IsDeleted == false*/, token);

            }
            else
            {
                indigenousMachineryInspectionList = await _indigenousMachineryRepository.FindByExpressionAsync(x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false, token);

            }
            return _mapper.Map<List<IdmDchgIndigenousInspectionDTO>>(indigenousMachineryInspectionList);
        }
        public async Task<IEnumerable<TblMachineryStatusDto>> GetAllMachineryStatusList(CancellationToken token)
        {
            var machinaryStatus = await _machinaryStatus.FindByExpressionAsync(x => x.MacStatus != null, token);
            return _mapper.Map<List<TblMachineryStatusDto>>(machinaryStatus);
        }

        public async Task<IEnumerable<TblProcureMastDto>> GetAllProcureList(CancellationToken token)
        {
            var procureStatus = await _procureStatus.FindByExpressionAsync(x => x.ProcureId != null, token);
            return _mapper.Map<List<TblProcureMastDto>>(procureStatus);
        }



        public async Task<bool> DeleteIndigenousMachineryInspectionDetails(IdmDchgIndigenousInspectionDTO bildMatInspectionDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmDchgIndigenousMachineryDet>(bildMatInspectionDTO);
            basicDetails.IsActive = false;
            basicDetails.IsDeleted = true;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            await _indigenousMachineryRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);


            var machineryAcquisition = await _idmIrPlmc.FirstOrDefaultByExpressionAsync(x => x.UniqueId == bildMatInspectionDTO.UniqueId, token);
            machineryAcquisition.IsActive = false;
            machineryAcquisition.IsDeleted = true;
            machineryAcquisition.ModifiedDate = DateTime.UtcNow;
            machineryAcquisition.ModifiedBy = _userInfo.Name;
            var response = await _idmIrPlmc.UpdateAsync(machineryAcquisition, token).ConfigureAwait(false);
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
        public async Task<bool> UpdateIndigenousMachineryInspectionDetails(IdmDchgIndigenousInspectionDTO bildMatInspectionDTO, CancellationToken token)
        {
            var currentDetails = await _indigenousMachineryRepository.FirstOrDefaultByExpressionAsync(x => x.Id == bildMatInspectionDTO.Id, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = false;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _indigenousMachineryRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);


                var basicDetails = _mapper.Map<TblIdmDchgIndigenousMachineryDet>(bildMatInspectionDTO);
                basicDetails.Id = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.ModifiedBy = _userInfo.Name;
                basicDetails.ModifiedDate = DateTime.UtcNow;

                await _indigenousMachineryRepository.AddAsync(basicDetails, token).ConfigureAwait(false);

                var machinaryAcquisitions = await _idmIrPlmc.FirstOrDefaultByExpressionAsync(x => x.IrPlmcId == bildMatInspectionDTO.IrPlmcId, token);
                if (machinaryAcquisitions != null)
                {
                    machinaryAcquisitions.IsDeleted = false;
                    machinaryAcquisitions.IsActive = false;
                    machinaryAcquisitions.ModifiedBy = _userInfo.Name;
                    machinaryAcquisitions.ModifiedDate = DateTime.UtcNow;
                    await _idmIrPlmc.UpdateAsync(machinaryAcquisitions, token).ConfigureAwait(false);

                    var machinaryAcquisition = new TblIdmIrPlmc();
                    machinaryAcquisition.LoanAcc = basicDetails.LoanAcc;
                    machinaryAcquisition.LoanSub = basicDetails.LoanSub;
                    machinaryAcquisition.OffcCd = basicDetails.OffcCd;
                    machinaryAcquisition.IrPlmcIno = basicDetails.Ino;
                    machinaryAcquisition.IrPlmcItem = basicDetails.ItemNo;
                    machinaryAcquisition.IrPlmcItemDets = basicDetails.ItemDetails;
                    machinaryAcquisition.IrPlmcAcqrdStatus = basicDetails.AquiredStatus;
                    machinaryAcquisition.IrPlmcSupplier = basicDetails.SupplierName;
                    machinaryAcquisition.IrPlmcAmt = basicDetails.TotalCost;
                    machinaryAcquisition.IrPlmcFlg = 1;
                    //newly added
                    machinaryAcquisition.IrPlmcAamt = bildMatInspectionDTO.IrPlmcAamt;
                    machinaryAcquisition.IrPlmcRelseStat = Convert.ToInt32(bildMatInspectionDTO.IrPlmcRelseStat);
                    machinaryAcquisition.IrPlmcTotalRelease = bildMatInspectionDTO.IrPlmcTotalRelease;
                    machinaryAcquisition.IrPlmcSecAmt = bildMatInspectionDTO.IrPlmcSecAmt;
                    machinaryAcquisition.UniqueId = basicDetails.UniqueId;
                    machinaryAcquisition.CreatedDate = DateTime.UtcNow;
                    machinaryAcquisition.CreatedBy = _userInfo.Name;
                    machinaryAcquisition.IsActive = true;
                    machinaryAcquisition.IsDeleted = false;

                    await _idmIrPlmc.AddAsync(machinaryAcquisition, token).ConfigureAwait(false);

                }

                await _work.CommitAsync(token).ConfigureAwait(false);


            }
            return false;
        }

        public async Task<bool> CreateIndigenousMachineryInspectionDetails(IdmDchgIndigenousInspectionDTO bildMatInspectionDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmDchgIndigenousMachineryDet>(bildMatInspectionDTO);
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreatedBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var machineryAcquisition = new TblIdmIrPlmc
            {
                LoanAcc = basicDetails.LoanAcc,
                LoanSub = basicDetails.LoanSub,
                OffcCd = basicDetails.OffcCd,
                IrPlmcIno = basicDetails.Ino,
                IrPlmcItem = basicDetails.ItemNo,
                IrPlmcItemDets = basicDetails.ItemDetails,
                IrPlmcAcqrdStatus = basicDetails.AquiredStatus,
                IrPlmcSupplier = basicDetails.SupplierName,
                IrPlmcAmt = basicDetails.TotalCost,
                //newly added
                IrPlmcAamt = bildMatInspectionDTO.IrPlmcAamt,
                IrPlmcRelseStat = Convert.ToInt32(bildMatInspectionDTO.IrPlmcRelseStat),
                IrPlmcTotalRelease = bildMatInspectionDTO.IrPlmcTotalRelease,
                IrPlmcSecAmt = bildMatInspectionDTO.IrPlmcSecAmt,
                IrPlmcFlg = 1,
                UniqueId = basicDetails.UniqueId,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = _userInfo.Name,
                IsActive = true,
                IsDeleted = false
            };
            var primaryColleteral = new TblIdmDeedDet();
            primaryColleteral.LoanAcc = basicDetails.LoanAcc;
            primaryColleteral.LoanSub = basicDetails.LoanSub;
            primaryColleteral.OffcCd = basicDetails.OffcCd;
            primaryColleteral.IsActive = true;
            primaryColleteral.IsDeleted = false;
            primaryColleteral.CreatedBy = _userInfo.Name;
            primaryColleteral.CreatedDate = DateTime.UtcNow;

            var response = await _indigenousMachineryRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _primaryColleteralRepository.AddAsync(primaryColleteral, token).ConfigureAwait(false);
            await _idmIrPlmc.AddAsync(machineryAcquisition, token).ConfigureAwait(false);
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

        #region ProjectCostDetail
        public async Task<IEnumerable<IdmDchgProjectCostDTO>> GetAllProjectCostDetailsList(long accountNumber, CancellationToken token)
        {
            var ProjectCostList = await _projectCostRepository.FindByExpressionAsync(x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false, token);
            return _mapper.Map<List<IdmDchgProjectCostDTO>>(ProjectCostList);
        }
        public async Task<bool> UpdateProjectCostDetails(IdmDchgProjectCostDTO PrjCostDTO, CancellationToken token)
        {
            var currentDetails = await _projectCostRepository.FirstOrDefaultByExpressionAsync(x => x.Id == PrjCostDTO.Id, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _projectCostRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);


                var basicDetails = _mapper.Map<TblIdmDchgProjectCost>(PrjCostDTO);
                basicDetails.Id = 0;
                basicDetails.DcpcAmount = PrjCostDTO.DcpcAmount / 100000;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.ModifiedBy = _userInfo.Name;
                basicDetails.ModifiedDate = DateTime.UtcNow;

                var response = await _projectCostRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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
        //Author Akhila on 06/09/2022
        public async Task<bool> CreateProjectCostDetails(IdmDchgProjectCostDTO PrjCostDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmDchgProjectCost>(PrjCostDTO);
            basicDetails.DcpcAmount = PrjCostDTO.DcpcAmount / 100000;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreatedBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var response = await _projectCostRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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
        public async Task<bool> DeleteProjectCostDetails(IdmDchgProjectCostDTO PrjCostDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmDchgProjectCost>(PrjCostDTO);
            basicDetails.IsActive = false;
            basicDetails.IsDeleted = true;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            var response = await _projectCostRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
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


        #region Letter Of Credit
        public async Task<IEnumerable<IdmDsbLetterOfCreditDTO>> GetAllLetterOfCreditDetailListAsync(long accountNumber, CancellationToken token)
        {
            var letterOfCreditInspectionList = await _letterOfCreditRepository.FindByExpressionAsync(x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false, token);
            return _mapper.Map<List<IdmDsbLetterOfCreditDTO>>(letterOfCreditInspectionList);
        }

        public async Task<bool> DeleteLetterOfCreditDetails(IdmDsbLetterOfCreditDTO letterOfCreditDetailsDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmDsbLetterOfCredit>(letterOfCreditDetailsDTO);
            basicDetails.IsActive = false;
            basicDetails.IsDeleted = true;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            var response = await _letterOfCreditRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
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

        public async Task<bool> UpdateLetterOfCreditDetails(IdmDsbLetterOfCreditDTO letterOfCreditDetailsDTO, CancellationToken token)
        {
            var currentDetails = await _letterOfCreditRepository.FirstOrDefaultByExpressionAsync(x => x.DcLocRowId == letterOfCreditDetailsDTO.DcLocRowId, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _letterOfCreditRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<TblIdmDsbLetterOfCredit>(letterOfCreditDetailsDTO);
                basicDetails.DcLocRowId = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.ModifiedBy = _userInfo.Name;
                basicDetails.ModifiedDate = DateTime.UtcNow;

                var response = await _letterOfCreditRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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

        public async Task<bool> CreateLetterOfCreditDetails(IdmDsbLetterOfCreditDTO letterOfCreditDetailsDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmDsbLetterOfCredit>(letterOfCreditDetailsDTO);
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreatedBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;
            var response = await _letterOfCreditRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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


        #region Means Of Finance 
        //Author: Swetha  Date: 25/08/2022
        public async Task<IEnumerable<IdmDchgMeansOfFinanceDTO>> GetAllMeansOfFinanceList(long accountNumber, CancellationToken token)
        {
            var importMachineryList = await _meansOfFinanceRepository.FindByExpressionAsync(x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false, token);
            return _mapper.Map<List<IdmDchgMeansOfFinanceDTO>>(importMachineryList);
        }

        public async Task<bool> CreateMeansOfFinanceDetails(IdmDchgMeansOfFinanceDTO meansOfFinance, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmDchgMeansOfFinance>(meansOfFinance);
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreatedBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var response = await _meansOfFinanceRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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
        public async Task<bool> UpdateMeansOfFinanceDetails(IdmDchgMeansOfFinanceDTO meansOfFinance, CancellationToken token)
        {
            var currentDetails = await _meansOfFinanceRepository.FirstOrDefaultByExpressionAsync(x => x.DcmfRowId == meansOfFinance.DcmfRowId, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _meansOfFinanceRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<TblIdmDchgMeansOfFinance>(meansOfFinance);
                basicDetails.DcmfRowId = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.ModifiedBy = _userInfo.Name;
                basicDetails.ModifiedDate = DateTime.UtcNow;

                var response = await _meansOfFinanceRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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

        public async Task<bool> DeleteMeansOfFinanceDetails(IdmDchgMeansOfFinanceDTO meansOfFinance, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmDchgMeansOfFinance>(meansOfFinance);
            basicDetails.IsActive = false;
            basicDetails.IsDeleted = true;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            var response = await _meansOfFinanceRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
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

        #region status of Implementation 
        public async Task<IEnumerable<IdmDsbStatImpDTO>> GetAllStatusofImplementation(long accountNumber, long InspectionId, CancellationToken token)
        {
            List<TblDsbStatImp> dsbStatImps;
            if (InspectionId != 0)
            {
                dsbStatImps = await _dsbStatImp.FindByExpressionAsync(x => x.LoanAcc == accountNumber/* && x.DsbIno == InspectionId && x.IsActive == true && x.IsDeleted == false*/, token);

            }
            else
            {
                dsbStatImps = await _dsbStatImp.FindByExpressionAsync(x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false, token);

                
            }
            return _mapper.Map<List<IdmDsbStatImpDTO>>(dsbStatImps);
        }

        public async Task<bool> CreateStatusofImplementation(IdmDsbStatImpDTO statusImp, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblDsbStatImp>(statusImp);
            basicDetails.CreatedBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;

            var response = await _dsbStatImp.AddAsync(basicDetails, token).ConfigureAwait(false); //uc5 
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


        public async Task<bool> UpdateStatusofImplementation(IdmDsbStatImpDTO statusImp, CancellationToken token)
        {
            var currentDetails = await _dsbStatImp.FirstOrDefaultByExpressionAsync(x => x.DsbId == statusImp.DsbId, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = false;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _dsbStatImp.UpdateAsync(currentDetails, token).ConfigureAwait(false);

                var basicDetails = _mapper.Map<TblDsbStatImp>(statusImp);
                basicDetails.DsbId = 0;
                basicDetails.IsActive = true;
                basicDetails.IsDeleted = false;
                basicDetails.ModifiedBy = _userInfo.Name;
                basicDetails.ModifiedDate = DateTime.UtcNow;

                var response = await _dsbStatImp.AddAsync(basicDetails, token).ConfigureAwait(false);
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

        public async Task<bool> DeleteStatusofImplementation(IdmDsbStatImpDTO statusImp, CancellationToken token)
        {
            //var basicDetails = _mapper.Map<TblDsbStatImp>(statusImp);
            var basicDetails = await _dsbStatImp.FirstOrDefaultByExpressionAsync(x => x.UniqueId == statusImp.UniqueId, token);
            basicDetails.IsActive = false;
            basicDetails.IsDeleted = true;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            var response = await _dsbStatImp.UpdateAsync(basicDetails, token).ConfigureAwait(false);
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
