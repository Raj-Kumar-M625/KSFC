namespace EDCS_TG.API.Data.Models
{
    public class Questions:BaseEntity
    {
       
        public string CategoryName { get; set; }
        public string CategoryDesc { get; set; }
        public string SubCategoryName { get; set; }
        public string QuestionReferenceNumber { get; set; }
        public string  SubCategoryDesc { get; set; }
        public int GroupId { get; set; }
        public string Locale { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public int DisplaySequence { get; set; }
        public string HasAdditionalComment { get; set; }
      
    }
}
