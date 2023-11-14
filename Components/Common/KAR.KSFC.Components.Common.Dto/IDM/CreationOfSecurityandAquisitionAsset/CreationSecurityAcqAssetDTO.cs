using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.CreationOfSecurityandAquisitionAsset
{
    public class CreationSecurityAcqAssetDTO
    {
        public List<TblIdmIrFurnDTO> FurnitureAcqDetails { get; set; }
        public List<IdmIrPlmcDTO> MachineryAcqDetails { get; set; }
        public List<TblIdmBuildingAcquisitionDetailsDTO> BuildingAcqDetails { get; set; }
        public List<TblIdmIrLandDTO> LandAcqDetails { get; set; }
        public List<TblLandTypeMastDTO> LandMaster { get; set; }
        public List<SelectListItem> AllLandType { get; set; }
    }
}
