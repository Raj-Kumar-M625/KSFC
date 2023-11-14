using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.Disbursement
{
    public class VerificationOfDisbursementDTO
    {
        public List<SelectListItem> CondType { get; set; }
        public List<LDConditionDetailsDTO> DisbursementDetails { get; set; }
        public List<AdditionConditionDetailsDTO> AdditionalCondition { get; set; }
        public List<Form8AndForm13DTO> Form8AndForm13List { get; set; }
        public IdmSidbiApprovalDTO SidbiApproval { get; set; }
        public IdmFirstInvestmentClauseDTO FirstInvestmentClause { get; set; }
        public List<RelaxationDTO> OtherRelaxation { get; set; }
    }
}
