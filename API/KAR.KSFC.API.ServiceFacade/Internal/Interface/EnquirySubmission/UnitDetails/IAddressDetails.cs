using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.UnitDetails.EnquirySubmission
{
    /// <summary>
    /// Address Details Interface
    /// </summary>
    public interface IAddressDetails
    {
        /// <summary>
        /// Add address details for enquiry submission
        /// </summary>
        /// <param name="addressDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<AddressDetailsDTO>> AddAddressDetails(List<AddressDetailsDTO> addressDTO, CancellationToken token);

        /// <summary>
        /// Update address details for enquiry submission
        /// </summary>
        /// <param name="addressDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<AddressDetailsDTO>> UpdateAddressDetails(List<AddressDetailsDTO> addressDTO, CancellationToken token);

        /// <summary>
        /// Get address Details by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<AddressDetailsDTO> GetByIdAddressDetails(int id, CancellationToken token);

        /// <summary>
        /// GetAddress Details By Enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<List<AddressDetailsDTO>> GetAddressDetailsByEnquiryId(int enquiryId, CancellationToken token);

        /// <summary>
        /// Remove address Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<bool>DeleteAddressDetails(int enquiryId, CancellationToken token);
    }
}
