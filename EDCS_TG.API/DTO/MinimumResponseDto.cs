using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace EDCS_TG.API.DTO
{
    public class MinimumResponseDto
    {
        public DateTime DateTimeUtc { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string Content { get; set; }

        public bool EraseData { get; set; }
        public bool EnableLogging { get; set; }

        public MinimumResponseDto()
        {
            DateTimeUtc = DateTime.UtcNow;
            Content = "";
            StatusCode = HttpStatusCode.BadRequest;
            EraseData = false;
            EnableLogging = false;
        }
    }
}
