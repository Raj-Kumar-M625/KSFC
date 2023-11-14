using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EpicCrmWebApi
{ 
    public class CustomerQuestionnaireModel
    {
        [Display(Name = "Customer Code")]
        public string CustomerCode { get; set; }
        public long QuestionPaperId { get; set; }

        public string QuestionPaperName { get; set; }

        public long ActivityId { get; set; }

        [Display(Name = "Date")]
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public long Id { get; set; }

        public long EmployeeId { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Client Type")]
        public string EntityType { get; set; }

        [Display(Name = "Client Name")]
        public string EntityName { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }


        public string QText { get; set; }

        public string AText { get; set; }
        public string TextComment { get; set; }
        public bool HasTextComment { get; set; }

        public bool IsSelected { get; set; }

        public string QuestionTypeName { get; set; }

        public long QuestionPaperQuestionId { get; set; }

        public long QuestionPaperAnswerId { get; set; }

      



    }
}