using System.Threading;
using System.Threading.Tasks;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.UnitDetails.EnquirySubmission
{
    /// <summary>
    /// Basic Details Interface
    /// </summary>
    public interface IBasicDetails
    {
        /// <summary>
        /// Add basic details for enquiry submission
        /// </summary>
        /// <param name="basicDetailsDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<BasicDetailsDto> AddBasicDetails(BasicDetailsDto basicDetailsDTO, CancellationToken token);

        /// <summary>
        /// Update basic details for enquiry submission
        /// </summary>
        /// <param name="basicDetailsDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<BasicDetailsDto> UpdateBasicDetails(BasicDetailsDto basicDetailsDTO, CancellationToken token);

        /// <summary>
        /// Get Basic Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<BasicDetailsDto> GetBasicDetails(int enquiryId, CancellationToken token);

        /// <summary>
        /// Remove Basic Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<bool>DeleteBasicDetails(int enquiryId, CancellationToken token);
    }
}
