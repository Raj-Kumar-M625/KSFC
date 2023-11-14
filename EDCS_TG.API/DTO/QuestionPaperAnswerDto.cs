using System.ComponentModel.DataAnnotations.Schema;

namespace EDCS_TG.API.DTO
{
    public class QuestionPaperAnswerDto
    {
        public int Id { get; set; }
        //public QuestionPaperQuestionDto? QuestionPaperQuestion { get; set; }
        public int QuestionPaperQuestionId { get; set; }
        public string? AText { get; set; }
    }
}
