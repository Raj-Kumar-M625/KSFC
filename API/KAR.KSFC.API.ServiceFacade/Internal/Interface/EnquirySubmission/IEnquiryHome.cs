using KAR.KSFC.Components.Common.Dto.EnquirySubmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission
{
    public interface IEnquiryHomeService
    {
        /// <summary>
        /// Remove Enquiry Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<bool> DeleteEnquiryAsync(int enquiryId, CancellationToken token);

        /// <summary>
        /// Get All Enquiry Details
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<EnquirySummary>> GetAllEnquiryAsync( CancellationToken token);

        /// <summary>
        /// Get By Id Enquiry Details
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<EnquiryDTO> GetEnquiryByIdAsync(int Id, CancellationToken token);

        /// <summary>
        /// Submit enquiry
        /// </summary>
        /// <param name="summary"></param>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<int> SubmitEnquiry(string summary, int enquiryId, CancellationToken token);

        /// <summary>
        /// Add New Equiry
        /// </summary>
        /// <param name="pan"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<int> AddNewEnqiry(string pan, CancellationToken token);
        /// <summary>
        /// Get All Enquiry For Admin
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<EnquirySummary>> GetAllEnquiriesForAdminAsync(CancellationToken token);
        Task<bool> UpdateEnquiryStatus(int enquiryId, int EnqStatus, CancellationToken token);
        Task<int> UpdateEnquiryAssociateSisterConcernStatus(int enquiryId, bool Status, CancellationToken token);
    }
}
