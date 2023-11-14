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
    public class PromoterLiabilityDetailsService : IPromoterLiabilityDetails
    {
        private readonly IEntityRepository<TblEnqPliabDet> _proLiabilityRepository;
        private readonly IEntityRepository<TblEnqTemptab> _enqRepository;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;
        public PromoterLiabilityDetailsService(IMapper mapper, UserInfo userInfo, IEntityRepository<TblEnqPliabDet> proLiabilityRepository, IUnitOfWork work, IEntityRepository<TblEnqTemptab> enqRepository = null)
        {
            _mapper = mapper;
            _userInfo = userInfo;
            _proLiabilityRepository = proLiabilityRepository;
            _work = work;
            _enqRepository = enqRepository;
        }
        public async Task<IEnumerable<PromoterLiabilityDetailsDTO>> AddPromoterLiabilityDetails(List<PromoterLiabilityDetailsDTO> liabilityDTO, CancellationToken token)
        {
            var liabilityList = _mapper.Map<List<TblEnqPliabDet>>(liabilityDTO);

            liabilityList.ForEach(x =>
            {
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.UserId;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });
            var result = await _proLiabilityRepository.AddAsync(liabilityList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);

            return _mapper.Map<List<PromoterLiabilityDetailsDTO>>(result);
        }

        public async Task<bool> DeletePromoterLiabilityDetails(int enquiryId, CancellationToken token)
        {
            var promoter = await _proLiabilityRepository.FirstOrDefaultByExpressionAsync(x => x.EnqPromliabId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (promoter == null)
            {
                throw new ArgumentException("data not found");
            }

            promoter.IsActive = false;
            promoter.IsDeleted = true;
            promoter.ModifiedBy = _userInfo.UserId;
            promoter.CreatedDate = DateTime.UtcNow;

            await _proLiabilityRepository.UpdateAsync(promoter, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }

        public async Task<List<PromoterLiabilityDetailsDTO>> GetByIdPromoterLiabilityDetails(int enquiryId, CancellationToken token)
        {
            var list = await _proLiabilityRepository.FindByExpressionAsync(x => x.EnqPromliabId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (!list.Any())
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<List<PromoterLiabilityDetailsDTO>>(list);
        }

        public async Task<IEnumerable<PromoterLiabilityDetailsDTO>> UpdatePromoterLiabilityDetails(List<PromoterLiabilityDetailsDTO> PromoterDTO, CancellationToken token)
        {
            var enquiry = await _enqRepository.FirstOrDefaultByExpressionAsync(x => x.EnqtempId == PromoterDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (enquiry == null)
            {
                throw new ArgumentException("Enquiry data not found");
            }

            var liaUList = await _proLiabilityRepository.FindByExpressionAsync(x => x.EnqtempId == PromoterDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            liaUList.ForEach(x =>
            {
                x.IsActive = false;
                x.IsDeleted = true;
                x.ModifiedBy = _userInfo.UserId;
                x.ModifiedDate = DateTime.UtcNow;
            });
            await _proLiabilityRepository.UpdateAsync(liaUList, token).ConfigureAwait(false);

            var sisterList = _mapper.Map<List<TblEnqPliabDet>>(PromoterDTO);

            sisterList.ForEach(x =>
            {
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.UserId;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var data = await _proLiabilityRepository.AddAsync(sisterList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<PromoterLiabilityDetailsDTO>>(data);
        }
    }
}
