using MessagePack;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace EDCS_TG.API.Data.Models
{
    public class Survey:BaseEntity
    {
           
         public string? SurveyId { get; set; }
        [ForeignKey("User")] public Guid? UserId { get; set; }
        public string? AdditionalComment { get; set; }
        [ForeignKey("Questions")]  public int? QuestionId { get; set; }
        public  string? Answer { get; set; }

        public int QuestionPaperId { get; set; }

    }
}
