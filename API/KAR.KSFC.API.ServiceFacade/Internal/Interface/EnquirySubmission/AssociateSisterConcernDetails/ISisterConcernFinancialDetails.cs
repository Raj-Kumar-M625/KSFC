using KAR.KSFC.Components.Common.Dto.EnquirySubmission.AssociateSisterConcernDetails;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.AssociateSisterConcernDetails
{
    /// <summary>
    /// Associate Sister Concern Details Interface
    /// </summary>
    public interface ISisterConcernFinancialDetails
    {
        /// <summary>
        /// Add Associate Sister Concern Financial details for enquiry submission
        /// </summary>
        /// <param name="SisterConcernFinancialDetailsDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<SisterConcernFinancialDetailsDTO>> AddSisterConcernFinancialDetails(List<SisterConcernFinancialDetailsDTO> SisterConcernDTO, CancellationToken token);

        /// <summary>
        /// Update Sister Concern Financial Details details for enquiry submission
        /// </summary>
        /// <param name="SisterConcernFinancialDetailsDTO"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<SisterConcernFinancialDetailsDTO>> UpdateSisterConcernFinancialDetails(List<SisterConcernFinancialDetailsDTO> SisterConcernDTO, CancellationToken token);

        /// <summary>
        /// Get Sister Concern Financial Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<List<SisterConcernFinancialDetailsDTO>> GetByIdSisterConcernFinancialDetails(int enquiryId, CancellationToken token);

        /// <summary>
        /// Remove Sister Concern Financial Details by enquiry Id
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<bool> DeleteSisterConcernFinancialDetails(int enquiryId, CancellationToken token);

        /// <summary>
        /// Get Sister Concern Financial Details by sisterId 
        /// </summary>
        /// <param name="enquiryId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<SisterConcernFinancialDetailsDTO> GetBySisterIdConcernFinancialDetails(int sisterId, CancellationToken token);
    }
    }

