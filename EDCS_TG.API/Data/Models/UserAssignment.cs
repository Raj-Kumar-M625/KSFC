namespace EDCS_TG.API.Data.Models
{
    public class UserAssignment:BaseEntity
    { 
        public Guid UserId { get; set; }
        public string? Hobli { get; set; }
        public string? District { get; set; }
        public string? Taluk { get; set; }
        public string? Village { get; set; }
    }
}
