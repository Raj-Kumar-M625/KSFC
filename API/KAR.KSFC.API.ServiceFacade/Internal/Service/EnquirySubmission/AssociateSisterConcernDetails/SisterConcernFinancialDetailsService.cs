using AutoMapper;

using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.AssociateSisterConcernDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.AssociateSisterConcernDetails;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.EnquirySubmission.AssociateSisterConcernDetails
{
    public class SisterConcernFinancialDetailsService : ISisterConcernFinancialDetails
    {

        private readonly IEntityRepository<TblEnqSfinDet> _siterDetailsRepository;
        private readonly IEntityRepository<TblEnqSisDet> _enqRepository;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;
        public SisterConcernFinancialDetailsService(IMapper mapper, UserInfo userInfo, IEntityRepository<TblEnqSfinDet> siterDetailsRepository, IUnitOfWork work, IEntityRepository<TblEnqSisDet> enqRepository = null)
        {
            _mapper = mapper;
            _userInfo = userInfo;
            _siterDetailsRepository = siterDetailsRepository;
            _work = work;
            _enqRepository = enqRepository;
        }
        public async Task<IEnumerable<SisterConcernFinancialDetailsDTO>> AddSisterConcernFinancialDetails(List<SisterConcernFinancialDetailsDTO> SisterDTO, CancellationToken token)
        {

            var detailsList = _mapper.Map<List<TblEnqSfinDet>>(SisterDTO);

            detailsList.ForEach(x =>
            {
                x.EnqSisfinId = 0;
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.Pan;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var result = await _siterDetailsRepository.AddAsync(detailsList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<SisterConcernFinancialDetailsDTO>>(result);
        }

        public async Task<bool> DeleteSisterConcernFinancialDetails(int enquiryId, CancellationToken token)
        {
            var sister = await _siterDetailsRepository.FirstOrDefaultByExpressionAsync(x => x.EnqSisId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (sister == null)
            {
                throw new ArgumentException("data not found");
            }

            sister.IsActive = false;
            sister.IsDeleted = true;
            sister.ModifiedBy = _userInfo.Pan;
            sister.CreatedDate = DateTime.UtcNow;

            await _siterDetailsRepository.UpdateAsync(sister, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }

        public async Task<List<SisterConcernFinancialDetailsDTO>> GetByIdSisterConcernFinancialDetails(int enquiryId, CancellationToken token)
        {
            var list = await _siterDetailsRepository.FindByMatchingPropertiesAsync
                (token, x => x.EnqSisId == enquiryId && x.IsActive == true && x.IsDeleted == false,
                sisF => sisF.EnqSis,
                fin => fin.FinyearCodeNavigation,
                ficompo => ficompo.FincompCdNavigation
                ).ConfigureAwait(false);
            
            return _mapper.Map<List<SisterConcernFinancialDetailsDTO>>(list);
        }

        public Task<SisterConcernFinancialDetailsDTO> GetBySisterIdConcernFinancialDetails(int sisterId, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SisterConcernFinancialDetailsDTO>> UpdateSisterConcernFinancialDetails(List<SisterConcernFinancialDetailsDTO> SisterConcernDTO, CancellationToken token)
        {
            var enquirySis = await _enqRepository.FindByExpressionAsync(x => x.EnqtempId == SisterConcernDTO[0].EnqtempId, token).ConfigureAwait(false);
            var EnqSisId = enquirySis.Select(x => x.EnqSisId);
            var sisterUList = await _siterDetailsRepository.FindByExpressionAsync(x => EnqSisId.Contains(x.EnqSisId) && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            sisterUList.ForEach(x =>
            {
                x.IsActive = false;
                x.IsDeleted = true;
                x.ModifiedBy = _userInfo.Pan;
                x.ModifiedDate = DateTime.UtcNow;
            });
            await _siterDetailsRepository.UpdateAsync(sisterUList, token).ConfigureAwait(false);

            var sisterList = _mapper.Map<List<TblEnqSfinDet>>(SisterConcernDTO);

            sisterList.ForEach(x =>
            {
                x.EnqSisfinId = 0;
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.Pan;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var data = await _siterDetailsRepository.AddAsync(sisterList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<SisterConcernFinancialDetailsDTO>>(data);
        }
    }
}
