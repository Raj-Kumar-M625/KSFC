using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.ProjectDetails
{
    /// <summary>
    /// Working Capital Concern Details Interface
    /// </summary>
    public interface IWorkingCapitalDetails
    {
        /// <summary>
        /// Add Working Capital Details  for enquiry submission
        /// </summary>
        /// <param name="ProjectWorkingCapitalDeatailsDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<ProjectWorkingCapitalDeatailsDTO> AddWorkingCapitalDetails(ProjectWorkingCapitalDeatailsDTO WorkingCapitalDTO, CancellationToken token);

        /// <summary>
        /// Update Project Working Capital Deatails  for enquiry submission
        /// </summary>
        /// <param name="ProjectWorkingCapitalDeatailsDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<ProjectWorkingCapitalDeatailsDTO> UpdateWorkingCapitalDetails(ProjectWorkingCapitalDeatailsDTO WorkingCapitalDTO, CancellationToken token);

        /// <summary>
        /// Get Working Capital Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<ProjectWorkingCapitalDeatailsDTO> GetByIdWorkingCapitalDetails(int enquiryId, CancellationToken token);

        /// <summary>
        /// Remove Working Capital Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<bool> DeleteWorkingCapitalDetails(int enquiryId, CancellationToken token);
    }
}

