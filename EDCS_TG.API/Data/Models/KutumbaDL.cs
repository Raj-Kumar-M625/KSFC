using System.ComponentModel.DataAnnotations;

namespace EDCS_TG.API.Data.Models
{
    public class KutumbaDL
    {
        [Key] public int Id { get; set; }
        public string? FAMILY_ID { get; set; }
        public string? MEMBER_ID { get; set; }
        public string? RC_NUMBER { get; set; }
        public string? MEMBER_NAME_ENG { get; set; }
        public string? MBR_HASH_AADHAR { get; set; }
        public string? MBR_DOB { get; set; }
        public string? MBR_GENDER { get; set; }
        public string? HasedResultValue { get; set; }
    }
}
