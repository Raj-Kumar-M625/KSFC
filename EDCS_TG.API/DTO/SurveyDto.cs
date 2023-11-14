using System.ComponentModel.DataAnnotations.Schema;

namespace EDCS_TG.API.DTO
{
    public class SurveyDto
    {


         public string? SurveyId { get; set; }
       public Guid? UserId { get; set; }
        public string? AdditionalComment { get; set; }
      public int? QuestionId { get; set; }
        public string? Answer { get; set; }

        public int QuestionPaperId { get; set; }
    }
}
