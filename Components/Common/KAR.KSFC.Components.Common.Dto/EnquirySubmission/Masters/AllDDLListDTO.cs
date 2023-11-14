using KAR.KSFC.Components.Common.Dto.Common;

using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class AllDDLListDTO
    {
        public List<SelectListItem> ListBranch { get; set; } //required//KSFC Branch
        public List<SelectListItem> ListLoanPurpose { get; set; }
        public List<SelectListItem> ListFirmSize { get; set; }
        public List<SelectListItem> ListProduct { get; set; }
        public List<SelectListItem> ListDistrict { get; set; } //required
        public List<SelectListItem> ListTaluka { get; set; }//required
        public List<SelectListItem> ListHobli { get; set; }//required
        public List<SelectListItem> ListVillage { get; set; }//required
        public List<SelectListItem> ListPremises { get; set; }
        public List<SelectListItem> ListRegnType { get; set; }
        public List<SelectListItem> ListPromDesgnType { get; set; }
        public List<SelectListItem> ListDomicileStatus { get; set; }
        public List<SelectListItem> ListAssetCategory { get; set; }
        public List<SelectListItem> ListAssetType { get; set; }
        public List<SelectListItem> ListAcquireMode { get; set; }//required  mode of acquire asset
        public List<SelectListItem> ListFacility { get; set; }
        public List<SelectListItem> ListFY { get; set; }
        public List<SelectListItem> ListFinancialComponent { get; set; }
        public List<SelectListItem> ListProjectCost { get; set; }
        public List<SelectListItem> ListMeansOfFinanceCategory { get; set; }
        public List<SelectListItem> ListMeansOfFinanceType { get; set; }
        public List<SelectListItem> ListSecurityType { get; set; }
        public List<SelectListItem> ListSecurityDet { get; set; }
        public List<SelectListItem> ListSecurityRelation { get; set; }
        public List<SelectListItem> ListIndustryType { get; set; }
        public List<SelectListItem> ListPromotorClass { get; set; }
        
    }

}
