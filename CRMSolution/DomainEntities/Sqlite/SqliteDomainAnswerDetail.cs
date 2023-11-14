using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainAnswerDetail
    {
        public long Id { get; set; }
        public long AnswerId { get; set; }
        public long SqliteQuestionPaperQuestionId { get; set; }
        public long SqliteQuestionPaperAnswerId { get; set; }
        public string IsAnswerChecked { get; set; }
    }
}
