using KAR.KSFC.Components.Common.Dto.IDM.CreationOfSecurityandAquisitionAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class OfficeDto
    {
        public byte OffcCd { get; set; }
        public string OffcNam { get; set; }
        public string OffcAdr1 { get; set; }
        public string OffcAdr2 { get; set; }
        public string OffcAdr3 { get; set; }
        public int? OffcPin { get; set; }
        public int? OffcTel1 { get; set; }
        public int? OffcTel2 { get; set; }
        public int? OffcTel3 { get; set; }
        public int? OffcTlx2 { get; set; }
        public int? OffcFax { get; set; }
        public byte? OffcDist { get; set; }
        public byte? OffcZone { get; set; }
        public int? OffcBmcd { get; set; }
        public string OffcIfsCd { get; set; }
        public string OffcInopbnkacNo { get; set; }
        public string OffcMailId { get; set; }
        public string OffcBmMailId { get; set; }
        public string OffcStNo { get; set; }
        public string OffcTaxNo { get; set; }
        public string OffcBsrCd { get; set; }
        public string OffcStdCd { get; set; }
        public string OffcNamKannada { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        //public virtual TblIdmIrLandDTO TblIdmIrLand { get; set; }
    }
}
