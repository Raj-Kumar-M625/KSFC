using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.Disbursement
{
    public class Form8AndForm13DTO
    {
        public int? SlNo { get; set; }
        public int? Action { get; set; }

        public string? FormType { get; set; }
        public long DF813Id { get; set; }
        public byte? DF813Offc { get; set; }
        public long? DF813Unit { get; set; }
        public int? DF813Sno { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DF813Dt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DF813RqDt { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string DF813Ref { get; set; }
        public string DF813cc { get; set; }
        public int? DF813t1 { get; set; }
        public int? DF813a1 { get; set; }
        public string DF813Upload { get; set; }
        public long? DF813LoanAcc { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblFm8Fm13CdTabDTO Tblfm8fm13CDTab { get; set; }

    }
}
