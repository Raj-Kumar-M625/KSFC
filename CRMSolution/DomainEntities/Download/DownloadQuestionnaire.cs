using System;
using DomainEntities.Questionnaire;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DownloadQuestionnaire
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string EntityType { get; set; }
        public long QuestionCount { get; set; }
        public IEnumerable<DownloadQuestionPaperQuestion> Questions { get; set; }
    }

    public class DownloadQuestionPaperQuestion
    {
        public long Id { get; set; }
        public long QuestionPaperId { get; set; }
        public string QuestionTypeName { get; set; }
        public string QText { get; set; }
        public int DisplaySequence { get; set; }
        public bool AdditionalComment { get; set; }
        public IEnumerable<DownloadQuestionPaperAnswer> AnswerChoices { get; set; }
    }

    public class DownloadQuestionPaperAnswer
    {
        public long Id { get; set; }
        public string AText { get; set; }
    }
}
