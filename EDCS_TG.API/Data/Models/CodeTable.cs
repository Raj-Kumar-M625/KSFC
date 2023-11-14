namespace EDCS_TG.API.Data.Models
{
    public class CodeTable: BaseEntity
    {
        public string? CodeType { get; set; }
        public string? CodeName { get; set; }
        public string? CodeValue { get; set; }
        public string? Locale { get; set; }
        public int? DisplaySequence { get; set; }
        public bool? IsActive { get; set; }



    }
}
