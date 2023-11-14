using System.ComponentModel.DataAnnotations.Schema;

namespace EDCS_TG.API.Data.Models
{
    public class DBTLocalEKYCData : BaseEntity
    {
        [ForeignKey("DBTEKYCResponse")]
        public int DBTEKYCResponseId { get; set; }
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
}
