namespace EDCS_TG.API.Data.Models
{
    public class SearchFilter
    {
        public string? District { get; set; }
        public string? Taluk { get; set; }
        public string? Hobli { get; set; }
        public string? Village { get; set; }
        public string? SurveyId { get; set; }
        public string? SurveyeeName { get; set; }
        public string? SurveyorName { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
