using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class IdmSecurityDetailsDTO
    {
        public int? Action { get; set; }
        public string AccountNumber { get; set; }
        public string ApprovedEmpId { get; set; }
        public string DeedDesc { get; set; }
        public string PjSecNam { get; set; }
        public string PjSecDets { get; set; }
        public string DeedNo { get; set; }
        public string DeedUpload { get; set; }

       

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? ExecutionDate { get; set; }
        public int IdmDeedDetId { get; set; }
        public long? LoanAcc { get; set; }
     
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int? SecurityCd { get; set; }
        public int? SecCd { get; set; } 

        [DisplayFormat(ApplyFormatInEditMode = true,DataFormatString = "{0:n2}")]
        public decimal? SecurityValue { get; set; }
        public int? SubregistrarCd { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual SecurityMasterDTO TblSecurityRefnoMast { get; set; }
    }
}
