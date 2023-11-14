
using System.ComponentModel.DataAnnotations.Schema;

namespace EDCS_TG.API.DTO { 
    public class QuestionPaperDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? EntityType { get; set; }
        public string? Locale { get; set; }
        public int QuestionCount { get; set; }
        public bool IsActive { get; set; }
        public ICollection<QuestionPaperQuestionDto>? QuestionPaperQuestions { get; set; }
    }
}
