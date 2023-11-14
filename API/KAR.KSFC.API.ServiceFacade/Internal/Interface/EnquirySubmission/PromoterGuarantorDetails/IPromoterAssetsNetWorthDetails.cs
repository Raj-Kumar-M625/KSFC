using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
 

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.PromoterGuarantorDetails
{

    /// <summary>
    /// Promoter Net Worth Details Interface
    /// </summary>
    public interface IPromoterAssetsNetWorthDetails
        {
            /// <summary>
            /// Add Promoter Net Worth details for enquiry submission
            /// </summary>
            /// <param name="PromoterNetWorthDTO"></param>
            /// <param name="token"></param>
            /// <returns></returns>
            public Task<IEnumerable<PromoterAssetsNetWorthDTO>> AddPromoterAssetsNetWorthDetails(List<PromoterAssetsNetWorthDTO> AssetsNetWorthDTO, CancellationToken token);

        /// <summary>
        /// Update Promoter NetWorth details for enquiry submission
        /// </summary>
        /// <param name="PromoterNetWorthDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<PromoterAssetsNetWorthDTO>> UpdatePromoterAssetsNetWorthDetails(List<PromoterAssetsNetWorthDTO> AssetsNetWorthDTO, CancellationToken token);

            /// <summary>
            /// Get Promoter Net Worth Details by enquiry Id
            /// </summary>
            /// <param name="enquiryId"></param>
            /// <param name="token"></param>
            /// <returns></returns>
        public Task<List<PromoterAssetsNetWorthDTO>> GetByIdPromoterAssetsNetWorthDetails(int enquiryId, CancellationToken token);

            /// <summary>
            /// Remove Promoter Net Worth Details by enquiry Id
            /// </summary>
            /// <param name="enquiryId"></param>
            /// <param name="token"></param>
            /// <returns></returns>
            public Task<bool> DeletePromoterAssetsNetWorthDetails(int enquiryId, CancellationToken token);
        }
    }

