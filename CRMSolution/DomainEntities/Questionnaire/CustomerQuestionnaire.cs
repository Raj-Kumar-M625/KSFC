using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Questionnaire
{

    public class CustomerQuestionnaire
    {
        public string CustomerCode { get; set; }
        public long QuestionPaperId { get; set; }
        public string QuestionPaperName { get; set; }
        public long ActivityId { get; set; }

        
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActiveInSap { get; set; }
        public long EmployeeId { get; set; }
        public long Id { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EntityType { get; set; }
        public string EntityName { get; set; }
        public bool IsActive { get; set; }
        public string HQCode { get; set; }
        public long SqliteQuestionnaireId { get; set; }
    }

    public class CustomerQuestionnairedetails : CustomerQuestionnaire
    {

        public string QText { get; set; }
        public string AText { get; set; }
        public string TextComment { get; set; }
        public bool HasTextComment { get; set; }
        public long QuestionPaperQuestionId { get; set; }
        public int DisplaySequence { get; set; }
        public long QuestionPaperAnswerId { get; set; }
        public string QuestionTypeName { get; set; }              
        public IEnumerable<QuestionPaperQuestion> Questions { get; set; }
        public IEnumerable<QuestionPaperAnswer> Answers { get; set; }
    }
}
