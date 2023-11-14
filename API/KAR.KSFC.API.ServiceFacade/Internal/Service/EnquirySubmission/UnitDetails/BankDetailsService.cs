using AutoMapper;

using KAR.KSFC.API.ServiceFacade.Internal.Interface.UnitDetails.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service.EnquirySubmission.UnitDetails
{
    /// <summary>
    /// Bank Details Service
    /// </summary>
    public class BankDetailsService : IBankDetails
    {
        private readonly IEntityRepository<TblEnqBankDet> _bankRepository;
        private readonly IEntityRepository<TblEnqTemptab> _enquiryRepository;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;
        public BankDetailsService(IEntityRepository<TblEnqBankDet> bankRepository, IEntityRepository<TblEnqTemptab> enquiryRepository, UserInfo userInfo, IMapper mapper, IUnitOfWork work)
        {
            _bankRepository = bankRepository;
            _enquiryRepository = enquiryRepository;
            _userInfo = userInfo;
            _mapper = mapper;
            _work = work;
        }

        /// <summary>
        /// Add Bank Details
        /// </summary>
        /// <param name="bankDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<BankDetailsDTO> AddBankDetailsAsync(BankDetailsDTO bankDTO, CancellationToken token)
        {
            var enquiry = await _enquiryRepository.FirstOrDefaultByExpressionAsync(x => x.EnqtempId == bankDTO.EnqtempId 
            && x.IsActive == true 
            && x.IsDeleted == false, token).ConfigureAwait(false);

            if (enquiry == null)
            {
                throw new ArgumentException("Enquiry data not found");
            }

            var bankDetails = _mapper.Map<TblEnqBankDet>(bankDTO);
            bankDetails.IsActive = true;
            bankDetails.IsDeleted = false;
            bankDetails.CreatedDate = DateTime.UtcNow;
            bankDetails.CreatedBy = _userInfo.UserId;
            bankDetails.UniqueId = Guid.NewGuid().ToString();

            var result = await _bankRepository.AddAsync(bankDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<BankDetailsDTO>(result);
        }

        /// <summary>
        /// Delete Bank Details
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> DeleteBankDetailsAsync(int enquiryId, CancellationToken token)
        {
            var bankDetails = await _bankRepository.FirstOrDefaultByExpressionAsync(x => x.EnqtempId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (bankDetails == null)
            {
                throw new ArgumentException("data not found");
            }

            bankDetails.IsActive = false;
            bankDetails.IsDeleted = true;
            bankDetails.CreatedBy = _userInfo.UserId;
            bankDetails.CreatedDate = DateTime.UtcNow;

            await _bankRepository.UpdateAsync(bankDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }

        /// <summary>
        /// Get Bank Details By Enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<BankDetailsDTO> GetByIdBankDetailsAsync(int enquiryId, CancellationToken token)
        {
            var bankDetails = await _bankRepository.FirstOrDefaultByExpressionAsync(x => x.EnqtempId == enquiryId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (bankDetails == null)
            {
                throw new ArgumentException("data not found");
            }
            return _mapper.Map<BankDetailsDTO>(bankDetails);
        }

        /// <summary>
        /// Update Bank Details
        /// </summary>
        /// <param name="bankDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<BankDetailsDTO> UpdateBankDetailsAsync(BankDetailsDTO bankDTO, CancellationToken token)
        {
            var bankDetails = await _bankRepository
                                    .FindByFirstOrDefalutMatchingPropertiesAsync(token, x => x.EnqtempId == bankDTO.EnqtempId
                                       && x.EnqBankId == bankDTO.EnqBankId
                                       && x.IsActive == true && x.IsDeleted == false)
                                    .ConfigureAwait(false);

            if (bankDetails == null)
            {
                throw new ArgumentException("No Data Found");
            }
            var updatedBankDetails = _mapper.Map<TblEnqBankDet>(bankDTO);

            updatedBankDetails.ModifiedBy = _userInfo.UserId;
            updatedBankDetails.ModifiedDate = DateTime.UtcNow;
            updatedBankDetails.IsActive = true;
            updatedBankDetails.IsDeleted = false;
            updatedBankDetails.CreatedBy = bankDetails.CreatedBy;
            updatedBankDetails.CreatedDate = bankDetails.CreatedDate;

            var result = await _bankRepository.UpdateAsync(updatedBankDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return _mapper.Map<BankDetailsDTO>(result);
        }
         
    }
}
