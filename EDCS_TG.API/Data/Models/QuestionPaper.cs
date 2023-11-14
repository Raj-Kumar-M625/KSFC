using EDCS_TG.API.Data.Models;

namespace EDCS_TG.API.Data.Models
{
    public class QuestionPaper : BaseEntity
    {
        public string? Name { get; set; }
        public string? EntityType { get; set; }
        public string? Locale { get; set; }
        public int QuestionCount { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<QuestionPaperQuestion> QuestionPaperQuestions { get; set; }
    }
}
