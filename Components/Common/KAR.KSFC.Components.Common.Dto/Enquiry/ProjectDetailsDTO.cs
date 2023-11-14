using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class ProjectDetailsDTO
    {
        public CapitalArrgmentDtlsDTO CapitalDtls { get; set; }
        public List<ProjectCostDTO> ListPrjctCost { get; set; }
        public List<MeansOfFinanceDTO> ListMeansOfFinance { get; set; }
        public List<ProjectPrevFYDetailsDTO> ListPrevYearFinDetails { get; set; }
        [Display(Name = "Total Cost")]
        public string TotalCost { get; set; }
    }
}
