using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.PromoterGuarantorDetails
{
    /// <summary>
    /// Promoter Details Interface
    /// </summary>
    public interface IPromoterDetails
        {
            /// <summary>
            /// Add Promoter details for enquiry submission
            /// </summary>
            /// <param name="PromoterDTO"></param>
            /// <param name="token"></param>
            /// <returns></returns>
            public Task<IEnumerable<PromoterDetailsDTO>> AddPromoterDetails(List<PromoterDetailsDTO> PromoterDTO, CancellationToken token);

            /// <summary>
            /// Update Promoter details for enquiry submission
            /// </summary>
            /// <param name="PromoterDTO"></param>
            /// <param name="token"></param>
            /// <returns></returns>
            public Task<IEnumerable<PromoterDetailsDTO>> UpdatePromoterDetails(List<PromoterDetailsDTO> PromoterDTO, CancellationToken token);

            /// <summary>
            /// Get Promoter Details by enquiry Id
            /// </summary>
            /// <param name="enquiryId"></param>
            /// <param name="token"></param>
            /// <returns></returns>
            public Task<IEnumerable<PromoterDetailsDTO>> GetByIdPromoterDetails(int enquiryId, CancellationToken token);

            /// <summary>
            /// Remove Promoter Details by enquiry Id
            /// </summary>
            /// <param name="enquiryId"></param>
            /// <param name="token"></param>
            /// <returns></returns>
            public Task<bool> DeletePromoterDetails(int EnqPromId, CancellationToken token);
         
        }
    }

