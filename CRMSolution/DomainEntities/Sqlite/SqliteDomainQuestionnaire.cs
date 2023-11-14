using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainQuestionnaire
    {
        public string PhoneDbId { get; set; }
        public bool IsNewEntity { get; set; }
        public string EntityName { get; set; }
        public DateTime QuestionnaireDate { get; set; }
        public string SqliteQuestionPaperName { get; set; }
        public long EntityId { get; set; }
        public long SqliteQuestionPaperId { get; set; }
        public string ActivityId { get; set; }
        public string ParentReferenceId { get; set; }
        //public long CustomerQuestionnaireId { get; set; }
        public DateTime DateCreated { get; set; }

        public IEnumerable<SqliteDomainAnswer> Answers { get; set; }
    }

    public class SqliteDomainQuestionnaireData : SqliteDomainQuestionnaire
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }
        public bool IsProcessed { get; set; }
        public string CustomerCode { get; set; }
        public System.DateTime DateUpdated { get; set; }

    }

}
