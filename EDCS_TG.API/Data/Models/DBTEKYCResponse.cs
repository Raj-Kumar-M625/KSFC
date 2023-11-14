namespace EDCS_TG.API.Data.Models
{
    public class DBTEKYCResponse : BaseEntity
    {
        public string? TxnNo { get; set; }
        public string? TxnDateTime { get; set; }
        public string? AadhaarHash { get; set; }
        public string? FinalStatus { get; set; }
        public string? VaultRefNumber { get; set; }
        public string? EKYCTxnNo { get; set; }
        public string? EKYCTimestamp { get; set; }
        public string? ResidentConsent { get; set; }
        public string? Status { get; set; }
        public string? ResponseStatus { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Error { get; set; }
        public string? UIDToken { get; set; }
        public string? ActionCode { get; set; }
        public string? OTP { get; set; }
        public string? OTPTxnNo { get; set; }
        public string? OTPTimeStamp { get; set; }
        public string? Photo { get; set; }
    }
}
