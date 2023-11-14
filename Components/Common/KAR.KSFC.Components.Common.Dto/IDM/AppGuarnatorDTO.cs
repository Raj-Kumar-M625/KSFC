using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class AppGuarnatorDTO
    {
        public int AppGuarId { get; set; }
        public long? PromoterCode { get; set; }
        public long? EgNo { get; set; }
        public int? OffcCd { get; set; }
        public int? UtCd { get; set; }
        public string UtName { get; set; }
        public string GuarName { get; set; }
        public string GuarGender { get; set; }
        public DateTime? GuarDob { get; set; }
        public decimal? GuarAge { get; set; }
        public string NameFatherSpouse { get; set; }
        public int? PclasCd { get; set; }
        public int? PsubclasCd { get; set; }
        public int? DomCd { get; set; }
        public string GuarPassport { get; set; }
        public string GuarPan { get; set; }
        public decimal? GuarMobileNo { get; set; }
        public string GuarEmail { get; set; }
        public decimal? PromTelNo { get; set; }
        public string PromPhoto { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
