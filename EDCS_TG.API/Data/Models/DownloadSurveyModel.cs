using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDCS_TG.API.Data.Models
{
    public class DownloadSurveyModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int? Id { get; set; }

        public string? SurveyorId { get; set; }

        public string? SurveyorName { get; set; }

        public string? SurveyId { get; set; }

        public string? Name { get; set; }

        public string? SubeCategoryName { get; set; }

        public string? QText { get; set; }

        public string? Answer { get; set; }

        public string? CreatedDate { get; set; }

        public string Status { get; set; }
    
    }
}
