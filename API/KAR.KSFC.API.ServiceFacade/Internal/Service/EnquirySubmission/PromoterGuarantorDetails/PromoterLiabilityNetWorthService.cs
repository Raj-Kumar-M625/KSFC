using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.EnquirySubmission.PromoterGuarantorDetails
{
    public class PromoterLiabilityNetWorthService : IPromoterLiabilityNetWorth
    {
        private readonly IEntityRepository<TblEnqPnwDet> _enqPNWRepository;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;
        public PromoterLiabilityNetWorthService(IMapper mapper, UserInfo userInfo, IEntityRepository<TblEnqPnwDet> enqPNWRepository, IUnitOfWork work)
        {
            _mapper = mapper;
            _userInfo = userInfo;
            _enqPNWRepository = enqPNWRepository;
            _work = work;
        }
        public async Task<IEnumerable<PromoterNetWorthDetailsDTO>> AddPromoterLiabilityNetWorthDetails(List<PromoterNetWorthDetailsDTO> NetWorthDTO, CancellationToken token)
        {

            var dataList = await _enqPNWRepository.FindByExpressionAsync(x => x.EnqtempId == NetWorthDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (dataList != null && dataList.Count > 0)
            {
                dataList.ForEach(x =>
                {
                    x.IsDeleted = true;
                    x.IsActive = false;
                    x.ModifiedBy = _userInfo.Pan;
                    x.ModifiedDate = DateTime.UtcNow;
                });
                await _enqPNWRepository.UpdateAsync(dataList, token).ConfigureAwait(false);
            }
            var netWorthList = _mapper.Map<List<TblEnqPnwDet>>(NetWorthDTO);
            netWorthList.ForEach(x =>
            {
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.Pan;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var result = await _enqPNWRepository.AddAsync(netWorthList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);

            return _mapper.Map<List<PromoterNetWorthDetailsDTO>>(result);
        }

        public async Task<bool> DeletePromoterLiabilityNetWorthDetails(int enquiryId, CancellationToken token)
        {
            var promoter = await _enqPNWRepository.FirstOrDefaultByExpressionAsync(x => x.EnqPromnwId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (promoter == null)
            {
                throw new ArgumentException("data not found");
            }

            promoter.IsActive = false;
            promoter.IsDeleted = true;
            promoter.ModifiedBy = _userInfo.UserId;
            promoter.CreatedDate = DateTime.UtcNow;

            await _enqPNWRepository.UpdateAsync(promoter, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }

        public async Task<List<PromoterNetWorthDetailsDTO>> GetByIdPromoterLiabilityNetWorthDetails(int enquiryId, CancellationToken token)
        {
            var list = await _enqPNWRepository.FindByExpressionAsync(x => x.EnqPromnwId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (!list.Any())
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<List<PromoterNetWorthDetailsDTO>>(list);
        }

        public async Task<IEnumerable<PromoterNetWorthDetailsDTO>> UpdatePromoterLiabilityNetWorthDetails(List<PromoterNetWorthDetailsDTO> PromoterDTO, CancellationToken token)
        {

            var dataList = await _enqPNWRepository.FindByExpressionAsync(x => x.EnqtempId == PromoterDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
           
            dataList.ForEach(x =>
            {
                x.IsDeleted = true;
                x.IsActive = false;
                x.ModifiedBy = _userInfo.Pan;
                x.ModifiedDate = DateTime.UtcNow;
            });
            await _enqPNWRepository.UpdateAsync(dataList, token).ConfigureAwait(false);

            var netWorthList = _mapper.Map<List<TblEnqPnwDet>>(PromoterDTO);

            netWorthList.ForEach(x =>
            {
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.Pan;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var result = await _enqPNWRepository.AddAsync(netWorthList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);

            return _mapper.Map<List<PromoterNetWorthDetailsDTO>>(result);
        }
    }
}
