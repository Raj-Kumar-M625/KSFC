namespace EDCS_TG.API.Data.Models
{
    public class CodeMaster : BaseEntity
    {
        public string CodeType { get; set; }

        public string Locale { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? IsActive { get; set; }

        public string Description { get; set; }

      
    }
}
