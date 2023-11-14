using EDCS_TG.API.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDCS_TG.API.Data.Models
{
    public class QuestionPaperAnswer : BaseEntity
    {
        [ForeignKey("QuestionPaperQuestion")] public int QuestionPaperQuestionId { get; set; }
        public QuestionPaperQuestion QuestionPaperQuestion { get; set; }
        public string? AText { get; set; }
    }
}
