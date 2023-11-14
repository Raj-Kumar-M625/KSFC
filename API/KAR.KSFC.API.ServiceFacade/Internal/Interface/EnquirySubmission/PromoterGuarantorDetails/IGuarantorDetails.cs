using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.PromoterGuarantorDetails
{
    /// <summary>
    /// Guarantor Details Interface
    /// </summary>
    public interface IGuarantorDetails
    {
            /// <summary>
            /// Add Guarantor details for enquiry submission
            /// </summary>
            /// <param name="GuarantorDTO"></param>
            /// <param name="token"></param>
            /// <returns></returns>
            public Task<IEnumerable<GuarantorDetailsDTO>> AddGuarantorDetails(List<GuarantorDetailsDTO> GuarantorDTO, CancellationToken token);

            /// <summary>
            /// Update Guarantor details for enquiry submission
            /// </summary>
            /// <param name="GuarantorDTO"></param>
            /// <param name="token"></param>
            /// <returns></returns>
            public Task<IEnumerable<GuarantorDetailsDTO>> UpdateGuarantorDetails(List<GuarantorDetailsDTO> GuarantorDTO, CancellationToken token);

            /// <summary>
            /// Get Guarantor Details by enquiry Id
            /// </summary>
            /// <param name="enquiryId"></param>
            /// <param name="token"></param>
            /// <returns></returns>
            public Task<List<GuarantorDetailsDTO>> GetByIdGuarantorDetails(int enquiryId, CancellationToken token);

            /// <summary>
            /// Remove Guarantor Details by enquiry Id
            /// </summary>
            /// <param name="enquiryId"></param>
            /// <param name="token"></param>
            /// <returns></returns>
            public Task<bool> DeleteGuarantorDetails(int EnqGuarId, CancellationToken token);
        }
    }

