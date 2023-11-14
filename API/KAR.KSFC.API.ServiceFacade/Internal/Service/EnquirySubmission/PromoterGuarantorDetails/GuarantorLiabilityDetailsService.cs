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
    public class GuarantorLiabilityDetailsService : IGuarantorLiabilityDetails
    {
        private readonly IEntityRepository<TblEnqGliabDet> _guarLiabilityRepository;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;
        public GuarantorLiabilityDetailsService(IMapper mapper, UserInfo userInfo, IEntityRepository<TblEnqGliabDet> guarLiabilityRepository, IUnitOfWork work)
        {
            _mapper = mapper;
            _userInfo = userInfo;
            _guarLiabilityRepository = guarLiabilityRepository;
            _work = work;
        }
        public async Task<IEnumerable<GuarantorLiabilityDetailsDTO>> AddGuarantorLiabilityDetails(List<GuarantorLiabilityDetailsDTO> LiabilityDTO, CancellationToken token)
        {
            var detailsList = _mapper.Map<List<TblEnqGliabDet>>(LiabilityDTO);

            detailsList.ForEach(x =>
            {
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.UserId;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var result = await _guarLiabilityRepository.AddAsync(detailsList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<GuarantorLiabilityDetailsDTO>>(result);
        }

        public async Task<bool> DeleteGuarantorLiabilityDetails(int enquiryId, CancellationToken token)
        {
            var guarantorLiability = await _guarLiabilityRepository.FirstOrDefaultByExpressionAsync(x => x.EnqGuarliabId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (guarantorLiability == null)
            {
                throw new ArgumentException("data not found");
            }

            guarantorLiability.IsActive = false;
            guarantorLiability.IsDeleted = true;
            guarantorLiability.ModifiedBy = _userInfo.UserId;
            guarantorLiability.CreatedDate = DateTime.UtcNow;

            await _guarLiabilityRepository.UpdateAsync(guarantorLiability, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }

        public async Task<List<GuarantorLiabilityDetailsDTO>> GetByIdGuarantorLiabilityDetails(int enquiryId, CancellationToken token)
        {
            var list = await _guarLiabilityRepository.FindByExpressionAsync(x => x.EnqGuarliabId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (!list.Any())
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<List<GuarantorLiabilityDetailsDTO>>(list);
        }

        public async Task<IEnumerable<GuarantorLiabilityDetailsDTO>> UpdateGuarantorLiabilityDetails(List<GuarantorLiabilityDetailsDTO> GuarantorDTO, CancellationToken token)
        {
            var dataList= await _guarLiabilityRepository.FindByExpressionAsync(x=>x.EnqtempId== GuarantorDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            dataList.ForEach(x =>
            {
                x.IsDeleted = true;
                x.IsActive = false;
                x.ModifiedBy = _userInfo.UserId;
                x.ModifiedDate = DateTime.UtcNow;
            });
            await _guarLiabilityRepository.UpdateAsync(dataList, token).ConfigureAwait(false);

            var detailsList = _mapper.Map<List<TblEnqGliabDet>>(GuarantorDTO);

            detailsList.ForEach(x =>
            {
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.UserId;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var result = await _guarLiabilityRepository.AddAsync(detailsList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<GuarantorLiabilityDetailsDTO>>(result);
        }
    }
}
