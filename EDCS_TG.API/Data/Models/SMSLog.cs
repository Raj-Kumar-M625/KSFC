using System.ComponentModel.DataAnnotations;

namespace EDCS_TG.API.Data.Models
{
    public class SMSLog
    {
        [Key] public int Id { get; set; }
        public string? SmsId { get; set; }
        public string? SmsText { get; set; }
        public string? ApiResponse { get; set; }
        public string? ApiStatus { get; set; }
        public DateTime? SmsDateTime { get; set; }
        public bool? HasCompleted { get; set; }
        public bool HasFailed { get; set; }
    }
}
