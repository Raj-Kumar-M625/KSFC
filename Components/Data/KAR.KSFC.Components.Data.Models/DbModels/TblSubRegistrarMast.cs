using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public class TblSubRegistrarMast 
    {
        public long SrOfficeId { get; set; }
        public int SubRegistrarCd { get; set; }
        public string? SrCode { get; set; }
        public string SrOfficeName { get; set; }
        public byte? DistCd { get; set; }
        public int? TlqCd { get; set; }
        public string SrOthDetails { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
    }
}
