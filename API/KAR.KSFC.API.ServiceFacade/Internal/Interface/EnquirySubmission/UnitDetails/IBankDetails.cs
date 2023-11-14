using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;

using System.Threading;
using System.Threading.Tasks;

 

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.UnitDetails.EnquirySubmission
{
    /// <summary>
    /// Bank Details Interface
    /// </summary>
    public interface IBankDetails
    {
        /// <summary>
        /// Add Bank details for enquiry submission
        /// </summary>
        /// <param name="bankDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<BankDetailsDTO> AddBankDetailsAsync(BankDetailsDTO bankDTO, CancellationToken token);

        /// <summary>
        /// Update Bank details for enquiry submission
        /// </summary>
        /// <param name="addressDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<BankDetailsDTO> UpdateBankDetailsAsync(BankDetailsDTO bankDTO, CancellationToken token);

        /// <summary>
        /// Get bank Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<BankDetailsDTO> GetByIdBankDetailsAsync(int enquiryId, CancellationToken token);

        /// <summary>
        /// Remove bank Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<bool> DeleteBankDetailsAsync(int enquiryId, CancellationToken token);
    }
}
