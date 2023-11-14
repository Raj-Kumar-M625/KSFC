using KAR.KSFC.Components.Common.Dto.EnquirySubmission.AssociateSisterConcernDetails;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.AssociateSisterConcernDetails
{
    /// <summary>
    /// Associate Sister Concern Details Interface
    /// </summary>
    public interface ISisterConcernDetails
        {
        /// <summary>
        /// Add Associate Sister Concern details for enquiry submission
        /// </summary>
        /// <param name="SisterConcernDetailsDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<SisterConcernDetailsDTO>> AddSisterConcernDetails(List<SisterConcernDetailsDTO> SisterConcernDTO, CancellationToken token);

        /// <summary>
        /// Update Sister Concern Details details for enquiry submission
        /// </summary>
        /// <param name="SisterConcernDetailsDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<SisterConcernDetailsDTO>> UpdateSisterConcernDetails(List<SisterConcernDetailsDTO> SisterConcernDTO, CancellationToken token);

        /// <summary>
        /// Get Sister Concern Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<List<SisterConcernDetailsDTO>> GetByIdSisterConcernDetails(int enquiryId, CancellationToken token);

        /// <summary>
        /// Remove Sister Concern Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<bool> DeleteSisterConcernDetails(int EnqSisId, CancellationToken token);
        public Task<bool> DeleteSisterConcernDetailsByEnqId(int EnqtempId, CancellationToken token);
        }
    }

