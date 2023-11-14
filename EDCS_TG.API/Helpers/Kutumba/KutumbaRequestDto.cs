namespace EDCS_TG.API.Helpers.Kutumba
{
    public class KutumbaRequestDto
    {
        public string? DepID { get; set; }
        public string? BenID { get; set; }
        public string? RC_Number { get; set; }
        public string? Aadhar_No { get; set; }
        public string? ClientCode { get; set; }
        public string? HashedMac { get; set; }
        public string? APIVersion { get; set; } = "1.0";
        public string? IsPhotoRequired { get; set; } = "0";
        public string? MemberId { get; set; }
        public string? Mobile_No { get; set; }
        public string? Request_ID { get; set; } = "0123456789";
        public string? UIDType { get; set; } = "1";
    }
}
