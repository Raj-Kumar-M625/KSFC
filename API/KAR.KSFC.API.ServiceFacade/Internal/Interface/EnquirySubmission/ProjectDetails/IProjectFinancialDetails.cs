using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.ProjectDetails
{
    public interface IProjectFinancialDetails
    {
        public Task<ProjectFinancialYearDetailsDTO> GetAllProjectFinancial(ProjectFinancialYearDetailsDTO projectCost, CancellationToken token);
        public Task<IEnumerable<ProjectFinancialYearDetailsDTO>> AddProjectFinancialDetails(List<ProjectFinancialYearDetailsDTO> projectCost, CancellationToken token);
        public Task<IEnumerable<ProjectFinancialYearDetailsDTO>> UpdateProjectFinancialDetails(List<ProjectFinancialYearDetailsDTO> projectFinancial, CancellationToken token);
        public Task<bool> DeleteProjectFinancialDetails(int Id, CancellationToken token);
        public Task<ProjectFinancialYearDetailsDTO> GetByIdProjectFinancialDetails(int Id, CancellationToken token);
    }
}
