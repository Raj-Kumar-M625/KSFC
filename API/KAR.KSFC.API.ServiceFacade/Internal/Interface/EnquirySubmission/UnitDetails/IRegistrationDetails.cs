using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

 

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.UnitDetails.EnquirySubmission
{
    /// <summary>
    /// Registration Details Interface
    /// </summary>
    public interface IRegistrationDetails
    {
        /// <summary>
        /// Add Registration details for enquiry submission
        /// </summary>
        /// <param name="bankDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<RegistrationNoDetailsDTO>> AddRegistrationDetails(List<RegistrationNoDetailsDTO> registrationDTO, CancellationToken token);

        /// <summary>
        /// Update Registration details for enquiry submission
        /// </summary>
        /// <param name="addressDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<RegistrationNoDetailsDTO>> UpdateRegistrationDetails(List<RegistrationNoDetailsDTO> registrationDTO, CancellationToken token);

        /// <summary>
        /// Get All Registration Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<RegistrationNoDetailsDTO>> GetRegistrationDetailsByEnquiryId(int enquiryId, CancellationToken token);

        /// <summary>
        /// Get Registration Details by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public Task<RegistrationNoDetailsDTO> GetRegistrationNoDetailsById(int Id, CancellationToken token);

        /// <summary>
        /// Remove Registration Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<bool>DeleteRegistrationDetails(int enquiryId, CancellationToken token);
    }
}
