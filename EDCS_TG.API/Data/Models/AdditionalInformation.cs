using System.ComponentModel.DataAnnotations.Schema;

namespace EDCS_TG.API.Data.Models
{
    public class AdditionalInformation:BaseEntity
    {
        [ForeignKey("PersonalDetails")] public int PersonalDetailsId { get; set; }
        public bool? FamilyAccept { get; set; }
        public bool? Contact { get; set; }
        public bool? WorkingTransgender { get; set; }
        public string? OrganizationName { get; set; }
        public string? CulturalFlair { get; set; }
        public string? CulturalFlairOthers { get; set; }
        public string? SexWorkProfession { get; set; }
        public bool? ManktiProfession { get; set; }
        public bool? KPSandotherdepartment { get; set; }
        public bool? CitizensSupport { get; set; }
        public string? IfOthers { get; set; }
    }
}
