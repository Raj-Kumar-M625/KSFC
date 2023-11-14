using AutoMapper;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.CreationOfDisbursmentProposalModule;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfDisbursmentProposal;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.IDMModule.CreationOfDisbursmentProposal
{
    public class CreationOfDisbursmentProposal : ICreationOfDisbursmentProposalService
    {
        private readonly IEntityRepository<IdmDsbdets> _RecomDisbursmentDetailsRepository;
        private readonly IEntityRepository<TblAllcCdTab> _AllocationDetailRepository;
        private readonly IEntityRepository<TblIdmReleDetls> _RecomDisbursmentReleaseDetailRepository;
        private readonly IEntityRepository<TblIdmBenfDet> _beneficiaryDetailsRepository;
        private readonly IEntityRepository<TblIdmDisbProp> _dispursmentRepository;

        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;
        private readonly UserInfo _userInfo;

        public CreationOfDisbursmentProposal(IEntityRepository<IdmDsbdets> RecomDisbursmentRepository,IEntityRepository<TblAllcCdTab> allocationDetailRepository, IEntityRepository<TblIdmDisbProp> dispursmentRepository
            , IEntityRepository<TblIdmReleDetls> recomDisbursmentReleaseDetailRepository, IUnitOfWork work,IMapper mapper, UserInfo userInfo, IEntityRepository<TblIdmBenfDet> beneficiaryDetailsRepository)
        {
            _RecomDisbursmentDetailsRepository = RecomDisbursmentRepository;
            _AllocationDetailRepository = allocationDetailRepository;
            _RecomDisbursmentReleaseDetailRepository = recomDisbursmentReleaseDetailRepository;
            _beneficiaryDetailsRepository = beneficiaryDetailsRepository;
            _dispursmentRepository = dispursmentRepository;
            _work = work;
            _mapper = mapper;
            _userInfo = userInfo;
        }

        #region Recommended Disbursement Details

        public async Task<IEnumerable<IdmDsbdetsDTO>> GetAllRecomDisbursementDetails(long accountNumber, CancellationToken token)
        {
            var data = await _RecomDisbursmentDetailsRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<IdmDsbdetsDTO>>(data);
        }
        public async Task<IEnumerable<TblAllcCdTabDTO>> GetAllocationCodeDetails(CancellationToken token)
        {
            var data = await _AllocationDetailRepository.FindByMatchingPropertiesAsync(token, x => x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<TblAllcCdTabDTO>>(data);
        }

        public async Task<IEnumerable<TblIdmReleDetlsDTO>> GetRecomDisbursementReleaseDetails(CancellationToken token)
        {
            var data = await _RecomDisbursmentReleaseDetailRepository.FindByMatchingPropertiesAsync(token, x => x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<TblIdmReleDetlsDTO>>(data);
        }

        public async Task<IdmDsbdetsDTO> UpdateRecomDisbursementDetail(IdmDsbdetsDTO idmDsbdetsDTO, CancellationToken token)
        {
            var currentDetails = await _RecomDisbursmentDetailsRepository.FirstOrDefaultByExpressionAsync(x => x.DsbdetsID == idmDsbdetsDTO.DsbdetsID, token);
            currentDetails.IsDeleted = true;
            currentDetails.IsActive = false;

            await _RecomDisbursmentDetailsRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

            var basicDetails = _mapper.Map<IdmDsbdets>(idmDsbdetsDTO);
            basicDetails.DsbdetsID = 0;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;

            var response = await _RecomDisbursmentDetailsRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<IdmDsbdetsDTO>(response);
        }

        #endregion


        #region disbursement proposal details
        /// <summary>
        ///  Author: Gowtham; Module: Disburesment Proposal Details; Date:07/10/2022
        /// </summary>
        public async Task<IEnumerable<TblIdmReleDetlsDTO>> GetAllProposalDetails(long accountNumber, CancellationToken token)
        {
            var data = await _RecomDisbursmentReleaseDetailRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false,
               DisbProp => DisbProp.TblIdmDisbProp
                ).ConfigureAwait(false);
            return _mapper.Map<List<TblIdmReleDetlsDTO>>(data);
        }

        public async Task<TblIdmReleDetlsDTO> CreateProposalDetail(TblIdmReleDetlsDTO tblIdmReleDetlsDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmReleDetls>(tblIdmReleDetlsDTO);
        
            TblIdmDisbProp disbprop = basicDetails.TblIdmDisbProp;
            disbprop.IsActive = true;
            disbprop.IsDeleted = false;
            disbprop.LoanAcc = basicDetails.LoanAcc;
            disbprop.LoanSub = basicDetails.LoanSub;
            disbprop.OffcCd = basicDetails.OffcCd;
            disbprop.CreatedBy = _userInfo.Name;
            disbprop.CreatedDate = DateTime.UtcNow;
             await _dispursmentRepository.AddAsync(disbprop,token).ConfigureAwait(false);

            TblIdmBenfDet benifdetails = new TblIdmBenfDet();

            benifdetails.BenfId = 0;
            benifdetails.IsActive = true;
            benifdetails.IsDeleted = false;
            benifdetails.LoanAcc = basicDetails.LoanAcc;
            benifdetails.LoanSub = basicDetails.LoanSub;
            benifdetails.OffcCd = basicDetails.OffcCd;
            benifdetails.BenfNumber = (int?)basicDetails.TblIdmDisbProp.PropNumber;
            benifdetails.BenfAmt = tblIdmReleDetlsDTO.BenfAmt;
            benifdetails.BenfDept = tblIdmReleDetlsDTO.DeptCode;
            benifdetails.CreatedBy = _userInfo.Name;
            benifdetails.CreatedDate = DateTime.UtcNow;
             await _beneficiaryDetailsRepository.AddAsync(benifdetails, token).ConfigureAwait(false);

            basicDetails.ReleId = 0;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreatedBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;
            basicDetails.PropNumber = basicDetails.TblIdmDisbProp.PropNumber;
            var response =   await _RecomDisbursmentReleaseDetailRepository.AddAsync(basicDetails,token).ConfigureAwait(false);

           
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<TblIdmReleDetlsDTO>(response);
        }

        public async Task<TblIdmReleDetlsDTO> UpdateProposalDetail(TblIdmReleDetlsDTO tblIdmReleDetlsDTO, CancellationToken token)
        {
            var currentDetails = await _RecomDisbursmentReleaseDetailRepository.FirstOrDefaultByExpressionAsync(x => x.ReleId == tblIdmReleDetlsDTO.ReleId, token);
            currentDetails.IsDeleted = true;
            currentDetails.IsActive = false;
            currentDetails.ModifiedBy = _userInfo.Name;
            currentDetails.ModifiedDate = DateTime.UtcNow;
            await _RecomDisbursmentReleaseDetailRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);


            var basicDetails = _mapper.Map<TblIdmReleDetls>(tblIdmReleDetlsDTO);
            basicDetails.ReleId = 0;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.ModifiedDate = DateTime.UtcNow;

            var response = await _RecomDisbursmentReleaseDetailRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<TblIdmReleDetlsDTO>(response);
        }

        public async Task<TblIdmReleDetlsDTO> DeleteProposalDetail(TblIdmReleDetlsDTO tblIdmReleDetlsDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmReleDetls>(tblIdmReleDetlsDTO);
            basicDetails.IsActive = false;
            basicDetails.IsDeleted = true;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            var result = await _RecomDisbursmentReleaseDetailRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<TblIdmReleDetlsDTO>(result);
        }
        #endregion

        #region Beneficiary Details
        /// <summary>
        ///  Author: Dev; Module: Beneficiary Details; Date:07/10/2022
        /// </summary>
        public async Task<IEnumerable<TblIdmBenfDetDTO>> GetAllBeneficiaryDetails(long accountNumber, CancellationToken token)
        {
            var benfdet = await _beneficiaryDetailsRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<TblIdmBenfDetDTO>>(benfdet);
        }

        public async Task<TblIdmBenfDetDTO> UpdateBeneficiaryDetails(TblIdmBenfDetDTO beneficiaryDet, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmBenfDet>(beneficiaryDet);
            var currentDetails = await _beneficiaryDetailsRepository.FirstOrDefaultByExpressionAsync(x => x.IsActive == true && x.IsDeleted == false && x.BenfNumber == beneficiaryDet.BenfNumber, token);
            if (currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                await _beneficiaryDetailsRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);
            }

            basicDetails.BenfId = 0;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;

            var response = await _beneficiaryDetailsRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<TblIdmBenfDetDTO>(response);
        }
        #endregion
    }
}
