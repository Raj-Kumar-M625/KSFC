using System.ComponentModel.DataAnnotations;

namespace EDCS_TG.API.Data.Models
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Created_Date = DateTime.Now;
            Updated_Date = DateTime.Now;
        }


        [Key]
        public int Id { get; set; }

        public DateTime? Created_Date { get; set; }

        public Guid? Created_By { get; set; }

        public DateTime? Updated_Date { get; set; }

        public Guid? Updated_By { get; set; }
 
    }
}
