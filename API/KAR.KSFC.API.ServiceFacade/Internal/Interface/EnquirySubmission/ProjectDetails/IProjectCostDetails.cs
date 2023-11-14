using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.ProjectDetails
{
    public interface IProjectCostDetails
    {
        public Task<ProjectCostDetailsDTO> GetAllProjectCosts(CancellationToken token);
        public Task<List<ProjectCostDetailsDTO>> AddProjectCostDetails(List<ProjectCostDetailsDTO> projectCost, CancellationToken token);
        public Task<bool> UpdateProjectCostDetails(ProjectCostDetailsDTO projectCost, CancellationToken token);
        public Task<bool> DeleteProjectCostDetails(int Id, CancellationToken token);
        public Task<ProjectCostDetailsDTO> GetByIdProjectCostDetails(int Id, CancellationToken token);
    }
}
