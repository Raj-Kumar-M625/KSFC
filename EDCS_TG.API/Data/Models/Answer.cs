using EDCS_TG.API.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRS.API.Data.Models
{
    public class Answer : BaseEntity
    {
        [ForeignKey("Survey")] public int? SurveyId { get; set; }
        public Survey? Survey { get; set; }
        [ForeignKey("QuestionPaper")] public int? QuestionPaperId { get; set; }
        [ForeignKey("QuestionPaperQuestion")] public int? QuestionPaperQuestionId { get; set; }
        public string? AnswerText { get; set; }
        public string? AdditionalComment { get; set; }
    }
}
