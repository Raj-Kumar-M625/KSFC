using AutoMapper;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission;
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
    public class SisterConcernDetailsService : ISisterConcernDetails
    {

        private readonly IEntityRepository<TblEnqSisDet> _siterDetailsRepository;
        private readonly IEntityRepository<TblEnqSfinDet> _sisFy;
        private readonly IEntityRepository<TblEnqTemptab> _enqRepository;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;
        private readonly IEnquiryHomeService _enquiryService;
        public SisterConcernDetailsService(IMapper mapper, UserInfo userInfo, IEntityRepository<TblEnqSisDet> siterDetailsRepository, IEntityRepository<TblEnqSfinDet> sisFy,
            IUnitOfWork work, IEnquiryHomeService enquiryHomeService, IEntityRepository<TblEnqTemptab> enqRepository = null)
        {
            _mapper = mapper;
            _userInfo = userInfo;
            _siterDetailsRepository = siterDetailsRepository;
            _work = work;
            _enqRepository = enqRepository;
            _enquiryService = enquiryHomeService;
            _sisFy = sisFy;
        }
        public async Task<IEnumerable<SisterConcernDetailsDTO>> AddSisterConcernDetails(List<SisterConcernDetailsDTO> SisterDTO, CancellationToken token)
        {
            var checkEnqTempId = SisterDTO.FirstOrDefault(x => x.EnqtempId == null || x.EnqtempId == 0);
            if (checkEnqTempId != null)
            {
                var tempId = await _enquiryService.AddNewEnqiry(_userInfo.Pan, token).ConfigureAwait(false);
                SisterDTO.ForEach(x => x.EnqtempId = tempId);
               
            }
            var detailsList = _mapper.Map<List<TblEnqSisDet>>(SisterDTO);
            detailsList.ForEach(x =>
            {
                x.EnqSisId = 0;
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.Pan;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });
            var result = await _siterDetailsRepository.AddAsync(detailsList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            await _enquiryService.UpdateEnquiryAssociateSisterConcernStatus(SisterDTO.FirstOrDefault().EnqtempId.Value, false, token);
            return _mapper.Map<List<SisterConcernDetailsDTO>>(result);
        }

        public async Task<bool> DeleteSisterConcernDetails(int EnqSisId, CancellationToken token)
        {
            var sister = await _siterDetailsRepository.FirstOrDefaultByExpressionAsync(x => x.EnqSisId == EnqSisId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
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
        public async Task<bool> DeleteSisterConcernDetailsByEnqId(int EnqtempId, CancellationToken token)
        {

            var sisterConcernList = await _siterDetailsRepository.FindByExpressionAsync(x => x.EnqtempId == EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (sisterConcernList.Count > 0)
            {
                sisterConcernList.ForEach(x =>
                {
                    x.IsActive = false;
                    x.IsDeleted = true;
                    x.ModifiedBy = _userInfo.Pan;
                    x.ModifiedDate = DateTime.UtcNow;
                });
                await _siterDetailsRepository.UpdateAsync(sisterConcernList, token).ConfigureAwait(false);
                await _work.CommitAsync(token).ConfigureAwait(false);
                var EnqSisId = sisterConcernList.Select(x => x.EnqSisId);
                var sisterUList = await _sisFy.FindByExpressionAsync(x => EnqSisId.Contains(x.EnqSisId) && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
                if (sisterUList.Count > 0)
                {
                    sisterUList.ForEach(x =>
                    {
                        x.IsActive = false;
                        x.IsDeleted = true;
                        x.ModifiedBy = _userInfo.Pan;
                        x.ModifiedDate = DateTime.UtcNow;
                    });
                    await _sisFy.UpdateAsync(sisterUList, token).ConfigureAwait(false);
                    await _work.CommitAsync(token).ConfigureAwait(false);
                }

            }

            return true;
        }

        public async Task<List<SisterConcernDetailsDTO>> GetByIdSisterConcernDetails(int enquiryId, CancellationToken token)
        {
            var list = await _siterDetailsRepository.FindByExpressionAsync(x => x.EnqSisId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (!list.Any())
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<List<SisterConcernDetailsDTO>>(list);
        }

        public async Task<IEnumerable<SisterConcernDetailsDTO>> UpdateSisterConcernDetails(List<SisterConcernDetailsDTO> SisterConcernDTO, CancellationToken token)
        {
            var enquiry = await _enqRepository.FirstOrDefaultByExpressionAsync(x => x.EnqtempId == SisterConcernDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (enquiry == null)
            {
                throw new ArgumentException("Enquiry data not found");
            }

            var sisterUList = await _siterDetailsRepository.FirstOrDefaultByExpressionAsync(x => x.EnqSisId == SisterConcernDTO[0].EnqSisId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            var itemtoUpdate = SisterConcernDTO.FirstOrDefault();
            sisterUList.EnqSisName = itemtoUpdate.EnqSisName;
            sisterUList.EnqSisIfsc = itemtoUpdate.EnqSisIfsc;
            sisterUList.BfacilityCode = itemtoUpdate.BfacilityCode;
            sisterUList.EnqOutamt = itemtoUpdate.EnqOutamt;
            sisterUList.EnqDeftamt = itemtoUpdate.EnqDeftamt;
            sisterUList.EnqRelief = itemtoUpdate.EnqRelief;
            sisterUList.EnqOts = itemtoUpdate.EnqOts;

            var sisItem = await _siterDetailsRepository.UpdateAsync(sisterUList, token).ConfigureAwait(false);
            var tblList = new List<TblEnqSisDet>();
            tblList.Add(sisItem);
            await _work.CommitAsync(token).ConfigureAwait(false);

            return _mapper.Map<List<SisterConcernDetailsDTO>>(tblList);
        }
    }
}
