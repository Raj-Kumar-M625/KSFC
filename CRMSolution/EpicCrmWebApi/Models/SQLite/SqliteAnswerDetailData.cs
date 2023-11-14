using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteAnswerDetailData
    {
        public string Id { get; set; }
        public string CrossRefId { get; set; }
        public string AnswerId { get; set; }
        public string SqliteQuestionPaperQuestionId { get; set; }
        public string SqliteQuestionPaperAnswerId { get; set; }
        public string IsAnswerChecked { get; set; }
    }
}