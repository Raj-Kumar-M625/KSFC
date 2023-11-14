using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteQuestionnaireDisplayData
    {
        [Display(Name = "Questionnaire Id")]
        public long Id { get; set; }
        public long BatchId { get; set; }
        
        [Display(Name = "Employee Id")]
        public long EmployeeId { get; set; }
        
        [Display(Name = "Customer Code")]
        public string CustomerCode { get; set; }
        
        [Display(Name = "Customer Name")]
        public string EntityName { get; set; }
        
        [Display(Name = "Questionnaire Name")]
        public string SqliteQuestionPaperName { get; set; }
        
        [Display(Name = "Phone Activity Id")]
        public string ActivityId { get; set; }
        [Display(Name = "Phone Db Id")]
        public string PhoneDbId { get; set; }
        public bool IsProcessed { get; set; }

        [Display(Name = "Date")]
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
    }
}