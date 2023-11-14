namespace EDCS_TG.API.Data.Models
{
    public class SurveyImages:BaseEntity
    {
        public int BasicSurveyId { get; set; }
        public Guid userId { get; set; }
        public string SurveyId { get; set; }
        public string Image { get; set; }
       
    }
}
