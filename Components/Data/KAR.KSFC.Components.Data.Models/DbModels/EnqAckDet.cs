using System;
using System.Collections.Generic;

#nullable disable
 
namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class EnqAckDet
    {
        public int EnqRowId { get; set; }
        public int? EnqRefNo { get; set; }
        public string EmpId { get; set; }
        public DateTime? EnqAckDate { get; set; }
       
    }
}
