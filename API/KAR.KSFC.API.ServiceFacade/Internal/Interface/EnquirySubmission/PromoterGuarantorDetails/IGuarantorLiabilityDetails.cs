using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
 

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.PromoterGuarantorDetails
{
    /// <summary>
    /// Guarantor Liability Details Interface
    /// </summary>
    public interface IGuarantorLiabilityDetails
    {
        /// <summary>
        /// Add Guarantor Liability details for enquiry submission
        /// </summary>
        /// <param name="GuarantorDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<GuarantorLiabilityDetailsDTO>> AddGuarantorLiabilityDetails(List<GuarantorLiabilityDetailsDTO> GuarantorDTO, CancellationToken token);

        /// <summary>
        /// Update Guarantor Liability details for enquiry submission
        /// </summary>
        /// <param name="GuarantorDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<GuarantorLiabilityDetailsDTO>> UpdateGuarantorLiabilityDetails(List<GuarantorLiabilityDetailsDTO> GuarantorDTO, CancellationToken token);

        /// <summary>
        /// Get Guarantor Liability Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<List<GuarantorLiabilityDetailsDTO>> GetByIdGuarantorLiabilityDetails(int enquiryId, CancellationToken token);

        /// <summary>
        /// Remove Guarantor LiabilityDetails by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<bool> DeleteGuarantorLiabilityDetails(int enquiryId, CancellationToken token);
        
    }
}
