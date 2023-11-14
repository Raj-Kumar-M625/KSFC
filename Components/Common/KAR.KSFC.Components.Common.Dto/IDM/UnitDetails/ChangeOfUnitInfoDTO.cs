using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.UnitDetails
{
    public class ChangeOfUnitInfoDTO
    {
        public AllDDLListDTO DDLDTO { get; set; }
        public List<AddressCdTabDTO> AddressCdTab { get; set; }
        public List<AppUnitDetailDTO> AppUnitDetail { get; set; }
        public List<DistCdTabDTO> DistCdTab { get; set; }
        public List<HobCdtabDTO> HobCdTab { get; set; }
        public List<IdmChangeBankDetailsDTO> ChangeBankDetails { get; set; }
        public List<IdmPromAddressDTO> PromoterAddress { get; set; }
        public List<IdmPromAssetDetDTO> PromoterAsset { get; set; }
        public List<IdmPromoterBankDetailsDTO> PromoterBankDetails { get; set; }
        public List<IdmPromoterDTO> PromoterDetails { get; set; }
        public List<IdmUnitAddressDTO> AddressList { get; set; }
        public IdmUnitDetailDTO UnitDetails { get; set; }
        public List<IdmUnitProductsDTO> UnitProducts { get; set; }
        public List<PromoterQualDTO> PromoterQualifications { get; set; }
        public List<PromoterSubClasDTO> PromoterSubClass { get; set; }
        public List<TblAcTypeCdtabDTO> AccountType { get; set; }
        public List<TblAssetCatCDTabDTO> AssetCategory { get; set; }
        public List<TblIndCdtabDTO> UnitIndustry { get; set; }
        public List<TblProdCdtabDTO> ProductCdtab { get; set; }
        public List<TblPromoterLiabDetDTO> PromoterLiability { get; set; }
        public List<TblPromoterNetWortDTO> PromoterNetWorth { get; set; }
        public IdmDchgWorkingCapitalDTO WorkingCapitalDetails { get; set; }
        public List<TlqCdTabDTO> TalukCd { get; set; }
        public List<VilCdTabDTO> VillageCd { get; set; }

        public List<SelectListItem> AllPositionDesignation { get; set; }
        public List<SelectListItem> AllDomicileStatus { get; set; }
        public List<SelectListItem> AllPromotorClass { get; set; }
        public List<SelectListItem> AllPromotorSubClass { get; set; }
        public List<SelectListItem> AllPromotorQual { get; set; }
        public List<SelectListItem> AllAccountType { get; set; }
        public List<SelectListItem> allProducdetailsMaster { get; set; }
        public List<SelectListItem> allindustrydetailsMaster { get; set; }
        public List<SelectListItem> AllAssetTypeMaster { get; set; }
        public List<SelectListItem> AllAssetCategoryMaster { get; set; }

        // from US#05
        //public IdmDchgWorkingCapitalDTO WorkingCapitalDetails { get; set; }

        public List<IdmDchgMeansOfFinanceDTO> MeansOfFinanceDetails { get; set; }

        public List<IdmDchgProjectCostDTO> ProjectCostDetail { get; set; }

        //public List<IdmDsbLetterOfCreditDTO> LetterOfCreditList { get; set; }

        public List<TblIdmDhcgAllcDTO> LoanAllocationDetails { get; set; }
        public List<IdmDchgBuildingDetDTO> BuildingInspectionDetails { get; set; }
        public List<IdmDchgLandDetDTO> LandInspectionDetails { get; set; }
        public List<IdmDChgFurnDTO> FurnitureInspectionDetails { get; set; }
        public List<IdmDchgIndigenousInspectionDTO> IndigenousMachineryDetails { get; set; }
        public List<IdmDchgImportMachineryDTO> ImportedMachineryDetails { get; set; }
        
        public List<TblIdmProjLandDTO> ProjectLandDetails { get; set; }
        
    }
}
