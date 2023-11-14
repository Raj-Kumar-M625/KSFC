using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.PromoterGuarantorDetails
{
    /// <summary>
    /// Promoter Liability Details Interface
    /// </summary>
    public interface IPromoterLiabilityDetails
    {
        /// <summary>
        /// Add Promoter Liability details for enquiry submission
        /// </summary>
        /// <param name="PromoterDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<PromoterLiabilityDetailsDTO>> AddPromoterLiabilityDetails(List<PromoterLiabilityDetailsDTO> ProLiabilityDTO, CancellationToken token);

        /// <summary>
        /// Update Promoter Liability details for enquiry submission
        /// </summary>
        /// <param name="PromoterDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<PromoterLiabilityDetailsDTO>> UpdatePromoterLiabilityDetails(List<PromoterLiabilityDetailsDTO> PromoterDTO, CancellationToken token);

        /// <summary>
        /// Get Promoter LiabilityDetails by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<List<PromoterLiabilityDetailsDTO>> GetByIdPromoterLiabilityDetails(int enquiryId, CancellationToken token);

        /// <summary>
        /// Remove Promoter LiabilityDetails by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<bool> DeletePromoterLiabilityDetails(int enquiryId, CancellationToken token);
        
    }
}
