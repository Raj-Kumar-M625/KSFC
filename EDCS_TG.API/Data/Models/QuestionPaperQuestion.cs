using EDCS_TG.API.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDCS_TG.API.Data.Models
{
    public class QuestionPaperQuestion : BaseEntity
    {
        [ForeignKey("QuestionPaper")] public int QuestionPaperId { get; set; }
        public QuestionPaper QuestionPaper { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryDesc { get; set; }
        public string? SubCategoryName { get; set; }
        public string? SubCategoryDesc { get; set; }
        public string? QuestionTypeName { get; set; }
        public string? QText { get; set; }
        public bool AdditionalComment { get; set; }
        public int DisplaySequence { get; set; }

        public bool IsMandatory { get; set; }
        public virtual ICollection<QuestionPaperAnswer> QuestionPaperAnswers { get; set; }
    }
}
