namespace EDCS_TG.API.DTO.DBT
{
    public class DBTRegisterRequestDto
    {
        public string? DeptCode { get; set; }
        public string? ApplnCode { get; set; }
        public string? SchemeCode { get; set; }
        public string? BeneficiaryID { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? IntegrationKey { get; set; }
        public string? IntegrationPassword { get; set; }
        public string? TxnNo { get; set; }
        public string? TxnDateTime { get; set; }
        public string? ServiceCode { get; set; }
        public string? ResponseRedirectURL { get; set; }
    }
}
