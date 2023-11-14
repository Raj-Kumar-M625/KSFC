using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class DocumentUploadDTO
    {
        public string UploadType { get; set; }
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public string OperationType { get; set; }
    }
}
