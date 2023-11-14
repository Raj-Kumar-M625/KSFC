using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public class TblIdmDsbFm813
    {
        public long DF813Id { get; set; }
        public byte? DF813Offc { get; set; }
        public long? DF813Unit { get; set; }
        public int? DF813Sno { get; set; }
        public DateTime? DF813Dt { get; set; }
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

        public virtual Tblfm8fm13CDTab Tblfm8fm13CDTab { get; set; }

    }
}
