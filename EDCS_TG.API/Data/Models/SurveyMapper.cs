using System.ComponentModel.DataAnnotations;
namespace EDCS_TG.API.Data.Models
{
    public class SurveyMapper:BaseEntity
    {

        public string? surveyId { get; set; }
        public int? status { get; set; }
        public int? CategoryId { get; set; }
    }
}
