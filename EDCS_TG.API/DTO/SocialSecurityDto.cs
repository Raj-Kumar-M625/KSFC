namespace EDCS_TG.API.DTO
{
    public class SocialSecurityDto
    {
        public bool? GetPension { get; set; }
        public string? PensionScheme { get; set; }
        public bool? OtherPensionScheme { get; set; }
        public string? GetRation { get; set; }
        public bool? RationScheme { get; set; }
        public string? OtherRationScheme { get; set; }
        public bool? BusinessSupport { get; set; }
        public string? BusinessSupportScheme { get; set; }
        public bool? EducationSupport { get; set; }
        public string? EducationSupportScheme { get; set; }
        public bool? CasteCertificate { get; set; }
        public bool? IncomeCertificate { get; set; }
        public bool? InsuranceCertificate { get; set; }
        public string? InsuranceTypes { get; set; }
        public string? InsuranceTypesOthers { get; set; }
        public string? IdentifyDocument { get; set; }
        public string? IdentifyDocumentOthers { get; set; }
        public bool? CardType { get; set; }
        public bool? CardTypeOthers { get; set; }
        public string? ProtectionAct { get; set; }
        public DateTime? ChangeGenderNotarizedAffidavit { get; set; }
        public Guid? AvailGovernmentSchemeName { get; set; }
        public string? AvailGovernmentSchemeType { get; set; }
    }
}
