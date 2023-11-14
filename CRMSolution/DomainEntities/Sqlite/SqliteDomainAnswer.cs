using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainAnswer
    {
        public long Id { get; set; }
        public long CrossRefId { get; set; }
        public long QuestionPaperQuestionId { get; set; }
        public string QuestionTypeName { get; set; }
        public string DescriptiveAnswer { get; set; }
        public bool HasTextComment { get; set; }
        public string TextComment { get; set; }

        public List<SqliteDomainAnswerDetail> DomainAnswerDetail { get; set; }
    }
}
