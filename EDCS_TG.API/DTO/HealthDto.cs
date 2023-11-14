namespace EDCS_TG.API.DTO
{
    public class HealthDto
    {
        public string? CommonHealthIssues { get; set; }
        public string? CommonHealthIssuesOthers { get; set; }
        public string? SufferingHealthIssues { get; set; }
        public string? SufferingHealthIssuesOthers { get; set; }
        public string? Hospital { get; set; }
        public string? TreatmentIssue { get; set; }
        public bool? AffirmationSurgery { get; set; }
        public string? PlaceofSurgery { get; set; }
        public string? WhichHospitalSurgery { get; set; }
        public bool? CostDetailsOfGas { get; set; }
        public bool? SuccessfulSurgery { get; set; }
        public bool? SurgeryReDone { get; set; }
        public bool? ReaminingSurgery { get; set; }
        public string? HormoneReplaceTherapy { get; set; }
        public string? HormoneReplacementTherapy { get; set; }
        public bool? CostPerMonth { get; set; }
        public bool? SideEffectOfHRT { get; set; }
        public string? SideEffectOfHRTOthers { get; set; }
        public string? ConversionTerapy { get; set; }
        public bool? MonthlyMedicalExpenses { get; set; }
        public string? AnyDisability { get; set; }
        public string? IfYesWhatKind { get; set; }
        public string? OtherDisability { get; set; }
        public string? PhysicalDisability { get; set; }
        public bool? MentalHealthDiagnosis { get; set; }
        public bool? MentalHealthSupport { get; set; }
        public bool? SensitiveMentalHealth { get; set; }
        public bool? MedicalEmegencyInYear { get; set; }
        public string? Affordable { get; set; }
        public bool? CostPrice { get; set; }
        public bool? MonetarySupport { get; set; }
        public string? MonetarySupportPlace { get; set; }
        public DateTime? NeedsMedicalCare { get; set; }
        public Guid? MedicalCostForMonth { get; set; }
    }
}
