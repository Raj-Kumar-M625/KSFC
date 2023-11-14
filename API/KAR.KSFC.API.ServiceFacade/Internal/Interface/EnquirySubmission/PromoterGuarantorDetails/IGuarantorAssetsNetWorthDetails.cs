using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
 

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.PromoterGuarantorDetails
{

    /// <summary>
    /// Guarantor Net Worth Details Interface
    /// </summary>
    public interface IGuarantorAssetsNetWorthDetails
    {
            /// <summary>
            /// Add Guarantor assets Net Worth details for enquiry submission
            /// </summary>
            /// <param name="GuarantorNetWorthDTO"></param>
            /// <param name="token"></param>
            /// <returns></returns>
            public Task<IEnumerable<GuarantorAssetsNetWorthDTO>> AddGuarantorAssetsNetWorthDetails(List<GuarantorAssetsNetWorthDTO> GuarantorAssetNWDTO, CancellationToken token);

        /// <summary>
        /// Update Guarantor NetWorth details for enquiry submission
        /// </summary>
        /// <param name="GuarantorNetWorthDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<GuarantorAssetsNetWorthDTO>> UpdateGuarantorAssetsNetWorthDetails(List<GuarantorAssetsNetWorthDTO> GuarantorAssetNWDTO, CancellationToken token);

            /// <summary>
            /// Get Guarantor Net Worth Details by enquiry Id
            /// </summary>
            /// <param name="enquiryId"></param>
            /// <param name="token"></param>
            /// <returns></returns>
        public Task<List<GuarantorAssetsNetWorthDTO>> GetByIdGuarantorAssetsNetWorthDetails(int enquiryId, CancellationToken token);

            /// <summary>
            /// Remove Guarantor Net Worth Details by enquiry Id
            /// </summary>
            /// <param name="enquiryId"></param>
            /// <param name="token"></param>
            /// <returns></returns>
            public Task<bool> DeleteGuarantorAssetsNetWorthDetails(int enquiryId, CancellationToken token);
        }
    }

