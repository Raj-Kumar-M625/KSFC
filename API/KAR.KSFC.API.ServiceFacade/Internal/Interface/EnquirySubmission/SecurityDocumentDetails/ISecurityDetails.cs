using KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.SecurityDocumentDetails
{

    /// <summary>
    /// ISecurity  Details Interface
    /// </summary>
    public interface ISecurityDetails
    {
        /// <summary>
        /// Add Security Details for enquiry submission
        /// </summary>
        /// <param name="SecurityDetailsDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<SecurityDetailsDTO>> AddSecurityDetails(List<SecurityDetailsDTO> SecurityDetailsDTO, CancellationToken token);

        /// <summary>
        /// Update Security details for enquiry submission
        /// </summary>
        /// <param name="SecurityDetailsDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<SecurityDetailsDTO>> UpdateSecurityDetails(List<SecurityDetailsDTO> SecurityDetailsDTO, CancellationToken token);

        /// <summary>
        /// Get Security Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<List<SecurityDetailsDTO>> GetByIdSecurityDetails(int enquiryId, CancellationToken token);

        /// <summary>
        /// Remove Security Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<bool> DeleteSecurityDetails(int enquiryId, CancellationToken token);
    }

}
