using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Questionnaire
{
    public class QuestionPaper
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string EntityType { get; set; }
        public long QuestionCount { get; set; }
        public long BidsAssignedCount { get; set; }

        public IEnumerable<QuestionPaperQuestion> Questions { get; set; }
    }
}
