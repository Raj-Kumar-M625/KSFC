using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using KAR.KSFC.API.ServiceFacade.Internal.Interface.UnitDetails.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.EnquirySubmission.UnitDetails
{
    /// <summary>
    /// Address Details
    /// </summary>
    public class AddressDetailsService : IAddressDetails
    {
        private readonly IEntityRepository<TblEnqAddressDet> _entityRepository;
        private readonly IEntityRepository<TblEnqTemptab> _enquiryRepository;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;
        public AddressDetailsService(IEntityRepository<TblEnqAddressDet> entityRepository, IEntityRepository<TblEnqTemptab> enquiryRepository, UserInfo userInfo, IMapper mapper, IUnitOfWork work)
        {
            _entityRepository = entityRepository;
            _enquiryRepository = enquiryRepository;
            _userInfo = userInfo;
            _mapper = mapper;
            _work = work;
        }
        /// <summary>
        /// Add Address Details
        /// </summary>
        /// <param name="addressDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AddressDetailsDTO>> AddAddressDetails(List<AddressDetailsDTO> addressDTO, CancellationToken token)
        {
            var enquiry = await _enquiryRepository.FirstOrDefaultByExpressionAsync(x => x.EnqtempId == addressDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (enquiry == null)
            {
                throw new ArgumentException("Enquiry data not found");
            }

            var addressList = _mapper.Map<List<TblEnqAddressDet>>(addressDTO);

            addressList.ForEach(x =>
            {
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.UserId;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var data = await _entityRepository.AddAsync(addressList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<AddressDetailsDTO>>(data);
        }
        /// <summary>
        /// Delete Address Details
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAddressDetails(int enquiryId, CancellationToken token)
        {
            var address = await _entityRepository.FirstOrDefaultByExpressionAsync(x => x.EnqAddresssId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (address == null)
            {
                throw new ArgumentException("data not found");
            }

            address.IsActive = false;
            address.IsDeleted = true;
            address.ModifiedBy = _userInfo.UserId;
            address.CreatedDate = DateTime.UtcNow;

            await _entityRepository.UpdateAsync(address, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }
        /// <summary>
        /// Get Address details
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<AddressDetailsDTO>> GetAddressDetailsByEnquiryId(int enquiryId, CancellationToken token)
        {
            var address = await _entityRepository.FindByExpressionAsync(x => x.EnqtempId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (address == null)
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<List<AddressDetailsDTO>>(address);
        }


        /// <summary>
        /// Get Address details By Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AddressDetailsDTO> GetByIdAddressDetails(int id, CancellationToken token)
        {
            var address = await _entityRepository.FirstOrDefaultByExpressionAsync(x => x.EnqAddresssId == id && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (address == null)
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<AddressDetailsDTO>(address);
        }

        /// <summary>
        /// Update Address Details
        /// </summary>
        /// <param name="addressDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<IEnumerable<AddressDetailsDTO>> UpdateAddressDetails(List<AddressDetailsDTO> addressDTO, CancellationToken token)
        {
            var enquiry = await _enquiryRepository.FirstOrDefaultByExpressionAsync(x => x.EnqtempId == addressDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (enquiry == null)
            {
                throw new ArgumentException("Enquiry data not found");
            }

            var addressUList= await _entityRepository.FindByExpressionAsync(x=>x.EnqtempId== addressDTO[0].EnqtempId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            addressUList.ForEach(x =>
            {
                x.IsActive = false;
                x.IsDeleted = true;
                x.ModifiedBy = _userInfo.UserId;
                x.ModifiedDate = DateTime.UtcNow;
            });
            await _entityRepository.UpdateAsync(addressUList, token).ConfigureAwait(false);

            var addressList = _mapper.Map<List<TblEnqAddressDet>>(addressDTO);

            addressList.ForEach(x =>
            {
                x.EnqAddresssId = 0;
                x.IsActive = true;
                x.IsDeleted = false;
                x.CreatedBy = _userInfo.UserId;
                x.CreatedDate = DateTime.UtcNow;
                x.UniqueId = Guid.NewGuid().ToString();
            });

            var data = await _entityRepository.AddAsync(addressList, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<List<AddressDetailsDTO>>(data);
        }


    }
}
