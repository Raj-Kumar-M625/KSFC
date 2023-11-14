using AutoMapper;

using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.SecurityDocumentDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.EnquirySubmission.SecurityDocumentDetails
{
    public class SecurityDetailsService : ISecurityDetails
    {
        private readonly IEntityRepository<TblEnqSecDet> _securityDetailsRepository;
        private readonly IEnquiryHomeService _enquiryService;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;


        public SecurityDetailsService(IMapper mapper, UserInfo userInfo, IEntityRepository<TblEnqSecDet> securityDetailsRepository, IUnitOfWork work, IEnquiryHomeService enquiryService)
        {
            _mapper = mapper;
            _userInfo = userInfo;
            _securityDetailsRepository = securityDetailsRepository;
            _work = work;
            _enquiryService = enquiryService;
        }

        public async Task<IEnumerable<SecurityDetailsDTO>> AddSecurityDetails(List<SecurityDetailsDTO> SecurityDetailsDTO, CancellationToken token)
        {
            if (SecurityDetailsDTO.Where(x => x.EnqtempId == 0).Any())
            {
                var enqId = await _enquiryService.AddNewEnqiry(_userInfo.Pan, token).ConfigureAwait(false);
                SecurityDetailsDTO.ForEach(x => x.EnqtempId = enqId);
            }

            var dataList = await _securityDetailsRepository.FindByExpressionAsync(x => x.EnqtempId == SecurityDetailsDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            dataList.ForEach(x =>
            {
                x.IsDeleted = true;
                x.IsActive = false;
                x.ModifiedBy = _userInfo.UserId;
                x.ModifiedDate = DateTime.UtcNow;
            });
            await _securityDetailsRepository.UpdateAsync(dataList, token).ConfigureAwait(false);

            var detailsList = _mapper.Map<List<TblEnqSecDet>>(SecurityDetailsDTO);
            detailsList.ForEach(x =>
            {
                x.EnqSecId = 0;
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.UserId;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var result = await _securityDetailsRepository.AddAsync(detailsList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<SecurityDetailsDTO>>(result);
        }

        public async Task<bool> DeleteSecurityDetails(int enquiryId, CancellationToken token)
        {
            var security = await _securityDetailsRepository.FirstOrDefaultByExpressionAsync(x => x.EnqSecId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (security == null)
            {
                throw new ArgumentException("data not found");
            }

            security.IsActive = false;
            security.IsDeleted = true;
            security.ModifiedBy = _userInfo.UserId;
            security.CreatedDate = DateTime.UtcNow;

            await _securityDetailsRepository.UpdateAsync(security, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);

            return true;
        }

        public async Task<List<SecurityDetailsDTO>> GetByIdSecurityDetails(int enquiryId, CancellationToken token)
        {
            var list = await _securityDetailsRepository.FindByExpressionAsync(x => x.EnqSecId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (!list.Any())
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<List<SecurityDetailsDTO>>(list);
        }

        public async Task<IEnumerable<SecurityDetailsDTO>> UpdateSecurityDetails(List<SecurityDetailsDTO> SecurityDetailsDTO, CancellationToken token)
        {

            var dataList = await _securityDetailsRepository.FindByExpressionAsync(x => x.EnqtempId == SecurityDetailsDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            dataList.ForEach(x =>
            {
                x.IsDeleted = true;
                x.IsActive = false;
                x.ModifiedBy = _userInfo.UserId;
                x.ModifiedDate = DateTime.UtcNow;
            });
            await _securityDetailsRepository.UpdateAsync(dataList, token).ConfigureAwait(false);
            var detailsList = _mapper.Map<List<TblEnqSecDet>>(SecurityDetailsDTO);

            detailsList.ForEach(x =>
            {
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.UserId;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var result = await _securityDetailsRepository.AddAsync(detailsList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<SecurityDetailsDTO>>(result);
        }
    }
}
