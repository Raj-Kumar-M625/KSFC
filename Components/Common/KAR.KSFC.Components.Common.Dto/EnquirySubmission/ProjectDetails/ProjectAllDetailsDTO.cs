using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails
{
    public class ProjectAllDetailsDTO
    {
        public ProjectAllDetailsDTO()
        {

        }
        public ProjectWorkingCapitalDeatailsDTO CapitalDtls { get; set; }
        public List<ProjectCostDetailsDTO> ListPrjctCost { get; set; }
        public List<ProjectMeansOfFinanceDTO> ListMeansOfFinance { get; set; }
        public List<ProjectFinancialYearDetailsDTO> ListPrevYearFinDetails { get; set; }
        [Display(Name = "Total Cost")]
        public string TotalCost { get; set; }
    }
}
