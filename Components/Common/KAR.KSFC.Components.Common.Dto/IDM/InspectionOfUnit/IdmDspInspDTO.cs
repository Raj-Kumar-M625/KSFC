using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit
{
    public class IdmDspInspDTO
    {
        public long DinRowID { get; set; }
        public long? LoanAcc { get; set; }
        public string? EncryptedLoanAcc { get; set; }
        public string? EncryptedLoanSub { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public string? EncryptedOffcCd { get; set; }
        public int DinNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DinDt { get; set; }
        public string DinTeam { get; set; }
        public int DinDept { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DinRdt { get; set; }
        public int? DinSeccd { get; set; }
        public Decimal DinSecAmt { get; set; }
        public int? DinSecrt { get; set; }
        public int? Dinland { get; set; }
        public int? DinLandArea { get; set; }
        public int? DinLandDev { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        
        public int? Action { get; set; }

      

    }
}
