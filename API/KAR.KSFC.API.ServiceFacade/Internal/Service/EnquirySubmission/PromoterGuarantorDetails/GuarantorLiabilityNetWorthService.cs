using AutoMapper;

using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.EnquirySubmission.PromoterGuarantorDetails
{
    public class GuarantorLiabilityNetWorthService : IGuarantorLiabilityNetWorth
    {
        private readonly IEntityRepository<TblEnqGnwDet> _guarNetWorthRepository;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;
        public GuarantorLiabilityNetWorthService(IMapper mapper, UserInfo userInfo, IEntityRepository<TblEnqGnwDet> guarNetWorthRepository, IUnitOfWork work)
        {
            _mapper = mapper;
            _userInfo = userInfo;
            _guarNetWorthRepository = guarNetWorthRepository;
            _work = work;
        }

        public async Task<IEnumerable<GuarantorNetWorthDetailsDTO>> AddGuarantorNetWorthDetails(List<GuarantorNetWorthDetailsDTO> NetWorthDTO, CancellationToken token)
        {
            var dataList = await _guarNetWorthRepository.FindByExpressionAsync(x => x.EnqtempId == NetWorthDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if(dataList!=null&& dataList.Count>0)
            {
                dataList.ForEach(x =>
                {
                    x.IsActive = false;
                    x.IsDeleted = true;
                    x.ModifiedBy = _userInfo.Pan;
                    x.ModifiedDate = DateTime.UtcNow;
                });
                await _guarNetWorthRepository.UpdateAsync(dataList, token).ConfigureAwait(false);
            }
           
            var detailsList = _mapper.Map<List<TblEnqGnwDet>>(NetWorthDTO);

            detailsList.ForEach(x =>
            {
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.Pan;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var result = await _guarNetWorthRepository.AddAsync(detailsList, token).ConfigureAwait(false);

            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<GuarantorNetWorthDetailsDTO>>(result);
        }

        public async Task<bool> DeleteGuarantorNetWorthDetails(int enquiryId, CancellationToken token)
        {
            var guarNetWorth = await _guarNetWorthRepository.FirstOrDefaultByExpressionAsync(x => x.EnqtempId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (guarNetWorth == null)
            {
                throw new ArgumentException("data not found");
            }

            guarNetWorth.IsActive = false;
            guarNetWorth.IsDeleted = true;
            guarNetWorth.ModifiedBy = _userInfo.Pan;
            guarNetWorth.CreatedDate = DateTime.UtcNow;

            await _guarNetWorthRepository.UpdateAsync(guarNetWorth, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }

        public async Task<List<GuarantorNetWorthDetailsDTO>> GetByIdGuarantorNetWorthDetails(int enquiryId, CancellationToken token)
        {
            var list = await _guarNetWorthRepository.FindByExpressionAsync(x => x.EnqGuarnwId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (!list.Any())
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<List<GuarantorNetWorthDetailsDTO>>(list);
        }

        public async Task<IEnumerable<GuarantorNetWorthDetailsDTO>> UpdateGuarantorNetWorthDetails(List<GuarantorNetWorthDetailsDTO> NetWorthDTO, CancellationToken token)
        {

            var dataList= await _guarNetWorthRepository.FindByExpressionAsync(x=>x.EnqtempId == NetWorthDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);

            if (dataList != null && dataList.Count > 0)
            {
                dataList.ForEach(x =>
                {
                    x.IsActive = false;
                    x.IsDeleted = true;
                    x.ModifiedBy = _userInfo.Pan;
                    x.ModifiedDate = DateTime.UtcNow;
                });
                await _guarNetWorthRepository.UpdateAsync(dataList, token).ConfigureAwait(false);
            }

            var detailsList = _mapper.Map<List<TblEnqGnwDet>>(NetWorthDTO);
            detailsList.ForEach(x =>
            {
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.Pan;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var result = await _guarNetWorthRepository.AddAsync(detailsList, token).ConfigureAwait(false);

            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<GuarantorNetWorthDetailsDTO>>(result);
        }
    }
}
