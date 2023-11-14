using KAR.KSFC.Components.Common.Dto.EnquirySubmission.AssociateSisterConcernDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;

using System.Collections.Generic;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission
{
    public class EnquiryDTO
    {
        public int? EnquiryRefNo { get; set; }
        public int? EnquiryId { get; set; }
        public int? Status { get; set; }
        public string SummaryNote  { get; set; }
        public UnitDetailsDTO UnitDetails { get; set; }
        public PromoterAllDetailsDTO PromoterAllDetailsDTO { get; set; }
        public GuarantorAllDetailsDTO GuarantorAllDetailsDTO { get; set; }
        public AssociateSisterConcernDetailsDTO AssociateConcernDetails { get; set; }
        public ProjectAllDetailsDTO ProjectDetails { get; set; }
        public List<SecurityDetailsDTO> ListSecurityDetailsDto { get; set; }
        public SecurityDetailsDTO SecurityDocs { get; set; }
        public AllDDLListDTO DDLDTO { get; set; }
        public Declaration Declaration { get; set; }
        public BasicDetailsDto BasicDetails { get; set; }
        public IEnumerable<AddressDetailsDTO> AddressDetails { get; set; }
        public BankDetailsDTO BankDetails { get; set; }
        public IEnumerable<RegistrationNoDetailsDTO> RegistrationDetails { get; set; }
        public IEnumerable<PromoterDetailsDTO> PromoterDetails { get; set; }
        public IEnumerable<PromoterAssetsNetWorthDTO> PromoterAssetsDetails { get; set; }
        public IEnumerable<PromoterLiabilityDetailsDTO> PromoterLiability { get; set; }
        public IEnumerable<PromoterNetWorthDetailsDTO> PromoterNetWorth { get; set; }
        public IEnumerable<GuarantorDetailsDTO> GuarantorDetails { get; set; }
        public IEnumerable<GuarantorAssetsNetWorthDTO> GuarantorAssetsDetails { get; set; }
        public IEnumerable<GuarantorLiabilityDetailsDTO> GuarantorLiability { get; set; }
        public IEnumerable<GuarantorNetWorthDetailsDTO> GuarantorNetWorth { get; set; }
        public IEnumerable<SisterConcernDetailsDTO> SisterConcernDetails { get; set; }
        public IEnumerable<SisterConcernFinancialDetailsDTO> SisterConcernFinancialDetails { get; set; }
        public ProjectWorkingCapitalDeatailsDTO WorkingCapitalDetails { get; set; }
        public IEnumerable<ProjectCostDetailsDTO> ProjectCostDetails { get; set; }
        public IEnumerable<ProjectMeansOfFinanceDTO> ProjectMeansOfFinanceDetails { get; set; }
        public IEnumerable<ProjectFinancialYearDetailsDTO> ProjectFinancialYearDetails { get; set; }
        public IEnumerable<SecurityDetailsDTO> SecurityDetails { get; set; }
        public IEnumerable<EnqDocumentDTO> DocumentList { get; set; }
        public bool? HasAssociateSisterConcern { get; set; }
    }
}
