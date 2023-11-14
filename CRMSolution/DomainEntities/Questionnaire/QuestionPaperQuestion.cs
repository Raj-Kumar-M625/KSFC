using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities.Questionnaire
{
    public class QuestionPaperQuestion
    {
        public long Id { get; set; }
        public long QuestionPaperId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDesc { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryDesc { get; set; }
        public string QuestionTypeName { get; set; }
        public string QText { get; set; }
        public int DisplaySequence { get; set; }
        public bool AdditionalComment { get; set; }
        public IEnumerable<QuestionPaperAnswer> AnswerChoices { get; set; }
    }
}
