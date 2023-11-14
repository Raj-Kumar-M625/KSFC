using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class IdmDDLListDTO
    {
        public List<SelectListItem> AllSecurityCategory { get; set; }
        public List<SelectListItem> AllIfscCode { get; set; }
        public List<SelectListItem> AllChargeType { get; set; }
        public List<SelectListItem> AllSecurityTypes { get; set; }
        public List<SelectListItem> AllSubRegistrarOffice { get; set; }
        public List<SelectListItem> AllAssetTypes { get; set; }
        public List<SelectListItem> AllConditionTypes { get; set; }
        public List<SelectListItem> AllConditionStages { get; set; }
        public List<SelectListItem> AllConditionDescriptions { get; set; }
        public List<SelectListItem> AllConditionStagesMaster { get; set; }
        public List<SelectListItem> AllForm8andForm13Master { get; set; }
        public List<SelectListItem> AllDistricts { get; set; }
        public List<SelectListItem> AllPositionDesignation { get; set; }  // By Dev on 01/09/2022
        public List<SelectListItem> AllDomicileStatus { get; set; }  // By Dev on 01/09/2022
        public List<SelectListItem> AllPromotorClass { get; set; }  // By Dev on 01/09/2022
                                                          
        #region Promoter Address 
        public List<SelectListItem> AllPromoterDistricts { get; set; }
        public List<SelectListItem> AllPromoterState { get; set; }
        public List<SelectListItem> AllPromoterNames { get; set; }
        public List<SelectListItem> AllPromoterPincode { get; set; }

        #endregion
        #region  Unit Information Address Details 
        public List<SelectListItem> AllDistrictDetails { get; set; }
        public List<SelectListItem> AllTalukDetails { get; set; }
        public List<SelectListItem> AllHobliDetails { get; set; }
        public List<SelectListItem> AllVillageDetails { get; set; }
        public List<SelectListItem> AllPincodeDetails { get; set; }
        #endregion
        public List<SelectListItem> AllPromotorSubClass { get; set; } // By Dev on 03/09/2022
        public List<SelectListItem> AllPromotorQual { get; set; } // By Dev on 03/09/2022
        public List<SelectListItem> AllAccountType { get; set; } // By Dev on 06/09/2022
      

        public List<SelectListItem> allProducdetailsMaster { get; set; }

        public List<SelectListItem> allindustrydetailsMaster { get; set; }

        public List<SelectListItem> AllAssetTypeMaster { get; set; }

        public List<SelectListItem> AllAssetCategoryMaster { get; set; }

        public List<SelectListItem> AllStateZone { get; set; }
        public List<SelectListItem> AllProjectCostComponents { get; set; }
        
         public List<SelectListItem> AllOtherDebitsDetails { get; set; }

        #region Loan Allocation
        public List<SelectListItem> GetAllAllocationCode { get; set; }
        #endregion
        //public AllDDLListDTO DDLDTO { get; set; }

        #region Creation of Security and Acquisition of Assets
        public List<SelectListItem> AllLandType { get; set; } // By Dev on 01/10/2022
        #endregion
        public List<SelectListItem> AllUmoMasterDetails { get; set; }
        public List<SelectListItem> AllConstutionDetails { get; set; }

        public List<SelectListItem> AllFinanceCategory { get; set; }
        public List<SelectListItem> AllDeptMaster { get; set; }
        public List<SelectListItem> AllDsbChargeMap { get; set; }


    }
}

