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
    public class GuarantorAssetsNetWorthDetailsService : IGuarantorAssetsNetWorthDetails
    {
        private readonly IEntityRepository<TblEnqGassetDet> _guarAssetsNWRepository;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;

        public GuarantorAssetsNetWorthDetailsService(IMapper mapper, UserInfo userInfo, IEntityRepository<TblEnqGassetDet> guarAssetsNWRepository, IUnitOfWork work)
        {
            _mapper = mapper;
            _userInfo = userInfo;
            _guarAssetsNWRepository = guarAssetsNWRepository;
            _work = work;
        }

        public async Task<IEnumerable<GuarantorAssetsNetWorthDTO>> AddGuarantorAssetsNetWorthDetails(List<GuarantorAssetsNetWorthDTO> GuarantorAssetNWDTO, CancellationToken token)
        {
            var detailsList = _mapper.Map<List<TblEnqGassetDet>>(GuarantorAssetNWDTO);

            detailsList.ForEach(x =>
            {
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.UserId;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var result = await _guarAssetsNWRepository.AddAsync(detailsList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<GuarantorAssetsNetWorthDTO>>(result);
        }

        public async Task<bool> DeleteGuarantorAssetsNetWorthDetails(int enquiryId, CancellationToken token)
        {
            var guarAssetsNetWorth = await _guarAssetsNWRepository.FirstOrDefaultByExpressionAsync(x => x.EnqtempId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (guarAssetsNetWorth == null)
            {
                throw new ArgumentException("data not found");
            }

            guarAssetsNetWorth.IsActive = false;
            guarAssetsNetWorth.IsDeleted = true;
            guarAssetsNetWorth.ModifiedBy = _userInfo.UserId;
            guarAssetsNetWorth.CreatedDate = DateTime.UtcNow;

            await _guarAssetsNWRepository.UpdateAsync(guarAssetsNetWorth, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }

        public async Task<List<GuarantorAssetsNetWorthDTO>> GetByIdGuarantorAssetsNetWorthDetails(int enquiryId, CancellationToken token)
        {
            var list = await _guarAssetsNWRepository.FindByExpressionAsync(x => x.EnqGuarassetId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (!list.Any())
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<List<GuarantorAssetsNetWorthDTO>>(list);
        }

        public async Task<IEnumerable<GuarantorAssetsNetWorthDTO>> UpdateGuarantorAssetsNetWorthDetails(List<GuarantorAssetsNetWorthDTO> GuarantorAssetNWDTO, CancellationToken token)
        {

            var guarantorAssetList= await _guarAssetsNWRepository.FindByExpressionAsync(x=>x.EnqtempId== GuarantorAssetNWDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            guarantorAssetList.ForEach(x =>
            {
                x.IsActive = false;
                x.IsDeleted = true;
                x.ModifiedBy = _userInfo.UserId;
                x.ModifiedDate = DateTime.UtcNow;
            });
            await _guarAssetsNWRepository.UpdateAsync(guarantorAssetList,token).ConfigureAwait(false);
            var detailsList = _mapper.Map<List<TblEnqGassetDet>>(GuarantorAssetNWDTO);
            detailsList.ForEach(x =>
            {
                x.EnqGuarassetId = 0;
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.UserId;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var result = await _guarAssetsNWRepository.AddAsync(detailsList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<GuarantorAssetsNetWorthDTO>>(result);
        }
    }
}
