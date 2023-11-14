using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
 

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.PromoterGuarantorDetails
{
    /// <summary>
    /// Guarantor Liability Net Worth Details Interface
    /// </summary>
    public interface IGuarantorLiabilityNetWorth
    {
        /// <summary>
        /// Add Guarantor  Net Worth details for enquiry submission
        /// </summary>
        /// <param name="GuarantorDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<GuarantorNetWorthDetailsDTO>> AddGuarantorNetWorthDetails(List<GuarantorNetWorthDetailsDTO> NetWorthDTO, CancellationToken token);

        /// <summary>
        /// Update Guarantor Liability Net Worth details for enquiry submission
        /// </summary>
        /// <param name="GuarantorDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<GuarantorNetWorthDetailsDTO>> UpdateGuarantorNetWorthDetails(List<GuarantorNetWorthDetailsDTO> NetWorthDTO, CancellationToken token);

        /// <summary>
        /// Get Guarantor Liability Net Worth Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<List<GuarantorNetWorthDetailsDTO>> GetByIdGuarantorNetWorthDetails(int enquiryId, CancellationToken token);

        /// <summary>
        /// Remove Guarantor Liability Net Worth Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<bool> DeleteGuarantorNetWorthDetails(int enquiryId, CancellationToken token);
        }
    }

