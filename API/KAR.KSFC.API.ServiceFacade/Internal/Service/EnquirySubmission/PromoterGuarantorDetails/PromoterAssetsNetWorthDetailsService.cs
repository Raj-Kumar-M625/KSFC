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
    public class PromoterAssetsNetWorthDetailsService : IPromoterAssetsNetWorthDetails
    {
        private readonly IEntityRepository<TblEnqPassetDet> _assetsNWRepository;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;
        public PromoterAssetsNetWorthDetailsService(IMapper mapper, UserInfo userInfo, IEntityRepository<TblEnqPassetDet> assetsNWRepository, IUnitOfWork work)
        {
            _mapper = mapper;
            _userInfo = userInfo;
            _assetsNWRepository = assetsNWRepository;
            _work = work;
        }
        public async Task<IEnumerable<PromoterAssetsNetWorthDTO>> AddPromoterAssetsNetWorthDetails(List<PromoterAssetsNetWorthDTO> AssetsNetWorthDTO, CancellationToken token)
        {
            var netWorthList = _mapper.Map<List<TblEnqPassetDet>>(AssetsNetWorthDTO);
            netWorthList.ForEach(x =>
            {
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.UserId;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var result = await _assetsNWRepository.AddAsync(netWorthList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<PromoterAssetsNetWorthDTO>>(result);
        }

        public async Task<bool> DeletePromoterAssetsNetWorthDetails(int enquiryId, CancellationToken token)
        {
            var promoter = await _assetsNWRepository.FirstOrDefaultByExpressionAsync(x => x.EnqPromassetId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (promoter == null)
            {
                throw new ArgumentException("data not found");
            }

            promoter.IsActive = false;
            promoter.IsDeleted = true;
            promoter.ModifiedBy = _userInfo.UserId;
            promoter.CreatedDate = DateTime.UtcNow;

            await _assetsNWRepository.UpdateAsync(promoter, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }

        public async Task<List<PromoterAssetsNetWorthDTO>> GetByIdPromoterAssetsNetWorthDetails(int enquiryId, CancellationToken token)
        {
            var list = await _assetsNWRepository.FindByExpressionAsync(x => x.EnqPromassetId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (!list.Any())
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<List<PromoterAssetsNetWorthDTO>>(list);
        }

        public async Task<IEnumerable<PromoterAssetsNetWorthDTO>> UpdatePromoterAssetsNetWorthDetails(List<PromoterAssetsNetWorthDTO> AssetsNetWorthDTO, CancellationToken token)
        {
            var dataList = await _assetsNWRepository.FindByExpressionAsync(x => x.EnqtempId == AssetsNetWorthDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);

            dataList.ForEach(x =>
            {
                x.IsActive = false;
                x.IsDeleted = true;
                x.ModifiedBy = _userInfo.Pan;
                x.ModifiedDate = DateTime.UtcNow;
            });
            await _assetsNWRepository.UpdateAsync(dataList, token).ConfigureAwait(false);

            var netWorthList = _mapper.Map<List<TblEnqPassetDet>>(AssetsNetWorthDTO);
            netWorthList.ForEach(x =>
            {
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.Pan;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var result = await _assetsNWRepository.AddAsync(netWorthList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<PromoterAssetsNetWorthDTO>>(result);

        }
    }
}
