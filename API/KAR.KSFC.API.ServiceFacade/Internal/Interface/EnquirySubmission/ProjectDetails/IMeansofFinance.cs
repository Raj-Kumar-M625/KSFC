using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.ProjectDetails
{
    public interface IMeansofFinance
    {
        public Task<ProjectMeansOfFinanceDTO> GetAllMeansOfFinance(ProjectMeansOfFinanceDTO ProjectMeansOfFinance, CancellationToken token);
        public Task<IEnumerable<ProjectMeansOfFinanceDTO>> AddMeansOfFinance(List<ProjectMeansOfFinanceDTO> ProjectMeansOfFinance, CancellationToken token);
        public Task<IEnumerable<ProjectMeansOfFinanceDTO>> UpdateMeansOfFinance(List<ProjectMeansOfFinanceDTO> ProjectMeansOfFinance, CancellationToken token);
        public Task<bool> DeleteMeansOfFinance(int Id, CancellationToken token);
        public Task<ProjectMeansOfFinanceDTO> GetByIdMeansOfFinance(int Id, CancellationToken token);
    }
}
