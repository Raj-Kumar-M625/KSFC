using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblEnqBankDet
    {
        public int EnqBankId { get; set; }
        public int? EnqtempId { get; set; }
        public string EnqAcctype { get; set; }
        public string EnqBankaccno { get; set; }
        public string EnqIfsc { get; set; }
        public string EnqAccName { get; set; }
        public string EnqBankname { get; set; }
        public string EnqBankbr { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblEnqTemptab Enqtemp { get; set; }
    }
}
