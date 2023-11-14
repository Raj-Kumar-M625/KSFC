using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class EntityWorkFlowDetail : BaseSearchCriteria
    {
        public long Id { get; set; }

        public long EntityId { get; set; }

        public string EntityName { get; set; }

        public string EntityNumber { get; set; }

        public long EmployeeId { get; set; }

        public string EmployeeCode { get; set; }

        public string EmployeeName { get; set; }

        public int Sequence { get; set; }
        public string TypeName { get; set; }
        public string SeasonName { get; set; }
        public string Phase { get; set; }

        public DateTime PlannedFromDate { get; set; }

        public DateTime PlannedEndDate { get; set; }

        public DateTime? CompletedOn { get; set; }

        public bool IsComplete { get; set; }

        public string Status { get; set; }

        public string PhaseCompleteStatus { get; set; }
        
        public string HQCode { get; set; }

        public bool EmployeeIsActive { get; set; }

        public bool EmployeeIsActiveInSap { get; set; }

        // Added on 19.4.19
        public bool IsStarted { get; set; }
        public Nullable<System.DateTime> WorkFlowDate { get; set; }
        public string MaterialType { get; set; }
        public int MaterialQuantity { get; set; }
        public bool GapFillingRequired { get; set; }
        public int GapFillingSeedQuantity { get; set; }
        public int LaborCount { get; set; }
        public int PercentCompleted { get; set; }

        // April 11 2020 - PJM
        public string BatchNumber { get; set; }
        public string LandSize { get; set; }
        public long ItemCount { get; set; }
        public string DWSEntry { get; set; }
        public long ItemsUsedCount { get; set; }
        public long YieldExpected { get; set; }
        public long BagsIssued { get; set; }
        public DateTime HarvestDate { get; set; }

        public string Agreement { get; set; }
        public string UniqueId { get; set; }

        public long AgreementId { get; set; }
        public bool IsFollowUpRow { get; set; }

        public long ActivityId { get; set; }

        public bool IsVisibleOnSearch { get; set; } = true;

        public bool IsActive { get; set; }
        public string Notes { get; set; }

        public string AgreementStatus { get; set; }
    }
}
