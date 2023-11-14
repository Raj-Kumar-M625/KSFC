namespace EDCS_TG.API.DTO.DBT
{
    public class EKYCData
    {
        public string? dob { get; set; }
        public string? gender { get; set; }
        public string? name { get; set; }
        public string? co { get; set; }
        public string? country { get; set; }
        public string? dist { get; set; }
        public string? house { get; set; }
        public string? street { get; set; }
        public string? lm { get; set; }
        public string? loc { get; set; }
        public string? pc { get; set; }
        public string? po { get; set; }
        public string? state { get; set; }
        public string? subdist { get; set; }
        public string? vtc { get; set; }
        public string? lang { get; set; }

    }
    public class LocalKYCData
    {
        public string? dob { get; set; }
        public string? gender { get; set; }
        public string? name { get; set; }
        public string? co { get; set; }
        public string? country { get; set; }
        public string? dist { get; set; }
        public string? house { get; set; }
        public string? street { get; set; }
        public string? lm { get; set; }
        public string? loc { get; set; }
        public string? pc { get; set; }
        public string? po { get; set; }
        public string? state { get; set; }
        public string? subdist { get; set; }
        public string? vtc { get; set; }
        public string? lang { get; set; }
    }
    public class KYCResponse
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
        public EKYCData? eKYCData { get; set; }
        public LocalKYCData? localKYCData { get; set; }
        public string? Photo { get; set; }
    }
}
