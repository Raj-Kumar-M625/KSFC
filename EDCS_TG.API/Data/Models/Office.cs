namespace EDCS_TG.API.Data.Models
{
    public class Office : BaseEntity
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? StateCode { get; set; }
        public string? StateName { get; set; }
        public string? DistrictCode { get; set; }
        public string? DistrictName { get; set; }
        public string? DistrictName_KA { get; set; }
        public string? TalukOrTownCode { get; set; }
        public string? TalukOrTownName { get; set; }
        public string? TalukOrTownName_KA { get; set; }
        public string? HobliOrZoneCode { get; set; }
        public string? HobliOrZoneName { get; set; }
        public string? HobliOrZoneName_KA { get; set; }
        public string? VillageOrWardCode { get; set; }
        public string? VillageOrWardName { get; set; }
        public string? VillageOrWardName_KA { get; set; }
    }
}
