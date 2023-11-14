using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class IdmFileUploadDto
    {
        public int SubModuleId { get; set; }
        public string SubModuleType { get; set; }

        public string MainModule { get; set; }
        public byte[] Bytes { get; set; }
        public IFormFile File { get; set; }
        public string FileType { get; set; }
    }
}
