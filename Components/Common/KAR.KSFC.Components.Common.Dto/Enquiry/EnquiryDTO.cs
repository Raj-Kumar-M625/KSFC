namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class EnquiryDTO
    {
        public UnitDetailsDTO UnitDetails { get; set; }
        public PromoterAllDetailsDTO PromoterAllDetailsDTO { get; set; }
        public GuarantorAllDetailsDTO GuarantorAllDetailsDTO { get; set; }
        public AssociateSisterConcernDetailsDTO AssociateConcernDetails { get; set; }
        public ProjectDetailsDTO ProjectDetails { get; set; }
        public SecurityDocumentsDTO SecurityDocs { get; set; }
        public DDLListDTO DDLDTO { get; set; }
        public int EnquiryId { get; set; }
    }
}
