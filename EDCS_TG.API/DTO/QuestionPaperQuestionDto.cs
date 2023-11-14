using System.ComponentModel.DataAnnotations.Schema;

namespace EDCS_TG.API.DTO
{
    public class QuestionPaperQuestionDto
    {
        public int Id { get; set; }
        //public QuestionPaperDto? QuestionPaper { get; set; }
        public int QuestionPaperId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryDesc { get; set; }
        public string? SubCategoryName { get; set; }
        public string? SubCategoryDesc { get; set; }
        public string? QuestionTypeName { get; set; }
        public string? QText { get; set; }
        public bool AdditionalComment { get; set; }
        public int DisplaySequence { get; set; }

        public bool IsMandatory { get; set; }
        public ICollection<QuestionPaperAnswerDto>? QuestionPaperAnswers { get; set; }
    }
}
