using AutoMapper;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.LoanAllocationModule;
using KAR.KSFC.Components.Common.Dto.IDM;
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

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.IDMModule.LoanAllocationModule
{
    /// <summary>
    ///  Author: Gagana K; Module:Loan Alloaction; Date:28/09/2022
    /// </summary>
    public class LoanAllocationService : ILoanAllocationService
    {
        private readonly IEntityRepository<TblIdmDhcgAllc> _loanAllocation;
        private readonly IEntityRepository<IdmDsbdets> _idmDsbDets;
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;
        private readonly UserInfo _userInfo;

        public LoanAllocationService(IEntityRepository<TblIdmDhcgAllc> loanAllocation, IEntityRepository<IdmDsbdets> idmDsbDets, IUnitOfWork work,
                           IMapper mapper, UserInfo userInfo)
        {
            _loanAllocation = loanAllocation;
            _idmDsbDets= idmDsbDets;
            _work = work;
            _mapper = mapper;
            _userInfo = userInfo;
        }
        public async Task<IEnumerable<TblIdmDhcgAllcDTO>> GetAllLoanAllocationList(long accountNumber, CancellationToken token)
        {
            var data = await _loanAllocation.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<TblIdmDhcgAllcDTO>>(data);
        }
        public async Task<bool> UpdateLoanAllocationDetails(TblIdmDhcgAllcDTO AllocationDTO, CancellationToken token)
        {
            var currentDetails = await _loanAllocation.FirstOrDefaultByExpressionAsync(x => x.DcalcId == AllocationDTO.DcalcId, token);
            currentDetails.IsDeleted = true;
            currentDetails.IsActive = false;
            currentDetails.ModifiedBy = _userInfo.Name;
            currentDetails.ModifiedDate = DateTime.UtcNow;
            await _loanAllocation.UpdateAsync(currentDetails, token).ConfigureAwait(false);


            var basicDetails = _mapper.Map<TblIdmDhcgAllc>(AllocationDTO);
            basicDetails.DcalcId = 0;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.ModifiedDate = DateTime.UtcNow;

            var response = await _loanAllocation.AddAsync(basicDetails, token).ConfigureAwait(false);
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
        public async Task<bool> CreateLoanAllocationDetails(TblIdmDhcgAllcDTO AllocationDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmDhcgAllc>(AllocationDTO);
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
            basicDetails.CreateBy = _userInfo.Name;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var recomanded = new IdmDsbdets();
            recomanded.UniqueId = basicDetails.UniqueId;
            recomanded.LoanAcc = basicDetails.LoanAcc;
            recomanded.OffcCd = basicDetails.OffcCd;
            recomanded.LoanSub= basicDetails.LoanSub;
            recomanded.DsbAcd = basicDetails.DcalcCd;
            recomanded.AlocAmt = basicDetails.DcalcAmt;
            recomanded.IsActive = true;
            recomanded.IsDeleted = false;
            recomanded.CreatedBy = _userInfo.Name;
            recomanded.CreatedDate = DateTime.UtcNow;

            var response = await _loanAllocation.AddAsync(basicDetails, token).ConfigureAwait(false);
            await _idmDsbDets.AddAsync(recomanded ,token).ConfigureAwait(false);
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
        public async Task<bool> DeleteLoanAllocationDetails(TblIdmDhcgAllcDTO AllocationDTO, CancellationToken token)
        {
            var basicDetails = _mapper.Map<TblIdmDhcgAllc>(AllocationDTO);
            basicDetails.IsActive = false;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.IsDeleted = true;
            var response = await _loanAllocation.UpdateAsync(basicDetails, token).ConfigureAwait(false);
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
}
