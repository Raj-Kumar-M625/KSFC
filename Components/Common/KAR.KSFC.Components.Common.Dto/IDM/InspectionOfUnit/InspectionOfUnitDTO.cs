using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit
{
    public class InspectionOfUnitDTO 
    {
        public List<SelectListItem> ProjectCostComponents { get; set; }
        public List<IdmDspInspDTO> InspectionDetail { get; set; }
        //public List<IdmDchgMeansOfFinanceDTO> MeansOfFinanceDetails { get; set; }
        public List<IdmDsbLetterOfCreditDTO> LetterOfCreditList { get; set; }
        public List<IdmDsbStatImpDTO> StatusofImplementationDetails { get; set; }
        //public List<IdmDchgProjectCostDTO> ProjectCostDetail { get; set; }

        //public IdmDchgWorkingCapitalDTO WorkingCapitalDetails { get; set; }
    }
}
