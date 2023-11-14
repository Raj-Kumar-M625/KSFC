using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteQuestionnaire : SqliteBase
    {
        public string Id { get; set; }
        public bool IsNewEntity { get; set; }        
        public string EntityId { get; set; }

        public string EntityName { get; set; }        
        public DateTime Date { get; set; }        
        public string QuestionPaperId { get; set; }
        public string Name { get; set; }        
        public string ActivityId { get; set; }        
        public string ParentReferenceId { get; set; }        
        public DateTime TimeStamp { get; set; }        

        public IEnumerable<SqliteAnswerData> Answers { get; set; }
    }
}