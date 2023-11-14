using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteAnswerData
    {
        public string Id { get; set; }
        public string CrossRefId { get; set; }
        public string QuestionPaperQuestionId { get; set; }
        public string QuestionTypeName { get; set; }
        public string DescriptiveAnswer { get; set; }
        public bool AdditionalComment { get; set; }
        public string Comments { get; set; }

        public List<SqliteAnswerDetailData> AnswerChoices { get; set; }
    }
}