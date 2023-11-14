using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.UnitDetails
{
    public class AssetLiabalityDTO
    {
        public long PromoterCode { get; set; }
        public string PromoterName { get; set; }
        public string? LiabDesc { get; set; }
        public decimal? LiabVal { get; set; }
        public string AssettypeDets { get; set; }
        public decimal? IdmAssetValue { get; set; }

    }
}
