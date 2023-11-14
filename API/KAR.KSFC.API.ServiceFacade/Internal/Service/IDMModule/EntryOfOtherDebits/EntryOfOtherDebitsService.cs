using AutoMapper;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.EntryOfOtherDebitsModule;
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

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.IDMModule.EntryOfOtherDebits
{

    // <Summary>
    // Author: Gagana K; Module:EntryOfOtherDebits; Date: 27/10/2022
    // <summary>
    public class EntryOfOtherDebitsService: IEntryOfOtherDebitsService
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;
        private readonly IEntityRepository<IdmOthdebitsDetails> _idmOtherdebitsRepository;
        private readonly UserInfo _userInfo;

        public EntryOfOtherDebitsService(IEntityRepository<IdmOthdebitsDetails> idmOtherdebitsRepository, IUnitOfWork work,
                            IMapper mapper,  UserInfo userInfo)
        {
            _work = work;
            _mapper = mapper;
            _idmOtherdebitsRepository = idmOtherdebitsRepository;
            _userInfo = userInfo;
        }
        public async Task<IEnumerable<IdmOthdebitsDetailsDTO>> GetAllOtherDebitsList(long accountNumber, CancellationToken token)
        {
            var data = await _idmOtherdebitsRepository.FindByMatchingPropertiesAsync(token, x => x.LoanAcc == accountNumber && x.IsActive == true && x.IsDeleted == false).ConfigureAwait(false);
            return _mapper.Map<List<IdmOthdebitsDetailsDTO>>(data);
        }
        public async Task<bool> UpdateOtherDebitDetails(IdmOthdebitsDetailsDTO othdebit, CancellationToken token)
        {
            var currentDetails = await _idmOtherdebitsRepository.FirstOrDefaultByExpressionAsync(x => x.OthdebitDetId == othdebit.OthdebitDetId, token);
            if(currentDetails != null)
            {
                currentDetails.IsDeleted = true;
                currentDetails.IsActive = false;
                currentDetails.ModifiedBy = _userInfo.Name;
                currentDetails.ModifiedDate = DateTime.UtcNow;
                await _idmOtherdebitsRepository.UpdateAsync(currentDetails, token).ConfigureAwait(false);

            }


            var basicDetails = _mapper.Map<IdmOthdebitsDetails>(othdebit);
            basicDetails.OthdebitDetId = 0;
            basicDetails.IsActive = true;
            basicDetails.IsDeleted = false;
           // basicDetails.IsSubmitted = true;
            basicDetails.ModifiedBy = _userInfo.Name;
            basicDetails.ModifiedDate  = DateTime.UtcNow;

            var response = await _idmOtherdebitsRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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

        public async Task<bool> DeleteOtherDebitDetails(IdmOthdebitsDetailsDTO othdebit, CancellationToken token)
        {
            var basicDetails = _mapper.Map<IdmOthdebitsDetails>(othdebit);
            basicDetails.IsActive = false;
            basicDetails.IsDeleted = true;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;
         
            var response = await _idmOtherdebitsRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
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

        public async Task<bool> CreateOtherDebitDetails(IdmOthdebitsDetailsDTO othdebit, CancellationToken token)
        {
            var basicDetails = _mapper.Map<IdmOthdebitsDetails>(othdebit);
         
            basicDetails.IsActive = true;
           // basicDetails.IsSubmitted = true;
            basicDetails.CreateBy = _userInfo.Name;
            basicDetails.IsDeleted = false;
            basicDetails.CreatedDate = DateTime.UtcNow;

            var response = await _idmOtherdebitsRepository.AddAsync(basicDetails, token).ConfigureAwait(false);
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

        public async Task<bool> SubmitOtherDebitDetails(IdmOthdebitsDetailsDTO othdebit, CancellationToken token)
        {
            var basicDetails = _mapper.Map<IdmOthdebitsDetails>(othdebit);
            basicDetails.IsSubmitted = true;
            basicDetails.ModifiedDate = DateTime.UtcNow;
            basicDetails.ModifiedBy = _userInfo.Name;

            var response = await _idmOtherdebitsRepository.UpdateAsync(basicDetails, token).ConfigureAwait(false);
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
