using AutoMapper;

using KAR.KSFC.API.ServiceFacade.Internal.Interface.UnitDetails.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.EnquirySubmission.UnitDetails
{
    /// <summary>
    /// Registration Details Service
    /// </summary>
    public class RegistrationDetailsService : IRegistrationDetails
    {
        private readonly IEntityRepository<TblEnqRegnoDet> _registrationRepository;
        private readonly IEntityRepository<TblEnqTemptab> _enqRepository;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;
        public RegistrationDetailsService(IEntityRepository<TblEnqRegnoDet> registrationRepository, IEntityRepository<TblEnqTemptab> enqRepository, UserInfo userInfo, IMapper mapper, IUnitOfWork work)
        {
            _registrationRepository = registrationRepository;
            _enqRepository = enqRepository;
            _userInfo = userInfo;
            _mapper = mapper;
            _work = work;
        }

        /// <param name="token"></param>
        /// <summary>
        /// Add Registration Details
        /// </summary>
        /// <param name="registrationDTO"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RegistrationNoDetailsDTO>> AddRegistrationDetails(List<RegistrationNoDetailsDTO> registrationDTO, CancellationToken token)
        {
            var enquiry = await _enqRepository.FirstOrDefaultByExpressionAsync(x => x.EnqtempId == registrationDTO[0].EnqtempId
            && x.IsActive == true
            && x.IsDeleted == false, token).ConfigureAwait(false);

            if (enquiry == null)
            {
                throw new ArgumentException("Enquiry data not found");
            }

            var detailsList = _mapper.Map<List<TblEnqRegnoDet>>(registrationDTO);

            detailsList.ForEach(x =>
            {
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.UserId;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var result = await _registrationRepository.AddAsync(detailsList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<RegistrationNoDetailsDTO>>(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRegistrationDetails(int enquiryId, CancellationToken token)
        {
            var address = await _registrationRepository.FirstOrDefaultByExpressionAsync(x => x.EnqRegnoId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (address == null)
            {
                throw new ArgumentException("data not found");
            }

            address.IsActive = false;
            address.IsDeleted = true;
            address.ModifiedBy = _userInfo.UserId;
            address.CreatedDate = DateTime.UtcNow;

            await _registrationRepository.UpdateAsync(address, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RegistrationNoDetailsDTO>> GetRegistrationDetailsByEnquiryId(int enquiryId, CancellationToken token)
        {
            var list = await _registrationRepository.FindByExpressionAsync(x => x.EnqtempId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (!list.Any())
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<List<RegistrationNoDetailsDTO>>(list);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="registrationDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RegistrationNoDetailsDTO>> UpdateRegistrationDetails(List<RegistrationNoDetailsDTO> registrationDTO, CancellationToken token)
        {
            var enquiry = await _enqRepository.FirstOrDefaultByExpressionAsync(x => x.EnqtempId == registrationDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (enquiry == null)
            {
                throw new ArgumentException("Enquiry data not found");
            }

            var registrationUList = await _registrationRepository.FindByExpressionAsync(x => x.EnqtempId == registrationDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            registrationUList.ForEach(x =>
            {
                x.IsActive = false;
                x.IsDeleted = true;
                x.ModifiedBy = _userInfo.UserId;
                x.ModifiedDate = DateTime.UtcNow;
            });
            await _registrationRepository.UpdateAsync(registrationUList, token).ConfigureAwait(false);

            var registrationList = _mapper.Map<List<TblEnqRegnoDet>>(registrationDTO);

            registrationList.ForEach(x =>
            {
                x.EnqRegnoId = 0;
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.UserId;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var data = await _registrationRepository.AddAsync(registrationList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<RegistrationNoDetailsDTO>>(data);
        }

        /// <summary>
        /// Get Registration Details By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<RegistrationNoDetailsDTO> GetRegistrationNoDetailsById(int id, CancellationToken token)
        {
            var data = await _registrationRepository.FirstOrDefaultByExpressionAsync(x => x.EnqRegnoId == id && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (data == null)
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<RegistrationNoDetailsDTO>(data);
        }
    }
}
