using KAR.KSFC.Components.Common.Dto.IDM.CreationOfDisbursmentProposal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit
{
    public class InspectionAccountDTO : InspectionOfUnitDTO
    {
        public List<IdmDchgLandDetDTO> LandInspectionDetails { get; set; }
        public List<IdmDchgBuildingDetDTO> BuildingInspectionDetails { get; set; }
        public List<IdmBuildingMaterialSiteInspectionDTO> BuildMatSiteInspectionDetails { get; set; }
        public List<IdmDchgIndigenousInspectionDTO> IndigenousMachineryInspectionDetails { get; set; }
        public List<IdmDchgImportMachineryDTO> ImportMachineryInspection { get; set; }
        public List<IdmDChgFurnDTO> FurnitureInspectionDetails { get; set; }
       // public IdmDchgWorkingCapitalDTO WorkingCapitalDetails { get; set; }
        public List<IdmDsbdetsDTO> idmDsbdets { get; set; }
        public List<IdmDsbStatImpDTO>StatusofImplementationDetails { get; set;}
       
    }
}
