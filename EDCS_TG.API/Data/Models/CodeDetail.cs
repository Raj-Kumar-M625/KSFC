namespace EDCS_TG.API.Data.Models
{
    public class CodeDetail : BaseEntity
    {
        public int CodeMasterId { get; set; }

        public string CodeName { get; set; }

        public string CodeValue { get; set; }

        public int DisplaySequence { get; set; }

        public bool? IsActive { get; set; }

       
    }
}
