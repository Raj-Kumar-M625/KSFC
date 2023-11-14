using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails
{
    public class FileDetailsDTO
    {
        public byte[] Bytes { get; set; }
        public string FileName { get; set; }
        public string DocumentType { get; set; }
    }
}
