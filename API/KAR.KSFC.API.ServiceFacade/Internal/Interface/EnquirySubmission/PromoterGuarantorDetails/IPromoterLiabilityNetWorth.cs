using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;



namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.PromoterGuarantorDetails
{
    /// <summary>
    /// Promoter Liability Net Worth Details Interface
    /// </summary>
    public interface IPromoterLiabilityNetWorth
    {
        /// <summary>
        /// Add Promoter Liability Net Worth details for enquiry submission
        /// </summary>
        /// <param name="PromoterDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<PromoterNetWorthDetailsDTO>> AddPromoterLiabilityNetWorthDetails(List<PromoterNetWorthDetailsDTO> GuarantorNetWorthDetailsDTO, CancellationToken token);

        /// <summary>
        /// Update Promoter Liability Net Worth details for enquiry submission
        /// </summary>
        /// <param name="PromoterDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<PromoterNetWorthDetailsDTO>> UpdatePromoterLiabilityNetWorthDetails(List<PromoterNetWorthDetailsDTO> PromoterDTO, CancellationToken token);

        /// <summary>
        /// Get Promoter Liability Net Worth Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<List<PromoterNetWorthDetailsDTO>> GetByIdPromoterLiabilityNetWorthDetails(int enquiryId, CancellationToken token);

        /// <summary>
        /// Remove Promoter Liability Net Worth Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<bool> DeletePromoterLiabilityNetWorthDetails(int enquiryId, CancellationToken token);
        }
    }

